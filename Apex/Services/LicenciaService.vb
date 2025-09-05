Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Text

' ---- Clases auxiliares ----
Public Class LicenciaEstacional
    Public Property Anio As Integer
    Public Property Mes As Integer
    Public Property Cantidad As Integer
End Class

Public Class LicenciaPrediccion
    Public Property Mes As Integer
    Public Property CantidadPromedio As Double
End Class

' ---- Servicio ----
Public Class LicenciaService
    Inherits GenericService(Of HistoricoLicencia)

    Private Shadows ReadOnly _unitOfWork As IUnitOfWork
    Private ReadOnly _auditoriaService As AuditoriaService

    Public Sub New()
        MyBase.New(New UnitOfWork())
        _unitOfWork = MyBase._unitOfWork
        _auditoriaService = New AuditoriaService(_unitOfWork) ' ← MISMA UoW
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
        _unitOfWork = unitOfWork
        _auditoriaService = New AuditoriaService(_unitOfWork) ' ← MISMA UoW
    End Sub

    Public ReadOnly Property UnitOfWork As IUnitOfWork
        Get
            Return _unitOfWork
        End Get
    End Property

    ' --- Helper auditoría: no rompe el flujo si falla ---
    Private Async Function RegistrarAuditoriaSeguraAsync(accion As String,
                                                         tipoEntidad As String,
                                                         descripcion As String,
                                                         Optional entidadId As Integer? = Nothing) As Task
        Try
            Await _auditoriaService.RegistrarActividadAsync(accion, tipoEntidad, descripcion, entidadId)
        Catch
            ' (Opcional) log a archivo/telemetría
        End Try
    End Function

    ' Elimina una licencia por Id en HistoricoLicencia
    Public Overloads Async Function DeleteAsync(id As Integer) As Task
        Dim setH = _unitOfWork.Context.Set(Of HistoricoLicencia)()
        Dim entity = Await setH.FirstOrDefaultAsync(Function(x) x.Id = id)
        If entity Is Nothing Then
            Throw New InvalidOperationException("La licencia no existe o ya fue eliminada.")
        End If

        ' Datos para auditoría ANTES de borrar
        Dim idEliminado As Integer = entity.Id
        Dim funcId As Integer = entity.FuncionarioId
        Dim tipoLicId As Integer = entity.TipoLicenciaId
        Dim fechaIni As Date = entity.inicio
        Dim fechaFin As Date = entity.finaliza
        Dim estado As String = If(entity.estado, String.Empty)

        setH.Remove(entity)
        Await _unitOfWork.CommitAsync()

        ' Auditoría
        Dim desc As String = $"Se eliminó la licencia #{idEliminado} del funcionario #{funcId}, tipo #{tipoLicId}, " &
                             $"rango {fechaIni:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}, estado='{estado}'."
        Await RegistrarAuditoriaSeguraAsync("Eliminar", "HistoricoLicencia", desc, idEliminado)
    End Function

    ' --------- Analítica (LINQ sobre EF: usa propiedades del modelo) ---------

    Public Function ObtenerDatosEstacionalidad(ByVal tipoLicenciaIDs As List(Of Integer),
                                               ByVal aniosSeleccionados As List(Of Integer)) As List(Of LicenciaEstacional)
        Using context As New ApexEntities()
            Dim query = context.HistoricoLicencia.AsQueryable()

            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
            End If

            If aniosSeleccionados IsNot Nothing AndAlso aniosSeleccionados.Any() Then
                query = query.Where(Function(lic) aniosSeleccionados.Contains(lic.inicio.Year))
            End If

            Dim datos = query.
                GroupBy(Function(lic) New With {Key .Anio = lic.inicio.Year, Key .Mes = lic.inicio.Month}).
                Select(Function(g) New LicenciaEstacional With {
                    .Anio = g.Key.Anio,
                    .Mes = g.Key.Mes,
                    .Cantidad = g.Count()
                }).
                OrderBy(Function(r) r.Anio).ThenBy(Function(r) r.Mes).
                ToList()

            ' 'Buscar' (opcional):
            ' _ = RegistrarAuditoriaSeguraAsync("Buscar", "HistoricoLicencia",
            '     $"Estacionalidad: tipos={String.Join(",", tipoLicenciaIDs ?? New List(Of Integer))}, años={String.Join(",", aniosSeleccionados ?? New List(Of Integer))} → {datos.Count} registros.")

            Return datos
        End Using
    End Function

    Public Function PredecirLicenciasPorMes(ByVal tipoLicenciaIDs As List(Of Integer),
                                            ByVal aniosParaPredecir As List(Of Integer)) As List(Of LicenciaPrediccion)
        Using context As New ApexEntities()
            Dim query = context.HistoricoLicencia.AsQueryable()

            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
            End If

            If aniosParaPredecir IsNot Nothing AndAlso aniosParaPredecir.Any() Then
                query = query.Where(Function(lic) aniosParaPredecir.Contains(lic.inicio.Year))
            End If

            Dim datos = query.
                GroupBy(Function(lic) lic.inicio.Month).
                Select(Function(g) New With {
                    .Mes = g.Key,
                    .TotalLicencias = g.Count(),
                    .AniosDistintos = g.Select(Function(x) x.inicio.Year).Distinct().Count()
                }).
                ToList()

            Dim prediccion = datos.
                Select(Function(r) New LicenciaPrediccion With {
                    .Mes = r.Mes,
                    .CantidadPromedio = If(r.AniosDistintos > 0, CDbl(r.TotalLicencias) / r.AniosDistintos, 0)
                }).
                OrderBy(Function(p) p.Mes).
                ToList()

            ' 'Buscar' (opcional):
            ' _ = RegistrarAuditoriaSeguraAsync("Buscar", "HistoricoLicencia",
            '     $"Predicción simple por mes: tipos={String.Join(",", tipoLicenciaIDs ?? New List(Of Integer))}, años={String.Join(",", aniosParaPredecir ?? New List(Of Integer))} → {prediccion.Count} meses.")

            Return prediccion
        End Using
    End Function

    Public Function GetAvailableYears() As List(Of Integer)
        Using context As New ApexEntities()
            Dim lista = context.HistoricoLicencia.
                   Select(Function(lic) lic.inicio.Year).
                   Distinct().
                   OrderByDescending(Function(y) y).
                   ToList()

            ' 'Buscar' (opcional):
            ' _ = RegistrarAuditoriaSeguraAsync("Buscar", "HistoricoLicencia",
            '     $"Años disponibles → {lista.Count} año(s).")

            Return lista
        End Using
    End Function

    ' --------- Consulta principal para la grilla (SQL crudo mapeado al DTO) ---------

    Public Async Function GetAllConDetallesAsync(
        Optional filtroNombre As String = "",
        Optional fechaDesde As Date? = Nothing,
        Optional fechaHasta As Date? = Nothing,
        Optional tiposLicenciaIds As List(Of Integer) = Nothing,
        Optional soloActivos As Boolean? = Nothing
    ) As Task(Of List(Of LicenciaConFuncionarioExtendidoDto))

        Dim sql As New StringBuilder()
        Dim parameters As New List(Of SqlParameter)()

        sql.AppendLine("SELECT")
        sql.AppendLine("    h.Id              AS LicenciaId,")
        sql.AppendLine("    h.FuncionarioId,")
        sql.AppendLine("    tl.Id             AS TipoLicenciaId,")
        sql.AppendLine("    tl.Nombre         AS TipoLicencia,")
        sql.AppendLine("    h.inicio          AS FechaInicio,")
        sql.AppendLine("    h.finaliza        AS FechaFin,")
        sql.AppendLine("    h.estado          AS EstadoLicencia,")
        sql.AppendLine("    f.CI,")
        sql.AppendLine("    f.Nombre          AS NombreFuncionario,")
        sql.AppendLine("    f.Activo,")
        sql.AppendLine("    f.FechaIngreso,")
        sql.AppendLine("    f.FechaNacimiento,")
        sql.AppendLine("    f.Domicilio,")
        sql.AppendLine("    f.Email,")
        sql.AppendLine("    CASE WHEN f.Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS EstadoActual,")
        sql.AppendLine("    COALESCE(c.Nombre,  'N/A') AS Cargo,")
        sql.AppendLine("    COALESCE(tf.Nombre, 'N/A') AS TipoDeFuncionario,")
        sql.AppendLine("    COALESCE(ecl.Nombre,'N/A') AS Escalafon,")
        sql.AppendLine("    COALESCE(fun.Nombre,'N/A') AS Funcion,")
        sql.AppendLine("    COALESCE(sec.Nombre,'N/A') AS Seccion,")
        sql.AppendLine("    COALESCE(pt.Nombre, 'N/A') AS PuestoDeTrabajo,")
        sql.AppendLine("    COALESCE(tur.Nombre,'N/A') AS Turno,")
        sql.AppendLine("    COALESCE(sem.Nombre,'N/A') AS Semana,")
        sql.AppendLine("    COALESCE(hor.Nombre,'N/A') AS Horario,")
        sql.AppendLine("    COALESCE(gen.Nombre,'N/A') AS Genero,")
        sql.AppendLine("    COALESCE(ec.Nombre, 'N/A') AS EstadoCivil,")
        sql.AppendLine("    COALESCE(ne.Nombre, 'N/A') AS NivelDeEstudio")
        sql.AppendLine("FROM dbo.HistoricoLicencia h")
        sql.AppendLine("JOIN dbo.Funcionario f ON f.Id = h.FuncionarioId")
        sql.AppendLine("JOIN dbo.TipoLicencia tl ON tl.Id = h.TipoLicenciaId")
        sql.AppendLine("LEFT JOIN dbo.Cargo c              ON c.Id = f.CargoId")
        sql.AppendLine("LEFT JOIN dbo.TipoFuncionario tf   ON tf.Id = f.TipoFuncionarioId")
        sql.AppendLine("LEFT JOIN dbo.Escalafon ecl        ON ecl.Id = f.EscalafonId")
        sql.AppendLine("LEFT JOIN dbo.Funcion fun          ON fun.Id = f.FuncionId")
        sql.AppendLine("LEFT JOIN dbo.Seccion sec          ON sec.Id = f.SeccionId")
        sql.AppendLine("LEFT JOIN dbo.PuestoTrabajo pt     ON pt.Id = f.PuestoTrabajoId")
        sql.AppendLine("LEFT JOIN dbo.Turno tur            ON tur.Id = f.TurnoId")
        sql.AppendLine("LEFT JOIN dbo.Semana sem           ON sem.Id = f.SemanaId")
        sql.AppendLine("LEFT JOIN dbo.Horario hor          ON hor.Id = f.HorarioId")
        sql.AppendLine("LEFT JOIN dbo.Genero gen           ON gen.Id = f.GeneroId")
        sql.AppendLine("LEFT JOIN dbo.EstadoCivil ec       ON ec.Id = f.EstadoCivilId")
        sql.AppendLine("LEFT JOIN dbo.NivelEstudio ne      ON ne.Id = f.NivelEstudioId")
        sql.AppendLine("WHERE 1 = 1")

        ' Rango de fechas
        If fechaDesde.HasValue Then
            sql.AppendLine("  AND h.finaliza >= @p0")
            parameters.Add(New SqlParameter("@p0", fechaDesde.Value))
        End If
        If fechaHasta.HasValue Then
            Dim pnameHasta As String = "@p" & parameters.Count
            sql.AppendLine($"  AND h.inicio <= {pnameHasta}")
            parameters.Add(New SqlParameter(pnameHasta, fechaHasta.Value))
        End If

        ' FTS sobre Funcionario (Nombre, CI)
        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
            Dim terminos = filtroNombre.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).
                Select(Function(w) $"""{w}*""")
            Dim expresionFts As String = String.Join(" AND ", terminos)
            Dim pnameFts As String = "@p" & parameters.Count
            sql.AppendLine($"  AND CONTAINS((f.Nombre, f.CI), {pnameFts})")
            parameters.Add(New SqlParameter(pnameFts, expresionFts))
        End If

        ' Filtro por tipos de licencia
        If tiposLicenciaIds IsNot Nothing AndAlso tiposLicenciaIds.Count > 0 Then
            Dim marcadores As New List(Of String)
            For i As Integer = 0 To tiposLicenciaIds.Count - 1
                Dim pname As String = "@p" & parameters.Count
                marcadores.Add(pname)
                parameters.Add(New SqlParameter(pname, tiposLicenciaIds(i)))
            Next
            sql.AppendLine("  AND tl.Id IN (" & String.Join(",", marcadores) & ")")
        End If

        ' Activos / Inactivos
        If soloActivos.HasValue Then
            sql.AppendLine(If(soloActivos.Value, "  AND f.Activo = 1", "  AND f.Activo = 0"))
        End If

        sql.AppendLine("ORDER BY h.inicio DESC")

        Dim query = _unitOfWork.Context.Database.SqlQuery(Of LicenciaConFuncionarioExtendidoDto)(
            sql.ToString(), parameters.ToArray()
        )

        Dim lista = Await query.ToListAsync()

        ' 'Buscar' (opcional):
        ' Dim tiposTxt As String = If(tiposLicenciaIds IsNot Nothing AndAlso tiposLicenciaIds.Any(),
        '                             String.Join(",", tiposLicenciaIds), "todos")
        ' Dim desc As String = $"Consulta grilla licencias: filtro='{filtroNombre}', desde={If(fechaDesde, Nothing)}, hasta={If(fechaHasta, Nothing)}, tipos={tiposTxt}, activos={If(soloActivos, Nothing)} → {lista.Count} fila(s)."
        ' Await RegistrarAuditoriaSeguraAsync("Buscar", "HistoricoLicencia", desc)

        Return lista
    End Function

    ' --------- Catálogos / Combos ---------

    Public Function GetAllTiposLicencia() As List(Of TipoLicencia)
        Using context As New ApexEntities()
            Dim lista = context.TipoLicencia.OrderBy(Function(tl) tl.Nombre).ToList()
            ' 'Buscar' (opcional): _ = RegistrarAuditoriaSeguraAsync("Buscar", "TipoLicencia", $"Tipos de licencia → {lista.Count}.")
            Return lista
        End Using
    End Function

    Public Async Function ObtenerTiposLicenciaParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim repo = _unitOfWork.Repository(Of TipoLicencia)()
        Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(t) t.Nombre).ToListAsync()
        ' 'Buscar' (opcional): _ = RegistrarAuditoriaSeguraAsync("Buscar", "TipoLicencia", $"Combo tipos → {lista.Count}.")
        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
    End Function

    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim funcionariosData = Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking().
            Where(Function(f) f.Activo).
            OrderBy(Function(f) f.Nombre).
            Select(Function(f) New With {.Id = f.Id, .Nombre = f.Nombre}).
            ToListAsync()

        ' 'Buscar' (opcional): _ = RegistrarAuditoriaSeguraAsync("Buscar", "Funcionario", $"Combo funcionarios activos → {funcionariosData.Count}.")
        Return funcionariosData.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
    End Function

    ''' <summary>
    ''' Estados únicos de HistoricoLicencia.estado
    ''' </summary>
    Public Async Function ObtenerEstadosDeLicenciaAsync() As Task(Of List(Of String))
        Dim estados = Await _unitOfWork.Repository(Of HistoricoLicencia)().GetAll().AsNoTracking().
                        Select(Function(lic) lic.estado).
                        Distinct().
                        Where(Function(s) s IsNot Nothing AndAlso s <> "").
                        OrderBy(Function(s) s).
                        ToListAsync()
        ' 'Buscar' (opcional): _ = RegistrarAuditoriaSeguraAsync("Buscar", "HistoricoLicencia", $"Estados de licencia → {estados.Count}.")
        Return estados
    End Function

    ' --------- Predicción con tendencia + estacionalidad ---------

    Public Function PredecirLicenciasConTendencia(ByVal tipoLicenciaIDs As List(Of Integer),
                                                  ByVal aniosParaPredecir As List(Of Integer)) As List(Of LicenciaPrediccion)
        Using context As New ApexEntities()
            Dim query = context.HistoricoLicencia.AsQueryable()

            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
            End If

            If aniosParaPredecir IsNot Nothing AndAlso aniosParaPredecir.Any() Then
                query = query.Where(Function(lic) aniosParaPredecir.Contains(lic.inicio.Year))
            End If

            Dim datosHistoricos = query.
                GroupBy(Function(lic) New With {Key .Anio = lic.inicio.Year, Key .Mes = lic.inicio.Month}).
                Select(Function(g) New With {.Anio = g.Key.Anio, .Mes = g.Key.Mes, .Cantidad = g.Count()}).
                OrderBy(Function(r) r.Anio).ThenBy(Function(r) r.Mes).
                ToList()

            If datosHistoricos.Count < 2 Then Return New List(Of LicenciaPrediccion)()

            Dim n As Integer = datosHistoricos.Count
            Dim minAnio As Integer = datosHistoricos.Min(Function(d) d.Anio)
            Dim sumX As Double = 0, sumY As Double = 0, sumXY As Double = 0, sumX2 As Double = 0

            For Each punto In datosHistoricos
                Dim x As Integer = (punto.Anio - minAnio) * 12 + punto.Mes
                Dim y As Double = punto.Cantidad
                sumX += x
                sumY += y
                sumXY += x * y
                sumX2 += x * x
            Next

            Dim m As Double = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX)
            Dim b As Double = (sumY - m * sumX) / n

            Dim desviacionesPorMes As New Dictionary(Of Integer, List(Of Double))
            For i As Integer = 1 To 12
                desviacionesPorMes.Add(i, New List(Of Double)())
            Next

            For Each punto In datosHistoricos
                Dim x As Integer = (punto.Anio - minAnio) * 12 + punto.Mes
                Dim valorTendencia As Double = m * x + b
                Dim desviacion As Double = punto.Cantidad - valorTendencia
                desviacionesPorMes(punto.Mes).Add(desviacion)
            Next

            Dim componenteEstacional As New Dictionary(Of Integer, Double)
            For i As Integer = 1 To 12
                componenteEstacional(i) = If(desviacionesPorMes(i).Any(), desviacionesPorMes(i).Average(), 0)
            Next

            Dim prediccion As New List(Of LicenciaPrediccion)
            Dim ultimoAnio As Integer = datosHistoricos.Max(Function(d) d.Anio)
            Dim ultimoMes As Integer = datosHistoricos.Where(Function(d) d.Anio = ultimoAnio).Max(Function(d) d.Mes)
            Dim xBase As Integer = (ultimoAnio - minAnio) * 12 + ultimoMes

            For i As Integer = 1 To 12
                Dim xFuturo As Integer = xBase + i
                Dim valorTendenciaFuturo As Double = m * xFuturo + b
                Dim mesFuturo As Integer = (ultimoMes + i - 1) Mod 12 + 1
                Dim valorPredicho As Double = valorTendenciaFuturo + componenteEstacional(mesFuturo)

                prediccion.Add(New LicenciaPrediccion With {
                    .Mes = mesFuturo,
                    .CantidadPromedio = If(valorPredicho < 0, 0, valorPredicho)
                })
            Next

            ' 'Buscar' (opcional):
            ' _ = RegistrarAuditoriaSeguraAsync("Buscar", "HistoricoLicencia",
            '     $"Predicción con tendencia: tipos={String.Join(",", tipoLicenciaIDs ?? New List(Of Integer))}, años={String.Join(",", aniosParaPredecir ?? New List(Of Integer))} → {prediccion.Count} meses.")

            Return prediccion.OrderBy(Function(p) p.Mes).ToList()
        End Using
    End Function

    ' En Apex/Services/LicenciaService.vb
    Public Async Function GetVigentesHoyAsync(
        Optional filtroNombre As String = "",
        Optional tiposLicenciaIds As List(Of Integer) = Nothing,
        Optional soloActivos As Boolean? = True
    ) As Task(Of List(Of LicenciaConFuncionarioExtendidoDto))

        Dim hoy As Date = Date.Today
        Dim lista = Await GetAllConDetallesAsync(
            filtroNombre:=filtroNombre,
            fechaDesde:=hoy,
            fechaHasta:=hoy,
            tiposLicenciaIds:=tiposLicenciaIds,
            soloActivos:=soloActivos
        )

        ' 'Buscar' (opcional):
        ' Await RegistrarAuditoriaSeguraAsync("Buscar", "HistoricoLicencia",
        '     $"Vigentes hoy ({hoy:yyyy-MM-dd}): filtro='{filtroNombre}', tipos={If(tiposLicenciaIds Is Nothing, "todos", String.Join(",", tiposLicenciaIds))}, activos={soloActivos} → {lista.Count}.")

        Return lista
    End Function

    Public Async Function GetSancionesAsync(
        Optional filtro As String = "",
        Optional tipoLicenciaId As Integer? = Nothing) As Task(Of List(Of vw_LicenciasCompletas))

        ' Usamos la vista correcta: vw_LicenciasCompletas
        Dim query = _unitOfWork.Repository(Of vw_LicenciasCompletas)().GetAll()

        ' Filtro por texto de búsqueda
        If Not String.IsNullOrWhiteSpace(filtro) Then
            query = query.Where(Function(s) s.NombreFuncionario.Contains(filtro) OrElse s.CI.Contains(filtro))
        End If

        If tipoLicenciaId.HasValue AndAlso tipoLicenciaId.Value > 0 Then
            ' Filtra por un tipo de sanción específico
            query = query.Where(Function(s) s.TipoLicenciaId = tipoLicenciaId.Value)
        Else
            ' Si no hay un tipo específico, trae TODAS las licencias que sean de categoría Sanción
            Dim idsCategoriasSancion As New List(Of Integer) From {
                ModConstantesApex.CATEGORIA_ID_SANCION_LEVE,
                ModConstantesApex.CATEGORIA_ID_SANCION_GRAVE
            }

            ' Necesitamos unir con TipoLicencia para acceder a CategoriaAusenciaId
            Dim licenciasQuery = _unitOfWork.Repository(Of TipoLicencia)().GetAll()
            Dim idsLicenciasSancion = Await licenciasQuery _
                .Where(Function(tl) idsCategoriasSancion.Contains(tl.CategoriaAusenciaId)) _
                .Select(Function(tl) tl.Id) _
                .ToListAsync()

            query = query.Where(Function(s) idsLicenciasSancion.Contains(s.TipoLicenciaId))
        End If

        Dim lista = Await query.OrderByDescending(Function(s) s.FechaInicio).ToListAsync()

        ' 'Buscar' (opcional):
        ' Await RegistrarAuditoriaSeguraAsync("Buscar", "HistoricoLicencia",
        '     $"Sanciones: filtro='{filtro}', tipoId={If(tipoLicenciaId, 0)} → {lista.Count}.")

        Return lista
    End Function
End Class

' ---- DTO para el SqlQuery de GetAllConDetallesAsync ----
Public Class LicenciaConFuncionarioExtendidoDto
    ' --- Licencia ---
    Public Property LicenciaId As Integer?
    Public Property FuncionarioId As Integer
    Public Property TipoLicenciaId As Integer?
    Public Property TipoLicencia As String
    Public Property FechaInicio As Date
    Public Property FechaFin As Date
    Public Property EstadoLicencia As String
    Public Property Observaciones As String

    ' --- Funcionario (base) ---
    Public Property CI As String
    Public Property NombreFuncionario As String
    Public Property Activo As Boolean
    Public Property FechaIngreso As Date?
    Public Property FechaNacimiento As Date?
    Public Property Domicilio As String
    Public Property Email As String

    ' --- Relaciones: Nombres (los Ids pueden venir Nothing si no los seleccionás) ---
    Public Property CargoId As Integer?
    Public Property Cargo As String
    Public Property TipoFuncionarioId As Integer?
    Public Property TipoDeFuncionario As String
    Public Property EscalafonId As Integer?
    Public Property Escalafon As String
    Public Property FuncionId As Integer?
    Public Property Funcion As String
    Public Property EstadoActual As String
    Public Property SeccionId As Integer?
    Public Property Seccion As String
    Public Property PuestoTrabajoId As Integer?
    Public Property PuestoDeTrabajo As String
    Public Property TurnoId As Integer?
    Public Property Turno As String
    Public Property SemanaId As Integer?
    Public Property Semana As String
    Public Property HorarioId As Integer?
    Public Property Horario As String
    Public Property GeneroId As Integer?
    Public Property Genero As String
    Public Property EstadoCivilId As Integer?
    Public Property EstadoCivil As String
    Public Property NivelEstudioId As Integer?
    Public Property NivelDeEstudio As String

    ' --- Extras útiles para la UI ---
    Public ReadOnly Property AnioInicio As Integer
        Get
            Return FechaInicio.Year
        End Get
    End Property

    Public ReadOnly Property MesInicio As Integer
        Get
            Return FechaInicio.Month
        End Get
    End Property

    Public ReadOnly Property DuracionDias As Integer
        Get
            Return CInt((FechaFin.Date - FechaInicio.Date).TotalDays) + 1
        End Get
    End Property

    Public ReadOnly Property Rango As String
        Get
            Return $"{FechaInicio:yyyy-MM-dd} a {FechaFin:yyyy-MM-dd}"
        End Get
    End Property
End Class

'Option Strict On
'Option Explicit On

'Imports System.Data.Entity
'Imports System.Data.SqlClient
'Imports System.Text

'' ---- Clases auxiliares ----
'Public Class LicenciaEstacional
'    Public Property Anio As Integer
'    Public Property Mes As Integer
'    Public Property Cantidad As Integer
'End Class

'Public Class LicenciaPrediccion
'    Public Property Mes As Integer
'    Public Property CantidadPromedio As Double
'End Class

'' ---- Servicio ----
'Public Class LicenciaService
'    Inherits GenericService(Of HistoricoLicencia)

'    Public Sub New()
'        MyBase.New(New UnitOfWork())
'    End Sub

'    Public Sub New(unitOfWork As IUnitOfWork)
'        MyBase.New(unitOfWork)
'    End Sub

'    Public ReadOnly Property UnitOfWork As IUnitOfWork
'        Get
'            Return _unitOfWork
'        End Get
'    End Property

'    ' Elimina una licencia por Id en HistoricoLicencia
'    Public Overloads Async Function DeleteAsync(id As Integer) As Task
'        Dim setH = _unitOfWork.Context.Set(Of HistoricoLicencia)()
'        Dim entity = Await setH.FirstOrDefaultAsync(Function(x) x.Id = id)
'        If entity Is Nothing Then
'            Throw New InvalidOperationException("La licencia no existe o ya fue eliminada.")
'        End If
'        setH.Remove(entity)
'        Await _unitOfWork.CommitAsync()
'    End Function

'    ' --------- Analítica (LINQ sobre EF: usa propiedades del modelo) ---------

'    Public Function ObtenerDatosEstacionalidad(ByVal tipoLicenciaIDs As List(Of Integer),
'                                               ByVal aniosSeleccionados As List(Of Integer)) As List(Of LicenciaEstacional)
'        Using context As New ApexEntities()
'            Dim query = context.HistoricoLicencia.AsQueryable()

'            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
'                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
'            End If

'            If aniosSeleccionados IsNot Nothing AndAlso aniosSeleccionados.Any() Then
'                query = query.Where(Function(lic) aniosSeleccionados.Contains(lic.inicio.Year))
'            End If

'            Dim datos = query.
'                GroupBy(Function(lic) New With {Key .Anio = lic.inicio.Year, Key .Mes = lic.inicio.Month}).
'                Select(Function(g) New LicenciaEstacional With {
'                    .Anio = g.Key.Anio,
'                    .Mes = g.Key.Mes,
'                    .Cantidad = g.Count()
'                }).
'                OrderBy(Function(r) r.Anio).ThenBy(Function(r) r.Mes).
'                ToList()

'            Return datos
'        End Using
'    End Function

'    Public Function PredecirLicenciasPorMes(ByVal tipoLicenciaIDs As List(Of Integer),
'                                            ByVal aniosParaPredecir As List(Of Integer)) As List(Of LicenciaPrediccion)
'        Using context As New ApexEntities()
'            Dim query = context.HistoricoLicencia.AsQueryable()

'            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
'                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
'            End If

'            If aniosParaPredecir IsNot Nothing AndAlso aniosParaPredecir.Any() Then
'                query = query.Where(Function(lic) aniosParaPredecir.Contains(lic.inicio.Year))
'            End If

'            Dim datos = query.
'                GroupBy(Function(lic) lic.inicio.Month).
'                Select(Function(g) New With {
'                    .Mes = g.Key,
'                    .TotalLicencias = g.Count(),
'                    .AniosDistintos = g.Select(Function(x) x.inicio.Year).Distinct().Count()
'                }).
'                ToList()

'            Dim prediccion = datos.
'                Select(Function(r) New LicenciaPrediccion With {
'                    .Mes = r.Mes,
'                    .CantidadPromedio = If(r.AniosDistintos > 0, CDbl(r.TotalLicencias) / r.AniosDistintos, 0)
'                }).
'                OrderBy(Function(p) p.Mes).
'                ToList()

'            Return prediccion
'        End Using
'    End Function

'    Public Function GetAvailableYears() As List(Of Integer)
'        Using context As New ApexEntities()
'            Return context.HistoricoLicencia.
'                   Select(Function(lic) lic.inicio.Year).
'                   Distinct().
'                   OrderByDescending(Function(y) y).
'                   ToList()
'        End Using
'    End Function

'    ' --------- Consulta principal para la grilla (SQL crudo mapeado al DTO) ---------

'    Public Async Function GetAllConDetallesAsync(
'    Optional filtroNombre As String = "",
'    Optional fechaDesde As Date? = Nothing,
'    Optional fechaHasta As Date? = Nothing,
'    Optional tiposLicenciaIds As List(Of Integer) = Nothing,
'    Optional soloActivos As Boolean? = Nothing
') As Task(Of List(Of LicenciaConFuncionarioExtendidoDto))

'        Dim sql As New Text.StringBuilder()
'        Dim parameters As New List(Of SqlClient.SqlParameter)()

'        sql.AppendLine("SELECT")
'        sql.AppendLine("    h.Id              AS LicenciaId,")
'        sql.AppendLine("    h.FuncionarioId,")
'        sql.AppendLine("    tl.Id             AS TipoLicenciaId,")
'        sql.AppendLine("    tl.Nombre         AS TipoLicencia,")
'        ' >>> columnas reales con alias para el DTO
'        sql.AppendLine("    h.inicio          AS FechaInicio,")
'        sql.AppendLine("    h.finaliza        AS FechaFin,")
'        sql.AppendLine("    h.estado          AS EstadoLicencia,")
'        sql.AppendLine("    f.CI,")
'        sql.AppendLine("    f.Nombre          AS NombreFuncionario,")
'        sql.AppendLine("    f.Activo,")
'        sql.AppendLine("    f.FechaIngreso,")
'        sql.AppendLine("    f.FechaNacimiento,")
'        sql.AppendLine("    f.Domicilio,")
'        sql.AppendLine("    f.Email,")
'        sql.AppendLine("    CASE WHEN f.Activo = 1 THEN 'Activo' ELSE 'Inactivo' END AS EstadoActual,")
'        sql.AppendLine("    COALESCE(c.Nombre,  'N/A') AS Cargo,")
'        sql.AppendLine("    COALESCE(tf.Nombre, 'N/A') AS TipoDeFuncionario,")
'        sql.AppendLine("    COALESCE(ecl.Nombre,'N/A') AS Escalafon,")
'        sql.AppendLine("    COALESCE(fun.Nombre,'N/A') AS Funcion,")
'        sql.AppendLine("    COALESCE(sec.Nombre,'N/A') AS Seccion,")
'        sql.AppendLine("    COALESCE(pt.Nombre, 'N/A') AS PuestoDeTrabajo,")
'        sql.AppendLine("    COALESCE(tur.Nombre,'N/A') AS Turno,")
'        sql.AppendLine("    COALESCE(sem.Nombre,'N/A') AS Semana,")
'        sql.AppendLine("    COALESCE(hor.Nombre,'N/A') AS Horario,")
'        sql.AppendLine("    COALESCE(gen.Nombre,'N/A') AS Genero,")
'        sql.AppendLine("    COALESCE(ec.Nombre, 'N/A') AS EstadoCivil,")
'        sql.AppendLine("    COALESCE(ne.Nombre, 'N/A') AS NivelDeEstudio")
'        sql.AppendLine("FROM dbo.HistoricoLicencia h")
'        sql.AppendLine("JOIN dbo.Funcionario f ON f.Id = h.FuncionarioId")
'        sql.AppendLine("JOIN dbo.TipoLicencia tl ON tl.Id = h.TipoLicenciaId")
'        sql.AppendLine("LEFT JOIN dbo.Cargo c              ON c.Id = f.CargoId")
'        sql.AppendLine("LEFT JOIN dbo.TipoFuncionario tf   ON tf.Id = f.TipoFuncionarioId")
'        sql.AppendLine("LEFT JOIN dbo.Escalafon ecl        ON ecl.Id = f.EscalafonId")
'        sql.AppendLine("LEFT JOIN dbo.Funcion fun          ON fun.Id = f.FuncionId")
'        sql.AppendLine("LEFT JOIN dbo.Seccion sec          ON sec.Id = f.SeccionId")
'        sql.AppendLine("LEFT JOIN dbo.PuestoTrabajo pt     ON pt.Id = f.PuestoTrabajoId")
'        sql.AppendLine("LEFT JOIN dbo.Turno tur            ON tur.Id = f.TurnoId")
'        sql.AppendLine("LEFT JOIN dbo.Semana sem           ON sem.Id = f.SemanaId")
'        sql.AppendLine("LEFT JOIN dbo.Horario hor          ON hor.Id = f.HorarioId")
'        sql.AppendLine("LEFT JOIN dbo.Genero gen           ON gen.Id = f.GeneroId")
'        sql.AppendLine("LEFT JOIN dbo.EstadoCivil ec       ON ec.Id = f.EstadoCivilId")
'        sql.AppendLine("LEFT JOIN dbo.NivelEstudio ne      ON ne.Id = f.NivelEstudioId")
'        sql.AppendLine("WHERE 1 = 1")

'        ' --- Rango de fechas (coincide con tu convención: finaliza >= desde e inicio <= hasta) ---
'        If fechaDesde.HasValue Then
'            sql.AppendLine("  AND h.finaliza >= @p0")
'            parameters.Add(New SqlClient.SqlParameter("@p0", fechaDesde.Value))
'        End If
'        If fechaHasta.HasValue Then
'            Dim pname = "@p" & parameters.Count
'            sql.AppendLine($"  AND h.inicio <= {pname}")
'            parameters.Add(New SqlClient.SqlParameter(pname, fechaHasta.Value))
'        End If

'        ' --- FTS sobre Funcionario (Nombre, CI) ---
'        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
'            Dim terminos = filtroNombre.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).
'                Select(Function(w) $"""{w}*""")
'            Dim expresionFts = String.Join(" AND ", terminos)
'            Dim pname = "@p" & parameters.Count
'            sql.AppendLine($"  AND CONTAINS((f.Nombre, f.CI), {pname})")
'            parameters.Add(New SqlClient.SqlParameter(pname, expresionFts))
'        End If

'        ' --- Filtro por tipos de licencia ---
'        If tiposLicenciaIds IsNot Nothing AndAlso tiposLicenciaIds.Count > 0 Then
'            Dim marcadores As New List(Of String)
'            For i = 0 To tiposLicenciaIds.Count - 1
'                Dim pname = "@p" & parameters.Count
'                marcadores.Add(pname)
'                parameters.Add(New SqlClient.SqlParameter(pname, tiposLicenciaIds(i)))
'            Next
'            sql.AppendLine("  AND tl.Id IN (" & String.Join(",", marcadores) & ")")
'        End If

'        ' --- Activos / Inactivos ---
'        If soloActivos.HasValue Then
'            sql.AppendLine(If(soloActivos.Value, "  AND f.Activo = 1", "  AND f.Activo = 0"))
'        End If

'        sql.AppendLine("ORDER BY h.inicio DESC")

'        Dim query = _unitOfWork.Context.Database.SqlQuery(Of LicenciaConFuncionarioExtendidoDto)(
'        sql.ToString(), parameters.ToArray()
'    )

'        Return Await query.ToListAsync()
'    End Function



'    ' --------- Catálogos / Combos ---------

'    Public Function GetAllTiposLicencia() As List(Of TipoLicencia)
'        Using context As New ApexEntities()
'            Return context.TipoLicencia.OrderBy(Function(tl) tl.Nombre).ToList()
'        End Using
'    End Function

'    Public Async Function ObtenerTiposLicenciaParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim repo = _unitOfWork.Repository(Of TipoLicencia)()
'        Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(t) t.Nombre).ToListAsync()
'        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim funcionariosData = Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking().
'            Where(Function(f) f.Activo).
'            OrderBy(Function(f) f.Nombre).
'            Select(Function(f) New With {.Id = f.Id, .Nombre = f.Nombre}).
'            ToListAsync()

'        Return funcionariosData.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
'    End Function

'    ''' <summary>
'    ''' Estados únicos de HistoricoLicencia.estado
'    ''' </summary>
'    Public Async Function ObtenerEstadosDeLicenciaAsync() As Task(Of List(Of String))
'        Dim estados = Await _unitOfWork.Repository(Of HistoricoLicencia)().GetAll().AsNoTracking().
'                        Select(Function(lic) lic.estado).
'                        Distinct().
'                        Where(Function(s) s IsNot Nothing AndAlso s <> "").
'                        OrderBy(Function(s) s).
'                        ToListAsync()
'        Return estados
'    End Function

'    ' --------- Predicción con tendencia + estacionalidad ---------

'    Public Function PredecirLicenciasConTendencia(ByVal tipoLicenciaIDs As List(Of Integer),
'                                                  ByVal aniosParaPredecir As List(Of Integer)) As List(Of LicenciaPrediccion)
'        Using context As New ApexEntities()
'            Dim query = context.HistoricoLicencia.AsQueryable()

'            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
'                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
'            End If

'            If aniosParaPredecir IsNot Nothing AndAlso aniosParaPredecir.Any() Then
'                query = query.Where(Function(lic) aniosParaPredecir.Contains(lic.inicio.Year))
'            End If

'            Dim datosHistoricos = query.
'                GroupBy(Function(lic) New With {Key .Anio = lic.inicio.Year, Key .Mes = lic.inicio.Month}).
'                Select(Function(g) New With {.Anio = g.Key.Anio, .Mes = g.Key.Mes, .Cantidad = g.Count()}).
'                OrderBy(Function(r) r.Anio).ThenBy(Function(r) r.Mes).
'                ToList()

'            If datosHistoricos.Count < 2 Then Return New List(Of LicenciaPrediccion)()

'            Dim n = datosHistoricos.Count
'            Dim minAnio = datosHistoricos.Min(Function(d) d.Anio)
'            Dim sumX As Double = 0, sumY As Double = 0, sumXY As Double = 0, sumX2 As Double = 0

'            For Each punto In datosHistoricos
'                Dim x = (punto.Anio - minAnio) * 12 + punto.Mes
'                Dim y = punto.Cantidad
'                sumX += x
'                sumY += y
'                sumXY += x * y
'                sumX2 += x * x
'            Next

'            Dim m = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX)
'            Dim b = (sumY - m * sumX) / n

'            Dim desviacionesPorMes As New Dictionary(Of Integer, List(Of Double))
'            For i = 1 To 12
'                desviacionesPorMes.Add(i, New List(Of Double)())
'            Next

'            For Each punto In datosHistoricos
'                Dim x = (punto.Anio - minAnio) * 12 + punto.Mes
'                Dim valorTendencia = m * x + b
'                Dim desviacion = punto.Cantidad - valorTendencia
'                desviacionesPorMes(punto.Mes).Add(desviacion)
'            Next

'            Dim componenteEstacional As New Dictionary(Of Integer, Double)
'            For i = 1 To 12
'                componenteEstacional(i) = If(desviacionesPorMes(i).Any(), desviacionesPorMes(i).Average(), 0)
'            Next

'            Dim prediccion As New List(Of LicenciaPrediccion)
'            Dim ultimoAnio = datosHistoricos.Max(Function(d) d.Anio)
'            Dim ultimoMes = datosHistoricos.Where(Function(d) d.Anio = ultimoAnio).Max(Function(d) d.Mes)
'            Dim xBase = (ultimoAnio - minAnio) * 12 + ultimoMes

'            For i = 1 To 12
'                Dim xFuturo = xBase + i
'                Dim valorTendenciaFuturo = m * xFuturo + b
'                Dim mesFuturo = (ultimoMes + i - 1) Mod 12 + 1
'                Dim valorPredicho = valorTendenciaFuturo + componenteEstacional(mesFuturo)

'                prediccion.Add(New LicenciaPrediccion With {
'                    .Mes = mesFuturo,
'                    .CantidadPromedio = If(valorPredicho < 0, 0, valorPredicho)
'                })
'            Next

'            Return prediccion.OrderBy(Function(p) p.Mes).ToList()
'        End Using
'    End Function
'    ' En Apex/Services/LicenciaService.vb
'    Public Async Function GetVigentesHoyAsync(
'    Optional filtroNombre As String = "",
'    Optional tiposLicenciaIds As List(Of Integer) = Nothing,
'    Optional soloActivos As Boolean? = True
') As Task(Of List(Of LicenciaConFuncionarioExtendidoDto))

'        Dim hoy = Date.Today
'        Return Await GetAllConDetallesAsync(
'        filtroNombre:=filtroNombre,
'        fechaDesde:=hoy,
'        fechaHasta:=hoy,
'        tiposLicenciaIds:=tiposLicenciaIds,
'        soloActivos:=soloActivos
'    )
'    End Function
'    Public Async Function GetSancionesAsync(
'        Optional filtro As String = "",
'        Optional tipoLicenciaId As Integer? = Nothing) As Task(Of List(Of vw_LicenciasCompletas))

'        ' Usamos la vista correcta: vw_LicenciasCompletas
'        Dim query = _unitOfWork.Repository(Of vw_LicenciasCompletas)().GetAll()

'        ' Filtro por texto de búsqueda
'        If Not String.IsNullOrWhiteSpace(filtro) Then
'            query = query.Where(Function(s) s.NombreFuncionario.Contains(filtro) Or s.CI.Contains(filtro))
'        End If

'        If tipoLicenciaId.HasValue AndAlso tipoLicenciaId.Value > 0 Then
'            ' Filtra por un tipo de sanción específico
'            query = query.Where(Function(s) s.TipoLicenciaId = tipoLicenciaId.Value)
'        Else
'            ' Si no hay un tipo específico, trae TODAS las licencias que sean de categoría Sanción
'            Dim idsCategoriasSancion As New List(Of Integer) From {
'                ModConstantesApex.CATEGORIA_ID_SANCION_LEVE,
'                ModConstantesApex.CATEGORIA_ID_SANCION_GRAVE
'            }

'            ' Para esto, necesitamos unir con TipoLicencia para acceder a CategoriaAusenciaId
'            Dim licenciasQuery = _unitOfWork.Repository(Of TipoLicencia)().GetAll()
'            Dim idsLicenciasSancion = Await licenciasQuery _
'                .Where(Function(tl) idsCategoriasSancion.Contains(tl.CategoriaAusenciaId)) _
'                .Select(Function(tl) tl.Id) _
'                .ToListAsync()

'            query = query.Where(Function(s) idsLicenciasSancion.Contains(s.TipoLicenciaId))
'        End If

'        Return Await query.OrderByDescending(Function(s) s.FechaInicio).ToListAsync()
'    End Function
'End Class

'' ---- DTO para el SqlQuery de GetAllConDetallesAsync ----
'Public Class LicenciaConFuncionarioExtendidoDto
'    ' --- Licencia ---
'    Public Property LicenciaId As Integer?
'    Public Property FuncionarioId As Integer
'    Public Property TipoLicenciaId As Integer?
'    Public Property TipoLicencia As String
'    Public Property FechaInicio As Date
'    Public Property FechaFin As Date
'    Public Property EstadoLicencia As String
'    Public Property Observaciones As String

'    ' --- Funcionario (base) ---
'    Public Property CI As String
'    Public Property NombreFuncionario As String
'    Public Property Activo As Boolean
'    Public Property FechaIngreso As Date?
'    Public Property FechaNacimiento As Date?
'    Public Property Domicilio As String
'    Public Property Email As String

'    ' --- Relaciones: Nombres (los Ids pueden venir Nothing si no los seleccionás) ---
'    Public Property CargoId As Integer?
'    Public Property Cargo As String

'    Public Property TipoFuncionarioId As Integer?
'    Public Property TipoDeFuncionario As String

'    Public Property EscalafonId As Integer?
'    Public Property Escalafon As String

'    Public Property FuncionId As Integer?
'    Public Property Funcion As String

'    Public Property EstadoActual As String

'    Public Property SeccionId As Integer?
'    Public Property Seccion As String

'    Public Property PuestoTrabajoId As Integer?
'    Public Property PuestoDeTrabajo As String

'    Public Property TurnoId As Integer?
'    Public Property Turno As String

'    Public Property SemanaId As Integer?
'    Public Property Semana As String

'    Public Property HorarioId As Integer?
'    Public Property Horario As String

'    Public Property GeneroId As Integer?
'    Public Property Genero As String

'    Public Property EstadoCivilId As Integer?
'    Public Property EstadoCivil As String

'    Public Property NivelEstudioId As Integer?
'    Public Property NivelDeEstudio As String

'    ' --- Extras útiles para la UI ---
'    Public ReadOnly Property AnioInicio As Integer
'        Get
'            Return FechaInicio.Year
'        End Get
'    End Property

'    Public ReadOnly Property MesInicio As Integer
'        Get
'            Return FechaInicio.Month
'        End Get
'    End Property

'    Public ReadOnly Property DuracionDias As Integer
'        Get
'            Return CInt((FechaFin.Date - FechaInicio.Date).TotalDays) + 1
'        End Get
'    End Property

'    Public ReadOnly Property Rango As String
'        Get
'            Return $"{FechaInicio:yyyy-MM-dd} a {FechaFin:yyyy-MM-dd}"
'        End Get
'    End Property
'End Class