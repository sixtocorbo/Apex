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
        Me.lblCI = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.lblFechaIngreso = New System.Windows.Forms.Label()
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
        Me.lblSubDireccion = New System.Windows.Forms.Label()
        Me.flpCargoDetalle = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblEscalafonTitulo = New System.Windows.Forms.Label()
        Me.lblEscalafonValor = New System.Windows.Forms.Label()
        Me.lblSubEscalafonTitulo = New System.Windows.Forms.Label()
        Me.lblSubEscalafonValor = New System.Windows.Forms.Label()
        Me.lblJerarquiaTitulo = New System.Windows.Forms.Label()
        Me.lblJerarquiaValor = New System.Windows.Forms.Label()
        Me.lblPresencia = New System.Windows.Forms.Label()
        Me.lblEstadoActividad = New System.Windows.Forms.Label()
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
        Me.flpHorarioDetalle.SuspendLayout()
        Me.flpUbicacionDetalle.SuspendLayout()
        Me.flpCargoDetalle.SuspendLayout()
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
        Me.splitContenedor.Size = New System.Drawing.Size(944, 695)
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
        Me.tlpResultados.Size = New System.Drawing.Size(539, 580)
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
        Me.dgvFuncionarios.Size = New System.Drawing.Size(431, 574)
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
        Me.lstCargos.Size = New System.Drawing.Size(96, 574)
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
        Me.panelDetalle.Size = New System.Drawing.Size(401, 695)
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
        Me.tlpDetalleVertical.Size = New System.Drawing.Size(377, 671)
        Me.tlpDetalleVertical.TabIndex = 1
        '
        'flpDetalles
        '
        Me.flpDetalles.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.flpDetalles.AutoScroll = True
        Me.flpDetalles.Controls.Add(Me.lblNombreCompleto)
        Me.flpDetalles.Controls.Add(Me.lblCI)
        Me.flpDetalles.Controls.Add(Me.lblTipo)
        Me.flpDetalles.Controls.Add(Me.lblFechaIngreso)
        Me.flpDetalles.Controls.Add(Me.flpHorarioDetalle)
        Me.flpDetalles.Controls.Add(Me.flpUbicacionDetalle)
        Me.flpDetalles.Controls.Add(Me.lblSubDireccion)
        Me.flpDetalles.Controls.Add(Me.flpCargoDetalle)
        Me.flpDetalles.Controls.Add(Me.lblPresencia)
        Me.flpDetalles.Controls.Add(Me.lblEstadoActividad)
        Me.flpDetalles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpDetalles.Location = New System.Drawing.Point(3, 398)
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
        'lblCI
        '
        Me.lblCI.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCI.AutoSize = True
        Me.lblCI.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblCI.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblCI.Location = New System.Drawing.Point(3, 54)
        Me.lblCI.Margin = New System.Windows.Forms.Padding(3, 0, 3, 8)
        Me.lblCI.Name = "lblCI"
        Me.lblCI.Size = New System.Drawing.Size(46, 28)
        Me.lblCI.TabIndex = 1
        Me.lblCI.Text = "CI: -"
        '
        'lblTipo
        '
        Me.lblTipo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblTipo.ForeColor = System.Drawing.Color.DimGray
        Me.lblTipo.Location = New System.Drawing.Point(3, 90)
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(66, 25)
        Me.lblTipo.TabIndex = 6
        Me.lblTipo.Text = "Tipo: -"
        '
        'lblFechaIngreso
        '
        Me.lblFechaIngreso.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblFechaIngreso.AutoSize = True
        Me.lblFechaIngreso.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblFechaIngreso.ForeColor = System.Drawing.Color.DimGray
        Me.lblFechaIngreso.Location = New System.Drawing.Point(3, 117)
        Me.lblFechaIngreso.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblFechaIngreso.Name = "lblFechaIngreso"
        Me.lblFechaIngreso.Size = New System.Drawing.Size(146, 25)
        Me.lblFechaIngreso.TabIndex = 8
        Me.lblFechaIngreso.Text = "Fecha Ingreso: -"
        '
        'flpHorarioDetalle

        Me.flpHorarioDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpHorarioDetalle.AutoSize = True
        Me.flpHorarioDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpHorarioDetalle.Controls.Add(Me.lblHorarioTitulo)
        Me.flpHorarioDetalle.Controls.Add(Me.lblSemanaValor)
        Me.flpHorarioDetalle.Controls.Add(Me.lblTurnoTitulo)
        Me.flpHorarioDetalle.Controls.Add(Me.lblTurnoValor)
        Me.flpHorarioDetalle.Controls.Add(Me.lblPlantillaTitulo)
        Me.flpHorarioDetalle.Controls.Add(Me.lblPlantillaValor)
        Me.flpHorarioDetalle.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
        Me.flpHorarioDetalle.Location = New System.Drawing.Point(3, 144)
        Me.flpHorarioDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpHorarioDetalle.Name = "flpHorarioDetalle"
        Me.flpHorarioDetalle.Size = New System.Drawing.Size(362, 25)
        Me.flpHorarioDetalle.TabIndex = 13
        Me.flpHorarioDetalle.WrapContents = True
        '
        'lblHorarioTitulo

        Me.lblHorarioTitulo.AutoSize = True
        Me.lblHorarioTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblHorarioTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblHorarioTitulo.Location = New System.Drawing.Point(3, 0)
        Me.lblHorarioTitulo.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.lblHorarioTitulo.Name = "lblHorarioTitulo"
        Me.lblHorarioTitulo.Size = New System.Drawing.Size(87, 25)
        Me.lblHorarioTitulo.TabIndex = 0
        Me.lblHorarioTitulo.Text = "Semana:"

        'lblSemanaValor

        Me.lblSemanaValor.AutoSize = True
        Me.lblSemanaValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblSemanaValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblSemanaValor.Location = New System.Drawing.Point(96, 0)
        Me.lblSemanaValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblSemanaValor.Name = "lblSemanaValor"
        Me.lblSemanaValor.Size = New System.Drawing.Size(19, 25)
        Me.lblSemanaValor.TabIndex = 1
        Me.lblSemanaValor.Text = "-"

        'lblTurnoTitulo

        Me.lblTurnoTitulo.AutoSize = True
        Me.lblTurnoTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblTurnoTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblTurnoTitulo.Location = New System.Drawing.Point(130, 0)
        Me.lblTurnoTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblTurnoTitulo.Name = "lblTurnoTitulo"
        Me.lblTurnoTitulo.Size = New System.Drawing.Size(70, 25)
        Me.lblTurnoTitulo.TabIndex = 2
        Me.lblTurnoTitulo.Text = "Turno:"

        'lblTurnoValor

        Me.lblTurnoValor.AutoSize = True
        Me.lblTurnoValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblTurnoValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblTurnoValor.Location = New System.Drawing.Point(206, 0)
        Me.lblTurnoValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblTurnoValor.Name = "lblTurnoValor"
        Me.lblTurnoValor.Size = New System.Drawing.Size(19, 25)
        Me.lblTurnoValor.TabIndex = 3
        Me.lblTurnoValor.Text = "-"

        'lblPlantillaTitulo

        Me.lblPlantillaTitulo.AutoSize = True
        Me.lblPlantillaTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblPlantillaTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblPlantillaTitulo.Location = New System.Drawing.Point(240, 0)
        Me.lblPlantillaTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblPlantillaTitulo.Name = "lblPlantillaTitulo"
        Me.lblPlantillaTitulo.Size = New System.Drawing.Size(88, 25)
        Me.lblPlantillaTitulo.TabIndex = 4
        Me.lblPlantillaTitulo.Text = "Horario:"

        'lblPlantillaValor

        Me.lblPlantillaValor.AutoSize = True
        Me.lblPlantillaValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblPlantillaValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblPlantillaValor.Location = New System.Drawing.Point(334, 0)
        Me.lblPlantillaValor.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.lblPlantillaValor.Name = "lblPlantillaValor"
        Me.lblPlantillaValor.Size = New System.Drawing.Size(19, 25)
        Me.lblPlantillaValor.TabIndex = 5
        Me.lblPlantillaValor.Text = "-"
        '
        '
        'flpUbicacionDetalle

        Me.flpUbicacionDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpUbicacionDetalle.AutoSize = True
        Me.flpUbicacionDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpUbicacionDetalle.Controls.Add(Me.lblUbicacionTitulo)
        Me.flpUbicacionDetalle.Controls.Add(Me.lblUnidadValor)
        Me.flpUbicacionDetalle.Controls.Add(Me.lblPuestoTitulo)
        Me.flpUbicacionDetalle.Controls.Add(Me.lblPuestoValor)
        Me.flpUbicacionDetalle.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
        Me.flpUbicacionDetalle.Location = New System.Drawing.Point(3, 171)
        Me.flpUbicacionDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpUbicacionDetalle.Name = "flpUbicacionDetalle"
        Me.flpUbicacionDetalle.Size = New System.Drawing.Size(286, 25)
        Me.flpUbicacionDetalle.TabIndex = 23
        Me.flpUbicacionDetalle.WrapContents = True

        'lblUbicacionTitulo

        Me.lblUbicacionTitulo.AutoSize = True
        Me.lblUbicacionTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblUbicacionTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblUbicacionTitulo.Location = New System.Drawing.Point(3, 0)
        Me.lblUbicacionTitulo.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.lblUbicacionTitulo.Name = "lblUbicacionTitulo"
        Me.lblUbicacionTitulo.Size = New System.Drawing.Size(102, 25)
        Me.lblUbicacionTitulo.TabIndex = 0
        Me.lblUbicacionTitulo.Text = "Ubicación:"

        'lblUnidadValor

        Me.lblUnidadValor.AutoSize = True
        Me.lblUnidadValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblUnidadValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblUnidadValor.Location = New System.Drawing.Point(86, 0)
        Me.lblUnidadValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblUnidadValor.Name = "lblUnidadValor"
        Me.lblUnidadValor.Size = New System.Drawing.Size(19, 25)
        Me.lblUnidadValor.TabIndex = 1
        Me.lblUnidadValor.Text = "-"

        'lblPuestoTitulo

        Me.lblPuestoTitulo.AutoSize = True
        Me.lblPuestoTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblPuestoTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblPuestoTitulo.Location = New System.Drawing.Point(121, 0)
        Me.lblPuestoTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblPuestoTitulo.Name = "lblPuestoTitulo"
        Me.lblPuestoTitulo.Size = New System.Drawing.Size(72, 25)
        Me.lblPuestoTitulo.TabIndex = 2
        Me.lblPuestoTitulo.Text = "Puesto:"

        'lblPuestoValor

        Me.lblPuestoValor.AutoSize = True
        Me.lblPuestoValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblPuestoValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblPuestoValor.Location = New System.Drawing.Point(199, 0)
        Me.lblPuestoValor.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.lblPuestoValor.Name = "lblPuestoValor"
        Me.lblPuestoValor.Size = New System.Drawing.Size(19, 25)
        Me.lblPuestoValor.TabIndex = 3
        Me.lblPuestoValor.Text = "-"

        'flpCargoDetalle

        Me.flpCargoDetalle.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.flpCargoDetalle.AutoSize = True
        Me.flpCargoDetalle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpCargoDetalle.Controls.Add(Me.lblEscalafonTitulo)
        Me.flpCargoDetalle.Controls.Add(Me.lblEscalafonValor)
        Me.flpCargoDetalle.Controls.Add(Me.lblSubEscalafonTitulo)
        Me.flpCargoDetalle.Controls.Add(Me.lblSubEscalafonValor)
        Me.flpCargoDetalle.Controls.Add(Me.lblJerarquiaTitulo)
        Me.flpCargoDetalle.Controls.Add(Me.lblJerarquiaValor)
        Me.flpCargoDetalle.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
        Me.flpCargoDetalle.Location = New System.Drawing.Point(3, 225)
        Me.flpCargoDetalle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.flpCargoDetalle.Name = "flpCargoDetalle"
        Me.flpCargoDetalle.Size = New System.Drawing.Size(358, 25)
        Me.flpCargoDetalle.TabIndex = 4
        Me.flpCargoDetalle.WrapContents = True

        'lblEscalafonTitulo

        Me.lblEscalafonTitulo.AutoSize = True
        Me.lblEscalafonTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblEscalafonTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblEscalafonTitulo.Location = New System.Drawing.Point(3, 0)
        Me.lblEscalafonTitulo.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.lblEscalafonTitulo.Name = "lblEscalafonTitulo"
        Me.lblEscalafonTitulo.Size = New System.Drawing.Size(99, 25)
        Me.lblEscalafonTitulo.TabIndex = 0
        Me.lblEscalafonTitulo.Text = "Escalafón:"

        'lblEscalafonValor

        Me.lblEscalafonValor.AutoSize = True
        Me.lblEscalafonValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblEscalafonValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblEscalafonValor.Location = New System.Drawing.Point(108, 0)
        Me.lblEscalafonValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblEscalafonValor.Name = "lblEscalafonValor"
        Me.lblEscalafonValor.Size = New System.Drawing.Size(19, 25)
        Me.lblEscalafonValor.TabIndex = 1
        Me.lblEscalafonValor.Text = "-"

        'lblSubEscalafonTitulo

        Me.lblSubEscalafonTitulo.AutoSize = True
        Me.lblSubEscalafonTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblSubEscalafonTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblSubEscalafonTitulo.Location = New System.Drawing.Point(142, 0)
        Me.lblSubEscalafonTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblSubEscalafonTitulo.Name = "lblSubEscalafonTitulo"
        Me.lblSubEscalafonTitulo.Size = New System.Drawing.Size(135, 25)
        Me.lblSubEscalafonTitulo.TabIndex = 2
        Me.lblSubEscalafonTitulo.Text = "SubEscalafón:"

        'lblSubEscalafonValor

        Me.lblSubEscalafonValor.AutoSize = True
        Me.lblSubEscalafonValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblSubEscalafonValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblSubEscalafonValor.Location = New System.Drawing.Point(280, 0)
        Me.lblSubEscalafonValor.Margin = New System.Windows.Forms.Padding(3, 0, 12, 0)
        Me.lblSubEscalafonValor.Name = "lblSubEscalafonValor"
        Me.lblSubEscalafonValor.Size = New System.Drawing.Size(19, 25)
        Me.lblSubEscalafonValor.TabIndex = 3
        Me.lblSubEscalafonValor.Text = "-"

        'lblJerarquiaTitulo

        Me.lblJerarquiaTitulo.AutoSize = True
        Me.lblJerarquiaTitulo.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold)
        Me.lblJerarquiaTitulo.ForeColor = System.Drawing.Color.DimGray
        Me.lblJerarquiaTitulo.Location = New System.Drawing.Point(315, 0)
        Me.lblJerarquiaTitulo.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.lblJerarquiaTitulo.Name = "lblJerarquiaTitulo"
        Me.lblJerarquiaTitulo.Size = New System.Drawing.Size(96, 25)
        Me.lblJerarquiaTitulo.TabIndex = 4
        Me.lblJerarquiaTitulo.Text = "Jerarquía:"

        'lblJerarquiaValor

        Me.lblJerarquiaValor.AutoSize = True
        Me.lblJerarquiaValor.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblJerarquiaValor.ForeColor = System.Drawing.Color.DimGray
        Me.lblJerarquiaValor.Location = New System.Drawing.Point(417, 0)
        Me.lblJerarquiaValor.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.lblJerarquiaValor.Name = "lblJerarquiaValor"
        Me.lblJerarquiaValor.Size = New System.Drawing.Size(19, 25)
        Me.lblJerarquiaValor.TabIndex = 5
        Me.lblJerarquiaValor.Text = "-"

        'lblSubDireccion
        '
        Me.lblSubDireccion.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblSubDireccion.AutoSize = True
        Me.lblSubDireccion.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblSubDireccion.ForeColor = System.Drawing.Color.DimGray
        Me.lblSubDireccion.Location = New System.Drawing.Point(3, 198)
        Me.lblSubDireccion.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblSubDireccion.Name = "lblSubDireccion"
        Me.lblSubDireccion.Size = New System.Drawing.Size(152, 25)
        Me.lblSubDireccion.TabIndex = 24
        Me.lblSubDireccion.Text = "SubDireccion: -"
        '
        'lblPresencia
        '
        Me.lblPresencia.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblPresencia.AutoSize = True
        Me.lblPresencia.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblPresencia.ForeColor = System.Drawing.Color.DimGray
        Me.lblPresencia.Location = New System.Drawing.Point(3, 252)
        Me.lblPresencia.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblPresencia.Name = "lblPresencia"
        Me.lblPresencia.Size = New System.Drawing.Size(110, 25)
        Me.lblPresencia.TabIndex = 20
        Me.lblPresencia.Text = "Presencia: -"
        '
        'lblEstadoActividad
        '
        Me.lblEstadoActividad.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblEstadoActividad.AutoSize = True
        Me.lblEstadoActividad.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblEstadoActividad.ForeColor = System.Drawing.Color.DimGray
        Me.lblEstadoActividad.Location = New System.Drawing.Point(3, 279)
        Me.lblEstadoActividad.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblEstadoActividad.Name = "lblEstadoActividad"
        Me.lblEstadoActividad.Size = New System.Drawing.Size(85, 25)
        Me.lblEstadoActividad.TabIndex = 21
        Me.lblEstadoActividad.Text = "Estado: -"
        '
        'btnVerSituacion
        '
        Me.btnVerSituacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnVerSituacion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerSituacion.Location = New System.Drawing.Point(3, 410)
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
        Me.pbFotoDetalle.Size = New System.Drawing.Size(371, 290)
        Me.pbFotoDetalle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFotoDetalle.TabIndex = 0
        Me.pbFotoDetalle.TabStop = False
        '
        'frmFuncionarioBuscar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(944, 695)
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
        Me.flpHorarioDetalle.ResumeLayout(False)
        Me.flpHorarioDetalle.PerformLayout()
        Me.flpUbicacionDetalle.ResumeLayout(False)
        Me.flpUbicacionDetalle.PerformLayout()
        Me.flpCargoDetalle.ResumeLayout(False)
        Me.flpCargoDetalle.PerformLayout()
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
    Friend WithEvents lblTipo As Label
    Friend WithEvents btnVerSituacion As Button
    Friend WithEvents lblFechaIngreso As Label
    Friend WithEvents lblCI As Label
    Friend WithEvents flpUbicacionDetalle As FlowLayoutPanel
    Friend WithEvents lblUbicacionTitulo As Label
    Friend WithEvents lblUnidadValor As Label
    Friend WithEvents lblPuestoTitulo As Label
    Friend WithEvents lblPuestoValor As Label
    Friend WithEvents lblSubDireccion As Label
    Friend WithEvents flpCargoDetalle As FlowLayoutPanel
    Friend WithEvents lblEscalafonTitulo As Label
    Friend WithEvents lblEscalafonValor As Label
    Friend WithEvents lblSubEscalafonTitulo As Label
    Friend WithEvents lblSubEscalafonValor As Label
    Friend WithEvents lblJerarquiaTitulo As Label
    Friend WithEvents lblJerarquiaValor As Label
    Friend WithEvents lblPresencia As Label
    Friend WithEvents lblEstadoActividad As Label
    Friend WithEvents btnCopiarContenido As Button
    Friend WithEvents flpDetalles As FlowLayoutPanel
    Friend WithEvents tlpDetalleVertical As TableLayoutPanel
    Friend WithEvents tlpAcciones As TableLayoutPanel
    Friend WithEvents btnGenerarFicha As Button
    Friend WithEvents btnSancionar As Button
    Friend WithEvents btnNovedades As Button
    Friend WithEvents btnNotificar As Button
End Class
