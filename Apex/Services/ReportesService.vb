Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Linq
Imports System.Threading.Tasks

Public Class ReportesService
    Private ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        _unitOfWork = New UnitOfWork()
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        _unitOfWork = unitOfWork
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

    ' --- ÚNICA fuente de verdad: Activo=True/False -> "Activo"/"Inactivo"
    Private Shared Function EstadoDisplay(valor As Boolean) As String
        Return If(valor, "Activo", "Inactivo")
    End Function

    Public Async Function GetDatosFichaFuncionalAsync(funcionarioId As Integer) As Task(Of FichaFuncionalDTO)
        ' Cargamos el funcionario con sus relaciones (solo lectura)
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

        ' Datos Laborales (usa etiquetas solo si hay valor)
        Dim datosLaborales As String = JoinNonEmpty(" | ",
            Labeled("Grado: ", funcionario.Cargo?.Grado?.ToString()),
            Labeled("Escalafón: ", funcionario.Escalafon?.Nombre),
            Labeled("Unidad: ", funcionario.Seccion?.Nombre),
            Labeled("Puesto de trabajo: ", funcionario.PuestoTrabajo?.Nombre)
        )

        ' Situación general (Estado viene del booleano Activo)
        Dim situacionGeneral As String = JoinNonEmpty(" | ",
            Labeled("Estado: ", EstadoDisplay(funcionario.Activo)),
            Labeled("Turno: ", funcionario.Turno?.Nombre),
            Labeled("Semana: ", funcionario.Semana?.Nombre),
            Labeled("Horario: ", funcionario.Horario?.Nombre)
        )
        Dim fechaIng As Date? = funcionario.FechaIngreso
        Dim fechaIngStr As String = If(fechaIng.HasValue, fechaIng.Value.ToShortDateString(), Nothing)
        ' Mapeo DTO
        Dim dto As New FichaFuncionalDTO With {
            .NombreCompleto = funcionario.Nombre,
            .Cedula = funcionario.CI,
            .FechaNacimiento = funcionario.FechaNacimiento?.ToShortDateString(),
            .Domicilio = funcionario.Domicilio,
            .Telefono = funcionario.Telefono,
            .Correo = funcionario.Email,
            .EstadoCivil = funcionario.EstadoCivil?.Nombre,
            .NivelEstudios = funcionario.NivelEstudio?.Nombre,
          .FechaIngreso = fechaIngStr,
            .Grado = funcionario.Cargo?.Grado?.ToString(),
            .Cargo = funcionario.Cargo?.Nombre,
            .Seccion = funcionario.Seccion?.Nombre,
            .PuestoTrabajo = funcionario.PuestoTrabajo?.Nombre,
            .Turno = funcionario.Turno?.Nombre,
            .Semana = funcionario.Semana?.Nombre,
            .Horario = funcionario.Horario?.Nombre,
            .Estado = EstadoDisplay(funcionario.Activo), ' <- solo booleano
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

    Private Shared Function FmtDate(d As Date?) As String
        Return If(d.HasValue, d.Value.ToShortDateString(), Nothing)
        ' Si prefieres formato fijo:
        ' Return If(d.HasValue, d.Value.ToString("yyyy-MM-dd"), Nothing)
    End Function

    ' Notificación para el RDLC
    Public Async Function GetDatosNotificacionAsync(notificacionId As Integer) As Task(Of vw_NotificacionesCompletas)
        Return Await _unitOfWork.Repository(Of vw_NotificacionesCompletas)().
            GetAll().
            FirstOrDefaultAsync(Function(n) n.Id = notificacionId) ' Cambiar a n.NotificacionId si tu vista lo llama así
    End Function
    ''' <summary>
    ''' Obtiene los datos combinados de una notificación y su detalle de designación para un reporte.
    ''' </summary>
    Public Async Function GetDatosDesignacionAsync(notificacionId As Integer) As Task(Of DesignacionReporteDTO)
        Using uow As New UnitOfWork()
            ' 1. Buscamos la notificación
            Dim notificacion = Await uow.Repository(Of NotificacionPersonal)().GetAll().
                Include(Function(n) n.Funcionario).
                FirstOrDefaultAsync(Function(n) n.Id = notificacionId)

            If notificacion Is Nothing Then Return Nothing

            ' 2. Asumimos que el campo "Documento" de la notificación guarda el ID del EstadoTransitorio.
            Dim estadoTransitorioId As Integer = 0
            If Integer.TryParse(notificacion.Documento, estadoTransitorioId) AndAlso estadoTransitorioId > 0 Then

                ' 3. Buscamos el detalle de la designación (CON LA CORRECCIÓN DE GETALL())
                Dim designacionDetalle = Await uow.Repository(Of DesignacionDetalle)().GetAll().
                    FirstOrDefaultAsync(Function(d) d.EstadoTransitorioId = estadoTransitorioId)

                If designacionDetalle IsNot Nothing Then
                    ' --- INICIO DE LA CORRECCIÓN ---
                    ' 4. Creamos el objeto DTO con los campos correctos de tu entidad
                    Dim dto As New DesignacionReporteDTO With {
                        .NombreFuncionario = notificacion.Funcionario.Nombre,
                        .CedulaFuncionario = notificacion.Funcionario.CI,
                        .FechaProgramada = notificacion.FechaProgramada,
                        .FechaDesde = designacionDetalle.FechaDesde,
                        .FechaHasta = designacionDetalle.FechaHasta,
                        .Observaciones = designacionDetalle.Observaciones,
                        .DocResolucion = designacionDetalle.DocResolucion,
                        .FechaResolucion = designacionDetalle.FechaResolucion,
                        .Expediente = notificacion.ExpMinisterial ' O el campo que corresponda
                    }
                    Return dto
                    ' --- FIN DE LA CORRECCIÓN ---
                End If
            End If

            ' Si no se encuentra el detalle, devolvemos Nothing
            Return Nothing
        End Using
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
    Public Property Estado As String            ' "Activo"/"Inactivo"
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
