' Archivo: sixtocorbo/apex/Apex-0de320c5ad8f21b48a295ddfce12e6266297c13c/Apex/UI/frmIncidenciaDetalle.vb

Imports System.Data.Entity

Public Class frmIncidenciasConfiguracion
    Private _uow As IUnitOfWork
    ' --- CORRECCIÓN 1: La propiedad ahora es pública para acceder desde el form principal. ---
    Public Incidencia As TipoLicencia
    Private _esNuevo As Boolean
    Private _categorias As List(Of CategoriaAusencia)

    Public Sub New(uow As IUnitOfWork, Optional incidenciaEditable As TipoLicencia = Nothing)
        InitializeComponent()
        _uow = uow
        ' --- CORRECCIÓN 2: Se trabaja directamente con la instancia pasada o se crea una nueva. ---
        If incidenciaEditable Is Nothing Then
            ' Modo "Agregar": Creamos una nueva instancia
            Incidencia = New TipoLicencia()
            _esNuevo = True
            Me.Text = "Agregar Incidencia"
        Else
            ' Modo "Editar": Usamos la instancia existente directamente
            Incidencia = incidenciaEditable
            _esNuevo = False
            Me.Text = "Editar Incidencia"
        End If
    End Sub

    Private Async Sub frmIncidenciaDetalle_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        Me.AcceptButton = btnGuardar
        Me.CancelButton = btnCancelar

        Await CargarCategorias()
        SiEsEdicionCargarDatos()

        Notifier.Info(Me, If(_esNuevo, "Creá una nueva incidencia.", "Editá la incidencia y guardá los cambios."))
    End Sub


    Private Async Function CargarCategorias() As Task
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            _categorias = Await _uow.Repository(Of CategoriaAusencia)().
            GetAll().
            OrderBy(Function(c) c.Nombre).
            AsNoTracking().
            ToListAsync()

            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList
            cboCategoria.DataSource = _categorias
            cboCategoria.DisplayMember = "Nombre"
            cboCategoria.ValueMember = "Id"
            cboCategoria.SelectedIndex = -1
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudieron cargar las categorías: {ex.Message}")
            _categorias = New List(Of CategoriaAusencia)()
            cboCategoria.DataSource = Nothing
        Finally
            Me.Cursor = oldCursor
        End Try
    End Function


    Private Sub SiEsEdicionCargarDatos()
        If _esNuevo OrElse Incidencia Is Nothing Then Return

        txtNombre.Text = Incidencia.Nombre
        chkEsAusencia.Checked = Incidencia.EsAusencia
        chkSuspendeViatico.Checked = Incidencia.SuspendeViatico
        chkAfectaPresentismo.Checked = Incidencia.AfectaPresentismo

        If Incidencia.CategoriaAusenciaId > 0 AndAlso _categorias IsNot Nothing Then
            Dim exists = _categorias.Any(Function(c) c.Id = Incidencia.CategoriaAusenciaId)
            cboCategoria.SelectedValue = If(exists, Incidencia.CategoriaAusenciaId, -1)
        Else
            cboCategoria.SelectedIndex = -1
        End If
    End Sub


    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validación: nombre requerido
        Dim nombre As String = If(txtNombre.Text, String.Empty).Trim()
        If nombre.Length = 0 Then
            Notifier.Warn(Me, "El nombre de la incidencia no puede estar vacío.")
            txtNombre.Focus()
            txtNombre.SelectAll()
            Return
        End If

        ' Mapear campos
        Incidencia.Nombre = nombre
        Incidencia.EsAusencia = chkEsAusencia.Checked
        Incidencia.SuspendeViatico = chkSuspendeViatico.Checked
        Incidencia.AfectaPresentismo = chkAfectaPresentismo.Checked

        ' Sincronizar FK + navegación (categoría puede ser opcional)
        If cboCategoria.SelectedValue IsNot Nothing Then
            Dim selectedId As Integer
            If Integer.TryParse(cboCategoria.SelectedValue.ToString(), selectedId) AndAlso selectedId > 0 Then
                Incidencia.CategoriaAusenciaId = selectedId
                If _categorias IsNot Nothing Then
                    Incidencia.CategoriaAusencia = _categorias.FirstOrDefault(Function(c) c.Id = selectedId)
                Else
                    Incidencia.CategoriaAusencia = Nothing
                End If
            Else
                Incidencia.CategoriaAusenciaId = Nothing
                Incidencia.CategoriaAusencia = Nothing
            End If
        Else
            Incidencia.CategoriaAusenciaId = Nothing
            Incidencia.CategoriaAusencia = Nothing
        End If

        ' Éxito: cerramos con OK y avisamos
        Notifier.Success(Me, If(_esNuevo, "Incidencia lista para crear.", "Cambios listos para guardar."))
        Me.DialogResult = DialogResult.OK
        ' (ShowDialog cerrará el form automáticamente al establecer DialogResult)
    End Sub


    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Notifier.Info(Me, "Edición cancelada.")
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnCancelar.PerformClick()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Enter AndAlso Not btnGuardar.Focused Then
            btnGuardar.PerformClick()
            e.Handled = True
        End If
    End Sub

End Class