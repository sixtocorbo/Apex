' Apex/Services/NotificacionPersonalService.vb
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
    ''' Obtiene una lista de notificaciones desde la vista para la grilla principal.
    ''' (VERSIÓN REFACTORIZADA)
    ''' </summary>
    Public Async Function GetAllConDetallesAsync(Optional filtroNombreFuncionario As String = "") As Task(Of List(Of vw_NotificacionesCompletas))
        ' Apuntamos directamente a la nueva vista
        Dim query = _unitOfWork.Repository(Of vw_NotificacionesCompletas)().GetAll().AsNoTracking()

        If Not String.IsNullOrWhiteSpace(filtroNombreFuncionario) Then
            query = query.Where(Function(n) n.NombreFuncionario.Contains(filtroNombreFuncionario) Or n.CI.Contains(filtroNombreFuncionario))
        End If

        ' Ya no se necesita el .Select() para transformar los datos
        Return Await query.OrderByDescending(Function(n) n.FechaProgramada).ToListAsync()
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

    ' --- MÉTODOS PARA POBLAR COMBOS (se mantienen igual) ---
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
    Public Async Function ObtenerEstadosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Byte, String)))
        Dim repo = _unitOfWork.Repository(Of NotificacionEstado)()
        Dim lista = Await repo.GetAll().AsNoTracking().OrderBy(Function(e) e.Orden).ToListAsync()
        Return lista.Select(Function(e) New KeyValuePair(Of Byte, String)(e.Id, e.Nombre)).ToList()
    End Function

    ' La clase DTO 'NotificacionParaVista' ya no es necesaria y ha sido eliminada.

End Class