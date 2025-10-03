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
        Me.btnAnalisisFuncionarios = New System.Windows.Forms.Button()
        Me.btnAnalisisEstacional = New System.Windows.Forms.Button()
        Me.btnFuncionariosGenero = New System.Windows.Forms.Button()
        Me.btnFuncionariosEdad = New System.Windows.Forms.Button()
        Me.btnFuncionariosArea = New System.Windows.Forms.Button()
        Me.btnFuncionariosCargo = New System.Windows.Forms.Button()
        Me.btnLicenciasPorTipo = New System.Windows.Forms.Button()
        Me.btnLicenciasPorEstado = New System.Windows.Forms.Button()
        Me.btnLicenciasTopFuncionarios = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnAnalisisFuncionarios
        '
        Me.btnAnalisisFuncionarios.Location = New System.Drawing.Point(4, 50)
        Me.btnAnalisisFuncionarios.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAnalisisFuncionarios.Name = "btnAnalisisFuncionarios"
        Me.btnAnalisisFuncionarios.Size = New System.Drawing.Size(330, 35)
        Me.btnAnalisisFuncionarios.TabIndex = 2
        Me.btnAnalisisFuncionarios.Text = "👥 Análisis de Personal"
        Me.btnAnalisisFuncionarios.UseVisualStyleBackColor = True
        '
        'btnAnalisisEstacional
        '
        Me.btnAnalisisEstacional.Location = New System.Drawing.Point(4, 5)
        Me.btnAnalisisEstacional.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnAnalisisEstacional.Name = "btnAnalisisEstacional"
        Me.btnAnalisisEstacional.Size = New System.Drawing.Size(330, 35)
        Me.btnAnalisisEstacional.TabIndex = 0
        Me.btnAnalisisEstacional.Text = "   📊 Análisis de Licencias"
        Me.btnAnalisisEstacional.UseVisualStyleBackColor = True
        '
        'btnFuncionariosGenero
        '
        Me.btnFuncionariosGenero.Location = New System.Drawing.Point(4, 95)
        Me.btnFuncionariosGenero.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosGenero.Name = "btnFuncionariosGenero"
        Me.btnFuncionariosGenero.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosGenero.TabIndex = 3
        Me.btnFuncionariosGenero.Text = "♀️ Distribución por Género"
        Me.btnFuncionariosGenero.UseVisualStyleBackColor = True
        '
        'btnFuncionariosEdad
        '
        Me.btnFuncionariosEdad.Location = New System.Drawing.Point(4, 140)
        Me.btnFuncionariosEdad.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosEdad.Name = "btnFuncionariosEdad"
        Me.btnFuncionariosEdad.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosEdad.TabIndex = 4
        Me.btnFuncionariosEdad.Text = "🎂 Distribución por Edad"
        Me.btnFuncionariosEdad.UseVisualStyleBackColor = True
        '
        'btnFuncionariosArea
        '
        Me.btnFuncionariosArea.Location = New System.Drawing.Point(4, 185)
        Me.btnFuncionariosArea.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosArea.Name = "btnFuncionariosArea"
        Me.btnFuncionariosArea.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosArea.TabIndex = 5
        Me.btnFuncionariosArea.Text = "🏢 Distribución por Área"
        Me.btnFuncionariosArea.UseVisualStyleBackColor = True
        '
        'btnFuncionariosCargo
        '
        Me.btnFuncionariosCargo.Location = New System.Drawing.Point(4, 230)
        Me.btnFuncionariosCargo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionariosCargo.Name = "btnFuncionariosCargo"
        Me.btnFuncionariosCargo.Size = New System.Drawing.Size(330, 35)
        Me.btnFuncionariosCargo.TabIndex = 6
        Me.btnFuncionariosCargo.Text = "🧭 Top Cargos con más Personal"
        Me.btnFuncionariosCargo.UseVisualStyleBackColor = True
        '
        'btnLicenciasPorTipo
        '
        Me.btnLicenciasPorTipo.Location = New System.Drawing.Point(4, 275)
        Me.btnLicenciasPorTipo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnLicenciasPorTipo.Name = "btnLicenciasPorTipo"
        Me.btnLicenciasPorTipo.Size = New System.Drawing.Size(330, 35)
        Me.btnLicenciasPorTipo.TabIndex = 7
        Me.btnLicenciasPorTipo.Text = "🗂️ Licencias por Tipo"
        Me.btnLicenciasPorTipo.UseVisualStyleBackColor = True
        '
        'btnLicenciasPorEstado
        '
        Me.btnLicenciasPorEstado.Location = New System.Drawing.Point(4, 320)
        Me.btnLicenciasPorEstado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnLicenciasPorEstado.Name = "btnLicenciasPorEstado"
        Me.btnLicenciasPorEstado.Size = New System.Drawing.Size(330, 35)
        Me.btnLicenciasPorEstado.TabIndex = 8
        Me.btnLicenciasPorEstado.Text = "📑 Licencias por Estado"
        Me.btnLicenciasPorEstado.UseVisualStyleBackColor = True
        '
        'btnLicenciasTopFuncionarios
        '
        Me.btnLicenciasTopFuncionarios.Location = New System.Drawing.Point(4, 365)
        Me.btnLicenciasTopFuncionarios.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnLicenciasTopFuncionarios.Name = "btnLicenciasTopFuncionarios"
        Me.btnLicenciasTopFuncionarios.Size = New System.Drawing.Size(330, 35)
        Me.btnLicenciasTopFuncionarios.TabIndex = 9
        Me.btnLicenciasTopFuncionarios.Text = "🏅 Funcionarios con más Licencias"
        Me.btnLicenciasTopFuncionarios.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoScroll = True
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAnalisisEstacional)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnAnalisisFuncionarios)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosGenero)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosEdad)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosArea)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnFuncionariosCargo)
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
    Friend WithEvents btnAnalisisFuncionarios As Button
    Friend WithEvents btnAnalisisEstacional As Button
    Friend WithEvents btnFuncionariosGenero As Button
    Friend WithEvents btnFuncionariosEdad As Button
    Friend WithEvents btnFuncionariosArea As Button
    Friend WithEvents btnFuncionariosCargo As Button
    Friend WithEvents btnLicenciasPorTipo As Button
    Friend WithEvents btnLicenciasPorEstado As Button
    Friend WithEvents btnLicenciasTopFuncionarios As Button
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
End Class