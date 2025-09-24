' En frmFuncionarioSituacionRPT.vb
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
    End Sub

    Private Async Sub frmFuncionarioSituacionRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarReporte()
    End Sub

    Private Async Function CargarReporte() As Task
        Dim previousCursor = Me.Cursor

        Try
            Me.Cursor = Cursors.WaitCursor

            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            ReportResourceLoader.LoadLocalReportDefinition(
                Me.ReportViewer1.LocalReport,
                GetType(frmFuncionarioSituacionRPT),
                "Apex.Reportes.SituacionFuncionario.rdlc",
                "SituacionFuncionario.rdlc",
                New String() {"..\..\Reportes\SituacionFuncionario.rdlc"})

            Using uow As New UnitOfWork()
                Using repo As New ReportesService(uow)
                    Dim datos = Await repo.ObtenerDatosSituacionAsync(_funcionarioId, _fechaDesde, _fechaHasta)
                    Dim funcionario = Await uow.Context.Set(Of Funcionario).FindAsync(_funcionarioId)

                    If funcionario Is Nothing Then
                        MessageBox.Show("No se encontró la información del funcionario solicitada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Me.Close()
                        Return
                    End If

                    If datos Is Nothing OrElse datos.Count = 0 Then
                        MessageBox.Show("No se registran eventos en el período indicado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Me.Close()
                        Return
                    End If

                    Dim rds As New ReportDataSource("DataSetSituacion", datos)
                    Me.ReportViewer1.LocalReport.DataSources.Add(rds)

                    Dim pNombre As New ReportParameter("FuncionarioNombre", funcionario.Nombre)
                    Dim pPeriodo As New ReportParameter("Periodo", $"Desde: {_fechaDesde:dd/MM/yyyy} Hasta: {_fechaHasta:dd/MM/yyyy}")
                    Me.ReportViewer1.LocalReport.SetParameters({pNombre, pPeriodo})
                End Using
            End Using

            Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            Me.ReportViewer1.ZoomMode = ZoomMode.Percent
            Me.ReportViewer1.ZoomPercent = 100
            Me.ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("Error al generar el reporte: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = previousCursor
        End Try
    End Function
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class
