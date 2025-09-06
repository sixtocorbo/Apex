<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmReportes
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
        Me.btnAnalisisFuncionarios = New System.Windows.Forms.Button()
        Me.btnAnalisisEstacional = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAnalisisFuncionarios
        '
        Me.btnAnalisisFuncionarios.Location = New System.Drawing.Point(297, 5)
        Me.btnAnalisisFuncionarios.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAnalisisFuncionarios.Name = "btnAnalisisFuncionarios"
        Me.btnAnalisisFuncionarios.Size = New System.Drawing.Size(285, 35)
        Me.btnAnalisisFuncionarios.TabIndex = 2
        Me.btnAnalisisFuncionarios.Text = "Análisis de funcionarios"
        Me.btnAnalisisFuncionarios.UseVisualStyleBackColor = True
        '
        'btnAnalisisEstacional
        '
        Me.btnAnalisisEstacional.Location = New System.Drawing.Point(4, 5)
        Me.btnAnalisisEstacional.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAnalisisEstacional.Name = "btnAnalisisEstacional"
        Me.btnAnalisisEstacional.Size = New System.Drawing.Size(285, 35)
        Me.btnAnalisisEstacional.TabIndex = 0
        Me.btnAnalisisEstacional.Text = "Analisis estacional"
        Me.btnAnalisisEstacional.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAnalisisEstacional)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAnalisisFuncionarios)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(717, 375)
        Me.FlowLayoutPanel1.TabIndex = 8
        '
        'frmReportes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 375)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(739, 431)
        Me.Name = "frmReportes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Reportes"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnAnalisisFuncionarios As Button
    Friend WithEvents btnAnalisisEstacional As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
End Class