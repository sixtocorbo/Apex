' NotificadorEventos.vb
Option Strict On
Option Explicit On

' ============================
' EventArgs tipados
' ============================

' Cambio en funcionario (puntual o global)
Public Class FuncionarioCambiadoEventArgs
    Inherits EventArgs
    Public ReadOnly Property FuncionarioId As Integer?

    Public Sub New(Optional funcionarioId As Integer? = Nothing)
        Me.FuncionarioId = funcionarioId
    End Sub
End Class

' Cambio en novedad (puntual o global)
Public Class NovedadCambiadaEventArgs
    Inherits EventArgs
    Public ReadOnly Property NovedadId As Integer?

    Public Sub New(Optional novedadId As Integer? = Nothing)
        Me.NovedadId = novedadId
    End Sub
End Class

' ============================
' Notificador
' ============================
Public Module NotificadorEventos

    ' --------- CANAL: Funcionarios ---------
    Public Event FuncionarioActualizado As EventHandler(Of FuncionarioCambiadoEventArgs)

    ' Notifica cambio puntual de un funcionario
    Public Sub NotificarCambiosEnFuncionario(id As Integer)
        RaiseEvent FuncionarioActualizado(Nothing, New FuncionarioCambiadoEventArgs(id))
    End Sub

    ' Notifica refresco general de funcionarios
    Public Sub NotificarRefrescoTotal()
        RaiseEvent FuncionarioActualizado(Nothing, New FuncionarioCambiadoEventArgs())
    End Sub

    ' Suscripción (devuelve IDisposable para desuscribir fácilmente)
    Public Function SuscribirFuncionario(handler As EventHandler(Of FuncionarioCambiadoEventArgs)) As IDisposable
        AddHandler FuncionarioActualizado, handler
        Return New Subscription(Sub() RemoveHandler FuncionarioActualizado, handler)
    End Function

    ' --------- CANAL: Novedades (NUEVO) ---------
    Public Event NovedadActualizada As EventHandler(Of NovedadCambiadaEventArgs)

    ' Notifica cambio puntual de una novedad
    Public Sub NotificarCambioEnNovedad(Optional novedadId As Integer? = Nothing)
        RaiseEvent NovedadActualizada(Nothing, New NovedadCambiadaEventArgs(novedadId))
    End Sub

    ' Suscripción para novedades
    Public Function SuscribirNovedad(handler As EventHandler(Of NovedadCambiadaEventArgs)) As IDisposable
        AddHandler NovedadActualizada, handler
        Return New Subscription(Sub() RemoveHandler NovedadActualizada, handler)
    End Function

    ' ============================
    ' Utilidad interna
    ' ============================
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
