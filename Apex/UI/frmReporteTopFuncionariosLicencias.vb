Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Globalization
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteTopFuncionariosLicencias
    Inherits Form

    Private ReadOnly _licenciaService As New LicenciaService()
    Private chartFuncionarios As Chart
    Private dtpDesde As DateTimePicker
    Private dtpHasta As DateTimePicker
    Private btnActualizar As Button
    Private nudTop As NumericUpDown

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartArea1")
        Dim legend As New Legend("Legend1")
        Dim title As New Title("Funcionarios con más Licencias")
        Dim panelFiltros As New FlowLayoutPanel()
        Dim lblDesde As New Label()
        Dim lblHasta As New Label()
        Dim lblTop As New Label()

        chartFuncionarios = New Chart()
        dtpDesde = New DateTimePicker()
        dtpHasta = New DateTimePicker()
        btnActualizar = New Button()
        nudTop = New NumericUpDown()

        SuspendLayout()

        panelFiltros.Dock = DockStyle.Top
        panelFiltros.Height = 45
        panelFiltros.Padding = New Padding(10, 10, 10, 0)
        panelFiltros.AutoSize = True
        panelFiltros.AutoSizeMode = AutoSizeMode.GrowAndShrink

        lblDesde.AutoSize = True
        lblDesde.Text = "Desde:"
        lblDesde.Margin = New Padding(0, 8, 5, 0)

        dtpDesde.Format = DateTimePickerFormat.Short
        dtpDesde.Width = 120
        dtpDesde.Margin = New Padding(0, 5, 15, 0)

        lblHasta.AutoSize = True
        lblHasta.Text = "Hasta:"
        lblHasta.Margin = New Padding(0, 8, 5, 0)

        dtpHasta.Format = DateTimePickerFormat.Short
        dtpHasta.Width = 120
        dtpHasta.Margin = New Padding(0, 5, 15, 0)

        lblTop.AutoSize = True
        lblTop.Text = "Top:"
        lblTop.Margin = New Padding(0, 8, 5, 0)

        nudTop.Minimum = 3
        nudTop.Maximum = 25
        nudTop.Value = 10
        nudTop.Width = 60
        nudTop.Margin = New Padding(0, 5, 15, 0)

        btnActualizar.Text = "Actualizar"
        btnActualizar.AutoSize = True
        btnActualizar.Margin = New Padding(0, 5, 0, 0)

        panelFiltros.Controls.Add(lblDesde)
        panelFiltros.Controls.Add(dtpDesde)
        panelFiltros.Controls.Add(lblHasta)
        panelFiltros.Controls.Add(dtpHasta)
        panelFiltros.Controls.Add(lblTop)
        panelFiltros.Controls.Add(nudTop)
        panelFiltros.Controls.Add(btnActualizar)

        chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash
        chartArea.AxisY.Interval = 1
        chartArea.AxisY.MajorGrid.Enabled = False
        chartArea.AxisY.IsLabelAutoFit = True
        chartArea.AxisY.LabelStyle.Interval = 1
        chartArea.AxisY.LabelStyle.Font = New Font("Segoe UI", 9.0!)
        chartArea.AxisY.LabelStyle.IsEndLabelVisible = True
        chartFuncionarios.ChartAreas.Add(chartArea)
        chartFuncionarios.Dock = DockStyle.Fill
        legend.Enabled = False
        chartFuncionarios.Legends.Add(legend)
        chartFuncionarios.Location = New Point(0, panelFiltros.Bottom)
        chartFuncionarios.Name = "chartFuncionarios"
        chartFuncionarios.Palette = ChartColorPalette.Chocolate
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartFuncionarios.Titles.Add(title)
        chartFuncionarios.TabIndex = 1

        AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 600)
        Controls.Add(chartFuncionarios)
        Controls.Add(panelFiltros)
        MinimumSize = New Size(700, 450)
        Name = "frmReporteTopFuncionariosLicencias"
        StartPosition = FormStartPosition.CenterParent
        Text = "Funcionarios con más Licencias"
        AcceptButton = btnActualizar

        ResumeLayout(False)
        PerformLayout()
    End Sub

    Private Async Sub frmReporteTopFuncionariosLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        Dim topN = CInt(nudTop.Value)
        Dim datos = Await Task.Run(Function() _licenciaService.GetTopFuncionariosConLicencias(topN, fechaDesde, fechaHasta))

        If chartFuncionarios.Titles.Count > 0 Then
            chartFuncionarios.Titles(0).Text = "Funcionarios con más Licencias"
        End If
        chartFuncionarios.Series.Clear()
        If datos Is Nothing OrElse Not datos.Any() Then
            If chartFuncionarios.Titles.Count > 0 Then
                chartFuncionarios.Titles(0).Text = "Funcionarios con más Licencias - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Funcionarios") With {
        .ChartType = SeriesChartType.Bar,
        .IsValueShownAsLabel = True
    }

        Dim chartAreaConfig As ChartArea = chartFuncionarios.ChartAreas(0)
        chartAreaConfig.AxisY.MaximumAutoSize = 100.0R
        chartAreaConfig.AxisY.ScaleView.ZoomReset(0)
        chartAreaConfig.AxisY.Minimum = Double.NaN
        chartAreaConfig.AxisY.Maximum = Double.NaN

        Dim totalFuncionarios = datos.Count
        Dim labelFontSize As Single

        If totalFuncionarios <= 6 Then
            labelFontSize = 9.0!
        ElseIf totalFuncionarios <= 12 Then
            labelFontSize = 8.0!
        Else
            labelFontSize = 7.0!
        End If

        ' --- FIX ---
        ' Set the font for the data point labels (the values shown on the bars).
        series.Font = New Font("Segoe UI", labelFontSize)

        ' The line below was applying the font to the Y-axis labels (the official's name), not the bar value.
        ' chartAreaConfig.AxisY.LabelStyle.Font = New Font("Segoe UI", labelFontSize)
        ' -----------

        For Each item In datos
            Dim point = New DataPoint()
            point.AxisLabel = item.Etiqueta
            point.YValues = New Double() {CDbl(item.Valor)}
            point.Label = item.Valor.ToString()
            series.Points.Add(point)
        Next

        Dim pointWidth = If(totalFuncionarios <= 5, 0.6R, Math.Max(0.2R, 0.6R - (totalFuncionarios - 5) * 0.04R))
        series("PointWidth") = pointWidth.ToString(CultureInfo.InvariantCulture)

        chartFuncionarios.Series.Add(series)
    End Function
End Class