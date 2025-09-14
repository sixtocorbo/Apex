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
    Public ReadOnly Property FechaCorta As String
        Get
            Return Fecha.ToString("dd/MM/yyyy")
        End Get
    End Property
End Class

Public Class FuncionarioReporteDTO
    Public Property NovedadId As Integer   ' <-- para filtrar en subreporte
    Public Property Id As Integer
    Public Property Nombre As String
End Class

Public Class FotoReporteDTO
    Public Property NovedadId As Integer   ' <-- para filtrar en subreporte
    Public Property Id As Integer
    Public Property Foto As Byte()
    Public Property FileName As String
End Class

