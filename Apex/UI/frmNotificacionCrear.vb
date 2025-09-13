Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class frmNotificacionCrear

    Private _svc As NotificacionService
    Private _notificacion As NotificacionPersonal
    Private _modo As ModoFormulario
    Private _idNotificacion As Integer
    Private _idFuncionarioPreseleccionado As Integer? = Nothing
    Public Property NotificacionId As Integer? = Nothing

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    ' Constructor para crear
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        Me.Text = "Nueva Notificación"
    End Sub
    ' --- INICIO DEL CAMBIO ---
    ' Nuevo constructor para crear con funcionario preseleccionado.
    ' Se añade un segundo argumento Booleano para diferenciar la firma.
    ''' <summary>
    ''' Crea una nueva notificación y preselecciona un funcionario.
    ''' </summary>
    ''' <param name="funcionarioId">ID del funcionario a seleccionar.</param>
    ''' <param name="esPreseleccion">Argumento para diferenciar la firma del constructor de edición.</param>
    Public Sub New(funcionarioId As Integer, esPreseleccion As Boolean)
        Me.New() ' Llama al constructor base
        _modo = ModoFormulario.Crear
        _idFuncionarioPreseleccionado = funcionarioId
    End Sub
    ' --- FIN DEL CAMBIO ---

    ' Constructor para editar
    Public Sub New(id As Integer)
        Me.New()
        _modo = ModoFormulario.Editar
        _idNotificacion = id
        Me.Text = "Editar Notificación"
    End Sub


    Private Async Sub frmNotificacionCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar
        AppTheme.Aplicar(Me)

        _svc = New NotificacionService()

        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            Await CargarCombosAsync()

            If _modo = ModoFormulario.Editar Then
                Await CargarDatosAsync()
                Notifier.Info(Me, "Editá los datos y guardá los cambios.")
            ElseIf _idFuncionarioPreseleccionado.HasValue Then
                cboFuncionario.SelectedValue = _idFuncionarioPreseleccionado.Value
                Notifier.Info(Me, "Completá los datos y guardá la notificación.")
            Else
                Notifier.Info(Me, "Seleccioná un funcionario y completá los datos.")
            End If

            Try
                AppTheme.SetCue(cboFuncionario, "Buscar por funcionario…")
                AppTheme.SetCue(txtDocumento, "Nº de documento…")
                AppTheme.SetCue(txtExpMinisterial, "Nº expediente ministerial…")
                AppTheme.SetCue(txtExpINR, "Nº expediente INR…")
                AppTheme.SetCue(txtOficina, "Oficina…")
                AppTheme.SetCue(txtMedio, "Texto de notificación…")
                AppTheme.SetCue(cboTipoNotificacion, "Tipo de notificación…")
            Catch
                ' ignorar si SetCue no está disponible
            End Try

        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo inicializar el formulario: {ex.Message}")
            Close()
        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub


    Private Async Function CargarCombosAsync() As Task
        ' Funcionarios
        cboFuncionario.DropDownStyle = ComboBoxStyle.DropDownList
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        cboFuncionario.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboFuncionario.AutoCompleteSource = AutoCompleteSource.ListItems
        cboFuncionario.DataSource = Await _svc.ObtenerFuncionariosParaComboAsync()

        ' Tipos de notificación
        cboTipoNotificacion.DropDownStyle = ComboBoxStyle.DropDownList
        cboTipoNotificacion.DisplayMember = "Value"
        cboTipoNotificacion.ValueMember = "Key"
        cboTipoNotificacion.DataSource = Await _svc.ObtenerTiposNotificacionParaComboAsync()

        ' Selección inicial
        If _modo = ModoFormulario.Crear AndAlso Not _idFuncionarioPreseleccionado.HasValue Then
            cboFuncionario.SelectedIndex = -1
        End If
        cboTipoNotificacion.SelectedIndex = -1
    End Function


    Private Async Function CargarDatosAsync() As Task
        _notificacion = Await _svc.GetByIdParaEdicionAsync(_idNotificacion)
        If _notificacion Is Nothing Then
            Notifier.[Error](Me, "No se encontró la notificación.")
            Close()
            Return
        End If

        ' Asegurar que el funcionario esté en el combo aunque esté inactivo
        Dim funcionariosSource = TryCast(cboFuncionario.DataSource, List(Of KeyValuePair(Of Integer, String)))
        If funcionariosSource IsNot Nothing AndAlso
       Not funcionariosSource.Any(Function(kvp) kvp.Key = _notificacion.FuncionarioId) Then
            Using tempUow As New UnitOfWork()
                Dim func = Await tempUow.Repository(Of Funcionario)().GetByIdAsync(_notificacion.FuncionarioId)
                If func IsNot Nothing Then
                    funcionariosSource.Add(New KeyValuePair(Of Integer, String)(func.Id, func.Nombre & " (Inactivo)"))
                    cboFuncionario.DataSource = funcionariosSource.OrderBy(Function(kvp) kvp.Value).ToList()
                End If
            End Using
        End If

        ' Bind de campos
        cboFuncionario.SelectedValue = _notificacion.FuncionarioId
        cboTipoNotificacion.SelectedValue = _notificacion.TipoNotificacionId
        dtpFechaProgramada.Value = _notificacion.FechaProgramada
        txtMedio.Text = _notificacion.Medio
        txtDocumento.Text = _notificacion.Documento
        txtExpMinisterial.Text = _notificacion.ExpMinisterial
        txtExpINR.Text = _notificacion.ExpINR
        txtOficina.Text = _notificacion.Oficina
    End Function


    Private Function ValidarCampos() As Boolean
        If cboFuncionario.SelectedIndex = -1 OrElse cboFuncionario.SelectedValue Is Nothing Then
            Notifier.Warn(Me, "Debe seleccionar un funcionario.")
            cboFuncionario.DroppedDown = True
            Return False
        End If
        If cboTipoNotificacion.SelectedIndex = -1 OrElse cboTipoNotificacion.SelectedValue Is Nothing Then
            Notifier.Warn(Me, "Debe seleccionar un tipo de notificación.")
            cboTipoNotificacion.DroppedDown = True
            Return False
        End If
        Return True
    End Function


    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Not ValidarCampos() Then Return

        Dim oldCursor = Me.Cursor
        btnGuardar.Enabled = False
        btnCancelar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Dim idNotificacionGuardada As Integer

            If _modo = ModoFormulario.Crear Then
                Dim req As New NotificacionCreateRequest With {
                .FuncionarioId = CInt(cboFuncionario.SelectedValue),
                .TipoNotificacionId = CByte(cboTipoNotificacion.SelectedValue),
                .FechaProgramada = dtpFechaProgramada.Value,
                .Medio = txtMedio.Text.Trim(),
                .Documento = txtDocumento.Text.Trim(),
                .ExpMinisterial = txtExpMinisterial.Text.Trim(),
                .ExpINR = txtExpINR.Text.Trim(),
                .Oficina = txtOficina.Text.Trim()
            }
                Dim creada = Await _svc.CreateNotificacionAsync(req)
                idNotificacionGuardada = creada.Id
                Me.NotificacionId = idNotificacionGuardada
                NotificadorEventos.NotificarCambiosEnFuncionario(req.FuncionarioId)
                Notifier.Success(Me, "Notificación creada correctamente.")
            Else
                Dim req As New NotificacionUpdateRequest With {
                .Id = _idNotificacion,
                .FuncionarioId = CInt(cboFuncionario.SelectedValue),
                .TipoNotificacionId = CByte(cboTipoNotificacion.SelectedValue),
                .FechaProgramada = dtpFechaProgramada.Value,
                .Medio = txtMedio.Text.Trim(),
                .Documento = txtDocumento.Text.Trim(),
                .ExpMinisterial = txtExpMinisterial.Text.Trim(),
                .ExpINR = txtExpINR.Text.Trim(),
                .Oficina = txtOficina.Text.Trim()
            }
                Await _svc.UpdateNotificacionAsync(req)
                idNotificacionGuardada = _idNotificacion
                Me.NotificacionId = idNotificacionGuardada
                NotificadorEventos.NotificarCambiosEnFuncionario(req.FuncionarioId)
                Notifier.Success(Me, "Notificación actualizada correctamente.")
            End If

            ' Confirmación modal de impresión (si querés, luego lo cambiamos a un ConfirmToast no modal)
            Dim imprimir = MessageBox.Show(
            "¿Desea imprimir el comprobante ahora?",
            "Imprimir Notificación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

            If imprimir = DialogResult.Yes Then
                Dim frmReporte As New frmNotificacionRPT(idNotificacionGuardada)
                NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frmReporte)
                Notifier.Info(Me, "Abriendo reporte de notificación…")
            End If

            DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al guardar la notificación: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnGuardar.Enabled = True
            btnCancelar.Enabled = True
        End Try
    End Sub


    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Notifier.Info(Me, "Acción cancelada.")
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnCancelar.PerformClick()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Enter AndAlso Not btnGuardar.Focused Then
            btnGuardar.PerformClick()
            e.Handled = True
        End If
    End Sub


End Class
