' FormActualizable.vb
Imports System.Windows.Forms
Imports System.Threading.Tasks

' --- CAMBIO 1: Se quita "MustInherit" ---
Public Class FormActualizable
    Inherits Form

    Private _suscripcion As IDisposable
    Private ReadOnly _debounce As New Timer() With {.Interval = 250}
    Private _ultimoEvento As FuncionarioCambiadoEventArgs
    Private ReadOnly _gate As New Object

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        ' Solo ejecutar en modo de ejecución, no en el diseñador
        If Not Me.DesignMode Then
            AddHandler _debounce.Tick, AddressOf DebounceTick
            _suscripcion = NotificadorEventos.Suscribir(AddressOf OnEvent)
        End If
    End Sub

    Private Sub OnEvent(sender As Object, e As FuncionarioCambiadoEventArgs)
        SyncLock _gate
            _ultimoEvento = e
            _debounce.Stop()
            _debounce.Start()
        End SyncLock
    End Sub

    Private Async Sub DebounceTick(sender As Object, e As EventArgs)
        _debounce.Stop()
        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Exit Sub
        Try
            Await RefrescarSegunEventoAsync(_ultimoEvento)
        Catch
            ' log opcional
        End Try
    End Sub

    ' --- CAMBIO 2: Se reemplaza "MustOverride" por "Overridable" y se agrega una implementación base ---
    ' Cada form define cómo refrescarse ante el evento
    Protected Overridable Function RefrescarSegunEventoAsync(e As FuncionarioCambiadoEventArgs) As Task
        ' Implementación base vacía para que el diseñador no falle.
        ' Las clases que hereden de esta deben sobreescribir este método.
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        _suscripcion?.Dispose()
        _debounce?.Dispose()
        MyBase.OnFormClosed(e)
    End Sub
End Class