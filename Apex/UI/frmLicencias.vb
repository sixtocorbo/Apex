Option Strict On
Option Explicit On

Public Class frmLicencias

    Private ReadOnly _licenciaSvc As New LicenciaService()
    ' --- Timer para controlar la búsqueda ---
    Private WithEvents _searchTimer As New Timer()
    Private _isFirstLoad As Boolean = True

    ' --- LOAD / UNLOAD ---
    ' Cambia "Private Sub" por "Private Async Sub"
    Private Async Sub frmGestionLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaLicencias()

        _searchTimer.Interval = 500
        _searchTimer.Enabled = False

        txtBusquedaLicencia.Focus()
        AddHandler NotificadorEventos.FuncionarioActualizado, AddressOf OnFuncionarioActualizado

        ' Esto está bien, lo dejas como está
        chkSoloVigentes.Checked = True
        _isFirstLoad = False

        Try
            AppTheme.SetCue(txtBusquedaLicencia, "Buscar por funcionario…")
        Catch
        End Try

        ' Notificación opcional, puedes quitarla si quieres
        Notifier.Info(Me, "Cargando licencias vigentes...")

        ' --- LÍNEA AÑADIDA ---
        ' Esta es la llamada que faltaba para cargar los datos
        Await CargarDatosLicenciasAsync()
    End Sub


    Private Sub frmGestionLicencias_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.FuncionarioActualizado, AddressOf OnFuncionarioActualizado
    End Sub

    Private Async Sub OnFuncionarioActualizado(sender As Object, e As FuncionarioCambiadoEventArgs)
        If Me.IsDisposed OrElse Not Me.IsHandleCreated Then Return

        ' Si querés que solo refresque ante cambios globales:
        ' If e Is Nothing OrElse Not e.FuncionarioId.HasValue Then
        '     ' refresco global
        ' Else
        '     Return ' ignorar cambios puntuales
        ' End If

        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(Async Sub() Await CargarDatosLicenciasAsync()))
        Else
            Await CargarDatosLicenciasAsync()
        End If
    End Sub

#Region "Configuración y Carga de Datos"

    Private Sub ConfigurarGrillaLicencias()
        With dgvLicencias
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "LicenciaId", .DataPropertyName = "LicenciaId", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoLicencia", .DataPropertyName = "TipoLicencia", .HeaderText = "Tipo", .Width = 180})
            Dim colInicio As New DataGridViewTextBoxColumn With {.Name = "FechaInicio", .DataPropertyName = "FechaInicio", .HeaderText = "Desde", .Width = 100}
            colInicio.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colInicio)
            Dim colFin As New DataGridViewTextBoxColumn With {.Name = "FechaFin", .DataPropertyName = "FechaFin", .HeaderText = "Hasta", .Width = 100}
            colFin.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colFin)
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "EstadoLicencia", .DataPropertyName = "EstadoLicencia", .HeaderText = "Estado", .Width = 120})
        End With
    End Sub

    Private _estaCargandoLicencias As Boolean = False

    Private Async Function CargarDatosLicenciasAsync() As Task
        If _estaCargandoLicencias Then Return
        _estaCargandoLicencias = True
        Try
            Dim filtro = txtBusquedaLicencia.Text.Trim()
            Dim soloVigentes = (chkSoloVigentes IsNot Nothing AndAlso chkSoloVigentes.Checked)

            LoadingHelper.MostrarCargando(Me)
            dgvLicencias.DataSource = Nothing

            Dim datos As List(Of LicenciaConFuncionarioExtendidoDto)
            If soloVigentes Then
                datos = Await _licenciaSvc.GetVigentesHoyAsync(filtroNombre:=filtro)
            Else
                datos = Await _licenciaSvc.GetAllConDetallesAsync(filtroNombre:=filtro)
            End If

            dgvLicencias.DataSource = datos
            dgvLicencias.ClearSelection()

        Catch ex As Exception
            Notifier.[Error](Me, $"Error al cargar licencias: {ex.Message}")
            dgvLicencias.DataSource = Nothing
        Finally
            _estaCargandoLicencias = False
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function
    Private Sub dgvLicencias_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles dgvLicencias.RowPrePaint
        ' Asegurarse de que no es una fila de encabezado
        If e.RowIndex < 0 Then Return

        ' Obtener el objeto de datos enlazado a la fila
        Dim row = Me.dgvLicencias.Rows(e.RowIndex)
        Dim dto = TryCast(row.DataBoundItem, LicenciaConFuncionarioExtendidoDto)

        If dto IsNot Nothing Then
            ' Si el funcionario está inactivo, pintar la fila.
            If Not dto.Activo Then
                row.DefaultCellStyle.BackColor = Color.MistyRose
                row.DefaultCellStyle.ForeColor = Color.DarkRed
            Else
                ' IMPORTANTE: Si no está inactivo, restaurar los colores por defecto.
                ' Esto evita que el color se "pegue" a otras filas al hacer scroll.
                row.DefaultCellStyle.BackColor = dgvLicencias.DefaultCellStyle.BackColor
                row.DefaultCellStyle.ForeColor = dgvLicencias.DefaultCellStyle.ForeColor
            End If
        End If
    End Sub


#End Region

#Region "Helpers de navegación (pila del Dashboard)"
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
            Notifier.Success(dash, $"Abierto: {formHijo.Text}")
        Catch ex As Exception
            Notifier.[Error](dash, $"No se pudo abrir la ventana: {ex.Message}")
        End Try
    End Sub

#End Region

#Region "Búsqueda y Acciones"

    Private Sub txtBusquedaLicencia_TextChanged(sender As Object, e As EventArgs) Handles txtBusquedaLicencia.TextChanged
        _searchTimer.Stop()
        _searchTimer.Start()
    End Sub

    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs) Handles _searchTimer.Tick
        _searchTimer.Stop()
        Await CargarDatosLicenciasAsync()
        Notifier.Info(Me, "Búsqueda actualizada.")
    End Sub


    Private Async Sub chkSoloVigentes_CheckedChanged(sender As Object, e As EventArgs) Handles chkSoloVigentes.CheckedChanged
        If _isFirstLoad Then Return
        Await CargarDatosLicenciasAsync()
    End Sub

    Private Sub btnNuevaLicencia_Click(sender As Object, e As EventArgs) Handles btnNuevaLicencia.Click
        ' Antes: NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
        ' Ahora: abrir como child para que al cerrar vuelva a esta pantalla
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

        Dim oldCursor = Me.Cursor
        btnEliminarLicencia.Enabled = False
        Me.Cursor = Cursors.WaitCursor

        Try
            Await _licenciaSvc.DeleteAsync(dto.LicenciaId.Value)
            Notifier.Info(Me, "Licencia eliminada.")
            NotificadorEventos.NotificarRefrescoTotal()
            Await CargarDatosLicenciasAsync()
        Catch ex As Exception
            Notifier.[Error](Me, $"Error al eliminar la licencia: {ex.Message}")
        Finally
            Me.Cursor = oldCursor
            btnEliminarLicencia.Enabled = True
        End Try
    End Sub


#End Region

End Class
