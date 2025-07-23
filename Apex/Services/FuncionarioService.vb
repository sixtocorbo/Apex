Imports System.Data.Entity

Public Class FuncionarioService
    Inherits GenericService(Of Funcionario)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' --- Método nuevo para cargar un funcionario con todos sus datos relacionados ---
    Public Async Function GetByIdCompletoAsync(id As Integer) As Task(Of Funcionario)
        Return Await _unitOfWork.Repository(Of Funcionario)().
            GetAll().
            Include(Function(f) f.FuncionarioDotacion).
            Include(Function(f) f.FuncionarioObservacion).
            Include(Function(f) f.FuncionarioEstadoLegal).
            Include(Function(f) f.FuncionarioDispositivo).
            FirstOrDefaultAsync(Function(f) f.Id = id)
    End Function

    ' --- Métodos para poblar los ComboBox (Catálogos) ---

    Public Async Function ObtenerCargosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Cargo)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(c) c.Nombre).
            ToListAsync()

        Return lista.Select(Function(c) New KeyValuePair(Of Integer, String)(c.Id, c.Nombre)).ToList()
    End Function

    Public Async Function ObtenerTiposFuncionarioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of TipoFuncionario)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(t) t.Nombre).
            ToListAsync()

        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
    End Function

    Public Async Function ObtenerEscalafonesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Escalafon)().GetAll().AsNoTracking().OrderBy(Function(e) e.Nombre).ToListAsync()

        Return lista.Select(Function(e) New KeyValuePair(Of Integer, String)(e.Id, e.Nombre)).ToList()
    End Function

    Public Async Function ObtenerFuncionesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Funcion)().GetAll().AsNoTracking().OrderBy(Function(f) f.Nombre).ToListAsync()

        Return lista.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
    End Function

    Public Async Function ObtenerEstadosCivilesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of EstadoCivil)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(ec) ec.Nombre).
            ToListAsync()

        Return lista.Select(Function(ec) New KeyValuePair(Of Integer, String)(ec.Id, ec.Nombre)).ToList()
    End Function

    Public Async Function ObtenerGenerosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Genero)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(g) g.Nombre).
            ToListAsync()

        Return lista.Select(Function(g) New KeyValuePair(Of Integer, String)(g.Id, g.Nombre)).ToList()
    End Function

    Public Async Function ObtenerNivelesEstudioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of NivelEstudio)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(ne) ne.Nombre).
            ToListAsync()

        Return lista.Select(Function(ne) New KeyValuePair(Of Integer, String)(ne.Id, ne.Nombre)).ToList()
    End Function

End Class