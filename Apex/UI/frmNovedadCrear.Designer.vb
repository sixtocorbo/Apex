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
        Me.TabControlMain = New System.Windows.Forms.TabControl()
        Me.TabPageNovedad = New System.Windows.Forms.TabPage()
        Me.txtTexto = New System.Windows.Forms.TextBox()
        Me.TabPageFuncionarios = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.lstFuncionariosSeleccionados = New System.Windows.Forms.ListBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnQuitarFuncionario = New System.Windows.Forms.Button()
        Me.btnAgregarFuncionario = New System.Windows.Forms.Button()
        Me.TabPageFotos = New System.Windows.Forms.TabPage()
        Me.flpFotos = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel3 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnEliminarFoto = New System.Windows.Forms.Button()
        Me.btnAgregarFoto = New System.Windows.Forms.Button()
        Me.PanelFecha = New System.Windows.Forms.Panel()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.PanelPrincipal.SuspendLayout()
        Me.TabControlMain.SuspendLayout()
        Me.TabPageNovedad.SuspendLayout()
        Me.TabPageFuncionarios.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.TabPageFotos.SuspendLayout()
        Me.FlowLayoutPanel3.SuspendLayout()
        Me.PanelFecha.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelPrincipal
        '
        Me.PanelPrincipal.Controls.Add(Me.TabControlMain)
        Me.PanelPrincipal.Controls.Add(Me.PanelFecha)
        Me.PanelPrincipal.Controls.Add(Me.FlowLayoutPanel2)
        Me.PanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelPrincipal.Location = New System.Drawing.Point(0, 0)
        Me.PanelPrincipal.Name = "PanelPrincipal"
        Me.PanelPrincipal.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelPrincipal.Size = New System.Drawing.Size(784, 461)
        Me.PanelPrincipal.TabIndex = 0
        '
        'TabControlMain
        '
        Me.TabControlMain.Controls.Add(Me.TabPageNovedad)
        Me.TabControlMain.Controls.Add(Me.TabPageFuncionarios)
        Me.TabControlMain.Controls.Add(Me.TabPageFotos)
        Me.TabControlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlMain.Location = New System.Drawing.Point(10, 55)
        Me.TabControlMain.Name = "TabControlMain"
        Me.TabControlMain.SelectedIndex = 0
        Me.TabControlMain.Size = New System.Drawing.Size(764, 350)
        Me.TabControlMain.TabIndex = 2
        '
        'TabPageNovedad
        '
        Me.TabPageNovedad.Controls.Add(Me.txtTexto)
        Me.TabPageNovedad.Location = New System.Drawing.Point(4, 37)
        Me.TabPageNovedad.Name = "TabPageNovedad"
        Me.TabPageNovedad.Padding = New System.Windows.Forms.Padding(10)
        Me.TabPageNovedad.Size = New System.Drawing.Size(756, 309)
        Me.TabPageNovedad.TabIndex = 0
        Me.TabPageNovedad.Text = "Novedad"
        Me.TabPageNovedad.UseVisualStyleBackColor = True
        '
        'txtTexto
        '
        Me.txtTexto.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtTexto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTexto.Location = New System.Drawing.Point(10, 10)
        Me.txtTexto.Multiline = True
        Me.txtTexto.Name = "txtTexto"
        Me.txtTexto.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTexto.Size = New System.Drawing.Size(736, 289)
        Me.txtTexto.TabIndex = 0
        '
        'TabPageFuncionarios
        '
        Me.TabPageFuncionarios.Controls.Add(Me.TableLayoutPanel2)
        Me.TabPageFuncionarios.Location = New System.Drawing.Point(4, 37)
        Me.TabPageFuncionarios.Name = "TabPageFuncionarios"
        Me.TabPageFuncionarios.Padding = New System.Windows.Forms.Padding(10)
        Me.TabPageFuncionarios.Size = New System.Drawing.Size(756, 309)
        Me.TabPageFuncionarios.TabIndex = 1
        Me.TabPageFuncionarios.Text = "Funcionarios"
        Me.TabPageFuncionarios.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.lstFuncionariosSeleccionados, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel1, 0, 1)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(10, 10)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(736, 289)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'lstFuncionariosSeleccionados
        '
        Me.lstFuncionariosSeleccionados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstFuncionariosSeleccionados.FormattingEnabled = True
        Me.lstFuncionariosSeleccionados.ItemHeight = 28
        Me.lstFuncionariosSeleccionados.Location = New System.Drawing.Point(3, 3)
        Me.lstFuncionariosSeleccionados.Name = "lstFuncionariosSeleccionados"
        Me.lstFuncionariosSeleccionados.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstFuncionariosSeleccionados.Size = New System.Drawing.Size(730, 237)
        Me.lstFuncionariosSeleccionados.TabIndex = 0
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.Controls.Add(Me.btnQuitarFuncionario)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAgregarFuncionario)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 246)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(730, 40)
        Me.FlowLayoutPanel1.TabIndex = 1
        '
        'btnQuitarFuncionario
        '
        Me.btnQuitarFuncionario.Location = New System.Drawing.Point(627, 3)
        Me.btnQuitarFuncionario.Name = "btnQuitarFuncionario"
        Me.btnQuitarFuncionario.Size = New System.Drawing.Size(100, 34)
        Me.btnQuitarFuncionario.TabIndex = 0
        Me.btnQuitarFuncionario.Text = "Quitar"
        Me.btnQuitarFuncionario.UseVisualStyleBackColor = True
        '
        'btnAgregarFuncionario
        '
        Me.btnAgregarFuncionario.Location = New System.Drawing.Point(521, 3)
        Me.btnAgregarFuncionario.Name = "btnAgregarFuncionario"
        Me.btnAgregarFuncionario.Size = New System.Drawing.Size(100, 34)
        Me.btnAgregarFuncionario.TabIndex = 1
        Me.btnAgregarFuncionario.Text = "Agregar..."
        Me.btnAgregarFuncionario.UseVisualStyleBackColor = True
        '
        'TabPageFotos
        '
        Me.TabPageFotos.Controls.Add(Me.flpFotos)
        Me.TabPageFotos.Controls.Add(Me.FlowLayoutPanel3)
        Me.TabPageFotos.Location = New System.Drawing.Point(4, 37)
        Me.TabPageFotos.Name = "TabPageFotos"
        Me.TabPageFotos.Padding = New System.Windows.Forms.Padding(10)
        Me.TabPageFotos.Size = New System.Drawing.Size(756, 309)
        Me.TabPageFotos.TabIndex = 2
        Me.TabPageFotos.Text = "Fotos"
        Me.TabPageFotos.UseVisualStyleBackColor = True
        '
        'flpFotos
        '
        Me.flpFotos.AutoScroll = True
        Me.flpFotos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpFotos.Location = New System.Drawing.Point(10, 10)
        Me.flpFotos.Name = "flpFotos"
        Me.flpFotos.Size = New System.Drawing.Size(736, 243)
        Me.flpFotos.TabIndex = 3
        '
        'FlowLayoutPanel3
        '
        Me.FlowLayoutPanel3.AutoSize = True
        Me.FlowLayoutPanel3.Controls.Add(Me.btnEliminarFoto)
        Me.FlowLayoutPanel3.Controls.Add(Me.btnAgregarFoto)
        Me.FlowLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel3.Location = New System.Drawing.Point(10, 253)
        Me.FlowLayoutPanel3.Name = "FlowLayoutPanel3"
        Me.FlowLayoutPanel3.Size = New System.Drawing.Size(736, 46)
        Me.FlowLayoutPanel3.TabIndex = 2
        '
        'btnEliminarFoto
        '
        Me.btnEliminarFoto.Location = New System.Drawing.Point(633, 3)
        Me.btnEliminarFoto.Name = "btnEliminarFoto"
        Me.btnEliminarFoto.Size = New System.Drawing.Size(100, 40)
        Me.btnEliminarFoto.TabIndex = 1
        Me.btnEliminarFoto.Text = "Eliminar"
        Me.btnEliminarFoto.UseVisualStyleBackColor = True
        '
        'btnAgregarFoto
        '
        Me.btnAgregarFoto.Location = New System.Drawing.Point(527, 3)
        Me.btnAgregarFoto.Name = "btnAgregarFoto"
        Me.btnAgregarFoto.Size = New System.Drawing.Size(100, 40)
        Me.btnAgregarFoto.TabIndex = 0
        Me.btnAgregarFoto.Text = "Agregar..."
        Me.btnAgregarFoto.UseVisualStyleBackColor = True
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
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(68, 8)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(121, 33)
        Me.dtpFecha.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 28)
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
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 28.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(784, 461)
        Me.Controls.Add(Me.PanelPrincipal)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MinimumSize = New System.Drawing.Size(600, 400)
        Me.Name = "frmNovedadCrear"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Crear Nueva Novedad"
        Me.PanelPrincipal.ResumeLayout(False)
        Me.TabControlMain.ResumeLayout(False)
        Me.TabPageNovedad.ResumeLayout(False)
        Me.TabPageNovedad.PerformLayout()
        Me.TabPageFuncionarios.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.TabPageFotos.ResumeLayout(False)
        Me.TabPageFotos.PerformLayout()
        Me.FlowLayoutPanel3.ResumeLayout(False)
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
    Friend WithEvents TabControlMain As TabControl
    Friend WithEvents TabPageNovedad As TabPage
    Friend WithEvents TabPageFuncionarios As TabPage
    Friend WithEvents txtTexto As TextBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents lstFuncionariosSeleccionados As ListBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnQuitarFuncionario As Button
    Friend WithEvents btnAgregarFuncionario As Button
    Friend WithEvents TabPageFotos As TabPage
    Friend WithEvents flpFotos As FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel3 As FlowLayoutPanel
    Friend WithEvents btnEliminarFoto As Button
    Friend WithEvents btnAgregarFoto As Button
End Class