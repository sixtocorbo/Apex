' Apex/Services/ConsultasGenericas.vb
Imports System.Data
Imports System.Threading.Tasks

Public Module ConsultasGenericas

    ' Se mantienen los nombres del Enum sin tildes para simplificar, 
    ' pero la lógica de mapeo usará los valores correctos.
    Public Enum TipoOrigenDatos
        Funcionarios
        Designacion
        Sumario
        OrdenCinco
        Sancion
        Enfermedad
        Reten
        Notificaciones
        Licencias
        Novedades
    End Enum

    Public Async Function ObtenerDatosGenericosAsync(tipo As TipoOrigenDatos, fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)
        Select Case tipo
            Case TipoOrigenDatos.Designacion,
                 TipoOrigenDatos.Sumario,
                 TipoOrigenDatos.OrdenCinco,
                 TipoOrigenDatos.Sancion,
                 TipoOrigenDatos.Enfermedad,
                 TipoOrigenDatos.Reten

                ' --- INICIO DE LA CORRECCIÓN ---
                ' Mapeo explícito a los valores exactos de la base de datos, incluyendo tildes.
                Dim tipoEstado As String
                Select Case tipo
                    Case TipoOrigenDatos.Designacion
                        tipoEstado = "Designación"
                    Case TipoOrigenDatos.Sancion
                        tipoEstado = "Sanción"
                    Case TipoOrigenDatos.OrdenCinco
                        tipoEstado = "Orden Cinco"
                    Case TipoOrigenDatos.Reten
                        tipoEstado = "Retén"
                    Case Else
                        ' Para Sumario y Enfermedad, que no llevan tilde.
                        tipoEstado = tipo.ToString()
                End Select
                ' --- FIN DE LA CORRECCIÓN ---

                Dim estadoService = New EstadoTransitorioService()
                Dim estados = Await estadoService.GetAllConDetallesAsync(fechaInicio, fechaFin, tipoEstado)
                Return estados.ToDataTable()

            Case TipoOrigenDatos.Notificaciones
                Dim notificacionService = New NotificacionService()
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
End Module