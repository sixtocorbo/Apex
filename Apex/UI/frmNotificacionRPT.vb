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
        Await CargarReporteAsync()
        AppTheme.Aplicar(Me)
    End Sub

    Private Async Function CargarReporteAsync() As Task
        Try
            Me.Cursor = Cursors.WaitCursor
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            Dim reportPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "NotificacionImprimir.rdlc")
            If Not File.Exists(reportPath) Then
                reportPath = Path.GetFullPath(Path.Combine(Application.StartupPath, "..\..\", "Reportes", "NotificacionImprimir.rdlc"))
            End If
            If Not File.Exists(reportPath) Then Throw New FileNotFoundException("No se encontró el archivo del reporte (RDLC) en: " & reportPath)

            ReportViewer1.LocalReport.ReportPath = reportPath
            ReportViewer1.LocalReport.DisplayName = $"Notificacion_{_notificacionId:000000}"

            ' Se asume que tu ReportesService tiene un método GetDatosNotificacionAsync que devuelve los datos necesarios.
            Dim notificacionData = Await _reportesService.GetDatosNotificacionAsync(_notificacionId)
            If notificacionData Is Nothing Then
                MessageBox.Show("No se encontraron datos detallados para la notificación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                Return
            End If

            ' --- INICIO DE LA CORRECCIÓN ---
            ' El ReportViewer espera una colección (IEnumerable), no un solo objeto.
            ' Creamos una lista y añadimos el único objeto a ella.
            ' REEMPLAZA "TuTipoDeObjetoNotificacion" por el nombre real de la clase que devuelve el servicio.

            Dim reportDataList As New List(Of Object) From {notificacionData}

            ' Ahora pasamos la lista al ReportDataSource en lugar del objeto individual.
            Dim rds As New ReportDataSource("DataSetNotificaciones", reportDataList)
            ' --- FIN DE LA CORRECCIÓN ---

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

    ' --- LÓGICA PARA CONFIRMAR LA FIRMA ---
    Private Async Sub btnConfirmarFirma_Click(sender As Object, e As EventArgs) Handles btnConfirmarFirma.Click
        Try
            Dim confirmResult = MessageBox.Show("¿Confirmas que la notificación fue firmada y deseas archivarla?",
                                                 "Confirmar Acción",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question)

            If confirmResult = DialogResult.Yes Then
                LoadingHelper.MostrarCargando(Me)

                Using uow As New UnitOfWork()
                    Dim notificacion = Await uow.Context.Set(Of NotificacionPersonal).FindAsync(_notificacionId)

                    If notificacion IsNot Nothing Then
                        ' Asumimos que el ID de estado para "Notificado" o "Firmado" es 2.
                        ' ¡Verifica en tu tabla NotificacionEstado que este sea el ID correcto!
                        notificacion.EstadoId = CByte(ModConstantesApex.EstadoNotificacionPersonal.Firmada)

                        notificacion.FechaProgramada = Date.Now ' Guardamos la fecha actual

                        Await uow.CommitAsync()

                        ' Notificamos al resto de la aplicación que los datos de este funcionario cambiaron.
                        NotificadorEventos.NotificarCambiosEnFuncionario(notificacion.FuncionarioId)

                        MessageBox.Show("La notificación ha sido archivada correctamente.", "Éxito",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)

                        Me.Close() ' Cerramos la ventana de impresión.
                    Else
                        MessageBox.Show("No se encontró la notificación para actualizar.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End Using
            End If

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al actualizar el estado: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class