Option Strict On
Option Explicit On
Imports System.IO
Imports System.Data.Entity

Public Enum ModoFormulario
    ' Define los modos del formulario: Crear o Editar
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

    ' Nueva variable para reemplazar el TextBox txtRutaFoto
    Private _rutaFotoSeleccionada As String

    '-------------------- Constructores ----------------------

    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
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

        ' cboEscalafon.DataSource = Await _svc.ObtenerEscalafonesAsync() ' <-- Deberás crear este método en tu servicio
        ' cboEscalafon.DisplayMember = "Value"
        ' cboEscalafon.ValueMember = "Key"
        '
        ' cboFuncion.DataSource = Await _svc.ObtenerFuncionesAsync() ' <-- Deberás crear este método en tu servicio
        ' cboFuncion.DisplayMember = "Value"
        ' cboFuncion.ValueMember = "Key"

        ' Pestaña Datos Personales
        ' cboEstadoCivil.DataSource = Await _svc.ObtenerEstadosCivilesAsync() ' <-- Deberás crear este método en tu servicio
        ' cboEstadoCivil.DisplayMember = "Value"
        ' cboEstadoCivil.ValueMember = "Key"
        '
        ' cboGenero.DataSource = Await _svc.ObtenerGenerosAsync() ' <-- Deberás crear este método en tu servicio
        ' cboGenero.DisplayMember = "Value"
        ' cboGenero.ValueMember = "Key"
        '
        ' cboNivelEstudio.DataSource = Await _svc.ObtenerNivelesEstudioAsync() ' <-- Deberás crear este método en tu servicio
        ' cboNivelEstudio.DisplayMember = "Value"
        ' cboNivelEstudio.ValueMember = "Key"

        ' Asignar -1 para que no haya selección inicial
        cboCargo.SelectedIndex = -1
    End Function

    '------------------ Cargar datos (edición) ---------------

    Private Async Function CargarDatosAsync() As Task
        ' NOTA: Deberás crear un método en tu servicio que incluya las relaciones (Include)
        Dim f = Await _svc.GetByIdAsync(_idFuncionario) ' Usamos el método simple por ahora

        If f Is Nothing Then
            MessageBox.Show("No se encontró el registro.")
            Close()
            Return
        End If

        ' --- Cargar Pestaña General ---
        txtCI.Text = f.CI
        txtNombre.Text = f.Nombre
        dtpFechaIngreso.Value = f.FechaIngreso
        cboTipoFuncionario.SelectedValue = f.TipoFuncionarioId
        cboCargo.SelectedValue = If(f.CargoId.HasValue, CInt(f.CargoId), -1)
        chkActivo.Checked = f.Activo
        ' cboEscalafon.SelectedValue = If(f.EscalafonId.HasValue, CInt(f.EscalafonId), -1)
        ' cboFuncion.SelectedValue = If(f.FuncionId.HasValue, CInt(f.FuncionId), -1)

        _fotoOriginal = f.Foto
        If _fotoOriginal IsNot Nothing AndAlso _fotoOriginal.Length > 0 Then
            pbFoto.Image = New Bitmap(New MemoryStream(_fotoOriginal))
        End If

        ' --- Cargar Pestaña Datos Personales ---
        ' dtpFechaNacimiento.Value = If(f.FechaNacimiento.HasValue, f.FechaNacimiento.Value, dtpFechaNacimiento.MinDate)
        ' txtDomicilio.Text = f.Domicilio
        ' txtEmail.Text = f.Email
        ' txtTelefono.Text = "" ' <-- Lógica pendiente para leer de FuncionarioDispositivo
        ' cboEstadoCivil.SelectedValue = If(f.EstadoCivilId.HasValue, CInt(f.EstadoCivilId), -1)
        ' cboGenero.SelectedValue = If(f.GeneroId.HasValue, CInt(f.GeneroId), -1)
        ' cboNivelEstudio.SelectedValue = If(f.NivelEstudioId.HasValue, CInt(f.NivelEstudioId), -1)

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
                f = Await _svc.GetByIdAsync(_idFuncionario)
                If f Is Nothing Then
                    MessageBox.Show("Funcionario no encontrado.")
                    Return
                End If
                f.UpdatedAt = DateTime.Now
            End If

            ' --- Mapear datos desde el formulario al objeto ---
            ' Pestaña General
            f.CI = txtCI.Text.Trim()
            f.Nombre = txtNombre.Text.Trim()
            f.FechaIngreso = dtpFechaIngreso.Value.Date
            f.TipoFuncionarioId = CInt(cboTipoFuncionario.SelectedValue)
            f.CargoId = If(cboCargo.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboCargo.SelectedValue))
            f.Activo = chkActivo.Checked
            ' f.EscalafonId = If(cboEscalafon.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEscalafon.SelectedValue))
            ' f.FuncionId = If(cboFuncion.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboFuncion.SelectedValue))


            ' Foto: usa la variable en lugar del textbox
            If Not String.IsNullOrWhiteSpace(_rutaFotoSeleccionada) Then
                f.Foto = File.ReadAllBytes(_rutaFotoSeleccionada)
            End If

            ' Pestaña Datos Personales
            ' f.FechaNacimiento = If(dtpFechaNacimiento.Value = dtpFechaNacimiento.MinDate, CType(Nothing, Date?), dtpFechaNacimiento.Value.Date)
            ' f.Domicilio = txtDomicilio.Text.Trim()
            ' f.Email = txtEmail.Text.Trim()
            ' f.EstadoCivilId = If(cboEstadoCivil.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboEstadoCivil.SelectedValue))
            ' f.GeneroId = If(cboGenero.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboGenero.SelectedValue))
            ' f.NivelEstudioId = If(cboNivelEstudio.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboNivelEstudio.SelectedValue))


            ' --- Guardar en la base de datos ---
            If _modo = ModoFormulario.Crear Then
                Await _svc.CreateAsync(f)
                MessageBox.Show("Funcionario creado correctamente.")
            Else
                Await _svc.UpdateAsync(f)
                MessageBox.Show("Funcionario actualizado correctamente.")
            End If

            DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
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
                ' Guarda la ruta en la variable, no en un textbox
                _rutaFotoSeleccionada = ofd.FileName
                pbFoto.Image = Image.FromFile(ofd.FileName)
            End If
        End Using
    End Sub

End Class