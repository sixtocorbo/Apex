<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmFuncionarioCrear
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.TabPageEstadosTransitorios = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelEstados = New System.Windows.Forms.TableLayoutPanel()
        Me.dgvEstadosTransitorios = New System.Windows.Forms.DataGridView()
        Me.chkVerHistorial = New System.Windows.Forms.CheckBox()
        Me.FlowLayoutPanelEstados = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnQuitarEstado = New System.Windows.Forms.Button()
        Me.btnEditarEstado = New System.Windows.Forms.Button()
        Me.btnAñadirEstado = New System.Windows.Forms.Button()
        Me.TabPageDotacion = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelDotacion = New System.Windows.Forms.TableLayoutPanel()
        Me.dgvDotacion = New System.Windows.Forms.DataGridView()
        Me.FlowLayoutPanelDotacion = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnQuitarDotacion = New System.Windows.Forms.Button()
        Me.btnEditarDotacion = New System.Windows.Forms.Button()
        Me.btnAñadirDotacion = New System.Windows.Forms.Button()
        Me.TabPageDatosPersonales = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelDatosPersonales = New System.Windows.Forms.TableLayoutPanel()
        Me.lblFechaNacimiento = New System.Windows.Forms.Label()
        Me.dtpFechaNacimiento = New System.Windows.Forms.DateTimePicker()
        Me.lblDomicilio = New System.Windows.Forms.Label()
        Me.txtDomicilio = New System.Windows.Forms.TextBox()
        Me.lblTelefono = New System.Windows.Forms.Label()
        Me.txtTelefono = New System.Windows.Forms.TextBox()
        Me.lblEmail = New System.Windows.Forms.Label()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.lblEstadoCivil = New System.Windows.Forms.Label()
        Me.cboEstadoCivil = New System.Windows.Forms.ComboBox()
        Me.lblGenero = New System.Windows.Forms.Label()
        Me.cboGenero = New System.Windows.Forms.ComboBox()
        Me.lblNivelEstudio = New System.Windows.Forms.Label()
        Me.cboNivelEstudio = New System.Windows.Forms.ComboBox()
        Me.TabPageGeneral = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanelGeneral = New System.Windows.Forms.TableLayoutPanel()
        Me.pbFoto = New System.Windows.Forms.PictureBox()
        Me.btnSeleccionarFoto = New System.Windows.Forms.Button()
        Me.TableLayoutPanelDatosGenerales = New System.Windows.Forms.TableLayoutPanel()
        Me.lblCI = New System.Windows.Forms.Label()
        Me.txtCI = New System.Windows.Forms.TextBox()
        Me.lblNombre = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.lblFechaIngreso = New System.Windows.Forms.Label()
        Me.dtpFechaIngreso = New System.Windows.Forms.DateTimePicker()
        Me.lblTipoFuncionario = New System.Windows.Forms.Label()
        Me.cboTipoFuncionario = New System.Windows.Forms.ComboBox()
        Me.lblCargo = New System.Windows.Forms.Label()
        Me.cboCargo = New System.Windows.Forms.ComboBox()
        Me.lblEscalafon = New System.Windows.Forms.Label()
        Me.cboEscalafon = New System.Windows.Forms.ComboBox()
        Me.lblFuncion = New System.Windows.Forms.Label()
        Me.cboFuncion = New System.Windows.Forms.ComboBox()
        Me.chkActivo = New System.Windows.Forms.CheckBox()
        Me.TabControlMain = New System.Windows.Forms.TabControl()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.TabPageEstadosTransitorios.SuspendLayout()
        Me.TableLayoutPanelEstados.SuspendLayout()
        CType(Me.dgvEstadosTransitorios, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanelEstados.SuspendLayout()
        Me.TabPageDotacion.SuspendLayout()
        Me.TableLayoutPanelDotacion.SuspendLayout()
        CType(Me.dgvDotacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanelDotacion.SuspendLayout()
        Me.TabPageDatosPersonales.SuspendLayout()
        Me.TableLayoutPanelDatosPersonales.SuspendLayout()
        Me.TabPageGeneral.SuspendLayout()
        Me.TableLayoutPanelGeneral.SuspendLayout()
        CType(Me.pbFoto, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanelDatosGenerales.SuspendLayout()
        Me.TabControlMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.Controls.Add(Me.btnGuardar)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCancelar)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 415)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Padding = New System.Windows.Forms.Padding(0, 5, 15, 5)
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(784, 46)
        Me.FlowLayoutPanel1.TabIndex = 1
        '
        'btnGuardar
        '
        Me.btnGuardar.AutoSize = True
        Me.btnGuardar.Location = New System.Drawing.Point(678, 8)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Padding = New System.Windows.Forms.Padding(10, 2, 10, 2)
        Me.btnGuardar.Size = New System.Drawing.Size(88, 30)
        Me.btnGuardar.TabIndex = 16
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.AutoSize = True
        Me.btnCancelar.Location = New System.Drawing.Point(581, 8)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Padding = New System.Windows.Forms.Padding(10, 2, 10, 2)
        Me.btnCancelar.Size = New System.Drawing.Size(91, 30)
        Me.btnCancelar.TabIndex = 17
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'TabPageEstadosTransitorios
        '
        Me.TabPageEstadosTransitorios.Controls.Add(Me.TableLayoutPanelEstados)
        Me.TabPageEstadosTransitorios.Location = New System.Drawing.Point(4, 22)
        Me.TabPageEstadosTransitorios.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TabPageEstadosTransitorios.Name = "TabPageEstadosTransitorios"
        Me.TabPageEstadosTransitorios.Padding = New System.Windows.Forms.Padding(8, 8, 8, 8)
        Me.TabPageEstadosTransitorios.Size = New System.Drawing.Size(776, 389)
        Me.TabPageEstadosTransitorios.TabIndex = 4
        Me.TabPageEstadosTransitorios.Text = "Estados Transitorios"
        Me.TabPageEstadosTransitorios.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelEstados
        '
        Me.TableLayoutPanelEstados.ColumnCount = 1
        Me.TableLayoutPanelEstados.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelEstados.Controls.Add(Me.dgvEstadosTransitorios, 0, 0)
        Me.TableLayoutPanelEstados.Controls.Add(Me.chkVerHistorial, 0, 1)
        Me.TableLayoutPanelEstados.Controls.Add(Me.FlowLayoutPanelEstados, 0, 2)
        Me.TableLayoutPanelEstados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelEstados.Location = New System.Drawing.Point(8, 8)
        Me.TableLayoutPanelEstados.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.TableLayoutPanelEstados.Name = "TableLayoutPanelEstados"
        Me.TableLayoutPanelEstados.RowCount = 3
        Me.TableLayoutPanelEstados.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelEstados.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelEstados.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelEstados.Size = New System.Drawing.Size(760, 373)
        Me.TableLayoutPanelEstados.TabIndex = 0
        '
        'dgvEstadosTransitorios
        '
        Me.dgvEstadosTransitorios.AllowUserToAddRows = False
        Me.dgvEstadosTransitorios.AllowUserToDeleteRows = False
        Me.dgvEstadosTransitorios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvEstadosTransitorios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvEstadosTransitorios.Location = New System.Drawing.Point(2, 2)
        Me.dgvEstadosTransitorios.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.dgvEstadosTransitorios.Name = "dgvEstadosTransitorios"
        Me.dgvEstadosTransitorios.ReadOnly = True
        Me.dgvEstadosTransitorios.RowHeadersWidth = 51
        Me.dgvEstadosTransitorios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvEstadosTransitorios.Size = New System.Drawing.Size(756, 314)
        Me.dgvEstadosTransitorios.TabIndex = 0
        '
        'chkVerHistorial
        '
        Me.chkVerHistorial.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.chkVerHistorial.AutoSize = True
        Me.chkVerHistorial.Location = New System.Drawing.Point(2, 320)
        Me.chkVerHistorial.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.chkVerHistorial.Name = "chkVerHistorial"
        Me.chkVerHistorial.Size = New System.Drawing.Size(126, 17)
        Me.chkVerHistorial.TabIndex = 2
        Me.chkVerHistorial.Text = "Ver historial completo"
        Me.chkVerHistorial.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanelEstados
        '
        Me.FlowLayoutPanelEstados.AutoSize = True
        Me.FlowLayoutPanelEstados.Controls.Add(Me.btnQuitarEstado)
        Me.FlowLayoutPanelEstados.Controls.Add(Me.btnEditarEstado)
        Me.FlowLayoutPanelEstados.Controls.Add(Me.btnAñadirEstado)
        Me.FlowLayoutPanelEstados.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanelEstados.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanelEstados.Location = New System.Drawing.Point(2, 341)
        Me.FlowLayoutPanelEstados.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.FlowLayoutPanelEstados.Name = "FlowLayoutPanelEstados"
        Me.FlowLayoutPanelEstados.Size = New System.Drawing.Size(756, 30)
        Me.FlowLayoutPanelEstados.TabIndex = 1
        '
        'btnQuitarEstado
        '
        Me.btnQuitarEstado.Location = New System.Drawing.Point(679, 2)
        Me.btnQuitarEstado.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnQuitarEstado.Name = "btnQuitarEstado"
        Me.btnQuitarEstado.Size = New System.Drawing.Size(75, 26)
        Me.btnQuitarEstado.TabIndex = 0
        Me.btnQuitarEstado.Text = "Quitar"
        Me.btnQuitarEstado.UseVisualStyleBackColor = True
        '
        'btnEditarEstado
        '
        Me.btnEditarEstado.Location = New System.Drawing.Point(600, 2)
        Me.btnEditarEstado.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnEditarEstado.Name = "btnEditarEstado"
        Me.btnEditarEstado.Size = New System.Drawing.Size(75, 26)
        Me.btnEditarEstado.TabIndex = 1
        Me.btnEditarEstado.Text = "Editar..."
        Me.btnEditarEstado.UseVisualStyleBackColor = True
        '
        'btnAñadirEstado
        '
        Me.btnAñadirEstado.Location = New System.Drawing.Point(521, 2)
        Me.btnAñadirEstado.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.btnAñadirEstado.Name = "btnAñadirEstado"
        Me.btnAñadirEstado.Size = New System.Drawing.Size(75, 26)
        Me.btnAñadirEstado.TabIndex = 2
        Me.btnAñadirEstado.Text = "Añadir..."
        Me.btnAñadirEstado.UseVisualStyleBackColor = True
        '
        'TabPageDotacion
        '
        Me.TabPageDotacion.Controls.Add(Me.TableLayoutPanelDotacion)
        Me.TabPageDotacion.Location = New System.Drawing.Point(4, 22)
        Me.TabPageDotacion.Name = "TabPageDotacion"
        Me.TabPageDotacion.Padding = New System.Windows.Forms.Padding(10, 10, 10, 10)
        Me.TabPageDotacion.Size = New System.Drawing.Size(776, 388)
        Me.TabPageDotacion.TabIndex = 2
        Me.TabPageDotacion.Text = "Dotación"
        Me.TabPageDotacion.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelDotacion
        '
        Me.TableLayoutPanelDotacion.ColumnCount = 1
        Me.TableLayoutPanelDotacion.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelDotacion.Controls.Add(Me.dgvDotacion, 0, 0)
        Me.TableLayoutPanelDotacion.Controls.Add(Me.FlowLayoutPanelDotacion, 0, 1)
        Me.TableLayoutPanelDotacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelDotacion.Location = New System.Drawing.Point(10, 10)
        Me.TableLayoutPanelDotacion.Name = "TableLayoutPanelDotacion"
        Me.TableLayoutPanelDotacion.RowCount = 2
        Me.TableLayoutPanelDotacion.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelDotacion.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelDotacion.Size = New System.Drawing.Size(756, 368)
        Me.TableLayoutPanelDotacion.TabIndex = 0
        '
        'dgvDotacion
        '
        Me.dgvDotacion.AllowUserToAddRows = False
        Me.dgvDotacion.AllowUserToDeleteRows = False
        Me.dgvDotacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDotacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDotacion.Location = New System.Drawing.Point(3, 3)
        Me.dgvDotacion.Name = "dgvDotacion"
        Me.dgvDotacion.ReadOnly = True
        Me.dgvDotacion.RowHeadersWidth = 51
        Me.dgvDotacion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDotacion.Size = New System.Drawing.Size(750, 327)
        Me.dgvDotacion.TabIndex = 0
        '
        'FlowLayoutPanelDotacion
        '
        Me.FlowLayoutPanelDotacion.AutoSize = True
        Me.FlowLayoutPanelDotacion.Controls.Add(Me.btnQuitarDotacion)
        Me.FlowLayoutPanelDotacion.Controls.Add(Me.btnEditarDotacion)
        Me.FlowLayoutPanelDotacion.Controls.Add(Me.btnAñadirDotacion)
        Me.FlowLayoutPanelDotacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanelDotacion.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanelDotacion.Location = New System.Drawing.Point(3, 336)
        Me.FlowLayoutPanelDotacion.Name = "FlowLayoutPanelDotacion"
        Me.FlowLayoutPanelDotacion.Size = New System.Drawing.Size(750, 29)
        Me.FlowLayoutPanelDotacion.TabIndex = 1
        '
        'btnQuitarDotacion
        '
        Me.btnQuitarDotacion.Location = New System.Drawing.Point(672, 3)
        Me.btnQuitarDotacion.Name = "btnQuitarDotacion"
        Me.btnQuitarDotacion.Size = New System.Drawing.Size(75, 23)
        Me.btnQuitarDotacion.TabIndex = 0
        Me.btnQuitarDotacion.Text = "Quitar"
        Me.btnQuitarDotacion.UseVisualStyleBackColor = True
        '
        'btnEditarDotacion
        '
        Me.btnEditarDotacion.Location = New System.Drawing.Point(591, 3)
        Me.btnEditarDotacion.Name = "btnEditarDotacion"
        Me.btnEditarDotacion.Size = New System.Drawing.Size(75, 23)
        Me.btnEditarDotacion.TabIndex = 1
        Me.btnEditarDotacion.Text = "Editar..."
        Me.btnEditarDotacion.UseVisualStyleBackColor = True
        '
        'btnAñadirDotacion
        '
        Me.btnAñadirDotacion.Location = New System.Drawing.Point(510, 3)
        Me.btnAñadirDotacion.Name = "btnAñadirDotacion"
        Me.btnAñadirDotacion.Size = New System.Drawing.Size(75, 23)
        Me.btnAñadirDotacion.TabIndex = 2
        Me.btnAñadirDotacion.Text = "Añadir..."
        Me.btnAñadirDotacion.UseVisualStyleBackColor = True
        '
        'TabPageDatosPersonales
        '
        Me.TabPageDatosPersonales.Controls.Add(Me.TableLayoutPanelDatosPersonales)
        Me.TabPageDatosPersonales.Location = New System.Drawing.Point(4, 22)
        Me.TabPageDatosPersonales.Name = "TabPageDatosPersonales"
        Me.TabPageDatosPersonales.Padding = New System.Windows.Forms.Padding(10, 10, 10, 10)
        Me.TabPageDatosPersonales.Size = New System.Drawing.Size(776, 388)
        Me.TabPageDatosPersonales.TabIndex = 1
        Me.TabPageDatosPersonales.Text = "Datos Personales"
        Me.TabPageDatosPersonales.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelDatosPersonales
        '
        Me.TableLayoutPanelDatosPersonales.ColumnCount = 2
        Me.TableLayoutPanelDatosPersonales.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
        Me.TableLayoutPanelDatosPersonales.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.lblFechaNacimiento, 0, 0)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.dtpFechaNacimiento, 1, 0)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.lblDomicilio, 0, 1)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.txtDomicilio, 1, 1)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.lblTelefono, 0, 2)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.txtTelefono, 1, 2)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.lblEmail, 0, 3)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.txtEmail, 1, 3)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.lblEstadoCivil, 0, 4)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.cboEstadoCivil, 1, 4)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.lblGenero, 0, 5)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.cboGenero, 1, 5)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.lblNivelEstudio, 0, 6)
        Me.TableLayoutPanelDatosPersonales.Controls.Add(Me.cboNivelEstudio, 1, 6)
        Me.TableLayoutPanelDatosPersonales.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelDatosPersonales.Location = New System.Drawing.Point(10, 10)
        Me.TableLayoutPanelDatosPersonales.Name = "TableLayoutPanelDatosPersonales"
        Me.TableLayoutPanelDatosPersonales.RowCount = 8
        Me.TableLayoutPanelDatosPersonales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelDatosPersonales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanelDatosPersonales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelDatosPersonales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelDatosPersonales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelDatosPersonales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelDatosPersonales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TableLayoutPanelDatosPersonales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelDatosPersonales.Size = New System.Drawing.Size(756, 368)
        Me.TableLayoutPanelDatosPersonales.TabIndex = 0
        '
        'lblFechaNacimiento
        '
        Me.lblFechaNacimiento.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFechaNacimiento.AutoSize = True
        Me.lblFechaNacimiento.Location = New System.Drawing.Point(21, 8)
        Me.lblFechaNacimiento.Name = "lblFechaNacimiento"
        Me.lblFechaNacimiento.Size = New System.Drawing.Size(96, 13)
        Me.lblFechaNacimiento.TabIndex = 0
        Me.lblFechaNacimiento.Text = "Fecha Nacimiento:"
        '
        'dtpFechaNacimiento
        '
        Me.dtpFechaNacimiento.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaNacimiento.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaNacimiento.Location = New System.Drawing.Point(123, 5)
        Me.dtpFechaNacimiento.Name = "dtpFechaNacimiento"
        Me.dtpFechaNacimiento.Size = New System.Drawing.Size(150, 20)
        Me.dtpFechaNacimiento.TabIndex = 1
        '
        'lblDomicilio
        '
        Me.lblDomicilio.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblDomicilio.AutoSize = True
        Me.lblDomicilio.Location = New System.Drawing.Point(65, 53)
        Me.lblDomicilio.Name = "lblDomicilio"
        Me.lblDomicilio.Size = New System.Drawing.Size(52, 13)
        Me.lblDomicilio.TabIndex = 2
        Me.lblDomicilio.Text = "Domicilio:"
        '
        'txtDomicilio
        '
        Me.txtDomicilio.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDomicilio.Location = New System.Drawing.Point(123, 33)
        Me.txtDomicilio.Multiline = True
        Me.txtDomicilio.Name = "txtDomicilio"
        Me.txtDomicilio.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDomicilio.Size = New System.Drawing.Size(630, 54)
        Me.txtDomicilio.TabIndex = 3
        '
        'lblTelefono
        '
        Me.lblTelefono.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblTelefono.AutoSize = True
        Me.lblTelefono.Location = New System.Drawing.Point(65, 98)
        Me.lblTelefono.Name = "lblTelefono"
        Me.lblTelefono.Size = New System.Drawing.Size(52, 13)
        Me.lblTelefono.TabIndex = 4
        Me.lblTelefono.Text = "Teléfono:"
        '
        'txtTelefono
        '
        Me.txtTelefono.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.txtTelefono.Location = New System.Drawing.Point(123, 95)
        Me.txtTelefono.Name = "txtTelefono"
        Me.txtTelefono.Size = New System.Drawing.Size(250, 20)
        Me.txtTelefono.TabIndex = 5
        '
        'lblEmail
        '
        Me.lblEmail.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblEmail.AutoSize = True
        Me.lblEmail.Location = New System.Drawing.Point(82, 128)
        Me.lblEmail.Name = "lblEmail"
        Me.lblEmail.Size = New System.Drawing.Size(35, 13)
        Me.lblEmail.TabIndex = 6
        Me.lblEmail.Text = "Email:"
        '
        'txtEmail
        '
        Me.txtEmail.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmail.Location = New System.Drawing.Point(123, 125)
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(630, 20)
        Me.txtEmail.TabIndex = 7
        '
        'lblEstadoCivil
        '
        Me.lblEstadoCivil.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblEstadoCivil.AutoSize = True
        Me.lblEstadoCivil.Location = New System.Drawing.Point(52, 158)
        Me.lblEstadoCivil.Name = "lblEstadoCivil"
        Me.lblEstadoCivil.Size = New System.Drawing.Size(65, 13)
        Me.lblEstadoCivil.TabIndex = 8
        Me.lblEstadoCivil.Text = "Estado Civil:"
        '
        'cboEstadoCivil
        '
        Me.cboEstadoCivil.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cboEstadoCivil.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEstadoCivil.FormattingEnabled = True
        Me.cboEstadoCivil.Location = New System.Drawing.Point(123, 154)
        Me.cboEstadoCivil.Name = "cboEstadoCivil"
        Me.cboEstadoCivil.Size = New System.Drawing.Size(250, 21)
        Me.cboEstadoCivil.TabIndex = 9
        '
        'lblGenero
        '
        Me.lblGenero.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblGenero.AutoSize = True
        Me.lblGenero.Location = New System.Drawing.Point(72, 188)
        Me.lblGenero.Name = "lblGenero"
        Me.lblGenero.Size = New System.Drawing.Size(45, 13)
        Me.lblGenero.TabIndex = 10
        Me.lblGenero.Text = "Género:"
        '
        'cboGenero
        '
        Me.cboGenero.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cboGenero.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboGenero.FormattingEnabled = True
        Me.cboGenero.Location = New System.Drawing.Point(123, 184)
        Me.cboGenero.Name = "cboGenero"
        Me.cboGenero.Size = New System.Drawing.Size(250, 21)
        Me.cboGenero.TabIndex = 11
        '
        'lblNivelEstudio
        '
        Me.lblNivelEstudio.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblNivelEstudio.AutoSize = True
        Me.lblNivelEstudio.Location = New System.Drawing.Point(30, 218)
        Me.lblNivelEstudio.Name = "lblNivelEstudio"
        Me.lblNivelEstudio.Size = New System.Drawing.Size(87, 13)
        Me.lblNivelEstudio.TabIndex = 12
        Me.lblNivelEstudio.Text = "Nivel de Estudio:"
        '
        'cboNivelEstudio
        '
        Me.cboNivelEstudio.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cboNivelEstudio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboNivelEstudio.FormattingEnabled = True
        Me.cboNivelEstudio.Location = New System.Drawing.Point(123, 214)
        Me.cboNivelEstudio.Name = "cboNivelEstudio"
        Me.cboNivelEstudio.Size = New System.Drawing.Size(250, 21)
        Me.cboNivelEstudio.TabIndex = 13
        '
        'TabPageGeneral
        '
        Me.TabPageGeneral.Controls.Add(Me.TableLayoutPanelGeneral)
        Me.TabPageGeneral.Location = New System.Drawing.Point(4, 22)
        Me.TabPageGeneral.Name = "TabPageGeneral"
        Me.TabPageGeneral.Padding = New System.Windows.Forms.Padding(10, 10, 10, 10)
        Me.TabPageGeneral.Size = New System.Drawing.Size(776, 389)
        Me.TabPageGeneral.TabIndex = 0
        Me.TabPageGeneral.Text = "General"
        Me.TabPageGeneral.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelGeneral
        '
        Me.TableLayoutPanelGeneral.ColumnCount = 2
        Me.TableLayoutPanelGeneral.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200.0!))
        Me.TableLayoutPanelGeneral.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelGeneral.Controls.Add(Me.pbFoto, 0, 0)
        Me.TableLayoutPanelGeneral.Controls.Add(Me.btnSeleccionarFoto, 0, 1)
        Me.TableLayoutPanelGeneral.Controls.Add(Me.TableLayoutPanelDatosGenerales, 1, 0)
        Me.TableLayoutPanelGeneral.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelGeneral.Location = New System.Drawing.Point(10, 10)
        Me.TableLayoutPanelGeneral.Name = "TableLayoutPanelGeneral"
        Me.TableLayoutPanelGeneral.RowCount = 2
        Me.TableLayoutPanelGeneral.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelGeneral.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanelGeneral.Size = New System.Drawing.Size(756, 369)
        Me.TableLayoutPanelGeneral.TabIndex = 0
        '
        'pbFoto
        '
        Me.pbFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pbFoto.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pbFoto.Location = New System.Drawing.Point(3, 3)
        Me.pbFoto.Name = "pbFoto"
        Me.pbFoto.Size = New System.Drawing.Size(194, 334)
        Me.pbFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pbFoto.TabIndex = 0
        Me.pbFoto.TabStop = False
        '
        'btnSeleccionarFoto
        '
        Me.btnSeleccionarFoto.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.btnSeleccionarFoto.Location = New System.Drawing.Point(59, 343)
        Me.btnSeleccionarFoto.Name = "btnSeleccionarFoto"
        Me.btnSeleccionarFoto.Size = New System.Drawing.Size(82, 23)
        Me.btnSeleccionarFoto.TabIndex = 1
        Me.btnSeleccionarFoto.Text = "Buscar Foto..."
        Me.btnSeleccionarFoto.UseVisualStyleBackColor = True
        '
        'TableLayoutPanelDatosGenerales
        '
        Me.TableLayoutPanelDatosGenerales.ColumnCount = 2
        Me.TableLayoutPanelDatosGenerales.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanelDatosGenerales.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.lblCI, 0, 0)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.txtCI, 1, 0)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.lblNombre, 0, 1)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.txtNombre, 1, 1)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.lblFechaIngreso, 0, 2)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.dtpFechaIngreso, 1, 2)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.lblTipoFuncionario, 0, 3)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.cboTipoFuncionario, 1, 3)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.lblCargo, 0, 4)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.cboCargo, 1, 4)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.lblEscalafon, 0, 5)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.cboEscalafon, 1, 5)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.lblFuncion, 0, 6)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.cboFuncion, 1, 6)
        Me.TableLayoutPanelDatosGenerales.Controls.Add(Me.chkActivo, 1, 7)
        Me.TableLayoutPanelDatosGenerales.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelDatosGenerales.Location = New System.Drawing.Point(203, 3)
        Me.TableLayoutPanelDatosGenerales.Name = "TableLayoutPanelDatosGenerales"
        Me.TableLayoutPanelDatosGenerales.RowCount = 8
        Me.TableLayoutPanelGeneral.SetRowSpan(Me.TableLayoutPanelDatosGenerales, 2)
        Me.TableLayoutPanelDatosGenerales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanelDatosGenerales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanelDatosGenerales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanelDatosGenerales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanelDatosGenerales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanelDatosGenerales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanelDatosGenerales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanelDatosGenerales.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5!))
        Me.TableLayoutPanelDatosGenerales.Size = New System.Drawing.Size(550, 363)
        Me.TableLayoutPanelDatosGenerales.TabIndex = 2
        '
        'lblCI
        '
        Me.lblCI.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCI.AutoSize = True
        Me.lblCI.Location = New System.Drawing.Point(72, 16)
        Me.lblCI.Name = "lblCI"
        Me.lblCI.Size = New System.Drawing.Size(20, 13)
        Me.lblCI.TabIndex = 0
        Me.lblCI.Text = "CI:"
        '
        'txtCI
        '
        Me.txtCI.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCI.Location = New System.Drawing.Point(98, 12)
        Me.txtCI.Name = "txtCI"
        Me.txtCI.Size = New System.Drawing.Size(449, 20)
        Me.txtCI.TabIndex = 1
        '
        'lblNombre
        '
        Me.lblNombre.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblNombre.AutoSize = True
        Me.lblNombre.Location = New System.Drawing.Point(45, 61)
        Me.lblNombre.Name = "lblNombre"
        Me.lblNombre.Size = New System.Drawing.Size(47, 13)
        Me.lblNombre.TabIndex = 2
        Me.lblNombre.Text = "Nombre:"
        '
        'txtNombre
        '
        Me.txtNombre.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombre.Location = New System.Drawing.Point(98, 57)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(449, 20)
        Me.txtNombre.TabIndex = 3
        '
        'lblFechaIngreso
        '
        Me.lblFechaIngreso.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFechaIngreso.AutoSize = True
        Me.lblFechaIngreso.Location = New System.Drawing.Point(14, 106)
        Me.lblFechaIngreso.Name = "lblFechaIngreso"
        Me.lblFechaIngreso.Size = New System.Drawing.Size(78, 13)
        Me.lblFechaIngreso.TabIndex = 4
        Me.lblFechaIngreso.Text = "Fecha Ingreso:"
        '
        'dtpFechaIngreso
        '
        Me.dtpFechaIngreso.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.dtpFechaIngreso.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaIngreso.Location = New System.Drawing.Point(98, 102)
        Me.dtpFechaIngreso.Name = "dtpFechaIngreso"
        Me.dtpFechaIngreso.Size = New System.Drawing.Size(150, 20)
        Me.dtpFechaIngreso.TabIndex = 5
        '
        'lblTipoFuncionario
        '
        Me.lblTipoFuncionario.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblTipoFuncionario.AutoSize = True
        Me.lblTipoFuncionario.Location = New System.Drawing.Point(3, 151)
        Me.lblTipoFuncionario.Name = "lblTipoFuncionario"
        Me.lblTipoFuncionario.Size = New System.Drawing.Size(89, 13)
        Me.lblTipoFuncionario.TabIndex = 6
        Me.lblTipoFuncionario.Text = "Tipo Funcionario:"
        '
        'cboTipoFuncionario
        '
        Me.cboTipoFuncionario.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboTipoFuncionario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTipoFuncionario.FormattingEnabled = True
        Me.cboTipoFuncionario.Location = New System.Drawing.Point(98, 147)
        Me.cboTipoFuncionario.Name = "cboTipoFuncionario"
        Me.cboTipoFuncionario.Size = New System.Drawing.Size(449, 21)
        Me.cboTipoFuncionario.TabIndex = 7
        '
        'lblCargo
        '
        Me.lblCargo.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblCargo.AutoSize = True
        Me.lblCargo.Location = New System.Drawing.Point(54, 196)
        Me.lblCargo.Name = "lblCargo"
        Me.lblCargo.Size = New System.Drawing.Size(38, 13)
        Me.lblCargo.TabIndex = 8
        Me.lblCargo.Text = "Cargo:"
        '
        'cboCargo
        '
        Me.cboCargo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboCargo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCargo.FormattingEnabled = True
        Me.cboCargo.Location = New System.Drawing.Point(98, 192)
        Me.cboCargo.Name = "cboCargo"
        Me.cboCargo.Size = New System.Drawing.Size(449, 21)
        Me.cboCargo.TabIndex = 9
        '
        'lblEscalafon
        '
        Me.lblEscalafon.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblEscalafon.AutoSize = True
        Me.lblEscalafon.Location = New System.Drawing.Point(35, 241)
        Me.lblEscalafon.Name = "lblEscalafon"
        Me.lblEscalafon.Size = New System.Drawing.Size(57, 13)
        Me.lblEscalafon.TabIndex = 10
        Me.lblEscalafon.Text = "Escalafón:"
        '
        'cboEscalafon
        '
        Me.cboEscalafon.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboEscalafon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEscalafon.FormattingEnabled = True
        Me.cboEscalafon.Location = New System.Drawing.Point(98, 237)
        Me.cboEscalafon.Name = "cboEscalafon"
        Me.cboEscalafon.Size = New System.Drawing.Size(449, 21)
        Me.cboEscalafon.TabIndex = 11
        '
        'lblFuncion
        '
        Me.lblFuncion.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblFuncion.AutoSize = True
        Me.lblFuncion.Location = New System.Drawing.Point(44, 286)
        Me.lblFuncion.Name = "lblFuncion"
        Me.lblFuncion.Size = New System.Drawing.Size(48, 13)
        Me.lblFuncion.TabIndex = 12
        Me.lblFuncion.Text = "Función:"
        '
        'cboFuncion
        '
        Me.cboFuncion.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFuncion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFuncion.FormattingEnabled = True
        Me.cboFuncion.Location = New System.Drawing.Point(98, 282)
        Me.cboFuncion.Name = "cboFuncion"
        Me.cboFuncion.Size = New System.Drawing.Size(449, 21)
        Me.cboFuncion.TabIndex = 13
        '
        'chkActivo
        '
        Me.chkActivo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.chkActivo.AutoSize = True
        Me.chkActivo.Checked = True
        Me.chkActivo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkActivo.Location = New System.Drawing.Point(98, 330)
        Me.chkActivo.Name = "chkActivo"
        Me.chkActivo.Size = New System.Drawing.Size(56, 17)
        Me.chkActivo.TabIndex = 14
        Me.chkActivo.Text = "Activo"
        Me.chkActivo.UseVisualStyleBackColor = True
        '
        'TabControlMain
        '
        Me.TabControlMain.Controls.Add(Me.TabPageGeneral)
        Me.TabControlMain.Controls.Add(Me.TabPageDatosPersonales)
        Me.TabControlMain.Controls.Add(Me.TabPageDotacion)
        Me.TabControlMain.Controls.Add(Me.TabPageEstadosTransitorios)
        Me.TabControlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlMain.Location = New System.Drawing.Point(0, 0)
        Me.TabControlMain.Name = "TabControlMain"
        Me.TabControlMain.SelectedIndex = 0
        Me.TabControlMain.Size = New System.Drawing.Size(784, 415)
        Me.TabControlMain.TabIndex = 2
        '
        'frmFuncionarioCrear
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 461)
        Me.Controls.Add(Me.TabControlMain)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.MinimumSize = New System.Drawing.Size(800, 499)
        Me.Name = "frmFuncionarioCrear"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Gestión de Funcionario"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.TabPageEstadosTransitorios.ResumeLayout(False)
        Me.TableLayoutPanelEstados.ResumeLayout(False)
        Me.TableLayoutPanelEstados.PerformLayout()
        CType(Me.dgvEstadosTransitorios, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanelEstados.ResumeLayout(False)
        Me.TabPageDotacion.ResumeLayout(False)
        Me.TableLayoutPanelDotacion.ResumeLayout(False)
        Me.TableLayoutPanelDotacion.PerformLayout()
        CType(Me.dgvDotacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanelDotacion.ResumeLayout(False)
        Me.TabPageDatosPersonales.ResumeLayout(False)
        Me.TableLayoutPanelDatosPersonales.ResumeLayout(False)
        Me.TableLayoutPanelDatosPersonales.PerformLayout()
        Me.TabPageGeneral.ResumeLayout(False)
        Me.TableLayoutPanelGeneral.ResumeLayout(False)
        CType(Me.pbFoto, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanelDatosGenerales.ResumeLayout(False)
        Me.TableLayoutPanelDatosGenerales.PerformLayout()
        Me.TabControlMain.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
    Friend WithEvents TabPageEstadosTransitorios As TabPage
    Friend WithEvents TableLayoutPanelEstados As TableLayoutPanel
    Friend WithEvents dgvEstadosTransitorios As DataGridView
    Friend WithEvents FlowLayoutPanelEstados As FlowLayoutPanel
    Friend WithEvents btnQuitarEstado As Button
    Friend WithEvents btnEditarEstado As Button
    Friend WithEvents btnAñadirEstado As Button
    Friend WithEvents TabPageDotacion As TabPage
    Friend WithEvents TableLayoutPanelDotacion As TableLayoutPanel
    Friend WithEvents dgvDotacion As DataGridView
    Friend WithEvents FlowLayoutPanelDotacion As FlowLayoutPanel
    Friend WithEvents btnQuitarDotacion As Button
    Friend WithEvents btnEditarDotacion As Button
    Friend WithEvents btnAñadirDotacion As Button
    Friend WithEvents TabPageDatosPersonales As TabPage
    Friend WithEvents TableLayoutPanelDatosPersonales As TableLayoutPanel
    Friend WithEvents lblFechaNacimiento As Label
    Friend WithEvents dtpFechaNacimiento As DateTimePicker
    Friend WithEvents lblDomicilio As Label
    Friend WithEvents txtDomicilio As TextBox
    Friend WithEvents lblTelefono As Label
    Friend WithEvents txtTelefono As TextBox
    Friend WithEvents lblEmail As Label
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents lblEstadoCivil As Label
    Friend WithEvents cboEstadoCivil As ComboBox
    Friend WithEvents lblGenero As Label
    Friend WithEvents cboGenero As ComboBox
    Friend WithEvents lblNivelEstudio As Label
    Friend WithEvents cboNivelEstudio As ComboBox
    Friend WithEvents TabPageGeneral As TabPage
    Friend WithEvents TableLayoutPanelGeneral As TableLayoutPanel
    Friend WithEvents pbFoto As PictureBox
    Friend WithEvents btnSeleccionarFoto As Button
    Friend WithEvents TableLayoutPanelDatosGenerales As TableLayoutPanel
    Friend WithEvents lblCI As Label
    Friend WithEvents txtCI As TextBox
    Friend WithEvents lblNombre As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents lblFechaIngreso As Label
    Friend WithEvents dtpFechaIngreso As DateTimePicker
    Friend WithEvents lblTipoFuncionario As Label
    Friend WithEvents cboTipoFuncionario As ComboBox
    Friend WithEvents lblCargo As Label
    Friend WithEvents cboCargo As ComboBox
    Friend WithEvents lblEscalafon As Label
    Friend WithEvents cboEscalafon As ComboBox
    Friend WithEvents lblFuncion As Label
    Friend WithEvents cboFuncion As ComboBox
    Friend WithEvents chkActivo As CheckBox
    Friend WithEvents TabControlMain As TabControl
    Friend WithEvents chkVerHistorial As CheckBox
End Class