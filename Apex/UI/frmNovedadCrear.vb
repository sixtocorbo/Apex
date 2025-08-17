' Apex/UI/frmNovedadCrear.vb
Imports System.ComponentModel
Imports System.Data.Entity

Public Class frmNovedadCrear

    Private _svc As New NovedadService()
    Private _novedad As Novedad
    Private _modo As ModoFormulario
    Private _novedadId As Integer ' Usaremos esta variable para guardar el ID en modo edición
    Private _funcionariosSeleccionados As New BindingList(Of Funcionario)

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    ' Constructor para CREAR una nueva novedad (sin cambios)
    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _novedad = New Novedad()
        Me.Text = "Crear Nueva Novedad"
        btnGuardar.Text = "Guardar"
    End Sub

    ' Constructor para EDITAR (Corregido)
    ' Ahora solo guarda el ID. La carga de datos se hará en el evento Load.
    Public Sub New(novedadId As Integer)
        Me.New() ' Llama al constructor base
        _modo = ModoFormulario.Editar
        _novedadId = novedadId ' Guardamos el ID para usarlo después
        Me.Text = "Editar Novedad"
        btnGuardar.Text = "Actualizar"
    End Sub

    ' Evento Load (Corregido)
    ' Ahora maneja la carga de datos para el modo edición
    Private Async Sub frmNovedadCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' Enlazar la lista de funcionarios al ListBox
        lstFuncionariosSeleccionados.DataSource = _funcionariosSeleccionados
        lstFuncionariosSeleccionados.DisplayMember = "Nombre"
        lstFuncionariosSeleccionados.ValueMember = "Id"

        If _modo = ModoFormulario.Editar Then
            ' Si estamos editando, cargamos los datos aquí
            Await CargarDatosParaEdicion()
        End If
    End Sub

    Private Async Function CargarDatosParaEdicion() As Task
        _novedad = Await _svc.GetByIdAsync(_novedadId)

        If _novedad Is Nothing Then
            MessageBox.Show("No se encontró la novedad para editar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        dtpFecha.Value = _novedad.Fecha
        txtTexto.Text = _novedad.Texto

        ' Cargar los funcionarios ya asociados
        Dim funcionariosAsociados = Await _svc.GetFuncionariosPorNovedadAsync(_novedad.Id)
        For Each func In funcionariosAsociados
            _funcionariosSeleccionados.Add(func)
        Next
    End Function

    Private Async Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog(Me) = DialogResult.OK AndAlso frm.FuncionarioSeleccionado IsNot Nothing Then
                Dim idFuncionarioSeleccionado = frm.FuncionarioSeleccionado.Id
                If Not _funcionariosSeleccionados.Any(Function(f) f.Id = idFuncionarioSeleccionado) Then
                    Using uow As New UnitOfWork()
                        Dim funcCompleto = Await uow.Repository(Of Funcionario)().GetByIdAsync(idFuncionarioSeleccionado)
                        If funcCompleto IsNot Nothing Then
                            _funcionariosSeleccionados.Add(funcCompleto)
                        End If
                    End Using
                Else
                    MessageBox.Show("El funcionario seleccionado ya se encuentra en la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        End Using
    End Sub

    Private Sub btnQuitarFuncionario_Click(sender As Object, e As EventArgs) Handles btnQuitarFuncionario.Click
        If lstFuncionariosSeleccionados.SelectedItems.Count > 0 Then
            Dim itemsAQuitar = lstFuncionariosSeleccionados.SelectedItems.Cast(Of Funcionario).ToList()
            For Each item In itemsAQuitar
                _funcionariosSeleccionados.Remove(item)
            Next
        End If
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtTexto.Text) Then
            MessageBox.Show("El texto de la novedad no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        If _funcionariosSeleccionados.Count = 0 Then
            MessageBox.Show("Debe seleccionar al menos un funcionario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        LoadingHelper.MostrarCargando(Me)
        Try
            Dim funcionarioIds = _funcionariosSeleccionados.Select(Function(f) f.Id).ToList()

            If _modo = ModoFormulario.Crear Then
                Await _svc.CrearNovedadCompletaAsync(dtpFecha.Value.Date, txtTexto.Text.Trim(), funcionarioIds)
                MessageBox.Show("Novedad creada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                _novedad.Fecha = dtpFecha.Value.Date
                _novedad.Texto = txtTexto.Text.Trim()
                Await _svc.ActualizarNovedadCompletaAsync(_novedad, funcionarioIds)
                MessageBox.Show("Novedad actualizada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

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