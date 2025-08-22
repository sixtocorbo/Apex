' Apex/UI/frmNotificaciones.vb
Imports System.Data.Entity
Imports System.Text ' Necesario para StringBuilder

Public Class frmNotificaciones

    Private _svc As NotificacionService
    Private _listaNotificaciones As List(Of vw_NotificacionesCompletas)

    Private Async Sub frmNotificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        _svc = New NotificacionService()
        ConfigurarGrilla()
        ' Ya que el método tiene "Handles...", no es necesario el AddHandler aquí.
        ' AddHandler dgvNotificaciones.SelectionChanged, AddressOf DgvNotificaciones_SelectionChanged
        Await CargarNotificacionesAsync()
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvNotificaciones
            .AutoGenerateColumns = False
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoNotificacion", .DataPropertyName = "TipoNotificacion", .HeaderText = "Tipo", .Width = 120})

            Dim fechaColumn As New DataGridViewTextBoxColumn With {
                .Name = "FechaProgramada", .DataPropertyName = "FechaProgramada",
                .HeaderText = "Fecha Programada", .Width = 150
            }
            fechaColumn.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            .Columns.Add(fechaColumn)

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Estado", .DataPropertyName = "Estado",
                .HeaderText = "Estado", .Width = 100
            })
        End With
    End Sub

    Private Async Function CargarNotificacionesAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim filtroNombre = txtBusquedaFuncionario.Text.Trim()
            _listaNotificaciones = Await _svc.GetAllConDetallesAsync(filtroNombre)

            ' --- MEJORA ---
            ' Limpiar el DataSource antes de volver a asignarlo.
            ' Esto previene problemas de refresco en la grilla.
            dgvNotificaciones.DataSource = Nothing
            dgvNotificaciones.DataSource = _listaNotificaciones

            If _listaNotificaciones Is Nothing OrElse _listaNotificaciones.Count = 0 Then
                txtTextoNotificacion.Clear()
            End If

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al cargar las notificaciones: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    ' --- INICIO DE LA CORRECCIÓN PRINCIPAL ---
    Private Sub DgvNotificaciones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNotificaciones.SelectionChanged
        ' Esta es la forma más directa y segura de obtener el objeto de la fila seleccionada.
        If dgvNotificaciones.CurrentRow Is Nothing OrElse dgvNotificaciones.CurrentRow.DataBoundItem Is Nothing Then
            txtTextoNotificacion.Clear()
            Return
        End If

        ' 1. Obtenemos el objeto completo de la fila.
        Dim notificacion = TryCast(dgvNotificaciones.CurrentRow.DataBoundItem, vw_NotificacionesCompletas)

        If notificacion IsNot Nothing Then
            ' 2. Para depurar y mejorar la UI, mostramos más detalles, no solo el texto.
            '    Esto también te ayudará a ver si el campo "Texto" está realmente vacío en la base de datos.
            Dim sb As New StringBuilder()
            sb.AppendLine($"Funcionario: {notificacion.NombreFuncionario}")
            sb.AppendLine($"Tipo: {notificacion.TipoNotificacion} | Estado: {notificacion.Estado}")
            sb.AppendLine("--------------------------------------------------")
            sb.AppendLine(If(String.IsNullOrWhiteSpace(notificacion.Texto), "(Esta notificación no tiene un texto detallado)", notificacion.Texto))

            txtTextoNotificacion.Text = sb.ToString()
        Else
            txtTextoNotificacion.Clear()
        End If
    End Sub
    ' --- FIN DE LA CORRECCIÓN PRINCIPAL ---

    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Await CargarNotificacionesAsync()
    End Sub

    Private Async Sub btnNueva_Click(sender As Object, e As EventArgs) Handles btnNueva.Click
        Using frm As New frmNotificacionCrear()
            If frm.ShowDialog() = DialogResult.OK Then
                Await CargarNotificacionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvNotificaciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvNotificaciones.CurrentRow.Cells("Id").Value)

        Using frm As New frmNotificacionCrear(idSeleccionado)
            If frm.ShowDialog() = DialogResult.OK Then
                Await CargarNotificacionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvNotificaciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvNotificaciones.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvNotificaciones.CurrentRow.Cells("NombreFuncionario").Value.ToString()

        If MessageBox.Show($"¿Está seguro de que desea eliminar la notificación para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _svc.DeleteAsync(idSeleccionado)
                Await CargarNotificacionesAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Async Sub btnCambiarEstado_Click_1(sender As Object, e As EventArgs) Handles btnCambiarEstado.Click
        If dgvNotificaciones.CurrentRow Is Nothing Then Return

        Dim idSeleccionado = CInt(dgvNotificaciones.CurrentRow.Cells("Id").Value)
        Dim notificacionSeleccionada = _listaNotificaciones.FirstOrDefault(Function(n) n.Id = idSeleccionado)

        If notificacionSeleccionada Is Nothing Then Return

        Using frm As New frmCambiarEstadoNotificacion()
            If frm.ShowDialog() = DialogResult.OK Then
                Dim nuevoEstadoId = frm.SelectedEstadoId
                Const ESTADO_VENCIDA As Byte = 2

                If nuevoEstadoId = ESTADO_VENCIDA AndAlso notificacionSeleccionada.FechaProgramada.Date > Date.Today Then
                    MessageBox.Show("No se puede marcar una notificación como 'Vencida' antes de su fecha programada.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Try
                    Await _svc.UpdateEstadoAsync(idSeleccionado, nuevoEstadoId)
                    Await CargarNotificacionesAsync()
                Catch ex As Exception
                    MessageBox.Show("Error al actualizar el estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub
End Class