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

    '-------------------- Variables --------------------------
    Private _svc As FuncionarioService
    Private _modo As ModoFormulario
    Private _idFuncionario As Integer
    Private _fotoOriginal As Byte()
    Private _rutaFotoSeleccionada As String

    ' --- Colecciones para manejar los datos en el formulario ---
    Private _dotaciones As BindingList(Of FuncionarioDotacion)
    Private _estadosTransitorios As BindingList(Of EstadoTransitorio)
    Private _tiposEstadoTransitorio As List(Of TipoEstadoTransitorio)


    '-------------------- Constructores ----------------------
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _dotaciones = New BindingList(Of FuncionarioDotacion)()
        _estadosTransitorios = New BindingList(Of EstadoTransitorio)()
    End Sub

    Public Sub New(id As Integer)
        Me.New()
        _modo = ModoFormulario.Editar
        _idFuncionario = id
    End Sub

    '------------------- Load form --------------------------
    Private Async Sub frmFuncionarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New FuncionarioService()
        Await CargarCombosAsync()

        _tiposEstadoTransitorio = Await _svc.ObtenerTiposEstadoTransitorioCompletosAsync()

        ConfigurarGrillaDotacion()
        ConfigurarGrillaEstados()
        dgvDotacion.DataSource = _dotaciones
        dgvEstadosTransitorios.DataSource = _estadosTransitorios

        ' --- Añadir manejadores de eventos para formateo manual ---
        AddHandler dgvDotacion.CellFormatting, AddressOf dgvDotacion_CellFormatting
        AddHandler dgvEstadosTransitorios.CellFormatting, AddressOf dgvEstadosTransitorios_CellFormatting


        If _modo = ModoFormulario.Editar Then
            Me.Text = "Editar Funcionario"
            btnGuardar.Text = "Actualizar"
            Await CargarDatosAsync()
        Else
            Me.Text = "Nuevo Funcionario"
            btnGuardar.Text = "Guardar"
            pbFoto.Image = My.Resources.Police
        End If
    End Sub
    Private Sub ConfigurarGrillaDotacion()
        With dgvDotacion
            .AutoGenerateColumns = False
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .DataPropertyName = "Id",
                .Visible = False
            })

            ' ✅ CORRECCIÓN CLAVE 1:
            ' Ya no usamos DataPropertyName para el Ítem.
            ' Le damos un nombre único para identificar la columna en el evento CellFormatting.
            .Columns.Add(New DataGridViewTextBoxColumn With {
               .Name = "colItem",
               .HeaderText = "Ítem",
               .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
           })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .DataPropertyName = "Talla",
                .HeaderText = "Talla"
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .DataPropertyName = "Observaciones",
                .HeaderText = "Observaciones",
                .Width = 200
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                 .DataPropertyName = "FechaAsign",
                .HeaderText = "Fecha Asignación",
                .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}
            })
        End With
    End Sub

    Private Sub ConfigurarGrillaEstados()
        With dgvEstadosTransitorios
            .AutoGenerateColumns = False
            .Columns.Clear()
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "TipoEstado",
                .HeaderText = "Tipo de Estado",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "FechaDesde",
                .DataPropertyName = "FechaDesde",
                .HeaderText = "Desde",
                .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "FechaHasta",
                .DataPropertyName = "FechaHasta",
                .HeaderText = "Hasta",
                .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Observaciones",
                .DataPropertyName = "Observaciones",
                .HeaderText = "Observaciones",
                .Width = 300
            })
        End With
    End Sub

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

    Private Async Function CargarDatosAsync() As Task
        Dim f = Await _svc.GetByIdCompletoAsync(_idFuncionario)
        If f Is Nothing Then
            MessageBox.Show("No se encontró el registro del funcionario.")
            Close()
            Return
        End If
        txtCI.Text = f.CI
        txtNombre.Text = f.Nombre
        dtpFechaIngreso.Value = f.FechaIngreso
        cboTipoFuncionario.SelectedValue = CInt(f.TipoFuncionarioId)
        cboCargo.SelectedValue = If(f.CargoId.HasValue, CInt(f.CargoId), -1)
        chkActivo.Checked = f.Activo
        cboEscalafon.SelectedValue = If(f.EscalafonId.HasValue, CInt(f.EscalafonId), -1)
        cboFuncion.SelectedValue = If(f.FuncionId.HasValue, CInt(f.FuncionId), -1)
        _fotoOriginal = f.Foto
        If _fotoOriginal IsNot Nothing AndAlso _fotoOriginal.Length > 0 Then
            pbFoto.Image = New Bitmap(New MemoryStream(_fotoOriginal))
        Else
            pbFoto.Image = My.Resources.Police
        End If
        dtpFechaNacimiento.Value = If(f.FechaNacimiento.HasValue, f.FechaNacimiento.Value, dtpFechaNacimiento.MinDate)
        txtDomicilio.Text = f.Domicilio
        txtEmail.Text = f.Email
        cboEstadoCivil.SelectedValue = If(f.EstadoCivilId.HasValue, CInt(f.EstadoCivilId), -1)
        cboGenero.SelectedValue = If(f.GeneroId.HasValue, CInt(f.GeneroId), -1)
        cboNivelEstudio.SelectedValue = If(f.NivelEstudioId.HasValue, CInt(f.NivelEstudioId), -1)
        _dotaciones = New BindingList(Of FuncionarioDotacion)(f.FuncionarioDotacion.ToList())
        _estadosTransitorios = New BindingList(Of EstadoTransitorio)(f.EstadoTransitorio.ToList())
        dgvDotacion.DataSource = _dotaciones
        dgvEstadosTransitorios.DataSource = _estadosTransitorios
    End Function

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            If String.IsNullOrWhiteSpace(txtCI.Text) OrElse String.IsNullOrWhiteSpace(txtNombre.Text) OrElse cboTipoFuncionario.SelectedIndex = -1 Then
                MessageBox.Show("Los campos CI, Nombre y Tipo de Funcionario son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If dtpFechaIngreso.Value > DateTime.Now Then
                MessageBox.Show("La fecha de ingreso no puede ser futura.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            Using uow As New UnitOfWork()
                Dim funcionario As Funcionario
                If _modo = ModoFormulario.Crear Then
                    funcionario = New Funcionario()
                    funcionario.CreatedAt = DateTime.Now
                    uow.Repository(Of Funcionario).Add(funcionario)
                Else
                    funcionario = Await uow.Context.Set(Of Funcionario)().Include(Function(f) f.FuncionarioDotacion.Select(Function(fd) fd.DotacionItem)).Include(Function(f) f.EstadoTransitorio.Select(Function(et) et.TipoEstadoTransitorio)).FirstOrDefaultAsync(Function(f) f.Id = _idFuncionario)
                    If funcionario Is Nothing Then
                        MessageBox.Show("No se pudo encontrar el funcionario para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    funcionario.UpdatedAt = DateTime.Now
                End If
                funcionario.CI = txtCI.Text.Trim()
                funcionario.Nombre = txtNombre.Text.Trim()
                funcionario.FechaIngreso = dtpFechaIngreso.Value.Date
                funcionario.TipoFuncionarioId = CInt(cboTipoFuncionario.SelectedValue)
                funcionario.CargoId = If(cboCargo.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboCargo.SelectedValue))
                funcionario.Activo = chkActivo.Checked
                funcionario.EscalafonId = If(cboEscalafon.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEscalafon.SelectedValue))
                funcionario.FuncionId = If(cboFuncion.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboFuncion.SelectedValue))
                funcionario.FechaNacimiento = If(dtpFechaNacimiento.Value = dtpFechaNacimiento.MinDate, CType(Nothing, Date?), dtpFechaNacimiento.Value.Date)
                funcionario.Domicilio = txtDomicilio.Text.Trim()
                funcionario.Email = txtEmail.Text.Trim()
                funcionario.EstadoCivilId = If(cboEstadoCivil.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEstadoCivil.SelectedValue))
                funcionario.GeneroId = If(cboGenero.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboGenero.SelectedValue))
                funcionario.NivelEstudioId = If(cboNivelEstudio.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboNivelEstudio.SelectedValue))
                If Not String.IsNullOrWhiteSpace(_rutaFotoSeleccionada) Then
                    funcionario.Foto = File.ReadAllBytes(_rutaFotoSeleccionada)
                End If
                ' --- Sincronizar colecciones ---
                SincronizarDotaciones(funcionario.FuncionarioDotacion, _dotaciones, uow.Context) ' <-- LLAMADA AL NUEVO MÉTODO
                SincronizarColeccion(funcionario.EstadoTransitorio, _estadosTransitorios, uow.Context) ' <-- Esta se mantiene igual
                Await uow.CommitAsync()
                MessageBox.Show(If(_modo = ModoFormulario.Crear, "Funcionario creado", "Funcionario actualizado") & " correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SincronizarColeccion(Of T As {Class, New})(dbCollection As ICollection(Of T), formCollection As BindingList(Of T), ctx As DbContext)
        Dim itemsParaBorrar = dbCollection.Where(Function(dbItem)
                                                     Dim dbId = CInt(dbItem.GetType().GetProperty("Id").GetValue(dbItem))
                                                     Return dbId > 0 AndAlso Not formCollection.Any(Function(formItem) CInt(formItem.GetType().GetProperty("Id").GetValue(formItem)) = dbId)
                                                 End Function).ToList()
        For Each item In itemsParaBorrar
            ctx.Entry(item).State = EntityState.Deleted
        Next
        For Each itemForm In formCollection
            Dim id = CInt(itemForm.GetType().GetProperty("Id").GetValue(itemForm))
            If id = 0 Then
                If GetType(T) Is GetType(FuncionarioDotacion) Then
                    Dim dotacion = CType(CType(itemForm, Object), FuncionarioDotacion)
                    If dotacion.DotacionItem IsNot Nothing AndAlso dotacion.DotacionItem.Id > 0 Then
                        ctx.Entry(dotacion.DotacionItem).State = EntityState.Unchanged
                    End If
                End If
                If GetType(T) Is GetType(EstadoTransitorio) Then
                    Dim estado = CType(CType(itemForm, Object), EstadoTransitorio)
                    If estado.TipoEstadoTransitorio IsNot Nothing Then
                        ctx.Entry(estado.TipoEstadoTransitorio).State = EntityState.Unchanged
                    End If
                End If
                dbCollection.Add(itemForm)
            Else
                Dim itemInDb = dbCollection.FirstOrDefault(Function(x) CInt(x.GetType().GetProperty("Id").GetValue(x)) = id)
                If itemInDb IsNot Nothing Then
                    ctx.Entry(itemInDb).CurrentValues.SetValues(itemForm)
                End If
            End If
        Next
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

#Region "CRUD Dotación"
    Private Sub btnAñadirDotacion_Click(sender As Object, e As EventArgs) Handles btnAñadirDotacion.Click
        Dim nuevaDotacion = New FuncionarioDotacion()
        Using frm As New frmFuncionarioDotacion(nuevaDotacion)
            If frm.ShowDialog() = DialogResult.OK Then
                Dim existe = _dotaciones.Any(Function(d)
                                                 Return d.DotacionItemId = frm.Dotacion.DotacionItemId AndAlso
                                                        String.Equals(d.Talla, frm.Dotacion.Talla, StringComparison.OrdinalIgnoreCase) AndAlso
                                                        String.Equals(d.Observaciones, frm.Dotacion.Observaciones, StringComparison.OrdinalIgnoreCase)
                                             End Function)

                If existe Then
                    MessageBox.Show("Esta combinación de ítem, talla y observaciones ya ha sido añadida.", "Registro Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                frm.Dotacion.DotacionItem = frm.ItemSeleccionado
                _dotaciones.Add(frm.Dotacion)
                _dotaciones.ResetBindings()
            End If
        End Using
    End Sub

    Private Sub btnEditarDotacion_Click(sender As Object, e As EventArgs) Handles btnEditarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        Dim dotacionSeleccionada = CType(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
        Using frm As New frmFuncionarioDotacion(dotacionSeleccionada)
            If frm.ShowDialog() = DialogResult.OK Then
                dotacionSeleccionada.DotacionItem = frm.ItemSeleccionado
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
                Dim tipoSeleccionado = _tiposEstadoTransitorio.FirstOrDefault(Function(t) t.Id = nuevoEstado.TipoEstadoTransitorioId)
                If tipoSeleccionado IsNot Nothing Then
                    nuevoEstado.TipoEstadoTransitorio = tipoSeleccionado
                End If
                _estadosTransitorios.Add(nuevoEstado)
                _estadosTransitorios.ResetBindings()
            End If
        End Using
    End Sub

    Private Sub btnEditarEstado_Click(sender As Object, e As EventArgs) Handles btnEditarEstado.Click
        If dgvEstadosTransitorios.CurrentRow Is Nothing Then Return
        Dim estadoSeleccionado = CType(dgvEstadosTransitorios.CurrentRow.DataBoundItem, EstadoTransitorio)
        Using frm As New frmFuncionarioEstadoTransitorio(estadoSeleccionado, _tiposEstadoTransitorio)
            If frm.ShowDialog() = DialogResult.OK Then
                _estadosTransitorios.ResetBindings()
            End If
        End Using
    End Sub

    Private Sub btnQuitarEstado_Click(sender As Object, e As EventArgs) Handles btnQuitarEstado.Click
        If dgvEstadosTransitorios.CurrentRow Is Nothing Then Return
        If MessageBox.Show("¿Está seguro de que desea quitar este estado transitorio?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim estadoSeleccionado = CType(dgvEstadosTransitorios.CurrentRow.DataBoundItem, EstadoTransitorio)
            _estadosTransitorios.Remove(estadoSeleccionado)
        End If
    End Sub
#End Region

#Region "Formateo Manual de Grillas"

    ' ✅ CORRECCIÓN CLAVE 2: Este es el evento que llenará la celda del Ítem manualmente.
    Private Sub dgvDotacion_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return

        Dim dgv = CType(sender, DataGridView)
        Dim colName = dgv.Columns(e.ColumnIndex).Name

        If colName = "colItem" Then
            Dim dotacion = TryCast(dgv.Rows(e.RowIndex).DataBoundItem, FuncionarioDotacion)
            If dotacion IsNot Nothing AndAlso dotacion.DotacionItem IsNot Nothing Then
                e.Value = dotacion.DotacionItem.Nombre
                e.FormattingApplied = True
            End If
        End If
    End Sub

    Private Sub dgvEstadosTransitorios_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If e.RowIndex < 0 OrElse e.ColumnIndex < 0 Then Return

        Dim dgv = CType(sender, DataGridView)
        Dim colName = dgv.Columns(e.ColumnIndex).Name
        Dim estado = CType(dgv.Rows(e.RowIndex).DataBoundItem, EstadoTransitorio)
        If estado Is Nothing Then Return

        If colName = "TipoEstado" Then
            If estado.TipoEstadoTransitorio IsNot Nothing Then
                e.Value = estado.TipoEstadoTransitorio.Nombre
                e.FormattingApplied = True
            End If
        End If

        If colName = "FechaHasta" Then
            If e.Value Is Nothing Then
                e.Value = String.Empty
                e.FormattingApplied = True
            End If
        End If
    End Sub
    Private Sub SincronizarDotaciones(dbCollection As ICollection(Of FuncionarioDotacion), formCollection As BindingList(Of FuncionarioDotacion), ctx As DbContext)
        ' 1. Elementos para eliminar: Aquellos que están en la base de datos pero ya no en la lista del formulario.
        Dim itemsParaBorrar = dbCollection.Where(Function(dbItem) Not formCollection.Any(Function(formItem) formItem.Id = dbItem.Id)).ToList()
        For Each item In itemsParaBorrar
            ctx.Entry(item).State = EntityState.Deleted
        Next

        ' 2. Elementos para añadir o actualizar.
        For Each dotacionForm In formCollection
            If dotacionForm.Id = 0 Then
                ' AÑADIR: Es un registro nuevo.
                ' Nos aseguramos de que su "DotacionItem" se trate como una entidad existente.
                If dotacionForm.DotacionItem IsNot Nothing AndAlso dotacionForm.DotacionItem.Id > 0 Then
                    ctx.Entry(dotacionForm.DotacionItem).State = EntityState.Unchanged
                End If
                dbCollection.Add(dotacionForm)
            Else
                ' ACTUALIZAR: Es un registro existente.
                Dim dotacionEnDb = dbCollection.FirstOrDefault(Function(db) db.Id = dotacionForm.Id)
                If dotacionEnDb IsNot Nothing Then
                    ' Actualizamos manualmente las propiedades en lugar de usar SetValues para mayor control.
                    dotacionEnDb.DotacionItemId = dotacionForm.DotacionItemId
                    dotacionEnDb.Talla = dotacionForm.Talla
                    dotacionEnDb.Observaciones = dotacionForm.Observaciones
                    dotacionEnDb.FechaAsign = dotacionForm.FechaAsign
                End If
            End If
        Next
    End Sub
#End Region

End Class