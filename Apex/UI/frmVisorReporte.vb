Imports System.Collections.Generic
Imports System.Linq
Imports System.Data
Imports System.Text
Imports System.Xml
Imports Microsoft.Reporting.WinForms

Public Class frmVisorReporte

    Private ReadOnly _titulo As String
    Private ReadOnly _filtros As String
    Private ReadOnly _cantidades As String
    Private ReadOnly _dtResultados As DataTable
    Private ReadOnly _subtitulo As String


    Public Sub New(titulo As String, filtrosAplicados As String, cantidadesDisponibles As String, dtResultados As DataTable)
        InitializeComponent()
        _titulo = titulo
        _filtros = filtrosAplicados
        _cantidades = cantidadesDisponibles
        _dtResultados = dtResultados
        _subtitulo = ObtenerSubtituloCantidades(cantidadesDisponibles)
    End Sub

    Private Sub frmVisorReporte_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = _titulo
        ConfigurarYMostrarReporte()
    End Sub

    Private Sub ConfigurarYMostrarReporte()
        Try
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            Dim lr = ReportViewer1.LocalReport

            Dim definitionContent = ReportResourceLoader.GetReportDefinitionContent(Me.GetType(),
                                                                                     "Apex.Reportes.ReporteFiltros.rdlc",
                                                                                     "ReporteFiltros.rdlc")

            If definitionContent.Definition.Source = ReportDefinitionSource.Embedded AndAlso Not String.IsNullOrWhiteSpace(definitionContent.Definition.ResourceName) Then
                lr.ReportEmbeddedResource = definitionContent.Definition.ResourceName
            Else
                lr.ReportEmbeddedResource = Nothing
            End If

            ' Cargar el XML del RDLC base en un documento para manipularlo
            Dim rdlcXml As New XmlDocument()
            Using stream As New IO.MemoryStream(definitionContent.Content)
                rdlcXml.Load(stream)
            End Using

            ' Generar dinámicamente el Tablix (tabla) y los campos del DataSet
            GenerarContenidoDinamico(rdlcXml, _dtResultados)

            ' Cargar la definición del reporte modificada desde el string XML
            Using ms As New IO.MemoryStream(Encoding.UTF8.GetBytes(rdlcXml.OuterXml))
                lr.LoadReportDefinition(ms)
            End Using

            ' Limpiar y agregar el origen de datos
            lr.DataSources.Clear()
            lr.DataSources.Add(New ReportDataSource("ResultadosDataSet", _dtResultados))

            ' Establecer los parámetros
            lr.SetParameters(New ReportParameter("ReportTitle", _titulo))
            lr.SetParameters(New ReportParameter("FiltrosAplicados", _filtros))
            lr.SetParameters(New ReportParameter("CantidadesDisponibles", _cantidades))
            lr.SetParameters(New ReportParameter("ReportSubTitle", _subtitulo))

            ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            ReportViewer1.ZoomMode = ZoomMode.PageWidth
            ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error de Reporte", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GenerarContenidoDinamico(rdlcXml As XmlDocument, dt As DataTable)
        Dim nsManager As New XmlNamespaceManager(rdlcXml.NameTable)
        nsManager.AddNamespace("df", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition")

        Dim dataSetNode = rdlcXml.SelectSingleNode("/df:Report/df:DataSets/df:DataSet[@Name='ResultadosDataSet']", nsManager)
        If dataSetNode Is Nothing Then
            Throw New Exception("No se pudo encontrar el nodo 'DataSet' en la definición del reporte.")
        End If

        ' 1. Crear campos del DataSet
        Dim existingFieldsNode = dataSetNode.SelectSingleNode("df:Fields", nsManager)
        If existingFieldsNode IsNot Nothing Then
            dataSetNode.RemoveChild(existingFieldsNode)
        End If

        Dim fieldsNode = rdlcXml.CreateElement("Fields", nsManager.LookupNamespace("df"))
        For Each col As DataColumn In dt.Columns
            Dim fieldNode = rdlcXml.CreateElement("Field", nsManager.LookupNamespace("df"))
            fieldNode.SetAttribute("Name", col.ColumnName)
            Dim dataFieldNode = rdlcXml.CreateElement("DataField", nsManager.LookupNamespace("df"))
            dataFieldNode.InnerText = col.ColumnName
            fieldNode.AppendChild(dataFieldNode)
            fieldsNode.AppendChild(fieldNode)
        Next
        dataSetNode.AppendChild(fieldsNode)

        ' 2. Crear el Tablix (Tabla de resultados)
        ' -- INICIO DE LA CORRECCIÓN --
        ' Primero, intenta buscar el nodo 'ReportItems' en la estructura más nueva (con ReportSections)
        Dim bodyNode As XmlNode = rdlcXml.SelectSingleNode("/df:Report/df:ReportSections/df:ReportSection/df:Body/df:ReportItems", nsManager)

        ' Si no lo encuentra, busca en la estructura más antigua (la que estamos usando)
        If bodyNode Is Nothing Then
            bodyNode = rdlcXml.SelectSingleNode("/df:Report/df:Body/df:ReportItems", nsManager)
        End If

        ' Si después de ambos intentos no se encuentra el nodo, es un error.
        If bodyNode Is Nothing Then
            Throw New Exception("No se pudo encontrar el nodo 'ReportItems' en la definición del reporte. La estructura del RDLC puede ser inválida.")
        End If
        ' -- FIN DE LA CORRECCIÓN --

        Dim existingTablix = bodyNode.SelectSingleNode("df:Tablix[@Name='TablixResultados']", nsManager)
        If existingTablix IsNot Nothing Then
            bodyNode.RemoveChild(existingTablix)
        End If

        Dim tablixXml As String = GenerarTablixXml(dt.Columns, nsManager)
        Dim tablixDoc As New XmlDocument()
        tablixDoc.LoadXml(tablixXml)
        Dim tablixNode = rdlcXml.ImportNode(tablixDoc.DocumentElement, True)
        bodyNode.AppendChild(tablixNode)
    End Sub

    Private Function GenerarTablixXml(columns As DataColumnCollection, nsManager As XmlNamespaceManager) As String
        Dim sb As New StringBuilder()
        Dim ns As String = nsManager.LookupNamespace("df")

        ' --- INICIO DE LA MODIFICACIÓN ---
        ' 1. Lista de columnas a ignorar en la tabla principal
        Dim columnasAIgnorar As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {"GlobalSearch"}

        ' 2. Filtramos las columnas que sí queremos mostrar
        Dim columnasAMostrar = columns.Cast(Of DataColumn)().
                                Where(Function(c) Not columnasAIgnorar.Contains(c.ColumnName)).
                                ToList()

        If columnasAMostrar.Count = 0 Then
            Throw New InvalidOperationException("No se encontraron columnas para construir el reporte.")
        End If

        Dim widthOverrides As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
            {"NombreCompleto", "2.8in"},
            {"NombreFuncionario", "2.8in"},
            {"Nombre", "2.5in"},
            {"Cedula", "1.2in"},
            {"CI", "1.2in"},
            {"Resumen", "3.5in"},
            {"Texto", "3.5in"},
            {"Observaciones", "3.7in"},
            {"Descripcion", "3.2in"},
            {"Detalle", "3.2in"},
            {"Motivo", "2.5in"},
            {"PuestoDeTrabajo", "2.5in"},
            {"Cargo", "2.0in"},
            {"Seccion", "2.0in"},
            {"Seccin", "2.0in"},
            {"TipoDeFuncionario", "2.2in"},
            {"Estado", "1.5in"},
            {"Tipo", "1.6in"},
            {"Oficina", "1.8in"},
            {"Rango", "2.2in"},
            {"Documento", "2.2in"},
            {"ExpMinisterial", "2.2in"},
            {"ExpINR", "2.2in"}
        }
        ' --- FIN DE LA MODIFICACIÓN ---

        sb.AppendLine($"<Tablix Name='TablixResultados' xmlns='{ns}'>")
        sb.AppendLine("<TablixBody>")
        sb.AppendLine("<TablixColumns>")

        ' --- MODIFICADO: Usar la lista filtrada y anchos específicos ---
        For Each col As DataColumn In columnasAMostrar
            Dim width As String
            Dim headerKey = If(String.IsNullOrWhiteSpace(col.Caption), col.ColumnName, col.Caption)
            If Not widthOverrides.TryGetValue(col.ColumnName, width) AndAlso
               Not widthOverrides.TryGetValue(headerKey, width) Then
                width = "1.2in"
            End If
            sb.AppendLine($"<TablixColumn><Width>{width}</Width></TablixColumn>")
        Next
        sb.AppendLine("</TablixColumns>")

        sb.AppendLine("<TablixRows>")
        ' Fila de encabezado
        sb.AppendLine("<TablixRow><Height>0.25in</Height><TablixCells>")
        ' --- MODIFICADO: Usar la lista filtrada ---
        For Each col As DataColumn In columnasAMostrar
            Dim headerText = If(String.IsNullOrWhiteSpace(col.Caption), col.ColumnName, col.Caption)
            sb.AppendLine("<TablixCell><CellContents><Textbox Name='Header" & col.ColumnName & "'><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>" & EscapeXml(headerText) & "</Value><Style><FontWeight>Bold</FontWeight><Color>White</Color></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs><Style><Border><Color>LightGrey</Color><Style>Solid</Style></Border><BackgroundColor>#4682B4</BackgroundColor><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>")
        Next
        sb.AppendLine("</TablixCells></TablixRow>")

        ' Fila de datos
        sb.AppendLine("<TablixRow><Height>0.25in</Height><TablixCells>")
        ' --- MODIFICADO: Usar la lista filtrada y formatear fechas ---
        For Each col As DataColumn In columnasAMostrar
            Dim valueExpression As String = $"=Fields!{col.ColumnName}.Value"

            sb.AppendLine($"<TablixCell><CellContents><Textbox Name='Data{col.ColumnName}'><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>{valueExpression}</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs><Style><Border><Color>LightGrey</Color><Style>Solid</Style></Border><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>")
        Next
        sb.AppendLine("</TablixCells></TablixRow>")
        sb.AppendLine("</TablixRows>")

        sb.AppendLine("</TablixBody>")
        sb.AppendLine("<TablixColumnHierarchy><TablixMembers>")
        ' --- MODIFICADO: Usar la lista filtrada ---
        For i = 0 To columnasAMostrar.Count - 1
            sb.AppendLine("<TablixMember />")
        Next
        sb.AppendLine("</TablixMembers></TablixColumnHierarchy>")
        sb.AppendLine("<TablixRowHierarchy><TablixMembers><TablixMember><KeepWithGroup>After</KeepWithGroup></TablixMember><TablixMember><Group Name='Details' /><TablixMembers><TablixMember /></TablixMembers></TablixMember></TablixMembers></TablixRowHierarchy>")

        sb.AppendLine("<DataSetName>ResultadosDataSet</DataSetName>")
        ' --- MODIFICADO: Ajustar propiedades para coincidir con el original ---
        sb.AppendLine("<Top>3.2in</Top>")
        sb.AppendLine("<Left>0in</Left>")
        sb.AppendLine("<Height>0.5in</Height>")
        sb.AppendLine("<Width>7.5in</Width>") ' Ancho total fijo
        sb.AppendLine("<ZIndex>3</ZIndex>")
        sb.AppendLine("<Style><Border><Style>None</Style></Border></Style>")
        sb.AppendLine("</Tablix>")

        Return sb.ToString()
    End Function

    Private Shared Function EscapeXml(texto As String) As String
        If String.IsNullOrEmpty(texto) Then Return String.Empty

        Return texto.Replace("&", "&amp;").
                     Replace("<", "&lt;").
                     Replace(">", "&gt;").
                     Replace("\"", "&quot;").
                     Replace("'", "&apos;")
    End Function

    Private Shared Function ObtenerSubtituloCantidades(cantidadesDisponibles As String) As String
        Const subtituloPredeterminado As String = "Cantidades no disponibles"

        If String.IsNullOrWhiteSpace(cantidadesDisponibles) Then
            Return subtituloPredeterminado
        End If

        Dim lineas = cantidadesDisponibles.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
        For Each linea In lineas
            Dim lineaLimpia = linea.Trim()
            If Not String.IsNullOrEmpty(lineaLimpia) Then
                Return lineaLimpia
            End If
        Next

        Return subtituloPredeterminado
    End Function
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
End Class
