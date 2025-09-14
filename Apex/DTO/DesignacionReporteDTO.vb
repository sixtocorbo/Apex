' DesignacionReporteDTO.vb (Versión Corregida)
Public Class DesignacionReporteDTO
    ' Datos del funcionario y notificación
    Public Property NombreFuncionario As String
    Public Property CedulaFuncionario As String
    Public Property FechaProgramada As Date?

    ' --- CAMPOS CORREGIDOS ---
    ' Datos específicos de la designación
    Public Property FechaDesde As Date?
    Public Property FechaHasta As Date?
    Public Property Observaciones As String
    Public Property DocResolucion As String
    Public Property FechaResolucion As Date?
    Public Property Expediente As String
End Class