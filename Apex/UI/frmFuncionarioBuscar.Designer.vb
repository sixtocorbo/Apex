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

    '======================================================================
    '                       ¡InitializeComponent!
    '======================================================================
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.panelFiltros = New System.Windows.Forms.Panel()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.splitContenedor = New System.Windows.Forms.SplitContainer()
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.panelDetalle = New System.Windows.Forms.Panel()
        Me.btnVerSituacion = New System.Windows.Forms.Button()
        Me.btnGenerarFicha = New System.Windows.Forms.Button()
        Me.lblEstadoActividad = New System.Windows.Forms.Label()
        Me.lblHorarioCompleto = New System.Windows.Forms.Label()
        Me.lblHorarioCompletoHeader = New System.Windows.Forms.Label()
        Me.lblPresencia = New System.Windows.Forms.Label()
        Me.lblPresenciaHeader = New System.Windows.Forms.Label()
        Me.lblFechaIngreso = New System.Windows.Forms.Label()
        Me.lblFechaIngresoHeader = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.lblTipoHeader = New System.Windows.Forms.Label()
        Me.lblCargo = New System.Windows.Forms.Label()
        Me.lblCargoHeader = New System.Windows.Forms.Label()
        Me.lblNombreCompleto = New System.Windows.Forms.Label()
        Me.lblCI = New System.Windows.Forms.Label()
        Me.pbFotoDetalle = New System.Windows.Forms.PictureBox()
        Me.FlowLayoutPanelAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSeleccionar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.panelFiltros.SuspendLayout()
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedor.Panel1.SuspendLayout()
        Me.splitContenedor.Panel2.SuspendLayout()
        Me.splitContenedor.SuspendLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelDetalle.SuspendLayout()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanelAcciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelFiltros
        '
        Me.panelFiltros.BackColor = System.Drawing.Color.WhiteSmoke
        Me.panelFiltros.Controls.Add(Me.txtBusqueda)
        Me.panelFiltros.Controls.Add(Me.lblBuscar)
        Me.panelFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelFiltros.Location = New System.Drawing.Point(0, 0)
        Me.panelFiltros.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.panelFiltros.Name = "panelFiltros"
        Me.panelFiltros.Size = New System.Drawing.Size(1686, 92)
        Me.panelFiltros.TabIndex = 0
        '
        'txtBusqueda
        '
        Me.txtBusqueda.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtBusqueda.Location = New System.Drawing.Point(99, 31)
        Me.txtBusqueda.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(355, 26)
        Me.txtBusqueda.TabIndex = 1
        '
        'lblBuscar
        '
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Location = New System.Drawing.Point(26, 35)
        Me.lblBuscar.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(63, 20)
        Me.lblBuscar.TabIndex = 0
        Me.lblBuscar.Text = "Buscar:"
        '
        'splitContenedor
        '
        Me.splitContenedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContenedor.Location = New System.Drawing.Point(0, 92)
        Me.splitContenedor.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.splitContenedor.Name = "splitContenedor"
        '
        'splitContenedor.Panel1
        '
        Me.splitContenedor.Panel1.Controls.Add(Me.dgvResultados)
        '
        'splitContenedor.Panel2
        '
        Me.splitContenedor.Panel2.Controls.Add(Me.panelDetalle)
        Me.splitContenedor.Size = New System.Drawing.Size(1686, 958)
        Me.splitContenedor.SplitterDistance = 590
        Me.splitContenedor.SplitterWidth = 6
        Me.splitContenedor.TabIndex = 1
        '
        'dgvResultados
        '
        Me.dgvResultados.AllowUserToAddRows = False
        Me.dgvResultados.AllowUserToDeleteRows = False
        Me.dgvResultados.AllowUserToResizeRows = False
        Me.dgvResultados.BackgroundColor = System.Drawing.Color.White
        Me.dgvResultados.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResultados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvResultados.Location = New System.Drawing.Point(0, 0)
        Me.dgvResultados.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvResultados.MultiSelect = False
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.ReadOnly = True
        Me.dgvResultados.RowHeadersWidth = 51
        Me.dgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResultados.Size = New System.Drawing.Size(590, 958)
        Me.dgvResultados.TabIndex = 0
        '
        'panelDetalle
        '
        Me.panelDetalle.BackColor = System.Drawing.Color.White
        Me.panelDetalle.Controls.Add(Me.btnVerSituacion)
        Me.panelDetalle.Controls.Add(Me.btnGenerarFicha)
        Me.panelDetalle.Controls.Add(Me.lblEstadoActividad)
        Me.panelDetalle.Controls.Add(Me.lblHorarioCompleto)
        Me.panelDetalle.Controls.Add(Me.lblHorarioCompletoHeader)
        Me.panelDetalle.Controls.Add(Me.lblPresencia)
        Me.panelDetalle.Controls.Add(Me.lblPresenciaHeader)
        Me.panelDetalle.Controls.Add(Me.lblFechaIngreso)
        Me.panelDetalle.Controls.Add(Me.lblFechaIngresoHeader)
        Me.panelDetalle.Controls.Add(Me.lblTipo)
        Me.panelDetalle.Controls.Add(Me.lblTipoHeader)
        Me.panelDetalle.Controls.Add(Me.lblCargo)
        Me.panelDetalle.Controls.Add(Me.lblCargoHeader)
        Me.panelDetalle.Controls.Add(Me.lblNombreCompleto)
        Me.panelDetalle.Controls.Add(Me.lblCI)
        Me.panelDetalle.Controls.Add(Me.pbFotoDetalle)
        Me.panelDetalle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelDetalle.Location = New System.Drawing.Point(0, 0)
        Me.panelDetalle.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.panelDetalle.Name = "panelDetalle"
        Me.panelDetalle.Padding = New System.Windows.Forms.Padding(15)
        Me.panelDetalle.Size = New System.Drawing.Size(1090, 958)
        Me.panelDetalle.TabIndex = 0
        '
        'btnVerSituacion
        '
        Me.btnVerSituacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnVerSituacion.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnVerSituacion.Location = New System.Drawing.Point(34, 853)
        Me.btnVerSituacion.Name = "btnVerSituacion"
        Me.btnVerSituacion.Size = New System.Drawing.Size(250, 38)
        Me.btnVerSituacion.TabIndex = 17
        Me.btnVerSituacion.Text = "Ver Situación"
        Me.btnVerSituacion.UseVisualStyleBackColor = True
        Me.btnVerSituacion.Visible = False
        '
        'btnGenerarFicha
        '
        Me.btnGenerarFicha.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnGenerarFicha.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.btnGenerarFicha.Location = New System.Drawing.Point(290, 853)
        Me.btnGenerarFicha.Name = "btnGenerarFicha"
        Me.btnGenerarFicha.Size = New System.Drawing.Size(250, 38)
        Me.btnGenerarFicha.TabIndex = 16
        Me.btnGenerarFicha.Text = "Ficha"
        Me.btnGenerarFicha.UseVisualStyleBackColor = True
        Me.btnGenerarFicha.Visible = False
        '
        'lblEstadoActividad
        '
        Me.lblEstadoActividad.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblEstadoActividad.AutoSize = True
        Me.lblEstadoActividad.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblEstadoActividad.Location = New System.Drawing.Point(30, 904)
        Me.lblEstadoActividad.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblEstadoActividad.Name = "lblEstadoActividad"
        Me.lblEstadoActividad.Size = New System.Drawing.Size(94, 28)
        Me.lblEstadoActividad.TabIndex = 14
        Me.lblEstadoActividad.Text = "Estado: -"
        '
        'lblHorarioCompleto
        '
        Me.lblHorarioCompleto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblHorarioCompleto.AutoSize = True
        Me.lblHorarioCompleto.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblHorarioCompleto.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblHorarioCompleto.Location = New System.Drawing.Point(180, 783)
        Me.lblHorarioCompleto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHorarioCompleto.Name = "lblHorarioCompleto"
        Me.lblHorarioCompleto.Size = New System.Drawing.Size(20, 28)
        Me.lblHorarioCompleto.TabIndex = 13
        Me.lblHorarioCompleto.Text = "-"
        '
        'lblHorarioCompletoHeader
        '
        Me.lblHorarioCompletoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblHorarioCompletoHeader.AutoSize = True
        Me.lblHorarioCompletoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblHorarioCompletoHeader.Location = New System.Drawing.Point(30, 783)
        Me.lblHorarioCompletoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHorarioCompletoHeader.Name = "lblHorarioCompletoHeader"
        Me.lblHorarioCompletoHeader.Size = New System.Drawing.Size(85, 28)
        Me.lblHorarioCompletoHeader.TabIndex = 12
        Me.lblHorarioCompletoHeader.Text = "Horario:"
        '
        'lblPresencia
        '
        Me.lblPresencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPresencia.AutoSize = True
        Me.lblPresencia.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblPresencia.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblPresencia.Location = New System.Drawing.Point(180, 821)
        Me.lblPresencia.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPresencia.Name = "lblPresencia"
        Me.lblPresencia.Size = New System.Drawing.Size(20, 28)
        Me.lblPresencia.TabIndex = 10
        Me.lblPresencia.Text = "-"
        '
        'lblPresenciaHeader
        '
        Me.lblPresenciaHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPresenciaHeader.AutoSize = True
        Me.lblPresenciaHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblPresenciaHeader.Location = New System.Drawing.Point(30, 821)
        Me.lblPresenciaHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPresenciaHeader.Name = "lblPresenciaHeader"
        Me.lblPresenciaHeader.Size = New System.Drawing.Size(103, 28)
        Me.lblPresenciaHeader.TabIndex = 9
        Me.lblPresenciaHeader.Text = "Presencia:"
        '
        'lblFechaIngreso
        '
        Me.lblFechaIngreso.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFechaIngreso.AutoSize = True
        Me.lblFechaIngreso.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblFechaIngreso.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblFechaIngreso.Location = New System.Drawing.Point(180, 744)
        Me.lblFechaIngreso.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaIngreso.Name = "lblFechaIngreso"
        Me.lblFechaIngreso.Size = New System.Drawing.Size(20, 28)
        Me.lblFechaIngreso.TabIndex = 8
        Me.lblFechaIngreso.Text = "-"
        '
        'lblFechaIngresoHeader
        '
        Me.lblFechaIngresoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFechaIngresoHeader.AutoSize = True
        Me.lblFechaIngresoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblFechaIngresoHeader.Location = New System.Drawing.Point(30, 744)
        Me.lblFechaIngresoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaIngresoHeader.Name = "lblFechaIngresoHeader"
        Me.lblFechaIngresoHeader.Size = New System.Drawing.Size(144, 28)
        Me.lblFechaIngresoHeader.TabIndex = 7
        Me.lblFechaIngresoHeader.Text = "Fecha Ingreso:"
        '
        'lblTipo
        '
        Me.lblTipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblTipo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTipo.Location = New System.Drawing.Point(180, 705)
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(20, 28)
        Me.lblTipo.TabIndex = 6
        Me.lblTipo.Text = "-"
        '
        'lblTipoHeader
        '
        Me.lblTipoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTipoHeader.AutoSize = True
        Me.lblTipoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblTipoHeader.Location = New System.Drawing.Point(30, 705)
        Me.lblTipoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTipoHeader.Name = "lblTipoHeader"
        Me.lblTipoHeader.Size = New System.Drawing.Size(57, 28)
        Me.lblTipoHeader.TabIndex = 5
        Me.lblTipoHeader.Text = "Tipo:"
        '
        'lblCargo
        '
        Me.lblCargo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCargo.AutoSize = True
        Me.lblCargo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCargo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblCargo.Location = New System.Drawing.Point(180, 667)
        Me.lblCargo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCargo.Name = "lblCargo"
        Me.lblCargo.Size = New System.Drawing.Size(20, 28)
        Me.lblCargo.TabIndex = 4
        Me.lblCargo.Text = "-"
        '
        'lblCargoHeader
        '
        Me.lblCargoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCargoHeader.AutoSize = True
        Me.lblCargoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblCargoHeader.Location = New System.Drawing.Point(30, 667)
        Me.lblCargoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCargoHeader.Name = "lblCargoHeader"
        Me.lblCargoHeader.Size = New System.Drawing.Size(70, 28)
        Me.lblCargoHeader.TabIndex = 3
        Me.lblCargoHeader.Text = "Cargo:"
        '
        'lblNombreCompleto
        '
        Me.lblNombreCompleto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNombreCompleto.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNombreCompleto.Location = New System.Drawing.Point(19, 613)
        Me.lblNombreCompleto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNombreCompleto.Name = "lblNombreCompleto"
        Me.lblNombreCompleto.Size = New System.Drawing.Size(1051, 39)
        Me.lblNombreCompleto.TabIndex = 2
        Me.lblNombreCompleto.Text = "Nombre: -"
        Me.lblNombreCompleto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCI
        '
        Me.lblCI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCI.Font = New System.Drawing.Font("Segoe UI", 11.25!)
        Me.lblCI.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblCI.Location = New System.Drawing.Point(19, 581)
        Me.lblCI.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCI.Name = "lblCI"
        Me.lblCI.Size = New System.Drawing.Size(1051, 31)
        Me.lblCI.TabIndex = 1
        Me.lblCI.Text = "CI: -"
        Me.lblCI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pbFotoDetalle
        '
        Me.pbFotoDetalle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbFotoDetalle.Location = New System.Drawing.Point(19, 20)
        Me.pbFotoDetalle.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pbFotoDetalle.Name = "pbFotoDetalle"
        Me.pbFotoDetalle.Size = New System.Drawing.Size(1051, 540)
        Me.pbFotoDetalle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFotoDetalle.TabIndex = 0
        Me.pbFotoDetalle.TabStop = False
        '
        'FlowLayoutPanelAcciones
        '
        Me.FlowLayoutPanelAcciones.AutoSize = True
        Me.FlowLayoutPanelAcciones.Controls.Add(Me.btnSeleccionar)
        Me.FlowLayoutPanelAcciones.Controls.Add(Me.btnCancelar)
        Me.FlowLayoutPanelAcciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanelAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanelAcciones.Location = New System.Drawing.Point(0, 1001)
        Me.FlowLayoutPanelAcciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.FlowLayoutPanelAcciones.Name = "FlowLayoutPanelAcciones"
        Me.FlowLayoutPanelAcciones.Padding = New System.Windows.Forms.Padding(6)
        Me.FlowLayoutPanelAcciones.Size = New System.Drawing.Size(1686, 49)
        Me.FlowLayoutPanelAcciones.TabIndex = 2
        '
        'btnSeleccionar
        '
        Me.btnSeleccionar.Location = New System.Drawing.Point(1559, 10)
        Me.btnSeleccionar.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnSeleccionar.Name = "btnSeleccionar"
        Me.btnSeleccionar.Size = New System.Drawing.Size(112, 29)
        Me.btnSeleccionar.TabIndex = 0
        Me.btnSeleccionar.Text = "Seleccionar"
        Me.btnSeleccionar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(1441, 10)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(112, 29)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmFuncionarioBuscar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1686, 1050)
        Me.Controls.Add(Me.FlowLayoutPanelAcciones)
        Me.Controls.Add(Me.splitContenedor)
        Me.Controls.Add(Me.panelFiltros)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmFuncionarioBuscar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Buscar Funcionario"
        Me.panelFiltros.ResumeLayout(False)
        Me.panelFiltros.PerformLayout()
        Me.splitContenedor.Panel1.ResumeLayout(False)
        Me.splitContenedor.Panel2.ResumeLayout(False)
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedor.ResumeLayout(False)
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelDetalle.ResumeLayout(False)
        Me.panelDetalle.PerformLayout()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanelAcciones.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents panelFiltros As System.Windows.Forms.Panel
    Friend WithEvents lblBuscar As System.Windows.Forms.Label
    Friend WithEvents txtBusqueda As System.Windows.Forms.TextBox
    Friend WithEvents splitContenedor As System.Windows.Forms.SplitContainer
    Friend WithEvents dgvResultados As System.Windows.Forms.DataGridView
    Friend WithEvents panelDetalle As System.Windows.Forms.Panel
    Friend WithEvents pbFotoDetalle As System.Windows.Forms.PictureBox
    Friend WithEvents lblCI As System.Windows.Forms.Label
    Friend WithEvents lblNombreCompleto As System.Windows.Forms.Label
    Friend WithEvents lblCargoHeader As System.Windows.Forms.Label
    Friend WithEvents lblCargo As System.Windows.Forms.Label
    Friend WithEvents lblTipoHeader As System.Windows.Forms.Label
    Friend WithEvents lblTipo As System.Windows.Forms.Label
    Friend WithEvents lblFechaIngresoHeader As System.Windows.Forms.Label
    Friend WithEvents lblFechaIngreso As System.Windows.Forms.Label
    Friend WithEvents lblPresencia As System.Windows.Forms.Label
    Friend WithEvents lblPresenciaHeader As System.Windows.Forms.Label
    Friend WithEvents FlowLayoutPanelAcciones As FlowLayoutPanel
    Friend WithEvents btnSeleccionar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents lblHorarioCompleto As Label
    Friend WithEvents lblHorarioCompletoHeader As Label
    Friend WithEvents lblEstadoActividad As Label
    Friend WithEvents btnGenerarFicha As Button
    Friend WithEvents btnVerSituacion As Button
End Class