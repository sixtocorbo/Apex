Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmNotificaciones
    Private ReadOnly _svc As New NotificacionService()
    ' Timer para la búsqueda demorada, gestionado directamente en este formulario.
    Private WithEvents tmrFiltro As New Timer()
    Private _entidadActual As vw_NotificacionesCompletas

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmGestionNotificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()

        tmrFiltro.Interval = 500
        tmrFiltro.Enabled = False

        Try
            AppTheme.SetCue(txtFiltro, "Filtrar por funcionario...")
            AppTheme.SetCue(rtbNotificacion, "Aquí se muestra el texto completo de la notificación seleccionada...")
        Catch
        End Try

        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
        Notifier.Info(Me, "Escribí para filtrar; doble clic abre cambio de estado.")
    End Sub

    Private Async Sub frmGestionNotificaciones_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Try
            Await IniciarBusquedaAsync()
            Me.ActiveControl = txtFiltro
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudieron cargar las notificaciones: {ex.Message}")
        End Try
    End Sub


    Private Sub frmGestionNotificaciones_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando NotificadorEventos.DatosActualizados se dispara.
    ''' </summary>
    Private Async Sub OnDatosActualizados(sender As Object, e As EventArgs)
        Await BuscarAsync()
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

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Id",
                .DataPropertyName = "Id",
                .HeaderText = "ID",
                .Visible = False
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "NombreFuncionario",
                .DataPropertyName = "NombreFuncionario",
                .HeaderText = "Funcionario",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "CI",
                .DataPropertyName = "CI",
                .HeaderText = "Cédula",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "FechaProgramada",
                .DataPropertyName = "FechaProgramada",
                .HeaderText = "Fecha Programada",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "yyyy-MM-dd HH:mm"}
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "TipoNotificacion",
                .DataPropertyName = "TipoNotificacion",
                .HeaderText = "Tipo",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Estado",
                .DataPropertyName = "Estado",
                .HeaderText = "Estado",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            })

            .ResumeLayout()
        End With
    End Sub

#Region "Helpers de navegación (pila del Dashboard)"
    Private Function GetDashboard() As frmDashboard
        Return Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
    End Function

    Private Sub AbrirChildEnDashboard(formHijo As Form)
        If formHijo Is Nothing Then
            Notifier.Warn(Me, "No hay formulario para abrir.")
            Return
        End If

        Dim dash = GetDashboard()
        If dash Is Nothing OrElse dash.IsDisposed Then
            Notifier.Warn(Me, "No se encontró el Dashboard activo.")
            Return
        End If

        Try
            dash.AbrirChild(formHijo)
            Notifier.Success(dash, $"Abierto: {formHijo.Text}")
        Catch ex As Exception
            Notifier.[Error](dash, $"No se pudo abrir la ventana: {ex.Message}")
        End Try
    End Sub

#End Region

#Region "Lógica de Búsqueda"
    Private Sub dgvNotificaciones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNotificaciones.SelectionChanged
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            _entidadActual = Nothing
            rtbNotificacion.Text = "—"
            Return
        End If
        Dim item = TryCast(dgvNotificaciones.SelectedRows(0).DataBoundItem, vw_NotificacionesCompletas)
        _entidadActual = item
        rtbNotificacion.Text = If(item?.Texto, "—")
    End Sub


    Private Async Function IniciarBusquedaAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Await BuscarAsync()
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo completar la búsqueda: {ex.Message}")
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Async Function BuscarAsync() As Task
        Try
            Dim filtroGeneral = txtFiltro.Text.Trim()
            Dim filtroFuncionario = txtFiltro.Text.Trim()
            Dim resultados = Await _svc.GetAllConDetallesAsync(filtro:=filtroGeneral, funcionarioFiltro:=filtroFuncionario)

            dgvNotificaciones.DataSource = New BindingList(Of vw_NotificacionesCompletas)(resultados)
            dgvNotificaciones.ClearSelection()
            rtbNotificacion.Text = "—"
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al buscar las notificaciones: {ex.Message}")
            dgvNotificaciones.DataSource = New BindingList(Of vw_NotificacionesCompletas)()
            rtbNotificacion.Text = "—"
        End Try
    End Function


    Private Sub Filtro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        tmrFiltro.Stop()
        tmrFiltro.Start()
    End Sub

    Private Async Sub tmrFiltro_Tick(sender As Object, e As EventArgs) Handles tmrFiltro.Tick
        tmrFiltro.Stop()
        Await IniciarBusquedaAsync()
        Notifier.Info(Me, "Búsqueda actualizada.")
    End Sub

    Private Async Sub dgvNotificaciones_DoubleClick(sender As Object, e As EventArgs) Handles dgvNotificaciones.DoubleClick
        Await CambiarEstado()
    End Sub

    Private Async Function CambiarEstado() As Task
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            Notifier.Warn(Me, "Seleccioná una notificación para cambiar su estado.")
            Return
        End If

        Dim selectedNotification = TryCast(dgvNotificaciones.SelectedRows(0).DataBoundItem, vw_NotificacionesCompletas)
        If selectedNotification Is Nothing Then
            Notifier.Warn(Me, "No se pudo leer la notificación seleccionada.")
            Return
        End If

        Using frm As New frmNotificacionCambiarEstado(selectedNotification.EstadoId)
            If frm.ShowDialog() = DialogResult.OK Then
                Dim btnOld = btnCambiarEstado.Enabled
                btnCambiarEstado.Enabled = False
                Try
                    Dim success As Boolean = Await _svc.UpdateEstadoAsync(selectedNotification.Id, frm.SelectedEstadoId)
                    If success Then
                        Notifier.Success(Me, "Estado actualizado.")
                        Await IniciarBusquedaAsync()
                    Else
                        Notifier.[Error](Me, "No se pudo actualizar el estado.")
                    End If
                Catch ex As Exception
                    Notifier.[Error](Me, $"Error al actualizar el estado: {ex.Message}")
                Finally
                    btnCambiarEstado.Enabled = btnOld
                End Try
            End If
        End Using
    End Function

#End Region

#Region "Acciones (CRUD)"
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        AbrirChildEnDashboard(New frmNotificacionCrear())
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            Notifier.Warn(Me, "Seleccione una notificación para imprimir.")
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells("Id").Value)
        AbrirChildEnDashboard(New frmNotificacionRPT(idSeleccionado))
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            Notifier.Warn(Me, "Seleccione una notificación para editar.")
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells("Id").Value)
        AbrirChildEnDashboard(New frmNotificacionCrear(idSeleccionado))
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            Notifier.Warn(Me, "Seleccione una notificación para eliminar.")
            Return
        End If

        Dim fila = dgvNotificaciones.SelectedRows(0)
        Dim idSeleccionado = CInt(fila.Cells("Id").Value)
        Dim nombreFuncionario = fila.Cells("NombreFuncionario").Value.ToString()

        If MessageBox.Show($"¿Eliminar la notificación de '{nombreFuncionario}'?",
                       "Confirmar eliminación",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Dim oldCursor = Me.Cursor
        btnEliminar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Await _svc.DeleteNotificacionAsync(idSeleccionado)
            Notifier.Info(Me, "Notificación eliminada.")
            Await IniciarBusquedaAsync()
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al eliminar: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnEliminar.Enabled = True
        End Try
    End Sub
#End Region

#Region "Mejoras de UX"
    Private Async Sub frmNotificaciones_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.F5
                Await IniciarBusquedaAsync()
            Case Keys.Delete
                btnEliminar.PerformClick()
        End Select
    End Sub

    Private Async Sub btnCambiarEstado_Click(sender As Object, e As EventArgs) Handles btnCambiarEstado.Click
        Await CambiarEstado()
    End Sub

    Private Sub btnNuevaMasiva_Click(sender As Object, e As EventArgs) Handles btnNuevaMasiva.Click
        AbrirChildEnDashboard(New frmNotificacionMasiva())
    End Sub

#End Region

End Class
