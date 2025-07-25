<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFiltros
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
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.btnLimpiarFiltros = New System.Windows.Forms.Button()
        Me.btnQuitarFiltro = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lstFiltrosActivos = New System.Windows.Forms.ListBox()
        Me.btnAgregarFiltro = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lstValores = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lstColumnas = New System.Windows.Forms.ListBox()
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.lblConteo = New System.Windows.Forms.Label()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.lblConteo)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnBuscar)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnLimpiarFiltros)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnQuitarFiltro)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label3)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstFiltrosActivos)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnAgregarFiltro)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label2)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstValores)
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.lstColumnas)
        Me.SplitContainer1.Panel1.Padding = New System.Windows.Forms.Padding(5)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.dgvResultados)
        Me.SplitContainer1.Size = New System.Drawing.Size(1082, 653)
        Me.SplitContainer1.SplitterDistance = 250
        Me.SplitContainer1.TabIndex = 0
        '
        'btnBuscar
        '
        Me.btnBuscar.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnBuscar.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBuscar.Location = New System.Drawing.Point(5, 574)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(240, 37)
        Me.btnBuscar.TabIndex = 9
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'lblConteo
        '
        Me.lblConteo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblConteo.Location = New System.Drawing.Point(5, 611)
        Me.lblConteo.Name = "lblConteo"
        Me.lblConteo.Size = New System.Drawing.Size(240, 37)
        Me.lblConteo.TabIndex = 10
        Me.lblConteo.Text = "Registros: 0"
        Me.lblConteo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnLimpiarFiltros
        '
        Me.btnLimpiarFiltros.Location = New System.Drawing.Point(145, 532)
        Me.btnLimpiarFiltros.Name = "btnLimpiarFiltros"
        Me.btnLimpiarFiltros.Size = New System.Drawing.Size(94, 29)
        Me.btnLimpiarFiltros.TabIndex = 8
        Me.btnLimpiarFiltros.Text = "Limpiar"
        Me.btnLimpiarFiltros.UseVisualStyleBackColor = True
        '
        'btnQuitarFiltro
        '
        Me.btnQuitarFiltro.Location = New System.Drawing.Point(8, 532)
        Me.btnQuitarFiltro.Name = "btnQuitarFiltro"
        Me.btnQuitarFiltro.Size = New System.Drawing.Size(94, 29)
        Me.btnQuitarFiltro.TabIndex = 7
        Me.btnQuitarFiltro.Text = "Quitar"
        Me.btnQuitarFiltro.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 401)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(102, 20)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Filtros Activos:"
        '
        'lstFiltrosActivos
        '
        Me.lstFiltrosActivos.FormattingEnabled = True
        Me.lstFiltrosActivos.ItemHeight = 20
        Me.lstFiltrosActivos.Location = New System.Drawing.Point(8, 424)
        Me.lstFiltrosActivos.Name = "lstFiltrosActivos"
        Me.lstFiltrosActivos.Size = New System.Drawing.Size(231, 104)
        Me.lstFiltrosActivos.TabIndex = 5
        '
        'btnAgregarFiltro
        '
        Me.btnAgregarFiltro.Location = New System.Drawing.Point(8, 360)
        Me.btnAgregarFiltro.Name = "btnAgregarFiltro"
        Me.btnAgregarFiltro.Size = New System.Drawing.Size(231, 29)
        Me.btnAgregarFiltro.TabIndex = 4
        Me.btnAgregarFiltro.Text = "↓ Agregar Filtro ↓"
        Me.btnAgregarFiltro.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 182)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(130, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Seleccionar Valor:"
        '
        'lstValores
        '
        Me.lstValores.FormattingEnabled = True
        Me.lstValores.ItemHeight = 20
        Me.lstValores.Location = New System.Drawing.Point(8, 205)
        Me.lstValores.Name = "lstValores"
        Me.lstValores.Size = New System.Drawing.Size(231, 144)
        Me.lstValores.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(155, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Seleccionar Columna:"
        '
        'lstColumnas
        '
        Me.lstColumnas.FormattingEnabled = True
        Me.lstColumnas.ItemHeight = 20
        Me.lstColumnas.Location = New System.Drawing.Point(8, 31)
        Me.lstColumnas.Name = "lstColumnas"
        Me.lstColumnas.Size = New System.Drawing.Size(231, 144)
        Me.lstColumnas.TabIndex = 0
        '
        'dgvResultados
        '
        Me.dgvResultados.AllowUserToAddRows = False
        Me.dgvResultados.AllowUserToDeleteRows = False
        Me.dgvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResultados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvResultados.Location = New System.Drawing.Point(0, 0)
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.ReadOnly = True
        Me.dgvResultados.RowHeadersWidth = 51
        Me.dgvResultados.RowTemplate.Height = 29
        Me.dgvResultados.Size = New System.Drawing.Size(828, 653)
        Me.dgvResultados.TabIndex = 0
        '
        'frmFiltros
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1082, 653)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Name = "frmFiltros"
        Me.Text = "Filtros Avanzados"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
    End Sub

    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents Label1 As Label
    Friend WithEvents lstColumnas As ListBox
    Friend WithEvents btnAgregarFiltro As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents lstValores As ListBox
    Friend WithEvents btnLimpiarFiltros As Button
    Friend WithEvents btnQuitarFiltro As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents lstFiltrosActivos As ListBox
    Friend WithEvents btnBuscar As Button
    Friend WithEvents dgvResultados As DataGridView
    Friend WithEvents lblConteo As Label
End Class