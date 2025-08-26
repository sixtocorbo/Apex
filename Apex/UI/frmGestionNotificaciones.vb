' Apex/UI/frmGestionNotificaciones.vb
Public Class frmGestionNotificaciones

    Private _notificacionSvc As New NotificacionService()

    Private Sub frmGestionNotificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaNotificaciones()
        ' Conectar el evento de cambio de selección
        AddHandler dgvNotificaciones.SelectionChanged, AddressOf dgvNotificaciones_SelectionChanged
        txtBusquedaNotificacion.Focus()
    End Sub

#Region "Configuración y Carga de Datos"

    Private Sub ConfigurarGrillaNotificaciones()
        With dgvNotificaciones
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoNotificacion", .DataPropertyName = "TipoNotificacion", .HeaderText = "Tipo", .Width = 150})

            Dim fechaColumn As New DataGridViewTextBoxColumn With {
                .Name = "FechaProgramada", .DataPropertyName = "FechaProgramada",
                .HeaderText = "Fecha Programada", .Width = 160
            }
            fechaColumn.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            .Columns.Add(fechaColumn)

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "EstadoId", .DataPropertyName = "EstadoId", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
        End With
    End Sub

    Private Async Function CargarDatosNotificacionesAsync() As Task
        Dim filtro = txtBusquedaNotificacion.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvNotificaciones.DataSource = Nothing
            Return
        End If

        LoadingHelper.MostrarCargando(Me)
        Try
            dgvNotificaciones.DataSource = Nothing
            dgvNotificaciones.DataSource = Await _notificacionSvc.GetAllConDetallesAsync(filtro)
        Catch ex As Exception
            MessageBox.Show("Error al cargar notificaciones: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Manejo de Búsqueda"

    Private Async Sub txtBusquedaNotificacion_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaNotificacion.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await CargarDatosNotificacionesAsync()
        End If
    End Sub

#End Region

#Region "Acciones para Notificaciones"

    Private Async Sub btnNuevaNotificacion_Click(sender As Object, e As EventArgs) Handles btnNuevaNotificacion.Click
        Using frm As New frmNotificacionCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosNotificacionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditarNotificacion_Click(sender As Object, e As EventArgs) Handles btnEditarNotificacion.Click
        If dgvNotificaciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvNotificaciones.CurrentRow.Cells("Id").Value)
        Using frm As New frmNotificacionCrear(idSeleccionado)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosNotificacionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarNotificacion_Click(sender As Object, e As EventArgs) Handles btnEliminarNotificacion.Click
        If dgvNotificaciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvNotificaciones.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvNotificaciones.CurrentRow.Cells("NombreFuncionario").Value.ToString()
        If MessageBox.Show($"¿Está seguro de que desea eliminar la notificación para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _notificacionSvc.DeleteAsync(idSeleccionado)
                Await CargarDatosNotificacionesAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la notificación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub dgvNotificaciones_SelectionChanged(sender As Object, e As EventArgs)
        If dgvNotificaciones.CurrentRow Is Nothing OrElse dgvNotificaciones.CurrentRow.DataBoundItem Is Nothing Then
            txtTextoNotificacion.Clear()
            Dim haySeleccion As Boolean = (dgvNotificaciones.SelectedRows.Count > 0)
            btnEditarNotificacion.Enabled = haySeleccion
            btnEliminarNotificacion.Enabled = haySeleccion
            btnImprimir.Enabled = haySeleccion
            btnCambiarEstado.Enabled = haySeleccion
            Return
        End If

        Dim notificacion = TryCast(dgvNotificaciones.CurrentRow.DataBoundItem, vw_NotificacionesCompletas)

        If notificacion IsNot Nothing Then
            Dim sb As New System.Text.StringBuilder()
            sb.AppendLine($"Funcionario: {notificacion.NombreFuncionario}")
            sb.AppendLine($"Tipo: {notificacion.TipoNotificacion} | Estado: {notificacion.Estado}")
            sb.AppendLine("--------------------------------------------------")
            sb.AppendLine(If(String.IsNullOrWhiteSpace(notificacion.Texto), "(Esta notificación no tiene un texto detallado)", notificacion.Texto))
            txtTextoNotificacion.Text = sb.ToString()
        Else
            txtTextoNotificacion.Clear()
        End If
    End Sub

    Private Async Sub btnCambiarEstado_Click(sender As Object, e As EventArgs) Handles btnCambiarEstado.Click
        If dgvNotificaciones.CurrentRow Is Nothing Then Return
        Dim notificacionSeleccionada = TryCast(dgvNotificaciones.CurrentRow.DataBoundItem, vw_NotificacionesCompletas)
        If notificacionSeleccionada Is Nothing Then Return

        Dim idSeleccionado = notificacionSeleccionada.Id
        Dim idEstadoActual = notificacionSeleccionada.EstadoId

        Using frm As New frmCambiarEstadoNotificacion(idEstadoActual)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Dim nuevoEstadoId = frm.SelectedEstadoId
                Try
                    Await _notificacionSvc.UpdateEstadoAsync(idSeleccionado, nuevoEstadoId)
                    Await CargarDatosNotificacionesAsync()
                Catch ex As Exception
                    MessageBox.Show("Error al actualizar el estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If dgvNotificaciones.CurrentRow IsNot Nothing Then
            Dim notificacionSeleccionada = TryCast(dgvNotificaciones.CurrentRow.DataBoundItem, vw_NotificacionesCompletas)
            If notificacionSeleccionada IsNot Nothing Then
                Using frmReporte As New frmNotificacionRPT(notificacionSeleccionada.Id)
                    frmReporte.ShowDialog(Me)
                End Using
            Else
                MessageBox.Show("No se pudo obtener la información de la notificación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("Por favor, seleccione una notificación para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

#End Region

End Class