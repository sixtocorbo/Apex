' Apex/Services/ConsultasGenericas.vb
Imports System.Data
Imports System.Data.Entity
Imports System.Linq
Imports System.Linq.Expressions
Imports System.Threading.Tasks
Imports Apex.Data ' Importación añadida para asegurar la visibilidad de todas las entidades

Public Module ConsultasGenericas

    Public Enum TipoOrigenDatos
        Funcionarios
        Licencias
        Notificaciones
        Novedades
        EstadosTransitorios
        Auditoria
    End Enum

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

                    ' Lógica para EstadosTransitorios (sin cambios)
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

                    Dim retenes = uow.Repository(Of RetenDetalle)().GetAll().
                        Include("EstadoTransitorio.Funcionario").
                        Where(Function(det) det.FechaReten <= fechaFin.Value AndAlso det.FechaReten >= fechaInicio.Value).
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
                    Dim notificacionService = New NotificacionService(uow)
                    Dim notificaciones = Await notificacionService.GetAllConDetallesAsync()
                    dt = notificaciones.Where(Function(n) n.FechaProgramada.Date >= fechaInicio AndAlso n.FechaProgramada.Date <= fechaFin).ToList().ToDataTable()

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

                    Dim queryEnriquecida = From aud In queryAuditoria
                                           Group Join func In uow.Context.Set(Of Funcionario)()
                                               On aud.RegistroId Equals func.Id.ToString() Into gjFunc = Group
                                           From f In gjFunc.DefaultIfEmpty()
                                           Group Join et In uow.Context.Set(Of EstadoTransitorio)().Include("Funcionario")
                                               On aud.RegistroId Equals et.Id.ToString() Into gjEt = Group
                                           From estadoT In gjEt.DefaultIfEmpty()
                                           Group Join lic In uow.Context.Set(Of Licencia)().Include("Funcionario")
                                               On aud.RegistroId Equals lic.Id.ToString() Into gjLic = Group
                                           From licencia In gjLic.DefaultIfEmpty()
                                           Select New With {
                                               aud.Id, aud.FechaHora, aud.UsuarioAccion, aud.TablaNombre, aud.CampoNombre, aud.ValorAnterior, aud.ValorNuevo,
                                               .Cedula = If(f IsNot Nothing, f.CI,
                                                         If(estadoT IsNot Nothing AndAlso estadoT.Funcionario IsNot Nothing, estadoT.Funcionario.CI,
                                                         If(licencia IsNot Nothing AndAlso licencia.Funcionario IsNot Nothing, licencia.Funcionario.CI, Nothing))),
                                               .NombreCompleto = If(f IsNot Nothing, f.Nombre,
                                                                If(estadoT IsNot Nothing AndAlso estadoT.Funcionario IsNot Nothing, estadoT.Funcionario.Nombre,
                                                                If(licencia IsNot Nothing AndAlso licencia.Funcionario IsNot Nothing, licencia.Funcionario.Nombre, Nothing)))
                                           }

                    If fechaInicio.HasValue AndAlso fechaFin.HasValue Then
                        queryEnriquecida = queryEnriquecida.Where(Function(a) a.FechaHora >= fechaInicio.Value AndAlso a.FechaHora <= fechaFin.Value)
                    End If

                    Dim resultados = Await queryEnriquecida.OrderByDescending(Function(a) a.FechaHora).ToListAsync()
                    dt = resultados.ToDataTable()

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
                ' --- LÍNEA CORREGIDA ---
                ' Se obtiene el tipo subyacente si es Nullable, o el tipo original si no lo es.
                Dim underlyingType = Nullable.GetUnderlyingType(propertyType)
                Dim targetType = If(underlyingType IsNot Nothing, underlyingType, propertyType)
                ' -----------------------
                convertedValue = Convert.ChangeType(valor, targetType)
            Catch ex As Exception
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

End Module