' Apex/UI/frmNovedades.vb
Imports System.Data.Entity
Imports System.IO

Public Class frmNovedades

    Private _bsNovedades As New BindingSource()
    Private _pictureBoxSeleccionado As PictureBox = Nothing
    Private _idNovedadSeleccionada As Integer?

    Private Async Sub frmNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        dgvNovedades.DataSource = _bsNovedades
        Await CargarNovedadesAsync()
    End Sub

    Private Async Sub dgvNovedades_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNovedades.SelectionChanged
        Await ActualizarDetalleDesdeSeleccion()
    End Sub

#Region "Carga de Datos y Configuración"

    Private Sub ConfigurarGrilla()
        ' (Sin cambios)
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

    Private Async Function CargarNovedadesAsync(Optional mantenerSeleccion As Boolean = True) As Task
        LoadingHelper.MostrarCargando(Me)

        ' 1. Guardar el ID de la selección actual si es necesario
        If mantenerSeleccion AndAlso dgvNovedades.CurrentRow IsNot Nothing Then
            _idNovedadSeleccionada = CInt(dgvNovedades.CurrentRow.Cells("Id").Value)
        End If

        ' 2. Desactivar el evento para evitar ejecuciones múltiples
        RemoveHandler dgvNovedades.SelectionChanged, AddressOf dgvNovedades_SelectionChanged

        Try
            ' 3. Limpiar y recargar la fuente de datos
            _bsNovedades.DataSource = Nothing
            Using svc As New NovedadService()
                _bsNovedades.DataSource = Await svc.GetAllAgrupadasAsync()
            End Using

            ' 4. Restaurar la selección si es posible
            If _idNovedadSeleccionada.HasValue Then
                Dim itemToSelect = _bsNovedades.List.Cast(Of vw_NovedadesAgrupadas)().FirstOrDefault(Function(n) n.Id = _idNovedadSeleccionada.Value)
                If itemToSelect IsNot Nothing Then
                    _bsNovedades.Position = _bsNovedades.IndexOf(itemToSelect)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error al cargar las novedades: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' --- CORRECCIÓN CLAVE ---
        ' 5. Mover la lógica de actualización de la UI FUERA del bloque Try/Catch/Finally
        AddHandler dgvNovedades.SelectionChanged, AddressOf dgvNovedades_SelectionChanged
        Await ActualizarDetalleDesdeSeleccion()
        LoadingHelper.OcultarCargando(Me)

    End Function

    Private Sub LimpiarDetalles()
        txtTextoNovedad.Clear()
        lstFuncionarios.DataSource = Nothing
        flpFotos.Controls.Clear()
        _pictureBoxSeleccionado = Nothing
    End Sub

    Private Async Function ActualizarDetalleDesdeSeleccion() As Task
        If dgvNovedades.CurrentRow Is Nothing OrElse dgvNovedades.CurrentRow.DataBoundItem Is Nothing Then
            LimpiarDetalles()
            Return
        End If

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        _idNovedadSeleccionada = novedadSeleccionada.Id

        Using svc As New NovedadService()
            Dim novedadCompleta = Await svc.GetByIdAsync(novedadSeleccionada.Id)
            txtTextoNovedad.Text = If(novedadCompleta IsNot Nothing, novedadCompleta.Texto, "")

            Dim funcionarios = Await svc.GetFuncionariosPorNovedadAsync(novedadSeleccionada.Id)
            lstFuncionarios.DataSource = funcionarios
            lstFuncionarios.DisplayMember = "Nombre"
            lstFuncionarios.ValueMember = "Id"

            Await CargarFotos(novedadSeleccionada.Id)
        End Using
    End Function
#End Region

#Region "Gestión de Fotos (Visualización)"
    Private Async Function CargarFotos(novedadId As Integer) As Task
        _pictureBoxSeleccionado = Nothing
        flpFotos.Controls.Clear() ' Punto clave: siempre limpiar primero

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
#End Region

#Region "Gestión de Novedades (CRUD)"
    Private Async Sub btnNuevaNovedad_Click(sender As Object, e As EventArgs) Handles btnNuevaNovedad.Click
        Using frm As New frmNovedadCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarNovedadesAsync(mantenerSeleccion:=False)
            End If
        End Using
    End Sub

    Private Async Sub btnEditarNovedad_Click(sender As Object, e As EventArgs) Handles btnEditarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad de la lista para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim novedadId = CInt(dgvNovedades.CurrentRow.Cells("Id").Value)
        Using frm As New frmNovedadCrear(novedadId)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarNovedadesAsync(mantenerSeleccion:=True)
            End If
        End Using
    End Sub
#End Region

End Class