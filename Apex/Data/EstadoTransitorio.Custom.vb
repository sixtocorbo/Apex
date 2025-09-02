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
            Case 1 ' Designacion
                desde = Me.DesignacionDetalle?.FechaDesde
                hasta = Me.DesignacionDetalle?.FechaHasta
            Case 2 ' Enfermedad
                desde = Me.EnfermedadDetalle?.FechaDesde
                hasta = Me.EnfermedadDetalle?.FechaHasta
            Case 3 ' Sancion
                desde = Me.SancionDetalle?.FechaDesde
                hasta = Me.SancionDetalle?.FechaHasta
            Case 4 ' OrdenCinco
                desde = Me.OrdenCincoDetalle?.FechaDesde
                hasta = Me.OrdenCincoDetalle?.FechaHasta
            Case 5 ' Reten
                desde = Me.RetenDetalle?.FechaReten
                hasta = Me.RetenDetalle?.FechaReten
            Case 6 ' Sumario
                desde = Me.SumarioDetalle?.FechaDesde
                hasta = Me.SumarioDetalle?.FechaHasta
            Case 21 ' Traslado
                desde = Me.TrasladoDetalle?.FechaDesde
                hasta = Me.TrasladoDetalle?.FechaHasta

            ' --- NUEVOS CASOS AÑADIDOS ---
            Case 29 ' Baja de Funcionario
                desde = Me.BajaDeFuncionarioDetalle?.FechaDesde
                hasta = Me.BajaDeFuncionarioDetalle?.FechaHasta
            Case 30 ' Cambio de Cargo
                desde = Me.CambioDeCargoDetalle?.FechaDesde
                hasta = Me.CambioDeCargoDetalle?.FechaHasta
            Case 31 ' Reactivación de Funcionario
                desde = Me.ReactivacionDeFuncionarioDetalle?.FechaDesde
                hasta = Nothing ' Este estado no tiene fecha de fin
            Case 32 ' Separación del Cargo
                desde = Me.SeparacionDelCargoDetalle?.FechaDesde
                hasta = Me.SeparacionDelCargoDetalle?.FechaHasta
            Case 33 ' Inicio de Procesamiento
                desde = Me.InicioDeProcesamientoDetalle?.FechaDesde
                hasta = Me.InicioDeProcesamientoDetalle?.FechaHasta
            Case 34 ' Desarmado
                desde = Me.DesarmadoDetalle?.FechaDesde
                hasta = Me.DesarmadoDetalle?.FechaHasta

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
            Case 1 : Return Color.LightSkyBlue      ' Designacion
            Case 2 : Return Color.LightCoral       ' Enfermedad
            Case 3 : Return Color.Khaki            ' Sancion
            Case 4 : Return Color.Plum             ' OrdenCinco
            Case 5 : Return Color.LightGray        ' Reten
            Case 6 : Return Color.LightSalmon      ' Sumario
            Case 21 : Return Color.LightGreen      ' Traslado

            ' --- NUEVOS COLORES AÑADIDOS ---
            Case 29 : Return Color.DarkRed         ' Baja de Funcionario
            Case 30 : Return Color.CornflowerBlue  ' Cambio de Cargo
            Case 31 : Return Color.MediumSeaGreen  ' Reactivación de Funcionario
            Case 32 : Return Color.IndianRed       ' Separación del Cargo
            Case 33 : Return Color.Orange          ' Inicio de Procesamiento
            Case 34 : Return Color.SlateGray       ' Desarmado

            Case Else : Return Color.White
        End Select
    End Function

End Class