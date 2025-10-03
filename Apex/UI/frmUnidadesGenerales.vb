Imports System.ComponentModel
Imports System.Data.Entity
Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmUnidadesGenerales

    Private _listaCompleta As List(Of UnidadGeneral)
    Private ReadOnly _bindingSource As New BindingSource()
    Private _seleccionActual As UnidadGeneral
    Private _estaCargando As Boolean
    Private _ultimoIdSeleccionado As Integer

    Private Async Sub frmUnidadesGenerales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.AcceptButton = btnGuardar
        Me.KeyPreview = True

        ConfigurarGrilla()
        Try
            AppTheme.SetCue(txtBuscar, "Buscar unidades generales...")
            AppTheme.SetCue(txtNombre, "Nombre de la unidad general")
        Catch
        End Try

        Await CargarDatosAsync()
        PrepararEventos()
        LimpiarFormulario()
    End Sub

    Private Sub PrepararEventos()
        AddHandler Me.KeyDown, AddressOf frmUnidadesGenerales_KeyDown
        AddHandler dgvUnidadesGenerales.CellDoubleClick, AddressOf dgvUnidadesGenerales_CellDoubleClick
    End Sub

    Private Sub ConfigurarGrilla()
        dgvUnidadesGenerales.AutoGenerateColumns = False
        dgvUnidadesGenerales.Columns.Clear()

        dgvUnidadesGenerales.EnableHeadersVisualStyles = False
        dgvUnidadesGenerales.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgvUnidadesGenerales.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
        dgvUnidadesGenerales.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvUnidadesGenerales.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        dgvUnidadesGenerales.ColumnHeadersHeight = 40
        dgvUnidadesGenerales.DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
        dgvUnidadesGenerales.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
        dgvUnidadesGenerales.DefaultCellStyle.SelectionForeColor = Color.White
        dgvUnidadesGenerales.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)
        dgvUnidadesGenerales.BorderStyle = BorderStyle.None
        dgvUnidadesGenerales.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal

        dgvUnidadesGenerales.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = NameOf(UnidadGeneral.Id),
            .Visible = False
        })

        dgvUnidadesGenerales.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = NameOf(UnidadGeneral.Nombre),
            .HeaderText = "Nombre",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
    End Sub

    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Cursor = Cursors.WaitCursor
        dgvUnidadesGenerales.Enabled = False

        Try
            Using svc As New UnidadGeneralService()
                Dim lista = Await svc.ObtenerUnidadesOrdenadasAsync()
                _listaCompleta = lista.ToList()
            End Using

            AplicarFiltro(txtBuscar.Text.Trim())
            SeleccionarFilaPorId(_ultimoIdSeleccionado)
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudieron cargar las unidades generales: {ex.Message}")
            _listaCompleta = New List(Of UnidadGeneral)()
            _bindingSource.DataSource = _listaCompleta
            dgvUnidadesGenerales.DataSource = _bindingSource
        Finally
            dgvUnidadesGenerales.Enabled = True
            Cursor = Cursors.Default
            _estaCargando = False
        End Try
    End Function

    Private Sub AplicarFiltro(filtro As String)
        If _listaCompleta Is Nothing Then Return

        Dim listaFiltrada = _listaCompleta
        If Not String.IsNullOrWhiteSpace(filtro) Then
            listaFiltrada = _listaCompleta.
                Where(Function(u) u.Nombre IsNot Nothing AndAlso
                                   u.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                OrderBy(Function(u) u.Nombre).
                ToList()
        Else
            listaFiltrada = _listaCompleta.OrderBy(Function(u) u.Nombre).ToList()
        End If

        _bindingSource.DataSource = listaFiltrada
        dgvUnidadesGenerales.DataSource = _bindingSource

        SeleccionarFilaPorId(_ultimoIdSeleccionado)
    End Sub

    Private Sub SeleccionarFilaPorId(id As Integer)
        If id <= 0 OrElse dgvUnidadesGenerales.Rows.Count = 0 Then
            dgvUnidadesGenerales.ClearSelection()
            _seleccionActual = Nothing
            Return
        End If

        For Each row As DataGridViewRow In dgvUnidadesGenerales.Rows
            Dim elemento = TryCast(row.DataBoundItem, UnidadGeneral)
            If elemento IsNot Nothing AndAlso elemento.Id = id Then
                row.Selected = True
                Dim firstVisible = dgvUnidadesGenerales.Columns.GetFirstColumn(DataGridViewElementStates.Visible)
                If firstVisible IsNot Nothing Then
                    dgvUnidadesGenerales.CurrentCell = row.Cells(firstVisible.Index)
                ElseIf row.Cells.Count > 0 Then
                    dgvUnidadesGenerales.CurrentCell = row.Cells(0)
                End If
                dgvUnidadesGenerales.FirstDisplayedScrollingRowIndex = Math.Max(0, row.Index)
                _seleccionActual = elemento
                MostrarDetalle()
                Exit Sub
            End If
        Next

        dgvUnidadesGenerales.ClearSelection()
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
        dgvUnidadesGenerales.ClearSelection()
    End Sub

    Private Sub dgvUnidadesGenerales_SelectionChanged(sender As Object, e As EventArgs) Handles dgvUnidadesGenerales.SelectionChanged
        If dgvUnidadesGenerales.CurrentRow Is Nothing Then Return
        _seleccionActual = TryCast(dgvUnidadesGenerales.CurrentRow.DataBoundItem, UnidadGeneral)
        If _seleccionActual IsNot Nothing Then
            _ultimoIdSeleccionado = _seleccionActual.Id
        End If
        MostrarDetalle()
    End Sub

    Private Sub dgvUnidadesGenerales_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
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
            Notifier.Warn(Me, "Debes ingresar un nombre para la unidad general.")
            txtNombre.Focus()
            Return
        End If

        Try
            Using svc As New UnidadGeneralService()
                If _seleccionActual Is Nothing OrElse _seleccionActual.Id = 0 Then
                    Dim entidad = New UnidadGeneral With {
                        .Nombre = nombre,
                        .EsActivo = True,
                        .FechaCreacion = DateTime.Now,
                        .UsuarioCreacion = ObtenerNombreUsuario()
                    }
                    Dim nuevoId = Await svc.CreateAsync(entidad)
                    _ultimoIdSeleccionado = nuevoId
                    Notifier.Success(Me, "Unidad general creada correctamente.")
                Else
                    _seleccionActual.Nombre = nombre
                    _seleccionActual.FechaActualizacion = DateTime.Now
                    _seleccionActual.UsuarioActualizacion = ObtenerNombreUsuario()
                    Await svc.UpdateAsync(_seleccionActual)
                    _ultimoIdSeleccionado = _seleccionActual.Id
                    Notifier.Success(Me, "Unidad general actualizada correctamente.")
                End If
            End Using

            Await CargarDatosAsync()
            SeleccionarFilaPorId(_ultimoIdSeleccionado)
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudo guardar la unidad general: {ex.Message}")
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _seleccionActual Is Nothing OrElse _seleccionActual.Id = 0 Then Return

        Dim respuesta = MessageBox.Show(
            "¿Seguro que deseas eliminar la unidad general seleccionada?", "Confirmación",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If respuesta <> DialogResult.Yes Then Return

        Try
            Using svc As New UnidadGeneralService()
                Await svc.DeleteAsync(_seleccionActual.Id)
            End Using

            Notifier.Success(Me, "Unidad general eliminada correctamente.")
            _ultimoIdSeleccionado = 0
            Await CargarDatosAsync()
            LimpiarFormulario()
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudo eliminar la unidad general: {ex.Message}")
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarFormulario()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub

    Private Sub frmUnidadesGenerales_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub

    Private Function ObtenerNombreUsuario() As String
        Try
            Dim usuario = Environment.UserName
            If Not String.IsNullOrWhiteSpace(usuario) Then
                Return usuario
            End If
        Catch
        End Try
        Return "Sistema"
    End Function
End Class
