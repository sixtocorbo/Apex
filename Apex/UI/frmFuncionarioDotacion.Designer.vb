<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioDotacion
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblItem = New System.Windows.Forms.Label()
        Me.cboItem = New System.Windows.Forms.ComboBox()
        Me.lblTalla = New System.Windows.Forms.Label()
        Me.txtTalla = New System.Windows.Forms.TextBox()
        Me.lblObservaciones = New System.Windows.Forms.Label()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.MainLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.MainLayout.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblItem
        '
        Me.lblItem.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblItem.AutoSize = True
        Me.lblItem.Location = New System.Drawing.Point(3, 8)
        Me.lblItem.Name = "lblItem"
        Me.lblItem.Size = New System.Drawing.Size(45, 20)
        Me.lblItem.TabIndex = 0
        Me.lblItem.Text = "Ítem:"
        '
        'cboItem
        '
        Me.cboItem.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboItem.FormattingEnabled = True
        Me.cboItem.Location = New System.Drawing.Point(127, 4)
        Me.cboItem.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboItem.Name = "cboItem"
        Me.cboItem.Size = New System.Drawing.Size(292, 28)
        Me.cboItem.TabIndex = 0
        '
        'lblTalla
        '
        Me.lblTalla.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblTalla.AutoSize = True
        Me.lblTalla.Location = New System.Drawing.Point(3, 44)
        Me.lblTalla.Name = "lblTalla"
        Me.lblTalla.Size = New System.Drawing.Size(46, 20)
        Me.lblTalla.TabIndex = 2
        Me.lblTalla.Text = "Talla:"
        '
        'txtTalla
        '
        Me.txtTalla.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTalla.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTalla.Location = New System.Drawing.Point(127, 40)
        Me.txtTalla.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtTalla.Name = "txtTalla"
        Me.txtTalla.Size = New System.Drawing.Size(292, 26)
        Me.txtTalla.TabIndex = 1
        '
        'lblObservaciones
        '
        Me.lblObservaciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblObservaciones.AutoSize = True
        Me.lblObservaciones.Location = New System.Drawing.Point(3, 74)
        Me.lblObservaciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 0)
        Me.lblObservaciones.Name = "lblObservaciones"
        Me.lblObservaciones.Size = New System.Drawing.Size(118, 20)
        Me.lblObservaciones.TabIndex = 4
        Me.lblObservaciones.Text = "Observaciones:"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtObservaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtObservaciones.Location = New System.Drawing.Point(127, 74)
        Me.txtObservaciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObservaciones.Size = New System.Drawing.Size(292, 102)
        Me.txtObservaciones.TabIndex = 2
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnGuardar.Location = New System.Drawing.Point(324, 184)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(95, 38)
        Me.btnGuardar.TabIndex = 3
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'MainLayout
        '
        Me.MainLayout.ColumnCount = 2
        Me.MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.Controls.Add(Me.lblItem, 0, 0)
        Me.MainLayout.Controls.Add(Me.btnGuardar, 1, 3)
        Me.MainLayout.Controls.Add(Me.cboItem, 1, 0)
        Me.MainLayout.Controls.Add(Me.txtObservaciones, 1, 2)
        Me.MainLayout.Controls.Add(Me.lblTalla, 0, 1)
        Me.MainLayout.Controls.Add(Me.lblObservaciones, 0, 2)
        Me.MainLayout.Controls.Add(Me.txtTalla, 1, 1)
        Me.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainLayout.Location = New System.Drawing.Point(10, 10)
        Me.MainLayout.Name = "MainLayout"
        Me.MainLayout.RowCount = 4
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.Size = New System.Drawing.Size(422, 226)
        Me.MainLayout.TabIndex = 7
        '
        'frmFuncionarioDotacion
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(442, 246)
        Me.Controls.Add(Me.MainLayout)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFuncionarioDotacion"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Dotación"
        Me.MainLayout.ResumeLayout(False)
        Me.MainLayout.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblItem As Label
    Friend WithEvents cboItem As ComboBox
    Friend WithEvents lblTalla As Label
    Friend WithEvents txtTalla As TextBox
    Friend WithEvents lblObservaciones As Label
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents btnGuardar As Button
    Friend WithEvents MainLayout As TableLayoutPanel
End Class