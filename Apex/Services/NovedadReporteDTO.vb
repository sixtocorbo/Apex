' Este archivo contendrá las clases para estructurar los datos del reporte.

Public Class NovedadReporteDTO
    Public Property Id As Integer
    Public Property Fecha As Date
    Public Property Texto As String
    Public Property Funcionarios As List(Of FuncionarioReporteDTO)
    Public Property Fotos As List(Of FotoReporteDTO)

    Public Sub New()
        Funcionarios = New List(Of FuncionarioReporteDTO)()
        Fotos = New List(Of FotoReporteDTO)()
    End Sub
End Class

Public Class FuncionarioReporteDTO
    Public Property Id As Integer
    Public Property Nombre As String
End Class

Public Class FotoReporteDTO
    Public Property Id As Integer
    Public Property Foto As Byte() ' Aquí viajarán los datos de la imagen
    Public Property FileName As String
End Class