' Apex/UI/frmGestionLicencias.vb
Option Strict On
Option Explicit On

Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmLicencias

    Private ReadOnly _licenciaSvc As New LicenciaService()

    ' --- Suscripción al notificador ---
    Private Sub frmGestionLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaLicencias()
        txtBusquedaLicencia.Focus()

        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados

        ' Por defecto: mostrar licencias cursando hoy
        chkSoloVigentes.Checked = True ' disparará el CheckedChanged y cargará
    End Sub

    Private Sub frmGestionLicencias_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
    End Sub

    Private Async Sub OnDatosActualizados(sender As Object, e As EventArgs)
        ' Asegura refresco en el hilo de UI
        If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
            If Me.InvokeRequired Then
                Me.BeginInvoke(New Action(Async Sub() Await CargarDatosLicenciasAsync()))
            Else
                Await CargarDatosLicenciasAsync()
            End If
        End If
    End Sub
    ' --- fin notificador ---

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

            ' Id (oculto)
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "LicenciaId",
                .DataPropertyName = "LicenciaId",
                .Visible = False
            })

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "NombreFuncionario",
                .DataPropertyName = "NombreFuncionario",
                .HeaderText = "Funcionario",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            })

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "TipoLicencia",
                .DataPropertyName = "TipoLicencia",
                .HeaderText = "Tipo",
                .Width = 180
            })

            Dim colInicio As New DataGridViewTextBoxColumn With {
                .Name = "FechaInicio",
                .DataPropertyName = "FechaInicio",
                .HeaderText = "Desde",
                .Width = 100
            }
            colInicio.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colInicio)

            Dim colFin As New DataGridViewTextBoxColumn With {
                .Name = "FechaFin",
                .DataPropertyName = "FechaFin",
                .HeaderText = "Hasta",
                .Width = 100
            }
            colFin.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colFin)

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "EstadoLicencia",
                .DataPropertyName = "EstadoLicencia",
                .HeaderText = "Estado",
                .Width = 120
            })
        End With
    End Sub

    Private Async Function CargarDatosLicenciasAsync() As Task
        Dim filtro = txtBusquedaLicencia.Text.Trim()
        Dim soloVigentes = (chkSoloVigentes IsNot Nothing AndAlso chkSoloVigentes.Checked)

        LoadingHelper.MostrarCargando(Me)
        Try
            dgvLicencias.DataSource = Nothing
            Dim datos As List(Of LicenciaConFuncionarioExtendidoDto)

            If soloVigentes Then
                ' Mostrar sólo las licencias cursando hoy (con o sin filtro de nombre/CI)
                datos = Await _licenciaSvc.GetVigentesHoyAsync(filtroNombre:=filtro)
            Else
                ' Sin restricción de vigencia: usa tu consulta general (con o sin filtro)
                datos = Await _licenciaSvc.GetAllConDetallesAsync(filtroNombre:=filtro)
            End If

            dgvLicencias.DataSource = datos
        Catch ex As Exception
            MessageBox.Show("Error al cargar licencias: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

#End Region

#Region "Manejo de Búsqueda"

    Private Async Sub txtBusquedaLicencia_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBusquedaLicencia.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Await CargarDatosLicenciasAsync()
        End If
    End Sub

    Private Async Sub chkSoloVigentes_CheckedChanged(sender As Object, e As EventArgs) Handles chkSoloVigentes.CheckedChanged
        ' Al cambiar el check, recargar según corresponda
        Await CargarDatosLicenciasAsync()
    End Sub

#End Region

#Region "Acciones para Licencias"

    Private Sub btnNuevaLicencia_Click(sender As Object, e As EventArgs) Handles btnNuevaLicencia.Click
        ' Abrir en el panel del Dashboard (tu patrón actual)
        Dim frm As New frmLicenciaCrear()
        Dim parentDashboard As frmDashboard = TryCast(Me.ParentForm, frmDashboard)
        If parentDashboard IsNot Nothing Then
            parentDashboard.AbrirFormEnPanel(frm)
        Else
            ' Fallback modal si no está en dashboard
            Using frm
                frm.ShowDialog(Me)
            End Using
        End If
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
                NotificadorEventos.NotificarActualizacion()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la licencia: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            End Try
        End If
    End Sub

#End Region

End Class
