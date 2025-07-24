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

    ' --- Colecciones para manejar los cambios en Dotación y Observaciones ---
    Private _dotaciones As BindingList(Of FuncionarioDotacion)
    Private _observaciones As BindingList(Of FuncionarioObservacion)
    Private _dotacionesEliminadas As New List(Of FuncionarioDotacion)
    Private _observacionesEliminadas As New List(Of FuncionarioObservacion)

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

        ' --- Cargar DataGridViews de Dotación y Observaciones ---
        _dotaciones = New BindingList(Of FuncionarioDotacion)(f.FuncionarioDotacion.ToList())
        _observaciones = New BindingList(Of FuncionarioObservacion)(f.FuncionarioObservacion.ToList())
        dgvDotacion.DataSource = _dotaciones
        dgvObservaciones.DataSource = _observaciones
    End Function

    '------------------- Guardar / Actualizar ----------------
    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            ' --- Validaciones básicas ---
            If String.IsNullOrWhiteSpace(txtCI.Text) Then
                MessageBox.Show("El CI es obligatorio.")
                Return
            End If
            If String.IsNullOrWhiteSpace(txtNombre.Text) Then
                MessageBox.Show("El nombre es obligatorio.")
                Return
            End If
            If cboTipoFuncionario.SelectedIndex = -1 Then
                MessageBox.Show("Debe seleccionar un tipo de funcionario.")
                Return
            End If
            If dtpFechaIngreso.Value > DateTime.Now Then
                MessageBox.Show("La fecha de ingreso no puede ser futura.")
                Return
            End If

            ' --- Preparar el objeto Funcionario ---
            Dim f As Funcionario
            If _modo = ModoFormulario.Crear Then
                f = New Funcionario()
                f.CreatedAt = DateTime.Now
            Else
                f = Await _svc.GetByIdCompletoAsync(_idFuncionario)
                If f Is Nothing Then
                    MessageBox.Show("Funcionario no encontrado.")
                    Return
                End If
                f.UpdatedAt = DateTime.Now
            End If

            ' --- Mapear datos desde el formulario al objeto ---
            f.CI = txtCI.Text.Trim()
            f.Nombre = txtNombre.Text.Trim()
            f.FechaIngreso = dtpFechaIngreso.Value.Date
            f.TipoFuncionarioId = CInt(cboTipoFuncionario.SelectedValue)
            f.CargoId = If(cboCargo.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboCargo.SelectedValue))
            f.Activo = chkActivo.Checked
            f.EscalafonId = If(cboEscalafon.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEscalafon.SelectedValue))
            f.FuncionId = If(cboFuncion.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboFuncion.SelectedValue))
            f.FechaNacimiento = If(dtpFechaNacimiento.Value = dtpFechaNacimiento.MinDate, CType(Nothing, Date?), dtpFechaNacimiento.Value.Date)
            f.Domicilio = txtDomicilio.Text.Trim()
            f.Email = txtEmail.Text.Trim()
            f.EstadoCivilId = If(cboEstadoCivil.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEstadoCivil.SelectedValue))
            f.GeneroId = If(cboGenero.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboGenero.SelectedValue))
            f.NivelEstudioId = If(cboNivelEstudio.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboNivelEstudio.SelectedValue))

            ' Foto
            If Not String.IsNullOrWhiteSpace(_rutaFotoSeleccionada) Then
                f.Foto = File.ReadAllBytes(_rutaFotoSeleccionada)
            End If

            ' --- Actualizar colecciones ---
            f.FuncionarioDotacion = _dotaciones.ToList()
            f.FuncionarioObservacion = _observaciones.ToList()

            ' --- Guardar en la base de datos ---
            If _modo = ModoFormulario.Crear Then
                Await _svc.CreateAsync(f)
                MessageBox.Show("Funcionario creado correctamente.")
            Else
                Using uow As New UnitOfWork()
                    ' Eliminar registros marcados
                    If _dotacionesEliminadas.Any() Then
                        uow.Repository(Of FuncionarioDotacion).RemoveRange(_dotacionesEliminadas)
                    End If
                    If _observacionesEliminadas.Any() Then
                        uow.Repository(Of FuncionarioObservacion).RemoveRange(_observacionesEliminadas)
                    End If
                    Await uow.CommitAsync()
                End Using
                Await _svc.UpdateAsync(f)
                MessageBox.Show("Funcionario actualizado correctamente.")
            End If

            DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
            frm.ShowDialog()
            _dotaciones.ResetBindings() ' Refresca la grilla
        End Using
    End Sub

    Private Sub btnQuitarDotacion_Click(sender As Object, e As EventArgs) Handles btnQuitarDotacion.Click
        If dgvDotacion.CurrentRow Is Nothing Then Return
        If MessageBox.Show("¿Está seguro de que desea quitar este elemento de dotación?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim dotacionSeleccionada = CType(dgvDotacion.CurrentRow.DataBoundItem, FuncionarioDotacion)
            If dotacionSeleccionada.Id > 0 Then
                _dotacionesEliminadas.Add(dotacionSeleccionada)
            End If
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
            frm.ShowDialog()
            _observaciones.ResetBindings() ' Refresca la grilla
        End Using
    End Sub

    Private Sub btnQuitarObservacion_Click(sender As Object, e As EventArgs) Handles btnQuitarObservacion.Click
        If dgvObservaciones.CurrentRow Is Nothing Then Return
        If MessageBox.Show("¿Está seguro de que desea quitar esta observación?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim observacionSeleccionada = CType(dgvObservaciones.CurrentRow.DataBoundItem, FuncionarioObservacion)
            If observacionSeleccionada.Id > 0 Then
                _observacionesEliminadas.Add(observacionSeleccionada)
            End If
            _observaciones.Remove(observacionSeleccionada)
        End If
    End Sub
#End Region

End Class