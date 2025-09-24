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
            ' (opcional) tema visual
            ' AppTheme.Aplicar(Me)
        Catch
        End Try

        Try
            Await CargarReporte()
        Catch ex As LocalProcessingException
            ' --- CAMBIO AQUÍ ---
            ' Muestra la excepción completa, incluyendo su tipo y el stack trace.
            Dim fullExceptionDetails As String = ex.ToString()
            MessageBox.Show(fullExceptionDetails, "Error Completo de Reporte", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ' ------------------
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error al generar el reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

                ' RDLC: Embedded → BaseDirectory\Reportes → StartupPath\Reportes → ClickOnce → extra (..\..)
                ReportResourceLoader.LoadLocalReportDefinition(
                    ReportViewer1.LocalReport,
                    GetType(frmFuncionarioSituacionRPT),
                    "Apex.Reportes.SituacionFuncionario.rdlc",
                    "SituacionFuncionario.rdlc",
                    New String() {"..\..\Reportes\SituacionFuncionario.rdlc"}
                )

                ' El nombre del DataSource debe coincidir con el DataSet definido en el RDLC (DataSetSituacion).
                Dim rds As New ReportDataSource("DataSetSituacion", datos)
                ReportViewer1.LocalReport.DataSources.Add(rds)

                ' Parámetros (solo si existen en el RDLC)
                Dim nombreFuncionario As String = If(funcionario?.Nombre, "N/A")
                Dim periodo As String = $"Desde: {_fechaDesde:dd/MM/yyyy} Hasta: {_fechaHasta:dd/MM/yyyy}"
                Dim pNombre As New ReportParameter("FuncionarioNombre", nombreFuncionario)
                Dim pPeriodo As New ReportParameter("Periodo", periodo)
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

    Private Sub frmFuncionarioSituacionRPT_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub

End Class