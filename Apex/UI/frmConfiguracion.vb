Imports System.ComponentModel

Public Class frmConfiguracion

    Private _cargoService As New CargoService()
    Private _gestionIncidenciasInstancia As frmIncidencias
    Private _gestionCargosInstancia As frmGrados
    Private _gestionSeccionesInstancia As frmSecciones
    Private _gestionAreasTrabajoInstancia As frmAreaTrabajoCategorias
    Private _gestionTurnosInstancia As frmTurnos
    Private _gestionNomenclaturasInstancia As frmNomenclaturas
    Private _gestionTiposEstadoTransitorioInstancia As frmEstadoTransitorioTipos
    Private _gestionCategoriasAusenciaInstancia As frmIncidenciasCategorias

    Private Sub btnGestionarIncidencias_Click(sender As Object, e As EventArgs)
        If _gestionIncidenciasInstancia Is Nothing OrElse _gestionIncidenciasInstancia.IsDisposed Then
            _gestionIncidenciasInstancia = New frmIncidencias()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionIncidenciasInstancia)
    End Sub

    Private Sub btnCargos_Click(sender As Object, e As EventArgs) Handles btnCargos.Click
        ' Ya no hay necesidad de verificar la instancia ni de delegados.
        _gestionCargosInstancia = New frmGrados()

        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionCargosInstancia)
    End Sub

    Private Sub btnSecciones_Click(sender As Object, e As EventArgs) Handles btnSecciones.Click
        If _gestionSeccionesInstancia Is Nothing OrElse _gestionSeccionesInstancia.IsDisposed Then
            _gestionSeccionesInstancia = New frmSecciones()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionSeccionesInstancia)
    End Sub

    Private Sub btnAreasTrabajo_Click(sender As Object, e As EventArgs) Handles btnAreasTrabajo.Click
        If _gestionAreasTrabajoInstancia Is Nothing OrElse _gestionAreasTrabajoInstancia.IsDisposed Then
            _gestionAreasTrabajoInstancia = New frmAreaTrabajoCategorias()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionAreasTrabajoInstancia)
    End Sub

    Private Sub btnTurnos_Click(sender As Object, e As EventArgs) Handles btnTurnos.Click
        If _gestionTurnosInstancia Is Nothing OrElse _gestionTurnosInstancia.IsDisposed Then
            _gestionTurnosInstancia = New frmTurnos()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionTurnosInstancia)
    End Sub

    Private Sub btnNomenclaturas_Click(sender As Object, e As EventArgs) Handles btnNomenclaturas.Click
        If _gestionNomenclaturasInstancia Is Nothing OrElse _gestionNomenclaturasInstancia.IsDisposed Then
            _gestionNomenclaturasInstancia = New frmNomenclaturas()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionNomenclaturasInstancia)
    End Sub

    Private Sub btnTiposEstadoTransitorio_Click(sender As Object, e As EventArgs)
        If _gestionTiposEstadoTransitorioInstancia Is Nothing OrElse _gestionTiposEstadoTransitorioInstancia.IsDisposed Then
            _gestionTiposEstadoTransitorioInstancia = New frmEstadoTransitorioTipos()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionTiposEstadoTransitorioInstancia)
    End Sub

    Private Sub btnCategoriasAusencia_Click(sender As Object, e As EventArgs)
        If _gestionCategoriasAusenciaInstancia Is Nothing OrElse _gestionCategoriasAusenciaInstancia.IsDisposed Then
            _gestionCategoriasAusenciaInstancia = New frmIncidenciasCategorias()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionCategoriasAusenciaInstancia)
    End Sub

    Private Sub frmConfiguracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
    End Sub
End Class