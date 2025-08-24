' Apex/UI/frmGestionAreasTrabajo.vb
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmGestionAreasTrabajo

    Private _areaService As New AreaTrabajoService()
    Private _listaAreas As BindingList(Of AreaTrabajo)
    Private _areaSeleccionada As AreaTrabajo

    Private Async Sub frmGestionAreasTrabajo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Await CargarDatosAsync()
        LimpiarCampos()
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

End Class