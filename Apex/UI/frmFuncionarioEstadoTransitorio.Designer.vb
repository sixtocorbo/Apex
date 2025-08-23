<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioEstadoTransitorio
    Inherits System.Windows.Forms.Form

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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblTipoEstado = New System.Windows.Forms.Label()
        Me.cboTipoEstado = New System.Windows.Forms.ComboBox()
        Me.lblFechaDesde = New System.Windows.Forms.Label()
        Me.dtpFechaDesde = New System.Windows.Forms.DateTimePicker()
        Me.lblFechaHasta = New System.Windows.Forms.Label()
        Me.dtpFechaHasta = New System.Windows.Forms.DateTimePicker()
        Me.chkFechaHasta = New System.Windows.Forms.CheckBox()
        Me.lblObservaciones = New System.Windows.Forms.Label()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.pnlDetallesEspecificos = New System.Windows.Forms.Panel()
        Me.txtTurnoReten = New System.Windows.Forms.TextBox()
        Me.lblTurnoReten = New System.Windows.Forms.Label()
        Me.txtDiagnostico = New System.Windows.Forms.TextBox()
        Me.lblDiagnostico = New System.Windows.Forms.Label()
        Me.txtResolucion = New System.Windows.Forms.TextBox()
        Me.lblResolucion = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnEliminarAdjunto = New System.Windows.Forms.Button()
        Me.btnVerAdjunto = New System.Windows.Forms.Button()
        Me.btnAdjuntar = New System.Windows.Forms.Button()
        Me.dgvAdjuntos = New System.Windows.Forms.DataGridView()
        Me.pnlDetallesEspecificos.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgvAdjuntos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTipoEstado
        '
        Me.lblTipoEstado.AutoSize = True
        Me.lblTipoEstado.Location = New System.Drawing.Point(12, 15)
        Me.lblTipoEstado.Name = "lblTipoEstado"
        Me.lblTipoEstado.Size = New System.Drawing.Size(40, 17)
        Me.lblTipoEstado.TabIndex = 0
        Me.lblTipoEstado.Text = "Tipo:"
        '
        'cboTipoEstado
        '
        Me.cboTipoEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoEstado.FormattingEnabled = True
        Me.cboTipoEstado.Location = New System.Drawing.Point(120, 12)
        Me.cboTipoEstado.Name = "cboTipoEstado"
        Me.cboTipoEstado.Size = New System.Drawing.Size(252, 24)
        Me.cboTipoEstado.TabIndex = 1
        '
        'lblFechaDesde
        '
        Me.lblFechaDesde.AutoSize = True
        Me.lblFechaDesde.Location = New System.Drawing.Point(12, 48)
        Me.lblFechaDesde.Name = "lblFechaDesde"
        Me.lblFechaDesde.Size = New System.Drawing.Size(94, 17)
        Me.lblFechaDesde.TabIndex = 2
        Me.lblFechaDesde.Text = "Fecha Desde:"
        '
        'dtpFechaDesde
        '
        Me.dtpFechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFechaDesde.Location = New System.Drawing.Point(120, 45)
        Me.dtpFechaDesde.Name = "dtpFechaDesde"
        Me.dtpFechaDesde.Size = New System.Drawing.Size(120, 22)
        Me.dtpFechaDesde.TabIndex = 3
        '
        'lblFechaHasta
        '
        Me.lblFechaHasta.AutoSize = True
        Me.lblFechaHasta.Location = New System.Drawing.Point(12, 81)
        Me.lblFechaHasta.Name = "lblFechaHasta"
        Me.lblFechaHasta.Size = New System.Drawing.Size(90, 17)
        Me.lblFechaHasta.TabIndex = 4
        Me.lblFechaHasta.Text = "Fecha Hasta:"
        '
        'dtpFechaHasta
        '
        Me.dtpFechaHasta.Enabled = False
        Me.dtpFechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFechaHasta.Location = New System.Drawing.Point(120, 78)
        Me.dtpFechaHasta.Name = "dtpFechaHasta"
        Me.dtpFechaHasta.Size = New System.Drawing.Size(120, 22)
        Me.dtpFechaHasta.TabIndex = 5
        '
        'chkFechaHasta
        '
        Me.chkFechaHasta.AutoSize = True
        Me.chkFechaHasta.Location = New System.Drawing.Point(250, 80)
        Me.chkFechaHasta.Name = "chkFechaHasta"
        Me.chkFechaHasta.Size = New System.Drawing.Size(122, 21)
        Me.chkFechaHasta.TabIndex = 6
        Me.chkFechaHasta.Text = "Sin fecha de fin"
        Me.chkFechaHasta.UseVisualStyleBackColor = True
        '
        'lblObservaciones
        '
        Me.lblObservaciones.AutoSize = True
        Me.lblObservaciones.Location = New System.Drawing.Point(12, 114)
        Me.lblObservaciones.Name = "lblObservaciones"
        Me.lblObservaciones.Size = New System.Drawing.Size(103, 17)
        Me.lblObservaciones.TabIndex = 7
        Me.lblObservaciones.Text = "Observaciones:"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Location = New System.Drawing.Point(120, 111)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObservaciones.Size = New System.Drawing.Size(252, 80)
        Me.txtObservaciones.TabIndex = 8
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Location = New System.Drawing.Point(297, 526)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(75, 23)
        Me.btnGuardar.TabIndex = 9
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancelar.Location = New System.Drawing.Point(216, 526)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 10
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'pnlDetallesEspecificos
        '
        Me.pnlDetallesEspecificos.Controls.Add(Me.txtTurnoReten)
        Me.pnlDetallesEspecificos.Controls.Add(Me.lblTurnoReten)
        Me.pnlDetallesEspecificos.Controls.Add(Me.txtDiagnostico)
        Me.pnlDetallesEspecificos.Controls.Add(Me.lblDiagnostico)
        Me.pnlDetallesEspecificos.Controls.Add(Me.txtResolucion)
        Me.pnlDetallesEspecificos.Controls.Add(Me.lblResolucion)
        Me.pnlDetallesEspecificos.Location = New System.Drawing.Point(15, 197)
        Me.pnlDetallesEspecificos.Name = "pnlDetallesEspecificos"
        Me.pnlDetallesEspecificos.Size = New System.Drawing.Size(357, 93)
        Me.pnlDetallesEspecificos.TabIndex = 11
        '
        'txtTurnoReten
        '
        Me.txtTurnoReten.Location = New System.Drawing.Point(105, 61)
        Me.txtTurnoReten.Name = "txtTurnoReten"
        Me.txtTurnoReten.Size = New System.Drawing.Size(249, 22)
        Me.txtTurnoReten.TabIndex = 5
        Me.txtTurnoReten.Visible = False
        '
        'lblTurnoReten
        '
        Me.lblTurnoReten.AutoSize = True
        Me.lblTurnoReten.Location = New System.Drawing.Point(-3, 64)
        Me.lblTurnoReten.Name = "lblTurnoReten"
        Me.lblTurnoReten.Size = New System.Drawing.Size(91, 17)
        Me.lblTurnoReten.TabIndex = 4
        Me.lblTurnoReten.Text = "Turno Retén:"
        Me.lblTurnoReten.Visible = False
        '
        'txtDiagnostico
        '
        Me.txtDiagnostico.Location = New System.Drawing.Point(105, 33)
        Me.txtDiagnostico.Name = "txtDiagnostico"
        Me.txtDiagnostico.Size = New System.Drawing.Size(249, 22)
        Me.txtDiagnostico.TabIndex = 3
        Me.txtDiagnostico.Visible = False
        '
        'lblDiagnostico
        '
        Me.lblDiagnostico.AutoSize = True
        Me.lblDiagnostico.Location = New System.Drawing.Point(-3, 36)
        Me.lblDiagnostico.Name = "lblDiagnostico"
        Me.lblDiagnostico.Size = New System.Drawing.Size(87, 17)
        Me.lblDiagnostico.TabIndex = 2
        Me.lblDiagnostico.Text = "Diagnóstico:"
        Me.lblDiagnostico.Visible = False
        '
        'txtResolucion
        '
        Me.txtResolucion.Location = New System.Drawing.Point(105, 5)
        Me.txtResolucion.Name = "txtResolucion"
        Me.txtResolucion.Size = New System.Drawing.Size(249, 22)
        Me.txtResolucion.TabIndex = 1
        Me.txtResolucion.Visible = False
        '
        'lblResolucion
        '
        Me.lblResolucion.AutoSize = True
        Me.lblResolucion.Location = New System.Drawing.Point(-3, 8)
        Me.lblResolucion.Name = "lblResolucion"
        Me.lblResolucion.Size = New System.Drawing.Size(83, 17)
        Me.lblResolucion.TabIndex = 0
        Me.lblResolucion.Text = "Resolución:"
        Me.lblResolucion.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnEliminarAdjunto)
        Me.GroupBox1.Controls.Add(Me.btnVerAdjunto)
        Me.GroupBox1.Controls.Add(Me.btnAdjuntar)
        Me.GroupBox1.Controls.Add(Me.dgvAdjuntos)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 296)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(357, 224)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Archivos Adjuntos"
        '
        'btnEliminarAdjunto
        '
        Me.btnEliminarAdjunto.Location = New System.Drawing.Point(245, 189)
        Me.btnEliminarAdjunto.Name = "btnEliminarAdjunto"
        Me.btnEliminarAdjunto.Size = New System.Drawing.Size(106, 29)
        Me.btnEliminarAdjunto.TabIndex = 3
        Me.btnEliminarAdjunto.Text = "Eliminar"
        Me.btnEliminarAdjunto.UseVisualStyleBackColor = True
        '
        'btnVerAdjunto
        '
        Me.btnVerAdjunto.Location = New System.Drawing.Point(125, 189)
        Me.btnVerAdjunto.Name = "btnVerAdjunto"
        Me.btnVerAdjunto.Size = New System.Drawing.Size(114, 29)
        Me.btnVerAdjunto.TabIndex = 2
        Me.btnVerAdjunto.Text = "Ver/Abrir"
        Me.btnVerAdjunto.UseVisualStyleBackColor = True
        '
        'btnAdjuntar
        '
        Me.btnAdjuntar.Location = New System.Drawing.Point(6, 189)
        Me.btnAdjuntar.Name = "btnAdjuntar"
        Me.btnAdjuntar.Size = New System.Drawing.Size(113, 29)
        Me.btnAdjuntar.TabIndex = 1
        Me.btnAdjuntar.Text = "Adjuntar..."
        Me.btnAdjuntar.UseVisualStyleBackColor = True
        '
        'dgvAdjuntos
        '
        Me.dgvAdjuntos.AllowUserToAddRows = False
        Me.dgvAdjuntos.AllowUserToDeleteRows = False
        Me.dgvAdjuntos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAdjuntos.Location = New System.Drawing.Point(6, 21)
        Me.dgvAdjuntos.Name = "dgvAdjuntos"
        Me.dgvAdjuntos.ReadOnly = True
        Me.dgvAdjuntos.RowHeadersVisible = False
        Me.dgvAdjuntos.RowHeadersWidth = 51
        Me.dgvAdjuntos.RowTemplate.Height = 24
        Me.dgvAdjuntos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAdjuntos.Size = New System.Drawing.Size(345, 162)
        Me.dgvAdjuntos.TabIndex = 0
        '
        'frmFuncionarioEstadoTransitorio
        '
        Me.AcceptButton = Me.btnGuardar
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancelar
        Me.ClientSize = New System.Drawing.Size(384, 561)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.pnlDetallesEspecificos)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.txtObservaciones)
        Me.Controls.Add(Me.lblObservaciones)
        Me.Controls.Add(Me.chkFechaHasta)
        Me.Controls.Add(Me.dtpFechaHasta)
        Me.Controls.Add(Me.lblFechaHasta)
        Me.Controls.Add(Me.dtpFechaDesde)
        Me.Controls.Add(Me.lblFechaDesde)
        Me.Controls.Add(Me.cboTipoEstado)
        Me.Controls.Add(Me.lblTipoEstado)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmFuncionarioEstadoTransitorio"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Estado Transitorio"
        Me.pnlDetallesEspecificos.ResumeLayout(False)
        Me.pnlDetallesEspecificos.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.dgvAdjuntos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTipoEstado As Label
    Friend WithEvents cboTipoEstado As ComboBox
    Friend WithEvents lblFechaDesde As Label
    Friend WithEvents dtpFechaDesde As DateTimePicker
    Friend WithEvents lblFechaHasta As Label
    Friend WithEvents dtpFechaHasta As DateTimePicker
    Friend WithEvents chkFechaHasta As CheckBox
    Friend WithEvents lblObservaciones As Label
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents pnlDetallesEspecificos As Panel
    Friend WithEvents txtResolucion As TextBox
    Friend WithEvents lblResolucion As Label
    Friend WithEvents txtDiagnostico As TextBox
    Friend WithEvents lblDiagnostico As Label
    Friend WithEvents txtTurnoReten As TextBox
    Friend WithEvents lblTurnoReten As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnEliminarAdjunto As Button
    Friend WithEvents btnVerAdjunto As Button
    Friend WithEvents btnAdjuntar As Button
    Friend WithEvents dgvAdjuntos As DataGridView
End Class