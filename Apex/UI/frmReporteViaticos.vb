' Apex/UI/frmGestionViaticos.vb
Public Class frmReporteViaticos

    Private _viaticoService As New ViaticoService()
    ' --- INICIO DE LA MODIFICACIÓN ---
    ' Usaremos un BindingSource para manejar el filtrado de manera eficiente.
    Private _bsViaticos As New BindingSource()
    ' --- FIN DE LA MODIFICACIÓN ---

    Private Sub frmGestionViaticos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        ' Por defecto, mostramos el mes anterior al actual
        dtpPeriodo.Value = Date.Today.AddMonths(-1)
        ' Enlazar la grilla al BindingSource
        dgvResultados.DataSource = _bsViaticos
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

            ' --- INICIO DE LA MODIFICACIÓN ---
            ' Convertimos la lista a una tabla y la asignamos al BindingSource
            _bsViaticos.DataSource = resultados.ToDataTable()
            ' Limpiamos cualquier filtro anterior
            txtFiltroDinamico.Clear()
            _bsViaticos.Filter = ""
            ' --- FIN DE LA MODIFICACIÓN ---

            If Not resultados.Any() Then
                MessageBox.Show("No se encontraron registros de viáticos para el período seleccionado.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al generar el reporte de viáticos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
            ActualizarContador()
        End Try
    End Sub

    ' --- INICIO: NUEVOS MÉTODOS PARA EL FILTRADO DINÁMICO ---
    Private Sub txtFiltroDinamico_TextChanged(sender As Object, e As EventArgs) Handles txtFiltroDinamico.TextChanged
        Dim textoFiltro = txtFiltroDinamico.Text.Trim()

        If String.IsNullOrWhiteSpace(textoFiltro) Then
            _bsViaticos.Filter = ""
        Else
            ' Construimos una consulta para el RowFilter del BindingSource.
            ' El carácter '%' actúa como comodín.
            Dim filtro = $"NombreFuncionario LIKE '%{textoFiltro}%' OR " &
                         $"Cedula LIKE '%{textoFiltro}%' OR " &
                         $"Motivo LIKE '%{textoFiltro}%' OR " &
                         $"Seccion LIKE '%{textoFiltro}%'"
            _bsViaticos.Filter = filtro
        End If

        ActualizarContador()
    End Sub

    Private Sub ActualizarContador()
        lblRegistros.Text = $"Registros: {_bsViaticos.Count}"
    End Sub
    ' --- FIN: NUEVOS MÉTODOS ---
End Class