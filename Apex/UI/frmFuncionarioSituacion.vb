' Archivo: sixtocorbo/apex/Apex-0de320c5ad8f21b48a295ddfce12e6266297c13c/Apex/UI/frmFuncionarioSituacion.vb

Imports System.Data.Entity
Imports System.Globalization
Imports System.Text

Public Class frmFuncionarioSituacion

    Private _funcionarioId As Integer
    Private _uow As IUnitOfWork
    Private _funcionario As Funcionario
    Private _todosLosEstados As List(Of EstadoTransitorio)
    Private _todasLasNovedades As List(Of vw_NovedadesCompletas)
    Private _selectedTimelineButton As Button = Nothing

    Public Sub New(idFuncionario As Integer)
        InitializeComponent()
        _funcionarioId = idFuncionario
        _uow = New UnitOfWork()
    End Sub

    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        AddHandler btnGenerar.Click, AddressOf btnGenerar_Click
        AddHandler dgvNovedades.CellDoubleClick, AddressOf dgvNovedades_CellDoubleClick
        AddHandler dgvEstados.CellFormatting, AddressOf dgvEstados_CellFormatting
        AddHandler dgvNovedades.DataBindingComplete, AddressOf DgvNovedades_DataBindingComplete
        AddHandler dgvEstados.DataBindingComplete, AddressOf DgvEstados_DataBindingComplete

        ' Establecer un rango de fechas por defecto (ej. el año actual)
        dtpDesde.Value = New Date(Date.Now.Year, 1, 1)
        dtpHasta.Value = New Date(Date.Now.Year, 12, 31)

        Await CargarDatosIniciales()
    End Sub

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

            ' Cargar TODAS las novedades y estados del funcionario una sola vez
            _todosLosEstados = Await _uow.Repository(Of EstadoTransitorio)().GetAll().
                Include(Function(et) et.TipoEstadoTransitorio).
                Include(Function(et) et.DesignacionDetalle).
                Include(Function(et) et.EnfermedadDetalle).
                Include(Function(et) et.SancionDetalle).
                Include(Function(et) et.OrdenCincoDetalle).
                Include(Function(et) et.RetenDetalle).
                Include(Function(et) et.SumarioDetalle).
                Where(Function(et) et.FuncionarioId = _funcionarioId).
                ToListAsync()

            _todasLasNovedades = Await _uow.Context.Set(Of vw_NovedadesCompletas)().
                Where(Function(n) n.FuncionarioId = _funcionario.Id).
                OrderBy(Function(n) n.Fecha).
                ToListAsync()

            ' Generar el timeline inicial con el rango por defecto
            btnGenerar.PerformClick()

        Catch ex As Exception
            MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs)
        Dim fechaInicio = dtpDesde.Value.Date
        Dim fechaFin = dtpHasta.Value.Date

        If fechaInicio > fechaFin Then
            MessageBox.Show("La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'.", "Rango inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        GenerarTimeline(fechaInicio, fechaFin)
        PoblarGrillaEstados(fechaInicio, fechaFin)
    End Sub

    Private Sub GenerarTimeline(fechaInicio As Date, fechaFin As Date)
        flpTimeline.Controls.Clear()
        _selectedTimelineButton = Nothing
        dgvNovedades.DataSource = Nothing

        ' Obtener fechas únicas con novedades en el rango
        Dim fechasConNovedades = _todasLasNovedades.
            Where(Function(n) n.Fecha.Date >= fechaInicio AndAlso n.Fecha.Date <= fechaFin).
            Select(Function(n) n.Fecha.Date).
            Distinct()

        ' Obtener fechas únicas de inicio/fin de estados en el rango
        Dim fechasConEstados = New List(Of Date)
        For Each estado In _todosLosEstados
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
            If desde.HasValue AndAlso desde.Value.Date >= fechaInicio AndAlso desde.Value.Date <= fechaFin Then fechasConEstados.Add(desde.Value.Date)
            If hasta.HasValue AndAlso hasta.Value.Date >= fechaInicio AndAlso hasta.Value.Date <= fechaFin Then fechasConEstados.Add(hasta.Value.Date)
        Next

        ' Combinar todas las fechas, ordenarlas y eliminar duplicados
        Dim todasLasFechas = fechasConNovedades.Union(fechasConEstados).Distinct().OrderBy(Function(d) d).ToList()

        If Not todasLasFechas.Any() Then
            Dim lblEmpty As New Label With {
                .Text = "No hay actividad en el período seleccionado.",
                .AutoSize = True,
                .Margin = New Padding(10)
            }
            flpTimeline.Controls.Add(lblEmpty)
            Return
        End If

        For Each fecha As Date In todasLasFechas
            Dim btnDia As New Button With {
                .Text = fecha.ToString("dd MMM yyyy"),
                .Tag = fecha,
                .Size = New Size(220, 30),
                .FlatStyle = FlatStyle.Flat,
                .TextAlign = ContentAlignment.MiddleLeft
            }
            btnDia.FlatAppearance.BorderSize = 1
            AddHandler btnDia.Click, AddressOf TimelineDia_Click
            flpTimeline.Controls.Add(btnDia)
        Next
    End Sub

    Private Sub TimelineDia_Click(sender As Object, e As EventArgs)
        Dim clickedButton = CType(sender, Button)
        Dim fechaSeleccionada = CType(clickedButton.Tag, Date)

        ' Resaltar el botón seleccionado
        If _selectedTimelineButton IsNot Nothing Then
            _selectedTimelineButton.BackColor = SystemColors.Control
            _selectedTimelineButton.ForeColor = SystemColors.ControlText
        End If
        clickedButton.BackColor = SystemColors.Highlight
        clickedButton.ForeColor = SystemColors.HighlightText
        _selectedTimelineButton = clickedButton

        ' Actualizar la grilla de novedades
        ActualizarVistaDeNovedades(fechaSeleccionada)
    End Sub

#Region "Lógica de Novedades"
    Private Sub ActualizarVistaDeNovedades(selectedDate As Date)
        Dim novedadesDelDia = _todasLasNovedades.
            Where(Function(n) n.Fecha.Date = selectedDate.Date).
            Select(Function(n) New With {.Id = n.Id, n.Fecha, .Texto = n.Texto}).ToList()

        dgvNovedades.DataSource = novedadesDelDia
        ConfigurarGrillaNovedades()
    End Sub

    Private Sub ConfigurarGrillaNovedades()
        If dgvNovedades.Columns.Contains("Fecha") Then Return
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
#End Region

#Region "Lógica de Estados"
    Private Sub PoblarGrillaEstados(fechaInicio As Date, fechaFin As Date)
        Dim estadosEnPeriodo = _todosLosEstados.Where(
            Function(s)
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
                ' Lógica para verificar si el rango del estado se superpone con el rango seleccionado
                If Not desde.HasValue Then Return False
                Dim rangoEstadoFin = If(hasta, desde.Value)
                Return desde.Value <= fechaFin AndAlso rangoEstadoFin >= fechaInicio
            End Function)

        Dim dataSource = estadosEnPeriodo.Select(Function(s)
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
        If dgvEstados.Columns.Contains("Tipo") Then Return
        dgvEstados.AutoGenerateColumns = False
        dgvEstados.Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Tipo", .HeaderText = "Tipo de Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
        Dim colDesde As New DataGridViewTextBoxColumn With {.DataPropertyName = "Desde", .HeaderText = "Fecha Desde", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
        colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstados.Columns.Add(colDesde)
        Dim colHasta As New DataGridViewTextBoxColumn With {.DataPropertyName = "Hasta", .HeaderText = "Fecha Hasta", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
        colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstados.Columns.Add(colHasta)
    End Sub
#End Region

#Region "Eventos Comunes de Grillas y otros"
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
        If e.RowIndex < 0 Then Return
        Dim dgv = CType(sender, DataGridView)
        Dim rowData = dgv.Rows(e.RowIndex).DataBoundItem
        Dim estado As EstadoTransitorio = rowData.GetType().GetProperty("Entity").GetValue(rowData, Nothing)
        If estado IsNot Nothing Then
            Select Case estado.TipoEstadoTransitorioId
                Case 1 : e.CellStyle.BackColor = Color.LightSkyBlue
                Case 2 : e.CellStyle.BackColor = Color.LightCoral
                Case 3 : e.CellStyle.BackColor = Color.Khaki
                Case 4 : e.CellStyle.BackColor = Color.Plum
                Case 5 : e.CellStyle.BackColor = Color.LightGray
                Case 6 : e.CellStyle.BackColor = Color.LightSalmon
                Case Else : e.CellStyle.BackColor = Color.White
            End Select
        End If
    End Sub

    Private Sub DgvNovedades_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If dgv.Columns.Contains("Id") Then dgv.Columns("Id").Visible = False
    End Sub

    Private Sub DgvEstados_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If dgv.Columns.Contains("Entity") Then dgv.Columns("Entity").Visible = False
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown

        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

#End Region
End Class