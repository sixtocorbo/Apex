' Apex/Services/LicenciaService.vb
Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Data.SqlClient

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

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' ============================================================
    ' CRUD con UoW por operación (¡sin compartir DbContext!)
    ' ============================================================

    ' CREATE
    Public Shadows Async Function CreateAsync(entity As HistoricoLicencia) As Task(Of Integer)
        Using uow As New UnitOfWork()
            Dim auditor = New AuditoriaService(uow)

            uow.Repository(Of HistoricoLicencia)().Add(entity)
            auditor.EncolarActividad(
                accion:="Crear",
                tipoEntidad:="HistoricoLicencia",
                descripcion:=$"Alta licencia para Func#{entity.FuncionarioId}, Tipo#{entity.TipoLicenciaId}, {entity.inicio:yyyy-MM-dd} a {entity.finaliza:yyyy-MM-dd}.",
                entidadId:=Nothing
            )

            Await uow.CommitAsync()
            Return CInt(entity.GetType().GetProperty("Id").GetValue(entity))
        End Using
    End Function

    ' READ (override para evitar entidades rastreadas)
    Public Shadows Async Function GetByIdAsync(id As Integer) As Task(Of HistoricoLicencia)
        Using uow As New UnitOfWork()
            Return Await uow.Context.Set(Of HistoricoLicencia)().
                AsNoTracking().
                FirstOrDefaultAsync(Function(lic) lic.Id = id)
        End Using
    End Function

    ' UPDATE
    Public Shadows Async Function UpdateAsync(entity As HistoricoLicencia) As Task
        Using uow As New UnitOfWork()
            Dim auditor = New AuditoriaService(uow)

            uow.Repository(Of HistoricoLicencia)().Update(entity)
            auditor.EncolarActividad(
                accion:="Actualizar",
                tipoEntidad:="HistoricoLicencia",
                descripcion:=$"Modif. licencia #{entity.Id}: Func#{entity.FuncionarioId}, Tipo#{entity.TipoLicenciaId}, {entity.inicio:yyyy-MM-dd} a {entity.finaliza:yyyy-MM-dd}, Estado='{entity.estado}'.",
                entidadId:=entity.Id
            )

            Await uow.CommitAsync()
        End Using
    End Function

    ' DELETE por Id
    Public Overloads Async Function DeleteAsync(id As Integer) As Task
        Using uow As New UnitOfWork()
            Dim setH = uow.Context.Set(Of HistoricoLicencia)()
            Dim entity = Await setH.FirstOrDefaultAsync(Function(x) x.Id = id)
            If entity Is Nothing Then
                Throw New InvalidOperationException("La licencia no existe o ya fue eliminada.")
            End If

            ' Datos para auditoría
            Dim idEliminado As Integer = entity.Id
            Dim funcId As Integer = entity.FuncionarioId
            Dim tipoLicId As Integer = entity.TipoLicenciaId
            Dim fechaIni As Date = entity.inicio
            Dim fechaFin As Date = entity.finaliza
            Dim estado As String = If(entity.estado, String.Empty)

            Dim auditor = New AuditoriaService(uow)

            setH.Remove(entity)
            auditor.EncolarActividad(
                accion:="Eliminar",
                tipoEntidad:="HistoricoLicencia",
                descripcion:=$"Se eliminó licencia #{idEliminado} de Func#{funcId}, Tipo#{tipoLicId}, {fechaIni:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}, Estado='{estado}'.",
                entidadId:=idEliminado
            )

            Await uow.CommitAsync()
        End Using
    End Function

    ' ============================================================
    ' Analítica (LINQ sobre EF): nuevo UoW por método
    ' ============================================================

    Public Function ObtenerDatosEstacionalidad(tipoLicenciaIDs As List(Of Integer),
                                                 aniosSeleccionados As List(Of Integer)) As List(Of LicenciaEstacional)
        Using context As New ApexEntities()
            Dim query = context.HistoricoLicencia.AsNoTracking().AsQueryable()

            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
            End If

            If aniosSeleccionados IsNot Nothing AndAlso aniosSeleccionados.Any() Then
                query = query.Where(Function(lic) aniosSeleccionados.Contains(lic.inicio.Year))
            End If

            Return query.
                GroupBy(Function(lic) New With {.Anio = lic.inicio.Year, .Mes = lic.inicio.Month}).
                Select(Function(g) New LicenciaEstacional With {.Anio = g.Key.Anio, .Mes = g.Key.Mes, .Cantidad = g.Count()}).
                OrderBy(Function(r) r.Anio).ThenBy(Function(r) r.Mes).
                ToList()
        End Using
    End Function

    Public Function PredecirLicenciasPorMes(tipoLicenciaIDs As List(Of Integer),
                                              aniosParaPredecir As List(Of Integer)) As List(Of LicenciaPrediccion)
        Using context As New ApexEntities()
            Dim query = context.HistoricoLicencia.AsNoTracking().AsQueryable()

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
                }).ToList()

            Return datos.
                Select(Function(r) New LicenciaPrediccion With {
                    .Mes = r.Mes,
                    .CantidadPromedio = If(r.AniosDistintos > 0, CDbl(r.TotalLicencias) / r.AniosDistintos, 0)
                }).
                OrderBy(Function(p) p.Mes).
                ToList()
        End Using
    End Function

    Public Function GetAvailableYears() As List(Of Integer)
        Using context As New ApexEntities()
            Return context.HistoricoLicencia.AsNoTracking().
                    Select(Function(lic) lic.inicio.Year).
                    Distinct().
                    OrderByDescending(Function(y) y).
                    ToList()
        End Using
    End Function

    Public Function GetAllTiposLicencia() As List(Of TipoLicencia)
        Using context As New ApexEntities()
            Return context.TipoLicencia.OrderBy(Function(tl) tl.Nombre).ToList()
        End Using
    End Function

    Public Async Function ObtenerTiposLicenciaParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of TipoLicencia)().
                AsNoTracking().
                OrderBy(Function(t) t.Nombre).
                ToListAsync()
            Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim funcionariosData = Await uow.Context.Set(Of Funcionario)().
                AsNoTracking().
                OrderBy(Function(f) f.Nombre).
                Select(Function(f) New With {.Id = f.Id, .Nombre = f.Nombre}).
                ToListAsync()
            Return funcionariosData.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerEstadosDeLicenciaAsync() As Task(Of List(Of String))
        Using uow As New UnitOfWork()
            Dim estados = Await uow.Context.Set(Of HistoricoLicencia)().
                AsNoTracking().
                Select(Function(lic) lic.estado).
                Distinct().
                Where(Function(s) s IsNot Nothing AndAlso s <> "").
                OrderBy(Function(s) s).
                ToListAsync()
            Return estados
        End Using
    End Function

    ' ============================================================
    ' Predicción con tendencia + estacionalidad (lectura)
    ' ============================================================

    Public Function PredecirLicenciasConTendencia(tipoLicenciaIDs As List(Of Integer),
                                                  aniosParaPredecir As List(Of Integer)) As List(Of LicenciaPrediccion)
        Using context As New ApexEntities()
            Dim query = context.HistoricoLicencia.AsQueryable()

            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
            End If
            If aniosParaPredecir IsNot Nothing AndAlso aniosParaPredecir.Any() Then
                query = query.Where(Function(lic) aniosParaPredecir.Contains(lic.inicio.Year))
            End If

            Dim datosHistoricos = query.
                GroupBy(Function(lic) New With {.Anio = lic.inicio.Year, .Mes = lic.inicio.Month}).
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
                sumX += x : sumY += y : sumXY += x * y : sumX2 += x * x
            Next

            Dim m As Double = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX)
            Dim b As Double = (sumY - m * sumX) / n

            Dim desvPorMes As Dictionary(Of Integer, List(Of Double)) =
    Enumerable.Range(1, 12).ToDictionary(Function(i) i, Function(i) New List(Of Double)())

            For Each punto In datosHistoricos
                Dim x As Integer = (punto.Anio - minAnio) * 12 + punto.Mes
                Dim tendencia As Double = m * x + b
                desvPorMes(punto.Mes).Add(punto.Cantidad - tendencia)
            Next

            Dim estacional As New Dictionary(Of Integer, Double)(desvPorMes.ToDictionary(Function(kv) kv.Key, Function(kv) If(kv.Value.Any(), kv.Value.Average(), 0.0)))

            Dim pred As New List(Of LicenciaPrediccion)
            Dim ultimoAnio As Integer = datosHistoricos.Max(Function(d) d.Anio)
            Dim ultimoMes As Integer = datosHistoricos.Where(Function(d) d.Anio = ultimoAnio).Max(Function(d) d.Mes)
            Dim xBase As Integer = (ultimoAnio - minAnio) * 12 + ultimoMes

            For i As Integer = 1 To 12
                Dim xFuturo As Integer = xBase + i
                Dim tendenciaFuturo As Double = m * xFuturo + b
                Dim mesFuturo As Integer = (ultimoMes + i - 1) Mod 12 + 1
                Dim valor As Double = tendenciaFuturo + estacional(mesFuturo)
                pred.Add(New LicenciaPrediccion With {.Mes = mesFuturo, .CantidadPromedio = If(valor < 0, 0, valor)})
            Next

            Return pred.OrderBy(Function(p) p.Mes).ToList()
        End Using
    End Function

    ' ============================================================
    ' Consultas específicas de UI
    ' ============================================================

    ''' <summary>
    ''' Este método ahora es mucho más rápido porque depende del GetAllConDetallesAsync refactorizado.
    ''' </summary>
    Public Async Function GetVigentesHoyAsync(
        Optional filtroNombre As String = "",
        Optional tiposLicenciaIds As List(Of Integer) = Nothing,
        Optional soloActivos As Boolean? = True,
        Optional fechaReferencia As Date? = Nothing
    ) As Task(Of List(Of LicenciaConFuncionarioExtendidoDto))
        Dim fechaConsulta As Date = If(fechaReferencia.HasValue, fechaReferencia.Value.Date, Date.Today)

        Using uow As New UnitOfWork()
            Dim query = uow.Repository(Of HistoricoLicencia)().GetAll().AsNoTracking().Where(Function(l) l.inicio <= fechaConsulta AndAlso l.finaliza >= fechaConsulta)

            If Not String.IsNullOrWhiteSpace(filtroNombre) Then
                query = query.Where(Function(h) h.Funcionario.Nombre.Contains(filtroNombre) OrElse h.Funcionario.CI.Contains(filtroNombre))
            End If

            If tiposLicenciaIds IsNot Nothing AndAlso tiposLicenciaIds.Any() Then
                query = query.Where(Function(h) tiposLicenciaIds.Contains(h.TipoLicenciaId))
            End If

            'If soloActivos.HasValue AndAlso soloActivos.Value Then
            '    query = query.Where(Function(h) h.Funcionario.Activo)
            'End If

            Return Await query.Select(Function(h) New LicenciaConFuncionarioExtendidoDto With {
            .LicenciaId = h.Id,
            .FuncionarioId = h.FuncionarioId,
            .TipoLicenciaId = h.TipoLicenciaId,
            .TipoLicencia = h.TipoLicencia.Nombre,
            .FechaInicio = h.inicio,
            .FechaFin = h.finaliza,
            .EstadoLicencia = h.estado,
            .Observaciones = h.Comentario,
            .CI = h.Funcionario.CI,
            .NombreFuncionario = h.Funcionario.Nombre,
            .Activo = h.Funcionario.Activo,
            .EstadoActual = If(h.Funcionario.Activo, "Activo", "Inactivo"),
            .Cargo = If(h.Funcionario.Cargo IsNot Nothing, h.Funcionario.Cargo.Nombre, "N/A"),
            .Escalafon = If(h.Funcionario.Escalafon IsNot Nothing, h.Funcionario.Escalafon.Nombre, "N/A"),
            .Seccion = If(h.Funcionario.Seccion IsNot Nothing, h.Funcionario.Seccion.Nombre, "N/A")
        }).OrderByDescending(Function(r) r.FechaInicio).ToListAsync()
        End Using
    End Function


    ''' <summary>
    ''' Versión optimizada de la respuesta anterior.
    ''' </summary>
    Public Async Function GetSancionesAsync(
        Optional filtro As String = "",
        Optional tipoLicenciaId As Integer? = Nothing
    ) As Task(Of List(Of LicenciaConFuncionarioExtendidoDto))
        Using uow As New UnitOfWork()
            Dim query = uow.Repository(Of HistoricoLicencia)().GetAll().AsNoTracking()

            If tipoLicenciaId.HasValue AndAlso tipoLicenciaId.Value > 0 Then
                query = query.Where(Function(h) h.TipoLicenciaId = tipoLicenciaId.Value)
            Else
                Dim idsCategoriasSancion As Integer() = {
                    ModConstantesApex.CategoriaAusenciaId.SancionLeve,
                    ModConstantesApex.CategoriaAusenciaId.SancionGrave
                }
                query = query.Where(Function(h) idsCategoriasSancion.Contains(h.TipoLicencia.CategoriaAusenciaId))
            End If

            If Not String.IsNullOrWhiteSpace(filtro) Then
                query = query.Where(Function(h) h.Funcionario.Nombre.Contains(filtro) OrElse h.Funcionario.CI.Contains(filtro))
            End If

            Dim resultado = query.Select(Function(h) New LicenciaConFuncionarioExtendidoDto With {
                .LicenciaId = h.Id,
                .FuncionarioId = h.FuncionarioId,
                .TipoLicenciaId = h.TipoLicenciaId,
                .TipoLicencia = h.TipoLicencia.Nombre,
                .FechaInicio = h.inicio,
                .FechaFin = h.finaliza,
                .EstadoLicencia = h.estado,
                .Observaciones = h.Comentario,
                .CI = h.Funcionario.CI,
                .NombreFuncionario = h.Funcionario.Nombre,
                .Activo = h.Funcionario.Activo,
                .EstadoActual = If(h.Funcionario.Activo, "Activo", "Inactivo"),
                .Cargo = If(h.Funcionario.Cargo IsNot Nothing, h.Funcionario.Cargo.Nombre, "N/A"),
                .Escalafon = If(h.Funcionario.Escalafon IsNot Nothing, h.Funcionario.Escalafon.Nombre, "N/A"),
                .Seccion = If(h.Funcionario.Seccion IsNot Nothing, h.Funcionario.Seccion.Nombre, "N/A")
            })

            Return Await resultado.OrderByDescending(Function(r) r.FechaInicio).ToListAsync()
        End Using
    End Function

    ''' <summary>
    ''' REFACTORIZADO: Este método ahora llama al procedimiento almacenado para un rendimiento óptimo.
    ''' Los filtros adicionales (nombre, tipo, etc.) se aplican en memoria sobre el conjunto de datos ya reducido.
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(
    Optional filtroNombre As String = "",
    Optional fechaDesde As Date? = Nothing,
    Optional fechaHasta As Date? = Nothing,
    Optional tiposLicenciaIds As List(Of Integer) = Nothing,
    Optional soloActivos As Boolean? = Nothing
) As Task(Of List(Of LicenciaConFuncionarioExtendidoDto))

        Using uow As New UnitOfWork()
            ' Si no se proveen fechas, usamos un rango por defecto amplio.
            Dim fechaInicio As Date = If(fechaDesde.HasValue, fechaDesde.Value, New Date(1900, 1, 1))
            Dim fechaFinal As Date = If(fechaHasta.HasValue, fechaHasta.Value, New Date(2100, 1, 1))

            ' 1. Preparamos los parámetros para el procedimiento almacenado
            Dim parameters As New List(Of SqlParameter) From {
    New SqlParameter("@FechaInicio", SqlDbType.Date) With {.Value = fechaInicio},
    New SqlParameter("@FechaFin", SqlDbType.Date) With {.Value = fechaFinal},
    New SqlParameter("@FiltroNombre", SqlDbType.NVarChar) With {
        .Value = If(Not String.IsNullOrWhiteSpace(filtroNombre), filtroNombre, CType(DBNull.Value, Object))
    },
    New SqlParameter("@TiposLicenciaIds", SqlDbType.VarChar, -1) With {
        .Value = If(tiposLicenciaIds IsNot Nothing AndAlso tiposLicenciaIds.Any(), String.Join(",", tiposLicenciaIds), CType(DBNull.Value, Object))
    },
    New SqlParameter("@SoloActivos", SqlDbType.Bit) With {
        .Value = If(soloActivos.HasValue, CType(soloActivos.Value, Object), CType(DBNull.Value, Object))
    }
}

            ' 2. Llamada ÚNICA y OPTIMIZADA a la base de datos
            ' El procedimiento ahora hace todo el filtrado por nosotros.
            Dim licencias = Await uow.Context.Database.SqlQuery(Of LicenciaConFuncionarioExtendidoDto)(
            "EXEC usp_Filtros_ObtenerLicenciasPorFecha @FechaInicio, @FechaFin, @FiltroNombre, @TiposLicenciaIds, @SoloActivos",
            parameters.ToArray()
        ).ToListAsync()

            ' 3. ¡YA NO HAY FILTRADO EN MEMORIA! Devolvemos el resultado directamente.
            Return licencias
        End Using
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

    ' --- Relaciones: Nombres ---
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

    ' --- Extras UI ---
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
