' Apex/UI/frmGestionCategoriasAusencia.vb
Imports System.ComponentModel

Public Class frmIncidenciasCategorias
    Public Sub New()
        InitializeComponent()
        ConfigurarLayoutResponsivoIncidencias()
    End Sub
    ' —o al final de InitializeComponent():
    ' ConfigurarLayoutResponsivoIncidencias()

    Private _categoriaService As New CategoriaAusenciaService()
    Private _listaCategorias As BindingList(Of CategoriaAusencia)
    Private _categoriaSeleccionada As CategoriaAusencia

    Private Async Sub frmGestionCategoriasAusencia_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        ConfigurarGrilla()
        Await CargarDatosAsync()
        LimpiarCampos()
        Notifier.Info(Me, "Listado de categorías listo.")
    End Sub
    ' Llamar después de InitializeComponent()
    Private Sub ConfigurarLayoutResponsivoIncidencias()
        ' ========= 1) SplitContainer: mínimos y docking =========
        With SplitContainer1
            .Dock = DockStyle.Fill
            .Panel1MinSize = 320    ' lista + búsqueda
            .Panel2MinSize = 360    ' edición + botones
            If .SplitterDistance < .Panel1MinSize Then
                .SplitterDistance = .Panel1MinSize
            End If
        End With

        ' ========= 2) Panel izquierdo (búsqueda + grilla) =========
        With Label8
            .Dock = DockStyle.Top
            .AutoSize = True
            .Padding = New Padding(0, 0, 0, 2)
        End With

        With txtBuscar
            .Dock = DockStyle.Top
            .Margin = New Padding(0, 2, 0, 6)
        End With

        dgvCategorias.Dock = DockStyle.Fill

        ' ========= 3) Panel derecho: crear un contenedor vertical =========
        ' Estructura: fila0=GroupBox (Auto) | fila1=Botones (Auto) | fila2=Filler (100%)
        Dim tlpDerecha As New TableLayoutPanel() With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 1,
        .RowCount = 3
    }
        tlpDerecha.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
        tlpDerecha.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tlpDerecha.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        tlpDerecha.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))

        ' Preparar GroupBox (se estira a lo ancho, alto automático)
        With GroupBox1
            .Dock = DockStyle.Top
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
            .Margin = New Padding(12, 12, 12, 6)
            ' Sus controles internos ya tienen Anchor; no es necesario cambiarlos.
        End With

        ' ========= 4) Botones: usar FlowLayoutPanel con Wrap =========
        Dim flpBotones As New FlowLayoutPanel() With {
        .Name = "flpBotones",
        .Dock = DockStyle.Top,
        .AutoSize = True,
        .AutoSizeMode = AutoSizeMode.GrowAndShrink,
        .FlowDirection = FlowDirection.RightToLeft, ' alineados a la derecha
        .WrapContents = True,
        .Margin = New Padding(12, 6, 12, 6),
        .Padding = New Padding(0)
    }

        ' Configurar botones y moverlos al FlowLayout
        Dim botones() As Button = {btnGuardar, btnEliminar, btnNuevo} ' derecha -> izquierda
        For Each b In botones
            b.AutoSize = True
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink
            b.MinimumSize = New Size(110, 36)
            b.Margin = New Padding(6)
            b.Dock = DockStyle.None
            flpBotones.Controls.Add(b)
        Next

        ' Limpiar Panel2 y montar nueva estructura
        SplitContainer1.Panel2.SuspendLayout()
        Try
            ' Quitar los controles existentes (evita duplicados visuales)
            For i As Integer = SplitContainer1.Panel2.Controls.Count - 1 To 0 Step -1
                Dim ctl = SplitContainer1.Panel2.Controls(i)
                If Not (ctl Is tlpDerecha) Then SplitContainer1.Panel2.Controls.Remove(ctl)
            Next

            ' Agregar GroupBox y botonera a la nueva tabla
            tlpDerecha.Controls.Add(GroupBox1, 0, 0)
            tlpDerecha.Controls.Add(flpBotones, 0, 1)

            ' Agregar el TableLayout al Panel2
            SplitContainer1.Panel2.Controls.Add(tlpDerecha)
        Finally
            SplitContainer1.Panel2.ResumeLayout()
        End Try
    End Sub


    Private Async Function CargarDatosAsync() As Task
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim lista = Await _categoriaService.GetAllAsync()
            _listaCategorias = New BindingList(Of CategoriaAusencia)(lista.ToList())
            dgvCategorias.DataSource = _listaCategorias
            ConfigurarGrilla()
            dgvCategorias.ClearSelection()
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al cargar las categorías: {ex.Message}")
            dgvCategorias.DataSource = New BindingList(Of CategoriaAusencia)()
        Finally
            Me.Cursor = oldCursor
        End Try
    End Function


    Private Sub ConfigurarGrilla()
        dgvCategorias.AutoGenerateColumns = False
        dgvCategorias.Columns.Clear()
        dgvCategorias.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .HeaderText = "Id", .Visible = False})
        dgvCategorias.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Nombre", .HeaderText = "Nombre", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
    End Sub

    Private Sub LimpiarCampos()
        _categoriaSeleccionada = New CategoriaAusencia()
        txtNombre.Clear()
        btnEliminar.Enabled = False
        dgvCategorias.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _categoriaSeleccionada IsNot Nothing Then
            txtNombre.Text = _categoriaSeleccionada.Nombre
            btnEliminar.Enabled = True
        Else
            LimpiarCampos()
        End If
    End Sub

    Private Sub dgvCategorias_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCategorias.SelectionChanged
        If dgvCategorias.CurrentRow IsNot Nothing AndAlso dgvCategorias.CurrentRow.DataBoundItem IsNot Nothing Then
            _categoriaSeleccionada = CType(dgvCategorias.CurrentRow.DataBoundItem, CategoriaAusencia)
            MostrarDetalles()
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If _listaCategorias Is Nothing Then Return

        Dim filtro As String = If(txtBuscar.Text, String.Empty).Trim()
        If filtro.Length = 0 Then
            dgvCategorias.DataSource = _listaCategorias
            dgvCategorias.ClearSelection()
            Return
        End If

        Dim upperFiltro As String = filtro.ToUpperInvariant()

        Dim resultado As List(Of CategoriaAusencia) =
        _listaCategorias.Where(Function(c)
                                   Dim nombre As String = If(c Is Nothing OrElse c.Nombre Is Nothing, String.Empty, c.Nombre)
                                   Return nombre.ToUpperInvariant().Contains(upperFiltro)
                               End Function).ToList()

        dgvCategorias.DataSource = New BindingList(Of CategoriaAusencia)(resultado)
        dgvCategorias.ClearSelection()
    End Sub


    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre de la categoría no puede estar vacío.")
            Return
        End If

        _categoriaSeleccionada.Nombre = txtNombre.Text.Trim()

        Dim oldCursor = Me.Cursor
        btnGuardar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            If _categoriaSeleccionada.Id = 0 Then
                Await _categoriaService.CreateAsync(_categoriaSeleccionada)
                Notifier.Success(Me, "Categoría creada correctamente.")
            Else
                Await _categoriaService.UpdateAsync(_categoriaSeleccionada)
                Notifier.Success(Me, "Categoría actualizada correctamente.")
            End If

            Await CargarDatosAsync()
            LimpiarCampos()

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al guardar la categoría: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnGuardar.Enabled = True
        End Try
    End Sub


    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _categoriaSeleccionada Is Nothing OrElse _categoriaSeleccionada.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar una categoría para eliminar.")
            Return
        End If

        Dim confirmacion = MessageBox.Show(
        $"¿Está seguro de que desea eliminar la categoría '{_categoriaSeleccionada.Nombre}'?",
        "Confirmar eliminación",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question)

        If confirmacion <> DialogResult.Yes Then Return

        Dim oldCursor = Me.Cursor
        btnEliminar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Await _categoriaService.DeleteAsync(_categoriaSeleccionada.Id)
            Notifier.Info(Me, "Categoría eliminada.")
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al eliminar la categoría: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnEliminar.Enabled = True
        End Try
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub


End Class
