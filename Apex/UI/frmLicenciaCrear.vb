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

        ' ← clave para que ESC cierre el form aunque el foco esté en otro control
        Me.KeyPreview = True

        _svc = New LicenciaService()
        Await CargarCombosAsync()

        If _modo = ModoFormulario.Editar Then
            Await CargarDatosAsync()
            ' Si se pasó un estado inicial, intentamos seleccionarlo si existe
            If Not String.IsNullOrWhiteSpace(_estadoInicial) Then
                Dim idx = cboEstado.FindStringExact(_estadoInicial)
                If idx >= 0 Then cboEstado.SelectedIndex = idx
            End If
        End If

        Try
            AppTheme.SetCue(txtComentario, "Observaciones...")
            AppTheme.SetCue(cboFuncionario, "Seleccione un funcionario...")
            AppTheme.SetCue(cboTipoLicencia, "Seleccione un tipo de licencia...")
            AppTheme.SetCue(cboEstado, "Seleccione un estado...")
            AppTheme.SetCue(dtpFechaInicio, "Seleccione la fecha de inicio...")
            AppTheme.SetCue(dtpFechaFin, "Seleccione la fecha de fin...")
        Catch
            ' Ignorar si no existe SetCue
        End Try
    End Sub

    Private Async Function CargarCombosAsync() As Task
        ' Funcionario
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        cboFuncionario.DataSource = Await _svc.ObtenerFuncionariosParaComboAsync()

        ' Tipo de Licencia
        cboTipoLicencia.DisplayMember = "Value"
        cboTipoLicencia.ValueMember = "Key"
        cboTipoLicencia.DataSource = Await _svc.ObtenerTiposLicenciaParaComboAsync()

        ' Estados (cadena simple)
        Dim estadosExistentes = Await _svc.ObtenerEstadosDeLicenciaAsync()

        cboEstado.DataSource = Nothing
        cboEstado.Items.Clear()
        If estadosExistentes IsNot Nothing AndAlso estadosExistentes.Any() Then
            ' Esperamos una lista de strings; si fuera otra cosa, ajustar aquí
            cboEstado.DataSource = estadosExistentes.ToList()
        Else
            cboEstado.Items.AddRange(New Object() {"Autorizado", "Rechazada", "Anulada", "Pendiente de..."})
        End If

        If _modo = ModoFormulario.Crear Then
            ' Selección por defecto
            Dim idx = cboEstado.FindStringExact("Autorizado")
            cboEstado.SelectedIndex = If(idx >= 0, idx, -1)
            cboFuncionario.SelectedIndex = -1
            cboTipoLicencia.SelectedIndex = -1
        End If
    End Function

    Private Async Function CargarDatosAsync() As Task
        _licencia = Await _svc.GetByIdAsync(_idLicencia)

        If _licencia Is Nothing Then
            MessageBox.Show("No se encontró la licencia.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If

        ' Asegurar que el Funcionario de la licencia esté en el combo (aunque esté inactivo)
        Dim funcionariosSource = TryCast(cboFuncionario.DataSource, List(Of KeyValuePair(Of Integer, String)))
        If funcionariosSource IsNot Nothing AndAlso
           Not funcionariosSource.Any(Function(kvp) kvp.Key = _licencia.FuncionarioId) Then

            Dim funcionarioDeLicencia = Await _svc.UnitOfWork.Repository(Of Funcionario)().GetByIdAsync(_licencia.FuncionarioId)
            If funcionarioDeLicencia IsNot Nothing Then
                funcionariosSource.Add(New KeyValuePair(Of Integer, String)(
                    funcionarioDeLicencia.Id, funcionarioDeLicencia.Nombre & " (Inactivo)"))
                cboFuncionario.DataSource = funcionariosSource.OrderBy(Function(kvp) kvp.Value).ToList()
            End If
        End If

        ' Setear valores
        cboFuncionario.SelectedValue = _licencia.FuncionarioId
        cboTipoLicencia.SelectedValue = _licencia.TipoLicenciaId
        dtpFechaInicio.Value = _licencia.inicio
        dtpFechaFin.Value = _licencia.finaliza

        ' Estado (si existe en la lista, seleccionarlo; si no, texto libre)
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
        If cboFuncionario.SelectedIndex = -1 OrElse
           cboTipoLicencia.SelectedIndex = -1 OrElse
           (cboEstado.SelectedIndex = -1 AndAlso String.IsNullOrWhiteSpace(cboEstado.Text)) Then

            MessageBox.Show("Debe seleccionar un funcionario, un tipo de licencia y un estado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If dtpFechaFin.Value.Date < dtpFechaInicio.Value.Date Then
            MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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

        Try
            If _modo = ModoFormulario.Crear Then
                _licencia.fecha_registro = DateTime.Now
                _licencia.fecha_actualizado = DateTime.Now
                Await _svc.CreateAsync(_licencia)
            Else
                _licencia.fecha_actualizado = DateTime.Now
                Await _svc.UpdateAsync(_licencia)
            End If

            ' Notificación genérica para que otras pantallas se refresquen si lo necesitan
            NotificadorEventos.NotificarActualizacionGeneral()

            ' Cerrar (el Dashboard restaurará el formulario anterior de la pila)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la licencia: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Atajo de teclado para cerrar/volver (la pila del Dashboard hace el resto)
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    ' Si tenés un botón Cancelar/Volver en el diseñador, asignale este handler:
    ' Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
    '     Me.Close()
    ' End Sub

End Class
