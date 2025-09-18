Public Module LoadingHelper

    ' Panel con semitransparencia real y doble buffer.
    Private NotInheritable Class OverlayPanel
        Inherits Panel
        Public Property FillColor As Color = Color.FromArgb(140, 255, 255, 255)
        Public Sub New()
            SetStyle(ControlStyles.AllPaintingInWmPaint Or
                     ControlStyles.OptimizedDoubleBuffer Or
                     ControlStyles.UserPaint Or
                     ControlStyles.ResizeRedraw, True)
            TabStop = True
            BackColor = Color.Transparent
        End Sub
        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            Using b As New SolidBrush(FillColor)
                e.Graphics.FillRectangle(b, ClientRectangle)
            End Using
        End Sub
    End Class

    Private Class LoadingState
        Public Overlay As OverlayPanel
        Public Container As TableLayoutPanel
        Public Label As Label
        Public Bar As ProgressBar
        Public Count As Integer
        Public PrevActive As Control
        Public PrevUseWait As Boolean
    End Class

    Private ReadOnly loadingStates As New Dictionary(Of Control, LoadingState)()

    ' Overload conveniente para Form (compat).
    Public Sub MostrarCargando(form As Form, Optional texto As String = "Cargando…", Optional indeterminado As Boolean = True)
        MostrarCargando(DirectCast(form, Control), texto, indeterminado)
    End Sub

    ' Versión general para cualquier contenedor.
    Public Sub MostrarCargando(parent As Control, Optional texto As String = "Cargando…", Optional indeterminado As Boolean = True)
        If parent Is Nothing OrElse parent.IsDisposed Then Return
        If parent.InvokeRequired Then
            parent.Invoke(Sub() MostrarCargando(parent, texto, indeterminado))
            Return
        End If

        Dim st As LoadingState = Nothing
        If Not loadingStates.TryGetValue(parent, st) Then
            Dim overlay As New OverlayPanel With {
                .Dock = DockStyle.Fill,
                .FillColor = Color.FromArgb(140, 255, 255, 255),
                .Cursor = Cursors.WaitCursor
            }

            Dim lbl As New Label With {
                .AutoSize = True,
                .Font = New Font("Segoe UI", 14, FontStyle.Bold),
                .ForeColor = Color.FromArgb(64, 64, 64),
                .Text = texto,
                .BackColor = Color.Transparent
            }

            Dim pb As New ProgressBar With {
                .Width = 240,
                .Height = 22
            }
            If indeterminado Then
                pb.Style = ProgressBarStyle.Marquee
                pb.MarqueeAnimationSpeed = 30
            Else
                pb.Style = ProgressBarStyle.Blocks
                pb.Value = 0
            End If

            Dim container As New TableLayoutPanel With {
                .AutoSize = True,
                .AutoSizeMode = AutoSizeMode.GrowAndShrink,
                .BackColor = Color.Transparent,
                .Padding = New Padding(16)
            }
            container.ColumnCount = 1
            container.RowCount = 2
            container.Controls.Add(lbl, 0, 0)
            container.Controls.Add(pb, 0, 1)
            overlay.Controls.Add(container)

            ' Centrado al redimensionar
            AddHandler overlay.Resize, Sub()
                                           container.Location = New Point(
                                               (overlay.ClientSize.Width - container.Width) \ 2,
                                               (overlay.ClientSize.Height - container.Height) \ 2)
                                       End Sub

            parent.SuspendLayout()
            parent.Controls.Add(overlay)
            overlay.BringToFront()
            parent.ResumeLayout()

            st = New LoadingState With {
                .Overlay = overlay,
                .Container = container,
                .Label = lbl,
                .Bar = pb,
                .Count = 0,
                .PrevActive = parent.FindForm()?.ActiveControl,
                .PrevUseWait = If(parent.FindForm() IsNot Nothing, parent.FindForm().UseWaitCursor, False)
            }
            loadingStates(parent) = st

            Dim f = parent.FindForm()
            If f IsNot Nothing Then
                f.UseWaitCursor = True
                AddHandler f.FormClosed, Sub() SafeDispose(parent)
            End If
        End If

        st.Count += 1
        st.Label.Text = texto
        If indeterminado Then
            st.Bar.Style = ProgressBarStyle.Marquee
            st.Bar.MarqueeAnimationSpeed = 30
        Else
            st.Bar.Style = ProgressBarStyle.Blocks
            st.Bar.MarqueeAnimationSpeed = 0
            st.Bar.Value = Math.Min(Math.Max(st.Bar.Value, 0), 100)
        End If

        st.Overlay.Visible = True
        st.Overlay.BringToFront()
        st.Overlay.Focus()
    End Sub

    ' Cambia texto y/o progreso (0..100). Si se pasa porcentaje, cambia a modo determinado.
    Public Sub ActualizarCargando(parent As Control, Optional texto As String = Nothing, Optional porcentaje As Integer? = Nothing)
        If parent Is Nothing OrElse parent.IsDisposed Then Return
        If parent.InvokeRequired Then
            parent.Invoke(Sub() ActualizarCargando(parent, texto, porcentaje))
            Return
        End If
        Dim st As LoadingState = Nothing
        If Not loadingStates.TryGetValue(parent, st) Then Return
        If texto IsNot Nothing Then st.Label.Text = texto
        If porcentaje.HasValue Then
            st.Bar.Style = ProgressBarStyle.Blocks
            st.Bar.MarqueeAnimationSpeed = 0
            st.Bar.Value = Math.Max(0, Math.Min(100, porcentaje.Value))
        End If
    End Sub

    Public Sub OcultarCargando(form As Form)
        OcultarCargando(DirectCast(form, Control))
    End Sub

    Public Sub OcultarCargando(parent As Control)
        If parent Is Nothing OrElse parent.IsDisposed Then Return
        If parent.InvokeRequired Then
            parent.Invoke(Sub() OcultarCargando(parent))
            Return
        End If
        Dim st As LoadingState = Nothing
        If Not loadingStates.TryGetValue(parent, st) Then Return

        st.Count -= 1
        If st.Count > 0 Then Return

        SafeDispose(parent)
    End Sub

    Private Sub SafeDispose(parent As Control)
        Dim st As LoadingState = Nothing
        If Not loadingStates.TryGetValue(parent, st) Then Return

        Dim f = parent.FindForm()
        If f IsNot Nothing Then f.UseWaitCursor = st.PrevUseWait

        If st.PrevActive IsNot Nothing AndAlso Not st.PrevActive.IsDisposed Then
            Try : st.PrevActive.Focus() : Catch : End Try
        End If

        If st.Overlay IsNot Nothing Then
            If parent IsNot Nothing AndAlso Not parent.IsDisposed Then
                parent.Controls.Remove(st.Overlay)
            End If
            st.Overlay.Dispose()
        End If

        loadingStates.Remove(parent)
    End Sub

End Module
