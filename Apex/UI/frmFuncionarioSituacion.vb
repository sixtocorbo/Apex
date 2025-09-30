Imports System.Data.Entity
Imports System.Drawing
Imports System.Text
Imports System.Linq ' <-- IMPORTANTE: Asegúrate de que esta línea esté al principio

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

        AddHandler dgvNovedades.DataBindingComplete, AddressOf DgvNovedades_DataBindingComplete
        AddHandler dgvEstados.DataBindingComplete, AddressOf DgvEstados_DataBindingComplete
        AddHandler dgvEstados.CellContentClick, AddressOf dgvEstados_CellContentClick


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
        Dim fechaFin = dtpHasta.Value.Date

        ' Evitar desbordes al trabajar con fechas máximas permitidas por el control.
        If fechaFin < DateTime.MaxValue.Date Then
            fechaFin = fechaFin.AddDays(1) ' Rango semi-abierto [inicio, fin)
        End If

        GenerarTimeline(fechaInicio, fechaFin)
        Await PoblarGrillaEstados(fechaInicio, fechaFin)
        SeleccionarPrimerDiaSiExiste()
    End Function

    Private Async Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Await ActualizarTodo()
    End Sub

#End Region

#Region " Timeline de Novedades (Sin Cambios) "

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

#Region " Grilla de Estados (Lógica Central Actualizada) "

    Private Async Function PoblarGrillaEstados(fechaInicio As Date, fechaFin As Date) As Task
        LoadingHelper.MostrarCargando(dgvEstados)
        Try
            ' 1) Cargar todos los Estados Transitorios y sus detalles de una sola vez
            Dim estadosEnPeriodo = Await _uow.Context.Set(Of EstadoTransitorio)() _
                .Include("TipoEstadoTransitorio") _
                .Include("BajaDeFuncionarioDetalle") _
                .Include("CambioDeCargoDetalle.Cargo") _
                .Include("CambioDeCargoDetalle.Cargo1") _
                .Include("DesarmadoDetalle") _
                .Include("DesignacionDetalle") _
                .Include("EnfermedadDetalle") _
                .Include("InicioDeProcesamientoDetalle") _
                .Include("OrdenCincoDetalle") _
                .Include("ReactivacionDeFuncionarioDetalle") _
                .Include("RetenDetalle") _
                .Include("SancionDetalle") _
                .Include("SeparacionDelCargoDetalle") _
                .Include("SumarioDetalle") _
                .Include("TrasladoDetalle") _
                .Where(Function(et) et.FuncionarioId = _funcionarioId) _
                .AsNoTracking() _
                .ToListAsync()

            ' 2) Licencias que se superponen con el rango
            Dim licenciasEnPeriodo = Await _uow.Context.Set(Of HistoricoLicencia)() _
                .Include(Function(l) l.TipoLicencia) _
                .Where(Function(l) l.FuncionarioId = _funcionarioId AndAlso
                                     l.inicio < fechaFin AndAlso
                                     l.finaliza >= fechaInicio) _
                .AsNoTracking().ToListAsync()

            ' 3) Notificaciones PENDIENTES dentro del rango
            Dim estadoPendienteId As Byte = CByte(ModConstantesApex.EstadoNotificacionPersonal.Pendiente)
            Dim estadoVencidaId As Byte = CByte(ModConstantesApex.EstadoNotificacionPersonal.Vencida)
            Dim notificacionesEnPeriodo = Await _uow.Context.Set(Of NotificacionPersonal)() _
                .Include(Function(n) n.TipoNotificacion) _
                .Include(Function(n) n.NotificacionEstado) _
                .Where(Function(n) n.FuncionarioId = _funcionarioId AndAlso
                                     (n.EstadoId = estadoPendienteId OrElse n.EstadoId = estadoVencidaId) AndAlso
                                     (n.FechaProgramada >= fechaInicio OrElse n.EstadoId = estadoVencidaId) AndAlso
                                     n.FechaProgramada < fechaFin) _
                .AsNoTracking().ToListAsync()

            ' 4) Cambios de Auditoría dentro del rango
            Dim funcionarioIdStr = _funcionarioId.ToString()
            Dim cambiosAuditados = Await _uow.Context.Set(Of AuditoriaCambios)() _
                .Where(Function(a) a.TablaNombre = "Funcionario" AndAlso
                                     a.RegistroId = funcionarioIdStr AndAlso
                                     a.FechaHora >= fechaInicio AndAlso
                                     a.FechaHora < fechaFin) _
                .AsNoTracking().ToListAsync()

            ' Unificar todos los tipos de eventos en una sola lista DTO
            Dim eventosUnificados As New List(Of EventoSituacionDTO)

            eventosUnificados.AddRange(estadosEnPeriodo _
                .Select(Function(et) New EventoSituacionDTO(et)) _
                .Where(Function(dto) dto.Desde.HasValue AndAlso
                                     dto.Desde.Value < fechaFin AndAlso
                                     (Not dto.Hasta.HasValue OrElse dto.Hasta.Value >= fechaInicio)))

            eventosUnificados.AddRange(licenciasEnPeriodo.Select(Function(l) New EventoSituacionDTO(l)))
            eventosUnificados.AddRange(notificacionesEnPeriodo.Select(Function(n) New EventoSituacionDTO(n)))
            eventosUnificados.AddRange(cambiosAuditados.Select(Function(a) New EventoSituacionDTO(a)))

            ' Ordenar la lista final por severidad y fecha descendente
            Dim eventosOrdenados = eventosUnificados _
                .OrderByDescending(Function(ev) ev.Severidad) _
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

#End Region

#Region " Grilla / Doble clic / Estilos "
    Private Sub dgvEstados_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex <> dgvEstados.Columns("colAcciones").Index Then Return
        Dim evento = TryCast(dgvEstados.Rows(e.RowIndex).DataBoundItem, EventoSituacionDTO)
        If evento Is Nothing Then Return

        Select Case evento.TipoEvento
            Case "Notificación"
                AccionesNotificacion(evento)
            Case "Estado"
                AccionesEstado(evento)
        End Select
    End Sub


    Private Sub AccionesNotificacion(evento As EventoSituacionDTO)
        Dim menu As New ContextMenuStrip()
        Dim cambiarEstadoItem = menu.Items.Add("Cambiar Estado")
        Dim imprimirItem = menu.Items.Add("Imprimir")
        Dim editarItem = menu.Items.Add("Editar")
        Dim eliminarItem = menu.Items.Add("Eliminar")

        AddHandler cambiarEstadoItem.Click, Async Sub()
                                                Using frm As New frmNotificacionCambiarEstado(evento.Id)
                                                    If frm.ShowDialog() = DialogResult.OK Then
                                                        Using svc As New NotificacionService()
                                                            Dim success = Await svc.UpdateEstadoAsync(evento.Id, frm.SelectedEstadoId)
                                                            If success Then
                                                                Notifier.Success(Me, "Estado actualizado.")
                                                                Await ActualizarTodo()
                                                            Else
                                                                Notifier.Error(Me, "No se pudo actualizar el estado.")
                                                            End If
                                                        End Using
                                                    End If
                                                End Using
                                            End Sub
        AddHandler imprimirItem.Click, Sub()
                                           Dim frm As New frmNotificacionRPT(evento.Id)
                                           NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
                                       End Sub
        AddHandler editarItem.Click, Sub()
                                         Dim frm As New frmNotificacionCrear(evento.Id)
                                         NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
                                     End Sub
        AddHandler eliminarItem.Click, Async Sub()
                                           If MessageBox.Show("¿Está seguro que desea eliminar esta notificación?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                               Using svc As New NotificacionService()
                                                   Await svc.DeleteNotificacionAsync(evento.Id)
                                                   Notifier.Success(Me, "Notificación eliminada.")
                                                   Await ActualizarTodo()
                                               End Using
                                           End If
                                       End Sub
        menu.Show(Cursor.Position)

    End Sub


    Private Sub AccionesEstado(evento As EventoSituacionDTO)
        Dim menu As New ContextMenuStrip()
        Dim editarItem = menu.Items.Add("Editar")
        Dim quitarItem = menu.Items.Add("Quitar")

        AddHandler editarItem.Click, Sub()
                                         ' TODO: Implementar la lógica para editar el estado
                                         MessageBox.Show("Funcionalidad para editar estados no implementada.")
                                     End Sub
        AddHandler quitarItem.Click, Async Sub()
                                         If MessageBox.Show("¿Está seguro que desea quitar este estado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                                             Dim estado = Await _uow.Context.Set(Of EstadoTransitorio)().FindAsync(evento.Id)
                                             If estado IsNot Nothing Then
                                                 _uow.Context.Set(Of EstadoTransitorio)().Remove(estado)
                                                 Await _uow.Context.SaveChangesAsync()
                                                 Notifier.Success(Me, "Estado quitado.")
                                                 Await ActualizarTodo()
                                             End If
                                         End If
                                     End Sub
        menu.Show(Cursor.Position)

    End Sub
    Private Async Sub dgvEstados_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 OrElse dgvEstados.CurrentRow Is Nothing Then Return
        Try
            Dim evento = TryCast(dgvEstados.CurrentRow.DataBoundItem, EventoSituacionDTO)
            If evento Is Nothing OrElse evento.Id = 0 Then
                MessageBox.Show("No se pudo identificar el ID del registro seleccionado.", "Aviso",
                                 MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Select Case evento.TipoEvento
                Case "Estado"
                    If evento.Tipo.ToUpper.Contains("DESIGNACION") Then
                        Await MostrarReporteDesignacionAsync(evento.Id)
                    Else
                        MessageBox.Show("No hay una acción específica definida para este tipo de estado.", "Información",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Case "Notificación"
                    Dim frm As New frmNotificacionRPT(evento.Id)
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

            Dim estadosDesignacion = Await _uow.Context.Set(Of EstadoTransitorio)() _
                .Include("TipoEstadoTransitorio") _
                .Include("DesignacionDetalle") _
                .Where(Function(e) e.FuncionarioId = _funcionarioId AndAlso e.TipoEstadoTransitorio.Nombre.ToUpper().Contains("DESIGNACI")) _
                .OrderByDescending(Function(e) e.Id) _
                .AsNoTracking() _
                .ToListAsync()

            If estadosDesignacion Is Nothing OrElse Not estadosDesignacion.Any() Then
                MessageBox.Show("No se encontraron estados de 'Designación' para este funcionario.", "Aviso",
                                 MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim lista As New List(Of DesignacionSeleccionDTO)
            For Each est In estadosDesignacion
                If est.DesignacionDetalle IsNot Nothing Then
                    lista.Add(New DesignacionSeleccionDTO With {
                        .EstadoTransitorioId = est.Id,
                        .FechaDesde = est.DesignacionDetalle.FechaDesde,
                        .FechaHasta = est.DesignacionDetalle.FechaHasta,
                        .Descripcion = $"ID {est.Id} - Desde {est.DesignacionDetalle.FechaDesde:dd/MM/yyyy}"
                    })
                End If
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
            .Name = "Tipo", .DataPropertyName = "Tipo", .HeaderText = "Tipo de Evento",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 180
        })

        Dim colDetalles As New DataGridViewTextBoxColumn With {
            .Name = "Detalles", .DataPropertyName = "Detalles", .HeaderText = "Detalles del Evento",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .MinimumWidth = 400
        }
        colDetalles.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvEstados.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvEstados.Columns.Add(colDetalles)

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
        dgvEstados.Columns.Add(colAcciones)
    End Sub


    Private Sub DgvNovedades_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        ConfigurarGrillaNovedades()
        If dgvNovedades.Columns.Contains("Id") Then dgvNovedades.Columns("Id").Visible = False
    End Sub

    Private Sub DgvEstados_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs)
        ConfigurarGrillaEstados()
        If dgvEstados.Columns.Contains("Id") Then dgvEstados.Columns("Id").Visible = False
        If dgvEstados.Columns.Contains("TipoEvento") Then dgvEstados.Columns("TipoEvento").Visible = False
        AplicarColoresEstados(dgvEstados)
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

#End Region

#Region " Severidad y Colores "

    Private Sub AplicarColoresEstados(dgv As DataGridView)
        For Each row As DataGridViewRow In dgv.Rows
            Dim evento = TryCast(row.DataBoundItem, EventoSituacionDTO)
            If evento Is Nothing Then Continue For

            Dim colorFondo = EstadoVisualHelper.ObtenerColor(evento.Severidad)
            Dim colorTexto = EstadoVisualHelper.ObtenerColorTexto(evento.Severidad)

            row.DefaultCellStyle.BackColor = colorFondo
            row.DefaultCellStyle.ForeColor = colorTexto
            row.DefaultCellStyle.SelectionBackColor = colorFondo
            row.DefaultCellStyle.SelectionForeColor = colorTexto
        Next
    End Sub

#End Region

#Region " Helpers, DTOs y Clases Internas "

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim fechaInicio = dtpDesde.Value.Date
        Dim fechaFin = dtpHasta.Value.Date
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
        dgv.DefaultCellStyle.Padding = New Padding(5, 5, 5, 5) ' Padding para multilínea
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
        dgv.DefaultCellStyle.SelectionForeColor = Color.White
        dgv.RowsDefaultCellStyle.BackColor = Color.White
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)
    End Sub

    ' DTO unificado para mostrar cualquier tipo de evento en la grilla
    Private Class EventoSituacionDTO
        Public Property Id As Integer
        Public Property TipoEvento As String
        Public Property Tipo As String
        Public Property Detalles As String
        Public Property Desde As Date?
        Public Property Hasta As Date?
        Public Property Severidad As EstadoVisualHelper.EventoSeveridad

        Public Sub New(estado As EstadoTransitorio)
            Me.Id = estado.Id
            Me.TipoEvento = "Estado"
            Me.Tipo = estado.TipoEstadoTransitorio.Nombre
            Me.Severidad = EstadoVisualHelper.DeterminarSeveridad(Me.Tipo)

            Dim sb As New StringBuilder()

            Select Case Me.Tipo
                Case "Baja de Funcionario"
                    Dim d = estado.BajaDeFuncionarioDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = Nothing
                        If Not String.IsNullOrWhiteSpace(d.Resolucion) Then sb.AppendLine($"Resolución: {d.Resolucion.Trim()}")
                        If d.FechaResolucion.HasValue Then sb.AppendLine($"Fecha Res.: {d.FechaResolucion.Value:dd/MM/yyyy}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Cambio de Cargo"
                    Dim d = estado.CambioDeCargoDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        Dim cargoAnt = If(d.Cargo IsNot Nothing, d.Cargo.Nombre, "N/A")
                        Dim cargoNue = If(d.Cargo1 IsNot Nothing, d.Cargo1.Nombre, "N/A")
                        sb.AppendLine($"Cambio de '{cargoAnt}' a '{cargoNue}'.")
                        If Not String.IsNullOrWhiteSpace(d.Resolucion) Then sb.AppendLine($"Resolución: {d.Resolucion.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Desarmado"
                    Dim d = estado.DesarmadoDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        If Not String.IsNullOrWhiteSpace(d.Resolucion) Then sb.AppendLine($"Resolución: {d.Resolucion.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Designación"
                    Dim d = estado.DesignacionDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        If Not String.IsNullOrWhiteSpace(d.DocResolucion) Then sb.AppendLine($"Resolución: {d.DocResolucion.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Enfermedad"
                    Dim d = estado.EnfermedadDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        If Not String.IsNullOrWhiteSpace(d.Diagnostico) Then sb.AppendLine($"Diagnóstico: {d.Diagnostico.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Inicio de Procesamiento"
                    Dim d = estado.InicioDeProcesamientoDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        If Not String.IsNullOrWhiteSpace(d.Expediente) Then sb.AppendLine($"Expediente: {d.Expediente.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Orden Cinco"
                    Dim d = estado.OrdenCincoDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Reactivación de Funcionario"
                    Dim d = estado.ReactivacionDeFuncionarioDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = Nothing
                        If Not String.IsNullOrWhiteSpace(d.Resolucion) Then sb.AppendLine($"Resolución: {d.Resolucion.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Retén"
                    Dim d = estado.RetenDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaReten
                        Me.Hasta = d.FechaReten
                        If Not String.IsNullOrWhiteSpace(d.Turno) Then sb.AppendLine($"Turno: {d.Turno.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.AsignadoPor) Then sb.AppendLine($"Asignado por: {d.AsignadoPor.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Sanción"
                    Dim d = estado.SancionDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        If Not String.IsNullOrWhiteSpace(d.TipoSancion) Then sb.AppendLine($"Tipo Sanción: {d.TipoSancion.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Resolucion) Then sb.AppendLine($"Resolución: {d.Resolucion.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Separación del Cargo"
                    Dim d = estado.SeparacionDelCargoDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        If Not String.IsNullOrWhiteSpace(d.Resolucion) Then sb.AppendLine($"Resolución: {d.Resolucion.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Sumario"
                    Dim d = estado.SumarioDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        If Not String.IsNullOrWhiteSpace(d.Expediente) Then sb.AppendLine($"Expediente: {d.Expediente.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
                Case "Traslado"
                    Dim d = estado.TrasladoDetalle
                    If d IsNot Nothing Then
                        Me.Desde = d.FechaDesde
                        Me.Hasta = d.FechaHasta
                        If Not String.IsNullOrWhiteSpace(d.Resolucion) Then sb.AppendLine($"Resolución: {d.Resolucion.Trim()}")
                        If Not String.IsNullOrWhiteSpace(d.Observaciones) Then sb.AppendLine(d.Observaciones.Trim())
                    End If
            End Select

            Me.Detalles = sb.ToString().Trim()
        End Sub

        Public Sub New(licencia As HistoricoLicencia)
            Me.Id = licencia.Id
            Me.TipoEvento = "Licencia"
            Me.Tipo = licencia.TipoLicencia.Nombre ' <--- CORRECTO
            Me.Desde = licencia.inicio
            Me.Hasta = licencia.finaliza
            Me.Detalles = licencia.Comentario?.Trim()
            Me.Severidad = EstadoVisualHelper.DeterminarSeveridad(Me.Tipo)
        End Sub

        Public Sub New(notificacion As NotificacionPersonal)
            Me.Id = notificacion.Id
            Me.TipoEvento = "Notificación"

            Dim estadoDescripcion = notificacion.NotificacionEstado?.Nombre?.Trim()
            Dim tipoBase = notificacion.TipoNotificacion?.Nombre?.Trim()
            If String.IsNullOrWhiteSpace(tipoBase) Then tipoBase = "Notificación"
            Me.Tipo = If(String.IsNullOrWhiteSpace(estadoDescripcion), tipoBase, $"{tipoBase} ({estadoDescripcion})")

            Me.Desde = notificacion.FechaProgramada
            ' --- INICIO DE LA CORRECCIÓN ---
            Me.Hasta = notificacion.FechaProgramada ' Asignamos la misma fecha a "Hasta"
            ' --- FIN DE LA CORRECCIÓN ---

            Dim sb As New StringBuilder()
            If Not String.IsNullOrWhiteSpace(notificacion.Medio) Then sb.AppendLine(notificacion.Medio.Trim())
            If Not String.IsNullOrWhiteSpace(notificacion.Documento) Then sb.AppendLine($"Documento: {notificacion.Documento.Trim()}")
            If Not String.IsNullOrWhiteSpace(notificacion.ExpMinisterial) Then sb.AppendLine($"Exp. Ministerial: {notificacion.ExpMinisterial.Trim()}")
            If Not String.IsNullOrWhiteSpace(notificacion.ExpINR) Then sb.AppendLine($"Exp. INR: {notificacion.ExpINR.Trim()}")
            If Not String.IsNullOrWhiteSpace(notificacion.Oficina) Then sb.AppendLine($"Oficina: {notificacion.Oficina.Trim()}")

            Me.Detalles = sb.ToString().Trim()

            Dim estadoVencidaId As Byte = CByte(ModConstantesApex.EstadoNotificacionPersonal.Vencida)
            Me.Severidad = If(notificacion.EstadoId = estadoVencidaId, EstadoVisualHelper.EventoSeveridad.Alta, EstadoVisualHelper.EventoSeveridad.Media)
        End Sub

        Public Sub New(auditoria As AuditoriaCambios)
            Me.Id = auditoria.Id
            Me.TipoEvento = "Auditoría"
            Me.Tipo = "Cambio de Datos"
            Me.Desde = auditoria.FechaHora
            Me.Hasta = Nothing
            Dim valAnt = If(String.IsNullOrWhiteSpace(auditoria.ValorAnterior), "[vacío]", auditoria.ValorAnterior)
            Dim valNue = If(String.IsNullOrWhiteSpace(auditoria.ValorNuevo), "[vacío]", auditoria.ValorNuevo)
            Me.Detalles = $"El campo '{auditoria.CampoNombre}' cambió de '{valAnt}' a '{valNue}'."
            Me.Severidad = EstadoVisualHelper.EventoSeveridad.Info
        End Sub

        ' La lógica de severidad se centraliza en EstadoVisualHelper.
    End Class

#End Region

End Class

' =================================================================================
' CLASE DTO AHORA PÚBLICA Y FUERA DEL FORMULARIO PRINCIPAL
' =================================================================================
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