' ****************************************************************************************
'  frmFuncionarioBuscar (versión minimalista, moderna y adaptable)
'  - Mantiene los nombres de controles existentes para no romper código previo
'  - Usa paddings consistentes, tipografía Segoe UI y colores neutrales
'  - Mejora DataGridView (densidad, alternado, headers claros, selección suave)
'  - Botonera responsiva con TableLayoutPanel (se reacomoda por breakpoints)
'  - Soporte HiDPI vía AutoScaleMode=Font
'  - Sin bucles ni lógica en InitializeComponent (evita errores del diseñador)
'  - Estilizado y comportamiento responsivo se aplican en el code‑behind (2ª mitad)
' ****************************************************************************************

' =============================
'  ARCHIVO: frmFuncionarioBuscar.Designer.vb
' =============================

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioBuscar
    Inherits System.Windows.Forms.Form

#Region "Dispose"
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
#End Region

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.splitContenedor = New System.Windows.Forms.SplitContainer()
        Me.panelIzquierdo = New System.Windows.Forms.Panel()
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.lblContador = New System.Windows.Forms.Label()
        Me.panelBusqueda = New System.Windows.Forms.Panel()
        Me.btnLimpiarBusqueda = New System.Windows.Forms.Button()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.panelDetalle = New System.Windows.Forms.Panel()
        Me.panelAcciones = New System.Windows.Forms.Panel()
        Me.tableLayoutAcciones = New System.Windows.Forms.TableLayoutPanel()
        Me.btnNotificar = New System.Windows.Forms.Button()
        Me.btnSancionar = New System.Windows.Forms.Button()
        Me.btnNovedades = New System.Windows.Forms.Button()
        Me.btnVerSituacion = New System.Windows.Forms.Button()
        Me.btnGenerarFicha = New System.Windows.Forms.Button()
        Me.panelEstado = New System.Windows.Forms.Panel()
        Me.lblEstadoActividad = New System.Windows.Forms.Label()
        Me.panelInformacion = New System.Windows.Forms.Panel()
        Me.tableLayoutInfo = New System.Windows.Forms.TableLayoutPanel()
        Me.lblCargoHeader = New System.Windows.Forms.Label()
        Me.lblCargo = New System.Windows.Forms.Label()
        Me.lblTipoHeader = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.lblFechaIngresoHeader = New System.Windows.Forms.Label()
        Me.lblFechaIngreso = New System.Windows.Forms.Label()
        Me.lblHorarioCompletoHeader = New System.Windows.Forms.Label()
        Me.lblHorarioCompleto = New System.Windows.Forms.Label()
        Me.lblPresenciaHeader = New System.Windows.Forms.Label()
        Me.lblPresencia = New System.Windows.Forms.Label()
        Me.panelHeader = New System.Windows.Forms.Panel()
        Me.panelDatos = New System.Windows.Forms.Panel()
        Me.pbCopyNombre = New System.Windows.Forms.PictureBox()
        Me.pbCopyCI = New System.Windows.Forms.PictureBox()
        Me.lblNombreCompleto = New System.Windows.Forms.Label()
        Me.lblCI = New System.Windows.Forms.Label()
        Me.pbFotoDetalle = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedor.Panel1.SuspendLayout()
        Me.splitContenedor.Panel2.SuspendLayout()
        Me.splitContenedor.SuspendLayout()
        Me.panelIzquierdo.SuspendLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelBusqueda.SuspendLayout()
        Me.panelDetalle.SuspendLayout()
        Me.panelAcciones.SuspendLayout()
        Me.tableLayoutAcciones.SuspendLayout()
        Me.panelEstado.SuspendLayout()
        Me.panelInformacion.SuspendLayout()
        Me.tableLayoutInfo.SuspendLayout()
        Me.panelHeader.SuspendLayout()
        Me.panelDatos.SuspendLayout()
        CType(Me.pbCopyNombre, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCopyCI, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        ' splitContenedor
        '
        Me.splitContenedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContenedor.Location = New System.Drawing.Point(0, 0)
        Me.splitContenedor.Name = "splitContenedor"
        '
        ' splitContenedor.Panel1
        '
        Me.splitContenedor.Panel1.Controls.Add(Me.panelIzquierdo)
        Me.splitContenedor.Panel1MinSize = 320
        '
        ' splitContenedor.Panel2
        '
        Me.splitContenedor.Panel2.Controls.Add(Me.panelDetalle)
        Me.splitContenedor.Panel2MinSize = 540
        Me.splitContenedor.Size = New System.Drawing.Size(1400, 720)
        Me.splitContenedor.SplitterDistance = 420
        Me.splitContenedor.SplitterWidth = 8
        Me.splitContenedor.TabIndex = 0
        '
        ' panelIzquierdo
        '
        Me.panelIzquierdo.BackColor = System.Drawing.Color.WhiteSmoke
        Me.panelIzquierdo.Controls.Add(Me.dgvResultados)
        Me.panelIzquierdo.Controls.Add(Me.lblContador)
        Me.panelIzquierdo.Controls.Add(Me.panelBusqueda)
        Me.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelIzquierdo.Location = New System.Drawing.Point(0, 0)
        Me.panelIzquierdo.Name = "panelIzquierdo"
        Me.panelIzquierdo.Padding = New System.Windows.Forms.Padding(6)
        Me.panelIzquierdo.Size = New System.Drawing.Size(420, 720)
        Me.panelIzquierdo.TabIndex = 0
        '
        ' dgvResultados
        '
        Me.dgvResultados.AllowUserToAddRows = False
        Me.dgvResultados.AllowUserToDeleteRows = False
        Me.dgvResultados.AllowUserToResizeRows = False
        Me.dgvResultados.BackgroundColor = System.Drawing.Color.White
        Me.dgvResultados.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvResultados.ColumnHeadersHeight = 40
        Me.dgvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvResultados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvResultados.GridColor = System.Drawing.Color.FromArgb(233, 236, 239)
        Me.dgvResultados.Location = New System.Drawing.Point(6, 96)
        Me.dgvResultados.MultiSelect = False
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.ReadOnly = True
        Me.dgvResultados.RowHeadersVisible = False
        Me.dgvResultados.RowHeadersWidth = 62
        Me.dgvResultados.RowTemplate.Height = 36
        Me.dgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResultados.Size = New System.Drawing.Size(408, 588)
        Me.dgvResultados.TabIndex = 1
        '
        ' lblContador
        '
        Me.lblContador.BackColor = System.Drawing.Color.White
        Me.lblContador.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblContador.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lblContador.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125)
        Me.lblContador.Location = New System.Drawing.Point(6, 684)
        Me.lblContador.Name = "lblContador"
        Me.lblContador.Padding = New System.Windows.Forms.Padding(12, 8, 12, 8)
        Me.lblContador.Size = New System.Drawing.Size(408, 30)
        Me.lblContador.TabIndex = 2
        Me.lblContador.Text = "0 funcionarios encontrados"
        Me.lblContador.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        ' panelBusqueda
        '
        Me.panelBusqueda.BackColor = System.Drawing.Color.White
        Me.panelBusqueda.Controls.Add(Me.btnLimpiarBusqueda)
        Me.panelBusqueda.Controls.Add(Me.txtBusqueda)
        Me.panelBusqueda.Controls.Add(Me.lblBuscar)
        Me.panelBusqueda.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelBusqueda.Location = New System.Drawing.Point(6, 6)
        Me.panelBusqueda.Name = "panelBusqueda"
        Me.panelBusqueda.Padding = New System.Windows.Forms.Padding(16)
        Me.panelBusqueda.Size = New System.Drawing.Size(408, 90)
        Me.panelBusqueda.TabIndex = 0
        '
        ' btnLimpiarBusqueda
        '
        Me.btnLimpiarBusqueda.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLimpiarBusqueda.BackColor = System.Drawing.Color.FromArgb(231, 76, 60)
        Me.btnLimpiarBusqueda.FlatAppearance.BorderSize = 0
        Me.btnLimpiarBusqueda.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLimpiarBusqueda.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnLimpiarBusqueda.ForeColor = System.Drawing.Color.White
        Me.btnLimpiarBusqueda.Location = New System.Drawing.Point(346, 50)
        Me.btnLimpiarBusqueda.Name = "btnLimpiarBusqueda"
        Me.btnLimpiarBusqueda.Size = New System.Drawing.Size(46, 28)
        Me.btnLimpiarBusqueda.TabIndex = 2
        Me.btnLimpiarBusqueda.Text = "✕"
        Me.ToolTip1.SetToolTip(Me.btnLimpiarBusqueda, "Limpiar búsqueda")
        Me.btnLimpiarBusqueda.UseVisualStyleBackColor = False
        '
        ' txtBusqueda
        '
        Me.txtBusqueda.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBusqueda.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBusqueda.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtBusqueda.Location = New System.Drawing.Point(16, 48)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(324, 27)
        Me.txtBusqueda.TabIndex = 1
        '
        ' lblBuscar
        '
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblBuscar.Location = New System.Drawing.Point(16, 20)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(76, 23)
        Me.lblBuscar.TabIndex = 0
        Me.lblBuscar.Text = "Buscar:"
        '
        ' panelDetalle
        '
        Me.panelDetalle.BackColor = System.Drawing.Color.White
        Me.panelDetalle.Controls.Add(Me.panelAcciones)
        Me.panelDetalle.Controls.Add(Me.panelEstado)
        Me.panelDetalle.Controls.Add(Me.panelInformacion)
        Me.panelDetalle.Controls.Add(Me.panelHeader)
        Me.panelDetalle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelDetalle.Location = New System.Drawing.Point(0, 0)
        Me.panelDetalle.Name = "panelDetalle"
        Me.panelDetalle.Padding = New System.Windows.Forms.Padding(20)
        Me.panelDetalle.Size = New System.Drawing.Size(972, 720)
        Me.panelDetalle.TabIndex = 0
        '
        ' panelAcciones
        '
        Me.panelAcciones.Controls.Add(Me.tableLayoutAcciones)
        Me.panelAcciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelAcciones.Location = New System.Drawing.Point(20, 580)
        Me.panelAcciones.Name = "panelAcciones"
        Me.panelAcciones.Padding = New System.Windows.Forms.Padding(0, 12, 0, 0)
        Me.panelAcciones.Size = New System.Drawing.Size(932, 60)
        Me.panelAcciones.TabIndex = 3
        '
        ' tableLayoutAcciones
        '
        Me.tableLayoutAcciones.ColumnCount = 5
        Me.tableLayoutAcciones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutAcciones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutAcciones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutAcciones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutAcciones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutAcciones.Controls.Add(Me.btnNotificar, 0, 0)
        Me.tableLayoutAcciones.Controls.Add(Me.btnSancionar, 1, 0)
        Me.tableLayoutAcciones.Controls.Add(Me.btnNovedades, 2, 0)
        Me.tableLayoutAcciones.Controls.Add(Me.btnVerSituacion, 3, 0)
        Me.tableLayoutAcciones.Controls.Add(Me.btnGenerarFicha, 4, 0)
        Me.tableLayoutAcciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tableLayoutAcciones.Location = New System.Drawing.Point(0, 12)
        Me.tableLayoutAcciones.Name = "tableLayoutAcciones"
        Me.tableLayoutAcciones.RowCount = 1
        Me.tableLayoutAcciones.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tableLayoutAcciones.Size = New System.Drawing.Size(932, 48)
        Me.tableLayoutAcciones.TabIndex = 0
        '
        ' Botones (estilo plano y colores)
        '
        Me.btnNotificar.BackColor = System.Drawing.Color.FromArgb(0, 123, 255)
        Me.btnNotificar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNotificar.FlatAppearance.BorderSize = 0
        Me.btnNotificar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNotificar.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnNotificar.ForeColor = System.Drawing.Color.White
        Me.btnNotificar.Text = "📧 Notificar"
        Me.btnNotificar.Name = "btnNotificar"

        Me.btnSancionar.BackColor = System.Drawing.Color.FromArgb(220, 53, 69)
        Me.btnSancionar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSancionar.FlatAppearance.BorderSize = 0
        Me.btnSancionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSancionar.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnSancionar.ForeColor = System.Drawing.Color.White
        Me.btnSancionar.Text = "⚠️ Sancionar"
        Me.btnSancionar.Name = "btnSancionar"

        Me.btnNovedades.BackColor = System.Drawing.Color.FromArgb(255, 193, 7)
        Me.btnNovedades.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNovedades.FlatAppearance.BorderSize = 0
        Me.btnNovedades.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNovedades.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnNovedades.ForeColor = System.Drawing.Color.Black
        Me.btnNovedades.Text = "📰 Novedades"
        Me.btnNovedades.Name = "btnNovedades"

        Me.btnVerSituacion.BackColor = System.Drawing.Color.FromArgb(108, 117, 125)
        Me.btnVerSituacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnVerSituacion.FlatAppearance.BorderSize = 0
        Me.btnVerSituacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnVerSituacion.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerSituacion.ForeColor = System.Drawing.Color.White
        Me.btnVerSituacion.Text = "📊 Situación"
        Me.btnVerSituacion.Name = "btnVerSituacion"
        Me.btnVerSituacion.Visible = False

        Me.btnGenerarFicha.BackColor = System.Drawing.Color.FromArgb(40, 167, 69)
        Me.btnGenerarFicha.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnGenerarFicha.FlatAppearance.BorderSize = 0
        Me.btnGenerarFicha.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGenerarFicha.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.btnGenerarFicha.ForeColor = System.Drawing.Color.White
        Me.btnGenerarFicha.Text = "📄 Ficha"
        Me.btnGenerarFicha.Name = "btnGenerarFicha"
        Me.btnGenerarFicha.Visible = False
        '
        ' panelEstado
        '
        Me.panelEstado.BackColor = System.Drawing.Color.FromArgb(248, 249, 250)
        Me.panelEstado.Controls.Add(Me.lblEstadoActividad)
        Me.panelEstado.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelEstado.Location = New System.Drawing.Point(20, 640)
        Me.panelEstado.Name = "panelEstado"
        Me.panelEstado.Padding = New System.Windows.Forms.Padding(16)
        Me.panelEstado.Size = New System.Drawing.Size(932, 60)
        Me.panelEstado.TabIndex = 2
        '
        ' lblEstadoActividad
        '
        Me.lblEstadoActividad.AutoSize = True
        Me.lblEstadoActividad.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblEstadoActividad.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblEstadoActividad.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69)
        Me.lblEstadoActividad.Location = New System.Drawing.Point(16, 16)
        Me.lblEstadoActividad.Name = "lblEstadoActividad"
        Me.lblEstadoActividad.Size = New System.Drawing.Size(104, 21)
        Me.lblEstadoActividad.TabIndex = 0
        Me.lblEstadoActividad.Text = "Estado: -"
        Me.lblEstadoActividad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        ' panelInformacion
        '
        Me.panelInformacion.Controls.Add(Me.tableLayoutInfo)
        Me.panelInformacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelInformacion.Location = New System.Drawing.Point(20, 220)
        Me.panelInformacion.Name = "panelInformacion"
        Me.panelInformacion.Padding = New System.Windows.Forms.Padding(0, 16, 0, 16)
        Me.panelInformacion.Size = New System.Drawing.Size(932, 420)
        Me.panelInformacion.TabIndex = 1
        '
        ' tableLayoutInfo
        '
        Me.tableLayoutInfo.ColumnCount = 2
        Me.tableLayoutInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tableLayoutInfo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
        Me.tableLayoutInfo.Controls.Add(Me.lblCargoHeader, 0, 0)
        Me.tableLayoutInfo.Controls.Add(Me.lblCargo, 1, 0)
        Me.tableLayoutInfo.Controls.Add(Me.lblTipoHeader, 0, 1)
        Me.tableLayoutInfo.Controls.Add(Me.lblTipo, 1, 1)
        Me.tableLayoutInfo.Controls.Add(Me.lblFechaIngresoHeader, 0, 2)
        Me.tableLayoutInfo.Controls.Add(Me.lblFechaIngreso, 1, 2)
        Me.tableLayoutInfo.Controls.Add(Me.lblHorarioCompletoHeader, 0, 3)
        Me.tableLayoutInfo.Controls.Add(Me.lblHorarioCompleto, 1, 3)
        Me.tableLayoutInfo.Controls.Add(Me.lblPresenciaHeader, 0, 4)
        Me.tableLayoutInfo.Controls.Add(Me.lblPresencia, 1, 4)
        Me.tableLayoutInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tableLayoutInfo.Location = New System.Drawing.Point(0, 16)
        Me.tableLayoutInfo.Name = "tableLayoutInfo"
        Me.tableLayoutInfo.RowCount = 5
        Me.tableLayoutInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutInfo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.tableLayoutInfo.Size = New System.Drawing.Size(932, 388)
        Me.tableLayoutInfo.TabIndex = 0
        '
        ' Etiquetas de info
        '
        Me.lblCargoHeader.AutoSize = True
        Me.lblCargoHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCargoHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblCargoHeader.ForeColor = System.Drawing.Color.FromArgb(52, 58, 64)
        Me.lblCargoHeader.Text = "Cargo:"
        Me.lblCargoHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.lblCargo.AutoSize = True
        Me.lblCargo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblCargo.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblCargo.ForeColor = System.Drawing.Color.FromArgb(73, 80, 87)
        Me.lblCargo.Text = "-"
        Me.lblCargo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.lblTipoHeader.AutoSize = True
        Me.lblTipoHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTipoHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblTipoHeader.ForeColor = System.Drawing.Color.FromArgb(52, 58, 64)
        Me.lblTipoHeader.Text = "Tipo:"
        Me.lblTipoHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.lblTipo.AutoSize = True
        Me.lblTipo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblTipo.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblTipo.ForeColor = System.Drawing.Color.FromArgb(73, 80, 87)
        Me.lblTipo.Text = "-"
        Me.lblTipo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.lblFechaIngresoHeader.AutoSize = True
        Me.lblFechaIngresoHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFechaIngresoHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblFechaIngresoHeader.ForeColor = System.Drawing.Color.FromArgb(52, 58, 64)
        Me.lblFechaIngresoHeader.Text = "Fecha Ingreso:"
        Me.lblFechaIngresoHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.lblFechaIngreso.AutoSize = True
        Me.lblFechaIngreso.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblFechaIngreso.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblFechaIngreso.ForeColor = System.Drawing.Color.FromArgb(73, 80, 87)
        Me.lblFechaIngreso.Text = "-"
        Me.lblFechaIngreso.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.lblHorarioCompletoHeader.AutoSize = True
        Me.lblHorarioCompletoHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblHorarioCompletoHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblHorarioCompletoHeader.ForeColor = System.Drawing.Color.FromArgb(52, 58, 64)
        Me.lblHorarioCompletoHeader.Text = "Horario:"
        Me.lblHorarioCompletoHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.lblHorarioCompleto.AutoSize = True
        Me.lblHorarioCompleto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblHorarioCompleto.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblHorarioCompleto.ForeColor = System.Drawing.Color.FromArgb(73, 80, 87)
        Me.lblHorarioCompleto.Text = "-"
        Me.lblHorarioCompleto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.lblPresenciaHeader.AutoSize = True
        Me.lblPresenciaHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblPresenciaHeader.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.lblPresenciaHeader.ForeColor = System.Drawing.Color.FromArgb(52, 58, 64)
        Me.lblPresenciaHeader.Text = "Presencia:"
        Me.lblPresenciaHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft

        Me.lblPresencia.AutoSize = True
        Me.lblPresencia.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblPresencia.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblPresencia.ForeColor = System.Drawing.Color.FromArgb(73, 80, 87)
        Me.lblPresencia.Text = "-"
        Me.lblPresencia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        ' panelHeader
        '
        Me.panelHeader.Controls.Add(Me.panelDatos)
        Me.panelHeader.Controls.Add(Me.pbFotoDetalle)
        Me.panelHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelHeader.Location = New System.Drawing.Point(20, 20)
        Me.panelHeader.Name = "panelHeader"
        Me.panelHeader.Size = New System.Drawing.Size(932, 200)
        Me.panelHeader.TabIndex = 0
        '
        ' panelDatos
        '
        Me.panelDatos.Controls.Add(Me.pbCopyNombre)
        Me.panelDatos.Controls.Add(Me.pbCopyCI)
        Me.panelDatos.Controls.Add(Me.lblNombreCompleto)
        Me.panelDatos.Controls.Add(Me.lblCI)
        Me.panelDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelDatos.Location = New System.Drawing.Point(180, 0)
        Me.panelDatos.Name = "panelDatos"
        Me.panelDatos.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.panelDatos.Size = New System.Drawing.Size(752, 200)
        Me.panelDatos.TabIndex = 1
        '
        ' pbCopyNombre
        '
        Me.pbCopyNombre.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbCopyNombre.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCopyNombre.Image = Global.Apex.My.Resources.Resources.copy_icon
        Me.pbCopyNombre.Location = New System.Drawing.Point(710, 72)
        Me.pbCopyNombre.Name = "pbCopyNombre"
        Me.pbCopyNombre.Size = New System.Drawing.Size(24, 24)
        Me.pbCopyNombre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbCopyNombre.TabIndex = 3
        Me.pbCopyNombre.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCopyNombre, "Copiar nombre")
        Me.pbCopyNombre.Visible = False
        '
        ' pbCopyCI
        '
        Me.pbCopyCI.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbCopyCI.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCopyCI.Image = Global.Apex.My.Resources.Resources.copy_icon
        Me.pbCopyCI.Location = New System.Drawing.Point(710, 116)
        Me.pbCopyCI.Name = "pbCopyCI"
        Me.pbCopyCI.Size = New System.Drawing.Size(24, 24)
        Me.pbCopyCI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbCopyCI.TabIndex = 2
        Me.pbCopyCI.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCopyCI, "Copiar CI")
        Me.pbCopyCI.Visible = False
        '
        ' lblNombreCompleto
        '
        Me.lblNombreCompleto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNombreCompleto.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold)
        Me.lblNombreCompleto.ForeColor = System.Drawing.Color.FromArgb(33, 37, 41)
        Me.lblNombreCompleto.Location = New System.Drawing.Point(20, 60)
        Me.lblNombreCompleto.Name = "lblNombreCompleto"
        Me.lblNombreCompleto.Size = New System.Drawing.Size(686, 40)
        Me.lblNombreCompleto.TabIndex = 0
        Me.lblNombreCompleto.Text = "Seleccione un funcionario"
        Me.lblNombreCompleto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        ' lblCI
        '
        Me.lblCI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCI.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblCI.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125)
        Me.lblCI.Location = New System.Drawing.Point(20, 110)
        Me.lblCI.Name = "lblCI"
        Me.lblCI.Size = New System.Drawing.Size(686, 30)
        Me.lblCI.TabIndex = 1
        Me.lblCI.Text = "CI: -"
        Me.lblCI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        ' pbFotoDetalle
        '
        Me.pbFotoDetalle.BackColor = System.Drawing.Color.FromArgb(248, 249, 250)
        Me.pbFotoDetalle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbFotoDetalle.Dock = System.Windows.Forms.DockStyle.Left
        Me.pbFotoDetalle.Location = New System.Drawing.Point(0, 0)
        Me.pbFotoDetalle.Name = "pbFotoDetalle"
        Me.pbFotoDetalle.Size = New System.Drawing.Size(180, 200)
        Me.pbFotoDetalle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFotoDetalle.TabIndex = 0
        Me.pbFotoDetalle.TabStop = False
        '
        ' frmFuncionarioBuscar (form)
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1400, 720)
        Me.Controls.Add(Me.splitContenedor)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.MinimumSize = New System.Drawing.Size(1100, 600)
        Me.Name = "frmFuncionarioBuscar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "🔍 Buscar Funcionario"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.splitContenedor.Panel1.ResumeLayout(False)
        Me.splitContenedor.Panel2.ResumeLayout(False)
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedor.ResumeLayout(False)
        Me.panelIzquierdo.ResumeLayout(False)
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelBusqueda.ResumeLayout(False)
        Me.panelBusqueda.PerformLayout()
        Me.panelDetalle.ResumeLayout(False)
        Me.panelAcciones.ResumeLayout(False)
        Me.tableLayoutAcciones.ResumeLayout(False)
        Me.panelEstado.ResumeLayout(False)
        Me.panelEstado.PerformLayout()
        Me.panelInformacion.ResumeLayout(False)
        Me.tableLayoutInfo.ResumeLayout(False)
        Me.tableLayoutInfo.PerformLayout()
        Me.panelHeader.ResumeLayout(False)
        Me.panelDatos.ResumeLayout(False)
        CType(Me.pbCopyNombre, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCopyCI, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    ' Controles (nombres conservados)
    Friend WithEvents splitContenedor As System.Windows.Forms.SplitContainer
    Friend WithEvents panelIzquierdo As System.Windows.Forms.Panel
    Friend WithEvents dgvResultados As System.Windows.Forms.DataGridView
    Friend WithEvents lblContador As System.Windows.Forms.Label
    Friend WithEvents panelBusqueda As System.Windows.Forms.Panel
    Friend WithEvents btnLimpiarBusqueda As System.Windows.Forms.Button
    Friend WithEvents txtBusqueda As System.Windows.Forms.TextBox
    Friend WithEvents lblBuscar As System.Windows.Forms.Label

    Friend WithEvents panelDetalle As System.Windows.Forms.Panel
    Friend WithEvents panelHeader As System.Windows.Forms.Panel
    Friend WithEvents pbFotoDetalle As System.Windows.Forms.PictureBox
    Friend WithEvents panelDatos As System.Windows.Forms.Panel
    Friend WithEvents lblNombreCompleto As System.Windows.Forms.Label
    Friend WithEvents lblCI As System.Windows.Forms.Label
    Friend WithEvents pbCopyCI As System.Windows.Forms.PictureBox
    Friend WithEvents pbCopyNombre As System.Windows.Forms.PictureBox

    Friend WithEvents panelInformacion As System.Windows.Forms.Panel
    Friend WithEvents tableLayoutInfo As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblCargoHeader As System.Windows.Forms.Label
    Friend WithEvents lblCargo As System.Windows.Forms.Label
    Friend WithEvents lblTipoHeader As System.Windows.Forms.Label
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents lblFechaIngresoHeader As System.Windows.Forms.Label
    Friend WithEvents lblFechaIngreso As System.Windows.Forms.Label
    Friend WithEvents lblHorarioCompletoHeader As System.Windows.Forms.Label
    Friend WithEvents lblHorarioCompleto As System.Windows.Forms.Label
    Friend WithEvents lblPresenciaHeader As System.Windows.Forms.Label
    Friend WithEvents lblPresencia As System.Windows.Forms.Label

    Friend WithEvents panelEstado As System.Windows.Forms.Panel
    Friend WithEvents lblEstadoActividad As System.Windows.Forms.Label

    Friend WithEvents panelAcciones As System.Windows.Forms.Panel
    Friend WithEvents tableLayoutAcciones As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnNotificar As System.Windows.Forms.Button
    Friend WithEvents btnSancionar As System.Windows.Forms.Button
    Friend WithEvents btnNovedades As System.Windows.Forms.Button
    Friend WithEvents btnVerSituacion As System.Windows.Forms.Button
    Friend WithEvents btnGenerarFicha As System.Windows.Forms.Button

    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

End Class


'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
'Partial Class frmFuncionarioBuscar
'    Inherits System.Windows.Forms.Form

'#Region "Dispose"
'    <System.Diagnostics.DebuggerNonUserCode()>
'    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
'        Try
'            If disposing AndAlso (components IsNot Nothing) Then
'                components.Dispose()
'            End If
'        Finally
'            MyBase.Dispose(disposing)

'        End Try
'    End Sub
'#End Region

'    Private components As System.ComponentModel.IContainer

'    <System.Diagnostics.DebuggerStepThrough()>
'    Private Sub InitializeComponent()
'        Me.components = New System.ComponentModel.Container()
'        Me.splitContenedor = New System.Windows.Forms.SplitContainer()
'        Me.PanelBusquedaLista = New System.Windows.Forms.Panel()
'        Me.txtBusqueda = New System.Windows.Forms.TextBox()
'        Me.lblBuscar = New System.Windows.Forms.Label()
'        Me.dgvResultados = New System.Windows.Forms.DataGridView()
'        Me.panelDetalle = New System.Windows.Forms.Panel()
'        Me.flpAcciones = New System.Windows.Forms.FlowLayoutPanel()
'        Me.btnNotificar = New System.Windows.Forms.Button()
'        Me.btnSancionar = New System.Windows.Forms.Button()
'        Me.btnNovedades = New System.Windows.Forms.Button()
'        Me.btnVerSituacion = New System.Windows.Forms.Button()
'        Me.btnGenerarFicha = New System.Windows.Forms.Button()
'        Me.pbCopyNombre = New System.Windows.Forms.PictureBox()
'        Me.pbCopyCI = New System.Windows.Forms.PictureBox()
'        Me.lblEstadoActividad = New System.Windows.Forms.Label()
'        Me.lblHorarioCompleto = New System.Windows.Forms.Label()
'        Me.lblHorarioCompletoHeader = New System.Windows.Forms.Label()
'        Me.lblPresencia = New System.Windows.Forms.Label()
'        Me.lblPresenciaHeader = New System.Windows.Forms.Label()
'        Me.lblFechaIngreso = New System.Windows.Forms.Label()
'        Me.lblFechaIngresoHeader = New System.Windows.Forms.Label()
'        Me.lblTipo = New System.Windows.Forms.Label()
'        Me.lblTipoHeader = New System.Windows.Forms.Label()
'        Me.lblCargo = New System.Windows.Forms.Label()
'        Me.lblCargoHeader = New System.Windows.Forms.Label()
'        Me.lblNombreCompleto = New System.Windows.Forms.Label()
'        Me.lblCI = New System.Windows.Forms.Label()
'        Me.pbFotoDetalle = New System.Windows.Forms.PictureBox()
'        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
'        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
'        Me.splitContenedor.Panel1.SuspendLayout()
'        Me.splitContenedor.Panel2.SuspendLayout()
'        Me.splitContenedor.SuspendLayout()
'        Me.PanelBusquedaLista.SuspendLayout()
'        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
'        Me.panelDetalle.SuspendLayout()
'        Me.flpAcciones.SuspendLayout()
'        CType(Me.pbCopyNombre, System.ComponentModel.ISupportInitialize).BeginInit()
'        CType(Me.pbCopyCI, System.ComponentModel.ISupportInitialize).BeginInit()
'        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
'        Me.SuspendLayout()
'        '
'        'splitContenedor
'        '
'        Me.splitContenedor.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.splitContenedor.Location = New System.Drawing.Point(0, 0)
'        Me.splitContenedor.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
'        Me.splitContenedor.Name = "splitContenedor"
'        '
'        'splitContenedor.Panel1
'        '
'        Me.splitContenedor.Panel1.Controls.Add(Me.dgvResultados)
'        Me.splitContenedor.Panel1.Controls.Add(Me.PanelBusquedaLista)
'        '
'        'splitContenedor.Panel2
'        '
'        Me.splitContenedor.Panel2.Controls.Add(Me.panelDetalle)
'        Me.splitContenedor.Size = New System.Drawing.Size(1283, 539)
'        Me.splitContenedor.SplitterDistance = 400
'        Me.splitContenedor.SplitterWidth = 6
'        Me.splitContenedor.TabIndex = 1
'        '
'        'PanelBusquedaLista
'        '
'        Me.PanelBusquedaLista.BackColor = System.Drawing.Color.WhiteSmoke
'        Me.PanelBusquedaLista.Controls.Add(Me.txtBusqueda)
'        Me.PanelBusquedaLista.Controls.Add(Me.lblBuscar)
'        Me.PanelBusquedaLista.Dock = System.Windows.Forms.DockStyle.Top
'        Me.PanelBusquedaLista.Location = New System.Drawing.Point(0, 0)
'        Me.PanelBusquedaLista.Name = "PanelBusquedaLista"
'        Me.PanelBusquedaLista.Size = New System.Drawing.Size(400, 60)
'        Me.PanelBusquedaLista.TabIndex = 1
'        '
'        'txtBusqueda
'        '
'        Me.txtBusqueda.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.txtBusqueda.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
'        Me.txtBusqueda.Font = New System.Drawing.Font("Segoe UI", 9.75!)
'        Me.txtBusqueda.Location = New System.Drawing.Point(85, 15)
'        Me.txtBusqueda.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
'        Me.txtBusqueda.Name = "txtBusqueda"
'        Me.txtBusqueda.Size = New System.Drawing.Size(296, 29)
'        Me.txtBusqueda.TabIndex = 1
'        '
'        'lblBuscar
'        '
'        Me.lblBuscar.AutoSize = True
'        Me.lblBuscar.Font = New System.Drawing.Font("Segoe UI", 9.75!)
'        Me.lblBuscar.Location = New System.Drawing.Point(15, 18)
'        Me.lblBuscar.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblBuscar.Name = "lblBuscar"
'        Me.lblBuscar.Size = New System.Drawing.Size(64, 23)
'        Me.lblBuscar.TabIndex = 0
'        Me.lblBuscar.Text = "Buscar:"
'        '
'        'dgvResultados
'        '
'        Me.dgvResultados.AllowUserToAddRows = False
'        Me.dgvResultados.AllowUserToDeleteRows = False
'        Me.dgvResultados.AllowUserToResizeRows = False
'        Me.dgvResultados.BackgroundColor = System.Drawing.Color.White
'        Me.dgvResultados.BorderStyle = System.Windows.Forms.BorderStyle.None
'        Me.dgvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
'        Me.dgvResultados.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.dgvResultados.Location = New System.Drawing.Point(0, 60)
'        Me.dgvResultados.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
'        Me.dgvResultados.MultiSelect = False
'        Me.dgvResultados.Name = "dgvResultados"
'        Me.dgvResultados.ReadOnly = True
'        Me.dgvResultados.RowHeadersWidth = 51
'        Me.dgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
'        Me.dgvResultados.Size = New System.Drawing.Size(400, 479)
'        Me.dgvResultados.TabIndex = 0
'        '
'        'panelDetalle
'        '
'        Me.panelDetalle.BackColor = System.Drawing.Color.White
'        Me.panelDetalle.Controls.Add(Me.flpAcciones)
'        Me.panelDetalle.Controls.Add(Me.pbCopyNombre)
'        Me.panelDetalle.Controls.Add(Me.pbCopyCI)
'        Me.panelDetalle.Controls.Add(Me.lblEstadoActividad)
'        Me.panelDetalle.Controls.Add(Me.lblHorarioCompleto)
'        Me.panelDetalle.Controls.Add(Me.lblHorarioCompletoHeader)
'        Me.panelDetalle.Controls.Add(Me.lblPresencia)
'        Me.panelDetalle.Controls.Add(Me.lblPresenciaHeader)
'        Me.panelDetalle.Controls.Add(Me.lblFechaIngreso)
'        Me.panelDetalle.Controls.Add(Me.lblFechaIngresoHeader)
'        Me.panelDetalle.Controls.Add(Me.lblTipo)
'        Me.panelDetalle.Controls.Add(Me.lblTipoHeader)
'        Me.panelDetalle.Controls.Add(Me.lblCargo)
'        Me.panelDetalle.Controls.Add(Me.lblCargoHeader)
'        Me.panelDetalle.Controls.Add(Me.lblNombreCompleto)
'        Me.panelDetalle.Controls.Add(Me.lblCI)
'        Me.panelDetalle.Controls.Add(Me.pbFotoDetalle)
'        Me.panelDetalle.Dock = System.Windows.Forms.DockStyle.Fill
'        Me.panelDetalle.Location = New System.Drawing.Point(0, 0)
'        Me.panelDetalle.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
'        Me.panelDetalle.Name = "panelDetalle"
'        Me.panelDetalle.Padding = New System.Windows.Forms.Padding(15)
'        Me.panelDetalle.Size = New System.Drawing.Size(877, 539)
'        Me.panelDetalle.TabIndex = 0
'        '
'        'flpAcciones
'        '
'        Me.flpAcciones.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.flpAcciones.Controls.Add(Me.btnNotificar)
'        Me.flpAcciones.Controls.Add(Me.btnSancionar)
'        Me.flpAcciones.Controls.Add(Me.btnNovedades)
'        Me.flpAcciones.Controls.Add(Me.btnVerSituacion)
'        Me.flpAcciones.Controls.Add(Me.btnGenerarFicha)
'        Me.flpAcciones.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
'        Me.flpAcciones.Location = New System.Drawing.Point(729, 20)
'        Me.flpAcciones.Name = "flpAcciones"
'        Me.flpAcciones.Size = New System.Drawing.Size(128, 499)
'        Me.flpAcciones.TabIndex = 20
'        '
'        'btnNotificar
'        '
'        Me.btnNotificar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
'        Me.btnNotificar.Location = New System.Drawing.Point(3, 3)
'        Me.btnNotificar.Name = "btnNotificar"
'        Me.btnNotificar.Size = New System.Drawing.Size(120, 38)
'        Me.btnNotificar.TabIndex = 0
'        Me.btnNotificar.Text = "Notificar"
'        Me.btnNotificar.UseVisualStyleBackColor = True
'        '
'        'btnSancionar
'        '
'        Me.btnSancionar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
'        Me.btnSancionar.Location = New System.Drawing.Point(3, 47)
'        Me.btnSancionar.Name = "btnSancionar"
'        Me.btnSancionar.Size = New System.Drawing.Size(120, 38)
'        Me.btnSancionar.TabIndex = 1
'        Me.btnSancionar.Text = "Sancionar"
'        Me.btnSancionar.UseVisualStyleBackColor = True
'        '
'        'btnNovedades
'        '
'        Me.btnNovedades.Font = New System.Drawing.Font("Segoe UI", 9.0!)
'        Me.btnNovedades.Location = New System.Drawing.Point(3, 91)
'        Me.btnNovedades.Name = "btnNovedades"
'        Me.btnNovedades.Size = New System.Drawing.Size(120, 38)
'        Me.btnNovedades.TabIndex = 2
'        Me.btnNovedades.Text = "Novedades"
'        Me.btnNovedades.UseVisualStyleBackColor = True
'        '
'        'btnVerSituacion
'        '
'        Me.btnVerSituacion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
'        Me.btnVerSituacion.Location = New System.Drawing.Point(3, 135)
'        Me.btnVerSituacion.Name = "btnVerSituacion"
'        Me.btnVerSituacion.Size = New System.Drawing.Size(120, 38)
'        Me.btnVerSituacion.TabIndex = 17
'        Me.btnVerSituacion.Text = "Situación"
'        Me.btnVerSituacion.UseVisualStyleBackColor = True
'        Me.btnVerSituacion.Visible = False
'        '
'        'btnGenerarFicha
'        '
'        Me.btnGenerarFicha.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
'        Me.btnGenerarFicha.Location = New System.Drawing.Point(3, 179)
'        Me.btnGenerarFicha.Name = "btnGenerarFicha"
'        Me.btnGenerarFicha.Size = New System.Drawing.Size(120, 38)
'        Me.btnGenerarFicha.TabIndex = 16
'        Me.btnGenerarFicha.Text = "Ficha"
'        Me.btnGenerarFicha.UseVisualStyleBackColor = True
'        Me.btnGenerarFicha.Visible = False
'        '
'        'pbCopyNombre
'        '
'        Me.pbCopyNombre.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.pbCopyNombre.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.pbCopyNombre.Image = Global.Apex.My.Resources.Resources.copy_icon
'        Me.pbCopyNombre.Location = New System.Drawing.Point(685, 260)
'        Me.pbCopyNombre.Name = "pbCopyNombre"
'        Me.pbCopyNombre.Size = New System.Drawing.Size(24, 24)
'        Me.pbCopyNombre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
'        Me.pbCopyNombre.TabIndex = 19
'        Me.pbCopyNombre.TabStop = False
'        Me.ToolTip1.SetToolTip(Me.pbCopyNombre, "Copiar Nombre")
'        Me.pbCopyNombre.Visible = False
'        '
'        'pbCopyCI
'        '
'        Me.pbCopyCI.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.pbCopyCI.Cursor = System.Windows.Forms.Cursors.Hand
'        Me.pbCopyCI.Image = Global.Apex.My.Resources.Resources.copy_icon
'        Me.pbCopyCI.Location = New System.Drawing.Point(685, 222)
'        Me.pbCopyCI.Name = "pbCopyCI"
'        Me.pbCopyCI.Size = New System.Drawing.Size(24, 24)
'        Me.pbCopyCI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
'        Me.pbCopyCI.TabIndex = 18
'        Me.pbCopyCI.TabStop = False
'        Me.ToolTip1.SetToolTip(Me.pbCopyCI, "Copiar CI")
'        Me.pbCopyCI.Visible = False
'        '
'        'lblEstadoActividad
'        '
'        Me.lblEstadoActividad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblEstadoActividad.AutoSize = True
'        Me.lblEstadoActividad.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
'        Me.lblEstadoActividad.Location = New System.Drawing.Point(245, 496)
'        Me.lblEstadoActividad.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblEstadoActividad.Name = "lblEstadoActividad"
'        Me.lblEstadoActividad.Size = New System.Drawing.Size(83, 23)
'        Me.lblEstadoActividad.TabIndex = 14
'        Me.lblEstadoActividad.Text = "Estado: -"
'        '
'        'lblHorarioCompleto
'        '
'        Me.lblHorarioCompleto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblHorarioCompleto.AutoSize = True
'        Me.lblHorarioCompleto.Font = New System.Drawing.Font("Segoe UI", 9.75!)
'        Me.lblHorarioCompleto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.lblHorarioCompleto.Location = New System.Drawing.Point(395, 428)
'        Me.lblHorarioCompleto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblHorarioCompleto.Name = "lblHorarioCompleto"
'        Me.lblHorarioCompleto.Size = New System.Drawing.Size(17, 23)
'        Me.lblHorarioCompleto.TabIndex = 13
'        Me.lblHorarioCompleto.Text = "-"
'        '
'        'lblHorarioCompletoHeader
'        '
'        Me.lblHorarioCompletoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblHorarioCompletoHeader.AutoSize = True
'        Me.lblHorarioCompletoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
'        Me.lblHorarioCompletoHeader.Location = New System.Drawing.Point(245, 428)
'        Me.lblHorarioCompletoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblHorarioCompletoHeader.Name = "lblHorarioCompletoHeader"
'        Me.lblHorarioCompletoHeader.Size = New System.Drawing.Size(76, 23)
'        Me.lblHorarioCompletoHeader.TabIndex = 12
'        Me.lblHorarioCompletoHeader.Text = "Horario:"
'        '
'        'lblPresencia
'        '
'        Me.lblPresencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblPresencia.AutoSize = True
'        Me.lblPresencia.Font = New System.Drawing.Font("Segoe UI", 9.75!)
'        Me.lblPresencia.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.lblPresencia.Location = New System.Drawing.Point(395, 461)
'        Me.lblPresencia.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblPresencia.Name = "lblPresencia"
'        Me.lblPresencia.Size = New System.Drawing.Size(17, 23)
'        Me.lblPresencia.TabIndex = 10
'        Me.lblPresencia.Text = "-"
'        '
'        'lblPresenciaHeader
'        '
'        Me.lblPresenciaHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblPresenciaHeader.AutoSize = True
'        Me.lblPresenciaHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
'        Me.lblPresenciaHeader.Location = New System.Drawing.Point(245, 461)
'        Me.lblPresenciaHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblPresenciaHeader.Name = "lblPresenciaHeader"
'        Me.lblPresenciaHeader.Size = New System.Drawing.Size(89, 23)
'        Me.lblPresenciaHeader.TabIndex = 9
'        Me.lblPresenciaHeader.Text = "Presencia:"
'        '
'        'lblFechaIngreso
'        '
'        Me.lblFechaIngreso.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblFechaIngreso.AutoSize = True
'        Me.lblFechaIngreso.Font = New System.Drawing.Font("Segoe UI", 9.75!)
'        Me.lblFechaIngreso.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.lblFechaIngreso.Location = New System.Drawing.Point(395, 395)
'        Me.lblFechaIngreso.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblFechaIngreso.Name = "lblFechaIngreso"
'        Me.lblFechaIngreso.Size = New System.Drawing.Size(17, 23)
'        Me.lblFechaIngreso.TabIndex = 8
'        Me.lblFechaIngreso.Text = "-"
'        '
'        'lblFechaIngresoHeader
'        '
'        Me.lblFechaIngresoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblFechaIngresoHeader.AutoSize = True
'        Me.lblFechaIngresoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
'        Me.lblFechaIngresoHeader.Location = New System.Drawing.Point(245, 395)
'        Me.lblFechaIngresoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblFechaIngresoHeader.Name = "lblFechaIngresoHeader"
'        Me.lblFechaIngresoHeader.Size = New System.Drawing.Size(124, 23)
'        Me.lblFechaIngresoHeader.TabIndex = 7
'        Me.lblFechaIngresoHeader.Text = "Fecha Ingreso:"
'        '
'        'lblTipo
'        '
'        Me.lblTipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblTipo.AutoSize = True
'        Me.lblTipo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
'        Me.lblTipo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.lblTipo.Location = New System.Drawing.Point(395, 362)
'        Me.lblTipo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblTipo.Name = "lblTipo"
'        Me.lblTipo.Size = New System.Drawing.Size(17, 23)
'        Me.lblTipo.TabIndex = 6
'        Me.lblTipo.Text = "-"
'        '
'        'lblTipoHeader
'        '
'        Me.lblTipoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblTipoHeader.AutoSize = True
'        Me.lblTipoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
'        Me.lblTipoHeader.Location = New System.Drawing.Point(245, 362)
'        Me.lblTipoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblTipoHeader.Name = "lblTipoHeader"
'        Me.lblTipoHeader.Size = New System.Drawing.Size(49, 23)
'        Me.lblTipoHeader.TabIndex = 5
'        Me.lblTipoHeader.Text = "Tipo:"
'        '
'        'lblCargo
'        '
'        Me.lblCargo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblCargo.AutoSize = True
'        Me.lblCargo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
'        Me.lblCargo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
'        Me.lblCargo.Location = New System.Drawing.Point(395, 329)
'        Me.lblCargo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblCargo.Name = "lblCargo"
'        Me.lblCargo.Size = New System.Drawing.Size(17, 23)
'        Me.lblCargo.TabIndex = 4
'        Me.lblCargo.Text = "-"
'        '
'        'lblCargoHeader
'        '
'        Me.lblCargoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
'        Me.lblCargoHeader.AutoSize = True
'        Me.lblCargoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
'        Me.lblCargoHeader.Location = New System.Drawing.Point(245, 329)
'        Me.lblCargoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblCargoHeader.Name = "lblCargoHeader"
'        Me.lblCargoHeader.Size = New System.Drawing.Size(61, 23)
'        Me.lblCargoHeader.TabIndex = 3
'        Me.lblCargoHeader.Text = "Cargo:"
'        '
'        'lblNombreCompleto
'        '
'        Me.lblNombreCompleto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.lblNombreCompleto.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
'        Me.lblNombreCompleto.Location = New System.Drawing.Point(249, 252)
'        Me.lblNombreCompleto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblNombreCompleto.Name = "lblNombreCompleto"
'        Me.lblNombreCompleto.Size = New System.Drawing.Size(430, 39)
'        Me.lblNombreCompleto.TabIndex = 2
'        Me.lblNombreCompleto.Text = "Nombre Funcionario"
'        '
'        'lblCI
'        '
'        Me.lblCI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
'            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
'        Me.lblCI.Font = New System.Drawing.Font("Segoe UI", 11.25!)
'        Me.lblCI.ForeColor = System.Drawing.SystemColors.ControlDarkDark
'        Me.lblCI.Location = New System.Drawing.Point(249, 218)
'        Me.lblCI.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
'        Me.lblCI.Name = "lblCI"
'        Me.lblCI.Size = New System.Drawing.Size(430, 31)
'        Me.lblCI.TabIndex = 1
'        Me.lblCI.Text = "CI: -"
'        '
'        'pbFotoDetalle
'        '
'        Me.pbFotoDetalle.Location = New System.Drawing.Point(19, 20)
'        Me.pbFotoDetalle.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
'        Me.pbFotoDetalle.Name = "pbFotoDetalle"
'        Me.pbFotoDetalle.Size = New System.Drawing.Size(200, 271)
'        Me.pbFotoDetalle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
'        Me.pbFotoDetalle.TabIndex = 0
'        Me.pbFotoDetalle.TabStop = False
'        '
'        'frmFuncionarioBuscar
'        '
'        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
'        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
'        Me.ClientSize = New System.Drawing.Size(1283, 539)
'        Me.Controls.Add(Me.splitContenedor)
'        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
'        Me.Name = "frmFuncionarioBuscar"
'        Me.Text = "Buscar Funcionario"
'        Me.splitContenedor.Panel1.ResumeLayout(False)
'        Me.splitContenedor.Panel2.ResumeLayout(False)
'        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).EndInit()
'        Me.splitContenedor.ResumeLayout(False)
'        Me.PanelBusquedaLista.ResumeLayout(False)
'        Me.PanelBusquedaLista.PerformLayout()
'        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).EndInit()
'        Me.panelDetalle.ResumeLayout(False)
'        Me.panelDetalle.PerformLayout()
'        Me.flpAcciones.ResumeLayout(False)
'        CType(Me.pbCopyNombre, System.ComponentModel.ISupportInitialize).EndInit()
'        CType(Me.pbCopyCI, System.ComponentModel.ISupportInitialize).EndInit()
'        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).EndInit()
'        Me.ResumeLayout(False)

'    End Sub

'    Friend WithEvents splitContenedor As System.Windows.Forms.SplitContainer
'    Friend WithEvents dgvResultados As System.Windows.Forms.DataGridView
'    Friend WithEvents panelDetalle As System.Windows.Forms.Panel
'    Friend WithEvents pbFotoDetalle As System.Windows.Forms.PictureBox
'    Friend WithEvents lblCI As System.Windows.Forms.Label
'    Friend WithEvents lblNombreCompleto As System.Windows.Forms.Label
'    Friend WithEvents lblCargoHeader As System.Windows.Forms.Label
'    Friend WithEvents lblCargo As System.Windows.Forms.Label
'    Friend WithEvents lblTipoHeader As System.Windows.Forms.Label
'    Friend WithEvents lblTipo As System.Windows.Forms.Label
'    Friend WithEvents lblFechaIngresoHeader As System.Windows.Forms.Label
'    Friend WithEvents lblFechaIngreso As System.Windows.Forms.Label
'    Friend WithEvents lblPresencia As System.Windows.Forms.Label
'    Friend WithEvents lblPresenciaHeader As System.Windows.Forms.Label
'    Friend WithEvents lblHorarioCompleto As Label
'    Friend WithEvents lblHorarioCompletoHeader As Label
'    Friend WithEvents lblEstadoActividad As Label
'    Friend WithEvents btnGenerarFicha As Button
'    Friend WithEvents btnVerSituacion As Button
'    Friend WithEvents pbCopyCI As PictureBox
'    Friend WithEvents pbCopyNombre As PictureBox
'    Friend WithEvents ToolTip1 As ToolTip
'    Friend WithEvents btnNotificar As Button
'    Friend WithEvents btnSancionar As Button
'    Friend WithEvents btnNovedades As Button
'    Friend WithEvents PanelBusquedaLista As Panel
'    Friend WithEvents txtBusqueda As TextBox
'    Friend WithEvents lblBuscar As Label
'    Friend WithEvents flpAcciones As FlowLayoutPanel
'End Class
