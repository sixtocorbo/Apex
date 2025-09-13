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
        ' Configuración del Timer
        tmrFiltro.Interval = 500 ' Espera medio segundo antes de buscar
        tmrFiltro.Enabled = False
        ' Cues
        Try
            AppTheme.SetCue(txtFiltro, "Filtrar por funcionario...")
            AppTheme.SetCue(rtbNotificacion, "Aquí se muestra el texto completo de la notificación seleccionada...")
        Catch
        End Try
        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
    End Sub

    Private Async Sub frmGestionNotificaciones_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ' Al mostrar el formulario, cargamos los datos iniciales
        Await IniciarBusquedaAsync()
        Me.ActiveControl = txtFiltro ' Foco en el filtro principal
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
        Dim dash = GetDashboard()
        If dash Is Nothing Then
            MessageBox.Show("No se encontró el Dashboard activo.", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        dash.AbrirChild(formHijo) ' ← usa la pila (Opción A)
    End Sub
#End Region

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
            Dim filtroFuncionario = txtFiltro.Text.Trim()

            Dim resultados = Await _svc.GetAllConDetallesAsync(filtro:=filtroGeneral, funcionarioFiltro:=filtroFuncionario)
            dgvNotificaciones.DataSource = New BindingList(Of vw_NotificacionesCompletas)(resultados)
        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al buscar las notificaciones: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Sub Filtro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        tmrFiltro.Stop()
        tmrFiltro.Start()
    End Sub

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
        ' Antes: buscabas el dashboard y llamabas AbrirFormEnPanel o Show()
        ' Ahora: abrir como child para volver automáticamente a esta lista
        AbrirChildEnDashboard(New frmNotificacionCrear())
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para imprimir.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells("Id").Value)

        ' Abrimos el RPT como child; al cerrarlo volvés a esta pantalla
        AbrirChildEnDashboard(New frmNotificacionRPT(idSeleccionado))
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para editar.", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells("Id").Value)

        ' Antes: ShowDialog
        ' Ahora: abrir como child en el Dashboard (se restaura al cerrar)
        AbrirChildEnDashboard(New frmNotificacionCrear(idSeleccionado))
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
