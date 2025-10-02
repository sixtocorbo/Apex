Imports System.IO
Imports System.Threading

Public Class frmNovedades
    Inherits FormActualizable

    Private _bsNovedades As New BindingSource()
    Private _pictureBoxSeleccionado As PictureBox = Nothing
    Private _idNovedadSeleccionada As Integer?
    Private _funcionariosSeleccionadosFiltro As New Dictionary(Of Integer, String)
    Private _esPrimeraNotificacionRecibida As Boolean = True

    ' --- Variables para la búsqueda mejorada ---
    Private WithEvents _searchTimer As New System.Windows.Forms.Timer()
    Private _ctsBusqueda As CancellationTokenSource
    Private _colorOriginalBusqueda As Color
    Private _estaBuscando As Boolean = False
    ' --- SOLUCIÓN: Bandera para controlar la condición de carrera ---
    Private _refrescandoPorNotificacion As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Async Sub ConfigurarParaFuncionario(funcionarioId As Integer, funcionarioNombre As String)
        If funcionarioId <= 0 Then Return

        chkFiltrarPorFecha.Checked = False

        _funcionariosSeleccionadosFiltro.Clear()
        _funcionariosSeleccionadosFiltro(funcionarioId) = funcionarioNombre
        ActualizarListaFuncionarios()

        If Me.IsHandleCreated Then
            Dim tk = ReiniciarToken()
            Await BuscarAsync(tk) ' <-- llamar sin asignar a "_"
        End If
    End Sub


#Region "Ciclo de Vida y Eventos Principales"

    Private Async Sub frmNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        dgvNovedades.ActivarDobleBuffer(True)
        ConfigurarGrilla()
        dgvNovedades.DataSource = _bsNovedades

        dtpFechaDesde.Value = DateTime.Now.AddMonths(-1)
        dtpFechaHasta.Value = DateTime.Now

        _searchTimer.Interval = 700
        _searchTimer.Enabled = False
        _colorOriginalBusqueda = txtBusqueda.BackColor

        Try
            AppTheme.SetCue(txtBusqueda, "Buscar por funcionario o novedad…")
        Catch
        End Try

        Dim tk = ReiniciarToken()
        Await BuscarAsync(tk)
        Notifier.Info(Me, "Tip: Doble clic en una fila abre el detalle.")
    End Sub

    Private Sub frmNovedades_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
    End Sub

    Protected Overrides Async Function RefrescarSegunNovedadAsync(e As NovedadCambiadaEventArgs) As Task
        If _estaBuscando Then Return
        _refrescandoPorNotificacion = True
        Try
            Dim tk = ReiniciarToken()
            Try
                Await BuscarAsync(tk)
            Catch oce As OperationCanceledException
                ' Cancelación normal por superposición de búsquedas
            End Try
        Finally
            _refrescandoPorNotificacion = False
        End Try
    End Function


    Protected Overrides Async Function RefrescarSegunFuncionarioAsync(e As FuncionarioCambiadoEventArgs) As Task
        If e Is Nothing OrElse Not Me.IsHandleCreated OrElse Me.IsDisposed Then Return

        ' Si ya hay una búsqueda corriendo, no dispares otra por arriba
        If _estaBuscando Then Return

        _refrescandoPorNotificacion = True
        Try
            ' Actualiza el filtro según el evento
            If Not e.FuncionarioId.HasValue Then
                _funcionariosSeleccionadosFiltro.Clear()
            Else
                Dim id = e.FuncionarioId.Value
                If Not _funcionariosSeleccionadosFiltro.ContainsKey(id) Then
                    Using uow As New UnitOfWork()
                        Dim funcionario = Await uow.Repository(Of Funcionario)().GetByIdAsync(id)
                        If funcionario IsNot Nothing Then
                            _funcionariosSeleccionadosFiltro(id) = funcionario.Nombre
                        End If
                    End Using
                End If
            End If

            ActualizarListaFuncionarios()

            ' Reiniciá el token y buscá, pero sin dejar que la cancelación burbujee
            Dim tk = ReiniciarToken()
            Try
                Await BuscarAsync(tk).ConfigureAwait(True)
            Catch oce As OperationCanceledException
                ' Cancelación esperable si se disparó otro refresco en el medio; no es error.
            End Try

        Finally
            _refrescandoPorNotificacion = False
        End Try
    End Function


    Private Async Sub dgvNovedades_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNovedades.SelectionChanged
        Await ActualizarDetalleDesdeSeleccion()
    End Sub

#End Region

#Region "Lógica de Búsqueda Asíncrona"

    Private Function ReiniciarToken() As CancellationToken
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
        _ctsBusqueda = New CancellationTokenSource()
        Return _ctsBusqueda.Token
    End Function

    Private Async Function BuscarAsync(token As CancellationToken) As Task
        If _estaBuscando Then Return
        _estaBuscando = True

        txtBusqueda.BackColor = Color.Gold
        Me.Cursor = Cursors.WaitCursor
        btnBuscar.Enabled = False
        dgvNovedades.Enabled = False
        LimpiarDetalles()

        Try
            token.ThrowIfCancellationRequested()
            Dim textoBusqueda As String = txtBusqueda.Text.Trim()
            Dim fechaDesde As DateTime? = If(chkFiltrarPorFecha.Checked, dtpFechaDesde.Value.Date, Nothing)
            Dim fechaHasta As DateTime? = If(chkFiltrarPorFecha.Checked, dtpFechaHasta.Value.Date.AddDays(1).AddTicks(-1), Nothing)
            Dim idsFuncionarios As List(Of Integer) = _funcionariosSeleccionadosFiltro.Keys.ToList()

            Dim lista As List(Of vw_NovedadesAgrupadas)
            Using svc As New NovedadService()
                lista = Await svc.BuscarNovedadesAvanzadoAsync(textoBusqueda, fechaDesde, fechaHasta, idsFuncionarios).WaitAsync(token)
            End Using

            token.ThrowIfCancellationRequested()
            _bsNovedades.DataSource = lista
            dgvNovedades.ClearSelection()

        Catch ex As OperationCanceledException
            ' Silencioso y esperado
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al buscar: {ex.Message}")
            _bsNovedades.DataSource = New List(Of vw_NovedadesAgrupadas)()
        Finally
            If Not Me.IsDisposed Then
                txtBusqueda.BackColor = _colorOriginalBusqueda
                Me.Cursor = Cursors.Default
                btnBuscar.Enabled = True
                dgvNovedades.Enabled = True
                If Not _refrescandoPorNotificacion Then
                    txtBusqueda.Focus()
                End If
            End If
            _estaBuscando = False
        End Try
    End Function

    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim tk = ReiniciarToken()
        Await BuscarAsync(tk)
    End Sub

    ' --- SOLUCIÓN: El evento TextChanged ahora respeta la bandera ---
    Private Sub txtBusqueda_TextChanged(sender As Object, e As EventArgs) Handles txtBusqueda.TextChanged
        ' 2. Si la bandera está levantada, ignora el evento y no reinicia el timer.
        If _refrescandoPorNotificacion Then Return
        _searchTimer.Stop()
        _searchTimer.Start()
    End Sub

    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs) Handles _searchTimer.Tick
        _searchTimer.Stop()
        Dim tk = ReiniciarToken()
        Await BuscarAsync(tk)
    End Sub

    Private Async Sub txtBusqueda_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusqueda.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                e.SuppressKeyPress = True
                _searchTimer.Stop()
                Dim tk = ReiniciarToken()
                Await BuscarAsync(tk)
            Case Keys.Down
                e.Handled = True
                MoverSeleccion(+1)
            Case Keys.Up
                e.Handled = True
                MoverSeleccion(-1)
        End Select
    End Sub

    Private Sub MoverSeleccion(direccion As Integer)
        If dgvNovedades.Rows.Count = 0 Then Return
        Dim indexActual As Integer = -1
        If dgvNovedades.CurrentRow IsNot Nothing Then
            indexActual = dgvNovedades.CurrentRow.Index
        End If
        Dim nuevoIndex = Math.Max(0, Math.Min(dgvNovedades.Rows.Count - 1, indexActual + direccion))
        If dgvNovedades.CurrentRow IsNot Nothing Then
            dgvNovedades.CurrentRow.Selected = False
        End If
        dgvNovedades.Rows(nuevoIndex).Selected = True
        dgvNovedades.CurrentCell = dgvNovedades.Rows(nuevoIndex).Cells(1) ' Seleccionar la primera celda visible
    End Sub

#End Region

#Region "Lógica del Detalle"

    Private Sub LimpiarDetalles()
        txtTextoNovedad.Clear()
        lstFuncionarios.DataSource = Nothing
        LimpiarFotos()
        _pictureBoxSeleccionado = Nothing
        _idNovedadSeleccionada = Nothing
    End Sub

    Private Sub ActualizarListaFuncionarios()
        lstFuncionariosFiltro.DataSource = Nothing
        If _funcionariosSeleccionadosFiltro.Any() Then
            lstFuncionariosFiltro.DataSource = New BindingSource(_funcionariosSeleccionadosFiltro, Nothing)
            lstFuncionariosFiltro.DisplayMember = "Value"
            lstFuncionariosFiltro.ValueMember = "Key"
        End If
    End Sub

    Private Async Function ActualizarDetalleDesdeSeleccion() As Task
        If dgvNovedades.CurrentRow Is Nothing OrElse dgvNovedades.CurrentRow.DataBoundItem Is Nothing Then
            LimpiarDetalles()
            Return
        End If

        Dim nov = TryCast(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        If nov Is Nothing OrElse (_idNovedadSeleccionada.HasValue AndAlso _idNovedadSeleccionada.Value = nov.Id) Then
            Return
        End If

        _idNovedadSeleccionada = nov.Id
        Try
            Using svc As New NovedadService()
                Dim novedadCompleta = Await svc.GetByIdAsync(nov.Id)
                txtTextoNovedad.Text = If(novedadCompleta IsNot Nothing, novedadCompleta.Texto, "")

                Dim funcionarios = Await svc.GetFuncionariosPorNovedadAsync(nov.Id)
                lstFuncionarios.DataSource = funcionarios
                lstFuncionarios.DisplayMember = "Nombre"
                lstFuncionarios.ValueMember = "Id"

                Await CargarFotos(nov.Id)
            End Using
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudo cargar el detalle: {ex.Message}")
            LimpiarDetalles()
        End Try
    End Function
#End Region

#Region "Gestión de Fotos (Visualización)"

    Private Function CrearImagenCopia(bytes() As Byte) As Image
        Using ms As New MemoryStream(bytes)
            Return Image.FromStream(ms)
        End Using
    End Function

    Private Sub LimpiarFotos()
        For Each ctrl As Control In flpFotos.Controls
            If TypeOf ctrl Is PictureBox Then
                Dim pic = CType(ctrl, PictureBox)
                pic.Image?.Dispose()
                pic.Dispose()
            End If
        Next
        flpFotos.Controls.Clear()
    End Sub

    Private Async Function CargarFotos(novedadId As Integer) As Task
        _pictureBoxSeleccionado = Nothing
        LimpiarFotos()
        Using svc As New NovedadService()
            Dim fotos = Await svc.GetFotosPorNovedadAsync(novedadId)
            For Each foto In fotos
                Dim pic As New PictureBox With {
                    .Image = CrearImagenCopia(foto.Foto),
                    .SizeMode = PictureBoxSizeMode.Zoom,
                    .Size = New Size(120, 120),
                    .Margin = New Padding(5),
                    .BorderStyle = BorderStyle.FixedSingle,
                    .Tag = foto.Id,
                    .Cursor = Cursors.Hand
                }
                AddHandler pic.DoubleClick, AddressOf PictureBox_DoubleClick
                AddHandler pic.Click, AddressOf PictureBox_Click
                flpFotos.Controls.Add(pic)
            Next
        End Using
    End Function

    Private Sub PictureBox_Click(sender As Object, e As EventArgs)
        Dim picClickeado = TryCast(sender, PictureBox)
        If picClickeado Is Nothing Then Return

        If _pictureBoxSeleccionado IsNot Nothing Then
            _pictureBoxSeleccionado.BackColor = Color.Transparent
        End If
        picClickeado.BackColor = Color.DodgerBlue
        _pictureBoxSeleccionado = picClickeado
    End Sub

    Private Sub PictureBox_DoubleClick(sender As Object, e As EventArgs)
        Dim pic = TryCast(sender, PictureBox)
        If pic IsNot Nothing AndAlso pic.Image IsNot Nothing Then
            AbrirChildEnDashboard(New frmFotografiaNovedades(pic.Image))
        End If
    End Sub
#End Region

#Region "Gestión de Novedades (CRUD) y Filtros"

    Private Sub btnNuevaNovedad_Click(sender As Object, e As EventArgs) Handles btnNuevaNovedad.Click
        AbrirChildEnDashboard(New frmNovedadCrear())
    End Sub

    Private Sub btnEditarNovedad_Click(sender As Object, e As EventArgs) Handles btnEditarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "Seleccioná una novedad para editar.")
            Return
        End If
        Dim novedadId = CInt(dgvNovedades.CurrentRow.Cells("Id").Value)
        AbrirChildEnDashboard(New frmNovedadCrear(novedadId))
    End Sub

    Private Async Sub btnEliminarNovedad_Click(sender As Object, e As EventArgs) Handles btnEliminarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "Seleccioná una novedad para eliminar.")
            Return
        End If
        Dim nov = TryCast(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        If nov Is Nothing Then Return
        If MessageBox.Show($"¿Eliminar la novedad del {nov.Fecha:d}?",
                          "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
            Return
        End If
        btnEliminarNovedad.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Try
            Using svc As New NovedadService()
                Await svc.DeleteNovedadCompletaAsync(nov.Id)
            End Using
            Notifier.Success(Me, "Novedad eliminada.")
            NotificadorEventos.NotificarCambioEnNovedad()
            Dim tk = ReiniciarToken()
            Await BuscarAsync(tk)
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al eliminar la novedad: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
            btnEliminarNovedad.Enabled = True
        End Try
    End Sub

    Private Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Dim funcId = frm.FuncionarioSeleccionado.Id
                If funcId > 0 AndAlso Not _funcionariosSeleccionadosFiltro.ContainsKey(funcId) Then
                    _funcionariosSeleccionadosFiltro.Add(funcId, frm.FuncionarioSeleccionado.Nombre)
                    ActualizarListaFuncionarios()
                End If
            End If
        End Using
    End Sub

    Private Sub btnLimpiarFuncionarios_Click(sender As Object, e As EventArgs) Handles btnLimpiarFuncionarios.Click
        _funcionariosSeleccionadosFiltro.Clear()
        ActualizarListaFuncionarios()
    End Sub

    Private Sub btnQuitarFuncionario_Click(sender As Object, e As EventArgs) Handles btnQuitarFuncionario.Click
        If lstFuncionariosFiltro.SelectedItem IsNot Nothing Then
            Dim selectedFuncId = CType(lstFuncionariosFiltro.SelectedValue, Integer)
            _funcionariosSeleccionadosFiltro.Remove(selectedFuncId)
            ActualizarListaFuncionarios()
        End If
    End Sub

    Private Async Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If _bsNovedades.DataSource Is Nothing OrElse _bsNovedades.Count = 0 Then
            Notifier.Warn(Me, "Primero buscá novedades para poder imprimir.")
            Return
        End If
        Dim novedadIds = DirectCast(_bsNovedades.DataSource, List(Of vw_NovedadesAgrupadas)).Select(Function(n) n.Id).ToList()
        Me.Cursor = Cursors.WaitCursor
        Try
            Using service As New NovedadService()
                Dim datosParaReporte = Await service.GetNovedadesParaReporteAsync(novedadIds)
                If datosParaReporte.Any() Then
                    AbrirChildEnDashboard(New frmNovedadesRPT(datosParaReporte))
                Else
                    Notifier.Warn(Me, "No se encontraron detalles para imprimir.")
                End If
            End Using
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al preparar la impresión: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#End Region

#Region "Helpers y Atajos"

    Private Function GetDashboard() As frmDashboard
        Return Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
    End Function

    Private Sub AbrirChildEnDashboard(formHijo As Form)
        If formHijo Is Nothing Then Return
        Dim dash = GetDashboard()
        If dash Is Nothing OrElse dash.IsDisposed Then Return
        Try
            dash.AbrirChild(formHijo)
        Catch ex As Exception
            Notifier.Error(dash, $"No se pudo abrir la ventana: {ex.Message}")
        End Try
    End Sub

    Private Async Sub frmNovedades_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.Handled Then Return
        Select Case e.KeyCode
            Case Keys.F5
                e.Handled = True
                Dim tk = ReiniciarToken()
                Await BuscarAsync(tk)
            Case Keys.Delete
                e.Handled = True
                If btnEliminarNovedad.Enabled Then btnEliminarNovedad.PerformClick()
        End Select
    End Sub

#End Region

#Region "Configuración de Grilla"
    ' Reemplaza este método en la región "Configuración de Grilla"
    Private Sub ConfigurarGrilla()
        With dgvNovedades
            ' --- CONFIGURACIÓN GENERAL ---
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .GridColor = Color.FromArgb(230, 230, 230)
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AutoGenerateColumns = False
            .BackgroundColor = Color.White

            ' --- ESTILO DE ENCABEZADOS (Headers) ---
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

            ' --- ESTILO DE FILAS (Rows) ---
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247) ' Efecto Cebra

            ' --- DEFINICIÓN DE COLUMNAS ---
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})

            Dim colFecha As New DataGridViewTextBoxColumn With {
            .Name = "Fecha", .DataPropertyName = "Fecha", .HeaderText = "Fecha",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ' Se ajusta al contenido
            .MinimumWidth = 110
        }
            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colFecha)

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Resumen", .DataPropertyName = "Resumen", .HeaderText = "Resumen de Novedad",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .FillWeight = 40 ' Ocupa un 40% del espacio Fill disponible
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Funcionarios", .DataPropertyName = "Funcionarios", .HeaderText = "Funcionarios Involucrados",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .FillWeight = 60 ' Ocupa un 60% del espacio Fill disponible
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ' Se ajusta al contenido
            .MinimumWidth = 100
        })
        End With
    End Sub

#End Region

End Class
