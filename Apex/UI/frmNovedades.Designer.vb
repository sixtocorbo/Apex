<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNovedades
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
        Me.SplitContenedor = New System.Windows.Forms.SplitContainer()
        Me.dgvNovedades = New System.Windows.Forms.DataGridView()
        Me.PanelEncabezadoLista = New System.Windows.Forms.Panel()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.lblBusqueda = New System.Windows.Forms.Label()
        Me.btnEliminarNovedad = New System.Windows.Forms.Button()
        Me.btnEditarNovedad = New System.Windows.Forms.Button()
        Me.btnNuevaNovedad = New System.Windows.Forms.Button()
        Me.TabControlDetalle = New System.Windows.Forms.TabControl()
        Me.TabPageTexto = New System.Windows.Forms.TabPage()
        Me.txtTextoNovedad = New System.Windows.Forms.TextBox()
        Me.TabPageFuncionarios = New System.Windows.Forms.TabPage()
        Me.lstFuncionarios = New System.Windows.Forms.ListBox()
        Me.TabPageFotos = New System.Windows.Forms.TabPage()
        Me.flpFotos = New System.Windows.Forms.FlowLayoutPanel()
        Me.PanelPrincipal.SuspendLayout()
        CType(Me.SplitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContenedor.Panel1.SuspendLayout()
        Me.SplitContenedor.Panel2.SuspendLayout()
        Me.SplitContenedor.SuspendLayout()
        CType(Me.dgvNovedades, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelEncabezadoLista.SuspendLayout()
        Me.TabControlDetalle.SuspendLayout()
        Me.TabPageTexto.SuspendLayout()
        Me.TabPageFuncionarios.SuspendLayout()
        Me.TabPageFotos.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelPrincipal
        '
        Me.PanelPrincipal.Controls.Add(Me.SplitContenedor)
        Me.PanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelPrincipal.Location = New System.Drawing.Point(0, 0)
        Me.PanelPrincipal.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelPrincipal.Name = "PanelPrincipal"
        Me.PanelPrincipal.Padding = New System.Windows.Forms.Padding(12)
        Me.PanelPrincipal.Size = New System.Drawing.Size(1245, 701)
        Me.PanelPrincipal.TabIndex = 0
        '
        'SplitContenedor
        '
        Me.SplitContenedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContenedor.Location = New System.Drawing.Point(12, 12)
        Me.SplitContenedor.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.SplitContenedor.Name = "SplitContenedor"
        '
        'SplitContenedor.Panel1
        '
        Me.SplitContenedor.Panel1.Controls.Add(Me.dgvNovedades)
        Me.SplitContenedor.Panel1.Controls.Add(Me.PanelEncabezadoLista)
        '
        'SplitContenedor.Panel2
        '
        Me.SplitContenedor.Panel2.Controls.Add(Me.TabControlDetalle)
        Me.SplitContenedor.Size = New System.Drawing.Size(1221, 677)
        Me.SplitContenedor.SplitterDistance = 569
        Me.SplitContenedor.TabIndex = 0
        '
        'dgvNovedades
        '
        Me.dgvNovedades.AllowUserToAddRows = False
        Me.dgvNovedades.AllowUserToDeleteRows = False
        Me.dgvNovedades.AllowUserToResizeColumns = False
        Me.dgvNovedades.AllowUserToResizeRows = False
        Me.dgvNovedades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNovedades.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvNovedades.Location = New System.Drawing.Point(0, 78)
        Me.dgvNovedades.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dgvNovedades.Name = "dgvNovedades"
        Me.dgvNovedades.ReadOnly = True
        Me.dgvNovedades.RowHeadersWidth = 51
        Me.dgvNovedades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNovedades.Size = New System.Drawing.Size(569, 599)
        Me.dgvNovedades.TabIndex = 1
        '
        'PanelEncabezadoLista
        '
        Me.PanelEncabezadoLista.Controls.Add(Me.btnBuscar)
        Me.PanelEncabezadoLista.Controls.Add(Me.txtBusqueda)
        Me.PanelEncabezadoLista.Controls.Add(Me.lblBusqueda)
        Me.PanelEncabezadoLista.Controls.Add(Me.btnEliminarNovedad)
        Me.PanelEncabezadoLista.Controls.Add(Me.btnEditarNovedad)
        Me.PanelEncabezadoLista.Controls.Add(Me.btnNuevaNovedad)
        Me.PanelEncabezadoLista.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelEncabezadoLista.Location = New System.Drawing.Point(0, 0)
        Me.PanelEncabezadoLista.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelEncabezadoLista.Name = "PanelEncabezadoLista"
        Me.PanelEncabezadoLista.Size = New System.Drawing.Size(569, 78)
        Me.PanelEncabezadoLista.TabIndex = 0
        '
        'btnBuscar
        '
        Me.btnBuscar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBuscar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnBuscar.Location = New System.Drawing.Point(451, 10)
        Me.btnBuscar.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(106, 35)
        Me.btnBuscar.TabIndex = 7
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'txtBusqueda
        '
        Me.txtBusqueda.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBusqueda.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtBusqueda.Location = New System.Drawing.Point(78, 11)
        Me.txtBusqueda.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(366, 31)
        Me.txtBusqueda.TabIndex = 6
        '
        'lblBusqueda
        '
        Me.lblBusqueda.AutoSize = True
        Me.lblBusqueda.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblBusqueda.Location = New System.Drawing.Point(3, 14)
        Me.lblBusqueda.Name = "lblBusqueda"
        Me.lblBusqueda.Size = New System.Drawing.Size(67, 25)
        Me.lblBusqueda.TabIndex = 5
        Me.lblBusqueda.Text = "Buscar:"
        '
        'btnEliminarNovedad
        '
        Me.btnEliminarNovedad.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnEliminarNovedad.Location = New System.Drawing.Point(248, 44)
        Me.btnEliminarNovedad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEliminarNovedad.Name = "btnEliminarNovedad"
        Me.btnEliminarNovedad.Size = New System.Drawing.Size(90, 35)
        Me.btnEliminarNovedad.TabIndex = 4
        Me.btnEliminarNovedad.Text = "Eliminar"
        Me.btnEliminarNovedad.UseVisualStyleBackColor = True
        '
        'btnEditarNovedad
        '
        Me.btnEditarNovedad.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnEditarNovedad.Location = New System.Drawing.Point(134, 44)
        Me.btnEditarNovedad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEditarNovedad.Name = "btnEditarNovedad"
        Me.btnEditarNovedad.Size = New System.Drawing.Size(107, 35)
        Me.btnEditarNovedad.TabIndex = 3
        Me.btnEditarNovedad.Text = "Editar..."
        Me.btnEditarNovedad.UseVisualStyleBackColor = True
        '
        'btnNuevaNovedad
        '
        Me.btnNuevaNovedad.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNuevaNovedad.Location = New System.Drawing.Point(8, 44)
        Me.btnNuevaNovedad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNuevaNovedad.Name = "btnNuevaNovedad"
        Me.btnNuevaNovedad.Size = New System.Drawing.Size(119, 35)
        Me.btnNuevaNovedad.TabIndex = 2
        Me.btnNuevaNovedad.Text = "Nueva"
        Me.btnNuevaNovedad.UseVisualStyleBackColor = True
        '
        'TabControlDetalle
        '
        Me.TabControlDetalle.Controls.Add(Me.TabPageTexto)
        Me.TabControlDetalle.Controls.Add(Me.TabPageFuncionarios)
        Me.TabControlDetalle.Controls.Add(Me.TabPageFotos)
        Me.TabControlDetalle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlDetalle.Location = New System.Drawing.Point(0, 0)
        Me.TabControlDetalle.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabControlDetalle.Name = "TabControlDetalle"
        Me.TabControlDetalle.SelectedIndex = 0
        Me.TabControlDetalle.Size = New System.Drawing.Size(648, 677)
        Me.TabControlDetalle.TabIndex = 0
        '
        'TabPageTexto
        '
        Me.TabPageTexto.Controls.Add(Me.txtTextoNovedad)
        Me.TabPageTexto.Location = New System.Drawing.Point(4, 29)
        Me.TabPageTexto.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabPageTexto.Name = "TabPageTexto"
        Me.TabPageTexto.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabPageTexto.Size = New System.Drawing.Size(640, 644)
        Me.TabPageTexto.TabIndex = 0
        Me.TabPageTexto.Text = "Texto de la Novedad"
        Me.TabPageTexto.UseVisualStyleBackColor = True
        '
        'txtTextoNovedad
        '
        Me.txtTextoNovedad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTextoNovedad.Location = New System.Drawing.Point(3, 4)
        Me.txtTextoNovedad.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtTextoNovedad.Multiline = True
        Me.txtTextoNovedad.Name = "txtTextoNovedad"
        Me.txtTextoNovedad.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTextoNovedad.Size = New System.Drawing.Size(634, 636)
        Me.txtTextoNovedad.TabIndex = 0
        '
        'TabPageFuncionarios
        '
        Me.TabPageFuncionarios.Controls.Add(Me.lstFuncionarios)
        Me.TabPageFuncionarios.Location = New System.Drawing.Point(4, 29)
        Me.TabPageFuncionarios.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabPageFuncionarios.Name = "TabPageFuncionarios"
        Me.TabPageFuncionarios.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabPageFuncionarios.Size = New System.Drawing.Size(640, 644)
        Me.TabPageFuncionarios.TabIndex = 1
        Me.TabPageFuncionarios.Text = "Funcionarios"
        Me.TabPageFuncionarios.UseVisualStyleBackColor = True
        '
        'lstFuncionarios
        '
        Me.lstFuncionarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstFuncionarios.FormattingEnabled = True
        Me.lstFuncionarios.ItemHeight = 20
        Me.lstFuncionarios.Location = New System.Drawing.Point(3, 4)
        Me.lstFuncionarios.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.lstFuncionarios.Name = "lstFuncionarios"
        Me.lstFuncionarios.Size = New System.Drawing.Size(634, 636)
        Me.lstFuncionarios.TabIndex = 1
        '
        'TabPageFotos
        '
        Me.TabPageFotos.Controls.Add(Me.flpFotos)
        Me.TabPageFotos.Location = New System.Drawing.Point(4, 29)
        Me.TabPageFotos.Name = "TabPageFotos"
        Me.TabPageFotos.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageFotos.Size = New System.Drawing.Size(640, 644)
        Me.TabPageFotos.TabIndex = 2
        Me.TabPageFotos.Text = "Fotos"
        Me.TabPageFotos.UseVisualStyleBackColor = True
        '
        'flpFotos
        '
        Me.flpFotos.AutoScroll = True
        Me.flpFotos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpFotos.Location = New System.Drawing.Point(3, 3)
        Me.flpFotos.Name = "flpFotos"
        Me.flpFotos.Size = New System.Drawing.Size(634, 638)
        Me.flpFotos.TabIndex = 0
        '
        'frmNovedades
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1245, 701)
        Me.Controls.Add(Me.PanelPrincipal)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmNovedades"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Novedades"
        Me.PanelPrincipal.ResumeLayout(False)
        Me.SplitContenedor.Panel1.ResumeLayout(False)
        Me.SplitContenedor.Panel2.ResumeLayout(False)
        CType(Me.SplitContenedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContenedor.ResumeLayout(False)
        CType(Me.dgvNovedades, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelEncabezadoLista.ResumeLayout(False)
        Me.PanelEncabezadoLista.PerformLayout()
        Me.TabControlDetalle.ResumeLayout(False)
        Me.TabPageTexto.ResumeLayout(False)
        Me.TabPageTexto.PerformLayout()
        Me.TabPageFuncionarios.ResumeLayout(False)
        Me.TabPageFotos.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelPrincipal As Panel
    Friend WithEvents SplitContenedor As SplitContainer
    Friend WithEvents dgvNovedades As DataGridView
    Friend WithEvents PanelEncabezadoLista As Panel
    Friend WithEvents btnNuevaNovedad As Button
    Friend WithEvents TabControlDetalle As TabControl
    Friend WithEvents TabPageTexto As TabPage
    Friend WithEvents TabPageFuncionarios As TabPage
    Friend WithEvents txtTextoNovedad As TextBox
    Friend WithEvents lstFuncionarios As ListBox
    Friend WithEvents btnEditarNovedad As Button
    Friend WithEvents TabPageFotos As TabPage
    Friend WithEvents flpFotos As FlowLayoutPanel
    Friend WithEvents btnEliminarNovedad As Button
    Friend WithEvents txtBusqueda As TextBox
    Friend WithEvents lblBusqueda As Label
    Friend WithEvents btnBuscar As Button
End Class