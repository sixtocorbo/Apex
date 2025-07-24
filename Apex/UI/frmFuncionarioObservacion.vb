Public Class frmFuncionarioObservacion
    Public Observacion As FuncionarioObservacion

    Public Sub New(observacion As FuncionarioObservacion)
        InitializeComponent()
        Me.Observacion = observacion
        If observacion IsNot Nothing Then
            txtCategoria.Text = observacion.Categoria
            txtTexto.Text = observacion.Texto
        End If
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtTexto.Text) Then
            MessageBox.Show("El texto de la observación es obligatorio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Observacion.Categoria = txtCategoria.Text.Trim()
        Observacion.Texto = txtTexto.Text.Trim()
        Observacion.FechaRegistro = DateTime.Now

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class