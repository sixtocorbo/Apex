<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioBuscar
    Inherits System.Windows.Forms.Form

#Region "Dispose"
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.splitContenedor = New System.Windows.Forms.SplitContainer()
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.panelDetalle = New System.Windows.Forms.Panel()
        Me.chkActivoDetalle = New System.Windows.Forms.CheckBox()
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
        Me.lblEstadoTransitorio = New System.Windows.Forms.Label()
        Me.lblEstadoTransitorioHeader = New System.Windows.Forms.Label()
        Me.panelFiltros.SuspendLayout()
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedor.Panel1.SuspendLayout()
        Me.splitContenedor.Panel2.SuspendLayout()
        Me.splitContenedor.SuspendLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelDetalle.SuspendLayout()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panelFiltros
        '
        Me.panelFiltros.BackColor = System.Drawing.Color.WhiteSmoke
        Me.panelFiltros.Controls.Add(Me.btnBuscar)
        Me.panelFiltros.Controls.Add(Me.txtBusqueda)
        Me.panelFiltros.Controls.Add(Me.lblBuscar)
        Me.panelFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelFiltros.Location = New System.Drawing.Point(0, 0)
        Me.panelFiltros.Margin = New System.Windows.Forms.Padding(4)
        Me.panelFiltros.Name = "panelFiltros"
        Me.panelFiltros.Size = New System.Drawing.Size(1499, 74)
        Me.panelFiltros.TabIndex = 0
        '
        'btnBuscar
        '
        Me.btnBuscar.Location = New System.Drawing.Point(413, 22)
        Me.btnBuscar.Margin = New System.Windows.Forms.Padding(4)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(113, 31)
        Me.btnBuscar.TabIndex = 2
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'txtBusqueda
        '
        Me.txtBusqueda.Location = New System.Drawing.Point(88, 25)
        Me.txtBusqueda.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(316, 22)
        Me.txtBusqueda.TabIndex = 1
        '
        'lblBuscar
        '
        Me.lblBuscar.AutoSize = True
        Me.lblBuscar.Location = New System.Drawing.Point(23, 28)
        Me.lblBuscar.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblBuscar.Name = "lblBuscar"
        Me.lblBuscar.Size = New System.Drawing.Size(52, 16)
        Me.lblBuscar.TabIndex = 0
        Me.lblBuscar.Text = "Buscar:"
        '
        'splitContenedor
        '
        Me.splitContenedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContenedor.Location = New System.Drawing.Point(0, 74)
        Me.splitContenedor.Margin = New System.Windows.Forms.Padding(4)
        Me.splitContenedor.Name = "splitContenedor"
        '
        'splitContenedor.Panel1
        '
        Me.splitContenedor.Panel1.Controls.Add(Me.dgvResultados)
        '
        'splitContenedor.Panel2
        '
        Me.splitContenedor.Panel2.Controls.Add(Me.panelDetalle)
        Me.splitContenedor.Size = New System.Drawing.Size(1499, 806)
        Me.splitContenedor.SplitterDistance = 866
        Me.splitContenedor.SplitterWidth = 5
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
        Me.dgvResultados.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvResultados.MultiSelect = False
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.ReadOnly = True
        Me.dgvResultados.RowHeadersWidth = 51
        Me.dgvResultados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResultados.Size = New System.Drawing.Size(866, 806)
        Me.dgvResultados.TabIndex = 0
        '
        'panelDetalle
        '
        Me.panelDetalle.BackColor = System.Drawing.Color.White
        Me.panelDetalle.Controls.Add(Me.lblEstadoTransitorio)
        Me.panelDetalle.Controls.Add(Me.lblEstadoTransitorioHeader)
        Me.panelDetalle.Controls.Add(Me.chkActivoDetalle)
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
        Me.panelDetalle.Margin = New System.Windows.Forms.Padding(4)
        Me.panelDetalle.Name = "panelDetalle"
        Me.panelDetalle.Padding = New System.Windows.Forms.Padding(13, 12, 13, 12)
        Me.panelDetalle.Size = New System.Drawing.Size(628, 806)
        Me.panelDetalle.TabIndex = 0
        '
        'chkActivoDetalle
        '
        Me.chkActivoDetalle.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkActivoDetalle.AutoSize = True
        Me.chkActivoDetalle.Enabled = False
        Me.chkActivoDetalle.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.chkActivoDetalle.Location = New System.Drawing.Point(31, 763)
        Me.chkActivoDetalle.Margin = New System.Windows.Forms.Padding(4)
        Me.chkActivoDetalle.Name = "chkActivoDetalle"
        Me.chkActivoDetalle.Size = New System.Drawing.Size(79, 27)
        Me.chkActivoDetalle.TabIndex = 11
        Me.chkActivoDetalle.Text = "Activo"
        Me.chkActivoDetalle.UseVisualStyleBackColor = True
        '
        'lblPresencia
        '
        Me.lblPresencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPresencia.AutoSize = True
        Me.lblPresencia.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblPresencia.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblPresencia.Location = New System.Drawing.Point(160, 697)
        Me.lblPresencia.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPresencia.Name = "lblPresencia"
        Me.lblPresencia.Size = New System.Drawing.Size(17, 23)
        Me.lblPresencia.TabIndex = 10
        Me.lblPresencia.Text = "-"
        '
        'lblPresenciaHeader
        '
        Me.lblPresenciaHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblPresenciaHeader.AutoSize = True
        Me.lblPresenciaHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblPresenciaHeader.Location = New System.Drawing.Point(27, 697)
        Me.lblPresenciaHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPresenciaHeader.Name = "lblPresenciaHeader"
        Me.lblPresenciaHeader.Size = New System.Drawing.Size(86, 23)
        Me.lblPresenciaHeader.TabIndex = 9
        Me.lblPresenciaHeader.Text = "Presencia:"
        '
        'lblFechaIngreso
        '
        Me.lblFechaIngreso.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFechaIngreso.AutoSize = True
        Me.lblFechaIngreso.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblFechaIngreso.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblFechaIngreso.Location = New System.Drawing.Point(160, 666)
        Me.lblFechaIngreso.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaIngreso.Name = "lblFechaIngreso"
        Me.lblFechaIngreso.Size = New System.Drawing.Size(17, 23)
        Me.lblFechaIngreso.TabIndex = 8
        Me.lblFechaIngreso.Text = "-"
        '
        'lblFechaIngresoHeader
        '
        Me.lblFechaIngresoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblFechaIngresoHeader.AutoSize = True
        Me.lblFechaIngresoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblFechaIngresoHeader.Location = New System.Drawing.Point(27, 666)
        Me.lblFechaIngresoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaIngresoHeader.Name = "lblFechaIngresoHeader"
        Me.lblFechaIngresoHeader.Size = New System.Drawing.Size(121, 23)
        Me.lblFechaIngresoHeader.TabIndex = 7
        Me.lblFechaIngresoHeader.Text = "Fecha Ingreso:"
        '
        'lblTipo
        '
        Me.lblTipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblTipo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblTipo.Location = New System.Drawing.Point(160, 635)
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(17, 23)
        Me.lblTipo.TabIndex = 6
        Me.lblTipo.Text = "-"
        '
        'lblTipoHeader
        '
        Me.lblTipoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTipoHeader.AutoSize = True
        Me.lblTipoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblTipoHeader.Location = New System.Drawing.Point(27, 635)
        Me.lblTipoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTipoHeader.Name = "lblTipoHeader"
        Me.lblTipoHeader.Size = New System.Drawing.Size(47, 23)
        Me.lblTipoHeader.TabIndex = 5
        Me.lblTipoHeader.Text = "Tipo:"
        '
        'lblCargo
        '
        Me.lblCargo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCargo.AutoSize = True
        Me.lblCargo.Font = New System.Drawing.Font("Segoe UI", 9.75!)
        Me.lblCargo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblCargo.Location = New System.Drawing.Point(160, 604)
        Me.lblCargo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCargo.Name = "lblCargo"
        Me.lblCargo.Size = New System.Drawing.Size(17, 23)
        Me.lblCargo.TabIndex = 4
        Me.lblCargo.Text = "-"
        '
        'lblCargoHeader
        '
        Me.lblCargoHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblCargoHeader.AutoSize = True
        Me.lblCargoHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblCargoHeader.Location = New System.Drawing.Point(27, 604)
        Me.lblCargoHeader.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCargoHeader.Name = "lblCargoHeader"
        Me.lblCargoHeader.Size = New System.Drawing.Size(60, 23)
        Me.lblCargoHeader.TabIndex = 3
        Me.lblCargoHeader.Text = "Cargo:"
        '
        'lblNombreCompleto
        '
        Me.lblNombreCompleto.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblNombreCompleto.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNombreCompleto.Location = New System.Drawing.Point(17, 561)
        Me.lblNombreCompleto.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNombreCompleto.Name = "lblNombreCompleto"
        Me.lblNombreCompleto.Size = New System.Drawing.Size(593, 31)
        Me.lblNombreCompleto.TabIndex = 2
        Me.lblNombreCompleto.Text = "Nombre del Funcionario"
        Me.lblNombreCompleto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblCI
        '
        Me.lblCI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCI.Font = New System.Drawing.Font("Segoe UI", 11.25!)
        Me.lblCI.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblCI.Location = New System.Drawing.Point(17, 536)
        Me.lblCI.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblCI.Name = "lblCI"
        Me.lblCI.Size = New System.Drawing.Size(593, 25)
        Me.lblCI.TabIndex = 1
        Me.lblCI.Text = "1234567-8"
        Me.lblCI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pbFotoDetalle
        '
        Me.pbFotoDetalle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbFotoDetalle.Location = New System.Drawing.Point(17, 16)
        Me.pbFotoDetalle.Margin = New System.Windows.Forms.Padding(4)
        Me.pbFotoDetalle.Name = "pbFotoDetalle"
        Me.pbFotoDetalle.Size = New System.Drawing.Size(593, 503)
        Me.pbFotoDetalle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFotoDetalle.TabIndex = 0
        Me.pbFotoDetalle.TabStop = False
        '
        'lblEstadoTransitorio
        '
        Me.lblEstadoTransitorio.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblEstadoTransitorio.AutoSize = True
        Me.lblEstadoTransitorio.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblEstadoTransitorio.ForeColor = System.Drawing.Color.Maroon
        Me.lblEstadoTransitorio.Location = New System.Drawing.Point(160, 727)
        Me.lblEstadoTransitorio.Name = "lblEstadoTransitorio"
        Me.lblEstadoTransitorio.Size = New System.Drawing.Size(17, 23)
        Me.lblEstadoTransitorio.Text = "-"
        '
        'lblEstadoTransitorioHeader
        '
        Me.lblEstadoTransitorioHeader.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblEstadoTransitorioHeader.AutoSize = True
        Me.lblEstadoTransitorioHeader.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.lblEstadoTransitorioHeader.Location = New System.Drawing.Point(27, 727)
        Me.lblEstadoTransitorioHeader.Name = "lblEstadoTransitorioHeader"
        Me.lblEstadoTransitorioHeader.Size = New System.Drawing.Size(115, 23)
        Me.lblEstadoTransitorioHeader.Text = "Estado Actual:"
        '
        'frmFuncionarioBuscar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1499, 880)
        Me.Controls.Add(Me.splitContenedor)
        Me.Controls.Add(Me.panelFiltros)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmFuncionarioBuscar"
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
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents panelFiltros As System.Windows.Forms.Panel
    Friend WithEvents lblBuscar As System.Windows.Forms.Label
    Friend WithEvents txtBusqueda As System.Windows.Forms.TextBox
    Friend WithEvents btnBuscar As System.Windows.Forms.Button
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
    Friend WithEvents chkActivoDetalle As System.Windows.Forms.CheckBox
    Friend WithEvents lblEstadoTransitorio As System.Windows.Forms.Label
    Friend WithEvents lblEstadoTransitorioHeader As System.Windows.Forms.Label

End Class