Public Class frmDashboard

    Private currentBtn As Button
    Private activeForm As Form

    Public Sub New()
        InitializeComponent()
        ' Asociar los manejadores de eventos a los botones de navegación
        AddHandler btnFuncionarios.Click, AddressOf ActivateButton
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
            Case "btnReportes"
                ' Aquí abrirías tu formulario de reportes en el futuro
                ' AbrirFormEnPanel(New frmReportes())
            Case "btnConfiguracion"
                ' Aquí abrirías tu formulario de configuración
                ' AbrirFormEnPanel(New frmConfiguracion())
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