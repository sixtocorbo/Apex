<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmResumenCantidades
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
        Me._layoutRoot = New System.Windows.Forms.TableLayoutPanel()
        Me._panelHeader = New System.Windows.Forms.Panel()
        Me._lblUltimaActualizacion = New System.Windows.Forms.Label()
        Me._btnActualizar = New System.Windows.Forms.Button()
        Me._dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me._flowCards = New System.Windows.Forms.FlowLayoutPanel()
        Me._tableDetalles = New System.Windows.Forms.TableLayoutPanel()
        Me._groupLicencias = New System.Windows.Forms.GroupBox()
        Me._lblLicenciasSinDatos = New System.Windows.Forms.Label()
        Me._dgvLicencias = New System.Windows.Forms.DataGridView()
        Me._groupPresencias = New System.Windows.Forms.GroupBox()
        Me._lblPresenciasSinDatos = New System.Windows.Forms.Label()
        Me._dgvPresencias = New System.Windows.Forms.DataGridView()
        Me._lblTotalFuncionarios = New System.Windows.Forms.Label()
        Me._lblActivos = New System.Windows.Forms.Label()
        Me._lblInactivos = New System.Windows.Forms.Label()
        Me._lblPresentes = New System.Windows.Forms.Label()
        Me._lblFrancos = New System.Windows.Forms.Label()
        Me._lblLicencias = New System.Windows.Forms.Label()
        Me._lblAusentes = New System.Windows.Forms.Label()
        Me._layoutRoot.SuspendLayout()
        Me._panelHeader.SuspendLayout()
        Me._tableDetalles.SuspendLayout()
        Me._groupLicencias.SuspendLayout()
        CType(Me._dgvLicencias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._groupPresencias.SuspendLayout()
        CType(Me._dgvPresencias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_layoutRoot
        '
        Me._layoutRoot.ColumnCount = 1
        Me._layoutRoot.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._layoutRoot.Controls.Add(Me._panelHeader, 0, 0)
        Me._layoutRoot.Controls.Add(Me._flowCards, 0, 1)
        Me._layoutRoot.Controls.Add(Me._tableDetalles, 0, 2)
        Me._layoutRoot.Dock = System.Windows.Forms.DockStyle.Fill
        Me._layoutRoot.Location = New System.Drawing.Point(0, 0)
        Me._layoutRoot.Name = "_layoutRoot"
        Me._layoutRoot.RowCount = 3
        Me._layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72.0!))
        Me._layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize))
        Me._layoutRoot.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me._layoutRoot.Size = New System.Drawing.Size(1084, 721)
        Me._layoutRoot.TabIndex = 0
        '
        '_panelHeader
        '
        Me._panelHeader.Controls.Add(Me._lblUltimaActualizacion)
        Me._panelHeader.Controls.Add(Me._btnActualizar)
        Me._panelHeader.Controls.Add(Me._dtpFecha)
        Me._panelHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me._panelHeader.Location = New System.Drawing.Point(3, 3)
        Me._panelHeader.Name = "_panelHeader"
        Me._panelHeader.Padding = New System.Windows.Forms.Padding(16)
        Me._panelHeader.Size = New System.Drawing.Size(1078, 66)
        Me._panelHeader.TabIndex = 0
        '
        '_lblUltimaActualizacion
        '
        Me._lblUltimaActualizacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._lblUltimaActualizacion.AutoSize = True
        Me._lblUltimaActualizacion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic)
        Me._lblUltimaActualizacion.Location = New System.Drawing.Point(925, 25)
        Me._lblUltimaActualizacion.Name = "_lblUltimaActualizacion"
        Me._lblUltimaActualizacion.Size = New System.Drawing.Size(134, 15)
        Me._lblUltimaActualizacion.TabIndex = 2
        Me._lblUltimaActualizacion.Text = "Actualizado: (ninguna)"
        Me._lblUltimaActualizacion.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        '_btnActualizar
        '
        Me._btnActualizar.AutoSize = True
        Me._btnActualizar.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me._btnActualizar.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me._btnActualizar.Location = New System.Drawing.Point(355, 13)
        Me._btnActualizar.Margin = New System.Windows.Forms.Padding(16, 0, 0, 0)
        Me._btnActualizar.Name = "_btnActualizar"
        Me._btnActualizar.Padding = New System.Windows.Forms.Padding(14, 6, 14, 6)
        Me._btnActualizar.Size = New System.Drawing.Size(117, 40)
        Me._btnActualizar.TabIndex = 1
        Me._btnActualizar.Text = "Actualizar"
        Me._btnActualizar.UseVisualStyleBackColor = True
        '
        '_dtpFecha
        '
        Me._dtpFecha.CustomFormat = "dddd dd 'de' MMMM yyyy"
        Me._dtpFecha.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me._dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me._dtpFecha.Location = New System.Drawing.Point(19, 20)
        Me._dtpFecha.Name = "_dtpFecha"
        Me._dtpFecha.Size = New System.Drawing.Size(320, 25)
        Me._dtpFecha.TabIndex = 0
        '
        '_flowCards
        '
        Me._flowCards.AutoSize = True
        Me._flowCards.Dock = System.Windows.Forms.DockStyle.Fill
        Me._flowCards.Location = New System.Drawing.Point(8, 75)
        Me._flowCards.Name = "_flowCards"
        Me._flowCards.Padding = New System.Windows.Forms.Padding(8, 0, 8, 8)
        Me._flowCards.Size = New System.Drawing.Size(1068, 1)
        Me._flowCards.TabIndex = 1
        '
        '_tableDetalles
        '
        Me._tableDetalles.ColumnCount = 2
        Me._tableDetalles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._tableDetalles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._tableDetalles.Controls.Add(Me._groupLicencias, 0, 0)
        Me._tableDetalles.Controls.Add(Me._groupPresencias, 1, 0)
        Me._tableDetalles.Dock = System.Windows.Forms.DockStyle.Fill
        Me._tableDetalles.Location = New System.Drawing.Point(3, 82)
        Me._tableDetalles.Name = "_tableDetalles"
        Me._tableDetalles.Padding = New System.Windows.Forms.Padding(8)
        Me._tableDetalles.RowCount = 1
        Me._tableDetalles.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me._tableDetalles.Size = New System.Drawing.Size(1078, 636)
        Me._tableDetalles.TabIndex = 2
        '
        '_groupLicencias
        '
        Me._groupLicencias.Controls.Add(Me._lblLicenciasSinDatos)
        Me._groupLicencias.Controls.Add(Me._dgvLicencias)
        Me._groupLicencias.Dock = System.Windows.Forms.DockStyle.Fill
        Me._groupLicencias.Location = New System.Drawing.Point(11, 11)
        Me._groupLicencias.Name = "_groupLicencias"
        Me._groupLicencias.Padding = New System.Windows.Forms.Padding(12)
        Me._groupLicencias.Size = New System.Drawing.Size(525, 614)
        Me._groupLicencias.TabIndex = 0
        Me._groupLicencias.TabStop = False
        Me._groupLicencias.Text = "Licencias por tipo"
        '
        '_lblLicenciasSinDatos
        '
        Me._lblLicenciasSinDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me._lblLicenciasSinDatos.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic)
        Me._lblLicenciasSinDatos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me._lblLicenciasSinDatos.Location = New System.Drawing.Point(12, 28)
        Me._lblLicenciasSinDatos.Name = "_lblLicenciasSinDatos"
        Me._lblLicenciasSinDatos.Size = New System.Drawing.Size(501, 574)
        Me._lblLicenciasSinDatos.TabIndex = 1
        Me._lblLicenciasSinDatos.Text = "No se registran licencias en la fecha seleccionada."
        Me._lblLicenciasSinDatos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me._lblLicenciasSinDatos.Visible = False
        '
        '_dgvLicencias
        '
        Me._dgvLicencias.Dock = System.Windows.Forms.DockStyle.Fill
        Me._dgvLicencias.Location = New System.Drawing.Point(12, 28)
        Me._dgvLicencias.Name = "_dgvLicencias"
        Me._dgvLicencias.Size = New System.Drawing.Size(501, 574)
        Me._dgvLicencias.TabIndex = 0
        '
        '_groupPresencias
        '
        Me._groupPresencias.Controls.Add(Me._lblPresenciasSinDatos)
        Me._groupPresencias.Controls.Add(Me._dgvPresencias)
        Me._groupPresencias.Dock = System.Windows.Forms.DockStyle.Fill
        Me._groupPresencias.Location = New System.Drawing.Point(542, 11)
        Me._groupPresencias.Name = "_groupPresencias"
        Me._groupPresencias.Padding = New System.Windows.Forms.Padding(12)
        Me._groupPresencias.Size = New System.Drawing.Size(525, 614)
        Me._groupPresencias.TabIndex = 1
        Me._groupPresencias.TabStop = False
        Me._groupPresencias.Text = "Presencias por estado"
        '
        '_lblPresenciasSinDatos
        '
        Me._lblPresenciasSinDatos.Dock = System.Windows.Forms.DockStyle.Fill
        Me._lblPresenciasSinDatos.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic)
        Me._lblPresenciasSinDatos.ForeColor = System.Drawing.Color.FromArgb(CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer), CType(CType(90, Byte), Integer))
        Me._lblPresenciasSinDatos.Location = New System.Drawing.Point(12, 28)
        Me._lblPresenciasSinDatos.Name = "_lblPresenciasSinDatos"
        Me._lblPresenciasSinDatos.Size = New System.Drawing.Size(501, 574)
        Me._lblPresenciasSinDatos.TabIndex = 1
        Me._lblPresenciasSinDatos.Text = "No se registran datos de presencia para la fecha seleccionada."
        Me._lblPresenciasSinDatos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me._lblPresenciasSinDatos.Visible = False
        '
        '_dgvPresencias
        '
        Me._dgvPresencias.Dock = System.Windows.Forms.DockStyle.Fill
        Me._dgvPresencias.Location = New System.Drawing.Point(12, 28)
        Me._dgvPresencias.Name = "_dgvPresencias"
        Me._dgvPresencias.Size = New System.Drawing.Size(501, 574)
        Me._dgvPresencias.TabIndex = 0
        '_lblTotalFuncionarios
        '
        Me._lblTotalFuncionarios.AutoSize = False
        Me._lblTotalFuncionarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me._lblTotalFuncionarios.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me._lblTotalFuncionarios.ForeColor = System.Drawing.Color.FromArgb(28, 41, 56)
        Me._lblTotalFuncionarios.MinimumSize = New System.Drawing.Size(0, 60)
        Me._lblTotalFuncionarios.Name = "_lblTotalFuncionarios"
        Me._lblTotalFuncionarios.Text = "0"
        Me._lblTotalFuncionarios.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lblActivos
        '
        Me._lblActivos.AutoSize = False
        Me._lblActivos.Dock = System.Windows.Forms.DockStyle.Fill
        Me._lblActivos.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me._lblActivos.ForeColor = System.Drawing.Color.FromArgb(28, 41, 56)
        Me._lblActivos.MinimumSize = New System.Drawing.Size(0, 60)
        Me._lblActivos.Name = "_lblActivos"
        Me._lblActivos.Text = "0"
        Me._lblActivos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lblInactivos
        '
        Me._lblInactivos.AutoSize = False
        Me._lblInactivos.Dock = System.Windows.Forms.DockStyle.Fill
        Me._lblInactivos.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me._lblInactivos.ForeColor = System.Drawing.Color.FromArgb(28, 41, 56)
        Me._lblInactivos.MinimumSize = New System.Drawing.Size(0, 60)
        Me._lblInactivos.Name = "_lblInactivos"
        Me._lblInactivos.Text = "0"
        Me._lblInactivos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lblPresentes
        '
        Me._lblPresentes.AutoSize = False
        Me._lblPresentes.Dock = System.Windows.Forms.DockStyle.Fill
        Me._lblPresentes.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me._lblPresentes.ForeColor = System.Drawing.Color.FromArgb(28, 41, 56)
        Me._lblPresentes.MinimumSize = New System.Drawing.Size(0, 60)
        Me._lblPresentes.Name = "_lblPresentes"
        Me._lblPresentes.Text = "0"
        Me._lblPresentes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lblFrancos
        '
        Me._lblFrancos.AutoSize = False
        Me._lblFrancos.Dock = System.Windows.Forms.DockStyle.Fill
        Me._lblFrancos.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me._lblFrancos.ForeColor = System.Drawing.Color.FromArgb(28, 41, 56)
        Me._lblFrancos.MinimumSize = New System.Drawing.Size(0, 60)
        Me._lblFrancos.Name = "_lblFrancos"
        Me._lblFrancos.Text = "0"
        Me._lblFrancos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lblLicencias
        '
        Me._lblLicencias.AutoSize = False
        Me._lblLicencias.Dock = System.Windows.Forms.DockStyle.Fill
        Me._lblLicencias.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me._lblLicencias.ForeColor = System.Drawing.Color.FromArgb(28, 41, 56)
        Me._lblLicencias.MinimumSize = New System.Drawing.Size(0, 60)
        Me._lblLicencias.Name = "_lblLicencias"
        Me._lblLicencias.Text = "0"
        Me._lblLicencias.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        '_lblAusentes
        '
        Me._lblAusentes.AutoSize = False
        Me._lblAusentes.Dock = System.Windows.Forms.DockStyle.Fill
        Me._lblAusentes.Font = New System.Drawing.Font("Segoe UI", 22.0!, System.Drawing.FontStyle.Bold)
        Me._lblAusentes.ForeColor = System.Drawing.Color.FromArgb(28, 41, 56)
        Me._lblAusentes.MinimumSize = New System.Drawing.Size(0, 60)
        Me._lblAusentes.Name = "_lblAusentes"
        Me._lblAusentes.Text = "0"
        Me._lblAusentes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmResumenCantidades
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1084, 721)
        Me.Controls.Add(Me._layoutRoot)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimumSize = New System.Drawing.Size(980, 720)
        Me.Name = "frmResumenCantidades"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Resumen de Cantidades"
        Me._layoutRoot.ResumeLayout(False)
        Me._layoutRoot.PerformLayout()
        Me._panelHeader.ResumeLayout(False)
        Me._panelHeader.PerformLayout()
        Me._tableDetalles.ResumeLayout(False)
        Me._groupLicencias.ResumeLayout(False)
        CType(Me._dgvLicencias, System.ComponentModel.ISupportInitialize).EndInit()
        Me._groupPresencias.ResumeLayout(False)
        CType(Me._dgvPresencias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents _layoutRoot As TableLayoutPanel
    Friend WithEvents _panelHeader As Panel
    Friend WithEvents _dtpFecha As DateTimePicker
    Friend WithEvents _btnActualizar As Button
    Friend WithEvents _lblUltimaActualizacion As Label
    Friend WithEvents _flowCards As FlowLayoutPanel
    Friend WithEvents _tableDetalles As TableLayoutPanel
    Friend WithEvents _groupLicencias As GroupBox
    Friend WithEvents _groupPresencias As GroupBox
    Friend WithEvents _dgvLicencias As DataGridView
    Friend WithEvents _dgvPresencias As DataGridView
    Friend WithEvents _lblLicenciasSinDatos As Label
    Friend WithEvents _lblPresenciasSinDatos As Label
    Friend WithEvents _lblTotalFuncionarios As Label
    Friend WithEvents _lblActivos As Label
    Friend WithEvents _lblInactivos As Label
    Friend WithEvents _lblPresentes As Label
    Friend WithEvents _lblFrancos As Label
    Friend WithEvents _lblLicencias As Label
    Friend WithEvents _lblAusentes As Label
End Class