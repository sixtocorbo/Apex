' Apex/Services/ReportesService.vb
Imports System.Data.Entity
Imports System.Linq
Imports System.Threading.Tasks

Public Class ReportesService
    Private ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        _unitOfWork = New UnitOfWork()
    End Sub

    ' Une sólo las partes no vacías con el separador indicado.
    Private Shared Function JoinNonEmpty(sep As String, ParamArray items() As String) As String
        If items Is Nothing Then Return String.Empty
        Dim parts = items.
            Where(Function(s) Not String.IsNullOrWhiteSpace(s)).
            Select(Function(s) s.Trim())
        Return String.Join(sep, parts)
    End Function

    ' Devuelve "Etiqueta: valor" sólo si value tiene contenido.
    Private Shared Function Labeled(label As String, value As String) As String
        If String.IsNullOrWhiteSpace(value) Then Return Nothing
        Return $"{label}{value.Trim()}"
    End Function

    ' Normaliza el texto de Estado: SI/SÍ -> Activo, NO -> Inactivo, otro -> tal cual
    Private Shared Function EstadoDisplay(valor As String) As String
        If String.IsNullOrWhiteSpace(valor) Then Return Nothing
        Dim raw = valor.Trim()
        Dim v = raw.ToUpperInvariant()
        If v = "SI" OrElse v = "SÍ" Then Return "Activo"
        If v = "NO" Then Return "Inactivo"
        Return raw
    End Function

    Public Async Function GetDatosFichaFuncionalAsync(funcionarioId As Integer) As Task(Of FichaFuncionalDTO)
        ' Consulta del funcionario con sus relaciones
        Dim funcionario = Await _unitOfWork.Repository(Of Funcionario)().
            GetAll().
            Include(Function(f) f.Cargo).
            Include(Function(f) f.Seccion).
            Include(Function(f) f.PuestoTrabajo).
            Include(Function(f) f.Turno).
            Include(Function(f) f.Semana).
            Include(Function(f) f.Horario).
            Include(Function(f) f.Escalafon).
            Include(Function(f) f.Funcion).
            Include(Function(f) f.EstadoCivil).
            Include(Function(f) f.NivelEstudio).
            Include(Function(f) f.Genero).
            AsNoTracking().
            FirstOrDefaultAsync(Function(f) f.Id = funcionarioId)

        If funcionario Is Nothing Then Return Nothing

        ' Estudia -> "Sí"/"No"
        Dim estudia As String = If(funcionario.Estudia, "Sí", "No")

        ' --- Datos Laborales (con etiquetas + separadores sólo entre partes no vacías) ---
        Dim datosLaborales As String = JoinNonEmpty(" | ",
            Labeled("Grado: ", funcionario.Cargo?.Grado?.ToString()),
            Labeled("Escalafón: ", funcionario.Escalafon?.Nombre),
            Labeled("Unidad: ", funcionario.Seccion?.Nombre),
            Labeled("Puesto de trabajo: ", funcionario.PuestoTrabajo?.Nombre)
        )

        ' --- Situación General (usa EstadoDisplay) ---
        Dim situacionGeneral As String = JoinNonEmpty(" | ",
           Labeled("Estado: ", EstadoDisplay(funcionario.Activo)),
            Labeled("Turno: ", funcionario.Turno?.Nombre),
            Labeled("Semana: ", funcionario.Semana?.Nombre),
            Labeled("Horario: ", funcionario.Horario?.Nombre)
        )

        ' Mapear DTO (Estado ya normalizado)
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
           .Estado = EstadoDisplay(funcionario.Activo),
            .Escalafon = funcionario.Escalafon?.Nombre,
            .Funcion = funcionario.Funcion?.Nombre,
            .Foto = funcionario.Foto,
            .Sexo = funcionario.Genero?.Nombre,
            .Ciudad = funcionario.Ciudad,
            .Seccional = funcionario.Seccional,
            .Estudia = estudia,
            .Credencial = funcionario.Credencial,
            .DatosLaborales = datosLaborales,
            .SituacionGeneral = situacionGeneral
        }

        Return dto
    End Function
    ' Normaliza el texto de Estado: True -> Activo, False -> Inactivo
    Private Shared Function EstadoDisplay(valor As Boolean) As String
        Return If(valor, "Activo", "Inactivo")
    End Function
    Public Async Function GetDatosNotificacionAsync(notificacionId As Integer) As Task(Of vw_NotificacionesCompletas)
        Dim notificacion = Await _unitOfWork.Repository(Of vw_NotificacionesCompletas)().
            GetAll().
            FirstOrDefaultAsync(Function(n) n.Id = notificacionId)

        Return notificacion
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
