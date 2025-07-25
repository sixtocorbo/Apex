' Reemplaza el contenido de este archivo.
Public Class frmDashboard

    Private currentBtn As Button
    Private Shadows activeForm As Form

    Public Sub New()
        InitializeComponent()
        ' Asociar los manejadores de eventos a los botones de navegación
        AddHandler btnFuncionarios.Click, AddressOf ActivateButton
        AddHandler btnFiltros.Click, AddressOf ActivateButton ' Mantenemos este
        AddHandler btnReportes.Click, AddressOf ActivateButton
        AddHandler btnConfiguracion.Click, AddressOf ActivateButton
    End Sub

    Private Sub ActivateButton(sender As Object, e As EventArgs)
        If sender Is Nothing Then Return

        DisableButton() ' Desactivar el botón anterior

        ' Activar el nuevo botón
        currentBtn = CType(sender, Button)
        currentBtn.BackColor = Color.FromArgb(81, 81, 112) ' Un color de resaltado
        currentBtn.ForeColor = Color.White
        currentBtn.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)

        ' Abrir el formulario correspondiente
        Select Case currentBtn.Name
            Case "btnFuncionarios"
                AbrirFormEnPanel(New frmFuncionarioBuscar())

            Case "btnFiltros"
                AbrirFormEnPanel(New frmFiltroAvanzado())

            Case "btnReportes"
                MessageBox.Show("Formulario de reportes aún no implementado.", "En desarrollo", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Case "btnConfiguracion"
                MessageBox.Show("Formulario de configuración aún no implementado.", "En desarrollo", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select
    End Sub

    Private Sub DisableButton()
        If currentBtn IsNot Nothing Then
            currentBtn.BackColor = Color.FromArgb(51, 51, 76) ' Color original del panel
            currentBtn.ForeColor = Color.Gainsboro
            currentBtn.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular)
        End If
    End Sub

    Private Sub AbrirFormEnPanel(childForm As Form)
        ' Si ya hay un formulario abierto, lo cerramos
        If activeForm IsNot Nothing Then
            activeForm.Close()
        End If

        activeForm = childForm
        childForm.TopLevel = False ' Muy importante para poder anidar el form
        childForm.FormBorderStyle = FormBorderStyle.None
        childForm.Dock = DockStyle.Fill
        Me.panelContenido.Controls.Add(childForm)
        Me.panelContenido.Tag = childForm
        childForm.BringToFront()
        childForm.Show()
    End Sub

End Class