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
        nsManager.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner")

        Dim dataSetNode = rdlcXml.SelectSingleNode("/df:Report/df:DataSets/df:DataSet[@Name='ResultadosDataSet']", nsManager)
        If dataSetNode Is Nothing Then
            Throw New Exception("No se pudo encontrar el nodo 'DataSet' en la definición del reporte.")
        End If

        ' 1. Crear campos del DataSet
        Dim existingFieldsNode = dataSetNode.SelectSingleNode("df:Fields", nsManager)
        If existingFieldsNode IsNot Nothing Then
            dataSetNode.RemoveChild(existingFieldsNode)
        End If

        Dim columnasAIgnorar As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {"GlobalSearch"}
        Dim columnasDisponibles = dt.Columns.Cast(Of DataColumn)().
                                   Where(Function(c) Not columnasAIgnorar.Contains(c.ColumnName)).
                                   ToList()

        Dim fieldsNode = rdlcXml.CreateElement("Fields", nsManager.LookupNamespace("df"))
        For Each col As DataColumn In columnasDisponibles
            Dim fieldNode = rdlcXml.CreateElement("Field", nsManager.LookupNamespace("df"))
            Dim safeFieldName = XmlConvert.EncodeName(col.ColumnName)
            fieldNode.SetAttribute("Name", safeFieldName)

            Dim dataFieldNode = rdlcXml.CreateElement("DataField", nsManager.LookupNamespace("df"))
            dataFieldNode.InnerText = col.ColumnName
            fieldNode.AppendChild(dataFieldNode)

            Dim typeNameNode = rdlcXml.CreateElement("rd", "TypeName", nsManager.LookupNamespace("rd"))
            typeNameNode.InnerText = GetType(String).FullName
            fieldNode.AppendChild(typeNameNode)

            fieldsNode.AppendChild(fieldNode)
        Next
        dataSetNode.AppendChild(fieldsNode)

        ' 2. Crear el Tablix (Tabla de resultados)
        Dim bodyNode As XmlNode = rdlcXml.SelectSingleNode("/df:Report/df:Body/df:ReportItems", nsManager)
        If bodyNode Is Nothing Then
            bodyNode = rdlcXml.SelectSingleNode("/df:Report/df:ReportSections/df:ReportSection/df:Body/df:ReportItems", nsManager)
        End If

        If bodyNode Is Nothing Then
            Throw New Exception("No se pudo encontrar el nodo 'ReportItems' en la definición del reporte.")
        End If

        Dim existingTablix = bodyNode.SelectSingleNode("df:Tablix[@Name='TablixResultados']", nsManager)
        If existingTablix IsNot Nothing Then
            bodyNode.RemoveChild(existingTablix)
        End If

        Dim tablixXml As String = GenerarTablixXml(columnasDisponibles, nsManager)
        If Not String.IsNullOrWhiteSpace(tablixXml) Then
            Dim tablixDoc As New XmlDocument()
            tablixDoc.LoadXml(tablixXml)
            Dim tablixNode = rdlcXml.ImportNode(tablixDoc.DocumentElement, True)
            bodyNode.AppendChild(tablixNode)
        End If
    End Sub

    Private Function GenerarTablixXml(columns As IEnumerable(Of DataColumn), nsManager As XmlNamespaceManager) As String
        Dim sb As New StringBuilder()
        Dim ns As String = nsManager.LookupNamespace("df")

        Dim columnasAMostrar = columns.ToList()

        If columnasAMostrar.Count = 0 Then
            Return String.Empty ' No generar tabla si no hay nada que mostrar
        End If

        Dim widthOverrides As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase) From {
            {"NombreCompleto", "2.8in"}, {"NombreFuncionario", "2.8in"}, {"Nombre", "2.5in"},
            {"Cedula", "1.2in"}, {"CI", "1.2in"}, {"Resumen", "3.5in"}, {"Texto", "3.5in"},
            {"Observaciones", "3.7in"}, {"Descripcion", "3.2in"}, {"Detalle", "3.2in"},
            {"Motivo", "2.5in"}, {"PuestoDeTrabajo", "2.5in"}, {"Cargo", "2.0in"},
            {"Seccion", "2.0in"}, {"Sección", "2.0in"}, {"Seccin", "2.0in"}, {"TipoDeFuncionario", "2.2in"},
            {"TipoFuncionario", "2.2in"}, {"Escalafon", "2.2in"}, {"Escalafón", "2.2in"},
            {"Estado", "1.5in"}, {"Tipo", "1.6in"}, {"Oficina", "1.8in"}, {"Rango", "2.2in"},
            {"Documento", "2.2in"}, {"ExpMinisterial", "2.2in"}, {"ExpINR", "2.2in"},
            {"Dependencia", "2.5in"}, {"Unidad", "2.0in"}
        }

        sb.AppendLine($"<Tablix Name='TablixResultados' xmlns='{ns}'>")
        sb.AppendLine("<TablixBody>")
        sb.AppendLine("<TablixColumns>")

        For Each col As DataColumn In columnasAMostrar
            ' --- CORRECCIÓN ADVERTENCIA BC42030 ---
            Dim width As String = "1.2in" ' Valor por defecto
            Dim tempWidth As String = Nothing
            Dim headerKey = If(String.IsNullOrWhiteSpace(col.Caption), col.ColumnName, col.Caption)

            If widthOverrides.TryGetValue(col.ColumnName, tempWidth) Then
                width = tempWidth
            ElseIf widthOverrides.TryGetValue(headerKey, tempWidth) Then
                width = tempWidth
            End If
            sb.AppendLine($"<TablixColumn><Width>{width}</Width></TablixColumn>")
        Next
        sb.AppendLine("</TablixColumns>")

        sb.AppendLine("<TablixRows>")
        ' Fila de encabezado
        sb.AppendLine("<TablixRow><Height>0.25in</Height><TablixCells>")
        For Each col As DataColumn In columnasAMostrar
            ' --- CORRECCIÓN ERROR DESERIALIZACIÓN ---
            Dim safeFieldName = XmlConvert.EncodeName(col.ColumnName)
            Dim headerName = XmlConvert.EncodeName("Header" & col.ColumnName)
            Dim headerText = If(String.IsNullOrWhiteSpace(col.Caption), col.ColumnName, col.Caption)
            sb.AppendLine($"<TablixCell><CellContents><Textbox Name='{headerName}'><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>{EscapeXml(headerText)}</Value><Style><FontWeight>Bold</FontWeight><Color>White</Color></Style></TextRun></TextRuns><Style><TextAlign>Left</TextAlign></Style></Paragraph></Paragraphs><Style><Border><Color>LightGrey</Color><Style>Solid</Style></Border><BackgroundColor>#4682B4</BackgroundColor><PaddingLeft>4pt</PaddingLeft><PaddingRight>4pt</PaddingRight><PaddingTop>4pt</PaddingTop><PaddingBottom>4pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>")
        Next
        sb.AppendLine("</TablixCells></TablixRow>")

        ' Fila de datos
        sb.AppendLine("<TablixRow><Height>0.25in</Height><TablixCells>")
        For Each col As DataColumn In columnasAMostrar
            Dim safeFieldName = XmlConvert.EncodeName(col.ColumnName)
            Dim dataName = XmlConvert.EncodeName("Data" & col.ColumnName)
            Dim valueExpression As String
            Dim textAlign As String = "Left"

            ' --- MEJORA: Formatear fechas y alinear tipos de datos ---
            If col.DataType Is GetType(DateTime) Then
                valueExpression = $"=IIF(IsNothing(Fields!{safeFieldName}.Value), """", Format(Fields!{safeFieldName}.Value, ""dd/MM/yyyy""))"
                textAlign = "Center"
            ElseIf GetType(ValueType).IsAssignableFrom(col.DataType) AndAlso col.DataType IsNot GetType(Boolean) Then
                valueExpression = $"=Fields!{safeFieldName}.Value"
                textAlign = "Right"
            Else
                valueExpression = $"=Fields!{safeFieldName}.Value"
            End If

            sb.AppendLine($"<TablixCell><CellContents><Textbox Name='{dataName}'><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>{valueExpression}</Value><Style /></TextRun></TextRuns><Style><TextAlign>{textAlign}</TextAlign></Style></Paragraph></Paragraphs><Style><Border><Color>LightGrey</Color><Style>Solid</Style></Border><PaddingLeft>4pt</PaddingLeft><PaddingRight>4pt</PaddingRight><PaddingTop>4pt</PaddingTop><PaddingBottom>4pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>")
        Next
        sb.AppendLine("</TablixCells></TablixRow>")
        sb.AppendLine("</TablixRows>")

        sb.AppendLine("</TablixBody>")
        sb.AppendLine("<TablixColumnHierarchy><TablixMembers>")
        For i = 0 To columnasAMostrar.Count - 1
            sb.AppendLine("<TablixMember />")
        Next
        sb.AppendLine("</TablixMembers></TablixColumnHierarchy>")
        sb.AppendLine("<TablixRowHierarchy><TablixMembers><TablixMember><KeepWithGroup>After</KeepWithGroup></TablixMember><TablixMember><Group Name='Details' /><TablixMembers><TablixMember /></TablixMembers></TablixMember></TablixMembers></TablixRowHierarchy>")

        sb.AppendLine("<DataSetName>ResultadosDataSet</DataSetName>")
        sb.AppendLine("<Top>3.2in</Top>")
        sb.AppendLine("<Left>0in</Left>")
        sb.AppendLine("<Height>0.5in</Height>")
        sb.AppendLine("<Width>7.5in</Width>")
        sb.AppendLine("<ZIndex>3</ZIndex>")
        sb.AppendLine("<Style><Border><Style>None</Style></Border></Style>")
        sb.AppendLine("</Tablix>")

        Return sb.ToString()
    End Function

    Private Shared Function EscapeXml(texto As String) As String
        If String.IsNullOrEmpty(texto) Then Return String.Empty
        Return texto.Replace("&", "&amp;") _
                    .Replace("<", "&lt;") _
                    .Replace(">", "&gt;") _
                    .Replace("""", "&quot;") _
                    .Replace("'", "&apos;")
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
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
End Class
