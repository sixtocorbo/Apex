Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmAreaTrabajoCategorias

    Private _listaAreas As BindingList(Of AreaTrabajo)
    Private _areaSeleccionada As AreaTrabajo
    Private _estaCargando As Boolean = False
    Private _ultimoIdSeleccionado As Integer = 0
    Private _ctsBusqueda As CancellationTokenSource

    ' ===== Ciclo de vida =====
    Private Async Sub frmAreaTrabajoCategorias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.AcceptButton = btnGuardar
        Me.KeyPreview = True

        ConfigurarGrilla()
        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre...")
            AppTheme.SetCue(txtNombre, "Nombre del área...")
        Catch
        End Try

        Await CargarDatosAsync()
        LimpiarCampos()
        Notifier.Success(Me, "Listado de áreas de trabajo listo.")
    End Sub

    ' ===== Datos =====
    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Cursor = Cursors.WaitCursor
        dgvAreas.Enabled = False

        Try
            Dim lista As List(Of AreaTrabajo)
            Using svc As New AreaTrabajoService()
                lista = Await svc.GetAllAsync()
            End Using

            _listaAreas = New BindingList(Of AreaTrabajo)(lista)
            dgvAreas.DataSource = _listaAreas
            RestaurarSeleccion()
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudieron cargar las áreas de trabajo: {ex.Message}")
            dgvAreas.DataSource = New BindingList(Of AreaTrabajo)()
        Finally
            Cursor = Cursors.Default
            dgvAreas.Enabled = True
            _estaCargando = False
        End Try
    End Function

    ' ===== UI =====
    Private Sub ConfigurarGrilla()
        ' --- APLICAR ESTILOS MODERNOS ---
        With dgvAreas
            ' --- Configuración General ---
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .GridColor = Color.FromArgb(230, 230, 230)
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AutoGenerateColumns = False
            .BackgroundColor = Color.White

            ' --- Estilo de Encabezados (Headers) ---
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

            ' --- Estilo de Filas (Rows) ---
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247) ' Efecto Cebra
        End With

        ' --- DEFINICIÓN DE COLUMNAS (Se mantiene tu lógica original) ---
        dgvAreas.Columns.Clear()

        dgvAreas.Columns.Add(New DataGridViewTextBoxColumn With {
        .DataPropertyName = "Id",
        .HeaderText = "Id",
        .Visible = False
    })

        dgvAreas.Columns.Add(New DataGridViewTextBoxColumn With {
        .DataPropertyName = "Nombre",
        .HeaderText = "Nombre del Área de Trabajo", ' Título más descriptivo
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    })

        ' --- RENDIMIENTO (Se mantiene tu llamada original) ---
        HabilitarDoubleBuffering(dgvAreas)
    End Sub

    Private Sub HabilitarDoubleBuffering(dgv As DataGridView)
        Try
            dgv.GetType().InvokeMember("DoubleBuffered",
                                       BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.SetProperty,
                                       Nothing, dgv, New Object() {True})
        Catch
        End Try
    End Sub

    Private Sub LimpiarCampos()
        _areaSeleccionada = New AreaTrabajo()
        txtNombre.Clear()
        btnEliminar.Enabled = False
        dgvAreas.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _areaSeleccionada Is Nothing Then
            LimpiarCampos()
            Return
        End If

        txtNombre.Text = _areaSeleccionada.Nombre
        btnEliminar.Enabled = (_areaSeleccionada.Id <> 0)
    End Sub

    Private Sub RestaurarSeleccion()
        If _ultimoIdSeleccionado = 0 OrElse dgvAreas.Rows.Count = 0 Then
            dgvAreas.ClearSelection()
            Return
        End If

        For Each row As DataGridViewRow In dgvAreas.Rows
            Dim a = TryCast(row.DataBoundItem, AreaTrabajo)
            If a IsNot Nothing AndAlso a.Id = _ultimoIdSeleccionado Then
                row.Selected = True
                Dim firstVisibleCol = dgvAreas.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
                If firstVisibleCol IsNot Nothing Then
                    dgvAreas.CurrentCell = row.Cells(firstVisibleCol.Index)
                ElseIf row.Cells.Count > 0 Then
                    dgvAreas.CurrentCell = row.Cells(0)
                End If
                dgvAreas.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index)
                Exit For
            End If
        Next
    End Sub

    ' ===== Eventos =====
    Private Sub dgvAreas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvAreas.SelectionChanged
        If dgvAreas.CurrentRow Is Nothing OrElse dgvAreas.CurrentRow.DataBoundItem Is Nothing Then Return
        _areaSeleccionada = CType(dgvAreas.CurrentRow.DataBoundItem, AreaTrabajo)
        _ultimoIdSeleccionado = _areaSeleccionada.Id
        MostrarDetalles()
    End Sub

    ' Búsqueda con debounce
    Private Async Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If _listaAreas Is Nothing Then Return

        _ctsBusqueda?.Cancel()
        _ctsBusqueda = New CancellationTokenSource()
        Dim tk = _ctsBusqueda.Token

        Try
            Await Task.Delay(250, tk)
        Catch ex As TaskCanceledException
            Return
        End Try
        If tk.IsCancellationRequested Then Return

        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvAreas.DataSource = _listaAreas
        Else
            Dim resultado = _listaAreas.
                Where(Function(a) a IsNot Nothing AndAlso a.Nombre IsNot Nothing AndAlso
                                  a.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                ToList()
            dgvAreas.DataSource = New BindingList(Of AreaTrabajo)(resultado)
        End If

        dgvAreas.ClearSelection()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre del área no puede estar vacío.")
            txtNombre.Focus()
            Return
        End If

        Dim nombre = txtNombre.Text.Trim()
        Dim idActual As Integer = If(_areaSeleccionada Is Nothing, 0, _areaSeleccionada.Id)

        ' Duplicados por nombre (case-insensitive), excluyendo el registro actual
        Dim existeDuplicado = (_listaAreas IsNot Nothing AndAlso
                               _listaAreas.Any(Function(x) x IsNot Nothing AndAlso
                                                       x.Id <> idActual AndAlso
                                                       String.Equals(x.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
        If existeDuplicado Then
            Notifier.Warn(Me, "Ya existe un área con ese nombre.")
            txtNombre.SelectAll()
            txtNombre.Focus()
            Return
        End If

        _areaSeleccionada.Nombre = nombre

        Cursor = Cursors.WaitCursor
        btnGuardar.Enabled = False
        Try
            Using svc As New AreaTrabajoService()
                If idActual = 0 Then
                    ' Puede devolver Integer (Id) o no devolver nada
                    Dim nuevoId As Integer = 0
                    Try
                        nuevoId = Await svc.CreateAsync(_areaSeleccionada)
                    Catch
                        ' Si no devuelve valor, seguimos
                    End Try
                    _ultimoIdSeleccionado = If(nuevoId > 0, nuevoId, 0)
                    Notifier.Success(Me, "Área creada correctamente.")
                Else
                    Await svc.UpdateAsync(_areaSeleccionada)
                    _ultimoIdSeleccionado = idActual
                    Notifier.Success(Me, "Área actualizada correctamente.")
                End If
            End Using

            Await CargarDatosAsync()
            If _ultimoIdSeleccionado = 0 Then
                ' Si no conocemos el Id (método sin retorno), intentar seleccionar por nombre
                Dim fila = _listaAreas.FirstOrDefault(Function(a) String.Equals(a.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                If fila IsNot Nothing Then
                    _ultimoIdSeleccionado = fila.Id
                    RestaurarSeleccion()
                End If
            End If
            MostrarDetalles()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al guardar el área: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _areaSeleccionada Is Nothing OrElse _areaSeleccionada.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar un área para eliminar.")
            Return
        End If

        Dim confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea eliminar el área '{_areaSeleccionada.Nombre}'?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion <> DialogResult.Yes Then Return

        Cursor = Cursors.WaitCursor
        btnEliminar.Enabled = False
        Try
            Using svc As New AreaTrabajoService()
                Await svc.DeleteAsync(_areaSeleccionada.Id)
            End Using
            Notifier.Success(Me, "Área eliminada.")
            _ultimoIdSeleccionado = 0
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al eliminar el área: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnEliminar.Enabled = True
        End Try
    End Sub

    ' ===== Atajos teclado y navegación =====
    Private Sub frmAreaTrabajoCategorias_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Close()
        ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
            btnNuevo.PerformClick()
        ElseIf e.Control AndAlso e.KeyCode = Keys.S Then
            btnGuardar.PerformClick()
        ElseIf e.KeyCode = Keys.Delete AndAlso btnEliminar.Enabled Then
            btnEliminar.PerformClick()
        End If
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        ' Si tienes un helper de navegación, úsalo aquí.
        ' NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmConfiguracion)(Me)
        Close()
    End Sub

End Class
