<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGestionViaticos
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
        Me.pnlFiltros = New System.Windows.Forms.Panel()
        Me.lblRegistros = New System.Windows.Forms.Label()
        Me.txtFiltroDinamico = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnGenerar = New System.Windows.Forms.Button()
        Me.dtpPeriodo = New System.Windows.Forms.DateTimePicker()
        Me.lblPeriodo = New System.Windows.Forms.Label()
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.pnlFiltros.SuspendLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlFiltros
        '
        Me.pnlFiltros.BackColor = System.Drawing.Color.WhiteSmoke
        Me.pnlFiltros.Controls.Add(Me.lblRegistros)
        Me.pnlFiltros.Controls.Add(Me.txtFiltroDinamico)
        Me.pnlFiltros.Controls.Add(Me.Label1)
        Me.pnlFiltros.Controls.Add(Me.btnGenerar)
        Me.pnlFiltros.Controls.Add(Me.dtpPeriodo)
        Me.pnlFiltros.Controls.Add(Me.lblPeriodo)
        Me.pnlFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlFiltros.Location = New System.Drawing.Point(0, 0)
        Me.pnlFiltros.Name = "pnlFiltros"
        Me.pnlFiltros.Size = New System.Drawing.Size(784, 100)
        Me.pnlFiltros.TabIndex = 0
        '
        'lblRegistros
        '
        Me.lblRegistros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRegistros.AutoSize = True
        Me.lblRegistros.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblRegistros.Location = New System.Drawing.Point(650, 65)
        Me.lblRegistros.Name = "lblRegistros"
        Me.lblRegistros.Size = New System.Drawing.Size(111, 25)
        Me.lblRegistros.TabIndex = 5
        Me.lblRegistros.Text = "Registros: 0"
        '
        'txtFiltroDinamico
        '
        Me.txtFiltroDinamico.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFiltroDinamico.Location = New System.Drawing.Point(121, 58)
        Me.txtFiltroDinamico.Name = "txtFiltroDinamico"
        Me.txtFiltroDinamico.Size = New System.Drawing.Size(417, 31)
        Me.txtFiltroDinamico.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 62)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 25)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Filtrar:"
        '
        'btnGenerar
        '
        Me.btnGenerar.Location = New System.Drawing.Point(334, 16)
        Me.btnGenerar.Name = "btnGenerar"
        Me.btnGenerar.Size = New System.Drawing.Size(204, 33)
        Me.btnGenerar.TabIndex = 2
        Me.btnGenerar.Text = "Generar Reporte"
        Me.btnGenerar.UseVisualStyleBackColor = True
        '
        'dtpPeriodo
        '
        Me.dtpPeriodo.CustomFormat = "MMMM yyyy"
        Me.dtpPeriodo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpPeriodo.Location = New System.Drawing.Point(121, 16)
        Me.dtpPeriodo.Name = "dtpPeriodo"
        Me.dtpPeriodo.Size = New System.Drawing.Size(193, 31)
        Me.dtpPeriodo.TabIndex = 1
        '
        'lblPeriodo
        '
        Me.lblPeriodo.AutoSize = True
        Me.lblPeriodo.Location = New System.Drawing.Point(12, 22)
        Me.lblPeriodo.Name = "lblPeriodo"
        Me.lblPeriodo.Size = New System.Drawing.Size(77, 25)
        Me.lblPeriodo.TabIndex = 0
        Me.lblPeriodo.Text = "Período:"
        '
        'dgvResultados
        '
        Me.dgvResultados.AllowUserToAddRows = False
        Me.dgvResultados.AllowUserToDeleteRows = False
        Me.dgvResultados.AllowUserToResizeColumns = False
        Me.dgvResultados.AllowUserToResizeRows = False
        Me.dgvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResultados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvResultados.Location = New System.Drawing.Point(0, 100)
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.ReadOnly = True
        Me.dgvResultados.RowHeadersWidth = 51
        Me.dgvResultados.RowTemplate.Height = 25
        Me.dgvResultados.Size = New System.Drawing.Size(784, 261)
        Me.dgvResultados.TabIndex = 1
        '
        'frmGestionViaticos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 361)
        Me.Controls.Add(Me.dgvResultados)
        Me.Controls.Add(Me.pnlFiltros)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Name = "frmGestionViaticos"
        Me.Text = "Gestión de Viáticos"
        Me.pnlFiltros.ResumeLayout(False)
        Me.pnlFiltros.PerformLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlFiltros As Panel
    Friend WithEvents btnGenerar As Button
    Friend WithEvents dtpPeriodo As DateTimePicker
    Friend WithEvents lblPeriodo As Label
    Friend WithEvents dgvResultados As DataGridView
    Friend WithEvents txtFiltroDinamico As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblRegistros As Label
End Class