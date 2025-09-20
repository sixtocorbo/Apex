<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmElegirDesignacion
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
        Me.dgvDesignaciones = New System.Windows.Forms.DataGridView()
        Me.MainLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.lblInstruccion = New System.Windows.Forms.Label()
        Me.pnlBotones = New System.Windows.Forms.TableLayoutPanel()
        Me.btnSeleccionar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        CType(Me.dgvDesignaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainLayout.SuspendLayout()
        Me.pnlBotones.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvDesignaciones
        '
        Me.dgvDesignaciones.AllowUserToAddRows = False
        Me.dgvDesignaciones.AllowUserToDeleteRows = False
        Me.dgvDesignaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDesignaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDesignaciones.Location = New System.Drawing.Point(3, 33)
        Me.dgvDesignaciones.MultiSelect = False
        Me.dgvDesignaciones.Name = "dgvDesignaciones"
        Me.dgvDesignaciones.ReadOnly = True
        Me.dgvDesignaciones.RowHeadersVisible = False
        Me.dgvDesignaciones.RowHeadersWidth = 51
        Me.dgvDesignaciones.RowTemplate.Height = 24
        Me.dgvDesignaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDesignaciones.Size = New System.Drawing.Size(478, 201)
        Me.dgvDesignaciones.TabIndex = 1
        '
        'MainLayout
        '
        Me.MainLayout.ColumnCount = 1
        Me.MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.Controls.Add(Me.lblInstruccion, 0, 0)
        Me.MainLayout.Controls.Add(Me.dgvDesignaciones, 0, 1)
        Me.MainLayout.Controls.Add(Me.pnlBotones, 0, 2)
        Me.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainLayout.Location = New System.Drawing.Point(10, 10)
        Me.MainLayout.Name = "MainLayout"
        Me.MainLayout.RowCount = 3
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.Size = New System.Drawing.Size(484, 291)
        Me.MainLayout.TabIndex = 5
        '
        'lblInstruccion
        '
        Me.lblInstruccion.AutoSize = True
        Me.lblInstruccion.Location = New System.Drawing.Point(3, 0)
        Me.lblInstruccion.Margin = New System.Windows.Forms.Padding(3, 0, 3, 10)
        Me.lblInstruccion.Name = "lblInstruccion"
        Me.lblInstruccion.Size = New System.Drawing.Size(461, 20)
        Me.lblInstruccion.TabIndex = 0
        Me.lblInstruccion.Text = "Se encontró más de una designación. Por favor, seleccione una:"
        '
        'pnlBotones
        '
        Me.pnlBotones.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.pnlBotones.AutoSize = True
        Me.pnlBotones.ColumnCount = 2
        Me.pnlBotones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.pnlBotones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.pnlBotones.Controls.Add(Me.btnSeleccionar, 0, 0)
        Me.pnlBotones.Controls.Add(Me.btnCancelar, 1, 0)
        Me.pnlBotones.Location = New System.Drawing.Point(207, 240)
        Me.pnlBotones.Margin = New System.Windows.Forms.Padding(3, 3, 0, 0)
        Me.pnlBotones.Name = "pnlBotones"
        Me.pnlBotones.RowCount = 1
        Me.pnlBotones.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlBotones.Size = New System.Drawing.Size(277, 51)
        Me.pnlBotones.TabIndex = 2
        '
        'btnSeleccionar
        '
        Me.btnSeleccionar.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSeleccionar.Location = New System.Drawing.Point(3, 3)
        Me.btnSeleccionar.Name = "btnSeleccionar"
        Me.btnSeleccionar.Size = New System.Drawing.Size(160, 45)
        Me.btnSeleccionar.TabIndex = 0
        Me.btnSeleccionar.Text = "Ver Reporte"
        Me.btnSeleccionar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(169, 3)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(105, 45)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmElegirDesignacion
        '
        Me.AcceptButton = Me.btnSeleccionar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(504, 311)
        Me.Controls.Add(Me.MainLayout)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(520, 350)
        Me.Name = "frmElegirDesignacion"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Seleccionar Designación"
        CType(Me.dgvDesignaciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainLayout.ResumeLayout(False)
        Me.MainLayout.PerformLayout()
        Me.pnlBotones.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lblInstruccion As Label
    Friend WithEvents btnSeleccionar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents dgvDesignaciones As DataGridView
    Friend WithEvents MainLayout As TableLayoutPanel
    Friend WithEvents pnlBotones As TableLayoutPanel
End Class