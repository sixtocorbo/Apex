Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms
Imports Microsoft.Reporting.WinForms

Public Class frmNotificacionRPT
    Private ReadOnly _reportesService As New ReportesService()
    Private ReadOnly _notificacionId As Integer

    Public Sub New(notificacionId As Integer)
        InitializeComponent()
        _notificacionId = notificacionId
    End Sub

    Private Async Sub frmNotificacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True

        Try
            Await CargarReporteAsync()
            Notifier.Success(Me, "Reporte listo para imprimir.")
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo cargar el reporte: {ex.Message}")
            Close()
        End Try
    End Sub


    Private Async Function CargarReporteAsync() As Task
        Dim oldCursor = Me.Cursor
        Dim oldEnabled = btnConfirmarFirma.Enabled
        Me.Cursor = Cursors.WaitCursor
        btnConfirmarFirma.Enabled = False

        Try
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            Dim base = AppDomain.CurrentDomain.BaseDirectory
            Dim reportPath As String = Path.Combine(base, "Reportes", "NotificacionImprimir.rdlc")
            If Not File.Exists(reportPath) Then
                reportPath = Path.GetFullPath(Path.Combine(Application.StartupPath, "..\..\", "Reportes", "NotificacionImprimir.rdlc"))
            End If
            If Not File.Exists(reportPath) Then
                Notifier.[Error](Me, "No se encontró el archivo de reporte 'NotificacionImprimir.rdlc'.")
                Close()
                Return
            End If

            ReportViewer1.LocalReport.ReportPath = reportPath
            ReportViewer1.LocalReport.DisplayName = $"Notificacion_{_notificacionId:000000}"

            Dim notificacionData = Await _reportesService.GetDatosNotificacionAsync(_notificacionId)
            If notificacionData Is Nothing Then
                Notifier.Info(Me, "No se encontraron datos para esta notificación.")
                Close()
                Return
            End If

            Dim reportDataList As New List(Of Object) From {notificacionData}
            Dim rds As New ReportDataSource("DataSetNotificaciones", reportDataList)
            ReportViewer1.LocalReport.DataSources.Add(rds)

            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 100
            ReportViewer1.RefreshReport()

        Catch ex As Exception
            Notifier.[Error](Me, $"Error al cargar el reporte: {ex.Message}")
            Throw
        Finally
            Me.Cursor = oldCursor
            btnConfirmarFirma.Enabled = oldEnabled
        End Try
    End Function


    ' --- LÓGICA PARA CONFIRMAR LA FIRMA ---
    Private Async Sub btnConfirmarFirma_Click(sender As Object, e As EventArgs) Handles btnConfirmarFirma.Click
        If MessageBox.Show("¿Confirmás que la notificación fue firmada y deseás archivarla?",
                       "Confirmar Acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Dim oldCursor = Me.Cursor
        Dim oldEnabled = btnConfirmarFirma.Enabled
        btnConfirmarFirma.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        LoadingHelper.MostrarCargando(Me)

        Try
            Using uow As New UnitOfWork()
                Dim notificacion = Await uow.Context.Set(Of NotificacionPersonal).FindAsync(_notificacionId)
                If notificacion Is Nothing Then
                    Notifier.[Error](Me, "No se encontró la notificación para actualizar.")
                    Return
                End If

                notificacion.EstadoId = CByte(ModConstantesApex.EstadoNotificacionPersonal.Firmada)
                notificacion.FechaProgramada = Date.Now

                Await uow.CommitAsync()
                NotificadorEventos.NotificarCambiosEnFuncionario(notificacion.FuncionarioId)
                Notifier.Success(Me, "Notificación archivada correctamente.")
                Close()
            End Using
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al actualizar el estado: {ex.Message}")
        Finally
            LoadingHelper.OcultarCargando(Me)
            Me.Cursor = oldCursor
            btnConfirmarFirma.Enabled = oldEnabled
        End Try
    End Sub


    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        ElseIf e.Control AndAlso e.KeyCode = Keys.P Then
            ' Atajo: Ctrl+P para abrir el diálogo de impresión del visor
            Try
                ReportViewer1.PrintDialog()
            Catch
                Notifier.Warn(Me, "No fue posible abrir el diálogo de impresión.")
            End Try
            e.Handled = True
        End If
    End Sub

End Class