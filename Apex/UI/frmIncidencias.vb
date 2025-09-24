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
        dgvIncidencias.ActivarDobleBuffer(True)
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
        With dgvIncidencias
            ' --- CONFIGURACIÓN GENERAL ---
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .GridColor = Color.FromArgb(230, 230, 230)
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AutoGenerateColumns = False
            .BackgroundColor = Color.White
            .ScrollBars = ScrollBars.Both

            ' --- ESTILO DE ENCABEZADOS (Headers) ---
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

            ' --- ESTILO DE FILAS (Rows) ---
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247) ' Efecto Cebra

            ' --- DEFINICIÓN DE COLUMNAS ---
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .HeaderText = "ID", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 60})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Nombre", .DataPropertyName = "Nombre", .HeaderText = "Nombre de la Incidencia", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .MinimumWidth = 250})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Categoria", .HeaderText = "Categoría", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 150})

            ' Centrar los CheckBox se ve mucho mejor
            Dim chkAusencia As New DataGridViewCheckBoxColumn With {.DataPropertyName = "EsAusencia", .HeaderText = "Es Ausencia", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 90}
            chkAusencia.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            chkAusencia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns.Add(chkAusencia)

            Dim chkViatico As New DataGridViewCheckBoxColumn With {.DataPropertyName = "SuspendeViatico", .HeaderText = "Susp. Viático", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 90}
            chkViatico.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            chkViatico.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns.Add(chkViatico)

            Dim chkPresentismo As New DataGridViewCheckBoxColumn With {.DataPropertyName = "AfectaPresentismo", .HeaderText = "Afecta Pres.", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 90}
            chkPresentismo.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            chkPresentismo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns.Add(chkPresentismo)
        End With
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