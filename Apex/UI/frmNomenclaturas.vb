Imports System.Data.Entity

Public Class frmNomenclaturas
    Private _unitOfWork As New UnitOfWork()
    Private _listaNomenclaturas As List(Of Nomenclatura)
    Private _nomenclaturaSeleccionada As Nomenclatura
    Public Sub New()
        InitializeComponent()
        ConfigurarLayoutResponsivoNomenclaturas()
    End Sub
    ' —o al final de InitializeComponent():
    ' ConfigurarLayoutResponsivoNomenclaturas()

    Private Async Sub frmGestionNomenclaturas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.WaitCursor
        Try
            AppTheme.Aplicar(Me)
            Await CargarDatosAsync()
            ConfigurarGrilla()
            LimpiarCampos()

            Try
                AppTheme.SetCue(txtBuscar, "Buscar por Nombre, Código o Área Responsable...")
                AppTheme.SetCue(txtNombre, "Nombre de la Nomenclatura")
                AppTheme.SetCue(txtCodigo, "Código de la Nomenclatura")
                AppTheme.SetCue(txtArea, "Área Responsable")
                AppTheme.SetCue(txtPatron, "Patrón (ej: DOC-YYYY-MM-DD-XXX)")
                AppTheme.SetCue(txtEjemplo, "Ejemplo (ej: DOC-2023-10-01-001)")
                AppTheme.SetCue(txtUbicacion, "Ubicación (ej: C:\Documentos\)")
                AppTheme.SetCue(txtObservaciones, "Observaciones")
            Catch
                ' Ignorar si SetCue no está disponible
            End Try

            Notifier.Info(Me, "Listado de nomenclaturas listo.")
        Catch ex As Exception
            Notifier.[Error](Me, $"Error al inicializar: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    ' Llamar después de InitializeComponent()
    Private Sub ConfigurarLayoutResponsivoNomenclaturas()
        ' ===== 1) TableLayout raíz =====
        With TableLayoutPanel1
            .Dock = DockStyle.Fill
            .ColumnCount = 1
            .ColumnStyles.Clear()
            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
            .RowStyles.Clear()
            ' Fila 0: búsqueda (Auto)
            .RowStyles.Add(New RowStyle(SizeType.AutoSize))
            ' Fila 1: grilla (100%)
            .RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
            ' Fila 2: GroupBox (Auto)
            .RowStyles.Add(New RowStyle(SizeType.AutoSize))
            ' Fila 3: Botonera (Auto)
            .RowStyles.Add(New RowStyle(SizeType.AutoSize))
            .AutoSize = False
        End With

        ' ===== 2) Área de búsqueda =====
        ' Reemplazar el Panel1 por un TLP interno (Label + TextBox) para evitar solapamientos
        Dim tlpBusqueda As New TableLayoutPanel() With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 2,
        .RowCount = 1,
        .Padding = New Padding(0),
        .Margin = New Padding(0)
    }
        tlpBusqueda.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))         ' Label
        tlpBusqueda.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))  ' TextBox

        Label8.AutoSize = True
        Label8.Margin = New Padding(4, 6, 8, 4)
        Label8.Text = "Buscar:"

        txtBuscar.Dock = DockStyle.Fill
        txtBuscar.Margin = New Padding(0, 3, 0, 3)

        tlpBusqueda.Controls.Add(Label8, 0, 0)
        tlpBusqueda.Controls.Add(txtBuscar, 1, 0)

        Panel1.Controls.Clear()
        Panel1.Dock = DockStyle.Fill
        Panel1.Padding = New Padding(4, 4, 4, 4)
        Panel1.Controls.Add(tlpBusqueda)

        ' ===== 3) Grilla =====
        With dgvNomenclaturas
            .Dock = DockStyle.Fill
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            If .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None Then
                .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            End If
        End With

        ' ===== 4) GroupBox (auto alto, no “Fill” en fila Auto) =====
        With GroupBox1
            .Dock = DockStyle.Top
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
            .Margin = New Padding(4, 8, 4, 8)
            .Padding = New Padding(8)
        End With

        ' Asegurar anclajes de controles largos dentro del GroupBox
        For Each tb In New TextBox() {txtNombre, txtObservaciones}
            tb.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        Next
        For Each tb In New TextBox() {txtCodigo, txtArea, txtPatron, txtEjemplo, txtUbicacion}
            tb.Anchor = AnchorStyles.Top Or AnchorStyles.Left
        Next
        For Each cb In New CheckBox() {chkUsaFecha, chkUsaNomenclaturaCodigo}
            cb.Anchor = AnchorStyles.Top Or AnchorStyles.Right
            cb.Margin = New Padding(6, cb.Margin.Top, 6, cb.Margin.Bottom)
        Next

        ' ===== 5) Botonera con FlowLayout (wrap + derecha) =====
        With FlowLayoutPanel1
            .Dock = DockStyle.Top
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
            .FlowDirection = FlowDirection.RightToLeft
            .WrapContents = True
            .Padding = New Padding(0)
            .Margin = New Padding(4, 4, 4, 8)
            .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        End With

        ' Configurar botones (orden: derecha -> izquierda)
        Dim botones() As Button = {btnVolver, btnGuardar, btnEliminar, btnNuevo}
        For Each b In botones
            b.AutoSize = True
            b.AutoSizeMode = AutoSizeMode.GrowAndShrink
            b.MinimumSize = New Size(120, 40)
            b.Margin = New Padding(6)
            b.Dock = DockStyle.None
        Next
    End Sub


    Private Async Function CargarDatosAsync() As Task
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim repo = _unitOfWork.Repository(Of Nomenclatura)()
            _listaNomenclaturas = Await repo.GetAll().OrderBy(Function(n) n.Nombre).ToListAsync()
            dgvNomenclaturas.DataSource = _listaNomenclaturas
            dgvNomenclaturas.ClearSelection()
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudieron cargar las nomenclaturas: {ex.Message}")
            dgvNomenclaturas.DataSource = New List(Of Nomenclatura)()
        Finally
            Me.Cursor = oldCursor
        End Try
    End Function


    Private Sub ConfigurarGrilla()
        If dgvNomenclaturas.Columns.Count = 0 Then Return

        dgvNomenclaturas.RowHeadersVisible = False
        dgvNomenclaturas.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvNomenclaturas.MultiSelect = False
        dgvNomenclaturas.ReadOnly = True
        dgvNomenclaturas.AllowUserToAddRows = False
        dgvNomenclaturas.AllowUserToDeleteRows = False

        ' Ocultas
        If dgvNomenclaturas.Columns.Contains("Id") Then dgvNomenclaturas.Columns("Id").Visible = False
        If dgvNomenclaturas.Columns.Contains("Patron") Then dgvNomenclaturas.Columns("Patron").Visible = False
        If dgvNomenclaturas.Columns.Contains("Ejemplo") Then dgvNomenclaturas.Columns("Ejemplo").Visible = False
        If dgvNomenclaturas.Columns.Contains("UbicacionArchivo") Then dgvNomenclaturas.Columns("UbicacionArchivo").Visible = False
        If dgvNomenclaturas.Columns.Contains("Observaciones") Then dgvNomenclaturas.Columns("Observaciones").Visible = False

        ' Ajustes
        If dgvNomenclaturas.Columns.Contains("Nombre") Then dgvNomenclaturas.Columns("Nombre").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        If dgvNomenclaturas.Columns.Contains("Codigo") Then dgvNomenclaturas.Columns("Codigo").Width = 120
        If dgvNomenclaturas.Columns.Contains("AreaResponsable") Then
            dgvNomenclaturas.Columns("AreaResponsable").HeaderText = "Área Resp."
            dgvNomenclaturas.Columns("AreaResponsable").Width = 120
        End If
        If dgvNomenclaturas.Columns.Contains("UsaFecha") Then dgvNomenclaturas.Columns("UsaFecha").Width = 90
        If dgvNomenclaturas.Columns.Contains("UsaNomenclaturaCodigo") Then
            dgvNomenclaturas.Columns("UsaNomenclaturaCodigo").HeaderText = "Usa Código"
            dgvNomenclaturas.Columns("UsaNomenclaturaCodigo").Width = 90
        End If
    End Sub


    Private Sub dgvNomenclaturas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNomenclaturas.SelectionChanged
        If dgvNomenclaturas.CurrentRow Is Nothing Then Return
        If dgvNomenclaturas.AllowUserToAddRows AndAlso dgvNomenclaturas.CurrentRow.Index = dgvNomenclaturas.NewRowIndex Then Return

        Dim item = TryCast(dgvNomenclaturas.CurrentRow.DataBoundItem, Nomenclatura)
        If item Is Nothing Then Return

        _nomenclaturaSeleccionada = item
        MostrarDetalles()
    End Sub


    Private Sub MostrarDetalles()
        If _nomenclaturaSeleccionada IsNot Nothing Then
            txtNombre.Text = _nomenclaturaSeleccionada.Nombre
            txtCodigo.Text = _nomenclaturaSeleccionada.Codigo
            txtArea.Text = _nomenclaturaSeleccionada.AreaResponsable
            txtPatron.Text = _nomenclaturaSeleccionada.Patron
            txtEjemplo.Text = _nomenclaturaSeleccionada.Ejemplo
            txtUbicacion.Text = _nomenclaturaSeleccionada.UbicacionArchivo
            txtObservaciones.Text = _nomenclaturaSeleccionada.Observaciones
            chkUsaFecha.Checked = _nomenclaturaSeleccionada.UsaFecha
            chkUsaNomenclaturaCodigo.Checked = _nomenclaturaSeleccionada.UsaNomenclaturaCodigo
            btnEliminar.Enabled = True
        Else
            LimpiarCampos()
        End If
    End Sub

    Private Sub LimpiarCampos()
        _nomenclaturaSeleccionada = New Nomenclatura()
        txtNombre.Clear()
        txtCodigo.Clear()
        txtArea.Clear()
        txtPatron.Clear()
        txtEjemplo.Clear()
        txtUbicacion.Clear()
        txtObservaciones.Clear()
        chkUsaFecha.Checked = False
        chkUsaNomenclaturaCodigo.Checked = False
        btnEliminar.Enabled = False
        dgvNomenclaturas.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) OrElse String.IsNullOrWhiteSpace(txtCodigo.Text) Then
            Notifier.Warn(Me, "El Nombre y el Código son obligatorios.")
            If String.IsNullOrWhiteSpace(txtNombre.Text) Then txtNombre.Focus() Else txtCodigo.Focus()
            Return
        End If

        _nomenclaturaSeleccionada.Nombre = txtNombre.Text.Trim()
        _nomenclaturaSeleccionada.Codigo = txtCodigo.Text.Trim()
        _nomenclaturaSeleccionada.AreaResponsable = txtArea.Text.Trim()
        _nomenclaturaSeleccionada.Patron = txtPatron.Text.Trim()
        _nomenclaturaSeleccionada.Ejemplo = txtEjemplo.Text.Trim()
        _nomenclaturaSeleccionada.UbicacionArchivo = txtUbicacion.Text.Trim()
        _nomenclaturaSeleccionada.Observaciones = txtObservaciones.Text.Trim()
        _nomenclaturaSeleccionada.UsaFecha = chkUsaFecha.Checked
        _nomenclaturaSeleccionada.UsaNomenclaturaCodigo = chkUsaNomenclaturaCodigo.Checked

        Dim repo = _unitOfWork.Repository(Of Nomenclatura)()

        Dim oldCursor = Me.Cursor
        btnGuardar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            If _nomenclaturaSeleccionada.Id = 0 Then
                repo.Add(_nomenclaturaSeleccionada)
            Else
                repo.Update(_nomenclaturaSeleccionada)
            End If

            Await _unitOfWork.CommitAsync()
            Notifier.Success(Me, "Nomenclatura guardada correctamente.")

            Await CargarDatosAsync()
            LimpiarCampos()

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al guardar: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnGuardar.Enabled = True
        End Try
    End Sub


    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _nomenclaturaSeleccionada Is Nothing OrElse _nomenclaturaSeleccionada.Id = 0 Then
            Notifier.Warn(Me, "Debe seleccionar una nomenclatura para eliminar.")
            Return
        End If

        If MessageBox.Show(
        $"¿Eliminar la nomenclatura '{_nomenclaturaSeleccionada.Nombre}'?",
        "Confirmar eliminación",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Dim oldCursor = Me.Cursor
        btnEliminar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Dim repo = _unitOfWork.Repository(Of Nomenclatura)()
            repo.Remove(_nomenclaturaSeleccionada)
            Await _unitOfWork.CommitAsync()

            Notifier.Info(Me, "Nomenclatura eliminada.")
            Await CargarDatosAsync()
            LimpiarCampos()

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al eliminar: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnEliminar.Enabled = True
        End Try
    End Sub


    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        If _listaNomenclaturas Is Nothing Then Return

        Dim q = If(txtBuscar.Text, String.Empty).Trim().ToUpperInvariant()
        If q.Length = 0 Then
            dgvNomenclaturas.DataSource = _listaNomenclaturas
            dgvNomenclaturas.ClearSelection()
            Return
        End If

        Dim resultado As List(Of Nomenclatura) =
        _listaNomenclaturas.Where(Function(n)
                                      Dim nombre = If(n?.Nombre, String.Empty).ToUpperInvariant()
                                      Dim codigo = If(n?.Codigo, String.Empty).ToUpperInvariant()
                                      Dim area = If(n?.AreaResponsable, String.Empty).ToUpperInvariant()
                                      Return nombre.Contains(q) OrElse codigo.Contains(q) OrElse area.Contains(q)
                                  End Function).ToList()

        dgvNomenclaturas.DataSource = resultado
        dgvNomenclaturas.ClearSelection()
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub

    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click

        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmConfiguracion)(Me)
    End Sub

End Class