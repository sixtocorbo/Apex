' Apex/UI/frmGestionAreasTrabajo.vb
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmAreaTrabajoCategorias

    Private _areaService As New AreaTrabajoService()
    Private _listaAreas As BindingList(Of AreaTrabajo)
    Private _areaSeleccionada As AreaTrabajo

    Private Async Sub frmGestionAreasTrabajo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre de área de trabajo...")
            AppTheme.SetCue(txtNombre, "Ingrese el nombre del área de trabajo...")

        Catch
            ' Ignorar si no existe SetCue
        End Try
        Await CargarDatosAsync()
        LimpiarCampos()
        AplicarLayoutModernoYResponsivo()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim lista = Await _areaService.GetAllAsync()
            _listaAreas = New BindingList(Of AreaTrabajo)(lista.ToList())
            dgvAreas.DataSource = _listaAreas
            ConfigurarGrilla()
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        dgvAreas.AutoGenerateColumns = False
        dgvAreas.Columns.Clear()
        dgvAreas.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .HeaderText = "Id", .Visible = False})
        dgvAreas.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Nombre", .HeaderText = "Nombre", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
    End Sub

    Private Sub LimpiarCampos()
        _areaSeleccionada = New AreaTrabajo()
        txtNombre.Clear()
        btnEliminar.Enabled = False
        dgvAreas.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _areaSeleccionada IsNot Nothing Then
            txtNombre.Text = _areaSeleccionada.Nombre
            btnEliminar.Enabled = True
        Else
            LimpiarCampos()
        End If
    End Sub

    Private Sub dgvAreas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAreas.SelectionChanged
        If dgvAreas.CurrentRow IsNot Nothing AndAlso dgvAreas.CurrentRow.DataBoundItem IsNot Nothing Then
            _areaSeleccionada = CType(dgvAreas.CurrentRow.DataBoundItem, AreaTrabajo)
            MostrarDetalles()
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvAreas.DataSource = _listaAreas
        Else
            Dim resultado = _listaAreas.Where(Function(a) a.Nombre.ToUpper().Contains(filtro.ToUpper())).ToList()
            dgvAreas.DataSource = New BindingList(Of AreaTrabajo)(resultado)
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("El nombre del área de trabajo no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _areaSeleccionada.Nombre = txtNombre.Text.Trim()

        Me.Cursor = Cursors.WaitCursor
        Try
            If _areaSeleccionada.Id = 0 Then
                Await _areaService.CreateAsync(_areaSeleccionada)
            Else
                Await _areaService.UpdateAsync(_areaSeleccionada)
            End If

            MessageBox.Show("Área de trabajo guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar el área de trabajo: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _areaSeleccionada Is Nothing OrElse _areaSeleccionada.Id = 0 Then
            MessageBox.Show("Debe seleccionar un área para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar el área '{_areaSeleccionada.Nombre}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Await _areaService.DeleteAsync(_areaSeleccionada.Id)
                MessageBox.Show("Área de trabajo eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Await CargarDatosAsync()
                LimpiarCampos()
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al eliminar el área: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
    '==============================================================================
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

        ' --- IZQUIERDA: encabezado de búsqueda limpio + grilla fill ---
        ReconstruirPanelIzquierdo()

        ' --- DERECHA: barra de acciones arriba + formulario que llena ---
        ReconstruirPanelDerecho()

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

        ' header (Label + TextBox en una fila)
        Dim header As New Panel With {.Dock = DockStyle.Top, .Padding = New Padding(12), .Height = 56}
        Dim tlp As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2}
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))

        ' reubicar controles existentes
        Label8.AutoSize = True
        Label8.Margin = New Padding(0, 2, 10, 0)
        Label8.Text = "Buscar"

        txtBuscar.BorderStyle = BorderStyle.FixedSingle
        txtBuscar.Margin = New Padding(0)
        txtBuscar.Dock = DockStyle.Fill
        txtBuscar.CharacterCasing = CharacterCasing.Upper

        tlp.Controls.Add(Label8, 0, 0)
        tlp.Controls.Add(txtBuscar, 1, 0)
        header.Controls.Add(tlp)

        ' grilla: fill, autosize, estilo moderno
        dgvAreas.Dock = DockStyle.Fill
        dgvAreas.ReadOnly = True
        dgvAreas.MultiSelect = False
        dgvAreas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAreas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvAreas.RowHeadersVisible = False
        dgvAreas.AllowUserToResizeRows = False
        dgvAreas.AllowUserToAddRows = False
        dgvAreas.AllowUserToDeleteRows = False
        AplicarEstiloGrilla(dgvAreas)

        ' limpiar docking previo y volver a agregar en orden
        host.Controls.Clear()
        host.Controls.Add(dgvAreas)
        host.Controls.Add(header)
    End Sub

    Private Sub AplicarEstiloGrilla(dgv As DataGridView)
        ' Cabecera
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5!, FontStyle.Bold)
        dgv.ColumnHeadersHeight = 36
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None

        ' Filas
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

        ' Normalizamos botones existentes
        PrepararBotonModerno(btnNuevo)
        PrepararBotonModerno(btnGuardar, bold:=True)
        PrepararBotonModerno(btnEliminar)
        PrepararBotonModerno(btnVolver)

        ' Los agregamos a la barra, en un orden lógico
        flp.Controls.AddRange(New Control() {btnNuevo, btnGuardar, btnEliminar, btnVolver})

        ' 2) Formulario de detalles: el GroupBox llena el espacio
        GroupBox1.Text = "Detalles"
        GroupBox1.Dock = DockStyle.Fill
        GroupBox1.Padding = New Padding(12)

        ' Dentro del group, ponemos un TLP 2 columnas (label auto + input fill)
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

        ' Quitamos del group y recolocamos ordenados
        GroupBox1.Controls.Clear()
        tlpForm.Controls.Add(Label1, 0, 0)
        tlpForm.Controls.Add(txtNombre, 1, 0)
        GroupBox1.Controls.Add(tlpForm)

        ' 3) Composer final a la derecha
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
End Class