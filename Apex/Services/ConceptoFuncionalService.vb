Option Strict On
Option Infer On
Imports System.Data.Entity

''' <summary>
''' DTO unificado para el reporte/consola de conceptos funcionales.
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

    ' Claves normalizadas para TipoEstadoTransitorio (no tenemos IDs canónicos aquí)
    Private Const KEY_ENFERMEDAD As String = "enfermedad"
    Private Const KEY_SANCION As String = "sancion"

    ''' <summary>
    ''' Incidencias de salud: Licencias (cat SALUD) + Estados Transitorios "Enfermedad".
    ''' </summary>
    Public Function ObtenerIncidenciasDeSalud(funcionarioId As Integer, fechaInicio As DateTime, fechaFin As DateTime) As List(Of ConceptoFuncionalItem)
        Dim desde = fechaInicio.Date
        Dim hastaExcl = fechaFin.Date.AddDays(1) ' límite superior exclusivo (DateTime)

        ' 1) Licencias categoría SALUD con solapamiento en el período
        Dim licenciasSalud = _unitOfWork.Repository(Of HistoricoLicencia)().GetAll() _
            .AsNoTracking() _
            .Where(Function(lic) lic.FuncionarioId = funcionarioId _
                               AndAlso lic.TipoLicencia.CategoriaAusenciaId = ModConstantesApex.CategoriaAusenciaId.Salud _
                               AndAlso lic.inicio < hastaExcl AndAlso lic.finaliza >= desde) _
            .Select(Function(l) New ConceptoFuncionalItem With {
                .FechaInicio = l.inicio,
                .FechaFinal = l.finaliza,
                .Tipo = l.TipoLicencia.Nombre,
                .Observaciones = l.datos,
                .Origen = "Licencia"
            }) _
            .ToList()

        ' 2) Estados Transitorios tipo "Enfermedad" con solapamiento
        Dim estadosBase = _unitOfWork.Repository(Of EstadoTransitorio)().GetAll() _
            .AsNoTracking() _
            .Where(Function(et) et.FuncionarioId = funcionarioId _
                               AndAlso et.EnfermedadDetalle IsNot Nothing _
                               AndAlso et.EnfermedadDetalle.FechaDesde < hastaExcl _
                               AndAlso (Not et.EnfermedadDetalle.FechaHasta.HasValue OrElse et.EnfermedadDetalle.FechaHasta.Value >= desde)) _
            .Select(Function(et) New With {
                .Tipo = et.TipoEstadoTransitorio.Nombre,
                .Desde = et.EnfermedadDetalle.FechaDesde,
                .Hasta = et.EnfermedadDetalle.FechaHasta,
                .Obs = et.EnfermedadDetalle.Observaciones
            }) _
            .ToList()

        Dim estadosEnfermedad = estadosBase _
            .Where(Function(x) ModConstantesApex.Normalizar(x.Tipo) = KEY_ENFERMEDAD) _
            .Select(Function(x) New ConceptoFuncionalItem With {
                .FechaInicio = x.Desde,
                .FechaFinal = x.Hasta,
                .Tipo = "Enfermedad",
                .Observaciones = x.Obs,
                .Origen = "Estado Transitorio"
            }) _
            .ToList()

        Return licenciasSalud.Concat(estadosEnfermedad) _
               .OrderBy(Function(i) i.FechaInicio) _
               .ToList()
    End Function

    ''' <summary>
    ''' Sanciones Graves: sólo licencias cuya categoría es SANCION_GRAVE.
    ''' </summary>
    Public Function ObtenerSancionesGraves(funcionarioId As Integer, fechaInicio As DateTime, fechaFin As DateTime) As List(Of ConceptoFuncionalItem)
        Dim desde = fechaInicio.Date
        Dim hastaExcl = fechaFin.Date.AddDays(1)

        Return _unitOfWork.Repository(Of HistoricoLicencia)().GetAll() _
            .AsNoTracking() _
            .Where(Function(lic) lic.FuncionarioId = funcionarioId _
                               AndAlso lic.TipoLicencia.CategoriaAusenciaId = ModConstantesApex.CategoriaAusenciaId.SancionGrave _
                               AndAlso lic.inicio < hastaExcl AndAlso lic.finaliza >= desde) _
            .Select(Function(l) New ConceptoFuncionalItem With {
                .FechaInicio = l.inicio,
                .FechaFinal = l.finaliza,
                .Tipo = l.TipoLicencia.Nombre,
                .Observaciones = l.datos,
                .Origen = "Sanción Grave"
            }) _
            .OrderBy(Function(i) i.FechaInicio) _
            .ToList()
    End Function

    ''' <summary>
    ''' Observaciones y Leves: Licencias SANCION_LEVE + Estados Transitorios de tipo "Sanción".
    ''' </summary>
    Public Function ObtenerObservacionesYLeves(funcionarioId As Integer, fechaInicio As DateTime, fechaFin As DateTime) As List(Of ConceptoFuncionalItem)
        Dim desde = fechaInicio.Date
        Dim hastaExcl = fechaFin.Date.AddDays(1)

        ' 1) Licencias categoría SANCION_LEVE
        Dim sancionesLeves = _unitOfWork.Repository(Of HistoricoLicencia)().GetAll() _
            .AsNoTracking() _
            .Where(Function(lic) lic.FuncionarioId = funcionarioId _
                               AndAlso lic.TipoLicencia.CategoriaAusenciaId = ModConstantesApex.CategoriaAusenciaId.SancionLeve _
                               AndAlso lic.inicio < hastaExcl AndAlso lic.finaliza >= desde) _
            .Select(Function(l) New ConceptoFuncionalItem With {
                .FechaInicio = l.inicio,
                .FechaFinal = l.finaliza,
                .Tipo = l.TipoLicencia.Nombre,
                .Observaciones = l.datos,
                .Origen = "Sanción Leve"
            }) _
            .ToList()

        ' 2) Estados Transitorios tipo "Sanción"
        Dim estadosSancionBase = _unitOfWork.Repository(Of EstadoTransitorio)().GetAll() _
            .AsNoTracking() _
            .Where(Function(et) et.FuncionarioId = funcionarioId _
                               AndAlso et.SancionDetalle IsNot Nothing _
                               AndAlso et.SancionDetalle.FechaDesde < hastaExcl _
                               AndAlso (Not et.SancionDetalle.FechaHasta.HasValue OrElse et.SancionDetalle.FechaHasta.Value >= desde)) _
            .Select(Function(et) New With {
                .Tipo = et.TipoEstadoTransitorio.Nombre,
                .Desde = et.SancionDetalle.FechaDesde,
                .Hasta = et.SancionDetalle.FechaHasta,
                .Obs = et.SancionDetalle.Observaciones
            }) _
            .ToList()

        Dim estadosSancion = estadosSancionBase _
            .Where(Function(x) ModConstantesApex.Normalizar(x.Tipo) = KEY_SANCION) _
            .Select(Function(x) New ConceptoFuncionalItem With {
                .FechaInicio = x.Desde,
                .FechaFinal = x.Hasta,
                .Tipo = "Sanción",
                .Observaciones = x.Obs,
                .Origen = "Observación"
            }) _
            .ToList()

        Return sancionesLeves.Concat(estadosSancion) _
               .OrderBy(Function(i) i.FechaInicio) _
               .ToList()
    End Function

End Class
