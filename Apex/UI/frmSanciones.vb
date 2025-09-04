' Apex/UI/frmSanciones.vb
Imports System.Data.Entity

Public Class frmSanciones

    Private _sancionSvc As New SancionService()
    Private _tipoLicenciaSvc As New GenericService(Of TipoLicencia)(New UnitOfWork())
    Private WithEvents SearchTimer As New System.Windows.Forms.Timer()

    Private Async Sub frmGestionSanciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaSanciones()

        Await CargarCombosAsync()

        ' Configura el temporizador para la búsqueda automática
        SearchTimer.Interval = 500 ' 500 ms de espera
        AddHandler txtBusquedaSancion.TextChanged, AddressOf IniciarTemporizador
        AddHandler cmbTipoLicencia.SelectedIndexChanged, AddressOf IniciarTemporizador
        AddHandler SearchTimer.Tick, AddressOf Temporizador_Tick

        txtBusquedaSancion.Focus()
    End Sub

#Region "Configuración y Carga de Datos"

    Private Async Function CargarCombosAsync() As Task
        Const CATEGORIA_SANCION_ID As Integer = 3

        ' --- CORRECCIÓN CLAVE: Se ajusta la forma de llamar al servicio ---
        ' 1. Obtenemos el IQueryable (la base de la consulta)
        Dim query = _tipoLicenciaSvc.GetAll()

        ' 2. Construimos la consulta y la ejecutamos con ToListAsync
        Dim tiposLicencia = Await query.Where(Function(tl) tl.CategoriaAusenciaId = CATEGORIA_SANCION_ID) _
                                     .ToListAsync()
        ' --- FIN DE LA CORRECCIÓN ---

        ' Añade un item para "Todos" al principio
        Dim listaConTodos = tiposLicencia.OrderBy(Function(x) x.Nombre).ToList()
        listaConTodos.Insert(0, New TipoLicencia With {.Id = 0, .Nombre = "[TODOS]"})

        cmbTipoLicencia.DataSource = listaConTodos
        cmbTipoLicencia.DisplayMember = "Nombre"
        cmbTipoLicencia.ValueMember = "Id"
    End Function

    Private Sub ConfigurarGrillaSanciones()
        With dgvSanciones
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})

            Dim colDesde As New DataGridViewTextBoxColumn With {.Name = "FechaDesde", .DataPropertyName = "FechaDesde", .HeaderText = "Desde", .Width = 100}
            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colDesde)

            Dim colHasta As New DataGridViewTextBoxColumn With {.Name = "FechaHasta", .DataPropertyName = "FechaHasta", .HeaderText = "Hasta", .Width = 100}
            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colHasta)

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Resolucion", .DataPropertyName = "Resolucion", .HeaderText = "Resolución", .Width = 120})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Observaciones", .DataPropertyName = "Observaciones", .HeaderText = "Observaciones", .Width = 200})
        End With
    End Sub

    Private Async Function CargarDatosSancionesAsync() As Task
        Dim filtro = txtBusquedaSancion.Text.Trim()
        Dim tipoLicenciaId As Integer? = Nothing

        If cmbTipoLicencia.SelectedValue IsNot Nothing AndAlso CInt(cmbTipoLicencia.SelectedValue) > 0 Then
            tipoLicenciaId = CInt(cmbTipoLicencia.SelectedValue)
        End If

        If String.IsNullOrWhiteSpace(filtro) AndAlso Not tipoLicenciaId.HasValue Then
            dgvSanciones.DataSource = Nothing
            Return
        End If

        LoadingHelper.MostrarCargando(Me)
        Try
            dgvSanciones.DataSource = Nothing
            dgvSanciones.DataSource = Await _sancionSvc.GetAllConDetallesAsync(filtro, Nothing, Nothing, tipoLicenciaId)
        Catch ex As Exception
            MessageBox.Show("Error al cargar sanciones: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Manejo de Búsqueda"

    Private Sub IniciarTemporizador(sender As Object, e As EventArgs)
        SearchTimer.Stop()
        SearchTimer.Start()
    End Sub

    Private Async Sub Temporizador_Tick(sender As Object, e As EventArgs)
        SearchTimer.Stop()
        Await CargarDatosSancionesAsync()
    End Sub

    Private Async Sub txtBusquedaSancion_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaSancion.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            SearchTimer.Stop()
            Await CargarDatosSancionesAsync()
        End If
    End Sub

#End Region

#Region "Acciones para Sanciones"

    Private Async Sub btnNuevaSancion_Click(sender As Object, e As EventArgs) Handles btnNuevaSancion.Click
        Using frm As New frmSancionCrear()
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosSancionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEditarSancion_Click(sender As Object, e As EventArgs) Handles btnEditarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then Return
        Dim sancionSeleccionada = CType(dgvSanciones.CurrentRow.DataBoundItem, vw_SancionesCompletas)
        If sancionSeleccionada Is Nothing Then Return

        Dim idSeleccionado = sancionSeleccionada.Id
        Using frm As New frmSancionCrear()
            frm.SancionId = idSeleccionado
            If frm.ShowDialog(Me) = DialogResult.OK Then
                Await CargarDatosSancionesAsync()
            End If
        End Using
    End Sub

    Private Async Sub btnEliminarSancion_Click(sender As Object, e As EventArgs) Handles btnEliminarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvSanciones.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvSanciones.CurrentRow.Cells("NombreFuncionario").Value.ToString()
        If MessageBox.Show($"¿Está seguro de que desea eliminar la sanción para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _sancionSvc.DeleteAsync(idSeleccionado)
                Await CargarDatosSancionesAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la sanción: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region

End Class