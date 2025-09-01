' /Common/NotificadorEventos.vb

''' <summary>
''' Clase auxiliar para pasar el ID del funcionario en el evento específico.
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
''' Soporta tanto notificaciones genéricas como específicas por entidad.
''' </summary>
Public Class NotificadorEventos

    ' --- 1. Evento Genérico (EL QUE YA USAS) ---
    ' Este se mantiene sin cambios para no romper el código existente.
    Public Shared Event DatosActualizados(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Notifica de forma genérica que "algo" ha cambiado en la aplicación.
    ''' </summary>
    Public Shared Sub NotificarActualizacion()
        RaiseEvent DatosActualizados(Nothing, EventArgs.Empty)
    End Sub


    ' --- 2. Nuevo Evento Específico para Funcionarios ---
    ' Este es el nuevo evento que usará el formulario de situación del funcionario.
    Public Shared Event FuncionarioActualizado As EventHandler(Of FuncionarioEventArgs)

    ''' <summary>
    ''' Notifica que un funcionario específico ha sido modificado.
    ''' </summary>
    ''' <param name="funcionarioId">El ID del funcionario que se guardó o modificó.</param>
    Public Shared Sub NotificarActualizacionFuncionario(funcionarioId As Integer)
        ' Dispara el evento específico, pasando el ID del funcionario.
        RaiseEvent FuncionarioActualizado(Nothing, New FuncionarioEventArgs(funcionarioId))

        ' También dispara el evento genérico para mantener la compatibilidad con
        ' los formularios antiguos que solo escuchan la notificación general.
        NotificarActualizacion()
    End Sub

End Class