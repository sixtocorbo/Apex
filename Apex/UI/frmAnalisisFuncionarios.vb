Imports System.Windows.Forms.DataVisualization.Charting

Public Class frmAnalisisFuncionarios
    Private _funcionarioService As New FuncionarioService()

    Private Sub frmAnalisisFuncionarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        CargarGraficos()
    End Sub

    Private Sub CargarGraficos()
        ' Usamos Task.Run para cargar los gráficos en segundo plano y no congelar la UI
        Task.Run(Sub()
                     CargarGraficoGenero()
                     CargarGraficoEdad()
                     CargarGraficoAreaTrabajo()
                     CargarGraficoCargo()
                 End Sub)
    End Sub

    Private Sub CargarGraficoGenero()
        Dim datos = _funcionarioService.GetDistribucionPorGenero()
        If datos Is Nothing OrElse Not datos.Any() Then Return

        ' Invocamos al hilo de la UI para actualizar el gráfico de forma segura
        Me.Invoke(Sub()
                      chartGenero.Series.Clear()
                      Dim series As New Series("Genero")
                      series.ChartType = SeriesChartType.Pie ' Gráfico de pastel
                      series.IsValueShownAsLabel = True ' Mostrar el valor en el gráfico

                      For Each item In datos
                          series.Points.AddXY(item.Etiqueta, item.Valor)
                      Next
                      chartGenero.Series.Add(series)
                  End Sub)
    End Sub

    Private Sub CargarGraficoEdad()
        Dim datos = _funcionarioService.GetDistribucionPorRangoEdad()
        If datos Is Nothing OrElse Not datos.Any() Then Return

        Me.Invoke(Sub()
                      chartEdad.Series.Clear()
                      Dim series As New Series("Edad")
                      series.ChartType = SeriesChartType.Bar ' Gráfico de barras

                      For Each item In datos
                          Dim point As New DataPoint()
                          point.SetValueY(item.Valor)
                          point.AxisLabel = item.Etiqueta
                          point.Label = item.Valor.ToString() ' Mostrar el valor encima de la barra
                          series.Points.Add(point)
                      Next
                      chartEdad.Series.Add(series)
                  End Sub)
    End Sub

    Private Sub CargarGraficoAreaTrabajo()
        Dim datos = _funcionarioService.GetDistribucionPorAreaTrabajo()
        If datos Is Nothing OrElse Not datos.Any() Then Return

        Me.Invoke(Sub()
                      chartAreaTrabajo.Series.Clear()
                      Dim series As New Series("AreaTrabajo")
                      series.ChartType = SeriesChartType.Column ' Gráfico de columnas

                      For Each item In datos
                          series.Points.AddXY(item.Etiqueta, item.Valor)
                      Next
                      chartAreaTrabajo.Series.Add(series)
                      chartAreaTrabajo.ChartAreas(0).AxisX.Interval = 1
                  End Sub)
    End Sub

    Private Sub CargarGraficoCargo()
        Dim datos = _funcionarioService.GetDistribucionPorCargo()
        If datos Is Nothing OrElse Not datos.Any() Then Return

        Me.Invoke(Sub()
                      chartCargo.Series.Clear()
                      Dim series As New Series("Cargo")
                      series.ChartType = SeriesChartType.Bar

                      For Each item In datos
                          series.Points.AddXY(item.Etiqueta, item.Valor)
                      Next
                      chartCargo.Series.Add(series)
                  End Sub)
    End Sub

End Class