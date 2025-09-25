
Public Class frmConceptoFuncional

    Private _funcionarioSeleccionado As Funcionario
    Private ReadOnly _unitOfWork As IUnitOfWork
    Private ReadOnly _conceptoService As ConceptoFuncionalService
    Private ReadOnly _funcionarioService As FuncionarioService

    Public Sub New()
        InitializeComponent()
        _unitOfWork = New UnitOfWork()
        _conceptoService = New ConceptoFuncionalService(_unitOfWork)
        _funcionarioService = New FuncionarioService(_unitOfWork)
        InicializarFormulario()
    End Sub

    Private Sub InicializarFormulario()
        ' --- APLICAR MEJORAS DE RENDIMIENTO ---
        dgvLicenciasMedicas.ActivarDobleBuffer(True)
        dgvSanciones.ActivarDobleBuffer(True)
        dgvObservaciones.ActivarDobleBuffer(True)

        ConfigurarDataGridViews()
        dtpFechaInicio.Value = DateTime.Now.AddMonths(-6)
        dtpFechaFin.Value = DateTime.Now
        ActualizarTemporalLabel()
    End Sub

    Private Async Sub btnBuscarFuncionario_Click(sender As Object, e As EventArgs) Handles btnBuscarFuncionario.Click
        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog() = DialogResult.OK AndAlso frm.FuncionarioSeleccionado IsNot Nothing Then
                Dim funcionarioMin = frm.FuncionarioSeleccionado
                _funcionarioSeleccionado = Await _funcionarioService.GetByIdAsync(funcionarioMin.Id)

                If _funcionarioSeleccionado IsNot Nothing Then
                    txtFuncionarioSeleccionado.Text = _funcionarioSeleccionado.Nombre
                    ActualizarDatos()
                End If
            End If
        End Using
    End Sub

    Private Sub Filtros_ValueChanged(sender As Object, e As EventArgs) Handles dtpFechaInicio.ValueChanged, dtpFechaFin.ValueChanged
        ActualizarDatos()
    End Sub

    Private Sub ActualizarDatos()
        If _funcionarioSeleccionado Is Nothing Then Return

        Try
            Dim funcionarioId = _funcionarioSeleccionado.Id
            Dim fechaInicio = dtpFechaInicio.Value.Date
            Dim fechaFin = dtpFechaFin.Value.Date

            ' Llamadas a los nuevos métodos refactorizados del servicio
            dgvLicenciasMedicas.DataSource = _conceptoService.ObtenerIncidenciasDeSalud(funcionarioId, fechaInicio, fechaFin)
            dgvSanciones.DataSource = _conceptoService.ObtenerSancionesGraves(funcionarioId, fechaInicio, fechaFin)
            dgvObservaciones.DataSource = _conceptoService.ObtenerObservacionesYLeves(funcionarioId, fechaInicio, fechaFin)

            ' Forzar la actualización visual de las grillas
            dgvLicenciasMedicas.Refresh()
            dgvSanciones.Refresh()
            dgvObservaciones.Refresh()

        Catch ex As Exception
            MessageBox.Show($"Error al obtener los datos: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ActualizarTemporalLabel()
        End Try
    End Sub

    Private Sub ActualizarTemporalLabel()
        lblTemporal.Text = $"Período evaluado: {dtpFechaInicio.Value:dd/MM/yyyy} - {dtpFechaFin.Value:dd/MM/yyyy}"
    End Sub

    Private Sub btnInforme_Click(sender As Object, e As EventArgs) Handles btnInforme.Click
        If _funcionarioSeleccionado Is Nothing Then
            MessageBox.Show("Debes seleccionar un funcionario para generar el informe.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Recolectar los datos de las grillas usando la clase unificada ConceptoFuncionalItem
        Dim salud = TryCast(dgvLicenciasMedicas.DataSource, List(Of ConceptoFuncionalItem))
        Dim graves = TryCast(dgvSanciones.DataSource, List(Of ConceptoFuncionalItem))
        Dim leves = TryCast(dgvObservaciones.DataSource, List(Of ConceptoFuncionalItem))

        ' Crear y mostrar el formulario del reporte, pasándole las listas
        Dim frm As New frmConceptoFuncionalRPT(_funcionarioSeleccionado, dtpFechaInicio.Value, dtpFechaFin.Value, salud, graves, leves)
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    ''' <summary>
    ''' Configura las 3 DataGridViews para que usen la clase DTO 'ConceptoFuncionalItem' corregida.
    ''' </summary>
    Private Sub ConfigurarDataGridViews()
        ConfigurarGridConcepto(dgvLicenciasMedicas)
        ConfigurarGridConcepto(dgvSanciones)
        ConfigurarGridConcepto(dgvObservaciones)
    End Sub

    Private Sub ConfigurarGridConcepto(ByVal dgv As DataGridView)
        ' --- CONFIGURACIÓN GENERAL (Estilo moderno) ---
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
        dgv.AutoGenerateColumns = False
        dgv.BackgroundColor = Color.White

        ' --- ESTILO DE ENCABEZADOS (Headers) ---
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersHeight = 40
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

        ' --- ESTILO DE FILAS (Rows) ---
        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
        dgv.DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
        dgv.DefaultCellStyle.SelectionForeColor = Color.White
        dgv.RowsDefaultCellStyle.BackColor = Color.White
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)

        ' --- DEFINICIÓN DE COLUMNAS (Mantenemos las tuyas con formato y anchos mínimos) ---
        dgv.Columns.Clear()

        Dim colInicio As New DataGridViewTextBoxColumn With {
        .DataPropertyName = "FechaInicio", .HeaderText = "Inicio",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 110
    }
        colInicio.DefaultCellStyle.Format = "dd/MM/yyyy"

        Dim colFin As New DataGridViewTextBoxColumn With {
        .DataPropertyName = "FechaFinal", .HeaderText = "Fin",
        .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 110
    }
        colFin.DefaultCellStyle.Format = "dd/MM/yyyy"

        dgv.Columns.AddRange(
        colInicio,
        colFin,
        New DataGridViewTextBoxColumn With {.DataPropertyName = "Tipo", .HeaderText = "Tipo", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 150},
        New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .HeaderText = "Detalle/Observaciones", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill},
        New DataGridViewTextBoxColumn With {.DataPropertyName = "Origen", .HeaderText = "Origen", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 120}
    )
    End Sub

    ' Reemplaza tus dos métodos frmConceptoFuncional_Load con este único método
    Private Sub frmConceptoFuncional_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Código del segundo Load
        AddHandler NotificadorEventos.FuncionarioActualizado, AddressOf ManejarCambiosEnFuncionario
        AppTheme.Aplicar(Me)

        Try
            ' Código del primer Load
            AppTheme.SetCue(txtFuncionarioSeleccionado, "Seleccione un funcionario...")
        Catch
            ' Ignorar si no existe SetCue
        End Try
    End Sub
    ' NUEVO: Agrega un evento FormClosed para desuscribirte
    Private Sub frmConceptoFuncional_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.FuncionarioActualizado, AddressOf ManejarCambiosEnFuncionario
        _unitOfWork.Dispose() ' Buena práctica para liberar recursos
    End Sub
#Region "Manejo de Notificaciones"



    Private Sub ManejarCambiosEnFuncionario(sender As Object, e As FuncionarioCambiadoEventArgs)
        If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return
        If _funcionarioSeleccionado Is Nothing OrElse e Is Nothing Then Return

        Dim debeRefrescar As Boolean =
            (Not e.FuncionarioId.HasValue) OrElse (e.FuncionarioId.Value = _funcionarioSeleccionado.Id)
        If Not debeRefrescar Then Return

        Dim refrescar As Action = Sub()
                                      If Me.IsDisposed Then Return
                                      ActualizarDatos()
                                      Notifier.Success(Me, "Los datos del funcionario se han actualizado.")
                                  End Sub

        If Me.InvokeRequired Then
            Me.BeginInvoke(refrescar)
        Else
            refrescar()
        End If
    End Sub


#End Region
End Class