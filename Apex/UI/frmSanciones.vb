Imports System.Data.Entity

Public Class frmSanciones

    ' --- CORRECCIÓN 1: Usar el servicio de licencias ---
    Private _licenciaSvc As New LicenciaService()
    Private _tipoLicenciaSvc As New GenericService(Of TipoLicencia)(New UnitOfWork())
    Private WithEvents SearchTimer As New System.Windows.Forms.Timer()

    Private Async Sub frmGestionSanciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaSanciones()

        Await CargarCombosAsync()
        Await CargarDatosSancionesAsync() ' <-- AÑADIDO: Carga los datos al abrir el formulario

        ' Configura el temporizador para la búsqueda automática
        SearchTimer.Interval = 500 ' 500 ms de espera
        AddHandler txtBusquedaSancion.TextChanged, AddressOf IniciarTemporizador
        AddHandler cmbTipoLicencia.SelectedIndexChanged, AddressOf IniciarTemporizador
        AddHandler SearchTimer.Tick, AddressOf Temporizador_Tick
        Try
            AppTheme.SetCue(txtBusquedaSancion, "Buscar por nombre de funcionario...")
            AppTheme.SetCue(cmbTipoLicencia, "Filtrar por tipo de sanción...")

        Catch
            ' Ignorar si no existe SetCue
        End Try
        txtBusquedaSancion.Focus()
    End Sub

#Region "Configuración y Carga de Datos"

    Private Async Function CargarCombosAsync() As Task
        Dim todosLosTipos As List(Of TipoLicencia) = Await _tipoLicenciaSvc.GetAllAsync()
        Dim tiposLicencia = todosLosTipos.Where(Function(tl) _
    tl.CategoriaAusenciaId = ModConstantesApex.CategoriaAusenciaId.SancionGrave OrElse
    tl.CategoriaAusenciaId = ModConstantesApex.CategoriaAusenciaId.SancionLeve
).ToList()

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

            ' --- CORRECCIÓN 2: Ajustar los DataPropertyName a la vista vw_LicenciasCompletas ---
            Dim colDesde As New DataGridViewTextBoxColumn With {.Name = "FechaDesde", .DataPropertyName = "FechaInicio", .HeaderText = "Desde", .Width = 100}
            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colDesde)

            Dim colHasta As New DataGridViewTextBoxColumn With {.Name = "FechaHasta", .DataPropertyName = "FechaFin", .HeaderText = "Hasta", .Width = 100}
            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colHasta)

            ' La columna 'Resolucion' no existe en HistoricoLicencia, la reemplazamos por 'Estado'
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Observaciones", .DataPropertyName = "Comentario", .HeaderText = "Observaciones", .Width = 200})
        End With
    End Sub

    Private Async Function CargarDatosSancionesAsync() As Task
        Dim filtro = txtBusquedaSancion.Text.Trim()
        Dim tipoLicenciaId As Integer? = Nothing

        If cmbTipoLicencia.SelectedValue IsNot Nothing AndAlso CInt(cmbTipoLicencia.SelectedValue) > 0 Then
            tipoLicenciaId = CInt(cmbTipoLicencia.SelectedValue)
        End If

        LoadingHelper.MostrarCargando(Me)
        Try
            dgvSanciones.DataSource = Nothing
            ' Llama al servicio de licencias para obtener las que son sanciones
            dgvSanciones.DataSource = Await _licenciaSvc.GetSancionesAsync(filtro, tipoLicenciaId)
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

    Private Sub btnNuevaSancion_Click(sender As Object, e As EventArgs) Handles btnNuevaSancion.Click
        AbrirChildEnDashboard(New frmLicenciaCrear())
    End Sub


    Private Sub btnEditarSancion_Click(sender As Object, e As EventArgs) Handles btnEditarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then Return

        ' --- CORRECCIÓN 1: Asegurarse de que el tipo de dato es el correcto ---
        Dim sancionSeleccionada = TryCast(dgvSanciones.CurrentRow.DataBoundItem, vw_LicenciasCompletas)
        If sancionSeleccionada Is Nothing Then Return

        Dim idSeleccionado = sancionSeleccionada.Id
        Dim estadoActual = sancionSeleccionada.Estado

        AbrirChildEnDashboard(New frmLicenciaCrear(idSeleccionado, estadoActual))

    End Sub

    Private Async Sub btnEliminarSancion_Click(sender As Object, e As EventArgs) Handles btnEliminarSancion.Click
        If dgvSanciones.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvSanciones.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvSanciones.CurrentRow.Cells("NombreFuncionario").Value.ToString()
        If MessageBox.Show($"¿Está seguro de que desea eliminar la sanción para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                ' --- CORRECCIÓN 3: Usar el servicio de licencias para eliminar ---
                Await _licenciaSvc.DeleteAsync(idSeleccionado)
                Await CargarDatosSancionesAsync()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la sanción: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region
    ' ==== Helpers de navegación (pila del Dashboard) ====
    Private Function GetDashboard() As frmDashboard
        Return Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
    End Function

    Private Sub AbrirChildEnDashboard(formHijo As Form)
        Dim dash = GetDashboard()
        If dash Is Nothing Then
            MessageBox.Show("No se encontró el Dashboard activo.", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        dash.AbrirChild(formHijo) ' ← usa la pila (Opción A)
    End Sub

End Class