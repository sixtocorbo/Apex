Imports System.IO
Imports System.Data.Entity

Public Class frmMain

    Private ReadOnly _svc As New FuncionarioService()

    '-------------------- Load ------------------------------
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigurarGrilla()        ' ⬅️  solo diseño
        ' NO llamamos a RecargarAsync()
    End Sub


    Private Sub ConfigurarGrilla()
        With dgv
            .AutoGenerateColumns = False
            .RowTemplate.Height = 70
            .RowTemplate.MinimumHeight = 70
            .Columns.Clear()

            ' Id (oculto)
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .DataPropertyName = "Id",
                .HeaderText = "Id",
                .Name = "Id",
                .Visible = False
            })

            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "CI", .HeaderText = "CI", .Name = "CI"})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Nombre", .HeaderText = "Nombre", .Name = "Nombre"})

            ' Foto
            .Columns.Add(New DataGridViewImageColumn With {
                .DataPropertyName = "Foto",
                .HeaderText = "Foto",
                .Name = "Foto",
                .ImageLayout = DataGridViewImageCellLayout.Zoom,
                .Width = 70
            })
        End With

        ' Manejo de imagen segura
        AddHandler dgv.CellFormatting, AddressOf FormatearFoto

        ' Evitar errores por datos nulos
        AddHandler dgv.DataError,
            Sub(sender2, e2) e2.ThrowException = False
    End Sub

    Private Sub FormatearFoto(sender As Object, e As DataGridViewCellFormattingEventArgs)
        If dgv.Columns(e.ColumnIndex).Name <> "Foto" Then Return

        Dim bytes = TryCast(e.Value, Byte())
        If bytes Is Nothing OrElse bytes.Length = 0 Then
            e.Value = My.Resources.Police  ' Imagen predeterminada (recurso)
        Else
            Try
                Using ms As New MemoryStream(bytes)
                    e.Value = Image.FromStream(ms)
                End Using
            Catch
                e.Value = My.Resources.Police
            End Try
        End If
        e.FormattingApplied = True
    End Sub

    '-------------------- Nuevo ----------------------------
    Private Async Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Using frm As New frmFuncionarioCrear
            If frm.ShowDialog() = DialogResult.OK Then
                Await RecargarAsync()
            End If
        End Using
    End Sub

    '-------------------- Editar ---------------------------
    Private Async Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If dgv.CurrentRow Is Nothing Then
            MessageBox.Show("Seleccione un funcionario.", "Aviso")
            Return
        End If

        Dim id = CInt(dgv.CurrentRow.Cells("Id").Value)
        If id <= 0 Then
            MessageBox.Show("Seleccione un funcionario válido.", "Aviso")
            Return
        End If
        Using frm As New frmFuncionarioCrear(id)
            If frm.ShowDialog() = DialogResult.OK Then
                Await RecargarAsync()
            End If
        End Using
    End Sub

    '------------------ Recargar lista ---------------------
    Private Async Function RecargarAsync() As Task
        ' Limpiar la grilla antes de recargar
        BindingSource1.DataSource = Nothing
        Dim lista = Await _svc.GetAllAsync()
        If lista Is Nothing OrElse lista.Count = 0 Then
            MessageBox.Show("No se encontraron funcionarios.", "Aviso")
            Return
        End If
        BindingSource1.DataSource = lista
        dgv.DataSource = BindingSource1
        dgv.ClearSelection()
        dgv.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
        dgv.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells)

    End Function

    '---------------- Buscar en la base ---------------------
    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click

        Using frm As New frmFuncionarioBuscar
            If frm.ShowDialog() = DialogResult.OK Then
                ' El diálogo trae la lista hallada
                Dim lista = frm.ResultadosFiltrados       ' 👈 nueva propiedad
                BindingSource1.DataSource = lista
                dgv.DataSource = BindingSource1
                dgv.ClearSelection()
            End If
        End Using
    End Sub

End Class
