Imports System.Drawing
Imports System.Globalization
Imports System.Text

Public Module EstadoVisualHelper

    Public Enum EventoSeveridad
        Info = 0
        Baja = 1
        Media = 2
        Alta = 3
        Critica = 4
    End Enum

    Public Function DeterminarSeveridad(tipoTexto As String) As EventoSeveridad
        Dim t As String = NormalizarTexto(tipoTexto)

        If String.IsNullOrWhiteSpace(t) Then Return EventoSeveridad.Info

        If t.Contains("INICIO DE PROCESAMIENTO") Then Return EventoSeveridad.Critica
        If t.Contains("SEPARACION") Then Return EventoSeveridad.Critica
        If t.Contains("BAJA") Then Return EventoSeveridad.Critica

        If t.Contains("SUMARIO") Then Return EventoSeveridad.Alta
        If t.Contains("SANCI") Then Return EventoSeveridad.Alta

        If t.Contains("ORDEN CINCO") OrElse t.Contains("ORDEN 5") Then Return EventoSeveridad.Media
        If t.Contains("ENFERMEDAD") Then Return EventoSeveridad.Media
        If t.Contains("TRASLADO") Then Return EventoSeveridad.Media
        If t.StartsWith("NOTIFICACION") Then Return EventoSeveridad.Media

        If t.Contains("RETEN") Then Return EventoSeveridad.Baja
        If t.Contains("DESIGNACION") Then Return EventoSeveridad.Baja
        If t.Contains("REACTIVACION") Then Return EventoSeveridad.Baja
        If t.Contains("CAMBIO DE CARGO") Then Return EventoSeveridad.Baja
        If t.StartsWith("LICENCIA") Then Return EventoSeveridad.Baja

        If t.StartsWith("CAMBIO") Then Return EventoSeveridad.Info
        If t.Contains("AUDITORIA") Then Return EventoSeveridad.Info

        Return EventoSeveridad.Info
    End Function

    Public Function ObtenerColor(severidad As EventoSeveridad) As Color
        Select Case severidad
            Case EventoSeveridad.Critica
                Return Color.FromArgb(229, 57, 53)
            Case EventoSeveridad.Alta
                Return Color.FromArgb(245, 124, 0)
            Case EventoSeveridad.Media
                Return Color.FromArgb(255, 179, 0)
            Case EventoSeveridad.Baja
                Return Color.FromArgb(56, 142, 60)
            Case Else
                Return Color.FromArgb(30, 136, 229)
        End Select
    End Function

    Public Function ObtenerColorTexto(severidad As EventoSeveridad) As Color
        Return Color.White
    End Function

    Public Function ObtenerColorPorTipo(tipoTexto As String) As Color
        Return ObtenerColor(DeterminarSeveridad(tipoTexto))
    End Function

    Private Function NormalizarTexto(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return String.Empty

        Dim normalized = valor.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder()

        For Each c As Char In normalized
            If CharUnicodeInfo.GetUnicodeCategory(c) <> UnicodeCategory.NonSpacingMark Then
                sb.Append(c)
            End If
        Next

        Return sb.ToString().Normalize(NormalizationForm.FormC).ToUpperInvariant()
    End Function

End Module
