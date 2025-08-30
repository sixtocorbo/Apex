<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNovedadesListas
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnLimpiarFuncionarios = New System.Windows.Forms.Button()
        Me.btnQuitarFuncionario = New System.Windows.Forms.Button()
        Me.btnAgregarFuncionario = New System.Windows.Forms.Button()
        Me.lstFuncionarios = New System.Windows.Forms.ListBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.chkFiltrarPorFecha = New System.Windows.Forms.CheckBox()
        Me.dtpFechaHasta = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpFechaDesde = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnGenerarReporte = New System.Windows.Forms.Button()
        Me.dgvNovedades = New System.Windows.Forms.DataGridView()
        Me.btnImprimir = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvNovedades, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.btnLimpiarFuncionarios)
        Me.GroupBox1.Controls.Add(Me.btnQuitarFuncionario)
        Me.GroupBox1.Controls.Add(Me.btnAgregarFuncionario)
        Me.GroupBox1.Controls.Add(Me.lstFuncionarios)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.chkFiltrarPorFecha)
        Me.GroupBox1.Controls.Add(Me.dtpFechaHasta)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.dtpFechaDesde)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(18, 18)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Size = New System.Drawing.Size(1140, 215)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Filtros del Reporte"
        '
        'btnLimpiarFuncionarios
        '
        Me.btnLimpiarFuncionarios.Location = New System.Drawing.Point(1005, 123)
        Me.btnLimpiarFuncionarios.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnLimpiarFuncionarios.Name = "btnLimpiarFuncionarios"
        Me.btnLimpiarFuncionarios.Size = New System.Drawing.Size(112, 35)
        Me.btnLimpiarFuncionarios.TabIndex = 9
        Me.btnLimpiarFuncionarios.Text = "Limpiar"
        Me.btnLimpiarFuncionarios.UseVisualStyleBackColor = True
        '
        'btnQuitarFuncionario
        '
        Me.btnQuitarFuncionario.Location = New System.Drawing.Point(1005, 78)
        Me.btnQuitarFuncionario.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnQuitarFuncionario.Name = "btnQuitarFuncionario"
        Me.btnQuitarFuncionario.Size = New System.Drawing.Size(112, 35)
        Me.btnQuitarFuncionario.TabIndex = 8
        Me.btnQuitarFuncionario.Text = "Quitar"
        Me.btnQuitarFuncionario.UseVisualStyleBackColor = True
        '
        'btnAgregarFuncionario
        '
        Me.btnAgregarFuncionario.Location = New System.Drawing.Point(1005, 34)
        Me.btnAgregarFuncionario.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAgregarFuncionario.Name = "btnAgregarFuncionario"
        Me.btnAgregarFuncionario.Size = New System.Drawing.Size(112, 35)
        Me.btnAgregarFuncionario.TabIndex = 7
        Me.btnAgregarFuncionario.Text = "Agregar..."
        Me.btnAgregarFuncionario.UseVisualStyleBackColor = True
        '
        'lstFuncionarios
        '
        Me.lstFuncionarios.FormattingEnabled = True
        Me.lstFuncionarios.ItemHeight = 20
        Me.lstFuncionarios.Location = New System.Drawing.Point(600, 34)
        Me.lstFuncionarios.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.lstFuncionarios.Name = "lstFuncionarios"
        Me.lstFuncionarios.Size = New System.Drawing.Size(394, 164)
        Me.lstFuncionarios.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(488, 38)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 20)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Funcionarios:"
        '
        'chkFiltrarPorFecha
        '
        Me.chkFiltrarPorFecha.AutoSize = True
        Me.chkFiltrarPorFecha.Checked = True
        Me.chkFiltrarPorFecha.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFiltrarPorFecha.Location = New System.Drawing.Point(14, 38)
        Me.chkFiltrarPorFecha.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkFiltrarPorFecha.Name = "chkFiltrarPorFecha"
        Me.chkFiltrarPorFecha.Size = New System.Drawing.Size(151, 24)
        Me.chkFiltrarPorFecha.TabIndex = 4
        Me.chkFiltrarPorFecha.Text = "Filtrar por Fecha"
        Me.chkFiltrarPorFecha.UseVisualStyleBackColor = True
        '
        'dtpFechaHasta
        '
        Me.dtpFechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaHasta.Location = New System.Drawing.Point(81, 120)
        Me.dtpFechaHasta.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFechaHasta.Name = "dtpFechaHasta"
        Me.dtpFechaHasta.Size = New System.Drawing.Size(170, 26)
        Me.dtpFechaHasta.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 125)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Hasta:"
        '
        'dtpFechaDesde
        '
        Me.dtpFechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaDesde.Location = New System.Drawing.Point(81, 78)
        Me.dtpFechaDesde.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFechaDesde.Name = "dtpFechaDesde"
        Me.dtpFechaDesde.Size = New System.Drawing.Size(170, 26)
        Me.dtpFechaDesde.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 83)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Desde:"
        '
        'btnGenerarReporte
        '
        Me.btnGenerarReporte.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarReporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGenerarReporte.Location = New System.Drawing.Point(794, 243)
        Me.btnGenerarReporte.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGenerarReporte.Name = "btnGenerarReporte"
        Me.btnGenerarReporte.Size = New System.Drawing.Size(180, 43)
        Me.btnGenerarReporte.TabIndex = 1
        Me.btnGenerarReporte.Text = "Generar Reporte"
        Me.btnGenerarReporte.UseVisualStyleBackColor = True
        '
        'dgvNovedades
        '
        Me.dgvNovedades.AllowUserToAddRows = False
        Me.dgvNovedades.AllowUserToDeleteRows = False
        Me.dgvNovedades.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvNovedades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNovedades.Location = New System.Drawing.Point(18, 295)
        Me.dgvNovedades.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvNovedades.Name = "dgvNovedades"
        Me.dgvNovedades.ReadOnly = True
        Me.dgvNovedades.RowHeadersWidth = 62
        Me.dgvNovedades.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNovedades.Size = New System.Drawing.Size(1140, 395)
        Me.dgvNovedades.TabIndex = 2
        '
        'btnImprimir
        '
        Me.btnImprimir.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImprimir.Location = New System.Drawing.Point(978, 243)
        Me.btnImprimir.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnImprimir.Name = "btnImprimir"
        Me.btnImprimir.Size = New System.Drawing.Size(180, 43)
        Me.btnImprimir.TabIndex = 3
        Me.btnImprimir.Text = "Imprimir"
        Me.btnImprimir.UseVisualStyleBackColor = True
        '
        'frmNovedadesListas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1176, 709)
        Me.Controls.Add(Me.btnImprimir)
        Me.Controls.Add(Me.dgvNovedades)
        Me.Controls.Add(Me.btnGenerarReporte)
        Me.Controls.Add(Me.GroupBox1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmNovedadesListas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Reporte de Novedades"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dgvNovedades, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnGenerarReporte As Button
    Friend WithEvents dgvNovedades As DataGridView
    Friend WithEvents dtpFechaDesde As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents dtpFechaHasta As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents chkFiltrarPorFecha As CheckBox
    Friend WithEvents Label3 As Label
    Friend WithEvents lstFuncionarios As ListBox
    Friend WithEvents btnLimpiarFuncionarios As Button
    Friend WithEvents btnQuitarFuncionario As Button
    Friend WithEvents btnAgregarFuncionario As Button
    Friend WithEvents btnImprimir As Button
End Class
