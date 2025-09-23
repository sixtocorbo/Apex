Imports System.Data.Entity
Imports System.Windows.Forms

Public Class frmUsuarioCrear
    Private ReadOnly _usuarioService As New UsuarioService()
    Private ReadOnly _funcionarioService As New FuncionarioService()

    ' En /UI/frmUsuarioCrear.vb

    Private Async Sub frmUsuarioCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' 1. Await espera a que la lista de funcionarios se cargue completamente en memoria.
            Dim todosLosFuncionarios = Await _funcionarioService.GetAllAsync()

            ' 2. Una vez que tienes la lista, ahora sí puedes filtrarla con .Where()
            Dim funcionariosSinUsuario = todosLosFuncionarios.Where(Function(f) f.Usuario Is Nothing).ToList()

            ' 3. Configura el ComboBox (esto no cambia)
            If funcionariosSinUsuario.Any() Then
                cmbFuncionario.DataSource = funcionariosSinUsuario
                cmbFuncionario.DisplayMember = "Nombre"
                cmbFuncionario.ValueMember = "Id"
            Else
                MessageBox.Show("No hay funcionarios disponibles para crearles un usuario.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
                btnGuardar.Enabled = False ' Deshabilitamos el botón si no hay funcionarios
            End If

        Catch ex As Exception
            MessageBox.Show("Error al cargar los funcionarios: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' Validaciones básicas
        If cmbFuncionario.SelectedItem Is Nothing Then
            MessageBox.Show("Debe seleccionar un funcionario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If String.IsNullOrWhiteSpace(txtNombreUsuario.Text) OrElse String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("El nombre de usuario y la contraseña no pueden estar vacíos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Dim funcionarioSeleccionado = CType(cmbFuncionario.SelectedItem, Funcionario)

            ' Crea la nueva entidad de Usuario
            Dim nuevoUsuario As New Usuario With {
                .NombreUsuario = txtNombreUsuario.Text.Trim(),
                .NombreCompleto = funcionarioSeleccionado.Nombre,
                .FuncionarioId = funcionarioSeleccionado.Id,
                .Activo = True
            }

            ' Llama al servicio para crear el usuario en la base de datos
            Await _usuarioService.CreateAsync(nuevoUsuario, txtPassword.Text)

            MessageBox.Show("Usuario registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error al registrar el usuario: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class