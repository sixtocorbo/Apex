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
        AddHandler dgvEstados.CellFormatting, AddressOf dgvEstados_CellFormatting
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

        ' ============================================================================
        ' CORRECCIÓN: Se ordena el timeline de forma descendente.
        ' ============================================================================
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
            OrderByDescending(Function(n) n.Id). ' Opcional: ordenar novedades del mismo día
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

            eventosUnificados.AddRange(estadosEnPeriodo.Select(Function(s) New With {
                .Tipo = s.TipoEstadoNombre,
                .Desde = s.FechaDesde,
                .Hasta = s.FechaHasta,
                .IsEstado = True
            }))

            eventosUnificados.AddRange(licenciasEnPeriodo.Select(Function(l) New With {
                .Tipo = $"LICENCIA: {l.TipoLicencia.Nombre}",
                .Desde = CType(l.inicio, Date?),
                .Hasta = CType(l.finaliza, Date?),
                .IsEstado = False
            }))

            ' ============================================================================
            ' CORRECCIÓN: Se ordena la grilla de forma descendente por fecha de inicio.
            ' ============================================================================
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

    Private Sub dgvEstados_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse dgvEstados.Rows(e.RowIndex).DataBoundItem Is Nothing Then Return

        Dim rowData = dgvEstados.Rows(e.RowIndex).DataBoundItem
        Dim isEstado = CBool(rowData.GetType().GetProperty("IsEstado")?.GetValue(rowData, Nothing))

        If isEstado Then
            e.CellStyle.BackColor = ColorTranslator.FromHtml("#fff1cc") ' Color para Estados
        Else
            e.CellStyle.BackColor = ColorTranslator.FromHtml("#d4e6f1") ' Color para Licencias
        End If
        e.CellStyle.ForeColor = Color.Black
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
        If dgv.Columns.Contains("IsEstado") Then dgv.Columns("IsEstado").Visible = False
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

#End Region

End Class