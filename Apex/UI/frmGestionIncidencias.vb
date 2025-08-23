Imports System.Data.Entity

Public Class frmGestionIncidencias
    Private _uow As IUnitOfWork
    Private _tiposLicencia As List(Of TipoLicencia)

    Public Sub New()
        InitializeComponent()
        _uow = New UnitOfWork()
    End Sub

    Private Async Sub frmGestionIncidencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarDatos()
    End Sub

    Private Async Function CargarDatos() As Task
        Dim repo = _uow.Repository(Of TipoLicencia)()
        _tiposLicencia = Await repo.GetAll().Include(Function(t) t.CategoriaAusencia).ToListAsync()
        dgvIncidencias.DataSource = Nothing
        dgvIncidencias.DataSource = _tiposLicencia
        ConfigurarGrilla()
    End Function

    Private Sub ConfigurarGrilla()
        dgvIncidencias.Columns.Clear()
        dgvIncidencias.AutoGenerateColumns = False
        dgvIncidencias.ReadOnly = True

        dgvIncidencias.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Id", .DataPropertyName = "Id", .HeaderText = "ID", .Width = 50})

        dgvIncidencias.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Nombre", .DataPropertyName = "Nombre", .HeaderText = "Nombre de la Incidencia", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})

        Dim categoriaColumn As New DataGridViewTextBoxColumn()
        categoriaColumn.Name = "Categoria"
        categoriaColumn.HeaderText = "Categoría"
        categoriaColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        dgvIncidencias.Columns.Add(categoriaColumn)

        dgvIncidencias.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "EsAusencia", .HeaderText = "Es Ausencia", .Width = 80})

        dgvIncidencias.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "SuspendeViatico", .HeaderText = "Susp. Viático", .Width = 80})

        dgvIncidencias.Columns.Add(New DataGridViewCheckBoxColumn With {
            .DataPropertyName = "AfectaPresentismo", .HeaderText = "Afecta Pres.", .Width = 80})
    End Sub

    Private Sub dgvIncidencias_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvIncidencias.CellFormatting
        If e.RowIndex >= 0 AndAlso dgvIncidencias.Columns(e.ColumnIndex).Name = "Categoria" Then
            Dim licencia = TryCast(dgvIncidencias.Rows(e.RowIndex).DataBoundItem, TipoLicencia)
            If licencia IsNot Nothing AndAlso licencia.CategoriaAusencia IsNot Nothing Then
                e.Value = licencia.CategoriaAusencia.Nombre
                e.FormattingApplied = True
            End If
        End If
    End Sub

    Private Async Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Using frm As New frmIncidenciaDetalle(_uow)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                _uow.Repository(Of TipoLicencia).Add(frm.Incidencia)
                Await _uow.CommitAsync()
                Await CargarDatos()
            End If
        End Using
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvIncidencias.CurrentRow Is Nothing Then Return

        Dim idSeleccionado = CInt(dgvIncidencias.CurrentRow.Cells("Id").Value)

        Dim repo = _uow.Repository(Of TipoLicencia)()
        Dim tipoLicencia = Await repo.GetAll().Include(Function(t) t.CategoriaAusencia).FirstOrDefaultAsync(Function(t) t.Id = idSeleccionado)

        If tipoLicencia Is Nothing Then
            MessageBox.Show("No se encontró el registro a editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Using frm As New frmIncidenciaDetalle(_uow, tipoLicencia)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                _uow.Context.Entry(frm.Incidencia).State = EntityState.Modified
                Await _uow.CommitAsync()
                Await CargarDatos()
            End If
        End Using
    End Sub

    ' SE ELIMINA LA LÓGICA DE ELIMINAR
    ' Private Async Sub btnEliminar_Click(...)

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Close()
    End Sub

    Private Sub frmGestionIncidencias_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _uow.Dispose()
    End Sub
End Class