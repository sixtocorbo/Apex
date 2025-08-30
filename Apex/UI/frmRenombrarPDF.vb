Imports System.Data.Entity
Imports System.IO
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports AxAcroPDFLib ' Asegúrate de tener la referencia a Adobe Acrobat Reader en tu proyecto

Public Class frmRenombrarPDF
    Inherits Form

    ' === CAMPOS PRIVADOS ===
    Private directorioPDF As String = String.Empty
    Private archivosPDF As List(Of String) = New List(Of String)()
    Private cadenaParaRenombrar As String = String.Empty
    Private _unitOfWork As IUnitOfWork
    Private _funcionarioSeleccionado As Funcionario

    Public Sub New()
        MyBase.New()
        InitializeComponent() ' <- Esto crea Timer1 y todos los controles del Designer
    End Sub

    Private Async Sub frmRenombrarPDF_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Mostrar cursor de espera mientras se carga
        Me.Cursor = Cursors.WaitCursor

        ' Inicializar UnitOfWork y temporizador
        _unitOfWork = New UnitOfWork()
        Timer1.Interval = 500 ' Intervalo para el debounce de la búsqueda

        ' Cargar nomenclaturas en el DataGridView
        Try
            Await CargarNomenclatura()
        Catch ex As Exception
            MessageBox.Show($"Error al cargar las nomenclaturas: {ex.Message}",
                        "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' === AJUSTES PARA RESOLUCIONES PEQUEÑAS ===
        ' Las propiedades AutoScroll de los FlowLayoutPanel ya gestionan las barras de desplazamiento.
        ' Ya no se necesita el código para ajustar las columnas del TableLayoutPanel.

        ' Ajustar el tamaño mínimo si deseas permitir ventanas más pequeñas (opcional)
        Me.MinimumSize = New Size(600, 400)

        ' Forzar el recalculado del diseño la primera vez (opcional)
        Me.PerformLayout()

        ' Devolver el cursor a su estado normal
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnBuscarFuncionario_Click(sender As Object, e As EventArgs) Handles btnBuscarFuncionario.Click
        Using form As New frmFuncionarioBuscar(frmFuncionarioBuscar.ModoApertura.Seleccion)
            If form.ShowDialog() = DialogResult.OK Then
                Dim funcionarioMinimo = form.FuncionarioSeleccionado
                If funcionarioMinimo IsNot Nothing Then
                    Me.Cursor = Cursors.WaitCursor
                    Dim repo = _unitOfWork.Repository(Of Funcionario)()
                    Me._funcionarioSeleccionado = repo.GetById(funcionarioMinimo.Id)
                    Me.Cursor = Cursors.Default

                    ActualizarDatosFuncionario()
                End If
            End If
        End Using
    End Sub

    Private Sub ActualizarDatosFuncionario()
        If _funcionarioSeleccionado IsNot Nothing Then
            txtNombre.Text = _funcionarioSeleccionado.Nombre
            txtCedula.Text = _funcionarioSeleccionado.CI

            NombreCadena() ' Actualizar el nombre propuesto
        Else
            txtNombre.Clear()
            txtCedula.Clear()
        End If
    End Sub

    ' === GESTIÓN DE ARCHIVOS Y CARPETAS ===
    Private Sub btnSeleccionarCarpeta_Click(sender As Object, e As EventArgs) Handles btnSeleccionarCarpeta.Click
        If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
            directorioPDF = FolderBrowserDialog1.SelectedPath
            CargarArchivosEnListBox(directorioPDF)
        End If
    End Sub

    Private Sub CargarArchivosEnListBox(path As String)
        Try
            Me.Cursor = Cursors.WaitCursor
            archivosPDF = Directory.GetFiles(path, "*.pdf", SearchOption.AllDirectories).ToList()
            ActualizarListBox(archivosPDF)
        Catch ex As Exception
            MessageBox.Show($"No se pudo leer la carpeta. Verifique los permisos. Error: {ex.Message}", "Error de Lectura", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ActualizarListBox(lista As List(Of String))
        ListBox1.Items.Clear()
        ListBox1.Items.AddRange(lista.ToArray())
        lblTotal.Text = $"Total Archivos PDF: {lista.Count}"
    End Sub

    ' === VISUALIZACIÓN Y SELECCIÓN DE PDF ===
    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click
        If ListBox1.SelectedItem IsNot Nothing Then
            Dim ruta = ListBox1.SelectedItem.ToString()
            MostrarPDF(ruta)
            lblNombreArchivo.Text = $"Archivo: {Path.GetFileName(ruta)}"
        Else
            lblNombreArchivo.Text = "Archivo: Ninguno"
        End If
    End Sub

    Private Sub MostrarPDF(ruta As String)
        If File.Exists(ruta) Then
            Try
                AxAcroPDF1.LoadFile(ruta)
                AxAcroPDF1.setShowToolbar(False)
                AxAcroPDF1.setView("Fit")
            Catch ex As Exception
                MessageBox.Show($"No se pudo cargar el visor de PDF. Error: {ex.Message}", "Error de Visualización", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        End If
    End Sub

    ' === BÚSQUEDA DINÁMICA DE ARCHIVOS ===
    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged
        Timer1.Stop()
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        BuscarArchivoInterno(TextBox1.Text, TextBox2.Text)
    End Sub

    Private Sub BuscarArchivoInterno(busq1 As String, busq2 As String)
        Dim res = archivosPDF.Where(Function(f)
                                        Return f.ToUpper().Contains(busq1.ToUpper()) AndAlso
                                               f.ToUpper().Contains(busq2.ToUpper())
                                    End Function).ToList()
        ActualizarListBox(res)
    End Sub

    ' === CARGA Y CONFIGURACIÓN DE NOMENCLATURAS ===
    Private Async Function CargarNomenclatura() As Task
        Dim repo = _unitOfWork.Repository(Of Nomenclatura)()
        ' Se proyecta a un tipo anónimo para renombrar las columnas que necesita el DataGridView.
        Dim nomenclaturasParaGrid = Await repo.GetAll().Select(Function(n) New With {
            .Id = n.Id,
            .NOMECLATURA = n.Nombre, ' Se usa el campo 'Nombre' de la entidad
            .FECHA = If(n.UsaFecha, "SI", "NO"),
            .NCODE = If(n.UsaNomenclaturaCodigo, "SI", "NO")
        }).ToListAsync()

        dgvTiposNomenclaturas.DataSource = nomenclaturasParaGrid
        ConfigurarDgvTiposNomenclaturas()
    End Function

    Private Sub ConfigurarDgvTiposNomenclaturas()
        If dgvTiposNomenclaturas.Rows.Count > 0 Then
            ' --> AHORA LOS NOMBRES COINCIDEN CON EL OBJETO ANÓNIMO CREADO
            dgvTiposNomenclaturas.Columns("Id").Visible = False
            dgvTiposNomenclaturas.Columns("FECHA").Visible = False
            dgvTiposNomenclaturas.Columns("NCODE").Visible = False
            dgvTiposNomenclaturas.Columns("NOMECLATURA").HeaderText = "Nomenclatura"
            dgvTiposNomenclaturas.Columns("NOMECLATURA").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If
    End Sub

    Private Sub dgvTiposNomenclaturas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvTiposNomenclaturas.SelectionChanged
        If dgvTiposNomenclaturas.CurrentRow IsNot Nothing Then
            ' --> SE ACCEDE A LAS CELDAS CON LOS NOMBRES CORRECTOS
            Dim usaFecha As Boolean = dgvTiposNomenclaturas.CurrentRow.Cells("FECHA").Value.ToString().Trim().ToUpper() = "SI"
            Dim usaCodigo As Boolean = dgvTiposNomenclaturas.CurrentRow.Cells("NCODE").Value.ToString().Trim().ToUpper() = "SI"

            txtFecha.Visible = usaFecha
            txtCodigo.Visible = usaCodigo
            NombreCadena()
        End If
    End Sub

    ' === CONSTRUCCIÓN DE LA CADENA PARA RENOMBRAR ===
    Private Sub NombreCadena()
        Dim nombre As String = If(_funcionarioSeleccionado IsNot Nothing, _funcionarioSeleccionado.Nombre, "")
        nombre = New String(nombre.Where(Function(c) Not Char.IsDigit(c)).ToArray()).Trim() ' Limpiar dígitos

        Dim COD As String = If(dgvTiposNomenclaturas.CurrentRow IsNot Nothing AndAlso dgvTiposNomenclaturas.CurrentRow.Cells("NOMECLATURA").Value IsNot Nothing,
                               dgvTiposNomenclaturas.CurrentRow.Cells("NOMECLATURA").Value.ToString(), String.Empty)

        Dim fechaParte = If(txtFecha.Visible, txtFecha.Value.ToString("dd-MM-yyyy"), String.Empty)
        Dim codigoParte = If(txtCodigo.Visible AndAlso Not String.IsNullOrEmpty(txtCodigo.Text), txtCodigo.Text, String.Empty)
        Dim notParte = If(CheckBox1.Checked, "NOT", String.Empty)
        Dim cedulaParte = txtCedula.Text

        Dim partes = New List(Of String) From {COD, notParte, codigoParte, fechaParte, nombre, cedulaParte}
        cadenaParaRenombrar = String.Join(" ", partes.Where(Function(s) Not String.IsNullOrEmpty(s)))
        txtNombreModificado.Text = cadenaParaRenombrar
    End Sub

    ' Disparamos la actualización del nombre propuesto si cambian los campos
    Private Sub CamposParaRenombrar_Changed(sender As Object, e As EventArgs) Handles txtCodigo.TextChanged, txtFecha.ValueChanged, CheckBox1.CheckedChanged
        NombreCadena()
    End Sub

    ' === ACCIÓN DE RENOMBRAR EL ARCHIVO ===
    Private Sub btnRenombrar_Click(sender As Object, e As EventArgs) Handles btnRenombrar.Click
        If _funcionarioSeleccionado Is Nothing Then
            MessageBox.Show("Debe seleccionar un funcionario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If ListBox1.SelectedItem Is Nothing Then
            MessageBox.Show("Debe seleccionar un archivo de la lista.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            NombreCadena() ' Aseguramos que el nombre esté actualizado
            Dim rutaActual = ListBox1.SelectedItem.ToString()
            Dim dirActual = Path.GetDirectoryName(rutaActual)
            Dim nuevaRuta = Path.Combine(dirActual, cadenaParaRenombrar & ".pdf")

            If File.Exists(nuevaRuta) Then
                MessageBox.Show("Ya existe un archivo con ese nombre en la carpeta de destino.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Renombrar el archivo
            File.Move(rutaActual, nuevaRuta)

            ' Actualizar la lista en memoria
            archivosPDF.Remove(rutaActual)
            archivosPDF.Add(nuevaRuta)

            ' Refrescar la búsqueda para mostrar el archivo renombrado
            BuscarArchivoInterno(TextBox1.Text, TextBox2.Text)

            ' Seleccionar el nuevo archivo en la lista
            Dim idx = ListBox1.Items.IndexOf(nuevaRuta)
            If idx <> -1 Then ListBox1.SelectedIndex = idx

            LimpiarSeleccion()

        Catch ex As Exception
            MessageBox.Show($"Error al renombrar el archivo: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' === LIMPIEZA Y CIERRE ===
    Private Sub LimpiarSeleccion()
        _funcionarioSeleccionado = Nothing
        txtNombre.Clear()
        txtCedula.Clear()
        txtCodigo.Clear()
        CheckBox1.Checked = False
        btnBuscarFuncionario.Focus()
    End Sub

    Private Sub frmRenombrarPDF_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub frmRenombrarPDF_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Liberar recursos
        If _unitOfWork IsNot Nothing Then
            _unitOfWork.Dispose()
        End If
    End Sub

End Class