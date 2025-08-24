' Archivo: sixtocorbo/apex/Apex-0de320c5ad8f21b48a295ddfce12e6266297c13c/Apex/UI/frmIncidenciaDetalle.vb

Imports System.Data.Entity

Public Class frmIncidenciaDetalle
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
        Await CargarCategorias()
        SiEsEdicionCargarDatos()
    End Sub

    Private Async Function CargarCategorias() As Task
        _categorias = Await _uow.Repository(Of CategoriaAusencia)().GetAll().ToListAsync()
        cboCategoria.DataSource = _categorias
        cboCategoria.DisplayMember = "Nombre"
        cboCategoria.ValueMember = "Id"
    End Function

    Private Sub SiEsEdicionCargarDatos()
        If Not _esNuevo Then
            txtNombre.Text = Incidencia.Nombre
            chkEsAusencia.Checked = Incidencia.EsAusencia
            chkSuspendeViatico.Checked = Incidencia.SuspendeViatico
            chkAfectaPresentismo.Checked = Incidencia.AfectaPresentismo

            ' --- INICIO DE LA CORRECCIÓN ---
            '
            ' Se cambia la comprobación de "HasValue" por una comprobación
            ' para ver si el ID es un número válido (mayor que 0).
            '
            If Incidencia.CategoriaAusenciaId > 0 Then
                cboCategoria.SelectedValue = Incidencia.CategoriaAusenciaId
            Else
                cboCategoria.SelectedIndex = -1 ' No hay categoría seleccionada
            End If
            ' --- FIN DE LA CORRECCIÓN ---
        End If
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Incidencia.Nombre = txtNombre.Text.Trim()
        Incidencia.EsAusencia = chkEsAusencia.Checked
        Incidencia.SuspendeViatico = chkSuspendeViatico.Checked
        Incidencia.AfectaPresentismo = chkAfectaPresentismo.Checked

        ' --- INICIO DE LA CORRECCIÓN CLAVE ---
        ' Sincronizamos tanto el ID como el objeto de navegación.
        ' Esto previene el error de integridad referencial.
        If cboCategoria.SelectedValue IsNot Nothing Then
            Dim selectedCategoryId = CInt(cboCategoria.SelectedValue)
            Incidencia.CategoriaAusenciaId = selectedCategoryId
            ' Asignamos el objeto completo desde la lista que ya cargamos.
            Incidencia.CategoriaAusencia = _categorias.FirstOrDefault(Function(c) c.Id = selectedCategoryId)
        Else
            Incidencia.CategoriaAusenciaId = Nothing
            Incidencia.CategoriaAusencia = Nothing
        End If
        ' --- FIN DE LA CORRECCIÓN CLAVE ---

        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub
End Class