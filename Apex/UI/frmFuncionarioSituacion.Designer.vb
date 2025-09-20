<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioSituacion
    Inherits FormActualizable

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
        Me.grpEstados = New System.Windows.Forms.GroupBox()
        Me.dgvEstados = New System.Windows.Forms.DataGridView()
        Me.SplitContainerRight = New System.Windows.Forms.SplitContainer()
        Me.grpNovedades = New System.Windows.Forms.GroupBox()
        Me.dgvNovedades = New System.Windows.Forms.DataGridView()
        Me.grpFechas = New System.Windows.Forms.GroupBox()
        Me.flpTimeline = New System.Windows.Forms.FlowLayoutPanel()
        Me.MainLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlHeader = New System.Windows.Forms.FlowLayoutPanel()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.grpEstados.SuspendLayout()
        CType(Me.dgvEstados, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SplitContainerRight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerRight.Panel1.SuspendLayout()
        Me.SplitContainerRight.Panel2.SuspendLayout()
        Me.SplitContainerRight.SuspendLayout()
        Me.grpNovedades.SuspendLayout()
        CType(Me.dgvNovedades, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpFechas.SuspendLayout()
        Me.MainLayout.SuspendLayout()
        Me.pnlHeader.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblNombre
        '
        Me.lblNombre.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblNombre.Location = New System.Drawing.Point(4, 8)
        Me.lblNombre.Margin = New System.Windows.Forms.Padding(4, 0, 20, 0)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(136, 29)
        Me.lblNombre.TabIndex = 0
        Me.lblNombre.Text = "lblNombre"
        '
        'dtpDesde
        '
        Me.dtpDesde.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpDesde.Location = New System.Drawing.Point(232, 10)
        Me.dtpDesde.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(152, 26)
        Me.dtpDesde.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(164, 12)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Desde:"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(392, 12)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Hasta:"
        '
        'dtpHasta
        '
        Me.dtpHasta.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpHasta.Location = New System.Drawing.Point(456, 10)
        Me.dtpHasta.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(152, 26)
        Me.dtpHasta.TabIndex = 4
        '
        'btnGenerar
        '
        Me.btnGenerar.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnGenerar.Location = New System.Drawing.Point(616, 5)
        Me.btnGenerar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGenerar.Name = "btnGenerar"
        Me.btnGenerar.Size = New System.Drawing.Size(112, 35)
        Me.btnGenerar.TabIndex = 6
        Me.btnGenerar.Text = "Generar"
        Me.btnGenerar.UseVisualStyleBackColor = True
        Me.btnGenerar.Visible = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(4, 56)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.grpEstados)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.SplitContainerRight)
        Me.SplitContainer1.Size = New System.Drawing.Size(682, 484)
        Me.SplitContainer1.SplitterDistance = 388
        Me.SplitContainer1.SplitterWidth = 6
        Me.SplitContainer1.TabIndex = 1
        '
        'grpEstados
        '
        Me.grpEstados.Controls.Add(Me.dgvEstados)
        Me.grpEstados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpEstados.Location = New System.Drawing.Point(0, 0)
        Me.grpEstados.Name = "grpEstados"
        Me.grpEstados.Padding = New System.Windows.Forms.Padding(8)
        Me.grpEstados.Size = New System.Drawing.Size(388, 484)
        Me.grpEstados.TabIndex = 0
        Me.grpEstados.TabStop = False
        Me.grpEstados.Text = "Estados en el período"
        '
        'dgvEstados
        '
        Me.dgvEstados.AllowUserToAddRows = False
        Me.dgvEstados.AllowUserToDeleteRows = False
        Me.dgvEstados.AllowUserToResizeColumns = False
        Me.dgvEstados.AllowUserToResizeRows = False
        Me.dgvEstados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvEstados.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgvEstados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True
        Me.dgvEstados.DefaultCellStyle = DataGridViewCellStyle1
        Me.dgvEstados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEstados.Location = New System.Drawing.Point(8, 27)
        Me.dgvEstados.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvEstados.Name = "dgvEstados"
        Me.dgvEstados.ReadOnly = True
        Me.dgvEstados.RowHeadersVisible = False
        Me.dgvEstados.RowHeadersWidth = 62
        Me.dgvEstados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEstados.Size = New System.Drawing.Size(372, 449)
        Me.dgvEstados.TabIndex = 2
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
        Me.SplitContainerRight.Panel1.Controls.Add(Me.grpNovedades)
        '
        'SplitContainerRight.Panel2
        '
        Me.SplitContainerRight.Panel2.Controls.Add(Me.grpFechas)
        Me.SplitContainerRight.Size = New System.Drawing.Size(288, 484)
        Me.SplitContainerRight.SplitterDistance = 226
        Me.SplitContainerRight.SplitterWidth = 6
        Me.SplitContainerRight.TabIndex = 0
        '
        'grpNovedades
        '
        Me.grpNovedades.Controls.Add(Me.dgvNovedades)
        Me.grpNovedades.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpNovedades.Location = New System.Drawing.Point(0, 0)
        Me.grpNovedades.Name = "grpNovedades"
        Me.grpNovedades.Padding = New System.Windows.Forms.Padding(8)
        Me.grpNovedades.Size = New System.Drawing.Size(288, 226)
        Me.grpNovedades.TabIndex = 0
        Me.grpNovedades.TabStop = False
        Me.grpNovedades.Text = "Panel de Novedades"
        '
        'dgvNovedades
        '
        Me.dgvNovedades.AllowUserToAddRows = False
        Me.dgvNovedades.AllowUserToDeleteRows = False
        Me.dgvNovedades.AllowUserToResizeColumns = False
        Me.dgvNovedades.AllowUserToResizeRows = False
        Me.dgvNovedades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvNovedades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False
        Me.dgvNovedades.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvNovedades.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvNovedades.Location = New System.Drawing.Point(8, 27)
        Me.dgvNovedades.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvNovedades.Name = "dgvNovedades"
        Me.dgvNovedades.ReadOnly = True
        Me.dgvNovedades.RowHeadersVisible = False
        Me.dgvNovedades.RowHeadersWidth = 62
        Me.dgvNovedades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNovedades.Size = New System.Drawing.Size(272, 191)
        Me.dgvNovedades.TabIndex = 1
        '
        'grpFechas
        '
        Me.grpFechas.Controls.Add(Me.flpTimeline)
        Me.grpFechas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grpFechas.Location = New System.Drawing.Point(0, 0)
        Me.grpFechas.Name = "grpFechas"
        Me.grpFechas.Padding = New System.Windows.Forms.Padding(8)
        Me.grpFechas.Size = New System.Drawing.Size(288, 252)
        Me.grpFechas.TabIndex = 1
        Me.grpFechas.TabStop = False
        Me.grpFechas.Text = "Fechas de Novedades"
        '
        'flpTimeline
        '
        Me.flpTimeline.AutoScroll = True
        Me.flpTimeline.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.flpTimeline.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpTimeline.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpTimeline.Location = New System.Drawing.Point(8, 27)
        Me.flpTimeline.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.flpTimeline.Name = "flpTimeline"
        Me.flpTimeline.Padding = New System.Windows.Forms.Padding(8)
        Me.flpTimeline.Size = New System.Drawing.Size(272, 217)
        Me.flpTimeline.TabIndex = 0
        Me.flpTimeline.WrapContents = False
        '
        'MainLayout
        '
        Me.MainLayout.ColumnCount = 1
        Me.MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.Controls.Add(Me.pnlHeader, 0, 0)
        Me.MainLayout.Controls.Add(Me.SplitContainer1, 0, 1)
        Me.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainLayout.Location = New System.Drawing.Point(10, 10)
        Me.MainLayout.Name = "MainLayout"
        Me.MainLayout.RowCount = 2
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.Size = New System.Drawing.Size(690, 545)
        Me.MainLayout.TabIndex = 7
        '
        'pnlHeader
        '
        Me.pnlHeader.AutoSize = True
        Me.pnlHeader.Controls.Add(Me.lblNombre)
        Me.pnlHeader.Controls.Add(Me.Label1)
        Me.pnlHeader.Controls.Add(Me.dtpDesde)
        Me.pnlHeader.Controls.Add(Me.Label2)
        Me.pnlHeader.Controls.Add(Me.dtpHasta)
        Me.pnlHeader.Controls.Add(Me.btnGenerar)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlHeader.Location = New System.Drawing.Point(3, 3)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(684, 45)
        Me.pnlHeader.TabIndex = 0
        '
        'frmFuncionarioSituacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(710, 565)
        Me.Controls.Add(Me.MainLayout)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmFuncionarioSituacion"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Situación del Funcionario"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.grpEstados.ResumeLayout(False)
        CType(Me.dgvEstados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerRight.Panel1.ResumeLayout(False)
        Me.SplitContainerRight.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerRight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerRight.ResumeLayout(False)
        Me.grpNovedades.ResumeLayout(False)
        CType(Me.dgvNovedades, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpFechas.ResumeLayout(False)
        Me.MainLayout.ResumeLayout(False)
        Me.MainLayout.PerformLayout()
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblNombre As Label
    Friend WithEvents dtpDesde As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents dtpHasta As DateTimePicker
    Friend WithEvents btnGenerar As Button
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents SplitContainerRight As SplitContainer
    Friend WithEvents flpTimeline As FlowLayoutPanel
    Friend WithEvents dgvEstados As DataGridView
    Friend WithEvents dgvNovedades As DataGridView
    Friend WithEvents grpEstados As GroupBox
    Friend WithEvents grpNovedades As GroupBox
    Friend WithEvents grpFechas As GroupBox
    Friend WithEvents MainLayout As TableLayoutPanel
    Friend WithEvents pnlHeader As FlowLayoutPanel
End Class