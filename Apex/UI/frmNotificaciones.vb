Option Strict On
Option Explicit On

Imports System.ComponentModel

Public Class frmNotificaciones
    Private ReadOnly _svc As New NotificacionService()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmGestionNotificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
    End Sub

    Private Sub frmGestionNotificaciones_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.ActiveControl = txtFiltro
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
                .DataPropertyName = "Id", ' Asegurate que la vista expone "Id". Si es "NotificacionId", cambia acá.
                .HeaderText = "ID",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
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

    Private Async Function BuscarAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim filtro = txtFiltro.Text.Trim()

            ' Si querés ver “últimas” cuando está vacío, podés quitar este If.
            If String.IsNullOrWhiteSpace(filtro) Then
                dgvNotificaciones.DataSource = Nothing
                Return
            End If

            Dim resultados = Await _svc.GetAllConDetallesAsync(filtro:=filtro)
            dgvNotificaciones.DataSource = New BindingList(Of vw_NotificacionesCompletas)(resultados)

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al buscar las notificaciones: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Async Sub txtFiltro_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFiltro.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            Await BuscarAsync()
        End If
    End Sub

#End Region

#Region "Acciones (CRUD)"

    Private Async Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Using frm As New frmNotificacionCrear()
            If frm.ShowDialog() = DialogResult.OK Then
                Await BuscarAsync()
            End If
        End Using
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
                Await BuscarAsync()
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

        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(frm)
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
                ' Usa el método atómico con auditoría:
                Await _svc.DeleteNotificacionAsync(idSeleccionado)
                Await BuscarAsync()
            Catch ex As Exception
                MessageBox.Show($"Ocurrió un error al eliminar: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region

#Region "Mejoras de UX"

    Private Async Sub dgvNotificaciones_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvNotificaciones.CellDoubleClick
        If e.RowIndex >= 0 AndAlso dgvNotificaciones.Rows.Count > 0 Then
            Dim idSeleccionado = CInt(dgvNotificaciones.Rows(e.RowIndex).Cells("Id").Value)
            Using frm As New frmNotificacionCrear(idSeleccionado)
                If frm.ShowDialog() = DialogResult.OK Then
                    Await BuscarAsync()
                End If
            End Using
        End If
    End Sub

    Private Async Sub frmNotificaciones_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.F5
                Await BuscarAsync()
            Case Keys.Delete
                btnEliminar.PerformClick()
        End Select
    End Sub

#End Region

End Class
