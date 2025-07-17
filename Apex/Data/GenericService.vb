Imports System.Data.Entity

Public Class GenericService(Of T As Class)
    Implements IGenericService(Of T)


    Protected ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New(unitOfWork As IUnitOfWork)
        _unitOfWork = unitOfWork
    End Sub

    Public Async Function GetAllAsync() As Task(Of List(Of T)) Implements IGenericService(Of T).GetAllAsync

        Return Await _unitOfWork.Repository(Of T)().GetAll().ToListAsync()
    End Function

    Public Async Function GetByIdAsync(id As Integer) As Task(Of T) Implements IGenericService(Of T).GetByIdAsync

        Return Await Task.FromResult(_unitOfWork.Repository(Of T)().GetById(id))
    End Function

    Public Async Function CreateAsync(entity As T) As Task(Of Integer) Implements IGenericService(Of T).CreateAsync

        _unitOfWork.Repository(Of T)().Add(entity)
        Await _unitOfWork.CommitAsync()

        Return CInt(entity.GetType().GetProperty("Id").GetValue(entity))
    End Function

    Public Async Function UpdateAsync(entity As T) As Task Implements IGenericService(Of T).UpdateAsync

        Await _unitOfWork.CommitAsync()
    End Function

    Public Async Function DeleteAsync(id As Integer) As Task Implements IGenericService(Of T).DeleteAsync

        Dim entity = _unitOfWork.Repository(Of T)().GetById(id)
        If entity IsNot Nothing Then
            _unitOfWork.Repository(Of T)().Remove(entity)
            Await _unitOfWork.CommitAsync()
        End If
    End Function
End Class
