' En: /UI/frmUsuarios.vb
Public Class frmUsuarios
    Private _svc As New UsuarioService()

    Private Async Sub frmUsuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        dgvUsuarios.ActivarDobleBuffer(True)
        ConfigurarGrilla()
        Await CargarDatosAsync()
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvUsuarios
            ' --- CONFIGURACIÓN GENERAL ---
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .GridColor = Color.FromArgb(230, 230, 230)
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AutoGenerateColumns = False
            .BackgroundColor = Color.White

            ' --- ESTILO DE ENCABEZADOS (Headers) ---
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

            ' --- ESTILO DE FILAS (Rows) ---
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247) ' Efecto Cebra

            ' --- DEFINICIÓN DE COLUMNAS ---
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "NombreUsuario", .DataPropertyName = "NombreUsuario", .HeaderText = "Usuario",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .FillWeight = 40, ' 40% del espacio sobrante
            .MinimumWidth = 150
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "NombreCompleto", .DataPropertyName = "NombreCompleto", .HeaderText = "Nombre Completo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .FillWeight = 60, ' 60% del espacio sobrante
            .MinimumWidth = 200
        })

            Dim chkActivo As New DataGridViewCheckBoxColumn With {
            .Name = "Activo", .DataPropertyName = "Activo", .HeaderText = "Activo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .MinimumWidth = 80
        }
            ' Centrar el checkbox se ve mucho mejor
            chkActivo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            chkActivo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns.Add(chkActivo)

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