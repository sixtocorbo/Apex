Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmSecciones

    Private _listaSecciones As BindingList(Of Seccion)
    Private _seccionSeleccionada As Seccion
    Private _estaCargando As Boolean = False
    Private _ultimoIdSeleccionado As Integer = 0
    Private _ctsBusqueda As CancellationTokenSource

    ' ===== Ciclo de vida =====
    Private Async Sub frmSecciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.AcceptButton = btnGuardar
        Me.KeyPreview = True

        ConfigurarGrilla()
        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre...")
            AppTheme.SetCue(txtNombre, "Nombre de la sección...")
        Catch
        End Try

        Await CargarDatosAsync()
        LimpiarCampos()
        Notifier.Info(Me, "Listado de secciones listo.")
    End Sub

    ' ===== Datos =====
    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Cursor = Cursors.WaitCursor
        dgvSecciones.Enabled = False

        Try
            Dim lista As List(Of Seccion)
            Using svc As New SeccionService()
                lista = Await svc.GetAllAsync()
            End Using

            _listaSecciones = New BindingList(Of Seccion)(lista)
            dgvSecciones.DataSource = _listaSecciones
            RestaurarSeleccion()
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudieron cargar las secciones: {ex.Message}")
            dgvSecciones.DataSource = New BindingList(Of Seccion)()
        Finally
            Cursor = Cursors.Default
            dgvSecciones.Enabled = True
            _estaCargando = False
        End Try
    End Function

    ' ===== UI =====
    Private Sub ConfigurarGrilla()
        dgvSecciones.AutoGenerateColumns = False
        dgvSecciones.Columns.Clear()

        dgvSecciones.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Id",
            .HeaderText = "Id",
            .Visible = False
        })

        dgvSecciones.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre",
            .HeaderText = "Nombre",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })

        HabilitarDoubleBuffering(dgvSecciones)
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
        _seccionSeleccionada = New Seccion()
        txtNombre.Clear()
        btnEliminar.Enabled = False
        dgvSecciones.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _seccionSeleccionada Is Nothing Then
            LimpiarCampos()
            Return
        End If

        txtNombre.Text = _seccionSeleccionada.Nombre
        btnEliminar.Enabled = (_seccionSeleccionada.Id <> 0)
    End Sub

    Private Sub RestaurarSeleccion()
        If _ultimoIdSeleccionado = 0 OrElse dgvSecciones.Rows.Count = 0 Then
            dgvSecciones.ClearSelection()
            Return
        End If

        For Each row As DataGridViewRow In dgvSecciones.Rows
            Dim s = TryCast(row.DataBoundItem, Seccion)
            If s IsNot Nothing AndAlso s.Id = _ultimoIdSeleccionado Then
                row.Selected = True
                Dim firstVisibleCol = dgvSecciones.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
                If firstVisibleCol IsNot Nothing Then
                    dgvSecciones.CurrentCell = row.Cells(firstVisibleCol.Index)
                ElseIf row.Cells.Count > 0 Then
                    dgvSecciones.CurrentCell = row.Cells(0)
                End If
                dgvSecciones.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index)
                Exit For
            End If
        Next
    End Sub

    ' ===== Eventos =====
    Private Sub dgvSecciones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSecciones.SelectionChanged
        If dgvSecciones.CurrentRow Is Nothing OrElse dgvSecciones.CurrentRow.DataBoundItem Is Nothing Then Return
        _seccionSeleccionada = CType(dgvSecciones.CurrentRow.DataBoundItem, Seccion)
        _ultimoIdSeleccionado = _seccionSeleccionada.Id
        MostrarDetalles()
    End Sub

    ' Búsqueda con debounce
    Private Async Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If _listaSecciones Is Nothing Then Return

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
            dgvSecciones.DataSource = _listaSecciones
        Else
            Dim resultado = _listaSecciones.
                Where(Function(s) s.Nombre IsNot Nothing AndAlso s.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                ToList()
            dgvSecciones.DataSource = New BindingList(Of Seccion)(resultado)
        End If

        dgvSecciones.ClearSelection()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre de la sección no puede estar vacío.")
            txtNombre.Focus()
            Return
        End If

        Dim nombre = txtNombre.Text.Trim()
        Dim idActual As Integer = If(_seccionSeleccionada Is Nothing, 0, _seccionSeleccionada.Id)

        ' duplicados (case-insensitive) excepto el registro actual
        Dim existeDuplicado = (_listaSecciones IsNot Nothing AndAlso
                               _listaSecciones.Any(Function(x) x IsNot Nothing AndAlso
                                                          x.Id <> idActual AndAlso
                                                          String.Equals(x.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
        If existeDuplicado Then
            Notifier.Warn(Me, "Ya existe una sección con ese nombre.")
            txtNombre.SelectAll()
            txtNombre.Focus()
            Return
        End If

        _seccionSeleccionada.Nombre = nombre

        Cursor = Cursors.WaitCursor
        btnGuardar.Enabled = False
        Try
            Using svc As New SeccionService()
                If idActual = 0 Then
                    ' Si CreateAsync devuelve Integer (Id) lo capturamos; si no, ignoramos
                    Dim nuevoId As Integer = 0
                    Try
                        nuevoId = Await svc.CreateAsync(_seccionSeleccionada)
                    Catch
                        ' Si el método no devuelve nada, no pasa nada
                    End Try
                    _ultimoIdSeleccionado = If(nuevoId > 0, nuevoId, 0)
                    Notifier.Success(Me, "Sección creada correctamente.")
                Else
                    Await svc.UpdateAsync(_seccionSeleccionada)
                    _ultimoIdSeleccionado = idActual
                    Notifier.Success(Me, "Sección actualizada correctamente.")
                End If
            End Using

            Await CargarDatosAsync()
            If _ultimoIdSeleccionado = 0 Then
                ' si no conocemos el id (método sin retorno), intenta seleccionar por nombre
                Dim fila = _listaSecciones.FirstOrDefault(Function(s) String.Equals(s.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                If fila IsNot Nothing Then
                    _ultimoIdSeleccionado = fila.Id
                    RestaurarSeleccion()
                End If
            End If
            MostrarDetalles()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al guardar la sección: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _seccionSeleccionada Is Nothing OrElse _seccionSeleccionada.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar una sección para eliminar.")
            Return
        End If

        Dim confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea eliminar la sección '{_seccionSeleccionada.Nombre}'?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion <> DialogResult.Yes Then Return

        Cursor = Cursors.WaitCursor
        btnEliminar.Enabled = False
        Try
            Using svc As New SeccionService()
                Await svc.DeleteAsync(_seccionSeleccionada.Id)
            End Using
            Notifier.Info(Me, "Sección eliminada.")
            _ultimoIdSeleccionado = 0
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al eliminar la sección: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnEliminar.Enabled = True
        End Try
    End Sub

    ' ===== Atajos teclado =====
    Private Sub frmSecciones_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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

End Class
