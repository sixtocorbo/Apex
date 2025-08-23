Imports System.IO
Imports System.Linq

Public Class frmFuncionarioEstadoTransitorio

    Public Estado As EstadoTransitorio
    Private _tiposEstado As List(Of TipoEstadoTransitorio)
    Private _unitOfWork As IUnitOfWork
    Private _tempFiles As New List(Of String) ' Lista para rastrear archivos PDF temporales
    Private _readOnly As Boolean = False

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


    ' Pega este código reemplazando el método Load completo en tu frmFuncionarioEstadoTransitorio.vb

    Private Sub frmFuncionarioEstadoTransitorio_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        CargarCombos()

        If Estado IsNot Nothing AndAlso Estado.Id > 0 Then
            ' MODO EDICIÓN O LECTURA
            cboTipoEstado.SelectedValue = Estado.TipoEstadoTransitorioId
            cboTipoEstado.Enabled = False
            CargarDatosDeDetalle()
            CargarAdjuntos(Estado.Id)
            GroupBox1.Enabled = True

            ' *** INICIO DE LA CORRECCIÓN ***
            ' Ahora que el GroupBox está habilitado, gestionamos la visibilidad
            ' del panel de vista previa.
            If dgvAdjuntos.Rows.Count = 0 Then
                ' Si no hay adjuntos, nos aseguramos de mostrar el panel de mensaje.
                ' Esto oculta el PictureBox y el WebBrowser, que son los que tapan los botones.
                MostrarPanelMensaje("Haga clic en 'Adjuntar' para agregar un archivo.")
            Else
                ' Si ya hay adjuntos, forzamos la selección para que muestre la vista previa del primero.
                dgvAdjuntos_SelectionChanged(Nothing, EventArgs.Empty)
            End If
            ' *** FIN DE LA CORRECCIÓN ***

        Else
            ' MODO CREACIÓN
            Estado = New EstadoTransitorio()
            chkFechaHasta.Checked = True
            cboTipoEstado.SelectedIndex = -1
            GroupBox1.Enabled = False
            MostrarPanelMensaje("Guarde el estado para poder adjuntar archivos.")
        End If

        If _readOnly Then
            SetReadOnlyMode()
        End If

        AddHandler cboTipoEstado.SelectedIndexChanged, AddressOf TipoEstado_Changed
        AddHandler dgvAdjuntos.SelectionChanged, AddressOf dgvAdjuntos_SelectionChanged
        TipoEstado_Changed(Nothing, EventArgs.Empty)
    End Sub

    Private Sub SetReadOnlyMode()
        cboTipoEstado.Enabled = False
        dtpFechaDesde.Enabled = False
        dtpFechaHasta.Enabled = False
        chkFechaHasta.Enabled = False
        txtObservaciones.ReadOnly = True
        txtResolucion.ReadOnly = True
        txtDiagnostico.ReadOnly = True
        txtTurnoReten.ReadOnly = True
        btnAdjuntar.Visible = False
        btnEliminarAdjunto.Visible = False
        btnGuardar.Text = "Cerrar"
        btnCancelar.Visible = False

        ' Centrar el botón Cerrar
        btnGuardar.Location = New Point((Me.ClientSize.Width - btnGuardar.Width) / 2, btnGuardar.Location.Y)
        dgvAdjuntos.ReadOnly = True
    End Sub


    Private Sub CargarDatosDeDetalle()
        Dim fechaHasta As Date? = Nothing
        Dim observaciones As String = ""

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

        txtObservaciones.Text = observaciones
        If Estado.TipoEstadoTransitorioId <> 5 Then
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
        lblResolucion.Visible = False : txtResolucion.Visible = False
        lblDiagnostico.Visible = False : txtDiagnostico.Visible = False
        lblTurnoReten.Visible = False : txtTurnoReten.Visible = False
        lblFechaDesde.Text = "Fecha Desde:"
        lblFechaHasta.Visible = True
        dtpFechaHasta.Visible = True
        chkFechaHasta.Visible = True

        If cboTipoEstado.SelectedIndex = -1 Then Return

        Dim tipoId = CInt(cboTipoEstado.SelectedValue)
        Select Case tipoId
            Case 1, 3, 4, 6
                lblResolucion.Visible = True
                txtResolucion.Visible = True
            Case 2
                lblDiagnostico.Visible = True
                txtDiagnostico.Visible = True
            Case 5
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
        If _readOnly Then
            Close()
            Return
        End If

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
            Case 1
                Dim detalle = If(Estado.Id > 0, Estado.DesignacionDetalle, New DesignacionDetalle())
                detalle.FechaDesde = dtpFechaDesde.Value.Date
                detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
                detalle.Observaciones = txtObservaciones.Text.Trim()
                detalle.DocResolucion = txtResolucion.Text.Trim()
                Estado.DesignacionDetalle = detalle
            Case 2
                Dim detalle = If(Estado.Id > 0, Estado.EnfermedadDetalle, New EnfermedadDetalle())
                detalle.FechaDesde = dtpFechaDesde.Value.Date
                detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
                detalle.Observaciones = txtObservaciones.Text.Trim()
                detalle.Diagnostico = txtDiagnostico.Text.Trim()
                Estado.EnfermedadDetalle = detalle
            Case 3
                Dim detalle = If(Estado.Id > 0, Estado.SancionDetalle, New SancionDetalle())
                detalle.FechaDesde = dtpFechaDesde.Value.Date
                detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
                detalle.Observaciones = txtObservaciones.Text.Trim()
                detalle.Resolucion = txtResolucion.Text.Trim()
                Estado.SancionDetalle = detalle
            Case 4
                Dim detalle = If(Estado.Id > 0, Estado.OrdenCincoDetalle, New OrdenCincoDetalle())
                detalle.FechaDesde = dtpFechaDesde.Value.Date
                detalle.FechaHasta = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
                detalle.Observaciones = txtObservaciones.Text.Trim()
                Estado.OrdenCincoDetalle = detalle
            Case 5
                Dim detalle = If(Estado.Id > 0, Estado.RetenDetalle, New RetenDetalle())
                detalle.FechaReten = dtpFechaDesde.Value.Date
                detalle.Observaciones = txtObservaciones.Text.Trim()
                detalle.Turno = txtTurnoReten.Text.Trim()
                Estado.RetenDetalle = detalle
            Case 6
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
            openDialog.Filter = "Archivos Soportados (*.pdf;*.jpg;*.jpeg;*.png)|*.pdf;*.jpg;*.jpeg;*.png|Todos los archivos (*.*)|*.*"

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

    Private Sub dgvAdjuntos_SelectionChanged(sender As Object, e As EventArgs)
        If dgvAdjuntos.CurrentRow Is Nothing Then
            MostrarPanelMensaje("Seleccione un archivo para previsualizar")
            Return
        End If

        Try
            Dim adjuntoId = CInt(dgvAdjuntos.CurrentRow.Cells("Id").Value)
            Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
            Dim adjunto = repo.GetById(adjuntoId)

            If adjunto Is Nothing Then Return

            Select Case adjunto.TipoMIME.ToLower()
                Case "image/jpeg", "image/png"
                    MostrarImagenPreview(adjunto.Contenido)
                Case "application/pdf"
                    MostrarPdfPreview(adjunto.Contenido)
                Case Else
                    MostrarPanelMensaje("Vista previa no disponible para este tipo de archivo.")
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
            ' Crear un archivo temporal con la extensión .pdf
            Dim tempPdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf")
            File.WriteAllBytes(tempPdfPath, contenido)
            _tempFiles.Add(tempPdfPath) ' Añadir a la lista para limpieza posterior
            wbPreview.Navigate(tempPdfPath)
        Catch ex As Exception
            MessageBox.Show($"No se pudo mostrar el PDF: {ex.Message}", "Error de Visualización", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        ' Limpiar la imagen para liberar memoria
        If pbPreview.Image IsNot Nothing Then
            pbPreview.Image.Dispose()
            pbPreview.Image = Nothing
        End If
        ' Detener la navegación del WebBrowser
        wbPreview.Stop()
        wbPreview.Dispose()

        ' Eliminar archivos temporales creados para los PDF
        For Each filePath In _tempFiles
            Try
                If File.Exists(filePath) Then
                    File.Delete(filePath)
                End If
            Catch ex As Exception
                ' Opcional: registrar el error si la limpieza falla
                Console.WriteLine($"No se pudo eliminar el archivo temporal: {filePath}")
            End Try
        Next
    End Sub

#End Region

End Class