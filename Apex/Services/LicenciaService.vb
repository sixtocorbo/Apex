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
    ' Sobrecargamos la función para que acepte una lista de IDs de TipoLicencia

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