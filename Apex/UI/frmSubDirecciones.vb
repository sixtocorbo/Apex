Imports System.ComponentModel
Imports System.Data.Entity
Imports System.Linq
Imports System.Threading.Tasks

Public Class frmSubDirecciones

    Private _listaCompleta As List(Of SubDireccion)
    Private ReadOnly _bindingSource As New BindingSource()
    Private _seleccionActual As SubDireccion
    Private _estaCargando As Boolean
    Private _ultimoIdSeleccionado As Integer

    Private Async Sub frmSubDirecciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.AcceptButton = btnGuardar
        Me.KeyPreview = True

        ConfigurarGrilla()
        Try
            AppTheme.SetCue(txtBuscar, "Buscar subdirecciones...")
            AppTheme.SetCue(txtNombre, "Nombre de la subdirección")
        Catch
        End Try

        Await CargarDatosAsync()
        PrepararEventos()
        LimpiarFormulario()
    End Sub

    Private Sub PrepararEventos()
        AddHandler Me.KeyDown, AddressOf frmSubDirecciones_KeyDown
        AddHandler dgvSubDirecciones.CellDoubleClick, AddressOf dgvSubDirecciones_CellDoubleClick
    End Sub

    Private Sub ConfigurarGrilla()
        dgvSubDirecciones.AutoGenerateColumns = False
        dgvSubDirecciones.Columns.Clear()

        dgvSubDirecciones.EnableHeadersVisualStyles = False
        dgvSubDirecciones.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgvSubDirecciones.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
        dgvSubDirecciones.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvSubDirecciones.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        dgvSubDirecciones.ColumnHeadersHeight = 40
        dgvSubDirecciones.DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
        dgvSubDirecciones.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
        dgvSubDirecciones.DefaultCellStyle.SelectionForeColor = Color.White
        dgvSubDirecciones.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)
        dgvSubDirecciones.BorderStyle = BorderStyle.None
        dgvSubDirecciones.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal

        dgvSubDirecciones.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Id",
            .Visible = False
        })

        dgvSubDirecciones.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre",
            .HeaderText = "Nombre",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
    End Sub

    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Cursor = Cursors.WaitCursor
        dgvSubDirecciones.Enabled = False

        Try
            Using svc As New SubDireccionService()
                Dim lista = Await svc.GetAllAsync()
                _listaCompleta = lista.
                    OrderBy(Function(sd) sd.Nombre).
                    ToList()
            End Using

            AplicarFiltro(txtBuscar.Text.Trim())
            SeleccionarFilaPorId(_ultimoIdSeleccionado)
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudieron cargar las subdirecciones: {ex.Message}")
            _listaCompleta = New List(Of SubDireccion)()
            _bindingSource.DataSource = _listaCompleta
            dgvSubDirecciones.DataSource = _bindingSource
        Finally
            dgvSubDirecciones.Enabled = True
            Cursor = Cursors.Default
            _estaCargando = False
        End Try
    End Function

    Private Sub AplicarFiltro(filtro As String)
        If _listaCompleta Is Nothing Then Return

        Dim listaFiltrada = _listaCompleta
        If Not String.IsNullOrWhiteSpace(filtro) Then
            listaFiltrada = _listaCompleta.
                Where(Function(sd) sd.Nombre IsNot Nothing AndAlso
                                   sd.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                OrderBy(Function(sd) sd.Nombre).
                ToList()
        Else
            listaFiltrada = _listaCompleta.OrderBy(Function(sd) sd.Nombre).ToList()
        End If

        _bindingSource.DataSource = listaFiltrada
        dgvSubDirecciones.DataSource = _bindingSource

        SeleccionarFilaPorId(_ultimoIdSeleccionado)
    End Sub

    Private Sub SeleccionarFilaPorId(id As Integer)
        If id <= 0 OrElse dgvSubDirecciones.Rows.Count = 0 Then
            dgvSubDirecciones.ClearSelection()
            _seleccionActual = Nothing
            Return
        End If

        For Each row As DataGridViewRow In dgvSubDirecciones.Rows
            Dim elemento = TryCast(row.DataBoundItem, SubDireccion)
            If elemento IsNot Nothing AndAlso elemento.Id = id Then
                row.Selected = True
                Dim firstVisible = dgvSubDirecciones.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
                If firstVisible IsNot Nothing Then
                    dgvSubDirecciones.CurrentCell = row.Cells(firstVisible.Index)
                ElseIf row.Cells.Count > 0 Then
                    dgvSubDirecciones.CurrentCell = row.Cells(0)
                End If
                dgvSubDirecciones.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index)
                _seleccionActual = elemento
                MostrarDetalle()
                Exit Sub
            End If
        Next

        dgvSubDirecciones.ClearSelection()
        _seleccionActual = Nothing
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
        dgvSubDirecciones.ClearSelection()
    End Sub

    Private Sub dgvSubDirecciones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvSubDirecciones.SelectionChanged
        If dgvSubDirecciones.CurrentRow Is Nothing Then Return
        _seleccionActual = TryCast(dgvSubDirecciones.CurrentRow.DataBoundItem, SubDireccion)
        If _seleccionActual IsNot Nothing Then
            _ultimoIdSeleccionado = _seleccionActual.Id
        End If
        MostrarDetalle()
    End Sub

    Private Sub dgvSubDirecciones_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex >= 0 Then
            txtNombre.Focus()
            txtNombre.SelectAll()
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        AplicarFiltro(txtBuscar.Text.Trim())
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim nombre = txtNombre.Text.Trim()
        If String.IsNullOrWhiteSpace(nombre) Then
            Notifier.Warn(Me, "Debes ingresar un nombre para la subdirección.")
            txtNombre.Focus()
            Return
        End If

        Try
            Using svc As New SubDireccionService()
                If _seleccionActual Is Nothing OrElse _seleccionActual.Id = 0 Then
                    Dim entidad = New SubDireccion With {
                        .Nombre = nombre
                    }
                    Dim nuevoId = Await svc.CreateAsync(entidad)
                    _ultimoIdSeleccionado = nuevoId
                    Notifier.Success(Me, "Subdirección creada correctamente.")
                Else
                    _seleccionActual.Nombre = nombre
                    Await svc.UpdateAsync(_seleccionActual)
                    _ultimoIdSeleccionado = _seleccionActual.Id
                    Notifier.Success(Me, "Subdirección actualizada correctamente.")
                End If
            End Using

            Await CargarDatosAsync()
            SeleccionarFilaPorId(_ultimoIdSeleccionado)
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudo guardar la subdirección: {ex.Message}")
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _seleccionActual Is Nothing OrElse _seleccionActual.Id = 0 Then Return

        Dim respuesta = MessageBox.Show(
            "¿Seguro que deseas eliminar la subdirección seleccionada?", "Confirmación",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If respuesta <> DialogResult.Yes Then Return

        Try
            Using svc As New SubDireccionService()
                Await svc.DeleteAsync(_seleccionActual.Id)
            End Using

            Notifier.Success(Me, "Subdirección eliminada correctamente.")
            _ultimoIdSeleccionado = 0
            Await CargarDatosAsync()
            LimpiarFormulario()
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudo eliminar la subdirección: {ex.Message}")
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarFormulario()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub

    Private Sub frmSubDirecciones_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
End Class
