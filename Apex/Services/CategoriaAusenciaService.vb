' Apex/Services/CategoriaAusenciaService.vb
Imports System.Data.Entity

Public Class CategoriaAusenciaService
    Inherits GenericService(Of CategoriaAusencia)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub
End Class