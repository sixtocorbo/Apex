' Apex/UI/frmAsistenteImportacion.vb
Imports System.Configuration
Imports System.Data
Imports System.Data.Entity.Core.EntityClient
Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading
Imports ExcelDataReader ' Asegúrate de tener esta librería desde NuGet

Public Class frmAsistenteImportacion

#Region "Variables y Enums del Formulario"
    Private Enum TipoImportacion
        Ninguna
        Licencias
        Historicos
        Dotaciones
    End Enum

    Private importacionActual As TipoImportacion = TipoImportacion.Ninguna
    Private rutaArchivoSeleccionado As String = ""
    Private datosValidos As DataTable
    Private reporteErrores As New System.Text.StringBuilder()
    Private stopWatch As New Stopwatch()
    Private WithEvents Temporizador As New System.Windows.Forms.Timer()
    Private newlyCreatedFuncionarios As New List(Of String)()
    Private newlyCreatedTiposLicencia As New List(Of String)()
#End Region

#Region "Navegación y Lógica de Pasos"
    Private Sub frmAsistenteImportacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Temporizador.Interval = 100
        NavegarPaso(1)
    End Sub

    Private Sub NavegarPaso(paso As Integer)
        pnlPaso1_Seleccion.Visible = (paso = 1)
        pnlPaso2_Cargar.Visible = (paso = 2)
        pnlPaso3_Validar.Visible = (paso = 3)
        pnlPaso4_Resumen.Visible = (paso = 4)
        If paso = 1 Then ResetearFormulario()
    End Sub

    Private Sub ResetearFormulario()
        importacionActual = TipoImportacion.Ninguna
        rutaArchivoSeleccionado = ""
        datosValidos = Nothing
        reporteErrores.Clear()
        newlyCreatedFuncionarios.Clear()
        newlyCreatedTiposLicencia.Clear()
        DeseleccionarCards()
        lblArchivoSeleccionado.Text = "Ningún archivo seleccionado."
        btnPaso1_Siguiente.Enabled = False
        btnPaso2_Procesar.Enabled = False
        dgvPrevisualizacion.DataSource = Nothing
        lblPaso3_Feedback.Text = ""
        gbxNuevosFuncionarios.Visible = False
        gbxNuevosTipos.Visible = False
    End Sub
#End Region

#Region "Paso 1: Selección"
    Private Sub pnlCard_Click(sender As Object, e As EventArgs) Handles pnlCardLicencias.Click, pnlCardHistoricos.Click, pnlCardDotaciones.Click
        DeseleccionarCards()
        Dim pnlSeleccionado As Panel = CType(sender, Panel)
        pnlSeleccionado.BackColor = Color.LightSteelBlue

        If pnlSeleccionado Is pnlCardLicencias Then
            importacionActual = TipoImportacion.Licencias
        ElseIf pnlSeleccionado Is pnlCardHistoricos Then
            importacionActual = TipoImportacion.Historicos
        ElseIf pnlSeleccionado Is pnlCardDotaciones Then
            importacionActual = TipoImportacion.Dotaciones
        End If
        btnPaso1_Siguiente.Enabled = True
    End Sub

    Private Sub DeseleccionarCards()
        pnlCardLicencias.BackColor = Color.White
        pnlCardHistoricos.BackColor = Color.White
        pnlCardDotaciones.BackColor = Color.White
    End Sub

    Private Sub btnPaso1_Siguiente_Click(sender As Object, e As EventArgs) Handles btnPaso1_Siguiente.Click
        Select Case importacionActual
            Case TipoImportacion.Licencias
                lblPaso2_Titulo.Text = "Paso 2: Cargar Archivo de Licencias (SGH)"
                btnDescargarPlantilla.Text = "⬇️ Descargar Plantilla_Licencias.xlsx"
            Case TipoImportacion.Historicos
                lblPaso2_Titulo.Text = "Paso 2: Cargar Archivo de Históricos (Presentismo)"
                btnDescargarPlantilla.Text = "⬇️ Descargar Plantilla_Presentismo.xlsx"
            Case TipoImportacion.Dotaciones
                lblPaso2_Titulo.Text = "Paso 2: Cargar Archivo de Dotaciones"
                btnDescargarPlantilla.Text = "⬇️ Descargar Plantilla_Dotaciones.xlsx"
        End Select
        NavegarPaso(2)
    End Sub
#End Region

#Region "Paso 2: Carga de Archivo"

    Private Sub btnPaso2_Volver_Click(sender As Object, e As EventArgs) Handles btnPaso2_Volver.Click
        NavegarPaso(1)
    End Sub

    Private Sub btnDescargarPlantilla_Click(sender As Object, e As EventArgs) Handles btnDescargarPlantilla.Click
        SaveFileDialog1.FileName = btnDescargarPlantilla.Text.Replace("⬇️ Descargar ", "")
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            ' Lógica para copiar la plantilla desde los recursos del proyecto
            ' Ejemplo: File.WriteAllBytes(SaveFileDialog1.FileName, My.Resources.Plantilla_Licencias)
            MessageBox.Show($"Plantilla guardada en: {SaveFileDialog1.FileName}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub pnlDropZone_Click(sender As Object, e As EventArgs) Handles pnlDropZone.Click, lblDropZone.Click
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            rutaArchivoSeleccionado = OpenFileDialog1.FileName
            lblArchivoSeleccionado.Text = $"Archivo: {Path.GetFileName(rutaArchivoSeleccionado)}"
            btnPaso2_Procesar.Enabled = True
        End If
    End Sub

    Private Sub pnlDropZone_DragEnter(sender As Object, e As DragEventArgs) Handles pnlDropZone.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub pnlDropZone_DragDrop(sender As Object, e As DragEventArgs) Handles pnlDropZone.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        If files.Length > 0 And (Path.GetExtension(files(0)).ToLower() = ".xlsx" Or Path.GetExtension(files(0)).ToLower() = ".xls") Then
            rutaArchivoSeleccionado = files(0)
            lblArchivoSeleccionado.Text = $"Archivo: {Path.GetFileName(rutaArchivoSeleccionado)}"
            btnPaso2_Procesar.Enabled = True
        Else
            MessageBox.Show("Por favor, arrastre un archivo de Excel válido (.xlsx o .xls).", "Formato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnPaso2_Procesar_Click(sender As Object, e As EventArgs) Handles btnPaso2_Procesar.Click
        Me.Cursor = Cursors.WaitCursor
        reporteErrores.Clear()
        Try
            Dim dt As DataTable = LeerYValidarCabecerasExcel(rutaArchivoSeleccionado)
            If dt Is Nothing Then
                MessageBox.Show(reporteErrores.ToString(), "Error de Formato de Archivo", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            ValidarDatos(dt)
            lblResumenValidos.Text = $"✔️ Registros Válidos: {datosValidos.Rows.Count}"
            Dim totalErrores As Integer = (dt.Rows.Count - datosValidos.Rows.Count) + reporteErrores.ToString().Split({Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).Length
            lblResumenErrores.Text = $"❌ Errores Encontrados: {totalErrores}"
            btnDescargarErrores.Visible = (totalErrores > 0)
            btnPaso3_Importar.Enabled = (datosValidos.Rows.Count > 0)
            btnPaso3_Importar.Text = $"✅ Confirmar e Importar {datosValidos.Rows.Count} Registros"
            dgvPrevisualizacion.DataSource = datosValidos
            NavegarPaso(3)
        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al procesar el archivo: {ex.Message}", "Error de Procesamiento", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region "Paso 3: Validación"

    Private Sub btnPaso3_Volver_Click(sender As Object, e As EventArgs) Handles btnPaso3_Volver.Click
        NavegarPaso(2)
    End Sub

    Private Sub btnDescargarErrores_Click(sender As Object, e As EventArgs) Handles btnDescargarErrores.Click
        SaveFileDialog1.FileName = "ReporteDeErrores.txt"
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            File.WriteAllText(SaveFileDialog1.FileName, reporteErrores.ToString())
            MessageBox.Show("Reporte de errores guardado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Async Sub btnPaso3_Importar_Click(sender As Object, e As EventArgs) Handles btnPaso3_Importar.Click
        Me.Cursor = Cursors.WaitCursor
        btnPaso3_Importar.Enabled = False
        btnPaso3_Volver.Enabled = False
        stopWatch.Restart()
        Temporizador.Start()

        Dim registrosImportados As Integer = 0
        Dim errorOcurrido As Exception = Nothing

        Try
            registrosImportados = Await Task.Run(Function()
                                                     Select Case importacionActual
                                                         Case TipoImportacion.Licencias
                                                             Return ImportarLicencias(datosValidos)
                                                         Case TipoImportacion.Historicos
                                                             Return ImportarHistoricos(datosValidos) ' <-- Llamada a la nueva función
                                                         Case TipoImportacion.Dotaciones
                                                             Return ImportarDotaciones(datosValidos)
                                                         Case Else
                                                             Return 0
                                                     End Select
                                                 End Function)
        Catch ex As Exception
            errorOcurrido = ex
        Finally
            stopWatch.Stop()
            Temporizador.Stop()
            Me.Cursor = Cursors.Default
            btnPaso3_Importar.Enabled = True
            btnPaso3_Volver.Enabled = True
        End Try

        If errorOcurrido IsNot Nothing Then
            MessageBox.Show($"Error al importar los datos: {errorOcurrido.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            lblResumenImportados.Text = $"• Registros Importados con Éxito: {registrosImportados}"
            lblResumenErroresFinal.Text = $"• Registros con Errores Omitidos: {reporteErrores.ToString().Split({Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).Length}"
            Dim tiempoTotal As String = stopWatch.Elapsed.ToString("g")
            lblResumenTiempo.Text = $"• Tiempo Total de la Operación: {tiempoTotal}"

            ' Mostrar listas de nuevos registros si los hay
            If newlyCreatedFuncionarios.Any() Then
                gbxNuevosFuncionarios.Visible = True
                lstNuevosFuncionarios.DataSource = newlyCreatedFuncionarios
            End If
            If newlyCreatedTiposLicencia.Any() Then
                gbxNuevosTipos.Visible = True
                lstNuevosTiposLicencia.DataSource = newlyCreatedTiposLicencia
            End If

            NavegarPaso(4)
        End If
    End Sub
#End Region

#Region "Paso 4: Resumen"

    Private Sub btnPaso4_OtraVez_Click(sender As Object, e As EventArgs) Handles btnPaso4_OtraVez.Click
        NavegarPaso(1)
    End Sub

    Private Sub btnPaso4_Finalizar_Click(sender As Object, e As EventArgs) Handles btnPaso4_Finalizar.Click
        Me.Close()
    End Sub
#End Region

#Region "Lógica de Negocio, Excel y Cronómetro"

    Private Sub Temporizador_Tick(sender As Object, e As EventArgs) Handles Temporizador.Tick
        If stopWatch.IsRunning Then
            Dim tiempoTranscurrido As String = stopWatch.Elapsed.ToString("g")
            lblPaso3_Feedback.Text = $"Importando... Tiempo transcurrido: {tiempoTranscurrido}"
        End If
    End Sub

    Private Function LeerYValidarCabecerasExcel(ruta As String) As DataTable
        reporteErrores.Clear()
        Dim expectedHeaders As List(Of String) = Nothing

        Select Case importacionActual
            Case TipoImportacion.Licencias
                expectedHeaders = New List(Of String) From {"Unidad Ejecutora", "Unidad Organizativa", "CI", "Nombre", "Tipo de Incidencia", "Estado", "Fecha Desde", "Fecha Hasta", "Cantidad", "Cantidad dentro del período", "Unidad", "Afecta a días", "Motivo", "¿Presentó certificado?", "Usuario aprobó/anuló/rechazó", "Fecha aprobación/anulación/rechazo", "Comentario"}
            Case TipoImportacion.Historicos
                expectedHeaders = New List(Of String) From {"CI", "AÑO", "MES", "INCIDENCIA", "MINUTOS", "DIAS", "OBSERVACIONES"}
            Case Else
                reporteErrores.AppendLine("Tipo de importación no reconocido o no implementado.")
                Return Nothing
        End Select

        Using stream = File.Open(ruta, FileMode.Open, FileAccess.Read)
            Using reader As IExcelDataReader = ExcelReaderFactory.CreateReader(stream)
                Dim ds = reader.AsDataSet(New ExcelDataSetConfiguration() With {.ConfigureDataTable = Function(__) New ExcelDataTableConfiguration() With {.UseHeaderRow = False}})
                Dim dtExcel = ds.Tables(0)
                Dim headerRowIndex = -1
                Dim headerMap As New Dictionary(Of String, Integer)

                For i = 0 To Math.Min(dtExcel.Rows.Count - 1, 20)
                    Dim currentRowHeaders = dtExcel.Rows(i).ItemArray.Select(Function(item) item.ToString().Trim()).ToList()
                    If Not expectedHeaders.Except(currentRowHeaders).Any() Then
                        headerRowIndex = i
                        For j = 0 To currentRowHeaders.Count - 1
                            If Not headerMap.ContainsKey(currentRowHeaders(j)) Then
                                headerMap.Add(currentRowHeaders(j), j)
                            End If
                        Next
                        Exit For
                    End If
                Next

                If headerRowIndex = -1 Then
                    reporteErrores.AppendLine("No se encontró la fila de encabezados correcta. Verifique el archivo.")
                    Return Nothing
                End If

                Dim dtFinal As New DataTable()
                For Each header In expectedHeaders
                    dtFinal.Columns.Add(header)
                Next

                For i = headerRowIndex + 1 To dtExcel.Rows.Count - 1
                    Dim originalRow = dtExcel.Rows(i)
                    If originalRow.ItemArray.All(Function(cell) cell Is DBNull.Value OrElse String.IsNullOrWhiteSpace(cell.ToString())) Then Continue For
                    Dim newRow = dtFinal.NewRow()
                    For Each header In expectedHeaders
                        newRow(header) = originalRow(headerMap(header))
                    Next
                    dtFinal.Rows.Add(newRow)
                Next
                Return dtFinal
            End Using
        End Using
    End Function

    Private Sub ValidarDatos(dtOriginal As DataTable)
        datosValidos = dtOriginal.Clone()
        For i = 0 To dtOriginal.Rows.Count - 1
            Dim fila = dtOriginal.Rows(i)
            If String.IsNullOrWhiteSpace(fila("CI").ToString()) Then
                reporteErrores.AppendLine($"Fila {i + 2}: La CI está vacía y ha sido omitida.")
            Else
                datosValidos.ImportRow(fila)
            End If
        Next
    End Sub

    Private Function ImportarLicencias(dtSource As DataTable) As Integer
        Dim dtParaSql As DataTable = CrearDataTableParaLicencias(dtSource)
        If dtParaSql Is Nothing OrElse dtParaSql.Rows.Count = 0 Then Return 0

        Dim efConnectionString As String = ConfigurationManager.ConnectionStrings("ApexEntities").ConnectionString
        Dim builder As New EntityConnectionStringBuilder(efConnectionString)
        Dim sqlConnectionString As String = builder.ProviderConnectionString

        Using conn As New SqlConnection(sqlConnectionString)
            Using cmd As New SqlCommand("dbo.usp_Apex_ImportarLicenciasMasivas", conn)
                cmd.CommandType = CommandType.StoredProcedure
                Dim param As SqlParameter = cmd.Parameters.AddWithValue("@Licencias", dtParaSql)
                param.SqlDbType = SqlDbType.Structured
                param.TypeName = "dbo.TipoTablaLicencia"

                Dim outParam As New SqlParameter("@RegistrosAfectados", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
                cmd.Parameters.Add(outParam)

                conn.Open()
                cmd.ExecuteNonQuery()

                If outParam.Value IsNot DBNull.Value Then
                    Return Convert.ToInt32(outParam.Value)
                End If
            End Using
        End Using
        Return 0
    End Function

    Private Function CrearDataTableParaLicencias(dtSource As DataTable) As DataTable
        Dim dtTarget As New DataTable()
        dtTarget.Columns.Add("FuncionarioId", GetType(Integer))
        dtTarget.Columns.Add("TipoLicenciaId", GetType(Integer))
        dtTarget.Columns.Add("inicio", GetType(Date))
        dtTarget.Columns.Add("finaliza", GetType(Date))
        dtTarget.Columns.Add("fecha_registro", GetType(Date))
        dtTarget.Columns.Add("fecha_actualizado", GetType(Date))
        dtTarget.Columns.Add("usuario", GetType(String))
        dtTarget.Columns.Add("datos", GetType(String))
        dtTarget.Columns.Add("estado", GetType(String))
        dtTarget.Columns.Add("Comentario", GetType(String))
        dtTarget.Columns.Add("Unidad_Ejecutora", GetType(String))
        dtTarget.Columns.Add("Unidad_Organizativa", GetType(String))
        dtTarget.Columns.Add("Cantidad", GetType(Integer))
        dtTarget.Columns.Add("Cantidad_dentro_del_período", GetType(Integer))
        dtTarget.Columns.Add("Unidad", GetType(String))
        dtTarget.Columns.Add("Afecta_a_días", GetType(String))
        dtTarget.Columns.Add("C_Presentó_certificado_", GetType(String))
        dtTarget.Columns.Add("Usuario_aprobó_anuló_rechazó", GetType(String))
        dtTarget.Columns.Add("Fecha_aprobación_anulación_rechazo", GetType(String))

        Dim funcionariosMap As Dictionary(Of String, Integer) = ObtenerMapaFuncionarios()
        Dim licenciasMap As Dictionary(Of String, Integer) = ObtenerMapaTiposLicencia()

        For Each sourceRow As DataRow In dtSource.Rows
            Try
                Dim ci As String = sourceRow("CI").ToString().Trim()
                Dim tipoLicenciaNombre As String = sourceRow("Tipo de Incidencia").ToString().Trim()
                Dim nombreFuncionario As String = sourceRow("Nombre").ToString().Trim()

                If Not funcionariosMap.ContainsKey(ci) Then
                    Dim nuevoFuncionario = CrearFuncionario(ci, nombreFuncionario)
                    If nuevoFuncionario IsNot Nothing Then
                        funcionariosMap.Add(ci, nuevoFuncionario.Id)
                        newlyCreatedFuncionarios.Add($"{ci} - {nombreFuncionario}")
                    Else
                        reporteErrores.AppendLine($"Fila omitida: No se pudo crear el funcionario con CI '{ci}'.")
                        Continue For
                    End If
                End If

                If Not licenciasMap.ContainsKey(tipoLicenciaNombre) Then
                    Dim nuevoTipo = CrearTipoLicencia(tipoLicenciaNombre)
                    If nuevoTipo IsNot Nothing Then
                        licenciasMap.Add(tipoLicenciaNombre, nuevoTipo.Id)
                        newlyCreatedTiposLicencia.Add(tipoLicenciaNombre)
                    Else
                        reporteErrores.AppendLine($"Fila omitida (CI: {ci}): No se pudo crear el tipo de licencia '{tipoLicenciaNombre}'.")
                        Continue For
                    End If
                End If

                Dim newRow = dtTarget.NewRow()
                newRow("FuncionarioId") = funcionariosMap(ci)
                newRow("TipoLicenciaId") = licenciasMap(tipoLicenciaNombre)
                newRow("inicio") = Convert.ToDateTime(sourceRow("Fecha Desde"))
                newRow("finaliza") = Convert.ToDateTime(sourceRow("Fecha Hasta"))
                newRow("fecha_registro") = DateTime.Now
                newRow("fecha_actualizado") = DateTime.Now
                newRow("usuario") = "SISTEMA_IMPORT"
                newRow("datos") = sourceRow("Motivo")?.ToString()
                newRow("estado") = sourceRow("Estado")?.ToString()
                newRow("Comentario") = sourceRow("Comentario")?.ToString()
                newRow("Unidad_Ejecutora") = sourceRow("Unidad Ejecutora")?.ToString()
                newRow("Unidad_Organizativa") = sourceRow("Unidad Organizativa")?.ToString()
                newRow("Cantidad") = TryParseInt(sourceRow("Cantidad").ToString())
                newRow("Cantidad_dentro_del_período") = TryParseInt(sourceRow("Cantidad dentro del período").ToString())
                newRow("Unidad") = sourceRow("Unidad")?.ToString()
                newRow("Afecta_a_días") = sourceRow("Afecta a días")?.ToString()
                newRow("C_Presentó_certificado_") = sourceRow("¿Presentó certificado?")?.ToString()
                newRow("Usuario_aprobó_anuló_rechazó") = sourceRow("Usuario aprobó/anuló/rechazó")?.ToString()
                newRow("Fecha_aprobación_anulación_rechazo") = sourceRow("Fecha aprobación/anulación/rechazo")?.ToString()
                dtTarget.Rows.Add(newRow)
            Catch ex As Exception
                reporteErrores.AppendLine($"Fila omitida (CI: {sourceRow("CI")}): Error al procesar - {ex.Message}")
            End Try
        Next
        Return dtTarget
    End Function

    Private Function TryParseInt(value As String) As Object
        Dim number As Integer
        If Integer.TryParse(value, number) Then Return number
        Return DBNull.Value
    End Function

    Private Function ObtenerMapaFuncionarios() As Dictionary(Of String, Integer)
        Using ctx As New ApexEntities()
            Return ctx.Funcionario.ToDictionary(Function(f) f.CI.Trim(), Function(f) f.Id)
        End Using
    End Function

    Private Function ObtenerMapaTiposLicencia() As Dictionary(Of String, Integer)
        Using ctx As New ApexEntities()
            Return ctx.TipoLicencia.ToDictionary(Function(t) t.Nombre.Trim(), Function(t) t.Id)
        End Using
    End Function

    Private Function CrearFuncionario(ci As String, nombre As String) As Funcionario
        Try
            Using ctx As New ApexEntities()
                Dim nuevoFuncionario As New Funcionario With {
                    .CI = ci, .Nombre = nombre, .FechaIngreso = New Date(1900, 1, 1),
                    .TipoFuncionarioId = 1, .Activo = True, .CreatedAt = DateTime.Now,
                    .CargoId = 1, .EstadoId = 1, .SeccionId = 1, .PuestoTrabajoId = 1,
                    .TurnoId = 1, .SemanaId = 1, .HorarioId = 1
                }
                ctx.Funcionario.Add(nuevoFuncionario)
                ctx.SaveChanges()
                Return nuevoFuncionario
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function CrearTipoLicencia(nombreLicencia As String) As TipoLicencia
        Try
            Using ctx As New ApexEntities()
                Dim nuevoTipo As New TipoLicencia With {
                    .Nombre = nombreLicencia, .CreatedAt = DateTime.Now, .EsAusencia = True,
                    .SuspendeViatico = True, .AfectaPresentismo = True, .EsHabil = False,
                    .CategoriaAusenciaId = 1
                }
                ctx.TipoLicencia.Add(nuevoTipo)
                ctx.SaveChanges()
                Return nuevoTipo
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function ImportarHistoricos(dtSource As DataTable) As Integer
        Dim dtParaSql As DataTable = CrearDataTableParaHistoricos(dtSource)
        If dtParaSql Is Nothing OrElse dtParaSql.Rows.Count = 0 Then Return 0

        Dim efConnectionString As String = ConfigurationManager.ConnectionStrings("ApexEntities").ConnectionString
        Dim builder As New EntityConnectionStringBuilder(efConnectionString)
        Dim sqlConnectionString As String = builder.ProviderConnectionString

        Using conn As New SqlConnection(sqlConnectionString)
            Using cmd As New SqlCommand("dbo.usp_Apex_ImportarAgregadosMensuales", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@TipoHistorico", "Presentismo")
                Dim param As SqlParameter = cmd.Parameters.AddWithValue("@Agregados", dtParaSql)
                param.SqlDbType = SqlDbType.Structured
                param.TypeName = "dbo.TipoTablaAgregadosMensuales"

                Dim outParam As New SqlParameter("@RegistrosAfectados", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
                cmd.Parameters.Add(outParam)

                conn.Open()
                cmd.ExecuteNonQuery()

                If outParam.Value IsNot DBNull.Value Then
                    Return Convert.ToInt32(outParam.Value)
                End If
            End Using
        End Using
        Return 0
    End Function
    Private Function CrearDataTableParaHistoricos(dtSource As DataTable) As DataTable
        Dim dtTarget As New DataTable()
        dtTarget.Columns.Add("FuncionarioId", GetType(Integer))
        dtTarget.Columns.Add("Anio", GetType(Short))
        dtTarget.Columns.Add("Mes", GetType(Byte))
        dtTarget.Columns.Add("Minutos", GetType(Integer))
        dtTarget.Columns.Add("Dias", GetType(Integer))
        dtTarget.Columns.Add("Incidencia", GetType(String))
        dtTarget.Columns.Add("Observaciones", GetType(String))

        Dim funcionariosMap As Dictionary(Of String, Integer) = ObtenerMapaFuncionarios()

        For Each sourceRow As DataRow In dtSource.Rows
            Try
                Dim ci As String = sourceRow("CI").ToString().Trim()

                If Not funcionariosMap.ContainsKey(ci) Then
                    ' Para históricos, asumimos que el funcionario DEBE existir. No lo creamos.
                    reporteErrores.AppendLine($"Fila omitida: La Cédula '{ci}' no fue encontrada en la base de datos.")
                    Continue For
                End If

                Dim newRow = dtTarget.NewRow()
                newRow("FuncionarioId") = funcionariosMap(ci)
                newRow("Anio") = TryParseShort(sourceRow("AÑO").ToString())
                newRow("Mes") = TryParseByte(sourceRow("MES").ToString())
                newRow("Minutos") = TryParseInt(sourceRow("MINUTOS").ToString())
                newRow("Dias") = TryParseInt(sourceRow("DIAS").ToString())
                newRow("Incidencia") = sourceRow("INCIDENCIA")?.ToString()
                newRow("Observaciones") = sourceRow("OBSERVACIONES")?.ToString()
                dtTarget.Rows.Add(newRow)
            Catch ex As Exception
                reporteErrores.AppendLine($"Fila omitida (CI: {sourceRow("CI")}): Error al procesar - {ex.Message}")
            End Try
        Next
        Return dtTarget
    End Function
    ' --- NUEVAS FUNCIONES AUXILIARES PARA CONVERSIÓN SEGURA ---
    Private Function TryParseShort(value As String) As Object
        Dim number As Short
        If Short.TryParse(value, number) Then Return number
        Return DBNull.Value
    End Function

    Private Function TryParseByte(value As String) As Object
        Dim number As Byte
        If Byte.TryParse(value, number) Then Return number
        Return DBNull.Value
    End Function
    Private Function ImportarDotaciones(dt As DataTable) As Integer
        MessageBox.Show("Funcionalidad no implementada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return 0
    End Function
#End Region

End Class