Imports System.Data.Entity
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmDashboard
    Inherits Form

    ' Botón y formulario activos actualmente
    Private currentBtn As Button
    Private Shadows activeForm As Form

    ' --- MODIFICACIÓN: Instancias para los nuevos formularios de gestión ---
    Private _licenciasInstancia As frmGestionLicencias
    Private _notificacionesInstancia As frmGestionNotificaciones
    Private _sancionesInstancia As frmGestionSanciones
    Private _conceptoFuncionalInstancia As frmConceptoFuncionalApex
    ' --- FIN MODIFICACIÓN ---

    ' Instancias para los otros formularios (se mantienen igual)
    Private _funcionarioBuscarInstancia As frmFuncionarioBuscar
    Private _filtroAvanzadoInstancia As frmFiltroAvanzado
    Private _novedadesInstancia As frmNovedades
    Private _viaticosInstancia As frmReporteViaticos
    Private _importacionInstancia As frmAsistenteImportacion
    Private _gestionNomenclaturasInstancia As frmGestionNomenclaturas
    Private _renombrarPDFInstancia As frmRenombrarPDF
    Private _reporteNovedadesInstancia As frmReporteNovedades
    Private _configuracionInstancia As frmConfiguracion
    Private _analisisEstacionalidadInstancia As frmAnalisisEstacionalidad
    Private _analisisPersonalInstancia As frmAnalisisFuncionarios


    Public Sub New()
        InitializeComponent()
        AddHandler Me.Shown, AddressOf frmDashboard_Shown

        ' --- MODIFICACIÓN: Se reemplaza btnGestion por los nuevos botones ---
        AddHandler btnLicencias.Click, AddressOf ActivateButton
        AddHandler btnNotificaciones.Click, AddressOf ActivateButton
        AddHandler btnSanciones.Click, AddressOf ActivateButton
        AddHandler btnConceptoFuncional.Click, AddressOf ActivateButton ' Se añade botón para Concepto Funcional
        ' --- FIN MODIFICACIÓN ---

        ' Handlers para los botones existentes
        AddHandler btnFuncionarios.Click, AddressOf ActivateButton
        AddHandler btnFiltros.Click, AddressOf ActivateButton
        AddHandler btnNovedades.Click, AddressOf ActivateButton
        AddHandler btnNomenclaturas.Click, AddressOf ActivateButton
        AddHandler btnRenombrarPDFs.Click, AddressOf ActivateButton
        AddHandler btnImportacion.Click, AddressOf ActivateButton
        AddHandler btnViaticos.Click, AddressOf ActivateButton
        AddHandler btnReportes.Click, AddressOf ActivateButton
        AddHandler btnAnalisis.Click, AddressOf ActivateButton
        AddHandler btnAnalisisPersonal.Click, AddressOf ActivateButton
        AddHandler btnConfiguracion.Click, AddressOf ActivateButton
    End Sub

    Private Async Sub frmDashboard_Shown(sender As Object, e As EventArgs)
        Await CargarSemanaActualAsync()
        ' Abrir el formulario de licencias por defecto al iniciar
        btnLicencias.PerformClick()
    End Sub

    Private Async Function CargarSemanaActualAsync() As Task
        Try
            Using uow As New UnitOfWork()
                Dim semana = Await uow.Context.Database.SqlQuery(Of Integer)("SELECT dbo.RegimenActual(GETDATE())").FirstOrDefaultAsync()
                If semana > 0 Then
                    lblSemanaActual.Text = $"SEMANA {semana}"
                Else
                    lblSemanaActual.Text = "Régimen no definido"
                End If
            End Using
        Catch ex As Exception
            lblSemanaActual.Text = "Error al cargar"
        End Try
    End Function

    Private Sub ActivateButton(sender As Object, e As EventArgs)
        If sender Is Nothing Then Return
        DisableButton()
        currentBtn = CType(sender, Button)
        currentBtn.BackColor = Color.FromArgb(81, 81, 112)
        currentBtn.ForeColor = Color.White
        currentBtn.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)

        ' --- MODIFICACIÓN: Lógica para manejar los nuevos formularios ---
        Select Case currentBtn.Name
            Case "btnLicencias"
                If _licenciasInstancia Is Nothing OrElse _licenciasInstancia.IsDisposed Then
                    _licenciasInstancia = New frmGestionLicencias()
                End If
                AbrirFormEnPanel(_licenciasInstancia)

            Case "btnNotificaciones"
                If _notificacionesInstancia Is Nothing OrElse _notificacionesInstancia.IsDisposed Then
                    _notificacionesInstancia = New frmGestionNotificaciones()
                End If
                AbrirFormEnPanel(_notificacionesInstancia)

            Case "btnSanciones"
                If _sancionesInstancia Is Nothing OrElse _sancionesInstancia.IsDisposed Then
                    _sancionesInstancia = New frmGestionSanciones()
                End If
                AbrirFormEnPanel(_sancionesInstancia)

            Case "btnConceptoFuncional"
                If _conceptoFuncionalInstancia Is Nothing OrElse _conceptoFuncionalInstancia.IsDisposed Then
                    _conceptoFuncionalInstancia = New frmConceptoFuncionalApex()
                End If
                AbrirFormEnPanel(_conceptoFuncionalInstancia)

            ' --- Casos para los otros botones (se mantienen igual) ---
            Case "btnFuncionarios"
                If _funcionarioBuscarInstancia Is Nothing OrElse _funcionarioBuscarInstancia.IsDisposed Then
                    _funcionarioBuscarInstancia = New frmFuncionarioBuscar()
                End If
                AbrirFormEnPanel(_funcionarioBuscarInstancia)

            Case "btnFiltros"
                If _filtroAvanzadoInstancia Is Nothing OrElse _filtroAvanzadoInstancia.IsDisposed Then
                    _filtroAvanzadoInstancia = New frmFiltroAvanzado()
                End If
                AbrirFormEnPanel(_filtroAvanzadoInstancia)

            Case "btnNovedades"
                If _novedadesInstancia Is Nothing OrElse _novedadesInstancia.IsDisposed Then
                    _novedadesInstancia = New frmNovedades()
                End If
                AbrirFormEnPanel(_novedadesInstancia)

            Case "btnNomenclaturas"
                If _gestionNomenclaturasInstancia Is Nothing OrElse _gestionNomenclaturasInstancia.IsDisposed Then
                    _gestionNomenclaturasInstancia = New frmGestionNomenclaturas()
                End If
                AbrirFormEnPanel(_gestionNomenclaturasInstancia)

            Case "btnRenombrarPDFs"
                If _renombrarPDFInstancia Is Nothing OrElse _renombrarPDFInstancia.IsDisposed Then
                    _renombrarPDFInstancia = New frmRenombrarPDF()
                End If
                AbrirFormEnPanel(_renombrarPDFInstancia)

            Case "btnImportacion"
                If _importacionInstancia Is Nothing OrElse _importacionInstancia.IsDisposed Then
                    _importacionInstancia = New frmAsistenteImportacion()
                End If
                AbrirFormEnPanel(_importacionInstancia)

            Case "btnViaticos"
                If _viaticosInstancia Is Nothing OrElse _viaticosInstancia.IsDisposed Then
                    _viaticosInstancia = New frmReporteViaticos()
                End If
                AbrirFormEnPanel(_viaticosInstancia)

            Case "btnReportes"
                If _reporteNovedadesInstancia Is Nothing OrElse _reporteNovedadesInstancia.IsDisposed Then
                    _reporteNovedadesInstancia = New frmReporteNovedades()
                End If
                AbrirFormEnPanel(_reporteNovedadesInstancia)

            Case "btnAnalisis"
                If _analisisEstacionalidadInstancia Is Nothing OrElse _analisisEstacionalidadInstancia.IsDisposed Then
                    _analisisEstacionalidadInstancia = New frmAnalisisEstacionalidad()
                End If
                AbrirFormEnPanel(_analisisEstacionalidadInstancia)

            Case "btnAnalisisPersonal"
                If _analisisPersonalInstancia Is Nothing OrElse _analisisPersonalInstancia.IsDisposed Then
                    _analisisPersonalInstancia = New frmAnalisisFuncionarios()
                End If
                AbrirFormEnPanel(_analisisPersonalInstancia)

            Case "btnConfiguracion"
                If _configuracionInstancia Is Nothing OrElse _configuracionInstancia.IsDisposed Then
                    _configuracionInstancia = New frmConfiguracion()
                End If
                AbrirFormEnPanel(_configuracionInstancia)
        End Select
    End Sub

    Private Sub DisableButton()
        If currentBtn IsNot Nothing Then
            currentBtn.BackColor = Color.FromArgb(51, 51, 76)
            currentBtn.ForeColor = Color.Gainsboro
            currentBtn.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular)
        End If
    End Sub

    Public Sub AbrirFormEnPanel(childForm As Form)
        ' Verifica si hay un formulario activo y si es diferente al nuevo.
        ' Si es así, lo oculta y libera sus recursos.
        If activeForm IsNot Nothing AndAlso activeForm IsNot childForm Then
            activeForm.Hide()
            activeForm.Dispose()
        End If
        activeForm = childForm
        If Not Me.panelContenido.Controls.Contains(childForm) Then
            childForm.TopLevel = False
            childForm.FormBorderStyle = FormBorderStyle.None
            childForm.Dock = DockStyle.Fill
            Me.panelContenido.Controls.Add(childForm)
            Me.panelContenido.Tag = childForm
        End If
        childForm.BringToFront()
        childForm.Show()
    End Sub

End Class