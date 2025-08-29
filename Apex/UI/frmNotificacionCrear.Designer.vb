<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNotificacionCrear
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
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.cboTipoNotificacion = New System.Windows.Forms.ComboBox()
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.dtpFechaProgramada = New System.Windows.Forms.DateTimePicker()
        Me.lblMedio = New System.Windows.Forms.Label()
        Me.txtMedio = New System.Windows.Forms.TextBox()
        Me.lblDocumento = New System.Windows.Forms.Label()
        Me.txtDocumento = New System.Windows.Forms.TextBox()
        Me.lblExpMinisterial = New System.Windows.Forms.Label()
        Me.txtExpMinisterial = New System.Windows.Forms.TextBox()
        Me.lblExpINR = New System.Windows.Forms.Label()
        Me.txtExpINR = New System.Windows.Forms.TextBox()
        Me.lblOficina = New System.Windows.Forms.Label()
        Me.txtOficina = New System.Windows.Forms.TextBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.lblFuncionario, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.cboFuncionario, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lblTipo, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.cboTipoNotificacion, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblFecha, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.dtpFechaProgramada, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblMedio, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.txtMedio, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblDocumento, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.txtDocumento, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.lblExpMinisterial, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.txtExpMinisterial, 1, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.lblExpINR, 0, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.txtExpINR, 1, 6)
        Me.TableLayoutPanel1.Controls.Add(Me.lblOficina, 0, 7)
        Me.TableLayoutPanel1.Controls.Add(Me.txtOficina, 1, 7)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(10, 10)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 9
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(464, 342)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'lblFuncionario
        '
        Me.lblFuncionario.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFuncionario.AutoSize = True
        Me.lblFuncionario.Location = New System.Drawing.Point(28, 2)
        Me.lblFuncionario.Name = "lblFuncionario"
        Me.lblFuncionario.Size = New System.Drawing.Size(108, 25)
        Me.lblFuncionario.TabIndex = 0
        Me.lblFuncionario.Text = "Funcionario:"
        '
        'cboFuncionario
        '
        Me.cboFuncionario.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFuncionario.FormattingEnabled = True
        Me.cboFuncionario.Location = New System.Drawing.Point(142, 3)
        Me.cboFuncionario.Name = "cboFuncionario"
        Me.cboFuncionario.Size = New System.Drawing.Size(319, 33)
        Me.cboFuncionario.TabIndex = 1
        '
        'lblTipo
        '
        Me.lblTipo.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(85, 32)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(51, 25)
        Me.lblTipo.TabIndex = 2
        Me.lblTipo.Text = "Tipo:"
        '
        'cboTipoNotificacion
        '
        Me.cboTipoNotificacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboTipoNotificacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoNotificacion.FormattingEnabled = True
        Me.cboTipoNotificacion.Location = New System.Drawing.Point(142, 33)
        Me.cboTipoNotificacion.Name = "cboTipoNotificacion"
        Me.cboTipoNotificacion.Size = New System.Drawing.Size(319, 33)
        Me.cboTipoNotificacion.TabIndex = 3
        '
        'lblFecha
        '
        Me.lblFecha.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(28, 62)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(108, 25)
        Me.lblFecha.TabIndex = 4
        Me.lblFecha.Text = "Fecha Prog.:"
        '
        'dtpFechaProgramada
        '
        Me.dtpFechaProgramada.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaProgramada.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaProgramada.Location = New System.Drawing.Point(142, 63)
        Me.dtpFechaProgramada.Name = "dtpFechaProgramada"
        Me.dtpFechaProgramada.Size = New System.Drawing.Size(120, 31)
        Me.dtpFechaProgramada.TabIndex = 5
        '
        'lblMedio
        '
        Me.lblMedio.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblMedio.AutoSize = True
        Me.lblMedio.Location = New System.Drawing.Point(79, 117)
        Me.lblMedio.Name = "lblMedio"
        Me.lblMedio.Size = New System.Drawing.Size(57, 25)
        Me.lblMedio.TabIndex = 6
        Me.lblMedio.Text = "Texto:"
        '
        'txtMedio
        '
        Me.txtMedio.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtMedio.Location = New System.Drawing.Point(142, 93)
        Me.txtMedio.Multiline = True
        Me.txtMedio.Name = "txtMedio"
        Me.txtMedio.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMedio.Size = New System.Drawing.Size(319, 74)
        Me.txtMedio.TabIndex = 7
        '
        'lblDocumento
        '
        Me.lblDocumento.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblDocumento.AutoSize = True
        Me.lblDocumento.Location = New System.Drawing.Point(26, 172)
        Me.lblDocumento.Name = "lblDocumento"
        Me.lblDocumento.Size = New System.Drawing.Size(110, 25)
        Me.lblDocumento.TabIndex = 8
        Me.lblDocumento.Text = "Documento:"
        '
        'txtDocumento
        '
        Me.txtDocumento.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocumento.Location = New System.Drawing.Point(142, 173)
        Me.txtDocumento.Name = "txtDocumento"
        Me.txtDocumento.Size = New System.Drawing.Size(319, 31)
        Me.txtDocumento.TabIndex = 9
        '
        'lblExpMinisterial
        '
        Me.lblExpMinisterial.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblExpMinisterial.AutoSize = True
        Me.lblExpMinisterial.Location = New System.Drawing.Point(3, 202)
        Me.lblExpMinisterial.Name = "lblExpMinisterial"
        Me.lblExpMinisterial.Size = New System.Drawing.Size(133, 25)
        Me.lblExpMinisterial.TabIndex = 10
        Me.lblExpMinisterial.Text = "Exp. Ministerial:"
        '
        'txtExpMinisterial
        '
        Me.txtExpMinisterial.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExpMinisterial.Location = New System.Drawing.Point(142, 203)
        Me.txtExpMinisterial.Name = "txtExpMinisterial"
        Me.txtExpMinisterial.Size = New System.Drawing.Size(319, 31)
        Me.txtExpMinisterial.TabIndex = 11
        '
        'lblExpINR
        '
        Me.lblExpINR.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblExpINR.AutoSize = True
        Me.lblExpINR.Location = New System.Drawing.Point(54, 232)
        Me.lblExpINR.Name = "lblExpINR"
        Me.lblExpINR.Size = New System.Drawing.Size(82, 25)
        Me.lblExpINR.TabIndex = 12
        Me.lblExpINR.Text = "Exp. INR:"
        '
        'txtExpINR
        '
        Me.txtExpINR.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExpINR.Location = New System.Drawing.Point(142, 233)
        Me.txtExpINR.Name = "txtExpINR"
        Me.txtExpINR.Size = New System.Drawing.Size(319, 31)
        Me.txtExpINR.TabIndex = 13
        '
        'lblOficina
        '
        Me.lblOficina.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblOficina.AutoSize = True
        Me.lblOficina.Location = New System.Drawing.Point(65, 262)
        Me.lblOficina.Name = "lblOficina"
        Me.lblOficina.Size = New System.Drawing.Size(71, 25)
        Me.lblOficina.TabIndex = 14
        Me.lblOficina.Text = "Oficina:"
        '
        'txtOficina
        '
        Me.txtOficina.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOficina.Location = New System.Drawing.Point(142, 263)
        Me.txtOficina.Name = "txtOficina"
        Me.txtOficina.Size = New System.Drawing.Size(319, 31)
        Me.txtOficina.TabIndex = 15
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnGuardar)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCancelar)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(10, 352)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(464, 40)
        Me.FlowLayoutPanel1.TabIndex = 1
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(348, 3)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(113, 34)
        Me.btnGuardar.TabIndex = 0
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(236, 3)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(106, 34)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmNotificacionCrear
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(484, 402)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmNotificacionCrear"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Nueva Notificación"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents lblFuncionario As Label
    Friend WithEvents cboFuncionario As ComboBox
    Friend WithEvents lblTipo As Label
    Friend WithEvents cboTipoNotificacion As ComboBox
    Friend WithEvents lblFecha As Label
    Friend WithEvents dtpFechaProgramada As DateTimePicker
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents lblMedio As Label
    Friend WithEvents txtMedio As TextBox
    Friend WithEvents lblDocumento As Label
    Friend WithEvents txtDocumento As TextBox
    Friend WithEvents lblExpMinisterial As Label
    Friend WithEvents txtExpMinisterial As TextBox
    Friend WithEvents lblExpINR As Label
    Friend WithEvents txtExpINR As TextBox
    Friend WithEvents lblOficina As Label
    Friend WithEvents txtOficina As TextBox
End Class