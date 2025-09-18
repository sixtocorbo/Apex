' NotifierShim.vb
Imports System.Windows.Forms

' Mantiene compatibilidad con todas las llamadas antiguas Notifier.*
' Delegando al nuevo Toast (o a tu ToastNotifier dentro de un namespace).
' Si tu Toast está en Apex.UI, podés importar ese namespace o usar el nombre calificado abajo.
' OJO: Si ya existía *otro* Notifier público en el proyecto, renómbralo (o cambiá a Friend)
' para evitar "es ambiguo en el espacio de nombres".

Public Module Notifier

    ' ---- API clásica ----
    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Info(f As Form, msg As String, Optional ms As Integer = 3000)
        Toast.Show(owner:=f, message:=msg, type:=ToastType.Info, durationMs:=ms)
        ' Si tenés el toast en otro namespace: Apex.UI.Toast.Show(...).
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Success(f As Form, msg As String, Optional ms As Integer = 3000)
        Toast.Show(owner:=f, message:=msg, type:=ToastType.Success, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Warn(f As Form, msg As String, Optional ms As Integer = 3500)
        Toast.Show(owner:=f, message:=msg, type:=ToastType.Warning, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub [Error](f As Form, msg As String, Optional ms As Integer = 4000)
        Toast.Show(owner:=f, message:=msg, type:=ToastType.Error, durationMs:=ms)
    End Sub

    ' ---- Sobrecargas sin owner, por si en algún lado llamabas Notifier.Info("texto") ----
    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Info(msg As String, Optional ms As Integer = 3000)
        Toast.Show(owner:=Nothing, message:=msg, type:=ToastType.Info, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Success(msg As String, Optional ms As Integer = 3000)
        Toast.Show(owner:=Nothing, message:=msg, type:=ToastType.Success, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Warn(msg As String, Optional ms As Integer = 3500)
        Toast.Show(owner:=Nothing, message:=msg, type:=ToastType.Warning, durationMs:=ms)
    End Sub

    <System.Diagnostics.DebuggerStepThrough>
    Public Sub [Error](msg As String, Optional ms As Integer = 4000)
        Toast.Show(owner:=Nothing, message:=msg, type:=ToastType.Error, durationMs:=ms)
    End Sub

    ' ---- Progreso "nuevo" manteniendo el nombre Notifier para no cambiar llamadas futuras ----
    ' Devuelve el toast persistente para que el llamador pueda Update/Close.
    <System.Diagnostics.DebuggerStepThrough>
    Public Function ProgressStart(f As Form, initialMessage As String) As Toast
        Return Toast.ShowSticky(f, initialMessage, ToastType.Info)
    End Function

End Module

