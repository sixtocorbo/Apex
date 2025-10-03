Imports System.Collections.Generic
Imports System.Linq

Public Class frmFusionarFunciones

    Private ReadOnly _funciones As List(Of Funcion)
    Private _ultimaSeleccionDestinoId As Integer
    Private _nombreEditadoManual As Boolean
    Private _cambiandoNombreProgramaticamente As Boolean

    Private _funcionesSeleccionadasIds As List(Of Integer)
    Private _funcionDestinoId As Integer
    Private _nombreDestino As String

    Public Sub New(funciones As IEnumerable(Of Funcion))
        InitializeComponent()

        If funciones Is Nothing Then Throw New ArgumentNullException(NameOf(funciones))

        _funciones = funciones.
            Where(Function(f) f IsNot Nothing).
            Select(Function(f) New Funcion With {.Id = f.Id, .Nombre = f.Nombre}).
            OrderBy(Function(f) f.Nombre).
            ToList()

        clbFunciones.DisplayMember = NameOf(Funcion.Nombre)
        cboFuncionPrincipal.DisplayMember = NameOf(Funcion.Nombre)
        cboFuncionPrincipal.ValueMember = NameOf(Funcion.Id)
    End Sub

    Public ReadOnly Property FuncionesSeleccionadasIds As List(Of Integer)
        Get
            Return If(_funcionesSeleccionadasIds, New List(Of Integer)())
        End Get
    End Property

    Public ReadOnly Property FuncionDestinoId As Integer
        Get
            Return _funcionDestinoId
        End Get
    End Property

    Public ReadOnly Property NombreDestino As String
        Get
            Return _nombreDestino
        End Get
    End Property

    Private Sub frmFusionarFunciones_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        CargarFunciones()
    End Sub

    Private Sub CargarFunciones()
        Dim filtro = txtBuscar.Text.Trim()
        Dim idsMarcados = New HashSet(Of Integer)(
            clbFunciones.CheckedItems.Cast(Of Object)().
                Select(Function(o) CType(o, Funcion).Id))

        clbFunciones.BeginUpdate()
        clbFunciones.Items.Clear()

        Dim listado = _funciones.AsEnumerable()
        If Not String.IsNullOrWhiteSpace(filtro) Then
            listado = listado.Where(Function(f) f.Nombre IsNot Nothing AndAlso f.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0)
        End If

        For Each funcion In listado
            Dim estaMarcada = idsMarcados.Contains(funcion.Id)
            clbFunciones.Items.Add(funcion, estaMarcada)
        Next

        clbFunciones.EndUpdate()
        ActualizarComboPrincipal()
    End Sub

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        CargarFunciones()
    End Sub

    Private Sub clbFunciones_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles clbFunciones.ItemCheck
        BeginInvoke(CType(Sub()
                              ActualizarComboPrincipal()
                          End Sub, Action))
    End Sub

    Private Sub ActualizarComboPrincipal()
        Dim seleccionadas = clbFunciones.CheckedItems.Cast(Of Object)().
            Select(Function(o) CType(o, Funcion)).
            OrderBy(Function(f) f.Nombre).
            ToList()

        Dim destinoActual As Integer? = Nothing
        If cboFuncionPrincipal.SelectedItem IsNot Nothing Then
            destinoActual = CType(cboFuncionPrincipal.SelectedValue, Integer)
        ElseIf _ultimaSeleccionDestinoId > 0 Then
            destinoActual = _ultimaSeleccionDestinoId
        End If

        cboFuncionPrincipal.DataSource = Nothing
        cboFuncionPrincipal.Enabled = seleccionadas.Count > 0

        If seleccionadas.Count = 0 Then
            EstablecerNombreFinal(String.Empty)
            Return
        End If

        cboFuncionPrincipal.DataSource = seleccionadas
        cboFuncionPrincipal.DisplayMember = NameOf(Funcion.Nombre)
        cboFuncionPrincipal.ValueMember = NameOf(Funcion.Id)

        Dim indiceSeleccion = -1
        If destinoActual.HasValue Then
            indiceSeleccion = seleccionadas.FindIndex(Function(f) f.Id = destinoActual.Value)
        End If

        If indiceSeleccion >= 0 Then
            cboFuncionPrincipal.SelectedIndex = indiceSeleccion
        Else
            cboFuncionPrincipal.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboFuncionPrincipal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFuncionPrincipal.SelectedIndexChanged
        If cboFuncionPrincipal.SelectedItem Is Nothing Then
            Return
        End If

        Dim funcion = CType(cboFuncionPrincipal.SelectedItem, Funcion)
        _ultimaSeleccionDestinoId = funcion.Id

        If Not _nombreEditadoManual Then
            EstablecerNombreFinal(funcion.Nombre)
        End If
    End Sub

    Private Sub EstablecerNombreFinal(nombre As String)
        _cambiandoNombreProgramaticamente = True
        txtNombreFinal.Text = If(nombre, String.Empty)
        _cambiandoNombreProgramaticamente = False
        _nombreEditadoManual = False
    End Sub

    Private Sub txtNombreFinal_TextChanged(sender As Object, e As EventArgs) Handles txtNombreFinal.TextChanged
        If _cambiandoNombreProgramaticamente Then
            Return
        End If
        _nombreEditadoManual = True
    End Sub

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Dim seleccion = clbFunciones.CheckedItems.Cast(Of Object)().
            Select(Function(o) CType(o, Funcion).Id).
            Distinct().
            ToList()

        If seleccion.Count < 2 Then
            Notifier.Warn(Me, "Debe seleccionar al menos dos funciones para fusionar.")
            Return
        End If

        If cboFuncionPrincipal.SelectedItem Is Nothing Then
            Notifier.Warn(Me, "Debe indicar cuál función quedará como destino.")
            Return
        End If

        Dim destino = CType(cboFuncionPrincipal.SelectedValue, Integer)
        If Not seleccion.Contains(destino) Then
            Notifier.Warn(Me, "La función seleccionada como destino debe estar dentro de la lista marcada.")
            Return
        End If

        Dim nombre = txtNombreFinal.Text.Trim()
        If String.IsNullOrWhiteSpace(nombre) Then
            Notifier.Warn(Me, "El nombre final no puede estar vacío.")
            txtNombreFinal.Focus()
            Return
        End If

        _funcionesSeleccionadasIds = seleccion
        _funcionDestinoId = destino
        _nombreDestino = nombre

        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = DialogResult.Cancel
    End Sub

End Class
