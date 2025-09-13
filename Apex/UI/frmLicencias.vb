Option Strict On
Option Explicit On

Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmLicencias

    Private ReadOnly _licenciaSvc As New LicenciaService()
    ' --- Timer para controlar la búsqueda ---
    Private WithEvents _searchTimer As New Timer()
    Private _isFirstLoad As Boolean = True

    ' --- LOAD / UNLOAD ---
    Private Sub frmGestionLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaLicencias()

        _searchTimer.Interval = 500
        _searchTimer.Enabled = False

        txtBusquedaLicencia.Focus()
        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
        chkSoloVigentes.Checked = True
        _isFirstLoad = False
        Try
            AppTheme.SetCue(txtBusquedaLicencia, "Buscar por funcionario…")
        Catch
        End Try
    End Sub

    Private Sub frmGestionLicencias_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
    End Sub

    Private Async Sub OnDatosActualizados(sender As Object, e As EventArgs)
        If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
            If Me.InvokeRequired Then
                Me.BeginInvoke(New Action(Async Sub() Await CargarDatosLicenciasAsync()))
            Else
                Await CargarDatosLicenciasAsync()
            End If
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
        Try
            _estaCargandoLicencias = True

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
        Catch ex As Exception
            MessageBox.Show("Error al cargar licencias: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _estaCargandoLicencias = False
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Helpers de navegación (pila del Dashboard)"

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

#End Region

#Region "Búsqueda y Acciones"

    Private Sub txtBusquedaLicencia_TextChanged(sender As Object, e As EventArgs) Handles txtBusquedaLicencia.TextChanged
        _searchTimer.Stop()
        _searchTimer.Start()
    End Sub

    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs) Handles _searchTimer.Tick
        _searchTimer.Stop()
        Await CargarDatosLicenciasAsync()
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
        If dgvLicencias.CurrentRow Is Nothing Then Return
        Dim dto = TryCast(dgvLicencias.CurrentRow.DataBoundItem, LicenciaConFuncionarioExtendidoDto)
        If dto Is Nothing OrElse Not dto.LicenciaId.HasValue Then
            MessageBox.Show("No se pudo determinar la licencia a editar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Antes: ShowDialog(Me)
        ' Ahora: abrir como child; al cerrar, el Dashboard restaura frmLicencias
        AbrirChildEnDashboard(New frmLicenciaCrear(dto.LicenciaId.Value, dto.EstadoLicencia))
    End Sub

    Private Async Sub btnEliminarLicencia_Click(sender As Object, e As EventArgs) Handles btnEliminarLicencia.Click
        If dgvLicencias.CurrentRow Is Nothing Then Return
        Dim dto = TryCast(dgvLicencias.CurrentRow.DataBoundItem, LicenciaConFuncionarioExtendidoDto)
        If dto Is Nothing OrElse Not dto.LicenciaId.HasValue Then
            MessageBox.Show("No se pudo determinar la licencia a eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim nombre = If(dto.NombreFuncionario, "(sin nombre)")
        If MessageBox.Show($"¿Eliminar la licencia de '{nombre}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _licenciaSvc.DeleteAsync(dto.LicenciaId.Value)
                NotificadorEventos.NotificarActualizacionGeneral()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la licencia: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region

End Class
