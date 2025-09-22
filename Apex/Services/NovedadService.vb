Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Data.SqlClient
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

    ' --- Helper interno: la auditoría nunca “rompe” la operación principal ---
    Private Async Function RegistrarAuditoriaSeguraAsync(accion As String,
                                                         tipoEntidad As String,
                                                         descripcion As String,
                                                         Optional entidadId As Integer? = Nothing) As Task
        Try
            Await _auditoriaService.RegistrarActividadAsync(accion, tipoEntidad, descripcion, entidadId)
        Catch
            ' Swallow (opcional: loguear a archivo/telemetría)
        End Try
    End Function

    ' ==================== CONSULTAS ====================

    ''' <summary>Obtiene las novedades AGRUPADAS para la grilla principal (con rango de fechas opcional).</summary>
    Public Async Function GetAllAgrupadasAsync(Optional fechaInicio As Date? = Nothing,
                                               Optional fechaFin As Date? = Nothing) As Task(Of List(Of vw_NovedadesAgrupadas))
        Dim query = _unitOfWork.Repository(Of vw_NovedadesAgrupadas)().GetAll().AsNoTracking()
        If fechaInicio.HasValue Then query = query.Where(Function(n) n.Fecha >= fechaInicio.Value)
        If fechaFin.HasValue Then query = query.Where(Function(n) n.Fecha <= fechaFin.Value)

        Dim lista = Await query.OrderByDescending(Function(n) n.Fecha).ThenBy(Function(n) n.Id).ToListAsync()

        Dim rango As String = $"rango: {If(fechaInicio.HasValue, fechaInicio.Value.ToString("yyyy-MM-dd"), "sin inicio")} - {If(fechaFin.HasValue, fechaFin.Value.ToString("yyyy-MM-dd"), "sin fin")}"
        Await RegistrarAuditoriaSeguraAsync("Listar", "Novedad", $"Se listaron {lista.Count} novedades agrupadas ({rango}).")

        Return lista
    End Function


    ' En NovedadService.vb

    Public Async Function BuscarNovedadesAvanzadoAsync(textoBusqueda As String,
                                                   fechaDesde As DateTime?,
                                                   fechaHasta As DateTime?,
                                                   idsFuncionarios As List(Of Integer)) As Task(Of List(Of vw_NovedadesAgrupadas))

        ' Caso sin filtros: últimas 200
        If String.IsNullOrWhiteSpace(textoBusqueda) AndAlso
       Not fechaDesde.HasValue AndAlso
       (idsFuncionarios Is Nothing OrElse Not idsFuncionarios.Any()) Then

            Dim listaSinFiltro = Await _unitOfWork.Repository(Of vw_NovedadesAgrupadas)().GetAll().
                                        OrderByDescending(Function(n) n.Fecha).
                                        ThenByDescending(Function(n) n.Id).
                                        Take(200).
                                        ToListAsync()

            Await RegistrarAuditoriaSeguraAsync("BuscarAvanzado", "Novedad",
                                            $"Búsqueda sin filtros. Se devuelven las últimas {listaSinFiltro.Count} novedades.")
            Return listaSinFiltro
        End If

        ' Consulta dinámica
        Dim sqlBuilder As New StringBuilder()
        Dim parameters As New List(Of SqlParameter)()
        Dim whereClauses As New List(Of String)()

        sqlBuilder.AppendLine("SELECT DISTINCT v.Id, v.Fecha, v.Resumen, v.Funcionarios, v.Estado")
        sqlBuilder.AppendLine("FROM dbo.vw_NovedadesAgrupadas AS v")

        ' Filtro de texto (FTS)
        If Not String.IsNullOrWhiteSpace(textoBusqueda) Then
            sqlBuilder.AppendLine("INNER JOIN dbo.Novedad AS n ON v.Id = n.Id") ' Para buscar en n.Texto

            Dim terminos = textoBusqueda.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).
                                     Select(Function(w) $"""{w.Trim()}*""")
            Dim expresionFts = String.Join(" AND ", terminos)
            parameters.Add(New SqlParameter("@patronFTS", expresionFts))

            Dim ftsCondition =
            " (CONTAINS(n.Texto, @patronFTS) OR " &
            "  EXISTS (SELECT 1 " &
            "          FROM dbo.NovedadFuncionario nf " &
            "          JOIN dbo.Funcionario f ON nf.FuncionarioId = f.Id " &
            "          WHERE nf.NovedadId = v.Id AND CONTAINS(f.Nombre, @patronFTS)))"
            whereClauses.Add(ftsCondition)
        End If

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Fechas (con validación para prevenir el desbordamiento de SqlDateTime)
        Dim minSqlDate As New Date(1753, 1, 1)

        If fechaDesde.HasValue AndAlso fechaDesde.Value >= minSqlDate Then
            whereClauses.Add("v.Fecha >= @fechaDesde")
            parameters.Add(New SqlParameter("@fechaDesde", fechaDesde.Value))
        End If
        If fechaHasta.HasValue AndAlso fechaHasta.Value >= minSqlDate Then
            whereClauses.Add("v.Fecha <= @fechaHasta")
            parameters.Add(New SqlParameter("@fechaHasta", fechaHasta.Value))
        End If
        ' --- FIN DE LA CORRECCIÓN ---

        ' Funcionarios
        If idsFuncionarios IsNot Nothing AndAlso idsFuncionarios.Any() Then
            Dim funcCondition = $"v.Id IN (SELECT nf.NovedadId FROM dbo.NovedadFuncionario nf WHERE nf.FuncionarioId IN ({String.Join(",", idsFuncionarios)}))"
            whereClauses.Add(funcCondition)
        End If

        If whereClauses.Any() Then
            sqlBuilder.AppendLine("WHERE " & String.Join(" AND ", whereClauses))
        End If

        sqlBuilder.AppendLine("ORDER BY v.Fecha DESC, v.Id DESC")

        Dim lista = Await _unitOfWork.Context.Database.SqlQuery(Of vw_NovedadesAgrupadas)(
        sqlBuilder.ToString(), parameters.ToArray()
    ).ToListAsync()

        Dim cantFuncs As Integer = If(idsFuncionarios Is Nothing, 0, idsFuncionarios.Count)
        Await RegistrarAuditoriaSeguraAsync("BuscarAvanzado", "Novedad",
        $"Búsqueda con filtros: Texto='{textoBusqueda}', FechaDesde='{fechaDesde}', FechaHasta='{fechaHasta}', Funcionarios='{cantFuncs}'. Resultados: {lista.Count}.")

        Return lista
    End Function

    ' ==================== ESCRITURAS / CRUD ====================

    ''' <summary>Crea la novedad y sus relaciones (usa/crea NovedadGenerada del día).</summary>
    Public Async Function CrearNovedadCompletaAsync(fecha As Date, texto As String, funcionarioIds As List(Of Integer)) As Task(Of Novedad)
        Using uow As New UnitOfWork()
            ' Usamos una transacción para garantizar que toda la operación sea atómica (o todo o nada)
            Using transaction = uow.Context.Database.BeginTransaction()
                Try
                    ' 1. Busca o crea el contenedor NovedadGenerada sin guardarlo todavía
                    Dim repoGenerada = uow.Repository(Of NovedadGenerada)()
                    Dim novedadDelDia = Await repoGenerada.GetByPredicateAsync(Function(ng) ng.Fecha = fecha.Date)

                    If novedadDelDia Is Nothing Then
                        novedadDelDia = New NovedadGenerada With {
                        .Fecha = fecha.Date,
                        .CreatedAt = DateTime.Now
                    }
                        repoGenerada.Add(novedadDelDia)
                    End If

                    ' 2. Crea la Novedad y la asocia con su contenedor y funcionarios
                    Dim nuevaNovedad = New Novedad With {
                    .Fecha = fecha,
                    .Texto = texto,
                    .EstadoId = 1, ' Pendiente
                    .CreatedAt = DateTime.Now,
                    .NovedadGenerada = novedadDelDia ' Asignación directa de la entidad
                }

                    If funcionarioIds IsNot Nothing Then
                        For Each funcId In funcionarioIds
                            nuevaNovedad.NovedadFuncionario.Add(New NovedadFuncionario With {.FuncionarioId = funcId})
                        Next
                    End If

                    uow.Repository(Of Novedad)().Add(nuevaNovedad)

                    ' 3. Guarda TODOS los cambios en una única y rápida transacción
                    Await uow.CommitAsync()

                    ' 4. Si todo salió bien, confirma la transacción
                    transaction.Commit()

                    ' El registro de auditoría va después de confirmar que todo se guardó
                    Await RegistrarAuditoriaSeguraAsync("Crear", "Novedad",
                    $"Se creó la novedad #{nuevaNovedad.Id} para la fecha {fecha:yyyy-MM-dd}.",
                    nuevaNovedad.Id)

                    Return nuevaNovedad
                Catch ex As Exception
                    ' Si algo falla, se revierte toda la operación para no dejar datos corruptos
                    transaction.Rollback()
                    Throw ' Relanzamos la excepción para que el usuario sea notificado del error
                End Try
            End Using
        End Using
    End Function

    ''' <summary>Elimina novedad + dependencias (funcionarios, fotos) de forma explícita/transaccional.</summary>
    Public Async Function DeleteNovedadCompletaAsync(novedadId As Integer) As Task
        Dim novedadAEliminar = Await _unitOfWork.Context.Set(Of Novedad)().
            Include(Function(n) n.NovedadFuncionario).
            Include(Function(n) n.NovedadFoto).
            SingleOrDefaultAsync(Function(n) n.Id = novedadId)

        If novedadAEliminar Is Nothing Then Return

        Dim cantFuncs As Integer = If(novedadAEliminar.NovedadFuncionario IsNot Nothing, novedadAEliminar.NovedadFuncionario.Count, 0)
        Dim cantFotos As Integer = If(novedadAEliminar.NovedadFoto IsNot Nothing, novedadAEliminar.NovedadFoto.Count, 0)
        Dim idEliminado As Integer = novedadAEliminar.Id

        Dim repoFuncionarios = _unitOfWork.Repository(Of NovedadFuncionario)()
        Dim repoFotos = _unitOfWork.Repository(Of NovedadFoto)()

        If cantFuncs > 0 Then repoFuncionarios.RemoveRange(novedadAEliminar.NovedadFuncionario.ToList())
        If cantFotos > 0 Then repoFotos.RemoveRange(novedadAEliminar.NovedadFoto.ToList())

        _unitOfWork.Repository(Of Novedad)().Remove(novedadAEliminar)
        Await _unitOfWork.CommitAsync()

        Await RegistrarAuditoriaSeguraAsync("Eliminar", "Novedad",
            $"Se eliminó la novedad #{idEliminado} con {cantFuncs} funcionario(s) y {cantFotos} foto(s).",
            idEliminado)
    End Function

    ''' <summary>Detalle completo por IDs (para reportes, etc.).</summary>
    Public Async Function GetNovedadesCompletasByIds(novedadIds As List(Of Integer)) As Task(Of List(Of vw_NovedadesCompletas))
        Using uow As New UnitOfWork()
            Dim repo = uow.Repository(Of vw_NovedadesCompletas)()
            Dim lista = Await repo.GetAll().Where(Function(n) novedadIds.Contains(n.Id)).ToListAsync()

            Await RegistrarAuditoriaSeguraAsync("ConsultarDetalle", "Novedad",
                $"Se consultaron detalles de {lista.Count} novedad(es). IDs: {String.Join(",", novedadIds)}.")

            Return lista
        End Using
    End Function

    ''' <summary>Actualiza novedad + sincroniza funcionarios.</summary>
    Public Async Function ActualizarNovedadCompletaAsync(novedadActualizada As Novedad,
                                                         nuevosFuncionarioIds As List(Of Integer)) As Task
        Dim novedadEnDb = Await _unitOfWork.Context.Set(Of Novedad)().
            Include(Function(n) n.NovedadFuncionario).
            SingleOrDefaultAsync(Function(n) n.Id = novedadActualizada.Id)

        If novedadEnDb Is Nothing Then
            Throw New KeyNotFoundException("La novedad que intenta actualizar ya no existe en la base de datos.")
        End If

        novedadEnDb.Fecha = novedadActualizada.Fecha
        novedadEnDb.Texto = novedadActualizada.Texto

        Dim idsActuales = novedadEnDb.NovedadFuncionario.Select(Function(nf) nf.FuncionarioId).ToList()
        Dim idsNuevos = If(nuevosFuncionarioIds, New List(Of Integer)())

        Dim idsParaQuitar = idsActuales.Except(idsNuevos).ToList()
        Dim idsParaAgregar = idsNuevos.Except(idsActuales).ToList()

        If idsParaQuitar.Any() Then
            Dim relacionesAQuitar = novedadEnDb.NovedadFuncionario.Where(Function(nf) idsParaQuitar.Contains(nf.FuncionarioId)).ToList()
            _unitOfWork.Repository(Of NovedadFuncionario)().RemoveRange(relacionesAQuitar)
        End If

        For Each funcId In idsParaAgregar
            novedadEnDb.NovedadFuncionario.Add(New NovedadFuncionario With {.FuncionarioId = funcId})
        Next

        Await _unitOfWork.CommitAsync()

        Await RegistrarAuditoriaSeguraAsync("Actualizar", "Novedad",
            $"Se actualizó la novedad #{novedadEnDb.Id}. Funcionarios: {idsNuevos.Count}.",
            novedadEnDb.Id)
    End Function

    ''' <summary>Funcionarios asociados a una novedad.</summary>
    Public Async Function GetFuncionariosPorNovedadAsync(novedadId As Integer) As Task(Of List(Of Funcionario))
        Dim lista = Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking().
            Where(Function(f) f.NovedadFuncionario.Any(Function(nf) nf.NovedadId = novedadId)).
            OrderBy(Function(f) f.Nombre).
            ToListAsync()

        Await RegistrarAuditoriaSeguraAsync("ConsultarFuncionariosDeNovedad", "Novedad",
            $"Se consultaron {lista.Count} funcionario(s) para la novedad #{novedadId}.", novedadId)

        Return lista
    End Function

    ''' <summary>Fotos de una novedad.</summary>
    Public Async Function GetFotosPorNovedadAsync(novedadId As Integer) As Task(Of List(Of NovedadFoto))
        Dim lista = Await _unitOfWork.Repository(Of NovedadFoto)().GetAll().AsNoTracking().
            Where(Function(nf) nf.NovedadId = novedadId).
            ToListAsync()

        Await RegistrarAuditoriaSeguraAsync("ConsultarFotosDeNovedad", "Novedad",
            $"Se consultaron {lista.Count} foto(s) para la novedad #{novedadId}.", novedadId)

        Return lista
    End Function

    ''' <summary>Agregar foto a novedad.</summary>
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

        Await RegistrarAuditoriaSeguraAsync("AgregarFoto", "Novedad",
            $"Se agregó la foto '{Path.GetFileName(rutaArchivo)}' a la novedad #{novedadId}.", novedadId)
    End Function

    ''' <summary>Eliminar foto por Id.</summary>
    Public Async Function DeleteFotoAsync(fotoId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFoto)()
        Dim foto = Await repo.GetByIdAsync(fotoId)
        If foto Is Nothing Then Return

        Dim novedadIdDeLaFoto As Integer = foto.NovedadId
        repo.Remove(foto)
        Await _unitOfWork.CommitAsync()

        Await RegistrarAuditoriaSeguraAsync("EliminarFoto", "Novedad",
            $"Se eliminó la foto #{fotoId} de la novedad #{novedadIdDeLaFoto}.", novedadIdDeLaFoto)
    End Function

    ''' <summary>Agregar funcionario a novedad existente.</summary>
    Public Async Function AgregarFuncionarioANovedadAsync(novedadId As Integer, funcionarioId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFuncionario)()
        Dim existe = Await repo.AnyAsync(Function(nf) nf.NovedadId = novedadId AndAlso nf.FuncionarioId = funcionarioId)
        If Not existe Then
            repo.Add(New NovedadFuncionario With {.NovedadId = novedadId, .FuncionarioId = funcionarioId})
            Await _unitOfWork.CommitAsync()
            Await RegistrarAuditoriaSeguraAsync("AgregarFuncionario", "Novedad",
                $"Se agregó el funcionario #{funcionarioId} a la novedad #{novedadId}.", novedadId)
        End If
    End Function

    ''' <summary>Quitar funcionario de novedad.</summary>
    Public Async Function QuitarFuncionarioDeNovedadAsync(novedadId As Integer, funcionarioId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFuncionario)()
        Dim relacion = Await repo.GetByPredicateAsync(Function(nf) nf.NovedadId = novedadId AndAlso nf.FuncionarioId = funcionarioId)
        If relacion IsNot Nothing Then
            repo.Remove(relacion)
            Await _unitOfWork.CommitAsync()
            Await RegistrarAuditoriaSeguraAsync("QuitarFuncionario", "Novedad",
                $"Se quitó el funcionario #{funcionarioId} de la novedad #{novedadId}.", novedadId)
        End If
    End Function

    ' ==================== PARA REPORTES ====================

    ''' <summary>DTOs jerárquicos para el RDLC (novedad + funcionarios + fotos).</summary>
    Public Async Function GetNovedadesParaReporteAsync(novedadIds As List(Of Integer)) As Task(Of List(Of NovedadReporteDTO))
        If novedadIds Is Nothing OrElse Not novedadIds.Any() Then
            Return New List(Of NovedadReporteDTO)()
        End If

        Dim novedadesConTodo = Await _unitOfWork.Repository(Of Novedad)().GetAll().
            Include(Function(n) n.NovedadFuncionario.Select(Function(nf) nf.Funcionario)).
            Include(Function(n) n.NovedadFoto).
            Where(Function(n) novedadIds.Contains(n.Id)).
            ToListAsync()

        Dim resultado As New List(Of NovedadReporteDTO)()

        For Each n In novedadesConTodo
            Dim dto = New NovedadReporteDTO With {.Id = n.Id, .Fecha = n.Fecha, .Texto = n.Texto}

            For Each funcRel In n.NovedadFuncionario
                dto.Funcionarios.Add(New FuncionarioReporteDTO With {.Id = funcRel.Funcionario.Id, .Nombre = funcRel.Funcionario.Nombre})
            Next

            For Each fotoEntidad In n.NovedadFoto
                dto.Fotos.Add(New FotoReporteDTO With {.Id = fotoEntidad.Id, .Foto = fotoEntidad.Foto, .FileName = fotoEntidad.FileName})
            Next

            resultado.Add(dto)
        Next

        Return resultado.OrderByDescending(Function(n) n.Fecha).ThenByDescending(Function(n) n.Id).ToList()
    End Function
End Class
