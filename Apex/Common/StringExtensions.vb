Imports System.Globalization
Imports System.Management.Instrumentation
Imports System.Text

Module StringExtensions
    <System.Runtime.CompilerServices.Extension()>
    Public Function RemoveDiacritics(text As String) As String
        If String.IsNullOrEmpty(text) Then Return text
        ' Descompone en letra + marca diacrítica
        Dim normalized = text.Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder()
        For Each ch As Char In normalized
            Dim cat = CharUnicodeInfo.GetUnicodeCategory(ch)
            If cat <> UnicodeCategory.NonSpacingMark Then
                sb.Append(ch)
            End If
        Next
        ' Reconstruye la cadena limpia
        Return sb.ToString().Normalize(NormalizationForm.FormC)

    End Function
End Module
