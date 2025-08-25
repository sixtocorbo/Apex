Imports Microsoft.Reporting.WinForms
Imports System.Linq

Public Class frmVisorReporteNovedades

    Private ReadOnly _datosParaReporte As Object

    Public Sub New(datos As Object)
        InitializeComponent()
        _datosParaReporte = datos
    End Sub

    Private Sub frmVisorReporteNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Habilitar referencias externas para que el subreporte pueda funcionar correctamente
            ReportViewer1.LocalReport.EnableExternalImages = True

            ' Indicar la ruta al reporte principal
            ReportViewer1.LocalReport.ReportEmbeddedResource = "Apex.NovedadesDelDia.rdlc"
            ReportViewer1.LocalReport.DataSources.Clear()

            ' El ReportDataSource principal apunta a la lista de novedades
            Dim rds As New ReportDataSource("DataSetNovedades", _datosParaReporte)
            ReportViewer1.LocalReport.DataSources.Add(rds)

            ' >>> ESTA PARTE ES LA CLAVE <<<
            ' Añadimos el manejador de evento que se activará por cada subreporte
            AddHandler ReportViewer1.LocalReport.SubreportProcessing, AddressOf LocalReport_SubreportProcessing

            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("Error al cargar el visor de reportes: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Este evento se dispara cada vez que el reporte principal intenta cargar un subreporte.
    ''' Su trabajo es "interceptar" esa llamada y darle los datos que necesita.
    ''' </summary>
    Private Sub LocalReport_SubreportProcessing(sender As Object, e As SubreportProcessingEventArgs)
        Try
            ' 1. Obtenemos el ID de la novedad que el reporte principal nos está pasando como parámetro
            Dim idNovedad As Integer = CInt(e.Parameters("NovedadId").Values(0))

            ' 2. Buscamos la novedad correspondiente en nuestra lista completa de datos
            Dim novedadActual = CType(_datosParaReporte, IEnumerable(Of Object)).
                              FirstOrDefault(Function(n) CInt(n.GetType().GetProperty("ID").GetValue(n, Nothing)) = idNovedad)

            If novedadActual IsNot Nothing Then
                ' 3. Obtenemos la lista de funcionarios específica de esa novedad
                Dim funcionarios As Object = novedadActual.GetType().GetProperty("Funcionarios").GetValue(novedadActual, Nothing)

                ' 4. Creamos un nuevo origen de datos para el subreporte y se lo asignamos
                Dim rdsFuncionarios As New ReportDataSource("DataSetFuncionarios", funcionarios)
                e.DataSources.Add(rdsFuncionarios)
            End If
        Catch ex As Exception
            MessageBox.Show("Error dentro de SubreportProcessing: " & ex.Message)
        End Try
    End Sub

End Class