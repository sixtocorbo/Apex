<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEstadosTransitoriosGeneral
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.dgvEstados = New System.Windows.Forms.DataGridView()
        Me.btnVerDetalles = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtFiltro = New System.Windows.Forms.TextBox()
        Me.MainLayout = New System.Windows.Forms.TableLayoutPanel()
        CType(Me.dgvEstados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainLayout.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvEstados
        '
        Me.dgvEstados.AllowUserToAddRows = False
        Me.dgvEstados.AllowUserToDeleteRows = False
        Me.dgvEstados.AllowUserToResizeRows = False
        Me.dgvEstados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.MainLayout.SetColumnSpan(Me.dgvEstados, 2)
        Me.dgvEstados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEstados.Location = New System.Drawing.Point(4, 42)
        Me.dgvEstados.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvEstados.MultiSelect = False
        Me.dgvEstados.Name = "dgvEstados"
        Me.dgvEstados.ReadOnly = True
        Me.dgvEstados.RowHeadersWidth = 62
        Me.dgvEstados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEstados.Size = New System.Drawing.Size(1150, 592)
        Me.dgvEstados.TabIndex = 2
        '
        'btnVerDetalles
        '
        Me.btnVerDetalles.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.MainLayout.SetColumnSpan(Me.btnVerDetalles, 2)
        Me.btnVerDetalles.Location = New System.Drawing.Point(1042, 644)
        Me.btnVerDetalles.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnVerDetalles.Name = "btnVerDetalles"
        Me.btnVerDetalles.Size = New System.Drawing.Size(112, 35)
        Me.btnVerDetalles.TabIndex = 3
        Me.btnVerDetalles.Text = "Ver Detalles"
        Me.btnVerDetalles.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 8)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(271, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Filtrar por Nombre, Apellido o Cédula:"
        '
        'txtFiltro
        '
        Me.txtFiltro.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFiltro.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtFiltro.Location = New System.Drawing.Point(283, 5)
        Me.txtFiltro.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtFiltro.Name = "txtFiltro"
        Me.txtFiltro.Size = New System.Drawing.Size(871, 26)
        Me.txtFiltro.TabIndex = 1
        '
        'MainLayout
        '
        Me.MainLayout.ColumnCount = 2
        Me.MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.Controls.Add(Me.Label1, 0, 0)
        Me.MainLayout.Controls.Add(Me.btnVerDetalles, 0, 2)
        Me.MainLayout.Controls.Add(Me.txtFiltro, 1, 0)
        Me.MainLayout.Controls.Add(Me.dgvEstados, 0, 1)
        Me.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainLayout.Location = New System.Drawing.Point(10, 10)
        Me.MainLayout.Name = "MainLayout"
        Me.MainLayout.RowCount = 3
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.Size = New System.Drawing.Size(1158, 684)
        Me.MainLayout.TabIndex = 5
        '
        'frmEstadosTransitoriosGeneral
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1178, 704)
        Me.Controls.Add(Me.MainLayout)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frmEstadosTransitoriosGeneral"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Visor de Estados Transitorios"
        CType(Me.dgvEstados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainLayout.ResumeLayout(False)
        Me.MainLayout.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvEstados As DataGridView
    Friend WithEvents btnVerDetalles As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtFiltro As TextBox
    Friend WithEvents MainLayout As TableLayoutPanel
End Class