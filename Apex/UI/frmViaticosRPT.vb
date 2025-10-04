Imports System.Data
Imports System.Globalization
Imports Microsoft.Reporting.WinForms

Public Class frmViaticosRPT

    Private ReadOnly _periodo As Date
    Private ReadOnly _datos As DataTable

    Public Sub New(periodo As Date, datos As DataTable)
        ' El constructor se mantiene igual
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

            ' --- INICIO DE LA CORRECCIÓN ---
            ' 1. Se elimina la llamada a 'ReportResourceLoader'.
            ' 2. Se establece el recurso incrustado directamente.
            '
            ' Este nombre DEBE coincidir con: <NamespaceRaízDelProyecto>.<Carpeta>.<NombreDelArchivo>.rdlc
            ' Asumiendo que tu Namespace es "Apex" y el archivo está en la carpeta "Reportes".
            ReportViewer1.LocalReport.ReportEmbeddedResource = "Apex.Reportes.ViaticosListado.rdlc"
            ' --- FIN DE LA CORRECCIÓN ---

            ' El resto de la configuración se mantiene igual
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
            MessageBox.Show($"No fue posible generar el informe: {ex.ToString()}", "Reporte de viáticos", MessageBoxButtons.OK, MessageBoxIcon.Error)
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