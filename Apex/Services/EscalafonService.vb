Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class EscalafonService
    Inherits GenericService(Of Escalafon)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    Public Async Function ObtenerEscalafonesParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Escalafon)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(e) e.Nombre).
            Select(Function(e) New With {.Id = e.Id, .Nombre = e.Nombre}).
            ToListAsync()

        Return lista.Select(Function(x) New KeyValuePair(Of Integer, String)(x.Id, x.Nombre)).ToList()
    End Function
End Class
