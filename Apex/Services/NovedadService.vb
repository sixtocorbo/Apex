' Apex/Services/NovedadService.vb
Imports System.Data.Entity
Imports System.IO

Public Class NovedadService
    Inherits GenericService(Of Novedad)

    Private Shadows ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        MyBase.New(New UnitOfWork())
        _unitOfWork = MyBase._unitOfWork
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
        _unitOfWork = unitOfWork
    End Sub

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
        Return Await query.OrderByDescending(Function(n) n.Fecha).ThenBy(Function(n) n.Id).ToListAsync()
    End Function


    ''' <summary>
    ''' Crea una nueva novedad junto con sus funcionarios asociados en una única transacción.
    ''' Primero asegura la existencia de una NovedadGenerada para la fecha dada.
    ''' </summary>
    Public Async Function CrearNovedadCompletaAsync(fecha As Date, texto As String, funcionarioIds As List(Of Integer)) As Task(Of Novedad)
        ' --- INICIO DE LA CORRECCIÓN CLAVE ---
        ' 1. Buscar o crear el contenedor NovedadGenerada para la fecha.
        Dim repoGenerada = _unitOfWork.Repository(Of NovedadGenerada)()
        Dim novedadDelDia = Await repoGenerada.GetByPredicateAsync(Function(ng) ng.Fecha = fecha.Date)

        If novedadDelDia Is Nothing Then
            ' Si no existe para esta fecha, la creamos.
            novedadDelDia = New NovedadGenerada With {
                .Fecha = fecha.Date,
                .CreatedAt = DateTime.Now
            }
            repoGenerada.Add(novedadDelDia)
            Await _unitOfWork.CommitAsync() ' Guardamos para obtener su ID.
        End If
        ' --- FIN DE LA CORRECCIÓN CLAVE ---

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
        Return nuevaNovedad
    End Function


    ''' <summary>
    ''' Elimina una novedad y todas sus relaciones (funcionarios, fotos) de forma explícita y transaccional.
    ''' </summary>
    Public Async Function DeleteNovedadCompletaAsync(novedadId As Integer) As Task
        ' Obtenemos la novedad incluyendo sus relaciones para poder eliminarlas.
        Dim novedadAEliminar = Await _unitOfWork.Context.Set(Of Novedad)().
            Include(Function(n) n.NovedadFuncionario).
            Include(Function(n) n.NovedadFoto).
            SingleOrDefaultAsync(Function(n) n.Id = novedadId)

        If novedadAEliminar IsNot Nothing Then
            ' --- INICIO DE LA CORRECCIÓN CLAVE ---
            ' 1. Obtener los repositorios para las tablas hijas.
            Dim repoFuncionarios = _unitOfWork.Repository(Of NovedadFuncionario)()
            Dim repoFotos = _unitOfWork.Repository(Of NovedadFoto)()

            ' 2. Eliminar explícitamente todos los registros de las tablas hijas.
            '    ToList() es importante para evitar errores de "colección modificada".
            repoFuncionarios.RemoveRange(novedadAEliminar.NovedadFuncionario.ToList())
            repoFotos.RemoveRange(novedadAEliminar.NovedadFoto.ToList())

            ' 3. Ahora que no tiene dependencias, eliminamos la novedad principal.
            _unitOfWork.Repository(Of Novedad)().Remove(novedadAEliminar)

            ' 4. Guardamos todos los cambios (las 3 eliminaciones) en una sola operación.
            Await _unitOfWork.CommitAsync()
            ' --- FIN DE LA CORRECCIÓN ---
        End If
    End Function
    ''' <summary>
    ''' Obtiene la información completa de una lista de novedades a partir de sus IDs.
    ''' </summary>
    ''' <param name="novedadIds">Lista de IDs de las novedades a buscar.</param>
    ''' <returns></returns>
    Public Async Function GetNovedadesCompletasByIds(novedadIds As List(Of Integer)) As Task(Of List(Of vw_NovedadesCompletas))
        Using uow As New UnitOfWork()
            Dim repo = uow.Repository(Of vw_NovedadesCompletas)()
            Return Await repo.GetAll().Where(Function(n) novedadIds.Contains(n.Id)).ToListAsync()
        End Using
    End Function
    ''' <summary>
    ''' Actualiza una novedad existente, sincronizando la lista de funcionarios. (Versión Definitiva)
    ''' </summary>
    Public Async Function ActualizarNovedadCompletaAsync(novedadActualizada As Novedad, nuevosFuncionarioIds As List(Of Integer)) As Task
        ' 1. Obtener la Novedad original desde la base de datos CON SEGUIMIENTO.
        '    Esto es crucial para que Entity Framework sepa qué entidades vigilar.
        Dim novedadEnDb = Await _unitOfWork.Context.Set(Of Novedad)().
            Include(Function(n) n.NovedadFuncionario).
            SingleOrDefaultAsync(Function(n) n.Id = novedadActualizada.Id)

        If novedadEnDb Is Nothing Then
            Throw New KeyNotFoundException("La novedad que intenta actualizar ya no existe en la base de datos.")
        End If

        ' 2. Actualizar las propiedades escalares (campos simples) de forma manual.
        novedadEnDb.Fecha = novedadActualizada.Fecha
        novedadEnDb.Texto = novedadActualizada.Texto
        ' Si tienes otros campos para editar, como el EstadoId, añádelos aquí.
        ' novedadEnDb.EstadoId = novedadActualizada.EstadoId 

        ' 3. Sincronizar la lista de funcionarios.
        Dim idsActuales = novedadEnDb.NovedadFuncionario.Select(Function(nf) nf.FuncionarioId).ToList()
        Dim idsParaQuitar = idsActuales.Except(nuevosFuncionarioIds).ToList()
        Dim idsParaAgregar = nuevosFuncionarioIds.Except(idsActuales).ToList()

        ' --- INICIO DE LA CORRECCIÓN CLAVE ---
        ' Quitar explícitamente los registros de la tabla de unión.
        If idsParaQuitar.Any() Then
            Dim relacionesAQuitar = novedadEnDb.NovedadFuncionario.Where(Function(nf) idsParaQuitar.Contains(nf.FuncionarioId)).ToList()

            ' Le decimos al repositorio que elimine este conjunto de registros.
            ' Esto se traduce en un comando DELETE en SQL, que es lo que queremos.
            _unitOfWork.Repository(Of NovedadFuncionario)().RemoveRange(relacionesAQuitar)
        End If
        ' --- FIN DE LA CORRECCIÓN CLAVE ---

        ' Agregar los nuevos.
        For Each funcId In idsParaAgregar
            novedadEnDb.NovedadFuncionario.Add(New NovedadFuncionario With {
                .FuncionarioId = funcId
            })
        Next

        ' 4. Guardar todos los cambios (actualizaciones, eliminaciones e inserciones) en una única transacción.
        Await _unitOfWork.CommitAsync()
    End Function




    ''' <summary>
    ''' Obtiene la lista de funcionarios asociados a una novedad específica.
    ''' </summary>
    Public Async Function GetFuncionariosPorNovedadAsync(novedadId As Integer) As Task(Of List(Of Funcionario))
        Return Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking().
            Where(Function(f) f.NovedadFuncionario.Any(Function(nf) nf.NovedadId = novedadId)).
            OrderBy(Function(f) f.Nombre).
            ToListAsync()
    End Function

    ''' <summary>
    ''' Obtiene las fotos asociadas a una novedad específica.
    ''' </summary>
    Public Async Function GetFotosPorNovedadAsync(novedadId As Integer) As Task(Of List(Of NovedadFoto))
        Return Await _unitOfWork.Repository(Of NovedadFoto)().GetAll().AsNoTracking().
            Where(Function(nf) nf.NovedadId = novedadId).
            ToListAsync()
    End Function

    ''' <summary>
    ''' Agrega una foto a una novedad existente. (Versión Simplificada)
    ''' </summary>
    Public Async Function AddFotoAsync(novedadId As Integer, rutaArchivo As String) As Task
        Dim fotoBytes = File.ReadAllBytes(rutaArchivo)

        ' Ahora solo necesitas el NovedadId, que es la relación correcta.
        Dim nuevaFoto = New NovedadFoto With {
            .NovedadId = novedadId,
            .Foto = fotoBytes,
            .FileName = Path.GetFileName(rutaArchivo),
            .CreatedAt = DateTime.Now
        }

        _unitOfWork.Repository(Of NovedadFoto)().Add(nuevaFoto)
        Await _unitOfWork.CommitAsync()
    End Function

    ''' <summary>
    ''' Elimina una foto específica por su ID.
    ''' </summary>
    Public Async Function DeleteFotoAsync(fotoId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFoto)()
        Dim foto = Await repo.GetByIdAsync(fotoId)
        If foto IsNot Nothing Then
            repo.Remove(foto)
            Await _unitOfWork.CommitAsync()
        End If
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
        End If
    End Function

End Class