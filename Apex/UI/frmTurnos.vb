' Apex/UI/frmGestionTurnos.vb
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmTurnos

    Private _turnoService As New TurnoService()
    Private _listaTurnos As BindingList(Of Turno)
    Private _turnoSeleccionado As Turno

    Private Async Sub frmGestionTurnos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Await CargarDatosAsync()
        LimpiarCampos()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim lista = Await _turnoService.GetAllAsync()
            _listaTurnos = New BindingList(Of Turno)(lista.ToList())
            dgvTurnos.DataSource = _listaTurnos
            ConfigurarGrilla()
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        dgvTurnos.AutoGenerateColumns = False
        dgvTurnos.Columns.Clear()

        Dim colNombre = New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre",
            .HeaderText = "Nombre",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        }

        Dim colCreatedAt = New DataGridViewTextBoxColumn With {
            .DataPropertyName = "CreatedAt",
            .HeaderText = "Creado el",
            .Width = 120
        }
        colCreatedAt.DefaultCellStyle.Format = "dd/MM/yyyy"

        dgvTurnos.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .Visible = False})
        dgvTurnos.Columns.Add(colNombre)
        dgvTurnos.Columns.Add(colCreatedAt)
    End Sub

    Private Sub LimpiarCampos()
        _turnoSeleccionado = New Turno()
        txtNombre.Clear()
        ' Los campos de hora y checkbox ya no son necesarios aquí
        btnEliminar.Enabled = False
        dgvTurnos.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _turnoSeleccionado IsNot Nothing Then
            txtNombre.Text = _turnoSeleccionado.Nombre
            ' Los campos de hora y checkbox ya no se muestran
            btnEliminar.Enabled = True
        Else
            LimpiarCampos()
        End If
    End Sub

    Private Sub dgvTurnos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvTurnos.SelectionChanged
        If dgvTurnos.CurrentRow IsNot Nothing AndAlso dgvTurnos.CurrentRow.DataBoundItem IsNot Nothing Then
            _turnoSeleccionado = CType(dgvTurnos.CurrentRow.DataBoundItem, Turno)
            MostrarDetalles()
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvTurnos.DataSource = _listaTurnos
        Else
            Dim resultado = _listaTurnos.Where(Function(t) t.Nombre.ToUpper().Contains(filtro.ToUpper())).ToList()
            dgvTurnos.DataSource = New BindingList(Of Turno)(resultado)
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("El nombre del turno no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _turnoSeleccionado.Nombre = txtNombre.Text.Trim()

        Me.Cursor = Cursors.WaitCursor
        Try
            If _turnoSeleccionado.Id = 0 Then
                _turnoSeleccionado.CreatedAt = DateTime.Now
                Await _turnoService.CreateAsync(_turnoSeleccionado)
            Else
                Await _turnoService.UpdateAsync(_turnoSeleccionado)
            End If

            MessageBox.Show("Turno guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar el turno: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _turnoSeleccionado Is Nothing OrElse _turnoSeleccionado.Id = 0 Then
            MessageBox.Show("Debe seleccionar un turno para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar el turno '{_turnoSeleccionado.Nombre}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Await _turnoService.DeleteAsync(_turnoSeleccionado.Id)
                MessageBox.Show("Turno eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Await CargarDatosAsync()
                LimpiarCampos()
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al eliminar el turno: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmConfiguracion)()
    End Sub
End Class