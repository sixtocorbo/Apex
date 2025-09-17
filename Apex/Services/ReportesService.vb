Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Linq
Imports System.Threading.Tasks

Public Class ReportesService
    Implements IDisposable

    Private ReadOnly _unitOfWork As IUnitOfWork
    Private ReadOnly _ownsUnitOfWork As Boolean ' true si este servicio creó la UoW

    Public Sub New()
        _unitOfWork = New UnitOfWork()
        _ownsUnitOfWork = True
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        _unitOfWork = unitOfWork
        _ownsUnitOfWork = False
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

    ' Única fuente de verdad: Activo=True/False -> "Activo"/"Inactivo"
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

        Dim estudia As String = If(funcionario.Estudia, "Sí", "No")

        Dim datosLaborales As String = JoinNonEmpty(" | ",
            Labeled("Grado: ", funcionario.Cargo?.Grado?.ToString()),
            Labeled("Escalafón: ", funcionario.Escalafon?.Nombre),
            Labeled("Unidad: ", funcionario.Seccion?.Nombre),
            Labeled("Puesto de trabajo: ", funcionario.PuestoTrabajo?.Nombre)
        )

        Dim situacionGeneral As String = JoinNonEmpty(" | ",
            Labeled("Estado: ", EstadoDisplay(funcionario.Activo)),
            Labeled("Turno: ", funcionario.Turno?.Nombre),
            Labeled("Semana: ", funcionario.Semana?.Nombre),
            Labeled("Horario: ", funcionario.Horario?.Nombre)
        )

        Dim fechaIng As Date? = funcionario.FechaIngreso
        Dim fechaIngStr As String = If(fechaIng.HasValue, fechaIng.Value.ToShortDateString(), Nothing)

        Dim dto As New FichaFuncionalDTO With {
            .FuncionarioId = funcionario.Id,
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

    Private Shared Function FmtDate(d As Date?) As String
        Return If(d.HasValue, d.Value.ToShortDateString(), Nothing)
        ' O bien: Return If(d.HasValue, d.Value.ToString("yyyy-MM-dd"), Nothing)
    End Function

    ' === Notificación para RDLC ===
    Public Async Function GetDatosNotificacionAsync(notificacionId As Integer) As Task(Of vw_NotificacionesCompletas)
        Return Await _unitOfWork.Repository(Of vw_NotificacionesCompletas)().
            GetAll().
            AsNoTracking().
            FirstOrDefaultAsync(Function(n) n.Id = notificacionId)
        ' Si en tu vista la PK se llama NotificacionId, usa esta línea en su lugar:
        ' .FirstOrDefaultAsync(Function(n) n.NotificacionId = notificacionId)
    End Function

    Public Async Function GetDatosDesignacionAsync(estadoTransitorioId As Integer) As Task(Of DesignacionReporteDTO)
        Using uow As New UnitOfWork()
            Dim estadoCompleto = Await uow.Context.Set(Of vw_EstadosTransitoriosCompletos)() _
                .AsNoTracking() _
                .FirstOrDefaultAsync(Function(e) e.Id = estadoTransitorioId)

            If estadoCompleto Is Nothing Then Return Nothing

            Dim dto As New DesignacionReporteDTO With {
                .NombreFuncionario = estadoCompleto.NombreFuncionario,
                .CedulaFuncionario = estadoCompleto.CI,
                .FechaDesde = estadoCompleto.FechaDesde,
                .FechaHasta = estadoCompleto.FechaHasta,
                .Observaciones = estadoCompleto.Observaciones,
                .FechaProgramada = Nothing
            }
            Return dto
        End Using
    End Function

    ' -------- IDisposable --------
    Private disposedValue As Boolean

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing AndAlso _ownsUnitOfWork Then
                _unitOfWork.Dispose()
            End If
            disposedValue = True
        End If
    End Sub

    ''' <summary>
    ''' Obtiene los datos para una o varias notificaciones para ser usadas en un reporte.
    ''' </summary>
    ''' <param name="notificacionIds">Lista de IDs de las notificaciones a buscar.</param>
    ''' <returns>Una lista de DTOs con la información para el reporte.</returns>
    Public Async Function GetDatosParaNotificacionesAsync(notificacionIds As List(Of Integer)) As Task(Of List(Of NotificacionImprimirDTO))
        ' Si no se reciben IDs, devuelve una lista vacía para evitar errores.
        If notificacionIds Is Nothing OrElse Not notificacionIds.Any() Then
            Return New List(Of NotificacionImprimirDTO)()
        End If

        ' <<< SOLUCIÓN: Creamos un UnitOfWork local para esta operación >>>
        ' Esto asegura que el contexto de la base de datos solo se use aquí y se libere correctamente.
        Using uow As New UnitOfWork()
            ' Realiza una única consulta a la base de datos para obtener todas las notificaciones necesarias.
            Dim datos = Await uow.Context.Set(Of NotificacionPersonal).AsNoTracking().
                Where(Function(n) notificacionIds.Contains(n.Id)).
                Select(Function(n) New NotificacionImprimirDTO With {
                    .FuncionarioId = n.Funcionario.Id,
                    .NombreFuncionario = n.Funcionario.Nombre,
                    .CI = n.Funcionario.CI,
                    .Cargo = If(n.Funcionario.Cargo IsNot Nothing, n.Funcionario.Cargo.Nombre, "N/A"),
                    .Seccion = If(n.Funcionario.Seccion IsNot Nothing, n.Funcionario.Seccion.Nombre, "N/A"),
                    .FechaProgramada = n.FechaProgramada,
                    .Texto = n.Medio, ' 'Medio' contiene el texto principal de la notificación
                    .TipoNotificacion = n.TipoNotificacion.Nombre,
                    .Documento = n.Documento,
                    .ExpMinisterial = n.ExpMinisterial,
                    .ExpINR = n.ExpINR,
                    .Oficina = n.Oficina
                }).
                ToListAsync()

            Return datos
        End Using
    End Function


    ''' <summary>
    ''' Obtiene de forma eficiente los datos para las fichas funcionales de una lista de funcionarios.
    ''' </summary>
    ''' <param name="ids">Lista de Ids de los funcionarios a incluir.</param>
    ''' <returns>Una lista de objetos DTO con la información lista para el reporte.</returns>
    Public Async Function ObtenerDatosParaFichasAsync(ids As List(Of Integer)) As Task(Of List(Of FichaFuncionalDTO))
        If ids Is Nothing OrElse Not ids.Any() Then
            Return New List(Of FichaFuncionalDTO)()
        End If

        Using uow As New UnitOfWork()
            ' 1. Obtenemos todos los funcionarios y sus datos relacionados en UNA SOLA CONSULTA.
            Dim funcionarios = Await uow.Repository(Of Funcionario)().
            GetQueryable().
            Where(Function(f) ids.Contains(f.Id)).
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
            ToListAsync()

            ' 2. Transformamos la lista de entidades a una lista de DTOs en memoria.
            '    Reutilizamos toda la lógica de tu función original aquí.
            Dim dtos = funcionarios.Select(Function(f)
                                               Dim estudia As String = If(f.Estudia, "Sí", "No")

                                               Dim datosLaborales As String = JoinNonEmpty(" | ",
                Labeled("Grado: ", f.Cargo?.Grado?.ToString()),
                Labeled("Escalafón: ", f.Escalafon?.Nombre),
                Labeled("Unidad: ", f.Seccion?.Nombre),
                Labeled("Puesto de trabajo: ", f.PuestoTrabajo?.Nombre)
            )

                                               Dim situacionGeneral As String = JoinNonEmpty(" | ",
                Labeled("Estado: ", EstadoDisplay(f.Activo)),
                Labeled("Turno: ", f.Turno?.Nombre),
                Labeled("Semana: ", f.Semana?.Nombre),
                Labeled("Horario: ", f.Horario?.Nombre)
            )

                                               Dim fechaIng As Date? = f.FechaIngreso
                                               Dim fechaIngStr As String = If(fechaIng.HasValue, fechaIng.Value.ToShortDateString(), Nothing)

                                               Return New FichaFuncionalDTO With {
                .FuncionarioId = f.Id,
                .NombreCompleto = f.Nombre,
                .Cedula = f.CI,
                .FechaNacimiento = f.FechaNacimiento?.ToShortDateString(),
                .Domicilio = f.Domicilio,
                .Telefono = f.Telefono,
                .Correo = f.Email,
                .EstadoCivil = f.EstadoCivil?.Nombre,
                .NivelEstudios = f.NivelEstudio?.Nombre,
                .FechaIngreso = fechaIngStr,
                .Grado = f.Cargo?.Grado?.ToString(),
                .Cargo = f.Cargo?.Nombre,
                .Seccion = f.Seccion?.Nombre,
                .PuestoTrabajo = f.PuestoTrabajo?.Nombre,
                .Turno = f.Turno?.Nombre,
                .Semana = f.Semana?.Nombre,
                .Horario = f.Horario?.Nombre,
                .Estado = EstadoDisplay(f.Activo),
                .Escalafon = f.Escalafon?.Nombre,
                .Funcion = f.Funcion?.Nombre,
                .Foto = f.Foto,
                .Sexo = f.Genero?.Nombre,
                .Ciudad = f.Ciudad,
                .Seccional = f.Seccional,
                .Estudia = estudia,
                .Credencial = f.Credencial,
                .DatosLaborales = datosLaborales,
                .SituacionGeneral = situacionGeneral
            }
                                           End Function).ToList()

            Return dtos
        End Using
    End Function
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class

' --- DTOs para la Ficha Funcional (versión simplificada) ---
Public Class FichaFuncionalDTO
    Public Property FuncionarioId As Integer
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

' (Opcional) DTO específico si algún RDLC lo requiere
Public Class NotificacionReporteDTO
    Public Property NombreFuncionario As String
    Public Property CedulaFuncionario As String
    Public Property FechaProgramada As DateTime
    Public Property Texto As String
    Public Property TipoNotificacion As String
End Class
