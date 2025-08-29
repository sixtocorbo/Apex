Public Class frmLicenciaCrear

    Private _svc As LicenciaService
    Private _licencia As HistoricoLicencia
    Private _modo As ModoFormulario
    Private _idLicencia As Integer
    Private _estadoInicial As String = "" ' Variable para guardar el estado que viene de la grilla (para editar)

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    ' Constructor para Crear
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _licencia = New HistoricoLicencia()
        Me.Text = "Nueva Licencia"
    End Sub

    ' Constructor para Editar (modificado para recibir el estado)
    Public Sub New(id As Integer, Optional estadoActual As String = "")
        Me.New()
        _modo = ModoFormulario.Editar
        _idLicencia = id
        _estadoInicial = estadoActual ' Guardamos el estado recibido
        Me.Text = "Editar Licencia"
    End Sub

    ' Evento Load modificado
    Private Async Sub frmLicenciaCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        _svc = New LicenciaService()
        Await CargarCombosAsync() ' Este método ahora también se encarga del estado

        If _modo = ModoFormulario.Editar Then
            ' Para editar, cargamos todos los datos de la licencia
            Await CargarDatosAsync()
            ' Y nos aseguramos de que el estado que pasamos desde la grilla sea el visible
            cboEstado.Text = _estadoInicial
        End If
    End Sub

    ' Método CargarCombosAsync modificado
    Private Async Function CargarCombosAsync() As Task
        ' Carga de Funcionarios y Tipos de Licencia (sin cambios)
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        cboFuncionario.DataSource = Await _svc.ObtenerFuncionariosParaComboAsync()

        cboTipoLicencia.DisplayMember = "Value"
        cboTipoLicencia.ValueMember = "Key"
        cboTipoLicencia.DataSource = Await _svc.ObtenerTiposLicenciaParaComboAsync()

        ' *** INICIO DE LA NUEVA LÓGICA PARA EL COMBO DE ESTADOS ***
        Dim estadosExistentes = Await _svc.ObtenerEstadosDeLicenciaAsync()

        If estadosExistentes IsNot Nothing AndAlso estadosExistentes.Any() Then
            ' Si hay estados en la base de datos, los usamos
            cboEstado.DataSource = estadosExistentes
        Else
            ' Si no hay ningún registro, usamos una lista por defecto (fallback)
            cboEstado.Items.Clear()
            cboEstado.Items.AddRange(New Object() {"Autorizado", "Rechazada", "Anulada", "Pendiente de..."})
        End If

        ' Para el modo CREAR, establecemos "Autorizado" como valor predeterminado
        If _modo = ModoFormulario.Crear Then
            cboEstado.SelectedItem = "Autorizado"
        End If
        ' *** FIN DE LA NUEVA LÓGICA ***

        ' Limpiar selección inicial para los otros combos en modo Crear
        If _modo = ModoFormulario.Crear Then
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

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Asegurar que el funcionario de la licencia exista en el ComboBox, incluso si está inactivo.
        Dim funcionariosSource = CType(cboFuncionario.DataSource, List(Of KeyValuePair(Of Integer, String)))

        ' Verificar si el funcionario de la licencia NO está en la lista del combo.
        If Not funcionariosSource.Any(Function(kvp) kvp.Key = _licencia.FuncionarioId) Then
            ' Si no está, lo buscamos directamente en la base de datos.
            Dim funcionarioDeLicencia = Await _svc.UnitOfWork.Repository(Of Funcionario)().GetByIdAsync(_licencia.FuncionarioId)
            If funcionarioDeLicencia IsNot Nothing Then
                ' Lo añadimos a la lista, marcándolo como inactivo para claridad del usuario.
                funcionariosSource.Add(New KeyValuePair(Of Integer, String)(funcionarioDeLicencia.Id, funcionarioDeLicencia.Nombre & " (Inactivo)"))
                ' Re-asignamos la fuente de datos y la reordenamos para mantener el orden alfabético.
                cboFuncionario.DataSource = funcionariosSource.OrderBy(Function(kvp) kvp.Value).ToList()
            End If
        End If
        ' --- FIN DE LA CORRECCIÓN ---

        ' Ahora, las asignaciones de valores funcionarán correctamente para cualquier funcionario.
        cboFuncionario.SelectedValue = _licencia.FuncionarioId
        cboTipoLicencia.SelectedValue = _licencia.TipoLicenciaId
        dtpFechaInicio.Value = _licencia.inicio
        dtpFechaFin.Value = _licencia.finaliza
        cboEstado.Text = If(_licencia.estado IsNot Nothing, _licencia.estado.Trim(), String.Empty)
        txtComentario.Text = _licencia.Comentario
    End Function

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If cboFuncionario.SelectedIndex = -1 OrElse cboTipoLicencia.SelectedIndex = -1 OrElse cboEstado.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un funcionario, un tipo de licencia y un estado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If dtpFechaFin.Value < dtpFechaInicio.Value Then
            MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _licencia.FuncionarioId = CInt(cboFuncionario.SelectedValue)
        _licencia.TipoLicenciaId = CInt(cboTipoLicencia.SelectedValue)
        _licencia.inicio = dtpFechaInicio.Value.Date
        _licencia.finaliza = dtpFechaFin.Value.Date
        _licencia.estado = cboEstado.SelectedItem.ToString()
        _licencia.Comentario = txtComentario.Text.Trim()
        _licencia.usuario = "SISTEMA" ' O el usuario logueado actualmente

        Try
            If _modo = ModoFormulario.Crear Then
                _licencia.fecha_registro = DateTime.Now
                _licencia.fecha_actualizado = DateTime.Now
                Await _svc.CreateAsync(_licencia)
            Else
                _licencia.fecha_actualizado = DateTime.Now
                Await _svc.UpdateAsync(_licencia)
            End If

            ' --- INICIO DE LA MODIFICACIÓN ---
            ' Notificar a toda la aplicación que los datos han cambiado.
            NotificadorEventos.NotificarActualizacion()
            ' --- FIN DE LA MODIFICACIÓN ---

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la licencia: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class