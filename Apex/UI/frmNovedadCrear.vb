' Apex/UI/frmNovedadCrear.vb
Imports System.ComponentModel
Imports System.Data.Entity
Imports System.IO


Public Class frmNovedadCrear
#Region "Mini wizard"
    ' === Wizard simple para forzar pasos ===
    Private Enum PasoWizard
        Novedad = 0
        Funcionarios = 1
        Fotos = 2
        Confirmacion = 3
    End Enum


    Private _forzarPasos As Boolean = True
    Private _pasoActual As PasoWizard = PasoWizard.Novedad
    Private _pasosCompletados As New HashSet(Of PasoWizard)
    ' --- Controles del Paso 4 (creados en tiempo de ejecución) ---
    Private TabPageConfirm As TabPage
    Private lblConfTitulo As Label
    Private lblConfFecha As Label
    Private txtConfTexto As TextBox
    Private lblConfFuncs As Label
    Private lstConfFuncs As ListBox
    Private lblConfFotos As Label
    Private flpConfFotos As FlowLayoutPanel
    ' --- Fin Controles Paso 4 ---

#End Region

    Private _svc As New NovedadService()
    Private _novedad As Novedad
    Private _modo As ModoFormulario
    Private _novedadId As Integer
    Private _funcionariosSeleccionados As New BindingList(Of Funcionario)

    Private _fotosExistentes As New BindingList(Of NovedadFoto)
    Private _rutasFotosNuevas As New List(Of String)
    ' --- MODIFICACIÓN: Lista para marcar fotos a eliminar ---
    Private _fotosParaEliminar As New List(Of NovedadFoto)
    Private _pictureBoxSeleccionado As PictureBox = Nothing

    Public Enum ModoFormulario
        Crear
        Editar
    End Enum

    Public Sub New()
        InitializeComponent()
        _modo = ModoFormulario.Crear
        _novedad = New Novedad()
        Me.Text = "Crear Nueva Novedad"
        btnGuardar.Text = "Guardar"
    End Sub

    Public Sub New(novedadId As Integer)
        Me.New()
        _modo = ModoFormulario.Editar
        _novedadId = novedadId
        Me.Text = "Editar Novedad"
        btnGuardar.Text = "Actualizar"
    End Sub

    Private Async Sub frmNovedadCrear_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        Me.KeyPreview = True
        Me.AcceptButton = btnGuardar

        lstFuncionariosSeleccionados.DataSource = _funcionariosSeleccionados
        lstFuncionariosSeleccionados.DisplayMember = "Nombre"
        lstFuncionariosSeleccionados.ValueMember = "Id"
        AddHandler btnSiguiente.Click, AddressOf btnSiguiente_Click
        AddHandler btnAnterior.Click, AddressOf btnAnterior_Click
        Try
            AppTheme.SetCue(txtTexto, "Ingrese el texto de la novedad...")
            AppTheme.SetCue(dtpFecha, "Fecha de la novedad…")
            AppTheme.SetCue(lstFuncionariosSeleccionados, "No hay funcionarios seleccionados…")
            AppTheme.SetCue(flpFotos, "No hay fotos agregadas…")
        Catch
        End Try
        'inicio wizard
        ' === WIZARD: bloquear navegación por pestañas si _forzarPasos ===
        If _forzarPasos Then
            AddHandler TabControlMain.Selecting, AddressOf TabControlMain_Selecting_Bloquear
        End If

        ' Botones iniciales
        ActualizarUIWizard()
        ' === Crear dinámicamente la pestaña "Confirmación" ===
        CrearTabConfirmacion()
        ' === Fin creación dinámica ===
        'finaliza wizard
        ' Pre-cargar si edita
        If _modo = ModoFormulario.Editar Then
            Try
                Await CargarDatosParaEdicion()
                ' Autocompletar pasos ya válidos
                If ValidarPasoNovedad(silent:=True) Then _pasosCompletados.Add(PasoWizard.Novedad)
                If ValidarPasoFuncionarios(silent:=True) Then _pasosCompletados.Add(PasoWizard.Funcionarios)
                ' Fotos es opcional
                _pasosCompletados.Add(PasoWizard.Fotos)
                ActualizarUIWizard()
            Catch ex As Exception
                Notifier.[Error](Me, $"No se pudo cargar la novedad: {ex.Message}")
                Close()
                Return
            End Try
        Else
            Notifier.Info(Me, "Paso 1: completá Fecha y Texto. Luego Siguiente.")
        End If
    End Sub
    Private Sub CrearTabConfirmacion()
        If TabPageConfirm IsNot Nothing Then Return

        TabPageConfirm = New TabPage() With {.Text = "Confirmación", .Padding = New Padding(10)}

        lblConfTitulo = New Label() With {
        .Text = "Paso 4: Confirmá antes de guardar",
        .Font = New Font("Segoe UI", 12.0F, FontStyle.Bold),
        .Dock = DockStyle.Top,
        .Height = 32
    }

        Dim pnl As New TableLayoutPanel With {
        .Dock = DockStyle.Fill,
        .ColumnCount = 2,
        .RowCount = 4
    }
        pnl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 40))
        pnl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 60))
        pnl.RowStyles.Add(New RowStyle(SizeType.AutoSize)) ' Fecha
        pnl.RowStyles.Add(New RowStyle(SizeType.Percent, 45)) ' Texto
        pnl.RowStyles.Add(New RowStyle(SizeType.Percent, 25)) ' Funcionarios
        pnl.RowStyles.Add(New RowStyle(SizeType.Percent, 30)) ' Fotos

        lblConfFecha = New Label() With {.AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Regular)}
        txtConfTexto = New TextBox() With {
        .Multiline = True, .ReadOnly = True, .ScrollBars = ScrollBars.Vertical, .Dock = DockStyle.Fill
    }

        lblConfFuncs = New Label() With {.Text = "Funcionarios:", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)}
        lstConfFuncs = New ListBox() With {.Dock = DockStyle.Fill}

        lblConfFotos = New Label() With {.Text = "Fotos:", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F, FontStyle.Bold)}
        flpConfFotos = New FlowLayoutPanel() With {.Dock = DockStyle.Fill, .AutoScroll = True, .WrapContents = True}

        ' fila 0: fecha (colspan 2)
        pnl.Controls.Add(New Label() With {.Text = "Fecha:", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F)}, 0, 0)
        pnl.Controls.Add(lblConfFecha, 1, 0)

        ' fila 1: texto
        pnl.Controls.Add(New Label() With {.Text = "Texto:", .AutoSize = True, .Font = New Font("Segoe UI", 10.0F)}, 0, 1)
        pnl.Controls.Add(txtConfTexto, 1, 1)

        ' fila 2: funcionarios
        pnl.Controls.Add(lblConfFuncs, 0, 2)
        pnl.Controls.Add(lstConfFuncs, 1, 2)

        ' fila 3: fotos
        pnl.Controls.Add(lblConfFotos, 0, 3)
        pnl.Controls.Add(flpConfFotos, 1, 3)

        TabPageConfirm.Controls.Add(pnl)
        TabPageConfirm.Controls.Add(lblConfTitulo)

        ' Insertamos al final
        If TabControlMain.TabPages.Count < 4 Then
            TabControlMain.TabPages.Add(TabPageConfirm)
        End If
    End Sub

    Private Sub ActualizarResumenConfirmacion()
        ' Fecha y texto
        lblConfFecha.Text = dtpFecha.Value.ToShortDateString()
        txtConfTexto.Text = txtTexto.Text

        ' Funcionarios
        lstConfFuncs.BeginUpdate()
        lstConfFuncs.Items.Clear()
        For Each f In _funcionariosSeleccionados
            lstConfFuncs.Items.Add($"{f.CI} - {f.Nombre}")
        Next
        lstConfFuncs.EndUpdate()

        ' Fotos (miniaturas)
        For Each ctrl As Control In flpConfFotos.Controls
            Dim pb = TryCast(ctrl, PictureBox)
            If pb IsNot Nothing Then
                If pb.Image IsNot Nothing Then pb.Image.Dispose()
                pb.Dispose()
            End If
        Next
        flpConfFotos.Controls.Clear()

        ' Reusar lo que ya está mostrado en la pestaña Fotos (flpFotos)
        For Each ctrl In flpFotos.Controls
            Dim src = TryCast(ctrl, PictureBox)
            If src Is Nothing OrElse src.Image Is Nothing Then Continue For

            Dim thumb As New PictureBox With {
            .SizeMode = PictureBoxSizeMode.Zoom,
            .Size = New Size(96, 96),
            .BorderStyle = BorderStyle.FixedSingle,
            .Margin = New Padding(4)
        }
            ' copiar en memoria para no compartir el mismo objeto
            Using bmp As New Bitmap(src.Image)
                thumb.Image = New Bitmap(bmp)
            End Using
            flpConfFotos.Controls.Add(thumb)
        Next
    End Sub

    'mini wizrd
    Private Async Sub btnSiguiente_Click(sender As Object, e As EventArgs)
        Select Case _pasoActual
            Case PasoWizard.Novedad
                If Not ValidarPasoNovedad() Then Return
                _pasosCompletados.Add(PasoWizard.Novedad)
                IrAPaso(PasoWizard.Funcionarios)

            Case PasoWizard.Funcionarios
                If Not ValidarPasoFuncionarios() Then Return
                _pasosCompletados.Add(PasoWizard.Funcionarios)
                IrAPaso(PasoWizard.Fotos)

            Case PasoWizard.Fotos
                _pasosCompletados.Add(PasoWizard.Fotos)
                ActualizarResumenConfirmacion()
                IrAPaso(PasoWizard.Confirmacion)

            Case PasoWizard.Confirmacion
                ' Último paso -> guardar/actualizar sin PerformClick
                Await GuardarOActualizarAsync()
        End Select
    End Sub



    Private Sub btnAnterior_Click(sender As Object, e As EventArgs)
        Select Case _pasoActual
            Case PasoWizard.Funcionarios : IrAPaso(PasoWizard.Novedad)
            Case PasoWizard.Fotos : IrAPaso(PasoWizard.Funcionarios)
            Case PasoWizard.Confirmacion : IrAPaso(PasoWizard.Fotos)
        End Select
    End Sub


    Private Sub IrAPaso(p As PasoWizard)
        _pasoActual = p
        TabControlMain.SelectedIndex = CInt(p)
        ActualizarUIWizard()
    End Sub

    Private Sub ActualizarUIWizard()
        btnAnterior.Enabled = (TabControlMain.SelectedIndex > 0)
        btnGuardar.Visible = False
        btnSiguiente.Visible = True
        Me.AcceptButton = btnSiguiente

        If TabControlMain.SelectedTab.Text = "Confirmación" Then
            btnSiguiente.Text = If(_modo = ModoFormulario.Crear, "Guardar", "Actualizar")
        Else
            btnSiguiente.Text = "Siguiente >"
        End If
    End Sub


    Private Function ValidarPasoNovedad(Optional silent As Boolean = False) As Boolean
        Dim ok As Boolean = True
        If String.IsNullOrWhiteSpace(txtTexto.Text) Then ok = False
        ' Podés agregar reglas de fecha, ej: no futuro
        ' If dtpFecha.Value.Date > Date.Today Then ok = False

        If Not ok AndAlso Not silent Then
            Notifier.Warn(Me, "Completá el texto (y verificá la fecha).")
            TabControlMain.SelectedTab = TabPageNovedad
            txtTexto.Focus()
        End If
        Return ok
    End Function

    Private Function ValidarPasoFuncionarios(Optional silent As Boolean = False) As Boolean
        Dim ok As Boolean = (_funcionariosSeleccionados IsNot Nothing AndAlso _funcionariosSeleccionados.Count > 0)
        If Not ok AndAlso Not silent Then
            Notifier.Warn(Me, "Seleccioná al menos un funcionario.")
            TabControlMain.SelectedTab = TabPageFuncionarios
        End If
        Return ok
    End Function


    'fin mini wizard
    Private Sub TabControlMain_Selecting_Bloquear(sender As Object, e As TabControlCancelEventArgs)
        If Not _forzarPasos Then Return
        ' Solo permitir la pestaña del paso actual
        Dim idxEsperado As Integer = CInt(_pasoActual)
        If e.TabPageIndex <> idxEsperado Then
            e.Cancel = True
            Notifier.Warn(Me, "Seguí el orden con los botones Siguiente / Anterior.")
        End If
    End Sub


    Private Async Function CargarDatosParaEdicion() As Task
        _novedad = Await _svc.GetByIdAsync(_novedadId)
        If _novedad Is Nothing Then
            Notifier.[Error](Me, "No se encontró la novedad para editar.")
            Close()
            Return
        End If

        dtpFecha.Value = _novedad.Fecha
        txtTexto.Text = _novedad.Texto

        _funcionariosSeleccionados.Clear()
        Dim funcionariosAsociados = Await _svc.GetFuncionariosPorNovedadAsync(_novedad.Id)
        For Each func In funcionariosAsociados
            _funcionariosSeleccionados.Add(func)
        Next

        Await CargarFotosAsync()
    End Function


#Region "Lógica de Funcionarios"
    Private Async Sub btnAgregarFuncionario_Click(sender As Object, e As EventArgs) Handles btnAgregarFuncionario.Click
        Using frm As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If frm.ShowDialog(Me) = DialogResult.OK AndAlso frm.FuncionarioSeleccionado IsNot Nothing Then
                Dim idFuncionarioSeleccionado = frm.FuncionarioSeleccionado.Id
                If Not _funcionariosSeleccionados.Any(Function(f) f.Id = idFuncionarioSeleccionado) Then
                    Using uow As New UnitOfWork()
                        Dim funcCompleto = Await uow.Repository(Of Funcionario)().GetByIdAsync(idFuncionarioSeleccionado)
                        If funcCompleto IsNot Nothing Then
                            _funcionariosSeleccionados.Add(funcCompleto)
                            Notifier.Success(Me, $"Agregado: {funcCompleto.Nombre}")
                        End If
                    End Using
                Else
                    Notifier.Warn(Me, "Ese funcionario ya está en la lista.")
                End If
            End If
        End Using
    End Sub

    Private Sub btnQuitarFuncionario_Click(sender As Object, e As EventArgs) Handles btnQuitarFuncionario.Click
        If lstFuncionariosSeleccionados.SelectedItems.Count = 0 Then
            Notifier.Info(Me, "Seleccioná al menos un funcionario para quitar.")
            Return
        End If

        Dim itemsAQuitar = lstFuncionariosSeleccionados.SelectedItems.Cast(Of Funcionario).ToList()
        For Each item In itemsAQuitar
            _funcionariosSeleccionados.Remove(item)
        Next
        Notifier.Info(Me, "Funcionario/s quitado/s del listado.")
    End Sub

#End Region

#Region "Lógica de Fotos"
    ' Crea una copia desacoplada del stream para evitar locks
    Private Function CrearImagenCopia(bytes() As Byte) As Image
        Using ms As New MemoryStream(bytes)
            Using bmp As New Bitmap(ms)
                Return New Bitmap(bmp)
            End Using
        End Using
    End Function

    Private Sub LimpiarFotos()
        For Each ctrl As Control In flpFotos.Controls
            Dim pic = TryCast(ctrl, PictureBox)
            If pic IsNot Nothing Then
                Dim img = pic.Image
                pic.Image = Nothing
                If img IsNot Nothing Then img.Dispose()
                pic.Dispose()
            End If
        Next
        flpFotos.Controls.Clear()
    End Sub

    Private Sub DisposePictureBox(ByRef pic As PictureBox)
        If pic Is Nothing Then Exit Sub
        If pic.Image IsNot Nothing Then
            pic.Image.Dispose()
            pic.Image = Nothing
        End If
        flpFotos.Controls.Remove(pic)
        pic.Dispose()
        pic = Nothing
    End Sub

    Private Async Function CargarFotosAsync() As Task
        LimpiarFotos()
        _fotosExistentes.Clear()
        _pictureBoxSeleccionado = Nothing

        If _modo = ModoFormulario.Editar Then
            Dim fotosDb = Await _svc.GetFotosPorNovedadAsync(_novedadId)
            For Each foto In fotosDb
                _fotosExistentes.Add(foto)
                AgregarPictureBox(foto.Foto, foto) ' tag = NovedadFoto (persistida)
            Next
        End If
    End Function

    Private Sub AgregarPictureBox(imageData As Byte(), tag As Object)
        Dim pic As New PictureBox With {
        .Image = CrearImagenCopia(imageData),
        .SizeMode = PictureBoxSizeMode.Zoom,
        .Size = New Size(120, 120),
        .Margin = New Padding(5),
        .BorderStyle = BorderStyle.FixedSingle,
        .Tag = tag,
        .Cursor = Cursors.Hand
    }
        AddHandler pic.DoubleClick, AddressOf PictureBox_DoubleClick
        AddHandler pic.Click, AddressOf PictureBox_Click
        flpFotos.Controls.Add(pic)
    End Sub

    Private Sub PictureBox_Click(sender As Object, e As EventArgs)
        Dim picClickeado = TryCast(sender, PictureBox)
        If picClickeado Is Nothing Then Return

        If _pictureBoxSeleccionado IsNot Nothing Then
            _pictureBoxSeleccionado.BackColor = Color.Transparent
        End If

        picClickeado.BackColor = Color.DodgerBlue
        _pictureBoxSeleccionado = picClickeado
    End Sub

    Private Sub PictureBox_DoubleClick(sender As Object, e As EventArgs)
        Dim pic = TryCast(sender, PictureBox)
        If pic IsNot Nothing AndAlso pic.Image IsNot Nothing Then
            Using frm As New frmFotografiaNovedades(pic.Image)
                frm.ShowDialog(Me)
            End Using
        End If
    End Sub

    Private Sub btnAgregarFoto_Click(sender As Object, e As EventArgs) Handles btnAgregarFoto.Click
        Using ofd As New OpenFileDialog With {
        .Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp",
        .Multiselect = True
    }
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim agregadas As Integer = 0
                For Each archivo In ofd.FileNames
                    Try
                        If Not File.Exists(archivo) Then Continue For
                        Dim ext = Path.GetExtension(archivo).ToLowerInvariant()
                        If Not {".jpg", ".jpeg", ".png", ".bmp"}.Contains(ext) Then Continue For

                        ' Tamaño ≤ 15 MB
                        Dim fi = New FileInfo(archivo)
                        If fi.Length > 15 * 1024 * 1024 Then
                            Notifier.Warn(Me, $"Se omitió {fi.Name}: supera 15 MB.")
                            Continue For
                        End If

                        ' Evitar duplicados por ruta
                        If _rutasFotosNuevas.Contains(archivo) Then
                            Continue For
                        End If

                        _rutasFotosNuevas.Add(archivo)
                        AgregarPictureBox(File.ReadAllBytes(archivo), archivo) ' tag = String (nueva)
                        agregadas += 1
                    Catch ex As Exception
                        Notifier.[Error](Me, $"No se pudo agregar '{Path.GetFileName(archivo)}': {ex.Message}")
                    End Try
                Next

                If agregadas > 0 Then
                    Notifier.Success(Me, $"{agregadas} foto(s) agregada(s).")
                Else
                    Notifier.Info(Me, "No se agregaron fotos.")
                End If
            End If
        End Using
    End Sub

    ' --- MODIFICACIÓN CLAVE: Lógica de eliminación ---
    Private Sub btnEliminarFoto_Click(sender As Object, e As EventArgs) Handles btnEliminarFoto.Click
        If _pictureBoxSeleccionado Is Nothing Then
            Notifier.Info(Me, "Seleccioná una foto para eliminar.")
            Return
        End If

        If MessageBox.Show("¿Eliminar la foto seleccionada?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> DialogResult.Yes Then
            Return
        End If

        If TypeOf _pictureBoxSeleccionado.Tag Is NovedadFoto Then
            Dim fotoAEliminar = CType(_pictureBoxSeleccionado.Tag, NovedadFoto)
            If Not _fotosParaEliminar.Contains(fotoAEliminar) Then _fotosParaEliminar.Add(fotoAEliminar)
        ElseIf TypeOf _pictureBoxSeleccionado.Tag Is String Then
            Dim rutaAEliminar = CType(_pictureBoxSeleccionado.Tag, String)
            _rutasFotosNuevas.Remove(rutaAEliminar)
        End If

        DisposePictureBox(_pictureBoxSeleccionado)
        Notifier.Info(Me, "Foto eliminada de la selección.")
    End Sub

#End Region

#Region "Guardado y Cancelación"
    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If _forzarPasos AndAlso _pasoActual <> PasoWizard.Confirmacion Then
            Do While _pasoActual <> PasoWizard.Confirmacion
                btnSiguiente.PerformClick()
                If _pasoActual <> PasoWizard.Confirmacion Then Return
            Loop
        End If
        Await GuardarOActualizarAsync()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
    ' Devuelve True si guardó/actualizó OK
    Private Async Function GuardarOActualizarAsync() As Task(Of Boolean)
        If String.IsNullOrWhiteSpace(txtTexto.Text) Then
            Notifier.Warn(Me, "El texto de la novedad no puede estar vacío.")
            Return False
        End If
        If _funcionariosSeleccionados.Count = 0 Then
            Notifier.Warn(Me, "Seleccioná al menos un funcionario.")
            Return False
        End If

        btnGuardar.Enabled = False
        btnSiguiente.Enabled = False
        LoadingHelper.MostrarCargando(Me)
        Dim oldCursor = Me.Cursor
        Me.Cursor = Cursors.WaitCursor

        Try
            Dim funcionarioIds = _funcionariosSeleccionados.Select(Function(f) f.Id).ToList()

            If _modo = ModoFormulario.Crear Then
                ' Crear novedad
                Dim nuevaNovedad = Await _svc.CrearNovedadCompletaAsync(
                dtpFecha.Value.Date,
                txtTexto.Text.Trim(),
                funcionarioIds
            )

                ' Fotos nuevas
                If nuevaNovedad IsNot Nothing AndAlso _rutasFotosNuevas.Any() Then
                    For Each ruta In _rutasFotosNuevas
                        Await _svc.AddFotoAsync(nuevaNovedad.Id, ruta)
                    Next
                End If

                ' Notificaciones
                Notifier.Success(Me, "Novedad creada correctamente.")
                NotificadorEventos.NotificarCambioEnNovedad(nuevaNovedad.Id) ' <<< AVISA AL LISTADO DE NOVEDADES
                For Each id In funcionarioIds
                    NotificadorEventos.NotificarCambiosEnFuncionario(id)      ' <<< AVISA A PANTALLAS VINCULADAS AL FUNCIONARIO
                Next

            Else
                ' --- Editar ---
                ' Eliminar marcadas
                If _fotosParaEliminar.Any() Then
                    For Each foto In _fotosParaEliminar
                        Await _svc.DeleteFotoAsync(foto.Id)
                    Next
                End If

                ' Agregar nuevas
                If _rutasFotosNuevas.Any() Then
                    For Each ruta In _rutasFotosNuevas
                        Await _svc.AddFotoAsync(_novedadId, ruta)
                    Next
                End If

                ' Actualizar datos de la novedad
                _novedad.Fecha = dtpFecha.Value.Date
                _novedad.Texto = txtTexto.Text.Trim()
                Await _svc.ActualizarNovedadCompletaAsync(_novedad, funcionarioIds)

                ' Notificaciones
                Notifier.Success(Me, "Novedad actualizada.")
                NotificadorEventos.NotificarCambioEnNovedad(_novedadId)       ' <<< AVISA AL LISTADO DE NOVEDADES
                For Each id In funcionarioIds
                    NotificadorEventos.NotificarCambiosEnFuncionario(id)      ' <<< AVISA A PANTALLAS VINCULADAS AL FUNCIONARIO
                Next
            End If

            ' Cerrar OK
            Me.DialogResult = DialogResult.OK
            Me.Close()
            Return True

        Catch ex As Exception
            Notifier.[Error](Me, $"Ocurrió un error al guardar: {ex.Message}")
            Return False

        Finally
            Me.Cursor = oldCursor
            btnGuardar.Enabled = True
            btnSiguiente.Enabled = True
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function


#End Region
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
            e.Handled = True
        End If
    End Sub

End Class