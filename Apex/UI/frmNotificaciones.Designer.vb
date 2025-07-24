<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNotificaciones
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
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.txtBusquedaFuncionario = New System.Windows.Forms.TextBox()
        Me.lblFuncionario = New System.Windows.Forms.Label()
        Me.panelAcciones = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnNueva = New System.Windows.Forms.Button()
        Me.btnEditar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnCambiarEstado = New System.Windows.Forms.Button()
        Me.splitContenedor = New System.Windows.Forms.SplitContainer()
        Me.dgvNotificaciones = New System.Windows.Forms.DataGridView()
        Me.txtTextoNotificacion = New System.Windows.Forms.TextBox()
        Me.panelFiltros.SuspendLayout()
        Me.panelAcciones.SuspendLayout()
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContenedor.Panel1.SuspendLayout()
        Me.splitContenedor.Panel2.SuspendLayout()
        Me.splitContenedor.SuspendLayout()
        CType(Me.dgvNotificaciones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'panelFiltros
        '
        Me.panelFiltros.BackColor = System.Drawing.Color.WhiteSmoke
        Me.panelFiltros.Controls.Add(Me.btnBuscar)
        Me.panelFiltros.Controls.Add(Me.txtBusquedaFuncionario)
        Me.panelFiltros.Controls.Add(Me.lblFuncionario)
        Me.panelFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelFiltros.Location = New System.Drawing.Point(0, 0)
        Me.panelFiltros.Name = "panelFiltros"
        Me.panelFiltros.Size = New System.Drawing.Size(984, 60)
        Me.panelFiltros.TabIndex = 1
        '
        'btnBuscar
        '
        Me.btnBuscar.Location = New System.Drawing.Point(338, 18)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(85, 25)
        Me.btnBuscar.TabIndex = 2
        Me.btnBuscar.Text = "Buscar"
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'txtBusquedaFuncionario
        '
        Me.txtBusquedaFuncionario.Location = New System.Drawing.Point(107, 20)
        Me.txtBusquedaFuncionario.Name = "txtBusquedaFuncionario"
        Me.txtBusquedaFuncionario.Size = New System.Drawing.Size(225, 27)
        Me.txtBusquedaFuncionario.TabIndex = 1
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
        'panelAcciones
        '
        Me.panelAcciones.Controls.Add(Me.btnNueva)
        Me.panelAcciones.Controls.Add(Me.btnEditar)
        Me.panelAcciones.Controls.Add(Me.btnEliminar)
        Me.panelAcciones.Controls.Add(Me.btnCambiarEstado)
        Me.panelAcciones.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelAcciones.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.panelAcciones.Location = New System.Drawing.Point(0, 520)
        Me.panelAcciones.Name = "panelAcciones"
        Me.panelAcciones.Padding = New System.Windows.Forms.Padding(10)
        Me.panelAcciones.Size = New System.Drawing.Size(984, 42)
        Me.panelAcciones.TabIndex = 3
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
        'btnCambiarEstado
        '
        Me.btnCambiarEstado.Location = New System.Drawing.Point(534, 13)
        Me.btnCambiarEstado.Name = "btnCambiarEstado"
        Me.btnCambiarEstado.Size = New System.Drawing.Size(184, 23)
        Me.btnCambiarEstado.TabIndex = 3
        Me.btnCambiarEstado.Text = "Cambiar Estado..."
        Me.btnCambiarEstado.UseVisualStyleBackColor = True
        '
        'splitContenedor
        '
        Me.splitContenedor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContenedor.Location = New System.Drawing.Point(0, 60)
        Me.splitContenedor.Name = "splitContenedor"
        Me.splitContenedor.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'splitContenedor.Panel1
        '
        Me.splitContenedor.Panel1.Controls.Add(Me.dgvNotificaciones)
        '
        'splitContenedor.Panel2
        '
        Me.splitContenedor.Panel2.Controls.Add(Me.txtTextoNotificacion)
        Me.splitContenedor.Panel2.Padding = New System.Windows.Forms.Padding(5)
        Me.splitContenedor.Size = New System.Drawing.Size(984, 460)
        Me.splitContenedor.SplitterDistance = 280
        Me.splitContenedor.TabIndex = 4
        '
        'dgvNotificaciones
        '
        Me.dgvNotificaciones.AllowUserToAddRows = False
        Me.dgvNotificaciones.AllowUserToDeleteRows = False
        Me.dgvNotificaciones.AllowUserToResizeRows = False
        Me.dgvNotificaciones.BackgroundColor = System.Drawing.Color.White
        Me.dgvNotificaciones.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvNotificaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNotificaciones.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvNotificaciones.Location = New System.Drawing.Point(0, 0)
        Me.dgvNotificaciones.MultiSelect = False
        Me.dgvNotificaciones.Name = "dgvNotificaciones"
        Me.dgvNotificaciones.ReadOnly = True
        Me.dgvNotificaciones.RowHeadersWidth = 51
        Me.dgvNotificaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNotificaciones.Size = New System.Drawing.Size(984, 280)
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
        Me.txtTextoNotificacion.Size = New System.Drawing.Size(974, 166)
        Me.txtTextoNotificacion.TabIndex = 0
        '
        'frmNotificaciones
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 562)
        Me.Controls.Add(Me.splitContenedor)
        Me.Controls.Add(Me.panelAcciones)
        Me.Controls.Add(Me.panelFiltros)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmNotificaciones"
        Me.Text = "Gestión de Notificaciones"
        Me.panelFiltros.ResumeLayout(False)
        Me.panelFiltros.PerformLayout()
        Me.panelAcciones.ResumeLayout(False)
        Me.splitContenedor.Panel1.ResumeLayout(False)
        Me.splitContenedor.Panel2.ResumeLayout(False)
        Me.splitContenedor.Panel2.PerformLayout()
        CType(Me.splitContenedor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContenedor.ResumeLayout(False)
        CType(Me.dgvNotificaciones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents panelFiltros As Panel
    Friend WithEvents btnBuscar As Button
    Friend WithEvents txtBusquedaFuncionario As TextBox
    Friend WithEvents lblFuncionario As Label
    Friend WithEvents dgvNotificaciones As DataGridView
    Friend WithEvents panelAcciones As FlowLayoutPanel
    Friend WithEvents btnNueva As Button
    Friend WithEvents btnEditar As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents btnCambiarEstado As Button
    Friend WithEvents splitContenedor As SplitContainer
    Friend WithEvents txtTextoNotificacion As TextBox
End Class