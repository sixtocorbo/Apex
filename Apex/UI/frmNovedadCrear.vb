' Apex/UI/frmNovedadCrear.vb
Imports System.ComponentModel

Public Class frmNovedadCrear

    Private _svc As New NovedadService()
    ' Usamos un BindingList para que el ListBox se actualice automáticamente
    Private _funcionariosSeleccionados As New BindingList(Of Funcionario)

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub frmNovedadCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ' Enlazar la lista de funcionarios al ListBox
        lstFuncionariosSeleccionados.DataSource = _funcionariosSeleccionados
        lstFuncionariosSeleccionados.DisplayMember = "Nombre"
        lstFuncionariosSeleccionados.ValueMember = "Id"
    End Sub

    Private Async Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        ' Abrimos el formulario en modo de selección
        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                If frm.FuncionarioSeleccionado IsNot Nothing Then
                    Dim idFuncionarioSeleccionado = frm.FuncionarioSeleccionado.Id

                    ' Verificar si el funcionario ya está en la lista para evitar duplicados
                    If Not _funcionariosSeleccionados.Any(Function(f) f.Id = idFuncionarioSeleccionado) Then
                        Using uow As New UnitOfWork()
                            ' Se necesita el objeto Funcionario completo, no solo el DTO FuncionarioMin
                            Dim funcCompleto = Await uow.Repository(Of Funcionario)().GetByIdAsync(idFuncionarioSeleccionado)
                            If funcCompleto IsNot Nothing Then
                                _funcionariosSeleccionados.Add(funcCompleto)
                            End If
                        End Using
                    Else
                        MessageBox.Show("El funcionario seleccionado ya se encuentra en la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            End If
        End Using
    End Sub

    Private Sub btnQuitarFuncionario_Click(sender As Object, e As EventArgs) Handles btnQuitarFuncionario.Click
        If lstFuncionariosSeleccionados.SelectedItems.Count > 0 Then
            ' Hacemos una copia de los items a quitar para evitar problemas al modificar la colección
            Dim itemsAQuitar = lstFuncionariosSeleccionados.SelectedItems.Cast(Of Funcionario).ToList()
            For Each item In itemsAQuitar
                _funcionariosSeleccionados.Remove(item)
            Next
        End If
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ' --- Validaciones ---
        If String.IsNullOrWhiteSpace(txtTexto.Text) Then
            MessageBox.Show("El texto de la novedad no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If _funcionariosSeleccionados.Count = 0 Then
            MessageBox.Show("Debe seleccionar al menos un funcionario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' --- Proceso de Guardado ---
        LoadingHelper.MostrarCargando(Me)
        Try
            ' 1. Obtener la lista de IDs de los funcionarios seleccionados
            Dim funcionarioIds = _funcionariosSeleccionados.Select(Function(f) f.Id).ToList()

            ' 2. Llamar al método del servicio que maneja toda la transacción
            Await _svc.CrearNovedadCompletaAsync(dtpFecha.Value.Date, txtTexto.Text.Trim(), funcionarioIds)

            MessageBox.Show("Novedad creada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            DialogResult = DialogResult.OK
            Close()

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al guardar la novedad: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

End Class