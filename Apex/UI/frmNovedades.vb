' Apex/UI/frmNovedades.vb
Imports System.Data.Entity
Imports System.IO

Public Class frmNovedades

    Private _svc As New NovedadService()
    Private _novedadGeneradaActual As NovedadGenerada

    Private Async Sub frmNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Await CargarNovedadesDelDia()
    End Sub

    Private Async Sub dtpFecha_ValueChanged(sender As Object, e As EventArgs) Handles dtpFecha.ValueChanged
        Await CargarNovedadesDelDia()
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
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NovedadGeneradaId", .DataPropertyName = "NovedadGeneradaId", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Funcionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
        End With
    End Sub

    Private Async Function CargarNovedadesDelDia() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim fechaSeleccionada = dtpFecha.Value.Date
            _novedadGeneradaActual = Await _svc.GetOrCreateNovedadGeneradaAsync(fechaSeleccionada)

            ' --- INICIO DE LA CORRECCIÓN ---
            ' Le pasamos la misma fecha dos veces para cumplir con la firma del método
            Dim novedades = Await _svc.GetAllConDetallesAsync(fechaSeleccionada, fechaSeleccionada)
            ' --- FIN DE LA CORRECCIÓN ---

            dgvNovedades.DataSource = novedades

            If Not novedades.Any() Then
                LimpiarDetalles()
            End If

            Await CargarFotos()
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
    End Sub

    Private Async Function MostrarDetalleNovedadSeleccionada() As Task
        If dgvNovedades.CurrentRow Is Nothing OrElse dgvNovedades.CurrentRow.DataBoundItem Is Nothing Then
            LimpiarDetalles()
            Return
        End If

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesCompletas)
        txtTextoNovedad.Text = novedadSeleccionada.Texto

        lstFuncionarios.DataSource = Nothing
        Dim funcionarios = Await _svc.GetFuncionariosPorNovedadAsync(novedadSeleccionada.Id)
        lstFuncionarios.DataSource = funcionarios
        lstFuncionarios.DisplayMember = "Nombre"
        lstFuncionarios.ValueMember = "Id"
    End Function

#End Region

#Region "Gestión de Fotos"

    Private Async Function CargarFotos() As Task
        flpFotos.Controls.Clear()
        If _novedadGeneradaActual Is Nothing Then Return

        Dim fotos = Await _svc.GetFotosPorNovedadGeneradaAsync(_novedadGeneradaActual.Id)
        For Each foto In fotos
            Dim pic As New PictureBox With {
                .Image = Image.FromStream(New MemoryStream(foto.Foto)),
                .SizeMode = PictureBoxSizeMode.Zoom,
                .Size = New Size(120, 120),
                .Margin = New Padding(5),
                .BorderStyle = BorderStyle.FixedSingle,
                .Tag = foto.Id
            }
            flpFotos.Controls.Add(pic)
        Next
    End Function

    Private Async Sub btnAgregarFoto_Click(sender As Object, e As EventArgs) Handles btnAgregarFoto.Click
        Using ofd As New OpenFileDialog With {
            .Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp",
            .Multiselect = True
        }
            If ofd.ShowDialog() = DialogResult.OK Then
                LoadingHelper.MostrarCargando(Me)
                Try
                    For Each archivo In ofd.FileNames
                        Await _svc.AddFotoAsync(_novedadGeneradaActual.Id, archivo)
                    Next
                    Await CargarFotos()
                Catch ex As Exception
                    MessageBox.Show("Error al agregar foto(s): " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    LoadingHelper.OcultarCargando(Me)
                End Try
            End If
        End Using
    End Sub

    Private Sub btnEliminarFoto_Click(sender As Object, e As EventArgs) Handles btnEliminarFoto.Click
        MessageBox.Show("Funcionalidad para seleccionar y eliminar fotos pendiente de implementación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#End Region

#Region "Gestión de Funcionarios y Novedades (Placeholder)"

    Private Sub btnNuevaNovedad_Click(sender As Object, e As EventArgs) Handles btnNuevaNovedad.Click
        MessageBox.Show("Funcionalidad para crear una nueva novedad pendiente de implementación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        MessageBox.Show("Funcionalidad para agregar funcionario pendiente de implementación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnQuitarFuncionario_Click(sender As Object, e As EventArgs) Handles btnQuitarFuncionario.Click
        MessageBox.Show("Funcionalidad para quitar funcionario pendiente de implementación.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

#End Region

End Class