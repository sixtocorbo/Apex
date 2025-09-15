' Apex/UI/frmNovedades.vb
Imports System.ComponentModel
Imports System.Data.Entity
Imports System.IO

Public Class frmNovedades

    Private _bsNovedades As New BindingSource()
    Private _pictureBoxSeleccionado As PictureBox = Nothing
    Private _idNovedadSeleccionada As Integer?
    Private _funcionariosSeleccionadosFiltro As New Dictionary(Of Integer, String)
    Private _esPrimeraNotificacionRecibida As Boolean = True

    Public Sub New()
        ' Esta llamada es requerida por el diseñador.
        InitializeComponent()

    End Sub



    Private Async Sub frmNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        Me.AcceptButton = btnBuscar

        ConfigurarGrilla()
        dgvNovedades.DataSource = _bsNovedades

        dtpFechaDesde.Value = DateTime.Now.AddMonths(-1)
        dtpFechaHasta.Value = DateTime.Now
        Try
            AppTheme.SetCue(txtBusqueda, "Buscar por funcionario o novedad…")
        Catch
        End Try

        Try
            Await BuscarAsync()
            Notifier.Info(Me, "Tip: Enter busca y doble clic abre el detalle.")
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudieron cargar las novedades: {ex.Message}")
        End Try
    End Sub


    Private Async Sub dgvNovedades_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNovedades.SelectionChanged
        Await ActualizarDetalleDesdeSeleccion()
    End Sub

#Region "Suscripción a Eventos (Notificador)"

    Private Async Sub HandleFuncionarioActualizado(sender As Object, e As FuncionarioCambiadoEventArgs)
        If e Is Nothing Then Return

        ' Si viene sin Id -> refresco global (limpio filtro y busco)
        If Not e.FuncionarioId.HasValue Then
            _funcionariosSeleccionadosFiltro.Clear()
            ActualizarListaFuncionarios()
            Await BuscarAsync()
            Return
        End If

        ' Lote: si es el primero, limpio filtro anterior
        If _esPrimeraNotificacionRecibida Then
            _funcionariosSeleccionadosFiltro.Clear()
            _esPrimeraNotificacionRecibida = False
        End If

        Dim id = e.FuncionarioId.Value

        ' Agrego el funcionario afectado al filtro (si no está)
        Using uow As New UnitOfWork()
            Dim funcionario = Await uow.Repository(Of Funcionario).GetByIdAsync(id)
            If funcionario IsNot Nothing AndAlso Not _funcionariosSeleccionadosFiltro.ContainsKey(id) Then
                _funcionariosSeleccionadosFiltro.Add(funcionario.Id, funcionario.Nombre)
            End If
        End Using

        ' Refresco UI + búsqueda
        ActualizarListaFuncionarios()
        Await BuscarAsync()

        ' Al final del lote, rearmo la bandera
        Me.BeginInvoke(Sub() _esPrimeraNotificacionRecibida = True)
    End Sub

#End Region

#Region "Carga de Datos y Búsqueda"

    Private Sub ConfigurarGrilla()
        With dgvNovedades
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})

            Dim colFecha As New DataGridViewTextBoxColumn With {
            .Name = "Fecha", .DataPropertyName = "Fecha", .HeaderText = "Fecha", .Width = 110
        }
            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colFecha)

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Resumen", .DataPropertyName = "Resumen", .HeaderText = "Resumen de Novedad",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Funcionarios", .DataPropertyName = "Funcionarios", .HeaderText = "Funcionarios Involucrados",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        })
        End With
    End Sub


    Private Async Function BuscarAsync() As Task
        Dim textoBusqueda As String = txtBusqueda.Text.Trim()
        Dim fechaDesde As DateTime? = Nothing
        Dim fechaHasta As DateTime? = Nothing
        If chkFiltrarPorFecha.Checked Then
            fechaDesde = dtpFechaDesde.Value.Date
            fechaHasta = dtpFechaHasta.Value.Date.AddDays(1).AddTicks(-1)
        End If
        Dim idsFuncionarios As List(Of Integer) = _funcionariosSeleccionadosFiltro.Keys.ToList()

        LoadingHelper.MostrarCargando(Me)
        btnBuscar.Enabled = False
        _bsNovedades.DataSource = Nothing
        LimpiarDetalles()

        Try
            Using svc As New NovedadService()
                Dim lista = Await svc.BuscarNovedadesAvanzadoAsync(textoBusqueda, fechaDesde, fechaHasta, idsFuncionarios)
                _bsNovedades.DataSource = lista
                dgvNovedades.ClearSelection()
            End Using
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al buscar las novedades: {ex.Message}")
            _bsNovedades.DataSource = New List(Of vw_NovedadesAgrupadas)()
        Finally
            LoadingHelper.OcultarCargando(Me)
            btnBuscar.Enabled = True
            dgvNovedades.Focus()
        End Try
    End Function


    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Await BuscarAsync()
    End Sub

    Private Async Sub txtBusqueda_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusqueda.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await BuscarAsync()
        End If
    End Sub

#End Region

#Region "Lógica del Detalle"

    Private Sub LimpiarDetalles()
        txtTextoNovedad.Clear()
        lstFuncionarios.DataSource = Nothing
        flpFotos.Controls.Clear()
        _pictureBoxSeleccionado = Nothing
        _idNovedadSeleccionada = Nothing
    End Sub

    Private Sub ActualizarListaFuncionarios()
        lstFuncionariosFiltro.DataSource = Nothing
        If _funcionariosSeleccionadosFiltro.Any() Then
            lstFuncionariosFiltro.DataSource = New BindingSource(_funcionariosSeleccionadosFiltro, Nothing)
            lstFuncionariosFiltro.DisplayMember = "Value"
            lstFuncionariosFiltro.ValueMember = "Key"
        End If
    End Sub

    Private Async Function ActualizarDetalleDesdeSeleccion() As Task
        If dgvNovedades.CurrentRow Is Nothing OrElse dgvNovedades.CurrentRow.DataBoundItem Is Nothing Then
            LimpiarDetalles()
            Return
        End If

        Dim nov = TryCast(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        If nov Is Nothing Then
            LimpiarDetalles()
            Return
        End If
        If _idNovedadSeleccionada.HasValue AndAlso _idNovedadSeleccionada.Value = nov.Id Then Return

        _idNovedadSeleccionada = nov.Id
        Try
            Using svc As New NovedadService()
                Dim novedadCompleta = Await svc.GetByIdAsync(nov.Id)
                txtTextoNovedad.Text = If(novedadCompleta IsNot Nothing, novedadCompleta.Texto, "")

                Dim funcionarios = Await svc.GetFuncionariosPorNovedadAsync(nov.Id)
                lstFuncionarios.DataSource = funcionarios
                lstFuncionarios.DisplayMember = "Nombre"
                lstFuncionarios.ValueMember = "Id"

                Await CargarFotos(nov.Id)
            End Using
        Catch ex As Exception
            Notifier.[Error](Me, $"No se pudo cargar el detalle: {ex.Message}")
            LimpiarDetalles()
        End Try
    End Function
#End Region

#Region "Gestión de Fotos (Visualización)"
    ' Helper: crea una copia de la imagen para poder desechar el stream
    Private Function CrearImagenCopia(bytes() As Byte) As Image
        Using ms As New MemoryStream(bytes)
            Using bmp As New Bitmap(ms)
                Return New Bitmap(bmp)
            End Using
        End Using
    End Function

    ' Helper: limpia y libera imágenes previas
    Private Sub LimpiarFotos()
        For Each ctrl As Control In flpFotos.Controls
            Dim pic = TryCast(ctrl, PictureBox)
            If pic IsNot Nothing Then
                Dim img = pic.Image
                pic.Image = Nothing
                If img IsNot Nothing Then img.Dispose()
                pic.Dispose()
            End If
        Next
        flpFotos.Controls.Clear()
    End Sub

    Private Async Function CargarFotos(novedadId As Integer) As Task
        _pictureBoxSeleccionado = Nothing
        LimpiarFotos()
        Using svc As New NovedadService()
            Dim fotos = Await svc.GetFotosPorNovedadAsync(novedadId)
            For Each foto In fotos
                Dim pic As New PictureBox With {
                .Image = CrearImagenCopia(foto.Foto),
                .SizeMode = PictureBoxSizeMode.Zoom,
                .Size = New Size(120, 120),
                .Margin = New Padding(5),
                .BorderStyle = BorderStyle.FixedSingle,
                .Tag = foto.Id,
                .Cursor = Cursors.Hand
            }
                AddHandler pic.DoubleClick, AddressOf PictureBox_DoubleClick
                AddHandler pic.Click, AddressOf PictureBox_Click
                flpFotos.Controls.Add(pic)
            Next
        End Using
    End Function


    Private Sub PictureBox_Click(sender As Object, e As EventArgs)
        Dim picClickeado = TryCast(sender, PictureBox)
        If picClickeado Is Nothing Then Return

        If _pictureBoxSeleccionado IsNot Nothing Then
            _pictureBoxSeleccionado.BackColor = Color.Transparent
        End If
        picClickeado.BackColor = Color.DodgerBlue
        _pictureBoxSeleccionado = picClickeado
    End Sub

    Private Sub PictureBox_DoubleClick(sender As Object, e As EventArgs)
        Dim pic = TryCast(sender, PictureBox)
        If pic IsNot Nothing AndAlso pic.Image IsNot Nothing Then
            Using frm As New frmFotografiaNovedades(pic.Image)
                frm.ShowDialog(Me)
            End Using
        End If
    End Sub
#End Region

#Region "Gestión de Novedades (CRUD) y Filtros"

    Private Sub btnNuevaNovedad_Click(sender As Object, e As EventArgs) Handles btnNuevaNovedad.Click
        AbrirChildEnDashboard(New frmNovedadCrear())
    End Sub

    Private Sub btnEditarNovedad_Click(sender As Object, e As EventArgs) Handles btnEditarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "Seleccioná una novedad para editar.")
            Return
        End If
        Dim novedadId = CInt(dgvNovedades.CurrentRow.Cells("Id").Value)
        AbrirChildEnDashboard(New frmNovedadCrear(novedadId))
    End Sub

    Private Async Sub btnEliminarNovedad_Click(sender As Object, e As EventArgs) Handles btnEliminarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            Notifier.Warn(Me, "Seleccioná una novedad para eliminar.")
            Return
        End If

        Dim nov = TryCast(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        If nov Is Nothing Then
            Notifier.Warn(Me, "No se pudo leer la novedad seleccionada.")
            Return
        End If

        If MessageBox.Show($"¿Eliminar la novedad del {nov.Fecha:d}?",
                       "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
            Return
        End If

        Dim oldCursor = Me.Cursor
        btnEliminarNovedad.Enabled = False
        LoadingHelper.MostrarCargando(Me)
        Me.Cursor = Cursors.WaitCursor

        Try
            Using svc As New NovedadService()
                Await svc.DeleteNovedadCompletaAsync(nov.Id)
            End Using
            Notifier.Info(Me, "Novedad eliminada.")
            Await BuscarAsync()
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al eliminar la novedad: {ex.Message}")
        Finally
            LoadingHelper.OcultarCargando(Me)
            Me.Cursor = oldCursor
            btnEliminarNovedad.Enabled = True
        End Try
    End Sub

    Private Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Dim funcId = frm.FuncionarioSeleccionado.Id
                If funcId > 0 AndAlso Not _funcionariosSeleccionadosFiltro.ContainsKey(funcId) Then
                    _funcionariosSeleccionadosFiltro.Add(funcId, frm.FuncionarioSeleccionado.Nombre)
                    ActualizarListaFuncionarios()
                    Notifier.Success(Me, "Funcionario agregado al filtro.")
                End If
            End If
        End Using
    End Sub

    Private Sub btnLimpiarFuncionarios_Click(sender As Object, e As EventArgs) Handles btnLimpiarFuncionarios.Click
        _funcionariosSeleccionadosFiltro.Clear()
        ActualizarListaFuncionarios()
        Notifier.Info(Me, "Filtro de funcionarios limpiado.")
    End Sub

    Private Sub btnQuitarFuncionario_Click(sender As Object, e As EventArgs) Handles btnQuitarFuncionario.Click
        If lstFuncionariosFiltro.SelectedItem IsNot Nothing Then
            Dim selectedFuncId = CType(lstFuncionariosFiltro.SelectedValue, Integer)
            _funcionariosSeleccionadosFiltro.Remove(selectedFuncId)
            ActualizarListaFuncionarios()
            Notifier.Info(Me, "Funcionario quitado del filtro.")
        End If
    End Sub

    Private Async Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        If _bsNovedades.DataSource Is Nothing OrElse _bsNovedades.Count = 0 Then
            Notifier.Warn(Me, "Primero buscá novedades para poder imprimir.")
            Return
        End If

        Dim novedadesEnGrilla = TryCast(_bsNovedades.DataSource, List(Of vw_NovedadesAgrupadas))
        If novedadesEnGrilla Is Nothing OrElse Not novedadesEnGrilla.Any() Then
            Notifier.Warn(Me, "No hay datos para imprimir.")
            Return
        End If

        Dim novedadIds = novedadesEnGrilla.Select(Function(n) n.Id).ToList()

        LoadingHelper.MostrarCargando(Me)
        Try
            Using service As New NovedadService()
                Dim datosParaReporte = Await service.GetNovedadesParaReporteAsync(novedadIds)
                If datosParaReporte IsNot Nothing AndAlso datosParaReporte.Any() Then
                    AbrirChildEnDashboard(New frmNovedadesRPT(datosParaReporte))
                    Notifier.Info(Me, "Abriendo reporte de novedades…")
                Else
                    Notifier.Warn(Me, "No se encontraron detalles para imprimir.")
                End If
            End Using
        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al preparar la impresión: {ex.Message}")
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub

#End Region
    ' ==== Helpers de navegación (pila del Dashboard) ====
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

        Try
            dash.AbrirChild(formHijo)
            Notifier.Success(dash, $"Abierto: {formHijo.Text}")
        Catch ex As Exception
            Notifier.[Error](dash, $"No se pudo abrir la ventana: {ex.Message}")
        End Try
    End Sub
    Private Async Sub frmNovedades_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter AndAlso Not btnBuscar.Focused Then
            btnBuscar.PerformClick()
            e.Handled = True
        ElseIf e.KeyCode = Keys.F5 Then
            Await BuscarAsync()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Delete Then
            btnEliminarNovedad.PerformClick()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub



End Class