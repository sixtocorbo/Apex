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
        Me.btnVerAdjunto = New System.Windows.Forms.Button()
        Me.btnEliminarAdjunto = New System.Windows.Forms.Button()
        Me.btnAdjuntar = New System.Windows.Forms.Button()
        Me.dgvAdjuntos = New System.Windows.Forms.DataGridView()
        Me.wbPreview = New System.Windows.Forms.WebBrowser()
        Me.pbPreview = New System.Windows.Forms.PictureBox()
        Me.lblPreviewNotAvailable = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.dgvAdjuntos, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnGuardar
        '
        Me.btnGuardar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGuardar.Location = New System.Drawing.Point(697, 426)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(75, 23)
        Me.btnGuardar.TabIndex = 0
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelar.Location = New System.Drawing.Point(616, 426)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 1
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'cboTipoEstado
        '
        Me.cboTipoEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoEstado.FormattingEnabled = True
        Me.cboTipoEstado.Location = New System.Drawing.Point(111, 12)
        Me.cboTipoEstado.Name = "cboTipoEstado"
        Me.cboTipoEstado.Size = New System.Drawing.Size(268, 21)
        Me.cboTipoEstado.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(84, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Tipo de Estado:"
        '
        'lblFechaDesde
        '
        Me.lblFechaDesde.AutoSize = True
        Me.lblFechaDesde.Location = New System.Drawing.Point(12, 45)
        Me.lblFechaDesde.Name = "lblFechaDesde"
        Me.lblFechaDesde.Size = New System.Drawing.Size(74, 13)
        Me.lblFechaDesde.TabIndex = 4
        Me.lblFechaDesde.Text = "Fecha Desde:"
        '
        'dtpFechaDesde
        '
        Me.dtpFechaDesde.Location = New System.Drawing.Point(111, 39)
        Me.dtpFechaDesde.Name = "dtpFechaDesde"
        Me.dtpFechaDesde.Size = New System.Drawing.Size(200, 20)
        Me.dtpFechaDesde.TabIndex = 5
        '
        'dtpFechaHasta
        '
        Me.dtpFechaHasta.Location = New System.Drawing.Point(111, 65)
        Me.dtpFechaHasta.Name = "dtpFechaHasta"
        Me.dtpFechaHasta.Size = New System.Drawing.Size(200, 20)
        Me.dtpFechaHasta.TabIndex = 7
        '
        'lblFechaHasta
        '
        Me.lblFechaHasta.AutoSize = True
        Me.lblFechaHasta.Location = New System.Drawing.Point(12, 71)
        Me.lblFechaHasta.Name = "lblFechaHasta"
        Me.lblFechaHasta.Size = New System.Drawing.Size(71, 13)
        Me.lblFechaHasta.TabIndex = 6
        Me.lblFechaHasta.Text = "Fecha Hasta:"
        '
        'chkFechaHasta
        '
        Me.chkFechaHasta.AutoSize = True
        Me.chkFechaHasta.Location = New System.Drawing.Point(317, 68)
        Me.chkFechaHasta.Name = "chkFechaHasta"
        Me.chkFechaHasta.Size = New System.Drawing.Size(62, 17)
        Me.chkFechaHasta.TabIndex = 8
        Me.chkFechaHasta.Text = "Sin Fin"
        Me.chkFechaHasta.UseVisualStyleBackColor = True
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Location = New System.Drawing.Point(111, 91)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.Size = New System.Drawing.Size(268, 49)
        Me.txtObservaciones.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(12, 94)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(81, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Observaciones:"
        '
        'lblResolucion
        '
        Me.lblResolucion.AutoSize = True
        Me.lblResolucion.Location = New System.Drawing.Point(404, 15)
        Me.lblResolucion.Name = "lblResolucion"
        Me.lblResolucion.Size = New System.Drawing.Size(63, 13)
        Me.lblResolucion.TabIndex = 12
        Me.lblResolucion.Text = "Resolución:"
        '
        'txtResolucion
        '
        Me.txtResolucion.Location = New System.Drawing.Point(473, 12)
        Me.txtResolucion.Name = "txtResolucion"
        Me.txtResolucion.Size = New System.Drawing.Size(299, 20)
        Me.txtResolucion.TabIndex = 11
        '
        'lblDiagnostico
        '
        Me.lblDiagnostico.AutoSize = True
        Me.lblDiagnostico.Location = New System.Drawing.Point(404, 42)
        Me.lblDiagnostico.Name = "lblDiagnostico"
        Me.lblDiagnostico.Size = New System.Drawing.Size(66, 13)
        Me.lblDiagnostico.TabIndex = 14
        Me.lblDiagnostico.Text = "Diagnóstico:"
        '
        'txtDiagnostico
        '
        Me.txtDiagnostico.Location = New System.Drawing.Point(473, 39)
        Me.txtDiagnostico.Name = "txtDiagnostico"
        Me.txtDiagnostico.Size = New System.Drawing.Size(299, 20)
        Me.txtDiagnostico.TabIndex = 13
        '
        'lblTurnoReten
        '
        Me.lblTurnoReten.AutoSize = True
        Me.lblTurnoReten.Location = New System.Drawing.Point(404, 68)
        Me.lblTurnoReten.Name = "lblTurnoReten"
        Me.lblTurnoReten.Size = New System.Drawing.Size(38, 13)
        Me.lblTurnoReten.TabIndex = 16
        Me.lblTurnoReten.Text = "Turno:"
        '
        'txtTurnoReten
        '
        Me.txtTurnoReten.Location = New System.Drawing.Point(473, 65)
        Me.txtTurnoReten.Name = "txtTurnoReten"
        Me.txtTurnoReten.Size = New System.Drawing.Size(299, 20)
        Me.txtTurnoReten.TabIndex = 15
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.SplitContainer1)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 146)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(757, 274)
        Me.GroupBox1.TabIndex = 17
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Archivos Adjuntos"
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(3, 16)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnVerAdjunto)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnEliminarAdjunto)
        Me.SplitContainer1.Panel1.Controls.Add(Me.btnAdjuntar)
        Me.SplitContainer1.Panel1.Controls.Add(Me.dgvAdjuntos)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.lblPreviewNotAvailable)
        Me.SplitContainer1.Panel2.Controls.Add(Me.wbPreview)
        Me.SplitContainer1.Panel2.Controls.Add(Me.pbPreview)
        Me.SplitContainer1.Size = New System.Drawing.Size(751, 255)
        Me.SplitContainer1.SplitterDistance = 350
        Me.SplitContainer1.TabIndex = 4
        '
        'btnVerAdjunto
        '
        Me.btnVerAdjunto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnVerAdjunto.Location = New System.Drawing.Point(94, 229)
        Me.btnVerAdjunto.Name = "btnVerAdjunto"
        Me.btnVerAdjunto.Size = New System.Drawing.Size(85, 23)
        Me.btnVerAdjunto.TabIndex = 3
        Me.btnVerAdjunto.Text = "Abrir Archivo"
        Me.btnVerAdjunto.UseVisualStyleBackColor = True
        '
        'btnEliminarAdjunto
        '
        Me.btnEliminarAdjunto.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarAdjunto.Location = New System.Drawing.Point(272, 229)
        Me.btnEliminarAdjunto.Name = "btnEliminarAdjunto"
        Me.btnEliminarAdjunto.Size = New System.Drawing.Size(75, 23)
        Me.btnEliminarAdjunto.TabIndex = 2
        Me.btnEliminarAdjunto.Text = "Eliminar"
        Me.btnEliminarAdjunto.UseVisualStyleBackColor = True
        '
        'btnAdjuntar
        '
        Me.btnAdjuntar.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAdjuntar.Location = New System.Drawing.Point(3, 229)
        Me.btnAdjuntar.Name = "btnAdjuntar"
        Me.btnAdjuntar.Size = New System.Drawing.Size(75, 23)
        Me.btnAdjuntar.TabIndex = 1
        Me.btnAdjuntar.Text = "Adjuntar"
        Me.btnAdjuntar.UseVisualStyleBackColor = True
        '
        'dgvAdjuntos
        '
        Me.dgvAdjuntos.AllowUserToAddRows = False
        Me.dgvAdjuntos.AllowUserToDeleteRows = False
        Me.dgvAdjuntos.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvAdjuntos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAdjuntos.Location = New System.Drawing.Point(3, 3)
        Me.dgvAdjuntos.MultiSelect = False
        Me.dgvAdjuntos.Name = "dgvAdjuntos"
        Me.dgvAdjuntos.ReadOnly = True
        Me.dgvAdjuntos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAdjuntos.Size = New System.Drawing.Size(344, 220)
        Me.dgvAdjuntos.TabIndex = 0
        '
        'wbPreview
        '
        Me.wbPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.wbPreview.Location = New System.Drawing.Point(0, 0)
        Me.wbPreview.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbPreview.Name = "wbPreview"
        Me.wbPreview.Size = New System.Drawing.Size(397, 255)
        Me.wbPreview.TabIndex = 1
        Me.wbPreview.Visible = False
        '
        'pbPreview
        '
        Me.pbPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbPreview.Location = New System.Drawing.Point(0, 0)
        Me.pbPreview.Name = "pbPreview"
        Me.pbPreview.Size = New System.Drawing.Size(397, 255)
        Me.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbPreview.TabIndex = 0
        Me.pbPreview.TabStop = False
        Me.pbPreview.Visible = False
        '
        'lblPreviewNotAvailable
        '
        Me.lblPreviewNotAvailable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblPreviewNotAvailable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreviewNotAvailable.Location = New System.Drawing.Point(0, 0)
        Me.lblPreviewNotAvailable.Name = "lblPreviewNotAvailable"
        Me.lblPreviewNotAvailable.Size = New System.Drawing.Size(397, 255)
        Me.lblPreviewNotAvailable.TabIndex = 2
        Me.lblPreviewNotAvailable.Text = "Seleccione un archivo para previsualizar"
        Me.lblPreviewNotAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmFuncionarioEstadoTransitorio
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 461)
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
        Me.MinimumSize = New System.Drawing.Size(800, 500)
        Me.Name = "frmFuncionarioEstadoTransitorio"
        Me.Text = "Estado Transitorio"
        Me.GroupBox1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        CType(Me.dgvAdjuntos, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents btnVerAdjunto As Button
    Friend WithEvents btnEliminarAdjunto As Button
    Friend WithEvents btnAdjuntar As Button
    Friend WithEvents dgvAdjuntos As DataGridView
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents wbPreview As WebBrowser
    Friend WithEvents pbPreview As PictureBox
    Friend WithEvents lblPreviewNotAvailable As Label
End Class