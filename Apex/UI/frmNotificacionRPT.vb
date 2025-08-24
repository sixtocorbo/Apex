Imports Microsoft.Reporting.WinForms
Imports System.Collections.Generic
Imports System.Windows.Forms

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
            ReportViewer1.LocalReport.ReportPath = "Reportes\NotificacionImprimir.rdlc"

            ' 1. Obtenemos los datos desde el servicio.
            Dim notificacionData = Await _reportesService.GetDatosNotificacionAsync(_notificacionId)

            ' 2. Verificamos si obtuvimos datos.
            If notificacionData Is Nothing Then
                MessageBox.Show("No se encontraron datos para la notificación seleccionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                Return
            End If

            ' 3. Creamos el origen de datos.
            '    LA SIGUIENTE LÍNEA ES LA MÁS IMPORTANTE.
            '    El nombre "DataSetNotificaciones" DEBE ser idéntico al del archivo .rdlc
            Dim rds As New ReportDataSource("DataSetNotificaciones", New List(Of vw_NotificacionesCompletas) From {notificacionData})

            ' 4. Añadimos el origen de datos al reporte y lo actualizamos.
            ReportViewer1.LocalReport.DataSources.Add(rds)
            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function
End Class