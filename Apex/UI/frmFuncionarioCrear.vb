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
    Public Property IdFuncionario As Integer
        Get
            Return _idFuncionario
        End Get
        Set(value As Integer)
            _idFuncionario = value
        End Set
    End Property
    Private _fotoOriginal As Byte()
    Public Property FotoOriginal As Byte()
        Get
            Return _fotoOriginal
        End Get
        Set(value As Byte())
            _fotoOriginal = value
        End Set
    End Property

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
            Await CargarDatosAsync()
            Me.Text = "Editar funcionario"
            btnGuardar.Text = "Actualizar"
        Else
            Me.Text = "Nuevo funcionario"
            btnGuardar.Text = "Guardar"
        End If
    End Sub

    '----------------- Combos Look‑ups -----------------------
    Private Async Function CargarCombosAsync() As Task
        cboTipoFuncionario.DataSource = Await _svc.ObtenerTiposFuncionarioAsync()
        cboTipoFuncionario.DisplayMember = "Value"
        cboTipoFuncionario.ValueMember = "Key"

        cboCargo.DataSource = Await _svc.ObtenerCargosAsync()
        cboCargo.DisplayMember = "Value"
        cboCargo.ValueMember = "Key"
        cboCargo.SelectedIndex = -1
    End Function

    '------------------ Cargar datos (edición) ---------------
    Private Async Function CargarDatosAsync() As Task
        Dim f = Await _svc.GetByIdAsync(_idFuncionario)
        If f Is Nothing Then
            MessageBox.Show("No se encontró el registro.")
            Close()
            Return
        End If
        txtCI.Text = f.CI
        txtNombre.Text = f.Nombre
        dtpFechaIngreso.Value = f.FechaIngreso
        cboTipoFuncionario.SelectedValue = f.TipoFuncionarioId
        cboCargo.SelectedValue = If(f.CargoId.HasValue, CInt(f.CargoId), -1)
        chkActivo.Checked = f.Activo

        _fotoOriginal = f.Foto
        If _fotoOriginal IsNot Nothing AndAlso _fotoOriginal.Length > 0 Then
            pbFoto.Image = New Bitmap(New MemoryStream(_fotoOriginal))
        End If
    End Function

    '------------------- Guardar / Actualizar ----------------
    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            ' Validaciones básicas
            Dim bytesFoto As Byte() = If(String.IsNullOrWhiteSpace(txtRutaFoto.Text),
                                          _fotoOriginal,
                                          File.ReadAllBytes(txtRutaFoto.Text))

            Dim f As Funcionario

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


            If _modo = ModoFormulario.Crear Then
                ' En modo creación, inicializamos un nuevo funcionario
                f = New Funcionario()
                f.CreatedAt = DateTime.Now
            Else
                ' En modo edición, obtenemos el funcionario existente
                f = Await _svc.GetByIdAsync(_idFuncionario)
                If f Is Nothing Then
                    MessageBox.Show("Funcionario no encontrado.")
                    Return
                End If

                f.UpdatedAt = DateTime.Now
            End If

            f.CI = txtCI.Text.Trim()
            f.Nombre = txtNombre.Text.Trim()
            f.FechaIngreso = dtpFechaIngreso.Value.Date
            f.TipoFuncionarioId = CInt(cboTipoFuncionario.SelectedValue)
            f.CargoId = If(cboCargo.SelectedIndex = -1, CType(Nothing, Integer?), CInt(cboCargo.SelectedValue))
            f.Foto = bytesFoto
            f.Activo = chkActivo.Checked


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
                txtRutaFoto.Text = ofd.FileName
                pbFoto.Image = Image.FromFile(ofd.FileName)

            End If
        End Using
    End Sub

End Class
