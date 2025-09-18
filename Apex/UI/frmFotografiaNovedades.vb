' Apex/UI/frmVisorFoto.vb

Public Class frmFotografiaNovedades

    ''' <summary>
    ''' Constructor que recibe la imagen a mostrar.
    ''' </summary>
    Public Sub New(imagen As Image)
        InitializeComponent()
        pbFotoGrande.Image = imagen
    End Sub

    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub

    Private Sub frmFotografiaNovedades_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
    End Sub
End Class