Imports System.Data
Imports System.Threading.Tasks

Public Module ConsultasGenericas

    Public Enum TipoOrigenDatos
        Funcionarios
        Designaciones
        Sumarios
        OrdenCinco
        Sanciones
        Enfermedad
        Retenes
        Notificaciones
        Licencias
        Novedades
    End Enum

    Public Async Function ObtenerDatosGenericosAsync(tipo As TipoOrigenDatos, fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)
        Select Case tipo
            Case TipoOrigenDatos.Designaciones,
                 TipoOrigenDatos.Sumarios,
                 TipoOrigenDatos.OrdenCinco,
                 TipoOrigenDatos.Sanciones,
                 TipoOrigenDatos.Enfermedad,
                 TipoOrigenDatos.Retenes
                Dim tipoEstado As String = tipo.ToString().Replace("OrdenCinco", "Orden Cinco")
                Dim estadoService = New EstadoTransitorioService()
                Dim estados = Await estadoService.GetAllConDetallesAsync(fechaInicio, fechaFin, tipoEstado)
                Return estados.ToDataTable()

            Case TipoOrigenDatos.Notificaciones
                Dim notificacionService = New NotificacionPersonalService()
                Dim notificaciones = Await notificacionService.GetAllConDetallesAsync()
                Return notificaciones.Where(Function(n) n.FechaProgramada.Date >= fechaInicio And n.FechaProgramada.Date <= fechaFin).ToList().ToDataTable()

            Case TipoOrigenDatos.Licencias
                Dim licenciaService = New LicenciaService()
                Dim licencias = Await licenciaService.GetAllConDetallesAsync(fechaDesde:=fechaInicio, fechaHasta:=fechaFin)
                Return licencias.ToDataTable()

            Case TipoOrigenDatos.Novedades
                Dim novedadService = New NovedadService()
                Dim novedades = Await novedadService.GetAllConDetallesAsync(fechaInicio, fechaFin)
                Return novedades.ToDataTable()

            Case TipoOrigenDatos.Funcionarios
                Dim funcionarioService = New FuncionarioService()
                Dim funcionarios = Await funcionarioService.GetFuncionariosParaVistaAsync()
                Return funcionarios.ToDataTable()

            Case Else
                Throw New NotImplementedException($"Consulta no implementada para el tipo {tipo}")
        End Select
    End Function

    ' --- Todas las funciones privadas de consulta han sido eliminadas ---

End Module