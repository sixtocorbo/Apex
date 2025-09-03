Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Data.Entity
Imports System.IO
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Text

Public Class frmFuncionarioCrear
    Inherits Form

#Region " Definiciones y Variables "

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    Private Class EstadoRow
        Public Property Id As Integer
        Public Property Origen As String
        Public Property TipoEstado As String
        Public Property FechaDesde As Date?
        Public Property FechaHasta As Date?
        Public Property Observaciones As String
        Public Property EntityRef As EstadoTransitorio
    End Class

    Private ReadOnly _uow As UnitOfWork
    Private _funcionario As Funcionario
    Private ReadOnly _svc As FuncionarioService
    Private ReadOnly _modo As ModoFormulario
    Private ReadOnly _idFuncionario As Integer
    Private _rutaFotoSeleccionada As String
    Private _tiposEstadoTransitorio As List(Of TipoEstadoTransitorio)
    Private _itemsDotacion As List(Of DotacionItem)
    Private _dotaciones As BindingList(Of FuncionarioDotacion)
    Private _estadoRows As BindingList(Of EstadoRow)
    Private _estadosParaEliminar As New List(Of EstadoTransitorio)
    Private ReadOnly bsDotacion As New BindingSource()
    Private ReadOnly bsEstados As New BindingSource()
    Private _seGuardo As Boolean = False
    Private _cerrandoPorCodigo As Boolean = False

#End Region

#Region " Constructores y Carga del Formulario "

    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _uow = New UnitOfWork()
        _svc = New FuncionarioService(_uow)
        _funcionario = New Funcionario() With {.FechaIngreso = DateTime.Now}
        _estadoRows = New BindingList(Of EstadoRow)()
    End Sub

    Public Sub New(id As Integer)
        InitializeComponent()
        _modo = ModoFormulario.Editar
        _idFuncionario = id
        _uow = New UnitOfWork()
        _svc = New FuncionarioService(_uow)
        _funcionario = _uow.Context.Set(Of Funcionario)().
            Include(Function(f) f.FuncionarioDotacion.Select(Function(fd) fd.DotacionItem)).
            Include(Function(f) f.EstadoTransitorio.Select(Function(et) et.TipoEstadoTransitorio)).
            FirstOrDefault(Function(f) f.Id = id)
        If _funcionario?.EstadoTransitorio IsNot Nothing Then
            For Each et In _funcionario.EstadoTransitorio
                LoadEstadoTransitorioDetails(et)
            Next
        End If
    End Sub

    Private Async Sub frmFuncionarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        If _funcionario Is Nothing AndAlso _modo = ModoFormulario.Editar Then
            MessageBox.Show("No se encontró el registro del funcionario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If
        Await CargarCombosAsync()
        _tiposEstadoTransitorio = Await _svc.ObtenerTiposEstadoTransitorioCompletosAsync()
        _itemsDotacion = Await _svc.ObtenerItemsDotacionCompletosAsync()
        _dotaciones = New BindingList(Of FuncionarioDotacion)(_funcionario.FuncionarioDotacion.ToList())
        _estadoRows = If(_modo = ModoFormulario.Editar, MapEstadosActivos(_funcionario.EstadoTransitorio.Where(AddressOf IsEstadoActivo)), New BindingList(Of EstadoRow)())
        ConfigurarGrillas()
        ConfigurarBindingSources()
        SuscribirEventosUI()
        If _modo = ModoFormulario.Editar Then
            Me.Text = "Editar Funcionario"
            btnGuardar.Text = "Actualizar"
            CargarDatosEnControles()
        Else
            Me.Text = "Nuevo Funcionario"
            btnGuardar.Text = "Guardar"
            pbFoto.Image = My.Resources.Police
        End If
    End Sub
    Private Sub CargarDatosEnControles()
        If _funcionario Is Nothing Then Return

        ' Texto
        txtCI.Text = If(_funcionario.CI, String.Empty).Trim()
        txtNombre.Text = If(_funcionario.Nombre, String.Empty).Trim()
        txtDomicilio.Text = If(_funcionario.Domicilio, String.Empty).Trim()
        txtEmail.Text = If(_funcionario.Email, String.Empty).Trim()
        txtTelefono.Text = If(_funcionario.Telefono, String.Empty).Trim()
        txtCiudad.Text = If(_funcionario.Ciudad, String.Empty).Trim()
        txtSeccional.Text = If(_funcionario.Seccional, String.Empty).Trim()
        txtCredencial.Text = If(_funcionario.Credencial, String.Empty).Trim()

        ' Checkboxes
        chkActivo.Checked = _funcionario.Activo
        chkEstudia.Checked = _funcionario.Estudia

        ' Fechas (defensivo con rango del DateTimePicker)
        dtpFechaIngreso.Value = SafePickerDate(dtpFechaIngreso, _funcionario.FechaIngreso)
        dtpFechaNacimiento.Value = SafePickerDate(dtpFechaNacimiento, _funcionario.FechaNacimiento)

        ' Combos (usa helpers que dejan -1 si el valor no existe en el origen de datos)
        SetSelectedOrNone(cboTipoFuncionario, _funcionario.TipoFuncionarioId)               ' no-nullable
        SetSelectedOrNone(cboCargo, _funcionario.CargoId)                                   ' nullable
        SetSelectedOrNone(cboEscalafon, _funcionario.EscalafonId)
        SetSelectedOrNone(cboFuncion, _funcionario.FuncionId)
        SetSelectedOrNone(cboEstadoCivil, _funcionario.EstadoCivilId)
        SetSelectedOrNone(cboGenero, _funcionario.GeneroId)
        SetSelectedOrNone(cboNivelEstudio, _funcionario.NivelEstudioId)
        SetSelectedOrNone(cboEstado, _funcionario.EstadoId)
        SetSelectedOrNone(cboSeccion, _funcionario.SeccionId)
        SetSelectedOrNone(cboPuestoTrabajo, _funcionario.PuestoTrabajoId)
        SetSelectedOrNone(cboTurno, _funcionario.TurnoId)
        SetSelectedOrNone(cboSemana, _funcionario.SemanaId)
        SetSelectedOrNone(cboHorario, _funcionario.HorarioId)

        ' Foto
        If _funcionario.Foto IsNot Nothing AndAlso _funcionario.Foto.Length > 0 Then
            Using ms As New MemoryStream(_funcionario.Foto)
                If pbFoto.Image IsNot Nothing Then pbFoto.Image.Dispose()
                pbFoto.Image = New Bitmap(ms)
            End Using
        Else
            pbFoto.Image = My.Resources.Police
        End If
    End Sub

    ' ---------- Helpers ----------

    ' Asegura que el DateTime esté dentro del rango del control; si no hay valor, usa Today o MinDate como fallback.
    Private Function SafePickerDate(picker As DateTimePicker, value As Date?) As Date
        Dim d As Date = If(value.HasValue, value.Value, Date.Today)
        If d < picker.MinDate Then d = picker.MinDate
        If d > picker.MaxDate Then d = picker.MaxDate
        Return d
    End Function

    ' Sobrecarga para IDs no anulables (ej. TipoFuncionarioId)
    Private Sub SetSelectedOrNone(cbo As ComboBox, value As Integer)
        If cbo Is Nothing Then Return
        Try
            cbo.SelectedValue = value
            Dim ok As Boolean = (cbo.SelectedValue IsNot Nothing AndAlso Convert.ToInt32(cbo.SelectedValue) = value)
            If Not ok Then cbo.SelectedIndex = -1
        Catch
            cbo.SelectedIndex = -1
        End Try
    End Sub

    ' Sobrecarga para IDs anulables (la mayoría de los combos)
    Private Sub SetSelectedOrNone(cbo As ComboBox, value As Integer?)
        If cbo Is Nothing Then Return
        If Not value.HasValue Then
            cbo.SelectedIndex = -1
            Return
        End If
        Try
            cbo.SelectedValue = value.Value
            Dim ok As Boolean = (cbo.SelectedValue IsNot Nothing AndAlso Convert.ToInt32(cbo.SelectedValue) = value.Value)
            If Not ok Then cbo.SelectedIndex = -1
        Catch
            cbo.SelectedIndex = -1
        End Try
    End Sub

#End Region

#Region " Lógica de Carga y Mapeo de Datos "

    Private Sub LoadEstadoTransitorioDetails(et As EstadoTransitorio)
        If et Is Nothing Then Exit Sub

        ' Asegurar que el entity esté adjunto (por si vino con AsNoTracking)
        Dim entry = _uow.Context.Entry(et)
        If entry.State = EntityState.Detached Then
            _uow.Context.Set(Of EstadoTransitorio)().Attach(et)
            entry = _uow.Context.Entry(et)
        End If

        Select Case et.TipoEstadoTransitorioId
            Case TiposEstadoCatalog.Designacion
                LoadRef(et, Function(x) x.DesignacionDetalle)

            Case TiposEstadoCatalog.Enfermedad
                LoadRef(et, Function(x) x.EnfermedadDetalle)

            Case TiposEstadoCatalog.Sancion
                LoadRef(et, Function(x) x.SancionDetalle)

            Case TiposEstadoCatalog.OrdenCinco
                LoadRef(et, Function(x) x.OrdenCincoDetalle)

            Case TiposEstadoCatalog.Reten
                LoadRef(et, Function(x) x.RetenDetalle)

            Case TiposEstadoCatalog.Sumario
                LoadRef(et, Function(x) x.SumarioDetalle)

            Case TiposEstadoCatalog.Traslado
                LoadRef(et, Function(x) x.TrasladoDetalle)

            Case TiposEstadoCatalog.BajaDeFuncionario
                LoadRef(et, Function(x) x.BajaDeFuncionarioDetalle)

            Case TiposEstadoCatalog.CambioDeCargo
                LoadRef(et, Function(x) x.CambioDeCargoDetalle)
                ' Cargar navs Cargo / Cargo1 para tener nombres
                Dim d = et.CambioDeCargoDetalle
                If d IsNot Nothing Then
                    Dim de = _uow.Context.Entry(d)
                    Dim rCargo = de.Reference(Function(x) x.Cargo)
                    If Not rCargo.IsLoaded Then rCargo.Load()
                    Dim rCargo1 = de.Reference(Function(x) x.Cargo1)
                    If Not rCargo1.IsLoaded Then rCargo1.Load()
                End If

            Case TiposEstadoCatalog.ReactivacionDeFuncionario
                LoadRef(et, Function(x) x.ReactivacionDeFuncionarioDetalle)

            Case TiposEstadoCatalog.SeparacionDelCargo
                LoadRef(et, Function(x) x.SeparacionDelCargoDetalle)

            Case TiposEstadoCatalog.InicioDeProcesamiento
                LoadRef(et, Function(x) x.InicioDeProcesamientoDetalle)

            Case TiposEstadoCatalog.Desarmado
                LoadRef(et, Function(x) x.DesarmadoDetalle)

            Case Else
                ' Logger.Warn($"TipoEstadoTransitorioId desconocido: {et.TipoEstadoTransitorioId}")
        End Select
    End Sub

    Private Sub LoadRef(Of TProp As Class)(
        et As EstadoTransitorio,
        selector As Expression(Of Func(Of EstadoTransitorio, TProp))
    )
        Dim r = _uow.Context.Entry(et).Reference(selector)
        If Not r.IsLoaded Then r.Load()
    End Sub

    Private Function BuildEstadoRow(e As EstadoTransitorio, origen As String) As EstadoRow
        If e Is Nothing Then Return Nothing

        ' Nombre del tipo (defensivo)
        Dim tipoNombre As String = String.Empty
        Dim tipo = _tiposEstadoTransitorio?.FirstOrDefault(Function(t) t.Id = e.TipoEstadoTransitorioId)
        If tipo IsNot Nothing Then tipoNombre = tipo.Nombre

        ' Fechas centralizadas
        Dim fd As Date? = Nothing, fh As Date? = Nothing
        e.GetFechas(fd, fh)

        Dim obs As String = String.Empty
        Dim parts As New List(Of String)()

        Dim addPart As Action(Of String, String) =
            Sub(label As String, value As String)
                If Not String.IsNullOrWhiteSpace(value) Then
                    parts.Add($"{label}: {value.Trim()}")
                End If
            End Sub

        Dim addDate As Action(Of String, Date?) =
            Sub(label As String, d As Date?)
                If d.HasValue Then parts.Add($"{label}: {d.Value:dd/MM/yyyy}")
            End Sub

        Select Case e.TipoEstadoTransitorioId
            Case TiposEstadoCatalog.Designacion
                Dim d = e.DesignacionDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    addPart("Resolución", d.DocResolucion)
                    addDate("Fecha res.", d.FechaResolucion)
                End If

            Case TiposEstadoCatalog.Enfermedad
                Dim d = e.EnfermedadDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    addPart("Diagnóstico", d.Diagnostico)
                End If

            Case TiposEstadoCatalog.Sancion
                Dim d = e.SancionDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    addPart("Resolución", d.Resolucion)
                    addPart("Tipo", d.TipoSancion)
                End If

            Case TiposEstadoCatalog.OrdenCinco
                Dim d = e.OrdenCincoDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                End If

            Case TiposEstadoCatalog.Reten
                Dim d = e.RetenDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    addDate("Fecha retén", d.FechaReten)
                    addPart("Turno", d.Turno)
                    addPart("Asignado por", d.AsignadoPor)
                End If

            Case TiposEstadoCatalog.Sumario
                Dim d = e.SumarioDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    addPart("Expediente", d.Expediente)
                End If

            Case TiposEstadoCatalog.Traslado
                Dim d = e.TrasladoDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                End If

            Case TiposEstadoCatalog.BajaDeFuncionario
                Dim d = e.BajaDeFuncionarioDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                End If

            Case TiposEstadoCatalog.CambioDeCargo
                Dim d = e.CambioDeCargoDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    Dim cargoAnt As String = GetCargoNombre(d, True)
                    Dim cargoNvo As String = GetCargoNombre(d, False)
                    addPart("Cargo anterior", cargoAnt)
                    addPart("Cargo nuevo", cargoNvo)
                    addPart("Resolución", d.Resolucion)
                End If

            Case TiposEstadoCatalog.ReactivacionDeFuncionario
                Dim d = e.ReactivacionDeFuncionarioDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    addPart("Resolución", d.Resolucion)
                End If

            Case TiposEstadoCatalog.SeparacionDelCargo
                Dim d = e.SeparacionDelCargoDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                End If

            Case TiposEstadoCatalog.InicioDeProcesamiento
                Dim d = e.InicioDeProcesamientoDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    addPart("Expediente", d.Expediente)
                End If

            Case TiposEstadoCatalog.Desarmado
                Dim d = e.DesarmadoDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                End If

            Case Else
                ' Tipo desconocido: no rompe
        End Select

        Dim det As String = String.Join(" | ", parts)
        Dim obsFinal As String =
            If(String.IsNullOrWhiteSpace(det), (If(obs, String.Empty)).Trim(),
               If(String.IsNullOrWhiteSpace(obs), det, $"{det} | {obs.Trim()}"))

        Return New EstadoRow With {
            .Id = e.Id,
            .Origen = origen,
            .TipoEstado = If(tipoNombre, String.Empty),
            .FechaDesde = fd,
            .FechaHasta = fh,
            .Observaciones = obsFinal,
            .EntityRef = e
        }
    End Function

    Private Function GetCargoNombre(d As CambioDeCargoDetalle, anterior As Boolean) As String
        Dim targetId As Integer? = If(anterior, d.CargoAnteriorId, CType(d.CargoNuevoId, Integer?))
        If Not targetId.HasValue Then Return Nothing

        ' 1) Si las navs están cargadas, usar la que coincida con el FK
        If d.Cargo IsNot Nothing AndAlso d.Cargo.Id = targetId.Value Then
            Return d.Cargo.Nombre
        End If
        If d.Cargo1 IsNot Nothing AndAlso d.Cargo1.Id = targetId.Value Then
            Return d.Cargo1.Nombre
        End If

        ' 2) Fallback: consultar solo ese Id
        Try
            Dim nombre As String = _uow.Repository(Of Cargo)().
                                   GetAll().
                                   Where(Function(x) x.Id = targetId.Value).
                                   Select(Function(x) x.Nombre).
                                   FirstOrDefault()
            If Not String.IsNullOrWhiteSpace(nombre) Then Return nombre
        Catch
            ' silencioso
        End Try

        ' 3) Último recurso
        Return "#" & targetId.Value.ToString()
    End Function

#End Region

#Region " Lógica de Guardado "

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        LoadingHelper.MostrarCargando(Me)
        If Await GuardarAsync() Then
            LoadingHelper.OcultarCargando(Me)
            MessageBox.Show(If(_modo = ModoFormulario.Crear, "Funcionario creado", "Funcionario actualizado") & " correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            NotificadorEventos.NotificarActualizacionFuncionario(_funcionario.Id)
            Me.DialogResult = DialogResult.OK
            _cerrandoPorCodigo = True
            Close()
        Else
            LoadingHelper.OcultarCargando(Me)
        End If
    End Sub

    Private Async Function GuardarAsync() As Task(Of Boolean)
        Try
            If Not ValidarDatos() Then Return False
            MapearControlesAFuncionario()
            If _modo = ModoFormulario.Crear Then
                _uow.Repository(Of Funcionario).Add(_funcionario)
            Else
                _uow.Repository(Of Funcionario).Update(_funcionario)
            End If
            SincronizarEstados()
            Await _uow.CommitAsync()
            _seGuardo = True
            Return True
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar: " & ex.Message & vbCrLf & ex.InnerException?.Message, "Error de Guardado", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Function ValidarDatos() As Boolean
        If String.IsNullOrWhiteSpace(txtCI.Text) OrElse String.IsNullOrWhiteSpace(txtNombre.Text) OrElse cboTipoFuncionario.SelectedIndex = -1 Then
            MessageBox.Show("Los campos CI, Nombre y Tipo de Funcionario son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Private Sub MapearControlesAFuncionario()
        _funcionario.CI = txtCI.Text.Trim()
        _funcionario.Nombre = txtNombre.Text.Trim()
        _funcionario.FechaIngreso = dtpFechaIngreso.Value.Date
        _funcionario.TipoFuncionarioId = CInt(cboTipoFuncionario.SelectedValue)
        _funcionario.CargoId = If(cboCargo.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboCargo.SelectedValue))
        _funcionario.Activo = chkActivo.Checked
        _funcionario.EscalafonId = If(cboEscalafon.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEscalafon.SelectedValue))
        _funcionario.FuncionId = If(cboFuncion.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboFuncion.SelectedValue))
        _funcionario.FechaNacimiento = If(dtpFechaNacimiento.Value = dtpFechaNacimiento.MinDate, CType(Nothing, Date?), dtpFechaNacimiento.Value.Date)
        _funcionario.Domicilio = txtDomicilio.Text.Trim()
        _funcionario.Email = txtEmail.Text.Trim()
        _funcionario.Telefono = txtTelefono.Text.Trim()
        _funcionario.EstadoCivilId = If(cboEstadoCivil.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEstadoCivil.SelectedValue))
        _funcionario.GeneroId = If(cboGenero.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboGenero.SelectedValue))
        _funcionario.NivelEstudioId = If(cboNivelEstudio.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboNivelEstudio.SelectedValue))
        _funcionario.EstadoId = If(cboEstado.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEstado.SelectedValue))
        _funcionario.SeccionId = If(cboSeccion.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboSeccion.SelectedValue))
        _funcionario.PuestoTrabajoId = If(cboPuestoTrabajo.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboPuestoTrabajo.SelectedValue))
        _funcionario.TurnoId = If(cboTurno.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboTurno.SelectedValue))
        _funcionario.SemanaId = If(cboSemana.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboSemana.SelectedValue))
        _funcionario.HorarioId = If(cboHorario.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboHorario.SelectedValue))
        _funcionario.Ciudad = txtCiudad.Text.Trim()
        _funcionario.Seccional = txtSeccional.Text.Trim()
        _funcionario.Credencial = txtCredencial.Text.Trim()
        _funcionario.Estudia = chkEstudia.Checked
        If Not String.IsNullOrWhiteSpace(_rutaFotoSeleccionada) Then _funcionario.Foto = File.ReadAllBytes(_rutaFotoSeleccionada)
    End Sub

    Private Sub SincronizarEstados()
        ' Borrados: ya están marcados como Deleted en el click
        For Each row In _estadoRows
            Dim estado = row.EntityRef
            If estado.Id <= 0 Then
                _funcionario.EstadoTransitorio.Add(estado)
            Else
                estado.Funcionario = _funcionario
                _uow.Context.Entry(estado).State = EntityState.Modified
            End If
        Next
    End Sub

#End Region

#Region " Eventos y Lógica de UI (Grillas, Botones, etc.) "

    Private Sub SuscribirEventosUI()
        AddHandler dgvDotacion.DataError, Sub(s, a) a.ThrowException = False
        AddHandler dgvDotacion.CellFormatting, AddressOf dgvDotacion_CellFormatting
        AddHandler chkVerHistorial.CheckedChanged, AddressOf chkVerHistorial_CheckedChanged
        AddHandler dgvEstadosTransitorios.SelectionChanged, AddressOf DgvEstadosTransitorios_SelectionChanged
        AddHandler dgvEstadosTransitorios.CellDoubleClick, AddressOf DgvEstadosTransitorios_CellDoubleClick
    End Sub

    Private Sub ConfigurarGrillas()
        ConfigurarGrillaDotacion()
        ConfigurarGrillaEstados()
    End Sub

    Private Sub ConfigurarBindingSources()
        bsDotacion.DataSource = _dotaciones
        dgvDotacion.DataSource = bsDotacion
        bsEstados.DataSource = _estadoRows
        dgvEstadosTransitorios.DataSource = bsEstados
    End Sub

    Private Function IsEstadoActivo(et As EstadoTransitorio) As Boolean
        Dim hoy = Date.Today
        Dim fechaDesde As Date? = Nothing, fechaHasta As Date? = Nothing
        et.GetFechas(fechaDesde, fechaHasta)

        ' Normalizar sentinelas (p.ej. 0001/1900) a "sin fin"
        If fechaHasta.HasValue AndAlso fechaHasta.Value.Year < 1902 Then
            fechaHasta = Nothing
        End If

        ' Retén: solo el día exacto (fecha puntual)
        If et.TipoEstadoTransitorioId = TiposEstadoCatalog.Reten Then
            Return fechaDesde.HasValue AndAlso fechaDesde.Value.Date = hoy
        End If

        If Not fechaDesde.HasValue Then Return False
        Return fechaDesde.Value.Date <= hoy AndAlso (Not fechaHasta.HasValue OrElse fechaHasta.Value.Date >= hoy)
    End Function


    Private Sub UpdateRowFromEntity(row As EstadoRow, e As EstadoTransitorio)
        Dim updated = BuildEstadoRow(e, row.Origen)
        row.TipoEstado = updated.TipoEstado : row.FechaDesde = updated.FechaDesde
        row.FechaHasta = updated.FechaHasta : row.Observaciones = updated.Observaciones
    End Sub

    Private _estaCargandoHistorial As Boolean = False
    Private Async Sub chkVerHistorial_CheckedChanged(sender As Object, e As EventArgs) Handles chkVerHistorial.CheckedChanged
        If _estaCargandoHistorial Then Return
        _estaCargandoHistorial = True
        Try
            If chkVerHistorial.Checked Then
                Await CargarHistorialCompleto()
            Else
                _estadoRows = MapEstadosActivos(_funcionario.EstadoTransitorio.Where(AddressOf IsEstadoActivo))
                bsEstados.DataSource = _estadoRows
                bsEstados.ResetBindings(False)
            End If
        Finally
            _estaCargandoHistorial = False
            DgvEstadosTransitorios_SelectionChanged(Nothing, EventArgs.Empty)
        End Try
    End Sub

    Private Async Function CargarHistorialCompleto() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim query = _uow.Context.Set(Of EstadoTransitorio)().
                        Where(Function(et) et.FuncionarioId = _idFuncionario)

            ' Includes completos (evita N+1 al construir filas)
            Dim historial = Await query.
                Include(Function(et) et.TipoEstadoTransitorio).
                Include(Function(et) et.DesignacionDetalle).
                Include(Function(et) et.SancionDetalle).
                Include(Function(et) et.SumarioDetalle).
                Include(Function(et) et.OrdenCincoDetalle).
                Include(Function(et) et.EnfermedadDetalle).
                Include(Function(et) et.RetenDetalle).
                Include(Function(et) et.DesarmadoDetalle).
                Include(Function(et) et.SeparacionDelCargoDetalle).
                Include(Function(et) et.TrasladoDetalle).
                Include(Function(et) et.InicioDeProcesamientoDetalle).
                Include(Function(et) et.CambioDeCargoDetalle).
                Include(Function(et) et.CambioDeCargoDetalle.Cargo).
                Include(Function(et) et.CambioDeCargoDetalle.Cargo1).
                OrderByDescending(Function(et) et.Id).
                ToListAsync()

            _estadoRows = MapEstadosHistorial(historial)
            bsEstados.DataSource = _estadoRows
            bsEstados.ResetBindings(False)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Sub DgvEstadosTransitorios_SelectionChanged(sender As Object, e As EventArgs)
        Dim row = TryCast(dgvEstadosTransitorios.CurrentRow?.DataBoundItem, EstadoRow)
        Dim editable = (row IsNot Nothing)
        btnEditarEstado.Enabled = editable : btnQuitarEstado.Enabled = editable : btnAñadirEstado.Enabled = True
    End Sub

    Private Sub btnAñadirEstado_Click(sender As Object, e As EventArgs) Handles btnAñadirEstado.Click
        ' Vincular el funcionario evita NullReference en el form de estado (Case 30)
        Dim nuevoEstado = New EstadoTransitorio() With {
            .Funcionario = _funcionario
        }

        Using frm As New frmFuncionarioEstadoTransitorio(nuevoEstado, _tiposEstadoTransitorio, _uow)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                _estadoRows.Add(BuildEstadoRow(frm.Estado, "Nuevo"))
                bsEstados.ResetBindings(False)
            End If
        End Using
    End Sub

    Private Sub btnEditarEstado_Click(sender As Object, e As EventArgs) Handles btnEditarEstado.Click
        Dim row = TryCast(dgvEstadosTransitorios.CurrentRow?.DataBoundItem, EstadoRow)
        If row?.EntityRef Is Nothing Then Return
        Using frm As New frmFuncionarioEstadoTransitorio(row.EntityRef, _tiposEstadoTransitorio, _uow)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                UpdateRowFromEntity(row, frm.Estado)
                bsEstados.ResetBindings(False)
            End If
        End Using
    End Sub

    Private Sub btnQuitarEstado_Click(sender As Object, e As EventArgs) Handles btnQuitarEstado.Click
        Dim row = TryCast(dgvEstadosTransitorios.CurrentRow?.DataBoundItem, EstadoRow)
        If row?.EntityRef Is Nothing Then Return

        If MessageBox.Show("¿Está seguro de que desea quitar este estado transitorio?", "Confirmar Eliminación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        Dim entidad = row.EntityRef

        If entidad.Id > 0 Then
            ' Importante: NO romper la relación de navegación.
            ' Marcá la entidad (y su detalle) como Deleted ya, así EF no intenta nullear FKs
            MarcarParaEliminar(entidad)
        Else
            ' Era nuevo/no persistido: simplemente sacarlo del contexto
            If _uow.Context.Entry(entidad).State <> EntityState.Detached Then
                _uow.Context.Entry(entidad).State = EntityState.Detached
            End If
        End If

        ' Actualizar solo UI
        _estadoRows.Remove(row)
        bsEstados.ResetBindings(False)
    End Sub

    Private Sub dgvDotacion_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 OrElse e.ColumnIndex >= dgv.Columns.Count Then Return
        Dim dataItem = TryCast(dgv.Rows(e.RowIndex).DataBoundItem, FuncionarioDotacion)
        If dataItem Is Nothing Then Return
        Dim colName = dgv.Columns(e.ColumnIndex).Name
        If colName = "colItem" Then e.Value = _itemsDotacion?.FirstOrDefault(Function(i) i.Id = dataItem.DotacionItemId)?.Nombre : e.FormattingApplied = True
        If colName = "FechaAsign" AndAlso e.Value IsNot Nothing Then e.Value = CType(e.Value, DateTime).ToString("dd/MM/yyyy") : e.FormattingApplied = True
    End Sub

    Private Sub ConfigurarGrillaDotacion()
        dgvDotacion.AutoGenerateColumns = False : dgvDotacion.Columns.Clear()
        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .Visible = False})
        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colItem", .HeaderText = "Ítem", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Talla", .HeaderText = "Talla"})
        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .HeaderText = "Observaciones", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaAsign", .Name = "FechaAsign", .HeaderText = "Fecha Asignación"})
    End Sub

    Private Sub ConfigurarGrillaEstados()
        dgvEstadosTransitorios.AutoGenerateColumns = False : dgvEstadosTransitorios.Columns.Clear()
        dgvEstadosTransitorios.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "TipoEstado", .HeaderText = "Tipo de Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
        Dim colDesde As New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaDesde", .HeaderText = "Desde", .Width = 100} : colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstadosTransitorios.Columns.Add(colDesde)
        Dim colHasta As New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaHasta", .HeaderText = "Hasta", .Width = 100} : colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstadosTransitorios.Columns.Add(colHasta)
        dgvEstadosTransitorios.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .HeaderText = "Observaciones / Detalles", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
    End Sub

    Private Sub btnSeleccionarFoto_Click(sender As Object, e As EventArgs) Handles btnSeleccionarFoto.Click
        Using ofd As New OpenFileDialog() With {.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp"}
            If ofd.ShowDialog() = DialogResult.OK Then _rutaFotoSeleccionada = ofd.FileName : pbFoto.Image = Image.FromFile(ofd.FileName)
        End Using
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        _cerrandoPorCodigo = True : Close()
    End Sub

#End Region

#Region " Cierre y Métodos Misceláneos "

    Private Sub frmFuncionarioCrear_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If _cerrandoPorCodigo OrElse _seGuardo Then Return
        If _uow.Context.ChangeTracker.HasChanges() OrElse _estadoRows.Any(Function(r) r.EntityRef.Id <= 0) OrElse _estadosParaEliminar.Any() Then
            Dim result = MessageBox.Show("Hay cambios sin guardar. ¿Desea guardarlos antes de salir?", "Cambios Pendientes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)
            Select Case result
                Case DialogResult.Yes
                    Me.BeginInvoke(New MethodInvoker(AddressOf btnGuardar.PerformClick))
                    If Not _seGuardo Then e.Cancel = True
                Case DialogResult.No
                Case DialogResult.Cancel : e.Cancel = True
            End Select
        End If
    End Sub

    Private Sub Foco(sender As Object, e As EventArgs) Handles Me.Shown
        txtCI.Focus()
    End Sub

    Private Async Function CargarCombosAsync() As Task
        cboTipoFuncionario.DataSource = Await _svc.ObtenerTiposFuncionarioAsync() : cboTipoFuncionario.DisplayMember = "Value" : cboTipoFuncionario.ValueMember = "Key"
        cboCargo.DataSource = Await _svc.ObtenerCargosAsync() : cboCargo.DisplayMember = "Value" : cboCargo.ValueMember = "Key"
        cboEscalafon.DataSource = Await _svc.ObtenerEscalafonesAsync() : cboEscalafon.DisplayMember = "Value" : cboEscalafon.ValueMember = "Key"
        cboFuncion.DataSource = Await _svc.ObtenerFuncionesAsync() : cboFuncion.DisplayMember = "Value" : cboFuncion.ValueMember = "Key"
        cboEstadoCivil.DataSource = Await _svc.ObtenerEstadosCivilesAsync() : cboEstadoCivil.DisplayMember = "Value" : cboEstadoCivil.ValueMember = "Key"
        cboGenero.DataSource = Await _svc.ObtenerGenerosAsync() : cboGenero.DisplayMember = "Value" : cboGenero.ValueMember = "Key"
        cboNivelEstudio.DataSource = Await _svc.ObtenerNivelesEstudioAsync() : cboNivelEstudio.DisplayMember = "Value" : cboNivelEstudio.ValueMember = "Key"
        cboEstado.DataSource = Await _svc.ObtenerEstadosAsync() : cboEstado.DisplayMember = "Value" : cboEstado.ValueMember = "Key"
        cboSeccion.DataSource = Await _svc.ObtenerSeccionesAsync() : cboSeccion.DisplayMember = "Value" : cboSeccion.ValueMember = "Key"
        cboPuestoTrabajo.DataSource = Await _svc.ObtenerPuestosTrabajoAsync() : cboPuestoTrabajo.DisplayMember = "Value" : cboPuestoTrabajo.ValueMember = "Key"
        cboTurno.DataSource = Await _svc.ObtenerTurnosAsync() : cboTurno.DisplayMember = "Value" : cboTurno.ValueMember = "Key"
        cboSemana.DataSource = Await _svc.ObtenerSemanasAsync() : cboSemana.DisplayMember = "Value" : cboSemana.ValueMember = "Key"
        cboHorario.DataSource = Await _svc.ObtenerHorariosAsync() : cboHorario.DisplayMember = "Value" : cboHorario.ValueMember = "Key"
        cboCargo.SelectedIndex = -1 : cboEscalafon.SelectedIndex = -1 : cboFuncion.SelectedIndex = -1
        cboEstadoCivil.SelectedIndex = -1 : cboGenero.SelectedIndex = -1 : cboNivelEstudio.SelectedIndex = -1
        cboEstado.SelectedIndex = -1 : cboSeccion.SelectedIndex = -1 : cboPuestoTrabajo.SelectedIndex = -1
        cboTurno.SelectedIndex = -1 : cboSemana.SelectedIndex = -1 : cboHorario.SelectedIndex = -1
    End Function

    Private Sub btnAuditoria_Click(sender As Object, e As EventArgs) Handles btnAuditoria.Click
        If _modo = ModoFormulario.Crear Then
            MessageBox.Show("Debe guardar el funcionario antes de poder ver su historial de cambios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim frm As New frmAuditoriaViewer(_funcionario.Id.ToString())
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(frm)
    End Sub

    Private Sub DgvEstadosTransitorios_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row = TryCast(dgvEstadosTransitorios.Rows(e.RowIndex).DataBoundItem, EstadoRow)
        If row?.EntityRef Is Nothing Then Return
        Using frm As New frmFuncionarioEstadoTransitorio(row.EntityRef, _uow, True)
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Function MapEstadosActivos(source As IEnumerable(Of EstadoTransitorio)) As BindingList(Of EstadoRow)
        Return New BindingList(Of EstadoRow)(source.Select(Function(et) BuildEstadoRow(et, "Estado")).ToList())
    End Function

    Private Function MapEstadosHistorial(source As IEnumerable(Of EstadoTransitorio)) As BindingList(Of EstadoRow)
        Return New BindingList(Of EstadoRow)(source.Select(Function(et) BuildEstadoRow(et, "Historial")).ToList())
    End Function

    Private Async Sub btnAñadirDotacion_Click(sender As Object, e As EventArgs) Handles btnAñadirDotacion.Click
        Dim nuevaDotacion = New FuncionarioDotacion()
        Using frm As New frmFuncionarioDotacion(nuevaDotacion)
            If frm.ShowDialog() = DialogResult.OK Then
                Try
                    _funcionario.FuncionarioDotacion.Add(frm.Dotacion)
                    Await _uow.CommitAsync()
                    _dotaciones.Add(frm.Dotacion)
                Catch ex As Exception
                    MessageBox.Show("Error al añadir la dotación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Async Sub btnEditarDotacion_Click(sender As Object, e As EventArgs) Handles btnEditarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        Dim dotacionSeleccionada = CType(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
        Using frm As New frmFuncionarioDotacion(dotacionSeleccionada)
            If frm.ShowDialog() = DialogResult.OK Then
                Try
                    _uow.Context.Entry(dotacionSeleccionada).State = EntityState.Modified
                    Await _uow.CommitAsync()
                    bsDotacion.ResetBindings(False)
                Catch ex As Exception
                    MessageBox.Show("Error al actualizar la dotación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Async Sub btnQuitarDotacion_Click(sender As Object, e As EventArgs) Handles btnQuitarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        Dim dotacionSeleccionada = TryCast(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
        If dotacionSeleccionada Is Nothing Then Return
        If MessageBox.Show("¿Está seguro de que desea quitar este elemento de dotación?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                _dotaciones.Remove(dotacionSeleccionada)
                If dotacionSeleccionada.Id > 0 Then
                    _uow.Repository(Of FuncionarioDotacion).Remove(dotacionSeleccionada)
                    Await _uow.CommitAsync()
                End If
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al quitar la dotación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                _dotaciones.Add(dotacionSeleccionada)
            End Try
        End If
    End Sub
#End Region

    Private Sub MarcarParaEliminar(estado As EstadoTransitorio)
        Dim ctx = _uow.Context

        ' Adjuntar si viniera detached
        If ctx.Entry(estado).State = EntityState.Detached Then
            ctx.Set(Of EstadoTransitorio)().Attach(estado)
        End If

        ' Cargar dependientes mínimos (evita que EF intente nullear FKs no anulables)
        ctx.Entry(estado).Collection(Function(e) e.EstadoTransitorioAdjunto).Load()

        ctx.Entry(estado).Reference(Function(e) e.DesignacionDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.EnfermedadDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.SancionDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.OrdenCincoDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.RetenDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.SumarioDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.TrasladoDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.BajaDeFuncionarioDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.CambioDeCargoDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.ReactivacionDeFuncionarioDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.SeparacionDelCargoDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.InicioDeProcesamientoDetalle).Load()
        ctx.Entry(estado).Reference(Function(e) e.DesarmadoDetalle).Load()

        ' 1) Borrar ADJUNTOS (1-N)
        If estado.EstadoTransitorioAdjunto IsNot Nothing AndAlso estado.EstadoTransitorioAdjunto.Any() Then
            For Each a In estado.EstadoTransitorioAdjunto.ToList()
                ctx.Entry(a).State = EntityState.Deleted
            Next
        End If

        ' 2) Borrar DETALLES (1-1) existentes
        If estado.DesignacionDetalle IsNot Nothing Then ctx.Entry(estado.DesignacionDetalle).State = EntityState.Deleted
        If estado.EnfermedadDetalle IsNot Nothing Then ctx.Entry(estado.EnfermedadDetalle).State = EntityState.Deleted
        If estado.SancionDetalle IsNot Nothing Then ctx.Entry(estado.SancionDetalle).State = EntityState.Deleted
        If estado.OrdenCincoDetalle IsNot Nothing Then ctx.Entry(estado.OrdenCincoDetalle).State = EntityState.Deleted
        If estado.RetenDetalle IsNot Nothing Then ctx.Entry(estado.RetenDetalle).State = EntityState.Deleted
        If estado.SumarioDetalle IsNot Nothing Then ctx.Entry(estado.SumarioDetalle).State = EntityState.Deleted
        If estado.TrasladoDetalle IsNot Nothing Then ctx.Entry(estado.TrasladoDetalle).State = EntityState.Deleted
        If estado.BajaDeFuncionarioDetalle IsNot Nothing Then ctx.Entry(estado.BajaDeFuncionarioDetalle).State = EntityState.Deleted
        If estado.CambioDeCargoDetalle IsNot Nothing Then ctx.Entry(estado.CambioDeCargoDetalle).State = EntityState.Deleted
        If estado.ReactivacionDeFuncionarioDetalle IsNot Nothing Then ctx.Entry(estado.ReactivacionDeFuncionarioDetalle).State = EntityState.Deleted
        If estado.SeparacionDelCargoDetalle IsNot Nothing Then ctx.Entry(estado.SeparacionDelCargoDetalle).State = EntityState.Deleted
        If estado.InicioDeProcesamientoDetalle IsNot Nothing Then ctx.Entry(estado.InicioDeProcesamientoDetalle).State = EntityState.Deleted
        If estado.DesarmadoDetalle IsNot Nothing Then ctx.Entry(estado.DesarmadoDetalle).State = EntityState.Deleted

        ' 3) Borrar el principal
        ctx.Entry(estado).State = EntityState.Deleted
    End Sub

End Class
