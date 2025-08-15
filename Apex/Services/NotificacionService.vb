Imports System.Data.Entity
Imports System.Data.SqlClient

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

    ''' <summary>
    ''' Obtiene notificaciones usando Full-Text Search de forma correcta.
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(
        Optional filtroNombreFuncionario As String = "",
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing
    ) As Task(Of List(Of vw_NotificacionesCompletas))

        Dim sqlBuilder As New System.Text.StringBuilder("SELECT * FROM vw_NotificacionesCompletas WHERE 1=1")
        Dim parameters As New List(Of Object)

        ' --- INICIO DE LA OPTIMIZACIÓN ---
        If fechaDesde.HasValue Then
            sqlBuilder.Append(" AND FechaProgramada >= @p" & parameters.Count)
            parameters.Add(New SqlParameter("@p" & parameters.Count, fechaDesde.Value))
        End If

        If fechaHasta.HasValue Then
            sqlBuilder.Append(" AND FechaProgramada <= @p" & parameters.Count)
            parameters.Add(New SqlParameter("@p" & parameters.Count, fechaHasta.Value))
        End If
        ' --- FIN DE LA OPTIMIZACIÓN ---

        If Not String.IsNullOrWhiteSpace(filtroNombreFuncionario) Then
            Dim terminos = filtroNombreFuncionario.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).Select(Function(w) $"""{w}*""")
            Dim expresionFts = String.Join(" AND ", terminos)
            sqlBuilder.Append($" AND FuncionarioId IN (SELECT Id FROM dbo.Funcionario WHERE CONTAINS((Nombre, CI), @p{parameters.Count}))")
            parameters.Add(New SqlParameter($"@p{parameters.Count}", expresionFts))
        End If

        sqlBuilder.Append(" ORDER BY FechaProgramada DESC")

        Dim query = _unitOfWork.Context.Database.SqlQuery(Of vw_NotificacionesCompletas)(sqlBuilder.ToString(), parameters.ToArray())
        Return Await query.ToListAsync()
    End Function

    Public Async Function UpdateEstadoAsync(notificacionId As Integer, nuevoEstadoId As Byte) As Task
        Dim notificacion = Await _unitOfWork.Repository(Of NotificacionPersonal)().GetByIdAsync(notificacionId)
        If notificacion IsNot Nothing Then
            notificacion.EstadoId = nuevoEstadoId
            notificacion.UpdatedAt = DateTime.Now
            _unitOfWork.Repository(Of NotificacionPersonal)().Update(notificacion)
            Await _unitOfWork.CommitAsync()
        End If
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