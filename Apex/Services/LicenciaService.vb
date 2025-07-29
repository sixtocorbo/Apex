' Apex/Services/LicenciaService.vb

Imports System.Data.Entity

Public Class LicenciaService
    Inherits GenericService(Of HistoricoLicencia)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' Clase DTO para la vista de la grilla
    Public Class LicenciaParaVista
        Public Property Id As Integer
        Public Property NombreFuncionario As String
        Public Property CI As String
        Public Property TipoLicencia As String
        Public Property FechaInicio As Date
        Public Property FechaFin As Date
        Public Property Estado As String
        Public Property Comentario As String
    End Class

    ''' <summary>
    ''' DTO para devolver resultados paginados.
    ''' </summary>
    Public Class ResultadoPaginadoLicencias
        Public Property Licencias As List(Of LicenciaParaVista)
        Public Property TotalRegistros As Integer
    End Class

    ''' <summary>
    ''' Obtiene una lista paginada de licencias para la grilla principal, con filtros.
    ''' </summary>
    Public Async Function GetAllPaginadoConDetallesAsync(
        paginaActual As Integer,
        tamañoPagina As Integer,
        Optional filtroNombre As String = "",
        Optional filtroTipoId As Integer? = Nothing,
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing
    ) As Task(Of ResultadoPaginadoLicencias)

        Dim query = _unitOfWork.Repository(Of HistoricoLicencia)().GetAll().AsNoTracking()

        ' --- Aplicar filtros ---
        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
            query = query.Where(Function(l) l.Funcionario.Nombre.Contains(filtroNombre) Or l.Funcionario.CI.Contains(filtroNombre))
        End If

        If filtroTipoId.HasValue AndAlso filtroTipoId.Value > 0 Then
            query = query.Where(Function(l) l.TipoLicenciaId = filtroTipoId.Value)
        End If

        ' **LÓGICA DE FECHA CORREGIDA** para encontrar licencias que se solapan con el rango.
        If fechaDesde.HasValue Then
            query = query.Where(Function(l) l.finaliza >= fechaDesde.Value)
        End If

        If fechaHasta.HasValue Then
            query = query.Where(Function(l) l.inicio <= fechaHasta.Value)
        End If

        ' --- Contar el total de registros ANTES de paginar ---
        Dim totalRegistros = Await query.CountAsync()

        ' --- Aplicar orden y paginación ---
        Dim licenciasPaginadas = Await query.OrderByDescending(Function(l) l.inicio) _
            .Skip((paginaActual - 1) * tamañoPagina) _
            .Take(tamañoPagina) _
            .Select(Function(l) New LicenciaParaVista With {
                .Id = l.Id,
                .NombreFuncionario = l.Funcionario.Nombre,
                .CI = l.Funcionario.CI,
                .TipoLicencia = l.TipoLicencia.Nombre,
                .FechaInicio = l.inicio,
                .FechaFin = l.finaliza,
                .Estado = l.estado,
                .Comentario = l.Comentario
            }).ToListAsync()

        Return New ResultadoPaginadoLicencias With {
            .Licencias = licenciasPaginadas,
            .TotalRegistros = totalRegistros
        }
    End Function


    ' --- MÉTODOS PARA POBLAR COMBOS ---
    Public Async Function ObtenerTiposLicenciaParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim repo = _unitOfWork.Repository(Of TipoLicencia)()
        Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(t) t.Nombre).ToListAsync()
        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
    End Function

    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim funcionariosData = Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking() _
        .Where(Function(f) f.Activo) _
        .OrderBy(Function(f) f.Nombre) _
        .Select(Function(f) New With {
            Key .Id = f.Id,
            Key .Nombre = f.Nombre
        }) _
        .ToListAsync()
        Return funcionariosData.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
    End Function

End Class