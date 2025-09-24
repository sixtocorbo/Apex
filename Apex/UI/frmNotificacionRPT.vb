Imports System.IO
Imports System.Reflection
Imports Microsoft.Reporting.WinForms

''' <summary>
''' Formulario para visualizar e imprimir una o varias notificaciones.
''' </summary>
Public Class frmNotificacionRPT

#Region " Campos y Constructores "

    ' --- Servicios y IDs ---
    Private ReadOnly _reportesService As New ReportesService()
    Private ReadOnly _notificacionId As Integer
    Private ReadOnly _notificacionIds As List(Of Integer)

    ''' <summary>
    ''' Constructor para mostrar un único reporte de notificación.
    ''' </summary>
    ''' <param name="notificacionId">El ID de la notificación a mostrar.</param>
    Public Sub New(notificacionId As Integer)
        InitializeComponent()
        _notificacionId = notificacionId
        ' El botón de confirmar firma solo tiene sentido para una única notificación
        btnConfirmarFirma.Visible = True
    End Sub

    ''' <summary>
    ''' Constructor para mostrar múltiples notificaciones en un solo reporte (impresión masiva).
    ''' </summary>
    ''' <param name="ids">Lista de IDs de las notificaciones a mostrar.</param>
    Public Sub New(ids As List(Of Integer))
        InitializeComponent()
        _notificacionIds = ids
        ' No se puede confirmar la firma en modo masivo
        btnConfirmarFirma.Visible = False
    End Sub

#End Region

#Region " Ciclo de Vida del Formulario "

    Private Async Sub frmNotificacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True

        Try
            Await CargarReporteAsync()
            Notifier.Success(Me, "Reporte listo para imprimir o guardar.")
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo cargar el reporte: {ex.Message}")
            Close()
        End Try
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        ElseIf e.Control AndAlso e.KeyCode = Keys.P Then
            Try
                ReportViewer1.PrintDialog()
            Catch
                Notifier.Warn(Me, "No fue posible abrir el diálogo de impresión.")
            End Try
            e.Handled = True
        End If
    End Sub

#End Region

#Region " Lógica Principal del Reporte "

    Private Async Function CargarReporteAsync() As Task
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        LoadingHelper.MostrarCargando(Me)

        Try
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            ' --- 1. Localizar el archivo .rdlc ---
            Dim reportResourceName As String = "Apex.Reportes.NotificacionImprimir.rdlc"
            Dim reportLoaded As Boolean = False

            ' Intentar cargar el reporte como recurso incrustado para evitar problemas de distribución.
            Dim executingAssembly As Assembly = GetType(frmNotificacionRPT).Assembly
            Using reportStream As Stream = executingAssembly.GetManifestResourceStream(reportResourceName)
                If reportStream IsNot Nothing Then
                    ReportViewer1.LocalReport.LoadReportDefinition(reportStream)
                    reportLoaded = True
                End If
            End Using

            If Not reportLoaded Then
                ' Si no se encontró como recurso, intentamos buscarlo en disco como respaldo.
                Dim reportPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "NotificacionImprimir.rdlc")
                If Not File.Exists(reportPath) Then
                    reportPath = Path.GetFullPath(Path.Combine(Application.StartupPath, "..\..\", "Reportes", "NotificacionImprimir.rdlc"))
                End If
                If Not File.Exists(reportPath) Then
                    Throw New FileNotFoundException("No se encontró el recurso de reporte 'NotificacionImprimir.rdlc'.")
                End If

                ReportViewer1.LocalReport.ReportPath = reportPath
            End If

            ReportViewer1.LocalReport.DisplayName = $"Notificaciones_{Date.Now:yyyyMMdd_HHmm}"

            ' --- 2. Obtener los datos según el constructor utilizado ---
            Dim datosParaReporte As Object
            Dim idsParaBuscar As New List(Of Integer)

            If _notificacionIds IsNot Nothing AndAlso _notificacionIds.Any() Then
                ' Caso 1: Se recibió una lista de IDs para impresión masiva
                idsParaBuscar.AddRange(_notificacionIds)
            ElseIf _notificacionId > 0 Then
                ' Caso 2: Se recibió un único ID
                idsParaBuscar.Add(_notificacionId)
            Else
                Notifier.Warn(Me, "No se proporcionaron notificaciones para generar el reporte.")
                Close()
                Return
            End If

            ' (Asumimos que tu ReportesService tiene un método que acepta una colección de IDs)
            datosParaReporte = Await _reportesService.GetDatosParaNotificacionesAsync(idsParaBuscar)

            If datosParaReporte Is Nothing OrElse Not CType(datosParaReporte, IEnumerable(Of Object)).Any() Then
                Notifier.Info(Me, "No se encontraron datos para las notificaciones seleccionadas.")
                Close()
                Return
            End If

            ' --- 3. Cargar los datos y refrescar el visor ---
            Dim rds As New ReportDataSource("DataSetNotificaciones", datosParaReporte)
            ReportViewer1.LocalReport.DataSources.Add(rds)

            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 100
            ReportViewer1.RefreshReport()

        Catch
            Throw ' Relanza la excepción para que sea capturada en el evento Load
        Finally
            Me.Cursor = oldCursor
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region " Lógica para Confirmar Firma "

    Private Async Sub btnConfirmarFirma_Click(sender As Object, e As EventArgs) Handles btnConfirmarFirma.Click
        If _notificacionId <= 0 Then Return ' Seguridad extra

        If MessageBox.Show("¿Confirmás que la notificación fue firmada y deseás archivarla?",
                           "Confirmar Acción", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        Dim oldCursor = Me.Cursor
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
                notificacion.UpdatedAt = Date.Now ' Actualizamos la fecha de modificación

                Await uow.CommitAsync()
                NotificadorEventos.NotificarCambiosEnFuncionario(notificacion.FuncionarioId) ' Notificamos el cambio
                Notifier.Success(Me, "Notificación archivada correctamente.")
                Close()
            End Using
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al actualizar el estado: {ex.Message}")
        Finally
            LoadingHelper.OcultarCargando(Me)
            Me.Cursor = oldCursor
            btnConfirmarFirma.Enabled = True ' Habilitamos de nuevo en caso de error
        End Try
    End Sub

#End Region

End Class