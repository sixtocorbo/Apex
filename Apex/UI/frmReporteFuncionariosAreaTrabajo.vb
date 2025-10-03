Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteFuncionariosAreaTrabajo
    Inherits Form

    Private ReadOnly _funcionarioService As New FuncionarioService()
    Private chartAreaTrabajo As Chart

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartArea1")
        Dim legend As New Legend("Legend1")
        Dim title As New Title("Distribución por Área de Trabajo")

        chartAreaTrabajo = New Chart()
        SuspendLayout()

        chartArea.AxisX.Interval = 1
        chartArea.AxisX.MajorGrid.Enabled = False
        chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
        chartAreaTrabajo.ChartAreas.Add(chartArea)
        chartAreaTrabajo.Dock = DockStyle.Fill
        legend.Enabled = False
        chartAreaTrabajo.Legends.Add(legend)
        chartAreaTrabajo.Location = New Point(0, 0)
        chartAreaTrabajo.Name = "chartAreaTrabajo"
        chartAreaTrabajo.Palette = ChartColorPalette.EarthTones
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartAreaTrabajo.Titles.Add(title)
        chartAreaTrabajo.TabIndex = 0

        AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 600)
        Controls.Add(chartAreaTrabajo)
        MinimumSize = New Size(600, 400)
        Name = "frmReporteFuncionariosAreaTrabajo"
        StartPosition = FormStartPosition.CenterParent
        Text = "Distribución de Funcionarios por Área de Trabajo"

        ResumeLayout(False)
    End Sub

    Private Async Sub frmReporteFuncionariosAreaTrabajo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        Await CargarGraficoAsync()
    End Sub

    Private Async Function CargarGraficoAsync() As Task
        Dim datos = Await Task.Run(Function() _funcionarioService.GetDistribucionPorAreaTrabajo())

        If chartAreaTrabajo.Titles.Count > 0 Then
            chartAreaTrabajo.Titles(0).Text = "Distribución por Área de Trabajo"
        End If
        chartAreaTrabajo.Series.Clear()
        If datos Is Nothing OrElse Not datos.Any() Then
            If chartAreaTrabajo.Titles.Count > 0 Then
                chartAreaTrabajo.Titles(0).Text = "Distribución por Área de Trabajo - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Áreas") With {
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

        chartAreaTrabajo.Series.Add(series)
        chartAreaTrabajo.ChartAreas(0).AxisX.Interval = 1
    End Function
End Class
