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
    End Sub
End Class