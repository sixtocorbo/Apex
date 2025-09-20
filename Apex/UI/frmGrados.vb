Imports System.ComponentModel
Imports System.Linq
Imports System.Threading

Public Class frmGrados

    Private _listaCargos As BindingList(Of Cargo)
    Private _cargoSeleccionado As Cargo
    Private _estaCargando As Boolean = False

    Private Async Sub frmGestionCargos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        ConfigurarGrilla()
        Await CargarDatosAsync()
        LimpiarCampos()
        Notifier.Info(Me, "Listado de cargos listo.")
    End Sub

    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Me.Cursor = Cursors.WaitCursor
        dgvCargos.Enabled = False

        Try
            Dim lista As List(Of Cargo)
            Using svc As New CargoService()
                lista = Await svc.GetAllAsync()
            End Using
            _listaCargos = New BindingList(Of Cargo)(lista)
            dgvCargos.DataSource = _listaCargos
            dgvCargos.ClearSelection()
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al cargar los datos: {ex.Message}")
            dgvCargos.DataSource = New BindingList(Of Cargo)()
        Finally
            Me.Cursor = Cursors.Default
            dgvCargos.Enabled = True
            _estaCargando = False
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
            txtGrado.Text = _cargoSeleccionado.Grado?.ToString()
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
        If _listaCargos Is Nothing Then Return
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvCargos.DataSource = _listaCargos
        Else
            Dim resultado = _listaCargos.Where(Function(c) c.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).ToList()
            dgvCargos.DataSource = New BindingList(Of Cargo)(resultado)
        End If
        dgvCargos.ClearSelection()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre del cargo no puede estar vacío.")
            Return
        End If

        _cargoSeleccionado.Nombre = txtNombre.Text.Trim()
        Dim gradoTemp As Integer
        _cargoSeleccionado.Grado = If(Integer.TryParse(txtGrado.Text.Trim(), gradoTemp), gradoTemp, CType(Nothing, Integer?))

        Me.Cursor = Cursors.WaitCursor
        btnGuardar.Enabled = False
        Try
            Using svc As New CargoService()
                If _cargoSeleccionado.Id = 0 Then
                    _cargoSeleccionado.CreatedAt = DateTime.Now
                    Await svc.CreateAsync(_cargoSeleccionado)
                    Notifier.Success(Me, "Cargo creado correctamente.")
                Else
                    _cargoSeleccionado.UpdatedAt = DateTime.Now
                    Await svc.UpdateAsync(_cargoSeleccionado)
                    Notifier.Success(Me, "Cargo actualizado correctamente.")
                End If
            End Using
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al guardar el cargo: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _cargoSeleccionado Is Nothing OrElse _cargoSeleccionado.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar un cargo para eliminar.")
            Return
        End If
        Dim confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea eliminar el cargo '{_cargoSeleccionado.Nombre}'?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmacion <> DialogResult.Yes Then Return

        Me.Cursor = Cursors.WaitCursor
        btnEliminar.Enabled = False
        Try
            Using svc As New CargoService()
                Await svc.DeleteAsync(_cargoSeleccionado.Id)
            End Using
            Notifier.Info(Me, "Cargo eliminado.")
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al eliminar el cargo: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
            btnEliminar.Enabled = True
        End Try
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub

End Class