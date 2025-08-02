' Apex/Services/AppTheme.vb
Imports System.Drawing
Imports System.Windows.Forms

Public Module AppTheme
    ' --- 1. Paleta de Colores y Fuentes Centralizada ---
    Public ReadOnly ColorPrincipal As Color = Color.FromArgb(0, 123, 255)      ' Azul
    ' --- CORRECCIÓN: Se añade la variable que faltaba ---
    Public ReadOnly ColorPrincipalSuave As Color = Color.FromArgb(220, 235, 255) ' Azul muy claro
    Public ReadOnly ColorFondo As Color = Color.FromArgb(248, 249, 250)         ' Gris muy claro
    Public ReadOnly ColorTexto As Color = Color.FromArgb(33, 37, 41)            ' Casi negro
    Public ReadOnly ColorBorde As Color = Color.FromArgb(222, 226, 230)         ' Gris claro

    Public ReadOnly FontPrincipal As New Font("Segoe UI", 9.0F, FontStyle.Regular)
    Public ReadOnly FontTitulos As New Font("Segoe UI", 10.0F, FontStyle.Bold)

    ''' <summary>
    ''' Aplica el tema de forma recursiva a un formulario y todos sus controles.
    ''' </summary>
    Public Sub Aplicar(frm As Form)
        frm.BackColor = ColorFondo
        frm.Font = FontPrincipal
        frm.ForeColor = ColorTexto
        AplicarRecursivo(frm.Controls)
    End Sub

    Private Sub AplicarRecursivo(controles As Control.ControlCollection)
        For Each ctrl As Control In controles
            ' --- 2. Estilos por Tipo de Control ---
            If TypeOf ctrl Is Panel Then
                ' Paneles de filtros con un fondo ligeramente diferente
                If ctrl.Name.ToLower().Contains("filtros") Then
                    ctrl.BackColor = Color.WhiteSmoke
                Else
                    ctrl.BackColor = ColorFondo
                End If

            ElseIf TypeOf ctrl Is Button Then
                Dim btn = CType(ctrl, Button)
                btn.BackColor = ColorPrincipal
                btn.ForeColor = Color.White
                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderSize = 0
                btn.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)
                AddHandler btn.MouseEnter, Sub(s, e) btn.BackColor = Color.FromArgb(0, 105, 217) ' Efecto Hover
                AddHandler btn.MouseLeave, Sub(s, e) btn.BackColor = ColorPrincipal

            ElseIf TypeOf ctrl Is DataGridView Then
                AplicarEstiloGrilla(CType(ctrl, DataGridView))

            ElseIf TypeOf ctrl Is Label Then
                If ctrl.Name.ToLower().Contains("header") Or ctrl.Name.ToLower().Contains("titulo") Then
                    ctrl.Font = FontTitulos
                End If

            ElseIf TypeOf ctrl Is GroupBox Then
                ctrl.Font = FontTitulos
            End If

            ' --- 3. Llamada Recursiva para Controles Anidados ---
            If ctrl.HasChildren Then
                AplicarRecursivo(ctrl.Controls)
            End If
        Next
    End Sub

    Private Sub AplicarEstiloGrilla(dgv As DataGridView)
        dgv.BackgroundColor = Color.White
        dgv.BorderStyle = BorderStyle.None
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = ColorBorde

        ' Estilo de Cabecera
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorFondo
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = ColorTexto
        dgv.ColumnHeadersDefaultCellStyle.Font = FontTitulos
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5)

        ' Estilo de Filas
        dgv.DefaultCellStyle.BackColor = Color.White
        dgv.DefaultCellStyle.ForeColor = ColorTexto
        dgv.DefaultCellStyle.SelectionBackColor = ColorPrincipalSuave
        dgv.DefaultCellStyle.SelectionForeColor = ColorTexto
        dgv.RowHeadersVisible = False
        dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorFondo
    End Sub
End Module