Imports System.Data
Imports System.Text
Imports System.Xml
Imports Microsoft.Reporting.WinForms

Public Class frmVisorReporte

    Private ReadOnly _titulo As String
    Private ReadOnly _filtros As String
    Private ReadOnly _cantidades As String
    Private ReadOnly _dtResultados As DataTable


    Public Sub New(titulo As String, filtrosAplicados As String, cantidadesDisponibles As String, dtResultados As DataTable)
        InitializeComponent()
        _titulo = titulo
        _filtros = filtrosAplicados
        _cantidades = cantidadesDisponibles
        _dtResultados = dtResultados
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

        sb.AppendLine($"<Tablix Name='TablixResultados' xmlns='{ns}'>")
        sb.AppendLine("<TablixBody>")
        sb.AppendLine("<TablixColumns>")
        For i = 0 To columns.Count - 1
            sb.AppendLine("<TablixColumn><Width>1in</Width></TablixColumn>") ' Ancho por defecto
        Next
        sb.AppendLine("</TablixColumns>")

        sb.AppendLine("<TablixRows>")
        ' Fila de encabezado
        sb.AppendLine("<TablixRow><Height>0.25in</Height><TablixCells>")
        For Each col As DataColumn In columns
            sb.AppendLine("<TablixCell><CellContents><Textbox Name='Header" & col.ColumnName & "'><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>" & col.ColumnName & "</Value><Style><FontWeight>Bold</FontWeight><Color>White</Color></Style></TextRun></TextRuns><Style /></Paragraph></Paragraphs><Style><Border><Color>LightGrey</Color><Style>Solid</Style></Border><BackgroundColor>#4E5865</BackgroundColor><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>")
        Next
        sb.AppendLine("</TablixCells></TablixRow>")

        ' Fila de datos
        sb.AppendLine("<TablixRow><Height>0.25in</Height><TablixCells>")
        For Each col As DataColumn In columns
            sb.AppendLine("<TablixCell><CellContents><Textbox Name='Data" & col.ColumnName & "'><CanGrow>true</CanGrow><KeepTogether>true</KeepTogether><Paragraphs><Paragraph><TextRuns><TextRun><Value>=Fields!" & col.ColumnName & ".Value</Value><Style /></TextRun></TextRuns><Style /></Paragraph></Paragraphs><Style><Border><Color>LightGrey</Color><Style>Solid</Style></Border><PaddingLeft>2pt</PaddingLeft><PaddingRight>2pt</PaddingRight><PaddingTop>2pt</PaddingTop><PaddingBottom>2pt</PaddingBottom></Style></Textbox></CellContents></TablixCell>")
        Next
        sb.AppendLine("</TablixCells></TablixRow>")
        sb.AppendLine("</TablixRows>")

        sb.AppendLine("</TablixBody>")
        sb.AppendLine("<TablixColumnHierarchy><TablixMembers>")
        For i = 0 To columns.Count - 1
            sb.AppendLine("<TablixMember />")
        Next
        sb.AppendLine("</TablixMembers></TablixColumnHierarchy>")
        sb.AppendLine("<TablixRowHierarchy><TablixMembers><TablixMember><KeepWithGroup>After</KeepWithGroup></TablixMember><TablixMember><Group Name='Details' /><TablixMembers><TablixMember /></TablixMembers></TablixMember></TablixMembers></TablixRowHierarchy>")

        sb.AppendLine("<DataSetName>ResultadosDataSet</DataSetName>")
        sb.AppendLine("<Top>1.3in</Top>")
        sb.AppendLine("<Height>0.5in</Height>")
        sb.AppendLine($"<Width>{columns.Count}in</Width>")
        sb.AppendLine("<Style><Border><Style>None</Style></Border></Style>")
        sb.AppendLine("</Tablix>")

        Return sb.ToString()
    End Function

End Class