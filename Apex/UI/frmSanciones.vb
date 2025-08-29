' Apex/UI/frmGestionSanciones.vb
Public Class frmSanciones

    Private _sancionSvc As New SancionService()

    Private Sub frmGestionSanciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaSanciones()
        txtBusquedaSancion.Focus()
    End Sub

#Region "Configuración y Carga de Datos"

    Private Sub ConfigurarGrillaSanciones()
        With dgvSanciones
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})

            Dim colDesde As New DataGridViewTextBoxColumn With {.Name = "FechaDesde", .DataPropertyName = "FechaDesde", .HeaderText = "Desde", .Width = 100}
            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colDesde)

            Dim colHasta As New DataGridViewTextBoxColumn With {.Name = "FechaHasta", .DataPropertyName = "FechaHasta", .HeaderText = "Hasta", .Width = 100}
            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colHasta)

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Resolucion", .DataPropertyName = "Resolucion", .HeaderText = "Resolución", .Width = 120})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Observaciones", .DataPropertyName = "Observaciones", .HeaderText = "Observaciones", .Width = 200})
        End With
    End Sub

    Private Async Function CargarDatosSancionesAsync() As Task
        Dim filtro = txtBusquedaSancion.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvSanciones.DataSource = Nothing
            Return
        End If
        LoadingHelper.MostrarCargando(Me)
        Try
            dgvSanciones.DataSource = Nothing
            dgvSanciones.DataSource = Await _sancionSvc.GetAllConDetallesAsync(filtro)
        Catch ex As Exception
            MessageBox.Show("Error al cargar sanciones: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Manejo de Búsqueda"

    Private Async Sub txtBusquedaSancion_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaSancion.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await CargarDatosSancionesAsync()
        End If
    End Sub

#End Region

#Region "Acciones para Sanciones"

    Private Async Sub btnNuevaSancion_Click(sender As Object, e As EventArgs) Handles btnNuevaSancion.Click
        Using frm As New frmSancionCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosSancionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditarSancion_Click(sender As Object, e As EventArgs) Handles btnEditarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then Return
        Dim sancionSeleccionada = CType(dgvSanciones.CurrentRow.DataBoundItem, vw_SancionesCompletas)
        If sancionSeleccionada Is Nothing Then Return

        Dim idSeleccionado = sancionSeleccionada.Id
        Using frm As New frmSancionCrear()
            frm.SancionId = idSeleccionado
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosSancionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarSancion_Click(sender As Object, e As EventArgs) Handles btnEliminarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvSanciones.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvSanciones.CurrentRow.Cells("NombreFuncionario").Value.ToString()
        If MessageBox.Show($"¿Está seguro de que desea eliminar la sanción para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _sancionSvc.DeleteAsync(idSeleccionado)
                Await CargarDatosSancionesAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la sanción: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region

End Class