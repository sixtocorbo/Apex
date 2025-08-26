<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGestionNotificaciones
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
        Me.splitContenedorNotificaciones = New System.Windows.Forms.SplitContainer()
        Me.dgvNotificaciones = New System.Windows.Forms.DataGridView()
        Me.txtTextoNotificacion = New System.Windows.Forms.TextBox()
        Me.PanelNotificaciones = New System.Windows.Forms.Panel()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.btnCambiarEstado = New System.Windows.Forms.Button()
        Me.btnEliminarNotificacion = New System.Windows.Forms.Button()
        Me.btnEditarNotificacion = New System.Windows.Forms.Button()
        Me.btnNuevaNotificacion = New System.Windows.Forms.Button()
        Me.PanelBusquedaNotificaciones = New System.Windows.Forms.Panel()
        Me.txtBusquedaNotificacion = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.splitContenedorNotificaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedorNotificaciones.Panel1.SuspendLayout()
        Me.splitContenedorNotificaciones.Panel2.SuspendLayout()
        Me.splitContenedorNotificaciones.SuspendLayout()
        CType(Me.dgvNotificaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelNotificaciones.SuspendLayout()
        Me.PanelBusquedaNotificaciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'splitContenedorNotificaciones
        '
        Me.splitContenedorNotificaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContenedorNotificaciones.Location = New System.Drawing.Point(0, 62)
        Me.splitContenedorNotificaciones.Name = "splitContenedorNotificaciones"
        Me.splitContenedorNotificaciones.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitContenedorNotificaciones.Panel1
        '
        Me.splitContenedorNotificaciones.Panel1.Controls.Add(Me.dgvNotificaciones)
        '
        'splitContenedorNotificaciones.Panel2
        '
        Me.splitContenedorNotificaciones.Panel2.Controls.Add(Me.txtTextoNotificacion)
        Me.splitContenedorNotificaciones.Panel2.Padding = New System.Windows.Forms.Padding(5)
        Me.splitContenedorNotificaciones.Size = New System.Drawing.Size(900, 438)
        Me.splitContenedorNotificaciones.SplitterDistance = 216
        Me.splitContenedorNotificaciones.TabIndex = 7
        '
        'dgvNotificaciones
        '
        Me.dgvNotificaciones.AllowUserToAddRows = False
        Me.dgvNotificaciones.AllowUserToDeleteRows = False
        Me.dgvNotificaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNotificaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvNotificaciones.Location = New System.Drawing.Point(0, 0)
        Me.dgvNotificaciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dgvNotificaciones.Name = "dgvNotificaciones"
        Me.dgvNotificaciones.ReadOnly = True
        Me.dgvNotificaciones.RowHeadersWidth = 51
        Me.dgvNotificaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNotificaciones.Size = New System.Drawing.Size(900, 216)
        Me.dgvNotificaciones.TabIndex = 2
        '
        'txtTextoNotificacion
        '
        Me.txtTextoNotificacion.BackColor = System.Drawing.SystemColors.Info
        Me.txtTextoNotificacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTextoNotificacion.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTextoNotificacion.Location = New System.Drawing.Point(5, 5)
        Me.txtTextoNotificacion.Multiline = True
        Me.txtTextoNotificacion.Name = "txtTextoNotificacion"
        Me.txtTextoNotificacion.ReadOnly = True
        Me.txtTextoNotificacion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTextoNotificacion.Size = New System.Drawing.Size(890, 208)
        Me.txtTextoNotificacion.TabIndex = 0
        '
        'PanelNotificaciones
        '
        Me.PanelNotificaciones.Controls.Add(Me.btnImprimir)
        Me.PanelNotificaciones.Controls.Add(Me.btnCambiarEstado)
        Me.PanelNotificaciones.Controls.Add(Me.btnEliminarNotificacion)
        Me.PanelNotificaciones.Controls.Add(Me.btnEditarNotificacion)
        Me.PanelNotificaciones.Controls.Add(Me.btnNuevaNotificacion)
        Me.PanelNotificaciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelNotificaciones.Location = New System.Drawing.Point(0, 500)
        Me.PanelNotificaciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelNotificaciones.Name = "PanelNotificaciones"
        Me.PanelNotificaciones.Size = New System.Drawing.Size(900, 62)
        Me.PanelNotificaciones.TabIndex = 6
        '
        'btnImprimir
        '
        Me.btnImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImprimir.Location = New System.Drawing.Point(390, 12)
        Me.btnImprimir.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(96, 38)
        Me.btnImprimir.TabIndex = 4
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'btnCambiarEstado
        '
        Me.btnCambiarEstado.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCambiarEstado.Location = New System.Drawing.Point(492, 12)
        Me.btnCambiarEstado.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCambiarEstado.Name = "btnCambiarEstado"
        Me.btnCambiarEstado.Size = New System.Drawing.Size(135, 38)
        Me.btnCambiarEstado.TabIndex = 3
        Me.btnCambiarEstado.Text = "Cambiar Estado..."
        Me.btnCambiarEstado.UseVisualStyleBackColor = True
        '
        'btnEliminarNotificacion
        '
        Me.btnEliminarNotificacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarNotificacion.Location = New System.Drawing.Point(633, 12)
        Me.btnEliminarNotificacion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEliminarNotificacion.Name = "btnEliminarNotificacion"
        Me.btnEliminarNotificacion.Size = New System.Drawing.Size(96, 38)
        Me.btnEliminarNotificacion.TabIndex = 2
        Me.btnEliminarNotificacion.Text = "Eliminar"
        Me.btnEliminarNotificacion.UseVisualStyleBackColor = True
        '
        'btnEditarNotificacion
        '
        Me.btnEditarNotificacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditarNotificacion.Location = New System.Drawing.Point(735, 12)
        Me.btnEditarNotificacion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEditarNotificacion.Name = "btnEditarNotificacion"
        Me.btnEditarNotificacion.Size = New System.Drawing.Size(96, 38)
        Me.btnEditarNotificacion.TabIndex = 1
        Me.btnEditarNotificacion.Text = "Editar..."
        Me.btnEditarNotificacion.UseVisualStyleBackColor = True
        '
        'btnNuevaNotificacion
        '
        Me.btnNuevaNotificacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaNotificacion.Location = New System.Drawing.Point(837, 12)
        Me.btnNuevaNotificacion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNuevaNotificacion.Name = "btnNuevaNotificacion"
        Me.btnNuevaNotificacion.Size = New System.Drawing.Size(96, 38)
        Me.btnNuevaNotificacion.TabIndex = 0
        Me.btnNuevaNotificacion.Text = "Nueva..."
        Me.btnNuevaNotificacion.UseVisualStyleBackColor = True
        '
        'PanelBusquedaNotificaciones
        '
        Me.PanelBusquedaNotificaciones.Controls.Add(Me.txtBusquedaNotificacion)
        Me.PanelBusquedaNotificaciones.Controls.Add(Me.Label2)
        Me.PanelBusquedaNotificaciones.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaNotificaciones.Location = New System.Drawing.Point(0, 0)
        Me.PanelBusquedaNotificaciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelBusquedaNotificaciones.Name = "PanelBusquedaNotificaciones"
        Me.PanelBusquedaNotificaciones.Size = New System.Drawing.Size(900, 62)
        Me.PanelBusquedaNotificaciones.TabIndex = 8
        '
        'txtBusquedaNotificacion
        '
        Me.txtBusquedaNotificacion.Location = New System.Drawing.Point(131, 14)
        Me.txtBusquedaNotificacion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBusquedaNotificacion.Name = "txtBusquedaNotificacion"
        Me.txtBusquedaNotificacion.Size = New System.Drawing.Size(380, 26)
        Me.txtBusquedaNotificacion.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 20)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Funcionario:"
        '
        'frmGestionNotificaciones
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 562)
        Me.Controls.Add(Me.splitContenedorNotificaciones)
        Me.Controls.Add(Me.PanelBusquedaNotificaciones)
        Me.Controls.Add(Me.PanelNotificaciones)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmGestionNotificaciones"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Notificaciones"
        Me.splitContenedorNotificaciones.Panel1.ResumeLayout(False)
        Me.splitContenedorNotificaciones.Panel2.ResumeLayout(False)
        Me.splitContenedorNotificaciones.Panel2.PerformLayout()
        CType(Me.splitContenedorNotificaciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedorNotificaciones.ResumeLayout(False)
        CType(Me.dgvNotificaciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelNotificaciones.ResumeLayout(False)
        Me.PanelBusquedaNotificaciones.ResumeLayout(False)
        Me.PanelBusquedaNotificaciones.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents splitContenedorNotificaciones As SplitContainer
    Friend WithEvents dgvNotificaciones As DataGridView
    Friend WithEvents txtTextoNotificacion As TextBox
    Friend WithEvents PanelNotificaciones As Panel
    Friend WithEvents btnImprimir As Button
    Friend WithEvents btnCambiarEstado As Button
    Friend WithEvents btnEliminarNotificacion As Button
    Friend WithEvents btnEditarNotificacion As Button
    Friend WithEvents btnNuevaNotificacion As Button
    Friend WithEvents PanelBusquedaNotificaciones As Panel
    Friend WithEvents txtBusquedaNotificacion As TextBox
    Friend WithEvents Label2 As Label
End Class