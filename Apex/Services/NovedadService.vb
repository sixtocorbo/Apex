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
    ''' </summary>
    Public Async Function CrearNovedadCompletaAsync(fecha As Date, texto As String, funcionarioIds As List(Of Integer)) As Task(Of Novedad)
        Dim nuevaNovedad = New Novedad With {
            .Fecha = fecha,
            .Texto = texto,
            .EstadoId = 1, ' Por defecto: "Pendiente"
            .CreatedAt = DateTime.Now
        }

        For Each funcId In funcionarioIds
            nuevaNovedad.NovedadFuncionario.Add(New NovedadFuncionario With {.FuncionarioId = funcId})
        Next

        _unitOfWork.Repository(Of Novedad)().Add(nuevaNovedad)
        Await _unitOfWork.CommitAsync()
        Return nuevaNovedad
    End Function

    Public Async Function ActualizarNovedadCompletaAsync(novedadActualizada As Novedad,
                                                     nuevosFuncionarioIds As List(Of Integer)) As Task
        If nuevosFuncionarioIds Is Nothing Then nuevosFuncionarioIds = New List(Of Integer)()

        ' 1) Traer tracked (NO uses GetAll)
        Dim ctx = _unitOfWork.Context
        Dim novedadEnDb = Await ctx.Set(Of Novedad)().
        Include(Function(n) n.NovedadFuncionario).
        SingleOrDefaultAsync(Function(n) n.Id = novedadActualizada.Id)

        If novedadEnDb Is Nothing Then Throw New Exception("La novedad que intenta actualizar ya no existe.")

        ' 2) Actualizar escalares (si quieres, asigna a mano)
        ctx.Entry(novedadEnDb).CurrentValues.SetValues(New With {
        .Fecha = novedadActualizada.Fecha,
        .Texto = novedadActualizada.Texto,
        .EstadoId = novedadActualizada.EstadoId
    })

        ' 3) Sincronizar funcionarios
        Dim actuales = novedadEnDb.NovedadFuncionario.Select(Function(nf) nf.FuncionarioId).ToList()
        Dim quitar = actuales.Except(nuevosFuncionarioIds).ToList()
        Dim agregar = nuevosFuncionarioIds.Except(actuales).ToList()

        If quitar.Any() Then
            Dim aQuitar = novedadEnDb.NovedadFuncionario.Where(Function(nf) quitar.Contains(nf.FuncionarioId)).ToList()
            For Each nf In aQuitar
                ctx.Set(Of NovedadFuncionario)().Remove(nf)
            Next
        End If

        For Each id In agregar
            ctx.Set(Of NovedadFuncionario)().Add(New NovedadFuncionario With {.NovedadId = novedadEnDb.Id, .FuncionarioId = id})
        Next

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
    ''' Agrega una foto a una novedad existente.
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