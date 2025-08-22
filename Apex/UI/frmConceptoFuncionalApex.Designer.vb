<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmConceptoFuncionalApex
    Inherits System.Windows.Forms.Form

    ' ... (Contenido generado por el diseñador) ...

    Friend WithEvents PanelFiltros As Panel
    Friend WithEvents dtpFechaFin As DateTimePicker
    Friend WithEvents dtpFechaInicio As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnBuscarFuncionario As Button
    Friend WithEvents txtFuncionarioSeleccionado As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents PanelPrincipal As Panel
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents dgvLicenciasMedicas As DataGridView
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents dgvSanciones As DataGridView
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents dgvObservaciones As DataGridView
    Friend WithEvents PanelFooter As Panel
    Friend WithEvents lblTemporal As Label
    Friend WithEvents btnInforme As Button

    Private Sub InitializeComponent()
        Me.PanelFiltros = New System.Windows.Forms.Panel()
        Me.btnBuscarFuncionario = New System.Windows.Forms.Button()
        Me.txtFuncionarioSeleccionado = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.dtpFechaFin = New System.Windows.Forms.DateTimePicker()
        Me.dtpFechaInicio = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PanelPrincipal = New System.Windows.Forms.Panel()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.dgvLicenciasMedicas = New System.Windows.Forms.DataGridView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.dgvSanciones = New System.Windows.Forms.DataGridView()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.dgvObservaciones = New System.Windows.Forms.DataGridView()
        Me.PanelFooter = New System.Windows.Forms.Panel()
        Me.btnInforme = New System.Windows.Forms.Button()
        Me.lblTemporal = New System.Windows.Forms.Label()
        Me.PanelFiltros.SuspendLayout()
        Me.PanelPrincipal.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.dgvLicenciasMedicas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.dgvSanciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.dgvObservaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelFooter.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelFiltros
        '
        Me.PanelFiltros.Controls.Add(Me.btnBuscarFuncionario)
        Me.PanelFiltros.Controls.Add(Me.txtFuncionarioSeleccionado)
        Me.PanelFiltros.Controls.Add(Me.Label3)
        Me.PanelFiltros.Controls.Add(Me.dtpFechaFin)
        Me.PanelFiltros.Controls.Add(Me.dtpFechaInicio)
        Me.PanelFiltros.Controls.Add(Me.Label2)
        Me.PanelFiltros.Controls.Add(Me.Label1)
        Me.PanelFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelFiltros.Location = New System.Drawing.Point(0, 0)
        Me.PanelFiltros.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelFiltros.Name = "PanelFiltros"
        Me.PanelFiltros.Size = New System.Drawing.Size(1200, 123)
        Me.PanelFiltros.TabIndex = 0
        '
        'btnBuscarFuncionario
        '
        Me.btnBuscarFuncionario.Location = New System.Drawing.Point(495, 45)
        Me.btnBuscarFuncionario.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnBuscarFuncionario.Name = "btnBuscarFuncionario"
        Me.btnBuscarFuncionario.Size = New System.Drawing.Size(42, 35)
        Me.btnBuscarFuncionario.TabIndex = 6
        Me.btnBuscarFuncionario.Text = "..."
        Me.btnBuscarFuncionario.UseVisualStyleBackColor = True
        '
        'txtFuncionarioSeleccionado
        '
        Me.txtFuncionarioSeleccionado.Location = New System.Drawing.Point(22, 48)
        Me.txtFuncionarioSeleccionado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtFuncionarioSeleccionado.Name = "txtFuncionarioSeleccionado"
        Me.txtFuncionarioSeleccionado.ReadOnly = True
        Me.txtFuncionarioSeleccionado.Size = New System.Drawing.Size(462, 26)
        Me.txtFuncionarioSeleccionado.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 23)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 20)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Funcionario"
        '
        'dtpFechaFin
        '
        Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaFin.Location = New System.Drawing.Point(780, 48)
        Me.dtpFechaFin.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFechaFin.Name = "dtpFechaFin"
        Me.dtpFechaFin.Size = New System.Drawing.Size(146, 26)
        Me.dtpFechaFin.TabIndex = 3
        '
        'dtpFechaInicio
        '
        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaInicio.Location = New System.Drawing.Point(597, 48)
        Me.dtpFechaInicio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dtpFechaInicio.Name = "dtpFechaInicio"
        Me.dtpFechaInicio.Size = New System.Drawing.Size(146, 26)
        Me.dtpFechaInicio.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(776, 23)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 20)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Hasta"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(592, 23)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Desde"
        '
        'PanelPrincipal
        '
        Me.PanelPrincipal.Controls.Add(Me.TabControl1)
        Me.PanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelPrincipal.Location = New System.Drawing.Point(0, 123)
        Me.PanelPrincipal.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelPrincipal.Name = "PanelPrincipal"
        Me.PanelPrincipal.Size = New System.Drawing.Size(1200, 492)
        Me.PanelPrincipal.TabIndex = 1
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1200, 492)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.dgvLicenciasMedicas)
        Me.TabPage1.Location = New System.Drawing.Point(4, 29)
        Me.TabPage1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPage1.Size = New System.Drawing.Size(1192, 459)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Licencias Médicas"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'dgvLicenciasMedicas
        '
        Me.dgvLicenciasMedicas.AllowUserToAddRows = False
        Me.dgvLicenciasMedicas.AllowUserToDeleteRows = False
        Me.dgvLicenciasMedicas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLicenciasMedicas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvLicenciasMedicas.Location = New System.Drawing.Point(4, 5)
        Me.dgvLicenciasMedicas.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvLicenciasMedicas.Name = "dgvLicenciasMedicas"
        Me.dgvLicenciasMedicas.ReadOnly = True
        Me.dgvLicenciasMedicas.RowHeadersWidth = 62
        Me.dgvLicenciasMedicas.Size = New System.Drawing.Size(1184, 449)
        Me.dgvLicenciasMedicas.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.dgvSanciones)
        Me.TabPage2.Location = New System.Drawing.Point(4, 29)
        Me.TabPage2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPage2.Size = New System.Drawing.Size(1192, 459)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Sanciones Graves"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'dgvSanciones
        '
        Me.dgvSanciones.AllowUserToAddRows = False
        Me.dgvSanciones.AllowUserToDeleteRows = False
        Me.dgvSanciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSanciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSanciones.Location = New System.Drawing.Point(4, 5)
        Me.dgvSanciones.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvSanciones.Name = "dgvSanciones"
        Me.dgvSanciones.ReadOnly = True
        Me.dgvSanciones.RowHeadersWidth = 62
        Me.dgvSanciones.Size = New System.Drawing.Size(1184, 449)
        Me.dgvSanciones.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.dgvObservaciones)
        Me.TabPage3.Location = New System.Drawing.Point(4, 29)
        Me.TabPage3.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(1192, 459)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Observaciones (Puntuales y Leves)"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'dgvObservaciones
        '
        Me.dgvObservaciones.AllowUserToAddRows = False
        Me.dgvObservaciones.AllowUserToDeleteRows = False
        Me.dgvObservaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvObservaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvObservaciones.Location = New System.Drawing.Point(0, 0)
        Me.dgvObservaciones.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvObservaciones.Name = "dgvObservaciones"
        Me.dgvObservaciones.ReadOnly = True
        Me.dgvObservaciones.RowHeadersWidth = 62
        Me.dgvObservaciones.Size = New System.Drawing.Size(1192, 459)
        Me.dgvObservaciones.TabIndex = 0
        '
        'PanelFooter
        '
        Me.PanelFooter.Controls.Add(Me.btnInforme)
        Me.PanelFooter.Controls.Add(Me.lblTemporal)
        Me.PanelFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelFooter.Location = New System.Drawing.Point(0, 615)
        Me.PanelFooter.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelFooter.Name = "PanelFooter"
        Me.PanelFooter.Size = New System.Drawing.Size(1200, 77)
        Me.PanelFooter.TabIndex = 2
        '
        'btnInforme
        '
        Me.btnInforme.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInforme.Location = New System.Drawing.Point(1026, 23)
        Me.btnInforme.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnInforme.Name = "btnInforme"
        Me.btnInforme.Size = New System.Drawing.Size(156, 35)
        Me.btnInforme.TabIndex = 1
        Me.btnInforme.Text = "Generar Informe"
        Me.btnInforme.UseVisualStyleBackColor = True
        '
        'lblTemporal
        '
        Me.lblTemporal.AutoSize = True
        Me.lblTemporal.Location = New System.Drawing.Point(18, 31)
        Me.lblTemporal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTemporal.Name = "lblTemporal"
        Me.lblTemporal.Size = New System.Drawing.Size(135, 20)
        Me.lblTemporal.TabIndex = 0
        Me.lblTemporal.Text = "Período evaluado:"
        '
        'frmConceptoFuncionalApex
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1200, 692)
        Me.Controls.Add(Me.PanelPrincipal)
        Me.Controls.Add(Me.PanelFooter)
        Me.Controls.Add(Me.PanelFiltros)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "frmConceptoFuncionalApex"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Concepto Funcional"
        Me.PanelFiltros.ResumeLayout(False)
        Me.PanelFiltros.PerformLayout()
        Me.PanelPrincipal.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.dgvLicenciasMedicas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.dgvSanciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        CType(Me.dgvObservaciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelFooter.ResumeLayout(False)
        Me.PanelFooter.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
End Class