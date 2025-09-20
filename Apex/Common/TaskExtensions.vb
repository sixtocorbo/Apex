' Archivo: Apex/Common/TaskExtensions.vb

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Threading.Tasks

Public Module TaskExtensions

    <Extension>
    Public Async Function WaitAsync(Of T)(_task As Task(Of T), ct As CancellationToken) As Task(Of T)
        If _task Is Nothing Then Throw New ArgumentNullException(NameOf(_task))
        If _task.IsCompleted Then
            Return Await _task ' ya completada, no crear overhead
        End If

        Dim tcs As New TaskCompletionSource(Of Boolean)(TaskCreationOptions.RunContinuationsAsynchronously)
        Using ctr = ct.Register(Sub() tcs.TrySetResult(True))
            ' 👇 CORRECCIÓN AQUÍ 👇
            If _task Is Await Task.WhenAny(_task, tcs.Task) Then
                Return Await _task
            Else
                Throw New OperationCanceledException(ct)
            End If
        End Using
    End Function

    <Extension>
    Public Async Function WaitAsync(task As Task, ct As CancellationToken) As Task
        If task Is Nothing Then Throw New ArgumentNullException(NameOf(task))
        If task.IsCompleted Then
            Await task
            Return
        End If

        Dim tcs As New TaskCompletionSource(Of Boolean)(TaskCreationOptions.RunContinuationsAsynchronously)
        Using ctr = ct.Register(Sub() tcs.TrySetResult(True))
            ' 👇 Y CORRECCIÓN AQUÍ 👇
            If task Is Await Task.WhenAny(task, tcs.Task) Then
                Await task
            Else
                Throw New OperationCanceledException(ct)
            End If
        End Using
    End Function

End Module