Imports System.Data.Entity
Imports System.Drawing
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmEscalafones

    Private _listaCompleta As List(Of Escalafon)
    Private ReadOnly _bindingSource As New BindingSource()
    Private _seleccionActual As Escalafon
    Private _estaCargando As Boolean
    Private _ultimoIdSeleccionado As Integer
    Private _ctsBusqueda As CancellationTokenSource

    Private Async Sub frmEscalafones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.AcceptButton = btnGuardar
        Me.KeyPreview = True

        dgvEscalafones.ActivarDobleBuffer(True)
        ConfigurarGrilla()

        Try
            AppTheme.SetCue(txtBuscar, "Buscar escalafones...")
            AppTheme.SetCue(txtNombre, "Nombre del escalafón")
        Catch
        End Try

        Await CargarDatosAsync()
        PrepararEventos()
        LimpiarFormulario()
        Notifier.Info(Me, "Listado de escalafones listo.")
    End Sub

    Private Sub PrepararEventos()
        AddHandler Me.KeyDown, AddressOf frmEscalafones_KeyDown
        AddHandler dgvEscalafones.CellDoubleClick, AddressOf dgvEscalafones_CellDoubleClick
    End Sub

    Private Sub ConfigurarGrilla()
        dgvEscalafones.AutoGenerateColumns = False
        dgvEscalafones.Columns.Clear()

        dgvEscalafones.EnableHeadersVisualStyles = False
        dgvEscalafones.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgvEscalafones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
        dgvEscalafones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvEscalafones.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        dgvEscalafones.ColumnHeadersHeight = 40
        dgvEscalafones.DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
        dgvEscalafones.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
        dgvEscalafones.DefaultCellStyle.SelectionForeColor = Color.White
        dgvEscalafones.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)
        dgvEscalafones.BorderStyle = BorderStyle.None
        dgvEscalafones.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal

        dgvEscalafones.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = NameOf(Escalafon.Id),
            .Visible = False
        })

        dgvEscalafones.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = NameOf(Escalafon.Nombre),
            .HeaderText = "Nombre",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
    End Sub

    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Cursor = Cursors.WaitCursor
        dgvEscalafones.Enabled = False

        Try
            Using svc As New EscalafonService()
                Dim lista = Await svc.GetAllAsync()
                _listaCompleta = lista.
                    OrderBy(Function(e) e.Nombre).
                    ToList()
            End Using

            AplicarFiltro(txtBuscar.Text.Trim())
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudieron cargar los escalafones: {ex.Message}")
            _listaCompleta = New List(Of Escalafon)()
            _bindingSource.DataSource = _listaCompleta
            dgvEscalafones.DataSource = _bindingSource
        Finally
            dgvEscalafones.Enabled = True
            Cursor = Cursors.Default
            _estaCargando = False
        End Try
    End Function

    Private Sub AplicarFiltro(filtro As String)
        If _listaCompleta Is Nothing Then Return

        Dim listaFiltrada = _listaCompleta
        If Not String.IsNullOrWhiteSpace(filtro) Then
            listaFiltrada = _listaCompleta.
                Where(Function(e) e.Nombre IsNot Nothing AndAlso e.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                OrderBy(Function(e) e.Nombre).
                ToList()
        Else
            listaFiltrada = _listaCompleta.OrderBy(Function(e) e.Nombre).ToList()
        End If

        _bindingSource.DataSource = listaFiltrada
        dgvEscalafones.DataSource = _bindingSource

        SeleccionarFilaPorId(_ultimoIdSeleccionado)
    End Sub

    Private Sub SeleccionarFilaPorId(id As Integer)
        If id <= 0 OrElse dgvEscalafones.Rows.Count = 0 Then
            dgvEscalafones.ClearSelection()
            _seleccionActual = Nothing
            btnEliminar.Enabled = False
            Return
        End If

        For Each row As DataGridViewRow In dgvEscalafones.Rows
            Dim elemento = TryCast(row.DataBoundItem, Escalafon)
            If elemento IsNot Nothing AndAlso elemento.Id = id Then
                row.Selected = True
                Dim firstVisible = dgvEscalafones.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
                If firstVisible IsNot Nothing Then
                    dgvEscalafones.CurrentCell = row.Cells(firstVisible.Index)
                ElseIf row.Cells.Count > 0 Then
                    dgvEscalafones.CurrentCell = row.Cells(0)
                End If
                dgvEscalafones.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index)
                _seleccionActual = elemento
                MostrarDetalle()
                Return
            End If
        Next

        dgvEscalafones.ClearSelection()
        _seleccionActual = Nothing
        btnEliminar.Enabled = False
    End Sub

    Private Sub MostrarDetalle()
        If _seleccionActual Is Nothing Then
            txtNombre.Clear()
            btnEliminar.Enabled = False
            Return
        End If

        txtNombre.Text = _seleccionActual.Nombre
        btnEliminar.Enabled = (_seleccionActual.Id > 0)
    End Sub

    Private Sub LimpiarFormulario()
        _seleccionActual = Nothing
        _ultimoIdSeleccionado = 0
        txtNombre.Clear()
        txtNombre.Focus()
        btnEliminar.Enabled = False
        dgvEscalafones.ClearSelection()
    End Sub

    Private Sub dgvEscalafones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvEscalafones.SelectionChanged
        If dgvEscalafones.CurrentRow Is Nothing OrElse dgvEscalafones.CurrentRow.DataBoundItem Is Nothing Then
            Return
        End If

        _seleccionActual = CType(dgvEscalafones.CurrentRow.DataBoundItem, Escalafon)
        _ultimoIdSeleccionado = _seleccionActual.Id
        MostrarDetalle()
    End Sub

    Private Sub dgvEscalafones_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            dgvEscalafones.Rows(e.RowIndex).Selected = True
            dgvEscalafones_SelectionChanged(dgvEscalafones, EventArgs.Empty)
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        _ctsBusqueda?.Cancel()
        _ctsBusqueda = New CancellationTokenSource()
        Dim token = _ctsBusqueda.Token

        Task.Run(Async Function()
                     Try
                         Await Task.Delay(250, token)
                         If token.IsCancellationRequested OrElse Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return
                         Invoke(New Action(Sub() AplicarFiltro(txtBuscar.Text.Trim())))
                     Catch ex As TaskCanceledException
                     End Try
                 End Function)
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarFormulario()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim nombre = txtNombre.Text.Trim()
        If String.IsNullOrWhiteSpace(nombre) Then
            Notifier.Warn(Me, "El nombre del escalafón no puede estar vacío.")
            txtNombre.Focus()
            Return
        End If

        Dim idActual = If(_seleccionActual Is Nothing, 0, _seleccionActual.Id)
        Dim existeDuplicado = (_listaCompleta IsNot Nothing AndAlso _listaCompleta.Any(Function(x) x.Id <> idActual AndAlso String.Equals(x.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
        If existeDuplicado Then
            Notifier.Warn(Me, "Ya existe un escalafón con ese nombre.")
            txtNombre.SelectAll()
            txtNombre.Focus()
            Return
        End If

        Cursor = Cursors.WaitCursor
        btnGuardar.Enabled = False

        Try
            Using svc As New EscalafonService()
                If idActual = 0 Then
                    Dim nuevo = New Escalafon With {.Nombre = nombre}
                    Dim nuevoId As Integer = 0
                    Try
                        nuevoId = Await svc.CreateAsync(nuevo)
                    Catch
                    End Try
                    _ultimoIdSeleccionado = If(nuevoId > 0, nuevoId, 0)
                    Notifier.Success(Me, "Escalafón creado correctamente.")
                Else
                    Dim entidad = Await svc.GetByIdAsync(idActual)
                    If entidad IsNot Nothing Then
                        entidad.Nombre = nombre
                        Await svc.UpdateAsync(entidad)
                    Else
                        Dim actualizado = New Escalafon With {.Id = idActual, .Nombre = nombre}
                        Await svc.UpdateAsync(actualizado)
                    End If
                    _ultimoIdSeleccionado = idActual
                    Notifier.Success(Me, "Escalafón actualizado correctamente.")
                End If
            End Using

            Await CargarDatosAsync()
            If _ultimoIdSeleccionado = 0 Then
                Dim fila = _listaCompleta.FirstOrDefault(Function(e) String.Equals(e.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                If fila IsNot Nothing Then
                    _ultimoIdSeleccionado = fila.Id
                End If
            End If
            SeleccionarFilaPorId(_ultimoIdSeleccionado)
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al guardar el escalafón: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _seleccionActual Is Nothing OrElse _seleccionActual.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar un escalafón para eliminar.")
            Return
        End If

        Dim confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea eliminar el escalafón '{_seleccionActual.Nombre}'?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion <> DialogResult.Yes Then Return

        Cursor = Cursors.WaitCursor
        btnEliminar.Enabled = False

        Try
            Using svc As New EscalafonService()
                Await svc.DeleteAsync(_seleccionActual.Id)
            End Using
            Notifier.Success(Me, "Escalafón eliminado.")
            _ultimoIdSeleccionado = 0
            Await CargarDatosAsync()
            LimpiarFormulario()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al eliminar el escalafón: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnEliminar.Enabled = True
        End Try
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub

    Private Sub frmEscalafones_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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

    Private Sub frmEscalafones_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
    End Sub
End Class
