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

            Case TiposEstadoCatalog.Traslado
                FillFromDetail(et.TrasladoDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            Case TiposEstadoCatalog.CambioDeCargo
                FillFromDetail(et.CambioDeCargoDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            ' ### INICIO DE CORRECCIÓN ###
            ' Se agregan los casos para los nuevos tipos de estado que antes estaban ignorados.
            ' Todos estos ahora se tratan como eventos con un posible rango de fechas.

            Case TiposEstadoCatalog.SeparacionDelCargo
                FillFromDetail(et.SeparacionDelCargoDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            Case TiposEstadoCatalog.InicioDeProcesamiento
                ' Asumimos que la tabla de detalle se llama ProcesadoDetalle o similar y está mapeada en el .edmx
                ' Si el nombre de la propiedad en 'et' es diferente, ajústalo aquí.
                ' Por ejemplo: FillFromDetail(et.ProcesadoDetalle, ...)
                FillFromDetail(et.InicioDeProcesamientoDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            Case TiposEstadoCatalog.Desarmado
                FillFromDetail(et.DesarmadoDetalle, {"FechaDesde"}, {"FechaHasta"}, desde, hasta)

            ' --- Casos con fecha PUNTUAL ---
            Case TiposEstadoCatalog.Reten
                Dim fechaReten = ReadDate(et.RetenDetalle, "FechaReten")
                If fechaReten.HasValue Then
                    desde = fechaReten
                    hasta = fechaReten
                End If

            Case TiposEstadoCatalog.BajaDeFuncionario
                Dim fechaBaja = ReadDate(et.BajaDeFuncionarioDetalle, "Fecha")
                If fechaBaja.HasValue Then
                    desde = fechaBaja
                    hasta = fechaBaja
                End If

            Case TiposEstadoCatalog.ReactivacionDeFuncionario
                Dim fechaReactivacion = ReadDate(et.ReactivacionDeFuncionarioDetalle, "Fecha")
                If fechaReactivacion.HasValue Then
                    desde = fechaReactivacion
                    hasta = fechaReactivacion
                End If
                ' ### FIN DE CORRECCIÓN ###

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