' Apex/Services/NovedadService.vb
Imports System.Data.Entity
Imports System.Data.SqlClient ' <--- IMPORTANTE: Añadir esta línea
Imports System.IO
Imports System.Text

Public Class NovedadService
    Inherits GenericService(Of Novedad)

    Private Shadows ReadOnly _unitOfWork As IUnitOfWork
    Private ReadOnly _auditoriaService As AuditoriaService

    Public Sub New()
        MyBase.New(New UnitOfWork())
        _unitOfWork = MyBase._unitOfWork
        _auditoriaService = New AuditoriaService(_unitOfWork)
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
        _unitOfWork = unitOfWork
        _auditoriaService = New AuditoriaService(_unitOfWork)
    End Sub

    ' --- Helper interno para que la auditoría nunca “rompa” la operación principal ---
    Private Async Function RegistrarAuditoriaSeguraAsync(accion As String,
                                                         tipoEntidad As String,
                                                         descripcion As String,
                                                         Optional entidadId As Integer? = Nothing) As Task
        Try
            Await _auditoriaService.RegistrarActividadAsync(accion, tipoEntidad, descripcion, entidadId)
        Catch
            ' No re-lanzamos: la auditoría no debe impedir la operación principal.
            ' Podés loguear a archivo/telemetría aquí si querés.
        End Try
    End Function

    ''' <summary>
    ''' Obtiene las novedades AGRUPADAS para la grilla principal usando la nueva vista.
    ''' </summary>
    Public Async Function GetAllAgrupadasAsync(Optional fechaInicio As Date? = Nothing, Optional fechaFin As Date? = Nothing) As Task(Of List(Of vw_NovedadesAgrupadas))
        Dim query = _unitOfWork.Repository(Of vw_NovedadesAgrupadas)().GetAll().AsNoTracking()
        If fechaInicio.HasValue Then
            query = query.Where(Function(n) n.Fecha >= fechaInicio.Value)
        End If
        If fechaFin.HasValue Then
            query = query.Where(Function(n) n.Fecha <= fechaFin.Value)
        End If

        Dim lista = Await query.OrderByDescending(Function(n) n.Fecha).ThenBy(Function(n) n.Id).ToListAsync()

        ' AUDITORÍA
        Dim rango As String = $"rango: {If(fechaInicio.HasValue, fechaInicio.Value.ToString("yyyy-MM-dd"), "sin inicio")} - {If(fechaFin.HasValue, fechaFin.Value.ToString("yyyy-MM-dd"), "sin fin")}"
        Await RegistrarAuditoriaSeguraAsync(
            accion:="Listar",
            tipoEntidad:="Novedad",
            descripcion:=$"Se listaron {lista.Count} novedades agrupadas ({rango})."
        )

        Return lista
    End Function

    ' --- MÉTODO DE BÚSQUEDA AVANZADA (NUEVO Y CENTRALIZADO) ---
    ''' <summary>
    ''' Realiza una búsqueda avanzada de novedades combinando Full-Text Search, rango de fechas y filtro por funcionarios.
    ''' </summary>
    ''' <returns>Una lista de novedades agrupadas que coinciden con los criterios.</returns>
    Public Async Function BuscarNovedadesAvanzadoAsync(
        textoBusqueda As String,
        fechaDesde As DateTime?,
        fechaHasta As DateTime?,
        idsFuncionarios As List(Of Integer)
    ) As Task(Of List(Of vw_NovedadesAgrupadas))

        ' --- Caso 1: Sin filtros, devolver las últimas 200 (comportamiento por defecto) ---
        If String.IsNullOrWhiteSpace(textoBusqueda) AndAlso Not fechaDesde.HasValue AndAlso (idsFuncionarios Is Nothing OrElse Not idsFuncionarios.Any()) Then
            Dim listaSinFiltro = Await _unitOfWork.Repository(Of vw_NovedadesAgrupadas).GetAll().
                                     OrderByDescending(Function(n) n.Fecha).ThenByDescending(Function(n) n.Id).
                                     Take(200).
                                     ToListAsync()
            ' AUDITORÍA
            Await RegistrarAuditoriaSeguraAsync(
                accion:="BuscarAvanzado",
                tipoEntidad:="Novedad",
                descripcion:=$"Búsqueda sin filtros. Se devuelven las últimas {listaSinFiltro.Count} novedades."
            )
            Return listaSinFiltro
        End If

        ' --- Caso 2: Búsqueda con filtros, construir consulta dinámica ---
        Dim sqlBuilder As New StringBuilder()
        Dim parameters As New List(Of SqlParameter)()
        Dim whereClauses As New List(Of String)()

        sqlBuilder.AppendLine("SELECT DISTINCT v.Id, v.Fecha, v.Resumen, v.Funcionarios, v.Estado")
        sqlBuilder.AppendLine("FROM dbo.vw_NovedadesAgrupadas AS v")

        ' --- Filtro de Texto (FTS) ---
        If Not String.IsNullOrWhiteSpace(textoBusqueda) Then
            sqlBuilder.AppendLine("INNER JOIN dbo.Novedad AS n ON v.Id = n.Id") ' Necesario para buscar en n.Texto

            Dim terminos = textoBusqueda.Split({" "c}, StringSplitOptions.RemoveEmptyEntries) _
                                        .Select(Function(w) $"""{w.Trim()}*""")
            Dim expresionFts = String.Join(" AND ", terminos)
            parameters.Add(New SqlParameter("@patronFTS", expresionFts))

            Dim ftsCondition = " (CONTAINS(n.Texto, @patronFTS) OR " &
                               "  EXISTS (SELECT 1 FROM dbo.NovedadFuncionario nf JOIN dbo.Funcionario f ON nf.FuncionarioId = f.Id WHERE nf.NovedadId = v.Id AND CONTAINS(f.Nombre, @patronFTS)))"
            whereClauses.Add(ftsCondition)
        End If

        ' --- Filtro de Fechas ---
        If fechaDesde.HasValue Then
            whereClauses.Add("v.Fecha >= @fechaDesde")
            parameters.Add(New SqlParameter("@fechaDesde", fechaDesde.Value))
        End If
        If fechaHasta.HasValue Then

            whereClauses.Add("v.Fecha <= @fechaHasta")
            parameters.Add(New SqlParameter("@fechaHasta", fechaHasta.Value))
        End If

        ' --- Filtro de Funcionarios ---
        If idsFuncionarios IsNot Nothing AndAlso idsFuncionarios.Any() Then
            ' Se crea una subconsulta que asegura que la novedad contenga AL MENOS UNO de los funcionarios seleccionados.
            Dim funcCondition = $"v.Id IN (SELECT nf.NovedadId FROM dbo.NovedadFuncionario nf WHERE nf.FuncionarioId IN ({String.Join(",", idsFuncionarios)}))"
            whereClauses.Add(funcCondition)
            ' Nota: Para listas muy grandes de funcionarios, un TVP (Table-Valued Parameter) sería más eficiente,
            ' pero para una selección manual de UI, esto es perfectamente adecuado y simple.
        End If

        ' --- Ensamblar la consulta final ---
        If whereClauses.Any() Then
            sqlBuilder.AppendLine("WHERE " & String.Join(" AND ", whereClauses))
        End If

        sqlBuilder.AppendLine("ORDER BY v.Fecha DESC, v.Id DESC")

        Dim lista = Await _unitOfWork.Context.Database.SqlQuery(Of vw_NovedadesAgrupadas)(sqlBuilder.ToString(), parameters.ToArray()).ToListAsync()

        ' AUDITORÍA
        Dim descAuditoria = $"Búsqueda con filtros: Texto='{textoBusqueda}', FechaDesde='{fechaDesde}', FechaHasta='{fechaHasta}', Funcionarios='{idsFuncionarios?.Count}'. Resultados: {lista.Count}."
        Await RegistrarAuditoriaSeguraAsync(
            accion:="BuscarAvanzado",
            tipoEntidad:="Novedad",
            descripcion:=descAuditoria
        )

        Return lista
    End Function


    ''' <summary>
    ''' Crea una nueva novedad junto con sus funcionarios asociados en una única transacción.
    ''' Primero asegura la existencia de una NovedadGenerada para la fecha dada.
    ''' </summary>
    Public Async Function CrearNovedadCompletaAsync(fecha As Date, texto As String, funcionarioIds As List(Of Integer)) As Task(Of Novedad)
        ' 1. Buscar o crear el contenedor NovedadGenerada para la fecha.
        Dim repoGenerada = _unitOfWork.Repository(Of NovedadGenerada)()
        Dim novedadDelDia = Await repoGenerada.GetByPredicateAsync(Function(ng) ng.Fecha = fecha.Date)

        Dim seCreoContenedor As Boolean = False
        If novedadDelDia Is Nothing Then
            ' Si no existe para esta fecha, la creamos.
            novedadDelDia = New NovedadGenerada With {
                .Fecha = fecha.Date,
                .CreatedAt = DateTime.Now
            }
            repoGenerada.Add(novedadDelDia)
            Await _unitOfWork.CommitAsync() ' Guardamos para obtener su ID.
            seCreoContenedor = True

            ' AUDITORÍA del contenedor
            Await RegistrarAuditoriaSeguraAsync(
                accion:="CrearContenedor",
                tipoEntidad:="NovedadGenerada",
                descripcion:=$"Se creó NovedadGenerada para la fecha {fecha:yyyy-MM-dd} con Id #{novedadDelDia.Id}.",
                entidadId:=novedadDelDia.Id
            )
        End If

        ' 2. Crear la Novedad específica, ahora con el ID del contenedor correcto.
        Dim nuevaNovedad = New Novedad With {
            .Fecha = fecha,
            .Texto = texto,
            .EstadoId = 1, ' Por defecto: "Pendiente"
            .CreatedAt = DateTime.Now,
            .NovedadGeneradaId = novedadDelDia.Id ' <-- Asignación del ID correcto
        }

        For Each funcId In funcionarioIds
            nuevaNovedad.NovedadFuncionario.Add(New NovedadFuncionario With {.FuncionarioId = funcId})
        Next

        _unitOfWork.Repository(Of Novedad)().Add(nuevaNovedad)
        Await _unitOfWork.CommitAsync()

        ' AUDITORÍA de la novedad creada
        Await RegistrarAuditoriaSeguraAsync(
            accion:="Crear",
            tipoEntidad:="Novedad",
            descripcion:=$"Se creó la novedad #{nuevaNovedad.Id} (fecha {fecha:yyyy-MM-dd}) con {funcionarioIds?.Count} funcionario(s). Contenedor creado: {seCreoContenedor}.",
            entidadId:=nuevaNovedad.Id
        )

        Return nuevaNovedad
    End Function


    ''' <summary>
    ''' Elimina una novedad y todas sus relaciones (funcionarios, fotos) de forma explícita y transaccional.
    ''' </summary>
    Public Async Function DeleteNovedadCompletaAsync(novedadId As Integer) As Task
        ' Cargar la novedad con sus dependencias
        Dim novedadAEliminar = Await _unitOfWork.Context.Set(Of Novedad)().
        Include(Function(n) n.NovedadFuncionario).
        Include(Function(n) n.NovedadFoto).
        SingleOrDefaultAsync(Function(n) n.Id = novedadId)

        If novedadAEliminar Is Nothing Then
            Return
        End If

        ' === Declaraciones locales ANTES de eliminar (para usar en la auditoría) ===
        Dim cantFuncs As Integer = If(novedadAEliminar.NovedadFuncionario IsNot Nothing,
                                  novedadAEliminar.NovedadFuncionario.Count, 0)
        Dim cantFotos As Integer = If(novedadAEliminar.NovedadFoto IsNot Nothing,
                                  novedadAEliminar.NovedadFoto.Count, 0)
        Dim idEliminado As Integer = novedadAEliminar.Id

        ' Eliminar explícitamente dependencias
        Dim repoFuncionarios = _unitOfWork.Repository(Of NovedadFuncionario)()
        Dim repoFotos = _unitOfWork.Repository(Of NovedadFoto)()

        repoFuncionarios.RemoveRange(novedadAEliminar.NovedadFuncionario.ToList())
        repoFotos.RemoveRange(novedadAEliminar.NovedadFoto.ToList())

        ' Eliminar la novedad principal
        _unitOfWork.Repository(Of Novedad)().Remove(novedadAEliminar)

        ' Persistir todo
        Await _unitOfWork.CommitAsync()

        ' Auditoría (usa las variables locales declaradas arriba)
        Await RegistrarAuditoriaSeguraAsync(
        accion:="Eliminar",
        tipoEntidad:="Novedad",
        descripcion:=$"Se eliminó la novedad #{idEliminado} con {cantFuncs} funcionario(s) y {cantFotos} foto(s).",
        entidadId:=idEliminado
    )
    End Function


    ''' <summary>
    ''' Obtiene la información completa de una lista de novedades a partir de sus IDs.
    ''' </summary>
    Public Async Function GetNovedadesCompletasByIds(novedadIds As List(Of Integer)) As Task(Of List(Of vw_NovedadesCompletas))
        Using uow As New UnitOfWork()
            Dim repo = uow.Repository(Of vw_NovedadesCompletas)()
            Dim lista = Await repo.GetAll().Where(Function(n) novedadIds.Contains(n.Id)).ToListAsync()

            ' AUDITORÍA
            Await RegistrarAuditoriaSeguraAsync(
                accion:="ConsultarDetalle",
                tipoEntidad:="Novedad",
                descripcion:=$"Se consultaron detalles de {lista.Count} novedad(es). IDs: {String.Join(",", novedadIds)}."
            )

            Return lista
        End Using
    End Function

    ''' <summary>
    ''' Actualiza una novedad existente, sincronizando la lista de funcionarios. (Versión Definitiva)
    ''' </summary>
    Public Async Function ActualizarNovedadCompletaAsync(novedadActualizada As Novedad, nuevosFuncionarioIds As List(Of Integer)) As Task
        ' 1. Obtener la Novedad original desde la base de datos CON SEGUIMIENTO.
        Dim novedadEnDb = Await _unitOfWork.Context.Set(Of Novedad)().
            Include(Function(n) n.NovedadFuncionario).
            SingleOrDefaultAsync(Function(n) n.Id = novedadActualizada.Id)

        If novedadEnDb Is Nothing Then
            Throw New KeyNotFoundException("La novedad que intenta actualizar ya no existe en la base de datos.")
        End If

        ' 2. Actualizar las propiedades escalares (campos simples) de forma manual.
        novedadEnDb.Fecha = novedadActualizada.Fecha
        novedadEnDb.Texto = novedadActualizada.Texto

        ' 3. Sincronizar la lista de funcionarios.
        Dim idsActuales = novedadEnDb.NovedadFuncionario.Select(Function(nf) nf.FuncionarioId).ToList()
        Dim idsParaQuitar = idsActuales.Except(nuevosFuncionarioIds).ToList()
        Dim idsParaAgregar = nuevosFuncionarioIds.Except(idsActuales).ToList()

        ' Quitar explícitamente los registros de la tabla de unión.
        If idsParaQuitar.Any() Then
            Dim relacionesAQuitar = novedadEnDb.NovedadFuncionario.Where(Function(nf) idsParaQuitar.Contains(nf.FuncionarioId)).ToList()
            _unitOfWork.Repository(Of NovedadFuncionario)().RemoveRange(relacionesAQuitar)
        End If

        ' Agregar los nuevos.
        For Each funcId In idsParaAgregar
            novedadEnDb.NovedadFuncionario.Add(New NovedadFuncionario With {
                .FuncionarioId = funcId
            })
        Next

        ' 4. Guardar todos los cambios en una única transacción.
        Await _unitOfWork.CommitAsync()

        ' AUDITORÍA
        Await RegistrarAuditoriaSeguraAsync(
            accion:="Actualizar",
            tipoEntidad:="Novedad",
            descripcion:=$"Se actualizó la novedad #{novedadEnDb.Id}. Funcionarios: {nuevosFuncionarioIds?.Count}.",
            entidadId:=novedadEnDb.Id
        )
    End Function

    ''' <summary>
    ''' Obtiene la lista de funcionarios asociados a una novedad específica.
    ''' </summary>
    Public Async Function GetFuncionariosPorNovedadAsync(novedadId As Integer) As Task(Of List(Of Funcionario))
        Dim lista = Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking().
            Where(Function(f) f.NovedadFuncionario.Any(Function(nf) nf.NovedadId = novedadId)).
            OrderBy(Function(f) f.Nombre).
            ToListAsync()

        ' AUDITORÍA
        Await RegistrarAuditoriaSeguraAsync(
            accion:="ConsultarFuncionariosDeNovedad",
            tipoEntidad:="Novedad",
            descripcion:=$"Se consultaron {lista.Count} funcionario(s) para la novedad #{novedadId}.",
            entidadId:=novedadId
        )

        Return lista
    End Function

    ''' <summary>
    ''' Obtiene las fotos asociadas a una novedad específica.
    ''' </summary>
    Public Async Function GetFotosPorNovedadAsync(novedadId As Integer) As Task(Of List(Of NovedadFoto))
        Dim lista = Await _unitOfWork.Repository(Of NovedadFoto)().GetAll().AsNoTracking().
            Where(Function(nf) nf.NovedadId = novedadId).
            ToListAsync()

        ' AUDITORÍA
        Await RegistrarAuditoriaSeguraAsync(
            accion:="ConsultarFotosDeNovedad",
            tipoEntidad:="Novedad",
            descripcion:=$"Se consultaron {lista.Count} foto(s) para la novedad #{novedadId}.",
            entidadId:=novedadId
        )

        Return lista
    End Function

    ''' <summary>
    ''' Agrega una foto a una novedad existente. (Versión Simplificada)
    ''' </summary>
    Public Async Function AddFotoAsync(novedadId As Integer, rutaArchivo As String) As Task
        Dim fotoBytes = File.ReadAllBytes(rutaArchivo)

        Dim nuevaFoto = New NovedadFoto With {
            .NovedadId = novedadId,
            .Foto = fotoBytes,
            .FileName = Path.GetFileName(rutaArchivo),
            .CreatedAt = DateTime.Now
        }

        _unitOfWork.Repository(Of NovedadFoto)().Add(nuevaFoto)
        Await _unitOfWork.CommitAsync()

        ' AUDITORÍA
        Await RegistrarAuditoriaSeguraAsync(
            accion:="AgregarFoto",
            tipoEntidad:="Novedad",
            descripcion:=$"Se agregó la foto '{Path.GetFileName(rutaArchivo)}' a la novedad #{novedadId}.",
            entidadId:=novedadId
        )
    End Function

    ''' <summary>
    ''' Elimina una foto específica por su ID.
    ''' </summary>
    Public Async Function DeleteFotoAsync(fotoId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFoto)()
        Dim foto = Await repo.GetByIdAsync(fotoId)

        If foto Is Nothing Then
            Return
        End If

        ' Si NovedadId es NOT NULL en la BD:
        Dim novedadIdDeLaFoto As Integer = foto.NovedadId

        repo.Remove(foto)
        Await _unitOfWork.CommitAsync()

        Await RegistrarAuditoriaSeguraAsync(
        accion:="EliminarFoto",
        tipoEntidad:="Novedad",
        descripcion:=$"Se eliminó la foto #{fotoId} de la novedad #{novedadIdDeLaFoto}.",
        entidadId:=novedadIdDeLaFoto
    )
    End Function

    ''' <summary>
    ''' Agrega un funcionario a una novedad que ya existe.
    ''' </summary>
    Public Async Function AgregarFuncionarioANovedadAsync(novedadId As Integer, funcionarioId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFuncionario)()
        Dim existe = Await repo.AnyAsync(Function(nf) nf.NovedadId = novedadId AndAlso nf.FuncionarioId = funcionarioId)

        If Not existe Then
            repo.Add(New NovedadFuncionario With {
                .NovedadId = novedadId,
                .FuncionarioId = funcionarioId
            })
            Await _unitOfWork.CommitAsync()

            ' AUDITORÍA
            Await RegistrarAuditoriaSeguraAsync(
                accion:="AgregarFuncionario",
                tipoEntidad:="Novedad",
                descripcion:=$"Se agregó el funcionario #{funcionarioId} a la novedad #{novedadId}.",
                entidadId:=novedadId
            )
        End If
    End Function

    ''' <summary>
    ''' Quita a un funcionario de una novedad existente.
    ''' </summary>
    Public Async Function QuitarFuncionarioDeNovedadAsync(novedadId As Integer, funcionarioId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFuncionario)()
        Dim relacion = Await repo.GetByPredicateAsync(Function(nf) nf.NovedadId = novedadId AndAlso nf.FuncionarioId = funcionarioId)
        If relacion IsNot Nothing Then
            repo.Remove(relacion)
            Await _unitOfWork.CommitAsync()

            ' AUDITORÍA
            Await RegistrarAuditoriaSeguraAsync(
                accion:="QuitarFuncionario",
                tipoEntidad:="Novedad",
                descripcion:=$"Se quitó el funcionario #{funcionarioId} de la novedad #{novedadId}.",
                entidadId:=novedadId
            )
        End If
    End Function
    ''' <summary>
    ''' Obtiene todos los datos necesarios (incluyendo funcionarios y fotos) para el reporte detallado.
    ''' </summary>
    Public Async Function GetNovedadesParaReporteAsync(novedadIds As List(Of Integer)) As Task(Of List(Of NovedadReporteDTO))
        If novedadIds Is Nothing OrElse Not novedadIds.Any() Then
            Return New List(Of NovedadReporteDTO)()
        End If

        ' Usamos Include para traer todas las entidades relacionadas en una sola consulta
        Dim novedadesConTodo = Await _unitOfWork.Repository(Of Novedad)().GetAll().
            Include(Function(n) n.NovedadFuncionario.Select(Function(nf) nf.Funcionario)).
            Include(Function(n) n.NovedadFoto).
            Where(Function(n) novedadIds.Contains(n.Id)).
            ToListAsync()

        ' Mapeamos los resultados a nuestra estructura de DTOs
        Dim resultado As New List(Of NovedadReporteDTO)()
        For Each n In novedadesConTodo
            Dim dto = New NovedadReporteDTO With {
                .Id = n.Id,
                .Fecha = n.Fecha,
                .Texto = n.Texto
            }

            ' Mapear funcionarios
            For Each funcRel In n.NovedadFuncionario
                dto.Funcionarios.Add(New FuncionarioReporteDTO With {
                    .Id = funcRel.Funcionario.Id,
                    .Nombre = funcRel.Funcionario.Nombre
                })
            Next

            ' Mapear fotos
            For Each fotoEntidad In n.NovedadFoto
                dto.Fotos.Add(New FotoReporteDTO With {
                    .Id = fotoEntidad.Id,
                    .Foto = fotoEntidad.Foto,
                    .FileName = fotoEntidad.FileName
                })
            Next
            resultado.Add(dto)
        Next

        Return resultado.OrderByDescending(Function(n) n.Fecha).ThenByDescending(Function(n) n.Id).ToList()
    End Function
End Class
