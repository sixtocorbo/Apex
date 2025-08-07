' Apex/Services/ChipControl.vb
Option Strict On
Option Explicit On
Imports System.Drawing
Imports System.Windows.Forms
Imports Apex.frmFiltroAvanzado

Public Class ChipControl
    Inherits UserControl

    Private ReadOnly _lblTexto As Label
    Private ReadOnly _btnCerrar As Button

    Public ReadOnly Property Regla As ReglaFiltro
    Public Event CerrarClick As EventHandler

    Public Sub New(regla As ReglaFiltro)
        If regla Is Nothing Then Throw New ArgumentNullException(NameOf(regla))
        Me.Regla = regla

        SuspendLayout()

        ' Configuración básica del Chip
        AutoSize = True
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        Margin = New Padding(3)
        BackColor = Color.FromArgb(220, 235, 255)
        BorderStyle = BorderStyle.FixedSingle

        ' Botón de cerrar (con el carácter tipográfico '×' de multiplicación)
        _btnCerrar = New Button() With {
            .Text = "×",
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .ForeColor = Color.DarkRed,
            .FlatStyle = FlatStyle.Flat,
            .Size = New Size(22, 22),
            .TabStop = False,
            .Cursor = Cursors.Hand ' Mejora la experiencia de usuario
        }
        _btnCerrar.FlatAppearance.BorderSize = 0
        AddHandler _btnCerrar.Click, AddressOf OnCerrarClick

        ' Label para el texto (configurado para auto-wrap con un máximo fijo)
        _lblTexto = New Label() With {
            .AutoSize = True,
            .Text = GetReglaDescripcion(regla),
            .MaximumSize = New Size(350, 0),
            .TextAlign = ContentAlignment.MiddleLeft
        }

        ' Se añaden en el orden visual deseado (aunque la posición se define en el layout)
        Controls.Add(_btnCerrar)
        Controls.Add(_lblTexto)

        ResumeLayout(False)
        AjustarLayout()
    End Sub

    Private Sub AjustarLayout()
        ' --- INICIO DE LA CORRECCIÓN: Botón a la izquierda ---

        ' 1. Posicionar el botón de cerrar primero, a la izquierda del control.
        '    El 'Y' se calcula para centrarlo verticalmente respecto a la altura del texto.
        Dim yBoton As Integer = 3 + Math.Max(0, (_lblTexto.Height - _btnCerrar.Height) \ 2)
        _btnCerrar.Location = New Point(3, yBoton)

        ' 2. Posicionar el texto a la derecha del botón, con un pequeño margen.
        _lblTexto.Location = New Point(_btnCerrar.Right + 3, 3)

        ' 3. Ajustar el tamaño total del control (el chip) para que contenga a ambos elementos.
        Width = _lblTexto.Right + 3
        Height = Math.Max(_btnCerrar.Bottom, _lblTexto.Bottom) + 3

        ' --- FIN DE LA CORRECCIÓN ---
    End Sub

    Private Sub OnCerrarClick(sender As Object, e As EventArgs)
        RaiseEvent CerrarClick(Me, EventArgs.Empty)
    End Sub

    Private Function GetReglaDescripcion(r As ReglaFiltro) As String
        Dim valores = If(r.Operador = OperadorComparacion.EnLista,
                           r.Valor1.Replace("|", ", "),
                           r.Valor1)
        Return $"{r.Columna}: {valores}"
    End Function
End Class