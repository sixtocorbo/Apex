Imports System.ComponentModel.DataAnnotations.Schema

Partial Public Class UnidadGeneral
    <NotMapped>
    Public Property Id As Integer
        Get
            Return UnidadGeneralId
        End Get
        Set(value As Integer)
            UnidadGeneralId = value
        End Set
    End Property
End Class
