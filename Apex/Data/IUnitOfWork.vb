' IUnitOfWork.vb
Imports System.Data.Entity

Public Interface IUnitOfWork
    Inherits IDisposable   ' ← clave

    ReadOnly Property Context As DbContext
    Function Repository(Of T As Class)() As IRepository(Of T)
    Function Commit() As Integer
    Function CommitAsync() As Task(Of Integer)
End Interface
