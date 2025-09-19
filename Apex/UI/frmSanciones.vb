Public Class frmSanciones

    Private ReadOnly _svc As New SancionService()
    Private WithEvents SearchTimer As New System.Windows.Forms.Timer()

    Private Async Sub frmSanciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True

        ConfigurarGrillaSanciones()
        CargarComboTipoSancion()
        Await CargarDatosSancionesAsync()

        ' Búsqueda diferida
        SearchTimer.Interval = 500
        AddHandler txtBusquedaSancion.TextChanged, AddressOf IniciarTemporizador
        AddHandler cmbTipoSancion.SelectedIndexChanged, AddressOf IniciarTemporizador
        AddHandler SearchTimer.Tick, AddressOf Temporizador_Tick

        Try
            AppTheme.SetCue(txtBusquedaSancion, "Buscar por nombre o CI…")
            AppTheme.SetCue(cmbTipoSancion, "Filtrar por tipo de sanción…")
        Catch
        End Try
        txtBusquedaSancion.Focus()
    End Sub

#Region "UI"
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
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 130})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Comentario", .DataPropertyName = "Comentario", .HeaderText = "Observaciones", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
        End With

        AddHandler dgvSanciones.CellDoubleClick,
     Sub(sender As Object, e As DataGridViewCellEventArgs)
         btnEditarSancion.PerformClick()
     End Sub

    End Sub

    Private Sub CargarComboTipoSancion()
        ' Valores que usás en frmSancionCrear (coherencia)
        Dim items = New List(Of String) From {
            "[TODOS]",
            "Puntos de Demérito",
            "Observaciones Escritas"
        }
        cmbTipoSancion.DataSource = items
        cmbTipoSancion.SelectedIndex = 0
    End Sub
#End Region

#Region "Carga de datos"
    Private Async Function CargarDatosSancionesAsync() As Task
        Dim filtro = txtBusquedaSancion.Text.Trim()
        Dim filtroTipo As String = Nothing
        If cmbTipoSancion.SelectedItem IsNot Nothing AndAlso cmbTipoSancion.SelectedItem.ToString() <> "[TODOS]" Then
            filtroTipo = cmbTipoSancion.SelectedItem.ToString()
        End If

        LoadingHelper.MostrarCargando(Me)
        Try
            ' Proyección a DTO para evitar ChangeTracker
            Dim lista As List(Of SancionListadoItem) = Await _svc.GetListadoAsync(filtro, filtroTipo)
            dgvSanciones.DataSource = Nothing
            dgvSanciones.DataSource = lista
        Catch ex As Exception
            ' --- NOTIFIER APLICADO ---
            Notifier.Warn(Me, $"Error al cargar sanciones: {ex.Message}", 3000)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function
#End Region

#Region "Búsqueda"
    Private Sub IniciarTemporizador(sender As Object, e As EventArgs)
        SearchTimer.Stop()
        SearchTimer.Start()
    End Sub

    Private Async Sub Temporizador_Tick(sender As Object, e As EventArgs)
        SearchTimer.Stop()
        Await CargarDatosSancionesAsync()
    End Sub

    Private Async Sub txtBusquedaSancion_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaSancion.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SearchTimer.Stop()
            Await CargarDatosSancionesAsync()
        End If
    End Sub

    Private Async Sub frmSanciones_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F5 Then
            Await CargarDatosSancionesAsync()
            ' --- NOTIFIER APLICADO ---
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
            ' --- NOTIFIER APLICADO ---
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
            ' --- NOTIFIER APLICADO ---
            Notifier.Info(Me, "Seleccioná una sanción para eliminar.", 2000)
            Return
        End If
        Dim fila = TryCast(dgvSanciones.CurrentRow.DataBoundItem, SancionListadoItem)
        If fila Is Nothing Then Return

        If MessageBox.Show($"¿Eliminar la sanción de '{fila.NombreFuncionario}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _svc.DeleteAsync(fila.Id) ' GenericService borra por PK
                ' --- NOTIFIER APLICADO ---
                Notifier.Success(Me, "Sanción eliminada correctamente.", 2000)

                Await CargarDatosSancionesAsync()
            Catch ex As Exception
                ' --- NOTIFIER APLICADO ---
                Notifier.Error(Me, $"Error al eliminar: {ex.Message}", 3000)
            End Try
        End If
    End Sub
#End Region

#Region "Navegación"
    Private Function GetDashboard() As frmDashboard
        Return Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
    End Function

    Private Sub AbrirChildEnDashboard(formHijo As Form)
        Dim dash = GetDashboard()
        If dash Is Nothing Then
            ' --- NOTIFIER APLICADO ---
            Notifier.Warn(Me, "No se encontró el Dashboard activo.", 2500)
            Return
        End If
        dash.AbrirChild(formHijo)
    End Sub
#End Region

End Class
Public Class SancionListadoItem
    Public Property Id As Integer              ' PK de EstadoTransitorio (Sanción)
    Public Property NombreFuncionario As String
    Public Property FechaDesde As Date
    Public Property FechaHasta As Date?
    Public Property Estado As String
    Public Property Comentario As String
    Public Property TipoSancion As String
End Class

