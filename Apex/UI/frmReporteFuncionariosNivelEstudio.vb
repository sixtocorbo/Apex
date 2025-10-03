Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteFuncionariosNivelEstudio
    Inherits Form

    Private ReadOnly _funcionarioService As New FuncionarioService()
    Private ReadOnly chartNivel As Chart

    Public Sub New()
        chartNivel = New Chart()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartAreaNivel")
        Dim legend As New Legend("LegendNivel")
        Dim title As New Title("Funcionarios por Nivel de Estudios")

        SuspendLayout()

        chartArea.AxisX.Interval = 1
        chartArea.AxisX.MajorGrid.Enabled = False
        chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
        chartNivel.ChartAreas.Add(chartArea)
        chartNivel.Dock = DockStyle.Fill
        legend.Enabled = False
        chartNivel.Legends.Add(legend)
        chartNivel.Location = New Point(0, 0)
        chartNivel.Name = "chartNivel"
        chartNivel.Palette = ChartColorPalette.Berry
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartNivel.Titles.Add(title)
        chartNivel.TabIndex = 0

        AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 600)
        Controls.Add(chartNivel)
        MinimumSize = New Size(600, 400)
        Name = "frmReporteFuncionariosNivelEstudio"
        StartPosition = FormStartPosition.CenterParent
        Text = "Distribución de Funcionarios por Nivel de Estudios"

        ResumeLayout(False)
    End Sub

    Private Async Sub frmReporteFuncionariosNivelEstudio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        Await CargarGraficoAsync()
    End Sub

    Private Async Function CargarGraficoAsync() As Task
        Dim datos = Await Task.Run(Function() _funcionarioService.GetDistribucionPorNivelEstudio())

        If chartNivel.Titles.Count > 0 Then
            chartNivel.Titles(0).Text = "Funcionarios por Nivel de Estudios"
        End If
        chartNivel.Series.Clear()

        If datos Is Nothing OrElse Not datos.Any() Then
            If chartNivel.Titles.Count > 0 Then
                chartNivel.Titles(0).Text = "Funcionarios por Nivel de Estudios - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Nivel de estudios") With {
            .ChartType = SeriesChartType.Column,
            .IsValueShownAsLabel = True
        }

        For Each item In datos
            Dim point As New DataPoint()
            point.SetValueY(item.Valor)
            point.AxisLabel = item.Etiqueta
            point.Label = item.Valor.ToString()
            series.Points.Add(point)
        Next

        chartNivel.Series.Add(series)
        chartNivel.ChartAreas(0).AxisX.Interval = 1
    End Function
End Class
