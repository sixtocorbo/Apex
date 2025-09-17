Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Data
Imports System.Globalization
Imports System.Text

Partial Public Class frmFiltros
    Private _dtOriginal As DataTable = New DataTable()
    Private _dvDatos As DataView = Nothing
    Private ReadOnly _filtros As New GestorFiltros()
    Private _isUpdatingValores As Boolean = False

#Region "Modelos y Clases de Ayuda"

    Public Enum OperadorComparacion
        Igual
        EnLista
    End Enum

    Public Class ReglaFiltro
        Public Property Columna As String = String.Empty
        Public Property Operador As OperadorComparacion
        Public Property Valor1 As String = String.Empty
        Public Property Valor2 As String = String.Empty

        Public Overrides Function ToString() As String
            Dim opStr = System.Enum.GetName(GetType(OperadorComparacion), Me.Operador)
            If Operador = OperadorComparacion.EnLista Then
                Return $"{Columna} {opStr} ({Valor1.Split("|"c).Length} valores)"
            Else
                Return $"{Columna} {opStr} {Valor1}"
            End If
        End Function

        Private Shared Function EscapeSqlLike(s As String) As String
            Return s.Replace("'", "''")
        End Function

        Private Shared Function FormatearValor(valor As String) As String
            If DateTime.TryParse(valor, CultureInfo.InvariantCulture, DateTimeStyles.None, Nothing) Then
                Return $"#{Convert.ToDateTime(valor).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}#"
            End If
            If Double.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, Nothing) Then
                Return valor
            End If
            Return $"'{EscapeSqlLike(valor)}'"
        End Function

        Public Function ToRowFilter() As String
            Dim colName = $"[{Columna}]"
            Select Case Operador
                Case OperadorComparacion.Igual
                    If DateTime.TryParse(Valor1, Nothing) Then
                        Dim fecha As Date = Convert.ToDateTime(Valor1).Date
                        Dim siguienteDia As Date = fecha.AddDays(1)
                        Return $"({colName} >= #{fecha:yyyy-MM-dd}# AND {colName} < #{siguienteDia:yyyy-MM-dd}#)"
                    Else
                        Return $"{colName} = {FormatearValor(Valor1)}"
                    End If
                Case OperadorComparacion.EnLista
                    Dim items = Valor1.Split("|"c).Select(AddressOf FormatearValor)
                    Return $"{colName} IN ({String.Join(",", items)})"
                Case Else
                    Throw New NotSupportedException($"Operador {Operador} aún no implementado.")
            End Select
        End Function
    End Class

    Friend Class GestorFiltros
        Private ReadOnly _reglas As New BindingList(Of ReglaFiltro)()
        Public ReadOnly Property Reglas As BindingList(Of ReglaFiltro)
            Get
                Return _reglas
            End Get
        End Property
        Public Sub Limpiar()
            _reglas.Clear()
        End Sub
        Public Sub Agregar(r As ReglaFiltro)
            _reglas.Add(r)
        End Sub
        Public Sub Quitar(r As ReglaFiltro)
            _reglas.Remove(r)
        End Sub
        Public Function RowFilter() As String
            If Not _reglas.Any() Then Return String.Empty
            Return String.Join(" AND ", _reglas.Select(Function(x) x.ToRowFilter()))
        End Function
    End Class

#End Region

#Region "Ciclo de Vida del Formulario"

    Private Sub frmFiltroAvanzado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Layout base responsivo + splitter
        ConfigurarLayoutResponsivo()
        AjustarSplitter()

        ' Tema
        AppTheme.Aplicar(Me)

        ' Pegado a los bordes, sin “gutter”
        Me.Margin = Padding.Empty
        Me.Padding = Padding.Empty
        splitContenedorPrincipal.Panel2.AutoScroll = False

        ' DGV sin parpadeo
        dgvDatos.DoubleBuffered(True)
        dgvDatos.SendToBack()

        ' Fuente de datos del combo
        cmbOrigenDatos.DataSource = [Enum].GetValues(GetType(TipoOrigenDatos))
        cmbOrigenDatos.SelectedIndex = -1

        ' Orden visual
        gbxBusquedaGlobal.BringToFront()
        flpChips.BringToFront()
        pnlAcciones.BringToFront()

        ' Action bars a la derecha
        pnlFiltroBotones.FlowDirection = FlowDirection.RightToLeft
        flpAcciones.FlowDirection = FlowDirection.RightToLeft

        ' Botones “neutros”
        btnLimpiar.Tag = "KeepBackColor"
        btnExportarExcel.Tag = "KeepBackColor"
        btnCopiarCorreos.Tag = "KeepBackColor"
        btnExportarFichasPDF.Tag = "KeepBackColor"

        ' Placeholder
        Try : AppTheme.SetCue(txtBusquedaGlobal, "Buscar en todos los campos…") : Catch : End Try
        Try : AppTheme.SetCue(txtBuscarValor, "Filtrar valores…") : Catch : End Try

        ' Atajos
        Me.AcceptButton = btnFiltrar
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf Form_KeyDown_EscCierra

        ' Eventos
        AddHandler btnCargar.Click, AddressOf btnCargar_Click_Async
        AddHandler lstColumnas.SelectedIndexChanged, AddressOf LstColumnas_SelectedIndexChanged
        AddHandler txtBusquedaGlobal.TextChanged, AddressOf TxtBusquedaGlobal_TextChanged
        ' txtBuscarValor usa Handles abajo

        BeautifyGrid()

        UpdateUIState()
    End Sub

    Private Sub Form_KeyDown_EscCierra(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

#End Region

#Region "Lógica de Carga de Datos"

    Private Async Sub btnCargar_Click_Async(sender As Object, e As EventArgs)
        Await CargarDatosAsync()
    End Sub

    Public Async Function CargarDatosAsync() As Task
        If cmbOrigenDatos.SelectedItem Is Nothing Then Return

        Dim origen = CType(cmbOrigenDatos.SelectedItem, TipoOrigenDatos)
        Dim fechaI = dtpFechaInicio.Value.Date
        Dim fechaF = dtpFechaFin.Value.Date

        If origen <> TipoOrigenDatos.Funcionarios AndAlso fechaI > fechaF Then
            MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnCargar.Enabled = False
        LoadingHelper.MostrarCargando(Me)

        Try
            Using wait As New WaitCursor()
                _dtOriginal = Await ConsultasGenericas.ObtenerDatosGenericosAsync(origen, fechaI, fechaF)
                AñadirColumnaBusquedaGlobal(_dtOriginal)
                _dvDatos = _dtOriginal.DefaultView
            End Using

            LimpiarFiltrosYChips()
            ConfigurarGrilla(_dtOriginal)
            ActualizarListaColumnas()

            dgvDatos.DataSource = _dvDatos
            AplicarFiltros()

        Catch ex As Exception
            MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LimpiarTodo()
        Finally
            LoadingHelper.OcultarCargando(Me)
            btnCargar.Enabled = True
            UpdateUIState()
        End Try
    End Function

    Private Sub AñadirColumnaBusquedaGlobal(dt As DataTable)
        Const SEARCH_COLUMN_NAME As String = "GlobalSearch"
        If dt Is Nothing OrElse dt.Columns.Contains(SEARCH_COLUMN_NAME) Then Return

        dt.Columns.Add(SEARCH_COLUMN_NAME, GetType(String))
        Dim stringColumns = dt.Columns.Cast(Of DataColumn).Where(Function(c) c.DataType = GetType(String)).ToList()

        For Each row As DataRow In dt.Rows
            Dim sb As New StringBuilder()
            For Each col In stringColumns
                sb.Append(row(col).ToString()).Append(" ")
            Next
            row(SEARCH_COLUMN_NAME) = sb.ToString()
        Next
    End Sub

    Private Sub ConfigurarGrilla(dt As DataTable)
        dgvDatos.DataSource = Nothing
        dgvDatos.Columns.Clear()
        dgvDatos.AutoGenerateColumns = False
        If dt Is Nothing Then Return

        Dim columnDefinitions As New Dictionary(Of String, String) From {
            {"NombreCompleto", "Nombre"}, {"Cedula", "Cédula"}, {"Cargo", "Cargo"},
            {"Escalafon", "Escalafón"}, {"Seccion", "Sección"}, {"Turno", "Turno"},
            {"Semana", "Semana"}, {"PuestoDeTrabajo", "Puesto de Trabajo"}
        }

        Dim origen = CType(cmbOrigenDatos.SelectedItem, TipoOrigenDatos)
        Dim columnsToShow = If(origen = TipoOrigenDatos.Funcionarios,
                               columnDefinitions.Keys,
                               dt.Columns.Cast(Of DataColumn).Select(Function(c) c.ColumnName))

        For Each colName In columnsToShow
            If colName.ToLower().EndsWith("id") OrElse colName = "GlobalSearch" Then Continue For

            Dim headerText = If(columnDefinitions.ContainsKey(colName), columnDefinitions(colName), colName)
            Dim dgvCol As New DataGridViewTextBoxColumn With {
                .DataPropertyName = colName,
                .HeaderText = headerText,
                .Name = colName,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            }

            If dt.Columns(colName).DataType = GetType(Date) OrElse dt.Columns(colName).DataType = GetType(DateTime) Then
                dgvCol.DefaultCellStyle.Format = "dd/MM/yyyy"
            End If

            dgvDatos.Columns.Add(dgvCol)
        Next

        ' Última columna “Fill”
        Dim ultima = dgvDatos.Columns.Cast(Of DataGridViewColumn).LastOrDefault(Function(c) c.Visible)
        If ultima IsNot Nothing Then ultima.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    End Sub

    Private Sub ActualizarListaColumnas()
        lstColumnas.Items.Clear()
        If _dtOriginal Is Nothing Then Return

        Dim columnNames = _dtOriginal.Columns.Cast(Of DataColumn).
                          Select(Function(c) c.ColumnName).
                          Where(Function(name) Not name.ToLower().EndsWith("id") AndAlso name <> "GlobalSearch").
                          ToArray()
        lstColumnas.Items.AddRange(columnNames)
        If lstColumnas.Items.Count > 0 Then lstColumnas.SelectedIndex = 0
    End Sub

#End Region

#Region "Eventos y Lógica de UI"

    Private Sub LstColumnas_SelectedIndexChanged(sender As Object, e As EventArgs)
        If _isUpdatingValores Then Return
        ActualizarListaDeValores()
    End Sub

    Private Sub TxtBusquedaGlobal_TextChanged(sender As Object, e As EventArgs)
        AplicarFiltros()
    End Sub

    Private Sub txtBuscarValor_TextChanged(sender As Object, e As EventArgs) Handles txtBuscarValor.TextChanged
        ActualizarListaDeValores()
    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        If lstColumnas.SelectedItem Is Nothing OrElse lstValores.SelectedItems.Count = 0 Then Return

        If EsFiltroRedundante() Then
            MessageBox.Show("Ha seleccionado todos los valores disponibles. El filtro es redundante y no se agregará.",
                             "Filtro Redundante", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim col = lstColumnas.SelectedItem.ToString()
        Dim nuevaRegla As ReglaFiltro

        If lstValores.SelectedItems.Count > 1 Then
            Dim valores = lstValores.SelectedItems.Cast(Of Object).Select(Function(v) v.ToString())
            nuevaRegla = New ReglaFiltro With {.Columna = col, .Operador = OperadorComparacion.EnLista, .Valor1 = String.Join("|", valores)}
        Else
            nuevaRegla = New ReglaFiltro With {.Columna = col, .Operador = OperadorComparacion.Igual, .Valor1 = lstValores.SelectedItem.ToString()}
        End If

        If _filtros.Reglas.Any(Function(r) r.ToString() = nuevaRegla.ToString()) Then
            MessageBox.Show("Este filtro ya ha sido agregado.", "Filtro Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        _filtros.Agregar(nuevaRegla)
        CrearChip(nuevaRegla)
        AplicarFiltros()
    End Sub

    Private Function EsFiltroRedundante() As Boolean
        Return lstValores.SelectedItems.Count = lstValores.Items.Count
    End Function

    Private Sub BtnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        LimpiarFiltrosYChips()
        AplicarFiltros()
    End Sub

#End Region

#Region "Gestión de Filtros y Chips"

    Private Sub AplicarFiltros()
        If _dvDatos Is Nothing Then Return

        Dim filtroReglas = _filtros.RowFilter()
        Dim filtroGlobal = ConstruirFiltroGlobal()

        _dvDatos.RowFilter = String.Join(" AND ", {filtroReglas, filtroGlobal}.Where(Function(s) Not String.IsNullOrWhiteSpace(s)))

        ActualizarListaDeValores()
        UpdateUIState()
    End Sub

    Private Function ConstruirFiltroGlobal() As String
        Dim searchText = txtBusquedaGlobal.Text.Trim()
        If String.IsNullOrWhiteSpace(searchText) Then Return String.Empty

        Dim palabras = searchText.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).
                                Select(Function(p) $"GlobalSearch LIKE '%{p.Replace("'", "''")}%'")
        Return $"({String.Join(" AND ", palabras)})"
    End Function

    Private Sub CrearChip(regla As ReglaFiltro)
        Dim nuevoChip As New ChipControl(regla)
        AddHandler nuevoChip.CerrarClick, AddressOf Chip_CerrarClick
        flpChips.Controls.Add(nuevoChip)
        UpdateFiltrosPanelHeight()
    End Sub

    Private Sub Chip_CerrarClick(sender As Object, e As EventArgs)
        Dim chip = CType(sender, ChipControl)
        Dim regla = chip.Regla

        If regla IsNot Nothing Then
            _filtros.Quitar(regla)
            flpChips.Controls.Remove(chip)
            chip.Dispose()
            RestaurarSeleccionDesdeChip(regla)
            AplicarFiltros()
            UpdateFiltrosPanelHeight()
        End If
    End Sub

    Private Sub RestaurarSeleccionDesdeChip(regla As ReglaFiltro)
        _isUpdatingValores = True
        lstColumnas.SelectedItem = regla.Columna
        ActualizarListaDeValores()

        Dim valoresParaSeleccionar = New HashSet(Of String)(regla.Valor1.Split("|"c))
        For i = 0 To lstValores.Items.Count - 1
            If valoresParaSeleccionar.Contains(lstValores.Items(i).ToString()) Then
                lstValores.SetSelected(i, True)
            End If
        Next
        _isUpdatingValores = False
    End Sub

#End Region

#Region "Métodos de Ayuda y UI"

    Private Sub UpdateUIState()
        Dim total As Integer = If(_dtOriginal?.Rows.Count, 0)
        Dim visibles As Integer = If(_dvDatos?.Count, 0)
        gbxFiltros.Enabled = (visibles > 0 OrElse total > 0)

        Separator1.Visible = (btnExportarExcel.Visible Or btnCopiarCorreos.Visible)

        lblConteoRegistros.Text = $"Registros: {visibles:n0} de {total:n0}"

        UpdateFiltrosPanelHeight()
        PanelChips.Visible = (flpChips.Controls.Count > 0)
    End Sub

    Private Sub LimpiarTodo()
        dgvDatos.DataSource = Nothing
        dgvDatos.Columns.Clear()
        _dtOriginal = New DataTable()
        _dvDatos = Nothing
        lstColumnas.Items.Clear()
        lstValores.Items.Clear()
        txtBuscarValor.Clear()
        LimpiarFiltrosYChips()
        UpdateUIState()
    End Sub

    Private Sub LimpiarFiltrosYChips()
        _filtros.Limpiar()
        flpChips.Controls.Clear()
        txtBusquedaGlobal.Clear()
    End Sub

    Private Sub ActualizarListaDeValores()
        lstValores.BeginUpdate()
        lstValores.Items.Clear()

        If lstColumnas.SelectedItem Is Nothing OrElse _dvDatos Is Nothing Then
            lstValores.EndUpdate()
            Return
        End If

        Dim colName = lstColumnas.SelectedItem.ToString()
        Dim dtValoresUnicos As DataTable = _dvDatos.ToTable(True, colName)

        Dim filtro As String = txtBuscarValor.Text.Trim()

        Dim valoresUnicos = dtValoresUnicos.AsEnumerable().
                            Select(Function(r) r.Field(Of Object)(colName)).
                            Where(Function(v) v IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(v.ToString())).
                            Select(Function(v) v.ToString()).
                            Where(Function(s) If(filtro = "", True, s.IndexOf(filtro, StringComparison.CurrentCultureIgnoreCase) >= 0)).
                            OrderBy(Function(s) s, StringComparer.CurrentCultureIgnoreCase).
                            ToArray()

        lstValores.Items.AddRange(valoresUnicos)
        lstValores.EndUpdate()

        AjustarHorizontalScrollbar(lstValores)
    End Sub

    Private Sub AjustarHorizontalScrollbar(lst As ListBox)
        Dim maxW As Integer = 0
        Using g = lst.CreateGraphics()
            For Each it In lst.Items
                Dim w = TextRenderer.MeasureText(g, it.ToString(), lst.Font).Width
                If w > maxW Then maxW = w
            Next
        End Using
        lst.HorizontalExtent = maxW + SystemInformation.VerticalScrollBarWidth
    End Sub

    Private Sub UpdateFiltrosPanelHeight()
        Const MAX_CHIP_PANEL_HEIGHT As Integer = 120
        flpChips.Visible = flpChips.Controls.Count > 0
        If flpChips.Visible Then
            flpChips.Height = Math.Min(MAX_CHIP_PANEL_HEIGHT, flpChips.GetPreferredSize(New Size(flpChips.Width, 0)).Height)
            flpChips.AutoScroll = flpChips.Height >= MAX_CHIP_PANEL_HEIGHT
        End If
    End Sub

    Private NotInheritable Class WaitCursor
        Implements IDisposable
        Private ReadOnly _old As Cursor
        Public Sub New()
            _old = Cursor.Current
            Cursor.Current = Cursors.WaitCursor
        End Sub
        Public Sub Dispose() Implements IDisposable.Dispose
            Cursor.Current = _old
        End Sub
    End Class

    Private Sub BeautifyGrid()
        dgvDatos.Dock = DockStyle.Fill
        dgvDatos.RowHeadersVisible = False
        dgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
        ' Última columna “fill” se setea al crear columnas
    End Sub

#End Region

#Region "Botón Copiar Correos + helpers públicos"

    Private Sub btnCopiarCorreos_Click(sender As Object, e As EventArgs) Handles btnCopiarCorreos.Click
        If _dvDatos Is Nothing OrElse _dvDatos.Count = 0 Then
            MessageBox.Show("No hay registros en la vista actual para copiar correos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Const NOMBRE_COLUMNA_CORREO As String = "Email"
        If Not _dvDatos.Table.Columns.Contains(NOMBRE_COLUMNA_CORREO) Then
            MessageBox.Show($"No se encontró la columna '{NOMBRE_COLUMNA_CORREO}' en los datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim correos As New List(Of String)
        For Each rowView As DataRowView In _dvDatos
            Dim correo = rowView(NOMBRE_COLUMNA_CORREO)?.ToString()
            If Not String.IsNullOrWhiteSpace(correo) Then correos.Add(correo.Trim())
        Next

        If correos.Any() Then
            Dim correosParaCopiar = String.Join("; ", correos.Distinct())
            Clipboard.SetText(correosParaCopiar)
            MessageBox.Show($"{correos.Count} correos han sido copiados al portapapeles.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("No se encontraron correos válidos en la selección actual.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Public Function GetIdsFiltrados(nombreColumnaId As String) As List(Of Integer)
        Dim ids As New List(Of Integer)
        If _dvDatos Is Nothing OrElse Not _dvDatos.Table.Columns.Contains(nombreColumnaId) Then Return ids
        For Each rowView As DataRowView In _dvDatos
            Dim v = rowView.Row.Field(Of Object)(nombreColumnaId)
            If v IsNot Nothing Then
                Dim id As Integer
                If Integer.TryParse(v.ToString(), id) Then ids.Add(id)
            End If
        Next
        Return ids.Distinct().ToList()
    End Function

    Public Function GetDiccionarioIdNombre(nombreColumnaId As String, nombreColumnaNombre As String) As Dictionary(Of Integer, String)
        Dim dict As New Dictionary(Of Integer, String)
        If _dvDatos Is Nothing OrElse
           Not _dvDatos.Table.Columns.Contains(nombreColumnaId) OrElse
           Not _dvDatos.Table.Columns.Contains(nombreColumnaNombre) Then Return dict

        For Each rowView As DataRowView In _dvDatos
            Dim vId = rowView.Row.Field(Of Object)(nombreColumnaId)
            Dim vNom = rowView.Row.Field(Of Object)(nombreColumnaNombre)
            If vId Is Nothing Then Continue For
            Dim id As Integer
            If Integer.TryParse(vId.ToString(), id) Then
                Dim nom = If(vNom?.ToString(), $"Funcionario #{id}")
                If Not dict.ContainsKey(id) Then dict.Add(id, nom)
            End If
        Next
        Return dict
    End Function

#End Region

#Region "Diseño Responsivo"

    Private _splitRatio As Double = 0.3

    Private Sub ConfigurarLayoutResponsivo()
        Me.splitContenedorPrincipal.FixedPanel = FixedPanel.None
        Me.splitContenedorPrincipal.IsSplitterFixed = False

        Me.splitContenedorPrincipal.DoubleBuffered(True)
        Me.pnlContenedor.DoubleBuffered(True)
        Me.TableLayoutPanelLeft.DoubleBuffered(True)
        Me.TableLayoutPanelRight.DoubleBuffered(True)
        Me.dgvDatos.DoubleBuffered(True)

        ' Los eventos Resize y Layout ya no son necesarios gracias al pnlContenedor
        AddHandler Me.splitContenedorPrincipal.SplitterMoved, AddressOf splitContenedorPrincipal_SplitterMoved
        AddHandler Me.Resize, AddressOf frmFiltros_Resize
    End Sub

    Private Sub frmFiltros_Resize(sender As Object, e As EventArgs)
        AjustarSplitter()
    End Sub

    Private Sub splitContenedorPrincipal_SplitterMoved(sender As Object, e As SplitterEventArgs)
        If splitContenedorPrincipal.Width > 0 Then
            _splitRatio = splitContenedorPrincipal.SplitterDistance / Math.Max(1, splitContenedorPrincipal.Width)
        End If
    End Sub

    Private Sub AjustarSplitter()
        Dim ancho As Integer = Math.Max(1, splitContenedorPrincipal.Width)
        Dim distanciaDeseada As Integer = CInt(ancho * _splitRatio)

        If splitContenedorPrincipal.Width > (splitContenedorPrincipal.Panel1MinSize + splitContenedorPrincipal.Panel2MinSize) Then
            splitContenedorPrincipal.SplitterDistance = Math.Max(splitContenedorPrincipal.Panel1MinSize,
                                                         Math.Min(distanciaDeseada, ancho - splitContenedorPrincipal.Panel2MinSize))
        End If
    End Sub

#End Region
End Class

Public Module ControlExtensions
    <System.Runtime.CompilerServices.Extension()>
    Public Sub DoubleBuffered(ByVal control As System.Windows.Forms.Control, ByVal enable As Boolean)
        Dim prop = control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
        prop.SetValue(control, enable, Nothing)
    End Sub
End Module
