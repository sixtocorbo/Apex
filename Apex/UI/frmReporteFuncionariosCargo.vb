Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteFuncionariosCargo
    Inherits Form

    Private ReadOnly _funcionarioService As New FuncionarioService()
    Private chartCargo As Chart

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartArea1")
        Dim legend As New Legend("Legend1")
        Dim title As New Title("Top 10 Cargos con más Personal")

        chartCargo = New Chart()
        SuspendLayout()

        chartArea.AxisX.Interval = 1
        chartArea.AxisX.MajorGrid.Enabled = False
        chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
        chartCargo.ChartAreas.Add(chartArea)
        chartCargo.Dock = DockStyle.Fill
        legend.Enabled = False
        chartCargo.Legends.Add(legend)
        chartCargo.Location = New Point(0, 0)
        chartCargo.Name = "chartCargo"
        chartCargo.Palette = ChartColorPalette.Berry
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartCargo.Titles.Add(title)
        chartCargo.TabIndex = 0

        AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 600)
        Controls.Add(chartCargo)
        MinimumSize = New Size(600, 400)
        Name = "frmReporteFuncionariosCargo"
        StartPosition = FormStartPosition.CenterParent
        Text = "Top de Cargos con más Funcionarios"

        ResumeLayout(False)
    End Sub

    Private Async Sub frmReporteFuncionariosCargo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        Await CargarGraficoAsync()
    End Sub

    Private Async Function CargarGraficoAsync() As Task
        Dim datos = Await Task.Run(Function() _funcionarioService.GetDistribucionPorCargo())

        If chartCargo.Titles.Count > 0 Then
            chartCargo.Titles(0).Text = "Top 10 Cargos con más Personal"
        End If
        chartCargo.Series.Clear()
        If datos Is Nothing OrElse Not datos.Any() Then
            If chartCargo.Titles.Count > 0 Then
                chartCargo.Titles(0).Text = "Top de Cargos - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Cargos") With {
            .ChartType = SeriesChartType.Bar,
            .IsValueShownAsLabel = True
        }

        For Each item In datos
            series.Points.AddXY(item.Etiqueta, item.Valor)
        Next

        chartCargo.Series.Add(series)
    End Function
End Class
