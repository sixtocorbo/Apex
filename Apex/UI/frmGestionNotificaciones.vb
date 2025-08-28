' Apex/UI/frmGestionNotificaciones.vb
Imports System.ComponentModel

Public Class frmGestionNotificaciones
    Private _svc As New NotificacionService()
    Private _notificaciones As List(Of vw_NotificacionesCompletas)
    Private _notificacionesFiltradas As BindingList(Of vw_NotificacionesCompletas)

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Async Sub frmGestionNotificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Await CargarDatos()
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvNotificaciones
            .SuspendLayout()
            .AutoGenerateColumns = False
            .Columns.Clear()
            .ReadOnly = True
            .AllowUserToAddRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .RowHeadersVisible = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .HeaderText = "ID", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "NombreCompleto", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "CI", .HeaderText = "Cédula", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaProgramada", .HeaderText = "Fecha Programada", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "TipoNotificacion", .HeaderText = "Tipo", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "EstadoActual", .HeaderText = "Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})

            .ResumeLayout()
        End With
    End Sub

    Private Async Function CargarDatos() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            _notificaciones = Await _svc.GetAllConDetallesAsync()
            _notificacionesFiltradas = New BindingList(Of vw_NotificacionesCompletas)(_notificaciones)
            dgvNotificaciones.DataSource = _notificacionesFiltradas

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al cargar las notificaciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Sub FiltrarNotificacionesEnVivo()
        Dim textoFiltro = txtFiltro.Text.ToLower().Trim()

        If String.IsNullOrEmpty(textoFiltro) Then
            _notificacionesFiltradas = New BindingList(Of vw_NotificacionesCompletas)(_notificaciones)
        Else
            ' CORRECCIÓN: Usar NombreCompleto y EstadoActual
            Dim resultado = _notificaciones.Where(Function(n)
                                                      Return (n.NombreFuncionario IsNot Nothing AndAlso n.NombreFuncionario.ToLower().Contains(textoFiltro)) OrElse
                                                           (n.CI IsNot Nothing AndAlso n.CI.ToLower().Contains(textoFiltro)) OrElse
                                                           (n.TipoNotificacion IsNot Nothing AndAlso n.TipoNotificacion.ToLower().Contains(textoFiltro)) OrElse
                                                           (n.Estado IsNot Nothing AndAlso n.Estado.ToLower().Contains(textoFiltro))
                                                  End Function).ToList()
            _notificacionesFiltradas = New BindingList(Of vw_NotificacionesCompletas)(resultado)
        End If

        dgvNotificaciones.DataSource = _notificacionesFiltradas
        dgvNotificaciones.Refresh()
    End Sub

    Private Async Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Using frm As New frmNotificacionCrear()
            If frm.ShowDialog() = DialogResult.OK Then
                Await CargarDatos()
            End If
        End Using
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells(0).Value)
        Using frm As New frmNotificacionCrear()
            frm.NotificacionId = idSeleccionado
            If frm.ShowDialog() = DialogResult.OK Then
                Await CargarDatos()
            End If
        End Using
    End Sub

    Private Sub txtFiltro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        ' Este evento se dispara cada vez que el texto cambia
        FiltrarNotificacionesEnVivo()
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells(0).Value)
        Using frm As New frmNotificacionRPT(idSeleccionado)
            frm.ShowDialog()
        End Using
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells(0).Value)
        Dim notificacion = _notificaciones.FirstOrDefault(Function(n) n.Id = idSeleccionado)

        If MessageBox.Show($"¿Está seguro de que desea eliminar la notificación para '{notificacion.NombreFuncionario}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _svc.DeleteAsync(idSeleccionado)
                Await CargarDatos()
            Catch ex As Exception
                MessageBox.Show($"Ocurrió un error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
End Class