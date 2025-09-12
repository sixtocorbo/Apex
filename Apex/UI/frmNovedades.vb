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

        ' Nos suscribimos al evento del notificador cuando se crea el formulario.
        AddHandler NotificadorEventos.FuncionarioActualizado, AddressOf HandleFuncionarioActualizado
    End Sub

    Private Sub frmNovedades_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        ' Es MUY IMPORTANTE desuscribirse del evento para evitar fugas de memoria.
        RemoveHandler NotificadorEventos.FuncionarioActualizado, AddressOf HandleFuncionarioActualizado
    End Sub

    Private Async Sub frmNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        dgvNovedades.DataSource = _bsNovedades

        ' Configuración inicial de los filtros
        dtpFechaDesde.Value = DateTime.Now.AddMonths(-1)
        dtpFechaHasta.Value = DateTime.Now
        Try
            AppTheme.SetCue(txtBusqueda, "Buscar por funcionario o novedad…")
        Catch
            ' Ignorar si no existe SetCue
        End Try
        ' Carga las novedades al abrir el formulario usando los filtros por defecto
        Await BuscarAsync()
    End Sub

    Private Async Sub dgvNovedades_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNovedades.SelectionChanged
        Await ActualizarDetalleDesdeSeleccion()
    End Sub

#Region "Suscripción a Eventos (Notificador)"

    ''' <summary>
    ''' Se ejecuta cuando NotificadorEventos informa un cambio en un funcionario.
    ''' Filtra la grilla para mostrar las novedades del funcionario afectado.
    ''' </summary>
    Private Async Sub HandleFuncionarioActualizado(sender As Object, e As FuncionarioEventArgs)
        ' Usamos una bandera para saber cuándo empezar un nuevo filtro.
        ' Si es la primera notificación de un "lote", limpiamos el filtro anterior.
        If _esPrimeraNotificacionRecibida Then
            _funcionariosSeleccionadosFiltro.Clear()
            _esPrimeraNotificacionRecibida = False
        End If

        ' Agregamos el funcionario de la notificación al filtro.
        Using uow As New UnitOfWork()
            Dim funcionario = Await uow.Repository(Of Funcionario).GetByIdAsync(e.FuncionarioId)
            If funcionario IsNot Nothing AndAlso Not _funcionariosSeleccionadosFiltro.ContainsKey(e.FuncionarioId) Then
                _funcionariosSeleccionadosFiltro.Add(funcionario.Id, funcionario.Nombre)
            End If
        End Using

        ' Actualizamos la UI y disparamos la búsqueda.
        ActualizarListaFuncionarios()
        Await BuscarAsync()

        ' Rearmamos la bandera para la próxima vez que se guarde una novedad.
        ' Usamos BeginInvoke para que se ejecute después de procesar todas las notificaciones pendientes del lote actual.
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
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Fecha", .DataPropertyName = "Fecha", .HeaderText = "Fecha", .Width = 100})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Resumen", .DataPropertyName = "Resumen", .HeaderText = "Resumen de Novedad", .Width = 300})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Funcionarios", .DataPropertyName = "Funcionarios", .HeaderText = "Funcionarios Involucrados", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
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
                _bsNovedades.DataSource = Await svc.BuscarNovedadesAvanzadoAsync(textoBusqueda, fechaDesde, fechaHasta, idsFuncionarios)
            End Using
        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al buscar las novedades: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        If _idNovedadSeleccionada.HasValue AndAlso _idNovedadSeleccionada.Value = novedadSeleccionada.Id Then
            Return
        End If

        _idNovedadSeleccionada = novedadSeleccionada.Id
        Using svc As New NovedadService()
            Dim novedadCompleta = Await svc.GetByIdAsync(novedadSeleccionada.Id)
            txtTextoNovedad.Text = If(novedadCompleta IsNot Nothing, novedadCompleta.Texto, "")
            Dim funcionarios = Await svc.GetFuncionariosPorNovedadAsync(novedadSeleccionada.Id)
            lstFuncionarios.DataSource = funcionarios
            lstFuncionarios.DisplayMember = "Nombre"
            lstFuncionarios.ValueMember = "Id"
            Await CargarFotos(novedadSeleccionada.Id)
        End Using
    End Function
#End Region

#Region "Gestión de Fotos (Visualización)"

    Private Async Function CargarFotos(novedadId As Integer) As Task
        _pictureBoxSeleccionado = Nothing
        flpFotos.Controls.Clear()
        Using svc As New NovedadService()
            Dim fotos = Await svc.GetFotosPorNovedadAsync(novedadId)
            For Each foto In fotos
                Dim pic As New PictureBox With {
                    .Image = Image.FromStream(New MemoryStream(foto.Foto)),
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
        ' El NavegacionHelper se encarga de abrir el formulario de forma desacoplada.
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(New frmNovedadCrear())
    End Sub

    Private Sub btnEditarNovedad_Click(sender As Object, e As EventArgs) Handles btnEditarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim novedadId = CInt(dgvNovedades.CurrentRow.Cells("Id").Value)
        Using frm As New frmNovedadCrear(novedadId)
            ' Aquí ShowDialog es correcto porque la edición es una acción modal que debe completarse.
            If frm.ShowDialog(Me) = DialogResult.OK Then
                ' No necesitamos llamar a NotificadorEventos aquí, porque el formulario de edición ya lo hace.
                ' La actualización se recibirá a través de HandleFuncionarioActualizado.
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarNovedad_Click(sender As Object, e As EventArgs) Handles btnEliminarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim novedadSeleccionada = CType(dgvNovedades.CurrentRow.DataBoundItem, vw_NovedadesAgrupadas)
        Dim confirmResult = MessageBox.Show($"¿Está seguro de que desea eliminar la novedad del {novedadSeleccionada.Fecha:d}?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

        If confirmResult = DialogResult.Yes Then
            LoadingHelper.MostrarCargando(Me)
            Try
                Using svc As New NovedadService()
                    Await svc.DeleteNovedadCompletaAsync(novedadSeleccionada.Id)
                End Using
                Await BuscarAsync() ' Refrescamos la lista después de eliminar.
            Catch ex As Exception
                MessageBox.Show("Ocurrió un error al eliminar la novedad: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                LoadingHelper.OcultarCargando(Me)
            End Try
        End If
    End Sub

    Private Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Dim funcId = frm.FuncionarioSeleccionado.Id
                If funcId > 0 AndAlso Not _funcionariosSeleccionadosFiltro.ContainsKey(funcId) Then
                    _funcionariosSeleccionadosFiltro.Add(frm.FuncionarioSeleccionado.Id, frm.FuncionarioSeleccionado.Nombre)
                    ActualizarListaFuncionarios()
                End If
            End If
        End Using
    End Sub

    Private Sub btnLimpiarFuncionarios_Click(sender As Object, e As EventArgs) Handles btnLimpiarFuncionarios.Click
        _funcionariosSeleccionadosFiltro.Clear()
        ActualizarListaFuncionarios()
    End Sub

    Private Sub btnQuitarFuncionario_Click(sender As Object, e As EventArgs) Handles btnQuitarFuncionario.Click
        If lstFuncionariosFiltro.SelectedItem IsNot Nothing Then
            Dim selectedFuncId = CType(lstFuncionariosFiltro.SelectedValue, Integer)
            _funcionariosSeleccionadosFiltro.Remove(selectedFuncId)
            ActualizarListaFuncionarios()
        End If
    End Sub

    Private Async Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        ' 1. Validar que haya datos en la grilla para imprimir.
        If _bsNovedades.DataSource Is Nothing OrElse _bsNovedades.Count = 0 Then
            Notifier.Warn(Me, "Primero debe realizar una búsqueda para poder imprimir.")
            Return
        End If

        ' 2. Obtener los IDs de las novedades que se muestran actualmente.
        Dim novedadesEnGrilla = CType(_bsNovedades.DataSource, List(Of vw_NovedadesAgrupadas))
        If novedadesEnGrilla Is Nothing OrElse Not novedadesEnGrilla.Any() Then
            Notifier.Warn(Me, "No hay datos para imprimir.")
            Return
        End If
        Dim novedadIds = novedadesEnGrilla.Select(Function(n) n.Id).ToList()


        LoadingHelper.MostrarCargando(Me)
        Try
            ' --- INICIO DE LA CORRECCIÓN CLAVE ---

            ' 3. Llamar al NUEVO método del servicio que devuelve los DTOs jerárquicos.
            Using service As New NovedadService()
                Dim datosParaReporte = Await service.GetNovedadesParaReporteAsync(novedadIds)

                If datosParaReporte IsNot Nothing AndAlso datosParaReporte.Any() Then
                    ' 4. Pasar la lista de DTOs (el tipo de dato correcto) al formulario del reporte.
                    Using frm As New frmNovedadesRPT(datosParaReporte)
                        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
                    End Using
                Else
                    Notifier.Warn(Me, "No se encontraron detalles para las novedades seleccionadas.")
                End If
            End Using

            ' --- FIN DE LA CORRECCIÓN CLAVE ---

        Catch ex As Exception
            Notifier.Error(Me, $"Ocurrió un error al preparar la impresión: {ex.Message}")
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub
#End Region

End Class