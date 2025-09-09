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
        Await CargarDatos()
        AppTheme.Aplicar(Me)
    End Sub

    Private Async Function CargarDatos() As Task
        Dim repo = _uow.Repository(Of TipoLicencia)()

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Se agrega AsNoTracking() para evitar conflictos de seguimiento en el futuro.
        _tiposLicencia = Await repo.GetAll().
        Include(Function(t) t.CategoriaAusencia).
        OrderByDescending(Function(t) t.Id).
        AsNoTracking().
        ToListAsync()
        ' --- FIN DE LA CORRECCIÓN ---

        dgvIncidencias.DataSource = Nothing
        dgvIncidencias.DataSource = _tiposLicencia
        ConfigurarGrilla()
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
        If e.RowIndex >= 0 AndAlso dgvIncidencias.Columns(e.ColumnIndex).Name = "Categoria" Then
            Dim licencia = TryCast(dgvIncidencias.Rows(e.RowIndex).DataBoundItem, TipoLicencia)
            If licencia IsNot Nothing AndAlso licencia.CategoriaAusencia IsNot Nothing Then
                e.Value = licencia.CategoriaAusencia.Nombre
                e.FormattingApplied = True
            End If
        End If
    End Sub

    Private Async Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Using frm As New frmIncidenciasConfiguracion(_uow)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                _uow.Repository(Of TipoLicencia).Add(frm.Incidencia)
                Await _uow.CommitAsync()
                Await CargarDatos()
            End If
        End Using
    End Sub


    ' Archivo: sixtocorbo/apex/Apex-aabdb9cacb3f1a24cadc076438a2c915e94e714c/Apex/UI/frmGestionIncidencias.vb

    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgvIncidencias.CurrentRow Is Nothing Then Return

        Dim idSeleccionado = CInt(dgvIncidencias.CurrentRow.Cells("Id").Value)
        Dim repo = _uow.Repository(Of TipoLicencia)()

        ' Paso 1: Cargar una copia "desconectada" para la edición, para no interferir con el contexto.
        Dim tipoLicenciaParaEditar = Await repo.GetAll().
    Include(Function(t) t.CategoriaAusencia).
    AsNoTracking().
    FirstOrDefaultAsync(Function(t) t.Id = idSeleccionado)

        If tipoLicenciaParaEditar Is Nothing Then
            MessageBox.Show("No se encontró el registro a editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Paso 2: Abrir el formulario de detalle para que el usuario haga los cambios.
        Using frm As New frmIncidenciasConfiguracion(_uow, tipoLicenciaParaEditar)
            If frm.ShowDialog(Me) = DialogResult.OK Then

                ' --- INICIO DE LA SOLUCIÓN FINAL ---

                ' Paso 3: Cargar la entidad ORIGINAL que el contexto SÍ está rastreando.
                ' CORRECCIÓN: Se obtiene la entidad directamente del context para que sea rastreada.
                Dim entidadOriginal = Await _uow.Context.Set(Of TipoLicencia)().
                                    FirstOrDefaultAsync(Function(t) t.Id = idSeleccionado)

                If entidadOriginal IsNot Nothing Then
                    ' Paso 4: Copiar los valores escalares (texto, checkboxes, etc.)
                    ' desde el objeto editado (frm.Incidencia) al objeto original.
                    _uow.Context.Entry(entidadOriginal).CurrentValues.SetValues(frm.Incidencia)

                    ' Paso 5: Actualizar explícitamente la clave foránea de la relación.
                    entidadOriginal.CategoriaAusenciaId = frm.Incidencia.CategoriaAusenciaId

                    ' Paso 6: Guardar el objeto original, que ahora tiene los valores actualizados.
                    Await _uow.CommitAsync()
                End If

                ' --- FIN DE LA SOLUCIÓN FINAL ---

                ' Paso 7: Refrescar la grilla.
                Await CargarDatos()
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