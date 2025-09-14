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
            If dgvFuncionarios.CurrentRow IsNot Nothing Then
                Return CType(dgvFuncionarios.CurrentRow.DataBoundItem, FuncionarioMin)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub New()
        InitializeComponent()

        _modo = ModoApertura.Navegacion
    End Sub

    Public Sub New(modo As ModoApertura)
        Me.New()
        _modo = modo
    End Sub

    Private Sub frmFuncionarioBuscar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Este botón indica estado prioritario: que no cambie de color al pasar el mouse
        btnVerSituacion.Tag = "KeepBackColor"
        panelDetalle.Visible = False
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        AddHandler btnVerSituacion.Click, AddressOf btnVerSituacion_Click
        AddHandler btnGenerarFicha.Click, AddressOf btnGenerarFicha_Click
        AddHandler NotificadorEventos.DatosActualizados, AddressOf OnDatosActualizados

        ' Configurar el temporizador de búsqueda
        SearchTimer.Interval = 500 ' 500ms de espera antes de buscar
        AddHandler SearchTimer.Tick, AddressOf SearchTimer_Tick
        AddHandler txtBusqueda.TextChanged, AddressOf txtBusqueda_TextChanged
        ' Placeholder en la búsqueda (si agregaste AppTheme.SetCue)
        Try
            AppTheme.SetCue(txtBusqueda, "Buscar…")
        Catch
            ' Ignorar si no existe SetCue
        End Try

    End Sub
    Private Sub dgvFuncionarios_SelectionChanged(sender As Object, e As EventArgs) Handles dgvFuncionarios.SelectionChanged
        ' Hacemos visible el botón si hay al menos una celda seleccionada
        If dgvFuncionarios.GetCellCount(DataGridViewElementStates.Selected) > 0 Then
            btnCopiarContenido.Visible = True
        Else
            btnCopiarContenido.Visible = False
        End If
    End Sub
    Private Sub btnCopiarContenido_Click(sender As Object, e As EventArgs) Handles btnCopiarContenido.Click
        If dgvFuncionarios.SelectedCells.Count > 0 Then
            Try
                ' 1. Se realiza la operación de copiar al portapapeles
                Dim clipboardContent = dgvFuncionarios.GetClipboardContent()
                If clipboardContent IsNot Nothing Then
                    Clipboard.SetDataObject(clipboardContent)

                    ' 2. ¡Aquí llamas a la notificación de éxito!
                    Notifier.Success(Me, "¡Contenido copiado al portapapeles!")
                End If

            Catch ex As Exception
                ' En caso de un error, puedes notificarlo también
                Notifier.Error(Me, "No se pudo copiar el contenido.")
            End Try
        End If
    End Sub
#Region "Atajos de Acciones"

    Private Sub btnNotificar_Click(sender As Object, e As EventArgs) Handles btnNotificar.Click
        If FuncionarioSeleccionado IsNot Nothing Then
            ' Se llama al nuevo constructor pasando el ID y el valor True
            Dim frm As New frmNotificacionCrear(FuncionarioSeleccionado.Id, True)
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
        End If
    End Sub

    Private Sub btnSancionar_Click(sender As Object, e As EventArgs) Handles btnSancionar.Click
        If FuncionarioSeleccionado IsNot Nothing Then
            ' Abre el formulario para crear una nueva sanción.
            Dim frm As New frmSancionCrear()
            ' frm.FuncionarioId = FuncionarioSeleccionado.Id ' (Si el form lo soporta)
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
        End If
    End Sub

    Private Sub btnNovedades_Click(sender As Object, e As EventArgs) Handles btnNovedades.Click
        If FuncionarioSeleccionado IsNot Nothing Then
            ' Abre el formulario de novedades, idealmente filtrando por el funcionario.
            Dim frm As New frmNovedades()
            ' frm.FiltrarPorFuncionario(FuncionarioSeleccionado.Id) ' (Si el form lo soporta)
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
        End If
    End Sub

#End Region
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
        With dgvFuncionarios
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
        AddHandler dgvFuncionarios.CurrentCellChanged, AddressOf MostrarDetalle
        ' --- FIN DEL CAMBIO ---

        AddHandler dgvFuncionarios.CellDoubleClick, AddressOf OnDgvDoubleClick
        AddHandler dgvFuncionarios.DataError, Sub(s, ev) ev.ThrowException = False
    End Sub
#End Region

#Region "Búsqueda con Full-Text y CONTAINS"
    Private Async Function BuscarAsync() As Task
        ' Si no hay texto en la caja de búsqueda, limpiamos la grilla y el detalle.
        If String.IsNullOrWhiteSpace(txtBusqueda.Text) Then
            dgvFuncionarios.DataSource = New List(Of FuncionarioMin)()
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
                sb.AppendLine("     Id, CI, Nombre")
                sb.AppendLine("FROM   dbo.Funcionario WITH (NOLOCK)")
                sb.AppendLine("WHERE  CONTAINS((CI, Nombre), @patron)")
                sb.AppendLine("ORDER BY Nombre;")

                Dim sql = sb.ToString()
                Dim pLimite = New SqlParameter("@limite", LIMITE_FILAS)
                Dim pPatron = New SqlParameter("@patron", expresionFts)

                Dim lista = Await ctx.Database _
                                 .SqlQuery(Of FuncionarioMin)(sql, pLimite, pPatron) _
                                 .ToListAsync()

                dgvFuncionarios.DataSource = Nothing
                dgvFuncionarios.DataSource = lista

                If lista.Any() Then
                    dgvFuncionarios.ClearSelection()
                    dgvFuncionarios.Rows(0).Selected = True
                    dgvFuncionarios.CurrentCell = dgvFuncionarios.Rows(0).Cells("CI")
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

        If dgvFuncionarios.Rows.Count = 0 Then Return

        Select Case e.KeyCode
            Case Keys.Down : MoverSeleccion(+1) : e.Handled = True
            Case Keys.Up : MoverSeleccion(-1) : e.Handled = True
        End Select
    End Sub

    Private Sub MoverSeleccion(direccion As Integer)
        Dim total = dgvFuncionarios.Rows.Count
        If total = 0 Then
            LimpiarDetalle()
            Exit Sub
        End If

        Dim indexActual As Integer =
        If(dgvFuncionarios.CurrentRow Is Nothing, -1, dgvFuncionarios.CurrentRow.Index)

        Dim nuevoIndex = Math.Max(0, Math.Min(total - 1, indexActual + direccion))

        dgvFuncionarios.ClearSelection()
        dgvFuncionarios.Rows(nuevoIndex).Selected = True
        dgvFuncionarios.CurrentCell = dgvFuncionarios.Rows(nuevoIndex).Cells("CI")
    End Sub
#End Region


#Region "Detalle lateral (Foto on-demand)"
    Private Async Sub MostrarDetalle(sender As Object, e As EventArgs)
        If dgvFuncionarios.CurrentRow Is Nothing OrElse dgvFuncionarios.CurrentRow.DataBoundItem Is Nothing Then
            LimpiarDetalle()
            Return
        End If

        panelDetalle.Visible = True
        Dim id = CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value)

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

            If dgvFuncionarios.CurrentRow Is Nothing OrElse CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value) <> id Then Return
            If f Is Nothing Then Return

            ' Muestra los botones de acción
            btnSancionar.Visible = True
            btnNovedades.Visible = True
            btnNotificar.Visible = True
            btnGenerarFicha.Visible = True

            ' Concatenamos el texto fijo con el valor
            lblCI.Text = "CI: " & f.CI
            lblNombreCompleto.Text = f.Nombre ' El nombre principal no lleva etiqueta
            lblCargo.Text = "Cargo: " & If(f.Cargo Is Nothing, "-", f.Cargo.Nombre)
            lblTipo.Text = "Tipo: " & f.TipoFuncionario.Nombre
            lblFechaIngreso.Text = "Fecha Ingreso: " & f.FechaIngreso.ToShortDateString()
            lblHorarioCompleto.Text = $"Horario: {If(f.Semana IsNot Nothing, f.Semana.Nombre, "-")} / {If(f.Turno IsNot Nothing, f.Turno.Nombre, "-")} / {If(f.Horario IsNot Nothing, f.Horario.Nombre, "-")}"

            ' El estado ya estaba correcto, lo mantenemos igual
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

                If _modo = ModoApertura.Seleccion Then
                    btnVerSituacion.Enabled = False
                    btnGenerarFicha.Enabled = False
                    ToolTip1.SetToolTip(btnVerSituacion, "No disponible en modo de selección")
                Else
                    btnVerSituacion.Enabled = True
                    btnGenerarFicha.Enabled = True
                End If

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

        ' ▼▼▼ LÍNEA FINAL PARA CORREGIR EL FOCO ▼▼▼
        ' Después de que toda la actualización visual ha terminado,
        ' forzamos el foco de vuelta al TextBox de búsqueda.
        txtBusqueda.Focus()
    End Sub


    ' Este código funciona bien si quieres abrir múltiples ventanas de situación.
    Private Sub btnVerSituacion_Click(sender As Object, e As EventArgs)
        If dgvFuncionarios.CurrentRow Is Nothing Then Return
        Dim id = CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value)
        Dim frm As New frmFuncionarioSituacion(id)
        NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
    End Sub

    Private Class SituacionParaBoton
        Public Property Prioridad As Integer
        Public Property Tipo As String
        Public Property ColorIndicador As String
    End Class
    ' Ejecuta la acción sobre la fila actual según el modo
    Private Sub AbrirOSeleccionarActual()
        If dgvFuncionarios.CurrentRow Is Nothing Then Exit Sub

        If _modo = ModoApertura.Seleccion Then
            SeleccionarYcerrar()
            Exit Sub
        End If

        ' === Modo Navegación: abrir editor del funcionario seleccionado ===
        Dim id As Integer = CInt(dgvFuncionarios.CurrentRow.Cells("Id").Value)

        Dim dashboard = Application.OpenForms.OfType(Of frmDashboard).FirstOrDefault()
        If dashboard Is Nothing Then Exit Sub

        Dim editorExistente = dashboard.panelContenido.Controls.
        OfType(Of frmFuncionarioCrear)().
        FirstOrDefault(Function(f) f.Tag IsNot Nothing AndAlso f.Tag.ToString() = id.ToString())

        If editorExistente IsNot Nothing Then
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(editorExistente)
        Else
            Dim frm As New frmFuncionarioCrear(id)
            frm.Tag = id.ToString()
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
        End If
    End Sub
    Private Sub dgvFuncionarios_KeyDown(sender As Object, e As KeyEventArgs) Handles dgvFuncionarios.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            AbrirOSeleccionarActual()
        End If
    End Sub

    ' Busca y reemplaza este método en tu archivo frmFuncionarioBuscar.vb

    Private Sub OnDgvDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        AbrirOSeleccionarActual()
    End Sub
    Private Sub frmFuncionarioBuscar_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            AbrirOSeleccionarActual()
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
        lblCI.Text = "CI: -"
        lblNombreCompleto.Text = "Seleccione un funcionario"
        lblCargo.Text = "Cargo: -"
        lblTipo.Text = "Tipo: -"
        lblFechaIngreso.Text = "Fecha Ingreso: -"
        lblEstadoActividad.Text = "Estado: -"
        lblPresencia.Text = ""
        pbFotoDetalle.Image = Nothing
        lblHorarioCompleto.Text = "Horario: -"
        _detallesEstadoActual.Clear()

        ' ▼▼▼ AQUÍ OCULTAS TODO ▼▼▼
        btnGenerarFicha.Visible = False
        btnVerSituacion.Visible = False
        btnSancionar.Visible = False
        btnNovedades.Visible = False
        btnNotificar.Visible = False
        panelDetalle.Visible = False
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

    Private Sub btnGenerarFicha_Click(sender As Object, e As EventArgs)
        If FuncionarioSeleccionado IsNot Nothing Then
            Dim frm As New frmFichaPersonalRPT(FuncionarioSeleccionado.Id)
            NavegacionHelper.AbrirNuevaInstanciaEnDashboard(frm)
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
