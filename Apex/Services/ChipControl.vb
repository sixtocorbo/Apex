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

    ' --- CONSTRUCTOR CORREGIDO: Ya no depende del ancho del padre ---
    Public Sub New(regla As ReglaFiltro) ' Se elimina el parámetro parentWidth
        If regla Is Nothing Then Throw New ArgumentNullException(NameOf(regla))
        Me.Regla = regla

        SuspendLayout()

        ' Configuración básica del Chip
        AutoSize = True
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        Margin = New Padding(3)
        BackColor = Color.FromArgb(220, 235, 255)
        BorderStyle = BorderStyle.FixedSingle

        ' Botón de cerrar
        _btnCerrar = New Button() With {
          .Text = "×",
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Bold),
            .ForeColor = Color.DarkRed,
            .FlatStyle = FlatStyle.Flat,
            .Size = New Size(22, 22),
            .TabStop = False
        }
        _btnCerrar.FlatAppearance.BorderSize = 0
        AddHandler _btnCerrar.Click, AddressOf OnCerrarClick

        ' Label para el texto (configurado para auto-wrap con un máximo fijo)
        _lblTexto = New Label() With {
            .AutoSize = True, ' Se deja en True para que calcule su tamaño
            .Location = New Point(3, 3),
            .Text = GetReglaDescripcion(regla),
        .MaximumSize = New Size(350, 0)
        }      ' >>> CAMBIO CLAVE: Se establece un ancho máximo fijo para el texto.
        ' Esto fuerza el auto-wrap sin depender del contenedor.        ' Puedes ajustar este valor (350) si lo consideras necesario.

        Controls.Add(_lblTexto)
        Controls.Add(_btnCerrar)

        ResumeLayout(False)
        AjustarLayout()
    End Sub

    Private Sub AjustarLayout()
        ' Centra verticalmente el botón con respecto al label
        Dim yBoton As Integer = _lblTexto.Location.Y + Math.Max(0, (_lblTexto.Height - _btnCerrar.Height) \ 2)
        _btnCerrar.Location = New Point(_lblTexto.Right + 3, yBoton)

        ' Ajusta el tamaño del propio UserControl (el chip) para que contenga todo
        Width = _btnCerrar.Right + 3
        Height = _lblTexto.Bottom + 3
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