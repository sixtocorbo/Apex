Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Public Class DayControl
    Inherits System.Windows.Forms.UserControl

    Private _dayNumber As Integer
    Private _hasEvent As Boolean
    Private _isSelected As Boolean

    Public Event DayClicked As EventHandler

    Public Sub New()
        InitializeComponent()
        DoubleBuffered = True ' Mejora el rendimiento del dibujado
    End Sub

    <Category("Appearance")>
    Public Property DayNumber As Integer
        Get
            Return _dayNumber
        End Get
        Set(value As Integer)
            _dayNumber = value
            lblDay.Text = If(value > 0, value.ToString(), "")
            Invalidate() ' Vuelve a dibujar el control
        End Set
    End Property

    <Category("Appearance")>
    Public Property HasEvent As Boolean
        Get
            Return _hasEvent
        End Get
        Set(value As Boolean)
            _hasEvent = value
            Invalidate()
        End Set
    End Property

    <Category("Appearance")>
    Public Property IsSelected As Boolean
        Get
            Return _isSelected
        End Get
        Set(value As Boolean)
            _isSelected = value
            Invalidate()
        End Set
    End Property

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Using g = e.Graphics
            g.SmoothingMode = SmoothingMode.AntiAlias

            ' Dibuja el borde de selección si está seleccionado
            If _isSelected Then
                Using p As New Pen(Color.DodgerBlue, 2)
                    g.DrawRectangle(p, 1, 1, Width - 2, Height - 2)
                End Using
            End If

            ' Dibuja el punto rojo si hay un evento
            If _hasEvent Then
                Dim circleDiameter As Single = 8
                Dim circleRect = New RectangleF(
                    (Width - circleDiameter) / 2,
                    lblDay.Bottom + 1,
                    circleDiameter,
                    circleDiameter)
                Using b As New SolidBrush(Color.Crimson)
                    g.FillEllipse(b, circleRect)
                End Using
            End If
        End Using
    End Sub

    ' Evento para notificar al formulario principal que se hizo clic
    Private Sub Control_Click(sender As Object, e As EventArgs) Handles Me.Click, lblDay.Click
        RaiseEvent DayClicked(Me, EventArgs.Empty)
    End Sub

    Private Sub DayControl_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        ' Mantiene el número del día centrado
        lblDay.Location = New Point((Width - lblDay.Width) / 2, 5)
        Invalidate()
    End Sub
End Class