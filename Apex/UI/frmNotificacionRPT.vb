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
            ' Muestra un indicador de carga
            Me.Cursor = Cursors.WaitCursor
            ReportViewer1.ProcessingMode = ProcessingMode.Local

            ' Limpia cualquier dato anterior
            ReportViewer1.LocalReport.DataSources.Clear()

            ' 1. Define la ruta del archivo RDLC.
            '    Asegúrate de que la propiedad "Copiar en el directorio de salida" del archivo
            '    NotificacionImprimir.rdlc esté establecida en "Copiar si es posterior".
            ReportViewer1.LocalReport.ReportPath = "Reportes\NotificacionImprimir.rdlc"

            ' 2. Obtiene los datos de la notificación usando nuestro servicio.
            Dim notificacionData = Await _reportesService.GetDatosNotificacionAsync(_notificacionId)
            If notificacionData Is Nothing Then
                MessageBox.Show("No se encontraron datos para la notificación seleccionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                Return
            End If

            ' 3. Crea una lista (el reporte espera una colección) y añade el objeto.
            Dim dataSourceList As New List(Of vw_NotificacionesCompletas) From {notificacionData}

            ' 4. Crea el ReportDataSource. El nombre "DataSetNotificaciones" debe coincidir con el del RDLC.
            Dim rds As New ReportDataSource("DataSetNotificaciones", dataSourceList)

            ' 5. Asigna el origen de datos al reporte.
            ReportViewer1.LocalReport.DataSources.Add(rds)

            ' 6. Actualiza y muestra el reporte.
            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show($"Error al cargar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' Restaura el cursor
            Me.Cursor = Cursors.Default
        End Try
    End Function
End Class