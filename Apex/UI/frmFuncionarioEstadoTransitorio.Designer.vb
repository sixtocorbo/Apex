<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioEstadoTransitorio
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
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
        Me.cboTipoEstado = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblFechaDesde = New System.Windows.Forms.Label()
        Me.dtpFechaDesde = New System.Windows.Forms.DateTimePicker()
        Me.dtpFechaHasta = New System.Windows.Forms.DateTimePicker()
        Me.lblFechaHasta = New System.Windows.Forms.Label()
        Me.chkFechaHasta = New System.Windows.Forms.CheckBox()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblResolucion = New System.Windows.Forms.Label()
        Me.txtResolucion = New System.Windows.Forms.TextBox()
        Me.lblDiagnostico = New System.Windows.Forms.Label()
        Me.txtDiagnostico = New System.Windows.Forms.TextBox()
        Me.lblTurnoReten = New System.Windows.Forms.Label()
        Me.txtTurnoReten = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.dgvAdjuntos = New System.Windows.Forms.DataGridView()
        Me.pnlAdjuntosAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnAdjuntar = New System.Windows.Forms.Button()
        Me.btnVerAdjunto = New System.Windows.Forms.Button()
        Me.btnEliminarAdjunto = New System.Windows.Forms.Button()
        Me.pnlPreview = New System.Windows.Forms.Panel()
        Me.lblPreviewNotAvailable = New System.Windows.Forms.Label()
        Me.wbPreview = New System.Windows.Forms.WebBrowser()
        Me.pbPreview = New System.Windows.Forms.PictureBox()
        Me.lblCargoAnterior = New System.Windows.Forms.Label()
        Me.cboCargoAnterior = New System.Windows.Forms.ComboBox()
        Me.lblCargoNuevo = New System.Windows.Forms.Label()
        Me.cboCargoNuevo = New System.Windows.Forms.ComboBox()
        Me.lblFechaResolucion = New System.Windows.Forms.Label()
        Me.dtpFechaResolucion = New System.Windows.Forms.DateTimePicker()
        Me.chkSinFechaResolucion = New System.Windows.Forms.CheckBox()
        Me.MainLayout = New System.Windows.Forms.TableLayoutPanel()
        Me.pnlDatos = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.pnlAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dgvAdjuntos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAdjuntosAcciones.SuspendLayout()
        Me.pnlPreview.SuspendLayout()
        CType(Me.pbPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MainLayout.SuspendLayout()
        Me.pnlDatos.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.pnlAcciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboTipoEstado
        '
        Me.pnlDatos.SetColumnSpan(Me.cboTipoEstado, 3)
        Me.cboTipoEstado.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboTipoEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoEstado.FormattingEnabled = True
        Me.cboTipoEstado.Location = New System.Drawing.Point(132, 5)
        Me.cboTipoEstado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cboTipoEstado.Name = "cboTipoEstado"
        Me.cboTipoEstado.Size = New System.Drawing.Size(1014, 28)
        Me.cboTipoEstado.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 9)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Tipo de Estado:"
        '
        'lblFechaDesde
        '
        Me.lblFechaDesde.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblFechaDesde.AutoSize = True
        Me.lblFechaDesde.Location = New System.Drawing.Point(4, 48)
        Me.lblFechaDesde.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaDesde.Name = "lblFechaDesde"
        Me.lblFechaDesde.Size = New System.Drawing.Size(109, 20)
        Me.lblFechaDesde.TabIndex = 4
        Me.lblFechaDesde.Text = "Fecha Desde:"
        '
        'dtpFechaDesde
        '
        Me.dtpFechaDesde.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtpFechaDesde.Location = New System.Drawing.Point(132, 43)
        Me.dtpFechaDesde.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFechaDesde.Name = "dtpFechaDesde"
        Me.dtpFechaDesde.Size = New System.Drawing.Size(441, 26)
        Me.dtpFechaDesde.TabIndex = 1
        '
        'dtpFechaHasta
        '
        Me.dtpFechaHasta.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtpFechaHasta.Location = New System.Drawing.Point(3, 3)
        Me.dtpFechaHasta.Margin = New System.Windows.Forms.Padding(3, 3, 0, 3)
        Me.dtpFechaHasta.Name = "dtpFechaHasta"
        Me.dtpFechaHasta.Size = New System.Drawing.Size(325, 26)
        Me.dtpFechaHasta.TabIndex = 0
        '
        'lblFechaHasta
        '
        Me.lblFechaHasta.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblFechaHasta.AutoSize = True
        Me.lblFechaHasta.Location = New System.Drawing.Point(4, 88)
        Me.lblFechaHasta.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaHasta.Name = "lblFechaHasta"
        Me.lblFechaHasta.Size = New System.Drawing.Size(105, 20)
        Me.lblFechaHasta.TabIndex = 6
        Me.lblFechaHasta.Text = "Fecha Hasta:"
        '
        'chkFechaHasta
        '
        Me.chkFechaHasta.AutoSize = True
        Me.chkFechaHasta.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkFechaHasta.Location = New System.Drawing.Point(332, 5)
        Me.chkFechaHasta.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkFechaHasta.Name = "chkFechaHasta"
        Me.chkFechaHasta.Size = New System.Drawing.Size(84, 24)
        Me.chkFechaHasta.TabIndex = 1
        Me.chkFechaHasta.Text = "Sin Fin"
        Me.chkFechaHasta.UseVisualStyleBackColor = True
        '
        'txtObservaciones
        '
        Me.txtObservaciones.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.pnlDatos.SetColumnSpan(Me.txtObservaciones, 3)
        Me.txtObservaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtObservaciones.Location = New System.Drawing.Point(132, 123)
        Me.txtObservaciones.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObservaciones.Size = New System.Drawing.Size(1014, 60)
        Me.txtObservaciones.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(4, 123)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 5, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 20)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Observaciones:"
        '
        'lblResolucion
        '
        Me.lblResolucion.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblResolucion.AutoSize = True
        Me.lblResolucion.Location = New System.Drawing.Point(581, 196)
        Me.lblResolucion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblResolucion.Name = "lblResolucion"
        Me.lblResolucion.Size = New System.Drawing.Size(92, 20)
        Me.lblResolucion.TabIndex = 12
        Me.lblResolucion.Text = "Resolución:"
        '
        'txtResolucion
        '
        Me.txtResolucion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtResolucion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtResolucion.Location = New System.Drawing.Point(705, 193)
        Me.txtResolucion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtResolucion.Name = "txtResolucion"
        Me.txtResolucion.Size = New System.Drawing.Size(441, 26)
        Me.txtResolucion.TabIndex = 5
        '
        'lblDiagnostico
        '
        Me.lblDiagnostico.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblDiagnostico.AutoSize = True
        Me.lblDiagnostico.Location = New System.Drawing.Point(581, 88)
        Me.lblDiagnostico.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDiagnostico.Name = "lblDiagnostico"
        Me.lblDiagnostico.Size = New System.Drawing.Size(97, 20)
        Me.lblDiagnostico.TabIndex = 14
        Me.lblDiagnostico.Text = "Diagnóstico:"
        '
        'txtDiagnostico
        '
        Me.txtDiagnostico.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtDiagnostico.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtDiagnostico.Location = New System.Drawing.Point(705, 83)
        Me.txtDiagnostico.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtDiagnostico.Name = "txtDiagnostico"
        Me.txtDiagnostico.Size = New System.Drawing.Size(441, 26)
        Me.txtDiagnostico.TabIndex = 3
        '
        'lblTurnoReten
        '
        Me.lblTurnoReten.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblTurnoReten.AutoSize = True
        Me.lblTurnoReten.Location = New System.Drawing.Point(4, 196)
        Me.lblTurnoReten.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTurnoReten.Name = "lblTurnoReten"
        Me.lblTurnoReten.Size = New System.Drawing.Size(54, 20)
        Me.lblTurnoReten.TabIndex = 16
        Me.lblTurnoReten.Text = "Turno:"
        '
        'txtTurnoReten
        '
        Me.txtTurnoReten.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTurnoReten.Location = New System.Drawing.Point(132, 193)
        Me.txtTurnoReten.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtTurnoReten.Name = "txtTurnoReten"
        Me.txtTurnoReten.Size = New System.Drawing.Size(441, 26)
        Me.txtTurnoReten.TabIndex = 5
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.SplitContainer1)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(4, 269)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Size = New System.Drawing.Size(1148, 364)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Archivos Adjuntos"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(4, 24)
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.dgvAdjuntos)
        Me.SplitContainer1.Panel1.Controls.Add(Me.pnlAdjuntosAcciones)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.pnlPreview)
        Me.SplitContainer1.Size = New System.Drawing.Size(1140, 335)
        Me.SplitContainer1.SplitterDistance = 540
        Me.SplitContainer1.SplitterWidth = 6
        Me.SplitContainer1.TabIndex = 0
        '
        'dgvAdjuntos
        '
        Me.dgvAdjuntos.AllowUserToAddRows = False
        Me.dgvAdjuntos.AllowUserToDeleteRows = False
        Me.dgvAdjuntos.AllowUserToResizeColumns = False
        Me.dgvAdjuntos.AllowUserToResizeRows = False
        Me.dgvAdjuntos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAdjuntos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvAdjuntos.Location = New System.Drawing.Point(0, 0)
        Me.dgvAdjuntos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvAdjuntos.MultiSelect = False
        Me.dgvAdjuntos.Name = "dgvAdjuntos"
        Me.dgvAdjuntos.ReadOnly = True
        Me.dgvAdjuntos.RowHeadersWidth = 62
        Me.dgvAdjuntos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAdjuntos.Size = New System.Drawing.Size(540, 290)
        Me.dgvAdjuntos.TabIndex = 0
        '
        'pnlAdjuntosAcciones
        '
        Me.pnlAdjuntosAcciones.AutoSize = True
        Me.pnlAdjuntosAcciones.Controls.Add(Me.btnAdjuntar)
        Me.pnlAdjuntosAcciones.Controls.Add(Me.btnVerAdjunto)
        Me.pnlAdjuntosAcciones.Controls.Add(Me.btnEliminarAdjunto)
        Me.pnlAdjuntosAcciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlAdjuntosAcciones.Location = New System.Drawing.Point(0, 290)
        Me.pnlAdjuntosAcciones.Name = "pnlAdjuntosAcciones"
        Me.pnlAdjuntosAcciones.Size = New System.Drawing.Size(540, 45)
        Me.pnlAdjuntosAcciones.TabIndex = 4
        '
        'btnAdjuntar
        '
        Me.btnAdjuntar.Location = New System.Drawing.Point(4, 5)
        Me.btnAdjuntar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAdjuntar.Name = "btnAdjuntar"
        Me.btnAdjuntar.Size = New System.Drawing.Size(112, 35)
        Me.btnAdjuntar.TabIndex = 1
        Me.btnAdjuntar.Text = "Adjuntar"
        Me.btnAdjuntar.UseVisualStyleBackColor = True
        '
        'btnVerAdjunto
        '
        Me.btnVerAdjunto.Location = New System.Drawing.Point(124, 5)
        Me.btnVerAdjunto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnVerAdjunto.Name = "btnVerAdjunto"
        Me.btnVerAdjunto.Size = New System.Drawing.Size(128, 35)
        Me.btnVerAdjunto.TabIndex = 3
        Me.btnVerAdjunto.Text = "Abrir Archivo"
        Me.btnVerAdjunto.UseVisualStyleBackColor = True
        '
        'btnEliminarAdjunto
        '
        Me.btnEliminarAdjunto.Location = New System.Drawing.Point(260, 5)
        Me.btnEliminarAdjunto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnEliminarAdjunto.Name = "btnEliminarAdjunto"
        Me.btnEliminarAdjunto.Size = New System.Drawing.Size(112, 35)
        Me.btnEliminarAdjunto.TabIndex = 2
        Me.btnEliminarAdjunto.Text = "Eliminar"
        Me.btnEliminarAdjunto.UseVisualStyleBackColor = True
        '
        'pnlPreview
        '
        Me.pnlPreview.Controls.Add(Me.lblPreviewNotAvailable)
        Me.pnlPreview.Controls.Add(Me.wbPreview)
        Me.pnlPreview.Controls.Add(Me.pbPreview)
        Me.pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlPreview.Location = New System.Drawing.Point(0, 0)
        Me.pnlPreview.Name = "pnlPreview"
        Me.pnlPreview.Size = New System.Drawing.Size(594, 335)
        Me.pnlPreview.TabIndex = 3
        '
        'lblPreviewNotAvailable
        '
        Me.lblPreviewNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblPreviewNotAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreviewNotAvailable.Location = New System.Drawing.Point(0, 0)
        Me.lblPreviewNotAvailable.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPreviewNotAvailable.Name = "lblPreviewNotAvailable"
        Me.lblPreviewNotAvailable.Size = New System.Drawing.Size(594, 335)
        Me.lblPreviewNotAvailable.TabIndex = 2
        Me.lblPreviewNotAvailable.Text = "Seleccione un archivo para previsualizar"
        Me.lblPreviewNotAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'wbPreview
        '
        Me.wbPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.wbPreview.Location = New System.Drawing.Point(0, 0)
        Me.wbPreview.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.wbPreview.MinimumSize = New System.Drawing.Size(30, 31)
        Me.wbPreview.Name = "wbPreview"
        Me.wbPreview.Size = New System.Drawing.Size(594, 335)
        Me.wbPreview.TabIndex = 1
        Me.wbPreview.Visible = False
        '
        'pbPreview
        '
        Me.pbPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbPreview.Location = New System.Drawing.Point(0, 0)
        Me.pbPreview.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pbPreview.Name = "pbPreview"
        Me.pbPreview.Size = New System.Drawing.Size(594, 335)
        Me.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbPreview.TabIndex = 0
        Me.pbPreview.TabStop = False
        Me.pbPreview.Visible = False
        '
        'lblCargoAnterior
        '
        Me.lblCargoAnterior.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCargoAnterior.AutoSize = True
        Me.lblCargoAnterior.Location = New System.Drawing.Point(3, 231)
        Me.lblCargoAnterior.Name = "lblCargoAnterior"
        Me.lblCargoAnterior.Size = New System.Drawing.Size(116, 20)
        Me.lblCargoAnterior.TabIndex = 18
        Me.lblCargoAnterior.Text = "Cargo Anterior:"
        '
        'cboCargoAnterior
        '
        Me.cboCargoAnterior.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboCargoAnterior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCargoAnterior.FormattingEnabled = True
        Me.cboCargoAnterior.Location = New System.Drawing.Point(131, 227)
        Me.cboCargoAnterior.Name = "cboCargoAnterior"
        Me.cboCargoAnterior.Size = New System.Drawing.Size(443, 28)
        Me.cboCargoAnterior.TabIndex = 7
        '
        'lblCargoNuevo
        '
        Me.lblCargoNuevo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblCargoNuevo.AutoSize = True
        Me.lblCargoNuevo.Location = New System.Drawing.Point(580, 231)
        Me.lblCargoNuevo.Name = "lblCargoNuevo"
        Me.lblCargoNuevo.Size = New System.Drawing.Size(105, 20)
        Me.lblCargoNuevo.TabIndex = 20
        Me.lblCargoNuevo.Text = "Cargo Nuevo:"
        '
        'cboCargoNuevo
        '
        Me.cboCargoNuevo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cboCargoNuevo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCargoNuevo.FormattingEnabled = True
        Me.cboCargoNuevo.Location = New System.Drawing.Point(704, 227)
        Me.cboCargoNuevo.Name = "cboCargoNuevo"
        Me.cboCargoNuevo.Size = New System.Drawing.Size(443, 28)
        Me.cboCargoNuevo.TabIndex = 8
        '
        'lblFechaResolucion
        '
        Me.lblFechaResolucion.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.lblFechaResolucion.AutoSize = True
        Me.lblFechaResolucion.Location = New System.Drawing.Point(581, 48)
        Me.lblFechaResolucion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaResolucion.Name = "lblFechaResolucion"
        Me.lblFechaResolucion.Size = New System.Drawing.Size(116, 20)
        Me.lblFechaResolucion.TabIndex = 22
        Me.lblFechaResolucion.Text = "Fecha Resolu.:"
        '
        'dtpFechaResolucion
        '
        Me.dtpFechaResolucion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dtpFechaResolucion.Location = New System.Drawing.Point(3, 3)
        Me.dtpFechaResolucion.Margin = New System.Windows.Forms.Padding(3, 3, 0, 3)
        Me.dtpFechaResolucion.Name = "dtpFechaResolucion"
        Me.dtpFechaResolucion.Size = New System.Drawing.Size(325, 26)
        Me.dtpFechaResolucion.TabIndex = 0
        '
        'chkSinFechaResolucion
        '
        Me.chkSinFechaResolucion.AutoSize = True
        Me.chkSinFechaResolucion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkSinFechaResolucion.Location = New System.Drawing.Point(332, 5)
        Me.chkSinFechaResolucion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkSinFechaResolucion.Name = "chkSinFechaResolucion"
        Me.chkSinFechaResolucion.Size = New System.Drawing.Size(107, 24)
        Me.chkSinFechaResolucion.TabIndex = 1
        Me.chkSinFechaResolucion.Text = "Sin Fecha"
        Me.chkSinFechaResolucion.UseVisualStyleBackColor = True
        '
        'MainLayout
        '
        Me.MainLayout.ColumnCount = 1
        Me.MainLayout.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.Controls.Add(Me.pnlDatos, 0, 0)
        Me.MainLayout.Controls.Add(Me.GroupBox1, 0, 1)
        Me.MainLayout.Controls.Add(Me.pnlAcciones, 0, 2)
        Me.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MainLayout.Location = New System.Drawing.Point(10, 10)
        Me.MainLayout.Name = "MainLayout"
        Me.MainLayout.RowCount = 3
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.MainLayout.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.MainLayout.Size = New System.Drawing.Size(1156, 689)
        Me.MainLayout.TabIndex = 25
        '
        'pnlDatos
        '
        Me.pnlDatos.AutoSize = True
        Me.pnlDatos.ColumnCount = 4
        Me.pnlDatos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.pnlDatos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.pnlDatos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.pnlDatos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.pnlDatos.Controls.Add(Me.cboTipoEstado, 1, 0)
        Me.pnlDatos.Controls.Add(Me.Label1, 0, 0)
        Me.pnlDatos.Controls.Add(Me.cboCargoNuevo, 3, 5)
        Me.pnlDatos.Controls.Add(Me.lblFechaDesde, 0, 1)
        Me.pnlDatos.Controls.Add(Me.lblCargoNuevo, 2, 5)
        Me.pnlDatos.Controls.Add(Me.dtpFechaDesde, 1, 1)
        Me.pnlDatos.Controls.Add(Me.cboCargoAnterior, 1, 5)
        Me.pnlDatos.Controls.Add(Me.lblFechaHasta, 0, 2)
        Me.pnlDatos.Controls.Add(Me.lblCargoAnterior, 0, 5)
        Me.pnlDatos.Controls.Add(Me.FlowLayoutPanel1, 1, 2)
        Me.pnlDatos.Controls.Add(Me.lblDiagnostico, 2, 2)
        Me.pnlDatos.Controls.Add(Me.txtDiagnostico, 3, 2)
        Me.pnlDatos.Controls.Add(Me.lblFechaResolucion, 2, 1)
        Me.pnlDatos.Controls.Add(Me.FlowLayoutPanel2, 3, 1)
        Me.pnlDatos.Controls.Add(Me.Label4, 0, 3)
        Me.pnlDatos.Controls.Add(Me.txtObservaciones, 1, 3)
        Me.pnlDatos.Controls.Add(Me.lblTurnoReten, 0, 4)
        Me.pnlDatos.Controls.Add(Me.txtTurnoReten, 1, 4)
        Me.pnlDatos.Controls.Add(Me.lblResolucion, 2, 4)
        Me.pnlDatos.Controls.Add(Me.txtResolucion, 3, 4)
        Me.pnlDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlDatos.Location = New System.Drawing.Point(3, 3)
        Me.pnlDatos.Name = "pnlDatos"
        Me.pnlDatos.RowCount = 6
        Me.pnlDatos.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlDatos.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlDatos.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlDatos.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlDatos.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlDatos.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.pnlDatos.Size = New System.Drawing.Size(1150, 258)
        Me.pnlDatos.TabIndex = 0
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.Controls.Add(Me.dtpFechaHasta)
        Me.FlowLayoutPanel1.Controls.Add(Me.chkFechaHasta)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(131, 81)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(420, 34)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.FlowLayoutPanel2.AutoSize = True
        Me.FlowLayoutPanel2.Controls.Add(Me.dtpFechaResolucion)
        Me.FlowLayoutPanel2.Controls.Add(Me.chkSinFechaResolucion)
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(704, 41)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(443, 34)
        Me.FlowLayoutPanel2.TabIndex = 2
        '
        'pnlAcciones
        '
        Me.pnlAcciones.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.pnlAcciones.AutoSize = True
        Me.pnlAcciones.Controls.Add(Me.btnGuardar)
        Me.pnlAcciones.Controls.Add(Me.btnCancelar)
        Me.pnlAcciones.Location = New System.Drawing.Point(923, 641)
        Me.pnlAcciones.Name = "pnlAcciones"
        Me.pnlAcciones.Size = New System.Drawing.Size(230, 45)
        Me.pnlAcciones.TabIndex = 2
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(4, 5)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(112, 35)
        Me.btnGuardar.TabIndex = 0
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(124, 5)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(102, 35)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmFuncionarioEstadoTransitorio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(1176, 709)
        Me.Controls.Add(Me.MainLayout)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(1189, 739)
        Me.Name = "frmFuncionarioEstadoTransitorio"
        Me.Padding = New System.Windows.Forms.Padding(10)
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Estado Transitorio"
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dgvAdjuntos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAdjuntosAcciones.ResumeLayout(False)
        Me.pnlPreview.ResumeLayout(False)
        CType(Me.pbPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MainLayout.ResumeLayout(False)
        Me.MainLayout.PerformLayout()
        Me.pnlDatos.ResumeLayout(False)
        Me.pnlDatos.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.FlowLayoutPanel2.PerformLayout()
        Me.pnlAcciones.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents cboTipoEstado As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lblFechaDesde As Label
    Friend WithEvents dtpFechaDesde As DateTimePicker
    Friend WithEvents dtpFechaHasta As DateTimePicker
    Friend WithEvents lblFechaHasta As Label
    Friend WithEvents chkFechaHasta As CheckBox
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents lblResolucion As Label
    Friend WithEvents txtResolucion As TextBox
    Friend WithEvents lblDiagnostico As Label
    Friend WithEvents txtDiagnostico As TextBox
    Friend WithEvents lblTurnoReten As Label
    Friend WithEvents txtTurnoReten As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents dgvAdjuntos As DataGridView
    Friend WithEvents btnAdjuntar As Button
    Friend WithEvents btnVerAdjunto As Button
    Friend WithEvents btnEliminarAdjunto As Button
    Friend WithEvents pbPreview As PictureBox
    Friend WithEvents wbPreview As WebBrowser
    Friend WithEvents lblPreviewNotAvailable As Label
    Friend WithEvents pnlAdjuntosAcciones As FlowLayoutPanel
    Friend WithEvents pnlPreview As Panel
    Friend WithEvents lblCargoAnterior As Label
    Friend WithEvents cboCargoAnterior As ComboBox
    Friend WithEvents lblCargoNuevo As Label
    Friend WithEvents cboCargoNuevo As ComboBox
    Friend WithEvents lblFechaResolucion As Label
    Friend WithEvents dtpFechaResolucion As DateTimePicker
    Friend WithEvents chkSinFechaResolucion As CheckBox
    Friend WithEvents MainLayout As TableLayoutPanel
    Friend WithEvents pnlDatos As TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents pnlAcciones As FlowLayoutPanel
End Class
