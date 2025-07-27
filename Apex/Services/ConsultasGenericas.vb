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
    ''' Clase auxiliar para devolver resultados paginados.
    ''' </summary>
    Public Class ResultadoPaginado(Of T)
        Public Property Items As List(Of T)
        Public Property TotalItems As Integer
    End Class

    ''' <summary>
    ''' Consulta genérica por tipo de datos y rango de fechas. (Función principal original)
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
                ' La llamada a la versión paginada se haría desde el formulario.
                ' Por compatibilidad, mantenemos la llamada original que trae todo.
                Return Await ConsultarLicencias(fechaInicio, fechaFin)
            Case TipoOrigenDatos.Novedades
                Return Await ConsultarNovedades(fechaInicio, fechaFin)
            Case TipoOrigenDatos.Funcionarios
                Return Await ConsultarFuncionarios(fechaInicio)
            Case Else
                Throw New NotImplementedException($"Consulta no implementada para el tipo {tipo}")
        End Select
    End Function

    ''' <summary>
    ''' NUEVA FUNCIÓN: Consulta de licencias con paginación.
    ''' </summary>
    Public Async Function ConsultarLicenciasPaginado(
        fechaInicio As Date,
        fechaFin As Date,
        pagina As Integer,
        tamañoPagina As Integer
    ) As Task(Of ResultadoPaginado(Of Object))

        Using uow As New UnitOfWork()
            ' 1. Construir la consulta base con los filtros
            Dim queryBase = uow.Repository(Of HistoricoLicencia)().GetAll().
                Where(Function(l) l.inicio <= fechaFin And l.finaliza >= fechaInicio)

            ' 2. Contar el total de registros SIN paginar
            Dim totalItems = Await queryBase.CountAsync()

            ' 3. Aplicar ordenamiento, paginación y seleccionar los datos
            Dim itemsPaginados = Await queryBase.
                OrderByDescending(Function(l) l.inicio).
                Skip((pagina - 1) * tamañoPagina).
                Take(tamañoPagina).
                Select(Function(l) New With {
                    .Id = l.Id,
                    .Funcionario = If(l.Funcionario IsNot Nothing, l.Funcionario.Nombre, "N/A"),
                    .CI = If(l.Funcionario IsNot Nothing, l.Funcionario.CI, "N/A"),
                    .Tipo = If(l.TipoLicencia IsNot Nothing, l.TipoLicencia.Nombre, "N/A"),
                    .Desde = l.inicio,
                    .Hasta = l.finaliza,
                    l.estado,
                    .Comentario = l.Comentario
                }).ToListAsync()

            ' 4. Devolver el resultado paginado
            Return New ResultadoPaginado(Of Object) With {
                .Items = itemsPaginados.Cast(Of Object).ToList(),
                .TotalItems = totalItems
            }
        End Using
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
                    .Id = n.Id,
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
                    .Id = l.Id,
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

    Private Async Function ConsultarFuncionarios(fecha As Date) As Task(Of DataTable)
        Using uow As New UnitOfWork()
            Dim query = uow.Repository(Of Funcionario)().GetAll().
                Select(Function(f) New With {
                    f.Nombre,
                    f.CI,
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