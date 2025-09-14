<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSanciones
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
        Me.dgvSanciones = New System.Windows.Forms.DataGridView()
        Me.PanelSanciones = New System.Windows.Forms.Panel()
        Me.btnEliminarSancion = New System.Windows.Forms.Button()
        Me.btnEditarSancion = New System.Windows.Forms.Button()
        Me.btnNuevaSancion = New System.Windows.Forms.Button()
        Me.PanelBusquedaSanciones = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbTipoSancion = New System.Windows.Forms.ComboBox()
        Me.txtBusquedaSancion = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        CType(Me.dgvSanciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelSanciones.SuspendLayout()
        Me.PanelBusquedaSanciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvSanciones
        '
        Me.dgvSanciones.AllowUserToAddRows = False
        Me.dgvSanciones.AllowUserToDeleteRows = False
        Me.dgvSanciones.AllowUserToResizeColumns = False
        Me.dgvSanciones.AllowUserToResizeRows = False
        Me.dgvSanciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSanciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSanciones.Location = New System.Drawing.Point(0, 62)
        Me.dgvSanciones.Name = "dgvSanciones"
        Me.dgvSanciones.ReadOnly = True
        Me.dgvSanciones.RowHeadersWidth = 51
        Me.dgvSanciones.RowTemplate.Height = 24
        Me.dgvSanciones.Size = New System.Drawing.Size(900, 438)
        Me.dgvSanciones.TabIndex = 5
        '
        'PanelSanciones
        '
        Me.PanelSanciones.Controls.Add(Me.btnEliminarSancion)
        Me.PanelSanciones.Controls.Add(Me.btnEditarSancion)
        Me.PanelSanciones.Controls.Add(Me.btnNuevaSancion)
        Me.PanelSanciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelSanciones.Location = New System.Drawing.Point(0, 500)
        Me.PanelSanciones.Name = "PanelSanciones"
        Me.PanelSanciones.Size = New System.Drawing.Size(900, 62)
        Me.PanelSanciones.TabIndex = 4
        '
        'btnEliminarSancion
        '
        Me.btnEliminarSancion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarSancion.Location = New System.Drawing.Point(596, 12)
        Me.btnEliminarSancion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEliminarSancion.Name = "btnEliminarSancion"
        Me.btnEliminarSancion.Size = New System.Drawing.Size(96, 38)
        Me.btnEliminarSancion.TabIndex = 2
        Me.btnEliminarSancion.Text = "Eliminar"
        Me.btnEliminarSancion.UseVisualStyleBackColor = True
        '
        'btnEditarSancion
        '
        Me.btnEditarSancion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditarSancion.Location = New System.Drawing.Point(698, 12)
        Me.btnEditarSancion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEditarSancion.Name = "btnEditarSancion"
        Me.btnEditarSancion.Size = New System.Drawing.Size(96, 38)
        Me.btnEditarSancion.TabIndex = 1
        Me.btnEditarSancion.Text = "Editar..."
        Me.btnEditarSancion.UseVisualStyleBackColor = True
        '
        'btnNuevaSancion
        '
        Me.btnNuevaSancion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaSancion.Location = New System.Drawing.Point(800, 12)
        Me.btnNuevaSancion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNuevaSancion.Name = "btnNuevaSancion"
        Me.btnNuevaSancion.Size = New System.Drawing.Size(96, 38)
        Me.btnNuevaSancion.TabIndex = 0
        Me.btnNuevaSancion.Text = "Nueva..."
        Me.btnNuevaSancion.UseVisualStyleBackColor = True
        '
        'PanelBusquedaSanciones
        '
        Me.PanelBusquedaSanciones.Controls.Add(Me.Label1)
        Me.PanelBusquedaSanciones.Controls.Add(Me.cmbTipoSancion)
        Me.PanelBusquedaSanciones.Controls.Add(Me.txtBusquedaSancion)
        Me.PanelBusquedaSanciones.Controls.Add(Me.Label3)
        Me.PanelBusquedaSanciones.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaSanciones.Location = New System.Drawing.Point(0, 0)
        Me.PanelBusquedaSanciones.Name = "PanelBusquedaSanciones"
        Me.PanelBusquedaSanciones.Size = New System.Drawing.Size(900, 62)
        Me.PanelBusquedaSanciones.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(430, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(105, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Tipo Sanción:"
        '
        'cmbTipoSancion
        '
        Me.cmbTipoSancion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbTipoSancion.FormattingEnabled = True
        Me.cmbTipoSancion.Location = New System.Drawing.Point(550, 14)
        Me.cmbTipoSancion.Name = "cmbTipoSancion"
        Me.cmbTipoSancion.Size = New System.Drawing.Size(280, 28)
        Me.cmbTipoSancion.TabIndex = 2
        '
        'txtBusquedaSancion
        '
        Me.txtBusquedaSancion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBusquedaSancion.Location = New System.Drawing.Point(131, 14)
        Me.txtBusquedaSancion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBusquedaSancion.Name = "txtBusquedaSancion"
        Me.txtBusquedaSancion.Size = New System.Drawing.Size(280, 26)
        Me.txtBusquedaSancion.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(96, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Funcionario:"
        '
        'frmSanciones
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 562)
        Me.Controls.Add(Me.dgvSanciones)
        Me.Controls.Add(Me.PanelSanciones)
        Me.Controls.Add(Me.PanelBusquedaSanciones)
        Me.Name = "frmSanciones"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Sanciones"
        CType(Me.dgvSanciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelSanciones.ResumeLayout(False)
        Me.PanelBusquedaSanciones.ResumeLayout(False)
        Me.PanelBusquedaSanciones.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvSanciones As DataGridView
    Friend WithEvents PanelSanciones As Panel
    Friend WithEvents btnEliminarSancion As Button
    Friend WithEvents btnEditarSancion As Button
    Friend WithEvents btnNuevaSancion As Button
    Friend WithEvents PanelBusquedaSanciones As Panel
    Friend WithEvents txtBusquedaSancion As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents cmbTipoSancion As ComboBox
    Friend WithEvents Label1 As Label
End Class