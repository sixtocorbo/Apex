' Apex/Services/ConsultasGenericas.vb
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Threading.Tasks
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
                    dt = licencias.ToDataTable()

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
                    ' El histórico de conceptos requiere conocer el policía y, opcionalmente, año/mes.
                    ' IdPolicia, Año y Mes pueden venir de los filtros o de tu interfaz (ajusta según tu implementación).
                    ' Aquí se asume que "filtros" contiene tuplas (columna, operador, valor) donde
                    ' el valor para "IdPolicia" es un entero.
                    Dim idPolicia As Integer = 0
                    Dim año As Integer? = Nothing
                    Dim mes As Integer? = Nothing

                    ' Si has pasado parámetros adicionales en "filtros", extrae el idPolicia (y opcionalmente año/mes).
                    If filtros IsNot Nothing Then
                        Dim filtroPolicia = filtros.FirstOrDefault(Function(t) t.Item1.Equals("IdPolicia", StringComparison.OrdinalIgnoreCase))
                        If filtroPolicia IsNot Nothing Then
                            Integer.TryParse(filtroPolicia.Item3, idPolicia)
                        End If
                    End If

                    ' Si deseas usar el rango de fechas del formulario como año/mes, usa fechaInicio/fechaFin:
                    If fechaInicio.HasValue Then
                        año = fechaInicio.Value.Year
                        mes = fechaInicio.Value.Month
                    End If

                    ' Llama a tu método para obtener el DataTable consolidado de histórico
                    dt = Await ConsultasGenericas.ObtenerHistoricoDataTableAsync(uow, idPolicia, año, mes)

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

    ''' <summary>
    ''' Obtiene un DataTable con el histórico de conceptos.
    ''' </summary>
    ''' <param name="uow">Unidad de trabajo (repositorio genérico).</param>
    ''' <param name="tipo">Tipo de histórico a consultar (Nocturnidad, Presentismo, Viaticos o Custodias).</param>
    ''' <param name="idPolicia">ID de funcionario para filtrar; si es Nothing, se devuelven todos.</param>
    ''' <param name="anio">Año a filtrar (opcional).</param>
    ''' <param name="mes">Mes a filtrar (opcional).</param>
    Public Async Function ObtenerHistoricoDataTableAsync(
    uow As IUnitOfWork,
    tipo As TipoHistorico,
    Optional idPolicia As Integer? = Nothing,
    Optional anio As Integer? = Nothing,
    Optional mes As Integer? = Nothing
) As Task(Of DataTable)

        Select Case tipo

        ' ----- HISTÓRICO DE NOCTURNIDAD -----
            Case TipoHistorico.Nocturnidad
                Dim lista As IEnumerable(Of HistoricoNocturnidad)

                ' Filtrado por funcionario y/o por año/mes si se indican
                If idPolicia.HasValue AndAlso anio.HasValue AndAlso mes.HasValue Then
                    lista = Await uow.Repository(Of HistoricoNocturnidad)().FindAsync(
                    Function(h) h.FuncionarioId = idPolicia.Value _
                             AndAlso h.Anio = anio.Value _
                             AndAlso h.Mes = mes.Value)
                ElseIf idPolicia.HasValue Then
                    lista = Await uow.Repository(Of HistoricoNocturnidad)().FindAsync(
                    Function(h) h.FuncionarioId = idPolicia.Value)
                ElseIf anio.HasValue AndAlso mes.HasValue Then
                    lista = Await uow.Repository(Of HistoricoNocturnidad)().FindAsync(
                    Function(h) h.Anio = anio.Value AndAlso h.Mes = mes.Value)
                Else
                    lista = Await uow.Repository(Of HistoricoNocturnidad)().GetAllByPredicateAsync(Function(h) True)
                End If

                Dim dt As DataTable = ModuloExtensions.ToDataTable(lista)
                If dt.Columns.Contains("Funcionario") Then dt.Columns.Remove("Funcionario")
                Return dt

        ' ----- HISTÓRICO DE PRESENTISMO -----
            Case TipoHistorico.Presentismo
                Dim lista As IEnumerable(Of HistoricoPresentismo)

                If idPolicia.HasValue AndAlso anio.HasValue AndAlso mes.HasValue Then
                    lista = Await uow.Repository(Of HistoricoPresentismo)().FindAsync(
                    Function(h) h.FuncionarioId = idPolicia.Value _
                             AndAlso h.Anio = anio.Value _
                             AndAlso h.Mes = mes.Value)
                ElseIf idPolicia.HasValue Then
                    lista = Await uow.Repository(Of HistoricoPresentismo)().FindAsync(
                    Function(h) h.FuncionarioId = idPolicia.Value)
                ElseIf anio.HasValue AndAlso mes.HasValue Then
                    lista = Await uow.Repository(Of HistoricoPresentismo)().FindAsync(
                    Function(h) h.Anio = anio.Value AndAlso h.Mes = mes.Value)
                Else
                    lista = Await uow.Repository(Of HistoricoPresentismo)().GetAllByPredicateAsync(Function(h) True)
                End If

                Dim dt As DataTable = ModuloExtensions.ToDataTable(lista)
                If dt.Columns.Contains("Funcionario") Then dt.Columns.Remove("Funcionario")
                Return dt

        ' ----- HISTÓRICO DE VIÁTICOS -----
            Case TipoHistorico.Viaticos
                Dim lista As IEnumerable(Of HistoricoViatico)

                If idPolicia.HasValue AndAlso anio.HasValue AndAlso mes.HasValue Then
                    lista = Await uow.Repository(Of HistoricoViatico)().FindAsync(
                    Function(h) h.FuncionarioId = idPolicia.Value _
                             AndAlso h.Anio = anio.Value _
                             AndAlso h.Mes = mes.Value)
                ElseIf idPolicia.HasValue Then
                    lista = Await uow.Repository(Of HistoricoViatico)().FindAsync(
                    Function(h) h.FuncionarioId = idPolicia.Value)
                ElseIf anio.HasValue AndAlso mes.HasValue Then
                    lista = Await uow.Repository(Of HistoricoViatico)().FindAsync(
                    Function(h) h.Anio = anio.Value AndAlso h.Mes = mes.Value)
                Else
                    lista = Await uow.Repository(Of HistoricoViatico)().GetAllByPredicateAsync(Function(h) True)
                End If

                Dim dt As DataTable = ModuloExtensions.ToDataTable(lista)
                If dt.Columns.Contains("Funcionario") Then dt.Columns.Remove("Funcionario")
                Return dt

        ' ----- HISTÓRICO DE CUSTODIAS -----
            Case TipoHistorico.Custodias
                Dim lista As IEnumerable(Of HistoricoCustodia)

                If idPolicia.HasValue AndAlso anio.HasValue AndAlso mes.HasValue Then
                    lista = Await uow.Repository(Of HistoricoCustodia)().FindAsync(
                    Function(h) h.FuncionarioId = idPolicia.Value _
                             AndAlso h.Fecha.Year = anio.Value _
                             AndAlso h.Fecha.Month = mes.Value)
                ElseIf idPolicia.HasValue Then
                    lista = Await uow.Repository(Of HistoricoCustodia)().FindAsync(
                    Function(h) h.FuncionarioId = idPolicia.Value)
                ElseIf anio.HasValue AndAlso mes.HasValue Then
                    lista = Await uow.Repository(Of HistoricoCustodia)().FindAsync(
                    Function(h) h.Fecha.Year = anio.Value AndAlso h.Fecha.Month = mes.Value)
                Else
                    lista = Await uow.Repository(Of HistoricoCustodia)().GetAllByPredicateAsync(Function(h) True)
                End If

                Dim dt As DataTable = ModuloExtensions.ToDataTable(lista)
                If dt.Columns.Contains("Funcionario") Then dt.Columns.Remove("Funcionario")
                Return dt

            Case Else
                ' Si llega un tipo no reconocido, se devuelve un DataTable vacío
                Return New DataTable()

        End Select

    End Function
End Module