Option Strict On
Option Explicit On

Imports System.Runtime.CompilerServices
Imports System.Reflection

Public Module EstadoTransitorioExtensions

    <Extension()>
    Public Sub GetFechas(et As EstadoTransitorio, ByRef desde As Date?, ByRef hasta As Date?)
        desde = Nothing
        hasta = Nothing
        If et Is Nothing Then Exit Sub

        ' La única fuente de verdad para las fechas son las tablas de detalle.
        ' Se busca la fecha correspondiente según el tipo de estado transitorio.
        Select Case et.TipoEstadoTransitorioId

            ' --- Casos con RANGO de fechas (Desde/Hasta) ---
            Case TiposEstadoCatalog.Designacion
                FillFromDetail(et.DesignacionDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            Case TiposEstadoCatalog.Enfermedad
                FillFromDetail(et.EnfermedadDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            Case TiposEstadoCatalog.Sancion
                FillFromDetail(et.SancionDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            Case TiposEstadoCatalog.OrdenCinco
                FillFromDetail(et.OrdenCincoDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            Case TiposEstadoCatalog.Sumario
                FillFromDetail(et.SumarioDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            ' --- Casos con fecha PUNTUAL ---
            Case TiposEstadoCatalog.Reten
                ' Para un Retén, la fecha es un evento único. Se fuerza Desde y Hasta al mismo día.
                Dim fechaReten = ReadDate(et.RetenDetalle, "FechaReten")
                If fechaReten.HasValue Then
                    desde = fechaReten
                    hasta = fechaReten
                End If

            ' --- Casos que NO tienen tabla de detalle propia en tu modelo actual ---
            ' Si en el futuro se añaden tablas de detalle para estos tipos,
            ' se deberán agregar sus respectivos `Case` aquí.
            ' Por ahora, no se hace nada y las fechas quedarán en Nothing, lo cual es correcto.
            Case TiposEstadoCatalog.Traslado,
                 TiposEstadoCatalog.BajaDeFuncionario,
                 TiposEstadoCatalog.CambioDeCargo,
                 TiposEstadoCatalog.ReactivacionDeFuncionario,
                 TiposEstadoCatalog.SeparacionDelCargo,
                 TiposEstadoCatalog.InicioDeProcesamiento,
                 TiposEstadoCatalog.Desarmado
                ' No hay tabla de detalle asociada de la cual leer fechas.

        End Select
    End Sub

#Region " Helpers de lectura/reflexión "

    ' Rellena las fechas desde un objeto de detalle.
    Private Sub FillFromDetail(detail As Object,
                               inicioNames As IEnumerable(Of String),
                               finNames As IEnumerable(Of String),
                               ByRef desde As Date?, ByRef hasta As Date?)
        If detail Is Nothing Then Exit Sub
        desde = FirstDate(detail, inicioNames)
        hasta = FirstDate(detail, finNames)
    End Sub

    ' Busca en un objeto la primera propiedad de fecha de una lista de nombres que tenga un valor.
    Private Function FirstDate(obj As Object, propNames As IEnumerable(Of String)) As Date?
        If obj Is Nothing OrElse propNames Is Nothing Then Return Nothing
        For Each name In propNames
            Dim v = ReadDate(obj, name)
            If v.HasValue Then Return v
        Next
        Return Nothing
    End Function

    ' Lee una propiedad de tipo Date o Nullable(Of Date) por su nombre (ignorando mayúsculas/minúsculas).
    Private Function ReadDate(obj As Object, propName As String) As Date?
        If obj Is Nothing OrElse String.IsNullOrWhiteSpace(propName) Then Return Nothing
        Dim p = obj.GetType().GetProperty(propName,
                                          BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.IgnoreCase)
        If p Is Nothing Then Return Nothing

        Dim rawValue = p.GetValue(obj, Nothing)
        If rawValue Is Nothing OrElse rawValue Is DBNull.Value Then Return Nothing

        Try
            Return Convert.ToDateTime(rawValue)
        Catch
            Return Nothing
        End Try
    End Function

#End Region

End Module