Option Strict On
Option Explicit On

Imports System.IO
Imports System.Data.Entity
Imports System.ComponentModel
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

#End Region

#Region " Lógica de Carga y Mapeo de Datos "

    ' Archivo: Apex/UI/frmFuncionarioCrear.vb

    Private Sub LoadEstadoTransitorioDetails(et As EstadoTransitorio)
        Select Case et.TipoEstadoTransitorioId
            Case TiposEstadoCatalog.Designacion
                _uow.Context.Entry(et).Reference(Function(x) x.DesignacionDetalle).Load()

            Case TiposEstadoCatalog.Enfermedad
                _uow.Context.Entry(et).Reference(Function(x) x.EnfermedadDetalle).Load()

            Case TiposEstadoCatalog.Sancion
                _uow.Context.Entry(et).Reference(Function(x) x.SancionDetalle).Load()

            Case TiposEstadoCatalog.OrdenCinco
                _uow.Context.Entry(et).Reference(Function(x) x.OrdenCincoDetalle).Load()

            Case TiposEstadoCatalog.Reten
                _uow.Context.Entry(et).Reference(Function(x) x.RetenDetalle).Load()

            Case TiposEstadoCatalog.Sumario
                _uow.Context.Entry(et).Reference(Function(x) x.SumarioDetalle).Load()

            Case TiposEstadoCatalog.CambioDeCargo
                _uow.Context.Entry(et).Reference(Function(x) x.CambioDeCargoDetalle).Load()

            Case TiposEstadoCatalog.Traslado
                _uow.Context.Entry(et).Reference(Function(x) x.TrasladoDetalle).Load()

            Case TiposEstadoCatalog.SeparacionDelCargo
                _uow.Context.Entry(et).Reference(Function(x) x.SeparacionDelCargoDetalle).Load()

            Case TiposEstadoCatalog.Desarmado
                _uow.Context.Entry(et).Reference(Function(x) x.DesarmadoDetalle).Load()
        End Select
    End Sub



    Private Function BuildEstadoRow(e As EstadoTransitorio, origen As String) As EstadoRow
        ' Nombre del tipo
        Dim tipoNombre = _tiposEstadoTransitorio _
        .FirstOrDefault(Function(t) t.Id = e.TipoEstadoTransitorioId)?.Nombre

        ' Fechas (centralizado, sin duplicar lógica)
        Dim fd As Date? = Nothing, fh As Date? = Nothing
        e.GetFechas(fd, fh)

        Dim obs As String = String.Empty
        Dim det As String = String.Empty

        Select Case e.TipoEstadoTransitorioId
            Case TiposEstadoCatalog.Designacion
                Dim d = e.DesignacionDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    If Not String.IsNullOrWhiteSpace(d.DocResolucion) Then det = $"Resolución: {d.DocResolucion}"
                End If

            Case TiposEstadoCatalog.Enfermedad
                Dim d = e.EnfermedadDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    If Not String.IsNullOrWhiteSpace(d.Diagnostico) Then det = $"Diagnóstico: {d.Diagnostico}"
                End If

            Case TiposEstadoCatalog.Sancion
                Dim d = e.SancionDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    If Not String.IsNullOrWhiteSpace(d.Resolucion) Then det = $"Resolución: {d.Resolucion}"
                End If

            Case TiposEstadoCatalog.OrdenCinco
                Dim d = e.OrdenCincoDetalle
                If d IsNot Nothing Then obs = d.Observaciones

            Case TiposEstadoCatalog.Reten
                Dim d = e.RetenDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    If Not String.IsNullOrWhiteSpace(d.Turno) Then det = $"Turno: {d.Turno}"
                End If

            Case TiposEstadoCatalog.Sumario
                Dim d = e.SumarioDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    If Not String.IsNullOrWhiteSpace(d.Expediente) Then det = $"Expediente: {d.Expediente}"
                End If

            Case TiposEstadoCatalog.Traslado
                Dim d = e.TrasladoDetalle
                If d IsNot Nothing Then obs = d.Observaciones

            Case TiposEstadoCatalog.CambioDeCargo
                Dim d = e.CambioDeCargoDetalle
                If d IsNot Nothing Then obs = d.Observaciones

            Case TiposEstadoCatalog.SeparacionDelCargo
                Dim d = e.SeparacionDelCargoDetalle
                If d IsNot Nothing Then obs = d.Observaciones

            Case TiposEstadoCatalog.Desarmado
                Dim d = e.DesarmadoDetalle
                If d IsNot Nothing Then obs = d.Observaciones
        End Select

        Dim obsFinal = If(String.IsNullOrWhiteSpace(det), obs, $"{det} | {obs}")

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


    Private Sub CargarDatosEnControles()
        txtCI.Text = _funcionario.CI
        txtNombre.Text = _funcionario.Nombre
        dtpFechaIngreso.Value = _funcionario.FechaIngreso
        cboTipoFuncionario.SelectedValue = CInt(_funcionario.TipoFuncionarioId)
        cboCargo.SelectedValue = If(_funcionario.CargoId.HasValue, CInt(_funcionario.CargoId), -1)
        chkActivo.Checked = _funcionario.Activo
        cboEscalafon.SelectedValue = If(_funcionario.EscalafonId.HasValue, CInt(_funcionario.EscalafonId), -1)
        cboFuncion.SelectedValue = If(_funcionario.FuncionId.HasValue, CInt(_funcionario.FuncionId), -1)
        cboEstado.SelectedValue = If(_funcionario.EstadoId.HasValue, CInt(_funcionario.EstadoId), -1)
        cboSeccion.SelectedValue = If(_funcionario.SeccionId.HasValue, CInt(_funcionario.SeccionId), -1)
        cboPuestoTrabajo.SelectedValue = If(_funcionario.PuestoTrabajoId.HasValue, CInt(_funcionario.PuestoTrabajoId), -1)
        cboTurno.SelectedValue = If(_funcionario.TurnoId.HasValue, CInt(_funcionario.TurnoId), -1)
        cboSemana.SelectedValue = If(_funcionario.SemanaId.HasValue, CInt(_funcionario.SemanaId), -1)
        cboHorario.SelectedValue = If(_funcionario.HorarioId.HasValue, CInt(_funcionario.HorarioId), -1)
        If _funcionario.Foto IsNot Nothing AndAlso _funcionario.Foto.Length > 0 Then pbFoto.Image = New Bitmap(New MemoryStream(_funcionario.Foto)) Else pbFoto.Image = My.Resources.Police
        dtpFechaNacimiento.Value = If(_funcionario.FechaNacimiento.HasValue, _funcionario.FechaNacimiento.Value, dtpFechaNacimiento.MinDate)
        txtDomicilio.Text = _funcionario.Domicilio
        txtEmail.Text = _funcionario.Email
        txtTelefono.Text = _funcionario.Telefono
        cboEstadoCivil.SelectedValue = If(_funcionario.EstadoCivilId.HasValue, CInt(_funcionario.EstadoCivilId), -1)
        cboGenero.SelectedValue = If(_funcionario.GeneroId.HasValue, CInt(_funcionario.GeneroId), -1)
        cboNivelEstudio.SelectedValue = If(_funcionario.NivelEstudioId.HasValue, CInt(_funcionario.NivelEstudioId), -1)
        txtCiudad.Text = _funcionario.Ciudad
        txtSeccional.Text = _funcionario.Seccional
        txtCredencial.Text = _funcionario.Credencial
        chkEstudia.Checked = _funcionario.Estudia
    End Sub

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
        Dim fechaActual = Date.Today, fechaDesde As Date? = Nothing, fechaHasta As Date? = Nothing
        et.GetFechas(fechaDesde, fechaHasta)
        If et.TipoEstadoTransitorioId = 5 Then Return If(fechaDesde.HasValue, fechaDesde.Value.Date = fechaActual, False)
        If Not fechaDesde.HasValue Then Return False
        Return fechaDesde.Value.Date <= fechaActual AndAlso (Not fechaHasta.HasValue OrElse fechaHasta.Value.Date >= fechaActual)
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
            If chkVerHistorial.Checked Then Await CargarHistorialCompleto() Else _estadoRows = MapEstadosActivos(_funcionario.EstadoTransitorio.Where(AddressOf IsEstadoActivo)) : bsEstados.DataSource = _estadoRows : bsEstados.ResetBindings(False)
        Finally
            _estaCargandoHistorial = False
            DgvEstadosTransitorios_SelectionChanged(Nothing, EventArgs.Empty)
        End Try
    End Sub

    ' Archivo: Apex/UI/frmFuncionarioCrear.vb

    Private Async Function CargarHistorialCompleto() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim query = _uow.Context.Set(Of EstadoTransitorio)().Where(Function(et) et.FuncionarioId = _idFuncionario)

            ' CONSULTA CORREGIDA: Se añaden los .Include() que faltaban
            Dim historial = Await query.Include(Function(et) et.TipoEstadoTransitorio) _
                             .Include(Function(et) et.DesignacionDetalle) _
                             .Include(Function(et) et.SancionDetalle) _
                             .Include(Function(et) et.SumarioDetalle) _
                             .Include(Function(et) et.OrdenCincoDetalle) _
                             .Include(Function(et) et.EnfermedadDetalle) _
                             .Include(Function(et) et.RetenDetalle) _
                             .Include(Function(et) et.DesarmadoDetalle) _
                             .Include(Function(et) et.SeparacionDelCargoDetalle) _
                             .Include(Function(et) et.InicioDeProcesamientoDetalle) _
                             .Include(Function(et) et.TrasladoDetalle) _
                             .OrderByDescending(Function(et) et.Id).ToListAsync()

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
        Dim nuevoEstado = New EstadoTransitorio()
        Using frm As New frmFuncionarioEstadoTransitorio(nuevoEstado, _tiposEstadoTransitorio, _uow)
            If frm.ShowDialog(Me) = DialogResult.OK Then _estadoRows.Add(BuildEstadoRow(frm.Estado, "Nuevo")) : bsEstados.ResetBindings(False)
        End Using
    End Sub

    Private Sub btnEditarEstado_Click(sender As Object, e As EventArgs) Handles btnEditarEstado.Click
        Dim row = TryCast(dgvEstadosTransitorios.CurrentRow?.DataBoundItem, EstadoRow)
        If row?.EntityRef Is Nothing Then Return
        Using frm As New frmFuncionarioEstadoTransitorio(row.EntityRef, _tiposEstadoTransitorio, _uow)
            If frm.ShowDialog(Me) = DialogResult.OK Then UpdateRowFromEntity(row, frm.Estado) : bsEstados.ResetBindings(False)
        End Using
    End Sub

    Private Sub btnQuitarEstado_Click(sender As Object, e As EventArgs) Handles btnQuitarEstado.Click
        Dim row = TryCast(dgvEstadosTransitorios.CurrentRow?.DataBoundItem, EstadoRow)
        If row?.EntityRef Is Nothing Then Return

        If MessageBox.Show("¿Está seguro de que desea quitar este estado transitorio?", "Confirmar Eliminación",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        Dim entidad = row.EntityRef

        If entidad.Id > 0 Then
            ' Importante: NO rompas la relación de navegación.
            ' NO hagas: _funcionario.EstadoTransitorio.Remove(entidad)

            ' Marcá la entidad (y su detalle) como Deleted ya, así EF no intenta nullear FKs
            MarcarParaEliminar(entidad)
        Else
            ' Era nuevo/no persistido: simplemente sacalo del contexto
            If _uow.Context.Entry(entidad).State <> EntityState.Detached Then
                _uow.Context.Entry(entidad).State = EntityState.Detached
            End If
        End If

        ' Actualizá solo la UI
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
        ' Attach si viniera detached
        If _uow.Context.Entry(estado).State = EntityState.Detached Then
            _uow.Context.Set(Of EstadoTransitorio)().Attach(estado)
        End If

        ' Si NO tenés ON DELETE CASCADE en BD, eliminá explícitamente el detalle correspondiente
        If estado.DesignacionDetalle IsNot Nothing Then _uow.Context.Entry(estado.DesignacionDetalle).State = EntityState.Deleted
        If estado.EnfermedadDetalle IsNot Nothing Then _uow.Context.Entry(estado.EnfermedadDetalle).State = EntityState.Deleted
        If estado.SancionDetalle IsNot Nothing Then _uow.Context.Entry(estado.SancionDetalle).State = EntityState.Deleted
        If estado.OrdenCincoDetalle IsNot Nothing Then _uow.Context.Entry(estado.OrdenCincoDetalle).State = EntityState.Deleted
        If estado.RetenDetalle IsNot Nothing Then _uow.Context.Entry(estado.RetenDetalle).State = EntityState.Deleted
        If estado.SumarioDetalle IsNot Nothing Then _uow.Context.Entry(estado.SumarioDetalle).State = EntityState.Deleted
        If estado.TrasladoDetalle IsNot Nothing Then _uow.Context.Entry(estado.TrasladoDetalle).State = EntityState.Deleted
        If estado.BajaDeFuncionarioDetalle IsNot Nothing Then _uow.Context.Entry(estado.BajaDeFuncionarioDetalle).State = EntityState.Deleted
        If estado.CambioDeCargoDetalle IsNot Nothing Then _uow.Context.Entry(estado.CambioDeCargoDetalle).State = EntityState.Deleted
        If estado.ReactivacionDeFuncionarioDetalle IsNot Nothing Then _uow.Context.Entry(estado.ReactivacionDeFuncionarioDetalle).State = EntityState.Deleted
        If estado.SeparacionDelCargoDetalle IsNot Nothing Then _uow.Context.Entry(estado.SeparacionDelCargoDetalle).State = EntityState.Deleted
        If estado.InicioDeProcesamientoDetalle IsNot Nothing Then _uow.Context.Entry(estado.InicioDeProcesamientoDetalle).State = EntityState.Deleted
        If estado.DesarmadoDetalle IsNot Nothing Then _uow.Context.Entry(estado.DesarmadoDetalle).State = EntityState.Deleted

        ' Ahora sí: eliminá el estado
        _uow.Context.Entry(estado).State = EntityState.Deleted
    End Sub

End Class