' Apex/UI/frmNovedades.vb
Imports System.ComponentModel
Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Text

Public Class frmNovedades

    Private _bsNovedades As New BindingSource()
    Private _pictureBoxSeleccionado As PictureBox = Nothing
    Private _idNovedadSeleccionada As Integer?

    Private Async Sub frmNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        dgvNovedades.DataSource = _bsNovedades
        ' Carga las últimas novedades al abrir el formulario
        Await BuscarAsync(String.Empty)
    End Sub

    Private Async Sub dgvNovedades_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNovedades.SelectionChanged
        Await ActualizarDetalleDesdeSeleccion()
    End Sub

#Region "Carga de Datos y Búsqueda"

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

    Private Async Function BuscarAsync(Optional textoBusqueda As String = Nothing) As Task
        If textoBusqueda Is Nothing Then textoBusqueda = txtBusqueda.Text

        LoadingHelper.MostrarCargando(Me)
        btnBuscar.Enabled = False
        _bsNovedades.DataSource = Nothing
        LimpiarDetalles() ' Limpia los detalles al iniciar la búsqueda

        Try
            Using svc As New NovedadService()
                _bsNovedades.DataSource = Await svc.BuscarNovedadesAgrupadasFTSAsync(textoBusqueda)
            End Using

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al buscar las novedades: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
            btnBuscar.Enabled = True
        End Try
    End Function

    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Await BuscarAsync()
    End Sub

    Private Async Sub txtBusqueda_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusqueda.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await BuscarAsync()
        End If
    End Sub

#End Region

#Region "Lógica del Detalle"

    Private Sub LimpiarDetalles()
        txtTextoNovedad.Clear()
        lstFuncionarios.DataSource = Nothing
        flpFotos.Controls.Clear()
        _pictureBoxSeleccionado = Nothing
        ' --- CORRECCIÓN ---
        ' Es crucial resetear el ID seleccionado para permitir que la nueva selección se cargue.
        _idNovedadSeleccionada = Nothing
    End Sub

    Private Async Function ActualizarDetalleDesdeSeleccion() As Task
        If dgvNovedades.CurrentRow Is Nothing OrElse dgvNovedades.CurrentRow.DataBoundItem Is Nothing Then
            LimpiarDetalles()
            Return
        End If

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)

        ' --- CORRECCIÓN ---
        ' Evita la recarga innecesaria si el evento se dispara para una novedad que ya está mostrada.
        ' Esto soluciona el problema de carga múltiple al asignar el DataSource.
        If _idNovedadSeleccionada.HasValue AndAlso _idNovedadSeleccionada.Value = novedadSeleccionada.Id Then
            Return
        End If

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
#End Region

#Region "Gestión de Novedades (CRUD)"

    Private Async Sub btnNuevaNovedad_Click(sender As Object, e As EventArgs) Handles btnNuevaNovedad.Click
        Using frm As New frmNovedadCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await BuscarAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditarNovedad_Click(sender As Object, e As EventArgs) Handles btnEditarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim novedadId = CInt(dgvNovedades.CurrentRow.Cells("Id").Value)
        Using frm As New frmNovedadCrear(novedadId)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await BuscarAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarNovedad_Click(sender As Object, e As EventArgs) Handles btnEliminarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        Dim confirmResult = MessageBox.Show($"¿Está seguro de que desea eliminar la novedad del {novedadSeleccionada.Fecha:d}?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirmResult = DialogResult.Yes Then
            LoadingHelper.MostrarCargando(Me)
            Try
                Using svc As New NovedadService()
                    Await svc.DeleteNovedadCompletaAsync(novedadSeleccionada.Id)
                End Using
                Await BuscarAsync()
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al eliminar la novedad: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                LoadingHelper.OcultarCargando(Me)
            End Try
        End If
    End Sub
#End Region

End Class