' /Common/FormBaseActualizable.vb

Imports System.ComponentModel

Public MustInherit Class FormBaseActualizable
    Inherits Form
    Implements IFormularioActualizable

    ' Propiedad para decidir si el formulario debe reaccionar a los cambios.
    ' Útil para formularios modales o de selección que no deben actualizarse en segundo plano.
    <DefaultValue(True)>
    Public Property RespondeANotificaciones As Boolean = True

    ' 1. El método que las clases hijas DEBEN implementar.
    Public MustOverride Sub ActualizarDatos() Implements IFormularioActualizable.ActualizarDatos

    ' 2. Suscripción automática al cargar el formulario.
    Private Sub FormBaseActualizable_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If RespondeANotificaciones AndAlso Not DesignMode Then
            AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
        End If
    End Sub

    ' 3. Desuscripción automática al cerrar.
    Private Sub FormBaseActualizable_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        If RespondeANotificaciones AndAlso Not DesignMode Then
            RemoveHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
        End If
    End Sub

    ' 4. El manejador de eventos es genérico y ahora vive en la clase base.
    ' Llama al método abstracto que la clase hija implementa.
    Private Sub OnDatosActualizados(sender As Object, e As EventArgs)
        ' Nos aseguramos de que el formulario todavía exista antes de intentar actualizar.
        If Me.IsHandleCreated AndAlso Not Me.IsDisposed Then
            ' No necesitamos invocar en el hilo de UI porque los eventos de WinForms ya se ejecutan allí.
            ActualizarDatos()
        End If
    End Sub

End Class