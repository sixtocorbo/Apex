Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmReporteLicenciasPorTipo
    Inherits Form

    Private ReadOnly _licenciaService As New LicenciaService()
    Private chartLicencias As Chart
    Private dtpDesde As DateTimePicker
    Private dtpHasta As DateTimePicker
    Private btnActualizar As Button

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        Dim chartArea As New ChartArea("ChartArea1")
        Dim legend As New Legend("Legend1")
        Dim title As New Title("Distribución de Licencias por Tipo")
        Dim panelFiltros As New FlowLayoutPanel()
        Dim lblDesde As New Label()
        Dim lblHasta As New Label()

        chartLicencias = New Chart()
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

        chartArea.BackColor = Color.White
        chartLicencias.ChartAreas.Add(chartArea)
        chartLicencias.Dock = DockStyle.Fill
        legend.Docking = Docking.Bottom
        chartLicencias.Legends.Add(legend)
        chartLicencias.Location = New Point(0, panelFiltros.Bottom)
        chartLicencias.Name = "chartLicencias"
        chartLicencias.Palette = ChartColorPalette.BrightPastel
        title.Font = New Font("Segoe UI", 12.0!, FontStyle.Bold)
        chartLicencias.Titles.Add(title)
        chartLicencias.TabIndex = 1

        AutoScaleDimensions = New SizeF(9.0!, 20.0!)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(900, 600)
        Controls.Add(chartLicencias)
        Controls.Add(panelFiltros)
        MinimumSize = New Size(650, 450)
        Name = "frmReporteLicenciasPorTipo"
        StartPosition = FormStartPosition.CenterParent
        Text = "Distribución de Licencias por Tipo"
        AcceptButton = btnActualizar

        ResumeLayout(False)
        PerformLayout()
    End Sub

    Private Async Sub frmReporteLicenciasPorTipo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        Dim datos = Await Task.Run(Function() _licenciaService.GetDistribucionPorTipoLicencia(fechaDesde, fechaHasta))

        If chartLicencias.Titles.Count > 0 Then
            chartLicencias.Titles(0).Text = "Distribución de Licencias por Tipo"
        End If
        chartLicencias.Series.Clear()
        If datos Is Nothing OrElse Not datos.Any() Then
            If chartLicencias.Titles.Count > 0 Then
                chartLicencias.Titles(0).Text = "Distribución de Licencias por Tipo - Sin datos"
            End If
            Return
        End If

        Dim series As New Series("Tipos") With {
            .ChartType = SeriesChartType.Pie,
            .IsValueShownAsLabel = True
        }

        For Each item In datos
            series.Points.AddXY(item.Etiqueta, item.Valor)
        Next

        chartLicencias.Series.Add(series)
    End Function
End Class
