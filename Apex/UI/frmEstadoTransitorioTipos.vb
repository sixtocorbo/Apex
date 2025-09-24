' Apex/UI/frmEstadoTransitorioTipos.vb
Imports System.ComponentModel
Imports System.Linq

Partial Class frmEstadoTransitorioTipos

    Private _listaTiposEstado As BindingList(Of TipoEstadoTransitorio)
    Private _tipoEstadoSeleccionado As TipoEstadoTransitorio
    Private _estaCargando As Boolean

    Private Async Sub frmEstadoTransitorioTipos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        AplicarLayoutModernoYResponsivo()
        ConfigurarGrilla()

        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre de tipo de estado…")
            AppTheme.SetCue(txtNombre, "Ingrese el nombre…")
        Catch
        End Try

        Await CargarDatosAsync()
        LimpiarCampos()
        Notifier.Info(Me, "Tipos de estado listos.")
    End Sub

    ' =================== DATOS ===================
    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True
        Cursor = Cursors.WaitCursor
        dgvTiposEstado.Enabled = False
        Try
            Dim lista As List(Of TipoEstadoTransitorio)
            Using svc As New TipoEstadoTransitorioService()
                lista = Await svc.GetAllAsync()
            End Using
            _listaTiposEstado = New BindingList(Of TipoEstadoTransitorio)(lista)
            dgvTiposEstado.DataSource = _listaTiposEstado
            dgvTiposEstado.ClearSelection()
        Catch ex As Exception
            Notifier.Error(Me, "No se pudieron cargar los tipos de estado: " & ex.Message)
            dgvTiposEstado.DataSource = New BindingList(Of TipoEstadoTransitorio)()
        Finally
            Cursor = Cursors.Default
            dgvTiposEstado.Enabled = True
            _estaCargando = False
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        dgvTiposEstado.AutoGenerateColumns = False
        dgvTiposEstado.Columns.Clear()

        dgvTiposEstado.Columns.Add(New DataGridViewTextBoxColumn With {
        .DataPropertyName = "Id",
        .HeaderText = "Id",
        .Visible = False
    })
        dgvTiposEstado.Columns.Add(New DataGridViewTextBoxColumn With {
        .DataPropertyName = "Nombre",
        .HeaderText = "Nombre del Tipo de Estado", ' <-- Título más descriptivo
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    })

        ' Estilo moderno y doble buffer
        AplicarEstiloGrilla(dgvTiposEstado)
    End Sub

    ' =================== UI STATE ===================
    Private Sub LimpiarCampos()
        _tipoEstadoSeleccionado = New TipoEstadoTransitorio()
        txtNombre.Clear()
        btnEliminar.Enabled = False
        dgvTiposEstado.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _tipoEstadoSeleccionado IsNot Nothing Then
            txtNombre.Text = _tipoEstadoSeleccionado.Nombre
            btnEliminar.Enabled = (_tipoEstadoSeleccionado.Id <> 0)
        Else
            LimpiarCampos()
        End If
    End Sub

    ' =================== EVENTOS GRID / BUSCAR ===================
    Private Sub dgvTiposEstado_SelectionChanged(sender As Object, e As EventArgs) Handles dgvTiposEstado.SelectionChanged
        If dgvTiposEstado.CurrentRow Is Nothing Then Return
        Dim item = TryCast(dgvTiposEstado.CurrentRow.DataBoundItem, TipoEstadoTransitorio)
        If item Is Nothing Then Return
        _tipoEstadoSeleccionado = item
        MostrarDetalles()
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If _listaTiposEstado Is Nothing Then Return
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvTiposEstado.DataSource = _listaTiposEstado
        Else
            Dim resultado = _listaTiposEstado.
                Where(Function(t) t.Nombre IsNot Nothing AndAlso
                                  t.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                ToList()
            dgvTiposEstado.DataSource = New BindingList(Of TipoEstadoTransitorio)(resultado)
        End If
        dgvTiposEstado.ClearSelection()
    End Sub

    ' =================== BOTONES ===================
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre no puede estar vacío.")
            txtNombre.Focus()
            Return
        End If

        _tipoEstadoSeleccionado.Nombre = txtNombre.Text.Trim()

        Cursor = Cursors.WaitCursor
        btnGuardar.Enabled = False
        Try
            Using svc As New TipoEstadoTransitorioService()
                If _tipoEstadoSeleccionado.Id = 0 Then
                    _tipoEstadoSeleccionado.CreatedAt = DateTime.Now
                    Await svc.CreateAsync(_tipoEstadoSeleccionado)
                    Notifier.Success(Me, "Tipo de estado creado.")
                Else
                    _tipoEstadoSeleccionado.UpdatedAt = DateTime.Now
                    Await svc.UpdateAsync(_tipoEstadoSeleccionado)
                    Notifier.Success(Me, "Tipo de estado actualizado.")
                End If
            End Using
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al guardar: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _tipoEstadoSeleccionado Is Nothing OrElse _tipoEstadoSeleccionado.Id = 0 Then
            Notifier.Warn(Me, "Seleccione un registro para eliminar.")
            Return
        End If

        Dim confirm = MessageBox.Show(
            $"¿Eliminar el tipo de estado '{_tipoEstadoSeleccionado.Nombre}'?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirm <> DialogResult.Yes Then Return

        Cursor = Cursors.WaitCursor
        btnEliminar.Enabled = False
        Try
            Using svc As New TipoEstadoTransitorioService()
                Await svc.DeleteAsync(_tipoEstadoSeleccionado.Id)
            End Using
            Notifier.Info(Me, "Tipo de estado eliminado.")
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Dim msg As String = MapearErrorRelacion(ex)
            Notifier.Error(Me, msg)
        Finally
            Cursor = Cursors.Default
            btnEliminar.Enabled = True
        End Try
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmConfiguracion)(Me)
    End Sub

    ' =================== HELPERS ===================
    Private Function MapearErrorRelacion(ex As Exception) As String
        ' Intenta detectar violación de clave foránea de forma agnóstica
        ' - SQL Server: SqlException.Number = 547
        ' - Mensajes con “REFERENCE”, “clave foránea”, etc.
        Try
            Dim inner = ex
            While inner IsNot Nothing
                ' Chequeo por SqlException sin referenciar directamente el assembly:
                If inner.GetType().Name = "SqlException" Then
                    Dim prop = inner.GetType().GetProperty("Number")
                    If prop IsNot Nothing Then
                        Dim numberObj = prop.GetValue(inner, Nothing)
                        If TypeOf numberObj Is Integer AndAlso CInt(numberObj) = 547 Then
                            Return "No se puede eliminar porque existen registros relacionados. " &
                                   "Revise dependencias o reasigne antes de eliminar."
                        End If
                    End If
                End If

                Dim msg = inner.Message
                If msg IsNot Nothing Then
                    Dim m = msg.ToUpperInvariant()
                    If m.Contains("FOREIGN KEY") OrElse m.Contains("REFERENCE") OrElse m.Contains("CLAVE FORÁNEA") Then
                        Return "No se puede eliminar porque el registro está siendo utilizado por otros datos relacionados."
                    End If
                End If
                inner = inner.InnerException
            End While
        Catch
        End Try

        ' Genérico
        Return "Ocurrió un error al eliminar: " & ex.Message
    End Function

    Private Sub AplicarEstiloGrilla(dgv As DataGridView)
        ' --- CONFIGURACIÓN GENERAL (Estilo moderno) ---
        dgv.BorderStyle = BorderStyle.None
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(230, 230, 230)
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.AllowUserToResizeRows = False
        dgv.BackgroundColor = Color.White

        ' --- ESTILO DE ENCABEZADOS (Headers) ---
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersHeight = 40
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

        ' --- ESTILO DE FILAS (Rows) ---
        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
        dgv.DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
        dgv.DefaultCellStyle.SelectionForeColor = Color.White
        dgv.RowsDefaultCellStyle.BackColor = Color.White
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)

        ' Doble buffer (se mantiene tu lógica original)
        Try
            Dim pi = GetType(DataGridView).GetProperty("DoubleBuffered",
            Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            If pi IsNot Nothing Then pi.SetValue(dgv, True, Nothing)
        Catch
        End Try
    End Sub

    ' =================== LAYOUT MODERNO/RESPONSIVO ===================
    Private Sub AplicarLayoutModernoYResponsivo()
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.MinimumSize = New Size(800, 520)

        With SplitContainer1
            .Dock = DockStyle.Fill
            .IsSplitterFixed = False
            .FixedPanel = FixedPanel.None
            .Panel1MinSize = 280
            .Panel2MinSize = 360
            AjustarSplitterProporcional()
            AddHandler Me.Resize, AddressOf OnFormResize_AjustarSplitter
        End With

        ' Panel IZQ: header compacto con Label + TextBox
        Dim header As New Panel With {.Dock = DockStyle.Top, .Padding = New Padding(12), .Height = 56}
        Dim tlp As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2}
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))

        Label8.AutoSize = True
        Label8.Margin = New Padding(0, 2, 10, 0)
        Label8.Text = "Buscar"

        txtBuscar.BorderStyle = BorderStyle.FixedSingle
        txtBuscar.Margin = New Padding(0)
        txtBuscar.Dock = DockStyle.Fill
        EstablecerPlaceholder(txtBuscar, "FILTRAR POR NOMBRE…")

        tlp.Controls.Add(Label8, 0, 0)
        tlp.Controls.Add(txtBuscar, 1, 0)
        header.Controls.Add(tlp)

        SplitContainer1.Panel1.Controls.Clear()
        SplitContainer1.Panel1.Controls.Add(dgvTiposEstado)
        SplitContainer1.Panel1.Controls.Add(header)

        ' Panel DER: barra de acciones (FlowLayout)
        Dim flp As New FlowLayoutPanel With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .FlowDirection = FlowDirection.LeftToRight,
            .Padding = New Padding(12),
            .WrapContents = True
        }
        PrepararBotonModerno(btnNuevo)
        PrepararBotonModerno(btnGuardar, True)
        PrepararBotonModerno(btnEliminar)
        PrepararBotonModerno(btnVolver)
        flp.Controls.AddRange(New Control() {btnNuevo, btnGuardar, btnEliminar, btnVolver})

        ' GroupBox como formulario
        GroupBox1.Text = "Detalles"
        GroupBox1.Dock = DockStyle.Fill
        GroupBox1.Padding = New Padding(12)

        Dim tlpForm As New TableLayoutPanel With {.Dock = DockStyle.Top, .AutoSize = True, .ColumnCount = 2}
        tlpForm.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tlpForm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
        tlpForm.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        Label1.Margin = New Padding(0, 6, 10, 6)
        Label1.AutoSize = True
        Label1.Text = "Nombre"

        txtNombre.BorderStyle = BorderStyle.FixedSingle
        txtNombre.Dock = DockStyle.Fill
        txtNombre.Margin = New Padding(0, 3, 0, 3)

        GroupBox1.Controls.Clear()
        tlpForm.Controls.Add(Label1, 0, 0)
        tlpForm.Controls.Add(txtNombre, 1, 0)
        GroupBox1.Controls.Add(tlpForm)

        SplitContainer1.Panel2.Controls.Clear()
        SplitContainer1.Panel2.Controls.Add(GroupBox1)
        SplitContainer1.Panel2.Controls.Add(flp)
    End Sub

    Private Sub AjustarSplitterProporcional()
        Dim ancho As Integer = Me.ClientSize.Width
        If ancho <= 0 Then Return
        Dim deseado As Integer = CInt(ancho * 0.34) ' ~34% izq
        SplitContainer1.SplitterDistance =
            Math.Max(SplitContainer1.Panel1MinSize,
                     Math.Min(deseado, ancho - SplitContainer1.Panel2MinSize))
    End Sub

    Private Sub OnFormResize_AjustarSplitter(sender As Object, e As EventArgs)
        AjustarSplitterProporcional()
    End Sub

    Private Sub PrepararBotonModerno(b As Button, Optional bold As Boolean = False)
        b.AutoSize = True
        b.AutoSizeMode = AutoSizeMode.GrowAndShrink
        b.FlatStyle = FlatStyle.Flat
        b.FlatAppearance.BorderColor = Color.Silver
        b.FlatAppearance.MouseOverBackColor = Color.Gainsboro
        b.Padding = New Padding(14, 8, 14, 8)
        b.Margin = New Padding(0, 0, 12, 8)
        b.Font = New Font("Segoe UI", If(bold, 9.5!, 9.0!), If(bold, FontStyle.Bold, FontStyle.Regular))
    End Sub

    ' Placeholder nativo (cue banner)
    Private Class NativeMethods
        <Runtime.InteropServices.DllImport("user32.dll", CharSet:=Runtime.InteropServices.CharSet.Unicode)>
        Public Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As String) As IntPtr
        End Function
    End Class

    Private Sub EstablecerPlaceholder(tb As TextBox, texto As String)
        Const EM_SETCUEBANNER As Integer = &H1501
        Try
            NativeMethods.SendMessage(tb.Handle, EM_SETCUEBANNER, IntPtr.Zero, texto)
        Catch
        End Try
    End Sub

    ' Atajos (Ctrl+N / Ctrl+S / Del / Esc)
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Select Case keyData
            Case (Keys.Control Or Keys.N) : If btnNuevo.Enabled Then btnNuevo.PerformClick() : Return True
            Case (Keys.Control Or Keys.S) : If btnGuardar.Enabled Then btnGuardar.PerformClick() : Return True
            Case Keys.Delete : If btnEliminar.Enabled Then btnEliminar.PerformClick() : Return True
            Case Keys.Escape : If btnVolver.Enabled Then btnVolver.PerformClick() : Return True
        End Select
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

End Class
