Imports System.Data.Entity
Imports System.Linq

Public Class frmIncidenciasConfiguracion
    ' Se elimina la variable _uow
    Public Incidencia As TipoLicencia
    Private _esNuevo As Boolean
    Private _categorias As List(Of CategoriaAusencia)

    ' --- CONSTRUCTOR MODIFICADO ---
    ' Ya no recibe un IUnitOfWork. Recibe una instancia opcional para edición.
    Public Sub New(Optional incidenciaEditable As TipoLicencia = Nothing)
        InitializeComponent()

        If incidenciaEditable Is Nothing Then
            ' Modo "Agregar"
            Incidencia = New TipoLicencia()
            _esNuevo = True
            Me.Text = "Agregar Incidencia"
        Else
            ' Modo "Editar"
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
        SiEsEdicionCargarDatos() ' Carga los datos de la incidencia a editar

        Notifier.Info(Me, If(_esNuevo, "Creá una nueva incidencia.", "Editá la incidencia y guardá los cambios."))
    End Sub

    ' --- MÉTODO MODIFICADO ---
    ' Ahora crea su propio UnitOfWork para ser independiente.
    Private Async Function CargarCategorias() As Task
        Me.Cursor = Cursors.WaitCursor
        Try
            Using uow As New UnitOfWork()
                _categorias = Await uow.Repository(Of CategoriaAusencia)().
                    GetAll().
                    OrderBy(Function(c) c.Nombre).
                    AsNoTracking().
                    ToListAsync()
            End Using

            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList
            cboCategoria.DataSource = _categorias
            cboCategoria.DisplayMember = "Nombre"
            cboCategoria.ValueMember = "Id"
            cboCategoria.SelectedIndex = -1
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudieron cargar las categorías: {ex.Message}")
            _categorias = New List(Of CategoriaAusencia)()
            cboCategoria.DataSource = Nothing
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Function

    Private Sub SiEsEdicionCargarDatos()
        If _esNuevo OrElse Incidencia Is Nothing Then Return

        txtNombre.Text = Incidencia.Nombre
        chkEsAusencia.Checked = Incidencia.EsAusencia
        chkSuspendeViatico.Checked = Incidencia.SuspendeViatico
        chkAfectaPresentismo.Checked = Incidencia.AfectaPresentismo

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Se comprueba si el ID es mayor a 0 en lugar de usar .HasValue
        If Incidencia.CategoriaAusenciaId > 0 AndAlso _categorias IsNot Nothing Then
            cboCategoria.SelectedValue = Incidencia.CategoriaAusenciaId
        Else
            cboCategoria.SelectedIndex = -1
        End If
        ' --- FIN DE LA CORRECCIÓN ---
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim nombre As String = txtNombre.Text.Trim()
        If String.IsNullOrWhiteSpace(nombre) Then
            Notifier.Warn(Me, "El nombre de la incidencia no puede estar vacío.")
            txtNombre.Focus()
            Return
        End If

        ' Mapear campos al objeto público Incidencia
        Incidencia.Nombre = nombre
        Incidencia.EsAusencia = chkEsAusencia.Checked
        Incidencia.SuspendeViatico = chkSuspendeViatico.Checked
        Incidencia.AfectaPresentismo = chkAfectaPresentismo.Checked

        If cboCategoria.SelectedValue IsNot Nothing Then
            Incidencia.CategoriaAusenciaId = CType(cboCategoria.SelectedValue, Integer)
        Else
            ' Si el modelo no permitiera nulos, aquí deberíamos asignar 0
            ' Pero como la FK puede ser nula en la DB, la propiedad en el modelo debería ser Integer?
            ' Asumimos que 0 o un valor no seleccionado implica "sin categoría"
            Incidencia.CategoriaAusenciaId = 0 ' O manejar según la lógica de negocio
        End If

        ' Se asigna el DialogResult. El formulario principal se encargará de guardar.
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnCancelar.PerformClick()
            e.Handled = True
        End If
    End Sub

End Class