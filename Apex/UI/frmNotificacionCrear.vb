Imports System.Data.Entity

Public Class frmNotificacionCrear

    Private _svc As NotificacionPersonalService
    Private _notificacion As NotificacionPersonal
    Private _modo As ModoFormulario
    Private _idNotificacion As Integer

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    ' Constructor para crear una nueva notificación
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _notificacion = New NotificacionPersonal()
        Me.Text = "Nueva Notificación"
    End Sub

    ' Constructor para editar una notificación existente
    Public Sub New(id As Integer)
        Me.New()
        _modo = ModoFormulario.Editar
        _idNotificacion = id
        Me.Text = "Editar Notificación"
    End Sub

    Private Async Sub frmNotificacionCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New NotificacionPersonalService()
        Await CargarCombosAsync()

        If _modo = ModoFormulario.Editar Then
            Await CargarDatosAsync()
        End If
    End Sub

    Private Async Function CargarCombosAsync() As Task
        ' Cargar Funcionarios
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        cboFuncionario.DataSource = Await ObtenerFuncionariosAsync()

        ' Cargar Tipos de Notificación
        cboTipoNotificacion.DisplayMember = "Value"
        cboTipoNotificacion.ValueMember = "Key"
        cboTipoNotificacion.DataSource = Await ObtenerTiposNotificacionAsync()
    End Function

    Private Async Function CargarDatosAsync() As Task
        _notificacion = Await _svc.GetByIdAsync(_idNotificacion)

        If _notificacion Is Nothing Then
            MessageBox.Show("No se encontró la notificación.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If

        cboFuncionario.SelectedValue = _notificacion.FuncionarioId
        cboTipoNotificacion.SelectedValue = _notificacion.TipoNotificacionId
        dtpFechaProgramada.Value = _notificacion.FechaProgramada
        txtMedio.Text = _notificacion.Medio
        txtDocumento.Text = _notificacion.Documento
        txtExpMinisterial.Text = _notificacion.ExpMinisterial
        txtExpINR.Text = _notificacion.ExpINR
        txtOficina.Text = _notificacion.Oficina
    End Function

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If cboFuncionario.SelectedIndex = -1 OrElse cboTipoNotificacion.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un funcionario y un tipo de notificación.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Mapear datos del formulario al objeto
        _notificacion.FuncionarioId = CInt(cboFuncionario.SelectedValue)
        _notificacion.TipoNotificacionId = CByte(cboTipoNotificacion.SelectedValue)
        _notificacion.FechaProgramada = dtpFechaProgramada.Value
        _notificacion.Medio = txtMedio.Text.Trim()
        _notificacion.Documento = txtDocumento.Text.Trim()
        _notificacion.ExpMinisterial = txtExpMinisterial.Text.Trim()
        _notificacion.ExpINR = txtExpINR.Text.Trim()
        _notificacion.Oficina = txtOficina.Text.Trim()

        Try
            If _modo = ModoFormulario.Crear Then
                _notificacion.CreatedAt = DateTime.Now
                _notificacion.EstadoId = 1 ' Estado "Pendiente" por defecto
                Await _svc.CreateAsync(_notificacion)
            Else
                _notificacion.UpdatedAt = DateTime.Now
                Await _svc.UpdateAsync(_notificacion)
            End If

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la notificación: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    ' --- Funciones auxiliares para cargar ComboBoxes ---
    Private Async Function ObtenerFuncionariosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim repo = uow.Repository(Of Funcionario)()
            Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(f) f.Nombre).ToListAsync()
            Return lista.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
        End Using
    End Function

    Private Async Function ObtenerTiposNotificacionAsync() As Task(Of List(Of KeyValuePair(Of Byte, String)))
        Using uow As New UnitOfWork()
            Dim repo = uow.Repository(Of TipoNotificacion)()
            Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(t) t.Orden).ToListAsync()
            Return lista.Select(Function(t) New KeyValuePair(Of Byte, String)(t.Id, t.Nombre)).ToList()
        End Using
    End Function

End Class