' Archivo: sixtocorbo/apex/Apex-0de320c5ad8f21b48a295ddfce12e6266297c13c/Apex/UI/frmGestionIncidencias.vb

Imports System.Data.Entity

Public Class frmIncidencias
    Private _uow As IUnitOfWork
    Private _tiposLicencia As List(Of TipoLicencia)

    Public Sub New()
        InitializeComponent()
        _uow = New UnitOfWork()
    End Sub

    Private Async Sub frmGestionIncidencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        Await CargarDatos()
    End Sub


    Private Async Function CargarDatos() As Task
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim repo = _uow.Repository(Of TipoLicencia)()

            _tiposLicencia = Await repo.GetAll().
            Include(Function(t) t.CategoriaAusencia).
            OrderByDescending(Function(t) t.Id).
            AsNoTracking().
            ToListAsync()

            dgvIncidencias.DataSource = Nothing
            dgvIncidencias.DataSource = _tiposLicencia
            ConfigurarGrilla()
            dgvIncidencias.ClearSelection()

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al cargar las incidencias: {ex.Message}")
            dgvIncidencias.DataSource = Nothing
        Finally
            Me.Cursor = oldCursor
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        dgvIncidencias.Columns.Clear()
        dgvIncidencias.AutoGenerateColumns = False
        dgvIncidencias.ReadOnly = True
        dgvIncidencias.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .HeaderText = "ID", .Width = 50})
        dgvIncidencias.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Nombre", .DataPropertyName = "Nombre", .HeaderText = "Nombre de la Incidencia", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
        Dim categoriaColumn As New DataGridViewTextBoxColumn()
        categoriaColumn.Name = "Categoria"
        categoriaColumn.HeaderText = "Categoría"
        categoriaColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        dgvIncidencias.Columns.Add(categoriaColumn)
        dgvIncidencias.Columns.Add(New DataGridViewCheckBoxColumn With {.DataPropertyName = "EsAusencia", .HeaderText = "Es Ausencia", .Width = 80})
        dgvIncidencias.Columns.Add(New DataGridViewCheckBoxColumn With {.DataPropertyName = "SuspendeViatico", .HeaderText = "Susp. Viático", .Width = 80})
        dgvIncidencias.Columns.Add(New DataGridViewCheckBoxColumn With {.DataPropertyName = "AfectaPresentismo", .HeaderText = "Afecta Pres.", .Width = 80})
    End Sub

    Private Sub dgvIncidencias_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvIncidencias.CellFormatting
        Dim dgv = CType(sender, DataGridView)
        If dgv Is Nothing OrElse dgv.DataSource Is Nothing Then Return
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 OrElse e.ColumnIndex >= dgv.Columns.Count Then Return
        If dgv.AllowUserToAddRows AndAlso e.RowIndex = dgv.NewRowIndex Then Return
        If e.RowIndex >= dgv.RowCount Then Return

        If dgv.Columns(e.ColumnIndex).Name = "Categoria" Then
            Dim lic As TipoLicencia = Nothing
            Try
                lic = TryCast(dgv.Rows(e.RowIndex).DataBoundItem, TipoLicencia)
            Catch
                Return
            End Try
            If lic IsNot Nothing AndAlso lic.CategoriaAusencia IsNot Nothing Then
                e.Value = lic.CategoriaAusencia.Nombre
                e.FormattingApplied = True
            End If
        End If
    End Sub


    Private Async Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Using frm As New frmIncidenciasConfiguracion(_uow)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Dim oldCursor = Me.Cursor
                btnAgregar.Enabled = False
                Me.Cursor = Cursors.WaitCursor
                Try
                    _uow.Repository(Of TipoLicencia).Add(frm.Incidencia)
                    Await _uow.CommitAsync()
                    Notifier.Success(Me, "Incidencia creada correctamente.")
                    Await CargarDatos()
                Catch ex As Exception
                    Notifier.[Error](Me, $"No se pudo crear la incidencia: {ex.Message}")
                Finally
                    Me.Cursor = oldCursor
                    btnAgregar.Enabled = True
                End Try
            End If
        End Using
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvIncidencias.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "Seleccioná una incidencia para editar.")
            Return
        End If
        If dgvIncidencias.AllowUserToAddRows AndAlso dgvIncidencias.CurrentRow.Index = dgvIncidencias.NewRowIndex Then
            Notifier.Warn(Me, "La fila nueva no es editable.")
            Return
        End If

        Dim idSeleccionado As Integer
        Dim idCell = dgvIncidencias.CurrentRow.Cells("Id")
        If idCell Is Nothing OrElse idCell.Value Is Nothing OrElse Not Integer.TryParse(idCell.Value.ToString(), idSeleccionado) Then
            Notifier.Warn(Me, "No se pudo determinar el ID de la incidencia.")
            Return
        End If

        Dim repo = _uow.Repository(Of TipoLicencia)()

        ' Cargar copia desconectada para edición
        Dim tipoLicenciaParaEditar = Await repo.GetAll().
        Include(Function(t) t.CategoriaAusencia).
        AsNoTracking().
        FirstOrDefaultAsync(Function(t) t.Id = idSeleccionado)

        If tipoLicenciaParaEditar Is Nothing Then
            Notifier.[Error](Me, "No se encontró el registro a editar.")
            Return
        End If

        Using frm As New frmIncidenciasConfiguracion(_uow, tipoLicenciaParaEditar)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Dim oldCursor = Me.Cursor
                btnEditar.Enabled = False
                Me.Cursor = Cursors.WaitCursor
                Try
                    ' Cargar la entidad original rastreada por el contexto
                    Dim entidadOriginal = Await _uow.Context.Set(Of TipoLicencia)().
                    FirstOrDefaultAsync(Function(t) t.Id = idSeleccionado)

                    If entidadOriginal Is Nothing Then
                        Notifier.[Error](Me, "La incidencia ya no existe en la base de datos.")
                        Return
                    End If

                    ' Copiar valores escalares
                    _uow.Context.Entry(entidadOriginal).CurrentValues.SetValues(frm.Incidencia)

                    ' Actualizar FK explícitamente
                    entidadOriginal.CategoriaAusenciaId = frm.Incidencia.CategoriaAusenciaId

                    Await _uow.CommitAsync()
                    Notifier.Success(Me, "Incidencia actualizada correctamente.")
                    Await CargarDatos()

                Catch ex As Exception
                    Notifier.[Error](Me, $"No se pudo actualizar la incidencia: {ex.Message}")
                Finally
                    Me.Cursor = oldCursor
                    btnEditar.Enabled = True
                End Try
            End If
        End Using
    End Sub


    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        NavegacionHelper.AbrirFormUnicoEnDashboard(Of frmConfiguracion)(Me)
    End Sub

    Private Sub frmGestionIncidencias_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _uow.Dispose()
    End Sub

End Class