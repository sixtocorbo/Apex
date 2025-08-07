''' <summary>
''' Operaciones CRUD básicas expuestas a la UI.
''' </summary>
Public Interface IGenericService(Of TDto As Class)
    Function GetAllAsync() As Task(Of List(Of TDto))
    Function GetByIdAsync(id As Integer) As Task(Of TDto)
    Function CreateAsync(dto As TDto) As Task(Of Integer)
    Function UpdateAsync(dto As TDto) As Task
    Function DeleteAsync(id As Integer) As Task
    ' --- MÉTODO AÑADIDO ---
    ''' <summary>
    ''' Marca una entidad para ser eliminada en el próximo Commit, sin guardarla inmediatamente.
    ''' </summary>
    Sub RemoveWithoutCommit(entity As TDto)
End Interface
