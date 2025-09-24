<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFiltrar
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
        Me.components = New System.ComponentModel.Container()
        Me.splitContenedorPrincipal = New System.Windows.Forms.SplitContainer()
        Me.TableLayoutPanelLeft = New System.Windows.Forms.TableLayoutPanel()
        Me.gbxOrigen = New System.Windows.Forms.GroupBox()
        Me.btnCargar = New System.Windows.Forms.Button()
        Me.dtpFechaFin = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpFechaInicio = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbOrigenDatos = New System.Windows.Forms.ComboBox()
        Me.lblOrigen = New System.Windows.Forms.Label()
        Me.gbxFiltros = New System.Windows.Forms.GroupBox()
        Me.pnlFiltroBotones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnFiltrar = New System.Windows.Forms.Button()
        Me.btnLimpiar = New System.Windows.Forms.Button()
        Me.lstValores = New System.Windows.Forms.ListBox()
        Me.lblValores = New System.Windows.Forms.Label()
        Me.lstColumnas = New System.Windows.Forms.ListBox()
        Me.lblColumna = New System.Windows.Forms.Label()
        Me.PanelChips = New System.Windows.Forms.Panel()
        Me.flpChips = New System.Windows.Forms.FlowLayoutPanel()
        Me.TableLayoutPanelRight = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlContenedorSuperior = New System.Windows.Forms.Panel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnExportarFichasPDF = New System.Windows.Forms.Button()
        Me.btnCopiarCorreos = New System.Windows.Forms.Button()
        Me.btnExportarExcel = New System.Windows.Forms.Button()
        Me.Separator1 = New System.Windows.Forms.Label()
        Me.gbxBusquedaGlobal = New System.Windows.Forms.GroupBox()
        Me.txtBusquedaGlobal = New System.Windows.Forms.TextBox()
        Me.pnlContenedorGrilla = New System.Windows.Forms.Panel()
        Me.dgvDatos = New System.Windows.Forms.DataGridView()
        Me.pnlBarraEstado = New System.Windows.Forms.Panel()
        Me.lblConteoRegistros = New System.Windows.Forms.Label()
        CType(Me.splitContenedorPrincipal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedorPrincipal.Panel1.SuspendLayout()
        Me.splitContenedorPrincipal.Panel2.SuspendLayout()
        Me.splitContenedorPrincipal.SuspendLayout()
        Me.TableLayoutPanelLeft.SuspendLayout()
        Me.gbxOrigen.SuspendLayout()
        Me.gbxFiltros.SuspendLayout()
        Me.pnlFiltroBotones.SuspendLayout()
        Me.PanelChips.SuspendLayout()
        Me.TableLayoutPanelRight.SuspendLayout()
        Me.pnlContenedorSuperior.SuspendLayout()
        Me.flpAcciones.SuspendLayout()
        Me.gbxBusquedaGlobal.SuspendLayout()
        Me.pnlContenedorGrilla.SuspendLayout()
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlBarraEstado.SuspendLayout()
        Me.SuspendLayout()
        '
        'splitContenedorPrincipal
        '
        Me.splitContenedorPrincipal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContenedorPrincipal.Location = New System.Drawing.Point(0, 0)
        Me.splitContenedorPrincipal.Name = "splitContenedorPrincipal"
        '
        'splitContenedorPrincipal.Panel1
        '
        Me.splitContenedorPrincipal.Panel1.Controls.Add(Me.TableLayoutPanelLeft)
        Me.splitContenedorPrincipal.Panel1MinSize = 320
        '
        'splitContenedorPrincipal.Panel2
        '
        Me.splitContenedorPrincipal.Panel2.Controls.Add(Me.TableLayoutPanelRight)
        Me.splitContenedorPrincipal.Panel2MinSize = 500
        Me.splitContenedorPrincipal.Size = New System.Drawing.Size(1184, 761)
        Me.splitContenedorPrincipal.SplitterDistance = 350
        Me.splitContenedorPrincipal.TabIndex = 0
        '
        'TableLayoutPanelLeft
        '
        Me.TableLayoutPanelLeft.ColumnCount = 1
        Me.TableLayoutPanelLeft.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLeft.Controls.Add(Me.gbxOrigen, 0, 0)
        Me.TableLayoutPanelLeft.Controls.Add(Me.gbxFiltros, 0, 2)
        Me.TableLayoutPanelLeft.Controls.Add(Me.PanelChips, 0, 1)
        Me.TableLayoutPanelLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelLeft.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelLeft.Name = "TableLayoutPanelLeft"
        Me.TableLayoutPanelLeft.RowCount = 3
        Me.TableLayoutPanelLeft.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.TableLayoutPanelLeft.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanelLeft.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelLeft.Size = New System.Drawing.Size(350, 761)
        Me.TableLayoutPanelLeft.TabIndex = 0
        '
        'gbxOrigen
        '
        Me.gbxOrigen.Controls.Add(Me.btnCargar)
        Me.gbxOrigen.Controls.Add(Me.dtpFechaFin)
        Me.gbxOrigen.Controls.Add(Me.Label2)
        Me.gbxOrigen.Controls.Add(Me.dtpFechaInicio)
        Me.gbxOrigen.Controls.Add(Me.Label1)
        Me.gbxOrigen.Controls.Add(Me.cmbOrigenDatos)
        Me.gbxOrigen.Controls.Add(Me.lblOrigen)
        Me.gbxOrigen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxOrigen.Location = New System.Drawing.Point(3, 3)
        Me.gbxOrigen.Name = "gbxOrigen"
        Me.gbxOrigen.Size = New System.Drawing.Size(344, 144)
        Me.gbxOrigen.TabIndex = 0
        Me.gbxOrigen.TabStop = False
        Me.gbxOrigen.Text = "Origen de Datos y Fechas"
        '
        'btnCargar
        '
        Me.btnCargar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCargar.Location = New System.Drawing.Point(234, 105)
        Me.btnCargar.Name = "btnCargar"
        Me.btnCargar.Size = New System.Drawing.Size(100, 28)
        Me.btnCargar.TabIndex = 6
        Me.btnCargar.Text = "Cargar Datos"
        Me.btnCargar.UseVisualStyleBackColor = True
        '
        'dtpFechaFin
        '
        Me.dtpFechaFin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaFin.Location = New System.Drawing.Point(54, 79)
        Me.dtpFechaFin.Name = "dtpFechaFin"
        Me.dtpFechaFin.Size = New System.Drawing.Size(280, 20)
        Me.dtpFechaFin.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(38, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Hasta:"
        '
        'dtpFechaInicio
        '
        Me.dtpFechaInicio.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaInicio.Location = New System.Drawing.Point(54, 53)
        Me.dtpFechaInicio.Name = "dtpFechaInicio"
        Me.dtpFechaInicio.Size = New System.Drawing.Size(280, 20)
        Me.dtpFechaInicio.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Desde:"
        '
        'cmbOrigenDatos
        '
        Me.cmbOrigenDatos.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbOrigenDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrigenDatos.FormattingEnabled = True
        Me.cmbOrigenDatos.Location = New System.Drawing.Point(54, 26)
        Me.cmbOrigenDatos.Name = "cmbOrigenDatos"
        Me.cmbOrigenDatos.Size = New System.Drawing.Size(280, 21)
        Me.cmbOrigenDatos.TabIndex = 1
        '
        'lblOrigen
        '
        Me.lblOrigen.AutoSize = True
        Me.lblOrigen.Location = New System.Drawing.Point(6, 29)
        Me.lblOrigen.Name = "lblOrigen"
        Me.lblOrigen.Size = New System.Drawing.Size(41, 13)
        Me.lblOrigen.TabIndex = 0
        Me.lblOrigen.Text = "Origen:"
        '
        'gbxFiltros
        '
        Me.gbxFiltros.Controls.Add(Me.pnlFiltroBotones)
        Me.gbxFiltros.Controls.Add(Me.lstValores)
        Me.gbxFiltros.Controls.Add(Me.lblValores)
        Me.gbxFiltros.Controls.Add(Me.lstColumnas)
        Me.gbxFiltros.Controls.Add(Me.lblColumna)
        Me.gbxFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxFiltros.Location = New System.Drawing.Point(3, 193)
        Me.gbxFiltros.Name = "gbxFiltros"
        Me.gbxFiltros.Size = New System.Drawing.Size(344, 565)
        Me.gbxFiltros.TabIndex = 1
        Me.gbxFiltros.TabStop = False
        Me.gbxFiltros.Text = "Filtros por Columna"
        '
        'pnlFiltroBotones
        '
        Me.pnlFiltroBotones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlFiltroBotones.Controls.Add(Me.btnFiltrar)
        Me.pnlFiltroBotones.Controls.Add(Me.btnLimpiar)
        Me.pnlFiltroBotones.Location = New System.Drawing.Point(9, 526)
        Me.pnlFiltroBotones.Name = "pnlFiltroBotones"
        Me.pnlFiltroBotones.Size = New System.Drawing.Size(325, 33)
        Me.pnlFiltroBotones.TabIndex = 4
        '
        'btnFiltrar
        '
        Me.btnFiltrar.Location = New System.Drawing.Point(232, 3)
        Me.btnFiltrar.Margin = New System.Windows.Forms.Padding(3, 3, 10, 3)
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(83, 28)
        Me.btnFiltrar.TabIndex = 0
        Me.btnFiltrar.Text = "Filtrar"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'btnLimpiar
        '
        Me.btnLimpiar.Location = New System.Drawing.Point(151, 3)
        Me.btnLimpiar.Name = "btnLimpiar"
        Me.btnLimpiar.Size = New System.Drawing.Size(75, 28)
        Me.btnLimpiar.TabIndex = 1
        Me.btnLimpiar.Text = "Limpiar"
        Me.btnLimpiar.UseVisualStyleBackColor = True
        '
        'lstValores
        '
        Me.lstValores.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstValores.FormattingEnabled = True
        Me.lstValores.HorizontalScrollbar = True
        Me.lstValores.Location = New System.Drawing.Point(161, 41)
        Me.lstValores.Name = "lstValores"
        Me.lstValores.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstValores.Size = New System.Drawing.Size(173, 472)
        Me.lstValores.TabIndex = 3
        '
        'lblValores
        '
        Me.lblValores.AutoSize = True
        Me.lblValores.Location = New System.Drawing.Point(158, 25)
        Me.lblValores.Name = "lblValores"
        Me.lblValores.Size = New System.Drawing.Size(45, 13)
        Me.lblValores.TabIndex = 2
        Me.lblValores.Text = "Valores:"
        '
        'lstColumnas
        '
        Me.lstColumnas.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstColumnas.FormattingEnabled = True
        Me.lstColumnas.IntegralHeight = False
        Me.lstColumnas.Location = New System.Drawing.Point(9, 41)
        Me.lstColumnas.Name = "lstColumnas"
        Me.lstColumnas.Size = New System.Drawing.Size(146, 472)
        Me.lstColumnas.TabIndex = 1
        '
        'lblColumna
        '
        Me.lblColumna.AutoSize = True
        Me.lblColumna.Location = New System.Drawing.Point(6, 25)
        Me.lblColumna.Name = "lblColumna"
        Me.lblColumna.Size = New System.Drawing.Size(51, 13)
        Me.lblColumna.TabIndex = 0
        Me.lblColumna.Text = "Columna:"
        '
        'PanelChips
        '
        Me.PanelChips.Controls.Add(Me.flpChips)
        Me.PanelChips.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelChips.Location = New System.Drawing.Point(3, 153)
        Me.PanelChips.Name = "PanelChips"
        Me.PanelChips.Size = New System.Drawing.Size(344, 34)
        Me.PanelChips.TabIndex = 2
        '
        'flpChips
        '
        Me.flpChips.AutoScroll = True
        Me.flpChips.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpChips.Location = New System.Drawing.Point(0, 0)
        Me.flpChips.Name = "flpChips"
        Me.flpChips.Padding = New System.Windows.Forms.Padding(3)
        Me.flpChips.Size = New System.Drawing.Size(344, 34)
        Me.flpChips.TabIndex = 0
        Me.flpChips.Visible = False
        '
        'TableLayoutPanelRight
        '
        Me.TableLayoutPanelRight.ColumnCount = 1
        Me.TableLayoutPanelRight.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelRight.Controls.Add(Me.pnlContenedorSuperior, 0, 0)
        Me.TableLayoutPanelRight.Controls.Add(Me.pnlContenedorGrilla, 0, 1)
        Me.TableLayoutPanelRight.Controls.Add(Me.pnlBarraEstado, 0, 2)
        Me.TableLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelRight.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelRight.Name = "TableLayoutPanelRight"
        Me.TableLayoutPanelRight.RowCount = 3
        Me.TableLayoutPanelRight.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65.0!))
        Me.TableLayoutPanelRight.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelRight.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelRight.Size = New System.Drawing.Size(830, 761)
        Me.TableLayoutPanelRight.TabIndex = 0
        '
        'pnlContenedorSuperior
        '
        Me.pnlContenedorSuperior.Controls.Add(Me.flpAcciones)
        Me.pnlContenedorSuperior.Controls.Add(Me.gbxBusquedaGlobal)
        Me.pnlContenedorSuperior.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContenedorSuperior.Location = New System.Drawing.Point(3, 3)
        Me.pnlContenedorSuperior.Name = "pnlContenedorSuperior"
        Me.pnlContenedorSuperior.Size = New System.Drawing.Size(824, 59)
        Me.pnlContenedorSuperior.TabIndex = 0
        '
        'flpAcciones
        '
        Me.flpAcciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpAcciones.Controls.Add(Me.btnExportarFichasPDF)
        Me.flpAcciones.Controls.Add(Me.btnCopiarCorreos)
        Me.flpAcciones.Controls.Add(Me.Separator1)
        Me.flpAcciones.Controls.Add(Me.btnExportarExcel)
        Me.flpAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flpAcciones.Location = New System.Drawing.Point(408, 11)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Size = New System.Drawing.Size(412, 42)
        Me.flpAcciones.TabIndex = 1
        '
        'btnExportarFichasPDF
        '
        Me.btnExportarFichasPDF.Location = New System.Drawing.Point(289, 3)
        Me.btnExportarFichasPDF.Name = "btnExportarFichasPDF"
        Me.btnExportarFichasPDF.Size = New System.Drawing.Size(120, 28)
        Me.btnExportarFichasPDF.TabIndex = 2
        Me.btnExportarFichasPDF.Text = "Exportar Fichas PDF"
        Me.btnExportarFichasPDF.UseVisualStyleBackColor = True
        '
        'btnCopiarCorreos
        '
        Me.btnCopiarCorreos.Location = New System.Drawing.Point(173, 3)
        Me.btnCopiarCorreos.Name = "btnCopiarCorreos"
        Me.btnCopiarCorreos.Size = New System.Drawing.Size(110, 28)
        Me.btnCopiarCorreos.TabIndex = 1
        Me.btnCopiarCorreos.Text = "Copiar Correos"
        Me.btnCopiarCorreos.UseVisualStyleBackColor = True
        '
        'btnExportarExcel
        '
        Me.btnExportarExcel.Location = New System.Drawing.Point(40, 3)
        Me.btnExportarExcel.Name = "btnExportarExcel"
        Me.btnExportarExcel.Size = New System.Drawing.Size(110, 28)
        Me.btnExportarExcel.TabIndex = 0
        Me.btnExportarExcel.Text = "Exportar a Excel"
        Me.btnExportarExcel.UseVisualStyleBackColor = True
        '
        'Separator1
        '
        Me.Separator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Separator1.Location = New System.Drawing.Point(156, 0)
        Me.Separator1.Name = "Separator1"
        Me.Separator1.Size = New System.Drawing.Size(2, 32)
        Me.Separator1.TabIndex = 3
        '
        'gbxBusquedaGlobal
        '
        Me.gbxBusquedaGlobal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbxBusquedaGlobal.Controls.Add(Me.txtBusquedaGlobal)
        Me.gbxBusquedaGlobal.Location = New System.Drawing.Point(3, 8)
        Me.gbxBusquedaGlobal.Name = "gbxBusquedaGlobal"
        Me.gbxBusquedaGlobal.Size = New System.Drawing.Size(399, 45)
        Me.gbxBusquedaGlobal.TabIndex = 0
        Me.gbxBusquedaGlobal.TabStop = False
        Me.gbxBusquedaGlobal.Text = "Búsqueda Global"
        '
        'txtBusquedaGlobal
        '
        Me.txtBusquedaGlobal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBusquedaGlobal.Location = New System.Drawing.Point(3, 16)
        Me.txtBusquedaGlobal.Name = "txtBusquedaGlobal"
        Me.txtBusquedaGlobal.Size = New System.Drawing.Size(393, 20)
        Me.txtBusquedaGlobal.TabIndex = 0
        '
        'pnlContenedorGrilla
        '
        Me.pnlContenedorGrilla.Controls.Add(Me.dgvDatos)
        Me.pnlContenedorGrilla.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContenedorGrilla.Location = New System.Drawing.Point(3, 68)
        Me.pnlContenedorGrilla.Name = "pnlContenedorGrilla"
        Me.pnlContenedorGrilla.Size = New System.Drawing.Size(824, 660)
        Me.pnlContenedorGrilla.TabIndex = 1
        '
        'dgvDatos
        '
        Me.dgvDatos.AllowUserToAddRows = False
        Me.dgvDatos.AllowUserToDeleteRows = False
        Me.dgvDatos.AllowUserToOrderColumns = True
        Me.dgvDatos.BackgroundColor = System.Drawing.SystemColors.Window
        Me.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDatos.Location = New System.Drawing.Point(0, 0)
        Me.dgvDatos.Name = "dgvDatos"
        Me.dgvDatos.ReadOnly = True
        Me.dgvDatos.Size = New System.Drawing.Size(824, 660)
        Me.dgvDatos.TabIndex = 0
        '
        'pnlBarraEstado
        '
        Me.pnlBarraEstado.Controls.Add(Me.lblConteoRegistros)
        Me.pnlBarraEstado.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlBarraEstado.Location = New System.Drawing.Point(3, 734)
        Me.pnlBarraEstado.Name = "pnlBarraEstado"
        Me.pnlBarraEstado.Size = New System.Drawing.Size(824, 24)
        Me.pnlBarraEstado.TabIndex = 2
        '
        'lblConteoRegistros
        '
        Me.lblConteoRegistros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblConteoRegistros.Location = New System.Drawing.Point(0, 0)
        Me.lblConteoRegistros.Name = "lblConteoRegistros"
        Me.lblConteoRegistros.Padding = New System.Windows.Forms.Padding(3, 0, 0, 0)
        Me.lblConteoRegistros.Size = New System.Drawing.Size(824, 24)
        Me.lblConteoRegistros.TabIndex = 0
        Me.lblConteoRegistros.Text = "Registros: 0 de 0"
        Me.lblConteoRegistros.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmFiltros
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1184, 761)
        Me.Controls.Add(Me.splitContenedorPrincipal)
        Me.MinimumSize = New System.Drawing.Size(900, 600)
        Me.Name = "frmFiltros"
        Me.Text = "Filtros Avanzados y Consultas"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.splitContenedorPrincipal.Panel1.ResumeLayout(False)
        Me.splitContenedorPrincipal.Panel2.ResumeLayout(False)
        CType(Me.splitContenedorPrincipal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedorPrincipal.ResumeLayout(False)
        Me.TableLayoutPanelLeft.ResumeLayout(False)
        Me.gbxOrigen.ResumeLayout(False)
        Me.gbxOrigen.PerformLayout()
        Me.gbxFiltros.ResumeLayout(False)
        Me.gbxFiltros.PerformLayout()
        Me.pnlFiltroBotones.ResumeLayout(False)
        Me.PanelChips.ResumeLayout(False)
        Me.TableLayoutPanelRight.ResumeLayout(False)
        Me.pnlContenedorSuperior.ResumeLayout(False)
        Me.flpAcciones.ResumeLayout(False)
        Me.gbxBusquedaGlobal.ResumeLayout(False)
        Me.gbxBusquedaGlobal.PerformLayout()
        Me.pnlContenedorGrilla.ResumeLayout(False)
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlBarraEstado.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents splitContenedorPrincipal As SplitContainer
    Friend WithEvents TableLayoutPanelLeft As TableLayoutPanel
    Friend WithEvents gbxOrigen As GroupBox
    Friend WithEvents btnCargar As Button
    Friend WithEvents dtpFechaFin As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents dtpFechaInicio As DateTimePicker
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbOrigenDatos As ComboBox
    Friend WithEvents lblOrigen As Label
    Friend WithEvents gbxFiltros As GroupBox
    Friend WithEvents pnlFiltroBotones As FlowLayoutPanel
    Friend WithEvents btnFiltrar As Button
    Friend WithEvents btnLimpiar As Button
    Friend WithEvents lstValores As ListBox
    Friend WithEvents lblValores As Label
    Friend WithEvents lstColumnas As ListBox
    Friend WithEvents lblColumna As Label
    Friend WithEvents TableLayoutPanelRight As TableLayoutPanel
    Friend WithEvents pnlContenedorSuperior As Panel
    Friend WithEvents gbxBusquedaGlobal As GroupBox
    Friend WithEvents txtBusquedaGlobal As TextBox
    Friend WithEvents pnlContenedorGrilla As Panel
    Friend WithEvents dgvDatos As DataGridView
    Friend WithEvents pnlBarraEstado As Panel
    Friend WithEvents lblConteoRegistros As Label
    Friend WithEvents PanelChips As Panel
    Friend WithEvents flpChips As FlowLayoutPanel
    Friend WithEvents flpAcciones As FlowLayoutPanel
    Friend WithEvents btnExportarFichasPDF As Button
    Friend WithEvents btnCopiarCorreos As Button
    Friend WithEvents btnExportarExcel As Button
    Friend WithEvents Separator1 As Label
End Class