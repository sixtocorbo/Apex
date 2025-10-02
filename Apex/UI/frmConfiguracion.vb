Public Class frmConfiguracion

    Private Sub frmConfiguracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarLayoutConfiguracion()
    End Sub
    ' Llamar después de InitializeComponent()
    Public Sub ConfigurarLayoutConfiguracion()
        ' ---- FlowLayout general
        With FlowLayoutPanel1
            .Dock = DockStyle.Fill
            .AutoScroll = True
            .WrapContents = True
            .FlowDirection = FlowDirection.LeftToRight
            .Padding = New Padding(12)
            .Margin = New Padding(0)
        End With

        ' ---- Normalizar botones (texto, tamaño mínimo, márgenes)
        Dim botones() As Button = {
        btnCargos, btnSecciones, btnAreasTrabajo, btnTurnos,
        btnGestionarIncidencias, btnSubDirecciones, btnNomenclaturas, btnEscalafones
    }

        For i = 0 To botones.Length - 1
            Dim b = botones(i)
            b.AutoSize = False
            b.Height = 44
            b.MinimumSize = New Size(220, 44)
            b.Margin = New Padding(12)
            b.FlatStyle = FlatStyle.Standard
            b.TextAlign = ContentAlignment.MiddleCenter
            b.TabIndex = i
        Next

        ' Ajuste responsivo de ancho (1/2/3 columnas según espacio)
        AddHandler FlowLayoutPanel1.Resize, AddressOf AjustarAnchoBotones
        AjustarAnchoBotones(Nothing, EventArgs.Empty)
    End Sub

    Private Sub AjustarAnchoBotones(sender As Object, e As EventArgs)
        Dim pnl = FlowLayoutPanel1
        If pnl Is Nothing OrElse pnl.ClientSize.Width <= 0 Then Exit Sub

        ' Medidas / “gaps”
        Dim gap As Integer = 24               ' suma aproximada de márgenes laterales (12 + 12)
        Dim paddingH As Integer = pnl.Padding.Left + pnl.Padding.Right

        ' Ancho deseado mínimo por botón
        Dim anchoMin As Integer = 260

        ' Calcular columnas posibles (máx 3)
        Dim anchoDisponible = pnl.ClientSize.Width - paddingH
        Dim cols As Integer = Math.Max(1, Math.Min(3, (anchoDisponible + gap) \ (anchoMin + gap)))

        ' Recalcular ancho exacto por columna
        Dim anchoBtn As Integer = Math.Max(anchoMin, (anchoDisponible - (cols - 1) * gap) \ cols)

        For Each c In pnl.Controls
            Dim b = TryCast(c, Button)
            If b IsNot Nothing Then
                b.Width = anchoBtn
                b.Height = 44
            End If
        Next
    End Sub

    ' === Helper local para abrir hijos en el Dashboard (usando la pila) ===
    Private Sub AbrirHijoEnDashboard(Of T As {Form, New})()
        Dim dash = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dash Is Nothing Then
            MessageBox.Show("No se encontró el Dashboard activo.", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        ' Creamos una instancia fresca del hijo y lo abrimos como “child”.
        dash.AbrirChild(New T())
    End Sub

    Private Sub btnGestionarIncidencias_Click(sender As Object, e As EventArgs) Handles btnGestionarIncidencias.Click
        AbrirHijoEnDashboard(Of frmIncidencias)()
    End Sub

    Private Sub btnCargos_Click(sender As Object, e As EventArgs) Handles btnCargos.Click
        AbrirHijoEnDashboard(Of frmGrados)()
    End Sub

    Private Sub btnSecciones_Click(sender As Object, e As EventArgs) Handles btnSecciones.Click
        AbrirHijoEnDashboard(Of frmSecciones)()
    End Sub

    Private Sub btnAreasTrabajo_Click(sender As Object, e As EventArgs) Handles btnAreasTrabajo.Click
        AbrirHijoEnDashboard(Of frmAreaTrabajoCategorias)()
    End Sub

    Private Sub btnTurnos_Click(sender As Object, e As EventArgs) Handles btnTurnos.Click
        AbrirHijoEnDashboard(Of frmTurnos)()
    End Sub

    Private Sub btnNomenclaturas_Click(sender As Object, e As EventArgs) Handles btnNomenclaturas.Click
        ' Antes: abrías y cerrabas este formulario.
        ' Ahora: lo abrimos como “child”; este queda oculto y se restaura al cerrar el hijo.
        AbrirHijoEnDashboard(Of frmNomenclaturas)()
    End Sub

    Private Sub btnSubDirecciones_Click(sender As Object, e As EventArgs) Handles btnSubDirecciones.Click
        AbrirHijoEnDashboard(Of frmSubDirecciones)()
    End Sub

    Private Sub btnEscalafones_Click(sender As Object, e As EventArgs) Handles btnEscalafones.Click
        AbrirHijoEnDashboard(Of frmEscalafones)()
    End Sub

End Class
