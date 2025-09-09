Public Class frmReportes
    Private Sub btnAnalisisEstacional_Click(sender As Object, e As EventArgs) Handles btnAnalisisEstacional.Click
        Dim frm As New frmAnalisisEstacionalidad
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnAnalisisFuncionarios_Click(sender As Object, e As EventArgs) Handles btnAnalisisFuncionarios.Click
        Dim frm As New frmAnalisisFuncionarios
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub
End Class