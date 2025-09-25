' Archivo: Apex/Common/TaskExtensions.vb

Imports System.Runtime.CompilerServices
Imports System.Threading
Imports System.Threading.Tasks

Public Module TaskExtensions

    <Extension>
    Public Async Function WaitAsync(Of T)(_task As Task(Of T), ct As CancellationToken) As Task(Of T)
        If _task Is Nothing Then Throw New ArgumentNullException(NameOf(_task))
        If _task.IsCompleted Then Return Await _task.ConfigureAwait(False)

        If ct.IsCancellationRequested Then
            Return Await Task.FromCanceled(Of T)(ct).ConfigureAwait(False)
        End If

        Dim cancellationTask = Task.Delay(Timeout.Infinite, ct)
        Dim completed = Await Task.WhenAny(_task, cancellationTask).ConfigureAwait(False)

        If completed Is _task Then
            Return Await _task.ConfigureAwait(False)
        End If

        Return Await Task.FromCanceled(Of T)(ct).ConfigureAwait(False)
    End Function

    <Extension>
    Public Async Function WaitAsync(task As Task, ct As CancellationToken) As Task
        If task Is Nothing Then Throw New ArgumentNullException(NameOf(task))
        If task.IsCompleted Then
            Await task.ConfigureAwait(False)
            Return
        End If

        If ct.IsCancellationRequested Then
            Await Task.FromCanceled(ct).ConfigureAwait(False)
            Return
        End If

        Dim cancellationTask = Task.Delay(Timeout.Infinite, ct)
        Dim completed = Await Task.WhenAny(task, cancellationTask).ConfigureAwait(False)

        If completed Is task Then
            Await task.ConfigureAwait(False)
            Return
        End If

        Await Task.FromCanceled(ct).ConfigureAwait(False)
    End Function

End Module
