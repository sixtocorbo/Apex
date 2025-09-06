' Apex/Helpers/NavegacionHelper.vb
Imports System.Windows.Forms

Public Module NavegacionHelper

    ''' <summary>
    ''' Busca el Dashboard principal y abre un formulario dentro de su panel.
    ''' Si no encuentra el Dashboard, abre el formulario de forma normal.
    ''' </summary>
    ''' <param name="formularioAAbrir">La instancia del formulario que se desea abrir.</param>
    Public Sub AbrirFormEnDashboard(ByVal formularioAAbrir As Form)

        ' 1. Busca la instancia abierta del Dashboard en toda la aplicación.
        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()

        ' 2. Comprueba si se encontró el Dashboard.
        If dashboard IsNot Nothing Then
            ' Si se encontró, llama a su método público para mostrar el formulario.
            dashboard.AbrirFormEnPanel(formularioAAbrir)
        Else
            ' 3. Si no se encontró, muestra un aviso y abre el formulario de forma independiente.
            '    Esto asegura que la aplicación no falle si el Dashboard no está disponible.
            MessageBox.Show("No se encontró el dashboard principal. El formulario se abrirá en una nueva ventana.",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            formularioAAbrir.Show()
        End If

    End Sub

End Module