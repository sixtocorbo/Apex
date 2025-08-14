' Apex/UI/frmGestion.vb
Imports System.Data.Entity

Public Class frmGestion

    ' Servicios para cada pestaña
    Private _licenciaSvc As New LicenciaService()
    Private _notificacionSvc As New NotificacionService()

    ' Se ejecuta cuando el formulario se carga
    Private Async Sub frmGestion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillas()
        ' Cargar los datos de la primera pestaña visible
        Await CargarDatosLicenciasAsync()
    End Sub

    ' Se ejecuta cuando el usuario cambia de pestaña
    Private Async Sub TabControlGestion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControlGestion.SelectedIndexChanged
        If TabControlGestion.SelectedTab Is TabPageLicencias Then
            Await CargarDatosLicenciasAsync()
        ElseIf TabControlGestion.SelectedTab Is TabPageNotificaciones Then
            Await CargarDatosNotificacionesAsync()
        End If
    End Sub

#Region "Configuración y Carga de Datos"

    Private Sub ConfigurarGrillas()
        ' Configuración para la grilla de Licencias
        With dgvLicencias
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoLicencia", .DataPropertyName = "TipoLicencia", .HeaderText = "Tipo", .Width = 180})

            Dim colInicio As New DataGridViewTextBoxColumn With {.Name = "FechaInicio", .DataPropertyName = "FechaInicio", .HeaderText = "Desde", .Width = 100}
            colInicio.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colInicio)

            Dim colFin As New DataGridViewTextBoxColumn With {.Name = "FechaFin", .DataPropertyName = "FechaFin", .HeaderText = "Hasta", .Width = 100}
            colFin.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colFin)

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
        End With

        ' Configuración para la grilla de Notificaciones
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

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
        End With
    End Sub

    Private Async Function CargarDatosLicenciasAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            dgvLicencias.DataSource = Nothing
            Dim filtro = txtBusquedaLicencia.Text.Trim()
            dgvLicencias.DataSource = Await _licenciaSvc.GetAllConDetallesAsync(filtroNombre:=filtro)
        Catch ex As Exception
            MessageBox.Show("Error al cargar licencias: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Async Function CargarDatosNotificacionesAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            dgvNotificaciones.DataSource = Nothing
            Dim filtro = txtBusquedaNotificacion.Text.Trim()
            dgvNotificaciones.DataSource = Await _notificacionSvc.GetAllConDetallesAsync(filtro)
        Catch ex As Exception
            MessageBox.Show("Error al cargar notificaciones: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Manejo de Búsqueda (Estilo frmFuncionarioBuscar)"

    Private Async Sub txtBusquedaLicencia_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaLicencia.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True ' Evita el sonido 'ding' al presionar Enter
            Await CargarDatosLicenciasAsync()
        End If
    End Sub

    Private Async Sub txtBusquedaNotificacion_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaNotificacion.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await CargarDatosNotificacionesAsync()
        End If
    End Sub

#End Region

#Region "Acciones para Licencias"

    Private Async Sub btnNuevaLicencia_Click(sender As Object, e As EventArgs) Handles btnNuevaLicencia.Click
        Using frm As New frmLicenciaCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosLicenciasAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditarLicencia_Click(sender As Object, e As EventArgs) Handles btnEditarLicencia.Click
        If dgvLicencias.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvLicencias.CurrentRow.Cells("Id").Value)
        Using frm As New frmLicenciaCrear(idSeleccionado)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosLicenciasAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarLicencia_Click(sender As Object, e As EventArgs) Handles btnEliminarLicencia.Click
        If dgvLicencias.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvLicencias.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvLicencias.CurrentRow.Cells("NombreFuncionario").Value.ToString()

        If MessageBox.Show($"¿Está seguro de que desea eliminar la licencia para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _licenciaSvc.DeleteAsync(idSeleccionado)
                Await CargarDatosLicenciasAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la licencia: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
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

    Private Async Sub btnCambiarEstado_Click(sender As Object, e As EventArgs) Handles btnCambiarEstado.Click
        If dgvNotificaciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvNotificaciones.CurrentRow.Cells("Id").Value)

        Using frm As New frmCambiarEstadoNotificacion()
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

#End Region

End Class