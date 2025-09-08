Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Reporting.WinForms

Public Class frmDesignacionRPT
    Private ReadOnly _reportesService As New ReportesService()
    Private ReadOnly _notificacionId As Integer

    Public Sub New(notificacionId As Integer)
        InitializeComponent()
        _notificacionId = notificacionId
    End Sub

    Private Async Sub frmDesignacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarReporteAsync()
        AppTheme.Aplicar(Me)
    End Sub

    Private Async Function CargarReporteAsync() As Task
        Try
            Me.Cursor = Cursors.WaitCursor
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            Dim reportPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "DesignacionImprimir.rdlc")
            If Not File.Exists(reportPath) Then
                reportPath = Path.GetFullPath(Path.Combine(Application.StartupPath, "..\..\", "Reportes", "DesignacionImprimir.rdlc"))
            End If

            If Not File.Exists(reportPath) Then Throw New FileNotFoundException("No se encontró el RDLC en: " & reportPath)

            ReportViewer1.LocalReport.ReportPath = reportPath
            ReportViewer1.LocalReport.DisplayName = $"Designacion_{_notificacionId:000000}"

            ' --- INICIO DE CAMBIOS ---
            ' Llamamos al nuevo método y usamos el nuevo DTO
            Dim designacionData = Await _reportesService.GetDatosDesignacionAsync(_notificacionId)
            If designacionData Is Nothing Then
                MessageBox.Show("No se encontraron datos detallados para la designación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                Return
            End If

            Dim rds As New ReportDataSource("DataSetNotificaciones",
                                            New List(Of DesignacionReporteDTO) From {designacionData})
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ' --- FIN DE CAMBIOS ---

            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 100
            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class