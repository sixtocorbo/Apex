Imports System.Threading
Imports System.Threading.Tasks

''' <summary>
''' Ejecuta la acción solo si el usuario deja de invocarla durante el
''' intervalo indicado.  Cancelación segura y retorno al hilo de UI.
''' </summary>
Public Class DebounceDispatcherAsync

    Private _cts As CancellationTokenSource
    Private ReadOnly _uiCtx As SynchronizationContext = SynchronizationContext.Current

    ''' <param name="delayMs">Milisegundos de “silencio” antes de lanzar la acción.</param>
    ''' <param name="action">Func(Of CancellationToken, Task) que contiene la lógica a ejecutar.</param>
    Public Sub Debounce(delayMs As Integer,
                    action As Func(Of CancellationToken, Task))

        ' Cancela la ejecución pendiente
        _cts?.Cancel()
        _cts?.Dispose()

        _cts = New CancellationTokenSource()
        Dim token = _cts.Token

        Task.Run(Async Function()
                     Try
                         ' 1) Creamos un TaskCompletionSource que completamos al cancelar el token
                         Dim canceledTcs = New TaskCompletionSource(Of Boolean)()
                         Using registration = token.Register(Sub() canceledTcs.TrySetResult(True))
                             ' 2) Esperamos EITHER al delay OR a que se dispare la cancelación
                             Dim finished = Await Task.WhenAny(
                             Task.Delay(delayMs),   ' el delay sin token
                             canceledTcs.Task       ' se completa al cancelar
                         ).ConfigureAwait(False)

                             ' 3) Si fue cancelado, salimos sin excepción
                             If token.IsCancellationRequested Then
                                 Return
                             End If
                         End Using

                         ' 4) Ejecutamos la acción en el hilo UI
                         _uiCtx.Post(
                         Async Sub(state As Object)
                             If Not token.IsCancellationRequested Then
                                 Await action(token).ConfigureAwait(False)
                             End If
                         End Sub,
                         Nothing)

                     Catch ex As Exception
                         ' Aquí ya no verás TaskCanceledException por el delay
                         ' Podés manejar otras excepciones si hicieran falta
                     End Try
                 End Function)
    End Sub


    ''' <summary>Cancela la ejecución pendiente (si la hay).</summary>
    Public Sub Cancel()
        _cts?.Cancel()
        _cts?.Dispose()
        _cts = Nothing
    End Sub

End Class
