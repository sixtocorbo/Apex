Imports System.Data.Entity
Imports System.Windows.Forms

Public Class frmNomenclaturas
    Private _unitOfWork As New UnitOfWork()
    Private _listaNomenclaturas As List(Of Nomenclatura)
    Private _nomenclaturaSeleccionada As Nomenclatura

    Private Async Sub frmGestionNomenclaturas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.WaitCursor
        Await CargarDatosAsync()
        ConfigurarGrilla()
        LimpiarCampos()
        Me.Cursor = Cursors.Default
    End Sub

    Private Async Function CargarDatosAsync() As Task
        Dim repo = _unitOfWork.Repository(Of Nomenclatura)()
        _listaNomenclaturas = Await repo.GetAll().OrderBy(Function(n) n.Nombre).ToListAsync()
        dgvNomenclaturas.DataSource = _listaNomenclaturas
    End Function

    Private Sub ConfigurarGrilla()
        If dgvNomenclaturas.Columns.Count = 0 Then Return

        ' Ocultamos las columnas que no necesitamos en la vista principal
        dgvNomenclaturas.Columns("Id").Visible = False
        dgvNomenclaturas.Columns("Patron").Visible = False
        dgvNomenclaturas.Columns("Ejemplo").Visible = False
        dgvNomenclaturas.Columns("UbicacionArchivo").Visible = False
        dgvNomenclaturas.Columns("Observaciones").Visible = False

        ' Ajustamos los anchos y los títulos
        dgvNomenclaturas.Columns("Nombre").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dgvNomenclaturas.Columns("Codigo").Width = 120
        dgvNomenclaturas.Columns("AreaResponsable").HeaderText = "Área Resp."
        dgvNomenclaturas.Columns("AreaResponsable").Width = 100
        dgvNomenclaturas.Columns("UsaFecha").HeaderText = "Usa Fecha"
        dgvNomenclaturas.Columns("UsaFecha").Width = 80
        dgvNomenclaturas.Columns("UsaNomenclaturaCodigo").HeaderText = "Usa Código"
        dgvNomenclaturas.Columns("UsaNomenclaturaCodigo").Width = 80
    End Sub

    Private Sub dgvNomenclaturas_SelectionChanged(sender As Object, e As EventArgs) Handles dgvNomenclaturas.SelectionChanged
        If dgvNomenclaturas.CurrentRow IsNot Nothing Then
            Dim idSeleccionado = CInt(dgvNomenclaturas.CurrentRow.Cells("Id").Value)
            _nomenclaturaSeleccionada = _listaNomenclaturas.FirstOrDefault(Function(n) n.Id = idSeleccionado)
            MostrarDetalles()
        End If
    End Sub

    Private Sub MostrarDetalles()
        If _nomenclaturaSeleccionada IsNot Nothing Then
            txtNombre.Text = _nomenclaturaSeleccionada.Nombre
            txtCodigo.Text = _nomenclaturaSeleccionada.Codigo
            txtArea.Text = _nomenclaturaSeleccionada.AreaResponsable
            txtPatron.Text = _nomenclaturaSeleccionada.Patron
            txtEjemplo.Text = _nomenclaturaSeleccionada.Ejemplo
            txtUbicacion.Text = _nomenclaturaSeleccionada.UbicacionArchivo
            txtObservaciones.Text = _nomenclaturaSeleccionada.Observaciones
            chkUsaFecha.Checked = _nomenclaturaSeleccionada.UsaFecha
            chkUsaNomenclaturaCodigo.Checked = _nomenclaturaSeleccionada.UsaNomenclaturaCodigo
            btnEliminar.Enabled = True
        Else
            LimpiarCampos()
        End If
    End Sub

    Private Sub LimpiarCampos()
        _nomenclaturaSeleccionada = New Nomenclatura()
        txtNombre.Clear()
        txtCodigo.Clear()
        txtArea.Clear()
        txtPatron.Clear()
        txtEjemplo.Clear()
        txtUbicacion.Clear()
        txtObservaciones.Clear()
        chkUsaFecha.Checked = False
        chkUsaNomenclaturaCodigo.Checked = False
        btnEliminar.Enabled = False
        dgvNomenclaturas.ClearSelection()
        txtNombre.Focus()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        LimpiarCampos()
    End Sub

    Private Async Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) OrElse String.IsNullOrWhiteSpace(txtCodigo.Text) Then
            MessageBox.Show("El Nombre y el Código son obligatorios.", "Datos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        _nomenclaturaSeleccionada.Nombre = txtNombre.Text.Trim()
        _nomenclaturaSeleccionada.Codigo = txtCodigo.Text.Trim()
        _nomenclaturaSeleccionada.AreaResponsable = txtArea.Text.Trim()
        _nomenclaturaSeleccionada.Patron = txtPatron.Text.Trim()
        _nomenclaturaSeleccionada.Ejemplo = txtEjemplo.Text.Trim()
        _nomenclaturaSeleccionada.UbicacionArchivo = txtUbicacion.Text.Trim()
        _nomenclaturaSeleccionada.Observaciones = txtObservaciones.Text.Trim()
        _nomenclaturaSeleccionada.UsaFecha = chkUsaFecha.Checked
        _nomenclaturaSeleccionada.UsaNomenclaturaCodigo = chkUsaNomenclaturaCodigo.Checked

        Me.Cursor = Cursors.WaitCursor
        Dim repo = _unitOfWork.Repository(Of Nomenclatura)()

        If _nomenclaturaSeleccionada.Id = 0 Then ' Es un registro nuevo
            repo.Add(_nomenclaturaSeleccionada)
        Else ' Es una actualización
            repo.Update(_nomenclaturaSeleccionada)
        End If

        ' --- CORRECCIÓN AQUÍ ---
        Await _unitOfWork.CommitAsync()

        Await CargarDatosAsync() ' Recargamos la grilla
        LimpiarCampos()
        Me.Cursor = Cursors.Default
        MessageBox.Show("Datos guardados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Async Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If _nomenclaturaSeleccionada Is Nothing OrElse _nomenclaturaSeleccionada.Id = 0 Then
            MessageBox.Show("Debe seleccionar una nomenclatura para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirmacion = MessageBox.Show($"¿Está seguro de que desea eliminar la nomenclatura '{_nomenclaturaSeleccionada.Nombre}'?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirmacion = DialogResult.Yes Then
            Me.Cursor = Cursors.WaitCursor
            Dim repo = _unitOfWork.Repository(Of Nomenclatura)()
            repo.Remove(_nomenclaturaSeleccionada)

            ' --- CORRECCIÓN AQUÍ ---
            Await _unitOfWork.CommitAsync()

            Await CargarDatosAsync()
            LimpiarCampos()
            Me.Cursor = Cursors.Default
            MessageBox.Show("Nomenclatura eliminada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        Dim textoBusqueda = txtBuscar.Text.Trim().ToUpper()
        If String.IsNullOrEmpty(textoBusqueda) Then
            dgvNomenclaturas.DataSource = _listaNomenclaturas
        Else
            Dim resultado = _listaNomenclaturas.Where(Function(n)
                                                          Return n.Nombre.ToUpper().Contains(textoBusqueda) OrElse
                                                                 n.Codigo.ToUpper().Contains(textoBusqueda) OrElse
                                                                 (n.AreaResponsable IsNot Nothing AndAlso n.AreaResponsable.ToUpper().Contains(textoBusqueda))
                                                      End Function).ToList()
            dgvNomenclaturas.DataSource = resultado
        End If
    End Sub
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
End Class