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

End Class