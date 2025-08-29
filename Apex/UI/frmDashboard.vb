' /UI/frmDashboard.vb

Imports System.Data.Entity
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmDashboard
    Inherits Form

    ' Usamos un único diccionario para almacenar las instancias de los formularios reutilizables.
    Private ReadOnly _formularios As New Dictionary(Of String, Form)

    ' Referencias al botón y formulario actualmente activos/visibles.
    Private _currentBtn As Button
    Private _activeForm As Form

    Public Sub New()
        InitializeComponent()
        AddHandler Me.Shown, AddressOf frmDashboard_Shown

        ' Se asigna el mismo manejador de click a TODOS los botones del menú.
        ' Esto centraliza la lógica de navegación.
        AddHandler btnNuevoFuncionario.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnBuscarFuncionario.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnLicencias.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnNotificaciones.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnSanciones.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnConceptoFuncional.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnFiltros.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnNovedades.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnNomenclaturas.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnRenombrarPDFs.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnImportacion.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnViaticos.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnReportes.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnAnalisis.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnAnalisisPersonal.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnConfiguracion.Click, AddressOf AbrirFormularioDesdeMenu_Click
    End Sub

    Private Async Sub frmDashboard_Shown(sender As Object, e As EventArgs)
        Await CargarSemanaActualAsync()
        ' Abrir el formulario de búsqueda por defecto al iniciar la aplicación.
        btnBuscarFuncionario.PerformClick()
    End Sub

#Region "Lógica de Navegación Principal"

    ''' <summary>
    ''' Método público y centralizado para mostrar cualquier formulario en el panel de contenido.
    ''' Es el único punto de entrada para cambiar el formulario visible.
    ''' </summary>
    Public Sub AbrirFormEnPanel(formToShow As Form)
        ' Si el formulario solicitado ya está activo, no hacemos nada.
        If _activeForm Is formToShow Then
            Return
        End If

        ' Oculta el formulario que está activo actualmente.
        If _activeForm IsNot Nothing Then
            _activeForm.Hide()
        End If

        ' Si el formulario es nuevo y nunca ha sido añadido al panel, lo configuramos.
        If Not Me.panelContenido.Controls.Contains(formToShow) Then
            formToShow.TopLevel = False
            formToShow.FormBorderStyle = FormBorderStyle.None
            formToShow.Dock = DockStyle.Fill
            Me.panelContenido.Controls.Add(formToShow)

            ' Si un formulario "temporal" (que no se reutiliza) se cierra, volvemos al buscador.
            If Not _formularios.ContainsValue(formToShow) Then
                AddHandler formToShow.FormClosed, Sub(s, args)
                                                      ' Al cerrarse, quitamos el control del panel y lo liberamos de memoria.
                                                      Me.panelContenido.Controls.Remove(CType(s, Form))
                                                      CType(s, Form).Dispose()
                                                      ' Y volvemos al formulario de búsqueda.
                                                      btnBuscarFuncionario.PerformClick()
                                                  End Sub
            End If
        End If

        ' Activamos el nuevo formulario.
        _activeForm = formToShow
        _activeForm.Show()
        _activeForm.BringToFront()
    End Sub

    ' Evento centralizado que se dispara al hacer clic en cualquier botón del menú.
    Private Sub AbrirFormularioDesdeMenu_Click(sender As Object, e As EventArgs)
        Dim botonClickeado = CType(sender, Button)
        ActivateButton(botonClickeado) ' Cambia el estilo visual del botón.

        Dim formToShow As Form = Nothing

        Select Case botonClickeado.Name
            ' Caso especial: "Nuevo Funcionario" siempre crea una instancia nueva y no se guarda para reutilizar.
            Case "btnNuevoFuncionario"
                formToShow = New frmFuncionarioCrear()

                ' Todos los demás casos obtienen una instancia reutilizable.
            Case Else
                Dim formType As Type = ObtenerTipoDeFormulario(botonClickeado.Name)
                If formType IsNot Nothing Then
                    formToShow = ObtenerOcrearInstancia(formType)
                End If
        End Select

        If formToShow IsNot Nothing Then
            AbrirFormEnPanel(formToShow)
        End If
    End Sub

    ''' <summary>
    ''' Busca una instancia de formulario en el diccionario. Si no existe, la crea y la guarda.
    ''' </summary>
    Private Function ObtenerOcrearInstancia(formType As Type) As Form
        Dim formName As String = formType.Name
        If _formularios.ContainsKey(formName) Then
            Return _formularios(formName)
        Else
            Dim newForm = CType(Activator.CreateInstance(formType), Form)
            _formularios.Add(formName, newForm)
            Return newForm
        End If
    End Function

    ''' <summary>
    ''' Devuelve el tipo de formulario correspondiente al nombre de un botón del menú.
    ''' </summary>
    Private Function ObtenerTipoDeFormulario(nombreBoton As String) As Type
        Select Case nombreBoton
            Case "btnBuscarFuncionario" : Return GetType(frmFuncionarioBuscar)
            Case "btnLicencias" : Return GetType(frmGestionLicencias)
            Case "btnNotificaciones" : Return GetType(frmGestionNotificaciones)
            Case "btnSanciones" : Return GetType(frmGestionSanciones)
            Case "btnConceptoFuncional" : Return GetType(frmConceptoFuncionalApex)
            Case "btnFiltros" : Return GetType(frmFiltroAvanzado)
            Case "btnNovedades" : Return GetType(frmNovedades)
            Case "btnNomenclaturas" : Return GetType(frmGestionNomenclaturas)
            Case "btnRenombrarPDFs" : Return GetType(frmRenombrarPDF)
            Case "btnImportacion" : Return GetType(frmAsistenteImportacion)
            Case "btnViaticos" : Return GetType(frmReporteViaticos)
            Case "btnReportes" : Return GetType(frmReporteNovedades)
            Case "btnAnalisis" : Return GetType(frmAnalisisEstacionalidad)
            Case "btnAnalisisPersonal" : Return GetType(frmAnalisisFuncionarios)
            Case "btnConfiguracion" : Return GetType(frmConfiguracion)
            Case Else : Return Nothing
        End Select
    End Function

#End Region

#Region "UI Helpers (Estilos y Carga de Datos)"

    ' Cambia el estilo del botón activo.
    Private Sub ActivateButton(senderBtn As Object)
        If senderBtn Is Nothing OrElse Not TypeOf senderBtn Is Button Then Return
        DisableButton() ' Restablece el botón anteriormente activo.
        _currentBtn = CType(senderBtn, Button)
        _currentBtn.BackColor = Color.FromArgb(81, 81, 112)
        _currentBtn.ForeColor = Color.White
        _currentBtn.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
    End Sub

    ' Restablece el estilo del botón que deja de estar activo.
    Private Sub DisableButton()
        If _currentBtn IsNot Nothing Then
            _currentBtn.BackColor = Color.FromArgb(51, 51, 76)
            _currentBtn.ForeColor = Color.Gainsboro
            _currentBtn.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular)
        End If
    End Sub

    ' Carga el número de semana actual en el logo.
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