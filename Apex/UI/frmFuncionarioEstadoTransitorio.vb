' Apex/UI/frmFuncionarioEstadoTransitorio.vb
Public Class frmFuncionarioEstadoTransitorio

    Public Estado As EstadoTransitorio
    Private _tiposEstado As List(Of TipoEstadoTransitorio)

    ' --- Propiedades para los detalles ---
    Public DesignacionDetalle As DesignacionDetalle
    Public SancionDetalle As SancionDetalle
    Public SumarioDetalle As SumarioDetalle
    Public OrdenCincoDetalle As OrdenCincoDetalle
    Public EnfermedadDetalle As EnfermedadDetalle
    Public RetenDetalle As RetenDetalle

    Public Sub New(estado As EstadoTransitorio, tiposEstado As List(Of TipoEstadoTransitorio))
        InitializeComponent()
        Me.Estado = estado
        _tiposEstado = tiposEstado
    End Sub

    Private Sub frmFuncionarioEstadoTransitorio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        CargarCombos()

        If Estado IsNot Nothing AndAlso Estado.Id > 0 Then
            ' MODO EDICIÓN
            cboTipoEstado.SelectedValue = Estado.TipoEstadoTransitorioId
            cboTipoEstado.Enabled = False ' No se puede cambiar el tipo de un estado existente
            CargarDatosDeDetalle()
        Else
            ' MODO CREACIÓN
            Estado = New EstadoTransitorio()
            chkFechaHasta.Checked = True
            cboTipoEstado.SelectedIndex = -1
        End If

        AddHandler cboTipoEstado.SelectedIndexChanged, AddressOf TipoEstado_Changed
        TipoEstado_Changed(Nothing, EventArgs.Empty) ' Llamada inicial para configurar la UI
    End Sub

    Private Sub CargarDatosDeDetalle()
        Select Case Estado.TipoEstadoTransitorioId
            Case 1 ' Designación
                DesignacionDetalle = Estado.DesignacionDetalle
                dtpFechaDesde.Value = DesignacionDetalle.FechaDesde
                txtObservaciones.Text = DesignacionDetalle.Observaciones
                txtResolucion.Text = DesignacionDetalle.DocResolucion
            Case 2 ' Enfermedad
                EnfermedadDetalle = Estado.EnfermedadDetalle
                dtpFechaDesde.Value = EnfermedadDetalle.FechaDesde
                txtObservaciones.Text = EnfermedadDetalle.Observaciones
                txtDiagnostico.Text = EnfermedadDetalle.Diagnostico
            Case 3 ' Sanción
                SancionDetalle = Estado.SancionDetalle
                dtpFechaDesde.Value = SancionDetalle.FechaDesde
                txtObservaciones.Text = SancionDetalle.Observaciones
                txtResolucion.Text = SancionDetalle.DocResolucion
            Case 4 ' Orden Cinco
                OrdenCincoDetalle = Estado.OrdenCincoDetalle
                dtpFechaDesde.Value = OrdenCincoDetalle.FechaDesde
                txtObservaciones.Text = OrdenCincoDetalle.Observaciones
            Case 5 ' Retén
                RetenDetalle = Estado.RetenDetalle
                dtpFechaDesde.Value = RetenDetalle.FechaReten
                txtObservaciones.Text = RetenDetalle.Observaciones
                txtTurnoReten.Text = RetenDetalle.Turno
            Case 6 ' Sumario
                SumarioDetalle = Estado.SumarioDetalle
                dtpFechaDesde.Value = SumarioDetalle.FechaDesde
                txtObservaciones.Text = SumarioDetalle.Observaciones
                txtResolucion.Text = SumarioDetalle.DocResolucion ' Asumiendo que usa el mismo campo que sanción
        End Select

        ' Cargar FechaHasta común para la mayoría
        If Estado.TipoEstadoTransitorioId <> 5 Then ' Retén no tiene FechaHasta
            Dim fechaHasta As Date? = CType(Estado, Object).GetType().GetProperty("FechaHasta").GetValue(Estado, Nothing)
            If fechaHasta.HasValue Then
                dtpFechaHasta.Value = fechaHasta.Value
                dtpFechaHasta.Enabled = True
                chkFechaHasta.Checked = False
            Else
                dtpFechaHasta.Enabled = False
                chkFechaHasta.Checked = True
            End If
        End If
    End Sub


    Private Sub TipoEstado_Changed(sender As Object, e As EventArgs)
        ' Ocultar todos los campos específicos primero
        lblResolucion.Visible = False
        txtResolucion.Visible = False
        lblDiagnostico.Visible = False
        txtDiagnostico.Visible = False
        lblTurnoReten.Visible = False
        txtTurnoReten.Visible = False

        ' Ocultar campos comunes que no aplican a todos
        lblFechaHasta.Visible = True
        dtpFechaHasta.Visible = True
        chkFechaHasta.Visible = True


        If cboTipoEstado.SelectedIndex = -1 Then Return

        ' Mostrar campos según el tipo seleccionado
        Dim tipoId = CInt(cboTipoEstado.SelectedValue)
        Select Case tipoId
            Case 1, 3, 4, 6 ' Designación, Sanción, Orden Cinco, Sumario
                lblResolucion.Visible = True
                txtResolucion.Visible = True
            Case 2 ' Enfermedad
                lblDiagnostico.Visible = True
                txtDiagnostico.Visible = True
            Case 5 ' Retén
                lblTurnoReten.Visible = True
                txtTurnoReten.Visible = True
                ' Retén no tiene rango de fechas, solo una fecha
                lblFechaDesde.Text = "Fecha Retén:"
                lblFechaHasta.Visible = False
                dtpFechaHasta.Visible = False
                chkFechaHasta.Visible = False
        End Select
    End Sub

    Private Sub CargarCombos()
        cboTipoEstado.DataSource = _tiposEstado
        cboTipoEstado.DisplayMember = "Nombre"
        cboTipoEstado.ValueMember = "Id"
    End Sub

    Private Sub chkFechaHasta_CheckedChanged(sender As Object, e As EventArgs) Handles chkFechaHasta.CheckedChanged
        dtpFechaHasta.Enabled = Not chkFechaHasta.Checked
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If cboTipoEstado.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un tipo de estado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim tipoId = CInt(cboTipoEstado.SelectedValue)

        If tipoId <> 5 AndAlso Not chkFechaHasta.Checked AndAlso dtpFechaHasta.Value.Date < dtpFechaDesde.Value.Date Then
            MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Asignar el ID del tipo al objeto principal
        Estado.TipoEstadoTransitorioId = tipoId

        ' Rellenar y asociar el objeto de detalle correcto
        Select Case tipoId
            Case 1 ' Designación
                Dim detalle = If(Estado.Id > 0, Estado.DesignacionDetalle, New DesignacionDetalle())
                detalle.FechaDesde = dtpFechaDesde.Value.Date
                detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
                detalle.Observaciones = txtObservaciones.Text.Trim()
                detalle.DocResolucion = txtResolucion.Text.Trim()
                Estado.DesignacionDetalle = detalle
            Case 2 ' Enfermedad
                Dim detalle = If(Estado.Id > 0, Estado.EnfermedadDetalle, New EnfermedadDetalle())
                detalle.FechaDesde = dtpFechaDesde.Value.Date
                detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
                detalle.Observaciones = txtObservaciones.Text.Trim()
                detalle.Diagnostico = txtDiagnostico.Text.Trim()
                Estado.EnfermedadDetalle = detalle
            Case 3 ' Sanción
                Dim detalle = If(Estado.Id > 0, Estado.SancionDetalle, New SancionDetalle())
                detalle.FechaDesde = dtpFechaDesde.Value.Date
                detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
                detalle.Observaciones = txtObservaciones.Text.Trim()
                detalle.DocResolucion = txtResolucion.Text.Trim()
                Estado.SancionDetalle = detalle
            Case 4 ' Orden Cinco
                Dim detalle = If(Estado.Id > 0, Estado.OrdenCincoDetalle, New OrdenCincoDetalle())
                detalle.FechaDesde = dtpFechaDesde.Value.Date
                detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
                detalle.Observaciones = txtObservaciones.Text.Trim()
                Estado.OrdenCincoDetalle = detalle
            Case 5 ' Retén
                Dim detalle = If(Estado.Id > 0, Estado.RetenDetalle, New RetenDetalle())
                detalle.FechaReten = dtpFechaDesde.Value.Date
                detalle.Observaciones = txtObservaciones.Text.Trim()
                detalle.Turno = txtTurnoReten.Text.Trim()
                Estado.RetenDetalle = detalle
            Case 6 ' Sumario
                Dim detalle = If(Estado.Id > 0, Estado.SumarioDetalle, New SumarioDetalle())
                detalle.FechaDesde = dtpFechaDesde.Value.Date
                detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
                detalle.Observaciones = txtObservaciones.Text.Trim()
                detalle.DocResolucion = txtResolucion.Text.Trim() ' Asumiendo que es un expediente/resolución
                Estado.SumarioDetalle = detalle
        End Select

        If Estado.Id = 0 Then
            Estado.CreatedAt = DateTime.Now
        Else
            Estado.UpdatedAt = DateTime.Now
        End If

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class