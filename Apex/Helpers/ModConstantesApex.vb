' Apex/Services/ModConstantesApex.vb

Public Class ModConstantesApex
    ''' <summary>
    ''' Contiene los IDs de las categorías de ausencias tal como figuran en la base de datos.
    ''' </summary>
    Public Const CATEGORIA_ID_GENERAL As Integer = 1
    Public Const CATEGORIA_ID_SALUD As Integer = 2
    Public Const CATEGORIA_ID_ESPECIAL As Integer = 3
    Public Const CATEGORIA_ID_SANCION_LEVE As Integer = 4
    Public Const CATEGORIA_ID_SANCION_GRAVE As Integer = 5

    ''' <summary>
    ''' Contiene los nombres de las categorías de ausencias.
    ''' </summary>
    Public Const CATEGORIA_NOMBRE_SALUD As String = "Salud"
    Public Const CATEGORIA_NOMBRE_SANCION_LEVE As String = "Sanción Leve"
    Public Const CATEGORIA_NOMBRE_SANCION_GRAVE As String = "Sanción Grave"
    Public Const CATEGORIA_NOMBRE_ESPECIAL As String = "Especial"
    Public Const CATEGORIA_NOMBRE_GENERAL As String = "General"

    ' -------- Notificaciones ----------
    Public Const ESTADO_NOTI_PENDIENTE As Byte = 1
    ' Si ya tenés otros estados, podés agregarlos aquí:
    Public Const ESTADO_NOTI_ENVIADA As Byte = 2
    Public Const ESTADO_NOTI_ENTREGADA As Byte = 3

    ''' <summary>
    ''' Define los IDs de los estados de una notificación personal.
    ''' </summary>
    Public Enum EstadoNotificacionId As Byte
        Pendiente = 1
        Vencida = 2
        Firmada = 3
    End Enum

End Class