' Apex/UI/frmGestionCargos.vb
Imports System.ComponentModel

Public Class frmGrados

    Private _cargoService As New CargoService()
    Private _listaCargos As BindingList(Of Cargo)
    Private _cargoSeleccionado As Cargo

    ' --- LOAD: tema, KeyPreview para ESC, grilla, datos, limpiar ---
    Private Async Sub frmGestionCargos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        ConfigurarGrilla()
        Await CargarDatosAsync()
        LimpiarCampos()
        Notifier.Info(Me, "Listado de cargos listo.")
    End Sub


    ' --- Carga de datos ---
    Private Async Function CargarDatosAsync() As Task
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim lista = Await _cargoService.GetAllAsync()
            _listaCargos = New BindingList(Of Cargo)(lista.ToList())
            dgvCargos.DataSource = _listaCargos
            dgvCargos.ClearSelection()
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al cargar los datos: {ex.Message}")
            dgvCargos.DataSource = New BindingList(Of Cargo)()
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function


    ' --- Configuración de grilla ---
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
            If _cargoSeleccionado.Grado.HasValue Then
                txtGrado.Text = _cargoSeleccionado.Grado.Value.ToString()
            Else
                txtGrado.Text = String.Empty
            End If
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

        Dim filtro As String = If(txtBuscar.Text, String.Empty).Trim()
        If filtro.Length = 0 Then
            dgvCargos.DataSource = _listaCargos
            dgvCargos.ClearSelection()
            Return
        End If

        Dim upperFiltro As String = filtro.ToUpperInvariant()

        ' Filtrado seguro ante Nothings (sin ?. ni ??)
        Dim resultado As List(Of Cargo) =
        _listaCargos.Where(Function(c)
                               Dim nombre As String = If(c Is Nothing OrElse c.Nombre Is Nothing, String.Empty, c.Nombre)
                               Return nombre.ToUpperInvariant().Contains(upperFiltro)
                           End Function).ToList()

        dgvCargos.DataSource = New BindingList(Of Cargo)(resultado)
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
        If Integer.TryParse(txtGrado.Text.Trim(), gradoTemp) Then
            _cargoSeleccionado.Grado = gradoTemp
        Else
            _cargoSeleccionado.Grado = Nothing
        End If

        Dim oldCursor = Me.Cursor
        btnGuardar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            If _cargoSeleccionado.Id = 0 Then
                _cargoSeleccionado.CreatedAt = DateTime.Now
                Await _cargoService.CreateAsync(_cargoSeleccionado)
                Notifier.Success(Me, "Cargo creado correctamente.")
            Else
                _cargoSeleccionado.UpdatedAt = DateTime.Now
                Await _cargoService.UpdateAsync(_cargoSeleccionado)
                Notifier.Success(Me, "Cargo actualizado correctamente.")
            End If

            Await CargarDatosAsync()
            LimpiarCampos()

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al guardar el cargo: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
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
        "Confirmar eliminación",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question)

        If confirmacion <> DialogResult.Yes Then Return

        Dim oldCursor = Me.Cursor
        btnEliminar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Await _cargoService.DeleteAsync(_cargoSeleccionado.Id)
            Notifier.Info(Me, "Cargo eliminado.")
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al eliminar el cargo: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnEliminar.Enabled = True
        End Try
    End Sub


    ' --- Atajo ESC para volver/cerrar (la pila del Dashboard hace el resto) ---
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub

End Class
