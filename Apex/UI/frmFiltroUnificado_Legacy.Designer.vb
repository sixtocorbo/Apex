<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFiltroUnificado_Legacy
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
        Me.pnlSuperior = New System.Windows.Forms.Panel()
        Me.lblAyuda = New System.Windows.Forms.Label()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.flpFiltrosActivos = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlSuperior.SuspendLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlSuperior
        '
        Me.pnlSuperior.Controls.Add(Me.lblAyuda)
        Me.pnlSuperior.Controls.Add(Me.txtBusqueda)
        Me.pnlSuperior.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlSuperior.Location = New System.Drawing.Point(0, 0)
        Me.pnlSuperior.Name = "pnlSuperior"
        Me.pnlSuperior.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlSuperior.Size = New System.Drawing.Size(984, 80)
        Me.pnlSuperior.TabIndex = 0
        '
        'lblAyuda
        '
        Me.lblAyuda.AutoSize = True
        Me.lblAyuda.ForeColor = System.Drawing.SystemColors.GrayText
        Me.lblAyuda.Location = New System.Drawing.Point(13, 45)
        Me.lblAyuda.Name = "lblAyuda"
        Me.lblAyuda.Size = New System.Drawing.Size(355, 20)
        Me.lblAyuda.TabIndex = 1
        Me.lblAyuda.Text = "Ej: juan perez activo:si cargo:agente ci:12345678"
        '
        'txtBusqueda
        '
        Me.txtBusqueda.Dock = System.Windows.Forms.DockStyle.Top
        Me.txtBusqueda.Font = New System.Drawing.Font("Segoe UI", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBusqueda.Location = New System.Drawing.Point(10, 10)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(964, 30)
        Me.txtBusqueda.TabIndex = 0
        '
        'dgvResultados
        '
        Me.dgvResultados.AllowUserToAddRows = False
        Me.dgvResultados.AllowUserToDeleteRows = False
        Me.dgvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResultados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvResultados.Location = New System.Drawing.Point(0, 114)
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.ReadOnly = True
        Me.dgvResultados.RowHeadersWidth = 51
        Me.dgvResultados.RowTemplate.Height = 24
        Me.dgvResultados.Size = New System.Drawing.Size(984, 448)
        Me.dgvResultados.TabIndex = 2
        '
        'flpFiltrosActivos
        '
        Me.flpFiltrosActivos.AutoSize = True
        Me.flpFiltrosActivos.Dock = System.Windows.Forms.DockStyle.Top
        Me.flpFiltrosActivos.Location = New System.Drawing.Point(0, 80)
        Me.flpFiltrosActivos.MinimumSize = New System.Drawing.Size(0, 34)
        Me.flpFiltrosActivos.Name = "flpFiltrosActivos"
        Me.flpFiltrosActivos.Padding = New System.Windows.Forms.Padding(10, 5, 10, 5)
        Me.flpFiltrosActivos.Size = New System.Drawing.Size(984, 34)
        Me.flpFiltrosActivos.TabIndex = 1
        '
        'frmFiltroUnificado
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 562)
        Me.Controls.Add(Me.dgvResultados)
        Me.Controls.Add(Me.flpFiltrosActivos)
        Me.Controls.Add(Me.pnlSuperior)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmFiltroUnificado"
        Me.Text = "Filtro Unificado (Alternativa)"
        Me.pnlSuperior.ResumeLayout(False)
        Me.pnlSuperior.PerformLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pnlSuperior As Panel
    Friend WithEvents txtBusqueda As TextBox
    Friend WithEvents dgvResultados As DataGridView
    Friend WithEvents flpFiltrosActivos As FlowLayoutPanel
    Friend WithEvents lblAyuda As Label
End Class