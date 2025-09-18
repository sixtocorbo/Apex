Imports System.ComponentModel

Public Class frmConfiguracion

    Private Sub frmConfiguracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
    End Sub

    ' === Helper local para abrir hijos en el Dashboard (usando la pila) ===
    Private Sub AbrirHijoEnDashboard(Of T As {Form, New})()
        Dim dash = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dash Is Nothing Then
            MessageBox.Show("No se encontró el Dashboard activo.", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        ' Creamos una instancia fresca del hijo y lo abrimos como “child”.
        dash.AbrirChild(New T())
    End Sub

    Private Sub btnGestionarIncidencias_Click(sender As Object, e As EventArgs) Handles btnGestionarIncidencias.Click
        AbrirHijoEnDashboard(Of frmIncidencias)()
    End Sub

    Private Sub btnCargos_Click(sender As Object, e As EventArgs) Handles btnCargos.Click
        AbrirHijoEnDashboard(Of frmGrados)()
    End Sub

    Private Sub btnSecciones_Click(sender As Object, e As EventArgs) Handles btnSecciones.Click
        AbrirHijoEnDashboard(Of frmSecciones)()
    End Sub

    Private Sub btnAreasTrabajo_Click(sender As Object, e As EventArgs) Handles btnAreasTrabajo.Click
        AbrirHijoEnDashboard(Of frmAreaTrabajoCategorias)()
    End Sub

    Private Sub btnTurnos_Click(sender As Object, e As EventArgs) Handles btnTurnos.Click
        AbrirHijoEnDashboard(Of frmTurnos)()
    End Sub

    Private Sub btnTiposEstadoTransitorio_Click(sender As Object, e As EventArgs) Handles btnTiposEstadoTransitorio.Click
        AbrirHijoEnDashboard(Of frmEstadoTransitorioTipos)()
    End Sub

    Private Sub btnCategoriasAusencia_Click(sender As Object, e As EventArgs) Handles btnCategoriasAusencia.Click
        AbrirHijoEnDashboard(Of frmIncidenciasCategorias)()
    End Sub

    Private Sub btnNomenclaturas_Click(sender As Object, e As EventArgs) Handles btnNomenclaturas.Click
        ' Antes: abrías y cerrabas este formulario.
        ' Ahora: lo abrimos como “child”; este queda oculto y se restaura al cerrar el hijo.
        AbrirHijoEnDashboard(Of frmNomenclaturas)()
    End Sub

End Class
