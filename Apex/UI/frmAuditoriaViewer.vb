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

        ' --- APLICAR MEJORAS DE RENDIMIENTO ---
        dgvAuditoria.ActivarDobleBuffer(True) ' <-- LÍNEA AÑADIDA

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

            ' --- CONFIGURACIÓN GENERAL (Estilo moderno) ---
            .BorderStyle = BorderStyle.None
            .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            .GridColor = Color.FromArgb(230, 230, 230)
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AutoGenerateColumns = False
            .BackgroundColor = Color.White

            ' --- ESTILO DE ENCABEZADOS (Headers) ---
            .EnableHeadersVisualStyles = False
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            .ColumnHeadersHeight = 40
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 41, 56)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
            .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .ColumnHeadersDefaultCellStyle.Padding = New Padding(5, 0, 0, 0)

            ' --- ESTILO DE FILAS (Rows) ---
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.0F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)

            ' --- DEFINICIÓN DE COLUMNAS (Mantenemos las tuyas con mejoras) ---
            .Columns.Clear()

            Dim colFecha As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "FechaHora", .HeaderText = "Fecha y Hora",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 140
        }
            colFecha.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss" ' Formato con hora
            .Columns.Add(colFecha)

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "UsuarioAccion", .HeaderText = "Usuario",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 120
        })
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "CampoNombre", .HeaderText = "Campo Modificado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 150
        })
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "ValorAnterior", .HeaderText = "Valor Anterior",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .FillWeight = 50 ' 50% del espacio
        })
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "ValorNuevo", .HeaderText = "Valor Nuevo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .FillWeight = 50 ' 50% del espacio
        })

            .ResumeLayout()
        End With
    End Sub

    ' --- INICIO DE LA CORRECCIÓN ---
    ''' <summary>
    ''' Este evento se dispara cada vez que se presiona una tecla mientras el formulario tiene el foco.
    ''' </summary>
    Private Sub frmAuditoriaViewer_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape...
        If e.KeyCode = Keys.Escape Then
            ' 1. Marcamos el evento como "manejado".
            '    Esto detiene la propagación de la tecla a otros formularios.
            e.Handled = True
            e.SuppressKeyPress = True ' Evita el sonido "ding" del sistema.

            ' 2. Simplemente cerramos ESTE formulario. El control volverá
            '    de forma natural al formulario del funcionario que lo abrió.
            Me.Close()
        End If
    End Sub
    ' --- FIN DE LA CORRECCIÓN ---
End Class