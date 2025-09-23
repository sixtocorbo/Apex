' En: Program.vb (el nuevo módulo que creaste)

Module Program
    ''' <summary>
    ''' Punto de entrada principal de la aplicación.
    ''' </summary>
    Public Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Creamos una instancia del formulario de login
        Using loginForm As New frmLogin()

            ' Mostramos el login como un diálogo. La aplicación esperará aquí
            ' hasta que el formulario de login se cierre.
            Dim result As DialogResult = loginForm.ShowDialog()

            ' Si el resultado del diálogo es OK (porque presionamos el botón Ingresar
            ' con credenciales válidas), entonces abrimos el Dashboard.
            If result = DialogResult.OK Then
                ' Si el login fue exitoso, ejecutamos la aplicación con el Dashboard.
                Application.Run(New frmDashboard())
            End If

            ' Si el login se cierra con cualquier otro resultado (ej. la 'X' de la ventana),
            ' la aplicación simplemente terminará.
        End Using
    End Sub
End Module