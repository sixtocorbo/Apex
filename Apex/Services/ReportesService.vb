Imports System.Data.Entity

Public Class ReportesService
    Private ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        _unitOfWork = New UnitOfWork()
    End Sub

    Public Async Function GetDatosFichaFuncionalAsync(funcionarioId As Integer) As Task(Of FichaFuncionalDTO)
        ' obtener funcionario con sus relaciones, incluyendo Genero
        Dim funcionario = Await _unitOfWork.Repository(Of Funcionario)().GetAll().
            Include(Function(f) f.Cargo).
            Include(Function(f) f.Cargo.Grado).
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
            Include(Function(f) f.Genero).     ' para Sexo
            AsNoTracking().
            FirstOrDefaultAsync(Function(f) f.Id = funcionarioId)

        If funcionario Is Nothing Then Return Nothing

        ' licencias y sanciones (igual que antes)… 

        ' construir campos nuevos
        Dim sexo As String = funcionario.Genero?.Nombre            ' “Masculino”, “Femenino”, etc.
        Dim ciudad As String = Nothing                             ' no existe en el modelo actual
        Dim seccional As String = Nothing                          ' tampoco existe
        Dim estudia As String = Nothing                            ' idem; si lo agregas, usar Sí/No
        Dim credencial As String = Nothing                         ' idem

        ' armar resumen de datos laborales y situación general
        Dim datosLaborales = $"{funcionario.Cargo?.Nombre} {funcionario.Cargo?.Grado} {funcionario.Escalafon?.Nombre} {funcionario.Seccion?.Nombre} {funcionario.PuestoTrabajo?.Nombre}".Trim()
        Dim situacionGeneral = $"Estado: {funcionario.Estado?.Nombre} | Turno: {funcionario.Turno?.Nombre} | Semana: {funcionario.Semana?.Nombre} | Horario: {funcionario.Horario?.Nombre}"

        ' mapear al DTO
        Dim dto As New FichaFuncionalDTO With {
            .NombreCompleto = funcionario.Nombre,
            .Cedula = funcionario.CI,
            .FechaNacimiento = funcionario.FechaNacimiento?.ToShortDateString(),
            .Domicilio = funcionario.Domicilio,
            .Telefono = funcionario.Telefono,
            .Email = funcionario.Email,          ' renombrado a Correo
            .EstadoCivil = funcionario.EstadoCivil?.Nombre,
            .NivelEstudio = funcionario.NivelEstudio?.Nombre,
            .FechaIngreso = funcionario.FechaIngreso.ToShortDateString(),
            .Cargo = funcionario.Cargo?.Nombre,
            .Grado = funcionario.Cargo?.Grado?.ToString(),
            .Seccion = funcionario.Seccion?.Nombre,
            .PuestoTrabajo = funcionario.PuestoTrabajo?.Nombre,
            .Turno = funcionario.Turno?.Nombre,
            .Semana = funcionario.Semana?.Nombre,
            .Horario = funcionario.Horario?.Nombre,
            .Estado = funcionario.Estado?.Nombre,
            .Escalafon = funcionario.Escalafon?.Nombre,
            .Funcion = funcionario.Funcion?.Nombre,
            .Foto = funcionario.Foto,
            .Licencias = licencias,
            .Sanciones = sanciones,        ' campos nuevos
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

' --- DTOs para la Ficha Funcional ---
Public Class FichaFuncionalDTO
    Public Property NombreCompleto As String
    Public Property Cedula As String
    Public Property Grado As String
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
    Public Property Turno As String
    Public Property Semana As String
    Public Property Horario As String
    Public Property Estado As String
    Public Property Escalafon As String
    Public Property Funcion As String
    Public Property Foto As Byte()
    Public Property Licencias As List(Of LicenciaFichaDTO)
    Public Property Sanciones As List(Of SancionFichaDTO)
End Class

Public Class LicenciaFichaDTO
    Public Property Inicio As String
    Public Property Finaliza As String
    Public Property Dias As Integer?
    Public Property TipoLicencia As String
    Public Property Anio As Integer
End Class

Public Class SancionFichaDTO
    Public Property FechaDesde As String
    Public Property FechaHasta As String
    Public Property Observaciones As String
    Public Property Resolucion As String
End Class