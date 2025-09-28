' Archivo: DTO/NotificacionImprimirDTO.vb

Public Class NotificacionImprimirDTO
    ' --- Datos existentes ---
    Public Property FuncionarioId As Integer
    Public Property NombreFuncionario As String
    Public Property CI As String
    Public Property Cargo As String
    Public Property Seccion As String
    Public Property FechaProgramada As DateTime
    Public Property Texto As String
    Public Property TipoNotificacion As String
    Public Property Documento As String
    Public Property ExpMinisterial As String
    Public Property ExpINR As String
    Public Property Oficina As String

    ' --- NUEVOS DATOS DEL FUNCIONARIO ---
    Public Property Domicilio As String
    Public Property Telefono As String
    Public Property GradoNumero As Integer? ' Usamos Integer? para permitir valores nulos
    Public Property GradoDenominacion As String
    Public Property Escalafon As String
    Public Property SubEscalafon As String

End Class