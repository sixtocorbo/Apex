' Apex/UI/frmCambiarEstadoNotificacion.vb

Public Class frmNotificacionCambiarEstado

    Private _svc As NotificacionService
    Public SelectedEstadoId As Byte

    ' --- INICIO DE LA CORRECCIÓN ---
    Private _idEstadoActual As Byte

    ' Constructor modificado para recibir el estado actual
    Public Sub New(Optional idEstadoActual As Byte = 0)
        InitializeComponent()
        _idEstadoActual = idEstadoActual
    End Sub
    ' --- FIN DE LA CORRECCIÓN ---

    Private Async Sub frmCambiarEstadoNotificacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        _svc = New NotificacionService()
        Await CargarCombosAsync()

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Si se recibió un estado actual, lo seleccionamos en el ComboBox
        If _idEstadoActual > 0 Then
            cboEstados.SelectedValue = _idEstadoActual
        End If
        ' --- FIN DE LA CORRECCIÓN ---
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
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class