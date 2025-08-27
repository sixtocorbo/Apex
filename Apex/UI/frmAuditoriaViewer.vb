Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Data.Entity

Public Class frmAuditoriaViewer

    Private _uow As UnitOfWork
    Private _registrosAuditoria As BindingList(Of AuditoriaCambios)
    Private _idRegistro As String

    Public Sub New(idRegistro As String)
        InitializeComponent()
        _uow = New UnitOfWork()
        _idRegistro = idRegistro
    End Sub

    Private Async Sub frmAuditoriaViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = $"Historial de Cambios para Registro: {_idRegistro}"
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        Await CargarDatosAsync()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Dim query = _uow.Context.Set(Of AuditoriaCambios)().
                        Where(Function(a) a.RegistroId = _idRegistro).
                        OrderByDescending(Function(a) a.FechaHora)

            Dim resultados = Await query.ToListAsync()
            _registrosAuditoria = New BindingList(Of AuditoriaCambios)(resultados)

            dgvAuditoria.DataSource = _registrosAuditoria
            dgvAuditoria.Refresh()

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al cargar el historial: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    Private Sub ConfigurarGrilla()
        With dgvAuditoria
            .SuspendLayout()
            .AutoGenerateColumns = False
            .Columns.Clear()
            .ReadOnly = True
            .AllowUserToAddRows = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .RowHeadersVisible = False

            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "FechaHora", .HeaderText = "Fecha y Hora", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "UsuarioAccion", .HeaderText = "Usuario", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "CampoNombre", .HeaderText = "Campo Modificado", .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "ValorAnterior", .HeaderText = "Valor Anterior", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "ValorNuevo", .HeaderText = "Valor Nuevo", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})

            .ResumeLayout()
        End With
    End Sub

End Class