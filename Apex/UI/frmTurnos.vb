Imports System.ComponentModel
Imports System.Linq
Imports System.Threading

Public Class frmTurnos

    Private _listaTurnos As BindingList(Of Turno)
    Private _turnoSeleccionado As Turno
    Private _estaCargando As Boolean = False

    Private Async Sub frmGestionTurnos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre de turno...")
            AppTheme.SetCue(txtNombre, "Ingrese el nombre del turno...")
        Catch
        End Try
        Await CargarDatosAsync()
        LimpiarCampos()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Me.Cursor = Cursors.WaitCursor
        dgvTurnos.Enabled = False

        Try
            Dim lista As List(Of Turno)
            Using svc As New TurnoService()
                lista = Await svc.GetAllAsync()
            End Using
            _listaTurnos = New BindingList(Of Turno)(lista)
            dgvTurnos.DataSource = _listaTurnos
        Catch ex As Exception
            Notifier.Error(Me, "No se pudieron cargar los turnos.")
        Finally
            Me.Cursor = Cursors.Default
            dgvTurnos.Enabled = True
            _estaCargando = False
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
        btnEliminar.Enabled = False
        dgvTurnos.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _turnoSeleccionado IsNot Nothing Then
            txtNombre.Text = _turnoSeleccionado.Nombre
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

    ' Filtrado en memoria (no necesita cambios)
    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvTurnos.DataSource = _listaTurnos
        Else
            ' Usamos StringComparison.OrdinalIgnoreCase para un mejor rendimiento y corrección
            Dim resultado = _listaTurnos.Where(Function(t) t.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).ToList()
            dgvTurnos.DataSource = New BindingList(Of Turno)(resultado)
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre del turno no puede estar vacío.")
            Return
        End If

        _turnoSeleccionado.Nombre = txtNombre.Text.Trim()
        Me.Cursor = Cursors.WaitCursor
        Try
            Using svc As New TurnoService()
                If _turnoSeleccionado.Id = 0 Then
                    _turnoSeleccionado.CreatedAt = DateTime.Now
                    Await svc.CreateAsync(_turnoSeleccionado)
                Else
                    Await svc.UpdateAsync(_turnoSeleccionado)
                End If
            End Using
            Notifier.Success(Me, "Turno guardado correctamente.")
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al guardar el turno: " & ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _turnoSeleccionado Is Nothing OrElse _turnoSeleccionado.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar un turno para eliminar.")
            Return
        End If

        Dim confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar el turno '{_turnoSeleccionado.Nombre}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmacion = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Using svc As New TurnoService()
                    Await svc.DeleteAsync(_turnoSeleccionado.Id)
                End Using
                Notifier.Info(Me, "Turno eliminado.")
                Await CargarDatosAsync()
                LimpiarCampos()
            Catch ex As Exception
                Notifier.Error(Me, "Ocurrió un error al eliminar el turno: " & ex.Message)
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

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click

        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmConfiguracion)(Me)
    End Sub
End Class