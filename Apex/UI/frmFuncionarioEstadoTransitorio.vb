Public Class frmFuncionarioEstadoTransitorio
    Public Estado As EstadoTransitorio
    Private _svc As FuncionarioService ' Reutilizamos el servicio

    Public Sub New(estado As EstadoTransitorio)
        InitializeComponent()
        Me.Estado = estado
        _svc = New FuncionarioService()
    End Sub

    Private Async Sub frmFuncionarioEstadoTransitorio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarCombosAsync()
        If Estado IsNot Nothing AndAlso Estado.Id > 0 Then
            ' Modo Edición
            cboTipoEstado.SelectedValue = Estado.TipoEstadoTransitorioId
            dtpFechaDesde.Value = Estado.FechaDesde
            If Estado.FechaHasta.HasValue Then
                dtpFechaHasta.Value = Estado.FechaHasta.Value
                dtpFechaHasta.Enabled = True
                chkFechaHasta.Checked = False
            Else
                dtpFechaHasta.Enabled = False
                chkFechaHasta.Checked = True
            End If
            txtObservaciones.Text = Estado.Observaciones
        Else
            ' Modo Creación
            chkFechaHasta.Checked = True
        End If
    End Sub

    Private Async Function CargarCombosAsync() As Task
        cboTipoEstado.DataSource = Await _svc.ObtenerTiposEstadoTransitorioAsync()
        cboTipoEstado.DisplayMember = "Value"
        cboTipoEstado.ValueMember = "Key"
    End Function

    Private Sub chkFechaHasta_CheckedChanged(sender As Object, e As EventArgs) Handles chkFechaHasta.CheckedChanged
        dtpFechaHasta.Enabled = Not chkFechaHasta.Checked
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If cboTipoEstado.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar un tipo de estado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If Not chkFechaHasta.Checked AndAlso dtpFechaHasta.Value.Date < dtpFechaDesde.Value.Date Then
            MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Estado.TipoEstadoTransitorioId = CInt(cboTipoEstado.SelectedValue)
        Estado.FechaDesde = dtpFechaDesde.Value.Date
        Estado.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
        Estado.Observaciones = txtObservaciones.Text.Trim()

        If Estado.Id = 0 Then
            Estado.CreatedAt = DateTime.Now
        Else
            Estado.UpdatedAt = DateTime.Now
        End If

        DialogResult = DialogResult.OK
        Close()
    End Sub
End Class