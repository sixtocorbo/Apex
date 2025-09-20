<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmIncidencias
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
        Me.dgvIncidencias = New System.Windows.Forms.DataGridView()
        Me.btnAgregar = New System.Windows.Forms.Button()
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.btnCerrar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.MainLayout = New System.Windows.Forms.TableLayoutPanel()
        CType(Me.dgvIncidencias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.MainLayout.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvIncidencias
        '
        Me.dgvIncidencias.AllowUserToAddRows = False
        Me.dgvIncidencias.AllowUserToDeleteRows = False
        Me.dgvIncidencias.AllowUserToResizeColumns = False
        Me.dgvIncidencias.AllowUserToResizeRows = False
        Me.dgvIncidencias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvIncidencias.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvIncidencias.Location = New System.Drawing.Point(4, 37)
        Me.dgvIncidencias.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvIncidencias.MultiSelect = False
        Me.dgvIncidencias.Name = "dgvIncidencias"
        Me.dgvIncidencias.ReadOnly = True
        Me.dgvIncidencias.RowHeadersWidth = 62
        Me.dgvIncidencias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvIncidencias.Size = New System.Drawing.Size(868, 438)
        Me.dgvIncidencias.TabIndex = 1
        '
        'btnAgregar
        '
        Me.btnAgregar.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnAgregar.Location = New System.Drawing.Point(9, 12)
        Me.btnAgregar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(112, 35)
        Me.btnAgregar.TabIndex = 0
        Me.btnAgregar.Text = "Agregar"
        Me.btnAgregar.UseVisualStyleBackColor = True
        '
        'btnEditar
        '
        Me.btnEditar.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.btnEditar.Location = New System.Drawing.Point(129, 12)
        Me.btnEditar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(112, 35)
        Me.btnEditar.TabIndex = 1
        Me.btnEditar.Text = "Editar"
        Me.btnEditar.UseVisualStyleBackColor = True
        '
        'btnCerrar
        '
        Me.btnCerrar.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.btnCerrar.Location = New System.Drawing.Point(755, 12)
        Me.btnCerrar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCerrar.Name = "btnCerrar"
        Me.btnCerrar.Size = New System.Drawing.Size(112, 35)
        Me.btnCerrar.TabIndex = 2
        Me.btnCerrar.Text = "Cerrar"
        Me.btnCerrar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 0)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(360, 32)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Mantenimiento de Incidencias"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.btnCerrar, 3, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnEditar, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.btnAgregar, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 480)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.Padding = New System.Windows.Forms.Padding(5)
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(876, 60)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'MainLayout
        '
        Me.MainLayout.ColumnCount = 1
        Me.MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.Controls.Add(Me.TableLayoutPanel1, 0, 2)
        Me.MainLayout.Controls.Add(Me.dgvIncidencias, 0, 1)
        Me.MainLayout.Controls.Add(Me.Label1, 0, 0)
        Me.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainLayout.Location = New System.Drawing.Point(0, 0)
        Me.MainLayout.Name = "MainLayout"
        Me.MainLayout.RowCount = 3
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.Size = New System.Drawing.Size(876, 540)
        Me.MainLayout.TabIndex = 7
        '
        'frmIncidencias
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(876, 540)
        Me.Controls.Add(Me.MainLayout)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(889, 570)
        Me.Name = "frmIncidencias"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Incidencias"
        CType(Me.dgvIncidencias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.MainLayout.ResumeLayout(False)
        Me.MainLayout.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents dgvIncidencias As DataGridView
    Friend WithEvents btnAgregar As Button
    Friend WithEvents btnEditar As Button
    Friend WithEvents btnCerrar As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents MainLayout As TableLayoutPanel '--> AÑADIDO
End Class