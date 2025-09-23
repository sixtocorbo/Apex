' En: /UI/frmUsuarios.vb
Public Class frmUsuarios
    Private _svc As New UsuarioService()

    Private Async Sub frmUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Await CargarDatosAsync()
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvUsuarios
            .AutoGenerateColumns = False
            .Columns.Clear()
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreUsuario", .DataPropertyName = "NombreUsuario", .HeaderText = "Usuario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreCompleto", .DataPropertyName = "NombreCompleto", .HeaderText = "Nombre Completo", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewCheckBoxColumn With {.Name = "Activo", .DataPropertyName = "Activo", .HeaderText = "Activo"})
        End With
    End Sub

    Private Async Function CargarDatosAsync() As Task
        dgvUsuarios.DataSource = Await _svc.GetAllAsync()
    End Function

    Private Async Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Using form As New frmUsuarioCrear()
            ' Si el usuario se creó correctamente (DialogResult.OK), recargamos la lista
            If form.ShowDialog() = DialogResult.OK Then
                Await CargarDatosAsync() ' Asegúrate de tener un método para cargar/refrescar los datos del grid
            End If
        End Using
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvUsuarios.CurrentRow Is Nothing Then Return

        Dim id = CInt(dgvUsuarios.CurrentRow.Cells("Id").Value)
        Using frm As New frmUsuarioCrear(id)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvUsuarios.CurrentRow Is Nothing Then Return

        Dim id = CInt(dgvUsuarios.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvUsuarios.CurrentRow.Cells("NombreUsuario").Value.ToString()

        If MessageBox.Show($"¿Está seguro de que desea eliminar al usuario '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Await _svc.DeleteAsync(id)
                Await CargarDatosAsync()
            Catch ex As Exception
                Notifier.Error(Me, "Error al eliminar: " & ex.Message)
            End Try
        End If
    End Sub
End Class