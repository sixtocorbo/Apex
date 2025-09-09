' Apex/UI/frmGestionTiposEstadoTransitorio.vb
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmEstadoTransitorioTipos

    Private _tipoEstadoService As New TipoEstadoTransitorioService()
    Private _listaTiposEstado As BindingList(Of TipoEstadoTransitorio)
    Private _tipoEstadoSeleccionado As TipoEstadoTransitorio

    Private Async Sub frmGestionTiposEstadoTransitorio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
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
End Class