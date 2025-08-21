Imports System.Data.Entity

''' <summary>
''' Servicio para obtener los datos del informe de Concepto Funcional.
''' VERSIÓN FINAL: Corregida para resolver errores de LINQ y usa la propiedad 'Activo' para determinar el estado de la sanción.
''' </summary>
Public Class ConceptoFuncionalService
    Private ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New(unitOfWork As IUnitOfWork)
        _unitOfWork = unitOfWork
    End Sub

    ''' <summary>
    ''' Obtiene incidencias (licencias médicas o sanciones leves) de un funcionario.
    ''' </summary>
    Public Function ObtenerIncidencias(
        funcionarioId As Integer,
        fechaInicio As DateTime,
        fechaFin As DateTime,
        categoria As String
    ) As IEnumerable(Of IncidenciaUI)

        ' 1. Se traen los datos filtrados desde la base de datos a memoria.
        Dim licenciasDb = _unitOfWork.Repository(Of HistoricoLicencia).Find(
            Function(lic)
                Return lic.FuncionarioId = funcionarioId AndAlso
                       lic.TipoLicencia.CategoriaAusencia.Nombre.Equals(categoria, StringComparison.OrdinalIgnoreCase) AndAlso
                       DbFunctions.TruncateTime(lic.inicio) >= fechaInicio.Date AndAlso
                       DbFunctions.TruncateTime(lic.inicio) <= fechaFin.Date
            End Function
        ).ToList()

        ' 2. Se transforman los datos al formato de la UI, ya en memoria.
        Return licenciasDb.Select(
            Function(l) New IncidenciaUI With {
                .FechaInicio = l.inicio,
                .FechaFinal = l.finaliza,
                .Tipo = l.TipoLicencia.Nombre,
                .Observaciones = l.datos
            })
    End Function

    ''' <summary>
    ''' Obtiene y combina las observaciones (sanciones puntuales y leves) de un funcionario.
    ''' </summary>
    Public Function ObtenerObservaciones(
        funcionarioId As Integer,
        fechaInicio As DateTime,
        fechaFin As DateTime
    ) As List(Of ObservacionUI)

        ' --- 1. Obtener sanciones PUNTUALES ---
        ' Se filtra en la DB y se trae la colección a memoria con ToList().
        Dim estadosTransitoriosDb = _unitOfWork.Repository(Of EstadoTransitorio).Find(
            Function(et)
                Return et.FuncionarioId = funcionarioId AndAlso
                       et.TipoEstadoTransitorio.Nombre.Equals(ModConstantesApex.CATEGORIA_ESTADO_SANCION, StringComparison.OrdinalIgnoreCase) AndAlso
                       et.SancionDetalle IsNot Nothing AndAlso
                       DbFunctions.TruncateTime(et.SancionDetalle.FechaDesde) >= fechaInicio.Date AndAlso
                       DbFunctions.TruncateTime(et.SancionDetalle.FechaDesde) <= fechaFin.Date
            End Function
        ).ToList()

        ' Se mapea la colección en memoria para evitar errores de conversión a SQL.
        Dim puntuales = estadosTransitoriosDb.Select(
            Function(et)
                ' CORRECCIÓN DEFINITIVA: Se usa la propiedad booleana 'Activo' que es más simple y directa.
                Dim estadoSancion As String = If(et.Activo, "Abierta", "Cerrada")

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