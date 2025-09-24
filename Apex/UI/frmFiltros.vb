Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Globalization
Imports System.Text

Partial Public Class frmFiltros
    Inherits FormActualizable
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

        Private Shared Function EscapeStringLiteral(s As String) As String
            Return s.Replace("'", "''")
        End Function
        Private Shared Function FormatearValor(valor As String) As String
            Dim dt As DateTime
            If DateTime.TryParse(valor, CultureInfo.InvariantCulture, DateTimeStyles.None, dt) Then
                Return $"#{dt:yyyy-MM-dd}#"
            End If

            Dim dbl As Double
            If Double.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, dbl) Then
                Return dbl.ToString(CultureInfo.InvariantCulture)
            End If

            Return $"'{EscapeStringLiteral(valor)}'"
        End Function

        'Private Shared Function FormatearValor(valor As String) As String
        '    If DateTime.TryParse(valor, CultureInfo.InvariantCulture, DateTimeStyles.None, Nothing) Then
        '        Return $"#{Convert.ToDateTime(valor).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}#"
        '    End If
        '    If Double.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, Nothing) Then
        '        Return valor
        '    End If
        '    Return $"'{EscapeStringLiteral(valor)}'"
        'End Function
        Public Function ToRowFilter() As String
            Dim colName = $"[{Columna}]"
            Select Case Operador
                Case OperadorComparacion.Igual
                    Dim fecha As Date
                    If DateTime.TryParse(Valor1, fecha) Then
                        fecha = fecha.Date
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

        'Public Function ToRowFilter() As String
        '    Dim colName = $"[{Columna}]"
        '    Select Case Operador
        '        Case OperadorComparacion.Igual
        '            If DateTime.TryParse(Valor1, Nothing) Then
        '                Dim fecha As Date = Convert.ToDateTime(Valor1).Date
        '                Dim siguienteDia As Date = fecha.AddDays(1)
        '                Return $"({colName} >= #{fecha:yyyy-MM-dd}# AND {colName} < #{siguienteDia:yyyy-MM-dd}#)"
        '            Else
        '                Return $"{colName} = {FormatearValor(Valor1)}"
        '            End If
        '        Case OperadorComparacion.EnLista
        '            Dim items = Valor1.Split("|"c).Select(AddressOf FormatearValor)
        '            Return $"{colName} IN ({String.Join(",", items)})"
        '        Case Else
        '            Throw New NotSupportedException($"Operador {Operador} aún no implementado.")
        '    End Select
        'End Function
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
    Private Async Sub ManejadorFuncionarioActualizado(sender As Object, e As FuncionarioCambiadoEventArgs)
        If cmbOrigenDatos.SelectedItem IsNot Nothing AndAlso CType(cmbOrigenDatos.SelectedItem, TipoOrigenDatos) = TipoOrigenDatos.Funcionarios Then
            Await CargarDatosAsync()
        End If
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
    Protected Overrides Async Function RefrescarSegunFuncionarioAsync(e As FuncionarioCambiadoEventArgs) As Task
        ' Si hay cualquier origen de datos seleccionado, lo recargamos,
        ' ya que un cambio en un funcionario puede afectar a licencias, sanciones, etc.
        If cmbOrigenDatos.SelectedItem IsNot Nothing Then
            Await CargarDatosAsync()
        End If
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
        LimpiarFiltrosYChips()
        UpdateUIState()
    End Sub

    Private Sub LimpiarFiltrosYChips()
        _filtros.Limpiar()
        flpChips.Controls.Clear()
        txtBusquedaGlobal.Clear()
    End Sub

    Private Sub ActualizarListaDeValores()
        ' Inicia la actualización para mejorar el rendimiento y evitar parpadeos
        lstValores.BeginUpdate()
        lstValores.Items.Clear()

        ' Si no hay una columna seleccionada o no hay datos, no hace nada
        If lstColumnas.SelectedItem Is Nothing OrElse _dvDatos Is Nothing Then
            lstValores.EndUpdate()
            Return
        End If

        ' Obtiene el nombre de la columna seleccionada
        Dim colName = lstColumnas.SelectedItem.ToString()

        ' Crea una tabla temporal solo con los valores únicos de esa columna,
        ' respetando los filtros ya aplicados en el DataView (_dvDatos)
        Dim dtValoresUnicos As DataTable = _dvDatos.ToTable(True, colName)

        ' Usando LINQ, convierte la tabla a una lista de strings:
        ' 1. Selecciona el valor de cada fila.
        ' 2. Filtra los valores nulos o vacíos.
        ' 3. Convierte el valor a String.
        ' 4. Ordena la lista alfabéticamente.
        Dim valoresUnicos = dtValoresUnicos.AsEnumerable().
                          Select(Function(r) r.Field(Of Object)(colName)).
                          Where(Function(v) v IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(v.ToString())).
                          Select(Function(v) v.ToString()).
                          OrderBy(Function(s) s, StringComparer.CurrentCultureIgnoreCase).
                          ToArray()

        ' Agrega todos los valores únicos a la lista
        lstValores.Items.AddRange(valoresUnicos)

        ' Finaliza la actualización y muestra los cambios
        lstValores.EndUpdate()

        ' Ajusta la barra de scroll horizontal si el texto es muy largo
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
    ' --- en frmFiltros ---

    Private Sub btnCopiarCorreos_Click(sender As Object, e As EventArgs) Handles btnCopiarCorreos.Click
        Try
            If _dvDatos Is Nothing OrElse _dvDatos.Count = 0 Then
                Notifier.Info(Me, "No hay registros en la vista actual.", 2000)
                Return
            End If

            ' 1) detectar la columna de correo disponible
            Dim candidatos = New String() {"CorreoElectronico", "Email", "Correo", "Mail"}
            Dim colCorreo As String = candidatos.FirstOrDefault(Function(c) _dvDatos.Table.Columns.Contains(c))

            If String.IsNullOrEmpty(colCorreo) Then
                MessageBox.Show("No se encontró una columna de correo (CorreoElectronico/Email/Correo).",
                            "Apex", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' 2) extraer y normalizar correos de la vista actual
            Dim setCorreos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

            For Each rv As DataRowView In _dvDatos
                ' Opción más clara y robusta
                Dim raw As String = Convert.ToString(rv(colCorreo)).Trim()
                If raw = "" Then Continue For

                For Each correo In ExtractEmails(raw)
                    setCorreos.Add(correo)
                Next
            Next

            ' 3) copiar al portapapeles, uno por línea (CRLF)
            If setCorreos.Count = 0 Then
                Notifier.Warn(Me, "No se encontraron correos válidos en la lista.", 2500)
                Return
            End If

            Dim lineas = setCorreos.OrderBy(Function(s) s, StringComparer.OrdinalIgnoreCase).ToArray()
            Dim joined As String = String.Join(Environment.NewLine, lineas)

            ' usar DataObject para máxima compatibilidad (Excel, Notepad, Outlook)
            Dim dobj As New DataObject()
            dobj.SetText(joined, TextDataFormat.UnicodeText)
            dobj.SetText(joined, TextDataFormat.Text)
            Clipboard.SetDataObject(dobj, True)

            Notifier.Success(Me, $"{lineas.Length} correos copiados (ordenados por alfabeto).", 2500)


        Catch ex As Exception
            MessageBox.Show("Error al copiar correos: " & ex.Message, "Apex",
                        MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' --- helpers privados ---

    ' Devuelve una lista de e-mails válidos encontrados en un texto (acepta múltiples separados por ; , / espacios o saltos de línea)
    Private Function ExtractEmails(input As String) As IEnumerable(Of String)
        If String.IsNullOrWhiteSpace(input) Then Return Enumerable.Empty(Of String)()

        ' separadores comunes
        Dim trozos = input.Split(New Char() {";"c, ","c, "/"c, " "c, ControlChars.Cr, ControlChars.Lf, ControlChars.Tab},
                             StringSplitOptions.RemoveEmptyEntries)

        Dim list As New List(Of String)
        For Each t In trozos
            Dim s = t.Trim().Trim("."c) ' limpiar puntitos al final
            If IsValidEmail(s) Then list.Add(s)
        Next
        Return list
    End Function

    ' Validación de email mejorada con Regex, más robusta y rápida.
    Private Function IsValidEmail(s As String) As Boolean
        If String.IsNullOrWhiteSpace(s) Then Return False

        ' Evita strings muy largos y espacios, que son inválidos y pueden hacer que el Regex sea lento.
        If s.Length > 254 OrElse s.Contains(" "c) Then Return False

        Try
            ' Usamos una expresión regular para validar el formato del email.
            ' Esto es más flexible y seguro que usar New MailAddress().
            Return System.Text.RegularExpressions.Regex.IsMatch(s,
            "^[^@\s]+@[^@\s]+\.[^@\s]+$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(250))
        Catch e As System.Text.RegularExpressions.RegexMatchTimeoutException
            ' Protección contra strings maliciosos que podrían causar un DoS.
            Return False
        Catch
            Return False
        End Try
    End Function


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
#Region "Lógica de Exportación"

    ' 2. AGREGA ESTE NUEVO MÉTODO COMPLETO A TU FORMULARIO
    ' --- Exportar un PDF por funcionario filtrado ---
    Private Async Sub btnExportarFichasPDF_Click(sender As Object, e As EventArgs) Handles btnExportarFichasPDF.Click
        Try
            ' 1) Validaciones básicas
            If cmbOrigenDatos.SelectedItem Is Nothing OrElse
           Not cmbOrigenDatos.SelectedItem.Equals(TipoOrigenDatos.Funcionarios) Then
                MessageBox.Show("Seleccioná 'Funcionarios' como origen de datos.", "Apex",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim ids As List(Of Integer) = GetIdsFiltrados("Id")
            If ids Is Nothing OrElse ids.Count = 0 Then
                MessageBox.Show("No hay funcionarios para exportar en la vista actual.", "Apex",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' 2) Elegir carpeta destino
            Using fbd As New FolderBrowserDialog()
                fbd.Description = "Elegí la carpeta donde guardar un PDF por funcionario"
                If fbd.ShowDialog(Me) <> DialogResult.OK Then Return

                Dim carpeta As String = fbd.SelectedPath
                Dim tituloOriginal = Me.Text
                Cursor = Cursors.WaitCursor
                Me.Text = "Exportando fichas... (Preparando datos...)"
                Me.Refresh()

                ' Toast persistente de progreso
                Dim toast As Toast = Toast.ShowSticky(Me, "Preparando exportación...", ToastType.Info)

                ' 3) Traer todos los datos en una sola consulta
                Dim todosLosDatos As List(Of FichaFuncionalDTO)
                Using svc As New ReportesService()
                    todosLosDatos = Await svc.ObtenerDatosParaFichasAsync(ids)
                End Using

                If todosLosDatos Is Nothing OrElse todosLosDatos.Count = 0 Then
                    toast.UpdateMessage("No se encontraron datos para exportar.")
                    toast.CloseAfter(2500)
                    Me.Text = tituloOriginal
                    Cursor = Cursors.Default
                    Return
                End If

                ' 4) Exportar un PDF por funcionario
                Dim totalAExportar As Integer = ids.Count
                Dim exportados As Integer = 0
                Dim noEncontrados As New List(Of Integer)
                Dim fallos As New List(Of String)

                For i = 0 To totalAExportar - 1
                    Dim id As Integer = ids(i)
                    Me.Text = $"Exportando fichas... {i + 1}/{totalAExportar}"
                    Application.DoEvents()

                    ' Filtrar el/los DTO del funcionario actual (requiere FichaFuncionalDTO.FuncionarioId)
                    Dim datosUno = todosLosDatos.Where(Function(d) d.FuncionarioId = id).ToList()
                    If datosUno.Count = 0 Then
                        noEncontrados.Add(id)
                        Continue For
                    End If

                    Dim dto = datosUno(0)

                    ' Actualizar toast (antes de renderizar)
                    toast.UpdateMessage($"Exportando {i + 1}/{totalAExportar} — {dto.NombreCompleto} (CI {dto.Cedula})")

                    ' Nombre de archivo: "Nombre Completo - CI ... .pdf" (o fallback Ficha_{id}.pdf)
                    Dim nombreArchivo As String = ConstruirNombreArchivo(dto, id)
                    Dim ruta As String = ObtenerRutaUnica(carpeta, nombreArchivo)

                    Try
                        ' Render en segundo plano usando LocalReport (sin ReportViewer)
                        Await Task.Run(
                        Sub()
                            Dim lr As New Microsoft.Reporting.WinForms.LocalReport()

                            ' RDLC: Embedded → BaseDirectory\Reportes → StartupPath\Reportes → ClickOnce → extra (..\..)
                            CargarDefinicionRDLC(lr, "FichaFuncional.rdlc", "..\..\Reportes\FichaFuncional.rdlc")

                            lr.DataSources.Clear()
                            lr.DataSources.Add(New Microsoft.Reporting.WinForms.ReportDataSource(
                                "FichaFuncionalDataSet", datosUno))

                            Dim bytes = lr.Render("PDF")
                            If bytes Is Nothing OrElse bytes.Length = 0 Then
                                Throw New ApplicationException("Render devolvió 0 bytes (verificar DataSet y RDLC).")
                            End If

                            System.IO.File.WriteAllBytes(ruta, bytes)
                            lr.ReleaseSandboxAppDomain()
                        End Sub
                    )
                        exportados += 1
                    Catch exUno As Exception
                        fallos.Add($"Id {id} — {dto.NombreCompleto}: {exUno.Message}")
                    End Try
                Next

                ' 5) Mensaje final (toast + MessageBox opcional)
                Me.Text = tituloOriginal
                Cursor = Cursors.Default

                Dim resumen As New System.Text.StringBuilder()
                resumen.AppendLine("Exportación finalizada.")
                resumen.AppendLine($"Se guardaron {exportados} de {totalAExportar} archivos PDF en: {carpeta}")
                If noEncontrados.Any() Then
                    resumen.AppendLine().AppendLine("Sin datos para IDs: " & String.Join(", ", noEncontrados))
                End If
                If fallos.Any() Then
                    resumen.AppendLine().AppendLine("Errores:").AppendLine(" - " & String.Join(Environment.NewLine & " - ", fallos))
                End If

                ' Actualizar y cerrar toast
                toast.UpdateMessage($"Listo: {exportados}/{totalAExportar} guardados en: {carpeta}")
                toast.CloseAfter(3000)

                ' (Opcional) también mostrar MessageBox resumen
                MessageBox.Show(resumen.ToString(), "Apex",
                            MessageBoxButtons.OK,
                            If(fallos.Any(), MessageBoxIcon.Warning, MessageBoxIcon.Information))

                ' (Opcional) abrir la carpeta al terminar
                Try : Process.Start("explorer.exe", carpeta) : Catch : End Try
            End Using

        Catch ex As Exception
            Cursor = Cursors.Default
            Me.Text = "Error al exportar"
            MessageBox.Show("Error al exportar fichas: " & ex.Message, "Apex",
                        MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub CargarDefinicionRDLC(lr As Microsoft.Reporting.WinForms.LocalReport,
                                 nombreCorto As String,
                                 rutaRelativa As String)

        If lr Is Nothing Then Throw New ArgumentNullException(NameOf(lr))
        If String.IsNullOrWhiteSpace(nombreCorto) Then Throw New ArgumentNullException(NameOf(nombreCorto))

        ' armar nombre de recurso embebido: Apex.Reportes.<archivo>.rdlc
        Dim asmName As String = Me.GetType().Assembly.GetName().Name ' "Apex"
        Dim embeddedResourceName As String = $"{asmName}.Reportes.{nombreCorto}"

        ' rutas extra (debug o alternativa)
        Dim extras As String() = Nothing
        If Not String.IsNullOrWhiteSpace(rutaRelativa) Then
            extras = New String() {rutaRelativa} ' ej: "..\..\Reportes\FichaFuncional.rdlc" o "Reportes\FichaFuncional.rdlc"
        End If

        ReportResourceLoader.LoadLocalReportDefinition(
        lr,
        Me.GetType(),
        embeddedResourceName,   ' <<-- NO Nothing
        nombreCorto,            ' <<-- solo el filename, p.ej. "FichaFuncional.rdlc"
        extras
    )
    End Sub

    'Private Sub CargarDefinicionRDLC(lr As Microsoft.Reporting.WinForms.LocalReport,
    '                             nombreCorto As String,
    '                             rutaRelativa As String)
    '    Dim asm = Me.GetType().Assembly
    '    Dim recurso = asm.GetManifestResourceNames().
    '    FirstOrDefault(Function(n) n.EndsWith("." & nombreCorto, StringComparison.OrdinalIgnoreCase))
    '    If Not String.IsNullOrEmpty(recurso) Then
    '        Using s = asm.GetManifestResourceStream(recurso)
    '            lr.LoadReportDefinition(s)
    '        End Using
    '        Exit Sub
    '    End If

    '    Dim fullPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rutaRelativa)
    '    If Not System.IO.File.Exists(fullPath) Then
    '        Throw New ApplicationException($"No se encontró el RDLC embebido ('...{nombreCorto}') ni en disco: {fullPath}")
    '    End If
    '    lr.ReportPath = fullPath
    'End Sub

#Region "Exportacion fichas"
    Private Function ConstruirNombreArchivo(dto As FichaFuncionalDTO, id As Integer) As String
        Dim nombre As String = (If(dto?.NombreCompleto, "")).Trim()
        Dim ci As String = (If(dto?.Cedula, "")).Trim()

        Dim baseName As String
        If nombre <> "" AndAlso ci <> "" Then
            baseName = $"{nombre} - CI {ci}"
        ElseIf nombre <> "" Then
            baseName = nombre
        ElseIf ci <> "" Then
            baseName = $"CI {ci}"
        Else
            baseName = $"Ficha_{id}"
        End If

        baseName = QuitarCaracteresInvalidos(baseName)
        If baseName.Length > 150 Then baseName = baseName.Substring(0, 150) 'evitar rutas largas
        Return baseName & ".pdf"
    End Function

    Private Function QuitarCaracteresInvalidos(s As String) As String
        If s Is Nothing Then Return ""
        For Each ch In IO.Path.GetInvalidFileNameChars()
            s = s.Replace(ch, "_"c)
        Next
        ' colapsar espacios múltiples
        s = System.Text.RegularExpressions.Regex.Replace(s, "\s+", " ").Trim()
        Return s
    End Function

    Private Function ObtenerRutaUnica(carpeta As String, fileName As String) As String
        Dim ruta = IO.Path.Combine(carpeta, fileName)
        Dim n = 2
        While IO.File.Exists(ruta)
            Dim sinExt = IO.Path.GetFileNameWithoutExtension(fileName)
            Dim ext = IO.Path.GetExtension(fileName)
            ruta = IO.Path.Combine(carpeta, $"{sinExt} ({n}){ext}")
            n += 1
        End While
        Return ruta
    End Function

    ' ------------------ Exportar a Excel (CSV) ------------------
    Private Sub btnExportarExcel_Click(sender As Object, e As EventArgs) Handles btnExportarExcel.Click
        Try
            If _dvDatos Is Nothing OrElse _dvDatos.Count = 0 Then
                Notifier.Info(Me, "No hay datos para exportar.", 2000)
                Return
            End If

            ' Columnas: solo las VISIBLES en la grilla y en su ORDEN actual
            Dim cols = dgvDatos.Columns.Cast(Of DataGridViewColumn)().
                       Where(Function(c) c.Visible AndAlso Not String.IsNullOrEmpty(c.DataPropertyName) AndAlso
                                          _dvDatos.Table.Columns.Contains(c.DataPropertyName)).
                       OrderBy(Function(c) c.DisplayIndex).
                       Select(Function(c) New With {.Name = c.DataPropertyName, .Header = c.HeaderText}).
                       ToList()

            If cols.Count = 0 Then
                MessageBox.Show("No hay columnas visibles para exportar.", "Apex",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' File dialog
            Using sfd As New SaveFileDialog()
                sfd.Title = "Guardar como"
                sfd.Filter = "CSV (separado por comas)|*.csv|Todos los archivos|*.*"
                sfd.FileName = $"Export_{Date.Now:yyyyMMdd_HHmm}.csv"
                If sfd.ShowDialog(Me) <> DialogResult.OK Then Return

                Cursor = Cursors.WaitCursor

                ' Construir CSV
                Dim sb As New System.Text.StringBuilder(1024)

                ' Encabezados
                sb.AppendLine(String.Join(",", cols.Select(Function(c) CsvEscape(c.Header))))

                ' Filas visibles (según _dvDatos con filtros aplicados)
                For Each rv As DataRowView In _dvDatos
                    Dim fields As New List(Of String)(cols.Count)
                    For Each c In cols
                        Dim val = rv.Row(c.Name)
                        Dim s As String = If(val Is Nothing OrElse val Is DBNull.Value, "",
                                             If(TypeOf val Is Date OrElse TypeOf val Is DateTime,
                                                DirectCast(val, DateTime).ToString("dd/MM/yyyy"),
                                                val.ToString()))
                        fields.Add(CsvEscape(s))
                    Next
                    sb.AppendLine(String.Join(",", fields))
                Next

                ' Escribir con UTF-8 BOM para que Excel detecte bien la codificación
                Dim bytes = New System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier:=True).GetBytes(sb.ToString())
                System.IO.File.WriteAllBytes(sfd.FileName, bytes)

                Cursor = Cursors.Default
                Notifier.Success(Me, "Datos exportados a CSV.", 2000)

                ' (Opcional) Abrir el archivo
                Try : Process.Start("explorer.exe", "/select," & sfd.FileName) : Catch : End Try
            End Using

        Catch ex As Exception
            Cursor = Cursors.Default
            MessageBox.Show("Error al exportar a Excel: " & ex.Message, "Apex",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Escapa un campo para CSV: comillas dobles, comas, saltos de línea, etc.
    Private Function CsvEscape(s As String) As String
        If s Is Nothing Then Return ""
        Dim mustQuote = s.Contains(",") OrElse s.Contains("""") OrElse s.Contains(ControlChars.Cr) OrElse s.Contains(ControlChars.Lf)
        s = s.Replace("""", """""")
        If mustQuote Then s = """" & s & """"
        Return s
    End Function


#End Region
#End Region
End Class

Public Module ControlExtensions
    <System.Runtime.CompilerServices.Extension()>
    Public Sub DoubleBuffered(ByVal control As System.Windows.Forms.Control, ByVal enable As Boolean)
        Dim prop = control.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic)
        prop.SetValue(control, enable, Nothing)
    End Sub
End Module
