' Apex/ModernUI/Theme.vb
Imports System.Drawing
Imports System.Windows.Forms

Public Module ModernUI
    ' Paleta de Colores
    Public ReadOnly ColorPrincipal As Color = Color.FromArgb(0, 123, 255) ' Azul vibrante
        Public ReadOnly ColorPrincipalSuave As Color = Color.FromArgb(220, 235, 255) ' Azul muy claro
        Public ReadOnly ColorFondo As Color = Color.White
        Public ReadOnly ColorFondoPanel As Color = Color.FromArgb(248, 249, 250) ' Gris muy claro
        Public ReadOnly ColorTextoPrincipal As Color = Color.FromArgb(33, 37, 41) ' Casi negro
        Public ReadOnly ColorTextoSecundario As Color = Color.FromArgb(108, 117, 125) ' Gris oscuro

        ' Fuentes
        Public ReadOnly FontPrincipal As New Font("Segoe UI", 9.0F, FontStyle.Regular)
        Public ReadOnly FontTitulos As New Font("Segoe UI", 10.0F, FontStyle.Bold)

        Public Sub Aplicar(frm As Form)
            frm.BackColor = ColorFondo
            frm.Font = FontPrincipal
            frm.ForeColor = ColorTextoPrincipal

            AplicarRecursivo(frm.Controls)
        End Sub

        Private Sub AplicarRecursivo(controles As Control.ControlCollection)
            For Each ctrl As Control In controles
                If TypeOf ctrl Is Panel Then
                    CType(ctrl, Panel).BackColor = If(ctrl.Name = "pnlFiltros", ColorFondoPanel, ColorFondo)
                ElseIf TypeOf ctrl Is Button Then
                    Dim btn = CType(ctrl, Button)
                    btn.BackColor = ColorPrincipal
                    btn.ForeColor = Color.White
                    btn.FlatStyle = FlatStyle.Flat
                    btn.FlatAppearance.BorderSize = 0
                    btn.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
                    ' Efecto Hover
                    AddHandler btn.MouseEnter, Sub(s, e) btn.BackColor = Color.FromArgb(0, 105, 217)
                    AddHandler btn.MouseLeave, Sub(s, e) btn.BackColor = ColorPrincipal
                ElseIf TypeOf ctrl Is DataGridView Then
                    AplicarEstiloGrilla(CType(ctrl, DataGridView))
                ElseIf TypeOf ctrl Is Label Then
                    ctrl.Font = If(ctrl.Name.Contains("Titulo"), FontTitulos, FontPrincipal)
                End If

                If ctrl.HasChildren Then
                    AplicarRecursivo(ctrl.Controls)
                End If
            Next
        End Sub

    Private Sub AplicarEstiloGrilla(dgv As DataGridView)
        dgv.BackgroundColor = ColorFondo
        dgv.BorderStyle = BorderStyle.None
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = Color.FromArgb(233, 236, 239)

        ' Estilo de Cabecera
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorFondo
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = ColorTextoPrincipal
        dgv.ColumnHeadersDefaultCellStyle.Font = FontTitulos

        ' Estilo de Filas
        dgv.DefaultCellStyle.BackColor = ColorFondo
        dgv.DefaultCellStyle.ForeColor = ColorTextoSecundario
        dgv.DefaultCellStyle.SelectionBackColor = ColorPrincipalSuave
        dgv.DefaultCellStyle.SelectionForeColor = ColorTextoPrincipal
        dgv.RowHeadersVisible = False
        dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorFondoPanel
    End Sub
End Module