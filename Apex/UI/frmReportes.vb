Imports System.Linq

Public Class frmReportes
    Private Shared ReadOnly CatalogosDinamicos As New List(Of CatalogoReporte)()
    Private Shared ReadOnly CatalogosSyncRoot As New Object()

    Private Sub frmReportes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarLayoutReportes()
        ConstruirCatalogos()
    End Sub

    Private Sub ConfigurarLayoutReportes()
        Dim panel = FlowLayoutPanel1
        If panel Is Nothing Then Return

        With panel
            .Dock = DockStyle.Fill
            .AutoScroll = True
            .WrapContents = True
            .FlowDirection = FlowDirection.LeftToRight
            .Padding = New Padding(12)
            .Margin = New Padding(0)
        End With

        RemoveHandler panel.Resize, AddressOf AjustarAnchoBotones
        AddHandler panel.Resize, AddressOf AjustarAnchoBotones
    End Sub

    Private Sub ConstruirCatalogos()
        Dim panel = FlowLayoutPanel1
        If panel Is Nothing OrElse panel.IsDisposed Then Return

        panel.SuspendLayout()
        panel.Controls.Clear()

        Dim catalogos = ObtenerCatalogos().ToList()
        For i = 0 To catalogos.Count - 1
            Dim catalogo = catalogos(i)
            Dim boton = CrearBotonCatalogo(catalogo, i)
            panel.Controls.Add(boton)
        Next

        panel.ResumeLayout()
        AjustarAnchoBotones(Nothing, EventArgs.Empty)
    End Sub

    Private Iterator Function ObtenerCatalogosPredeterminados() As IEnumerable(Of CatalogoReporte)
        Yield CrearCatalogo(Of frmResumenCantidades)("📋 Resumen de Cantidades")
        Yield CrearCatalogo(Of frmAnalisisEstacionalidad)("   📊 Análisis de Licencias")
        Yield CrearCatalogo(Of frmReporteFuncionariosGenero)("♀️ Distribución por Género")
        Yield CrearCatalogo(Of frmReporteFuncionariosEdad)(" Distribución por Edad")
        Yield CrearCatalogo(Of frmReporteFuncionariosAreaTrabajo)("🏢 Distribución por Área")
        Yield CrearCatalogo(Of frmReporteFuncionariosCargo)("🧭 Top Cargos con más Personal")
        Yield CrearCatalogo(Of frmReporteFuncionariosEstado)("🟢 Activos vs. Inactivos")
        Yield CrearCatalogo(Of frmReporteFuncionariosTurno)("⏰ Distribución por Turno")
        Yield CrearCatalogo(Of frmReporteFuncionariosNivelEstudio)("🎓 Nivel de Estudios")
        Yield CrearCatalogo(Of frmReporteLicenciasPorTipo)("🗂️ Licencias por Tipo")
        Yield CrearCatalogo(Of frmReporteLicenciasPorEstado)("📑 Licencias por Estado")
        Yield CrearCatalogo(Of frmReporteTopFuncionariosLicencias)("🏅 Funcionarios con más Licencias")
        Yield CrearCatalogo(Of frmReportePresenciasPorSeccion)("👥 Presentes por Sección")
        Yield CrearCatalogo(Of frmReportePresenciasPorPuestoTrabajo)("🛠️ Presentes por Puesto")
    End Function

    Private Function ObtenerCatalogos() As IEnumerable(Of CatalogoReporte)
        Dim lista = New List(Of CatalogoReporte)()
        lista.AddRange(ObtenerCatalogosPredeterminados())

        SyncLock CatalogosSyncRoot
            lista.AddRange(CatalogosDinamicos)
        End SyncLock

        Return lista
    End Function

    Private Function CrearBotonCatalogo(catalogo As CatalogoReporte, tabIndex As Integer) As Button
        Dim boton = New Button()

        With boton
            .AutoSize = False
            .Height = 44
            .MinimumSize = New Size(220, 44)
            .Margin = New Padding(12)
            .FlatStyle = FlatStyle.Standard
            .TextAlign = ContentAlignment.MiddleCenter
            .Text = catalogo.Titulo
            .TabIndex = tabIndex
        End With

        AddHandler boton.Click,
            Sub(sender, e)
                AbrirCatalogo(catalogo)
            End Sub

        Return boton
    End Function

    Private Shared Function CrearCatalogo(Of T As {Form, New})(titulo As String) As CatalogoReporte
        Return New CatalogoReporte(titulo, Function() New T())
    End Function

    Private Sub AbrirCatalogo(catalogo As CatalogoReporte)
        If catalogo Is Nothing Then Return

        Dim frm = catalogo.CrearFormulario()
        AbrirFormularioReporte(frm)
    End Sub

    Private Sub AbrirFormularioReporte(frm As Form)
        If frm Is Nothing Then Return

        PrepararFormularioParaEscape(frm)
        MostrarEnDashboard(frm)
    End Sub

    Private Sub PrepararFormularioParaEscape(frm As Form)
        If frm Is Nothing Then Return

        frm.KeyPreview = True
        AddHandler frm.KeyDown, AddressOf FormularioReporte_KeyDown
    End Sub

    Private Sub FormularioReporte_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode <> Keys.Escape Then Return

        e.Handled = True
        Dim frm = TryCast(sender, Form)
        frm?.Close()
    End Sub

    Private Sub MostrarEnDashboard(frm As Form)
        If frm Is Nothing Then Return

        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dashboard IsNot Nothing Then
            dashboard.AbrirChild(frm)
        Else
            frm.Show()
        End If
    End Sub

    Private Sub AjustarAnchoBotones(sender As Object, e As EventArgs)
        Dim pnl = FlowLayoutPanel1
        If pnl Is Nothing OrElse pnl.ClientSize.Width <= 0 Then Exit Sub

        Dim gap As Integer = 24
        Dim paddingH As Integer = pnl.Padding.Left + pnl.Padding.Right
        Dim anchoMin As Integer = 260
        Dim anchoDisponible = pnl.ClientSize.Width - paddingH
        Dim cols As Integer = Math.Max(1, Math.Min(3, (anchoDisponible + gap) \ (anchoMin + gap)))
        Dim anchoBtn As Integer = Math.Max(anchoMin, (anchoDisponible - (cols - 1) * gap) \ cols)

        For Each c In pnl.Controls
            Dim b = TryCast(c, Button)
            If b IsNot Nothing Then
                b.Width = anchoBtn
                b.Height = 44
            End If
        Next
    End Sub

    Public Shared Sub RegistrarCatalogo(titulo As String, creador As Func(Of Form))
        If String.IsNullOrWhiteSpace(titulo) Then Throw New ArgumentException("El título no puede ser vacío", NameOf(titulo))
        If creador Is Nothing Then Throw New ArgumentNullException(NameOf(creador))

        Dim catalogo = New CatalogoReporte(titulo.Trim(), creador)

        SyncLock CatalogosSyncRoot
            CatalogosDinamicos.RemoveAll(Function(c) String.Equals(c.Titulo, catalogo.Titulo, StringComparison.OrdinalIgnoreCase))
            CatalogosDinamicos.Add(catalogo)
        End SyncLock

        For Each frm In Application.OpenForms.OfType(Of frmReportes)().ToArray()
            frm.ConstruirCatalogos()
        Next
    End Sub

    Public Shared Sub RegistrarCatalogo(Of T As {Form, New})(titulo As String)
        RegistrarCatalogo(titulo, Function() New T())
    End Sub

    Private NotInheritable Class CatalogoReporte
        Public ReadOnly Property Titulo As String
        Private ReadOnly creador As Func(Of Form)

        Public Sub New(titulo As String, creador As Func(Of Form))
            Me.Titulo = titulo
            Me.creador = creador
        End Sub

        Public Function CrearFormulario() As Form
            Return creador?.Invoke()
        End Function
    End Class
End Class
