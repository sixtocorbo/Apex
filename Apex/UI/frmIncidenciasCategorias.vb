' Apex/UI/frmIncidenciasCategorias.vb
Imports System.ComponentModel
Imports System.Linq
Imports System.Threading.Tasks

Partial Class frmIncidenciasCategorias

    Private _categoriaService As New CategoriaAusenciaService()
    Private _listaCategorias As BindingList(Of CategoriaAusencia)
    Private _categoriaSeleccionada As CategoriaAusencia
    Private _estaCargando As Boolean
    ' --- Campo nuevo para bloquear el SelectionChanged mientras limpiamos ---
    Private _suspendiendoSeleccion As Boolean = False


    Private Async Sub frmIncidenciasCategorias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        AplicarLayoutModernoYResponsivo()
        ConfigurarGrilla()

        Try
            AppTheme.SetCue(txtBuscar, "Buscar por nombre…")
            AppTheme.SetCue(txtNombre, "Ingrese el nombre de la categoría…")
        Catch
        End Try

        ' Carga inicial (ahora correctamente awaited)
        Await CargarDatosAsync()
    End Sub

    ' =================== DATOS ===================
    Private Async Function CargarDatosAsync() As Task
        If _estaCargando Then Return
        _estaCargando = True

        Cursor = Cursors.WaitCursor
        dgvCategorias.Enabled = False
        Try
            Dim lista As List(Of CategoriaAusencia)
            Using svc As New CategoriaAusenciaService()
                lista = Await svc.GetAllAsync()
            End Using
            _listaCategorias = New BindingList(Of CategoriaAusencia)(lista)
            dgvCategorias.DataSource = _listaCategorias
            dgvCategorias.ClearSelection()
            LimpiarCampos()
            Notifier.Info(Me, "Listado de categorías listo.")
        Catch ex As Exception
            Notifier.[Error](Me, "No se pudieron cargar las categorías: " & ex.Message)
            dgvCategorias.DataSource = New BindingList(Of CategoriaAusencia)()
        Finally
            Cursor = Cursors.Default
            dgvCategorias.Enabled = True
            _estaCargando = False
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        dgvCategorias.AutoGenerateColumns = False
        dgvCategorias.Columns.Clear()

        dgvCategorias.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Id",
            .HeaderText = "Id",
            .Visible = False
        })
        dgvCategorias.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Nombre",
            .HeaderText = "Nombre",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })

        AplicarEstiloGrilla(dgvCategorias)
    End Sub

    ' =================== UI STATE ===================
    Private Sub LimpiarCampos()
        _suspendiendoSeleccion = True   ' << activar bloqueo

        _categoriaSeleccionada = New CategoriaAusencia()
        txtNombre.Clear()
        btnEliminar.Enabled = False

        ' OJO: ClearSelection no quita la CurrentCell. Hay que anularla también:
        dgvCategorias.ClearSelection()
        dgvCategorias.CurrentCell = Nothing

        _suspendiendoSeleccion = False  ' << desactivar bloqueo
        txtNombre.Focus()
    End Sub


    Private Sub MostrarDetalles()
        If _categoriaSeleccionada IsNot Nothing Then
            txtNombre.Text = _categoriaSeleccionada.Nombre
            btnEliminar.Enabled = (_categoriaSeleccionada.Id <> 0)
        Else
            LimpiarCampos()
        End If
    End Sub

    ' =================== EVENTOS GRID / BUSCAR ===================
    Private Sub dgvCategorias_SelectionChanged(sender As Object, e As EventArgs) Handles dgvCategorias.SelectionChanged
        ' Si estamos limpiando/creando, ignorar el rebote
        If _suspendiendoSeleccion Then Return

        ' Si no hay selección real, no hacer nada
        If dgvCategorias.CurrentRow Is Nothing OrElse dgvCategorias.CurrentCell Is Nothing Then Return

        Dim item = TryCast(dgvCategorias.CurrentRow.DataBoundItem, CategoriaAusencia)
        If item Is Nothing Then Return

        _categoriaSeleccionada = item
        MostrarDetalles()
    End Sub


    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If _listaCategorias Is Nothing Then Return
        Dim filtro = txtBuscar.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvCategorias.DataSource = _listaCategorias
        Else
            Dim resultado = _listaCategorias.
                Where(Function(c) c.Nombre IsNot Nothing AndAlso
                                  c.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0).
                ToList()
            dgvCategorias.DataSource = New BindingList(Of CategoriaAusencia)(resultado)
        End If
        dgvCategorias.ClearSelection()
    End Sub

    ' =================== BOTONES ===================
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()   ' con el bloqueo activo no se repoblarán los campos
    End Sub


    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            Notifier.Warn(Me, "El nombre de la categoría no puede estar vacío.")
            txtNombre.Focus()
            Return
        End If

        _categoriaSeleccionada.Nombre = txtNombre.Text.Trim()

        Cursor = Cursors.WaitCursor
        btnGuardar.Enabled = False
        Try
            Using svc As New CategoriaAusenciaService()
                If _categoriaSeleccionada.Id = 0 Then
                    ' Si tu entidad no tiene CreatedAt/UpdatedAt, no los seteamos
                    Await svc.CreateAsync(_categoriaSeleccionada)
                    Notifier.Success(Me, "Categoría creada correctamente.")
                Else
                    Await svc.UpdateAsync(_categoriaSeleccionada)
                    Notifier.Success(Me, "Categoría actualizada correctamente.")
                End If
            End Using
            Await CargarDatosAsync()
        Catch ex As Exception
            Notifier.[Error](Me, "Ocurrió un error al guardar: " & ex.Message)
        Finally
            Cursor = Cursors.Default
            btnGuardar.Enabled = True
        End Try
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _categoriaSeleccionada Is Nothing OrElse _categoriaSeleccionada.Id = 0 Then
            Notifier.Warn(Me, "Seleccione una categoría para eliminar.")
            Return
        End If

        If MessageBox.Show(
        $"¿Eliminar la categoría '{_categoriaSeleccionada.Nombre}'?",
        "Confirmar eliminación",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then Return

        Cursor = Cursors.WaitCursor : btnEliminar.Enabled = False
        Try
            Using svc As New CategoriaAusenciaService()
                Await svc.EliminarAsync(_categoriaSeleccionada.Id)
            End Using
            Notifier.Info(Me, "Categoría eliminada.")
            Await CargarDatosAsync()
            LimpiarCampos()
        Catch ex As InvalidOperationException
            ' Mensaje ya “traducido” en el service
            Notifier.Error(Me, ex.Message, 6000)
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al eliminar: " & ex.Message, 6000)
        Finally
            Cursor = Cursors.Default : btnEliminar.Enabled = True
        End Try
    End Sub


    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Close()
    End Sub

    ' =================== HELPERS ===================
    Private Function MapearErrorRelacion(ex As Exception) As String
        ' Detectar FK constraint (SQL Server u otros)
        Try
            Dim inner As Exception = ex
            While inner IsNot Nothing
                ' SQL Server: SqlException.Number = 547
                If inner.GetType().Name = "SqlException" Then
                    Dim prop = inner.GetType().GetProperty("Number")
                    If prop IsNot Nothing Then
                        Dim raw = prop.GetValue(inner, Nothing)
                        Dim n As Integer = -1
                        If raw IsNot Nothing Then
                            Try
                                n = Convert.ToInt32(raw)
                            Catch
                            End Try
                        End If
                        If n = 547 Then
                            Return "No se puede eliminar porque hay registros relacionados que dependen de esta categoría."
                        End If
                    End If
                End If

                ' Heurística por mensaje
                Dim m = inner.Message
                If m IsNot Nothing Then
                    Dim u = m.ToUpperInvariant()
                    If u.Contains("FOREIGN KEY") OrElse u.Contains("REFERENCE") OrElse u.Contains("CLAVE FORÁNEA") Then
                        Return "No se puede eliminar: la categoría está en uso por otros datos relacionados."
                    End If
                End If

                inner = inner.InnerException
            End While
        Catch
        End Try

        Return "Ocurrió un error al eliminar: " & ex.Message
    End Function

    Private Sub AplicarEstiloGrilla(dgv As DataGridView)
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.WhiteSmoke
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.5!, FontStyle.Bold)
        dgv.ColumnHeadersHeight = 36
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None

        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9.0!, FontStyle.Regular)
        dgv.DefaultCellStyle.SelectionBackColor = Color.Gainsboro
        dgv.DefaultCellStyle.SelectionForeColor = Color.Black
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248)
        dgv.RowTemplate.Height = 28

        ' Doble buffer por reflexión
        Try
            Dim pi = GetType(DataGridView).GetProperty("DoubleBuffered",
                Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic)
            If pi IsNot Nothing Then pi.SetValue(dgv, True, Nothing)
        Catch
        End Try
    End Sub

    ' =================== LAYOUT ===================
    Private Sub AplicarLayoutModernoYResponsivo()
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.MinimumSize = New Size(900, 580)

        With SplitContainer1
            .Dock = DockStyle.Fill
            .IsSplitterFixed = False
            .FixedPanel = FixedPanel.None
            .Panel1MinSize = 320
            .Panel2MinSize = 360
            AjustarSplitterProporcional()
            AddHandler Me.Resize, AddressOf OnFormResize_AjustarSplitter
        End With

        ' Reconstruir Panel Izquierdo: header (Label + TextBox) + grid
        Dim header As New Panel With {.Dock = DockStyle.Top, .Padding = New Padding(12), .Height = 56}
        Dim tlp As New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2}
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tlp.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))

        Label8.AutoSize = True
        Label8.Margin = New Padding(0, 2, 10, 0)
        Label8.Text = "Buscar"

        txtBuscar.BorderStyle = BorderStyle.FixedSingle
        txtBuscar.Margin = New Padding(0)
        txtBuscar.Dock = DockStyle.Fill

        tlp.Controls.Add(Label8, 0, 0)
        tlp.Controls.Add(txtBuscar, 1, 0)
        header.Controls.Add(tlp)

        SplitContainer1.Panel1.Controls.Clear()
        SplitContainer1.Panel1.Controls.Add(dgvCategorias)
        SplitContainer1.Panel1.Controls.Add(header)

        ' Panel Derecho: barra de acciones + GroupBox
        Dim flp As New FlowLayoutPanel With {
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .FlowDirection = FlowDirection.LeftToRight,
            .Padding = New Padding(12),
            .WrapContents = True
        }
        PrepararBotonModerno(btnNuevo)
        PrepararBotonModerno(btnGuardar, True)
        PrepararBotonModerno(btnEliminar)
        flp.Controls.AddRange(New Control() {btnNuevo, btnGuardar, btnEliminar})

        GroupBox1.Text = "Detalles"
        GroupBox1.Dock = DockStyle.Fill
        GroupBox1.Padding = New Padding(12)

        Dim tlpForm As New TableLayoutPanel With {.Dock = DockStyle.Top, .AutoSize = True, .ColumnCount = 2}
        tlpForm.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        tlpForm.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
        tlpForm.RowStyles.Add(New RowStyle(SizeType.AutoSize))

        Label1.Margin = New Padding(0, 6, 10, 6)
        Label1.AutoSize = True
        Label1.Text = "Nombre"

        txtNombre.BorderStyle = BorderStyle.FixedSingle
        txtNombre.Dock = DockStyle.Fill
        txtNombre.Margin = New Padding(0, 3, 0, 3)

        GroupBox1.Controls.Clear()
        tlpForm.Controls.Add(Label1, 0, 0)
        tlpForm.Controls.Add(txtNombre, 1, 0)
        GroupBox1.Controls.Add(tlpForm)

        SplitContainer1.Panel2.Controls.Clear()
        SplitContainer1.Panel2.Controls.Add(GroupBox1)
        SplitContainer1.Panel2.Controls.Add(flp)
    End Sub

    Private Sub AjustarSplitterProporcional()
        Dim ancho As Integer = Me.ClientSize.Width
        If ancho <= 0 Then Return
        Dim deseado As Integer = CInt(ancho * 0.36) ' ~36% izquierda
        SplitContainer1.SplitterDistance =
            Math.Max(SplitContainer1.Panel1MinSize,
                     Math.Min(deseado, ancho - SplitContainer1.Panel2MinSize))
    End Sub

    Private Sub OnFormResize_AjustarSplitter(sender As Object, e As EventArgs)
        AjustarSplitterProporcional()
    End Sub

    Private Sub PrepararBotonModerno(b As Button, Optional bold As Boolean = False)
        b.AutoSize = True
        b.AutoSizeMode = AutoSizeMode.GrowAndShrink
        b.FlatStyle = FlatStyle.Flat
        b.FlatAppearance.BorderColor = Color.Silver
        b.FlatAppearance.MouseOverBackColor = Color.Gainsboro
        b.Padding = New Padding(14, 8, 14, 8)
        b.Margin = New Padding(0, 0, 12, 8)
        b.Font = New Font("Segoe UI", If(bold, 9.5!, 9.0!), If(bold, FontStyle.Bold, FontStyle.Regular))
    End Sub

End Class
