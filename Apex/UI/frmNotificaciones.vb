Imports System.Data.Entity

Public Class frmNotificaciones

    Private _svc As NotificacionPersonalService
    Private _listaNotificaciones As List(Of NotificacionPersonalService.NotificacionParaVista)

    Private Async Sub frmNotificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New NotificacionPersonalService()
        ConfigurarGrilla()
        AddHandler dgvNotificaciones.SelectionChanged, AddressOf DgvNotificaciones_SelectionChanged
        Await CargarNotificacionesAsync()
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvNotificaciones
            .AutoGenerateColumns = False
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Id", .DataPropertyName = "Id", .Visible = False
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario",
                .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "TipoNotificacion", .DataPropertyName = "TipoNotificacion",
                .HeaderText = "Tipo", .Width = 120
            })

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
        Dim notificacionSeleccionada = _listaNotificaciones.FirstOrDefault(Function(n) n.Id = idSeleccionado)

        If notificacionSeleccionada IsNot Nothing Then
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

    Private Async Sub btnMarcarFirmada_Click(sender As Object, e As EventArgs) Handles btnMarcarFirmada.Click
        If dgvNotificaciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvNotificaciones.CurrentRow.Cells("Id").Value)

        Const ESTADO_FIRMADA As Byte = 3 ' Asumiendo que el ID de "Firmada" es 3

        Try
            Await _svc.UpdateEstadoAsync(idSeleccionado, ESTADO_FIRMADA)
            Await CargarNotificacionesAsync()
        Catch ex As Exception
            MessageBox.Show("Error al actualizar el estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class