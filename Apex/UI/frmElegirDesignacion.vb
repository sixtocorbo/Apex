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
            .AutoGenerateColumns = False
            .Columns.Clear()
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .Name = "Descripcion",
                .DataPropertyName = "Descripcion",
                .HeaderText = "Designación",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            })
            .Columns.Add(New DataGridViewCheckBoxColumn With {
                .Name = "Vigente",
                .DataPropertyName = "Vigente",
                .HeaderText = "Vigente",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            })
        End With
    End Sub

    Private Sub frmElegirDesignacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
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