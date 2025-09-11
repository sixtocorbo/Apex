' Apex/UI/frmGestionTiposEstadoTransitorio.vb
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmEstadoTransitorioTipos

    Private _tipoEstadoService As New TipoEstadoTransitorioService()
    Private _listaTiposEstado As BindingList(Of TipoEstadoTransitorio)
    Private _tipoEstadoSeleccionado As TipoEstadoTransitorio

    Private Async Sub frmGestionTiposEstadoTransitorio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre de tipo de estado...")
            AppTheme.SetCue(txtNombre, "Ingrese el nombre del tipo de estado...")

        Catch
            ' Ignorar si no existe SetCue
        End Try
        ConfigurarGrilla()
        Await CargarDatosAsync()
        LimpiarCampos()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim lista = Await _tipoEstadoService.GetAllAsync()
            _listaTiposEstado = New BindingList(Of TipoEstadoTransitorio)(lista.ToList())
            dgvTiposEstado.DataSource = _listaTiposEstado
            ConfigurarGrilla()
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        dgvTiposEstado.AutoGenerateColumns = False
        dgvTiposEstado.Columns.Clear()
        dgvTiposEstado.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .HeaderText = "Id", .Visible = False})
        dgvTiposEstado.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Nombre", .HeaderText = "Nombre", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
    End Sub

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
            btnEliminar.Enabled = True
        Else
            LimpiarCampos()
        End If
    End Sub

    Private Sub dgvTiposEstado_SelectionChanged(sender As Object, e As EventArgs) Handles dgvTiposEstado.SelectionChanged
        If dgvTiposEstado.CurrentRow IsNot Nothing AndAlso dgvTiposEstado.CurrentRow.DataBoundItem IsNot Nothing Then
            _tipoEstadoSeleccionado = CType(dgvTiposEstado.CurrentRow.DataBoundItem, TipoEstadoTransitorio)
            MostrarDetalles()
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvTiposEstado.DataSource = _listaTiposEstado
        Else
            Dim resultado = _listaTiposEstado.Where(Function(t) t.Nombre.ToUpper().Contains(filtro.ToUpper())).ToList()
            dgvTiposEstado.DataSource = New BindingList(Of TipoEstadoTransitorio)(resultado)
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("El nombre del tipo de estado no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _tipoEstadoSeleccionado.Nombre = txtNombre.Text.Trim()

        Me.Cursor = Cursors.WaitCursor
        Try
            If _tipoEstadoSeleccionado.Id = 0 Then
                Await _tipoEstadoService.CreateAsync(_tipoEstadoSeleccionado)
            Else
                Await _tipoEstadoService.UpdateAsync(_tipoEstadoSeleccionado)
            End If

            MessageBox.Show("Tipo de estado guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar el tipo de estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _tipoEstadoSeleccionado Is Nothing OrElse _tipoEstadoSeleccionado.Id = 0 Then
            MessageBox.Show("Debe seleccionar un tipo de estado para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar el tipo de estado '{_tipoEstadoSeleccionado.Nombre}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Await _tipoEstadoService.DeleteAsync(_tipoEstadoSeleccionado.Id)
                MessageBox.Show("Tipo de estado eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Await CargarDatosAsync()
                LimpiarCampos()
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al eliminar el tipo de estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        ' Simplemente creamos una nueva instancia del menú de configuración
        ' y le pedimos a nuestro ayudante que la muestre.
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmConfiguracion)(Me)
    End Sub
    ' ================== MODERNO + RESPONSIVO PARA frmEstadoTransitorioTipos ==================
    ' Llamar una vez luego de InitializeComponent:
    ' Public Sub New()
    '     InitializeComponent()
    '     AplicarLayoutModernoYResponsivo()
    ' End Sub
    '
    ' Private Sub frmEstadoTransitorioTipos_Load(...) Handles MyBase.Load
    '     AplicarLayoutModernoYResponsivo()
    ' End Sub

    Private Sub AplicarLayoutModernoYResponsivo()
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.MinimumSize = New Size(800, 520)

        ' --- SplitContainer: proporción estable + mínimos ---
        With SplitContainer1
            .Dock = DockStyle.Fill
            .IsSplitterFixed = False
            .FixedPanel = FixedPanel.None
            .Panel1MinSize = 260
            .Panel2MinSize = 360
            AjustarSplitterProporcional()
            AddHandler Me.Resize, AddressOf OnFormResize_AjustarSplitter
        End With

        ' --- IZQUIERDA: encabezado de búsqueda + grilla fill ---
        ReconstruirPanelIzquierdo()

        ' --- DERECHA: barra de acciones + formulario que llena ---
        ReconstruirPanelDerecho()

        ' --- Persistencia del ancho del panel izquierdo ---

        ' --- Atajos útiles ---
        AddHandler txtBuscar.KeyDown, AddressOf TxtBuscar_KeyDown
    End Sub

    Private Sub AjustarSplitterProporcional()
        Dim ancho As Integer = Me.ClientSize.Width
        If ancho <= 0 Then Return
        Dim deseado As Integer = CInt(ancho * 0.34) ' ~34% a la izquierda
        SplitContainer1.SplitterDistance =
        Math.Max(SplitContainer1.Panel1MinSize,
                 Math.Min(deseado, ancho - SplitContainer1.Panel2MinSize))
    End Sub

    Private Sub OnFormResize_AjustarSplitter(sender As Object, e As EventArgs)
        AjustarSplitterProporcional()
    End Sub


    ' -------------------------- IZQUIERDA --------------------------
    Private Sub ReconstruirPanelIzquierdo()
        Dim host As Control = SplitContainer1.Panel1

        ' Header (Label + TextBox en una fila)
        Dim header As New Panel With {.Dock = DockStyle.Top, .Padding = New Padding(12), .Height = 56}
        Dim tlp As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2}
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))

        ' Reubicar controles existentes
        Label8.AutoSize = True
        Label8.Margin = New Padding(0, 2, 10, 0)
        Label8.Text = "Buscar"

        txtBuscar.BorderStyle = BorderStyle.FixedSingle
        txtBuscar.Margin = New Padding(0)
        txtBuscar.Dock = DockStyle.Fill
        txtBuscar.CharacterCasing = CharacterCasing.Upper
        EstablecerPlaceholder(txtBuscar, "FILTRAR POR NOMBRE...")

        tlp.Controls.Add(Label8, 0, 0)
        tlp.Controls.Add(txtBuscar, 1, 0)
        header.Controls.Add(tlp)

        ' Grilla: fill + estilo moderno
        dgvTiposEstado.Dock = DockStyle.Fill
        dgvTiposEstado.ReadOnly = True
        dgvTiposEstado.MultiSelect = False
        dgvTiposEstado.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvTiposEstado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvTiposEstado.RowHeadersVisible = False
        dgvTiposEstado.AllowUserToResizeRows = False
        dgvTiposEstado.AllowUserToAddRows = False
        dgvTiposEstado.AllowUserToDeleteRows = False
        AplicarEstiloGrilla(dgvTiposEstado)

        ' Limpiar y agregar en orden
        host.Controls.Clear()
        host.Controls.Add(dgvTiposEstado)
        host.Controls.Add(header)
    End Sub

    Private Sub AplicarEstiloGrilla(dgv As DataGridView)
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5!, FontStyle.Bold)
        dgv.ColumnHeadersHeight = 36
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None

        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9.0!, FontStyle.Regular)
        dgv.DefaultCellStyle.SelectionBackColor = Color.Gainsboro
        dgv.DefaultCellStyle.SelectionForeColor = Color.Black
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248)
        dgv.RowTemplate.Height = 28

        ' Doble buffer para scroll suave
        Try
            Dim pi = GetType(DataGridView).GetProperty("DoubleBuffered",
            Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            If pi IsNot Nothing Then pi.SetValue(dgv, True, Nothing)
        Catch
        End Try
    End Sub

    ' -------------------------- DERECHA --------------------------
    Private Sub ReconstruirPanelDerecho()
        Dim host As Control = SplitContainer1.Panel2
        host.SuspendLayout()

        ' 1) Barra de acciones
        Dim flp As New FlowLayoutPanel With {
        .Dock = DockStyle.Top,
        .AutoSize = True,
        .AutoSizeMode = AutoSizeMode.GrowAndShrink,
        .FlowDirection = FlowDirection.LeftToRight,
        .Padding = New Padding(12),
        .WrapContents = True
    }

        ' Normalizar botones existentes
        PrepararBotonModerno(btnNuevo)
        PrepararBotonModerno(btnGuardar, bold:=True)
        PrepararBotonModerno(btnEliminar)
        PrepararBotonModerno(btnVolver)

        flp.Controls.AddRange(New Control() {btnNuevo, btnGuardar, btnEliminar, btnVolver})

        ' 2) Formulario (GroupBox) que llena el espacio
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

        ' 3) Ensamble
        host.Controls.Clear()
        host.Controls.Add(GroupBox1)
        host.Controls.Add(flp)
        host.ResumeLayout()
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

    ' -------------------------- UX EXTRA --------------------------
    ' Placeholder/CueBanner nativo
    <Drawing.ToolboxBitmap(GetType(TextBox))>
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

    ' Enter en buscar -> foco a grilla
    Private Sub TxtBuscar_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            dgvTiposEstado.Focus()
            e.Handled = True
        End If
    End Sub

    ' Atajos globales (opcional)
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        Select Case keyData
            Case (Keys.Control Or Keys.N) : If btnNuevo.Enabled Then btnNuevo.PerformClick() : Return True
            Case (Keys.Control Or Keys.S) : If btnGuardar.Enabled Then btnGuardar.PerformClick() : Return True
            Case Keys.Delete : If btnEliminar.Enabled Then btnEliminar.PerformClick() : Return True
            Case Keys.Escape : If btnVolver.Enabled Then btnVolver.PerformClick() : Return True
        End Select
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function
    ' ================== /FIN BLOQUE ==================

End Class