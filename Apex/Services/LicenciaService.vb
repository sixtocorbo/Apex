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
    ''' Obtiene una lista de licencias para la grilla principal, con filtros.
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(
        Optional filtroNombre As String = "",
        Optional filtroTipoId As Integer? = Nothing,
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing
    ) As Task(Of List(Of LicenciaParaVista))

        Dim query = _unitOfWork.Repository(Of HistoricoLicencia)().
            GetAll().
            Include(Function(l) l.Funcionario).
            Include(Function(l) l.TipoLicencia).
            AsNoTracking()

        ' Aplicar filtros
        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
            query = query.Where(Function(l) l.Funcionario.Nombre.Contains(filtroNombre) Or l.Funcionario.CI.Contains(filtroNombre))
        End If

        If filtroTipoId.HasValue AndAlso filtroTipoId.Value > 0 Then
            query = query.Where(Function(l) l.TipoLicenciaId = filtroTipoId.Value)
        End If

        If fechaDesde.HasValue Then
            query = query.Where(Function(l) l.inicio >= fechaDesde.Value)
        End If

        If fechaHasta.HasValue Then
            query = query.Where(Function(l) l.finaliza <= fechaHasta.Value)
        End If

        Return Await query.Select(Function(l) New LicenciaParaVista With {
            .Id = l.Id,
            .NombreFuncionario = l.Funcionario.Nombre,
            .CI = l.Funcionario.CI,
            .TipoLicencia = l.TipoLicencia.Nombre,
            .FechaInicio = l.inicio,
            .FechaFin = l.finaliza,
            .Estado = l.estado,
            .Comentario = l.Comentario
        }).OrderByDescending(Function(l) l.FechaInicio).ToListAsync()
    End Function

    ' --- MÉTODOS PARA POBLAR COMBOS ---
    Public Async Function ObtenerTiposLicenciaParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim repo = _unitOfWork.Repository(Of TipoLicencia)()
        Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(t) t.Nombre).ToListAsync()
        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
    End Function

    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim repo = _unitOfWork.Repository(Of Funcionario)()
        Dim lista = Await repo.GetAll().AsNoTracking().Where(Function(f) f.Activo).OrderBy(Function(f) f.Nombre).ToListAsync()
        Return lista.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
    End Function
    ''' <summary>
    ''' Obtiene los valores distintos de una columna específica para llenar los combos de filtros.
    ''' </summary>
    Public Async Function ObtenerValoresDistintosAsync(propiedad As String) As Task(Of List(Of String))
        Dim query = _unitOfWork.Repository(Of HistoricoLicencia)().GetAll()

        ' Mapeo de propiedades a las entidades correspondientes
        Select Case propiedad
            Case "Funcionario.Nombre"
                Return Await query.Select(Function(l) l.Funcionario.Nombre).Distinct().OrderBy(Function(x) x).ToListAsync()
            Case "Funcionario.CI"
                Return Await query.Select(Function(l) l.Funcionario.CI).Distinct().OrderBy(Function(x) x).ToListAsync()
            Case "TipoLicencia.Nombre"
                Return Await query.Select(Function(l) l.TipoLicencia.Nombre).Distinct().OrderBy(Function(x) x).ToListAsync()
            Case "estado"
                Return Await query.Select(Function(l) l.estado).Distinct().OrderBy(Function(x) x).ToListAsync()
            Case Else
                Return New List(Of String)()
        End Select
    End Function
End Class