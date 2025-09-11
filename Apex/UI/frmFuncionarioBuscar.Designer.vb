<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioBuscar
    Inherits System.Windows.Forms.Form

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
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.PanelBusquedaLista = New System.Windows.Forms.Panel()
        Me.tlpBusqueda = New System.Windows.Forms.TableLayoutPanel()
        Me.lblBuscar = New System.Windows.Forms.Label()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGenerarFicha = New System.Windows.Forms.Button()
        Me.btnSancionar = New System.Windows.Forms.Button()
        Me.btnNovedades = New System.Windows.Forms.Button()
        Me.btnNotificar = New System.Windows.Forms.Button()
        Me.panelDetalle = New System.Windows.Forms.Panel()
        Me.tlpDetalleVertical = New System.Windows.Forms.TableLayoutPanel()
        Me.pbFotoDetalle = New System.Windows.Forms.PictureBox()
        Me.lblNombreCompleto = New System.Windows.Forms.Label()
        Me.lblCI = New System.Windows.Forms.Label()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.pbCopyCI = New System.Windows.Forms.PictureBox()
        Me.pbCopyNombre = New System.Windows.Forms.PictureBox()
        Me.btnVerSituacion = New System.Windows.Forms.Button()
        Me.lblPresencia = New System.Windows.Forms.Label()
        Me.lblFechaIngreso = New System.Windows.Forms.Label()
        Me.lblHorarioCompleto = New System.Windows.Forms.Label()
        Me.lblCargo = New System.Windows.Forms.Label()
        Me.lblEstadoActividad = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedor.Panel1.SuspendLayout()
        Me.splitContenedor.Panel2.SuspendLayout()
        Me.splitContenedor.SuspendLayout()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelBusquedaLista.SuspendLayout()
        Me.tlpBusqueda.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.panelDetalle.SuspendLayout()
        Me.tlpDetalleVertical.SuspendLayout()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCopyCI, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCopyNombre, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.splitContenedor.Panel1.Controls.Add(Me.dgvResultados)
        Me.splitContenedor.Panel1.Controls.Add(Me.PanelBusquedaLista)
        Me.splitContenedor.Panel1MinSize = 280
        '
        'splitContenedor.Panel2
        '
        Me.splitContenedor.Panel2.Controls.Add(Me.panelDetalle)
        Me.splitContenedor.Panel2MinSize = 400
        Me.splitContenedor.Size = New System.Drawing.Size(1121, 544)
        Me.splitContenedor.SplitterDistance = 301
        Me.splitContenedor.TabIndex = 0
        '
        'dgvResultados
        '
        Me.dgvResultados.ColumnHeadersHeight = 34
        Me.dgvResultados.Location = New System.Drawing.Point(0, 268)
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.RowHeadersWidth = 62
        Me.dgvResultados.Size = New System.Drawing.Size(298, 276)
        Me.dgvResultados.TabIndex = 1
        '
        'PanelBusquedaLista
        '
        Me.PanelBusquedaLista.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelBusquedaLista.Controls.Add(Me.tlpBusqueda)
        Me.PanelBusquedaLista.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaLista.Location = New System.Drawing.Point(0, 0)
        Me.PanelBusquedaLista.Name = "PanelBusquedaLista"
        Me.PanelBusquedaLista.Padding = New System.Windows.Forms.Padding(12)
        Me.PanelBusquedaLista.Size = New System.Drawing.Size(301, 72)
        Me.PanelBusquedaLista.TabIndex = 0
        '
        'tlpBusqueda
        '
        Me.tlpBusqueda.ColumnCount = 2
        Me.tlpBusqueda.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpBusqueda.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBusqueda.Controls.Add(Me.lblBuscar, 0, 0)
        Me.tlpBusqueda.Controls.Add(Me.txtBusqueda, 1, 0)
        Me.tlpBusqueda.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpBusqueda.Location = New System.Drawing.Point(12, 12)
        Me.tlpBusqueda.Name = "tlpBusqueda"
        Me.tlpBusqueda.RowCount = 1
        Me.tlpBusqueda.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBusqueda.Size = New System.Drawing.Size(277, 48)
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
        Me.txtBusqueda.Size = New System.Drawing.Size(188, 33)
        Me.txtBusqueda.TabIndex = 1
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnGenerarFicha)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSancionar)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNovedades)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNotificar)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(738, 45)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'btnGenerarFicha
        '
        Me.btnGenerarFicha.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnGenerarFicha.Location = New System.Drawing.Point(582, 3)
        Me.btnGenerarFicha.Name = "btnGenerarFicha"
        Me.btnGenerarFicha.Size = New System.Drawing.Size(153, 36)
        Me.btnGenerarFicha.TabIndex = 16
        Me.btnGenerarFicha.Text = "Ficha"
        Me.btnGenerarFicha.UseVisualStyleBackColor = True
        Me.btnGenerarFicha.Visible = False
        '
        'btnSancionar
        '
        Me.btnSancionar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnSancionar.Location = New System.Drawing.Point(423, 3)
        Me.btnSancionar.Name = "btnSancionar"
        Me.btnSancionar.Size = New System.Drawing.Size(153, 36)
        Me.btnSancionar.TabIndex = 1
        Me.btnSancionar.Text = "Sancionar"
        Me.btnSancionar.UseVisualStyleBackColor = True
        '
        'btnNovedades
        '
        Me.btnNovedades.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNovedades.Location = New System.Drawing.Point(264, 3)
        Me.btnNovedades.Name = "btnNovedades"
        Me.btnNovedades.Size = New System.Drawing.Size(153, 36)
        Me.btnNovedades.TabIndex = 2
        Me.btnNovedades.Text = "Novedades"
        Me.btnNovedades.UseVisualStyleBackColor = True
        '
        'btnNotificar
        '
        Me.btnNotificar.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnNotificar.Location = New System.Drawing.Point(105, 3)
        Me.btnNotificar.Name = "btnNotificar"
        Me.btnNotificar.Size = New System.Drawing.Size(153, 36)
        Me.btnNotificar.TabIndex = 0
        Me.btnNotificar.Text = "Notificar"
        Me.btnNotificar.UseVisualStyleBackColor = True
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
        Me.panelDetalle.Size = New System.Drawing.Size(816, 544)
        Me.panelDetalle.TabIndex = 0
        '
        'tlpDetalleVertical
        '
        Me.tlpDetalleVertical.ColumnCount = 2
        Me.tlpDetalleVertical.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpDetalleVertical.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 48.0!))
        Me.tlpDetalleVertical.Controls.Add(Me.pbFotoDetalle, 0, 1)
        Me.tlpDetalleVertical.Controls.Add(Me.lblNombreCompleto, 0, 4)
        Me.tlpDetalleVertical.Controls.Add(Me.FlowLayoutPanel1, 0, 0)
        Me.tlpDetalleVertical.Controls.Add(Me.lblCI, 0, 5)
        Me.tlpDetalleVertical.Controls.Add(Me.lblTipo, 0, 6)
        Me.tlpDetalleVertical.Controls.Add(Me.pbCopyCI, 1, 5)
        Me.tlpDetalleVertical.Controls.Add(Me.pbCopyNombre, 1, 4)
        Me.tlpDetalleVertical.Controls.Add(Me.btnVerSituacion, 0, 3)
        Me.tlpDetalleVertical.Controls.Add(Me.lblPresencia, 0, 7)
        Me.tlpDetalleVertical.Controls.Add(Me.lblFechaIngreso, 0, 8)
        Me.tlpDetalleVertical.Controls.Add(Me.lblHorarioCompleto, 0, 9)
        Me.tlpDetalleVertical.Controls.Add(Me.lblCargo, 0, 10)
        Me.tlpDetalleVertical.Controls.Add(Me.lblEstadoActividad, 0, 11)
        Me.tlpDetalleVertical.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpDetalleVertical.Location = New System.Drawing.Point(12, 12)
        Me.tlpDetalleVertical.Name = "tlpDetalleVertical"
        Me.tlpDetalleVertical.RowCount = 12
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalleVertical.Size = New System.Drawing.Size(792, 520)
        Me.tlpDetalleVertical.TabIndex = 0
        '
        'pbFotoDetalle
        '
        Me.pbFotoDetalle.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbFotoDetalle.Location = New System.Drawing.Point(3, 54)
        Me.pbFotoDetalle.Margin = New System.Windows.Forms.Padding(3, 3, 3, 16)
        Me.pbFotoDetalle.Name = "pbFotoDetalle"
        Me.pbFotoDetalle.Size = New System.Drawing.Size(738, 155)
        Me.pbFotoDetalle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFotoDetalle.TabIndex = 0
        Me.pbFotoDetalle.TabStop = False
        '
        'lblNombreCompleto
        '
        Me.lblNombreCompleto.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblNombreCompleto.AutoSize = True
        Me.lblNombreCompleto.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblNombreCompleto.Location = New System.Drawing.Point(3, 275)
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
        Me.lblCI.Location = New System.Drawing.Point(3, 321)
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
        Me.lblTipo.Location = New System.Drawing.Point(3, 357)
        Me.lblTipo.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(66, 25)
        Me.lblTipo.TabIndex = 6
        Me.lblTipo.Text = "Tipo: -"
        '
        'pbCopyCI
        '
        Me.pbCopyCI.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCopyCI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbCopyCI.Location = New System.Drawing.Point(747, 324)
        Me.pbCopyCI.Name = "pbCopyCI"
        Me.pbCopyCI.Size = New System.Drawing.Size(42, 30)
        Me.pbCopyCI.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbCopyCI.TabIndex = 18
        Me.pbCopyCI.TabStop = False
        Me.pbCopyCI.Visible = False
        '
        'pbCopyNombre
        '
        Me.pbCopyNombre.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCopyNombre.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbCopyNombre.Location = New System.Drawing.Point(747, 270)
        Me.pbCopyNombre.Name = "pbCopyNombre"
        Me.pbCopyNombre.Size = New System.Drawing.Size(42, 48)
        Me.pbCopyNombre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbCopyNombre.TabIndex = 19
        Me.pbCopyNombre.TabStop = False
        Me.pbCopyNombre.Visible = False
        '
        'btnVerSituacion
        '
        Me.btnVerSituacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.btnVerSituacion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.btnVerSituacion.Location = New System.Drawing.Point(3, 228)
        Me.btnVerSituacion.Name = "btnVerSituacion"
        Me.btnVerSituacion.Size = New System.Drawing.Size(738, 36)
        Me.btnVerSituacion.TabIndex = 17
        Me.btnVerSituacion.Text = "Situación"
        Me.btnVerSituacion.UseVisualStyleBackColor = True
        Me.btnVerSituacion.Visible = False
        '
        'lblPresencia
        '
        Me.lblPresencia.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblPresencia.AutoSize = True
        Me.lblPresencia.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblPresencia.ForeColor = System.Drawing.Color.DimGray
        Me.lblPresencia.Location = New System.Drawing.Point(3, 384)
        Me.lblPresencia.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblPresencia.Name = "lblPresencia"
        Me.lblPresencia.Size = New System.Drawing.Size(20, 25)
        Me.lblPresencia.TabIndex = 10
        Me.lblPresencia.Text = "-"
        '
        'lblFechaIngreso
        '
        Me.lblFechaIngreso.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblFechaIngreso.AutoSize = True
        Me.lblFechaIngreso.Font = New System.Drawing.Font("Segoe UI", 9.5!)
        Me.lblFechaIngreso.ForeColor = System.Drawing.Color.DimGray
        Me.lblFechaIngreso.Location = New System.Drawing.Point(3, 411)
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
        Me.lblHorarioCompleto.Location = New System.Drawing.Point(3, 438)
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
        Me.lblCargo.Location = New System.Drawing.Point(3, 465)
        Me.lblCargo.Margin = New System.Windows.Forms.Padding(3, 0, 3, 2)
        Me.lblCargo.Name = "lblCargo"
        Me.lblCargo.Size = New System.Drawing.Size(80, 25)
        Me.lblCargo.TabIndex = 4
        Me.lblCargo.Text = "Cargo: -"
        '
        'lblEstadoActividad
        '
        Me.lblEstadoActividad.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblEstadoActividad.AutoSize = True
        Me.lblEstadoActividad.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblEstadoActividad.Location = New System.Drawing.Point(3, 492)
        Me.lblEstadoActividad.Name = "lblEstadoActividad"
        Me.lblEstadoActividad.Size = New System.Drawing.Size(94, 28)
        Me.lblEstadoActividad.TabIndex = 14
        Me.lblEstadoActividad.Text = "Estado: -"
        '
        'frmFuncionarioBuscar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1121, 544)
        Me.Controls.Add(Me.splitContenedor)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(800, 600)
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
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.panelDetalle.ResumeLayout(False)
        Me.tlpDetalleVertical.ResumeLayout(False)
        Me.tlpDetalleVertical.PerformLayout()
        CType(Me.pbFotoDetalle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCopyCI, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCopyNombre, System.ComponentModel.ISupportInitialize).EndInit()
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