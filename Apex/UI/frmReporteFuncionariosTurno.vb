Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteFuncionariosTurno
    Inherits Form

    Private ReadOnly _funcionarioService As New FuncionarioService()
    Private ReadOnly chartTurno As Chart

    Public Sub New()
        chartTurno = New Chart()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartAreaTurno")
        Dim legend As New Legend("LegendTurno")
        Dim title As New Title("Funcionarios por Turno")

        SuspendLayout()

        chartArea.AxisX.Interval = 1
        chartArea.AxisX.MajorGrid.Enabled = False
        chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
        chartTurno.ChartAreas.Add(chartArea)
        chartTurno.Dock = DockStyle.Fill
        legend.Enabled = False
        chartTurno.Legends.Add(legend)
        chartTurno.Location = New Point(0, 0)
        chartTurno.Name = "chartTurno"
        chartTurno.Palette = ChartColorPalette.SeaGreen
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartTurno.Titles.Add(title)
        chartTurno.TabIndex = 0

        AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 600)
        Controls.Add(chartTurno)
        MinimumSize = New Size(600, 400)
        Name = "frmReporteFuncionariosTurno"
        StartPosition = FormStartPosition.CenterParent
        Text = "Distribución de Funcionarios por Turno"

        ResumeLayout(False)
    End Sub

    Private Async Sub frmReporteFuncionariosTurno_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        Await CargarGraficoAsync()
    End Sub

    Private Async Function CargarGraficoAsync() As Task
        Dim datos = Await Task.Run(Function() _funcionarioService.GetDistribucionPorTurno())

        If chartTurno.Titles.Count > 0 Then
            chartTurno.Titles(0).Text = "Funcionarios por Turno"
        End If
        chartTurno.Series.Clear()

        If datos Is Nothing OrElse Not datos.Any() Then
            If chartTurno.Titles.Count > 0 Then
                chartTurno.Titles(0).Text = "Funcionarios por Turno - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Turnos") With {
            .ChartType = SeriesChartType.Bar,
            .IsValueShownAsLabel = True
        }

        For Each item In datos
            Dim point As New DataPoint()
            point.SetValueY(item.Valor)
            point.AxisLabel = item.Etiqueta
            point.Label = item.Valor.ToString()
            series.Points.Add(point)
        Next

        chartTurno.Series.Add(series)
        chartTurno.ChartAreas(0).AxisX.Interval = 1
    End Function
End Class
