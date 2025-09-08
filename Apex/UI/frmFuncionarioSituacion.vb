Imports System.Data.Entity
Imports System.Globalization
Imports System.Text

Public Class frmFuncionarioSituacion

#Region " Campos y Constructor "

    '--- Variables de clase ---
    Private ReadOnly _funcionarioId As Integer
    Private ReadOnly _uow As IUnitOfWork
    Private _funcionario As Funcionario

    '--- Almacenamiento de datos ---
    Private _todasLasNovedades As List(Of vw_NovedadesCompletas) = New List(Of vw_NovedadesCompletas)()

    '--- UI State ---
    Private _selectedTimelineButton As Button = Nothing

    '--- Manejador de eventos para suscripción asíncrona ---
    Private ReadOnly _funcionarioActualizadoHandler As EventHandler(Of FuncionarioEventArgs)

    Public Sub New(idFuncionario As Integer)
        InitializeComponent()
        _funcionarioId = idFuncionario
        _uow = New UnitOfWork()
        _funcionarioActualizadoHandler = AddressOf ManejarFuncionarioActualizado
    End Sub

#End Region

#Region " Carga y Ciclo de Vida del Formulario "

    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' Suscripciones a eventos
        AddHandler dgvNovedades.CellDoubleClick, AddressOf dgvNovedades_CellDoubleClick
        AddHandler dgvNovedades.DataBindingComplete, AddressOf DgvNovedades_DataBindingComplete
        AddHandler dgvEstados.DataBindingComplete, AddressOf DgvEstados_DataBindingComplete
        AddHandler NotificadorEventos.FuncionarioActualizado, _funcionarioActualizadoHandler

        ' Configuración inicial
        dtpDesde.Value = Date.Today
        dtpHasta.Value = Date.Today

        If btnGenerar IsNot Nothing Then btnGenerar.Visible = True

        ' Carga inicial de datos y actualización de la vista
        Await CargarDatosEsenciales()
        Await ActualizarTodo()
    End Sub

    Private Sub frmFuncionarioSituacion_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.FuncionarioActualizado, _funcionarioActualizadoHandler
        _uow.Dispose()
    End Sub

    Private Async Function CargarDatosEsenciales() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            _funcionario = Await _uow.Context.Set(Of Funcionario)().FindAsync(_funcionarioId)
            If _funcionario Is Nothing Then
                MessageBox.Show("No se encontró el funcionario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Close()
                Return
            End If

            lblNombre.Text = $"Situación de: {_funcionario.Nombre} ({_funcionario.CI})"

            _todasLasNovedades = Await _uow.Context.Set(Of vw_NovedadesCompletas)().
                Where(Function(n) n.FuncionarioId = _funcionario.Id).
                AsNoTracking().
                ToListAsync()

        Catch ex As Exception
            MessageBox.Show($"Error al cargar los datos iniciales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region " Lógica de Actualización y Eventos "

    Private Async Sub ManejarFuncionarioActualizado(sender As Object, e As FuncionarioEventArgs)
        If e.FuncionarioId = _funcionarioId Then
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Async Sub()
                                         Await CargarDatosEsenciales()
                                         Await ActualizarTodo()
                                     End Sub))
            Else
                Await CargarDatosEsenciales()
                Await ActualizarTodo()
            End If
        End If
    End Sub

    Private Async Function ActualizarTodo() As Task
        Dim fechaInicio = dtpDesde.Value.Date
        Dim fechaFin = dtpHasta.Value.Date

        GenerarTimeline(fechaInicio, fechaFin)
        Await PoblarGrillaEstados(fechaInicio, fechaFin)
        SeleccionarPrimerDiaSiExiste()
    End Function

    Private Async Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Await ActualizarTodo()
    End Sub

#End Region

#Region " Lógica del Timeline de Novedades "

    Private Sub GenerarTimeline(fechaInicio As Date, fechaFin As Date)
        flpTimeline.SuspendLayout()
        flpTimeline.Controls.Clear()
        _selectedTimelineButton = Nothing
        dgvNovedades.DataSource = Nothing

        Dim fechasNovedades = _todasLasNovedades _
            .Where(Function(n) n.Fecha.Date >= fechaInicio AndAlso n.Fecha.Date <= fechaFin) _
            .Select(Function(n) n.Fecha.Date).Distinct().OrderByDescending(Function(d) d).ToList()

        If Not fechasNovedades.Any() Then
            Dim lblEmpty As New Label With {
                .Text = "No hay novedades en el período seleccionado.",
                .AutoSize = True,
                .Margin = New Padding(10)
            }
            flpTimeline.Controls.Add(lblEmpty)
        Else
            For Each fecha As Date In fechasNovedades
                Dim btnDia As New Button With {
                    .Text = fecha.ToString("dd MMM yyyy"),
                    .Tag = fecha,
                    .Width = flpTimeline.ClientSize.Width - 20,
                    .Height = 30,
                    .FlatStyle = FlatStyle.Flat,
                    .TextAlign = ContentAlignment.MiddleLeft,
                    .Margin = New Padding(0, 0, 0, 6)
                }
                btnDia.FlatAppearance.BorderSize = 1
                AddHandler btnDia.Click, AddressOf TimelineDia_Click
                flpTimeline.Controls.Add(btnDia)
            Next
        End If

        flpTimeline.ResumeLayout()
    End Sub

    Private Sub SeleccionarPrimerDiaSiExiste()
        Dim firstBtn = flpTimeline.Controls.OfType(Of Button)().FirstOrDefault()
        If firstBtn IsNot Nothing Then
            TimelineDia_Click(firstBtn, EventArgs.Empty)
        End If
    End Sub

    Private Sub TimelineDia_Click(sender As Object, e As EventArgs)
        Dim clickedButton = CType(sender, Button)
        Dim fechaSeleccionada = CType(clickedButton.Tag, Date)

        If _selectedTimelineButton IsNot Nothing Then
            _selectedTimelineButton.BackColor = SystemColors.Control
            _selectedTimelineButton.ForeColor = SystemColors.ControlText
        End If
        clickedButton.BackColor = SystemColors.Highlight
        clickedButton.ForeColor = SystemColors.HighlightText
        _selectedTimelineButton = clickedButton

        ActualizarVistaDeNovedades(fechaSeleccionada)
    End Sub

    Private Sub ActualizarVistaDeNovedades(selectedDate As Date)
        Dim novedadesDelDia = _todasLasNovedades.
            Where(Function(n) n.Fecha.Date = selectedDate.Date).
            OrderByDescending(Function(n) n.Id).
            Select(Function(n) New With {.Id = n.Id, n.Fecha, .Texto = n.Texto}).ToList()

        dgvNovedades.DataSource = novedadesDelDia
    End Sub

#End Region

#Region " Lógica de la Grilla de Estados "

    Private Async Function PoblarGrillaEstados(fechaInicio As Date, fechaFin As Date) As Task
        LoadingHelper.MostrarCargando(dgvEstados)
        Try
            Dim estadosEnPeriodo = Await _uow.Context.Set(Of vw_EstadosTransitoriosCompletos)().
                Where(Function(e) e.FuncionarioId = _funcionarioId AndAlso
                                  e.FechaDesde.HasValue AndAlso
                                  e.FechaDesde.Value <= fechaFin AndAlso
                                  (e.FechaHasta Is Nothing OrElse e.FechaHasta.Value >= fechaInicio)).
                AsNoTracking().
                ToListAsync()

            Dim licenciasEnPeriodo = Await _uow.Context.Set(Of HistoricoLicencia)() _
                .Include(Function(l) l.TipoLicencia) _
                .Where(Function(l) l.FuncionarioId = _funcionarioId AndAlso (l.inicio <= fechaFin And l.finaliza >= fechaInicio)) _
                .AsNoTracking() _
                .ToListAsync()

            Dim eventosUnificados As New List(Of Object)

            ' ============================================================================
            ' CORRECCIÓN: Se agrega el TipoId al objeto anónimo para usarlo al colorear.
            ' ============================================================================
            eventosUnificados.AddRange(estadosEnPeriodo.Select(Function(s) New With {
                .Tipo = s.TipoEstadoNombre,
                .Desde = s.FechaDesde,
                .Hasta = s.FechaHasta,
                .IsEstado = True,
                .TipoId = s.TipoEstadoTransitorioId
            }))

            eventosUnificados.AddRange(licenciasEnPeriodo.Select(Function(l) New With {
                .Tipo = $"LICENCIA: {l.TipoLicencia.Nombre}",
                .Desde = CType(l.inicio, Date?),
                .Hasta = CType(l.finaliza, Date?),
                .IsEstado = False,
                .TipoId = 0 ' Un valor por defecto para las licencias
            }))

            dgvEstados.DataSource = eventosUnificados.OrderByDescending(Function(x) x.Desde).ToList()

        Catch ex As Exception
            MessageBox.Show($"Error al poblar la grilla de estados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            dgvEstados.DataSource = Nothing
        Finally
            LoadingHelper.OcultarCargando(dgvEstados)
        End Try
    End Function

#End Region

#Region " Configuración y Eventos de Grillas "
    ' ... (Esta región no necesita cambios)
    Private Sub ConfigurarGrillaNovedades()
        If dgvNovedades.Columns.Count > 0 Then Return
        dgvNovedades.AutoGenerateColumns = False
        dgvNovedades.Columns.Clear()

        Dim colFecha As New DataGridViewTextBoxColumn With {
            .Name = "Fecha", .DataPropertyName = "Fecha", .HeaderText = "Fecha",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        }
        colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvNovedades.Columns.Add(colFecha)

        dgvNovedades.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Texto", .DataPropertyName = "Texto", .HeaderText = "Novedad",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
    End Sub

    Private Sub ConfigurarGrillaEstados()
        If dgvEstados.Columns.Count > 0 Then Return
        dgvEstados.AutoGenerateColumns = False
        dgvEstados.Columns.Clear()
        dgvEstados.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Tipo", .DataPropertyName = "Tipo", .HeaderText = "Tipo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        })
        Dim colDesde As New DataGridViewTextBoxColumn With {
            .Name = "Desde", .DataPropertyName = "Desde", .HeaderText = "Desde",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        }
        colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstados.Columns.Add(colDesde)
        Dim colHasta As New DataGridViewTextBoxColumn With {
            .Name = "Hasta", .DataPropertyName = "Hasta", .HeaderText = "Hasta",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        }
        colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstados.Columns.Add(colHasta)
    End Sub

    Private Sub dgvNovedades_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse dgvNovedades.CurrentRow Is Nothing Then Return
        Try
            Dim rowData = dgvNovedades.CurrentRow.DataBoundItem
            Dim novedadId = CInt(rowData.GetType().GetProperty("Id").GetValue(rowData, Nothing))

            If novedadId > 0 Then
                Using frm As New frmNovedadCrear(novedadId)
                    frm.ShowDialog(Me)
                End Using
            End If
        Catch ex As Exception
            MessageBox.Show("No se pudo abrir el detalle de la novedad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub DgvNovedades_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        ConfigurarGrillaNovedades()
        If dgv.Columns.Contains("Id") Then dgv.Columns("Id").Visible = False
    End Sub

    Private Sub DgvEstados_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        ConfigurarGrillaEstados()
        If dgv.Columns.Contains("IsEstado") Then dgv.Columns("IsEstado").Visible = False
        If dgv.Columns.Contains("TipoId") Then dgv.Columns("TipoId").Visible = False
        ' << NUEVO: aplicar colores por severidad >>
        AplicarColoresEstados(dgv)
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

#End Region
    '=============================================================================
    ' ===================== Severidad y Colores =====================
    Private Enum Severidad
        Info = 0
        Baja = 1
        Media = 2
        Alta = 3
        Critica = 4
    End Enum

    Private Sub AplicarColoresEstados(dgv As DataGridView)
        For Each row As DataGridViewRow In dgv.Rows
            Dim data As Object = row.DataBoundItem
            If data Is Nothing Then Continue For

            Dim tipoTxt As String = GetPropValue(Of String)(data, "Tipo", "")
            Dim tipoId As Integer = GetPropValue(Of Integer)(data, "TipoId", 0)
            Dim isEstado As Boolean = GetPropValue(Of Boolean)(data, "IsEstado", False)

            Dim sev As Severidad = ClasificarSeveridad(tipoTxt, tipoId, isEstado)
            PintarFilaPorSeveridad(row, sev)
        Next
    End Sub

    Private Function ClasificarSeveridad(tipoTexto As String, tipoId As Integer, isEstado As Boolean) As Severidad
        Dim t As String = If(tipoTexto, String.Empty).ToUpperInvariant()

        ' --- Reglas por texto (robusto a cambios de IDs / nombres) ---
        If t.StartsWith("LICENCIA") Then Return Severidad.Info
        If t.Contains("INICIO DE PROCESAMIENTO") Then Return Severidad.Critica
        If t.Contains("SEPARACION") OrElse t.Contains("SEPARACIÓN") Then Return Severidad.Critica
        If t.Contains("BAJA") Then Return Severidad.Critica
        If t.Contains("SUMARIO") Then Return Severidad.Alta
        If t.Contains("SANCI") Then Return Severidad.Alta
        If t.Contains("ENFERMEDAD") Then Return Severidad.Media
        If t.Contains("ORDEN CINCO") OrElse t.Contains("ORDEN 5") Then Return Severidad.Media
        If t.Contains("TRASLADO") Then Return Severidad.Media
        If t.Contains("RETEN") OrElse t.Contains("RETÉN") Then Return Severidad.Baja
        If t.Contains("DESIGNACION") OrElse t.Contains("DESIGNACIÓN") Then Return Severidad.Baja
        If t.Contains("REACTIVACION") OrElse t.Contains("REACTIVACIÓN") Then Return Severidad.Baja
        If t.Contains("CAMBIO DE CARGO") Then Return Severidad.Baja

        ' --- (Opcional) Reglas por ID si querés afinar: ej. 5 = Retén en tu código ---
        'Select Case tipoId
        '    Case 5 : Return Severidad.Baja ' Retén
        'End Select

        Return Severidad.Baja
    End Function

    Private Sub PintarFilaPorSeveridad(row As DataGridViewRow, sev As Severidad)
        Dim strong As Color
        Dim text As Color = Color.White

        Select Case sev
            Case Severidad.Critica
                strong = Color.FromArgb(229, 57, 53)     ' rojo 600
            Case Severidad.Alta
                strong = Color.FromArgb(245, 124, 0)     ' naranja 700
            Case Severidad.Media
                strong = Color.FromArgb(255, 179, 0)     ' ámbar 600
            Case Severidad.Baja
                strong = Color.FromArgb(56, 142, 60)     ' verde 600
            Case Else ' Info
                strong = Color.FromArgb(30, 136, 229)    ' azul 600
        End Select

        ' MISMO color fuerte tanto seleccionado como no seleccionado
        row.DefaultCellStyle.BackColor = strong
        row.DefaultCellStyle.ForeColor = text
        row.DefaultCellStyle.SelectionBackColor = strong
        row.DefaultCellStyle.SelectionForeColor = text
    End Sub


    ' ==== Helpers seguros con Option Strict On ====
    Private Function GetPropValue(Of T)(obj As Object, propName As String, defaultValue As T) As T
        If obj Is Nothing Then Return defaultValue
        Dim p = obj.GetType().GetProperty(propName, Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.IgnoreCase)
        If p Is Nothing Then Return defaultValue
        Dim val = p.GetValue(obj, Nothing)
        If val Is Nothing Then Return defaultValue
        Try
            Return CType(val, T)
        Catch
            Try
                Return DirectCast(Convert.ChangeType(val, GetType(T), Globalization.CultureInfo.InvariantCulture), T)
            Catch
                Return defaultValue
            End Try
        End Try
    End Function

End Class