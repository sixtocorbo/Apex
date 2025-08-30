<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAuditoriaViewer
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.dgvAuditoria = New System.Windows.Forms.DataGridView()
        CType(Me.dgvAuditoria, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvAuditoria
        '
        Me.dgvAuditoria.AllowUserToAddRows = False
        Me.dgvAuditoria.AllowUserToDeleteRows = False
        Me.dgvAuditoria.AllowUserToResizeColumns = False
        Me.dgvAuditoria.AllowUserToResizeRows = False
        Me.dgvAuditoria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAuditoria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAuditoria.Location = New System.Drawing.Point(0, 0)
        Me.dgvAuditoria.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvAuditoria.Name = "dgvAuditoria"
        Me.dgvAuditoria.RowHeadersWidth = 62
        Me.dgvAuditoria.Size = New System.Drawing.Size(1176, 709)
        Me.dgvAuditoria.TabIndex = 0
        '
        'frmAuditoriaViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1176, 709)
        Me.Controls.Add(Me.dgvAuditoria)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmAuditoriaViewer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Visor de Auditoría"
        CType(Me.dgvAuditoria, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents dgvAuditoria As DataGridView
End Class
