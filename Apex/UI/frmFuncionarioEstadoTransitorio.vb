Imports System.IO
Imports System.Windows.Forms

Public Class frmFuncionarioEstadoTransitorio
    Public Event EstadoConfigurado(estado As EstadoTransitorio)

    Public Estado As EstadoTransitorio
    Private _tiposEstado As List(Of TipoEstadoTransitorio)
    Private _unitOfWork As IUnitOfWork
    Private _tempFiles As New List(Of String)
    Private _readOnly As Boolean = False
    Private _listaCargos As List(Of Cargo)

    ' Propiedades para todos los detalles (se mantienen para compatibilidad)
    Public DesignacionDetalle As DesignacionDetalle
    Public SancionDetalle As SancionDetalle
    Public SumarioDetalle As SumarioDetalle
    Public OrdenCincoDetalle As OrdenCincoDetalle
    Public EnfermedadDetalle As EnfermedadDetalle
    Public RetenDetalle As RetenDetalle
    Public TrasladoDetalle As TrasladoDetalle
    Public BajaDeFuncionarioDetalle As BajaDeFuncionarioDetalle
    Public CambioDeCargoDetalle As CambioDeCargoDetalle
    Public ReactivacionDeFuncionarioDetalle As ReactivacionDeFuncionarioDetalle
    Public SeparacionDelCargoDetalle As SeparacionDelCargoDetalle
    Public InicioDeProcesamientoDetalle As InicioDeProcesamientoDetalle
    Public DesarmadoDetalle As DesarmadoDetalle

    Public Sub New(estado As EstadoTransitorio, tiposEstado As List(Of TipoEstadoTransitorio), uow As IUnitOfWork)
        InitializeComponent()
        Me.Estado = estado
        _tiposEstado = tiposEstado
        _unitOfWork = uow
    End Sub

    Public Sub New(estado As EstadoTransitorio, uow As IUnitOfWork, readOnlyValue As Boolean)
        InitializeComponent()
        Me.Estado = estado
        _unitOfWork = uow
        _readOnly = readOnlyValue
        _tiposEstado = _unitOfWork.Repository(Of TipoEstadoTransitorio)().GetAll().ToList()
    End Sub

    Public Property ReadOnlyProperty As Boolean
        Get
            Return _readOnly
        End Get
        Set(value As Boolean)
            _readOnly = value
        End Set
    End Property

    Private Sub frmFuncionarioEstadoTransitorio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        CargarCombos()

        If Estado IsNot Nothing AndAlso Estado.Id > 0 AndAlso Estado.Funcionario Is Nothing Then
            Try
                _unitOfWork.Context.Entry(Estado).Reference(Function(x) x.Funcionario).Load()
            Catch
            End Try
        End If

        If Estado IsNot Nothing AndAlso Estado.Id > 0 Then
            cboTipoEstado.SelectedValue = Estado.TipoEstadoTransitorioId
            cboTipoEstado.Enabled = False
            CargarDatosDeDetalle()
            CargarAdjuntos(Estado.Id)
            pnlPreview.Enabled = dgvAdjuntos.Rows.Count > 0
            dgvAdjuntos.Enabled = dgvAdjuntos.Rows.Count > 0
        Else
            If Estado Is Nothing Then Estado = New EstadoTransitorio()
            chkFechaHasta.Checked = True
            chkSinFechaResolucion.Checked = True
            cboTipoEstado.SelectedIndex = -1
            pnlPreview.Enabled = False
            MostrarPanelMensaje("Haga clic en 'Adjuntar' para agregar un archivo.")
        End If

        If _readOnly Then SetReadOnlyMode()

        AddHandler cboTipoEstado.SelectedIndexChanged, AddressOf TipoEstado_Changed
        AddHandler dgvAdjuntos.SelectionChanged, AddressOf dgvAdjuntos_SelectionChanged
        TipoEstado_Changed(Nothing, EventArgs.Empty)
    End Sub

    Private Sub SetReadOnlyMode()
        For Each ctrl As Control In pnlDatos.Controls
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).ReadOnly = True
            ElseIf TypeOf ctrl Is ComboBox Then
                CType(ctrl, ComboBox).Enabled = False
            ElseIf TypeOf ctrl Is DateTimePicker Then
                CType(ctrl, DateTimePicker).Enabled = False
            ElseIf TypeOf ctrl Is CheckBox Then
                CType(ctrl, CheckBox).Enabled = False
            End If
        Next
        txtObservaciones.ReadOnly = True
        btnAdjuntar.Enabled = False
        btnEliminarAdjunto.Enabled = False
        btnVerAdjunto.Enabled = dgvAdjuntos.Rows.Count > 0
        btnGuardar.Text = "Cerrar"
        btnCancelar.Visible = False
        btnGuardar.Location = New Point((Me.ClientSize.Width - btnGuardar.Width) / 2, btnGuardar.Location.Y)
    End Sub

    Private Sub CargarDatosDeDetalle()
        Dim fechaHasta As Date? = Nothing
        Dim observaciones As String = ""
        Dim fechaResolucion As Date? = Nothing

        Select Case Estado.TipoEstadoTransitorioId
            Case TipoEstadoTransitorioId.Designacion
                Dim d = Estado.DesignacionDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.DocResolucion
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.Enfermedad
                Dim d = Estado.EnfermedadDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtDiagnostico.Text = d.Diagnostico
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.Sancion
                Dim d = Estado.SancionDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion
                fechaResolucion = d.FechaResolucion
                txtTipoSancion.Text = d.TipoSancion

            Case TipoEstadoTransitorioId.OrdenCinco
                Dim d = Estado.OrdenCincoDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.Reten
                Dim d = Estado.RetenDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaReten
                observaciones = d.Observaciones
                txtTurnoReten.Text = d.Turno
                txtAsignadoPor.Text = d.AsignadoPor

            Case TipoEstadoTransitorioId.Sumario
                Dim d = Estado.SumarioDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Expediente
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.Traslado
                Dim d = Estado.TrasladoDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.BajaDeFuncionario
                Dim d = Estado.BajaDeFuncionarioDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.CambioDeCargo
                Dim d = Estado.CambioDeCargoDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion
                cboCargoAnterior.SelectedValue = d.CargoAnteriorId
                cboCargoNuevo.SelectedValue = d.CargoNuevoId
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.ReactivacionDeFuncionario
                Dim d = Estado.ReactivacionDeFuncionarioDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.SeparacionDelCargo
                Dim d = Estado.SeparacionDelCargoDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.InicioDeProcesamiento
                Dim d = Estado.InicioDeProcesamientoDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Expediente
                fechaResolucion = d.FechaResolucion

            Case TipoEstadoTransitorioId.Desarmado
                Dim d = Estado.DesarmadoDetalle : If d Is Nothing Then GoTo Fin
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion
                fechaResolucion = d.FechaResolucion
        End Select

Fin:
        txtObservaciones.Text = observaciones

        If fechaHasta.HasValue Then
            chkFechaHasta.Checked = False
            dtpFechaHasta.Enabled = True
            dtpFechaHasta.Value = fechaHasta.Value
        Else
            chkFechaHasta.Checked = True
            dtpFechaHasta.Enabled = False
            dtpFechaHasta.Value = dtpFechaDesde.Value.Date
        End If

        If dtpFechaHasta.Enabled AndAlso dtpFechaHasta.Value.Date < dtpFechaDesde.Value.Date Then
            dtpFechaHasta.Value = dtpFechaDesde.Value.Date
        End If

        If fechaResolucion.HasValue Then
            dtpFechaResolucion.Value = fechaResolucion.Value
            dtpFechaResolucion.Enabled = True
            chkSinFechaResolucion.Checked = False
        Else
            dtpFechaResolucion.Enabled = False
            chkSinFechaResolucion.Checked = True
        End If
    End Sub

    Private Sub TipoEstado_Changed(sender As Object, e As EventArgs)
        If cboTipoEstado.SelectedIndex = -1 OrElse cboTipoEstado.SelectedValue Is Nothing Then
            UpdateFieldVisibility(0)
            Return
        End If

        Dim tipoId As Integer
        If Integer.TryParse(cboTipoEstado.SelectedValue.ToString(), tipoId) Then
            UpdateFieldVisibility(tipoId)
        End If
    End Sub

    Private Sub UpdateFieldVisibility(tipoId As Integer)
        ' Ocultar todos los campos específicos por defecto
        lblResolucion.Visible = False
        txtResolucion.Visible = False
        lblFechaResolucion.Visible = False
        dtpFechaResolucion.Visible = False
        chkSinFechaResolucion.Visible = False
        lblDiagnostico.Visible = False
        txtDiagnostico.Visible = False
        lblTurnoReten.Visible = False
        txtTurnoReten.Visible = False
        lblAsignadoPor.Visible = False
        txtAsignadoPor.Visible = False
        lblCargoAnterior.Visible = False
        cboCargoAnterior.Visible = False
        lblCargoNuevo.Visible = False
        cboCargoNuevo.Visible = False
        lblTipoSancion.Visible = False
        txtTipoSancion.Visible = False

        ' Siempre visible (excepto para Retén y Reactivación)
        ToggleHastaSection(RequiereFechaHasta(tipoId))

        ' Restaurar etiquetas
        lblFechaDesde.Text = "Fecha Desde:"
        lblResolucion.Text = "Resolución:"

        Select Case tipoId
            Case ModConstantesApex.TipoEstadoTransitorioId.Sancion
                ShowResolucion(True)
                lblTipoSancion.Visible = True
                txtTipoSancion.Visible = True

            Case ModConstantesApex.TipoEstadoTransitorioId.Designacion,
             ModConstantesApex.TipoEstadoTransitorioId.Sumario,
             ModConstantesApex.TipoEstadoTransitorioId.InicioDeProcesamiento
                ShowResolucion(True)
                If tipoId = ModConstantesApex.TipoEstadoTransitorioId.Designacion Then
                    lblResolucion.Text = "Doc. Resolución:"
                ElseIf tipoId = ModConstantesApex.TipoEstadoTransitorioId.Sumario OrElse
                   tipoId = ModConstantesApex.TipoEstadoTransitorioId.InicioDeProcesamiento Then
                    lblResolucion.Text = "Expediente:"
                End If

            Case ModConstantesApex.TipoEstadoTransitorioId.Enfermedad
                ShowDiagnostico(True)
                ShowResolucion(True)
                lblResolucion.Visible = False
                txtResolucion.Visible = False

            Case ModConstantesApex.TipoEstadoTransitorioId.CambioDeCargo
                ShowResolucion(True)
                ShowCargos(True)
                SetCargoAnteriorDesdeFuncionario()

            Case ModConstantesApex.TipoEstadoTransitorioId.Reten
                lblFechaDesde.Text = "Fecha Retén:"
                ShowTurnoReten(True)
                lblAsignadoPor.Visible = True
                txtAsignadoPor.Visible = True

            Case ModConstantesApex.TipoEstadoTransitorioId.ReactivacionDeFuncionario
                ShowResolucion(True)

            Case ModConstantesApex.TipoEstadoTransitorioId.OrdenCinco
                lblFechaResolucion.Visible = True
                dtpFechaResolucion.Visible = True
                chkSinFechaResolucion.Visible = True
                lblResolucion.Visible = False
                txtResolucion.Visible = False

            Case ModConstantesApex.TipoEstadoTransitorioId.Traslado,
             ModConstantesApex.TipoEstadoTransitorioId.BajaDeFuncionario,
             ModConstantesApex.TipoEstadoTransitorioId.SeparacionDelCargo,
             ModConstantesApex.TipoEstadoTransitorioId.Desarmado
                ShowResolucion(True)

            Case 0 ' Caso para cuando no hay nada seleccionado
                ToggleHastaSection(False)
        End Select
    End Sub

    Private Sub ShowResolucion(visible As Boolean)
        lblResolucion.Visible = visible
        txtResolucion.Visible = visible
        lblFechaResolucion.Visible = visible
        dtpFechaResolucion.Visible = visible
        chkSinFechaResolucion.Visible = visible
    End Sub

    Private Sub ShowDiagnostico(visible As Boolean)
        lblDiagnostico.Visible = visible
        txtDiagnostico.Visible = visible
    End Sub

    Private Sub ShowTurnoReten(visible As Boolean)
        lblTurnoReten.Visible = visible
        txtTurnoReten.Visible = visible
    End Sub

    Private Sub ShowCargos(visible As Boolean)
        lblCargoAnterior.Visible = visible
        cboCargoAnterior.Visible = visible
        lblCargoNuevo.Visible = visible
        cboCargoNuevo.Visible = visible
    End Sub

    Private Sub ToggleHastaSection(visible As Boolean)
        lblFechaHasta.Visible = visible
        dtpFechaHasta.Visible = visible
        chkFechaHasta.Visible = visible
    End Sub

    Private Sub CargarCombos()
        cboTipoEstado.DataSource = _tiposEstado
        cboTipoEstado.DisplayMember = "Nombre"
        cboTipoEstado.ValueMember = "Id"

        _listaCargos = _unitOfWork.Repository(Of Cargo)().GetAll().OrderBy(Function(c) c.Nombre).ToList()
        cboCargoAnterior.DataSource = New List(Of Cargo)(_listaCargos)
        cboCargoAnterior.DisplayMember = "Nombre"
        cboCargoAnterior.ValueMember = "Id"
        cboCargoNuevo.DataSource = New List(Of Cargo)(_listaCargos)
        cboCargoNuevo.DisplayMember = "Nombre"
        cboCargoNuevo.ValueMember = "Id"
        cboCargoAnterior.SelectedIndex = -1
        cboCargoNuevo.SelectedIndex = -1
    End Sub

    Private Sub chkFechaHasta_CheckedChanged(sender As Object, e As EventArgs) Handles chkFechaHasta.CheckedChanged
        dtpFechaHasta.Enabled = Not chkFechaHasta.Checked
        If dtpFechaHasta.Enabled AndAlso dtpFechaHasta.Value.Date < dtpFechaDesde.Value.Date Then
            dtpFechaHasta.Value = dtpFechaDesde.Value.Date
        End If
    End Sub
    Private Sub dtpFechaDesde_ValueChanged(sender As Object, e As EventArgs) Handles dtpFechaDesde.ValueChanged
        If Not chkFechaHasta.Checked AndAlso dtpFechaHasta.Value.Date < dtpFechaDesde.Value.Date Then
            dtpFechaHasta.Value = dtpFechaDesde.Value.Date
        End If
    End Sub

    Private Sub chkSinFechaResolucion_CheckedChanged(sender As Object, e As EventArgs) Handles chkSinFechaResolucion.CheckedChanged
        dtpFechaResolucion.Enabled = Not chkSinFechaResolucion.Checked
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If _readOnly Then
            Close()
            Return
        End If

        If cboTipoEstado.SelectedIndex = -1 OrElse cboTipoEstado.SelectedValue Is Nothing Then
            Notifier.Warn(Me, "Debe seleccionar un tipo de estado.")
            Return
        End If

        Dim tipoId As Integer = CInt(cboTipoEstado.SelectedValue)
        Dim fechaHastaSel As Date? = If(chkFechaHasta.Checked OrElse Not chkFechaHasta.Visible, CType(Nothing, Date?), dtpFechaHasta.Value.Date)

        If RequiereFechaHasta(tipoId) AndAlso fechaHastaSel.HasValue AndAlso fechaHastaSel.Value < dtpFechaDesde.Value.Date Then
            Notifier.Warn(Me, "La fecha de fin no puede ser anterior a la fecha de inicio.")
            Return
        End If

        If tipoId = ModConstantesApex.TipoEstadoTransitorioId.CambioDeCargo Then
            If cboCargoNuevo.SelectedIndex = -1 Then
                Notifier.Warn(Me, "Debe seleccionar el nuevo cargo.")
                Return
            End If

            If Estado.Funcionario Is Nothing AndAlso Estado.Id > 0 Then
                Try : _unitOfWork.Context.Entry(Estado).Reference(Function(x) x.Funcionario).Load() : Catch : End Try
            End If

            Dim cargoActualId As Integer? = Estado.Funcionario?.CargoId
            If Not cargoActualId.HasValue Then
                Notifier.Warn(Me, "El funcionario no tiene un cargo actual asignado.")
                Return
            End If

            Dim cargoNuevoId = CType(cboCargoNuevo.SelectedValue, Integer)
            If cargoNuevoId = cargoActualId.Value Then
                Notifier.Warn(Me, "El nuevo cargo no puede ser igual al cargo actual.")
                Return
            End If

            Dim cargoNuevo = _listaCargos.FirstOrDefault(Function(c) c.Id = cargoNuevoId)
            Dim cargoActual = _listaCargos.FirstOrDefault(Function(c) c.Id = cargoActualId.Value)
            Dim gradoNuevo = cargoNuevo?.Grado.GetValueOrDefault(0)
            Dim gradoActual = cargoActual?.Grado.GetValueOrDefault(0)

            Dim mensajeConfirmacion As String
            Dim tituloConfirmacion As String = "Confirmar Cambio de Cargo"
            Dim icono As MessageBoxIcon = MessageBoxIcon.Question

            If gradoNuevo > gradoActual Then
                mensajeConfirmacion = $"¿Está seguro de que desea aplicar el ASCENSO al funcionario al cargo '{cargoNuevo.Nombre}'?"
            ElseIf gradoNuevo < gradoActual Then
                mensajeConfirmacion = $"¡ATENCIÓN! El nuevo cargo '{cargoNuevo.Nombre}' representa un DESCENSO." & vbCrLf &
                                      "¿Desea continuar y aplicar el cambio de todas formas?"
                icono = MessageBoxIcon.Warning
            Else
                mensajeConfirmacion = $"El nuevo cargo '{cargoNuevo.Nombre}' tiene el mismo grado que el actual (movimiento lateral)." & vbCrLf &
                                      "¿Desea confirmar el cambio?"
            End If

            If MessageBox.Show(mensajeConfirmacion, tituloConfirmacion, MessageBoxButtons.YesNo, icono) = DialogResult.No Then
                Return
            End If
        End If

        Try
            MapearDatosDesdeControles()
            RaiseEvent EstadoConfigurado(Me.Estado)
            Close()

        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al configurar el estado: {ex.Message}")
        End Try
    End Sub

    Private Sub MapearDatosDesdeControles()
        Estado.TipoEstadoTransitorioId = CInt(cboTipoEstado.SelectedValue)

        Dim fechaHastaSel As Date? = If(chkFechaHasta.Checked OrElse Not chkFechaHasta.Visible, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
        Dim fechaResolucionSel As Date? = If(chkSinFechaResolucion.Checked OrElse Not chkSinFechaResolucion.Visible, CType(Nothing, Date?), dtpFechaResolucion.Value.Date)

        Select Case Estado.TipoEstadoTransitorioId
            Case ModConstantesApex.TipoEstadoTransitorioId.Designacion
                If Estado.DesignacionDetalle Is Nothing Then Estado.DesignacionDetalle = New DesignacionDetalle()
                Dim d = Estado.DesignacionDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.DocResolucion = txtResolucion.Text.Trim()
                d.FechaResolucion = fechaResolucionSel

            Case ModConstantesApex.TipoEstadoTransitorioId.Enfermedad
                If Estado.EnfermedadDetalle Is Nothing Then Estado.EnfermedadDetalle = New EnfermedadDetalle()
                Dim d = Estado.EnfermedadDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Diagnostico = txtDiagnostico.Text.Trim()
                d.FechaResolucion = fechaResolucionSel

            Case ModConstantesApex.TipoEstadoTransitorioId.Sancion
                If Estado.SancionDetalle Is Nothing Then Estado.SancionDetalle = New SancionDetalle()
                Dim d = Estado.SancionDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                d.FechaResolucion = fechaResolucionSel
                d.TipoSancion = txtTipoSancion.Text.Trim()

            Case ModConstantesApex.TipoEstadoTransitorioId.OrdenCinco
                If Estado.OrdenCincoDetalle Is Nothing Then Estado.OrdenCincoDetalle = New OrdenCincoDetalle()
                Dim d = Estado.OrdenCincoDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.FechaResolucion = fechaResolucionSel

            Case ModConstantesApex.TipoEstadoTransitorioId.Reten
                If Estado.RetenDetalle Is Nothing Then Estado.RetenDetalle = New RetenDetalle()
                Dim d = Estado.RetenDetalle
                d.FechaReten = dtpFechaDesde.Value.Date
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Turno = txtTurnoReten.Text.Trim()
                d.AsignadoPor = txtAsignadoPor.Text.Trim()

            Case ModConstantesApex.TipoEstadoTransitorioId.Sumario
                If Estado.SumarioDetalle Is Nothing Then Estado.SumarioDetalle = New SumarioDetalle()
                Dim d = Estado.SumarioDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Expediente = txtResolucion.Text.Trim()
                d.FechaResolucion = fechaResolucionSel

            Case ModConstantesApex.TipoEstadoTransitorioId.Traslado
                If Estado.TrasladoDetalle Is Nothing Then Estado.TrasladoDetalle = New TrasladoDetalle()
                Dim d = Estado.TrasladoDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                d.FechaResolucion = fechaResolucionSel

            Case ModConstantesApex.TipoEstadoTransitorioId.BajaDeFuncionario
                If Estado.BajaDeFuncionarioDetalle Is Nothing Then Estado.BajaDeFuncionarioDetalle = New BajaDeFuncionarioDetalle()
                Dim d = Estado.BajaDeFuncionarioDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                d.FechaResolucion = fechaResolucionSel

            Case ModConstantesApex.TipoEstadoTransitorioId.CambioDeCargo
                If Estado.CambioDeCargoDetalle Is Nothing Then Estado.CambioDeCargoDetalle = New CambioDeCargoDetalle()
                Dim d = Estado.CambioDeCargoDetalle
                Dim cargoNuevoId = CType(cboCargoNuevo.SelectedValue, Integer)
                Dim cargoActualId = Estado.Funcionario.CargoId.Value
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                d.CargoAnteriorId = cargoActualId
                d.CargoNuevoId = cargoNuevoId
                d.FechaResolucion = fechaResolucionSel
                Estado.Funcionario.CargoId = cargoNuevoId

                ' ▼▼▼ LÍNEA A AGREGAR ▼▼▼
                ' Notificamos al UnitOfWork que la entidad Funcionario ha cambiado
                ' para que guarde la modificación del CargoId.
                _unitOfWork.Repository(Of Funcionario).Update(Estado.Funcionario)

            Case ModConstantesApex.TipoEstadoTransitorioId.ReactivacionDeFuncionario
                If Estado.ReactivacionDeFuncionarioDetalle Is Nothing Then Estado.ReactivacionDeFuncionarioDetalle = New ReactivacionDeFuncionarioDetalle()
                Dim d = Estado.ReactivacionDeFuncionarioDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                d.FechaResolucion = fechaResolucionSel

            Case ModConstantesApex.TipoEstadoTransitorioId.SeparacionDelCargo
                If Estado.SeparacionDelCargoDetalle Is Nothing Then Estado.SeparacionDelCargoDetalle = New SeparacionDelCargoDetalle()
                Dim d = Estado.SeparacionDelCargoDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                d.FechaResolucion = fechaResolucionSel

            Case ModConstantesApex.TipoEstadoTransitorioId.InicioDeProcesamiento
                If Estado.InicioDeProcesamientoDetalle Is Nothing Then Estado.InicioDeProcesamientoDetalle = New InicioDeProcesamientoDetalle()
                Dim d = Estado.InicioDeProcesamientoDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Expediente = txtResolucion.Text.Trim()
                d.FechaResolucion = fechaResolucionSel

            Case ModConstantesApex.TipoEstadoTransitorioId.Desarmado
                If Estado.DesarmadoDetalle Is Nothing Then Estado.DesarmadoDetalle = New DesarmadoDetalle()
                Dim d = Estado.DesarmadoDetalle
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                d.FechaResolucion = fechaResolucionSel
        End Select

        If Estado.Id = 0 Then
            Estado.CreatedAt = DateTime.Now
        Else
            Estado.UpdatedAt = DateTime.Now
        End If
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

#Region "Lógica de Archivos Adjuntos"
    Private Class AdjuntoRow
        Public Property Id As Integer
        Public Property NombreArchivo As String
        Public Property FechaCreacion As Date
        Public Property Origen As String
    End Class

    Private Sub ConfigurarGrillaAdjuntos()
        With dgvAdjuntos
            .AutoGenerateColumns = False
            .Columns.Clear()
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = NameOf(AdjuntoRow.Id), .Name = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = NameOf(AdjuntoRow.NombreArchivo), .HeaderText = "Nombre del Archivo", .Name = "NombreArchivo", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            Dim colFecha As New DataGridViewTextBoxColumn With {.DataPropertyName = NameOf(AdjuntoRow.FechaCreacion), .HeaderText = "Fecha de Carga", .Name = "FechaCreacion", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells}
            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            .Columns.Add(colFecha)
        End With
    End Sub

    Private Sub CargarAdjuntos(estadoId As Integer)
        If dgvAdjuntos.Columns.Count = 0 Then ConfigurarGrillaAdjuntos()
        Dim idSeleccionado As Integer? = Nothing
        If dgvAdjuntos.CurrentRow IsNot Nothing AndAlso dgvAdjuntos.CurrentRow.DataBoundItem IsNot Nothing Then
            Dim actual = TryCast(dgvAdjuntos.CurrentRow.DataBoundItem, AdjuntoRow)
            If actual IsNot Nothing Then idSeleccionado = actual.Id
        End If

        Dim adjuntos As New List(Of AdjuntoRow)
        If estadoId > 0 Then
            Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
            Dim adjuntosDB = repo.GetAll().Where(Function(a) a.EstadoTransitorioId = estadoId).Select(Function(a) New AdjuntoRow With {.Id = a.Id, .NombreArchivo = a.NombreArchivo, .FechaCreacion = a.FechaCreacion, .Origen = "BD"}).ToList()
            adjuntos.AddRange(adjuntosDB)
        End If
        If Estado?.AdjuntosNuevos?.Any() Then
            Dim adjuntosMemoria = Estado.AdjuntosNuevos.Select(Function(a) New AdjuntoRow With {.Id = a.Id, .NombreArchivo = a.NombreArchivo, .FechaCreacion = a.FechaCreacion, .Origen = "Memoria"}).ToList()
            adjuntos.AddRange(adjuntosMemoria)
        End If

        Dim listaOrdenada = adjuntos.OrderByDescending(Function(x) x.FechaCreacion).ThenBy(Function(x) x.NombreArchivo).ToList()
        Dim bs = If(TryCast(dgvAdjuntos.DataSource, BindingSource), New BindingSource With {.DataSource = listaOrdenada})
        If dgvAdjuntos.DataSource Is Nothing Then dgvAdjuntos.DataSource = bs Else bs.DataSource = listaOrdenada

        Dim hay As Boolean = listaOrdenada.Any()
        dgvAdjuntos.Enabled = hay
        pnlPreview.Enabled = hay
        If Not hay Then MostrarPanelMensaje("No hay archivos adjuntos.")

        If hay AndAlso idSeleccionado.HasValue Then
            For Each row As DataGridViewRow In dgvAdjuntos.Rows
                Dim item = TryCast(row.DataBoundItem, AdjuntoRow)
                If item IsNot Nothing AndAlso item.Id = idSeleccionado.Value Then
                    row.Selected = True
                    dgvAdjuntos.CurrentCell = row.Cells("NombreArchivo")
                    Exit For
                End If
            Next
        ElseIf hay Then
            dgvAdjuntos.Rows(0).Selected = True
        End If
    End Sub

    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using openDialog As New OpenFileDialog()
            openDialog.Title = "Seleccione un archivo (PDF o Imagen)"
            openDialog.Filter = "Archivos Soportados (*.pdf;*.jpg;*.jpeg;*.png)|*.pdf;*.jpg;*.jpeg;*.png|Todos los archivos (*.*)|*.*"

            If openDialog.ShowDialog() = DialogResult.OK Then
                Dim oldCursor = Me.Cursor
                btnAdjuntar.Enabled = False
                Me.Cursor = Cursors.WaitCursor

                Try
                    Dim ruta = openDialog.FileName
                    Dim contenidoBytes = File.ReadAllBytes(ruta)

                    Dim nuevoAdjunto = New EstadoTransitorioAdjunto With {
                    .NombreArchivo = Path.GetFileName(ruta),
                    .TipoMIME = GetMimeType(ruta),
                    .Contenido = contenidoBytes,
                    .FechaCreacion = DateTime.Now
                }

                    If Estado.Id = 0 Then
                        nuevoAdjunto.Id = -(Estado.AdjuntosNuevos.Count + 1)
                        Estado.AdjuntosNuevos.Add(nuevoAdjunto)
                        CargarAdjuntos(0)

                        Notifier.Success(Me, $"Adjunto agregado (pendiente): {nuevoAdjunto.NombreArchivo} · {FormatearBytes(contenidoBytes.Length)}")
                    Else
                        nuevoAdjunto.EstadoTransitorioId = Estado.Id
                        Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
                        repo.Add(nuevoAdjunto)
                        _unitOfWork.Commit()

                        CargarAdjuntos(Estado.Id)

                        Notifier.Success(Me, $"Adjunto guardado: {nuevoAdjunto.NombreArchivo} · {FormatearBytes(contenidoBytes.Length)}")
                    End If

                Catch ioEx As IOException
                    Notifier.[Error](Me, $"No se pudo leer el archivo (¿en uso o sin permisos?): {ioEx.Message}")
                Catch ex As Exception
                    Notifier.[Error](Me, $"Error al adjuntar el archivo: {ex.Message}")
                Finally
                    Me.Cursor = oldCursor
                    btnAdjuntar.Enabled = True
                End Try
            End If
        End Using
    End Sub

    Private Function FormatearBytes(bytes As Long) As String
        Dim tamanos = {"B", "KB", "MB", "GB", "TB"}
        Dim i As Integer = 0
        Dim val As Double = bytes
        While val >= 1024 AndAlso i < tamanos.Length - 1
            val /= 1024
            i += 1
        End While
        Return $"{val:0.##} {tamanos(i)}"
    End Function

    Private Sub btnVerAdjunto_Click(sender As Object, e As EventArgs) Handles btnVerAdjunto.Click
        If dgvAdjuntos.CurrentRow Is Nothing Then Return
        Try
            Dim adjuntoId = CInt(dgvAdjuntos.CurrentRow.Cells("Id").Value)
            Dim adjunto = If(adjuntoId < 0,
                Estado.AdjuntosNuevos.FirstOrDefault(Function(a) a.Id = adjuntoId),
                _unitOfWork.Repository(Of EstadoTransitorioAdjunto)().GetById(adjuntoId))
            If adjunto IsNot Nothing Then
                Dim tempPath = Path.Combine(Path.GetTempPath(), adjunto.NombreArchivo)
                File.WriteAllBytes(tempPath, adjunto.Contenido)
                Process.Start(tempPath)
            End If
        Catch ex As Exception
            MessageBox.Show($"No se pudo abrir el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnEliminarAdjunto_Click(sender As Object, e As EventArgs) Handles btnEliminarAdjunto.Click
        If dgvAdjuntos.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "No hay ningún adjunto seleccionado.")
            Return
        End If

        Dim idCell = dgvAdjuntos.CurrentRow.Cells("Id")
        If idCell Is Nothing OrElse idCell.Value Is Nothing Then
            Notifier.Warn(Me, "No se pudo determinar el adjunto seleccionado.")
            Return
        End If

        Dim adjuntoId As Integer
        If Not Integer.TryParse(idCell.Value.ToString(), adjuntoId) Then
            Notifier.Warn(Me, "El identificador del adjunto no es válido.")
            Return
        End If

        If MessageBox.Show("¿Está seguro de que desea eliminar este archivo adjunto?",
                          "Confirmar Eliminación",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
            Return
        End If

        Dim oldCursor = Me.Cursor
        btnEliminarAdjunto.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            If adjuntoId < 0 Then
                Dim adjuntoAEliminar = Estado.AdjuntosNuevos.FirstOrDefault(Function(a) a.Id = adjuntoId)
                If adjuntoAEliminar IsNot Nothing Then
                    Estado.AdjuntosNuevos.Remove(adjuntoAEliminar)
                Else
                    Notifier.Warn(Me, "No se encontró el adjunto en la lista pendiente.")
                    Return
                End If
                CargarAdjuntos(0)
            Else
                Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
                repo.RemoveById(adjuntoId)
                _unitOfWork.Commit()

                CargarAdjuntos(Estado.Id)
            End If

            Notifier.Success(Me, "Adjunto eliminado correctamente.")
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo eliminar el archivo: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnEliminarAdjunto.Enabled = True
        End Try
    End Sub

    Private Sub dgvAdjuntos_SelectionChanged(sender As Object, e As EventArgs)
        If dgvAdjuntos.CurrentRow Is Nothing Then
            MostrarPanelMensaje("Seleccione un archivo para previsualizar")
            Return
        End If
        Try
            Dim adjuntoId = CInt(dgvAdjuntos.CurrentRow.Cells("Id").Value)
            Dim adjunto = If(adjuntoId < 0,
                Estado.AdjuntosNuevos.FirstOrDefault(Function(a) a.Id = adjuntoId),
                _unitOfWork.Repository(Of EstadoTransitorioAdjunto)().GetById(adjuntoId))
            If adjunto Is Nothing Then Return
            Select Case adjunto.TipoMIME.ToLower()
                Case "image/jpeg", "image/png" : MostrarImagenPreview(adjunto.Contenido)
                Case "application/pdf" : MostrarPdfPreview(adjunto.Contenido)
                Case Else : MostrarPanelMensaje("Vista previa no disponible.")
            End Select
        Catch ex As Exception
            MostrarPanelMensaje($"Error al cargar la vista previa: {ex.Message}")
        End Try
    End Sub

    Private Sub MostrarImagenPreview(contenido As Byte())
        pbPreview.Visible = True
        wbPreview.Visible = False
        lblPreviewNotAvailable.Visible = False
        Using ms As New MemoryStream(contenido)
            pbPreview.Image = Image.FromStream(ms)
        End Using
    End Sub

    Private Sub MostrarPdfPreview(contenido As Byte())
        pbPreview.Visible = False
        wbPreview.Visible = True
        lblPreviewNotAvailable.Visible = False
        Try
            Dim tempPdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf")
            File.WriteAllBytes(tempPdfPath, contenido)
            _tempFiles.Add(tempPdfPath)
            wbPreview.Navigate(tempPdfPath)
        Catch ex As Exception
            MessageBox.Show($"No se pudo mostrar el PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub MostrarPanelMensaje(mensaje As String)
        pbPreview.Visible = False
        wbPreview.Visible = False
        lblPreviewNotAvailable.Visible = True
        lblPreviewNotAvailable.Text = mensaje
    End Sub

    Private Function GetMimeType(fileName As String) As String
        Dim extension = Path.GetExtension(fileName).ToLowerInvariant()
        Select Case extension
            Case ".pdf" : Return "application/pdf"
            Case ".jpg", ".jpeg" : Return "image/jpeg"
            Case ".png" : Return "image/png"
            Case Else : Return "application/octet-stream"
        End Select
    End Function

    Private Sub frmFuncionarioEstadoTransitorio_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        pbPreview.Image?.Dispose()
        pbPreview.Image = Nothing
        Try
            If wbPreview IsNot Nothing AndAlso Not wbPreview.IsDisposed Then
                wbPreview.Navigate("about:blank")
                Application.DoEvents()
                wbPreview.Stop()
                wbPreview.Dispose()
            End If
        Catch ex As Exception
            Console.WriteLine($"Error al disponer el WebBrowser: {ex.Message}")
        End Try
        GC.Collect()
        GC.WaitForPendingFinalizers()
        For Each filePath In _tempFiles
            Try
                If File.Exists(filePath) Then File.Delete(filePath)
            Catch ex As Exception
                Console.WriteLine($"No se pudo eliminar el archivo temporal: {filePath}. Error: {ex.Message}")
            End Try
        Next
    End Sub
#End Region

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then btnCancelar.PerformClick()
    End Sub

    Private Sub SetCargoAnteriorDesdeFuncionario()
        If Estado IsNot Nothing AndAlso Estado.Id > 0 AndAlso Estado.Funcionario Is Nothing Then
            Try : _unitOfWork.Context.Entry(Estado).Reference(Function(e) e.Funcionario).Load()
            Catch : End Try
        End If

        Dim cargoAnteriorId As Integer? = Nothing

        If Estado?.CambioDeCargoDetalle IsNot Nothing AndAlso Estado.CambioDeCargoDetalle.CargoAnteriorId > 0 Then
            cargoAnteriorId = Estado.CambioDeCargoDetalle.CargoAnteriorId
        ElseIf Estado IsNot Nothing AndAlso Estado.Funcionario IsNot Nothing Then
            cargoAnteriorId = Estado.Funcionario.CargoId
        End If

        cboCargoAnterior.SelectedIndex = -1

        If cargoAnteriorId.HasValue AndAlso _listaCargos IsNot Nothing AndAlso
           _listaCargos.Any(Function(c) c.Id = cargoAnteriorId.Value) Then
            cboCargoAnterior.SelectedValue = cargoAnteriorId.Value
        End If

        cboCargoAnterior.Enabled = False
    End Sub
    Private Function RequiereFechaHasta(tipoId As Integer) As Boolean
        Select Case tipoId
            Case ModConstantesApex.TipoEstadoTransitorioId.Reten,
                 ModConstantesApex.TipoEstadoTransitorioId.ReactivacionDeFuncionario
                Return False
            Case Else
                Return True
        End Select
    End Function

End Class