<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmIncidenciasConfiguracion
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.chkEsAusencia = New System.Windows.Forms.CheckBox()
        Me.chkSuspendeViatico = New System.Windows.Forms.CheckBox()
        Me.chkAfectaPresentismo = New System.Windows.Forms.CheckBox()
        Me.chkEsHabil = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cboCategoria = New System.Windows.Forms.ComboBox()
        Me.btnGuardar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 23)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Nombre:"
        '
        'txtNombre
        '
        Me.txtNombre.Enabled = False
        Me.txtNombre.Location = New System.Drawing.Point(112, 18)
        Me.txtNombre.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.ReadOnly = True
        Me.txtNombre.Size = New System.Drawing.Size(444, 26)
        Me.txtNombre.TabIndex = 1
        '
        'chkEsAusencia
        '
        Me.chkEsAusencia.AutoSize = True
        Me.chkEsAusencia.Location = New System.Drawing.Point(112, 77)
        Me.chkEsAusencia.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkEsAusencia.Name = "chkEsAusencia"
        Me.chkEsAusencia.Size = New System.Drawing.Size(182, 24)
        Me.chkEsAusencia.TabIndex = 2
        Me.chkEsAusencia.Text = "Considerar Ausencia"
        Me.chkEsAusencia.UseVisualStyleBackColor = True
        '
        'chkSuspendeViatico
        '
        Me.chkSuspendeViatico.AutoSize = True
        Me.chkSuspendeViatico.Location = New System.Drawing.Point(112, 112)
        Me.chkSuspendeViatico.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkSuspendeViatico.Name = "chkSuspendeViatico"
        Me.chkSuspendeViatico.Size = New System.Drawing.Size(160, 24)
        Me.chkSuspendeViatico.TabIndex = 3
        Me.chkSuspendeViatico.Text = "Suspende Viático"
        Me.chkSuspendeViatico.UseVisualStyleBackColor = True
        '
        'chkAfectaPresentismo
        '
        Me.chkAfectaPresentismo.AutoSize = True
        Me.chkAfectaPresentismo.Location = New System.Drawing.Point(328, 77)
        Me.chkAfectaPresentismo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkAfectaPresentismo.Name = "chkAfectaPresentismo"
        Me.chkAfectaPresentismo.Size = New System.Drawing.Size(174, 24)
        Me.chkAfectaPresentismo.TabIndex = 4
        Me.chkAfectaPresentismo.Text = "Afecta Presentismo"
        Me.chkAfectaPresentismo.UseVisualStyleBackColor = True
        '
        'chkEsHabil
        '
        Me.chkEsHabil.AutoSize = True
        Me.chkEsHabil.Location = New System.Drawing.Point(328, 112)
        Me.chkEsHabil.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.chkEsHabil.Name = "chkEsHabil"
        Me.chkEsHabil.Size = New System.Drawing.Size(176, 24)
        Me.chkEsHabil.TabIndex = 5
        Me.chkEsHabil.Text = "Contar Días Hábiles"
        Me.chkEsHabil.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 169)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 20)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Categoría:"
        '
        'cboCategoria
        '
        Me.cboCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategoria.FormattingEnabled = True
        Me.cboCategoria.Location = New System.Drawing.Point(112, 165)
        Me.cboCategoria.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.cboCategoria.Name = "cboCategoria"
        Me.cboCategoria.Size = New System.Drawing.Size(444, 28)
        Me.cboCategoria.TabIndex = 7
        '
        'btnGuardar
        '
        Me.btnGuardar.Location = New System.Drawing.Point(446, 231)
        Me.btnGuardar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnGuardar.Name = "btnGuardar"
        Me.btnGuardar.Size = New System.Drawing.Size(112, 35)
        Me.btnGuardar.TabIndex = 8
        Me.btnGuardar.Text = "Guardar"
        Me.btnGuardar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(324, 231)
        Me.btnCancelar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(112, 35)
        Me.btnCancelar.TabIndex = 9
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'frmIncidenciaDetalle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(576, 285)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnGuardar)
        Me.Controls.Add(Me.cboCategoria)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.chkEsHabil)
        Me.Controls.Add(Me.chkAfectaPresentismo)
        Me.Controls.Add(Me.chkSuspendeViatico)
        Me.Controls.Add(Me.chkEsAusencia)
        Me.Controls.Add(Me.txtNombre)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmIncidenciaDetalle"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Detalle de Incidencia"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents chkEsAusencia As CheckBox
    Friend WithEvents chkSuspendeViatico As CheckBox
    Friend WithEvents chkAfectaPresentismo As CheckBox
    Friend WithEvents chkEsHabil As CheckBox
    Friend WithEvents Label2 As Label
    Friend WithEvents cboCategoria As ComboBox
    Friend WithEvents btnGuardar As Button
    Friend WithEvents btnCancelar As Button
End Class
