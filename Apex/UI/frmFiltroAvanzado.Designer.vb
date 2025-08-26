' Apex/UI/frmFiltroAvanzado.Designer.vb
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFiltroAvanzado
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
        Me.splitContenedorPrincipal = New System.Windows.Forms.SplitContainer()
        Me.pnlIzquierdo = New System.Windows.Forms.Panel()
        Me.gbxFiltros = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlFiltroBotones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnFiltrar = New System.Windows.Forms.Button()
        Me.btnLimpiar = New System.Windows.Forms.Button()
        Me.lstColumnas = New System.Windows.Forms.ListBox()
        Me.lstValores = New System.Windows.Forms.ListBox()
        Me.gbxOrigenDatos = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.cmbOrigenDatos = New System.Windows.Forms.ComboBox()
        Me.dtpFechaInicio = New System.Windows.Forms.DateTimePicker()
        Me.dtpFechaFin = New System.Windows.Forms.DateTimePicker()
        Me.btnCargar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnlDerecho = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.gbxBusquedaGlobal = New System.Windows.Forms.GroupBox()
        Me.txtBusquedaGlobal = New System.Windows.Forms.TextBox()
        Me.pnlAcciones = New System.Windows.Forms.Panel()
        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnExportarExcel = New System.Windows.Forms.Button()
        Me.btnCopiarCorreos = New System.Windows.Forms.Button()
        Me.btnExportarFichasPDF = New System.Windows.Forms.Button()
        Me.lblConteoRegistros = New System.Windows.Forms.Label()
        Me.Separator1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dgvDatos = New System.Windows.Forms.DataGridView()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.flpChips = New System.Windows.Forms.FlowLayoutPanel()
        CType(Me.splitContenedorPrincipal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedorPrincipal.Panel1.SuspendLayout()
        Me.splitContenedorPrincipal.Panel2.SuspendLayout()
        Me.splitContenedorPrincipal.SuspendLayout()
        Me.pnlIzquierdo.SuspendLayout()
        Me.gbxFiltros.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.pnlFiltroBotones.SuspendLayout()
        Me.gbxOrigenDatos.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.pnlDerecho.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.gbxBusquedaGlobal.SuspendLayout()
        Me.pnlAcciones.SuspendLayout()
        Me.flpAcciones.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
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
        Me.splitContenedorPrincipal.Panel1.Controls.Add(Me.pnlIzquierdo)
        Me.splitContenedorPrincipal.Panel1MinSize = 200
        '
        'splitContenedorPrincipal.Panel2
        '
        Me.splitContenedorPrincipal.Panel2.Controls.Add(Me.pnlDerecho)
        Me.splitContenedorPrincipal.Panel2MinSize = 200
        Me.splitContenedorPrincipal.Size = New System.Drawing.Size(1924, 937)
        Me.splitContenedorPrincipal.SplitterDistance = 596
        Me.splitContenedorPrincipal.TabIndex = 0
        '
        'pnlIzquierdo
        '
        Me.pnlIzquierdo.Controls.Add(Me.gbxFiltros)
        Me.pnlIzquierdo.Controls.Add(Me.gbxOrigenDatos)
        Me.pnlIzquierdo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlIzquierdo.Location = New System.Drawing.Point(0, 0)
        Me.pnlIzquierdo.Name = "pnlIzquierdo"
        Me.pnlIzquierdo.Padding = New System.Windows.Forms.Padding(12)
        Me.pnlIzquierdo.Size = New System.Drawing.Size(596, 937)
        Me.pnlIzquierdo.TabIndex = 0
        '
        'gbxFiltros
        '
        Me.gbxFiltros.Controls.Add(Me.TableLayoutPanel1)
        Me.gbxFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxFiltros.Location = New System.Drawing.Point(12, 210)
        Me.gbxFiltros.Name = "gbxFiltros"
        Me.gbxFiltros.Padding = New System.Windows.Forms.Padding(12)
        Me.gbxFiltros.Size = New System.Drawing.Size(572, 715)
        Me.gbxFiltros.TabIndex = 1
        Me.gbxFiltros.TabStop = False
        Me.gbxFiltros.Text = "2. Construir Filtro"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.pnlFiltroBotones, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lstColumnas, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lstValores, 1, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 31)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(548, 672)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'pnlFiltroBotones
        '
        Me.pnlFiltroBotones.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pnlFiltroBotones.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.pnlFiltroBotones, 2)
        Me.pnlFiltroBotones.Controls.Add(Me.btnFiltrar)
        Me.pnlFiltroBotones.Controls.Add(Me.btnLimpiar)
        Me.pnlFiltroBotones.Location = New System.Drawing.Point(169, 3)
        Me.pnlFiltroBotones.Name = "pnlFiltroBotones"
        Me.pnlFiltroBotones.Size = New System.Drawing.Size(210, 37)
        Me.pnlFiltroBotones.TabIndex = 2
        '
        'btnFiltrar
        '
        Me.btnFiltrar.Location = New System.Drawing.Point(3, 3)
        Me.btnFiltrar.Name = "btnFiltrar"
        Me.btnFiltrar.Size = New System.Drawing.Size(99, 31)
        Me.btnFiltrar.TabIndex = 0
        Me.btnFiltrar.Text = "Filtrar"
        Me.btnFiltrar.UseVisualStyleBackColor = True
        '
        'btnLimpiar
        '
        Me.btnLimpiar.Location = New System.Drawing.Point(108, 3)
        Me.btnLimpiar.Name = "btnLimpiar"
        Me.btnLimpiar.Size = New System.Drawing.Size(99, 31)
        Me.btnLimpiar.TabIndex = 1
        Me.btnLimpiar.Text = "Limpiar"
        Me.btnLimpiar.UseVisualStyleBackColor = True
        '
        'lstColumnas
        '
        Me.lstColumnas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstColumnas.FormattingEnabled = True
        Me.lstColumnas.ItemHeight = 20
        Me.lstColumnas.Location = New System.Drawing.Point(3, 46)
        Me.lstColumnas.Name = "lstColumnas"
        Me.lstColumnas.Size = New System.Drawing.Size(268, 623)
        Me.lstColumnas.TabIndex = 0
        '
        'lstValores
        '
        Me.lstValores.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstValores.FormattingEnabled = True
        Me.lstValores.ItemHeight = 20
        Me.lstValores.Location = New System.Drawing.Point(277, 46)
        Me.lstValores.Name = "lstValores"
        Me.lstValores.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstValores.Size = New System.Drawing.Size(268, 623)
        Me.lstValores.TabIndex = 1
        '
        'gbxOrigenDatos
        '
        Me.gbxOrigenDatos.Controls.Add(Me.TableLayoutPanel2)
        Me.gbxOrigenDatos.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbxOrigenDatos.Location = New System.Drawing.Point(12, 12)
        Me.gbxOrigenDatos.Name = "gbxOrigenDatos"
        Me.gbxOrigenDatos.Padding = New System.Windows.Forms.Padding(12)
        Me.gbxOrigenDatos.Size = New System.Drawing.Size(572, 198)
        Me.gbxOrigenDatos.TabIndex = 0
        Me.gbxOrigenDatos.TabStop = False
        Me.gbxOrigenDatos.Text = "1. Origen de Datos"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.cmbOrigenDatos, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.dtpFechaInicio, 1, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.dtpFechaFin, 1, 2)
        Me.TableLayoutPanel2.Controls.Add(Me.btnCargar, 1, 3)
        Me.TableLayoutPanel2.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(12, 31)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 4
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(548, 155)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'cmbOrigenDatos
        '
        Me.cmbOrigenDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbOrigenDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrigenDatos.FormattingEnabled = True
        Me.cmbOrigenDatos.Location = New System.Drawing.Point(69, 3)
        Me.cmbOrigenDatos.Name = "cmbOrigenDatos"
        Me.cmbOrigenDatos.Size = New System.Drawing.Size(476, 28)
        Me.cmbOrigenDatos.TabIndex = 0
        '
        'dtpFechaInicio
        '
        Me.dtpFechaInicio.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaInicio.Location = New System.Drawing.Point(69, 37)
        Me.dtpFechaInicio.Name = "dtpFechaInicio"
        Me.dtpFechaInicio.Size = New System.Drawing.Size(132, 26)
        Me.dtpFechaInicio.TabIndex = 1
        '
        'dtpFechaFin
        '
        Me.dtpFechaFin.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaFin.Location = New System.Drawing.Point(69, 69)
        Me.dtpFechaFin.Name = "dtpFechaFin"
        Me.dtpFechaFin.Size = New System.Drawing.Size(132, 26)
        Me.dtpFechaFin.TabIndex = 2
        '
        'btnCargar
        '
        Me.btnCargar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCargar.Location = New System.Drawing.Point(69, 101)
        Me.btnCargar.Name = "btnCargar"
        Me.btnCargar.Size = New System.Drawing.Size(476, 35)
        Me.btnCargar.TabIndex = 3
        Me.btnCargar.Text = "Cargar"
        Me.btnCargar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Origen:"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 20)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Desde:"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 20)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Hasta:"
        '
        'pnlDerecho
        '
        Me.pnlDerecho.Controls.Add(Me.TableLayoutPanel3)
        Me.pnlDerecho.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDerecho.Location = New System.Drawing.Point(0, 0)
        Me.pnlDerecho.Name = "pnlDerecho"
        Me.pnlDerecho.Padding = New System.Windows.Forms.Padding(12)
        Me.pnlDerecho.Size = New System.Drawing.Size(1324, 937)
        Me.pnlDerecho.TabIndex = 0
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 1
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.gbxBusquedaGlobal, 0, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.pnlAcciones, 0, 1)
        Me.TableLayoutPanel3.Controls.Add(Me.Panel1, 0, 2)
        Me.TableLayoutPanel3.Controls.Add(Me.Panel2, 0, 3)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 4
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(1300, 913)
        Me.TableLayoutPanel3.TabIndex = 8
        '
        'gbxBusquedaGlobal
        '
        Me.gbxBusquedaGlobal.Controls.Add(Me.txtBusquedaGlobal)
        Me.gbxBusquedaGlobal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxBusquedaGlobal.Location = New System.Drawing.Point(3, 3)
        Me.gbxBusquedaGlobal.Name = "gbxBusquedaGlobal"
        Me.gbxBusquedaGlobal.Padding = New System.Windows.Forms.Padding(12)
        Me.gbxBusquedaGlobal.Size = New System.Drawing.Size(1294, 79)
        Me.gbxBusquedaGlobal.TabIndex = 0
        Me.gbxBusquedaGlobal.TabStop = False
        Me.gbxBusquedaGlobal.Text = "Búsqueda Rápida en todos los campos de texto"
        '
        'txtBusquedaGlobal
        '
        Me.txtBusquedaGlobal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBusquedaGlobal.Location = New System.Drawing.Point(12, 31)
        Me.txtBusquedaGlobal.Name = "txtBusquedaGlobal"
        Me.txtBusquedaGlobal.Size = New System.Drawing.Size(1270, 26)
        Me.txtBusquedaGlobal.TabIndex = 0
        '
        'pnlAcciones
        '
        Me.pnlAcciones.AutoSize = True
        Me.pnlAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlAcciones.Controls.Add(Me.flpAcciones)
        Me.pnlAcciones.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlAcciones.Location = New System.Drawing.Point(3, 88)
        Me.pnlAcciones.Name = "pnlAcciones"
        Me.pnlAcciones.Size = New System.Drawing.Size(1294, 53)
        Me.pnlAcciones.TabIndex = 7
        '
        'flpAcciones
        '
        Me.flpAcciones.AutoSize = True
        Me.flpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpAcciones.Controls.Add(Me.btnExportarExcel)
        Me.flpAcciones.Controls.Add(Me.btnCopiarCorreos)
        Me.flpAcciones.Controls.Add(Me.btnExportarFichasPDF)
        Me.flpAcciones.Controls.Add(Me.lblConteoRegistros)
        Me.flpAcciones.Controls.Add(Me.Separator1)
        Me.flpAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpAcciones.Location = New System.Drawing.Point(0, 0)
        Me.flpAcciones.Name = "flpAcciones"
        Me.flpAcciones.Padding = New System.Windows.Forms.Padding(5)
        Me.flpAcciones.Size = New System.Drawing.Size(1294, 53)
        Me.flpAcciones.TabIndex = 0
        '
        'btnExportarExcel
        '
        Me.btnExportarExcel.Location = New System.Drawing.Point(8, 8)
        Me.btnExportarExcel.Name = "btnExportarExcel"
        Me.btnExportarExcel.Size = New System.Drawing.Size(136, 37)
        Me.btnExportarExcel.TabIndex = 12
        Me.btnExportarExcel.Text = "Exportar Excel"
        Me.btnExportarExcel.UseVisualStyleBackColor = True
        '
        'btnCopiarCorreos
        '
        Me.btnCopiarCorreos.Location = New System.Drawing.Point(150, 8)
        Me.btnCopiarCorreos.Name = "btnCopiarCorreos"
        Me.btnCopiarCorreos.Size = New System.Drawing.Size(136, 37)
        Me.btnCopiarCorreos.TabIndex = 13
        Me.btnCopiarCorreos.Text = "Copiar Correos"
        Me.btnCopiarCorreos.UseVisualStyleBackColor = True
        '
        'btnExportarFichasPDF
        '
        Me.btnExportarFichasPDF.Location = New System.Drawing.Point(292, 8)
        Me.btnExportarFichasPDF.Name = "btnExportarFichasPDF"
        Me.btnExportarFichasPDF.Size = New System.Drawing.Size(136, 37)
        Me.btnExportarFichasPDF.TabIndex = 14
        Me.btnExportarFichasPDF.Text = "Exportar Fichas"
        Me.btnExportarFichasPDF.UseVisualStyleBackColor = True
        '
        'lblConteoRegistros
        '
        Me.lblConteoRegistros.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblConteoRegistros.AutoSize = True
        Me.lblConteoRegistros.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConteoRegistros.Location = New System.Drawing.Point(441, 16)
        Me.lblConteoRegistros.Margin = New System.Windows.Forms.Padding(10, 0, 3, 0)
        Me.lblConteoRegistros.Name = "lblConteoRegistros"
        Me.lblConteoRegistros.Size = New System.Drawing.Size(106, 20)
        Me.lblConteoRegistros.TabIndex = 23
        Me.lblConteoRegistros.Text = "Registros: 0"
        '
        'Separator1
        '
        Me.Separator1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Separator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Separator1.Location = New System.Drawing.Point(553, 8)
        Me.Separator1.Name = "Separator1"
        Me.Separator1.Size = New System.Drawing.Size(3, 37)
        Me.Separator1.TabIndex = 15
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.dgvDatos)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 147)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1294, 613)
        Me.Panel1.TabIndex = 5
        '
        'dgvDatos
        '
        Me.dgvDatos.AllowDrop = True
        Me.dgvDatos.AllowUserToAddRows = False
        Me.dgvDatos.AllowUserToDeleteRows = False
        Me.dgvDatos.AllowUserToResizeColumns = False
        Me.dgvDatos.AllowUserToResizeRows = False
        Me.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDatos.Location = New System.Drawing.Point(0, 0)
        Me.dgvDatos.Name = "dgvDatos"
        Me.dgvDatos.ReadOnly = True
        Me.dgvDatos.RowHeadersWidth = 51
        Me.dgvDatos.RowTemplate.Height = 24
        Me.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDatos.Size = New System.Drawing.Size(1294, 613)
        Me.dgvDatos.TabIndex = 4
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.flpChips)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(3, 766)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1294, 144)
        Me.Panel2.TabIndex = 6
        '
        'flpChips
        '
        Me.flpChips.AutoScroll = True
        Me.flpChips.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpChips.Location = New System.Drawing.Point(0, 0)
        Me.flpChips.Name = "flpChips"
        Me.flpChips.Padding = New System.Windows.Forms.Padding(6)
        Me.flpChips.Size = New System.Drawing.Size(1294, 144)
        Me.flpChips.TabIndex = 2
        '
        'frmFiltroAvanzado
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1924, 937)
        Me.Controls.Add(Me.splitContenedorPrincipal)
        Me.MinimumSize = New System.Drawing.Size(1280, 800)
        Me.Name = "frmFiltroAvanzado"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Filtro Avanzado"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.splitContenedorPrincipal.Panel1.ResumeLayout(False)
        Me.splitContenedorPrincipal.Panel2.ResumeLayout(False)
        CType(Me.splitContenedorPrincipal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedorPrincipal.ResumeLayout(False)
        Me.pnlIzquierdo.ResumeLayout(False)
        Me.gbxFiltros.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.pnlFiltroBotones.ResumeLayout(False)
        Me.gbxOrigenDatos.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.pnlDerecho.ResumeLayout(False)
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel3.PerformLayout()
        Me.gbxBusquedaGlobal.ResumeLayout(False)
        Me.gbxBusquedaGlobal.PerformLayout()
        Me.pnlAcciones.ResumeLayout(False)
        Me.pnlAcciones.PerformLayout()
        Me.flpAcciones.ResumeLayout(False)
        Me.flpAcciones.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents splitContenedorPrincipal As SplitContainer
    Friend WithEvents pnlIzquierdo As Panel
    Friend WithEvents gbxFiltros As GroupBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents pnlFiltroBotones As FlowLayoutPanel
    Friend WithEvents btnFiltrar As Button
    Friend WithEvents btnLimpiar As Button
    Friend WithEvents lstColumnas As ListBox
    Friend WithEvents lstValores As ListBox
    Friend WithEvents gbxOrigenDatos As GroupBox
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents cmbOrigenDatos As ComboBox
    Friend WithEvents dtpFechaInicio As DateTimePicker
    Friend WithEvents dtpFechaFin As DateTimePicker
    Friend WithEvents btnCargar As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents pnlDerecho As Panel
    Friend WithEvents gbxBusquedaGlobal As GroupBox
    Friend WithEvents txtBusquedaGlobal As TextBox
    Friend WithEvents flpChips As FlowLayoutPanel
    Friend WithEvents dgvDatos As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents pnlAcciones As Panel
    Friend WithEvents btnExportarExcel As Button
    Friend WithEvents btnCopiarCorreos As Button
    Friend WithEvents btnExportarFichasPDF As Button
    Friend WithEvents Separator1 As Label
    Friend WithEvents lblConteoRegistros As Label
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents flpAcciones As FlowLayoutPanel
End Class