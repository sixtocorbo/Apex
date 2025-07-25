' Reemplaza todo el contenido de tu archivo Apex/UI/frmFiltroAvanzado.Designer.vb con este código.

Option Strict On
Option Explicit On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFiltroAvanzado
    Inherits System.Windows.Forms.Form

    '--- Declaración de controles -------------------------------------------------
    Private components As System.ComponentModel.IContainer
    Friend WithEvents pnlSuperior As FlowLayoutPanel
    Friend WithEvents lstColumnas As ListBox
    Friend WithEvents lstValores As ListBox
    Friend WithEvents btnAgregar As Button
    Friend WithEvents btnLimpiar As Button
    Friend WithEvents txtBusquedaGlobal As TextBox
    Friend WithEvents flpChips As FlowLayoutPanel
    Friend WithEvents dgvDatos As DataGridView
    Friend WithEvents btnExportarExcel As Button
    Friend WithEvents cmbOrigenDatos As ComboBox
    Friend WithEvents dtpFechaInicio As DateTimePicker
    Friend WithEvents dtpFechaFin As DateTimePicker
    Friend WithEvents btnCargar As Button
    Friend WithEvents btnExportarFichasPDF As Button
    Friend WithEvents lblConteoRegistros As Label
    Friend WithEvents btnCopiarCorreos As Button

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.pnlSuperior = New System.Windows.Forms.FlowLayoutPanel()
        Me.lstColumnas = New System.Windows.Forms.ListBox()
        Me.lstValores = New System.Windows.Forms.ListBox()
        Me.txtBusquedaGlobal = New System.Windows.Forms.TextBox()
        Me.btnAgregar = New System.Windows.Forms.Button()
        Me.btnLimpiar = New System.Windows.Forms.Button()
        Me.cmbOrigenDatos = New System.Windows.Forms.ComboBox()
        Me.dtpFechaInicio = New System.Windows.Forms.DateTimePicker()
        Me.dtpFechaFin = New System.Windows.Forms.DateTimePicker()
        Me.btnCargar = New System.Windows.Forms.Button()
        Me.btnExportarExcel = New System.Windows.Forms.Button()
        Me.btnCopiarCorreos = New System.Windows.Forms.Button()
        Me.btnExportarFichasPDF = New System.Windows.Forms.Button()
        Me.lblConteoRegistros = New System.Windows.Forms.Label()
        Me.flpChips = New System.Windows.Forms.FlowLayoutPanel()
        Me.dgvDatos = New System.Windows.Forms.DataGridView()
        Me.pnlSuperior.SuspendLayout()
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlSuperior
        '
        Me.pnlSuperior.AutoSize = True
        Me.pnlSuperior.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlSuperior.Controls.Add(Me.lstColumnas)
        Me.pnlSuperior.Controls.Add(Me.lstValores)
        Me.pnlSuperior.Controls.Add(Me.txtBusquedaGlobal)
        Me.pnlSuperior.Controls.Add(Me.btnAgregar)
        Me.pnlSuperior.Controls.Add(Me.btnLimpiar)
        Me.pnlSuperior.Controls.Add(Me.cmbOrigenDatos)
        Me.pnlSuperior.Controls.Add(Me.dtpFechaInicio)
        Me.pnlSuperior.Controls.Add(Me.dtpFechaFin)
        Me.pnlSuperior.Controls.Add(Me.btnCargar)
        Me.pnlSuperior.Controls.Add(Me.btnExportarExcel)
        Me.pnlSuperior.Controls.Add(Me.btnCopiarCorreos)
        Me.pnlSuperior.Controls.Add(Me.btnExportarFichasPDF)
        Me.pnlSuperior.Controls.Add(Me.lblConteoRegistros)
        Me.pnlSuperior.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlSuperior.Location = New System.Drawing.Point(0, 0)
        Me.pnlSuperior.Name = "pnlSuperior"
        Me.pnlSuperior.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlSuperior.Size = New System.Drawing.Size(1509, 423)
        Me.pnlSuperior.TabIndex = 2
        '
        'lstColumnas
        '
        Me.lstColumnas.ItemHeight = 23
        Me.lstColumnas.Location = New System.Drawing.Point(13, 13)
        Me.lstColumnas.Name = "lstColumnas"
        Me.lstColumnas.Size = New System.Drawing.Size(195, 349)
        Me.lstColumnas.TabIndex = 1
        '
        'lstValores
        '
        Me.lstValores.ItemHeight = 23
        Me.lstValores.Location = New System.Drawing.Point(214, 13)
        Me.lstValores.Name = "lstValores"
        Me.lstValores.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstValores.Size = New System.Drawing.Size(236, 349)
        Me.lstValores.TabIndex = 5
        '
        'txtBusquedaGlobal
        '
        Me.txtBusquedaGlobal.Location = New System.Drawing.Point(456, 13)
        Me.txtBusquedaGlobal.Name = "txtBusquedaGlobal"
        Me.txtBusquedaGlobal.Size = New System.Drawing.Size(174, 30)
        Me.txtBusquedaGlobal.TabIndex = 8
        '
        'btnAgregar
        '
        Me.btnAgregar.Location = New System.Drawing.Point(636, 13)
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(75, 30)
        Me.btnAgregar.TabIndex = 7
        Me.btnAgregar.Text = "Filtrar"
        '
        'btnLimpiar
        '
        Me.btnLimpiar.Location = New System.Drawing.Point(717, 13)
        Me.btnLimpiar.Name = "btnLimpiar"
        Me.btnLimpiar.Size = New System.Drawing.Size(103, 30)
        Me.btnLimpiar.TabIndex = 10
        Me.btnLimpiar.Text = "Limpiar"
        '
        'cmbOrigenDatos
        '
        Me.cmbOrigenDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrigenDatos.FormattingEnabled = True
        Me.cmbOrigenDatos.Location = New System.Drawing.Point(826, 13)
        Me.cmbOrigenDatos.Name = "cmbOrigenDatos"
        Me.cmbOrigenDatos.Size = New System.Drawing.Size(175, 31)
        Me.cmbOrigenDatos.TabIndex = 13
        '
        'dtpFechaInicio
        '
        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFechaInicio.Location = New System.Drawing.Point(1007, 13)
        Me.dtpFechaInicio.Name = "dtpFechaInicio"
        Me.dtpFechaInicio.Size = New System.Drawing.Size(136, 30)
        Me.dtpFechaInicio.TabIndex = 14
        '
        'dtpFechaFin
        '
        Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFechaFin.Location = New System.Drawing.Point(1149, 13)
        Me.dtpFechaFin.Name = "dtpFechaFin"
        Me.dtpFechaFin.Size = New System.Drawing.Size(134, 30)
        Me.dtpFechaFin.TabIndex = 15
        '
        'btnCargar
        '
        Me.btnCargar.Location = New System.Drawing.Point(1289, 13)
        Me.btnCargar.Name = "btnCargar"
        Me.btnCargar.Size = New System.Drawing.Size(100, 30)
        Me.btnCargar.TabIndex = 16
        Me.btnCargar.Text = "Cargar"
        '
        'btnExportarExcel
        '
        Me.btnExportarExcel.Location = New System.Drawing.Point(13, 368)
        Me.btnExportarExcel.Name = "btnExportarExcel"
        Me.btnExportarExcel.Size = New System.Drawing.Size(151, 30)
        Me.btnExportarExcel.TabIndex = 12
        Me.btnExportarExcel.Text = "Exportar Excel"
        '
        'btnCopiarCorreos
        '
        Me.btnCopiarCorreos.Location = New System.Drawing.Point(170, 368)
        Me.btnCopiarCorreos.Name = "btnCopiarCorreos"
        Me.btnCopiarCorreos.Size = New System.Drawing.Size(154, 30)
        Me.btnCopiarCorreos.TabIndex = 19
        Me.btnCopiarCorreos.Text = "Copiar correos"
        '
        'btnExportarFichasPDF
        '
        Me.btnExportarFichasPDF.Location = New System.Drawing.Point(330, 368)
        Me.btnExportarFichasPDF.Name = "btnExportarFichasPDF"
        Me.btnExportarFichasPDF.Size = New System.Drawing.Size(151, 30)
        Me.btnExportarFichasPDF.TabIndex = 17
        Me.btnExportarFichasPDF.Text = "Exportar Fichas"
        '
        'lblConteoRegistros
        '
        Me.lblConteoRegistros.AutoSize = True
        Me.lblConteoRegistros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblConteoRegistros.Font = New System.Drawing.Font("Segoe UI", 20.0!)
        Me.lblConteoRegistros.ForeColor = System.Drawing.Color.Red
        Me.lblConteoRegistros.Location = New System.Drawing.Point(487, 365)
        Me.lblConteoRegistros.Name = "lblConteoRegistros"
        Me.lblConteoRegistros.Size = New System.Drawing.Size(2, 48)
        Me.lblConteoRegistros.TabIndex = 18
        '
        'flpChips
        '
        Me.flpChips.AutoSize = True
        Me.flpChips.Dock = System.Windows.Forms.DockStyle.Top
        Me.flpChips.Location = New System.Drawing.Point(0, 423)
        Me.flpChips.Name = "flpChips"
        Me.flpChips.Padding = New System.Windows.Forms.Padding(10)
        Me.flpChips.Size = New System.Drawing.Size(1509, 20)
        Me.flpChips.TabIndex = 1
        '
        'dgvDatos
        '
        Me.dgvDatos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.dgvDatos.ColumnHeadersHeight = 29
        Me.dgvDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDatos.Location = New System.Drawing.Point(0, 443)
        Me.dgvDatos.Name = "dgvDatos"
        Me.dgvDatos.RowHeadersWidth = 51
        Me.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDatos.Size = New System.Drawing.Size(1509, 157)
        Me.dgvDatos.TabIndex = 0
        '
        'frmFiltroAvanzado
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 23.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1509, 600)
        Me.Controls.Add(Me.dgvDatos)
        Me.Controls.Add(Me.flpChips)
        Me.Controls.Add(Me.pnlSuperior)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.2!)
        Me.Name = "frmFiltroAvanzado"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filtro avanzado"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlSuperior.ResumeLayout(False)
        Me.pnlSuperior.PerformLayout()
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

End Class
''------------------------------------------------------------------------------
'' <auto-generated>
''     Diseñador de Windows Forms: versión con ListBox de columnas, operadores y valores.
''------------------------------------------------------------------------------
'Option Strict On
'Option Explicit On

'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
'Partial Class frmFiltroAvanzado
'    Inherits System.Windows.Forms.Form

'    '--- Declaración de controles -------------------------------------------------
'    Private components As System.ComponentModel.IContainer
'    Friend WithEvents pnlSuperior As FlowLayoutPanel
'    Friend WithEvents lstColumnas As ListBox
'    Friend WithEvents lstValores As ListBox
'    Friend WithEvents btnAgregar As Button
'    Friend WithEvents btnLimpiar As Button
'    Friend WithEvents txtBusquedaGlobal As TextBox
'    Friend WithEvents flpChips As FlowLayoutPanel
'    Friend WithEvents dgvDatos As DataGridView

'    <System.Diagnostics.DebuggerStepThrough()>
'    Private Sub InitializeComponent()
'        Me.pnlSuperior = New System.Windows.Forms.FlowLayoutPanel()
'        Me.lstColumnas = New System.Windows.Forms.ListBox()
'        Me.lstValores = New System.Windows.Forms.ListBox()
'        Me.txtBusquedaGlobal = New System.Windows.Forms.TextBox()
'        Me.btnAgregar = New System.Windows.Forms.Button()
'        Me.btnDeshacer = New System.Windows.Forms.Button()
'        Me.btnLimpiar = New System.Windows.Forms.Button()
'        Me.cmbOrigenDatos = New System.Windows.Forms.ComboBox()
'        Me.dtpFechaInicio = New System.Windows.Forms.DateTimePicker()
'        Me.dtpFechaFin = New System.Windows.Forms.DateTimePicker()
'        Me.btnCargar = New System.Windows.Forms.Button()
'        Me.btnExportarExcel = New System.Windows.Forms.Button()
'        Me.btnCopiarCorreos = New System.Windows.Forms.Button()
'        Me.btnExportarFichasPDF = New System.Windows.Forms.Button()
'        Me.lblConteoRegistros = New System.Windows.Forms.Label()
'        Me.flpChips = New System.Windows.Forms.FlowLayoutPanel()
'        Me.dgvDatos = New System.Windows.Forms.DataGridView()
'        Me.pnlSuperior.SuspendLayout()
'        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).BeginInit()
'        Me.SuspendLayout()
'        '
'        'pnlSuperior
'        '
'        Me.pnlSuperior.AutoSize = True
'        Me.pnlSuperior.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
'        Me.pnlSuperior.Controls.Add(Me.lstColumnas)
'        Me.pnlSuperior.Controls.Add(Me.lstValores)
'        Me.pnlSuperior.Controls.Add(Me.txtBusquedaGlobal)
'        Me.pnlSuperior.Controls.Add(Me.btnAgregar)
'        Me.pnlSuperior.Controls.Add(Me.btnDeshacer)
'        Me.pnlSuperior.Controls.Add(Me.btnLimpiar)
'        Me.pnlSuperior.Controls.Add(Me.cmbOrigenDatos)
'        Me.pnlSuperior.Controls.Add(Me.dtpFechaInicio)
'        Me.pnlSuperior.Controls.Add(Me.dtpFechaFin)
'        Me.pnlSuperior.Controls.Add(Me.btnCargar)
'        Me.pnlSuperior.Controls.Add(Me.btnExportarExcel)
'        Me.pnlSuperior.Controls.Add(Me.btnCopiarCorreos)
'        Me.pnlSuperior.Controls.Add(Me.btnExportarFichasPDF)
'        Me.pnlSuperior.Controls.Add(Me.lblConteoRegistros)
'        Me.pnlSuperior.Dock = System.Windows.Forms.DockStyle.Top
'        Me.pnlSuperior.Location = New System.Drawing.Point(0, 0)
'        Me.pnlSuperior.Name = "pnlSuperior"
'        Me.pnlSuperior.Padding = New System.Windows.Forms.Padding(10)
'        Me.pnlSuperior.Size = New System.Drawing.Size(1509, 423)
'        Me.pnlSuperior.TabIndex = 2
'        '
'        'lstColumnas
'        '
'        Me.lstColumnas.ItemHeight = 23
'        Me.lstColumnas.Location = New System.Drawing.Point(13, 13)
'        Me.lstColumnas.Name = "lstColumnas"
'        Me.lstColumnas.Size = New System.Drawing.Size(195, 349)
'        Me.lstColumnas.TabIndex = 1
'        '
'        'lstValores
'        '
'        Me.lstValores.ItemHeight = 23
'        Me.lstValores.Location = New System.Drawing.Point(214, 13)
'        Me.lstValores.Name = "lstValores"
'        Me.lstValores.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
'        Me.lstValores.Size = New System.Drawing.Size(236, 349)
'        Me.lstValores.TabIndex = 5
'        '
'        'txtBusquedaGlobal
'        '
'        Me.txtBusquedaGlobal.Location = New System.Drawing.Point(456, 13)
'        Me.txtBusquedaGlobal.Name = "txtBusquedaGlobal"
'        Me.txtBusquedaGlobal.Size = New System.Drawing.Size(174, 30)
'        Me.txtBusquedaGlobal.TabIndex = 8
'        '
'        'btnAgregar
'        '
'        Me.btnAgregar.Location = New System.Drawing.Point(636, 13)
'        Me.btnAgregar.Name = "btnAgregar"
'        Me.btnAgregar.Size = New System.Drawing.Size(75, 30)
'        Me.btnAgregar.TabIndex = 7
'        Me.btnAgregar.Text = "Filtrar"
'        '
'        'btnDeshacer
'        '
'        Me.btnDeshacer.Location = New System.Drawing.Point(717, 13)
'        Me.btnDeshacer.Name = "btnDeshacer"
'        Me.btnDeshacer.Size = New System.Drawing.Size(103, 30)
'        Me.btnDeshacer.TabIndex = 11
'        Me.btnDeshacer.Text = "Deshacer"
'        '
'        'btnLimpiar
'        '
'        Me.btnLimpiar.Location = New System.Drawing.Point(826, 13)
'        Me.btnLimpiar.Name = "btnLimpiar"
'        Me.btnLimpiar.Size = New System.Drawing.Size(75, 30)
'        Me.btnLimpiar.TabIndex = 10
'        Me.btnLimpiar.Text = "Limpiar"
'        '
'        'cmbOrigenDatos
'        '
'        Me.cmbOrigenDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
'        Me.cmbOrigenDatos.FormattingEnabled = True
'        Me.cmbOrigenDatos.Location = New System.Drawing.Point(907, 13)
'        Me.cmbOrigenDatos.Name = "cmbOrigenDatos"
'        Me.cmbOrigenDatos.Size = New System.Drawing.Size(175, 31)
'        Me.cmbOrigenDatos.TabIndex = 13
'        '
'        'dtpFechaInicio
'        '
'        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
'        Me.dtpFechaInicio.Location = New System.Drawing.Point(1088, 13)
'        Me.dtpFechaInicio.Name = "dtpFechaInicio"
'        Me.dtpFechaInicio.Size = New System.Drawing.Size(136, 30)
'        Me.dtpFechaInicio.TabIndex = 14
'        '
'        'dtpFechaFin
'        '
'        Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
'        Me.dtpFechaFin.Location = New System.Drawing.Point(1230, 13)
'        Me.dtpFechaFin.Name = "dtpFechaFin"
'        Me.dtpFechaFin.Size = New System.Drawing.Size(134, 30)
'        Me.dtpFechaFin.TabIndex = 15
'        '
'        'btnCargar
'        '
'        Me.btnCargar.Location = New System.Drawing.Point(1370, 13)
'        Me.btnCargar.Name = "btnCargar"
'        Me.btnCargar.Size = New System.Drawing.Size(100, 30)
'        Me.btnCargar.TabIndex = 16
'        Me.btnCargar.Text = "Cargar"
'        '
'        'btnExportarExcel
'        '
'        Me.btnExportarExcel.Location = New System.Drawing.Point(13, 368)
'        Me.btnExportarExcel.Name = "btnExportarExcel"
'        Me.btnExportarExcel.Size = New System.Drawing.Size(151, 30)
'        Me.btnExportarExcel.TabIndex = 12
'        Me.btnExportarExcel.Text = "Exportar Excel"
'        '
'        'btnCopiarCorreos
'        '
'        Me.btnCopiarCorreos.Location = New System.Drawing.Point(170, 368)
'        Me.btnCopiarCorreos.Name = "btnCopiarCorreos"
'        Me.btnCopiarCorreos.Size = New System.Drawing.Size(154, 30)
'        Me.btnCopiarCorreos.TabIndex = 19
'        Me.btnCopiarCorreos.Text = "Copiar correos"
'        '
'        'btnExportarFichasPDF
'        '
'        Me.btnExportarFichasPDF.Location = New System.Drawing.Point(330, 368)
'        Me.btnExportarFichasPDF.Name = "btnExportarFichasPDF"
'        Me.btnExportarFichasPDF.Size = New System.Drawing.Size(151, 30)
'        Me.btnExportarFichasPDF.TabIndex = 17
'        Me.btnExportarFichasPDF.Text = "Exportar Fichas"
'        '
'        'lblConteoRegistros
'        '
'        Me.lblConteoRegistros.AutoSize = True
'        Me.lblConteoRegistros.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
'        Me.lblConteoRegistros.Font = New System.Drawing.Font("Segoe UI", 20.0!)
'        Me.lblConteoRegistros.ForeColor = System.Drawing.Color.Red
'        Me.lblConteoRegistros.Location = New System.Drawing.Point(487, 365)
'        Me.lblConteoRegistros.Name = "lblConteoRegistros"
'        Me.lblConteoRegistros.Size = New System.Drawing.Size(2, 48)
'        Me.lblConteoRegistros.TabIndex = 18
'        '
'        'flpChips
'        '
'        Me.flpChips.AutoSize = True
'        Me.flpChips.Dock = System.Windows.Forms.DockStyle.Top
'        Me.flpChips.Location = New System.Drawing.Point(0, 423)
'        Me.flpChips.Name = "flpChips"
'        Me.flpChips.Padding = New System.Windows.Forms.Padding(10)
'        Me.flpChips.Size = New System.Drawing.Size(1509, 20)
'        Me.flpChips.TabIndex = 1
'        '
'        'dgvDatos
'        '
'        Me.dgvDatos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
'        Me.dgvDatos.ColumnHeadersHeight = 29
'        Me.dgvDatos.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.dgvDatos.Location = New System.Drawing.Point(0, 443)
'        Me.dgvDatos.Name = "dgvDatos"
'        Me.dgvDatos.RowHeadersWidth = 51
'        Me.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
'        Me.dgvDatos.Size = New System.Drawing.Size(1509, 157)
'        Me.dgvDatos.TabIndex = 0
'        '
'        'frmFiltroAvanzado
'        '
'        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 23.0!)
'        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
'        Me.ClientSize = New System.Drawing.Size(1509, 600)
'        Me.Controls.Add(Me.dgvDatos)
'        Me.Controls.Add(Me.flpChips)
'        Me.Controls.Add(Me.pnlSuperior)
'        Me.Font = New System.Drawing.Font("Segoe UI", 10.2!)
'        Me.Name = "frmFiltroAvanzado"
'        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
'        Me.Text = "Filtro avanzado"
'        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
'        Me.pnlSuperior.ResumeLayout(False)
'        Me.pnlSuperior.PerformLayout()
'        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).EndInit()
'        Me.ResumeLayout(False)
'        Me.PerformLayout()

'    End Sub

'    Friend WithEvents btnDeshacer As Button
'    Friend WithEvents btnExportarExcel As Button
'    Friend WithEvents cmbOrigenDatos As ComboBox
'    Friend WithEvents dtpFechaInicio As DateTimePicker
'    Friend WithEvents dtpFechaFin As DateTimePicker
'    Friend WithEvents btnCargar As Button
'    Friend WithEvents btnExportarFichasPDF As Button
'    Friend WithEvents lblConteoRegistros As Label
'    Friend WithEvents btnCopiarCorreos As Button
'End Class