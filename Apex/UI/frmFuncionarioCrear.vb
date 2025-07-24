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
    Private _observaciones As BindingList(Of FuncionarioObservacion)

    '-------------------- Constructores ----------------------
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _dotaciones = New BindingList(Of FuncionarioDotacion)()
        _observaciones = New BindingList(Of FuncionarioObservacion)()
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

        ' --- Configurar DataGridViews ---
        dgvDotacion.DataSource = _dotaciones
        dgvObservaciones.DataSource = _observaciones

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

    '----------------- Combos Look-ups -----------------------
    Private Async Function CargarCombosAsync() As Task
        ' Pestaña General
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

        ' Asignar -1 para que no haya selección inicial
        cboCargo.SelectedIndex = -1
        cboEscalafon.SelectedIndex = -1
        cboFuncion.SelectedIndex = -1
        cboEstadoCivil.SelectedIndex = -1
        cboGenero.SelectedIndex = -1
        cboNivelEstudio.SelectedIndex = -1
    End Function

    '------------------ Cargar datos (edición) ---------------
    Private Async Function CargarDatosAsync() As Task
        Dim f = Await _svc.GetByIdCompletoAsync(_idFuncionario)

        If f Is Nothing Then
            MessageBox.Show("No se encontró el registro del funcionario.")
            Close()
            Return
        End If

        ' --- Cargar Pestaña General ---
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

        ' --- Cargar Pestaña Datos Personales ---
        dtpFechaNacimiento.Value = If(f.FechaNacimiento.HasValue, f.FechaNacimiento.Value, dtpFechaNacimiento.MinDate)
        txtDomicilio.Text = f.Domicilio
        txtEmail.Text = f.Email
        cboEstadoCivil.SelectedValue = If(f.EstadoCivilId.HasValue, CInt(f.EstadoCivilId), -1)
        cboGenero.SelectedValue = If(f.GeneroId.HasValue, CInt(f.GeneroId), -1)
        cboNivelEstudio.SelectedValue = If(f.NivelEstudioId.HasValue, CInt(f.NivelEstudioId), -1)

        ' --- Cargar DataGridViews ---
        _dotaciones = New BindingList(Of FuncionarioDotacion)(f.FuncionarioDotacion.ToList())
        _observaciones = New BindingList(Of FuncionarioObservacion)(f.FuncionarioObservacion.ToList())
        dgvDotacion.DataSource = _dotaciones
        dgvObservaciones.DataSource = _observaciones
    End Function

    '------------------- Guardar / Actualizar ----------------
    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            ' --- Validaciones básicas ---
            If String.IsNullOrWhiteSpace(txtCI.Text) OrElse
           String.IsNullOrWhiteSpace(txtNombre.Text) OrElse
           cboTipoFuncionario.SelectedIndex = -1 Then
                MessageBox.Show("Los campos CI, Nombre y Tipo de Funcionario son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
            If dtpFechaIngreso.Value > DateTime.Now Then
                MessageBox.Show("La fecha de ingreso no puede ser futura.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using uow As New UnitOfWork()
                Dim funcionario As Funcionario

                ' --- Lógica para CREAR o CARGAR el funcionario ---
                If _modo = ModoFormulario.Crear Then
                    funcionario = New Funcionario()
                    funcionario.CreatedAt = DateTime.Now
                    uow.Repository(Of Funcionario).Add(funcionario)
                Else
                    ' Se carga la entidad CON TRACKING para poder actualizarla
                    ' ********* CAMBIO CLAVE AQUÍ *********
                    funcionario = Await uow.Context.Set(Of Funcionario)() _
                    .Include(Function(f) f.FuncionarioDotacion) _
                    .Include(Function(f) f.FuncionarioObservacion) _
                    .FirstOrDefaultAsync(Function(f) f.Id = _idFuncionario)

                    If funcionario Is Nothing Then
                        MessageBox.Show("No se pudo encontrar el funcionario para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    funcionario.UpdatedAt = DateTime.Now
                End If

                ' --- Mapear datos escalares del formulario al objeto ---
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
                SincronizarColeccion(funcionario.FuncionarioDotacion, _dotaciones, uow.Context)
                SincronizarColeccion(funcionario.FuncionarioObservacion, _observaciones, uow.Context)

                ' --- Guardar todos los cambios en una sola transacción ---
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
        ' 1. Eliminar los que ya no están en la lista del formulario
        Dim itemsParaBorrar = dbCollection.Where(Function(dbItem)
                                                     Dim dbId = CInt(dbItem.GetType().GetProperty("Id").GetValue(dbItem))
                                                     Return dbId > 0 AndAlso Not formCollection.Any(Function(formItem) CInt(formItem.GetType().GetProperty("Id").GetValue(formItem)) = dbId)
                                                 End Function).ToList()

        For Each item In itemsParaBorrar
            ctx.Entry(item).State = EntityState.Deleted
        Next

        ' 2. Actualizar los existentes y agregar los nuevos
        For Each itemForm In formCollection
            Dim id = CInt(itemForm.GetType().GetProperty("Id").GetValue(itemForm))
            If id = 0 Then ' Nuevo
                dbCollection.Add(itemForm)
            Else ' Existente
                Dim itemInDb = dbCollection.FirstOrDefault(Function(x) CInt(x.GetType().GetProperty("Id").GetValue(x)) = id)
                If itemInDb IsNot Nothing Then
                    ' Copia los valores del objeto del formulario al objeto que está siendo rastreado por el contexto
                    ctx.Entry(itemInDb).CurrentValues.SetValues(itemForm)
                End If
            End If
        Next
    End Sub


    '-------------------- Cancelar ---------------------------
    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

    '------------------ Seleccionar Foto ---------------------
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
                _dotaciones.Add(frm.Dotacion)
            End If
        End Using
    End Sub

    Private Sub btnEditarDotacion_Click(sender As Object, e As EventArgs) Handles btnEditarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        Dim dotacionSeleccionada = CType(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
        Using frm As New frmFuncionarioDotacion(dotacionSeleccionada)
            If frm.ShowDialog() = DialogResult.OK Then
                _dotaciones.ResetBindings() ' Refresca la grilla
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

#Region "CRUD Observaciones"
    Private Sub btnAñadirObservacion_Click(sender As Object, e As EventArgs) Handles btnAñadirObservacion.Click
        Dim nuevaObservacion = New FuncionarioObservacion()
        Using frm As New frmFuncionarioObservacion(nuevaObservacion)
            If frm.ShowDialog() = DialogResult.OK Then
                _observaciones.Add(frm.Observacion)
            End If
        End Using
    End Sub

    Private Sub btnEditarObservacion_Click(sender As Object, e As EventArgs) Handles btnEditarObservacion.Click
        If dgvObservaciones.CurrentRow Is Nothing Then Return
        Dim observacionSeleccionada = CType(dgvObservaciones.CurrentRow.DataBoundItem, FuncionarioObservacion)
        Using frm As New frmFuncionarioObservacion(observacionSeleccionada)
            If frm.ShowDialog() = DialogResult.OK Then
                _observaciones.ResetBindings() ' Refresca la grilla
            End If
        End Using
    End Sub

    Private Sub btnQuitarObservacion_Click(sender As Object, e As EventArgs) Handles btnQuitarObservacion.Click
        If dgvObservaciones.CurrentRow Is Nothing Then Return
        If MessageBox.Show("¿Está seguro de que desea quitar esta observación?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim observacionSeleccionada = CType(dgvObservaciones.CurrentRow.DataBoundItem, FuncionarioObservacion)
            _observaciones.Remove(observacionSeleccionada)
        End If
    End Sub
#End Region

End Class