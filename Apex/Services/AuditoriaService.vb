Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class AuditoriaService
    Inherits GenericService(Of RegistroActividad)

    Public Sub New()
        ' Llama al constructor de la clase base con una nueva UnitOfWork.
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        ' Permite inyectar una UnitOfWork existente.
        MyBase.New(unitOfWork)
    End Sub

    ''' <summary>
    ''' Registra un nuevo evento de actividad en el sistema (hace SaveChanges).
    ''' </summary>
    Public Async Function RegistrarActividadAsync(accion As String, tipoEntidad As String, descripcion As String, Optional entidadId As Integer? = Nothing) As Task
        Dim usuarioActual = My.User.Name ' O la forma que uses para identificar al usuario

        Dim nuevoRegistro As New RegistroActividad With {
            .FechaHora = DateTime.Now,
            .Usuario = usuarioActual,
            .Accion = accion,
            .TipoEntidad = tipoEntidad,
            .EntidadId = entidadId,
            .Descripcion = descripcion
        }

        ' Usa el CreateAsync de la clase base (SaveChanges incluido).
        Await MyBase.CreateAsync(nuevoRegistro)
    End Function

    ''' <summary>
    ''' Encola un evento de auditoría en el DbContext compartido SIN commitear.
    ''' Úsalo dentro de una transacción para confirmar todo junto con CommitAsync().
    ''' </summary>
    Public Function EncolarActividad(accion As String, tipoEntidad As String, descripcion As String, Optional entidadId As Integer? = Nothing) As RegistroActividad
        Dim usuarioActual = My.User.Name ' O tu IUserContext si tenés
        Dim reg As New RegistroActividad With {
            .FechaHora = DateTime.Now,
            .Usuario = usuarioActual,
            .Accion = accion,
            .TipoEntidad = tipoEntidad,
            .EntidadId = entidadId,
            .Descripcion = descripcion
        }
        ' Nota: _unitOfWork es accesible desde GenericService (Protected); ya lo usás en ObtenerActividadPorFechaAsync.
        _unitOfWork.Repository(Of RegistroActividad)().Add(reg)
        Return reg
    End Function

    ''' <summary>
    ''' Obtiene todas las actividades registradas en una fecha específica.
    ''' </summary>
    Public Async Function ObtenerActividadPorFechaAsync(fecha As Date) As Task(Of List(Of RegistroActividad))
        Return Await _unitOfWork.Repository(Of RegistroActividad)().GetAll().AsNoTracking().
            Where(Function(a) CBool(DbFunctions.TruncateTime(a.FechaHora) = fecha.Date)).
            OrderByDescending(Function(a) a.FechaHora).
            ToListAsync()
    End Function

    ' (Opcional) Helper para registrar sin romper el flujo si la auditoría falla.
    Public Async Function RegistrarAuditoriaSeguraAsync(accion As String, tipoEntidad As String, descripcion As String, Optional entidadId As Integer? = Nothing) As Task
        Try
            Await RegistrarActividadAsync(accion, tipoEntidad, descripcion, entidadId)
        Catch
            ' Log opcional; no relanzamos.
        End Try
    End Function
End Class
