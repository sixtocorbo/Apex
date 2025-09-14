' Apex/Services/CategoriaAusenciaService.vb
Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class CategoriaAusenciaService
    Inherits GenericService(Of CategoriaAusencia)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' (Opcional) Exponer el UoW si te resulta útil en la UI / reportes.
    Public ReadOnly Property UnitOfWork As IUnitOfWork
        Get
            Return _unitOfWork
        End Get
    End Property

    ' (Opcional) Helper para combos (Id/Nombre) ordenado alfabéticamente.
    Public Async Function ObtenerCategoriasParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of CategoriaAusencia)().
            GetAll().AsNoTracking().
            OrderBy(Function(c) c.Nombre).
            Select(Function(c) New With {.Id = c.Id, .Nombre = c.Nombre}).
            ToListAsync()

        Return lista.Select(Function(c) New KeyValuePair(Of Integer, String)(c.Id, c.Nombre)).ToList()
    End Function
End Class
