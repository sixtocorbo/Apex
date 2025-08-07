' Apex/UI/frmFuncionarioCrear.vb
Option Strict On
Option Explicit On

Imports System.IO
Imports System.Data.Entity
Imports System.ComponentModel

Public Enum ModoFormulario
    Crear
    Editar
End Enum

Public Class frmFuncionarioCrear
    Inherits Form

    ' --- Variables de la clase ---
    Private _uow As UnitOfWork
    Private _funcionario As Funcionario
    Private _svc As FuncionarioService
    Private _modo As ModoFormulario
    Private _idFuncionario As Integer
    Private _rutaFotoSeleccionada As String
    Private _tiposEstadoTransitorio As List(Of TipoEstadoTransitorio)
    Private _dotaciones As BindingList(Of FuncionarioDotacion)
    Private _estadosTransitorios As BindingList(Of EstadoTransitorio)
    Private _historialConsolidado As List(Of Object)
    Private _itemsDotacion As List(Of DotacionItem)


    '-------------------- Constructores ----------------------
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _uow = New UnitOfWork()
        _funcionario = New Funcionario() With {.FechaIngreso = DateTime.Now}
        _uow.Context.Set(Of Funcionario).Add(_funcionario)
    End Sub

    Public Sub New(id As Integer)
        InitializeComponent()
        _modo = ModoFormulario.Editar
        _idFuncionario = id
        _uow = New UnitOfWork()
        _funcionario = _uow.Context.Set(Of Funcionario)().
                        Include(Function(f) f.FuncionarioDotacion.Select(Function(fd) fd.DotacionItem)).
                        Include(Function(f) f.EstadoTransitorio.Select(Function(et) et.TipoEstadoTransitorio)).
                        FirstOrDefault(Function(f) f.Id = id)

        If _funcionario IsNot Nothing AndAlso _funcionario.EstadoTransitorio IsNot Nothing Then
            For Each et In _funcionario.EstadoTransitorio
                Select Case et.TipoEstadoTransitorioId
                    Case 1 : _uow.Context.Entry(et).Reference(Function(x) x.DesignacionDetalle).Load()
                    Case 2 : _uow.Context.Entry(et).Reference(Function(x) x.EnfermedadDetalle).Load()
                    Case 3 : _uow.Context.Entry(et).Reference(Function(x) x.SancionDetalle).Load()
                    Case 4 : _uow.Context.Entry(et).Reference(Function(x) x.OrdenCincoDetalle).Load()
                    Case 5 : _uow.Context.Entry(et).Reference(Function(x) x.RetenDetalle).Load()
                    Case 6 : _uow.Context.Entry(et).Reference(Function(x) x.SumarioDetalle).Load()
                End Select
            Next
        End If
    End Sub

    '------------------- Carga del Formulario --------------------------
    Private Async Sub frmFuncionarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New FuncionarioService(_uow)
        Await CargarCombosAsync()

        _tiposEstadoTransitorio = Await _svc.ObtenerTiposEstadoTransitorioCompletosAsync()
        _itemsDotacion = Await _svc.ObtenerItemsDotacionCompletosAsync()

        If _funcionario Is Nothing AndAlso _modo = ModoFormulario.Editar Then
            MessageBox.Show("No se encontró el registro del funcionario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Close()
            Return
        End If

        _dotaciones = New BindingList(Of FuncionarioDotacion)(_funcionario.FuncionarioDotacion.ToList())
        _estadosTransitorios = New BindingList(Of EstadoTransitorio)(_funcionario.EstadoTransitorio.ToList())

        ConfigurarGrillaDotacion()
        ConfigurarGrillaEstados()

        dgvDotacion.DataSource = _dotaciones
        dgvEstadosTransitorios.DataSource = _estadosTransitorios

        AddHandler dgvDotacion.CellFormatting, AddressOf dgvDotacion_CellFormatting
        AddHandler chkVerHistorial.CheckedChanged, AddressOf chkVerHistorial_CheckedChanged
        AddHandler dgvEstadosTransitorios.CellFormatting, AddressOf dgvEstadosTransitorios_CellFormatting
        AddHandler dgvEstadosTransitorios.SelectionChanged, AddressOf DgvEstadosTransitorios_SelectionChanged

        If _modo = ModoFormulario.Editar Then
            Me.Text = "Editar Funcionario"
            btnGuardar.Text = "Actualizar"
            CargarDatosEnControles()
        Else
            Me.Text = "Nuevo Funcionario"
            btnGuardar.Text = "Guardar"
            pbFoto.Image = My.Resources.Police
        End If
    End Sub

    Private Sub CargarDatosEnControles()
        txtCI.Text = _funcionario.CI
        txtNombre.Text = _funcionario.Nombre
        dtpFechaIngreso.Value = _funcionario.FechaIngreso
        cboTipoFuncionario.SelectedValue = CInt(_funcionario.TipoFuncionarioId)
        cboCargo.SelectedValue = If(_funcionario.CargoId.HasValue, CInt(_funcionario.CargoId), -1)
        chkActivo.Checked = _funcionario.Activo
        cboEscalafon.SelectedValue = If(_funcionario.EscalafonId.HasValue, CInt(_funcionario.EscalafonId), -1)
        cboFuncion.SelectedValue = If(_funcionario.FuncionId.HasValue, CInt(_funcionario.FuncionId), -1)

        If _funcionario.Foto IsNot Nothing AndAlso _funcionario.Foto.Length > 0 Then
            pbFoto.Image = New Bitmap(New MemoryStream(_funcionario.Foto))
        Else
            pbFoto.Image = My.Resources.Police
        End If

        dtpFechaNacimiento.Value = If(_funcionario.FechaNacimiento.HasValue, _funcionario.FechaNacimiento.Value, dtpFechaNacimiento.MinDate)
        txtDomicilio.Text = _funcionario.Domicilio
        txtEmail.Text = _funcionario.Email
        cboEstadoCivil.SelectedValue = If(_funcionario.EstadoCivilId.HasValue, CInt(_funcionario.EstadoCivilId), -1)
        cboGenero.SelectedValue = If(_funcionario.GeneroId.HasValue, CInt(_funcionario.GeneroId), -1)
        cboNivelEstudio.SelectedValue = If(_funcionario.NivelEstudioId.HasValue, CInt(_funcionario.NivelEstudioId), -1)
    End Sub

    Private Async Sub chkVerHistorial_CheckedChanged(sender As Object, e As EventArgs)
        If chkVerHistorial.Checked Then
            Await CargarHistorialCompleto()
        Else
            dgvEstadosTransitorios.DataSource = _estadosTransitorios
        End If
    End Sub

    Private Async Function CargarHistorialCompleto() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim repo = _uow.Repository(Of EstadoTransitorio)()
            Dim historial = Await repo.GetAll().
                Include(Function(et) et.TipoEstadoTransitorio).
                Include(Function(et) et.DesignacionDetalle).
                Include(Function(et) et.SancionDetalle).
                Include(Function(et) et.SumarioDetalle).
                Include(Function(et) et.OrdenCincoDetalle).
                Include(Function(et) et.EnfermedadDetalle).
                Include(Function(et) et.RetenDetalle).
                Where(Function(et) et.FuncionarioId = _idFuncionario).
                OrderByDescending(Function(et) et.Id).
                ToListAsync()

            _historialConsolidado = historial.Select(Function(h)
                                                         Dim fechaDesde As Date? = Nothing
                                                         Dim fechaHasta As Date? = Nothing
                                                         Dim observaciones As String = ""
                                                         Dim origen As String = "Estado"

                                                         Select Case h.TipoEstadoTransitorioId
                                                             Case 1 ' Designación
                                                                 If h.DesignacionDetalle IsNot Nothing Then
                                                                     fechaDesde = h.DesignacionDetalle.FechaDesde
                                                                     fechaHasta = h.DesignacionDetalle.FechaHasta
                                                                     observaciones = h.DesignacionDetalle.Observaciones
                                                                 End If
                                                             Case 2 ' Enfermedad
                                                                 If h.EnfermedadDetalle IsNot Nothing Then
                                                                     fechaDesde = h.EnfermedadDetalle.FechaDesde
                                                                     fechaHasta = h.EnfermedadDetalle.FechaHasta
                                                                     observaciones = h.EnfermedadDetalle.Observaciones
                                                                 End If
                                                             Case 3 ' Sanción
                                                                 If h.SancionDetalle IsNot Nothing Then
                                                                     fechaDesde = h.SancionDetalle.FechaDesde
                                                                     fechaHasta = h.SancionDetalle.FechaHasta
                                                                     observaciones = h.SancionDetalle.Observaciones
                                                                 End If
                                                             Case 4 ' Orden Cinco
                                                                 If h.OrdenCincoDetalle IsNot Nothing Then
                                                                     fechaDesde = h.OrdenCincoDetalle.FechaDesde
                                                                     fechaHasta = h.OrdenCincoDetalle.FechaHasta
                                                                     observaciones = h.OrdenCincoDetalle.Observaciones
                                                                 End If
                                                             Case 5 ' Retén
                                                                 If h.RetenDetalle IsNot Nothing Then
                                                                     fechaDesde = h.RetenDetalle.FechaReten
                                                                     fechaHasta = Nothing
                                                                     observaciones = h.RetenDetalle.Observaciones
                                                                 End If
                                                             Case 6 ' Sumario
                                                                 If h.SumarioDetalle IsNot Nothing Then
                                                                     fechaDesde = h.SumarioDetalle.FechaDesde
                                                                     fechaHasta = h.SumarioDetalle.FechaHasta
                                                                     observaciones = h.SumarioDetalle.Observaciones
                                                                 End If
                                                         End Select

                                                         Return New With {
                                                             .Id = h.Id,
                                                             .Origen = origen,
                                                             .TipoEstado = If(h.TipoEstadoTransitorio IsNot Nothing, h.TipoEstadoTransitorio.Nombre, "Desconocido"),
                                                             .FechaDesde = fechaDesde,
                                                             .FechaHasta = fechaHasta,
                                                             .Observaciones = observaciones
                                                         }
                                                     End Function).Cast(Of Object).ToList()

            dgvEstadosTransitorios.DataSource = _historialConsolidado
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function


    Private Sub DgvEstadosTransitorios_SelectionChanged(sender As Object, e As EventArgs)
        Dim editable = False

        If dgvEstadosTransitorios.CurrentRow Is Nothing OrElse dgvEstadosTransitorios.CurrentRow.DataBoundItem Is Nothing Then
            btnEditarEstado.Enabled = False
            btnQuitarEstado.Enabled = False
            Return
        End If

        Dim itemSeleccionado = dgvEstadosTransitorios.CurrentRow.DataBoundItem

        If Not chkVerHistorial.Checked Then
            editable = True
        Else
            Dim origenProperty As System.Reflection.PropertyInfo = itemSeleccionado.GetType().GetProperty("Origen")

            If origenProperty IsNot Nothing Then
                Dim origenValue = origenProperty.GetValue(itemSeleccionado, Nothing)
                If origenValue IsNot Nothing Then
                    editable = String.Equals(origenValue.ToString(), "Estado", StringComparison.OrdinalIgnoreCase)
                End If
            End If
        End If

        btnAñadirEstado.Enabled = Not chkVerHistorial.Checked
        btnEditarEstado.Enabled = editable
        btnQuitarEstado.Enabled = editable
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If String.IsNullOrWhiteSpace(txtCI.Text) OrElse String.IsNullOrWhiteSpace(txtNombre.Text) OrElse cboTipoFuncionario.SelectedIndex = -1 Then
                MessageBox.Show("Los campos CI, Nombre y Tipo de Funcionario son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            _funcionario.CI = txtCI.Text.Trim()
            _funcionario.Nombre = txtNombre.Text.Trim()
            _funcionario.FechaIngreso = dtpFechaIngreso.Value.Date
            _funcionario.TipoFuncionarioId = CInt(cboTipoFuncionario.SelectedValue)
            _funcionario.CargoId = If(cboCargo.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboCargo.SelectedValue))
            _funcionario.Activo = chkActivo.Checked
            _funcionario.EscalafonId = If(cboEscalafon.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEscalafon.SelectedValue))
            _funcionario.FuncionId = If(cboFuncion.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboFuncion.SelectedValue))
            _funcionario.FechaNacimiento = If(dtpFechaNacimiento.Value = dtpFechaNacimiento.MinDate, CType(Nothing, Date?), dtpFechaNacimiento.Value.Date)
            _funcionario.Domicilio = txtDomicilio.Text.Trim()
            _funcionario.Email = txtEmail.Text.Trim()
            _funcionario.EstadoCivilId = If(cboEstadoCivil.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEstadoCivil.SelectedValue))
            _funcionario.GeneroId = If(cboGenero.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboGenero.SelectedValue))
            _funcionario.NivelEstudioId = If(cboNivelEstudio.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboNivelEstudio.SelectedValue))

            If Not String.IsNullOrWhiteSpace(_rutaFotoSeleccionada) Then
                _funcionario.Foto = File.ReadAllBytes(_rutaFotoSeleccionada)
            End If

            If _modo = ModoFormulario.Crear Then
                _funcionario.CreatedAt = DateTime.Now
            Else
                _funcionario.UpdatedAt = DateTime.Now
            End If

            SincronizarColeccion(_funcionario.FuncionarioDotacion, _dotaciones)
            SincronizarColeccion(_funcionario.EstadoTransitorio, _estadosTransitorios)

            Await _uow.CommitAsync()
            MessageBox.Show(If(_modo = ModoFormulario.Crear, "Funcionario creado", "Funcionario actualizado") & " correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

            DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SincronizarColeccion(Of T As Class)(dbCollection As ICollection(Of T), formCollection As BindingList(Of T))
        Dim itemsParaBorrar = dbCollection.Except(formCollection).ToList()
        For Each item In itemsParaBorrar
            _uow.Context.Entry(item).State = EntityState.Deleted
        Next

        Dim itemsParaAnadir = formCollection.Except(dbCollection).ToList()
        For Each item In itemsParaAnadir
            dbCollection.Add(item)
        Next
    End Sub

#Region "Métodos Auxiliares y de UI"
    Private Async Function CargarCombosAsync() As Task
        cboTipoFuncionario.DataSource = Await _svc.ObtenerTiposFuncionarioAsync()
        cboTipoFuncionario.DisplayMember = "Value"
        cboTipoFuncionario.ValueMember = "Key"
        cboCargo.DataSource = Await _svc.ObtenerCargosAsync()
        cboCargo.DisplayMember = "Value"
        cboCargo.ValueMember = "Key"
        cboEscalafon.DataSource = Await _svc.ObtenerEscalafonesAsync()
        cboEscalafon.DisplayMember = "Value"
        cboEscalafon.ValueMember = "Key"
        cboFuncion.DataSource = Await _svc.ObtenerFuncionesAsync()
        cboFuncion.DisplayMember = "Value"
        cboFuncion.ValueMember = "Key"
        cboEstadoCivil.DataSource = Await _svc.ObtenerEstadosCivilesAsync()
        cboEstadoCivil.DisplayMember = "Value"
        cboEstadoCivil.ValueMember = "Key"
        cboGenero.DataSource = Await _svc.ObtenerGenerosAsync()
        cboGenero.DisplayMember = "Value"
        cboGenero.ValueMember = "Key"
        cboNivelEstudio.DataSource = Await _svc.ObtenerNivelesEstudioAsync()
        cboNivelEstudio.DisplayMember = "Value"
        cboNivelEstudio.ValueMember = "Key"
        cboCargo.SelectedIndex = -1
        cboEscalafon.SelectedIndex = -1
        cboFuncion.SelectedIndex = -1
        cboEstadoCivil.SelectedIndex = -1
        cboGenero.SelectedIndex = -1
        cboNivelEstudio.SelectedIndex = -1
    End Function

    Private Sub ConfigurarGrillaDotacion()
        With dgvDotacion
            .AutoGenerateColumns = False
            .Columns.Clear()
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "colItem", .HeaderText = "Ítem", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Talla", .HeaderText = "Talla"})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .HeaderText = "Observaciones", .Width = 200})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaAsign", .HeaderText = "Fecha Asignación", .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}})
        End With
    End Sub

    Private Sub ConfigurarGrillaEstados()
        With dgvEstadosTransitorios
            .AutoGenerateColumns = False
            .Columns.Clear()
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoEstado", .HeaderText = "Tipo de Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "FechaDesde", .HeaderText = "Desde"})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "FechaHasta", .HeaderText = "Hasta"})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Observaciones", .HeaderText = "Observaciones", .Width = 300})
        End With
    End Sub

    Private Sub dgvDotacion_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        Dim dgv = CType(sender, DataGridView)
        Dim colName = dgv.Columns(e.ColumnIndex).Name

        If colName = "colItem" Then
            Dim dotacion = TryCast(dgv.Rows(e.RowIndex).DataBoundItem, FuncionarioDotacion)
            If dotacion IsNot Nothing Then
                If dotacion.DotacionItem IsNot Nothing Then
                    e.Value = dotacion.DotacionItem.Nombre
                ElseIf dotacion.DotacionItemId > 0 AndAlso _itemsDotacion IsNot Nothing Then
                    Dim item = _itemsDotacion.FirstOrDefault(Function(i) i.Id = dotacion.DotacionItemId)
                    If item IsNot Nothing Then
                        e.Value = item.Nombre
                    End If
                End If
                e.FormattingApplied = True
            End If
        End If
    End Sub

    Private Sub dgvEstadosTransitorios_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        Dim dgv = CType(sender, DataGridView)
        Dim colName = dgv.Columns(e.ColumnIndex).Name

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Lógica única y robusta para formatear la grilla de estados.

        e.Value = "" ' Empezar con un valor vacío por defecto

        Dim dataItem = dgv.Rows(e.RowIndex).DataBoundItem
        If dataItem Is Nothing Then Return

        If chkVerHistorial.Checked Then
            ' MODO HISTORIAL: Los datos son de un tipo anónimo
            Select Case colName
                Case "TipoEstado" : e.Value = dataItem.GetType().GetProperty("TipoEstado")?.GetValue(dataItem, Nothing)
                Case "Observaciones" : e.Value = dataItem.GetType().GetProperty("Observaciones")?.GetValue(dataItem, Nothing)
                Case "FechaDesde"
                    Dim fechaDesdeObj = dataItem.GetType().GetProperty("FechaDesde")?.GetValue(dataItem, Nothing)
                    If fechaDesdeObj IsNot Nothing AndAlso Not DBNull.Value.Equals(fechaDesdeObj) Then
                        e.Value = CDate(fechaDesdeObj).ToShortDateString()
                    End If
                Case "FechaHasta"
                    Dim fechaHastaObj = dataItem.GetType().GetProperty("FechaHasta")?.GetValue(dataItem, Nothing)
                    If fechaHastaObj IsNot Nothing AndAlso Not DBNull.Value.Equals(fechaHastaObj) Then
                        e.Value = CDate(fechaHastaObj).ToShortDateString()
                    End If
            End Select
        Else
            ' MODO EDICIÓN: Los datos son del tipo EstadoTransitorio
            Dim estado = TryCast(dataItem, EstadoTransitorio)
            If estado Is Nothing Then Return

            ' Si el Id del tipo no está seteado (fila nueva), no hacer nada
            If estado.TipoEstadoTransitorioId = 0 Then
                e.FormattingApplied = True
                Return
            End If

            If colName = "TipoEstado" Then
                Dim tipo = _tiposEstadoTransitorio.FirstOrDefault(Function(t) t.Id = estado.TipoEstadoTransitorioId)
                e.Value = If(tipo IsNot Nothing, tipo.Nombre, "")
            Else
                Dim fechaDesde, fechaHasta, observaciones As String
                fechaDesde = "" : fechaHasta = "" : observaciones = ""

                Select Case estado.TipoEstadoTransitorioId
                    Case 1 ' Designación
                        If estado.DesignacionDetalle IsNot Nothing Then
                            fechaDesde = estado.DesignacionDetalle.FechaDesde.ToShortDateString()
                            fechaHasta = If(estado.DesignacionDetalle.FechaHasta.HasValue, estado.DesignacionDetalle.FechaHasta.Value.ToShortDateString(), "")
                            observaciones = estado.DesignacionDetalle.Observaciones
                        End If
                    Case 2 ' Enfermedad
                        If estado.EnfermedadDetalle IsNot Nothing Then
                            fechaDesde = estado.EnfermedadDetalle.FechaDesde.ToShortDateString()
                            fechaHasta = If(estado.EnfermedadDetalle.FechaHasta.HasValue, estado.EnfermedadDetalle.FechaHasta.Value.ToShortDateString(), "")
                            observaciones = estado.EnfermedadDetalle.Observaciones & " (" & estado.EnfermedadDetalle.Diagnostico & ")"
                        End If
                    Case 3 ' Sanción
                        If estado.SancionDetalle IsNot Nothing Then
                            fechaDesde = estado.SancionDetalle.FechaDesde.ToShortDateString()
                            fechaHasta = If(estado.SancionDetalle.FechaHasta.HasValue, estado.SancionDetalle.FechaHasta.Value.ToShortDateString(), "")
                            observaciones = estado.SancionDetalle.Observaciones
                        End If
                    Case 5 ' Retén
                        If estado.RetenDetalle IsNot Nothing Then
                            fechaDesde = estado.RetenDetalle.FechaReten.ToShortDateString()
                            observaciones = estado.RetenDetalle.Observaciones
                        End If
                End Select

                Select Case colName
                    Case "FechaDesde" : e.Value = fechaDesde
                    Case "FechaHasta" : e.Value = fechaHasta
                    Case "Observaciones" : e.Value = observaciones
                End Select
            End If
        End If

        e.FormattingApplied = True
        ' --- FIN DE LA CORRECCIÓN ---
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    Private Sub btnSeleccionarFoto_Click(sender As Object, e As EventArgs) Handles btnSeleccionarFoto.Click
        Using ofd As New OpenFileDialog() With {.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp"}
            If ofd.ShowDialog() = DialogResult.OK Then
                _rutaFotoSeleccionada = ofd.FileName
                pbFoto.Image = Image.FromFile(ofd.FileName)
            End If
        End Using
    End Sub
#End Region

#Region "CRUD Dotación"
    Private Sub btnAñadirDotacion_Click(sender As Object, e As EventArgs) Handles btnAñadirDotacion.Click
        Dim nuevaDotacion = New FuncionarioDotacion()
        Using frm As New frmFuncionarioDotacion(nuevaDotacion)
            If frm.ShowDialog() = DialogResult.OK Then
                _dotaciones.Add(frm.Dotacion)
            End If
        End Using
    End Sub

    Private Sub btnEditarDotacion_Click(sender As Object, e As EventArgs) Handles btnEditarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        Dim dotacionSeleccionada = CType(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
        Using frm As New frmFuncionarioDotacion(dotacionSeleccionada)
            If frm.ShowDialog() = DialogResult.OK Then
                _dotaciones.ResetBindings()
            End If
        End Using
    End Sub

    Private Sub btnQuitarDotacion_Click(sender As Object, e As EventArgs) Handles btnQuitarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        If MessageBox.Show("¿Está seguro de que desea quitar este elemento de dotación?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim dotacionSeleccionada = CType(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
            _dotaciones.Remove(dotacionSeleccionada)
        End If
    End Sub
#End Region

#Region "CRUD Estados Transitorios"
    Private Sub btnAñadirEstado_Click(sender As Object, e As EventArgs) Handles btnAñadirEstado.Click
        Dim nuevoEstado = New EstadoTransitorio()
        Using frm As New frmFuncionarioEstadoTransitorio(nuevoEstado, _tiposEstadoTransitorio)
            If frm.ShowDialog() = DialogResult.OK Then
                _estadosTransitorios.Add(frm.Estado)
            End If
        End Using
    End Sub

    Private Sub btnEditarEstado_Click(sender As Object, e As EventArgs) Handles btnEditarEstado.Click
        If dgvEstadosTransitorios.CurrentRow Is Nothing Then Return

        Dim estadoParaEditar As EstadoTransitorio = Nothing

        If chkVerHistorial.Checked Then
            Dim itemSeleccionado = dgvEstadosTransitorios.CurrentRow.DataBoundItem
            Dim id = CInt(itemSeleccionado.GetType().GetProperty("Id").GetValue(itemSeleccionado, Nothing))
            estadoParaEditar = _estadosTransitorios.FirstOrDefault(Function(et) et.Id = id)
        Else
            estadoParaEditar = TryCast(dgvEstadosTransitorios.CurrentRow.DataBoundItem, EstadoTransitorio)
        End If

        If estadoParaEditar Is Nothing Then
            MessageBox.Show("El registro seleccionado no es un estado transitorio editable.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Using frm As New frmFuncionarioEstadoTransitorio(estadoParaEditar, _tiposEstadoTransitorio)
            If frm.ShowDialog() = DialogResult.OK Then
                _estadosTransitorios.ResetBindings()
            End If
        End Using
    End Sub

    Private Sub btnQuitarEstado_Click(sender As Object, e As EventArgs) Handles btnQuitarEstado.Click
        If dgvEstadosTransitorios.CurrentRow Is Nothing Then Return

        Dim estadoParaQuitar = TryCast(dgvEstadosTransitorios.CurrentRow.DataBoundItem, EstadoTransitorio)
        If estadoParaQuitar Is Nothing Then
            If chkVerHistorial.Checked Then
                Dim itemSeleccionado = dgvEstadosTransitorios.CurrentRow.DataBoundItem
                Dim id = CInt(itemSeleccionado.GetType().GetProperty("Id").GetValue(itemSeleccionado, Nothing))
                estadoParaQuitar = _estadosTransitorios.FirstOrDefault(Function(et) et.Id = id)
            End If
        End If

        If estadoParaQuitar Is Nothing Then
            MessageBox.Show("El registro seleccionado no es un estado transitorio eliminable.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If


        If MessageBox.Show("¿Está seguro de que desea quitar este estado transitorio?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            _estadosTransitorios.Remove(estadoParaQuitar)
        End If
    End Sub
#End Region

End Class