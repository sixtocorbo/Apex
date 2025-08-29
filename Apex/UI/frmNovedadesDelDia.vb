Imports System.Data.Entity
Imports System.Windows.Forms
Imports Microsoft.Reporting.WinForms

Public Class frmNovedadesDelDia
    Private _uow As IUnitOfWork

    Public Sub New()
        InitializeComponent()
        _uow = New UnitOfWork()
    End Sub

    Private Async Sub frmNovedadesDelDia_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Inicializa el selector de fecha con el día actual
        dtpFecha.Value = DateTime.Now.Date
        ' Carga las novedades del día actual al abrir el formulario
        Await CargarNovedades()
    End Sub

    Private Async Sub btnCargarNovedades_Click(sender As Object, e As EventArgs) Handles btnCargarNovedades.Click
        ' Vuelve a cargar las novedades cuando el usuario hace clic en el botón
        Await CargarNovedades()
    End Sub

    ' Método principal para obtener y mostrar los datos
    Private Async Function CargarNovedades() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim fechaSeleccionada = dtpFecha.Value.Date
            Dim repo = _uow.Repository(Of vw_NovedadesCompletas)()

            ' 1. Obtener todas las novedades para la fecha seleccionada desde la vista
            Dim novedadesCompletas = Await repo.GetAll().
                Where(Function(n) n.Fecha = fechaSeleccionada).
                ToListAsync()

            ' 2. Agrupar los resultados por el ID de la novedad
            '    y concatenar los nombres de los funcionarios involucrados.
            Dim novedadesAgrupadas = novedadesCompletas.
                GroupBy(Function(n) n.Id).
                Select(Function(g) New With {
                    .Id = g.Key,
                    .Fecha = g.First().Fecha,
                    .Resumen = g.First().Texto,
                    .Estado = g.First().Estado,
                    .Funcionarios = String.Join(", ", g.Select(Function(f) f.NombreFuncionario).Distinct())
                }).
                OrderByDescending(Function(n) n.Fecha).
                ToList()

            ' 3. Asignar los datos procesados a la grilla
            dgvNovedadesDelDia.DataSource = novedadesAgrupadas
            ConfigurarGrilla()

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al cargar las novedades: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    ' Configura la apariencia de las columnas en la grilla
    Private Sub ConfigurarGrilla()
        If dgvNovedadesDelDia.Columns.Contains("Id") Then
            dgvNovedadesDelDia.Columns("Id").Visible = False
        End If
        If dgvNovedadesDelDia.Columns.Contains("Fecha") Then
            dgvNovedadesDelDia.Columns("Fecha").HeaderText = "Fecha"
            dgvNovedadesDelDia.Columns("Fecha").Width = 80
        End If
        If dgvNovedadesDelDia.Columns.Contains("Resumen") Then
            dgvNovedadesDelDia.Columns("Resumen").HeaderText = "Resumen de la Novedad"
            dgvNovedadesDelDia.Columns("Resumen").Width = 350
        End If
        If dgvNovedadesDelDia.Columns.Contains("Funcionarios") Then
            dgvNovedadesDelDia.Columns("Funcionarios").HeaderText = "Funcionarios Involucrados"
            dgvNovedadesDelDia.Columns("Funcionarios").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
        If dgvNovedadesDelDia.Columns.Contains("Estado") Then
            dgvNovedadesDelDia.Columns("Estado").HeaderText = "Estado"
            dgvNovedadesDelDia.Columns("Estado").Width = 120
        End If
    End Sub

    ' Libera los recursos al cerrar el formulario
    Private Sub frmNovedadesDelDia_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _uow.Dispose()
    End Sub

    ' Evento Click para el nuevo botón de Imprimir
    ' En el archivo: frmNovedadesDelDia.vb

    Private Async Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If dgvNovedadesDelDia.DataSource Is Nothing OrElse dgvNovedadesDelDia.Rows.Count = 0 Then
            MessageBox.Show("No hay datos para imprimir.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        LoadingHelper.MostrarCargando(Me)
        Try
            ' 1. Obtener los IDs de las novedades que se están mostrando
            Dim idsNovedades = CType(dgvNovedadesDelDia.DataSource, IEnumerable(Of Object)).
                            Select(Function(n) CInt(n.GetType().GetProperty("Id").GetValue(n))).
                            ToList()

            ' 2. Volver a buscar los datos completos
            Dim repo = _uow.Repository(Of vw_NovedadesCompletas)()
            Dim novedadesCompletas = Await repo.GetAll().
            Where(Function(n) idsNovedades.Contains(n.Id)).
            ToListAsync()

            ' 3. CORRECCIÓN: Agrupar los datos incluyendo el ID para pasarlo al reporte
            Dim datosParaReporte = novedadesCompletas.
            GroupBy(Function(n) n.Id).
            Select(Function(g) New With {
                .Id = g.Key, ' <--- AÑADIMOS EL ID AQUÍ
                .Fecha = g.First().Fecha,
                .Texto = g.First().Texto,
                .Funcionarios = g.Select(Function(f) New With {.NombreFuncionario = f.NombreFuncionario}).ToList()
            }).ToList()

            ' 4. Mostrar el visor de reportes
            Using frmVisor As New frmNovedadesRPT(datosParaReporte)
                frmVisor.ShowDialog(Me)
            End Using

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al preparar la impresión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub

    ' Evento para procesar el subreporte (la lista de funcionarios)
    Private Sub LocalReport_SubreportProcessing(sender As Object, e As SubreportProcessingEventArgs)
        Try
            ' Obtenemos el ID de la novedad que se está procesando
            Dim idNovedad As Integer = CInt(e.Parameters("NovedadId").Values(0))

            ' Buscamos la novedad en nuestra lista de datos
            Dim novedadActual = CType(DirectCast(sender, LocalReport).DataSources("DataSetNovedades").Value, IEnumerable(Of Object)).
                            FirstOrDefault(Function(n) CInt(n.GetType().GetProperty("Id").GetValue(n)) = idNovedad)

            If novedadActual IsNot Nothing Then
                ' Obtenemos la lista de funcionarios de esa novedad
                Dim funcionarios = novedadActual.GetType().GetProperty("Funcionarios").GetValue(novedadActual)
                ' Creamos un nuevo origen de datos para el subreporte
                Dim rdsFuncionarios As New ReportDataSource("DataSetFuncionarios", funcionarios)
                e.DataSources.Add(rdsFuncionarios)
            End If
        Catch ex As Exception
            MessageBox.Show("Error dentro de SubreportProcessing: " & ex.Message)
        End Try
    End Sub
End Class