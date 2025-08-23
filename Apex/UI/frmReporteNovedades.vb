Imports System.Data.Entity
Imports System.Windows.Forms

Public Class frmReporteNovedades
    Private _uow As IUnitOfWork
    Private _funcionariosSeleccionados As New Dictionary(Of Integer, String)

    Public Sub New()
        InitializeComponent()
        _uow = New UnitOfWork()
    End Sub

    Private Sub frmReporteNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configuración inicial de los controles de fecha
        dtpFechaDesde.Value = DateTime.Now.AddMonths(-1)
        dtpFechaHasta.Value = DateTime.Now
    End Sub

#Region "Filtro de Funcionarios"

    Private Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        Using frm As New frmFuncionarioBuscar()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Dim funcId = frm.FuncionarioId
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
End Class