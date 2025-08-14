Public Class frmLicenciaCrear

    Private _svc As LicenciaService
    Private _licencia As HistoricoLicencia
    Private _modo As ModoFormulario
    Private _idLicencia As Integer

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

    ' Constructor para Editar
    Public Sub New(id As Integer)
        Me.New()
        _modo = ModoFormulario.Editar
        _idLicencia = id
        Me.Text = "Editar Licencia"
    End Sub

    Private Async Sub frmLicenciaCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        _svc = New LicenciaService()
        Await CargarCombosAsync()

        If _modo = ModoFormulario.Editar Then
            Await CargarDatosAsync()
        Else
            ' Por defecto, estado "Aprobada" para nuevas licencias
            cboEstado.SelectedItem = "Aprobada"
        End If
    End Sub

    Private Async Function CargarCombosAsync() As Task
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        cboFuncionario.DataSource = Await _svc.ObtenerFuncionariosParaComboAsync()

        cboTipoLicencia.DisplayMember = "Value"
        cboTipoLicencia.ValueMember = "Key"
        cboTipoLicencia.DataSource = Await _svc.ObtenerTiposLicenciaParaComboAsync()

        cboFuncionario.SelectedIndex = -1
        cboTipoLicencia.SelectedIndex = -1
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
        cboEstado.SelectedItem = _licencia.estado
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