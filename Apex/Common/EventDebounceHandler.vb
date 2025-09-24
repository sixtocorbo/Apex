' EventDebounceHandler.vb
Option Strict On
Option Explicit On

Imports System.Threading.Tasks

''' <summary>
''' Clase genérica que encapsula la lógica de debounce para cualquier tipo de evento.
''' Recibe un evento, espera un intervalo corto para evitar ráfagas y luego
''' ejecuta una acción de forma segura en el hilo de la UI del formulario.
''' </summary>
Public Class EventDebounceHandler(Of TEventArgs As Class)
    Implements IDisposable

    Private ReadOnly _debounceTimer As New Timer()
    Private ReadOnly _gate As New Object
    Private _lastArgs As TEventArgs
    Private ReadOnly _form As Form
    Private ReadOnly _refreshAction As Func(Of TEventArgs, Task)

    Public Sub New(form As Form, refreshAction As Func(Of TEventArgs, Task), Optional interval As Integer = 250)
        _form = form
        _refreshAction = refreshAction
        _debounceTimer.Interval = interval
        AddHandler _debounceTimer.Tick, AddressOf Debounce_Tick
    End Sub

    ''' <summary>
    ''' Método público para recibir el evento entrante.
    ''' </summary>
    Public Sub HandleEvent(e As TEventArgs)
        SyncLock _gate
            _lastArgs = e
            _debounceTimer.Stop()
            _debounceTimer.Start()
        End SyncLock
    End Sub

    Private Async Sub Debounce_Tick(sender As Object, e As EventArgs)
        _debounceTimer.Stop()
        If _form Is Nothing OrElse Not _form.IsHandleCreated OrElse _form.IsDisposed Then Return

        Try
            If _form.InvokeRequired Then
                _form.BeginInvoke(CType(Sub()
                                            Dim _ = EjecutarRefrescoSeguroAsync(_lastArgs)
                                        End Sub, MethodInvoker))
            Else
                Await EjecutarRefrescoSeguroAsync(_lastArgs)
            End If
        Catch
            ' Opcional: Registrar el error si la acción falla.
        End Try
    End Sub

    Private Async Function EjecutarRefrescoSeguroAsync(args As TEventArgs) As Task
        Try
            Await _refreshAction(args)
        Catch ex As OperationCanceledException
            ' Cancelaciones esperadas: no hacer nada.
        Catch
            ' Opcional: Registrar el error si la acción falla.
        End Try
    End Function

#Region "IDisposable"
    Private _disposedValue As Boolean
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not _disposedValue Then
            If disposing Then
                RemoveHandler _debounceTimer.Tick, AddressOf Debounce_Tick
                _debounceTimer.Dispose()
            End If
            _disposedValue = True
        End If
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
