Public Class frmLicenciaCrear

    Private _svc As LicenciaService
    Private _licencia As HistoricoLicencia
    Private _modo As ModoFormulario
    Private _idLicencia As Integer
    Private _estadoInicial As String = ""

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _licencia = New HistoricoLicencia()
        Me.Text = "Nueva Licencia"
    End Sub

    Public Sub New(id As Integer, Optional estadoActual As String = "")
        Me.New()
        _modo = ModoFormulario.Editar
        _idLicencia = id
        _estadoInicial = estadoActual
        Me.Text = "Editar Licencia"
    End Sub

    Private Async Sub frmLicenciaCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        Me.KeyPreview = True
        Me.AcceptButton = btnGuardar
        ' Si tenés botón Cancelar, podés setear:
        ' Me.CancelButton = btnCancelar

        _svc = New LicenciaService()

        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor
        Try
            Await CargarCombosAsync()

            If _modo = ModoFormulario.Editar Then
                Await CargarDatosAsync()
                If Not String.IsNullOrWhiteSpace(_estadoInicial) Then
                    Dim idx = cboEstado.FindStringExact(_estadoInicial)
                    If idx >= 0 Then cboEstado.SelectedIndex = idx
                End If
                Notifier.Info(Me, "Editá los datos y guardá los cambios.")
            Else
                Notifier.Info(Me, "Completá los datos de la nueva licencia.")
            End If

            Try
                AppTheme.SetCue(txtComentario, "Observaciones...")
                AppTheme.SetCue(cboFuncionario, "Seleccione un funcionario...")
                AppTheme.SetCue(cboTipoLicencia, "Seleccione un tipo de licencia...")
                AppTheme.SetCue(cboEstado, "Seleccione un estado...")
            Catch
                ' Ignorar si no existe SetCue
            End Try
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo inicializar el formulario: {ex.Message}")
            Close()
        Finally
            Me.Cursor = oldCursor
        End Try
    End Sub


    Private Async Function CargarCombosAsync() As Task
        ' Funcionario
        cboFuncionario.DropDownStyle = ComboBoxStyle.DropDownList
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        Dim funcionarios = Await _svc.ObtenerFuncionariosParaComboAsync()
        cboFuncionario.DataSource = funcionarios

        ' Tipo de Licencia
        cboTipoLicencia.DropDownStyle = ComboBoxStyle.DropDownList
        cboTipoLicencia.DisplayMember = "Value"
        cboTipoLicencia.ValueMember = "Key"
        Dim tipos = Await _svc.ObtenerTiposLicenciaParaComboAsync()
        cboTipoLicencia.DataSource = tipos

        ' Estados (cadena simple)
        Dim estadosExistentes = Await _svc.ObtenerEstadosDeLicenciaAsync()

        cboEstado.DropDownStyle = ComboBoxStyle.DropDown
        cboEstado.DataSource = Nothing
        cboEstado.Items.Clear()
        If estadosExistentes IsNot Nothing AndAlso estadosExistentes.Any() Then
            cboEstado.DataSource = estadosExistentes.ToList()
        Else
            cboEstado.Items.AddRange(New Object() {"Autorizado", "Rechazada", "Anulada", "Pendiente"})
        End If

        If _modo = ModoFormulario.Crear Then
            Dim idx = cboEstado.FindStringExact("Autorizado")
            cboEstado.SelectedIndex = If(idx >= 0, idx, -1)
            cboFuncionario.SelectedIndex = -1
            cboTipoLicencia.SelectedIndex = -1
        End If
    End Function


    Private Async Function CargarDatosAsync() As Task
        _licencia = Await _svc.GetByIdAsync(_idLicencia)

        If _licencia Is Nothing Then
            Notifier.[Error](Me, "No se encontró la licencia.")
            Close()
            Return
        End If

        ' Asegurar que el funcionario esté en el combo (aunque esté inactivo)
        Dim funcionariosSource = TryCast(cboFuncionario.DataSource, List(Of KeyValuePair(Of Integer, String)))
        If funcionariosSource IsNot Nothing AndAlso
   Not funcionariosSource.Any(Function(kvp) kvp.Key = _licencia.FuncionarioId) Then

            Dim funcionarioDeLicencia As Funcionario = Nothing
            Using uow As New UnitOfWork()
                funcionarioDeLicencia = Await uow.Repository(Of Funcionario)().
                                   GetByIdAsync(_licencia.FuncionarioId)
            End Using

            If funcionarioDeLicencia IsNot Nothing Then
                funcionariosSource.Add(New KeyValuePair(Of Integer, String)(
            funcionarioDeLicencia.Id, funcionarioDeLicencia.Nombre & " (Inactivo)"))
                cboFuncionario.DataSource = funcionariosSource.
                                    OrderBy(Function(kvp) kvp.Value).
                                    ToList()
            End If
        End If


        ' Setear valores
        cboFuncionario.SelectedValue = _licencia.FuncionarioId
        cboTipoLicencia.SelectedValue = _licencia.TipoLicenciaId
        dtpFechaInicio.Value = _licencia.inicio
        dtpFechaFin.Value = _licencia.finaliza

        Dim est = If(_licencia.estado, String.Empty).Trim()
        Dim idxEstado = cboEstado.FindStringExact(est)
        If idxEstado >= 0 Then
            cboEstado.SelectedIndex = idxEstado
        Else
            cboEstado.Text = est
        End If

        txtComentario.Text = _licencia.Comentario
    End Function


    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones básicas
        If cboFuncionario.SelectedIndex = -1 Then
            Notifier.Warn(Me, "Debe seleccionar un funcionario.")
            cboFuncionario.DroppedDown = True
            Return
        End If

        If cboTipoLicencia.SelectedIndex = -1 Then
            Notifier.Warn(Me, "Debe seleccionar un tipo de licencia.")
            cboTipoLicencia.DroppedDown = True
            Return
        End If

        If cboEstado.SelectedIndex = -1 AndAlso String.IsNullOrWhiteSpace(cboEstado.Text) Then
            Notifier.Warn(Me, "Debe seleccionar o escribir un estado.")
            cboEstado.DroppedDown = True
            Return
        End If

        If dtpFechaFin.Value.Date < dtpFechaInicio.Value.Date Then
            Notifier.Warn(Me, "La fecha de fin no puede ser anterior a la fecha de inicio.")
            Return
        End If

        ' Mapear entidad
        _licencia.FuncionarioId = CInt(cboFuncionario.SelectedValue)
        _licencia.TipoLicenciaId = CInt(cboTipoLicencia.SelectedValue)
        _licencia.inicio = dtpFechaInicio.Value.Date
        _licencia.finaliza = dtpFechaFin.Value.Date
        _licencia.estado = If(cboEstado.SelectedIndex >= 0,
                          cboEstado.SelectedItem.ToString(),
                          cboEstado.Text.Trim())
        _licencia.Comentario = txtComentario.Text.Trim()
        _licencia.usuario = "SISTEMA"

        Dim oldCursor = Me.Cursor
        btnGuardar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            If _modo = ModoFormulario.Crear Then
                _licencia.fecha_registro = DateTime.Now
                _licencia.fecha_actualizado = DateTime.Now
                Await _svc.CreateAsync(_licencia)
                Notifier.Success(Me, "Licencia creada correctamente.")
            Else
                _licencia.fecha_actualizado = DateTime.Now
                Await _svc.UpdateAsync(_licencia)
                Notifier.Success(Me, "Licencia actualizada correctamente.")
            End If

            ' Notificación genérica para que otras pantallas se refresquen si lo necesitan
            NotificadorEventos.NotificarActualizacionGeneral()

            Me.DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al guardar la licencia: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnGuardar.Enabled = True
        End Try
    End Sub


    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Close()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Enter AndAlso Not btnGuardar.Focused Then
            btnGuardar.PerformClick()
            e.Handled = True
        End If
    End Sub
    Private Sub dtpFechaInicio_ValueChanged(sender As Object, e As EventArgs) Handles dtpFechaInicio.ValueChanged
        If dtpFechaFin.Value.Date < dtpFechaInicio.Value.Date Then
            dtpFechaFin.Value = dtpFechaInicio.Value
        End If
    End Sub

    Private Sub cboEstado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboEstado.SelectedIndexChanged
        Dim est As String = If(cboEstado.SelectedItem, cboEstado.Text)?.ToString().Trim().ToUpperInvariant()
        If est = "RECHAZADA" OrElse est = "ANULADA" Then
            Notifier.Info(Me, "Recordá agregar una observación en Comentario.")
        End If
    End Sub

End Class
