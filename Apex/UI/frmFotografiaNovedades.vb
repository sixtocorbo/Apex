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

    ''' <summary>
    ''' Cierra el formulario al hacer clic en la imagen.
    ''' </summary>
    Private Sub pbFotoGrande_Click(sender As Object, e As EventArgs) Handles pbFotoGrande.Click
        Me.Close()
    End Sub
End Class