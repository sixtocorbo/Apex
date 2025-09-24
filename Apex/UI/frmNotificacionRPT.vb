Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.Reporting.WinForms

''' <summary>
''' Formulario para visualizar e imprimir una o varias notificaciones.
''' </summary>
Public Class frmNotificacionRPT

#Region "Campos y Constructores"

    Private ReadOnly _reportesService As New ReportesService()
    Private ReadOnly _notificacionId As Integer
    Private ReadOnly _notificacionIds As List(Of Integer)

    ' Única notificación
    Public Sub New(notificacionId As Integer)
        InitializeComponent()
        _notificacionId = notificacionId
        btnConfirmarFirma.Visible = True
        Me.KeyPreview = True
    End Sub

    ' Impresión masiva
    Public Sub New(ids As List(Of Integer))
        InitializeComponent()
        _notificacionIds = ids
        btnConfirmarFirma.Visible = False
        Me.KeyPreview = True
    End Sub

#End Region

#Region "Ciclo de Vida"

    Private Async Sub frmNotificacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        Try
            Await CargarReporteAsync()
            Notifier.Success(Me, "Reporte listo para imprimir o guardar.")
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo cargar el reporte: {ex.Message}")
            Close()
        End Try
    End Sub

    Private Sub frmNotificacionRPT_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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

#Region "Carga del Reporte"

    Private Async Function CargarReporteAsync() As Task
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        LoadingHelper.MostrarCargando(Me)

        Try
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            ' RDLC: Embedded → BaseDirectory\Reportes → StartupPath\Reportes → ClickOnce → extra (..\..)
            ReportResourceLoader.LoadLocalReportDefinition(
                ReportViewer1.LocalReport,
                GetType(frmNotificacionRPT),
                "Apex.Reportes.NotificacionImprimir.rdlc",
                "NotificacionImprimir.rdlc",
                New String() {"..\..\Reportes\NotificacionImprimir.rdlc"}
            )

            ReportViewer1.LocalReport.DisplayName = $"Notificaciones_{Date.Now:yyyyMMdd_HHmm}"

            ' IDs según el constructor
            Dim idsParaBuscar As New List(Of Integer)
            If _notificacionIds IsNot Nothing AndAlso _notificacionIds.Any() Then
                idsParaBuscar.AddRange(_notificacionIds)
            ElseIf _notificacionId > 0 Then
                idsParaBuscar.Add(_notificacionId)
            Else
                Notifier.Warn(Me, "No se proporcionaron notificaciones para generar el reporte.")
                Close()
                Return
            End If

            ' Datos
            Dim datosParaReporte = Await _reportesService.GetDatosParaNotificacionesAsync(idsParaBuscar)
            If datosParaReporte Is Nothing OrElse Not CType(datosParaReporte, IEnumerable(Of Object)).Any() Then
                Notifier.Info(Me, "No se encontraron datos para las notificaciones seleccionadas.")
                Close()
                Return
            End If

            ' DataSource (nombre debe coincidir con el RDLC)
            Dim rds As New ReportDataSource("DataSetNotificaciones", datosParaReporte)
            ReportViewer1.LocalReport.DataSources.Add(rds)

            ' Presentación
            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 100

            ReportViewer1.RefreshReport()
            Await Task.Yield()

        Finally
            Me.Cursor = oldCursor
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Confirmar Firma"

    Private Async Sub btnConfirmarFirma_Click(sender As Object, e As EventArgs) Handles btnConfirmarFirma.Click
        If _notificacionId <= 0 Then Return

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
                notificacion.UpdatedAt = Date.Now

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
            btnConfirmarFirma.Enabled = True
        End Try
    End Sub

#End Region

End Class
