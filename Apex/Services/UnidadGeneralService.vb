Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Linq

Public Class UnidadGeneralService
    Inherits GenericService(Of UnidadGeneral)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    Public Async Function ObtenerUnidadesOrdenadasAsync() As Task(Of List(Of UnidadGeneral))
        Dim lista = Await _unitOfWork.Repository(Of UnidadGeneral)().
            GetAll().
            OrderBy(Function(u) u.Nombre).
            ToListAsync()

        Return lista
    End Function
End Class
