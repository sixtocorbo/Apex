Imports System.Threading

Public Class frmSanciones

    ' --- INICIO DE CAMBIOS: Variables para la búsqueda mejorada ---
    Private WithEvents _searchTimer As New System.Windows.Forms.Timer()
    Private _ctsBusqueda As CancellationTokenSource
    Private _colorOriginalBusqueda As Color
    Private _estaBuscando As Boolean = False
    ' --- FIN DE CAMBIOS ---

    Private Async Sub frmSanciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True

        ConfigurarGrillaSanciones()
        CargarComboTipoSancion()

        ' --- INICIO DE CAMBIOS: Configuración de la nueva búsqueda ---
        _searchTimer.Interval = 700
        _searchTimer.Enabled = False
        _colorOriginalBusqueda = txtBusquedaSancion.BackColor

        ' Los eventos ahora se manejan directamente en la firma de los métodos
        ' AddHandler txtBusquedaSancion.TextChanged, AddressOf IniciarTemporizador ' Eliminado
        ' AddHandler cmbTipoSancion.SelectedIndexChanged, AddressOf IniciarTemporizador ' Eliminado
        ' AddHandler SearchTimer.Tick, AddressOf Temporizador_Tick ' Eliminado

        Try
            AppTheme.SetCue(txtBusquedaSancion, "Buscar por nombre o CI…")
        Catch
        End Try

        txtBusquedaSancion.Focus()

        Dim tk = ReiniciarToken()
        Await CargarDatosSancionesAsync(tk)
        ' --- FIN DE CAMBIOS ---
    End Sub

    Private Sub frmSanciones_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        ' Limpieza del CTS para evitar fugas de memoria
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
    End Sub

#Region "Lógica de Búsqueda Asíncrona"

    Private Function ReiniciarToken() As CancellationToken
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
        _ctsBusqueda = New CancellationTokenSource()
        Return _ctsBusqueda.Token
    End Function

    Private Async Function CargarDatosSancionesAsync(token As CancellationToken) As Task
        If _estaBuscando Then Return
        _estaBuscando = True

        ' Feedback visual no bloqueante
        txtBusquedaSancion.BackColor = Color.Gold
        Me.Cursor = Cursors.WaitCursor
        dgvSanciones.Enabled = False

        Try
            token.ThrowIfCancellationRequested()

            Dim filtro = txtBusquedaSancion.Text.Trim()
            Dim filtroTipo As String = Nothing
            If cmbTipoSancion.SelectedItem IsNot Nothing AndAlso cmbTipoSancion.SelectedItem.ToString() <> "[TODOS]" Then
                filtroTipo = cmbTipoSancion.SelectedItem.ToString()
            End If

            Await Task.Delay(50, token) ' Pausa para refrescar UI

            Dim lista As List(Of SancionListadoItem)
            Using svc As New SancionService()
                lista = Await svc.GetListadoAsync(filtro, filtroTipo).WaitAsync(token)
            End Using

            token.ThrowIfCancellationRequested()

            dgvSanciones.DataSource = lista

        Catch ex As OperationCanceledException
            ' Búsqueda cancelada. Silencioso.
        Catch ex As Exception
            Notifier.Warn(Me, $"Error al cargar sanciones: {ex.Message}", 3000)
        Finally
            If Not Me.IsDisposed Then
                txtBusquedaSancion.BackColor = _colorOriginalBusqueda
                Me.Cursor = Cursors.Default
                dgvSanciones.Enabled = True
                txtBusquedaSancion.Focus()
            End If
            _estaBuscando = False
        End Try
    End Function

    ' Se dispara al escribir en el TextBox o cambiar el ComboBox
    Private Sub Filtros_Cambiaron(sender As Object, e As EventArgs) Handles txtBusquedaSancion.TextChanged, cmbTipoSancion.SelectedIndexChanged
        _searchTimer.Stop()
        _searchTimer.Start()
    End Sub

    ' El timer ejecuta la búsqueda
    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs) Handles _searchTimer.Tick
        _searchTimer.Stop()
        Dim tk = ReiniciarToken()
        Await CargarDatosSancionesAsync(tk)
    End Sub

    ' Búsqueda inmediata con Enter
    Private Async Sub txtBusquedaSancion_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaSancion.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            _searchTimer.Stop()
            Dim tk = ReiniciarToken()
            Await CargarDatosSancionesAsync(tk)
        End If
    End Sub

    Private Async Sub frmSanciones_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F5 Then
            Dim tk = ReiniciarToken()
            Await CargarDatosSancionesAsync(tk)
            Notifier.Success(Me, "Lista de sanciones actualizada.", 1500)
            e.Handled = True
        ElseIf e.KeyCode = Keys.Delete Then
            btnEliminarSancion.PerformClick()
            e.Handled = True
        End If
    End Sub
#End Region

#Region "Acciones"
    Private Sub btnNuevaSancion_Click(sender As Object, e As EventArgs) Handles btnNuevaSancion.Click
        AbrirChildEnDashboard(New frmSancionCrear())
    End Sub

    Private Sub btnEditarSancion_Click(sender As Object, e As EventArgs) Handles btnEditarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then
            Notifier.Info(Me, "Seleccioná una sanción para editar.", 2000)
            Return
        End If
        Dim fila = TryCast(dgvSanciones.CurrentRow.DataBoundItem, SancionListadoItem)
        If fila Is Nothing Then Return

        Dim frm = New frmSancionCrear() With {.SancionId = fila.Id}
        AbrirChildEnDashboard(frm)
    End Sub

    Private Async Sub btnEliminarSancion_Click(sender As Object, e As EventArgs) Handles btnEliminarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then
            Notifier.Info(Me, "Seleccioná una sanción para eliminar.", 2000)
            Return
        End If
        Dim fila = TryCast(dgvSanciones.CurrentRow.DataBoundItem, SancionListadoItem)
        If fila Is Nothing Then Return

        If MessageBox.Show($"¿Eliminar la sanción de '{fila.NombreFuncionario}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Try
                Using svc As New SancionService()
                    Await svc.DeleteAsync(fila.Id)
                End Using
                Notifier.Success(Me, "Sanción eliminada correctamente.", 2000)

                Dim tk = ReiniciarToken()
                Await CargarDatosSancionesAsync(tk)
            Catch ex As Exception
                Notifier.Error(Me, $"Error al eliminar: {ex.Message}", 3000)
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub
#End Region

#Region "UI y Navegación"
    Private Sub ConfigurarGrillaSanciones()
        With dgvSanciones
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            Dim colDesde As New DataGridViewTextBoxColumn With {.Name = "FechaDesde", .DataPropertyName = "FechaDesde", .HeaderText = "Desde", .Width = 100}
            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colDesde)
            Dim colHasta As New DataGridViewTextBoxColumn With {.Name = "FechaHasta", .DataPropertyName = "FechaHasta", .HeaderText = "Hasta", .Width = 100}
            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colHasta)
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoSancion", .DataPropertyName = "TipoSancion", .HeaderText = "Tipo", .Width = 150})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .Width = 130})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Comentario", .DataPropertyName = "Comentario", .HeaderText = "Observaciones", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
        End With

        AddHandler dgvSanciones.CellDoubleClick, Sub(sender As Object, e As DataGridViewCellEventArgs)
                                                     btnEditarSancion.PerformClick()
                                                 End Sub
    End Sub

    Private Sub CargarComboTipoSancion()
        Dim items = New List(Of String) From {
            "[TODOS]",
            "Puntos de Demérito",
            "Observaciones Escritas"
        }
        cmbTipoSancion.DataSource = items
        cmbTipoSancion.SelectedIndex = 0
    End Sub

    Private Function GetDashboard() As frmDashboard
        Return Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
    End Function

    Private Sub AbrirChildEnDashboard(formHijo As Form)
        Dim dash = GetDashboard()
        If dash Is Nothing Then
            Notifier.Warn(Me, "No se encontró el Dashboard activo.", 2500)
            Return
        End If
        dash.AbrirChild(formHijo)
    End Sub
#End Region

End Class

Public Class SancionListadoItem
    Public Property Id As Integer
    Public Property NombreFuncionario As String
    Public Property FechaDesde As Date
    Public Property FechaHasta As Date?
    Public Property Estado As String
    Public Property Comentario As String
    Public Property TipoSancion As String
End Class