' Apex/Services/CargoService.vb
Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class CargoService
    Inherits GenericService(Of Cargo)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' (Opcional) Acceso al UoW por si la UI o reportes lo necesitan.
    Public ReadOnly Property UnitOfWork As IUnitOfWork
        Get
            Return _unitOfWork
        End Get
    End Property

    ' (Opcional) Helper para poblar combos: Id / “Grado - Nombre”.
    Public Async Function ObtenerCargosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim items = Await _unitOfWork.Repository(Of Cargo)().
            GetAll().AsNoTracking().
            OrderBy(Function(c) c.Nombre).
            Select(Function(c) New With {
                .Id = c.Id,
                .Texto = If(c.Grado.HasValue AndAlso c.Grado.Value > 0,
                            $"G{c.Grado.Value} - {c.Nombre}",
                            c.Nombre)
            }).
            ToListAsync()

        Return items.Select(Function(x) New KeyValuePair(Of Integer, String)(x.Id, x.Texto)).ToList()
    End Function
End Class
