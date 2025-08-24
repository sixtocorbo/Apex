Public Class frmConfiguracion

    Private Sub btnGestionarIncidencias_Click(sender As Object, e As EventArgs) Handles btnGestionarIncidencias.Click
        Using frm As New frmGestionIncidencias()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnCargos_Click(sender As Object, e As EventArgs) Handles btnCargos.Click
        ' Llama al nuevo formulario de gestión de cargos
        Using frm As New frmGestionCargos()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnSecciones_Click(sender As Object, e As EventArgs) Handles btnSecciones.Click
        ' Llama al nuevo formulario de gestión de secciones
        Using frm As New frmGestionSecciones()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnAreasTrabajo_Click(sender As Object, e As EventArgs) Handles btnAreasTrabajo.Click
        ' Llama al nuevo formulario de gestión de áreas de trabajo
        Using frm As New frmGestionAreasTrabajo()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnTurnos_Click(sender As Object, e As EventArgs) Handles btnTurnos.Click
        ' Llama al nuevo formulario de gestión de turnos
        Using frm As New frmGestionTurnos()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnNomenclaturas_Click(sender As Object, e As EventArgs) Handles btnNomenclaturas.Click
        Using frm As New frmGestionNomenclaturas()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnTiposEstadoTransitorio_Click(sender As Object, e As EventArgs) Handles btnTiposEstadoTransitorio.Click
        ' Llama al nuevo formulario de gestión
        Using frm As New frmGestionTiposEstadoTransitorio()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnCategoriasAusencia_Click(sender As Object, e As EventArgs) Handles btnCategoriasAusencia.Click
        ' Llama al nuevo formulario de gestión
        Using frm As New frmGestionCategoriasAusencia()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Me.Close()
    End Sub

End Class