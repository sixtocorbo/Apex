Imports Apex.Data
Imports Apex.Services

Public Class frmConceptoFuncionalApex

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

    Private Sub btnBuscarFuncionario_Click(sender As Object, e As EventArgs) Handles btnBuscarFuncionario.Click
        Using frm As New frmFuncionarioBuscar(_funcionarioService)
            If frm.ShowDialog() = DialogResult.OK AndAlso frm.FuncionarioSeleccionado IsNot Nothing Then
                _funcionarioSeleccionado = frm.FuncionarioSeleccionado
                txtFuncionarioSeleccionado.Text = $"{_funcionarioSeleccionado.Apellido}, {_funcionarioSeleccionado.Nombre}"
                ActualizarDatos()
            End If
        End Using
    End Sub

    Private Sub Filtros_ValueChanged(sender As Object, e As EventArgs) Handles dtpFechaInicio.ValueChanged, dtpFechaFin.ValueChanged
        ActualizarDatos()
    End Sub

    Private Sub ActualizarDatos()
        If _funcionarioSeleccionado Is Nothing Then Return

        ' MostrarCargando(Me) ' -> Asumo que tienes un helper para esto en Apex
        Try
            Dim funcionarioId = _funcionarioSeleccionado.Id
            Dim fechaInicio = dtpFechaInicio.Value.Date
            Dim fechaFin = dtpFechaFin.Value.Date

            dgvLicenciasMedicas.DataSource = _conceptoService.ObtenerIncidencias(funcionarioId, fechaInicio, fechaFin, ModConstantesApex.CATEGORIA_SALUD)
            dgvSanciones.DataSource = _conceptoService.ObtenerIncidencias(funcionarioId, fechaInicio, fechaFin, ModConstantesApex.CATEGORIA_SANCION_GRAVE)
            dgvObservaciones.DataSource = _conceptoService.ObtenerObservaciones(funcionarioId, fechaInicio, fechaFin)

        Catch ex As Exception
            MessageBox.Show($"Error al obtener los datos: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' OcultarCargando(Me) ' -> Asumo que tienes un helper para esto
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

        ' Aquí iría la lógica para llamar al formulario de ReportViewer,
        ' pasándole los parámetros necesarios para generar el RDLC.
        MessageBox.Show("Lógica para generar el informe no implementada.", "Información")
    End Sub

    ' --- CONFIGURACIÓN DE GRIDS Y CLASES DTO ---

    Private Sub ConfigurarDataGridViews()
        ConfigurarGridIncidencias(dgvLicenciasMedicas)
        ConfigurarGridIncidencias(dgvSanciones)

        dgvObservaciones.AutoGenerateColumns = False
        dgvObservaciones.Columns.Clear()
        dgvObservaciones.Columns.AddRange(
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Fecha", .HeaderText = "Fecha", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Causa", .HeaderText = "Causa", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Sancion", .HeaderText = "Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
        )
    End Sub

    Private Sub ConfigurarGridIncidencias(ByVal dgv As DataGridView)
        dgv.AutoGenerateColumns = False
        dgv.Columns.Clear()
        dgv.Columns.AddRange(
            New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaInicio", .HeaderText = "Inicio", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaFinal", .HeaderText = "Fin", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Tipo", .HeaderText = "Tipo", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells},
            New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .HeaderText = "Observaciones", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill}
        )
    End Sub
End Class

' --- Clases DTO para el DataBinding ---
Public Class IncidenciaUI
    Public Property FechaInicio As DateTime
    Public Property FechaFinal As DateTime?
    Public Property Tipo As String
    Public Property Observaciones As String
End Class

Public Class ObservacionUI
    Public Property Fecha As DateTime
    Public Property Causa As String
    Public Property Sancion As String
End Class