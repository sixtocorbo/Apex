Imports System.Data.Entity
Imports System.Globalization
Imports System.Text

Public Class frmFuncionarioSituacion

    Private _funcionarioId As Integer
    Private _uow As IUnitOfWork
    Private _funcionario As Funcionario

    ' Estados transitorios (designación, enfermedad, sanción, O5, retén, sumario)
    Private _todosLosEstados As List(Of EstadoTransitorio)

    ' Novedades (vista)
    Private _todasLasNovedades As List(Of vw_NovedadesCompletas)

    ' *** NUEVO: licencias del funcionario ***
    Private _todasLasLicencias As List(Of HistoricoLicencia)

    Private _selectedTimelineButton As Button = Nothing

    Public Sub New(idFuncionario As Integer)
        InitializeComponent()
        _funcionarioId = idFuncionario
        _uow = New UnitOfWork()
    End Sub

    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' Handlers UI (quitamos el de btnGenerar)
        AddHandler dgvNovedades.CellDoubleClick, AddressOf dgvNovedades_CellDoubleClick
        AddHandler dgvEstados.CellFormatting, AddressOf dgvEstados_CellFormatting
        AddHandler dgvNovedades.DataBindingComplete, AddressOf DgvNovedades_DataBindingComplete
        AddHandler dgvEstados.DataBindingComplete, AddressOf DgvEstados_DataBindingComplete

        ' Fechas por defecto: HOY — HOY
        dtpDesde.Value = Date.Today
        dtpHasta.Value = Date.Today

        ' Ocultar/eliminar el botón Generar (si existe en el diseñador)
        If btnGenerar IsNot Nothing Then btnGenerar.Visible = False

        ' Actualizar al cambiar fechas
        AddHandler dtpDesde.ValueChanged, AddressOf RangoFechas_Cambio
        AddHandler dtpHasta.ValueChanged, AddressOf RangoFechas_Cambio

        Await CargarDatosIniciales()

        ' Primera carga con hoy–hoy
        ActualizarTodo()
    End Sub
    Private Sub RangoFechas_Cambio(sender As Object, e As EventArgs)
        ' Asegurar coherencia del rango (si Desde > Hasta, igualamos Hasta)
        If dtpDesde.Value.Date > dtpHasta.Value.Date Then
            dtpHasta.Value = dtpDesde.Value.Date
        End If
        ActualizarTodo()
    End Sub

    Private Sub ActualizarTodo()
        Dim fechaInicio = dtpDesde.Value.Date
        Dim fechaFin = dtpHasta.Value.Date

        GenerarTimeline(fechaInicio, fechaFin)   ' SOLO novedades
        PoblarGrillaEstados(fechaInicio, fechaFin) ' estados + licencias
        SeleccionarPrimerDiaSiExiste()
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

            ' Cargar todo una vez (carga ansiosa)
            _todosLosEstados = Await _uow.Repository(Of EstadoTransitorio)().GetAll().
                Include(Function(et) et.TipoEstadoTransitorio).
                Include(Function(et) et.DesignacionDetalle).
                Include(Function(et) et.EnfermedadDetalle).
                Include(Function(et) et.SancionDetalle).
                Include(Function(et) et.OrdenCincoDetalle).
                Include(Function(et) et.RetenDetalle).
                Include(Function(et) et.SumarioDetalle).
                Where(Function(et) et.FuncionarioId = _funcionarioId).
                AsNoTracking().
                ToListAsync()

            _todasLasNovedades = Await _uow.Context.Set(Of vw_NovedadesCompletas)().
                Where(Function(n) n.FuncionarioId = _funcionario.Id).
                OrderBy(Function(n) n.Fecha).
                AsNoTracking().
                ToListAsync()

            ' *** NUEVO: cargar licencias del funcionario ***
            _todasLasLicencias = Await _uow.Context.Set(Of HistoricoLicencia)() _
                .Include(Function(l) l.TipoLicencia) _
                .Where(Function(l) l.FuncionarioId = _funcionarioId) _
                .AsNoTracking() _
                .ToListAsync()

            ' Generar vista inicial con el rango por defecto
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

        ' Si hay días, seleccionar el primero automáticamente
        SeleccionarPrimerDiaSiExiste()
    End Sub

    Private Sub GenerarTimeline(fechaInicio As Date, fechaFin As Date)
        flpTimeline.SuspendLayout()
        flpTimeline.Controls.Clear()
        _selectedTimelineButton = Nothing
        dgvNovedades.DataSource = Nothing

        ' SOLO NOVEDADES dentro del rango
        Dim fechasNovedades = _todasLasNovedades _
        .Where(Function(n) n.Fecha.Date >= fechaInicio AndAlso n.Fecha.Date <= fechaFin) _
        .Select(Function(n) n.Fecha.Date) _
        .Distinct() _
        .OrderBy(Function(d) d) _
        .ToList()

        If fechasNovedades.Count = 0 Then
            Dim lblEmpty As New Label With {
            .Text = "No hay novedades en el período seleccionado.",
            .AutoSize = True,
            .Margin = New Padding(10)
        }
            flpTimeline.Controls.Add(lblEmpty)
            flpTimeline.ResumeLayout()
            Return
        End If

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

        ' Resaltar seleccionado
        If _selectedTimelineButton IsNot Nothing Then
            _selectedTimelineButton.BackColor = SystemColors.Control
            _selectedTimelineButton.ForeColor = SystemColors.ControlText
        End If
        clickedButton.BackColor = SystemColors.Highlight
        clickedButton.ForeColor = SystemColors.HighlightText
        _selectedTimelineButton = clickedButton

        ' Actualizar novedades del día
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
        If dgvNovedades.Columns.Count = 0 OrElse Not dgvNovedades.Columns.Contains("Fecha") Then
            dgvNovedades.AutoGenerateColumns = False
            dgvNovedades.Columns.Clear()

            Dim colFecha As New DataGridViewTextBoxColumn With {
                .Name = "Fecha",
                .DataPropertyName = "Fecha",
                .HeaderText = "Fecha",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            }
            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvNovedades.Columns.Add(colFecha)

            dgvNovedades.Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Texto",
                .DataPropertyName = "Texto",
                .HeaderText = "Novedad",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            })
        End If
    End Sub
#End Region

#Region "Lógica de Estados + Licencias"
    Private Sub PoblarGrillaEstados(fechaInicio As Date, fechaFin As Date)

        ' Función auxiliar: ¿[desde, hasta] se solapa con [fechaInicio, fechaFin]?
        Dim overlaps As Func(Of Date?, Date?, Boolean) =
        Function(desde As Date?, hasta As Date?) As Boolean
            If Not desde.HasValue Then Return False
            ' Si no hay fin, considerar abierto hasta "infinito"
            Dim finEfectivo As Date = If(hasta.HasValue, hasta.Value, Date.MaxValue)
            Return (desde.Value <= fechaFin) AndAlso (finEfectivo >= fechaInicio)
        End Function

        Dim estadosEnPeriodo = _todosLosEstados.Where(Function(s)
                                                          Dim desde As Date? = Nothing
                                                          Dim hasta As Date? = Nothing

                                                          Select Case s.TipoEstadoTransitorioId
                                                              Case 1 ' Designación
                                                                  desde = s.DesignacionDetalle?.FechaDesde
                                                                  ' Normaliza: si el campo viene como 0001-01-01, tomarlo como Nothing
                                                                  Dim h As Date? = s.DesignacionDetalle?.FechaHasta
                                                                  If h.HasValue AndAlso h.Value = Date.MinValue Then h = Nothing
                                                                  hasta = h

                                                              Case 2 ' Enfermedad
                                                                  desde = s.EnfermedadDetalle?.FechaDesde
                                                                  Dim h As Date? = s.EnfermedadDetalle?.FechaHasta
                                                                  If h.HasValue AndAlso h.Value = Date.MinValue Then h = Nothing
                                                                  hasta = h

                                                              Case 3 ' Sanción
                                                                  desde = s.SancionDetalle?.FechaDesde
                                                                  Dim h As Date? = s.SancionDetalle?.FechaHasta
                                                                  If h.HasValue AndAlso h.Value = Date.MinValue Then h = Nothing
                                                                  hasta = h

                                                              Case 4 ' Orden Cinco
                                                                  desde = s.OrdenCincoDetalle?.FechaDesde
                                                                  Dim h As Date? = s.OrdenCincoDetalle?.FechaHasta
                                                                  If h.HasValue AndAlso h.Value = Date.MinValue Then h = Nothing
                                                                  hasta = h

                                                              Case 5 ' Retén (fecha única)
                                                                  desde = s.RetenDetalle?.FechaReten
                                                                  hasta = desde

                                                              Case 6 ' Sumario
                                                                  desde = s.SumarioDetalle?.FechaDesde
                                                                  Dim h As Date? = s.SumarioDetalle?.FechaHasta
                                                                  If h.HasValue AndAlso h.Value = Date.MinValue Then h = Nothing
                                                                  hasta = h
                                                          End Select

                                                          Return overlaps(desde, hasta)
                                                      End Function)

        Dim dataSource = estadosEnPeriodo.Select(Function(s)
                                                     Dim desde As Date? = Nothing
                                                     Dim hasta As Date? = Nothing

                                                     Select Case s.TipoEstadoTransitorioId
                                                         Case 1 : desde = s.DesignacionDetalle?.FechaDesde : hasta = s.DesignacionDetalle?.FechaHasta
                                                         Case 2 : desde = s.EnfermedadDetalle?.FechaDesde : hasta = s.EnfermedadDetalle?.FechaHasta
                                                         Case 3 : desde = s.SancionDetalle?.FechaDesde : hasta = s.SancionDetalle?.FechaHasta
                                                         Case 4 : desde = s.OrdenCincoDetalle?.FechaDesde : hasta = s.OrdenCincoDetalle?.FechaHasta
                                                         Case 5 : desde = s.RetenDetalle?.FechaReten : hasta = s.RetenDetalle?.FechaReten
                                                         Case 6 : desde = s.SumarioDetalle?.FechaDesde : hasta = s.SumarioDetalle?.FechaHasta
                                                     End Select

                                                     ' Limpia Date.MinValue -> Nothing para que la grilla quede en blanco (vigente)
                                                     If hasta.HasValue AndAlso hasta.Value = Date.MinValue Then hasta = Nothing

                                                     Return New With {
            .Tipo = s.TipoEstadoTransitorio.Nombre,
            .Desde = desde,
            .Hasta = hasta,
            .Entity = s
        }
                                                 End Function).
    OrderBy(Function(x) x.Desde).
    ToList()

        dgvEstados.DataSource = dataSource
        ConfigurarGrillaEstados()
    End Sub


    Private Sub ConfigurarGrillaEstados()
        If dgvEstados.Columns.Count = 0 OrElse Not dgvEstados.Columns.Contains("Tipo") Then
            dgvEstados.AutoGenerateColumns = False
            dgvEstados.Columns.Clear()

            dgvEstados.Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Tipo",
                .DataPropertyName = "Tipo",
                .HeaderText = "Tipo",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            })

            Dim colDesde As New DataGridViewTextBoxColumn With {
                .Name = "Desde",
                .DataPropertyName = "Desde",
                .HeaderText = "Desde",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            }
            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvEstados.Columns.Add(colDesde)

            Dim colHasta As New DataGridViewTextBoxColumn With {
                .Name = "Hasta",
                .DataPropertyName = "Hasta",
                .HeaderText = "Hasta",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            }
            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
            dgvEstados.Columns.Add(colHasta)
        End If
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
        If rowData Is Nothing Then Return

        ' Si es licencia, color propio y salir
        Dim origenProp = rowData.GetType().GetProperty("Origen")
        If origenProp IsNot Nothing AndAlso CStr(origenProp.GetValue(rowData, Nothing)) = "Licencia" Then
            e.CellStyle.BackColor = Color.LightGreen
            Return
        End If

        ' Estados transitorios (como lo tenías)
        Dim estado As EstadoTransitorio = TryCast(rowData.GetType().GetProperty("Entity")?.GetValue(rowData, Nothing), EstadoTransitorio)
        If estado Is Nothing Then Return

        Select Case estado.TipoEstadoTransitorioId
            Case 1 : e.CellStyle.BackColor = Color.LightSkyBlue     ' Designación
            Case 2 : e.CellStyle.BackColor = Color.LightCoral       ' Enfermedad
            Case 3 : e.CellStyle.BackColor = Color.Khaki            ' Sanción
            Case 4 : e.CellStyle.BackColor = Color.Plum             ' Orden Cinco
            Case 5 : e.CellStyle.BackColor = Color.LightGray        ' Retén
            Case 6 : e.CellStyle.BackColor = Color.LightSalmon      ' Sumario
            Case Else : e.CellStyle.BackColor = Color.White
        End Select
    End Sub

    Private Sub DgvNovedades_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If dgv.Columns.Contains("Id") Then dgv.Columns("Id").Visible = False
    End Sub

    Private Sub DgvEstados_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        If dgv.Columns.Contains("Entity") Then dgv.Columns("Entity").Visible = False
        If dgv.Columns.Contains("EntityLic") Then dgv.Columns("EntityLic").Visible = False
        If dgv.Columns.Contains("Origen") Then dgv.Columns("Origen").Visible = False
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
#End Region

End Class

'Imports System.Data.Entity
'Imports System.Globalization
'Imports System.Text

'Public Class frmFuncionarioSituacion

'    Private _funcionarioId As Integer
'    Private _uow As IUnitOfWork
'    Private _funcionario As Funcionario
'    Private _todosLosEstados As List(Of EstadoTransitorio)
'    Private _todasLasNovedades As List(Of vw_NovedadesCompletas)
'    Private _selectedTimelineButton As Button = Nothing
'    Private _todasLasLicencias As List(Of HistoricoLicencia)

'    Public Sub New(idFuncionario As Integer)
'        InitializeComponent()
'        _funcionarioId = idFuncionario
'        _uow = New UnitOfWork()
'    End Sub

'    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        AppTheme.Aplicar(Me)

'        ' Handlers UI
'        AddHandler btnGenerar.Click, AddressOf btnGenerar_Click
'        AddHandler dgvNovedades.CellDoubleClick, AddressOf dgvNovedades_CellDoubleClick
'        AddHandler dgvEstados.CellFormatting, AddressOf dgvEstados_CellFormatting
'        AddHandler dgvNovedades.DataBindingComplete, AddressOf DgvNovedades_DataBindingComplete
'        AddHandler dgvEstados.DataBindingComplete, AddressOf DgvEstados_DataBindingComplete

'        ' Rango por defecto: año actual
'        dtpDesde.Value = New Date(Date.Now.Year, 1, 1)
'        dtpHasta.Value = New Date(Date.Now.Year, 12, 31)

'        Await CargarDatosIniciales()
'    End Sub

'    Private Async Function CargarDatosIniciales() As Task
'        LoadingHelper.MostrarCargando(Me)
'        Try
'            _funcionario = Await _uow.Context.Set(Of Funcionario)().FindAsync(_funcionarioId)
'            If _funcionario Is Nothing Then
'                MessageBox.Show("No se encontró el funcionario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                Close()
'                Return
'            End If

'            lblNombre.Text = $"Situación de: {_funcionario.Nombre} ({_funcionario.CI})"

'            ' Cargar todo una vez (carga ansiosa)
'            _todosLosEstados = Await _uow.Repository(Of EstadoTransitorio)().GetAll().
'                Include(Function(et) et.TipoEstadoTransitorio).
'                Include(Function(et) et.DesignacionDetalle).
'                Include(Function(et) et.EnfermedadDetalle).
'                Include(Function(et) et.SancionDetalle).
'                Include(Function(et) et.OrdenCincoDetalle).
'                Include(Function(et) et.RetenDetalle).
'                Include(Function(et) et.SumarioDetalle).
'                Where(Function(et) et.FuncionarioId = _funcionarioId).
'                AsNoTracking().
'                ToListAsync()

'            _todasLasNovedades = Await _uow.Context.Set(Of vw_NovedadesCompletas)().
'                Where(Function(n) n.FuncionarioId = _funcionario.Id).
'                OrderBy(Function(n) n.Fecha).
'                AsNoTracking().
'                ToListAsync()
'            _todasLasLicencias = Await _uow.Context.Set(Of HistoricoLicencia)() _
'    .Include("TipoLicencia") _
'    .Where(Function(l) l.FuncionarioId = _funcionarioId) _
'    .AsNoTracking() _
'    .ToListAsync()
'            ' Generar vista inicial con el rango por defecto
'            btnGenerar.PerformClick()

'        Catch ex As Exception
'            MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        Finally
'            LoadingHelper.OcultarCargando(Me)
'        End Try
'    End Function

'    Private Sub btnGenerar_Click(sender As Object, e As EventArgs)
'        Dim fechaInicio = dtpDesde.Value.Date
'        Dim fechaFin = dtpHasta.Value.Date

'        If fechaInicio > fechaFin Then
'            MessageBox.Show("La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'.", "Rango inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'            Return
'        End If

'        GenerarTimeline(fechaInicio, fechaFin)
'        PoblarGrillaEstados(fechaInicio, fechaFin)

'        ' Si hay días, seleccionar el primero automáticamente
'        SeleccionarPrimerDiaSiExiste()
'    End Sub

'    Private Sub GenerarTimeline(fechaInicio As Date, fechaFin As Date)
'        flpTimeline.SuspendLayout()
'        flpTimeline.Controls.Clear()
'        _selectedTimelineButton = Nothing
'        dgvNovedades.DataSource = Nothing

'        ' Fechas con novedades en el rango
'        Dim fechasConNovedades = _todasLasNovedades.
'            Where(Function(n) n.Fecha.Date >= fechaInicio AndAlso n.Fecha.Date <= fechaFin).
'            Select(Function(n) n.Fecha.Date).
'            Distinct()

'        ' Fechas con estados (inicios/fin) en el rango
'        Dim fechasConEstados = New List(Of Date)
'        For Each estado In _todosLosEstados
'            Dim desde As Date? = Nothing
'            Dim hasta As Date? = Nothing
'            Select Case estado.TipoEstadoTransitorioId
'                Case 1 : desde = estado.DesignacionDetalle?.FechaDesde : hasta = estado.DesignacionDetalle?.FechaHasta
'                Case 2 : desde = estado.EnfermedadDetalle?.FechaDesde : hasta = estado.EnfermedadDetalle?.FechaHasta
'                Case 3 : desde = estado.SancionDetalle?.FechaDesde : hasta = estado.SancionDetalle?.FechaHasta
'                Case 4 : desde = estado.OrdenCincoDetalle?.FechaDesde : hasta = estado.OrdenCincoDetalle?.FechaHasta
'                Case 5 : desde = estado.RetenDetalle?.FechaReten
'                Case 6 : desde = estado.SumarioDetalle?.FechaDesde : hasta = estado.SumarioDetalle?.FechaHasta
'            End Select
'            If desde.HasValue AndAlso desde.Value.Date >= fechaInicio AndAlso desde.Value.Date <= fechaFin Then fechasConEstados.Add(desde.Value.Date)
'            If hasta.HasValue AndAlso hasta.Value.Date >= fechaInicio AndAlso hasta.Value.Date <= fechaFin Then fechasConEstados.Add(hasta.Value.Date)
'        Next

'        Dim todasLasFechas = fechasConNovedades.Union(fechasConEstados).Distinct().OrderBy(Function(d) d).ToList()

'        If Not todasLasFechas.Any() Then
'            Dim lblEmpty As New Label With {
'                .Text = "No hay actividad en el período seleccionado.",
'                .AutoSize = True,
'                .Margin = New Padding(10)
'            }
'            flpTimeline.Controls.Add(lblEmpty)
'            flpTimeline.ResumeLayout()
'            Return
'        End If

'        For Each fecha As Date In todasLasFechas
'            Dim btnDia As New Button With {
'                .Text = fecha.ToString("dd MMM yyyy"),
'                .Tag = fecha,
'                .Width = flpTimeline.ClientSize.Width - 20, ' margen de padding
'                .Height = 30,
'                .FlatStyle = FlatStyle.Flat,
'                .TextAlign = ContentAlignment.MiddleLeft,
'                .Margin = New Padding(0, 0, 0, 6)
'            }
'            btnDia.FlatAppearance.BorderSize = 1
'            AddHandler btnDia.Click, AddressOf TimelineDia_Click
'            flpTimeline.Controls.Add(btnDia)
'        Next

'        flpTimeline.ResumeLayout()
'    End Sub

'    Private Sub SeleccionarPrimerDiaSiExiste()
'        Dim firstBtn = flpTimeline.Controls.OfType(Of Button)().FirstOrDefault()
'        If firstBtn IsNot Nothing Then
'            TimelineDia_Click(firstBtn, EventArgs.Empty)
'        End If
'    End Sub

'    Private Sub TimelineDia_Click(sender As Object, e As EventArgs)
'        Dim clickedButton = CType(sender, Button)
'        Dim fechaSeleccionada = CType(clickedButton.Tag, Date)

'        ' Resaltar seleccionado
'        If _selectedTimelineButton IsNot Nothing Then
'            _selectedTimelineButton.BackColor = SystemColors.Control
'            _selectedTimelineButton.ForeColor = SystemColors.ControlText
'        End If
'        clickedButton.BackColor = SystemColors.Highlight
'        clickedButton.ForeColor = SystemColors.HighlightText
'        _selectedTimelineButton = clickedButton

'        ' Actualizar novedades del día
'        ActualizarVistaDeNovedades(fechaSeleccionada)
'    End Sub

'#Region "Lógica de Novedades"
'    Private Sub ActualizarVistaDeNovedades(selectedDate As Date)
'        Dim novedadesDelDia = _todasLasNovedades.
'            Where(Function(n) n.Fecha.Date = selectedDate.Date).
'            Select(Function(n) New With {.Id = n.Id, n.Fecha, .Texto = n.Texto}).ToList()

'        dgvNovedades.DataSource = novedadesDelDia
'        ConfigurarGrillaNovedades()
'    End Sub

'    Private Sub ConfigurarGrillaNovedades()
'        If dgvNovedades.Columns.Count = 0 OrElse Not dgvNovedades.Columns.Contains("Fecha") Then
'            dgvNovedades.AutoGenerateColumns = False
'            dgvNovedades.Columns.Clear()

'            Dim colFecha As New DataGridViewTextBoxColumn With {
'                .Name = "Fecha",
'                .DataPropertyName = "Fecha",
'                .HeaderText = "Fecha",
'                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
'            }
'            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
'            dgvNovedades.Columns.Add(colFecha)

'            dgvNovedades.Columns.Add(New DataGridViewTextBoxColumn With {
'                .Name = "Texto",
'                .DataPropertyName = "Texto",
'                .HeaderText = "Novedad",
'                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
'            })
'        End If
'    End Sub
'#End Region

'#Region "Lógica de Estados"
'    Private Sub PoblarGrillaEstados(fechaInicio As Date, fechaFin As Date)
'        Dim estadosEnPeriodo = _todosLosEstados.Where(
'            Function(s)
'                Dim desde As Date? = Nothing
'                Dim hasta As Date? = Nothing
'                Select Case s.TipoEstadoTransitorioId
'                    Case 1 : desde = s.DesignacionDetalle?.FechaDesde : hasta = s.DesignacionDetalle?.FechaHasta
'                    Case 2 : desde = s.EnfermedadDetalle?.FechaDesde : hasta = s.EnfermedadDetalle?.FechaHasta
'                    Case 3 : desde = s.SancionDetalle?.FechaDesde : hasta = s.SancionDetalle?.FechaHasta
'                    Case 4 : desde = s.OrdenCincoDetalle?.FechaDesde : hasta = s.OrdenCincoDetalle?.FechaHasta
'                    Case 5 : desde = s.RetenDetalle?.FechaReten
'                    Case 6 : desde = s.SumarioDetalle?.FechaDesde : hasta = s.SumarioDetalle?.FechaHasta
'                End Select

'                If Not desde.HasValue Then Return False
'                Dim rangoEstadoFin = If(hasta, desde.Value)
'                Return desde.Value <= fechaFin AndAlso rangoEstadoFin >= fechaInicio
'            End Function)

'        Dim dataSource = estadosEnPeriodo.Select(Function(s)
'                                                     Dim desde As Date? = Nothing
'                                                     Dim hasta As Date? = Nothing
'                                                     Select Case s.TipoEstadoTransitorioId
'                                                         Case 1 : desde = s.DesignacionDetalle?.FechaDesde : hasta = s.DesignacionDetalle?.FechaHasta
'                                                         Case 2 : desde = s.EnfermedadDetalle?.FechaDesde : hasta = s.EnfermedadDetalle?.FechaHasta
'                                                         Case 3 : desde = s.SancionDetalle?.FechaDesde : hasta = s.SancionDetalle?.FechaHasta
'                                                         Case 4 : desde = s.OrdenCincoDetalle?.FechaDesde : hasta = s.OrdenCincoDetalle?.FechaHasta
'                                                         Case 5 : desde = s.RetenDetalle?.FechaReten
'                                                         Case 6 : desde = s.SumarioDetalle?.FechaDesde : hasta = s.SumarioDetalle?.FechaHasta
'                                                     End Select
'                                                     Return New With {
'                                                         .Tipo = s.TipoEstadoTransitorio.Nombre,
'                                                         .Desde = desde,
'                                                         .Hasta = hasta,
'                                                         .Entity = s
'                                                     }
'                                                 End Function).
'                                                 OrderBy(Function(x) x.Desde).
'                                                 ToList()

'        dgvEstados.DataSource = dataSource
'        ConfigurarGrillaEstados()
'    End Sub

'    Private Sub ConfigurarGrillaEstados()
'        If dgvEstados.Columns.Count = 0 OrElse Not dgvEstados.Columns.Contains("Tipo") Then
'            dgvEstados.AutoGenerateColumns = False
'            dgvEstados.Columns.Clear()

'            dgvEstados.Columns.Add(New DataGridViewTextBoxColumn With {
'                .Name = "Tipo",
'                .DataPropertyName = "Tipo",
'                .HeaderText = "Tipo de Estado",
'                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
'            })

'            Dim colDesde As New DataGridViewTextBoxColumn With {
'                .Name = "Desde",
'                .DataPropertyName = "Desde",
'                .HeaderText = "Fecha Desde",
'                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
'            }
'            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
'            dgvEstados.Columns.Add(colDesde)

'            Dim colHasta As New DataGridViewTextBoxColumn With {
'                .Name = "Hasta",
'                .DataPropertyName = "Hasta",
'                .HeaderText = "Fecha Hasta",
'                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
'            }
'            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
'            dgvEstados.Columns.Add(colHasta)
'        End If
'    End Sub
'#End Region

'#Region "Eventos Comunes de Grillas y otros"
'    Private Sub dgvNovedades_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
'        If e.RowIndex < 0 OrElse dgvNovedades.CurrentRow Is Nothing Then Return
'        Try
'            Dim rowData = dgvNovedades.CurrentRow.DataBoundItem
'            Dim novedadId = CInt(rowData.GetType().GetProperty("Id").GetValue(rowData, Nothing))

'            If novedadId > 0 Then
'                Using frm As New frmNovedadCrear(novedadId)
'                    frm.ShowDialog(Me)
'                End Using
'            End If
'        Catch ex As Exception
'            MessageBox.Show("No se pudo abrir el detalle de la novedad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'        End Try
'    End Sub

'    Private Sub dgvEstados_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
'        If e.RowIndex < 0 Then Return
'        Dim dgv = CType(sender, DataGridView)
'        Dim rowData = dgv.Rows(e.RowIndex).DataBoundItem
'        If rowData Is Nothing Then Return

'        Dim estado As EstadoTransitorio = TryCast(rowData.GetType().GetProperty("Entity")?.GetValue(rowData, Nothing), EstadoTransitorio)
'        If estado Is Nothing Then Return

'        Select Case estado.TipoEstadoTransitorioId
'            Case 1 : e.CellStyle.BackColor = Color.LightSkyBlue     ' Designación
'            Case 2 : e.CellStyle.BackColor = Color.LightCoral       ' Enfermedad
'            Case 3 : e.CellStyle.BackColor = Color.Khaki            ' Sanción
'            Case 4 : e.CellStyle.BackColor = Color.Plum             ' Orden Cinco
'            Case 5 : e.CellStyle.BackColor = Color.LightGray        ' Retén
'            Case 6 : e.CellStyle.BackColor = Color.LightSalmon      ' Sumario
'            Case Else : e.CellStyle.BackColor = Color.White
'        End Select
'    End Sub

'    Private Sub DgvNovedades_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
'        Dim dgv = CType(sender, DataGridView)
'        If dgv.Columns.Contains("Id") Then dgv.Columns("Id").Visible = False
'    End Sub

'    Private Sub DgvEstados_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
'        Dim dgv = CType(sender, DataGridView)
'        If dgv.Columns.Contains("Entity") Then dgv.Columns("Entity").Visible = False
'    End Sub

'    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
'        If e.KeyCode = Keys.Escape Then
'            Me.Close()
'        End If
'    End Sub
'#End Region

'End Class
