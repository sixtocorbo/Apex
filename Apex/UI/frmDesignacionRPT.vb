Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Reporting.WinForms

Public Class frmDesignacionRPT
    Private ReadOnly _reportesService As New ReportesService()
    ' --- CAMBIO 1: Renombrar la variable para mayor claridad ---
    Private ReadOnly _estadoTransitorioId As Integer

    ' --- CAMBIO 2: Actualizar el constructor ---
    Public Sub New(estadoTransitorioId As Integer)
        InitializeComponent()
        _estadoTransitorioId = estadoTransitorioId
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
            ReportViewer1.LocalReport.DisplayName = $"Designacion_{_estadoTransitorioId:000000}"

            Dim designacionData = Await _reportesService.GetDatosDesignacionAsync(_estadoTransitorioId)

            If designacionData Is Nothing Then
                MessageBox.Show("No se encontraron datos para el reporte de la designación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                Return
            End If

            ' --- CAMBIO CLAVE: El nombre del DataSource ahora es "DataSetDesignaciones" ---
            Dim rds As New ReportDataSource("DataSetDesignaciones",
                                        New List(Of DesignacionReporteDTO) From {designacionData})
            ReportViewer1.LocalReport.DataSources.Add(rds)

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