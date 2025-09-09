' Apex/UI/frmNovedades.vb
Imports System.ComponentModel
Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Text

Public Class frmNovedades

    Private _bsNovedades As New BindingSource()
    Private _pictureBoxSeleccionado As PictureBox = Nothing
    Private _idNovedadSeleccionada As Integer?
    Private _funcionariosSeleccionadosFiltro As New Dictionary(Of Integer, String)

    ' --- 1. MODIFICAR EL CONSTRUCTOR Y AGREGAR UN MANEJADOR PARA EL EVENTO "DISPOSED" ---
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

        ' Carga las novedades al abrir el formulario usando los filtros por defecto
        Await BuscarAsync()
    End Sub

    Private Async Sub dgvNovedades_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNovedades.SelectionChanged
        Await ActualizarDetalleDesdeSeleccion()
    End Sub

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
        ' 1. Recopilar todos los criterios de búsqueda de la UI
        Dim textoBusqueda As String = txtBusqueda.Text.Trim()
        Dim fechaDesde As DateTime? = Nothing
        Dim fechaHasta As DateTime? = Nothing
        If chkFiltrarPorFecha.Checked Then
            fechaDesde = dtpFechaDesde.Value.Date
            ' Establece la hora al último instante del día para incluir todos los registros de esa fecha.
            fechaHasta = dtpFechaHasta.Value.Date.AddDays(1).AddTicks(-1)
        End If
        Dim idsFuncionarios As List(Of Integer) = _funcionariosSeleccionadosFiltro.Keys.ToList()

        ' 2. Mostrar feedback al usuario
        LoadingHelper.MostrarCargando(Me)
        btnBuscar.Enabled = False
        _bsNovedades.DataSource = Nothing
        LimpiarDetalles()

        Try
            ' 3. Llamar al NUEVO método de servicio que acepta todos los filtros
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

#Region "Gestión de Novedades (CRUD)"

    Private Sub btnNuevaNovedad_Click(sender As Object, e As EventArgs) Handles btnNuevaNovedad.Click
        Dim dashboard = Me.ParentForm
        If dashboard IsNot Nothing AndAlso TypeOf dashboard Is frmDashboard Then
            Dim formCrear As New frmNovedadCrear()
            CType(dashboard, frmDashboard).AbrirFormEnPanel(formCrear)
        Else
            Dim formCrear As New frmNovedadCrear()
            formCrear.Show()
        End If
    End Sub
    ' --- 3. AÑADIR EL NUEVO MÉTODO MANEJADOR ---
    ''' <summary>
    ''' Se ejecuta cuando NotificadorEventos informa un cambio en un funcionario.
    ''' </summary>
    Private Async Sub HandleFuncionarioActualizado(sender As Object, e As FuncionarioEventArgs)
        ' Para evitar que el filtro se llene con IDs de otras gestiones,
        ' lo limpiamos la primera vez que llega una notificación de este "lote".
        ' Una forma sencilla es verificar si el ID ya está en la lista. Si no hay ninguno, es un lote nuevo.
        If Not _funcionariosSeleccionadosFiltro.ContainsKey(e.FuncionarioId) Then
            ' Opcional: Si quieres que cada nueva gestión reemplace el filtro anterior, descomenta la siguiente línea:
            ' _funcionariosSeleccionadosFiltro.Clear()
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
    End Sub

    Private Async Sub btnEditarNovedad_Click(sender As Object, e As EventArgs) Handles btnEditarNovedad.Click
        If dgvNovedades.CurrentRow Is Nothing Then
            MessageBox.Show("Por favor, seleccione una novedad para editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim novedadId = CInt(dgvNovedades.CurrentRow.Cells("Id").Value)
        Using frm As New frmNovedadCrear(novedadId)
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await BuscarAsync()
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
                Await BuscarAsync()
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
                ' --- CORRECCIÓN AQUÍ ---
                ' Se accede al ID a través del FuncionarioSeleccionado
                Dim funcId = frm.FuncionarioSeleccionado.Id
                ' --- FIN DE LA CORRECCIÓN ---
                If funcId > 0 AndAlso Not _funcionariosSeleccionadosFiltro.ContainsKey(funcId) Then
                    Dim uow As New UnitOfWork()
                    Dim repo = uow.Repository(Of Funcionario)()
                    Dim funcionario = repo.GetById(funcId)
                    If funcionario IsNot Nothing Then
                        _funcionariosSeleccionadosFiltro.Add(funcionario.Id, funcionario.Nombre)
                        ActualizarListaFuncionarios()
                    End If
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
        ' 1. Verificamos si hay datos en la grilla para imprimir
        If _bsNovedades.DataSource Is Nothing OrElse _bsNovedades.Count = 0 Then
            MessageBox.Show("Primero debe generar un reporte para poder imprimirlo.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        LoadingHelper.MostrarCargando(Me)
        Try
            ' 2. Obtenemos la lista de datos DESDE el BindingSource
            Dim novedadesEnGrilla = CType(_bsNovedades.DataSource, List(Of vw_NovedadesAgrupadas))

            If novedadesEnGrilla Is Nothing OrElse Not novedadesEnGrilla.Any() Then
                MessageBox.Show("No hay datos para imprimir.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim novedadIds = novedadesEnGrilla.Select(Function(n) n.Id).ToList()

            ' 3. Buscamos los datos completos de las novedades desde el servicio
            Dim novedadService As New NovedadService()
            Dim datosCompletosParaReporte = Await novedadService.GetNovedadesCompletasByIds(novedadIds)

            ' 4. Mapeamos los datos para el reporte
            Dim datosMapeados = datosCompletosParaReporte.
            GroupBy(Function(n) n.Id).
            Select(Function(g) New With {
                .Id = g.Key,
                .Fecha = g.First().Fecha,
                .Texto = g.First().Texto,
                .Funcionarios = g.Select(Function(n) New With {
                    .NombreFuncionario = n.NombreFuncionario
                }).ToList()
            }).
            OrderByDescending(Function(n) n.Fecha).
            ToList()

            ' 5. Abrimos el formulario visor y le pasamos los datos
            Dim frm As New frmNovedadesRPT(datosMapeados)

            ' Usamos nuestro nuevo método de ayuda para manejar toda la lógica.
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)

        Catch ex As InvalidCastException
            MessageBox.Show("Ocurrió un error de datos internos al preparar la impresión.", "Error de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al preparar la impresión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub
#End Region

End Class