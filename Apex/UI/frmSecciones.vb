' Apex/UI/frmGestionSecciones.vb
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmSecciones

    Private _seccionService As New SeccionService()
    Private _listaSecciones As BindingList(Of Seccion)
    Private _seccionSeleccionada As Seccion

    Private Async Sub frmGestionSecciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre de sección...")
            AppTheme.SetCue(txtNombre, "Ingrese el nombre de la sección...")

        Catch
            ' Ignorar si no existe SetCue
        End Try
        Await CargarDatosAsync()
        LimpiarCampos()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim lista = Await _seccionService.GetAllAsync()
            _listaSecciones = New BindingList(Of Seccion)(lista.ToList())
            dgvSecciones.DataSource = _listaSecciones
            ConfigurarGrilla()
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        dgvSecciones.AutoGenerateColumns = False
        dgvSecciones.Columns.Clear()
        dgvSecciones.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .HeaderText = "Id", .Visible = False})
        dgvSecciones.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Nombre", .HeaderText = "Nombre", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
    End Sub

    Private Sub LimpiarCampos()
        _seccionSeleccionada = New Seccion()
        txtNombre.Clear()
        btnEliminar.Enabled = False
        dgvSecciones.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _seccionSeleccionada IsNot Nothing Then
            txtNombre.Text = _seccionSeleccionada.Nombre
            btnEliminar.Enabled = True
        Else
            LimpiarCampos()
        End If
    End Sub

    Private Sub dgvSecciones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSecciones.SelectionChanged
        If dgvSecciones.CurrentRow IsNot Nothing AndAlso dgvSecciones.CurrentRow.DataBoundItem IsNot Nothing Then
            _seccionSeleccionada = CType(dgvSecciones.CurrentRow.DataBoundItem, Seccion)
            MostrarDetalles()
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvSecciones.DataSource = _listaSecciones
        Else
            Dim resultado = _listaSecciones.Where(Function(s) s.Nombre.ToUpper().Contains(filtro.ToUpper())).ToList()
            dgvSecciones.DataSource = New BindingList(Of Seccion)(resultado)
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("El nombre de la sección no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _seccionSeleccionada.Nombre = txtNombre.Text.Trim()

        Me.Cursor = Cursors.WaitCursor
        Try
            If _seccionSeleccionada.Id = 0 Then
                ' Para un nuevo registro, solo asignamos CreatedAt si existe en el modelo.
                ' Si tu modelo 'Seccion' tampoco tiene 'CreatedAt', puedes eliminar la siguiente línea.
                _seccionSeleccionada.CreatedAt = DateTime.Now
                Await _seccionService.CreateAsync(_seccionSeleccionada)
            Else
                ' --- CORRECCIÓN ---
                ' La siguiente línea ha sido eliminada porque 'UpdatedAt' no existe en 'Seccion'.
                ' _seccionSeleccionada.UpdatedAt = DateTime.Now 
                Await _seccionService.UpdateAsync(_seccionSeleccionada)
            End If

            MessageBox.Show("Sección guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la sección: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _seccionSeleccionada Is Nothing OrElse _seccionSeleccionada.Id = 0 Then
            MessageBox.Show("Debe seleccionar una sección para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar la sección '{_seccionSeleccionada.Nombre}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Await _seccionService.DeleteAsync(_seccionSeleccionada.Id)
                MessageBox.Show("Sección eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Await CargarDatosAsync()
                LimpiarCampos()
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al eliminar la sección: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        ' CAMBIO CLAVE:
        ' Ya no crea una nueva instancia. Llama al helper para que
        ' busque y muestre el frmConfiguracion que ya está abierto.
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmConfiguracion)(Me)
    End Sub
End Class