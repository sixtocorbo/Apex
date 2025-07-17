Public Interface IRepository(Of T As Class)
    ' Define the basic CRUD operations for a repository
    Function GetAll() As IQueryable(Of T)
    Function GetAllAsync() As Task(Of IQueryable(Of T))
    Function GetByIdAsync(id As Integer) As Task(Of T)
    Function GetById(id As Integer) As T
    Function GetByPredicate(predicate As Func(Of T, Boolean)) As T
    Function GetByPredicateAsync(predicate As Func(Of T, Boolean)) As Task(Of T)
    Function GetAllByPredicate(predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
    Function GetAllByPredicateAsync(predicate As Func(Of T, Boolean)) As Task(Of IEnumerable(Of T))

    Sub Add(entity As T)
    Sub AddRange(entities As IEnumerable(Of T))
    Sub RemoveRange(entities As IEnumerable(Of T))
    Sub RemoveById(id As Integer)
    Sub RemoveByPredicate(predicate As Func(Of T, Boolean))
    ' Define additional methods for the repository
    Sub Remove(entity As T)
    Sub Clear()

    Sub SaveChanges()
    Function SaveChangesAsync() As Task

    Sub Update(entity As T)
    Sub UpdateRange(entities As IEnumerable(Of T))

    Function Find(predicate As Func(Of T, Boolean)) As IEnumerable(Of T)
    Function FindAsync(predicate As Func(Of T, Boolean)) As Task(Of IEnumerable(Of T))

    Function Count() As Integer
    Function CountAsync() As Task(Of Integer)
    Function Any() As Boolean
    Function AnyAsync() As Task(Of Boolean)

    Function Any(predicate As Func(Of T, Boolean)) As Boolean
    Function AnyAsync(predicate As Func(Of T, Boolean)) As Task(Of Boolean)


End Interface