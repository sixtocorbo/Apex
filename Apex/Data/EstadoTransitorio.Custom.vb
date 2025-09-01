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
            Case 1 ' Designación
                desde = Me.DesignacionDetalle?.FechaDesde
                hasta = Me.DesignacionDetalle?.FechaHasta
            Case 2 ' Enfermedad
                desde = Me.EnfermedadDetalle?.FechaDesde
                hasta = Me.EnfermedadDetalle?.FechaHasta
            Case 3 ' Sanción
                desde = Me.SancionDetalle?.FechaDesde
                hasta = Me.SancionDetalle?.FechaHasta
            Case 4 ' Orden Cinco
                desde = Me.OrdenCincoDetalle?.FechaDesde
                hasta = Me.OrdenCincoDetalle?.FechaHasta
            Case 5 ' Retén (es un solo día)
                desde = Me.RetenDetalle?.FechaReten
                hasta = Me.RetenDetalle?.FechaReten
            Case 6 ' Sumario
                desde = Me.SumarioDetalle?.FechaDesde
                hasta = Me.SumarioDetalle?.FechaHasta
            Case Else
                desde = Nothing
                hasta = Nothing
        End Select
    End Sub

    ''' <summary>
    ''' Devuelve un color distintivo para cada tipo de estado transitorio, para usar en la UI.
    ''' </summary>
    ''' <returns>Un objeto Color.</returns>
    Public Function GetColor() As Color
        Select Case Me.TipoEstadoTransitorioId
            Case 1 ' Designación
                Return Color.LightSkyBlue
            Case 2 ' Enfermedad
                Return Color.LightCoral
            Case 3 ' Sanción
                Return Color.Khaki
            Case 4 ' Orden Cinco
                Return Color.Plum
            Case 5 ' Retén
                Return Color.LightGray
            Case 6 ' Sumario
                Return Color.LightSalmon
            Case Else
                Return Color.White
        End Select
    End Function
End Class
