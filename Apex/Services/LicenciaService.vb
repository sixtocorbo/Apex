' Apex/Services/LicenciaService.vb
Imports System.Data.Entity
Imports System.Data.SqlClient
Public Class LicenciaEstacional
    Public Property Anio As Integer
    Public Property Mes As Integer
    Public Property Cantidad As Integer
End Class
' Agrega esta clase al inicio del archivo, junto a LicenciaEstacional
Public Class LicenciaPrediccion
    Public Property Mes As Integer
    Public Property CantidadPromedio As Double
End Class
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

    ' Modifica la firma y la lógica de esta función
    Public Function ObtenerDatosEstacionalidad(ByVal tipoLicenciaIDs As List(Of Integer), ByVal aniosSeleccionados As List(Of Integer)) As List(Of LicenciaEstacional)
        Using context As New ApexEntities()
            Dim query = context.HistoricoLicencia.AsQueryable()

            ' Filtro por Tipo de Licencia
            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
            End If

            ' --- NUEVO FILTRO POR AÑO ---
            If aniosSeleccionados IsNot Nothing AndAlso aniosSeleccionados.Any() Then
                query = query.Where(Function(lic) aniosSeleccionados.Contains(lic.inicio.Year))
            End If

            Dim datos = query _
            .GroupBy(Function(lic) New With {Key .Anio = lic.inicio.Year, Key .Mes = lic.inicio.Month}) _
            .Select(Function(g) New LicenciaEstacional With {
                .Anio = g.Key.Anio,
                .Mes = g.Key.Mes,
                .Cantidad = g.Count()
            }) _
            .OrderBy(Function(r) r.Anio).ThenBy(Function(r) r.Mes) _
            .ToList()

            Return datos
        End Using
    End Function

    ' Modifica la firma y la lógica de esta otra función también
    Public Function PredecirLicenciasPorMes(ByVal tipoLicenciaIDs As List(Of Integer), ByVal aniosParaPredecir As List(Of Integer)) As List(Of LicenciaPrediccion)
        Using context As New ApexEntities()
            Dim query = context.HistoricoLicencia.AsQueryable()

            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
            End If

            ' --- NUEVO FILTRO POR AÑO PARA LA BASE DE LA PREDICCIÓN ---
            If aniosParaPredecir IsNot Nothing AndAlso aniosParaPredecir.Any() Then
                query = query.Where(Function(lic) aniosParaPredecir.Contains(lic.inicio.Year))
            End If

            Dim datos = query _
            .GroupBy(Function(lic) lic.inicio.Month) _
            .Select(Function(g) New With {
                .Mes = g.Key,
                .TotalLicencias = g.Count(),
                .AniosDistintos = g.Select(Function(x) x.inicio.Year).Distinct().Count()
            }) _
            .ToList()

            Dim prediccion = datos.Select(Function(r) New LicenciaPrediccion With {
            .Mes = r.Mes,
            .CantidadPromedio = If(r.AniosDistintos > 0, CDbl(r.TotalLicencias) / r.AniosDistintos, 0)
        }).OrderBy(Function(p) p.Mes).ToList()

            Return prediccion
        End Using
    End Function

    Public Function GetAvailableYears() As List(Of Integer)
        Using context As New ApexEntities()
            Return context.HistoricoLicencia _
            .Select(Function(lic) lic.inicio.Year) _
            .Distinct() _
            .OrderByDescending(Function(y) y) _
            .ToList()
        End Using
    End Function

    Public Async Function GetAllConDetallesAsync(
    Optional filtroNombre As String = "",
    Optional fechaDesde As Date? = Nothing,
    Optional fechaHasta As Date? = Nothing,
    Optional tiposLicenciaIds As List(Of Integer) = Nothing,
    Optional soloActivos As Boolean? = Nothing
) As Task(Of List(Of LicenciaConFuncionarioExtendidoDto))

        Dim sql As New System.Text.StringBuilder()
        sql.AppendLine("SELECT")
        sql.AppendLine("    v.FuncionarioId,")
        sql.AppendLine("    tl.Id AS TipoLicenciaId,")
        sql.AppendLine("    tl.Nombre AS TipoLicencia,")
        sql.AppendLine("    v.FechaInicio,")
        sql.AppendLine("    v.FechaFin,")
        sql.AppendLine("    v.Estado AS EstadoLicencia,")
        sql.AppendLine("    f.CI,")
        sql.AppendLine("    f.Nombre AS NombreFuncionario,")
        sql.AppendLine("    f.Activo,")
        sql.AppendLine("    f.FechaIngreso,")
        sql.AppendLine("    f.FechaNacimiento,")
        sql.AppendLine("    f.Domicilio,")
        sql.AppendLine("    f.Email,")
        sql.AppendLine("    COALESCE(c.Nombre, 'N/A')  AS Cargo,")
        sql.AppendLine("    COALESCE(tf.Nombre, 'N/A') AS TipoDeFuncionario,")
        sql.AppendLine("    COALESCE(ecl.Nombre, 'N/A') AS Escalafon,")
        sql.AppendLine("    COALESCE(fun.Nombre, 'N/A') AS Funcion,")
        sql.AppendLine("    COALESCE(est.Nombre, 'N/A') AS EstadoActual,")
        sql.AppendLine("    COALESCE(sec.Nombre, 'N/A') AS Seccion,")
        sql.AppendLine("    COALESCE(pt.Nombre, 'N/A')  AS PuestoDeTrabajo,")
        sql.AppendLine("    COALESCE(tur.Nombre, 'N/A') AS Turno,")
        sql.AppendLine("    COALESCE(sem.Nombre, 'N/A') AS Semana,")
        sql.AppendLine("    COALESCE(hor.Nombre, 'N/A') AS Horario,")
        sql.AppendLine("    COALESCE(gen.Nombre, 'N/A') AS Genero,")
        sql.AppendLine("    COALESCE(ec.Nombre, 'N/A')  AS EstadoCivil,")
        sql.AppendLine("    COALESCE(ne.Nombre, 'N/A')  AS NivelDeEstudio")
        sql.AppendLine("FROM dbo.vw_LicenciasCompletas v")
        sql.AppendLine("JOIN dbo.Funcionario f       ON f.Id = v.FuncionarioId")
        sql.AppendLine("JOIN dbo.TipoLicencia tl     ON tl.Id = v.TipoLicenciaId")
        ' --- LEFT JOIN a todas las relacionales del Funcionario ---
        sql.AppendLine("LEFT JOIN dbo.Cargo c              ON c.Id = f.CargoId")
        sql.AppendLine("LEFT JOIN dbo.TipoFuncionario tf   ON tf.Id = f.TipoFuncionarioId")
        sql.AppendLine("LEFT JOIN dbo.Escalafon ecl        ON ecl.Id = f.EscalafonId")
        sql.AppendLine("LEFT JOIN dbo.Funcion fun          ON fun.Id = f.FuncionId")
        sql.AppendLine("LEFT JOIN dbo.Estado est           ON est.Id = f.EstadoId")
        sql.AppendLine("LEFT JOIN dbo.Seccion sec          ON sec.Id = f.SeccionId")
        sql.AppendLine("LEFT JOIN dbo.PuestoTrabajo pt     ON pt.Id = f.PuestoTrabajoId")
        sql.AppendLine("LEFT JOIN dbo.Turno tur            ON tur.Id = f.TurnoId")
        sql.AppendLine("LEFT JOIN dbo.Semana sem           ON sem.Id = f.SemanaId")
        sql.AppendLine("LEFT JOIN dbo.Horario hor          ON hor.Id = f.HorarioId")
        sql.AppendLine("LEFT JOIN dbo.Genero gen           ON gen.Id = f.GeneroId")
        sql.AppendLine("LEFT JOIN dbo.EstadoCivil ec       ON ec.Id = f.EstadoCivilId")
        sql.AppendLine("LEFT JOIN dbo.NivelEstudio ne      ON ne.Id = f.NivelEstudioId")
        sql.AppendLine("WHERE 1 = 1")

        Dim parameters As New List(Of SqlClient.SqlParameter)

        ' --- Rango de fechas (coincide con tu convención: fin >= desde y inicio <= hasta) ---
        If fechaDesde.HasValue Then
            sql.AppendLine("  AND v.FechaFin >= @p0")
            parameters.Add(New SqlClient.SqlParameter("@p0", fechaDesde.Value))
        End If
        If fechaHasta.HasValue Then
            Dim pname = "@p" & parameters.Count
            sql.AppendLine($"  AND v.FechaInicio <= {pname}")
            parameters.Add(New SqlClient.SqlParameter(pname, fechaHasta.Value))
        End If

        ' --- Filtro FTS sobre Funcionario(Nombre, CI) ---
        If Not String.IsNullOrWhiteSpace(filtroNombre) Then
            Dim terminos = filtroNombre.Split({" "c}, StringSplitOptions.RemoveEmptyEntries) _
                                   .Select(Function(w) $"""{w}*""")
            Dim expresionFts = String.Join(" AND ", terminos)
            Dim pname = "@p" & parameters.Count
            sql.AppendLine($"  AND CONTAINS((f.Nombre, f.CI), {pname})")
            parameters.Add(New SqlClient.SqlParameter(pname, expresionFts))
        End If

        ' --- Filtro por tipos de licencia (si se suministra lista) ---
        If tiposLicenciaIds IsNot Nothing AndAlso tiposLicenciaIds.Count > 0 Then
            Dim marcadores As New List(Of String)
            For i = 0 To tiposLicenciaIds.Count - 1
                Dim pname = "@p" & parameters.Count
                marcadores.Add(pname)
                parameters.Add(New SqlClient.SqlParameter(pname, tiposLicenciaIds(i)))
            Next
            sql.AppendLine("  AND tl.Id IN (" & String.Join(",", marcadores) & ")")
        End If

        ' --- Filtro por activos/inactivos (opcional) ---
        If soloActivos.HasValue Then
            If soloActivos.Value Then
                sql.AppendLine("  AND f.Activo = 1")
            Else
                sql.AppendLine("  AND f.Activo = 0")
            End If
        End If

        sql.AppendLine("ORDER BY v.FechaInicio DESC")

        Dim query = _unitOfWork.Context.Database _
        .SqlQuery(Of LicenciaConFuncionarioExtendidoDto)(sql.ToString(), parameters.ToArray())

        Return Await query.ToListAsync()
    End Function


    Public Function GetAllTiposLicencia() As List(Of TipoLicencia)
        Using context As New ApexEntities()
            Return context.TipoLicencia.OrderBy(Function(tl) tl.Nombre).ToList()
        End Using
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
    Public Function PredecirLicenciasConTendencia(ByVal tipoLicenciaIDs As List(Of Integer), ByVal aniosParaPredecir As List(Of Integer)) As List(Of LicenciaPrediccion)
        Using context As New ApexEntities()
            Dim query = context.HistoricoLicencia.AsQueryable()

            If tipoLicenciaIDs IsNot Nothing AndAlso tipoLicenciaIDs.Any() Then
                query = query.Where(Function(lic) tipoLicenciaIDs.Contains(lic.TipoLicenciaId))
            End If

            If aniosParaPredecir IsNot Nothing AndAlso aniosParaPredecir.Any() Then
                query = query.Where(Function(lic) aniosParaPredecir.Contains(lic.inicio.Year))
            End If

            ' 1. Obtener los datos históricos agrupados
            Dim datosHistoricos = query _
                .GroupBy(Function(lic) New With {Key .Anio = lic.inicio.Year, Key .Mes = lic.inicio.Month}) _
                .Select(Function(g) New With {
                    .Anio = g.Key.Anio,
                    .Mes = g.Key.Mes,
                    .Cantidad = g.Count()
                }) _
                .OrderBy(Function(r) r.Anio).ThenBy(Function(r) r.Mes) _
                .ToList()

            If datosHistoricos.Count < 2 Then Return New List(Of LicenciaPrediccion)() ' Necesitamos al menos 2 puntos para una tendencia

            ' 2. Calcular la regresión lineal (y = mx + b)
            Dim n = datosHistoricos.Count
            Dim minAnio = datosHistoricos.Min(Function(d) d.Anio)
            Dim sumX As Double = 0, sumY As Double = 0, sumXY As Double = 0, sumX2 As Double = 0

            For Each punto In datosHistoricos
                ' "x" es el número de meses desde el inicio
                Dim x = (punto.Anio - minAnio) * 12 + punto.Mes
                Dim y = punto.Cantidad
                sumX += x
                sumY += y
                sumXY += x * y
                sumX2 += x * x
            Next

            Dim m = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX) ' Pendiente (tendencia)
            Dim b = (sumY - m * sumX) / n ' Intercepto

            ' 3. Calcular el componente estacional
            Dim desviacionesPorMes As New Dictionary(Of Integer, List(Of Double))
            For i = 1 To 12
                desviacionesPorMes.Add(i, New List(Of Double))
            Next

            For Each punto In datosHistoricos
                Dim x = (punto.Anio - minAnio) * 12 + punto.Mes
                Dim valorTendencia = m * x + b
                Dim desviacion = punto.Cantidad - valorTendencia
                desviacionesPorMes(punto.Mes).Add(desviacion)
            Next

            Dim componenteEstacional As New Dictionary(Of Integer, Double)
            For i = 1 To 12
                componenteEstacional.Add(i, If(desviacionesPorMes(i).Any(), desviacionesPorMes(i).Average(), 0))
            Next

            ' 4. Predecir los próximos 12 meses
            Dim prediccion As New List(Of LicenciaPrediccion)
            Dim ultimoAnio = datosHistoricos.Max(Function(d) d.Anio)
            Dim ultimoMes = datosHistoricos.Where(Function(d) d.Anio = ultimoAnio).Max(Function(d) d.Mes)

            Dim xBase = (ultimoAnio - minAnio) * 12 + ultimoMes

            For i = 1 To 12
                Dim xFuturo = xBase + i
                Dim valorTendenciaFuturo = m * xFuturo + b
                Dim mesFuturo = (ultimoMes + i - 1) Mod 12 + 1
                Dim valorPredicho = valorTendenciaFuturo + componenteEstacional(mesFuturo)

                prediccion.Add(New LicenciaPrediccion With {
                    .Mes = mesFuturo,
                    .CantidadPromedio = If(valorPredicho < 0, 0, valorPredicho) ' Evitar predicciones negativas
                })
            Next

            Return prediccion.OrderBy(Function(p) p.Mes).ToList()
        End Using
    End Function

End Class
' Ponelo en el mismo archivo (arriba) o en un DTOs.vb
Public Class LicenciaConFuncionarioExtendidoDto
    ' --- Licencia ---
    Public Property LicenciaId As Integer?            ' si la vista expone Id; sino dejalo en Nothing
    Public Property FuncionarioId As Integer
    Public Property TipoLicenciaId As Integer?
    Public Property TipoLicencia As String
    Public Property FechaInicio As Date
    Public Property FechaFin As Date
    Public Property EstadoLicencia As String
    Public Property Observaciones As String           ' opcional según tu vista/tabla

    ' --- Funcionario (base) ---
    Public Property CI As String
    Public Property NombreFuncionario As String
    Public Property Activo As Boolean
    Public Property FechaIngreso As Date?
    Public Property FechaNacimiento As Date?
    Public Property Domicilio As String
    Public Property Email As String

    ' --- Relaciones: IDs + Nombres ---
    Public Property CargoId As Integer?
    Public Property Cargo As String

    Public Property TipoFuncionarioId As Integer?
    Public Property TipoDeFuncionario As String

    Public Property EscalafonId As Integer?
    Public Property Escalafon As String

    Public Property FuncionId As Integer?
    Public Property Funcion As String

    Public Property EstadoId As Integer?
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

    ' --- Extras útiles para filtros/ordenación ---
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

