' Apex/Services/AreaTrabajoService.vb
Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class AreaTrabajoService
    Inherits GenericService(Of AreaTrabajo)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' (Opcional) Exponer el UoW si la UI o reportes lo necesitan.
    Public ReadOnly Property UnitOfWork As IUnitOfWork
        Get
            Return _unitOfWork
        End Get
    End Property

    ' (Opcional) Helper para combos: Id / Nombre (ordenado)
    Public Async Function ObtenerAreasTrabajoParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim items = Await _unitOfWork.Repository(Of AreaTrabajo)().
            GetAll().AsNoTracking().
            OrderBy(Function(a) a.Nombre).
            Select(Function(a) New With {.Id = a.Id, .Nombre = a.Nombre}).
            ToListAsync()

        Return items.Select(Function(x) New KeyValuePair(Of Integer, String)(x.Id, x.Nombre)).ToList()
    End Function
End Class
