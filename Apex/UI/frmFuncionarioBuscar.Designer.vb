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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.splitContenedor = New System.Windows.Forms.SplitContainer()
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.PanelBusquedaLista = New System.Windows.Forms.Panel()
        Me.tlpBusqueda = New System.Windows.Forms.TableLayoutPanel()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.panelDetalle = New System.Windows.Forms.Panel()
        Me.tlpDetalleVertical = New System.Windows.Forms.TableLayoutPanel()
        Me.pbFotoDetalle = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btnNovedades = New System.Windows.Forms.Button()
        Me.btnSancionar = New System.Windows.Forms.Button()
        Me.btnNotificar = New System.Windows.Forms.Button()
        Me.btnGenerarFicha = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblNombreCompleto = New System.Windows.Forms.Label()
        Me.pbCopyNombre = New System.Windows.Forms.PictureBox()
        Me.lblCI = New System.Windows.Forms.Label()
        Me.pbCopyCI = New System.Windows.Forms.PictureBox()
        Me.lblCargo = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.lblFechaIngreso = New System.Windows.Forms.Label()
        Me.lblHorarioCompleto = New System.Windows.Forms.Label()
        Me.lblPresencia = New System.Windows.Forms.Label()
        Me.lblEstadoActividad = New System.Windows.Forms.Label()
        Me.btnVerSituacion = New System.Windows.Forms.Button()
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedor.Panel1.SuspendLayout()
        Me.splitContenedor.Panel2.SuspendLayout()
        Me.splitContenedor.SuspendLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelBusquedaLista.SuspendLayout()
        Me.tlpBusqueda.SuspendLayout()
        Me.panelDetalle.SuspendLayout()
        Me.tlpDetalleVertical.SuspendLayout()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.pbCopyNombre, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCopyCI, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'splitContenedor
        '
        Me.splitContenedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContenedor.Location = New System.Drawing.Point(0, 0)
        Me.splitContenedor.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.splitContenedor.Name = "splitContenedor"
        '
        'splitContenedor.Panel1
        '
        Me.splitContenedor.Panel1.Controls.Add(Me.dgvResultados)
        Me.splitContenedor.Panel1.Controls.Add(Me.PanelBusquedaLista)
        '
        'splitContenedor.Panel2
        '
        Me.splitContenedor.Panel2.Controls.Add(Me.panelDetalle)
        Me.splitContenedor.Size = New System.Drawing.Size(1467, 804)
        Me.splitContenedor.SplitterDistance = 361
        Me.splitContenedor.SplitterWidth = 6
        Me.splitContenedor.TabIndex = 1
        '
        'dgvResultados
        '
        Me.dgvResultados.AllowUserToAddRows = False
        Me.dgvResultados.AllowUserToDeleteRows = False
        Me.dgvResultados.AllowUserToResizeColumns = False
        Me.dgvResultados.AllowUserToResizeRows = False
        Me.dgvResultados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvResultados.BackgroundColor = System.Drawing.Color.White
        Me.dgvResultados.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvResultados.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvResultados.ColumnHeadersHeight = 32
        Me.dgvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvResultados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvResultados.EnableHeadersVisualStyles = False
        Me.dgvResultados.GridColor = System.Drawing.Color.Silver
        Me.dgvResultados.Location = New System.Drawing.Point(0, 106)
        Me.dgvResultados.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvResultados.MultiSelect = False
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.ReadOnly = True
        Me.dgvResultados.RowHeadersVisible = False
        Me.dgvResultados.RowHeadersWidth = 62
        Me.dgvResultados.RowTemplate.Height = 28
        Me.dgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResultados.Size = New System.Drawing.Size(361, 698)
        Me.dgvResultados.TabIndex = 0
        '
        'PanelBusquedaLista
        '
        Me.PanelBusquedaLista.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelBusquedaLista.Controls.Add(Me.tlpBusqueda)
        Me.PanelBusquedaLista.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaLista.Location = New System.Drawing.Point(0, 0)
        Me.PanelBusquedaLista.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelBusquedaLista.Name = "PanelBusquedaLista"
        Me.PanelBusquedaLista.Padding = New System.Windows.Forms.Padding(15)
        Me.PanelBusquedaLista.Size = New System.Drawing.Size(361, 106)
        Me.PanelBusquedaLista.TabIndex = 1
        '
        'tlpBusqueda
        '
        Me.tlpBusqueda.ColumnCount = 2
        Me.tlpBusqueda.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBusqueda.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBusqueda.Controls.Add(Me.lblBuscar, 0, 0)
        Me.tlpBusqueda.Controls.Add(Me.txtBusqueda, 1, 0)
        Me.tlpBusqueda.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpBusqueda.Location = New System.Drawing.Point(15, 15)
        Me.tlpBusqueda.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tlpBusqueda.Name = "tlpBusqueda"
        Me.tlpBusqueda.RowCount = 1
        Me.tlpBusqueda.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBusqueda.Size = New System.Drawing.Size(331, 76)
        Me.tlpBusqueda.TabIndex = 0
        '
        'lblBuscar
        '
        Me.lblBuscar.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblBuscar.Location = New System.Drawing.Point(4, 24)
        Me.lblBuscar.Margin = New System.Windows.Forms.Padding(4, 0, 12, 0)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(72, 28)
        Me.lblBuscar.TabIndex = 0
        Me.lblBuscar.Text = "Buscar:"
        '
        'txtBusqueda
        '
        Me.txtBusqueda.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBusqueda.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBusqueda.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.txtBusqueda.Location = New System.Drawing.Point(92, 21)
        Me.txtBusqueda.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(235, 33)
        Me.txtBusqueda.TabIndex = 1
        '
        'panelDetalle
        '
        Me.panelDetalle.BackColor = System.Drawing.Color.White
        Me.panelDetalle.Controls.Add(Me.tlpDetalleVertical)
        Me.panelDetalle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelDetalle.Location = New System.Drawing.Point(0, 0)
        Me.panelDetalle.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.panelDetalle.Name = "panelDetalle"
        Me.panelDetalle.Padding = New System.Windows.Forms.Padding(15)
        Me.panelDetalle.Size = New System.Drawing.Size(1100, 804)
        Me.panelDetalle.TabIndex = 0
        '
        'tlpDetalleVertical
        '
        Me.tlpDetalleVertical.ColumnCount = 2
        Me.tlpDetalleVertical.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpDetalleVertical.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpDetalleVertical.Controls.Add(Me.lblNombreCompleto, 0, 1)
        Me.tlpDetalleVertical.Controls.Add(Me.pbFotoDetalle, 0, 0)
        Me.tlpDetalleVertical.Controls.Add(Me.lblEstadoActividad, 1, 4)
        Me.tlpDetalleVertical.Controls.Add(Me.FlowLayoutPanel1, 1, 0)
        Me.tlpDetalleVertical.Controls.Add(Me.lblFechaIngreso, 0, 6)
        Me.tlpDetalleVertical.Controls.Add(Me.lblPresencia, 0, 4)
        Me.tlpDetalleVertical.Controls.Add(Me.lblTipo, 0, 3)
        Me.tlpDetalleVertical.Controls.Add(Me.lblCI, 0, 2)
        Me.tlpDetalleVertical.Controls.Add(Me.lblHorarioCompleto, 0, 7)
        Me.tlpDetalleVertical.Controls.Add(Me.lblCargo, 0, 8)
        Me.tlpDetalleVertical.Controls.Add(Me.pbCopyNombre, 1, 2)
        Me.tlpDetalleVertical.Controls.Add(Me.pbCopyCI, 1, 1)
        Me.tlpDetalleVertical.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpDetalleVertical.Location = New System.Drawing.Point(15, 15)
        Me.tlpDetalleVertical.Name = "tlpDetalleVertical"
        Me.tlpDetalleVertical.RowCount = 10
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8.0!))
        Me.tlpDetalleVertical.Size = New System.Drawing.Size(1070, 774)
        Me.tlpDetalleVertical.TabIndex = 0
        '
        'pbFotoDetalle
        '
        Me.pbFotoDetalle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbFotoDetalle.Location = New System.Drawing.Point(4, 5)
        Me.pbFotoDetalle.Margin = New System.Windows.Forms.Padding(4, 5, 4, 20)
        Me.pbFotoDetalle.Name = "pbFotoDetalle"
        Me.pbFotoDetalle.Size = New System.Drawing.Size(638, 518)
        Me.pbFotoDetalle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFotoDetalle.TabIndex = 0
        Me.pbFotoDetalle.TabStop = False
        '
        'btnNovedades
        '
        Me.btnNovedades.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNovedades.Location = New System.Drawing.Point(244, 5)
        Me.btnNovedades.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNovedades.Name = "btnNovedades"
        Me.btnNovedades.Size = New System.Drawing.Size(112, 48)
        Me.btnNovedades.TabIndex = 2
        Me.btnNovedades.Text = "Novedades"
        Me.btnNovedades.UseVisualStyleBackColor = True
        '
        'btnSancionar
        '
        Me.btnSancionar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnSancionar.Location = New System.Drawing.Point(124, 5)
        Me.btnSancionar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnSancionar.Name = "btnSancionar"
        Me.btnSancionar.Size = New System.Drawing.Size(112, 48)
        Me.btnSancionar.TabIndex = 1
        Me.btnSancionar.Text = "Sancionar"
        Me.btnSancionar.UseVisualStyleBackColor = True
        '
        'btnNotificar
        '
        Me.btnNotificar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNotificar.Location = New System.Drawing.Point(4, 63)
        Me.btnNotificar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNotificar.Name = "btnNotificar"
        Me.btnNotificar.Size = New System.Drawing.Size(112, 48)
        Me.btnNotificar.TabIndex = 0
        Me.btnNotificar.Text = "Notificar"
        Me.btnNotificar.UseVisualStyleBackColor = True
        '
        'btnGenerarFicha
        '
        Me.btnGenerarFicha.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGenerarFicha.Location = New System.Drawing.Point(4, 5)
        Me.btnGenerarFicha.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGenerarFicha.Name = "btnGenerarFicha"
        Me.btnGenerarFicha.Size = New System.Drawing.Size(112, 48)
        Me.btnGenerarFicha.TabIndex = 16
        Me.btnGenerarFicha.Text = "Ficha"
        Me.btnGenerarFicha.UseVisualStyleBackColor = True
        Me.btnGenerarFicha.Visible = False
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnGenerarFicha)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSancionar)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNovedades)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNotificar)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnVerSituacion)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(649, 3)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(418, 537)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'lblNombreCompleto
        '
        Me.lblNombreCompleto.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNombreCompleto.AutoSize = True
        Me.lblNombreCompleto.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold)
        Me.lblNombreCompleto.Location = New System.Drawing.Point(4, 543)
        Me.lblNombreCompleto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNombreCompleto.Name = "lblNombreCompleto"
        Me.lblNombreCompleto.Size = New System.Drawing.Size(638, 40)
        Me.lblNombreCompleto.TabIndex = 2
        Me.lblNombreCompleto.Text = "Nombre Funcionario"
        '
        'pbCopyNombre
        '
        Me.pbCopyNombre.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.pbCopyNombre.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCopyNombre.Image = Global.Apex.My.Resources.Resources.copy_icon
        Me.pbCopyNombre.Location = New System.Drawing.Point(650, 588)
        Me.pbCopyNombre.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pbCopyNombre.Name = "pbCopyNombre"
        Me.pbCopyNombre.Size = New System.Drawing.Size(33, 29)
        Me.pbCopyNombre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbCopyNombre.TabIndex = 19
        Me.pbCopyNombre.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCopyNombre, "Copiar Nombre")
        Me.pbCopyNombre.Visible = False
        '
        'lblCI
        '
        Me.lblCI.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCI.AutoSize = True
        Me.lblCI.Font = New System.Drawing.Font("Segoe UI", 11.25!)
        Me.lblCI.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblCI.Location = New System.Drawing.Point(4, 587)
        Me.lblCI.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCI.Name = "lblCI"
        Me.lblCI.Size = New System.Drawing.Size(638, 31)
        Me.lblCI.TabIndex = 1
        Me.lblCI.Text = "CI: -"
        '
        'pbCopyCI
        '
        Me.pbCopyCI.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.pbCopyCI.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCopyCI.Image = Global.Apex.My.Resources.Resources.copy_icon
        Me.pbCopyCI.Location = New System.Drawing.Point(650, 548)
        Me.pbCopyCI.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pbCopyCI.Name = "pbCopyCI"
        Me.pbCopyCI.Size = New System.Drawing.Size(33, 29)
        Me.pbCopyCI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbCopyCI.TabIndex = 18
        Me.pbCopyCI.TabStop = False
        Me.ToolTip1.SetToolTip(Me.pbCopyCI, "Copiar CI")
        Me.pbCopyCI.Visible = False
        '
        'lblCargo
        '
        Me.lblCargo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCargo.AutoSize = True
        Me.lblCargo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCargo.ForeColor = System.Drawing.Color.DimGray
        Me.lblCargo.Location = New System.Drawing.Point(4, 745)
        Me.lblCargo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 2)
        Me.lblCargo.Name = "lblCargo"
        Me.lblCargo.Size = New System.Drawing.Size(638, 28)
        Me.lblCargo.TabIndex = 4
        Me.lblCargo.Text = "Cargo: -"
        '
        'lblTipo
        '
        Me.lblTipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblTipo.ForeColor = System.Drawing.Color.DimGray
        Me.lblTipo.Location = New System.Drawing.Point(4, 622)
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 2)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(638, 28)
        Me.lblTipo.TabIndex = 6
        Me.lblTipo.Text = "Tipo: -"
        '
        'lblFechaIngreso
        '
        Me.lblFechaIngreso.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFechaIngreso.AutoSize = True
        Me.lblFechaIngreso.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblFechaIngreso.ForeColor = System.Drawing.Color.DimGray
        Me.lblFechaIngreso.Location = New System.Drawing.Point(4, 685)
        Me.lblFechaIngreso.Margin = New System.Windows.Forms.Padding(4, 0, 4, 2)
        Me.lblFechaIngreso.Name = "lblFechaIngreso"
        Me.lblFechaIngreso.Size = New System.Drawing.Size(638, 28)
        Me.lblFechaIngreso.TabIndex = 8
        Me.lblFechaIngreso.Text = "Fecha Ingreso: -"
        '
        'lblHorarioCompleto
        '
        Me.lblHorarioCompleto.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblHorarioCompleto.AutoSize = True
        Me.lblHorarioCompleto.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblHorarioCompleto.ForeColor = System.Drawing.Color.DimGray
        Me.lblHorarioCompleto.Location = New System.Drawing.Point(4, 715)
        Me.lblHorarioCompleto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 2)
        Me.lblHorarioCompleto.Name = "lblHorarioCompleto"
        Me.lblHorarioCompleto.Size = New System.Drawing.Size(638, 28)
        Me.lblHorarioCompleto.TabIndex = 13
        Me.lblHorarioCompleto.Text = "Horario: -"
        '
        'lblPresencia
        '
        Me.lblPresencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPresencia.AutoSize = True
        Me.lblPresencia.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblPresencia.ForeColor = System.Drawing.Color.DimGray
        Me.lblPresencia.Location = New System.Drawing.Point(4, 653)
        Me.lblPresencia.Margin = New System.Windows.Forms.Padding(4, 0, 4, 2)
        Me.lblPresencia.Name = "lblPresencia"
        Me.lblPresencia.Size = New System.Drawing.Size(638, 28)
        Me.lblPresencia.TabIndex = 10
        Me.lblPresencia.Text = "-"
        '
        'lblEstadoActividad
        '
        Me.lblEstadoActividad.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEstadoActividad.AutoSize = True
        Me.lblEstadoActividad.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblEstadoActividad.Location = New System.Drawing.Point(650, 657)
        Me.lblEstadoActividad.Margin = New System.Windows.Forms.Padding(4, 5, 4, 0)
        Me.lblEstadoActividad.Name = "lblEstadoActividad"
        Me.lblEstadoActividad.Size = New System.Drawing.Size(416, 28)
        Me.lblEstadoActividad.TabIndex = 14
        Me.lblEstadoActividad.Text = "Estado: -"
        '
        'btnVerSituacion
        '
        Me.btnVerSituacion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerSituacion.Location = New System.Drawing.Point(124, 63)
        Me.btnVerSituacion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnVerSituacion.Name = "btnVerSituacion"
        Me.btnVerSituacion.Size = New System.Drawing.Size(112, 48)
        Me.btnVerSituacion.TabIndex = 17
        Me.btnVerSituacion.Text = "Situación"
        Me.btnVerSituacion.UseVisualStyleBackColor = True
        Me.btnVerSituacion.Visible = False
        '
        'frmFuncionarioBuscar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1467, 804)
        Me.Controls.Add(Me.splitContenedor)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(1489, 770)
        Me.Name = "frmFuncionarioBuscar"
        Me.Text = "Buscar Funcionario"
        Me.splitContenedor.Panel1.ResumeLayout(False)
        Me.splitContenedor.Panel2.ResumeLayout(False)
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedor.ResumeLayout(False)
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelBusquedaLista.ResumeLayout(False)
        Me.tlpBusqueda.ResumeLayout(False)
        Me.tlpBusqueda.PerformLayout()
        Me.panelDetalle.ResumeLayout(False)
        Me.tlpDetalleVertical.ResumeLayout(False)
        Me.tlpDetalleVertical.PerformLayout()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        CType(Me.pbCopyNombre, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCopyCI, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents splitContenedor As SplitContainer
    Friend WithEvents dgvResultados As DataGridView
    Friend WithEvents panelDetalle As Panel
    Friend WithEvents pbFotoDetalle As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents PanelBusquedaLista As Panel
    Friend WithEvents txtBusqueda As TextBox
    Friend WithEvents lblBuscar As Label
    '
    ' Contenedores nuevos o refactorizados
    '
    Friend WithEvents tlpBusqueda As TableLayoutPanel
    Friend WithEvents tlpDetalleVertical As TableLayoutPanel
    Friend WithEvents btnGenerarFicha As Button
    Friend WithEvents btnNotificar As Button
    Friend WithEvents btnSancionar As Button
    Friend WithEvents btnNovedades As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents lblNombreCompleto As Label
    Friend WithEvents lblPresencia As Label
    Friend WithEvents lblHorarioCompleto As Label
    Friend WithEvents lblTipo As Label
    Friend WithEvents pbCopyNombre As PictureBox
    Friend WithEvents pbCopyCI As PictureBox
    Friend WithEvents lblEstadoActividad As Label
    Friend WithEvents btnVerSituacion As Button
    Friend WithEvents lblFechaIngreso As Label
    Friend WithEvents lblCI As Label
    Friend WithEvents lblCargo As Label
End Class