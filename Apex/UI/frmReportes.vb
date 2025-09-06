Public Class frmReportes
    Private Sub btnAnalisisEstacional_Click(sender As Object, e As EventArgs) Handles btnAnalisisEstacional.Click
        Dim frm As New frmAnalisisEstacionalidad
        NavegacionHelper.AbrirFormEnDashboard(frm)
    End Sub

    Private Sub btnAnalisisFuncionarios_Click(sender As Object, e As EventArgs) Handles btnAnalisisFuncionarios.Click
        Dim frm As New frmAnalisisFuncionarios
        NavegacionHelper.AbrirFormEnDashboard(frm)
    End Sub
End Class