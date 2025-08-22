' Apex/UI/frmGestion.vb
Imports System.Data.Entity

Public Class frmGestion

    ' Servicios para cada pestaña
    Private _licenciaSvc As New LicenciaService()
    Private _notificacionSvc As New NotificacionService()
    Private _sancionSvc As New SancionService()

    ' Se ejecuta cuando el formulario se carga
    Private Sub frmGestion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillas()
        ' Esta línea es la que conecta el clic en la grilla con la función de arriba.
        AddHandler dgvNotificaciones.SelectionChanged, AddressOf dgvNotificaciones_SelectionChanged
    End Sub

    ' Se ejecuta cuando el usuario cambia de pestaña
    Private Sub TabControlGestion_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControlGestion.SelectedIndexChanged
        If TabControlGestion.SelectedTab Is TabPageLicencias Then
            txtBusquedaLicencia.Focus()
        ElseIf TabControlGestion.SelectedTab Is TabPageNotificaciones Then
            txtBusquedaNotificacion.Focus()
        ElseIf TabControlGestion.SelectedTab Is TabPageSanciones Then
            txtBusquedaSancion.Focus()
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

        ' Configuración para la nueva grilla de Sanciones
        With dgvSanciones
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            Dim colDesde As New DataGridViewTextBoxColumn With {.Name = "FechaDesde", .DataPropertyName = "FechaDesde", .HeaderText = "Desde", .Width = 100}
            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colDesde)

            Dim colHasta As New DataGridViewTextBoxColumn With {.Name = "FechaHasta", .DataPropertyName = "FechaHasta", .HeaderText = "Hasta", .Width = 100}
            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colHasta)
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Resolucion", .DataPropertyName = "Resolucion", .HeaderText = "Resolución", .Width = 120})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Observaciones", .DataPropertyName = "Observaciones", .HeaderText = "Observaciones", .Width = 200})
        End With
    End Sub

    Private Async Function CargarDatosLicenciasAsync() As Task
        Dim filtro = txtBusquedaLicencia.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvLicencias.DataSource = Nothing
            Return
        End If
        LoadingHelper.MostrarCargando(Me)
        Try
            dgvLicencias.DataSource = Nothing
            dgvLicencias.DataSource = Await _licenciaSvc.GetAllConDetallesAsync(filtroNombre:=filtro)
        Catch ex As Exception
            MessageBox.Show("Error al cargar licencias: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

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

    Private Async Function CargarDatosSancionesAsync() As Task
        Dim filtro = txtBusquedaSancion.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvSanciones.DataSource = Nothing
            Return
        End If
        LoadingHelper.MostrarCargando(Me)
        Try
            dgvSanciones.DataSource = Nothing
            dgvSanciones.DataSource = Await _sancionSvc.GetAllConDetallesAsync(filtro)
        Catch ex As Exception
            MessageBox.Show("Error al cargar sanciones: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Manejo de Búsqueda"

    Private Async Sub txtBusquedaLicencia_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaLicencia.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await CargarDatosLicenciasAsync()
        End If
    End Sub

    Private Async Sub txtBusquedaNotificacion_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaNotificacion.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await CargarDatosNotificacionesAsync()
        End If
    End Sub

    Private Async Sub txtBusquedaSancion_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaSancion.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await CargarDatosSancionesAsync()
        End If
    End Sub

#End Region

#Region "Acciones para Licencias"

    Private Sub btnConceptoFuncional_Click(sender As Object, e As EventArgs) Handles btnConceptoFuncional.Click
        Using frm As New frmConceptoFuncionalApex()
            frm.ShowDialog(Me)
        End Using
    End Sub

    Private Async Sub btnNuevaLicencia_Click(sender As Object, e As EventArgs) Handles btnNuevaLicencia.Click
        Using frm As New frmLicenciaCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosLicenciasAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditarLicencia_Click(sender As Object, e As EventArgs) Handles btnEditarLicencia.Click
        If dgvLicencias.CurrentRow Is Nothing Then Return
        Dim licenciaSeleccionada = CType(dgvLicencias.CurrentRow.DataBoundItem, vw_LicenciasCompletas)
        If licenciaSeleccionada Is Nothing Then Return
        Dim idSeleccionado = licenciaSeleccionada.Id
        Dim estadoActual = licenciaSeleccionada.Estado
        Using frm As New frmLicenciaCrear(idSeleccionado, estadoActual)
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
    ' Este método se ejecuta automáticamente cada vez que haces clic en una fila nueva de la grilla.
    Private Sub dgvNotificaciones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNotificaciones.SelectionChanged
        ' Primero, se asegura de que haya una fila seleccionada. Si no la hay, limpia el cuadro de texto y termina.
        If dgvNotificaciones.CurrentRow Is Nothing OrElse dgvNotificaciones.CurrentRow.DataBoundItem Is Nothing Then
            txtTextoNotificacion.Clear()
            Return
        End If

        ' 1. Obtiene el objeto completo de la notificación que está enlazado a la fila seleccionada.
        Dim notificacion = TryCast(dgvNotificaciones.CurrentRow.DataBoundItem, vw_NotificacionesCompletas)

        If notificacion IsNot Nothing Then
            ' 2. Se utiliza un StringBuilder para construir el texto que se mostrará de forma más eficiente.
            Dim sb As New System.Text.StringBuilder()

            ' 3. Se añaden los detalles más importantes de la notificación para dar más contexto.
            sb.AppendLine($"Funcionario: {notificacion.NombreFuncionario}")
            sb.AppendLine($"Tipo: {notificacion.TipoNotificacion} | Estado: {notificacion.Estado}")
            sb.AppendLine("--------------------------------------------------")

            ' 4. Se añade el texto principal de la notificación. Si está vacío, se muestra un mensaje amigable.
            sb.AppendLine(If(String.IsNullOrWhiteSpace(notificacion.Texto), "(Esta notificación no tiene un texto detallado)", notificacion.Texto))

            ' 5. Finalmente, se asigna todo el texto construido al TextBox.
            txtTextoNotificacion.Text = sb.ToString()
        Else
            ' Si por alguna razón no se puede obtener el objeto, se limpia el cuadro de texto.
            txtTextoNotificacion.Clear()
        End If
    End Sub

    ' --- FIN DE LA FUNCIÓN ---
    Private Async Sub btnCambiarEstado_Click(sender As Object, e As EventArgs) Handles btnCambiarEstado.Click
        If dgvNotificaciones.CurrentRow Is Nothing Then Return

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Obtener el objeto completo de la notificación seleccionada
        Dim notificacionSeleccionada = TryCast(dgvNotificaciones.CurrentRow.DataBoundItem, vw_NotificacionesCompletas)
        If notificacionSeleccionada Is Nothing Then Return

        Dim idSeleccionado = notificacionSeleccionada.Id
        Dim idEstadoActual = notificacionSeleccionada.EstadoId

        ' Usamos el nuevo constructor para pasar el estado actual
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
        ' --- FIN DE LA CORRECCIÓN ---
    End Sub

#End Region

#Region "Acciones para Sanciones"

    Private Async Sub btnNuevaSancion_Click(sender As Object, e As EventArgs) Handles btnNuevaSancion.Click
        Using frm As New frmSancionCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosSancionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditarSancion_Click(sender As Object, e As EventArgs) Handles btnEditarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then Return
        Dim sancionSeleccionada = CType(dgvSanciones.CurrentRow.DataBoundItem, vw_SancionesCompletas)
        If sancionSeleccionada Is Nothing Then Return

        Dim idSeleccionado = sancionSeleccionada.Id
        Using frm As New frmSancionCrear()
            frm.SancionId = idSeleccionado
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosSancionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarSancion_Click(sender As Object, e As EventArgs) Handles btnEliminarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvSanciones.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvSanciones.CurrentRow.Cells("NombreFuncionario").Value.ToString()
        If MessageBox.Show($"¿Está seguro de que desea eliminar la sanción para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _sancionSvc.DeleteAsync(idSeleccionado)
                Await CargarDatosSancionesAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la sanción: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region
End Class