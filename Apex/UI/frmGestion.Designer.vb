<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGestion
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
        Me.TabControlGestion = New System.Windows.Forms.TabControl()
        Me.TabPageLicencias = New System.Windows.Forms.TabPage()
        Me.dgvLicencias = New System.Windows.Forms.DataGridView()
        Me.PanelLicencias = New System.Windows.Forms.Panel()
        Me.btnConceptoFuncional = New System.Windows.Forms.Button()
        Me.btnEliminarLicencia = New System.Windows.Forms.Button()
        Me.btnEditarLicencia = New System.Windows.Forms.Button()
        Me.btnNuevaLicencia = New System.Windows.Forms.Button()
        Me.PanelBusquedaLicencias = New System.Windows.Forms.Panel()
        Me.txtBusquedaLicencia = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabPageNotificaciones = New System.Windows.Forms.TabPage()
        Me.splitContenedorNotificaciones = New System.Windows.Forms.SplitContainer()
        Me.dgvNotificaciones = New System.Windows.Forms.DataGridView()
        Me.txtTextoNotificacion = New System.Windows.Forms.TextBox()
        Me.PanelNotificaciones = New System.Windows.Forms.Panel()
        Me.btnCambiarEstado = New System.Windows.Forms.Button()
        Me.btnEliminarNotificacion = New System.Windows.Forms.Button()
        Me.btnEditarNotificacion = New System.Windows.Forms.Button()
        Me.btnNuevaNotificacion = New System.Windows.Forms.Button()
        Me.PanelBusquedaNotificaciones = New System.Windows.Forms.Panel()
        Me.txtBusquedaNotificacion = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TabPageSanciones = New System.Windows.Forms.TabPage()
        Me.dgvSanciones = New System.Windows.Forms.DataGridView()
        Me.PanelSanciones = New System.Windows.Forms.Panel()
        Me.btnEliminarSancion = New System.Windows.Forms.Button()
        Me.btnEditarSancion = New System.Windows.Forms.Button()
        Me.btnNuevaSancion = New System.Windows.Forms.Button()
        Me.PanelBusquedaSanciones = New System.Windows.Forms.Panel()
        Me.txtBusquedaSancion = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TabControlGestion.SuspendLayout()
        Me.TabPageLicencias.SuspendLayout()
        CType(Me.dgvLicencias, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelLicencias.SuspendLayout()
        Me.PanelBusquedaLicencias.SuspendLayout()
        Me.TabPageNotificaciones.SuspendLayout()
        CType(Me.splitContenedorNotificaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedorNotificaciones.Panel1.SuspendLayout()
        Me.splitContenedorNotificaciones.Panel2.SuspendLayout()
        Me.splitContenedorNotificaciones.SuspendLayout()
        CType(Me.dgvNotificaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelNotificaciones.SuspendLayout()
        Me.PanelBusquedaNotificaciones.SuspendLayout()
        Me.TabPageSanciones.SuspendLayout()
        CType(Me.dgvSanciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelSanciones.SuspendLayout()
        Me.PanelBusquedaSanciones.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControlGestion
        '
        Me.TabControlGestion.Controls.Add(Me.TabPageLicencias)
        Me.TabControlGestion.Controls.Add(Me.TabPageNotificaciones)
        Me.TabControlGestion.Controls.Add(Me.TabPageSanciones)
        Me.TabControlGestion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControlGestion.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControlGestion.Location = New System.Drawing.Point(0, 0)
        Me.TabControlGestion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabControlGestion.Name = "TabControlGestion"
        Me.TabControlGestion.SelectedIndex = 0
        Me.TabControlGestion.Size = New System.Drawing.Size(900, 562)
        Me.TabControlGestion.TabIndex = 0
        '
        'TabPageLicencias
        '
        Me.TabPageLicencias.Controls.Add(Me.dgvLicencias)
        Me.TabPageLicencias.Controls.Add(Me.PanelLicencias)
        Me.TabPageLicencias.Controls.Add(Me.PanelBusquedaLicencias)
        Me.TabPageLicencias.Location = New System.Drawing.Point(4, 37)
        Me.TabPageLicencias.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabPageLicencias.Name = "TabPageLicencias"
        Me.TabPageLicencias.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabPageLicencias.Size = New System.Drawing.Size(892, 521)
        Me.TabPageLicencias.TabIndex = 0
        Me.TabPageLicencias.Text = "Licencias"
        Me.TabPageLicencias.UseVisualStyleBackColor = True
        '
        'dgvLicencias
        '
        Me.dgvLicencias.AllowUserToAddRows = False
        Me.dgvLicencias.AllowUserToDeleteRows = False
        Me.dgvLicencias.AllowUserToResizeColumns = False
        Me.dgvLicencias.AllowUserToResizeRows = False
        Me.dgvLicencias.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvLicencias.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvLicencias.Location = New System.Drawing.Point(3, 66)
        Me.dgvLicencias.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dgvLicencias.Name = "dgvLicencias"
        Me.dgvLicencias.ReadOnly = True
        Me.dgvLicencias.RowHeadersWidth = 51
        Me.dgvLicencias.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvLicencias.Size = New System.Drawing.Size(886, 389)
        Me.dgvLicencias.TabIndex = 1
        '
        'PanelLicencias
        '
        Me.PanelLicencias.Controls.Add(Me.btnConceptoFuncional)
        Me.PanelLicencias.Controls.Add(Me.btnEliminarLicencia)
        Me.PanelLicencias.Controls.Add(Me.btnEditarLicencia)
        Me.PanelLicencias.Controls.Add(Me.btnNuevaLicencia)
        Me.PanelLicencias.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelLicencias.Location = New System.Drawing.Point(3, 455)
        Me.PanelLicencias.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelLicencias.Name = "PanelLicencias"
        Me.PanelLicencias.Size = New System.Drawing.Size(886, 62)
        Me.PanelLicencias.TabIndex = 0
        '
        'btnConceptoFuncional
        '
        Me.btnConceptoFuncional.Location = New System.Drawing.Point(15, 12)
        Me.btnConceptoFuncional.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnConceptoFuncional.Name = "btnConceptoFuncional"
        Me.btnConceptoFuncional.Size = New System.Drawing.Size(180, 38)
        Me.btnConceptoFuncional.TabIndex = 3
        Me.btnConceptoFuncional.Text = "Ver Concepto Funcional"
        Me.btnConceptoFuncional.UseVisualStyleBackColor = True
        '
        'btnEliminarLicencia
        '
        Me.btnEliminarLicencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarLicencia.Location = New System.Drawing.Point(572, 12)
        Me.btnEliminarLicencia.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEliminarLicencia.Name = "btnEliminarLicencia"
        Me.btnEliminarLicencia.Size = New System.Drawing.Size(96, 38)
        Me.btnEliminarLicencia.TabIndex = 2
        Me.btnEliminarLicencia.Text = "Eliminar"
        Me.btnEliminarLicencia.UseVisualStyleBackColor = True
        '
        'btnEditarLicencia
        '
        Me.btnEditarLicencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditarLicencia.Location = New System.Drawing.Point(675, 12)
        Me.btnEditarLicencia.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEditarLicencia.Name = "btnEditarLicencia"
        Me.btnEditarLicencia.Size = New System.Drawing.Size(96, 38)
        Me.btnEditarLicencia.TabIndex = 1
        Me.btnEditarLicencia.Text = "Editar..."
        Me.btnEditarLicencia.UseVisualStyleBackColor = True
        '
        'btnNuevaLicencia
        '
        Me.btnNuevaLicencia.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaLicencia.Location = New System.Drawing.Point(777, 12)
        Me.btnNuevaLicencia.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNuevaLicencia.Name = "btnNuevaLicencia"
        Me.btnNuevaLicencia.Size = New System.Drawing.Size(96, 38)
        Me.btnNuevaLicencia.TabIndex = 0
        Me.btnNuevaLicencia.Text = "Nueva..."
        Me.btnNuevaLicencia.UseVisualStyleBackColor = True
        '
        'PanelBusquedaLicencias
        '
        Me.PanelBusquedaLicencias.Controls.Add(Me.txtBusquedaLicencia)
        Me.PanelBusquedaLicencias.Controls.Add(Me.Label1)
        Me.PanelBusquedaLicencias.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaLicencias.Location = New System.Drawing.Point(3, 4)
        Me.PanelBusquedaLicencias.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelBusquedaLicencias.Name = "PanelBusquedaLicencias"
        Me.PanelBusquedaLicencias.Size = New System.Drawing.Size(886, 62)
        Me.PanelBusquedaLicencias.TabIndex = 2
        '
        'txtBusquedaLicencia
        '
        Me.txtBusquedaLicencia.Location = New System.Drawing.Point(131, 14)
        Me.txtBusquedaLicencia.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBusquedaLicencia.Name = "txtBusquedaLicencia"
        Me.txtBusquedaLicencia.Size = New System.Drawing.Size(380, 33)
        Me.txtBusquedaLicencia.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 28)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Funcionario:"
        '
        'TabPageNotificaciones
        '
        Me.TabPageNotificaciones.Controls.Add(Me.splitContenedorNotificaciones)
        Me.TabPageNotificaciones.Controls.Add(Me.PanelNotificaciones)
        Me.TabPageNotificaciones.Controls.Add(Me.PanelBusquedaNotificaciones)
        Me.TabPageNotificaciones.Location = New System.Drawing.Point(4, 37)
        Me.TabPageNotificaciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabPageNotificaciones.Name = "TabPageNotificaciones"
        Me.TabPageNotificaciones.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabPageNotificaciones.Size = New System.Drawing.Size(892, 521)
        Me.TabPageNotificaciones.TabIndex = 1
        Me.TabPageNotificaciones.Text = "Notificaciones"
        Me.TabPageNotificaciones.UseVisualStyleBackColor = True
        '
        'splitContenedorNotificaciones
        '
        Me.splitContenedorNotificaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContenedorNotificaciones.Location = New System.Drawing.Point(3, 66)
        Me.splitContenedorNotificaciones.Name = "splitContenedorNotificaciones"
        Me.splitContenedorNotificaciones.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitContenedorNotificaciones.Panel1
        '
        Me.splitContenedorNotificaciones.Panel1.Controls.Add(Me.dgvNotificaciones)
        '
        'splitContenedorNotificaciones.Panel2
        '
        Me.splitContenedorNotificaciones.Panel2.Controls.Add(Me.txtTextoNotificacion)
        Me.splitContenedorNotificaciones.Panel2.Padding = New System.Windows.Forms.Padding(5)
        Me.splitContenedorNotificaciones.Size = New System.Drawing.Size(886, 389)
        Me.splitContenedorNotificaciones.SplitterDistance = 183
        Me.splitContenedorNotificaciones.TabIndex = 4
        '
        'dgvNotificaciones
        '
        Me.dgvNotificaciones.AllowUserToAddRows = False
        Me.dgvNotificaciones.AllowUserToDeleteRows = False
        Me.dgvNotificaciones.AllowUserToResizeColumns = False
        Me.dgvNotificaciones.AllowUserToResizeRows = False
        Me.dgvNotificaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNotificaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvNotificaciones.Location = New System.Drawing.Point(0, 0)
        Me.dgvNotificaciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.dgvNotificaciones.Name = "dgvNotificaciones"
        Me.dgvNotificaciones.ReadOnly = True
        Me.dgvNotificaciones.RowHeadersWidth = 51
        Me.dgvNotificaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNotificaciones.Size = New System.Drawing.Size(886, 183)
        Me.dgvNotificaciones.TabIndex = 2
        '
        'txtTextoNotificacion
        '
        Me.txtTextoNotificacion.BackColor = System.Drawing.SystemColors.Info
        Me.txtTextoNotificacion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtTextoNotificacion.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTextoNotificacion.Location = New System.Drawing.Point(5, 5)
        Me.txtTextoNotificacion.Multiline = True
        Me.txtTextoNotificacion.Name = "txtTextoNotificacion"
        Me.txtTextoNotificacion.ReadOnly = True
        Me.txtTextoNotificacion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTextoNotificacion.Size = New System.Drawing.Size(876, 192)
        Me.txtTextoNotificacion.TabIndex = 0
        '
        'PanelNotificaciones
        '
        Me.PanelNotificaciones.Controls.Add(Me.btnCambiarEstado)
        Me.PanelNotificaciones.Controls.Add(Me.btnEliminarNotificacion)
        Me.PanelNotificaciones.Controls.Add(Me.btnEditarNotificacion)
        Me.PanelNotificaciones.Controls.Add(Me.btnNuevaNotificacion)
        Me.PanelNotificaciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelNotificaciones.Location = New System.Drawing.Point(3, 455)
        Me.PanelNotificaciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelNotificaciones.Name = "PanelNotificaciones"
        Me.PanelNotificaciones.Size = New System.Drawing.Size(886, 62)
        Me.PanelNotificaciones.TabIndex = 1
        '
        'btnCambiarEstado
        '
        Me.btnCambiarEstado.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCambiarEstado.Location = New System.Drawing.Point(417, 12)
        Me.btnCambiarEstado.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCambiarEstado.Name = "btnCambiarEstado"
        Me.btnCambiarEstado.Size = New System.Drawing.Size(135, 38)
        Me.btnCambiarEstado.TabIndex = 3
        Me.btnCambiarEstado.Text = "Cambiar Estado..."
        Me.btnCambiarEstado.UseVisualStyleBackColor = True
        '
        'btnEliminarNotificacion
        '
        Me.btnEliminarNotificacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarNotificacion.Location = New System.Drawing.Point(559, 12)
        Me.btnEliminarNotificacion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEliminarNotificacion.Name = "btnEliminarNotificacion"
        Me.btnEliminarNotificacion.Size = New System.Drawing.Size(96, 38)
        Me.btnEliminarNotificacion.TabIndex = 2
        Me.btnEliminarNotificacion.Text = "Eliminar"
        Me.btnEliminarNotificacion.UseVisualStyleBackColor = True
        '
        'btnEditarNotificacion
        '
        Me.btnEditarNotificacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditarNotificacion.Location = New System.Drawing.Point(661, 12)
        Me.btnEditarNotificacion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEditarNotificacion.Name = "btnEditarNotificacion"
        Me.btnEditarNotificacion.Size = New System.Drawing.Size(96, 38)
        Me.btnEditarNotificacion.TabIndex = 1
        Me.btnEditarNotificacion.Text = "Editar..."
        Me.btnEditarNotificacion.UseVisualStyleBackColor = True
        '
        'btnNuevaNotificacion
        '
        Me.btnNuevaNotificacion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaNotificacion.Location = New System.Drawing.Point(764, 12)
        Me.btnNuevaNotificacion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNuevaNotificacion.Name = "btnNuevaNotificacion"
        Me.btnNuevaNotificacion.Size = New System.Drawing.Size(96, 38)
        Me.btnNuevaNotificacion.TabIndex = 0
        Me.btnNuevaNotificacion.Text = "Nueva..."
        Me.btnNuevaNotificacion.UseVisualStyleBackColor = True
        '
        'PanelBusquedaNotificaciones
        '
        Me.PanelBusquedaNotificaciones.Controls.Add(Me.txtBusquedaNotificacion)
        Me.PanelBusquedaNotificaciones.Controls.Add(Me.Label2)
        Me.PanelBusquedaNotificaciones.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaNotificaciones.Location = New System.Drawing.Point(3, 4)
        Me.PanelBusquedaNotificaciones.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.PanelBusquedaNotificaciones.Name = "PanelBusquedaNotificaciones"
        Me.PanelBusquedaNotificaciones.Size = New System.Drawing.Size(886, 62)
        Me.PanelBusquedaNotificaciones.TabIndex = 3
        '
        'txtBusquedaNotificacion
        '
        Me.txtBusquedaNotificacion.Location = New System.Drawing.Point(131, 14)
        Me.txtBusquedaNotificacion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBusquedaNotificacion.Name = "txtBusquedaNotificacion"
        Me.txtBusquedaNotificacion.Size = New System.Drawing.Size(380, 33)
        Me.txtBusquedaNotificacion.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(119, 28)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Funcionario:"
        '
        'TabPageSanciones
        '
        Me.TabPageSanciones.Controls.Add(Me.dgvSanciones)
        Me.TabPageSanciones.Controls.Add(Me.PanelSanciones)
        Me.TabPageSanciones.Controls.Add(Me.PanelBusquedaSanciones)
        Me.TabPageSanciones.Location = New System.Drawing.Point(4, 37)
        Me.TabPageSanciones.Name = "TabPageSanciones"
        Me.TabPageSanciones.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPageSanciones.Size = New System.Drawing.Size(892, 521)
        Me.TabPageSanciones.TabIndex = 2
        Me.TabPageSanciones.Text = "Sanciones"
        Me.TabPageSanciones.UseVisualStyleBackColor = True
        '
        'dgvSanciones
        '
        Me.dgvSanciones.AllowUserToAddRows = False
        Me.dgvSanciones.AllowUserToDeleteRows = False
        Me.dgvSanciones.AllowUserToResizeColumns = False
        Me.dgvSanciones.AllowUserToResizeRows = False
        Me.dgvSanciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSanciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvSanciones.Location = New System.Drawing.Point(3, 65)
        Me.dgvSanciones.Name = "dgvSanciones"
        Me.dgvSanciones.ReadOnly = True
        Me.dgvSanciones.RowHeadersWidth = 51
        Me.dgvSanciones.RowTemplate.Height = 24
        Me.dgvSanciones.Size = New System.Drawing.Size(886, 391)
        Me.dgvSanciones.TabIndex = 2
        '
        'PanelSanciones
        '
        Me.PanelSanciones.Controls.Add(Me.btnEliminarSancion)
        Me.PanelSanciones.Controls.Add(Me.btnEditarSancion)
        Me.PanelSanciones.Controls.Add(Me.btnNuevaSancion)
        Me.PanelSanciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelSanciones.Location = New System.Drawing.Point(3, 456)
        Me.PanelSanciones.Name = "PanelSanciones"
        Me.PanelSanciones.Size = New System.Drawing.Size(886, 62)
        Me.PanelSanciones.TabIndex = 1
        '
        'btnEliminarSancion
        '
        Me.btnEliminarSancion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEliminarSancion.Location = New System.Drawing.Point(572, 12)
        Me.btnEliminarSancion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEliminarSancion.Name = "btnEliminarSancion"
        Me.btnEliminarSancion.Size = New System.Drawing.Size(96, 38)
        Me.btnEliminarSancion.TabIndex = 2
        Me.btnEliminarSancion.Text = "Eliminar"
        Me.btnEliminarSancion.UseVisualStyleBackColor = True
        '
        'btnEditarSancion
        '
        Me.btnEditarSancion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditarSancion.Location = New System.Drawing.Point(675, 12)
        Me.btnEditarSancion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnEditarSancion.Name = "btnEditarSancion"
        Me.btnEditarSancion.Size = New System.Drawing.Size(96, 38)
        Me.btnEditarSancion.TabIndex = 1
        Me.btnEditarSancion.Text = "Editar..."
        Me.btnEditarSancion.UseVisualStyleBackColor = True
        '
        'btnNuevaSancion
        '
        Me.btnNuevaSancion.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNuevaSancion.Location = New System.Drawing.Point(777, 12)
        Me.btnNuevaSancion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnNuevaSancion.Name = "btnNuevaSancion"
        Me.btnNuevaSancion.Size = New System.Drawing.Size(96, 38)
        Me.btnNuevaSancion.TabIndex = 0
        Me.btnNuevaSancion.Text = "Nueva..."
        Me.btnNuevaSancion.UseVisualStyleBackColor = True
        '
        'PanelBusquedaSanciones
        '
        Me.PanelBusquedaSanciones.Controls.Add(Me.txtBusquedaSancion)
        Me.PanelBusquedaSanciones.Controls.Add(Me.Label3)
        Me.PanelBusquedaSanciones.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaSanciones.Location = New System.Drawing.Point(3, 3)
        Me.PanelBusquedaSanciones.Name = "PanelBusquedaSanciones"
        Me.PanelBusquedaSanciones.Size = New System.Drawing.Size(886, 62)
        Me.PanelBusquedaSanciones.TabIndex = 0
        '
        'txtBusquedaSancion
        '
        Me.txtBusquedaSancion.Location = New System.Drawing.Point(131, 14)
        Me.txtBusquedaSancion.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBusquedaSancion.Name = "txtBusquedaSancion"
        Me.txtBusquedaSancion.Size = New System.Drawing.Size(380, 33)
        Me.txtBusquedaSancion.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 28)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Funcionario:"
        '
        'frmGestion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(900, 562)
        Me.Controls.Add(Me.TabControlGestion)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "frmGestion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestión de Licencias y Notificaciones"
        Me.TabControlGestion.ResumeLayout(False)
        Me.TabPageLicencias.ResumeLayout(False)
        CType(Me.dgvLicencias, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelLicencias.ResumeLayout(False)
        Me.PanelBusquedaLicencias.ResumeLayout(False)
        Me.PanelBusquedaLicencias.PerformLayout()
        Me.TabPageNotificaciones.ResumeLayout(False)
        Me.splitContenedorNotificaciones.Panel1.ResumeLayout(False)
        Me.splitContenedorNotificaciones.Panel2.ResumeLayout(False)
        Me.splitContenedorNotificaciones.Panel2.PerformLayout()
        CType(Me.splitContenedorNotificaciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedorNotificaciones.ResumeLayout(False)
        CType(Me.dgvNotificaciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelNotificaciones.ResumeLayout(False)
        Me.PanelBusquedaNotificaciones.ResumeLayout(False)
        Me.PanelBusquedaNotificaciones.PerformLayout()
        Me.TabPageSanciones.ResumeLayout(False)
        CType(Me.dgvSanciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelSanciones.ResumeLayout(False)
        Me.PanelBusquedaSanciones.ResumeLayout(False)
        Me.PanelBusquedaSanciones.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControlGestion As TabControl
    Friend WithEvents TabPageLicencias As TabPage
    Friend WithEvents TabPageNotificaciones As TabPage
    Friend WithEvents PanelLicencias As Panel
    Friend WithEvents dgvLicencias As DataGridView
    Friend WithEvents btnEliminarLicencia As Button
    Friend WithEvents btnEditarLicencia As Button
    Friend WithEvents btnNuevaLicencia As Button
    Friend WithEvents PanelNotificaciones As Panel
    Friend WithEvents btnCambiarEstado As Button
    Friend WithEvents btnEliminarNotificacion As Button
    Friend WithEvents btnEditarNotificacion As Button
    Friend WithEvents btnNuevaNotificacion As Button
    Friend WithEvents dgvNotificaciones As DataGridView
    Friend WithEvents PanelBusquedaLicencias As Panel
    Friend WithEvents txtBusquedaLicencia As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PanelBusquedaNotificaciones As Panel
    Friend WithEvents txtBusquedaNotificacion As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TabPageSanciones As TabPage
    Friend WithEvents dgvSanciones As DataGridView
    Friend WithEvents PanelSanciones As Panel
    Friend WithEvents PanelBusquedaSanciones As Panel
    Friend WithEvents txtBusquedaSancion As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnEliminarSancion As Button
    Friend WithEvents btnEditarSancion As Button
    Friend WithEvents btnNuevaSancion As Button
    Friend WithEvents btnConceptoFuncional As Button
    ' --- INICIO DE LA MODIFICACIÓN ---
    Friend WithEvents splitContenedorNotificaciones As SplitContainer
    Friend WithEvents txtTextoNotificacion As TextBox
    ' --- FIN DE LA MODIFICACIÓN ---
End Class