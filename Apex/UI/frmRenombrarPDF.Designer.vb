<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmRenombrarPDF
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRenombrarPDF))
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.SplitContainerLeft = New System.Windows.Forms.SplitContainer()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.PanelBusquedaArchivos = New System.Windows.Forms.Panel()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.btnSeleccionarCarpeta = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.dgvTiposNomenclaturas = New System.Windows.Forms.DataGridView()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.txtFecha = New System.Windows.Forms.DateTimePicker()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.PanelVisorPDF = New System.Windows.Forms.Panel()
        Me.AxAcroPDF1 = New AxAcroPDFLib.AxAcroPDF()
        Me.lblNombreArchivo = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtNombreModificado = New System.Windows.Forms.TextBox()
        Me.btnRenombrar = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtCedula = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnBuscarFuncionario = New System.Windows.Forms.Button()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        CType(Me.SplitContainerLeft, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainerLeft.Panel1.SuspendLayout()
        Me.SplitContainerLeft.Panel2.SuspendLayout()
        Me.SplitContainerLeft.SuspendLayout()
        Me.PanelBusquedaArchivos.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.dgvTiposNomenclaturas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelVisorPDF.SuspendLayout()
        CType(Me.AxAcroPDF1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Timer1
        '
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.SplitContainerLeft)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.PanelVisorPDF)
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox1)
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(5)
        Me.SplitContainer1.Size = New System.Drawing.Size(1264, 741)
        Me.SplitContainer1.SplitterDistance = 415
        Me.SplitContainer1.TabIndex = 22
        '
        'SplitContainerLeft
        '
        Me.SplitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainerLeft.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainerLeft.Name = "SplitContainerLeft"
        Me.SplitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerLeft.Panel1
        '
        Me.SplitContainerLeft.Panel1.Controls.Add(Me.ListBox1)
        Me.SplitContainerLeft.Panel1.Controls.Add(Me.PanelBusquedaArchivos)
        Me.SplitContainerLeft.Panel1.Padding = New System.Windows.Forms.Padding(5)
        '
        'SplitContainerLeft.Panel2
        '
        Me.SplitContainerLeft.Panel2.Controls.Add(Me.GroupBox4)
        Me.SplitContainerLeft.Panel2.Padding = New System.Windows.Forms.Padding(5)
        Me.SplitContainerLeft.Size = New System.Drawing.Size(415, 741)
        Me.SplitContainerLeft.SplitterDistance = 480
        Me.SplitContainerLeft.TabIndex = 0
        '
        'ListBox1
        '
        Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(5, 172)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(405, 303)
        Me.ListBox1.TabIndex = 1
        '
        'PanelBusquedaArchivos
        '
        Me.PanelBusquedaArchivos.Controls.Add(Me.lblTotal)
        Me.PanelBusquedaArchivos.Controls.Add(Me.GroupBox3)
        Me.PanelBusquedaArchivos.Controls.Add(Me.btnSeleccionarCarpeta)
        Me.PanelBusquedaArchivos.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelBusquedaArchivos.Location = New System.Drawing.Point(5, 5)
        Me.PanelBusquedaArchivos.Name = "PanelBusquedaArchivos"
        Me.PanelBusquedaArchivos.Size = New System.Drawing.Size(405, 167)
        Me.PanelBusquedaArchivos.TabIndex = 2
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(4, 143)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(398, 13)
        Me.lblTotal.TabIndex = 21
        Me.lblTotal.Text = "Total Archivos PDF: 0"
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.TextBox1)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Controls.Add(Me.TextBox2)
        Me.GroupBox3.Location = New System.Drawing.Point(3, 49)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(399, 82)
        Me.GroupBox3.TabIndex = 20
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Buscar en lista"
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(82, 23)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(311, 20)
        Me.TextBox1.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Contiene 1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(16, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Contiene 2"
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.Location = New System.Drawing.Point(82, 49)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(311, 20)
        Me.TextBox2.TabIndex = 17
        '
        'btnSeleccionarCarpeta
        '
        Me.btnSeleccionarCarpeta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeleccionarCarpeta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSeleccionarCarpeta.Location = New System.Drawing.Point(3, 3)
        Me.btnSeleccionarCarpeta.Name = "btnSeleccionarCarpeta"
        Me.btnSeleccionarCarpeta.Size = New System.Drawing.Size(399, 40)
        Me.btnSeleccionarCarpeta.TabIndex = 0
        Me.btnSeleccionarCarpeta.Text = "📂 Seleccionar Carpeta"
        Me.btnSeleccionarCarpeta.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.dgvTiposNomenclaturas)
        Me.GroupBox4.Controls.Add(Me.txtCodigo)
        Me.GroupBox4.Controls.Add(Me.txtFecha)
        Me.GroupBox4.Controls.Add(Me.CheckBox1)
        Me.GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox4.Location = New System.Drawing.Point(5, 5)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(405, 247)
        Me.GroupBox4.TabIndex = 14
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Nomenclatura"
        '
        'dgvTiposNomenclaturas
        '
        Me.dgvTiposNomenclaturas.AllowUserToAddRows = False
        Me.dgvTiposNomenclaturas.AllowUserToDeleteRows = False
        Me.dgvTiposNomenclaturas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvTiposNomenclaturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTiposNomenclaturas.Location = New System.Drawing.Point(6, 19)
        Me.dgvTiposNomenclaturas.Name = "dgvTiposNomenclaturas"
        Me.dgvTiposNomenclaturas.ReadOnly = True
        Me.dgvTiposNomenclaturas.RowHeadersVisible = False
        Me.dgvTiposNomenclaturas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTiposNomenclaturas.Size = New System.Drawing.Size(393, 161)
        Me.dgvTiposNomenclaturas.TabIndex = 1
        '
        'txtCodigo
        '
        Me.txtCodigo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCodigo.Location = New System.Drawing.Point(171, 212)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(228, 20)
        Me.txtCodigo.TabIndex = 9
        '
        'txtFecha
        '
        Me.txtFecha.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtFecha.Location = New System.Drawing.Point(171, 186)
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(228, 20)
        Me.txtFecha.TabIndex = 10
        '
        'CheckBox1
        '
        Me.CheckBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(6, 186)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(122, 17)
        Me.CheckBox1.TabIndex = 11
        Me.CheckBox1.Text = "Es una Notificación"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'PanelVisorPDF
        '
        Me.PanelVisorPDF.Controls.Add(Me.AxAcroPDF1)
        Me.PanelVisorPDF.Controls.Add(Me.lblNombreArchivo)
        Me.PanelVisorPDF.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelVisorPDF.Location = New System.Drawing.Point(5, 5)
        Me.PanelVisorPDF.Name = "PanelVisorPDF"
        Me.PanelVisorPDF.Size = New System.Drawing.Size(835, 510)
        Me.PanelVisorPDF.TabIndex = 1
        '
        'AxAcroPDF1
        '
        Me.AxAcroPDF1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AxAcroPDF1.Enabled = True
        Me.AxAcroPDF1.Location = New System.Drawing.Point(0, 0)
        Me.AxAcroPDF1.Name = "AxAcroPDF1"
        Me.AxAcroPDF1.OcxState = CType(resources.GetObject("AxAcroPDF1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxAcroPDF1.Size = New System.Drawing.Size(835, 490)
        Me.AxAcroPDF1.TabIndex = 14
        '
        'lblNombreArchivo
        '
        Me.lblNombreArchivo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblNombreArchivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNombreArchivo.Location = New System.Drawing.Point(0, 490)
        Me.lblNombreArchivo.Name = "lblNombreArchivo"
        Me.lblNombreArchivo.Size = New System.Drawing.Size(835, 20)
        Me.lblNombreArchivo.TabIndex = 15
        Me.lblNombreArchivo.Text = "Archivo: Ninguno"
        Me.lblNombreArchivo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtNombreModificado)
        Me.GroupBox1.Controls.Add(Me.btnRenombrar)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox1.Location = New System.Drawing.Point(5, 515)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(10)
        Me.GroupBox1.Size = New System.Drawing.Size(835, 221)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Opciones de Renombrado"
        '
        'txtNombreModificado
        '
        Me.txtNombreModificado.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreModificado.BackColor = System.Drawing.SystemColors.Info
        Me.txtNombreModificado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombreModificado.Location = New System.Drawing.Point(13, 133)
        Me.txtNombreModificado.Name = "txtNombreModificado"
        Me.txtNombreModificado.ReadOnly = True
        Me.txtNombreModificado.Size = New System.Drawing.Size(809, 22)
        Me.txtNombreModificado.TabIndex = 15
        Me.txtNombreModificado.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnRenombrar
        '
        Me.btnRenombrar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRenombrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRenombrar.Location = New System.Drawing.Point(13, 164)
        Me.btnRenombrar.Name = "btnRenombrar"
        Me.btnRenombrar.Size = New System.Drawing.Size(809, 47)
        Me.btnRenombrar.TabIndex = 14
        Me.btnRenombrar.Text = "✔️ Renombrar Archivo Seleccionado"
        Me.btnRenombrar.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.txtCedula)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txtNombre)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.btnBuscarFuncionario)
        Me.GroupBox2.Location = New System.Drawing.Point(13, 26)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(809, 90)
        Me.GroupBox2.TabIndex = 12
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Funcionario"
        '
        'txtCedula
        '
        Me.txtCedula.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCedula.Location = New System.Drawing.Point(594, 46)
        Me.txtCedula.Name = "txtCedula"
        Me.txtCedula.ReadOnly = True
        Me.txtCedula.Size = New System.Drawing.Size(209, 20)
        Me.txtCedula.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(591, 26)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Cédula"
        '
        'txtNombre
        '
        Me.txtNombre.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombre.Location = New System.Drawing.Point(198, 46)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.ReadOnly = True
        Me.txtNombre.Size = New System.Drawing.Size(390, 20)
        Me.txtNombre.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(195, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(108, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Nombre del associado"
        '
        'btnBuscarFuncionario
        '
        Me.btnBuscarFuncionario.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBuscarFuncionario.Location = New System.Drawing.Point(6, 19)
        Me.btnBuscarFuncionario.Name = "btnBuscarFuncionario"
        Me.btnBuscarFuncionario.Size = New System.Drawing.Size(186, 65)
        Me.btnBuscarFuncionario.TabIndex = 4
        Me.btnBuscarFuncionario.Text = "👤 Buscar y Asociar Funcionario"
        Me.btnBuscarFuncionario.UseVisualStyleBackColor = True
        '
        'frmRenombrarPDF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1264, 741)
        Me.Controls.Add(Me.SplitContainer1)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(960, 600)
        Me.Name = "frmRenombrarPDF"
        Me.Text = "Renombrado Inteligente de Archivos PDF"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.SplitContainerLeft.Panel1.ResumeLayout(False)
        Me.SplitContainerLeft.Panel2.ResumeLayout(False)
        CType(Me.SplitContainerLeft, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainerLeft.ResumeLayout(False)
        Me.PanelBusquedaArchivos.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.dgvTiposNomenclaturas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelVisorPDF.ResumeLayout(False)
        CType(Me.AxAcroPDF1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents Timer1 As Timer
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents PanelBusquedaArchivos As Panel
    Friend WithEvents btnSeleccionarCarpeta As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents lblTotal As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btnBuscarFuncionario As Button
    Friend WithEvents txtCedula As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents dgvTiposNomenclaturas As DataGridView
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents txtFecha As DateTimePicker
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents txtNombreModificado As TextBox
    Friend WithEvents btnRenombrar As Button
    Friend WithEvents SplitContainerLeft As SplitContainer
    Friend WithEvents PanelVisorPDF As Panel
    Friend WithEvents AxAcroPDF1 As AxAcroPDFLib.AxAcroPDF
    Friend WithEvents lblNombreArchivo As Label
End Class