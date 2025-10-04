Option Strict On
Option Explicit On

Imports System.Data
Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Globalization
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public Class frmResumenCantidades

    ' El ToolTip se mantiene aquí ya que se usa dinámicamente en la lógica.
    Private ReadOnly _toolTip As New ToolTip()

    Public Sub New()
        ' Esta llamada es necesaria para inicializar los componentes definidos en el Designer.
        InitializeComponent()
    End Sub

    Private Async Sub frmResumenCantidades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Aplica el tema de la aplicación si existe.
            AppTheme.Aplicar(Me)
        Catch
            ' Ignora errores si AppTheme no está disponible.
        End Try

        ' --- Configuración dinámica de la UI que se movió del InitializeComponent ---
        ConfigurarTarjetas()
        ConfigurarDataGridViews()
        ' --------------------------------------------------------------------------

        _dtpFecha.Value = Date.Today
        Await ActualizarDatosAsync() ' Carga inicial de datos.
    End Sub

    Private Sub ConfigurarTarjetas()
        ' Se crean y añaden las tarjetas de resumen aquí para mantener el designer limpio.
        _flowCards.Controls.Add(CrearCard("Total de funcionarios", _lblTotalFuncionarios, "Cantidad total de funcionarios registrados."))
        _flowCards.Controls.Add(CrearCard("Funcionarios activos", _lblActivos, "Funcionarios con estado activo."))
        _flowCards.Controls.Add(CrearCard("Funcionarios inactivos", _lblInactivos, "Funcionarios marcados como inactivos."))
        _flowCards.Controls.Add(CrearCard("Presentes", _lblPresentes, "Funcionarios activos clasificados como presentes según la presencia del día."))
        _flowCards.Controls.Add(CrearCard("Francos", _lblFrancos, "Funcionarios activos asignados como franco en la fecha seleccionada."))
        _flowCards.Controls.Add(CrearCard("Licencias vigentes", _lblLicencias, "Funcionarios activos con alguna licencia activa en la fecha."))
        _flowCards.Controls.Add(CrearCard("Ausentes sin clasificar", _lblAusentes, "Funcionarios activos que no figuran como presentes, francos ni con licencias."))
    End Sub

    Private Sub ConfigurarDataGridViews()
        ' Se aplica la configuración común a las grillas.
        ConfigurarEstiloDataGridView(_dgvLicencias)
        ConfigurarEstiloDataGridView(_dgvPresencias)
    End Sub

    Private Async Sub BtnActualizar_Click(sender As Object, e As EventArgs) Handles _btnActualizar.Click
        Await ActualizarDatosAsync()
    End Sub

    Private Async Function ActualizarDatosAsync() As Task
        If Not _btnActualizar.Enabled Then Return

        _btnActualizar.Enabled = False
        Cursor = Cursors.WaitCursor
        Try
            Dim fechaConsulta = _dtpFecha.Value.Date
            Dim resumen = Await ConstruirResumenAsync(fechaConsulta)

            ActualizarTarjetas(resumen)
            ActualizarDetalleLicencias(resumen)
            ActualizarDetallePresencias(resumen)

            _lblUltimaActualizacion.Text = $"Actualizado: {DateTime.Now:G}"
            _toolTip.SetToolTip(_lblUltimaActualizacion, $"Datos calculados para {fechaConsulta:D}.")
        Catch ex As Exception
            MessageBox.Show(Me, "Ocurrió un error al actualizar el resumen: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            _btnActualizar.Enabled = True
            Cursor = Cursors.Default
        End Try
    End Function

    Private Sub ActualizarTarjetas(resumen As ResumenDatos)
        _lblTotalFuncionarios.Text = FormatearNumero(resumen.TotalFuncionarios)
        _lblActivos.Text = FormatearNumero(resumen.Activos)
        _lblInactivos.Text = FormatearNumero(resumen.Inactivos)
        _lblPresentes.Text = FormatearNumero(resumen.Presentes)
        _lblFrancos.Text = FormatearNumero(resumen.Francos)
        _lblLicencias.Text = FormatearNumero(resumen.LicenciasTotales)
        _lblAusentes.Text = FormatearNumero(resumen.Ausentes)
        _toolTip.SetToolTip(_lblAusentes, $"Funcionarios activos sin una clasificación de presencia conocida ({resumen.Ausentes} en total).")
    End Sub

    Private Sub ActualizarDetalleLicencias(resumen As ResumenDatos)
        Dim licencias = resumen.LicenciasPorTipo.OrderByDescending(Function(p) p.Value).ThenBy(Function(p) p.Key, StringComparer.OrdinalIgnoreCase).ToList()
        Dim hayDatos = licencias.Any()

        _lblLicenciasSinDatos.Visible = Not hayDatos
        _dgvLicencias.Visible = hayDatos

        If hayDatos Then
            Dim tabla = New DataTable()
            tabla.Columns.Add("Tipo", GetType(String))
            tabla.Columns.Add("Cantidad", GetType(Integer))
            For Each item In licencias
                tabla.Rows.Add(item.Key, item.Value)
            Next
            _dgvLicencias.DataSource = tabla
        Else
            _dgvLicencias.DataSource = Nothing
        End If
    End Sub

    Private Sub ActualizarDetallePresencias(resumen As ResumenDatos)
        Dim presencias = resumen.PresenciasPorEstado.OrderByDescending(Function(p) p.Value).ThenBy(Function(p) p.Key, StringComparer.OrdinalIgnoreCase).ToList()
        Dim hayDatos = presencias.Any()

        _lblPresenciasSinDatos.Visible = Not hayDatos
        _dgvPresencias.Visible = hayDatos

        If hayDatos Then
            Dim tabla = New DataTable()
            tabla.Columns.Add("Estado", GetType(String))
            tabla.Columns.Add("Cantidad", GetType(Integer))
            For Each item In presencias
                tabla.Rows.Add(item.Key, item.Value)
            Next
            _dgvPresencias.DataSource = tabla
        Else
            _dgvPresencias.DataSource = Nothing
        End If
    End Sub

    Private Async Function ConstruirResumenAsync(fecha As Date) As Task(Of ResumenDatos)
        Dim resumen = New ResumenDatos() With {
            .FechaConsulta = fecha,
            .LicenciasPorTipo = New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase),
            .PresenciasPorEstado = New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        }

        Using uow As New UnitOfWork()
            Dim ctx = uow.Context
            resumen.TotalFuncionarios = Await ctx.Set(Of Funcionario)().CountAsync()

            Dim activosIds = Await ctx.Set(Of Funcionario)().Where(Function(f) f.Activo).Select(Function(f) f.Id).ToListAsync()
            Dim activosSet = New HashSet(Of Integer)(activosIds)
            resumen.Activos = activosSet.Count
            resumen.Inactivos = resumen.TotalFuncionarios - resumen.Activos

            Dim pFecha = New SqlParameter("@Fecha", fecha)
            Dim presencias = Await ctx.Database.SqlQuery(Of PresenciaDTO)("EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha).ToListAsync()

            For Each registro In presencias
                Dim estado = If(String.IsNullOrWhiteSpace(registro.Resultado), "Sin información", registro.Resultado.Trim())
                If resumen.PresenciasPorEstado.ContainsKey(estado) Then
                    resumen.PresenciasPorEstado(estado) += 1
                Else
                    resumen.PresenciasPorEstado(estado) = 1
                End If

                If Not activosSet.Contains(registro.FuncionarioId) Then Continue For

                Select Case ClasificarResultado(estado)
                    Case CategoriaPresencia.Presente : resumen.Presentes += 1
                    Case CategoriaPresencia.Franco : resumen.Francos += 1
                End Select
            Next
        End Using

        Dim licenciasPorTipo = Await ObtenerLicenciasPorTipoAsync(fecha)
        If licenciasPorTipo.Any() Then
            resumen.LicenciasPorTipo = licenciasPorTipo
            resumen.LicenciasTotales = licenciasPorTipo.Values.Sum()
        End If

        resumen.Ausentes = Math.Max(0, resumen.Activos - resumen.Presentes - resumen.Francos - resumen.LicenciasTotales)

        Return resumen
    End Function

    Private Shared Async Function ObtenerLicenciasPorTipoAsync(fecha As Date) As Task(Of Dictionary(Of String, Integer))
        Dim resultado = New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        Dim tabla = Await ConsultasGenericas.ObtenerDatosGenericosAsync(TipoOrigenDatos.Licencias, fecha, fecha)
        If tabla Is Nothing OrElse tabla.Rows.Count = 0 Then Return resultado

        Dim columnaDisponible = {"TipoLicencia", "Tipo", "Licencia"}.FirstOrDefault(Function(c) tabla.Columns.Contains(c))
        If String.IsNullOrEmpty(columnaDisponible) Then Return resultado

        For Each row As DataRow In tabla.Rows
            Dim tipo = Convert.ToString(row(columnaDisponible)).Trim()
            If String.IsNullOrEmpty(tipo) Then tipo = "Sin especificar"

            If resultado.ContainsKey(tipo) Then
                resultado(tipo) += 1
            Else
                resultado(tipo) = 1
            End If
        Next

        Return resultado
    End Function

    ' --- Métodos de ayuda para la UI ---
    Private Function CrearCard(titulo As String, valorLabel As Label, descripcion As String) As Control
        Dim panel = New Panel() With {
            .Width = 260,
            .Height = 120,
            .Margin = New Padding(8),
            .Padding = New Padding(16),
            .BackColor = Color.White,
            .BorderStyle = BorderStyle.FixedSingle
        }
        Dim lblTitulo = New Label() With {
            .Text = titulo,
            .Dock = DockStyle.Top,
            .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(90, 90, 90),
            .Height = 32
        }
        panel.Controls.Add(valorLabel)
        panel.Controls.Add(lblTitulo)
        valorLabel.BringToFront()
        _toolTip.SetToolTip(panel, descripcion)
        _toolTip.SetToolTip(lblTitulo, descripcion)
        _toolTip.SetToolTip(valorLabel, descripcion)
        Return panel
    End Function

    Private Sub ConfigurarEstiloDataGridView(grid As DataGridView)
        With grid
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .RowHeadersVisible = False
            .BackgroundColor = Color.White
            .BorderStyle = BorderStyle.None
            ' --- Estilos Adicionales para Mejor Apariencia ---
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .GridColor = Color.FromArgb(230, 230, 230)
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)
            .Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            .Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        End With
    End Sub

    ' --- Métodos y Estructuras auxiliares ---
    Private Shared Function ClasificarResultado(resultado As String) As CategoriaPresencia
        If String.IsNullOrWhiteSpace(resultado) Then Return CategoriaPresencia.Desconocido
        Dim valor = resultado.Trim()
        If valor.StartsWith("Inactivo", StringComparison.OrdinalIgnoreCase) Then Return CategoriaPresencia.Inactivo
        If valor.Equals("Franco", StringComparison.OrdinalIgnoreCase) Then Return CategoriaPresencia.Franco
        If valor.StartsWith("Presente", StringComparison.OrdinalIgnoreCase) Then Return CategoriaPresencia.Presente
        Return CategoriaPresencia.Licencia
    End Function

    Private Shared Function FormatearNumero(valor As Integer) As String
        Return valor.ToString("N0", CultureInfo.CurrentCulture)
    End Function

    Private Class ResumenDatos
        Public Property FechaConsulta As Date
        Public Property TotalFuncionarios As Integer
        Public Property Activos As Integer
        Public Property Inactivos As Integer
        Public Property Presentes As Integer
        Public Property Francos As Integer
        Public Property LicenciasTotales As Integer
        Public Property Ausentes As Integer
        Public Property LicenciasPorTipo As Dictionary(Of String, Integer)
        Public Property PresenciasPorEstado As Dictionary(Of String, Integer)
    End Class

    Private Enum CategoriaPresencia
        Presente
        Franco
        Licencia
        Inactivo
        Desconocido
    End Enum

End Class