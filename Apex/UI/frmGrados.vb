Imports System.ComponentModel
Imports System.Linq
Imports System.Threading

Public Class frmGrados

    ' ------------------ CAMPOS ------------------
    Private _listaCargos As BindingList(Of Cargo)
    Private _cargoSeleccionado As Cargo
    Private _estaCargando As Boolean = False

    ' UX helpers
    Private _buscarTimer As System.Windows.Forms.Timer
    Private _ultimoIdSeleccionado As Integer = 0

    ' Placeholder del TextBox (cue banner)
    <System.Runtime.InteropServices.DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Unicode)>
    Private Shared Function SendMessage(hWnd As IntPtr, msg As Integer, wParam As IntPtr, lParam As String) As IntPtr
    End Function
    Private Const EM_SETCUEBANNER As Integer = &H1501
    Private Sub EstablecerPlaceholder(tb As TextBox, texto As String)
        If tb Is Nothing Then Exit Sub
        If tb.IsHandleCreated Then
            SendMessage(tb.Handle, EM_SETCUEBANNER, CType(1, IntPtr), texto)
        Else
            AddHandler tb.HandleCreated, Sub() SendMessage(tb.Handle, EM_SETCUEBANNER, CType(1, IntPtr), texto)
        End If
    End Sub

    ' ------------------ LOAD ------------------
    Private Async Sub frmGrados_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        dgvCargos.ActivarDobleBuffer(True)
        ConfigurarGrilla()

        ' Placeholder y debounce
        EstablecerPlaceholder(txtBuscar, "ESCRIBE PARA FILTRAR…")
        _buscarTimer = New System.Windows.Forms.Timer() With {.Interval = 200}
        AddHandler _buscarTimer.Tick, Sub()
                                          _buscarTimer.Stop()
                                          AplicarFiltro(txtBuscar.Text.Trim())
                                      End Sub

        ' Validación de entrada
        AddHandler txtGrado.KeyPress, AddressOf SoloNumeros_KeyPress

        ' Doble click para editar
        AddHandler dgvCargos.CellDoubleClick, AddressOf dgvCargos_CellDoubleClick

        ' Atajos
        AddHandler Me.KeyDown, AddressOf Frm_KeyDown

        Await CargarDatosAsync()
        LimpiarCampos()
        Notifier.Info(Me, "Listado de cargos listo.")
    End Sub

    ' ------------------ CARGA DE DATOS ------------------
    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Me.Cursor = Cursors.WaitCursor
        dgvCargos.Enabled = False
        RecordarSeleccion()

        Try
            Dim lista As List(Of Cargo)
            Using svc As New CargoService()
                lista = Await svc.GetAllAsync()
            End Using
            _listaCargos = New BindingList(Of Cargo)(lista)
            dgvCargos.DataSource = _listaCargos
            dgvCargos.ClearSelection()
            RestaurarSeleccion()
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al cargar los datos: {ex.Message}")
            dgvCargos.DataSource = New BindingList(Of Cargo)()
        Finally
            Me.Cursor = Cursors.Default
            dgvCargos.Enabled = True
            _estaCargando = False
        End Try
    End Function

    Private Sub RecordarSeleccion()
        If dgvCargos.CurrentRow IsNot Nothing AndAlso dgvCargos.CurrentRow.DataBoundItem IsNot Nothing Then
            Dim c = CType(dgvCargos.CurrentRow.DataBoundItem, Cargo)
            _ultimoIdSeleccionado = c.Id
        Else
            _ultimoIdSeleccionado = 0
        End If
    End Sub

    Private Sub RestaurarSeleccion()
        If _ultimoIdSeleccionado = 0 OrElse dgvCargos.Rows.Count = 0 Then
            dgvCargos.ClearSelection()
            Exit Sub
        End If

        For Each row As DataGridViewRow In dgvCargos.Rows
            Dim c = TryCast(row.DataBoundItem, Cargo)
            If c IsNot Nothing AndAlso c.Id = _ultimoIdSeleccionado Then
                row.Selected = True

                ' Primera columna visible del DGV (no de las celdas)
                Dim firstVisibleCol = dgvCargos.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
                If firstVisibleCol IsNot Nothing Then
                    dgvCargos.CurrentCell = row.Cells(firstVisibleCol.Index)
                Else
                    ' Fallback: primera celda de la fila si no hay columnas visibles (caso raro)
                    If row.Cells.Count > 0 Then dgvCargos.CurrentCell = row.Cells(0)
                End If

                ' Opcional: desplazar scroll para asegurarse de que se vea
                dgvCargos.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index)

                Exit For
            End If
        Next
    End Sub


    ' ------------------ UI ------------------
    Private Sub ConfigurarGrilla()
        With dgvCargos
            ' --- CONFIGURACIÓN GENERAL (Unificamos con el estilo moderno) ---
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

            ' --- DEFINICIÓN DE COLUMNAS (Mantenemos las tuyas con mejoras) ---
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .HeaderText = "Id", .Visible = False})

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre", .HeaderText = "Nombre",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .MinimumWidth = 200
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Grado", .HeaderText = "Grado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, ' Más flexible que un ancho fijo
            .MinimumWidth = 90,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter} ' Centrado
        })
        End With
    End Sub

    Private Sub LimpiarCampos()
        _cargoSeleccionado = New Cargo()
        txtNombre.Clear()
        txtGrado.Clear()
        btnEliminar.Enabled = False
        dgvCargos.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _cargoSeleccionado IsNot Nothing Then
            txtNombre.Text = _cargoSeleccionado.Nombre
            txtGrado.Text = _cargoSeleccionado.Grado?.ToString()
            btnEliminar.Enabled = True
        Else
            LimpiarCampos()
        End If
    End Sub

    ' ------------------ EVENTOS CONTROLES ------------------
    Private Sub dgvCargos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCargos.SelectionChanged
        If dgvCargos.CurrentRow IsNot Nothing AndAlso dgvCargos.CurrentRow.DataBoundItem IsNot Nothing Then
            _cargoSeleccionado = CType(dgvCargos.CurrentRow.DataBoundItem, Cargo)
            MostrarDetalles()
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If _buscarTimer Is Nothing Then Return
        _buscarTimer.Stop()
        _buscarTimer.Start()
    End Sub

    Private Sub AplicarFiltro(filtro As String)
        If _listaCargos Is Nothing Then Return
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvCargos.DataSource = _listaCargos
        Else
            Dim resultado = _listaCargos.
                Where(Function(c) c.Nombre?.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                ToList()
            dgvCargos.DataSource = New BindingList(Of Cargo)(resultado)
        End If
        dgvCargos.ClearSelection()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre del cargo no puede estar vacío.")
            Return
        End If

        ' Normaliza espacios
        _cargoSeleccionado.Nombre = System.Text.RegularExpressions.Regex.Replace(
            txtNombre.Text.Trim(), "\s+", " ")

        Dim gradoTemp As Integer
        _cargoSeleccionado.Grado = If(Integer.TryParse(txtGrado.Text.Trim(), gradoTemp), gradoTemp, CType(Nothing, Integer?))

        Me.Cursor = Cursors.WaitCursor
        btnGuardar.Enabled = False
        Try
            Using svc As New CargoService()
                If _cargoSeleccionado.Id = 0 Then
                    _cargoSeleccionado.CreatedAt = DateTime.Now
                    Await svc.CreateAsync(_cargoSeleccionado)
                    Notifier.Success(Me, "Cargo creado correctamente.")
                Else
                    _cargoSeleccionado.UpdatedAt = DateTime.Now
                    Await svc.UpdateAsync(_cargoSeleccionado)
                    Notifier.Success(Me, "Cargo actualizado correctamente.")
                End If
            End Using
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al guardar el cargo: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _cargoSeleccionado Is Nothing OrElse _cargoSeleccionado.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar un cargo para eliminar.")
            Return
        End If
        Dim confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea eliminar el cargo '{_cargoSeleccionado.Nombre}'?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmacion <> DialogResult.Yes Then Return

        Me.Cursor = Cursors.WaitCursor
        btnEliminar.Enabled = False
        Try
            Using svc As New CargoService()
                Await svc.DeleteAsync(_cargoSeleccionado.Id)
            End Using
            Notifier.Info(Me, "Cargo eliminado.")
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al eliminar el cargo: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
            btnEliminar.Enabled = True
        End Try
    End Sub

    ' ------------------ UX EXTRA ------------------
    Private Sub SoloNumeros_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Char.IsControl(e.KeyChar) Then Return
        If Not Char.IsDigit(e.KeyChar) Then e.Handled = True
    End Sub

    Private Sub dgvCargos_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        txtNombre.Focus()
        txtNombre.SelectAll()
    End Sub

    Private Sub Frm_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Close()
            e.Handled = True
        ElseIf e.Control AndAlso e.KeyCode = Keys.N Then
            btnNuevo.PerformClick() : e.Handled = True
        ElseIf e.Control AndAlso e.KeyCode = Keys.S Then
            btnGuardar.PerformClick() : e.Handled = True
        ElseIf e.KeyCode = Keys.Delete AndAlso dgvCargos.Focused Then
            If btnEliminar.Enabled Then btnEliminar.PerformClick()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Enter AndAlso (txtNombre.Focused OrElse txtGrado.Focused) Then
            btnGuardar.PerformClick() : e.Handled = True
        End If
    End Sub

End Class
