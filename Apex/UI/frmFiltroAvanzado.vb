' Reemplaza todo el contenido de tu archivo Apex/UI/frmFiltroAvanzado.vb con este código.

Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Partial Public Class frmFiltroAvanzado

#Region "Modelos auxiliares"
    Friend Enum OperadorComparacion
        Igual
        Contiene
        EmpiezaCon
        TerminaCon
        Mayor
        MayorIgual
        Menor
        MenorIgual
        Entre
        EnLista
    End Enum

    Friend Class ReglaFiltro
        Public Property Columna As String = String.Empty
        Public Property Operador As OperadorComparacion
        Public Property Valor1 As String = String.Empty
        Public Property Valor2 As String = String.Empty

        Public Overrides Function ToString() As String
            Dim op = [Enum].GetName(GetType(OperadorComparacion), Operador)
            Return If(Operador = OperadorComparacion.Entre,
                       $"{Columna} {op} ({Valor1} – {Valor2})",
                       $"{Columna} {op} {Valor1}")
        End Function

        Private Const FORMAT_DATE As String = "#yyyy-MM-dd#"

        Private Shared Function Esc(s As String) As String
            Return s.Replace("'", "''")
        End Function

        Private Shared Function Formatear(valor As String) As String
            If DateTime.TryParse(valor, Nothing) Then
                Return CDate(valor).ToString(FORMAT_DATE, CultureInfo.InvariantCulture)
            End If

            Dim n As Double
            If Double.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, n) Then
                Return n.ToString(CultureInfo.InvariantCulture)
            End If

            Return $"'{Esc(valor)}'"
        End Function

        Public Function ToRowFilter() As String
            Dim col = $"[{Columna}]"

            Select Case Operador
                Case OperadorComparacion.Igual
                    If DateTime.TryParse(Valor1, Nothing) Then
                        Dim fecha As Date = CDate(Valor1).Date
                        Dim siguienteDia As Date = fecha.AddDays(1)
                        Return $"{col} >= #{fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}# AND {col} < #{siguienteDia.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}#"
                    Else
                        Return $"{col} = {Formatear(Valor1)}"
                    End If

                Case OperadorComparacion.EnLista
                    Dim items = Valor1.Split("|"c).Select(AddressOf Formatear)
                    Return $"{col} IN ({String.Join(",", items)})"

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
            If _reglas.Count = 0 Then Return String.Empty
            Return String.Join(" AND ", _reglas.Select(Function(x) x.ToRowFilter()))
        End Function
    End Class
#End Region

#Region "Campos"
    Private dtOriginal As DataTable = Nothing
    Private dvDatos As DataView = Nothing
    Private ReadOnly filtros As New GestorFiltros()
#End Region

#Region "Constructor / Load"
    Private Sub frmFiltroAvanzado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmbOrigenDatos.DataSource = [Enum].GetValues(GetType(ConsultasGenericas.TipoOrigenDatos))
        cmbOrigenDatos.SelectedIndex = 0
        dtpFechaInicio.Value = Date.Today.AddMonths(-1)
        dtpFechaFin.Value = Date.Today
        dgvDatos.DataSource = Nothing

        ' Handlers fijos
        AddHandler lstColumnas.SelectedIndexChanged, AddressOf ColumnaCambiada
        AddHandler btnAgregar.Click, AddressOf BtnAgregar_Click
        AddHandler btnLimpiar.Click, AddressOf BtnLimpiar_Click
        AddHandler txtBusquedaGlobal.TextChanged, AddressOf txtBusquedaGlobal_TextChanged_Handler
        AddHandler cmbOrigenDatos.SelectedIndexChanged, AddressOf cmbOrigenDatos_SelectedIndexChanged
        cmbOrigenDatos_SelectedIndexChanged(cmbOrigenDatos, EventArgs.Empty)
    End Sub
#End Region

#Region "Carga de datos"
    Private Async Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
        Dim origenSeleccionado = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos)
        Dim fechaInicio = dtpFechaInicio.Value.Date
        Dim fechaFin As Date

        If origenSeleccionado = ConsultasGenericas.TipoOrigenDatos.Funcionarios Then
            fechaFin = fechaInicio
        Else
            fechaFin = dtpFechaFin.Value.Date
            If fechaInicio > fechaFin Then
                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        End If

        btnCargar.Enabled = False
        lblConteoRegistros.Text = "Buscando..."
        lblConteoRegistros.Refresh()

        Try
            Using wait As New WaitCursor()
                dtOriginal = Await ConsultasGenericas.ObtenerDatosGenericosAsync(origenSeleccionado, fechaInicio, fechaFin)
            End Using

            If dtOriginal Is Nothing OrElse dtOriginal.Rows.Count = 0 Then
                MessageBox.Show("No se encontraron datos para los criterios seleccionados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LimpiarTodo()
                Return
            End If

            dvDatos = New DataView(dtOriginal)
            dgvDatos.DataSource = dvDatos

            If origenSeleccionado = ConsultasGenericas.TipoOrigenDatos.Notificaciones Then
                AgregarColumnaEstadoTransitorio()
            End If

            lstColumnas.Items.Clear()
            For Each col As DataColumn In dtOriginal.Columns
                lstColumnas.Items.Add(col.ColumnName)
            Next
            If lstColumnas.Items.Count > 0 Then lstColumnas.SelectedIndex = 0

            filtros.Limpiar()
            flpChips.Controls.Clear()
            txtBusquedaGlobal.Clear()
            AplicarFiltros()

            lstColumnas.Enabled = True
            lstValores.Enabled = True
            btnAgregar.Enabled = True
            btnLimpiar.Enabled = True

        Catch ex As Exception
            MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnCargar.Enabled = True
        End Try
    End Sub

    Private Sub LimpiarTodo()
        dgvDatos.DataSource = Nothing
        lstColumnas.Items.Clear()
        lstValores.Items.Clear()
        flpChips.Controls.Clear()
        filtros.Limpiar()
    End Sub
#End Region

#Region "Eventos principales"
    Private Sub ColumnaCambiada(sender As Object, e As EventArgs)
        lstValores.Items.Clear()
        If lstColumnas.SelectedItem Is Nothing OrElse dvDatos Is Nothing Then Exit Sub

        Dim col = lstColumnas.SelectedItem.ToString()

        Dim valores = dvDatos.ToTable(True, col).AsEnumerable() _
                      .Select(Function(r) r(0).ToString()) _
                      .Where(Function(v) Not String.IsNullOrEmpty(v)) _
                      .Distinct(StringComparer.CurrentCultureIgnoreCase) _
                      .OrderBy(Function(v) v, StringComparer.CurrentCultureIgnoreCase) _
                      .ToArray()

        lstValores.Items.AddRange(valores)
    End Sub

    Private Sub cmbOrigenDatos_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim origenSeleccionado = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos)
        If origenSeleccionado = ConsultasGenericas.TipoOrigenDatos.Funcionarios Then
            dtpFechaInicio.Value = Date.Today
            dtpFechaFin.Value = Date.Today
            dtpFechaFin.Enabled = False
        Else
            dtpFechaInicio.Value = Date.Today.AddMonths(-1)
            dtpFechaFin.Value = Date.Today
            dtpFechaFin.Enabled = True
        End If
    End Sub

    ' Reemplaza esta función completa en tu archivo frmFiltroAvanzado.vb

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If lstColumnas.SelectedItem Is Nothing OrElse lstValores.SelectedItems.Count = 0 Then Return

        Dim col As String = lstColumnas.SelectedItem.ToString()
        Dim selCount As Integer = lstValores.SelectedItems.Count
        Dim nuevaRegla As ReglaFiltro

        ' 1. Creamos la nueva regla que se va a agregar
        If selCount > 1 Then
            Dim valores = lstValores.SelectedItems.Cast(Of Object)().Select(Function(v) v.ToString()).ToArray()
            nuevaRegla = New ReglaFiltro With {
            .Columna = col,
            .Operador = OperadorComparacion.EnLista,
            .Valor1 = String.Join("|", valores)
        }
        Else
            Dim v As String = lstValores.SelectedItem.ToString()
            nuevaRegla = New ReglaFiltro With {
            .Columna = col,
            .Operador = OperadorComparacion.Igual,
            .Valor1 = v
        }
        End If

        ' --- INICIO DE LA MEJORA ---
        ' 2. Verificamos si ya existe una regla idéntica en la lista de filtros
        Dim filtroYaExiste As Boolean = filtros.Reglas.Any(
        Function(reglaExistente)
            Return reglaExistente.Columna.Equals(nuevaRegla.Columna, StringComparison.InvariantCultureIgnoreCase) AndAlso
                   reglaExistente.Operador = nuevaRegla.Operador AndAlso
                   reglaExistente.Valor1.Equals(nuevaRegla.Valor1, StringComparison.InvariantCultureIgnoreCase)
        End Function)

        If filtroYaExiste Then
            MessageBox.Show("Ese filtro ya ha sido aplicado.", "Filtro Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return ' Salimos de la subrutina para no agregarlo de nuevo
        End If
        ' --- FIN DE LA MEJORA ---

        ' 3. Si el filtro no existe, lo agregamos como antes
        filtros.Agregar(nuevaRegla)
        CrearChip(nuevaRegla)
        AplicarFiltros()
    End Sub

    Private Sub txtBusquedaGlobal_TextChanged_Handler(sender As Object, e As EventArgs)
        AplicarFiltros()
    End Sub
#End Region

#Region "Filtrado"
    Private Sub AplicarFiltros()
        If dvDatos Is Nothing Then Return

        Dim rf = filtros.RowFilter()
        Dim globalF = ConstruirFiltroGlobal()
        dvDatos.RowFilter = String.Join(" AND ", {rf, globalF}.Where(Function(s) Not String.IsNullOrWhiteSpace(s)))
        Text = $"Filas: {dvDatos.Count} / {dtOriginal.Rows.Count}"
        lblConteoRegistros.Text = $"Registros encontrados: {dvDatos.Count}"
        ColumnaCambiada(Nothing, EventArgs.Empty)
    End Sub

    Private Function ConstruirFiltroGlobal() As String
        Dim t = txtBusquedaGlobal.Text.Trim()
        If t = String.Empty Then Return String.Empty

        Dim palabras = t.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim cond As New List(Of String)()
        For Each col As DataColumn In dtOriginal.Columns
            If col.DataType = GetType(String) Then
                For Each p In palabras
                    cond.Add($"[{col.ColumnName}] LIKE '%{p.Replace("'", "''")}%'")
                Next
            End If
        Next
        Return "(" & String.Join(" OR ", cond) & ")"
    End Function
#End Region

#Region "Chips UI"
    ' Reemplaza la función existente en la región #Region "Chips UI"

    Private Sub CrearChip(regla As ReglaFiltro)
        Dim descripcionValores As String = If(regla.Operador = OperadorComparacion.EnLista,
                                         regla.Valor1.Replace("|", ", "),
                                         regla.Valor1)

        Dim reglaDesc As String = $"{regla.Columna}: {descripcionValores}"

        ' 1. El chip ahora es un FlowLayoutPanel, que se encargará del layout automáticamente.
        Dim chipContainer As New FlowLayoutPanel() With {
        .AutoSize = True,
        .AutoSizeMode = AutoSizeMode.GrowAndShrink, ' Se ajusta al contenido
        .Margin = New Padding(3),
        .BackColor = Color.FromArgb(220, 235, 255), ' Un azul claro
        .BorderStyle = BorderStyle.FixedSingle,
        .FlowDirection = FlowDirection.LeftToRight,
        .WrapContents = False ' Para que la etiqueta y el botón siempre estén en la misma línea
    }

        ' 2. La etiqueta para el texto del filtro
        Dim lblTexto As New Label() With {
        .Text = reglaDesc,
        .AutoSize = True,
        .Margin = New Padding(3),
        .TextAlign = ContentAlignment.MiddleLeft
    }

        ' 3. El botón para cerrar
        Dim btnCerrar As New Button() With {
        .Text = "×",
        .Font = New Font("Segoe UI", 8, FontStyle.Bold),
        .ForeColor = Color.DarkRed,
        .FlatStyle = FlatStyle.Flat,
        .Size = New Size(22, 22),
        .Tag = regla, ' Guardamos la regla para saber cuál quitar
        .Margin = New Padding(3, 1, 1, 1)
    }
        btnCerrar.FlatAppearance.BorderSize = 0

        AddHandler btnCerrar.Click, AddressOf ChipCerrar_Click

        ' 4. Añadimos los controles al chip. El FlowLayoutPanel los ordenará solo.
        chipContainer.Controls.Add(lblTexto)
        chipContainer.Controls.Add(btnCerrar)

        ' 5. Añadimos el chip completo al panel principal
        flpChips.Controls.Add(chipContainer)
    End Sub

    Private Sub ChipCerrar_Click(sender As Object, e As EventArgs)
        Dim btn = CType(sender, Button)
        Dim reglaParaQuitar = CType(btn.Tag, ReglaFiltro)

        If reglaParaQuitar IsNot Nothing Then
            filtros.Quitar(reglaParaQuitar)
            flpChips.Controls.Remove(btn.Parent)
            AplicarFiltros()
        End If
    End Sub

    Private Sub BtnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        filtros.Limpiar()
        flpChips.Controls.Clear()
        txtBusquedaGlobal.Clear()
        AplicarFiltros()
    End Sub
#End Region

#Region "Utilidades y Exportación"
    ' Pequeña clase utilitaria para mostrar cursor de espera
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

    ' Copia todos los correos (columna “Correo”) del resultado filtrado
    Private Sub btnCopiarCorreos_Click(sender As Object, e As EventArgs) _
            Handles btnCopiarCorreos.Click

        If dvDatos Is Nothing OrElse dvDatos.Count = 0 Then
            MessageBox.Show("No hay datos para procesar.", "Información",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If Not dtOriginal.Columns.Contains("Correo") Then
            MessageBox.Show("La vista actual no contiene una columna 'Correo'.", "Aviso",
                           MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim correos = dvDatos.ToTable().AsEnumerable().
                      Select(Function(r) r.Field(Of String)("Correo")).
                      Where(Function(c) Not String.IsNullOrWhiteSpace(c)).
                      Distinct(StringComparer.InvariantCultureIgnoreCase).
                      ToArray()

        If correos.Length = 0 Then
            MessageBox.Show("No se encontraron correos en el resultado.",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim textoCorreos As String = String.Join("; ", correos)
        Clipboard.SetText(textoCorreos)

        MessageBox.Show($"Se copiaron {correos.Length} correos al portapapeles." &
                        $"{Environment.NewLine}Ya puedes pegarlos en tu cliente de correo.",
                        "Correos copiados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub AgregarColumnaEstadoTransitorio()
        If dgvDatos.Columns.Contains("colEstadoTransitorio") Then Exit Sub

        Dim cmb As New DataGridViewComboBoxColumn() With {
            .Name = "colEstadoTransitorio",
            .HeaderText = "Estado transitorio",
            .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
            .FlatStyle = FlatStyle.Flat,
            .ReadOnly = False
        }
        cmb.Items.AddRange(New String() {
            "Designación", "Sanción", "Sumario",
            "Orden 5", "Enfermedad", "Retén"
        })
        dgvDatos.Columns.Insert(0, cmb)
    End Sub

    Private Sub dgvDatos_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDatos.CellClick
        If e.RowIndex < 0 Then Return
        If dgvDatos.Columns(e.ColumnIndex).Name <> "colEstadoTransitorio" Then Return

        dgvDatos.BeginEdit(True)
        Dim cb = TryCast(dgvDatos.EditingControl, DataGridViewComboBoxEditingControl)
        If cb IsNot Nothing Then
            cb.DroppedDown = True
        End If
    End Sub

    Private Function CodigoInterno(texto As String) As String
        Select Case texto
            Case "Designación" : Return "D"
            Case "Sanción" : Return "N"
            Case "Sumario" : Return "S"
            Case "Orden 5" : Return "O"
            Case "Enfermedad" : Return "E"
            Case "Retén" : Return "R"
            Case Else : Return Nothing
        End Select
    End Function
#End Region

End Class
'Option Strict On
'Option Explicit On

'Imports System.ComponentModel
'Imports System.Data
'Imports System.Globalization
'Imports System.IO
'Imports System.Runtime.InteropServices
'Imports System.Text


''#########################################################################
''  Filtro avanzado – lógica versión ListBox
''#########################################################################

'Partial Public Class frmFiltroAvanzado

'#Region "Modelos auxiliares"
'    Friend Enum OperadorComparacion
'        Igual
'        Contiene
'        EmpiezaCon
'        TerminaCon
'        Mayor
'        MayorIgual
'        Menor
'        MenorIgual
'        Entre
'        EnLista
'    End Enum

'    Friend Class ReglaFiltro
'        Public Property Columna As String = String.Empty
'        Public Property Operador As OperadorComparacion
'        Public Property Valor1 As String = String.Empty
'        Public Property Valor2 As String = String.Empty

'        Public Overrides Function ToString() As String
'            Dim op = [Enum].GetName(GetType(OperadorComparacion), Operador)
'            Return If(Operador = OperadorComparacion.Entre,
'                       $"{Columna} {op} ({Valor1} – {Valor2})",
'                       $"{Columna} {op} {Valor1}")
'        End Function

'        Private Const FORMAT_DATE As String = "#yyyy-MM-dd#"

'        Private Shared Function Esc(s As String) As String
'            Return s.Replace("'", "''")
'        End Function

'        Private Shared Function Formatear(valor As String) As String
'            If DateTime.TryParse(valor, Nothing) Then
'                Return CDate(valor).ToString(FORMAT_DATE, CultureInfo.InvariantCulture)
'            End If

'            Dim n As Double
'            If Double.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, n) Then
'                Return n.ToString(CultureInfo.InvariantCulture)
'            End If

'            Return $"'{Esc(valor)}'"
'        End Function

'        Public Function ToRowFilter() As String
'            Dim col = $"[{Columna}]"

'            Select Case Operador
'                Case OperadorComparacion.Igual
'                    ' Manejo especial para fechas
'                    If DateTime.TryParse(Valor1, Nothing) Then
'                        Dim fecha As Date = CDate(Valor1).Date
'                        Dim siguienteDia As Date = fecha.AddDays(1)
'                        Return $"{col} >= #{fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}# AND {col} < #{siguienteDia.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}#"
'                    Else
'                        Return $"{col} = {Formatear(Valor1)}"
'                    End If

'                Case OperadorComparacion.EnLista
'                    Dim items = Valor1.Split("|"c).Select(AddressOf Formatear)
'                    Return $"{col} IN ({String.Join(",", items)})"

'                Case Else
'                    Throw New NotSupportedException($"Operador {Operador} aún no implementado.")
'            End Select
'        End Function

'    End Class

'    '-----------------------------------------------------------------
'    ' GestorFiltros: administra la colección de reglas activas
'    '-----------------------------------------------------------------
'    Friend Class GestorFiltros
'        Private ReadOnly _reglas As New BindingList(Of ReglaFiltro)()
'        Private ReadOnly historial As New Stack(Of List(Of ReglaFiltro))()

'        Public ReadOnly Property Reglas As BindingList(Of ReglaFiltro)
'            Get
'                Return _reglas
'            End Get
'        End Property

'        Private Shared Function Clonar(lista As IEnumerable(Of ReglaFiltro)) As List(Of ReglaFiltro)
'            Return lista.Select(Function(r) New ReglaFiltro With {
'                        .Columna = r.Columna,
'                        .Operador = r.Operador,
'                        .Valor1 = r.Valor1,
'                        .Valor2 = r.Valor2}).ToList()
'        End Function

'        Public Sub Limpiar()
'            _reglas.Clear()
'            historial.Clear()
'        End Sub

'        Public Sub Agregar(r As ReglaFiltro)
'            historial.Push(Clonar(_reglas))
'            _reglas.Add(r)
'        End Sub

'        Public Sub Quitar(r As ReglaFiltro)
'            historial.Push(Clonar(_reglas))
'            _reglas.Remove(r)
'        End Sub

'        Public Function Retroceder() As Boolean
'            If historial.Count = 0 Then Return False
'            Dim estadoAnterior = historial.Pop()
'            _reglas.Clear()
'            For Each r In estadoAnterior
'                _reglas.Add(r)
'            Next
'            Return True
'        End Function

'        Public Function RowFilter() As String
'            If _reglas.Count = 0 Then Return String.Empty
'            Return String.Join(" AND ", _reglas.Select(Function(x) x.ToRowFilter()))
'        End Function
'    End Class
'#End Region

'#Region "Campos"
'    Private dtOriginal As DataTable = Nothing
'    Private dvDatos As DataView = Nothing
'    Private ReadOnly filtros As New GestorFiltros()

'    ' Relación botón‑regla para mantener Tag íntegro tras Deshacer
'    Private ReadOnly chipsById As New Dictionary(Of Integer, ReglaFiltro)()
'#End Region

'#Region "Constructor / Load"
'    Private Sub frmFiltroAvanzado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        cmbOrigenDatos.DataSource = [Enum].GetValues(GetType(TipoOrigenDatos))
'        cmbOrigenDatos.SelectedIndex = 0
'        dtpFechaInicio.Value = Date.Today.AddMonths(-1)
'        dtpFechaFin.Value = Date.Today
'        dgvDatos.DataSource = Nothing

'        ' Handlers fijos
'        AddHandler lstColumnas.SelectedIndexChanged, AddressOf ColumnaCambiada
'        AddHandler btnAgregar.Click, AddressOf BtnAgregar_Click
'        AddHandler btnLimpiar.Click, AddressOf BtnLimpiar_Click
'        AddHandler txtBusquedaGlobal.TextChanged, AddressOf txtBusquedaGlobal_TextChanged_Handler
'        AddHandler cmbOrigenDatos.SelectedIndexChanged, AddressOf cmbOrigenDatos_SelectedIndexChanged
'        cmbOrigenDatos_SelectedIndexChanged(cmbOrigenDatos, EventArgs.Empty)
'    End Sub
'#End Region

'#Region "Carga de datos"
'    Private Async Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
'        Dim origenSeleccionado = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos)
'        Dim fechaInicio = dtpFechaInicio.Value.Date
'        Dim fechaFin As Date

'        If origenSeleccionado = ConsultasGenericas.TipoOrigenDatos.Funcionarios Then
'            fechaFin = fechaInicio
'        Else
'            fechaFin = dtpFechaFin.Value.Date
'            If fechaInicio > fechaFin Then
'                MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                Return
'            End If
'        End If

'        btnCargar.Enabled = False

'        ' ► Antes de iniciar la consulta, indico al usuario que se está buscando:
'        lblConteoRegistros.Text = "Buscando..."
'        ' Forzar actualización inmediata de la etiqueta (opcional)
'        lblConteoRegistros.Refresh()

'        Try
'            dtOriginal = Await ConsultasGenericas.ObtenerDatosGenericosAsync(origenSeleccionado, fechaInicio, fechaFin)

'            If dtOriginal Is Nothing OrElse dtOriginal.Rows.Count = 0 Then
'                MessageBox.Show("No se encontraron datos para los criterios seleccionados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
'                LimpiarTodo()
'                Return
'            End If

'            dvDatos = New DataView(dtOriginal)
'            dgvDatos.DataSource = dvDatos

'            If origenSeleccionado = TipoOrigenDatos.Notificaciones Then
'                AgregarColumnaEstadoTransitorio()
'            End If

'            lstColumnas.Items.Clear()
'            For Each col As DataColumn In dtOriginal.Columns
'                lstColumnas.Items.Add(col.ColumnName)
'            Next
'            If lstColumnas.Items.Count > 0 Then lstColumnas.SelectedIndex = 0

'            filtros.Limpiar()
'            flpChips.Controls.Clear()
'            txtBusquedaGlobal.Clear()
'            AplicarFiltros()  ' Aquí se actualizará lblConteoRegistros con el conteo real

'            lstColumnas.Enabled = True
'            lstValores.Enabled = True
'            btnAgregar.Enabled = True
'            btnLimpiar.Enabled = True

'        Catch ex As Exception
'            MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        Finally
'            btnCargar.Enabled = True
'        End Try
'    End Sub


'    Private Sub LimpiarTodo()
'        dgvDatos.DataSource = Nothing
'        lstColumnas.Items.Clear()
'        lstValores.Items.Clear()
'        flpChips.Controls.Clear()
'        filtros.Limpiar()
'        chipsById.Clear()
'    End Sub
'#End Region

'#Region "Eventos principales"
'    Private Sub ColumnaCambiada(sender As Object, e As EventArgs)
'        lstValores.Items.Clear()
'        If lstColumnas.SelectedItem Is Nothing OrElse dvDatos Is Nothing Then Exit Sub

'        Dim col = lstColumnas.SelectedItem.ToString()

'        Dim valores = dvDatos.ToTable(False, col).AsEnumerable() _
'                      .Select(Function(r) r(0)) _
'                      .Where(Function(v) v IsNot DBNull.Value) _
'                      .Select(Function(v) v.ToString()) _
'                      .Distinct(StringComparer.CurrentCultureIgnoreCase) _
'                      .OrderBy(Function(v) v, StringComparer.CurrentCultureIgnoreCase) _
'                      .ToArray()

'        lstValores.Items.AddRange(valores)
'    End Sub
'    Private Sub cmbOrigenDatos_SelectedIndexChanged(sender As Object, e As EventArgs)
'        Dim origenSeleccionado = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos)
'        If origenSeleccionado = ConsultasGenericas.TipoOrigenDatos.Funcionarios Then
'            dtpFechaInicio.Value = Date.Today
'            dtpFechaFin.Value = Date.Today
'            dtpFechaFin.Enabled = False
'        Else
'            dtpFechaInicio.Value = Date.Today.AddMonths(-1)
'            dtpFechaFin.Value = Date.Today
'            dtpFechaFin.Enabled = True
'        End If
'    End Sub



'    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs)
'        If lstColumnas.SelectedItem Is Nothing OrElse lstValores.SelectedItems.Count = 0 Then Return

'        Dim col As String = lstColumnas.SelectedItem.ToString()
'        Dim selCount As Integer = lstValores.SelectedItems.Count

'        If selCount > 1 Then
'            Dim valores = lstValores.SelectedItems.Cast(Of Object)().Select(Function(v) v.ToString()).ToArray()
'            Dim regla = New ReglaFiltro With {
'                .Columna = col,
'                .Operador = OperadorComparacion.EnLista,
'                .Valor1 = String.Join("|", valores)
'            }
'            filtros.Agregar(regla)
'            CrearChip(regla, String.Join(", ", valores))
'        Else
'            Dim v As String = lstValores.SelectedItem.ToString()
'            Dim regla = New ReglaFiltro With {
'                .Columna = col,
'                .Operador = OperadorComparacion.Igual,
'                .Valor1 = v
'            }
'            filtros.Agregar(regla)
'            CrearChip(regla, v)
'        End If

'        AplicarFiltros()
'    End Sub

'    Private Sub BtnLimpiar_Click(sender As Object, e As EventArgs)
'        filtros.Limpiar()
'        flpChips.Controls.Clear()
'        chipsById.Clear()
'        txtBusquedaGlobal.Clear()
'        AplicarFiltros()
'    End Sub

'    Private Sub ChipCerrar_Click(sender As Object, e As EventArgs)
'        Dim btn = CType(sender, Button)
'        Dim clave As Integer = CInt(btn.Tag)
'        If chipsById.TryGetValue(clave, Nothing) Then
'            Dim regla = chipsById(clave)
'            filtros.Quitar(regla)
'            chipsById.Remove(clave)
'        End If
'        flpChips.Controls.Remove(btn.Parent)
'        AplicarFiltros()
'    End Sub

'    Private Sub txtBusquedaGlobal_TextChanged_Handler(sender As Object, e As EventArgs)
'        AplicarFiltros()
'    End Sub
'#End Region

'#Region "Filtrado"
'    Private Sub AplicarFiltros()
'        If dvDatos Is Nothing Then Return

'        Dim rf = filtros.RowFilter()
'        Dim globalF = ConstruirFiltroGlobal()
'        dvDatos.RowFilter = String.Join(" AND ", {rf, globalF}.Where(Function(s) Not String.IsNullOrWhiteSpace(s)))
'        Text = $"Filas: {dvDatos.Count} / {dtOriginal.Rows.Count}"
'        lblConteoRegistros.Text = $"Registros encontrados: {dvDatos.Count}"
'        ColumnaCambiada(Nothing, EventArgs.Empty)
'    End Sub

'    Private Function ConstruirFiltroGlobal() As String
'        Dim t = txtBusquedaGlobal.Text.Trim()
'        If t = String.Empty Then Return String.Empty

'        Dim palabras = t.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
'        Dim cond As New List(Of String)()
'        For Each col As DataColumn In dtOriginal.Columns
'            If col.DataType = GetType(String) Then
'                For Each p In palabras
'                    cond.Add($"[{col.ColumnName}] LIKE '%{p.Replace("'", "''")}%'")
'                Next
'            End If
'        Next
'        Return "(" & String.Join(" OR ", cond) & ")"
'    End Function
'#End Region

'#Region "Chips UI"
'    Private Sub CrearChip(regla As ReglaFiltro, descripcionValores As String)
'        Dim reglaDesc As String = If(regla.Operador = OperadorComparacion.EnLista,
'                                     $"{regla.Columna} IN ({descripcionValores})",
'                                     $"{regla.Columna} {regla.Operador} {descripcionValores}")

'        Dim p As New Panel() With {.AutoSize = True}
'        Dim l As New Label() With {.Text = reglaDesc, .AutoSize = True, .Padding = New Padding(3)}
'        Dim x As New Button() With {.Text = "x", .Width = 25, .Height = 25, .Tag = regla.GetHashCode()}

'        chipsById(regla.GetHashCode()) = regla
'        AddHandler x.Click, AddressOf ChipCerrar_Click

'        p.Controls.Add(l)
'        p.Controls.Add(x)
'        flpChips.Controls.Add(p)
'    End Sub

'    Private Sub btnDeshacer_Click(sender As Object, e As EventArgs) Handles btnDeshacer.Click
'        If filtros.Retroceder() Then
'            flpChips.Controls.Clear()
'            chipsById.Clear()
'            For Each r In filtros.Reglas
'                Dim texto = If(r.Operador = OperadorComparacion.EnLista, r.Valor1.Replace("|", ", "), r.Valor1)
'                CrearChip(r, texto)
'            Next
'            AplicarFiltros()
'        Else
'            MessageBox.Show("No hay más pasos para deshacer.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        End If
'    End Sub
'#End Region

'    ' Pequeña clase utilitaria para mostrar cursor de espera
'    Private NotInheritable Class WaitCursor
'        Implements IDisposable
'        Private ReadOnly _old As Cursor
'        Public Sub New()
'            _old = Cursor.Current
'            Cursor.Current = Cursors.WaitCursor
'        End Sub
'        Public Sub Dispose() Implements IDisposable.Dispose
'            Cursor.Current = _old
'        End Sub
'    End Class
'    ' Copia todos los correos (columna “Correo”) del resultado filtrado
'    Private Sub btnCopiarCorreos_Click(sender As Object, e As EventArgs) _
'            Handles btnCopiarCorreos.Click

'        ' 1) Verifica que haya datos filtrados
'        If dvDatos Is Nothing OrElse dvDatos.Count = 0 Then
'            MessageBox.Show("No hay datos para procesar.", "Información",
'                            MessageBoxButtons.OK, MessageBoxIcon.Information)
'            Return
'        End If

'        ' 2) Extrae la columna “Correo”, omite vacíos y quita duplicados
'        Dim correos = dvDatos.ToTable().AsEnumerable().
'                      Select(Function(r) r.Field(Of String)("Correo")).
'                      Where(Function(c) Not String.IsNullOrWhiteSpace(c)).
'                      Distinct(StringComparer.InvariantCultureIgnoreCase).
'                      ToArray()

'        If correos.Length = 0 Then
'            MessageBox.Show("No se encontraron correos en el resultado.",
'                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'            Return
'        End If

'        ' 3) Une con “; ”, típico separador de Outlook / Gmail
'        Dim textoCorreos As String = String.Join("; ", correos)

'        ' 4) Copia al portapapeles
'        Clipboard.SetText(textoCorreos)

'        ' 5) Feedback al usuario
'        MessageBox.Show($"Se copiaron {correos.Length} correos al portapapeles." &
'                        $"{Environment.NewLine}Ya puedes pegarlos en tu cliente de correo.",
'                        "Correos copiados",
'                        MessageBoxButtons.OK, MessageBoxIcon.Information)
'    End Sub
'    '---------------------------------------------------------------
'    ' Crea la columna ComboBox solo si aún no existe
'    '---------------------------------------------------------------
'    Private Sub AgregarColumnaEstadoTransitorio()
'        If dgvDatos.Columns.Contains("colEstadoTransitorio") Then Exit Sub

'        Dim cmb As New DataGridViewComboBoxColumn() With {
'        .Name = "colEstadoTransitorio",
'        .HeaderText = "Estado transitorio",
'        .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
'        .FlatStyle = FlatStyle.Flat,
'        .ReadOnly = False
'    }
'        cmb.Items.AddRange(New String() {
'        "Designación", "Sanción", "Sumario",
'        "Orden 5", "Enfermedad", "Retén"
'    })

'        ' Inserta en la posición 0 (la primera columna):
'        dgvDatos.Columns.Insert(0, cmb)
'    End Sub

'    '-----------------------------------------------------------------
'    ' Abre el ComboBox apenas el usuario hace clic en la celda
'    '-----------------------------------------------------------------
'    Private Sub dgvDatos_CellClick(
'        sender As Object,
'        e As DataGridViewCellEventArgs) _
'    Handles dgvDatos.CellClick

'        If e.RowIndex < 0 Then Return      ' Encabezados

'        ' ¿Es la columna de Estado transitorio?
'        If dgvDatos.Columns(e.ColumnIndex).Name <> "colEstadoTransitorio" Then Return

'        ' Pone la celda en modo edición
'        dgvDatos.BeginEdit(True)

'        ' Forza el despliegue del combo
'        Dim cb = TryCast(dgvDatos.EditingControl, DataGridViewComboBoxEditingControl)
'        If cb IsNot Nothing Then
'            cb.DroppedDown = True
'        End If
'    End Sub

'    Private Function CodigoInterno(texto As String) As String
'        Select Case texto
'            Case "Designación" : Return "D"
'            Case "Sanción" : Return "N"
'            Case "Sumario" : Return "S"
'            Case "Orden 5" : Return "O"
'            Case "Enfermedad" : Return "E"
'            Case "Retén" : Return "R"
'            Case Else : Return Nothing
'        End Select
'    End Function

'End Class