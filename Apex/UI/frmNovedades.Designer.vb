<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNovedades
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.PanelPrincipal = New System.Windows.Forms.Panel()
        Me.SplitContenedor = New System.Windows.Forms.SplitContainer()
        Me.dgvNovedades = New System.Windows.Forms.DataGridView()
        Me.PanelEncabezadoLista = New System.Windows.Forms.Panel()
        Me.btnNuevaNovedad = New System.Windows.Forms.Button()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabControlDetalle = New System.Windows.Forms.TabControl()
        Me.TabPageTexto = New System.Windows.Forms.TabPage()
        Me.txtTextoNovedad = New System.Windows.Forms.TextBox()
        Me.TabPageFuncionarios = New System.Windows.Forms.TabPage()
        Me.lstFuncionarios = New System.Windows.Forms.ListBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnQuitarFuncionario = New System.Windows.Forms.Button()
        Me.btnAgregarFuncionario = New System.Windows.Forms.Button()
        Me.TabPageFotos = New System.Windows.Forms.TabPage()
        Me.flpFotos = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnEliminarFoto = New System.Windows.Forms.Button()
        Me.btnAgregarFoto = New System.Windows.Forms.Button()
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
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.TabPageFotos.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelPrincipal
        '
        Me.PanelPrincipal.Controls.Add(Me.SplitContenedor)
        Me.PanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelPrincipal.Location = New System.Drawing.Point(0, 0)
        Me.PanelPrincipal.Name = "PanelPrincipal"
        Me.PanelPrincipal.Padding = New System.Windows.Forms.Padding(10)
        Me.PanelPrincipal.Size = New System.Drawing.Size(984, 561)
        Me.PanelPrincipal.TabIndex = 0
        '
        'SplitContenedor
        '
        Me.SplitContenedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContenedor.Location = New System.Drawing.Point(10, 10)
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
        Me.SplitContenedor.Size = New System.Drawing.Size(964, 541)
        Me.SplitContenedor.SplitterDistance = 450
        Me.SplitContenedor.TabIndex = 0
        '
        'dgvNovedades
        '
        Me.dgvNovedades.AllowUserToAddRows = False
        Me.dgvNovedades.AllowUserToDeleteRows = False
        Me.dgvNovedades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNovedades.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvNovedades.Location = New System.Drawing.Point(0, 50)
        Me.dgvNovedades.Name = "dgvNovedades"
        Me.dgvNovedades.ReadOnly = True
        Me.dgvNovedades.RowHeadersWidth = 51
        Me.dgvNovedades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNovedades.Size = New System.Drawing.Size(450, 491)
        Me.dgvNovedades.TabIndex = 1
        '
        'PanelEncabezadoLista
        '
        Me.PanelEncabezadoLista.Controls.Add(Me.btnNuevaNovedad)
        Me.PanelEncabezadoLista.Controls.Add(Me.dtpFecha)
        Me.PanelEncabezadoLista.Controls.Add(Me.Label1)
        Me.PanelEncabezadoLista.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelEncabezadoLista.Location = New System.Drawing.Point(0, 0)
        Me.PanelEncabezadoLista.Name = "PanelEncabezadoLista"
        Me.PanelEncabezadoLista.Size = New System.Drawing.Size(450, 50)
        Me.PanelEncabezadoLista.TabIndex = 0
        '
        'btnNuevaNovedad
        '
        Me.btnNuevaNovedad.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNuevaNovedad.Location = New System.Drawing.Point(286, 10)
        Me.btnNuevaNovedad.Name = "btnNuevaNovedad"
        Me.btnNuevaNovedad.Size = New System.Drawing.Size(120, 30)
        Me.btnNuevaNovedad.TabIndex = 2
        Me.btnNuevaNovedad.Text = "Nueva Novedad"
        Me.btnNuevaNovedad.UseVisualStyleBackColor = True
        '
        'dtpFecha
        '
        Me.dtpFecha.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(65, 12)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(117, 27)
        Me.dtpFecha.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Label1.Location = New System.Drawing.Point(10, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Fecha:"
        '
        'TabControlDetalle
        '
        Me.TabControlDetalle.Controls.Add(Me.TabPageTexto)
        Me.TabControlDetalle.Controls.Add(Me.TabPageFuncionarios)
        Me.TabControlDetalle.Controls.Add(Me.TabPageFotos)
        Me.TabControlDetalle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlDetalle.Location = New System.Drawing.Point(0, 0)
        Me.TabControlDetalle.Name = "TabControlDetalle"
        Me.TabControlDetalle.SelectedIndex = 0
        Me.TabControlDetalle.Size = New System.Drawing.Size(510, 541)
        Me.TabControlDetalle.TabIndex = 0
        '
        'TabPageTexto
        '
        Me.TabPageTexto.Controls.Add(Me.txtTextoNovedad)
        Me.TabPageTexto.Location = New System.Drawing.Point(4, 30)
        Me.TabPageTexto.Name = "TabPageTexto"
        Me.TabPageTexto.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageTexto.Size = New System.Drawing.Size(502, 507)
        Me.TabPageTexto.TabIndex = 0
        Me.TabPageTexto.Text = "Texto de la Novedad"
        Me.TabPageTexto.UseVisualStyleBackColor = True
        '
        'txtTextoNovedad
        '
        Me.txtTextoNovedad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTextoNovedad.Location = New System.Drawing.Point(3, 3)
        Me.txtTextoNovedad.Multiline = True
        Me.txtTextoNovedad.Name = "txtTextoNovedad"
        Me.txtTextoNovedad.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTextoNovedad.Size = New System.Drawing.Size(496, 501)
        Me.txtTextoNovedad.TabIndex = 0
        '
        'TabPageFuncionarios
        '
        Me.TabPageFuncionarios.Controls.Add(Me.lstFuncionarios)
        Me.TabPageFuncionarios.Controls.Add(Me.FlowLayoutPanel1)
        Me.TabPageFuncionarios.Location = New System.Drawing.Point(4, 30)
        Me.TabPageFuncionarios.Name = "TabPageFuncionarios"
        Me.TabPageFuncionarios.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageFuncionarios.Size = New System.Drawing.Size(502, 507)
        Me.TabPageFuncionarios.TabIndex = 1
        Me.TabPageFuncionarios.Text = "Funcionarios"
        Me.TabPageFuncionarios.UseVisualStyleBackColor = True
        '
        'lstFuncionarios
        '
        Me.lstFuncionarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstFuncionarios.FormattingEnabled = True
        Me.lstFuncionarios.ItemHeight = 23
        Me.lstFuncionarios.Location = New System.Drawing.Point(3, 3)
        Me.lstFuncionarios.Name = "lstFuncionarios"
        Me.lstFuncionarios.Size = New System.Drawing.Size(496, 455)
        Me.lstFuncionarios.TabIndex = 1
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.Controls.Add(Me.btnQuitarFuncionario)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAgregarFuncionario)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 458)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(496, 46)
        Me.FlowLayoutPanel1.TabIndex = 0
        '
        'btnQuitarFuncionario
        '
        Me.btnQuitarFuncionario.Location = New System.Drawing.Point(393, 3)
        Me.btnQuitarFuncionario.Name = "btnQuitarFuncionario"
        Me.btnQuitarFuncionario.Size = New System.Drawing.Size(100, 40)
        Me.btnQuitarFuncionario.TabIndex = 1
        Me.btnQuitarFuncionario.Text = "Quitar"
        Me.btnQuitarFuncionario.UseVisualStyleBackColor = True
        '
        'btnAgregarFuncionario
        '
        Me.btnAgregarFuncionario.Location = New System.Drawing.Point(287, 3)
        Me.btnAgregarFuncionario.Name = "btnAgregarFuncionario"
        Me.btnAgregarFuncionario.Size = New System.Drawing.Size(100, 40)
        Me.btnAgregarFuncionario.TabIndex = 0
        Me.btnAgregarFuncionario.Text = "Agregar..."
        Me.btnAgregarFuncionario.UseVisualStyleBackColor = True
        '
        'TabPageFotos
        '
        Me.TabPageFotos.Controls.Add(Me.flpFotos)
        Me.TabPageFotos.Controls.Add(Me.FlowLayoutPanel2)
        Me.TabPageFotos.Location = New System.Drawing.Point(4, 30)
        Me.TabPageFotos.Name = "TabPageFotos"
        Me.TabPageFotos.Size = New System.Drawing.Size(502, 507)
        Me.TabPageFotos.TabIndex = 2
        Me.TabPageFotos.Text = "Fotos"
        Me.TabPageFotos.UseVisualStyleBackColor = True
        '
        'flpFotos
        '
        Me.flpFotos.AutoScroll = True
        Me.flpFotos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpFotos.Location = New System.Drawing.Point(0, 0)
        Me.flpFotos.Name = "flpFotos"
        Me.flpFotos.Size = New System.Drawing.Size(502, 461)
        Me.flpFotos.TabIndex = 2
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.AutoSize = True
        Me.FlowLayoutPanel2.Controls.Add(Me.btnEliminarFoto)
        Me.FlowLayoutPanel2.Controls.Add(Me.btnAgregarFoto)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(0, 461)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(502, 46)
        Me.FlowLayoutPanel2.TabIndex = 1
        '
        'btnEliminarFoto
        '
        Me.btnEliminarFoto.Location = New System.Drawing.Point(399, 3)
        Me.btnEliminarFoto.Name = "btnEliminarFoto"
        Me.btnEliminarFoto.Size = New System.Drawing.Size(100, 40)
        Me.btnEliminarFoto.TabIndex = 1
        Me.btnEliminarFoto.Text = "Eliminar"
        Me.btnEliminarFoto.UseVisualStyleBackColor = True
        '
        'btnAgregarFoto
        '
        Me.btnAgregarFoto.Location = New System.Drawing.Point(293, 3)
        Me.btnAgregarFoto.Name = "btnAgregarFoto"
        Me.btnAgregarFoto.Size = New System.Drawing.Size(100, 40)
        Me.btnAgregarFoto.TabIndex = 0
        Me.btnAgregarFoto.Text = "Agregar..."
        Me.btnAgregarFoto.UseVisualStyleBackColor = True
        '
        'frmNovedades
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 561)
        Me.Controls.Add(Me.PanelPrincipal)
        Me.Name = "frmNovedades"
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
        Me.TabPageFuncionarios.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.TabPageFotos.ResumeLayout(False)
        Me.TabPageFotos.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelPrincipal As Panel
    Friend WithEvents SplitContenedor As SplitContainer
    Friend WithEvents dgvNovedades As DataGridView
    Friend WithEvents PanelEncabezadoLista As Panel
    Friend WithEvents btnNuevaNovedad As Button
    Friend WithEvents dtpFecha As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents TabControlDetalle As TabControl
    Friend WithEvents TabPageTexto As TabPage
    Friend WithEvents TabPageFuncionarios As TabPage
    Friend WithEvents TabPageFotos As TabPage
    Friend WithEvents txtTextoNovedad As TextBox
    Friend WithEvents lstFuncionarios As ListBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnQuitarFuncionario As Button
    Friend WithEvents btnAgregarFuncionario As Button
    Friend WithEvents flpFotos As FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents btnEliminarFoto As Button
    Friend WithEvents btnAgregarFoto As Button
End Class