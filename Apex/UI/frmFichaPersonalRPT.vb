Imports Microsoft.Reporting.WinForms

Public Class frmFichaPersonalRPT

    Private ReadOnly _funcionarioId As Integer

    Public Sub New(funcionarioId As Integer)
        InitializeComponent()
        _funcionarioId = funcionarioId
        Me.KeyPreview = True
    End Sub

    Private Async Sub frmFichaFuncionalRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Await CargarReporteAsync()
            AppTheme.Aplicar(Me)
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al generar el reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Async Function CargarReporteAsync() As Task
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            Dim reporteService = New ReportesService()
            Dim datosFicha = Await reporteService.GetDatosFichaFuncionalAsync(_funcionarioId)

            If datosFicha Is Nothing Then
                MessageBox.Show("No se encontraron datos para el funcionario seleccionado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
                Return
            End If

            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            ' RDLC: recurso embebido con fallback a disco
            ReportResourceLoader.LoadLocalReportDefinition(
                ReportViewer1.LocalReport,
                GetType(frmFichaPersonalRPT),
                "Apex.Reportes.FichaFuncional.rdlc",
                "FichaFuncional.rdlc"
            )

            ' DataSource (debe coincidir con el nombre del DataSet en el RDLC)
            Dim rdsFicha As New ReportDataSource("FichaFuncionalDataSet",
                                                 New List(Of FichaFuncionalDTO) From {datosFicha})
            ReportViewer1.LocalReport.DataSources.Add(rdsFicha)

            ' Presentación
            ReportViewer1.LocalReport.DisplayName = $"FichaFuncional_{_funcionarioId:000000}"
            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 100

            ReportViewer1.RefreshReport()
            Await Task.Yield()

        Finally
            Me.Cursor = oldCursor
        End Try
    End Function

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub

End Class

'' Apex/UI/frmFichaFuncionalRPT.vb
'Imports Microsoft.Reporting.WinForms

'Public Class frmFichaPersonalRPT

'    Private ReadOnly _funcionarioId As Integer

'    Public Sub New(funcionarioId As Integer)
'        InitializeComponent()
'        _funcionarioId = funcionarioId
'    End Sub

'    Private Async Sub frmFichaFuncionalRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        Await CargarReporteAsync()
'        AppTheme.Aplicar(Me)
'    End Sub

'    Private Async Function CargarReporteAsync() As Task
'        Try
'            Dim reporteService = New ReportesService()
'            Dim datosFicha = Await reporteService.GetDatosFichaFuncionalAsync(_funcionarioId)

'            If datosFicha Is Nothing Then
'                MessageBox.Show("No se encontraron datos para el funcionario seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'                Return
'            End If

'            ReportViewer1.LocalReport.DataSources.Clear()

'            ' --- INICIO DE LA CORRECCIÓN ---
'            ' Se eliminan las referencias a los DataSets de licencias y sanciones que ya no existen en el reporte.
'            Dim rdsFicha = New ReportDataSource("FichaFuncionalDataSet", New List(Of FichaFuncionalDTO) From {datosFicha})
'            ReportViewer1.LocalReport.DataSources.Add(rdsFicha)
'            ' --- FIN DE LA CORRECCIÓN ---

'            ReportViewer1.RefreshReport()

'        Catch ex As Exception
'            MessageBox.Show("Ocurrió un error al generar el reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Function
'    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
'        ' Si la tecla presionada es Escape, se cierra el formulario.
'        If e.KeyCode = Keys.Escape Then
'            Me.Close()
'        End If
'    End Sub
'End Class