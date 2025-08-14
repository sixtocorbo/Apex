<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNovedadCrear
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
        Me.PanelPrincipal = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.gbxTexto = New System.Windows.Forms.GroupBox()
        Me.txtTexto = New System.Windows.Forms.TextBox()
        Me.gbxFuncionarios = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.lstFuncionariosSeleccionados = New System.Windows.Forms.ListBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnQuitarFuncionario = New System.Windows.Forms.Button()
        Me.btnAgregarFuncionario = New System.Windows.Forms.Button()
        Me.PanelFecha = New System.Windows.Forms.Panel()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.PanelPrincipal.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.gbxTexto.SuspendLayout()
        Me.gbxFuncionarios.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.PanelFecha.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelPrincipal
        '
        Me.PanelPrincipal.Controls.Add(Me.TableLayoutPanel1)
        Me.PanelPrincipal.Controls.Add(Me.PanelFecha)
        Me.PanelPrincipal.Controls.Add(Me.FlowLayoutPanel2)
        Me.PanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelPrincipal.Location = New System.Drawing.Point(0, 0)
        Me.PanelPrincipal.Name = "PanelPrincipal"
        Me.PanelPrincipal.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelPrincipal.Size = New System.Drawing.Size(784, 461)
        Me.PanelPrincipal.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.gbxTexto, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.gbxFuncionarios, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(10, 55)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(764, 350)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'gbxTexto
        '
        Me.gbxTexto.Controls.Add(Me.txtTexto)
        Me.gbxTexto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxTexto.Location = New System.Drawing.Point(3, 3)
        Me.gbxTexto.Name = "gbxTexto"
        Me.gbxTexto.Padding = New System.Windows.Forms.Padding(10)
        Me.gbxTexto.Size = New System.Drawing.Size(376, 344)
        Me.gbxTexto.TabIndex = 0
        Me.gbxTexto.TabStop = False
        Me.gbxTexto.Text = "Texto de la Novedad"
        '
        'txtTexto
        '
        Me.txtTexto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTexto.Location = New System.Drawing.Point(10, 28)
        Me.txtTexto.Multiline = True
        Me.txtTexto.Name = "txtTexto"
        Me.txtTexto.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTexto.Size = New System.Drawing.Size(356, 306)
        Me.txtTexto.TabIndex = 0
        '
        'gbxFuncionarios
        '
        Me.gbxFuncionarios.Controls.Add(Me.TableLayoutPanel2)
        Me.gbxFuncionarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxFuncionarios.Location = New System.Drawing.Point(385, 3)
        Me.gbxFuncionarios.Name = "gbxFuncionarios"
        Me.gbxFuncionarios.Padding = New System.Windows.Forms.Padding(10)
        Me.gbxFuncionarios.Size = New System.Drawing.Size(376, 344)
        Me.gbxFuncionarios.TabIndex = 1
        Me.gbxFuncionarios.TabStop = False
        Me.gbxFuncionarios.Text = "Funcionarios Involucrados"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.lstFuncionariosSeleccionados, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel1, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(10, 28)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(356, 306)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'lstFuncionariosSeleccionados
        '
        Me.lstFuncionariosSeleccionados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstFuncionariosSeleccionados.FormattingEnabled = True
        Me.lstFuncionariosSeleccionados.ItemHeight = 21
        Me.lstFuncionariosSeleccionados.Location = New System.Drawing.Point(3, 3)
        Me.lstFuncionariosSeleccionados.Name = "lstFuncionariosSeleccionados"
        Me.lstFuncionariosSeleccionados.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstFuncionariosSeleccionados.Size = New System.Drawing.Size(350, 254)
        Me.lstFuncionariosSeleccionados.TabIndex = 0
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.Controls.Add(Me.btnQuitarFuncionario)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAgregarFuncionario)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 263)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(350, 40)
        Me.FlowLayoutPanel1.TabIndex = 1
        '
        'btnQuitarFuncionario
        '
        Me.btnQuitarFuncionario.Location = New System.Drawing.Point(247, 3)
        Me.btnQuitarFuncionario.Name = "btnQuitarFuncionario"
        Me.btnQuitarFuncionario.Size = New System.Drawing.Size(100, 34)
        Me.btnQuitarFuncionario.TabIndex = 0
        Me.btnQuitarFuncionario.Text = "Quitar"
        Me.btnQuitarFuncionario.UseVisualStyleBackColor = True
        '
        'btnAgregarFuncionario
        '
        Me.btnAgregarFuncionario.Location = New System.Drawing.Point(141, 3)
        Me.btnAgregarFuncionario.Name = "btnAgregarFuncionario"
        Me.btnAgregarFuncionario.Size = New System.Drawing.Size(100, 34)
        Me.btnAgregarFuncionario.TabIndex = 1
        Me.btnAgregarFuncionario.Text = "Agregar..."
        Me.btnAgregarFuncionario.UseVisualStyleBackColor = True
        '
        'PanelFecha
        '
        Me.PanelFecha.Controls.Add(Me.dtpFecha)
        Me.PanelFecha.Controls.Add(Me.Label1)
        Me.PanelFecha.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelFecha.Location = New System.Drawing.Point(10, 10)
        Me.PanelFecha.Name = "PanelFecha"
        Me.PanelFecha.Size = New System.Drawing.Size(764, 45)
        Me.PanelFecha.TabIndex = 0
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFecha.Location = New System.Drawing.Point(68, 8)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(121, 29)
        Me.dtpFecha.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 21)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Fecha:"
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.btnGuardar)
        Me.FlowLayoutPanel2.Controls.Add(Me.btnCancelar)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(10, 405)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(764, 46)
        Me.FlowLayoutPanel2.TabIndex = 1
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(658, 3)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(103, 40)
        Me.btnGuardar.TabIndex = 0
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(549, 3)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(103, 40)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmNovedadCrear
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(784, 461)
        Me.Controls.Add(Me.PanelPrincipal)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "frmNovedadCrear"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Crear Nueva Novedad"
        Me.PanelPrincipal.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.gbxTexto.ResumeLayout(False)
        Me.gbxTexto.PerformLayout()
        Me.gbxFuncionarios.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.PanelFecha.ResumeLayout(False)
        Me.PanelFecha.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelPrincipal As Panel
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents PanelFecha As Panel
    Friend WithEvents dtpFecha As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents gbxTexto As GroupBox
    Friend WithEvents txtTexto As TextBox
    Friend WithEvents gbxFuncionarios As GroupBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents lstFuncionariosSeleccionados As ListBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnQuitarFuncionario As Button
    Friend WithEvents btnAgregarFuncionario As Button
End Class