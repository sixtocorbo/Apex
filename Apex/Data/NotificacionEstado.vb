'------------------------------------------------------------------------------
' <auto-generated>
'     Este código se generó a partir de una plantilla.
'
'     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
'     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
' </auto-generated>
'------------------------------------------------------------------------------

Imports System
Imports System.Collections.Generic

Partial Public Class NotificacionEstado
    Public Property Id As Byte
    Public Property Nombre As String
    Public Property Orden As Byte

    Public Overridable Property NotificacionPersonal As ICollection(Of NotificacionPersonal) = New HashSet(Of NotificacionPersonal)
    Public Overridable Property Novedad As ICollection(Of Novedad) = New HashSet(Of Novedad)

End Class
