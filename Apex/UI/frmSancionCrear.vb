' Apex/UI/frmSancionCrear.vb

Public Class frmSancionCrear

    Private _svc As New SancionService()
    Private _funcionarioSvc As New FuncionarioService()
    Private _sancion As EstadoTransitorio
    Private _detalle As SancionDetalle
    Private _modo As ModoFormulario

    ' Propiedad pública para pasar el ID en modo edición
    Public SancionId As Integer? = Nothing
    Private _funcionarioIdParaSancionar As Integer? = Nothing
    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    Public Sub New()
        InitializeComponent()
    End Sub
    Public Sub New(funcionarioId As Integer)
        Me.New() ' Llama al constructor por defecto para que se inicialicen los componentes
        _funcionarioIdParaSancionar = funcionarioId
    End Sub

    Private Async Sub frmSancionCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' --- AÑADIDO: Cargar el ComboBox de Tipo Sanción ---
        cmbTipoSancion.Items.Clear()
        cmbTipoSancion.Items.Add("Puntos de Demérito")
        cmbTipoSancion.Items.Add("Observaciones Escritas")
        ' ---------------------------------------------------

        ' Determina el modo basado en si se pasó un ID
        If SancionId.HasValue Then
            _modo = ModoFormulario.Editar
            Me.Text = "Editar Sanción"
        Else
            _modo = ModoFormulario.Crear
            Me.Text = "Nueva Sanción"
        End If

        Await CargarCombosAsync()

        ' --- AÑADE ESTE BLOQUE DE CÓDIGO ---
        ' Si se pasó un ID desde el buscador, lo seleccionamos
        If _funcionarioIdParaSancionar.HasValue Then
            cboFuncionario.SelectedValue = _funcionarioIdParaSancionar.Value
            cboFuncionario.Enabled = False ' Opcional: deshabilita para que no se pueda cambiar
        End If
        ' --- FIN DEL BLOQUE AÑADIDO ---

        If _modo = ModoFormulario.Editar Then
            Await CargarDatosAsync()
        Else
            ' Es un registro nuevo, inicializamos los objetos
            _sancion = New EstadoTransitorio()
            _detalle = New SancionDetalle()
            _sancion.SancionDetalle = _detalle
            chkFechaHasta.Checked = True ' Por defecto, sin fecha de fin
            cmbTipoSancion.SelectedIndex = 0 ' Por defecto, "Puntos de Demérito"
        End If
        Try
            AppTheme.SetCue(cboFuncionario, "Seleccione un funcionario...")
            AppTheme.SetCue(cmbTipoSancion, "Seleccione un tipo de sanción...")
            AppTheme.SetCue(txtResolucion, "Ingrese la resolución...")
            AppTheme.SetCue(txtObservaciones, "Ingrese observaciones...")

        Catch
            ' Ignorar si no existe SetCue
        End Try
    End Sub

    Private Async Function CargarCombosAsync() As Task
        cboFuncionario.DisplayMember = "Value"
        cboFuncionario.ValueMember = "Key"
        cboFuncionario.DataSource = Await _funcionarioSvc.ObtenerFuncionariosParaComboAsync()
        cboFuncionario.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboFuncionario.AutoCompleteSource = AutoCompleteSource.ListItems

        If _modo = ModoFormulario.Crear Then
            cboFuncionario.SelectedIndex = -1
        End If
    End Function

    Private Async Function CargarDatosAsync() As Task
        _sancion = Await _svc.GetByIdAsync(SancionId.Value)

        If _sancion Is Nothing OrElse _sancion.SancionDetalle Is Nothing Then
            MessageBox.Show("No se encontró la sanción.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If

        _detalle = _sancion.SancionDetalle

        Dim funcionariosSource = CType(cboFuncionario.DataSource, List(Of KeyValuePair(Of Integer, String)))
        If Not funcionariosSource.Any(Function(kvp) kvp.Key = _sancion.FuncionarioId) Then
            Dim func = Await _funcionarioSvc.GetByIdAsync(_sancion.FuncionarioId)
            If func IsNot Nothing Then
                funcionariosSource.Add(New KeyValuePair(Of Integer, String)(func.Id, func.Nombre & " (Inactivo)"))
                cboFuncionario.DataSource = funcionariosSource.OrderBy(Function(kvp) kvp.Value).ToList()
            End If
        End If

        cboFuncionario.SelectedValue = _sancion.FuncionarioId
        dtpFechaDesde.Value = _detalle.FechaDesde
        If _detalle.FechaHasta.HasValue Then
            dtpFechaHasta.Value = _detalle.FechaHasta.Value
            chkFechaHasta.Checked = False
        Else
            chkFechaHasta.Checked = True
        End If
        txtResolucion.Text = _detalle.Resolucion
        txtObservaciones.Text = _detalle.Observaciones

        ' --- AÑADIDO: Cargar el valor guardado del Tipo de Sanción ---
        If Not String.IsNullOrEmpty(_detalle.TipoSancion) Then
            cmbTipoSancion.SelectedItem = _detalle.TipoSancion
        Else
            cmbTipoSancion.SelectedIndex = 0 ' Valor por defecto si no hay nada guardado
        End If
        ' ----------------------------------------------------------
    End Function

    Private Sub chkFechaHasta_CheckedChanged(sender As Object, e As EventArgs) Handles chkFechaHasta.CheckedChanged
        dtpFechaHasta.Enabled = Not chkFechaHasta.Checked
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If cboFuncionario.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un funcionario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' --- AÑADIDO: Validación para el nuevo ComboBox ---
        If cmbTipoSancion.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un tipo de sanción.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        ' ------------------------------------------------

        If Not chkFechaHasta.Checked AndAlso dtpFechaHasta.Value < dtpFechaDesde.Value Then
            MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _sancion.FuncionarioId = CInt(cboFuncionario.SelectedValue)
        _sancion.TipoEstadoTransitorioId = 3 ' ID para Sanción

        _detalle.FechaDesde = dtpFechaDesde.Value.Date
        _detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
        _detalle.Resolucion = txtResolucion.Text.Trim()
        _detalle.Observaciones = txtObservaciones.Text.Trim()

        ' --- AÑADIDO: Guardar el valor del ComboBox ---
        _detalle.TipoSancion = cmbTipoSancion.SelectedItem.ToString()
        ' --------------------------------------------

        Try
            If _modo = ModoFormulario.Crear Then
                _sancion.CreatedAt = DateTime.Now
                Await _svc.CreateAsync(_sancion)
            Else
                _sancion.UpdatedAt = DateTime.Now
                Await _svc.UpdateAsync(_sancion)
            End If

            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la sanción: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class