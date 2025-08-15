<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmConfiguracion
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
        Me.PanelPrincipal = New System.Windows.Forms.Panel()
        Me.TabControlConfig = New System.Windows.Forms.TabControl()
        Me.TabPageViaticos = New System.Windows.Forms.TabPage()
        Me.txtReglasViaticos = New System.Windows.Forms.TextBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.PanelPrincipal.SuspendLayout()
        Me.TabControlConfig.SuspendLayout()
        Me.TabPageViaticos.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelPrincipal
        '
        Me.PanelPrincipal.Controls.Add(Me.TabControlConfig)
        Me.PanelPrincipal.Controls.Add(Me.FlowLayoutPanel1)
        Me.PanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelPrincipal.Location = New System.Drawing.Point(0, 0)
        Me.PanelPrincipal.Name = "PanelPrincipal"
        Me.PanelPrincipal.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelPrincipal.Size = New System.Drawing.Size(784, 461)
        Me.PanelPrincipal.TabIndex = 0
        '
        'TabControlConfig
        '
        Me.TabControlConfig.Controls.Add(Me.TabPageViaticos)
        Me.TabControlConfig.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlConfig.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.TabControlConfig.Location = New System.Drawing.Point(10, 10)
        Me.TabControlConfig.Name = "TabControlConfig"
        Me.TabControlConfig.SelectedIndex = 0
        Me.TabControlConfig.Size = New System.Drawing.Size(764, 395)
        Me.TabControlConfig.TabIndex = 1
        '
        'TabPageViaticos
        '
        Me.TabPageViaticos.Controls.Add(Me.txtReglasViaticos)
        Me.TabPageViaticos.Location = New System.Drawing.Point(4, 32)
        Me.TabPageViaticos.Name = "TabPageViaticos"
        Me.TabPageViaticos.Padding = New System.Windows.Forms.Padding(10)
        Me.TabPageViaticos.Size = New System.Drawing.Size(756, 359)
        Me.TabPageViaticos.TabIndex = 0
        Me.TabPageViaticos.Text = "Reglas de Viáticos"
        Me.TabPageViaticos.UseVisualStyleBackColor = True
        '
        'txtReglasViaticos
        '
        Me.txtReglasViaticos.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.txtReglasViaticos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtReglasViaticos.Font = New System.Drawing.Font("Consolas", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReglasViaticos.Location = New System.Drawing.Point(10, 10)
        Me.txtReglasViaticos.Multiline = True
        Me.txtReglasViaticos.Name = "txtReglasViaticos"
        Me.txtReglasViaticos.ReadOnly = True
        Me.txtReglasViaticos.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtReglasViaticos.Size = New System.Drawing.Size(736, 339)
        Me.txtReglasViaticos.TabIndex = 0
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCerrar)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(10, 405)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(764, 46)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'btnCerrar
        '
        Me.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCerrar.Location = New System.Drawing.Point(658, 3)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(103, 40)
        Me.btnCerrar.TabIndex = 0
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'frmConfiguracion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCerrar
        Me.ClientSize = New System.Drawing.Size(784, 461)
        Me.Controls.Add(Me.PanelPrincipal)
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "frmConfiguracion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Configuración y Reglas de Negocio"
        Me.PanelPrincipal.ResumeLayout(False)
        Me.TabControlConfig.ResumeLayout(False)
        Me.TabPageViaticos.ResumeLayout(False)
        Me.TabPageViaticos.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents PanelPrincipal As Panel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnCerrar As Button
    Friend WithEvents TabControlConfig As TabControl
    Friend WithEvents TabPageViaticos As TabPage
    Friend WithEvents txtReglasViaticos As TextBox
End Class