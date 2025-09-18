Option Strict On
Option Explicit On

Public Class frmNotificacionCrear

    Private _svc As NotificacionService
    Private _notificacion As NotificacionPersonal
    Private _modo As ModoFormulario
    Private _idNotificacion As Integer
    Private _idFuncionarioPreseleccionado As Integer? = Nothing
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
    ' --- INICIO DEL CAMBIO ---
    ' Nuevo constructor para crear con funcionario preseleccionado.
    ' Se añade un segundo argumento Booleano para diferenciar la firma.
    ''' <summary>
    ''' Crea una nueva notificación y preselecciona un funcionario.
    ''' </summary>
    ''' <param name="funcionarioId">ID del funcionario a seleccionar.</param>
    ''' <param name="esPreseleccion">Argumento para diferenciar la firma del constructor de edición.</param>
    Public Sub New(funcionarioId As Integer, esPreseleccion As Boolean)
        Me.New() ' Llama al constructor base
        _modo = ModoFormulario.Crear
        _idFuncionarioPreseleccionado = funcionarioId
    End Sub
    ' --- FIN DEL CAMBIO ---

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
        ElseIf _idFuncionarioPreseleccionado.HasValue Then
            cboFuncionario.SelectedValue = _idFuncionarioPreseleccionado.Value
        Else
            cboFuncionario.SelectedIndex = -1
            cboTipoNotificacion.SelectedIndex = -1
        End If

        Try
            AppTheme.SetCue(cboFuncionario, "Buscar por funcionario…")
            AppTheme.SetCue(cboTipoNotificacion, "Seleccionar tipo…")
            AppTheme.SetCue(txtDocumento, "Nro. documento…")
            AppTheme.SetCue(txtOficina, "Oficina…")
            AppTheme.SetCue(txtMedio, "Texto de notificación…")
        Catch
        End Try
        If _modo = ModoFormulario.Editar Then
            BloquearFuncionarioEnEdicion()
        End If
    End Sub
    ' Agregá este helper en el form:
    Private Sub BloquearFuncionarioEnEdicion()
        ' Evita la excepción: en DropDownList, AutoCompleteMode != None solo si Source=ListItems.
        cboFuncionario.AutoCompleteSource = AutoCompleteSource.ListItems
        cboFuncionario.AutoCompleteMode = AutoCompleteMode.None  ' seguro en edición
        cboFuncionario.DropDownStyle = ComboBoxStyle.DropDownList

        cboFuncionario.Enabled = False
        cboFuncionario.TabStop = False

        ' (Opcional) tooltip explicativo si tenés un ToolTip en el form:
        ' ToolTip1.SetToolTip(cboFuncionario, "En edición no se permite cambiar el funcionario.")
    End Sub


    Private Async Function CargarCombosAsync() As Task
        ' Funcionarios
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        Dim src = Await _svc.ObtenerFuncionariosParaComboAsync() ' IList(Of KeyValuePair(Of Integer,String)) o List(...)
        cboFuncionario.DataSource = src

        ' === AUTOCOMPLETE SEGURO PARA DropDownList ===
        With cboFuncionario
            .AutoCompleteMode = AutoCompleteMode.None
            .DropDownStyle = ComboBoxStyle.DropDownList
            .AutoCompleteSource = AutoCompleteSource.ListItems
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .SelectedIndex = -1  ' Sólo como estado inicial
        End With

        ' Tipos de notificación
        cboTipoNotificacion.DisplayMember = "Value"
        cboTipoNotificacion.ValueMember = "Key"
        cboTipoNotificacion.DataSource = Await _svc.ObtenerTiposNotificacionParaComboAsync()
        With cboTipoNotificacion
            .DropDownStyle = ComboBoxStyle.DropDownList
            .SelectedIndex = -1
        End With
    End Function


    Private Async Function CargarDatosAsync() As Task
        _notificacion = Await _svc.GetByIdParaEdicionAsync(_idNotificacion)

        If _notificacion Is Nothing Then
            Notifier.Error(Me, "No se encontró la notificación.")
            Close()
            Return
        End If

        ' === Asegurar que el funcionario esté en el combo (aunque esté inactivo) ===
        Dim origen = TryCast(cboFuncionario.DataSource, IList(Of KeyValuePair(Of Integer, String)))
        If origen Is Nothing Then
            ' fallback genérico por si algún refactor cambió el tipo
            Dim tmp = TryCast(cboFuncionario.DataSource, IEnumerable(Of KeyValuePair(Of Integer, String)))
            If tmp IsNot Nothing Then origen = tmp.ToList() ' hacemos una lista editable
        End If

        If origen IsNot Nothing Then
            Dim funcId = _notificacion.FuncionarioId
            If Not origen.Any(Function(kvp) kvp.Key = funcId) Then
                Using tempUow As New UnitOfWork()
                    Dim f = Await tempUow.Repository(Of Funcionario)().GetByIdAsync(funcId)
                    If f IsNot Nothing Then
                        ' Si el origen no es List, convertir a List para poder Add + ordenar
                        Dim lista As List(Of KeyValuePair(Of Integer, String)) =
                        If(TypeOf origen Is List(Of KeyValuePair(Of Integer, String)),
                           DirectCast(origen, List(Of KeyValuePair(Of Integer, String))),
                           origen.ToList())

                        lista.Add(New KeyValuePair(Of Integer, String)(f.Id, f.Nombre & " (Inactivo)"))
                        lista = lista.OrderBy(Function(kvp) kvp.Value).ToList()

                        ' Reasignar el DataSource y volver a aplicar miembros por seguridad
                        cboFuncionario.DataSource = lista
                        cboFuncionario.DisplayMember = "Value"
                        cboFuncionario.ValueMember = "Key"
                    End If
                End Using
            End If

            ' === Forzar selección robusta ===
            cboFuncionario.SelectedValue = _notificacion.FuncionarioId
            If cboFuncionario.SelectedIndex = -1 Then
                ' fallback por índice
                Dim lista2 = TryCast(cboFuncionario.DataSource, IEnumerable(Of KeyValuePair(Of Integer, String)))
                If lista2 IsNot Nothing Then
                    Dim idx = lista2.ToList().FindIndex(Function(kvp) kvp.Key = _notificacion.FuncionarioId)
                    If idx >= 0 Then cboFuncionario.SelectedIndex = idx
                End If
            End If
        Else
            ' Edge case: no hay DataSource
            Notifier.Warn(Me, "No se pudo inicializar el combo de funcionarios. Reintentá abrir la edición.")
        End If

        ' === Resto de campos ===
        cboTipoNotificacion.SelectedValue = _notificacion.TipoNotificacionId
        dtpFechaProgramada.Value = _notificacion.FechaProgramada
        txtMedio.Text = _notificacion.Medio
        txtDocumento.Text = _notificacion.Documento
        txtExpMinisterial.Text = _notificacion.ExpMinisterial
        txtExpINR.Text = _notificacion.ExpINR
        txtOficina.Text = _notificacion.Oficina
    End Function


    Private Function ValidarCampos() As Boolean
        ' En creación sí exigimos funcionario
        If _modo = ModoFormulario.Crear AndAlso cboFuncionario.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un funcionario.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        If cboTipoNotificacion.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un tipo de notificación.", "Validación",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function



    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Not ValidarCampos() Then Return

        Dim oldCursor = Me.Cursor
        btnGuardar.Enabled = False
        btnCancelar.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Dim idNotificacionGuardada As Integer

            If _modo = ModoFormulario.Crear Then
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
                idNotificacionGuardada = creada.Id
                Me.NotificacionId = idNotificacionGuardada

                Notifier.Success(Me, "Notificación creada correctamente.")
            Else
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
                idNotificacionGuardada = _idNotificacion
                Me.NotificacionId = idNotificacionGuardada

                Notifier.Success(Me, "Notificación actualizada correctamente.")
            End If

            ' Confirmación modal de impresión (si querés, luego lo cambiamos a un ConfirmToast no modal)
            Dim imprimir = MessageBox.Show(
            "¿Desea imprimir el comprobante ahora?",
            "Imprimir Notificación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)

            If imprimir = DialogResult.Yes Then
                Dim frmReporte As New frmNotificacionRPT(idNotificacionGuardada)
                NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frmReporte)
                Notifier.Info(Me, "Abriendo reporte de notificación…")
            End If

            DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al guardar la notificación: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnGuardar.Enabled = True
            btnCancelar.Enabled = True
        End Try
    End Sub


    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Notifier.Info(Me, "Acción cancelada.")
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            btnCancelar.PerformClick()
            e.Handled = True

        End If
    End Sub


End Class
