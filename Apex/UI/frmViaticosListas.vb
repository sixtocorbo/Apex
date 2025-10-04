Imports System.Data
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
            ' --- CONFIGURACIÓN GENERAL ---
            .BorderStyle = BorderStyle.None ' Sin borde exterior 3D.
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal ' Solo líneas horizontales, estilo moderno.
            .GridColor = Color.FromArgb(230, 230, 230) ' Un gris claro para las líneas.
            .RowHeadersVisible = False ' Ya lo tenías, es una buena práctica.
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect ' También correcto.
            .AllowUserToResizeRows = False ' Evita que el usuario desajuste las alturas.
            .AutoGenerateColumns = False ' Fundamental.
            .BackgroundColor = Color.White ' Fondo blanco si no hay suficientes filas.

            ' --- ESTILO DE ENCABEZADOS (Headers) ---
            .EnableHeadersVisualStyles = False ' Permite usar nuestros estilos personalizados.
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None ' Sin borde en el encabezado.
            .ColumnHeadersHeight = 40 ' Más altura para un look más espaciado.
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56) ' Un color oscuro y profesional.
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White ' Texto blanco para contraste.
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold) ' Fuente legible y en negrita.
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft ' Alineación a la izquierda.

            ' --- ESTILO DE FILAS (Rows) ---
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.0F) ' Fuente estándar para las celdas.
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0) ' Espaciado interno en las celdas.
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255) ' Color de selección moderno.
            .DefaultCellStyle.SelectionForeColor = Color.White ' Texto blanco al seleccionar.
            .RowsDefaultCellStyle.BackColor = Color.White ' Fondo de filas.
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247) ' "Efecto Cebra" para fácil lectura.

            ' --- DEFINICIÓN DE COLUMNAS (Ajustadas para mejor tamaño y alineación) ---
            .Columns.Clear()

            ' Grado (corto)
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Grado", .HeaderText = "Grado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleLeft}
        })
            ' Cédula (numérico, pero puede tratarse como texto)
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Cedula", .HeaderText = "Cédula",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleLeft}
        })
            ' Funcionario (ocupa el espacio restante)
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, ' Correcto para ocupar el resto.
            .MinimumWidth = 200 ' Ancho mínimo para que no se comprima demasiado.
        })
            ' Sección
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Seccion", .HeaderText = "Sección",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        })
            ' Días a Pagar (numérico, alineado al centro)
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "DiasAPagar", .HeaderText = "Días",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
        })
            ' Motivo (ancho mínimo para que no se corte de inmediato)
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Motivo", .HeaderText = "Motivo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, ' Usaremos ancho fijo pero con un mínimo.
            .Width = 200
        })
            ' Observaciones
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Observaciones", .HeaderText = "Observaciones",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            .Width = 250
        })

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

    Private Sub btnVerReporte_Click(sender As Object, e As EventArgs) Handles btnVerReporte.Click
        If _bsViaticos.Count = 0 Then
            Notifier.Info(Me, "No hay datos para mostrar en el informe.")
            Return
        End If

        Dim dtReporte As DataTable = Nothing
        Dim vista = TryCast(_bsViaticos.List, DataView)

        If vista IsNot Nothing Then
            dtReporte = vista.ToTable()
        Else
            Dim dtOriginal = TryCast(_bsViaticos.DataSource, DataTable)
            If dtOriginal IsNot Nothing Then
                dtReporte = dtOriginal.Copy()
            End If
        End If

        If dtReporte Is Nothing OrElse dtReporte.Rows.Count = 0 Then
            Notifier.Info(Me, "No hay datos para mostrar en el informe.")
            Return
        End If

        Dim visor = AbrirHijoEnDashboard(Of frmViaticosRPT)(
            Sub(f)
                f.Preparar(dtpPeriodo.Value, dtReporte)
                AddHandler f.FormClosed, AddressOf Visor_FormClosed
            End Sub)

        If visor Is Nothing Then
            Return
        End If
    End Sub

    Private Sub Visor_FormClosed(sender As Object, e As FormClosedEventArgs)
        Dim visor = TryCast(sender, Form)
        If visor IsNot Nothing Then
            RemoveHandler visor.FormClosed, AddressOf Visor_FormClosed
        End If

        VolverAListadoFuncionarios()
    End Sub

    Private Sub VolverAListadoFuncionarios()
        Dim dash = GetDashboard()

        If dash Is Nothing OrElse dash.IsDisposed Then
            If Not Me.IsDisposed Then
                Try
                    Me.Close()
                Catch
                End Try
            End If
            Return
        End If

        Dim accion As MethodInvoker = Sub()
                                          Try
                                              If Not Me.IsDisposed Then
                                                  Me.Close()
                                              End If
                                          Catch
                                          End Try

                                          Try
                                              dash.btnBuscarFuncionario.PerformClick()
                                          Catch ex As Exception
                                              Notifier.Warn(dash, "No se pudo volver al listado de funcionarios.")
                                          End Try
                                      End Sub

        If dash.InvokeRequired Then
            dash.BeginInvoke(accion)
        Else
            accion()
        End If
    End Sub

    Private Function GetDashboard() As frmDashboard
        Return Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
    End Function

    Private Function AbrirHijoEnDashboard(Of TForm As {Form, New})(Optional configurar As Action(Of TForm) = Nothing) As TForm
        Dim dash = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()

        If dash Is Nothing OrElse dash.IsDisposed Then
            MessageBox.Show("No se encontró el Dashboard activo.", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return Nothing
        End If

        Dim formHijo As TForm

        Try
            formHijo = New TForm()
        Catch ex As Exception
            Notifier.Error(Me, $"No se pudo crear la ventana: {ex.Message}")
            Return Nothing
        End Try

        If configurar IsNot Nothing Then
            Try
                configurar(formHijo)
            Catch ex As Exception
                formHijo.Dispose()
                Notifier.Error(Me, $"No se pudo preparar la ventana: {ex.Message}")
                Return Nothing
            End Try
        End If

        Try
            dash.AbrirChild(formHijo)
        Catch ex As Exception
            formHijo.Dispose()
            Notifier.Error(dash, $"No se pudo abrir la ventana: {ex.Message}")
            Return Nothing
        End Try

        Return formHijo
    End Function

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
