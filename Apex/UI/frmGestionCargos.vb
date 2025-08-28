' Apex/UI/frmGestionCargos.vb
Imports System.Data.Entity
Imports System.Windows.Forms
Imports System.ComponentModel

Public Class frmGestionCargos

    ' Ya no se necesitan el delegado ni el evento
    ' Public Delegate Sub CargosModificadosEventHandler()
    ' Public Event CargosModificados As CargosModificadosEventHandler

    Private _cargoService As New CargoService()
    Private _listaCargos As BindingList(Of Cargo)
    Private _cargoSeleccionado As Cargo

    Private Async Sub frmGestionCargos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Await CargarDatosAsync()
        LimpiarCampos()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim lista = Await _cargoService.GetAllAsync()
            _listaCargos = New BindingList(Of Cargo)(lista.ToList())
            dgvCargos.DataSource = _listaCargos
            ConfigurarGrilla()
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        dgvCargos.AutoGenerateColumns = False
        dgvCargos.Columns.Clear()
        dgvCargos.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .HeaderText = "Id", .Visible = False})
        dgvCargos.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Nombre", .HeaderText = "Nombre", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
        dgvCargos.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Grado", .HeaderText = "Grado", .Width = 80})
    End Sub

    Private Sub LimpiarCampos()
        _cargoSeleccionado = New Cargo()
        txtNombre.Clear()
        txtGrado.Clear()
        btnEliminar.Enabled = False
        dgvCargos.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _cargoSeleccionado IsNot Nothing Then
            txtNombre.Text = _cargoSeleccionado.Nombre
            ' **Líneas corregidas**
            If _cargoSeleccionado.Grado.HasValue Then
                txtGrado.Text = _cargoSeleccionado.Grado.Value.ToString()
            Else
                txtGrado.Text = ""
            End If
            ' **Fin de líneas corregidas**
            btnEliminar.Enabled = True
        Else
            LimpiarCampos()
        End If
    End Sub

    Private Sub dgvCargos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCargos.SelectionChanged
        If dgvCargos.CurrentRow IsNot Nothing AndAlso dgvCargos.CurrentRow.DataBoundItem IsNot Nothing Then
            _cargoSeleccionado = CType(dgvCargos.CurrentRow.DataBoundItem, Cargo)
            MostrarDetalles()
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvCargos.DataSource = _listaCargos
        Else
            Dim resultado = _listaCargos.Where(Function(c) c.Nombre.ToUpper().Contains(filtro.ToUpper())).ToList()
            dgvCargos.DataSource = New BindingList(Of Cargo)(resultado)
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("El nombre del cargo no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _cargoSeleccionado.Nombre = txtNombre.Text.Trim()

        ' Lógica corregida para el Grado
        Dim gradoTemp As Integer
        If Integer.TryParse(txtGrado.Text.Trim(), gradoTemp) Then
            _cargoSeleccionado.Grado = gradoTemp
        Else
            _cargoSeleccionado.Grado = Nothing
        End If

        Me.Cursor = Cursors.WaitCursor
        Try
            If _cargoSeleccionado.Id = 0 Then
                ' Es un nuevo registro
                _cargoSeleccionado.CreatedAt = DateTime.Now
                Await _cargoService.CreateAsync(_cargoSeleccionado)
            Else
                ' Es una actualización
                _cargoSeleccionado.UpdatedAt = DateTime.Now
                Await _cargoService.UpdateAsync(_cargoSeleccionado)
            End If

            MessageBox.Show("Cargo guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Await CargarDatosAsync()
            LimpiarCampos()

            ' Se eliminó el RaiseEvent CargosModificados()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar el cargo: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _cargoSeleccionado Is Nothing OrElse _cargoSeleccionado.Id = 0 Then
            MessageBox.Show("Debe seleccionar un cargo para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar el cargo '{_cargoSeleccionado.Nombre}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Await _cargoService.DeleteAsync(_cargoSeleccionado.Id)
                MessageBox.Show("Cargo eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Await CargarDatosAsync()
                LimpiarCampos()

                ' Se eliminó el RaiseEvent CargosModificados()
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al eliminar el cargo: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub
End Class