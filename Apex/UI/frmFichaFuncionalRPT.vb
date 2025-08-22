Imports Microsoft.Reporting.WinForms

Public Class frmFichaFuncionalRPT
    Private ReadOnly _funcionarioId As Integer

    Public Sub New(funcionarioId As Integer)
        InitializeComponent()
        _funcionarioId = funcionarioId
    End Sub

    Private Async Sub frmFichaFuncionalRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarReporteAsync()
    End Sub

    Private Async Function CargarReporteAsync() As Task
        Try
            Dim reporteService = New ReportesService()
            Dim datosFicha = Await reporteService.GetDatosFichaFuncionalAsync(_funcionarioId)

            If datosFicha Is Nothing Then
                MessageBox.Show("No se encontraron datos para el funcionario seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ReportViewer1.LocalReport.DataSources.Clear()

            ' --- INICIO DE LA MODIFICACIÓN ---
            Dim rdsFicha = New ReportDataSource("FichaFuncionalDataSet", New List(Of FichaFuncionalDTO) From {datosFicha})
            Dim rdsLicencias = New ReportDataSource("LicenciasDataSet", datosFicha.Licencias)
            Dim rdsSanciones = New ReportDataSource("SancionesDataSet", datosFicha.Sanciones)

            ReportViewer1.LocalReport.DataSources.Add(rdsFicha)
            ReportViewer1.LocalReport.DataSources.Add(rdsLicencias)
            ReportViewer1.LocalReport.DataSources.Add(rdsSanciones)
            ' --- FIN DE LA MODIFICACIÓN ---

            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al generar el reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function
End Class