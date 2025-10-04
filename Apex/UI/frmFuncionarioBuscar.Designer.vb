<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioBuscar
    Inherits FormActualizable
    'Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.splitContenedor = New System.Windows.Forms.SplitContainer()
        Me.tlpResultados = New System.Windows.Forms.TableLayoutPanel()
        Me.dgvFuncionarios = New System.Windows.Forms.DataGridView()
        Me.lstCargos = New System.Windows.Forms.ListBox()
        Me.PanelBusquedaLista = New System.Windows.Forms.Panel()
        Me.tlpBusqueda = New System.Windows.Forms.TableLayoutPanel()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.btnCopiarContenido = New System.Windows.Forms.Button()
        Me.panelDetalle = New System.Windows.Forms.Panel()
        Me.tlpDetalleVertical = New System.Windows.Forms.TableLayoutPanel()
        Me.flpDetalles = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblNombreCompleto = New System.Windows.Forms.Label()
        Me.flpCIDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblCITitulo = New System.Windows.Forms.Label()
        Me.lblCIValor = New System.Windows.Forms.Label()
        Me.flpTipoDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblTipoTitulo = New System.Windows.Forms.Label()
        Me.lblTipoValor = New System.Windows.Forms.Label()
        Me.flpFechaIngresoDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblFechaIngresoTitulo = New System.Windows.Forms.Label()
        Me.lblFechaIngresoValor = New System.Windows.Forms.Label()
        Me.flpHorarioDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblHorarioTitulo = New System.Windows.Forms.Label()
        Me.lblSemanaValor = New System.Windows.Forms.Label()
        Me.lblTurnoTitulo = New System.Windows.Forms.Label()
        Me.lblTurnoValor = New System.Windows.Forms.Label()
        Me.lblPlantillaTitulo = New System.Windows.Forms.Label()
        Me.lblPlantillaValor = New System.Windows.Forms.Label()
        Me.flpUbicacionDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblUbicacionTitulo = New System.Windows.Forms.Label()
        Me.lblUnidadValor = New System.Windows.Forms.Label()
        Me.lblPuestoTitulo = New System.Windows.Forms.Label()
        Me.lblPuestoValor = New System.Windows.Forms.Label()
        Me.flpSubDireccionDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblSubDireccionTitulo = New System.Windows.Forms.Label()
        Me.lblSubDireccionValor = New System.Windows.Forms.Label()
        Me.flpCargoDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblEscalafonTitulo = New System.Windows.Forms.Label()
        Me.lblEscalafonValor = New System.Windows.Forms.Label()
        Me.lblSubEscalafonTitulo = New System.Windows.Forms.Label()
        Me.lblSubEscalafonValor = New System.Windows.Forms.Label()
        Me.lblJerarquiaTitulo = New System.Windows.Forms.Label()
        Me.lblJerarquiaValor = New System.Windows.Forms.Label()
        Me.flpPresenciaDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblPresenciaTitulo = New System.Windows.Forms.Label()
        Me.lblPresenciaValor = New System.Windows.Forms.Label()
        Me.flpEstadoDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblEstadoTitulo = New System.Windows.Forms.Label()
        Me.lblEstadoValor = New System.Windows.Forms.Label()
        Me.btnVerSituacion = New System.Windows.Forms.Button()
        Me.tlpAcciones = New System.Windows.Forms.TableLayoutPanel()
        Me.btnGenerarFicha = New System.Windows.Forms.Button()
        Me.btnSancionar = New System.Windows.Forms.Button()
        Me.btnNovedades = New System.Windows.Forms.Button()
        Me.btnNotificar = New System.Windows.Forms.Button()
        Me.pbFotoDetalle = New System.Windows.Forms.PictureBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedor.Panel1.SuspendLayout()
        Me.splitContenedor.Panel2.SuspendLayout()
        Me.splitContenedor.SuspendLayout()
        Me.tlpResultados.SuspendLayout()
        CType(Me.dgvFuncionarios, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelBusquedaLista.SuspendLayout()
        Me.tlpBusqueda.SuspendLayout()
        Me.panelDetalle.SuspendLayout()
        Me.tlpDetalleVertical.SuspendLayout()
        Me.flpDetalles.SuspendLayout()
        Me.flpCIDetalle.SuspendLayout()
        Me.flpTipoDetalle.SuspendLayout()
        Me.flpFechaIngresoDetalle.SuspendLayout()
        Me.flpHorarioDetalle.SuspendLayout()
        Me.flpUbicacionDetalle.SuspendLayout()
        Me.flpSubDireccionDetalle.SuspendLayout()
        Me.flpCargoDetalle.SuspendLayout()
        Me.flpPresenciaDetalle.SuspendLayout()
        Me.flpEstadoDetalle.SuspendLayout()
        Me.tlpAcciones.SuspendLayout()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'splitContenedor
        '
        Me.splitContenedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContenedor.Location = New System.Drawing.Point(0, 0)
        Me.splitContenedor.Name = "splitContenedor"
        '
        'splitContenedor.Panel1
        '
        Me.splitContenedor.Panel1.Controls.Add(Me.tlpResultados)
        Me.splitContenedor.Panel1.Controls.Add(Me.PanelBusquedaLista)
        Me.splitContenedor.Panel1MinSize = 320
        '
        'splitContenedor.Panel2
        '
        Me.splitContenedor.Panel2.Controls.Add(Me.panelDetalle)
        Me.splitContenedor.Panel2MinSize = 400
        Me.splitContenedor.Size = New System.Drawing.Size(944, 699)
        Me.splitContenedor.SplitterDistance = 539
        Me.splitContenedor.TabIndex = 0
        '
        'tlpResultados
        '
        Me.tlpResultados.ColumnCount = 2
        Me.tlpResultados.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpResultados.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102.0!))
        Me.tlpResultados.Controls.Add(Me.dgvFuncionarios, 0, 0)
        Me.tlpResultados.Controls.Add(Me.lstCargos, 1, 0)
        Me.tlpResultados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpResultados.Location = New System.Drawing.Point(0, 115)
        Me.tlpResultados.Name = "tlpResultados"
        Me.tlpResultados.RowCount = 1
        Me.tlpResultados.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpResultados.Size = New System.Drawing.Size(539, 584)
        Me.tlpResultados.TabIndex = 1
        '
        'dgvFuncionarios
        '
        Me.dgvFuncionarios.AllowUserToAddRows = False
        Me.dgvFuncionarios.AllowUserToDeleteRows = False
        Me.dgvFuncionarios.AllowUserToResizeColumns = False
        Me.dgvFuncionarios.AllowUserToResizeRows = False
        Me.dgvFuncionarios.ColumnHeadersHeight = 34
        Me.dgvFuncionarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvFuncionarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvFuncionarios.Location = New System.Drawing.Point(3, 3)
        Me.dgvFuncionarios.Name = "dgvFuncionarios"
        Me.dgvFuncionarios.ReadOnly = True
        Me.dgvFuncionarios.RowHeadersWidth = 62
        Me.dgvFuncionarios.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvFuncionarios.Size = New System.Drawing.Size(431, 578)
        Me.dgvFuncionarios.TabIndex = 1
        '
        'lstCargos
        '
        Me.lstCargos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstCargos.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lstCargos.FormattingEnabled = True
        Me.lstCargos.IntegralHeight = False
        Me.lstCargos.ItemHeight = 25
        Me.lstCargos.Location = New System.Drawing.Point(440, 3)
        Me.lstCargos.Name = "lstCargos"
        Me.lstCargos.Size = New System.Drawing.Size(96, 578)
        Me.lstCargos.TabIndex = 2
        '
        'PanelBusquedaLista
        '
        Me.PanelBusquedaLista.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelBusquedaLista.Controls.Add(Me.tlpBusqueda)
        Me.PanelBusquedaLista.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaLista.Location = New System.Drawing.Point(0, 0)
        Me.PanelBusquedaLista.Name = "PanelBusquedaLista"
        Me.PanelBusquedaLista.Padding = New System.Windows.Forms.Padding(12)
        Me.PanelBusquedaLista.Size = New System.Drawing.Size(539, 115)
        Me.PanelBusquedaLista.TabIndex = 0
        '
        'tlpBusqueda
        '
        Me.tlpBusqueda.ColumnCount = 2
        Me.tlpBusqueda.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBusqueda.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBusqueda.Controls.Add(Me.lblBuscar, 0, 0)
        Me.tlpBusqueda.Controls.Add(Me.txtBusqueda, 1, 0)
        Me.tlpBusqueda.Controls.Add(Me.btnCopiarContenido, 1, 1)
        Me.tlpBusqueda.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpBusqueda.Location = New System.Drawing.Point(12, 12)
        Me.tlpBusqueda.Name = "tlpBusqueda"
        Me.tlpBusqueda.RowCount = 2
        Me.tlpBusqueda.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBusqueda.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44.0!))
        Me.tlpBusqueda.Size = New System.Drawing.Size(515, 91)
        Me.tlpBusqueda.TabIndex = 0
        '
        'lblBuscar
        '
        Me.lblBuscar.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblBuscar.Location = New System.Drawing.Point(3, 11)
        Me.lblBuscar.Margin = New System.Windows.Forms.Padding(3, 0, 8, 0)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(72, 25)
        Me.lblBuscar.TabIndex = 0
        Me.lblBuscar.Text = "Buscar:"
        '
        'txtBusqueda
        '
        Me.txtBusqueda.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBusqueda.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBusqueda.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.txtBusqueda.Location = New System.Drawing.Point(86, 7)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(426, 33)
        Me.txtBusqueda.TabIndex = 1
        '
        'btnCopiarContenido
        '
        Me.btnCopiarContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCopiarContenido.Location = New System.Drawing.Point(86, 50)
        Me.btnCopiarContenido.Name = "btnCopiarContenido"
        Me.btnCopiarContenido.Size = New System.Drawing.Size(426, 38)
        Me.btnCopiarContenido.TabIndex = 3
        Me.btnCopiarContenido.Text = "Copiar selección"
        Me.btnCopiarContenido.UseVisualStyleBackColor = True
        Me.btnCopiarContenido.Visible = False
        '
        'panelDetalle
        '
        Me.panelDetalle.BackColor = System.Drawing.Color.White
        Me.panelDetalle.Controls.Add(Me.tlpDetalleVertical)
        Me.panelDetalle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelDetalle.Location = New System.Drawing.Point(0, 0)
        Me.panelDetalle.Name = "panelDetalle"
        Me.panelDetalle.Padding = New System.Windows.Forms.Padding(12)
        Me.panelDetalle.Size = New System.Drawing.Size(401, 699)
        Me.panelDetalle.TabIndex = 0
        '
        'tlpDetalleVertical
        '
        Me.tlpDetalleVertical.ColumnCount = 1
        Me.tlpDetalleVertical.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpDetalleVertical.Controls.Add(Me.flpDetalles, 0, 3)
        Me.tlpDetalleVertical.Controls.Add(Me.btnVerSituacion, 0, 2)
        Me.tlpDetalleVertical.Controls.Add(Me.tlpAcciones, 0, 0)
        Me.tlpDetalleVertical.Controls.Add(Me.pbFotoDetalle, 0, 1)
        Me.tlpDetalleVertical.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpDetalleVertical.Location = New System.Drawing.Point(12, 12)
        Me.tlpDetalleVertical.Name = "tlpDetalleVertical"
        Me.tlpDetalleVertical.RowCount = 4
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.Size = New System.Drawing.Size(377, 675)
        Me.tlpDetalleVertical.TabIndex = 1
        '
        'flpDetalles
        '
        Me.flpDetalles.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.flpDetalles.AutoScroll = True
        Me.flpDetalles.Controls.Add(Me.lblNombreCompleto)
        Me.flpDetalles.Controls.Add(Me.flpCIDetalle)
        Me.flpDetalles.Controls.Add(Me.flpTipoDetalle)
        Me.flpDetalles.Controls.Add(Me.flpFechaIngresoDetalle)
        Me.flpDetalles.Controls.Add(Me.flpHorarioDetalle)
        Me.flpDetalles.Controls.Add(Me.flpUbicacionDetalle)
        Me.flpDetalles.Controls.Add(Me.flpSubDireccionDetalle)
        Me.flpDetalles.Controls.Add(Me.flpCargoDetalle)
        Me.flpDetalles.Controls.Add(Me.flpPresenciaDetalle)
        Me.flpDetalles.Controls.Add(Me.flpEstadoDetalle)
        Me.flpDetalles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpDetalles.Location = New System.Drawing.Point(3, 402)
        Me.flpDetalles.Name = "flpDetalles"
        Me.flpDetalles.Size = New System.Drawing.Size(371, 270)
        Me.flpDetalles.TabIndex = 22
        '
        'lblNombreCompleto
        '
        Me.lblNombreCompleto.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblNombreCompleto.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblNombreCompleto.Location = New System.Drawing.Point(3, 8)
        Me.lblNombreCompleto.Margin = New System.Windows.Forms.Padding(3, 8, 3, 8)
        Me.lblNombreCompleto.Name = "lblNombreCompleto"
        Me.lblNombreCompleto.Size = New System.Drawing.Size(624, 38)
        Me.lblNombreCompleto.TabIndex = 2
        Me.lblNombreCompleto.Text = "Nombre Funcionario"
        Me.lblNombreCompleto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'flpCIDetalle

        Me.flpCIDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpCIDetalle.AutoSize = True
        Me.flpCIDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpCIDetalle.Controls.Add(Me.lblCITitulo)
        Me.flpCIDetalle.Controls.Add(Me.lblCIValor)
        Me.flpCIDetalle.Location = New System.Drawing.Point(3, 54)
        Me.flpCIDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpCIDetalle.Name = "flpCIDetalle"
        Me.flpCIDetalle.Size = New System.Drawing.Size(69, 25)
        Me.flpCIDetalle.TabIndex = 3
        '
        'lblCITitulo

        Me.lblCITitulo.AutoSize = True
        Me.lblCITitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblCITitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblCITitulo.Location = New System.Drawing.Point(3, 0)
        Me.lblCITitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblCITitulo.Name = "lblCITitulo"
        Me.lblCITitulo.Size = New System.Drawing.Size(36, 25)
        Me.lblCITitulo.TabIndex = 0
        Me.lblCITitulo.Text = "CI:"
        '
        'lblCIValor

        Me.lblCIValor.AutoSize = True
        Me.lblCIValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblCIValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblCIValor.Location = New System.Drawing.Point(42, 0)
        Me.lblCIValor.Margin = New System.Windows.Forms.Padding(0, 0, 12, 0)
        Me.lblCIValor.Name = "lblCIValor"
        Me.lblCIValor.Size = New System.Drawing.Size(20, 25)
        Me.lblCIValor.TabIndex = 1
        Me.lblCIValor.Text = "-"
        '
        'flpTipoDetalle

        Me.flpTipoDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpTipoDetalle.AutoSize = True
        Me.flpTipoDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpTipoDetalle.Controls.Add(Me.lblTipoTitulo)
        Me.flpTipoDetalle.Controls.Add(Me.lblTipoValor)
        Me.flpTipoDetalle.Location = New System.Drawing.Point(3, 81)
        Me.flpTipoDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpTipoDetalle.Name = "flpTipoDetalle"
        Me.flpTipoDetalle.Size = New System.Drawing.Size(78, 25)
        Me.flpTipoDetalle.TabIndex = 4
        '
        'lblTipoTitulo

        Me.lblTipoTitulo.AutoSize = True
        Me.lblTipoTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblTipoTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblTipoTitulo.Location = New System.Drawing.Point(0, 0)
        Me.lblTipoTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblTipoTitulo.Name = "lblTipoTitulo"
        Me.lblTipoTitulo.Size = New System.Drawing.Size(48, 25)
        Me.lblTipoTitulo.TabIndex = 0
        Me.lblTipoTitulo.Text = "Tipo:"
        '
        'lblTipoValor

        Me.lblTipoValor.AutoSize = True
        Me.lblTipoValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblTipoValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblTipoValor.Location = New System.Drawing.Point(51, 0)
        Me.lblTipoValor.Margin = New System.Windows.Forms.Padding(0, 0, 12, 0)
        Me.lblTipoValor.Name = "lblTipoValor"
        Me.lblTipoValor.Size = New System.Drawing.Size(20, 25)
        Me.lblTipoValor.TabIndex = 1
        Me.lblTipoValor.Text = "-"
        '
        'flpFechaIngresoDetalle

        Me.flpFechaIngresoDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpFechaIngresoDetalle.AutoSize = True
        Me.flpFechaIngresoDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpFechaIngresoDetalle.Controls.Add(Me.lblFechaIngresoTitulo)
        Me.flpFechaIngresoDetalle.Controls.Add(Me.lblFechaIngresoValor)
        Me.flpFechaIngresoDetalle.Location = New System.Drawing.Point(3, 108)
        Me.flpFechaIngresoDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 4)
        Me.flpFechaIngresoDetalle.Name = "flpFechaIngresoDetalle"
        Me.flpFechaIngresoDetalle.Size = New System.Drawing.Size(164, 25)
        Me.flpFechaIngresoDetalle.TabIndex = 5
        '
        'lblFechaIngresoTitulo

        Me.lblFechaIngresoTitulo.AutoSize = True
        Me.lblFechaIngresoTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblFechaIngresoTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblFechaIngresoTitulo.Location = New System.Drawing.Point(0, 0)
        Me.lblFechaIngresoTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblFechaIngresoTitulo.Name = "lblFechaIngresoTitulo"
        Me.lblFechaIngresoTitulo.Size = New System.Drawing.Size(133, 25)
        Me.lblFechaIngresoTitulo.TabIndex = 0
        Me.lblFechaIngresoTitulo.Text = "Fecha Ingreso:"
        '
        'lblFechaIngresoValor

        Me.lblFechaIngresoValor.AutoSize = True
        Me.lblFechaIngresoValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblFechaIngresoValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblFechaIngresoValor.Location = New System.Drawing.Point(136, 0)
        Me.lblFechaIngresoValor.Margin = New System.Windows.Forms.Padding(0, 0, 12, 0)
        Me.lblFechaIngresoValor.Name = "lblFechaIngresoValor"
        Me.lblFechaIngresoValor.Size = New System.Drawing.Size(20, 25)
        Me.lblFechaIngresoValor.TabIndex = 1
        Me.lblFechaIngresoValor.Text = "-"
        '
        'flpHorarioDetalle
        '
        Me.flpHorarioDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpHorarioDetalle.AutoSize = True
        Me.flpHorarioDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpHorarioDetalle.Controls.Add(Me.lblHorarioTitulo)
        Me.flpHorarioDetalle.Controls.Add(Me.lblSemanaValor)
        Me.flpHorarioDetalle.Controls.Add(Me.lblTurnoTitulo)
        Me.flpHorarioDetalle.Controls.Add(Me.lblTurnoValor)
        Me.flpHorarioDetalle.Controls.Add(Me.lblPlantillaTitulo)
        Me.flpHorarioDetalle.Controls.Add(Me.lblPlantillaValor)
        Me.flpHorarioDetalle.Location = New System.Drawing.Point(3, 144)
        Me.flpHorarioDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpHorarioDetalle.Name = "flpHorarioDetalle"
        Me.flpHorarioDetalle.Size = New System.Drawing.Size(353, 25)
        Me.flpHorarioDetalle.TabIndex = 13
        '
        'lblHorarioTitulo
        '
        Me.lblHorarioTitulo.AutoSize = True
        Me.lblHorarioTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblHorarioTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblHorarioTitulo.Location = New System.Drawing.Point(3, 0)
        Me.lblHorarioTitulo.Name = "lblHorarioTitulo"
        Me.lblHorarioTitulo.Size = New System.Drawing.Size(87, 25)
        Me.lblHorarioTitulo.TabIndex = 0
        Me.lblHorarioTitulo.Text = "Semana:"
        '
        'lblSemanaValor
        '
        Me.lblSemanaValor.AutoSize = True
        Me.lblSemanaValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblSemanaValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblSemanaValor.Location = New System.Drawing.Point(96, 0)
        Me.lblSemanaValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblSemanaValor.Name = "lblSemanaValor"
        Me.lblSemanaValor.Size = New System.Drawing.Size(20, 25)
        Me.lblSemanaValor.TabIndex = 1
        Me.lblSemanaValor.Text = "-"
        '
        'lblTurnoTitulo
        '
        Me.lblTurnoTitulo.AutoSize = True
        Me.lblTurnoTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblTurnoTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblTurnoTitulo.Location = New System.Drawing.Point(128, 0)
        Me.lblTurnoTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblTurnoTitulo.Name = "lblTurnoTitulo"
        Me.lblTurnoTitulo.Size = New System.Drawing.Size(71, 25)
        Me.lblTurnoTitulo.TabIndex = 2
        Me.lblTurnoTitulo.Text = "Turno:"
        '
        'lblTurnoValor
        '
        Me.lblTurnoValor.AutoSize = True
        Me.lblTurnoValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblTurnoValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblTurnoValor.Location = New System.Drawing.Point(205, 0)
        Me.lblTurnoValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblTurnoValor.Name = "lblTurnoValor"
        Me.lblTurnoValor.Size = New System.Drawing.Size(20, 25)
        Me.lblTurnoValor.TabIndex = 3
        Me.lblTurnoValor.Text = "-"
        '
        'lblPlantillaTitulo
        '
        Me.lblPlantillaTitulo.AutoSize = True
        Me.lblPlantillaTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblPlantillaTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblPlantillaTitulo.Location = New System.Drawing.Point(237, 0)
        Me.lblPlantillaTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblPlantillaTitulo.Name = "lblPlantillaTitulo"
        Me.lblPlantillaTitulo.Size = New System.Drawing.Size(87, 25)
        Me.lblPlantillaTitulo.TabIndex = 4
        Me.lblPlantillaTitulo.Text = "Horario:"
        '
        'lblPlantillaValor
        '
        Me.lblPlantillaValor.AutoSize = True
        Me.lblPlantillaValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblPlantillaValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblPlantillaValor.Location = New System.Drawing.Point(330, 0)
        Me.lblPlantillaValor.Name = "lblPlantillaValor"
        Me.lblPlantillaValor.Size = New System.Drawing.Size(20, 25)
        Me.lblPlantillaValor.TabIndex = 5
        Me.lblPlantillaValor.Text = "-"
        '
        'flpUbicacionDetalle
        '
        Me.flpUbicacionDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpUbicacionDetalle.AutoSize = True
        Me.flpUbicacionDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpUbicacionDetalle.Controls.Add(Me.lblUbicacionTitulo)
        Me.flpUbicacionDetalle.Controls.Add(Me.lblUnidadValor)
        Me.flpUbicacionDetalle.Controls.Add(Me.lblPuestoTitulo)
        Me.flpUbicacionDetalle.Controls.Add(Me.lblPuestoValor)
        Me.flpUbicacionDetalle.Location = New System.Drawing.Point(3, 171)
        Me.flpUbicacionDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpUbicacionDetalle.Name = "flpUbicacionDetalle"
        Me.flpUbicacionDetalle.Size = New System.Drawing.Size(253, 25)
        Me.flpUbicacionDetalle.TabIndex = 23
        '
        'lblUbicacionTitulo
        '
        Me.lblUbicacionTitulo.AutoSize = True
        Me.lblUbicacionTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblUbicacionTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblUbicacionTitulo.Location = New System.Drawing.Point(3, 0)
        Me.lblUbicacionTitulo.Name = "lblUbicacionTitulo"
        Me.lblUbicacionTitulo.Size = New System.Drawing.Size(105, 25)
        Me.lblUbicacionTitulo.TabIndex = 0
        Me.lblUbicacionTitulo.Text = "Ubicación:"
        '
        'lblUnidadValor
        '
        Me.lblUnidadValor.AutoSize = True
        Me.lblUnidadValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblUnidadValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblUnidadValor.Location = New System.Drawing.Point(114, 0)
        Me.lblUnidadValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblUnidadValor.Name = "lblUnidadValor"
        Me.lblUnidadValor.Size = New System.Drawing.Size(20, 25)
        Me.lblUnidadValor.TabIndex = 1
        Me.lblUnidadValor.Text = "-"
        '
        'lblPuestoTitulo
        '
        Me.lblPuestoTitulo.AutoSize = True
        Me.lblPuestoTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblPuestoTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblPuestoTitulo.Location = New System.Drawing.Point(146, 0)
        Me.lblPuestoTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblPuestoTitulo.Name = "lblPuestoTitulo"
        Me.lblPuestoTitulo.Size = New System.Drawing.Size(78, 25)
        Me.lblPuestoTitulo.TabIndex = 2
        Me.lblPuestoTitulo.Text = "Puesto:"
        '
        'lblPuestoValor
        '
        Me.lblPuestoValor.AutoSize = True
        Me.lblPuestoValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblPuestoValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblPuestoValor.Location = New System.Drawing.Point(230, 0)
        Me.lblPuestoValor.Name = "lblPuestoValor"
        Me.lblPuestoValor.Size = New System.Drawing.Size(20, 25)
        Me.lblPuestoValor.TabIndex = 3
        Me.lblPuestoValor.Text = "-"
        '
        'flpSubDireccionDetalle
        '
        Me.flpSubDireccionDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpSubDireccionDetalle.AutoSize = True
        Me.flpSubDireccionDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpSubDireccionDetalle.Controls.Add(Me.lblSubDireccionTitulo)
        Me.flpSubDireccionDetalle.Controls.Add(Me.lblSubDireccionValor)
        Me.flpSubDireccionDetalle.Location = New System.Drawing.Point(3, 198)
        Me.flpSubDireccionDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpSubDireccionDetalle.Name = "flpSubDireccionDetalle"
        Me.flpSubDireccionDetalle.Size = New System.Drawing.Size(165, 25)
        Me.flpSubDireccionDetalle.TabIndex = 24
        '
        'lblSubDireccionTitulo
        '
        Me.lblSubDireccionTitulo.AutoSize = True
        Me.lblSubDireccionTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblSubDireccionTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblSubDireccionTitulo.Location = New System.Drawing.Point(0, 0)
        Me.lblSubDireccionTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblSubDireccionTitulo.Name = "lblSubDireccionTitulo"
        Me.lblSubDireccionTitulo.Size = New System.Drawing.Size(136, 25)
        Me.lblSubDireccionTitulo.TabIndex = 0
        Me.lblSubDireccionTitulo.Text = "SubDireccion:"
        '
        'lblSubDireccionValor
        '
        Me.lblSubDireccionValor.AutoSize = True
        Me.lblSubDireccionValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblSubDireccionValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblSubDireccionValor.Location = New System.Drawing.Point(142, 0)
        Me.lblSubDireccionValor.Name = "lblSubDireccionValor"
        Me.lblSubDireccionValor.Size = New System.Drawing.Size(20, 25)
        Me.lblSubDireccionValor.TabIndex = 1
        Me.lblSubDireccionValor.Text = "-"
        '
        'flpCargoDetalle
        '
        Me.flpCargoDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpCargoDetalle.AutoSize = True
        Me.flpCargoDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpCargoDetalle.Controls.Add(Me.lblEscalafonTitulo)
        Me.flpCargoDetalle.Controls.Add(Me.lblEscalafonValor)
        Me.flpCargoDetalle.Controls.Add(Me.lblSubEscalafonTitulo)
        Me.flpCargoDetalle.Controls.Add(Me.lblSubEscalafonValor)
        Me.flpCargoDetalle.Controls.Add(Me.lblJerarquiaTitulo)
        Me.flpCargoDetalle.Controls.Add(Me.lblJerarquiaValor)
        Me.flpCargoDetalle.Location = New System.Drawing.Point(633, 0)
        Me.flpCargoDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpCargoDetalle.Name = "flpCargoDetalle"
        Me.flpCargoDetalle.Size = New System.Drawing.Size(138, 125)
        Me.flpCargoDetalle.TabIndex = 4
        '
        'lblEscalafonTitulo
        '
        Me.lblEscalafonTitulo.AutoSize = True
        Me.lblEscalafonTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblEscalafonTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblEscalafonTitulo.Location = New System.Drawing.Point(3, 0)
        Me.lblEscalafonTitulo.Name = "lblEscalafonTitulo"
        Me.lblEscalafonTitulo.Size = New System.Drawing.Size(100, 25)
        Me.lblEscalafonTitulo.TabIndex = 0
        Me.lblEscalafonTitulo.Text = "Escalafón:"
        '
        'lblEscalafonValor
        '
        Me.lblEscalafonValor.AutoSize = True
        Me.lblEscalafonValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblEscalafonValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblEscalafonValor.Location = New System.Drawing.Point(3, 25)
        Me.lblEscalafonValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblEscalafonValor.Name = "lblEscalafonValor"
        Me.lblEscalafonValor.Size = New System.Drawing.Size(20, 25)
        Me.lblEscalafonValor.TabIndex = 1
        Me.lblEscalafonValor.Text = "-"
        '
        'lblSubEscalafonTitulo
        '
        Me.lblSubEscalafonTitulo.AutoSize = True
        Me.lblSubEscalafonTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblSubEscalafonTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblSubEscalafonTitulo.Location = New System.Drawing.Point(0, 50)
        Me.lblSubEscalafonTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblSubEscalafonTitulo.Name = "lblSubEscalafonTitulo"
        Me.lblSubEscalafonTitulo.Size = New System.Drawing.Size(135, 25)
        Me.lblSubEscalafonTitulo.TabIndex = 2
        Me.lblSubEscalafonTitulo.Text = "SubEscalafón:"
        '
        'lblSubEscalafonValor
        '
        Me.lblSubEscalafonValor.AutoSize = True
        Me.lblSubEscalafonValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblSubEscalafonValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblSubEscalafonValor.Location = New System.Drawing.Point(3, 75)
        Me.lblSubEscalafonValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblSubEscalafonValor.Name = "lblSubEscalafonValor"
        Me.lblSubEscalafonValor.Size = New System.Drawing.Size(20, 25)
        Me.lblSubEscalafonValor.TabIndex = 3
        Me.lblSubEscalafonValor.Text = "-"
        '
        'lblJerarquiaTitulo
        '
        Me.lblJerarquiaTitulo.AutoSize = True
        Me.lblJerarquiaTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblJerarquiaTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblJerarquiaTitulo.Location = New System.Drawing.Point(35, 75)
        Me.lblJerarquiaTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblJerarquiaTitulo.Name = "lblJerarquiaTitulo"
        Me.lblJerarquiaTitulo.Size = New System.Drawing.Size(100, 25)
        Me.lblJerarquiaTitulo.TabIndex = 4
        Me.lblJerarquiaTitulo.Text = "Jerarquía:"
        '
        'lblJerarquiaValor
        '
        Me.lblJerarquiaValor.AutoSize = True
        Me.lblJerarquiaValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblJerarquiaValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblJerarquiaValor.Location = New System.Drawing.Point(3, 100)
        Me.lblJerarquiaValor.Name = "lblJerarquiaValor"
        Me.lblJerarquiaValor.Size = New System.Drawing.Size(20, 25)
        Me.lblJerarquiaValor.TabIndex = 5
        Me.lblJerarquiaValor.Text = "-"
        '
        'flpPresenciaDetalle

        Me.flpPresenciaDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpPresenciaDetalle.AutoSize = True
        Me.flpPresenciaDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpPresenciaDetalle.Controls.Add(Me.lblPresenciaTitulo)
        Me.flpPresenciaDetalle.Controls.Add(Me.lblPresenciaValor)
        Me.flpPresenciaDetalle.Location = New System.Drawing.Point(633, 127)
        Me.flpPresenciaDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpPresenciaDetalle.Name = "flpPresenciaDetalle"
        Me.flpPresenciaDetalle.Size = New System.Drawing.Size(149, 25)
        Me.flpPresenciaDetalle.TabIndex = 20
        '
        'lblPresenciaTitulo

        Me.lblPresenciaTitulo.AutoSize = True
        Me.lblPresenciaTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblPresenciaTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblPresenciaTitulo.Location = New System.Drawing.Point(0, 0)
        Me.lblPresenciaTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblPresenciaTitulo.Name = "lblPresenciaTitulo"
        Me.lblPresenciaTitulo.Size = New System.Drawing.Size(103, 25)
        Me.lblPresenciaTitulo.TabIndex = 0
        Me.lblPresenciaTitulo.Text = "Presencia:"
        '
        'lblPresenciaValor

        Me.lblPresenciaValor.AutoSize = True
        Me.lblPresenciaValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblPresenciaValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblPresenciaValor.Location = New System.Drawing.Point(106, 0)
        Me.lblPresenciaValor.Margin = New System.Windows.Forms.Padding(0, 0, 12, 0)
        Me.lblPresenciaValor.Name = "lblPresenciaValor"
        Me.lblPresenciaValor.Size = New System.Drawing.Size(20, 25)
        Me.lblPresenciaValor.TabIndex = 1
        Me.lblPresenciaValor.Text = "-"
        '
        'flpEstadoDetalle

        Me.flpEstadoDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpEstadoDetalle.AutoSize = True
        Me.flpEstadoDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpEstadoDetalle.Controls.Add(Me.lblEstadoTitulo)
        Me.flpEstadoDetalle.Controls.Add(Me.lblEstadoValor)
        Me.flpEstadoDetalle.Location = New System.Drawing.Point(633, 154)
        Me.flpEstadoDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpEstadoDetalle.Name = "flpEstadoDetalle"
        Me.flpEstadoDetalle.Size = New System.Drawing.Size(124, 25)
        Me.flpEstadoDetalle.TabIndex = 21
        '
        'lblEstadoTitulo

        Me.lblEstadoTitulo.AutoSize = True
        Me.lblEstadoTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblEstadoTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblEstadoTitulo.Location = New System.Drawing.Point(0, 0)
        Me.lblEstadoTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblEstadoTitulo.Name = "lblEstadoTitulo"
        Me.lblEstadoTitulo.Size = New System.Drawing.Size(76, 25)
        Me.lblEstadoTitulo.TabIndex = 0
        Me.lblEstadoTitulo.Text = "Estado:"
        '
        'lblEstadoValor

        Me.lblEstadoValor.AutoSize = True
        Me.lblEstadoValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblEstadoValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblEstadoValor.Location = New System.Drawing.Point(79, 0)
        Me.lblEstadoValor.Margin = New System.Windows.Forms.Padding(0, 0, 12, 0)
        Me.lblEstadoValor.Name = "lblEstadoValor"
        Me.lblEstadoValor.Size = New System.Drawing.Size(20, 25)
        Me.lblEstadoValor.TabIndex = 1
        Me.lblEstadoValor.Text = "-"
        '
        'btnVerSituacion
        '
        Me.btnVerSituacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnVerSituacion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerSituacion.Location = New System.Drawing.Point(3, 360)
        Me.btnVerSituacion.Name = "btnVerSituacion"
        Me.btnVerSituacion.Size = New System.Drawing.Size(371, 36)
        Me.btnVerSituacion.TabIndex = 17
        Me.btnVerSituacion.Text = "Situación"
        Me.btnVerSituacion.UseVisualStyleBackColor = True
        Me.btnVerSituacion.Visible = False
        '
        'tlpAcciones
        '
        Me.tlpAcciones.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpAcciones.AutoSize = True
        Me.tlpAcciones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpAcciones.ColumnCount = 4
        Me.tlpAcciones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpAcciones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpAcciones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpAcciones.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpAcciones.Controls.Add(Me.btnGenerarFicha, 0, 0)
        Me.tlpAcciones.Controls.Add(Me.btnSancionar, 1, 0)
        Me.tlpAcciones.Controls.Add(Me.btnNovedades, 2, 0)
        Me.tlpAcciones.Controls.Add(Me.btnNotificar, 3, 0)
        Me.tlpAcciones.Location = New System.Drawing.Point(0, 0)
        Me.tlpAcciones.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.tlpAcciones.Name = "tlpAcciones"
        Me.tlpAcciones.RowCount = 1
        Me.tlpAcciones.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpAcciones.Size = New System.Drawing.Size(377, 38)
        Me.tlpAcciones.TabIndex = 3
        '
        'btnGenerarFicha
        '
        Me.btnGenerarFicha.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnGenerarFicha.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGenerarFicha.Location = New System.Drawing.Point(3, 3)
        Me.btnGenerarFicha.MinimumSize = New System.Drawing.Size(90, 32)
        Me.btnGenerarFicha.Name = "btnGenerarFicha"
        Me.btnGenerarFicha.Size = New System.Drawing.Size(90, 32)
        Me.btnGenerarFicha.TabIndex = 0
        Me.btnGenerarFicha.Text = "Ficha"
        Me.btnGenerarFicha.UseVisualStyleBackColor = True
        Me.btnGenerarFicha.Visible = False
        '
        'btnSancionar
        '
        Me.btnSancionar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSancionar.Location = New System.Drawing.Point(97, 3)
        Me.btnSancionar.MinimumSize = New System.Drawing.Size(90, 32)
        Me.btnSancionar.Name = "btnSancionar"
        Me.btnSancionar.Size = New System.Drawing.Size(90, 32)
        Me.btnSancionar.TabIndex = 1
        Me.btnSancionar.Text = "Sancionar"
        Me.btnSancionar.UseVisualStyleBackColor = True
        '
        'btnNovedades
        '
        Me.btnNovedades.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNovedades.Location = New System.Drawing.Point(191, 3)
        Me.btnNovedades.MinimumSize = New System.Drawing.Size(90, 32)
        Me.btnNovedades.Name = "btnNovedades"
        Me.btnNovedades.Size = New System.Drawing.Size(90, 32)
        Me.btnNovedades.TabIndex = 2
        Me.btnNovedades.Text = "Novedades"
        Me.btnNovedades.UseVisualStyleBackColor = True
        '
        'btnNotificar
        '
        Me.btnNotificar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNotificar.Location = New System.Drawing.Point(285, 3)
        Me.btnNotificar.MinimumSize = New System.Drawing.Size(90, 32)
        Me.btnNotificar.Name = "btnNotificar"
        Me.btnNotificar.Size = New System.Drawing.Size(90, 32)
        Me.btnNotificar.TabIndex = 3
        Me.btnNotificar.Text = "Notificar"
        Me.btnNotificar.UseVisualStyleBackColor = True
        '
        'pbFotoDetalle
        '
        Me.pbFotoDetalle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbFotoDetalle.Location = New System.Drawing.Point(3, 47)
        Me.pbFotoDetalle.Margin = New System.Windows.Forms.Padding(3, 3, 3, 16)
        Me.pbFotoDetalle.Name = "pbFotoDetalle"
        Me.pbFotoDetalle.Size = New System.Drawing.Size(371, 294)
        Me.pbFotoDetalle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFotoDetalle.TabIndex = 0
        Me.pbFotoDetalle.TabStop = False
        '
        'frmFuncionarioBuscar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(944, 699)
        Me.Controls.Add(Me.splitContenedor)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frmFuncionarioBuscar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Buscar Funcionario"
        Me.splitContenedor.Panel1.ResumeLayout(False)
        Me.splitContenedor.Panel2.ResumeLayout(False)
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedor.ResumeLayout(False)
        Me.tlpResultados.ResumeLayout(False)
        CType(Me.dgvFuncionarios, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelBusquedaLista.ResumeLayout(False)
        Me.tlpBusqueda.ResumeLayout(False)
        Me.tlpBusqueda.PerformLayout()
        Me.panelDetalle.ResumeLayout(False)
        Me.tlpDetalleVertical.ResumeLayout(False)
        Me.tlpDetalleVertical.PerformLayout()
        Me.flpDetalles.ResumeLayout(False)
        Me.flpDetalles.PerformLayout()
        Me.flpCIDetalle.ResumeLayout(False)
        Me.flpCIDetalle.PerformLayout()
        Me.flpTipoDetalle.ResumeLayout(False)
        Me.flpTipoDetalle.PerformLayout()
        Me.flpFechaIngresoDetalle.ResumeLayout(False)
        Me.flpFechaIngresoDetalle.PerformLayout()
        Me.flpHorarioDetalle.ResumeLayout(False)
        Me.flpHorarioDetalle.PerformLayout()
        Me.flpUbicacionDetalle.ResumeLayout(False)
        Me.flpUbicacionDetalle.PerformLayout()
        Me.flpSubDireccionDetalle.ResumeLayout(False)
        Me.flpSubDireccionDetalle.PerformLayout()
        Me.flpCargoDetalle.ResumeLayout(False)
        Me.flpCargoDetalle.PerformLayout()
        Me.flpPresenciaDetalle.ResumeLayout(False)
        Me.flpPresenciaDetalle.PerformLayout()
        Me.flpEstadoDetalle.ResumeLayout(False)
        Me.flpEstadoDetalle.PerformLayout()
        Me.tlpAcciones.ResumeLayout(False)
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents splitContenedor As SplitContainer
    Friend WithEvents dgvFuncionarios As DataGridView
    Friend WithEvents panelDetalle As Panel
    Friend WithEvents pbFotoDetalle As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents PanelBusquedaLista As Panel
    Friend WithEvents tlpResultados As TableLayoutPanel
    Friend WithEvents lstCargos As ListBox
    Friend WithEvents txtBusqueda As TextBox
    Friend WithEvents lblBuscar As Label
    Friend WithEvents tlpBusqueda As TableLayoutPanel
    Friend WithEvents lblNombreCompleto As Label
    Friend WithEvents flpHorarioDetalle As FlowLayoutPanel
    Friend WithEvents lblHorarioTitulo As Label
    Friend WithEvents lblSemanaValor As Label
    Friend WithEvents lblTurnoTitulo As Label
    Friend WithEvents lblTurnoValor As Label
    Friend WithEvents lblPlantillaTitulo As Label
    Friend WithEvents lblPlantillaValor As Label
    Friend WithEvents flpCIDetalle As FlowLayoutPanel
    Friend WithEvents lblCITitulo As Label
    Friend WithEvents lblCIValor As Label
    Friend WithEvents flpTipoDetalle As FlowLayoutPanel
    Friend WithEvents lblTipoTitulo As Label
    Friend WithEvents lblTipoValor As Label
    Friend WithEvents flpFechaIngresoDetalle As FlowLayoutPanel
    Friend WithEvents lblFechaIngresoTitulo As Label
    Friend WithEvents lblFechaIngresoValor As Label
    Friend WithEvents btnVerSituacion As Button
    Friend WithEvents flpUbicacionDetalle As FlowLayoutPanel
    Friend WithEvents lblUbicacionTitulo As Label
    Friend WithEvents lblUnidadValor As Label
    Friend WithEvents lblPuestoTitulo As Label
    Friend WithEvents lblPuestoValor As Label
    Friend WithEvents flpSubDireccionDetalle As FlowLayoutPanel
    Friend WithEvents lblSubDireccionTitulo As Label
    Friend WithEvents lblSubDireccionValor As Label
    Friend WithEvents flpCargoDetalle As FlowLayoutPanel
    Friend WithEvents lblEscalafonTitulo As Label
    Friend WithEvents lblEscalafonValor As Label
    Friend WithEvents lblSubEscalafonTitulo As Label
    Friend WithEvents lblSubEscalafonValor As Label
    Friend WithEvents lblJerarquiaTitulo As Label
    Friend WithEvents lblJerarquiaValor As Label
    Friend WithEvents flpPresenciaDetalle As FlowLayoutPanel
    Friend WithEvents lblPresenciaTitulo As Label
    Friend WithEvents lblPresenciaValor As Label
    Friend WithEvents flpEstadoDetalle As FlowLayoutPanel
    Friend WithEvents lblEstadoTitulo As Label
    Friend WithEvents lblEstadoValor As Label
    Friend WithEvents btnCopiarContenido As Button
    Friend WithEvents flpDetalles As FlowLayoutPanel
    Friend WithEvents tlpDetalleVertical As TableLayoutPanel
    Friend WithEvents tlpAcciones As TableLayoutPanel
    Friend WithEvents btnGenerarFicha As Button
    Friend WithEvents btnSancionar As Button
    Friend WithEvents btnNovedades As Button
    Friend WithEvents btnNotificar As Button
End Class
