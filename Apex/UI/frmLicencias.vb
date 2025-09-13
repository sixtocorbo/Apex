Option Strict On
Option Explicit On

Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmLicencias

    Private ReadOnly _licenciaSvc As New LicenciaService()
    ' --- NUEVO: Timer para controlar la búsqueda ---
    Private WithEvents _searchTimer As New Timer()
    Private _isFirstLoad As Boolean = True

    ' --- Suscripción al notificador ---
    Private Sub frmGestionLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaLicencias()

        ' --- Configuración del Timer ---
        _searchTimer.Interval = 500 ' Medio segundo de espera antes de buscar
        _searchTimer.Enabled = False

        txtBusquedaLicencia.Focus()
        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
        chkSoloVigentes.Checked = True
        _isFirstLoad = False
        Try
            AppTheme.SetCue(txtBusquedaLicencia, "Buscar por funcionario…")
        Catch
            ' Ignorar si no existe SetCue
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

    ' --- SOLUCIÓN: Añadir una variable a nivel de clase para controlar el estado de carga ---
    Private _estaCargandoLicencias As Boolean = False

    Private Async Function CargarDatosLicenciasAsync() As Task
        ' Si ya hay una operación de carga en curso, no hacer nada y salir.
        If _estaCargandoLicencias Then Return

        Try
            ' Marcar que la carga ha comenzado.
            _estaCargandoLicencias = True

            Dim filtro = txtBusquedaLicencia.Text.Trim()
            Dim soloVigentes = (chkSoloVigentes IsNot Nothing AndAlso chkSoloVigentes.Checked)

            LoadingHelper.MostrarCargando(Me)
            ' Opcional: Deshabilitar controles de UI que puedan disparar el evento de nuevo
            ' txtBusquedaLicencia.Enabled = False
            ' chkSoloVigentes.Enabled = False

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
            ' Marcar que la carga ha finalizado, permitiendo futuras cargas.
            _estaCargandoLicencias = False

            ' Opcional: Volver a habilitar los controles de UI
            ' txtBusquedaLicencia.Enabled = True
            ' chkSoloVigentes.Enabled = True
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Manejo de Búsqueda y Acciones"

    ' --- CAMBIO: Se utiliza TextChanged en lugar de KeyDown para mayor fluidez ---
    Private Sub txtBusquedaLicencia_TextChanged(sender As Object, e As EventArgs) Handles txtBusquedaLicencia.TextChanged
        ' Reinicia el temporizador cada vez que el usuario escribe algo.
        _searchTimer.Stop()
        _searchTimer.Start()
    End Sub

    ' --- NUEVO: El Timer ejecuta la búsqueda cuando el usuario deja de escribir ---
    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs) Handles _searchTimer.Tick
        _searchTimer.Stop() ' Detiene el timer para que no se ejecute repetidamente.
        Await CargarDatosLicenciasAsync()
    End Sub

    Private Async Sub chkSoloVigentes_CheckedChanged(sender As Object, e As EventArgs) Handles chkSoloVigentes.CheckedChanged
        ' Evitamos que se ejecute durante la carga inicial del formulario
        If _isFirstLoad Then Return
        Await CargarDatosLicenciasAsync()
    End Sub

    Private Sub btnNuevaLicencia_Click(sender As Object, e As EventArgs) Handles btnNuevaLicencia.Click
        Dim frm As New frmLicenciaCrear()
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
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
        Using frm As New frmLicenciaCrear(dto.LicenciaId.Value, dto.EstadoLicencia)
            frm.ShowDialog(Me)
        End Using
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