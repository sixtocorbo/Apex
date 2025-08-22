Imports System.Data.Entity

Public Class ReportesService
    Private ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        _unitOfWork = New UnitOfWork()
    End Sub

    Public Async Function GetDatosFichaFuncionalAsync(funcionarioId As Integer) As Task(Of FichaFuncionalDTO)
        Dim funcionario = Await _unitOfWork.Repository(Of Funcionario)().GetAll().
            Include(Function(f) f.Cargo).
            Include(Function(f) f.Seccion).
            Include(Function(f) f.PuestoTrabajo).
            Include(Function(f) f.Horario).
            Include(Function(f) f.EstadoCivil).
            Include(Function(f) f.NivelEstudio).
            AsNoTracking().
            FirstOrDefaultAsync(Function(f) f.Id = funcionarioId)

        If funcionario Is Nothing Then
            Return Nothing
        End If

        Dim dto = New FichaFuncionalDTO With {
            .NombreCompleto = funcionario.Nombre,
            .Cedula = funcionario.CI,
            .FechaNacimiento = funcionario.FechaNacimiento?.ToShortDateString(),
            .Domicilio = funcionario.Domicilio,
            .Telefono = funcionario.Telefono,
            .Email = funcionario.Email,
            .EstadoCivil = funcionario.EstadoCivil?.Nombre,
            .NivelEstudio = funcionario.NivelEstudio?.Nombre,
            .FechaIngreso = funcionario.FechaIngreso.ToShortDateString(),
            .Cargo = funcionario.Cargo?.Nombre,
            .Seccion = funcionario.Seccion?.Nombre,
            .PuestoTrabajo = funcionario.PuestoTrabajo?.Nombre,
            .Horario = funcionario.Horario?.Nombre,
            .Foto = funcionario.Foto
        }

        Return dto
    End Function

End Class

' DTO para la ficha funcional
Public Class FichaFuncionalDTO
    Public Property NombreCompleto As String
    Public Property Cedula As String
    Public Property FechaNacimiento As String
    Public Property Domicilio As String
    Public Property Telefono As String
    Public Property Email As String
    Public Property EstadoCivil As String
    Public Property NivelEstudio As String
    Public Property FechaIngreso As String
    Public Property Cargo As String
    Public Property Seccion As String
    Public Property PuestoTrabajo As String
    Public Property Horario As String
    Public Property Foto As Byte()
End Class