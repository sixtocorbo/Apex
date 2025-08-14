' Apex/UI/frmNotificaciones.vb
Imports System.Data.Entity

Public Class frmNotificaciones

    Private _svc As NotificacionService
    ' --- CAMBIO: Usamos la entidad de la vista en lugar del DTO ---
    Private _listaNotificaciones As List(Of vw_NotificacionesCompletas)

    Private Async Sub frmNotificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        _svc = New NotificacionService()
        ConfigurarGrilla()
        AddHandler dgvNotificaciones.SelectionChanged, AddressOf DgvNotificaciones_SelectionChanged
        Await CargarNotificacionesAsync()
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvNotificaciones
            .AutoGenerateColumns = False
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            ' --- CAMBIO: Se ajusta el DataPropertyName para que coincida con la vista ---
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

    Private Sub DgvNotificaciones_SelectionChanged(sender As Object, e As EventArgs)
        If dgvNotificaciones.CurrentRow Is Nothing Then Return

        Dim idSeleccionado = CInt(dgvNotificaciones.CurrentRow.Cells("Id").Value)
        ' --- CAMBIO: Buscamos en la nueva lista _listaNotificaciones ---
        Dim notificacionSeleccionada = _listaNotificaciones.FirstOrDefault(Function(n) n.Id = idSeleccionado)

        If notificacionSeleccionada IsNot Nothing Then
            ' --- CAMBIO: La propiedad ahora se llama 'Texto' en la vista ---
            txtTextoNotificacion.Text = notificacionSeleccionada.Texto
        Else
            txtTextoNotificacion.Clear()
        End If
    End Sub

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

        ' Abrimos el nuevo formulario de diálogo
        Using frm As New frmCambiarEstadoNotificacion()
            If frm.ShowDialog() = DialogResult.OK Then
                Dim nuevoEstadoId = frm.SelectedEstadoId

                ' --- Lógica de Validación ---
                Const ESTADO_VENCIDA As Byte = 2 ' Asumiendo que el ID de "Vencida" es 2

                ' Regla 1: No se puede marcar como "Vencida" si la fecha programada es futura.
                If nuevoEstadoId = ESTADO_VENCIDA AndAlso notificacionSeleccionada.FechaProgramada.Date > Date.Today Then
                    MessageBox.Show("No se puede marcar una notificación como 'Vencida' antes de su fecha programada.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Aquí puedes añadir más reglas de validación si lo necesitas...


                ' Si todas las validaciones pasan, actualizamos el estado.
                Try
                    Await _svc.UpdateEstadoAsync(idSeleccionado, nuevoEstadoId)
                    Await CargarNotificacionesAsync() ' Recargar la grilla para ver el cambio
                Catch ex As Exception
                    MessageBox.Show("Error al actualizar el estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub
End Class