' DesignacionReporteDTO.vb
Public Class DesignacionReporteDTO
    ' Datos del funcionario y notificación
    Public Property NombreFuncionario As String
    Public Property CedulaFuncionario As String
    Public Property FechaProgramada As Date?

    ' Datos específicos de la designación
    Public Property Destino As String
    Public Property Tarea As String
    Public Property TomaDePosesion As Date?
    Public Property Observaciones As String
    Public Property Expediente As String
End Class