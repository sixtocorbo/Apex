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

            ' --- CORRECCIÓN AQUÍ ---
            ' Indicar la ruta COMPLETA al reporte principal
            ReportViewer1.LocalReport.ReportEmbeddedResource = "Apex.Reportes.NovedadesDelDia.rdlc"
            ReportViewer1.LocalReport.DataSources.Clear()

            ' El ReportDataSource principal apunta a la lista de novedades
            Dim rds As New ReportDataSource("DataSetNovedades", _datosParaReporte)
            ReportViewer1.LocalReport.DataSources.Add(rds)

            ' Añadimos el manejador de evento que se activará por cada subreporte
            AddHandler ReportViewer1.LocalReport.SubreportProcessing, AddressOf LocalReport_SubreportProcessing

            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("Error al cargar el visor de reportes: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LocalReport_SubreportProcessing(sender As Object, e As SubreportProcessingEventArgs)
        Try
            Dim idNovedad As Integer = CInt(e.Parameters("NovedadId").Values(0))
            Dim novedadActual = CType(_datosParaReporte, IEnumerable(Of Object)).
                              FirstOrDefault(Function(n) CInt(n.GetType().GetProperty("ID").GetValue(n, Nothing)) = idNovedad)

            If novedadActual IsNot Nothing Then
                Dim funcionarios As Object = novedadActual.GetType().GetProperty("Funcionarios").GetValue(novedadActual, Nothing)
                Dim rdsFuncionarios As New ReportDataSource("DataSetFuncionarios", funcionarios)
                e.DataSources.Add(rdsFuncionarios)
            End If
        Catch ex As Exception
            MessageBox.Show("Error dentro de SubreportProcessing: " & ex.Message)
        End Try
    End Sub

End Class