''' <summary>
''' Operaciones CRUD básicas expuestas a la UI.
''' </summary>
Public Interface IGenericService(Of TDto As Class)
    Function GetAllAsync() As Task(Of List(Of TDto))
    Function GetByIdAsync(id As Integer) As Task(Of TDto)
    Function CreateAsync(dto As TDto) As Task(Of Integer)
    Function UpdateAsync(dto As TDto) As Task
    Function DeleteAsync(id As Integer) As Task
End Interface
