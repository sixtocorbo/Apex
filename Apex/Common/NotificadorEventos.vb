''' <summary>
''' Clase auxiliar para pasar el ID del funcionario en eventos específicos.
''' </summary>
Public Class FuncionarioEventArgs
    Inherits EventArgs
    Public ReadOnly Property FuncionarioId As Integer
    Public Sub New(id As Integer)
        FuncionarioId = id
    End Sub
End Class

''' <summary>
''' Gestiona la comunicación de eventos entre diferentes partes de la aplicación.
''' </summary>
Public Class NotificadorEventos

    ' --- Evento Genérico ---
    ' Para formularios que necesitan saber que "algo" cambió, pero no qué.
    Public Shared Event DatosActualizados As EventHandler

    ' --- Evento Específico ---
    ' Para formularios que necesitan saber qué funcionario específico cambió.
    Public Shared Event FuncionarioActualizado As EventHandler(Of FuncionarioEventArgs)

    ''' <summary>
    ''' Notifica de forma genérica que "algo" ha cambiado en la aplicación.
    ''' Usado por formularios como el de Gestión de Licencias.
    ''' </summary>
    Public Shared Sub NotificarActualizacionGeneral()
        RaiseEvent DatosActualizados(Nothing, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' Notifica que un funcionario específico ha sido modificado o sus datos relacionados han cambiado.
    ''' </summary>
    ''' <param name="funcionarioId">El ID del funcionario afectado.</param>
    Public Shared Sub NotificarCambiosEnFuncionario(funcionarioId As Integer)
        ' Dispara el evento específico para formularios como frmFuncionarioBuscar y frmFuncionarioSituacion
        RaiseEvent FuncionarioActualizado(Nothing, New FuncionarioEventArgs(funcionarioId))

        ' También dispara el evento genérico para mantener la compatibilidad con otros formularios.
        NotificarActualizacionGeneral()
    End Sub

End Class