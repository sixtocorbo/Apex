Imports Microsoft.Reporting.WinForms

Public Class frmFuncionarioSituacionRPT

    Private ReadOnly _funcionarioId As Integer
    Private ReadOnly _fechaDesde As Date
    Private ReadOnly _fechaHasta As Date

    Public Sub New(funcionarioId As Integer, fechaDesde As Date, fechaHasta As Date)
        InitializeComponent()
        _funcionarioId = funcionarioId
        _fechaDesde = fechaDesde
        _fechaHasta = fechaHasta
        Me.KeyPreview = True
    End Sub

    Private Async Sub frmFuncionarioSituacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Await CargarReporte()
            ' (opcional) si usás AppTheme:
            ' AppTheme.Aplicar(Me)
        Catch ex As Exception
            MessageBox.Show("Error al generar el reporte: " & ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Async Function CargarReporte() As Task
        Dim old = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            Using uow As New UnitOfWork()
                Dim repo As New ReportesService(uow)
                Dim datos = Await repo.ObtenerDatosSituacionAsync(_funcionarioId, _fechaDesde, _fechaHasta)
                Dim funcionario = Await uow.Context.Set(Of Funcionario).FindAsync(_funcionarioId)

                ReportViewer1.ProcessingMode = ProcessingMode.Local
                ReportViewer1.LocalReport.DataSources.Clear()

                ' 1) Cargar SI o SI el RDLC (embedded + fallbacks)
                ReportResourceLoader.LoadLocalReportDefinition(
                ReportViewer1.LocalReport,
                GetType(frmFuncionarioSituacionRPT),
                "Apex.Reportes.SituacionFuncionario.rdlc",   ' <-- revisá que el Build Action del .rdlc sea Embedded Resource con este nombre
                "SituacionFuncionario.rdlc",
                New String() {"..\..\Reportes\SituacionFuncionario.rdlc"} ' <-- fallback extra para debug
            )

                ' (opcional) assert útil para diagnosticar si el RDLC no cargó:
                If String.IsNullOrEmpty(ReportViewer1.LocalReport.ReportPath) _
               AndAlso ReportViewer1.LocalReport.ListRenderingExtensions() Is Nothing Then
                    Throw New ApplicationException("El RDLC no se cargó. Verificá el nombre del recurso embebido y los fallbacks.")
                End If

                ' 2) DataSource (nombre EXACTO como en el RDLC)
                Dim rds As New ReportDataSource("DataSetSituacion", datos)
                ReportViewer1.LocalReport.DataSources.Add(rds)

                ' 3) Parámetros (evitar Nothing)
                Dim nombreFuncionario As String = If(funcionario?.Nombre, "N/A")
                Dim periodo As String = $"Desde: {_fechaDesde:dd/MM/yyyy} Hasta: {_fechaHasta:dd/MM/yyyy}"

                Dim pNombre As New ReportParameter("FuncionarioNombre", nombreFuncionario)
                Dim pPeriodo As New ReportParameter("Periodo", periodo)
                ReportViewer1.LocalReport.SetParameters({pNombre, pPeriodo})

                ' 4) Presentación
                ReportViewer1.LocalReport.DisplayName = $"Situacion_{_funcionarioId:000000}"
                ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
                ReportViewer1.ZoomMode = ZoomMode.Percent
                ReportViewer1.ZoomPercent = 100

                ReportViewer1.RefreshReport()
                Await Task.Yield()
            End Using

        Catch ex As Microsoft.Reporting.WinForms.LocalProcessingException
            Dim msg = ex.Message
            If ex.InnerException IsNot Nothing Then msg &= Environment.NewLine & ex.InnerException.Message
            MessageBox.Show("Error al generar el reporte: " & msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        Finally
            Me.Cursor = old
        End Try
    End Function


    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub
End Class
