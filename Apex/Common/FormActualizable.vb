' FormActualizable.vb
Option Strict On
Option Explicit On

Imports System.Threading.Tasks
Imports System.Windows.Forms

' NOTA:
' - Esta clase NO es MustInherit para que el diseñador no falle.
' - Suscribe a dos canales:
'     * NotificadorEventos.FuncionarioActualizado (debounce propio)
'     * NotificadorEventos.NovedadActualizada    (debounce propio)
' - Cada form hijo puede sobreescribir:
'     * RefrescarSegunEventoAsync(e As FuncionarioCambiadoEventArgs)
'     * RefrescarSegunNovedadAsync(e As NovedadCambiadaEventArgs)

Public Class FormActualizable
    Inherits Form

    ' Suscripciones
    Private _suscripcionFunc As IDisposable
    Private _suscripcionNov As IDisposable

    ' Debounce para FUNCIONARIOS
    Private ReadOnly _debounceFunc As New Timer() With {.Interval = 250}
    Private _ultimoEventoFunc As FuncionarioCambiadoEventArgs
    Private ReadOnly _gateFunc As New Object

    ' Debounce para NOVEDADES
    Private ReadOnly _debounceNov As New Timer() With {.Interval = 250}
    Private _ultimoEventoNov As NovedadCambiadaEventArgs
    Private ReadOnly _gateNov As New Object

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        If Me.DesignMode Then Return

        ' Handlers de debounce
        AddHandler _debounceFunc.Tick, AddressOf DebounceFuncTick
        AddHandler _debounceNov.Tick, AddressOf DebounceNovTick

        ' Suscribir a ambos canales
        _suscripcionFunc = NotificadorEventos.SuscribirFuncionario(AddressOf OnFuncionarioEvent)
        _suscripcionNov = NotificadorEventos.SuscribirNovedad(AddressOf OnNovedadEvent)
    End Sub

    ' =======================
    ' Canal: FUNCIONARIOS
    ' =======================
    Private Sub OnFuncionarioEvent(sender As Object, e As FuncionarioCambiadoEventArgs)
        SyncLock _gateFunc
            _ultimoEventoFunc = e
            _debounceFunc.Stop()
            _debounceFunc.Start()
        End SyncLock
    End Sub

    Private Async Sub DebounceFuncTick(sender As Object, e As EventArgs)
        _debounceFunc.Stop()
        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Exit Sub

        Try
            ' Si querés máxima seguridad de hilo, podés invocar:
            If Me.InvokeRequired Then
                Me.BeginInvoke(CType(Async Sub()
                                         Try
                                             Await RefrescarSegunEventoAsync(_ultimoEventoFunc)
                                         Catch
                                             ' log opcional
                                         End Try
                                     End Sub, MethodInvoker))
            Else
                Await RefrescarSegunEventoAsync(_ultimoEventoFunc)
            End If
        Catch
            ' log opcional
        End Try
    End Sub

    ' Por defecto no hace nada: los formularios hijos lo sobreescriben
    Protected Overridable Function RefrescarSegunEventoAsync(e As FuncionarioCambiadoEventArgs) As Task
        Return Task.CompletedTask
    End Function

    ' =======================
    ' Canal: NOVEDADES
    ' =======================
    Private Sub OnNovedadEvent(sender As Object, e As NovedadCambiadaEventArgs)
        SyncLock _gateNov
            _ultimoEventoNov = e
            _debounceNov.Stop()
            _debounceNov.Start()
        End SyncLock
    End Sub

    Private Async Sub DebounceNovTick(sender As Object, e As EventArgs)
        _debounceNov.Stop()
        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Exit Sub

        Try
            If Me.InvokeRequired Then
                Me.BeginInvoke(CType(Async Sub()
                                         Try
                                             Await RefrescarSegunNovedadAsync(_ultimoEventoNov)
                                         Catch
                                             ' log opcional
                                         End Try
                                     End Sub, MethodInvoker))
            Else
                Await RefrescarSegunNovedadAsync(_ultimoEventoNov)
            End If
        Catch
            ' log opcional
        End Try
    End Sub

    ' Por defecto no hace nada: los formularios hijos lo sobreescriben
    Protected Overridable Function RefrescarSegunNovedadAsync(e As NovedadCambiadaEventArgs) As Task
        Return Task.CompletedTask
    End Function

    ' Limpieza
    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        Try
            _suscripcionFunc?.Dispose()
            _suscripcionNov?.Dispose()
        Finally
            Try
                RemoveHandler _debounceFunc.Tick, AddressOf DebounceFuncTick
                _debounceFunc.Dispose()
            Catch
            End Try

            Try
                RemoveHandler _debounceNov.Tick, AddressOf DebounceNovTick
                _debounceNov.Dispose()
            Catch
            End Try
        End Try

        MyBase.OnFormClosed(e)
    End Sub
End Class
