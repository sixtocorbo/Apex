Option Strict On
Option Explicit On
Imports System.Runtime.CompilerServices
Imports System.Reflection

Public Module EstadoTransitorioExtensions

    <Extension()>
    Public Sub GetFechas(et As EstadoTransitorio, ByRef desde As Date?, ByRef hasta As Date?)
        desde = Nothing : hasta = Nothing
        If et Is Nothing Then Exit Sub

        ' 1) Cabecera (si tu entidad tiene estas propiedades)
        FillIfEmpty(et, {"FechaDesde", "FechaInicio", "InicioVigencia"}, desde)
        FillIfEmpty(et, {"FechaHasta", "FechaFin", "FinVigencia"}, hasta)

        ' 2) Overrides por tipo (prioridad sobre cabecera cuando aplique)
        Select Case et.TipoEstadoTransitorioId

            Case TiposEstadoCatalog.Reten
                ' Fecha puntual (coincide con tu IsEstadoActivo)
                ForceFromDetail(et.RetenDetalle, {"FechaReten", "Fecha"}, Nothing, desde, hasta)

            Case TiposEstadoCatalog.Enfermedad
                FillFromDetail(et.EnfermedadDetalle,
                               {"FechaDesde", "FechaInicio", "Fecha"},
                               {"FechaHasta", "FechaFin"}, desde, hasta)

            Case TiposEstadoCatalog.Sancion
                FillFromDetail(et.SancionDetalle,
                               {"FechaDesde", "FechaInicio", "Fecha"},
                               {"FechaHasta", "FechaFin"}, desde, hasta)

            Case TiposEstadoCatalog.CambioDeCargo
                ' Suele ser efectiva en una fecha puntual
                ForceFromDetail(et.CambioDeCargoDetalle,
                                {"Fecha", "FechaEfectiva"},
                                Nothing, desde, hasta)

            Case TiposEstadoCatalog.ReactivacionDeFuncionario
                ForceFromDetail(et.ReactivacionDeFuncionarioDetalle,
                                {"Fecha", "FechaReactivacion"},
                                Nothing, desde, hasta)

            Case TiposEstadoCatalog.BajaDeFuncionario
                ForceFromDetail(et.BajaDeFuncionarioDetalle,
                                {"Fecha", "FechaBaja"},
                                Nothing, desde, hasta)

            Case TiposEstadoCatalog.Designacion
                ' Si tu detalle trae sólo la fecha de resolución, se toma como puntual
                If Not desde.HasValue AndAlso Not hasta.HasValue Then
                    FillFromDetail(et.DesignacionDetalle,
                                   {"FechaResolucion", "Fecha"},
                                   Nothing, desde, hasta, puntualSiSoloInicio:=True)
                End If

           ' --- dentro de GetFechas, en el Case “genérico” ---
            Case TiposEstadoCatalog.OrdenCinco, TiposEstadoCatalog.Sumario, TiposEstadoCatalog.Traslado,
                 TiposEstadoCatalog.SeparacionDelCargo, TiposEstadoCatalog.InicioDeProcesamiento,
                 TiposEstadoCatalog.Desarmado

                ' Elegir el primer detalle disponible sin chocar con Option Strict On
                Dim det As Object = FirstNonNull(
                    et.OrdenCincoDetalle,
                    et.SumarioDetalle,
                    et.TrasladoDetalle,
                    et.SeparacionDelCargoDetalle,
                    et.InicioDeProcesamientoDetalle,
                    et.DesarmadoDetalle
                )

                FillFromDetail(det,
                               {"FechaDesde", "FechaInicio", "Fecha", "FechaEfectiva"},
                               {"FechaHasta", "FechaFin"},
                               desde, hasta, puntualSiSoloInicio:=True)


        End Select
    End Sub
    Private Function FirstNonNull(ParamArray items As Object()) As Object
        For Each it In items
            If it IsNot Nothing Then Return it
        Next
        Return Nothing
    End Function

#Region " Helpers de lectura/reflexión "

    ' Rellena si el destino no tiene valor aún
    Private Sub FillIfEmpty(obj As Object, propNames As IEnumerable(Of String), ByRef target As Date?)
        If target.HasValue OrElse obj Is Nothing Then Exit Sub
        Dim v = FirstDate(obj, propNames)
        If v.HasValue Then target = v
    End Sub

    ' Para rangos típicos (Desde/Hasta). Si sólo hay inicio y se indicó puntual, deja Hasta = Nothing.
    Private Sub FillFromDetail(detail As Object,
                               inicioNames As IEnumerable(Of String),
                               finNames As IEnumerable(Of String),
                               ByRef desde As Date?, ByRef hasta As Date?,
                               Optional puntualSiSoloInicio As Boolean = False)
        If detail Is Nothing Then Exit Sub
        Dim d = FirstDate(detail, inicioNames)
        Dim h = If(finNames Is Nothing, Nothing, FirstDate(detail, finNames))

        If d.HasValue Then desde = d
        If h.HasValue Then
            hasta = h
        ElseIf d.HasValue AndAlso puntualSiSoloInicio Then
            hasta = Nothing
        End If
    End Sub

    ' Para fechas puntuales que deben sobrescribir la cabecera (Retén, Cambio de cargo, etc.)
    Private Sub ForceFromDetail(detail As Object,
                                inicioNames As IEnumerable(Of String),
                                finNames As IEnumerable(Of String),
                                ByRef desde As Date?, ByRef hasta As Date?)
        If detail Is Nothing Then Exit Sub
        Dim d = FirstDate(detail, inicioNames)
        Dim h = If(finNames Is Nothing, Nothing, FirstDate(detail, finNames))
        If d.HasValue Then desde = d
        If finNames Is Nothing Then
            hasta = Nothing
        Else
            hasta = h
        End If
    End Sub

    ' Busca la primera propiedad de fecha que exista y tenga valor
    Private Function FirstDate(obj As Object, propNames As IEnumerable(Of String)) As Date?
        If obj Is Nothing OrElse propNames Is Nothing Then Return Nothing
        For Each name In propNames
            Dim v = ReadDate(obj, name)
            If v.HasValue Then Return v
        Next
        Return Nothing
    End Function

    ' Lee una propiedad Date/Nullable(Of Date) por nombre (case-insensitive)
    Private Function ReadDate(obj As Object, propName As String) As Date?
        If obj Is Nothing Then Return Nothing
        Dim t = obj.GetType()
        Dim p = t.GetProperty(propName,
                              BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.IgnoreCase)
        If p Is Nothing Then Return Nothing
        Dim raw = p.GetValue(obj, Nothing)
        If raw Is Nothing Then Return Nothing
        Try
            Dim dt As Date = Convert.ToDateTime(raw)
            Return dt
        Catch
            Return Nothing
        End Try
    End Function

#End Region

End Module
