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

    ' --- INICIO DE LA CORRECCIÓN ---
    Private _itemsDotacion As List(Of DotacionItem) ' Almacenará los tipos de dotación.
    ' --- FIN DE LA CORRECCIÓN ---


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
    End Sub

    '------------------- Carga del Formulario --------------------------
    Private Async Sub frmFuncionarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New FuncionarioService(_uow)
        Await CargarCombosAsync()

        _tiposEstadoTransitorio = Await _svc.ObtenerTiposEstadoTransitorioCompletosAsync()

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Cargamos la lista de dotaciones para usarla en el formateo de la grilla.
        _itemsDotacion = Await _svc.ObtenerItemsDotacionCompletosAsync()
        ' --- FIN DE LA CORRECCIÓN ---

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
            Dim repo = _uow.Repository(Of vw_FuncionarioEstadosConsolidados)()
            Dim historial = Await repo.GetAll().
                                    Where(Function(h) h.FuncionarioId = _idFuncionario).
                                    OrderByDescending(Function(h) h.FechaDesde).
                                    ToListAsync()

            _historialConsolidado = historial.Select(Function(h) New With {
                .Id = h.Id,
                .Origen = h.Origen,
                .TipoEstado = h.TipoEstadoNombre,
                .FechaDesde = h.FechaDesde,
                .FechaHasta = h.FechaHasta,
                .Observaciones = h.Observaciones
            }).Cast(Of Object).ToList()

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
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoEstado", .DataPropertyName = "TipoEstado", .HeaderText = "Tipo de Estado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "FechaDesde", .DataPropertyName = "FechaDesde", .HeaderText = "Desde", .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "FechaHasta", .DataPropertyName = "FechaHasta", .HeaderText = "Hasta", .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Observaciones", .DataPropertyName = "Observaciones", .HeaderText = "Observaciones", .Width = 300})
        End With
    End Sub

    Private Sub dgvDotacion_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        Dim dgv = CType(sender, DataGridView)
        Dim colName = dgv.Columns(e.ColumnIndex).Name

        If colName = "colItem" Then
            Dim dotacion = TryCast(dgv.Rows(e.RowIndex).DataBoundItem, FuncionarioDotacion)
            If dotacion IsNot Nothing Then
                ' --- INICIO DE LA CORRECCIÓN ---
                If dotacion.DotacionItem IsNot Nothing Then
                    ' Para items existentes, usamos la propiedad de navegación.
                    e.Value = dotacion.DotacionItem.Nombre
                ElseIf dotacion.DotacionItemId > 0 AndAlso _itemsDotacion IsNot Nothing Then
                    ' Para items NUEVOS, buscamos el nombre en la lista en memoria.
                    Dim item = _itemsDotacion.FirstOrDefault(Function(i) i.Id = dotacion.DotacionItemId)
                    If item IsNot Nothing Then
                        e.Value = item.Nombre
                    End If
                End If
                e.FormattingApplied = True
                ' --- FIN DE LA CORRECCIÓN ---
            End If
        End If
    End Sub


    Private Sub dgvEstadosTransitorios_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return
        Dim dgv = CType(sender, DataGridView)
        Dim colName = dgv.Columns(e.ColumnIndex).Name

        If chkVerHistorial.Checked Then
            ' No se necesita formateo especial, los datos vienen de la vista
        Else
            ' Formateo para el modo edición (entidad EstadoTransitorio)
            Dim estado = TryCast(dgv.Rows(e.RowIndex).DataBoundItem, EstadoTransitorio)
            If estado Is Nothing Then Return

            If colName = "TipoEstado" Then
                If estado.TipoEstadoTransitorio IsNot Nothing Then
                    e.Value = estado.TipoEstadoTransitorio.Nombre
                ElseIf estado.TipoEstadoTransitorioId > 0 AndAlso _tiposEstadoTransitorio IsNot Nothing Then
                    Dim tipo = _tiposEstadoTransitorio.FirstOrDefault(Function(t) t.Id = estado.TipoEstadoTransitorioId)
                    If tipo IsNot Nothing Then
                        e.Value = tipo.Nombre
                    End If
                End If
                e.FormattingApplied = True
            End If
        End If

        If colName = "FechaHasta" Then
            If e.Value Is Nothing OrElse (TypeOf e.Value Is Date AndAlso CDate(e.Value) = Date.MinValue) Then
                e.Value = String.Empty
                e.FormattingApplied = True
            End If
        End If
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
                _estadosTransitorios.Add(nuevoEstado)
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

        Dim estadoParaQuitar As EstadoTransitorio = Nothing

        If chkVerHistorial.Checked Then
            Dim itemSeleccionado = dgvEstadosTransitorios.CurrentRow.DataBoundItem
            Dim id = CInt(itemSeleccionado.GetType().GetProperty("Id").GetValue(itemSeleccionado, Nothing))
            estadoParaQuitar = _estadosTransitorios.FirstOrDefault(Function(et) et.Id = id)
        Else
            estadoParaQuitar = TryCast(dgvEstadosTransitorios.CurrentRow.DataBoundItem, EstadoTransitorio)
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