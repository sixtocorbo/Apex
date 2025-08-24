Imports System.Data.Entity
Imports System.Globalization
Imports System.Text

Public Class frmFuncionarioSituacion

    Private _funcionarioId As Integer
    Private _uow As IUnitOfWork
    Private _funcionario As Funcionario
    Private _estados As List(Of EstadoTransitorio)
    Private _novedadesDelPeriodo As List(Of vw_NovedadesCompletas) = New List(Of vw_NovedadesCompletas)
    Private _toolTipData As New Dictionary(Of Date, String)

    '--- Variables para el nuevo calendario ---
    Private _currentDate As Date = Date.Now
    Private _selectedDayControl As DayControl = Nothing

    Public Sub New(idFuncionario As Integer)
        InitializeComponent()
        _funcionarioId = idFuncionario
        _uow = New UnitOfWork()
    End Sub

    Private Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        '--- Eventos de Controles ---
        AddHandler dtpAño.ValueChanged, AddressOf dtpAño_ValueChanged
        AddHandler dgvNovedades.CellDoubleClick, AddressOf dgvNovedades_CellDoubleClick
        AddHandler dgvEstados.CellFormatting, AddressOf dgvEstados_CellFormatting
        AddHandler dgvEstados.SelectionChanged, AddressOf dgvEstados_SelectionChanged
        AddHandler btnPrevMonth.Click, AddressOf btnPrevMonth_Click
        AddHandler btnNextMonth.Click, AddressOf btnNextMonth_Click

        '--- Eventos para ocultar columnas en grillas ---
        AddHandler dgvNovedades.DataBindingComplete, AddressOf DgvNovedades_DataBindingComplete
        AddHandler dgvEstados.DataBindingComplete, AddressOf DgvEstados_DataBindingComplete

        '--- Fecha inicial para el calendario ---
        _currentDate = New Date(dtpAño.Value.Year, Date.Now.Month, 1)
    End Sub
    Private Async Sub frmFuncionarioSituacion_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Await CargarDatos()
    End Sub
    Private Async Sub dtpAño_ValueChanged(sender As Object, e As EventArgs)
        _currentDate = New Date(dtpAño.Value.Year, _currentDate.Month, 1)
        Await CargarDatos()
    End Sub

    Private Async Function CargarDatos() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            _funcionario = Await _uow.Context.Set(Of Funcionario)().FindAsync(_funcionarioId)
            If _funcionario Is Nothing Then
                MessageBox.Show("No se encontró el funcionario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Close()
                Return
            End If

            lblNombre.Text = $"Situación de: {_funcionario.Nombre} ({_funcionario.CI})"

            Dim fechaInicio As New Date(dtpAño.Value.Year, 1, 1)
            Dim fechaFin As New Date(dtpAño.Value.Year, 12, 31)

            _estados = Await _uow.Repository(Of EstadoTransitorio)().GetAll().
                Include(Function(et) et.TipoEstadoTransitorio).
                Include(Function(et) et.DesignacionDetalle).
                Include(Function(et) et.EnfermedadDetalle).
                Include(Function(et) et.SancionDetalle).
                Include(Function(et) et.OrdenCincoDetalle).
                Include(Function(et) et.RetenDetalle).
                Include(Function(et) et.SumarioDetalle).
                Where(Function(et) et.FuncionarioId = _funcionarioId).
                ToListAsync()

            _novedadesDelPeriodo = Await _uow.Context.Set(Of vw_NovedadesCompletas)().
                Where(Function(n) n.FuncionarioId = _funcionario.Id AndAlso n.Fecha >= fechaInicio AndAlso n.Fecha <= fechaFin).
                OrderBy(Function(n) n.Fecha).
                ToListAsync()

            GenerarDatosToolTip(fechaInicio, fechaFin)
            PoblarGrillaEstados()

            '--- Generar el nuevo calendario ---
            GenerateCalendar()

        Catch ex As Exception
            MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Sub GenerarDatosToolTip(fechaInicio As Date, fechaFin As Date)
        _toolTipData.Clear()
        Dim fechaActual = fechaInicio

        While fechaActual <= fechaFin
            Dim sb As New StringBuilder()
            For Each estado In _estados
                Dim desde As Date? = Nothing
                Dim hasta As Date? = Nothing
                Select Case estado.TipoEstadoTransitorioId
                    Case 1 : desde = estado.DesignacionDetalle?.FechaDesde : hasta = estado.DesignacionDetalle?.FechaHasta
                    Case 2 : desde = estado.EnfermedadDetalle?.FechaDesde : hasta = estado.EnfermedadDetalle?.FechaHasta
                    Case 3 : desde = estado.SancionDetalle?.FechaDesde : hasta = estado.SancionDetalle?.FechaHasta
                    Case 4 : desde = estado.OrdenCincoDetalle?.FechaDesde : hasta = estado.OrdenCincoDetalle?.FechaHasta
                    Case 5 : desde = estado.RetenDetalle?.FechaReten
                    Case 6 : desde = estado.SumarioDetalle?.FechaDesde : hasta = estado.SumarioDetalle?.FechaHasta
                End Select
                If desde.HasValue AndAlso fechaActual.Date >= desde.Value.Date AndAlso (Not hasta.HasValue OrElse fechaActual.Date <= hasta.Value.Date) Then
                    sb.AppendLine($"- {estado.TipoEstadoTransitorio.Nombre}")
                End If
            Next

            Dim novedadesDelDia = _novedadesDelPeriodo.Where(Function(n) n.Fecha.Date = fechaActual.Date)
            For Each novedad In novedadesDelDia
                sb.AppendLine($"- Novedad: {novedad.Texto}")
            Next

            If sb.Length > 0 Then
                _toolTipData(fechaActual.Date) = sb.ToString().TrimEnd()
            End If

            fechaActual = fechaActual.AddDays(1)
        End While
    End Sub

#Region "Lógica del Calendario Personalizado"

    Private Sub GenerateCalendar()
        flpCalendar.SuspendLayout()
        flpCalendar.Controls.Clear()

        Dim firstDayOfMonth = New Date(_currentDate.Year, _currentDate.Month, 1)
        Dim daysInMonth = Date.DaysInMonth(_currentDate.Year, _currentDate.Month)

        lblMonthYear.Text = firstDayOfMonth.ToString("MMMM yyyy", New CultureInfo("es-ES")).ToUpper()

        ' Añade espacios en blanco para los días antes del día 1 del mes
        Dim startingDayOfWeek = CInt(firstDayOfMonth.DayOfWeek)
        For i = 0 To startingDayOfWeek - 1
            Dim blankControl = New Panel With {.Size = New Size(158, 80), .Margin = New Padding(2)}
            flpCalendar.Controls.Add(blankControl)
        Next

        For day = 1 To daysInMonth
            Dim dayDate = New Date(_currentDate.Year, _currentDate.Month, day)
            Dim dayCtrl As New DayControl With {
                .DayNumber = day,
                .Size = New Size(158, 80),
                .Margin = New Padding(2)
            }

            If _toolTipData.ContainsKey(dayDate) Then
                dayCtrl.HasEvent = True
            End If

            AddHandler dayCtrl.DayClicked, AddressOf DayControl_Clicked
            flpCalendar.Controls.Add(dayCtrl)
        Next

        flpCalendar.ResumeLayout()

        ' Selecciona el primer día por defecto o el día actual si es el mes actual
        Dim dayToSelect = 1
        If _currentDate.Year = Date.Now.Year AndAlso _currentDate.Month = Date.Now.Month Then
            dayToSelect = Date.Now.Day
        End If

        Dim dayControlToSelect As DayControl = flpCalendar.Controls.OfType(Of DayControl).FirstOrDefault(Function(dc) dc.DayNumber = dayToSelect)
        If dayControlToSelect IsNot Nothing Then
            SelectDay(dayControlToSelect)
        Else ' si no se encuentra, selecciona el primero
            Dim firstDay As DayControl = flpCalendar.Controls.OfType(Of DayControl).FirstOrDefault()
            If firstDay IsNot Nothing Then
                SelectDay(firstDay)
            End If
        End If
    End Sub

    Private Sub DayControl_Clicked(sender As Object, e As EventArgs)
        Dim clickedDay = CType(sender, DayControl)
        SelectDay(clickedDay)
    End Sub

    Private Sub SelectDay(dayCtrl As DayControl)
        If dayCtrl Is Nothing Then Return

        If _selectedDayControl IsNot Nothing Then
            _selectedDayControl.IsSelected = False
        End If

        dayCtrl.IsSelected = True
        _selectedDayControl = dayCtrl

        Dim selectedDate = New Date(_currentDate.Year, _currentDate.Month, dayCtrl.DayNumber)
        ActualizarVistaDeNovedades(selectedDate)
    End Sub

    Private Sub btnPrevMonth_Click(sender As Object, e As EventArgs)
        _currentDate = _currentDate.AddMonths(-1)
        GenerateCalendar()
    End Sub

    Private Sub btnNextMonth_Click(sender As Object, e As EventArgs)
        _currentDate = _currentDate.AddMonths(1)
        GenerateCalendar()
    End Sub

#End Region

#Region "Lógica de Novedades"
    Private Sub ActualizarVistaDeNovedades(selectedDate As Date)
        Dim novedadesDelDia = _novedadesDelPeriodo.
            Where(Function(n) n.Fecha.Date = selectedDate.Date).
            Select(Function(n) New With {.Id = n.Id, n.Fecha, .Texto = n.Texto}).ToList()

        dgvNovedades.DataSource = novedadesDelDia
        ConfigurarGrillaNovedades()
    End Sub

    Private Sub ConfigurarGrillaNovedades()
        If dgvNovedades.Columns.Count > 0 Then Return ' Configurar solo una vez

        dgvNovedades.AutoGenerateColumns = False
        Dim colFecha As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Fecha", .HeaderText = "Fecha", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        }
        colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvNovedades.Columns.Add(colFecha)
        dgvNovedades.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Texto", .HeaderText = "Novedad", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
    End Sub

    Private Sub DgvNovedades_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If dgv.Columns.Contains("Id") Then
            dgv.Columns("Id").Visible = False
        End If
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
#End Region

#Region "Lógica de Estados (La lista con colores)"
    Private Sub PoblarGrillaEstados()
        Dim dataSource = _estados.Select(Function(s)
                                             Dim desde As Date? = Nothing
                                             Dim hasta As Date? = Nothing
                                             Select Case s.TipoEstadoTransitorioId
                                                 Case 1 : desde = s.DesignacionDetalle?.FechaDesde : hasta = s.DesignacionDetalle?.FechaHasta
                                                 Case 2 : desde = s.EnfermedadDetalle?.FechaDesde : hasta = s.EnfermedadDetalle?.FechaHasta
                                                 Case 3 : desde = s.SancionDetalle?.FechaDesde : hasta = s.SancionDetalle?.FechaHasta
                                                 Case 4 : desde = s.OrdenCincoDetalle?.FechaDesde : hasta = s.OrdenCincoDetalle?.FechaHasta
                                                 Case 5 : desde = s.RetenDetalle?.FechaReten
                                                 Case 6 : desde = s.SumarioDetalle?.FechaDesde : hasta = s.SumarioDetalle?.FechaHasta
                                             End Select
                                             Return New With {
                                                    .Tipo = s.TipoEstadoTransitorio.Nombre,
                                                 .Desde = desde,
                                                 .Hasta = hasta,
                                                 .Entity = s
                                             }
                                         End Function).OrderBy(Function(x) x.Desde).ToList()

        dgvEstados.DataSource = dataSource
    End Sub

    Private Sub ConfigurarGrillaEstados()
        If dgvEstados.Columns.Count > 0 Then Return ' Configurar solo una vez

        dgvEstados.AutoGenerateColumns = False
        dgvEstados.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Tipo", .HeaderText = "Tipo de Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
        Dim colDesde As New DataGridViewTextBoxColumn With {.DataPropertyName = "Desde", .HeaderText = "Fecha Desde", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
        colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstados.Columns.Add(colDesde)
        Dim colHasta As New DataGridViewTextBoxColumn With {.DataPropertyName = "Hasta", .HeaderText = "Fecha Hasta", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
        colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstados.Columns.Add(colHasta)
    End Sub

    Private Sub DgvEstados_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If dgv.Columns.Contains("Entity") Then
            dgv.Columns("Entity").Visible = False
        End If
    End Sub

    Private Sub dgvEstados_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 Then Return
        Dim dgv = CType(sender, DataGridView)
        Dim rowData = dgv.Rows(e.RowIndex).DataBoundItem
        Dim estado As EstadoTransitorio = rowData.GetType().GetProperty("Entity").GetValue(rowData, Nothing)

        If estado IsNot Nothing Then
            Select Case estado.TipoEstadoTransitorioId
                Case 1 ' Designación
                    e.CellStyle.BackColor = Color.LightSkyBlue
                Case 2 ' Enfermedad
                    e.CellStyle.BackColor = Color.LightCoral
                Case 3 ' Sanción
                    e.CellStyle.BackColor = Color.Khaki
                Case 4 ' Orden Cinco
                    e.CellStyle.BackColor = Color.Plum
                Case 5 ' Retén
                    e.CellStyle.BackColor = Color.LightGray
                Case 6 ' Sumario
                    e.CellStyle.BackColor = Color.LightSalmon
                Case Else
                    e.CellStyle.BackColor = Color.White
            End Select
        End If
    End Sub

    Private Sub dgvEstados_SelectionChanged(sender As Object, e As EventArgs)
        If dgvEstados.CurrentRow Is Nothing Then Return
        Try
            Dim rowData = dgvEstados.CurrentRow.DataBoundItem
            Dim fechaDesde As Date? = CType(rowData.GetType().GetProperty("Desde").GetValue(rowData, Nothing), Date?)

            If fechaDesde.HasValue AndAlso (fechaDesde.Value.Month <> _currentDate.Month OrElse fechaDesde.Value.Year <> _currentDate.Year) Then
                _currentDate = fechaDesde.Value
                GenerateCalendar()
            End If

        Catch ex As Exception
        End Try
    End Sub
#End Region
End Class