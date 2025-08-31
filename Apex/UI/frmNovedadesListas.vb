Imports System.Data.Entity
Imports System.IO
Imports System.Windows.Forms

Public Class frmNovedadesListas
    Private _uow As IUnitOfWork
    Private _funcionariosSeleccionados As New Dictionary(Of Integer, String)

    Public Sub New()
        InitializeComponent()
        _uow = New UnitOfWork()
    End Sub

    Private Sub frmReporteNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ' Configuración inicial de los controles de fecha
        dtpFechaDesde.Value = DateTime.Now.AddMonths(-1)
        dtpFechaHasta.Value = DateTime.Now
    End Sub

#Region "Filtro de Funcionarios"

    ' Apex/UI/frmReporteNovedades.vb
    Private Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                ' --- CORRECCIÓN AQUÍ ---
                ' Se accede al ID a través del FuncionarioSeleccionado
                Dim funcId = frm.FuncionarioSeleccionado.Id
                ' --- FIN DE LA CORRECCIÓN ---
                If funcId > 0 AndAlso Not _funcionariosSeleccionados.ContainsKey(funcId) Then
                    Dim repo = _uow.Repository(Of Funcionario)()
                    Dim funcionario = repo.GetById(funcId)
                    If funcionario IsNot Nothing Then
                        _funcionariosSeleccionados.Add(funcionario.Id, funcionario.Nombre)
                        ActualizarListaFuncionarios()
                    End If
                End If
            End If
        End Using
    End Sub

    Private Sub btnQuitarFuncionario_Click(sender As Object, e As EventArgs) Handles btnQuitarFuncionario.Click
        If lstFuncionarios.SelectedItem IsNot Nothing Then
            Dim selectedFuncId = CType(lstFuncionarios.SelectedValue, Integer)
            _funcionariosSeleccionados.Remove(selectedFuncId)
            ActualizarListaFuncionarios()
        End If
    End Sub

    Private Sub btnLimpiarFuncionarios_Click(sender As Object, e As EventArgs) Handles btnLimpiarFuncionarios.Click
        _funcionariosSeleccionados.Clear()
        ActualizarListaFuncionarios()
    End Sub

    Private Sub ActualizarListaFuncionarios()
        lstFuncionarios.DataSource = Nothing
        If _funcionariosSeleccionados.Any() Then
            lstFuncionarios.DataSource = New BindingSource(_funcionariosSeleccionados, Nothing)
            lstFuncionarios.DisplayMember = "Value"
            lstFuncionarios.ValueMember = "Key"
        End If
    End Sub

#End Region

#Region "Generación del Reporte"

    Private Async Sub btnGenerarReporte_Click(sender As Object, e As EventArgs) Handles btnGenerarReporte.Click
        LoadingHelper.MostrarCargando(Me)
        Try
            ' Empezamos con la vista que agrupa las novedades
            Dim repo = _uow.Repository(Of vw_NovedadesAgrupadas)()
            Dim query = repo.GetAll()

            ' 1. Aplicar filtro de fecha
            If chkFiltrarPorFecha.Checked Then
                Dim fechaDesde = dtpFechaDesde.Value.Date
                Dim fechaHasta = dtpFechaHasta.Value.Date
                query = query.Where(Function(n) n.Fecha >= fechaDesde AndAlso n.Fecha <= fechaHasta)
            End If

            ' 2. Aplicar filtro de funcionarios (si hay alguno seleccionado)
            If _funcionariosSeleccionados.Any() Then
                Dim idsFuncionarios = _funcionariosSeleccionados.Keys.ToList()
                Dim repoNovedadFuncionario = _uow.Repository(Of NovedadFuncionario)()

                ' Obtenemos los Ids de las novedades que involucran a los funcionarios seleccionados
                Dim novedadIds = Await repoNovedadFuncionario.GetAll().
                                     Where(Function(nf) idsFuncionarios.Contains(nf.FuncionarioId)).
                                     Select(Function(nf) nf.NovedadId).
                                     Distinct().
                                     ToListAsync()

                query = query.Where(Function(n) novedadIds.Contains(n.Id))
            End If

            ' 3. Ejecutar la consulta y mostrar los resultados
            Dim resultado = Await query.OrderByDescending(Function(n) n.Fecha).ToListAsync()
            dgvNovedades.DataSource = resultado

            ConfigurarGrilla()

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al generar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub

    Private Sub ConfigurarGrilla()
        If dgvNovedades.Columns.Contains("Id") Then
            dgvNovedades.Columns("Id").Visible = False
        End If
        If dgvNovedades.Columns.Contains("Fecha") Then
            dgvNovedades.Columns("Fecha").HeaderText = "Fecha"
            dgvNovedades.Columns("Fecha").Width = 80
        End If
        If dgvNovedades.Columns.Contains("Resumen") Then
            dgvNovedades.Columns("Resumen").HeaderText = "Resumen de la Novedad"
            dgvNovedades.Columns("Resumen").Width = 300
        End If
        If dgvNovedades.Columns.Contains("Funcionarios") Then
            dgvNovedades.Columns("Funcionarios").HeaderText = "Funcionarios Involucrados"
            dgvNovedades.Columns("Funcionarios").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
        If dgvNovedades.Columns.Contains("Estado") Then
            dgvNovedades.Columns("Estado").HeaderText = "Estado"
            dgvNovedades.Columns("Estado").Width = 100
        End If
    End Sub

#End Region

    Private Sub frmReporteNovedades_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _uow.Dispose()
    End Sub

#Region "Impresión del Reporte"

    Private Async Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        ' 1. Verificamos si hay datos en la grilla para imprimir
        If dgvNovedades.DataSource Is Nothing OrElse dgvNovedades.Rows.Count = 0 Then
            MessageBox.Show("Primero debe generar un reporte para poder imprimirlo.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        LoadingHelper.MostrarCargando(Me)
        Try
            ' 2. Obtenemos los IDs de las novedades que se están mostrando en la grilla
            Dim novedadesEnGrilla = CType(dgvNovedades.DataSource, List(Of vw_NovedadesAgrupadas))
            Dim novedadIds = novedadesEnGrilla.Select(Function(n) n.Id).ToList()

            ' 3. Buscamos los datos completos de las novedades desde el servicio
            Dim novedadService As New NovedadService()
            Dim datosCompletosParaReporte = Await novedadService.GetNovedadesCompletasByIds(novedadIds)

            ' 4. CORRECCIÓN: Agrupamos y mapeamos los datos para el nuevo reporte
            '    Aseguramos que los nombres de las propiedades sean los correctos: Id, Texto y Funcionarios
            Dim datosMapeados = datosCompletosParaReporte.
            GroupBy(Function(n) n.Id).
            Select(Function(g) New With {
                .Id = g.Key,
                .Fecha = g.First().Fecha,
                .Texto = g.First().Texto, ' Propiedad corregida de "Descripcion" a "Texto"
                .Funcionarios = g.Select(Function(n) New With {
                    .NombreFuncionario = n.NombreFuncionario
                }).ToList() ' Propiedad corregida de "Funcionarios"
            }).
            OrderByDescending(Function(n) n.Fecha).
            ToList()

            ' 5. Abrimos el formulario visor y le pasamos los datos
            Dim frmVisor As New frmNovedadesRPT(datosMapeados)


            ' Se obtiene una referencia al formulario Dashboard
            Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)

            ' Se llama al método público del Dashboard para abrir el formulario en el panel
            parentDashboard.AbrirFormEnPanel(frmVisor)

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al preparar la impresión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub

    ' Función auxiliar para cargar una imagen por defecto si el funcionario no tiene foto
    Private Function ObtenerImagenPorDefecto() As Byte()
        Try
            Dim img As Image = My.Resources.Police ' Asegúrate de que este recurso exista
            Using ms As New MemoryStream()
                img.Save(ms, Imaging.ImageFormat.Png)
                Return ms.ToArray()
            End Using
        Catch ex As Exception
            Return New Byte() {} ' Devuelve un array vacío si hay un error
        End Try
    End Function

#End Region
    ' Función auxiliar para cargar una imagen por defecto si el funcionario no tiene foto

End Class