Imports System.IO
Imports System.Linq

Public Class frmFuncionarioEstadoTransitorio

    Public Estado As EstadoTransitorio
    Private _tiposEstado As List(Of TipoEstadoTransitorio)
    Private _unitOfWork As IUnitOfWork

    ' Propiedades para los nuevos detalles
    Public DesignacionDetalle As DesignacionDetalle
    Public SancionDetalle As SancionDetalle
    Public SumarioDetalle As SumarioDetalle
    Public OrdenCincoDetalle As OrdenCincoDetalle
    Public EnfermedadDetalle As EnfermedadDetalle
    Public RetenDetalle As RetenDetalle

    Public Sub New(estado As EstadoTransitorio, tiposEstado As List(Of TipoEstadoTransitorio), uow As IUnitOfWork)
        InitializeComponent()
        Me.Estado = estado
        _tiposEstado = tiposEstado
        _unitOfWork = uow
    End Sub

    Private Sub frmFuncionarioEstadoTransitorio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        CargarCombos()

        If Estado IsNot Nothing AndAlso Estado.Id > 0 Then
            ' MODO EDICIÓN
            cboTipoEstado.SelectedValue = Estado.TipoEstadoTransitorioId
            cboTipoEstado.Enabled = False
            CargarDatosDeDetalle()
            CargarAdjuntos(Estado.Id)
            GroupBox1.Enabled = True ' Habilitar la sección de adjuntos
        Else
            ' MODO CREACIÓN
            Estado = New EstadoTransitorio()
            chkFechaHasta.Checked = True
            cboTipoEstado.SelectedIndex = -1
            GroupBox1.Enabled = False ' Deshabilitar adjuntos hasta que se guarde el estado
        End If

        AddHandler cboTipoEstado.SelectedIndexChanged, AddressOf TipoEstado_Changed
        TipoEstado_Changed(Nothing, EventArgs.Empty)
    End Sub

    Private Sub CargarDatosDeDetalle()
        Dim fechaHasta As Date? = Nothing
        Dim observaciones As String = ""

        ' Obtiene el detalle correcto y sus datos comunes
        Select Case Estado.TipoEstadoTransitorioId
            Case 1 ' Designación
                DesignacionDetalle = Estado.DesignacionDetalle
                dtpFechaDesde.Value = DesignacionDetalle.FechaDesde
                fechaHasta = DesignacionDetalle.FechaHasta
                observaciones = DesignacionDetalle.Observaciones
                txtResolucion.Text = DesignacionDetalle.DocResolucion
            Case 2 ' Enfermedad
                EnfermedadDetalle = Estado.EnfermedadDetalle
                dtpFechaDesde.Value = EnfermedadDetalle.FechaDesde
                fechaHasta = EnfermedadDetalle.FechaHasta
                observaciones = EnfermedadDetalle.Observaciones
                txtDiagnostico.Text = EnfermedadDetalle.Diagnostico
            Case 3 ' Sanción
                SancionDetalle = Estado.SancionDetalle
                dtpFechaDesde.Value = SancionDetalle.FechaDesde
                fechaHasta = SancionDetalle.FechaHasta
                observaciones = SancionDetalle.Observaciones
                txtResolucion.Text = SancionDetalle.Resolucion
            Case 4 ' Orden Cinco
                OrdenCincoDetalle = Estado.OrdenCincoDetalle
                dtpFechaDesde.Value = OrdenCincoDetalle.FechaDesde
                fechaHasta = OrdenCincoDetalle.FechaHasta
                observaciones = OrdenCincoDetalle.Observaciones
            Case 5 ' Retén
                RetenDetalle = Estado.RetenDetalle
                dtpFechaDesde.Value = RetenDetalle.FechaReten
                observaciones = RetenDetalle.Observaciones
                txtTurnoReten.Text = RetenDetalle.Turno
            Case 6 ' Sumario
                SumarioDetalle = Estado.SumarioDetalle
                dtpFechaDesde.Value = SumarioDetalle.FechaDesde
                fechaHasta = SumarioDetalle.FechaHasta
                observaciones = SumarioDetalle.Observaciones
                txtResolucion.Text = SumarioDetalle.Expediente
        End Select

        ' Asignar valores a los controles comunes
        txtObservaciones.Text = observaciones
        If Estado.TipoEstadoTransitorioId <> 5 Then ' Retén no tiene FechaHasta
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
        ' Ocultar todos los campos específicos
        lblResolucion.Visible = False : txtResolucion.Visible = False
        lblDiagnostico.Visible = False : txtDiagnostico.Visible = False
        lblTurnoReten.Visible = False : txtTurnoReten.Visible = False

        ' Restaurar visibilidad y texto por defecto de campos comunes
        lblFechaDesde.Text = "Fecha Desde:"
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

        Estado.TipoEstadoTransitorioId = tipoId

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
                detalle.Resolucion = txtResolucion.Text.Trim()
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
                detalle.Expediente = txtResolucion.Text.Trim()
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

#Region "Lógica de Archivos Adjuntos"

    Private Sub CargarAdjuntos(estadoId As Integer)
        Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
        Dim adjuntos = repo.GetAll().Where(Function(a) a.EstadoTransitorioId = estadoId) _
                                   .Select(Function(a) New With {a.Id, a.NombreArchivo, a.FechaCreacion}) _
                                   .ToList()
        dgvAdjuntos.DataSource = adjuntos

        ' Configurar columnas del DataGridView
        If dgvAdjuntos.Rows.Count > 0 Then
            dgvAdjuntos.Columns("Id").Visible = False
            dgvAdjuntos.Columns("NombreArchivo").HeaderText = "Nombre del Archivo"
            dgvAdjuntos.Columns("NombreArchivo").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dgvAdjuntos.Columns("FechaCreacion").HeaderText = "Fecha de Carga"
        End If
    End Sub

    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using openDialog As New OpenFileDialog()
            openDialog.Title = "Seleccione un archivo (PDF o Imagen)"
            openDialog.Filter = "Archivos PDF (*.pdf)|*.pdf|Imágenes (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"

            If openDialog.ShowDialog() = DialogResult.OK Then
                Try
                    Dim contenidoBytes = File.ReadAllBytes(openDialog.FileName)

                    Dim nuevoAdjunto As New EstadoTransitorioAdjunto With {
                        .EstadoTransitorioId = Estado.Id,
                        .NombreArchivo = Path.GetFileName(openDialog.FileName),
                        .TipoMIME = GetMimeType(openDialog.FileName),
                        .Contenido = contenidoBytes,
                        .FechaCreacion = DateTime.Now
                    }

                    Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
                    repo.Add(nuevoAdjunto)
                    _unitOfWork.Commit()

                    MessageBox.Show("Archivo adjuntado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    CargarAdjuntos(Estado.Id)

                Catch ex As Exception
                    MessageBox.Show($"Error al adjuntar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub btnVerAdjunto_Click(sender As Object, e As EventArgs) Handles btnVerAdjunto.Click
        If dgvAdjuntos.CurrentRow Is Nothing Then Return

        Try
            Dim adjuntoId = CInt(dgvAdjuntos.CurrentRow.Cells("Id").Value)
            Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
            Dim adjunto = repo.GetById(adjuntoId)

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
        If dgvAdjuntos.CurrentRow Is Nothing Then Return

        If MessageBox.Show("¿Está seguro de que desea eliminar este archivo adjunto?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim adjuntoId = CInt(dgvAdjuntos.CurrentRow.Cells("Id").Value)
                Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
                repo.RemoveById(adjuntoId)
                _unitOfWork.Commit()

                CargarAdjuntos(Estado.Id)
            Catch ex As Exception
                MessageBox.Show($"No se pudo eliminar el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    ' Función de ayuda para obtener el tipo MIME sin usar System.Web
    Private Function GetMimeType(fileName As String) As String
        Dim extension = Path.GetExtension(fileName).ToLowerInvariant()
        Select Case extension
            Case ".pdf"
                Return "application/pdf"
            Case ".jpg", ".jpeg"
                Return "image/jpeg"
            Case ".png"
                Return "image/png"
            Case Else
                Return "application/octet-stream" ' Tipo genérico para archivos desconocidos
        End Select
    End Function

#End Region

End Class