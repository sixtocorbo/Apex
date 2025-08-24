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
        Me.TabControlPrincipal = New System.Windows.Forms.TabControl()
        Me.tpEstructura = New System.Windows.Forms.TabPage()
        Me.btnNomenclaturas = New System.Windows.Forms.Button()
        Me.btnTurnos = New System.Windows.Forms.Button()
        Me.btnAreasTrabajo = New System.Windows.Forms.Button()
        Me.btnSecciones = New System.Windows.Forms.Button()
        Me.btnCargos = New System.Windows.Forms.Button()
        Me.tpPersonal = New System.Windows.Forms.TabPage()
        Me.btnCategoriasAusencia = New System.Windows.Forms.Button()
        Me.btnTiposEstadoTransitorio = New System.Windows.Forms.Button()
        Me.btnGestionarIncidencias = New System.Windows.Forms.Button()
        Me.btnVolver = New System.Windows.Forms.Button()
        Me.TabControlPrincipal.SuspendLayout()
        Me.tpEstructura.SuspendLayout()
        Me.tpPersonal.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControlPrincipal
        '
        Me.TabControlPrincipal.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControlPrincipal.Controls.Add(Me.tpEstructura)
        Me.TabControlPrincipal.Controls.Add(Me.tpPersonal)
        Me.TabControlPrincipal.Location = New System.Drawing.Point(18, 18)
        Me.TabControlPrincipal.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TabControlPrincipal.Name = "TabControlPrincipal"
        Me.TabControlPrincipal.SelectedIndex = 0
        Me.TabControlPrincipal.Size = New System.Drawing.Size(690, 320)
        Me.TabControlPrincipal.TabIndex = 0
        '
        'tpEstructura
        '
        Me.tpEstructura.Controls.Add(Me.btnNomenclaturas)
        Me.tpEstructura.Controls.Add(Me.btnTurnos)
        Me.tpEstructura.Controls.Add(Me.btnAreasTrabajo)
        Me.tpEstructura.Controls.Add(Me.btnSecciones)
        Me.tpEstructura.Controls.Add(Me.btnCargos)
        Me.tpEstructura.Location = New System.Drawing.Point(4, 29)
        Me.tpEstructura.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpEstructura.Name = "tpEstructura"
        Me.tpEstructura.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpEstructura.Size = New System.Drawing.Size(682, 287)
        Me.tpEstructura.TabIndex = 0
        Me.tpEstructura.Text = "Gestión de Estructura Organizacional"
        Me.tpEstructura.UseVisualStyleBackColor = True
        '
        'btnNomenclaturas
        '
        Me.btnNomenclaturas.Location = New System.Drawing.Point(30, 206)
        Me.btnNomenclaturas.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNomenclaturas.Name = "btnNomenclaturas"
        Me.btnNomenclaturas.Size = New System.Drawing.Size(285, 35)
        Me.btnNomenclaturas.TabIndex = 4
        Me.btnNomenclaturas.Text = "Gestionar Nomenclaturas"
        Me.btnNomenclaturas.UseVisualStyleBackColor = True
        '
        'btnTurnos
        '
        Me.btnTurnos.Location = New System.Drawing.Point(30, 162)
        Me.btnTurnos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnTurnos.Name = "btnTurnos"
        Me.btnTurnos.Size = New System.Drawing.Size(285, 35)
        Me.btnTurnos.TabIndex = 3
        Me.btnTurnos.Text = "Gestionar Turnos"
        Me.btnTurnos.UseVisualStyleBackColor = True
        '
        'btnAreasTrabajo
        '
        Me.btnAreasTrabajo.Location = New System.Drawing.Point(30, 117)
        Me.btnAreasTrabajo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAreasTrabajo.Name = "btnAreasTrabajo"
        Me.btnAreasTrabajo.Size = New System.Drawing.Size(285, 35)
        Me.btnAreasTrabajo.TabIndex = 2
        Me.btnAreasTrabajo.Text = "Gestionar Áreas de Trabajo"
        Me.btnAreasTrabajo.UseVisualStyleBackColor = True
        '
        'btnSecciones
        '
        Me.btnSecciones.Location = New System.Drawing.Point(30, 72)
        Me.btnSecciones.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnSecciones.Name = "btnSecciones"
        Me.btnSecciones.Size = New System.Drawing.Size(285, 35)
        Me.btnSecciones.TabIndex = 1
        Me.btnSecciones.Text = "Gestionar Secciones"
        Me.btnSecciones.UseVisualStyleBackColor = True
        '
        'btnCargos
        '
        Me.btnCargos.Location = New System.Drawing.Point(30, 28)
        Me.btnCargos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCargos.Name = "btnCargos"
        Me.btnCargos.Size = New System.Drawing.Size(285, 35)
        Me.btnCargos.TabIndex = 0
        Me.btnCargos.Text = "Gestionar Cargos"
        Me.btnCargos.UseVisualStyleBackColor = True
        '
        'tpPersonal
        '
        Me.tpPersonal.Controls.Add(Me.btnCategoriasAusencia)
        Me.tpPersonal.Controls.Add(Me.btnTiposEstadoTransitorio)
        Me.tpPersonal.Controls.Add(Me.btnGestionarIncidencias)
        Me.tpPersonal.Location = New System.Drawing.Point(4, 29)
        Me.tpPersonal.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpPersonal.Name = "tpPersonal"
        Me.tpPersonal.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tpPersonal.Size = New System.Drawing.Size(682, 287)
        Me.tpPersonal.TabIndex = 1
        Me.tpPersonal.Text = "Gestión de Personal y Ausencias"
        Me.tpPersonal.UseVisualStyleBackColor = True
        '
        'btnCategoriasAusencia
        '
        Me.btnCategoriasAusencia.Location = New System.Drawing.Point(17, 100)
        Me.btnCategoriasAusencia.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCategoriasAusencia.Name = "btnCategoriasAusencia"
        Me.btnCategoriasAusencia.Size = New System.Drawing.Size(285, 35)
        Me.btnCategoriasAusencia.TabIndex = 3
        Me.btnCategoriasAusencia.Text = "Gestionar Categorías de Ausencia"
        Me.btnCategoriasAusencia.UseVisualStyleBackColor = True
        '
        'btnTiposEstadoTransitorio
        '
        Me.btnTiposEstadoTransitorio.Location = New System.Drawing.Point(17, 55)
        Me.btnTiposEstadoTransitorio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnTiposEstadoTransitorio.Name = "btnTiposEstadoTransitorio"
        Me.btnTiposEstadoTransitorio.Size = New System.Drawing.Size(285, 35)
        Me.btnTiposEstadoTransitorio.TabIndex = 2
        Me.btnTiposEstadoTransitorio.Text = "Gestionar Tipos de Estado"
        Me.btnTiposEstadoTransitorio.UseVisualStyleBackColor = True
        '
        'btnGestionarIncidencias
        '
        Me.btnGestionarIncidencias.Location = New System.Drawing.Point(17, 10)
        Me.btnGestionarIncidencias.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGestionarIncidencias.Name = "btnGestionarIncidencias"
        Me.btnGestionarIncidencias.Size = New System.Drawing.Size(285, 35)
        Me.btnGestionarIncidencias.TabIndex = 1
        Me.btnGestionarIncidencias.Text = "Gestionar Incidencias"
        Me.btnGestionarIncidencias.UseVisualStyleBackColor = True
        '
        'btnVolver
        '
        Me.btnVolver.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnVolver.Location = New System.Drawing.Point(596, 348)
        Me.btnVolver.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnVolver.Name = "btnVolver"
        Me.btnVolver.Size = New System.Drawing.Size(112, 35)
        Me.btnVolver.TabIndex = 1
        Me.btnVolver.Text = "Volver"
        Me.btnVolver.UseVisualStyleBackColor = True
        '
        'frmConfiguracion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(726, 402)
        Me.Controls.Add(Me.btnVolver)
        Me.Controls.Add(Me.TabControlPrincipal)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(739, 431)
        Me.Name = "frmConfiguracion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Configuración del Sistema"
        Me.TabControlPrincipal.ResumeLayout(False)
        Me.tpEstructura.ResumeLayout(False)
        Me.tpPersonal.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControlPrincipal As TabControl
    Friend WithEvents tpEstructura As TabPage
    Friend WithEvents btnVolver As Button
    Friend WithEvents btnNomenclaturas As Button
    Friend WithEvents btnTurnos As Button
    Friend WithEvents btnAreasTrabajo As Button
    Friend WithEvents btnSecciones As Button
    Friend WithEvents btnCargos As Button
    Friend WithEvents tpPersonal As TabPage
    Friend WithEvents btnGestionarIncidencias As Button
    Friend WithEvents btnTiposEstadoTransitorio As Button
    Friend WithEvents btnCategoriasAusencia As Button
End Class