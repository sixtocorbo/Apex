' Apex/Services/FuncionarioService.vb
Imports System.Data.Entity

Public Class FuncionarioService
    Inherits GenericService(Of Funcionario)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    Public Async Function GetByIdCompletoAsync(id As Integer) As Task(Of Funcionario)
        Return Await _unitOfWork.Repository(Of Funcionario)().
        GetAll().
        Include(Function(f) f.Cargo).
        Include(Function(f) f.Escalafon).
        Include(Function(f) f.Funcion).
        Include(Function(f) f.TipoFuncionario).
        Include(Function(f) f.EstadoCivil).
        Include(Function(f) f.Genero).
        Include(Function(f) f.NivelEstudio).
        Include(Function(f) f.FuncionarioDotacion.Select(Function(d) d.DotacionItem)).
        Include(Function(f) f.FuncionarioEstadoLegal).
        Include(Function(f) f.FuncionarioDispositivo).
        Include(Function(f) f.EstadoTransitorio.Select(Function(et) et.TipoEstadoTransitorio)).
        FirstOrDefaultAsync(Function(f) f.Id = id)
    End Function

    ' --- ¡NUEVO MÉTODO AÑADIDO! ---
    ''' <summary>
    ''' Obtiene una lista simplificada de funcionarios para vistas y reportes.
    ''' </summary>
    Public Async Function GetFuncionariosParaVistaAsync() As Task(Of List(Of Object))
        Using uow As New UnitOfWork()
            Dim query = uow.Repository(Of Funcionario)().GetAll().
                Select(Function(f) New With {
                    f.Nombre,
                    f.CI,
                    .Cargo = If(f.Cargo IsNot Nothing, f.Cargo.Nombre, "-"),
                    .TipoFuncionario = If(f.TipoFuncionario IsNot Nothing, f.TipoFuncionario.Nombre, "-"),
                    f.FechaIngreso,
                    f.Activo,
                    .Correo = f.Email
                })
            Dim result = Await query.ToListAsync()
            Return result.Cast(Of Object).ToList()
        End Using
    End Function


    ' --- Métodos para poblar los ComboBox (Catálogos) ---
    Public Async Function ObtenerCargosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Cargo)().GetAll().AsNoTracking().OrderBy(Function(c) c.Nombre).ToListAsync()
        Return lista.Select(Function(c) New KeyValuePair(Of Integer, String)(c.Id, c.Nombre)).ToList()
    End Function

    Public Async Function ObtenerTiposFuncionarioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of TipoFuncionario)().GetAll().AsNoTracking().OrderBy(Function(t) t.Nombre).ToListAsync()
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
        Dim lista = Await _unitOfWork.Repository(Of EstadoCivil)().GetAll().AsNoTracking().OrderBy(Function(ec) ec.Nombre).ToListAsync()
        Return lista.Select(Function(ec) New KeyValuePair(Of Integer, String)(ec.Id, ec.Nombre)).ToList()
    End Function

    Public Async Function ObtenerGenerosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Genero)().GetAll().AsNoTracking().OrderBy(Function(g) g.Nombre).ToListAsync()
        Return lista.Select(Function(g) New KeyValuePair(Of Integer, String)(g.Id, g.Nombre)).ToList()
    End Function

    Public Async Function ObtenerNivelesEstudioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of NivelEstudio)().GetAll().AsNoTracking().OrderBy(Function(ne) ne.Nombre).ToListAsync()
        Return lista.Select(Function(ne) New KeyValuePair(Of Integer, String)(ne.Id, ne.Nombre)).ToList()
    End Function

    Public Async Function ObtenerItemsDotacionCompletosAsync() As Task(Of List(Of DotacionItem))
        Return Await _unitOfWork.Repository(Of DotacionItem)().
        GetAll().
            AsNoTracking().
            OrderBy(Function(di) di.Nombre).
            ToListAsync()
    End Function

    Public Async Function ObtenerTiposEstadoTransitorioCompletosAsync() As Task(Of List(Of TipoEstadoTransitorio))
        Return Await _unitOfWork.Repository(Of TipoEstadoTransitorio)().
        GetAll().
            AsNoTracking().
            OrderBy(Function(t) t.Nombre).
            ToListAsync()
    End Function
    Public Async Function BuscarConFiltrosDinamicosAsync(
       textoLibre As String,
       filtros As Dictionary(Of String, String)
   ) As Task(Of List(Of FuncionarioVistaDTO))

        Dim query = _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking()

        ' 1. Filtro de texto libre (busca en Nombre y CI)
        If Not String.IsNullOrWhiteSpace(textoLibre) Then
            Dim palabrasLibres = textoLibre.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
            For Each palabra In palabrasLibres
                query = query.Where(Function(f) f.Nombre.Contains(palabra) Or f.CI.Contains(palabra))
            Next
        End If

        ' 2. Aplicar filtros estructurados
        For Each filtro In filtros
            Select Case filtro.Key.ToLower()
                Case "cargo"
                    query = query.Where(Function(f) f.Cargo IsNot Nothing AndAlso f.Cargo.Nombre.Contains(filtro.Value))
                Case "ci"
                    query = query.Where(Function(f) f.CI.Contains(filtro.Value))
                Case "activo"
                    Dim esActivo As Boolean = (filtro.Value.ToLower() = "si" OrElse filtro.Value.ToLower() = "true")
                    query = query.Where(Function(f) f.Activo = esActivo)
                    ' ... Agrega más casos para otras columnas: "seccion", "tipo", etc.
            End Select
        Next

        ' 3. Proyectar al DTO para la vista y limitar resultados
        Return Await query.Select(Function(f) New FuncionarioVistaDTO With {
            .Id = f.Id,
            .CI = f.CI,
            .Nombre = f.Nombre,
            .CargoNombre = If(f.Cargo IsNot Nothing, f.Cargo.Nombre, "N/A"),
             .Activo = f.Activo
        }).OrderBy(Function(f) f.Nombre).Take(500).ToListAsync()

    End Function
    ' -- Métodos para Estados Transitorios --
    Public Async Function ObtenerTiposEstadoTransitorioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await ObtenerTiposEstadoTransitorioCompletosAsync()
        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
    End Function

End Class
Public Class FuncionarioVistaDTO
    Public Property Id As Integer
    Public Property CI As String
    Public Property Nombre As String
    Public Property CargoNombre As String
    Public Property Activo As Boolean
End Class