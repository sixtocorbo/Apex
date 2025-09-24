Option Strict On
Option Explicit On

Imports System.Threading

Public Class frmLicencias

    Private ReadOnly _licenciaSvc As New LicenciaService()
    Private WithEvents _searchTimer As New System.Windows.Forms.Timer()
    Private _isFirstLoad As Boolean = True
    Private _estaCargandoLicencias As Boolean = False

    ' --- NUEVO: CancellationTokenSource para cancelar búsquedas en curso ---
    Private _ctsBusqueda As CancellationTokenSource

    ' --- MEJORA: Variable para guardar el color original del textbox ---
    Private _colorOriginalBusqueda As Color

#Region "Ciclo de Vida del Formulario"

    Private Async Sub frmGestionLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        dgvLicencias.ActivarDobleBuffer(True)
        ConfigurarGrillaLicencias()

        ' MEJORA: Aumentamos el intervalo para que no se active tan rápido
        _searchTimer.Interval = 700
        _searchTimer.Enabled = False

        ' MEJORA: Guardamos el color original del textbox
        _colorOriginalBusqueda = txtBusquedaLicencia.BackColor

        txtBusquedaLicencia.Focus()
        AddHandler NotificadorEventos.FuncionarioActualizado, AddressOf OnFuncionarioActualizado

        dtpFechaVigencia.Value = Date.Today
        chkSoloVigentes.Checked = True
        dtpFechaVigencia.Enabled = chkSoloVigentes.Checked

        Try
            AppTheme.SetCue(txtBusquedaLicencia, "Buscar por funcionario…")
        Catch
        End Try

        ' --- PRIMERA CARGA CON TOKEN ---
        Dim tk = ReiniciarToken()
        Await CargarDatosLicenciasAsync(tk)
        _isFirstLoad = False
    End Sub

    Private Sub frmGestionLicencias_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.FuncionarioActualizado, AddressOf OnFuncionarioActualizado
        ' Limpieza del CTS
        Try
            _ctsBusqueda?.Cancel()
            _ctsBusqueda?.Dispose()
        Catch
        End Try
    End Sub

    Private Async Sub OnFuncionarioActualizado(sender As Object, e As FuncionarioCambiadoEventArgs)
        If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return

        Dim run As Func(Of Task) =
            Async Function()
                Dim tk = ReiniciarToken()
                Await CargarDatosLicenciasAsync(tk)
            End Function

        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(Async Sub() Await run()))
        Else
            Await run()
        End If
    End Sub

#End Region

#Region "Configuración y Carga de Datos"

    Private Sub ConfigurarGrillaLicencias()
        With dgvLicencias
            ' --- CONFIGURACIÓN GENERAL ---
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .GridColor = Color.FromArgb(230, 230, 230)
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AutoGenerateColumns = False
            .BackgroundColor = Color.White

            ' --- ESTILO DE ENCABEZADOS (Headers) ---
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

            ' --- ESTILO DE FILAS (Rows) ---
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247) ' Efecto Cebra

            ' --- DEFINICIÓN DE COLUMNAS ---
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "LicenciaId", .DataPropertyName = "LicenciaId", .Visible = False})

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "TipoLicencia", .DataPropertyName = "TipoLicencia", .HeaderText = "Tipo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .MinimumWidth = 180
        })

            Dim colInicio As New DataGridViewTextBoxColumn With {
            .Name = "FechaInicio", .DataPropertyName = "FechaInicio", .HeaderText = "Desde",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .MinimumWidth = 100
        }
            colInicio.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colInicio)

            Dim colFin As New DataGridViewTextBoxColumn With {
            .Name = "FechaFin", .DataPropertyName = "FechaFin", .HeaderText = "Hasta",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .MinimumWidth = 100
        }
            colFin.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colFin)

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "EstadoLicencia", .DataPropertyName = "EstadoLicencia", .HeaderText = "Estado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .MinimumWidth = 120
        })
        End With
    End Sub

    ' --- MÉTODO MODIFICADO ---
    ' Ahora acepta CancellationToken y cancela de forma silenciosa si corresponde.
    Private Async Function CargarDatosLicenciasAsync(token As CancellationToken) As Task
        If _estaCargandoLicencias Then Return
        _estaCargandoLicencias = True

        ' Inicia el feedback visual no bloqueante
        txtBusquedaLicencia.BackColor = Color.Gold
        dgvLicencias.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            token.ThrowIfCancellationRequested()

            Dim filtro = txtBusquedaLicencia.Text.Trim()
            Dim soloVigentes = (chkSoloVigentes IsNot Nothing AndAlso chkSoloVigentes.Checked)

            ' Pausa breve para que la UI se actualice antes de la tarea pesada (cancelable)
            Await Task.Delay(50, token)

            Dim datos As List(Of LicenciaConFuncionarioExtendidoDto)
            If soloVigentes Then
                Dim fechaVigencia = If(dtpFechaVigencia IsNot Nothing, dtpFechaVigencia.Value.Date, Date.Today)
                datos = Await _licenciaSvc.GetVigentesHoyAsync(filtroNombre:=filtro, fechaReferencia:=fechaVigencia).WaitAsync(token)
            Else
                datos = Await _licenciaSvc.GetAllConDetallesAsync(filtroNombre:=filtro).WaitAsync(token)
            End If

            token.ThrowIfCancellationRequested()

            dgvLicencias.DataSource = datos
            dgvLicencias.ClearSelection()

        Catch ex As OperationCanceledException
            ' Silenciar: búsqueda cancelada por una nueva.
            ' Este bloque ya captura TaskCanceledException también.

            '--- 👇 BLOQUE ELIMINADO 👇 ---
            ' Catch ex As TaskCanceledException
            '     ' Silenciar también.

        Catch ex As Exception
            Notifier.Error(Me, $"Error al cargar licencias: {ex.Message}")
            dgvLicencias.DataSource = Nothing
        Finally
            ' Restaura la UI a su estado normal si el form sigue vivo
            If Not Me.IsDisposed Then
                txtBusquedaLicencia.BackColor = _colorOriginalBusqueda
                dgvLicencias.Enabled = True
                Me.Cursor = Cursors.Default
            End If
            _estaCargandoLicencias = False
        End Try
    End Function

    Private Sub dgvLicencias_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvLicencias.RowPrePaint
        If e.RowIndex < 0 Then Return

        Dim row = Me.dgvLicencias.Rows(e.RowIndex)
        Dim dto = TryCast(row.DataBoundItem, LicenciaConFuncionarioExtendidoDto)
        If dto Is Nothing Then Return

        If Not dto.Activo Then
            ' Si está inactivo, siempre pintamos de rojo, sin importar si es par o impar.
            row.DefaultCellStyle.BackColor = Color.MistyRose
            row.DefaultCellStyle.ForeColor = Color.DarkRed
        Else
            ' Si está activo, restauramos el color que le corresponde por defecto (blanco o gris claro).
            ' Esto asegura que el efecto cebra se mantenga.
            If e.RowIndex Mod 2 = 0 Then
                row.DefaultCellStyle.BackColor = dgvLicencias.RowsDefaultCellStyle.BackColor ' Generalmente Blanco
            Else
                row.DefaultCellStyle.BackColor = dgvLicencias.AlternatingRowsDefaultCellStyle.BackColor ' Gris claro
            End If
            row.DefaultCellStyle.ForeColor = dgvLicencias.DefaultCellStyle.ForeColor ' Color de texto normal
        End If
    End Sub

#End Region

#Region "Helpers de Navegación"

    Private Function GetDashboard() As frmDashboard
        Return Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
    End Function

    Private Sub AbrirChildEnDashboard(formHijo As Form)
        If formHijo Is Nothing Then
            Notifier.Warn(Me, "No hay formulario para abrir.")
            Return
        End If

        Dim dash = GetDashboard()
        If dash Is Nothing OrElse dash.IsDisposed Then
            Notifier.Warn(Me, "No se encontró el Dashboard activo.")
            Return
        End If

        If dash.InvokeRequired Then
            dash.BeginInvoke(CType(Sub() AbrirChildEnDashboard(formHijo), MethodInvoker))
            Return
        End If

        Try
            dash.AbrirChild(formHijo)
        Catch ex As Exception
            Notifier.Error(dash, $"No se pudo abrir la ventana: {ex.Message}")
        End Try
    End Sub

#End Region

#Region "Búsqueda y Acciones"

    ' Reinicia el token cancelando el anterior.
    Private Function ReiniciarToken() As CancellationToken
        Try
            _ctsBusqueda?.Cancel()
            _ctsBusqueda?.Dispose()
        Catch
        End Try
        _ctsBusqueda = New CancellationTokenSource()
        Return _ctsBusqueda.Token
    End Function

    Private Sub txtBusquedaLicencia_TextChanged(sender As Object, e As EventArgs) Handles txtBusquedaLicencia.TextChanged
        _searchTimer.Stop()
        _searchTimer.Start()
    End Sub

    ' Dispara búsqueda con cancelación al vencer el timer.
    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs) Handles _searchTimer.Tick
        _searchTimer.Stop()
        Dim tk = ReiniciarToken()
        Await CargarDatosLicenciasAsync(tk)
        txtBusquedaLicencia.Focus()
    End Sub

    Private Async Sub txtBusquedaLicencia_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaLicencia.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            _searchTimer.Stop()
            Dim tk = ReiniciarToken()
            Await CargarDatosLicenciasAsync(tk)
            txtBusquedaLicencia.Focus()
        End If
    End Sub

    Private Async Sub chkSoloVigentes_CheckedChanged(sender As Object, e As EventArgs) Handles chkSoloVigentes.CheckedChanged
        If _isFirstLoad Then Return
        dtpFechaVigencia.Enabled = chkSoloVigentes.Checked
        Dim tk = ReiniciarToken()
        Await CargarDatosLicenciasAsync(tk)
    End Sub

    Private Async Sub dtpFechaVigencia_ValueChanged(sender As Object, e As EventArgs) Handles dtpFechaVigencia.ValueChanged
        If _isFirstLoad OrElse Not chkSoloVigentes.Checked Then Return
        Dim tk = ReiniciarToken()
        Await CargarDatosLicenciasAsync(tk)
    End Sub

    Private Sub btnNuevaLicencia_Click(sender As Object, e As EventArgs) Handles btnNuevaLicencia.Click
        AbrirChildEnDashboard(New frmLicenciaCrear())
    End Sub

    Private Sub btnEditarLicencia_Click(sender As Object, e As EventArgs) Handles btnEditarLicencia.Click
        EditarSeleccionada()
    End Sub

    Private Sub dgvLicencias_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLicencias.CellDoubleClick
        If e.RowIndex >= 0 Then
            EditarSeleccionada()
        End If
    End Sub

    Private Sub EditarSeleccionada()
        If dgvLicencias.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "Seleccioná una licencia para editar.")
            Return
        End If
        Dim dto = TryCast(dgvLicencias.CurrentRow.DataBoundItem, LicenciaConFuncionarioExtendidoDto)
        If dto Is Nothing OrElse Not dto.LicenciaId.HasValue Then
            Notifier.Warn(Me, "No se pudo determinar la licencia a editar.")
            Return
        End If

        AbrirChildEnDashboard(New frmLicenciaCrear(dto.LicenciaId.Value, dto.EstadoLicencia))
    End Sub

    Private Async Sub btnEliminarLicencia_Click(sender As Object, e As EventArgs) Handles btnEliminarLicencia.Click
        If dgvLicencias.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "Seleccioná una licencia para eliminar.")
            Return
        End If

        Dim dto = TryCast(dgvLicencias.CurrentRow.DataBoundItem, LicenciaConFuncionarioExtendidoDto)
        If dto Is Nothing OrElse Not dto.LicenciaId.HasValue Then
            Notifier.Warn(Me, "No se pudo determinar la licencia a eliminar.")
            Return
        End If

        Dim nombre = If(dto.NombreFuncionario, "(sin nombre)")
        If MessageBox.Show($"¿Eliminar la licencia de '{nombre}'?", "Confirmar",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        ' Se usa el feedback de carga no bloqueante también aquí para consistencia
        Dim oldCursor = Me.Cursor
        btnEliminarLicencia.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Await _licenciaSvc.DeleteAsync(dto.LicenciaId.Value)
            Notifier.Success(Me, "Licencia eliminada.")
            ' El evento OnFuncionarioActualizado se encargará de recargar los datos si es necesario
            NotificadorEventos.NotificarRefrescoTotal()
        Catch ex As Exception
            Notifier.Error(Me, $"Error al eliminar la licencia: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnEliminarLicencia.Enabled = True
        End Try
    End Sub

#End Region

End Class





