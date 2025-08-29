' Apex/UI/frmNovedadCrear.vb
Imports System.ComponentModel
Imports System.Data.Entity
Imports System.IO

Public Class frmNovedadCrear

    Private _svc As New NovedadService()
    Private _novedad As Novedad
    Private _modo As ModoFormulario
    Private _novedadId As Integer
    Private _funcionariosSeleccionados As New BindingList(Of Funcionario)

    Private _fotosExistentes As New BindingList(Of NovedadFoto)
    Private _rutasFotosNuevas As New List(Of String)
    ' --- MODIFICACIÓN: Lista para marcar fotos a eliminar ---
    Private _fotosParaEliminar As New List(Of NovedadFoto)
    Private _pictureBoxSeleccionado As PictureBox = Nothing

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _novedad = New Novedad()
        Me.Text = "Crear Nueva Novedad"
        btnGuardar.Text = "Guardar"
    End Sub

    Public Sub New(novedadId As Integer)
        Me.New()
        _modo = ModoFormulario.Editar
        _novedadId = novedadId
        Me.Text = "Editar Novedad"
        btnGuardar.Text = "Actualizar"
    End Sub

    Private Async Sub frmNovedadCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        lstFuncionariosSeleccionados.DataSource = _funcionariosSeleccionados
        lstFuncionariosSeleccionados.DisplayMember = "Nombre"
        lstFuncionariosSeleccionados.ValueMember = "Id"

        If _modo = ModoFormulario.Editar Then
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

        _funcionariosSeleccionados.Clear()
        Dim funcionariosAsociados = Await _svc.GetFuncionariosPorNovedadAsync(_novedad.Id)
        For Each func In funcionariosAsociados
            _funcionariosSeleccionados.Add(func)
        Next

        Await CargarFotosAsync()
    End Function

#Region "Lógica de Funcionarios"
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
#End Region

#Region "Lógica de Fotos"
    Private Async Function CargarFotosAsync() As Task
        flpFotos.Controls.Clear()
        _fotosExistentes.Clear()
        _pictureBoxSeleccionado = Nothing

        If _modo = ModoFormulario.Editar Then
            Dim fotosDb = Await _svc.GetFotosPorNovedadAsync(_novedadId)
            For Each foto In fotosDb
                _fotosExistentes.Add(foto)
                AgregarPictureBox(foto.Foto, foto)
            Next
        End If
    End Function

    Private Sub AgregarPictureBox(imageData As Byte(), tag As Object)
        Dim pic As New PictureBox With {
            .Image = Image.FromStream(New MemoryStream(imageData)),
            .SizeMode = PictureBoxSizeMode.Zoom,
            .Size = New Size(120, 120),
            .Margin = New Padding(5),
            .BorderStyle = BorderStyle.FixedSingle,
            .Tag = tag,
            .Cursor = Cursors.Hand
        }
        AddHandler pic.DoubleClick, AddressOf PictureBox_DoubleClick
        AddHandler pic.Click, AddressOf PictureBox_Click
        flpFotos.Controls.Add(pic)
    End Sub

    Private Sub PictureBox_Click(sender As Object, e As EventArgs)
        Dim picClickeado = TryCast(sender, PictureBox)
        If picClickeado Is Nothing Then Return

        If _pictureBoxSeleccionado IsNot Nothing Then
            _pictureBoxSeleccionado.BackColor = Color.Transparent
        End If

        picClickeado.BackColor = Color.DodgerBlue
        _pictureBoxSeleccionado = picClickeado
    End Sub

    Private Sub PictureBox_DoubleClick(sender As Object, e As EventArgs)
        Dim pic = TryCast(sender, PictureBox)
        If pic IsNot Nothing AndAlso pic.Image IsNot Nothing Then
            Using frm As New frmFotografiaNovedades(pic.Image)
                frm.ShowDialog(Me)
            End Using
        End If
    End Sub

    Private Sub btnAgregarFoto_Click(sender As Object, e As EventArgs) Handles btnAgregarFoto.Click
        Using ofd As New OpenFileDialog With {
            .Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp",
            .Multiselect = True
        }
            If ofd.ShowDialog() = DialogResult.OK Then
                For Each archivo In ofd.FileNames
                    ' Siempre añadimos las fotos nuevas a la lista temporal
                    _rutasFotosNuevas.Add(archivo)
                    ' Y mostramos una vista previa
                    AgregarPictureBox(File.ReadAllBytes(archivo), archivo)
                Next
            End If
        End Using
    End Sub

    ' --- MODIFICACIÓN CLAVE: Lógica de eliminación ---
    Private Sub btnEliminarFoto_Click(sender As Object, e As EventArgs) Handles btnEliminarFoto.Click
        If _pictureBoxSeleccionado Is Nothing Then
            MessageBox.Show("Por favor, seleccione una foto para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show("¿Está seguro de que desea eliminar la foto seleccionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            ' Si la foto ya existía en la BD, la añadimos a la lista de "pendientes para eliminar".
            If TypeOf _pictureBoxSeleccionado.Tag Is NovedadFoto Then
                Dim fotoAEliminar = CType(_pictureBoxSeleccionado.Tag, NovedadFoto)
                _fotosParaEliminar.Add(fotoAEliminar)

                ' Si la foto era nueva (aún no guardada), simplemente la quitamos de la lista de "pendientes para añadir".
            ElseIf TypeOf _pictureBoxSeleccionado.Tag Is String Then
                Dim rutaAEliminar = CType(_pictureBoxSeleccionado.Tag, String)
                _rutasFotosNuevas.Remove(rutaAEliminar)
            End If

            ' En ambos casos, quitamos el control visual de la pantalla.
            flpFotos.Controls.Remove(_pictureBoxSeleccionado)
            _pictureBoxSeleccionado.Dispose()
            _pictureBoxSeleccionado = Nothing
        End If
    End Sub
#End Region

#Region "Guardado y Cancelación"
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
                ' 1. Crear la novedad principal
                Dim nuevaNovedad = Await _svc.CrearNovedadCompletaAsync(dtpFecha.Value.Date, txtTexto.Text.Trim(), funcionarioIds)

                ' 2. Si se creó, agregar las fotos nuevas
                If nuevaNovedad IsNot Nothing AndAlso _rutasFotosNuevas.Any() Then
                    For Each ruta In _rutasFotosNuevas
                        Await _svc.AddFotoAsync(nuevaNovedad.Id, ruta)
                    Next
                End If
                MessageBox.Show("Novedad creada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else ' Modo Editar
                ' --- MODIFICACIÓN CLAVE: Lógica de guardado en edición ---

                ' 1. Eliminar las fotos que el usuario marcó para borrar
                If _fotosParaEliminar.Any() Then
                    For Each foto In _fotosParaEliminar
                        Await _svc.DeleteFotoAsync(foto.Id)
                    Next
                End If

                ' 2. Agregar las nuevas fotos que el usuario seleccionó
                If _rutasFotosNuevas.Any() Then
                    For Each ruta In _rutasFotosNuevas
                        Await _svc.AddFotoAsync(_novedadId, ruta)
                    Next
                End If

                ' 3. Actualizar los datos principales de la novedad
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
#End Region

End Class