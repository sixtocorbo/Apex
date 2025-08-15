' Apex/Services/LicenciaService.vb
Imports System.Data.Entity
Imports System.Data.SqlClient

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
    ''' Obtiene licencias usando Full-Text Search de forma correcta.
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(
        Optional filtroNombre As String = "",
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing
    ) As Task(Of List(Of vw_LicenciasCompletas))

        Dim sqlBuilder As New System.Text.StringBuilder("SELECT * FROM vw_LicenciasCompletas WHERE 1=1")
        Dim parameters As New List(Of Object)

        If fechaDesde.HasValue Then
            sqlBuilder.Append(" AND FechaFin >= @p0")
            parameters.Add(New SqlParameter("@p0", fechaDesde.Value))
        End If

        If fechaHasta.HasValue Then
            sqlBuilder.Append(" AND FechaInicio <= @p" & parameters.Count)
            parameters.Add(New SqlParameter("@p" & parameters.Count, fechaHasta.Value))
        End If

        ' --- INICIO DE LA CORRECCIÓN ---
        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
            Dim terminos = filtroNombre.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).Select(Function(w) $"""{w}*""")
            Dim expresionFts = String.Join(" AND ", terminos)

            ' Se busca en la tabla Funcionario y se usa el resultado para filtrar la vista
            sqlBuilder.Append($" AND FuncionarioId IN (SELECT Id FROM dbo.Funcionario WHERE CONTAINS((Nombre, CI), @p{parameters.Count}))")
            parameters.Add(New SqlParameter($"@p{parameters.Count}", expresionFts))
        End If
        ' --- FIN DE LA CORRECCIÓN ---

        sqlBuilder.Append(" ORDER BY FechaInicio DESC")

        Dim query = _unitOfWork.Context.Database.SqlQuery(Of vw_LicenciasCompletas)(sqlBuilder.ToString(), parameters.ToArray())
        Return Await query.ToListAsync()
    End Function

    ' --- MÉTODOS PARA COMBOS (sin cambios) ---
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
    ''' <summary>
    ''' Obtiene una lista de todos los valores de estado únicos de la tabla de licencias.
    ''' </summary>
    Public Async Function ObtenerEstadosDeLicenciaAsync() As Task(Of List(Of String))
        Dim estados = Await _unitOfWork.Repository(Of HistoricoLicencia)().GetAll().AsNoTracking().
                        Select(Function(lic) lic.estado).
                        Distinct().
                        Where(Function(s) s IsNot Nothing AndAlso s <> "").
                        OrderBy(Function(s) s).
                        ToListAsync()
        Return estados
    End Function
End Class