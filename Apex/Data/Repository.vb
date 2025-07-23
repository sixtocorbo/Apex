﻿Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure
Imports System.Linq.Expressions

Public Class Repository(Of T As Class)
    Implements IRepository(Of T)

    Private ReadOnly _context As DbContext
    Private ReadOnly _dbSet As DbSet(Of T)

    Public Sub New(context As DbContext)
        _context = context
        _dbSet = context.Set(Of T)()
    End Sub

    Public Function GetAll() As IQueryable(Of T) Implements IRepository(Of T).GetAll
        Return _dbSet.AsNoTracking()
    End Function

    Public Async Function GetAllAsync() As Task(Of IQueryable(Of T)) Implements IRepository(Of T).GetAllAsync
        Return Await Task.FromResult(_dbSet.AsNoTracking())
    End Function

    Public Function GetById(id As Integer) As T Implements IRepository(Of T).GetById
        Return _dbSet.Find(id)
    End Function

    Public Async Function GetByIdAsync(id As Integer) As Task(Of T) Implements IRepository(Of T).GetByIdAsync
        Return Await _dbSet.FindAsync(id)
    End Function

    Public Function GetByPredicate(predicate As Expression(Of Func(Of T, Boolean))) As T Implements IRepository(Of T).GetByPredicate
        Return _dbSet.AsNoTracking().FirstOrDefault(predicate)
    End Function

    Public Async Function GetByPredicateAsync(predicate As Expression(Of Func(Of T, Boolean))) As Task(Of T) Implements IRepository(Of T).GetByPredicateAsync
        Return Await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate)
    End Function

    Public Function GetAllByPredicate(predicate As Expression(Of Func(Of T, Boolean))) As IEnumerable(Of T) Implements IRepository(Of T).GetAllByPredicate
        Return _dbSet.AsNoTracking().Where(predicate).ToList()
    End Function

    Public Async Function GetAllByPredicateAsync(predicate As Expression(Of Func(Of T, Boolean))) As Task(Of IEnumerable(Of T)) Implements IRepository(Of T).GetAllByPredicateAsync
        Return Await _dbSet.AsNoTracking().Where(predicate).ToListAsync()
    End Function

    Public Sub Add(entity As T) Implements IRepository(Of T).Add
        _dbSet.Add(entity)
    End Sub

    Public Sub AddRange(entities As IEnumerable(Of T)) Implements IRepository(Of T).AddRange
        _dbSet.AddRange(entities)
    End Sub

    Public Sub Remove(entity As T) Implements IRepository(Of T).Remove
        _dbSet.Remove(entity)
    End Sub

    Public Sub RemoveRange(entities As IEnumerable(Of T)) Implements IRepository(Of T).RemoveRange
        _dbSet.RemoveRange(entities)
    End Sub

    Public Sub RemoveById(id As Integer) Implements IRepository(Of T).RemoveById
        Dim entity = _dbSet.Find(id)
        If entity IsNot Nothing Then
            _dbSet.Remove(entity)
        End If
    End Sub

    Public Sub RemoveByPredicate(predicate As Expression(Of Func(Of T, Boolean))) Implements IRepository(Of T).RemoveByPredicate
        Dim entities = _dbSet.Where(predicate).ToList()
        _dbSet.RemoveRange(entities)
    End Sub

    Public Sub Clear() Implements IRepository(Of T).Clear
        _dbSet.RemoveRange(_dbSet)
    End Sub

    Public Sub SaveChanges() Implements IRepository(Of T).SaveChanges
        _context.SaveChanges()
    End Sub

    Public Async Function SaveChangesAsync() As Task Implements IRepository(Of T).SaveChangesAsync
        Await _context.SaveChangesAsync()
    End Function

    Public Sub Update(entity As T) Implements IRepository(Of T).Update
        _dbSet.Attach(entity)
        _context.Entry(entity).State = EntityState.Modified
    End Sub

    Public Sub UpdateRange(entities As IEnumerable(Of T)) Implements IRepository(Of T).UpdateRange
        For Each _entity In entities
            _dbSet.Attach(_entity)
            _context.Entry(_entity).State = EntityState.Modified
        Next
    End Sub

    Public Function Find(predicate As Expression(Of Func(Of T, Boolean))) As IEnumerable(Of T) Implements IRepository(Of T).Find
        Return _dbSet.AsNoTracking().Where(predicate).ToList()
    End Function

    Public Async Function FindAsync(predicate As Expression(Of Func(Of T, Boolean))) As Task(Of IEnumerable(Of T)) Implements IRepository(Of T).FindAsync
        Return Await _dbSet.AsNoTracking().Where(predicate).ToListAsync()
    End Function

    Public Function Count() As Integer Implements IRepository(Of T).Count
        Return _dbSet.Count()
    End Function

    Public Async Function CountAsync() As Task(Of Integer) Implements IRepository(Of T).CountAsync
        Return Await _dbSet.CountAsync()
    End Function

    Public Function Any() As Boolean Implements IRepository(Of T).Any
        Return _dbSet.Any()
    End Function

    Public Async Function AnyAsync() As Task(Of Boolean) Implements IRepository(Of T).AnyAsync
        Return Await _dbSet.AnyAsync()
    End Function

    Public Function Any(predicate As Expression(Of Func(Of T, Boolean))) As Boolean Implements IRepository(Of T).Any
        Return _dbSet.Any(predicate)
    End Function

    Public Async Function AnyAsync(predicate As Expression(Of Func(Of T, Boolean))) As Task(Of Boolean) Implements IRepository(Of T).AnyAsync
        Return Await _dbSet.AnyAsync(predicate)
    End Function

End Class

'Imports System.Data.Entity
'Imports System.Data.Entity.Infrastructure
'Imports System.Linq.Expressions

'Public Class Repository(Of T As Class)
'    Implements IRepository(Of T)

'    Private ReadOnly _context As DbContext
'    Private ReadOnly _dbSet As DbSet(Of T)

'    Public Sub New(context As DbContext)
'        _context = context
'        _dbSet = context.Set(Of T)()
'    End Sub

'    Public Function GetAll() As IQueryable(Of T) Implements IRepository(Of T).GetAll
'        Return _dbSet.AsNoTracking()
'    End Function

'    Public Async Function GetAllAsync() As Task(Of IQueryable(Of T)) Implements IRepository(Of T).GetAllAsync
'        Return Await Task.FromResult(_dbSet.AsNoTracking())
'    End Function

'    Public Function GetById(id As Integer) As T Implements IRepository(Of T).GetById
'        Return _dbSet.Find(id)
'    End Function

'    Public Async Function GetByIdAsync(id As Integer) As Task(Of T) Implements IRepository(Of T).GetByIdAsync
'        Return Await _dbSet.FindAsync(id)
'    End Function

'    Public Function GetByPredicate(predicate As Func(Of T, Boolean)) As T Implements IRepository(Of T).GetByPredicate
'        Return _dbSet.AsNoTracking().FirstOrDefault(predicate)
'    End Function

'    Public Async Function GetByPredicateAsync(predicate As Func(Of T, Boolean)) As Task(Of T) Implements IRepository(Of T).GetByPredicateAsync
'        Return Await Task.Run(Function() _dbSet.AsNoTracking().FirstOrDefault(predicate))
'    End Function

'    Public Function GetAllByPredicate(predicate As Func(Of T, Boolean)) As IEnumerable(Of T) Implements IRepository(Of T).GetAllByPredicate
'        Return _dbSet.AsNoTracking().Where(predicate).ToList()
'    End Function

'    'Public Async Function GetAllByPredicateAsync(predicate As Func(Of T, Boolean)) As Task(Of IEnumerable(Of T)) Implements IRepository(Of T).GetAllByPredicateAsync
'    '    Return Await Task.Run(Function() _dbSet.AsNoTracking().Where(predicate).ToList())
'    'End Function
'    Public Async Function GetAllByPredicateAsync(predicate As Func(Of T, Boolean)) As Task(Of IEnumerable(Of T)) Implements IRepository(Of T).GetAllByPredicateAsync
'        ' Versión más eficiente que no bloquea un hilo
'        Return Await _dbSet.AsNoTracking().Where(predicate).ToListAsync()
'    End Function

'    Public Sub Add(entity As T) Implements IRepository(Of T).Add
'        _dbSet.Add(entity)
'    End Sub

'    Public Sub AddRange(entities As IEnumerable(Of T)) Implements IRepository(Of T).AddRange
'        _dbSet.AddRange(entities)
'    End Sub

'    Public Sub Remove(entity As T) Implements IRepository(Of T).Remove
'        _dbSet.Remove(entity)
'    End Sub

'    Public Sub RemoveRange(entities As IEnumerable(Of T)) Implements IRepository(Of T).RemoveRange
'        _dbSet.RemoveRange(entities)
'    End Sub

'    Public Sub RemoveById(id As Integer) Implements IRepository(Of T).RemoveById
'        Dim entity = _dbSet.Find(id)
'        If entity IsNot Nothing Then
'            _dbSet.Remove(entity)
'        End If
'    End Sub

'    Public Sub RemoveByPredicate(predicate As Func(Of T, Boolean)) Implements IRepository(Of T).RemoveByPredicate
'        Dim entities = _dbSet.Where(predicate).ToList()
'        _dbSet.RemoveRange(entities)
'    End Sub

'    Public Sub Clear() Implements IRepository(Of T).Clear
'        _dbSet.RemoveRange(_dbSet)
'    End Sub

'    Public Sub SaveChanges() Implements IRepository(Of T).SaveChanges
'        _context.SaveChanges()
'    End Sub

'    Public Async Function SaveChangesAsync() As Task Implements IRepository(Of T).SaveChangesAsync
'        Await _context.SaveChangesAsync()
'    End Function

'    Public Sub Update(entity As T) Implements IRepository(Of T).Update
'        _dbSet.Attach(entity)
'        _context.Entry(entity).State = EntityState.Modified
'    End Sub

'    Public Sub UpdateRange(entities As IEnumerable(Of T)) Implements IRepository(Of T).UpdateRange
'        For Each _entity In entities
'            _dbSet.Attach(_entity)
'            _context.Entry(_entity).State = EntityState.Modified
'        Next
'    End Sub



'    Public Function Find(predicate As Func(Of T, Boolean)) As IEnumerable(Of T) Implements IRepository(Of T).Find
'        Return _dbSet.AsNoTracking().Where(predicate).ToList()
'    End Function

'    Public Async Function FindAsync(predicate As Func(Of T, Boolean)) As Task(Of IEnumerable(Of T)) Implements IRepository(Of T).FindAsync
'        Return Await Task.Run(Function() _dbSet.AsNoTracking().Where(predicate).ToList())
'    End Function

'    Public Function Count() As Integer Implements IRepository(Of T).Count
'        Return _dbSet.Count()
'    End Function

'    Public Async Function CountAsync() As Task(Of Integer) Implements IRepository(Of T).CountAsync
'        Return Await _dbSet.CountAsync()
'    End Function

'    Public Function Any() As Boolean Implements IRepository(Of T).Any
'        Return _dbSet.Any()
'    End Function

'    Public Async Function AnyAsync() As Task(Of Boolean) Implements IRepository(Of T).AnyAsync
'        Return Await _dbSet.AnyAsync()
'    End Function

'    Public Function Any(predicate As Func(Of T, Boolean)) As Boolean Implements IRepository(Of T).Any
'        Return _dbSet.Any(predicate)
'    End Function

'    Public Async Function AnyAsync(predicate As Func(Of T, Boolean)) As Task(Of Boolean) Implements IRepository(Of T).AnyAsync
'        Return Await Task.Run(Function() _dbSet.Any(predicate))
'    End Function

'End Class
