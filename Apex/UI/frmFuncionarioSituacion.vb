Imports System.Data.Entity
Imports System.Drawing

Public Class frmFuncionarioSituacion

    Private ReadOnly _funcionarioId As Integer

    Public Sub New(funcionarioId As Integer)
        InitializeComponent()
        _funcionarioId = funcionarioId
    End Sub

    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
        ' --- INICIO DE LA CORRECCIÓN ---
        ' Se agrega el manejador de eventos para el pintado de filas.
        AddHandler dgvSituacion.RowPrePaint, AddressOf dgvSituacion_RowPrePaint
        ' --- FIN DE LA CORRECCIÓN ---
        Await CargarSituacionAsync()
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvSituacion
            .AutoGenerateColumns = False
            .Columns.Clear()
            .RowHeadersVisible = False
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .ReadOnly = True

            .Columns.Add(New DataGridViewTextBoxColumn With {
                .DataPropertyName = "Tipo",
                .HeaderText = "Estado",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
            })
            .Columns.Add(New DataGridViewTextBoxColumn With {
                .DataPropertyName = "Detalles",
                .HeaderText = "Detalles",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            })
        End With
    End Sub

    Private Async Function CargarSituacionAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            Using uow As New UnitOfWork()
                Dim situacion = Await uow.Context.Database.SqlQuery(Of SituacionDTO)(
                    "SELECT Prioridad, Tipo, Detalles, ColorIndicador FROM dbo.vw_FuncionarioSituacionActual WHERE FuncionarioId = @p0 ORDER BY Prioridad",
                    _funcionarioId
                ).ToListAsync()

                dgvSituacion.DataSource = situacion

                ' --- INICIO DE LA CORRECCIÓN ---
                ' El bucle 'For Each' que coloreaba las filas aquí ha sido eliminado.
                ' La lógica ahora está en el evento RowPrePaint.
                ' --- FIN DE LA CORRECCIÓN ---
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar la situación del funcionario: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

    ' --- INICIO DE LA CORRECCIÓN ---
    ' Este nuevo método se encarga de colorear cada fila justo antes de que se muestre.
    Private Sub dgvSituacion_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs)
        If e.RowIndex < 0 OrElse e.RowIndex >= Me.dgvSituacion.Rows.Count Then Return

        Dim row As DataGridViewRow = Me.dgvSituacion.Rows(e.RowIndex)
        Dim dto = TryCast(row.DataBoundItem, SituacionDTO)

        If dto IsNot Nothing AndAlso Not String.IsNullOrEmpty(dto.ColorIndicador) Then
            Try
                Dim backColor = Color.FromName(dto.ColorIndicador)
                row.DefaultCellStyle.BackColor = backColor
                row.DefaultCellStyle.SelectionBackColor = backColor

                ' Ajustar color del texto para mejorar la legibilidad
                If backColor.GetBrightness() < 0.5 Then
                    row.DefaultCellStyle.ForeColor = Color.White
                    row.DefaultCellStyle.SelectionForeColor = Color.White
                Else
                    row.DefaultCellStyle.ForeColor = Color.Black
                    row.DefaultCellStyle.SelectionForeColor = Color.Black
                End If
            Catch ex As Exception
                ' Si el color no es válido, se mantiene el estilo por defecto
            End Try
        End If
    End Sub
    ' --- FIN DE LA CORRECCIÓN ---


    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Me.Close()
    End Sub

    ' DTO para el binding
    Public Class SituacionDTO
        Public Property Prioridad As Integer
        Public Property Tipo As String
        Public Property Detalles As String
        Public Property ColorIndicador As String
    End Class

End Class