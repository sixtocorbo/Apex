' Apex/Services/CargoService.vb
Imports System.Data.Entity

Public Class CargoService
    Inherits GenericService(Of Cargo)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub
End Class