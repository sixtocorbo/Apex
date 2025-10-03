Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteLicenciasPorEstado
    Inherits Form

    Private ReadOnly _licenciaService As New LicenciaService()
    Private chartEstados As Chart
    Private dtpDesde As DateTimePicker
    Private dtpHasta As DateTimePicker
    Private btnActualizar As Button

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartArea1")
        Dim legend As New Legend("Legend1")
        Dim title As New Title("Licencias por Estado")
        Dim panelFiltros As New FlowLayoutPanel()
        Dim lblDesde As New Label()
        Dim lblHasta As New Label()

        chartEstados = New Chart()
        dtpDesde = New DateTimePicker()
        dtpHasta = New DateTimePicker()
        btnActualizar = New Button()

        SuspendLayout()

        panelFiltros.Dock = DockStyle.Top
        panelFiltros.Height = 45
        panelFiltros.Padding = New Padding(10, 10, 10, 0)
        panelFiltros.AutoSize = True
        panelFiltros.AutoSizeMode = AutoSizeMode.GrowAndShrink

        lblDesde.AutoSize = True
        lblDesde.Text = "Desde:"
        lblDesde.Margin = New Padding(0, 8, 5, 0)

        dtpDesde.Format = DateTimePickerFormat.[Short]
        dtpDesde.Width = 120
        dtpDesde.Margin = New Padding(0, 5, 15, 0)

        lblHasta.AutoSize = True
        lblHasta.Text = "Hasta:"
        lblHasta.Margin = New Padding(0, 8, 5, 0)

        dtpHasta.Format = DateTimePickerFormat.[Short]
        dtpHasta.Width = 120
        dtpHasta.Margin = New Padding(0, 5, 15, 0)

        btnActualizar.Text = "Actualizar"
        btnActualizar.AutoSize = True
        btnActualizar.Margin = New Padding(0, 5, 0, 0)

        panelFiltros.Controls.Add(lblDesde)
        panelFiltros.Controls.Add(dtpDesde)
        panelFiltros.Controls.Add(lblHasta)
        panelFiltros.Controls.Add(dtpHasta)
        panelFiltros.Controls.Add(btnActualizar)

        chartArea.AxisX.Interval = 1
        chartArea.AxisX.MajorGrid.Enabled = False
        chartArea.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash
        chartEstados.ChartAreas.Add(chartArea)
        chartEstados.Dock = DockStyle.Fill
        legend.Enabled = False
        chartEstados.Legends.Add(legend)
        chartEstados.Location = New Point(0, panelFiltros.Bottom)
        chartEstados.Name = "chartEstados"
        chartEstados.Palette = ChartColorPalette.Excel
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartEstados.Titles.Add(title)
        chartEstados.TabIndex = 1

        AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 600)
        Controls.Add(chartEstados)
        Controls.Add(panelFiltros)
        MinimumSize = New Size(650, 450)
        Name = "frmReporteLicenciasPorEstado"
        StartPosition = FormStartPosition.CenterParent
        Text = "Licencias por Estado"
        AcceptButton = btnActualizar

        ResumeLayout(False)
        PerformLayout()
    End Sub

    Private Async Sub frmReporteLicenciasPorEstado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        dtpHasta.Value = Date.Today
        dtpDesde.Value = Date.Today.AddMonths(-12)

        AddHandler btnActualizar.Click, AddressOf btnActualizar_Click

        Await CargarGraficoAsync()
    End Sub

    Private Async Sub btnActualizar_Click(sender As Object, e As EventArgs)
        Await CargarGraficoAsync()
    End Sub

    Private Async Function CargarGraficoAsync() As Task
        Dim fechaDesde = dtpDesde.Value.Date
        Dim fechaHasta = dtpHasta.Value.Date

        If fechaDesde > fechaHasta Then
            MessageBox.Show("La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'.", "Rango inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim datos = Await Task.Run(Function() _licenciaService.GetDistribucionPorEstadoLicencia(fechaDesde, fechaHasta))

        If chartEstados.Titles.Count > 0 Then
            chartEstados.Titles(0).Text = "Licencias por Estado"
        End If
        chartEstados.Series.Clear()
        If datos Is Nothing OrElse Not datos.Any() Then
            If chartEstados.Titles.Count > 0 Then
                chartEstados.Titles(0).Text = "Licencias por Estado - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Estados") With {
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

        chartEstados.Series.Add(series)
    End Function
End Class
