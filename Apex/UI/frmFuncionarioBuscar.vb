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

    Private Const LIMITE_FILAS As Integer = 500

    Public Property ResultadosFiltrados As List(Of FuncionarioMin)

    ' Asegurá que dgvResultados esté inicializado y no sea Nothing antes de usarlo

    Private Sub frmFuncionarioBuscar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarGrilla()
    End Sub

#Region "Diseño de grilla"
    Private Sub ConfigurarGrilla()
        With dgvResultados
            .AutoGenerateColumns = False
            .RowTemplate.Height = 40
            .RowTemplate.MinimumHeight = 40
            .Columns.Clear()

            '–– Id (oculto) ––
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Id",
            .DataPropertyName = "Id",
            .Visible = False
        })

            '–– CI ––
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "CI",                  ' ← nombre de la columna
            .DataPropertyName = "CI",
            .HeaderText = "CI",
            .Width = 90
        })

            '–– Nombre ––
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Nombre",
            .DataPropertyName = "Nombre",
            .HeaderText = "Nombre",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
        End With

        ' Eventos
        AddHandler dgvResultados.SelectionChanged, AddressOf MostrarDetalle
        AddHandler dgvResultados.CellDoubleClick, AddressOf OnDgvDoubleClick
        AddHandler dgvResultados.DataError, Sub(s, ev) ev.ThrowException = False
    End Sub
#End Region


#Region "Búsqueda con Full-Text y CONTAINS"
    'aprovechando la funcionalidad de búsqueda Full-Text de SQL Server
    Private Async Function BuscarAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        btnBuscar.Enabled = False

        Try
            Using uow As New UnitOfWork()
                Dim ctx = uow.Context
                Dim filtro As String = txtBusqueda.Text.Trim()

                '–– Condición mínima de 3 letras ––
                If filtro.Length < 3 Then
                    dgvResultados.DataSource = Nothing
                    ResultadosFiltrados = New List(Of FuncionarioMin)
                    LimpiarDetalle()
                    Return
                End If

                '–– Construir patrón FTS por término ––
                Dim terminos = filtro.Split(" "c) _
                               .Where(Function(w) Not String.IsNullOrWhiteSpace(w)) _
                               .Select(Function(w) $"""{w}*""")
                Dim expresionFts = String.Join(" AND ", terminos)

                '–– SQL dinámico (CORREGIDO) ––
                Dim sb As New StringBuilder()
                sb.AppendLine("SELECT TOP (@limite)")
                sb.AppendLine("       Id, CI, Nombre")
                sb.AppendLine("FROM   dbo.Funcionario WITH (NOLOCK)")
                sb.AppendLine("WHERE  CONTAINS((CI, Nombre), @patron)") ' <-- LÍNEA MODIFICADA
                sb.AppendLine("ORDER BY Nombre;")

                Dim sql = sb.ToString()
                Dim pLimite = New SqlParameter("@limite", LIMITE_FILAS)
                ' El parámetro @filtro ya no es necesario
                Dim pPatron = New SqlParameter("@patron", expresionFts)

                '–– Ejecutar consulta (CORREGIDO)
                Dim lista = Await ctx.Database _
                                .SqlQuery(Of FuncionarioMin)(sql, pLimite, pPatron) _ ' <-- Parámetro @filtro eliminado
                                .ToListAsync()

                '–– Actualizar grilla
                dgvResultados.DataSource = Nothing
                dgvResultados.DataSource = lista
                ResultadosFiltrados = lista

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
    'Private Async Function BuscarAsync() As Task
    '    LoadingHelper.MostrarCargando(Me)
    '    btnBuscar.Enabled = False

    '    Try
    '        Using uow As New UnitOfWork()
    '            Dim ctx = uow.Context
    '            Dim filtro As String = txtBusqueda.Text.Trim()

    '            '–– Condición mínima de 3 letras ––
    '            If filtro.Length < 3 Then
    '                dgvResultados.DataSource = Nothing
    '                ResultadosFiltrados = New List(Of FuncionarioMin)
    '                LimpiarDetalle()
    '                Return
    '            End If

    '            '–– Construir patrón FTS por término ––
    '            Dim terminos = filtro.Split(" "c) _
    '                              .Where(Function(w) Not String.IsNullOrWhiteSpace(w)) _
    '                              .Select(Function(w) $"""{w}*""")
    '            Dim expresionFts = String.Join(" AND ", terminos)

    '            '–– SQL dinámico ––
    '            Dim sb As New StringBuilder()
    '            sb.AppendLine("SELECT TOP (@limite)")
    '            sb.AppendLine("       Id, CI, Nombre")
    '            sb.AppendLine("FROM   dbo.Funcionario WITH (NOLOCK)")
    '            sb.AppendLine("WHERE 1 = 1")
    '            sb.AppendLine("  AND (CI LIKE '%' + @filtro + '%'")
    '            sb.AppendLine("   OR CONTAINS(Nombre, @patron))")
    '            sb.AppendLine("ORDER BY Nombre;")

    '            Dim sql = sb.ToString()
    '            Dim pLimite = New SqlParameter("@limite", LIMITE_FILAS)
    '            Dim pFiltro = New SqlParameter("@filtro", filtro)
    '            Dim pPatron = New SqlParameter("@patron", expresionFts)

    '            '–– Ejecutar consulta
    '            Dim lista = Await ctx.Database _
    '                             .SqlQuery(Of FuncionarioMin)(sql, pLimite, pFiltro, pPatron) _
    '                            .ToListAsync()

    '            '–– Actualizar grilla
    '            dgvResultados.DataSource = Nothing
    '            dgvResultados.DataSource = lista
    '            ResultadosFiltrados = lista

    '            If lista.Any() Then
    '                dgvResultados.ClearSelection()
    '                dgvResultados.Rows(0).Selected = True
    '                dgvResultados.CurrentCell = dgvResultados.Rows(0).Cells("CI")
    '            Else
    '                LimpiarDetalle()
    '            End If

    '            If lista.Count = LIMITE_FILAS Then
    '                MessageBox.Show($"Mostrando los primeros {LIMITE_FILAS} resultados." &
    '                            "Refiná la búsqueda para ver más.",
    '                            "Aviso",
    '                            MessageBoxButtons.OK, MessageBoxIcon.Information)
    '            End If
    '        End Using

    '    Catch ex As SqlException When ex.Number = -2
    '        MessageBox.Show("La consulta excedió el tiempo de espera. Refiná los filtros o intentá nuevamente.",
    '                    "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning)

    '    Catch ex As Exception
    '        MessageBox.Show("Ocurrió un error inesperado: " & ex.Message,
    '                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

    '    Finally
    '        LoadingHelper.OcultarCargando(Me)
    '        btnBuscar.Enabled = True
    '    End Try
    'End Function


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
            LimpiarDetalle()        ' ← por las dudas
            Exit Sub
        End If

        Dim indexActual As Integer =
        If(dgvResultados.CurrentRow Is Nothing, -1, dgvResultados.CurrentRow.Index)

        Dim nuevoIndex = Math.Max(0, Math.Min(total - 1, indexActual + direccion))

        dgvResultados.ClearSelection()
        dgvResultados.Rows(nuevoIndex).Selected = True
        dgvResultados.CurrentCell = dgvResultados.Rows(nuevoIndex).Cells("CI")

        '— Forzamos refresco del detalle (por si el evento no se dispara)
        MostrarDetalle(Nothing, EventArgs.Empty)
    End Sub


#End Region

    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        ' Ejecuta búsqueda "manual" sin cancelación (si querés cancelar, usá un token)
        Await BuscarAsync()
    End Sub

#Region "Detalle lateral (Foto on-demand)"
    Private Async Sub MostrarDetalle(sender As Object, e As EventArgs)
        If dgvResultados.CurrentRow Is Nothing OrElse dgvResultados.CurrentRow.DataBoundItem Is Nothing Then Return
        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)
        Dim fechaActual = Date.Today

        Using uow As New UnitOfWork()
            Dim f = Await uow.Repository(Of Funcionario)() _
                         .GetAll() _
                         .Include(Function(x) x.Cargo) _
                         .Include(Function(x) x.TipoFuncionario) _
                         .Include(Function(x) x.EstadoTransitorio.Select(Function(et) et.TipoEstadoTransitorio)) _
                         .AsNoTracking() _
                         .Where(Function(x) x.Id = id) _
                         .Select(Function(x) New With {
                             x.Foto,
                             x.Nombre,
                             x.CI,
                             x.Cargo,
                             x.TipoFuncionario,
                             x.FechaIngreso,
                             x.Activo,
                             .EstadosTransitoriosActivos = x.EstadoTransitorio.Where(
                                 Function(et) et.FechaDesde <= fechaActual AndAlso
                                              (Not et.FechaHasta.HasValue OrElse et.FechaHasta.Value >= fechaActual)
                             ).Select(Function(et) et.TipoEstadoTransitorio.Nombre).ToList()
                         }).FirstOrDefaultAsync()

            If dgvResultados.CurrentRow Is Nothing _
       OrElse dgvResultados.CurrentRow.DataBoundItem Is Nothing _
       OrElse CInt(dgvResultados.CurrentRow.Cells("Id").Value) <> id Then Return

            If f Is Nothing Then Return

            lblCI.Text = f.CI
            lblNombreCompleto.Text = $"{f.Nombre}"
            lblCargo.Text = If(f.Cargo Is Nothing, "-", f.Cargo.Nombre)
            lblTipo.Text = f.TipoFuncionario.Nombre
            lblFechaIngreso.Text = f.FechaIngreso.ToShortDateString()
            chkActivoDetalle.Checked = f.Activo

            ' --- LÓGICA MEJORADA PARA MOSTRAR ESTADOS Y LICENCIAS ---
            Dim licenciasActivas = Await uow.Repository(Of HistoricoLicencia)() _
            .GetAll() _
            .Where(Function(l) l.FuncionarioId = id AndAlso
                               l.inicio <= fechaActual AndAlso
                               l.finaliza >= fechaActual AndAlso
                               l.estado IsNot Nothing AndAlso l.estado <> "Rechazado" AndAlso l.estado <> "Anulado") _
            .Select(Function(l) l.TipoLicencia.Nombre) _
            .ToListAsync()

            Dim todosLosEstados As New List(Of String)
            If f.EstadosTransitoriosActivos IsNot Nothing Then
                todosLosEstados.AddRange(f.EstadosTransitoriosActivos)
            End If

            If licenciasActivas IsNot Nothing AndAlso licenciasActivas.Any() Then
                Dim yaTieneLicencia = todosLosEstados.Any(Function(st) st.ToLower().Contains("licencia"))
                If Not yaTieneLicencia Then
                    todosLosEstados.AddRange(licenciasActivas)
                End If
            End If

            If todosLosEstados.Any() Then
                lblEstadoTransitorio.Text = String.Join(", ", todosLosEstados)
            Else
                lblEstadoTransitorio.Text = "Normal"
            End If
            ' --- FIN DE LA LÓGICA MEJORADA ---

            If f.Foto Is Nothing OrElse f.Foto.Length = 0 Then
                pbFotoDetalle.Image = My.Resources.Police
            Else
                Using ms As New MemoryStream(f.Foto)
                    pbFotoDetalle.Image = Image.FromStream(ms)
                End Using
            End If
            ' Mostrar presencia
            Dim hoy As Date = Date.Today
            lblPresencia.Text = Await ObtenerPresenciaAsync(id, hoy)
        End Using
    End Sub

    Private Async Sub OnDgvDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If dgvResultados.CurrentRow Is Nothing Then Return
        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)
        Using frm As New frmFuncionarioCrear(id)
            If frm.ShowDialog() = DialogResult.OK Then
                Await BuscarAsync()
            End If
        End Using
    End Sub

    ''' <summary>
    ''' Devuelve “Presente”, “Franco”, “COVID”, etc. para un funcionario y fecha dada.
    ''' </summary>
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
    '--- Panel lateral  -------------------------------------------------
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