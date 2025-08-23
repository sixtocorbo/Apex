<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDashboard
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
        Me.panelNavegacion = New System.Windows.Forms.Panel()
        Me.btnConfiguracion = New System.Windows.Forms.Button()
        Me.btnReportes = New System.Windows.Forms.Button()
        Me.btnViaticos = New System.Windows.Forms.Button()
        Me.btnImportacion = New System.Windows.Forms.Button()
        Me.btnRenombrarPDFs = New System.Windows.Forms.Button()
        Me.btnNomenclaturas = New System.Windows.Forms.Button()
        Me.btnGestion = New System.Windows.Forms.Button()
        Me.btnNovedades = New System.Windows.Forms.Button()
        Me.btnFiltros = New System.Windows.Forms.Button()
        Me.btnFuncionarios = New System.Windows.Forms.Button()
        Me.panelLogo = New System.Windows.Forms.Panel()
        Me.lblSemanaActual = New System.Windows.Forms.Label()
        Me.lblAppName = New System.Windows.Forms.Label()
        Me.panelContenido = New System.Windows.Forms.Panel()
        Me.panelNavegacion.SuspendLayout()
        Me.panelLogo.SuspendLayout()
        Me.SuspendLayout()
        '
        'panelNavegacion
        '
        Me.panelNavegacion.AutoScroll = True
        Me.panelNavegacion.BackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(51, Byte), Integer), CType(CType(76, Byte), Integer))
        Me.panelNavegacion.Controls.Add(Me.btnConfiguracion)
        Me.panelNavegacion.Controls.Add(Me.btnReportes)
        Me.panelNavegacion.Controls.Add(Me.btnViaticos)
        Me.panelNavegacion.Controls.Add(Me.btnImportacion)
        Me.panelNavegacion.Controls.Add(Me.btnRenombrarPDFs)
        Me.panelNavegacion.Controls.Add(Me.btnNomenclaturas)
        Me.panelNavegacion.Controls.Add(Me.btnGestion)
        Me.panelNavegacion.Controls.Add(Me.btnNovedades)
        Me.panelNavegacion.Controls.Add(Me.btnFiltros)
        Me.panelNavegacion.Controls.Add(Me.btnFuncionarios)
        Me.panelNavegacion.Controls.Add(Me.panelLogo)
        Me.panelNavegacion.Dock = System.Windows.Forms.DockStyle.Left
        Me.panelNavegacion.Location = New System.Drawing.Point(0, 0)
        Me.panelNavegacion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.panelNavegacion.Name = "panelNavegacion"
        Me.panelNavegacion.Size = New System.Drawing.Size(330, 1048)
        Me.panelNavegacion.TabIndex = 0
        '
        'btnConfiguracion
        '
        Me.btnConfiguracion.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnConfiguracion.FlatAppearance.BorderSize = 0
        Me.btnConfiguracion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnConfiguracion.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnConfiguracion.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnConfiguracion.Location = New System.Drawing.Point(0, 950)
        Me.btnConfiguracion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnConfiguracion.Name = "btnConfiguracion"
        Me.btnConfiguracion.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnConfiguracion.Size = New System.Drawing.Size(330, 92)
        Me.btnConfiguracion.TabIndex = 4
        Me.btnConfiguracion.Text = "   ⚙️ Configuración"
        Me.btnConfiguracion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnConfiguracion.UseVisualStyleBackColor = True
        '
        'btnReportes
        '
        Me.btnReportes.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnReportes.FlatAppearance.BorderSize = 0
        Me.btnReportes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnReportes.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnReportes.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnReportes.Location = New System.Drawing.Point(0, 858)
        Me.btnReportes.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnReportes.Name = "btnReportes"
        Me.btnReportes.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnReportes.Size = New System.Drawing.Size(330, 92)
        Me.btnReportes.TabIndex = 3
        Me.btnReportes.Text = "   📈 Reportes"
        Me.btnReportes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReportes.UseVisualStyleBackColor = True
        '
        'btnViaticos
        '
        Me.btnViaticos.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnViaticos.FlatAppearance.BorderSize = 0
        Me.btnViaticos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnViaticos.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnViaticos.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnViaticos.Location = New System.Drawing.Point(0, 766)
        Me.btnViaticos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnViaticos.Name = "btnViaticos"
        Me.btnViaticos.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnViaticos.Size = New System.Drawing.Size(330, 92)
        Me.btnViaticos.TabIndex = 8
        Me.btnViaticos.Text = "   💵 Viáticos"
        Me.btnViaticos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnViaticos.UseVisualStyleBackColor = True
        '
        'btnImportacion
        '
        Me.btnImportacion.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnImportacion.FlatAppearance.BorderSize = 0
        Me.btnImportacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnImportacion.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnImportacion.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnImportacion.Location = New System.Drawing.Point(0, 674)
        Me.btnImportacion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnImportacion.Name = "btnImportacion"
        Me.btnImportacion.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnImportacion.Size = New System.Drawing.Size(330, 92)
        Me.btnImportacion.TabIndex = 9
        Me.btnImportacion.Text = "   📥 Importar"
        Me.btnImportacion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnImportacion.UseVisualStyleBackColor = True
        '
        'btnRenombrarPDFs
        '
        Me.btnRenombrarPDFs.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnRenombrarPDFs.FlatAppearance.BorderSize = 0
        Me.btnRenombrarPDFs.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnRenombrarPDFs.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRenombrarPDFs.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnRenombrarPDFs.Location = New System.Drawing.Point(0, 582)
        Me.btnRenombrarPDFs.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnRenombrarPDFs.Name = "btnRenombrarPDFs"
        Me.btnRenombrarPDFs.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnRenombrarPDFs.Size = New System.Drawing.Size(330, 92)
        Me.btnRenombrarPDFs.TabIndex = 11
        Me.btnRenombrarPDFs.Text = "   📄 Renombrar PDFs"
        Me.btnRenombrarPDFs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnRenombrarPDFs.UseVisualStyleBackColor = True
        '
        'btnNomenclaturas
        '
        Me.btnNomenclaturas.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnNomenclaturas.FlatAppearance.BorderSize = 0
        Me.btnNomenclaturas.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNomenclaturas.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNomenclaturas.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnNomenclaturas.Location = New System.Drawing.Point(0, 490)
        Me.btnNomenclaturas.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNomenclaturas.Name = "btnNomenclaturas"
        Me.btnNomenclaturas.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnNomenclaturas.Size = New System.Drawing.Size(330, 92)
        Me.btnNomenclaturas.TabIndex = 10
        Me.btnNomenclaturas.Text = "   🗂️ Nomenclaturas"
        Me.btnNomenclaturas.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNomenclaturas.UseVisualStyleBackColor = True
        '
        'btnGestion
        '
        Me.btnGestion.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnGestion.FlatAppearance.BorderSize = 0
        Me.btnGestion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGestion.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGestion.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnGestion.Location = New System.Drawing.Point(0, 398)
        Me.btnGestion.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGestion.Name = "btnGestion"
        Me.btnGestion.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnGestion.Size = New System.Drawing.Size(330, 92)
        Me.btnGestion.TabIndex = 6
        Me.btnGestion.Text = "   📝 Gestión"
        Me.btnGestion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGestion.UseVisualStyleBackColor = True
        '
        'btnNovedades
        '
        Me.btnNovedades.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnNovedades.FlatAppearance.BorderSize = 0
        Me.btnNovedades.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnNovedades.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnNovedades.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnNovedades.Location = New System.Drawing.Point(0, 306)
        Me.btnNovedades.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnNovedades.Name = "btnNovedades"
        Me.btnNovedades.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnNovedades.Size = New System.Drawing.Size(330, 92)
        Me.btnNovedades.TabIndex = 7
        Me.btnNovedades.Text = "   📰 Novedades"
        Me.btnNovedades.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnNovedades.UseVisualStyleBackColor = True
        '
        'btnFiltros
        '
        Me.btnFiltros.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnFiltros.FlatAppearance.BorderSize = 0
        Me.btnFiltros.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFiltros.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFiltros.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnFiltros.Location = New System.Drawing.Point(0, 214)
        Me.btnFiltros.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFiltros.Name = "btnFiltros"
        Me.btnFiltros.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnFiltros.Size = New System.Drawing.Size(330, 92)
        Me.btnFiltros.TabIndex = 5
        Me.btnFiltros.Text = "   🔍 Filtros"
        Me.btnFiltros.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFiltros.UseVisualStyleBackColor = True
        '
        'btnFuncionarios
        '
        Me.btnFuncionarios.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnFuncionarios.FlatAppearance.BorderSize = 0
        Me.btnFuncionarios.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFuncionarios.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnFuncionarios.ForeColor = System.Drawing.Color.Gainsboro
        Me.btnFuncionarios.Location = New System.Drawing.Point(0, 122)
        Me.btnFuncionarios.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnFuncionarios.Name = "btnFuncionarios"
        Me.btnFuncionarios.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
        Me.btnFuncionarios.Size = New System.Drawing.Size(330, 92)
        Me.btnFuncionarios.TabIndex = 1
        Me.btnFuncionarios.Text = "   👤 Funcionarios"
        Me.btnFuncionarios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFuncionarios.UseVisualStyleBackColor = True
        '
        'panelLogo
        '
        Me.panelLogo.BackColor = System.Drawing.Color.FromArgb(CType(CType(39, Byte), Integer), CType(CType(39, Byte), Integer), CType(CType(58, Byte), Integer))
        Me.panelLogo.Controls.Add(Me.lblSemanaActual)
        Me.panelLogo.Controls.Add(Me.lblAppName)
        Me.panelLogo.Dock = System.Windows.Forms.DockStyle.Top
        Me.panelLogo.Location = New System.Drawing.Point(0, 0)
        Me.panelLogo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.panelLogo.Name = "panelLogo"
        Me.panelLogo.Size = New System.Drawing.Size(330, 122)
        Me.panelLogo.TabIndex = 0
        '
        'lblSemanaActual
        '
        Me.lblSemanaActual.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblSemanaActual.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSemanaActual.ForeColor = System.Drawing.Color.Gold
        Me.lblSemanaActual.Location = New System.Drawing.Point(0, 80)
        Me.lblSemanaActual.Name = "lblSemanaActual"
        Me.lblSemanaActual.Size = New System.Drawing.Size(330, 42)
        Me.lblSemanaActual.TabIndex = 1
        Me.lblSemanaActual.Text = "Semana: -"
        Me.lblSemanaActual.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblAppName
        '
        Me.lblAppName.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblAppName.Font = New System.Drawing.Font("Segoe UI Semibold", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAppName.ForeColor = System.Drawing.Color.White
        Me.lblAppName.Location = New System.Drawing.Point(0, 0)
        Me.lblAppName.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblAppName.Name = "lblAppName"
        Me.lblAppName.Size = New System.Drawing.Size(330, 80)
        Me.lblAppName.TabIndex = 0
        Me.lblAppName.Text = "APEX"
        Me.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'panelContenido
        '
        Me.panelContenido.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelContenido.Location = New System.Drawing.Point(330, 0)
        Me.panelContenido.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.panelContenido.Name = "panelContenido"
        Me.panelContenido.Size = New System.Drawing.Size(1566, 1048)
        Me.panelContenido.TabIndex = 1
        '
        'frmDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1896, 1048)
        Me.Controls.Add(Me.panelContenido)
        Me.Controls.Add(Me.panelNavegacion)
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(1430, 896)
        Me.Name = "frmDashboard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sistema de Gestión Apex"
        Me.panelNavegacion.ResumeLayout(False)
        Me.panelLogo.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents panelNavegacion As Panel
    Friend WithEvents panelLogo As Panel
    Friend WithEvents btnFuncionarios As Button
    Friend WithEvents btnReportes As Button
    Friend WithEvents btnConfiguracion As Button
    Friend WithEvents lblAppName As Label
    Friend WithEvents panelContenido As Panel
    Friend WithEvents btnFiltros As Button
    Friend WithEvents btnGestion As Button
    Friend WithEvents btnNovedades As Button
    Friend WithEvents btnViaticos As Button
    Friend WithEvents btnImportacion As Button
    Friend WithEvents lblSemanaActual As Label
    Friend WithEvents btnNomenclaturas As Button
    Friend WithEvents btnRenombrarPDFs As Button ' --> DECLARACIÓN DEL BOTÓN
End Class