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
        ' Validación: ítem requerido
        If cboItem.SelectedIndex = -1 OrElse cboItem.SelectedValue Is Nothing Then
            Notifier.Warn(Me, "Debe seleccionar un ítem de la lista.")
            Return
        End If

        Dim oldCursor = Me.Cursor
        btnGuardar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            ' --- CORRECCIÓN CLAVE ---
            ' Asignar sólo el ID para evitar que EF intente insertar un DotacionItem duplicado.
            Dim dotacionItemId As Integer
            If Not Integer.TryParse(cboItem.SelectedValue.ToString(), dotacionItemId) Then
                Notifier.Warn(Me, "El ítem seleccionado no es válido.")
                Return
            End If
            Dotacion.DotacionItemId = dotacionItemId
            ' (Importante: NO asignar la navegación DotacionItem = cboItem.SelectedItem)

            ' Otros campos
            Dotacion.Talla = txtTalla.Text.Trim()
            Dotacion.Observaciones = txtObservaciones.Text.Trim()
            Dotacion.FechaAsign = DateTime.Now

            ' Éxito (mostrar antes de cerrar)
            Notifier.Success(Me, "Dotación guardada correctamente.")
            DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al guardar la dotación: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnGuardar.Enabled = True
        End Try
    End Sub



    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Close()
        End If
    End Sub
End Class