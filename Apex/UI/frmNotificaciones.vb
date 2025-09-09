Option Strict On
Option Explicit On

Imports System.ComponentModel

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
        ' Configuración del Timer
        tmrFiltro.Interval = 500 ' Espera medio segundo antes de buscar
        tmrFiltro.Enabled = False
    End Sub

    Private Async Sub frmGestionNotificaciones_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ' Al mostrar el formulario, cargamos los datos iniciales
        Await IniciarBusquedaAsync()
        Me.ActiveControl = txtFiltro ' Foco en el filtro principal
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

#Region "Lógica de Búsqueda"
    Private Sub dgvNotificaciones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNotificaciones.SelectionChanged
        If dgvNotificaciones.SelectedRows.Count > 0 Then
            _entidadActual = CType(dgvNotificaciones.SelectedRows(0).DataBoundItem, vw_NotificacionesCompletas)
            rtbNotificacion.Text = _entidadActual.Texto
        End If
    End Sub
    Private Async Function IniciarBusquedaAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Await BuscarAsync()
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Async Function BuscarAsync() As Task
        Try
            Dim filtroGeneral = txtFiltro.Text.Trim()
            ' CORRECCIÓN: Aseguramos que tome el texto del control correcto.
            Dim filtroFuncionario = txtFiltro.Text.Trim()

            Dim resultados = Await _svc.GetAllConDetallesAsync(filtro:=filtroGeneral, funcionarioFiltro:=filtroFuncionario)
            dgvNotificaciones.DataSource = New BindingList(Of vw_NotificacionesCompletas)(resultados)

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al buscar las notificaciones: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    ' CORRECCIÓN: El evento ahora maneja los cambios de ambos TextBox.
    Private Sub Filtro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        tmrFiltro.Stop()
        tmrFiltro.Start()
    End Sub

    ' Cuando el timer finaliza, ejecuta la búsqueda.
    Private Async Sub tmrFiltro_Tick(sender As Object, e As EventArgs) Handles tmrFiltro.Tick
        tmrFiltro.Stop()
        Await IniciarBusquedaAsync()
    End Sub

    Private Async Sub dgvNotificaciones_DoubleClick(sender As Object, e As EventArgs) Handles dgvNotificaciones.DoubleClick
        Await CambiarEstado()
    End Sub

    Private Async Function CambiarEstado() As Task
        If dgvNotificaciones.SelectedRows.Count > 0 Then
            Dim selectedNotification = CType(dgvNotificaciones.SelectedRows(0).DataBoundItem, vw_NotificacionesCompletas)
            Using frm As New frmNotificacionCambiarEstado(selectedNotification.EstadoId)
                If frm.ShowDialog() = DialogResult.OK Then
                    Dim success As Boolean = Await _svc.UpdateEstadoAsync(selectedNotification.Id, frm.SelectedEstadoId)
                    If success Then
                        MessageBox.Show("Estado de la notificación actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Await IniciarBusquedaAsync()
                    Else
                        MessageBox.Show("Hubo un error al actualizar el estado de la notificación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            End Using
        End If
    End Function

#End Region

#Region "Acciones (CRUD)"

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        ' Buscamos el formulario principal (Dashboard) que contiene a este.
        Dim dashboard = Me.ParentForm

        ' Verificamos que el formulario padre sea el Dashboard.
        If dashboard IsNot Nothing AndAlso TypeOf dashboard Is frmDashboard Then
            ' Creamos el formulario para la nueva notificación.
            Dim formCrear As New frmNotificacionCrear()

            ' Usamos el método público del Dashboard para abrir el formulario en el panel.
            CType(dashboard, frmDashboard).AbrirFormEnPanel(formCrear)
        Else
            ' Comportamiento anterior por si no se encuentra el Dashboard.
            Dim formCrear As New frmNotificacionCrear()
            formCrear.Show()
        End If
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para editar.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells("Id").Value)
        Using frm As New frmNotificacionCrear(idSeleccionado)
            If frm.ShowDialog() = DialogResult.OK Then
                Await IniciarBusquedaAsync()
            End If
        End Using
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para imprimir.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells("Id").Value)
        Dim frm As New frmNotificacionRPT(idSeleccionado)
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para eliminar.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim filaSeleccionada = dgvNotificaciones.SelectedRows(0)
        Dim idSeleccionado = CInt(filaSeleccionada.Cells("Id").Value)
        Dim nombreFuncionario = filaSeleccionada.Cells("NombreFuncionario").Value.ToString()

        If MessageBox.Show($"¿Está seguro de que desea eliminar la notificación para '{nombreFuncionario}'?",
                           "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _svc.DeleteNotificacionAsync(idSeleccionado)
                Await IniciarBusquedaAsync()
            Catch ex As Exception
                MessageBox.Show($"Ocurrió un error al eliminar: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
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

#End Region

End Class