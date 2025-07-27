' Apex/UI/frmFuncionarioDotacion.vb

Public Class frmFuncionarioDotacion

    Public Dotacion As FuncionarioDotacion
    ' Almacenará la lista completa de ítems, no solo pares de clave/valor.
    Private _itemsDotacion As List(Of DotacionItem)
    ' Expone el ítem seleccionado para que el formulario padre pueda acceder a él.
    Public ReadOnly Property ItemSeleccionado As DotacionItem
        Get
            Return CType(cboItem.SelectedItem, DotacionItem)
        End Get
    End Property
    Private _svc As New FuncionarioService()
    Public Sub New(dotacion As FuncionarioDotacion)
        InitializeComponent()
        Me.Dotacion = dotacion
    End Sub

    ' Evento que se dispara al cargar el formulario.
    Private Async Sub frmFuncionarioDotacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarComboItems()

        If Dotacion IsNot Nothing AndAlso Dotacion.Id > 0 Then
            ' Para la edición, busca y selecciona el objeto completo en el ComboBox.
            cboItem.SelectedItem = _itemsDotacion.FirstOrDefault(Function(i) i.Id = Dotacion.DotacionItemId)
            txtTalla.Text = Dotacion.Talla
            txtObservaciones.Text = Dotacion.Observaciones
        End If
    End Sub

    ' Nuevo método para poblar el ComboBox desde la base de datos.
    Private Async Function CargarComboItems() As Task
        ' Carga la lista de objetos completos.
        _itemsDotacion = Await _svc.ObtenerItemsDotacionCompletosAsync() ' Necesitarás crear este método en tu servicio.
        cboItem.DataSource = _itemsDotacion
        cboItem.DisplayMember = "Nombre" ' Muestra la propiedad Nombre.
        cboItem.ValueMember = "Id" ' El valor sigue siendo el Id.
        cboItem.SelectedIndex = -1
    End Function

    ' Lógica actualizada del botón Guardar para usar el ComboBox.
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If cboItem.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un ítem de la lista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dotacion.DotacionItemId = CInt(cboItem.SelectedValue)
        Dotacion.Talla = txtTalla.Text.Trim()
        Dotacion.Observaciones = txtObservaciones.Text.Trim()
        Dotacion.FechaAsign = DateTime.Now

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class