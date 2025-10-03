Imports System.Linq

Public Class frmReportes
    Private Sub btnAnalisisEstacional_Click(sender As Object, e As EventArgs) Handles btnAnalisisEstacional.Click
        AbrirFormularioReporte(Of frmAnalisisEstacionalidad)()
    End Sub

    Private Sub btnResumenCantidades_Click(sender As Object, e As EventArgs) Handles btnResumenCantidades.Click
        AbrirFormularioReporte(Of frmResumenCantidades)()
    End Sub

    Private Sub btnFuncionariosGenero_Click(sender As Object, e As EventArgs) Handles btnFuncionariosGenero.Click
        AbrirFormularioReporte(Of frmReporteFuncionariosGenero)()
    End Sub

    Private Sub btnFuncionariosEdad_Click(sender As Object, e As EventArgs) Handles btnFuncionariosEdad.Click
        AbrirFormularioReporte(Of frmReporteFuncionariosEdad)()
    End Sub

    Private Sub btnFuncionariosArea_Click(sender As Object, e As EventArgs) Handles btnFuncionariosArea.Click
        AbrirFormularioReporte(Of frmReporteFuncionariosAreaTrabajo)()
    End Sub

    Private Sub btnFuncionariosCargo_Click(sender As Object, e As EventArgs) Handles btnFuncionariosCargo.Click
        AbrirFormularioReporte(Of frmReporteFuncionariosCargo)()
    End Sub

    Private Sub btnFuncionariosEstado_Click(sender As Object, e As EventArgs) Handles btnFuncionariosEstado.Click
        AbrirFormularioReporte(Of frmReporteFuncionariosEstado)()
    End Sub

    Private Sub btnFuncionariosTurno_Click(sender As Object, e As EventArgs) Handles btnFuncionariosTurno.Click
        AbrirFormularioReporte(Of frmReporteFuncionariosTurno)()
    End Sub

    Private Sub btnFuncionariosNivelEstudio_Click(sender As Object, e As EventArgs) Handles btnFuncionariosNivelEstudio.Click
        AbrirFormularioReporte(Of frmReporteFuncionariosNivelEstudio)()
    End Sub

    Private Sub btnLicenciasPorTipo_Click(sender As Object, e As EventArgs) Handles btnLicenciasPorTipo.Click
        AbrirFormularioReporte(Of frmReporteLicenciasPorTipo)()
    End Sub

    Private Sub btnLicenciasPorEstado_Click(sender As Object, e As EventArgs) Handles btnLicenciasPorEstado.Click
        AbrirFormularioReporte(Of frmReporteLicenciasPorEstado)()
    End Sub

    Private Sub btnLicenciasTopFuncionarios_Click(sender As Object, e As EventArgs) Handles btnLicenciasTopFuncionarios.Click
        AbrirFormularioReporte(Of frmReporteTopFuncionariosLicencias)()
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
            btnResumenCantidades,
            btnAnalisisEstacional,
            btnFuncionariosGenero,
            btnFuncionariosEdad,
            btnFuncionariosArea,
            btnFuncionariosCargo,
            btnFuncionariosEstado,
            btnFuncionariosTurno,
            btnFuncionariosNivelEstudio,
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

    Private Sub AbrirFormularioReporte(Of T As {Form, New})()
        Dim frm = New T()
        PrepararFormularioParaEscape(frm)
        MostrarEnDashboard(frm)
    End Sub

    Private Sub PrepararFormularioParaEscape(frm As Form)
        If frm Is Nothing Then Return

        frm.KeyPreview = True
        AddHandler frm.KeyDown, AddressOf FormularioReporte_KeyDown
    End Sub

    Private Sub FormularioReporte_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode <> Keys.Escape Then Return

        e.Handled = True
        Dim frm = TryCast(sender, Form)
        frm?.Close()
    End Sub

    Private Sub MostrarEnDashboard(frm As Form)
        If frm Is Nothing Then Return

        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dashboard IsNot Nothing Then
            dashboard.AbrirChild(frm)
        Else
            frm.Show()
        End If
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
