Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class AuditoriaService
    Inherits GenericService(Of RegistroActividad)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(uow As IUnitOfWork)
        MyBase.New(uow)
    End Sub

    ''' <summary>
    ''' Guarda inmediatamente un registro (commit).
    ''' </summary>
    Public Async Function RegistrarActividadAsync(accion As String, tipoEntidad As String, descripcion As String, Optional entidadId As Integer? = Nothing) As Task
        Dim usuarioActual = My.User.Name
        Dim reg As New RegistroActividad With {
            .FechaHora = DateTime.Now,
            .Usuario = usuarioActual,
            .Accion = accion,
            .TipoEntidad = tipoEntidad,
            .EntidadId = entidadId,
            .Descripcion = descripcion
        }
        _unitOfWork.Repository(Of RegistroActividad)().Add(reg)
        Await _unitOfWork.CommitAsync()
    End Function

    ''' <summary>
    ''' Encola el registro en el mismo DbContext SIN commit (para transaccionar junto con otros cambios).
    ''' </summary>
    Public Function EncolarActividad(accion As String, tipoEntidad As String, descripcion As String, Optional entidadId As Integer? = Nothing) As RegistroActividad
        Dim usuarioActual = My.User.Name
        Dim reg As New RegistroActividad With {
            .FechaHora = DateTime.Now,
            .Usuario = usuarioActual,
            .Accion = accion,
            .TipoEntidad = tipoEntidad,
            .EntidadId = entidadId,
            .Descripcion = descripcion
        }
        _unitOfWork.Repository(Of RegistroActividad)().Add(reg)
        Return reg
    End Function

    ' --- Helper opcional para llamadas estáticas existentes ---
    Public Shared Function EncolarActividad(uow As IUnitOfWork, accion As String, tipoEntidad As String, descripcion As String, Optional entidadId As Integer? = Nothing) As RegistroActividad
        Dim svc As New AuditoriaService(uow)
        Return svc.EncolarActividad(accion, tipoEntidad, descripcion, entidadId)
    End Function
End Class
