Public Class frmCambiarEstadoNotificacion

    Private _svc As NotificacionService
    Public SelectedEstadoId As Byte

    Private Async Sub frmCambiarEstadoNotificacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New NotificacionService()
        Await CargarCombosAsync()
    End Sub

    Private Async Function CargarCombosAsync() As Task
        cboEstados.DisplayMember = "Value"
        cboEstados.ValueMember = "Key"
        cboEstados.DataSource = Await _svc.ObtenerEstadosParaComboAsync()
    End Function

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        If cboEstados.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un estado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        SelectedEstadoId = CByte(cboEstados.SelectedValue)
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

End Class