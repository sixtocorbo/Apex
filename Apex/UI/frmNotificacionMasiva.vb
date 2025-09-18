Imports System.ComponentModel

Public Class frmNotificacionMasiva
    Private ReadOnly _svc As New NotificacionService()
    Private _destinatarios As New BindingList(Of KeyValuePair(Of Integer, String))() ' Id, Nombre

    Private Sub frmNotificacionMasiva_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        lstDestinatarios.DataSource = _destinatarios
        lstDestinatarios.DisplayMember = "Value"
        lstDestinatarios.ValueMember = "Key"
        CargarTiposAsync()
        Try
            AppTheme.SetCue(txtMedio, "Texto a notificar…")
            AppTheme.SetCue(txtDocumento, "N° documento…")
            AppTheme.SetCue(txtExpMinisterial, "Expediente ministerial…")
            AppTheme.SetCue(txtExpINR, "Exp. INR…")
            AppTheme.SetCue(txtOficina, "Oficina…")
        Catch
        End Try
    End Sub

    Private Async Sub CargarTiposAsync()
        cboTipoNotificacion.DisplayMember = "Value"
        cboTipoNotificacion.ValueMember = "Key"
        cboTipoNotificacion.DataSource = Await _svc.ObtenerTiposNotificacionParaComboAsync()
        cboTipoNotificacion.SelectedIndex = -1
    End Sub

    Private Sub btnSeleccionarDestinatarios_Click(sender As Object, e As EventArgs) Handles btnSeleccionarDestinatarios.Click
        Using f As New frmFiltros()
            ' Preconfigurar a "Funcionarios"
            f.ShowDialog(Me)
            ' Tomamos TODOS los IDs visibles tras filtros (o los seleccionados si preferís)
            Dim ids As List(Of Integer) = f.GetIdsFiltrados("Id") ' ver helper más abajo
            Dim nombres As Dictionary(Of Integer, String) = f.GetDiccionarioIdNombre("Id", "NombreCompleto")

            _destinatarios.Clear()
            For Each id In ids
                Dim nombre = If(nombres.ContainsKey(id), nombres(id), $"Funcionario #{id}")
                _destinatarios.Add(New KeyValuePair(Of Integer, String)(id, $"{nombre} ({id})"))
            Next

            Notifier.Info(Me, $"Destinatarios cargados: {_destinatarios.Count}.")
        End Using
    End Sub

    Private Async Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        If cboTipoNotificacion.SelectedIndex = -1 Then
            Notifier.Warn(Me, "Seleccioná un tipo de notificación.")
            Return
        End If
        If _destinatarios.Count = 0 Then
            Notifier.Warn(Me, "No hay destinatarios.")
            Return
        End If

        Dim confirma = MessageBox.Show(
            $"Se crearán notificaciones para {_destinatarios.Count} funcionarios." & vbCrLf &
            $"Tipo: {CStr(cboTipoNotificacion.Text)} — Fecha: {dtpFechaProgramada.Value:g}" & vbCrLf &
            "¿Confirmar?",
            "Confirmar notificación masiva", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirma <> DialogResult.Yes Then Return

        ToggleUI(False)
        pbProgreso.Value = 0
        pbProgreso.Maximum = _destinatarios.Count

        Dim baseReq As New NotificacionCreateRequest With {
            .TipoNotificacionId = CByte(cboTipoNotificacion.SelectedValue),
            .FechaProgramada = dtpFechaProgramada.Value,
            .Medio = txtMedio.Text.Trim(),
            .Documento = txtDocumento.Text.Trim(),
            .ExpMinisterial = txtExpMinisterial.Text.Trim(),
            .ExpINR = txtExpINR.Text.Trim(),
            .Oficina = txtOficina.Text.Trim()
        }

        Dim ids = _destinatarios.Select(Function(kv) kv.Key).ToList()

        Try
            Dim resultado = Await _svc.CreateNotificacionesMasivasAsync(
                ids, baseReq,
                skipDuplicadas:=True,
                reportProgress:=Sub() Me.BeginInvoke(Sub() pbProgreso.Increment(1))
            )

            ' <<< CAMBIO CLAVE: La notificación se hace aquí, DESPUÉS de que la operación masiva termine >>>
            If resultado.Creadas > 0 Then
                ' Notificamos una sola vez que algo cambió en la aplicación.
                ' Después
                NotificadorEventos.NotificarRefrescoTotal()

            End If

            ' Resumen UX
            Dim msg =
                $"Creadas: {resultado.Creadas} | Duplicadas (omitidas): {resultado.OmitidasPorDuplicado} | Errores: {resultado.Errores.Count}"
            If resultado.Errores.Count > 0 Then
                msg &= vbCrLf & "Ver detalle en el panel inferior."
                ' Si querés, mostrás un DataGridView con resultado.Errores (Id, Mensaje)
            End If

            If resultado.Errores.Count = 0 AndAlso resultado.OmitidasPorDuplicado = 0 Then
                Notifier.Success(Me, msg)
            ElseIf resultado.Errores.Count = 0 Then
                Notifier.Warn(Me, msg)
            Else
                Notifier.Error(Me, msg)
            End If

        Catch ex As Exception
            Notifier.Error(Me, "Fallo el proceso masivo: " & ex.Message)
        Finally
            ToggleUI(True)
            pbProgreso.Value = pbProgreso.Maximum
        End Try
    End Sub

    Private Sub ToggleUI(enabled As Boolean)
        For Each ctl As Control In Me.Controls
            ctl.Enabled = enabled
        Next
        pbProgreso.Enabled = True
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub
End Class
