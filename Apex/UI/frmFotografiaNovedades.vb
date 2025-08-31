' Apex/UI/frmVisorFoto.vb
Imports System.Drawing

Public Class frmFotografiaNovedades

    ''' <summary>
    ''' Constructor que recibe la imagen a mostrar.
    ''' </summary>
    Public Sub New(imagen As Image)
        InitializeComponent()
        pbFotoGrande.Image = imagen
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub frmFotografiaNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
    End Sub
End Class