' Apex/UI/frmGestionViaticos.vb
Public Class frmGestionViaticos

    Private _viaticoService As New ViaticoService()

    Private Sub frmGestionViaticos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        ' Por defecto, mostramos el mes anterior al actual
        dtpPeriodo.Value = Date.Today.AddMonths(-1)
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvResultados
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Grado", .HeaderText = "Grado", .Width = 60})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Cedula", .HeaderText = "Cédula", .Width = 90})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Seccion", .HeaderText = "Sección", .Width = 80})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "DiasAPagar", .HeaderText = "Días a Pagar", .Width = 80})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Motivo", .HeaderText = "Motivo", .Width = 150})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Observaciones", .HeaderText = "Obs.", .Width = 150})
        End With
    End Sub

    Private Async Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim periodoSeleccionado = dtpPeriodo.Value
            Dim resultados = Await _viaticoService.CalcularLiquidacionAsync(periodoSeleccionado)
            dgvResultados.DataSource = resultados

            If Not resultados.Any() Then
                MessageBox.Show("No se encontraron registros de viáticos para el período seleccionado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al generar el reporte de viáticos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub

End Class