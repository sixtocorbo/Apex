Imports System.Data.Entity
Imports System.Data.SqlClient

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

    Public Async Function GetAllConDetallesAsync(
        Optional filtroNombre As String = "",
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing
    ) As Task(Of List(Of vw_SancionesCompletas))

        Dim sqlBuilder As New System.Text.StringBuilder("SELECT * FROM vw_SancionesCompletas WHERE 1=1")
        Dim parameters As New List(Of Object)

        If fechaDesde.HasValue Then
            sqlBuilder.Append(" AND FechaHasta >= @p0")
            parameters.Add(New SqlParameter("@p0", fechaDesde.Value))
        End If

        If fechaHasta.HasValue Then
            sqlBuilder.Append(" AND FechaDesde <= @p" & parameters.Count)
            parameters.Add(New SqlParameter("@p" & parameters.Count, fechaHasta.Value))
        End If

        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
            Dim terminos = filtroNombre.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).Select(Function(w) $"""{w}*""")
            Dim expresionFts = String.Join(" AND ", terminos)

            sqlBuilder.Append($" AND FuncionarioId IN (SELECT Id FROM dbo.Funcionario WHERE CONTAINS((Nombre, CI), @p{parameters.Count}))")
            parameters.Add(New SqlParameter($"@p{parameters.Count}", expresionFts))
        End If

        sqlBuilder.Append(" ORDER BY FechaDesde DESC")

        Dim query = _unitOfWork.Context.Database.SqlQuery(Of vw_SancionesCompletas)(sqlBuilder.ToString(), parameters.ToArray())
        Return Await query.ToListAsync()
    End Function
End Class