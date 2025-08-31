<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioSituacion
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.btnGenerar = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.flpTimeline = New System.Windows.Forms.FlowLayoutPanel()
        Me.SplitContainerRight = New System.Windows.Forms.SplitContainer()
        Me.gbxEstados = New System.Windows.Forms.GroupBox()
        Me.dgvEstados = New System.Windows.Forms.DataGridView()
        Me.gbxNovedades = New System.Windows.Forms.GroupBox()
        Me.dgvNovedades = New System.Windows.Forms.DataGridView()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainerRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerRight.Panel1.SuspendLayout()
        Me.SplitContainerRight.Panel2.SuspendLayout()
        Me.SplitContainerRight.SuspendLayout()
        Me.gbxEstados.SuspendLayout()
        CType(Me.dgvEstados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbxNovedades.SuspendLayout()
        CType(Me.dgvNovedades, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblNombre.Location = New System.Drawing.Point(18, 14)
        Me.lblNombre.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(136, 29)
        Me.lblNombre.TabIndex = 0
        Me.lblNombre.Text = "lblNombre"
        '
        'dtpDesde
        '
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(88, 68)
        Me.dtpDesde.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(152, 26)
        Me.dtpDesde.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(20, 72)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Desde:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(266, 72)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Hasta:"
        '
        'dtpHasta
        '
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(332, 68)
        Me.dtpHasta.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(152, 26)
        Me.dtpHasta.TabIndex = 4
        '
        'btnGenerar
        '
        Me.btnGenerar.Location = New System.Drawing.Point(510, 65)
        Me.btnGenerar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGenerar.Name = "btnGenerar"
        Me.btnGenerar.Size = New System.Drawing.Size(182, 35)
        Me.btnGenerar.TabIndex = 6
        Me.btnGenerar.Text = "Generar"
        Me.btnGenerar.UseVisualStyleBackColor = True
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SplitContainer1.Location = New System.Drawing.Point(18, 122)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.flpTimeline)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainerRight)
        Me.SplitContainer1.Size = New System.Drawing.Size(1164, 552)
        Me.SplitContainer1.SplitterDistance = 360
        Me.SplitContainer1.SplitterWidth = 6
        Me.SplitContainer1.TabIndex = 3
        '
        'flpTimeline
        '
        Me.flpTimeline.AutoScroll = True
        Me.flpTimeline.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flpTimeline.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpTimeline.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpTimeline.Location = New System.Drawing.Point(0, 0)
        Me.flpTimeline.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.flpTimeline.Name = "flpTimeline"
        Me.flpTimeline.Padding = New System.Windows.Forms.Padding(8)
        Me.flpTimeline.Size = New System.Drawing.Size(360, 552)
        Me.flpTimeline.TabIndex = 0
        Me.flpTimeline.WrapContents = False
        '
        'SplitContainerRight
        '
        Me.SplitContainerRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerRight.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerRight.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SplitContainerRight.Name = "SplitContainerRight"
        Me.SplitContainerRight.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerRight.Panel1
        '
        Me.SplitContainerRight.Panel1.Controls.Add(Me.gbxEstados)
        '
        'SplitContainerRight.Panel2
        '
        Me.SplitContainerRight.Panel2.Controls.Add(Me.gbxNovedades)
        Me.SplitContainerRight.Size = New System.Drawing.Size(798, 552)
        Me.SplitContainerRight.SplitterDistance = 170
        Me.SplitContainerRight.SplitterWidth = 6
        Me.SplitContainerRight.TabIndex = 0
        '
        'gbxEstados
        '
        Me.gbxEstados.Controls.Add(Me.dgvEstados)
        Me.gbxEstados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxEstados.Location = New System.Drawing.Point(0, 0)
        Me.gbxEstados.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbxEstados.Name = "gbxEstados"
        Me.gbxEstados.Padding = New System.Windows.Forms.Padding(8)
        Me.gbxEstados.Size = New System.Drawing.Size(798, 170)
        Me.gbxEstados.TabIndex = 0
        Me.gbxEstados.TabStop = False
        Me.gbxEstados.Text = "Estados en el período"
        '
        'dgvEstados
        '
        Me.dgvEstados.AllowUserToAddRows = False
        Me.dgvEstados.AllowUserToDeleteRows = False
        Me.dgvEstados.AllowUserToResizeColumns = False
        Me.dgvEstados.AllowUserToResizeRows = False
        Me.dgvEstados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvEstados.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEstados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEstados.Location = New System.Drawing.Point(8, 27)
        Me.dgvEstados.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvEstados.Name = "dgvEstados"
        Me.dgvEstados.ReadOnly = True
        Me.dgvEstados.RowHeadersVisible = False
        Me.dgvEstados.RowHeadersWidth = 62
        Me.dgvEstados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEstados.Size = New System.Drawing.Size(782, 135)
        Me.dgvEstados.TabIndex = 2
        '
        'gbxNovedades
        '
        Me.gbxNovedades.Controls.Add(Me.dgvNovedades)
        Me.gbxNovedades.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxNovedades.Location = New System.Drawing.Point(0, 0)
        Me.gbxNovedades.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.gbxNovedades.Name = "gbxNovedades"
        Me.gbxNovedades.Padding = New System.Windows.Forms.Padding(8)
        Me.gbxNovedades.Size = New System.Drawing.Size(798, 376)
        Me.gbxNovedades.TabIndex = 0
        Me.gbxNovedades.TabStop = False
        Me.gbxNovedades.Text = "Novedades del día (según selección en Timeline)"
        '
        'dgvNovedades
        '
        Me.dgvNovedades.AllowUserToAddRows = False
        Me.dgvNovedades.AllowUserToDeleteRows = False
        Me.dgvNovedades.AllowUserToResizeColumns = False
        Me.dgvNovedades.AllowUserToResizeRows = False
        Me.dgvNovedades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvNovedades.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvNovedades.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvNovedades.Location = New System.Drawing.Point(8, 27)
        Me.dgvNovedades.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvNovedades.Name = "dgvNovedades"
        Me.dgvNovedades.ReadOnly = True
        Me.dgvNovedades.RowHeadersVisible = False
        Me.dgvNovedades.RowHeadersWidth = 62
        Me.dgvNovedades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNovedades.Size = New System.Drawing.Size(782, 341)
        Me.dgvNovedades.TabIndex = 1
        '
        'frmFuncionarioSituacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 692)
        Me.Controls.Add(Me.btnGenerar)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpHasta)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.dtpDesde)
        Me.Controls.Add(Me.lblNombre)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmFuncionarioSituacion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Situación del Funcionario"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainerRight.Panel1.ResumeLayout(False)
        Me.SplitContainerRight.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerRight.ResumeLayout(False)
        Me.gbxEstados.ResumeLayout(False)
        CType(Me.dgvEstados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbxNovedades.ResumeLayout(False)
        CType(Me.dgvNovedades, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblNombre As Label
    Friend WithEvents dtpDesde As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents dtpHasta As DateTimePicker
    Friend WithEvents btnGenerar As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents flpTimeline As FlowLayoutPanel
    Friend WithEvents SplitContainerRight As SplitContainer
    Friend WithEvents gbxEstados As GroupBox
    Friend WithEvents dgvEstados As DataGridView
    Friend WithEvents gbxNovedades As GroupBox
    Friend WithEvents dgvNovedades As DataGridView
End Class
