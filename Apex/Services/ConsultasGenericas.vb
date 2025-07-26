Imports System.Data
Imports System.Data.Entity
Imports System.Threading.Tasks
Imports System.Reflection

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
                Return Await ConsultarEstadoTransitorio(fechaInicio, fechaFin, "Designación")
            Case TipoOrigenDatos.Sumarios
                Return Await ConsultarEstadoTransitorio(fechaInicio, fechaFin, "Sumario")
            Case TipoOrigenDatos.OrdenCinco
                Return Await ConsultarEstadoTransitorio(fechaInicio, fechaFin, "Orden Cinco")
            Case TipoOrigenDatos.Sanciones
                Return Await ConsultarEstadoTransitorio(fechaInicio, fechaFin, "Sanción")
            Case TipoOrigenDatos.Enfermedad
                Return Await ConsultarEstadoTransitorio(fechaInicio, fechaFin, "Enfermedad")
            Case TipoOrigenDatos.Retenes
                Return Await ConsultarEstadoTransitorio(fechaInicio, fechaFin, "Retén")
            Case TipoOrigenDatos.Notificaciones
                Return Await ConsultarNotificaciones(fechaInicio, fechaFin)
            Case TipoOrigenDatos.Licencias
                Return Await ConsultarLicencias(fechaInicio, fechaFin)
            Case TipoOrigenDatos.Novedades
                Return Await ConsultarNovedades(fechaInicio, fechaFin)
            Case TipoOrigenDatos.Funcionarios
                Return Await ConsultarFuncionarios(fechaInicio)
            Case Else
                Throw New NotImplementedException($"Consulta no implementada para el tipo {tipo}")
        End Select
    End Function

    Private Async Function ConsultarEstadoTransitorio(fechaInicio As Date, fechaFin As Date, tipoEstado As String) As Task(Of DataTable)
        Using uow As New UnitOfWork()
            Dim query = uow.Repository(Of EstadoTransitorio)().GetAll().
                Where(Function(et) et.TipoEstadoTransitorio.Nombre = tipoEstado And et.FechaDesde >= fechaInicio And et.FechaDesde <= fechaFin).
                Select(Function(et) New With {
                    .Funcionario = et.Funcionario.Nombre,
                    et.Funcionario.CI,
                    .Desde = et.FechaDesde,
                    .Hasta = et.FechaHasta,
                    .Observaciones = et.Observaciones
                })
            Dim result = Await query.ToListAsync()
            Return ToDataTable(result)
        End Using
    End Function

    Private Async Function ConsultarNotificaciones(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)
        Using uow As New UnitOfWork()
            Dim fechaFinInclusive = fechaFin.AddDays(1)

            Dim query = uow.Repository(Of NotificacionPersonal)().GetAll().
            Where(Function(n) n.FechaProgramada >= fechaInicio And n.FechaProgramada < fechaFinInclusive).
            Select(Function(n) New With {
                .Id = n.Id, ' <-- CORRECCIÓN AÑADIDA
                .Funcionario = If(n.Funcionario IsNot Nothing, n.Funcionario.Nombre, "N/A"),
                .CI = If(n.Funcionario IsNot Nothing, n.Funcionario.CI, "N/A"),
                .Tipo = If(n.TipoNotificacion IsNot Nothing, n.TipoNotificacion.Nombre, "N/A"),
                .Fecha = n.FechaProgramada,
                .Estado = If(n.NotificacionEstado IsNot Nothing, n.NotificacionEstado.Nombre, "N/A"),
                .Texto = n.Medio
            })
            Dim result = Await query.ToListAsync()
            Return ToDataTable(result)
        End Using
    End Function

    Private Async Function ConsultarLicencias(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)
        Using uow As New UnitOfWork()
            Dim query = uow.Repository(Of HistoricoLicencia)().GetAll().
            Where(Function(l) l.inicio <= fechaFin And l.finaliza >= fechaInicio).
            Select(Function(l) New With {
                .Id = l.Id, ' <-- CORRECCIÓN AÑADIDA
                .Funcionario = If(l.Funcionario IsNot Nothing, l.Funcionario.Nombre, "FUNCIONARIO NO ENCONTRADO"),
                .CI = If(l.Funcionario IsNot Nothing, l.Funcionario.CI, "N/A"),
                .Tipo = If(l.TipoLicencia IsNot Nothing, l.TipoLicencia.Nombre, "TIPO NO ENCONTRADO"),
                .Desde = l.inicio,
                .Hasta = l.finaliza,
                l.estado,
                .Comentario = l.Comentario
            })
            Dim result = Await query.ToListAsync()
            Return ToDataTable(result)
        End Using
    End Function
    Private Async Function ConsultarNovedades(fechaInicio As Date, fechaFin As Date) As Task(Of DataTable)
        Using uow As New UnitOfWork()
            Dim query = uow.Repository(Of Novedad)().GetAll().
                Where(Function(n) n.Fecha >= fechaInicio And n.Fecha <= fechaFin).
                SelectMany(Function(n) n.NovedadFuncionario.Select(Function(nf) New With {
                    .Funcionario = nf.Funcionario.Nombre,
                    nf.Funcionario.CI,
                    n.Fecha,
                    .Estado = n.NotificacionEstado.Nombre,
                    n.Texto
                }))
            Dim result = Await query.ToListAsync()
            Return ToDataTable(result)
        End Using
    End Function

    ' Services/ConsultasGenericas.vb

    Private Async Function ConsultarFuncionarios(fecha As Date) As Task(Of DataTable)
        Using uow As New UnitOfWork()
            Dim query = uow.Repository(Of Funcionario)().GetAll().
        Select(Function(f) New With {
            f.Nombre,
            f.CI,            ' CORRECCIÓN: Usar una expresión condicional para obtener el nombre o un guion si es nulo.
            .Cargo = If(f.Cargo IsNot Nothing, f.Cargo.Nombre, "-"),
            .TipoFuncionario = If(f.TipoFuncionario IsNot Nothing, f.TipoFuncionario.Nombre, "-"),
            f.FechaIngreso,
            f.Activo,
            .Correo = f.Email
        })
            Dim result = Await query.ToListAsync()
            Return ToDataTable(result)
        End Using
    End Function
End Module