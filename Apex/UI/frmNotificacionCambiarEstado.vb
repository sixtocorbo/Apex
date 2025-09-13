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
        Me.KeyPreview = True
        Me.AcceptButton = btnAceptar
        Me.CancelButton = btnCancelar ' si no existe, quitá esta línea

        _svc = New NotificacionService()

        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            Await CargarCombosAsync()

            ' Seleccionar estado actual si vino por ctor
            If _idEstadoActual > 0 Then
                cboEstados.SelectedValue = _idEstadoActual
            Else
                cboEstados.SelectedIndex = -1
            End If

            Try
                AppTheme.SetCue(cboEstados, "Seleccione un estado...")
            Catch
                ' Ignorar si SetCue no está disponible
            End Try

            Notifier.Info(Me, "Elegí el nuevo estado y confirmá.")
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo cargar la lista de estados: {ex.Message}")
            Close()
        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub


    Private Async Function CargarCombosAsync() As Task
        cboEstados.DropDownStyle = ComboBoxStyle.DropDownList
        cboEstados.DisplayMember = "Value"
        cboEstados.ValueMember = "Key"

        Try
            cboEstados.DataSource = Await _svc.ObtenerEstadosParaComboAsync()
        Catch ex As Exception
            cboEstados.DataSource = Nothing
            Notifier.[Error](Me, $"Error recuperando estados: {ex.Message}")
            Throw
        End Try
    End Function


    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        If cboEstados.SelectedIndex = -1 OrElse cboEstados.SelectedValue Is Nothing Then
            Notifier.Warn(Me, "Debe seleccionar un estado.")
            cboEstados.DroppedDown = True
            Return
        End If

        Dim val As Byte
        If Not Byte.TryParse(cboEstados.SelectedValue.ToString(), val) Then
            Notifier.[Error](Me, "El estado seleccionado no es válido.")
            Return
        End If

        SelectedEstadoId = val
        Notifier.Success(Me, "Estado seleccionado.")
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            If btnCancelar IsNot Nothing Then
                btnCancelar.PerformClick()
            Else
                Me.Close()
            End If
            e.Handled = True
        ElseIf e.KeyCode = Keys.Enter AndAlso Not btnAceptar.Focused Then
            btnAceptar.PerformClick()
            e.Handled = True
        End If
    End Sub
    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Notifier.Info(Me, "Acción cancelada.")
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class