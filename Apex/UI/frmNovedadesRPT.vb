Imports Microsoft.Reporting.WinForms
Imports System.IO
Imports System.Linq

Public Class frmNovedadesRPT

    Private ReadOnly _datosParaReporte As Object

    Public Sub New(datos As Object)
        InitializeComponent()
        _datosParaReporte = datos
    End Sub

    Private Sub frmVisorReporteNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarReporte()
    End Sub

    Private Sub CargarReporte()
        Try
            Me.ReportViewer1.LocalReport.DataSources.Clear()

            ' --- INICIO DE LA SOLUCIÓN ---
            ' 1. Habilitamos la carga de recursos externos (esto es clave para los subreportes).
            Me.ReportViewer1.LocalReport.EnableExternalImages = True

            ' 2. Especificamos la ruta exacta del archivo .rdlc principal.
            Dim reportPath As String = Path.Combine(Application.StartupPath, "Reportes", "NovedadDetallada.rdlc")

            If Not File.Exists(reportPath) Then
                Throw New FileNotFoundException("No se pudo encontrar el archivo del reporte principal. Asegúrate de que su propiedad 'Copiar en el directorio de salida' esté configurada como 'Copiar si es más reciente'.", reportPath)
            End If

            Me.ReportViewer1.LocalReport.ReportPath = reportPath
            ' --- FIN DE LA SOLUCIÓN ---

            ' 3. Asignamos el origen de datos principal.
            Dim rdsNovedades As New ReportDataSource("DataSetNovedades", _datosParaReporte)
            Me.ReportViewer1.LocalReport.DataSources.Add(rdsNovedades)

            ' 4. Asignamos el manejador para los datos del subreporte.
            AddHandler Me.ReportViewer1.LocalReport.SubreportProcessing, AddressOf SubreportProcessingHandler

            Me.ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al generar el reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SubreportProcessingHandler(sender As Object, e As SubreportProcessingEventArgs)
        Try
            Dim idNovedadActual As Integer = CInt(e.Parameters("NovedadId").Values(0))
            Dim listaCompleta = CType(_datosParaReporte, System.Collections.Generic.IEnumerable(Of Object))
            Dim novedadEncontrada = listaCompleta.FirstOrDefault(Function(n) CInt(n.GetType().GetProperty("Id").GetValue(n)) = idNovedadActual)

            If novedadEncontrada IsNot Nothing Then
                Dim funcionariosDeLaNovedad = novedadEncontrada.GetType().GetProperty("Funcionarios").GetValue(novedadEncontrada, Nothing)
                e.DataSources.Add(New ReportDataSource("DataSetFuncionarios", funcionariosDeLaNovedad))
            End If
        Catch ex As Exception
            MessageBox.Show("Error al procesar la lista de funcionarios: " & ex.Message)
        End Try
    End Sub

End Class