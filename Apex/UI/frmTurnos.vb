Imports System.ComponentModel
Imports System.Linq
Imports System.Reflection
Imports System.Threading
Imports System.Threading.Tasks

Public Class frmTurnos

    Private _listaTurnos As BindingList(Of Turno)
    Private _turnoSeleccionado As Turno
    Private _estaCargando As Boolean = False
    Private _ultimoIdSeleccionado As Integer = 0
    Private _ctsBusqueda As CancellationTokenSource

    ' ===== Ciclo de vida =====
    Private Async Sub frmTurnos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.AcceptButton = btnGuardar
        Me.KeyPreview = True

        ConfigurarGrilla()

        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre...")
            AppTheme.SetCue(txtNombre, "Nombre del turno...")
        Catch
        End Try

        Await CargarDatosAsync()
        LimpiarCampos()
        Notifier.Info(Me, "Listado de turnos listo.")
    End Sub

    ' ===== Datos =====
    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Cursor = Cursors.WaitCursor
        dgvTurnos.Enabled = False

        Try
            Dim lista As List(Of Turno)
            Using svc As New TurnoService()
                lista = Await svc.GetAllAsync()
            End Using

            _listaTurnos = New BindingList(Of Turno)(lista)
            dgvTurnos.DataSource = _listaTurnos
            RestaurarSeleccion()
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudieron cargar los turnos: {ex.Message}")
            dgvTurnos.DataSource = New BindingList(Of Turno)()
        Finally
            Cursor = Cursors.Default
            dgvTurnos.Enabled = True
            _estaCargando = False
        End Try
    End Function

    ' ===== UI =====
    Private Sub ConfigurarGrilla()
        dgvTurnos.AutoGenerateColumns = False
        dgvTurnos.Columns.Clear()

        dgvTurnos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Id",
            .HeaderText = "Id",
            .Visible = False
        })

        dgvTurnos.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre",
            .HeaderText = "Nombre",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })

        ' Si quisieras mostrar fechas u otras columnas, agrégalas aquí
        HabilitarDoubleBuffering(dgvTurnos)
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
        _turnoSeleccionado = New Turno()
        txtNombre.Clear()
        btnEliminar.Enabled = False
        dgvTurnos.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub MostrarDetalles()
        If _turnoSeleccionado Is Nothing Then
            LimpiarCampos()
            Return
        End If

        txtNombre.Text = _turnoSeleccionado.Nombre
        btnEliminar.Enabled = (_turnoSeleccionado.Id <> 0)
    End Sub

    Private Sub RestaurarSeleccion()
        If _ultimoIdSeleccionado = 0 OrElse dgvTurnos.Rows.Count = 0 Then
            dgvTurnos.ClearSelection()
            Return
        End If

        For Each row As DataGridViewRow In dgvTurnos.Rows
            Dim t = TryCast(row.DataBoundItem, Turno)
            If t IsNot Nothing AndAlso t.Id = _ultimoIdSeleccionado Then
                row.Selected = True
                Dim firstVisibleCol = dgvTurnos.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
                If firstVisibleCol IsNot Nothing Then
                    dgvTurnos.CurrentCell = row.Cells(firstVisibleCol.Index)
                ElseIf row.Cells.Count > 0 Then
                    dgvTurnos.CurrentCell = row.Cells(0)
                End If
                dgvTurnos.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index)
                Exit For
            End If
        Next
    End Sub

    ' ===== Eventos =====
    Private Sub dgvTurnos_SelectionChanged(sender As Object, e As EventArgs) Handles dgvTurnos.SelectionChanged
        If dgvTurnos.CurrentRow Is Nothing OrElse dgvTurnos.CurrentRow.DataBoundItem Is Nothing Then Return
        _turnoSeleccionado = CType(dgvTurnos.CurrentRow.DataBoundItem, Turno)
        _ultimoIdSeleccionado = _turnoSeleccionado.Id
        MostrarDetalles()
    End Sub

    ' Búsqueda con debounce (250 ms)
    Private Async Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If _listaTurnos Is Nothing Then Return

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
            dgvTurnos.DataSource = _listaTurnos
        Else
            Dim resultado = _listaTurnos.
                Where(Function(t) t IsNot Nothing AndAlso t.Nombre IsNot Nothing AndAlso
                                  t.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                ToList()
            dgvTurnos.DataSource = New BindingList(Of Turno)(resultado)
        End If

        dgvTurnos.ClearSelection()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre del turno no puede estar vacío.")
            txtNombre.Focus()
            Return
        End If

        Dim nombre = txtNombre.Text.Trim()
        Dim idActual As Integer = If(_turnoSeleccionado Is Nothing, 0, _turnoSeleccionado.Id)

        ' Validación de duplicados por nombre (ignora mayúsculas/minúsculas y excluye el actual)
        Dim existeDuplicado = (_listaTurnos IsNot Nothing AndAlso
                               _listaTurnos.Any(Function(x) x IsNot Nothing AndAlso
                                                       x.Id <> idActual AndAlso
                                                       String.Equals(x.Nombre, nombre, StringComparison.OrdinalIgnoreCase)))
        If existeDuplicado Then
            Notifier.Warn(Me, "Ya existe un turno con ese nombre.")
            txtNombre.SelectAll()
            txtNombre.Focus()
            Return
        End If

        _turnoSeleccionado.Nombre = nombre

        Cursor = Cursors.WaitCursor
        btnGuardar.Enabled = False
        Try
            Using svc As New TurnoService()
                If idActual = 0 Then
                    ' Si CreateAsync devuelve el Id, úsalo; si no, continúa sin romper
                    Dim nuevoId As Integer = 0
                    Try
                        nuevoId = Await svc.CreateAsync(_turnoSeleccionado)
                    Catch
                        ' método sin retorno o distinta signatura
                    End Try
                    _ultimoIdSeleccionado = If(nuevoId > 0, nuevoId, 0)
                    Notifier.Success(Me, "Turno creado correctamente.")
                Else
                    Await svc.UpdateAsync(_turnoSeleccionado)
                    _ultimoIdSeleccionado = idActual
                    Notifier.Success(Me, "Turno actualizado correctamente.")
                End If
            End Using

            Await CargarDatosAsync()
            If _ultimoIdSeleccionado = 0 Then
                ' Si no se obtuvo Id de CreateAsync, intenta re-seleccionar por nombre
                Dim fila = _listaTurnos.FirstOrDefault(Function(t) String.Equals(t.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                If fila IsNot Nothing Then
                    _ultimoIdSeleccionado = fila.Id
                    RestaurarSeleccion()
                End If
            End If
            MostrarDetalles()
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al guardar el turno: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _turnoSeleccionado Is Nothing OrElse _turnoSeleccionado.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar un turno para eliminar.")
            Return
        End If

        Dim confirmacion = MessageBox.Show(
            $"¿Está seguro de que desea eliminar el turno '{_turnoSeleccionado.Nombre}'?",
            "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion <> DialogResult.Yes Then Return

        Cursor = Cursors.WaitCursor
        btnEliminar.Enabled = False
        Try
            Using svc As New TurnoService()
                Await svc.DeleteAsync(_turnoSeleccionado.Id)
            End Using
            Notifier.Info(Me, "Turno eliminado.")
            _ultimoIdSeleccionado = 0
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As Exception
            If EsViolacionFK(ex) Then
                Notifier.Warn(Me,
                    "No se puede eliminar porque existen registros relacionados " &
                    "(por ejemplo asignaciones u otros). " &
                    "Elimine o reasigne esas referencias y vuelva a intentar.")
            Else
                Notifier.Error(Me, "Ocurrió un error al eliminar: " & ex.Message)
            End If
        Finally
            Cursor = Cursors.Default
            btnEliminar.Enabled = True
        End Try


    End Sub
    ' --- Helpers seguros para leer propiedades por reflexión en VB ---
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

    ' --- Detección de violaciones de clave foránea multibase ---
    Private Function EsViolacionFK(ex As Exception) As Boolean
        Dim e As Exception = ex
        While e IsNot Nothing

            Dim typeName As String = If(e.GetType().FullName, String.Empty)

            ' SQL Server (System.Data.SqlClient / Microsoft.Data.SqlClient): Number = 547
            If typeName = "System.Data.SqlClient.SqlException" _
           OrElse typeName = "Microsoft.Data.SqlClient.SqlException" Then
                Dim n As Integer
                If GetPropInt(e, "Number", n) AndAlso n = 547 Then
                    Return True
                End If
            End If

            ' MySQL (MySql.Data / MySqlConnector): Number = 1451/1452
            If typeName.IndexOf("MySql", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Dim n As Integer
                If GetPropInt(e, "Number", n) AndAlso (n = 1451 OrElse n = 1452) Then
                    Return True
                End If
            End If

            ' PostgreSQL (Npgsql.PostgresException): SqlState = 23503
            If typeName.IndexOf("Postgres", StringComparison.OrdinalIgnoreCase) >= 0 _
           OrElse typeName.IndexOf("Npgsql", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Dim state As String = GetPropString(e, "SqlState")
                If String.Equals(state, "23503", StringComparison.OrdinalIgnoreCase) Then
                    Return True
                End If
            End If

            ' SQLite (System.Data.SQLite / Microsoft.Data.Sqlite): por mensaje
            If typeName.IndexOf("Sqlite", StringComparison.OrdinalIgnoreCase) >= 0 _
           OrElse typeName.IndexOf("SQLite", StringComparison.OrdinalIgnoreCase) >= 0 Then
                Dim msg As String = If(e.Message, String.Empty).ToUpperInvariant()
                If msg.Contains("FOREIGN KEY") OrElse msg.Contains("CONSTRAINT") Then
                    Return True
                End If
            End If

            ' Fallback por mensaje (por si la capa de servicio encapsula la excepción)
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


    ' ===== Atajos de teclado =====
    Private Sub frmTurnos_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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
        ' Si tienes helper de navegación, úsalo; si no, cerramos.
        ' NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmConfiguracion)(Me)
        Close()
    End Sub

End Class
