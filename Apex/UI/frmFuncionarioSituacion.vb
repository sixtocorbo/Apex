Imports System.Data.Entity
Imports System.Text
Public Class frmFuncionarioSituacion

    Private _funcionarioId As Integer
    Private _uow As IUnitOfWork
    Private _funcionario As Funcionario
    Private _estados As List(Of EstadoTransitorio)
    Private _novedadesDelPeriodo As List(Of vw_NovedadesCompletas) = New List(Of vw_NovedadesCompletas)

    Private _toolTipData As New Dictionary(Of Date, String)
    Private _lastToolTipDate As Date

    Public Sub New(idFuncionario As Integer)
        InitializeComponent()
        _funcionarioId = idFuncionario
        _uow = New UnitOfWork()
    End Sub

    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        AddHandler dtpAño.ValueChanged, AddressOf dtpAño_ValueChanged
        AddHandler MonthCalendar1.DateChanged, AddressOf MonthCalendar1_DateChanged
        AddHandler dgvNovedades.CellDoubleClick, AddressOf dgvNovedades_CellDoubleClick
        AddHandler MonthCalendar1.MouseMove, AddressOf MonthCalendar1_MouseMove
        AddHandler dgvEstados.CellFormatting, AddressOf dgvEstados_CellFormatting
        AddHandler dgvEstados.SelectionChanged, AddressOf dgvEstados_SelectionChanged
        Await CargarDatos()
    End Sub

    Private Async Sub dtpAño_ValueChanged(sender As Object, e As EventArgs)
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

            Dim fechasConNovedades = _novedadesDelPeriodo.Select(Function(n) n.Fecha).Distinct().ToArray()
            MonthCalendar1.BoldedDates = fechasConNovedades

            GenerarDatosToolTip(fechaInicio, fechaFin)
            PoblarGrillaEstados()
            ActualizarVistaDeNovedades(MonthCalendar1.SelectionStart)

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

#Region "Lógica de Novedades"
    Private Sub ActualizarVistaDeNovedades(fechaVisible As Date)
        Dim primerDiaMes = New Date(fechaVisible.Year, fechaVisible.Month, 1)
        Dim ultimoDiaMes = primerDiaMes.AddMonths(1).AddDays(-1)

        Dim novedadesDelMes = _novedadesDelPeriodo.
            Where(Function(n) n.Fecha >= primerDiaMes AndAlso n.Fecha <= ultimoDiaMes).
            Select(Function(n) New With {.Id = n.Id, n.Fecha, .Texto = n.Texto}).ToList()

        dgvNovedades.DataSource = novedadesDelMes
        ConfigurarGrillaNovedades()
    End Sub

    Private Sub ConfigurarGrillaNovedades()
        If dgvNovedades.DataSource Is Nothing Then Return
        With dgvNovedades
            .SuspendLayout()
            .AutoGenerateColumns = False
            If .Columns.Count = 0 Then
                Dim colFecha As New DataGridViewTextBoxColumn With {.DataPropertyName = "Fecha", .HeaderText = "Fecha", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
                colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns.Add(colFecha)
                .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Texto", .HeaderText = "Novedad", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
                ' (CAMBIO CLAVE) - Nos aseguramos que la columna ID exista pero esté oculta
                .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .Name = "Id", .Visible = False})
            End If
            .ResumeLayout()
        End With
    End Sub

    Private Sub dgvNovedades_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse dgvNovedades.CurrentRow Is Nothing Then Return
        Try
            Dim novedadId = CInt(dgvNovedades.CurrentRow.Cells("Id").Value)
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
        ConfigurarGrillaEstados()
    End Sub

    Private Sub ConfigurarGrillaEstados()
        If dgvEstados.DataSource Is Nothing Then Return
        With dgvEstados
            .SuspendLayout()
            .AutoGenerateColumns = False
            If .Columns.Count = 0 Then
                .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Tipo", .HeaderText = "Tipo de Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
                Dim colDesde As New DataGridViewTextBoxColumn With {.DataPropertyName = "Desde", .HeaderText = "Fecha Desde", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
                colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns.Add(colDesde)
                Dim colHasta As New DataGridViewTextBoxColumn With {.DataPropertyName = "Hasta", .HeaderText = "Fecha Hasta", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
                colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
                .Columns.Add(colHasta)
                ' (CAMBIO CLAVE) - Ocultamos la columna del objeto "proxy"
                .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Entity", .Visible = False})
            End If
            .ResumeLayout()
        End With
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
            If fechaDesde.HasValue Then
                MonthCalendar1.SetDate(fechaDesde.Value)
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region

    Private Sub MonthCalendar1_DateChanged(sender As Object, e As DateRangeEventArgs)
        ActualizarVistaDeNovedades(e.Start)
    End Sub

    Private Sub MonthCalendar1_MouseMove(sender As Object, e As MouseEventArgs)
        Dim hitTest = MonthCalendar1.HitTest(e.Location)
        If hitTest.HitArea = MonthCalendar.HitArea.Date Then
            Dim fecha As Date = hitTest.Time
            If fecha <> _lastToolTipDate Then
                _lastToolTipDate = fecha
                If _toolTipData.ContainsKey(fecha) Then
                    ToolTip1.SetToolTip(MonthCalendar1, _toolTipData(fecha))
                Else
                    ToolTip1.SetToolTip(MonthCalendar1, fecha.ToString("dd MMMM yyyy"))
                End If
            End If
        Else
            _lastToolTipDate = Date.MinValue
            ToolTip1.SetToolTip(MonthCalendar1, String.Empty)
        End If
    End Sub

End Class