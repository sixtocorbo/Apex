<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmReportes
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
        Me.btnResumenCantidades = New System.Windows.Forms.Button()
        Me.btnAnalisisEstacional = New System.Windows.Forms.Button()
        Me.btnFuncionariosGenero = New System.Windows.Forms.Button()
        Me.btnFuncionariosEdad = New System.Windows.Forms.Button()
        Me.btnFuncionariosArea = New System.Windows.Forms.Button()
        Me.btnFuncionariosCargo = New System.Windows.Forms.Button()
        Me.btnFuncionariosEstado = New System.Windows.Forms.Button()
        Me.btnFuncionariosTurno = New System.Windows.Forms.Button()
        Me.btnFuncionariosNivelEstudio = New System.Windows.Forms.Button()
        Me.btnLicenciasPorTipo = New System.Windows.Forms.Button()
        Me.btnLicenciasPorEstado = New System.Windows.Forms.Button()
        Me.btnLicenciasTopFuncionarios = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnResumenCantidades
        '
        Me.btnResumenCantidades.Location = New System.Drawing.Point(4, 5)
        Me.btnResumenCantidades.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnResumenCantidades.Name = "btnResumenCantidades"
        Me.btnResumenCantidades.Size = New System.Drawing.Size(330, 35)
        Me.btnResumenCantidades.TabIndex = 0
        Me.btnResumenCantidades.Text = "📋 Resumen de Cantidades"
        Me.btnResumenCantidades.UseVisualStyleBackColor = True
        '
        'btnAnalisisEstacional
        '
        Me.btnAnalisisEstacional.Location = New System.Drawing.Point(4, 50)
        Me.btnAnalisisEstacional.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAnalisisEstacional.Name = "btnAnalisisEstacional"
        Me.btnAnalisisEstacional.Size = New System.Drawing.Size(330, 35)
        Me.btnAnalisisEstacional.TabIndex = 1
        Me.btnAnalisisEstacional.Text = "   📊 Análisis de Licencias"
        Me.btnAnalisisEstacional.UseVisualStyleBackColor = True
        '
        'btnFuncionariosGenero
        '
        Me.btnFuncionariosGenero.Location = New System.Drawing.Point(4, 95)
        Me.btnFuncionariosGenero.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosGenero.Name = "btnFuncionariosGenero"
        Me.btnFuncionariosGenero.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosGenero.TabIndex = 2
        Me.btnFuncionariosGenero.Text = "♀️ Distribución por Género"
        Me.btnFuncionariosGenero.UseVisualStyleBackColor = True
        '
        'btnFuncionariosEdad
        '
        Me.btnFuncionariosEdad.Location = New System.Drawing.Point(4, 140)
        Me.btnFuncionariosEdad.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosEdad.Name = "btnFuncionariosEdad"
        Me.btnFuncionariosEdad.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosEdad.TabIndex = 3
        Me.btnFuncionariosEdad.Text = "🎂 Distribución por Edad"
        Me.btnFuncionariosEdad.UseVisualStyleBackColor = True
        '
        'btnFuncionariosArea
        '
        Me.btnFuncionariosArea.Location = New System.Drawing.Point(4, 185)
        Me.btnFuncionariosArea.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosArea.Name = "btnFuncionariosArea"
        Me.btnFuncionariosArea.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosArea.TabIndex = 4
        Me.btnFuncionariosArea.Text = "🏢 Distribución por Área"
        Me.btnFuncionariosArea.UseVisualStyleBackColor = True
        '
        'btnFuncionariosCargo
        '
        Me.btnFuncionariosCargo.Location = New System.Drawing.Point(4, 230)
        Me.btnFuncionariosCargo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosCargo.Name = "btnFuncionariosCargo"
        Me.btnFuncionariosCargo.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosCargo.TabIndex = 5
        Me.btnFuncionariosCargo.Text = "🧭 Top Cargos con más Personal"
        Me.btnFuncionariosCargo.UseVisualStyleBackColor = True
        '
        'btnFuncionariosEstado
        '
        Me.btnFuncionariosEstado.Location = New System.Drawing.Point(4, 275)
        Me.btnFuncionariosEstado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosEstado.Name = "btnFuncionariosEstado"
        Me.btnFuncionariosEstado.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosEstado.TabIndex = 6
        Me.btnFuncionariosEstado.Text = "🟢 Activos vs. Inactivos"
        Me.btnFuncionariosEstado.UseVisualStyleBackColor = True
        '
        'btnFuncionariosTurno
        '
        Me.btnFuncionariosTurno.Location = New System.Drawing.Point(4, 320)
        Me.btnFuncionariosTurno.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosTurno.Name = "btnFuncionariosTurno"
        Me.btnFuncionariosTurno.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosTurno.TabIndex = 7
        Me.btnFuncionariosTurno.Text = "⏰ Distribución por Turno"
        Me.btnFuncionariosTurno.UseVisualStyleBackColor = True
        '
        'btnFuncionariosNivelEstudio
        '
        Me.btnFuncionariosNivelEstudio.Location = New System.Drawing.Point(4, 365)
        Me.btnFuncionariosNivelEstudio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosNivelEstudio.Name = "btnFuncionariosNivelEstudio"
        Me.btnFuncionariosNivelEstudio.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosNivelEstudio.TabIndex = 8
        Me.btnFuncionariosNivelEstudio.Text = "🎓 Nivel de Estudios"
        Me.btnFuncionariosNivelEstudio.UseVisualStyleBackColor = True
        '
        'btnLicenciasPorTipo
        '
        Me.btnLicenciasPorTipo.Location = New System.Drawing.Point(4, 410)
        Me.btnLicenciasPorTipo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnLicenciasPorTipo.Name = "btnLicenciasPorTipo"
        Me.btnLicenciasPorTipo.Size = New System.Drawing.Size(330, 35)
        Me.btnLicenciasPorTipo.TabIndex = 9
        Me.btnLicenciasPorTipo.Text = "🗂️ Licencias por Tipo"
        Me.btnLicenciasPorTipo.UseVisualStyleBackColor = True
        '
        'btnLicenciasPorEstado
        '
        Me.btnLicenciasPorEstado.Location = New System.Drawing.Point(4, 455)
        Me.btnLicenciasPorEstado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnLicenciasPorEstado.Name = "btnLicenciasPorEstado"
        Me.btnLicenciasPorEstado.Size = New System.Drawing.Size(330, 35)
        Me.btnLicenciasPorEstado.TabIndex = 10
        Me.btnLicenciasPorEstado.Text = "📑 Licencias por Estado"
        Me.btnLicenciasPorEstado.UseVisualStyleBackColor = True
        '
        'btnLicenciasTopFuncionarios
        '
        Me.btnLicenciasTopFuncionarios.Location = New System.Drawing.Point(4, 500)
        Me.btnLicenciasTopFuncionarios.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnLicenciasTopFuncionarios.Name = "btnLicenciasTopFuncionarios"
        Me.btnLicenciasTopFuncionarios.Size = New System.Drawing.Size(330, 35)
        Me.btnLicenciasTopFuncionarios.TabIndex = 11
        Me.btnLicenciasTopFuncionarios.Text = "🏅 Funcionarios con más Licencias"
        Me.btnLicenciasTopFuncionarios.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoScroll = True
        Me.FlowLayoutPanel1.Controls.Add(Me.btnResumenCantidades)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAnalisisEstacional)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosGenero)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosEdad)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosArea)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosCargo)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosEstado)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosTurno)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosNivelEstudio)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnLicenciasPorTipo)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnLicenciasPorEstado)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnLicenciasTopFuncionarios)
        Me.FlowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Padding = New System.Windows.Forms.Padding(0, 0, 10, 10)
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(717, 375)
        Me.FlowLayoutPanel1.TabIndex = 8
        Me.FlowLayoutPanel1.WrapContents = False
        '
        'frmReportes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 375)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(739, 431)
        Me.Name = "frmReportes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Reportes"
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnAnalisisEstacional As Button
    Friend WithEvents btnFuncionariosGenero As Button
    Friend WithEvents btnFuncionariosEdad As Button
    Friend WithEvents btnFuncionariosArea As Button
    Friend WithEvents btnFuncionariosCargo As Button
    Friend WithEvents btnLicenciasPorTipo As Button
    Friend WithEvents btnLicenciasPorEstado As Button
    Friend WithEvents btnLicenciasTopFuncionarios As Button
    Friend WithEvents btnFuncionariosEstado As Button
    Friend WithEvents btnFuncionariosTurno As Button
    Friend WithEvents btnFuncionariosNivelEstudio As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnResumenCantidades As Button
End Class