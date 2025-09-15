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
        Me.dgvFuncionarios = New System.Windows.Forms.DataGridView()
        Me.PanelBusquedaLista = New System.Windows.Forms.Panel()
        Me.tlpBusqueda = New System.Windows.Forms.TableLayoutPanel()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.btnCopiarContenido = New System.Windows.Forms.Button()
        Me.panelDetalle = New System.Windows.Forms.Panel()
        Me.pbFotoDetalle = New System.Windows.Forms.PictureBox()
        Me.btnVerSituacion = New System.Windows.Forms.Button()
        Me.lblNombreCompleto = New System.Windows.Forms.Label()
        Me.lblCI = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.lblFechaIngreso = New System.Windows.Forms.Label()
        Me.lblHorarioCompleto = New System.Windows.Forms.Label()
        Me.lblCargo = New System.Windows.Forms.Label()
        Me.lblPresencia = New System.Windows.Forms.Label()
        Me.lblEstadoActividad = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.tlpDetalleVertical = New System.Windows.Forms.TableLayoutPanel()
        Me.btnNotificar = New System.Windows.Forms.Button()
        Me.btnNovedades = New System.Windows.Forms.Button()
        Me.btnSancionar = New System.Windows.Forms.Button()
        Me.btnGenerarFicha = New System.Windows.Forms.Button()
        Me.tlpAcciones = New System.Windows.Forms.TableLayoutPanel()
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedor.Panel1.SuspendLayout()
        Me.splitContenedor.Panel2.SuspendLayout()
        Me.splitContenedor.SuspendLayout()
        CType(Me.dgvFuncionarios, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelBusquedaLista.SuspendLayout()
        Me.tlpBusqueda.SuspendLayout()
        Me.panelDetalle.SuspendLayout()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.tlpDetalleVertical.SuspendLayout()
        Me.tlpAcciones.SuspendLayout()
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
        Me.splitContenedor.Panel1.Controls.Add(Me.dgvFuncionarios)
        Me.splitContenedor.Panel1.Controls.Add(Me.PanelBusquedaLista)
        Me.splitContenedor.Panel1MinSize = 280
        '
        'splitContenedor.Panel2
        '
        Me.splitContenedor.Panel2.Controls.Add(Me.panelDetalle)
        Me.splitContenedor.Panel2MinSize = 400
        Me.splitContenedor.Size = New System.Drawing.Size(1097, 673)
        Me.splitContenedor.SplitterDistance = 294
        Me.splitContenedor.TabIndex = 0
        '
        'dgvFuncionarios
        '
        Me.dgvFuncionarios.AllowUserToAddRows = False
        Me.dgvFuncionarios.AllowUserToDeleteRows = False
        Me.dgvFuncionarios.AllowUserToResizeColumns = False
        Me.dgvFuncionarios.AllowUserToResizeRows = False
        Me.dgvFuncionarios.ColumnHeadersHeight = 34
        Me.dgvFuncionarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvFuncionarios.Location = New System.Drawing.Point(0, 115)
        Me.dgvFuncionarios.Name = "dgvFuncionarios"
        Me.dgvFuncionarios.ReadOnly = True
        Me.dgvFuncionarios.RowHeadersWidth = 62
        Me.dgvFuncionarios.Size = New System.Drawing.Size(294, 558)
        Me.dgvFuncionarios.TabIndex = 1
        '
        'PanelBusquedaLista
        '
        Me.PanelBusquedaLista.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelBusquedaLista.Controls.Add(Me.tlpBusqueda)
        Me.PanelBusquedaLista.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaLista.Location = New System.Drawing.Point(0, 0)
        Me.PanelBusquedaLista.Name = "PanelBusquedaLista"
        Me.PanelBusquedaLista.Padding = New System.Windows.Forms.Padding(12)
        Me.PanelBusquedaLista.Size = New System.Drawing.Size(294, 115)
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
        Me.tlpBusqueda.Size = New System.Drawing.Size(270, 91)
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
        Me.txtBusqueda.Size = New System.Drawing.Size(181, 33)
        Me.txtBusqueda.TabIndex = 1
        '
        'btnCopiarContenido
        '
        Me.btnCopiarContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnCopiarContenido.Location = New System.Drawing.Point(86, 50)
        Me.btnCopiarContenido.Name = "btnCopiarContenido"
        Me.btnCopiarContenido.Size = New System.Drawing.Size(181, 38)
        Me.btnCopiarContenido.TabIndex = 3
        Me.btnCopiarContenido.Text = "Copiar selección"
        Me.btnCopiarContenido.UseVisualStyleBackColor = True
        Me.btnCopiarContenido.Visible = False
        '
        'panelDetalle
        '
        Me.panelDetalle.AutoScroll = True
        Me.panelDetalle.BackColor = System.Drawing.Color.White
        Me.panelDetalle.Controls.Add(Me.tlpDetalleVertical)
        Me.panelDetalle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelDetalle.Location = New System.Drawing.Point(0, 0)
        Me.panelDetalle.Name = "panelDetalle"
        Me.panelDetalle.Padding = New System.Windows.Forms.Padding(12)
        Me.panelDetalle.Size = New System.Drawing.Size(799, 673)
        Me.panelDetalle.TabIndex = 0
        '
        'pbFotoDetalle
        '
        Me.pbFotoDetalle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbFotoDetalle.Location = New System.Drawing.Point(3, 47)
        Me.pbFotoDetalle.Margin = New System.Windows.Forms.Padding(3, 3, 3, 16)
        Me.pbFotoDetalle.Name = "pbFotoDetalle"
        Me.pbFotoDetalle.Size = New System.Drawing.Size(769, 268)
        Me.pbFotoDetalle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFotoDetalle.TabIndex = 0
        Me.pbFotoDetalle.TabStop = False
        '
        'btnVerSituacion
        '
        Me.btnVerSituacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnVerSituacion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerSituacion.Location = New System.Drawing.Point(3, 334)
        Me.btnVerSituacion.Name = "btnVerSituacion"
        Me.btnVerSituacion.Size = New System.Drawing.Size(769, 36)
        Me.btnVerSituacion.TabIndex = 17
        Me.btnVerSituacion.Text = "Situación"
        Me.btnVerSituacion.UseVisualStyleBackColor = True
        Me.btnVerSituacion.Visible = False
        '
        'lblNombreCompleto
        '
        Me.lblNombreCompleto.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblNombreCompleto.AutoSize = True
        Me.lblNombreCompleto.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblNombreCompleto.Location = New System.Drawing.Point(3, 8)
        Me.lblNombreCompleto.Margin = New System.Windows.Forms.Padding(3, 8, 3, 8)
        Me.lblNombreCompleto.Name = "lblNombreCompleto"
        Me.lblNombreCompleto.Size = New System.Drawing.Size(288, 38)
        Me.lblNombreCompleto.TabIndex = 2
        Me.lblNombreCompleto.Text = "Nombre Funcionario"
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
        'lblHorarioCompleto
        '
        Me.lblHorarioCompleto.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblHorarioCompleto.AutoSize = True
        Me.lblHorarioCompleto.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblHorarioCompleto.ForeColor = System.Drawing.Color.DimGray
        Me.lblHorarioCompleto.Location = New System.Drawing.Point(3, 144)
        Me.lblHorarioCompleto.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblHorarioCompleto.Name = "lblHorarioCompleto"
        Me.lblHorarioCompleto.Size = New System.Drawing.Size(93, 25)
        Me.lblHorarioCompleto.TabIndex = 13
        Me.lblHorarioCompleto.Text = "Horario: -"
        '
        'lblCargo
        '
        Me.lblCargo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCargo.AutoSize = True
        Me.lblCargo.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblCargo.ForeColor = System.Drawing.Color.DimGray
        Me.lblCargo.Location = New System.Drawing.Point(3, 171)
        Me.lblCargo.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblCargo.Name = "lblCargo"
        Me.lblCargo.Size = New System.Drawing.Size(80, 25)
        Me.lblCargo.TabIndex = 4
        Me.lblCargo.Text = "Cargo: -"
        '
        'lblPresencia
        '
        Me.lblPresencia.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblPresencia.AutoSize = True
        Me.lblPresencia.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblPresencia.ForeColor = System.Drawing.Color.DimGray
        Me.lblPresencia.Location = New System.Drawing.Point(3, 198)
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
        Me.lblEstadoActividad.Location = New System.Drawing.Point(3, 225)
        Me.lblEstadoActividad.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblEstadoActividad.Name = "lblEstadoActividad"
        Me.lblEstadoActividad.Size = New System.Drawing.Size(85, 25)
        Me.lblEstadoActividad.TabIndex = 21
        Me.lblEstadoActividad.Text = "Estado: -"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoScroll = True
        Me.FlowLayoutPanel1.Controls.Add(Me.lblNombreCompleto)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblCI)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblTipo)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblFechaIngreso)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblHorarioCompleto)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblCargo)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblPresencia)
        Me.FlowLayoutPanel1.Controls.Add(Me.lblEstadoActividad)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 376)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(769, 270)
        Me.FlowLayoutPanel1.TabIndex = 22
        '
        'tlpDetalleVertical
        '
        Me.tlpDetalleVertical.AutoScroll = True
        Me.tlpDetalleVertical.ColumnCount = 1
        Me.tlpDetalleVertical.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpDetalleVertical.Controls.Add(Me.FlowLayoutPanel1, 0, 3)
        Me.tlpDetalleVertical.Controls.Add(Me.btnVerSituacion, 0, 2)
        Me.tlpDetalleVertical.Controls.Add(Me.tlpAcciones, 0, 0)
        Me.tlpDetalleVertical.Controls.Add(Me.pbFotoDetalle, 0, 1)
        Me.tlpDetalleVertical.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpDetalleVertical.Location = New System.Drawing.Point(12, 12)
        Me.tlpDetalleVertical.Name = "tlpDetalleVertical"
        Me.tlpDetalleVertical.RowCount = 4
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.Size = New System.Drawing.Size(775, 649)
        Me.tlpDetalleVertical.TabIndex = 1
        '
        'btnNotificar
        '
        Me.btnNotificar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNotificar.Location = New System.Drawing.Point(582, 3)
        Me.btnNotificar.MinimumSize = New System.Drawing.Size(90, 32)
        Me.btnNotificar.Name = "btnNotificar"
        Me.btnNotificar.Size = New System.Drawing.Size(190, 32)
        Me.btnNotificar.TabIndex = 3
        Me.btnNotificar.Text = "Notificar"
        Me.btnNotificar.UseVisualStyleBackColor = True
        '
        'btnNovedades
        '
        Me.btnNovedades.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnNovedades.Location = New System.Drawing.Point(389, 3)
        Me.btnNovedades.MinimumSize = New System.Drawing.Size(90, 32)
        Me.btnNovedades.Name = "btnNovedades"
        Me.btnNovedades.Size = New System.Drawing.Size(187, 32)
        Me.btnNovedades.TabIndex = 2
        Me.btnNovedades.Text = "Novedades"
        Me.btnNovedades.UseVisualStyleBackColor = True
        '
        'btnSancionar
        '
        Me.btnSancionar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnSancionar.Location = New System.Drawing.Point(196, 3)
        Me.btnSancionar.MinimumSize = New System.Drawing.Size(90, 32)
        Me.btnSancionar.Name = "btnSancionar"
        Me.btnSancionar.Size = New System.Drawing.Size(187, 32)
        Me.btnSancionar.TabIndex = 1
        Me.btnSancionar.Text = "Sancionar"
        Me.btnSancionar.UseVisualStyleBackColor = True
        '
        'btnGenerarFicha
        '
        Me.btnGenerarFicha.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnGenerarFicha.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGenerarFicha.Location = New System.Drawing.Point(3, 3)
        Me.btnGenerarFicha.MinimumSize = New System.Drawing.Size(90, 32)
        Me.btnGenerarFicha.Name = "btnGenerarFicha"
        Me.btnGenerarFicha.Size = New System.Drawing.Size(187, 32)
        Me.btnGenerarFicha.TabIndex = 0
        Me.btnGenerarFicha.Text = "Ficha"
        Me.btnGenerarFicha.UseVisualStyleBackColor = True
        Me.btnGenerarFicha.Visible = False
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
        Me.tlpAcciones.Size = New System.Drawing.Size(775, 38)
        Me.tlpAcciones.TabIndex = 3
        '
        'frmFuncionarioBuscar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1097, 673)
        Me.Controls.Add(Me.splitContenedor)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frmFuncionarioBuscar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Buscar Funcionario"
        Me.splitContenedor.Panel1.ResumeLayout(False)
        Me.splitContenedor.Panel2.ResumeLayout(False)
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedor.ResumeLayout(False)
        CType(Me.dgvFuncionarios, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelBusquedaLista.ResumeLayout(False)
        Me.tlpBusqueda.ResumeLayout(False)
        Me.tlpBusqueda.PerformLayout()
        Me.panelDetalle.ResumeLayout(False)
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.tlpDetalleVertical.ResumeLayout(False)
        Me.tlpDetalleVertical.PerformLayout()
        Me.tlpAcciones.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents splitContenedor As SplitContainer
    Friend WithEvents dgvFuncionarios As DataGridView
    Friend WithEvents panelDetalle As Panel
    Friend WithEvents pbFotoDetalle As PictureBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents PanelBusquedaLista As Panel
    Friend WithEvents txtBusqueda As TextBox
    Friend WithEvents lblBuscar As Label
    Friend WithEvents tlpBusqueda As TableLayoutPanel
    Friend WithEvents lblNombreCompleto As Label
    Friend WithEvents lblHorarioCompleto As Label
    Friend WithEvents lblTipo As Label
    Friend WithEvents btnVerSituacion As Button
    Friend WithEvents lblFechaIngreso As Label
    Friend WithEvents lblCI As Label
    Friend WithEvents lblCargo As Label
    Friend WithEvents lblPresencia As Label
    Friend WithEvents lblEstadoActividad As Label
    Friend WithEvents btnCopiarContenido As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents tlpDetalleVertical As TableLayoutPanel
    Friend WithEvents tlpAcciones As TableLayoutPanel
    Friend WithEvents btnGenerarFicha As Button
    Friend WithEvents btnSancionar As Button
    Friend WithEvents btnNovedades As Button
    Friend WithEvents btnNotificar As Button
End Class
