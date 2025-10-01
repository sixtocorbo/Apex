Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class SubDireccionService
    Inherits GenericService(Of SubDireccion)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    Public Async Function ObtenerSubDireccionesParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of SubDireccion)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(sd) sd.Nombre).
            Select(Function(sd) New With {.Id = sd.Id, .Nombre = sd.Nombre}).
            ToListAsync()

        Return lista.Select(Function(x) New KeyValuePair(Of Integer, String)(x.Id, x.Nombre)).ToList()
    End Function
End Class
