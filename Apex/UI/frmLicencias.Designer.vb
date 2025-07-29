<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLicencias
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
        Me.panelFiltros = New System.Windows.Forms.Panel()
        Me.chkHasta = New System.Windows.Forms.CheckBox()
        Me.chkDesde = New System.Windows.Forms.CheckBox()
        Me.dtpHasta = New System.Windows.Forms.DateTimePicker()
        Me.dtpDesde = New System.Windows.Forms.DateTimePicker()
        Me.cboTipoLicencia = New System.Windows.Forms.ComboBox()
        Me.lblTipo = New System.Windows.Forms.Label()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.txtBusqueda = New System.Windows.Forms.TextBox()
        Me.lblFuncionario = New System.Windows.Forms.Label()
        Me.dgvLicencias = New System.Windows.Forms.DataGridView()
        Me.panelAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnNueva = New System.Windows.Forms.Button()
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnSiguiente = New System.Windows.Forms.Button()
        Me.lblPaginacion = New System.Windows.Forms.Label()
        Me.btnAnterior = New System.Windows.Forms.Button()
        Me.panelFiltros.SuspendLayout()
        CType(Me.dgvLicencias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.panelAcciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelFiltros
        '
        Me.panelFiltros.BackColor = System.Drawing.Color.WhiteSmoke
        Me.panelFiltros.Controls.Add(Me.chkHasta)
        Me.panelFiltros.Controls.Add(Me.chkDesde)
        Me.panelFiltros.Controls.Add(Me.dtpHasta)
        Me.panelFiltros.Controls.Add(Me.dtpDesde)
        Me.panelFiltros.Controls.Add(Me.cboTipoLicencia)
        Me.panelFiltros.Controls.Add(Me.lblTipo)
        Me.panelFiltros.Controls.Add(Me.btnBuscar)
        Me.panelFiltros.Controls.Add(Me.txtBusqueda)
        Me.panelFiltros.Controls.Add(Me.lblFuncionario)
        Me.panelFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelFiltros.Location = New System.Drawing.Point(0, 0)
        Me.panelFiltros.Name = "panelFiltros"
        Me.panelFiltros.Size = New System.Drawing.Size(984, 100)
        Me.panelFiltros.TabIndex = 2
        '
        'chkHasta
        '
        Me.chkHasta.AutoSize = True
        Me.chkHasta.Location = New System.Drawing.Point(340, 59)
        Me.chkHasta.Name = "chkHasta"
        Me.chkHasta.Size = New System.Drawing.Size(72, 24)
        Me.chkHasta.TabIndex = 8
        Me.chkHasta.Text = "Hasta:"
        Me.chkHasta.UseVisualStyleBackColor = True
        '
        'chkDesde
        '
        Me.chkDesde.AutoSize = True
        Me.chkDesde.Location = New System.Drawing.Point(340, 22)
        Me.chkDesde.Name = "chkDesde"
        Me.chkDesde.Size = New System.Drawing.Size(76, 24)
        Me.chkDesde.TabIndex = 7
        Me.chkDesde.Text = "Desde:"
        Me.chkDesde.UseVisualStyleBackColor = True
        '
        'dtpHasta
        '
        Me.dtpHasta.Enabled = False
        Me.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpHasta.Location = New System.Drawing.Point(420, 57)
        Me.dtpHasta.Name = "dtpHasta"
        Me.dtpHasta.Size = New System.Drawing.Size(120, 27)
        Me.dtpHasta.TabIndex = 6
        '
        'dtpDesde
        '
        Me.dtpDesde.Enabled = False
        Me.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDesde.Location = New System.Drawing.Point(420, 20)
        Me.dtpDesde.Name = "dtpDesde"
        Me.dtpDesde.Size = New System.Drawing.Size(120, 27)
        Me.dtpDesde.TabIndex = 5
        '
        'cboTipoLicencia
        '
        Me.cboTipoLicencia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoLicencia.FormattingEnabled = True
        Me.cboTipoLicencia.Location = New System.Drawing.Point(107, 57)
        Me.cboTipoLicencia.Name = "cboTipoLicencia"
        Me.cboTipoLicencia.Size = New System.Drawing.Size(225, 28)
        Me.cboTipoLicencia.TabIndex = 4
        '
        'lblTipo
        '
        Me.lblTipo.AutoSize = True
        Me.lblTipo.Location = New System.Drawing.Point(12, 60)
        Me.lblTipo.Name = "lblTipo"
        Me.lblTipo.Size = New System.Drawing.Size(42, 20)
        Me.lblTipo.TabIndex = 3
        Me.lblTipo.Text = "Tipo:"
        '
        'btnBuscar
        '
        Me.btnBuscar.Location = New System.Drawing.Point(780, 58)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(85, 25)
        Me.btnBuscar.TabIndex = 2
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'txtBusqueda
        '
        Me.txtBusqueda.Location = New System.Drawing.Point(107, 20)
        Me.txtBusqueda.Name = "txtBusqueda"
        Me.txtBusqueda.Size = New System.Drawing.Size(225, 27)
        Me.txtBusqueda.TabIndex = 1
        '
        'lblFuncionario
        '
        Me.lblFuncionario.AutoSize = True
        Me.lblFuncionario.Location = New System.Drawing.Point(12, 23)
        Me.lblFuncionario.Name = "lblFuncionario"
        Me.lblFuncionario.Size = New System.Drawing.Size(89, 20)
        Me.lblFuncionario.TabIndex = 0
        Me.lblFuncionario.Text = "Funcionario:"
        '
        'dgvLicencias
        '
        Me.dgvLicencias.AllowUserToAddRows = False
        Me.dgvLicencias.AllowUserToDeleteRows = False
        Me.dgvLicencias.AllowUserToResizeRows = False
        Me.dgvLicencias.BackgroundColor = System.Drawing.Color.White
        Me.dgvLicencias.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvLicencias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLicencias.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvLicencias.Location = New System.Drawing.Point(0, 100)
        Me.dgvLicencias.MultiSelect = False
        Me.dgvLicencias.Name = "dgvLicencias"
        Me.dgvLicencias.ReadOnly = True
        Me.dgvLicencias.RowHeadersWidth = 51
        Me.dgvLicencias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLicencias.Size = New System.Drawing.Size(984, 420)
        Me.dgvLicencias.TabIndex = 3
        '
        'panelAcciones
        '
        Me.panelAcciones.Controls.Add(Me.btnNueva)
        Me.panelAcciones.Controls.Add(Me.btnEditar)
        Me.panelAcciones.Controls.Add(Me.btnEliminar)
        Me.panelAcciones.Controls.Add(Me.btnSiguiente)
        Me.panelAcciones.Controls.Add(Me.lblPaginacion)
        Me.panelAcciones.Controls.Add(Me.btnAnterior)
        Me.panelAcciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.panelAcciones.Location = New System.Drawing.Point(0, 520)
        Me.panelAcciones.Name = "panelAcciones"
        Me.panelAcciones.Padding = New System.Windows.Forms.Padding(10)
        Me.panelAcciones.Size = New System.Drawing.Size(984, 42)
        Me.panelAcciones.TabIndex = 4
        '
        'btnNueva
        '
        Me.btnNueva.Location = New System.Drawing.Point(886, 13)
        Me.btnNueva.Name = "btnNueva"
        Me.btnNueva.Size = New System.Drawing.Size(75, 23)
        Me.btnNueva.TabIndex = 0
        Me.btnNueva.Text = "Nueva..."
        Me.btnNueva.UseVisualStyleBackColor = True
        '
        'btnEditar
        '
        Me.btnEditar.Location = New System.Drawing.Point(805, 13)
        Me.btnEditar.Name = "btnEditar"
        Me.btnEditar.Size = New System.Drawing.Size(75, 23)
        Me.btnEditar.TabIndex = 1
        Me.btnEditar.Text = "Editar..."
        Me.btnEditar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Location = New System.Drawing.Point(724, 13)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(75, 23)
        Me.btnEliminar.TabIndex = 2
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'btnSiguiente
        '
        Me.btnSiguiente.Enabled = False
        Me.btnSiguiente.Location = New System.Drawing.Point(630, 13)
        Me.btnSiguiente.Name = "btnSiguiente"
        Me.btnSiguiente.Size = New System.Drawing.Size(88, 23)
        Me.btnSiguiente.TabIndex = 3
        Me.btnSiguiente.Text = "Siguiente >"
        Me.btnSiguiente.UseVisualStyleBackColor = True
        '
        'lblPaginacion
        '
        Me.lblPaginacion.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.lblPaginacion.AutoSize = True
        Me.lblPaginacion.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPaginacion.Location = New System.Drawing.Point(521, 14)
        Me.lblPaginacion.Name = "lblPaginacion"
        Me.lblPaginacion.Size = New System.Drawing.Size(103, 20)
        Me.lblPaginacion.TabIndex = 5
        Me.lblPaginacion.Text = "Página 1 de 1"
        Me.lblPaginacion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnAnterior
        '
        Me.btnAnterior.Enabled = False
        Me.btnAnterior.Location = New System.Drawing.Point(427, 13)
        Me.btnAnterior.Name = "btnAnterior"
        Me.btnAnterior.Size = New System.Drawing.Size(88, 23)
        Me.btnAnterior.TabIndex = 4
        Me.btnAnterior.Text = "< Anterior"
        Me.btnAnterior.UseVisualStyleBackColor = True
        '
        'frmLicencias
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 562)
        Me.Controls.Add(Me.dgvLicencias)
        Me.Controls.Add(Me.panelAcciones)
        Me.Controls.Add(Me.panelFiltros)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmLicencias"
        Me.Text = "Gestión de Licencias"
        Me.panelFiltros.ResumeLayout(False)
        Me.panelFiltros.PerformLayout()
        CType(Me.dgvLicencias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.panelAcciones.ResumeLayout(False)
        Me.panelAcciones.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents panelFiltros As Panel
    Friend WithEvents btnBuscar As Button
    Friend WithEvents txtBusqueda As TextBox
    Friend WithEvents lblFuncionario As Label
    Friend WithEvents dgvLicencias As DataGridView
    Friend WithEvents panelAcciones As FlowLayoutPanel
    Friend WithEvents btnNueva As Button
    Friend WithEvents btnEditar As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents cboTipoLicencia As ComboBox
    Friend WithEvents lblTipo As Label
    Friend WithEvents dtpHasta As DateTimePicker
    Friend WithEvents dtpDesde As DateTimePicker
    Friend WithEvents chkHasta As CheckBox
    Friend WithEvents chkDesde As CheckBox
    ' --- Añade estas líneas ---
    Friend WithEvents btnSiguiente As Button
    Friend WithEvents lblPaginacion As Label
    Friend WithEvents btnAnterior As Button
End Class