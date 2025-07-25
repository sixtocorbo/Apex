Imports System.Data
Imports System.Data.Entity
Imports System.Threading.Tasks

Public Module ConsultasGenericas

    Public Enum TipoOrigenDatos
        Funcionarios
        Presentismo
        ViaticosAltas
        ViaticosBajas
        Designaciones
        Auditoria
        Sumarios
        OrdenCinco
        Enfermedad
        Sanciones
        Notificaciones
        Licencias
        Novedades
        Retenes
    End Enum

    ''' <summary>
    ''' Consulta genérica por tipo de datos y rango de fechas.
    ''' </summary>
    Public Async Function ObtenerDatosGenericosAsync(tipo As TipoOrigenDatos, fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)
        Select Case tipo
            Case TipoOrigenDatos.Designaciones
                Return Await ConsultarDesignaciones(fechaInicio, fechaFin)
            Case TipoOrigenDatos.Sumarios
                Return Await ConsultarSumarios(fechaInicio, fechaFin)
            Case TipoOrigenDatos.OrdenCinco
                Return Await ConsultarOrdenCinco(fechaInicio, fechaFin)
            Case TipoOrigenDatos.Sanciones
                Return Await ConsultarSanciones(fechaInicio, fechaFin)
            Case TipoOrigenDatos.Enfermedad
                Return Await ConsultarEnfermedades(fechaInicio, fechaFin)
            Case Else
                Throw New NotImplementedException($"Consulta no implementada para el tipo {tipo}")
        End Select
    End Function

    '------------------------ DESIGNACIONES ------------------------
    Private Async Function ConsultarDesignaciones(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)

    End Function


    '------------------------ SUMARIOS ------------------------
    Private Async Function ConsultarSumarios(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)

    End Function



    '------------------------ ORDEN CINCO ------------------------
    Private Async Function ConsultarOrdenCinco(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)

    End Function


    '------------------------ SANCIONES ------------------------
    Private Async Function ConsultarSanciones(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)

    End Function

    '------------------------ ENFERMEDADES ------------------------
    Private Async Function ConsultarEnfermedades(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)

    End Function


    '------------------------ AUDITORÍA ------------------------
    Private Async Function ConsultarAuditoria(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)

    End Function

    '------------------------ NOTIFICACIONES ------------------------
    Private Async Function ConsultarNotificaciones(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)

    End Function

    '------------------------ LICENCIAS ------------------------
    Private Async Function ConsultarLicencias(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)

    End Function

    '------------------------ NOVEDADES ------------------------
    Private Async Function ConsultarNovedades(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)

    End Function


    '------------------------ FUNCIONARIOS (CON PRESENTISMO) ------------------------
    Private Async Function ConsultarFuncionarios(fecha As Date) As Task(Of DataTable)

    End Function

End Module

