' En tu archivo: Apex/UI/frmGestionNotificaciones.vb

Imports System.ComponentModel

Public Class frmNotificaciones
    Private _svc As New NotificacionService()

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmGestionNotificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .HeaderText = "ID", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "CI", .HeaderText = "Cédula", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaProgramada", .HeaderText = "Fecha Programada", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "TipoNotificacion", .HeaderText = "Tipo", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Estado", .HeaderText = "Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})

            .ResumeLayout()
        End With
    End Sub

#Region "Lógica de Búsqueda"

    Private Async Function BuscarAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        ' Asumo que tienes un botón de búsqueda llamado btnBuscar
        ' btnBuscar.Enabled = False 

        Try
            Dim filtro = txtFiltro.Text.Trim()

            ' Si el filtro está vacío, limpiamos la grilla y salimos.
            If String.IsNullOrWhiteSpace(filtro) Then
                dgvNotificaciones.DataSource = Nothing
                Return
            End If

            ' Llamamos al servicio con el filtro
            Dim resultados = Await _svc.GetAllConDetallesAsync(filtro)
            dgvNotificaciones.DataSource = New BindingList(Of vw_NotificacionesCompletas)(resultados)

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al buscar las notificaciones: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
            ' btnBuscar.Enabled = True
        End Try
    End Function

    ' Evento para buscar al presionar Enter en el cuadro de texto
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
                Await BuscarAsync() ' Refrescamos la búsqueda actual
            End If
        End Using
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells("Id").Value)
        Using frm As New frmNotificacionCrear()
            frm.NotificacionId = idSeleccionado
            If frm.ShowDialog() = DialogResult.OK Then
                Await BuscarAsync() ' Refrescamos la búsqueda actual
            End If
        End Using
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim idSeleccionado = CInt(dgvNotificaciones.SelectedRows(0).Cells("Id").Value)
        Dim frm As New frmNotificacionRPT(idSeleccionado)

        ' Se obtiene una referencia al formulario Dashboard
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)

        ' Se llama al método público del Dashboard para abrir el formulario en el panel
        parentDashboard.AbrirFormEnPanel(frm)
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If dgvNotificaciones.SelectedRows.Count = 0 Then
            MessageBox.Show("Seleccione una notificación para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim filaSeleccionada = dgvNotificaciones.SelectedRows(0)
        Dim idSeleccionado = CInt(filaSeleccionada.Cells("Id").Value)
        Dim nombreFuncionario = filaSeleccionada.Cells("NombreFuncionario").Value.ToString()

        If MessageBox.Show($"¿Está seguro de que desea eliminar la notificación para '{nombreFuncionario}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _svc.DeleteAsync(idSeleccionado)
                Await BuscarAsync() ' Refrescamos la búsqueda actual
            Catch ex As Exception
                MessageBox.Show($"Ocurrió un error al eliminar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class