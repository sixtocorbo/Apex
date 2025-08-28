Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Text

Public Class NotificacionService
    Inherits GenericService(Of NotificacionPersonal)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    Public Async Function GetByIdParaEdicionAsync(id As Integer) As Task(Of NotificacionPersonal)
        Return Await _unitOfWork.Repository(Of NotificacionPersonal)().
            GetAll().
            Include(Function(n) n.Funcionario).
            Include(Function(n) n.TipoNotificacion).
            FirstOrDefaultAsync(Function(n) n.Id = id)
    End Function

    ' En tu archivo: Apex/Services/NotificacionService.vb

    Public Async Function GetAllConDetallesAsync(
    Optional filtro As String = "",
    Optional fechaDesde As Date? = Nothing,
    Optional fechaHasta As Date? = Nothing
) As Task(Of List(Of vw_NotificacionesCompletas))

        ' La consulta base empieza seleccionando desde la vista
        Dim sql As String = "SELECT vwn.* FROM vw_NotificacionesCompletas AS vwn"
        Dim parameters As New List(Of Object)

        ' Si hay un filtro de texto, unimos con la tabla Funcionario para usar su índice Full-Text
        If Not String.IsNullOrWhiteSpace(filtro) Then
            Dim terminos = filtro.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).Select(Function(w) $"""{w}*""")
            Dim expresionFts = String.Join(" AND ", terminos)

            ' UNIMOS con Funcionario (f) y aplicamos el CONTAINS sobre la tabla real (f.CI, f.Nombre)
            sql &= " INNER JOIN dbo.Funcionario AS f ON vwn.FuncionarioId = f.Id WHERE CONTAINS((f.CI, f.Nombre), @p0)"
            parameters.Add(New SqlParameter("@p0", expresionFts))
        Else
            ' Si no hay filtro, simplemente agregamos un WHERE que no hace nada
            sql &= " WHERE 1=1"
        End If

        ' Construimos el resto de la consulta
        Dim sqlBuilder As New System.Text.StringBuilder(sql)

        If fechaDesde.HasValue Then
            sqlBuilder.Append(" AND vwn.FechaProgramada >= @p" & parameters.Count)
            parameters.Add(New SqlParameter("@p" & parameters.Count, fechaDesde.Value))
        End If

        If fechaHasta.HasValue Then
            sqlBuilder.Append(" AND vwn.FechaProgramada <= @p" & parameters.Count)
            parameters.Add(New SqlParameter("@p" & parameters.Count, fechaHasta.Value))
        End If

        sqlBuilder.Append(" ORDER BY vwn.FechaProgramada DESC")

        ' Ejecutamos la consulta final
        Dim query = _unitOfWork.Context.Database.SqlQuery(Of vw_NotificacionesCompletas)(sqlBuilder.ToString(), parameters.ToArray())
        Return Await query.ToListAsync()
    End Function

    ''' <summary>
    ''' Actualiza el estado de una notificación de forma más eficiente.
    ''' </summary>
    Public Async Function UpdateEstadoAsync(notificacionId As Integer, nuevoEstadoId As Byte) As Task(Of Boolean)
        ' Este enfoque es más eficiente: ejecuta una sola sentencia UPDATE en la BD
        ' sin necesidad de traer la entidad primero. Previene condiciones de carrera.
        Dim sql As String = "UPDATE NotificacionPersonal SET EstadoId = @p1, UpdatedAt = @p2 WHERE Id = @p0"

        Dim result = Await _unitOfWork.Context.Database.ExecuteSqlCommandAsync(
            sql,
            New SqlParameter("@p0", notificacionId),
            New SqlParameter("@p1", nuevoEstadoId),
            New SqlParameter("@p2", DateTime.Now)
        )

        ' ExecuteSqlCommandAsync retorna el número de filas afectadas.
        ' Devolvemos True si se afectó al menos una fila.
        Return result > 0
    End Function

    ' --- MÉTODOS PARA COMBOS (sin cambios) ---
    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim repo = _unitOfWork.Repository(Of Funcionario)()
        Dim lista = Await repo.GetAll().AsNoTracking().Where(Function(f) f.Activo).OrderBy(Function(f) f.Nombre).ToListAsync()
        Return lista.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
    End Function

    Public Async Function ObtenerTiposNotificacionParaComboAsync() As Task(Of List(Of KeyValuePair(Of Byte, String)))
        Dim repo = _unitOfWork.Repository(Of TipoNotificacion)()
        Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(t) t.Orden).ToListAsync()
        Return lista.Select(Function(t) New KeyValuePair(Of Byte, String)(t.Id, t.Nombre)).ToList()
    End Function

    Public Async Function ObtenerEstadosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Byte, String)))
        Dim repo = _unitOfWork.Repository(Of NotificacionEstado)()
        Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(e) e.Orden).ToListAsync()
        Return lista.Select(Function(e) New KeyValuePair(Of Byte, String)(e.Id, e.Nombre)).ToList()
    End Function

End Class