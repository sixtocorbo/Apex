' Apex/UI/frmConceptoFuncionalRPT.vb
Imports Microsoft.Reporting.WinForms

Public Class frmConceptoFuncionalRPT

    Private ReadOnly _funcionario As Funcionario
    Private ReadOnly _fechaInicio As Date
    Private ReadOnly _fechaFin As Date
    Private ReadOnly _incidenciasSalud As List(Of IncidenciaUI)
    Private ReadOnly _incidenciasSanciones As List(Of IncidenciaUI)
    Private ReadOnly _observaciones As List(Of ObservacionUI)

    Public Sub New(funcionario As Funcionario, fechaInicio As Date, fechaFin As Date,
                   incidenciasSalud As List(Of IncidenciaUI),
                   incidenciasSanciones As List(Of IncidenciaUI),
                   observaciones As List(Of ObservacionUI))
        InitializeComponent()
        _funcionario = funcionario
        _fechaInicio = fechaInicio
        _fechaFin = fechaFin
        _incidenciasSalud = incidenciasSalud
        _incidenciasSanciones = incidenciasSanciones
        _observaciones = observaciones
    End Sub

    Private Sub frmConceptoFuncionalRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Limpiar fuentes de datos anteriores
            Me.ReportViewer1.LocalReport.DataSources.Clear()

            ' Configurar los parámetros del informe
            Dim pNombre As New ReportParameter("pNombreCompleto", _funcionario.Nombre)
            Dim pCedula As New ReportParameter("pCedula", _funcionario.CI)
            Dim pCargo As New ReportParameter("pCargo", If(_funcionario.Cargo IsNot Nothing, _funcionario.Cargo.Nombre, "N/A"))
            Dim pFechaIngreso As New ReportParameter("pFechaIngreso", _funcionario.FechaIngreso.ToShortDateString())
            Dim pPeriodo As New ReportParameter("pPeriodo", $"{_fechaInicio:dd/MM/yyyy} al {_fechaFin:dd/MM/yyyy}")
            Me.ReportViewer1.LocalReport.SetParameters({pNombre, pCedula, pCargo, pFechaIngreso, pPeriodo})

            ' Configurar las fuentes de datos (DataSets)
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsLicenciasMedicas", _incidenciasSalud))
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsSancionesGraves", _incidenciasSanciones))
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsObservaciones", _observaciones))

            Me.ReportViewer1.RefreshReport()
        Catch ex As Exception
            MessageBox.Show("Error al generar el informe: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class