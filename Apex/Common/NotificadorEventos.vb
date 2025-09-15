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

    ' === Evento legado (compatible con código existente) ===
    Public Event DatosActualizados As EventHandler

    ' === Evento nuevo y tipado (recomendado) ===
    Public Event FuncionarioActualizado As EventHandler(Of FuncionarioCambiadoEventArgs)

    ' Notifica cambio puntual: incluye el Id del funcionario afectado.
    ' Dispara también el evento legado para mantener compatibilidad.
    Public Sub NotificarCambiosEnFuncionario(id As Integer)
        RaiseEvent FuncionarioActualizado(Nothing, New FuncionarioCambiadoEventArgs(id))
        RaiseEvent DatosActualizados(Nothing, EventArgs.Empty)
    End Sub

    ' Notifica un refresco general (sin Id específico).
    ' También dispara el evento legado para compatibilidad.
    Public Sub NotificarRefrescoTotal()
        RaiseEvent FuncionarioActualizado(Nothing, New FuncionarioCambiadoEventArgs())
        RaiseEvent DatosActualizados(Nothing, EventArgs.Empty)
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

'''' <summary>
'''' Clase auxiliar para pasar el ID del funcionario en eventos específicos.
'''' </summary>
'Public Class FuncionarioEventArgs
'    Inherits EventArgs
'    Public ReadOnly Property FuncionarioId As Integer
'    Public Sub New(id As Integer)
'        FuncionarioId = id
'    End Sub
'End Class

'''' <summary>
'''' Gestiona la comunicación de eventos entre diferentes partes de la aplicación.
'''' </summary>
'Public Class NotificadorEventos

'    ' --- Evento Genérico ---
'    ' Para formularios que necesitan saber que "algo" cambió, pero no qué.
'    Public Shared Event DatosActualizados As EventHandler

'    ' --- Evento Específico ---
'    ' Para formularios que necesitan saber qué funcionario específico cambió.
'    Public Shared Event FuncionarioActualizado As EventHandler(Of FuncionarioEventArgs)

'    ''' <summary>
'    ''' Notifica de forma genérica que "algo" ha cambiado en la aplicación.
'    ''' Usado por formularios como el de Gestión de Licencias.
'    ''' </summary>
'    Public Shared Sub NotificarActualizacionGeneral()
'        RaiseEvent DatosActualizados(Nothing, EventArgs.Empty)
'    End Sub

'    ''' <summary>
'    ''' Notifica que un funcionario específico ha sido modificado o sus datos relacionados han cambiado.
'    ''' </summary>
'    ''' <param name="funcionarioId">El ID del funcionario afectado.</param>
'    Public Shared Sub NotificarCambiosEnFuncionario(funcionarioId As Integer)
'        ' Dispara el evento específico para formularios como frmFuncionarioBuscar y frmFuncionarioSituacion
'        RaiseEvent FuncionarioActualizado(Nothing, New FuncionarioEventArgs(funcionarioId))

'        ' También dispara el evento genérico para mantener la compatibilidad con otros formularios.
'        NotificarActualizacionGeneral()
'    End Sub

'End Class