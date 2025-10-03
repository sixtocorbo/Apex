Public Class frmReportes
    Private Sub btnAnalisisEstacional_Click(sender As Object, e As EventArgs) Handles btnAnalisisEstacional.Click
        Dim frm As New frmAnalisisEstacionalidad
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnAnalisisFuncionarios_Click(sender As Object, e As EventArgs) Handles btnAnalisisFuncionarios.Click
        Dim frm As New frmAnalisisFuncionarios
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnFuncionariosGenero_Click(sender As Object, e As EventArgs) Handles btnFuncionariosGenero.Click
        Dim frm As New frmReporteFuncionariosGenero
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnFuncionariosEdad_Click(sender As Object, e As EventArgs) Handles btnFuncionariosEdad.Click
        Dim frm As New frmReporteFuncionariosEdad
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnFuncionariosArea_Click(sender As Object, e As EventArgs) Handles btnFuncionariosArea.Click
        Dim frm As New frmReporteFuncionariosAreaTrabajo
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnFuncionariosCargo_Click(sender As Object, e As EventArgs) Handles btnFuncionariosCargo.Click
        Dim frm As New frmReporteFuncionariosCargo
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnLicenciasPorTipo_Click(sender As Object, e As EventArgs) Handles btnLicenciasPorTipo.Click
        Dim frm As New frmReporteLicenciasPorTipo
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnLicenciasPorEstado_Click(sender As Object, e As EventArgs) Handles btnLicenciasPorEstado.Click
        Dim frm As New frmReporteLicenciasPorEstado
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnLicenciasTopFuncionarios_Click(sender As Object, e As EventArgs) Handles btnLicenciasTopFuncionarios.Click
        Dim frm As New frmReporteTopFuncionariosLicencias
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub frmReportes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarLayoutReportes()
    End Sub

    Private Sub ConfigurarLayoutReportes()
        With FlowLayoutPanel1
            .Dock = DockStyle.Fill
            .AutoScroll = True
            .WrapContents = True
            .FlowDirection = FlowDirection.LeftToRight
            .Padding = New Padding(12)
            .Margin = New Padding(0)
        End With

        Dim botones() As Button = {
            btnAnalisisEstacional,
            btnAnalisisFuncionarios,
            btnFuncionariosGenero,
            btnFuncionariosEdad,
            btnFuncionariosArea,
            btnFuncionariosCargo,
            btnLicenciasPorTipo,
            btnLicenciasPorEstado,
            btnLicenciasTopFuncionarios
        }

        For i = 0 To botones.Length - 1
            Dim b = botones(i)
            b.AutoSize = False
            b.Height = 44
            b.MinimumSize = New Size(220, 44)
            b.Margin = New Padding(12)
            b.FlatStyle = FlatStyle.Standard
            b.TextAlign = ContentAlignment.MiddleCenter
            b.TabIndex = i
        Next

        AddHandler FlowLayoutPanel1.Resize, AddressOf AjustarAnchoBotones
        AjustarAnchoBotones(Nothing, EventArgs.Empty)
    End Sub

    Private Sub AjustarAnchoBotones(sender As Object, e As EventArgs)
        Dim pnl = FlowLayoutPanel1
        If pnl Is Nothing OrElse pnl.ClientSize.Width <= 0 Then Exit Sub

        Dim gap As Integer = 24
        Dim paddingH As Integer = pnl.Padding.Left + pnl.Padding.Right
        Dim anchoMin As Integer = 260
        Dim anchoDisponible = pnl.ClientSize.Width - paddingH
        Dim cols As Integer = Math.Max(1, Math.Min(3, (anchoDisponible + gap) \ (anchoMin + gap)))
        Dim anchoBtn As Integer = Math.Max(anchoMin, (anchoDisponible - (cols - 1) * gap) \ cols)

        For Each c In pnl.Controls
            Dim b = TryCast(c, Button)
            If b IsNot Nothing Then
                b.Width = anchoBtn
                b.Height = 44
            End If
        Next
    End Sub
End Class
