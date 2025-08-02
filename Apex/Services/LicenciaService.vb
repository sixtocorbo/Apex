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

    ' NOTA: La clase DTO 'LicenciaParaVista' ya no es necesaria,
    ' porque la vista y su entidad generada la reemplazan.

    ''' <summary>
    ''' Obtiene TODAS las licencias desde la vista vw_LicenciasCompletas, aplicando filtros.
    ''' (VERSIÓN REFACTORIZADA)
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(
        Optional filtroNombre As String = "",
        Optional filtroTipoId As Integer? = Nothing,
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing
    ) As Task(Of List(Of vw_LicenciasCompletas)) ' Devuelve directamente el tipo de la vista

        ' Apuntamos directamente a la nueva vista
        Dim query = _unitOfWork.Repository(Of vw_LicenciasCompletas)().GetAll().AsNoTracking()

        ' --- Aplicar filtros (la lógica es más limpia) ---
        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
            query = query.Where(Function(l) l.NombreFuncionario.Contains(filtroNombre) Or l.CI.Contains(filtroNombre))
        End If

        If filtroTipoId.HasValue AndAlso filtroTipoId.Value > 0 Then
            query = query.Where(Function(l) l.TipoLicenciaId = filtroTipoId.Value)
        End If

        ' **LÓGICA DE FECHA CORREGIDA** para encontrar licencias que se solapan con el rango.
        If fechaDesde.HasValue Then
            query = query.Where(Function(l) l.FechaFin >= fechaDesde.Value)
        End If

        If fechaHasta.HasValue Then
            query = query.Where(Function(l) l.FechaInicio <= fechaHasta.Value)
        End If

        ' --- Ya no se necesita un .Select() para transformar los datos ---
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