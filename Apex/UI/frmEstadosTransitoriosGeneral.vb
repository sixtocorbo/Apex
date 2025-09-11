Imports System.Data.Entity

Public Class frmEstadosTransitoriosGeneral
    Private _unitOfWork As IUnitOfWork

    Public Sub New(uow As IUnitOfWork)
        InitializeComponent()
        _unitOfWork = uow
    End Sub

    Private Sub frmEstadosTransitoriosGeneral_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Try
            AppTheme.SetCue(txtFiltro, "Filtrar por nombre o CI del funcionario...")

        Catch
            ' Ignorar si no existe SetCue
        End Try
        CargarDatos()
    End Sub

    Private Sub CargarDatos()
        Dim repo = _unitOfWork.Repository(Of vw_EstadosTransitoriosCompletos)()
        Dim estados = repo.GetAll().ToList()
        dgvEstados.DataSource = estados
        ConfigurarGrilla()
    End Sub

    Private Sub ConfigurarGrilla()
        If dgvEstados.Rows.Count > 0 Then
            dgvEstados.Columns("Id").Visible = False
            dgvEstados.Columns("FuncionarioId").Visible = False
            dgvEstados.Columns("NombreCompleto").HeaderText = "Funcionario"
            dgvEstados.Columns("NombreCompleto").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvEstados.Columns("TipoEstado").HeaderText = "Tipo de Estado"
            dgvEstados.Columns("FechaDesde").HeaderText = "Fecha Desde"
            dgvEstados.Columns("FechaHasta").HeaderText = "Fecha Hasta"
        End If
    End Sub

    Private Sub txtFiltro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        FiltrarDatos()
    End Sub

    Private Sub FiltrarDatos()
        Dim filtro = txtFiltro.Text.ToLower()
        Dim repo = _unitOfWork.Repository(Of vw_EstadosTransitoriosCompletos)()
        Dim estados = repo.GetAll().Where(Function(e) e.NombreFuncionario.ToLower().Contains(filtro) Or e.CI.Contains(filtro)).ToList()
        dgvEstados.DataSource = estados
    End Sub

    Private Sub btnVerDetalles_Click(sender As Object, e As EventArgs) Handles btnVerDetalles.Click
        If dgvEstados.CurrentRow Is Nothing Then Return

        Dim estadoId = CInt(dgvEstados.CurrentRow.Cells("Id").Value)
        Dim estadoRepo = _unitOfWork.Repository(Of EstadoTransitorio)()
        Dim estado = estadoRepo.GetById(estadoId)

        If estado IsNot Nothing Then
            Using frm As New frmFuncionarioEstadoTransitorio(estado, _unitOfWork, True)
                frm.ShowDialog()
            End Using
        Else
            MessageBox.Show("No se pudo encontrar el estado seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

End Class