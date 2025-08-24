' Apex/Services/TurnoService.vb
Imports System.Data.Entity

Public Class TurnoService
    Inherits GenericService(Of Turno)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub
End Class