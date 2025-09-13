' Apex/UI/frmGestionCategoriasAusencia.vb
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmIncidenciasCategorias

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
        ElseIf e.KeyCode = Keys.Enter AndAlso btnGuardar.Focused = False Then
            btnGuardar.PerformClick()
            e.Handled = True
        End If
    End Sub


End Class
