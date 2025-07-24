Imports System.Data.Entity

Public Class NotificacionPersonalService
    Inherits GenericService(Of NotificacionPersonal)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ''' <summary>
    ''' Obtiene una notificación por su ID para edición, incluyendo las entidades relacionadas.
    ''' </summary>
    Public Async Function GetByIdParaEdicionAsync(id As Integer) As Task(Of NotificacionPersonal)
        Return Await _unitOfWork.Repository(Of NotificacionPersonal)().
            GetAll().
            Include(Function(n) n.Funcionario).
            Include(Function(n) n.TipoNotificacion).
            FirstOrDefaultAsync(Function(n) n.Id = id)
    End Function

    ''' <summary>
    ''' Obtiene una lista de notificaciones para la grilla principal.
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(Optional filtroNombreFuncionario As String = "") As Task(Of List(Of NotificacionParaVista))
        Dim query = _unitOfWork.Repository(Of NotificacionPersonal)().
            GetAll().
            Include(Function(n) n.Funcionario).
            Include(Function(n) n.TipoNotificacion).
            Include(Function(n) n.NotificacionEstado).
            AsNoTracking()

        If Not String.IsNullOrWhiteSpace(filtroNombreFuncionario) Then
            query = query.Where(Function(n) n.Funcionario.Nombre.Contains(filtroNombreFuncionario))
        End If

        Return Await query.Select(Function(n) New NotificacionParaVista With {
            .Id = n.Id,
            .NombreFuncionario = n.Funcionario.Nombre,
            .TipoNotificacion = n.TipoNotificacion.Nombre,
            .FechaProgramada = n.FechaProgramada,
            .Estado = n.NotificacionEstado.Nombre,
            .Texto = n.Medio
        }).OrderByDescending(Function(n) n.FechaProgramada).ToListAsync()
    End Function

    ''' <summary>
    ''' Actualiza el estado de una notificación.
    ''' </summary>
    Public Async Function UpdateEstadoAsync(notificacionId As Integer, nuevoEstadoId As Byte) As Task
        Dim notificacion = Await _unitOfWork.Repository(Of NotificacionPersonal)().GetByIdAsync(notificacionId)
        If notificacion IsNot Nothing Then
            notificacion.EstadoId = nuevoEstadoId
            notificacion.UpdatedAt = DateTime.Now
            _unitOfWork.Repository(Of NotificacionPersonal)().Update(notificacion)
            Await _unitOfWork.CommitAsync()
        End If
    End Function

    ' --- MÉTODOS PARA POBLAR COMBOS ---
    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim repo = _unitOfWork.Repository(Of Funcionario)()
        Dim lista = Await repo.GetAll().AsNoTracking().Where(Function(f) f.Activo).OrderBy(Function(f) f.Nombre).ToListAsync()
        Return lista.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
    End Function

    Public Async Function ObtenerTiposNotificacionParaComboAsync() As Task(Of List(Of KeyValuePair(Of Byte, String)))
        Dim repo = _unitOfWork.Repository(Of TipoNotificacion)()
        Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(t) t.Orden).ToListAsync()
        Return lista.Select(Function(t) New KeyValuePair(Of Byte, String)(t.Id, t.Nombre)).ToList()
    End Function

    Public Class NotificacionParaVista
        Public Property Id As Integer
        Public Property NombreFuncionario As String
        Public Property TipoNotificacion As String
        Public Property FechaProgramada As Date
        Public Property Estado As String
        Public Property Texto As String
    End Class

End Class