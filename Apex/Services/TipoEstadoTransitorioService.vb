' Apex/Services/TipoEstadoTransitorioService.vb
Option Strict On
Option Explicit On

Imports System.Data.Entity

Public Class TipoEstadoTransitorioService
    Inherits GenericService(Of TipoEstadoTransitorio)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' Lista simple ordenada (solo lectura)
    Public Async Function ObtenerTodosOrdenadosAsync() As Task(Of List(Of TipoEstadoTransitorio))
        Return Await _unitOfWork.Repository(Of TipoEstadoTransitorio)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(t) t.Nombre).
            ToListAsync()
    End Function

    ' Para combos: Id / Nombre
    Public Async Function ObtenerParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of TipoEstadoTransitorio)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(t) t.Nombre).
            Select(Function(t) New With {.Id = t.Id, .Nombre = t.Nombre}).
            ToListAsync()

        Return lista.Select(Function(x) New KeyValuePair(Of Integer, String)(x.Id, x.Nombre)).ToList()
    End Function

    ' Búsqueda por texto (Nombre LIKE)
    Public Async Function BuscarPorNombreAsync(texto As String) As Task(Of List(Of TipoEstadoTransitorio))
        Dim q = _unitOfWork.Repository(Of TipoEstadoTransitorio)().GetAll().AsNoTracking()

        If Not String.IsNullOrWhiteSpace(texto) Then
            Dim t = texto.Trim()
            q = q.Where(Function(x) x.Nombre.Contains(t))
        End If

        Return Await q.OrderBy(Function(x) x.Nombre).ToListAsync()
    End Function
End Class
