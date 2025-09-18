Imports System.IO
Imports Microsoft.Reporting.WinForms

Public Class frmNovedadesRPT

    Private ReadOnly _items As List(Of NovedadReporteDTO)

    Public Sub New(items As List(Of NovedadReporteDTO))
        InitializeComponent()
        _items = If(items, New List(Of NovedadReporteDTO)())
    End Sub

    Private Sub frmNovedadesRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        CargarReporte()
    End Sub

    Private Sub CargarReporte()
        Try
            ' --- Rutas verificadas ---
            Dim basePath As String = Path.Combine(Application.StartupPath, "Reportes")
            Dim mainReportPath As String = Path.Combine(basePath, "NovedadDetallada.rdlc")
            Dim funcSubreportPath As String = Path.Combine(basePath, "ReporteNovedades_Funcionarios.rdlc")
            Dim fotoSubreportPath As String = Path.Combine(basePath, "ReporteNovedades_Fotos.rdlc")

            If Not File.Exists(mainReportPath) Then Throw New FileNotFoundException("Falta el reporte principal.", mainReportPath)
            If Not File.Exists(funcSubreportPath) Then Throw New FileNotFoundException("Falta el subreporte de funcionarios.", funcSubreportPath)
            If Not File.Exists(fotoSubreportPath) Then Throw New FileNotFoundException("Falta el subreporte de fotos.", fotoSubreportPath)

            ' --- ReportViewer base ---
            ReportViewer1.Reset()
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()
            ReportViewer1.LocalReport.EnableExternalImages = False ' Las fotos vienen como byte[], no externas.
            ReportViewer1.LocalReport.ReportPath = mainReportPath

            ' Dataset principal (el RDLC debe definir DataSetNovedades con campos: Id, Fecha, Texto, etc.)
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DataSetNovedades", _items))

            ' Evitar duplicar handler si el form se reusa
            RemoveHandler ReportViewer1.LocalReport.SubreportProcessing, AddressOf SubreportProcessingHandler
            AddHandler ReportViewer1.LocalReport.SubreportProcessing, AddressOf SubreportProcessingHandler

            ' UI del visor
            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.Percent
            ReportViewer1.ZoomPercent = 100

            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("No se pudo cargar el reporte: " & ex.Message, "Reporte", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SubreportProcessingHandler(sender As Object, e As SubreportProcessingEventArgs)
        Try
            ' En cada subreporte el RDLC debe pasar un parámetro "NovedadId" con =Fields!Id.Value
            Dim idNovedad As Integer = CInt(e.Parameters("NovedadId").Values(0))
            Dim novedad = _items.FirstOrDefault(Function(n) n.Id = idNovedad)
            If novedad Is Nothing Then
                ' Si no hay match, devolver dataset vacío para evitar error de RDLC
                If e.ReportPath = "ReporteNovedades_Funcionarios" Then
                    e.DataSources.Add(New ReportDataSource("DataSetFuncionarios", New List(Of FuncionarioReporteDTO)))
                ElseIf e.ReportPath = "ReporteNovedades_Fotos" Then
                    e.DataSources.Add(New ReportDataSource("DataSetFotos", New List(Of FotoReporteDTO)))
                End If
                Return
            End If

            Select Case e.ReportPath
                Case "ReporteNovedades_Funcionarios"
                    ' El subreporte debe tener un DataSet llamado DataSetFuncionarios con campos: Id, Nombre (o los que uses)
                    e.DataSources.Add(New ReportDataSource("DataSetFuncionarios", novedad.Funcionarios))

                Case "ReporteNovedades_Fotos"
                    ' El subreporte debe tener un DataSet "DataSetFotos" con campos: Foto (byte[]), FileName
                    ' En el RDLC, el Image control: Source=Database, Value=Fields!Foto.Value, MIMEType (por ejemplo "image/jpeg")
                    e.DataSources.Add(New ReportDataSource("DataSetFotos", novedad.Fotos))
            End Select

        Catch ex As Exception
            MessageBox.Show("Error al procesar subreporte: " & ex.Message, "Subreporte", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmNovedadesRPT_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub
End Class

'Imports Microsoft.Reporting.WinForms
'Imports System.IO
'Imports System.Linq

'Public Class frmNovedadesRPT

'    Private ReadOnly _datosParaReporte As Object

'    Public Sub New(datos As Object)
'        InitializeComponent()
'        _datosParaReporte = datos
'    End Sub

'    Private Sub frmVisorReporteNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        AppTheme.Aplicar(Me)
'        CargarReporte()
'    End Sub

'    Private Sub CargarReporte()
'        Try
'            ' --- INICIO DE LA MEJORA ---
'            ' Verificamos TODOS los archivos necesarios ANTES de empezar.

'            Dim basePath As String = Path.Combine(Application.StartupPath, "Reportes")
'            Dim mainReportPath As String = Path.Combine(basePath, "NovedadDetallada.rdlc")
'            Dim funcSubreportPath As String = Path.Combine(basePath, "ReporteNovedades_Funcionarios.rdlc")
'            Dim fotoSubreportPath As String = Path.Combine(basePath, "ReporteNovedades_Fotos.rdlc")

'            If Not File.Exists(mainReportPath) Then
'                Throw New FileNotFoundException("No se encontró el archivo del reporte principal.", mainReportPath)
'            End If
'            If Not File.Exists(funcSubreportPath) Then
'                Throw New FileNotFoundException("No se encontró el subreporte de funcionarios. Asegúrate de que su propiedad 'Copiar en el directorio de salida' esté configurada como 'Copiar si es más reciente'.", funcSubreportPath)
'            End If
'            If Not File.Exists(fotoSubreportPath) Then
'                Throw New FileNotFoundException("No se encontró el subreporte de fotos. Asegúrate de que su propiedad 'Copiar en el directorio de salida' esté configurada como 'Copiar si es más reciente'.", fotoSubreportPath)
'            End If
'            ' --- FIN DE LA MEJORA ---

'            Me.ReportViewer1.LocalReport.DataSources.Clear()
'            Me.ReportViewer1.LocalReport.EnableExternalImages = True

'            ' Usamos la ruta ya verificada
'            Me.ReportViewer1.LocalReport.ReportPath = mainReportPath

'            Dim rdsNovedades As New ReportDataSource("DataSetNovedades", _datosParaReporte)
'            Me.ReportViewer1.LocalReport.DataSources.Add(rdsNovedades)

'            AddHandler Me.ReportViewer1.LocalReport.SubreportProcessing, AddressOf SubreportProcessingHandler

'            Me.ReportViewer1.RefreshReport()

'        Catch ex As Exception
'            ' Ahora, cualquier error de archivo faltante será capturado aquí y mostrado claramente.
'            MessageBox.Show("Ocurrió un error al generar el reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

'            ' Opcional: Cierra el formulario si el reporte no se puede cargar.
'            ' Me.Close()
'        End Try
'    End Sub

'    Private Sub SubreportProcessingHandler(sender As Object, e As SubreportProcessingEventArgs)
'        Try
'            Dim idNovedadActual As Integer = CInt(e.Parameters("NovedadId").Values(0))
'            Dim listaCompletaDTO = CType(_datosParaReporte, List(Of NovedadReporteDTO))
'            Dim novedadEncontrada = listaCompletaDTO.FirstOrDefault(Function(n) n.Id = idNovedadActual)

'            If novedadEncontrada IsNot Nothing Then
'                ' Diferenciamos qué subreporte nos está pidiendo datos
'                If e.ReportPath = "ReporteNovedades_Funcionarios" Then
'                    e.DataSources.Add(New ReportDataSource("DataSetFuncionarios", novedadEncontrada.Funcionarios))
'                ElseIf e.ReportPath = "ReporteNovedades_Fotos" Then
'                    e.DataSources.Add(New ReportDataSource("DataSetFotos", novedadEncontrada.Fotos))
'                End If
'            End If
'        Catch ex As Exception
'            MessageBox.Show("Error al procesar subreporte: " & ex.Message, "Error de Subreporte", MessageBoxButtons.OK, MessageBoxIcon.Error)
'        End Try
'    End Sub


'    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
'        ' Si la tecla presionada es Escape, se cierra el formulario.
'        If e.KeyCode = Keys.Escape Then
'            Me.Close()
'        End If
'    End Sub
'End Class