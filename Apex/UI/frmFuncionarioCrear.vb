Option Strict On
Option Explicit On

Imports System.IO
Imports System.Data.Entity
Imports System.ComponentModel
Imports System.Text

Public Class frmFuncionarioCrear
    Inherits Form

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    ' --- View model para mostrar Estados en la grilla ---
    Private Class EstadoRow
        Public Property Id As Integer
        Public Property Origen As String
        Public Property TipoEstado As String
        Public Property FechaDesde As Date?
        Public Property FechaHasta As Date?
        Public Property Observaciones As String
        Public Property EntityRef As EstadoTransitorio
    End Class

    ' --- Variables de la clase ---
    Private _uow As UnitOfWork
    Private _funcionario As Funcionario
    Private _svc As FuncionarioService
    Private _modo As ModoFormulario
    Private _idFuncionario As Integer
    Private _rutaFotoSeleccionada As String
    Private _tiposEstadoTransitorio As List(Of TipoEstadoTransitorio)
    Private _itemsDotacion As List(Of DotacionItem)

    Private _dotaciones As BindingList(Of FuncionarioDotacion)
    Private _estadoRows As BindingList(Of EstadoRow)
    Private _estadosParaEliminar As New List(Of EstadoTransitorio)


    ' BindingSources
    Private ReadOnly bsDotacion As New BindingSource()
    Private ReadOnly bsEstados As New BindingSource()

    ' Estado de guardado / cierre
    Private _seGuardo As Boolean = False
    Private _cerrandoPorCodigo As Boolean = False

    '-------------------- Constructores ----------------------
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _uow = New UnitOfWork()
        _funcionario = New Funcionario() With {.FechaIngreso = DateTime.Now}
        _estadoRows = New BindingList(Of EstadoRow)()
    End Sub

    Public Sub New(id As Integer)
        InitializeComponent()
        _modo = ModoFormulario.Editar
        _idFuncionario = id
        _uow = New UnitOfWork()
        _funcionario = _uow.Context.Set(Of Funcionario)().
                        Include(Function(f) f.FuncionarioDotacion.Select(Function(fd) fd.DotacionItem)).
                        Include(Function(f) f.EstadoTransitorio.Select(Function(et) et.TipoEstadoTransitorio)).
                        FirstOrDefault(Function(f) f.Id = id)

        If _funcionario IsNot Nothing AndAlso _funcionario.EstadoTransitorio IsNot Nothing Then
            For Each et In _funcionario.EstadoTransitorio
                Select Case et.TipoEstadoTransitorioId
            ' --- ESTADOS MANUALES ---
                    Case 1 : _uow.Context.Entry(et).Reference(Function(x) x.DesignacionDetalle).Load()
                    Case 2 : _uow.Context.Entry(et).Reference(Function(x) x.EnfermedadDetalle).Load()
                    Case 3 : _uow.Context.Entry(et).Reference(Function(x) x.SancionDetalle).Load()
                    Case 4 : _uow.Context.Entry(et).Reference(Function(x) x.OrdenCincoDetalle).Load()
                    Case 5 : _uow.Context.Entry(et).Reference(Function(x) x.RetenDetalle).Load()
                    Case 6 : _uow.Context.Entry(et).Reference(Function(x) x.SumarioDetalle).Load()
                    Case 21 : _uow.Context.Entry(et).Reference(Function(x) x.TrasladoDetalle).Load()

            ' --- EVENTOS AUTOMÁTICOS ---
                    Case 19 : _uow.Context.Entry(et).Reference(Function(x) x.BajaDeFuncionarioDetalle).Load()
                    Case 20 : _uow.Context.Entry(et).Reference(Function(x) x.CambioDeCargoDetalle).Load()
                    Case 22 : _uow.Context.Entry(et).Reference(Function(x) x.ReactivacionDeFuncionarioDetalle).Load()
                    Case 23 : _uow.Context.Entry(et).Reference(Function(x) x.SeparacionDelCargoDetalle).Load()
                    Case 24 : _uow.Context.Entry(et).Reference(Function(x) x.InicioDeProcesamientoDetalle).Load()
                End Select
            Next
        End If
    End Sub

    '------------------- Carga del Formulario --------------------------
    Private Async Sub frmFuncionarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        _svc = New FuncionarioService(_uow)

        Await CargarCombosAsync()

        _tiposEstadoTransitorio = Await _svc.ObtenerTiposEstadoTransitorioCompletosAsync()
        _itemsDotacion = Await _svc.ObtenerItemsDotacionCompletosAsync()

        If _funcionario Is Nothing AndAlso _modo = ModoFormulario.Editar Then
            MessageBox.Show("No se encontró el registro del funcionario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If

        _dotaciones = New BindingList(Of FuncionarioDotacion)(If(_funcionario.FuncionarioDotacion IsNot Nothing, _funcionario.FuncionarioDotacion.ToList(), New List(Of FuncionarioDotacion)()))
        _estadoRows = If(_modo = ModoFormulario.Editar, MapEstadosActivos(_funcionario.EstadoTransitorio.Where(AddressOf IsEstadoActivo)), New BindingList(Of EstadoRow)())

        ConfigurarGrillaDotacion()
        ConfigurarGrillaEstados()

        bsDotacion.DataSource = _dotaciones
        dgvDotacion.DataSource = bsDotacion

        bsEstados.DataSource = _estadoRows
        dgvEstadosTransitorios.DataSource = bsEstados

        AddHandler dgvDotacion.DataError, Sub(s, a) a.ThrowException = False
        AddHandler dgvDotacion.CellFormatting, AddressOf dgvDotacion_CellFormatting
        AddHandler chkVerHistorial.CheckedChanged, AddressOf chkVerHistorial_CheckedChanged
        AddHandler dgvEstadosTransitorios.SelectionChanged, AddressOf DgvEstadosTransitorios_SelectionChanged
        AddHandler dgvEstadosTransitorios.CellDoubleClick, AddressOf DgvEstadosTransitorios_CellDoubleClick

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

    Private Sub DgvEstadosTransitorios_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return

        Dim row = TryCast(dgvEstadosTransitorios.Rows(e.RowIndex).DataBoundItem, EstadoRow)
        If row Is Nothing OrElse row.EntityRef Is Nothing Then Return

        Using frm As New frmFuncionarioEstadoTransitorio(row.EntityRef, _uow, True)
            frm.ShowDialog(Me)
        End Using
    End Sub

    ' -------------------- Helpers de mapeo ---------------------
    Private Function MapEstadosActivos(source As IEnumerable(Of EstadoTransitorio)) As BindingList(Of EstadoRow)
        Dim list As New BindingList(Of EstadoRow)
        For Each et In source
            Dim row = BuildEstadoRow(et, "Estado")
            list.Add(row)
        Next
        Return list
    End Function

    Private Function MapEstadosHistorial(source As IEnumerable(Of EstadoTransitorio)) As BindingList(Of EstadoRow)
        Dim list As New BindingList(Of EstadoRow)
        For Each et In source
            Dim row = BuildEstadoRow(et, "Historial")
            list.Add(row)
        Next
        Return list
    End Function

    Private Function BuildEstadoRow(e As EstadoTransitorio, origen As String) As EstadoRow
        Dim tipoNombre = _tiposEstadoTransitorio.FirstOrDefault(Function(t) t.Id = e.TipoEstadoTransitorioId)?.Nombre
        Dim fd As Date? = Nothing, fh As Date? = Nothing, obs As String = "", det As String = ""

        Select Case e.TipoEstadoTransitorioId
            Case 1
                If e.DesignacionDetalle IsNot Nothing Then
                    fd = e.DesignacionDetalle.FechaDesde
                    fh = e.DesignacionDetalle.FechaHasta
                    obs = e.DesignacionDetalle.Observaciones
                    If Not String.IsNullOrWhiteSpace(e.DesignacionDetalle.DocResolucion) Then det = $"Resolución: {e.DesignacionDetalle.DocResolucion}"
                End If
            Case 2
                If e.EnfermedadDetalle IsNot Nothing Then
                    fd = e.EnfermedadDetalle.FechaDesde
                    fh = e.EnfermedadDetalle.FechaHasta
                    obs = e.EnfermedadDetalle.Observaciones
                    If Not String.IsNullOrWhiteSpace(e.EnfermedadDetalle.Diagnostico) Then det = $"Diagnóstico: {e.EnfermedadDetalle.Diagnostico}"
                End If
            Case 3
                If e.SancionDetalle IsNot Nothing Then
                    fd = e.SancionDetalle.FechaDesde
                    fh = e.SancionDetalle.FechaHasta
                    obs = e.SancionDetalle.Observaciones
                    If Not String.IsNullOrWhiteSpace(e.SancionDetalle.Resolucion) Then det = $"Resolución: {e.SancionDetalle.Resolucion}"
                End If
            Case 4
                If e.OrdenCincoDetalle IsNot Nothing Then
                    fd = e.OrdenCincoDetalle.FechaDesde
                    fh = e.OrdenCincoDetalle.FechaHasta
                    obs = e.OrdenCincoDetalle.Observaciones
                End If
            Case 5
                If e.RetenDetalle IsNot Nothing Then
                    fd = e.RetenDetalle.FechaReten
                    fh = Nothing
                    obs = e.RetenDetalle.Observaciones
                    If Not String.IsNullOrWhiteSpace(e.RetenDetalle.Turno) Then det = $"Turno: {e.RetenDetalle.Turno}"
                End If
            Case 6
                If e.SumarioDetalle IsNot Nothing Then
                    fd = e.SumarioDetalle.FechaDesde
                    fh = e.SumarioDetalle.FechaHasta
                    obs = e.SumarioDetalle.Observaciones
                    If Not String.IsNullOrWhiteSpace(e.SumarioDetalle.Expediente) Then det = $"Expediente: {e.SumarioDetalle.Expediente}"
                End If
            Case 19 ' Baja
                If e.BajaDeFuncionarioDetalle IsNot Nothing Then
                    fd = e.BajaDeFuncionarioDetalle.FechaDesde : obs = e.BajaDeFuncionarioDetalle.Observaciones
                End If
            Case 20 ' Cambio de Cargo
                If e.CambioDeCargoDetalle IsNot Nothing Then
                    fd = e.CambioDeCargoDetalle.FechaDesde : obs = e.CambioDeCargoDetalle.Observaciones
                End If
            Case 21 ' Traslado
                If e.TrasladoDetalle IsNot Nothing Then
                    fd = e.TrasladoDetalle.FechaDesde : obs = e.TrasladoDetalle.Observaciones
                End If
            Case 22 ' Reactivación
                If e.ReactivacionDeFuncionarioDetalle IsNot Nothing Then
                    fd = e.ReactivacionDeFuncionarioDetalle.FechaDesde : obs = e.ReactivacionDeFuncionarioDetalle.Observaciones
                End If
            Case 23 ' Separación del Cargo
                If e.SeparacionDelCargoDetalle IsNot Nothing Then
                    fd = e.SeparacionDelCargoDetalle.FechaDesde : obs = e.SeparacionDelCargoDetalle.Observaciones
                End If
            Case 24 ' Inicio de Procesamiento
                If e.InicioDeProcesamientoDetalle IsNot Nothing Then
                    fd = e.InicioDeProcesamientoDetalle.FechaDesde : obs = e.InicioDeProcesamientoDetalle.Observaciones
                End If
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

    Private Sub UpdateRowFromEntity(row As EstadoRow, e As EstadoTransitorio)
        Dim updated = BuildEstadoRow(e, row.Origen)
        row.TipoEstado = updated.TipoEstado
        row.FechaDesde = updated.FechaDesde
        row.FechaHasta = updated.FechaHasta
        row.Observaciones = updated.Observaciones
    End Sub

    '------------------- Lógica de dominio --------------------------
    Private Function IsEstadoActivo(et As EstadoTransitorio) As Boolean
        Dim fechaActual = Date.Today
        Dim fechaDesde As Date? = Nothing
        Dim fechaHasta As Date? = Nothing

        Select Case et.TipoEstadoTransitorioId
            Case 1 : If et.DesignacionDetalle IsNot Nothing Then fechaDesde = et.DesignacionDetalle.FechaDesde : fechaHasta = et.DesignacionDetalle.FechaHasta
            Case 2 : If et.EnfermedadDetalle IsNot Nothing Then fechaDesde = et.EnfermedadDetalle.FechaDesde : fechaHasta = et.EnfermedadDetalle.FechaHasta
            Case 3 : If et.SancionDetalle IsNot Nothing Then fechaDesde = et.SancionDetalle.FechaDesde : fechaHasta = et.SancionDetalle.FechaHasta
            Case 4 : If et.OrdenCincoDetalle IsNot Nothing Then fechaDesde = et.OrdenCincoDetalle.FechaDesde : fechaHasta = et.OrdenCincoDetalle.FechaHasta
            Case 5 : If et.RetenDetalle IsNot Nothing Then Return et.RetenDetalle.FechaReten.Date = fechaActual
            Case 6 : If et.SumarioDetalle IsNot Nothing Then fechaDesde = et.SumarioDetalle.FechaDesde : fechaHasta = et.SumarioDetalle.FechaHasta
            Case Else : Return False
        End Select

        If Not fechaDesde.HasValue Then Return False
        Return fechaDesde.Value.Date <= fechaActual AndAlso (Not fechaHasta.HasValue OrElse fechaHasta.Value.Date >= fechaActual)
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

        If _funcionario.Foto IsNot Nothing AndAlso _funcionario.Foto.Length > 0 Then
            pbFoto.Image = New Bitmap(New MemoryStream(_funcionario.Foto))
        Else
            pbFoto.Image = My.Resources.Police
        End If

        dtpFechaNacimiento.Value = If(_funcionario.FechaNacimiento.HasValue, _funcionario.FechaNacimiento.Value, dtpFechaNacimiento.MinDate)
        txtDomicilio.Text = _funcionario.Domicilio
        txtEmail.Text = _funcionario.Email
        txtTelefono.Text = _funcionario.Telefono
        cboEstadoCivil.SelectedValue = If(_funcionario.EstadoCivilId.HasValue, CInt(_funcionario.EstadoCivilId), -1)
        cboGenero.SelectedValue = If(_funcionario.GeneroId.HasValue, CInt(_funcionario.GeneroId), -1)
        cboNivelEstudio.SelectedValue = If(_funcionario.NivelEstudioId.HasValue, CInt(_funcionario.NivelEstudioId), -1)

        chkProcesado.Checked = _funcionario.Procesado
        chkSeparado.Checked = _funcionario.SeparadoDeCargo
        chkDesarmado.Checked = _funcionario.Desarmado

        txtCiudad.Text = _funcionario.Ciudad
        txtSeccional.Text = _funcionario.Seccional
        txtCredencial.Text = _funcionario.Credencial
        chkEstudia.Checked = _funcionario.Estudia
    End Sub

    '------------------- Cambiar vista Activos / Historial --------------------------
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
            Dim query = _uow.Context.Set(Of EstadoTransitorio)()
            Dim historial = Await query.
                Include(Function(et) et.TipoEstadoTransitorio).
                Include(Function(et) et.DesignacionDetalle).
                Include(Function(et) et.SancionDetalle).
                Include(Function(et) et.SumarioDetalle).
                Include(Function(et) et.OrdenCincoDetalle).
                Include(Function(et) et.EnfermedadDetalle).
                Include(Function(et) et.RetenDetalle).
                Where(Function(et) et.FuncionarioId = _idFuncionario).
                OrderByDescending(Function(et) et.Id).ToListAsync()

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
        btnEditarEstado.Enabled = editable
        btnQuitarEstado.Enabled = editable
        btnAñadirEstado.Enabled = True
    End Sub

#Region "CRUD Estados Transitorios - LÓGICA EN MEMORIA"
    Private Sub btnAñadirEstado_Click(sender As Object, e As EventArgs) Handles btnAñadirEstado.Click
        Dim nuevoEstado = New EstadoTransitorio()
        Using frm As New frmFuncionarioEstadoTransitorio(nuevoEstado, _tiposEstadoTransitorio, _uow)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Dim newRow = BuildEstadoRow(frm.Estado, "Nuevo")
                _estadoRows.Add(newRow)
                bsEstados.ResetBindings(False)
            End If
        End Using
    End Sub

    Private Sub btnEditarEstado_Click(sender As Object, e As EventArgs) Handles btnEditarEstado.Click
        Dim row = TryCast(dgvEstadosTransitorios.CurrentRow?.DataBoundItem, EstadoRow)
        If row Is Nothing OrElse row.EntityRef Is Nothing Then Return

        Using frm As New frmFuncionarioEstadoTransitorio(row.EntityRef, _tiposEstadoTransitorio, _uow)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                UpdateRowFromEntity(row, frm.Estado)
                bsEstados.ResetBindings(False)
            End If
        End Using
    End Sub

    Private Sub btnQuitarEstado_Click(sender As Object, e As EventArgs) Handles btnQuitarEstado.Click
        Dim row = TryCast(dgvEstadosTransitorios.CurrentRow?.DataBoundItem, EstadoRow)
        If row Is Nothing OrElse row.EntityRef Is Nothing Then Return

        If MessageBox.Show("¿Está seguro de que desea quitar este estado transitorio?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim estadoParaQuitar = row.EntityRef

            If estadoParaQuitar.Id > 0 Then
                _estadosParaEliminar.Add(estadoParaQuitar)
            End If

            _estadoRows.Remove(row)
            bsEstados.ResetBindings(False)
        End If
    End Sub
#End Region

    '------------------- LÓGICA DE GUARDADO CENTRALIZADA --------------------------
    Private Async Function GuardarAsync() As Task(Of Boolean)
        Try
            If String.IsNullOrWhiteSpace(txtCI.Text) OrElse String.IsNullOrWhiteSpace(txtNombre.Text) OrElse cboTipoFuncionario.SelectedIndex = -1 Then
                MessageBox.Show("Los campos CI, Nombre y Tipo de Funcionario son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If

            ' 1. Mapear datos del funcionario
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
            _funcionario.Procesado = chkProcesado.Checked
            _funcionario.SeparadoDeCargo = chkSeparado.Checked
            _funcionario.Desarmado = chkDesarmado.Checked
            _funcionario.Ciudad = txtCiudad.Text.Trim()
            _funcionario.Seccional = txtSeccional.Text.Trim()
            _funcionario.Credencial = txtCredencial.Text.Trim()
            _funcionario.Estudia = chkEstudia.Checked

            If Not String.IsNullOrWhiteSpace(_rutaFotoSeleccionada) Then
                _funcionario.Foto = File.ReadAllBytes(_rutaFotoSeleccionada)
            End If

            ' 2. Añadir o actualizar el funcionario en el contexto
            If _modo = ModoFormulario.Crear Then
                _funcionario.CreatedAt = DateTime.Now
                _uow.Repository(Of Funcionario).Add(_funcionario)
            Else
                _funcionario.UpdatedAt = DateTime.Now
                _uow.Repository(Of Funcionario).Update(_funcionario)
            End If

            ' 3. Sincronizar estados transitorios
            Dim estadosRepo = _uow.Repository(Of EstadoTransitorio)()
            Dim adjuntosRepo = _uow.Repository(Of EstadoTransitorioAdjunto)()

            ' (CAMBIO CLAVE) - Lógica de eliminación robusta
            If _estadosParaEliminar.Any() Then
                For Each estadoOriginal In _estadosParaEliminar
                    Dim estadoAEliminar = Await _uow.Context.Set(Of EstadoTransitorio)().
                        Include(Function(et) et.DesignacionDetalle).
                        Include(Function(et) et.EnfermedadDetalle).
                        Include(Function(et) et.SancionDetalle).
                        Include(Function(et) et.OrdenCincoDetalle).
                        Include(Function(et) et.RetenDetalle).
                        Include(Function(et) et.SumarioDetalle).
                        Include(Function(et) et.EstadoTransitorioAdjunto).
                        FirstOrDefaultAsync(Function(et) et.Id = estadoOriginal.Id)

                    If estadoAEliminar IsNot Nothing Then
                        If estadoAEliminar.DesignacionDetalle IsNot Nothing Then _uow.Context.Set(Of DesignacionDetalle)().Remove(estadoAEliminar.DesignacionDetalle)
                        If estadoAEliminar.EnfermedadDetalle IsNot Nothing Then _uow.Context.Set(Of EnfermedadDetalle)().Remove(estadoAEliminar.EnfermedadDetalle)
                        If estadoAEliminar.SancionDetalle IsNot Nothing Then _uow.Context.Set(Of SancionDetalle)().Remove(estadoAEliminar.SancionDetalle)
                        If estadoAEliminar.OrdenCincoDetalle IsNot Nothing Then _uow.Context.Set(Of OrdenCincoDetalle)().Remove(estadoAEliminar.OrdenCincoDetalle)
                        If estadoAEliminar.RetenDetalle IsNot Nothing Then _uow.Context.Set(Of RetenDetalle)().Remove(estadoAEliminar.RetenDetalle)
                        If estadoAEliminar.SumarioDetalle IsNot Nothing Then _uow.Context.Set(Of SumarioDetalle)().Remove(estadoAEliminar.SumarioDetalle)

                        If estadoAEliminar.EstadoTransitorioAdjunto.Any() Then
                            adjuntosRepo.RemoveRange(estadoAEliminar.EstadoTransitorioAdjunto.ToList())
                        End If

                        estadosRepo.Remove(estadoAEliminar)
                    End If
                Next
            End If

            ' Guardar funcionario para obtener su ID si es nuevo y procesar eliminaciones
            Await _uow.CommitAsync()

            ' Procesar estados de la grilla (nuevos y modificados)
            For Each row In _estadoRows
                Dim estado = row.EntityRef
                If estado.Id <= 0 Then ' Es un estado nuevo
                    estado.Id = 0
                    estado.FuncionarioId = _funcionario.Id
                    estadosRepo.Add(estado)
                    Await _uow.CommitAsync() ' Guardamos para obtener el ID del estado

                    If estado.AdjuntosNuevos.Any() Then
                        For Each adjunto In estado.AdjuntosNuevos
                            adjunto.Id = 0
                            adjunto.EstadoTransitorioId = estado.Id
                            adjuntosRepo.Add(adjunto)
                        Next
                    End If
                Else ' Es un estado existente que pudo haber sido modificado
                    _uow.Context.Entry(estado).State = EntityState.Modified
                End If
            Next

            ' 4. Guardar todos los cambios pendientes
            Await _uow.CommitAsync()
            _seGuardo = True
            Return True

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar: " & ex.Message & vbCrLf & ex.InnerException?.Message, "Error de Guardado", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        LoadingHelper.MostrarCargando(Me)
        If Await GuardarAsync() Then
            LoadingHelper.OcultarCargando(Me)
            MessageBox.Show(If(_modo = ModoFormulario.Crear, "Funcionario creado", "Funcionario actualizado") & " correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' --- INICIO DE LA MODIFICACIÓN ---
            ' Notificar a toda la aplicación que los datos han cambiado.
            NotificadorEventos.NotificarActualizacion()
            ' --- FIN DE LA MODIFICACIÓN ---

            Me.DialogResult = DialogResult.OK
            _cerrandoPorCodigo = True
            Close()
        Else
            LoadingHelper.OcultarCargando(Me)
        End If
    End Sub

    Private Sub frmFuncionarioCrear_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If _cerrandoPorCodigo OrElse _seGuardo Then Return

        If _uow.Context.ChangeTracker.HasChanges() OrElse _estadoRows.Any(Function(r) r.EntityRef.Id <= 0) OrElse _estadosParaEliminar.Any() Then
            Dim result = MessageBox.Show("Hay cambios sin guardar. ¿Desea guardarlos antes de salir?", "Cambios Pendientes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)
            Select Case result
                Case DialogResult.Yes
                    ' Usamos BeginInvoke para permitir que el formulario se cierre después de hacer clic en guardar.
                    Me.BeginInvoke(New MethodInvoker(AddressOf btnGuardar.PerformClick))
                    If Not _seGuardo Then e.Cancel = True
                Case DialogResult.No
                    ' Permite el cierre sin guardar
                Case DialogResult.Cancel
                    e.Cancel = True
            End Select
        End If
    End Sub

#Region "Métodos Auxiliares y de UI"
    Private Async Function CargarCombosAsync() As Task
        cboTipoFuncionario.DataSource = Await _svc.ObtenerTiposFuncionarioAsync()
        cboTipoFuncionario.DisplayMember = "Value"
        cboTipoFuncionario.ValueMember = "Key"
        cboCargo.DataSource = Await _svc.ObtenerCargosAsync()
        cboCargo.DisplayMember = "Value"
        cboCargo.ValueMember = "Key"
        cboEscalafon.DataSource = Await _svc.ObtenerEscalafonesAsync()
        cboEscalafon.DisplayMember = "Value"
        cboEscalafon.ValueMember = "Key"
        cboFuncion.DataSource = Await _svc.ObtenerFuncionesAsync()
        cboFuncion.DisplayMember = "Value"
        cboFuncion.ValueMember = "Key"
        cboEstadoCivil.DataSource = Await _svc.ObtenerEstadosCivilesAsync()
        cboEstadoCivil.DisplayMember = "Value"
        cboEstadoCivil.ValueMember = "Key"
        cboGenero.DataSource = Await _svc.ObtenerGenerosAsync()
        cboGenero.DisplayMember = "Value"
        cboGenero.ValueMember = "Key"
        cboNivelEstudio.DataSource = Await _svc.ObtenerNivelesEstudioAsync()
        cboNivelEstudio.DisplayMember = "Value"
        cboNivelEstudio.ValueMember = "Key"
        cboEstado.DataSource = Await _svc.ObtenerEstadosAsync()
        cboEstado.DisplayMember = "Value"
        cboEstado.ValueMember = "Key"
        cboSeccion.DataSource = Await _svc.ObtenerSeccionesAsync()
        cboSeccion.DisplayMember = "Value"
        cboSeccion.ValueMember = "Key"
        cboPuestoTrabajo.DataSource = Await _svc.ObtenerPuestosTrabajoAsync()
        cboPuestoTrabajo.DisplayMember = "Value"
        cboPuestoTrabajo.ValueMember = "Key"
        cboTurno.DataSource = Await _svc.ObtenerTurnosAsync()
        cboTurno.DisplayMember = "Value"
        cboTurno.ValueMember = "Key"
        cboSemana.DataSource = Await _svc.ObtenerSemanasAsync()
        cboSemana.DisplayMember = "Value"
        cboSemana.ValueMember = "Key"
        cboHorario.DataSource = Await _svc.ObtenerHorariosAsync()
        cboHorario.DisplayMember = "Value"
        cboHorario.ValueMember = "Key"

        cboCargo.SelectedIndex = -1
        cboEscalafon.SelectedIndex = -1
        cboFuncion.SelectedIndex = -1
        cboEstadoCivil.SelectedIndex = -1
        cboGenero.SelectedIndex = -1
        cboNivelEstudio.SelectedIndex = -1
        cboEstado.SelectedIndex = -1
        cboSeccion.SelectedIndex = -1
        cboPuestoTrabajo.SelectedIndex = -1
        cboTurno.SelectedIndex = -1
        cboSemana.SelectedIndex = -1
        cboHorario.SelectedIndex = -1
    End Function

    Private Sub ConfigurarGrillaDotacion()
        With dgvDotacion
            .SuspendLayout()
            .AutoGenerateColumns = False
            .Columns.Clear()
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .EditMode = DataGridViewEditMode.EditProgrammatically
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .RowHeadersVisible = False
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colItem", .HeaderText = "Ítem", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .ValueType = GetType(String)})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Talla", .Name = "Talla", .HeaderText = "Talla", .ValueType = GetType(Object)})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .Name = "Observaciones", .HeaderText = "Observaciones", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .ValueType = GetType(String)})
            Dim colFecha As New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaAsign", .Name = "FechaAsign", .HeaderText = "Fecha Asignación", .ValueType = GetType(Object)}
            colFecha.DefaultCellStyle.NullValue = ""
            .Columns.Add(colFecha)
            .ResumeLayout()
        End With
    End Sub

    Private Sub ConfigurarGrillaEstados()
        With dgvEstadosTransitorios
            .SuspendLayout()
            .AutoGenerateColumns = False
            .Columns.Clear()
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .RowHeadersVisible = False
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "TipoEstado", .Name = "TipoEstado", .HeaderText = "Tipo de Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .ValueType = GetType(String), .SortMode = DataGridViewColumnSortMode.NotSortable})
            Dim colDesde As New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaDesde", .Name = "FechaDesde", .HeaderText = "Desde", .Width = 100, .ValueType = GetType(Date)}
            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
            colDesde.DefaultCellStyle.NullValue = ""
            .Columns.Add(colDesde)
            Dim colHasta As New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaHasta", .Name = "FechaHasta", .HeaderText = "Hasta", .Width = 100, .ValueType = GetType(Date)}
            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
            colHasta.DefaultCellStyle.NullValue = ""
            .Columns.Add(colHasta)
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .Name = "Observaciones", .HeaderText = "Observaciones / Detalles", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .ValueType = GetType(String), .SortMode = DataGridViewColumnSortMode.NotSortable})
            .ResumeLayout()
        End With
    End Sub

    Private Sub dgvDotacion_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        If dgv.Columns Is Nothing OrElse e.ColumnIndex >= dgv.Columns.Count Then Return

        Dim cm As CurrencyManager = TryCast(dgv.BindingContext(dgv.DataSource), CurrencyManager)
        If cm Is Nothing OrElse e.RowIndex >= cm.Count Then
            e.Value = Nothing : e.FormattingApplied = True : Return
        End If

        Dim colName = dgv.Columns(e.ColumnIndex).Name

        If colName = "colItem" Then
            Dim dataItem = TryCast(dgv.Rows(e.RowIndex).DataBoundItem, FuncionarioDotacion)
            If dataItem Is Nothing Then Return
            Dim item = _itemsDotacion?.FirstOrDefault(Function(i) i.Id = dataItem.DotacionItemId)
            e.Value = If(item IsNot Nothing, item.Nombre, String.Empty)
            e.FormattingApplied = True
            Return
        End If

        If colName = "FechaAsign" Then
            Dim raw = e.Value
            If raw Is Nothing OrElse raw Is DBNull.Value Then
                e.Value = "" : e.FormattingApplied = True : Return
            End If
            Dim dt As DateTime
            If TypeOf raw Is DateTime Then
                dt = CType(raw, DateTime)
                e.Value = dt.ToString("dd/MM/yyyy")
            Else
                If DateTime.TryParse(Convert.ToString(raw), dt) Then
                    e.Value = dt.ToString("dd/MM/yyyy")
                Else
                    e.Value = ""
                End If
            End If
            e.FormattingApplied = True
            Return
        End If

        If colName = "Talla" Then
            e.Value = If(e.Value Is Nothing, "", e.Value.ToString())
            e.FormattingApplied = True
            Return
        End If
    End Sub
#End Region

#Region "CRUD Dotación"
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
                    Dim svcDotacion As New GenericService(Of FuncionarioDotacion)(_uow)
                    Await svcDotacion.DeleteAsync(dotacionSeleccionada.Id)
                End If
                If _funcionario.FuncionarioDotacion.Contains(dotacionSeleccionada) Then
                    _funcionario.FuncionarioDotacion.Remove(dotacionSeleccionada)
                End If
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al quitar la dotación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                _dotaciones.Add(dotacionSeleccionada)
            End Try
        End If
    End Sub
#End Region

    Private Sub btnSeleccionarFoto_Click(sender As Object, e As EventArgs) Handles btnSeleccionarFoto.Click
        Using ofd As New OpenFileDialog() With {.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp"}
            If ofd.ShowDialog() = DialogResult.OK Then
                _rutaFotoSeleccionada = ofd.FileName
                pbFoto.Image = Image.FromFile(ofd.FileName)
            End If
        End Using
    End Sub

    Private Sub btnAuditoria_Click(sender As Object, e As EventArgs) Handles btnAuditoria.Click
        If _modo = ModoFormulario.Crear Then
            MessageBox.Show("Debe guardar el funcionario antes de poder ver su historial de cambios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim frm As New frmAuditoriaViewer(_funcionario.Id.ToString())
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(frm)
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        _cerrandoPorCodigo = True
        Close()
    End Sub


    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            btnCancelar.PerformClick()
        End If
    End Sub

    Private Sub Foco(sender As Object, e As EventArgs) Handles Me.Shown
        txtCI.Focus()
    End Sub
End Class