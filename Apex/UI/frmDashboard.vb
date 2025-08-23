Imports System.Data.Entity
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmDashboard
    Inherits Form

    Private currentBtn As Button
    Private Shadows activeForm As Form

    ' --- Instancias de los formularios para mantener su estado ---
    Private _funcionarioBuscarInstancia As frmFuncionarioBuscar
    Private _filtroAvanzadoInstancia As frmFiltroAvanzado
    Private _gestionInstancia As frmGestion
    Private _novedadesInstancia As frmNovedades
    Private _viaticosInstancia As frmGestionViaticos
    Private _importacionInstancia As frmAsistenteImportacion
    Private _gestionNomenclaturasInstancia As frmGestionNomenclaturas
    Private _renombrarPDFInstancia As frmRenombrarPDF
    Private _configuracionInstancia As frmConfiguracion ' --> INSTANCIA PARA EL FORM DE CONFIGURACIÓN

    Public Sub New()
        InitializeComponent()
        ' Asociar los manejadores de eventos a los botones de navegación
        AddHandler btnFuncionarios.Click, AddressOf ActivateButton
        AddHandler btnFiltros.Click, AddressOf ActivateButton
        AddHandler btnNovedades.Click, AddressOf ActivateButton
        AddHandler btnGestion.Click, AddressOf ActivateButton
        AddHandler btnNomenclaturas.Click, AddressOf ActivateButton
        AddHandler btnRenombrarPDFs.Click, AddressOf ActivateButton
        AddHandler btnImportacion.Click, AddressOf ActivateButton
        AddHandler btnViaticos.Click, AddressOf ActivateButton
        AddHandler btnReportes.Click, AddressOf ActivateButton
        AddHandler btnConfiguracion.Click, AddressOf ActivateButton

        AddHandler Me.Shown, AddressOf frmDashboard_Shown
    End Sub

    Private Async Sub frmDashboard_Shown(sender As Object, e As EventArgs)
        Await CargarSemanaActualAsync()
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

        Select Case currentBtn.Name
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

            Case "btnGestion"
                If _gestionInstancia Is Nothing OrElse _gestionInstancia.IsDisposed Then
                    _gestionInstancia = New frmGestion()
                End If
                AbrirFormEnPanel(_gestionInstancia)

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
                    _viaticosInstancia = New frmGestionViaticos()
                End If
                AbrirFormEnPanel(_viaticosInstancia)

            Case "btnReportes"
                MessageBox.Show("Formulario de reportes aún no implementado.", "En desarrollo", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Case "btnConfiguracion"
                ' --- CAMBIO AQUÍ: ABRIR DENTRO DEL PANEL ---
                If _configuracionInstancia Is Nothing OrElse _configuracionInstancia.IsDisposed Then
                    _configuracionInstancia = New frmConfiguracion()
                End If
                AbrirFormEnPanel(_configuracionInstancia)
                ' --- FIN DEL CAMBIO ---
            Case "btnReportes"
                ' --- MODIFICACIÓN AQUÍ ---
                ' Ahora el botón de reportes abrirá el nuevo formulario de novedades
                If _reporteNovedadesInstancia Is Nothing OrElse _reporteNovedadesInstancia.IsDisposed Then
                    _reporteNovedadesInstancia = New frmReporteNovedades()
                End If
                AbrirFormEnPanel(_reporteNovedadesInstancia)
                ' --- FIN DE LA MODIFICACIÓN ---
        End Select
    End Sub

    Private Sub DisableButton()
        If currentBtn IsNot Nothing Then
            currentBtn.BackColor = Color.FromArgb(51, 51, 76)
            currentBtn.ForeColor = Color.Gainsboro
            currentBtn.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular)
        End If
    End Sub

    Private Sub AbrirFormEnPanel(childForm As Form)
        If activeForm IsNot Nothing AndAlso activeForm IsNot childForm Then
            activeForm.Hide()
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