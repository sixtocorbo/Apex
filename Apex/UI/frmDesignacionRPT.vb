Imports Microsoft.Reporting.WinForms

Public Class frmDesignacionRPT
    Private ReadOnly _reportesService As New ReportesService()
    Private ReadOnly _estadoTransitorioId As Integer

    Public Sub New(estadoTransitorioId As Integer)
        InitializeComponent()
        _estadoTransitorioId = estadoTransitorioId
        Me.KeyPreview = True
    End Sub

    Private Async Sub frmDesignacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Await CargarReporteAsync()
            AppTheme.Aplicar(Me)
        Catch ex As Exception
            MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Async Function CargarReporteAsync() As Task
        Dim old = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            ' RDLC: recurso embebido con fallbacks a disco (incluye tu ..\..\ para debug)
            ReportResourceLoader.LoadLocalReportDefinition(
                ReportViewer1.LocalReport,
                GetType(frmDesignacionRPT),
                "Apex.Reportes.DesignacionImprimir.rdlc",
                "DesignacionImprimir.rdlc",
                New String() {"..\..\Reportes\DesignacionImprimir.rdlc"}
            )

            ReportViewer1.LocalReport.DisplayName = $"Designacion_{_estadoTransitorioId:000000}"

            Dim designacionData = Await _reportesService.GetDatosDesignacionAsync(_estadoTransitorioId)
            If designacionData Is Nothing Then
                MessageBox.Show("No se encontraron datos para el reporte de la designación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                Return
            End If

            ' Nombre del DataSet en el RDLC: DataSetDesignaciones
            Dim rds As New ReportDataSource(
                "DataSetDesignaciones",
                New List(Of DesignacionReporteDTO) From {designacionData}
            )
            ReportViewer1.LocalReport.DataSources.Add(rds)

            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 100
            ReportViewer1.RefreshReport()

            Await Task.Yield()
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
