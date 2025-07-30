' --- 1. Define una clase Theme (en un módulo aparte) ---
Public Module ModernTheme
    Public ReadOnly Primario As Color = Color.FromArgb(33, 150, 243)
    Public ReadOnly FondoClaro As Color = Color.FromArgb(250, 250, 250)

    Public Sub Aplicar(form As frmFiltroAvanzado)
        form.BackColor = FondoClaro
        For Each ctrl In form.Controls.OfType(Of Control)()
            AplicarA(ctrl)
        Next

        ' DataGridView principal
        form.dgvDatos.GridColor = Color.Gainsboro
        form.dgvDatos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        form.dgvDatos.ColumnHeadersDefaultCellStyle.BackColor = Primario
        form.dgvDatos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        form.dgvDatos.EnableHeadersVisualStyles = False
        form.dgvDatos.RowHeadersVisible = False
    End Sub

    Private Sub AplicarA(ctrl As Control)
        If TypeOf ctrl Is Button Then
            With DirectCast(ctrl, Button)
                .FlatStyle = FlatStyle.Flat
                .FlatAppearance.BorderSize = 0
                .BackColor = Primario
                .ForeColor = Color.White
                .Font = New Font("Segoe UI", 9, FontStyle.Bold)
                .Height = 32
            End With
        ElseIf TypeOf ctrl Is Label Then
            ctrl.ForeColor = Color.FromArgb(33, 33, 33)
            ctrl.Font = New Font("Segoe UI", 9)
        End If

        ' Recorrer hijos
        For Each hijo In ctrl.Controls.OfType(Of Control)()
            AplicarA(hijo)
        Next
    End Sub
End Module
