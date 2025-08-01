Imports Apex.frmFiltroAvanzado

Public Class ChipControl
    Inherits UserControl

    Private ReadOnly lblTexto As Label
    Private ReadOnly btnCerrar As Button
    Public ReadOnly Property Regla As ReglaFiltro

    Public Event CerrarClick As EventHandler

    Public Sub New(regla As ReglaFiltro)
        Me.Regla = regla

        ' Use SuspendLayout for performance during initialization
        Me.SuspendLayout()

        ' Basic properties for the UserControl itself
        Me.AutoSize = True
        Me.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.Margin = New Padding(3)
        Me.BackColor = Color.FromArgb(220, 235, 255)
        Me.BorderStyle = BorderStyle.FixedSingle

        ' Label for the filter text
        lblTexto = New Label() With {
           .Text = GetReglaDescripcion(regla),
           .AutoSize = True,
           .Location = New Point(3, 5),
           .TextAlign = ContentAlignment.MiddleLeft
        }

        ' Button to close/remove the chip
        btnCerrar = New Button() With {
           .Text = "×",
           .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
           .ForeColor = Color.DarkRed,
           .FlatStyle = FlatStyle.Flat,
           .Size = New Size(22, 22)
        }
        btnCerrar.FlatAppearance.BorderSize = 0
        AddHandler btnCerrar.Click, AddressOf OnCerrarClick

        ' Add controls to the UserControl
        Me.Controls.Add(lblTexto)
        Me.Controls.Add(btnCerrar)

        ' Position the close button next to the label
        btnCerrar.Location = New Point(lblTexto.Right + 3, (Me.Height - btnCerrar.Height) \ 2)

        ' Set final size
        Me.Width = btnCerrar.Right + 3
        Me.Height = lblTexto.Height + 10

        Me.ResumeLayout(False)
    End Sub

    Private Sub OnCerrarClick(sender As Object, e As EventArgs)
        RaiseEvent CerrarClick(Me, EventArgs.Empty)
    End Sub

    Private Function GetReglaDescripcion(regla As ReglaFiltro) As String
        Dim descripcionValores As String = If(regla.Operador = OperadorComparacion.EnLista, regla.Valor1.Replace("|", ", "), regla.Valor1)
        Return $"{regla.Columna}: {descripcionValores}"
    End Function

End Class
