' Apex/Services/TurnoService.vb
Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class TurnoService
    Inherits GenericService(Of Turno)

    ' Mantengo una referencia explícita al UoW por coherencia con tus otros servicios
    Private Shadows ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        MyBase.New(New UnitOfWork())
        _unitOfWork = MyBase._unitOfWork
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
        _unitOfWork = unitOfWork
    End Sub

    Public ReadOnly Property UnitOfWork As IUnitOfWork
        Get
            Return _unitOfWork
        End Get
    End Property

    ' Lista completa ordenada por Nombre (solo lectura)
    Public Async Function GetAllOrdenadosAsync() As Task(Of List(Of Turno))
        Return Await _unitOfWork.Repository(Of Turno)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(t) t.Nombre).
            ToListAsync()
    End Function

    ' Datos para ComboBox (Id/Nombre)
    Public Async Function ObtenerTurnosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Turno)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(t) t.Nombre).
            Select(Function(t) New With {.Id = t.Id, .Nombre = t.Nombre}).
            ToListAsync()

        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
    End Function
End Class
