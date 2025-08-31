' Apex/Services/AppTheme.vb
Imports System.Drawing
Imports System.Windows.Forms

Public Module AppTheme
    ' --- 1) Paleta ---
    Public ReadOnly ColorPrincipal As Color = Color.FromArgb(0, 123, 255)        ' Azul
    Public ReadOnly ColorPrincipalSuave As Color = Color.FromArgb(220, 235, 255) ' Azul muy claro
    Public ReadOnly ColorFondo As Color = Color.FromArgb(248, 249, 250)          ' Gris muy claro
    Public ReadOnly ColorTexto As Color = Color.FromArgb(33, 37, 41)             ' Casi negro
    Public ReadOnly ColorBorde As Color = Color.FromArgb(222, 226, 230)          ' Gris claro

    Public ReadOnly FontPrincipal As New Font("Segoe UI", 9.0F, FontStyle.Regular)
    Public ReadOnly FontTitulos As New Font("Segoe UI", 10.0F, FontStyle.Bold)

    ' --- 2) Aplicación de tema ---
    Public Sub Aplicar(frm As Form)
        frm.BackColor = ColorFondo
        frm.Font = FontPrincipal
        frm.ForeColor = ColorTexto
        AplicarRecursivo(frm.Controls)
    End Sub

    Private Sub AplicarRecursivo(controles As Control.ControlCollection)
        For Each ctrl As Control In controles

            If TypeOf ctrl Is Panel Then
                If ctrl.Name.ToLower().Contains("filtros") Then
                    ctrl.BackColor = Color.WhiteSmoke
                Else
                    ctrl.BackColor = ColorFondo
                End If

            ElseIf TypeOf ctrl Is Button Then
                Dim thisBtn = DirectCast(ctrl, Button)
                Dim keep As Boolean =
                    (thisBtn.Tag IsNot Nothing AndAlso
                     thisBtn.Tag.ToString().Equals("KeepBackColor", StringComparison.OrdinalIgnoreCase))

                thisBtn.FlatStyle = FlatStyle.Flat
                thisBtn.UseVisualStyleBackColor = False
                thisBtn.FlatAppearance.BorderSize = 0
                thisBtn.Font = New Font("Segoe UI", 9.0F, FontStyle.Bold)

                If keep Then
                    ' Mantener SIEMPRE el mismo fondo (sin hover de relleno)
                    Dim syncHover As Action =
                        Sub()
                            thisBtn.FlatAppearance.MouseOverBackColor = thisBtn.BackColor
                            thisBtn.FlatAppearance.MouseDownBackColor = thisBtn.BackColor
                        End Sub
                    syncHover()
                    AddHandler thisBtn.BackColorChanged, Sub(s, e) syncHover()

                Else
                    ' Botón estándar con hover de relleno
                    thisBtn.BackColor = ColorPrincipal
                    thisBtn.ForeColor = Color.White
                    AddHandler thisBtn.MouseEnter, Sub(s, e) thisBtn.BackColor = Color.FromArgb(0, 105, 217)
                    AddHandler thisBtn.MouseLeave, Sub(s, e) thisBtn.BackColor = ColorPrincipal
                End If

                ' --- Borde reactivo para TODOS los botones ---
                thisBtn.Cursor = Cursors.Hand

                ' Estado base de borde
                thisBtn.FlatAppearance.BorderSize = 1
                thisBtn.FlatAppearance.BorderColor = Darken(thisBtn.BackColor, 0.25F)

                ' Hover
                AddHandler thisBtn.MouseEnter,
                    Sub(s, e)
                        thisBtn.FlatAppearance.BorderSize = 2
                        thisBtn.FlatAppearance.BorderColor = Darken(thisBtn.BackColor, 0.45F)
                    End Sub
                AddHandler thisBtn.MouseLeave,
                    Sub(s, e)
                        thisBtn.FlatAppearance.BorderSize = 1
                        thisBtn.FlatAppearance.BorderColor = Darken(thisBtn.BackColor, 0.25F)
                    End Sub
                ' Focus (teclado)
                AddHandler thisBtn.GotFocus,
                    Sub(s, e)
                        thisBtn.FlatAppearance.BorderSize = 2
                        thisBtn.FlatAppearance.BorderColor = Darken(thisBtn.BackColor, 0.45F)
                    End Sub
                AddHandler thisBtn.LostFocus,
                    Sub(s, e)
                        thisBtn.FlatAppearance.BorderSize = 1
                        thisBtn.FlatAppearance.BorderColor = Darken(thisBtn.BackColor, 0.25F)
                    End Sub
                ' Si cambian el BackColor en runtime, reestablecer borde base
                AddHandler thisBtn.BackColorChanged,
                    Sub(s, e)
                        thisBtn.FlatAppearance.BorderColor = Darken(thisBtn.BackColor, 0.25F)
                    End Sub

            ElseIf TypeOf ctrl Is DataGridView Then
                AplicarEstiloGrilla(DirectCast(ctrl, DataGridView))

            ElseIf TypeOf ctrl Is Label Then
                If ctrl.Name.ToLower().Contains("header") OrElse ctrl.Name.ToLower().Contains("titulo") Then
                    ctrl.Font = FontTitulos
                End If

            ElseIf TypeOf ctrl Is GroupBox Then
                ctrl.Font = FontTitulos
            End If

            ' Recursión
            If ctrl.HasChildren Then
                AplicarRecursivo(ctrl.Controls)
            End If
        Next
    End Sub

    ' --- 3) Utilidad para oscurecer colores ---
    Private Function Darken(c As Color, amount As Single) As Color
        Dim factor As Single = Math.Max(0.0F, Math.Min(1.0F, 1.0F - amount))
        Return Color.FromArgb(c.A,
                              CInt(c.R * factor),
                              CInt(c.G * factor),
                              CInt(c.B * factor))
    End Function

    ' --- 4) Estilo de grillas ---
    Private Sub AplicarEstiloGrilla(dgv As DataGridView)
        dgv.BackgroundColor = Color.White
        dgv.BorderStyle = BorderStyle.None
        dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        dgv.GridColor = ColorBorde

        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorFondo
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = ColorTexto
        dgv.ColumnHeadersDefaultCellStyle.Font = FontTitulos
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(5)

        dgv.DefaultCellStyle.BackColor = Color.White
        dgv.DefaultCellStyle.ForeColor = ColorTexto
        dgv.DefaultCellStyle.SelectionBackColor = ColorPrincipalSuave
        dgv.DefaultCellStyle.SelectionForeColor = ColorTexto
        dgv.RowHeadersVisible = False
        dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorFondo
    End Sub
End Module
