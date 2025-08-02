' Apex/UI/FiltroAvanzadoUI.vb
Imports Apex.ConsultasGenericas

' Interfaz que define las acciones que un origen de datos puede tener.
Public Interface IAccionHandler
    Sub ConfigurarVisibilidadBotones(form As frmFiltroAvanzado, hayDatos As Boolean)
    Sub ManejarBotonNuevo(form As frmFiltroAvanzado)
    Sub ManejarBotonEditar(form As frmFiltroAvanzado)
    Sub ManejarBotonEliminar(form As frmFiltroAvanzado)
    Sub ManejarBotonExtra(form As frmFiltroAvanzado) ' Para "Cambiar Estado" o futuros botones
End Interface

' Implementación específica para Licencias
Public Class LicenciaAccionHandler
    Implements IAccionHandler

    Public Sub ConfigurarVisibilidadBotones(form As frmFiltroAvanzado, hayDatos As Boolean) Implements IAccionHandler.ConfigurarVisibilidadBotones
        form.btnNuevaLicencia.Visible = hayDatos
        form.btnEditarLicencia.Visible = hayDatos
        form.btnEliminarLicencia.Visible = hayDatos
        ' Ocultamos los botones de notificaciones
        form.btnNuevaNotificacion.Visible = False
        form.btnEditarNotificacion.Visible = False
        form.btnEliminarNotificacion.Visible = False
        form.btnCambiarEstado.Visible = False
    End Sub

    Public Sub ManejarBotonNuevo(form As frmFiltroAvanzado) Implements IAccionHandler.ManejarBotonNuevo
        Using frm As New frmLicenciaCrear()
            If frm.ShowDialog() = DialogResult.OK Then
                form.btnCargar.PerformClick()
            End If
        End Using
    End Sub

    Public Async Sub ManejarBotonEditar(form As frmFiltroAvanzado) Implements IAccionHandler.ManejarBotonEditar
        If form.dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(form.dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim id = CInt(drv("Id"))
        Using frm As New frmLicenciaCrear(id)
            If frm.ShowDialog() = DialogResult.OK Then
                ' Directamente llamamos al método de carga, ya que estamos en el contexto del form
                Await form.CargarDatosAsync()
            End If
        End Using
    End Sub

    Public Async Sub ManejarBotonEliminar(form As frmFiltroAvanzado) Implements IAccionHandler.ManejarBotonEliminar
        If form.dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(form.dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim id = CInt(drv("Id"))
        Dim nombre = drv("Funcionario").ToString()
        If MessageBox.Show($"¿Está seguro de que desea eliminar la licencia para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await New LicenciaService().DeleteAsync(id)
                Await form.CargarDatosAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Public Sub ManejarBotonExtra(form As frmFiltroAvanzado) Implements IAccionHandler.ManejarBotonExtra
        ' No hay botón extra para licencias
    End Sub
End Class

' Implementación específica para Notificaciones
Public Class NotificacionAccionHandler
    Implements IAccionHandler

    Public Sub ConfigurarVisibilidadBotones(form As frmFiltroAvanzado, hayDatos As Boolean) Implements IAccionHandler.ConfigurarVisibilidadBotones
        ' Ocultamos los botones de licencias
        form.btnNuevaLicencia.Visible = False
        form.btnEditarLicencia.Visible = False
        form.btnEliminarLicencia.Visible = False
        ' Mostramos los de notificaciones
        form.btnNuevaNotificacion.Visible = hayDatos
        form.btnEditarNotificacion.Visible = hayDatos
        form.btnEliminarNotificacion.Visible = hayDatos
        form.btnCambiarEstado.Visible = hayDatos
    End Sub

    Public Sub ManejarBotonNuevo(form As frmFiltroAvanzado) Implements IAccionHandler.ManejarBotonNuevo
        Using frm As New frmNotificacionCrear()
            If frm.ShowDialog() = DialogResult.OK Then
                form.btnCargar.PerformClick()
            End If
        End Using
    End Sub

    Public Sub ManejarBotonEditar(form As frmFiltroAvanzado) Implements IAccionHandler.ManejarBotonEditar
        If form.dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(form.dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim id = CInt(drv("Id"))
        Using frm As New frmNotificacionCrear(id)
            If frm.ShowDialog() = DialogResult.OK Then
                form.btnCargar.PerformClick()
            End If
        End Using
    End Sub

    Public Async Sub ManejarBotonEliminar(form As frmFiltroAvanzado) Implements IAccionHandler.ManejarBotonEliminar
        If form.dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(form.dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim id = CInt(drv("Id"))
        Dim nombre = drv("Funcionario").ToString()
        If MessageBox.Show($"¿Está seguro de que desea eliminar la notificación para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await New NotificacionPersonalService().DeleteAsync(id)
                Await form.CargarDatosAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Public Async Sub ManejarBotonExtra(form As frmFiltroAvanzado) Implements IAccionHandler.ManejarBotonExtra
        If form.dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(form.dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim id = CInt(drv("Id"))

        Using frm As New frmCambiarEstadoNotificacion()
            If frm.ShowDialog() = DialogResult.OK Then
                Dim nuevoEstadoId = frm.SelectedEstadoId
                Try
                    Await New NotificacionPersonalService().UpdateEstadoAsync(id, nuevoEstadoId)
                    Await form.CargarDatosAsync()
                Catch ex As Exception
                    MessageBox.Show("Error al actualizar el estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub
End Class