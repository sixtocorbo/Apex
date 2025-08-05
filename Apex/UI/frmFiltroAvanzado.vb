' Apex/UI/frmFiltroAvanzado.vb
Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports Apex.frmFiltroAvanzado
Imports Apex.ConsultasGenericas

Partial Public Class frmFiltroAvanzado

    ' --- NUEVO: Campo para gestionar las acciones actuales ---
    Private _accionHandler As IAccionHandler

#Region "Modelos y Clases de Ayuda"

    Public Enum OperadorComparacion
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

    Public Class ReglaFiltro
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

        Private Shared Function Esc(s As String) As String
            Return s.Replace("'", "''")
        End Function

        Private Shared Function Formatear(valor As String) As String
            Dim fecha As DateTime
            If DateTime.TryParse(valor, CultureInfo.InvariantCulture, DateTimeStyles.None, fecha) Then
                Return $"'{fecha:yyyy-MM-dd}'"
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

#Region "Campos del Formulario"
    Private dtOriginal As DataTable = New DataTable()
    Private dvDatos As DataView = Nothing
    Private ReadOnly filtros As New GestorFiltros()
#End Region

#Region "Constructor y Eventos Load"
    Private Sub frmFiltroAvanzado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        dgvDatos.SendToBack()
        cmbOrigenDatos.DataSource = [Enum].GetValues(GetType(ConsultasGenericas.TipoOrigenDatos))
        cmbOrigenDatos.SelectedIndex = -1

        gbxBusquedaGlobal.BringToFront()
        flpChips.BringToFront()
        pnlAcciones.BringToFront()

        AddHandler cmbOrigenDatos.SelectedIndexChanged, AddressOf cmbOrigenDatos_SelectedIndexChanged
        AddHandler btnCargar.Click, AddressOf btnCargar_Click
        AddHandler lstColumnas.SelectedIndexChanged, AddressOf ColumnaCambiada
        AddHandler txtBusquedaGlobal.TextChanged, AddressOf txtBusquedaGlobal_TextChanged_Handler

        AddHandler btnNuevaLicencia.Click, AddressOf btnGenerico_Nuevo_Click
        AddHandler btnNuevaNotificacion.Click, AddressOf btnGenerico_Nuevo_Click
        AddHandler btnEditarLicencia.Click, AddressOf btnGenerico_Editar_Click
        AddHandler btnEditarNotificacion.Click, AddressOf btnGenerico_Editar_Click
        AddHandler btnEliminarLicencia.Click, AddressOf btnGenerico_Eliminar_Click
        AddHandler btnEliminarNotificacion.Click, AddressOf btnGenerico_Eliminar_Click
        AddHandler btnCambiarEstado.Click, AddressOf btnGenerico_Extra_Click

        cmbOrigenDatos_SelectedIndexChanged(Nothing, EventArgs.Empty)
        UpdateFiltrosPanelHeight()
    End Sub
#End Region

#Region "Lógica de Carga de Datos"

    ' --- CORRECCIÓN: Cambiado a Public para que sea accesible desde las clases Handler ---
    Public Async Function CargarDatosAsync() As Task
        If cmbOrigenDatos.SelectedItem Is Nothing Then Return

        Dim origen = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos)
        Dim fechaI = dtpFechaInicio.Value.Date
        Dim fechaF = dtpFechaFin.Value.Date

        If origen <> ConsultasGenericas.TipoOrigenDatos.Funcionarios AndAlso fechaI > fechaF Then
            MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnCargar.Enabled = False
        LoadingHelper.MostrarCargando(Me)

        Try
            Using wait As New WaitCursor()
                dtOriginal = Await ConsultasGenericas.ObtenerDatosGenericosAsync(origen, fechaI, fechaF)
                dvDatos = dtOriginal.DefaultView
            End Using

            ConfigurarGrilla(dtOriginal)
            ActualizarListaColumnas()
            filtros.Limpiar()
            flpChips.Controls.Clear()
            UpdateFiltrosPanelHeight()

            dgvDatos.DataSource = dvDatos
            AplicarFiltros()

            gbxFiltros.Enabled = dtOriginal IsNot Nothing AndAlso dtOriginal.Rows.Count > 0

        Catch ex As Exception
            MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LimpiarTodo()
        Finally
            LoadingHelper.OcultarCargando(Me)
            btnCargar.Enabled = True
            ActualizarAccionesDisponibles()
        End Try
    End Function

    Private Async Sub btnCargar_Click(sender As Object, e As EventArgs)
        Await CargarDatosAsync()
    End Sub

    Private Sub ConfigurarGrilla(dt As DataTable)
        dgvDatos.DataSource = Nothing
        dgvDatos.Columns.Clear()
        dgvDatos.AutoGenerateColumns = False

        If dt Is Nothing Then Return

        ' >>> CAMBIO CLAVE #1: Cambiamos el modo de auto-ajuste.
        ' None permite que las barras de scroll aparezcan si el contenido es más ancho.
        dgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None

        Dim origenSeleccionado = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos)

        If origenSeleccionado = ConsultasGenericas.TipoOrigenDatos.Funcionarios Then
            ' Columnas específicas para Funcionarios
            Dim columnasDeseadas As New Dictionary(Of String, String) From {
            {"NombreCompleto", "Nombre"},
            {"Cedula", "Cédula"},
            {"Cargo", "Cargo"},
            {"Escalafon", "Escalafón"},
            {"Seccion", "Sección"},
            {"Turno", "Turno"},
            {"Semana", "Semana"},
            {"PuestoDeTrabajo", "Puesto de Trabajo"}
        }

            For Each kvp In columnasDeseadas
                If dt.Columns.Contains(kvp.Key) Then
                    Dim dgvCol As New DataGridViewTextBoxColumn With {
                    .DataPropertyName = kvp.Key,
                    .HeaderText = kvp.Value,
                    .Name = kvp.Key
                }

                    ' >>> CAMBIO CLAVE #2: Hacemos que la columna del nombre
                    ' ocupe el espacio restante (Fill), mientras las otras
                    ' se ajustan a su contenido.
                    If kvp.Key = "NombreCompleto" Then
                        dgvCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    Else
                        dgvCol.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
                    End If

                    dgvDatos.Columns.Add(dgvCol)
                End If
            Next
        Else
            ' Comportamiento para otros orígenes de datos
            For Each col As DataColumn In dt.Columns
                Dim dgvCol As New DataGridViewTextBoxColumn With {
                .DataPropertyName = col.ColumnName,
                .HeaderText = col.ColumnName,
                .Name = col.ColumnName,
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells ' Ajuste para otras vistas
            }
                If col.DataType = GetType(Date) OrElse col.DataType = GetType(DateTime) Then
                    dgvCol.DefaultCellStyle.Format = "dd/MM/yyyy"
                End If
                dgvDatos.Columns.Add(dgvCol)
            Next
        End If
    End Sub

    Private Sub ActualizarListaColumnas()
        lstColumnas.Items.Clear()
        lstValores.Items.Clear()
        If dtOriginal IsNot Nothing Then
            For Each col As DataColumn In dtOriginal.Columns
                lstColumnas.Items.Add(col.ColumnName)
            Next
            If lstColumnas.Items.Count > 0 Then lstColumnas.SelectedIndex = 0
        End If
    End Sub

#End Region

#Region "Eventos de UI (Botones, Combos, etc.)"

    Private Sub cmbOrigenDatos_SelectedIndexChanged(sender As Object, e As EventArgs)
        LimpiarTodo()
        If cmbOrigenDatos.SelectedItem IsNot Nothing Then
            Dim origenSeleccionado = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos)
            dtpFechaFin.Enabled = (origenSeleccionado <> ConsultasGenericas.TipoOrigenDatos.Funcionarios)

            Select Case origenSeleccionado
                Case TipoOrigenDatos.Licencias
                    _accionHandler = New LicenciaAccionHandler()
                Case TipoOrigenDatos.Notificaciones
                    _accionHandler = New NotificacionAccionHandler()
                Case Else
                    _accionHandler = Nothing
            End Select
        Else
            _accionHandler = Nothing
        End If
        ActualizarAccionesDisponibles()
    End Sub

    Private Sub LimpiarTodo()
        dgvDatos.DataSource = Nothing
        dgvDatos.Columns.Clear()
        dtOriginal = New DataTable()
        dvDatos = Nothing
        lstColumnas.Items.Clear()
        lstValores.Items.Clear()
        flpChips.Controls.Clear()
        filtros.Limpiar()
        gbxFiltros.Enabled = False
        ActualizarAccionesDisponibles()
        UpdateFiltrosPanelHeight()
    End Sub

    Private Sub ColumnaCambiada(sender As Object, e As EventArgs)
        ActualizarListaDeValores()
    End Sub

    Private Sub ActualizarListaDeValores()
        lstValores.Items.Clear()
        If lstColumnas.SelectedItem Is Nothing OrElse dvDatos Is Nothing OrElse dvDatos.Table Is Nothing Then Exit Sub

        Dim colName = lstColumnas.SelectedItem.ToString()
        Dim valoresUnicosTbl As DataTable = dvDatos.ToTable(True, colName)
        Dim valores = valoresUnicosTbl.AsEnumerable() _
                                .Select(Function(r) r(colName).ToString()) _
                                .Where(Function(v) Not String.IsNullOrWhiteSpace(v)) _
                                .OrderBy(Function(v) v, StringComparer.CurrentCultureIgnoreCase) _
                                .ToArray()

        lstValores.Items.AddRange(valores)
    End Sub


    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If lstColumnas.SelectedItem Is Nothing OrElse lstValores.SelectedItems.Count = 0 Then Return

        Dim col As String = lstColumnas.SelectedItem.ToString()
        Dim selCount As Integer = lstValores.SelectedItems.Count
        Dim nuevaRegla As ReglaFiltro

        If selCount > 1 Then
            Dim valores = lstValores.SelectedItems.Cast(Of Object)().Select(Function(v) v.ToString()).ToArray()
            nuevaRegla = New ReglaFiltro With {.Columna = col, .Operador = OperadorComparacion.EnLista, .Valor1 = String.Join("|", valores)}
        Else
            Dim v As String = lstValores.SelectedItem.ToString()
            nuevaRegla = New ReglaFiltro With {.Columna = col, .Operador = OperadorComparacion.Igual, .Valor1 = v}
        End If

        filtros.Agregar(nuevaRegla)
        CrearChip(nuevaRegla)
        AplicarFiltros()
    End Sub

    Private Sub txtBusquedaGlobal_TextChanged_Handler(sender As Object, e As EventArgs)
        AplicarFiltros()
    End Sub

#End Region

#Region "Filtrado y UI Dinámica"

    Private Sub AplicarFiltros()
        If dvDatos Is Nothing Then Return
        dvDatos.RowFilter = String.Join(" AND ", {filtros.RowFilter(), ConstruirFiltroGlobal()}.Where(Function(s) Not String.IsNullOrWhiteSpace(s)))
        ActualizarListaDeValores()
        ActualizarAccionesDisponibles()
    End Sub

    Private Sub ActualizarAccionesDisponibles()
        Dim hayDatos = (dvDatos IsNot Nothing AndAlso dvDatos.Count > 0)

        ' Ocultamos todos los botones de acciones por defecto
        btnNuevaLicencia.Visible = False
        btnEditarLicencia.Visible = False
        btnEliminarLicencia.Visible = False
        btnNuevaNotificacion.Visible = False
        btnEditarNotificacion.Visible = False
        btnEliminarNotificacion.Visible = False
        btnCambiarEstado.Visible = False

        ' Si hay un gestor de acciones, le delegamos la visibilidad
        If _accionHandler IsNot Nothing Then
            _accionHandler.ConfigurarVisibilidadBotones(Me, hayDatos)
        End If

        Separator1.Visible = btnNuevaLicencia.Visible Or btnNuevaNotificacion.Visible

        If dvDatos IsNot Nothing Then
            lblConteoRegistros.Text = $"Registros encontrados: {dvDatos.Count}"
        Else
            lblConteoRegistros.Text = "Registros encontrados: 0"
        End If
    End Sub

    Private Function ConstruirFiltroGlobal() As String
        Dim t = txtBusquedaGlobal.Text.Trim()
        If t = String.Empty Then Return String.Empty

        Dim palabras = t.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim cond As New List(Of String)()

        If dvDatos IsNot Nothing AndAlso dvDatos.Table IsNot Nothing Then
            For Each col As DataColumn In dvDatos.Table.Columns
                If col.DataType = GetType(String) Then
                    For Each p In palabras
                        cond.Add($"[{col.ColumnName}] LIKE '%{p.Replace("'", "''")}%'")
                    Next
                End If
            Next
        End If

        If cond.Count = 0 Then Return String.Empty
        Return "(" & String.Join(" OR ", cond) & ")"
    End Function

#End Region

#Region "Chips UI"
    Private Sub CrearChip(regla As ReglaFiltro)
        Dim nuevoChip As New ChipControl(regla, flpChips.Width)
        AddHandler nuevoChip.CerrarClick, AddressOf Chip_CerrarClick
        flpChips.Controls.Add(nuevoChip)
        UpdateFiltrosPanelHeight()
    End Sub

    Private Sub Chip_CerrarClick(sender As Object, e As EventArgs)
        Dim chip As ChipControl = CType(sender, ChipControl)
        Dim reglaParaQuitar As ReglaFiltro = chip.Regla

        If reglaParaQuitar IsNot Nothing Then
            filtros.Quitar(reglaParaQuitar)
            flpChips.Controls.Remove(chip)
            AplicarFiltros()
            UpdateFiltrosPanelHeight()
        End If
    End Sub

    Private Sub UpdateFiltrosPanelHeight()
        Const MAX_HEIGHT As Integer = 120
        ' La constante MARGIN_VERTICAL ya no es necesaria aquí

        If flpChips.Controls.Count = 0 Then
            flpChips.Visible = False
        Else
            flpChips.Visible = True
            If flpChips.Height > MAX_HEIGHT Then
                flpChips.AutoScroll = True
                flpChips.Height = MAX_HEIGHT
            Else
                flpChips.AutoScroll = False
            End If
        End If

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Estas líneas ya no son necesarias porque el Docking se encarga del layout.
        ' Panel1.Top = flpChips.Bottom + MARGIN_VERTICAL
        ' Panel1.Height = pnlAcciones.Top - Panel1.Top - MARGIN_VERTICAL
        ' --- FIN DE LA CORRECCIÓN ---
    End Sub

    Private Sub BtnLimpiar_Click(sender As Object, e As EventArgs) Handles btnLimpiar.Click
        filtros.Limpiar()
        flpChips.Controls.Clear()
        txtBusquedaGlobal.Clear()
        AplicarFiltros()
        UpdateFiltrosPanelHeight()
    End Sub
#End Region

#Region "Utilidades y Acciones de Botones"
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

    Private Sub btnCopiarCorreos_Click(sender As Object, e As EventArgs) Handles btnCopiarCorreos.Click
        If dvDatos Is Nothing OrElse dvDatos.Count = 0 Then
            MessageBox.Show("No hay datos para procesar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If Not dtOriginal.Columns.Contains("Correo") Then
            MessageBox.Show("La vista actual no contiene una columna 'Correo'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim correos = dvDatos.ToTable().AsEnumerable().
        Select(Function(r) r.Field(Of String)("Correo")).
                            Where(Function(c) Not String.IsNullOrWhiteSpace(c)).
                            Distinct(StringComparer.InvariantCultureIgnoreCase).
                            ToArray()

        If correos.Length = 0 Then
            MessageBox.Show("No se encontraron correos en el resultado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim textoCorreos As String = String.Join("; ", correos)
        Clipboard.SetText(textoCorreos)

        MessageBox.Show($"Se copiaron {correos.Length} correos al portapapeles." &
                      $"{Environment.NewLine}Ya puedes pegarlos en tu cliente de correo.",
                        "Correos copiados",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnGenerico_Nuevo_Click(sender As Object, e As EventArgs)
        _accionHandler?.ManejarBotonNuevo(Me)
    End Sub

    Private Sub btnGenerico_Editar_Click(sender As Object, e As EventArgs)
        _accionHandler?.ManejarBotonEditar(Me)
    End Sub

    Private Sub btnGenerico_Eliminar_Click(sender As Object, e As EventArgs)
        _accionHandler?.ManejarBotonEliminar(Me)
    End Sub

    Private Sub btnGenerico_Extra_Click(sender As Object, e As EventArgs)
        _accionHandler?.ManejarBotonExtra(Me)
    End Sub

#End Region

End Class


