' Apex/Services/EstadoTransitorioService.vb
Imports System.Data.Entity

Public Class EstadoTransitorioService
    Inherits GenericService(Of EstadoTransitorio)

    Private Shadows ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        MyBase.New(New UnitOfWork())
        _unitOfWork = MyBase._unitOfWork
    End Sub

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

End Class
