Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Public Class frmFuncionarioBuscar
    Inherits FormActualizable

    Public Enum ModoApertura
        Navegacion ' Para ver y editar desde el Dashboard
        Seleccion  ' Para seleccionar un funcionario y devolverlo
    End Enum

    Private ReadOnly _modo As ModoApertura
    Private Const LIMITE_FILAS As Integer = 500
    Private _detallesEstadoActual As New List(Of String)
    Private Shared ReadOnly Property RangoSituacionInicio As Date
        Get
            Return Date.Today
        End Get
    End Property

    Private Shared ReadOnly Property RangoSituacionFin As Date
        Get
            Return Date.Today.AddDays(1)
        End Get
    End Property

    ' --- INICIO DE CAMBIOS: Variables para la búsqueda mejorada ---
    Private WithEvents _searchTimer As New System.Windows.Forms.Timer()
    Private _ctsBusqueda As CancellationTokenSource
    Private _colorOriginalBusqueda As Color
    Private _estaBuscando As Boolean = False
    Private _resultadosBusqueda As New List(Of FuncionarioMin)()
    Private _cargoSeleccionado As String = Nothing
    Private _cargoResaltado As String = Nothing
    Private _suspendCargoEvents As Boolean = False
    ' --- FIN DE CAMBIOS ---


    Public ReadOnly Property FuncionarioSeleccionado As FuncionarioMin

        Get
            If dgvFuncionarios.CurrentRow IsNot Nothing Then
                Return CType(dgvFuncionarios.CurrentRow.DataBoundItem, FuncionarioMin)
            End If
            Return Nothing
        End Get
    End Property
    Private Function GetSelectedFuncionarioId() As Integer
        If FuncionarioSeleccionado IsNot Nothing Then
            Return FuncionarioSeleccionado.Id
        End If
        Return 0
    End Function

    Public Sub New()
        InitializeComponent()
        ConfigurarLayoutResponsivoBuscar()

        _modo = ModoApertura.Navegacion
    End Sub

    Public Sub New(modo As ModoApertura)
        Me.New()
        _modo = modo
    End Sub
#Region "Ciclo de Vida y Eventos Principales"
    Private Async Sub frmFuncionarioBuscar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarLayoutResponsivo()
        AjustarSplitter()
        AjustarAnchosTexto()
        btnVerSituacion.Tag = "KeepBackColor"
        panelDetalle.Visible = False
        AppTheme.Aplicar(Me)

        ' --- APLICAR MEJORAS ---
        dgvFuncionarios.ActivarDobleBuffer(True) ' <-- LÍNEA AÑADIDA
        ConfigurarGrilla()

        lstCargos.DisplayMember = "DisplayText"
        lstCargos.DrawMode = DrawMode.OwnerDrawFixed
        lstCargos.Enabled = False

        ' --- INICIO DE CAMBIOS: Configuración de la nueva búsqueda ---
        ' (El resto de tu código sigue igual)
        _searchTimer.Interval = 700 ' Aumentamos el intervalo
        _searchTimer.Enabled = False
        _colorOriginalBusqueda = txtBusqueda.BackColor

        AddHandler dgvFuncionarios.CurrentCellChanged, AddressOf MostrarDetalle
        AddHandler dgvFuncionarios.CellDoubleClick, AddressOf OnDgvDoubleClick

        Try
            AppTheme.SetCue(txtBusqueda, "Buscar por CI o Nombre…")
        Catch
        End Try

        ' Realizamos la primera carga de forma asíncrona
        Dim tk = ReiniciarToken()
        Await BuscarAsync(tk)
        ' --- FIN DE CAMBIOS ---
    End Sub

    ' Llamar después de InitializeComponent()
    Private Sub ConfigurarLayoutResponsivoBuscar()
        ' ===============================
        ' 1) SplitContainer: paneles con mínimo sensato
        ' ===============================
        With splitContenedor
            .Dock = DockStyle.Fill
            .Panel1MinSize = 320
            .Panel2MinSize = 380
            If .SplitterDistance < .Panel1MinSize Then
                .SplitterDistance = .Panel1MinSize
            End If
        End With

        ' ===============================
        ' 2) Búsqueda (arriba a la izquierda)
        ' ===============================
        With PanelBusquedaLista
            .Dock = DockStyle.Top
            .Padding = New Padding(12)
            .Height = 115
        End With

        With tlpBusqueda
            .Dock = DockStyle.Fill
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
            .ColumnStyles(0).SizeType = SizeType.AutoSize
            .ColumnStyles(1).SizeType = SizeType.Percent
            .ColumnStyles(1).Width = 100
            .RowStyles(0).SizeType = SizeType.AutoSize
            .RowStyles(1).SizeType = SizeType.AutoSize
        End With

        txtBusqueda.Anchor = AnchorStyles.Left Or AnchorStyles.Right

        With tlpResultados
            .Dock = DockStyle.Fill
            If .ColumnStyles.Count >= 2 Then
                .ColumnStyles(0).SizeType = SizeType.Percent
                .ColumnStyles(0).Width = 100
                .ColumnStyles(1).SizeType = SizeType.Absolute
            End If
            If .RowStyles.Count > 0 Then
                .RowStyles(0).SizeType = SizeType.Percent
                .RowStyles(0).Height = 100
            End If
        End With

        lstCargos.Dock = DockStyle.Fill
        lstCargos.Margin = New Padding(8, 0, 12, 12)

        With tlpDetalleVertical
            .Dock = DockStyle.Fill
            If .RowStyles.Count >= 4 Then
                .RowStyles(0).SizeType = SizeType.AutoSize      ' Acciones
                .RowStyles(1).SizeType = SizeType.Percent       ' Foto
                .RowStyles(1).Height = 50
                .RowStyles(2).SizeType = SizeType.AutoSize      ' Botón "Situación"
                .RowStyles(3).SizeType = SizeType.Percent       ' Detalles
                .RowStyles(3).Height = 50
            End If
            .Padding = New Padding(0)
        End With

        ' --- Foto: que se estire bien
        With pbFotoDetalle
            .Dock = DockStyle.Fill
            .SizeMode = PictureBoxSizeMode.Zoom
            .Margin = New Padding(3, 3, 3, 12)
            .MinimumSize = New Size(0, 160)
        End With

        ' --- Botón Ver Situación: ocupa ancho y se autoajusta
        With btnVerSituacion
            .Dock = DockStyle.Fill
            .Margin = New Padding(3, 0, 3, 6)
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
        End With

        ' --- Detalles: llenar con scroll vertical; no envolver horizontal
        With flpDetalles
            .Dock = DockStyle.Fill
            .AutoScroll = True
            .FlowDirection = FlowDirection.TopDown
            .WrapContents = False
            .Padding = New Padding(3)
            .Margin = New Padding(3, 0, 3, 0)
        End With

        ' ===============================
        ' 4) Botonera de acciones con WRAP (sustituye a tlpAcciones)
        ' ===============================
        ' Creamos un FlowLayoutPanel para acciones
        Dim flpAcciones As New FlowLayoutPanel() With {
        .Name = "flpAcciones",
        .AutoSize = True,
        .AutoSizeMode = AutoSizeMode.GrowAndShrink,
        .Dock = DockStyle.Fill,
        .FlowDirection = FlowDirection.RightToLeft,  ' alineado a la derecha
        .WrapContents = True,                         ' clave para que “baje de renglón”
        .Padding = New Padding(0, 0, 0, 6),
        .Margin = New Padding(0, 0, 0, 6)
    }

        ' Movemos los botones existentes del TableLayout a este FlowLayout
        ' (ordenados de derecha a izquierda para que visualmente queden [Notificar][Novedades][Sancionar][Ficha])
        Dim botones() As Button = {btnNotificar, btnNovedades, btnSancionar, btnGenerarFicha}
        For Each b In botones
            b.AutoSize = True
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink
            b.MinimumSize = New Size(96, 32)
            b.Margin = New Padding(6)
            b.Dock = DockStyle.None
            flpAcciones.Controls.Add(b)
        Next

        ' Quitamos tlpAcciones del layout y lo destruimos
        If tlpDetalleVertical.Controls.Contains(tlpAcciones) Then
            tlpDetalleVertical.Controls.Remove(tlpAcciones)
            tlpAcciones.Dispose()
        End If

        ' Insertamos flpAcciones en la fila 0, columna 0
        tlpDetalleVertical.Controls.Add(flpAcciones, 0, 0)

        ' ===============================
        ' 5) DataGrid a la izquierda: que siempre llene
        ' ===============================
        dgvFuncionarios.Dock = DockStyle.Fill
    End Sub

    ' Implementación requerida por la base:
    Protected Overrides Async Function RefrescarSegunFuncionarioAsync(e As FuncionarioCambiadoEventArgs) As Task
        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Return
        Dim tk = ReiniciarToken()
        Await BuscarAsync(tk)
    End Function
#End Region
    ' --- INICIO DE CAMBIOS: Método para reiniciar el token ---
    Private Function ReiniciarToken() As CancellationToken
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
        _ctsBusqueda = New CancellationTokenSource()
        Return _ctsBusqueda.Token
    End Function

    Private Sub dgvFuncionarios_SelectionChanged(sender As Object, e As EventArgs) Handles dgvFuncionarios.SelectionChanged
        btnCopiarContenido.Visible = (FuncionarioSeleccionado IsNot Nothing)
    End Sub

    Private Sub btnCopiarContenido_Click(sender As Object, e As EventArgs) Handles btnCopiarContenido.Click
        Dim funcionario = FuncionarioSeleccionado
        If funcionario Is Nothing Then
            Notifier.Warn(Me, "Seleccione un funcionario de la lista.")
            Return
        End If

        Dim opcion = SolicitarOpcionDeCopia()
        If opcion Is Nothing Then
            Return ' el usuario canceló la operación
        End If

        Dim copioCedula As Boolean = False
        Dim copioNombre As Boolean = False

        Select Case opcion.Value
            Case OpcionCopiado.Cedula
                copioCedula = True
            Case OpcionCopiado.Nombre
                copioNombre = True
            Case OpcionCopiado.Ambos
                copioCedula = Not String.IsNullOrWhiteSpace(funcionario.CI)
                copioNombre = Not String.IsNullOrWhiteSpace(funcionario.Nombre)
        End Select

        Dim textoACopiar As String = ObtenerTextoACopiar(funcionario, opcion.Value)
        If String.IsNullOrWhiteSpace(textoACopiar) Then
            Notifier.Warn(Me, "No hay datos disponibles para copiar en la selección actual.")
            Return
        End If

        Try
            Dim data As New DataObject()
            data.SetText(textoACopiar, TextDataFormat.UnicodeText)
            data.SetText(textoACopiar, TextDataFormat.Text)
            Clipboard.SetDataObject(data, True)

            Dim mensaje As String
            If copioCedula AndAlso copioNombre Then
                mensaje = "Cédula y nombre copiados al portapapeles."
            ElseIf copioCedula Then
                mensaje = "Cédula copiada al portapapeles."
            ElseIf copioNombre Then
                mensaje = "Nombre copiado al portapapeles."
            Else
                mensaje = "Datos copiados al portapapeles."
            End If

            Notifier.Success(Me, mensaje)
        Catch ex As Exception
            Notifier.Error(Me, "No se pudo copiar el contenido.")
        End Try
    End Sub

    Private Function ObtenerTextoACopiar(funcionario As FuncionarioMin, opcion As OpcionCopiado) As String
        Select Case opcion
            Case OpcionCopiado.Cedula
                Return funcionario.CI?.Trim()
            Case OpcionCopiado.Nombre
                Return funcionario.Nombre?.Trim()
            Case OpcionCopiado.Ambos
                Dim partes As New List(Of String)

                If Not String.IsNullOrWhiteSpace(funcionario.CI) Then
                    partes.Add(funcionario.CI.Trim())
                End If

                If Not String.IsNullOrWhiteSpace(funcionario.Nombre) Then
                    partes.Add(funcionario.Nombre.Trim())
                End If

                Return String.Join(" - ", partes)
            Case Else
                Return String.Empty
        End Select
    End Function

    Private Function SolicitarOpcionDeCopia() As OpcionCopiado?
        Using dialogo As New CopiarSeleccionDialog()
            Dim resultado = dialogo.ShowDialog(Me)
            If resultado = DialogResult.OK Then
                Return dialogo.OpcionSeleccionada
            End If
        End Using

        Return Nothing
    End Function

#Region "Atajos de Acciones"

    Private Sub btnNotificar_Click(sender As Object, e As EventArgs) Handles btnNotificar.Click
        If FuncionarioSeleccionado IsNot Nothing Then
            ' Se llama al nuevo constructor pasando el ID y el valor True
            Dim frm As New frmNotificacionCrear(FuncionarioSeleccionado.Id, True)
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
        End If
    End Sub

    Private Sub btnSancionar_Click(sender As Object, e As EventArgs) Handles btnSancionar.Click
        Dim selectedId = GetSelectedFuncionarioId()
        If selectedId <= 0 Then
            Notifier.Warn(Me, "Por favor, seleccione un funcionario de la grilla.")
            Return
        End If

        Dim frm As New frmSancionCrear(selectedId)
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnNovedades_Click(sender As Object, e As EventArgs) Handles btnNovedades.Click
        Dim funcionario = FuncionarioSeleccionado
        If funcionario IsNot Nothing Then
            ' Abre el formulario de novedades, idealmente filtrando por el funcionario.
            Dim frm As New frmNovedades()
            frm.ConfigurarParaFuncionario(funcionario.Id, funcionario.Nombre)
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
        End If
    End Sub

#End Region
    ' --- MODIFICADO: Ahora dispara el timer ---
    Private Sub txtBusqueda_TextChanged(sender As Object, e As EventArgs) Handles txtBusqueda.TextChanged
        _searchTimer.Stop()
        _searchTimer.Start()
    End Sub

    ' --- MODIFICADO: Inicia la búsqueda con el token ---
    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs) Handles _searchTimer.Tick
        _searchTimer.Stop()
        Dim tk = ReiniciarToken()
        Await BuscarAsync(tk)
    End Sub


    ' Es una buena práctica desuscribirse para evitar fugas de memoria.
    Private Sub frmFuncionarioBuscar_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        ' Limpieza del CTS para evitar fugas de memoria
        Try
            _ctsBusqueda?.Cancel()
            _ctsBusqueda?.Dispose()
        Catch
        End Try
    End Sub

    '    RemoveHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
    'End Sub

#Region "Diseño de grilla"
    ' Reemplaza este método en la región "Diseño de grilla"
    Private Sub ConfigurarGrilla()
        With dgvFuncionarios
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .GridColor = Color.FromArgb(230, 230, 230)
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AllowUserToResizeRows = False
            .AutoGenerateColumns = False
            .BackgroundColor = Color.White

            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells

            ' Estilos…
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

            .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)

            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Id",
                .DataPropertyName = "Id",
                .Visible = False
            })

            ' Nombre: llena el resto del ancho disponible
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Nombre",
                .DataPropertyName = "Nombre",
                .HeaderText = "Nombre",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                .MinimumWidth = 220,
                .DefaultCellStyle = New DataGridViewCellStyle With {
                    .WrapMode = DataGridViewTriState.False
                }
            })
        End With

        AddHandler dgvFuncionarios.DataError, Sub(s, ev) ev.ThrowException = False
        AddHandler dgvFuncionarios.DataBindingComplete, AddressOf AjustarColumnasVisibles
        AddHandler dgvFuncionarios.Resize, AddressOf AjustarColumnasVisibles
    End Sub
    Private Sub AjustarColumnasVisibles(Optional sender As Object = Nothing, Optional e As EventArgs = Nothing)
        If dgvFuncionarios.Columns.Count = 0 Then Exit Sub

        dgvFuncionarios.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells)

        For Each columna In dgvFuncionarios.Columns.Cast(Of DataGridViewColumn)().Where(Function(c) c.Visible)
            If columna.MinimumWidth > 0 AndAlso columna.Width < columna.MinimumWidth Then
                columna.Width = columna.MinimumWidth
            End If
        Next

        AjustarAnchoListaCargos()
    End Sub

    Private Sub AjustarAnchoListaCargos()
        If tlpResultados Is Nothing OrElse lstCargos Is Nothing Then Return
        If tlpResultados.ColumnStyles.Count < 2 Then Return
        Dim anchoContenido = CalcularAnchoContenidoListBox(lstCargos)
        Dim anchoPanel = Math.Max(0, splitContenedor.Panel1.ClientSize.Width)
        Dim anchoMaximoPermitido = Math.Min(CInt(anchoPanel * 0.28), 260)
        Dim anchoCalculado = Math.Max(140, Math.Min(anchoContenido, anchoMaximoPermitido))
        Dim margenHorizontal = lstCargos.Margin.Left + lstCargos.Margin.Right
        tlpResultados.ColumnStyles(1).Width = anchoCalculado + margenHorizontal
    End Sub

    Private Shared Function CalcularAnchoContenidoListBox(list As ListBox) As Integer
        If list.Items.Count = 0 Then
            Return 160
        End If

        Dim maxWidth As Integer = 0

        Using g = list.CreateGraphics()
            For Each item In list.Items
                Dim texto = list.GetItemText(item)
                If Not String.IsNullOrEmpty(texto) Then
                    Dim medida = System.Windows.Forms.TextRenderer.MeasureText(g, texto, list.Font)
                    If medida.Width > maxWidth Then
                        maxWidth = medida.Width
                    End If
                End If
            Next
        End Using

        Dim espacioScroll = If(list.Items.Count * Math.Max(1, list.ItemHeight) > Math.Max(1, list.ClientSize.Height), System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, 0)

        Return maxWidth + espacioScroll + 24
    End Function

    Private Sub EstablecerCeldaActualEnPrimeraColumnaVisible(fila As DataGridViewRow)
        If fila Is Nothing Then Return

        Dim columnaVisible = dgvFuncionarios.Columns _
            .Cast(Of DataGridViewColumn)() _
            .FirstOrDefault(Function(col) col.Visible)

        If columnaVisible IsNot Nothing Then
            dgvFuncionarios.CurrentCell = fila.Cells(columnaVisible.Index)
        End If
    End Sub


#End Region

#Region "Búsqueda con Full-Text y CONTAINS"
    ''' <summary>
    ''' Ejecuta una búsqueda asíncrona de funcionarios utilizando consultas Full-Text.
    ''' </summary>
    Private Async Function BuscarAsync(token As CancellationToken) As Task
        If _estaBuscando Then Return
        _estaBuscando = True

        ' Proporciona retroalimentación visual sin bloquear la interfaz.
        txtBusqueda.BackColor = Color.Gold
        Me.Cursor = Cursors.WaitCursor
        dgvFuncionarios.Enabled = False

        Try
            ' Si no hay texto, se limpia la grilla y se abandona la búsqueda.
            If String.IsNullOrWhiteSpace(txtBusqueda.Text) Then
                _resultadosBusqueda = New List(Of FuncionarioMin)()
                _cargoSeleccionado = Nothing
                ActualizarListaCargos(_resultadosBusqueda)
                AplicarFiltroCargo(False)
                Return
            End If

            token.ThrowIfCancellationRequested()

            Await Task.Delay(50, token) ' Pequeña pausa para refrescar UI

            Dim lista As List(Of FuncionarioMin)
            Using uow As New UnitOfWork()
                Dim ctx = uow.Context
                Dim filtro As String = txtBusqueda.Text.Trim()

                Dim terminos = filtro.Split(" "c).Where(Function(w) Not String.IsNullOrWhiteSpace(w)).Select(Function(w) $"""{w}*""")
                Dim expresionFts = String.Join(" AND ", terminos)

                Dim sb As New StringBuilder()
                sb.AppendLine("SELECT TOP (@limite) f.Id, f.CI, f.Nombre, ISNULL(c.Nombre, 'N/A') AS CargoNombre")
                sb.AppendLine("FROM dbo.Funcionario f WITH (NOLOCK)")
                sb.AppendLine("LEFT JOIN dbo.Cargo c ON f.CargoId = c.Id")
                sb.AppendLine("WHERE CONTAINS((f.CI, f.Nombre), @patron)")
                sb.AppendLine("ORDER BY f.Nombre;")

                Dim sql = sb.ToString()
                Dim pLimite = New SqlParameter("@limite", LIMITE_FILAS)
                Dim pPatron = New SqlParameter("@patron", expresionFts)

                ' Ejecuta la consulta respetando el token de cancelación.
                lista = Await ctx.Database.SqlQuery(Of FuncionarioMin)(sql, pLimite, pPatron).ToListAsync().WaitAsync(token)
            End Using

            token.ThrowIfCancellationRequested()

            _resultadosBusqueda = lista

            ActualizarListaCargos(_resultadosBusqueda)
            AplicarFiltroCargo(False)

            If _resultadosBusqueda.Count = LIMITE_FILAS Then
                Notifier.Info(Me, $"Mostrando los primeros {LIMITE_FILAS} resultados.")
            End If

        Catch ex As OperationCanceledException
            ' La cancelación es esperada cuando se inicia una nueva búsqueda.
        Catch ex As SqlException When ex.Number = -2
            Notifier.Warn(Me, "La consulta excedió el tiempo de espera.")
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error inesperado: " & ex.Message)
        Finally
            ' Restaura la interfaz a su estado inicial.
            If Not Me.IsDisposed Then
                txtBusqueda.BackColor = _colorOriginalBusqueda
                Me.Cursor = Cursors.Default
                dgvFuncionarios.Enabled = True
                txtBusqueda.Focus()
            End If
            _estaBuscando = False
        End Try
    End Function

    Private Sub lstCargos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstCargos.SelectedIndexChanged
        If _suspendCargoEvents Then Return

        Dim item = TryCast(lstCargos.SelectedItem, CargoItem)
        If item Is Nothing OrElse item.EsTodos Then
            _cargoSeleccionado = Nothing
        Else
            _cargoSeleccionado = item.Nombre
        End If

        AplicarFiltroCargo(True)
    End Sub

    Private Sub AplicarFiltroCargo(Optional mantenerSeleccion As Boolean = False)
        Dim fuente As List(Of FuncionarioMin) = If(_resultadosBusqueda, New List(Of FuncionarioMin)())
        Dim filtrados As List(Of FuncionarioMin)

        If String.IsNullOrEmpty(_cargoSeleccionado) Then
            filtrados = fuente.ToList()
        Else
            filtrados = fuente.Where(Function(f) String.Equals(NormalizarCargo(f.CargoNombre), _cargoSeleccionado, StringComparison.OrdinalIgnoreCase)).ToList()
        End If

        Dim idSeleccionado As Integer? = Nothing
        If mantenerSeleccion AndAlso dgvFuncionarios.CurrentRow IsNot Nothing Then
            idSeleccionado = CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value)
        End If

        dgvFuncionarios.DataSource = Nothing
        dgvFuncionarios.DataSource = filtrados

        If filtrados.Any() Then
            Dim indiceSeleccion As Integer = 0

            If mantenerSeleccion AndAlso idSeleccionado.HasValue Then
                Dim encontrado = filtrados.FindIndex(Function(f) f.Id = idSeleccionado.Value)
                If encontrado >= 0 Then
                    indiceSeleccion = encontrado
                End If
            End If

            dgvFuncionarios.ClearSelection()
            If indiceSeleccion >= 0 AndAlso indiceSeleccion < dgvFuncionarios.Rows.Count Then
                Dim filaSeleccionada = dgvFuncionarios.Rows(indiceSeleccion)
                filaSeleccionada.Selected = True
                EstablecerCeldaActualEnPrimeraColumnaVisible(filaSeleccionada)
            End If
        Else
            LimpiarDetalle()
        End If
    End Sub

    Private Sub ActualizarListaCargos(funcionarios As IList(Of FuncionarioMin))
        _suspendCargoEvents = True
        lstCargos.BeginUpdate()

        Try
            lstCargos.Items.Clear()

            If funcionarios Is Nothing OrElse funcionarios.Count = 0 Then
                lstCargos.Enabled = False
                lstCargos.SelectedIndex = -1
                _cargoSeleccionado = Nothing
                Return
            End If

            lstCargos.Enabled = True

            Dim grupos = funcionarios _
                .GroupBy(Function(f) NormalizarCargo(f.CargoNombre), StringComparer.OrdinalIgnoreCase) _
                .Select(Function(g) New With {.Nombre = g.Key, .Cantidad = g.Count()}) _
                .OrderByDescending(Function(g) g.Cantidad) _
                .ThenBy(Function(g) g.Nombre, StringComparer.CurrentCultureIgnoreCase) _
                .ToList()

            Dim total = funcionarios.Count
            lstCargos.Items.Add(New CargoItem("Todos", total, True))

            Dim indiceSeleccion As Integer = 0
            Dim anterior = _cargoSeleccionado
            Dim indiceActual As Integer = 1

            For Each grupo In grupos
                Dim item = New CargoItem(grupo.Nombre, grupo.Cantidad, False)
                lstCargos.Items.Add(item)

                If Not String.IsNullOrEmpty(anterior) AndAlso anterior.Equals(grupo.Nombre, StringComparison.OrdinalIgnoreCase) Then
                    indiceSeleccion = indiceActual
                End If

                indiceActual += 1
            Next

            If indiceSeleccion = 0 Then
                _cargoSeleccionado = Nothing
            End If

            lstCargos.SelectedIndex = indiceSeleccion
        Finally
            lstCargos.EndUpdate()
            _suspendCargoEvents = False
            AjustarAnchoListaCargos()
            lstCargos.Invalidate()
        End Try
    End Sub

    Private Sub ActualizarResaltadoCargo(cargoNormalizado As String)
        Dim nuevoValor = If(String.IsNullOrWhiteSpace(cargoNormalizado), Nothing, cargoNormalizado)

        Dim haCambiado As Boolean

        If nuevoValor Is Nothing Then
            haCambiado = _cargoResaltado IsNot Nothing
        Else
            haCambiado = Not String.Equals(_cargoResaltado, nuevoValor, StringComparison.OrdinalIgnoreCase)
        End If

        If haCambiado Then
            _cargoResaltado = nuevoValor
            lstCargos.Invalidate()
        End If
    End Sub

    Private Sub lstCargos_DrawItem(sender As Object, e As DrawItemEventArgs) Handles lstCargos.DrawItem
        e.DrawBackground()

        If e.Index < 0 OrElse e.Index >= lstCargos.Items.Count Then
            e.DrawFocusRectangle()
            Return
        End If

        Dim item = TryCast(lstCargos.Items(e.Index), CargoItem)
        If item IsNot Nothing Then
            Dim estaSeleccionado = (e.State And DrawItemState.Selected) = DrawItemState.Selected
            Dim colorTexto = If(estaSeleccionado, SystemColors.HighlightText, SystemColors.ControlText)

            If Not item.EsTodos AndAlso Not String.IsNullOrEmpty(_cargoResaltado) AndAlso
                item.Nombre.Equals(_cargoResaltado, StringComparison.OrdinalIgnoreCase) Then

                If Not estaSeleccionado Then
                    colorTexto = Color.DarkGreen
                End If
            End If

            TextRenderer.DrawText(e.Graphics, item.DisplayText, e.Font, e.Bounds, colorTexto,
                                  TextFormatFlags.Left Or TextFormatFlags.VerticalCenter)
        End If

        e.DrawFocusRectangle()
    End Sub

    Private Shared Function NormalizarCargo(nombre As String) As String
        If String.IsNullOrWhiteSpace(nombre) OrElse nombre.Equals("N/A", StringComparison.OrdinalIgnoreCase) Then
            Return "Sin cargo"
        End If

        Return nombre.Trim()
    End Function

    Private NotInheritable Class CargoItem
        Public Sub New(nombre As String, cantidad As Integer, esTodos As Boolean)
            Me.Nombre = nombre
            Me.Cantidad = cantidad
            Me.EsTodos = esTodos
        End Sub

        Public ReadOnly Property Nombre As String
        Public ReadOnly Property Cantidad As Integer
        Public ReadOnly Property EsTodos As Boolean

        Public ReadOnly Property DisplayText As String
            Get
                If EsTodos Then
                    Return $"Todos ({Cantidad})"
                End If

                Return $"{Nombre} ({Cantidad})"
            End Get
        End Property
    End Class

    ' Permite iniciar la búsqueda o seleccionar un funcionario con la tecla Enter.
    Private Async Sub txtBusqueda_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusqueda.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                e.Handled = True
                e.SuppressKeyPress = True
                _searchTimer.Stop()

                ' En modo selección se elige el funcionario; en modo navegación se ejecuta la búsqueda.
                If _modo = ModoApertura.Seleccion Then
                    AbrirOSeleccionarActual()
                Else
                    Dim tk = ReiniciarToken()
                    Await BuscarAsync(tk)
                End If

            Case Keys.Down
                If dgvFuncionarios.Rows.Count > 0 Then
                    e.Handled = True
                    MoverSeleccion(+1)
                End If

            Case Keys.Up
                If dgvFuncionarios.Rows.Count > 0 Then
                    e.Handled = True
                    MoverSeleccion(-1)
                End If
        End Select
    End Sub

    Private Sub MoverSeleccion(direccion As Integer)
        Dim total = dgvFuncionarios.Rows.Count
        If total = 0 Then
            LimpiarDetalle()
            Exit Sub
        End If

        Dim indexActual As Integer =
        If(dgvFuncionarios.CurrentRow Is Nothing, -1, dgvFuncionarios.CurrentRow.Index)

        Dim nuevoIndex = Math.Max(0, Math.Min(total - 1, indexActual + direccion))

        dgvFuncionarios.ClearSelection()
        Dim filaSeleccionada = dgvFuncionarios.Rows(nuevoIndex)
        filaSeleccionada.Selected = True
        EstablecerCeldaActualEnPrimeraColumnaVisible(filaSeleccionada)
    End Sub
#End Region


#Region "Detalle lateral (Foto on-demand)"
    Private Async Sub MostrarDetalle(sender As Object, e As EventArgs)
        Dim cargoActual As String = Nothing

        If dgvFuncionarios.CurrentRow IsNot Nothing AndAlso dgvFuncionarios.CurrentRow.DataBoundItem IsNot Nothing Then
            Dim funcionarioActual = TryCast(dgvFuncionarios.CurrentRow.DataBoundItem, FuncionarioMin)
            If funcionarioActual IsNot Nothing Then
                cargoActual = NormalizarCargo(funcionarioActual.CargoNombre)
            End If
        End If

        ActualizarResaltadoCargo(cargoActual)

        If dgvFuncionarios.CurrentRow Is Nothing OrElse dgvFuncionarios.CurrentRow.DataBoundItem Is Nothing Then
            LimpiarDetalle()
            Return
        End If

        panelDetalle.Visible = True
        Dim id = CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value)

        Using uow As New UnitOfWork()
            Dim f = Await uow.Repository(Of Funcionario)() _
            .GetAll() _
            .Include(Function(x) x.Cargo) _
            .Include(Function(x) x.Escalafon) _
            .Include(Function(x) x.SubEscalafon) _
            .Include(Function(x) x.TipoFuncionario) _
            .Include(Function(x) x.Semana) _
            .Include(Function(x) x.Turno) _
            .Include(Function(x) x.Horario) _
            .Include(Function(x) x.Seccion) _
            .Include(Function(x) x.PuestoTrabajo) _
            .Include(Function(x) x.SubDireccion) _
            .AsNoTracking() _
            .FirstOrDefaultAsync(Function(x) x.Id = id)

            If dgvFuncionarios.CurrentRow Is Nothing OrElse CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value) <> id Then Return
            If f Is Nothing Then Return

            ' Muestra los botones de acción
            btnSancionar.Visible = True
            btnNovedades.Visible = True
            btnNotificar.Visible = True
            btnGenerarFicha.Visible = True

            ' Concatenamos el texto fijo con el valor
            lblCIValor.Text = If(String.IsNullOrWhiteSpace(f.CI), "-", f.CI.Trim())
            lblNombreCompleto.Text = f.Nombre ' El nombre principal no lleva etiqueta
            Dim escalafonNombre = If(f.Escalafon IsNot Nothing, f.Escalafon.Nombre, "-")
            Dim subEscalafonNombre = If(f.SubEscalafon IsNot Nothing, f.SubEscalafon.Nombre, "-")
            Dim cargoNombre = If(f.Cargo IsNot Nothing, f.Cargo.Nombre, "-")
            lblEscalafonValor.Text = escalafonNombre
            lblSubEscalafonValor.Text = subEscalafonNombre
            lblJerarquiaValor.Text = cargoNombre
            lblTipoValor.Text = If(f.TipoFuncionario IsNot Nothing, f.TipoFuncionario.Nombre, "-")
            lblFechaIngresoValor.Text = If(f.FechaIngreso = Date.MinValue, "-", f.FechaIngreso.ToShortDateString())
            lblSemanaValor.Text = If(f.Semana IsNot Nothing, f.Semana.Nombre, "-")
            lblTurnoValor.Text = If(f.Turno IsNot Nothing, f.Turno.Nombre, "-")
            lblPlantillaValor.Text = If(f.Horario IsNot Nothing, f.Horario.Nombre, "-")
            lblUnidadValor.Text = If(f.Seccion IsNot Nothing, f.Seccion.Nombre, "-")
            lblPuestoValor.Text = If(f.PuestoTrabajo IsNot Nothing, f.PuestoTrabajo.Nombre, "-")
            lblSubDireccionValor.Text = If(f.SubDireccion IsNot Nothing, f.SubDireccion.Nombre, "-")

            ' El estado ya estaba correcto, lo mantenemos igual
            If f.Activo Then
                lblEstadoValor.Text = "Activo"
                lblEstadoValor.ForeColor = Color.DarkGreen
            Else
                lblEstadoValor.Text = "Inactivo"
                lblEstadoValor.ForeColor = Color.Maroon
            End If

            Dim situaciones = Await ObtenerSituacionesAsync(uow, id)

            AplicarSituacionesAlBoton(situaciones)

            lblPresenciaValor.Text = Await ObtenerPresenciaAsync(id, Date.Today)
            If Not f.Activo AndAlso (situaciones Is Nothing OrElse Not situaciones.Any()) Then
                lblPresenciaValor.Text = Await ObtenerPresenciaAsync(id, Date.Today)
            End If

            If f.Foto Is Nothing OrElse f.Foto.Length = 0 Then
                pbFotoDetalle.Image = My.Resources.Police
            Else
                Using ms As New MemoryStream(f.Foto)
                    pbFotoDetalle.Image = Image.FromStream(ms)
                End Using
            End If
        End Using

        ' Después de actualizar la vista, devuelve el foco al cuadro de búsqueda.
        txtBusqueda.Focus()
    End Sub

    Private Sub AplicarSituacionesAlBoton(situaciones As List(Of SituacionParaBoton))
        If situaciones IsNot Nothing AndAlso situaciones.Any() Then
            btnVerSituacion.Visible = True

            ' --- INICIO DE LA CORRECCIÓN ---
            ' 1. Aseguramos encontrar la situación de MÁXIMA severidad, sin depender del orden previo.
            Dim situacionMasGrave = situaciones.OrderByDescending(Function(s) s.Severidad).First()
            ' --- FIN DE LA CORRECCIÓN ---

            If _modo = ModoApertura.Seleccion Then
                btnVerSituacion.Enabled = False
                btnGenerarFicha.Enabled = False
                ToolTip1.SetToolTip(btnVerSituacion, "No disponible en modo de selección")
            Else
                btnVerSituacion.Enabled = True
                btnGenerarFicha.Enabled = True
            End If

            ' El texto SÍ depende de la cantidad total de situaciones.
            If situaciones.Count > 1 Then
                btnVerSituacion.Text = "Situación Múltiple"
            Else
                ' Si solo hay una, mostramos su tipo.
                btnVerSituacion.Text = situacionMasGrave.Tipo
            End If

            ' El color y el estilo se basan en la situación MÁS GRAVE encontrada.
            Dim colorSituacion = EstadoVisualHelper.ObtenerColor(situacionMasGrave.Severidad)
            Dim colorTexto = EstadoVisualHelper.ObtenerColorTexto(situacionMasGrave.Severidad)

            btnVerSituacion.BackColor = colorSituacion
            btnVerSituacion.ForeColor = colorTexto
            btnVerSituacion.UseVisualStyleBackColor = False
        Else
            btnVerSituacion.Visible = False
        End If
    End Sub
    Private Shared Async Function ObtenerSituacionesAsync(uow As UnitOfWork, funcionarioId As Integer) As Task(Of List(Of SituacionParaBoton))
        Dim fechaInicio = RangoSituacionInicio
        Dim fechaFin = RangoSituacionFin
        Dim situaciones As New List(Of SituacionParaBoton)()

        ' 1. OBTENER ESTADOS TRANSITORIOS (Lógica replicada de frmFuncionarioSituacion)
        Dim estadosTransitorios = Await uow.Context.Set(Of EstadoTransitorio)() _
            .Include("TipoEstadoTransitorio") _
            .Include("BajaDeFuncionarioDetalle") _
            .Include("CambioDeCargoDetalle") _
            .Include("DesarmadoDetalle") _
            .Include("DesignacionDetalle") _
            .Include("EnfermedadDetalle") _
            .Include("InicioDeProcesamientoDetalle") _
            .Include("OrdenCincoDetalle") _
            .Include("ReactivacionDeFuncionarioDetalle") _
            .Include("RetenDetalle") _
            .Include("SancionDetalle") _
            .Include("SeparacionDelCargoDetalle") _
            .Include("SumarioDetalle") _
            .Include("TrasladoDetalle") _
            .Where(Function(et) et.FuncionarioId = funcionarioId) _
            .AsNoTracking() _
            .ToListAsync()

        ' Procesar cada estado para encontrar los activos y extraer sus fechas
        For Each et In estadosTransitorios
            Dim desde As Date? = Nothing
            Dim hasta As Date? = Nothing
            Dim tipoNombre As String = et.TipoEstadoTransitorio.Nombre

            ' Extraer fechas de la tabla de detalle correspondiente
            et.GetFechas(desde, hasta) ' Usamos la extensión que SÍ existe

            ' Comprobar si el estado está activo en el rango de fechas actual
            If desde.HasValue AndAlso desde.Value < fechaFin AndAlso (Not hasta.HasValue OrElse hasta.Value >= fechaInicio) Then
                situaciones.Add(New SituacionParaBoton() With {
                    .Tipo = tipoNombre,
                    .Desde = desde,
                    .Hasta = hasta,
                    .Severidad = EstadoVisualHelper.DeterminarSeveridad(tipoNombre)
                })
            End If
        Next

        ' 2. OBTENER LICENCIAS (Lógica ya corregida)
        Dim licencias = Await uow.Context.Set(Of HistoricoLicencia)() _
            .Include(Function(l) l.TipoLicencia) _
            .Where(Function(l) l.FuncionarioId = funcionarioId AndAlso
                                l.inicio < fechaFin AndAlso
                                l.finaliza >= fechaInicio) _
            .AsNoTracking() _
            .ToListAsync()

        situaciones.AddRange(licencias.Select(Function(l)
                                                  Dim tipo = l.TipoLicencia.Nombre
                                                  Dim severidad = EstadoVisualHelper.DeterminarSeveridad(tipo)
                                                  Return New SituacionParaBoton() With {
                                                      .Tipo = tipo,
                                                      .Desde = l.inicio,
                                                      .Hasta = l.finaliza,
                                                      .Severidad = severidad
                                                  }
                                              End Function))

        ' 3. OBTENER NOTIFICACIONES PENDIENTES O VENCIDAS
        Dim estadoPendienteId As Byte = CByte(ModConstantesApex.EstadoNotificacionPersonal.Pendiente)
        Dim estadoVencidaId As Byte = CByte(ModConstantesApex.EstadoNotificacionPersonal.Vencida)
        Dim notificaciones = Await uow.Context.Set(Of NotificacionPersonal)() _
            .Include(Function(n) n.TipoNotificacion) _
            .Include(Function(n) n.NotificacionEstado) _
            .Where(Function(n) n.FuncionarioId = funcionarioId AndAlso
                    (n.EstadoId = estadoPendienteId OrElse n.EstadoId = estadoVencidaId) AndAlso
                    (n.FechaProgramada >= fechaInicio OrElse n.EstadoId = estadoVencidaId) AndAlso
                    n.FechaProgramada < fechaFin) _
            .AsNoTracking() _
            .ToListAsync()

        situaciones.AddRange(notificaciones.Select(Function(n) New SituacionParaBoton(n)))

        ' 4. ORDENAR CORRECTAMENTE
        Return situaciones _
            .OrderByDescending(Function(s) s.Severidad) _
            .ThenByDescending(Function(s) s.Desde.GetValueOrDefault(fechaInicio)) _
            .ThenBy(Function(s) s.Tipo) _
            .ToList()
    End Function

    Private Async Function RefrescarBotonSituacionAsync(funcionarioId As Integer) As Task
        Try
            Using uow As New UnitOfWork()
                Dim situaciones = Await ObtenerSituacionesAsync(uow, funcionarioId)
                ActualizarBotonSituacion(funcionarioId, situaciones)
            End Using
        Catch ex As Exception
            ' Ignoramos errores para no interrumpir la experiencia del usuario.
        End Try
    End Function

    Private Sub ActualizarBotonSituacion(funcionarioId As Integer, situaciones As List(Of SituacionParaBoton))
        If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return

        Dim actualizar As Action = Sub()
                                       If Me.IsDisposed Then Return
                                       If dgvFuncionarios.CurrentRow Is Nothing Then Return
                                       Dim idActual As Integer = CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value)
                                       If idActual <> funcionarioId Then Return
                                       AplicarSituacionesAlBoton(situaciones)
                                   End Sub

        If Me.InvokeRequired Then
            Try
                Me.BeginInvoke(actualizar)
            Catch ex As ObjectDisposedException
            End Try
        Else
            actualizar()
        End If
    End Sub


    ' Este código funciona bien si quieres abrir múltiples ventanas de situación.
    Private Sub btnVerSituacion_Click(sender As Object, e As EventArgs) Handles btnVerSituacion.Click
        If dgvFuncionarios.CurrentRow Is Nothing Then Return
        Dim id = CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value)
        Dim frm As New frmFuncionarioSituacion(id)
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Protected Overrides Async Function RefrescarSegunEstadoAsync(e As EstadoCambiadoEventArgs) As Task
        Await MyBase.RefrescarSegunEstadoAsync(e)

        If e Is Nothing Then Return
        If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return

        Await RefrescarBotonSituacionAsync(e.FuncionarioId)
    End Function

    Private Class SituacionParaBoton
        Public Property Tipo As String
        Public Property Desde As Date?
        Public Property Hasta As Date?
        Public Property Severidad As EstadoVisualHelper.EventoSeveridad
        Public Property TipoEvento As String

        Public Sub New()

        End Sub
        Public Sub New(notificacion As NotificacionPersonal)
            Me.TipoEvento = "Notificación"

            Dim estadoDescripcion = notificacion.NotificacionEstado?.Nombre?.Trim()
            Dim tipoBase = notificacion.TipoNotificacion?.Nombre?.Trim()
            If String.IsNullOrWhiteSpace(tipoBase) Then tipoBase = "Notificación"
            Me.Tipo = If(String.IsNullOrWhiteSpace(estadoDescripcion), tipoBase, $"{tipoBase} ({estadoDescripcion})")

            Me.Desde = notificacion.FechaProgramada
            Me.Hasta = Nothing

            Dim estadoVencidaId As Byte = CByte(ModConstantesApex.EstadoNotificacionPersonal.Vencida)
            Me.Severidad = If(notificacion.EstadoId = estadoVencidaId, EstadoVisualHelper.EventoSeveridad.Alta, EstadoVisualHelper.EventoSeveridad.Media)
        End Sub
    End Class
    ' Ejecuta la acción sobre la fila actual según el modo
    Private Sub AbrirOSeleccionarActual()
        If dgvFuncionarios.CurrentRow Is Nothing Then Exit Sub

        If _modo = ModoApertura.Seleccion Then
            SeleccionarYcerrar()
            Exit Sub
        End If

        ' === Modo Navegación: abrir editor del funcionario seleccionado ===
        Dim id As Integer = CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value)

        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard).FirstOrDefault()
        If dashboard Is Nothing Then Exit Sub

        Dim editorExistente = dashboard.panelContenido.Controls.
        OfType(Of frmFuncionarioCrear)().
        FirstOrDefault(Function(f) f.Tag IsNot Nothing AndAlso f.Tag.ToString() = id.ToString())

        If editorExistente IsNot Nothing Then
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(editorExistente)
        Else
            Dim frm As New frmFuncionarioCrear(id)
            frm.Tag = id.ToString()
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
        End If
    End Sub
    Private Sub dgvFuncionarios_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvFuncionarios.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            AbrirOSeleccionarActual()
        End If
    End Sub

    ' Busca y reemplaza este método en tu archivo frmFuncionarioBuscar.vb

    Private Sub OnDgvDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        AbrirOSeleccionarActual()
    End Sub
    Private Sub frmFuncionarioBuscar_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            AbrirOSeleccionarActual()
            Return
        End If

    End Sub
    Private Sub CancelarYCerrar()
        ' Si te abrieron con ShowDialog, devolvés Cancel; si es dentro del dashboard igual cierra.
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
    Private Async Function ObtenerPresenciaAsync(id As Integer, fecha As Date) As Task(Of String)
        Using uow As New UnitOfWork()
            Dim ctx = uow.Context
            Dim pFecha = New SqlParameter("@Fecha", fecha.Date)
            Dim lista = Await ctx.Database.SqlQuery(Of PresenciaDTO)(
                "EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha
            ).ToListAsync()
            Dim presencia = lista.Where(Function(r) r.FuncionarioId = id).
                                    Select(Function(r) r.Resultado).
                                    FirstOrDefault()
            Return If(presencia, "-")
        End Using
    End Function

    Private Sub LimpiarDetalle()
        lblCIValor.Text = "-"
        lblNombreCompleto.Text = "Seleccione un funcionario"
        lblEscalafonValor.Text = "-"
        lblSubEscalafonValor.Text = "-"
        lblJerarquiaValor.Text = "-"
        lblTipoValor.Text = "-"
        lblFechaIngresoValor.Text = "-"
        lblEstadoValor.Text = "-"
        lblEstadoValor.ForeColor = Color.DimGray
        lblPresenciaValor.Text = "-"
        pbFotoDetalle.Image = Nothing
        lblSemanaValor.Text = "-"
        lblTurnoValor.Text = "-"
        lblPlantillaValor.Text = "-"
        lblUnidadValor.Text = "-"
        lblPuestoValor.Text = "-"
        lblSubDireccionValor.Text = "-"
        _detallesEstadoActual.Clear()

        ' Oculta los botones de acción y el panel de detalle.
        btnGenerarFicha.Visible = False
        btnVerSituacion.Visible = False
        btnSancionar.Visible = False
        btnNovedades.Visible = False
        btnNotificar.Visible = False
        panelDetalle.Visible = False

        ActualizarResaltadoCargo(Nothing)
    End Sub

#End Region

#Region "Lógica de Selección"

    Private Sub SeleccionarYcerrar()
        If FuncionarioSeleccionado IsNot Nothing Then
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Else
            MessageBox.Show("Por favor, seleccione un funcionario de la lista.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnGenerarFicha_Click(sender As Object, e As EventArgs) Handles btnGenerarFicha.Click
        If FuncionarioSeleccionado IsNot Nothing Then
            Dim frm As New frmFichaPersonalRPT(FuncionarioSeleccionado.Id)
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
        Else
            MessageBox.Show("Por favor, seleccione un funcionario de la lista.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
#End Region

    ' --- INICIO DE NUEVA REGIÓN ---
#Region "Copiar al portapapeles"
    ''' <summary>
    ''' Cambia el color de fondo del ícono a verde por 1 segundo para notificar al usuario.
    ''' </summary>
    Private Async Function MostrarEfectoDeCopiaAsync(pb As PictureBox) As Task
        ' 1. Deshabilitamos el control para evitar múltiples clics.
        pb.Enabled = False

        ' 2. Guardamos el color de fondo original y lo cambiamos a un color de confirmación.
        Dim colorOriginal = pb.BackColor
        pb.BackColor = Color.PaleGreen ' Un verde suave es ideal para confirmación.

        ' 3. Esperamos 1 segundo (1000 milisegundos).
        Await Task.Delay(1000)

        ' 4. Restauramos el color original y volvemos a habilitar el control.
        pb.BackColor = colorOriginal
        pb.Enabled = True
    End Function

    Private Enum OpcionCopiado
        Cedula
        Nombre
        Ambos
    End Enum

    Private Class CopiarSeleccionDialog
        Inherits Form

        Private _opcionSeleccionada As OpcionCopiado?

        Public ReadOnly Property OpcionSeleccionada As OpcionCopiado?
            Get
                Return _opcionSeleccionada
            End Get
        End Property

        Public Sub New()
            Me.Text = "Copiar selección"
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.StartPosition = FormStartPosition.CenterParent
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.ShowInTaskbar = False
            Me.AutoSize = True
            Me.AutoSizeMode = AutoSizeMode.GrowAndShrink
            Me.Padding = New Padding(16)

            Dim layout As New TableLayoutPanel() With {
                .ColumnCount = 1,
                .RowCount = 3,
                .Dock = DockStyle.Fill,
                .AutoSize = True,
                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .Padding = New Padding(0),
                .Margin = New Padding(0)
            }

            Dim lblMensaje As New Label() With {
                .Text = "¿Qué desea copiar al portapapeles?",
                .AutoSize = True,
                .Dock = DockStyle.Fill,
                .Margin = New Padding(0, 0, 0, 8)
            }

            Dim lblDetalle As New Label() With {
                .Text = "Elija una de las opciones para continuar:",
                .AutoSize = True,
                .Dock = DockStyle.Fill,
                .Margin = New Padding(0, 0, 0, 16)
            }

            Dim panelBotones As New FlowLayoutPanel() With {
                .Dock = DockStyle.Fill,
                .FlowDirection = FlowDirection.LeftToRight,
                .AutoSize = True,
                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .WrapContents = True
            }

            Dim btnCedula As New Button() With {
                .Text = "Copiar cédula",
                .AutoSize = True,
                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .Margin = New Padding(0, 0, 8, 0)
            }
            AddHandler btnCedula.Click, Sub() Seleccionar(OpcionCopiado.Cedula)

            Dim btnNombre As New Button() With {
                .Text = "Copiar nombre",
                .AutoSize = True,
                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .Margin = New Padding(0, 0, 8, 0)
            }
            AddHandler btnNombre.Click, Sub() Seleccionar(OpcionCopiado.Nombre)

            Dim btnAmbos As New Button() With {
                .Text = "Copiar ambos",
                .AutoSize = True,
                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .Margin = New Padding(0, 0, 8, 0)
            }
            AddHandler btnAmbos.Click, Sub() Seleccionar(OpcionCopiado.Ambos)

            Dim btnCancelar As New Button() With {
                .Text = "Cancelar",
                .AutoSize = True,
                .AutoSizeMode = AutoSizeMode.GrowAndShrink
            }
            btnCancelar.DialogResult = DialogResult.Cancel

            panelBotones.Controls.AddRange(New Control() {btnCedula, btnNombre, btnAmbos, btnCancelar})

            layout.Controls.Add(lblMensaje, 0, 0)
            layout.Controls.Add(lblDetalle, 0, 1)
            layout.Controls.Add(panelBotones, 0, 2)

            Me.Controls.Add(layout)

            Me.CancelButton = btnCancelar
        End Sub

        Private Sub Seleccionar(opcion As OpcionCopiado)
            _opcionSeleccionada = opcion
            DialogResult = DialogResult.OK
        End Sub
    End Class


#End Region
    ' --- FIN DE NUEVA REGIÓN ---

#Region "DTO ligero"
    Public Class FuncionarioMin
        Public Property Id As Integer
        Public Property CI As String
        Public Property Nombre As String
        Public Property CargoNombre As String
    End Class
    Public Class PresenciaDTO
        Public Property FuncionarioId As Integer
        Public Property Resultado As String
    End Class

#End Region

    '==============
    Private _splitRatio As Double = 0.32
    Private Sub ConfigurarLayoutResponsivo()
        ' --- SplitContainer: libre y proporcional ---
        splitContenedor.Dock = DockStyle.Fill
        splitContenedor.FixedPanel = FixedPanel.None
        splitContenedor.IsSplitterFixed = False

        ' Recordar proporción cuando el usuario mueva el splitter
        AddHandler splitContenedor.SplitterMoved, AddressOf splitContenedor_SplitterMoved
        AddHandler Me.Resize, AddressOf frmFuncionarioBuscar_Resize

        ' --- Búsqueda (izquierda) ---
        ' Etiqueta autosize y textbox ocupa el resto
        If tlpBusqueda.ColumnStyles.Count >= 2 Then
            tlpBusqueda.ColumnStyles(0).SizeType = SizeType.AutoSize
            tlpBusqueda.ColumnStyles(1).SizeType = SizeType.Percent
            tlpBusqueda.ColumnStyles(1).Width = 100.0!
        End If

        ' --- Grilla (izquierda) ---
        dgvFuncionarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
        dgvFuncionarios.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells
        dgvFuncionarios.RowHeadersVisible = False
        dgvFuncionarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvFuncionarios.MultiSelect = False
        SetDoubleBuffered(dgvFuncionarios)

        ' --- Panel derecho: TableLayout en porcentajes ---
        tlpDetalleVertical.SuspendLayout()
        tlpDetalleVertical.RowStyles.Clear()
        tlpDetalleVertical.RowCount = 4
        ' Fila 0: Acciones (alto según contenido)
        tlpDetalleVertical.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        ' Fila 1: Foto (55% del alto disponible)
        tlpDetalleVertical.RowStyles.Add(New RowStyle(SizeType.Percent, 55.0!))
        ' Fila 2: Botón Situación (alto según contenido)
        tlpDetalleVertical.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        ' Fila 3: Detalles (45% del alto disponible)
        tlpDetalleVertical.RowStyles.Add(New RowStyle(SizeType.Percent, 45.0!))

        ' Foto: ya está en Zoom. Dejar Dock Fill garantiza que expanda/reduzca bien.
        pbFotoDetalle.Dock = DockStyle.Fill

        ' Detalles: que LLENE la celda y haga scroll, con items en columna
        flpDetalles.Dock = DockStyle.Fill
        flpDetalles.AutoScroll = True
        flpDetalles.WrapContents = False
        flpDetalles.FlowDirection = FlowDirection.TopDown

        ' Acciones: que el TLP se ajuste a su contenido dentro de su celda
        tlpAcciones.AutoSize = True
        tlpAcciones.AutoSizeMode = AutoSizeMode.GrowAndShrink

        tlpDetalleVertical.ResumeLayout()

        ' Ajustar ancho de etiquetas cuando cambie el tamaño del contenedor
        AddHandler flpDetalles.SizeChanged, AddressOf AjustarAnchosTexto

        ' (Opcional) Habilitar AutoScale por DPI si trabajás con monitores HiDPI
        ' Me.AutoScaleMode = AutoScaleMode.Dpi
    End Sub

    Private Sub frmFuncionarioBuscar_Resize(sender As Object, e As EventArgs)
        AjustarSplitter()
        AjustarAnchosTexto()
        AjustarAnchoListaCargos()
    End Sub

    Private Sub splitContenedor_SplitterMoved(sender As Object, e As SplitterEventArgs)
        _splitRatio = splitContenedor.SplitterDistance / Math.Max(1, splitContenedor.Width)
    End Sub

    Private Sub AjustarSplitter()
        Dim ancho = Math.Max(1, splitContenedor.Width)
        Dim deseado = CInt(ancho * _splitRatio)
        splitContenedor.SplitterDistance = Math.Max(splitContenedor.Panel1MinSize, Math.Min(deseado, ancho - splitContenedor.Panel2MinSize))
    End Sub

    Private Sub AjustarAnchosTexto(Optional sender As Object = Nothing, Optional e As EventArgs = Nothing)
        ' Para que las etiquetas hagan wrap y no se corten
        Dim margen As Integer = 24
        Dim anchoDisponible As Integer = Math.Max(120, flpDetalles.ClientSize.Width - margen)
        For Each lbl As Label In New Label() {
            lblNombreCompleto, lblCIValor, lblTipoValor, lblFechaIngresoValor,
            lblSemanaValor, lblTurnoValor, lblPlantillaValor,
            lblUnidadValor, lblPuestoValor, lblPresenciaValor, lblEstadoValor
        }
            lbl.MaximumSize = New Size(anchoDisponible, 0) ' 0 = altura auto
            lbl.AutoEllipsis = True
        Next

        For Each panel In New FlowLayoutPanel() {
            flpCIDetalle, flpTipoDetalle, flpFechaIngresoDetalle,
            flpHorarioDetalle, flpUbicacionDetalle, flpCargoDetalle,
            flpPresenciaDetalle, flpEstadoDetalle
        }
            panel.MaximumSize = New Size(anchoDisponible, 0)
        Next

        Dim preferido = lblNombreCompleto.GetPreferredSize(New Size(anchoDisponible, 0))
        lblNombreCompleto.MinimumSize = New Size(anchoDisponible, 0)
        lblNombreCompleto.MaximumSize = New Size(anchoDisponible, 0)
        lblNombreCompleto.Size = New Size(anchoDisponible, preferido.Height)
        lblNombreCompleto.TextAlign = ContentAlignment.MiddleCenter
    End Sub

    ' Reduce el flicker en la grilla cuando se redimensiona
    Private Sub SetDoubleBuffered(ctrl As Control)
        Try
            Dim pi = ctrl.GetType().GetProperty("DoubleBuffered", Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            If pi IsNot Nothing Then pi.SetValue(ctrl, True, Nothing)
        Catch
            ' Ignorar si no se puede (no crítico)
        End Try
    End Sub

End Class

