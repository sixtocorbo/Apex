Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class FuncionService
    Inherits GenericService(Of Funcion)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    Public Async Function ObtenerFuncionesParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Funcion)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(f) f.Nombre).
            Select(Function(f) New With {.Id = f.Id, .Nombre = f.Nombre}).
            ToListAsync()

        Return lista.Select(Function(x) New KeyValuePair(Of Integer, String)(x.Id, x.Nombre)).ToList()
    End Function
End Class
