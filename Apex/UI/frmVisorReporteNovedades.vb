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
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.EnableExternalImages = True
            ReportViewer1.LocalReport.DataSources.Clear()

            ' 1) Cargar reporte principal desde el recurso incrustado
            Dim asm = GetType(frmVisorReporteNovedades).Assembly
            Using mainStream = asm.GetManifestResourceStream("Apex.Reportes.NovedadesDelDia.rdlc")
                If mainStream Is Nothing Then
                    Throw New ApplicationException("No se encuentra el recurso 'Apex.Reportes.NovedadesDelDia.rdlc'. " &
                                               "Verifica Build Action = Embedded Resource y el namespace.")
                End If
                ReportViewer1.LocalReport.LoadReportDefinition(mainStream)
            End Using

            ' 2) Registrar el SUBREPORTE (el nombre lógico debe coincidir con ReportName del control Subreport)
            Using subStream = asm.GetManifestResourceStream("Apex.Reportes.NovedadesDelDia_Funcionarios.rdlc")
                If subStream Is Nothing Then
                    Throw New ApplicationException("Falta el subreporte 'Apex.Reportes.NovedadesDelDia_Funcionarios.rdlc'.")
                End If
                ReportViewer1.LocalReport.LoadSubreportDefinition("NovedadesDelDia_Funcionarios", subStream)
            End Using

            ' 3) DataSet principal
            Dim rds As New ReportDataSource("DataSetNovedades", CType(_datosParaReporte, IEnumerable(Of Object)))
            ReportViewer1.LocalReport.DataSources.Add(rds)

            ' 4) Handler para inyectar el datasource del subreporte
            AddHandler ReportViewer1.LocalReport.SubreportProcessing,
            Sub(s, args)
                Dim idNovedadParam = args.Parameters("NovedadId")
                If idNovedadParam Is Nothing OrElse idNovedadParam.Values.Count = 0 Then
                    args.DataSources.Add(New ReportDataSource("DataSetFuncionarios", New List(Of Object)))
                    Return
                End If

                Dim idNovedad As Integer = CInt(idNovedadParam.Values(0))
                Dim lista = CType(_datosParaReporte, IEnumerable(Of Object))
                Dim nov = lista.FirstOrDefault(Function(n) CInt(n.GetType().GetProperty("ID").GetValue(n)) = idNovedad)

                Dim funcionarios As Object = Nothing
                If nov IsNot Nothing Then
                    Dim p = nov.GetType().GetProperty("Funcionarios")
                    If p IsNot Nothing Then funcionarios = p.GetValue(nov)
                End If

                If funcionarios Is Nothing Then funcionarios = New List(Of Object)
                args.DataSources.Add(New ReportDataSource("DataSetFuncionarios", funcionarios))
            End Sub

            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("Error al cargar el visor de reportes: " & ex.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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