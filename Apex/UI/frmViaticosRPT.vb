Imports System.Data
Imports System.Globalization
Imports Microsoft.Reporting.WinForms
' No necesitamos los imports de IO y Reflection si usamos el Loader
' Imports System.IO
' Imports System.Reflection

Public Class frmViaticosRPT

    Private _periodo As Date
    Private _datos As DataTable
    Private _estaPreparado As Boolean

    Public Sub New()
        InitializeComponent()
        Me.KeyPreview = True
    End Sub

    Public Sub New(periodo As Date, datos As DataTable)
        Me.New()
        Preparar(periodo, datos)
    End Sub

    Public Sub Preparar(periodo As Date, datos As DataTable)
        If datos Is Nothing Then Throw New ArgumentNullException(NameOf(datos))

        _periodo = periodo
        _datos = datos
        _estaPreparado = True
    End Sub

    Private Sub frmViaticosRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not _estaPreparado OrElse _datos Is Nothing Then
            Notifier.Warn(Me, "No se proporcionaron datos para el reporte.")
            Me.Close()
            Return
        End If

        ConfigurarReporte()
        AppTheme.Aplicar(Me)
    End Sub

    Private Sub ConfigurarReporte()
        If Not _estaPreparado OrElse _datos Is Nothing Then
            Throw New InvalidOperationException("El reporte no fue configurado correctamente antes de mostrarse.")
        End If

        Dim cursorAnterior = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            ReportViewer1.Reset()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            Dim nombreRecursoCorrecto As String = "Apex.Reportes.ViaticosListado.rdlc"

            Dim definicion = ReportResourceLoader.LoadLocalReportDefinition(
                ReportViewer1.LocalReport,
                Me.GetType(),
                nombreRecursoCorrecto,      ' <-- ¡ESTA ES LA CORRECCIÓN!
                "ViaticosListado.rdlc")

            ' Si por alguna razón el código llega aquí, es porque encontró el reporte.
            ' El resto de la configuración se mantiene.
            Select Case definicion.Source
                Case ReportDefinitionSource.Embedded
                    ReportViewer1.LocalReport.ReportEmbeddedResource = definicion.ResourceName
                Case ReportDefinitionSource.File
                    ReportViewer1.LocalReport.ReportPath = definicion.FilePath
            End Select
            ' --- FIN DEL CÓDIGO DE DIAGNÓSTICO ---


            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSetViaticos", _datos))

            Dim cultura = CultureInfo.GetCultureInfo("es-UY")
            Dim periodoTexto = cultura.TextInfo.ToTitleCase(_periodo.ToString("MMMM yyyy", cultura))

            Dim parametros = New ReportParameter() {
                New ReportParameter("Titulo", "Listado de viáticos"),
                New ReportParameter("Periodo", periodoTexto),
                New ReportParameter("TotalRegistros", _datos.Rows.Count.ToString(cultura))
            }

            ReportViewer1.LocalReport.SetParameters(parametros)
            ReportViewer1.LocalReport.DisplayName = $"Viaticos_{_periodo:yyyyMM}"

            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.PageWidth
            ReportViewer1.RefreshReport()

        Catch ex As Exception
            ' ¡ESTA ES LA PARTE IMPORTANTE!
            ' El mensaje de error ahora contendrá la lista que necesitamos.
            MessageBox.Show($"Diagnóstico del Reporte:" & vbCrLf & vbCrLf & ex.ToString(), "Error Cargando Reporte", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = cursorAnterior
        End Try
    End Sub

    Private Sub frmViaticosRPT_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub

End Class
