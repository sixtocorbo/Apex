' Apex/Services/ConsultasGenericas.vb
Imports System.Data.Entity
Imports System.Linq.Expressions
Public Module ConsultasGenericas



    Public Async Function ObtenerDatosGenericosAsync(
        tipo As TipoOrigenDatos,
        Optional fechaInicio As Date? = Nothing,
        Optional fechaFin As Date? = Nothing,
        Optional filtros As List(Of Tuple(Of String, String, String)) = Nothing
    ) As Task(Of DataTable)
        Using uow As New UnitOfWork()
            Dim dt As New DataTable()

            Select Case tipo
                Case TipoOrigenDatos.EstadosTransitorios
                    If Not fechaInicio.HasValue OrElse Not fechaFin.HasValue Then
                        Throw New ArgumentNullException("Las fechas de inicio y fin son requeridas para EstadosTransitorios.")
                    End If

                    ' Lógica para EstadosTransitorios (sin cambios, ya era correcta)
                    Dim sanciones = uow.Repository(Of SancionDetalle)().GetAll().
                        Include("EstadoTransitorio.Funcionario").
                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
                        Select(Function(det) New With {
                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
                            .Tipo = "Sanción",
                            .FechaInicio = det.FechaDesde,
                            .FechaFin = CType(det.FechaHasta, DateTime?),
                            .Observaciones = det.Observaciones
                        })

                    Dim designaciones = uow.Repository(Of DesignacionDetalle)().GetAll().
                        Include("EstadoTransitorio.Funcionario").
                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
                        Select(Function(det) New With {
                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
                            .Tipo = "Designación",
                            .FechaInicio = det.FechaDesde,
                            .FechaFin = det.FechaHasta,
                            .Observaciones = det.Observaciones
                        })

                    Dim sumarios = uow.Repository(Of SumarioDetalle)().GetAll().
                        Include("EstadoTransitorio.Funcionario").
                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
                        Select(Function(det) New With {
                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
                            .Tipo = "Sumario",
                            .FechaInicio = det.FechaDesde,
                            .FechaFin = det.FechaHasta,
                            .Observaciones = det.Observaciones
                        })

                    Dim ordenCinco = uow.Repository(Of OrdenCincoDetalle)().GetAll().
                        Include("EstadoTransitorio.Funcionario").
                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
                        Select(Function(det) New With {
                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
                            .Tipo = "Orden Cinco",
                            .FechaInicio = det.FechaDesde,
                            .FechaFin = CType(det.FechaHasta, DateTime?),
                            .Observaciones = det.Observaciones
                        })
                    ' Se suma un día a la fecha 'fin' para que la consulta incluya todo el día.
                    Dim fechaFinInclusive As Date = fechaFin.Value.AddDays(1)

                    Dim retenes = uow.Repository(Of RetenDetalle)().GetAll().
                        Include("EstadoTransitorio.Funcionario").
                        Where(Function(det) det.FechaReten >= fechaInicio.Value AndAlso det.FechaReten < fechaFinInclusive).
                        Select(Function(det) New With {
                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
                            .Tipo = "Retén",
                            .FechaInicio = det.FechaReten,
                            .FechaFin = CType(det.FechaReten, DateTime?),
                            .Observaciones = det.Observaciones
                        })

                    Dim enfermedades = uow.Repository(Of EnfermedadDetalle)().GetAll().
                        Include("EstadoTransitorio.Funcionario").
                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
                        Select(Function(det) New With {
                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
                            .Tipo = "Enfermedad",
                            .FechaInicio = det.FechaDesde,
                            .FechaFin = CType(det.FechaHasta, DateTime?),
                            .Observaciones = det.Observaciones
                        })

                    Dim resultadoUnion = Await sanciones.Union(designaciones).
                                                      Union(sumarios).
                                                      Union(ordenCinco).
                                                      Union(retenes).
                                                      Union(enfermedades).ToListAsync()
                    dt = resultadoUnion.ToDataTable()

                Case TipoOrigenDatos.Notificaciones
                    If Not fechaInicio.HasValue OrElse Not fechaFin.HasValue Then
                        Throw New ArgumentNullException("Las fechas de inicio y fin son requeridas para Notificaciones.")
                    End If
                    Dim notificacionService = New NotificacionService(uow)
                    Dim notificaciones = Await notificacionService.GetAllConDetallesAsync()
                    dt = notificaciones.Where(Function(n) n.FechaProgramada.Date >= fechaInicio.Value AndAlso n.FechaProgramada.Date <= fechaFin.Value).ToList().ToDataTable()
                Case TipoOrigenDatos.Licencias
                    Dim licenciaService = New LicenciaService(uow)
                    Dim licencias = Await licenciaService.GetAllConDetallesAsync(fechaDesde:=fechaInicio, fechaHasta:=fechaFin)

                    ' Si no hay licencias, devolvemos una tabla vacía.
                    If Not licencias.Any() Then
                        Return New DataTable()
                    End If

                    ' --- INICIO DE LA MODIFICACIÓN ---

                    ' 1. Obtenemos una lista de todos los IDs de funcionario únicos de las licencias.
                    Dim funcionarioIds = licencias.Select(Function(l) l.FuncionarioId).Distinct().ToList()

                    ' 2. Hacemos UNA SOLA consulta a la base de datos para traer todos los funcionarios necesarios.
                    Dim funcionarios = Await uow.Repository(Of Funcionario)().
                        GetAllByPredicateAsync(Function(f) funcionarioIds.Contains(f.Id))

                    ' 3. Creamos un diccionario para buscar funcionarios por su ID de forma ultra-rápida.
                    Dim funcionariosMap = funcionarios.ToDictionary(Function(f) f.Id)

                    ' 4. Proyectamos los datos de las licencias a un nuevo objeto que SÍ incluye Nombre y Cédula.
                    Dim resultadoEnriquecido = licencias.Select(Function(lic)
                                                                    Dim func As Funcionario = Nothing
                                                                    ' Buscamos el funcionario en nuestro mapa.
                                                                    funcionariosMap.TryGetValue(lic.FuncionarioId, func)

                                                                    Return New With {
                                                        .NombreCompleto = func.Nombre,
                                                        .Cedula = func.CI,
                                                        .TipoLicencia = lic.TipoLicencia,
                                                        .FechaInicio = lic.FechaInicio,
                                                        .FechaFin = lic.FechaFin,
                                                        .Dias = lic.DuracionDias,
                                                        .Observaciones = lic.Observaciones,
                                                        .Activo = func.Activo,
                                                        .TipoDeFuncionario = func.TipoFuncionario,
                                                        .Cargo = func.Cargo,
                                                        .Seccion = func.Seccion,
                                                        .Escalafon = func.Escalafon
                                                    }
                                                                End Function).ToList()

                    ' 5. Convertimos la lista ENRIQUECIDA a un DataTable.
                    dt = resultadoEnriquecido.ToDataTable()

    ' --- FIN DE LA MODIFICACIÓN ---
                'Case TipoOrigenDatos.Licencias
                '    Dim licenciaService = New LicenciaService(uow)
                '    Dim licencias = Await licenciaService.GetAllConDetallesAsync(fechaDesde:=fechaInicio, fechaHasta:=fechaFin)
                '    dt = licencias.ToDataTable()

                Case TipoOrigenDatos.Novedades
                    Dim novedadService = New NovedadService(uow)
                    Dim novedades = Await novedadService.GetAllAgrupadasAsync(fechaInicio, fechaFin)
                    dt = novedades.ToDataTable()

                Case TipoOrigenDatos.Funcionarios
                    Dim funcionarioService = New FuncionarioService(uow)
                    Dim funcionarios = Await funcionarioService.GetFuncionariosParaVistaAsync()
                    dt = funcionarios.ToDataTable()

                Case TipoOrigenDatos.Auditoria
                    Dim queryAuditoria = uow.Context.Set(Of AuditoriaCambios)().AsQueryable()

                    If filtros IsNot Nothing AndAlso filtros.Any() Then
                        queryAuditoria = ApplyAdvancedFilters(queryAuditoria, filtros)
                    End If

                    ' --- INICIO DE LA CORRECCIÓN ---
                    If fechaInicio.HasValue AndAlso fechaFin.HasValue Then
                        ' Se suma un día a la fecha 'fin' para que la consulta incluya todo el día.
                        Dim fechaFinInclusive As Date = fechaFin.Value.AddDays(1)
                        ' Se usa '<' en lugar de '<=' para la fecha final.
                        queryAuditoria = queryAuditoria.Where(Function(a) a.FechaHora >= fechaInicio.Value AndAlso a.FechaHora < fechaFinInclusive)
                    End If
                    ' --- FIN DE LA CORRECCIÓN ---

                    ' Subconsulta para cambios en la tabla 'Funcionario'
                    Dim auditFuncionarios = From aud In queryAuditoria
                                            Where aud.TablaNombre = "Funcionario"
                                            Join f In uow.Context.Set(Of Funcionario)() On aud.RegistroId Equals f.Id.ToString()
                                            Select New AuditoriaEnriquecida With {
                                                .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
                                                .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
                                                .Cedula = f.CI, .NombreCompleto = f.Nombre
                                            }

                    ' Subconsulta para cambios en la tabla 'EstadoTransitorio'
                    Dim auditEstados = From aud In queryAuditoria
                                       Where aud.TablaNombre = "EstadoTransitorio"
                                       Join et In uow.Context.Set(Of EstadoTransitorio)().Include("Funcionario") On aud.RegistroId Equals et.Id.ToString()
                                       Select New AuditoriaEnriquecida With {
                                           .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
                                           .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
                                           .Cedula = If(et.Funcionario IsNot Nothing, et.Funcionario.CI, Nothing),
                                           .NombreCompleto = If(et.Funcionario IsNot Nothing, et.Funcionario.Nombre, Nothing)
                                       }

                    ' Subconsulta para cambios en la tabla 'HistoricoLicencia'
                    Dim auditLicencias = From aud In queryAuditoria
                                         Where aud.TablaNombre = "HistoricoLicencia"
                                         Join lic In uow.Context.Set(Of HistoricoLicencia)().Include("Funcionario") On aud.RegistroId Equals lic.Id.ToString()
                                         Select New AuditoriaEnriquecida With {
                                             .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
                                             .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
                                             .Cedula = If(lic.Funcionario IsNot Nothing, lic.Funcionario.CI, Nothing),
                                             .NombreCompleto = If(lic.Funcionario IsNot Nothing, lic.Funcionario.Nombre, Nothing)
                                         }

                    ' Subconsulta para el resto de las tablas de auditoría
                    Dim tablasConJoin = {"Funcionario", "EstadoTransitorio", "HistoricoLicencia"}
                    Dim auditOtros = From aud In queryAuditoria
                                     Where Not tablasConJoin.Contains(aud.TablaNombre)
                                     Select New AuditoriaEnriquecida With {
                                         .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
                                         .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
                                         .Cedula = Nothing, .NombreCompleto = Nothing
                                     }

                    ' Unir todas las consultas de auditoría en una sola
                    Dim queryFinal = auditFuncionarios.Union(auditEstados).Union(auditLicencias).Union(auditOtros)

                    Dim resultados = Await queryFinal.OrderByDescending(Function(a) a.FechaHora).ToListAsync()
                    dt = resultados.ToDataTable()
                Case TipoOrigenDatos.HistoricoConceptos
                    ' Leer filtros si existen (TipoHistorico, FuncionarioId, etc.)
                    Dim tipoHist As TipoHistorico? = Nothing
                    Dim idPolicia As Integer? = Nothing

                    If filtros IsNot Nothing Then
                        ' TipoHistorico: puede ser "Nocturnidad", "Presentismo", "Viaticos" o "Custodias".
                        Dim ft = filtros.FirstOrDefault(Function(t) t.Item1.Equals("TipoHistorico", StringComparison.OrdinalIgnoreCase))
                        If ft IsNot Nothing Then
                            Dim aux As TipoHistorico
                            If [Enum].TryParse(ft.Item3, True, aux) Then tipoHist = aux
                        End If

                        ' Id del funcionario (puede llamarse "FuncionarioId" o "IdPolicia" en tus filtros)
                        Dim fp = filtros.FirstOrDefault(Function(t) t.Item1.Equals("FuncionarioId", StringComparison.OrdinalIgnoreCase) _
                                     OrElse t.Item1.Equals("IdPolicia", StringComparison.OrdinalIgnoreCase))
                        If fp IsNot Nothing Then
                            Dim val As Integer
                            If Integer.TryParse(fp.Item3, val) Then idPolicia = val
                        End If
                    End If

                    ' No pases anio/mes a menos que realmente quieras filtrarlos.
                    ' El método unificado se encarga de unir las tablas históricas con la tabla Funcionario.
                    dt = Await ConsultasGenericas.ObtenerHistoricoDataTableAsync(
            uow,
            tipoHist,
            idPolicia,
            Nothing,  ' año
            Nothing)  ' mes

                Case Else
                    Throw New NotImplementedException($"La consulta para el tipo '{tipo.ToString()}' no está implementada.")
            End Select

            Return dt
        End Using
    End Function

    Private Function ApplyAdvancedFilters(Of T)(source As IQueryable(Of T), filtros As List(Of Tuple(Of String, String, String))) As IQueryable(Of T)
        If filtros Is Nothing OrElse Not filtros.Any() Then
            Return source
        End If

        Dim parameter = Expression.Parameter(GetType(T), "entity")
        Dim body As Expression = Nothing

        For Each filtro In filtros
            Dim propertyName = filtro.Item1
            Dim operador = filtro.Item2
            Dim valor = filtro.Item3

            Dim propertyInfo = GetType(T).GetProperty(propertyName)
            If propertyInfo Is Nothing Then
                Continue For
            End If

            Dim propertyExpr = Expression.Property(parameter, propertyName)
            Dim propertyType = propertyInfo.PropertyType

            Dim convertedValue As Object
            Try
                Dim underlyingType = Nullable.GetUnderlyingType(propertyType)
                Dim targetType = If(underlyingType IsNot Nothing, underlyingType, propertyType)
                convertedValue = Convert.ChangeType(valor, targetType)
            Catch ex As Exception
                ' System.Diagnostics.Debug.WriteLine($"Error al convertir valor para filtro: {ex.Message}")
                Continue For
            End Try

            Dim valueExpr = Expression.Constant(convertedValue, propertyType)
            Dim condition As Expression = Nothing

            Select Case operador.ToLower()
                Case "contiene"
                    If propertyType Is GetType(String) Then
                        Dim method = GetType(String).GetMethod("Contains", New Type() {GetType(String)})
                        condition = Expression.Call(propertyExpr, method, Expression.Constant(valor, GetType(String)))
                    End If
                Case "es igual a"
                    condition = Expression.Equal(propertyExpr, valueExpr)
                Case "mayor o igual que"
                    condition = Expression.GreaterThanOrEqual(propertyExpr, valueExpr)
                Case "menor o igual que"
                    condition = Expression.LessThanOrEqual(propertyExpr, valueExpr)
            End Select

            If condition IsNot Nothing Then
                body = If(body Is Nothing, condition, Expression.AndAlso(body, condition))
            End If
        Next

        If body IsNot Nothing Then
            Dim lambda = Expression.Lambda(Of Func(Of T, Boolean))(body, parameter)
            Return source.Where(lambda)
        End If

        Return source
    End Function

    ' Clase auxiliar para unificar los resultados de las consultas de auditoría
    Private Class AuditoriaEnriquecida
        Public Property Id As Integer
        Public Property FechaHora As DateTime
        Public Property UsuarioAccion As String
        Public Property TablaNombre As String
        Public Property CampoNombre As String
        Public Property ValorAnterior As String
        Public Property ValorNuevo As String
        Public Property Cedula As String
        Public Property NombreCompleto As String
    End Class

    Public Async Function ObtenerHistoricoDataTableAsync(
    uow As IUnitOfWork,
    Optional tipo As TipoHistorico? = Nothing,
    Optional idPolicia As Integer? = Nothing,
    Optional anio As Integer? = Nothing,
    Optional mes As Integer? = Nothing
) As Task(Of DataTable)

        ' --- Precargar funcionarios ---
        Dim funcionarios = Await uow.Repository(Of Funcionario)() _
                                .GetAllByPredicateAsync(Function(f) True)
        Dim dicF As Dictionary(Of Integer, Funcionario) =
        funcionarios.ToDictionary(Function(f) f.Id)

        Dim datos As New List(Of Object)()

        ' Función auxiliar para obtener Cedula y Nombre
        Dim getFuncionarioInfo As Func(Of Integer, (CI As String, Nombre As String)) =
        Function(fid As Integer)
            Dim f As Funcionario = Nothing
            If dicF.TryGetValue(fid, f) Then
                Return (f.CI, f.Nombre)
            Else
                Return (Nothing, Nothing)
            End If
        End Function

        ' --- Nocturnidad ---
        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Nocturnidad Then
            Dim noctList As IEnumerable(Of HistoricoNocturnidad)
            If idPolicia.HasValue Then
                noctList = Await uow.Repository(Of HistoricoNocturnidad)().FindAsync(
                Function(h) h.FuncionarioId = idPolicia.Value)
            Else
                noctList = Await uow.Repository(Of HistoricoNocturnidad)().GetAllByPredicateAsync(
                Function(h) True)
            End If

            For Each h In noctList
                If (Not anio.HasValue OrElse h.Anio = anio.Value) AndAlso
               (Not mes.HasValue OrElse h.Mes = mes.Value) Then
                    Dim info = getFuncionarioInfo(h.FuncionarioId)
                    datos.Add(New With {
                    .FuncionarioId = h.FuncionarioId,
                    .Cedula = info.CI,
                    .NombreCompleto = info.Nombre,
                    .Tipo = "Nocturnidad",
                    .Anio = CInt(h.Anio),
                    .Mes = CInt(h.Mes),
                    .Fecha = CType(Nothing, Date?),
                    .Minutos = If(h.Minutos.HasValue, CType(h.Minutos.Value, Integer?), CType(Nothing, Integer?)),
                    .Dias = CType(Nothing, Integer?),
                    .Incidencia = CType(Nothing, String),
                    .Observaciones = CType(Nothing, String),
                    .Motivo = CType(Nothing, String),
                    .Area = CType(Nothing, String)
                })
                End If
            Next
        End If

        ' --- Presentismo ---
        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Presentismo Then
            Dim presList As IEnumerable(Of HistoricoPresentismo)
            If idPolicia.HasValue Then
                presList = Await uow.Repository(Of HistoricoPresentismo)().FindAsync(
                Function(h) h.FuncionarioId = idPolicia.Value)
            Else
                presList = Await uow.Repository(Of HistoricoPresentismo)().GetAllByPredicateAsync(
                Function(h) True)
            End If

            For Each h In presList
                If (Not anio.HasValue OrElse h.Anio = anio.Value) AndAlso
               (Not mes.HasValue OrElse h.Mes = mes.Value) Then
                    Dim info = getFuncionarioInfo(h.FuncionarioId)
                    datos.Add(New With {
                    .FuncionarioId = h.FuncionarioId,
                    .Cedula = info.CI,
                    .NombreCompleto = info.Nombre,
                    .Tipo = "Presentismo",
                    .Anio = CInt(h.Anio),
                    .Mes = CInt(h.Mes),
                    .Fecha = CType(Nothing, Date?),
                    .Minutos = If(h.Minutos.HasValue, CType(h.Minutos.Value, Integer?), CType(Nothing, Integer?)),
                    .Dias = If(h.Dias.HasValue, CType(h.Dias.Value, Integer?), CType(Nothing, Integer?)),
                    .Incidencia = h.Incidencia,
                    .Observaciones = h.Observaciones,
                    .Motivo = CType(Nothing, String),
                    .Area = CType(Nothing, String)
                })
                End If
            Next
        End If

        ' --- Viáticos ---
        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Viaticos Then
            Dim viatList As IEnumerable(Of HistoricoViatico)
            If idPolicia.HasValue Then
                viatList = Await uow.Repository(Of HistoricoViatico)().FindAsync(
                Function(h) h.FuncionarioId = idPolicia.Value)
            Else
                viatList = Await uow.Repository(Of HistoricoViatico)().GetAllByPredicateAsync(
                Function(h) True)
            End If

            For Each h In viatList
                If (Not anio.HasValue OrElse h.Anio = anio.Value) AndAlso
               (Not mes.HasValue OrElse h.Mes = mes.Value) Then
                    Dim info = getFuncionarioInfo(h.FuncionarioId)
                    datos.Add(New With {
                    .FuncionarioId = h.FuncionarioId,
                    .Cedula = info.CI,
                    .NombreCompleto = info.Nombre,
                    .Tipo = "Viáticos",
                    .Anio = CInt(h.Anio),
                    .Mes = CInt(h.Mes),
                    .Fecha = CType(Nothing, Date?),
                    .Minutos = CType(Nothing, Integer?),
                    .Dias = CType(Nothing, Integer?),
                    .Incidencia = h.Incidencia,
                    .Observaciones = CType(Nothing, String),
                    .Motivo = h.Motivo,
                    .Area = CType(Nothing, String)
                })
                End If
            Next
        End If

        ' --- Custodias ---
        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Custodias Then
            Dim custList As IEnumerable(Of HistoricoCustodia)
            If idPolicia.HasValue Then
                custList = Await uow.Repository(Of HistoricoCustodia)().FindAsync(
                Function(h) h.FuncionarioId = idPolicia.Value)
            Else
                custList = Await uow.Repository(Of HistoricoCustodia)().GetAllByPredicateAsync(
                Function(h) True)
            End If

            For Each h In custList
                Dim a As Integer = h.Fecha.Year
                Dim m As Integer = h.Fecha.Month
                If (Not anio.HasValue OrElse a = anio.Value) AndAlso
               (Not mes.HasValue OrElse m = mes.Value) Then
                    Dim info = getFuncionarioInfo(h.FuncionarioId)
                    datos.Add(New With {
                    .FuncionarioId = h.FuncionarioId,
                    .Cedula = info.CI,
                    .NombreCompleto = info.Nombre,
                    .Tipo = "Custodias",
                    .Anio = a,
                    .Mes = m,
                    .Fecha = CType(h.Fecha, Date?),
                    .Minutos = CType(Nothing, Integer?),
                    .Dias = CType(Nothing, Integer?),
                    .Incidencia = CType(Nothing, String),
                    .Observaciones = CType(Nothing, String),
                    .Motivo = CType(Nothing, String),
                    .Area = h.Area
                })
                End If
            Next
        End If

        ' Construye el DataTable con todas las columnas (FuncionarioId, Cedula, NombreCompleto, Tipo, Anio, Mes, Fecha, Minutos, Dias, Incidencia, Observaciones, Motivo, Area)
        Return ModuloExtensions.ToDataTable(datos)
    End Function
End Module