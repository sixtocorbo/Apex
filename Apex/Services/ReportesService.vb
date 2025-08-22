Imports System.Data.Entity

Public Class ReportesService
    Private ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        _unitOfWork = New UnitOfWork()
    End Sub

    Public Async Function GetDatosFichaFuncionalAsync(funcionarioId As Integer) As Task(Of FichaFuncionalDTO)
        ' Obtener datos principales del funcionario y sus relaciones
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
            AsNoTracking().
            FirstOrDefaultAsync(Function(f) f.Id = funcionarioId)

        If funcionario Is Nothing Then
            Return Nothing
        End If

        ' --- INICIO DE LA CORRECCIÓN ---

        ' 1. Obtener historial de licencias (datos crudos)
        Dim licenciasDb = Await _unitOfWork.Repository(Of HistoricoLicencia)().GetAll().
            Include(Function(l) l.TipoLicencia).
            Where(Function(l) l.FuncionarioId = funcionarioId).
            OrderByDescending(Function(l) l.inicio).
            ToListAsync()

        ' 2. Proyectar a DTO en memoria
        Dim licencias = licenciasDb.Select(Function(l) New LicenciaFichaDTO With {
            .Inicio = l.inicio.ToShortDateString(),
            .Finaliza = l.finaliza.ToShortDateString(),
            .Dias = (l.finaliza - l.inicio).Days + 1,
            .TipoLicencia = l.TipoLicencia.Nombre,
            .Anio = l.inicio.Year
        }).ToList()


        ' 1. Obtener historial de sanciones (datos crudos)
        Dim sancionesDb = Await _unitOfWork.Repository(Of vw_SancionesCompletas)().GetAll().
            Where(Function(s) s.FuncionarioId = funcionarioId).
            OrderByDescending(Function(s) s.FechaDesde).
            ToListAsync()

        ' 2. Proyectar a DTO en memoria
        Dim sanciones = sancionesDb.Select(Function(s) New SancionFichaDTO With {
            .FechaDesde = s.FechaDesde.ToShortDateString(),
            .FechaHasta = If(s.FechaHasta.HasValue, s.FechaHasta.Value.ToShortDateString(), "Indefinido"),
            .Observaciones = s.Observaciones,
            .Resolucion = s.Resolucion
        }).ToList()

        ' --- FIN DE LA CORRECCIÓN ---

        ' Mapear todos los datos al DTO principal
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
            .Sanciones = sanciones
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