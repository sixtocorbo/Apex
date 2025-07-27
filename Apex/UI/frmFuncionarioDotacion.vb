' Apex/UI/frmFuncionarioDotacion.vb

Public Class frmFuncionarioDotacion

    Public Dotacion As FuncionarioDotacion
    ' Se añade una instancia del servicio para poder cargar el catálogo de ítems.
    Private _svc As New FuncionarioService()

    Public Sub New(dotacion As FuncionarioDotacion)
        InitializeComponent()
        Me.Dotacion = dotacion
    End Sub

    ' Evento que se dispara al cargar el formulario.
    Private Async Sub frmFuncionarioDotacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Carga los ítems de dotación en el menú desplegable.
        Await CargarComboItems()

        ' Si estamos en modo "Editar", selecciona el ítem actual y carga los demás datos.
        If Dotacion IsNot Nothing AndAlso Dotacion.Id > 0 Then
            cboItem.SelectedValue = Dotacion.DotacionItemId
            txtTalla.Text = Dotacion.Talla
            txtObservaciones.Text = Dotacion.Observaciones
        End If
    End Sub

    ' Nuevo método para poblar el ComboBox desde la base de datos.
    Private Async Function CargarComboItems() As Task
        cboItem.DataSource = Await _svc.ObtenerItemsDotacionAsync()
        cboItem.DisplayMember = "Value"
        cboItem.ValueMember = "Key"
        cboItem.SelectedIndex = -1 ' Asegura que no haya nada seleccionado al inicio.
    End Function

    ' Lógica actualizada del botón Guardar para usar el ComboBox.
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If cboItem.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un ítem de la lista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dotacion.DotacionItemId = CInt(cboItem.SelectedValue) ' Guarda el ID del ítem seleccionado.
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