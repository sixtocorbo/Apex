' Apex/Services/EstadoTransitorioService.vb
Imports System.Data.Entity

Public Class EstadoTransitorioService
    Inherits GenericService(Of EstadoTransitorio)

    Private Shadows ReadOnly _unitOfWork As IUnitOfWork

    ' Este es el constructor original que se usa en otras partes de la app.
    Public Sub New()
        MyBase.New(New UnitOfWork())
        _unitOfWork = MyBase._unitOfWork
    End Sub

    ' --- CONSTRUCTOR AÑADIDO ---
    ' Este nuevo constructor acepta una UnitOfWork existente,
    ' que es lo que necesitas en frmFuncionarioCrear.
    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
        _unitOfWork = unitOfWork
    End Sub
    ' --- FIN DE LA CORRECCIÓN ---

    ''' <summary>
    ''' Obtiene una lista de estados transitorios desde la vista, con filtros.
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(fechaInicio As Date, fechaFin As Date, Optional tipoEstado As String = "") As Task(Of List(Of vw_EstadosTransitoriosCompletos))
        Dim query = _unitOfWork.Repository(Of vw_EstadosTransitoriosCompletos)().GetAll().AsNoTracking()

        ' Filtro por rango de fechas
        query = query.Where(Function(et) et.FechaDesde >= fechaInicio And et.FechaDesde <= fechaFin)

        ' Filtro opcional por nombre del tipo de estado
        If Not String.IsNullOrWhiteSpace(tipoEstado) Then
            query = query.Where(Function(et) et.TipoEstadoNombre = tipoEstado)
        End If

        Return Await query.OrderByDescending(Function(et) et.FechaDesde).ToListAsync()
    End Function

    ''' <summary>
    ''' Elimina un EstadoTransitorio y todas sus dependencias (detalles y adjuntos) para evitar FKs no anulables.
    ''' </summary>
    Public Async Function EliminarEstadoTransitorioSeguroAsync(estadoId As Integer) As Task(Of Boolean)
        ' Cargamos el estado con TODOS los detalles y adjuntos
        Dim repoEstado = _unitOfWork.Repository(Of EstadoTransitorio)()
        Dim estado = Await repoEstado.GetAll().
            Include(Function(e) e.DesignacionDetalle).
            Include(Function(e) e.EnfermedadDetalle).
            Include(Function(e) e.SancionDetalle).
            Include(Function(e) e.OrdenCincoDetalle).
            Include(Function(e) e.RetenDetalle).
            Include(Function(e) e.SumarioDetalle).
            Include(Function(e) e.TrasladoDetalle).
            Include(Function(e) e.BajaDeFuncionarioDetalle).
            Include(Function(e) e.CambioDeCargoDetalle).
            Include(Function(e) e.ReactivacionDeFuncionarioDetalle).
            Include(Function(e) e.SeparacionDelCargoDetalle).
            Include(Function(e) e.InicioDeProcesamientoDetalle).
            Include(Function(e) e.DesarmadoDetalle).
            Include(Function(e) e.AdjuntosNuevos).SingleOrDefaultAsync(Function(x) x.Id = estadoId)

        If estado Is Nothing Then Return False

        ' 1) Borrar adjuntos (si tu entidad se llama EstadoTransitorioAdjunto)
        If estado.AdjuntosNuevos IsNot Nothing AndAlso estado.AdjuntosNuevos.Any() Then
            Dim repoAdj = _unitOfWork.Repository(Of EstadoTransitorioAdjunto)()
            For Each a In estado.AdjuntosNuevos.ToList()
                repoAdj.Remove(a)
            Next
        End If

        ' 2) Borrar cada detalle si existe
        Dim repoDesignacion = _unitOfWork.Repository(Of DesignacionDetalle)()
        Dim repoEnfermedad = _unitOfWork.Repository(Of EnfermedadDetalle)()
        Dim repoSancion = _unitOfWork.Repository(Of SancionDetalle)()
        Dim repoOrdenCinco = _unitOfWork.Repository(Of OrdenCincoDetalle)()
        Dim repoReten = _unitOfWork.Repository(Of RetenDetalle)()
        Dim repoSumario = _unitOfWork.Repository(Of SumarioDetalle)()
        Dim repoTraslado = _unitOfWork.Repository(Of TrasladoDetalle)()
        Dim repoBaja = _unitOfWork.Repository(Of BajaDeFuncionarioDetalle)()
        Dim repoCambio = _unitOfWork.Repository(Of CambioDeCargoDetalle)()
        Dim repoReactivacion = _unitOfWork.Repository(Of ReactivacionDeFuncionarioDetalle)()
        Dim repoSeparacion = _unitOfWork.Repository(Of SeparacionDelCargoDetalle)()
        Dim repoProcesamiento = _unitOfWork.Repository(Of InicioDeProcesamientoDetalle)()
        Dim repoDesarmado = _unitOfWork.Repository(Of DesarmadoDetalle)()

        If estado.DesignacionDetalle IsNot Nothing Then repoDesignacion.Remove(estado.DesignacionDetalle)
        If estado.EnfermedadDetalle IsNot Nothing Then repoEnfermedad.Remove(estado.EnfermedadDetalle)
        If estado.SancionDetalle IsNot Nothing Then repoSancion.Remove(estado.SancionDetalle)
        If estado.OrdenCincoDetalle IsNot Nothing Then repoOrdenCinco.Remove(estado.OrdenCincoDetalle)
        If estado.RetenDetalle IsNot Nothing Then repoReten.Remove(estado.RetenDetalle)
        If estado.SumarioDetalle IsNot Nothing Then repoSumario.Remove(estado.SumarioDetalle)
        If estado.TrasladoDetalle IsNot Nothing Then repoTraslado.Remove(estado.TrasladoDetalle)
        If estado.BajaDeFuncionarioDetalle IsNot Nothing Then repoBaja.Remove(estado.BajaDeFuncionarioDetalle)
        If estado.CambioDeCargoDetalle IsNot Nothing Then repoCambio.Remove(estado.CambioDeCargoDetalle)
        If estado.ReactivacionDeFuncionarioDetalle IsNot Nothing Then repoReactivacion.Remove(estado.ReactivacionDeFuncionarioDetalle)
        If estado.SeparacionDelCargoDetalle IsNot Nothing Then repoSeparacion.Remove(estado.SeparacionDelCargoDetalle)
        If estado.InicioDeProcesamientoDetalle IsNot Nothing Then repoProcesamiento.Remove(estado.InicioDeProcesamientoDetalle)
        If estado.DesarmadoDetalle IsNot Nothing Then repoDesarmado.Remove(estado.DesarmadoDetalle)

        ' 3) Finalmente, borrar el principal
        repoEstado.Remove(estado)

        ' 4) Guardar cambios
        Await _unitOfWork.CommitAsync()
        Return True
    End Function
End Class