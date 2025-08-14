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

    ' --- INICIO DE LA CORRECCIÓN ---
    ' Se modifica para aceptar fechas opcionales (Nullable)
    Public Async Function GetAllConDetallesAsync(Optional fechaInicio As Date? = Nothing, Optional fechaFin As Date? = Nothing) As Task(Of List(Of vw_NovedadesCompletas))
        Dim query = _unitOfWork.Repository(Of vw_NovedadesCompletas)().GetAll().AsNoTracking()

        If fechaInicio.HasValue Then
            query = query.Where(Function(n) n.Fecha >= fechaInicio.Value)
        End If
        If fechaFin.HasValue Then
            query = query.Where(Function(n) n.Fecha <= fechaFin.Value)
        End If

        Return Await query.OrderByDescending(Function(n) n.Fecha).ThenBy(Function(n) n.Id).ToListAsync()
    End Function
    ' --- FIN DE LA CORRECCIÓN ---

    Public Async Function GetOrCreateNovedadGeneradaAsync(fecha As Date) As Task(Of NovedadGenerada)
        Dim repo = _unitOfWork.Repository(Of NovedadGenerada)()
        Dim novedadGenerada = Await repo.GetByPredicateAsync(Function(ng) ng.Fecha = fecha)

        If novedadGenerada Is Nothing Then
            novedadGenerada = New NovedadGenerada With {
                .Fecha = fecha,
                .CreatedAt = DateTime.Now
            }
            repo.Add(novedadGenerada)
            Await _unitOfWork.CommitAsync()
        End If

        Return novedadGenerada
    End Function

    ' ... (El resto de los métodos del servicio permanecen igual)
    ' --- INICIO DE LA CORRECCIÓN ---
    Public Async Function CrearNovedadCompletaAsync(novedadGeneradaId As Integer, fecha As Date, texto As String, funcionarioIds As List(Of Integer)) As Task
        ' 1. Crear la entidad Novedad principal
        Dim nuevaNovedad = New Novedad With {
            .NovedadGeneradaId = novedadGeneradaId,
            .Fecha = fecha,
            .Texto = texto,
            .EstadoId = 1, ' Pendiente
            .CreatedAt = DateTime.Now
        }

        ' 2. Crear y añadir las entidades de asociación (NovedadFuncionario) a la propiedad de navegación de la Novedad.
        '    Entity Framework se encargará de crear los registros y asignar las claves foráneas correctamente.
        For Each funcId In funcionarioIds
            nuevaNovedad.NovedadFuncionario.Add(New NovedadFuncionario With {
                .FuncionarioId = funcId
            })
        Next

        ' 3. Añadir el objeto principal (que ahora contiene a sus "hijos") al repositorio.
        _unitOfWork.Repository(Of Novedad)().Add(nuevaNovedad)

        ' 4. Guardar todos los cambios (la Novedad y todas las asociaciones NovedadFuncionario) en una única transacción.
        Await _unitOfWork.CommitAsync()
    End Function

    ' --- FIN DE LA CORRECCIÓN ---

    Public Async Function GetFuncionariosPorNovedadAsync(novedadId As Integer) As Task(Of List(Of Funcionario))
        Return Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking().
            Where(Function(f) f.NovedadFuncionario.Any(Function(nf) nf.NovedadId = novedadId)).
            OrderBy(Function(f) f.Nombre).
            ToListAsync()
    End Function

    Public Async Function GetFotosPorNovedadGeneradaAsync(novedadGeneradaId As Integer) As Task(Of List(Of NovedadFoto))
        Return Await _unitOfWork.Repository(Of NovedadFoto)().GetAll().AsNoTracking().
            Where(Function(nf) nf.NovedadGeneradaId = novedadGeneradaId).
            ToListAsync()
    End Function

    Public Async Function AddFotoAsync(novedadGeneradaId As Integer, rutaArchivo As String) As Task
        Dim fotoBytes = File.ReadAllBytes(rutaArchivo)
        Dim nuevaFoto = New NovedadFoto With {
            .NovedadGeneradaId = novedadGeneradaId,
            .Foto = fotoBytes,
            .FileName = Path.GetFileName(rutaArchivo),
            .CreatedAt = DateTime.Now
        }
        _unitOfWork.Repository(Of NovedadFoto)().Add(nuevaFoto)
        Await _unitOfWork.CommitAsync()
    End Function

    Public Async Function DeleteFotoAsync(fotoId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFoto)()
        Dim foto = Await repo.GetByIdAsync(fotoId)
        If foto IsNot Nothing Then
            repo.Remove(foto)
            Await _unitOfWork.CommitAsync()
        End If
    End Function
End Class