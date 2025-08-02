' Apex/Services/NovedadService.vb
Imports System.Data.Entity

Public Class NovedadService
    Inherits GenericService(Of Novedad)

    Private Shadows ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        MyBase.New(New UnitOfWork())
        _unitOfWork = MyBase._unitOfWork
    End Sub

    Public Async Function GetAllConDetallesAsync(fechaInicio As Date, fechaFin As Date) As Task(Of List(Of vw_NovedadesCompletas))
        Dim query = _unitOfWork.Repository(Of vw_NovedadesCompletas)().GetAll().AsNoTracking()

        query = query.Where(Function(n) n.Fecha >= fechaInicio And n.Fecha <= fechaFin)

        Return Await query.OrderByDescending(Function(n) n.Fecha).ToListAsync()
    End Function

End Class