Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Threading

Public Class frmNotificaciones
    Private _entidadActual As vw_NotificacionesCompletas

    ' Variables para la búsqueda mejorada
    Private WithEvents _searchTimer As New System.Windows.Forms.Timer()
    Private _ctsBusqueda As CancellationTokenSource
    Private _colorOriginalBusqueda As Color
    Private _estaBuscando As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

#Region "Ciclo de Vida y Eventos Principales"

    Private Sub frmGestionNotificaciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        AppTheme.Aplicar(Me)
        dgvNotificaciones.ActivarDobleBuffer(True)
        ConfigurarGrilla()

        ' Configuración de la nueva búsqueda
        _searchTimer.Interval = 700
        _searchTimer.Enabled = False
        _colorOriginalBusqueda = txtFiltro.BackColor

        Try
            AppTheme.SetCue(txtFiltro, "Filtrar por funcionario...")
            AppTheme.SetCue(rtbNotificacion, "Aquí se muestra el texto completo de la notificación seleccionada...")
        Catch
        End Try

        AddHandler NotificadorEventos.FuncionarioActualizado, AddressOf OnFuncionarioActualizado
    End Sub

    Private Async Sub frmGestionNotificaciones_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Dim tk = ReiniciarToken()
        Await BuscarAsync(tk)
        Me.ActiveControl = txtFiltro
    End Sub

    Private Sub frmGestionNotificaciones_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.FuncionarioActualizado, AddressOf OnFuncionarioActualizado
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
    End Sub

    Private Async Sub OnFuncionarioActualizado(sender As Object, e As FuncionarioCambiadoEventArgs)
        Dim tk = ReiniciarToken()
        Await BuscarAsync(tk)
    End Sub

#End Region

#Region "Lógica de Búsqueda Asíncrona"

    Private Function ReiniciarToken() As CancellationToken
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
        _ctsBusqueda = New CancellationTokenSource()
        Return _ctsBusqueda.Token
    End Function

    Private Async Function BuscarAsync(token As CancellationToken) As Task
        If _estaBuscando Then Return
        _estaBuscando = True

        txtFiltro.BackColor = Color.Gold
        Me.Cursor = Cursors.WaitCursor
        dgvNotificaciones.Enabled = False

        Try
            token.ThrowIfCancellationRequested()
            Dim filtroFuncionario = txtFiltro.Text.Trim()
            Await Task.Delay(50, token)

            ' --- CAMBIO CLAVE: Se crea una instancia del servicio por cada búsqueda ---
            Using svc As New NotificacionService()
                Dim resultados = Await svc.GetAllConDetallesAsync(filtro:="", funcionarioFiltro:=filtroFuncionario).WaitAsync(token)
                token.ThrowIfCancellationRequested()
                dgvNotificaciones.DataSource = New BindingList(Of vw_NotificacionesCompletas)(resultados)
            End Using ' El servicio y su DbContext se liberan aquí

            dgvNotificaciones.ClearSelection()
            rtbNotificacion.Text = "—"

        Catch ex As OperationCanceledException
            ' Búsqueda cancelada. Silencioso.
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al buscar: {ex.Message}")
            dgvNotificaciones.DataSource = New BindingList(Of vw_NotificacionesCompletas)()
            rtbNotificacion.Text = "—"
        Finally
            If Not Me.IsDisposed Then
                txtFiltro.BackColor = _colorOriginalBusqueda
                Me.Cursor = Cursors.Default
                dgvNotificaciones.Enabled = True
                txtFiltro.Focus()
            End If
            _estaBuscando = False
        End Try
    End Function

    Private Sub Filtro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        _searchTimer.Stop()
        _searchTimer.Start()
    End Sub

    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs) Handles _searchTimer.Tick
        _searchTimer.Stop()
        Dim tk = ReiniciarToken()
        Await BuscarAsync(tk)
        txtFiltro.Focus()
    End Sub

    Private Async Sub txtFiltro_KeyDown(sender As Object, e As KeyEventArgs) Handles txtFiltro.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            _searchTimer.Stop()
            Dim tk = ReiniciarToken()
            Await BuscarAsync(tk)
            txtFiltro.Focus()
        End If
    End Sub

#End Region

#Region "Acciones y Eventos de la Grilla"

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
                    ' --- CAMBIO CLAVE: Instancia local del servicio ---
                    Using svc As New NotificacionService()
                        Dim success As Boolean = Await svc.UpdateEstadoAsync(selectedNotification.Id, frm.SelectedEstadoId)
                        If success Then
                            Notifier.Success(Me, "Estado actualizado.")
                            Dim tk = ReiniciarToken()
                            Await BuscarAsync(tk)
                        Else
                            Notifier.Error(Me, "No se pudo actualizar el estado.")
                        End If
                    End Using
                Catch ex As Exception
                    Notifier.Error(Me, $"Error al actualizar el estado: {ex.Message}")
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
            Notifier.Warn(Me, "Seleccione una o más notificaciones para imprimir.")
            Return
        End If

        Dim idsSeleccionados As New List(Of Integer)
        For Each row As DataGridViewRow In dgvNotificaciones.SelectedRows
            Dim notificacion = TryCast(row.DataBoundItem, vw_NotificacionesCompletas)
            If notificacion IsNot Nothing Then
                idsSeleccionados.Add(notificacion.Id)
            End If
        Next

        If idsSeleccionados.Count = 0 Then
            Notifier.Error(Me, "No se pudieron obtener los IDs de las notificaciones seleccionadas.")
            Return
        End If
        AbrirChildEnDashboard(New frmNotificacionRPT(idsSeleccionados))
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
            ' --- CAMBIO CLAVE: Instancia local del servicio ---
            Using svc As New NotificacionService()
                Await svc.DeleteNotificacionAsync(idSeleccionado)
            End Using

            Notifier.Info(Me, "Notificación eliminada.")
            Dim tk = ReiniciarToken()
            Await BuscarAsync(tk)
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al eliminar: {ex.Message}")
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
                Dim tk = ReiniciarToken()
                Await BuscarAsync(tk)
            Case Keys.Delete
                If btnEliminar.Enabled Then
                    btnEliminar.PerformClick()
                End If
        End Select
    End Sub

    Private Async Sub btnCambiarEstado_Click(sender As Object, e As EventArgs) Handles btnCambiarEstado.Click
        Await CambiarEstado()
    End Sub

    Private Sub btnNuevaMasiva_Click(sender As Object, e As EventArgs) Handles btnNuevaMasiva.Click
        AbrirChildEnDashboard(New frmNotificacionMasiva())
    End Sub

#End Region

#Region "Configuración de Grilla y Helpers"

    ' Reemplaza este método en la región "Configuración de Grilla y Helpers"
    Private Sub ConfigurarGrilla()
        With dgvNotificaciones
            .SuspendLayout()

            ' --- CONFIGURACIÓN GENERAL ---
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .GridColor = Color.FromArgb(230, 230, 230)
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = True ' Se mantiene de tu configuración original
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToResizeRows = False
            .AutoGenerateColumns = False
            .BackgroundColor = Color.White

            ' --- ESTILO DE ENCABEZADOS (Headers) ---
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

            ' --- ESTILO DE FILAS (Rows) ---
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247) ' Efecto Cebra

            ' --- DEFINICIÓN DE COLUMNAS (Mantiene tu lógica original con estilo mejorado) ---
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ' Ocupa el espacio restante
            .MinimumWidth = 200
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "CI", .DataPropertyName = "CI", .HeaderText = "Cédula",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ' Se ajusta al contenido
            .MinimumWidth = 90
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "FechaProgramada", .DataPropertyName = "FechaProgramada", .HeaderText = "Fecha Programada",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .MinimumWidth = 150,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "yyyy-MM-dd HH:mm"} ' Se mantiene tu formato
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "TipoNotificacion", .DataPropertyName = "TipoNotificacion", .HeaderText = "Tipo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .MinimumWidth = 120
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .MinimumWidth = 100
        })

            .ResumeLayout()
        End With
    End Sub

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
        Catch ex As Exception
            Notifier.Error(dash, $"No se pudo abrir la ventana: {ex.Message}")
        End Try
    End Sub

#End Region

End Class