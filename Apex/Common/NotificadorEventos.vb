' /Common/NotificadorEventos.vb

Public Class NotificadorEventos

    ' Declaramos un evento estático (Shared). Cualquier parte de la aplicación puede "escucharlo".
    Public Shared Event DatosActualizados(sender As Object, e As EventArgs)

    ''' <summary>
    ''' Llama a este método desde cualquier lugar donde se guarden datos
    ''' que puedan afectar a otros formularios (ej: después de guardar un funcionario).
    ''' </summary>
    Public Shared Sub NotificarActualizacion()
        RaiseEvent DatosActualizados(Nothing, EventArgs.Empty)
    End Sub

End Class