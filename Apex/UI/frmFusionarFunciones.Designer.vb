<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFusionarFunciones
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblDescripcion = New System.Windows.Forms.Label()
        Me.pnlBusqueda = New System.Windows.Forms.TableLayoutPanel()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.clbFunciones = New System.Windows.Forms.CheckedListBox()
        Me.pnlPrincipal = New System.Windows.Forms.TableLayoutPanel()
        Me.lblPrincipal = New System.Windows.Forms.Label()
        Me.cboFuncionPrincipal = New System.Windows.Forms.ComboBox()
        Me.pnlNombre = New System.Windows.Forms.TableLayoutPanel()
        Me.lblNombreFinal = New System.Windows.Forms.Label()
        Me.txtNombreFinal = New System.Windows.Forms.TextBox()
        Me.lblNota = New System.Windows.Forms.Label()
        Me.pnlBotones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.pnlBusqueda.SuspendLayout()
        Me.pnlPrincipal.SuspendLayout()
        Me.pnlNombre.SuspendLayout()
        Me.pnlBotones.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.lblDescripcion, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.pnlBusqueda, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.clbFunciones, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.pnlPrincipal, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.pnlNombre, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.lblNota, 0, 5)
        Me.TableLayoutPanel1.Controls.Add(Me.pnlBotones, 0, 6)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(10, 10)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 7
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(544, 540)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblDescripcion.Location = New System.Drawing.Point(3, 0)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Padding = New System.Windows.Forms.Padding(0, 0, 0, 5)
        Me.lblDescripcion.Size = New System.Drawing.Size(538, 45)
        Me.lblDescripcion.TabIndex = 0
        Me.lblDescripcion.Text = "Seleccione las funciones que desea fusionar y elija cuál quedará como principal." & _
            Environment.NewLine & "Las referencias existentes se actualizarán automáticamente."
        '
        'pnlBusqueda
        '
        Me.pnlBusqueda.ColumnCount = 2
        Me.pnlBusqueda.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.pnlBusqueda.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.pnlBusqueda.Controls.Add(Me.lblBuscar, 0, 0)
        Me.pnlBusqueda.Controls.Add(Me.txtBuscar, 1, 0)
        Me.pnlBusqueda.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlBusqueda.Location = New System.Drawing.Point(3, 48)
        Me.pnlBusqueda.Name = "pnlBusqueda"
        Me.pnlBusqueda.RowCount = 1
        Me.pnlBusqueda.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlBusqueda.Size = New System.Drawing.Size(538, 38)
        Me.pnlBusqueda.TabIndex = 1
        '
        'lblBuscar
        '
        Me.lblBuscar.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Location = New System.Drawing.Point(3, 6)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(69, 25)
        Me.lblBuscar.TabIndex = 0
        Me.lblBuscar.Text = "Buscar:"
        '
        'txtBuscar
        '
        Me.txtBuscar.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBuscar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBuscar.Location = New System.Drawing.Point(78, 3)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(457, 31)
        Me.txtBuscar.TabIndex = 1
        '
        'clbFunciones
        '
        Me.clbFunciones.CheckOnClick = True
        Me.clbFunciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clbFunciones.FormattingEnabled = True
        Me.clbFunciones.Location = New System.Drawing.Point(3, 92)
        Me.clbFunciones.Name = "clbFunciones"
        Me.clbFunciones.Size = New System.Drawing.Size(538, 298)
        Me.clbFunciones.TabIndex = 2
        '
        'pnlPrincipal
        '
        Me.pnlPrincipal.ColumnCount = 2
        Me.pnlPrincipal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.pnlPrincipal.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.pnlPrincipal.Controls.Add(Me.lblPrincipal, 0, 0)
        Me.pnlPrincipal.Controls.Add(Me.cboFuncionPrincipal, 1, 0)
        Me.pnlPrincipal.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlPrincipal.Location = New System.Drawing.Point(3, 396)
        Me.pnlPrincipal.Name = "pnlPrincipal"
        Me.pnlPrincipal.RowCount = 1
        Me.pnlPrincipal.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlPrincipal.Size = New System.Drawing.Size(538, 38)
        Me.pnlPrincipal.TabIndex = 3
        '
        'lblPrincipal
        '
        Me.lblPrincipal.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblPrincipal.AutoSize = True
        Me.lblPrincipal.Location = New System.Drawing.Point(3, 6)
        Me.lblPrincipal.Name = "lblPrincipal"
        Me.lblPrincipal.Size = New System.Drawing.Size(160, 25)
        Me.lblPrincipal.TabIndex = 0
        Me.lblPrincipal.Text = "Función que quedará:"
        '
        'cboFuncionPrincipal
        '
        Me.cboFuncionPrincipal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboFuncionPrincipal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFuncionPrincipal.FormattingEnabled = True
        Me.cboFuncionPrincipal.Location = New System.Drawing.Point(169, 3)
        Me.cboFuncionPrincipal.Name = "cboFuncionPrincipal"
        Me.cboFuncionPrincipal.Size = New System.Drawing.Size(366, 33)
        Me.cboFuncionPrincipal.TabIndex = 1
        '
        'pnlNombre
        '
        Me.pnlNombre.ColumnCount = 2
        Me.pnlNombre.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.pnlNombre.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.pnlNombre.Controls.Add(Me.lblNombreFinal, 0, 0)
        Me.pnlNombre.Controls.Add(Me.txtNombreFinal, 1, 0)
        Me.pnlNombre.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlNombre.Location = New System.Drawing.Point(3, 440)
        Me.pnlNombre.Name = "pnlNombre"
        Me.pnlNombre.RowCount = 1
        Me.pnlNombre.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlNombre.Size = New System.Drawing.Size(538, 38)
        Me.pnlNombre.TabIndex = 4
        '
        'lblNombreFinal
        '
        Me.lblNombreFinal.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblNombreFinal.AutoSize = True
        Me.lblNombreFinal.Location = New System.Drawing.Point(3, 6)
        Me.lblNombreFinal.Name = "lblNombreFinal"
        Me.lblNombreFinal.Size = New System.Drawing.Size(128, 25)
        Me.lblNombreFinal.TabIndex = 0
        Me.lblNombreFinal.Text = "Nombre final:"
        '
        'txtNombreFinal
        '
        Me.txtNombreFinal.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNombreFinal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtNombreFinal.Location = New System.Drawing.Point(137, 3)
        Me.txtNombreFinal.Name = "txtNombreFinal"
        Me.txtNombreFinal.Size = New System.Drawing.Size(398, 31)
        Me.txtNombreFinal.TabIndex = 1
        '
        'lblNota
        '
        Me.lblNota.AutoSize = True
        Me.lblNota.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblNota.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblNota.Location = New System.Drawing.Point(3, 481)
        Me.lblNota.Margin = New System.Windows.Forms.Padding(3, 3, 3, 3)
        Me.lblNota.Name = "lblNota"
        Me.lblNota.Size = New System.Drawing.Size(538, 25)
        Me.lblNota.TabIndex = 5
        Me.lblNota.Text = "Las funciones seleccionadas que no queden como destino serán eliminadas."
        '
        'pnlBotones
        '
        Me.pnlBotones.Controls.Add(Me.btnAceptar)
        Me.pnlBotones.Controls.Add(Me.btnCancelar)
        Me.pnlBotones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBotones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.pnlBotones.Location = New System.Drawing.Point(3, 510)
        Me.pnlBotones.Name = "pnlBotones"
        Me.pnlBotones.Padding = New System.Windows.Forms.Padding(0, 10, 0, 0)
        Me.pnlBotones.Size = New System.Drawing.Size(538, 27)
        Me.pnlBotones.TabIndex = 6
        '
        'btnAceptar
        '
        Me.btnAceptar.AutoSize = True
        Me.btnAceptar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnAceptar.Location = New System.Drawing.Point(445, 13)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(90, 35)
        Me.btnAceptar.TabIndex = 0
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.AutoSize = True
        Me.btnCancelar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(349, 13)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(90, 35)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmFusionarFunciones
        '
        Me.AcceptButton = Me.btnAceptar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(564, 560)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFusionarFunciones"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Fusionar funciones"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.pnlBusqueda.ResumeLayout(False)
        Me.pnlBusqueda.PerformLayout()
        Me.pnlPrincipal.ResumeLayout(False)
        Me.pnlPrincipal.PerformLayout()
        Me.pnlNombre.ResumeLayout(False)
        Me.pnlNombre.PerformLayout()
        Me.pnlBotones.ResumeLayout(False)
        Me.pnlBotones.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblDescripcion As System.Windows.Forms.Label
    Friend WithEvents pnlBusqueda As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblBuscar As System.Windows.Forms.Label
    Friend WithEvents txtBuscar As System.Windows.Forms.TextBox
    Friend WithEvents clbFunciones As System.Windows.Forms.CheckedListBox
    Friend WithEvents pnlPrincipal As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblPrincipal As System.Windows.Forms.Label
    Friend WithEvents cboFuncionPrincipal As System.Windows.Forms.ComboBox
    Friend WithEvents pnlNombre As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblNombreFinal As System.Windows.Forms.Label
    Friend WithEvents txtNombreFinal As System.Windows.Forms.TextBox
    Friend WithEvents lblNota As System.Windows.Forms.Label
    Friend WithEvents pnlBotones As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents btnCancelar As System.Windows.Forms.Button
End Class
