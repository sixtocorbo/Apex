' Apex/Services/ConceptoFuncionalService.vb
Imports System.Data.Entity

''' <summary>
''' Clase DTO (Objeto de Transferencia de Datos) unificada para mostrar todas las incidencias en el reporte.
''' ESTA ES LA ESTRUCTURA CORRECTA QUE EL REPORTE DEBE USAR EN TODOS SUS DATASETS.
''' </summary>
Public Class ConceptoFuncionalItem
    Public Property FechaInicio As DateTime
    Public Property FechaFinal As Date?
    Public Property Tipo As String
    Public Property Observaciones As String
    Public Property Origen As String
End Class

Public Class ConceptoFuncionalService
    Private ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New(unitOfWork As IUnitOfWork)
        _unitOfWork = unitOfWork
    End Sub

    Public Function ObtenerIncidenciasDeSalud(funcionarioId As Integer, fechaInicio As DateTime, fechaFin As DateTime) As List(Of ConceptoFuncionalItem)
        ' 1. Obtener Licencias de categoría "Salud"
        Dim licenciasSalud = _unitOfWork.Repository(Of HistoricoLicencia).GetAll().
            Where(Function(lic) lic.FuncionarioId = funcionarioId AndAlso
                                 lic.TipoLicencia.CategoriaAusencia.Nombre.Trim().ToLower() = "salud" AndAlso
                                 DbFunctions.TruncateTime(lic.inicio) >= fechaInicio.Date AndAlso
                                 DbFunctions.TruncateTime(lic.inicio) <= fechaFin.Date
            ).Select(Function(l) New ConceptoFuncionalItem With {
                .FechaInicio = l.inicio,
                .FechaFinal = l.finaliza,
                .Tipo = l.TipoLicencia.Nombre,
                .Observaciones = l.datos,
                .Origen = "Licencia"
            }).ToList()

        ' 2. Obtener Estados Transitorios de tipo "Enfermedad"
        Dim estadosEnfermedad = _unitOfWork.Repository(Of EstadoTransitorio).GetAll().
            Where(Function(et) et.FuncionarioId = funcionarioId AndAlso
                                 et.TipoEstadoTransitorio.Nombre.Trim().ToLower() = "enfermedad" AndAlso
                                 et.EnfermedadDetalle IsNot Nothing AndAlso
                                 DbFunctions.TruncateTime(et.EnfermedadDetalle.FechaDesde) >= fechaInicio.Date AndAlso
                                 DbFunctions.TruncateTime(et.EnfermedadDetalle.FechaDesde) <= fechaFin.Date
            ).Select(Function(et) New ConceptoFuncionalItem With {
                .FechaInicio = et.EnfermedadDetalle.FechaDesde,
                .FechaFinal = et.EnfermedadDetalle.FechaHasta,
                .Tipo = et.TipoEstadoTransitorio.Nombre,
                .Observaciones = et.EnfermedadDetalle.Observaciones,
                .Origen = "Estado Transitorio"
            }).ToList()

        Return licenciasSalud.Concat(estadosEnfermedad).OrderBy(Function(i) i.FechaInicio).ToList()
    End Function

    Public Function ObtenerSancionesGraves(funcionarioId As Integer, fechaInicio As DateTime, fechaFin As DateTime) As List(Of ConceptoFuncionalItem)
        Return _unitOfWork.Repository(Of HistoricoLicencia).GetAll().
            Where(Function(lic) lic.FuncionarioId = funcionarioId AndAlso
                                 lic.TipoLicencia.CategoriaAusencia.Nombre.Trim().ToLower() = "sanciongrave" AndAlso
                                 DbFunctions.TruncateTime(lic.inicio) >= fechaInicio.Date AndAlso
                                 DbFunctions.TruncateTime(lic.inicio) <= fechaFin.Date
            ).Select(Function(l) New ConceptoFuncionalItem With {
                .FechaInicio = l.inicio,
                .FechaFinal = l.finaliza,
                .Tipo = l.TipoLicencia.Nombre,
                .Observaciones = l.datos,
                .Origen = "Sanción Grave"
            }).OrderBy(Function(i) i.FechaInicio).ToList()
    End Function

    Public Function ObtenerObservacionesYLeves(funcionarioId As Integer, fechaInicio As DateTime, fechaFin As DateTime) As List(Of ConceptoFuncionalItem)
        ' 1. Obtener Sanciones Leves
        Dim sancionesLeves = _unitOfWork.Repository(Of HistoricoLicencia).GetAll().
            Where(Function(lic) lic.FuncionarioId = funcionarioId AndAlso
                                 lic.TipoLicencia.CategoriaAusencia.Nombre.Trim().ToLower() = "sancionleve" AndAlso
                                 DbFunctions.TruncateTime(lic.inicio) >= fechaInicio.Date AndAlso
                                 DbFunctions.TruncateTime(lic.inicio) <= fechaFin.Date
            ).Select(Function(l) New ConceptoFuncionalItem With {
                .FechaInicio = l.inicio,
                .FechaFinal = l.finaliza,
                .Tipo = l.TipoLicencia.Nombre,
                .Observaciones = l.datos,
                .Origen = "Sanción Leve"
            }).ToList()

        ' 2. Obtener Estados Transitorios de tipo "Sanción"
        Dim estadosSancion = _unitOfWork.Repository(Of EstadoTransitorio).GetAll().
            Where(Function(et) et.FuncionarioId = funcionarioId AndAlso
                                 et.TipoEstadoTransitorio.Nombre.Trim().ToLower() = "sanción" AndAlso
                                 et.SancionDetalle IsNot Nothing AndAlso
                                 DbFunctions.TruncateTime(et.SancionDetalle.FechaDesde) >= fechaInicio.Date AndAlso
                                 DbFunctions.TruncateTime(et.SancionDetalle.FechaDesde) <= fechaFin.Date
            ).Select(Function(et) New ConceptoFuncionalItem With {
                .FechaInicio = et.SancionDetalle.FechaDesde,
                .FechaFinal = et.SancionDetalle.FechaHasta,
                .Tipo = et.TipoEstadoTransitorio.Nombre,
                .Observaciones = et.SancionDetalle.Observaciones,
                .Origen = "Observación"
            }).ToList()

        Return sancionesLeves.Concat(estadosSancion).OrderBy(Function(i) i.FechaInicio).ToList()
    End Function

End Class