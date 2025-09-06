Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Text

' ---------- DTOs de entrada ----------
Public Class NotificacionCreateRequest
    Public Property FuncionarioId As Integer
    Public Property TipoNotificacionId As Byte
    Public Property FechaProgramada As Date
    Public Property Medio As String
    Public Property Documento As String
    Public Property ExpMinisterial As String
    Public Property ExpINR As String
    Public Property Oficina As String
End Class

Public Class NotificacionUpdateRequest
    Public Property Id As Integer
    Public Property FuncionarioId As Integer
    Public Property TipoNotificacionId As Byte
    Public Property FechaProgramada As Date
    Public Property Medio As String
    Public Property Documento As String
    Public Property ExpMinisterial As String
    Public Property ExpINR As String
    Public Property Oficina As String
End Class
' -------------------------------------

Public Class NotificacionService
    Inherits GenericService(Of NotificacionPersonal)

    Private Shadows ReadOnly _unitOfWork As IUnitOfWork
    Private ReadOnly _auditoria As AuditoriaService

    Public Sub New()
        MyBase.New(New UnitOfWork())
        _unitOfWork = MyBase._unitOfWork
        _auditoria = New AuditoriaService(_unitOfWork)
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
        _unitOfWork = unitOfWork
        _auditoria = New AuditoriaService(_unitOfWork)
    End Sub

    ' ----------------- Helpers -----------------
    Private Sub ValidarCreate(r As NotificacionCreateRequest)
        If r Is Nothing Then Throw New ArgumentNullException(NameOf(r))
        If r.FuncionarioId <= 0 Then Throw New InvalidOperationException("Debe seleccionar un funcionario.")
        If r.TipoNotificacionId = 0 Then Throw New InvalidOperationException("Debe seleccionar un tipo de notificación.")
        If r.FechaProgramada = Date.MinValue Then Throw New InvalidOperationException("Debe indicar una fecha programada válida.")
    End Sub

    Private Sub ValidarUpdate(r As NotificacionUpdateRequest)
        If r Is Nothing Then Throw New ArgumentNullException(NameOf(r))
        If r.Id <= 0 Then Throw New InvalidOperationException("Id de notificación inválido.")
        ValidarCreate(New NotificacionCreateRequest With {
            .FuncionarioId = r.FuncionarioId,
            .TipoNotificacionId = r.TipoNotificacionId,
            .FechaProgramada = r.FechaProgramada,
            .Medio = r.Medio,
            .Documento = r.Documento,
            .ExpMinisterial = r.ExpMinisterial,
            .ExpINR = r.ExpINR,
            .Oficina = r.Oficina
        })
    End Sub

    Private Function FormatearFts(filtro As String) As String
        Dim terminos = filtro.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).
            Select(Function(w) $"""{w.Trim()}*""")
        Return String.Join(" AND ", terminos)
    End Function

    ' Si no querés depender de una constante para “Pendiente”, resolvelo por DB:
    Private Async Function ObtenerEstadoNotiPendienteAsync() As Task(Of Byte)
        Dim repo = _unitOfWork.Repository(Of NotificacionEstado)()
        ' 1) Por orden = 1
        Dim id As Byte = CByte(Await repo.GetAll().
            Where(Function(e) e.Orden = 1).
            Select(Function(e) e.Id).
            FirstOrDefaultAsync())
        If id = 0 Then
            ' 2) Por nombre literal
            id = CByte(Await repo.GetAll().
                Where(Function(e) e.Nombre = "Pendiente").
                Select(Function(e) e.Id).
                FirstOrDefaultAsync())
        End If
        If id = 0 Then Throw New InvalidOperationException("No se encontró el estado 'Pendiente' en NotificacionEstado.")
        Return id
    End Function

    ' ----------------- Lecturas -----------------
    Public Async Function GetByIdParaEdicionAsync(id As Integer) As Task(Of NotificacionPersonal)
        Return Await _unitOfWork.Repository(Of NotificacionPersonal)().
            GetAll().
            Include(Function(n) n.Funcionario).
            Include(Function(n) n.TipoNotificacion).
            FirstOrDefaultAsync(Function(n) n.Id = id)
    End Function

    Public Async Function GetAllConDetallesAsync(
        Optional filtro As String = "",
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing
    ) As Task(Of List(Of vw_NotificacionesCompletas))

        Dim baseSql As String = "SELECT vwn.* FROM vw_NotificacionesCompletas AS vwn"
        Dim parameters As New List(Of SqlParameter)

        If Not String.IsNullOrWhiteSpace(filtro) Then
            baseSql &= " INNER JOIN dbo.Funcionario AS f ON vwn.FuncionarioId = f.Id WHERE CONTAINS((f.CI, f.Nombre), @p0)"
            parameters.Add(New SqlParameter("@p0", FormatearFts(filtro)))
        Else
            baseSql &= " WHERE 1=1"
        End If

        Dim sb As New StringBuilder(baseSql)

        If fechaDesde.HasValue Then
            Dim pname = "@p" & parameters.Count
            sb.Append(" AND vwn.FechaProgramada >= " & pname)
            parameters.Add(New SqlParameter(pname, fechaDesde.Value))
        End If

        If fechaHasta.HasValue Then
            Dim pname = "@p" & parameters.Count
            sb.Append(" AND vwn.FechaProgramada <= " & pname)
            parameters.Add(New SqlParameter(pname, fechaHasta.Value))
        End If

        sb.Append(" ORDER BY vwn.FechaProgramada DESC")

        Dim query = _unitOfWork.Context.Database.SqlQuery(Of vw_NotificacionesCompletas)(
            sb.ToString(), parameters.ToArray()
        )
        Return Await query.ToListAsync()
    End Function

    ' ----------------- Escrituras con transacción + auditoría -----------------

    Public Async Function CreateNotificacionAsync(req As NotificacionCreateRequest) As Task(Of NotificacionPersonal)
        ValidarCreate(req)

        ' Validaciones de existencia (para mensajes claros y evitar FK)
        Dim funcExiste = Await _unitOfWork.Repository(Of Funcionario)().
            AnyAsync(Function(f) f.Id = req.FuncionarioId)
        If Not funcExiste Then Throw New InvalidOperationException("El funcionario seleccionado no existe.")

        Dim tipoExiste = Await _unitOfWork.Repository(Of TipoNotificacion)().
            AnyAsync(Function(t) t.Id = req.TipoNotificacionId)
        If Not tipoExiste Then Throw New InvalidOperationException("El tipo de notificación seleccionado no existe.")

        ' Estado “Pendiente” resuelto por DB (evita constantes mágicas)
        Dim estadoPendiente As Byte = Await ObtenerEstadoNotiPendienteAsync()

        Dim entidad As New NotificacionPersonal With {
            .FuncionarioId = req.FuncionarioId,
            .TipoNotificacionId = req.TipoNotificacionId,
            .FechaProgramada = req.FechaProgramada,
            .Medio = req.Medio?.Trim(),
            .Documento = req.Documento?.Trim(),
            .ExpMinisterial = req.ExpMinisterial?.Trim(),
            .ExpINR = req.ExpINR?.Trim(),
            .Oficina = req.Oficina?.Trim(),
            .EstadoId = estadoPendiente,
            .CreatedAt = DateTime.Now
        }

        Using tx = _unitOfWork.Context.Database.BeginTransaction()
            Try
                _unitOfWork.Repository(Of NotificacionPersonal)().Add(entidad)

                Dim desc = $"Creación Notificación (Func #{req.FuncionarioId}, Tipo #{req.TipoNotificacionId}, Prog {req.FechaProgramada:yyyy-MM-dd HH:mm})."
                _auditoria.EncolarActividad("Crear", "NotificacionPersonal", desc, Nothing)

                Await _unitOfWork.CommitAsync()
                tx.Commit()
                Return entidad
            Catch
                tx.Rollback()
                Throw
            End Try
        End Using
    End Function

    Public Async Function UpdateNotificacionAsync(req As NotificacionUpdateRequest) As Task
        ValidarUpdate(req)

        Dim entidad = Await _unitOfWork.Repository(Of NotificacionPersonal)().
            GetAll().
            FirstOrDefaultAsync(Function(n) n.Id = req.Id)

        If entidad Is Nothing Then Throw New InvalidOperationException("La notificación no existe.")

        entidad.FuncionarioId = req.FuncionarioId
        entidad.TipoNotificacionId = req.TipoNotificacionId
        entidad.FechaProgramada = req.FechaProgramada
        entidad.Medio = req.Medio?.Trim()
        entidad.Documento = req.Documento?.Trim()
        entidad.ExpMinisterial = req.ExpMinisterial?.Trim()
        entidad.ExpINR = req.ExpINR?.Trim()
        entidad.Oficina = req.Oficina?.Trim()
        entidad.UpdatedAt = DateTime.Now

        Using tx = _unitOfWork.Context.Database.BeginTransaction()
            Try
                Dim desc = $"Actualización Notif #{entidad.Id} (Func #{req.FuncionarioId}, Tipo #{req.TipoNotificacionId}, Prog {req.FechaProgramada:yyyy-MM-dd HH:mm})."
                _auditoria.EncolarActividad("Actualizar", "NotificacionPersonal", desc, entidad.Id)

                Await _unitOfWork.CommitAsync()
                tx.Commit()
            Catch
                tx.Rollback()
                Throw
            End Try
        End Using
    End Function

    Public Async Function DeleteNotificacionAsync(id As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NotificacionPersonal)()
        Dim entidad = Await repo.GetByIdAsync(id)
        If entidad Is Nothing Then Throw New InvalidOperationException("La notificación no existe o ya fue eliminada.")

        Using tx = _unitOfWork.Context.Database.BeginTransaction()
            Try
                repo.Remove(entidad)

                Dim desc = $"Eliminación Notif #{id} (Func #{entidad.FuncionarioId}, Tipo #{entidad.TipoNotificacionId}, Prog {entidad.FechaProgramada:yyyy-MM-dd HH:mm})."
                _auditoria.EncolarActividad("Eliminar", "NotificacionPersonal", desc, id)

                Await _unitOfWork.CommitAsync()
                tx.Commit()
            Catch
                tx.Rollback()
                Throw
            End Try
        End Using
    End Function

    ''' <summary>
    ''' Actualiza el estado de una notificación (atómico con auditoría).
    ''' </summary>
    Public Async Function UpdateEstadoAsync(notificacionId As Integer, nuevoEstadoId As Byte) As Task(Of Boolean)
        Using tx = _unitOfWork.Context.Database.BeginTransaction()
            Try
                Dim sql As String = "UPDATE NotificacionPersonal SET EstadoId = @p1, UpdatedAt = @p2 WHERE Id = @p0"
                Dim afectadas = Await _unitOfWork.Context.Database.ExecuteSqlCommandAsync(
                    sql,
                    New SqlParameter("@p0", notificacionId),
                    New SqlParameter("@p1", nuevoEstadoId),
                    New SqlParameter("@p2", DateTime.Now)
                )

                If afectadas > 0 Then
                    Dim desc = $"Cambio de estado Notif #{notificacionId} → EstadoId={nuevoEstadoId}."
                    _auditoria.EncolarActividad("ActualizarEstado", "NotificacionPersonal", desc, notificacionId)
                    Await _unitOfWork.CommitAsync()
                    tx.Commit()
                    Return True
                Else
                    tx.Rollback()
                    Return False
                End If
            Catch
                tx.Rollback()
                Throw
            End Try
        End Using
    End Function

    ' ----------------- Combos -----------------
    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking().
            Where(Function(f) f.Activo).
            OrderBy(Function(f) f.Nombre).
            Select(Function(f) New With {.Id = f.Id, .Nombre = f.Nombre}).
            ToListAsync()
        Return lista.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
    End Function

    Public Async Function ObtenerTiposNotificacionParaComboAsync() As Task(Of List(Of KeyValuePair(Of Byte, String)))
        Dim lista = Await _unitOfWork.Repository(Of TipoNotificacion)().GetAll().AsNoTracking().
            OrderBy(Function(t) t.Orden).
            Select(Function(t) New With {.Id = t.Id, .Nombre = t.Nombre}).
            ToListAsync()
        Return lista.Select(Function(t) New KeyValuePair(Of Byte, String)(CByte(t.Id), t.Nombre)).ToList()
    End Function

    Public Async Function ObtenerEstadosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Byte, String)))
        Dim lista = Await _unitOfWork.Repository(Of NotificacionEstado)().GetAll().AsNoTracking().
            OrderBy(Function(e) e.Orden).
            Select(Function(e) New With {.Id = e.Id, .Nombre = e.Nombre}).
            ToListAsync()
        Return lista.Select(Function(e) New KeyValuePair(Of Byte, String)(e.Id, e.Nombre)).ToList()
    End Function

End Class
