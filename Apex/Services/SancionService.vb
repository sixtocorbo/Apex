Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Text

Public Class SancionService
    Inherits GenericService(Of EstadoTransitorio)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    Public ReadOnly Property UnitOfWork As IUnitOfWork
        Get
            Return _unitOfWork
        End Get
    End Property

    ''' <summary>
    ''' Trae sanciones desde la vista vw_SancionesCompletas con filtros opcionales.
    ''' IMPORTANTE: La vista debe exponer las columnas usadas aquí (FechaDesde, FechaHasta, TipoLicenciaId).
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(
        Optional filtroNombre As String = "",
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing,
        Optional tipoLicenciaId As Integer? = Nothing
    ) As Task(Of List(Of vw_SancionesCompletas))

        Dim sb As New StringBuilder("SELECT * FROM vw_SancionesCompletas WHERE 1=1")
        Dim parameters As New List(Of SqlParameter)

        ' Helper local para agregar parámetros con índice correcto
        Dim AddParam As Func(Of Object, String) =
            Function(val As Object) As String
                Dim pname = "@p" & parameters.Count
                parameters.Add(New SqlParameter(pname, If(val Is Nothing, DBNull.Value, val)))
                Return pname
            End Function

        ' Rango de fechas (se solapan con el período)
        If fechaDesde.HasValue Then
            sb.Append(" AND FechaHasta >= ").Append(AddParam(fechaDesde.Value))
        End If

        If fechaHasta.HasValue Then
            sb.Append(" AND FechaDesde <= ").Append(AddParam(fechaHasta.Value))
        End If

        ' Búsqueda por nombre/CI con FTS sobre Funcionario
        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
            Dim terminos = filtroNombre.Split({" "c}, StringSplitOptions.RemoveEmptyEntries) _
                                       .Select(Function(w) $"""{w.Trim()}*""")
            Dim expresionFts = String.Join(" AND ", terminos)
            sb.Append(" AND FuncionarioId IN (SELECT Id FROM dbo.Funcionario WHERE CONTAINS((Nombre, CI), ")
            sb.Append(AddParam(expresionFts)).Append("))")
        End If

        ' Filtro por tipo de licencia (la vista debe exponer TipoLicenciaId)
        If tipoLicenciaId.HasValue AndAlso tipoLicenciaId.Value > 0 Then
            sb.Append(" AND TipoLicenciaId = ").Append(AddParam(tipoLicenciaId.Value))
        End If

        sb.Append(" ORDER BY FechaDesde DESC")

        ' Ejecutar
        Dim sql = sb.ToString()
        Dim args = parameters.Cast(Of Object)().ToArray()

        Dim query = _unitOfWork.Context.Database.SqlQuery(Of vw_SancionesCompletas)(sql, args)
        Return Await query.ToListAsync()
    End Function
    Public Async Function GetListadoAsync(
    Optional filtroNombreOCi As String = "",
    Optional filtroTipoSancion As String = Nothing
) As Task(Of List(Of SancionListadoItem))

        ' Armamos SQL sobre la vista de sanciones
        Dim sb As New StringBuilder("SELECT Id, NombreFuncionario, FechaDesde, FechaHasta, Estado, Comentario, TipoSancion FROM vw_SancionesCompletas WHERE 1=1")
        Dim ps As New List(Of SqlParameter)

        ' Filtro por nombre/CI (FTS)
        If Not String.IsNullOrWhiteSpace(filtroNombreOCi) Then
            Dim terminos = filtroNombreOCi.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).
                                       Select(Function(w) $"""{w.Trim()}*""")
            Dim fts = String.Join(" AND ", terminos)
            sb.Append(" AND FuncionarioId IN (SELECT Id FROM dbo.Funcionario WHERE CONTAINS((Nombre, CI), @p" & ps.Count & "))")
            ps.Add(New SqlParameter("@p" & (ps.Count), fts))
        End If

        ' Filtro por tipo de sanción (texto exacto que cargás en frmSancionCrear)
        If Not String.IsNullOrWhiteSpace(filtroTipoSancion) Then
            sb.Append(" AND TipoSancion = @p" & ps.Count)
            ps.Add(New SqlParameter("@p" & (ps.Count), filtroTipoSancion))
        End If

        sb.Append(" ORDER BY FechaDesde DESC")

        ' Ejecutamos y proyectamos directo a DTO
        Dim raw = Await _unitOfWork.Context.Database _
       .SqlQuery(Of VwSancionRow)(sb.ToString(), ps.Cast(Of Object).ToArray()) _
       .ToListAsync() ' ← quita AsNoTracking

        Return raw.Select(Function(r) New SancionListadoItem With {
            .Id = r.Id,
            .NombreFuncionario = r.NombreFuncionario,
            .FechaDesde = r.FechaDesde,
            .FechaHasta = r.FechaHasta,
            .Estado = r.Estado,
            .Comentario = r.Comentario,
            .TipoSancion = r.TipoSancion
        }).ToList()

    End Function

    ' Row helper para mapear columnas de la vista (nombres iguales a la vista)
    Private Class VwSancionRow
        Public Property Id As Integer
        Public Property NombreFuncionario As String
        Public Property FechaDesde As Date
        Public Property FechaHasta As Date?
        Public Property Estado As String
        Public Property Comentario As String
        Public Property TipoSancion As String
    End Class

End Class
