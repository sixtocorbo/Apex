' Apex/Services/ConceptoFuncionalService.vb
Imports System.Data.Entity

''' <summary>
''' Servicio para obtener los datos del informe de Concepto Funcional.
''' </summary>
Public Class ConceptoFuncionalService
    Private ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New(unitOfWork As IUnitOfWork)
        _unitOfWork = unitOfWork
    End Sub

    ''' <summary>
    ''' Obtiene incidencias (licencias médicas o sanciones) de un funcionario.
    ''' </summary>
    Public Function ObtenerIncidencias(
        funcionarioId As Integer,
        fechaInicio As DateTime,
        fechaFin As DateTime,
        categoria As String
    ) As List(Of IncidenciaUI)

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Se añade una verificación de NULL y se simplifica la comparación de strings.
        Dim licenciasDb = _unitOfWork.Repository(Of HistoricoLicencia).GetAll().
            Where(
                Function(lic) lic.FuncionarioId = funcionarioId AndAlso
                       lic.TipoLicencia IsNot Nothing AndAlso
                       lic.TipoLicencia.CategoriaAusencia IsNot Nothing AndAlso
                       lic.TipoLicencia.CategoriaAusencia.Nombre = categoria AndAlso
                       DbFunctions.TruncateTime(lic.inicio) >= fechaInicio.Date AndAlso
                       DbFunctions.TruncateTime(lic.inicio) <= fechaFin.Date
            ).ToList()
        ' --- FIN DE LA CORRECCIÓN ---

        Return licenciasDb.Select(
            Function(l) New IncidenciaUI With {
                .FechaInicio = l.inicio,
                .FechaFinal = l.finaliza,
                .Tipo = l.TipoLicencia.Nombre,
                .Observaciones = l.datos
            }).ToList()
    End Function

    ''' <summary>
    ''' Obtiene y combina las observaciones (sanciones puntuales y leves) de un funcionario.
    ''' </summary>
    Public Function ObtenerObservaciones(
        funcionarioId As Integer,
        fechaInicio As DateTime,
        fechaFin As DateTime
    ) As List(Of ObservacionUI)

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Se añade una verificación de NULL y se simplifica la comparación de strings.
        Dim estadosTransitoriosDb = _unitOfWork.Repository(Of EstadoTransitorio).GetAll().
            Where(
                Function(et) et.FuncionarioId = funcionarioId AndAlso
                       et.TipoEstadoTransitorio IsNot Nothing AndAlso
                       et.TipoEstadoTransitorio.Nombre = ModConstantesApex.CATEGORIA_ESTADO_SANCION AndAlso
                       et.SancionDetalle IsNot Nothing AndAlso
                       DbFunctions.TruncateTime(et.SancionDetalle.FechaDesde) >= fechaInicio.Date AndAlso
                       DbFunctions.TruncateTime(et.SancionDetalle.FechaDesde) <= fechaFin.Date
            ).ToList()
        ' --- FIN DE LA CORRECCIÓN ---

        Dim puntuales = estadosTransitoriosDb.Select(
            Function(et)
                Dim estadoSancion As String = If(Not et.SancionDetalle.FechaHasta.HasValue OrElse et.SancionDetalle.FechaHasta.Value >= Date.Today, "Abierta", "Cerrada")
                Return New ObservacionUI With {
                    .Fecha = et.SancionDetalle.FechaDesde,
                    .Causa = et.SancionDetalle.Observaciones,
                    .Sancion = estadoSancion
                }
            End Function).ToList()

        ' --- 2. Obtener sanciones LEVES ---
        Dim leves = ObtenerIncidencias(funcionarioId, fechaInicio, fechaFin, ModConstantesApex.CATEGORIA_SANCION_LEVE) _
            .Select(Function(i) New ObservacionUI With {
                .Fecha = i.FechaInicio,
                .Causa = $"LEVE: {i.Tipo}",
                .Sancion = i.Observaciones
            }).ToList()

        ' --- 3. Combinar y ordenar los resultados ---
        Return puntuales.Concat(leves).OrderBy(Function(o) o.Fecha).ToList()
    End Function
End Class