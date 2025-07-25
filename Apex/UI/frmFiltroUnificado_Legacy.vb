Imports System.Data.Entity
Imports System.Text.RegularExpressions
Imports System.Threading

Public Class frmFiltroUnificado_Legacy

    Private _svc As FuncionarioService
    Private _cts As CancellationTokenSource

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Async Sub frmFiltroUnificado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _svc = New FuncionarioService()
        ConfigurarGrilla()
        AddHandler txtBusqueda.TextChanged, AddressOf OnBusquedaTextChanged
        ' Carga inicial de datos sin filtros
        Await RealizarBusquedaAsync()
    End Sub

    Private Sub ConfigurarGrilla()
        With dgvResultados
            .AutoGenerateColumns = False
            .Columns.Clear()
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "CI", .HeaderText = "CI", .Width = 100})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "Nombre", .HeaderText = "Nombre", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill})
            .Columns.Add(New DataGridViewTextBoxColumn With {.DataPropertyName = "CargoNombre", .HeaderText = "Cargo", .Width = 180})
            .Columns.Add(New DataGridViewCheckBoxColumn With {.DataPropertyName = "Activo", .HeaderText = "Activo", .Width = 80})
        End With
    End Sub

    Private Async Sub OnBusquedaTextChanged(sender As Object, e As EventArgs)
        ' Cancelar la búsqueda anterior si todavía está en curso
        _cts?.Cancel()
        _cts = New CancellationTokenSource()

        Try
            ' Esperar un breve momento después de que el usuario deja de escribir
            Await Task.Delay(300, _cts.Token)
            Await RealizarBusquedaAsync()
        Catch ex As TaskCanceledException
            ' Ignorar, es normal si el usuario escribe rápido
        End Try
    End Sub

    Private Async Function RealizarBusquedaAsync() As Task
        LoadingHelper.MostrarCargando(Me)
        Try
            ' --- INICIO DE LA CORRECCIÓN ---
            ' Deconstruimos la tupla en pasos separados para compatibilidad.
            Dim tuplaParseada = ParsearTextoBusqueda(txtBusqueda.Text)
            Dim textoLibre As String = tuplaParseada.Item1
            Dim filtros As Dictionary(Of String, String) = tuplaParseada.Item2
            ' --- FIN DE LA CORRECCIÓN ---

            ActualizarPildorasUI(filtros)

            Dim resultados = Await _svc.BuscarConFiltrosDinamicosAsync(textoLibre, filtros)
            dgvResultados.DataSource = resultados
            Me.Text = $"Filtro Unificado - {resultados.Count} resultados"

        Catch ex As Exception
            MessageBox.Show($"Ocurrió un error al buscar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            LoadingHelper.OcultarCargando(Me) ' Corregido typo Ocular -> Ocultar
        End Try
    End Function

    ' Esta función devuelve una tupla, que es un tipo de dato que agrupa varios valores.
    Private Function ParsearTextoBusqueda(textoCompleto As String) As (String, Dictionary(Of String, String))
        Dim filtros As New Dictionary(Of String, String)(StringComparer.InvariantCultureIgnoreCase)
        Dim regex As New Regex("(\w+):(?:""([^""]*)""|(\S+))")

        Dim textoLibre = regex.Replace(textoCompleto,
            Function(m)
                Dim clave = m.Groups(1).Value.ToLower()
                Dim valor = If(m.Groups(2).Success, m.Groups(2).Value, m.Groups(3).Value)
                filtros(clave) = valor
                Return ""
            End Function).Trim()

        Return (textoLibre, filtros)
    End Function

#Region "Gestión de Píldoras (Chips) de Filtro"

    Private Sub ActualizarPildorasUI(filtros As Dictionary(Of String, String))
        flpFiltrosActivos.Controls.Clear()
        For Each filtro In filtros
            Dim pildora = CrearPildora(filtro.Key, filtro.Value)
            flpFiltrosActivos.Controls.Add(pildora)
        Next
    End Sub

    Private Function CrearPildora(clave As String, valor As String) As Control
        Dim lbl As New Label With {
            .Text = $"{clave.ToUpperInvariant()}: {valor}",
            .BackColor = Color.LightSkyBlue,
            .ForeColor = Color.Black,
            .Margin = New Padding(3),
            .Padding = New Padding(5, 3, 5, 3),
            .AutoSize = True
        }

        Dim btnCerrar As New Button With {
            .Text = "×",
            .Font = New Font("Segoe UI", 8, FontStyle.Bold),
            .BackColor = Color.LightSkyBlue,
            .ForeColor = Color.DarkRed,
            .FlatStyle = FlatStyle.Flat,
            .Size = New Size(20, lbl.Height),
            .Margin = New Padding(0, 3, 3, 3),
            .Tag = clave
        }
        btnCerrar.FlatAppearance.BorderSize = 0

        AddHandler btnCerrar.Click, AddressOf QuitarFiltro_Click

        Dim panel As New FlowLayoutPanel With {
            .AutoSize = True,
            .Margin = New Padding(2),
            .BackColor = Color.LightSkyBlue
        }

        panel.Controls.Add(lbl)
        panel.Controls.Add(btnCerrar)
        Return panel
    End Function

    Private Sub QuitarFiltro_Click(sender As Object, e As EventArgs)
        Dim btn = CType(sender, Button)
        Dim clave = btn.Tag.ToString()

        Dim regex As New Regex($"{clave}:(?:""[^""]*""|\S+)\s*", RegexOptions.IgnoreCase)
        txtBusqueda.Text = regex.Replace(txtBusqueda.Text, "")

        ' La búsqueda se disparará automáticamente por el evento TextChanged
    End Sub
#End Region

End Class