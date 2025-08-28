Imports System.Data.Entity
Imports System.Windows.Forms
Imports System.ComponentModel

Public Class frmConfiguracion

    Private _cargoService As New CargoService()
    Private _gestionIncidenciasInstancia As frmGestionIncidencias
    Private _gestionCargosInstancia As frmGestionCargos
    Private _gestionSeccionesInstancia As frmGestionSecciones
    Private _gestionAreasTrabajoInstancia As frmGestionAreasTrabajo
    Private _gestionTurnosInstancia As frmGestionTurnos
    Private _gestionNomenclaturasInstancia As frmGestionNomenclaturas
    Private _gestionTiposEstadoTransitorioInstancia As frmGestionTiposEstadoTransitorio
    Private _gestionCategoriasAusenciaInstancia As frmGestionCategoriasAusencia

    ' Variable para el delegado de eventos, usando el tipo correcto
    Private cargosModificadosDelegate As frmGestionCargos.CargosModificadosEventHandler

    Private Async Sub frmConfiguracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Cargar la lista de cargos al iniciar el formulario
        CargarCargosEnTab()
    End Sub

    Private Sub btnGestionarIncidencias_Click(sender As Object, e As EventArgs) Handles btnGestionarIncidencias.Click
        If _gestionIncidenciasInstancia Is Nothing OrElse _gestionIncidenciasInstancia.IsDisposed Then
            _gestionIncidenciasInstancia = New frmGestionIncidencias()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionIncidenciasInstancia)
    End Sub

    Private Sub btnCargos_Click(sender As Object, e As EventArgs) Handles btnCargos.Click
        If _gestionCargosInstancia Is Nothing OrElse _gestionCargosInstancia.IsDisposed Then
            _gestionCargosInstancia = New frmGestionCargos()

            ' Asignar el método a una variable delegada y suscribirse
            cargosModificadosDelegate = AddressOf CargarCargosEnTab
            AddHandler _gestionCargosInstancia.CargosModificados, cargosModificadosDelegate
        End If

        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionCargosInstancia)

        TabControlPrincipal.SelectedTab = tpCargos
    End Sub

    Private Async Sub CargarCargosEnTab()
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim lista = Await _cargoService.GetAllAsync()
            dgvCargosTab.DataSource = New BindingList(Of Cargo)(lista.ToList())
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub btnSecciones_Click(sender As Object, e As EventArgs) Handles btnSecciones.Click
        If _gestionSeccionesInstancia Is Nothing OrElse _gestionSeccionesInstancia.IsDisposed Then
            _gestionSeccionesInstancia = New frmGestionSecciones()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionSeccionesInstancia)
    End Sub

    Private Sub btnAreasTrabajo_Click(sender As Object, e As EventArgs) Handles btnAreasTrabajo.Click
        If _gestionAreasTrabajoInstancia Is Nothing OrElse _gestionAreasTrabajoInstancia.IsDisposed Then
            _gestionAreasTrabajoInstancia = New frmGestionAreasTrabajo()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionAreasTrabajoInstancia)
    End Sub

    Private Sub btnTurnos_Click(sender As Object, e As EventArgs) Handles btnTurnos.Click
        If _gestionTurnosInstancia Is Nothing OrElse _gestionTurnosInstancia.IsDisposed Then
            _gestionTurnosInstancia = New frmGestionTurnos()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionTurnosInstancia)
    End Sub

    Private Sub btnNomenclaturas_Click(sender As Object, e As EventArgs) Handles btnNomenclaturas.Click
        If _gestionNomenclaturasInstancia Is Nothing OrElse _gestionNomenclaturasInstancia.IsDisposed Then
            _gestionNomenclaturasInstancia = New frmGestionNomenclaturas()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionNomenclaturasInstancia)
    End Sub

    Private Sub btnTiposEstadoTransitorio_Click(sender As Object, e As EventArgs) Handles btnTiposEstadoTransitorio.Click
        If _gestionTiposEstadoTransitorioInstancia Is Nothing OrElse _gestionTiposEstadoTransitorioInstancia.IsDisposed Then
            _gestionTiposEstadoTransitorioInstancia = New frmGestionTiposEstadoTransitorio()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionTiposEstadoTransitorioInstancia)
    End Sub

    Private Sub btnCategoriasAusencia_Click(sender As Object, e As EventArgs) Handles btnCategoriasAusencia.Click
        If _gestionCategoriasAusenciaInstancia Is Nothing OrElse _gestionCategoriasAusenciaInstancia.IsDisposed Then
            _gestionCategoriasAusenciaInstancia = New frmGestionCategoriasAusencia()
        End If
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(_gestionCategoriasAusenciaInstancia)
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        ' Desuscribirse del evento antes de cerrar el formulario para evitar fugas de memoria
        If _gestionCargosInstancia IsNot Nothing Then
            RemoveHandler _gestionCargosInstancia.CargosModificados, cargosModificadosDelegate
        End If
        Me.Close()
    End Sub
End Class