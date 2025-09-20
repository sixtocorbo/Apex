Imports System.Threading
Imports System.Linq ' <-- AÑADIDO: Necesario para el método .Any()

Public Class frmViaticosListas

    Private _bsViaticos As New BindingSource()

    ' --- Variables para la carga asíncrona ---
    Private _ctsBusqueda As CancellationTokenSource
    Private _estaBuscando As Boolean = False

    Private Sub frmViaticosListas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Try
            AppTheme.SetCue(txtFiltroDinamico, "Filtrar por Nombre, Cédula, Motivo o Sección...")
        Catch
        End Try
        ConfigurarGrilla()
        dtpPeriodo.Value = Date.Today.AddMonths(-1)
        dgvResultados.DataSource = _bsViaticos

        ' Carga inicial al abrir el formulario
        btnGenerar.PerformClick()
    End Sub

    Private Sub frmGestionViaticos_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
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

#Region "Lógica de Carga Asíncrona"

    Private Function ReiniciarToken() As CancellationToken
        _ctsBusqueda?.Cancel()
        _ctsBusqueda?.Dispose()
        _ctsBusqueda = New CancellationTokenSource()
        Return _ctsBusqueda.Token
    End Function

    Private Async Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Dim tk = ReiniciarToken()
        Await CargarViaticosAsync(tk)
    End Sub

    Private Async Function CargarViaticosAsync(token As CancellationToken) As Task
        If _estaBuscando Then Return
        _estaBuscando = True

        Me.Cursor = Cursors.WaitCursor
        btnGenerar.Enabled = False
        dgvResultados.Enabled = False
        _bsViaticos.DataSource = Nothing

        Try
            token.ThrowIfCancellationRequested()
            Dim periodoSeleccionado = dtpPeriodo.Value
            Await Task.Delay(50, token)

            ' --- CORREGIDO: Usar el tipo de dato correcto 'ViaticoResultadoDTO' ---
            Dim resultados As List(Of ViaticoResultadoDTO)
            Using svc As New ViaticoService()
                resultados = Await svc.CalcularLiquidacionAsync(periodoSeleccionado).WaitAsync(token)
            End Using

            token.ThrowIfCancellationRequested()

            _bsViaticos.DataSource = resultados.ToDataTable()
            txtFiltroDinamico.Clear()
            _bsViaticos.Filter = ""

            If Not resultados.Any() Then
                Notifier.Info(Me, "No se encontraron viáticos para el período seleccionado.")
            End If

        Catch ex As OperationCanceledException
            ' Carga cancelada. Silencioso.
        Catch ex As Exception
            Notifier.Error(Me, "Ocurrió un error al generar el reporte: " & ex.Message)
        Finally
            If Not Me.IsDisposed Then
                Me.Cursor = Cursors.Default
                btnGenerar.Enabled = True
                dgvResultados.Enabled = True
                ActualizarContador()
            End If
            _estaBuscando = False
        End Try
    End Function

#End Region

#Region "Filtrado en Memoria (Cliente)"

    Private Sub txtFiltroDinamico_TextChanged(sender As Object, e As EventArgs) Handles txtFiltroDinamico.TextChanged
        Dim textoFiltro = txtFiltroDinamico.Text.Trim()
        If String.IsNullOrWhiteSpace(textoFiltro) Then
            _bsViaticos.Filter = ""
        Else
            Dim textoFiltroSeguro = textoFiltro.Replace("'", "''")
            Dim filtro = $"NombreFuncionario LIKE '%{textoFiltroSeguro}%' OR " &
                         $"Cedula LIKE '%{textoFiltroSeguro}%' OR " &
                         $"Motivo LIKE '%{textoFiltroSeguro}%' OR " &
                         $"Seccion LIKE '%{textoFiltroSeguro}%'"
            Try
                _bsViaticos.Filter = filtro
            Catch ex As Exception
                _bsViaticos.Filter = ""
            End Try
        End If
        ActualizarContador()
    End Sub

    Private Sub ActualizarContador()
        lblRegistros.Text = $"Registros: {_bsViaticos.Count}"
    End Sub

#End Region

End Class