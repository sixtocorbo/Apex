Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteFuncionariosGenero
    Inherits Form

    Private ReadOnly _funcionarioService As New FuncionarioService()
    Private chartGenero As Chart

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartArea1")
        Dim legend As New Legend("Legend1")
        Dim title As New Title("Distribución por Género")

        Me.chartGenero = New Chart()
        Me.SuspendLayout()

        chartArea.BackColor = Color.White
        chartGenero.ChartAreas.Add(chartArea)
        chartGenero.Dock = DockStyle.Fill
        legend.Docking = Docking.Bottom
        chartGenero.Legends.Add(legend)
        chartGenero.Location = New Point(0, 0)
        chartGenero.Name = "chartGenero"
        chartGenero.Palette = ChartColorPalette.Pastel
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartGenero.Titles.Add(title)
        chartGenero.TabIndex = 0
        chartGenero.Text = "Distribución por Género"

        Me.AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.ClientSize = New Size(900, 600)
        Me.Controls.Add(chartGenero)
        Me.MinimumSize = New Size(600, 400)
        Me.Name = "frmReporteFuncionariosGenero"
        Me.StartPosition = FormStartPosition.CenterParent
        Me.Text = "Distribución de Funcionarios por Género"

        Me.ResumeLayout(False)
    End Sub

    Private Async Sub frmReporteFuncionariosGenero_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        Await CargarGraficoAsync()
    End Sub

    Private Async Function CargarGraficoAsync() As Task
        Dim datos = Await Task.Run(Function() _funcionarioService.GetDistribucionPorGenero())

        If chartGenero.Titles.Count > 0 Then
            chartGenero.Titles(0).Text = "Distribución por Género"
        End If
        chartGenero.Series.Clear()
        If datos Is Nothing OrElse Not datos.Any() Then
            If chartGenero.Titles.Count > 0 Then
                chartGenero.Titles(0).Text = "Distribución por Género - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Género") With {
            .ChartType = SeriesChartType.Pie,
            .IsValueShownAsLabel = True
        }

        For Each item In datos
            series.Points.AddXY(item.Etiqueta, item.Valor)
        Next

        chartGenero.Series.Add(series)
    End Function
End Class
