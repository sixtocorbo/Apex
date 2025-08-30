Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Reporting.WinForms

Public Class frmNotificacionRPT
    Private ReadOnly _reportesService As New ReportesService()
    Private ReadOnly _notificacionId As Integer

    ' Constructor modificado para recibir el ID de la notificación
    Public Sub New(notificacionId As Integer)
        ' This call is required by the designer.
        InitializeComponent()

        _notificacionId = notificacionId
    End Sub

    Private Async Sub frmNotificacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarReporteAsync()
    End Sub

    Private Async Function CargarReporteAsync() As Task
        Try
            Me.Cursor = Cursors.WaitCursor

            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            ' Ruta absoluta a bin\...\Reportes\NotificacionImprimir.rdlc
            Dim reportPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "NotificacionImprimir.rdlc")

            ' Fallback útil si corres desde VS y aún no está copiado
            If Not File.Exists(reportPath) Then
                reportPath = Path.GetFullPath(Path.Combine(Application.StartupPath, "..\..\", "Reportes", "NotificacionImprimir.rdlc"))
            End If

            If Not File.Exists(reportPath) Then
                Throw New FileNotFoundException("No se encontró el RDLC en: " & reportPath)
            End If

            ReportViewer1.LocalReport.ReportPath = reportPath
            ReportViewer1.LocalReport.DisplayName = $"Notificacion_{_notificacionId:000000}"

            Dim notificacionData = Await _reportesService.GetDatosNotificacionAsync(_notificacionId)
            If notificacionData Is Nothing Then
                MessageBox.Show("No se encontraron datos para la notificación seleccionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                Return
            End If

            Dim rds As New ReportDataSource("DataSetNotificaciones",
                         New List(Of vw_NotificacionesCompletas) From {notificacionData})
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
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class