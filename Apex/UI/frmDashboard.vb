' /UI/frmDashboard.vb

Imports System.Data.Entity
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmDashboard
    Inherits Form

    ' Usamos un Dictionary para almacenar las instancias de los formularios reutilizables.
    ' La clave (String) es el nombre del tipo de formulario, el valor (Form) es la instancia.
    Private ReadOnly _formulariosReutilizables As New Dictionary(Of String, Form)

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
    ''' Método público que permite a un formulario hijo abrir otro formulario dentro del panel principal.
    ''' Es el punto de entrada para la navegación entre formularios (ej: desde Buscar a Editar).
    ''' </summary>
    Public Sub AbrirFormEnPanel(formHijo As Form)
        ' Oculta el formulario actualmente activo.
        If _activeForm IsNot Nothing Then
            _activeForm.Hide()
        End If

        ' Configura y muestra el nuevo formulario.
        _activeForm = formHijo
        _activeForm.TopLevel = False
        _activeForm.FormBorderStyle = FormBorderStyle.None
        _activeForm.Dock = DockStyle.Fill
        Me.panelContenido.Controls.Add(_activeForm)
        Me.panelContenido.Tag = _activeForm

        ' Cuando el formulario hijo se cierre, volvemos a mostrar el de búsqueda.
        AddHandler _activeForm.FormClosed, Sub(s, args)
                                               ' Nos aseguramos de que el formulario de búsqueda exista.
                                               If _formulariosReutilizables.ContainsKey(NameOf(frmFuncionarioBuscar)) Then
                                                   Dim formBusqueda = _formulariosReutilizables(NameOf(frmFuncionarioBuscar))
                                                   AbrirFormEnPanel(formBusqueda)
                                                   ActivateButton(btnBuscarFuncionario)
                                               End If
                                           End Sub

        _activeForm.BringToFront()
        _activeForm.Show()
    End Sub


    ' Evento centralizado que se dispara al hacer clic en cualquier botón del menú.
    Private Sub AbrirFormularioDesdeMenu_Click(sender As Object, e As EventArgs)
        Dim botonClickeado = CType(sender, Button)
        ActivateButton(botonClickeado) ' Cambia el estilo visual del botón.

        ' Decide qué formulario abrir basándose en el nombre del botón.
        Select Case botonClickeado.Name
            ' Caso especial: "Nuevo Funcionario" siempre crea una instancia nueva.
            Case "btnNuevoFuncionario"
                AbrirFormEnPanel(New frmFuncionarioCrear())

            ' Todos los demás casos reutilizan la misma instancia del formulario.
            Case "btnBuscarFuncionario"
                AbrirFormularioReutilizable(GetType(frmFuncionarioBuscar))
            Case "btnLicencias"
                AbrirFormularioReutilizable(GetType(frmGestionLicencias))
            Case "btnNotificaciones"
                AbrirFormularioReutilizable(GetType(frmGestionNotificaciones))
            Case "btnSanciones"
                AbrirFormularioReutilizable(GetType(frmGestionSanciones))
            Case "btnConceptoFuncional"
                AbrirFormularioReutilizable(GetType(frmConceptoFuncionalApex))
            Case "btnFiltros"
                AbrirFormularioReutilizable(GetType(frmFiltroAvanzado))
            Case "btnNovedades"
                AbrirFormularioReutilizable(GetType(frmNovedades))
            Case "btnNomenclaturas"
                AbrirFormularioReutilizable(GetType(frmGestionNomenclaturas))
            Case "btnRenombrarPDFs"
                AbrirFormularioReutilizable(GetType(frmRenombrarPDF))
            Case "btnImportacion"
                AbrirFormularioReutilizable(GetType(frmAsistenteImportacion))
            Case "btnViaticos"
                AbrirFormularioReutilizable(GetType(frmReporteViaticos))
            Case "btnReportes"
                AbrirFormularioReutilizable(GetType(frmReporteNovedades))
            Case "btnAnalisis"
                AbrirFormularioReutilizable(GetType(frmAnalisisEstacionalidad))
            Case "btnAnalisisPersonal"
                AbrirFormularioReutilizable(GetType(frmAnalisisFuncionarios))
            Case "btnConfiguracion"
                AbrirFormularioReutilizable(GetType(frmConfiguracion))
        End Select
    End Sub

    ''' <summary>
    ''' Gestiona la apertura de formularios que persisten en memoria (reutilizados).
    ''' </summary>
    Private Sub AbrirFormularioReutilizable(formType As Type)
        Dim formName As String = formType.Name
        Dim formToShow As Form

        ' Verifica si el formulario ya fue creado y está en nuestro diccionario.
        If _formulariosReutilizables.ContainsKey(formName) Then
            ' Si ya existe, simplemente se obtiene la referencia.
            formToShow = _formulariosReutilizables(formName)
        Else
            ' Si no existe, se crea, se almacena y se obtiene la referencia.
            formToShow = CType(Activator.CreateInstance(formType), Form)
            _formulariosReutilizables.Add(formName, formToShow)
        End If

        ' Llama al método central para mostrar el formulario.
        AbrirFormEnPanel(formToShow)
    End Sub

#End Region

#Region "UI Helpers (Estilos y Carga de Datos)"

    ' Cambia el estilo del botón activo.
    Private Sub ActivateButton(senderBtn As Object)
        If senderBtn Is Nothing OrElse TypeOf senderBtn IsNot Button Then Return
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