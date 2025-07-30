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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.dgvDatos = New System.Windows.Forms.DataGridView()
        Me.flpChips = New System.Windows.Forms.FlowLayoutPanel()
        Me.gbxBusquedaGlobal = New System.Windows.Forms.GroupBox()
        Me.txtBusquedaGlobal = New System.Windows.Forms.TextBox()
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
        Me.Panel1.SuspendLayout()
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbxBusquedaGlobal.SuspendLayout()
        Me.pnlAcciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'splitContenedorPrincipal
        '
        Me.splitContenedorPrincipal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        ' Permitir que ambos paneles del SplitContainer se redimensionen de manera
        ' proporcional al tamaño del formulario. Esto hace que el DataGridView y
        ' los controles de la columna izquierda se adapten mejor a cambios de tamaño.
        Me.splitContenedorPrincipal.FixedPanel = System.Windows.Forms.FixedPanel.None
        Me.splitContenedorPrincipal.Location = New System.Drawing.Point(0, 0)
        Me.splitContenedorPrincipal.Margin = New System.Windows.Forms.Padding(2)
        Me.splitContenedorPrincipal.Name = "splitContenedorPrincipal"
        '
        'splitContenedorPrincipal.Panel1
        '
        Me.splitContenedorPrincipal.Panel1.Controls.Add(Me.pnlIzquierdo)
        ' Reducir el tamaño mínimo del Panel1 para permitir que la interfaz se
        ' ajuste a tamaños de ventana más pequeños.
        Me.splitContenedorPrincipal.Panel1MinSize = 200
        '
        'splitContenedorPrincipal.Panel2
        '
        Me.splitContenedorPrincipal.Panel2.Controls.Add(Me.pnlDerecho)
        ' Reducir el tamaño mínimo del Panel2 para permitir que la interfaz se
        ' ajuste a tamaños de ventana más pequeños.
        Me.splitContenedorPrincipal.Panel2MinSize = 200
        Me.splitContenedorPrincipal.Size = New System.Drawing.Size(1028, 609)
        Me.splitContenedorPrincipal.SplitterDistance = 450
        Me.splitContenedorPrincipal.SplitterWidth = 3
        Me.splitContenedorPrincipal.TabIndex = 0
        '
        'pnlIzquierdo
        '
        Me.pnlIzquierdo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlIzquierdo.AutoScroll = True
        Me.pnlIzquierdo.Controls.Add(Me.gbxFiltros)
        Me.pnlIzquierdo.Controls.Add(Me.gbxOrigenDatos)
        Me.pnlIzquierdo.Location = New System.Drawing.Point(0, 0)
        Me.pnlIzquierdo.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlIzquierdo.Name = "pnlIzquierdo"
        Me.pnlIzquierdo.Padding = New System.Windows.Forms.Padding(8)
        Me.pnlIzquierdo.Size = New System.Drawing.Size(450, 609)
        Me.pnlIzquierdo.TabIndex = 0
        '
        'gbxFiltros
        '
        Me.gbxFiltros.Controls.Add(Me.TableLayoutPanel1)
        Me.gbxFiltros.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbxFiltros.Location = New System.Drawing.Point(8, 137)
        Me.gbxFiltros.Margin = New System.Windows.Forms.Padding(2)
        Me.gbxFiltros.Name = "gbxFiltros"
        Me.gbxFiltros.Padding = New System.Windows.Forms.Padding(8)
        Me.gbxFiltros.Size = New System.Drawing.Size(434, 464)
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(8, 21)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(2)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(418, 435)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'lstColumnas
        '
        Me.lstColumnas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstColumnas.FormattingEnabled = True
        Me.lstColumnas.Location = New System.Drawing.Point(2, 2)
        Me.lstColumnas.Margin = New System.Windows.Forms.Padding(2)
        Me.lstColumnas.Name = "lstColumnas"
        Me.lstColumnas.Size = New System.Drawing.Size(205, 403)
        Me.lstColumnas.TabIndex = 0
        '
        'lstValores
        '
        Me.lstValores.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstValores.FormattingEnabled = True
        Me.lstValores.Location = New System.Drawing.Point(211, 2)
        Me.lstValores.Margin = New System.Windows.Forms.Padding(2)
        Me.lstValores.Name = "lstValores"
        Me.lstValores.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstValores.Size = New System.Drawing.Size(205, 403)
        Me.lstValores.TabIndex = 1
        '
        'pnlFiltroBotones
        '
        Me.pnlFiltroBotones.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pnlFiltroBotones.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.pnlFiltroBotones, 2)
        Me.pnlFiltroBotones.Controls.Add(Me.btnAgregar)
        Me.pnlFiltroBotones.Controls.Add(Me.btnLimpiar)
        Me.pnlFiltroBotones.Location = New System.Drawing.Point(139, 409)
        Me.pnlFiltroBotones.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlFiltroBotones.Name = "pnlFiltroBotones"
        Me.pnlFiltroBotones.Size = New System.Drawing.Size(140, 24)
        Me.pnlFiltroBotones.TabIndex = 2
        '
        'btnAgregar
        '
        Me.btnAgregar.Location = New System.Drawing.Point(2, 2)
        Me.btnAgregar.Margin = New System.Windows.Forms.Padding(2)
        Me.btnAgregar.Name = "btnAgregar"
        Me.btnAgregar.Size = New System.Drawing.Size(66, 20)
        Me.btnAgregar.TabIndex = 0
        Me.btnAgregar.Text = "Agregar"
        Me.btnAgregar.UseVisualStyleBackColor = True
        '
        'btnLimpiar
        '
        Me.btnLimpiar.Location = New System.Drawing.Point(72, 2)
        Me.btnLimpiar.Margin = New System.Windows.Forms.Padding(2)
        Me.btnLimpiar.Name = "btnLimpiar"
        Me.btnLimpiar.Size = New System.Drawing.Size(66, 20)
        Me.btnLimpiar.TabIndex = 1
        Me.btnLimpiar.Text = "Limpiar"
        Me.btnLimpiar.UseVisualStyleBackColor = True
        '
        'gbxOrigenDatos
        '
        Me.gbxOrigenDatos.Controls.Add(Me.TableLayoutPanel2)
        Me.gbxOrigenDatos.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbxOrigenDatos.Location = New System.Drawing.Point(8, 8)
        Me.gbxOrigenDatos.Margin = New System.Windows.Forms.Padding(2)
        Me.gbxOrigenDatos.Name = "gbxOrigenDatos"
        Me.gbxOrigenDatos.Padding = New System.Windows.Forms.Padding(8)
        Me.gbxOrigenDatos.Size = New System.Drawing.Size(434, 129)
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
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(8, 21)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(2)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 4
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(418, 100)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'cmbOrigenDatos
        '
        Me.cmbOrigenDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cmbOrigenDatos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOrigenDatos.FormattingEnabled = True
        Me.cmbOrigenDatos.Location = New System.Drawing.Point(47, 2)
        Me.cmbOrigenDatos.Margin = New System.Windows.Forms.Padding(2)
        Me.cmbOrigenDatos.Name = "cmbOrigenDatos"
        Me.cmbOrigenDatos.Size = New System.Drawing.Size(369, 21)
        Me.cmbOrigenDatos.TabIndex = 0
        '
        'dtpFechaInicio
        '
        Me.dtpFechaInicio.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaInicio.Location = New System.Drawing.Point(47, 27)
        Me.dtpFechaInicio.Margin = New System.Windows.Forms.Padding(2)
        Me.dtpFechaInicio.Name = "dtpFechaInicio"
        Me.dtpFechaInicio.Size = New System.Drawing.Size(89, 20)
        Me.dtpFechaInicio.TabIndex = 1
        '
        'dtpFechaFin
        '
        Me.dtpFechaFin.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaFin.Location = New System.Drawing.Point(47, 51)
        Me.dtpFechaFin.Margin = New System.Windows.Forms.Padding(2)
        Me.dtpFechaFin.Name = "dtpFechaFin"
        Me.dtpFechaFin.Size = New System.Drawing.Size(89, 20)
        Me.dtpFechaFin.TabIndex = 2
        '
        'btnCargar
        '
        Me.btnCargar.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCargar.Location = New System.Drawing.Point(345, 75)
        Me.btnCargar.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCargar.Name = "btnCargar"
        Me.btnCargar.Size = New System.Drawing.Size(71, 23)
        Me.btnCargar.TabIndex = 3
        Me.btnCargar.Text = "Cargar"
        Me.btnCargar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(2, 6)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Origen:"
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 30)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(41, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Desde:"
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(5, 54)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Hasta:"
        '
        'pnlDerecho
        '
        Me.pnlDerecho.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlDerecho.Controls.Add(Me.Panel1)
        Me.pnlDerecho.Controls.Add(Me.flpChips)
        Me.pnlDerecho.Controls.Add(Me.gbxBusquedaGlobal)
        Me.pnlDerecho.Controls.Add(Me.pnlAcciones)
        Me.pnlDerecho.Location = New System.Drawing.Point(1, 0)
        Me.pnlDerecho.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlDerecho.Name = "pnlDerecho"
        Me.pnlDerecho.Padding = New System.Windows.Forms.Padding(8)
        Me.pnlDerecho.Size = New System.Drawing.Size(577, 609)
        Me.pnlDerecho.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.dgvDatos)
        Me.Panel1.Location = New System.Drawing.Point(8, 100)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(560, 434)
        Me.Panel1.TabIndex = 5
        '
        'dgvDatos
        '
        Me.dgvDatos.AllowDrop = True
        Me.dgvDatos.AllowUserToAddRows = False
        Me.dgvDatos.AllowUserToDeleteRows = False
        Me.dgvDatos.AllowUserToResizeColumns = False
        Me.dgvDatos.AllowUserToResizeRows = False
        ' Ajustar el modo de ajuste de columnas para que las columnas ocupen de
        ' manera proporcional el espacio disponible dentro del DataGridView. De esta
        ' forma, al cambiar el tamaño del formulario, el grid se redimensionará de
        ' forma automática sin afectar la funcionalidad existente.
        Me.dgvDatos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDatos.Location = New System.Drawing.Point(0, 0)
        Me.dgvDatos.Margin = New System.Windows.Forms.Padding(2)
        Me.dgvDatos.Name = "dgvDatos"
        Me.dgvDatos.ReadOnly = True
        Me.dgvDatos.RowHeadersWidth = 51
        Me.dgvDatos.RowTemplate.Height = 24
        Me.dgvDatos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDatos.Size = New System.Drawing.Size(560, 434)
        Me.dgvDatos.TabIndex = 4
        '
        'flpChips
        '
        Me.flpChips.AutoScroll = True
        Me.flpChips.Dock = System.Windows.Forms.DockStyle.Top
        Me.flpChips.Location = New System.Drawing.Point(8, 58)
        Me.flpChips.Margin = New System.Windows.Forms.Padding(2)
        Me.flpChips.Name = "flpChips"
        Me.flpChips.Padding = New System.Windows.Forms.Padding(4)
        Me.flpChips.Size = New System.Drawing.Size(561, 42)
        Me.flpChips.TabIndex = 2
        Me.flpChips.WrapContents = False
        '
        'gbxBusquedaGlobal
        '
        Me.gbxBusquedaGlobal.Controls.Add(Me.txtBusquedaGlobal)
        Me.gbxBusquedaGlobal.Dock = System.Windows.Forms.DockStyle.Top
        Me.gbxBusquedaGlobal.Location = New System.Drawing.Point(8, 8)
        Me.gbxBusquedaGlobal.Margin = New System.Windows.Forms.Padding(2)
        Me.gbxBusquedaGlobal.Name = "gbxBusquedaGlobal"
        Me.gbxBusquedaGlobal.Padding = New System.Windows.Forms.Padding(8)
        Me.gbxBusquedaGlobal.Size = New System.Drawing.Size(561, 50)
        Me.gbxBusquedaGlobal.TabIndex = 0
        Me.gbxBusquedaGlobal.TabStop = False
        Me.gbxBusquedaGlobal.Text = "Búsqueda Rápida en todos los campos de texto"
        '
        'txtBusquedaGlobal
        '
        Me.txtBusquedaGlobal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtBusquedaGlobal.Location = New System.Drawing.Point(8, 21)
        Me.txtBusquedaGlobal.Margin = New System.Windows.Forms.Padding(2)
        Me.txtBusquedaGlobal.Name = "txtBusquedaGlobal"
        Me.txtBusquedaGlobal.Size = New System.Drawing.Size(545, 20)
        Me.txtBusquedaGlobal.TabIndex = 0
        '
        'pnlAcciones
        '
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
        Me.pnlAcciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlAcciones.Location = New System.Drawing.Point(8, 534)
        Me.pnlAcciones.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlAcciones.Name = "pnlAcciones"
        Me.pnlAcciones.Padding = New System.Windows.Forms.Padding(4)
        Me.pnlAcciones.Size = New System.Drawing.Size(561, 67)
        Me.pnlAcciones.TabIndex = 3
        '
        'btnExportarExcel
        '
        Me.btnExportarExcel.Location = New System.Drawing.Point(6, 6)
        Me.btnExportarExcel.Margin = New System.Windows.Forms.Padding(2)
        Me.btnExportarExcel.Name = "btnExportarExcel"
        Me.btnExportarExcel.Size = New System.Drawing.Size(91, 24)
        Me.btnExportarExcel.TabIndex = 0
        Me.btnExportarExcel.Text = "Exportar Excel"
        Me.btnExportarExcel.UseVisualStyleBackColor = True
        '
        'btnCopiarCorreos
        '
        Me.btnCopiarCorreos.Location = New System.Drawing.Point(101, 6)
        Me.btnCopiarCorreos.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCopiarCorreos.Name = "btnCopiarCorreos"
        Me.btnCopiarCorreos.Size = New System.Drawing.Size(91, 24)
        Me.btnCopiarCorreos.TabIndex = 1
        Me.btnCopiarCorreos.Text = "Copiar Correos"
        Me.btnCopiarCorreos.UseVisualStyleBackColor = True
        '
        'btnExportarFichasPDF
        '
        Me.btnExportarFichasPDF.Location = New System.Drawing.Point(196, 6)
        Me.btnExportarFichasPDF.Margin = New System.Windows.Forms.Padding(2)
        Me.btnExportarFichasPDF.Name = "btnExportarFichasPDF"
        Me.btnExportarFichasPDF.Size = New System.Drawing.Size(91, 24)
        Me.btnExportarFichasPDF.TabIndex = 2
        Me.btnExportarFichasPDF.Text = "Exportar Fichas"
        Me.btnExportarFichasPDF.UseVisualStyleBackColor = True
        '
        'lblConteoRegistros
        '
        Me.lblConteoRegistros.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblConteoRegistros.AutoSize = True
        Me.lblConteoRegistros.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConteoRegistros.Location = New System.Drawing.Point(291, 11)
        Me.lblConteoRegistros.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblConteoRegistros.Name = "lblConteoRegistros"
        Me.lblConteoRegistros.Size = New System.Drawing.Size(149, 13)
        Me.lblConteoRegistros.TabIndex = 3
        Me.lblConteoRegistros.Text = "Registros encontrados: 0"
        '
        'Separator1
        '
        Me.Separator1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Separator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Separator1.Location = New System.Drawing.Point(444, 6)
        Me.Separator1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Separator1.Name = "Separator1"
        Me.Separator1.Size = New System.Drawing.Size(2, 24)
        Me.Separator1.TabIndex = 4
        '
        'btnNuevaNotificacion
        '
        Me.btnNuevaNotificacion.Location = New System.Drawing.Point(450, 6)
        Me.btnNuevaNotificacion.Margin = New System.Windows.Forms.Padding(2)
        Me.btnNuevaNotificacion.Name = "btnNuevaNotificacion"
        Me.btnNuevaNotificacion.Size = New System.Drawing.Size(56, 24)
        Me.btnNuevaNotificacion.TabIndex = 5
        Me.btnNuevaNotificacion.Text = "Nueva"
        Me.btnNuevaNotificacion.UseVisualStyleBackColor = True
        '
        'btnEditarNotificacion
        '
        Me.btnEditarNotificacion.Location = New System.Drawing.Point(6, 34)
        Me.btnEditarNotificacion.Margin = New System.Windows.Forms.Padding(2)
        Me.btnEditarNotificacion.Name = "btnEditarNotificacion"
        Me.btnEditarNotificacion.Size = New System.Drawing.Size(56, 24)
        Me.btnEditarNotificacion.TabIndex = 6
        Me.btnEditarNotificacion.Text = "Editar"
        Me.btnEditarNotificacion.UseVisualStyleBackColor = True
        '
        'btnEliminarNotificacion
        '
        Me.btnEliminarNotificacion.Location = New System.Drawing.Point(66, 34)
        Me.btnEliminarNotificacion.Margin = New System.Windows.Forms.Padding(2)
        Me.btnEliminarNotificacion.Name = "btnEliminarNotificacion"
        Me.btnEliminarNotificacion.Size = New System.Drawing.Size(56, 24)
        Me.btnEliminarNotificacion.TabIndex = 7
        Me.btnEliminarNotificacion.Text = "Eliminar"
        Me.btnEliminarNotificacion.UseVisualStyleBackColor = True
        '
        'btnCambiarEstado
        '
        Me.btnCambiarEstado.Location = New System.Drawing.Point(126, 34)
        Me.btnCambiarEstado.Margin = New System.Windows.Forms.Padding(2)
        Me.btnCambiarEstado.Name = "btnCambiarEstado"
        Me.btnCambiarEstado.Size = New System.Drawing.Size(98, 24)
        Me.btnCambiarEstado.TabIndex = 8
        Me.btnCambiarEstado.Text = "Cambiar Estado"
        Me.btnCambiarEstado.UseVisualStyleBackColor = True
        '
        'btnNuevaLicencia
        '
        Me.pnlAcciones.SetFlowBreak(Me.btnNuevaLicencia, True)
        Me.btnNuevaLicencia.Location = New System.Drawing.Point(228, 34)
        Me.btnNuevaLicencia.Margin = New System.Windows.Forms.Padding(2)
        Me.btnNuevaLicencia.Name = "btnNuevaLicencia"
        Me.btnNuevaLicencia.Size = New System.Drawing.Size(56, 24)
        Me.btnNuevaLicencia.TabIndex = 9
        Me.btnNuevaLicencia.Text = "Nueva"
        Me.btnNuevaLicencia.UseVisualStyleBackColor = True
        '
        'btnEditarLicencia
        '
        Me.btnEditarLicencia.Location = New System.Drawing.Point(6, 62)
        Me.btnEditarLicencia.Margin = New System.Windows.Forms.Padding(2)
        Me.btnEditarLicencia.Name = "btnEditarLicencia"
        Me.btnEditarLicencia.Size = New System.Drawing.Size(56, 24)
        Me.btnEditarLicencia.TabIndex = 10
        Me.btnEditarLicencia.Text = "Editar"
        Me.btnEditarLicencia.UseVisualStyleBackColor = True
        '
        'btnEliminarLicencia
        '
        Me.btnEliminarLicencia.Location = New System.Drawing.Point(66, 62)
        Me.btnEliminarLicencia.Margin = New System.Windows.Forms.Padding(2)
        Me.btnEliminarLicencia.Name = "btnEliminarLicencia"
        Me.btnEliminarLicencia.Size = New System.Drawing.Size(56, 24)
        Me.btnEliminarLicencia.TabIndex = 11
        Me.btnEliminarLicencia.Text = "Eliminar"
        Me.btnEliminarLicencia.UseVisualStyleBackColor = True
        '
        'frmFiltroAvanzado
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 609)
        Me.Controls.Add(Me.splitContenedorPrincipal)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MinimumSize = New System.Drawing.Size(679, 495)
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
        Me.Panel1.ResumeLayout(False)
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbxBusquedaGlobal.ResumeLayout(False)
        Me.gbxBusquedaGlobal.PerformLayout()
        Me.pnlAcciones.ResumeLayout(False)
        Me.pnlAcciones.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
End Class