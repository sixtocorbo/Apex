<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DayControl
    Inherits System.Windows.Forms.UserControl

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
        Me.lblDay = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblDay
        '
        Me.lblDay.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblDay.AutoSize = True
        Me.lblDay.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDay.Location = New System.Drawing.Point(21, 5)
        Me.lblDay.Name = "lblDay"
        Me.lblDay.Size = New System.Drawing.Size(22, 17)
        Me.lblDay.TabIndex = 0
        Me.lblDay.Text = "00"
        Me.lblDay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DayControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.lblDay)
        Me.Cursor = System.Windows.Forms.Cursors.Hand
        Me.Name = "DayControl"
        Me.Size = New System.Drawing.Size(60, 50)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblDay As Label
End Class