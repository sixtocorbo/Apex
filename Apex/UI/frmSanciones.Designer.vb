<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSanciones
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
        Me.dgvSanciones = New System.Windows.Forms.DataGridView()
        Me.PanelSanciones = New System.Windows.Forms.Panel()
        Me.btnEliminarSancion = New System.Windows.Forms.Button()
        Me.btnEditarSancion = New System.Windows.Forms.Button()
        Me.btnNuevaSancion = New System.Windows.Forms.Button()
        Me.PanelBusquedaSanciones = New System.Windows.Forms.Panel()
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
        Me.PanelBusquedaSanciones.Controls.Add(Me.txtBusquedaSancion)
        Me.PanelBusquedaSanciones.Controls.Add(Me.Label3)
        Me.PanelBusquedaSanciones.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaSanciones.Location = New System.Drawing.Point(0, 0)
        Me.PanelBusquedaSanciones.Name = "PanelBusquedaSanciones"
        Me.PanelBusquedaSanciones.Size = New System.Drawing.Size(900, 62)
        Me.PanelBusquedaSanciones.TabIndex = 3
        '
        'txtBusquedaSancion
        '
        Me.txtBusquedaSancion.Location = New System.Drawing.Point(131, 14)
        Me.txtBusquedaSancion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBusquedaSancion.Name = "txtBusquedaSancion"
        Me.txtBusquedaSancion.Size = New System.Drawing.Size(380, 26)
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
        'frmGestionSanciones
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 562)
        Me.Controls.Add(Me.dgvSanciones)
        Me.Controls.Add(Me.PanelSanciones)
        Me.Controls.Add(Me.PanelBusquedaSanciones)
        Me.Name = "frmGestionSanciones"
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
End Class