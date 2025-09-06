''Option Strict On
''Option Explicit On

''Imports System.Data.Entity
''Imports System.Data.SqlClient
''Imports System.Drawing
''Imports System.IO
''Imports System.Text
''Imports System.Threading
''Imports System.Windows.Forms

''Public Class frmFuncionarioBuscar
''    Inherits Form


''    Public Enum ModoApertura
''        Navegacion ' Para ver y editar desde el Dashboard
''        Seleccion  ' Para seleccionar un funcionario y devolverlo
''    End Enum

''    Private ReadOnly _modo As ModoApertura
''    Private Const LIMITE_FILAS As Integer = 500
''    Private _detallesEstadoActual As New List(Of String)

''    ' Temporizador para la búsqueda automática.
''    ' Se especifica el namespace completo para evitar la ambigüedad.
''    Private WithEvents SearchTimer As New System.Windows.Forms.Timer()

''    Public ReadOnly Property FuncionarioSeleccionado As FuncionarioMin

''        Get
''            If dgvResultados.CurrentRow IsNot Nothing Then
''                Return CType(dgvResultados.CurrentRow.DataBoundItem, FuncionarioMin)
''            End If
''            Return Nothing
''        End Get
''    End Property

''    Public Sub New()
''        InitializeComponent()

''        _modo = ModoApertura.Navegacion
''        FlowLayoutPanelAcciones.Visible = False
''    End Sub

''    Public Sub New(modo As ModoApertura)
''        Me.New()
''        _modo = modo
''        FlowLayoutPanelAcciones.Visible = (_modo = ModoApertura.Seleccion)
''    End Sub

''    Private Sub frmFuncionarioBuscar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
''        ' Este botón indica estado prioritario: que no cambie de color al pasar el mouse
''        btnVerSituacion.Tag = "KeepBackColor"

''        AppTheme.Aplicar(Me)
''        ConfigurarGrilla()
''        AddHandler btnVerSituacion.Click, AddressOf btnVerSituacion_Click
''        AddHandler btnGenerarFicha.Click, AddressOf btnGenerarFicha_Click
''        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados

''        ' Configurar el temporizador de búsqueda
''        SearchTimer.Interval = 500 ' 500ms de espera antes de buscar
''        AddHandler SearchTimer.Tick, AddressOf SearchTimer_Tick
''        AddHandler txtBusqueda.TextChanged, AddressOf txtBusqueda_TextChanged
''    End Sub

''    Private Sub txtBusqueda_TextChanged(sender As Object, e As EventArgs)
''        ' Reiniciar el temporizador cada vez que el texto cambia
''        SearchTimer.Stop()
''        SearchTimer.Start()
''    End Sub

''    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs)
''        ' Cuando el temporizador se cumple, detenerlo y ejecutar la búsqueda
''        SearchTimer.Stop()
''        Await BuscarAsync()
''    End Sub

''    ''' <summary>
''    ''' Este método se ejecutará automáticamente cuando otro formulario notifique un cambio.
''    ''' </summary>
''    Private Async Sub OnDatosActualizados(sender As Object, e As EventArgs)

''        ' El formulario se actualizará incluso si está oculto, asegurando que los datos
''        ' estén frescos cuando el usuario vuelva a esta pantalla.
''        If Me.IsHandleCreated Then
''            Await BuscarAsync()
''        End If

''    End Sub

''    ' Es una buena práctica desuscribirse para evitar fugas de memoria.
''    Private Sub frmFuncionarioBuscar_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
''        RemoveHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
''    End Sub

''#Region "Diseño de grilla"
''    Private Sub ConfigurarGrilla()
''        With dgvResultados
''            .AutoGenerateColumns = False
''            .RowTemplate.Height = 40
''            .RowTemplate.MinimumHeight = 40
''            .Columns.Clear()


''            .Columns.Add(New DataGridViewTextBoxColumn With {
''                .Name = "Id",
''                .DataPropertyName = "Id",
''                .Visible = False
''            })

''            .Columns.Add(New DataGridViewTextBoxColumn With {
''                .Name = "CI",
''                .DataPropertyName = "CI",
''                .HeaderText = "CI",
''                .Width = 90
''            })

''            .Columns.Add(New DataGridViewTextBoxColumn With {
''                .Name = "Nombre",
''                .DataPropertyName = "Nombre",
''                .HeaderText = "Nombre",
''                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
''            })
''        End With

''        ' --- CAMBIO REALIZADO AQUÍ ---
''        ' Cambiamos el evento para que la actualización con las teclas sea robusta.
''        AddHandler dgvResultados.CurrentCellChanged, AddressOf MostrarDetalle
''        ' --- FIN DEL CAMBIO ---

''        AddHandler dgvResultados.CellDoubleClick, AddressOf OnDgvDoubleClick
''        AddHandler dgvResultados.DataError, Sub(s, ev) ev.ThrowException = False
''    End Sub
''#End Region

''#Region "Búsqueda con Full-Text y CONTAINS"
''    Private Async Function BuscarAsync() As Task
''        ' Si no hay texto en la caja de búsqueda, limpiamos la grilla y el detalle.
''        If String.IsNullOrWhiteSpace(txtBusqueda.Text) Then
''            dgvResultados.DataSource = New List(Of FuncionarioMin)()
''            LimpiarDetalle()
''            Return
''        End If

''        LoadingHelper.MostrarCargando(Me)

''        Try
''            Using uow As New UnitOfWork()
''                Dim ctx = uow.Context
''                Dim filtro As String = txtBusqueda.Text.Trim()

''                Dim terminos = filtro.Split(" "c) _
''                                     .Where(Function(w) Not String.IsNullOrWhiteSpace(w)) _
''                                     .Select(Function(w) $"""{w}*""")
''                Dim expresionFts = String.Join(" AND ", terminos)

''                Dim sb As New StringBuilder()
''                sb.AppendLine("SELECT TOP (@limite)")
''                sb.AppendLine("      Id, CI, Nombre")
''                sb.AppendLine("FROM   dbo.Funcionario WITH (NOLOCK)")
''                sb.AppendLine("WHERE  CONTAINS((CI, Nombre), @patron)")
''                sb.AppendLine("ORDER BY Nombre;")

''                Dim sql = sb.ToString()
''                Dim pLimite = New SqlParameter("@limite", LIMITE_FILAS)
''                Dim pPatron = New SqlParameter("@patron", expresionFts)

''                Dim lista = Await ctx.Database _
''                                .SqlQuery(Of FuncionarioMin)(sql, pLimite, pPatron) _
''                                .ToListAsync()

''                dgvResultados.DataSource = Nothing
''                dgvResultados.DataSource = lista

''                If lista.Any() Then
''                    dgvResultados.ClearSelection()
''                    dgvResultados.Rows(0).Selected = True
''                    dgvResultados.CurrentCell = dgvResultados.Rows(0).Cells("CI")
''                    btnGenerarFicha.Visible = True
''                Else
''                    LimpiarDetalle()
''                End If

''                If lista.Count = LIMITE_FILAS Then
''                    MessageBox.Show($"Mostrando los primeros {LIMITE_FILAS} resultados." &
''                                    "Refiná la búsqueda para ver más.",
''                                "Aviso",
''                                MessageBoxButtons.OK, MessageBoxIcon.Information)
''                End If
''            End Using

''        Catch ex As SqlException When ex.Number = -2
''            MessageBox.Show("La consulta excedió el tiempo de espera. Refiná los filtros o intentá nuevamente.",
''                    "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning)

''        Catch ex As Exception
''            MessageBox.Show("Ocurrió un error inesperado: " & ex.Message,
''                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

''        Finally
''            LoadingHelper.OcultarCargando(Me)
''        End Try
''    End Function

''    ' --- MÉTODO CORREGIDO: Se quita "Async" ---
''    Private Sub txtBusqueda_KeyDown(sender As Object, e As KeyEventArgs) _
''    Handles txtBusqueda.KeyDown

''        If e.KeyCode = Keys.Enter Then
''            e.Handled = True
''            e.SuppressKeyPress = True
''        End If

''        If dgvResultados.Rows.Count = 0 Then Return

''        Select Case e.KeyCode
''            Case Keys.Down : MoverSeleccion(+1) : e.Handled = True
''            Case Keys.Up : MoverSeleccion(-1) : e.Handled = True
''        End Select
''    End Sub

''    Private Sub MoverSeleccion(direccion As Integer)
''        Dim total = dgvResultados.Rows.Count
''        If total = 0 Then
''            LimpiarDetalle()
''            Exit Sub
''        End If

''        Dim indexActual As Integer =
''        If(dgvResultados.CurrentRow Is Nothing, -1, dgvResultados.CurrentRow.Index)

''        Dim nuevoIndex = Math.Max(0, Math.Min(total - 1, indexActual + direccion))

''        dgvResultados.ClearSelection()
''        dgvResultados.Rows(nuevoIndex).Selected = True
''        dgvResultados.CurrentCell = dgvResultados.Rows(nuevoIndex).Cells("CI")
''    End Sub
''#End Region


''#Region "Detalle lateral (Foto on-demand)"
''    Private Async Sub MostrarDetalle(sender As Object, e As EventArgs)
''        If dgvResultados.CurrentRow Is Nothing OrElse dgvResultados.CurrentRow.DataBoundItem Is Nothing Then
''            LimpiarDetalle()
''            Return
''        End If
''        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)

''        Using uow As New UnitOfWork()
''            Dim f = Await uow.Repository(Of Funcionario)() _
''             .GetAll() _
''            .Include(Function(x) x.Cargo) _
''            .Include(Function(x) x.TipoFuncionario) _
''            .Include(Function(x) x.Semana) _
''            .Include(Function(x) x.Turno) _
''            .Include(Function(x) x.Horario) _
''            .AsNoTracking() _
''            .FirstOrDefaultAsync(Function(x) x.Id = id)

''            If dgvResultados.CurrentRow Is Nothing OrElse CInt(dgvResultados.CurrentRow.Cells("Id").Value) <> id Then Return
''            If f Is Nothing Then Return

''            lblCI.Text = f.CI
''            lblNombreCompleto.Text = f.Nombre
''            lblCargo.Text = If(f.Cargo Is Nothing, "-", f.Cargo.Nombre)
''            lblTipo.Text = f.TipoFuncionario.Nombre
''            lblFechaIngreso.Text = f.FechaIngreso.ToShortDateString()
''            lblHorarioCompleto.Text = $"{If(f.Semana IsNot Nothing, f.Semana.Nombre, "-")} / {If(f.Turno IsNot Nothing, f.Turno.Nombre, "-")} / {If(f.Horario IsNot Nothing, f.Horario.Nombre, "-")}"

''            If f.Activo Then
''                lblEstadoActividad.Text = "Estado: Activo"
''                lblEstadoActividad.ForeColor = Color.DarkGreen
''            Else
''                lblEstadoActividad.Text = "Estado: Inactivo"
''                lblEstadoActividad.ForeColor = Color.Maroon
''            End If

''            Dim situaciones = Await uow.Context.Database.SqlQuery(Of SituacionParaBoton)(
''            "SELECT Prioridad, Tipo, ColorIndicador FROM dbo.vw_FuncionarioSituacionActual WHERE FuncionarioId = @p0 ORDER BY Prioridad",
''            id
''        ).ToListAsync()

''            If situaciones IsNot Nothing AndAlso situaciones.Any() Then
''                btnVerSituacion.Visible = True
''                Dim primeraSituacion = situaciones.First()

''                If situaciones.Count > 1 Then
''                    btnVerSituacion.Text = "Situación Múltiple"
''                Else
''                    btnVerSituacion.Text = primeraSituacion.Tipo
''                End If

''                Try
''                    btnVerSituacion.BackColor = Color.FromName(primeraSituacion.ColorIndicador)
''                    btnVerSituacion.ForeColor = Color.White
''                Catch ex As Exception
''                    btnVerSituacion.BackColor = SystemColors.Control
''                    btnVerSituacion.ForeColor = SystemColors.ControlText
''                End Try
''            Else
''                btnVerSituacion.Visible = False
''            End If

''            lblPresencia.Text = Await ObtenerPresenciaAsync(id, Date.Today)
''            If Not f.Activo AndAlso (situaciones Is Nothing OrElse Not situaciones.Any()) Then
''                lblPresencia.Text = Await ObtenerPresenciaAsync(id, Date.Today)
''            End If

''            If f.Foto Is Nothing OrElse f.Foto.Length = 0 Then
''                pbFotoDetalle.Image = My.Resources.Police
''            Else
''                Using ms As New MemoryStream(f.Foto)
''                    pbFotoDetalle.Image = Image.FromStream(ms)
''                End Using
''            End If
''        End Using
''    End Sub

''    Private Sub btnVerSituacion_Click(sender As Object, e As EventArgs)
''        If dgvResultados.CurrentRow Is Nothing Then Return
''        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)
''        Dim frm As New frmFuncionarioSituacion(id)
''        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)

''        parentDashboard.AbrirFormEnPanel(frm)
''    End Sub

''    Private Class SituacionParaBoton
''        Public Property Prioridad As Integer
''        Public Property Tipo As String
''        Public Property ColorIndicador As String
''    End Class

''    Private Sub OnDgvDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
''        If dgvResultados.CurrentRow Is Nothing Then Return

''        If _modo = ModoApertura.Seleccion Then
''            SeleccionarYcerrar()
''        Else ' Modo Navegacion
''            Dim id As Integer = CInt(dgvResultados.CurrentRow.Cells("Id").Value)
''            Dim frm As New frmFuncionarioCrear(id)
''            Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
''            If parentDashboard IsNot Nothing Then
''                parentDashboard.AbrirFormEnPanel(frm)
''            End If
''        End If
''    End Sub

''    Private Async Function ObtenerPresenciaAsync(id As Integer, fecha As Date) As Task(Of String)
''        Using uow As New UnitOfWork()
''            Dim ctx = uow.Context
''            Dim pFecha = New SqlParameter("@Fecha", fecha.Date)
''            Dim lista = Await ctx.Database.SqlQuery(Of PresenciaDTO)(
''                "EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha
''            ).ToListAsync()
''            Dim presencia = lista.Where(Function(r) r.FuncionarioId = id).
''                         Select(Function(r) r.Resultado).
''                         FirstOrDefault()
''            Return If(presencia, "-")
''        End Using
''    End Function

''    Private Sub LimpiarDetalle()
''        lblCI.Text = ""
''        lblNombreCompleto.Text = ""
''        lblCargo.Text = ""
''        lblTipo.Text = ""
''        lblFechaIngreso.Text = ""
''        lblEstadoActividad.Text = ""
''        lblPresencia.Text = ""
''        pbFotoDetalle.Image = Nothing
''        lblHorarioCompleto.Text = ""
''        _detallesEstadoActual.Clear()
''        btnGenerarFicha.Visible = False
''        btnVerSituacion.Visible = False
''    End Sub

''    Private Sub lblEstadoTransitorio_DoubleClick(sender As Object, e As EventArgs)
''        If _detallesEstadoActual IsNot Nothing AndAlso _detallesEstadoActual.Any() Then
''            Dim detalleTexto = String.Join(Environment.NewLine, _detallesEstadoActual.Distinct())
''            MessageBox.Show(detalleTexto, "Detalle del Estado Actual", MessageBoxButtons.OK, MessageBoxIcon.Information)
''        End If
''    End Sub

''#End Region

''#Region "Lógica de Selección"

''    Private Sub SeleccionarYcerrar()
''        If FuncionarioSeleccionado IsNot Nothing Then
''            Me.DialogResult = DialogResult.OK
''            Me.Close()
''        Else
''            MessageBox.Show("Por favor, seleccione un funcionario de la lista.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information)
''        End If
''    End Sub

''    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
''        SeleccionarYcerrar()
''    End Sub

''    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
''        Me.DialogResult = DialogResult.Cancel
''        Me.Close()
''    End Sub
''    Private Sub btnGenerarFicha_Click(sender As Object, e As EventArgs)
''        If FuncionarioSeleccionado IsNot Nothing Then
''            Dim frm As New frmFichaPersonalRPT(FuncionarioSeleccionado.Id)
''            Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
''            parentDashboard.AbrirFormEnPanel(frm)
''        Else
''            MessageBox.Show("Por favor, seleccione un funcionario de la lista.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information)
''        End If
''    End Sub
''#End Region


''#Region "DTO ligero"
''    Public Class FuncionarioMin
''        Public Property Id As Integer
''        Public Property CI As String
''        Public Property Nombre As String
''    End Class
''    Public Class PresenciaDTO
''        Public Property FuncionarioId As Integer
''        Public Property Resultado As String
''    End Class


''#End Region

''End Class
'Option Strict On
'Option Explicit On

'Imports System.Data.Entity
'Imports System.Data.SqlClient
'Imports System.Drawing
'Imports System.IO
'Imports System.Text
'Imports System.Threading
'Imports System.Windows.Forms

'Public Class frmFuncionarioBuscar
'    Inherits Form


'    Public Enum ModoApertura
'        Navegacion ' Para ver y editar desde el Dashboard
'        Seleccion  ' Para seleccionar un funcionario y devolverlo
'    End Enum

'    Private ReadOnly _modo As ModoApertura
'    Private Const LIMITE_FILAS As Integer = 500
'    Private _detallesEstadoActual As New List(Of String)

'    ' Temporizador para la búsqueda automática.
'    ' Se especifica el namespace completo para evitar la ambigüedad.
'    Private WithEvents SearchTimer As New System.Windows.Forms.Timer()

'    Public ReadOnly Property FuncionarioSeleccionado As FuncionarioMin

'        Get
'            If dgvResultados.CurrentRow IsNot Nothing Then
'                Return CType(dgvResultados.CurrentRow.DataBoundItem, FuncionarioMin)
'            End If
'            Return Nothing
'        End Get
'    End Property

'    Public Sub New()
'        InitializeComponent()

'        _modo = ModoApertura.Navegacion
'        FlowLayoutPanelAcciones.Visible = False
'    End Sub

'    Public Sub New(modo As ModoApertura)
'        Me.New()
'        _modo = modo
'        FlowLayoutPanelAcciones.Visible = (_modo = ModoApertura.Seleccion)
'    End Sub

'    Private Sub frmFuncionarioBuscar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        ' Este botón indica estado prioritario: que no cambie de color al pasar el mouse
'        btnVerSituacion.Tag = "KeepBackColor"

'        AppTheme.Aplicar(Me)
'        ConfigurarGrilla()
'        AddHandler btnVerSituacion.Click, AddressOf btnVerSituacion_Click
'        AddHandler btnGenerarFicha.Click, AddressOf btnGenerarFicha_Click
'        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados

'        ' Configurar el temporizador de búsqueda
'        SearchTimer.Interval = 500 ' 500ms de espera antes de buscar
'        AddHandler SearchTimer.Tick, AddressOf SearchTimer_Tick
'        AddHandler txtBusqueda.TextChanged, AddressOf txtBusqueda_TextChanged
'    End Sub

'    Private Sub txtBusqueda_TextChanged(sender As Object, e As EventArgs)
'        ' Reiniciar el temporizador cada vez que el texto cambia
'        SearchTimer.Stop()
'        SearchTimer.Start()
'    End Sub

'    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs)
'        ' Cuando el temporizador se cumple, detenerlo y ejecutar la búsqueda
'        SearchTimer.Stop()
'        Await BuscarAsync()
'    End Sub

'    ''' <summary>
'    ''' Este método se ejecutará automáticamente cuando otro formulario notifique un cambio.
'    ''' </summary>
'    Private Async Sub OnDatosActualizados(sender As Object, e As EventArgs)

'        ' El formulario se actualizará incluso si está oculto, asegurando que los datos
'        ' estén frescos cuando el usuario vuelva a esta pantalla.
'        If Me.IsHandleCreated Then
'            Await BuscarAsync()
'        End If

'    End Sub

'    ' Es una buena práctica desuscribirse para evitar fugas de memoria.
'    Private Sub frmFuncionarioBuscar_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
'        RemoveHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
'    End Sub

'#Region "Diseño de grilla"
'    Private Sub ConfigurarGrilla()
'        With dgvResultados
'            .AutoGenerateColumns = False
'            .RowTemplate.Height = 40
'            .RowTemplate.MinimumHeight = 40
'            .Columns.Clear()


'            .Columns.Add(New DataGridViewTextBoxColumn With {
'                .Name = "Id",
'                .DataPropertyName = "Id",
'                .Visible = False
'            })

'            .Columns.Add(New DataGridViewTextBoxColumn With {
'                .Name = "CI",
'                .DataPropertyName = "CI",
'                .HeaderText = "CI",
'                .Width = 90
'            })

'            .Columns.Add(New DataGridViewTextBoxColumn With {
'                .Name = "Nombre",
'                .DataPropertyName = "Nombre",
'                .HeaderText = "Nombre",
'                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
'            })
'        End With

'        ' --- CAMBIO REALIZADO AQUÍ ---
'        ' Cambiamos el evento para que la actualización con las teclas sea robusta.
'        AddHandler dgvResultados.CurrentCellChanged, AddressOf MostrarDetalle
'        ' --- FIN DEL CAMBIO ---

'        AddHandler dgvResultados.CellDoubleClick, AddressOf OnDgvDoubleClick
'        AddHandler dgvResultados.DataError, Sub(s, ev) ev.ThrowException = False
'    End Sub
'#End Region

'#Region "Búsqueda con Full-Text y CONTAINS"
'    Private Async Function BuscarAsync() As Task
'        ' Si no hay texto en la caja de búsqueda, limpiamos la grilla y el detalle.
'        If String.IsNullOrWhiteSpace(txtBusqueda.Text) Then
'            dgvResultados.DataSource = New List(Of FuncionarioMin)()
'            LimpiarDetalle()
'            Return
'        End If

'        LoadingHelper.MostrarCargando(Me)

'        Try
'            Using uow As New UnitOfWork()
'                Dim ctx = uow.Context
'                Dim filtro As String = txtBusqueda.Text.Trim()

'                Dim terminos = filtro.Split(" "c) _
'                                        .Where(Function(w) Not String.IsNullOrWhiteSpace(w)) _
'                                        .Select(Function(w) $"""{w}*""")
'                Dim expresionFts = String.Join(" AND ", terminos)

'                Dim sb As New StringBuilder()
'                sb.AppendLine("SELECT TOP (@limite)")
'                sb.AppendLine("      Id, CI, Nombre")
'                sb.AppendLine("FROM   dbo.Funcionario WITH (NOLOCK)")
'                sb.AppendLine("WHERE  CONTAINS((CI, Nombre), @patron)")
'                sb.AppendLine("ORDER BY Nombre;")

'                Dim sql = sb.ToString()
'                Dim pLimite = New SqlParameter("@limite", LIMITE_FILAS)
'                Dim pPatron = New SqlParameter("@patron", expresionFts)

'                Dim lista = Await ctx.Database _
'                                     .SqlQuery(Of FuncionarioMin)(sql, pLimite, pPatron) _
'                                     .ToListAsync()

'                dgvResultados.DataSource = Nothing
'                dgvResultados.DataSource = lista

'                If lista.Any() Then
'                    dgvResultados.ClearSelection()
'                    dgvResultados.Rows(0).Selected = True
'                    dgvResultados.CurrentCell = dgvResultados.Rows(0).Cells("CI")
'                    btnGenerarFicha.Visible = True
'                Else
'                    LimpiarDetalle()
'                End If

'                If lista.Count = LIMITE_FILAS Then
'                    MessageBox.Show($"Mostrando los primeros {LIMITE_FILAS} resultados." &
'                                    "Refiná la búsqueda para ver más.",
'                                    "Aviso",
'                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
'                End If
'            End Using

'        Catch ex As SqlException When ex.Number = -2
'            MessageBox.Show("La consulta excedió el tiempo de espera. Refiná los filtros o intentá nuevamente.",
'                    "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning)

'        Catch ex As Exception
'            MessageBox.Show("Ocurrió un error inesperado: " & ex.Message,
'                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

'        Finally
'            LoadingHelper.OcultarCargando(Me)
'        End Try
'    End Function

'    ' --- MÉTODO CORREGIDO: Se quita "Async" ---
'    Private Sub txtBusqueda_KeyDown(sender As Object, e As KeyEventArgs) _
'    Handles txtBusqueda.KeyDown

'        If e.KeyCode = Keys.Enter Then
'            e.Handled = True
'            e.SuppressKeyPress = True
'        End If

'        If dgvResultados.Rows.Count = 0 Then Return

'        Select Case e.KeyCode
'            Case Keys.Down : MoverSeleccion(+1) : e.Handled = True
'            Case Keys.Up : MoverSeleccion(-1) : e.Handled = True
'        End Select
'    End Sub

'    Private Sub MoverSeleccion(direccion As Integer)
'        Dim total = dgvResultados.Rows.Count
'        If total = 0 Then
'            LimpiarDetalle()
'            Exit Sub
'        End If

'        Dim indexActual As Integer =
'        If(dgvResultados.CurrentRow Is Nothing, -1, dgvResultados.CurrentRow.Index)

'        Dim nuevoIndex = Math.Max(0, Math.Min(total - 1, indexActual + direccion))

'        dgvResultados.ClearSelection()
'        dgvResultados.Rows(nuevoIndex).Selected = True
'        dgvResultados.CurrentCell = dgvResultados.Rows(nuevoIndex).Cells("CI")
'    End Sub
'#End Region


'#Region "Detalle lateral (Foto on-demand)"
'    Private Async Sub MostrarDetalle(sender As Object, e As EventArgs)
'        If dgvResultados.CurrentRow Is Nothing OrElse dgvResultados.CurrentRow.DataBoundItem Is Nothing Then
'            LimpiarDetalle()
'            Return
'        End If
'        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)

'        Using uow As New UnitOfWork()
'            Dim f = Await uow.Repository(Of Funcionario)() _
'             .GetAll() _
'             .Include(Function(x) x.Cargo) _
'             .Include(Function(x) x.TipoFuncionario) _
'             .Include(Function(x) x.Semana) _
'             .Include(Function(x) x.Turno) _
'             .Include(Function(x) x.Horario) _
'             .AsNoTracking() _
'             .FirstOrDefaultAsync(Function(x) x.Id = id)

'            If dgvResultados.CurrentRow Is Nothing OrElse CInt(dgvResultados.CurrentRow.Cells("Id").Value) <> id Then Return
'            If f Is Nothing Then Return

'            lblCI.Text = f.CI
'            lblNombreCompleto.Text = f.Nombre
'            lblCargo.Text = If(f.Cargo Is Nothing, "-", f.Cargo.Nombre)
'            lblTipo.Text = f.TipoFuncionario.Nombre
'            lblFechaIngreso.Text = f.FechaIngreso.ToShortDateString()
'            lblHorarioCompleto.Text = $"{If(f.Semana IsNot Nothing, f.Semana.Nombre, "-")} / {If(f.Turno IsNot Nothing, f.Turno.Nombre, "-")} / {If(f.Horario IsNot Nothing, f.Horario.Nombre, "-")}"

'            If f.Activo Then
'                lblEstadoActividad.Text = "Estado: Activo"
'                lblEstadoActividad.ForeColor = Color.DarkGreen
'            Else
'                lblEstadoActividad.Text = "Estado: Inactivo"
'                lblEstadoActividad.ForeColor = Color.Maroon
'            End If

'            Dim situaciones = Await uow.Context.Database.SqlQuery(Of SituacionParaBoton)(
'            "SELECT Prioridad, Tipo, ColorIndicador FROM dbo.vw_FuncionarioSituacionActual WHERE FuncionarioId = @p0 ORDER BY Prioridad",
'            id
'        ).ToListAsync()

'            If situaciones IsNot Nothing AndAlso situaciones.Any() Then
'                btnVerSituacion.Visible = True
'                Dim primeraSituacion = situaciones.First()

'                If situaciones.Count > 1 Then
'                    btnVerSituacion.Text = "Situación Múltiple"
'                Else
'                    btnVerSituacion.Text = primeraSituacion.Tipo
'                End If

'                Try
'                    btnVerSituacion.BackColor = Color.FromName(primeraSituacion.ColorIndicador)
'                    btnVerSituacion.ForeColor = Color.White
'                Catch ex As Exception
'                    btnVerSituacion.BackColor = SystemColors.Control
'                    btnVerSituacion.ForeColor = SystemColors.ControlText
'                End Try
'            Else
'                btnVerSituacion.Visible = False
'            End If

'            lblPresencia.Text = Await ObtenerPresenciaAsync(id, Date.Today)
'            If Not f.Activo AndAlso (situaciones Is Nothing OrElse Not situaciones.Any()) Then
'                lblPresencia.Text = Await ObtenerPresenciaAsync(id, Date.Today)
'            End If

'            If f.Foto Is Nothing OrElse f.Foto.Length = 0 Then
'                pbFotoDetalle.Image = My.Resources.Police
'            Else
'                Using ms As New MemoryStream(f.Foto)
'                    pbFotoDetalle.Image = Image.FromStream(ms)
'                End Using
'            End If
'        End Using
'    End Sub

'    Private Sub btnVerSituacion_Click(sender As Object, e As EventArgs)
'        If dgvResultados.CurrentRow Is Nothing Then Return
'        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)
'        Dim frm As New frmFuncionarioSituacion(id)
'        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)

'        parentDashboard.AbrirFormEnPanel(frm)
'    End Sub

'    Private Class SituacionParaBoton
'        Public Property Prioridad As Integer
'        Public Property Tipo As String
'        Public Property ColorIndicador As String
'    End Class

'    Private Sub OnDgvDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
'        If dgvResultados.CurrentRow Is Nothing Then Return

'        If _modo = ModoApertura.Seleccion Then
'            SeleccionarYcerrar()
'        Else ' Modo Navegacion
'            Dim id As Integer = CInt(dgvResultados.CurrentRow.Cells("Id").Value)
'            Dim frm As New frmFuncionarioCrear(id)
'            Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
'            If parentDashboard IsNot Nothing Then
'                parentDashboard.AbrirFormEnPanel(frm)
'            End If
'        End If
'    End Sub

'    Private Async Function ObtenerPresenciaAsync(id As Integer, fecha As Date) As Task(Of String)
'        Using uow As New UnitOfWork()
'            Dim ctx = uow.Context
'            Dim pFecha = New SqlParameter("@Fecha", fecha.Date)
'            Dim lista = Await ctx.Database.SqlQuery(Of PresenciaDTO)(
'                "EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha
'            ).ToListAsync()
'            Dim presencia = lista.Where(Function(r) r.FuncionarioId = id).
'                              Select(Function(r) r.Resultado).
'                              FirstOrDefault()
'            Return If(presencia, "-")
'        End Using
'    End Function

'    Private Sub LimpiarDetalle()
'        lblCI.Text = ""
'        lblNombreCompleto.Text = ""
'        lblCargo.Text = ""
'        lblTipo.Text = ""
'        lblFechaIngreso.Text = ""
'        lblEstadoActividad.Text = ""
'        lblPresencia.Text = ""
'        pbFotoDetalle.Image = Nothing
'        lblHorarioCompleto.Text = ""
'        _detallesEstadoActual.Clear()
'        btnGenerarFicha.Visible = False
'        btnVerSituacion.Visible = False
'    End Sub

'    Private Sub lblEstadoTransitorio_DoubleClick(sender As Object, e As EventArgs)
'        If _detallesEstadoActual IsNot Nothing AndAlso _detallesEstadoActual.Any() Then
'            Dim detalleTexto = String.Join(Environment.NewLine, _detallesEstadoActual.Distinct())
'            MessageBox.Show(detalleTexto, "Detalle del Estado Actual", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        End If
'    End Sub

'#End Region

'#Region "Lógica de Selección"

'    Private Sub SeleccionarYcerrar()
'        If FuncionarioSeleccionado IsNot Nothing Then
'            Me.DialogResult = DialogResult.OK
'            Me.Close()
'        Else
'            MessageBox.Show("Por favor, seleccione un funcionario de la lista.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        End If
'    End Sub

'    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
'        SeleccionarYcerrar()
'    End Sub

'    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
'        Me.DialogResult = DialogResult.Cancel
'        Me.Close()
'    End Sub
'    Private Sub btnGenerarFicha_Click(sender As Object, e As EventArgs)
'        If FuncionarioSeleccionado IsNot Nothing Then
'            Dim frm As New frmFichaPersonalRPT(FuncionarioSeleccionado.Id)
'            Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
'            parentDashboard.AbrirFormEnPanel(frm)
'        Else
'            MessageBox.Show("Por favor, seleccione un funcionario de la lista.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information)
'        End If
'    End Sub
'#End Region

'#Region "Copiar al portapapeles"
'    Private Sub tsmiCopiarNombre_Click(sender As Object, e As EventArgs) Handles tsmiCopiarNombre.Click
'        ' Copia el nombre del funcionario seleccionado al portapapeles.
'        If dgvResultados.CurrentRow IsNot Nothing Then
'            Dim nombre As String = dgvResultados.CurrentRow.Cells("Nombre").Value?.ToString()
'            If Not String.IsNullOrEmpty(nombre) Then
'                My.Computer.Clipboard.SetText(nombre)
'            End If
'        End If
'    End Sub

'    Private Sub tsmiCopiarCedula_Click(sender As Object, e As EventArgs) Handles tsmiCopiarCedula.Click
'        ' Copia la cédula del funcionario seleccionado al portapapeles.
'        If dgvResultados.CurrentRow IsNot Nothing Then
'            Dim cedula As String = dgvResultados.CurrentRow.Cells("CI").Value?.ToString()
'            If Not String.IsNullOrEmpty(cedula) Then
'                My.Computer.Clipboard.SetText(cedula)
'            End If
'        End If
'    End Sub

'    Private Sub dgvResultados_MouseDown(sender As Object, e As MouseEventArgs) Handles dgvResultados.MouseDown
'        ' Selecciona la fila con el clic derecho para que el menú contextual se aplique a la fila correcta.
'        If e.Button = MouseButtons.Right Then
'            Dim hitTestInfo As DataGridView.HitTestInfo = dgvResultados.HitTest(e.X, e.Y)
'            If hitTestInfo.RowIndex >= 0 Then
'                dgvResultados.ClearSelection()
'                dgvResultados.Rows(hitTestInfo.RowIndex).Selected = True
'                dgvResultados.CurrentCell = dgvResultados.Rows(hitTestInfo.RowIndex).Cells("CI")
'            End If
'        End If
'    End Sub
'#End Region


'#Region "DTO ligero"
'    Public Class FuncionarioMin
'        Public Property Id As Integer
'        Public Property CI As String
'        Public Property Nombre As String
'    End Class
'    Public Class PresenciaDTO
'        Public Property FuncionarioId As Integer
'        Public Property Resultado As String
'    End Class


'#End Region

'End Class
'Option Strict On
'Option Explicit On

'Imports System.Data.Entity
'Imports System.Data.SqlClient
'Imports System.Drawing
'Imports System.IO
'Imports System.Text
'Imports System.Threading
'Imports System.Windows.Forms

Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text

Public Class frmFuncionarioBuscar
    Inherits Form


    Public Enum ModoApertura
        Navegacion ' Para ver y editar desde el Dashboard
        Seleccion  ' Para seleccionar un funcionario y devolverlo
    End Enum

    Private ReadOnly _modo As ModoApertura
    Private Const LIMITE_FILAS As Integer = 500
    Private _detallesEstadoActual As New List(Of String)

    ' Temporizador para la búsqueda automática.
    ' Se especifica el namespace completo para evitar la ambigüedad.
    Private WithEvents SearchTimer As New System.Windows.Forms.Timer()

    Public ReadOnly Property FuncionarioSeleccionado As FuncionarioMin

        Get
            If dgvResultados.CurrentRow IsNot Nothing Then
                Return CType(dgvResultados.CurrentRow.DataBoundItem, FuncionarioMin)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub New()
        InitializeComponent()

        _modo = ModoApertura.Navegacion
        FlowLayoutPanelAcciones.Visible = False
    End Sub

    Public Sub New(modo As ModoApertura)
        Me.New()
        _modo = modo
        FlowLayoutPanelAcciones.Visible = (_modo = ModoApertura.Seleccion)
    End Sub

    Private Sub frmFuncionarioBuscar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Este botón indica estado prioritario: que no cambie de color al pasar el mouse
        btnVerSituacion.Tag = "KeepBackColor"

        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        AddHandler btnVerSituacion.Click, AddressOf btnVerSituacion_Click
        AddHandler btnGenerarFicha.Click, AddressOf btnGenerarFicha_Click
        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados

        ' Configurar el temporizador de búsqueda
        SearchTimer.Interval = 500 ' 500ms de espera antes de buscar
        AddHandler SearchTimer.Tick, AddressOf SearchTimer_Tick
        AddHandler txtBusqueda.TextChanged, AddressOf txtBusqueda_TextChanged
    End Sub

    Private Sub txtBusqueda_TextChanged(sender As Object, e As EventArgs)
        ' Reiniciar el temporizador cada vez que el texto cambia
        SearchTimer.Stop()
        SearchTimer.Start()
    End Sub

    Private Async Sub SearchTimer_Tick(sender As Object, e As EventArgs)
        ' Cuando el temporizador se cumple, detenerlo y ejecutar la búsqueda
        SearchTimer.Stop()
        Await BuscarAsync()
    End Sub

    ''' <summary>
    ''' Este método se ejecutará automáticamente cuando otro formulario notifique un cambio.
    ''' </summary>
    Private Async Sub OnDatosActualizados(sender As Object, e As EventArgs)

        ' El formulario se actualizará incluso si está oculto, asegurando que los datos
        ' estén frescos cuando el usuario vuelva a esta pantalla.
        If Me.IsHandleCreated Then
            Await BuscarAsync()
        End If

    End Sub

    ' Es una buena práctica desuscribirse para evitar fugas de memoria.
    Private Sub frmFuncionarioBuscar_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados
    End Sub

#Region "Diseño de grilla"
    Private Sub ConfigurarGrilla()
        With dgvResultados
            .AutoGenerateColumns = False
            .RowTemplate.Height = 40
            .RowTemplate.MinimumHeight = 40
            .Columns.Clear()


            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Id",
                .DataPropertyName = "Id",
                .Visible = False
            })

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "CI",
                .DataPropertyName = "CI",
                .HeaderText = "CI",
                .Width = 90
            })

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Nombre",
                .DataPropertyName = "Nombre",
                .HeaderText = "Nombre",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            })
        End With

        ' --- CAMBIO REALIZADO AQUÍ ---
        ' Cambiamos el evento para que la actualización con las teclas sea robusta.
        AddHandler dgvResultados.CurrentCellChanged, AddressOf MostrarDetalle
        ' --- FIN DEL CAMBIO ---

        AddHandler dgvResultados.CellDoubleClick, AddressOf OnDgvDoubleClick
        AddHandler dgvResultados.DataError, Sub(s, ev) ev.ThrowException = False
    End Sub
#End Region

#Region "Búsqueda con Full-Text y CONTAINS"
    Private Async Function BuscarAsync() As Task
        ' Si no hay texto en la caja de búsqueda, limpiamos la grilla y el detalle.
        If String.IsNullOrWhiteSpace(txtBusqueda.Text) Then
            dgvResultados.DataSource = New List(Of FuncionarioMin)()
            LimpiarDetalle()
            Return
        End If

        LoadingHelper.MostrarCargando(Me)

        Try
            Using uow As New UnitOfWork()
                Dim ctx = uow.Context
                Dim filtro As String = txtBusqueda.Text.Trim()

                Dim terminos = filtro.Split(" "c) _
                                     .Where(Function(w) Not String.IsNullOrWhiteSpace(w)) _
                                     .Select(Function(w) $"""{w}*""")
                Dim expresionFts = String.Join(" AND ", terminos)

                Dim sb As New StringBuilder()
                sb.AppendLine("SELECT TOP (@limite)")
                sb.AppendLine("      Id, CI, Nombre")
                sb.AppendLine("FROM   dbo.Funcionario WITH (NOLOCK)")
                sb.AppendLine("WHERE  CONTAINS((CI, Nombre), @patron)")
                sb.AppendLine("ORDER BY Nombre;")

                Dim sql = sb.ToString()
                Dim pLimite = New SqlParameter("@limite", LIMITE_FILAS)
                Dim pPatron = New SqlParameter("@patron", expresionFts)

                Dim lista = Await ctx.Database _
                                     .SqlQuery(Of FuncionarioMin)(sql, pLimite, pPatron) _
                                     .ToListAsync()

                dgvResultados.DataSource = Nothing
                dgvResultados.DataSource = lista

                If lista.Any() Then
                    dgvResultados.ClearSelection()
                    dgvResultados.Rows(0).Selected = True
                    dgvResultados.CurrentCell = dgvResultados.Rows(0).Cells("CI")
                    btnGenerarFicha.Visible = True
                Else
                    LimpiarDetalle()
                End If

                If lista.Count = LIMITE_FILAS Then
                    MessageBox.Show($"Mostrando los primeros {LIMITE_FILAS} resultados." &
                                    "Refiná la búsqueda para ver más.",
                                    "Aviso",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using

        Catch ex As SqlException When ex.Number = -2
            MessageBox.Show("La consulta excedió el tiempo de espera. Refiná los filtros o intentá nuevamente.",
                    "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        Catch ex As Exception
            MessageBox.Show("Ocurrió un error inesperado: " & ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    ' --- MÉTODO CORREGIDO: Se quita "Async" ---
    Private Sub txtBusqueda_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles txtBusqueda.KeyDown

        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
        End If

        If dgvResultados.Rows.Count = 0 Then Return

        Select Case e.KeyCode
            Case Keys.Down : MoverSeleccion(+1) : e.Handled = True
            Case Keys.Up : MoverSeleccion(-1) : e.Handled = True
        End Select
    End Sub

    Private Sub MoverSeleccion(direccion As Integer)
        Dim total = dgvResultados.Rows.Count
        If total = 0 Then
            LimpiarDetalle()
            Exit Sub
        End If

        Dim indexActual As Integer =
        If(dgvResultados.CurrentRow Is Nothing, -1, dgvResultados.CurrentRow.Index)

        Dim nuevoIndex = Math.Max(0, Math.Min(total - 1, indexActual + direccion))

        dgvResultados.ClearSelection()
        dgvResultados.Rows(nuevoIndex).Selected = True
        dgvResultados.CurrentCell = dgvResultados.Rows(nuevoIndex).Cells("CI")
    End Sub
#End Region


#Region "Detalle lateral (Foto on-demand)"
    Private Async Sub MostrarDetalle(sender As Object, e As EventArgs)
        If dgvResultados.CurrentRow Is Nothing OrElse dgvResultados.CurrentRow.DataBoundItem Is Nothing Then
            LimpiarDetalle()
            Return
        End If
        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)

        Using uow As New UnitOfWork()
            Dim f = Await uow.Repository(Of Funcionario)() _
             .GetAll() _
             .Include(Function(x) x.Cargo) _
             .Include(Function(x) x.TipoFuncionario) _
             .Include(Function(x) x.Semana) _
             .Include(Function(x) x.Turno) _
             .Include(Function(x) x.Horario) _
             .AsNoTracking() _
             .FirstOrDefaultAsync(Function(x) x.Id = id)

            If dgvResultados.CurrentRow Is Nothing OrElse CInt(dgvResultados.CurrentRow.Cells("Id").Value) <> id Then Return
            If f Is Nothing Then Return

            lblCI.Text = f.CI
            lblNombreCompleto.Text = f.Nombre
            lblCargo.Text = If(f.Cargo Is Nothing, "-", f.Cargo.Nombre)
            lblTipo.Text = f.TipoFuncionario.Nombre
            lblFechaIngreso.Text = f.FechaIngreso.ToShortDateString()
            lblHorarioCompleto.Text = $"{If(f.Semana IsNot Nothing, f.Semana.Nombre, "-")} / {If(f.Turno IsNot Nothing, f.Turno.Nombre, "-")} / {If(f.Horario IsNot Nothing, f.Horario.Nombre, "-")}"

            ' --- INICIO DE CAMBIOS ---
            ' Hacemos visibles los íconos de copiar
            pbCopyCI.Visible = True
            pbCopyNombre.Visible = True
            ' --- FIN DE CAMBIOS ---

            If f.Activo Then
                lblEstadoActividad.Text = "Estado: Activo"
                lblEstadoActividad.ForeColor = Color.DarkGreen
            Else
                lblEstadoActividad.Text = "Estado: Inactivo"
                lblEstadoActividad.ForeColor = Color.Maroon
            End If

            Dim situaciones = Await uow.Context.Database.SqlQuery(Of SituacionParaBoton)(
            "SELECT Prioridad, Tipo, ColorIndicador FROM dbo.vw_FuncionarioSituacionActual WHERE FuncionarioId = @p0 ORDER BY Prioridad",
            id
            ).ToListAsync()

            If situaciones IsNot Nothing AndAlso situaciones.Any() Then
                btnVerSituacion.Visible = True
                Dim primeraSituacion = situaciones.First()

                If situaciones.Count > 1 Then
                    btnVerSituacion.Text = "Situación Múltiple"
                Else
                    btnVerSituacion.Text = primeraSituacion.Tipo
                End If

                Try
                    btnVerSituacion.BackColor = Color.FromName(primeraSituacion.ColorIndicador)
                    btnVerSituacion.ForeColor = Color.White
                Catch ex As Exception
                    btnVerSituacion.BackColor = SystemColors.Control
                    btnVerSituacion.ForeColor = SystemColors.ControlText
                End Try
            Else
                btnVerSituacion.Visible = False
            End If

            lblPresencia.Text = Await ObtenerPresenciaAsync(id, Date.Today)
            If Not f.Activo AndAlso (situaciones Is Nothing OrElse Not situaciones.Any()) Then
                lblPresencia.Text = Await ObtenerPresenciaAsync(id, Date.Today)
            End If

            If f.Foto Is Nothing OrElse f.Foto.Length = 0 Then
                pbFotoDetalle.Image = My.Resources.Police
            Else
                Using ms As New MemoryStream(f.Foto)
                    pbFotoDetalle.Image = Image.FromStream(ms)
                End Using
            End If
        End Using
    End Sub

    Private Sub btnVerSituacion_Click(sender As Object, e As EventArgs)
        If dgvResultados.CurrentRow Is Nothing Then Return
        Dim id = CInt(dgvResultados.CurrentRow.Cells("Id").Value)
        Dim frm As New frmFuncionarioSituacion(id)
        Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)

        parentDashboard.AbrirFormEnPanel(frm)
    End Sub

    Private Class SituacionParaBoton
        Public Property Prioridad As Integer
        Public Property Tipo As String
        Public Property ColorIndicador As String
    End Class

    Private Sub OnDgvDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If dgvResultados.CurrentRow Is Nothing Then Return

        If _modo = ModoApertura.Seleccion Then
            SeleccionarYcerrar()
        Else ' Modo Navegacion
            Dim id As Integer = CInt(dgvResultados.CurrentRow.Cells("Id").Value)
            Dim frm As New frmFuncionarioCrear(id)
            Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
            If parentDashboard IsNot Nothing Then
                parentDashboard.AbrirFormEnPanel(frm)
            End If
        End If
    End Sub

    Private Async Function ObtenerPresenciaAsync(id As Integer, fecha As Date) As Task(Of String)
        Using uow As New UnitOfWork()
            Dim ctx = uow.Context
            Dim pFecha = New SqlParameter("@Fecha", fecha.Date)
            Dim lista = Await ctx.Database.SqlQuery(Of PresenciaDTO)(
                "EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha
            ).ToListAsync()
            Dim presencia = lista.Where(Function(r) r.FuncionarioId = id).
                                  Select(Function(r) r.Resultado).
                                  FirstOrDefault()
            Return If(presencia, "-")
        End Using
    End Function

    Private Sub LimpiarDetalle()
        lblCI.Text = ""
        lblNombreCompleto.Text = ""
        lblCargo.Text = ""
        lblTipo.Text = ""
        lblFechaIngreso.Text = ""
        lblEstadoActividad.Text = ""
        lblPresencia.Text = ""
        pbFotoDetalle.Image = Nothing
        lblHorarioCompleto.Text = ""
        _detallesEstadoActual.Clear()
        btnGenerarFicha.Visible = False
        btnVerSituacion.Visible = False

        ' --- INICIO DE CAMBIOS ---
        ' Ocultamos los íconos de copiar
        pbCopyCI.Visible = False
        pbCopyNombre.Visible = False
        ' --- FIN DE CAMBIOS ---
    End Sub

    Private Sub lblEstadoTransitorio_DoubleClick(sender As Object, e As EventArgs)
        If _detallesEstadoActual IsNot Nothing AndAlso _detallesEstadoActual.Any() Then
            Dim detalleTexto = String.Join(Environment.NewLine, _detallesEstadoActual.Distinct())
            MessageBox.Show(detalleTexto, "Detalle del Estado Actual", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

#End Region

#Region "Lógica de Selección"

    Private Sub SeleccionarYcerrar()
        If FuncionarioSeleccionado IsNot Nothing Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show("Por favor, seleccione un funcionario de la lista.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        SeleccionarYcerrar()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub btnGenerarFicha_Click(sender As Object, e As EventArgs)
        If FuncionarioSeleccionado IsNot Nothing Then
            Dim frm As New frmFichaPersonalRPT(FuncionarioSeleccionado.Id)
            Dim parentDashboard As frmDashboard = CType(Me.ParentForm, frmDashboard)
            parentDashboard.AbrirFormEnPanel(frm)
        Else
            MessageBox.Show("Por favor, seleccione un funcionario de la lista.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
#End Region

    ' --- INICIO DE NUEVA REGIÓN ---
#Region "Copiar al portapapeles"
    ''' <summary>
    ''' Cambia el color de fondo del ícono a verde por 1 segundo para notificar al usuario.
    ''' </summary>
    Private Async Function MostrarEfectoDeCopiaAsync(pb As PictureBox) As Task
        ' 1. Deshabilitamos el control para evitar múltiples clics.
        pb.Enabled = False

        ' 2. Guardamos el color de fondo original y lo cambiamos a un color de confirmación.
        Dim colorOriginal = pb.BackColor
        pb.BackColor = Color.PaleGreen ' Un verde suave es ideal para confirmación.

        ' 3. Esperamos 1 segundo (1000 milisegundos).
        Await Task.Delay(1000)

        ' 4. Restauramos el color original y volvemos a habilitar el control.
        pb.BackColor = colorOriginal
        pb.Enabled = True
    End Function
    Private Async Sub pbCopyCI_Click(sender As Object, e As EventArgs) Handles pbCopyCI.Click
        ' Copia el texto de la cédula al portapapeles, si no está vacío.
        If Not String.IsNullOrWhiteSpace(lblCI.Text) Then
            My.Computer.Clipboard.SetText(lblCI.Text)
            Await MostrarEfectoDeCopiaAsync(pbCopyCI)
        End If
    End Sub

    Private Async Sub pbCopyNombre_Click(sender As Object, e As EventArgs) Handles pbCopyNombre.Click
        ' Copia el texto del nombre al portapapeles, si no está vacío.
        If Not String.IsNullOrWhiteSpace(lblNombreCompleto.Text) Then
            My.Computer.Clipboard.SetText(lblNombreCompleto.Text)
            Await MostrarEfectoDeCopiaAsync(pbCopyNombre)
        End If
    End Sub
#End Region
    ' --- FIN DE NUEVA REGIÓN ---


#Region "DTO ligero"
    Public Class FuncionarioMin
        Public Property Id As Integer
        Public Property CI As String
        Public Property Nombre As String
    End Class
    Public Class PresenciaDTO
        Public Property FuncionarioId As Integer
        Public Property Resultado As String
    End Class


#End Region

End Class
