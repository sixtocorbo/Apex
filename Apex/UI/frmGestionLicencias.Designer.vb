<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGestionLicencias
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
        Me.dgvLicencias = New System.Windows.Forms.DataGridView()
        Me.PanelLicencias = New System.Windows.Forms.Panel()
        Me.btnEliminarLicencia = New System.Windows.Forms.Button()
        Me.btnEditarLicencia = New System.Windows.Forms.Button()
        Me.btnNuevaLicencia = New System.Windows.Forms.Button()
        Me.PanelBusquedaLicencias = New System.Windows.Forms.Panel()
        Me.txtBusquedaLicencia = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.dgvLicencias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelLicencias.SuspendLayout()
        Me.PanelBusquedaLicencias.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvLicencias
        '
        Me.dgvLicencias.AllowUserToAddRows = False
        Me.dgvLicencias.AllowUserToDeleteRows = False
        Me.dgvLicencias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLicencias.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvLicencias.Location = New System.Drawing.Point(0, 62)
        Me.dgvLicencias.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dgvLicencias.Name = "dgvLicencias"
        Me.dgvLicencias.ReadOnly = True
        Me.dgvLicencias.RowHeadersWidth = 51
        Me.dgvLicencias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLicencias.Size = New System.Drawing.Size(900, 438)
        Me.dgvLicencias.TabIndex = 4
        '
        'PanelLicencias
        '
        Me.PanelLicencias.Controls.Add(Me.btnEliminarLicencia)
        Me.PanelLicencias.Controls.Add(Me.btnEditarLicencia)
        Me.PanelLicencias.Controls.Add(Me.btnNuevaLicencia)
        Me.PanelLicencias.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelLicencias.Location = New System.Drawing.Point(0, 500)
        Me.PanelLicencias.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelLicencias.Name = "PanelLicencias"
        Me.PanelLicencias.Size = New System.Drawing.Size(900, 62)
        Me.PanelLicencias.TabIndex = 3
        '
        'btnEliminarLicencia
        '
        Me.btnEliminarLicencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarLicencia.Location = New System.Drawing.Point(596, 12)
        Me.btnEliminarLicencia.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEliminarLicencia.Name = "btnEliminarLicencia"
        Me.btnEliminarLicencia.Size = New System.Drawing.Size(96, 38)
        Me.btnEliminarLicencia.TabIndex = 2
        Me.btnEliminarLicencia.Text = "Eliminar"
        Me.btnEliminarLicencia.UseVisualStyleBackColor = True
        '
        'btnEditarLicencia
        '
        Me.btnEditarLicencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditarLicencia.Location = New System.Drawing.Point(698, 12)
        Me.btnEditarLicencia.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEditarLicencia.Name = "btnEditarLicencia"
        Me.btnEditarLicencia.Size = New System.Drawing.Size(96, 38)
        Me.btnEditarLicencia.TabIndex = 1
        Me.btnEditarLicencia.Text = "Editar..."
        Me.btnEditarLicencia.UseVisualStyleBackColor = True
        '
        'btnNuevaLicencia
        '
        Me.btnNuevaLicencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaLicencia.Location = New System.Drawing.Point(800, 12)
        Me.btnNuevaLicencia.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNuevaLicencia.Name = "btnNuevaLicencia"
        Me.btnNuevaLicencia.Size = New System.Drawing.Size(96, 38)
        Me.btnNuevaLicencia.TabIndex = 0
        Me.btnNuevaLicencia.Text = "Nueva..."
        Me.btnNuevaLicencia.UseVisualStyleBackColor = True
        '
        'PanelBusquedaLicencias
        '
        Me.PanelBusquedaLicencias.Controls.Add(Me.txtBusquedaLicencia)
        Me.PanelBusquedaLicencias.Controls.Add(Me.Label1)
        Me.PanelBusquedaLicencias.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaLicencias.Location = New System.Drawing.Point(0, 0)
        Me.PanelBusquedaLicencias.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelBusquedaLicencias.Name = "PanelBusquedaLicencias"
        Me.PanelBusquedaLicencias.Size = New System.Drawing.Size(900, 62)
        Me.PanelBusquedaLicencias.TabIndex = 5
        '
        'txtBusquedaLicencia
        '
        Me.txtBusquedaLicencia.Location = New System.Drawing.Point(131, 14)
        Me.txtBusquedaLicencia.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBusquedaLicencia.Name = "txtBusquedaLicencia"
        Me.txtBusquedaLicencia.Size = New System.Drawing.Size(380, 26)
        Me.txtBusquedaLicencia.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Funcionario:"
        '
        'frmGestionLicencias
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 562)
        Me.Controls.Add(Me.dgvLicencias)
        Me.Controls.Add(Me.PanelBusquedaLicencias)
        Me.Controls.Add(Me.PanelLicencias)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmGestionLicencias"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Licencias"
        CType(Me.dgvLicencias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelLicencias.ResumeLayout(False)
        Me.PanelBusquedaLicencias.ResumeLayout(False)
        Me.PanelBusquedaLicencias.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvLicencias As DataGridView
    Friend WithEvents PanelLicencias As Panel
    Friend WithEvents btnEliminarLicencia As Button
    Friend WithEvents btnEditarLicencia As Button
    Friend WithEvents btnNuevaLicencia As Button
    Friend WithEvents PanelBusquedaLicencias As Panel
    Friend WithEvents txtBusquedaLicencia As TextBox
    Friend WithEvents Label1 As Label
End Class