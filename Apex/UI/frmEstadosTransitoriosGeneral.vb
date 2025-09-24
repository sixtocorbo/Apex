Public Class frmEstadosTransitoriosGeneral
    Private _unitOfWork As IUnitOfWork

    Public Sub New(uow As IUnitOfWork)
        InitializeComponent()
        _unitOfWork = uow
    End Sub

    Private Sub frmEstadosTransitoriosGeneral_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' --- APLICAR MEJORAS ---
        dgvEstados.ActivarDobleBuffer(True) ' <-- LÍNEA AÑADIDA
        ConfigurarGrilla()                  ' <-- MOVIMOS ESTA LÍNEA AQUÍ

        Try
            AppTheme.SetCue(txtFiltro, "Filtrar por nombre o CI del funcionario...")
        Catch
            ' Ignorar si no existe SetCue
        End Try
        CargarDatos() ' Ahora solo carga los datos
    End Sub

    Private Sub CargarDatos()
        Dim repo = _unitOfWork.Repository(Of vw_EstadosTransitoriosCompletos)()
        Dim estados = repo.GetAll().ToList()
        dgvEstados.DataSource = estados
        ' ConfigurarGrilla() <-- QUITAMOS ESTA LÍNEA DE AQUÍ
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvEstados
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
            .AutoGenerateColumns = False ' <-- Clave: Desactivamos la autogeneración
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

            ' --- DEFINICIÓN MANUAL DE COLUMNAS (Más robusto) ---
            .Columns.Clear()

            ' Columnas Ocultas
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Id", .Visible = False})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "FuncionarioId", .Visible = False})

            ' Columnas Visibles
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "NombreFuncionario", .HeaderText = "Funcionario",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .FillWeight = 40
        })
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "CI", .HeaderText = "Cédula",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 90
        })
            ' Nota: Asegúrate que el DataPropertyName coincida con tu vista (ej: TipoEstadoNombre)
            .Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "TipoEstadoNombre", .HeaderText = "Tipo de Estado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .FillWeight = 60
        })

            Dim colDesde As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "FechaDesde", .HeaderText = "Fecha Desde",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 110
        }
            colDesde.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colDesde)

            Dim colHasta As New DataGridViewTextBoxColumn With {
            .DataPropertyName = "FechaHasta", .HeaderText = "Fecha Hasta",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells, .MinimumWidth = 110
        }
            colHasta.DefaultCellStyle.Format = "dd/MM/yyyy"
            .Columns.Add(colHasta)
        End With
    End Sub

    Private Sub txtFiltro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        FiltrarDatos()
    End Sub

    Private Sub FiltrarDatos()
        Dim filtro = txtFiltro.Text.ToLower()
        Dim repo = _unitOfWork.Repository(Of vw_EstadosTransitoriosCompletos)()
        Dim estados = repo.GetAll().Where(Function(e) e.NombreFuncionario.ToLower().Contains(filtro) Or e.CI.Contains(filtro)).ToList()
        dgvEstados.DataSource = estados
    End Sub

    Private Sub btnVerDetalles_Click(sender As Object, e As EventArgs) Handles btnVerDetalles.Click
        If dgvEstados.CurrentRow Is Nothing Then Return

        Dim estadoId = CInt(dgvEstados.CurrentRow.Cells("Id").Value)
        Dim estadoRepo = _unitOfWork.Repository(Of EstadoTransitorio)()
        Dim estado = estadoRepo.GetById(estadoId)

        If estado IsNot Nothing Then
            Using frm As New frmFuncionarioEstadoTransitorio(estado, _unitOfWork, True)
                frm.ShowDialog()
            End Using
        Else
            MessageBox.Show("No se pudo encontrar el estado seleccionado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub Cerrando(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Si la tecla presionada es Escape, se cierra el formulario.
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

End Class