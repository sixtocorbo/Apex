<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAsistenteImportacion
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
        Me.pnlPaso1_Seleccion = New System.Windows.Forms.Panel()
        Me.btnPaso1_Siguiente = New System.Windows.Forms.Button()
        Me.pnlCardDotaciones = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pnlCardHistoricos = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pnlCardLicencias = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblPaso1_Titulo = New System.Windows.Forms.Label()
        Me.pnlPaso2_Cargar = New System.Windows.Forms.Panel()
        Me.btnPaso2_Procesar = New System.Windows.Forms.Button()
        Me.btnPaso2_Volver = New System.Windows.Forms.Button()
        Me.pnlDropZone = New System.Windows.Forms.Panel()
        Me.lblDropZone = New System.Windows.Forms.Label()
        Me.lblArchivoSeleccionado = New System.Windows.Forms.Label()
        Me.btnDescargarPlantilla = New System.Windows.Forms.Button()
        Me.lblPaso2_Titulo = New System.Windows.Forms.Label()
        Me.pnlPaso3_Validar = New System.Windows.Forms.Panel()
        Me.lblPaso3_Feedback = New System.Windows.Forms.Label()
        Me.btnPaso3_Importar = New System.Windows.Forms.Button()
        Me.btnDescargarErrores = New System.Windows.Forms.Button()
        Me.btnPaso3_Volver = New System.Windows.Forms.Button()
        Me.lblResumenErrores = New System.Windows.Forms.Label()
        Me.lblResumenValidos = New System.Windows.Forms.Label()
        Me.dgvPrevisualizacion = New System.Windows.Forms.DataGridView()
        Me.lblPaso3_Titulo = New System.Windows.Forms.Label()
        Me.pnlPaso4_Resumen = New System.Windows.Forms.Panel()
        Me.gbxNuevosTipos = New System.Windows.Forms.GroupBox()
        Me.lstNuevosTiposLicencia = New System.Windows.Forms.ListBox()
        Me.gbxNuevosFuncionarios = New System.Windows.Forms.GroupBox()
        Me.lstNuevosFuncionarios = New System.Windows.Forms.ListBox()
        Me.lblResumenTiempo = New System.Windows.Forms.Label()
        Me.btnPaso4_Finalizar = New System.Windows.Forms.Button()
        Me.btnPaso4_OtraVez = New System.Windows.Forms.Button()
        Me.lblResumenErroresFinal = New System.Windows.Forms.Label()
        Me.lblResumenImportados = New System.Windows.Forms.Label()
        Me.lblPaso4_Titulo = New System.Windows.Forms.Label()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.pnlPaso1_5_SubTipo = New System.Windows.Forms.Panel()
        Me.btnPaso1_5_Siguiente = New System.Windows.Forms.Button()
        Me.btnPaso1_5_Volver = New System.Windows.Forms.Button()
        Me.gbxSubtipo = New System.Windows.Forms.GroupBox()
        Me.rbNocturnidad = New System.Windows.Forms.RadioButton()
        Me.rbPresentismo = New System.Windows.Forms.RadioButton()
        Me.lblPaso1_5_Titulo = New System.Windows.Forms.Label()
        Me.pnlPaso1_Seleccion.SuspendLayout()
        Me.pnlCardDotaciones.SuspendLayout()
        Me.pnlCardHistoricos.SuspendLayout()
        Me.pnlCardLicencias.SuspendLayout()
        Me.pnlPaso2_Cargar.SuspendLayout()
        Me.pnlDropZone.SuspendLayout()
        Me.pnlPaso3_Validar.SuspendLayout()
        CType(Me.dgvPrevisualizacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlPaso4_Resumen.SuspendLayout()
        Me.gbxNuevosTipos.SuspendLayout()
        Me.gbxNuevosFuncionarios.SuspendLayout()
        Me.pnlPaso1_5_SubTipo.SuspendLayout()
        Me.gbxSubtipo.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlPaso1_Seleccion
        '
        Me.pnlPaso1_Seleccion.Controls.Add(Me.btnPaso1_Siguiente)
        Me.pnlPaso1_Seleccion.Controls.Add(Me.pnlCardDotaciones)
        Me.pnlPaso1_Seleccion.Controls.Add(Me.pnlCardHistoricos)
        Me.pnlPaso1_Seleccion.Controls.Add(Me.pnlCardLicencias)
        Me.pnlPaso1_Seleccion.Controls.Add(Me.lblPaso1_Titulo)
        Me.pnlPaso1_Seleccion.Location = New System.Drawing.Point(12, 12)
        Me.pnlPaso1_Seleccion.Name = "pnlPaso1_Seleccion"
        Me.pnlPaso1_Seleccion.Size = New System.Drawing.Size(760, 437)
        Me.pnlPaso1_Seleccion.TabIndex = 0
        '
        'btnPaso1_Siguiente
        '
        Me.btnPaso1_Siguiente.Location = New System.Drawing.Point(623, 388)
        Me.btnPaso1_Siguiente.Name = "btnPaso1_Siguiente"
        Me.btnPaso1_Siguiente.Size = New System.Drawing.Size(120, 35)
        Me.btnPaso1_Siguiente.TabIndex = 4
        Me.btnPaso1_Siguiente.Text = "Siguiente >"
        Me.btnPaso1_Siguiente.UseVisualStyleBackColor = True
        '
        'pnlCardDotaciones
        '
        Me.pnlCardDotaciones.BackColor = System.Drawing.Color.White
        Me.pnlCardDotaciones.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlCardDotaciones.Controls.Add(Me.Label3)
        Me.pnlCardDotaciones.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pnlCardDotaciones.Location = New System.Drawing.Point(510, 80)
        Me.pnlCardDotaciones.Name = "pnlCardDotaciones"
        Me.pnlCardDotaciones.Size = New System.Drawing.Size(200, 150)
        Me.pnlCardDotaciones.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(45, 61)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(110, 28)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Dotaciones"
        '
        'pnlCardHistoricos
        '
        Me.pnlCardHistoricos.BackColor = System.Drawing.Color.White
        Me.pnlCardHistoricos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlCardHistoricos.Controls.Add(Me.Label2)
        Me.pnlCardHistoricos.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pnlCardHistoricos.Location = New System.Drawing.Point(280, 80)
        Me.pnlCardHistoricos.Name = "pnlCardHistoricos"
        Me.pnlCardHistoricos.Size = New System.Drawing.Size(200, 150)
        Me.pnlCardHistoricos.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.Location = New System.Drawing.Point(50, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 28)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Históricos"
        '
        'pnlCardLicencias
        '
        Me.pnlCardLicencias.BackColor = System.Drawing.Color.White
        Me.pnlCardLicencias.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlCardLicencias.Controls.Add(Me.Label1)
        Me.pnlCardLicencias.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pnlCardLicencias.Location = New System.Drawing.Point(50, 80)
        Me.pnlCardLicencias.Name = "pnlCardLicencias"
        Me.pnlCardLicencias.Size = New System.Drawing.Size(200, 150)
        Me.pnlCardLicencias.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(55, 61)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(90, 28)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Licencias"
        '
        'lblPaso1_Titulo
        '
        Me.lblPaso1_Titulo.AutoSize = True
        Me.lblPaso1_Titulo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblPaso1_Titulo.Location = New System.Drawing.Point(20, 20)
        Me.lblPaso1_Titulo.Name = "lblPaso1_Titulo"
        Me.lblPaso1_Titulo.Size = New System.Drawing.Size(437, 32)
        Me.lblPaso1_Titulo.TabIndex = 0
        Me.lblPaso1_Titulo.Text = "Paso 1: Seleccione el tipo de archivo"
        '
        'pnlPaso2_Cargar
        '
        Me.pnlPaso2_Cargar.Controls.Add(Me.btnPaso2_Procesar)
        Me.pnlPaso2_Cargar.Controls.Add(Me.btnPaso2_Volver)
        Me.pnlPaso2_Cargar.Controls.Add(Me.pnlDropZone)
        Me.pnlPaso2_Cargar.Controls.Add(Me.lblArchivoSeleccionado)
        Me.pnlPaso2_Cargar.Controls.Add(Me.btnDescargarPlantilla)
        Me.pnlPaso2_Cargar.Controls.Add(Me.lblPaso2_Titulo)
        Me.pnlPaso2_Cargar.Location = New System.Drawing.Point(12, 12)
        Me.pnlPaso2_Cargar.Name = "pnlPaso2_Cargar"
        Me.pnlPaso2_Cargar.Size = New System.Drawing.Size(760, 437)
        Me.pnlPaso2_Cargar.TabIndex = 1
        '
        'btnPaso2_Procesar
        '
        Me.btnPaso2_Procesar.Location = New System.Drawing.Point(623, 388)
        Me.btnPaso2_Procesar.Name = "btnPaso2_Procesar"
        Me.btnPaso2_Procesar.Size = New System.Drawing.Size(120, 35)
        Me.btnPaso2_Procesar.TabIndex = 5
        Me.btnPaso2_Procesar.Text = "Procesar >"
        Me.btnPaso2_Procesar.UseVisualStyleBackColor = True
        '
        'btnPaso2_Volver
        '
        Me.btnPaso2_Volver.Location = New System.Drawing.Point(26, 388)
        Me.btnPaso2_Volver.Name = "btnPaso2_Volver"
        Me.btnPaso2_Volver.Size = New System.Drawing.Size(120, 35)
        Me.btnPaso2_Volver.TabIndex = 4
        Me.btnPaso2_Volver.Text = "< Volver"
        Me.btnPaso2_Volver.UseVisualStyleBackColor = True
        '
        'pnlDropZone
        '
        Me.pnlDropZone.AllowDrop = True
        Me.pnlDropZone.BackColor = System.Drawing.Color.AliceBlue
        Me.pnlDropZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDropZone.Controls.Add(Me.lblDropZone)
        Me.pnlDropZone.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pnlDropZone.Location = New System.Drawing.Point(26, 120)
        Me.pnlDropZone.Name = "pnlDropZone"
        Me.pnlDropZone.Size = New System.Drawing.Size(717, 150)
        Me.pnlDropZone.TabIndex = 3
        '
        'lblDropZone
        '
        Me.lblDropZone.AutoSize = True
        Me.lblDropZone.Location = New System.Drawing.Point(240, 65)
        Me.lblDropZone.Name = "lblDropZone"
        Me.lblDropZone.Size = New System.Drawing.Size(237, 20)
        Me.lblDropZone.TabIndex = 0
        Me.lblDropZone.Text = "Arrastre el archivo aquí o haga clic"
        '
        'lblArchivoSeleccionado
        '
        Me.lblArchivoSeleccionado.AutoSize = True
        Me.lblArchivoSeleccionado.Location = New System.Drawing.Point(22, 280)
        Me.lblArchivoSeleccionado.Name = "lblArchivoSeleccionado"
        Me.lblArchivoSeleccionado.Size = New System.Drawing.Size(0, 20)
        Me.lblArchivoSeleccionado.TabIndex = 2
        '
        'btnDescargarPlantilla
        '
        Me.btnDescargarPlantilla.Location = New System.Drawing.Point(26, 70)
        Me.btnDescargarPlantilla.Name = "btnDescargarPlantilla"
        Me.btnDescargarPlantilla.Size = New System.Drawing.Size(250, 35)
        Me.btnDescargarPlantilla.TabIndex = 1
        Me.btnDescargarPlantilla.Text = "⬇️ Descargar Plantilla"
        Me.btnDescargarPlantilla.UseVisualStyleBackColor = True
        '
        'lblPaso2_Titulo
        '
        Me.lblPaso2_Titulo.AutoSize = True
        Me.lblPaso2_Titulo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblPaso2_Titulo.Location = New System.Drawing.Point(20, 20)
        Me.lblPaso2_Titulo.Name = "lblPaso2_Titulo"
        Me.lblPaso2_Titulo.Size = New System.Drawing.Size(326, 32)
        Me.lblPaso2_Titulo.TabIndex = 0
        Me.lblPaso2_Titulo.Text = "Paso 2: Cargar Archivo de..."
        '
        'pnlPaso3_Validar
        '
        Me.pnlPaso3_Validar.Controls.Add(Me.lblPaso3_Feedback)
        Me.pnlPaso3_Validar.Controls.Add(Me.btnPaso3_Importar)
        Me.pnlPaso3_Validar.Controls.Add(Me.btnDescargarErrores)
        Me.pnlPaso3_Validar.Controls.Add(Me.btnPaso3_Volver)
        Me.pnlPaso3_Validar.Controls.Add(Me.lblResumenErrores)
        Me.pnlPaso3_Validar.Controls.Add(Me.lblResumenValidos)
        Me.pnlPaso3_Validar.Controls.Add(Me.dgvPrevisualizacion)
        Me.pnlPaso3_Validar.Controls.Add(Me.lblPaso3_Titulo)
        Me.pnlPaso3_Validar.Location = New System.Drawing.Point(12, 12)
        Me.pnlPaso3_Validar.Name = "pnlPaso3_Validar"
        Me.pnlPaso3_Validar.Size = New System.Drawing.Size(760, 437)
        Me.pnlPaso3_Validar.TabIndex = 2
        '
        'lblPaso3_Feedback
        '
        Me.lblPaso3_Feedback.AutoSize = True
        Me.lblPaso3_Feedback.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPaso3_Feedback.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.lblPaso3_Feedback.Location = New System.Drawing.Point(308, 396)
        Me.lblPaso3_Feedback.Name = "lblPaso3_Feedback"
        Me.lblPaso3_Feedback.Size = New System.Drawing.Size(103, 20)
        Me.lblPaso3_Feedback.TabIndex = 7
        Me.lblPaso3_Feedback.Text = "Importando..."
        '
        'btnPaso3_Importar
        '
        Me.btnPaso3_Importar.Location = New System.Drawing.Point(523, 388)
        Me.btnPaso3_Importar.Name = "btnPaso3_Importar"
        Me.btnPaso3_Importar.Size = New System.Drawing.Size(220, 35)
        Me.btnPaso3_Importar.TabIndex = 6
        Me.btnPaso3_Importar.Text = "✅ Confirmar e Importar"
        Me.btnPaso3_Importar.UseVisualStyleBackColor = True
        '
        'btnDescargarErrores
        '
        Me.btnDescargarErrores.Location = New System.Drawing.Point(152, 388)
        Me.btnDescargarErrores.Name = "btnDescargarErrores"
        Me.btnDescargarErrores.Size = New System.Drawing.Size(150, 35)
        Me.btnDescargarErrores.TabIndex = 5
        Me.btnDescargarErrores.Text = "Descargar Errores"
        Me.btnDescargarErrores.UseVisualStyleBackColor = True
        '
        'btnPaso3_Volver
        '
        Me.btnPaso3_Volver.Location = New System.Drawing.Point(26, 388)
        Me.btnPaso3_Volver.Name = "btnPaso3_Volver"
        Me.btnPaso3_Volver.Size = New System.Drawing.Size(120, 35)
        Me.btnPaso3_Volver.TabIndex = 4
        Me.btnPaso3_Volver.Text = "< Volver"
        Me.btnPaso3_Volver.UseVisualStyleBackColor = True
        '
        'lblResumenErrores
        '
        Me.lblResumenErrores.AutoSize = True
        Me.lblResumenErrores.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblResumenErrores.ForeColor = System.Drawing.Color.Red
        Me.lblResumenErrores.Location = New System.Drawing.Point(22, 100)
        Me.lblResumenErrores.Name = "lblResumenErrores"
        Me.lblResumenErrores.Size = New System.Drawing.Size(193, 23)
        Me.lblResumenErrores.TabIndex = 3
        Me.lblResumenErrores.Text = "❌ Errores Encontrados: 0"
        '
        'lblResumenValidos
        '
        Me.lblResumenValidos.AutoSize = True
        Me.lblResumenValidos.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblResumenValidos.ForeColor = System.Drawing.Color.Green
        Me.lblResumenValidos.Location = New System.Drawing.Point(22, 70)
        Me.lblResumenValidos.Name = "lblResumenValidos"
        Me.lblResumenValidos.Size = New System.Drawing.Size(183, 23)
        Me.lblResumenValidos.TabIndex = 2
        Me.lblResumenValidos.Text = "✔️ Registros Válidos: 0"
        '
        'dgvPrevisualizacion
        '
        Me.dgvPrevisualizacion.AllowUserToAddRows = False
        Me.dgvPrevisualizacion.AllowUserToDeleteRows = False
        Me.dgvPrevisualizacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPrevisualizacion.Location = New System.Drawing.Point(26, 140)
        Me.dgvPrevisualizacion.Name = "dgvPrevisualizacion"
        Me.dgvPrevisualizacion.ReadOnly = True
        Me.dgvPrevisualizacion.RowHeadersWidth = 51
        Me.dgvPrevisualizacion.RowTemplate.Height = 24
        Me.dgvPrevisualizacion.Size = New System.Drawing.Size(717, 230)
        Me.dgvPrevisualizacion.TabIndex = 1
        '
        'lblPaso3_Titulo
        '
        Me.lblPaso3_Titulo.AutoSize = True
        Me.lblPaso3_Titulo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblPaso3_Titulo.Location = New System.Drawing.Point(20, 20)
        Me.lblPaso3_Titulo.Name = "lblPaso3_Titulo"
        Me.lblPaso3_Titulo.Size = New System.Drawing.Size(328, 32)
        Me.lblPaso3_Titulo.TabIndex = 0
        Me.lblPaso3_Titulo.Text = "Paso 3: Validar y Confirmar"
        '
        'pnlPaso4_Resumen
        '
        Me.pnlPaso4_Resumen.Controls.Add(Me.gbxNuevosTipos)
        Me.pnlPaso4_Resumen.Controls.Add(Me.gbxNuevosFuncionarios)
        Me.pnlPaso4_Resumen.Controls.Add(Me.lblResumenTiempo)
        Me.pnlPaso4_Resumen.Controls.Add(Me.btnPaso4_Finalizar)
        Me.pnlPaso4_Resumen.Controls.Add(Me.btnPaso4_OtraVez)
        Me.pnlPaso4_Resumen.Controls.Add(Me.lblResumenErroresFinal)
        Me.pnlPaso4_Resumen.Controls.Add(Me.lblResumenImportados)
        Me.pnlPaso4_Resumen.Controls.Add(Me.lblPaso4_Titulo)
        Me.pnlPaso4_Resumen.Location = New System.Drawing.Point(12, 12)
        Me.pnlPaso4_Resumen.Name = "pnlPaso4_Resumen"
        Me.pnlPaso4_Resumen.Size = New System.Drawing.Size(760, 437)
        Me.pnlPaso4_Resumen.TabIndex = 3
        '
        'gbxNuevosTipos
        '
        Me.gbxNuevosTipos.Controls.Add(Me.lstNuevosTiposLicencia)
        Me.gbxNuevosTipos.Location = New System.Drawing.Point(390, 240)
        Me.gbxNuevosTipos.Name = "gbxNuevosTipos"
        Me.gbxNuevosTipos.Size = New System.Drawing.Size(320, 140)
        Me.gbxNuevosTipos.TabIndex = 7
        Me.gbxNuevosTipos.TabStop = False
        Me.gbxNuevosTipos.Text = "Nuevos Tipos de Licencia Creados"
        Me.gbxNuevosTipos.Visible = False
        '
        'lstNuevosTiposLicencia
        '
        Me.lstNuevosTiposLicencia.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstNuevosTiposLicencia.FormattingEnabled = True
        Me.lstNuevosTiposLicencia.ItemHeight = 20
        Me.lstNuevosTiposLicencia.Location = New System.Drawing.Point(3, 23)
        Me.lstNuevosTiposLicencia.Name = "lstNuevosTiposLicencia"
        Me.lstNuevosTiposLicencia.Size = New System.Drawing.Size(314, 114)
        Me.lstNuevosTiposLicencia.TabIndex = 0
        '
        'gbxNuevosFuncionarios
        '
        Me.gbxNuevosFuncionarios.Controls.Add(Me.lstNuevosFuncionarios)
        Me.gbxNuevosFuncionarios.Location = New System.Drawing.Point(54, 240)
        Me.gbxNuevosFuncionarios.Name = "gbxNuevosFuncionarios"
        Me.gbxNuevosFuncionarios.Size = New System.Drawing.Size(320, 140)
        Me.gbxNuevosFuncionarios.TabIndex = 6
        Me.gbxNuevosFuncionarios.TabStop = False
        Me.gbxNuevosFuncionarios.Text = "Nuevos Funcionarios Creados"
        Me.gbxNuevosFuncionarios.Visible = False
        '
        'lstNuevosFuncionarios
        '
        Me.lstNuevosFuncionarios.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lstNuevosFuncionarios.FormattingEnabled = True
        Me.lstNuevosFuncionarios.ItemHeight = 20
        Me.lstNuevosFuncionarios.Location = New System.Drawing.Point(3, 23)
        Me.lstNuevosFuncionarios.Name = "lstNuevosFuncionarios"
        Me.lstNuevosFuncionarios.Size = New System.Drawing.Size(314, 114)
        Me.lstNuevosFuncionarios.TabIndex = 0
        '
        'lblResumenTiempo
        '
        Me.lblResumenTiempo.AutoSize = True
        Me.lblResumenTiempo.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblResumenTiempo.Location = New System.Drawing.Point(50, 200)
        Me.lblResumenTiempo.Name = "lblResumenTiempo"
        Me.lblResumenTiempo.Size = New System.Drawing.Size(262, 28)
        Me.lblResumenTiempo.TabIndex = 5
        Me.lblResumenTiempo.Text = "• Tiempo de la Operación: 0s"
        '
        'btnPaso4_Finalizar
        '
        Me.btnPaso4_Finalizar.Location = New System.Drawing.Point(623, 388)
        Me.btnPaso4_Finalizar.Name = "btnPaso4_Finalizar"
        Me.btnPaso4_Finalizar.Size = New System.Drawing.Size(120, 35)
        Me.btnPaso4_Finalizar.TabIndex = 4
        Me.btnPaso4_Finalizar.Text = "Finalizar"
        Me.btnPaso4_Finalizar.UseVisualStyleBackColor = True
        '
        'btnPaso4_OtraVez
        '
        Me.btnPaso4_OtraVez.Location = New System.Drawing.Point(26, 388)
        Me.btnPaso4_OtraVez.Name = "btnPaso4_OtraVez"
        Me.btnPaso4_OtraVez.Size = New System.Drawing.Size(150, 35)
        Me.btnPaso4_OtraVez.TabIndex = 3
        Me.btnPaso4_OtraVez.Text = "Importar Otro Archivo"
        Me.btnPaso4_OtraVez.UseVisualStyleBackColor = True
        '
        'lblResumenErroresFinal
        '
        Me.lblResumenErroresFinal.AutoSize = True
        Me.lblResumenErroresFinal.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblResumenErroresFinal.Location = New System.Drawing.Point(50, 150)
        Me.lblResumenErroresFinal.Name = "lblResumenErroresFinal"
        Me.lblResumenErroresFinal.Size = New System.Drawing.Size(296, 28)
        Me.lblResumenErroresFinal.TabIndex = 2
        Me.lblResumenErroresFinal.Text = "• Registros con Errores Omitidos: 0"
        '
        'lblResumenImportados
        '
        Me.lblResumenImportados.AutoSize = True
        Me.lblResumenImportados.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblResumenImportados.Location = New System.Drawing.Point(50, 100)
        Me.lblResumenImportados.Name = "lblResumenImportados"
        Me.lblResumenImportados.Size = New System.Drawing.Size(262, 28)
        Me.lblResumenImportados.TabIndex = 1
        Me.lblResumenImportados.Text = "• Registros Importados: 0"
        '
        'lblPaso4_Titulo
        '
        Me.lblPaso4_Titulo.AutoSize = True
        Me.lblPaso4_Titulo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblPaso4_Titulo.Location = New System.Drawing.Point(20, 20)
        Me.lblPaso4_Titulo.Name = "lblPaso4_Titulo"
        Me.lblPaso4_Titulo.Size = New System.Drawing.Size(370, 32)
        Me.lblPaso4_Titulo.TabIndex = 0
        Me.lblPaso4_Titulo.Text = "Paso 4: Resumen de Importación"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'pnlPaso1_5_SubTipo
        '
        Me.pnlPaso1_5_SubTipo.Controls.Add(Me.btnPaso1_5_Siguiente)
        Me.pnlPaso1_5_SubTipo.Controls.Add(Me.btnPaso1_5_Volver)
        Me.pnlPaso1_5_SubTipo.Controls.Add(Me.gbxSubtipo)
        Me.pnlPaso1_5_SubTipo.Controls.Add(Me.lblPaso1_5_Titulo)
        Me.pnlPaso1_5_SubTipo.Location = New System.Drawing.Point(12, 12)
        Me.pnlPaso1_5_SubTipo.Name = "pnlPaso1_5_SubTipo"
        Me.pnlPaso1_5_SubTipo.Size = New System.Drawing.Size(760, 437)
        Me.pnlPaso1_5_SubTipo.TabIndex = 4
        '
        'btnPaso1_5_Siguiente
        '
        Me.btnPaso1_5_Siguiente.Location = New System.Drawing.Point(623, 388)
        Me.btnPaso1_5_Siguiente.Name = "btnPaso1_5_Siguiente"
        Me.btnPaso1_5_Siguiente.Size = New System.Drawing.Size(120, 35)
        Me.btnPaso1_5_Siguiente.TabIndex = 3
        Me.btnPaso1_5_Siguiente.Text = "Siguiente >"
        Me.btnPaso1_5_Siguiente.UseVisualStyleBackColor = True
        '
        'btnPaso1_5_Volver
        '
        Me.btnPaso1_5_Volver.Location = New System.Drawing.Point(26, 388)
        Me.btnPaso1_5_Volver.Name = "btnPaso1_5_Volver"
        Me.btnPaso1_5_Volver.Size = New System.Drawing.Size(120, 35)
        Me.btnPaso1_5_Volver.TabIndex = 2
        Me.btnPaso1_5_Volver.Text = "< Volver"
        Me.btnPaso1_5_Volver.UseVisualStyleBackColor = True
        '
        'gbxSubtipo
        '
        Me.gbxSubtipo.Controls.Add(Me.rbNocturnidad)
        Me.gbxSubtipo.Controls.Add(Me.rbPresentismo)
        Me.gbxSubtipo.Location = New System.Drawing.Point(50, 80)
        Me.gbxSubtipo.Name = "gbxSubtipo"
        Me.gbxSubtipo.Size = New System.Drawing.Size(660, 100)
        Me.gbxSubtipo.TabIndex = 1
        Me.gbxSubtipo.TabStop = False
        Me.gbxSubtipo.Text = "Tipo de Histórico"
        '
        'rbNocturnidad
        '
        Me.rbNocturnidad.AutoSize = True
        Me.rbNocturnidad.Location = New System.Drawing.Point(180, 45)
        Me.rbNocturnidad.Name = "rbNocturnidad"
        Me.rbNocturnidad.Size = New System.Drawing.Size(110, 24)
        Me.rbNocturnidad.TabIndex = 1
        Me.rbNocturnidad.TabStop = True
        Me.rbNocturnidad.Text = "Nocturnidad"
        Me.rbNocturnidad.UseVisualStyleBackColor = True
        '
        'rbPresentismo
        '
        Me.rbPresentismo.AutoSize = True
        Me.rbPresentismo.Location = New System.Drawing.Point(30, 45)
        Me.rbPresentismo.Name = "rbPresentismo"
        Me.rbPresentismo.Size = New System.Drawing.Size(111, 24)
        Me.rbPresentismo.TabIndex = 0
        Me.rbPresentismo.TabStop = True
        Me.rbPresentismo.Text = "Presentismo"
        Me.rbPresentismo.UseVisualStyleBackColor = True
        '
        'lblPaso1_5_Titulo
        '
        Me.lblPaso1_5_Titulo.AutoSize = True
        Me.lblPaso1_5_Titulo.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
        Me.lblPaso1_5_Titulo.Location = New System.Drawing.Point(20, 20)
        Me.lblPaso1_5_Titulo.Name = "lblPaso1_5_Titulo"
        Me.lblPaso1_5_Titulo.Size = New System.Drawing.Size(496, 32)
        Me.lblPaso1_5_Titulo.TabIndex = 0
        Me.lblPaso1_5_Titulo.Text = "Paso 1.5: Especifique el tipo de histórico"
        '
        'frmAsistenteImportacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 461)
        Me.Controls.Add(Me.pnlPaso1_Seleccion)
        Me.Controls.Add(Me.pnlPaso2_Cargar)
        Me.Controls.Add(Me.pnlPaso3_Validar)
        Me.Controls.Add(Me.pnlPaso4_Resumen)
        Me.Controls.Add(Me.pnlPaso1_5_SubTipo)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAsistenteImportacion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Asistente de Importación de Datos"
        Me.pnlPaso1_Seleccion.ResumeLayout(False)
        Me.pnlPaso1_Seleccion.PerformLayout()
        Me.pnlCardDotaciones.ResumeLayout(False)
        Me.pnlCardDotaciones.PerformLayout()
        Me.pnlCardHistoricos.ResumeLayout(False)
        Me.pnlCardHistoricos.PerformLayout()
        Me.pnlCardLicencias.ResumeLayout(False)
        Me.pnlCardLicencias.PerformLayout()
        Me.pnlPaso2_Cargar.ResumeLayout(False)
        Me.pnlPaso2_Cargar.PerformLayout()
        Me.pnlDropZone.ResumeLayout(False)
        Me.pnlDropZone.PerformLayout()
        Me.pnlPaso3_Validar.ResumeLayout(False)
        Me.pnlPaso3_Validar.PerformLayout()
        CType(Me.dgvPrevisualizacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlPaso4_Resumen.ResumeLayout(False)
        Me.pnlPaso4_Resumen.PerformLayout()
        Me.gbxNuevosTipos.ResumeLayout(False)
        Me.gbxNuevosFuncionarios.ResumeLayout(False)
        Me.pnlPaso1_5_SubTipo.ResumeLayout(False)
        Me.pnlPaso1_5_SubTipo.PerformLayout()
        Me.gbxSubtipo.ResumeLayout(False)
        Me.gbxSubtipo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlPaso1_Seleccion As Panel
    Friend WithEvents btnPaso1_Siguiente As Button
    Friend WithEvents pnlCardDotaciones As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents pnlCardHistoricos As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents pnlCardLicencias As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents lblPaso1_Titulo As Label
    Friend WithEvents pnlPaso2_Cargar As Panel
    Friend WithEvents btnPaso2_Procesar As Button
    Friend WithEvents btnPaso2_Volver As Button
    Friend WithEvents pnlDropZone As Panel
    Friend WithEvents lblDropZone As Label
    Friend WithEvents lblArchivoSeleccionado As Label
    Friend WithEvents btnDescargarPlantilla As Button
    Friend WithEvents lblPaso2_Titulo As Label
    Friend WithEvents pnlPaso3_Validar As Panel
    Friend WithEvents btnPaso3_Importar As Button
    Friend WithEvents btnDescargarErrores As Button
    Friend WithEvents btnPaso3_Volver As Button
    Friend WithEvents lblResumenErrores As Label
    Friend WithEvents lblResumenValidos As Label
    Friend WithEvents dgvPrevisualizacion As DataGridView
    Friend WithEvents lblPaso3_Titulo As Label
    Friend WithEvents pnlPaso4_Resumen As Panel
    Friend WithEvents btnPaso4_Finalizar As Button
    Friend WithEvents btnPaso4_OtraVez As Button
    Friend WithEvents lblResumenErroresFinal As Label
    Friend WithEvents lblResumenImportados As Label
    Friend WithEvents lblPaso4_Titulo As Label
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents SaveFileDialog1 As SaveFileDialog
    Friend WithEvents lblPaso3_Feedback As Label
    Friend WithEvents lblResumenTiempo As Label
    Friend WithEvents gbxNuevosTipos As GroupBox
    Friend WithEvents lstNuevosTiposLicencia As ListBox
    Friend WithEvents gbxNuevosFuncionarios As GroupBox
    Friend WithEvents lstNuevosFuncionarios As ListBox
    Friend WithEvents pnlPaso1_5_SubTipo As Panel
    Friend WithEvents btnPaso1_5_Siguiente As Button
    Friend WithEvents btnPaso1_5_Volver As Button
    Friend WithEvents gbxSubtipo As GroupBox
    Friend WithEvents rbNocturnidad As RadioButton
    Friend WithEvents rbPresentismo As RadioButton
    Friend WithEvents lblPaso1_5_Titulo As Label
End Class