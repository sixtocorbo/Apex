<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSancionCrear
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblFuncionario = New System.Windows.Forms.Label()
        Me.cboFuncionario = New System.Windows.Forms.ComboBox()
        Me.lblFechaDesde = New System.Windows.Forms.Label()
        Me.dtpFechaDesde = New System.Windows.Forms.DateTimePicker()
        Me.lblFechaHasta = New System.Windows.Forms.Label()
        Me.dtpFechaHasta = New System.Windows.Forms.DateTimePicker()
        Me.chkFechaHasta = New System.Windows.Forms.CheckBox()
        Me.lblResolucion = New System.Windows.Forms.Label()
        Me.txtResolucion = New System.Windows.Forms.TextBox()
        Me.lblTipoSancion = New System.Windows.Forms.Label()
        Me.cmbTipoSancion = New System.Windows.Forms.ComboBox()
        Me.lblObservaciones = New System.Windows.Forms.Label()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 3
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.lblFuncionario, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.cboFuncionario, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblFechaDesde, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.dtpFechaDesde, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblFechaHasta, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.dtpFechaHasta, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.chkFechaHasta, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblResolucion, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txtResolucion, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblTipoSancion, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.cmbTipoSancion, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.lblObservaciones, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.txtObservaciones, 1, 5)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(10, 10)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 7
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(524, 338)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'lblFuncionario
        '
        Me.lblFuncionario.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFuncionario.AutoSize = True
        Me.lblFuncionario.Location = New System.Drawing.Point(27, 2)
        Me.lblFuncionario.Name = "lblFuncionario"
        Me.lblFuncionario.Size = New System.Drawing.Size(108, 25)
        Me.lblFuncionario.TabIndex = 0
        Me.lblFuncionario.Text = "Funcionario:"
        '
        'cboFuncionario
        '
        Me.cboFuncionario.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.cboFuncionario, 2)
        Me.cboFuncionario.FormattingEnabled = True
        Me.cboFuncionario.Location = New System.Drawing.Point(141, 3)
        Me.cboFuncionario.Name = "cboFuncionario"
        Me.cboFuncionario.Size = New System.Drawing.Size(380, 33)
        Me.cboFuncionario.TabIndex = 1
        '
        'lblFechaDesde
        '
        Me.lblFechaDesde.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFechaDesde.AutoSize = True
        Me.lblFechaDesde.Location = New System.Drawing.Point(19, 32)
        Me.lblFechaDesde.Name = "lblFechaDesde"
        Me.lblFechaDesde.Size = New System.Drawing.Size(116, 25)
        Me.lblFechaDesde.TabIndex = 2
        Me.lblFechaDesde.Text = "Fecha Desde:"
        '
        'dtpFechaDesde
        '
        Me.dtpFechaDesde.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaDesde.Location = New System.Drawing.Point(141, 33)
        Me.dtpFechaDesde.Name = "dtpFechaDesde"
        Me.dtpFechaDesde.Size = New System.Drawing.Size(120, 31)
        Me.dtpFechaDesde.TabIndex = 3
        '
        'lblFechaHasta
        '
        Me.lblFechaHasta.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFechaHasta.AutoSize = True
        Me.lblFechaHasta.Location = New System.Drawing.Point(24, 62)
        Me.lblFechaHasta.Name = "lblFechaHasta"
        Me.lblFechaHasta.Size = New System.Drawing.Size(111, 25)
        Me.lblFechaHasta.TabIndex = 4
        Me.lblFechaHasta.Text = "Fecha Hasta:"
        '
        'dtpFechaHasta
        '
        Me.dtpFechaHasta.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaHasta.Location = New System.Drawing.Point(141, 63)
        Me.dtpFechaHasta.Name = "dtpFechaHasta"
        Me.dtpFechaHasta.Size = New System.Drawing.Size(120, 31)
        Me.dtpFechaHasta.TabIndex = 5
        '
        'chkFechaHasta
        '
        Me.chkFechaHasta.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.chkFechaHasta.AutoSize = True
        Me.chkFechaHasta.Location = New System.Drawing.Point(387, 63)
        Me.chkFechaHasta.Name = "chkFechaHasta"
        Me.chkFechaHasta.Size = New System.Drawing.Size(134, 24)
        Me.chkFechaHasta.TabIndex = 6
        Me.chkFechaHasta.Text = "Sin fecha fin"
        Me.chkFechaHasta.UseVisualStyleBackColor = True
        '
        'lblResolucion
        '
        Me.lblResolucion.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblResolucion.AutoSize = True
        Me.lblResolucion.Location = New System.Drawing.Point(34, 92)
        Me.lblResolucion.Name = "lblResolucion"
        Me.lblResolucion.Size = New System.Drawing.Size(101, 25)
        Me.lblResolucion.TabIndex = 7
        Me.lblResolucion.Text = "Resolución:"
        '
        'txtResolucion
        '
        Me.txtResolucion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtResolucion, 2)
        Me.txtResolucion.Location = New System.Drawing.Point(141, 93)
        Me.txtResolucion.Name = "txtResolucion"
        Me.txtResolucion.Size = New System.Drawing.Size(380, 31)
        Me.txtResolucion.TabIndex = 8
        '
        'lblTipoSancion
        '
        Me.lblTipoSancion.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblTipoSancion.AutoSize = True
        Me.lblTipoSancion.Location = New System.Drawing.Point(84, 122)
        Me.lblTipoSancion.Name = "lblTipoSancion"
        Me.lblTipoSancion.Size = New System.Drawing.Size(51, 25)
        Me.lblTipoSancion.TabIndex = 11
        Me.lblTipoSancion.Text = "Tipo:"
        '
        'cmbTipoSancion
        '
        Me.cmbTipoSancion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.cmbTipoSancion, 2)
        Me.cmbTipoSancion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoSancion.FormattingEnabled = True
        Me.cmbTipoSancion.Location = New System.Drawing.Point(141, 123)
        Me.cmbTipoSancion.Name = "cmbTipoSancion"
        Me.cmbTipoSancion.Size = New System.Drawing.Size(380, 33)
        Me.cmbTipoSancion.TabIndex = 9
        '
        'lblObservaciones
        '
        Me.lblObservaciones.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblObservaciones.AutoSize = True
        Me.lblObservaciones.Location = New System.Drawing.Point(3, 221)
        Me.lblObservaciones.Name = "lblObservaciones"
        Me.lblObservaciones.Size = New System.Drawing.Size(132, 25)
        Me.lblObservaciones.TabIndex = 9
        Me.lblObservaciones.Text = "Observaciones:"
        '
        'txtObservaciones
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.txtObservaciones, 2)
        Me.txtObservaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtObservaciones.Location = New System.Drawing.Point(141, 153)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObservaciones.Size = New System.Drawing.Size(380, 162)
        Me.txtObservaciones.TabIndex = 10
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnGuardar)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCancelar)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(10, 348)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(524, 40)
        Me.FlowLayoutPanel1.TabIndex = 1
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(430, 3)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(91, 34)
        Me.btnGuardar.TabIndex = 0
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(334, 3)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(90, 34)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmSancionCrear
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(544, 398)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmSancionCrear"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Nueva Sanción"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents lblFuncionario As Label
    Friend WithEvents cboFuncionario As ComboBox
    Friend WithEvents lblFechaDesde As Label
    Friend WithEvents dtpFechaDesde As DateTimePicker
    Friend WithEvents lblFechaHasta As Label
    Friend WithEvents dtpFechaHasta As DateTimePicker
    Friend WithEvents chkFechaHasta As CheckBox
    Friend WithEvents lblResolucion As Label
    Friend WithEvents txtResolucion As TextBox
    Friend WithEvents lblObservaciones As Label
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents lblTipoSancion As Label
    Friend WithEvents cmbTipoSancion As ComboBox
End Class