Option Strict On
Option Infer On
Imports System.Globalization
Imports System.Text

Public Module ModConstantesApex
#Region "Tipos de Estado Transitorio (IDs de BD)"
    ' IDs según la tabla [dbo].[TipoEstadoTransitorio]
    Public NotInheritable Class TipoEstadoTransitorioId
        Public Const Designacion As Integer = 1
        Public Const Enfermedad As Integer = 2
        Public Const Sancion As Integer = 3
        Public Const OrdenCinco As Integer = 4
        Public Const Reten As Integer = 5
        Public Const Sumario As Integer = 6
        Public Const BajaDeFuncionario As Integer = 7
        Public Const CambioDeCargo As Integer = 8
        Public Const ReactivacionDeFuncionario As Integer = 9
        Public Const SeparacionDelCargo As Integer = 10
        Public Const InicioDeProcesamiento As Integer = 11
        Public Const Desarmado As Integer = 12
        Public Const Traslado As Integer = 21
        Private Sub New() : End Sub
    End Class

    ' Compatibilidad con código existente: reexpone con el nombre usado en otras partes
    Public NotInheritable Class TiposEstadoCatalog
        Public Const Designacion = TipoEstadoTransitorioId.Designacion
        Public Const Enfermedad = TipoEstadoTransitorioId.Enfermedad
        Public Const Sancion = TipoEstadoTransitorioId.Sancion
        Public Const OrdenCinco = TipoEstadoTransitorioId.OrdenCinco
        Public Const Reten = TipoEstadoTransitorioId.Reten
        Public Const Sumario = TipoEstadoTransitorioId.Sumario
        Public Const BajaDeFuncionario = TipoEstadoTransitorioId.BajaDeFuncionario
        Public Const CambioDeCargo = TipoEstadoTransitorioId.CambioDeCargo
        Public Const ReactivacionDeFuncionario = TipoEstadoTransitorioId.ReactivacionDeFuncionario
        Public Const SeparacionDelCargo = TipoEstadoTransitorioId.SeparacionDelCargo
        Public Const InicioDeProcesamiento = TipoEstadoTransitorioId.InicioDeProcesamiento
        Public Const Desarmado = TipoEstadoTransitorioId.Desarmado
        Public Const Traslado = TipoEstadoTransitorioId.Traslado
        Private Sub New() : End Sub
    End Class

    ' Helpers de uso común
    Public Function RequiereFechaHasta(tipoId As Integer) As Boolean
        ' No requiere fecha hasta: Retén y Reactivación
        Return Not (tipoId = TipoEstadoTransitorioId.Reten OrElse
                tipoId = TipoEstadoTransitorioId.ReactivacionDeFuncionario)
    End Function
#End Region

#Region "Categorías de Ausencia"
    ' --- IDs tal cual en BD ---
    Public NotInheritable Class CategoriaAusenciaId
        Public Const General As Integer = 1
        Public Const Salud As Integer = 2
        Public Const Especial As Integer = 3
        Public Const SancionLeve As Integer = 4
        Public Const SancionGrave As Integer = 5
        Private Sub New() : End Sub
    End Class

    ' --- Nombres “bonitos” para mostrar en UI ---
    Public NotInheritable Class CategoriaAusenciaNombre
        Public Const General As String = "General"
        Public Const Salud As String = "Salud"
        Public Const Especial As String = "Especial"
        Public Const SancionLeve As String = "Sanción Leve"
        Public Const SancionGrave As String = "Sanción Grave"
        Private Sub New() : End Sub
    End Class

    ' --- Claves normalizadas (sin tildes/espacios) para comparaciones por texto ---
    Public NotInheritable Class CategoriaAusenciaKey
        Public Const General As String = "general"
        Public Const Salud As String = "salud"
        Public Const Especial As String = "especial"
        Public Const SancionLeve As String = "sancionleve"
        Public Const SancionGrave As String = "sanciongrave"
        Private Sub New() : End Sub
    End Class

    ' Normaliza: quita tildes, pasa a minúsculas y remueve no-letras/espacios
    Public Function Normalizar(ByVal texto As String) As String
        If String.IsNullOrWhiteSpace(texto) Then Return ""
        Dim d = texto.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder(d.Length)
        For Each ch As Char In d
            If CharUnicodeInfo.GetUnicodeCategory(ch) <> UnicodeCategory.NonSpacingMark Then sb.Append(ch)
        Next
        Dim s = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant()
        s = New String(s.Where(Function(c) Char.IsLetter(c)).ToArray())
        Return s
    End Function

    ' Mapear nombre -> ID (por si te llega texto desde BD o importaciones)
    Public Function CategoriaIdPorNombre(nombre As String) As Integer?
        Select Case Normalizar(nombre)
            Case CategoriaAusenciaKey.Salud : Return CategoriaAusenciaId.Salud
            Case CategoriaAusenciaKey.Especial : Return CategoriaAusenciaId.Especial
            Case CategoriaAusenciaKey.SancionLeve : Return CategoriaAusenciaId.SancionLeve
            Case CategoriaAusenciaKey.SancionGrave : Return CategoriaAusenciaId.SancionGrave
            Case CategoriaAusenciaKey.General : Return CategoriaAusenciaId.General
            Case Else : Return Nothing
        End Select
    End Function

    ' Predicados preferidos (por ID)
    Public Function EsSalud(categoriaId As Integer?) As Boolean
        Return categoriaId.HasValue AndAlso categoriaId.Value = CategoriaAusenciaId.Salud
    End Function
    Public Function EsSancionLeve(categoriaId As Integer?) As Boolean
        Return categoriaId.HasValue AndAlso categoriaId.Value = CategoriaAusenciaId.SancionLeve
    End Function
    Public Function EsSancionGrave(categoriaId As Integer?) As Boolean
        Return categoriaId.HasValue AndAlso categoriaId.Value = CategoriaAusenciaId.SancionGrave
    End Function

    ' Predicados alternativos (por nombre normalizado)
    Public Function EsSalud(nombreCategoria As String) As Boolean
        Return Normalizar(nombreCategoria) = CategoriaAusenciaKey.Salud
    End Function
    Public Function EsSancionLeve(nombreCategoria As String) As Boolean
        Return Normalizar(nombreCategoria) = CategoriaAusenciaKey.SancionLeve
    End Function
    Public Function EsSancionGrave(nombreCategoria As String) As Boolean
        Return Normalizar(nombreCategoria) = CategoriaAusenciaKey.SancionGrave
    End Function
#End Region

#Region "Estados de Notificaciones"
    ' Estados del flujo de ENVÍO (cola de notificaciones)
    Public Enum EstadoNotificacionEnvio As Byte
        Pendiente = 1
        Enviada = 2
        Entregada = 3
    End Enum

    ' Estados de la notificación PERSONAL (gestión del documento)
    Public Enum EstadoNotificacionPersonal As Byte
        Pendiente = 1
        Vencida = 2
        Firmada = 3
    End Enum

    ' (Compatibilidad) Si querés mantener los viejos Const, dejalos obsoletos:
    <Obsolete("Usar EstadoNotificacionEnvio")>
    Public Const ESTADO_NOTI_PENDIENTE As Byte = CByte(EstadoNotificacionEnvio.Pendiente)
    <Obsolete("Usar EstadoNotificacionEnvio")>
    Public Const ESTADO_NOTI_ENVIADA As Byte = CByte(EstadoNotificacionEnvio.Enviada)
    <Obsolete("Usar EstadoNotificacionEnvio")>
    Public Const ESTADO_NOTI_ENTREGADA As Byte = CByte(EstadoNotificacionEnvio.Entregada)
#End Region

End Module
