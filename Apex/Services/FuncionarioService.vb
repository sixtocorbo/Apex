Imports System.Data.Entity

Public Class FuncionarioService
    Inherits GenericService(Of Funcionario)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    Public Async Function ObtenerCargosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Cargo)().
        GetAll().
        AsNoTracking().
        OrderBy(Function(c) c.Nombre).
        ToListAsync() ' 👈 materializamos la consulta

        Return lista.Select(Function(c) New KeyValuePair(Of Integer, String)(c.Id, c.Nombre)).ToList()
    End Function


    Public Async Function ObtenerTiposFuncionarioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of TipoFuncionario)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(t) t.Nombre).
            ToListAsync() ' 👈 Materializás en memoria

        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
    End Function

    ''' <summary>
    ''' Devuelve una consulta base de funcionarios con Include y AsNoTracking.
    ''' </summary>
    Public Function ObtenerQueryBase() As IQueryable(Of Funcionario)
        Return _unitOfWork.
            Repository(Of Funcionario)().
            GetAll().
            Include(Function(f) f.Cargo).
            Include(Function(f) f.TipoFuncionario).
            AsNoTracking()
    End Function
    Public Async Function ObtenerListaFiltradaAsync(legajo As Integer?, ci As String) As Task(Of List(Of Funcionario))

        Using uow As New UnitOfWork()
            Dim q = uow.Repository(Of Funcionario)() _
                   .GetAll() _
                   .Include(Function(f) f.Cargo) _
                   .Include(Function(f) f.TipoFuncionario) _
                   .AsNoTracking()

            If Not String.IsNullOrEmpty(ci) Then q = q.Where(Function(f) f.CI.Contains(ci))

            Return Await q.OrderBy(Function(f) f.Nombre).ToListAsync()
        End Using
    End Function

End Class
