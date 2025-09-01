' Definimos la clase como PARCIAL. Esto la "une" a la clase que genera Entity Framework.
Partial Public Class EstadoTransitorio
    ' Esta propiedad personalizada NUNCA será borrada si regeneras el modelo,
    ' porque vive en su propio archivo.
    Public Property AdjuntosNuevos As New List(Of EstadoTransitorioAdjunto)
    ''' <summary>
    ''' Obtiene las fechas de inicio y fin del estado transitorio, consultando la tabla de detalle correspondiente.
    ''' </summary>
    ''' <param name="desde">Parámetro de salida para la fecha de inicio.</param>
    ''' <param name="hasta">Parámetro de salida para la fecha de fin.</param>
    Public Sub GetFechas(ByRef desde As Date?, ByRef hasta As Date?)
        Select Case Me.TipoEstadoTransitorioId
            Case TiposEstadoCatalog.Designacion
                desde = Me.DesignacionDetalle?.FechaDesde
                hasta = Me.DesignacionDetalle?.FechaHasta
            Case TiposEstadoCatalog.Enfermedad
                desde = Me.EnfermedadDetalle?.FechaDesde
                hasta = Me.EnfermedadDetalle?.FechaHasta
            Case TiposEstadoCatalog.Sancion
                desde = Me.SancionDetalle?.FechaDesde
                hasta = Me.SancionDetalle?.FechaHasta
            Case TiposEstadoCatalog.OrdenCinco
                desde = Me.OrdenCincoDetalle?.FechaDesde
                hasta = Me.OrdenCincoDetalle?.FechaHasta
            Case TiposEstadoCatalog.Reten      ' un solo día
                desde = Me.RetenDetalle?.FechaReten
                hasta = Me.RetenDetalle?.FechaReten
            Case TiposEstadoCatalog.Sumario
                desde = Me.SumarioDetalle?.FechaDesde
                hasta = Me.SumarioDetalle?.FechaHasta
            Case TiposEstadoCatalog.Traslado
                desde = Me.TrasladoDetalle?.FechaDesde
                hasta = Me.TrasladoDetalle?.FechaHasta
            Case Else
                desde = Nothing : hasta = Nothing
        End Select
    End Sub

    ''' <summary>
    ''' Devuelve un color distintivo para cada tipo de estado transitorio, para usar en la UI.
    ''' </summary>
    ''' <returns>Un objeto Color.</returns>
    Public Function GetColor() As Color
        Select Case Me.TipoEstadoTransitorioId
            Case TiposEstadoCatalog.Designacion : Return Color.LightSkyBlue
            Case TiposEstadoCatalog.Enfermedad : Return Color.LightCoral
            Case TiposEstadoCatalog.Sancion : Return Color.Khaki
            Case TiposEstadoCatalog.OrdenCinco : Return Color.Plum
            Case TiposEstadoCatalog.Reten : Return Color.LightGray
            Case TiposEstadoCatalog.Sumario : Return Color.LightSalmon
            Case TiposEstadoCatalog.Traslado : Return Color.LightGreen
            Case Else : Return Color.White
        End Select
    End Function

End Class
