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
        _svc = New LicenciaService()
        Await CargarCombosAsync()

        If _modo = ModoFormulario.Editar Then
            Await CargarDatosAsync()
            cboEstado.Text = _estadoInicial
        End If
    End Sub

    Private Async Function CargarCombosAsync() As Task
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        cboFuncionario.DataSource = Await _svc.ObtenerFuncionariosParaComboAsync()

        cboTipoLicencia.DisplayMember = "Value"
        cboTipoLicencia.ValueMember = "Key"
        cboTipoLicencia.DataSource = Await _svc.ObtenerTiposLicenciaParaComboAsync()

        Dim estadosExistentes = Await _svc.ObtenerEstadosDeLicenciaAsync()

        If estadosExistentes IsNot Nothing AndAlso estadosExistentes.Any() Then
            cboEstado.DataSource = estadosExistentes
        Else
            cboEstado.Items.Clear()
            cboEstado.Items.AddRange(New Object() {"Autorizado", "Rechazada", "Anulada", "Pendiente de..."})
        End If

        If _modo = ModoFormulario.Crear Then
            cboEstado.SelectedItem = "Autorizado"
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

        Dim funcionariosSource = CType(cboFuncionario.DataSource, List(Of KeyValuePair(Of Integer, String)))

        If Not funcionariosSource.Any(Function(kvp) kvp.Key = _licencia.FuncionarioId) Then
            Dim funcionarioDeLicencia = Await _svc.UnitOfWork.Repository(Of Funcionario)().GetByIdAsync(_licencia.FuncionarioId)
            If funcionarioDeLicencia IsNot Nothing Then
                funcionariosSource.Add(New KeyValuePair(Of Integer, String)(funcionarioDeLicencia.Id, funcionarioDeLicencia.Nombre & " (Inactivo)"))
                cboFuncionario.DataSource = funcionariosSource.OrderBy(Function(kvp) kvp.Value).ToList()
            End If
        End If

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

            ' --- CAMBIO CLAVE: Llamada a la notificación genérica correcta ---
            NotificadorEventos.NotificarActualizacionGeneral()

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la licencia: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class