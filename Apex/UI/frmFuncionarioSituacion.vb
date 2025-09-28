Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Text

Public Class frmFuncionarioSituacion
    Inherits FormActualizable

#Region " Campos y Constructor "

    Private ReadOnly _funcionarioId As Integer
    Private ReadOnly _uow As IUnitOfWork
    Private _funcionario As Funcionario
    Private _todasLasNovedades As List(Of vw_NovedadesCompletas) = New List(Of vw_NovedadesCompletas)()
    Private _selectedTimelineButton As Button = Nothing

    Public Sub New(idFuncionario As Integer)
        InitializeComponent()
        _funcionarioId = idFuncionario
        _uow = New UnitOfWork()
    End Sub

#End Region

#Region " Carga y Ciclo de Vida "

    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        dgvNovedades.ActivarDobleBuffer(True)
        dgvEstados.ActivarDobleBuffer(True)

        'AddHandler dgvNovedades.CellDoubleClick, AddressOf dgvNovedades_CellDoubleClick
        AddHandler dgvNovedades.DataBindingComplete, AddressOf DgvNovedades_DataBindingComplete
        AddHandler dgvEstados.DataBindingComplete, AddressOf DgvEstados_DataBindingComplete
        AddHandler dgvEstados.CellDoubleClick, AddressOf dgvEstados_CellDoubleClick

        dtpDesde.Value = Date.Today
        dtpHasta.Value = Date.Today

        If btnGenerar IsNot Nothing Then btnGenerar.Visible = True

        Await CargarDatosEsenciales()
        Await ActualizarTodo()
    End Sub

    Protected Overrides Async Function RefrescarSegunFuncionarioAsync(e As FuncionarioCambiadoEventArgs) As Task
        If e IsNot Nothing AndAlso e.FuncionarioId.HasValue AndAlso e.FuncionarioId.Value <> _funcionarioId Then Return
        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Return
        Await CargarDatosEsenciales()
        Await ActualizarTodo()
    End Function

    Private Sub frmFuncionarioSituacion_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        _uow.Dispose()
    End Sub

    Private Async Function CargarDatosEsenciales() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            _funcionario = Await _uow.Context.Set(Of Funcionario)().FindAsync(_funcionarioId)
            If _funcionario Is Nothing Then
                Notifier.[Error](Me, "No se encontró el funcionario.")
                Close()
                Return
            End If

            lblNombre.Text = $"Situación de: {_funcionario.Nombre} ({_funcionario.CI})"

            _todasLasNovedades = Await _uow.Context.Set(Of vw_NovedadesCompletas)() _
                .Where(Function(n) n.FuncionarioId = _funcionario.Id) _
                .AsNoTracking() _
                .ToListAsync()

        Catch ex As Exception
            Notifier.[Error](Me, $"Error al cargar los datos iniciales: {ex.Message}")
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region " Actualización / Eventos "

    Private Async Function ActualizarTodo() As Task
        Dim fechaInicio = dtpDesde.Value.Date
        Dim fechaFin = dtpHasta.Value.Date.AddDays(1) ' semirango

        GenerarTimeline(fechaInicio, fechaFin)
        Await PoblarGrillaEstados(fechaInicio, fechaFin)
        SeleccionarPrimerDiaSiExiste()
    End Function

    Private Async Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Await ActualizarTodo()
    End Sub

#End Region

#Region " Timeline de Novedades (igual) "

    Private Sub GenerarTimeline(fechaInicio As Date, fechaFin As Date)
        flpTimeline.SuspendLayout()
        flpTimeline.Controls.Clear()
        _selectedTimelineButton = Nothing
        dgvNovedades.DataSource = Nothing

        Dim fechasNovedades = _todasLasNovedades _
            .Where(Function(n) n.Fecha.Date >= fechaInicio AndAlso n.Fecha.Date < fechaFin) _
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
        Dim novedadesDelDia = _todasLasNovedades _
            .Where(Function(n) n.Fecha.Date = selectedDate.Date) _
            .OrderByDescending(Function(n) n.Id) _
            .Select(Function(n) New With {.Id = n.Id, n.Fecha, .Texto = n.Texto}).ToList()

        dgvNovedades.DataSource = novedadesDelDia
    End Sub

#End Region

#Region " Grilla de Estados (actualizado) "

    Private Async Function PoblarGrillaEstados(fechaInicio As Date, fechaFin As Date) As Task
        LoadingHelper.MostrarCargando(dgvEstados)
        Try
            ' 1) Estados transitorios en el período
            Dim estadosEnPeriodo = Await _uow.Context.Set(Of vw_EstadosTransitoriosCompletos)() _
                .Where(Function(e) e.FuncionarioId = _funcionarioId AndAlso
                                    e.FechaDesde.HasValue AndAlso
                                    e.FechaDesde.Value < fechaFin AndAlso
                                    (Not e.FechaHasta.HasValue OrElse e.FechaHasta.Value >= fechaInicio)) _
                .AsNoTracking().ToListAsync()

            ' 2) Licencias en el período
            Dim licenciasEnPeriodo = Await _uow.Context.Set(Of HistoricoLicencia)() _
                .Include(Function(l) l.TipoLicencia) _
                .Where(Function(l) l.FuncionarioId = _funcionarioId AndAlso
                                        l.inicio < fechaFin AndAlso
                                        l.finaliza >= fechaInicio) _
                .AsNoTracking().ToListAsync()

            ' 3) Notificaciones PENDIENTES en el período
            Dim estadoPendienteId As Byte = CByte(ModConstantesApex.EstadoNotificacionPersonal.Pendiente)
            Dim notificacionesEnPeriodo = Await _uow.Context.Set(Of NotificacionPersonal)() _
                .Include(Function(n) n.TipoNotificacion) _
                .Where(Function(n) n.FuncionarioId = _funcionarioId AndAlso
                                    n.EstadoId = estadoPendienteId AndAlso
                                    n.FechaProgramada >= fechaInicio AndAlso
                                    n.FechaProgramada < fechaFin) _
                .AsNoTracking().ToListAsync()

            ' 4) Cambios de Auditoría
            Dim funcionarioIdStr = _funcionarioId.ToString()
            Dim cambiosAuditados = Await _uow.Context.Set(Of AuditoriaCambios)() _
                .Where(Function(a) a.TablaNombre = "Funcionario" AndAlso
                                    a.RegistroId = funcionarioIdStr AndAlso
                                    a.FechaHora >= fechaInicio AndAlso
                                    a.FechaHora < fechaFin) _
                .AsNoTracking().ToListAsync()

            ' 5) Estados "activos en rango" → colores y prioridades para el período
            Dim estadosActivos = Await CargarEstadosActivosEnRangoAsync(_funcionarioId, fechaInicio, fechaFin)

            Dim estadosActivosDict = estadosActivos _
                .GroupBy(Function(ea) NormalizarTexto(ea.Tipo)) _
                .ToDictionary(Function(g) g.Key, Function(g) g.OrderBy(Function(x) x.Prioridad).First())

            Dim eventosUnificados As New List(Of EventoSituacionDTO)

            ' Estados
            eventosUnificados.AddRange(estadosEnPeriodo.Select(Function(s)
                                                                   Dim tipo = s.TipoEstadoNombre
                                                                   Dim activo = BuscarEstadoActivo(estadosActivosDict, tipo)
                                                                   Dim prioridad = If(activo?.Prioridad, CType(Nothing, Integer?))
                                                                   Dim colorIndicador = If(activo?.ColorIndicador, Nothing)
                                                                   Dim sev = If(prioridad.HasValue, MapearPrioridadASeveridad(prioridad.Value), ClasificarSeveridad(tipo))
                                                                   Return New EventoSituacionDTO With {
                                                                       .Id = s.Id,
                                                                       .TipoEvento = "Estado",
                                                                       .Tipo = tipo,
                                                                       .Desde = s.FechaDesde,
                                                                       .Hasta = s.FechaHasta,
                                                                       .Severidad = sev,
                                                                       .Prioridad = prioridad,
                                                                       .ColorIndicador = colorIndicador
                                                                   }
                                                               End Function))

            ' Licencias
            eventosUnificados.AddRange(licenciasEnPeriodo.Select(Function(l)
                                                                     Dim tipo = $"LICENCIA: {l.TipoLicencia.Nombre}"
                                                                     Dim activo = BuscarEstadoActivo(estadosActivosDict, tipo, l.TipoLicencia.Nombre)
                                                                     Dim prioridad = If(activo?.Prioridad, CType(Nothing, Integer?))
                                                                     Dim colorIndicador = If(activo?.ColorIndicador, Nothing)
                                                                     Dim sev = If(prioridad.HasValue, MapearPrioridadASeveridad(prioridad.Value), ClasificarSeveridad(tipo))
                                                                     Return New EventoSituacionDTO With {
                                                                         .Id = l.Id,
                                                                         .TipoEvento = "Licencia",
                                                                         .Tipo = tipo,
                                                                         .Desde = CType(l.inicio, Date?),
                                                                         .Hasta = CType(l.finaliza, Date?),
                                                                         .Severidad = sev,
                                                                         .Prioridad = prioridad,
                                                                         .ColorIndicador = colorIndicador
                                                                     }
                                                                 End Function))

            ' Notificaciones
            eventosUnificados.AddRange(notificacionesEnPeriodo.Select(Function(n)
                                                                          Return New EventoSituacionDTO With {
                                                                              .Id = n.Id,
                                                                              .TipoEvento = "Notificacion",
                                                                              .Tipo = $"NOTIFICACIÓN PENDIENTE: {n.TipoNotificacion.Nombre}",
                                                                              .Desde = CType(n.FechaProgramada, Date?),
                                                                              .Hasta = Nothing,
                                                                              .Severidad = ClasificarSeveridad("NOTIFICACION"),
                                                                              .Prioridad = Nothing,
                                                                              .ColorIndicador = Nothing
                                                                          }
                                                                      End Function))

            ' Auditoría
            eventosUnificados.AddRange(cambiosAuditados.Select(Function(a)
                                                                   Return New EventoSituacionDTO With {
                                                                       .Id = a.Id,
                                                                       .TipoEvento = "Auditoria",
                                                                       .Tipo = $"CAMBIO: El campo '{a.CampoNombre}' se modificó de '{If(String.IsNullOrWhiteSpace(a.ValorAnterior), "[vacío]", a.ValorAnterior)}' a '{If(String.IsNullOrWhiteSpace(a.ValorNuevo), "[vacío]", a.ValorNuevo)}'.",
                                                                       .Desde = CType(a.FechaHora, Date?),
                                                                       .Hasta = Nothing,
                                                                       .Severidad = ClasificarSeveridad("CAMBIO"),
                                                                       .Prioridad = Nothing,
                                                                       .ColorIndicador = Nothing
                                                                   }
                                                               End Function))

            Dim eventosOrdenados = eventosUnificados _
                .OrderBy(Function(ev) If(ev.Prioridad.HasValue, 0, 1)) _
                .ThenBy(Function(ev) ev.Prioridad.GetValueOrDefault(Integer.MaxValue)) _
                .ThenByDescending(Function(ev) ev.Severidad) _
                .ThenByDescending(Function(ev) ev.Desde.GetValueOrDefault(Date.MinValue)) _
                .ThenBy(Function(ev) ev.Tipo) _
                .ToList()

            dgvEstados.DataSource = eventosOrdenados

        Catch ex As Exception
            MessageBox.Show($"Error al poblar la grilla de estados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            dgvEstados.DataSource = Nothing
        Finally
            LoadingHelper.OcultarCargando(dgvEstados)
        End Try
    End Function

    Private Async Function CargarEstadosActivosEnRangoAsync(funcionarioId As Integer, desde As Date, hasta As Date) As Task(Of List(Of FuncionarioEstadoActivoDTO))
        ' Consulta directa a la iTVF con parámetros
        Dim sql As String = "SELECT FuncionarioId, Prioridad, Tipo, Detalles, ColorIndicador " &
                            "FROM dbo.fn_FuncionarioEstadosEnRango(@FuncionarioId, @Desde, @Hasta);"

        Dim p1 As New SqlParameter("@FuncionarioId", funcionarioId)
        Dim p2 As New SqlParameter("@Desde", desde)
        Dim p3 As New SqlParameter("@Hasta", hasta)

        ' Nota: EF6 usa Database.SqlQuery para tipos no-mapeados (DTO)
        Dim lista = Await _uow.Context.Database.SqlQuery(Of FuncionarioEstadoActivoDTO)(sql, p1, p2, p3).ToListAsync()
        Return lista
    End Function

#End Region

#Region " Grilla / Doble clic / Estilos "

    Private Async Sub dgvEstados_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse dgvEstados.CurrentRow Is Nothing Then Return
        Try
            Dim rowData = dgvEstados.CurrentRow.DataBoundItem
            Dim tipoEvento = NormalizarTexto(GetPropValue(Of String)(rowData, "TipoEvento", ""))
            Dim tipoTexto = NormalizarTexto(GetPropValue(Of String)(rowData, "Tipo", ""))
            Dim registroId = GetPropValue(Of Integer)(rowData, "Id", 0)
            If registroId = 0 Then
                MessageBox.Show("No se pudo identificar el ID del registro seleccionado.", "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Select Case tipoEvento
                Case "ESTADO"
                    If tipoTexto.Contains("DESIGNACION") Then
                        Await MostrarReporteDesignacionAsync(registroId)
                    Else
                        ' Opcional: mostrar detalle genérico
                        MessageBox.Show("No hay acción específica para este estado.", "Info",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Case "NOTIFICACION"
                    Dim frm As New frmNotificacionRPT(registroId)
                    NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
            End Select

        Catch ex As Exception
            MessageBox.Show("No se pudo procesar la acción: " & ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Async Function MostrarReporteDesignacionAsync(Optional estadoIdEspecifico As Integer = 0) As Task
        Try
            If estadoIdEspecifico > 0 Then
                Dim frm As New frmDesignacionRPT(estadoIdEspecifico)
                NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
                Return
            End If

            Dim estadosDesignacion = Await _uow.Context.Set(Of vw_EstadosTransitoriosCompletos)() _
                .Where(Function(e) e.FuncionarioId = _funcionarioId AndAlso
                                     e.FechaDesde.HasValue AndAlso
                                     e.TipoEstadoNombre.ToUpper().Contains("DESIGNACI")) _
                .OrderByDescending(Function(e) e.FechaDesde) _
                .AsNoTracking() _
                .ToListAsync()

            If estadosDesignacion Is Nothing OrElse Not estadosDesignacion.Any() Then
                MessageBox.Show("No se encontraron estados de 'Designación' para este funcionario.", "Aviso",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim lista As New List(Of DesignacionSeleccionDTO)
            For Each est In estadosDesignacion
                lista.Add(New DesignacionSeleccionDTO With {
                    .EstadoTransitorioId = est.Id,
                    .FechaDesde = est.FechaDesde,
                    .FechaHasta = est.FechaHasta,
                    .Descripcion = $"Desde {If(est.FechaDesde.HasValue, est.FechaDesde.Value.ToString("dd/MM/yyyy"), "-")} hasta {If(est.FechaHasta.HasValue, est.FechaHasta.Value.ToString("dd/MM/yyyy"), "VIGENTE")}"
                })
            Next

            Dim vigentes = lista.Where(Function(x) x.Vigente).ToList()
            Dim idParaReporte As Integer = 0

            If vigentes.Count = 1 Then
                idParaReporte = vigentes(0).EstadoTransitorioId
            Else
                Using selector As New frmElegirDesignacion(lista)
                    If selector.ShowDialog(Me) = DialogResult.OK AndAlso selector.Seleccion IsNot Nothing Then
                        idParaReporte = selector.Seleccion.EstadoTransitorioId
                    End If
                End Using
            End If

            If idParaReporte > 0 Then
                Dim frm As New frmDesignacionRPT(idParaReporte)
                NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
            End If

        Catch ex As Exception
            MessageBox.Show("No se pudo abrir el reporte de Designación: " & ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Function NormalizarTexto(s As String) As String
        If String.IsNullOrWhiteSpace(s) Then Return String.Empty
        Dim formD = s.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder()
        For Each ch In formD
            Dim cat = Globalization.CharUnicodeInfo.GetUnicodeCategory(ch)
            If cat <> Globalization.UnicodeCategory.NonSpacingMark Then sb.Append(ch)
        Next
        Return sb.ToString().Normalize(NormalizationForm.FormC).ToUpperInvariant()
    End Function

    Private Sub ConfigurarGrillaNovedades()
        AplicarEstiloModernoGrilla(dgvNovedades)
        dgvNovedades.AutoGenerateColumns = False
        dgvNovedades.Columns.Clear()

        Dim colFecha As New DataGridViewTextBoxColumn With {
            .Name = "Fecha", .DataPropertyName = "Fecha", .HeaderText = "Fecha",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 120
        }
        colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvNovedades.Columns.Add(colFecha)

        dgvNovedades.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Texto", .DataPropertyName = "Texto", .HeaderText = "Novedad",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
    End Sub

    Private Sub ConfigurarGrillaEstados()
        AplicarEstiloModernoGrilla(dgvEstados)
        dgvEstados.AutoGenerateColumns = False
        dgvEstados.Columns.Clear()

        dgvEstados.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Tipo", .DataPropertyName = "Tipo", .HeaderText = "Tipo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .MinimumWidth = 300
        })

        Dim colDesde As New DataGridViewTextBoxColumn With {
            .Name = "Desde", .DataPropertyName = "Desde", .HeaderText = "Desde",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 110
        }
        colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstados.Columns.Add(colDesde)

        Dim colHasta As New DataGridViewTextBoxColumn With {
            .Name = "Hasta", .DataPropertyName = "Hasta", .HeaderText = "Hasta",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 110
        }
        colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
        dgvEstados.Columns.Add(colHasta)
    End Sub

    Private Sub DgvNovedades_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        ConfigurarGrillaNovedades()
        If dgv.Columns.Contains("Id") Then dgv.Columns("Id").Visible = False
    End Sub

    Private Sub DgvEstados_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        Dim dgv = CType(sender, DataGridView)
        ConfigurarGrillaEstados()
        If dgv.Columns.Contains("Id") Then dgv.Columns("Id").Visible = False
        If dgv.Columns.Contains("TipoEvento") Then dgv.Columns("TipoEvento").Visible = False
        AplicarColoresEstados(dgv)
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

#End Region

#Region " Severidad / Colores "

    Private Enum Severidad
        Info = 0
        Baja = 1
        Media = 2
        Alta = 3
        Critica = 4
    End Enum

    Private Sub AplicarColoresEstados(dgv As DataGridView)
        For Each row As DataGridViewRow In dgv.Rows
            Dim evento = TryCast(row.DataBoundItem, EventoSituacionDTO)
            If evento Is Nothing Then Continue For

            Dim aplico As Boolean = False
            If Not String.IsNullOrWhiteSpace(evento.ColorIndicador) Then
                Dim c = Color.FromName(evento.ColorIndicador)
                If c.IsKnownColor OrElse c.IsNamedColor OrElse c.A > 0 Then
                    AplicarColorDeFila(row, c)
                    aplico = True
                End If
            End If
            If Not aplico Then
                PintarFilaPorSeveridad(row, evento.Severidad)
            End If
        Next
    End Sub

    Private Function ClasificarSeveridad(tipoTexto As String) As Severidad
        Dim t As String = NormalizarTexto(tipoTexto)
        If t.StartsWith("CAMBIO") Then Return Severidad.Info
        If t.StartsWith("NOTIFICACION") Then Return Severidad.Media
        If t.StartsWith("LICENCIA") Then Return Severidad.Info
        If t.Contains("INICIO DE PROCESAMIENTO") Then Return Severidad.Critica
        If t.Contains("SEPARACION") Then Return Severidad.Critica
        If t.Contains("BAJA") Then Return Severidad.Critica
        If t.Contains("SUMARIO") Then Return Severidad.Alta
        If t.Contains("SANCI") Then Return Severidad.Alta
        If t.Contains("ENFERMEDAD") Then Return Severidad.Media
        If t.Contains("ORDEN CINCO") OrElse t.Contains("ORDEN 5") Then Return Severidad.Alta
        If t.Contains("TRASLADO") Then Return Severidad.Media
        If t.Contains("RETEN") Then Return Severidad.Baja
        If t.Contains("DESIGNACION") Then Return Severidad.Baja
        If t.Contains("REACTIVACION") Then Return Severidad.Baja
        If t.Contains("CAMBIO DE CARGO") Then Return Severidad.Baja
        Return Severidad.Baja
    End Function

    Private Sub PintarFilaPorSeveridad(row As DataGridViewRow, sev As Severidad)
        Dim strong As Color
        Dim text As Color = Color.White
        Select Case sev
            Case Severidad.Critica : strong = Color.FromArgb(229, 57, 53)
            Case Severidad.Alta : strong = Color.FromArgb(245, 124, 0)
            Case Severidad.Media : strong = Color.FromArgb(255, 179, 0)
            Case Severidad.Baja : strong = Color.FromArgb(56, 142, 60)
            Case Else : strong = Color.FromArgb(30, 136, 229)
        End Select
        row.DefaultCellStyle.BackColor = strong
        row.DefaultCellStyle.ForeColor = text
        row.DefaultCellStyle.SelectionBackColor = strong
        row.DefaultCellStyle.SelectionForeColor = text
    End Sub

    Private Sub AplicarColorDeFila(row As DataGridViewRow, color As Color)
        row.DefaultCellStyle.BackColor = color
        row.DefaultCellStyle.ForeColor = Color.White
        row.DefaultCellStyle.SelectionBackColor = color
        row.DefaultCellStyle.SelectionForeColor = Color.White
    End Sub

    Private Function MapearPrioridadASeveridad(prioridad As Integer) As Severidad
        Select Case prioridad
            Case 1, 2 : Return Severidad.Critica
            Case 3, 4, 5 : Return Severidad.Alta
            Case 6, 7 : Return Severidad.Media
            Case Else : Return Severidad.Baja
        End Select
    End Function

    Private Function BuscarEstadoActivo(estadosActivosDict As Dictionary(Of String, FuncionarioEstadoActivoDTO),
                                        tipoPrincipal As String,
                                        Optional tipoAlternativo As String = Nothing) As FuncionarioEstadoActivoDTO
        If estadosActivosDict Is Nothing OrElse estadosActivosDict.Count = 0 Then Return Nothing
        Dim clavePrincipal = NormalizarTexto(tipoPrincipal)
        Dim resultado As FuncionarioEstadoActivoDTO = Nothing
        If estadosActivosDict.TryGetValue(clavePrincipal, resultado) Then Return resultado
        If Not String.IsNullOrWhiteSpace(tipoAlternativo) Then
            Dim claveAlternativa = NormalizarTexto(tipoAlternativo)
            If estadosActivosDict.TryGetValue(claveAlternativa, resultado) Then Return resultado
        End If
        If clavePrincipal.StartsWith("LICENCIA:") Then
            Dim claveSinPrefijo = clavePrincipal.Replace("LICENCIA:", String.Empty).Trim()
            If estadosActivosDict.TryGetValue(claveSinPrefijo, resultado) Then Return resultado
        End If
        Return Nothing
    End Function

#End Region

#Region " Helpers UI / DTOs / Utils "

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim fechaInicio = dtpDesde.Value.Date
        Dim fechaFin = dtpHasta.Value.Date.AddDays(1)
        Dim frm As New frmFuncionarioSituacionRPT(_funcionarioId, fechaInicio, fechaFin)
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub

    Private Sub AplicarEstiloModernoGrilla(dgv As DataGridView)
        dgv.BorderStyle = BorderStyle.None
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(230, 230, 230)
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.ReadOnly = True
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.AllowUserToResizeRows = False
        dgv.BackgroundColor = Color.White

        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersHeight = 40
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        dgv.DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
        dgv.DefaultCellStyle.SelectionForeColor = Color.White
        dgv.RowsDefaultCellStyle.BackColor = Color.White
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)
    End Sub

    Private Class EventoSituacionDTO
        Public Property Id As Integer
        Public Property TipoEvento As String
        Public Property Tipo As String
        Public Property Desde As Date?
        Public Property Hasta As Date?
        Public Property Severidad As Severidad
        Public Property Prioridad As Integer?
        Public Property ColorIndicador As String
    End Class

    Private Class FuncionarioEstadoActivoDTO
        Public Property FuncionarioId As Integer
        Public Property Prioridad As Integer
        Public Property Tipo As String
        Public Property Detalles As String
        Public Property ColorIndicador As String
    End Class

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

#End Region

End Class

Public Class DesignacionSeleccionDTO
    Public Property EstadoTransitorioId As Integer
    Public Property FechaDesde As Date?
    Public Property FechaHasta As Date?
    Public Property Descripcion As String

    Public ReadOnly Property Vigente As Boolean
        Get
            Dim hoy = Date.Today
            Dim esVigenteSinFechaFin = Not FechaHasta.HasValue AndAlso FechaDesde.HasValue AndAlso FechaDesde.Value.Date <= hoy
            Dim esVigenteConFechaFin = FechaDesde.HasValue AndAlso FechaHasta.HasValue AndAlso FechaDesde.Value.Date <= hoy AndAlso FechaHasta.Value.Date >= hoy
            Return esVigenteSinFechaFin OrElse esVigenteConFechaFin
        End Get
    End Property
End Class

'Imports System.Data.Entity
'Imports System.Drawing
'Imports System.Text

'Public Class frmFuncionarioSituacion
'    Inherits FormActualizable
'#Region " Campos y Constructor "

'    '--- Variables de clase ---
'    Private ReadOnly _funcionarioId As Integer
'    Private ReadOnly _uow As IUnitOfWork
'    Private _funcionario As Funcionario

'    '--- Almacenamiento de datos ---
'    Private _todasLasNovedades As List(Of vw_NovedadesCompletas) = New List(Of vw_NovedadesCompletas)()

'    '--- UI State ---
'    Private _selectedTimelineButton As Button = Nothing


'    Public Sub New(idFuncionario As Integer)
'        InitializeComponent()
'        _funcionarioId = idFuncionario
'        _uow = New UnitOfWork()
'    End Sub

'#End Region

'#Region " Carga y Ciclo de Vida del Formulario "

'    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        AppTheme.Aplicar(Me)
'        dgvNovedades.ActivarDobleBuffer(True) ' <-- LÍNEA AÑADIDA
'        dgvEstados.ActivarDobleBuffer(True)   ' <-- LÍNEA AÑADIDA
'        ' Suscripciones a eventos
'        AddHandler dgvNovedades.CellDoubleClick, AddressOf dgvNovedades_CellDoubleClick
'        AddHandler dgvNovedades.DataBindingComplete, AddressOf DgvNovedades_DataBindingComplete
'        AddHandler dgvEstados.DataBindingComplete, AddressOf DgvEstados_DataBindingComplete
'        AddHandler dgvEstados.CellDoubleClick, AddressOf dgvEstados_CellDoubleClick


'        ' Configuración inicial
'        dtpDesde.Value = Date.Today
'        dtpHasta.Value = Date.Today

'        If btnGenerar IsNot Nothing Then btnGenerar.Visible = True

'        ' Carga inicial de datos y actualización de la vista
'        Await CargarDatosEsenciales()
'        Await ActualizarTodo()
'    End Sub
'    Protected Overrides Async Function RefrescarSegunFuncionarioAsync(e As FuncionarioCambiadoEventArgs) As Task
'        ' Refrescá sólo si corresponde al funcionario visible,
'        ' o si es un refresco global (sin Id).
'        If e IsNot Nothing AndAlso e.FuncionarioId.HasValue AndAlso e.FuncionarioId.Value <> _funcionarioId Then
'            Return
'        End If

'        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Return

'        ' Importante: mantené tu secuencia actual
'        Await CargarDatosEsenciales()
'        Await ActualizarTodo()
'    End Function
'    Private Sub frmFuncionarioSituacion_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
'        _uow.Dispose()
'    End Sub

'    Private Async Function CargarDatosEsenciales() As Task
'        LoadingHelper.MostrarCargando(Me)
'        Try
'            _funcionario = Await _uow.Context.Set(Of Funcionario)().FindAsync(_funcionarioId)

'            If _funcionario Is Nothing Then
'                Notifier.[Error](Me, "No se encontró el funcionario.")
'                Close()
'                Return
'            End If

'            lblNombre.Text = $"Situación de: {_funcionario.Nombre} ({_funcionario.CI})"

'            _todasLasNovedades = Await _uow.Context.Set(Of vw_NovedadesCompletas)().
'            Where(Function(n) n.FuncionarioId = _funcionario.Id).
'            AsNoTracking().
'            ToListAsync()

'        Catch ex As Exception
'            Notifier.[Error](Me, $"Error al cargar los datos iniciales: {ex.Message}")
'        Finally
'            LoadingHelper.OcultarCargando(Me)
'        End Try
'    End Function


'#End Region

'#Region " Lógica de Actualización y Eventos "

'    Private Async Function ActualizarTodo() As Task
'        Dim fechaInicio = dtpDesde.Value.Date
'        Dim fechaFin = dtpHasta.Value.Date.AddDays(1)

'        GenerarTimeline(fechaInicio, fechaFin)
'        Await PoblarGrillaEstados(fechaInicio, fechaFin)
'        SeleccionarPrimerDiaSiExiste()
'    End Function

'    Private Async Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
'        Await ActualizarTodo()
'    End Sub

'#End Region

'#Region " Lógica del Timeline de Novedades "
'    ' --- SIN CAMBIOS EN ESTA REGIÓN ---
'    Private Sub GenerarTimeline(fechaInicio As Date, fechaFin As Date)
'        flpTimeline.SuspendLayout()
'        flpTimeline.Controls.Clear()
'        _selectedTimelineButton = Nothing
'        dgvNovedades.DataSource = Nothing

'        Dim fechasNovedades = _todasLasNovedades _
'            .Where(Function(n) n.Fecha.Date >= fechaInicio AndAlso n.Fecha.Date < fechaFin) _
'            .Select(Function(n) n.Fecha.Date).Distinct().OrderByDescending(Function(d) d).ToList()

'        If Not fechasNovedades.Any() Then
'            Dim lblEmpty As New Label With {
'                .Text = "No hay novedades en el período seleccionado.",
'                .AutoSize = True,
'                .Margin = New Padding(10)
'            }
'            flpTimeline.Controls.Add(lblEmpty)
'        Else
'            For Each fecha As Date In fechasNovedades
'                Dim btnDia As New Button With {
'                    .Text = fecha.ToString("dd MMM yyyy"),
'                    .Tag = fecha,
'                    .Width = flpTimeline.ClientSize.Width - 20,
'                    .Height = 30,
'                    .FlatStyle = FlatStyle.Flat,
'                    .TextAlign = ContentAlignment.MiddleLeft,
'                    .Margin = New Padding(0, 0, 0, 6)
'                }
'                btnDia.FlatAppearance.BorderSize = 1
'                AddHandler btnDia.Click, AddressOf TimelineDia_Click
'                flpTimeline.Controls.Add(btnDia)
'            Next
'        End If

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

'        If _selectedTimelineButton IsNot Nothing Then
'            _selectedTimelineButton.BackColor = SystemColors.Control
'            _selectedTimelineButton.ForeColor = SystemColors.ControlText
'        End If
'        clickedButton.BackColor = SystemColors.Highlight
'        clickedButton.ForeColor = SystemColors.HighlightText
'        _selectedTimelineButton = clickedButton

'        ActualizarVistaDeNovedades(fechaSeleccionada)
'    End Sub

'    Private Sub ActualizarVistaDeNovedades(selectedDate As Date)
'        Dim novedadesDelDia = _todasLasNovedades.
'            Where(Function(n) n.Fecha.Date = selectedDate.Date).
'            OrderByDescending(Function(n) n.Id).
'            Select(Function(n) New With {.Id = n.Id, n.Fecha, .Texto = n.Texto}).ToList()

'        dgvNovedades.DataSource = novedadesDelDia
'    End Sub

'#End Region

'#Region " Lógica de la Grilla de Estados "

'    ' --- FUNCIÓN CLAVE ACTUALIZADA Y SIMPLIFICADA ---
'    Private Async Function PoblarGrillaEstados(fechaInicio As Date, fechaFin As Date) As Task
'        LoadingHelper.MostrarCargando(dgvEstados)
'        Try
'            ' 1) Estados transitorios
'            Dim estadosEnPeriodo = Await _uow.Context.Set(Of vw_EstadosTransitoriosCompletos)() _
'                .Where(Function(e) e.FuncionarioId = _funcionarioId AndAlso
'                                    e.FechaDesde.HasValue AndAlso
'                                    e.FechaDesde.Value < fechaFin AndAlso
'                                    (Not e.FechaHasta.HasValue OrElse e.FechaHasta.Value >= fechaInicio)) _
'                .AsNoTracking().ToListAsync()

'            ' 2) Licencias
'            Dim licenciasEnPeriodo = Await _uow.Context.Set(Of HistoricoLicencia)() _
'                .Include(Function(l) l.TipoLicencia) _
'                .Where(Function(l) l.FuncionarioId = _funcionarioId AndAlso
'                                        l.inicio < fechaFin AndAlso
'                                        l.finaliza >= fechaInicio) _
'                .AsNoTracking().ToListAsync()

'            ' 3) Notificaciones PENDIENTES
'            Dim estadoPendienteId As Byte = CByte(ModConstantesApex.EstadoNotificacionPersonal.Pendiente)
'            Dim notificacionesEnPeriodo = Await _uow.Context.Set(Of NotificacionPersonal)() _
'                .Include(Function(n) n.TipoNotificacion) _
'                .Where(Function(n) n.FuncionarioId = _funcionarioId AndAlso
'                                    n.EstadoId = estadoPendienteId AndAlso
'                                    n.FechaProgramada >= fechaInicio AndAlso
'                                    n.FechaProgramada < fechaFin) _
'                .AsNoTracking().ToListAsync()

'            ' 4) Cambios de Auditoría
'            Dim funcionarioIdStr = _funcionarioId.ToString()
'            Dim cambiosAuditados = Await _uow.Context.Set(Of AuditoriaCambios)() _
'    .Where(Function(a) a.TablaNombre = "Funcionario" AndAlso
'                            a.RegistroId = funcionarioIdStr AndAlso
'                            a.FechaHora >= fechaInicio AndAlso
'                            a.FechaHora < fechaFin) _
'    .AsNoTracking().ToListAsync()

'            Dim estadosActivos = Await _uow.Context.Set(Of vw_FuncionarioEstadosActivos)() _
'                .Where(Function(ea) ea.FuncionarioId = _funcionarioId) _
'                .AsNoTracking().ToListAsync()

'            Dim estadosActivosDict = estadosActivos _
'                .GroupBy(Function(ea) NormalizarTexto(ea.Tipo)) _
'                .ToDictionary(Function(g) g.Key, Function(g) g.OrderBy(Function(x) x.Prioridad).First())

'            Dim eventosUnificados As New List(Of EventoSituacionDTO)

'            ' Unificamos todos los eventos en un solo tipo de objeto para la grilla
'            eventosUnificados.AddRange(estadosEnPeriodo.Select(Function(s)
'                                                                  Dim tipo = s.TipoEstadoNombre
'                                                                  Dim activo = BuscarEstadoActivo(estadosActivosDict, tipo)
'                                                                  Dim prioridad = If(activo?.Prioridad, CType(Nothing, Integer?))
'                                                                  Dim colorIndicador = If(activo?.ColorIndicador, Nothing)
'                                                                  Dim severidadCalculada = If(prioridad.HasValue, _
'                                                                                              MapearPrioridadASeveridad(prioridad.Value), _
'                                                                                              ClasificarSeveridad(tipo))

'                                                                  Return New EventoSituacionDTO With {
'                                                                      .Id = s.Id,
'                                                                      .TipoEvento = "Estado",
'                                                                      .Tipo = tipo,
'                                                                      .Desde = s.FechaDesde,
'                                                                      .Hasta = s.FechaHasta,
'                                                                      .Severidad = severidadCalculada,
'                                                                      .Prioridad = prioridad,
'                                                                      .ColorIndicador = colorIndicador
'                                                                  }
'                                                              End Function))

'            eventosUnificados.AddRange(licenciasEnPeriodo.Select(Function(l)
'                                                                     Dim tipo = $"LICENCIA: {l.TipoLicencia.Nombre}"
'                                                                     Dim activo = BuscarEstadoActivo(estadosActivosDict, tipo, l.TipoLicencia.Nombre)
'                                                                     Dim prioridad = If(activo?.Prioridad, CType(Nothing, Integer?))
'                                                                     Dim colorIndicador = If(activo?.ColorIndicador, Nothing)
'                                                                     Dim severidadCalculada = If(prioridad.HasValue, _
'                                                                                                 MapearPrioridadASeveridad(prioridad.Value), _
'                                                                                                 ClasificarSeveridad(tipo))

'                                                                     Return New EventoSituacionDTO With {
'                                                                         .Id = l.Id,
'                                                                         .TipoEvento = "Licencia",
'                                                                         .Tipo = tipo,
'                                                                         .Desde = CType(l.inicio, Date?),
'                                                                         .Hasta = CType(l.finaliza, Date?),
'                                                                         .Severidad = severidadCalculada,
'                                                                         .Prioridad = prioridad,
'                                                                         .ColorIndicador = colorIndicador
'                                                                     }
'                                                                 End Function))

'            eventosUnificados.AddRange(notificacionesEnPeriodo.Select(Function(n)
'                                                                          Dim tipo = $"NOTIFICACIÓN PENDIENTE: {n.TipoNotificacion.Nombre}"
'                                                                          Return New EventoSituacionDTO With {
'                                                                              .Id = n.Id,
'                                                                              .TipoEvento = "Notificacion",
'                                                                              .Tipo = tipo,
'                                                                              .Desde = CType(n.FechaProgramada, Date?),
'                                                                              .Hasta = Nothing,
'                                                                              .Severidad = ClasificarSeveridad(tipo),
'                                                                              .Prioridad = Nothing,
'                                                                              .ColorIndicador = Nothing
'                                                                          }
'                                                                      End Function))

'            eventosUnificados.AddRange(cambiosAuditados.Select(Function(a)
'                                                                   Dim tipo = $"CAMBIO: El campo '{a.CampoNombre}' se modificó de '{If(String.IsNullOrWhiteSpace(a.ValorAnterior), "[vacío]", a.ValorAnterior)}' a '{If(String.IsNullOrWhiteSpace(a.ValorNuevo), "[vacío]", a.ValorNuevo)}'."
'                                                                   Return New EventoSituacionDTO With {
'                                                                       .Id = a.Id,
'                                                                       .TipoEvento = "Auditoria",
'                                                                       .Tipo = tipo,
'                                                                       .Desde = CType(a.FechaHora, Date?),
'                                                                       .Hasta = Nothing,
'                                                                       .Severidad = ClasificarSeveridad(tipo),
'                                                                       .Prioridad = Nothing,
'                                                                       .ColorIndicador = Nothing
'                                                                   }
'                                                               End Function))

'            Dim eventosOrdenados = eventosUnificados _
'                .OrderBy(Function(ev) If(ev.Prioridad.HasValue, 0, 1)) _
'                .ThenBy(Function(ev) ev.Prioridad.GetValueOrDefault(Integer.MaxValue)) _
'                .ThenByDescending(Function(ev) ev.Severidad) _
'                .ThenByDescending(Function(ev) ev.Desde.GetValueOrDefault(Date.MinValue)) _
'                .ThenBy(Function(ev) ev.Tipo) _
'                .ToList()

'            dgvEstados.DataSource = eventosOrdenados

'        Catch ex As Exception
'            MessageBox.Show($"Error al poblar la grilla de estados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            dgvEstados.DataSource = Nothing
'        Finally
'            LoadingHelper.OcultarCargando(dgvEstados)
'        End Try
'    End Function
'#End Region

'#Region "Lógica de grilla y estados"

'    ' --- FUNCIÓN CLAVE ACTUALIZADA ---
'    Private Async Sub dgvEstados_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
'        If e.RowIndex < 0 OrElse dgvEstados.CurrentRow Is Nothing Then Return
'        Try
'            Dim rowData = dgvEstados.CurrentRow.DataBoundItem

'            ' Obtenemos las propiedades clave de la fila seleccionada
'            Dim tipoEvento = NormalizarTexto(GetPropValue(Of String)(rowData, "TipoEvento", ""))
'            Dim tipoTexto = NormalizarTexto(GetPropValue(Of String)(rowData, "Tipo", ""))
'            Dim registroId = GetPropValue(Of Integer)(rowData, "Id", 0)

'            If registroId = 0 Then
'                MessageBox.Show("No se pudo identificar el ID del registro seleccionado.", "Aviso",
'                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
'                Return
'            End If

'            ' --- Lógica Definitiva ---
'            Select Case tipoEvento
'                Case "ESTADO"
'                    ' Si es un Estado y contiene "DESIGNACION", abre el reporte.
'                    If tipoTexto.Contains("DESIGNACION") Then
'                        Await MostrarReporteDesignacionAsync(registroId)
'                    End If

'                Case "NOTIFICACION"
'                    ' Si es una Notificación, abre el formulario de REPORTE (impresión).
'                    ' Sabemos que es pendiente porque solo esas se cargan en la grilla.
'                    Dim frm As New frmNotificacionRPT(registroId)
'                    NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)

'            End Select

'        Catch ex As Exception
'            MessageBox.Show("No se pudo procesar la acción: " & ex.Message, "Error",
'                            MessageBoxButtons.OK, MessageBoxIcon.Warning)
'        End Try
'    End Sub

'    ' --- SIN CAMBIOS EN ESTA FUNCIÓN ---
'    Private Async Function MostrarReporteDesignacionAsync(Optional estadoIdEspecifico As Integer = 0) As Task
'        Try
'            If estadoIdEspecifico > 0 Then
'                Dim frm As New frmDesignacionRPT(estadoIdEspecifico)
'                NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
'                Return
'            End If

'            Dim estadosDesignacion = Await _uow.Context.Set(Of vw_EstadosTransitoriosCompletos)() _
'            .Where(Function(e) e.FuncionarioId = _funcionarioId AndAlso
'                                 e.FechaDesde.HasValue AndAlso
'                                 e.TipoEstadoNombre.ToUpper().Contains("DESIGNACI")) _
'            .OrderByDescending(Function(e) e.FechaDesde) _
'            .AsNoTracking() _
'            .ToListAsync()

'            If estadosDesignacion Is Nothing OrElse Not estadosDesignacion.Any() Then
'                MessageBox.Show("No se encontraron estados de 'Designación' para este funcionario.", "Aviso",
'                                 MessageBoxButtons.OK, MessageBoxIcon.Information)
'                Return
'            End If

'            Dim lista As New List(Of DesignacionSeleccionDTO)
'            For Each est In estadosDesignacion
'                lista.Add(New DesignacionSeleccionDTO With {
'                .EstadoTransitorioId = est.Id,
'                .FechaDesde = est.FechaDesde,
'                .FechaHasta = est.FechaHasta,
'                .Descripcion = $"Desde {If(est.FechaDesde.HasValue, est.FechaDesde.Value.ToString("dd/MM/yyyy"), "-")} hasta {If(est.FechaHasta.HasValue, est.FechaHasta.Value.ToString("dd/MM/yyyy"), "VIGENTE")}"
'            })
'            Next

'            Dim vigentes = lista.Where(Function(x) x.Vigente).ToList()
'            Dim idParaReporte As Integer = 0

'            If vigentes.Count = 1 Then
'                idParaReporte = vigentes(0).EstadoTransitorioId
'            Else
'                Using selector As New frmElegirDesignacion(lista)
'                    If selector.ShowDialog(Me) = DialogResult.OK AndAlso selector.Seleccion IsNot Nothing Then
'                        idParaReporte = selector.Seleccion.EstadoTransitorioId
'                    End If
'                End Using
'            End If

'            If idParaReporte > 0 Then
'                Dim frm As New frmDesignacionRPT(idParaReporte)
'                NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
'            End If

'        Catch ex As Exception
'            MessageBox.Show("No se pudo abrir el reporte de Designación: " & ex.Message, "Error",
'                            MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Function

'    ' --- SIN CAMBIOS EN ESTA FUNCIÓN ---
'    Private Function NormalizarTexto(s As String) As String
'        If String.IsNullOrWhiteSpace(s) Then Return String.Empty
'        Dim formD = s.Normalize(NormalizationForm.FormD)
'        Dim sb As New StringBuilder()
'        For Each ch In formD
'            Dim cat = Globalization.CharUnicodeInfo.GetUnicodeCategory(ch)
'            If cat <> Globalization.UnicodeCategory.NonSpacingMark Then sb.Append(ch)
'        Next
'        Return sb.ToString().Normalize(NormalizationForm.FormC).ToUpperInvariant()
'    End Function

'#End Region

'#Region " Configuración y Eventos de Grillas "
'    ' --- SIN CAMBIOS EN ESTA REGIÓN ---
'    Private Sub ConfigurarGrillaNovedades()
'        ' Aplicamos el estilo base moderno
'        AplicarEstiloModernoGrilla(dgvNovedades)

'        ' Si ya están creadas, no las volvemos a crear
'        If dgvNovedades.Columns.Contains("Fecha") Then Return

'        dgvNovedades.AutoGenerateColumns = False
'        dgvNovedades.Columns.Clear()

'        Dim colFecha As New DataGridViewTextBoxColumn With {
'        .Name = "Fecha", .DataPropertyName = "Fecha", .HeaderText = "Fecha",
'        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
'        .MinimumWidth = 120
'    }
'        colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
'        dgvNovedades.Columns.Add(colFecha)

'        dgvNovedades.Columns.Add(New DataGridViewTextBoxColumn With {
'        .Name = "Texto", .DataPropertyName = "Texto", .HeaderText = "Novedad",
'        .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
'    })
'    End Sub

'    Private Sub ConfigurarGrillaEstados()
'        ' Aplicamos el estilo base moderno
'        AplicarEstiloModernoGrilla(dgvEstados)

'        ' Si ya están creadas, no las volvemos a crear
'        If dgvEstados.Columns.Contains("Tipo") Then Return

'        dgvEstados.AutoGenerateColumns = False
'        dgvEstados.Columns.Clear()

'        dgvEstados.Columns.Add(New DataGridViewTextBoxColumn With {
'        .Name = "Tipo", .DataPropertyName = "Tipo", .HeaderText = "Tipo",
'        .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
'        .MinimumWidth = 300
'    })

'        Dim colDesde As New DataGridViewTextBoxColumn With {
'        .Name = "Desde", .DataPropertyName = "Desde", .HeaderText = "Desde",
'        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
'        .MinimumWidth = 110
'    }
'        colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
'        dgvEstados.Columns.Add(colDesde)

'        Dim colHasta As New DataGridViewTextBoxColumn With {
'        .Name = "Hasta", .DataPropertyName = "Hasta", .HeaderText = "Hasta",
'        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
'        .MinimumWidth = 110
'    }
'        colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
'        dgvEstados.Columns.Add(colHasta)
'    End Sub

'    Private Sub dgvNovedades_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
'        If e.RowIndex < 0 OrElse dgvNovedades.CurrentRow Is Nothing Then Return
'        Try
'            Dim rowData = dgvNovedades.CurrentRow.DataBoundItem
'            Dim novedadId = CInt(rowData.GetType().GetProperty("Id").GetValue(rowData, Nothing))

'            If novedadId > 0 Then
'                ' Abrimos en modo solo lectura (True) y usamos el helper del dashboard
'                Dim frm As New frmNovedadCrear(novedadId, True)
'                AbrirChildEnDashboard(frm)
'            End If
'        Catch ex As Exception
'            MessageBox.Show("No se pudo abrir el detalle de la novedad.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
'        End Try
'    End Sub
'    Private Function GetDashboard() As frmDashboard
'        Return Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
'    End Function
'    Private Sub AbrirChildEnDashboard(formHijo As Form)
'        If formHijo Is Nothing Then
'            Notifier.Warn(Me, "No hay formulario para abrir.")
'            Return
'        End If

'        Dim dash = GetDashboard()
'        If dash Is Nothing OrElse dash.IsDisposed Then
'            ' Fallback si el dashboard no está disponible
'            Notifier.Warn(Me, "No se encontró el Dashboard activo. Abriendo como diálogo.")
'            formHijo.ShowDialog(Me)
'            Return
'        End If

'        If dash.InvokeRequired Then
'            dash.BeginInvoke(CType(Sub() AbrirChildEnDashboard(formHijo), MethodInvoker))
'            Return
'        End If

'        Try
'            dash.Activate()
'            dash.BringToFront()
'            dash.AbrirChild(formHijo)
'        Catch ex As Exception
'            Notifier.[Error](dash, $"No se pudo abrir la ventana: {ex.Message}")
'        End Try
'    End Sub

'    Private Sub DgvNovedades_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
'        Dim dgv = CType(sender, DataGridView)
'        ConfigurarGrillaNovedades()
'        If dgv.Columns.Contains("Id") Then dgv.Columns("Id").Visible = False
'    End Sub

'    Private Sub DgvEstados_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
'        Dim dgv = CType(sender, DataGridView)
'        ConfigurarGrillaEstados()
'        ' Ocultamos las columnas que no son para el usuario
'        If dgv.Columns.Contains("Id") Then dgv.Columns("Id").Visible = False
'        If dgv.Columns.Contains("TipoEvento") Then dgv.Columns("TipoEvento").Visible = False
'        AplicarColoresEstados(dgv)
'    End Sub

'    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
'        If e.KeyCode = Keys.Escape Then Me.Close()
'    End Sub

'#End Region

'#Region " Severidad y Colores "
'    ' --- FUNCIÓN CLAVE ACTUALIZADA ---
'    Private Enum Severidad
'        Info = 0
'        Baja = 1
'        Media = 2
'        Alta = 3
'        Critica = 4
'    End Enum

'    Private Sub AplicarColoresEstados(dgv As DataGridView)
'        For Each row As DataGridViewRow In dgv.Rows
'            Dim evento = TryCast(row.DataBoundItem, EventoSituacionDTO)
'            If evento Is Nothing Then Continue For

'            Dim colorAplicado As Boolean = False

'            If Not String.IsNullOrWhiteSpace(evento.ColorIndicador) Then
'                Dim colorr = Color.FromName(evento.ColorIndicador)
'                If colorr.IsKnownColor OrElse colorr.IsNamedColor OrElse colorr.A > 0 Then
'                    AplicarColorDeFila(row, colorr)
'                    colorAplicado = True
'                End If
'            End If

'            If Not colorAplicado Then
'                PintarFilaPorSeveridad(row, evento.Severidad)
'            End If
'        Next
'    End Sub

'    Private Function ClasificarSeveridad(tipoTexto As String) As Severidad
'        Dim t As String = NormalizarTexto(tipoTexto)
'        If t.StartsWith("CAMBIO") Then Return Severidad.Info
'        If t.StartsWith("NOTIFICACION") Then Return Severidad.Media ' Simplificado
'        If t.StartsWith("LICENCIA") Then Return Severidad.Info
'        If t.Contains("INICIO DE PROCESAMIENTO") Then Return Severidad.Critica
'        If t.Contains("SEPARACION") Then Return Severidad.Critica
'        If t.Contains("BAJA") Then Return Severidad.Critica
'        If t.Contains("SUMARIO") Then Return Severidad.Alta
'        If t.Contains("SANCI") Then Return Severidad.Alta
'        If t.Contains("ENFERMEDAD") Then Return Severidad.Media
'        If t.Contains("ORDEN CINCO") OrElse t.Contains("ORDEN 5") Then Return Severidad.Alta
'        If t.Contains("TRASLADO") Then Return Severidad.Media
'        If t.Contains("RETEN") Then Return Severidad.Baja
'        If t.Contains("DESIGNACION") Then Return Severidad.Baja
'        If t.Contains("REACTIVACION") Then Return Severidad.Baja
'        If t.Contains("CAMBIO DE CARGO") Then Return Severidad.Baja

'        Return Severidad.Baja
'    End Function

'    Private Sub PintarFilaPorSeveridad(row As DataGridViewRow, sev As Severidad)
'        Dim strong As Color
'        Dim text As Color = Color.White

'        Select Case sev
'            Case Severidad.Critica
'                strong = Color.FromArgb(229, 57, 53)     ' rojo 600
'            Case Severidad.Alta
'                strong = Color.FromArgb(245, 124, 0)     ' naranja 700
'            Case Severidad.Media
'                strong = Color.FromArgb(255, 179, 0)     ' ámbar 600
'            Case Severidad.Baja
'                strong = Color.FromArgb(56, 142, 60)     ' verde 600
'            Case Else ' Info
'                strong = Color.FromArgb(30, 136, 229)    ' azul 600
'        End Select

'        row.DefaultCellStyle.BackColor = strong
'        row.DefaultCellStyle.ForeColor = text
'        row.DefaultCellStyle.SelectionBackColor = strong
'        row.DefaultCellStyle.SelectionForeColor = text
'    End Sub

'    Private Sub AplicarColorDeFila(row As DataGridViewRow, color As Color)
'        row.DefaultCellStyle.BackColor = color
'        row.DefaultCellStyle.ForeColor = Color.White
'        row.DefaultCellStyle.SelectionBackColor = color
'        row.DefaultCellStyle.SelectionForeColor = Color.White
'    End Sub

'    Private Function MapearPrioridadASeveridad(prioridad As Integer) As Severidad
'        Select Case prioridad
'            Case 1, 2
'                Return Severidad.Critica
'            Case 3, 4, 5
'                Return Severidad.Alta
'            Case 6, 7
'                Return Severidad.Media
'            Case Else
'                Return Severidad.Baja
'        End Select
'    End Function

'    Private Function BuscarEstadoActivo(estadosActivosDict As Dictionary(Of String, vw_FuncionarioEstadosActivos), tipoPrincipal As String, Optional tipoAlternativo As String = Nothing) As vw_FuncionarioEstadosActivos
'        If estadosActivosDict Is Nothing OrElse estadosActivosDict.Count = 0 Then Return Nothing

'        Dim clavePrincipal = NormalizarTexto(tipoPrincipal)
'        Dim resultado As vw_FuncionarioEstadosActivos = Nothing

'        If estadosActivosDict.TryGetValue(clavePrincipal, resultado) Then
'            Return resultado
'        End If

'        If Not String.IsNullOrWhiteSpace(tipoAlternativo) Then
'            Dim claveAlternativa = NormalizarTexto(tipoAlternativo)
'            If estadosActivosDict.TryGetValue(claveAlternativa, resultado) Then
'                Return resultado
'            End If
'        End If

'        If clavePrincipal.StartsWith("LICENCIA:") Then
'            Dim claveSinPrefijo = clavePrincipal.Replace("LICENCIA:", String.Empty).Trim()
'            If estadosActivosDict.TryGetValue(claveSinPrefijo, resultado) Then
'                Return resultado
'            End If
'        End If

'        Return Nothing
'    End Function

'    Private Function GetPropValue(Of T)(obj As Object, propName As String, defaultValue As T) As T
'        If obj Is Nothing Then Return defaultValue
'        Dim p = obj.GetType().GetProperty(propName, Reflection.BindingFlags.Instance Or Reflection.BindingFlags.Public Or Reflection.BindingFlags.IgnoreCase)
'        If p Is Nothing Then Return defaultValue
'        Dim val = p.GetValue(obj, Nothing)
'        If val Is Nothing Then Return defaultValue
'        Try
'            Return CType(val, T)
'        Catch
'            Try
'                Return DirectCast(Convert.ChangeType(val, GetType(T), Globalization.CultureInfo.InvariantCulture), T)
'            Catch
'                Return defaultValue
'            End Try
'        End Try
'    End Function


'    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
'        Dim fechaInicio = dtpDesde.Value.Date
'        Dim fechaFin = dtpHasta.Value.Date.AddDays(1)

'        ' Creamos y abrimos el formulario del reporte con las fechas correctas
'        Dim frm As New frmFuncionarioSituacionRPT(_funcionarioId, fechaInicio, fechaFin)
'        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
'    End Sub

'    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
'        Close()
'    End Sub

'#End Region
'    ' Agrega este nuevo método en cualquier lugar dentro de la clase del formulario
'    Private Sub AplicarEstiloModernoGrilla(dgv As DataGridView)
'        ' --- CONFIGURACIÓN GENERAL ---
'        dgv.BorderStyle = BorderStyle.None
'        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
'        dgv.GridColor = Color.FromArgb(230, 230, 230)
'        dgv.RowHeadersVisible = False
'        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
'        dgv.MultiSelect = False
'        dgv.ReadOnly = True
'        dgv.AllowUserToAddRows = False
'        dgv.AllowUserToDeleteRows = False
'        dgv.AllowUserToResizeRows = False
'        dgv.BackgroundColor = Color.White

'        ' --- ESTILO DE ENCABEZADOS (Headers) ---
'        dgv.EnableHeadersVisualStyles = False
'        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
'        dgv.ColumnHeadersHeight = 40
'        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
'        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
'        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
'        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
'        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

'        ' --- ESTILO DE FILAS (Rows) ---
'        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
'        dgv.DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
'        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
'        dgv.DefaultCellStyle.SelectionForeColor = Color.White
'        dgv.RowsDefaultCellStyle.BackColor = Color.White
'        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)
'    End Sub
'    Private Class EventoSituacionDTO
'        Public Property Id As Integer
'        Public Property TipoEvento As String
'        Public Property Tipo As String
'        Public Property Desde As Date?
'        Public Property Hasta As Date?
'        Public Property Severidad As Severidad
'        Public Property Prioridad As Integer?
'        Public Property ColorIndicador As String
'    End Class

'End Class

'Public Class DesignacionSeleccionDTO
'    Public Property EstadoTransitorioId As Integer
'    Public Property FechaDesde As Date?
'    Public Property FechaHasta As Date?
'    Public Property Descripcion As String

'    Public ReadOnly Property Vigente As Boolean
'        Get
'            Dim hoy = Date.Today
'            Dim esVigenteSinFechaFin = Not FechaHasta.HasValue AndAlso FechaDesde.HasValue AndAlso FechaDesde.Value.Date <= hoy
'            Dim esVigenteConFechaFin = FechaDesde.HasValue AndAlso FechaHasta.HasValue AndAlso FechaDesde.Value.Date <= hoy AndAlso FechaHasta.Value.Date >= hoy
'            Return esVigenteSinFechaFin OrElse esVigenteConFechaFin
'        End Get
'    End Property
'End Class