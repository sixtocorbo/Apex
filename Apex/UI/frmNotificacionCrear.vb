Imports System.Data.Entity

Public Class frmNotificacionCrear

    Private _svc As NotificacionService
    Private _notificacion As NotificacionPersonal
    Private _modo As ModoFormulario
    Private _idNotificacion As Integer
    Public Property NotificacionId As Integer? = Nothing
    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    ' Constructor para crear
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _notificacion = New NotificacionPersonal()
        Me.Text = "Nueva Notificación"
    End Sub

    ' Constructor para editar
    Public Sub New(id As Integer)
        Me.New()
        _modo = ModoFormulario.Editar
        _idNotificacion = id
        Me.Text = "Editar Notificación"
    End Sub

    Private Async Sub frmNotificacionCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        _svc = New NotificacionService()
        Await CargarCombosAsync()

        If _modo = ModoFormulario.Editar Then
            Await CargarDatosAsync()
        End If
    End Sub

    Private Async Function CargarCombosAsync() As Task
        ' Cargar Funcionarios llamando al servicio
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        cboFuncionario.DataSource = Await _svc.ObtenerFuncionariosParaComboAsync()
        ' --- LÍNEAS AÑADIDAS: Se activa la magia del autocompletado ---
        cboFuncionario.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboFuncionario.AutoCompleteSource = AutoCompleteSource.ListItems

        ' Cargar Tipos de Notificación llamando al servicio
        cboTipoNotificacion.DisplayMember = "Value"
        cboTipoNotificacion.ValueMember = "Key"
        cboTipoNotificacion.DataSource = Await _svc.ObtenerTiposNotificacionParaComboAsync()

        ' Limpiar selección inicial
        cboFuncionario.SelectedIndex = -1
        cboTipoNotificacion.SelectedIndex = -1
    End Function


    Private Async Function CargarDatosAsync() As Task
        _notificacion = Await _svc.GetByIdParaEdicionAsync(_idNotificacion)

        If _notificacion Is Nothing Then
            MessageBox.Show("No se encontró la notificación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If

        ' --- INICIO DE LA CORRECCIÓN CLAVE ---
        ' Asegurar que el funcionario de la notificación exista en el ComboBox, incluso si está inactivo.
        Dim funcionariosSource = CType(cboFuncionario.DataSource, List(Of KeyValuePair(Of Integer, String)))

        ' Verificar si el funcionario de la notificación NO está en la lista del combo.
        If Not funcionariosSource.Any(Function(kvp) kvp.Key = _notificacion.FuncionarioId) Then
            ' Si no está, lo buscamos directamente en la base de datos (usando el servicio actual).
            Using tempUow As New UnitOfWork()
                Dim funcionarioDeNotificacion = Await tempUow.Repository(Of Funcionario)().GetByIdAsync(_notificacion.FuncionarioId)
                If funcionarioDeNotificacion IsNot Nothing Then
                    ' Lo añadimos a la lista, marcándolo como inactivo para claridad del usuario.
                    funcionariosSource.Add(New KeyValuePair(Of Integer, String)(funcionarioDeNotificacion.Id, funcionarioDeNotificacion.Nombre & " (Inactivo)"))
                    ' Re-asignamos la fuente de datos y la reordenamos para mantener el orden alfabético.
                    cboFuncionario.DataSource = funcionariosSource.OrderBy(Function(kvp) kvp.Value).ToList()
                End If
            End Using
        End If
        ' --- FIN DE LA CORRECCIÓN CLAVE ---

        ' Ahora, las asignaciones de valores funcionarán correctamente para cualquier funcionario.
        cboFuncionario.SelectedValue = _notificacion.FuncionarioId
        cboTipoNotificacion.SelectedValue = _notificacion.TipoNotificacionId
        dtpFechaProgramada.Value = _notificacion.FechaProgramada
        txtMedio.Text = _notificacion.Medio
        txtDocumento.Text = _notificacion.Documento
        txtExpMinisterial.Text = _notificacion.ExpMinisterial
        txtExpINR.Text = _notificacion.ExpINR
        txtOficina.Text = _notificacion.Oficina
    End Function

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If cboFuncionario.SelectedIndex = -1 OrElse cboTipoNotificacion.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un funcionario y un tipo de notificación.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _notificacion.FuncionarioId = CInt(cboFuncionario.SelectedValue)
        _notificacion.TipoNotificacionId = CByte(cboTipoNotificacion.SelectedValue)
        _notificacion.FechaProgramada = dtpFechaProgramada.Value
        _notificacion.Medio = txtMedio.Text.Trim()
        _notificacion.Documento = txtDocumento.Text.Trim()
        _notificacion.ExpMinisterial = txtExpMinisterial.Text.Trim()
        _notificacion.ExpINR = txtExpINR.Text.Trim()
        _notificacion.Oficina = txtOficina.Text.Trim()

        Try
            If _modo = ModoFormulario.Crear Then
                _notificacion.CreatedAt = DateTime.Now
                _notificacion.EstadoId = 1 ' Estado "Pendiente" por defecto
                Await _svc.CreateAsync(_notificacion)
            Else
                _notificacion.UpdatedAt = DateTime.Now
                Await _svc.UpdateAsync(_notificacion)
            End If

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la notificación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class