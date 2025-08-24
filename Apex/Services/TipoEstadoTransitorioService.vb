' Apex/Services/TipoEstadoTransitorioService.vb
Imports System.Data.Entity

Public Class TipoEstadoTransitorioService
    Inherits GenericService(Of TipoEstadoTransitorio)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub
End Class