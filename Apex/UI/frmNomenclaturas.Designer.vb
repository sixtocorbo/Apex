<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmNomenclaturas
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
        Me.dgvNomenclaturas = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.tlpDetalles = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCodigo = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtArea = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtPatron = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtUbicacion = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtEjemplo = New System.Windows.Forms.TextBox()
        Me.chkUsaFecha = New System.Windows.Forms.CheckBox()
        Me.chkUsaNomenclaturaCodigo = New System.Windows.Forms.CheckBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtObservaciones = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtBuscar = New System.Windows.Forms.TextBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnEliminar = New System.Windows.Forms.Button()
        Me.btnNuevo = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnVolver = New System.Windows.Forms.Button()
        CType(Me.dgvNomenclaturas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.tlpDetalles.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'dgvNomenclaturas
        '
        Me.dgvNomenclaturas.AllowUserToAddRows = False
        Me.dgvNomenclaturas.AllowUserToDeleteRows = False
        Me.dgvNomenclaturas.AllowUserToResizeRows = False
        Me.dgvNomenclaturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNomenclaturas.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvNomenclaturas.Location = New System.Drawing.Point(3, 40)
        Me.dgvNomenclaturas.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.dgvNomenclaturas.MultiSelect = False
        Me.dgvNomenclaturas.Name = "dgvNomenclaturas"
        Me.dgvNomenclaturas.ReadOnly = True
        Me.dgvNomenclaturas.RowHeadersWidth = 62
        Me.dgvNomenclaturas.RowTemplate.Height = 28
        Me.dgvNomenclaturas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNomenclaturas.Size = New System.Drawing.Size(778, 203)
        Me.dgvNomenclaturas.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.Controls.Add(Me.tlpDetalles)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.Location = New System.Drawing.Point(3, 247)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(6)
        Me.GroupBox1.Size = New System.Drawing.Size(778, 275)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Detalles de la Nomenclatura"
        '
        'tlpDetalles
        '
        Me.tlpDetalles.AutoSize = True
        Me.tlpDetalles.ColumnCount = 4
        Me.tlpDetalles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpDetalles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpDetalles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpDetalles.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpDetalles.Controls.Add(Me.Label1, 0, 0)
        Me.tlpDetalles.Controls.Add(Me.txtNombre, 1, 0)
        Me.tlpDetalles.Controls.Add(Me.Label2, 0, 1)
        Me.tlpDetalles.Controls.Add(Me.txtCodigo, 1, 1)
        Me.tlpDetalles.Controls.Add(Me.Label3, 2, 1)
        Me.tlpDetalles.Controls.Add(Me.txtArea, 3, 1)
        Me.tlpDetalles.Controls.Add(Me.Label4, 0, 2)
        Me.tlpDetalles.Controls.Add(Me.txtPatron, 1, 2)
        Me.tlpDetalles.Controls.Add(Me.Label6, 2, 2)
        Me.tlpDetalles.Controls.Add(Me.txtUbicacion, 3, 2)
        Me.tlpDetalles.Controls.Add(Me.Label5, 0, 3)
        Me.tlpDetalles.Controls.Add(Me.txtEjemplo, 1, 3)
        Me.tlpDetalles.Controls.Add(Me.chkUsaFecha, 2, 3)
        Me.tlpDetalles.Controls.Add(Me.chkUsaNomenclaturaCodigo, 3, 3)
        Me.tlpDetalles.Controls.Add(Me.Label7, 0, 4)
        Me.tlpDetalles.Controls.Add(Me.txtObservaciones, 1, 4)
        Me.tlpDetalles.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpDetalles.Location = New System.Drawing.Point(6, 24)
        Me.tlpDetalles.Name = "tlpDetalles"
        Me.tlpDetalles.RowCount = 5
        Me.tlpDetalles.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalles.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalles.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalles.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalles.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpDetalles.Size = New System.Drawing.Size(766, 245)
        Me.tlpDetalles.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nombre:"
        '
        'txtNombre
        '
        Me.txtNombre.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.tlpDetalles.SetColumnSpan(Me.txtNombre, 3)
        Me.txtNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNombre.Location = New System.Drawing.Point(78, 2)
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(685, 35)
        Me.txtNombre.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 20)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Código:"
        '
        'txtCodigo
        '
        Me.txtCodigo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCodigo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtCodigo.Location = New System.Drawing.Point(78, 41)
        Me.txtCodigo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtCodigo.Name = "txtCodigo"
        Me.txtCodigo.Size = New System.Drawing.Size(264, 26)
        Me.txtCodigo.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(348, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 20)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Área:"
        '
        'txtArea
        '
        Me.txtArea.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtArea.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtArea.Location = New System.Drawing.Point(403, 41)
        Me.txtArea.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtArea.Name = "txtArea"
        Me.txtArea.Size = New System.Drawing.Size(360, 26)
        Me.txtArea.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 78)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 20)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Patrón:"
        '
        'txtPatron
        '
        Me.txtPatron.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPatron.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtPatron.Location = New System.Drawing.Point(78, 73)
        Me.txtPatron.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtPatron.Name = "txtPatron"
        Me.txtPatron.Size = New System.Drawing.Size(264, 26)
        Me.txtPatron.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(348, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 20)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Ubic.:"
        '
        'txtUbicacion
        '
        Me.txtUbicacion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtUbicacion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtUbicacion.Location = New System.Drawing.Point(403, 73)
        Me.txtUbicacion.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtUbicacion.Name = "txtUbicacion"
        Me.txtUbicacion.Size = New System.Drawing.Size(360, 26)
        Me.txtUbicacion.TabIndex = 4
        '
        'Label5
        '
        Me.Label5.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 110)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(70, 20)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Ejemplo:"
        '
        'txtEjemplo
        '
        Me.txtEjemplo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEjemplo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.txtEjemplo.Location = New System.Drawing.Point(78, 105)
        Me.txtEjemplo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtEjemplo.Name = "txtEjemplo"
        Me.txtEjemplo.Size = New System.Drawing.Size(264, 26)
        Me.txtEjemplo.TabIndex = 5
        '
        'chkUsaFecha
        '
        Me.chkUsaFecha.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.chkUsaFecha.AutoSize = True
        Me.chkUsaFecha.Location = New System.Drawing.Point(348, 108)
        Me.chkUsaFecha.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkUsaFecha.Name = "chkUsaFecha"
        Me.chkUsaFecha.Size = New System.Drawing.Size(113, 24)
        Me.chkUsaFecha.TabIndex = 6
        Me.chkUsaFecha.Text = "Usa Fecha"
        Me.chkUsaFecha.UseVisualStyleBackColor = True
        '
        'chkUsaNomenclaturaCodigo
        '
        Me.chkUsaNomenclaturaCodigo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.chkUsaNomenclaturaCodigo.AutoSize = True
        Me.chkUsaNomenclaturaCodigo.Location = New System.Drawing.Point(403, 108)
        Me.chkUsaNomenclaturaCodigo.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkUsaNomenclaturaCodigo.Name = "chkUsaNomenclaturaCodigo"
        Me.chkUsaNomenclaturaCodigo.Size = New System.Drawing.Size(189, 24)
        Me.chkUsaNomenclaturaCodigo.TabIndex = 7
        Me.chkUsaNomenclaturaCodigo.Text = "Usa Código (NCODE)"
        Me.chkUsaNomenclaturaCodigo.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(3, 181)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(41, 20)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Obs:"
        '
        'txtObservaciones
        '
        Me.txtObservaciones.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtObservaciones.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.tlpDetalles.SetColumnSpan(Me.txtObservaciones, 3)
        Me.txtObservaciones.Location = New System.Drawing.Point(78, 137)
        Me.txtObservaciones.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtObservaciones.Multiline = True
        Me.txtObservaciones.Name = "txtObservaciones"
        Me.txtObservaciones.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtObservaciones.Size = New System.Drawing.Size(685, 106)
        Me.txtObservaciones.TabIndex = 8
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(3, 5)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(63, 20)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Buscar:"
        '
        'txtBuscar
        '
        Me.txtBuscar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtBuscar.Location = New System.Drawing.Point(72, 2)
        Me.txtBuscar.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtBuscar.Name = "txtBuscar"
        Me.txtBuscar.Size = New System.Drawing.Size(703, 26)
        Me.txtBuscar.TabIndex = 0
        '
        'btnGuardar
        '
        Me.btnGuardar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGuardar.Location = New System.Drawing.Point(546, 3)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(109, 41)
        Me.btnGuardar.TabIndex = 9
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnEliminar
        '
        Me.btnEliminar.Location = New System.Drawing.Point(431, 3)
        Me.btnEliminar.Name = "btnEliminar"
        Me.btnEliminar.Size = New System.Drawing.Size(109, 41)
        Me.btnEliminar.TabIndex = 10
        Me.btnEliminar.Text = "Eliminar"
        Me.btnEliminar.UseVisualStyleBackColor = True
        '
        'btnNuevo
        '
        Me.btnNuevo.Location = New System.Drawing.Point(316, 3)
        Me.btnNuevo.Name = "btnNuevo"
        Me.btnNuevo.Size = New System.Drawing.Size(109, 41)
        Me.btnNuevo.TabIndex = 11
        Me.btnNuevo.Text = "Nuevo"
        Me.btnNuevo.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.dgvNomenclaturas, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 3)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(784, 571)
        Me.TableLayoutPanel1.TabIndex = 12
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.txtBuscar)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 2)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Padding = New System.Windows.Forms.Padding(0, 0, 0, 4)
        Me.Panel1.Size = New System.Drawing.Size(778, 34)
        Me.Panel1.TabIndex = 0
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.Controls.Add(Me.btnVolver)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnGuardar)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnEliminar)
        Me.FlowLayoutPanel1.Controls.Add(Me.btnNuevo)
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(116, 526)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(665, 47)
        Me.FlowLayoutPanel1.TabIndex = 3
        '
        'btnVolver
        '
        Me.btnVolver.Location = New System.Drawing.Point(661, 3)
        Me.btnVolver.Name = "btnVolver"
        Me.btnVolver.Size = New System.Drawing.Size(103, 51)
        Me.btnVolver.TabIndex = 12
        Me.btnVolver.Text = "Volver"
        Me.btnVolver.UseVisualStyleBackColor = True
        '
        'frmNomenclaturas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 571)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frmNomenclaturas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestor de Nomenclaturas"
        CType(Me.dgvNomenclaturas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.tlpDetalles.ResumeLayout(False)
        Me.tlpDetalles.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents dgvNomenclaturas As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtObservaciones As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtUbicacion As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtEjemplo As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtPatron As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtArea As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtCodigo As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents chkUsaNomenclaturaCodigo As CheckBox
    Friend WithEvents chkUsaFecha As CheckBox
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnEliminar As Button
    Friend WithEvents btnNuevo As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents txtBuscar As TextBox
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents btnVolver As Button
    Private WithEvents tlpDetalles As TableLayoutPanel
End Class