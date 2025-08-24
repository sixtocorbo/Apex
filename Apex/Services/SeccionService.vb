' Apex/Services/SeccionService.vb
Imports System.Data.Entity

Public Class SeccionService
    Inherits GenericService(Of Seccion)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub
End Class