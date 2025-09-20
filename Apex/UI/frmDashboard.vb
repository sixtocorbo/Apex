' /UI/frmDashboard.vb

Public Class frmDashboard
    Inherits Form

    ' --- Config Responsivo ---
    Private _navExpandedWidth As Integer = 330          ' ancho base expandido (como en el diseñador)
    Private _navCollapsedWidth As Integer = 64           ' ancho colapsado: solo íconos/emojis
    Private _navRatio As Double = 0.18                   ' ~18% del ancho de la ventana para la barra expandida
    Private _contentMinWidth As Integer = 900            ' mínimo de área de contenido
    Private _navIsCollapsed As Boolean = False           ' estado actual
    Private ReadOnly _tip As New ToolTip()               ' tooltips para modo colapsado

    ' ---- Peek por hover + animación ----
    Private _hoverExpanded As Boolean = False
    Private _navAnimTimer As Timer
    Private _hoverOutTimer As Timer
    Private _animFrom As Integer
    Private _animTo As Integer
    Private _animStartTicks As Long
    Private _animDurationMs As Integer = 160             ' velocidad de la animación (ms)

    ' ========= INSTANCIAS PERSISTENTES =========
    ' Usamos un diccionario para almacenar instancias de formularios reutilizables (singletons embebidos).
    Private ReadOnly _formularios As New Dictionary(Of String, Form)

    ' Mapa: formulario -> nombre del control a enfocar al mostrarse.
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

    ' ========= NAVEGACIÓN ACTUAL =========
    Private _currentBtn As Button            ' botón activo del menú
    Private _activeForm As Form              ' form embebido actualmente visible

    ' Flag anti-reentrada / anti “spam click” de menú
    Private _navBusy As Boolean = False

    ' ========= PILA DE NAVEGACIÓN (Opción A) =========
    ' Guarda el historial de formularios padres cuando abrís un “hijo”.
    Private ReadOnly _navStack As New Stack(Of Form)()

    Public Sub New()
        InitializeComponent()
        AddHandler Me.Shown, AddressOf frmDashboard_Shown

        ' Asignar un único handler a todos los botones del menú
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

        ' Abrir Búsqueda por defecto
        btnBuscarFuncionario.PerformClick()

        ' Asegurar foco preferido
        Me.BeginInvoke(Sub()
                           If _activeForm IsNot Nothing Then EnfocarControlPreferido(_activeForm)
                       End Sub)
    End Sub

#Region "NAVEGACIÓN"

    ' ========= API para abrir formularios =========

    ''' <summary>
    ''' Método centralizado para mostrar formularios embebidos.
    ''' Si isChild=True, apila el formulario actual y ocúltalo, para restaurarlo al cerrar el hijo.
    ''' </summary>
    Public Sub AbrirFormEnPanel(formToShow As Form, Optional isChild As Boolean = False)
        ' --- Guard clause ---
        If formToShow Is Nothing Then
            Notifier.Error(Me, "No se pudo abrir la ventana: referencia nula.")
            Exit Sub
        End If

        ' Si ya viene disposed (por un error en el Load del ctor, por ejemplo), abortar elegante.
        If formToShow.IsDisposed Then
            Notifier.Error(Me, $"No se pudo abrir la ventana: el formulario '{formToShow.Name}' ya se cerró.")
            Exit Sub
        End If
        If _activeForm IsNot Nothing Then
            If isChild Then
                ' Si el formulario actual será un "padre",
                ' SIEMPRE lo ocultamos para poder volver a él,
                ' sin importar si es temporal o persistente.
                _navStack.Push(_activeForm)
                _activeForm.Hide()
            Else
                ' Si no es un padre (navegación normal desde el menú),
                ' aplicamos la lógica original de cerrar temporales.
                If Not _formularios.ContainsValue(_activeForm) Then
                    _activeForm.Close()
                Else
                    _activeForm.Hide()
                End If
            End If
        End If
        '' --- Manejo del activo ---
        'If _activeForm IsNot Nothing Then
        '    If isChild Then _navStack.Push(_activeForm)

        '    If Not _formularios.ContainsValue(_activeForm) Then
        '        _activeForm.Close()
        '    Else
        '        _activeForm.Hide()
        '    End If
        'End If

        ' --- Embebido ---
        If Not Me.panelContenido.Controls.Contains(formToShow) Then
            formToShow.TopLevel = False
            formToShow.FormBorderStyle = FormBorderStyle.None
            formToShow.Dock = DockStyle.Fill
            Me.panelContenido.Controls.Add(formToShow)

            AddHandler formToShow.FormClosed,
            Sub(s, args)
                Dim f = CType(s, Form)
                Me.panelContenido.Controls.Remove(f)
                ' f.Dispose() no es necesario: ya viene dispuesto tras Close/CloseReason
                If _navStack.Count > 0 Then
                    Dim volver = _navStack.Pop()
                    If volver IsNot Nothing AndAlso Not volver.IsDisposed Then
                        _activeForm = volver
                        Try
                            _activeForm.Show()
                            _activeForm.BringToFront()
                            EnfocarControlPreferido(_activeForm)
                        Catch ex As ObjectDisposedException
                            Notifier.Warn(Me, "La ventana anterior ya no está disponible. Volviendo al inicio.")
                            btnBuscarFuncionario.PerformClick()
                        End Try
                        ActualizarTituloDashboard()
                        Exit Sub
                    End If
                End If
                btnBuscarFuncionario.PerformClick()
                ActualizarTituloDashboard()
            End Sub
        End If

        ' --- Mostrar activo con resguardo ---
        _activeForm = formToShow
        Try
            _activeForm.Show()
            _activeForm.BringToFront()
            EnfocarControlPreferido(_activeForm)
        Catch ex As ObjectDisposedException
            Notifier.Error(Me, "No se pudo abrir la ventana: el objeto fue desechado durante la inicialización.")
            ' Fallback razonable
            If _navStack.Count > 0 Then
                Dim volver = _navStack.Pop()
                If volver IsNot Nothing AndAlso Not volver.IsDisposed Then
                    _activeForm = volver
                    _activeForm.Show()
                    _activeForm.BringToFront()
                Else
                    btnBuscarFuncionario.PerformClick()
                End If
            Else
                btnBuscarFuncionario.PerformClick()
            End If
        End Try

        ActualizarTituloDashboard()
    End Sub

    ''' <summary>
    ''' Abrir un formulario como “hijo” del actual (lo apila y restaura al cerrarse).
    ''' Útil para llamar desde cualquier form embebido.
    ''' </summary>
    Public Sub AbrirChild(formHijo As Form)
        AbrirFormEnPanel(formHijo, isChild:=True)
    End Sub

    ''' <summary>
    ''' Cierra formularios temporales (no persistentes) excepto uno opcional.
    ''' </summary>
    Private Sub CerrarFormulariosTemporales(Optional except As Form = Nothing)
        Dim temporales = panelContenido.Controls.OfType(Of Form)().
            Where(Function(f) Not _formularios.ContainsValue(f) AndAlso f IsNot except).
            ToList()

        For Each f In temporales
            If Not f.IsDisposed Then f.Close()
        Next
    End Sub

    ''' <summary>
    ''' Handler centralizado de los botones del menú.
    ''' Resetea la pila (nuevo flujo), cierra temporales y navega.
    ''' </summary>
    Private Sub AbrirFormularioDesdeMenu_Click(sender As Object, e As EventArgs)
        If _navBusy Then Return
        _navBusy = True

        ' Nuevo flujo de navegación → vaciar la pila
        _navStack.Clear()

        ' Cerrar temporales que quedaron colgados
        CerrarFormulariosTemporales()

        Try
            Dim botonClickeado = CType(sender, Button)
            ActivateButton(botonClickeado) ' Estilo visual
            panelContenido.Select()         ' Sacar foco del botón

            Dim formToShow As Form = Nothing

            ' Caso especial: “Buscar” → asegurarse de cerrar “Crear” si lo hubiera
            If botonClickeado.Name = "btnBuscarFuncionario" Then
                Dim formCrear = BuscarInstanciaExistente(Of frmFuncionarioCrear)()
                If formCrear IsNot Nothing Then
                    formCrear.Close()
                End If
            End If

            Select Case botonClickeado.Name
                Case "btnNuevoFuncionario"
                    ' No crear duplicados de Crear
                    Dim existente = BuscarInstanciaExistente(Of frmFuncionarioCrear)()
                    If existente IsNot Nothing Then
                        formToShow = existente
                    Else
                        formToShow = New frmFuncionarioCrear()
                    End If

                Case Else
                    Dim formType As Type = ObtenerTipoDeFormulario(botonClickeado.Name)
                    If formType IsNot Nothing Then
                        formToShow = ObtenerOcrearInstancia(formType)
                    End If
            End Select

            If formToShow IsNot Nothing Then
                ' Navegación “desde menú” → isChild := False (nuevo tope de stack)
                AbrirFormEnPanel(formToShow, isChild:=False)
            End If
            ' --- Auto-colapsar la barra tras la navegación ---
            If _hoverExpanded Then
                ' Si estaba abierta por “peek” (hover), cerrala con la animación existente
                CollapsePeekAnimated()
            ElseIf Not _navIsCollapsed Then
                ' Si estaba expandida “en serio”, colapsá con animación
                SetNavCollapsedState(True, manual:=True, applyWidthNow:=False)
                AnimateNavWidth(_navCollapsedWidth)
            End If

        Finally
            Me.BeginInvoke(Sub() _navBusy = False)
        End Try
    End Sub

    ''' <summary>
    ''' Busca una instancia en el diccionario. Si no existe o está disposed, crea y guarda una nueva.
    ''' </summary>
    Private Function ObtenerOcrearInstancia(formType As Type) As Form
        Dim formName As String = formType.Name

        If _formularios.ContainsKey(formName) Then
            Dim frmExistente = _formularios(formName)
            If frmExistente IsNot Nothing AndAlso Not frmExistente.IsDisposed Then
                Return frmExistente
            End If
        End If

        Dim newForm = CType(Activator.CreateInstance(formType), Form)
        _formularios(formName) = newForm
        Return newForm
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

#Region "UI HELPERS"

    ''' <summary>
    ''' Actualiza el título del Dashboard con la info del form activo y el conteo de embebidos.
    ''' </summary>
    Private Sub ActualizarTituloDashboard()
        Dim count As Integer = Me.panelContenido.Controls.OfType(Of Form)().Count()
        Dim tituloBase As String = "Sistema de Gestión Apex"
        Dim infoActiva As String = String.Empty

        If _activeForm IsNot Nothing AndAlso Not _activeForm.IsDisposed Then
            infoActiva = $"| Activo: {_activeForm.Text} | Formularios: {count}"
        Else
            infoActiva = $"| Formularios Abiertos: {count}"
        End If

        Me.Text = $"{tituloBase} {infoActiva}"
    End Sub

    ' Cambia el estilo del botón activo.
    Private Sub ActivateButton(senderBtn As Object)
        If senderBtn Is Nothing OrElse Not TypeOf senderBtn Is Button Then Return
        DisableButton()
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

    ' Enfoca el control preferido del formulario mostrado.
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
                                             ' Si está dentro de una TabPage, seleccionar la pestaña primero
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

                                             c.Select()
                                             Dim tb = TryCast(c, TextBoxBase)
                                             If tb IsNot Nothing Then tb.SelectAll()
                                             Exit Sub
                                         End If
                                     End If
                                 Next

                                 ' Fallback si ninguno estuvo disponible
                                 form.Select()
                             End Sub)
        Else
            form.BeginInvoke(Sub() form.Select())
        End If
    End Sub

    ' Obtiene una instancia ya embebida de un tipo de Form (si existe y no está disposed).
    Private Function BuscarInstanciaExistente(Of T As Form)() As T
        Return Me.panelContenido.Controls.OfType(Of T)().
            FirstOrDefault(Function(f) Not f.IsDisposed)
    End Function

#End Region

#Region "CARGA INICIAL / DATOS"

    ' Carga el número de semana actual en el logo (muestra “Régimen no definido” si no hay).
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

    Private Shared _tiposInitDone As Boolean = False

    Private Sub frmDashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ConfigurarEstilosIniciales()
        AplicarLayoutResponsivo(force:=True)

        ' Toggle con click en el logo y con Ctrl+B
        AddHandler panelLogo.Click, AddressOf ToggleNav
        AddHandler Me.KeyDown, AddressOf frmDashboard_KeyDown

        ' Reaccionar a cambios de tamaño
        AddHandler Me.Resize, AddressOf frmDashboard_Resize

        ' HiDPI (opcional si no está ya configurado)
        Me.AutoScaleMode = AutoScaleMode.Dpi

        ' ---------- INIT Peek por hover + Timers ----------
        _navAnimTimer = New Timer() With {.Interval = 15}
        AddHandler _navAnimTimer.Tick, AddressOf NavAnimTimer_Tick

        _hoverOutTimer = New Timer() With {.Interval = 220}
        AddHandler _hoverOutTimer.Tick, AddressOf HoverOutTimer_Tick

        AddHandler panelNavegacion.MouseEnter, AddressOf panelNavegacion_MouseEnter
        AddHandler panelNavegacion.MouseLeave, AddressOf panelNavegacion_MouseLeave
        AddHandler panelContenido.MouseEnter, AddressOf panelContenido_MouseEnter


        ' Mantener peek mientras recorro botones; cerrar al click
        For Each b In NavButtons()
            AddHandler b.MouseEnter, AddressOf panelNavegacion_MouseEnter
            AddHandler b.MouseLeave, AddressOf panelNavegacion_MouseLeave
            AddHandler b.Click, AddressOf NavButton_Click
        Next
        ' ---------------------------------------------------

        If Not _tiposInitDone Then
            Try
                Using uow As New UnitOfWork()
                    TiposEstadoCatalog.Init(uow) ' UNA sola vez por proceso
                End Using
                _tiposInitDone = True
            Catch ex As Exception
                ' Evita que la app crashee si la DB no está disponible al arrancar
                Notifier.Error(Me, "No se pudo inicializar TiposEstadoCatalog: " & ex.Message)
            End Try
        End If

        ActualizarTituloDashboard()
        '        Dim btnToggle As New Button() With {
        '    .Text = "📌",
        '    .Width = 40, .Height = 32,
        '    .Dock = DockStyle.Top,
        '    .FlatStyle = FlatStyle.Flat,
        '    .ForeColor = Color.Gainsboro,
        '    .BackColor = Color.FromArgb(39, 39, 58)
        '}
        '        btnToggle.FlatAppearance.BorderSize = 0
        '        panelNavegacion.Controls.Add(btnToggle)
        '        btnToggle.BringToFront()

        '        AddHandler btnToggle.Click, Sub()
        '                                        SetNavCollapsedState(Not _navIsCollapsed, manual:=True)
        '                                    End Sub
    End Sub
    Private Sub panelContenido_MouseEnter(sender As Object, e As EventArgs)
        If _hoverExpanded Then CollapsePeekAnimated()
    End Sub


#End Region

    '============================================================
    ' ⌨️ Atajo: Ctrl+B para colapsar/expandir
    Private Sub frmDashboard_KeyDown(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.B Then
            ToggleNav(Nothing, EventArgs.Empty)
        End If
    End Sub

    Private Sub frmDashboard_Resize(sender As Object, e As EventArgs)
        AplicarLayoutResponsivo()
    End Sub

    Private Sub ToggleNav(sender As Object, e As EventArgs)
        SetNavCollapsedState(Not _navIsCollapsed, manual:=True)
    End Sub
    Private Function ShouldAutoCollapse() As Boolean
        ' Calcular el ancho expandido que usarías
        Dim deseado As Integer = CInt(Math.Max(_navExpandedWidth, Me.ClientSize.Width * _navRatio))
        Dim maxPorContenido As Integer = Math.Max(220, Me.ClientSize.Width - _contentMinWidth)
        Dim anchoExpandido As Integer = Math.Min(deseado, maxPorContenido)

        Dim contenidoSiExpandido As Integer = Me.ClientSize.Width - anchoExpandido
        Return contenidoSiExpandido < _contentMinWidth
    End Function
    ' === Núcleo responsivo ===
    Private Sub AplicarLayoutResponsivo(Optional force As Boolean = False)
        Dim autoShouldCollapse As Boolean = ShouldAutoCollapse()
        If (autoShouldCollapse <> _navIsCollapsed) OrElse force Then
            SetNavCollapsedState(autoShouldCollapse)
        End If

        If Not _navIsCollapsed Then
            ' Ancho proporcional y respetando el mínimo de contenido
            Dim deseado As Integer = CInt(Math.Max(_navExpandedWidth, Me.ClientSize.Width * _navRatio))
            Dim maxPorContenido As Integer = Math.Max(220, Me.ClientSize.Width - _contentMinWidth)
            panelNavegacion.Width = Math.Min(deseado, maxPorContenido)
        Else
            panelNavegacion.Width = _navCollapsedWidth
        End If

        ' Evitar flicker al redimensionar
        SetDoubleBuffered(panelContenido)
        SetDoubleBuffered(panelNavegacion)
    End Sub

    ' === Estado colapsado/expandido (con opción de no aplicar ancho inmediato para animar) ===
    Private Sub SetNavCollapsedState(collapse As Boolean,
                                     Optional manual As Boolean = False,
                                     Optional applyWidthNow As Boolean = True)
        _navIsCollapsed = collapse

        ' Si el usuario colapsa/expande manualmente, actualizamos la proporción
        If manual AndAlso Not collapse Then
            _navRatio = Math.Min(0.4, Math.Max(0.12, panelNavegacion.Width / Math.Max(1.0, Me.ClientSize.Width)))
        End If

        ' Aplicar estilos a los botones según estado
        For Each b In NavButtons()
            If b.Tag Is Nothing Then b.Tag = b.Text ' guardo el texto original una sola vez
            b.AutoEllipsis = True
            b.FlatStyle = FlatStyle.Flat
            b.FlatAppearance.BorderSize = 0
            b.Margin = New Padding(0)
            b.Height = 56

            If _navIsCollapsed Then
                Dim full = CStr(b.Tag)
                b.TextAlign = ContentAlignment.MiddleCenter
                b.Padding = New Padding(0)
                b.Text = ExtractEmoji(full)
                _tip.SetToolTip(b, full.Trim())
            Else
                b.TextAlign = ContentAlignment.MiddleLeft
                b.Padding = New Padding(18, 0, 0, 0)
                b.Text = CStr(b.Tag)
                _tip.SetToolTip(b, Nothing)
            End If
        Next

        ' Logo y labels
        If _navIsCollapsed Then
            lblAppName.Visible = False
            lblSemanaActual.Visible = False
            panelLogo.Height = 72
        Else
            lblAppName.Visible = True
            lblSemanaActual.Visible = True
            panelLogo.Height = 122
        End If

        ' Ajustar ancho ahora o dejar que lo haga la animación
        If applyWidthNow Then
            panelNavegacion.Width = If(_navIsCollapsed, _navCollapsedWidth, GetExpandedWidth())
        End If

        panelNavegacion.PerformLayout()
        panelContenido.PerformLayout()
    End Sub

    ' === Hover / Peek ===
    Private Sub panelNavegacion_MouseEnter(sender As Object, e As EventArgs)
        ' Si está colapsada, expandir solo para "peek" con animación
        If Not _hoverExpanded AndAlso panelNavegacion.Width <= _navCollapsedWidth + 2 Then
            _hoverExpanded = True
            ' Cambiar a estado expandido, pero sin tocar el ancho (lo anima)
            SetNavCollapsedState(False, manual:=False, applyWidthNow:=False)
            AnimateNavWidth(GetExpandedWidth())
        End If
        ' Mientras el mouse está sobre la barra, no intentamos colapsar
        _hoverOutTimer.Stop()
    End Sub

    Private Sub panelNavegacion_MouseLeave(sender As Object, e As EventArgs)
        ' Esperar un poco y verificar si realmente salió (por si va a otro botón)
        If _hoverExpanded Then
            _hoverOutTimer.Stop()
            _hoverOutTimer.Start()
        End If
    End Sub

    Private Sub HoverOutTimer_Tick(sender As Object, e As EventArgs)
        If Not _hoverExpanded Then
            _hoverOutTimer.Stop()
            Return
        End If

        ' ¿El cursor está fuera de la barra (en coords de pantalla)?

        ' ANTES (Incorrecto para tu caso de uso):
        ' Dim r As Rectangle = panelNavegacion.RectangleToScreen(panelNavegacion.ClientRectangle)

        ' DESPUÉS (Correcto):
        ' Creamos un rectángulo usando el tamaño completo del control en la pantalla.
        ' Esto SÍ incluye la barra de scroll.
        Dim r As New Rectangle(panelNavegacion.PointToScreen(Point.Empty), panelNavegacion.Size)

        If Not r.Contains(Cursor.Position) Then
            _hoverOutTimer.Stop()
            CollapsePeekAnimated()
        End If
    End Sub

    Private Sub NavButton_Click(sender As Object, e As EventArgs)
        ' Al seleccionar algo en modo peek, contraer
        If _hoverExpanded Then CollapsePeekAnimated()
    End Sub

    ' === Animación de ancho ===
    Private Sub AnimateNavWidth(targetWidth As Integer, Optional durationMs As Integer = -1)
        If durationMs <= 0 Then durationMs = _animDurationMs
        _animFrom = panelNavegacion.Width
        _animTo = targetWidth
        _animStartTicks = Environment.TickCount
        _navAnimTimer.Interval = 15
        _navAnimTimer.Start()
    End Sub

    Private Sub NavAnimTimer_Tick(sender As Object, e As EventArgs)
        Dim elapsed As Double = Environment.TickCount - _animStartTicks
        Dim t As Double = Math.Min(1.0, elapsed / Math.Max(1.0, _animDurationMs))
        ' Ease-out cúbica
        Dim ease As Double = 1 - Math.Pow(1 - t, 3)
        Dim w As Integer = CInt(_animFrom + ((_animTo - _animFrom) * ease))
        panelNavegacion.Width = w
        If t >= 1.0 Then
            _navAnimTimer.Stop()
        End If
    End Sub

    Private Sub CollapsePeekAnimated()
        _hoverExpanded = False
        ' Volver a estado colapsado (texto->íconos), sin tocar ancho aún
        SetNavCollapsedState(True, manual:=False, applyWidthNow:=False)
        AnimateNavWidth(_navCollapsedWidth)
    End Sub

    ' === Utilidades ===
    Private Iterator Function NavButtons() As IEnumerable(Of Button)
        ' Todos los Button dockeados en la barra, sin contar el panelLogo
        For Each c In panelNavegacion.Controls.OfType(Of Button)()
            Yield c
        Next
    End Function

    Private Function ExtractEmoji(fullText As String) As String
        ' Devuelve el primer token (emoji o palabra) del texto.
        If String.IsNullOrWhiteSpace(fullText) Then Return "•"
        Dim t = fullText.TrimStart()
        Dim sp = t.IndexOf(" "c)
        If sp > 0 Then
            Return t.Substring(0, sp) ' usualmente el emoji
        End If
        Return If(t.Length <= 2, t, t.Substring(0, 2))
    End Function

    Private Function GetExpandedWidth() As Integer
        Dim deseado As Integer = CInt(Math.Max(_navExpandedWidth, Me.ClientSize.Width * _navRatio))
        Dim maxPorContenido As Integer = Math.Max(220, Me.ClientSize.Width - _contentMinWidth)
        Return Math.Min(deseado, maxPorContenido)
    End Function

    Private Sub SetDoubleBuffered(ctrl As Control)
        Try
            Dim pi = GetType(Control).GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            If pi IsNot Nothing Then pi.SetValue(ctrl, True, Nothing)
        Catch
            ' no crítico
        End Try
    End Sub

    Private Sub ConfigurarEstilosIniciales()
        ' Colores de la barra (por si el diseñador cambia DPI)
        panelNavegacion.BackColor = Color.FromArgb(51, 51, 76)
        panelLogo.BackColor = Color.FromArgb(39, 39, 58)

        ' Botones: fuente y color coherente
        For Each b In NavButtons()
            b.ForeColor = Color.Gainsboro
            b.Cursor = Cursors.Hand
            b.TextImageRelation = TextImageRelation.Overlay
            b.ImageAlign = ContentAlignment.MiddleLeft
        Next

        ' Asegurar docking
        panelNavegacion.Dock = DockStyle.Left
        panelContenido.Dock = DockStyle.Fill
    End Sub

End Class
