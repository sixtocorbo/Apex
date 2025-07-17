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

    Private debounce As New DebounceDispatcherAsync()
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

            .Columns.Add(New DataGridViewTextBoxColumn With {.Name = "Id", .DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "CI", .HeaderText = "CI"})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Nombre", .HeaderText = "Nombre"})
        End With

        AddHandler dgvResultados.SelectionChanged, AddressOf MostrarDetalle
        AddHandler dgvResultados.CellDoubleClick, AddressOf OnDgvDoubleClick
        AddHandler dgvResultados.DataError, Sub(s, ev) ev.ThrowException = False
    End Sub
#End Region

#Region "Búsqueda con Full-Text y CONTAINS"
    Private Async Function BuscarAsync(tok As CancellationToken) As Task
        btnBuscar.Enabled = False

        Try
            Using uow As New UnitOfWork()
                Dim ctx = uow.Context
                Dim filtro As String = txtBusqueda.Text.Trim()

                '–– Condición mínima de 3 letras ––
                If filtro.Length < 3 OrElse tok.IsCancellationRequested Then
                    dgvResultados.DataSource = Nothing
                    ResultadosFiltrados = New List(Of FuncionarioMin)
                    Return
                End If

                '–– Construir patrón FTS por término ––
                Dim terminos = filtro.Split(" "c) _
                                .Where(Function(w) Not String.IsNullOrWhiteSpace(w)) _
                                .Select(Function(w) $"""{w}*""")
                Dim expresionFts = String.Join(" AND ", terminos)

                '–– SQL dinámico ––
                Dim sb As New StringBuilder()
                sb.AppendLine("SELECT TOP (@limite)")
                sb.AppendLine("       Id, CI, Nombre")
                sb.AppendLine("FROM   dbo.Funcionario WITH (NOLOCK)")
                sb.AppendLine("WHERE  1 = 1")
                sb.AppendLine("  AND (CI LIKE '%' + @filtro + '%'")
                sb.AppendLine("   OR  CONTAINS(Nombre, @patron))")
                sb.AppendLine("ORDER BY Nombre;")

                Dim sql = sb.ToString()
                Dim pLimite = New SqlParameter("@limite", LIMITE_FILAS)
                Dim pFiltro = New SqlParameter("@filtro", filtro)
                Dim pPatron = New SqlParameter("@patron", expresionFts)

                tok.ThrowIfCancellationRequested()

                '–– EJECUTO sin token para evitar cancelación en mitad del Open ––
                Dim lista = Await ctx.Database _
                            .SqlQuery(Of FuncionarioMin)(sql, pLimite, pFiltro, pPatron) _
                            .ToListAsync()

                If tok.IsCancellationRequested Then Return

                '–– Actualizo la grilla en hilo UI ––
                dgvResultados.DataSource = Nothing
                If lista.Any() Then
                    dgvResultados.DataSource = lista
                    dgvResultados.ClearSelection()
                End If
                ResultadosFiltrados = lista

                If lista.Count = LIMITE_FILAS Then
                    MessageBox.Show($"Mostrando los primeros {LIMITE_FILAS} resultados. " &
                                "Refiná la búsqueda para ver más.",
                                "Aviso",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using

        Catch ex As OperationCanceledException
            ' Silencioso
        Catch ex As SqlException When ex.Number = -2
            MessageBox.Show("La consulta excedió el tiempo de espera. Refiná los filtros o intentá nuevamente.",
                        "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            btnBuscar.Enabled = True
        End Try
    End Function
#End Region

    Private Async Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        ' Ejecuta búsqueda "manual" sin cancelación (si querés cancelar, usá un token)
        Await BuscarAsync(CancellationToken.None)
    End Sub

#Region "Debounce en el TextBox"
    Private Sub txtBusqueda_TextChanged(sender As Object, e As EventArgs) Handles txtBusqueda.TextChanged
        If txtBusqueda.Text.Trim().Length >= 3 Then
            debounce.Debounce(400, Async Function(token)
                                       Await BuscarAsync(token)
                                   End Function)
        Else
            debounce.Cancel()
            If dgvResultados.DataSource IsNot Nothing Then
                dgvResultados.DataSource = Nothing
            End If
            ResultadosFiltrados = New List(Of FuncionarioMin)
        End If
    End Sub
#End Region

#Region "Detalle lateral (Foto on-demand)"
    Private Async Sub MostrarDetalle(sender As Object, e As EventArgs)
        If dgvResultados.CurrentRow Is Nothing OrElse dgvResultados.CurrentRow.DataBoundItem Is Nothing Then Return
        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)

        Using uow As New UnitOfWork()
            Dim f = Await uow.Repository(Of Funcionario)() _
                             .GetAll() _
                             .Include(Function(x) x.Cargo) _
                             .Include(Function(x) x.TipoFuncionario) _
                             .AsNoTracking() _
                             .Where(Function(x) x.Id = id) _
                             .Select(Function(x) New With {
                                 x.Foto,
                                 x.Nombre,
                                 x.CI,
                                 x.Cargo,
                                 x.TipoFuncionario,
                                 x.FechaIngreso,
                                 x.Activo
                             }).FirstOrDefaultAsync()
            If f Is Nothing Then Return

            lblCI.Text = f.CI
            lblNombreCompleto.Text = $"{f.Nombre}"
            lblCargo.Text = If(f.Cargo Is Nothing, "-", f.Cargo.Nombre)
            lblTipo.Text = f.TipoFuncionario.Nombre
            lblFechaIngreso.Text = f.FechaIngreso.ToShortDateString()
            chkActivoDetalle.Checked = f.Activo

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
                Await BuscarAsync(CancellationToken.None)
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


