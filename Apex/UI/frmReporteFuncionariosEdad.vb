Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteFuncionariosEdad
    Inherits Form

    Private ReadOnly _funcionarioService As New FuncionarioService()
    Private chartEdad As Chart

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartArea1")
        Dim legend As New Legend("Legend1")
        Dim title As New Title("Distribución por Rango de Edad")

        chartEdad = New Chart()
        SuspendLayout()

        chartArea.AxisX.Interval = 1
        chartArea.AxisX.MajorGrid.Enabled = False
        chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
        chartEdad.ChartAreas.Add(chartArea)
        chartEdad.Dock = DockStyle.Fill
        legend.Enabled = False
        chartEdad.Legends.Add(legend)
        chartEdad.Location = New Point(0, 0)
        chartEdad.Name = "chartEdad"
        chartEdad.Palette = ChartColorPalette.SeaGreen
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartEdad.Titles.Add(title)
        chartEdad.TabIndex = 0

        AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 600)
        Controls.Add(chartEdad)
        MinimumSize = New Size(600, 400)
        Name = "frmReporteFuncionariosEdad"
        StartPosition = FormStartPosition.CenterParent
        Text = "Distribución de Funcionarios por Rango de Edad"

        ResumeLayout(False)
    End Sub

    Private Async Sub frmReporteFuncionariosEdad_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        Await CargarGraficoAsync()
    End Sub

    Private Async Function CargarGraficoAsync() As Task
        Dim datos = Await Task.Run(Function() _funcionarioService.GetDistribucionPorRangoEdad())

        If chartEdad.Titles.Count > 0 Then
            chartEdad.Titles(0).Text = "Distribución por Rango de Edad"
        End If
        chartEdad.Series.Clear()
        If datos Is Nothing OrElse Not datos.Any() Then
            If chartEdad.Titles.Count > 0 Then
                chartEdad.Titles(0).Text = "Distribución por Rango de Edad - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Edad") With {
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

        chartEdad.Series.Add(series)
    End Function
End Class
