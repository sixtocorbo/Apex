' Apex/UI/frmFiltroAvanzado.vb
Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Data

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

        Private Shared Function Esc(s As String) As String
            Return s.Replace("'", "''")
        End Function

        Private Shared Function Formatear(valor As String) As String
            If DateTime.TryParse(valor, Nothing) Then
                Return $"'{CDate(valor):yyyy-MM-dd}'"
            End If

            Dim n As Double
            If Double.TryParse(valor, n) Then
                Return n.ToString()
            End If

            Return $"'{Esc(valor)}'"
        End Function

        Public Function ToRowFilter() As String
            Dim col = $"[{Columna}]"

            Select Case Operador
                Case OperadorComparacion.Igual
                    Return $"{col} = {Formatear(Valor1)}"
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
    Private _licenciaService As LicenciaService
    Private _notificacionService As NotificacionPersonalService
    ' --- Variables para paginación ---
    Private _paginaActual As Integer = 1
    Private _tamañoPagina As Integer = 100 ' Cargar de a 100 registros
    Private _totalRegistros As Integer = 0
#End Region

#Region "Constructor / Load"
    Private Sub frmFiltroAvanzado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _licenciaService = New LicenciaService()
        _notificacionService = New NotificacionPersonalService()

        cmbOrigenDatos.DataSource = [Enum].GetValues(GetType(ConsultasGenericas.TipoOrigenDatos))
        cmbOrigenDatos.SelectedIndex = 0
        dtpFechaInicio.Value = Date.Today.AddMonths(-1)
        dtpFechaFin.Value = Date.Today
        dgvDatos.DataSource = Nothing

        ' Handlers
        AddHandler lstColumnas.SelectedIndexChanged, AddressOf ColumnaCambiada
        AddHandler cmbOrigenDatos.SelectedIndexChanged, AddressOf cmbOrigenDatos_SelectedIndexChanged
        AddHandler txtBusquedaGlobal.TextChanged, AddressOf txtBusquedaGlobal_TextChanged_Handler

        ' Acciones
        AddHandler btnNuevaLicencia.Click, AddressOf btnNuevaLicencia_Click
        AddHandler btnEditarLicencia.Click, AddressOf btnEditarLicencia_Click
        AddHandler btnEliminarLicencia.Click, AddressOf btnEliminarLicencia_Click

        ' --- Añadir handlers para paginación ---
        AddHandler btnAnterior.Click, AddressOf btnAnterior_Click
        AddHandler btnSiguiente.Click, AddressOf btnSiguiente_Click

        cmbOrigenDatos_SelectedIndexChanged(cmbOrigenDatos, EventArgs.Empty)
        ActualizarAccionesDisponibles()
    End Sub
#End Region

#Region "Carga de datos"
    Private Async Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
        _paginaActual = 1 ' Siempre reiniciar a la página 1 al cargar nuevos datos
        Await CargarDatos()
    End Sub

    Private Async Function CargarDatos() As Task
        Dim origenSeleccionado = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos)
        Dim fechaInicio = dtpFechaInicio.Value.Date
        Dim fechaFin = dtpFechaFin.Value.Date

        If origenSeleccionado <> ConsultasGenericas.TipoOrigenDatos.Funcionarios AndAlso fechaInicio > fechaFin Then
            MessageBox.Show("La fecha de inicio no puede ser mayor que la fecha de fin.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        btnCargar.Enabled = False
        lblConteoRegistros.Text = "Buscando..."
        lblConteoRegistros.Refresh()

        Try
            Using wait As New WaitCursor()
                If origenSeleccionado = ConsultasGenericas.TipoOrigenDatos.Licencias Then
                    ' --- LÓGICA PAGINADA ---
                    Dim resultado = Await ConsultasGenericas.ConsultarLicenciasPaginado(fechaInicio, fechaFin, _paginaActual, _tamañoPagina)
                    dtOriginal = ToDataTable(resultado.Items)
                    _totalRegistros = resultado.TotalItems
                Else
                    ' --- LÓGICA ANTERIOR (SIN PAGINAR) ---
                    _totalRegistros = 0 ' Reiniciar para que no se muestre la paginación
                    dtOriginal = Await ConsultasGenericas.ObtenerDatosGenericosAsync(origenSeleccionado, fechaInicio, fechaFin)
                End If
            End Using

            If dtOriginal Is Nothing OrElse dtOriginal.Rows.Count = 0 Then
                If _paginaActual = 1 Then ' Solo mostrar mensaje si es la primera carga
                    MessageBox.Show("No se encontraron datos para los criterios seleccionados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                LimpiarTodo()
                Return
            End If

            dvDatos = New DataView(dtOriginal)
            dgvDatos.DataSource = dvDatos

            If _paginaActual = 1 Then ' Solo recargar columnas en la primera carga
                lstColumnas.Items.Clear()
                For Each col As DataColumn In dtOriginal.Columns
                    lstColumnas.Items.Add(col.ColumnName)
                Next
                If lstColumnas.Items.Count > 0 Then lstColumnas.SelectedIndex = 0

                filtros.Limpiar()
                flpChips.Controls.Clear()
                txtBusquedaGlobal.Clear()
            End If

            AplicarFiltros()
            gbxFiltros.Enabled = True

        Catch ex As Exception
            MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnCargar.Enabled = True
            ActualizarAccionesDisponibles() ' Llama a esto para mostrar/ocultar paginación
        End Try
    End Function

    Private Sub LimpiarTodo()
        dgvDatos.DataSource = Nothing
        dtOriginal = Nothing
        dvDatos = Nothing
        lstColumnas.Items.Clear()
        lstValores.Items.Clear()
        flpChips.Controls.Clear()
        filtros.Limpiar()
        gbxFiltros.Enabled = False
        _totalRegistros = 0
        _paginaActual = 1
        ActualizarAccionesDisponibles() ' Actualiza para ocultar todo
    End Sub
#End Region

#Region "Eventos principales y Paginación"
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
        dtpFechaFin.Enabled = (origenSeleccionado <> ConsultasGenericas.TipoOrigenDatos.Funcionarios)
        LimpiarTodo()
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

    ' --- Eventos de los nuevos botones de paginación ---
    Private Async Sub btnAnterior_Click(sender As Object, e As EventArgs)
        If _paginaActual > 1 Then
            _paginaActual -= 1
            Await CargarDatos()
        End If
    End Sub

    Private Async Sub btnSiguiente_Click(sender As Object, e As EventArgs)
        Dim totalPaginas = CInt(Math.Ceiling(_totalRegistros / CDbl(_tamañoPagina)))
        If _paginaActual < totalPaginas Then
            _paginaActual += 1
            Await CargarDatos()
        End If
    End Sub
#End Region

#Region "Filtrado"
    Private Sub AplicarFiltros()
        If dvDatos Is Nothing Then Return

        Dim rf = filtros.RowFilter()
        Dim globalF = ConstruirFiltroGlobal()
        dvDatos.RowFilter = String.Join(" AND ", {rf, globalF}.Where(Function(s) Not String.IsNullOrWhiteSpace(s)))

        If CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos) = ConsultasGenericas.TipoOrigenDatos.Licencias Then
            ActualizarControlesPaginacion()
        Else
            lblConteoRegistros.Text = $"Registros encontrados: {dvDatos.Count}"
        End If

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

        If cond.Count = 0 Then Return String.Empty
        Return "(" & String.Join(" OR ", cond) & ")"
    End Function
#End Region

#Region "Chips UI"
    Private Sub CrearChip(regla As ReglaFiltro)
        Dim descripcionValores As String = If(regla.Operador = OperadorComparacion.EnLista, regla.Valor1.Replace("|", ", "), regla.Valor1)
        Dim reglaDesc As String = $"{regla.Columna}: {descripcionValores}"
        Dim chipContainer As New FlowLayoutPanel() With {.AutoSize = True, .AutoSizeMode = AutoSizeMode.GrowAndShrink, .Margin = New Padding(3), .BackColor = Color.FromArgb(220, 235, 255), .BorderStyle = BorderStyle.FixedSingle, .FlowDirection = FlowDirection.LeftToRight, .WrapContents = False}
        Dim lblTexto As New Label() With {.Text = reglaDesc, .AutoSize = True, .MaximumSize = New Size(450, 0), .Margin = New Padding(3), .TextAlign = ContentAlignment.MiddleLeft}
        Dim btnCerrar As New Button() With {.Text = "×", .Font = New Font("Segoe UI", 8, FontStyle.Bold), .ForeColor = Color.DarkRed, .FlatStyle = FlatStyle.Flat, .Size = New Size(22, 22), .Tag = regla, .Margin = New Padding(3, 1, 1, 1)}
        btnCerrar.FlatAppearance.BorderSize = 0
        AddHandler btnCerrar.Click, AddressOf ChipCerrar_Click
        chipContainer.Controls.Add(lblTexto)
        chipContainer.Controls.Add(btnCerrar)
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

    '=========================================================================='
    '==                       AQUÍ EMPIEZA LA SOLUCIÓN                         =='
    '=========================================================================='
#Region "Utilidades y Exportación"
    ''' <summary>
    ''' Clase de ayuda para mostrar un cursor de espera durante operaciones largas.
    ''' </summary>
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
#End Region
    '=========================================================================='
    '==                        AQUÍ TERMINA LA SOLUCIÓN                        =='
    '=========================================================================='


#Region "Acciones Dinámicas"

    Private Sub ActualizarControlesPaginacion()
        Dim esPaginable = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos) = ConsultasGenericas.TipoOrigenDatos.Licencias AndAlso _totalRegistros > 0
        btnAnterior.Visible = esPaginable
        btnSiguiente.Visible = esPaginable
        lblPaginacion.Visible = esPaginable

        If esPaginable Then
            Dim totalPaginas = CInt(Math.Ceiling(_totalRegistros / CDbl(_tamañoPagina)))
            If totalPaginas = 0 Then totalPaginas = 1
            lblPaginacion.Text = $"Página {_paginaActual} de {totalPaginas}"
            btnAnterior.Enabled = _paginaActual > 1
            btnSiguiente.Enabled = _paginaActual < totalPaginas
            lblConteoRegistros.Text = $"Mostrando {dvDatos.Count} de {_totalRegistros} registros"
        Else
            lblConteoRegistros.Text = If(dvDatos IsNot Nothing, $"Registros encontrados: {dvDatos.Count}", "Registros encontrados: 0")
        End If
    End Sub

    Private Sub ActualizarAccionesDisponibles()
        Dim origenSeleccionado = CType(cmbOrigenDatos.SelectedItem, ConsultasGenericas.TipoOrigenDatos)
        btnNuevaLicencia.Visible = False : btnEditarLicencia.Visible = False : btnEliminarLicencia.Visible = False
        btnNuevaNotificacion.Visible = False : btnEditarNotificacion.Visible = False : btnEliminarNotificacion.Visible = False
        btnCambiarEstado.Visible = False
        Separator1.Visible = (dtOriginal IsNot Nothing AndAlso dtOriginal.Rows.Count > 0)
        ActualizarControlesPaginacion()

        Select Case origenSeleccionado
            Case ConsultasGenericas.TipoOrigenDatos.Licencias
                btnNuevaLicencia.Visible = True : btnEditarLicencia.Visible = True : btnEliminarLicencia.Visible = True
            Case ConsultasGenericas.TipoOrigenDatos.Notificaciones
                btnNuevaNotificacion.Visible = True : btnEditarNotificacion.Visible = True : btnEliminarNotificacion.Visible = True : btnCambiarEstado.Visible = True
        End Select
    End Sub

    ' --- Acciones para Licencias ---
    Private Sub btnNuevaLicencia_Click(sender As Object, e As EventArgs)
        Using frm As New frmLicenciaCrear()
            If frm.ShowDialog() = DialogResult.OK Then
                btnCargar.PerformClick()
            End If
        End Using
    End Sub

    Private Async Sub btnEditarLicencia_Click(sender As Object, e As EventArgs)
        If dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim idSeleccionado = CInt(drv("Id"))
        Using frm As New frmLicenciaCrear(idSeleccionado)
            If frm.ShowDialog() = DialogResult.OK Then
                Await CargarDatos()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarLicencia_Click(sender As Object, e As EventArgs)
        If dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim idSeleccionado = CInt(drv("Id"))
        Dim nombre = drv("Funcionario").ToString()
        If MessageBox.Show($"¿Está seguro de que desea eliminar la licencia para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _licenciaService.DeleteAsync(idSeleccionado)
                Await CargarDatos()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' --- Acciones para Notificaciones ---
    Private Sub btnNuevaNotificacion_Click(sender As Object, e As EventArgs) Handles btnNuevaNotificacion.Click
        Using frm As New frmNotificacionCrear()
            If frm.ShowDialog() = DialogResult.OK Then
                btnCargar.PerformClick()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarNotificacion_Click(sender As Object, e As EventArgs) Handles btnEliminarNotificacion.Click
        If dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim idSeleccionado = CInt(drv("Id"))
        Dim nombre = drv("Funcionario").ToString()

        If MessageBox.Show($"¿Está seguro de que desea eliminar la notificación para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _notificacionService.DeleteAsync(idSeleccionado)
                btnCargar.PerformClick()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Async Sub btnCambiarEstado_Click(sender As Object, e As EventArgs) Handles btnCambiarEstado.Click
        If dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim idSeleccionado = CInt(drv("Id"))

        Using frm As New frmCambiarEstadoNotificacion()
            If frm.ShowDialog() = DialogResult.OK Then
                Dim nuevoEstadoId = frm.SelectedEstadoId
                Try
                    Await _notificacionService.UpdateEstadoAsync(idSeleccionado, nuevoEstadoId)
                    btnCargar.PerformClick()
                Catch ex As Exception
                    MessageBox.Show("Error al actualizar el estado: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub
    Private Sub btnEditarNotificacion_Click(sender As Object, e As EventArgs) Handles btnEditarNotificacion.Click
        If dgvDatos.CurrentRow Is Nothing Then Return
        Dim drv = CType(dgvDatos.CurrentRow.DataBoundItem, DataRowView)
        Dim idSeleccionado = CInt(drv("Id"))

        Using frm As New frmNotificacionCrear(idSeleccionado)
            If frm.ShowDialog() = DialogResult.OK Then
                btnCargar.PerformClick()
            End If
        End Using
    End Sub
#End Region

End Class