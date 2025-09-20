<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGrados
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.dgvCargos = New System.Windows.Forms.DataGridView()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.pnlEdicion = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pnlInputs = New System.Windows.Forms.TableLayoutPanel()
        Me.txtGrado = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pnlBotones = New System.Windows.Forms.TableLayoutPanel()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnNuevo = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dgvCargos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlEdicion.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.pnlInputs.SuspendLayout()
        Me.pnlBotones.SuspendLayout()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.SplitContainer1.Location = New System.Drawing.Point(10, 10)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.dgvCargos)
        Me.SplitContainer1.Panel1.Controls.Add(Me.txtBuscar)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label8)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.pnlEdicion)
        Me.SplitContainer1.Size = New System.Drawing.Size(1014, 555)
        Me.SplitContainer1.SplitterDistance = 580
        Me.SplitContainer1.TabIndex = 0
        '
        'dgvCargos
        '
        Me.dgvCargos.AllowUserToAddRows = False
        Me.dgvCargos.AllowUserToDeleteRows = False
        Me.dgvCargos.AllowUserToResizeColumns = False
        Me.dgvCargos.AllowUserToResizeRows = False
        Me.dgvCargos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvCargos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvCargos.Location = New System.Drawing.Point(0, 46)
        Me.dgvCargos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvCargos.MultiSelect = False
        Me.dgvCargos.Name = "dgvCargos"
        Me.dgvCargos.ReadOnly = True
        Me.dgvCargos.RowHeadersVisible = False
        Me.dgvCargos.RowHeadersWidth = 62
        Me.dgvCargos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvCargos.Size = New System.Drawing.Size(580, 509)
        Me.dgvCargos.TabIndex = 2
        '
        'txtBuscar
        '
        Me.txtBuscar.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtBuscar.Location = New System.Drawing.Point(0, 20)
        Me.txtBuscar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(580, 26)
        Me.txtBuscar.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label8.Location = New System.Drawing.Point(0, 0)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(63, 20)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Buscar:"
        '
        'pnlEdicion
        '
        Me.pnlEdicion.ColumnCount = 1
        Me.pnlEdicion.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.pnlEdicion.Controls.Add(Me.GroupBox1, 0, 0)
        Me.pnlEdicion.Controls.Add(Me.pnlBotones, 0, 1)
        Me.pnlEdicion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlEdicion.Location = New System.Drawing.Point(0, 0)
        Me.pnlEdicion.Name = "pnlEdicion"
        Me.pnlEdicion.RowCount = 3
        Me.pnlEdicion.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlEdicion.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlEdicion.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.pnlEdicion.Size = New System.Drawing.Size(430, 555)
        Me.pnlEdicion.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.pnlInputs)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 5)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(10)
        Me.GroupBox1.Size = New System.Drawing.Size(422, 120)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Detalles del Cargo"
        '
        'pnlInputs
        '
        Me.pnlInputs.ColumnCount = 2
        Me.pnlInputs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.pnlInputs.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.pnlInputs.Controls.Add(Me.txtGrado, 1, 1)
        Me.pnlInputs.Controls.Add(Me.Label2, 0, 1)
        Me.pnlInputs.Controls.Add(Me.txtNombre, 1, 0)
        Me.pnlInputs.Controls.Add(Me.Label1, 0, 0)
        Me.pnlInputs.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlInputs.Location = New System.Drawing.Point(10, 29)
        Me.pnlInputs.Name = "pnlInputs"
        Me.pnlInputs.RowCount = 2
        Me.pnlInputs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.pnlInputs.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.pnlInputs.Size = New System.Drawing.Size(402, 81)
        Me.pnlInputs.TabIndex = 0
        '
        'txtGrado
        '
        Me.txtGrado.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.txtGrado.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtGrado.Location = New System.Drawing.Point(81, 47)
        Me.txtGrado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtGrado.Name = "txtGrado"
        Me.txtGrado.Size = New System.Drawing.Size(148, 26)
        Me.txtGrado.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 50)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(58, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Grado:"
        '
        'txtNombre
        '
        Me.txtNombre.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtNombre.Location = New System.Drawing.Point(81, 7)
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(317, 26)
        Me.txtNombre.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 10)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nombre:"
        '
        'pnlBotones
        '
        Me.pnlBotones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlBotones.ColumnCount = 2
        Me.pnlBotones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.pnlBotones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.pnlBotones.Controls.Add(Me.btnGuardar, 1, 0)
        Me.pnlBotones.Controls.Add(Me.btnEliminar, 0, 1)
        Me.pnlBotones.Controls.Add(Me.btnNuevo, 0, 0)
        Me.pnlBotones.Location = New System.Drawing.Point(3, 133)
        Me.pnlBotones.Name = "pnlBotones"
        Me.pnlBotones.RowCount = 2
        Me.pnlBotones.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.pnlBotones.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.pnlBotones.Size = New System.Drawing.Size(424, 112)
        Me.pnlBotones.TabIndex = 1
        '
        'btnGuardar
        '
        Me.btnGuardar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnGuardar.Location = New System.Drawing.Point(216, 5)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(204, 46)
        Me.btnGuardar.TabIndex = 1
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnEliminar.Location = New System.Drawing.Point(4, 61)
        Me.btnEliminar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(204, 46)
        Me.btnEliminar.TabIndex = 2
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'btnNuevo
        '
        Me.btnNuevo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNuevo.Location = New System.Drawing.Point(4, 5)
        Me.btnNuevo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(204, 46)
        Me.btnNuevo.TabIndex = 0
        Me.btnNuevo.Text = "Nuevo"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'frmGrados
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1034, 575)
        Me.Controls.Add(Me.SplitContainer1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frmGrados"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Cargos"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dgvCargos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlEdicion.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.pnlInputs.ResumeLayout(False)
        Me.pnlInputs.PerformLayout()
        Me.pnlBotones.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents dgvCargos As DataGridView
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtGrado As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnNuevo As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents btnGuardar As Button
    Friend WithEvents pnlEdicion As TableLayoutPanel
    Friend WithEvents pnlInputs As TableLayoutPanel
    Friend WithEvents pnlBotones As TableLayoutPanel
End Class