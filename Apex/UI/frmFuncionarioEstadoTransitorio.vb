Imports System.IO
Imports System.Linq

Public Class frmFuncionarioEstadoTransitorio

    Public Estado As EstadoTransitorio
    Private _tiposEstado As List(Of TipoEstadoTransitorio)
    Private _unitOfWork As IUnitOfWork
    Private _tempFiles As New List(Of String) ' Lista para rastrear archivos PDF temporales
    Private _readOnly As Boolean = False

    ' Propiedades para los detalles
    Public DesignacionDetalle As DesignacionDetalle
    Public SancionDetalle As SancionDetalle
    Public SumarioDetalle As SumarioDetalle
    Public OrdenCincoDetalle As OrdenCincoDetalle
    Public EnfermedadDetalle As EnfermedadDetalle
    Public RetenDetalle As RetenDetalle
    Public TrasladoDetalle As TrasladoDetalle

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
        TiposEstadoCatalog.Init(_unitOfWork)
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

        If Estado IsNot Nothing AndAlso Estado.Id > 0 Then
            ' MODO EDICIÓN O LECTURA
            cboTipoEstado.SelectedValue = Estado.TipoEstadoTransitorioId
            cboTipoEstado.Enabled = False
            CargarDatosDeDetalle()
            CargarAdjuntos(Estado.Id)

            GroupBox1.Enabled = True
            If dgvAdjuntos.Rows.Count = 0 Then
                MostrarPanelMensaje("Haga clic en 'Adjuntar' para agregar un archivo.")
                pnlPreview.Enabled = False
                dgvAdjuntos.Enabled = False
            Else
                dgvAdjuntos.Enabled = True
                pnlPreview.Enabled = True
            End If
        Else
            ' MODO CREACIÓN
            Estado = New EstadoTransitorio()
            chkFechaHasta.Checked = True
            cboTipoEstado.SelectedIndex = -1

            ' Habilitamos la sección de adjuntos desde el inicio
            GroupBox1.Enabled = True
            dgvAdjuntos.Enabled = True ' La grilla estará vacía pero funcional
            pnlPreview.Enabled = False ' La previsualización no tiene sentido aún
            MostrarPanelMensaje("Haga clic en 'Adjuntar' para agregar un archivo.")
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

        ' Corrección: Cambiamos Visible a Enabled
        btnAdjuntar.Enabled = False
        btnEliminarAdjunto.Enabled = False
        btnVerAdjunto.Enabled = dgvAdjuntos.Rows.Count > 0 ' Solo habilitado si hay algo que ver

        btnGuardar.Text = "Cerrar"
        btnCancelar.Visible = False

        ' Centrar el botón Cerrar
        btnGuardar.Location = New Point((Me.ClientSize.Width - btnGuardar.Width) / 2, btnGuardar.Location.Y)
    End Sub

    Private Sub CargarDatosDeDetalle()
        Dim fechaHasta As Date? = Nothing
        Dim observaciones As String = ""

        Select Case Estado.TipoEstadoTransitorioId
            Case TiposEstadoCatalog.Designacion
                Dim d = Estado.DesignacionDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.DocResolucion

            Case TiposEstadoCatalog.Enfermedad
                Dim d = Estado.EnfermedadDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtDiagnostico.Text = d.Diagnostico

            Case TiposEstadoCatalog.Sancion
                Dim d = Estado.SancionDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion

            Case TiposEstadoCatalog.OrdenCinco
                Dim d = Estado.OrdenCincoDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones

            Case TiposEstadoCatalog.Reten
                Dim d = Estado.RetenDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaReten
                observaciones = d.Observaciones
                txtTurnoReten.Text = d.Turno

            Case TiposEstadoCatalog.Sumario
                Dim d = Estado.SumarioDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Expediente

            Case TiposEstadoCatalog.Traslado
                Dim d = Estado.TrasladoDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
        End Select

        txtObservaciones.Text = observaciones

        ' Para Retén no se maneja FechaHasta
        If Estado.TipoEstadoTransitorioId <> TiposEstadoCatalog.Reten Then
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
        ' Reset UI
        ShowExtra(False, False, False)
        lblResolucion.Text = "Resolución:"
        lblFechaDesde.Text = "Fecha Desde:"
        ToggleHastaSection(True)

        If cboTipoEstado.SelectedIndex = -1 OrElse cboTipoEstado.SelectedValue Is Nothing Then Exit Sub

        Dim tipoId As Integer
        If Not Integer.TryParse(cboTipoEstado.SelectedValue.ToString(), tipoId) Then Exit Sub

        Select Case tipoId
            Case TiposEstadoCatalog.Designacion, TiposEstadoCatalog.Sancion, TiposEstadoCatalog.Sumario
                ShowExtra(True, False, False)
                ' Ajuste de etiqueta según el tipo
                If tipoId = TiposEstadoCatalog.Designacion Then lblResolucion.Text = "Doc. Resolución:"
                If tipoId = TiposEstadoCatalog.Sumario Then lblResolucion.Text = "Expediente:"

            Case TiposEstadoCatalog.Enfermedad
                ShowExtra(False, True, False)

            Case TiposEstadoCatalog.Reten
                ShowExtra(False, False, True)
                lblFechaDesde.Text = "Fecha Retén:"
                ToggleHastaSection(False)

            Case TiposEstadoCatalog.OrdenCinco, TiposEstadoCatalog.Traslado
                ' No tienen campos adicionales visibles

            Case Else
                ' Nada
        End Select
    End Sub

    ' Helpers
    Private Sub ShowExtra(showResol As Boolean, showDiag As Boolean, showReten As Boolean)
        lblResolucion.Visible = showResol : txtResolucion.Visible = showResol
        lblDiagnostico.Visible = showDiag : txtDiagnostico.Visible = showDiag
        lblTurnoReten.Visible = showReten : txtTurnoReten.Visible = showReten
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
    End Sub

    Private Sub chkFechaHasta_CheckedChanged(sender As Object, e As EventArgs) Handles chkFechaHasta.CheckedChanged
        dtpFechaHasta.Enabled = Not chkFechaHasta.Checked
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If _readOnly Then
            Close()
            Return
        End If

        If cboTipoEstado.SelectedIndex = -1 OrElse cboTipoEstado.SelectedValue Is Nothing Then
            MessageBox.Show("Debe seleccionar un tipo de estado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim tipoId As Integer
        If Not Integer.TryParse(cboTipoEstado.SelectedValue.ToString(), tipoId) Then
            MessageBox.Show("Tipo de estado inválido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Validación de fechas (no aplica para Retén)
        Dim fechaHastaSel As Date? = If(chkFechaHasta.Checked, CType(Nothing, Date?), dtpFechaHasta.Value.Date)
        If tipoId <> TiposEstadoCatalog.Reten AndAlso fechaHastaSel.HasValue AndAlso fechaHastaSel.Value < dtpFechaDesde.Value.Date Then
            MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Estado.TipoEstadoTransitorioId = tipoId

        Select Case tipoId
            Case TiposEstadoCatalog.Designacion
                Dim d = If(Estado.DesignacionDetalle, New DesignacionDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.DocResolucion = txtResolucion.Text.Trim()
                Estado.DesignacionDetalle = d
            ' d.EstadoTransitorio = Estado ' <- solo si tu mapeo 1-1 lo requiere

            Case TiposEstadoCatalog.Enfermedad
                Dim d = If(Estado.EnfermedadDetalle, New EnfermedadDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Diagnostico = txtDiagnostico.Text.Trim()
                Estado.EnfermedadDetalle = d

            Case TiposEstadoCatalog.Sancion
                Dim d = If(Estado.SancionDetalle, New SancionDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                Estado.SancionDetalle = d

            Case TiposEstadoCatalog.OrdenCinco
                Dim d = If(Estado.OrdenCincoDetalle, New OrdenCincoDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                Estado.OrdenCincoDetalle = d

            Case TiposEstadoCatalog.Reten
                Dim d = If(Estado.RetenDetalle, New RetenDetalle())
                d.FechaReten = dtpFechaDesde.Value.Date
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Turno = txtTurnoReten.Text.Trim()
                Estado.RetenDetalle = d

            Case TiposEstadoCatalog.Sumario
                Dim d = If(Estado.SumarioDetalle, New SumarioDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Expediente = txtResolucion.Text.Trim()
                Estado.SumarioDetalle = d

            Case TiposEstadoCatalog.Traslado
                Dim d = If(Estado.TrasladoDetalle, New TrasladoDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                Estado.TrasladoDetalle = d
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
    ' Clase liviana para la grilla
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

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = NameOf(AdjuntoRow.Id),
            .Name = "Id",
            .Visible = False
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = NameOf(AdjuntoRow.NombreArchivo),
            .HeaderText = "Nombre del Archivo",
            .Name = "NombreArchivo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })

            Dim colFecha As New DataGridViewTextBoxColumn With {
            .DataPropertyName = NameOf(AdjuntoRow.FechaCreacion),
            .HeaderText = "Fecha de Carga",
            .Name = "FechaCreacion",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        }
            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm"
            .Columns.Add(colFecha)

            ' (Opcional, útil para depurar) columna Origen
            '.Columns.Add(New DataGridViewTextBoxColumn With {
            '    .DataPropertyName = NameOf(AdjuntoRow.Origen),
            '    .HeaderText = "Origen",
            '    .Name = "Origen",
            '    .Width = 80
            '})
        End With
    End Sub

    Private Sub CargarAdjuntos(estadoId As Integer)
        ' Configurar columnas una sola vez
        If dgvAdjuntos.Columns.Count = 0 Then ConfigurarGrillaAdjuntos()

        ' Guardar selección actual (si la hay) para restaurarla luego
        Dim idSeleccionado As Integer? = Nothing
        If dgvAdjuntos.CurrentRow IsNot Nothing AndAlso dgvAdjuntos.CurrentRow.DataBoundItem IsNot Nothing Then
            Dim actual = TryCast(dgvAdjuntos.CurrentRow.DataBoundItem, AdjuntoRow)
            If actual IsNot Nothing Then idSeleccionado = actual.Id
        End If

        Dim adjuntos As New List(Of AdjuntoRow)

        ' Desde BD
        If estadoId > 0 Then
            Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
            ' Proyecta solo lo necesario (Id, Nombre, Fecha) – evita traer BLOB al grid
            Dim adjuntosDB = repo.GetAll().
            Where(Function(a) a.EstadoTransitorioId = estadoId).
            Select(Function(a) New AdjuntoRow With {
                .Id = a.Id,
                .NombreArchivo = a.NombreArchivo,
                .FechaCreacion = a.FechaCreacion,
                .Origen = "BD"
            }).ToList()
            adjuntos.AddRange(adjuntosDB)
        End If

        ' Desde memoria (Adjuntos nuevos aún no persistidos)
        If Estado IsNot Nothing AndAlso Estado.AdjuntosNuevos IsNot Nothing AndAlso Estado.AdjuntosNuevos.Count > 0 Then
            Dim adjuntosMemoria = Estado.AdjuntosNuevos.Select(Function(a) New AdjuntoRow With {
            .Id = a.Id, ' negativo (tu convención), perfecto
            .NombreArchivo = a.NombreArchivo,
            .FechaCreacion = a.FechaCreacion,
            .Origen = "Memoria"
        }).ToList()
            adjuntos.AddRange(adjuntosMemoria)
        End If

        ' Orden estable y útil: más recientes primero, luego por nombre
        Dim listaOrdenada = adjuntos.
        OrderByDescending(Function(x) x.FechaCreacion).
        ThenBy(Function(x) x.NombreArchivo).
        ToList()

        ' Enlazar (BindingSource para refrescos y orden)
        Dim bs = TryCast(dgvAdjuntos.DataSource, BindingSource)
        If bs Is Nothing Then
            bs = New BindingSource()
            dgvAdjuntos.DataSource = bs
        End If
        bs.DataSource = listaOrdenada

        ' Habilitar/Deshabilitar UI
        Dim hay = listaOrdenada.Count > 0
        dgvAdjuntos.Enabled = hay
        pnlPreview.Enabled = hay
        If Not hay Then
            MostrarPanelMensaje("No hay archivos adjuntos.")
        End If

        ' Restaurar selección si es posible
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
            dgvAdjuntos.CurrentCell = dgvAdjuntos.Rows(0).Cells("NombreArchivo")
        End If
    End Sub

    'Private Sub CargarAdjuntos(estadoId As Integer)
    '    Dim adjuntosMostrados As New List(Of Object)

    '    If estadoId > 0 Then
    '        Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
    '        Dim adjuntosDB = repo.GetAll().Where(Function(a) a.EstadoTransitorioId = estadoId) _
    '                                .Select(Function(a) New With {a.Id, a.NombreArchivo, a.FechaCreacion}) _
    '                                .ToList()
    '        adjuntosMostrados.AddRange(adjuntosDB)
    '    End If

    '    If Estado IsNot Nothing AndAlso Estado.AdjuntosNuevos.Any() Then
    '        Dim adjuntosMemoria = Estado.AdjuntosNuevos.Select(Function(a) New With {a.Id, a.NombreArchivo, a.FechaCreacion}).ToList()
    '        adjuntosMostrados.AddRange(adjuntosMemoria)
    '    End If

    '    dgvAdjuntos.DataSource = Nothing
    '    dgvAdjuntos.DataSource = adjuntosMostrados

    '    If dgvAdjuntos.Rows.Count > 0 Then
    '        dgvAdjuntos.Columns("Id").Visible = False
    '        dgvAdjuntos.Columns("NombreArchivo").HeaderText = "Nombre del Archivo"
    '        dgvAdjuntos.Columns("NombreArchivo").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
    '        dgvAdjuntos.Columns("FechaCreacion").HeaderText = "Fecha de Carga"
    '        dgvAdjuntos.Enabled = True
    '        pnlPreview.Enabled = True
    '    Else
    '        dgvAdjuntos.Enabled = False
    '        pnlPreview.Enabled = False
    '        MostrarPanelMensaje("No hay archivos adjuntos.")
    '    End If
    'End Sub

    Private Sub btnAdjuntar_Click(sender As Object, e As EventArgs) Handles btnAdjuntar.Click
        Using openDialog As New OpenFileDialog()
            openDialog.Title = "Seleccione un archivo (PDF o Imagen)"
            openDialog.Filter = "Archivos Soportados (*.pdf;*.jpg;*.jpeg;*.png)|*.pdf;*.jpg;*.jpeg;*.png|Todos los archivos (*.*)|*.*"

            If openDialog.ShowDialog() = DialogResult.OK Then
                Try
                    Dim contenidoBytes = File.ReadAllBytes(openDialog.FileName)
                    Dim nuevoAdjunto As New EstadoTransitorioAdjunto With {
                        .NombreArchivo = Path.GetFileName(openDialog.FileName),
                        .TipoMIME = GetMimeType(openDialog.FileName),
                        .Contenido = contenidoBytes,
                        .FechaCreacion = DateTime.Now
                    }

                    If Estado.Id = 0 Then
                        nuevoAdjunto.Id = -(Estado.AdjuntosNuevos.Count + 1)
                        Estado.AdjuntosNuevos.Add(nuevoAdjunto)
                        CargarAdjuntos(0)
                    Else
                        nuevoAdjunto.EstadoTransitorioId = Estado.Id
                        Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
                        repo.Add(nuevoAdjunto)
                        _unitOfWork.Commit()
                        CargarAdjuntos(Estado.Id)
                    End If
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
            Dim adjunto As EstadoTransitorioAdjunto

            If adjuntoId < 0 Then
                adjunto = Estado.AdjuntosNuevos.FirstOrDefault(Function(a) a.Id = adjuntoId)
            Else
                Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
                adjunto = repo.GetById(adjuntoId)
            End If

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

                If adjuntoId < 0 Then
                    Dim adjuntoAEliminar = Estado.AdjuntosNuevos.FirstOrDefault(Function(a) a.Id = adjuntoId)
                    If adjuntoAEliminar IsNot Nothing Then
                        Estado.AdjuntosNuevos.Remove(adjuntoAEliminar)
                    End If
                    CargarAdjuntos(Estado.Id)
                Else
                    Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
                    repo.RemoveById(adjuntoId)
                    _unitOfWork.Commit()
                    CargarAdjuntos(Estado.Id)
                End If
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
            Dim adjunto As EstadoTransitorioAdjunto

            If adjuntoId < 0 Then
                adjunto = Estado.AdjuntosNuevos.FirstOrDefault(Function(a) a.Id = adjuntoId)
            Else
                Dim repo = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
                adjunto = repo.GetById(adjuntoId)
            End If

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
            Dim tempPdfPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.pdf")
            File.WriteAllBytes(tempPdfPath, contenido)
            _tempFiles.Add(tempPdfPath)
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
        If pbPreview.Image IsNot Nothing Then
            pbPreview.Image.Dispose()
            pbPreview.Image = Nothing
        End If

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
                If File.Exists(filePath) Then
                    File.Delete(filePath)
                End If
            Catch ex As Exception
                Console.WriteLine($"No se pudo eliminar el archivo temporal: {filePath}. Error: {ex.Message}")
            End Try
        Next
    End Sub

#End Region
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            btnCancelar.PerformClick()
        End If
    End Sub
End Class