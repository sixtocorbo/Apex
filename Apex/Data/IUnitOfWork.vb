Imports System.Data.Entity
Imports System.Threading.Tasks

''' <summary>
''' Unidad de trabajo: expone un método genérico para obtener repositorios
''' y coordina el commit de todos los cambios.
''' </summary>
Public Interface IUnitOfWork
    ''' <summary>
    ''' Devuelve (y cachea) un IRepository(Of T) para la entidad solicitada.
    ''' </summary>
    Function Repository(Of T As Class)() As IRepository(Of T)

    Function Commit() As Integer
    Function CommitAsync() As Task(Of Integer)
    ReadOnly Property Context As DbContext
    Sub Dispose()
End Interface
