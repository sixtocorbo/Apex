Imports System.Data
Imports System.Globalization
Imports Microsoft.Reporting.WinForms
' No necesitamos los imports de IO y Reflection si usamos el Loader
' Imports System.IO
' Imports System.Reflection

Public Class frmViaticosRPT

    Private ReadOnly _periodo As Date
    Private ReadOnly _datos As DataTable

    Public Sub New(periodo As Date, datos As DataTable)
        If datos Is Nothing Then Throw New ArgumentNullException(NameOf(datos))
        InitializeComponent()
        _periodo = periodo
        _datos = datos
        Me.KeyPreview = True
    End Sub

    Private Sub frmViaticosRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarReporte()
        AppTheme.Aplicar(Me)
    End Sub

    Private Sub ConfigurarReporte()
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
