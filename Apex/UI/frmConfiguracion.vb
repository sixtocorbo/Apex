Imports System.ComponentModel

Public Class frmConfiguracion

    Private Sub frmConfiguracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
    End Sub

    Private Sub btnGestionarIncidencias_Click(sender As Object, e As EventArgs) Handles btnGestionarIncidencias.Click
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmIncidencias)(Me)
    End Sub

    Private Sub btnCargos_Click(sender As Object, e As EventArgs) Handles btnCargos.Click
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmGrados)(Me)
    End Sub

    Private Sub btnSecciones_Click(sender As Object, e As EventArgs) Handles btnSecciones.Click
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmSecciones)(Me)
    End Sub

    Private Sub btnAreasTrabajo_Click(sender As Object, e As EventArgs) Handles btnAreasTrabajo.Click
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmAreaTrabajoCategorias)(Me)
    End Sub

    Private Sub btnTurnos_Click(sender As Object, e As EventArgs) Handles btnTurnos.Click
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmTurnos)(Me)
    End Sub

    Private Sub btnTiposEstadoTransitorio_Click(sender As Object, e As EventArgs) Handles btnTiposEstadoTransitorio.Click
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmEstadoTransitorioTipos)(Me)
    End Sub

    Private Sub btnCategoriasAusencia_Click(sender As Object, e As EventArgs) Handles btnCategoriasAusencia.Click
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmIncidenciasCategorias)(Me)
    End Sub

    Private Sub btnNomenclaturas_Click(sender As Object, e As EventArgs) Handles btnNomenclaturas.Click
        ' Le decimos que abra Nomenclaturas y cierre ESTE formulario (Me).
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmNomenclaturas)(Me)
    End Sub

End Class