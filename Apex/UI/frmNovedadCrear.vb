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

    '--- NUEVO: Lista para gestionar las fotos en la UI ---
    Private _fotos As New BindingList(Of NovedadFoto)
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
        ' Ocultar pestaña de fotos al crear, ya que se necesita un ID de novedad.
        TabControlMain.TabPages.Remove(TabPageFotos)
    End Sub

    Public Sub New(novedadId As Integer)
        Me.New()
        _modo = ModoFormulario.Editar
        _novedadId = novedadId
        Me.Text = "Editar Novedad"
        btnGuardar.Text = "Actualizar"
        ' Mostrar la pestaña de fotos al editar
        If Not TabControlMain.TabPages.Contains(TabPageFotos) Then
            TabControlMain.TabPages.Add(TabPageFotos)
        End If
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

        ' Cargar funcionarios asociados
        _funcionariosSeleccionados.Clear()
        Dim funcionariosAsociados = Await _svc.GetFuncionariosPorNovedadAsync(_novedad.Id)
        For Each func In funcionariosAsociados
            _funcionariosSeleccionados.Add(func)
        Next

        ' Cargar fotos asociadas
        Await CargarFotosAsync()
    End Function

    ' --- LÓGICA DE FUNCIONARIOS (Sin cambios) ---
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

    ' --- NUEVA LÓGICA PARA FOTOS (Movida desde frmNovedades) ---
    Private Async Function CargarFotosAsync() As Task
        _pictureBoxSeleccionado = Nothing
        flpFotos.Controls.Clear()
        _fotos.Clear()

        Dim fotosDb = Await _svc.GetFotosPorNovedadAsync(_novedadId)
        For Each foto In fotosDb
            _fotos.Add(foto)
            Dim pic As New PictureBox With {
                .Image = Image.FromStream(New MemoryStream(foto.Foto)),
                .SizeMode = PictureBoxSizeMode.Zoom,
                .Size = New Size(120, 120),
                .Margin = New Padding(5),
                .BorderStyle = BorderStyle.FixedSingle,
                .Tag = foto, ' Guardar el objeto completo
                .Cursor = Cursors.Hand
            }
            AddHandler pic.DoubleClick, AddressOf PictureBox_DoubleClick
            AddHandler pic.Click, AddressOf PictureBox_Click
            flpFotos.Controls.Add(pic)
        Next
    End Function

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
            Using frm As New frmVisorFoto(pic.Image)
                frm.ShowDialog(Me)
            End Using
        End If
    End Sub

    Private Async Sub btnAgregarFoto_Click(sender As Object, e As EventArgs) Handles btnAgregarFoto.Click
        Using ofd As New OpenFileDialog With {
            .Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp",
            .Multiselect = True
        }
            If ofd.ShowDialog() = DialogResult.OK Then
                LoadingHelper.MostrarCargando(Me)
                Try
                    For Each archivo In ofd.FileNames
                        Await _svc.AddFotoAsync(_novedadId, archivo)
                    Next
                    Await CargarFotosAsync()
                Catch ex As Exception
                    MessageBox.Show("Error al agregar foto(s): " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    LoadingHelper.OcultarCargando(Me)
                End Try
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarFoto_Click(sender As Object, e As EventArgs) Handles btnEliminarFoto.Click
        If _pictureBoxSeleccionado Is Nothing Then
            MessageBox.Show("Por favor, seleccione una foto para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        If MessageBox.Show("¿Está seguro de que desea eliminar la foto seleccionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Dim fotoAEliminar = CType(_pictureBoxSeleccionado.Tag, NovedadFoto)
            LoadingHelper.MostrarCargando(Me)
            Try
                Await _svc.DeleteFotoAsync(fotoAEliminar.Id)
                Await CargarFotosAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la foto: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                LoadingHelper.OcultarCargando(Me)
            End Try
        End If
    End Sub

    ' --- LÓGICA DE GUARDADO (Sin cambios) ---
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