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
        Me.PanelFiltros.Name = "PanelFiltros"
        Me.PanelFiltros.Size = New System.Drawing.Size(800, 80)
        Me.PanelFiltros.TabIndex = 0
        '
        'btnBuscarFuncionario
        '
        Me.btnBuscarFuncionario.Location = New System.Drawing.Point(330, 29)
        Me.btnBuscarFuncionario.Name = "btnBuscarFuncionario"
        Me.btnBuscarFuncionario.Size = New System.Drawing.Size(28, 23)
        Me.btnBuscarFuncionario.TabIndex = 6
        Me.btnBuscarFuncionario.Text = "..."
        Me.btnBuscarFuncionario.UseVisualStyleBackColor = True
        '
        'txtFuncionarioSeleccionado
        '
        Me.txtFuncionarioSeleccionado.Location = New System.Drawing.Point(15, 31)
        Me.txtFuncionarioSeleccionado.Name = "txtFuncionarioSeleccionado"
        Me.txtFuncionarioSeleccionado.ReadOnly = True
        Me.txtFuncionarioSeleccionado.Size = New System.Drawing.Size(309, 20)
        Me.txtFuncionarioSeleccionado.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 15)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Funcionario"
        '
        'dtpFechaFin
        '
        Me.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFechaFin.Location = New System.Drawing.Point(520, 31)
        Me.dtpFechaFin.Name = "dtpFechaFin"
        Me.dtpFechaFin.Size = New System.Drawing.Size(99, 20)
        Me.dtpFechaFin.TabIndex = 3
        '
        'dtpFechaInicio
        '
        Me.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFechaInicio.Location = New System.Drawing.Point(398, 31)
        Me.dtpFechaInicio.Name = "dtpFechaInicio"
        Me.dtpFechaInicio.Size = New System.Drawing.Size(99, 20)
        Me.dtpFechaInicio.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(517, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Hasta"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(395, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Desde"
        '
        'PanelPrincipal
        '
        Me.PanelPrincipal.Controls.Add(Me.TabControl1)
        Me.PanelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelPrincipal.Location = New System.Drawing.Point(0, 80)
        Me.PanelPrincipal.Name = "PanelPrincipal"
        Me.PanelPrincipal.Size = New System.Drawing.Size(800, 320)
        Me.PanelPrincipal.TabIndex = 1
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(800, 320)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.dgvLicenciasMedicas)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(792, 294)
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
        Me.dgvLicenciasMedicas.Location = New System.Drawing.Point(3, 3)
        Me.dgvLicenciasMedicas.Name = "dgvLicenciasMedicas"
        Me.dgvLicenciasMedicas.ReadOnly = True
        Me.dgvLicenciasMedicas.Size = New System.Drawing.Size(786, 288)
        Me.dgvLicenciasMedicas.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.dgvSanciones)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(792, 294)
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
        Me.dgvSanciones.Location = New System.Drawing.Point(3, 3)
        Me.dgvSanciones.Name = "dgvSanciones"
        Me.dgvSanciones.ReadOnly = True
        Me.dgvSanciones.Size = New System.Drawing.Size(786, 288)
        Me.dgvSanciones.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.dgvObservaciones)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(792, 294)
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
        Me.dgvObservaciones.Name = "dgvObservaciones"
        Me.dgvObservaciones.ReadOnly = True
        Me.dgvObservaciones.Size = New System.Drawing.Size(792, 294)
        Me.dgvObservaciones.TabIndex = 0
        '
        'PanelFooter
        '
        Me.PanelFooter.Controls.Add(Me.btnInforme)
        Me.PanelFooter.Controls.Add(Me.lblTemporal)
        Me.PanelFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelFooter.Location = New System.Drawing.Point(0, 400)
        Me.PanelFooter.Name = "PanelFooter"
        Me.PanelFooter.Size = New System.Drawing.Size(800, 50)
        Me.PanelFooter.TabIndex = 2
        '
        'btnInforme
        '
        Me.btnInforme.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInforme.Location = New System.Drawing.Point(684, 15)
        Me.btnInforme.Name = "btnInforme"
        Me.btnInforme.Size = New System.Drawing.Size(104, 23)
        Me.btnInforme.TabIndex = 1
        Me.btnInforme.Text = "Generar Informe"
        Me.btnInforme.UseVisualStyleBackColor = True
        '
        'lblTemporal
        '
        Me.lblTemporal.AutoSize = True
        Me.lblTemporal.Location = New System.Drawing.Point(12, 20)
        Me.lblTemporal.Name = "lblTemporal"
        Me.lblTemporal.Size = New System.Drawing.Size(95, 13)
        Me.lblTemporal.TabIndex = 0
        Me.lblTemporal.Text = "Período evaluado:"
        '
        'frmConceptoFuncionalApex
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.PanelPrincipal)
        Me.Controls.Add(Me.PanelFooter)
        Me.Controls.Add(Me.PanelFiltros)
        Me.Name = "frmConceptoFuncionalApex"
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