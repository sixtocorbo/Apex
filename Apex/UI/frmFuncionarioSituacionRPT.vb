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
        Me.KeyPreview = True
    End Sub

    Private Async Sub frmFuncionarioSituacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Await CargarReporte()
            ' (opcional) si usás AppTheme:
            ' AppTheme.Aplicar(Me)
        Catch ex As Exception
            MessageBox.Show("Error al generar el reporte: " & ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Async Function CargarReporte() As Task
        Dim old = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            Using uow As New UnitOfWork()
                Dim repo As New ReportesService(uow)
                Dim datos = Await repo.ObtenerDatosSituacionAsync(_funcionarioId, _fechaDesde, _fechaHasta)
                Dim funcionario = Await uow.Context.Set(Of Funcionario).FindAsync(_funcionarioId)

                ReportViewer1.ProcessingMode = ProcessingMode.Local
                ReportViewer1.LocalReport.DataSources.Clear()

                ' RDLC: Embedded → BaseDirectory\Reportes → StartupPath\Reportes → ClickOnce
                ReportResourceLoader.LoadLocalReportDefinition(
                    ReportViewer1.LocalReport,
                    GetType(frmFuncionarioSituacionRPT),
                    "Apex.Reportes.SituacionFuncionario.rdlc",
                    "SituacionFuncionario.rdlc"
                )

                ' DataSource: debe coincidir con el nombre del DataSet en el RDLC
                Dim rds As New ReportDataSource("DataSetSituacion", datos)
                ReportViewer1.LocalReport.DataSources.Add(rds)

                ' Parámetros
                Dim pNombre As New ReportParameter("FuncionarioNombre", funcionario?.Nombre)
                Dim pPeriodo As New ReportParameter("Periodo", $"Desde: {_fechaDesde:dd/MM/yyyy} Hasta: {_fechaHasta:dd/MM/yyyy}")
                ReportViewer1.LocalReport.SetParameters({pNombre, pPeriodo})

                ' Presentación
                ReportViewer1.LocalReport.DisplayName = $"Situacion_{_funcionarioId:000000}"
                ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
                ReportViewer1.ZoomMode = ZoomMode.Percent
                ReportViewer1.ZoomPercent = 100

                ReportViewer1.RefreshReport()
                Await Task.Yield()
            End Using

        Finally
            Me.Cursor = old
        End Try
    End Function

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub
End Class
