' En frmFuncionarioSituacionRPT.vb
Imports Microsoft.Reporting.WinForms

Public Class frmFuncionarioSituacionRPT

    Private ReadOnly _funcionarioId As Integer
    Private ReadOnly _fechaDesde As Date
    Private ReadOnly _fechaHasta As Date

    Public Sub New(funcionarioId As Integer, fechaDesde As Date, fechaHasta As Date)
        InitializeComponent()
        _funcionarioId = funcionarioId
        _fechaDesde = fechaDesde
        _fechaHasta = fechaHasta
    End Sub

    Private Async Sub frmFuncionarioSituacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarReporte()
    End Sub

    Private Async Function CargarReporte() As Task
        Try
            Dim uow As New UnitOfWork()
            Dim repo As New ReportesService(uow)
            Dim datos = Await repo.ObtenerDatosSituacionAsync(_funcionarioId, _fechaDesde, _fechaHasta)
            Dim funcionario = Await uow.Context.Set(Of Funcionario).FindAsync(_funcionarioId)

            ' Limpiar orígenes de datos previos
            Me.ReportViewer1.LocalReport.DataSources.Clear()

            ReportResourceLoader.LoadLocalReportDefinition(
                Me.ReportViewer1.LocalReport,
                GetType(frmFuncionarioSituacionRPT),
                "Apex.Reportes.SituacionFuncionario.rdlc",
                "SituacionFuncionario.rdlc")

            ' Asignar el nuevo origen de datos
            Dim rds As New ReportDataSource("DataSetSituacion", datos) ' El nombre debe coincidir con el del RDLC
            Me.ReportViewer1.LocalReport.DataSources.Add(rds)

            ' Pasar parámetros al reporte (opcional pero recomendado)
            Dim pNombre As New ReportParameter("FuncionarioNombre", funcionario.Nombre)
            Dim pPeriodo As New ReportParameter("Periodo", $"Desde: {_fechaDesde:dd/MM/yyyy} Hasta: {_fechaHasta:dd/MM/yyyy}")
            Me.ReportViewer1.LocalReport.SetParameters({pNombre, pPeriodo})

            ' Actualizar y mostrar el reporte
            Me.ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("Error al generar el reporte: " & ex.Message)
        End Try
    End Function
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class
