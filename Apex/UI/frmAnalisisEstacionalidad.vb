Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmAnalisisEstacionalidad
    Private _licenciaService As New LicenciaService()

    Private Sub frmAnalisisEstacionalidad_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarFiltrosLicencia()
        CargarFiltrosAnio()
        CargarModelosPrediccion()
        LimpiarGrafico()
        AddHandler btnFiltrar.Click, AddressOf btnFiltrar_Click
        AddHandler btnPredecir.Click, AddressOf btnPredecir_Click
    End Sub

    Private Sub CargarFiltrosLicencia()
        Dim tiposLicencia = _licenciaService.GetAllTiposLicencia()
        chkTiposLicencia.DataSource = tiposLicencia
        chkTiposLicencia.DisplayMember = "nombre"
        chkTiposLicencia.ValueMember = "id"
    End Sub

    Private Sub CargarFiltrosAnio()
        Dim anios = _licenciaService.GetAvailableYears()
        chkAnios.DataSource = anios
        For i As Integer = 0 To chkAnios.Items.Count - 1
            chkAnios.SetItemChecked(i, False)
        Next
    End Sub

    Private Sub CargarModelosPrediccion()
        cboModeloPrediccion.Items.Add("Promedio Histórico (Simple)")
        cboModeloPrediccion.Items.Add("Regresión Lineal (con Tendencia)")
        cboModeloPrediccion.SelectedIndex = 0 ' Seleccionar el primero por defecto
    End Sub

    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs)
        CargarGraficoEstacionalidad()
    End Sub

    Private Sub btnPredecir_Click(sender As Object, e As EventArgs)
        CargarGraficoEstacionalidad()
        AñadirLineaDePrediccion()
    End Sub

    Private Sub LimpiarGrafico()
        Chart1.Series.Clear()
        Chart1.Titles.Clear()
        Chart1.Titles.Add("Análisis y Predicción de Licencias")
        Chart1.ChartAreas(0).AxisX.Title = "Mes del Año"
        Chart1.ChartAreas(0).AxisY.Title = "Cantidad de Licencias"
        PersonalizarEjeX()
    End Sub

    Private Sub CargarGraficoEstacionalidad()
        Dim selectedLicenseIds = GetSelectedLicenseIds()
        Dim selectedYears = GetSelectedYears()

        If Not selectedLicenseIds.Any() OrElse Not selectedYears.Any() Then
            MessageBox.Show("Por favor, seleccione al menos un tipo de licencia y un año para analizar.", "Filtros vacíos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        LimpiarGrafico()

        Dim datos = _licenciaService.ObtenerDatosEstacionalidad(selectedLicenseIds, selectedYears)
        If datos Is Nothing OrElse Not datos.Any() Then
            MessageBox.Show("No se encontraron licencias históricas con los filtros seleccionados.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim años = datos.Select(Function(d) d.Anio).Distinct()
        For Each anio As Integer In años
            Dim seriesAnio As New Series(anio.ToString())
            seriesAnio.ChartType = SeriesChartType.Line
            seriesAnio.BorderWidth = 3
            seriesAnio.MarkerStyle = MarkerStyle.Circle
            seriesAnio.MarkerSize = 8
            Dim datosDelAnio = datos.Where(Function(d) d.Anio = anio)
            For Each registro In datosDelAnio
                seriesAnio.Points.AddXY(registro.Mes, registro.Cantidad)
            Next
            Chart1.Series.Add(seriesAnio)
        Next
    End Sub

    Private Sub AñadirLineaDePrediccion()
        Dim selectedLicenseIds = GetSelectedLicenseIds()
        Dim selectedYears = GetSelectedYears()

        If Not selectedLicenseIds.Any() OrElse Not selectedYears.Any() Then Return

        If Chart1.Series.IndexOf("Predicción") >= 0 Then
            Chart1.Series.RemoveAt(Chart1.Series.IndexOf("Predicción"))
        End If

        Dim datosPrediccion As List(Of LicenciaPrediccion)

        ' --- AQUÍ ESTÁ LA LÓGICA PARA ELEGIR EL MODELO ---
        Select Case cboModeloPrediccion.SelectedIndex
            Case 0 ' Promedio Histórico
                datosPrediccion = _licenciaService.PredecirLicenciasPorMes(selectedLicenseIds, selectedYears)
            Case 1 ' Regresión Lineal
                datosPrediccion = _licenciaService.PredecirLicenciasConTendencia(selectedLicenseIds, selectedYears)
            Case Else
                Return
        End Select


        If datosPrediccion Is Nothing OrElse Not datosPrediccion.Any() Then
            MessageBox.Show("No hay suficientes datos históricos (con los años seleccionados) para generar una predicción con este modelo.", "Datos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim seriesPrediccion As New Series("Predicción")
        seriesPrediccion.ChartType = SeriesChartType.Line
        seriesPrediccion.BorderWidth = 4
        seriesPrediccion.Color = Color.Red
        seriesPrediccion.BorderDashStyle = ChartDashStyle.Dash

        For Each punto In datosPrediccion
            seriesPrediccion.Points.AddXY(punto.Mes, Math.Round(punto.CantidadPromedio, 2))
        Next

        Chart1.Series.Add(seriesPrediccion)
    End Sub

    Private Function GetSelectedLicenseIds() As List(Of Integer)
        Dim selectedIds As New List(Of Integer)
        For Each item As TipoLicencia In chkTiposLicencia.CheckedItems
            selectedIds.Add(item.Id)
        Next
        Return selectedIds
    End Function

    Private Function GetSelectedYears() As List(Of Integer)
        Dim selectedYears As New List(Of Integer)
        For Each item As Integer In chkAnios.CheckedItems
            selectedYears.Add(item)
        Next
        Return selectedYears
    End Function

    Private Sub PersonalizarEjeX()
        Chart1.ChartAreas(0).AxisX.CustomLabels.Clear()
        Chart1.ChartAreas(0).AxisX.Interval = 1
        Dim meses() As String = {"", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"}
        For i As Integer = 1 To 12
            Dim label As New CustomLabel(i - 0.5, i + 0.5, meses(i), 0, LabelMarkStyle.None)
            Chart1.ChartAreas(0).AxisX.CustomLabels.Add(label)
        Next
        Chart1.ChartAreas(0).AxisX.IsLabelAutoFit = False
    End Sub

End Class