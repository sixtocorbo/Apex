Imports System.Data.Entity
Imports System.Windows.Forms

''' <summary>
''' Formulario para crear o editar usuarios del sistema.
''' </summary>
Public Class frmUsuarioCrear
    ' Servicios utilizados para recuperar y persistir datos relacionados al usuario.
    Private ReadOnly _usuarioService As New UsuarioService()
    Private ReadOnly _funcionarioService As New FuncionarioService()

    ' Identificador del usuario que se está editando; Nothing indica modo creación.
    Private _id As Integer?


    Public Sub New(id As Integer)
        Me.New()
        _id = id
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub


    Private Async Sub frmUsuarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _id.HasValue Then
            ' Configura el formulario para editar un usuario existente.
            Me.Text = "Editar Usuario"
            Await CargarDatosParaEdicion()
        Else
            ' Configura el formulario para crear un nuevo usuario.
            Me.Text = "Crear Nuevo Usuario"
            Await CargarFuncionariosDisponibles()
        End If
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones básicas antes de intentar guardar.
        If cmbFuncionario.SelectedItem Is Nothing Then
            MessageBox.Show("Debe seleccionar un funcionario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If String.IsNullOrWhiteSpace(txtNombreUsuario.Text) Then
            MessageBox.Show("El nombre de usuario no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If Not _id.HasValue AndAlso String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("La contraseña no puede estar vacía.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If


        Try
            If _id.HasValue Then
                ' Actualiza el usuario existente con los datos del formulario.
                Dim usuarioExistente = Await _usuarioService.GetByIdAsync(_id.Value)
                If usuarioExistente IsNot Nothing Then
                    usuarioExistente.NombreUsuario = txtNombreUsuario.Text.Trim()
                    ' Actualiza la contraseña únicamente si se proporcionó un nuevo valor.
                    Dim nuevaPassword = If(String.IsNullOrWhiteSpace(txtPassword.Text), Nothing, txtPassword.Text)
                    Await _usuarioService.UpdateAsync(usuarioExistente, nuevaPassword)
                    MessageBox.Show("Usuario actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                ' Crea un nuevo usuario asociado al funcionario seleccionado.
                Dim funcionarioSeleccionado = CType(cmbFuncionario.SelectedItem, Funcionario)
                Dim nuevoUsuario As New Usuario With {
                    .NombreUsuario = txtNombreUsuario.Text.Trim(),
                    .NombreCompleto = funcionarioSeleccionado.Nombre,
                    .FuncionarioId = funcionarioSeleccionado.Id,
                    .Activo = True
                }
                Await _usuarioService.CreateAsync(nuevoUsuario, txtPassword.Text)
                MessageBox.Show("Usuario registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show($"Error al guardar el usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos del usuario a editar y bloquea la selección del funcionario asociado.
    ''' </summary>
    Private Async Function CargarDatosParaEdicion() As Task
        Try
            Dim usuario = Await _usuarioService.GetByIdAsync(_id.Value)
            If usuario Is Nothing Then
                MessageBox.Show("No se encontró el usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Me.Close()
                Return
            End If

            txtNombreUsuario.Text = usuario.NombreUsuario


            ' Carga únicamente el funcionario asociado al usuario en edición.
            Dim funcionarioActual = Await _funcionarioService.GetByIdAsync(usuario.FuncionarioId)
            If funcionarioActual IsNot Nothing Then
                cmbFuncionario.DataSource = New List(Of Funcionario) From {funcionarioActual}
                cmbFuncionario.DisplayMember = "Nombre"
                cmbFuncionario.ValueMember = "Id"
                cmbFuncionario.Enabled = False ' Evita cambiar el funcionario durante la edición.
            End If

        Catch ex As Exception
            MessageBox.Show($"Error al cargar los datos del usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    ''' <summary>
    ''' Carga el listado de funcionarios disponibles para vincular a un nuevo usuario.
    ''' </summary>
    Private Async Function CargarFuncionariosDisponibles() As Task
        Try
            ' Obtiene todos los funcionarios registrados en el sistema.
            Dim todosLosFuncionarios = Await _funcionarioService.GetAllAsync()

            ' Configura el ComboBox únicamente si hay resultados disponibles.
            If todosLosFuncionarios.Any() Then
                cmbFuncionario.DataSource = todosLosFuncionarios.OrderBy(Of String)(Function(f) f.Nombre).ToList()
                cmbFuncionario.DisplayMember = "Nombre"
                cmbFuncionario.ValueMember = "Id"
                cmbFuncionario.SelectedIndex = -1 ' Evita selección predeterminada.
                cmbFuncionario.Text = "Seleccione un funcionario"
            Else
                MessageBox.Show("No hay funcionarios registrados en el sistema. Debe crear un funcionario antes de poder asignarle un usuario.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                btnGuardar.Enabled = False ' Deshabilita el guardado hasta que exista un funcionario disponible.
            End If

        Catch ex As Exception
            MessageBox.Show($"Error al cargar los funcionarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

End Class
