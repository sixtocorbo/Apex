Imports System.ComponentModel
Imports System.Linq
Imports System.Threading

Public Class frmSecciones

    Private _listaSecciones As BindingList(Of Seccion)
    Private _seccionSeleccionada As Seccion
    Private _estaCargando As Boolean = False

    Private Async Sub frmGestionSecciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre de sección...")
            AppTheme.SetCue(txtNombre, "Ingrese el nombre de la sección...")
        Catch
        End Try
        Await CargarDatosAsync()
        LimpiarCampos()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Me.Cursor = Cursors.WaitCursor
        dgvSecciones.Enabled = False

        Try
            Dim lista As List(Of Seccion)
            Using svc As New SeccionService()
                lista = Await svc.GetAllAsync()
            End Using
            _listaSecciones = New BindingList(Of Seccion)(lista)
            dgvSecciones.DataSource = _listaSecciones
        Catch ex As Exception
            Notifier.Error(Me, "No se pudieron cargar las secciones.")
        Finally
            Me.Cursor = Cursors.Default
            dgvSecciones.Enabled = True
            _estaCargando = False
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
            Dim resultado = _listaSecciones.Where(Function(s) s.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).ToList()
            dgvSecciones.DataSource = New BindingList(Of Seccion)(resultado)
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre de la sección no puede estar vacío.")
            Return
        End If

        _seccionSeleccionada.Nombre = txtNombre.Text.Trim()
        Me.Cursor = Cursors.WaitCursor
        Try
            Using svc As New SeccionService()
                If _seccionSeleccionada.Id = 0 Then
                    _seccionSeleccionada.CreatedAt = DateTime.Now
                    Await svc.CreateAsync(_seccionSeleccionada)
                Else
                    Await svc.UpdateAsync(_seccionSeleccionada)
                End If
            End Using
            Notifier.Success(Me, "Sección guardada correctamente.")
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al guardar la sección: " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _seccionSeleccionada Is Nothing OrElse _seccionSeleccionada.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar una sección para eliminar.")
            Return
        End If

        Dim confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar la sección '{_seccionSeleccionada.Nombre}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmacion = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Using svc As New SeccionService()
                    Await svc.DeleteAsync(_seccionSeleccionada.Id)
                End Using
                Notifier.Info(Me, "Sección eliminada.")
                Await CargarDatosAsync()
                LimpiarCampos()
            Catch ex As Exception
                Notifier.Error(Me, "Ocurrió un error al eliminar la sección: " & ex.Message)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

End Class