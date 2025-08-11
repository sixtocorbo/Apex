' Apex/UI/frmFuncionarioCrear.vb
Option Strict On
Option Explicit On

Imports System.IO
Imports System.Data.Entity
Imports System.ComponentModel

Public Class frmFuncionarioCrear
    Inherits Form

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    ' --- Variables de la clase ---
    Private _uow As UnitOfWork
    Private _funcionario As Funcionario
    Private _svc As FuncionarioService
    Private _estadoSvc As EstadoTransitorioService
    Private _modo As ModoFormulario
    Private _idFuncionario As Integer
    Private _rutaFotoSeleccionada As String
    Private _tiposEstadoTransitorio As List(Of TipoEstadoTransitorio)
    Private _dotaciones As BindingList(Of FuncionarioDotacion)
    Private _estadosTransitorios As BindingList(Of EstadoTransitorio)
    Private _historialConsolidado As List(Of Object)
    Private _itemsDotacion As List(Of DotacionItem)
    Private _cambiandoOrigen As Boolean = False
    Private _estaCargandoHistorial As Boolean = False
    Private _silenciarEventosEstados As Boolean = False

    '-------------------- Constructores ----------------------
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _uow = New UnitOfWork()
        _funcionario = New Funcionario() With {.FechaIngreso = DateTime.Now}
        _uow.Context.Set(Of Funcionario).Add(_funcionario)
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
                    Case 1 : _uow.Context.Entry(et).Reference(Function(x) x.DesignacionDetalle).Load()
                    Case 2 : _uow.Context.Entry(et).Reference(Function(x) x.EnfermedadDetalle).Load()
                    Case 3 : _uow.Context.Entry(et).Reference(Function(x) x.SancionDetalle).Load()
                    Case 4 : _uow.Context.Entry(et).Reference(Function(x) x.OrdenCincoDetalle).Load()
                    Case 5 : _uow.Context.Entry(et).Reference(Function(x) x.RetenDetalle).Load()
                    Case 6 : _uow.Context.Entry(et).Reference(Function(x) x.SumarioDetalle).Load()
                End Select
            Next
        End If
    End Sub

    '------------------- Carga del Formulario --------------------------
    Private Async Sub frmFuncionarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New FuncionarioService(_uow)
        _estadoSvc = New EstadoTransitorioService(_uow)

        Await CargarCombosAsync()

        _tiposEstadoTransitorio = Await _svc.ObtenerTiposEstadoTransitorioCompletosAsync()
        _itemsDotacion = Await _svc.ObtenerItemsDotacionCompletosAsync()

        If _funcionario Is Nothing AndAlso _modo = ModoFormulario.Editar Then
            MessageBox.Show("No se encontró el registro del funcionario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If

        _dotaciones = New BindingList(Of FuncionarioDotacion)(_funcionario.FuncionarioDotacion.ToList())

        ' --- INICIO DE LA MODIFICACIÓN CLAVE ---
        ' Filtra los estados para mostrar solo los activos por defecto.
        Dim estadosActivos = _funcionario.EstadoTransitorio.Where(Function(et) IsEstadoActivo(et)).ToList()
        _estadosTransitorios = New BindingList(Of EstadoTransitorio)(estadosActivos)
        ' --- FIN DE LA MODIFICACIÓN CLAVE ---

        ConfigurarGrillaDotacion()
        ConfigurarGrillaEstados()

        dgvDotacion.DataSource = _dotaciones
        dgvEstadosTransitorios.DataSource = _estadosTransitorios

        AddHandler dgvDotacion.CellFormatting, AddressOf dgvDotacion_CellFormatting
        AddHandler chkVerHistorial.CheckedChanged, AddressOf chkVerHistorial_CheckedChanged
        AddHandler dgvEstadosTransitorios.CellFormatting, AddressOf dgvEstadosTransitorios_CellFormatting
        AddHandler dgvEstadosTransitorios.SelectionChanged, AddressOf DgvEstadosTransitorios_SelectionChanged
        AddHandler dgvEstadosTransitorios.DataError, Sub(s, a) a.ThrowException = False

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

    ''' <summary>
    ''' Función de ayuda para determinar si un estado transitorio está vigente a la fecha actual.
    ''' </summary>
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
            Case Else : Return False ' Si el tipo no es reconocido, no se considera activo.
        End Select

        If Not fechaDesde.HasValue Then Return False

        ' La condición principal: debe haber empezado y no haber terminado aún.
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
    End Sub

    Private Async Sub chkVerHistorial_CheckedChanged(sender As Object, e As EventArgs) Handles chkVerHistorial.CheckedChanged
        If _estaCargandoHistorial Then Return

        _estaCargandoHistorial = True
        _cambiandoOrigen = True

        Try
            dgvEstadosTransitorios.DataSource = Nothing
            dgvEstadosTransitorios.Columns.Clear()
            ConfigurarGrillaEstados()

            If chkVerHistorial.Checked Then
                Await CargarHistorialCompleto() ' Esto carga _historialConsolidado y lo asigna a la grilla
            Else
                ' Volvemos a filtrar la lista de estados activos desde la fuente principal
                Dim estadosActivos = _funcionario.EstadoTransitorio.Where(Function(et) IsEstadoActivo(et)).ToList()
                _estadosTransitorios = New BindingList(Of EstadoTransitorio)(estadosActivos)
                dgvEstadosTransitorios.DataSource = _estadosTransitorios
            End If
        Finally
            _cambiandoOrigen = False
            _estaCargandoHistorial = False
            DgvEstadosTransitorios_SelectionChanged(Nothing, EventArgs.Empty) ' Actualizar estado de botones
        End Try
    End Sub

    Private Async Function CargarHistorialCompleto() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim repo = _uow.Repository(Of EstadoTransitorio)()
            Dim historial = Await repo.GetAll().
            Include(Function(et) et.TipoEstadoTransitorio).
            Include(Function(et) et.DesignacionDetalle).
            Include(Function(et) et.SancionDetalle).
            Include(Function(et) et.SumarioDetalle).
            Include(Function(et) et.OrdenCincoDetalle).
            Include(Function(et) et.EnfermedadDetalle).
            Include(Function(et) et.RetenDetalle).
            Where(Function(et) et.FuncionarioId = _idFuncionario).
            OrderByDescending(Function(et) et.Id).
            ToListAsync()

            _historialConsolidado = historial.Select(Function(h)
                                                         Dim fechaDesde As Date? = Nothing
                                                         Dim fechaHasta As Date? = Nothing
                                                         Dim observaciones As String = ""
                                                         Dim origen As String = "Estado"
                                                         Dim detallePrincipal As String = "" '--- Variable para el detalle clave

                                                         Select Case h.TipoEstadoTransitorioId
                                                             Case 1 ' Designación
                                                                 If h.DesignacionDetalle IsNot Nothing Then
                                                                     fechaDesde = h.DesignacionDetalle.FechaDesde
                                                                     fechaHasta = h.DesignacionDetalle.FechaHasta
                                                                     observaciones = h.DesignacionDetalle.Observaciones
                                                                     If Not String.IsNullOrWhiteSpace(h.DesignacionDetalle.DocResolucion) Then detallePrincipal = $"Resolución: {h.DesignacionDetalle.DocResolucion}"
                                                                 End If
                                                             Case 2 ' Enfermedad
                                                                 If h.EnfermedadDetalle IsNot Nothing Then
                                                                     fechaDesde = h.EnfermedadDetalle.FechaDesde
                                                                     fechaHasta = h.EnfermedadDetalle.FechaHasta
                                                                     observaciones = h.EnfermedadDetalle.Observaciones
                                                                     If Not String.IsNullOrWhiteSpace(h.EnfermedadDetalle.Diagnostico) Then detallePrincipal = $"Diagnóstico: {h.EnfermedadDetalle.Diagnostico}"
                                                                 End If
                                                             Case 3 ' Sanción
                                                                 If h.SancionDetalle IsNot Nothing Then
                                                                     fechaDesde = h.SancionDetalle.FechaDesde
                                                                     fechaHasta = h.SancionDetalle.FechaHasta
                                                                     observaciones = h.SancionDetalle.Observaciones
                                                                     If Not String.IsNullOrWhiteSpace(h.SancionDetalle.Resolucion) Then detallePrincipal = $"Resolución: {h.SancionDetalle.Resolucion}"
                                                                 End If
                                                             Case 4 ' Orden Cinco
                                                                 If h.OrdenCincoDetalle IsNot Nothing Then
                                                                     fechaDesde = h.OrdenCincoDetalle.FechaDesde
                                                                     fechaHasta = h.OrdenCincoDetalle.FechaHasta
                                                                     observaciones = h.OrdenCincoDetalle.Observaciones
                                                                 End If
                                                             Case 5 ' Retén
                                                                 If h.RetenDetalle IsNot Nothing Then
                                                                     fechaDesde = h.RetenDetalle.FechaReten
                                                                     fechaHasta = Nothing
                                                                     observaciones = h.RetenDetalle.Observaciones
                                                                     If Not String.IsNullOrWhiteSpace(h.RetenDetalle.Turno) Then detallePrincipal = $"Turno: {h.RetenDetalle.Turno}"
                                                                 End If
                                                             Case 6 ' Sumario
                                                                 If h.SumarioDetalle IsNot Nothing Then
                                                                     fechaDesde = h.SumarioDetalle.FechaDesde
                                                                     fechaHasta = h.SumarioDetalle.FechaHasta
                                                                     observaciones = h.SumarioDetalle.Observaciones
                                                                     If Not String.IsNullOrWhiteSpace(h.SumarioDetalle.Expediente) Then detallePrincipal = $"Expediente: {h.SumarioDetalle.Expediente}"
                                                                 End If
                                                         End Select

                                                         Dim obsFinal = If(String.IsNullOrWhiteSpace(detallePrincipal), observaciones, $"{detallePrincipal} | {observaciones}")

                                                         Return New With {
                .Id = h.Id,
                .Origen = origen,
                .TipoEstado = If(h.TipoEstadoTransitorio IsNot Nothing, h.TipoEstadoTransitorio.Nombre, "Desconocido"),
                .FechaDesde = fechaDesde,
                .FechaHasta = fechaHasta,
                .Observaciones = obsFinal '--- Usamos la observación enriquecida
            }
                                                     End Function).Cast(Of Object).ToList()

            dgvEstadosTransitorios.DataSource = _historialConsolidado
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function


    Private Sub DgvEstadosTransitorios_SelectionChanged(sender As Object, e As EventArgs)
        If _silenciarEventosEstados Then Return

        ' defensivo: puede no haber fila/índice válido durante refrescos
        If dgvEstadosTransitorios.RowCount = 0 _
       OrElse dgvEstadosTransitorios.CurrentCell Is Nothing _
       OrElse dgvEstadosTransitorios.CurrentCell.RowIndex < 0 _
       OrElse dgvEstadosTransitorios.CurrentCell.RowIndex >= dgvEstadosTransitorios.RowCount Then

            btnEditarEstado.Enabled = False
            btnQuitarEstado.Enabled = False
            btnAñadirEstado.Enabled = True
            Return
        End If

        Dim puedeEditarOQuitar As Boolean = False
        btnAñadirEstado.Enabled = True

        Dim itemSeleccionado As Object = dgvEstadosTransitorios.CurrentRow.DataBoundItem
        If itemSeleccionado Is Nothing Then
            btnEditarEstado.Enabled = False
            btnQuitarEstado.Enabled = False
            Return
        End If

        If Not chkVerHistorial.Checked Then
            puedeEditarOQuitar = True
        Else
            Dim origenProperty = itemSeleccionado.GetType().GetProperty("Origen")
            If origenProperty IsNot Nothing Then
                Dim origenValue = origenProperty.GetValue(itemSeleccionado, Nothing)
                If origenValue IsNot Nothing Then
                    puedeEditarOQuitar = String.Equals(origenValue.ToString(), "Estado", StringComparison.OrdinalIgnoreCase)
                End If
            End If
        End If

        btnEditarEstado.Enabled = puedeEditarOQuitar
        btnQuitarEstado.Enabled = puedeEditarOQuitar
    End Sub


    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If String.IsNullOrWhiteSpace(txtCI.Text) OrElse String.IsNullOrWhiteSpace(txtNombre.Text) OrElse cboTipoFuncionario.SelectedIndex = -1 Then
                MessageBox.Show("Los campos CI, Nombre y Tipo de Funcionario son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

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

            If Not String.IsNullOrWhiteSpace(_rutaFotoSeleccionada) Then
                _funcionario.Foto = File.ReadAllBytes(_rutaFotoSeleccionada)
            End If

            If _modo = ModoFormulario.Crear Then
                _funcionario.CreatedAt = DateTime.Now
            Else
                _funcionario.UpdatedAt = DateTime.Now
            End If

            SincronizarColeccion(_funcionario.FuncionarioDotacion, _dotaciones)
            SincronizarColeccion(_funcionario.EstadoTransitorio, _estadosTransitorios)

            Await _uow.CommitAsync()
            MessageBox.Show(If(_modo = ModoFormulario.Crear, "Funcionario creado", "Funcionario actualizado") & " correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

            DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SincronizarColeccion(Of T As Class)(dbCollection As ICollection(Of T), formCollection As IEnumerable(Of T))
        Dim itemsParaAnadir = formCollection.Except(dbCollection).ToList()
        For Each item In itemsParaAnadir
            dbCollection.Add(item)
        Next
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

            ' Unbound: el nombre del ítem lo resolvemos en CellFormatting
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "colItem",
            .HeaderText = "Ítem",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .ValueType = GetType(String)
        })

            ' Dejá que infiera el tipo real (no fuerces ValueType aquí)
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Talla", .HeaderText = "Talla"})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .HeaderText = "Observaciones", .Width = 200})

            ' Fecha: aclaramos que es Date para que no intente parsear strings
            Dim colFecha As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "FechaAsign",
            .Name = "FechaAsign",
            .HeaderText = "Fecha Asignación",
            .ValueType = GetType(Date)
        }
            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
            colFecha.DefaultCellStyle.NullValue = Nothing
            .Columns.Add(colFecha)

            .ResumeLayout()
        End With
    End Sub


    Private Sub ConfigurarGrillaEstados()
        With dgvEstadosTransitorios
            .SuspendLayout()

            .AutoGenerateColumns = False
            .Columns.Clear()

            ' >>> defensivo
            .AllowUserToAddRows = False   ' evita la "fila nueva" que dispara IndexOutOfRange
            .AllowUserToDeleteRows = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .RowHeadersVisible = False

            ' Tipo de estado
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "TipoEstado",
            .HeaderText = "Tipo de Estado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .ValueType = GetType(String),
            .SortMode = DataGridViewColumnSortMode.NotSortable
        })

            ' Desde
            Dim colDesde As New DataGridViewTextBoxColumn With {
            .Name = "FechaDesde",
            .HeaderText = "Desde",
            .Width = 100,
            .ValueType = GetType(Date)
        }
            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
            colDesde.DefaultCellStyle.NullValue = ""   ' muestra vacío si es Nothing
            .Columns.Add(colDesde)

            ' Hasta
            Dim colHasta As New DataGridViewTextBoxColumn With {
            .Name = "FechaHasta",
            .HeaderText = "Hasta",
            .Width = 100,
            .ValueType = GetType(Date)
        }
            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
            colHasta.DefaultCellStyle.NullValue = ""
            .Columns.Add(colHasta)

            ' Observaciones
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Observaciones",
            .HeaderText = "Observaciones / Detalles",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .ValueType = GetType(String),
            .SortMode = DataGridViewColumnSortMode.NotSortable
        })

            .ResumeLayout()
        End With
    End Sub


    Private Sub dgvDotacion_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv = CType(sender, DataGridView)

        If _silenciarEventosEstados Then Return
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        If dgv.Columns Is Nothing OrElse e.ColumnIndex >= dgv.Columns.Count Then Return

        Dim cm As CurrencyManager = TryCast(dgv.BindingContext(dgv.DataSource), CurrencyManager)
        If cm Is Nothing OrElse e.RowIndex >= cm.Count Then
            e.Value = Nothing : e.FormattingApplied = True : Return
        End If

        Dim colName = dgv.Columns(e.ColumnIndex).Name

        ' --- Ítem (como ya tenías)
        If colName = "colItem" Then
            Dim dataItem = TryCast(dgv.Rows(e.RowIndex).DataBoundItem, FuncionarioDotacion)
            If dataItem Is Nothing Then Return
            Dim item = _itemsDotacion?.FirstOrDefault(Function(i) i.Id = dataItem.DotacionItemId)
            e.Value = If(item IsNot Nothing, item.Nombre, String.Empty)
            e.FormattingApplied = True
            Return
        End If

        ' --- Fecha Asignación (nuevo)
        If colName = "FechaAsign" Then
            Dim raw = e.Value
            If raw Is Nothing OrElse raw Is DBNull.Value Then
                e.Value = "" : e.FormattingApplied = True : Return
            End If

            Dim dt As DateTime
            If TypeOf raw Is DateTime Then
                dt = CType(raw, DateTime)
                e.Value = dt.ToString("dd/MM/yyyy")
                e.FormattingApplied = True
            Else
                ' Intenta parsear string a fecha
                If DateTime.TryParse(Convert.ToString(raw), dt) Then
                    e.Value = dt.ToString("dd/MM/yyyy")
                    e.FormattingApplied = True
                Else
                    e.Value = "" : e.FormattingApplied = True
                End If
            End If
            Return
        End If

        ' --- (opcional) Talla tolerante si en el modelo fuera numérico y llega texto
        If colName = "Talla" Then
            e.Value = If(e.Value Is Nothing, "", e.Value.ToString())
            e.FormattingApplied = True
            Return
        End If
    End Sub


    Private Sub dgvEstadosTransitorios_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        Dim dgv = CType(sender, DataGridView)

        ' >>> salir si estamos re-enlazando / cambiando origen o el evento no es válido
        If _silenciarEventosEstados OrElse _cambiandoOrigen Then Return
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        If dgv.Columns Is Nothing OrElse e.ColumnIndex >= dgv.Columns.Count Then Return

        ' >>> usar CurrencyManager para validar el índice real de la lista enlazada
        Dim cm As CurrencyManager = TryCast(dgv.BindingContext(dgv.DataSource), CurrencyManager)
        If cm Is Nothing OrElse e.RowIndex >= cm.Count Then
            ' índice fuera de rango durante transición de DataSource: no formatear
            e.Value = Nothing
            e.FormattingApplied = True
            Return
        End If

        Dim colName = dgv.Columns(e.ColumnIndex).Name

        ' Ojo: DataBoundItem puede lanzar si el índice no existe; a esta altura ya validamos con cm.Count
        Dim dataItem As Object = Nothing
        Try
            dataItem = dgv.Rows(e.RowIndex).DataBoundItem
        Catch
            ' Si igualmente falla, mejor salimos silenciosamente
            e.Value = Nothing
            e.FormattingApplied = True
            Return
        End Try

        If dataItem Is Nothing Then Return

        If colName = "TipoEstado" OrElse colName = "Observaciones" OrElse colName = "FechaDesde" OrElse colName = "FechaHasta" Then
            Dim tipoEstado As String = ""
            Dim fechaDesde As Date? = Nothing
            Dim fechaHasta As Date? = Nothing
            Dim observaciones As String = ""

            If chkVerHistorial.Checked Then
                tipoEstado = CStr(dataItem.GetType().GetProperty("TipoEstado")?.GetValue(dataItem, Nothing))
                observaciones = CStr(dataItem.GetType().GetProperty("Observaciones")?.GetValue(dataItem, Nothing))
                Dim fd = dataItem.GetType().GetProperty("FechaDesde")?.GetValue(dataItem, Nothing)
                If fd IsNot Nothing AndAlso Not DBNull.Value.Equals(fd) Then fechaDesde = CDate(fd)
                Dim fh = dataItem.GetType().GetProperty("FechaHasta")?.GetValue(dataItem, Nothing)
                If fh IsNot Nothing AndAlso Not DBNull.Value.Equals(fh) Then fechaHasta = CDate(fh)
            Else
                Dim estado = TryCast(dataItem, EstadoTransitorio)
                If estado Is Nothing OrElse estado.TipoEstadoTransitorioId = 0 Then Return

                Dim tipo = _tiposEstadoTransitorio.FirstOrDefault(Function(t) t.Id = estado.TipoEstadoTransitorioId)
                tipoEstado = If(tipo IsNot Nothing, tipo.Nombre, "")
                Dim detallePrincipal As String = ""

                Select Case estado.TipoEstadoTransitorioId
                    Case 1
                        If estado.DesignacionDetalle IsNot Nothing Then
                            fechaDesde = estado.DesignacionDetalle.FechaDesde
                            fechaHasta = estado.DesignacionDetalle.FechaHasta
                            observaciones = estado.DesignacionDetalle.Observaciones
                            If Not String.IsNullOrWhiteSpace(estado.DesignacionDetalle.DocResolucion) Then detallePrincipal = $"Resolución: {estado.DesignacionDetalle.DocResolucion}"
                        End If
                    Case 2
                        If estado.EnfermedadDetalle IsNot Nothing Then
                            fechaDesde = estado.EnfermedadDetalle.FechaDesde
                            fechaHasta = estado.EnfermedadDetalle.FechaHasta
                            observaciones = estado.EnfermedadDetalle.Observaciones
                            If Not String.IsNullOrWhiteSpace(estado.EnfermedadDetalle.Diagnostico) Then detallePrincipal = $"Diagnóstico: {estado.EnfermedadDetalle.Diagnostico}"
                        End If
                    Case 3
                        If estado.SancionDetalle IsNot Nothing Then
                            fechaDesde = estado.SancionDetalle.FechaDesde
                            fechaHasta = estado.SancionDetalle.FechaHasta
                            observaciones = estado.SancionDetalle.Observaciones
                            If Not String.IsNullOrWhiteSpace(estado.SancionDetalle.Resolucion) Then detallePrincipal = $"Resolución: {estado.SancionDetalle.Resolucion}"
                        End If
                    Case 4
                        If estado.OrdenCincoDetalle IsNot Nothing Then
                            fechaDesde = estado.OrdenCincoDetalle.FechaDesde
                            fechaHasta = estado.OrdenCincoDetalle.FechaHasta
                            observaciones = estado.OrdenCincoDetalle.Observaciones
                        End If
                    Case 5
                        If estado.RetenDetalle IsNot Nothing Then
                            fechaDesde = estado.RetenDetalle.FechaReten
                            fechaHasta = Nothing
                            observaciones = estado.RetenDetalle.Observaciones
                            If Not String.IsNullOrWhiteSpace(estado.RetenDetalle.Turno) Then detallePrincipal = $"Turno: {estado.RetenDetalle.Turno}"
                        End If
                    Case 6
                        If estado.SumarioDetalle IsNot Nothing Then
                            fechaDesde = estado.SumarioDetalle.FechaDesde
                            fechaHasta = estado.SumarioDetalle.FechaHasta
                            observaciones = estado.SumarioDetalle.Observaciones
                            If Not String.IsNullOrWhiteSpace(estado.SumarioDetalle.Expediente) Then detallePrincipal = $"Expediente: {estado.SumarioDetalle.Expediente}"
                        End If
                End Select

                observaciones = If(String.IsNullOrWhiteSpace(detallePrincipal), observaciones, $"{detallePrincipal} | {observaciones}")
            End If

            Select Case colName
                Case "TipoEstado"
                    e.Value = tipoEstado
                    e.FormattingApplied = True
                Case "Observaciones"
                    e.Value = If(String.IsNullOrEmpty(observaciones), String.Empty, observaciones)
                    e.FormattingApplied = True
                Case "FechaDesde"
                    e.Value = If(fechaDesde.HasValue, CType(fechaDesde.Value, Object), Nothing)
                Case "FechaHasta"
                    e.Value = If(fechaHasta.HasValue, CType(fechaHasta.Value, Object), Nothing)
            End Select
        End If
    End Sub

    'Private Sub dgvEstadosTransitorios_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
    '    Dim dgv = CType(sender, DataGridView)


    '    If _cambiandoOrigen OrElse e.RowIndex < 0 OrElse e.RowIndex >= dgv.RowCount OrElse e.ColumnIndex < 0 Then Return

    '    Dim colName = dgv.Columns(e.ColumnIndex).Name
    '    Dim dataItem = dgv.Rows(e.RowIndex).DataBoundItem
    '    If dataItem Is Nothing Then Return

    '    ' Solo manejamos las columnas que necesitan una lógica de extracción de datos compleja.
    '    If colName = "TipoEstado" OrElse colName = "Observaciones" OrElse colName = "FechaDesde" OrElse colName = "FechaHasta" Then
    '        Dim tipoEstado As String = ""
    '        Dim fechaDesde As Date? = Nothing
    '        Dim fechaHasta As Date? = Nothing
    '        Dim observaciones As String = ""

    '        ' Lógica para obtener los valores según si la vista de historial está activa o no.
    '        If chkVerHistorial.Checked Then
    '            tipoEstado = CStr(dataItem.GetType().GetProperty("TipoEstado")?.GetValue(dataItem, Nothing))
    '            observaciones = CStr(dataItem.GetType().GetProperty("Observaciones")?.GetValue(dataItem, Nothing))
    '            Dim fechaDesdeObj = dataItem.GetType().GetProperty("FechaDesde")?.GetValue(dataItem, Nothing)
    '            If fechaDesdeObj IsNot Nothing AndAlso Not DBNull.Value.Equals(fechaDesdeObj) Then fechaDesde = CDate(fechaDesdeObj)
    '            Dim fechaHastaObj = dataItem.GetType().GetProperty("FechaHasta")?.GetValue(dataItem, Nothing)
    '            If fechaHastaObj IsNot Nothing AndAlso Not DBNull.Value.Equals(fechaHastaObj) Then fechaHasta = CDate(fechaHastaObj)
    '        Else
    '            Dim estado = TryCast(dataItem, EstadoTransitorio)
    '            If estado Is Nothing OrElse estado.TipoEstadoTransitorioId = 0 Then Return
    '            Dim tipo = _tiposEstadoTransitorio.FirstOrDefault(Function(t) t.Id = estado.TipoEstadoTransitorioId)
    '            tipoEstado = If(tipo IsNot Nothing, tipo.Nombre, "")
    '            Dim detallePrincipal As String = ""
    '            Select Case estado.TipoEstadoTransitorioId
    '                Case 1 ' Designación
    '                    If estado.DesignacionDetalle IsNot Nothing Then
    '                        fechaDesde = estado.DesignacionDetalle.FechaDesde
    '                        fechaHasta = estado.DesignacionDetalle.FechaHasta
    '                        observaciones = estado.DesignacionDetalle.Observaciones
    '                        If Not String.IsNullOrWhiteSpace(estado.DesignacionDetalle.DocResolucion) Then detallePrincipal = $"Resolución: {estado.DesignacionDetalle.DocResolucion}"
    '                    End If
    '                Case 2 ' Enfermedad
    '                    If estado.EnfermedadDetalle IsNot Nothing Then
    '                        fechaDesde = estado.EnfermedadDetalle.FechaDesde
    '                        fechaHasta = estado.EnfermedadDetalle.FechaHasta
    '                        observaciones = estado.EnfermedadDetalle.Observaciones
    '                        If Not String.IsNullOrWhiteSpace(estado.EnfermedadDetalle.Diagnostico) Then detallePrincipal = $"Diagnóstico: {estado.EnfermedadDetalle.Diagnostico}"
    '                    End If
    '                Case 3 ' Sanción
    '                    If estado.SancionDetalle IsNot Nothing Then
    '                        fechaDesde = estado.SancionDetalle.FechaDesde
    '                        fechaHasta = estado.SancionDetalle.FechaHasta
    '                        observaciones = estado.SancionDetalle.Observaciones
    '                        If Not String.IsNullOrWhiteSpace(estado.SancionDetalle.Resolucion) Then detallePrincipal = $"Resolución: {estado.SancionDetalle.Resolucion}"
    '                    End If
    '                Case 4 ' Orden Cinco
    '                    If estado.OrdenCincoDetalle IsNot Nothing Then
    '                        fechaDesde = estado.OrdenCincoDetalle.FechaDesde
    '                        fechaHasta = estado.OrdenCincoDetalle.FechaHasta
    '                        observaciones = estado.OrdenCincoDetalle.Observaciones
    '                    End If
    '                Case 5 ' Retén
    '                    If estado.RetenDetalle IsNot Nothing Then
    '                        fechaDesde = estado.RetenDetalle.FechaReten
    '                        fechaHasta = Nothing
    '                        observaciones = estado.RetenDetalle.Observaciones
    '                        If Not String.IsNullOrWhiteSpace(estado.RetenDetalle.Turno) Then detallePrincipal = $"Turno: {estado.RetenDetalle.Turno}"
    '                    End If
    '                Case 6 ' Sumario
    '                    If estado.SumarioDetalle IsNot Nothing Then
    '                        fechaDesde = estado.SumarioDetalle.FechaDesde
    '                        fechaHasta = estado.SumarioDetalle.FechaHasta
    '                        observaciones = estado.SumarioDetalle.Observaciones
    '                        If Not String.IsNullOrWhiteSpace(estado.SumarioDetalle.Expediente) Then detallePrincipal = $"Expediente: {estado.SumarioDetalle.Expediente}"
    '                    End If
    '            End Select
    '            observaciones = If(String.IsNullOrWhiteSpace(detallePrincipal), observaciones, $"{detallePrincipal} | {observaciones}")
    '        End If

    '        ' Asignar los valores a las celdas correspondientes.
    '        Select Case colName
    '            Case "TipoEstado"
    '                e.Value = tipoEstado
    '                e.FormattingApplied = True
    '            Case "Observaciones"
    '                e.Value = If(String.IsNullOrEmpty(observaciones), String.Empty, observaciones)
    '                e.FormattingApplied = True
    '            Case "FechaDesde"
    '                e.Value = If(fechaDesde.HasValue, CType(fechaDesde.Value, Object), Nothing)
    '            Case "FechaHasta"
    '                e.Value = If(fechaHasta.HasValue, CType(fechaHasta.Value, Object), Nothing)
    '        End Select
    '    End If
    'End Sub


    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub btnSeleccionarFoto_Click(sender As Object, e As EventArgs) Handles btnSeleccionarFoto.Click
        Using ofd As New OpenFileDialog() With {.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp"}
            If ofd.ShowDialog() = DialogResult.OK Then
                _rutaFotoSeleccionada = ofd.FileName
                pbFoto.Image = Image.FromFile(ofd.FileName)
            End If
        End Using
    End Sub
#End Region

#Region "CRUD Dotación"
    Private Sub btnAñadirDotacion_Click(sender As Object, e As EventArgs) Handles btnAñadirDotacion.Click
        Dim nuevaDotacion = New FuncionarioDotacion()
        Using frm As New frmFuncionarioDotacion(nuevaDotacion)
            If frm.ShowDialog() = DialogResult.OK Then
                _dotaciones.Add(frm.Dotacion)
            End If
        End Using
    End Sub

    Private Sub btnEditarDotacion_Click(sender As Object, e As EventArgs) Handles btnEditarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        Dim dotacionSeleccionada = CType(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
        Using frm As New frmFuncionarioDotacion(dotacionSeleccionada)
            If frm.ShowDialog() = DialogResult.OK Then
                _dotaciones.ResetBindings()
            End If
        End Using
    End Sub

    ' Reemplaza el método existente con este
    Private Async Sub btnQuitarDotacion_Click(sender As Object, e As EventArgs) Handles btnQuitarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        Dim dotacionSeleccionada = TryCast(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
        If dotacionSeleccionada Is Nothing Then Return

        If MessageBox.Show("¿Está seguro de que desea quitar este elemento de dotación?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                ' 1. Quitar de la lista de la UI para un refresco visual inmediato.
                _dotaciones.Remove(dotacionSeleccionada)

                ' 2. Si es un item que ya existe en la BD (Id > 0), eliminarlo de forma permanente.
                ' Si es un item nuevo (Id = 0), simplemente se quita de la colección en memoria.
                If dotacionSeleccionada.Id > 0 Then
                    ' Se usa el servicio genérico para encapsular la lógica de eliminación y guardado.
                    Dim svcDotacion As New GenericService(Of FuncionarioDotacion)(_uow)
                    Await svcDotacion.DeleteAsync(dotacionSeleccionada.Id)
                End If

                ' 3. Asegurarse de quitarlo también de la colección principal del funcionario.
                If _funcionario.FuncionarioDotacion.Contains(dotacionSeleccionada) Then
                    _funcionario.FuncionarioDotacion.Remove(dotacionSeleccionada)
                End If

            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al quitar la dotación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ' Si la eliminación falla, volvemos a agregar el item a la lista para mantener la consistencia visual.
                _dotaciones.Add(dotacionSeleccionada)
            End Try
        End If
    End Sub
#End Region

#Region "CRUD Estados Transitorios"
    Private Sub btnAñadirEstado_Click(sender As Object, e As EventArgs) Handles btnAñadirEstado.Click
        Dim nuevoEstado = New EstadoTransitorio()

        Using frm As New frmFuncionarioEstadoTransitorio(nuevoEstado, _tiposEstadoTransitorio)
            If frm.ShowDialog() = DialogResult.OK Then
                ' Añadimos el nuevo estado a la colección principal del funcionario para el guardado final
                _funcionario.EstadoTransitorio.Add(frm.Estado)

                ' Si no estamos en la vista de historial y el nuevo estado está activo,
                ' lo añadimos directamente a la lista enlazada para un refresco inmediato.
                If Not chkVerHistorial.Checked AndAlso IsEstadoActivo(frm.Estado) Then
                    _estadosTransitorios.Add(frm.Estado)
                ElseIf chkVerHistorial.Checked Then
                    ' Si estamos en vista de historial, es necesario recargar todo para mantener el orden y la consistencia.
                    chkVerHistorial_CheckedChanged(Nothing, EventArgs.Empty)
                End If
            End If
        End Using
    End Sub

    Private Sub btnEditarEstado_Click(sender As Object, e As EventArgs) Handles btnEditarEstado.Click
        If dgvEstadosTransitorios.CurrentRow Is Nothing OrElse dgvEstadosTransitorios.CurrentRow.DataBoundItem Is Nothing Then Return

        Dim estadoParaEditar As EstadoTransitorio = Nothing
        Dim id As Integer = 0
        Dim index As Integer = dgvEstadosTransitorios.CurrentRow.Index

        If chkVerHistorial.Checked Then
            Dim itemSeleccionado = dgvEstadosTransitorios.CurrentRow.DataBoundItem
            If itemSeleccionado IsNot Nothing AndAlso itemSeleccionado.GetType().GetProperty("Id") IsNot Nothing Then
                id = CInt(itemSeleccionado.GetType().GetProperty("Id").GetValue(itemSeleccionado, Nothing))
                estadoParaEditar = _funcionario.EstadoTransitorio.FirstOrDefault(Function(et) et.Id = id)
            End If
        Else
            estadoParaEditar = TryCast(dgvEstadosTransitorios.CurrentRow.DataBoundItem, EstadoTransitorio)
        End If

        If estadoParaEditar Is Nothing Then
            MessageBox.Show("El registro seleccionado no se pudo encontrar para editar o no es editable.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Using frm As New frmFuncionarioEstadoTransitorio(estadoParaEditar, _tiposEstadoTransitorio)
            If frm.ShowDialog() = DialogResult.OK Then
                ' Si estamos en vista de historial, recargamos todo
                If chkVerHistorial.Checked Then
                    chkVerHistorial_CheckedChanged(Nothing, EventArgs.Empty)
                Else
                    ' Si estamos en la vista de activos, actualizamos la fila o la quitamos
                    If IsEstadoActivo(estadoParaEditar) Then
                        _estadosTransitorios.ResetItem(index)
                    Else
                        _estadosTransitorios.RemoveAt(index)
                    End If
                End If
            End If
        End Using
    End Sub

    Private Async Sub btnQuitarEstado_Click(sender As Object, e As EventArgs) Handles btnQuitarEstado.Click
        If dgvEstadosTransitorios.CurrentRow Is Nothing Then Return

        Dim oldIndex As Integer = dgvEstadosTransitorios.CurrentRow.Index
        Dim estadoParaQuitar As EstadoTransitorio = Nothing
        Dim id As Integer = 0

        ' Resolver seleccionado según vista
        If chkVerHistorial.Checked Then
            Dim itemSel = dgvEstadosTransitorios.CurrentRow.DataBoundItem
            If itemSel IsNot Nothing Then
                Dim propId = itemSel.GetType().GetProperty("Id")
                If propId IsNot Nothing Then
                    id = CInt(propId.GetValue(itemSel, Nothing))
                    estadoParaQuitar = _funcionario.EstadoTransitorio.FirstOrDefault(Function(et) et.Id = id)
                End If
            End If
        Else
            estadoParaQuitar = TryCast(dgvEstadosTransitorios.CurrentRow.DataBoundItem, EstadoTransitorio)
            If estadoParaQuitar IsNot Nothing Then id = estadoParaQuitar.Id
        End If

        If estadoParaQuitar Is Nothing Then
            MessageBox.Show("El registro seleccionado no se pudo encontrar para eliminar o no es editable.",
                            "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show("¿Está seguro de que desea quitar este estado transitorio?",
                           "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        _silenciarEventosEstados = True
        Try
            ' --- 1) Sacar de las fuentes de la grilla ---
            If chkVerHistorial.Checked Then
                Dim itemHistory = _historialConsolidado.FirstOrDefault(Function(item)
                                                                           Dim p = item.GetType().GetProperty("Id")
                                                                           Dim itemId As Integer = If(p Is Nothing OrElse p.GetValue(item, Nothing) Is Nothing, 0, CInt(p.GetValue(item, Nothing)))
                                                                           Return itemId = id
                                                                       End Function)
                If itemHistory IsNot Nothing Then
                    _historialConsolidado.Remove(itemHistory)
                    dgvEstadosTransitorios.DataSource = Nothing
                    dgvEstadosTransitorios.DataSource = _historialConsolidado
                End If
            Else
                _estadosTransitorios.Remove(estadoParaQuitar) ' BindingList refresca sola
            End If

            ' --- 2) Romper vínculos y eliminar detalles primero ---
            If _funcionario.EstadoTransitorio.Contains(estadoParaQuitar) Then
                _funcionario.EstadoTransitorio.Remove(estadoParaQuitar)
            End If

            If estadoParaQuitar.DesignacionDetalle IsNot Nothing Then
                _uow.Context.Set(Of DesignacionDetalle)().Remove(estadoParaQuitar.DesignacionDetalle)
                estadoParaQuitar.DesignacionDetalle = Nothing
            End If
            If estadoParaQuitar.EnfermedadDetalle IsNot Nothing Then
                _uow.Context.Set(Of EnfermedadDetalle)().Remove(estadoParaQuitar.EnfermedadDetalle)
                estadoParaQuitar.EnfermedadDetalle = Nothing
            End If
            If estadoParaQuitar.SancionDetalle IsNot Nothing Then
                _uow.Context.Set(Of SancionDetalle)().Remove(estadoParaQuitar.SancionDetalle)
                estadoParaQuitar.SancionDetalle = Nothing
            End If
            If estadoParaQuitar.OrdenCincoDetalle IsNot Nothing Then
                _uow.Context.Set(Of OrdenCincoDetalle)().Remove(estadoParaQuitar.OrdenCincoDetalle)
                estadoParaQuitar.OrdenCincoDetalle = Nothing
            End If
            If estadoParaQuitar.RetenDetalle IsNot Nothing Then
                _uow.Context.Set(Of RetenDetalle)().Remove(estadoParaQuitar.RetenDetalle)
                estadoParaQuitar.RetenDetalle = Nothing
            End If
            If estadoParaQuitar.SumarioDetalle IsNot Nothing Then
                _uow.Context.Set(Of SumarioDetalle)().Remove(estadoParaQuitar.SumarioDetalle)
                estadoParaQuitar.SumarioDetalle = Nothing
            End If

            ' Cortar navs que podrían re-vincular
            estadoParaQuitar.Funcionario = Nothing
            estadoParaQuitar.TipoEstadoTransitorio = Nothing

            ' --- 3) Eliminar el padre desde el DbSet ---
            _uow.Context.Set(Of EstadoTransitorio)().Remove(estadoParaQuitar)

            ' --- 4) Guardar YA para evitar re-fixup posterior ---
            Await _uow.CommitAsync()

            ' --- 5) Reposicionar selección si quedan filas ---
            If dgvEstadosTransitorios.RowCount > 0 AndAlso dgvEstadosTransitorios.Columns.Count > 0 Then
                Dim nuevoIndex = Math.Min(oldIndex, dgvEstadosTransitorios.RowCount - 1)
                Dim firstVisibleCol As Integer = 0
                For i = 0 To dgvEstadosTransitorios.Columns.Count - 1
                    If dgvEstadosTransitorios.Columns(i).Visible Then firstVisibleCol = i : Exit For
                Next
                dgvEstadosTransitorios.ClearSelection()
                dgvEstadosTransitorios.CurrentCell = dgvEstadosTransitorios.Rows(nuevoIndex).Cells(firstVisibleCol)
                dgvEstadosTransitorios.Rows(nuevoIndex).Selected = True
            Else
                btnEditarEstado.Enabled = False
                btnQuitarEstado.Enabled = False
            End If

        Catch ex As Exception
            MessageBox.Show("No se pudo eliminar el estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _silenciarEventosEstados = False
            BeginInvoke(CType(Sub() DgvEstadosTransitorios_SelectionChanged(Nothing, EventArgs.Empty), MethodInvoker))
        End Try
    End Sub



#End Region

End Class