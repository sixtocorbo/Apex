<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUnidadesGenerales
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
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

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TableLayoutPanelMain = New System.Windows.Forms.TableLayoutPanel()
        Me.PanelListado = New System.Windows.Forms.Panel()
        Me.dgvUnidadesGenerales = New System.Windows.Forms.DataGridView()
        Me.PanelBusqueda = New System.Windows.Forms.Panel()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.PanelEdicion = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanelBotones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.TableLayoutPanelMain.SuspendLayout()
        Me.PanelListado.SuspendLayout()
        CType(Me.dgvUnidadesGenerales, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelBusqueda.SuspendLayout()
        Me.PanelEdicion.SuspendLayout()
        Me.FlowLayoutPanelBotones.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanelMain
        '
        Me.TableLayoutPanelMain.ColumnCount = 2
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.0!))
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
        Me.TableLayoutPanelMain.Controls.Add(Me.PanelListado, 0, 0)
        Me.TableLayoutPanelMain.Controls.Add(Me.PanelEdicion, 1, 0)
        Me.TableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMain.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelMain.Name = "TableLayoutPanelMain"
        Me.TableLayoutPanelMain.RowCount = 1
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMain.Size = New System.Drawing.Size(884, 481)
        Me.TableLayoutPanelMain.TabIndex = 0
        '
        'PanelListado
        '
        Me.PanelListado.Controls.Add(Me.dgvUnidadesGenerales)
        Me.PanelListado.Controls.Add(Me.PanelBusqueda)
        Me.PanelListado.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelListado.Location = New System.Drawing.Point(3, 3)
        Me.PanelListado.Name = "PanelListado"
        Me.PanelListado.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelListado.Size = New System.Drawing.Size(479, 475)
        Me.PanelListado.TabIndex = 0
        '
        'dgvUnidadesGenerales
        '
        Me.dgvUnidadesGenerales.AllowUserToAddRows = False
        Me.dgvUnidadesGenerales.AllowUserToDeleteRows = False
        Me.dgvUnidadesGenerales.AllowUserToResizeRows = False
        Me.dgvUnidadesGenerales.BackgroundColor = System.Drawing.Color.White
        Me.dgvUnidadesGenerales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUnidadesGenerales.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvUnidadesGenerales.Location = New System.Drawing.Point(10, 66)
        Me.dgvUnidadesGenerales.MultiSelect = False
        Me.dgvUnidadesGenerales.Name = "dgvUnidadesGenerales"
        Me.dgvUnidadesGenerales.ReadOnly = True
        Me.dgvUnidadesGenerales.RowHeadersVisible = False
        Me.dgvUnidadesGenerales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvUnidadesGenerales.Size = New System.Drawing.Size(459, 399)
        Me.dgvUnidadesGenerales.TabIndex = 1
        '
        'PanelBusqueda
        '
        Me.PanelBusqueda.Controls.Add(Me.txtBuscar)
        Me.PanelBusqueda.Controls.Add(Me.lblBuscar)
        Me.PanelBusqueda.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusqueda.Location = New System.Drawing.Point(10, 10)
        Me.PanelBusqueda.Name = "PanelBusqueda"
        Me.PanelBusqueda.Padding = New System.Windows.Forms.Padding(0, 0, 0, 10)
        Me.PanelBusqueda.Size = New System.Drawing.Size(459, 56)
        Me.PanelBusqueda.TabIndex = 0
        '
        'txtBuscar
        '
        Me.txtBuscar.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtBuscar.Location = New System.Drawing.Point(0, 25)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(459, 26)
        Me.txtBuscar.TabIndex = 1
        '
        'lblBuscar
        '
        Me.lblBuscar.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblBuscar.Location = New System.Drawing.Point(0, 0)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(459, 25)
        Me.lblBuscar.TabIndex = 0
        Me.lblBuscar.Text = "Buscar"
        Me.lblBuscar.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'PanelEdicion
        '
        Me.PanelEdicion.Controls.Add(Me.FlowLayoutPanelBotones)
        Me.PanelEdicion.Controls.Add(Me.txtNombre)
        Me.PanelEdicion.Controls.Add(Me.lblNombre)
        Me.PanelEdicion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelEdicion.Location = New System.Drawing.Point(488, 3)
        Me.PanelEdicion.Name = "PanelEdicion"
        Me.PanelEdicion.Padding = New System.Windows.Forms.Padding(20)
        Me.PanelEdicion.Size = New System.Drawing.Size(393, 475)
        Me.PanelEdicion.TabIndex = 1
        '
        'FlowLayoutPanelBotones
        '
        Me.FlowLayoutPanelBotones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanelBotones.AutoSize = True
        Me.FlowLayoutPanelBotones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanelBotones.Controls.Add(Me.btnNuevo)
        Me.FlowLayoutPanelBotones.Controls.Add(Me.btnGuardar)
        Me.FlowLayoutPanelBotones.Controls.Add(Me.btnEliminar)
        Me.FlowLayoutPanelBotones.Controls.Add(Me.btnCerrar)
        Me.FlowLayoutPanelBotones.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
        Me.FlowLayoutPanelBotones.Location = New System.Drawing.Point(20, 388)
        Me.FlowLayoutPanelBotones.Name = "FlowLayoutPanelBotones"
        Me.FlowLayoutPanelBotones.Size = New System.Drawing.Size(353, 84)
        Me.FlowLayoutPanelBotones.TabIndex = 2
        Me.FlowLayoutPanelBotones.WrapContents = False
        '
        'btnNuevo
        '
        Me.btnNuevo.AutoSize = True
        Me.btnNuevo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnNuevo.Margin = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Padding = New System.Windows.Forms.Padding(10, 6, 10, 6)
        Me.btnNuevo.Size = New System.Drawing.Size(87, 39)
        Me.btnNuevo.TabIndex = 0
        Me.btnNuevo.Text = "Nuevo"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'btnGuardar
        '
        Me.btnGuardar.AutoSize = True
        Me.btnGuardar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Padding = New System.Windows.Forms.Padding(10, 6, 10, 6)
        Me.btnGuardar.Size = New System.Drawing.Size(104, 39)
        Me.btnGuardar.TabIndex = 1
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.AutoSize = True
        Me.btnEliminar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Padding = New System.Windows.Forms.Padding(10, 6, 10, 6)
        Me.btnEliminar.Size = New System.Drawing.Size(101, 39)
        Me.btnEliminar.TabIndex = 2
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.AutoSize = True
        Me.btnCerrar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Padding = New System.Windows.Forms.Padding(10, 6, 10, 6)
        Me.btnCerrar.Size = New System.Drawing.Size(81, 39)
        Me.btnCerrar.TabIndex = 3
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'txtNombre
        '
        Me.txtNombre.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombre.Location = New System.Drawing.Point(20, 92)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(353, 26)
        Me.txtNombre.TabIndex = 1
        '
        'lblNombre
        '
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(17, 65)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(146, 20)
        Me.lblNombre.TabIndex = 0
        Me.lblNombre.Text = "Nombre unidad general"
        '
        'frmUnidadesGenerales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 481)
        Me.Controls.Add(Me.TableLayoutPanelMain)
        Me.MinimumSize = New System.Drawing.Size(900, 520)
        Me.Name = "frmUnidadesGenerales"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Gestión de Unidades Generales"
        Me.TableLayoutPanelMain.ResumeLayout(False)
        Me.PanelListado.ResumeLayout(False)
        CType(Me.dgvUnidadesGenerales, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelBusqueda.ResumeLayout(False)
        Me.PanelBusqueda.PerformLayout()
        Me.PanelEdicion.ResumeLayout(False)
        Me.PanelEdicion.PerformLayout()
        Me.FlowLayoutPanelBotones.ResumeLayout(False)
        Me.FlowLayoutPanelBotones.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanelMain As TableLayoutPanel
    Friend WithEvents PanelListado As Panel
    Friend WithEvents dgvUnidadesGenerales As DataGridView
    Friend WithEvents PanelBusqueda As Panel
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents lblBuscar As Label
    Friend WithEvents PanelEdicion As Panel
    Friend WithEvents FlowLayoutPanelBotones As FlowLayoutPanel
    Friend WithEvents btnNuevo As Button
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents btnCerrar As Button
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents lblNombre As Label
End Class
