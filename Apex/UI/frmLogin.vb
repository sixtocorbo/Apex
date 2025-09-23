' En: /UI/frmLogin.vb
Imports System.Windows.Forms

Public Class frmLogin
    ' Instancia del servicio de usuario que creaste antes
    Private ReadOnly _usuarioService As New UsuarioService()

    ' Convertimos el evento Click en asíncrono para poder usar 'Await'
    Private Async Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' 1. Validar que los campos no estén vacíos
        If String.IsNullOrWhiteSpace(txtUsuario.Text) OrElse String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Por favor, ingrese su usuario y contraseña.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            ' Deshabilitamos el botón para evitar múltiples clics mientras se procesa
            btnLogin.Enabled = False
            btnLogin.Text = "Verificando..."
            Me.Cursor = Cursors.WaitCursor

            ' 2. Llamar al servicio para validar las credenciales
            Dim usuarioValidado = Await _usuarioService.ValidateCredentialsAsync(txtUsuario.Text, txtPassword.Text)

            ' 3. Comprobar el resultado
            If usuarioValidado IsNot Nothing Then
                ' ¡Éxito! Cerramos el login y continuamos a la app principal.
                ' Guardamos el usuario validado en un lugar accesible, como un módulo global (opcional)
                ' ModuloGlobal.UsuarioActual = usuarioValidado 
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Else
                ' Fallo. Mostramos un mensaje de error.
                MessageBox.Show("El usuario o la contraseña son incorrectos.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            ' Manejar cualquier otro error que pueda ocurrir durante el proceso
            MessageBox.Show("Ocurrió un error inesperado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Reactivamos el botón y restauramos el cursor, sin importar el resultado
            btnLogin.Enabled = True
            btnLogin.Text = "Ingresar"
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    ''' <summary>
    ''' Maneja el clic en el botón para crear un nuevo usuario.
    ''' </summary>
    Private Sub btnCrearUsuario_Click(sender As Object, e As EventArgs) Handles btnCrearUsuario.Click
        ' Creamos una instancia del formulario de creación y la mostramos.
        ' Usamos ShowDialog para que el flujo se detenga hasta que se cierre el formulario de creación.
        Using form As New frmUsuarioCrear()
            form.ShowDialog(Me)
        End Using
    End Sub

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Establecer el botón "Ingresar" como el botón por defecto para que se active con la tecla Enter
        Me.AcceptButton = btnLogin
    End Sub
End Class