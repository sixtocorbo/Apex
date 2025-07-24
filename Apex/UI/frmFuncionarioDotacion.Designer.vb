<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioDotacion
    Inherits System.Windows.Forms.Form

    ' ... (código del diseñador existente) ...

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblItem = New System.Windows.Forms.Label()
        Me.txtItem = New System.Windows.Forms.TextBox()
        Me.lblTalla = New System.Windows.Forms.Label()
        Me.txtTalla = New System.Windows.Forms.TextBox()
        Me.lblObservaciones = New System.Windows.Forms.Label()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        ' lblItem
        '
        Me.lblItem.AutoSize = True
        Me.lblItem.Location = New System.Drawing.Point(12, 15)
        Me.lblItem.Name = "lblItem"
        Me.lblItem.Size = New System.Drawing.Size(35, 17)
        Me.lblItem.TabIndex = 0
        Me.lblItem.Text = "Ítem:"
        '
        ' txtItem
        '
        Me.txtItem.Location = New System.Drawing.Point(110, 12)
        Me.txtItem.Name = "txtItem"
        Me.txtItem.Size = New System.Drawing.Size(262, 22)
        Me.txtItem.TabIndex = 1
        '
        ' lblTalla
        '
        Me.lblTalla.AutoSize = True
        Me.lblTalla.Location = New System.Drawing.Point(12, 43)
        Me.lblTalla.Name = "lblTalla"
        Me.lblTalla.Size = New System.Drawing.Size(43, 17)
        Me.lblTalla.TabIndex = 2
        Me.lblTalla.Text = "Talla:"
        '
        ' txtTalla
        '
        Me.txtTalla.Location = New System.Drawing.Point(110, 40)
        Me.txtTalla.Name = "txtTalla"
        Me.txtTalla.Size = New System.Drawing.Size(262, 22)
        Me.txtTalla.TabIndex = 3
        '
        ' lblObservaciones
        '
        Me.lblObservaciones.AutoSize = True
        Me.lblObservaciones.Location = New System.Drawing.Point(12, 71)
        Me.lblObservaciones.Name = "lblObservaciones"
        Me.lblObservaciones.Size = New System.Drawing.Size(103, 17)
        Me.lblObservaciones.TabIndex = 4
        Me.lblObservaciones.Text = "Observaciones:"
        '
        ' txtObservaciones
        '
        Me.txtObservaciones.Location = New System.Drawing.Point(110, 68)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(262, 80)
        Me.txtObservaciones.TabIndex = 5
        '
        ' btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(297, 164)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(75, 23)
        Me.btnGuardar.TabIndex = 6
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        ' btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(216, 164)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 7
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        ' frmFuncionarioDotacion
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(384, 199)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.txtObservaciones)
        Me.Controls.Add(Me.lblObservaciones)
        Me.Controls.Add(Me.txtTalla)
        Me.Controls.Add(Me.lblTalla)
        Me.Controls.Add(Me.txtItem)
        Me.Controls.Add(Me.lblItem)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFuncionarioDotacion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Dotación"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblItem As Label
    Friend WithEvents txtItem As TextBox
    Friend WithEvents lblTalla As Label
    Friend WithEvents txtTalla As TextBox
    Friend WithEvents lblObservaciones As Label
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
End Class