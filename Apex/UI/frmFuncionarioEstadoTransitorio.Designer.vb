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
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
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
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dgvAdjuntos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAdjuntosAcciones.SuspendLayout()
        Me.pnlPreview.SuspendLayout()
        CType(Me.pbPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Location = New System.Drawing.Point(1046, 655)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(112, 35)
        Me.btnGuardar.TabIndex = 0
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.Location = New System.Drawing.Point(924, 655)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(112, 35)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'cboTipoEstado
        '
        Me.cboTipoEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoEstado.FormattingEnabled = True
        Me.cboTipoEstado.Location = New System.Drawing.Point(166, 18)
        Me.cboTipoEstado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cboTipoEstado.Name = "cboTipoEstado"
        Me.cboTipoEstado.Size = New System.Drawing.Size(400, 28)
        Me.cboTipoEstado.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 23)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(120, 20)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Tipo de Estado:"
        '
        'lblFechaDesde
        '
        Me.lblFechaDesde.AutoSize = True
        Me.lblFechaDesde.Location = New System.Drawing.Point(18, 69)
        Me.lblFechaDesde.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaDesde.Name = "lblFechaDesde"
        Me.lblFechaDesde.Size = New System.Drawing.Size(109, 20)
        Me.lblFechaDesde.TabIndex = 4
        Me.lblFechaDesde.Text = "Fecha Desde:"
        '
        'dtpFechaDesde
        '
        Me.dtpFechaDesde.Location = New System.Drawing.Point(166, 60)
        Me.dtpFechaDesde.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFechaDesde.Name = "dtpFechaDesde"
        Me.dtpFechaDesde.Size = New System.Drawing.Size(298, 26)
        Me.dtpFechaDesde.TabIndex = 5
        '
        'dtpFechaHasta
        '
        Me.dtpFechaHasta.Location = New System.Drawing.Point(166, 100)
        Me.dtpFechaHasta.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFechaHasta.Name = "dtpFechaHasta"
        Me.dtpFechaHasta.Size = New System.Drawing.Size(298, 26)
        Me.dtpFechaHasta.TabIndex = 7
        '
        'lblFechaHasta
        '
        Me.lblFechaHasta.AutoSize = True
        Me.lblFechaHasta.Location = New System.Drawing.Point(18, 109)
        Me.lblFechaHasta.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaHasta.Name = "lblFechaHasta"
        Me.lblFechaHasta.Size = New System.Drawing.Size(105, 20)
        Me.lblFechaHasta.TabIndex = 6
        Me.lblFechaHasta.Text = "Fecha Hasta:"
        '
        'chkFechaHasta
        '
        Me.chkFechaHasta.AutoSize = True
        Me.chkFechaHasta.Location = New System.Drawing.Point(476, 105)
        Me.chkFechaHasta.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkFechaHasta.Name = "chkFechaHasta"
        Me.chkFechaHasta.Size = New System.Drawing.Size(84, 24)
        Me.chkFechaHasta.TabIndex = 8
        Me.chkFechaHasta.Text = "Sin Fin"
        Me.chkFechaHasta.UseVisualStyleBackColor = True
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Location = New System.Drawing.Point(166, 140)
        Me.txtObservaciones.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(400, 73)
        Me.txtObservaciones.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(18, 145)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 20)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Observaciones:"
        '
        'lblResolucion
        '
        Me.lblResolucion.AutoSize = True
        Me.lblResolucion.Location = New System.Drawing.Point(579, 23)
        Me.lblResolucion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblResolucion.Name = "lblResolucion"
        Me.lblResolucion.Size = New System.Drawing.Size(92, 20)
        Me.lblResolucion.TabIndex = 12
        Me.lblResolucion.Text = "Resolución:"
        '
        'txtResolucion
        '
        Me.txtResolucion.Location = New System.Drawing.Point(727, 18)
        Me.txtResolucion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtResolucion.Name = "txtResolucion"
        Me.txtResolucion.Size = New System.Drawing.Size(427, 26)
        Me.txtResolucion.TabIndex = 11
        '
        'lblDiagnostico
        '
        Me.lblDiagnostico.AutoSize = True
        Me.lblDiagnostico.Location = New System.Drawing.Point(579, 105)
        Me.lblDiagnostico.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblDiagnostico.Name = "lblDiagnostico"
        Me.lblDiagnostico.Size = New System.Drawing.Size(97, 20)
        Me.lblDiagnostico.TabIndex = 14
        Me.lblDiagnostico.Text = "Diagnóstico:"
        '
        'txtDiagnostico
        '
        Me.txtDiagnostico.Location = New System.Drawing.Point(727, 100)
        Me.txtDiagnostico.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtDiagnostico.Name = "txtDiagnostico"
        Me.txtDiagnostico.Size = New System.Drawing.Size(427, 26)
        Me.txtDiagnostico.TabIndex = 13
        '
        'lblTurnoReten
        '
        Me.lblTurnoReten.AutoSize = True
        Me.lblTurnoReten.Location = New System.Drawing.Point(579, 145)
        Me.lblTurnoReten.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTurnoReten.Name = "lblTurnoReten"
        Me.lblTurnoReten.Size = New System.Drawing.Size(54, 20)
        Me.lblTurnoReten.TabIndex = 16
        Me.lblTurnoReten.Text = "Turno:"
        '
        'txtTurnoReten
        '
        Me.txtTurnoReten.Location = New System.Drawing.Point(727, 140)
        Me.txtTurnoReten.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtTurnoReten.Name = "txtTurnoReten"
        Me.txtTurnoReten.Size = New System.Drawing.Size(427, 26)
        Me.txtTurnoReten.TabIndex = 15
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.SplitContainer1)
        Me.GroupBox1.Location = New System.Drawing.Point(22, 225)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Size = New System.Drawing.Size(1136, 422)
        Me.GroupBox1.TabIndex = 17
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
        Me.SplitContainer1.Size = New System.Drawing.Size(1128, 393)
        Me.SplitContainer1.SplitterDistance = 525
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
        Me.dgvAdjuntos.Size = New System.Drawing.Size(525, 343)
        Me.dgvAdjuntos.TabIndex = 0
        '
        'pnlAdjuntosAcciones
        '
        Me.pnlAdjuntosAcciones.Controls.Add(Me.btnAdjuntar)
        Me.pnlAdjuntosAcciones.Controls.Add(Me.btnVerAdjunto)
        Me.pnlAdjuntosAcciones.Controls.Add(Me.btnEliminarAdjunto)
        Me.pnlAdjuntosAcciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlAdjuntosAcciones.Location = New System.Drawing.Point(0, 343)
        Me.pnlAdjuntosAcciones.Name = "pnlAdjuntosAcciones"
        Me.pnlAdjuntosAcciones.Size = New System.Drawing.Size(525, 50)
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
        Me.pnlPreview.Size = New System.Drawing.Size(597, 393)
        Me.pnlPreview.TabIndex = 3
        '
        'lblPreviewNotAvailable
        '
        Me.lblPreviewNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblPreviewNotAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreviewNotAvailable.Location = New System.Drawing.Point(0, 0)
        Me.lblPreviewNotAvailable.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblPreviewNotAvailable.Name = "lblPreviewNotAvailable"
        Me.lblPreviewNotAvailable.Size = New System.Drawing.Size(597, 393)
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
        Me.wbPreview.Size = New System.Drawing.Size(597, 393)
        Me.wbPreview.TabIndex = 1
        Me.wbPreview.Visible = False
        '
        'pbPreview
        '
        Me.pbPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbPreview.Location = New System.Drawing.Point(0, 0)
        Me.pbPreview.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.pbPreview.Name = "pbPreview"
        Me.pbPreview.Size = New System.Drawing.Size(597, 393)
        Me.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbPreview.TabIndex = 0
        Me.pbPreview.TabStop = False
        Me.pbPreview.Visible = False
        '
        'lblCargoAnterior
        '
        Me.lblCargoAnterior.AutoSize = True
        Me.lblCargoAnterior.Location = New System.Drawing.Point(579, 145)
        Me.lblCargoAnterior.Name = "lblCargoAnterior"
        Me.lblCargoAnterior.Size = New System.Drawing.Size(125, 20)
        Me.lblCargoAnterior.TabIndex = 18
        Me.lblCargoAnterior.Text = "Cargo Anterior:"
        '
        'cboCargoAnterior
        '
        Me.cboCargoAnterior.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCargoAnterior.FormattingEnabled = True
        Me.cboCargoAnterior.Location = New System.Drawing.Point(727, 140)
        Me.cboCargoAnterior.Name = "cboCargoAnterior"
        Me.cboCargoAnterior.Size = New System.Drawing.Size(427, 28)
        Me.cboCargoAnterior.TabIndex = 19
        '
        'lblCargoNuevo
        '
        Me.lblCargoNuevo.AutoSize = True
        Me.lblCargoNuevo.Location = New System.Drawing.Point(579, 185)
        Me.lblCargoNuevo.Name = "lblCargoNuevo"
        Me.lblCargoNuevo.Size = New System.Drawing.Size(107, 20)
        Me.lblCargoNuevo.TabIndex = 20
        Me.lblCargoNuevo.Text = "Cargo Nuevo:"
        '
        'cboCargoNuevo
        '
        Me.cboCargoNuevo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCargoNuevo.FormattingEnabled = True
        Me.cboCargoNuevo.Location = New System.Drawing.Point(727, 180)
        Me.cboCargoNuevo.Name = "cboCargoNuevo"
        Me.cboCargoNuevo.Size = New System.Drawing.Size(427, 28)
        Me.cboCargoNuevo.TabIndex = 21
        '
        'lblFechaResolucion
        '
        Me.lblFechaResolucion.AutoSize = True
        Me.lblFechaResolucion.Location = New System.Drawing.Point(579, 65)
        Me.lblFechaResolucion.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblFechaResolucion.Name = "lblFechaResolucion"
        Me.lblFechaResolucion.Size = New System.Drawing.Size(141, 20)
        Me.lblFechaResolucion.TabIndex = 22
        Me.lblFechaResolucion.Text = "Fecha Resolución:"
        '
        'dtpFechaResolucion
        '
        Me.dtpFechaResolucion.Location = New System.Drawing.Point(727, 60)
        Me.dtpFechaResolucion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFechaResolucion.Name = "dtpFechaResolucion"
        Me.dtpFechaResolucion.Size = New System.Drawing.Size(298, 26)
        Me.dtpFechaResolucion.TabIndex = 23
        '
        'chkSinFechaResolucion
        '
        Me.chkSinFechaResolucion.AutoSize = True
        Me.chkSinFechaResolucion.Location = New System.Drawing.Point(1037, 63)
        Me.chkSinFechaResolucion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkSinFechaResolucion.Name = "chkSinFechaResolucion"
        Me.chkSinFechaResolucion.Size = New System.Drawing.Size(110, 24)
        Me.chkSinFechaResolucion.TabIndex = 24
        Me.chkSinFechaResolucion.Text = "Sin Fecha"
        Me.chkSinFechaResolucion.UseVisualStyleBackColor = True
        '
        'frmFuncionarioEstadoTransitorio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1176, 709)
        Me.Controls.Add(Me.chkSinFechaResolucion)
        Me.Controls.Add(Me.dtpFechaResolucion)
        Me.Controls.Add(Me.lblFechaResolucion)
        Me.Controls.Add(Me.cboCargoNuevo)
        Me.Controls.Add(Me.lblCargoNuevo)
        Me.Controls.Add(Me.cboCargoAnterior)
        Me.Controls.Add(Me.lblCargoAnterior)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblTurnoReten)
        Me.Controls.Add(Me.txtTurnoReten)
        Me.Controls.Add(Me.lblDiagnostico)
        Me.Controls.Add(Me.txtDiagnostico)
        Me.Controls.Add(Me.lblResolucion)
        Me.Controls.Add(Me.txtResolucion)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtObservaciones)
        Me.Controls.Add(Me.chkFechaHasta)
        Me.Controls.Add(Me.dtpFechaHasta)
        Me.Controls.Add(Me.lblFechaHasta)
        Me.Controls.Add(Me.dtpFechaDesde)
        Me.Controls.Add(Me.lblFechaDesde)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cboTipoEstado)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(1189, 739)
        Me.Name = "frmFuncionarioEstadoTransitorio"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Estado Transitorio"
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dgvAdjuntos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAdjuntosAcciones.ResumeLayout(False)
        Me.pnlPreview.ResumeLayout(False)
        CType(Me.pbPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

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
    ' Controles nuevos
    Friend WithEvents lblFechaResolucion As Label
    Friend WithEvents dtpFechaResolucion As DateTimePicker
    Friend WithEvents chkSinFechaResolucion As CheckBox
End Class