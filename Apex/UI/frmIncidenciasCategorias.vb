' Apex/UI/frmGestionCategoriasAusencia.vb
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmIncidenciasCategorias

    Private _categoriaService As New CategoriaAusenciaService()
    Private _listaCategorias As BindingList(Of CategoriaAusencia)
    Private _categoriaSeleccionada As CategoriaAusencia

    Private Async Sub frmGestionCategoriasAusencia_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Await CargarDatosAsync()
        LimpiarCampos()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim lista = Await _categoriaService.GetAllAsync()
            _listaCategorias = New BindingList(Of CategoriaAusencia)(lista.ToList())
            dgvCategorias.DataSource = _listaCategorias
            ConfigurarGrilla()
        Finally
            Me.Cursor = Cursors.Default
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
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvCategorias.DataSource = _listaCategorias
        Else
            Dim resultado = _listaCategorias.Where(Function(c) c.Nombre.ToUpper().Contains(filtro.ToUpper())).ToList()
            dgvCategorias.DataSource = New BindingList(Of CategoriaAusencia)(resultado)
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("El nombre de la categoría no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _categoriaSeleccionada.Nombre = txtNombre.Text.Trim()

        Me.Cursor = Cursors.WaitCursor
        Try
            If _categoriaSeleccionada.Id = 0 Then
                ' --- CORRECCIÓN ---
                ' La siguiente línea ha sido eliminada porque 'CreatedAt' no existe en 'CategoriaAusencia'.
                ' _categoriaSeleccionada.CreatedAt = DateTime.Now 
                Await _categoriaService.CreateAsync(_categoriaSeleccionada)
            Else
                Await _categoriaService.UpdateAsync(_categoriaSeleccionada)
            End If

            MessageBox.Show("Categoría guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la categoría: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _categoriaSeleccionada Is Nothing OrElse _categoriaSeleccionada.Id = 0 Then
            MessageBox.Show("Debe seleccionar una categoría para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar la categoría '{_categoriaSeleccionada.Nombre}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Await _categoriaService.DeleteAsync(_categoriaSeleccionada.Id)
                MessageBox.Show("Categoría eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Await CargarDatosAsync()
                LimpiarCampos()
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al eliminar la categoría: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
End Class