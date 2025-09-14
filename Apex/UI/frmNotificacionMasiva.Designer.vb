<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNotificacionMasiva
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.tlpRoot = New System.Windows.Forms.TableLayoutPanel()
        Me.gbDatos = New System.Windows.Forms.GroupBox()
        Me.tlpDatos = New System.Windows.Forms.TableLayoutPanel()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.cboTipoNotificacion = New System.Windows.Forms.ComboBox()
        Me.lblFecha = New System.Windows.Forms.Label()
        Me.dtpFechaProgramada = New System.Windows.Forms.DateTimePicker()
        Me.lblMedio = New System.Windows.Forms.Label()
        Me.txtMedio = New System.Windows.Forms.TextBox()
        Me.lblDocumento = New System.Windows.Forms.Label()
        Me.txtDocumento = New System.Windows.Forms.TextBox()
        Me.lblExpMin = New System.Windows.Forms.Label()
        Me.txtExpMinisterial = New System.Windows.Forms.TextBox()
        Me.lblExpINR = New System.Windows.Forms.Label()
        Me.txtExpINR = New System.Windows.Forms.TextBox()
        Me.lblOficina = New System.Windows.Forms.Label()
        Me.txtOficina = New System.Windows.Forms.TextBox()
        Me.gbDestinatarios = New System.Windows.Forms.GroupBox()
        Me.pnlDestinatariosTop = New System.Windows.Forms.Panel()
        Me.btnSeleccionarDestinatarios = New System.Windows.Forms.Button()
        Me.lstDestinatarios = New System.Windows.Forms.ListBox()
        Me.tlpBottom = New System.Windows.Forms.TableLayoutPanel()
        Me.lblResumen = New System.Windows.Forms.Label()
        Me.pbProgreso = New System.Windows.Forms.ProgressBar()
        Me.flpBotones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGenerar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.tlpRoot.SuspendLayout()
        Me.gbDatos.SuspendLayout()
        Me.tlpDatos.SuspendLayout()
        Me.gbDestinatarios.SuspendLayout()
        Me.pnlDestinatariosTop.SuspendLayout()
        Me.tlpBottom.SuspendLayout()
        Me.flpBotones.SuspendLayout()
        Me.SuspendLayout()
        '
        'tlpRoot
        '
        Me.tlpRoot.ColumnCount = 2
        Me.tlpRoot.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpRoot.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpRoot.Controls.Add(Me.gbDatos, 0, 0)
        Me.tlpRoot.Controls.Add(Me.gbDestinatarios, 1, 0)
        Me.tlpRoot.Controls.Add(Me.tlpBottom, 0, 1)
        Me.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpRoot.Location = New System.Drawing.Point(0, 0)
        Me.tlpRoot.Name = "tlpRoot"
        Me.tlpRoot.RowCount = 2
        Me.tlpRoot.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpRoot.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.tlpRoot.Size = New System.Drawing.Size(980, 600)
        Me.tlpRoot.TabIndex = 0
        '
        'gbDatos
        '
        Me.gbDatos.Controls.Add(Me.tlpDatos)
        Me.gbDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbDatos.Location = New System.Drawing.Point(10, 10)
        Me.gbDatos.Margin = New System.Windows.Forms.Padding(10, 10, 5, 5)
        Me.gbDatos.Name = "gbDatos"
        Me.gbDatos.Padding = New System.Windows.Forms.Padding(10)
        Me.gbDatos.Size = New System.Drawing.Size(475, 515)
        Me.gbDatos.TabIndex = 0
        Me.gbDatos.TabStop = False
        Me.gbDatos.Text = "Datos de la notificación"
        '
        'tlpDatos
        '
        Me.tlpDatos.ColumnCount = 2
        Me.tlpDatos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130.0!))
        Me.tlpDatos.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpDatos.Controls.Add(Me.lblTipo, 0, 0)
        Me.tlpDatos.Controls.Add(Me.cboTipoNotificacion, 1, 0)
        Me.tlpDatos.Controls.Add(Me.lblFecha, 0, 1)
        Me.tlpDatos.Controls.Add(Me.dtpFechaProgramada, 1, 1)
        Me.tlpDatos.Controls.Add(Me.lblMedio, 0, 2)
        Me.tlpDatos.Controls.Add(Me.txtMedio, 1, 2)
        Me.tlpDatos.Controls.Add(Me.lblDocumento, 0, 3)
        Me.tlpDatos.Controls.Add(Me.txtDocumento, 1, 3)
        Me.tlpDatos.Controls.Add(Me.lblExpMin, 0, 4)
        Me.tlpDatos.Controls.Add(Me.txtExpMinisterial, 1, 4)
        Me.tlpDatos.Controls.Add(Me.lblExpINR, 0, 5)
        Me.tlpDatos.Controls.Add(Me.txtExpINR, 1, 5)
        Me.tlpDatos.Controls.Add(Me.lblOficina, 0, 6)
        Me.tlpDatos.Controls.Add(Me.txtOficina, 1, 6)
        Me.tlpDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpDatos.Location = New System.Drawing.Point(10, 26)
        Me.tlpDatos.Name = "tlpDatos"
        Me.tlpDatos.RowCount = 8
        Me.tlpDatos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.tlpDatos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.tlpDatos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
        Me.tlpDatos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.tlpDatos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.tlpDatos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.tlpDatos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.tlpDatos.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpDatos.Padding = New System.Windows.Forms.Padding(0, 5, 0, 0)
        Me.tlpDatos.Size = New System.Drawing.Size(455, 479)
        Me.tlpDatos.TabIndex = 0
        '
        'lblTipo
        '
        Me.lblTipo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(3, 13)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(124, 20)
        Me.lblTipo.TabIndex = 0
        Me.lblTipo.Text = "Tipo:"
        '
        'cboTipoNotificacion
        '
        Me.cboTipoNotificacion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboTipoNotificacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoNotificacion.FormattingEnabled = True
        Me.cboTipoNotificacion.Location = New System.Drawing.Point(133, 8)
        Me.cboTipoNotificacion.Name = "cboTipoNotificacion"
        Me.cboTipoNotificacion.Size = New System.Drawing.Size(319, 28)
        Me.cboTipoNotificacion.TabIndex = 1
        '
        'lblFecha
        '
        Me.lblFecha.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFecha.AutoSize = True
        Me.lblFecha.Location = New System.Drawing.Point(3, 49)
        Me.lblFecha.Name = "lblFecha"
        Me.lblFecha.Size = New System.Drawing.Size(124, 20)
        Me.lblFecha.TabIndex = 2
        Me.lblFecha.Text = "Fecha programada:"
        '
        'dtpFechaProgramada
        '
        Me.dtpFechaProgramada.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dtpFechaProgramada.CustomFormat = "yyyy-MM-dd HH:mm"
        Me.dtpFechaProgramada.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpFechaProgramada.Location = New System.Drawing.Point(133, 44)
        Me.dtpFechaProgramada.Name = "dtpFechaProgramada"
        Me.dtpFechaProgramada.Size = New System.Drawing.Size(319, 27)
        Me.dtpFechaProgramada.TabIndex = 3
        '
        'lblMedio
        '
        Me.lblMedio.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMedio.AutoSize = True
        Me.lblMedio.Location = New System.Drawing.Point(3, 98)
        Me.lblMedio.Name = "lblMedio"
        Me.lblMedio.Size = New System.Drawing.Size(124, 20)
        Me.lblMedio.TabIndex = 4
        Me.lblMedio.Text = "Texto (medio):"
        '
        'txtMedio
        '
        Me.txtMedio.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtMedio.Location = New System.Drawing.Point(133, 80)
        Me.txtMedio.Multiline = True
        Me.txtMedio.Name = "txtMedio"
        Me.txtMedio.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMedio.Size = New System.Drawing.Size(319, 74)
        Me.txtMedio.TabIndex = 5
        '
        'lblDocumento
        '
        Me.lblDocumento.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDocumento.AutoSize = True
        Me.lblDocumento.Location = New System.Drawing.Point(3, 162)
        Me.lblDocumento.Name = "lblDocumento"
        Me.lblDocumento.Size = New System.Drawing.Size(124, 20)
        Me.lblDocumento.TabIndex = 6
        Me.lblDocumento.Text = "Documento:"
        '
        'txtDocumento
        '
        Me.txtDocumento.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDocumento.Location = New System.Drawing.Point(133, 160)
        Me.txtDocumento.Name = "txtDocumento"
        Me.txtDocumento.Size = New System.Drawing.Size(319, 27)
        Me.txtDocumento.TabIndex = 7
        '
        'lblExpMin
        '
        Me.lblExpMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblExpMin.AutoSize = True
        Me.lblExpMin.Location = New System.Drawing.Point(3, 198)
        Me.lblExpMin.Name = "lblExpMin"
        Me.lblExpMin.Size = New System.Drawing.Size(124, 20)
        Me.lblExpMin.TabIndex = 8
        Me.lblExpMin.Text = "Exp. Ministerial:"
        '
        'txtExpMinisterial
        '
        Me.txtExpMinisterial.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExpMinisterial.Location = New System.Drawing.Point(133, 196)
        Me.txtExpMinisterial.Name = "txtExpMinisterial"
        Me.txtExpMinisterial.Size = New System.Drawing.Size(319, 27)
        Me.txtExpMinisterial.TabIndex = 9
        '
        'lblExpINR
        '
        Me.lblExpINR.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblExpINR.AutoSize = True
        Me.lblExpINR.Location = New System.Drawing.Point(3, 234)
        Me.lblExpINR.Name = "lblExpINR"
        Me.lblExpINR.Size = New System.Drawing.Size(124, 20)
        Me.lblExpINR.TabIndex = 10
        Me.lblExpINR.Text = "Exp. INR:"
        '
        'txtExpINR
        '
        Me.txtExpINR.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExpINR.Location = New System.Drawing.Point(133, 232)
        Me.txtExpINR.Name = "txtExpINR"
        Me.txtExpINR.Size = New System.Drawing.Size(319, 27)
        Me.txtExpINR.TabIndex = 11
        '
        'lblOficina
        '
        Me.lblOficina.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOficina.AutoSize = True
        Me.lblOficina.Location = New System.Drawing.Point(3, 270)
        Me.lblOficina.Name = "lblOficina"
        Me.lblOficina.Size = New System.Drawing.Size(124, 20)
        Me.lblOficina.TabIndex = 12
        Me.lblOficina.Text = "Oficina:"
        '
        'txtOficina
        '
        Me.txtOficina.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtOficina.Location = New System.Drawing.Point(133, 268)
        Me.txtOficina.Name = "txtOficina"
        Me.txtOficina.Size = New System.Drawing.Size(319, 27)
        Me.txtOficina.TabIndex = 13
        '
        'gbDestinatarios
        '
        Me.gbDestinatarios.Controls.Add(Me.lstDestinatarios)
        Me.gbDestinatarios.Controls.Add(Me.pnlDestinatariosTop)
        Me.gbDestinatarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbDestinatarios.Location = New System.Drawing.Point(495, 10)
        Me.gbDestinatarios.Margin = New System.Windows.Forms.Padding(5, 10, 10, 5)
        Me.gbDestinatarios.Name = "gbDestinatarios"
        Me.gbDestinatarios.Padding = New System.Windows.Forms.Padding(10)
        Me.gbDestinatarios.Size = New System.Drawing.Size(475, 515)
        Me.gbDestinatarios.TabIndex = 1
        Me.gbDestinatarios.TabStop = False
        Me.gbDestinatarios.Text = "Destinatarios"
        '
        'pnlDestinatariosTop
        '
        Me.pnlDestinatariosTop.Controls.Add(Me.btnSeleccionarDestinatarios)
        Me.pnlDestinatariosTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlDestinatariosTop.Location = New System.Drawing.Point(10, 26)
        Me.pnlDestinatariosTop.Name = "pnlDestinatariosTop"
        Me.pnlDestinatariosTop.Padding = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.pnlDestinatariosTop.Size = New System.Drawing.Size(455, 40)
        Me.pnlDestinatariosTop.TabIndex = 0
        '
        'btnSeleccionarDestinatarios
        '
        Me.btnSeleccionarDestinatarios.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeleccionarDestinatarios.Location = New System.Drawing.Point(235, 6)
        Me.btnSeleccionarDestinatarios.Name = "btnSeleccionarDestinatarios"
        Me.btnSeleccionarDestinatarios.Size = New System.Drawing.Size(210, 28)
        Me.btnSeleccionarDestinatarios.TabIndex = 0
        Me.btnSeleccionarDestinatarios.Text = "Seleccionar con Filtros…"
        Me.btnSeleccionarDestinatarios.UseVisualStyleBackColor = True
        '
        'lstDestinatarios
        '
        Me.lstDestinatarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstDestinatarios.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.lstDestinatarios.FormattingEnabled = True
        Me.lstDestinatarios.IntegralHeight = False
        Me.lstDestinatarios.ItemHeight = 20
        Me.lstDestinatarios.Location = New System.Drawing.Point(10, 66)
        Me.lstDestinatarios.Name = "lstDestinatarios"
        Me.lstDestinatarios.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.lstDestinatarios.Size = New System.Drawing.Size(455, 439)
        Me.lstDestinatarios.TabIndex = 1
        '
        'tlpBottom
        '
        Me.tlpBottom.ColumnCount = 3
        Me.tlpRoot.SetColumnSpan(Me.tlpBottom, 2)
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.tlpBottom.Controls.Add(Me.lblResumen, 0, 0)
        Me.tlpBottom.Controls.Add(Me.pbProgreso, 1, 0)
        Me.tlpBottom.Controls.Add(Me.flpBotones, 2, 0)
        Me.tlpBottom.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpBottom.Location = New System.Drawing.Point(10, 530)
        Me.tlpBottom.Margin = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.tlpBottom.Name = "tlpBottom"
        Me.tlpBottom.RowCount = 1
        Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpBottom.Size = New System.Drawing.Size(960, 60)
        Me.tlpBottom.TabIndex = 2
        '
        'lblResumen
        '
        Me.lblResumen.Anchor = CType(((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right)), System.Windows.Forms.AnchorStyles)
        Me.lblResumen.AutoSize = True
        Me.lblResumen.Location = New System.Drawing.Point(3, 20)
        Me.lblResumen.Name = "lblResumen"
        Me.lblResumen.Size = New System.Drawing.Size(330, 20)
        Me.lblResumen.TabIndex = 0
        Me.lblResumen.Text = "Listo para generar."
        '
        'pbProgreso
        '
        Me.pbProgreso.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbProgreso.Location = New System.Drawing.Point(339, 10)
        Me.pbProgreso.Margin = New System.Windows.Forms.Padding(3, 10, 3, 10)
        Me.pbProgreso.Name = "pbProgreso"
        Me.pbProgreso.Size = New System.Drawing.Size(378, 40)
        Me.pbProgreso.TabIndex = 1
        '
        'flpBotones
        '
        Me.flpBotones.Anchor = CType((System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpBotones.AutoSize = True
        Me.flpBotones.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpBotones.Controls.Add(Me.btnGenerar)
        Me.flpBotones.Controls.Add(Me.btnCancelar)
        Me.flpBotones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.flpBotones.Location = New System.Drawing.Point(735, 10)
        Me.flpBotones.Margin = New System.Windows.Forms.Padding(3, 10, 3, 10)
        Me.flpBotones.Name = "flpBotones"
        Me.flpBotones.Size = New System.Drawing.Size(222, 40)
        Me.flpBotones.TabIndex = 2
        '
        'btnGenerar
        '
        Me.btnGenerar.Location = New System.Drawing.Point(114, 3)
        Me.btnGenerar.Name = "btnGenerar"
        Me.btnGenerar.Size = New System.Drawing.Size(105, 34)
        Me.btnGenerar.TabIndex = 0
        Me.btnGenerar.Text = "Generar"
        Me.btnGenerar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(3, 3)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(105, 34)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmNotificacionMasiva
        '
        Me.AcceptButton = Me.btnGenerar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(980, 600)
        Me.Controls.Add(Me.tlpRoot)
        Me.MinimumSize = New System.Drawing.Size(900, 550)
        Me.Name = "frmNotificacionMasiva"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Notificación masiva"
        Me.tlpRoot.ResumeLayout(False)
        Me.gbDatos.ResumeLayout(False)
        Me.tlpDatos.ResumeLayout(False)
        Me.tlpDatos.PerformLayout()
        Me.gbDestinatarios.ResumeLayout(False)
        Me.pnlDestinatariosTop.ResumeLayout(False)
        Me.tlpBottom.ResumeLayout(False)
        Me.tlpBottom.PerformLayout()
        Me.flpBotones.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tlpRoot As TableLayoutPanel
    Friend WithEvents gbDatos As GroupBox
    Friend WithEvents tlpDatos As TableLayoutPanel
    Friend WithEvents lblTipo As Label
    Friend WithEvents cboTipoNotificacion As ComboBox
    Friend WithEvents lblFecha As Label
    Friend WithEvents dtpFechaProgramada As DateTimePicker
    Friend WithEvents lblMedio As Label
    Friend WithEvents txtMedio As TextBox
    Friend WithEvents lblDocumento As Label
    Friend WithEvents txtDocumento As TextBox
    Friend WithEvents lblExpMin As Label
    Friend WithEvents txtExpMinisterial As TextBox
    Friend WithEvents lblExpINR As Label
    Friend WithEvents txtExpINR As TextBox
    Friend WithEvents lblOficina As Label
    Friend WithEvents txtOficina As TextBox
    Friend WithEvents gbDestinatarios As GroupBox
    Friend WithEvents pnlDestinatariosTop As Panel
    Friend WithEvents btnSeleccionarDestinatarios As Button
    Friend WithEvents lstDestinatarios As ListBox
    Friend WithEvents tlpBottom As TableLayoutPanel
    Friend WithEvents lblResumen As Label
    Friend WithEvents pbProgreso As ProgressBar
    Friend WithEvents flpBotones As FlowLayoutPanel
    Friend WithEvents btnGenerar As Button
    Friend WithEvents btnCancelar As Button
End Class
