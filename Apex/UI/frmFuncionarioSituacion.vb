Imports System.Data.Entity

Public Class frmFuncionarioSituacion

    Private ReadOnly _funcionarioId As Integer

    Public Sub New(funcionarioId As Integer)
        InitializeComponent()
        _funcionarioId = funcionarioId
    End Sub

    Private Async Sub frmFuncionarioSituacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AppTheme.Aplicar(Me)
        ConfigurarGrilla()
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

                ' Colorear filas
                For Each row As DataGridViewRow In dgvSituacion.Rows
                    Dim dto = TryCast(row.DataBoundItem, SituacionDTO)
                    If dto IsNot Nothing AndAlso Not String.IsNullOrEmpty(dto.ColorIndicador) Then
                        Try
                            row.DefaultCellStyle.BackColor = Color.FromName(dto.ColorIndicador)
                            row.DefaultCellStyle.ForeColor = Color.White
                        Catch ex As Exception
                            ' Color no válido, se ignora
                        End Try
                    End If
                Next
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar la situación del funcionario: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me)
        End Try
    End Function

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