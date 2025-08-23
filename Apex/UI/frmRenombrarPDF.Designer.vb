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
        Me.FlowLayoutPanelLeft = New System.Windows.Forms.FlowLayoutPanel()
        Me.PanelBusquedaArchivos = New System.Windows.Forms.Panel()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.btnSeleccionarCarpeta = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.dgvTiposNomenclaturas = New System.Windows.Forms.DataGridView()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.txtFecha = New System.Windows.Forms.DateTimePicker()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.FlowLayoutPanelRight = New System.Windows.Forms.FlowLayoutPanel()
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
        Me.FlowLayoutPanelLeft.SuspendLayout()
        Me.PanelBusquedaArchivos.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.dgvTiposNomenclaturas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FlowLayoutPanelRight.SuspendLayout()
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
        Me.SplitContainer1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.FlowLayoutPanelLeft)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.FlowLayoutPanelRight)
        Me.SplitContainer1.Size = New System.Drawing.Size(1356, 762)
        Me.SplitContainer1.SplitterDistance = 425
        Me.SplitContainer1.SplitterWidth = 6
        Me.SplitContainer1.TabIndex = 0
        '
        'FlowLayoutPanelLeft
        '
        Me.FlowLayoutPanelLeft.AutoScroll = True
        Me.FlowLayoutPanelLeft.Controls.Add(Me.PanelBusquedaArchivos)
        Me.FlowLayoutPanelLeft.Controls.Add(Me.ListBox1)
        Me.FlowLayoutPanelLeft.Controls.Add(Me.GroupBox4)
        Me.FlowLayoutPanelLeft.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanelLeft.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanelLeft.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanelLeft.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.FlowLayoutPanelLeft.Name = "FlowLayoutPanelLeft"
        Me.FlowLayoutPanelLeft.Size = New System.Drawing.Size(425, 762)
        Me.FlowLayoutPanelLeft.TabIndex = 0
        Me.FlowLayoutPanelLeft.WrapContents = False
        '
        'PanelBusquedaArchivos
        '
        Me.PanelBusquedaArchivos.Controls.Add(Me.lblTotal)
        Me.PanelBusquedaArchivos.Controls.Add(Me.GroupBox3)
        Me.PanelBusquedaArchivos.Controls.Add(Me.btnSeleccionarCarpeta)
        Me.PanelBusquedaArchivos.Location = New System.Drawing.Point(4, 5)
        Me.PanelBusquedaArchivos.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelBusquedaArchivos.Name = "PanelBusquedaArchivos"
        Me.PanelBusquedaArchivos.Size = New System.Drawing.Size(366, 257)
        Me.PanelBusquedaArchivos.TabIndex = 13
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(4, 220)
        Me.lblTotal.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(357, 20)
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
        Me.GroupBox3.Location = New System.Drawing.Point(3, 75)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox3.Size = New System.Drawing.Size(359, 126)
        Me.GroupBox3.TabIndex = 20
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Buscar en lista"
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(123, 35)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(225, 26)
        Me.TextBox1.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 40)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 20)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Contiene 1"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(24, 80)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 20)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Contiene 2"
        '
        'TextBox2
        '
        Me.TextBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox2.Location = New System.Drawing.Point(123, 75)
        Me.TextBox2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(225, 26)
        Me.TextBox2.TabIndex = 17
        '
        'btnSeleccionarCarpeta
        '
        Me.btnSeleccionarCarpeta.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSeleccionarCarpeta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSeleccionarCarpeta.Location = New System.Drawing.Point(3, 5)
        Me.btnSeleccionarCarpeta.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnSeleccionarCarpeta.Name = "btnSeleccionarCarpeta"
        Me.btnSeleccionarCarpeta.Size = New System.Drawing.Size(359, 62)
        Me.btnSeleccionarCarpeta.TabIndex = 0
        Me.btnSeleccionarCarpeta.Text = "📂 Seleccionar Carpeta"
        Me.btnSeleccionarCarpeta.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 20
        Me.ListBox1.Location = New System.Drawing.Point(4, 272)
        Me.ListBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(366, 224)
        Me.ListBox1.TabIndex = 14
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.dgvTiposNomenclaturas)
        Me.GroupBox4.Controls.Add(Me.txtCodigo)
        Me.GroupBox4.Controls.Add(Me.txtFecha)
        Me.GroupBox4.Controls.Add(Me.CheckBox1)
        Me.GroupBox4.Location = New System.Drawing.Point(4, 506)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox4.Size = New System.Drawing.Size(366, 350)
        Me.GroupBox4.TabIndex = 15
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
        Me.dgvTiposNomenclaturas.Location = New System.Drawing.Point(9, 29)
        Me.dgvTiposNomenclaturas.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.dgvTiposNomenclaturas.Name = "dgvTiposNomenclaturas"
        Me.dgvTiposNomenclaturas.ReadOnly = True
        Me.dgvTiposNomenclaturas.RowHeadersVisible = False
        Me.dgvTiposNomenclaturas.RowHeadersWidth = 62
        Me.dgvTiposNomenclaturas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTiposNomenclaturas.Size = New System.Drawing.Size(348, 208)
        Me.dgvTiposNomenclaturas.TabIndex = 1
        '
        'txtCodigo
        '
        Me.txtCodigo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtCodigo.Location = New System.Drawing.Point(3, 317)
        Me.txtCodigo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(340, 26)
        Me.txtCodigo.TabIndex = 9
        '
        'txtFecha
        '
        Me.txtFecha.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.txtFecha.Location = New System.Drawing.Point(3, 281)
        Me.txtFecha.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtFecha.Name = "txtFecha"
        Me.txtFecha.Size = New System.Drawing.Size(340, 26)
        Me.txtFecha.TabIndex = 10
        '
        'CheckBox1
        '
        Me.CheckBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(3, 247)
        Me.CheckBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(171, 24)
        Me.CheckBox1.TabIndex = 11
        Me.CheckBox1.Text = "Es una Notificación"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanelRight
        '
        Me.FlowLayoutPanelRight.AutoScroll = True
        Me.FlowLayoutPanelRight.Controls.Add(Me.PanelVisorPDF)
        Me.FlowLayoutPanelRight.Controls.Add(Me.GroupBox1)
        Me.FlowLayoutPanelRight.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanelRight.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanelRight.Location = New System.Drawing.Point(0, 0)
        Me.FlowLayoutPanelRight.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.FlowLayoutPanelRight.Name = "FlowLayoutPanelRight"
        Me.FlowLayoutPanelRight.Size = New System.Drawing.Size(925, 762)
        Me.FlowLayoutPanelRight.TabIndex = 0
        Me.FlowLayoutPanelRight.WrapContents = False
        '
        'PanelVisorPDF
        '
        Me.PanelVisorPDF.Controls.Add(Me.AxAcroPDF1)
        Me.PanelVisorPDF.Controls.Add(Me.lblNombreArchivo)
        Me.PanelVisorPDF.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelVisorPDF.Location = New System.Drawing.Point(4, 5)
        Me.PanelVisorPDF.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.PanelVisorPDF.Name = "PanelVisorPDF"
        Me.PanelVisorPDF.Size = New System.Drawing.Size(881, 521)
        Me.PanelVisorPDF.TabIndex = 2
        '
        'AxAcroPDF1
        '
        Me.AxAcroPDF1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AxAcroPDF1.Enabled = True
        Me.AxAcroPDF1.Location = New System.Drawing.Point(0, 0)
        Me.AxAcroPDF1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.AxAcroPDF1.Name = "AxAcroPDF1"
        Me.AxAcroPDF1.OcxState = CType(resources.GetObject("AxAcroPDF1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxAcroPDF1.Size = New System.Drawing.Size(881, 490)
        Me.AxAcroPDF1.TabIndex = 14
        '
        'lblNombreArchivo
        '
        Me.lblNombreArchivo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.lblNombreArchivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNombreArchivo.Location = New System.Drawing.Point(0, 490)
        Me.lblNombreArchivo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblNombreArchivo.Name = "lblNombreArchivo"
        Me.lblNombreArchivo.Size = New System.Drawing.Size(881, 31)
        Me.lblNombreArchivo.TabIndex = 15
        Me.lblNombreArchivo.Text = "Archivo: Ninguno"
        Me.lblNombreArchivo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtNombreModificado)
        Me.GroupBox1.Controls.Add(Me.btnRenombrar)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 536)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(15, 15, 15, 15)
        Me.GroupBox1.Size = New System.Drawing.Size(881, 324)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Opciones de Renombrado"
        '
        'txtNombreModificado
        '
        Me.txtNombreModificado.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombreModificado.BackColor = System.Drawing.SystemColors.Info
        Me.txtNombreModificado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombreModificado.Location = New System.Drawing.Point(4, 194)
        Me.txtNombreModificado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNombreModificado.Name = "txtNombreModificado"
        Me.txtNombreModificado.ReadOnly = True
        Me.txtNombreModificado.Size = New System.Drawing.Size(858, 30)
        Me.txtNombreModificado.TabIndex = 15
        Me.txtNombreModificado.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnRenombrar
        '
        Me.btnRenombrar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRenombrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRenombrar.Location = New System.Drawing.Point(4, 232)
        Me.btnRenombrar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnRenombrar.Name = "btnRenombrar"
        Me.btnRenombrar.Size = New System.Drawing.Size(858, 72)
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
        Me.GroupBox2.Location = New System.Drawing.Point(20, 40)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.GroupBox2.Size = New System.Drawing.Size(842, 143)
        Me.GroupBox2.TabIndex = 12
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Funcionario"
        '
        'txtCedula
        '
        Me.txtCedula.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCedula.Location = New System.Drawing.Point(661, 53)
        Me.txtCedula.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtCedula.Name = "txtCedula"
        Me.txtCedula.ReadOnly = True
        Me.txtCedula.Size = New System.Drawing.Size(173, 26)
        Me.txtCedula.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(657, 28)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(59, 20)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Cédula"
        '
        'txtNombre
        '
        Me.txtNombre.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombre.Location = New System.Drawing.Point(296, 53)
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.ReadOnly = True
        Me.txtNombre.Size = New System.Drawing.Size(357, 26)
        Me.txtNombre.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(292, 22)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(166, 20)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Nombre del associado"
        '
        'btnBuscarFuncionario
        '
        Me.btnBuscarFuncionario.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBuscarFuncionario.Location = New System.Drawing.Point(9, 29)
        Me.btnBuscarFuncionario.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnBuscarFuncionario.Name = "btnBuscarFuncionario"
        Me.btnBuscarFuncionario.Size = New System.Drawing.Size(279, 100)
        Me.btnBuscarFuncionario.TabIndex = 4
        Me.btnBuscarFuncionario.Text = "👤 Buscar y Asociar Funcionario"
        Me.btnBuscarFuncionario.UseVisualStyleBackColor = True
        '
        'frmRenombrarPDF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1356, 762)
        Me.Controls.Add(Me.SplitContainer1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MinimumSize = New System.Drawing.Size(889, 585)
        Me.Name = "frmRenombrarPDF"
        Me.Text = "Renombrado Inteligente de Archivos PDF"
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.FlowLayoutPanelLeft.ResumeLayout(False)
        Me.PanelBusquedaArchivos.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.dgvTiposNomenclaturas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FlowLayoutPanelRight.ResumeLayout(False)
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
    Friend WithEvents FlowLayoutPanelLeft As FlowLayoutPanel
    Friend WithEvents PanelBusquedaArchivos As Panel
    Friend WithEvents lblTotal As Label
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents btnSeleccionarCarpeta As Button
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents dgvTiposNomenclaturas As DataGridView
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents txtFecha As DateTimePicker
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents FlowLayoutPanelRight As FlowLayoutPanel
    Friend WithEvents PanelVisorPDF As Panel
    Friend WithEvents AxAcroPDF1 As AxAcroPDFLib.AxAcroPDF
    Friend WithEvents lblNombreArchivo As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtNombreModificado As TextBox
    Friend WithEvents btnRenombrar As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents txtCedula As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents btnBuscarFuncionario As Button
End Class