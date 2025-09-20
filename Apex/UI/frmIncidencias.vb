Imports System.Data.Entity
Imports System.Threading

Public Class frmIncidencias
    Private _tiposLicencia As List(Of TipoLicencia)
    Private _estaCargando As Boolean = False

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Async Sub frmGestionIncidencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        ConfigurarGrilla()
        Await CargarDatos()
    End Sub

    Private Async Function CargarDatos() As Task
        If _estaCargando Then Return
        _estaCargando = True
        Me.Cursor = Cursors.WaitCursor
        dgvIncidencias.Enabled = False

        Try
            Using uow As New UnitOfWork()
                _tiposLicencia = Await uow.Repository(Of TipoLicencia).GetAll().
                    Include(Function(t) t.CategoriaAusencia).
                    OrderBy(Function(t) t.Nombre).
                    AsNoTracking().
                    ToListAsync()
            End Using
            dgvIncidencias.DataSource = _tiposLicencia
            dgvIncidencias.ClearSelection()
        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al cargar las incidencias: {ex.Message}")
        Finally
            Me.Cursor = Cursors.Default
            dgvIncidencias.Enabled = True
            _estaCargando = False
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        ' --- NOTA IMPORTANTE ---
        ' Asegúrate de que en el Diseñador de Formularios, la propiedad
        ' dgvIncidencias.AutoSizeColumnsMode esté establecida en "None".

        dgvIncidencias.AutoGenerateColumns = False
        dgvIncidencias.Columns.Clear()

        ' --> LÍNEA AÑADIDA: Asegura que las barras de scroll aparezcan si son necesarias
        dgvIncidencias.ScrollBars = ScrollBars.Both

        dgvIncidencias.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .HeaderText = "ID", .Width = 50})
        dgvIncidencias.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Nombre", .DataPropertyName = "Nombre", .HeaderText = "Nombre de la Incidencia", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
        dgvIncidencias.Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Categoria", .HeaderText = "Categoría", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
        dgvIncidencias.Columns.Add(New DataGridViewCheckBoxColumn With {.DataPropertyName = "EsAusencia", .HeaderText = "Es Ausencia", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
        dgvIncidencias.Columns.Add(New DataGridViewCheckBoxColumn With {.DataPropertyName = "SuspendeViatico", .HeaderText = "Susp. Viático", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
        dgvIncidencias.Columns.Add(New DataGridViewCheckBoxColumn With {.DataPropertyName = "AfectaPresentismo", .HeaderText = "Afecta Pres.", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
    End Sub

    Private Sub dgvIncidencias_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvIncidencias.CellFormatting
        If e.RowIndex < 0 Then Return
        If dgvIncidencias.Columns(e.ColumnIndex).Name = "Categoria" Then
            Dim lic = TryCast(dgvIncidencias.Rows(e.RowIndex).DataBoundItem, TipoLicencia)
            If lic IsNot Nothing AndAlso lic.CategoriaAusencia IsNot Nothing Then
                e.Value = lic.CategoriaAusencia.Nombre
                e.FormattingApplied = True
            End If
        End If
    End Sub

    Private Async Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Using frm As New frmIncidenciasConfiguracion()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Me.Cursor = Cursors.WaitCursor
                Try
                    Using uow As New UnitOfWork()
                        uow.Repository(Of TipoLicencia).Add(frm.Incidencia)
                        Await uow.CommitAsync()
                    End Using
                    Notifier.Success(Me, "Incidencia creada.")
                    Await CargarDatos()
                Catch ex As Exception
                    Notifier.Error(Me, $"No se pudo crear la incidencia: {ex.Message}")
                Finally
                    Me.Cursor = Cursors.Default
                End Try
            End If
        End Using
    End Sub

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvIncidencias.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "Seleccioná una incidencia para editar.")
            Return
        End If
        Dim idSeleccionado = CInt(dgvIncidencias.CurrentRow.Cells("Id").Value)

        Dim entidadParaEditar As TipoLicencia
        Using uow As New UnitOfWork()
            entidadParaEditar = Await uow.Repository(Of TipoLicencia).GetByIdAsync(idSeleccionado)
        End Using

        If entidadParaEditar Is Nothing Then
            Notifier.Error(Me, "No se encontró el registro. Pudo haber sido eliminado.")
            Return
        End If

        Using frm As New frmIncidenciasConfiguracion(entidadParaEditar)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Me.Cursor = Cursors.WaitCursor
                Try
                    Using uow As New UnitOfWork()
                        uow.Repository(Of TipoLicencia).Update(frm.Incidencia)
                        Await uow.CommitAsync()
                    End Using
                    Notifier.Success(Me, "Incidencia actualizada.")
                    Await CargarDatos()
                Catch ex As Exception
                    Notifier.Error(Me, $"No se pudo actualizar: {ex.Message}")
                Finally
                    Me.Cursor = Cursors.Default
                End Try
            End If
        End Using
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class