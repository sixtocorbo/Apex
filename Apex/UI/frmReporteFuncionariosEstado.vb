Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteFuncionariosEstado
    Inherits Form

    Private ReadOnly _funcionarioService As New FuncionarioService()
    Private ReadOnly chartEstado As Chart

    Public Sub New()
        chartEstado = New Chart()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartAreaEstado")
        Dim legend As New Legend("LegendEstado")
        Dim title As New Title("Funcionarios por Estado")

        SuspendLayout()

        chartArea.BackColor = Color.White
        chartEstado.ChartAreas.Add(chartArea)
        chartEstado.Dock = DockStyle.Fill
        legend.Docking = Docking.Bottom
        chartEstado.Legends.Add(legend)
        chartEstado.Location = New Point(0, 0)
        chartEstado.Name = "chartEstado"
        chartEstado.Palette = ChartColorPalette.BrightPastel
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartEstado.Titles.Add(title)
        chartEstado.TabIndex = 0

        AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 600)
        Controls.Add(chartEstado)
        MinimumSize = New Size(520, 360)
        Name = "frmReporteFuncionariosEstado"
        StartPosition = FormStartPosition.CenterParent
        Text = "Funcionarios Activos vs. Inactivos"

        ResumeLayout(False)
    End Sub

    Private Async Sub frmReporteFuncionariosEstado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        Await CargarGraficoAsync()
    End Sub

    Private Async Function CargarGraficoAsync() As Task
        Dim datos = Await Task.Run(Function() _funcionarioService.GetDistribucionPorEstado())

        If chartEstado.Titles.Count > 0 Then
            chartEstado.Titles(0).Text = "Funcionarios por Estado"
        End If
        chartEstado.Series.Clear()

        If datos Is Nothing OrElse Not datos.Any() Then
            If chartEstado.Titles.Count > 0 Then
                chartEstado.Titles(0).Text = "Funcionarios por Estado - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Estado") With {
            .ChartType = SeriesChartType.Pie,
            .IsValueShownAsLabel = True
        }

        For Each item In datos
            series.Points.AddXY(item.Etiqueta, item.Valor)
        Next

        chartEstado.Series.Add(series)
    End Function
End Class
