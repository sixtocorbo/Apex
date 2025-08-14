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

    Public Async Function GetAllConDetallesAsync(fecha As Date) As Task(Of List(Of vw_NovedadesCompletas))
        Dim query = _unitOfWork.Repository(Of vw_NovedadesCompletas)().GetAll().AsNoTracking()
        query = query.Where(Function(n) n.Fecha = fecha)
        Return Await query.OrderBy(Function(n) n.Id).ToListAsync()
    End Function

    ''' <summary>
    ''' Busca o crea el contenedor de novedades para una fecha específica.
    ''' </summary>
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

    ''' <summary>
    ''' Obtiene los funcionarios asociados a una novedad específica.
    ''' </summary>
    Public Async Function GetFuncionariosPorNovedadAsync(novedadId As Integer) As Task(Of List(Of Funcionario))
        Return Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking().
            Where(Function(f) f.NovedadFuncionario.Any(Function(nf) nf.NovedadId = novedadId)).
            OrderBy(Function(f) f.Nombre).
            ToListAsync()
    End Function

    ''' <summary>
    ''' Obtiene las fotos asociadas a un día de novedades.
    ''' </summary>
    Public Async Function GetFotosPorNovedadGeneradaAsync(novedadGeneradaId As Integer) As Task(Of List(Of NovedadFoto))
        Return Await _unitOfWork.Repository(Of NovedadFoto)().GetAll().AsNoTracking().
            Where(Function(nf) nf.NovedadGeneradaId = novedadGeneradaId).
            ToListAsync()
    End Function

    ''' <summary>
    ''' Añade una foto a una NovedadGenerada.
    ''' </summary>
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

    ''' <summary>
    ''' Elimina una foto por su ID.
    ''' </summary>
    Public Async Function DeleteFotoAsync(fotoId As Integer) As Task
        Dim repo = _unitOfWork.Repository(Of NovedadFoto)()
        Dim foto = Await repo.GetByIdAsync(fotoId)
        If foto IsNot Nothing Then
            repo.Remove(foto)
            Await _unitOfWork.CommitAsync()
        End If
    End Function

End Class