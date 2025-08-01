' ChipControl.vb
Option Strict On
Option Explicit On
Imports System.Drawing
Imports System.Windows.Forms
Imports Apex.frmFiltroAvanzado
'Imports Apex.Filtros  ' si ReglaFiltro está en otro namespace

Public Class ChipControl
    Inherits UserControl

    Private ReadOnly _lblTexto As Label
    Private ReadOnly _btnCerrar As Button

    ' ← pon Friend si ReglaFiltro sigue siendo Friend
    Public ReadOnly Property Regla As ReglaFiltro

    Public Event CerrarClick As EventHandler

    ' ---------------------------
    ' CONSTRUCTOR DEL CONTROL
    ' ---------------------------
    Public Sub New(regla As ReglaFiltro)
        If regla Is Nothing Then Throw New ArgumentNullException(NameOf(regla))
        Me.Regla = regla

        SuspendLayout()

        AutoSize = True
        AutoSizeMode = AutoSizeMode.GrowAndShrink
        Margin = New Padding(3)
        BackColor = Color.FromArgb(220, 235, 255)
        BorderStyle = BorderStyle.FixedSingle

        _lblTexto = New Label() With {
            .AutoSize = True,
            .Location = New Point(3, 3),
            .Text = GetReglaDescripcion(regla)
        }

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

        Controls.Add(_lblTexto)
        Controls.Add(_btnCerrar)

        ResumeLayout(False)   ' ← aquí el motor calcula tamaños reales
        AjustarLayout()       ' ← llamas al método que acomoda todo
    End Sub

    ' ----------------------------------
    ' MÉTODO PRIVADO AJUSTAR LAYOUT
    ' ----------------------------------
    Private Sub AjustarLayout()
        ' Centrar verticalmente el botón respecto al label
        Dim y As Integer = Math.Max(0, (_lblTexto.Height - _btnCerrar.Height) \ 2)
        _btnCerrar.Location = New Point(_lblTexto.Right + 3, y)

        ' Definir tamaño global del chip
        Width = _btnCerrar.Right + 3
        Height = Math.Max(_lblTexto.Bottom, _btnCerrar.Bottom) + 3
    End Sub

    ' ----------------------------------
    ' DEMÁS MIEMBROS
    ' ----------------------------------
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
