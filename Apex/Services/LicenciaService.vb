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

    Public ReadOnly Property UnitOfWork As IUnitOfWork
        Get
            Return _unitOfWork
        End Get
    End Property

    ''' <summary>
    ''' Obtiene TODAS las licencias desde la vista vw_LicenciasCompletas, aplicando filtros.
    ''' (VERSIÓN CORREGIDA CON FILTRO DE NOMBRE)
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(
        Optional filtroNombre As String = "",
        Optional filtroTipoId As Integer? = Nothing,
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing
    ) As Task(Of List(Of vw_LicenciasCompletas))

        Dim query = _unitOfWork.Repository(Of vw_LicenciasCompletas)().GetAll().AsNoTracking()

        ' --- INICIO DE LA MODIFICACIÓN ---
        ' Aplicar filtro por nombre de funcionario o CI
        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
            query = query.Where(Function(l) l.NombreFuncionario.Contains(filtroNombre) Or l.CI.Contains(filtroNombre))
        End If
        ' --- FIN DE LA MODIFICACIÓN ---

        If filtroTipoId.HasValue AndAlso filtroTipoId.Value > 0 Then
            query = query.Where(Function(l) l.TipoLicenciaId = filtroTipoId.Value)
        End If

        If fechaDesde.HasValue Then
            query = query.Where(Function(l) l.FechaFin >= fechaDesde.Value)
        End If

        If fechaHasta.HasValue Then
            query = query.Where(Function(l) l.FechaInicio <= fechaHasta.Value)
        End If

        Dim licencias = Await query.OrderByDescending(Function(l) l.FechaInicio).ToListAsync()

        Return licencias
    End Function

    ' --- MÉTODOS PARA POBLAR COMBOS (se mantienen igual) ---
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