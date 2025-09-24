Imports Microsoft.Reporting.WinForms
Imports System.IO
Imports System.Reflection
Imports System.Deployment.Application
Imports System.Linq

Public Class frmConceptoFuncionalRPT

#Region " Campos y Constructor "

    Private ReadOnly _funcionario As Funcionario
    Private ReadOnly _fechaInicio As Date
    Private ReadOnly _fechaFin As Date
    Private ReadOnly _incidenciasSalud As List(Of ConceptoFuncionalItem)
    Private ReadOnly _sancionesGraves As List(Of ConceptoFuncionalItem)
    Private ReadOnly _observacionesYLeves As List(Of ConceptoFuncionalItem)

    Public Sub New(funcionario As Funcionario, fechaInicio As Date, fechaFin As Date,
                   incidenciasSalud As List(Of ConceptoFuncionalItem),
                   sancionesGraves As List(Of ConceptoFuncionalItem),
                   observacionesYLeves As List(Of ConceptoFuncionalItem))

        InitializeComponent()
        _funcionario = funcionario
        _fechaInicio = fechaInicio
        _fechaFin = fechaFin
        ' Asegurarse de no pasar una referencia nula al reporte
        _incidenciasSalud = If(incidenciasSalud, New List(Of ConceptoFuncionalItem)())
        _sancionesGraves = If(sancionesGraves, New List(Of ConceptoFuncionalItem)())
        _observacionesYLeves = If(observacionesYLeves, New List(Of ConceptoFuncionalItem)())
    End Sub

#End Region

#Region " Ciclo de Vida del Formulario "

    Private Async Sub frmConceptoFuncionalRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ' Me.KeyPreview ya está en True en el diseñador

        Try
            Await CargarReporteAsync()
        Catch ex As Exception
            MessageBox.Show("Error fatal al generar el informe: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub

#End Region

#Region " Lógica Principal del Reporte "

    Private Async Function CargarReporteAsync() As Task
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            ReportViewer1.ProcessingMode = ProcessingMode.Local
            ReportViewer1.LocalReport.DataSources.Clear()

            ' --- 1. Localizar el archivo .rdlc de forma robusta ---
            Dim reportResourceName As String = "Apex.Reportes.ConceptoFuncional.rdlc"
            Dim reportLoaded As Boolean = False
            Dim executingAssembly As Assembly = GetType(frmConceptoFuncionalRPT).Assembly
            Dim resourceNames = executingAssembly.GetManifestResourceNames()

            ' Opción A: localizar cualquier recurso incrustado que termine en ConceptoFuncional.rdlc
            Dim resourceMatch As String = Nothing
            If resourceNames IsNot Nothing Then
                resourceMatch = resourceNames.FirstOrDefault(Function(nombre) nombre.Equals(reportResourceName, StringComparison.OrdinalIgnoreCase))

                If String.IsNullOrWhiteSpace(resourceMatch) Then
                    resourceMatch = resourceNames.FirstOrDefault(Function(nombre) nombre.EndsWith(".ConceptoFuncional.rdlc", StringComparison.OrdinalIgnoreCase))
                End If
            End If

            If Not String.IsNullOrWhiteSpace(resourceMatch) Then
                Using reportStream As Stream = executingAssembly.GetManifestResourceStream(resourceMatch)
                    If reportStream IsNot Nothing Then
                        ReportViewer1.LocalReport.LoadReportDefinition(reportStream)
                        reportLoaded = True
                    End If
                End Using
            End If

            ' Opción B: Si falla, buscarlo en el disco como respaldo
            If Not reportLoaded Then
                Dim posiblesRutas As New List(Of String) From {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reportes", "ConceptoFuncional.rdlc"),
                    Path.Combine(Application.StartupPath, "Reportes", "ConceptoFuncional.rdlc"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConceptoFuncional.rdlc")
                }
                If ApplicationDeployment.IsNetworkDeployed Then
                    posiblesRutas.Add(Path.Combine(ApplicationDeployment.CurrentDeployment.DataDirectory, "Reportes", "ConceptoFuncional.rdlc"))
                    posiblesRutas.Add(Path.Combine(ApplicationDeployment.CurrentDeployment.DataDirectory, "ConceptoFuncional.rdlc"))
                End If

                Dim reportPath As String = posiblesRutas.FirstOrDefault(Function(ruta) File.Exists(ruta))

                If String.IsNullOrWhiteSpace(reportPath) Then
                    ' Intentar una búsqueda recursiva como último recurso
                    Try
                        reportPath = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "ConceptoFuncional.rdlc", SearchOption.AllDirectories).FirstOrDefault()
                    Catch ex As UnauthorizedAccessException
                        ' Ignorar rutas sin permiso y continuar con el manejo de error estándar
                    End Try
                End If

                If String.IsNullOrWhiteSpace(reportPath) Then
                    ' --- Manejo de error mejorado para facilitar la depuración ---
                    Dim mensajeError As String = $"No se encontró el recurso de reporte '{reportResourceName}'." & Environment.NewLine & Environment.NewLine
                    mensajeError &= "Rutas de archivo verificadas: " & String.Join(", ", posiblesRutas) & Environment.NewLine
                    If resourceNames IsNot Nothing AndAlso resourceNames.Any() Then
                        mensajeError &= "Recursos incrustados disponibles: " & String.Join(", ", resourceNames)
                    Else
                        mensajeError &= "No se encontraron recursos incrustados en el ensamblado. Verifica que la 'Build Action' del archivo .rdlc sea 'Embedded Resource'."
                    End If
                    Throw New FileNotFoundException(mensajeError)
                End If
                ReportViewer1.LocalReport.ReportPath = reportPath
            End If

            ' --- 2. Configurar los parámetros del informe ---
            Dim pNombre As New ReportParameter("pNombreCompleto", _funcionario.Nombre)
            Dim pCedula As New ReportParameter("pCedula", _funcionario.CI)
            Dim pCargo As New ReportParameter("pCargo", If(_funcionario.Cargo IsNot Nothing, _funcionario.Cargo.Nombre, "N/A"))
            Dim pFechaIngreso As New ReportParameter("pFechaIngreso", _funcionario.FechaIngreso.ToShortDateString())
            Dim pPeriodo As New ReportParameter("pPeriodo", $"{_fechaInicio:dd/MM/yyyy} al {_fechaFin:dd/MM/yyyy}")
            Me.ReportViewer1.LocalReport.SetParameters({pNombre, pCedula, pCargo, pFechaIngreso, pPeriodo})

            ' --- 3. Configurar las fuentes de datos ---
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsLicenciasMedicas", _incidenciasSalud))
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsSancionesGraves", _sancionesGraves))
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsObservaciones", _observacionesYLeves))

            ' --- 4. Refrescar y mostrar el visor ---
            Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
            Me.ReportViewer1.ZoomMode = ZoomMode.Percent
            Me.ReportViewer1.ZoomPercent = 100
            Me.ReportViewer1.RefreshReport()

        Finally
            Me.Cursor = oldCursor
        End Try
    End Function

#End Region

End Class
