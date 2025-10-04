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
    Inherits Form

    Private ReadOnly _toolTip As New ToolTip()
    Private ReadOnly _dtpFecha As DateTimePicker
    Private ReadOnly _btnActualizar As Button
    Private ReadOnly _lblUltimaActualizacion As Label
    Private ReadOnly _panelHeader As Panel
    Private ReadOnly _flowCards As FlowLayoutPanel
    Private ReadOnly _layoutRoot As TableLayoutPanel
    Private ReadOnly _tableDetalles As TableLayoutPanel
    Private ReadOnly _groupLicencias As GroupBox
    Private ReadOnly _groupPresencias As GroupBox
    Private ReadOnly _dgvLicencias As DataGridView
    Private ReadOnly _dgvPresencias As DataGridView
    Private ReadOnly _lblLicenciasSinDatos As Label
    Private ReadOnly _lblPresenciasSinDatos As Label

    Private ReadOnly _lblTotalFuncionarios As Label
    Private ReadOnly _lblActivos As Label
    Private ReadOnly _lblInactivos As Label
    Private ReadOnly _lblPresentes As Label
    Private ReadOnly _lblFrancos As Label
    Private ReadOnly _lblLicencias As Label
    Private ReadOnly _lblAusentes As Label

    Public Sub New()
        _dtpFecha = New DateTimePicker()
        _btnActualizar = New Button()
        _lblUltimaActualizacion = New Label()
        _panelHeader = New Panel()
        _flowCards = New FlowLayoutPanel()
        _layoutRoot = New TableLayoutPanel()
        _tableDetalles = New TableLayoutPanel()
        _groupLicencias = New GroupBox()
        _groupPresencias = New GroupBox()
        _dgvLicencias = New DataGridView()
        _dgvPresencias = New DataGridView()
        _lblLicenciasSinDatos = New Label()
        _lblPresenciasSinDatos = New Label()

        _lblTotalFuncionarios = CrearEtiquetaValor()
        _lblActivos = CrearEtiquetaValor()
        _lblInactivos = CrearEtiquetaValor()
        _lblPresentes = CrearEtiquetaValor()
        _lblFrancos = CrearEtiquetaValor()
        _lblLicencias = CrearEtiquetaValor()
        _lblAusentes = CrearEtiquetaValor()

        InitializeComponent()
    End Sub

    Private Sub InitializeComponent()
        SuspendLayout()

        Text = "Resumen de Cantidades"
        StartPosition = FormStartPosition.CenterScreen
        MinimumSize = New Size(980, 720)
        Size = New Size(1100, 760)
        Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)

        _panelHeader.Dock = DockStyle.Fill
        _panelHeader.Padding = New Padding(16)
        _panelHeader.Height = 72

        _dtpFecha.Format = DateTimePickerFormat.Custom
        _dtpFecha.CustomFormat = "dddd dd 'de' MMMM yyyy"
        _dtpFecha.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)
        _dtpFecha.Width = 320
        _dtpFecha.Anchor = AnchorStyles.Left Or AnchorStyles.Top

        _btnActualizar.Text = "Actualizar"
        _btnActualizar.Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)
        _btnActualizar.AutoSize = True
        _btnActualizar.AutoSizeMode = AutoSizeMode.GrowAndShrink
        _btnActualizar.Padding = New Padding(14, 6, 14, 6)
        _btnActualizar.Margin = New Padding(16, 0, 0, 0)
        _btnActualizar.Anchor = AnchorStyles.Left Or AnchorStyles.Top

        _lblUltimaActualizacion.AutoSize = True
        _lblUltimaActualizacion.Font = New Font("Segoe UI", 9.0F, FontStyle.Italic)
        _lblUltimaActualizacion.TextAlign = ContentAlignment.MiddleRight
        _lblUltimaActualizacion.Anchor = AnchorStyles.Right Or AnchorStyles.Top

        _panelHeader.Controls.Add(_dtpFecha)
        _panelHeader.Controls.Add(_btnActualizar)
        _panelHeader.Controls.Add(_lblUltimaActualizacion)

        AddHandler _panelHeader.Layout, AddressOf PanelHeaderLayout

        _flowCards.Dock = DockStyle.Fill
        _flowCards.AutoSize = True
        _flowCards.AutoSizeMode = AutoSizeMode.GrowAndShrink
        _flowCards.WrapContents = True
        _flowCards.FlowDirection = FlowDirection.LeftToRight
        _flowCards.Padding = New Padding(8, 0, 8, 8)

        _flowCards.Controls.Add(CrearCard("Total de funcionarios", _lblTotalFuncionarios, "Cantidad total de funcionarios registrados."))
        _flowCards.Controls.Add(CrearCard("Funcionarios activos", _lblActivos, "Funcionarios con estado activo."))
        _flowCards.Controls.Add(CrearCard("Funcionarios inactivos", _lblInactivos, "Funcionarios marcados como inactivos."))
        _flowCards.Controls.Add(CrearCard("Presentes", _lblPresentes, "Funcionarios activos clasificados como presentes según la presencia del día."))
        _flowCards.Controls.Add(CrearCard("Francos", _lblFrancos, "Funcionarios activos asignados como franco en la fecha seleccionada."))
        _flowCards.Controls.Add(CrearCard("Licencias vigentes", _lblLicencias, "Funcionarios activos con alguna licencia activa en la fecha."))
        _flowCards.Controls.Add(CrearCard("Ausentes sin clasificar", _lblAusentes, "Funcionarios activos que no figuran como presentes, francos ni con licencias."))

        ConfigurarDataGridView(_dgvLicencias)
        ConfigurarDataGridView(_dgvPresencias)

        _lblLicenciasSinDatos.AutoSize = True
        _lblLicenciasSinDatos.Text = "No se registran licencias en la fecha seleccionada."
        _lblLicenciasSinDatos.Font = New Font("Segoe UI", 9.0F, FontStyle.Italic)
        _lblLicenciasSinDatos.ForeColor = Color.FromArgb(90, 90, 90)
        _lblLicenciasSinDatos.Visible = False
        _lblLicenciasSinDatos.Dock = DockStyle.Top
        _lblLicenciasSinDatos.TextAlign = ContentAlignment.MiddleCenter

        _groupLicencias.Text = "Licencias por tipo"
        _groupLicencias.Dock = DockStyle.Fill
        _groupLicencias.Padding = New Padding(12)
        _groupLicencias.Controls.Add(_dgvLicencias)
        _groupLicencias.Controls.Add(_lblLicenciasSinDatos)

        _lblPresenciasSinDatos.AutoSize = True
        _lblPresenciasSinDatos.Text = "No se registran datos de presencia para la fecha seleccionada."
        _lblPresenciasSinDatos.Font = New Font("Segoe UI", 9.0F, FontStyle.Italic)
        _lblPresenciasSinDatos.ForeColor = Color.FromArgb(90, 90, 90)
        _lblPresenciasSinDatos.Visible = False
        _lblPresenciasSinDatos.Dock = DockStyle.Top
        _lblPresenciasSinDatos.TextAlign = ContentAlignment.MiddleCenter

        _groupPresencias.Text = "Presencias por estado"
        _groupPresencias.Dock = DockStyle.Fill
        _groupPresencias.Padding = New Padding(12)
        _groupPresencias.Controls.Add(_dgvPresencias)
        _groupPresencias.Controls.Add(_lblPresenciasSinDatos)

        _tableDetalles.ColumnCount = 2
        _tableDetalles.RowCount = 1
        _tableDetalles.Dock = DockStyle.Fill
        _tableDetalles.Padding = New Padding(8)
        _tableDetalles.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        _tableDetalles.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0F))
        _tableDetalles.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        _tableDetalles.Controls.Add(_groupLicencias, 0, 0)
        _tableDetalles.Controls.Add(_groupPresencias, 1, 0)

        _layoutRoot.ColumnCount = 1
        _layoutRoot.RowCount = 3
        _layoutRoot.Dock = DockStyle.Fill
        _layoutRoot.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        _layoutRoot.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        _layoutRoot.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        _layoutRoot.Controls.Add(_panelHeader, 0, 0)
        _layoutRoot.Controls.Add(_flowCards, 0, 1)
        _layoutRoot.Controls.Add(_tableDetalles, 0, 2)

        Controls.Add(_layoutRoot)

        AddHandler Load, AddressOf frmResumenCantidades_Load
        AddHandler _btnActualizar.Click, Async Sub(sender, args) Await ActualizarDatosAsync()

        ResumeLayout(False)
    End Sub

    Private Sub PanelHeaderLayout(sender As Object, e As LayoutEventArgs)
        _dtpFecha.Location = New Point(0, (_panelHeader.Height - _dtpFecha.Height) \ 2)
        _btnActualizar.Location = New Point(_dtpFecha.Right + 16, (_panelHeader.Height - _btnActualizar.Height) \ 2)
        _lblUltimaActualizacion.Location = New Point(_panelHeader.Width - _lblUltimaActualizacion.Width, (_panelHeader.Height - _lblUltimaActualizacion.Height) \ 2)
    End Sub

    Private Async Sub frmResumenCantidades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        _dtpFecha.Value = Date.Today
        Await ActualizarDatosAsync() ' espera a que termine
    End Sub


    Private Async Function ActualizarDatosAsync() As Task
        If Not _btnActualizar.Enabled Then
            Return
        End If

        _btnActualizar.Enabled = False
        Cursor = Cursors.WaitCursor
        Try
            Dim fechaConsulta = _dtpFecha.Value.Date
            Dim resumen = Await ConstruirResumenAsync(fechaConsulta)
            ActualizarTarjetas(resumen)
            ActualizarLicencias(resumen)
            ActualizarPresencias(resumen)

            _lblUltimaActualizacion.Text = $"Actualizado: {DateTime.Now:G}"
            _toolTip.SetToolTip(_lblUltimaActualizacion, $"Datos calculados para {fechaConsulta:D}.")
        Catch ex As Exception
            MessageBox.Show(Me, "Ocurrió un error al actualizar el resumen: " & ex.Message, "Apex", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub ActualizarLicencias(resumen As ResumenDatos)
        Dim licencias = resumen.LicenciasPorTipo.OrderByDescending(Function(p) p.Value).ThenBy(Function(p) p.Key, StringComparer.OrdinalIgnoreCase).ToList()
        If licencias.Count = 0 Then
            _dgvLicencias.DataSource = Nothing
            _lblLicenciasSinDatos.Visible = True
            Return
        End If

        Dim tabla = New DataTable()
        tabla.Columns.Add("Tipo", GetType(String))
        tabla.Columns.Add("Cantidad", GetType(Integer))

        For Each item In licencias
            tabla.Rows.Add(item.Key, item.Value)
        Next

        _dgvLicencias.DataSource = tabla
        _dgvLicencias.Columns(0).HeaderText = "Tipo de licencia"
        _dgvLicencias.Columns(1).HeaderText = "Cantidad"
        _dgvLicencias.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        _dgvLicencias.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        _lblLicenciasSinDatos.Visible = False
    End Sub

    Private Sub ActualizarPresencias(resumen As ResumenDatos)
        Dim presencias = resumen.PresenciasPorEstado.OrderByDescending(Function(p) p.Value).ThenBy(Function(p) p.Key, StringComparer.OrdinalIgnoreCase).ToList()
        If presencias.Count = 0 Then
            _dgvPresencias.DataSource = Nothing
            _lblPresenciasSinDatos.Visible = True
            Return
        End If

        Dim tabla = New DataTable()
        tabla.Columns.Add("Estado", GetType(String))
        tabla.Columns.Add("Cantidad", GetType(Integer))

        For Each item In presencias
            tabla.Rows.Add(item.Key, item.Value)
        Next

        _dgvPresencias.DataSource = tabla
        _dgvPresencias.Columns(0).HeaderText = "Estado reportado"
        _dgvPresencias.Columns(1).HeaderText = "Cantidad"
        _dgvPresencias.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        _dgvPresencias.Columns(1).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells

        _lblPresenciasSinDatos.Visible = False
    End Sub

    Private Shared Function FormatearNumero(valor As Integer) As String
        Return valor.ToString("N0", CultureInfo.CurrentCulture)
    End Function

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

                If Not activosSet.Contains(registro.FuncionarioId) Then
                    Continue For
                End If

                Select Case ClasificarResultado(estado)
                    Case CategoriaPresencia.Presente
                        resumen.Presentes += 1
                    Case CategoriaPresencia.Franco
                        resumen.Francos += 1
                    Case CategoriaPresencia.Licencia
                        resumen.Ausentes += 1
                    Case Else
                        resumen.Ausentes += 1
                End Select
            Next
        End Using

        Dim licenciasPorTipo = Await ObtenerLicenciasPorTipoAsync(fecha)
        If licenciasPorTipo.Count > 0 Then
            resumen.LicenciasPorTipo = licenciasPorTipo
            resumen.LicenciasTotales = licenciasPorTipo.Values.Sum()
        End If

        Dim ausentesCalculados = resumen.Activos - resumen.Presentes - resumen.Francos - resumen.LicenciasTotales
        If ausentesCalculados < 0 Then ausentesCalculados = 0
        resumen.Ausentes = ausentesCalculados

        Return resumen
    End Function

    Private Shared Function ClasificarResultado(resultado As String) As CategoriaPresencia
        If String.IsNullOrWhiteSpace(resultado) Then
            Return CategoriaPresencia.Desconocido
        End If

        Dim valor = resultado.Trim()

        If valor.StartsWith("Inactivo", StringComparison.OrdinalIgnoreCase) Then
            Return CategoriaPresencia.Inactivo
        End If

        If valor.Equals("Franco", StringComparison.OrdinalIgnoreCase) Then
            Return CategoriaPresencia.Franco
        End If

        If valor.StartsWith("Presente", StringComparison.OrdinalIgnoreCase) Then
            Return CategoriaPresencia.Presente
        End If

        Return CategoriaPresencia.Licencia
    End Function

    Private Shared Async Function ObtenerLicenciasPorTipoAsync(fecha As Date) As Task(Of Dictionary(Of String, Integer))
        Dim resultado = New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        Dim tabla = Await ConsultasGenericas.ObtenerDatosGenericosAsync(TipoOrigenDatos.Licencias, fecha, fecha)
        If tabla Is Nothing OrElse tabla.Rows.Count = 0 Then
            Return resultado
        End If

        Dim columnasLicencia = New String() {"TipoLicencia", "Tipo", "Licencia"}
        Dim columnaDisponible = columnasLicencia.FirstOrDefault(Function(c) tabla.Columns.Contains(c))
        If String.IsNullOrEmpty(columnaDisponible) Then
            Return resultado
        End If

        For Each row As DataRow In tabla.Rows
            Dim tipo = Convert.ToString(row(columnaDisponible)).Trim()
            If String.IsNullOrEmpty(tipo) Then
                tipo = "Sin especificar"
            End If

            If resultado.ContainsKey(tipo) Then
                resultado(tipo) += 1
            Else
                resultado(tipo) = 1
            End If
        Next

        Return resultado
    End Function

    Private Shared Function CrearEtiquetaValor() As Label
        Return New Label() With {
            .AutoSize = False,
            .Font = New Font("Segoe UI", 22.0F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(28, 41, 56),
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleCenter,
            .MinimumSize = New Size(0, 60)
        }
    End Function

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

    Private Shared Sub ConfigurarDataGridView(grid As DataGridView)
        With grid
            .Dock = DockStyle.Fill
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .RowHeadersVisible = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .BackgroundColor = Color.White
            .BorderStyle = BorderStyle.None
            .ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        End With
    End Sub

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
