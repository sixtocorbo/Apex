Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Windows.Forms

Public Class frmFuncionarioBuscar
    Inherits Form

    '--- INICIO DE LA CORRECCIÓN ---
    Public Enum ModoApertura
        Navegacion ' Para ver y editar desde el Dashboard
        Seleccion  ' Para seleccionar un funcionario y devolverlo
    End Enum

    Private ReadOnly _modo As ModoApertura
    '--- FIN DE LA CORRECCIÓN ---

    Private Const LIMITE_FILAS As Integer = 500

    Public ReadOnly Property FuncionarioSeleccionado As FuncionarioMin
        Get
            If dgvResultados.CurrentRow IsNot Nothing Then
                Return CType(dgvResultados.CurrentRow.DataBoundItem, FuncionarioMin)
            End If
            Return Nothing
        End Get
    End Property

    '--- INICIO DE LA CORRECCIÓN ---
    ' Constructor original para el modo Navegación (Dashboard)
    Public Sub New()
        InitializeComponent()
        _modo = ModoApertura.Navegacion
        ' Ocultar los botones de selección en este modo
        FlowLayoutPanelAcciones.Visible = False
    End Sub

    ' Nuevo constructor para el modo Selección
    Public Sub New(modo As ModoApertura)
        Me.New() ' Llama al constructor sin parámetros primero
        _modo = modo
        ' Asegurar que los botones sean visibles si es modo Selección
        FlowLayoutPanelAcciones.Visible = (_modo = ModoApertura.Seleccion)
    End Sub
    '--- FIN DE LA CORRECCIÓN ---


    Private Sub frmFuncionarioBuscar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
    End Sub

    Private Sub frmFuncionarioBuscar_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.ActiveControl = txtBusqueda
    End Sub

#Region "Diseño de grilla"
    Private Sub ConfigurarGrilla()
        With dgvResultados
            .AutoGenerateColumns = False
            .RowTemplate.Height = 40
            .RowTemplate.MinimumHeight = 40
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Id",
                .DataPropertyName = "Id",
                .Visible = False
            })

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "CI",
                .DataPropertyName = "CI",
                .HeaderText = "CI",
                .Width = 90
            })

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Nombre",
                .DataPropertyName = "Nombre",
                .HeaderText = "Nombre",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            })
        End With

        AddHandler dgvResultados.SelectionChanged, AddressOf MostrarDetalle
        AddHandler dgvResultados.CellDoubleClick, AddressOf OnDgvDoubleClick
        AddHandler dgvResultados.DataError, Sub(s, ev) ev.ThrowException = False
    End Sub
#End Region


#Region "Búsqueda con Full-Text y CONTAINS"
    Private Async Function BuscarAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        btnBuscar.Enabled = False

        Try
            Using uow As New UnitOfWork()
                Dim ctx = uow.Context
                Dim filtro As String = txtBusqueda.Text.Trim()

                If filtro.Length < 3 Then
                    dgvResultados.DataSource = Nothing
                    LimpiarDetalle()
                    Return
                End If

                Dim terminos = filtro.Split(" "c) _
                                     .Where(Function(w) Not String.IsNullOrWhiteSpace(w)) _
                                     .Select(Function(w) $"""{w}*""")
                Dim expresionFts = String.Join(" AND ", terminos)

                Dim sb As New StringBuilder()
                sb.AppendLine("SELECT TOP (@limite)")
                sb.AppendLine("      Id, CI, Nombre")
                sb.AppendLine("FROM   dbo.Funcionario WITH (NOLOCK)")
                sb.AppendLine("WHERE  CONTAINS((CI, Nombre), @patron)")
                sb.AppendLine("ORDER BY Nombre;")

                Dim sql = sb.ToString()
                Dim pLimite = New SqlParameter("@limite", LIMITE_FILAS)
                Dim pPatron = New SqlParameter("@patron", expresionFts)

                Dim lista = Await ctx.Database _
                                .SqlQuery(Of FuncionarioMin)(sql, pLimite, pPatron) _
                                .ToListAsync()

                dgvResultados.DataSource = Nothing
                dgvResultados.DataSource = lista

                If lista.Any() Then
                    dgvResultados.ClearSelection()
                    dgvResultados.Rows(0).Selected = True
                    dgvResultados.CurrentCell = dgvResultados.Rows(0).Cells("CI")
                Else
                    LimpiarDetalle()
                End If

                If lista.Count = LIMITE_FILAS Then
                    MessageBox.Show($"Mostrando los primeros {LIMITE_FILAS} resultados." &
                                "Refiná la búsqueda para ver más.",
                                "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using

        Catch ex As SqlException When ex.Number = -2
            MessageBox.Show("La consulta excedió el tiempo de espera. Refiná los filtros o intentá nuevamente.",
                    "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error inesperado: " & ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            LoadingHelper.OcultarCargando(Me)
            btnBuscar.Enabled = True
        End Try
    End Function

    Private Async Sub txtBusqueda_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles txtBusqueda.KeyDown

        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            Await BuscarAsync()
            Return
        End If

        If dgvResultados.Rows.Count = 0 Then Return

        Select Case e.KeyCode
            Case Keys.Down : MoverSeleccion(+1) : e.Handled = True
            Case Keys.Up : MoverSeleccion(-1) : e.Handled = True
        End Select
    End Sub

    Private Sub MoverSeleccion(direccion As Integer)
        Dim total = dgvResultados.Rows.Count
        If total = 0 Then
            LimpiarDetalle()
            Exit Sub
        End If

        Dim indexActual As Integer =
        If(dgvResultados.CurrentRow Is Nothing, -1, dgvResultados.CurrentRow.Index)

        Dim nuevoIndex = Math.Max(0, Math.Min(total - 1, indexActual + direccion))

        dgvResultados.ClearSelection()
        dgvResultados.Rows(nuevoIndex).Selected = True
        dgvResultados.CurrentCell = dgvResultados.Rows(nuevoIndex).Cells("CI")

        MostrarDetalle(Nothing, EventArgs.Empty)
    End Sub


#End Region

    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Await BuscarAsync()
    End Sub

#Region "Detalle lateral (Foto on-demand)"
    Private Async Sub MostrarDetalle(sender As Object, e As EventArgs)
        If dgvResultados.CurrentRow Is Nothing OrElse dgvResultados.CurrentRow.DataBoundItem Is Nothing Then Return
        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)

        Using uow As New UnitOfWork()
            Dim f = Await uow.Repository(Of Funcionario)() _
                 .GetAll() _
                .Include(Function(x) x.Cargo) _
                .Include(Function(x) x.TipoFuncionario) _
                .Include(Function(x) x.EstadoTransitorio.Select(Function(et) et.TipoEstadoTransitorio)) _
                .Include(Function(x) x.EstadoTransitorio.Select(Function(et) et.DesignacionDetalle)) _
                .Include(Function(x) x.EstadoTransitorio.Select(Function(et) et.SancionDetalle)) _
                .Include(Function(x) x.EstadoTransitorio.Select(Function(et) et.SumarioDetalle)) _
                .Include(Function(x) x.EstadoTransitorio.Select(Function(et) et.OrdenCincoDetalle)) _
                .Include(Function(x) x.EstadoTransitorio.Select(Function(et) et.EnfermedadDetalle)) _
                .Include(Function(x) x.EstadoTransitorio.Select(Function(et) et.RetenDetalle)) _
                .AsNoTracking() _
                .FirstOrDefaultAsync(Function(x) x.Id = id)

            If dgvResultados.CurrentRow Is Nothing OrElse CInt(dgvResultados.CurrentRow.Cells("Id").Value) <> id Then Return
            If f Is Nothing Then Return

            Dim fechaActual = Date.Today
            Dim estadosActivos As New List(Of String)

            For Each et In f.EstadoTransitorio
                Dim esActivo As Boolean = False
                Select Case et.TipoEstadoTransitorioId
                    Case 1 ' Designación
                        If et.DesignacionDetalle IsNot Nothing Then
                            esActivo = fechaActual >= et.DesignacionDetalle.FechaDesde AndAlso (Not et.DesignacionDetalle.FechaHasta.HasValue OrElse fechaActual <= et.DesignacionDetalle.FechaHasta.Value)
                        End If
                    Case 2 ' Enfermedad
                        If et.EnfermedadDetalle IsNot Nothing Then
                            esActivo = fechaActual >= et.EnfermedadDetalle.FechaDesde AndAlso (Not et.EnfermedadDetalle.FechaHasta.HasValue OrElse fechaActual <= et.EnfermedadDetalle.FechaHasta.Value)
                        End If
                    Case 3 ' Sanción
                        If et.SancionDetalle IsNot Nothing Then
                            esActivo = fechaActual >= et.SancionDetalle.FechaDesde AndAlso (Not et.SancionDetalle.FechaHasta.HasValue OrElse fechaActual <= et.SancionDetalle.FechaHasta.Value)
                        End If
                    Case 4 ' Orden Cinco
                        If et.OrdenCincoDetalle IsNot Nothing Then
                            esActivo = fechaActual >= et.OrdenCincoDetalle.FechaDesde AndAlso (Not et.OrdenCincoDetalle.FechaHasta.HasValue OrElse fechaActual <= et.OrdenCincoDetalle.FechaHasta.Value)
                        End If
                    Case 5 ' Retén
                        If et.RetenDetalle IsNot Nothing Then
                            esActivo = fechaActual = et.RetenDetalle.FechaReten
                        End If
                    Case 6 ' Sumario
                        If et.SumarioDetalle IsNot Nothing Then
                            esActivo = fechaActual >= et.SumarioDetalle.FechaDesde AndAlso (Not et.SumarioDetalle.FechaHasta.HasValue OrElse fechaActual <= et.SumarioDetalle.FechaHasta.Value)
                        End If
                End Select

                If esActivo Then
                    estadosActivos.Add(et.TipoEstadoTransitorio.Nombre)
                End If
            Next

            lblCI.Text = f.CI
            lblNombreCompleto.Text = f.Nombre
            lblCargo.Text = If(f.Cargo Is Nothing, "-", f.Cargo.Nombre)
            lblTipo.Text = f.TipoFuncionario.Nombre
            lblFechaIngreso.Text = f.FechaIngreso.ToShortDateString()
            chkActivoDetalle.Checked = f.Activo

            If estadosActivos.Any() Then
                lblEstadoTransitorio.Text = String.Join(", ", estadosActivos)
            Else
                lblEstadoTransitorio.Text = "Normal"
            End If

            If f.Foto Is Nothing OrElse f.Foto.Length = 0 Then
                pbFotoDetalle.Image = My.Resources.Police
            Else
                Using ms As New MemoryStream(f.Foto)
                    pbFotoDetalle.Image = Image.FromStream(ms)
                End Using
            End If

            lblPresencia.Text = Await ObtenerPresenciaAsync(id, fechaActual)
        End Using
    End Sub

    ''' <summary>
    ''' Maneja el doble clic para seleccionar o para abrir la ficha de edición.
    ''' </summary>
    Private Async Sub OnDgvDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If dgvResultados.CurrentRow Is Nothing Then Return

        '--- INICIO DE LA CORRECCIÓN ---
        If _modo = ModoApertura.Seleccion Then
            SeleccionarYcerrar()
        Else ' Modo Navegacion
            Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)
            Using frm As New frmFuncionarioCrear(id)
                If frm.ShowDialog() = DialogResult.OK Then
                    Await BuscarAsync()
                End If
            End Using
        End If
        '--- FIN DE LA CORRECCIÓN ---
    End Sub

    Private Async Function ObtenerPresenciaAsync(id As Integer, fecha As Date) As Task(Of String)
        Using uow As New UnitOfWork()
            Dim ctx = uow.Context
            Dim pFecha = New SqlParameter("@Fecha", fecha.Date)
            Dim lista = Await ctx.Database.SqlQuery(Of PresenciaDTO)(
                "EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha
            ).ToListAsync()
            Dim presencia = lista.Where(Function(r) r.FuncionarioId = id).
                             Select(Function(r) r.Resultado).
                             FirstOrDefault()
            Return If(presencia, "-")
        End Using
    End Function

    Private Sub LimpiarDetalle()
        lblCI.Text = ""
        lblNombreCompleto.Text = ""
        lblCargo.Text = ""
        lblTipo.Text = ""
        lblFechaIngreso.Text = ""
        chkActivoDetalle.Checked = False
        lblPresencia.Text = ""
        lblEstadoTransitorio.Text = ""
        pbFotoDetalle.Image = Nothing
    End Sub



#End Region

#Region "Lógica de Selección"

    Private Sub SeleccionarYcerrar()
        If FuncionarioSeleccionado IsNot Nothing Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Por favor, seleccione un funcionario de la lista.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        SeleccionarYcerrar()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

#End Region


#Region "DTO ligero"
    Public Class FuncionarioMin
        Public Property Id As Integer
        Public Property CI As String
        Public Property Nombre As String
    End Class
    Public Class PresenciaDTO

        Public Property FuncionarioId As Integer
        Public Property Resultado As String
    End Class

#End Region

End Class