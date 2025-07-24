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
    ''' Obtiene una lista de notificaciones para mostrar en la grilla principal,
    ''' incluyendo detalles del funcionario, tipo, estado y el texto de la notificación.
    ''' </summary>
    ''' <param name="filtroNombreFuncionario">Texto para filtrar por nombre de funcionario.</param>
    Public Async Function GetAllConDetallesAsync(Optional filtroNombreFuncionario As String = "") As Task(Of List(Of NotificacionParaVista))
        Dim query = _unitOfWork.Repository(Of NotificacionPersonal)().
            GetAll().
            Include(Function(n) n.Funcionario).
            Include(Function(n) n.TipoNotificacion).
            Include(Function(n) n.NotificacionEstado).
            AsNoTracking()

        ' Aplicar filtro si se proporcionó
        If Not String.IsNullOrWhiteSpace(filtroNombreFuncionario) Then
            query = query.Where(Function(n) n.Funcionario.Nombre.Contains(filtroNombreFuncionario))
        End If

        ' Proyectar a un DTO para la vista
        Return Await query.Select(Function(n) New NotificacionParaVista With {
            .Id = n.Id,
            .NombreFuncionario = n.Funcionario.Nombre,
            .TipoNotificacion = n.TipoNotificacion.Nombre,
            .FechaProgramada = n.FechaProgramada,
            .Estado = n.NotificacionEstado.Nombre,
            .Texto = n.Medio ' Se obtiene el texto/medio de la notificación
        }).OrderByDescending(Function(n) n.FechaProgramada).ToListAsync()
    End Function

    ''' <summary>
    ''' Actualiza únicamente el estado de una notificación específica.
    ''' </summary>
    ''' <param name="notificacionId">ID de la notificación a actualizar.</param>
    ''' <param name="nuevoEstadoId">ID del nuevo estado (de la tabla NotificacionEstado).</param>
    Public Async Function UpdateEstadoAsync(notificacionId As Integer, nuevoEstadoId As Byte) As Task
        Dim notificacion = Await _unitOfWork.Repository(Of NotificacionPersonal)().GetByIdAsync(notificacionId)
        If notificacion IsNot Nothing Then
            notificacion.EstadoId = nuevoEstadoId
            notificacion.UpdatedAt = DateTime.Now
            _unitOfWork.Repository(Of NotificacionPersonal)().Update(notificacion)
            Await _unitOfWork.CommitAsync()
        End If
    End Function

    ''' <summary>
    ''' DTO (Data Transfer Object) para simplificar los datos mostrados en la grilla.
    ''' </summary>
    Public Class NotificacionParaVista
        Public Property Id As Integer
        Public Property NombreFuncionario As String
        Public Property TipoNotificacion As String
        Public Property FechaProgramada As Date
        Public Property Estado As String
        Public Property Texto As String ' Propiedad para el texto de la notificación
    End Class

End Class