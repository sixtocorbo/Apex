<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmConfiguracion
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
        Me.btnTurnos = New System.Windows.Forms.Button()
        Me.btnAreasTrabajo = New System.Windows.Forms.Button()
        Me.btnSecciones = New System.Windows.Forms.Button()
        Me.btnCargos = New System.Windows.Forms.Button()
        Me.btnGestionarIncidencias = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnSubDirecciones = New System.Windows.Forms.Button()
        Me.btnNomenclaturas = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnTurnos
        '
        Me.btnTurnos.Location = New System.Drawing.Point(297, 50)
        Me.btnTurnos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnTurnos.Name = "btnTurnos"
        Me.btnTurnos.Size = New System.Drawing.Size(285, 35)
        Me.btnTurnos.TabIndex = 3
        Me.btnTurnos.Text = "Gestionar Turnos"
        Me.btnTurnos.UseVisualStyleBackColor = True
        '
        'btnAreasTrabajo
        '
        Me.btnAreasTrabajo.Location = New System.Drawing.Point(4, 50)
        Me.btnAreasTrabajo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAreasTrabajo.Name = "btnAreasTrabajo"
        Me.btnAreasTrabajo.Size = New System.Drawing.Size(285, 35)
        Me.btnAreasTrabajo.TabIndex = 2
        Me.btnAreasTrabajo.Text = "Gestionar Áreas de Trabajo"
        Me.btnAreasTrabajo.UseVisualStyleBackColor = True
        '
        'btnSecciones
        '
        Me.btnSecciones.Location = New System.Drawing.Point(297, 5)
        Me.btnSecciones.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnSecciones.Name = "btnSecciones"
        Me.btnSecciones.Size = New System.Drawing.Size(285, 35)
        Me.btnSecciones.TabIndex = 1
        Me.btnSecciones.Text = "Gestionar Secciones"
        Me.btnSecciones.UseVisualStyleBackColor = True
        '
        'btnCargos
        '
        Me.btnCargos.Location = New System.Drawing.Point(4, 5)
        Me.btnCargos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCargos.Name = "btnCargos"
        Me.btnCargos.Size = New System.Drawing.Size(285, 35)
        Me.btnCargos.TabIndex = 0
        Me.btnCargos.Text = "Gestionar Cargos"
        Me.btnCargos.UseVisualStyleBackColor = True
        '
        'btnGestionarIncidencias
        '
        Me.btnGestionarIncidencias.Location = New System.Drawing.Point(4, 95)
        Me.btnGestionarIncidencias.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGestionarIncidencias.Name = "btnGestionarIncidencias"
        Me.btnGestionarIncidencias.Size = New System.Drawing.Size(285, 35)
        Me.btnGestionarIncidencias.TabIndex = 5
        Me.btnGestionarIncidencias.Text = "Gestionar Incidencias"
        Me.btnGestionarIncidencias.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Controls.Add(Me.btnCargos)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSecciones)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAreasTrabajo)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnTurnos)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnGestionarIncidencias)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnSubDirecciones)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNomenclaturas)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(717, 375)
        Me.FlowLayoutPanel1.TabIndex = 8
        '
        'btnSubDirecciones
        '
        Me.btnSubDirecciones.Location = New System.Drawing.Point(297, 95)
        Me.btnSubDirecciones.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnSubDirecciones.Name = "btnSubDirecciones"
        Me.btnSubDirecciones.Size = New System.Drawing.Size(285, 35)
        Me.btnSubDirecciones.TabIndex = 9
        Me.btnSubDirecciones.Text = "Gestionar Subdirecciones"
        Me.btnSubDirecciones.UseVisualStyleBackColor = True
        '
        'btnNomenclaturas
        '
        Me.btnNomenclaturas.Location = New System.Drawing.Point(4, 140)
        Me.btnNomenclaturas.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNomenclaturas.Name = "btnNomenclaturas"
        Me.btnNomenclaturas.Size = New System.Drawing.Size(285, 35)
        Me.btnNomenclaturas.TabIndex = 8
        Me.btnNomenclaturas.Text = "Nomenclaturas"
        Me.btnNomenclaturas.UseVisualStyleBackColor = True
        '
        'frmConfiguracion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 375)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(739, 431)
        Me.Name = "frmConfiguracion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Configuración del Sistema"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnTurnos As Button
    Friend WithEvents btnAreasTrabajo As Button
    Friend WithEvents btnSecciones As Button
    Friend WithEvents btnCargos As Button
    Friend WithEvents btnGestionarIncidencias As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnSubDirecciones As Button
    Friend WithEvents btnNomenclaturas As Button
End Class