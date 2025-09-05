' Apex/UI/frmAsistenteImportacion.vb
Imports System.Configuration
Imports System.Data
Imports System.Data.Entity.Core.EntityClient
Imports System.Data.SqlClient
Imports System.IO
Imports System.Threading
Imports ExcelDataReader

Public Class frmAsistenteImportacion

#Region "Variables y Enums del Formulario"

    Private Enum TipoImportacion
        Ninguna
        Licencias
        Presentismo
        Nocturnidad
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
        NavegarPaso(1) ' Inicia directamente en el paso 1 unificado
    End Sub

    Private Sub NavegarPaso(paso As Integer)
        ' Oculta todos los paneles principales
        pnlPaso1_Seleccion.Visible = (paso = 1)
        pnlPaso2_Cargar.Visible = (paso = 2)
        pnlPaso3_Validar.Visible = (paso = 3)
        pnlPaso4_Resumen.Visible = (paso = 4)

        ' Trae el panel del paso actual al frente
        Select Case paso
            Case 1
                pnlPaso1_Seleccion.BringToFront()
                ResetearFormulario()
            Case 2
                pnlPaso2_Cargar.BringToFront()
            Case 3
                pnlPaso3_Validar.BringToFront()
            Case 4
                pnlPaso4_Resumen.BringToFront()
        End Select
    End Sub

    Private Sub ResetearFormulario()
        importacionActual = TipoImportacion.Ninguna
        rutaArchivoSeleccionado = ""
        datosValidos = Nothing
        reporteErrores.Clear()
        newlyCreatedFuncionarios.Clear()
        newlyCreatedTiposLicencia.Clear()
        lblArchivoSeleccionado.Text = "Ningún archivo seleccionado."
        btnPaso1_Siguiente.Enabled = True
        btnPaso2_Procesar.Enabled = False
        dgvPrevisualizacion.DataSource = Nothing
        lblPaso3_Feedback.Text = ""
        gbxNuevosFuncionarios.Visible = False
        gbxNuevosTipos.Visible = False
        rbLicencias.Checked = False
        rbPresentismo.Checked = False
        rbNocturnidad.Checked = False
        rbDotaciones.Checked = False
    End Sub

#End Region

#Region "Paso 1: Selección"

    Private Sub btnPaso1_Siguiente_Click(sender As Object, e As EventArgs) Handles btnPaso1_Siguiente.Click
        If rbLicencias.Checked Then
            importacionActual = TipoImportacion.Licencias
            lblPaso2_Titulo.Text = "Paso 2: Cargar Archivo de Licencias (SGH)"
        ElseIf rbPresentismo.Checked Then
            importacionActual = TipoImportacion.Presentismo
            lblPaso2_Titulo.Text = "Paso 2: Cargar Archivo de Históricos (Presentismo)"
        ElseIf rbNocturnidad.Checked Then
            importacionActual = TipoImportacion.Nocturnidad
            lblPaso2_Titulo.Text = "Paso 2: Cargar Archivo de Históricos (Nocturnidad)"
        ElseIf rbDotaciones.Checked Then
            importacionActual = TipoImportacion.Dotaciones
            lblPaso2_Titulo.Text = "Paso 2: Cargar Archivo de Dotaciones"
        Else
            MessageBox.Show("Por favor, seleccione un tipo de archivo para importar.", "Selección Requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        MostrarEncabezadosRequeridos()
        NavegarPaso(2)
    End Sub

#End Region

#Region "Paso 2: Carga de Archivo"

    Private Sub btnPaso2_Volver_Click(sender As Object, e As EventArgs) Handles btnPaso2_Volver.Click
        NavegarPaso(1)
    End Sub
    ''' <summary>
    ''' Devuelve la lista de encabezados esperados para un tipo de importación.
    ''' </summary>
    Private Function GetExpectedHeaders(tipo As TipoImportacion) As List(Of String)
        Select Case tipo
            Case TipoImportacion.Licencias
                Return New List(Of String) From {"Unidad Ejecutora", "Unidad Organizativa", "CI", "Nombre", "Tipo de Incidencia", "Estado", "Fecha Desde", "Fecha Hasta", "Cantidad", "Cantidad dentro del período", "Unidad", "Afecta a días", "Motivo", "¿Presentó certificado?", "Usuario aprobó/anuló/rechazó", "Fecha aprobación/anulación/rechazo", "Comentario"}
            Case TipoImportacion.Presentismo
                Return New List(Of String) From {"CI", "AÑO", "MES", "INCIDENCIA", "MINUTOS", "DIAS", "OBSERVACIONES"}
            Case TipoImportacion.Nocturnidad
                Return New List(Of String) From {"CI", "AÑO", "MES", "MINUTOS"}
            Case TipoImportacion.Dotaciones
                Return New List(Of String) From {"CI", "Tipo Prenda", "Talle", "Fecha Entrega", "Observaciones"}
            Case Else
                Return New List(Of String)()
        End Select
    End Function
    Private Sub MostrarEncabezadosRequeridos()
        Dim headers As List(Of String) = GetExpectedHeaders(importacionActual)
        If headers.Any() Then
            lblEncabezados.Text = "El archivo Excel debe contener las siguientes columnas (en cualquier orden):" & Environment.NewLine & Environment.NewLine
            lblEncabezados.Text &= String.Join(", ", headers)
            gbxInstrucciones.Visible = True
        Else
            gbxInstrucciones.Visible = False
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
                                                         Case TipoImportacion.Licencias : Return ImportarLicencias(datosValidos)
                                                         Case TipoImportacion.Presentismo : Return ImportarHistoricos(datosValidos, "Presentismo")
                                                         Case TipoImportacion.Nocturnidad : Return ImportarHistoricos(datosValidos, "Nocturnidad")
                                                         Case Else : Return 0
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
            lblResumenImportados.Text = $"• Registros Afectados con Éxito: {registrosImportados}"
            lblResumenErroresFinal.Text = $"• Filas con Errores Omitidas: {reporteErrores.ToString().Split({Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).Length}"
            lblResumenTiempo.Text = $"• Tiempo Total: {stopWatch.Elapsed:g}"
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
        If stopWatch.IsRunning Then lblPaso3_Feedback.Text = $"Importando... Tiempo: {stopWatch.Elapsed:g}"
    End Sub

    Private Function LeerYValidarCabecerasExcel(ruta As String) As DataTable
        reporteErrores.Clear()
        Dim expectedHeaders As List(Of String) = GetExpectedHeaders(importacionActual)

        If Not expectedHeaders.Any() Then
            reporteErrores.AppendLine("Tipo de importación no reconocido o no implementado.")
            Return Nothing
        End If

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
                            If Not headerMap.ContainsKey(currentRowHeaders(j)) Then headerMap.Add(currentRowHeaders(j), j)
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
                If outParam.Value IsNot DBNull.Value Then Return Convert.ToInt32(outParam.Value)
            End Using
        End Using
        Return 0
    End Function

    Private Function ImportarHistoricos(dtSource As DataTable, tipo As String) As Integer
        Dim dtParaSql As DataTable = CrearDataTableParaHistoricos(dtSource, tipo)
        If dtParaSql Is Nothing OrElse dtParaSql.Rows.Count = 0 Then Return 0
        Dim efConnectionString = ConfigurationManager.ConnectionStrings("ApexEntities").ConnectionString
        Dim builder = New EntityConnectionStringBuilder(efConnectionString)
        Dim sqlConnectionString = builder.ProviderConnectionString

        Using conn As New SqlConnection(sqlConnectionString)
            Using cmd As New SqlCommand("dbo.usp_Apex_ImportarAgregadosMensuales", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@TipoHistorico", tipo)
                Dim param = cmd.Parameters.AddWithValue("@Agregados", dtParaSql)
                param.SqlDbType = SqlDbType.Structured
                param.TypeName = "dbo.TipoTablaAgregadosMensuales"
                Dim outParam = New SqlParameter("@RegistrosAfectados", SqlDbType.Int) With {.Direction = ParameterDirection.Output}
                cmd.Parameters.Add(outParam)
                conn.Open()
                cmd.ExecuteNonQuery()
                If outParam.Value IsNot DBNull.Value Then Return Convert.ToInt32(outParam.Value)
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
                Dim ci As String = NormalizarCI(sourceRow("CI").ToString())

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

    Private Function CrearDataTableParaHistoricos(dtSource As DataTable, tipo As String) As DataTable
        Dim dtTarget As New DataTable()
        dtTarget.Columns.Add("FuncionarioId", GetType(Integer))
        dtTarget.Columns.Add("Anio", GetType(Short))
        dtTarget.Columns.Add("Mes", GetType(Byte))
        dtTarget.Columns.Add("Minutos", GetType(Integer))
        dtTarget.Columns.Add("Dias", GetType(Integer))
        dtTarget.Columns.Add("Incidencia", GetType(String))
        dtTarget.Columns.Add("Observaciones", GetType(String))

        Dim funcionariosMap As Dictionary(Of String, Integer) = ObtenerMapaFuncionarios()
        Dim mapNormalizado As New Dictionary(Of String, Integer)(StringComparer.Ordinal)
        For Each kv In funcionariosMap
            Dim k = NormalizarCI(kv.Key)
            If Not mapNormalizado.ContainsKey(k) Then mapNormalizado.Add(k, kv.Value)
        Next
        funcionariosMap = mapNormalizado

        Dim datosAgrupados As New Dictionary(Of String, DataRow)(StringComparer.Ordinal)

        For Each sourceRow As DataRow In dtSource.Rows
            Try
                Dim ciNorm As String = NormalizarCI(Convert.ToString(sourceRow("CI")))
                If String.IsNullOrWhiteSpace(ciNorm) Then
                    reporteErrores.AppendLine("Fila omitida: CI vacía o inválida.")
                    Continue For
                End If

                Dim anioObj As Object = TryParseShort(Convert.ToString(sourceRow("AÑO")))
                Dim mesObj As Object = TryParseByte(Convert.ToString(sourceRow("MES")))
                If anioObj Is DBNull.Value OrElse mesObj Is DBNull.Value Then
                    reporteErrores.AppendLine($"Fila omitida (CI: {sourceRow("CI")}): AÑO o MES inválidos.")
                    Continue For
                End If
                Dim anio As Short = CType(anioObj, Short)
                Dim mes As Byte = CType(mesObj, Byte)

                Dim minutosObj As Object = TryParseInt(Convert.ToString(sourceRow("MINUTOS")))
                Dim minutos As Integer = If(minutosObj Is DBNull.Value, 0, CType(minutosObj, Integer))

                Dim fid As Integer
                If Not funcionariosMap.TryGetValue(ciNorm, fid) Then
                    Dim nuevo = CrearFuncionario(ciNorm, "(auto)")
                    If nuevo Is Nothing Then
                        reporteErrores.AppendLine($"Fila omitida: La CI '{ciNorm}' no existe y no pudo crearse.")
                        Continue For
                    End If
                    fid = nuevo.Id
                    funcionariosMap(ciNorm) = fid
                    newlyCreatedFuncionarios.Add($"{ciNorm} - (creado por importación de {tipo})")
                End If

                Dim clave As String = $"{ciNorm}-{anio}-{mes}"

                If datosAgrupados.ContainsKey(clave) Then
                    Dim filaExistente As DataRow = datosAgrupados(clave)
                    filaExistente("Minutos") = CInt(filaExistente("Minutos")) + minutos

                    If tipo = "Presentismo" Then
                        Dim diasObj As Object = TryParseInt(Convert.ToString(sourceRow("DIAS")))
                        Dim dias As Integer = If(diasObj Is DBNull.Value, 0, CType(diasObj, Integer))
                        filaExistente("Dias") = CInt(filaExistente("Dias")) + dias

                        Dim incNueva As String = If(dtSource.Columns.Contains("INCIDENCIA"), Convert.ToString(sourceRow("INCIDENCIA")), Nothing)
                        Dim obsNueva As String = If(dtSource.Columns.Contains("OBSERVACIONES"), Convert.ToString(sourceRow("OBSERVACIONES")), Nothing)

                        Dim incActual As String = If(filaExistente.IsNull("Incidencia"), "", CStr(filaExistente("Incidencia")))
                        Dim obsActual As String = If(filaExistente.IsNull("Observaciones"), "", CStr(filaExistente("Observaciones")))

                        If Not String.IsNullOrWhiteSpace(incNueva) Then
                            filaExistente("Incidencia") = If(String.IsNullOrEmpty(incActual), incNueva, incActual & "; " & incNueva)
                        End If
                        If Not String.IsNullOrWhiteSpace(obsNueva) Then
                            filaExistente("Observaciones") = If(String.IsNullOrEmpty(obsActual), obsNueva, obsActual & "; " & obsNueva)
                        End If
                    End If

                Else
                    Dim newRow As DataRow = dtTarget.NewRow()
                    newRow("FuncionarioId") = fid
                    newRow("Anio") = anio
                    newRow("Mes") = mes
                    newRow("Minutos") = minutos

                    If tipo = "Presentismo" Then
                        Dim diasObj As Object = TryParseInt(Convert.ToString(sourceRow("DIAS")))
                        newRow("Dias") = If(diasObj Is DBNull.Value, 0, CType(diasObj, Integer))
                        newRow("Incidencia") = If(dtSource.Columns.Contains("INCIDENCIA"), Convert.ToString(sourceRow("INCIDENCIA")), Nothing)
                        newRow("Observaciones") = If(dtSource.Columns.Contains("OBSERVACIONES"), Convert.ToString(sourceRow("OBSERVACIONES")), Nothing)
                    Else
                        newRow("Dias") = DBNull.Value
                        newRow("Incidencia") = DBNull.Value
                        newRow("Observaciones") = DBNull.Value
                    End If

                    datosAgrupados.Add(clave, newRow)
                End If

            Catch ex As Exception
                reporteErrores.AppendLine($"Fila omitida (CI: {sourceRow("CI")}): Error - {ex.Message}")
            End Try
        Next

        For Each row As DataRow In datosAgrupados.Values
            dtTarget.Rows.Add(row)
        Next

        Return dtTarget
    End Function

    Private Function TryParseInt(value As String) As Object
        Dim number As Integer
        If Integer.TryParse(value, number) Then Return number
        Return DBNull.Value
    End Function

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

    Private Function ObtenerMapaFuncionarios() As Dictionary(Of String, Integer)
        Using ctx As New ApexEntities()
            Return ctx.Funcionario.ToDictionary(Function(f) NormalizarCI(f.CI), Function(f) f.Id)
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
                Dim nuevoFuncionario As New Funcionario With {.CI = ci, .Nombre = nombre, .FechaIngreso = New Date(1900, 1, 1), .TipoFuncionarioId = 1, .Activo = True, .CreatedAt = DateTime.Now, .CargoId = 1, .SeccionId = 1, .PuestoTrabajoId = 1, .TurnoId = 1, .SemanaId = 1, .HorarioId = 1
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

    Private Function NormalizarCI(raw As String) As String
        If String.IsNullOrWhiteSpace(raw) Then Return ""
        Dim digits = New String(raw.Where(AddressOf Char.IsDigit).ToArray())
        Return digits
    End Function

    Private Function ImportarDotaciones(dt As DataTable) As Integer
        MessageBox.Show("Funcionalidad no implementada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return 0
    End Function
#End Region

End Class