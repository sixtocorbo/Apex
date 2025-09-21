' FormActualizable.vb
Option Strict On
Option Explicit On
Imports System.ComponentModel
Public MustInherit Class FormActualizable
    Inherits Form

#Region " Suscripciones y Handlers "

    ' Suscripciones a los eventos globales
    Private _suscripcionFunc As IDisposable
    Private _suscripcionNov As IDisposable
    Private _suscripcionEstado As IDisposable

    ' Instancias de la clase auxiliar que manejan toda la lógica de debounce
    Private _handlerFunc As EventDebounceHandler(Of FuncionarioCambiadoEventArgs)
    Private _handlerNov As EventDebounceHandler(Of NovedadCambiadaEventArgs)
    Private _handlerEstado As EventDebounceHandler(Of EstadoCambiadoEventArgs)

#End Region

#Region " Carga y Cierre del Formulario "

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        If Me.DesignMode Then Return

        ' 1. Creamos los manejadores, pasándoles el método que deben ejecutar
        _handlerFunc = New EventDebounceHandler(Of FuncionarioCambiadoEventArgs)(Me, AddressOf RefrescarSegunFuncionarioAsync)
        _handlerNov = New EventDebounceHandler(Of NovedadCambiadaEventArgs)(Me, AddressOf RefrescarSegunNovedadAsync)
        _handlerEstado = New EventDebounceHandler(Of EstadoCambiadoEventArgs)(Me, AddressOf RefrescarSegunEstadoAsync)

        ' 2. Nos suscribimos a los eventos y redirigimos todo a nuestros manejadores
        _suscripcionFunc = NotificadorEventos.SuscribirFuncionario(Sub(s, ev) _handlerFunc.HandleEvent(ev))
        _suscripcionNov = NotificadorEventos.SuscribirNovedad(Sub(s, ev) _handlerNov.HandleEvent(ev))
        _suscripcionEstado = NotificadorEventos.SuscribirEstado(Sub(s, ev) _handlerEstado.HandleEvent(ev))
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        ' Limpiamos todo al cerrar
        _suscripcionFunc?.Dispose()
        _suscripcionNov?.Dispose()
        _suscripcionEstado?.Dispose()

        _handlerFunc?.Dispose()
        _handlerNov?.Dispose()
        _handlerEstado?.Dispose()

        MyBase.OnFormClosed(e)
    End Sub

#End Region

#Region " Métodos Overridable para los Formularios Hijos "

    ' Estas funciones no cambian. Siguen siendo los "puntos de entrada"
    ' para que los formularios hijos implementen su lógica de refresco.

    Protected Overridable Function RefrescarSegunFuncionarioAsync(e As FuncionarioCambiadoEventArgs) As Task
        Return Task.CompletedTask
    End Function

    Protected Overridable Function RefrescarSegunNovedadAsync(e As NovedadCambiadaEventArgs) As Task
        Return Task.CompletedTask
    End Function

    Protected Overridable Function RefrescarSegunEstadoAsync(e As EstadoCambiadoEventArgs) As Task
        Return Task.CompletedTask
    End Function

#End Region

End Class