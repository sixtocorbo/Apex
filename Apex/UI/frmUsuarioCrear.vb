Imports System.Data.Entity
Imports System.Windows.Forms

Public Class frmUsuarioCrear
    ' --- Servicios ---
    Private ReadOnly _usuarioService As New UsuarioService()
    Private ReadOnly _funcionarioService As New FuncionarioService()

    ' --- Propiedades ---
    Private _id As Integer? ' Guarda el ID del usuario. Si es Nothing, es un nuevo usuario.

    ' ==========================================================================
    ' CONSTRUCTORES
    ' ==========================================================================

    Public Sub New(id As Integer)
        Me.New()
        _id = id
    End Sub

    Public Sub New()
        InitializeComponent()
    End Sub

    ' ==========================================================================
    ' EVENTOS DEL FORMULARIO
    ' ==========================================================================

    Private Async Sub frmUsuarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _id.HasValue Then
            ' --- MODO EDICIÓN ---
            Me.Text = "Editar Usuario"
            Await CargarDatosParaEdicion()
        Else
            ' --- MODO CREACIÓN ---
            Me.Text = "Crear Nuevo Usuario"
            Await CargarFuncionariosDisponibles()
        End If
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' --- Validaciones (el código que ya tenías está bien) ---
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
                ' --- LÓGICA PARA ACTUALIZAR ---
                Dim usuarioExistente = Await _usuarioService.GetByIdAsync(_id.Value)
                If usuarioExistente IsNot Nothing Then
                    usuarioExistente.NombreUsuario = txtNombreUsuario.Text.Trim()
                    ' Solo actualiza la contraseña si se escribió algo en el TextBox
                    Dim nuevaPassword = If(String.IsNullOrWhiteSpace(txtPassword.Text), Nothing, txtPassword.Text)
                    Await _usuarioService.UpdateAsync(usuarioExistente, nuevaPassword)
                    MessageBox.Show("Usuario actualizado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                ' --- LÓGICA PARA CREAR ---
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

    ' ==========================================================================
    ' MÉTODOS PRIVADOS DE AYUDA
    ' ==========================================================================

    ''' <summary>
    ''' MODO EDICIÓN: Carga los datos del usuario y bloquea el ComboBox con el funcionario correspondiente.
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


            ' Cargar solo el funcionario asociado al usuario
            Dim funcionarioActual = Await _funcionarioService.GetByIdAsync(usuario.FuncionarioId)
            If funcionarioActual IsNot Nothing Then
                cmbFuncionario.DataSource = New List(Of Funcionario) From {funcionarioActual}
                cmbFuncionario.DisplayMember = "Nombre"
                cmbFuncionario.ValueMember = "Id"
                cmbFuncionario.Enabled = False ' Bloqueamos el control
            End If

        Catch ex As Exception
            MessageBox.Show($"Error al cargar los datos del usuario: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    ''' <summary>
    ''' MODO CREACIÓN: Carga en el ComboBox a TODOS los funcionarios del sistema para asociar uno.
    ''' </summary>
    Private Async Function CargarFuncionariosDisponibles() As Task
        Try
            ' 1. Cargamos TODOS los funcionarios del sistema, sin ningún filtro.
            Dim todosLosFuncionarios = Await _funcionarioService.GetAllAsync()

            ' 2. Verificamos si la lista tiene elementos.
            If todosLosFuncionarios.Any() Then
                cmbFuncionario.DataSource = todosLosFuncionarios.OrderBy(Of String)(Function(f) f.Nombre).ToList()
                cmbFuncionario.DisplayMember = "Nombre"
                cmbFuncionario.ValueMember = "Id"
                cmbFuncionario.SelectedIndex = -1 ' Para que no aparezca ninguno seleccionado por defecto
                cmbFuncionario.Text = "Seleccione un funcionario"
            Else
                MessageBox.Show("No hay funcionarios registrados en el sistema. Debe crear un funcionario antes de poder asignarle un usuario.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                btnGuardar.Enabled = False ' Deshabilitamos el botón si no hay opciones
            End If

        Catch ex As Exception
            MessageBox.Show($"Error al cargar los funcionarios: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

End Class