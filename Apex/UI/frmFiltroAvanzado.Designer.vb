Option Strict On
Option Explicit On

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFiltroAvanzado
    Inherits System.Windows.Forms.Form

    ' --- Declaraciones de Controles ---
    Private components As System.ComponentModel.IContainer
    Private WithEvents splitContenedorPrincipal As SplitContainer
    Private WithEvents pnlIzquierdo As Panel
    Private WithEvents gbxFiltros As GroupBox
    Private WithEvents TableLayoutPanel1 As TableLayoutPanel
    Private WithEvents pnlFiltroBotones As FlowLayoutPanel
    Private WithEvents gbxOrigenDatos As GroupBox
    Private WithEvents TableLayoutPanel2 As TableLayoutPanel
    Private WithEvents Label1 As Label
    Private WithEvents Label2 As Label
    Private WithEvents Label3 As Label
    Private WithEvents pnlDerecho As Panel
    Private WithEvents gbxBusquedaGlobal As GroupBox
    Private WithEvents pnlAcciones As FlowLayoutPanel
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
    Friend WithEvents Separator1 As Label
    Friend WithEvents btnNuevaNotificacion As Button
    Friend WithEvents btnEditarNotificacion As Button
    Friend WithEvents btnEliminarNotificacion As Button
    Friend WithEvents btnCambiarEstado As Button
    Friend WithEvents btnNuevaLicencia As Button
    Friend WithEvents btnEditarLicencia As Button
    Friend WithEvents btnEliminarLicencia As Button
    Friend WithEvents btnSiguiente As Button
    Friend WithEvents lblPaginacion As Label
    Friend WithEvents btnAnterior As Button


    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.splitContenedorPrincipal = New System.Windows.Forms.SplitContainer()
        Me.pnlIzquierdo = New System.Windows.Forms.Panel()
        Me.gbxFiltros = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lstColumnas = New System.Windows.Forms.ListBox()
        Me.lstValores = New System.Windows.Forms.ListBox()
        Me.pnlFiltroBotones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnAgregar = New System.Windows.Forms.Button()
        Me.btnLimpiar = New System.Windows.Forms.Button()
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
        Me.dgvDatos = New System.Windows.Forms.DataGridView()
        Me.pnlAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnExportarExcel = New System.Windows.Forms.Button()
        Me.btnCopiarCorreos = New System.Windows.Forms.Button()
        Me.btnExportarFichasPDF = New System.Windows.Forms.Button()
        Me.lblConteoRegistros = New System.Windows.Forms.Label()
        Me.Separator1 = New System.Windows.Forms.Label()
        Me.btnNuevaNotificacion = New System.Windows.Forms.Button()
        Me.btnEditarNotificacion = New System.Windows.Forms.Button()
        Me.btnEliminarNotificacion = New System.Windows.Forms.Button()
        Me.btnCambiarEstado = New System.Windows.Forms.Button()
        Me.btnNuevaLicencia = New System.Windows.Forms.Button()
        Me.btnEditarLicencia = New System.Windows.Forms.Button()
        Me.btnEliminarLicencia = New System.Windows.Forms.Button()
        Me.btnSiguiente = New System.Windows.Forms.Button()
        Me.lblPaginacion = New System.Windows.Forms.Label()
        Me.btnAnterior = New System.Windows.Forms.Button()
        Me.flpChips = New System.Windows.Forms.FlowLayoutPanel()
        Me.gbxBusquedaGlobal = New System.Windows.Forms.GroupBox()
        Me.txtBusquedaGlobal = New System.Windows.Forms.TextBox()
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
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAcciones.SuspendLayout()
        Me.gbxBusquedaGlobal.SuspendLayout()
        Me.SuspendLayout()
        '
        'splitContenedorPrincipal
        '
        Me.splitContenedorPrincipal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.splitContenedorPrincipal.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.splitContenedorPrincipal.Location = New System.Drawing.Point(0, 0)
        Me.splitContenedorPrincipal.Name = "splitContenedorPrincipal"
        '
        'splitContenedorPrincipal.Panel1
        '
        Me.splitContenedorPrincipal.Panel1.Controls.Add(Me.pnlIzquierdo)
        Me.splitContenedorPrincipal.Panel1MinSize = 350
        '
        'splitContenedorPrincipal.Panel2
        '
        Me.splitContenedorPrincipal.Panel2.Controls.Add(Me.pnlDerecho)
        Me.splitContenedorPrincipal.Panel2MinSize = 400
        Me.splitContenedorPrincipal.Size = New System.Drawing.Size(1493, 749)
        Me.splitContenedorPrincipal.SplitterDistance = 450
        Me.splitContenedorPrincipal.TabIndex = 0
        '
        'pnlIzquierdo
        '
        Me.pnlIzquierdo.AutoScroll = True
        Me.pnlIzquierdo.Controls.Add(Me.gbxFiltros)
        Me.pnlIzquierdo.Controls.Add(Me.gbxOrigenDatos)
        Me.pnlIzquierdo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlIzquierdo.Location = New System.Drawing.Point(0, 0)
        Me.pnlIzquierdo.Name = "pnlIzquierdo"
        Me.pnlIzquierdo.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlIzquierdo.Size = New System.Drawing.Size(450, 749)
        Me.pnlIzquierdo.TabIndex = 0
        '
        'gbxFiltros
        '
        Me.gbxFiltros.Controls.Add(Me.TableLayoutPanel1)
        Me.gbxFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxFiltros.Location = New System.Drawing.Point(10, 169)
        Me.gbxFiltros.Name = "gbxFiltros"
        Me.gbxFiltros.Padding = New System.Windows.Forms.Padding(10)
        Me.gbxFiltros.Size = New System.Drawing.Size(430, 570)
        Me.gbxFiltros.TabIndex = 1
        Me.gbxFiltros.TabStop = False
        Me.gbxFiltros.Text = "2. Construir Filtro"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.lstColumnas, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.lstValores, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.pnlFiltroBotones, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(10, 25)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(410, 535)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'lstColumnas
        '
        Me.lstColumnas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstColumnas.FormattingEnabled = True
        Me.lstColumnas.ItemHeight = 16
        Me.lstColumnas.Location = New System.Drawing.Point(3, 3)
        Me.lstColumnas.Name = "lstColumnas"
        Me.lstColumnas.Size = New System.Drawing.Size(199, 484)
        Me.lstColumnas.TabIndex = 0
        '
        'lstValores
        '
        Me.lstValores.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstValores.FormattingEnabled = True
        Me.lstValores.ItemHeight = 16
        Me.lstValores.Location = New System.Drawing.Point(208, 3)
        Me.lstValores.Name = "lstValores"
        Me.lstValores.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstValores.Size = New System.Drawing.Size(199, 484)
        Me.lstValores.TabIndex = 1
        '
        'pnlFiltroBotones
        '
        Me.pnlFiltroBotones.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pnlFiltroBotones.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.pnlFiltroBotones, 2)
        Me.pnlFiltroBotones.Controls.Add(Me.btnAgregar)
        Me.pnlFiltroBotones.Controls.Add(Me.btnLimpiar)
        Me.pnlFiltroBotones.Location = New System.Drawing.Point(111, 502)
        Me.pnlFiltroBotones.Name = "pnlFiltroBotones"
        Me.pnlFiltroBotones.Size = New System.Drawing.Size(188, 30)
        Me.pnlFiltroBotones.TabIndex = 2
        '
        'btnAgregar
        '
        Me.btnAgregar.Location = New System.Drawing.Point(3, 3)
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(88, 24)
        Me.btnAgregar.TabIndex = 0
        Me.btnAgregar.Text = "Agregar"
        Me.btnAgregar.UseVisualStyleBackColor = True
        '
        'btnLimpiar
        '
        Me.btnLimpiar.Location = New System.Drawing.Point(97, 3)
        Me.btnLimpiar.Name = "btnLimpiar"
        Me.btnLimpiar.Size = New System.Drawing.Size(88, 24)
        Me.btnLimpiar.TabIndex = 1
        Me.btnLimpiar.Text = "Limpiar"
        Me.btnLimpiar.UseVisualStyleBackColor = True
        '
        'gbxOrigenDatos
        '
        Me.gbxOrigenDatos.Controls.Add(Me.TableLayoutPanel2)
        Me.gbxOrigenDatos.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbxOrigenDatos.Location = New System.Drawing.Point(10, 10)
        Me.gbxOrigenDatos.Name = "gbxOrigenDatos"
        Me.gbxOrigenDatos.Padding = New System.Windows.Forms.Padding(10)
        Me.gbxOrigenDatos.Size = New System.Drawing.Size(430, 159)
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
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(10, 25)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 4
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(410, 124)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'cmbOrigenDatos
        '
        Me.cmbOrigenDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbOrigenDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrigenDatos.FormattingEnabled = True
        Me.cmbOrigenDatos.Location = New System.Drawing.Point(60, 3)
        Me.cmbOrigenDatos.Name = "cmbOrigenDatos"
        Me.cmbOrigenDatos.Size = New System.Drawing.Size(347, 24)
        Me.cmbOrigenDatos.TabIndex = 0
        '
        'dtpFechaInicio
        '
        Me.dtpFechaInicio.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaInicio.Location = New System.Drawing.Point(60, 33)
        Me.dtpFechaInicio.Name = "dtpFechaInicio"
        Me.dtpFechaInicio.Size = New System.Drawing.Size(117, 22)
        Me.dtpFechaInicio.TabIndex = 1
        '
        'dtpFechaFin
        '
        Me.dtpFechaFin.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaFin.Location = New System.Drawing.Point(60, 61)
        Me.dtpFechaFin.Name = "dtpFechaFin"
        Me.dtpFechaFin.Size = New System.Drawing.Size(117, 22)
        Me.dtpFechaFin.TabIndex = 2
        '
        'btnCargar
        '
        Me.btnCargar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCargar.Location = New System.Drawing.Point(312, 89)
        Me.btnCargar.Name = "btnCargar"
        Me.btnCargar.Size = New System.Drawing.Size(95, 28)
        Me.btnCargar.TabIndex = 3
        Me.btnCargar.Text = "Cargar"
        Me.btnCargar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Origen:"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Desde:"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 16)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Hasta:"
        '
        'pnlDerecho
        '
        Me.pnlDerecho.Controls.Add(Me.dgvDatos)
        Me.pnlDerecho.Controls.Add(Me.pnlAcciones)
        Me.pnlDerecho.Controls.Add(Me.flpChips)
        Me.pnlDerecho.Controls.Add(Me.gbxBusquedaGlobal)
        Me.pnlDerecho.Location = New System.Drawing.Point(0, 0)
        Me.pnlDerecho.Name = "pnlDerecho"
        Me.pnlDerecho.Padding = New System.Windows.Forms.Padding(10)
        Me.pnlDerecho.Size = New System.Drawing.Size(1039, 749)
        Me.pnlDerecho.TabIndex = 0
        '
        'dgvDatos
        '
        Me.dgvDatos.AllowDrop = True
        Me.dgvDatos.AllowUserToAddRows = False
        Me.dgvDatos.AllowUserToDeleteRows = False
        Me.dgvDatos.AllowUserToResizeColumns = False
        Me.dgvDatos.AllowUserToResizeRows = False
        Me.dgvDatos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvDatos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDatos.Location = New System.Drawing.Point(10, 124)
        Me.dgvDatos.Name = "dgvDatos"
        Me.dgvDatos.ReadOnly = True
        Me.dgvDatos.RowHeadersWidth = 51
        Me.dgvDatos.RowTemplate.Height = 24
        Me.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDatos.Size = New System.Drawing.Size(1019, 568)
        Me.dgvDatos.TabIndex = 4
        '
        'pnlAcciones
        '
        Me.pnlAcciones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlAcciones.Controls.Add(Me.btnExportarExcel)
        Me.pnlAcciones.Controls.Add(Me.btnCopiarCorreos)
        Me.pnlAcciones.Controls.Add(Me.btnExportarFichasPDF)
        Me.pnlAcciones.Controls.Add(Me.lblConteoRegistros)
        Me.pnlAcciones.Controls.Add(Me.Separator1)
        Me.pnlAcciones.Controls.Add(Me.btnNuevaNotificacion)
        Me.pnlAcciones.Controls.Add(Me.btnEditarNotificacion)
        Me.pnlAcciones.Controls.Add(Me.btnEliminarNotificacion)
        Me.pnlAcciones.Controls.Add(Me.btnCambiarEstado)
        Me.pnlAcciones.Controls.Add(Me.btnNuevaLicencia)
        Me.pnlAcciones.Controls.Add(Me.btnEditarLicencia)
        Me.pnlAcciones.Controls.Add(Me.btnEliminarLicencia)
        Me.pnlAcciones.Controls.Add(Me.btnSiguiente)
        Me.pnlAcciones.Controls.Add(Me.lblPaginacion)
        Me.pnlAcciones.Controls.Add(Me.btnAnterior)
        Me.pnlAcciones.Location = New System.Drawing.Point(10, 698)
        Me.pnlAcciones.Name = "pnlAcciones"
        Me.pnlAcciones.Padding = New System.Windows.Forms.Padding(5)
        Me.pnlAcciones.Size = New System.Drawing.Size(1019, 85)
        Me.pnlAcciones.TabIndex = 3
        '
        'btnExportarExcel
        '
        Me.btnExportarExcel.Location = New System.Drawing.Point(8, 8)
        Me.btnExportarExcel.Name = "btnExportarExcel"
        Me.btnExportarExcel.Size = New System.Drawing.Size(121, 30)
        Me.btnExportarExcel.TabIndex = 0
        Me.btnExportarExcel.Text = "Exportar Excel"
        Me.btnExportarExcel.UseVisualStyleBackColor = True
        '
        'btnCopiarCorreos
        '
        Me.btnCopiarCorreos.Location = New System.Drawing.Point(135, 8)
        Me.btnCopiarCorreos.Name = "btnCopiarCorreos"
        Me.btnCopiarCorreos.Size = New System.Drawing.Size(121, 30)
        Me.btnCopiarCorreos.TabIndex = 1
        Me.btnCopiarCorreos.Text = "Copiar Correos"
        Me.btnCopiarCorreos.UseVisualStyleBackColor = True
        '
        'btnExportarFichasPDF
        '
        Me.btnExportarFichasPDF.Location = New System.Drawing.Point(262, 8)
        Me.btnExportarFichasPDF.Name = "btnExportarFichasPDF"
        Me.btnExportarFichasPDF.Size = New System.Drawing.Size(121, 30)
        Me.btnExportarFichasPDF.TabIndex = 2
        Me.btnExportarFichasPDF.Text = "Exportar Fichas"
        Me.btnExportarFichasPDF.UseVisualStyleBackColor = True
        '
        'lblConteoRegistros
        '
        Me.lblConteoRegistros.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblConteoRegistros.AutoSize = True
        Me.lblConteoRegistros.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConteoRegistros.Location = New System.Drawing.Point(389, 15)
        Me.lblConteoRegistros.Name = "lblConteoRegistros"
        Me.lblConteoRegistros.Size = New System.Drawing.Size(180, 16)
        Me.lblConteoRegistros.TabIndex = 3
        Me.lblConteoRegistros.Text = "Registros encontrados: 0"
        '
        'Separator1
        '
        Me.Separator1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Separator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Separator1.Location = New System.Drawing.Point(575, 8)
        Me.Separator1.Name = "Separator1"
        Me.Separator1.Size = New System.Drawing.Size(2, 30)
        Me.Separator1.TabIndex = 4
        '
        'btnNuevaNotificacion
        '
        Me.btnNuevaNotificacion.Location = New System.Drawing.Point(583, 8)
        Me.btnNuevaNotificacion.Name = "btnNuevaNotificacion"
        Me.btnNuevaNotificacion.Size = New System.Drawing.Size(75, 30)
        Me.btnNuevaNotificacion.TabIndex = 5
        Me.btnNuevaNotificacion.Text = "Nueva"
        Me.btnNuevaNotificacion.UseVisualStyleBackColor = True
        '
        'btnEditarNotificacion
        '
        Me.btnEditarNotificacion.Location = New System.Drawing.Point(664, 8)
        Me.btnEditarNotificacion.Name = "btnEditarNotificacion"
        Me.btnEditarNotificacion.Size = New System.Drawing.Size(75, 30)
        Me.btnEditarNotificacion.TabIndex = 6
        Me.btnEditarNotificacion.Text = "Editar"
        Me.btnEditarNotificacion.UseVisualStyleBackColor = True
        '
        'btnEliminarNotificacion
        '
        Me.btnEliminarNotificacion.Location = New System.Drawing.Point(745, 8)
        Me.btnEliminarNotificacion.Name = "btnEliminarNotificacion"
        Me.btnEliminarNotificacion.Size = New System.Drawing.Size(75, 30)
        Me.btnEliminarNotificacion.TabIndex = 7
        Me.btnEliminarNotificacion.Text = "Eliminar"
        Me.btnEliminarNotificacion.UseVisualStyleBackColor = True
        '
        'btnCambiarEstado
        '
        Me.btnCambiarEstado.Location = New System.Drawing.Point(826, 8)
        Me.btnCambiarEstado.Name = "btnCambiarEstado"
        Me.btnCambiarEstado.Size = New System.Drawing.Size(130, 30)
        Me.btnCambiarEstado.TabIndex = 8
        Me.btnCambiarEstado.Text = "Cambiar Estado"
        Me.btnCambiarEstado.UseVisualStyleBackColor = True
        '
        'btnNuevaLicencia
        '
        Me.pnlAcciones.SetFlowBreak(Me.btnNuevaLicencia, True)
        Me.btnNuevaLicencia.Location = New System.Drawing.Point(8, 44)
        Me.btnNuevaLicencia.Name = "btnNuevaLicencia"
        Me.btnNuevaLicencia.Size = New System.Drawing.Size(75, 30)
        Me.btnNuevaLicencia.TabIndex = 9
        Me.btnNuevaLicencia.Text = "Nueva"
        Me.btnNuevaLicencia.UseVisualStyleBackColor = True
        '
        'btnEditarLicencia
        '
        Me.btnEditarLicencia.Location = New System.Drawing.Point(8, 80)
        Me.btnEditarLicencia.Name = "btnEditarLicencia"
        Me.btnEditarLicencia.Size = New System.Drawing.Size(75, 30)
        Me.btnEditarLicencia.TabIndex = 10
        Me.btnEditarLicencia.Text = "Editar"
        Me.btnEditarLicencia.UseVisualStyleBackColor = True
        '
        'btnEliminarLicencia
        '
        Me.btnEliminarLicencia.Location = New System.Drawing.Point(89, 80)
        Me.btnEliminarLicencia.Name = "btnEliminarLicencia"
        Me.btnEliminarLicencia.Size = New System.Drawing.Size(75, 30)
        Me.btnEliminarLicencia.TabIndex = 11
        Me.btnEliminarLicencia.Text = "Eliminar"
        Me.btnEliminarLicencia.UseVisualStyleBackColor = True
        '
        'btnSiguiente
        '
        Me.btnSiguiente.Location = New System.Drawing.Point(170, 80)
        Me.btnSiguiente.Name = "btnSiguiente"
        Me.btnSiguiente.Size = New System.Drawing.Size(94, 30)
        Me.btnSiguiente.TabIndex = 12
        Me.btnSiguiente.Text = "Siguiente >"
        Me.btnSiguiente.UseVisualStyleBackColor = True
        '
        'lblPaginacion
        '
        Me.lblPaginacion.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblPaginacion.AutoSize = True
        Me.lblPaginacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaginacion.Location = New System.Drawing.Point(270, 87)
        Me.lblPaginacion.Name = "lblPaginacion"
        Me.lblPaginacion.Size = New System.Drawing.Size(102, 16)
        Me.lblPaginacion.TabIndex = 13
        Me.lblPaginacion.Text = "Página 0 de 0"
        '
        'btnAnterior
        '
        Me.btnAnterior.Location = New System.Drawing.Point(378, 80)
        Me.btnAnterior.Name = "btnAnterior"
        Me.btnAnterior.Size = New System.Drawing.Size(94, 30)
        Me.btnAnterior.TabIndex = 14
        Me.btnAnterior.Text = "< Anterior"
        Me.btnAnterior.UseVisualStyleBackColor = True
        '
        'flpChips
        '
        Me.flpChips.AutoScroll = True
        Me.flpChips.Dock = System.Windows.Forms.DockStyle.Top
        Me.flpChips.Location = New System.Drawing.Point(10, 72)
        Me.flpChips.Name = "flpChips"
        Me.flpChips.Padding = New System.Windows.Forms.Padding(5)
        Me.flpChips.Size = New System.Drawing.Size(1019, 42)
        Me.flpChips.TabIndex = 2
        Me.flpChips.WrapContents = False
        '
        'gbxBusquedaGlobal
        '
        Me.gbxBusquedaGlobal.Controls.Add(Me.txtBusquedaGlobal)
        Me.gbxBusquedaGlobal.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbxBusquedaGlobal.Location = New System.Drawing.Point(10, 10)
        Me.gbxBusquedaGlobal.Name = "gbxBusquedaGlobal"
        Me.gbxBusquedaGlobal.Padding = New System.Windows.Forms.Padding(10)
        Me.gbxBusquedaGlobal.Size = New System.Drawing.Size(1019, 62)
        Me.gbxBusquedaGlobal.TabIndex = 0
        Me.gbxBusquedaGlobal.TabStop = False
        Me.gbxBusquedaGlobal.Text = "Búsqueda Rápida en todos los campos de texto"
        '
        'txtBusquedaGlobal
        '
        Me.txtBusquedaGlobal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBusquedaGlobal.Location = New System.Drawing.Point(10, 25)
        Me.txtBusquedaGlobal.Name = "txtBusquedaGlobal"
        Me.txtBusquedaGlobal.Size = New System.Drawing.Size(999, 22)
        Me.txtBusquedaGlobal.TabIndex = 0
        '
        'frmFiltroAvanzado
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1493, 749)
        Me.Controls.Add(Me.splitContenedorPrincipal)
        Me.MinimumSize = New System.Drawing.Size(900, 600)
        Me.Name = "frmFiltroAvanzado"
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
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAcciones.ResumeLayout(False)
        Me.pnlAcciones.PerformLayout()
        Me.gbxBusquedaGlobal.ResumeLayout(False)
        Me.gbxBusquedaGlobal.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
End Class