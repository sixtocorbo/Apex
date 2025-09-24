Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Deployment.Application
Imports System.IO
Imports System.Linq
Imports System.Reflection
Imports System.Windows.Forms
Imports Microsoft.Reporting.WinForms

Public Module ReportResourceLoader

    Public Enum ReportDefinitionSource
        Embedded
        File
    End Enum

    Public Structure ReportDefinitionResult
        Public Property Source As ReportDefinitionSource
        Public Property ResourceName As String
        Public Property FilePath As String
    End Structure

    Public Function LoadLocalReportDefinition(localReport As LocalReport,
                                              ownerType As Type,
                                              expectedResourceName As String,
                                              reportFileName As String,
                                              Optional additionalRelativeSearchPaths As IEnumerable(Of String) = Nothing) As ReportDefinitionResult

        If localReport Is Nothing Then Throw New ArgumentNullException(NameOf(localReport))
        If ownerType Is Nothing Then Throw New ArgumentNullException(NameOf(ownerType))
        If String.IsNullOrWhiteSpace(reportFileName) Then Throw New ArgumentException("Se requiere el nombre del archivo del reporte.", NameOf(reportFileName))

        Dim searchResult = LocateReportDefinition(ownerType,
                                                  expectedResourceName,
                                                  reportFileName,
                                                  additionalRelativeSearchPaths)

        ' Embedded
        If Not String.IsNullOrEmpty(searchResult.ResourceName) Then
            Using reportStream As Stream = searchResult.Assembly.GetManifestResourceStream(searchResult.ResourceName)
                If reportStream Is Nothing Then
                    Throw New FileNotFoundException($"No se pudo abrir el recurso incrustado '{searchResult.ResourceName}'.")
                End If
                localReport.LoadReportDefinition(reportStream)
            End Using
            Return New ReportDefinitionResult With {
                .Source = ReportDefinitionSource.Embedded,
                .ResourceName = searchResult.ResourceName
            }
        End If

        ' File path
        If Not String.IsNullOrEmpty(searchResult.FilePath) Then
            localReport.ReportPath = searchResult.FilePath
            Return New ReportDefinitionResult With {
                .Source = ReportDefinitionSource.File,
                .FilePath = searchResult.FilePath
            }
        End If

        Throw BuildMissingReportException(reportFileName,
                                          expectedResourceName,
                                          searchResult.AvailableResourceNames,
                                          searchResult.SearchedFilePaths)
    End Function

    Public Sub LoadSubreportDefinition(localReport As LocalReport,
                                       subreportName As String,
                                       ownerType As Type,
                                       expectedResourceName As String,
                                       reportFileName As String,
                                       Optional additionalRelativeSearchPaths As IEnumerable(Of String) = Nothing)
        If localReport Is Nothing Then Throw New ArgumentNullException(NameOf(localReport))
        If String.IsNullOrWhiteSpace(subreportName) Then Throw New ArgumentException("Se requiere el nombre del subreporte.", NameOf(subreportName))

        Dim searchResult = LocateReportDefinition(ownerType,
                                                  expectedResourceName,
                                                  reportFileName,
                                                  additionalRelativeSearchPaths)

        ' Embedded
        If Not String.IsNullOrEmpty(searchResult.ResourceName) Then
            Using reportStream As Stream = searchResult.Assembly.GetManifestResourceStream(searchResult.ResourceName)
                If reportStream Is Nothing Then
                    Throw New FileNotFoundException($"No se pudo abrir el recurso incrustado '{searchResult.ResourceName}'.")
                End If
                localReport.LoadSubreportDefinition(subreportName, reportStream)
            End Using
            Exit Sub
        End If

        ' File path
        If Not String.IsNullOrEmpty(searchResult.FilePath) Then
            Using fileStream As FileStream = File.OpenRead(searchResult.FilePath)
                localReport.LoadSubreportDefinition(subreportName, fileStream)
            End Using
            Exit Sub
        End If

        Throw BuildMissingReportException(reportFileName,
                                          expectedResourceName,
                                          searchResult.AvailableResourceNames,
                                          searchResult.SearchedFilePaths)
    End Sub

    ' ---------- Internals ----------

    Private Function LocateReportDefinition(ownerType As Type,
                                            expectedResourceName As String,
                                            reportFileName As String,
                                            additionalRelativeSearchPaths As IEnumerable(Of String)) As ReportDefinitionSearchResult

        Dim executingAssembly As Assembly = ownerType.Assembly
        Dim resourceNames As String() = executingAssembly.GetManifestResourceNames()

        ' 1) Match exacto con el nombre esperado (si viene)
        Dim resourceMatch As String = Nothing
        If Not String.IsNullOrWhiteSpace(expectedResourceName) Then
            resourceMatch = resourceNames.FirstOrDefault(Function(name) name.Equals(expectedResourceName, StringComparison.OrdinalIgnoreCase))
        End If

        ' 2) Si no hay exacto, buscar por sufijo .<file>.rdlc
        If String.IsNullOrWhiteSpace(resourceMatch) Then
            Dim suffix As String = "." & reportFileName
            resourceMatch = resourceNames.FirstOrDefault(Function(name) name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) _
                                                           OrElse name.EndsWith(reportFileName, StringComparison.OrdinalIgnoreCase))
        End If

        If Not String.IsNullOrWhiteSpace(resourceMatch) Then
            Return New ReportDefinitionSearchResult With {
                .Assembly = executingAssembly,
                .ResourceName = resourceMatch,
                .AvailableResourceNames = resourceNames,
                .SearchedFilePaths = New String() {}
            }
        End If

        ' 3) Buscar en disco (varias carpetas)
        Dim searchedPaths As New List(Of String)
        Dim possiblePaths As New List(Of String)
        Dim baseDirectory As String = AppDomain.CurrentDomain.BaseDirectory
        Dim startupPath As String = Application.StartupPath

        ' convenciones comunes
        possiblePaths.Add(Path.Combine(baseDirectory, "Reportes", reportFileName))
        possiblePaths.Add(Path.Combine(startupPath, "Reportes", reportFileName))
        possiblePaths.Add(Path.Combine(baseDirectory, reportFileName))
        possiblePaths.Add(Path.Combine(startupPath, reportFileName))

        ' rutas extra (relativas o absolutas)
        If additionalRelativeSearchPaths IsNot Nothing Then
            For Each relative In additionalRelativeSearchPaths
                If String.IsNullOrWhiteSpace(relative) Then Continue For
                Dim c1 As String = NormalizeCandidate(startupPath, relative)
                Dim c2 As String = NormalizeCandidate(baseDirectory, relative)
                If Not String.IsNullOrEmpty(c1) Then possiblePaths.Add(c1)
                If Not String.IsNullOrEmpty(c2) Then possiblePaths.Add(c2)
            Next
        End If

        ' ClickOnce (protegido)
        Try
            If ApplicationDeployment.IsNetworkDeployed Then
                Dim dataDir As String = ApplicationDeployment.CurrentDeployment.DataDirectory
                possiblePaths.Add(Path.Combine(dataDir, "Reportes", reportFileName))
                possiblePaths.Add(Path.Combine(dataDir, reportFileName))
            End If
        Catch
            ' Ignorar si no aplica ClickOnce o no hay permisos
        End Try

        ' Buscar primer archivo existente
        Dim reportPath As String = possiblePaths.FirstOrDefault(Function(path As String)
                                                                    If String.IsNullOrWhiteSpace(path) Then Return False
                                                                    searchedPaths.Add(path)
                                                                    Return File.Exists(path)
                                                                End Function)

        ' Búsqueda ampliada (opcional, puede fallar por permisos: protegido)
        If String.IsNullOrWhiteSpace(reportPath) Then
            Try
                Dim found As String = Directory.EnumerateFiles(baseDirectory, reportFileName, SearchOption.AllDirectories).FirstOrDefault()
                If Not String.IsNullOrWhiteSpace(found) Then
                    reportPath = found
                    searchedPaths.Add(found)
                End If
            Catch ex As UnauthorizedAccessException
                ' Ignorar y seguir
            Catch
                ' Ignorar y seguir
            End Try
        End If

        If Not String.IsNullOrWhiteSpace(reportPath) Then
            Return New ReportDefinitionSearchResult With {
                .Assembly = executingAssembly,
                .FilePath = reportPath,
                .AvailableResourceNames = resourceNames,
                .SearchedFilePaths = searchedPaths.Distinct().ToArray()
            }
        End If

        Return New ReportDefinitionSearchResult With {
            .Assembly = executingAssembly,
            .AvailableResourceNames = resourceNames,
            .SearchedFilePaths = searchedPaths.Distinct().ToArray()
        }
    End Function

    Private Function NormalizeCandidate(root As String, relativeOrAbsolute As String) As String
        If String.IsNullOrWhiteSpace(relativeOrAbsolute) Then Return Nothing
        Try
            If Path.IsPathRooted(relativeOrAbsolute) Then
                Return Path.GetFullPath(relativeOrAbsolute)
            Else
                Return Path.GetFullPath(Path.Combine(root, relativeOrAbsolute))
            End If
        Catch
            Return Nothing
        End Try
    End Function

    Private Function BuildMissingReportException(reportFileName As String,
                                                 expectedResourceName As String,
                                                 resourceNames As String(),
                                                 searchedPaths As String()) As FileNotFoundException
        Dim mensajeError As String = $"No se encontró el recurso de reporte '{reportFileName}'."

        If Not String.IsNullOrWhiteSpace(expectedResourceName) Then
            mensajeError &= Environment.NewLine & $"Nombre de recurso esperado: {expectedResourceName}"
        End If

        If resourceNames IsNot Nothing AndAlso resourceNames.Any() Then
            mensajeError &= Environment.NewLine & "Recursos incrustados disponibles: " & String.Join(", ", resourceNames)
        Else
            mensajeError &= Environment.NewLine & "No se encontraron recursos incrustados en el ensamblado. Verifica que la 'Build Action' del archivo .rdlc sea 'Embedded Resource'."
        End If

        If searchedPaths IsNot Nothing AndAlso searchedPaths.Any() Then
            mensajeError &= Environment.NewLine & "Rutas probadas: " & String.Join(", ", searchedPaths)
        End If

        Return New FileNotFoundException(mensajeError)
    End Function

    Private Class ReportDefinitionSearchResult
        Public Property Assembly As Assembly
        Public Property ResourceName As String
        Public Property FilePath As String
        Public Property AvailableResourceNames As String()
        Public Property SearchedFilePaths As String()
    End Class

End Module

'Imports System.Deployment.Application
'Imports System.IO
'Imports System.Reflection
'Imports Microsoft.Reporting.WinForms

'Public Module ReportResourceLoader

'    Public Enum ReportDefinitionSource
'        Embedded
'        File
'    End Enum

'    Public Structure ReportDefinitionResult
'        Public Property Source As ReportDefinitionSource
'        Public Property ResourceName As String
'        Public Property FilePath As String
'    End Structure

'    Public Function LoadLocalReportDefinition(localReport As LocalReport,
'                                              ownerType As Type,
'                                              expectedResourceName As String,
'                                              reportFileName As String,
'                                              Optional additionalRelativeSearchPaths As IEnumerable(Of String) = Nothing) As ReportDefinitionResult
'        If localReport Is Nothing Then Throw New ArgumentNullException(NameOf(localReport))
'        If ownerType Is Nothing Then Throw New ArgumentNullException(NameOf(ownerType))
'        If String.IsNullOrWhiteSpace(reportFileName) Then Throw New ArgumentException("Se requiere el nombre del archivo del reporte.", NameOf(reportFileName))

'        Dim searchResult = LocateReportDefinition(ownerType,
'                                                  expectedResourceName,
'                                                  reportFileName,
'                                                  additionalRelativeSearchPaths)

'        If searchResult.ResourceName IsNot Nothing Then
'            Using reportStream = searchResult.Assembly.GetManifestResourceStream(searchResult.ResourceName)
'                If reportStream Is Nothing Then
'                    Throw New FileNotFoundException($"No se pudo abrir el recurso incrustado '{searchResult.ResourceName}'.")
'                End If
'                localReport.LoadReportDefinition(reportStream)
'            End Using
'            Return New ReportDefinitionResult With {
'                .Source = ReportDefinitionSource.Embedded,
'                .ResourceName = searchResult.ResourceName
'            }
'        End If

'        If searchResult.FilePath IsNot Nothing Then
'            localReport.ReportPath = searchResult.FilePath
'            Return New ReportDefinitionResult With {
'                .Source = ReportDefinitionSource.File,
'                .FilePath = searchResult.FilePath
'            }
'        End If

'        Throw BuildMissingReportException(reportFileName,
'                                          expectedResourceName,
'                                          searchResult.AvailableResourceNames,
'                                          searchResult.SearchedFilePaths)
'    End Function

'    Public Sub LoadSubreportDefinition(localReport As LocalReport,
'                                       subreportName As String,
'                                       ownerType As Type,
'                                       expectedResourceName As String,
'                                       reportFileName As String,
'                                       Optional additionalRelativeSearchPaths As IEnumerable(Of String) = Nothing)
'        If localReport Is Nothing Then Throw New ArgumentNullException(NameOf(localReport))
'        If String.IsNullOrWhiteSpace(subreportName) Then Throw New ArgumentException("Se requiere el nombre del subreporte.", NameOf(subreportName))

'        Dim searchResult = LocateReportDefinition(ownerType,
'                                                  expectedResourceName,
'                                                  reportFileName,
'                                                  additionalRelativeSearchPaths)

'        If searchResult.ResourceName IsNot Nothing Then
'            Using reportStream = searchResult.Assembly.GetManifestResourceStream(searchResult.ResourceName)
'                If reportStream Is Nothing Then
'                    Throw New FileNotFoundException($"No se pudo abrir el recurso incrustado '{searchResult.ResourceName}'.")
'                End If
'                localReport.LoadSubreportDefinition(subreportName, reportStream)
'            End Using
'            Return
'        End If

'        If searchResult.FilePath IsNot Nothing Then
'            Using fileStream = File.OpenRead(searchResult.FilePath)
'                localReport.LoadSubreportDefinition(subreportName, fileStream)
'            End Using
'            Return
'        End If

'        Throw BuildMissingReportException(reportFileName,
'                                          expectedResourceName,
'                                          searchResult.AvailableResourceNames,
'                                          searchResult.SearchedFilePaths)
'    End Sub

'    Private Function LocateReportDefinition(ownerType As Type,
'                                             expectedResourceName As String,
'                                             reportFileName As String,
'                                             additionalRelativeSearchPaths As IEnumerable(Of String)) As ReportDefinitionSearchResult
'        Dim executingAssembly As Assembly = ownerType.Assembly
'        Dim resourceNames = executingAssembly.GetManifestResourceNames()

'        Dim resourceMatch As String = Nothing
'        If Not String.IsNullOrWhiteSpace(expectedResourceName) Then
'            resourceMatch = resourceNames.FirstOrDefault(Function(name) name.Equals(expectedResourceName, StringComparison.OrdinalIgnoreCase))
'        End If

'        If String.IsNullOrWhiteSpace(resourceMatch) Then
'            Dim suffix = "." & reportFileName
'            resourceMatch = resourceNames.FirstOrDefault(Function(name) name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase) OrElse name.EndsWith(reportFileName, StringComparison.OrdinalIgnoreCase))
'        End If

'        If Not String.IsNullOrWhiteSpace(resourceMatch) Then
'            Return New ReportDefinitionSearchResult With {
'                .Assembly = executingAssembly,
'                .ResourceName = resourceMatch,
'                .AvailableResourceNames = resourceNames,
'                .SearchedFilePaths = Array.Empty(Of String)()
'            }
'        End If

'        Dim searchedPaths As New List(Of String)
'        Dim possiblePaths As New List(Of String)
'        Dim baseDirectory = AppDomain.CurrentDomain.BaseDirectory
'        Dim startupPath = Application.StartupPath

'        possiblePaths.Add(Path.Combine(baseDirectory, "Reportes", reportFileName))
'        possiblePaths.Add(Path.Combine(startupPath, "Reportes", reportFileName))
'        possiblePaths.Add(Path.Combine(baseDirectory, reportFileName))
'        possiblePaths.Add(Path.Combine(startupPath, reportFileName))

'        If additionalRelativeSearchPaths IsNot Nothing Then
'            For Each relative In additionalRelativeSearchPaths
'                If String.IsNullOrWhiteSpace(relative) Then Continue For
'                Dim candidate As String
'                If Path.IsPathRooted(relative) Then
'                    candidate = relative
'                Else
'                    candidate = Path.GetFullPath(Path.Combine(startupPath, relative))
'                End If
'                possiblePaths.Add(candidate)
'            Next
'        End If

'        If ApplicationDeployment.IsNetworkDeployed Then
'            Dim dataDir = ApplicationDeployment.CurrentDeployment.DataDirectory
'            possiblePaths.Add(Path.Combine(dataDir, "Reportes", reportFileName))
'            possiblePaths.Add(Path.Combine(dataDir, reportFileName))
'        End If

'        Dim reportPath = possiblePaths.FirstOrDefault(Function(path)
'                                                          searchedPaths.Add(path)
'                                                          Return File.Exists(path)
'                                                      End Function)

'        If String.IsNullOrWhiteSpace(reportPath) Then
'            Try
'                reportPath = Directory.EnumerateFiles(baseDirectory, reportFileName, SearchOption.AllDirectories).FirstOrDefault()
'                If Not String.IsNullOrWhiteSpace(reportPath) Then
'                    searchedPaths.Add(reportPath)
'                End If
'            Catch ex As UnauthorizedAccessException
'                ' Ignorar rutas no accesibles y continuar
'            End Try
'        End If

'        If Not String.IsNullOrWhiteSpace(reportPath) Then
'            Return New ReportDefinitionSearchResult With {
'                .Assembly = executingAssembly,
'                .FilePath = reportPath,
'                .AvailableResourceNames = resourceNames,
'                .SearchedFilePaths = searchedPaths.Distinct().ToArray()
'            }
'        End If

'        Return New ReportDefinitionSearchResult With {
'            .Assembly = executingAssembly,
'            .AvailableResourceNames = resourceNames,
'            .SearchedFilePaths = searchedPaths.Distinct().ToArray()
'        }
'    End Function

'    Private Function BuildMissingReportException(reportFileName As String,
'                                                 expectedResourceName As String,
'                                                 resourceNames As String(),
'                                                 searchedPaths As String()) As FileNotFoundException
'        Dim mensajeError As String = $"No se encontró el recurso de reporte '{reportFileName}'."

'        If Not String.IsNullOrWhiteSpace(expectedResourceName) Then
'            mensajeError &= Environment.NewLine & $"Nombre de recurso esperado: {expectedResourceName}"
'        End If

'        If resourceNames IsNot Nothing AndAlso resourceNames.Any() Then
'            mensajeError &= Environment.NewLine & "Recursos incrustados disponibles: " & String.Join(", ", resourceNames)
'        Else
'            mensajeError &= Environment.NewLine & "No se encontraron recursos incrustados en el ensamblado. Verifica que la 'Build Action' del archivo .rdlc sea 'Embedded Resource'."
'        End If

'        If searchedPaths IsNot Nothing AndAlso searchedPaths.Any() Then
'            mensajeError &= Environment.NewLine & "Rutas probadas: " & String.Join(", ", searchedPaths)
'        End If

'        Return New FileNotFoundException(mensajeError)
'    End Function

'    Private Class ReportDefinitionSearchResult
'        Public Property Assembly As Assembly
'        Public Property ResourceName As String
'        Public Property FilePath As String
'        Public Property AvailableResourceNames As String()
'        Public Property SearchedFilePaths As String()
'    End Class

'End Module