' FormActualizable.vb
Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.ComponentModel.Design

Public Class FormActualizable
    Inherits Form

    ' --------- Utilidad robusta para detectar el diseñador ----------
    Protected ReadOnly Property IsDesigner As Boolean
        Get
            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then Return True
            If Site IsNot Nothing AndAlso Site.DesignMode Then Return True
            ' Fallback por si acaso:
            Return String.Equals(
                Diagnostics.Process.GetCurrentProcess().ProcessName,
                "devenv",
                StringComparison.OrdinalIgnoreCase)
        End Get
    End Property
    ' ----------------------------------------------------------------

#Region " Suscripciones y Handlers "
    Private _suscripcionFunc As IDisposable
    Private _suscripcionNov As IDisposable
    Private _suscripcionEstado As IDisposable

    Private _handlerFunc As EventDebounceHandler(Of FuncionarioCambiadoEventArgs)
    Private _handlerNov As EventDebounceHandler(Of NovedadCambiadaEventArgs)
    Private _handlerEstado As EventDebounceHandler(Of EstadoCambiadoEventArgs)
#End Region

#Region " Carga y Cierre del Formulario "

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        If IsDesigner Then Return ' <- clave para el diseñador

        _handlerFunc = New EventDebounceHandler(Of FuncionarioCambiadoEventArgs)(Me, AddressOf RefrescarSegunFuncionarioAsync)
        _handlerNov = New EventDebounceHandler(Of NovedadCambiadaEventArgs)(Me, AddressOf RefrescarSegunNovedadAsync)
        _handlerEstado = New EventDebounceHandler(Of EstadoCambiadoEventArgs)(Me, AddressOf RefrescarSegunEstadoAsync)

        _suscripcionFunc = NotificadorEventos.SuscribirFuncionario(Sub(s, ev) _handlerFunc.HandleEvent(ev))
        _suscripcionNov = NotificadorEventos.SuscribirNovedad(Sub(s, ev) _handlerNov.HandleEvent(ev))
        _suscripcionEstado = NotificadorEventos.SuscribirEstado(Sub(s, ev) _handlerEstado.HandleEvent(ev))
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        _suscripcionFunc?.Dispose()
        _suscripcionNov?.Dispose()
        _suscripcionEstado?.Dispose()

        _handlerFunc?.Dispose()
        _handlerNov?.Dispose()
        _handlerEstado?.Dispose()

        MyBase.OnFormClosed(e)
    End Sub

#End Region

#Region " Puntos de extensión para los hijos "

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
