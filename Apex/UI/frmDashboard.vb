' /UI/frmDashboard.vb

Imports System.Data.Entity
Imports System.Drawing
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports System.Linq
Public Class frmDashboard
    Inherits Form

    ' Usamos un único diccionario para almacenar las instancias de los formularios reutilizables.
    Private ReadOnly _formularios As New Dictionary(Of String, Form)

    ' Mapa: formulario -> nombre del control a enfocar al mostrarse.
    ' Agregá aquí otras pantallas si querés controlar su foco inicial.
    Private ReadOnly _controlFocoPreferido As New Dictionary(Of Type, String()) From {
    {GetType(frmFuncionarioBuscar), New String() {"txtBusqueda", "txtBuscar", "txtFiltro", "txtBusquedaGlobal"}},
    {GetType(frmFiltros), New String() {"txtBusquedaGlobal", "txtBuscar", "txtFiltroGlobal", "txtFiltro"}},
    {GetType(frmFuncionarioCrear), New String() {"txtCI", "txtCedula", "txtDocumento", "txtNombre"}},
    {GetType(frmLicencias), New String() {"txtBuscar", "txtBusqueda", "txtFiltro"}},
    {GetType(frmNotificaciones), New String() {"txtBuscar", "txtBusqueda", "txtFiltro", "txtAsunto"}},
    {GetType(frmSanciones), New String() {"txtBuscar", "txtBusqueda", "txtFiltro"}},
    {GetType(frmConceptoFuncional), New String() {"txtBuscar", "txtBusqueda", "txtNombre", "cmbConcepto"}},
    {GetType(frmNovedades), New String() {"txtBuscar", "txtBusqueda", "txtFiltro"}},
    {GetType(frmRenombrarPDF), New String() {"txtCarpeta", "txtRuta", "txtPrefijo", "btnSeleccionarCarpeta"}},
    {GetType(frmAsistenteImportacion), New String() {"txtArchivo", "btnSeleccionarArchivo"}},
    {GetType(frmViaticosListas), New String() {"txtBuscar", "txtBusqueda", "txtFiltro"}},
    {GetType(frmConfiguracion), New String() {"txtBuscar", "txtBusqueda", "txtRuta", "txtCarpeta"}}
}


    ' Referencias al botón y formulario actualmente activos/visibles.
    Private _currentBtn As Button
    Private _activeForm As Form

    ' Flag anti-reentrada / anti “spam click” de menú
    Private _navBusy As Boolean = False

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
        AddHandler btnRenombrarPDFs.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnImportacion.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnViaticos.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnReportes.Click, AddressOf AbrirFormularioDesdeMenu_Click
        AddHandler btnConfiguracion.Click, AddressOf AbrirFormularioDesdeMenu_Click
    End Sub

    Private Async Sub frmDashboard_Shown(sender As Object, e As EventArgs)
        Await CargarSemanaActualAsync()
        ' Abrir el formulario de búsqueda por defecto al iniciar la aplicación.
        btnBuscarFuncionario.PerformClick()

        ' Asegurar foco en el control preferido del form activo después de cargar.
        Me.BeginInvoke(Sub()
                           If _activeForm IsNot Nothing Then EnfocarControlPreferido(_activeForm)
                       End Sub)
    End Sub

#Region "Lógica de Navegación Principal"

    ''' <summary>
    ''' Método público y centralizado para mostrar cualquier formulario en el panel de contenido.
    ''' Es el único punto de entrada para cambiar el formulario visible.
    ''' </summary>
    Public Sub AbrirFormEnPanel(formToShow As Form)
        ' Si el formulario solicitado ya está activo, igual forzamos el foco (por si clickean de nuevo el mismo botón del menú).
        If _activeForm Is formToShow Then
            EnfocarControlPreferido(formToShow)
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

        ' Clave: poner el foco al final del ciclo de mensajes.
        EnfocarControlPreferido(_activeForm)
    End Sub

    ' Evento centralizado que se dispara al hacer clic en cualquier botón del menú.
    Private Sub AbrirFormularioDesdeMenu_Click(sender As Object, e As EventArgs)
        If _navBusy Then Return
        _navBusy = True

        Try
            Dim botonClickeado = CType(sender, Button)
            ActivateButton(botonClickeado) ' Cambia el estilo visual del botón.

            ' Saca el foco del botón del menú para que no "lo recupere" luego.
            panelContenido.Select()

            Dim formToShow As Form = Nothing

            Select Case botonClickeado.Name
                ' Caso especial: "Nuevo Funcionario" -> NO crear duplicados
                Case "btnNuevoFuncionario"
                    ' Si ya existe una instancia embebida y visible/no disposed, reúsala.
                    Dim existente = BuscarInstanciaExistente(Of frmFuncionarioCrear)()
                    If existente IsNot Nothing Then
                        formToShow = existente
                    Else
                        formToShow = New frmFuncionarioCrear()
                    End If

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

        Finally
            ' Liberamos el lock al final del ciclo de mensajes para permitir un nuevo click.
            Me.BeginInvoke(Sub() _navBusy = False)
        End Try
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
            Case "btnLicencias" : Return GetType(frmLicencias)
            Case "btnNotificaciones" : Return GetType(frmNotificaciones)
            Case "btnSanciones" : Return GetType(frmSanciones)
            Case "btnConceptoFuncional" : Return GetType(frmConceptoFuncional)
            Case "btnFiltros" : Return GetType(frmFiltros)
            Case "btnNovedades" : Return GetType(frmNovedades)
            Case "btnRenombrarPDFs" : Return GetType(frmRenombrarPDF)
            Case "btnImportacion" : Return GetType(frmAsistenteImportacion)
            Case "btnViaticos" : Return GetType(frmViaticosListas)
            Case "btnReportes" : Return GetType(frmReportes)
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

    ' Helper: enfoca el control preferido del formulario mostrado.
    ' Helper: enfoca el control preferido del formulario mostrado.
    Private Sub EnfocarControlPreferido(form As Form)
        Dim preferidos As String() = Nothing
        If _controlFocoPreferido.TryGetValue(form.GetType(), preferidos) Then
            form.BeginInvoke(Sub()
                                 If form.IsDisposed OrElse Not form.Visible Then Return

                                 For Each nombre In preferidos
                                     Dim encontrados = form.Controls.Find(nombre, True)
                                     If encontrados IsNot Nothing AndAlso encontrados.Length > 0 Then
                                         Dim c = encontrados(0)

                                         If c.Visible AndAlso c.Enabled AndAlso c.CanSelect Then
                                             ' Si está dentro de un TabPage, seleccionamos la pestaña primero
                                             Dim padre As Control = c
                                             While padre IsNot Nothing
                                                 Dim tp = TryCast(padre, TabPage)
                                                 If tp IsNot Nothing Then
                                                     Dim tc = TryCast(tp.Parent, TabControl)
                                                     If tc IsNot Nothing Then tc.SelectedTab = tp
                                                     Exit While
                                                 End If
                                                 padre = padre.Parent
                                             End While

                                             c.Select() ' mejor que Focus() para WinForms embebidos
                                             Dim tb = TryCast(c, TextBoxBase)
                                             If tb IsNot Nothing Then tb.SelectAll()
                                             Exit Sub
                                         End If
                                     End If
                                 Next

                                 ' Fallback si ninguno de los preferidos estuvo disponible
                                 form.Select()
                             End Sub)
        Else
            ' Si no hay preferencia registrada, al menos sacamos el foco del botón del menú
            form.BeginInvoke(Sub() form.Select())
        End If
    End Sub


    ' Helper: obtiene una instancia ya embebida de un tipo de Form (si existe y no está disposed).
    Private Function BuscarInstanciaExistente(Of T As Form)() As T
        Return Me.panelContenido.Controls.OfType(Of T)().
               FirstOrDefault(Function(f) Not f.IsDisposed)
    End Function

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

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using uow As New UnitOfWork()
            TiposEstadoCatalog.Init(uow) ' ← UNA sola vez por proceso
        End Using
    End Sub

#End Region

End Class
