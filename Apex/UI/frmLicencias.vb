Imports System.Data.Entity

Public Class frmLicencias

    Private _svc As LicenciaService
    Private _listaLicencias As List(Of LicenciaService.LicenciaParaVista)

    Private Async Sub frmLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New LicenciaService()
        ConfigurarGrilla()
        Await CargarCombosAsync()
        Await CargarLicenciasAsync()
    End Sub

    Private Async Function CargarCombosAsync() As Task
        Dim tipos = Await _svc.ObtenerTiposLicenciaParaComboAsync()
        ' Añadir un item "Todos" al principio de la lista
        tipos.Insert(0, New KeyValuePair(Of Integer, String)(0, "[Todos los Tipos]"))
        cboTipoLicencia.DataSource = tipos
        cboTipoLicencia.DisplayMember = "Value"
        cboTipoLicencia.ValueMember = "Key"
    End Function

    Private Sub ConfigurarGrilla()
        With dgvLicencias
            .AutoGenerateColumns = False
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "CI", .DataPropertyName = "CI", .HeaderText = "C.I.", .Width = 100})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoLicencia", .DataPropertyName = "TipoLicencia", .HeaderText = "Tipo de Licencia", .Width = 200})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "FechaInicio", .DataPropertyName = "FechaInicio", .HeaderText = "Desde", .Width = 120, .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "FechaFin", .DataPropertyName = "FechaFin", .HeaderText = "Hasta", .Width = 120, .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 100})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Comentario", .DataPropertyName = "Comentario", .HeaderText = "Comentario", .Width = 250})
        End With
    End Sub

    Private Async Function CargarLicenciasAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim filtroNombre = txtBusqueda.Text.Trim()
            Dim tipoId = CType(cboTipoLicencia.SelectedValue, Integer?)
            Dim desde = If(chkDesde.Checked, CType(dtpDesde.Value.Date, Date?), Nothing)
            Dim hasta = If(chkHasta.Checked, CType(dtpHasta.Value.Date, Date?), Nothing)

            _listaLicencias = Await _svc.GetAllConDetallesAsync(filtroNombre, tipoId, desde, hasta)
            dgvLicencias.DataSource = _listaLicencias

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al cargar las licencias: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Await CargarLicenciasAsync()
    End Sub

    Private Sub chkDesde_CheckedChanged(sender As Object, e As EventArgs) Handles chkDesde.CheckedChanged
        dtpDesde.Enabled = chkDesde.Checked
    End Sub

    Private Sub chkHasta_CheckedChanged(sender As Object, e As EventArgs) Handles chkHasta.CheckedChanged
        dtpHasta.Enabled = chkHasta.Checked
    End Sub

    ' Lógica para botones de Nueva, Editar, Eliminar Licencia (requiere frmLicenciaCrear)
    ' Private Async Sub btnNueva_Click(sender As Object, e As EventArgs) Handles btnNueva.Click
    '     Using frm As New frmLicenciaCrear()
    '         If frm.ShowDialog() = DialogResult.OK Then
    '             Await CargarLicenciasAsync()
    '         End If
    '     End Using
    ' End Sub

End Class