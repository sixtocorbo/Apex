Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Data.Entity
Imports System.IO
Imports System.Linq.Expressions

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
        ConfigurarLayoutResponsivo()
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
        dgvDotacion.ActivarDobleBuffer(True)          ' <-- LÍNEA AÑADIDA
        dgvEstadosTransitorios.ActivarDobleBuffer(True) ' <-- LÍNEA AÑADIDA
        Try
            AppTheme.SetCue(txtCI, "Cédula de Identidad")
            AppTheme.SetCue(txtNombre, "Nombre completo")
            AppTheme.SetCue(txtDomicilio, "Domicilio")
            AppTheme.SetCue(txtEmail, "Correo electrónico")
            AppTheme.SetCue(txtTelefono, "Teléfono")
            AppTheme.SetCue(txtCiudad, "Ciudad")
            AppTheme.SetCue(txtSeccional, "Seccional")
            AppTheme.SetCue(txtCredencial, "Número de credencial")
            AppTheme.SetCue(cboTipoFuncionario, "Seleccione un tipo de funcionario")
            AppTheme.SetCue(cboCargo, "Seleccione un cargo")
            AppTheme.SetCue(cboEscalafon, "Seleccione un escalafón")
            AppTheme.SetCue(cboFuncion, "Seleccione una función")
            AppTheme.SetCue(cboEstadoCivil, "Seleccione un estado civil")
            AppTheme.SetCue(cboGenero, "Seleccione un género")
            AppTheme.SetCue(cboNivelEstudio, "Seleccione un nivel de estudio")
            AppTheme.SetCue(cboSeccion, "Seleccione una sección")
            AppTheme.SetCue(cboPuestoTrabajo, "Seleccione un puesto de trabajo")
            AppTheme.SetCue(cboTurno, "Seleccione un turno")
            AppTheme.SetCue(cboSemana, "Seleccione una semana")
            AppTheme.SetCue(cboHorario, "Seleccione un horario")
            AppTheme.SetCue(cboSubDireccion, "Seleccione una subdirección")
            AppTheme.SetCue(cboSubEscalafon, "Seleccione un subescalafón")
            AppTheme.SetCue(cboPrestadorSalud, "Seleccione un prestador de salud")

        Catch
            ' Ignorar si no existe SetCue
        End Try
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
            pbFoto.Image = My.Resources.Police.ToBitmap
        End If

    End Sub
    ' Llamar después de InitializeComponent()
    Private Sub ConfigurarLayoutResponsivo()
        ' ===============================
        ' 1) Botonera superior (Auditoría / Guardar / Cancelar)
        ' ===============================
        With flowlayoutPanelBotones
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
            .Dock = DockStyle.Top
            .FlowDirection = FlowDirection.RightToLeft     ' Alinea a la derecha
            .WrapContents = True                           ' Permite salto de línea en chico
            .AutoScroll = False
            .Padding = New Padding(0, 0, 12, 6)           ' Respiro a la derecha/abajo
        End With

        ' Botones: que crezcan en alto/coloquen márgenes consistentes
        For Each b In New Button() {btnAuditoria, btnGuardar, btnCancelar}
            b.AutoSize = True
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink
            b.MinimumSize = New Size(100, 36)             ' Evita que queden demasiado chicos
            b.Margin = New Padding(6)                     ' Espacio entre botones
            b.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Next

        ' Si el orden visual no queda como querés con RightToLeft,
        ' asegurá posiciones (de derecha a izquierda):
        flowlayoutPanelBotones.Controls.SetChildIndex(btnCancelar, 0)
        flowlayoutPanelBotones.Controls.SetChildIndex(btnGuardar, 1)
        flowlayoutPanelBotones.Controls.SetChildIndex(btnAuditoria, 2)

        ' ===============================
        ' 2) Estados Transitorios (botones abajo)
        ' ===============================
        ' Asegurate que las últimas dos filas del TableLayout sean AutoSize (ya lo tenés),
        ' y que el DataGrid esté en la fila Percent 100%.
        With TableLayoutPanelEstados
            .RowStyles(0).SizeType = SizeType.Percent
            .RowStyles(0).Height = 100
            .RowStyles(1).SizeType = SizeType.AutoSize     ' chkVerHistorial
            .RowStyles(2).SizeType = SizeType.AutoSize     ' FlowLayoutPanelEstados
            .Dock = DockStyle.Fill
        End With

        With FlowLayoutPanelEstados
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
            .Dock = DockStyle.Fill                         ' Toma el ancho disponible (importante para calcular wraps)
            .FlowDirection = FlowDirection.RightToLeft     ' Botonera pegada a la derecha
            .WrapContents = True                           ' En pantallas chicas, salta de línea
            .AutoScroll = False                            ' No hace falta scroll si usamos AutoSize en fila
            .Padding = New Padding(0, 4, 0, 0)
            .Margin = New Padding(0, 6, 0, 0)
        End With

        For Each b In New Button() {btnQuitarEstado, btnEditarEstado, btnAñadirEstado}
            b.AutoSize = True
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink
            b.MinimumSize = New Size(96, 32)
            b.Margin = New Padding(6)
            b.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Next

        ' ===============================
        ' 3) Dotación (botones abajo)
        ' ===============================
        With TableLayoutPanelDotacion
            .RowStyles(0).SizeType = SizeType.Percent
            .RowStyles(0).Height = 100                    ' Grilla: ocupa el resto
            .RowStyles(1).SizeType = SizeType.AutoSize    ' Botonera: crece según contenido
            .Dock = DockStyle.Fill
        End With

        With FlowLayoutPanelDotacion
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
            .Dock = DockStyle.Fill
            .FlowDirection = FlowDirection.RightToLeft
            .WrapContents = True
            .AutoScroll = False
            .Padding = New Padding(0, 4, 0, 0)
            .Margin = New Padding(0, 6, 0, 0)
        End With

        For Each b In New Button() {btnQuitarDotacion, btnEditarDotacion, btnAñadirDotacion}
            b.AutoSize = True
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink
            b.MinimumSize = New Size(96, 30)
            b.Margin = New Padding(6)
            b.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Next

        ' ===============================
        ' 4) Ajustes generales del Form
        ' ===============================
        ' Permití reducir un poco el tamaño mínimo si querés favorecer los wraps:
        If Me.MinimumSize.Width > 900 Then
            Me.MinimumSize = New Size(900, Math.Max(600, Me.MinimumSize.Height))
        End If

        ' El TabControl ocupa todo y recalcula correctamente los anchos
        TabControlMain.Dock = DockStyle.Fill

        ' La grilla siempre estirada
        dgvEstadosTransitorios.Dock = DockStyle.Fill
        dgvDotacion.Dock = DockStyle.Fill
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
        SetSelectedOrNone(cboTipoFuncionario, _funcionario.TipoFuncionarioId)        ' no-nullable
        SetSelectedOrNone(cboCargo, _funcionario.CargoId)                       ' nullable
        SetSelectedOrNone(cboEscalafon, _funcionario.EscalafonId)
        SetSelectedOrNone(cboFuncion, _funcionario.FuncionId)
        SetSelectedOrNone(cboEstadoCivil, _funcionario.EstadoCivilId)
        SetSelectedOrNone(cboGenero, _funcionario.GeneroId)
        SetSelectedOrNone(cboNivelEstudio, _funcionario.NivelEstudioId)
        SetSelectedOrNone(cboSeccion, _funcionario.SeccionId)
        SetSelectedOrNone(cboPuestoTrabajo, _funcionario.PuestoTrabajoId)
        SetSelectedOrNone(cboTurno, _funcionario.TurnoId)
        SetSelectedOrNone(cboSemana, _funcionario.SemanaId)
        SetSelectedOrNone(cboHorario, _funcionario.HorarioId)
        SetSelectedOrNone(cboSubDireccion, _funcionario.SubDireccionId)
        SetSelectedOrNone(cboSubEscalafon, _funcionario.SubEscalafonId)
        SetSelectedOrNone(cboPrestadorSalud, _funcionario.PrestadorSaludId)

        ' Foto
        If _funcionario.Foto IsNot Nothing AndAlso _funcionario.Foto.Length > 0 Then
            Using ms As New MemoryStream(_funcionario.Foto)
                If pbFoto.Image IsNot Nothing Then pbFoto.Image.Dispose()
                pbFoto.Image = New Bitmap(ms)
            End Using
        Else
            pbFoto.Image = My.Resources.Police.ToBitmap()
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
                    addPart("Resolución", d.Resolucion)
                    addDate("Fecha res.", d.FechaResolucion)
                End If

            Case TiposEstadoCatalog.BajaDeFuncionario
                Dim d = e.BajaDeFuncionarioDetalle
                If d IsNot Nothing Then
                    obs = d.Observaciones
                    addPart("Resolución", d.Resolucion)
                    addDate("Fecha res.", d.FechaResolucion)
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
                    addPart("Resolución", d.Resolucion)
                    addDate("Fecha res.", d.FechaResolucion)
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
                    addPart("Resolución", d.Resolucion)
                    addDate("Fecha res.", d.FechaResolucion)
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

            Dim mensajeExito As String = If(_modo = ModoFormulario.Crear, "Funcionario creado", "Funcionario actualizado") & " correctamente."
            Notifier.Success(Me, mensajeExito)

            Me.DialogResult = DialogResult.OK
            _cerrandoPorCodigo = True
            Close()
        Else
            LoadingHelper.OcultarCargando(Me)
            ' Opcional: También puedes notificar el error aquí si GuardarAsync devuelve False
            Notifier.Error(Me, "No se pudo guardar el funcionario.")
        End If
    End Sub

    ' Archivo: UI/frmFuncionarioCrear.vb

    Private Async Function GuardarAsync() As Task(Of Boolean)
        Try
            If Not ValidarDatos() Then Return False

            ' 1. El mapeo y la sincronización se mantienen en el form
            MapearControlesAFuncionario()
            SincronizarEstados()

            ' 2. Se llama al nuevo método del servicio para que él se encargue de guardar y notificar
            Await _svc.GuardarFuncionarioAsync(_funcionario, _uow)

            _seGuardo = True
            Return True
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al intentar guardar los datos.")
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
        Dim cargoSeleccionado As Integer? = Nothing
        If cboCargo.SelectedIndex <> -1 Then
            Dim selectedValue = cboCargo.SelectedValue
            If selectedValue IsNot Nothing Then
                Dim valor As Integer
                If TypeOf selectedValue Is Integer Then
                    valor = CInt(selectedValue)
                    cargoSeleccionado = valor
                ElseIf Integer.TryParse(selectedValue.ToString(), valor) Then
                    cargoSeleccionado = valor
                End If
            End If
        End If
        If cargoSeleccionado.HasValue Then
            _funcionario.CargoId = cargoSeleccionado.Value
        End If
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
        _funcionario.SeccionId = If(cboSeccion.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboSeccion.SelectedValue))
        _funcionario.PuestoTrabajoId = If(cboPuestoTrabajo.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboPuestoTrabajo.SelectedValue))
        _funcionario.TurnoId = If(cboTurno.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboTurno.SelectedValue))
        _funcionario.SemanaId = If(cboSemana.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboSemana.SelectedValue))
        _funcionario.HorarioId = If(cboHorario.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboHorario.SelectedValue))
        _funcionario.Ciudad = txtCiudad.Text.Trim()
        _funcionario.Seccional = txtSeccional.Text.Trim()
        _funcionario.Credencial = txtCredencial.Text.Trim()
        _funcionario.Estudia = chkEstudia.Checked
        _funcionario.SubDireccionId = If(cboSubDireccion.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboSubDireccion.SelectedValue))
        _funcionario.SubEscalafonId = If(cboSubEscalafon.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboSubEscalafon.SelectedValue))
        _funcionario.PrestadorSaludId = If(cboPrestadorSalud.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboPrestadorSalud.SelectedValue))
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
    ' En frmFuncionarioCrear.vb

    ' En frmFuncionarioCrear.vb

    Private Sub btnAñadirEstado_Click(sender As Object, e As EventArgs) Handles btnAñadirEstado.Click
        ' 1. Preparas el nuevo estado con sus parámetros
        Dim nuevoEstado = New EstadoTransitorio() With {
        .Funcionario = _funcionario
    }

        ' 2. Creas la instancia del formulario hijo
        Dim frm As New frmFuncionarioEstadoTransitorio(nuevoEstado, _tiposEstadoTransitorio, _uow)

        ' 3. Te suscribes a su evento para recibir la respuesta
        AddHandler frm.EstadoConfigurado, AddressOf FrmEstado_EstadoConfigurado_Nuevo

        ' 4. Usas tu helper para abrir el formulario en el Dashboard
        AbrirChildEnDashboard(frm)
    End Sub



    ' Este método se ejecutará cuando el hijo (abierto para AÑADIR) 
    ' dispare el evento "EstadoConfigurado"
    Private Sub FrmEstado_EstadoConfigurado_Nuevo(estado As EstadoTransitorio)
        ' a) Actualizas la grilla.
        _estadoRows.Add(BuildEstadoRow(estado, "Nuevo"))
        bsEstados.ResetBindings(False)

        If estado?.TipoEstadoTransitorioId = ModConstantesApex.TipoEstadoTransitorioId.CambioDeCargo Then
            Dim detalle = estado.CambioDeCargoDetalle
            If detalle IsNot Nothing AndAlso detalle.CargoNuevoId > 0 Then
                cboCargo.SelectedValue = detalle.CargoNuevoId
            End If
        End If

        ' b) ¡NADA MÁS! El Dashboard se encargará de volver a este formulario.
    End Sub

    Private Sub btnEditarEstado_Click(sender As Object, e As EventArgs) Handles btnEditarEstado.Click
        Dim row = TryCast(dgvEstadosTransitorios.CurrentRow?.DataBoundItem, EstadoRow)
        If row?.EntityRef Is Nothing Then Return

        Dim frm As New frmFuncionarioEstadoTransitorio(row.EntityRef, _tiposEstadoTransitorio, _uow)

        AddHandler frm.EstadoConfigurado, Sub(estadoModificado As EstadoTransitorio)
                                              UpdateRowFromEntity(row, estadoModificado)
                                              bsEstados.ResetBindings(False)

                                              If estadoModificado?.TipoEstadoTransitorioId = ModConstantesApex.TipoEstadoTransitorioId.CambioDeCargo Then
                                                  Dim detalle = estadoModificado.CambioDeCargoDetalle
                                                  If detalle IsNot Nothing AndAlso detalle.CargoNuevoId > 0 Then
                                                      cboCargo.SelectedValue = detalle.CargoNuevoId
                                                  End If
                                              End If
                                          End Sub

        AbrirChildEnDashboard(frm)
    End Sub

    Private Sub btnQuitarEstado_Click(sender As Object, e As EventArgs) Handles btnQuitarEstado.Click
        ' 1) Validación de selección
        Dim row As EstadoRow = TryCast(dgvEstadosTransitorios.CurrentRow?.DataBoundItem, EstadoRow)
        If row Is Nothing OrElse row.EntityRef Is Nothing Then
            Notifier.Warn(Me, "No hay ningún estado seleccionado.")
            Return
        End If

        ' 2) Confirmación destructiva
        If MessageBox.Show("¿Está seguro de que desea quitar este estado transitorio?",
                       "Confirmar Eliminación",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Dim oldCursor = Me.Cursor
        btnQuitarEstado.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Dim entidad = row.EntityRef

            If entidad.Id > 0 Then
                ' Persistido: marcar para eliminar en EF (sin romper navegación/FKs)
                MarcarParaEliminar(entidad)
                ' Nota: el commit real ocurrirá al guardar el formulario/pantalla
                Notifier.Info(Me, "Estado marcado para eliminar. Se aplicará al actualizar.")
            Else
                ' No persistido: sacar del contexto si estaba adjunto
                Dim entry = _uow.Context.Entry(entidad)
                If entry IsNot Nothing AndAlso entry.State <> EntityState.Detached Then
                    entry.State = EntityState.Detached
                End If
                Notifier.Info(Me, "Estado quitado de la lista.")
            End If

            ' 3) Actualizar solo UI (lista y binding)
            _estadoRows.Remove(row)
            bsEstados.ResetBindings(False)

            ' (Opcional) limpiar selección visual
            dgvEstadosTransitorios.ClearSelection()

        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo quitar el estado: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnQuitarEstado.Enabled = True
        End Try
    End Sub


    Private Sub dgvDotacion_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) _
    Handles dgvDotacion.CellFormatting

        Dim dgv = CType(sender, DataGridView)

        ' Guardas defensivas básicas
        If dgv Is Nothing OrElse dgv.DataSource Is Nothing Then Return
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 OrElse e.ColumnIndex >= dgv.Columns.Count Then Return

        ' Si hay "fila nueva" visible, evitar formatearla
        If dgv.AllowUserToAddRows AndAlso e.RowIndex = dgv.NewRowIndex Then Return

        ' Si por cambios de binding el RowCount se quedó atrás, evitá acceder
        If e.RowIndex >= dgv.RowCount Then Return

        ' Evitar formatear filas compartidas sin DataBoundItem (virtualización)
        Dim row As DataGridViewRow = dgv.Rows(e.RowIndex)
        If row Is Nothing OrElse row.IsNewRow Then Return

        ' Este acceso puede lanzar si el CurrencyManager está desincronizado → Try/Catch
        Dim dataItem As FuncionarioDotacion = Nothing
        Try
            dataItem = TryCast(row.DataBoundItem, FuncionarioDotacion)
        Catch
            ' Si falla, salimos silenciosamente: el ciclo de binding volverá a llamar cuando esté estable
            Return
        End Try

        If dataItem Is Nothing Then Return

        ' Formateo por nombre de columna
        Dim colName = dgv.Columns(e.ColumnIndex).Name

        If colName = "colItem" Then
            e.Value = _itemsDotacion?.FirstOrDefault(Function(i) i.Id = dataItem.DotacionItemId)?.Nombre
            e.FormattingApplied = True
            Return
        End If

        If colName = "FechaAsign" AndAlso e.Value IsNot Nothing Then
            ' Evitar casteos inseguros si el valor viene nulo o con tipo diferente
            Dim dt As DateTime
            If TypeOf e.Value Is DateTime Then
                dt = DirectCast(e.Value, DateTime)
                e.Value = dt.ToString("dd/MM/yyyy")
                e.FormattingApplied = True
            End If
        End If
    End Sub

    Private Sub ConfigurarGrillaDotacion()
        ' Aplicamos el estilo base moderno
        AplicarEstiloModernoGrilla(dgvDotacion)

        ' Definición de columnas
        dgvDotacion.AutoGenerateColumns = False
        dgvDotacion.Columns.Clear()

        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .Visible = False})
        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {
        .Name = "colItem", .HeaderText = "Ítem",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .FillWeight = 40
    })
        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {
        .DataPropertyName = "Talla", .HeaderText = "Talla",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 80
    })
        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {
        .DataPropertyName = "Observaciones", .HeaderText = "Observaciones",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .FillWeight = 60
    })
        dgvDotacion.Columns.Add(New DataGridViewTextBoxColumn With {
        .DataPropertyName = "FechaAsign", .Name = "FechaAsign", .HeaderText = "Fecha Asignación",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 120
    })
    End Sub

    Private Sub ConfigurarGrillaEstados()
        ' Aplicamos el estilo base moderno
        AplicarEstiloModernoGrilla(dgvEstadosTransitorios)

        ' Definición de columnas
        dgvEstadosTransitorios.AutoGenerateColumns = False
        dgvEstadosTransitorios.Columns.Clear()

        dgvEstadosTransitorios.Columns.Add(New DataGridViewTextBoxColumn With {
        .DataPropertyName = "TipoEstado", .HeaderText = "Tipo de Estado",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 200
    })

        Dim colDesde As New DataGridViewTextBoxColumn With {
        .DataPropertyName = "FechaDesde", .HeaderText = "Desde",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 110
    }
        colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstadosTransitorios.Columns.Add(colDesde)

        Dim colHasta As New DataGridViewTextBoxColumn With {
        .DataPropertyName = "FechaHasta", .HeaderText = "Hasta",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 110
    }
        colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstadosTransitorios.Columns.Add(colHasta)

        dgvEstadosTransitorios.Columns.Add(New DataGridViewTextBoxColumn With {
        .DataPropertyName = "Observaciones", .HeaderText = "Observaciones / Detalles",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    })
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
        ' Si el formulario se está cerrando programáticamente o ya se guardó, no hacer nada.
        If _cerrandoPorCodigo OrElse _seGuardo Then Return

        ' Verificar si hay cambios pendientes.
        If _uow.Context.ChangeTracker.HasChanges() OrElse _estadoRows.Any(Function(r) r.EntityRef.Id <= 0) OrElse _estadosParaEliminar.Any() Then
            Dim result = MessageBox.Show("Hay cambios sin guardar. ¿Desea guardarlos antes de salir?", "Cambios Pendientes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning)

            Select Case result
                Case DialogResult.Yes

                    btnGuardar.PerformClick()

                    If Not _seGuardo Then
                        e.Cancel = True
                    End If

                Case DialogResult.No

                    If _uow IsNot Nothing Then
                        _uow.Dispose()
                    End If

                Case DialogResult.Cancel

                    e.Cancel = True
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
        cboSeccion.DataSource = Await _svc.ObtenerSeccionesAsync() : cboSeccion.DisplayMember = "Value" : cboSeccion.ValueMember = "Key"
        cboPuestoTrabajo.DataSource = Await _svc.ObtenerPuestosTrabajoAsync() : cboPuestoTrabajo.DisplayMember = "Value" : cboPuestoTrabajo.ValueMember = "Key"
        cboTurno.DataSource = Await _svc.ObtenerTurnosAsync() : cboTurno.DisplayMember = "Value" : cboTurno.ValueMember = "Key"
        cboSemana.DataSource = Await _svc.ObtenerSemanasAsync() : cboSemana.DisplayMember = "Value" : cboSemana.ValueMember = "Key"
        cboHorario.DataSource = Await _svc.ObtenerHorariosAsync() : cboHorario.DisplayMember = "Value" : cboHorario.ValueMember = "Key"
        cboCargo.SelectedIndex = -1 : cboEscalafon.SelectedIndex = -1 : cboFuncion.SelectedIndex = -1
        cboEstadoCivil.SelectedIndex = -1 : cboGenero.SelectedIndex = -1 : cboNivelEstudio.SelectedIndex = -1
        cboTurno.SelectedIndex = -1 : cboSemana.SelectedIndex = -1 : cboHorario.SelectedIndex = -1
        cboSubDireccion.DataSource = Await _svc.ObtenerSubDireccionesAsync() : cboSubDireccion.DisplayMember = "Value" : cboSubDireccion.ValueMember = "Key"
        cboSubEscalafon.DataSource = Await _svc.ObtenerSubEscalafonesAsync() : cboSubEscalafon.DisplayMember = "Value" : cboSubEscalafon.ValueMember = "Key"
        cboPrestadorSalud.DataSource = Await _svc.ObtenerPrestadoresSaludAsync() : cboPrestadorSalud.DisplayMember = "Value" : cboPrestadorSalud.ValueMember = "Key"
    End Function

    Private Sub btnAuditoria_Click(sender As Object, e As EventArgs) Handles btnAuditoria.Click
        If _modo = ModoFormulario.Crear Then
            MessageBox.Show("Debe guardar el funcionario antes de poder ver su historial de cambios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim frm As New frmAuditoriaViewer(_funcionario.Id.ToString())
        AbrirChildEnDashboard(frm)  ' ← en vez de NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
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

    Private Sub btnAñadirDotacion_Click(sender As Object, e As EventArgs) Handles btnAñadirDotacion.Click
        Dim nuevaDotacion = New FuncionarioDotacion() With {.Funcionario = _funcionario}
        Dim frm = New frmFuncionarioDotacion(nuevaDotacion)

        AddHandler frm.DotacionConfigurada,
            Async Sub(dotacion As FuncionarioDotacion)
                Await ProcesarDotacionDesdeChildAsync(dotacion, True)
            End Sub

        AbrirChildEnDashboard(frm)
    End Sub

    Private Sub btnEditarDotacion_Click(sender As Object, e As EventArgs) Handles btnEditarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        Dim dotacionSeleccionada = CType(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
        Dim frm = New frmFuncionarioDotacion(dotacionSeleccionada)

        AddHandler frm.DotacionConfigurada,
            Async Sub(dotacion As FuncionarioDotacion)
                Await ProcesarDotacionDesdeChildAsync(dotacion, False)
            End Sub

        AbrirChildEnDashboard(frm)
    End Sub

    Private Async Function ProcesarDotacionDesdeChildAsync(dotacion As FuncionarioDotacion, esNueva As Boolean) As Task
        If dotacion Is Nothing Then Return

        Try
            dotacion.Funcionario = _funcionario

            If dotacion.FuncionarioId = 0 AndAlso _funcionario IsNot Nothing AndAlso _funcionario.Id > 0 Then
                dotacion.FuncionarioId = _funcionario.Id
            End If

            If esNueva AndAlso _funcionario IsNot Nothing AndAlso Not _funcionario.FuncionarioDotacion.Contains(dotacion) Then
                _funcionario.FuncionarioDotacion.Add(dotacion)
            End If

            If Not esNueva Then
                _uow.Context.Entry(dotacion).State = EntityState.Modified
            End If

            Await _uow.CommitAsync()

            If esNueva Then
                If Not _dotaciones.Contains(dotacion) Then
                    _dotaciones.Add(dotacion)
                Else
                    bsDotacion.ResetBindings(False)
                End If
            Else
                bsDotacion.ResetBindings(False)
            End If

        Catch ex As Exception
            Dim mensaje = If(esNueva, "Error al añadir la dotación: ", "Error al actualizar la dotación: ") & ex.Message
            Notifier.Error(Me, mensaje)
        End Try
    End Function

    Private Async Sub btnQuitarDotacion_Click(sender As Object, e As EventArgs) Handles btnQuitarDotacion.Click
        ' 1) Validación de selección
        If dgvDotacion.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "No hay ninguna dotación seleccionada.")
            Return
        End If
        If dgvDotacion.AllowUserToAddRows AndAlso dgvDotacion.CurrentRow.Index = dgvDotacion.NewRowIndex Then
            Notifier.Warn(Me, "La fila nueva no puede eliminarse.")
            Return
        End If

        Dim dotacionSeleccionada = TryCast(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
        If dotacionSeleccionada Is Nothing Then
            Notifier.Warn(Me, "No se pudo determinar la dotación seleccionada.")
            Return
        End If

        ' 2) Confirmación destructiva (mantenemos modal)
        If MessageBox.Show("¿Está seguro de que desea quitar este elemento de dotación?",
                       "Confirmar",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        ' 3) UX: deshabilitar botón + cursor espera
        Dim oldCursor = Me.Cursor
        btnQuitarDotacion.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        ' 4) Pausar eventos del BindingSource para evitar inconsistencias durante el borrado
        Dim bs = TryCast(dgvDotacion.DataSource, BindingSource)
        If bs IsNot Nothing Then bs.RaiseListChangedEvents = False

        Dim removidaLocal As Boolean = False

        Try
            ' 5) Remover de la colección enlazada a la grilla (UI)
            If _dotaciones IsNot Nothing Then
                removidaLocal = _dotaciones.Remove(dotacionSeleccionada)
            End If

            ' 6) Si estaba persistida, borrar en BD
            If dotacionSeleccionada.Id > 0 Then
                Dim repo = _uow.Repository(Of FuncionarioDotacion)()
                repo.Remove(dotacionSeleccionada)
                Await _uow.CommitAsync()
                Notifier.Success(Me, "Dotación eliminada de la base de datos.")
            Else
                Notifier.Info(Me, "Dotación quitada de la lista (aún no persistida).")
            End If

        Catch ex As Exception
            ' Revertir eliminación local si falló el commit
            If removidaLocal AndAlso _dotaciones IsNot Nothing Then
                _dotaciones.Add(dotacionSeleccionada)
            End If
            Notifier.[Error](Me, $"Ocurrió un error al quitar la dotación: {ex.Message}")

        Finally
            ' 7) Reanudar eventos y refrescar binding
            If bs IsNot Nothing Then
                bs.RaiseListChangedEvents = True
                bs.ResetBindings(False)
            Else
                ' Si no hay BindingSource, al menos refrescar la grilla
                dgvDotacion.Refresh()
            End If

            ' 8) Limpiar selección y refrescar cuando el CurrencyManager ya se estabilizó
            BeginInvoke(CType(Sub()
                                  dgvDotacion.ClearSelection()
                                  dgvDotacion.Refresh()
                              End Sub, MethodInvoker))

            ' 9) Restaurar UX
            Me.Cursor = oldCursor
            btnQuitarDotacion.Enabled = True
        End Try
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
    Private Sub frmFuncionarioCrear_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnCancelar.PerformClick()  ' respeta tu lógica de cierre existente
        End If
    End Sub
    ' ==== Helpers de navegación (pila del Dashboard) ====
    Private Function GetDashboard() As frmDashboard
        Return Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
    End Function

    Private Sub AbrirChildEnDashboard(formHijo As Form)
        ' 1) Validaciones
        If formHijo Is Nothing Then
            Notifier.Warn(Me, "No hay formulario para abrir.")
            Return
        End If

        Dim dash = GetDashboard()
        If dash Is Nothing OrElse dash.IsDisposed Then
            Notifier.Warn(Me, "No se encontró el Dashboard activo.")
            Return
        End If

        ' 2) Garantizar que corra en el hilo de UI del dashboard
        If dash.InvokeRequired Then
            dash.BeginInvoke(CType(Sub() AbrirChildEnDashboard(formHijo), MethodInvoker))
            Return
        End If

        ' 3) Intentar abrir y dar feedback
        Try
            dash.Activate()
            dash.BringToFront()
            dash.AbrirChild(formHijo) ' ← usa la pila (Opción A)

            ' Toast de éxito anclado al Dashboard
            Notifier.Info(dash, $"Abierto: {formHijo.Text}")
        Catch ex As Exception
            Notifier.[Error](dash, $"No se pudo abrir la ventana: {ex.Message}")
        End Try
    End Sub
    ' Agrega este nuevo método en cualquier lugar dentro de la clase del formulario
    Private Sub AplicarEstiloModernoGrilla(dgv As DataGridView)
        ' --- CONFIGURACIÓN GENERAL ---
        dgv.BorderStyle = BorderStyle.None
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(230, 230, 230)
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.AllowUserToResizeRows = False
        dgv.BackgroundColor = Color.White

        ' --- ESTILO DE ENCABEZADOS (Headers) ---
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersHeight = 40
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

        ' --- ESTILO DE FILAS (Rows) ---
        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        dgv.DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
        dgv.DefaultCellStyle.SelectionForeColor = Color.White
        dgv.RowsDefaultCellStyle.BackColor = Color.White
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)
    End Sub

End Class
