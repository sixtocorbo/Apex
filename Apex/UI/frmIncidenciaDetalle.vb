Imports System.Data.Entity

Public Class frmIncidenciaDetalle
    Private _uow As IUnitOfWork
    Public Incidencia As TipoLicencia
    Private _modoCreacion As Boolean

    Public Sub New(uow As IUnitOfWork, Optional incidenciaEditable As TipoLicencia = Nothing)
        InitializeComponent()
        _uow = uow

        If incidenciaEditable Is Nothing Then
            ' Modo Creación
            _modoCreacion = True
            Me.Text = "Nueva Incidencia"
            Incidencia = New TipoLicencia()
        Else
            ' Modo Edición
            _modoCreacion = False
            Me.Text = "Editar Incidencia"
            Incidencia = incidenciaEditable
        End If
    End Sub

    Private Async Sub frmIncidenciaDetalle_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await CargarCategorias()
        If Not _modoCreacion Then
            CargarDatos()
            ' IMPEDIR LA EDICIÓN DEL NOMBRE
            txtNombre.ReadOnly = True
        End If
    End Sub

    Private Async Function CargarCategorias() As Task
        Dim repo = _uow.Repository(Of CategoriaAusencia)()
        cboCategoria.DataSource = Await repo.GetAll().ToListAsync()
        cboCategoria.DisplayMember = "Nombre"
        cboCategoria.ValueMember = "Id"
    End Function

    Private Sub CargarDatos()
        txtNombre.Text = Incidencia.Nombre
        chkEsAusencia.Checked = Incidencia.EsAusencia
        chkSuspendeViatico.Checked = Incidencia.SuspendeViatico
        chkAfectaPresentismo.Checked = Incidencia.AfectaPresentismo
        chkEsHabil.Checked = Incidencia.EsHabil

        If cboCategoria.Items.Count > 0 AndAlso Incidencia.CategoriaAusenciaId > 0 Then
            cboCategoria.SelectedValue = Incidencia.CategoriaAusenciaId
        Else
            cboCategoria.SelectedIndex = -1
        End If
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If String.IsNullOrWhiteSpace(txtNombre.Text) Then
            MessageBox.Show("El nombre no puede estar vacío.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If cboCategoria.SelectedIndex = -1 Then
            MessageBox.Show("Debe seleccionar una categoría.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If _modoCreacion Then
            Incidencia.Nombre = txtNombre.Text.Trim()
        End If

        Incidencia.EsAusencia = chkEsAusencia.Checked
        Incidencia.SuspendeViatico = chkSuspendeViatico.Checked
        Incidencia.AfectaPresentismo = chkAfectaPresentismo.Checked
        Incidencia.EsHabil = chkEsHabil.Checked
        Incidencia.CategoriaAusenciaId = CInt(cboCategoria.SelectedValue)

        If _modoCreacion Then
            Incidencia.CreatedAt = DateTime.Now
        Else
            Incidencia.UpdatedAt = DateTime.Now
        End If

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class