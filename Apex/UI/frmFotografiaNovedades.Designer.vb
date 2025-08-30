<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFotografiaNovedades
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
        Me.pbFotoGrande = New System.Windows.Forms.PictureBox()
        CType(Me.pbFotoGrande, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbFotoGrande
        '
        Me.pbFotoGrande.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbFotoGrande.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbFotoGrande.Location = New System.Drawing.Point(0, 0)
        Me.pbFotoGrande.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.pbFotoGrande.Name = "pbFotoGrande"
        Me.pbFotoGrande.Size = New System.Drawing.Size(882, 701)
        Me.pbFotoGrande.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFotoGrande.TabIndex = 0
        Me.pbFotoGrande.TabStop = False
        '
        'frmFotografiaNovedades
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(882, 701)
        Me.Controls.Add(Me.pbFotoGrande)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimizeBox = False
        Me.Name = "frmFotografiaNovedades"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Visor de Foto"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.pbFotoGrande, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pbFotoGrande As PictureBox
End Class