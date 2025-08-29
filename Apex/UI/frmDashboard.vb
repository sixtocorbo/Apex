Imports System.Data.Entity
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmDashboard
    Inherits Form

    ' Botón y formulario activos actualmente
    Private currentBtn As Button
    Private Shadows activeForm As Form

    ' --- Instancias de formularios para reutilización ---
    Private _licenciasInstancia As frmGestionLicencias
    Private _notificacionesInstancia As frmGestionNotificaciones
    Private _sancionesInstancia As frmGestionSanciones
    Private _conceptoFuncionalInstancia As frmConceptoFuncionalApex
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

        ' 1. El botón del menú principal SOLO gestiona el submenú y su apariencia.
        AddHandler btnFuncionarios.Click, AddressOf ManejarClickMenuPrincipal

        ' 2. TODOS los demás botones que abren un formulario usan el mismo manejador.
        AddHandler btnNuevoFuncionario.Click, AddressOf AbrirFormulario_Click
        AddHandler btnBuscarFuncionario.Click, AddressOf AbrirFormulario_Click
        AddHandler btnLicencias.Click, AddressOf AbrirFormulario_Click
        AddHandler btnNotificaciones.Click, AddressOf AbrirFormulario_Click
        AddHandler btnSanciones.Click, AddressOf AbrirFormulario_Click
        AddHandler btnConceptoFuncional.Click, AddressOf AbrirFormulario_Click
        AddHandler btnFiltros.Click, AddressOf AbrirFormulario_Click
        AddHandler btnNovedades.Click, AddressOf AbrirFormulario_Click
        AddHandler btnNomenclaturas.Click, AddressOf AbrirFormulario_Click
        AddHandler btnRenombrarPDFs.Click, AddressOf AbrirFormulario_Click
        AddHandler btnImportacion.Click, AddressOf AbrirFormulario_Click
        AddHandler btnViaticos.Click, AddressOf AbrirFormulario_Click
        AddHandler btnReportes.Click, AddressOf AbrirFormulario_Click
        AddHandler btnAnalisis.Click, AddressOf AbrirFormulario_Click
        AddHandler btnAnalisisPersonal.Click, AddressOf AbrirFormulario_Click
        AddHandler btnConfiguracion.Click, AddressOf AbrirFormulario_Click
    End Sub

    Private Async Sub frmDashboard_Shown(sender As Object, e As EventArgs)
        Await CargarSemanaActualAsync()
        ' Abrir un formulario por defecto al iniciar la aplicación
        btnLicencias.PerformClick()
    End Sub

#Region "Lógica de Navegación del Menú"

    ' Evento para el botón principal "Funcionarios"
    Private Sub ManejarClickMenuPrincipal(sender As Object, e As EventArgs)
        ActivateButton(sender) ' Activa visualmente el botón "Funcionarios"
        pnlSubMenuFuncionario.Visible = Not pnlSubMenuFuncionario.Visible ' Muestra/oculta el submenú
    End Sub

    ' Evento centralizado para TODOS los botones que abren un formulario
    Private Sub AbrirFormulario_Click(sender As Object, e As EventArgs)
        Dim botonClickeado = CType(sender, Button)
        ActivateButton(botonClickeado) ' Activa visualmente el botón clickeado

        ' Si el botón pertenece al submenú, oculta el panel después del click
        If botonClickeado.Parent Is pnlSubMenuFuncionario Then
            pnlSubMenuFuncionario.Visible = False
        End If

        ' --- Lógica central para abrir el formulario correcto ---
        Select Case botonClickeado.Name
            ' --- Casos del submenú de Funcionarios ---
            Case "btnNuevoFuncionario"
                AbrirFormEnPanel(New frmFuncionarioCrear())

            Case "btnBuscarFuncionario"
                If _funcionarioBuscarInstancia Is Nothing OrElse _funcionarioBuscarInstancia.IsDisposed Then
                    _funcionarioBuscarInstancia = New frmFuncionarioBuscar()
                End If
                AbrirFormEnPanel(_funcionarioBuscarInstancia)

            ' --- Casos de los otros menús principales ---
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

            Case "btnFiltros"
                If _filtroAvanzadoInstancia Is Nothing OrElse _filtroAvanzadoInstancia.IsDisposed Then
                    _filtroAvanzadoInstancia = New frmFiltroAvanzado()
                End If
                AbrirFormEnPanel(_filtroAvanzadoInstancia)

            ' --- INICIO DE LA CORRECCIÓN: Casos Faltantes ---
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
            ' --- FIN DE LA CORRECCIÓN ---

            Case "btnConfiguracion"
                If _configuracionInstancia Is Nothing OrElse _configuracionInstancia.IsDisposed Then
                    _configuracionInstancia = New frmConfiguracion()
                End If
                AbrirFormEnPanel(_configuracionInstancia)
        End Select
    End Sub

    ' Método para gestionar la apertura de formularios en el panel principal
    Public Sub AbrirFormEnPanel(childForm As Form)
        ' Si ya hay un formulario abierto y es diferente al nuevo, ciérralo y límpialo de memoria.
        If activeForm IsNot Nothing AndAlso activeForm IsNot childForm Then
            activeForm.Close()
            activeForm.Dispose()
        End If

        ' Si el formulario que se quiere abrir es el mismo que ya está activo, no hagas nada.
        If activeForm Is childForm AndAlso Not childForm.IsDisposed Then
            Return
        End If

        activeForm = childForm
        childForm.TopLevel = False
        childForm.FormBorderStyle = FormBorderStyle.None
        childForm.Dock = DockStyle.Fill
        Me.panelContenido.Controls.Add(childForm)
        Me.panelContenido.Tag = childForm
        childForm.BringToFront()
        childForm.Show()
    End Sub

#End Region

#Region "UI Helpers (Estilos de botones y carga de datos)"

    ' Cambia el estilo del botón activo
    Private Sub ActivateButton(senderBtn As Object)
        If senderBtn Is Nothing Then Return
        DisableButton() ' Restablece el botón anteriormente activo
        currentBtn = CType(senderBtn, Button)
        currentBtn.BackColor = Color.FromArgb(81, 81, 112)
        currentBtn.ForeColor = Color.White
        currentBtn.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
    End Sub

    ' Restablece el estilo del botón que deja de estar activo
    Private Sub DisableButton()
        If currentBtn IsNot Nothing Then
            currentBtn.BackColor = Color.FromArgb(51, 51, 76)
            currentBtn.ForeColor = Color.Gainsboro
            currentBtn.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular)
        End If
    End Sub

    ' Carga el número de semana actual en el logo
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

#End Region

End Class