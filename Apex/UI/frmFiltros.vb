' Apex/UI/frmFiltroAvanzado.vb
Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Data
Imports System.Globalization
Imports System.Text

' NOTA: Se eliminaron imports no utilizados como System.IO y System.Runtime.InteropServices
' para mantener el código limpio.

Partial Public Class frmFiltros
    Private _dtOriginal As DataTable = New DataTable()
    Private _dvDatos As DataView = Nothing
    Private ReadOnly _filtros As New GestorFiltros()

    ' --- MEJORA: Campo para evitar la actualización redundante de la lista de valores ---
    Private _isUpdatingValores As Boolean = False

#Region "Modelos y Clases de Ayuda"

    Public Enum OperadorComparacion
        Igual
        EnLista
        ' Se pueden agregar más operadores aquí si es necesario
    End Enum

    Public Class ReglaFiltro
        Public Property Columna As String = String.Empty
        Public Property Operador As OperadorComparacion
        Public Property Valor1 As String = String.Empty
        Public Property Valor2 As String = String.Empty ' Reservado para operadores como 'Entre'

        Public Overrides Function ToString() As String
            Dim opStr = System.Enum.GetName(GetType(OperadorComparacion), Me.Operador)
            If Operador = OperadorComparacion.EnLista Then
                ' Para 'EnLista', el valor puede ser muy largo. Mostramos un resumen.
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
                ' CORRECCIÓN: Usar # en lugar de ' para las fechas.
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
                        ' --- MEJORA: Filtrado de fechas para cubrir el día completo ---
                        Dim fecha As Date = Convert.ToDateTime(Valor1).Date
                        Dim siguienteDia As Date = fecha.AddDays(1)
                        Return $"({colName} >= #{fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}# AND {colName} < #{siguienteDia.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}#)"
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
        ' >>> INICIO DE LÍNEAS NUEVAS <<<
        ConfigurarLayoutResponsivo()
        AjustarSplitter()
        ' >>> FIN DE LÍNEAS NUEVAS <<<

        ' Tema base
        AppTheme.Aplicar(Me)

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

        ' Action bars alineadas a la derecha
        pnlFiltroBotones.FlowDirection = FlowDirection.RightToLeft
        flpAcciones.FlowDirection = FlowDirection.RightToLeft

        ' Botones “neutros” (mantienen BackColor del tema)
        btnLimpiar.Tag = "KeepBackColor"
        btnExportarExcel.Tag = "KeepBackColor"
        btnCopiarCorreos.Tag = "KeepBackColor"
        btnExportarFichasPDF.Tag = "KeepBackColor"

        ' Placeholder en la búsqueda (si agregaste AppTheme.SetCue)
        Try
            AppTheme.SetCue(txtBusquedaGlobal, "Buscar en todos los campos…")
        Catch
            ' Ignorar si no existe SetCue
        End Try

        ' Atajos
        Me.AcceptButton = btnFiltrar
        Me.KeyPreview = True
        AddHandler Me.KeyDown, AddressOf Form_KeyDown_EscCierra

        ' Eventos
        AddHandler btnCargar.Click, AddressOf btnCargar_Click_Async
        AddHandler lstColumnas.SelectedIndexChanged, AddressOf LstColumnas_SelectedIndexChanged
        AddHandler txtBusquedaGlobal.TextChanged, AddressOf TxtBusquedaGlobal_TextChanged

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
                ' --- MEJORA: Añadir columna de búsqueda global para optimizar el rendimiento ---
                AñadirColumnaBusquedaGlobal(_dtOriginal)
                _dvDatos = _dtOriginal.DefaultView
            End Using

            ' Resetea el estado del formulario antes de cargar nuevos datos
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
        ' Esta columna invisible concatena todos los campos de texto para una búsqueda global ultrarrápida.
        Const SEARCH_COLUMN_NAME As String = "GlobalSearch"
        If dt Is Nothing OrElse dt.Columns.Contains(SEARCH_COLUMN_NAME) Then Return

        dt.Columns.Add(SEARCH_COLUMN_NAME, GetType(String))

        Dim stringColumns = dt.Columns.Cast(Of DataColumn).Where(Function(c) c.DataType = GetType(String)).ToList()

        For Each row As DataRow In dt.Rows
            Dim combinedValue As New StringBuilder()
            For Each col In stringColumns
                combinedValue.Append(row(col).ToString() & " ")
            Next
            row(SEARCH_COLUMN_NAME) = combinedValue.ToString()
        Next
    End Sub

    Private Sub ConfigurarGrilla(dt As DataTable)
        dgvDatos.DataSource = Nothing
        dgvDatos.Columns.Clear()
        dgvDatos.AutoGenerateColumns = False

        If dt Is Nothing Then Return

        ' --- MEJORA: Definiciones de columnas centralizadas para mayor claridad ---
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
            ' Omitir columnas 'Id' y la columna de búsqueda global
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
        ' >>> INICIO DE MEJORA VISUAL <<<
        ' Hacer que la última columna visible llene el espacio restante.
        Dim ultimaColumnaVisible = dgvDatos.Columns.Cast(Of DataGridViewColumn).LastOrDefault(Function(c) c.Visible)
        If ultimaColumnaVisible IsNot Nothing Then
            ultimaColumnaVisible.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
        ' >>> FIN DE MEJORA VISUAL <<<
    End Sub

    Private Sub ActualizarListaColumnas()
        lstColumnas.Items.Clear()
        If _dtOriginal IsNot Nothing Then
            Dim columnNames = _dtOriginal.Columns.Cast(Of DataColumn).
                              Select(Function(c) c.ColumnName).
                              Where(Function(name) Not name.ToLower().EndsWith("id") AndAlso name <> "GlobalSearch").
                              ToArray()
            lstColumnas.Items.AddRange(columnNames)
            If lstColumnas.Items.Count > 0 Then lstColumnas.SelectedIndex = 0
        End If
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

        ' --- MEJORA: Lógica de validación movida a su propia función para mayor claridad ---
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
        ' Si el usuario selecciona todos los ítems, el filtro no tiene efecto.
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

        ' Se aplica el filtro combinado a la vista de datos
        _dvDatos.RowFilter = String.Join(" AND ", {filtroReglas, filtroGlobal}.Where(Function(s) Not String.IsNullOrWhiteSpace(s)))

        ActualizarListaDeValores()

        ' El resto del código se mantiene igual
        UpdateUIState()
    End Sub

    Private Function ConstruirFiltroGlobal() As String
        Dim searchText = txtBusquedaGlobal.Text.Trim()
        If String.IsNullOrWhiteSpace(searchText) Then Return String.Empty

        ' --- MEJORA: La búsqueda ahora se hace sobre la columna pre-calculada 'GlobalSearch' ---
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

            ' Selecciona la columna y valores del chip eliminado para una mejor experiencia de usuario
            RestaurarSeleccionDesdeChip(regla)

            AplicarFiltros()
            UpdateFiltrosPanelHeight()
        End If
    End Sub

    Private Sub RestaurarSeleccionDesdeChip(regla As ReglaFiltro)
        _isUpdatingValores = True ' Evita que el evento SelectedIndexChanged se dispare dos veces
        lstColumnas.SelectedItem = regla.Columna
        ActualizarListaDeValores() ' Actualiza los valores para la columna seleccionada

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
        Dim hayDatos = (_dvDatos IsNot Nothing AndAlso _dvDatos.Table.Rows.Count > 0)
        gbxFiltros.Enabled = hayDatos

        ' Asegura que el separador solo se muestre si hay botones a ambos lados
        Separator1.Visible = (btnExportarExcel.Visible Or btnCopiarCorreos.Visible)

        ' Actualiza el conteo de registros
        If _dvDatos IsNot Nothing Then
            lblConteoRegistros.Text = $"Registros: {_dvDatos.Count}"
        Else
            lblConteoRegistros.Text = "Registros: 0"
        End If
        UpdateFiltrosPanelHeight()
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
        lstValores.BeginUpdate() ' Optimiza el rendimiento
        lstValores.Items.Clear()

        ' Verificamos que la columna esté seleccionada y que la vista de datos (_dvDatos) exista
        If lstColumnas.SelectedItem Is Nothing OrElse _dvDatos Is Nothing Then
            lstValores.EndUpdate()
            Return
        End If

        Dim colName = lstColumnas.SelectedItem.ToString()

        Dim dtValoresUnicos As DataTable = _dvDatos.ToTable(True, colName)

        ' Ahora extraemos los valores de esta nueva tabla pequeña y ordenada.
        Dim valoresUnicos = dtValoresUnicos.AsEnumerable().
                        Select(Function(r) r.Field(Of Object)(colName)).
                        Where(Function(v) v IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(v.ToString())).
                        OrderBy(Function(v) v.ToString(), StringComparer.CurrentCultureIgnoreCase).
                        Select(Function(v) v.ToString()).
                        ToArray()

        lstValores.Items.AddRange(valoresUnicos)
        lstValores.EndUpdate()
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
#End Region


    Private Sub btnCopiarCorreos_Click(sender As Object, e As EventArgs) Handles btnCopiarCorreos.Click
        ' 1. Verifica si hay datos filtrados en la vista actual.
        If _dvDatos Is Nothing OrElse _dvDatos.Count = 0 Then
            MessageBox.Show("No hay registros en la vista actual para copiar correos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' 2. Define el nombre de la columna que contiene los correos.
        '    ¡IMPORTANTE! Cambia "Email" por el nombre real de tu columna si es diferente.
        Const NOMBRE_COLUMNA_CORREO As String = "Email"

        ' 3. Comprueba si la columna de correo existe en los datos.
        If Not _dvDatos.Table.Columns.Contains(NOMBRE_COLUMNA_CORREO) Then
            MessageBox.Show($"No se encontró la columna '{NOMBRE_COLUMNA_CORREO}' en los datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' 4. Recolecta todos los correos de la vista filtrada.
        Dim correos As New List(Of String)
        For Each rowView As DataRowView In _dvDatos
            ' Obtiene el valor del correo de la fila actual.
            Dim correo = rowView(NOMBRE_COLUMNA_CORREO)?.ToString()

            ' Agrega el correo a la lista solo si no es nulo y no está vacío.
            If Not String.IsNullOrWhiteSpace(correo) Then
                correos.Add(correo.Trim())
            End If
        Next

        ' 5. Si se encontraron correos, únelos y cópialos al portapapeles.
        If correos.Any() Then
            ' Une todos los correos con un punto y coma, que es el estándar para los clientes de correo.
            Dim correosParaCopiar = String.Join("; ", correos.Distinct()) ' Distinct() para evitar duplicados.

            ' Copia la cadena de correos al portapapeles.
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

#Region "Diseño Responsivo"

    ' Variable para guardar la proporción del splitter elegida por el usuario.
    Private _splitRatio As Double = 0.3 ' Proporción inicial del 30% para el panel de filtros.

    Private Sub ConfigurarLayoutResponsivo()
        ' Configura el SplitContainer para que sea flexible.
        Me.splitContenedorPrincipal.FixedPanel = FixedPanel.None
        Me.splitContenedorPrincipal.IsSplitterFixed = False

        ' Habilita DoubleBuffering en los contenedores principales para un redimensionamiento suave.
        Me.splitContenedorPrincipal.DoubleBuffered(True)
        Me.splitContenedorPrincipal.Panel1.DoubleBuffered(True)
        Me.splitContenedorPrincipal.Panel2.DoubleBuffered(True)
        Me.TableLayoutPanelLeft.DoubleBuffered(True)
        Me.TableLayoutPanelRight.DoubleBuffered(True)
        ' La grilla ya tiene su propio DoubleBuffered, pero no hace daño reasegurarlo.
        Me.dgvDatos.DoubleBuffered(True)

        ' Asocia los eventos de redimensionamiento.
        AddHandler Me.Resize, AddressOf frmFiltros_Resize
        AddHandler Me.splitContenedorPrincipal.SplitterMoved, AddressOf splitContenedorPrincipal_SplitterMoved
    End Sub

    Private Sub frmFiltros_Resize(sender As Object, e As EventArgs)
        ' Cada vez que el formulario cambia de tamaño, ajustamos el splitter.
        AjustarSplitter()
    End Sub

    Private Sub splitContenedorPrincipal_SplitterMoved(sender As Object, e As SplitterEventArgs)
        ' Cuando el usuario mueve el splitter, guardamos la nueva proporción.
        ' Usamos Math.Max para evitar una división por cero si el control es muy pequeño.
        _splitRatio = splitContenedorPrincipal.SplitterDistance / Math.Max(1, splitContenedorPrincipal.Width)
    End Sub

    Private Sub AjustarSplitter()
        ' Aplica la proporción guardada al ancho actual del SplitContainer.
        Dim ancho As Integer = Math.Max(1, splitContenedorPrincipal.Width)
        Dim distanciaDeseada As Integer = CInt(ancho * _splitRatio)

        ' Asegura que la nueva distancia respete los tamaños mínimos de los paneles.
        splitContenedorPrincipal.SplitterDistance = Math.Max(splitContenedorPrincipal.Panel1MinSize,
                                                      Math.Min(distanciaDeseada, ancho - splitContenedorPrincipal.Panel2MinSize))
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
