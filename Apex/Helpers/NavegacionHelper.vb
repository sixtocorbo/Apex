Imports System.Windows.Forms
Imports System.Linq

Public Module NavegacionHelper

    ''' <summary>
    ''' Busca si ya existe una instancia del formulario especificado. Si existe, la trae al frente.
    ''' Si no existe, crea una nueva instancia y la abre dentro del panel del Dashboard.
    ''' </summary>
    ''' <typeparam name="T">El tipo del formulario a abrir (ej: frmConfiguracion).</typeparam>
    Public Sub AbrirFormUnicoEnDashboard(Of T As {Form, New})()

        ' 1. Busca la instancia abierta del Dashboard.
        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dashboard Is Nothing Then
            MessageBox.Show("No se encontró el dashboard principal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' 2. Busca si ya existe una instancia del formulario que queremos abrir (del tipo T).
        Dim formularioExistente = dashboard.MdiChildren.OfType(Of T)().FirstOrDefault()

        If formularioExistente IsNot Nothing Then
            ' 3. Si ya existe, lo trae al frente y lo activa.
            dashboard.AbrirFormEnPanel(formularioExistente)
        Else
            ' 4. Si no existe, crea una nueva instancia y la abre.
            Dim nuevoFormulario As New T()
            dashboard.AbrirFormEnPanel(nuevoFormulario)
        End If

    End Sub

    ''' <summary>
    ''' Mantiene la funcionalidad original por si necesitas abrir múltiples instancias de otros formularios.
    ''' </summary>
    Public Sub AbrirNuevaInstanciaEnDashboard(ByVal formularioAAbrir As Form)
        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dashboard IsNot Nothing Then
            dashboard.AbrirFormEnPanel(formularioAAbrir)
        Else
            formularioAAbrir.Show()
        End If
    End Sub

End Module