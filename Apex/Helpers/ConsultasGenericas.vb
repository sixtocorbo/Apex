' Apex/Helpers/ConsultasGenericas.vb
Imports System.Data.Entity
Imports System.Collections.Generic
Imports System.Linq
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

                    If Not resultadoUnion.Any() Then
                        Return New DataTable()
                    End If

                    Dim metadatosPorFuncionario = Await CargarMetadatosFuncionariosAsync(
                        uow,
                        resultadoUnion.Select(Function(r) CType(r.FuncionarioId, Integer?))
                    )

                    Dim resultadoEnriquecido = resultadoUnion.Select(
                        Function(r)
                            Dim meta = ObtenerMetadatosFuncionario(metadatosPorFuncionario, CType(r.FuncionarioId, Integer?))

                            Return New With {
                                .FuncionarioId = r.FuncionarioId,
                                .NombreCompleto = NormalizarValorReporte(r.NombreCompleto),
                                .Cedula = NormalizarValorReporte(r.Cedula),
                                .Tipo = NormalizarValorReporte(r.Tipo),
                                .FechaInicio = r.FechaInicio,
                                .FechaFin = r.FechaFin,
                                .Observaciones = NormalizarValorReporte(r.Observaciones, String.Empty),
                                .TipoDeFuncionario = meta.TipoDeFuncionario,
                                .Cargo = meta.Cargo,
                                .Seccion = meta.Seccion,
                                .Escalafon = meta.Escalafon,
                                .SubEscalafon = meta.SubEscalafon,
                                .SubDireccion = meta.SubDireccion,
                                .Funcion = meta.Funcion,
                                .PuestoDeTrabajo = meta.PuestoDeTrabajo,
                                .Turno = meta.Turno,
                                .Semana = meta.Semana,
                                .Horario = meta.Horario,
                                .PrestadorSalud = meta.PrestadorSalud,
                                .EstadoFuncionario = meta.EstadoFuncionario,
                                .Activo = meta.Activo
                            }
                        End Function
                    ).ToList()

                    dt = resultadoEnriquecido.ToDataTable()

                Case TipoOrigenDatos.Notificaciones
                    If Not fechaInicio.HasValue OrElse Not fechaFin.HasValue Then
                        Throw New ArgumentNullException("Las fechas de inicio y fin son requeridas para Notificaciones.")
                    End If
                    Dim notificacionService = New NotificacionService(uow)
                    Dim notificaciones = Await notificacionService.GetAllConDetallesAsync(
                        fechaDesde:=fechaInicio,
                        fechaHasta:=fechaFin
                    )

                    If Not notificaciones.Any() Then
                        Return CrearTablaNotificacionesVacia()
                    End If

                    Dim metadatosPorFuncionario = Await CargarMetadatosFuncionariosAsync(uow, notificaciones.Select(Function(n) CType(n.FuncionarioId, Integer?)))

                    Dim resultadoNotificaciones = notificaciones.Select(Function(n)
                                                                            Dim meta = ObtenerMetadatosFuncionario(metadatosPorFuncionario, CType(n.FuncionarioId, Integer?))

                                                                            Return New With {
                                                                                .NombreCompleto = NormalizarValorReporte(n.NombreFuncionario),
                                                                                .Cedula = NormalizarValorReporte(n.CI),
                                                                                .TipoNotificacion = NormalizarValorReporte(n.TipoNotificacion),
                                                                                .Estado = NormalizarValorReporte(n.Estado),
                                                                                .FechaProgramada = n.FechaProgramada,
                                                                                .Texto = NormalizarValorReporte(n.Texto, String.Empty),
                                                                                .Documento = NormalizarValorReporte(n.Documento),
                                                                                .ExpMinisterial = NormalizarValorReporte(n.ExpMinisterial),
                                                                                .ExpINR = NormalizarValorReporte(n.ExpINR),
                                                                                .Oficina = NormalizarValorReporte(n.Oficina),
                                                                                .TipoDeFuncionario = meta.TipoDeFuncionario,
                                                                                .Cargo = meta.Cargo,
                                                                                .Seccion = meta.Seccion,
                                                                                .Escalafon = meta.Escalafon,
                                                                                .SubEscalafon = meta.SubEscalafon,
                                                                                .SubDireccion = meta.SubDireccion,
                                                                                .Funcion = meta.Funcion,
                                                                                .PuestoDeTrabajo = meta.PuestoDeTrabajo,
                                                                                .Turno = meta.Turno,
                                                                                .Semana = meta.Semana,
                                                                                .Horario = meta.Horario,
                                                                                .PrestadorSalud = meta.PrestadorSalud,
                                                                                .EstadoFuncionario = meta.EstadoFuncionario,
                                                                                .Activo = meta.Activo
                                                                            }
                                                                        End Function).ToList()

                    dt = resultadoNotificaciones.ToDataTable()

                Case TipoOrigenDatos.Licencias
                    Dim licenciaService = New LicenciaService(uow)
                    Dim licencias = Await licenciaService.GetAllConDetallesAsync(fechaDesde:=fechaInicio, fechaHasta:=fechaFin)

                    If licencias Is Nothing OrElse Not licencias.Any() Then
                        Return CrearTablaLicenciasVacia()
                    End If

                    Dim resultadoFinal = licencias.Select(Function(lic) New With {
                        .NombreCompleto = NormalizarValorReporte(lic.NombreFuncionario, "N/A"),
                        .Cedula = NormalizarValorReporte(lic.CI, "N/A"),
                        .TipoLicencia = NormalizarValorReporte(lic.TipoLicencia),
                        .FechaInicio = lic.FechaInicio,
                        .FechaFin = lic.FechaFin,
                        .Dias = lic.DuracionDias,
                        .Observaciones = NormalizarValorReporte(lic.Observaciones, String.Empty),
                        .Activo = lic.Activo,
                        .TipoDeFuncionario = NormalizarValorReporte(lic.TipoDeFuncionario),
                        .Cargo = NormalizarValorReporte(lic.Cargo),
                        .Seccion = NormalizarValorReporte(lic.Seccion),
                        .Escalafon = NormalizarValorReporte(lic.Escalafon)
                    }).ToList()

                    dt = resultadoFinal.ToDataTable()

                Case TipoOrigenDatos.Novedades
                    Dim novedadService = New NovedadService(uow)
                    Dim novedadesAgrupadas = Await novedadService.GetAllAgrupadasAsync(fechaInicio, fechaFin)

                    If novedadesAgrupadas Is Nothing OrElse Not novedadesAgrupadas.Any() Then
                        Return CrearTablaNovedadesVacia()
                    End If

                    Dim novedadIds = novedadesAgrupadas.Select(Function(n) n.Id).Distinct().ToList()
                    Dim novedadesDetalladas = Await novedadService.GetNovedadesCompletasByIds(novedadIds)
                    If novedadesDetalladas Is Nothing Then
                        novedadesDetalladas = New List(Of vw_NovedadesCompletas)()
                    End If
                    Dim metadatosPorFuncionario = Await CargarMetadatosFuncionariosAsync(uow, novedadesDetalladas.Select(Function(n) n.FuncionarioId))

                    Dim resumenPorNovedad = novedadesAgrupadas.ToDictionary(Function(n) n.Id,
                                                                         Function(n) New With {
                                                                            .Resumen = NormalizarValorReporte(n.Resumen, String.Empty),
                                                                            .Funcionarios = NormalizarValorReporte(n.Funcionarios, String.Empty),
                                                                            .Estado = NormalizarValorReporte(n.Estado)
                                                                         })

                    Dim funcionariosDetalladosPorNovedad = ConstruirMapaFuncionariosPorNovedad(novedadesDetalladas)

                    Dim resultadoNovedades As New List(Of Object)()

                    If novedadesDetalladas IsNot Nothing AndAlso novedadesDetalladas.Any() Then
                        For Each detalle In novedadesDetalladas
                            Dim infoResumen = Nothing
                            resumenPorNovedad.TryGetValue(detalle.Id, infoResumen)

                            Dim listaFuncionariosDetallada = ObtenerFuncionariosDetallados(funcionariosDetalladosPorNovedad, detalle.Id)
                            Dim listaFuncionarios = If(String.IsNullOrWhiteSpace(listaFuncionariosDetallada),
                                                        If(infoResumen IsNot Nothing, infoResumen.Funcionarios, NormalizarValorReporte(detalle.NombreFuncionario, String.Empty)),
                                                        listaFuncionariosDetallada)
                            Dim cantidadFuncionarios = CalcularCantidadFuncionarios(listaFuncionarios)
                            Dim meta = ObtenerMetadatosFuncionario(metadatosPorFuncionario, detalle.FuncionarioId)
                            Dim estadoDetallado = If(infoResumen IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(infoResumen.Estado),
                                                   infoResumen.Estado,
                                                   NormalizarValorReporte(detalle.Estado))
                            Dim estadoParaObservacion = If(infoResumen IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(infoResumen.Estado),
                                                            NormalizarValorReporte(infoResumen.Estado, String.Empty),
                                                            NormalizarValorReporte(detalle.Estado, String.Empty))
                            Dim nombreNormalizado = NormalizarValorReporte(detalle.NombreFuncionario, "Sin Asignar")
                            Dim cedulaNormalizada = NormalizarValorReporte(detalle.CI, String.Empty)
                            Dim observaciones = ConstruirObservacionesNovedad(
                                If(infoResumen IsNot Nothing, infoResumen.Resumen, detalle.Texto),
                                detalle.Texto,
                                estadoParaObservacion
                            )

                            resultadoNovedades.Add(New With {
                                .NovedadId = detalle.Id,
                                .Fecha = detalle.Fecha,
                                .Resumen = If(infoResumen IsNot Nothing, infoResumen.Resumen, NormalizarValorReporte(detalle.Texto, String.Empty)),
                                .Texto = NormalizarValorReporte(detalle.Texto, String.Empty),
                                .Estado = estadoDetallado,
                                .FuncionariosLista = listaFuncionarios,
                                .CantidadFuncionarios = cantidadFuncionarios,
                                .FuncionarioId = detalle.FuncionarioId,
                                .NombreCompleto = nombreNormalizado,
                                .Cedula = cedulaNormalizada,
                                .Observaciones = observaciones,
                                .TipoDeFuncionario = meta.TipoDeFuncionario,
                                .Cargo = meta.Cargo,
                                .Seccion = meta.Seccion,
                                .Escalafon = meta.Escalafon,
                                .SubEscalafon = meta.SubEscalafon,
                                .SubDireccion = meta.SubDireccion,
                                .Funcion = meta.Funcion,
                                .PuestoDeTrabajo = meta.PuestoDeTrabajo,
                                .Turno = meta.Turno,
                                .Semana = meta.Semana,
                                .Horario = meta.Horario,
                                .PrestadorSalud = meta.PrestadorSalud,
                                .EstadoFuncionario = meta.EstadoFuncionario,
                                .Activo = meta.Activo
                            })
                        Next
                    End If

                    Dim idsConDetalle = If(novedadesDetalladas IsNot Nothing, novedadesDetalladas.Select(Function(n) n.Id).Distinct().ToList(), New List(Of Integer)())
                    For Each agrupada In novedadesAgrupadas
                        If idsConDetalle.Contains(agrupada.Id) Then Continue For

                        Dim metaVacio = CrearMetadatosFuncionarioVacios()
                        Dim listaFuncionariosDetallada = ObtenerFuncionariosDetallados(funcionariosDetalladosPorNovedad, agrupada.Id)
                        Dim listaFuncionarios = If(String.IsNullOrWhiteSpace(listaFuncionariosDetallada),
                                                  NormalizarValorReporte(agrupada.Funcionarios, String.Empty),
                                                  listaFuncionariosDetallada)
                        Dim estadoAgrupado = NormalizarValorReporte(agrupada.Estado)
                        Dim estadoAgrupadoObservacion = NormalizarValorReporte(agrupada.Estado, String.Empty)
                        Dim observacionesAgrupado = ConstruirObservacionesNovedad(
                            agrupada.Resumen,
                            Nothing,
                            estadoAgrupadoObservacion
                        )
                        Dim nombreAgrupado = If(String.IsNullOrWhiteSpace(listaFuncionarios),
                                              "Sin funcionarios asignados",
                                              listaFuncionarios)

                        resultadoNovedades.Add(New With {
                            .NovedadId = agrupada.Id,
                            .Fecha = agrupada.Fecha,
                            .Resumen = NormalizarValorReporte(agrupada.Resumen, String.Empty),
                            .Texto = NormalizarValorReporte(Nothing, String.Empty),
                            .Estado = estadoAgrupado,
                            .FuncionariosLista = listaFuncionarios,
                            .CantidadFuncionarios = CalcularCantidadFuncionarios(listaFuncionarios),
                            .FuncionarioId = CType(Nothing, Integer?),
                            .NombreCompleto = nombreAgrupado,
                            .Cedula = NormalizarValorReporte(Nothing, String.Empty),
                            .Observaciones = observacionesAgrupado,
                            .TipoDeFuncionario = metaVacio.TipoDeFuncionario,
                            .Cargo = metaVacio.Cargo,
                            .Seccion = metaVacio.Seccion,
                            .Escalafon = metaVacio.Escalafon,
                            .SubEscalafon = metaVacio.SubEscalafon,
                            .SubDireccion = metaVacio.SubDireccion,
                            .Funcion = metaVacio.Funcion,
                            .PuestoDeTrabajo = metaVacio.PuestoDeTrabajo,
                            .Turno = metaVacio.Turno,
                            .Semana = metaVacio.Semana,
                            .Horario = metaVacio.Horario,
                            .PrestadorSalud = metaVacio.PrestadorSalud,
                            .EstadoFuncionario = metaVacio.EstadoFuncionario,
                            .Activo = metaVacio.Activo
                        })
                    Next

                    If resultadoNovedades.Count = 0 Then
                        Return CrearTablaNovedadesVacia()
                    End If

                    dt = resultadoNovedades.ToDataTable()

                Case TipoOrigenDatos.Funcionarios
                    Dim funcionarioService = New FuncionarioService(uow)
                    Dim funcionarios = Await funcionarioService.GetFuncionariosParaVistaAsync()
                    dt = funcionarios.ToDataTable()

                Case TipoOrigenDatos.Auditoria
                    Dim queryAuditoria = uow.Context.Set(Of AuditoriaCambios)().AsQueryable()

                    If filtros IsNot Nothing AndAlso filtros.Any() Then
                        queryAuditoria = ApplyAdvancedFilters(queryAuditoria, filtros)
                    End If

                    If fechaInicio.HasValue AndAlso fechaFin.HasValue Then
                        Dim fechaFinInclusive As Date = fechaFin.Value.AddDays(1)
                        queryAuditoria = queryAuditoria.Where(Function(a) a.FechaHora >= fechaInicio.Value AndAlso a.FechaHora < fechaFinInclusive)
                    End If

                    Dim auditFuncionarios = From aud In queryAuditoria
                                            Where aud.TablaNombre = "Funcionario"
                                            Join f In uow.Context.Set(Of Funcionario)() On aud.RegistroId Equals f.Id.ToString()
                                            Select New AuditoriaEnriquecida With {
                                               .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
                                               .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
                                               .Cedula = f.CI, .NombreCompleto = f.Nombre
                                            }

                    Dim auditEstados = From aud In queryAuditoria
                                       Where aud.TablaNombre = "EstadoTransitorio"
                                       Join et In uow.Context.Set(Of EstadoTransitorio)().Include("Funcionario") On aud.RegistroId Equals et.Id.ToString()
                                       Select New AuditoriaEnriquecida With {
                                          .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
                                          .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
                                          .Cedula = If(et.Funcionario IsNot Nothing, et.Funcionario.CI, Nothing),
                                          .NombreCompleto = If(et.Funcionario IsNot Nothing, et.Funcionario.Nombre, Nothing)
                                       }

                    Dim auditLicencias = From aud In queryAuditoria
                                         Where aud.TablaNombre = "HistoricoLicencia"
                                         Join lic In uow.Context.Set(Of HistoricoLicencia)().Include("Funcionario") On aud.RegistroId Equals lic.Id.ToString()
                                         Select New AuditoriaEnriquecida With {
                                            .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
                                            .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
                                            .Cedula = If(lic.Funcionario IsNot Nothing, lic.Funcionario.CI, Nothing),
                                            .NombreCompleto = If(lic.Funcionario IsNot Nothing, lic.Funcionario.Nombre, Nothing)
                                         }

                    Dim tablasConJoin = {"Funcionario", "EstadoTransitorio", "HistoricoLicencia"}
                    Dim auditOtros = From aud In queryAuditoria
                                     Where Not tablasConJoin.Contains(aud.TablaNombre)
                                     Select New AuditoriaEnriquecida With {
                                        .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
                                        .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
                                        .Cedula = Nothing, .NombreCompleto = Nothing
                                     }

                    Dim queryFinal = auditFuncionarios.Union(auditEstados).Union(auditLicencias).Union(auditOtros)

                    Dim resultados = Await queryFinal.OrderByDescending(Function(a) a.FechaHora).ToListAsync()
                    dt = resultados.ToDataTable()

                Case TipoOrigenDatos.HistoricoConceptos
                    Dim tipoHist As TipoHistorico? = Nothing
                    Dim idPolicia As Integer? = Nothing

                    If filtros IsNot Nothing Then
                        Dim ft = filtros.FirstOrDefault(Function(t) t.Item1.Equals("TipoHistorico", StringComparison.OrdinalIgnoreCase))
                        If ft IsNot Nothing Then
                            Dim aux As TipoHistorico
                            If [Enum].TryParse(ft.Item3, True, aux) Then tipoHist = aux
                        End If

                        Dim fp = filtros.FirstOrDefault(Function(t) t.Item1.Equals("FuncionarioId", StringComparison.OrdinalIgnoreCase) _
                                OrElse t.Item1.Equals("IdPolicia", StringComparison.OrdinalIgnoreCase))
                        If fp IsNot Nothing Then
                            Dim val As Integer
                            If Integer.TryParse(fp.Item3, val) Then idPolicia = val
                        End If
                    End If

                    dt = Await ConsultasGenericas.ObtenerHistoricoDataTableAsync(
                        uow,
                        tipoHist,
                        idPolicia,
                        Nothing,
                        Nothing)

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
        Dim funcionarios = Await uow.Repository(Of Funcionario)().GetAllByPredicateAsync(Function(f) True)
        Dim dicF As Dictionary(Of Integer, Funcionario) = funcionarios.ToDictionary(Function(f) f.Id)
        Dim datos As New List(Of Object)()

        Dim getFuncionarioInfo As Func(Of Integer, (CI As String, Nombre As String)) =
        Function(fid As Integer)
            Dim f As Funcionario = Nothing
            If dicF.TryGetValue(fid, f) Then
                Return (f.CI, f.Nombre)
            Else
                Return (Nothing, Nothing)
            End If
        End Function

        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Nocturnidad Then
            Dim noctList As IEnumerable(Of HistoricoNocturnidad)
            If idPolicia.HasValue Then
                noctList = Await uow.Repository(Of HistoricoNocturnidad)().FindAsync(Function(h) h.FuncionarioId = idPolicia.Value)
            Else
                noctList = Await uow.Repository(Of HistoricoNocturnidad)().GetAllByPredicateAsync(Function(h) True)
            End If

            For Each h In noctList
                If (Not anio.HasValue OrElse h.Anio = anio.Value) AndAlso (Not mes.HasValue OrElse h.Mes = mes.Value) Then
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

        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Presentismo Then
            Dim presList As IEnumerable(Of HistoricoPresentismo)
            If idPolicia.HasValue Then
                presList = Await uow.Repository(Of HistoricoPresentismo)().FindAsync(Function(h) h.FuncionarioId = idPolicia.Value)
            Else
                presList = Await uow.Repository(Of HistoricoPresentismo)().GetAllByPredicateAsync(Function(h) True)
            End If

            For Each h In presList
                If (Not anio.HasValue OrElse h.Anio = anio.Value) AndAlso (Not mes.HasValue OrElse h.Mes = mes.Value) Then
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

        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Viaticos Then
            Dim viatList As IEnumerable(Of HistoricoViatico)
            If idPolicia.HasValue Then
                viatList = Await uow.Repository(Of HistoricoViatico)().FindAsync(Function(h) h.FuncionarioId = idPolicia.Value)
            Else
                viatList = Await uow.Repository(Of HistoricoViatico)().GetAllByPredicateAsync(Function(h) True)
            End If

            For Each h In viatList
                If (Not anio.HasValue OrElse h.Anio = anio.Value) AndAlso (Not mes.HasValue OrElse h.Mes = mes.Value) Then
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

        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Custodias Then
            Dim custList As IEnumerable(Of HistoricoCustodia)
            If idPolicia.HasValue Then
                custList = Await uow.Repository(Of HistoricoCustodia)().FindAsync(Function(h) h.FuncionarioId = idPolicia.Value)
            Else
                custList = Await uow.Repository(Of HistoricoCustodia)().GetAllByPredicateAsync(Function(h) True)
            End If

            For Each h In custList
                Dim a As Integer = h.Fecha.Year
                Dim m As Integer = h.Fecha.Month
                If (Not anio.HasValue OrElse a = anio.Value) AndAlso (Not mes.HasValue OrElse m = mes.Value) Then
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

        Return ModuloExtensions.ToDataTable(datos)
    End Function

    Private Async Function CargarMetadatosFuncionariosAsync(uow As UnitOfWork, funcionarioIds As IEnumerable(Of Integer?)) As Task(Of Dictionary(Of Integer, FuncionarioReporteMetadata))
        If funcionarioIds Is Nothing Then
            Return New Dictionary(Of Integer, FuncionarioReporteMetadata)()
        End If

        Dim ids = funcionarioIds.
            Where(Function(id) id.HasValue AndAlso id.Value > 0).
            Select(Function(id) id.Value).
            Distinct().
            ToList()

        If ids.Count = 0 Then
            Return New Dictionary(Of Integer, FuncionarioReporteMetadata)()
        End If

        Dim query = uow.Context.Set(Of Funcionario)().AsNoTracking() _
            .Where(Function(f) ids.Contains(f.Id)) _
            .Include(Function(f) f.Cargo) _
            .Include(Function(f) f.TipoFuncionario) _
            .Include(Function(f) f.Seccion) _
            .Include(Function(f) f.Escalafon) _
            .Include(Function(f) f.SubEscalafon) _
            .Include(Function(f) f.SubDireccion) _
            .Include(Function(f) f.Funcion) _
            .Include(Function(f) f.PuestoTrabajo) _
            .Include(Function(f) f.Turno) _
            .Include(Function(f) f.Semana) _
            .Include(Function(f) f.Horario) _
            .Include(Function(f) f.PrestadorSalud)

        Dim funcionarios = Await query.ToListAsync()

        Return funcionarios.ToDictionary(Function(f) f.Id, Function(f) ConstruirMetadatosFuncionario(f))
    End Function

    Private Function ObtenerMetadatosFuncionario(metadata As IDictionary(Of Integer, FuncionarioReporteMetadata), funcionarioId As Integer?) As FuncionarioReporteMetadata
        If metadata Is Nothing OrElse Not funcionarioId.HasValue Then
            Return CrearMetadatosFuncionarioVacios()
        End If

        Dim result As FuncionarioReporteMetadata = Nothing
        If metadata.TryGetValue(funcionarioId.Value, result) Then
            Return result
        End If

        Return CrearMetadatosFuncionarioVacios()
    End Function

    Private Function ConstruirMetadatosFuncionario(funcionario As Funcionario) As FuncionarioReporteMetadata
        If funcionario Is Nothing Then
            Return CrearMetadatosFuncionarioVacios()
        End If

        Return New FuncionarioReporteMetadata With {
            .TipoDeFuncionario = NormalizarValorReporte(If(funcionario.TipoFuncionario IsNot Nothing, funcionario.TipoFuncionario.Nombre, Nothing)),
            .Cargo = NormalizarValorReporte(If(funcionario.Cargo IsNot Nothing, funcionario.Cargo.Nombre, Nothing)),
            .Seccion = NormalizarValorReporte(If(funcionario.Seccion IsNot Nothing, funcionario.Seccion.Nombre, Nothing)),
            .Escalafon = NormalizarValorReporte(If(funcionario.Escalafon IsNot Nothing, funcionario.Escalafon.Nombre, Nothing)),
            .SubEscalafon = NormalizarValorReporte(If(funcionario.SubEscalafon IsNot Nothing, funcionario.SubEscalafon.Nombre, Nothing)),
            .SubDireccion = NormalizarValorReporte(If(funcionario.SubDireccion IsNot Nothing, funcionario.SubDireccion.Nombre, Nothing)),
            .Funcion = NormalizarValorReporte(If(funcionario.Funcion IsNot Nothing, funcionario.Funcion.Nombre, Nothing)),
            .PuestoDeTrabajo = NormalizarValorReporte(If(funcionario.PuestoTrabajo IsNot Nothing, funcionario.PuestoTrabajo.Nombre, Nothing)),
            .Turno = NormalizarValorReporte(If(funcionario.Turno IsNot Nothing, funcionario.Turno.Nombre, Nothing)),
            .Semana = NormalizarValorReporte(If(funcionario.Semana IsNot Nothing, funcionario.Semana.Nombre, Nothing)),
            .Horario = NormalizarValorReporte(If(funcionario.Horario IsNot Nothing, funcionario.Horario.Nombre, Nothing)),
            .PrestadorSalud = NormalizarValorReporte(If(funcionario.PrestadorSalud IsNot Nothing, funcionario.PrestadorSalud.Nombre, Nothing)),
            .EstadoFuncionario = If(funcionario.Activo, "Activo", "Inactivo"),
            .Activo = funcionario.Activo
        }
    End Function

    Private Function CrearMetadatosFuncionarioVacios() As FuncionarioReporteMetadata
        Return New FuncionarioReporteMetadata With {
            .TipoDeFuncionario = NormalizarValorReporte(Nothing),
            .Cargo = NormalizarValorReporte(Nothing),
            .Seccion = NormalizarValorReporte(Nothing),
            .Escalafon = NormalizarValorReporte(Nothing),
            .SubEscalafon = NormalizarValorReporte(Nothing),
            .SubDireccion = NormalizarValorReporte(Nothing),
            .Funcion = NormalizarValorReporte(Nothing),
            .PuestoDeTrabajo = NormalizarValorReporte(Nothing),
            .Turno = NormalizarValorReporte(Nothing),
            .Semana = NormalizarValorReporte(Nothing),
            .Horario = NormalizarValorReporte(Nothing),
            .PrestadorSalud = NormalizarValorReporte(Nothing),
            .EstadoFuncionario = NormalizarValorReporte("Sin Estado"),
            .Activo = False
        }
    End Function

    Private Function NormalizarValorReporte(value As String, Optional valorDefecto As String = "N/A") As String
        If String.IsNullOrWhiteSpace(value) Then
            Return valorDefecto
        End If
        Return value.Trim()
    End Function

    Private Function CalcularCantidadFuncionarios(funcionariosCadena As String) As Integer
        If String.IsNullOrWhiteSpace(funcionariosCadena) Then
            Return 0
        End If
        Dim separadores = New String() {",", ";", ControlChars.Lf, ControlChars.Cr, "|", Environment.NewLine}

        Dim tokens = funcionariosCadena.Split(separadores, StringSplitOptions.RemoveEmptyEntries)
        Return tokens.Select(Function(t) t.Trim()).Count(Function(t) Not String.IsNullOrWhiteSpace(t))
    End Function

    Private Function ConstruirMapaFuncionariosPorNovedad(detalles As IEnumerable(Of vw_NovedadesCompletas)) As IDictionary(Of Integer, String)
        Dim resultado As New Dictionary(Of Integer, String)()
        If detalles Is Nothing Then
            Return resultado
        End If
        For Each grupo In detalles.Where(Function(d) d IsNot Nothing).GroupBy(Function(d) d.Id)
            Dim etiquetas As New List(Of String)()
            Dim yaIncluidos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            For Each detalle In grupo
                Dim etiqueta = FormatearFuncionarioParaLista(detalle.NombreFuncionario, detalle.CI)
                If String.IsNullOrWhiteSpace(etiqueta) Then Continue For
                If yaIncluidos.Add(etiqueta) Then
                    etiquetas.Add(etiqueta)
                End If
            Next
            If etiquetas.Count > 0 Then
                resultado(grupo.Key) = String.Join("; ", etiquetas)
            End If
        Next
        Return resultado
    End Function

    Private Function ObtenerFuncionariosDetallados(mapa As IDictionary(Of Integer, String), novedadId As Integer) As String
        If mapa Is Nothing Then
            Return String.Empty
        End If
        Dim valor As String = Nothing
        If mapa.TryGetValue(novedadId, valor) Then
            Return valor
        End If
        Return String.Empty
    End Function

    Private Function FormatearFuncionarioParaLista(nombre As String, cedula As String) As String
        Dim nombreLimpio = NormalizarValorReporte(nombre, String.Empty)
        Dim cedulaLimpia = NormalizarValorReporte(cedula, String.Empty)
        Dim partes As New List(Of String)()
        If Not String.IsNullOrWhiteSpace(nombreLimpio) Then
            partes.Add(nombreLimpio)
        End If
        If Not String.IsNullOrWhiteSpace(cedulaLimpia) Then
            If partes.Count > 0 Then
                partes.Add($"({cedulaLimpia})")
            Else
                partes.Add(cedulaLimpia)
            End If
        End If
        Return String.Join(" ", partes).Trim()
    End Function

    Private Function CrearTablaNovedadesVacia() As DataTable
        Dim table As New DataTable()
        table.Columns.Add("NovedadId", GetType(Integer))
        table.Columns.Add("Fecha", GetType(Date))
        table.Columns.Add("Resumen", GetType(String))
        table.Columns.Add("Texto", GetType(String))
        table.Columns.Add("Estado", GetType(String))
        table.Columns.Add("FuncionariosLista", GetType(String))
        table.Columns.Add("CantidadFuncionarios", GetType(Integer))
        table.Columns.Add("FuncionarioId", GetType(Integer))
        table.Columns.Add("NombreCompleto", GetType(String))
        table.Columns.Add("Cedula", GetType(String))
        table.Columns.Add("Observaciones", GetType(String))
        table.Columns.Add("TipoDeFuncionario", GetType(String))
        table.Columns.Add("Cargo", GetType(String))
        table.Columns.Add("Seccion", GetType(String))
        table.Columns.Add("Escalafon", GetType(String))
        table.Columns.Add("SubEscalafon", GetType(String))
        table.Columns.Add("SubDireccion", GetType(String))
        table.Columns.Add("Funcion", GetType(String))
        table.Columns.Add("PuestoDeTrabajo", GetType(String))
        table.Columns.Add("Turno", GetType(String))
        table.Columns.Add("Semana", GetType(String))
        table.Columns.Add("Horario", GetType(String))
        table.Columns.Add("PrestadorSalud", GetType(String))
        table.Columns.Add("EstadoFuncionario", GetType(String))
        table.Columns.Add("Activo", GetType(Boolean))
        Return table
    End Function

    Private Function ConstruirObservacionesNovedad(resumen As String, texto As String, estado As String) As String
        Dim partes As New List(Of String)()
        Dim resumenNormalizado = NormalizarValorReporte(resumen, String.Empty)
        Dim textoNormalizado = NormalizarValorReporte(texto, String.Empty)
        Dim estadoNormalizado = NormalizarValorReporte(estado, String.Empty)
        If Not String.IsNullOrWhiteSpace(resumenNormalizado) Then
            partes.Add(resumenNormalizado)
        End If
        If Not String.IsNullOrWhiteSpace(textoNormalizado) Then
            partes.Add(textoNormalizado)
        End If
        If Not String.IsNullOrWhiteSpace(estadoNormalizado) Then
            partes.Add($"Estado: {estadoNormalizado}")
        End If
        If partes.Count = 0 Then
            Return "Sin observaciones"
        End If
        Dim vistaUnica = New List(Of String)()
        Dim existentes = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        For Each item In partes
            Dim valor = item.Trim()
            If valor.Length = 0 Then Continue For
            If existentes.Add(valor) Then
                vistaUnica.Add(valor)
            End If
        Next
        If vistaUnica.Count = 0 Then
            Return "Sin observaciones"
        End If
        Return String.Join(" | ", vistaUnica)
    End Function

    Private Class FuncionarioReporteMetadata
        Public Property TipoDeFuncionario As String
        Public Property Cargo As String
        Public Property Seccion As String
        Public Property Escalafon As String
        Public Property SubEscalafon As String
        Public Property SubDireccion As String
        Public Property Funcion As String
        Public Property PuestoDeTrabajo As String
        Public Property Turno As String
        Public Property Semana As String
        Public Property Horario As String
        Public Property PrestadorSalud As String
        Public Property EstadoFuncionario As String
        Public Property Activo As Boolean
    End Class

    Private Function CrearTablaNotificacionesVacia() As DataTable
        Dim table As New DataTable()
        table.Columns.Add("NombreCompleto", GetType(String))
        table.Columns.Add("Cedula", GetType(String))
        table.Columns.Add("TipoNotificacion", GetType(String))
        table.Columns.Add("Estado", GetType(String))
        table.Columns.Add("FechaProgramada", GetType(Date))
        table.Columns.Add("Texto", GetType(String))
        table.Columns.Add("Documento", GetType(String))
        table.Columns.Add("ExpMinisterial", GetType(String))
        table.Columns.Add("ExpINR", GetType(String))
        table.Columns.Add("Oficina", GetType(String))
        table.Columns.Add("TipoDeFuncionario", GetType(String))
        table.Columns.Add("Cargo", GetType(String))
        table.Columns.Add("Seccion", GetType(String))
        table.Columns.Add("Escalafon", GetType(String))
        table.Columns.Add("SubEscalafon", GetType(String))
        table.Columns.Add("SubDireccion", GetType(String))
        table.Columns.Add("Funcion", GetType(String))
        table.Columns.Add("PuestoDeTrabajo", GetType(String))
        table.Columns.Add("Turno", GetType(String))
        table.Columns.Add("Semana", GetType(String))
        table.Columns.Add("Horario", GetType(String))
        table.Columns.Add("PrestadorSalud", GetType(String))
        table.Columns.Add("EstadoFuncionario", GetType(String))
        table.Columns.Add("Activo", GetType(Boolean))
        Return table
    End Function

    Private Function CrearTablaLicenciasVacia() As DataTable
        Dim table As New DataTable()
        table.Columns.Add("NombreCompleto", GetType(String))
        table.Columns.Add("Cedula", GetType(String))
        table.Columns.Add("TipoLicencia", GetType(String))
        table.Columns.Add("FechaInicio", GetType(Date))
        table.Columns.Add("FechaFin", GetType(Date))
        table.Columns.Add("Dias", GetType(Integer))
        table.Columns.Add("Observaciones", GetType(String))
        table.Columns.Add("Activo", GetType(Boolean))
        table.Columns.Add("TipoDeFuncionario", GetType(String))
        table.Columns.Add("Cargo", GetType(String))
        table.Columns.Add("Seccion", GetType(String))
        table.Columns.Add("Escalafon", GetType(String))
        Return table
    End Function
End Module
'' Apex/Services/ConsultasGenericas.vb
'Imports System.Data.Entity
'Imports System.Collections.Generic
'Imports System.Linq
'Imports System.Linq.Expressions
'Public Module ConsultasGenericas



'    Public Async Function ObtenerDatosGenericosAsync(
'        tipo As TipoOrigenDatos,
'        Optional fechaInicio As Date? = Nothing,
'        Optional fechaFin As Date? = Nothing,
'        Optional filtros As List(Of Tuple(Of String, String, String)) = Nothing
'    ) As Task(Of DataTable)
'        Using uow As New UnitOfWork()
'            Dim dt As New DataTable()

'            Select Case tipo
'                Case TipoOrigenDatos.EstadosTransitorios
'                    If Not fechaInicio.HasValue OrElse Not fechaFin.HasValue Then
'                        Throw New ArgumentNullException("Las fechas de inicio y fin son requeridas para EstadosTransitorios.")
'                    End If

'                    ' Lógica para EstadosTransitorios (sin cambios, ya era correcta)
'                    Dim sanciones = uow.Repository(Of SancionDetalle)().GetAll().
'                        Include("EstadoTransitorio.Funcionario").
'                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
'                        Select(Function(det) New With {
'                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
'                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
'                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
'                            .Tipo = "Sanción",
'                            .FechaInicio = det.FechaDesde,
'                            .FechaFin = CType(det.FechaHasta, DateTime?),
'                            .Observaciones = det.Observaciones
'                        })

'                    Dim designaciones = uow.Repository(Of DesignacionDetalle)().GetAll().
'                        Include("EstadoTransitorio.Funcionario").
'                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
'                        Select(Function(det) New With {
'                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
'                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
'                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
'                            .Tipo = "Designación",
'                            .FechaInicio = det.FechaDesde,
'                            .FechaFin = det.FechaHasta,
'                            .Observaciones = det.Observaciones
'                        })

'                    Dim sumarios = uow.Repository(Of SumarioDetalle)().GetAll().
'                        Include("EstadoTransitorio.Funcionario").
'                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
'                        Select(Function(det) New With {
'                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
'                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
'                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
'                            .Tipo = "Sumario",
'                            .FechaInicio = det.FechaDesde,
'                            .FechaFin = det.FechaHasta,
'                            .Observaciones = det.Observaciones
'                        })

'                    Dim ordenCinco = uow.Repository(Of OrdenCincoDetalle)().GetAll().
'                        Include("EstadoTransitorio.Funcionario").
'                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
'                        Select(Function(det) New With {
'                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
'                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
'                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
'                            .Tipo = "Orden Cinco",
'                            .FechaInicio = det.FechaDesde,
'                            .FechaFin = CType(det.FechaHasta, DateTime?),
'                            .Observaciones = det.Observaciones
'                        })
'                    ' Se suma un día a la fecha 'fin' para que la consulta incluya todo el día.
'                    Dim fechaFinInclusive As Date = fechaFin.Value.AddDays(1)

'                    Dim retenes = uow.Repository(Of RetenDetalle)().GetAll().
'                        Include("EstadoTransitorio.Funcionario").
'                        Where(Function(det) det.FechaReten >= fechaInicio.Value AndAlso det.FechaReten < fechaFinInclusive).
'                        Select(Function(det) New With {
'                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
'                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
'                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
'                            .Tipo = "Retén",
'                            .FechaInicio = det.FechaReten,
'                            .FechaFin = CType(det.FechaReten, DateTime?),
'                            .Observaciones = det.Observaciones
'                        })

'                    Dim enfermedades = uow.Repository(Of EnfermedadDetalle)().GetAll().
'                        Include("EstadoTransitorio.Funcionario").
'                        Where(Function(det) det.FechaDesde <= fechaFin.Value AndAlso (Not det.FechaHasta.HasValue OrElse det.FechaHasta.Value >= fechaInicio.Value)).
'                        Select(Function(det) New With {
'                            .FuncionarioId = det.EstadoTransitorio.FuncionarioId,
'                            .NombreCompleto = det.EstadoTransitorio.Funcionario.Nombre,
'                            .Cedula = det.EstadoTransitorio.Funcionario.CI,
'                            .Tipo = "Enfermedad",
'                            .FechaInicio = det.FechaDesde,
'                            .FechaFin = CType(det.FechaHasta, DateTime?),
'                            .Observaciones = det.Observaciones
'                        })

'                    Dim resultadoUnion = Await sanciones.Union(designaciones).
'                                                      Union(sumarios).
'                                                      Union(ordenCinco).
'                                                      Union(retenes).
'                                                      Union(enfermedades).ToListAsync()

'                    If Not resultadoUnion.Any() Then
'                        Return New DataTable()
'                    End If

'                    Dim metadatosPorFuncionario = Await CargarMetadatosFuncionariosAsync(
'                        uow,
'                        resultadoUnion.Select(Function(r) CType(r.FuncionarioId, Integer?))
'                    )

'                    Dim resultadoEnriquecido = resultadoUnion.Select(
'                        Function(r)
'                            Dim meta = ObtenerMetadatosFuncionario(metadatosPorFuncionario, CType(r.FuncionarioId, Integer?))

'                            Return New With {
'                                .FuncionarioId = r.FuncionarioId,
'                                .NombreCompleto = NormalizarValorReporte(r.NombreCompleto),
'                                .Cedula = NormalizarValorReporte(r.Cedula),
'                                .Tipo = NormalizarValorReporte(r.Tipo),
'                                .FechaInicio = r.FechaInicio,
'                                .FechaFin = r.FechaFin,
'                                .Observaciones = NormalizarValorReporte(r.Observaciones, String.Empty),
'                                .TipoDeFuncionario = meta.TipoDeFuncionario,
'                                .Cargo = meta.Cargo,
'                                .Seccion = meta.Seccion,
'                                .Escalafon = meta.Escalafon,
'                                .SubEscalafon = meta.SubEscalafon,
'                                .SubDireccion = meta.SubDireccion,
'                                .Funcion = meta.Funcion,
'                                .PuestoDeTrabajo = meta.PuestoDeTrabajo,
'                                .Turno = meta.Turno,
'                                .Semana = meta.Semana,
'                                .Horario = meta.Horario,
'                                .PrestadorSalud = meta.PrestadorSalud,
'                                .EstadoFuncionario = meta.EstadoFuncionario,
'                                .Activo = meta.Activo
'                            }
'                        End Function
'                    ).ToList()

'                    dt = resultadoEnriquecido.ToDataTable()

'                Case TipoOrigenDatos.Notificaciones
'                    If Not fechaInicio.HasValue OrElse Not fechaFin.HasValue Then
'                        Throw New ArgumentNullException("Las fechas de inicio y fin son requeridas para Notificaciones.")
'                    End If
'                    Dim notificacionService = New NotificacionService(uow)
'                    Dim notificaciones = Await notificacionService.GetAllConDetallesAsync(
'                        fechaDesde:=fechaInicio,
'                        fechaHasta:=fechaFin
'                    )

'                    If Not notificaciones.Any() Then
'                        Return CrearTablaNotificacionesVacia()
'                    End If

'                    Dim metadatosPorFuncionario = Await CargarMetadatosFuncionariosAsync(uow, notificaciones.Select(Function(n) CType(n.FuncionarioId, Integer?)))

'                    Dim resultadoNotificaciones = notificaciones.Select(Function(n)
'                                                                            Dim meta = ObtenerMetadatosFuncionario(metadatosPorFuncionario, CType(n.FuncionarioId, Integer?))

'                                                                            Return New With {
'                            .NombreCompleto = NormalizarValorReporte(n.NombreFuncionario),
'                            .Cedula = NormalizarValorReporte(n.CI),
'                            .TipoNotificacion = NormalizarValorReporte(n.TipoNotificacion),
'                            .Estado = NormalizarValorReporte(n.Estado),
'                            .FechaProgramada = n.FechaProgramada,
'                            .Texto = NormalizarValorReporte(n.Texto, String.Empty),
'                            .Documento = NormalizarValorReporte(n.Documento),
'                            .ExpMinisterial = NormalizarValorReporte(n.ExpMinisterial),
'                            .ExpINR = NormalizarValorReporte(n.ExpINR),
'                            .Oficina = NormalizarValorReporte(n.Oficina),
'                            .TipoDeFuncionario = meta.TipoDeFuncionario,
'                            .Cargo = meta.Cargo,
'                            .Seccion = meta.Seccion,
'                            .Escalafon = meta.Escalafon,
'                            .SubEscalafon = meta.SubEscalafon,
'                            .SubDireccion = meta.SubDireccion,
'                            .Funcion = meta.Funcion,
'                            .PuestoDeTrabajo = meta.PuestoDeTrabajo,
'                            .Turno = meta.Turno,
'                            .Semana = meta.Semana,
'                            .Horario = meta.Horario,
'                            .PrestadorSalud = meta.PrestadorSalud,
'                            .EstadoFuncionario = meta.EstadoFuncionario,
'                            .Activo = meta.Activo
'                        }
'                                                                        End Function).ToList()

'                    dt = resultadoNotificaciones.ToDataTable()
'                Case TipoOrigenDatos.Licencias
'                    Dim licenciaService = New LicenciaService(uow)
'                    Dim licencias = Await licenciaService.GetAllConDetallesAsync(fechaDesde:=fechaInicio, fechaHasta:=fechaFin)

'                    ' Si no hay licencias, devolvemos una tabla vacía.
'                    If Not licencias.Any() Then
'                        Return New DataTable()
'                    End If

'                    ' --- INICIO DE LA MODIFICACIÓN ---

'                    ' Proyectamos los datos de las licencias usando la información ya enriquecida
'                    ' que devuelve el procedimiento almacenado. De esta manera evitamos incluir
'                    ' objetos complejos (entidades de EF) en el resultado, lo que generaba que
'                    ' en pantalla se mostraran los nombres de tipo (por ejemplo "Data_Entity…").
'                    Dim resultadoEnriquecido = licencias.Select(Function(lic) New With {
'                        .NombreCompleto = lic.NombreFuncionario,
'                        .Cedula = lic.CI,
'                        .TipoLicencia = lic.TipoLicencia,
'                        .FechaInicio = lic.FechaInicio,
'                        .FechaFin = lic.FechaFin,
'                        .Dias = lic.DuracionDias,
'                        .Observaciones = lic.Observaciones,
'                        .Activo = lic.Activo,
'                        .TipoDeFuncionario = lic.TipoDeFuncionario,
'                        .Cargo = lic.Cargo,
'                        .Seccion = lic.Seccion,
'                        .Escalafon = lic.Escalafon
'                    }).ToList()

'                    ' Convertimos la lista enriquecida a un DataTable.
'                    dt = resultadoEnriquecido.ToDataTable()

'    ' --- FIN DE LA MODIFICACIÓN ---
'                'Case TipoOrigenDatos.Licencias
'                '    Dim licenciaService = New LicenciaService(uow)
'                '    Dim licencias = Await licenciaService.GetAllConDetallesAsync(fechaDesde:=fechaInicio, fechaHasta:=fechaFin)
'                '    dt = licencias.ToDataTable()

'                Case TipoOrigenDatos.Novedades
'                    Dim novedadService = New NovedadService(uow)
'                    Dim novedadesAgrupadas = Await novedadService.GetAllAgrupadasAsync(fechaInicio, fechaFin)

'                    If novedadesAgrupadas Is Nothing OrElse Not novedadesAgrupadas.Any() Then
'                        Return CrearTablaNovedadesVacia()
'                    End If

'                    Dim novedadIds = novedadesAgrupadas.Select(Function(n) n.Id).Distinct().ToList()
'                    Dim novedadesDetalladas = Await novedadService.GetNovedadesCompletasByIds(novedadIds)
'                    If novedadesDetalladas Is Nothing Then
'                        novedadesDetalladas = New List(Of vw_NovedadesCompletas)()
'                    End If
'                    Dim metadatosPorFuncionario = Await CargarMetadatosFuncionariosAsync(uow, novedadesDetalladas.Select(Function(n) n.FuncionarioId))

'                    Dim resumenPorNovedad = novedadesAgrupadas.ToDictionary(Function(n) n.Id,
'                                                                          Function(n) New With {
'                                                                              .Resumen = NormalizarValorReporte(n.Resumen, String.Empty),
'                                                                              .Funcionarios = NormalizarValorReporte(n.Funcionarios, String.Empty),
'                                                                              .Estado = NormalizarValorReporte(n.Estado)
'                                                                          })

'                    Dim funcionariosDetalladosPorNovedad = ConstruirMapaFuncionariosPorNovedad(novedadesDetalladas)

'                    Dim resultadoNovedades As New List(Of Object)()

'                    If novedadesDetalladas IsNot Nothing AndAlso novedadesDetalladas.Any() Then
'                        For Each detalle In novedadesDetalladas
'                            Dim infoResumen = Nothing
'                            resumenPorNovedad.TryGetValue(detalle.Id, infoResumen)

'                            Dim listaFuncionariosDetallada = ObtenerFuncionariosDetallados(funcionariosDetalladosPorNovedad, detalle.Id)
'                            Dim listaFuncionarios = If(String.IsNullOrWhiteSpace(listaFuncionariosDetallada),
'                                                       If(infoResumen IsNot Nothing, infoResumen.Funcionarios, NormalizarValorReporte(detalle.NombreFuncionario, String.Empty)),
'                                                       listaFuncionariosDetallada)
'                            Dim cantidadFuncionarios = CalcularCantidadFuncionarios(listaFuncionarios)
'                            Dim meta = ObtenerMetadatosFuncionario(metadatosPorFuncionario, detalle.FuncionarioId)

'                            resultadoNovedades.Add(New With {
'                                .NovedadId = detalle.Id,
'                                .Fecha = detalle.Fecha,
'                                .Resumen = If(infoResumen IsNot Nothing, infoResumen.Resumen, NormalizarValorReporte(detalle.Texto, String.Empty)),
'                                .Texto = NormalizarValorReporte(detalle.Texto, String.Empty),
'                                .Estado = If(infoResumen IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(infoResumen.Estado), infoResumen.Estado, NormalizarValorReporte(detalle.Estado)),
'                                .FuncionariosLista = listaFuncionarios,
'                                .CantidadFuncionarios = cantidadFuncionarios,
'                                .FuncionarioId = detalle.FuncionarioId,
'                                .FuncionarioNombre = NormalizarValorReporte(detalle.NombreFuncionario, "Sin Asignar"),
'                                .FuncionarioCedula = NormalizarValorReporte(detalle.CI),
'                                .TipoDeFuncionario = meta.TipoDeFuncionario,
'                                .Cargo = meta.Cargo,
'                                .Seccion = meta.Seccion,
'                                .Escalafon = meta.Escalafon,
'                                .SubEscalafon = meta.SubEscalafon,
'                                .SubDireccion = meta.SubDireccion,
'                                .Funcion = meta.Funcion,
'                                .PuestoDeTrabajo = meta.PuestoDeTrabajo,
'                                .Turno = meta.Turno,
'                                .Semana = meta.Semana,
'                                .Horario = meta.Horario,
'                                .PrestadorSalud = meta.PrestadorSalud,
'                                .EstadoFuncionario = meta.EstadoFuncionario,
'                                .Activo = meta.Activo
'                            })
'                        Next
'                    End If

'                    Dim idsConDetalle = If(novedadesDetalladas IsNot Nothing, novedadesDetalladas.Select(Function(n) n.Id).Distinct().ToList(), New List(Of Integer)())
'                    For Each agrupada In novedadesAgrupadas
'                        If idsConDetalle.Contains(agrupada.Id) Then Continue For

'                        Dim metaVacio = CrearMetadatosFuncionarioVacios()
'                        Dim listaFuncionariosDetallada = ObtenerFuncionariosDetallados(funcionariosDetalladosPorNovedad, agrupada.Id)
'                        Dim listaFuncionarios = If(String.IsNullOrWhiteSpace(listaFuncionariosDetallada),
'                                                   NormalizarValorReporte(agrupada.Funcionarios, String.Empty),
'                                                   listaFuncionariosDetallada)

'                        resultadoNovedades.Add(New With {
'                            .NovedadId = agrupada.Id,
'                            .Fecha = agrupada.Fecha,
'                            .Resumen = NormalizarValorReporte(agrupada.Resumen, String.Empty),
'                            .Texto = NormalizarValorReporte(Nothing, String.Empty),
'                            .Estado = NormalizarValorReporte(agrupada.Estado),
'                            .FuncionariosLista = listaFuncionarios,
'                            .CantidadFuncionarios = CalcularCantidadFuncionarios(listaFuncionarios),
'                            .FuncionarioId = CType(Nothing, Integer?),
'                            .FuncionarioNombre = NormalizarValorReporte(Nothing, "Sin Asignar"),
'                            .FuncionarioCedula = NormalizarValorReporte(Nothing),
'                            .TipoDeFuncionario = metaVacio.TipoDeFuncionario,
'                            .Cargo = metaVacio.Cargo,
'                            .Seccion = metaVacio.Seccion,
'                            .Escalafon = metaVacio.Escalafon,
'                            .SubEscalafon = metaVacio.SubEscalafon,
'                            .SubDireccion = metaVacio.SubDireccion,
'                            .Funcion = metaVacio.Funcion,
'                            .PuestoDeTrabajo = metaVacio.PuestoDeTrabajo,
'                            .Turno = metaVacio.Turno,
'                            .Semana = metaVacio.Semana,
'                            .Horario = metaVacio.Horario,
'                            .PrestadorSalud = metaVacio.PrestadorSalud,
'                            .EstadoFuncionario = metaVacio.EstadoFuncionario,
'                            .Activo = metaVacio.Activo
'                        })
'                    Next

'                    If resultadoNovedades.Count = 0 Then
'                        Return CrearTablaNovedadesVacia()
'                    End If

'                    dt = resultadoNovedades.ToDataTable()
'                Case TipoOrigenDatos.Funcionarios
'                    Dim funcionarioService = New FuncionarioService(uow)
'                    Dim funcionarios = Await funcionarioService.GetFuncionariosParaVistaAsync()
'                    dt = funcionarios.ToDataTable()

'                Case TipoOrigenDatos.Auditoria
'                    Dim queryAuditoria = uow.Context.Set(Of AuditoriaCambios)().AsQueryable()

'                    If filtros IsNot Nothing AndAlso filtros.Any() Then
'                        queryAuditoria = ApplyAdvancedFilters(queryAuditoria, filtros)
'                    End If

'                    ' --- INICIO DE LA CORRECCIÓN ---
'                    If fechaInicio.HasValue AndAlso fechaFin.HasValue Then
'                        ' Se suma un día a la fecha 'fin' para que la consulta incluya todo el día.
'                        Dim fechaFinInclusive As Date = fechaFin.Value.AddDays(1)
'                        ' Se usa '<' en lugar de '<=' para la fecha final.
'                        queryAuditoria = queryAuditoria.Where(Function(a) a.FechaHora >= fechaInicio.Value AndAlso a.FechaHora < fechaFinInclusive)
'                    End If
'                    ' --- FIN DE LA CORRECCIÓN ---

'                    ' Subconsulta para cambios en la tabla 'Funcionario'
'                    Dim auditFuncionarios = From aud In queryAuditoria
'                                            Where aud.TablaNombre = "Funcionario"
'                                            Join f In uow.Context.Set(Of Funcionario)() On aud.RegistroId Equals f.Id.ToString()
'                                            Select New AuditoriaEnriquecida With {
'                                                .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
'                                                .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
'                                                .Cedula = f.CI, .NombreCompleto = f.Nombre
'                                            }

'                    ' Subconsulta para cambios en la tabla 'EstadoTransitorio'
'                    Dim auditEstados = From aud In queryAuditoria
'                                       Where aud.TablaNombre = "EstadoTransitorio"
'                                       Join et In uow.Context.Set(Of EstadoTransitorio)().Include("Funcionario") On aud.RegistroId Equals et.Id.ToString()
'                                       Select New AuditoriaEnriquecida With {
'                                           .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
'                                           .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
'                                           .Cedula = If(et.Funcionario IsNot Nothing, et.Funcionario.CI, Nothing),
'                                           .NombreCompleto = If(et.Funcionario IsNot Nothing, et.Funcionario.Nombre, Nothing)
'                                       }

'                    ' Subconsulta para cambios en la tabla 'HistoricoLicencia'
'                    Dim auditLicencias = From aud In queryAuditoria
'                                         Where aud.TablaNombre = "HistoricoLicencia"
'                                         Join lic In uow.Context.Set(Of HistoricoLicencia)().Include("Funcionario") On aud.RegistroId Equals lic.Id.ToString()
'                                         Select New AuditoriaEnriquecida With {
'                                             .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
'                                             .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
'                                             .Cedula = If(lic.Funcionario IsNot Nothing, lic.Funcionario.CI, Nothing),
'                                             .NombreCompleto = If(lic.Funcionario IsNot Nothing, lic.Funcionario.Nombre, Nothing)
'                                         }

'                    ' Subconsulta para el resto de las tablas de auditoría
'                    Dim tablasConJoin = {"Funcionario", "EstadoTransitorio", "HistoricoLicencia"}
'                    Dim auditOtros = From aud In queryAuditoria
'                                     Where Not tablasConJoin.Contains(aud.TablaNombre)
'                                     Select New AuditoriaEnriquecida With {
'                                         .Id = aud.Id, .FechaHora = aud.FechaHora, .UsuarioAccion = aud.UsuarioAccion, .TablaNombre = aud.TablaNombre,
'                                         .CampoNombre = aud.CampoNombre, .ValorAnterior = aud.ValorAnterior, .ValorNuevo = aud.ValorNuevo,
'                                         .Cedula = Nothing, .NombreCompleto = Nothing
'                                     }

'                    ' Unir todas las consultas de auditoría en una sola
'                    Dim queryFinal = auditFuncionarios.Union(auditEstados).Union(auditLicencias).Union(auditOtros)

'                    Dim resultados = Await queryFinal.OrderByDescending(Function(a) a.FechaHora).ToListAsync()
'                    dt = resultados.ToDataTable()
'                Case TipoOrigenDatos.HistoricoConceptos
'                    ' Leer filtros si existen (TipoHistorico, FuncionarioId, etc.)
'                    Dim tipoHist As TipoHistorico? = Nothing
'                    Dim idPolicia As Integer? = Nothing

'                    If filtros IsNot Nothing Then
'                        ' TipoHistorico: puede ser "Nocturnidad", "Presentismo", "Viaticos" o "Custodias".
'                        Dim ft = filtros.FirstOrDefault(Function(t) t.Item1.Equals("TipoHistorico", StringComparison.OrdinalIgnoreCase))
'                        If ft IsNot Nothing Then
'                            Dim aux As TipoHistorico
'                            If [Enum].TryParse(ft.Item3, True, aux) Then tipoHist = aux
'                        End If

'                        ' Id del funcionario (puede llamarse "FuncionarioId" o "IdPolicia" en tus filtros)
'                        Dim fp = filtros.FirstOrDefault(Function(t) t.Item1.Equals("FuncionarioId", StringComparison.OrdinalIgnoreCase) _
'                                     OrElse t.Item1.Equals("IdPolicia", StringComparison.OrdinalIgnoreCase))
'                        If fp IsNot Nothing Then
'                            Dim val As Integer
'                            If Integer.TryParse(fp.Item3, val) Then idPolicia = val
'                        End If
'                    End If

'                    ' No pases anio/mes a menos que realmente quieras filtrarlos.
'                    ' El método unificado se encarga de unir las tablas históricas con la tabla Funcionario.
'                    dt = Await ConsultasGenericas.ObtenerHistoricoDataTableAsync(
'            uow,
'            tipoHist,
'            idPolicia,
'            Nothing,  ' año
'            Nothing)  ' mes

'                Case Else
'                    Throw New NotImplementedException($"La consulta para el tipo '{tipo.ToString()}' no está implementada.")
'            End Select

'            Return dt
'        End Using
'    End Function

'    Private Function ApplyAdvancedFilters(Of T)(source As IQueryable(Of T), filtros As List(Of Tuple(Of String, String, String))) As IQueryable(Of T)
'        If filtros Is Nothing OrElse Not filtros.Any() Then
'            Return source
'        End If

'        Dim parameter = Expression.Parameter(GetType(T), "entity")
'        Dim body As Expression = Nothing

'        For Each filtro In filtros
'            Dim propertyName = filtro.Item1
'            Dim operador = filtro.Item2
'            Dim valor = filtro.Item3

'            Dim propertyInfo = GetType(T).GetProperty(propertyName)
'            If propertyInfo Is Nothing Then
'                Continue For
'            End If

'            Dim propertyExpr = Expression.Property(parameter, propertyName)
'            Dim propertyType = propertyInfo.PropertyType

'            Dim convertedValue As Object
'            Try
'                Dim underlyingType = Nullable.GetUnderlyingType(propertyType)
'                Dim targetType = If(underlyingType IsNot Nothing, underlyingType, propertyType)
'                convertedValue = Convert.ChangeType(valor, targetType)
'            Catch ex As Exception
'                ' System.Diagnostics.Debug.WriteLine($"Error al convertir valor para filtro: {ex.Message}")
'                Continue For
'            End Try

'            Dim valueExpr = Expression.Constant(convertedValue, propertyType)
'            Dim condition As Expression = Nothing

'            Select Case operador.ToLower()
'                Case "contiene"
'                    If propertyType Is GetType(String) Then
'                        Dim method = GetType(String).GetMethod("Contains", New Type() {GetType(String)})
'                        condition = Expression.Call(propertyExpr, method, Expression.Constant(valor, GetType(String)))
'                    End If
'                Case "es igual a"
'                    condition = Expression.Equal(propertyExpr, valueExpr)
'                Case "mayor o igual que"
'                    condition = Expression.GreaterThanOrEqual(propertyExpr, valueExpr)
'                Case "menor o igual que"
'                    condition = Expression.LessThanOrEqual(propertyExpr, valueExpr)
'            End Select

'            If condition IsNot Nothing Then
'                body = If(body Is Nothing, condition, Expression.AndAlso(body, condition))
'            End If
'        Next

'        If body IsNot Nothing Then
'            Dim lambda = Expression.Lambda(Of Func(Of T, Boolean))(body, parameter)
'            Return source.Where(lambda)
'        End If

'        Return source
'    End Function

'    ' Clase auxiliar para unificar los resultados de las consultas de auditoría
'    Private Class AuditoriaEnriquecida
'        Public Property Id As Integer
'        Public Property FechaHora As DateTime
'        Public Property UsuarioAccion As String
'        Public Property TablaNombre As String
'        Public Property CampoNombre As String
'        Public Property ValorAnterior As String
'        Public Property ValorNuevo As String
'        Public Property Cedula As String
'        Public Property NombreCompleto As String
'    End Class

'    Public Async Function ObtenerHistoricoDataTableAsync(
'    uow As IUnitOfWork,
'    Optional tipo As TipoHistorico? = Nothing,
'    Optional idPolicia As Integer? = Nothing,
'    Optional anio As Integer? = Nothing,
'    Optional mes As Integer? = Nothing
') As Task(Of DataTable)

'        ' --- Precargar funcionarios ---
'        Dim funcionarios = Await uow.Repository(Of Funcionario)() _
'                                .GetAllByPredicateAsync(Function(f) True)
'        Dim dicF As Dictionary(Of Integer, Funcionario) =
'        funcionarios.ToDictionary(Function(f) f.Id)

'        Dim datos As New List(Of Object)()

'        ' Función auxiliar para obtener Cedula y Nombre
'        Dim getFuncionarioInfo As Func(Of Integer, (CI As String, Nombre As String)) =
'        Function(fid As Integer)
'            Dim f As Funcionario = Nothing
'            If dicF.TryGetValue(fid, f) Then
'                Return (f.CI, f.Nombre)
'            Else
'                Return (Nothing, Nothing)
'            End If
'        End Function

'        ' --- Nocturnidad ---
'        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Nocturnidad Then
'            Dim noctList As IEnumerable(Of HistoricoNocturnidad)
'            If idPolicia.HasValue Then
'                noctList = Await uow.Repository(Of HistoricoNocturnidad)().FindAsync(
'                Function(h) h.FuncionarioId = idPolicia.Value)
'            Else
'                noctList = Await uow.Repository(Of HistoricoNocturnidad)().GetAllByPredicateAsync(
'                Function(h) True)
'            End If

'            For Each h In noctList
'                If (Not anio.HasValue OrElse h.Anio = anio.Value) AndAlso
'               (Not mes.HasValue OrElse h.Mes = mes.Value) Then
'                    Dim info = getFuncionarioInfo(h.FuncionarioId)
'                    datos.Add(New With {
'                    .FuncionarioId = h.FuncionarioId,
'                    .Cedula = info.CI,
'                    .NombreCompleto = info.Nombre,
'                    .Tipo = "Nocturnidad",
'                    .Anio = CInt(h.Anio),
'                    .Mes = CInt(h.Mes),
'                    .Fecha = CType(Nothing, Date?),
'                    .Minutos = If(h.Minutos.HasValue, CType(h.Minutos.Value, Integer?), CType(Nothing, Integer?)),
'                    .Dias = CType(Nothing, Integer?),
'                    .Incidencia = CType(Nothing, String),
'                    .Observaciones = CType(Nothing, String),
'                    .Motivo = CType(Nothing, String),
'                    .Area = CType(Nothing, String)
'                })
'                End If
'            Next
'        End If

'        ' --- Presentismo ---
'        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Presentismo Then
'            Dim presList As IEnumerable(Of HistoricoPresentismo)
'            If idPolicia.HasValue Then
'                presList = Await uow.Repository(Of HistoricoPresentismo)().FindAsync(
'                Function(h) h.FuncionarioId = idPolicia.Value)
'            Else
'                presList = Await uow.Repository(Of HistoricoPresentismo)().GetAllByPredicateAsync(
'                Function(h) True)
'            End If

'            For Each h In presList
'                If (Not anio.HasValue OrElse h.Anio = anio.Value) AndAlso
'               (Not mes.HasValue OrElse h.Mes = mes.Value) Then
'                    Dim info = getFuncionarioInfo(h.FuncionarioId)
'                    datos.Add(New With {
'                    .FuncionarioId = h.FuncionarioId,
'                    .Cedula = info.CI,
'                    .NombreCompleto = info.Nombre,
'                    .Tipo = "Presentismo",
'                    .Anio = CInt(h.Anio),
'                    .Mes = CInt(h.Mes),
'                    .Fecha = CType(Nothing, Date?),
'                    .Minutos = If(h.Minutos.HasValue, CType(h.Minutos.Value, Integer?), CType(Nothing, Integer?)),
'                    .Dias = If(h.Dias.HasValue, CType(h.Dias.Value, Integer?), CType(Nothing, Integer?)),
'                    .Incidencia = h.Incidencia,
'                    .Observaciones = h.Observaciones,
'                    .Motivo = CType(Nothing, String),
'                    .Area = CType(Nothing, String)
'                })
'                End If
'            Next
'        End If

'        ' --- Viáticos ---
'        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Viaticos Then
'            Dim viatList As IEnumerable(Of HistoricoViatico)
'            If idPolicia.HasValue Then
'                viatList = Await uow.Repository(Of HistoricoViatico)().FindAsync(
'                Function(h) h.FuncionarioId = idPolicia.Value)
'            Else
'                viatList = Await uow.Repository(Of HistoricoViatico)().GetAllByPredicateAsync(
'                Function(h) True)
'            End If

'            For Each h In viatList
'                If (Not anio.HasValue OrElse h.Anio = anio.Value) AndAlso
'               (Not mes.HasValue OrElse h.Mes = mes.Value) Then
'                    Dim info = getFuncionarioInfo(h.FuncionarioId)
'                    datos.Add(New With {
'                    .FuncionarioId = h.FuncionarioId,
'                    .Cedula = info.CI,
'                    .NombreCompleto = info.Nombre,
'                    .Tipo = "Viáticos",
'                    .Anio = CInt(h.Anio),
'                    .Mes = CInt(h.Mes),
'                    .Fecha = CType(Nothing, Date?),
'                    .Minutos = CType(Nothing, Integer?),
'                    .Dias = CType(Nothing, Integer?),
'                    .Incidencia = h.Incidencia,
'                    .Observaciones = CType(Nothing, String),
'                    .Motivo = h.Motivo,
'                    .Area = CType(Nothing, String)
'                })
'                End If
'            Next
'        End If

'        ' --- Custodias ---
'        If Not tipo.HasValue OrElse tipo.Value = TipoHistorico.Custodias Then
'            Dim custList As IEnumerable(Of HistoricoCustodia)
'            If idPolicia.HasValue Then
'                custList = Await uow.Repository(Of HistoricoCustodia)().FindAsync(
'                Function(h) h.FuncionarioId = idPolicia.Value)
'            Else
'                custList = Await uow.Repository(Of HistoricoCustodia)().GetAllByPredicateAsync(
'                Function(h) True)
'            End If

'            For Each h In custList
'                Dim a As Integer = h.Fecha.Year
'                Dim m As Integer = h.Fecha.Month
'                If (Not anio.HasValue OrElse a = anio.Value) AndAlso
'               (Not mes.HasValue OrElse m = mes.Value) Then
'                    Dim info = getFuncionarioInfo(h.FuncionarioId)
'                    datos.Add(New With {
'                    .FuncionarioId = h.FuncionarioId,
'                    .Cedula = info.CI,
'                    .NombreCompleto = info.Nombre,
'                    .Tipo = "Custodias",
'                    .Anio = a,
'                    .Mes = m,
'                    .Fecha = CType(h.Fecha, Date?),
'                    .Minutos = CType(Nothing, Integer?),
'                    .Dias = CType(Nothing, Integer?),
'                    .Incidencia = CType(Nothing, String),
'                    .Observaciones = CType(Nothing, String),
'                    .Motivo = CType(Nothing, String),
'                    .Area = h.Area
'                })
'                End If
'            Next
'        End If

'        ' Construye el DataTable con todas las columnas (FuncionarioId, Cedula, NombreCompleto, Tipo, Anio, Mes, Fecha, Minutos, Dias, Incidencia, Observaciones, Motivo, Area)
'        Return ModuloExtensions.ToDataTable(datos)
'    End Function

'    Private Async Function CargarMetadatosFuncionariosAsync(uow As UnitOfWork, funcionarioIds As IEnumerable(Of Integer?)) As Task(Of Dictionary(Of Integer, FuncionarioReporteMetadata))
'        If funcionarioIds Is Nothing Then
'            Return New Dictionary(Of Integer, FuncionarioReporteMetadata)()
'        End If

'        Dim ids = funcionarioIds.
'            Where(Function(id) id.HasValue AndAlso id.Value > 0).
'            Select(Function(id) id.Value).
'            Distinct().
'            ToList()

'        If ids.Count = 0 Then
'            Return New Dictionary(Of Integer, FuncionarioReporteMetadata)()
'        End If

'        Dim query = uow.Context.Set(Of Funcionario)().AsNoTracking() _
'            .Where(Function(f) ids.Contains(f.Id)) _
'            .Include(Function(f) f.Cargo) _
'            .Include(Function(f) f.TipoFuncionario) _
'            .Include(Function(f) f.Seccion) _
'            .Include(Function(f) f.Escalafon) _
'            .Include(Function(f) f.SubEscalafon) _
'            .Include(Function(f) f.SubDireccion) _
'            .Include(Function(f) f.Funcion) _
'            .Include(Function(f) f.PuestoTrabajo) _
'            .Include(Function(f) f.Turno) _
'            .Include(Function(f) f.Semana) _
'            .Include(Function(f) f.Horario) _
'            .Include(Function(f) f.PrestadorSalud)

'        Dim funcionarios = Await query.ToListAsync()

'        Return funcionarios.ToDictionary(Function(f) f.Id, Function(f) ConstruirMetadatosFuncionario(f))
'    End Function

'    Private Function ObtenerMetadatosFuncionario(metadata As IDictionary(Of Integer, FuncionarioReporteMetadata), funcionarioId As Integer?) As FuncionarioReporteMetadata
'        If metadata Is Nothing OrElse Not funcionarioId.HasValue Then
'            Return CrearMetadatosFuncionarioVacios()
'        End If

'        Dim result As FuncionarioReporteMetadata = Nothing
'        If metadata.TryGetValue(funcionarioId.Value, result) Then
'            Return result
'        End If

'        Return CrearMetadatosFuncionarioVacios()
'    End Function

'    Private Function ConstruirMetadatosFuncionario(funcionario As Funcionario) As FuncionarioReporteMetadata
'        If funcionario Is Nothing Then
'            Return CrearMetadatosFuncionarioVacios()
'        End If

'        Return New FuncionarioReporteMetadata With {
'            .TipoDeFuncionario = NormalizarValorReporte(If(funcionario.TipoFuncionario IsNot Nothing, funcionario.TipoFuncionario.Nombre, Nothing)),
'            .Cargo = NormalizarValorReporte(If(funcionario.Cargo IsNot Nothing, funcionario.Cargo.Nombre, Nothing)),
'            .Seccion = NormalizarValorReporte(If(funcionario.Seccion IsNot Nothing, funcionario.Seccion.Nombre, Nothing)),
'            .Escalafon = NormalizarValorReporte(If(funcionario.Escalafon IsNot Nothing, funcionario.Escalafon.Nombre, Nothing)),
'            .SubEscalafon = NormalizarValorReporte(If(funcionario.SubEscalafon IsNot Nothing, funcionario.SubEscalafon.Nombre, Nothing)),
'            .SubDireccion = NormalizarValorReporte(If(funcionario.SubDireccion IsNot Nothing, funcionario.SubDireccion.Nombre, Nothing)),
'            .Funcion = NormalizarValorReporte(If(funcionario.Funcion IsNot Nothing, funcionario.Funcion.Nombre, Nothing)),
'            .PuestoDeTrabajo = NormalizarValorReporte(If(funcionario.PuestoTrabajo IsNot Nothing, funcionario.PuestoTrabajo.Nombre, Nothing)),
'            .Turno = NormalizarValorReporte(If(funcionario.Turno IsNot Nothing, funcionario.Turno.Nombre, Nothing)),
'            .Semana = NormalizarValorReporte(If(funcionario.Semana IsNot Nothing, funcionario.Semana.Nombre, Nothing)),
'            .Horario = NormalizarValorReporte(If(funcionario.Horario IsNot Nothing, funcionario.Horario.Nombre, Nothing)),
'            .PrestadorSalud = NormalizarValorReporte(If(funcionario.PrestadorSalud IsNot Nothing, funcionario.PrestadorSalud.Nombre, Nothing)),
'            .EstadoFuncionario = If(funcionario.Activo, "Activo", "Inactivo"),
'            .Activo = funcionario.Activo
'        }
'    End Function

'    Private Function CrearMetadatosFuncionarioVacios() As FuncionarioReporteMetadata
'        Return New FuncionarioReporteMetadata With {
'            .TipoDeFuncionario = NormalizarValorReporte(Nothing),
'            .Cargo = NormalizarValorReporte(Nothing),
'            .Seccion = NormalizarValorReporte(Nothing),
'            .Escalafon = NormalizarValorReporte(Nothing),
'            .SubEscalafon = NormalizarValorReporte(Nothing),
'            .SubDireccion = NormalizarValorReporte(Nothing),
'            .Funcion = NormalizarValorReporte(Nothing),
'            .PuestoDeTrabajo = NormalizarValorReporte(Nothing),
'            .Turno = NormalizarValorReporte(Nothing),
'            .Semana = NormalizarValorReporte(Nothing),
'            .Horario = NormalizarValorReporte(Nothing),
'            .PrestadorSalud = NormalizarValorReporte(Nothing),
'            .EstadoFuncionario = NormalizarValorReporte("Sin Estado"),
'            .Activo = False
'        }
'    End Function

'    Private Function NormalizarValorReporte(value As String, Optional valorDefecto As String = "N/A") As String
'        If String.IsNullOrWhiteSpace(value) Then
'            Return valorDefecto
'        End If

'        Return value.Trim()
'    End Function

'    Private Function CalcularCantidadFuncionarios(funcionariosCadena As String) As Integer
'        If String.IsNullOrWhiteSpace(funcionariosCadena) Then
'            Return 0
'        End If

'        Dim tokens = funcionariosCadena.Split(New Char() {","c, ";"c, ControlChars.Lf, ControlChars.Cr, "|"c}, StringSplitOptions.RemoveEmptyEntries)

'        Return tokens.Select(Function(t) t.Trim()).Count(Function(t) Not String.IsNullOrWhiteSpace(t))
'    End Function

'    Private Function ConstruirMapaFuncionariosPorNovedad(detalles As IEnumerable(Of vw_NovedadesCompletas)) As IDictionary(Of Integer, String)
'        Dim resultado As New Dictionary(Of Integer, String)()

'        If detalles Is Nothing Then
'            Return resultado
'        End If

'        For Each grupo In detalles.Where(Function(d) d IsNot Nothing).GroupBy(Function(d) d.Id)
'            Dim etiquetas As New List(Of String)()
'            Dim yaIncluidos As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

'            For Each detalle In grupo
'                Dim etiqueta = FormatearFuncionarioParaLista(detalle.NombreFuncionario, detalle.CI)
'                If String.IsNullOrWhiteSpace(etiqueta) Then Continue For
'                If yaIncluidos.Add(etiqueta) Then
'                    etiquetas.Add(etiqueta)
'                End If
'            Next

'            If etiquetas.Count > 0 Then
'                resultado(grupo.Key) = String.Join("; ", etiquetas)
'            End If
'        Next

'        Return resultado
'    End Function

'    Private Function ObtenerFuncionariosDetallados(mapa As IDictionary(Of Integer, String), novedadId As Integer) As String
'        If mapa Is Nothing Then
'            Return String.Empty
'        End If

'        Dim valor As String = Nothing
'        If mapa.TryGetValue(novedadId, valor) Then
'            Return valor
'        End If

'        Return String.Empty
'    End Function

'    Private Function FormatearFuncionarioParaLista(nombre As String, cedula As String) As String
'        Dim nombreLimpio = NormalizarValorReporte(nombre, String.Empty)
'        Dim cedulaLimpia = NormalizarValorReporte(cedula, String.Empty)

'        Dim partes As New List(Of String)()

'        If Not String.IsNullOrWhiteSpace(nombreLimpio) Then
'            partes.Add(nombreLimpio)
'        End If

'        If Not String.IsNullOrWhiteSpace(cedulaLimpia) Then
'            If partes.Count > 0 Then
'                partes.Add($"({cedulaLimpia})")
'            Else
'                partes.Add(cedulaLimpia)
'            End If
'        End If

'        Return String.Join(" ", partes).Trim()
'    End Function

'    Private Function CrearTablaNovedadesVacia() As DataTable
'        Dim table As New DataTable()

'        table.Columns.Add("NovedadId", GetType(Integer))
'        table.Columns.Add("Fecha", GetType(Date))
'        table.Columns.Add("Resumen", GetType(String))
'        table.Columns.Add("Texto", GetType(String))
'        table.Columns.Add("Estado", GetType(String))
'        table.Columns.Add("FuncionariosLista", GetType(String))
'        table.Columns.Add("CantidadFuncionarios", GetType(Integer))
'        table.Columns.Add("FuncionarioId", GetType(Integer))
'        table.Columns.Add("FuncionarioNombre", GetType(String))
'        table.Columns.Add("FuncionarioCedula", GetType(String))
'        table.Columns.Add("TipoDeFuncionario", GetType(String))
'        table.Columns.Add("Cargo", GetType(String))
'        table.Columns.Add("Seccion", GetType(String))
'        table.Columns.Add("Escalafon", GetType(String))
'        table.Columns.Add("SubEscalafon", GetType(String))
'        table.Columns.Add("SubDireccion", GetType(String))
'        table.Columns.Add("Funcion", GetType(String))
'        table.Columns.Add("PuestoDeTrabajo", GetType(String))
'        table.Columns.Add("Turno", GetType(String))
'        table.Columns.Add("Semana", GetType(String))
'        table.Columns.Add("Horario", GetType(String))
'        table.Columns.Add("PrestadorSalud", GetType(String))
'        table.Columns.Add("EstadoFuncionario", GetType(String))
'        table.Columns.Add("Activo", GetType(Boolean))

'        Return table
'    End Function

'    Private Class FuncionarioReporteMetadata
'        Public Property TipoDeFuncionario As String
'        Public Property Cargo As String
'        Public Property Seccion As String
'        Public Property Escalafon As String
'        Public Property SubEscalafon As String
'        Public Property SubDireccion As String
'        Public Property Funcion As String
'        Public Property PuestoDeTrabajo As String
'        Public Property Turno As String
'        Public Property Semana As String
'        Public Property Horario As String
'        Public Property PrestadorSalud As String
'        Public Property EstadoFuncionario As String
'        Public Property Activo As Boolean
'    End Class

'    Private Function CrearTablaNotificacionesVacia() As DataTable
'        Dim table As New DataTable()

'        table.Columns.Add("NombreCompleto", GetType(String))
'        table.Columns.Add("Cedula", GetType(String))
'        table.Columns.Add("TipoNotificacion", GetType(String))
'        table.Columns.Add("Estado", GetType(String))
'        table.Columns.Add("FechaProgramada", GetType(Date))
'        table.Columns.Add("Texto", GetType(String))
'        table.Columns.Add("Documento", GetType(String))
'        table.Columns.Add("ExpMinisterial", GetType(String))
'        table.Columns.Add("ExpINR", GetType(String))
'        table.Columns.Add("Oficina", GetType(String))
'        table.Columns.Add("TipoDeFuncionario", GetType(String))
'        table.Columns.Add("Cargo", GetType(String))
'        table.Columns.Add("Seccion", GetType(String))
'        table.Columns.Add("Escalafon", GetType(String))
'        table.Columns.Add("SubEscalafon", GetType(String))
'        table.Columns.Add("SubDireccion", GetType(String))
'        table.Columns.Add("Funcion", GetType(String))
'        table.Columns.Add("PuestoDeTrabajo", GetType(String))
'        table.Columns.Add("Turno", GetType(String))
'        table.Columns.Add("Semana", GetType(String))
'        table.Columns.Add("Horario", GetType(String))
'        table.Columns.Add("PrestadorSalud", GetType(String))
'        table.Columns.Add("EstadoFuncionario", GetType(String))
'        table.Columns.Add("Activo", GetType(Boolean))

'        Return table
'    End Function
'End Module
