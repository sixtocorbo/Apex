' Apex/UI/frmFuncionarioDotacion.vb
Public Class frmFuncionarioDotacion

    Public Dotacion As FuncionarioDotacion
    Private _svc As New FuncionarioService()

    ' Propiedad pública para que el formulario padre pueda obtener el objeto completo.
    Public ReadOnly Property ItemSeleccionado As DotacionItem
        Get
            Return CType(cboItem.SelectedItem, DotacionItem)
        End Get
    End Property

    Public Sub New(dotacion As FuncionarioDotacion)
        InitializeComponent()
        Me.Dotacion = dotacion
    End Sub

    Private Async Sub frmFuncionarioDotacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Await CargarComboItems()

        If Dotacion IsNot Nothing AndAlso Dotacion.Id > 0 Then
            ' Selecciona el objeto completo en el ComboBox durante la edición.
            cboItem.SelectedValue = Dotacion.DotacionItemId
            txtTalla.Text = Dotacion.Talla
            txtObservaciones.Text = Dotacion.Observaciones
        End If
    End Sub

    ' Carga los objetos completos de DotacionItem.
    Private Async Function CargarComboItems() As Task
        cboItem.DataSource = Await _svc.ObtenerItemsDotacionCompletosAsync()
        cboItem.DisplayMember = "Nombre" ' La propiedad a mostrar
        cboItem.ValueMember = "Id"       ' El valor subyacente
        cboItem.SelectedIndex = -1
    End Function

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If cboItem.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un ítem de la lista.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' --- INICIO DE LA CORRECCIÓN CLAVE ---
        ' Asignamos únicamente el ID del ítem de dotación.
        ' Esto evita que Entity Framework intente insertar un nuevo DotacionItem duplicado.
        Dotacion.DotacionItemId = CInt(cboItem.SelectedValue)
        ' La línea que asignaba el objeto completo ha sido eliminada.
        ' --- FIN DE LA CORRECCIÓN CLAVE ---

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