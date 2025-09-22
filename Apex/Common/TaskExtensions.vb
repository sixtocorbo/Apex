' Archivo: Apex/Common/TaskExtensions.vb

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Threading.Tasks

Public Module TaskExtensions

    <Extension>
    Public Async Function WaitAsync(Of T)(_task As Task(Of T), ct As CancellationToken) As Task(Of T)
        If _task Is Nothing Then Throw New ArgumentNullException(NameOf(_task))
        If _task.IsCompleted Then Return Await _task

        ' Short-circuit si ya viene cancelado
        ct.ThrowIfCancellationRequested()

        Dim tcs As New TaskCompletionSource(Of Boolean)(TaskCreationOptions.RunContinuationsAsynchronously)
        Using ctr = ct.Register(Sub() tcs.TrySetResult(True))
            If _task Is Await Task.WhenAny(_task, tcs.Task).ConfigureAwait(False) Then
                Return Await _task.ConfigureAwait(False)
            Else
                Throw New OperationCanceledException(ct)
            End If
        End Using
    End Function

    <Extension>
    Public Async Function WaitAsync(task As Task, ct As CancellationToken) As Task
        If task Is Nothing Then Throw New ArgumentNullException(NameOf(task))
        If task.IsCompleted Then
            Await task.ConfigureAwait(False)
            Return
        End If

        ct.ThrowIfCancellationRequested()

        Dim tcs As New TaskCompletionSource(Of Boolean)(TaskCreationOptions.RunContinuationsAsynchronously)
        Using ctr = ct.Register(Sub() tcs.TrySetResult(True))
            If task Is Await Task.WhenAny(task, tcs.Task).ConfigureAwait(False) Then
                Await task.ConfigureAwait(False)
            Else
                Throw New OperationCanceledException(ct)
            End If
        End Using
    End Function

End Module