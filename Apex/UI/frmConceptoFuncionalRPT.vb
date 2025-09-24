
Imports Microsoft.Reporting.WinForms

Public Class frmConceptoFuncionalRPT

#Region "Campos y Constructor"

    Private ReadOnly _funcionario As Funcionario
    Private ReadOnly _fechaInicio As Date
    Private ReadOnly _fechaFin As Date
    Private ReadOnly _incidenciasSalud As List(Of ConceptoFuncionalItem)
    Private ReadOnly _sancionesGraves As List(Of ConceptoFuncionalItem)
    Private ReadOnly _observacionesYLeves As List(Of ConceptoFuncionalItem)

    Public Sub New(funcionario As Funcionario, fechaInicio As Date, fechaFin As Date,
                   incidenciasSalud As List(Of ConceptoFuncionalItem),
                   sancionesGraves As List(Of ConceptoFuncionalItem),
                   observacionesYLeves As List(Of ConceptoFuncionalItem))

        InitializeComponent()

        _funcionario = funcionario
        _fechaInicio = fechaInicio
        _fechaFin = fechaFin

        ' Evitar referencias nulas al pasar al ReportViewer
        _incidenciasSalud = If(incidenciasSalud, New List(Of ConceptoFuncionalItem)())
        _sancionesGraves = If(sancionesGraves, New List(Of ConceptoFuncionalItem)())
        _observacionesYLeves = If(observacionesYLeves, New List(Of ConceptoFuncionalItem)())

        Me.KeyPreview = True
    End Sub

#End Region

#Region "Eventos de Form"

    Private Async Sub frmConceptoFuncionalRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Await CargarReporteAsync()
        Catch ex As Exception
            MessageBox.Show("Error fatal al generar el informe: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub

#End Region

#Region "Lógica Principal del Reporte"

    Private Async Function CargarReporteAsync() As Task
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            ' 1) Definición del RDLC (recurso embebido con fallback a disco)
            ReportResourceLoader.LoadLocalReportDefinition(
                ReportViewer1.LocalReport,
                GetType(frmConceptoFuncionalRPT),
                "Apex.Reportes.ConceptoFuncional.rdlc",
                "ConceptoFuncional.rdlc"
            )

            ' 2) Parámetros del informe
            Dim pNombre As New ReportParameter("pNombreCompleto", _funcionario.Nombre)
            Dim pCedula As New ReportParameter("pCedula", _funcionario.CI)
            Dim pCargo As New ReportParameter("pCargo", If(_funcionario.Cargo IsNot Nothing, _funcionario.Cargo.Nombre, "N/A"))
            Dim pFechaIngreso As New ReportParameter("pFechaIngreso", _funcionario.FechaIngreso.ToShortDateString())
            Dim pPeriodo As New ReportParameter("pPeriodo", $"{_fechaInicio:dd/MM/yyyy} al {_fechaFin:dd/MM/yyyy}")

            ReportViewer1.LocalReport.SetParameters({pNombre, pCedula, pCargo, pFechaIngreso, pPeriodo})

            ' 3) DataSources (los nombres deben coincidir con los del RDLC)
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsLicenciasMedicas", _incidenciasSalud))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsSancionesGraves", _sancionesGraves))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsObservaciones", _observacionesYLeves))

            ' 4) Presentación
            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 100
            ReportViewer1.RefreshReport()

            ' Pequeña espera para garantizar render en UI si fuera necesario
            Await Task.Yield()

        Finally
            Me.Cursor = oldCursor
        End Try
    End Function

#End Region

End Class

