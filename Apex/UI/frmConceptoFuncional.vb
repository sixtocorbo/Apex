
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
        dgv.AutoGenerateColumns = False
        dgv.Columns.Clear()
        dgv.Columns.AddRange(
            New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaInicio", .HeaderText = "Inicio", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaFinal", .HeaderText = "Fin", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Tipo", .HeaderText = "Tipo", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .HeaderText = "Detalle/Observaciones", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Origen", .HeaderText = "Origen", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
        )
    End Sub

    Private Sub frmConceptoFuncional_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        Try
            AppTheme.SetCue(txtFuncionarioSeleccionado, "Seleccione un funcionario...")

        Catch
            ' Ignorar si no existe SetCue
        End Try
    End Sub
    ' NUEVO: Agrega un evento FormClosed para desuscribirte
    Private Sub frmConceptoFuncional_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        _unitOfWork.Dispose() ' Buena práctica para liberar recursos
    End Sub
#Region "Manejo de Notificaciones"



    Private Sub ManejarCambiosEnFuncionario(sender As Object, e As FuncionarioCambiadoEventArgs)
        If _funcionarioSeleccionado Is Nothing OrElse e Is Nothing Then Return

        ' Si viene sin Id → refresco “global” (por ejemplo, cambios de catálogos)
        If Not e.FuncionarioId.HasValue Then
            If Me.InvokeRequired Then
                Me.BeginInvoke(New Action(AddressOf ActualizarDatos))
            Else
                ActualizarDatos()
            End If
            Exit Sub
        End If

        ' Sólo refrescar si el cambio es del funcionario actualmente seleccionado
        If e.FuncionarioId.Value <> _funcionarioSeleccionado.Id Then Return

        ' Asegurar ejecución en el hilo de UI
        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(Sub()
                                          ActualizarDatos()
                                          Notifier.Info(Me, "Los datos del funcionario se han actualizado.")
                                      End Sub))
        Else
            ActualizarDatos()
            Notifier.Info(Me, "Los datos del funcionario se han actualizado.")
        End If
    End Sub

#End Region
End Class