' Apex/UI/frmFuncionarioEstadoTransitorio.vb
Public Class frmFuncionarioEstadoTransitorio

    Public Estado As EstadoTransitorio
    Private _tiposEstado As List(Of TipoEstadoTransitorio)

    ' Constructor modificado para recibir la lista de tipos
    Public Sub New(estado As EstadoTransitorio, tiposEstado As List(Of TipoEstadoTransitorio))
        InitializeComponent()
        Me.Estado = estado
        _tiposEstado = tiposEstado ' Recibir la lista
    End Sub

    Private Sub frmFuncionarioEstadoTransitorio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        CargarCombos() ' Ya no necesita ser asíncrono

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
            cboTipoEstado.SelectedIndex = -1
        End If
    End Sub

    Private Sub CargarCombos()
        ' Usar la lista pasada a través del constructor
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
        If Not chkFechaHasta.Checked AndAlso dtpFechaHasta.Value.Date < dtpFechaDesde.Value.Date Then
            MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Asignamos únicamente el ID del tipo de estado.
        ' Esto evita que Entity Framework intente insertar un nuevo TipoEstadoTransitorio duplicado.
        Estado.TipoEstadoTransitorioId = CInt(cboTipoEstado.SelectedValue)

        ' La línea que asignaba el objeto completo ha sido eliminada, ya que era la causa del error.
        ' Estado.TipoEstadoTransitorio = CType(cboTipoEstado.SelectedItem, TipoEstadoTransitorio)
        ' --- FIN DE LA CORRECCIÓN ---

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

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class