Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class frmNotificacionCrear

    Private _svc As NotificacionService
    Private _notificacion As NotificacionPersonal
    Private _modo As ModoFormulario
    Private _idNotificacion As Integer
    Public Property NotificacionId As Integer? = Nothing

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    ' Constructor para crear
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        Me.Text = "Nueva Notificación"
    End Sub

    ' Constructor para editar
    Public Sub New(id As Integer)
        Me.New()
        _modo = ModoFormulario.Editar
        _idNotificacion = id
        Me.Text = "Editar Notificación"
    End Sub

    Private Async Sub frmNotificacionCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True
        AppTheme.Aplicar(Me)

        _svc = New NotificacionService()
        Await CargarCombosAsync()

        If _modo = ModoFormulario.Editar Then
            Await CargarDatosAsync()
        End If
    End Sub

    Private Async Function CargarCombosAsync() As Task
        ' Funcionarios
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        cboFuncionario.DataSource = Await _svc.ObtenerFuncionariosParaComboAsync()

        ' Autocompletado
        cboFuncionario.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboFuncionario.AutoCompleteSource = AutoCompleteSource.ListItems

        ' Tipos de notificación
        cboTipoNotificacion.DisplayMember = "Value"
        cboTipoNotificacion.ValueMember = "Key"
        cboTipoNotificacion.DataSource = Await _svc.ObtenerTiposNotificacionParaComboAsync()

        ' Limpiar selección inicial
        cboFuncionario.SelectedIndex = -1
        cboTipoNotificacion.SelectedIndex = -1
    End Function

    Private Async Function CargarDatosAsync() As Task
        _notificacion = Await _svc.GetByIdParaEdicionAsync(_idNotificacion)

        If _notificacion Is Nothing Then
            MessageBox.Show("No se encontró la notificación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If

        ' Asegurar que el funcionario esté en el combo aunque esté inactivo
        Dim funcionariosSource = CType(cboFuncionario.DataSource, List(Of KeyValuePair(Of Integer, String)))
        If Not funcionariosSource.Any(Function(kvp) kvp.Key = _notificacion.FuncionarioId) Then
            Using tempUow As New UnitOfWork()
                Dim funcionarioDeNotificacion = Await tempUow.Repository(Of Funcionario)().GetByIdAsync(_notificacion.FuncionarioId)
                If funcionarioDeNotificacion IsNot Nothing Then
                    funcionariosSource.Add(New KeyValuePair(Of Integer, String)(funcionarioDeNotificacion.Id, funcionarioDeNotificacion.Nombre & " (Inactivo)"))
                    cboFuncionario.DataSource = funcionariosSource.OrderBy(Function(kvp) kvp.Value).ToList()
                End If
            End Using
        End If

        ' Bind de campos
        cboFuncionario.SelectedValue = _notificacion.FuncionarioId
        cboTipoNotificacion.SelectedValue = _notificacion.TipoNotificacionId
        dtpFechaProgramada.Value = _notificacion.FechaProgramada
        txtMedio.Text = _notificacion.Medio
        txtDocumento.Text = _notificacion.Documento
        txtExpMinisterial.Text = _notificacion.ExpMinisterial
        txtExpINR.Text = _notificacion.ExpINR
        txtOficina.Text = _notificacion.Oficina
    End Function

    Private Function ValidarCampos() As Boolean
        If cboFuncionario.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un funcionario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        If cboTipoNotificacion.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un tipo de notificación.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Not ValidarCampos() Then Return

        btnGuardar.Enabled = False
        btnCancelar.Enabled = False
        Try
            If _modo = ModoFormulario.Crear Then
                ' Armar request y delegar en el Service (CreateNotificacionAsync)
                Dim req As New NotificacionCreateRequest With {
                    .FuncionarioId = CInt(cboFuncionario.SelectedValue),
                    .TipoNotificacionId = CByte(cboTipoNotificacion.SelectedValue),
                    .FechaProgramada = dtpFechaProgramada.Value,
                    .Medio = txtMedio.Text.Trim(),
                    .Documento = txtDocumento.Text.Trim(),
                    .ExpMinisterial = txtExpMinisterial.Text.Trim(),
                    .ExpINR = txtExpINR.Text.Trim(),
                    .Oficina = txtOficina.Text.Trim()
                }

                Dim creada = Await _svc.CreateNotificacionAsync(req)
                Me.NotificacionId = creada.Id

            Else
                ' Editar
                Dim req As New NotificacionUpdateRequest With {
                    .Id = _idNotificacion,
                    .FuncionarioId = CInt(cboFuncionario.SelectedValue),
                    .TipoNotificacionId = CByte(cboTipoNotificacion.SelectedValue),
                    .FechaProgramada = dtpFechaProgramada.Value,
                    .Medio = txtMedio.Text.Trim(),
                    .Documento = txtDocumento.Text.Trim(),
                    .ExpMinisterial = txtExpMinisterial.Text.Trim(),
                    .ExpINR = txtExpINR.Text.Trim(),
                    .Oficina = txtOficina.Text.Trim()
                }

                Await _svc.UpdateNotificacionAsync(req)
                Me.NotificacionId = _idNotificacion
            End If

            DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la notificación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnGuardar.Enabled = True
            btnCancelar.Enabled = True
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

End Class
