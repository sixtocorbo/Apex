Public Class frmFuncionarioDotacion

    Public Dotacion As FuncionarioDotacion

    Public Sub New(dotacion As FuncionarioDotacion)
        InitializeComponent()
        Me.Dotacion = dotacion
        If dotacion IsNot Nothing Then
            txtItem.Text = dotacion.Item
            txtTalla.Text = dotacion.Talla
            txtObservaciones.Text = dotacion.Observaciones
        End If
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtItem.Text) Then
            MessageBox.Show("El ítem es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dotacion.Item = txtItem.Text.Trim()
        Dotacion.Talla = txtTalla.Text.Trim()
        Dotacion.Observaciones = txtObservaciones.Text.Trim()
        Dotacion.FechaAsign = DateTime.Now ' Asignar la fecha actual

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class