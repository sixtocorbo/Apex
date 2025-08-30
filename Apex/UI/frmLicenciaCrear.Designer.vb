<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLicenciaCrear
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
        Me.lblTipoLicencia = New System.Windows.Forms.Label()
        Me.cboTipoLicencia = New System.Windows.Forms.ComboBox()
        Me.lblFechaInicio = New System.Windows.Forms.Label()
        Me.dtpFechaInicio = New System.Windows.Forms.DateTimePicker()
        Me.lblFechaFin = New System.Windows.Forms.Label()
        Me.dtpFechaFin = New System.Windows.Forms.DateTimePicker()
        Me.lblEstado = New System.Windows.Forms.Label()
        Me.cboEstado = New System.Windows.Forms.ComboBox()
        Me.lblComentario = New System.Windows.Forms.Label()
        Me.txtComentario = New System.Windows.Forms.TextBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGuardar = New System.Windows.Forms.Button()
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
        Me.TableLayoutPanel1.Controls.Add(Me.lblTipoLicencia, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.cboTipoLicencia, 1, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lblFechaInicio, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.dtpFechaInicio, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.lblFechaFin, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.dtpFechaFin, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.lblEstado, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.cboEstado, 1, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.lblComentario, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.txtComentario, 1, 5)
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
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(464, 282)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'lblFuncionario
        '
        Me.lblFuncionario.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFuncionario.AutoSize = True
        Me.lblFuncionario.Location = New System.Drawing.Point(36, 2)
        Me.lblFuncionario.Name = "lblFuncionario"
        Me.lblFuncionario.Size = New System.Drawing.Size(108, 25)
        Me.lblFuncionario.TabIndex = 0
        Me.lblFuncionario.Text = "Funcionario:"
        '
        'cboFuncionario
        '
        Me.cboFuncionario.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFuncionario.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboFuncionario.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboFuncionario.FormattingEnabled = True
        Me.cboFuncionario.Location = New System.Drawing.Point(150, 3)
        Me.cboFuncionario.Name = "cboFuncionario"
        Me.cboFuncionario.Size = New System.Drawing.Size(311, 33)
        Me.cboFuncionario.TabIndex = 1
        '
        'lblTipoLicencia
        '
        Me.lblTipoLicencia.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblTipoLicencia.AutoSize = True
        Me.lblTipoLicencia.Location = New System.Drawing.Point(3, 32)
        Me.lblTipoLicencia.Name = "lblTipoLicencia"
        Me.lblTipoLicencia.Size = New System.Drawing.Size(141, 25)
        Me.lblTipoLicencia.TabIndex = 2
        Me.lblTipoLicencia.Text = "Tipo de Licencia:"
        '
        'cboTipoLicencia
        '
        Me.cboTipoLicencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboTipoLicencia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoLicencia.FormattingEnabled = True
        Me.cboTipoLicencia.Location = New System.Drawing.Point(150, 33)
        Me.cboTipoLicencia.Name = "cboTipoLicencia"
        Me.cboTipoLicencia.Size = New System.Drawing.Size(311, 33)
        Me.cboTipoLicencia.TabIndex = 3
        '
        'lblFechaInicio
        '
        Me.lblFechaInicio.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFechaInicio.AutoSize = True
        Me.lblFechaInicio.Location = New System.Drawing.Point(36, 62)
        Me.lblFechaInicio.Name = "lblFechaInicio"
        Me.lblFechaInicio.Size = New System.Drawing.Size(108, 25)
        Me.lblFechaInicio.TabIndex = 4
        Me.lblFechaInicio.Text = "Fecha Inicio:"
        '
        'dtpFechaInicio
        '
        Me.dtpFechaInicio.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaInicio.Location = New System.Drawing.Point(150, 63)
        Me.dtpFechaInicio.Name = "dtpFechaInicio"
        Me.dtpFechaInicio.Size = New System.Drawing.Size(133, 31)
        Me.dtpFechaInicio.TabIndex = 5
        '
        'lblFechaFin
        '
        Me.lblFechaFin.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFechaFin.AutoSize = True
        Me.lblFechaFin.Location = New System.Drawing.Point(55, 92)
        Me.lblFechaFin.Name = "lblFechaFin"
        Me.lblFechaFin.Size = New System.Drawing.Size(89, 25)
        Me.lblFechaFin.TabIndex = 6
        Me.lblFechaFin.Text = "Fecha Fin:"
        '
        'dtpFechaFin
        '
        Me.dtpFechaFin.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaFin.Location = New System.Drawing.Point(150, 93)
        Me.dtpFechaFin.Name = "dtpFechaFin"
        Me.dtpFechaFin.Size = New System.Drawing.Size(133, 31)
        Me.dtpFechaFin.TabIndex = 7
        '
        'lblEstado
        '
        Me.lblEstado.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblEstado.AutoSize = True
        Me.lblEstado.Location = New System.Drawing.Point(74, 122)
        Me.lblEstado.Name = "lblEstado"
        Me.lblEstado.Size = New System.Drawing.Size(70, 25)
        Me.lblEstado.TabIndex = 10
        Me.lblEstado.Text = "Estado:"
        '
        'cboEstado
        '
        Me.cboEstado.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEstado.FormattingEnabled = True
        Me.cboEstado.Items.AddRange(New Object() {"Autorizado", "Rechazada", "Anulado", "Pendiente de Autorización"})
        Me.cboEstado.Location = New System.Drawing.Point(150, 123)
        Me.cboEstado.Name = "cboEstado"
        Me.cboEstado.Size = New System.Drawing.Size(311, 33)
        Me.cboEstado.TabIndex = 11
        '
        'lblComentario
        '
        Me.lblComentario.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblComentario.AutoSize = True
        Me.lblComentario.Location = New System.Drawing.Point(35, 193)
        Me.lblComentario.Name = "lblComentario"
        Me.lblComentario.Size = New System.Drawing.Size(109, 25)
        Me.lblComentario.TabIndex = 8
        Me.lblComentario.Text = "Comentario:"
        '
        'txtComentario
        '
        Me.txtComentario.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtComentario.Location = New System.Drawing.Point(150, 153)
        Me.txtComentario.Multiline = True
        Me.txtComentario.Name = "txtComentario"
        Me.txtComentario.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtComentario.Size = New System.Drawing.Size(311, 106)
        Me.txtComentario.TabIndex = 9
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnGuardar)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(10, 292)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(464, 40)
        Me.FlowLayoutPanel1.TabIndex = 1
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(359, 3)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(102, 34)
        Me.btnGuardar.TabIndex = 0
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'frmLicenciaCrear
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 342)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmLicenciaCrear"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Nueva Licencia"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents lblFuncionario As Label
    Friend WithEvents cboFuncionario As ComboBox
    Friend WithEvents lblTipoLicencia As Label
    Friend WithEvents cboTipoLicencia As ComboBox
    Friend WithEvents lblFechaInicio As Label
    Friend WithEvents dtpFechaInicio As DateTimePicker
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnGuardar As Button
    Friend WithEvents lblFechaFin As Label
    Friend WithEvents dtpFechaFin As DateTimePicker
    Friend WithEvents lblComentario As Label
    Friend WithEvents txtComentario As TextBox
    Friend WithEvents lblEstado As Label
    Friend WithEvents cboEstado As ComboBox
End Class