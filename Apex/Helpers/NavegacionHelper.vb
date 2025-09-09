' /Modules/NavegacionHelper.vb

Imports System.Windows.Forms
Imports System.Linq

Public Module NavegacionHelper

    ''' <summary>
    ''' ✅ CASO 1: Para formularios SIN parámetros que deben ser únicos (ej: Configuración).
    ''' Cierra el formulario de origen.
    ''' </summary>
    Public Sub AbrirFormUnicoEnDashboard(Of T As {Form, New})(Optional sourceForm As Form = Nothing)
        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dashboard Is Nothing Then Return

        Dim panelContenido = CType(dashboard.Controls("panelContenido"), Panel)
        Dim formularioExistente = panelContenido.Controls.OfType(Of T)().FirstOrDefault()
        Dim formToShow As Form

        If formularioExistente IsNot Nothing Then
            formToShow = formularioExistente
        Else
            formToShow = New T()
        End If

        dashboard.AbrirFormEnPanel(formToShow)

        If sourceForm IsNot Nothing Then
            sourceForm.Close()
        End If
    End Sub

    ''' <summary>
    ''' ✅ CASO 2: Para formularios de detalle que necesitan un ID y deben ser únicos.
    ''' Cierra una instancia previa antes de abrir la nueva.
    ''' </summary>
    Public Sub AbrirFormDetalleUnico(Of T As Form)(id As Integer, sourceForm As Form)
        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dashboard Is Nothing Then Return

        Dim panelContenido = CType(dashboard.Controls("panelContenido"), Panel)
        Dim formExistente = panelContenido.Controls.OfType(Of T)().FirstOrDefault()
        If formExistente IsNot Nothing Then
            formExistente.Close()
        End If

        Dim nuevoFormulario = CType(Activator.CreateInstance(GetType(T), id), Form)
        dashboard.AbrirFormEnPanel(nuevoFormulario)
    End Sub

    ''' <summary>
    ''' ✅ CASO 3: Para formularios que ya creaste (con 'New') porque necesitan muchos parámetros (ej: Reportes).
    ''' Simplemente toma el formulario y lo muestra en el panel.
    ''' </summary>
    Public Sub AbrirNuevaInstanciaEnDashboard(ByVal formularioAAbrir As Form)
        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dashboard IsNot Nothing Then
            dashboard.AbrirFormEnPanel(formularioAAbrir)
        Else
            ' Fallback por si el dashboard no se encuentra
            formularioAAbrir.Show()
        End If
    End Sub

End Module