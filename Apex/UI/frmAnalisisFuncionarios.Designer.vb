<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAnalisisFuncionarios
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim ChartArea5 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend5 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Title5 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Dim ChartArea6 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend6 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Title6 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Dim ChartArea7 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend7 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Title7 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Dim ChartArea8 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend8 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Dim Title8 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.chartGenero = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.chartEdad = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.chartAreaTrabajo = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.chartCargo = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.chartGenero, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chartEdad, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chartAreaTrabajo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.chartCargo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.chartGenero, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.chartEdad, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.chartAreaTrabajo, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.chartCargo, 1, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1803, 1048)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'chartGenero
        '
        ChartArea5.Name = "ChartArea1"
        Me.chartGenero.ChartAreas.Add(ChartArea5)
        Me.chartGenero.Dock = System.Windows.Forms.DockStyle.Fill
        Legend5.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
        Legend5.Name = "Legend1"
        Me.chartGenero.Legends.Add(Legend5)
        Me.chartGenero.Location = New System.Drawing.Point(4, 5)
        Me.chartGenero.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chartGenero.Name = "chartGenero"
        Me.chartGenero.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel
        Me.chartGenero.Size = New System.Drawing.Size(893, 514)
        Me.chartGenero.TabIndex = 0
        Me.chartGenero.Text = "Chart1"
        Title5.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Title5.Name = "Title1"
        Title5.Text = "Distribución por Género"
        Me.chartGenero.Titles.Add(Title5)
        '
        'chartEdad
        '
        ChartArea6.Name = "ChartArea1"
        Me.chartEdad.ChartAreas.Add(ChartArea6)
        Me.chartEdad.Dock = System.Windows.Forms.DockStyle.Fill
        Legend6.Enabled = False
        Legend6.Name = "Legend1"
        Me.chartEdad.Legends.Add(Legend6)
        Me.chartEdad.Location = New System.Drawing.Point(905, 5)
        Me.chartEdad.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chartEdad.Name = "chartEdad"
        Me.chartEdad.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.SeaGreen
        Me.chartEdad.Size = New System.Drawing.Size(894, 514)
        Me.chartEdad.TabIndex = 1
        Me.chartEdad.Text = "Chart2"
        Title6.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Title6.Name = "Title1"
        Title6.Text = "Distribución por Rango de Edad"
        Me.chartEdad.Titles.Add(Title6)
        '
        'chartAreaTrabajo
        '
        ChartArea7.Name = "ChartArea1"
        Me.chartAreaTrabajo.ChartAreas.Add(ChartArea7)
        Me.chartAreaTrabajo.Dock = System.Windows.Forms.DockStyle.Fill
        Legend7.Enabled = False
        Legend7.Name = "Legend1"
        Me.chartAreaTrabajo.Legends.Add(Legend7)
        Me.chartAreaTrabajo.Location = New System.Drawing.Point(4, 529)
        Me.chartAreaTrabajo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chartAreaTrabajo.Name = "chartAreaTrabajo"
        Me.chartAreaTrabajo.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.EarthTones
        Me.chartAreaTrabajo.Size = New System.Drawing.Size(893, 514)
        Me.chartAreaTrabajo.TabIndex = 2
        Me.chartAreaTrabajo.Text = "Chart3"
        Title7.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Title7.Name = "Title1"
        Title7.Text = "Distribución por Área de Trabajo"
        Me.chartAreaTrabajo.Titles.Add(Title7)
        '
        'chartCargo
        '
        ChartArea8.Name = "ChartArea1"
        Me.chartCargo.ChartAreas.Add(ChartArea8)
        Me.chartCargo.Dock = System.Windows.Forms.DockStyle.Fill
        Legend8.Enabled = False
        Legend8.Name = "Legend1"
        Me.chartCargo.Legends.Add(Legend8)
        Me.chartCargo.Location = New System.Drawing.Point(905, 529)
        Me.chartCargo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chartCargo.Name = "chartCargo"
        Me.chartCargo.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry
        Me.chartCargo.Size = New System.Drawing.Size(894, 514)
        Me.chartCargo.TabIndex = 3
        Me.chartCargo.Text = "Chart4"
        Title8.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Title8.Name = "Title1"
        Title8.Text = "Top 10 Cargos con más personal"
        Me.chartCargo.Titles.Add(Title8)
        '
        'frmAnalisisFuncionarios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1803, 1048)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmAnalisisFuncionarios"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dashboard de Personal"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.chartGenero, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chartEdad, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chartAreaTrabajo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.chartCargo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents chartGenero As DataVisualization.Charting.Chart
    Friend WithEvents chartEdad As DataVisualization.Charting.Chart
    Friend WithEvents chartAreaTrabajo As DataVisualization.Charting.Chart
    Friend WithEvents chartCargo As DataVisualization.Charting.Chart
End Class