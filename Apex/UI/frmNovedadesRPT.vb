Imports Microsoft.Reporting.WinForms

Public Class frmNovedadesRPT

    Private ReadOnly _items As List(Of NovedadReporteDTO)

    Public Sub New(items As List(Of NovedadReporteDTO))
        InitializeComponent()
        _items = If(items, New List(Of NovedadReporteDTO)())
        Me.KeyPreview = True
    End Sub

    Private Sub frmNovedadesRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try : AppTheme.Aplicar(Me) : Catch : End Try
        CargarReporte()
    End Sub

    Private Sub frmNovedadesRPT_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub

    Private Sub CargarReporte()
        Dim old = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            ReportViewer1.Reset()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.LocalReport.EnableExternalImages = False ' Las fotos son byte[] en DataSet

            ' --- RDLC principal (embedded + fallbacks) ---
            ReportResourceLoader.LoadLocalReportDefinition(
                ReportViewer1.LocalReport,
                GetType(frmNovedadesRPT),
                "Apex.Reportes.NovedadDetallada.rdlc",
                "NovedadDetallada.rdlc",
                New String() {"..\..\Reportes\NovedadDetallada.rdlc"}
            )

            ' --- Subreportes (nombres deben coincidir con el ReportPath del subreporte en el RDLC principal) ---
            ReportResourceLoader.LoadSubreportDefinition(
                ReportViewer1.LocalReport,
                "ReporteNovedades_Funcionarios",
                GetType(frmNovedadesRPT),
                "Apex.Reportes.ReporteNovedades_Funcionarios.rdlc",
                "ReporteNovedades_Funcionarios.rdlc",
                New String() {"..\..\Reportes\ReporteNovedades_Funcionarios.rdlc"}
            )

            ReportResourceLoader.LoadSubreportDefinition(
                ReportViewer1.LocalReport,
                "ReporteNovedades_Fotos",
                GetType(frmNovedadesRPT),
                "Apex.Reportes.ReporteNovedades_Fotos.rdlc",
                "ReporteNovedades_Fotos.rdlc",
                New String() {"..\..\Reportes\ReporteNovedades_Fotos.rdlc"}
            )

            ' DataSet principal (debe llamarse igual en el RDLC)
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSetNovedades", _items))

            ' Subreport handler (evitar duplicaciones)
            RemoveHandler ReportViewer1.LocalReport.SubreportProcessing, AddressOf SubreportProcessingHandler
            AddHandler ReportViewer1.LocalReport.SubreportProcessing, AddressOf SubreportProcessingHandler

            ' UI visor
            ReportViewer1.LocalReport.DisplayName = $"Novedades_{Date.Now:yyyyMMdd_HHmm}"
            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 100

            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("No se pudo cargar el reporte: " & ex.Message, "Reporte", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = old
        End Try
    End Sub

    Private Sub SubreportProcessingHandler(sender As Object, e As SubreportProcessingEventArgs)
        Try
            ' En cada subreporte el RDLC debe pasar un parámetro "NovedadId" (=Fields!Id.Value)
            Dim idNovedad As Integer = CInt(e.Parameters("NovedadId").Values(0))
            Dim novedad = _items.FirstOrDefault(Function(n) n.Id = idNovedad)

            If novedad Is Nothing Then
                ' Retornar datasets vacíos para no romper el render
                If e.ReportPath = "ReporteNovedades_Funcionarios" Then
                    e.DataSources.Add(New ReportDataSource("DataSetFuncionarios", New List(Of FuncionarioReporteDTO)))
                ElseIf e.ReportPath = "ReporteNovedades_Fotos" Then
                    e.DataSources.Add(New ReportDataSource("DataSetFotos", New List(Of FotoReporteDTO)))
                End If
                Return
            End If

            Select Case e.ReportPath
                Case "ReporteNovedades_Funcionarios"
                    ' DataSet del subreporte debe llamarse "DataSetFuncionarios"
                    e.DataSources.Add(New ReportDataSource("DataSetFuncionarios", novedad.Funcionarios))

                Case "ReporteNovedades_Fotos"
                    ' DataSet del subreporte debe llamarse "DataSetFotos"
                    ' En el RDLC, Image: Source=Database, Value=Fields!Foto.Value, MIMEType acorde
                    e.DataSources.Add(New ReportDataSource("DataSetFotos", novedad.Fotos))
            End Select

        Catch ex As Exception
            MessageBox.Show("Error al procesar subreporte: " & ex.Message, "Subreporte", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
