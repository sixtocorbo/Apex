' /UI/frmDashboard.vb

Imports System.Data.Entity
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmDashboard
    Inherits Form

    ' Usamos un Dictionary para almacenar las instancias únicas de los formularios reutilizables.
    ' La clave (String) es el nombre del tipo de formulario, el valor (Form) es la instancia.
    Private ReadOnly _formulariosAbiertos As New Dictionary(Of String, Form)

    ' Referencias al botón y formulario actualmente activos/visibles.
    Private currentBtn As Button
    Private Shadows activeForm As Form

    Public Sub New()
        InitializeComponent()
        AddHandler Me.Shown, AddressOf frmDashboard_Shown

        ' Se asigna el mismo manejador de click a TODOS los botones del menú.
        ' Esto centraliza la lógica de navegación.
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
        ' Abrir el formulario de licencias por defecto al iniciar la aplicación.
        btnLicencias.PerformClick()
    End Sub

#Region "Lógica de Navegación del Menú"

    ' Evento centralizado que se dispara al hacer clic en cualquier botón del menú.
    Private Sub AbrirFormulario_Click(sender As Object, e As EventArgs)
        Dim botonClickeado = CType(sender, Button)
        ActivateButton(botonClickeado) ' Cambia el estilo visual del botón.

        ' Decide qué formulario abrir basándose en el nombre del botón.
        Select Case botonClickeado.Name
            ' Caso especial: "Nuevo Funcionario" siempre crea una instancia nueva.
            Case "btnNuevoFuncionario"
                AbrirFormularioUnico(New frmFuncionarioCrear())

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
    ''' Gestiona la apertura de formularios que deben persistir en memoria (ser reutilizados).
    ''' </summary>
    Public Sub AbrirFormularioReutilizable(formType As Type)
        Dim formName As String = formType.Name

        ' Oculta el formulario que está activo actualmente.
        If activeForm IsNot Nothing Then
            activeForm.Hide()
        End If

        ' Verifica si el formulario ya fue creado y está en nuestro diccionario.
        If _formulariosAbiertos.ContainsKey(formName) Then
            ' Si ya existe, simplemente se activa y se muestra.
            activeForm = _formulariosAbiertos(formName)
            activeForm.Show()
            activeForm.BringToFront()

            ' Si el formulario puede actualizarse, se le ordena que lo haga.
            If TypeOf activeForm Is IFormularioActualizable Then
                CType(activeForm, IFormularioActualizable).ActualizarDatos()
            End If
        Else
            ' Si no existe, se crea, configura, almacena y muestra.
            Dim newForm = CType(Activator.CreateInstance(formType), Form)
            activeForm = newForm
            _formulariosAbiertos.Add(formName, activeForm)

            activeForm.TopLevel = False
            activeForm.FormBorderStyle = FormBorderStyle.None
            activeForm.Dock = DockStyle.Fill
            Me.panelContenido.Controls.Add(activeForm)
            activeForm.Show()
            activeForm.BringToFront()
        End If
    End Sub

    ''' <summary>
    ''' Gestiona la apertura de formularios que deben ser siempre una instancia nueva y se destruyen al cerrar.
    ''' </summary>
    Public Sub AbrirFormularioUnico(childForm As Form)
        If activeForm IsNot Nothing Then
            activeForm.Hide()
        End If

        activeForm = childForm
        activeForm.TopLevel = False
        activeForm.FormBorderStyle = FormBorderStyle.None
        activeForm.Dock = DockStyle.Fill
        Me.panelContenido.Controls.Add(activeForm)

        ' Se añade un manejador al evento 'FormClosed'. Cuando el formulario se cierre,
        ' se eliminará de los controles y se volverá a un formulario por defecto.
        AddHandler activeForm.FormClosed, Sub(s, args)
                                              Dim formCerrado = CType(s, Form)
                                              Me.panelContenido.Controls.Remove(formCerrado)
                                              formCerrado.Dispose()

                                              ' Vuelve a activar el botón y el formulario de búsqueda.
                                              btnBuscarFuncionario.PerformClick()
                                              End AddHandler

                                              activeForm.Show()
                                              activeForm.BringToFront()
                                          End Sub
    End Sub
#End Region

#Region "UI Helpers (Estilos y Carga de Datos)"

    ' Cambia el estilo del botón activo.
    Private Sub ActivateButton(senderBtn As Object)
        If senderBtn Is Nothing Then Return
        DisableButton() ' Restablece el botón anteriormente activo.
        currentBtn = CType(senderBtn, Button)
        currentBtn.BackColor = Color.FromArgb(81, 81, 112)
        currentBtn.ForeColor = Color.White
        currentBtn.Font = New Font("Segoe UI", 12.0F, FontStyle.Bold)
    End Sub

    ' Restablece el estilo del botón que deja de estar activo.
    Private Sub DisableButton()
        If currentBtn IsNot Nothing Then
            currentBtn.BackColor = Color.FromArgb(51, 51, 76)
            currentBtn.ForeColor = Color.Gainsboro
            currentBtn.Font = New Font("Segoe UI", 11.25F, FontStyle.Regular)
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