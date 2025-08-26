<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAnalisisEstacionalidad
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
        Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
        Me.PanelFiltros = New System.Windows.Forms.Panel()
        Me.cboModeloPrediccion = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.chkAnios = New System.Windows.Forms.CheckedListBox()
        Me.btnPredecir = New System.Windows.Forms.Button()
        Me.btnFiltrar = New System.Windows.Forms.Button()
        Me.chkTiposLicencia = New System.Windows.Forms.CheckedListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PanelGrafico = New System.Windows.Forms.Panel()
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelFiltros.SuspendLayout()
        Me.PanelGrafico.SuspendLayout()
        Me.SuspendLayout()
        '
        'Chart1
        '
        ChartArea1.Name = "ChartArea1"
        Me.Chart1.ChartAreas.Add(ChartArea1)
        Me.Chart1.Dock = System.Windows.Forms.DockStyle.Fill
        Legend1.Name = "Legend1"
        Me.Chart1.Legends.Add(Legend1)
        Me.Chart1.Location = New System.Drawing.Point(0, 0)
        Me.Chart1.Name = "Chart1"
        Me.Chart1.Size = New System.Drawing.Size(952, 681)
        Me.Chart1.TabIndex = 0
        Me.Chart1.Text = "Chart1"
        '
        'PanelFiltros
        '
        Me.PanelFiltros.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelFiltros.Controls.Add(Me.cboModeloPrediccion)
        Me.PanelFiltros.Controls.Add(Me.Label3)
        Me.PanelFiltros.Controls.Add(Me.Label2)
        Me.PanelFiltros.Controls.Add(Me.chkAnios)
        Me.PanelFiltros.Controls.Add(Me.btnPredecir)
        Me.PanelFiltros.Controls.Add(Me.btnFiltrar)
        Me.PanelFiltros.Controls.Add(Me.chkTiposLicencia)
        Me.PanelFiltros.Controls.Add(Me.Label1)
        Me.PanelFiltros.Dock = System.Windows.Forms.DockStyle.Left
        Me.PanelFiltros.Location = New System.Drawing.Point(0, 0)
        Me.PanelFiltros.Name = "PanelFiltros"
        Me.PanelFiltros.Size = New System.Drawing.Size(250, 681)
        Me.PanelFiltros.TabIndex = 1
        '
        'cboModeloPrediccion
        '
        Me.cboModeloPrediccion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboModeloPrediccion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboModeloPrediccion.FormattingEnabled = True
        Me.cboModeloPrediccion.Location = New System.Drawing.Point(12, 574)
        Me.cboModeloPrediccion.Name = "cboModeloPrediccion"
        Me.cboModeloPrediccion.Size = New System.Drawing.Size(226, 21)
        Me.cboModeloPrediccion.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 554)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(147, 17)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Modelo de Predicción"
        '
        'Label2
        '
        Me.Label2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 420)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(135, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Años a considerar"
        '
        'chkAnios
        '
        Me.chkAnios.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkAnios.CheckOnClick = True
        Me.chkAnios.FormattingEnabled = True
        Me.chkAnios.Location = New System.Drawing.Point(12, 443)
        Me.chkAnios.Name = "chkAnios"
        Me.chkAnios.Size = New System.Drawing.Size(226, 94)
        Me.chkAnios.TabIndex = 4
        '
        'btnPredecir
        '
        Me.btnPredecir.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPredecir.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnPredecir.Location = New System.Drawing.Point(12, 607)
        Me.btnPredecir.Name = "btnPredecir"
        Me.btnPredecir.Size = New System.Drawing.Size(226, 31)
        Me.btnPredecir.TabIndex = 3
        Me.btnPredecir.Text = "Predecir Próximo Año"
        Me.btnPredecir.UseVisualStyleBackColor = True
        '
        'btnFiltrar
        '
        Me.btnFiltrar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFiltrar.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFiltrar.Location = New System.Drawing.Point(12, 644)
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(226, 31)
        Me.btnFiltrar.TabIndex = 2
        Me.btnFiltrar.Text = "Aplicar Filtro"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'chkTiposLicencia
        '
        Me.chkTiposLicencia.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkTiposLicencia.CheckOnClick = True
        Me.chkTiposLicencia.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTiposLicencia.FormattingEnabled = True
        Me.chkTiposLicencia.Location = New System.Drawing.Point(12, 38)
        Me.chkTiposLicencia.Name = "chkTiposLicencia"
        Me.chkTiposLicencia.Size = New System.Drawing.Size(226, 364)
        Me.chkTiposLicencia.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(130, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tipos de Licencia"
        '
        'PanelGrafico
        '
        Me.PanelGrafico.Controls.Add(Me.Chart1)
        Me.PanelGrafico.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelGrafico.Location = New System.Drawing.Point(250, 0)
        Me.PanelGrafico.Name = "PanelGrafico"
        Me.PanelGrafico.Size = New System.Drawing.Size(952, 681)
        Me.PanelGrafico.TabIndex = 2
        '
        'frmAnalisisEstacionalidad
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1202, 681)
        Me.Controls.Add(Me.PanelGrafico)
        Me.Controls.Add(Me.PanelFiltros)
        Me.Name = "frmAnalisisEstacionalidad"
        Me.Text = "Análisis y Predicción de Licencias"
        CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelFiltros.ResumeLayout(False)
        Me.PanelFiltros.PerformLayout()
        Me.PanelGrafico.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Chart1 As DataVisualization.Charting.Chart
    Friend WithEvents PanelFiltros As Panel
    Friend WithEvents btnFiltrar As Button
    Friend WithEvents chkTiposLicencia As CheckedListBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PanelGrafico As Panel
    Friend WithEvents btnPredecir As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents chkAnios As CheckedListBox
    Friend WithEvents cboModeloPrediccion As ComboBox
    Friend WithEvents Label3 As Label
End Class