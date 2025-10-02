Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Reflection
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmFunciones

    Private _listaFunciones As BindingList(Of Funcion)
    Private _funcionSeleccionada As Funcion
    Private _estaCargando As Boolean
    Private _ultimoIdSeleccionado As Integer
    Private _ctsBusqueda As CancellationTokenSource

    Private Async Sub frmFunciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.AcceptButton = btnGuardar
        Me.KeyPreview = True

        ConfigurarGrilla()

        Try
            AppTheme.SetCue(txtBuscar, "Buscar funciones...")
            AppTheme.SetCue(txtNombre, "Nombre de la función...")
        Catch
        End Try

        Await CargarDatosAsync()
        LimpiarCampos()
        Notifier.Info(Me, "Listado de funciones listo.")
    End Sub

    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Cursor = Cursors.WaitCursor
        dgvFunciones.Enabled = False

        Try
            Dim lista As List(Of Funcion)
            Using svc As New FuncionService()
                lista = Await svc.GetAllAsync()
            End Using

            _listaFunciones = New BindingList(Of Funcion)(lista)
            dgvFunciones.DataSource = _listaFunciones
            RestaurarSeleccion()
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudieron cargar las funciones: {ex.Message}")
            dgvFunciones.DataSource = New BindingList(Of Funcion)()
        Finally
            Cursor = Cursors.Default
            dgvFunciones.Enabled = True
            _estaCargando = False
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        With dgvFunciones
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

            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

            .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)
        End With

        dgvFunciones.Columns.Clear()

        dgvFunciones.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = NameOf(Funcion.Id),
            .Visible = False
        })

        dgvFunciones.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = NameOf(Funcion.Nombre),
            .HeaderText = "Nombre de la Función",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })

        HabilitarDoubleBuffering(dgvFunciones)
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
        _funcionSeleccionada = New Funcion()
        txtNombre.Clear()
        btnEliminar.Enabled = False
        dgvFunciones.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _funcionSeleccionada Is Nothing Then
            LimpiarCampos()
            Return
        End If

        txtNombre.Text = _funcionSeleccionada.Nombre
        btnEliminar.Enabled = (_funcionSeleccionada.Id <> 0)
    End Sub

    Private Sub RestaurarSeleccion()
        If _ultimoIdSeleccionado = 0 OrElse dgvFunciones.Rows.Count = 0 Then
            dgvFunciones.ClearSelection()
            Return
        End If

        For Each row As DataGridViewRow In dgvFunciones.Rows
            Dim f = TryCast(row.DataBoundItem, Funcion)
            If f IsNot Nothing AndAlso f.Id = _ultimoIdSeleccionado Then
                row.Selected = True
                Dim firstVisibleCol = dgvFunciones.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
                If firstVisibleCol IsNot Nothing Then
                    dgvFunciones.CurrentCell = row.Cells(firstVisibleCol.Index)
                ElseIf row.Cells.Count > 0 Then
                    dgvFunciones.CurrentCell = row.Cells(0)
                End If
                dgvFunciones.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index)
                Exit For
            End If
        Next
    End Sub

    Private Sub dgvFunciones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvFunciones.SelectionChanged
        If dgvFunciones.CurrentRow Is Nothing OrElse dgvFunciones.CurrentRow.DataBoundItem Is Nothing Then Return
        _funcionSeleccionada = CType(dgvFunciones.CurrentRow.DataBoundItem, Funcion)
        _ultimoIdSeleccionado = _funcionSeleccionada.Id
        MostrarDetalles()
    End Sub

    Private Async Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If _listaFunciones Is Nothing Then Return

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
            dgvFunciones.DataSource = _listaFunciones
        Else
            Dim resultado = _listaFunciones.
                Where(Function(func) func IsNot Nothing AndAlso func.Nombre IsNot Nothing AndAlso
                                      func.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                ToList()
            dgvFunciones.DataSource = New BindingList(Of Funcion)(resultado)
        End If

        dgvFunciones.ClearSelection()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre de la función no puede estar vacío.")
            txtNombre.Focus()
            Return
        End If

        Dim nombre = txtNombre.Text.Trim()
        Dim idActual As Integer = If(_funcionSeleccionada Is Nothing, 0, _funcionSeleccionada.Id)

        Dim existeDuplicado = (_listaFunciones IsNot Nothing AndAlso
                               _listaFunciones.Any(Function(x) x IsNot Nothing AndAlso
                                                           x.Id <> idActual AndAlso
                                                           String.Equals(x.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
        If existeDuplicado Then
            Notifier.Warn(Me, "Ya existe una función con ese nombre.")
            txtNombre.SelectAll()
            txtNombre.Focus()
            Return
        End If

        _funcionSeleccionada.Nombre = nombre

        Cursor = Cursors.WaitCursor
        btnGuardar.Enabled = False
        Try
            Using svc As New FuncionService()
                If idActual = 0 Then
                    Dim nuevoId As Integer = 0
                    Try
                        nuevoId = Await svc.CreateAsync(_funcionSeleccionada)
                    Catch
                    End Try
                    _ultimoIdSeleccionado = If(nuevoId > 0, nuevoId, 0)
                    Notifier.Success(Me, "Función creada correctamente.")
                Else
                    Await svc.UpdateAsync(_funcionSeleccionada)
                    _ultimoIdSeleccionado = idActual
                    Notifier.Success(Me, "Función actualizada correctamente.")
                End If
            End Using

            Await CargarDatosAsync()
            If _ultimoIdSeleccionado = 0 Then
                Dim fila = _listaFunciones.FirstOrDefault(Function(func) String.Equals(func.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                If fila IsNot Nothing Then
                    _ultimoIdSeleccionado = fila.Id
                    RestaurarSeleccion()
                End If
            End If
            MostrarDetalles()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al guardar la función: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _funcionSeleccionada Is Nothing OrElse _funcionSeleccionada.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar una función para eliminar.")
            Return
        End If

        Dim confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea eliminar la función '{_funcionSeleccionada.Nombre}'?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion <> DialogResult.Yes Then Return

        Cursor = Cursors.WaitCursor
        btnEliminar.Enabled = False
        Try
            Using svc As New FuncionService()
                Await svc.DeleteAsync(_funcionSeleccionada.Id)
            End Using
            Notifier.Success(Me, "Función eliminada.")
            _ultimoIdSeleccionado = 0
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            If EsViolacionFK(ex) Then
                Notifier.Warn(Me,
                    "No se puede eliminar porque existen registros relacionados (por ejemplo funcionarios asignados). " &
                    "Elimine o reasigne esas referencias y vuelva a intentar.")
            Else
                Notifier.Error(Me, "Ocurrió un error al eliminar: " & ex.Message)
            End If
        Finally
            Cursor = Cursors.Default
            btnEliminar.Enabled = True
        End Try
    End Sub

    Private Function GetPropValue(obj As Object, propName As String) As Object
        If obj Is Nothing Then Return Nothing
        Try
            Dim p = obj.GetType().GetProperty(propName)
            If p Is Nothing Then Return Nothing
            Return p.GetValue(obj, Nothing)
        Catch
            Return Nothing
        End Try
    End Function

    Private Function GetPropInt(obj As Object, propName As String, ByRef valueOut As Integer) As Boolean
        valueOut = 0
        Dim v As Object = GetPropValue(obj, propName)
        If v Is Nothing Then Return False
        Dim s As String = Convert.ToString(v)
        Return Integer.TryParse(s, valueOut)
    End Function

    Private Function GetPropString(obj As Object, propName As String) As String
        Dim v As Object = GetPropValue(obj, propName)
        If v Is Nothing Then Return Nothing
        Return Convert.ToString(v)
    End Function

    Private Function EsViolacionFK(ex As Exception) As Boolean
        Dim e As Exception = ex
        While e IsNot Nothing

            Dim typeName As String = If(e.GetType().FullName, String.Empty)

            If typeName = "System.Data.SqlClient.SqlException" _
           OrElse typeName = "Microsoft.Data.SqlClient.SqlException" Then
                Dim n As Integer
                If GetPropInt(e, "Number", n) AndAlso n = 547 Then
                    Return True
                End If
            End If

            If typeName.IndexOf("MySql", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Dim n As Integer
                If GetPropInt(e, "Number", n) AndAlso (n = 1451 OrElse n = 1452) Then
                    Return True
                End If
            End If

            If typeName.IndexOf("Postgres", StringComparison.OrdinalIgnoreCase) >= 0 _
           OrElse typeName.IndexOf("Npgsql", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Dim state As String = GetPropString(e, "SqlState")
                If String.Equals(state, "23503", StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            End If

            If typeName.IndexOf("Sqlite", StringComparison.OrdinalIgnoreCase) >= 0 _
           OrElse typeName.IndexOf("SQLite", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Dim msg As String = If(e.Message, String.Empty).ToUpperInvariant()
                If msg.Contains("FOREIGN KEY") OrElse msg.Contains("CONSTRAINT") Then
                    Return True
                End If
            End If

            Dim texto As String = If(e.Message, String.Empty).ToUpperInvariant()
            If texto.Contains("FOREIGN KEY") _
           OrElse texto.Contains("REFERENTIAL INTEGRITY") _
           OrElse texto.Contains("REFERENCE CONSTRAINT") Then
                Return True
            End If

            e = e.InnerException
        End While

        Return False
    End Function

    Private Sub frmFunciones_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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
        Close()
    End Sub

End Class
