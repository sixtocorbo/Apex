Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Enum ToastType
    Info
    Success
    Warning
    [Error]
End Enum

Public NotInheritable Class Toast
    Inherits Form

    Private Const DefaultDurationMs As Integer = 3000
    Private Const CornerRadius As Integer = 12
    Private Const PaddingPx As Integer = 12
    Private Const MaxWidthPx As Integer = 420
    Private Const MarginFromEdges As Integer = 20
    Private Const StackGap As Integer = 8
    Private Const FadeIntervalMs As Integer = 15
    Private Const FadeStep As Double = 0.08

    Private Shared ReadOnly OpenToasts As New List(Of Toast)

    Private ReadOnly _lifeTimer As New Timer() With {.Interval = DefaultDurationMs}
    Private ReadOnly _fadeTimer As New Timer() With {.Interval = FadeIntervalMs}
    Private _fadingIn As Boolean = True
    Private _targetScreen As Screen

    Private ReadOnly _lbl As New Label() With {
        .AutoSize = False,
        .MaximumSize = New Size(MaxWidthPx, 0),
        .Font = New Font("Segoe UI", 9.5!, FontStyle.Regular, GraphicsUnit.Point),
        .ForeColor = Color.White
    }

    <DllImport("gdi32.dll", SetLastError:=True)>
    Private Shared Function CreateRoundRectRgn(leftRect As Integer, topRect As Integer, rightRect As Integer, bottomRect As Integer, widthEllipse As Integer, heightEllipse As Integer) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Private Shared Function SetWindowRgn(hWnd As IntPtr, hRgn As IntPtr, bRedraw As Boolean) As Integer
    End Function

    Private Sub New(texto As String, tipo As ToastType, screen As Screen)
        Me.FormBorderStyle = FormBorderStyle.None
        Me.ShowInTaskbar = False
        Me.TopMost = True
        Me.StartPosition = FormStartPosition.Manual
        Me.Opacity = 0
        Me.DoubleBuffered = True
        Me._targetScreen = screen

        Dim back As Color
        Dim border As Color
        Select Case tipo
            Case ToastType.Success : back = Color.FromArgb(46, 125, 50) : border = Color.FromArgb(27, 94, 32)
            Case ToastType.Warning : back = Color.FromArgb(245, 124, 0) : border = Color.FromArgb(230, 81, 0)
            Case ToastType.Error : back = Color.FromArgb(211, 47, 47) : border = Color.FromArgb(183, 28, 28)
            Case Else : back = Color.FromArgb(33, 150, 243) : border = Color.FromArgb(25, 118, 210)
        End Select

        Me.BackColor = back

        _lbl.Text = texto
        _lbl.Padding = New Padding(PaddingPx)
        _lbl.MaximumSize = New Size(MaxWidthPx, 0)
        _lbl.AutoSize = True
        _lbl.BackColor = Color.Transparent

        Using g = CreateGraphics()
            Dim proposed = New Size(MaxWidthPx - PaddingPx * 2, Integer.MaxValue)
            Dim sz = TextRenderer.MeasureText(g, texto, _lbl.Font, proposed, TextFormatFlags.WordBreak)
            Dim width = Math.Min(MaxWidthPx, sz.Width + PaddingPx * 2 + 2)
            Dim height = sz.Height + PaddingPx * 2 + 2
            Me.Size = New Size(width, height)
        End Using

        _lbl.Dock = DockStyle.Fill
        Controls.Add(_lbl)

        Me.Padding = New Padding(1)
        AddHandler Me.Paint, Sub(sender As Object, e As PaintEventArgs)
                                 Using pen As New Pen(border, 1)
                                     Dim rect = New Rectangle(0, 0, Me.ClientSize.Width - 1, Me.ClientSize.Height - 1)
                                     e.Graphics.SmoothingMode = SmoothingMode.AntiAlias
                                     Using gp As New GraphicsPath()
                                         Dim r = CornerRadius
                                         gp.AddArc(rect.X, rect.Y, r, r, 180, 90)
                                         gp.AddArc(rect.Right - r, rect.Y, r, r, 270, 90)
                                         gp.AddArc(rect.Right - r, rect.Bottom - r, r, r, 0, 90)
                                         gp.AddArc(rect.X, rect.Bottom - r, r, r, 90, 90)
                                         gp.CloseFigure()
                                         e.Graphics.DrawPath(pen, gp)
                                     End Using
                                 End Using
                             End Sub

        AddHandler _lifeTimer.Tick, AddressOf LifeTimer_Tick
        AddHandler _fadeTimer.Tick, AddressOf FadeTimer_Tick
        AddHandler Me.Click, Sub() BeginFadeOut()
        AddHandler _lbl.Click, Sub() BeginFadeOut()
    End Sub

    Protected Overrides ReadOnly Property CreateParams As CreateParams
        Get
            Dim cp = MyBase.CreateParams
            Const WS_EX_NOACTIVATE As Integer = &H8000000
            Const WS_EX_TOOLWINDOW As Integer = &H80
            cp.ExStyle = cp.ExStyle Or WS_EX_NOACTIVATE Or WS_EX_TOOLWINDOW
            Return cp
        End Get
    End Property

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        Dim rgn = CreateRoundRectRgn(0, 0, Me.Width, Me.Height, CornerRadius, CornerRadius)
        SetWindowRgn(Me.Handle, rgn, True)
        _fadingIn = True
        _fadeTimer.Start()
        _lifeTimer.Start()
    End Sub

    Private Sub ShowForm()
        MyBase.Show()
    End Sub

    Private Sub LifeTimer_Tick(sender As Object, e As EventArgs)
        _lifeTimer.Stop()
        BeginFadeOut()
    End Sub

    Private Sub FadeTimer_Tick(sender As Object, e As EventArgs)
        If _fadingIn Then
            Me.Opacity = Math.Min(1.0, Me.Opacity + FadeStep)
            If Me.Opacity >= 1.0 Then _fadeTimer.Stop()
        Else
            Me.Opacity = Math.Max(0.0, Me.Opacity - FadeStep)
            If Me.Opacity <= 0 Then
                _fadeTimer.Stop()
                Close()
            End If
        End If
    End Sub

    Private Sub BeginFadeOut()
        _fadingIn = False
        _fadeTimer.Start()
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        SyncLock OpenToasts
            Dim idx = OpenToasts.IndexOf(Me)
            If idx >= 0 Then
                OpenToasts.RemoveAt(idx)
                For i = idx To OpenToasts.Count - 1
                    Dim t = OpenToasts(i)
                    Dim wa = t._targetScreen.WorkingArea
                    Dim desiredY = wa.Top + MarginFromEdges + (i * (t.Height + StackGap))
                    t.Location = New Point(t.Location.X, desiredY)
                Next
            End If
        End SyncLock
    End Sub

    Public Shared Shadows Sub Show(owner As Form, message As String, Optional type As ToastType = ToastType.Success, Optional durationMs As Integer = DefaultDurationMs)
        ' ======================= INICIO DE LA CORRECCIÓN =======================
        ' Primero, verificar si el formulario 'owner' es válido antes de intentar cualquier operación con él.
        ' Si ya fue desechado (IsDisposed), salimos silenciosamente para evitar el crash.
        If owner IsNot Nothing AndAlso owner.IsDisposed Then
            Exit Sub
        End If
        ' ======================= FIN DE LA CORRECCIÓN =======================

        If owner IsNot Nothing AndAlso owner.InvokeRequired Then
            owner.BeginInvoke(CType(Sub() Show(owner, message, type, durationMs), MethodInvoker))
            Return
        End If

        Dim screen As Screen = If(owner IsNot Nothing, Screen.FromControl(owner), Screen.PrimaryScreen)
        Dim t As New Toast(message, type, screen)
        t._lifeTimer.Interval = durationMs

        Dim wa = screen.WorkingArea
        SyncLock OpenToasts
            Dim stackIndex = OpenToasts.Count
            ' TOP-RIGHT: arranca desde arriba y apila hacia abajo
            Dim x = wa.Right - t.Width - MarginFromEdges
            Dim y = wa.Top + MarginFromEdges + (stackIndex * (t.Height + StackGap))
            t.Location = New Point(x, y)
            OpenToasts.Add(t)
        End SyncLock

        t.ShowForm()
    End Sub

    Public Shared Shadows Sub Show(message As String, Optional type As ToastType = ToastType.Success, Optional durationMs As Integer = DefaultDurationMs)
        Show(owner:=Nothing, message:=message, type:=type, durationMs:=durationMs)
    End Sub
End Class

Public Module Notifier
    <System.Diagnostics.DebuggerStepThrough>
    Public Sub Info(f As Form, msg As String, Optional ms As Integer = 3000)
        Toast.Show(owner:=f, message:=msg, type:=ToastType.Info, durationMs:=ms)
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
End Module