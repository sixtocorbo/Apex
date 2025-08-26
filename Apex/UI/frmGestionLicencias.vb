' Apex/UI/frmGestionLicencias.vb
Public Class frmGestionLicencias

    Private _licenciaSvc As New LicenciaService()

    Private Sub frmGestionLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaLicencias()
        txtBusquedaLicencia.Focus()
    End Sub

#Region "Configuración y Carga de Datos"

    Private Sub ConfigurarGrillaLicencias()
        With dgvLicencias
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoLicencia", .DataPropertyName = "TipoLicencia", .HeaderText = "Tipo", .Width = 180})

            Dim colInicio As New DataGridViewTextBoxColumn With {.Name = "FechaInicio", .DataPropertyName = "FechaInicio", .HeaderText = "Desde", .Width = 100}
            colInicio.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colInicio)

            Dim colFin As New DataGridViewTextBoxColumn With {.Name = "FechaFin", .DataPropertyName = "FechaFin", .HeaderText = "Hasta", .Width = 100}
            colFin.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colFin)

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
        End With
    End Sub

    Private Async Function CargarDatosLicenciasAsync() As Task
        Dim filtro = txtBusquedaLicencia.Text.Trim()
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvLicencias.DataSource = Nothing
            Return
        End If
        LoadingHelper.MostrarCargando(Me)
        Try
            dgvLicencias.DataSource = Nothing
            dgvLicencias.DataSource = Await _licenciaSvc.GetAllConDetallesAsync(filtroNombre:=filtro)
        Catch ex As Exception
            MessageBox.Show("Error al cargar licencias: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Manejo de Búsqueda"

    Private Async Sub txtBusquedaLicencia_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaLicencia.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await CargarDatosLicenciasAsync()
        End If
    End Sub

#End Region

#Region "Acciones para Licencias"


    Private Async Sub btnNuevaLicencia_Click(sender As Object, e As EventArgs) Handles btnNuevaLicencia.Click
        Using frm As New frmLicenciaCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosLicenciasAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditarLicencia_Click(sender As Object, e As EventArgs) Handles btnEditarLicencia.Click
        If dgvLicencias.CurrentRow Is Nothing Then Return
        Dim licenciaSeleccionada = CType(dgvLicencias.CurrentRow.DataBoundItem, vw_LicenciasCompletas)
        If licenciaSeleccionada Is Nothing Then Return
        Dim idSeleccionado = licenciaSeleccionada.Id
        Dim estadoActual = licenciaSeleccionada.Estado
        Using frm As New frmLicenciaCrear(idSeleccionado, estadoActual)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosLicenciasAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarLicencia_Click(sender As Object, e As EventArgs) Handles btnEliminarLicencia.Click
        If dgvLicencias.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvLicencias.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvLicencias.CurrentRow.Cells("NombreFuncionario").Value.ToString()
        If MessageBox.Show($"¿Está seguro de que desea eliminar la licencia para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _licenciaSvc.DeleteAsync(idSeleccionado)
                Await CargarDatosLicenciasAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la licencia: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region

End Class