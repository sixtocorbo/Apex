' Apex/UI/frmGestionLicencias.vb
Public Class frmGestionLicencias

    Private _licenciaSvc As New LicenciaService()

    ' --- INICIO DE LA MODIFICACIÓN #1: Conectar al Notificador de Eventos ---
    Private Async Sub frmGestionLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrillaLicencias()
        txtBusquedaLicencia.Focus()
        ' Nos suscribimos al evento para enterarnos de cambios hechos en otros formularios.
        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
        ' Forzamos una búsqueda inicial si hay texto en el cuadro de búsqueda.
        If Not String.IsNullOrWhiteSpace(txtBusquedaLicencia.Text) Then
            Await CargarDatosLicenciasAsync()
        End If
    End Sub

    ' Es una buena práctica desuscribirse para evitar fugas de memoria.
    Private Sub frmGestionLicencias_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
    End Sub

    ' Este método se disparará automáticamente cuando se guarde una licencia.
    Private Async Sub OnDatosActualizados(sender As Object, e As EventArgs)
        If Me.IsHandleCreated AndAlso Me.Visible Then
            Await CargarDatosLicenciasAsync()
        End If
    End Sub
    ' --- FIN DE LA MODIFICACIÓN #1 ---

#Region "Configuración y Carga de Datos"

    Private Sub ConfigurarGrillaLicencias()
        With dgvLicencias
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoLicencia", .DataPropertyName = "TipoLicencia", .HeaderText = "Tipo", .Width = 180})

            Dim colInicio As New DataGridViewTextBoxColumn With {.Name = "FechaInicio", .DataPropertyName = "FechaInicio", .HeaderText = "Desde", .Width = 100}
            colInicio.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colInicio)

            Dim colFin As New DataGridViewTextBoxColumn With {.Name = "FechaFin", .DataPropertyName = "FechaFin", .HeaderText = "Hasta", .Width = 100}
            colFin.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colFin)

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 120})
        End With
    End Sub

    Private Async Function CargarDatosLicenciasAsync() As Task
        Dim filtro = txtBusquedaLicencia.Text.Trim()
        ' Ahora solo no cargamos nada si el filtro está vacío.
        If String.IsNullOrWhiteSpace(filtro) Then
            dgvLicencias.DataSource = New List(Of vw_LicenciasCompletas)() ' Asignar lista vacía
            Return
        End If
        LoadingHelper.MostrarCargando(Me)
        Try
            dgvLicencias.DataSource = Nothing
            dgvLicencias.DataSource = Await _licenciaSvc.GetAllConDetallesAsync(filtroNombre:=filtro)
        Catch ex As Exception
            MessageBox.Show("Error al cargar licencias: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

#End Region

#Region "Acciones para Licencias"

    Private Sub btnNuevaLicencia_Click(sender As Object, e As EventArgs) Handles btnNuevaLicencia.Click
        ' --- INICIO DE LA CORRECCIÓN ---
        ' Abrimos el formulario como un diálogo modal.
        ' La ejecución se detendrá aquí hasta que el formulario frmLicenciaCrear se cierre.
        Dim frm As New frmLicenciaCrear()
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
        parentDashboard.AbrirFormEnPanel(frm)
    End Sub

    Private Sub btnEditarLicencia_Click(sender As Object, e As EventArgs) Handles btnEditarLicencia.Click
        If dgvLicencias.CurrentRow Is Nothing Then Return
        Dim licenciaSeleccionada = CType(dgvLicencias.CurrentRow.DataBoundItem, vw_LicenciasCompletas)
        If licenciaSeleccionada Is Nothing Then Return

        Dim idSeleccionado = licenciaSeleccionada.Id
        Dim estadoActual = licenciaSeleccionada.Estado

        ' --- INICIO DE LA CORRECCIÓN (consistencia) ---
        ' El modo de edición ya usaba ShowDialog, lo cual es correcto. Lo mantenemos.
        Using frm As New frmLicenciaCrear(idSeleccionado, estadoActual)
            frm.ShowDialog(Me)
        End Using
        ' --- FIN DE LA CORRECCIÓN ---
    End Sub

    Private Async Sub btnEliminarLicencia_Click(sender As Object, e As EventArgs) Handles btnEliminarLicencia.Click
        If dgvLicencias.CurrentRow Is Nothing Then Return
        Dim idSeleccionado = CInt(dgvLicencias.CurrentRow.Cells("Id").Value)
        Dim nombre = dgvLicencias.CurrentRow.Cells("NombreFuncionario").Value.ToString()

        If MessageBox.Show($"¿Está seguro de que desea eliminar la licencia para '{nombre}'?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                Await _licenciaSvc.DeleteAsync(idSeleccionado)
                ' Notificamos la eliminación para que todos los formularios se actualicen.
                NotificadorEventos.NotificarActualizacion()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la licencia: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

#End Region

End Class