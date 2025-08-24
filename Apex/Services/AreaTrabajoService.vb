' Apex/Services/AreaTrabajoService.vb
Imports System.Data.Entity

Public Class AreaTrabajoService
    Inherits GenericService(Of AreaTrabajo)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub
End Class