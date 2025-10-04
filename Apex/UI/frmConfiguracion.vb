Imports System.Linq

Public Class frmConfiguracion

    Private Shared ReadOnly CatalogosPredeterminados As CatalogoConfiguracion() = {
        New CatalogoConfiguracion("Gestionar Cargos", GetType(frmGrados), 10),
        New CatalogoConfiguracion("Gestionar Secciones", GetType(frmSecciones), 20),
        New CatalogoConfiguracion("Gestionar Áreas de Trabajo", GetType(frmAreaTrabajoCategorias), 30),
        New CatalogoConfiguracion("Gestionar Turnos", GetType(frmTurnos), 40),
        New CatalogoConfiguracion("Gestionar Incidencias", GetType(frmIncidencias), 50),
        New CatalogoConfiguracion("Gestionar Subdirecciones", GetType(frmSubDirecciones), 60),
        New CatalogoConfiguracion("Gestionar Unidades Generales", GetType(frmUnidadesGenerales), 70),
        New CatalogoConfiguracion("Nomenclaturas", GetType(frmNomenclaturas), 80),
        New CatalogoConfiguracion("Gestionar Escalafones", GetType(frmEscalafones), 90),
        New CatalogoConfiguracion("Gestionar Funciones", GetType(frmFunciones), 100)
    }

    Private Shared ReadOnly CatalogosExtras As New List(Of CatalogoConfiguracion)

    Private Sub frmConfiguracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarLayoutConfiguracion()
        ConstruirCatalogos()
    End Sub

    ''' <summary>
    ''' Permite registrar catálogos adicionales antes de mostrar el formulario.
    ''' </summary>
    ''' <param name="texto">Texto visible en el botón.</param>
    ''' <param name="formulario">Formulario que se abrirá al hacer clic.</param>
    ''' <param name="orden">Orden opcional para posicionar el botón (se ordena ascendente).</param>
    Public Shared Sub RegistrarCatalogo(texto As String, formulario As Type, Optional orden As Integer? = Nothing)
        If String.IsNullOrWhiteSpace(texto) Then Throw New ArgumentException("El texto del catálogo no puede estar vacío.", NameOf(texto))
        If formulario Is Nothing Then Throw New ArgumentNullException(NameOf(formulario))
        If Not GetType(Form).IsAssignableFrom(formulario) Then
            Throw New ArgumentException("El tipo indicado debe heredar de Form.", NameOf(formulario))
        End If

        Dim nuevoCatalogo As New CatalogoConfiguracion(texto.Trim(), formulario, orden.GetValueOrDefault(CalcularOrdenSugerido()))

        SyncLock CatalogosExtras
            CatalogosExtras.Add(nuevoCatalogo)
        End SyncLock
    End Sub

    Private Shared Function CalcularOrdenSugerido() As Integer
        Dim baseOrden = CatalogosPredeterminados.Length * 10
        Dim extrasCount As Integer
        SyncLock CatalogosExtras
            extrasCount = CatalogosExtras.Count
        End SyncLock
        Return baseOrden + ((extrasCount + 1) * 10)
    End Function

    Private Sub ConstruirCatalogos()
        Dim catalogos As List(Of CatalogoConfiguracion) = ObtenerCatalogos().OrderBy(Function(c) c.Orden).ThenBy(Function(c) c.Texto).ToList()

        FlowLayoutPanel1.SuspendLayout()
        FlowLayoutPanel1.Controls.Clear()

        For i = 0 To catalogos.Count - 1
            Dim catalogo = catalogos(i)
            Dim boton = CrearBotonCatalogo(catalogo, i)
            FlowLayoutPanel1.Controls.Add(boton)
        Next

        FlowLayoutPanel1.ResumeLayout()
        AjustarAnchoBotones(Nothing, EventArgs.Empty)
    End Sub

    Private Shared Function ObtenerCatalogos() As IEnumerable(Of CatalogoConfiguracion)
        Dim extras As CatalogoConfiguracion()
        SyncLock CatalogosExtras
            extras = CatalogosExtras.ToArray()
        End SyncLock

        Return CatalogosPredeterminados.Concat(extras)
    End Function

    Private Function CrearBotonCatalogo(catalogo As CatalogoConfiguracion, tabIndex As Integer) As Button
        Dim boton As New Button() With {
            .AutoSize = False,
            .Height = 44,
            .MinimumSize = New Size(220, 44),
            .Margin = New Padding(12),
            .FlatStyle = FlatStyle.Standard,
            .TextAlign = ContentAlignment.MiddleCenter,
            .TabIndex = tabIndex,
            .Text = catalogo.Texto,
            .Tag = catalogo
        }
        AddHandler boton.Click, AddressOf Catalogo_Click
        Return boton
    End Function

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
            If b Is Not Nothing Then
                b.Width = anchoBtn
                b.Height = 44
            End If
        Next
    End Sub

    Private Sub Catalogo_Click(sender As Object, e As EventArgs)
        Dim boton = TryCast(sender, Button)
        Dim catalogo = TryCast(boton?.Tag, CatalogoConfiguracion)
        If catalogo Is Nothing Then Return

        AbrirHijoEnDashboard(catalogo.Formulario)
    End Sub

    ' === Helper local para abrir hijos en el Dashboard (usando la pila) ===
    Private Sub AbrirHijoEnDashboard(formType As Type)
        Dim dash = Application.OpenForms.OfType(Of frmDashboard)().FirstOrDefault()
        If dash Is Nothing Then
            MessageBox.Show("No se encontró el Dashboard activo.", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If
        ' Creamos una instancia fresca del hijo y lo abrimos como “child”.
        Dim child = TryCast(Activator.CreateInstance(formType), Form)
        If child Is Nothing Then
            MessageBox.Show("No se pudo crear el formulario solicitado.", "Navegación", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        dash.AbrirChild(child)
    End Sub

    Private Class CatalogoConfiguracion
        Public Sub New(texto As String, formulario As Type, orden As Integer)
            Me.Texto = texto
            Me.Formulario = formulario
            Me.Orden = orden
        End Sub

        Public ReadOnly Property Texto As String
        Public ReadOnly Property Formulario As Type
        Public ReadOnly Property Orden As Integer
    End Class
End Class
