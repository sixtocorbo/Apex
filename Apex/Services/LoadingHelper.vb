Imports System.Drawing
Imports System.Windows.Forms

Public Module LoadingHelper

    Private Class LoadingState
        Public Overlay As Panel
    End Class

    Private loadingStates As New Dictionary(Of Form, LoadingState)()

    Public Sub MostrarCargando(form As Form)
        If form.InvokeRequired Then
            form.Invoke(Sub() MostrarCargando(form))
            Return
        End If

        If loadingStates.ContainsKey(form) Then Return

        ' Panel overlay
        Dim overlay As New Panel With {
            .BackColor = Color.FromArgb(128, 255, 255, 255), ' Blanco semitransparente
            .Dock = DockStyle.Fill
        }

        ' Label
        Dim lbl As New Label With {
            .Text = "Cargando...",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .ForeColor = Color.DarkSlateBlue,
            .AutoSize = True,
            .BackColor = Color.Transparent
        }

        ' ProgressBar estilo Marquee
        Dim pb As New ProgressBar With {
            .Style = ProgressBarStyle.Marquee,
            .MarqueeAnimationSpeed = 30,
            .Width = 200,
            .Height = 20
        }

        ' Contenedor centrado
        Dim container As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.Transparent,
            .ColumnCount = 1,
            .RowCount = 2
        }
        container.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        container.RowStyles.Add(New RowStyle(SizeType.Percent, 50))
        container.Controls.Add(lbl, 0, 0)
        container.Controls.Add(pb, 0, 1)
        container.SetCellPosition(lbl, New TableLayoutPanelCellPosition(0, 0))
        container.SetCellPosition(pb, New TableLayoutPanelCellPosition(0, 1))
        container.Anchor = AnchorStyles.None
        container.Padding = New Padding(0)
        container.Margin = New Padding(0)

        overlay.Controls.Add(container)
        form.Controls.Add(overlay)
        overlay.BringToFront()

        loadingStates(form) = New LoadingState With {
            .Overlay = overlay
        }
    End Sub

    Public Sub OcultarCargando(form As Form)
        If form.InvokeRequired Then
            form.Invoke(Sub() OcultarCargando(form))
            Return
        End If

        If loadingStates.ContainsKey(form) Then
            Dim state = loadingStates(form)
            form.Controls.Remove(state.Overlay)
            state.Overlay.Dispose()
            loadingStates.Remove(form)
        End If
    End Sub
End Module
