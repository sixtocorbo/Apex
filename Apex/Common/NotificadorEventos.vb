' NotificadorEventos.vb
Option Strict On
Option Explicit On

' Evento tipado (permite saber si el cambio es global o de un funcionario puntual)
Public Class FuncionarioCambiadoEventArgs
    Inherits EventArgs
    Public ReadOnly Property FuncionarioId As Integer?

    Public Sub New(Optional funcionarioId As Integer? = Nothing)
        Me.FuncionarioId = funcionarioId
    End Sub
End Class

Public Module NotificadorEventos

    ' === Evento nuevo y tipado (recomendado) ===
    Public Event FuncionarioActualizado As EventHandler(Of FuncionarioCambiadoEventArgs)

    ' Notifica cambio puntual: incluye el Id del funcionario afectado.
    ' Dispara también el evento legado para mantener compatibilidad.
    Public Sub NotificarCambiosEnFuncionario(id As Integer)
        RaiseEvent FuncionarioActualizado(Nothing, New FuncionarioCambiadoEventArgs(id))
    End Sub

    ' Notifica un refresco general (sin Id específico).
    ' También dispara el evento legado para compatibilidad.
    Public Sub NotificarRefrescoTotal()
        RaiseEvent FuncionarioActualizado(Nothing, New FuncionarioCambiadoEventArgs())
    End Sub

    ' Suscripción conveniente al evento tipado, devolviendo IDisposable
    ' para asegurar la desuscripción (ej.: en FormClosed).
    Public Function Suscribir(handler As EventHandler(Of FuncionarioCambiadoEventArgs)) As IDisposable
        AddHandler FuncionarioActualizado, handler
        Return New Subscription(Sub() RemoveHandler FuncionarioActualizado, handler)
    End Function

    ' Implementación mínima de IDisposable para suscripciones
    Private NotInheritable Class Subscription
        Implements IDisposable

        Private ReadOnly _dispose As Action
        Public Sub New(dispose As Action)
            _dispose = dispose
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            _dispose?.Invoke()
        End Sub
    End Class

End Module
