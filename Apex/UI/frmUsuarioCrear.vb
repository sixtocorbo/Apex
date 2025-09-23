' En: /UI/frmUsuarioCrear.vb
Public Class frmUsuarioCrear
    Private ReadOnly _id As Integer?
    Private _svc As New UsuarioService()
    Private _usuario As Usuario

    Public Sub New(Optional id As Integer? = Nothing)
        InitializeComponent()
        _id = id
    End Sub

    Private Async Sub frmUsuarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        If _id.HasValue Then
            Me.Text = "Editar Usuario"
            _usuario = Await _svc.GetByIdAsync(_id.Value)
            CargarDatos()
        Else
            Me.Text = "Nuevo Usuario"
            _usuario = New Usuario With {.Activo = True}
        End If
    End Sub

    Private Sub CargarDatos()
        txtNombreUsuario.Text = _usuario.NombreUsuario
        txtNombreCompleto.Text = _usuario.NombreCompleto
        chkActivo.Checked = _usuario.Activo
        ' La contraseña no se carga por seguridad
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombreUsuario.Text) OrElse String.IsNullOrWhiteSpace(txtNombreCompleto.Text) Then
            Notifier.Warn(Me, "El nombre de usuario y el nombre completo son obligatorios.")
            Return
        End If

        If Not _id.HasValue AndAlso String.IsNullOrWhiteSpace(txtPassword.Text) Then
            Notifier.Warn(Me, "La contraseña es obligatoria para nuevos usuarios.")
            Return
        End If

        _usuario.NombreUsuario = txtNombreUsuario.Text.Trim()
        _usuario.NombreCompleto = txtNombreCompleto.Text.Trim()
        _usuario.Activo = chkActivo.Checked

        Try
            If _id.HasValue Then
                Await _svc.UpdateAsync(_usuario, txtPassword.Text)
            Else
                Await _svc.CreateAsync(_usuario, txtPassword.Text)
            End If
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            Notifier.Error(Me, "Error al guardar: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class