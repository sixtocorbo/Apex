' Apex/UI/frmLicencias.vb
Imports System.Data.Entity

Public Class frmLicencias

    Private _svc As LicenciaService
    Private _listaLicencias As List(Of LicenciaService.LicenciaParaVista)

    ' --- Variables para paginación ---
    Private _paginaActual As Integer = 1
    Private _tamañoPagina As Integer = 50 ' Puedes ajustar este número
    Private _totalRegistros As Integer = 0

    Private Async Sub frmLicencias_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New LicenciaService()
        ConfigurarGrilla()
        Await CargarCombosAsync()
        Await CargarLicenciasAsync()
        ' --- Añadir manejadores para los nuevos botones ---
        AddHandler btnAnterior.Click, AddressOf btnAnterior_Click
        AddHandler btnSiguiente.Click, AddressOf btnSiguiente_Click
    End Sub

    Private Async Function CargarCombosAsync() As Task
        Dim tipos = Await _svc.ObtenerTiposLicenciaParaComboAsync()
        ' Añadir un item "Todos" al principio de la lista
        tipos.Insert(0, New KeyValuePair(Of Integer, String)(0, "[Todos los Tipos]"))
        cboTipoLicencia.DataSource = tipos
        cboTipoLicencia.DisplayMember = "Value"
        cboTipoLicencia.ValueMember = "Key"
    End Function

    Private Sub ConfigurarGrilla()
        With dgvLicencias
            .AutoGenerateColumns = False
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "CI", .DataPropertyName = "CI", .HeaderText = "C.I.", .Width = 100})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "NombreFuncionario", .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "TipoLicencia", .DataPropertyName = "TipoLicencia", .HeaderText = "Tipo de Licencia", .Width = 200})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "FechaInicio", .DataPropertyName = "FechaInicio", .HeaderText = "Desde", .Width = 120, .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "FechaFin", .DataPropertyName = "FechaFin", .HeaderText = "Hasta", .Width = 120, .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Estado", .DataPropertyName = "Estado", .HeaderText = "Estado", .Width = 100})
            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Comentario", .DataPropertyName = "Comentario", .HeaderText = "Comentario", .Width = 250})
        End With
    End Sub

    Private Async Function CargarLicenciasAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim filtroNombre = txtBusqueda.Text.Trim()
            Dim tipoId = CType(cboTipoLicencia.SelectedValue, Integer?)
            Dim desde = If(chkDesde.Checked, CType(dtpDesde.Value.Date, Date?), Nothing)
            Dim hasta = If(chkHasta.Checked, CType(dtpHasta.Value.Date, Date?), Nothing)

            ' --- Llamada al nuevo método paginado ---
            Dim resultadoPaginado = Await _svc.GetAllPaginadoConDetallesAsync(_paginaActual, _tamañoPagina, filtroNombre, tipoId, desde, hasta)

            _listaLicencias = resultadoPaginado.Licencias
            _totalRegistros = resultadoPaginado.TotalRegistros

            dgvLicencias.DataSource = _listaLicencias

            ' --- LÓGICA AÑADIDA para actualizar la UI de paginación ---
            ActualizarControlesPaginacion()

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error al cargar las licencias: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    ''' <summary>
    ''' Nuevo método para manejar el estado de los botones de paginación y la etiqueta.
    ''' </summary>
    Private Sub ActualizarControlesPaginacion()
        Dim totalPaginas = CInt(Math.Ceiling(_totalRegistros / CDbl(_tamañoPagina)))
        If totalPaginas = 0 Then totalPaginas = 1 ' Para evitar "Página 1 de 0"

        lblPaginacion.Text = $"Página {_paginaActual} de {totalPaginas}"
        btnAnterior.Enabled = _paginaActual > 1
        btnSiguiente.Enabled = _paginaActual < totalPaginas
    End Sub

    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        _paginaActual = 1 ' Reiniciar a la primera página en cada nueva búsqueda
        Await CargarLicenciasAsync()
    End Sub

    ' --- Eventos de los nuevos botones ---
    Private Async Sub btnAnterior_Click(sender As Object, e As EventArgs)
        If _paginaActual > 1 Then
            _paginaActual -= 1
            Await CargarLicenciasAsync()
        End If
    End Sub

    Private Async Sub btnSiguiente_Click(sender As Object, e As EventArgs)
        Dim totalPaginas = CInt(Math.Ceiling(_totalRegistros / CDbl(_tamañoPagina)))
        If _paginaActual < totalPaginas Then
            _paginaActual += 1
            Await CargarLicenciasAsync()
        End If
    End Sub

    Private Sub chkDesde_CheckedChanged(sender As Object, e As EventArgs) Handles chkDesde.CheckedChanged
        dtpDesde.Enabled = chkDesde.Checked
    End Sub

    Private Sub chkHasta_CheckedChanged(sender As Object, e As EventArgs) Handles chkHasta.CheckedChanged
        dtpHasta.Enabled = chkHasta.Checked
    End Sub

End Class