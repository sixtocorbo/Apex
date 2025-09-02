Imports System.IO
Imports System.Linq
Imports System.Data.Entity

Public Class frmFuncionarioEstadoTransitorio

    Public Estado As EstadoTransitorio
    Private _tiposEstado As List(Of TipoEstadoTransitorio)
    Private _unitOfWork As IUnitOfWork
    Private _tempFiles As New List(Of String)
    Private _readOnly As Boolean = False
    Private _listaCargos As List(Of Cargo)

    ' Propiedades para todos los detalles
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

        ' Si estamos editando y no viene cargado el Funcionario, intentamos cargarlo.
        If Estado IsNot Nothing AndAlso Estado.Id > 0 AndAlso Estado.Funcionario Is Nothing Then
            Try
                _unitOfWork.Context.Entry(Estado).Reference(Function(x) x.Funcionario).Load()
            Catch
                ' ignorar si falla
            End Try
        End If

        If Estado IsNot Nothing AndAlso Estado.Id > 0 Then
            ' MODO EDICIÓN O LECTURA
            cboTipoEstado.SelectedValue = Estado.TipoEstadoTransitorioId
            cboTipoEstado.Enabled = False
            CargarDatosDeDetalle()
            CargarAdjuntos(Estado.Id)
            pnlPreview.Enabled = dgvAdjuntos.Rows.Count > 0
            dgvAdjuntos.Enabled = dgvAdjuntos.Rows.Count > 0
        Else
            ' MODO CREACIÓN
            If Estado Is Nothing Then Estado = New EstadoTransitorio()
            chkFechaHasta.Checked = True
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
        cboTipoEstado.Enabled = False
        dtpFechaDesde.Enabled = False
        dtpFechaHasta.Enabled = False
        chkFechaHasta.Enabled = False
        txtObservaciones.ReadOnly = True
        txtResolucion.ReadOnly = True
        txtDiagnostico.ReadOnly = True
        txtTurnoReten.ReadOnly = True
        cboCargoAnterior.Enabled = False
        cboCargoNuevo.Enabled = False
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

        Select Case Estado.TipoEstadoTransitorioId
            Case 1 ' Designacion
                Dim d = Estado.DesignacionDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.DocResolucion

            Case 2 ' Enfermedad
                Dim d = Estado.EnfermedadDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtDiagnostico.Text = d.Diagnostico

            Case 3 ' Sancion
                Dim d = Estado.SancionDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion

            Case 4 ' OrdenCinco
                Dim d = Estado.OrdenCincoDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones

            Case 5 ' Reten
                Dim d = Estado.RetenDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaReten
                observaciones = d.Observaciones
                txtTurnoReten.Text = d.Turno

            Case 6 ' Sumario
                Dim d = Estado.SumarioDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Expediente

            Case 21 ' Traslado
                Dim d = Estado.TrasladoDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones

            Case 29 ' Baja de Funcionario
                Dim d = Estado.BajaDeFuncionarioDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones

            Case 30 ' Cambio de Cargo
                Dim d = Estado.CambioDeCargoDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion
                cboCargoAnterior.SelectedValue = d.CargoAnteriorId
                cboCargoNuevo.SelectedValue = d.CargoNuevoId

            Case 31 ' Reactivación de Funcionario
                Dim d = Estado.ReactivacionDeFuncionarioDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                observaciones = d.Observaciones
                txtResolucion.Text = d.Resolucion

            Case 32 ' Separación del Cargo
                Dim d = Estado.SeparacionDelCargoDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones

            Case 33 ' Inicio de Procesamiento
                Dim d = Estado.InicioDeProcesamientoDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
                txtResolucion.Text = d.Expediente

            Case 34 ' Desarmado
                Dim d = Estado.DesarmadoDetalle
                If d Is Nothing Then Exit Select
                dtpFechaDesde.Value = d.FechaDesde
                fechaHasta = d.FechaHasta
                observaciones = d.Observaciones
        End Select

        txtObservaciones.Text = observaciones

        If Estado.TipoEstadoTransitorioId <> 5 AndAlso Estado.TipoEstadoTransitorioId <> 31 Then
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
        ShowExtra(False, False, False, False)
        lblResolucion.Text = "Resolución:"
        lblFechaDesde.Text = "Fecha Desde:"
        ToggleHastaSection(True)

        If cboTipoEstado.SelectedIndex = -1 OrElse cboTipoEstado.SelectedValue Is Nothing Then Exit Sub

        Dim tipoId As Integer
        If Not Integer.TryParse(cboTipoEstado.SelectedValue.ToString(), tipoId) Then Exit Sub

        Select Case tipoId
            Case 1, 3, 6, 31, 33
                ShowExtra(True, False, False, False)
                If tipoId = 1 Then lblResolucion.Text = "Doc. Resolución:"
                If tipoId = 6 Or tipoId = 33 Then lblResolucion.Text = "Expediente:"

            Case 30 ' Cambio de Cargo
                ShowExtra(True, False, False, True)
                lblResolucion.Text = "Resolución:"
                SetCargoAnteriorDesdeFuncionario()

            Case 2 ' Enfermedad
                ShowExtra(False, True, False, False)

            Case 5 ' Reten
                ShowExtra(False, False, True, False)
                lblFechaDesde.Text = "Fecha Retén:"
                ToggleHastaSection(False)

            Case 4, 21, 29, 32, 34
                ShowExtra(False, False, False, False)
            Case Else
                ShowExtra(False, False, False, False)
        End Select
    End Sub

    Private Sub ShowExtra(showResol As Boolean, showDiag As Boolean, showReten As Boolean, showCargos As Boolean)
        lblResolucion.Visible = showResol
        txtResolucion.Visible = showResol
        lblDiagnostico.Visible = showDiag
        txtDiagnostico.Visible = showDiag
        lblTurnoReten.Visible = showReten
        txtTurnoReten.Visible = showReten
        lblCargoAnterior.Visible = showCargos
        cboCargoAnterior.Visible = showCargos
        lblCargoNuevo.Visible = showCargos
        cboCargoNuevo.Visible = showCargos
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

        Dim tipoId As Integer = CInt(cboTipoEstado.SelectedValue)
        Dim fechaHastaSel As Date? = If(chkFechaHasta.Checked Or Not chkFechaHasta.Visible, CType(Nothing, Date?), dtpFechaHasta.Value.Date)

        If tipoId <> 5 AndAlso tipoId <> 31 AndAlso fechaHastaSel.HasValue AndAlso fechaHastaSel.Value < dtpFechaDesde.Value.Date Then
            MessageBox.Show("La fecha de fin no puede ser anterior a la fecha de inicio.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Estado.TipoEstadoTransitorioId = tipoId

        Select Case tipoId
            Case 1 ' Designacion
                Dim d = If(Estado.DesignacionDetalle, New DesignacionDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.DocResolucion = txtResolucion.Text.Trim()
                Estado.DesignacionDetalle = d

            Case 2 ' Enfermedad
                Dim d = If(Estado.EnfermedadDetalle, New EnfermedadDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Diagnostico = txtDiagnostico.Text.Trim()
                Estado.EnfermedadDetalle = d

            Case 3 ' Sancion
                Dim d = If(Estado.SancionDetalle, New SancionDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                Estado.SancionDetalle = d

            Case 4 ' OrdenCinco
                Dim d = If(Estado.OrdenCincoDetalle, New OrdenCincoDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                Estado.OrdenCincoDetalle = d

            Case 5 ' Reten
                Dim d = If(Estado.RetenDetalle, New RetenDetalle())
                d.FechaReten = dtpFechaDesde.Value.Date
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Turno = txtTurnoReten.Text.Trim()
                Estado.RetenDetalle = d

            Case 6 ' Sumario
                Dim d = If(Estado.SumarioDetalle, New SumarioDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Expediente = txtResolucion.Text.Trim()
                Estado.SumarioDetalle = d

            Case 21 ' Traslado
                Dim d = If(Estado.TrasladoDetalle, New TrasladoDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                Estado.TrasladoDetalle = d

            Case 29 ' Baja de Funcionario
                Dim d = If(Estado.BajaDeFuncionarioDetalle, New BajaDeFuncionarioDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                Estado.BajaDeFuncionarioDetalle = d

            Case 30 ' Cambio de Cargo
                If cboCargoNuevo.SelectedIndex = -1 Then
                    MessageBox.Show("Debe seleccionar el nuevo cargo.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                ' Asegurar funcionario cargado si estamos editando
                If Estado.Funcionario Is Nothing AndAlso Estado.Id > 0 Then
                    Try
                        _unitOfWork.Context.Entry(Estado).Reference(Function(x) x.Funcionario).Load()
                    Catch
                        ' ignorar
                    End Try
                End If

                Dim cargoActualId As Integer? = Estado.Funcionario?.CargoId
                If Not cargoActualId.HasValue Then
                    MessageBox.Show("El funcionario no tiene un cargo actual asignado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Dim cargoNuevoId = CType(cboCargoNuevo.SelectedValue, Integer)

                If cargoNuevoId = cargoActualId.Value Then
                    MessageBox.Show("El nuevo cargo no puede ser igual al cargo actual.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
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
                    mensajeConfirmacion = $"¡ATENCIÓN! El nuevo cargo '{cargoNuevo.Nombre}' representa un DESCENSO." & vbCrLf & "¿Desea continuar y aplicar el cambio de todas formas?"
                    icono = MessageBoxIcon.Warning
                Else
                    mensajeConfirmacion = $"El nuevo cargo '{cargoNuevo.Nombre}' tiene el mismo grado que el actual (movimiento lateral)." & vbCrLf & "¿Desea confirmar el cambio?"
                End If

                If MessageBox.Show(mensajeConfirmacion, tituloConfirmacion, MessageBoxButtons.YesNo, icono) = DialogResult.No Then
                    Return
                End If

                Dim d = If(Estado.CambioDeCargoDetalle, New CambioDeCargoDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                d.CargoAnteriorId = cargoActualId.Value
                d.CargoNuevoId = cargoNuevoId
                Estado.CambioDeCargoDetalle = d

                ' Actualizar cargo del funcionario
                Estado.Funcionario.CargoId = cargoNuevoId

            Case 31 ' Reactivación de Funcionario
                Dim d = If(Estado.ReactivacionDeFuncionarioDetalle, New ReactivacionDeFuncionarioDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Resolucion = txtResolucion.Text.Trim()
                Estado.ReactivacionDeFuncionarioDetalle = d

            Case 32 ' Separación del Cargo
                Dim d = If(Estado.SeparacionDelCargoDetalle, New SeparacionDelCargoDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                Estado.SeparacionDelCargoDetalle = d

            Case 33 ' Inicio de Procesamiento
                Dim d = If(Estado.InicioDeProcesamientoDetalle, New InicioDeProcesamientoDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                d.Expediente = txtResolucion.Text.Trim()
                Estado.InicioDeProcesamientoDetalle = d

            Case 34 ' Desarmado
                Dim d = If(Estado.DesarmadoDetalle, New DesarmadoDetalle())
                d.FechaDesde = dtpFechaDesde.Value.Date
                d.FechaHasta = fechaHastaSel
                d.Observaciones = txtObservaciones.Text.Trim()
                Estado.DesarmadoDetalle = d
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
                Try
                    Dim contenidoBytes = File.ReadAllBytes(openDialog.FileName)
                    Dim nuevoAdjunto = New EstadoTransitorioAdjunto With {
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
        If dgvAdjuntos.CurrentRow Is Nothing Then Return
        If MessageBox.Show("¿Está seguro de que desea eliminar este archivo adjunto?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim adjuntoId = CInt(dgvAdjuntos.CurrentRow.Cells("Id").Value)
                If adjuntoId < 0 Then
                    Dim adjuntoAEliminar = Estado.AdjuntosNuevos.FirstOrDefault(Function(a) a.Id = adjuntoId)
                    If adjuntoAEliminar IsNot Nothing Then Estado.AdjuntosNuevos.Remove(adjuntoAEliminar)
                Else
                    _unitOfWork.Repository(Of EstadoTransitorioAdjunto)().RemoveById(adjuntoId)
                    _unitOfWork.Commit()
                End If
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

    ' ===== Helper seguro para "Cambio de Cargo" =====
    Private Sub SetCargoAnteriorDesdeFuncionario()
        ' Si el estado ya existe y la navegación no vino cargada, intentar cargarla
        If Estado IsNot Nothing AndAlso Estado.Id > 0 AndAlso Estado.Funcionario Is Nothing Then
            Try
                _unitOfWork.Context.Entry(Estado).Reference(Function(e) e.Funcionario).Load()
            Catch
                ' ignorar si no carga
            End Try
        End If

        Dim cargoAnteriorId As Integer? = Nothing

        ' Si estamos editando y ya existe un detalle de cambio, usamos ese cargo anterior
        If Estado?.CambioDeCargoDetalle IsNot Nothing AndAlso Estado.CambioDeCargoDetalle.CargoAnteriorId > 0 Then
            cargoAnteriorId = Estado.CambioDeCargoDetalle.CargoAnteriorId
        ElseIf Estado IsNot Nothing AndAlso Estado.Funcionario IsNot Nothing Then
            cargoAnteriorId = Estado.Funcionario.CargoId
        End If

        ' Preparar combo seguro
        cboCargoAnterior.SelectedIndex = -1

        If cargoAnteriorId.HasValue AndAlso _listaCargos IsNot Nothing AndAlso
           _listaCargos.Any(Function(c) c.Id = cargoAnteriorId.Value) Then
            cboCargoAnterior.SelectedValue = cargoAnteriorId.Value
        End If

        cboCargoAnterior.Enabled = False
    End Sub

End Class
