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
    '(*** CAMBIO ***) Inicializamos las listas para que nunca sean Nothing.
    Private _todosLosEstados As List(Of EstadoTransitorio) = New List(Of EstadoTransitorio)()
    Private _todasLasNovedades As List(Of vw_NovedadesCompletas) = New List(Of vw_NovedadesCompletas)()
    Private _todasLasLicencias As List(Of HistoricoLicencia) = New List(Of HistoricoLicencia)()

    '--- UI State ---
    Private _selectedTimelineButton As Button = Nothing

    '--- Manejador de eventos para suscripción asíncrona ---
    Private ReadOnly _funcionarioActualizadoHandler As EventHandler(Of FuncionarioEventArgs)

    Public Sub New(idFuncionario As Integer)
        InitializeComponent()
        _funcionarioId = idFuncionario
        _uow = New UnitOfWork()

        ' Asignamos el método a la variable para resolver el problema de AddressOf con métodos Async
        _funcionarioActualizadoHandler = AddressOf ManejarFuncionarioActualizado
    End Sub

#End Region

#Region " Carga y Ciclo de Vida del Formulario "

    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' Suscribir manejadores de eventos de la UI
        AddHandler dgvNovedades.CellDoubleClick, AddressOf dgvNovedades_CellDoubleClick
        AddHandler dgvEstados.CellFormatting, AddressOf dgvEstados_CellFormatting
        AddHandler dgvNovedades.DataBindingComplete, AddressOf DgvNovedades_DataBindingComplete
        AddHandler dgvEstados.DataBindingComplete, AddressOf DgvEstados_DataBindingComplete
        AddHandler dtpDesde.ValueChanged, AddressOf RangoFechas_Cambio
        AddHandler dtpHasta.ValueChanged, AddressOf RangoFechas_Cambio

        ' Suscribirse al notificador de eventos para auto-actualización
        AddHandler NotificadorEventos.FuncionarioActualizado, _funcionarioActualizadoHandler

        ' Configuración inicial
        dtpDesde.Value = Date.Today
        dtpHasta.Value = Date.Today
        If btnGenerar IsNot Nothing Then btnGenerar.Visible = False

        ' Carga inicial de datos y actualización de la vista
        Await CargarDatosIniciales()
        ActualizarTodo()
    End Sub

    Private Sub frmFuncionarioSituacion_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        ' Darse de baja del evento para evitar fugas de memoria
        RemoveHandler NotificadorEventos.FuncionarioActualizado, _funcionarioActualizadoHandler
    End Sub

    ''' <summary>
    ''' Carga todos los datos necesarios para el funcionario desde la base de datos.
    ''' </summary>
    Private Async Function CargarDatosIniciales() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            _funcionario = Await _uow.Context.Set(Of Funcionario)().FindAsync(_funcionarioId)
            If _funcionario Is Nothing Then
                MessageBox.Show("No se encontró el funcionario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Close()
                Return
            End If

            lblNombre.Text = $"Situación de: {_funcionario.Nombre} ({_funcionario.CI})"

            _todosLosEstados = Await _uow.Repository(Of EstadoTransitorio)().GetAll().
                Include(Function(et) et.TipoEstadoTransitorio).
                Include(Function(et) et.DesignacionDetalle).
                Include(Function(et) et.EnfermedadDetalle).
                Include(Function(et) et.SancionDetalle).
                Include(Function(et) et.OrdenCincoDetalle).
                Include(Function(et) et.RetenDetalle).
                Include(Function(et) et.SumarioDetalle).
                Include(Function(et) et.TrasladoDetalle).
                Where(Function(et) et.FuncionarioId = _funcionarioId).
                AsNoTracking().
                ToListAsync()

            _todasLasNovedades = Await _uow.Context.Set(Of vw_NovedadesCompletas)().
                Where(Function(n) n.FuncionarioId = _funcionario.Id).
                OrderBy(Function(n) n.Fecha).
                AsNoTracking().
                ToListAsync()

            _todasLasLicencias = Await _uow.Context.Set(Of HistoricoLicencia)() _
                .Include(Function(l) l.TipoLicencia) _
                .Where(Function(l) l.FuncionarioId = _funcionarioId) _
                .AsNoTracking() _
                .ToListAsync()

            '(*** CAMBIO ***) Se elimina la llamada redundante a PerformClick.
            'btnGenerar.PerformClick()

        Catch ex As Exception
            MessageBox.Show($"Error al cargar los datos iniciales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region " Lógica de Actualización y Eventos "

    ''' <summary>
    ''' Método que se ejecuta cuando se recibe una notificación de actualización de funcionario.
    ''' </summary>
    Private Async Sub ManejarFuncionarioActualizado(sender As Object, e As FuncionarioEventArgs)
        If e.FuncionarioId = _funcionarioId Then
            ' Se asegura de que la actualización de la UI se ejecute en el hilo correcto
            If Me.InvokeRequired Then
                Me.Invoke(New Action(Async Sub()
                                         Await CargarDatosIniciales()
                                         ActualizarTodo()
                                     End Sub))
            Else
                Await CargarDatosIniciales()
                ActualizarTodo()
            End If
        End If
    End Sub

    Private Sub RangoFechas_Cambio(sender As Object, e As EventArgs)
        If dtpDesde.Value.Date > dtpHasta.Value.Date Then
            dtpHasta.Value = dtpDesde.Value.Date
        End If
        ActualizarTodo()
    End Sub

    ''' <summary>
    ''' Orquesta la actualización de todas las partes visuales del formulario.
    ''' </summary>
    Private Sub ActualizarTodo()
        Dim fechaInicio = dtpDesde.Value.Date
        Dim fechaFin = dtpHasta.Value.Date

        GenerarTimeline(fechaInicio, fechaFin)
        PoblarGrillaEstados(fechaInicio, fechaFin)
        SeleccionarPrimerDiaSiExiste()
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
            .Select(Function(n) n.Fecha.Date).Distinct().OrderBy(Function(d) d).ToList()

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
            Select(Function(n) New With {.Id = n.Id, n.Fecha, .Texto = n.Texto}).ToList()

        dgvNovedades.DataSource = novedadesDelDia
    End Sub

#End Region

#Region " Lógica de la Grilla de Estados "

    Private Sub PoblarGrillaEstados(fechaInicio As Date, fechaFin As Date)
        Dim overlaps As Func(Of Date?, Date?, Boolean) =
            Function(desde As Date?, hasta As Date?) As Boolean
                If Not desde.HasValue Then Return False
                Dim finEfectivo As Date = If(hasta.HasValue, hasta.Value, Date.MaxValue)
                Return (desde.Value.Date <= fechaFin) AndAlso (finEfectivo.Date >= fechaInicio)
            End Function

        Dim estadosEnPeriodo = _todosLosEstados.Where(Function(s)
                                                          Dim desde As Date? = Nothing, hasta As Date? = Nothing
                                                          s.GetFechas(desde, hasta)
                                                          Return overlaps(desde, hasta)
                                                      End Function)

        Dim dataSource = estadosEnPeriodo.Select(Function(s)
                                                     Dim desde As Date? = Nothing, hasta As Date? = Nothing
                                                     s.GetFechas(desde, hasta)
                                                     If hasta.HasValue AndAlso hasta.Value = Date.MinValue Then hasta = Nothing
                                                     Return New With {
                                                       .Tipo = s.TipoEstadoTransitorio.Nombre,
                                                       .Desde = desde,
                                                       .Hasta = hasta,
                                                       .Entity = s
                                                     }
                                                 End Function).OrderBy(Function(x) x.Desde).ToList()

        dgvEstados.DataSource = dataSource
    End Sub

#End Region

#Region " Configuración y Eventos de Grillas "

    Private Sub ConfigurarGrillaNovedades()
        If dgvNovedades.Columns.Count = 0 Then
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
        End If
    End Sub

    Private Sub ConfigurarGrillaEstados()
        If dgvEstados.Columns.Count = 0 Then
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
        End If
    End Sub

    Private Sub dgvNovedades_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse dgvNovedades.CurrentRow Is Nothing Then Return
        Try
            ' Acceso seguro a la propiedad 'Id' del objeto anónimo
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

    Private Sub dgvEstados_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 Then Return
        Dim dgv = CType(sender, DataGridView)
        Dim rowData = dgv.Rows(e.RowIndex).DataBoundItem
        If rowData Is Nothing Then Return

        ' Acceso seguro a la propiedad 'Entity' del objeto anónimo
        Dim estado As EstadoTransitorio = TryCast(rowData.GetType().GetProperty("Entity")?.GetValue(rowData, Nothing), EstadoTransitorio)
        If estado IsNot Nothing Then
            e.CellStyle.BackColor = estado.GetColor()
        End If
    End Sub

    Private Sub DgvNovedades_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        ConfigurarGrillaNovedades()
        If dgv.Columns.Contains("Id") Then dgv.Columns("Id").Visible = False
    End Sub

    Private Sub DgvEstados_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        ConfigurarGrillaEstados()
        If dgv.Columns.Contains("Entity") Then dgv.Columns("Entity").Visible = False
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    ' Este método ahora es redundante ya que el cambio de fecha llama a ActualizarTodo directamente
    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        ActualizarTodo()
    End Sub
#End Region

End Class