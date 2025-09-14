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
End Class
