' Apex/Services/ReportesService.vb
Imports System.Data.Entity

Public Class ReportesService
    Private ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        _unitOfWork = New UnitOfWork()
    End Sub

    Public Async Function GetDatosFichaFuncionalAsync(funcionarioId As Integer) As Task(Of FichaFuncionalDTO)
        ' Se mantiene la consulta principal del funcionario con sus datos relacionados
        Dim funcionario = Await _unitOfWork.Repository(Of Funcionario)().GetAll().
            Include(Function(f) f.Cargo).
            Include(Function(f) f.Seccion).
            Include(Function(f) f.PuestoTrabajo).
            Include(Function(f) f.Turno).
            Include(Function(f) f.Semana).
            Include(Function(f) f.Horario).
            Include(Function(f) f.Estado).
            Include(Function(f) f.Escalafon).
            Include(Function(f) f.Funcion).
            Include(Function(f) f.EstadoCivil).
            Include(Function(f) f.NivelEstudio).
            Include(Function(f) f.Genero).
            AsNoTracking().
            FirstOrDefaultAsync(Function(f) f.Id = funcionarioId)

        If funcionario Is Nothing Then Return Nothing

        ' --- INICIO DE LA MODIFICACIÓN ---
        ' Se eliminaron las consultas para licencias y sanciones que ya no se usan en el reporte.
        ' --- FIN DE LA MODIFICACIÓN ---

        ' Construir campos nuevos
        Dim sexo As String = funcionario.Genero?.Nombre
        Dim ciudad As String = funcionario.Ciudad?.Nombre
        Dim seccional As String = funcionario.Seccional?.Nombre
        Dim estudia As String = Nothing
        Dim credencial As String = funcionario.Credencial?.Numero

        ' Armar resumen de datos laborales y situación general
        Dim datosLaborales = $"{funcionario.Cargo?.Nombre} {funcionario.Escalafon?.Nombre} {funcionario.Seccion?.Nombre} {funcionario.PuestoTrabajo?.Nombre}".Trim()
        Dim situacionGeneral = $"Estado: {funcionario.Estado?.Nombre} | Turno: {funcionario.Turno?.Nombre} | Semana: {funcionario.Semana?.Nombre} | Horario: {funcionario.Horario?.Nombre}"

        ' Mapear al DTO simplificado
        Dim dto As New FichaFuncionalDTO With {
            .NombreCompleto = funcionario.Nombre,
            .Cedula = funcionario.CI,
            .FechaNacimiento = funcionario.FechaNacimiento?.ToShortDateString(),
            .Domicilio = funcionario.Domicilio,
            .Telefono = funcionario.Telefono,
            .Correo = funcionario.Email,
            .EstadoCivil = funcionario.EstadoCivil?.Nombre,
            .NivelEstudios = funcionario.NivelEstudio?.Nombre,
            .FechaIngreso = funcionario.FechaIngreso.ToShortDateString(),
            .Grado = funcionario.Cargo?.Grado?.ToString(),
            .Cargo = funcionario.Cargo?.Nombre,
            .Seccion = funcionario.Seccion?.Nombre,
            .PuestoTrabajo = funcionario.PuestoTrabajo?.Nombre,
            .Turno = funcionario.Turno?.Nombre,
            .Semana = funcionario.Semana?.Nombre,
            .Horario = funcionario.Horario?.Nombre,
            .Estado = funcionario.Estado?.Nombre,
            .Escalafon = funcionario.Escalafon?.Nombre,
            .Funcion = funcionario.Funcion?.Nombre,
            .Foto = funcionario.Foto,
            .Sexo = sexo,
            .Ciudad = ciudad,
            .Seccional = seccional,
            .Estudia = estudia,
            .Credencial = credencial,
            .DatosLaborales = datosLaborales,
            .SituacionGeneral = situacionGeneral
        }

        Return dto
    End Function

End Class

' --- DTOs para la Ficha Funcional (versión simplificada) ---
Public Class FichaFuncionalDTO
    Public Property NombreCompleto As String
    Public Property Cedula As String
    Public Property Grado As String
    Public Property FechaNacimiento As String
    Public Property Domicilio As String
    Public Property Telefono As String
    Public Property Correo As String
    Public Property EstadoCivil As String
    Public Property NivelEstudios As String
    Public Property FechaIngreso As String
    Public Property Cargo As String
    Public Property Seccion As String
    Public Property PuestoTrabajo As String
    Public Property Turno As String
    Public Property Semana As String
    Public Property Horario As String
    Public Property Estado As String
    Public Property Escalafon As String
    Public Property Funcion As String
    Public Property Foto As Byte()
    Public Property Sexo As String
    Public Property Ciudad As String
    Public Property Seccional As String
    Public Property Estudia As String
    Public Property Credencial As String
    Public Property DatosLaborales As String
    Public Property SituacionGeneral As String
End Class

' Las clases LicenciaFichaDTO y SancionFichaDTO se han eliminado al no ser necesarias.