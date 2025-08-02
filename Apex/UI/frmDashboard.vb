' Reemplaza el contenido de este archivo.
Public Class frmDashboard

    Private currentBtn As Button
    Private Shadows activeForm As Form

    ' --- Instancias de los formularios para mantener su estado ---
    Private _funcionarioBuscarInstancia As frmFuncionarioBuscar
    Private _filtroAvanzadoInstancia As frmFiltroAvanzado


    Public Sub New()
        InitializeComponent()
        ' Asociar los manejadores de eventos a los botones de navegación
        AddHandler btnFuncionarios.Click, AddressOf ActivateButton
        AddHandler btnFiltros.Click, AddressOf ActivateButton
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
                If _funcionarioBuscarInstancia Is Nothing OrElse _funcionarioBuscarInstancia.IsDisposed Then
                    _funcionarioBuscarInstancia = New frmFuncionarioBuscar()
                End If
                AbrirFormEnPanel(_funcionarioBuscarInstancia)

            Case "btnFiltros"
                If _filtroAvanzadoInstancia Is Nothing OrElse _filtroAvanzadoInstancia.IsDisposed Then
                    _filtroAvanzadoInstancia = New frmFiltroAvanzado()
                End If
                AbrirFormEnPanel(_filtroAvanzadoInstancia)

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
        ' Si hay un formulario activo y es diferente al que queremos abrir, lo ocultamos.
        If activeForm IsNot Nothing AndAlso activeForm IsNot childForm Then
            activeForm.Hide()
        End If

        ' Establecemos el nuevo formulario como activo.
        activeForm = childForm

        ' Si el formulario aún no ha sido añadido al panel de contenido, lo configuramos y añadimos.
        If Not Me.panelContenido.Controls.Contains(childForm) Then
            childForm.TopLevel = False
            childForm.FormBorderStyle = FormBorderStyle.None
            childForm.Dock = DockStyle.Fill
            Me.panelContenido.Controls.Add(childForm)
            Me.panelContenido.Tag = childForm
        End If

        ' Finalmente, traemos el formulario al frente y nos aseguramos de que sea visible.
        childForm.BringToFront()
        childForm.Show()
    End Sub

End Class