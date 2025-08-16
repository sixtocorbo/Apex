' Apex/UI/frmNovedades.vb
Imports System.Data.Entity
Imports System.IO

Public Class frmNovedades

    ' Variable para mantener una referencia a la foto seleccionada para eliminar
    Private _pictureBoxSeleccionado As PictureBox = Nothing

    Private Async Sub frmNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        ' Carga inicial de todas las novedades
        Await CargarNovedadesAsync()
    End Sub

    Private Async Sub dgvNovedades_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNovedades.SelectionChanged
        Await MostrarDetalleNovedadSeleccionada()
    End Sub

#Region "Carga de Datos y Configuración"

    Private Sub ConfigurarGrilla()
        With dgvNovedades
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Fecha", .DataPropertyName = "Fecha", .HeaderText = "Fecha", .Width = 100})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Resumen", .DataPropertyName = "Resumen", .HeaderText = "Resumen de Novedad", .Width = 300})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Funcionarios", .DataPropertyName = "Funcionarios", .HeaderText = "Funcionarios Involucrados", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
        End With
    End Sub

    Private Async Function CargarNovedadesAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Using svc As New NovedadService()
                ' Usamos el nuevo método que trae los datos agrupados
                dgvNovedades.DataSource = Await svc.GetAllAgrupadasAsync()
            End Using

            If dgvNovedades.Rows.Count = 0 Then
                LimpiarDetalles()
            End If

        Catch ex As Exception
            MessageBox.Show("Error al cargar las novedades: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Sub LimpiarDetalles()
        txtTextoNovedad.Clear()
        lstFuncionarios.DataSource = Nothing
        flpFotos.Controls.Clear()
        _pictureBoxSeleccionado = Nothing
    End Sub

    Private Async Function MostrarDetalleNovedadSeleccionada() As Task
        If dgvNovedades.CurrentRow Is Nothing OrElse dgvNovedades.CurrentRow.DataBoundItem Is Nothing Then
            LimpiarDetalles()
            Return
        End If

        Dim novedadAgrupada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)

        Using svc As New NovedadService()
            ' Para el texto completo, necesitamos buscar la novedad original por su Id
            Dim novedadCompleta = Await svc.GetByIdAsync(novedadAgrupada.Id)
            If novedadCompleta IsNot Nothing Then
                txtTextoNovedad.Text = novedadCompleta.Texto
            End If

            ' Los funcionarios y fotos se cargan usando el Id de la novedad
            Dim funcionarios = Await svc.GetFuncionariosPorNovedadAsync(novedadAgrupada.Id)
            lstFuncionarios.DataSource = funcionarios
            lstFuncionarios.DisplayMember = "Nombre"
            lstFuncionarios.ValueMember = "Id"

            Await CargarFotos(novedadAgrupada.Id)
        End Using
    End Function

#End Region

#Region "Gestión de Fotos"

    Private Async Function CargarFotos(novedadId As Integer) As Task
        _pictureBoxSeleccionado = Nothing ' Limpiar selección al recargar
        flpFotos.Controls.Clear()

        Using svc As New NovedadService()
            Dim fotos = Await svc.GetFotosPorNovedadAsync(novedadId)
            For Each foto In fotos
                Dim pic As New PictureBox With {
                    .Image = Image.FromStream(New MemoryStream(foto.Foto)),
                    .SizeMode = PictureBoxSizeMode.Zoom,
                    .Size = New Size(120, 120),
                    .Margin = New Padding(5),
                    .BorderStyle = BorderStyle.FixedSingle,
                    .Tag = foto.Id,
                    .Cursor = Cursors.Hand
                }
                AddHandler pic.DoubleClick, AddressOf PictureBox_DoubleClick
                AddHandler pic.Click, AddressOf PictureBox_Click
                flpFotos.Controls.Add(pic)
            Next
        End Using
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
        If dgvNovedades.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad de la lista antes de agregar fotos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        Dim novedadId = novedadSeleccionada.Id

        Using ofd As New OpenFileDialog With {
            .Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp",
            .Multiselect = True
        }
            If ofd.ShowDialog() = DialogResult.OK Then
                LoadingHelper.MostrarCargando(Me)
                Try
                    Using svc As New NovedadService()
                        For Each archivo In ofd.FileNames
                            Await svc.AddFotoAsync(novedadId, archivo)
                        Next
                    End Using
                    Await CargarFotos(novedadId)
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
            MessageBox.Show("Por favor, seleccione una foto para eliminar haciendo clic sobre ella.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim confirmResult = MessageBox.Show("¿Está seguro de que desea eliminar la foto seleccionada?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmResult = DialogResult.Yes Then
            Dim fotoId = CInt(_pictureBoxSeleccionado.Tag)
            LoadingHelper.MostrarCargando(Me)
            Try
                Using svc As New NovedadService()
                    Await svc.DeleteFotoAsync(fotoId)
                End Using
                Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
                Await CargarFotos(novedadSeleccionada.Id)
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la foto: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                LoadingHelper.OcultarCargando(Me)
            End Try
        End If
    End Sub

#End Region

#Region "Gestión de Funcionarios y Novedades"

    Private Async Sub btnNuevaNovedad_Click(sender As Object, e As EventArgs) Handles btnNuevaNovedad.Click
        Using frm As New frmNovedadCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarNovedadesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        Dim novedadId = novedadSeleccionada.Id

        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                If frm.FuncionarioSeleccionado IsNot Nothing Then
                    Dim funcionarioId = frm.FuncionarioSeleccionado.Id
                    Try
                        Using svc As New NovedadService()
                            Await svc.AgregarFuncionarioANovedadAsync(novedadId, funcionarioId)
                        End Using
                        Await CargarNovedadesAsync() ' Recargamos la grilla para ver el funcionario añadido
                    Catch ex As Exception
                        MessageBox.Show("Error al agregar el funcionario: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        End Using
    End Sub

    Private Async Sub btnQuitarFuncionario_Click(sender As Object, e As EventArgs) Handles btnQuitarFuncionario.Click
        If dgvNovedades.CurrentRow Is Nothing OrElse lstFuncionarios.SelectedItem Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad y un funcionario de la lista para quitar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        Dim funcionarioSeleccionado = CType(lstFuncionarios.SelectedItem, Funcionario)
        Dim novedadId = novedadSeleccionada.Id
        Dim funcionarioId = funcionarioSeleccionado.Id

        Dim confirmResult = MessageBox.Show($"¿Está seguro de que desea quitar a '{funcionarioSeleccionado.Nombre}' de esta novedad?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmResult = DialogResult.Yes Then
            Try
                Using svc As New NovedadService()
                    Await svc.QuitarFuncionarioDeNovedadAsync(novedadId, funcionarioId)
                End Using
                Await CargarNovedadesAsync() ' Recargamos la grilla para reflejar el cambio
            Catch ex As Exception
                MessageBox.Show("Error al quitar el funcionario: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region

End Class