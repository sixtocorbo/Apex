Public Class frmElegirDesignacion

    Public ReadOnly Property Seleccion As DesignacionSeleccionDTO
        Get
            If dgvDesignaciones.SelectedRows.Count > 0 Then
                Return CType(dgvDesignaciones.SelectedRows(0).DataBoundItem, DesignacionSeleccionDTO)
            End If
            Return Nothing
        End Get
    End Property

    Public Sub New(designaciones As List(Of DesignacionSeleccionDTO))
        InitializeComponent()
        ConfigurarGrilla()
        dgvDesignaciones.DataSource = designaciones
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvDesignaciones
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
            .DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
            .DefaultCellStyle.Padding = New Padding(5, 0, 5, 0)
            .DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 153, 255)
            .DefaultCellStyle.SelectionForeColor = Color.White
            .RowsDefaultCellStyle.BackColor = Color.White
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(242, 245, 247)

            ' --- DEFINICIÓN DE COLUMNAS ---
            .Columns.Clear()

            .Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Descripcion", .DataPropertyName = "Descripcion", .HeaderText = "Designación",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            .MinimumWidth = 250
        })

            Dim chkVigente As New DataGridViewCheckBoxColumn With {
            .Name = "Vigente", .DataPropertyName = "Vigente", .HeaderText = "Vigente",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
            .MinimumWidth = 80
        }
            ' Centrar el checkbox se ve mucho mejor
            chkVigente.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            chkVigente.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns.Add(chkVigente)
        End With
    End Sub

    Private Sub frmElegirDesignacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)

        ' --- APLICAR MEJORAS DE RENDIMIENTO ---
        dgvDesignaciones.ActivarDobleBuffer(True) ' <-- LÍNEA AÑADIDA

        If dgvDesignaciones.Rows.Count > 0 Then
            dgvDesignaciones.Rows(0).Selected = True
        End If
    End Sub

    Private Sub dgvDesignaciones_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDesignaciones.CellDoubleClick
        If e.RowIndex >= 0 Then
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End If
    End Sub

End Class