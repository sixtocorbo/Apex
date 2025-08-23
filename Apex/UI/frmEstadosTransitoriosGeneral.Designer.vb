<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEstadosTransitoriosGeneral
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
        Me.dgvEstados = New System.Windows.Forms.DataGridView()
        Me.btnVerDetalles = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtFiltro = New System.Windows.Forms.TextBox()
        CType(Me.dgvEstados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'dgvEstados
        '
        Me.dgvEstados.AllowUserToAddRows = False
        Me.dgvEstados.AllowUserToDeleteRows = False
        Me.dgvEstados.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvEstados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEstados.Location = New System.Drawing.Point(12, 38)
        Me.dgvEstados.MultiSelect = False
        Me.dgvEstados.Name = "dgvEstados"
        Me.dgvEstados.ReadOnly = True
        Me.dgvEstados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEstados.Size = New System.Drawing.Size(760, 372)
        Me.dgvEstados.TabIndex = 0
        '
        'btnVerDetalles
        '
        Me.btnVerDetalles.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVerDetalles.Location = New System.Drawing.Point(697, 416)
        Me.btnVerDetalles.Name = "btnVerDetalles"
        Me.btnVerDetalles.Size = New System.Drawing.Size(75, 23)
        Me.btnVerDetalles.TabIndex = 1
        Me.btnVerDetalles.Text = "Ver Detalles"
        Me.btnVerDetalles.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCerrar.Location = New System.Drawing.Point(12, 416)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(75, 23)
        Me.btnCerrar.TabIndex = 2
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(183, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Filtrar por Nombre, Apellido o Cédula:"
        '
        'txtFiltro
        '
        Me.txtFiltro.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFiltro.Location = New System.Drawing.Point(201, 12)
        Me.txtFiltro.Name = "txtFiltro"
        Me.txtFiltro.Size = New System.Drawing.Size(571, 20)
        Me.txtFiltro.TabIndex = 4
        '
        'frmEstadosTransitoriosGeneral
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 451)
        Me.Controls.Add(Me.txtFiltro)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCerrar)
        Me.Controls.Add(Me.btnVerDetalles)
        Me.Controls.Add(Me.dgvEstados)
        Me.MinimumSize = New System.Drawing.Size(800, 490)
        Me.Name = "frmEstadosTransitoriosGeneral"
        Me.Text = "Visor de Estados Transitorios"
        CType(Me.dgvEstados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents dgvEstados As DataGridView
    Friend WithEvents btnVerDetalles As Button
    Friend WithEvents btnCerrar As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtFiltro As TextBox
End Class