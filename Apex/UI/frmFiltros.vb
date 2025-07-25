Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Data.Entity

Public Class frmFiltros
    Inherits Form

#Region "Modelos y Gestor de Filtros"

    ' Enum para los operadores de comparación disponibles
    Public Enum OperadorComparacion
        Igual
        NoIgual
        Contiene
        MayorOIgual
        MenorOIgual
    End Enum

    ' Clase para representar una regla de filtro individual
    Public Class ReglaFiltro
        Public Property Columna As String
        Public Property Operador As OperadorComparacion
        Public Property Valor As Object

        Public Overrides Function ToString() As String
            Return $"{Columna} {Operador.ToString()} ""{Valor}"""
        End Function
    End Class

    ' Administra la colección de reglas de filtro activas
    Private Class GestorFiltros
        Public ReadOnly Property Reglas As New BindingList(Of ReglaFiltro)()

        Public Sub Agregar(r As ReglaFiltro)
            ' Evita añadir reglas duplicadas
            If Not Reglas.Any(Function(existing) existing.Columna = r.Columna AndAlso existing.Valor.Equals(r.Valor)) Then
                Reglas.Add(r)
            End If
        End Sub

        Public Sub Quitar(r As ReglaFiltro)
            Reglas.Remove(r)
        End Sub

        Public Sub Limpiar()
            Reglas.Clear()
        End Sub
    End Class

#End Region

#Region "Campos del Formulario"

    Private ReadOnly _gestorFiltros As New GestorFiltros()
    Private _servicioLicencias As New LicenciaService()
    Private _columnasDisponibles As New Dictionary(Of String, String) From {
        {"Funcionario", "Funcionario.Nombre"},
        {"CI", "Funcionario.CI"},
        {"Tipo de Licencia", "TipoLicencia.Nombre"},
        {"Estado", "estado"}
    }

#End Region

#Region "Load y Configuración"

    Private Sub frmFiltros_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarControles()
        CargarColumnas()
    End Sub

    Private Sub ConfigurarControles()
        dgvResultados.AutoGenerateColumns = False
        ConfigurarGrillaResultados()

        ' Enlazar la lista de reglas al ListBox para visualización
        lstFiltrosActivos.DataSource = _gestorFiltros.Reglas
        lstFiltrosActivos.DisplayMember = "ToString"
    End Sub

    Private Sub CargarColumnas()
        lstColumnas.Items.Clear()
        For Each item In _columnasDisponibles.Keys
            lstColumnas.Items.Add(item)
        Next
        If lstColumnas.Items.Count > 0 Then
            lstColumnas.SelectedIndex = 0
        End If
    End Sub

    Private Sub ConfigurarGrillaResultados()
        With dgvResultados.Columns
            .Clear()
            .Add(New DataGridViewTextBoxColumn With {.HeaderText = "C.I.", .DataPropertyName = "CI", .Width = 100})
            .Add(New DataGridViewTextBoxColumn With {.HeaderText = "Funcionario", .DataPropertyName = "NombreFuncionario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Add(New DataGridViewTextBoxColumn With {.HeaderText = "Tipo de Licencia", .DataPropertyName = "TipoLicencia", .Width = 200})
            .Add(New DataGridViewTextBoxColumn With {.HeaderText = "Desde", .DataPropertyName = "FechaInicio", .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}, .Width = 120})
            .Add(New DataGridViewTextBoxColumn With {.HeaderText = "Hasta", .DataPropertyName = "FechaFin", .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy"}, .Width = 120})
            .Add(New DataGridViewTextBoxColumn With {.HeaderText = "Estado", .DataPropertyName = "Estado", .Width = 100})
        End With
    End Sub

#End Region

#Region "Eventos y Lógica de Filtrado"

    Private Async Sub lstColumnas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstColumnas.SelectedIndexChanged
        If lstColumnas.SelectedItem Is Nothing Then Return

        Dim nombreColumna = lstColumnas.SelectedItem.ToString()
        Dim propiedadReal = _columnasDisponibles(nombreColumna)

        lstValores.DataSource = Nothing
        lstValores.Items.Clear()

        Try
            Dim valores = Await _servicioLicencias.ObtenerValoresDistintosAsync(propiedadReal)
            lstValores.DataSource = valores
        Catch ex As Exception
            MessageBox.Show($"Error al cargar valores para {nombreColumna}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnAgregarFiltro_Click(sender As Object, e As EventArgs) Handles btnAgregarFiltro.Click
        If lstColumnas.SelectedItem Is Nothing OrElse lstValores.SelectedItem Is Nothing Then Return

        Dim columna = lstColumnas.SelectedItem.ToString()
        Dim valor = lstValores.SelectedItem.ToString()

        _gestorFiltros.Agregar(New ReglaFiltro With {
            .Columna = _columnasDisponibles(columna),
            .Operador = OperadorComparacion.Igual,
            .Valor = valor
        })
    End Sub

    Private Sub btnQuitarFiltro_Click(sender As Object, e As EventArgs) Handles btnQuitarFiltro.Click
        If lstFiltrosActivos.SelectedItem IsNot Nothing Then
            Dim regla = CType(lstFiltrosActivos.SelectedItem, ReglaFiltro)
            _gestorFiltros.Quitar(regla)
        End If
    End Sub

    Private Sub btnLimpiarFiltros_Click(sender As Object, e As EventArgs) Handles btnLimpiarFiltros.Click
        _gestorFiltros.Limpiar()
    End Sub

    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim resultados = Await _servicioLicencias.GetWithAdvancedFilterAsync(_gestorFiltros.Reglas.ToList())
            dgvResultados.DataSource = resultados
            lblConteo.Text = $"Registros: {resultados.Count}"
        Catch ex As Exception
            MessageBox.Show("Error al ejecutar la búsqueda: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Sub

#End Region

End Class