' Apex/UI/frmNovedades.vb
Imports System.Data.Entity
Imports System.IO

Public Class frmNovedades

    Private _novedadGeneradaActual As NovedadGenerada

    Private Async Sub frmNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        ' --- INICIO DE LA CORRECCIÓN ---
        ' Por defecto, al cargar, no filtramos por fecha para ver todo.
        Await CargarNovedadesAsync()
        ' --- FIN DE LA CORRECCIÓN ---
    End Sub

    ' --- INICIO DE LA CORRECCIÓN ---
    ' Este evento ahora llamará al método de carga general
    Private Async Sub dtpFecha_ValueChanged(sender As Object, e As EventArgs) Handles dtpFecha.ValueChanged
        ' Para mantener la funcionalidad de seleccionar un día y ver sus detalles de fotos, etc.
        ' actualizamos la NovedadGenerada del día seleccionado.
        Using svc As New NovedadService()
            _novedadGeneradaActual = Await svc.GetOrCreateNovedadGeneradaAsync(dtpFecha.Value.Date)
            Await CargarFotos()
        End Using

        ' Si el usuario quiere filtrar por este día, puede presionar "Nueva Novedad" o un futuro botón de "Filtrar"
    End Sub
    ' --- FIN DE LA CORRECCIÓN ---

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
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Fecha", .DataPropertyName = "Fecha", .HeaderText = "Fecha", .Width = 100})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Funcionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
        End With
    End Sub

    ' --- INICIO DE LA CORRECCIÓN ---
    ' Renombrado y modificado para aceptar un rango o cargar todo
    Private Async Function CargarNovedadesAsync(Optional fechaDesde As Date? = Nothing, Optional fechaHasta As Date? = Nothing) As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Using svc As New NovedadService()
                ' Si no se pasan fechas, el servicio traerá todo.
                Dim novedades = Await svc.GetAllConDetallesAsync(fechaDesde, fechaHasta)
                dgvNovedades.DataSource = novedades
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
    ' --- FIN DE LA CORRECCIÓN ---


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
        Using svc As New NovedadService()
            Dim funcionarios = Await svc.GetFuncionariosPorNovedadAsync(novedadSeleccionada.Id)
            lstFuncionarios.DataSource = funcionarios

            ' Actualizar la NovedadGenerada y las fotos para la novedad seleccionada
            _novedadGeneradaActual = Await svc.GetOrCreateNovedadGeneradaAsync(novedadSeleccionada.Fecha)
            Await CargarFotos()
        End Using
        lstFuncionarios.DisplayMember = "Nombre"
        lstFuncionarios.ValueMember = "Id"
    End Function

#End Region

#Region "Gestión de Fotos"

    Private Async Function CargarFotos() As Task
        flpFotos.Controls.Clear()
        If _novedadGeneradaActual Is Nothing Then Return

        Using svc As New NovedadService()
            Dim fotos = Await svc.GetFotosPorNovedadGeneradaAsync(_novedadGeneradaActual.Id)
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
        End Using
    End Function

    Private Async Sub btnAgregarFoto_Click(sender As Object, e As EventArgs) Handles btnAgregarFoto.Click
        Using ofd As New OpenFileDialog With {
            .Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp",
            .Multiselect = True
        }
            If ofd.ShowDialog() = DialogResult.OK Then
                LoadingHelper.MostrarCargando(Me)
                Try
                    Using svc As New NovedadService()
                        For Each archivo In ofd.FileNames
                            Await svc.AddFotoAsync(_novedadGeneradaActual.Id, archivo)
                        Next
                    End Using
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

#Region "Gestión de Funcionarios y Novedades"

    Private Async Sub btnNuevaNovedad_Click(sender As Object, e As EventArgs) Handles btnNuevaNovedad.Click
        Using frm As New frmNovedadCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarNovedadesAsync() ' Recarga todas las novedades
            End If
        End Using
    End Sub


    ' --- INICIO DE LA CORRECCIÓN ---
    Private Async Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad de la lista.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesCompletas)
        Dim novedadId = novedadSeleccionada.Id

        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                If frm.FuncionarioSeleccionado IsNot Nothing Then
                    Dim funcionarioId = frm.FuncionarioSeleccionado.Id
                    Try
                        Using svc As New NovedadService()
                            Await svc.AgregarFuncionarioANovedadAsync(novedadId, funcionarioId)
                        End Using
                        Await MostrarDetalleNovedadSeleccionada()
                        ' Es necesario recargar la grilla principal porque ahora habrá una nueva fila
                        Await CargarNovedadesAsync(If(dtpFecha.Checked, dtpFecha.Value.Date, CType(Nothing, Date?)), If(dtpFecha.Checked, dtpFecha.Value.Date, CType(Nothing, Date?)))
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

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesCompletas)
        Dim funcionarioSeleccionado = CType(lstFuncionarios.SelectedItem, Funcionario)

        Dim novedadId = novedadSeleccionada.Id
        Dim funcionarioId = funcionarioSeleccionado.Id

        Dim confirmResult = MessageBox.Show($"¿Está seguro de que desea quitar a '{funcionarioSeleccionado.Nombre}' de esta novedad?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmResult = DialogResult.Yes Then
            Try
                Using svc As New NovedadService()
                    Await svc.QuitarFuncionarioDeNovedadAsync(novedadId, funcionarioId)
                End Using
                ' Recargamos todo para que la fila desaparezca de la grilla principal
                Await CargarNovedadesAsync(If(dtpFecha.Checked, dtpFecha.Value.Date, CType(Nothing, Date?)), If(dtpFecha.Checked, dtpFecha.Value.Date, CType(Nothing, Date?)))
            Catch ex As Exception
                MessageBox.Show("Error al quitar el funcionario: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    ' --- FIN DE LA CORRECCIÓN ---
#End Region

End Class