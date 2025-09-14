' Apex/Services/SeccionService.vb
Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class SeccionService
    Inherits GenericService(Of Seccion)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' Helper de combo (ordenado y AsNoTracking)
    Public Async Function ObtenerSeccionesParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Seccion)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(s) s.Nombre).
            Select(Function(s) New With {.Id = s.Id, .Nombre = s.Nombre}).
            ToListAsync()

        Return lista.Select(Function(x) New KeyValuePair(Of Integer, String)(x.Id, x.Nombre)).ToList()
    End Function
End Class
