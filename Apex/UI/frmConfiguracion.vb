Public Class frmConfiguracion

    Private Sub btnGestionarViaticos_Click(sender As Object, e As EventArgs) Handles btnGestionarViaticos.Click
        Using frm As New frmGestionViaticos()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnGestionarIncidencias_Click(sender As Object, e As EventArgs) Handles btnGestionarIncidencias.Click
        Using frm As New frmGestionIncidencias()
            frm.ShowDialog(Me)
        End Using
    End Sub

    ' --- NUEVO MÉTODO PARA EL BOTÓN VOLVER ---
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Me.Close()
    End Sub

End Class