Imports Microsoft.Reporting.WinForms

Public Class frmConceptoFuncionalRPT

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

    Private Sub frmConceptoFuncionalRPT_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.ReportViewer1.LocalReport.DataSources.Clear()

            ' Configurar los parámetros del informe
            Dim pNombre As New ReportParameter("pNombreCompleto", _funcionario.Nombre)
            Dim pCedula As New ReportParameter("pCedula", _funcionario.CI)
            Dim pCargo As New ReportParameter("pCargo", If(_funcionario.Cargo IsNot Nothing, _funcionario.Cargo.Nombre, "N/A"))
            Dim pFechaIngreso As New ReportParameter("pFechaIngreso", _funcionario.FechaIngreso.ToShortDateString())
            Dim pPeriodo As New ReportParameter("pPeriodo", $"{_fechaInicio:dd/MM/yyyy} al {_fechaFin:dd/MM/yyyy}")
            Me.ReportViewer1.LocalReport.SetParameters({pNombre, pCedula, pCargo, pFechaIngreso, pPeriodo})

            ' Configurar las fuentes de datos con los nombres exactos que espera el archivo .rdlc
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsLicenciasMedicas", _incidenciasSalud))
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsSancionesGraves", _sancionesGraves))
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("dsObservaciones", _observacionesYLeves))

            Me.ReportViewer1.RefreshReport()
        Catch ex As Exception
            MessageBox.Show("Error al generar el informe: " & ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class