' Apex/Services/NovedadService.vb
Imports System.Data.Entity
Imports System.IO

Public Class NovedadService
    Inherits GenericService(Of Novedad) ' Hereda la implementación IDisposable

    ' El constructor Shadows es correcto para que cada servicio tenga su propio UnitOfWork
    Private Shadows ReadOnly _unitOfWork As IUnitOfWork

    Public Sub New()
        MyBase.New(New UnitOfWork())
        _unitOfWork = MyBase._unitOfWork
    End Sub

    ' ... (Los demás métodos como GetAllConDetallesAsync, GetOrCreateNovedadGeneradaAsync, etc., permanecen igual)

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

    Public Async Function GetOrCreateNovedadGeneradaAsync(fecha As Date) As Task(Of NovedadGenerada)
        Dim repo = _unitOfWork.Repository(Of NovedadGenerada)()
        Dim novedadGenerada = Await repo.GetByPredicateAsync(Function(ng) ng.Fecha = fecha)
        If novedadGenerada Is Nothing Then
            novedadGenerada = New NovedadGenerada With {.Fecha = fecha, .CreatedAt = DateTime.Now}
            repo.Add(novedadGenerada)
            Await _unitOfWork.CommitAsync()
        End If
        Return novedadGenerada
    End Function

    Public Async Function CrearNovedadCompletaAsync(novedadGeneradaId As Integer, fecha As Date, texto As String, funcionarioIds As List(Of Integer)) As Task
        Dim nuevaNovedad = New Novedad With {
            .NovedadGeneradaId = novedadGeneradaId,
            .Fecha = fecha,
            .Texto = texto,
            .EstadoId = 1, ' Pendiente
            .CreatedAt = DateTime.Now
        }
        For Each funcId In funcionarioIds
            nuevaNovedad.NovedadFuncionario.Add(New NovedadFuncionario With {.FuncionarioId = funcId})
        Next
        _unitOfWork.Repository(Of Novedad)().Add(nuevaNovedad)
        Await _unitOfWork.CommitAsync()
    End Function

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

    ' --- INICIO DE LA CORRECCIÓN: Métodos añadidos ---

    ''' <summary>
    ''' Asocia un funcionario a una novedad existente.
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
    ''' Desvincula un funcionario de una novedad existente.
    ''' </summary>
    Public Async Function QuitarFuncionarioDeNovedadAsync(novedadId As Integer, funcionarioId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFuncionario)()
        Dim relacion = Await repo.GetByPredicateAsync(Function(nf) nf.NovedadId = novedadId AndAlso nf.FuncionarioId = funcionarioId)
        If relacion IsNot Nothing Then
            repo.Remove(relacion)
            Await _unitOfWork.CommitAsync()
        End If
    End Function

    ' --- FIN DE LA CORRECCIÓN ---

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