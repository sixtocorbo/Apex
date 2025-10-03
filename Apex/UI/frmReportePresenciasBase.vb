Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Threading.Tasks
Imports System.Windows.Forms

Public MustInherit Class frmReportePresenciasBase
    Inherits Form

    Private ReadOnly _funcionarioService As New FuncionarioService()
    Private ReadOnly _rbPresentes As RadioButton
    Private ReadOnly _rbAusentes As RadioButton
    Private ReadOnly _dtpFecha As DateTimePicker
    Private ReadOnly _lblEstado As Label
    Private ReadOnly _bsGrupos As BindingSource
    Private ReadOnly _bsFuncionarios As BindingSource
    Private ReadOnly _dgvGrupos As DataGridView
    Private ReadOnly _dgvFuncionarios As DataGridView
    Private ReadOnly _toolTip As ToolTip
    Private ReadOnly _splitContainer As SplitContainer

    Private _cargando As Boolean
    Private _refrescarPendiente As Boolean
    Private _ignorarCambioFecha As Boolean = True

    Protected Sub New()
        DoubleBuffered = True

        _rbPresentes = New RadioButton() With {
            .Text = "Presentes",
            .AutoSize = True,
            .Checked = True
        }

        _rbAusentes = New RadioButton() With {
            .Text = "Ausentes",
            .AutoSize = True,
            .Margin = New Padding(18, 0, 0, 0)
        }

        _dtpFecha = New DateTimePicker() With {
            .Format = DateTimePickerFormat.Custom,
            .CustomFormat = "dddd dd 'de' MMMM yyyy",
            .Width = 260
        }

        _lblEstado = New Label() With {
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleRight,
            .AutoSize = False,
            .ForeColor = SystemColors.GrayText
        }

        _bsGrupos = New BindingSource() With {
            .DataSource = GetType(PresenciaAgrupada)
        }

        _bsFuncionarios = New BindingSource() With {
            .DataSource = GetType(PresenciaFuncionarioDetalle)
        }

        _dgvGrupos = New DataGridView()
        _dgvFuncionarios = New DataGridView()
        _toolTip = New ToolTip()
        _splitContainer = New SplitContainer()

        InitializeComponent()
    End Sub

    Protected MustOverride ReadOnly Property Agrupacion As AgrupacionPresencia
    Protected MustOverride ReadOnly Property TituloFormulario As String
    Protected MustOverride ReadOnly Property EtiquetaGrupo As String

    Protected Overridable ReadOnly Property TooltipAusentes As String
        Get
            Return "Funcionarios activos sin marcar como presentes en la fecha consultada."
        End Get
    End Property

    Protected Overridable Function ObtenerFechaConsulta() As Date
        If _dtpFecha IsNot Nothing Then
            Return _dtpFecha.Value.Date
        End If

        Return Date.Today
    End Function

    Protected Overridable Function ObtenerFechaInicial() As Date
        Return Date.Today
    End Function

    Private Sub InitializeComponent()
        SuspendLayout()

        Dim panelSuperior As New TableLayoutPanel() With {
            .ColumnCount = 2,
            .Dock = DockStyle.Top,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .Padding = New Padding(12, 12, 12, 6)
        }
        panelSuperior.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        panelSuperior.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))

        Dim panelFiltros As New FlowLayoutPanel() With {
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.LeftToRight,
            .Margin = New Padding(0)
        }
        Dim lblFecha As New Label() With {
            .AutoSize = True,
            .Text = "Fecha:",
            .Margin = New Padding(0, 6, 8, 0)
        }

        _dtpFecha.Margin = New Padding(0, 0, 18, 0)

        panelFiltros.Controls.Add(lblFecha)
        panelFiltros.Controls.Add(_dtpFecha)
        panelFiltros.Controls.Add(_rbPresentes)
        panelFiltros.Controls.Add(_rbAusentes)

        panelSuperior.Controls.Add(panelFiltros, 0, 0)
        panelSuperior.Controls.Add(_lblEstado, 1, 0)

        ConfigurarDataGridView(_dgvGrupos)
        _dgvGrupos.AutoGenerateColumns = False
        _dgvGrupos.MultiSelect = False
        _dgvGrupos.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        _dgvGrupos.RowHeadersVisible = False
        _dgvGrupos.DataSource = _bsGrupos
        _dgvGrupos.Columns.Add(New DataGridViewTextBoxColumn() With {
            .DataPropertyName = NameOf(PresenciaAgrupada.Grupo),
            .HeaderText = EtiquetaGrupo,
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
        _dgvGrupos.Columns.Add(New DataGridViewTextBoxColumn() With {
            .DataPropertyName = NameOf(PresenciaAgrupada.Cantidad),
            .HeaderText = "Cantidad",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        })

        ConfigurarDataGridView(_dgvFuncionarios)
        _dgvFuncionarios.AutoGenerateColumns = False
        _dgvFuncionarios.MultiSelect = False
        _dgvFuncionarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        _dgvFuncionarios.RowHeadersVisible = False
        _dgvFuncionarios.DataSource = _bsFuncionarios
        _dgvFuncionarios.Columns.Add(New DataGridViewTextBoxColumn() With {
            .DataPropertyName = NameOf(PresenciaFuncionarioDetalle.Nombre),
            .HeaderText = "Funcionario",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        })
        _dgvFuncionarios.Columns.Add(New DataGridViewTextBoxColumn() With {
            .DataPropertyName = NameOf(PresenciaFuncionarioDetalle.Seccion),
            .HeaderText = "Sección",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        })
        _dgvFuncionarios.Columns.Add(New DataGridViewTextBoxColumn() With {
            .DataPropertyName = NameOf(PresenciaFuncionarioDetalle.PuestoTrabajo),
            .HeaderText = "Puesto de trabajo",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        })
        _dgvFuncionarios.Columns.Add(New DataGridViewTextBoxColumn() With {
            .DataPropertyName = NameOf(PresenciaFuncionarioDetalle.Estado),
            .HeaderText = "Estado reportado",
            .AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        })

        _splitContainer.Dock = DockStyle.Fill
        _splitContainer.Orientation = Orientation.Vertical
        _splitContainer.SplitterDistance = 280
        _splitContainer.Panel1.Controls.Add(_dgvGrupos)
        _splitContainer.Panel2.Controls.Add(_dgvFuncionarios)

        Controls.Add(_splitContainer)
        Controls.Add(panelSuperior)

        Font = New Font("Segoe UI", 9.0F, FontStyle.Regular, GraphicsUnit.Point)
        Text = TituloFormulario
        MinimumSize = New Size(820, 540)
        ClientSize = New Size(1024, 640)
        StartPosition = FormStartPosition.CenterParent

        AddHandler Load, AddressOf frmReportePresenciasBase_Load
        AddHandler _rbPresentes.CheckedChanged, AddressOf Filtro_CheckedChanged
        AddHandler _rbAusentes.CheckedChanged, AddressOf Filtro_CheckedChanged
        AddHandler _bsGrupos.CurrentChanged, AddressOf bsGrupos_CurrentChanged
        AddHandler _dgvGrupos.SelectionChanged, AddressOf dgvGrupos_SelectionChanged
        AddHandler _dtpFecha.ValueChanged, AddressOf dtpFecha_ValueChanged

        _toolTip.SetToolTip(_dtpFecha, "Seleccione la fecha a consultar")
        _toolTip.SetToolTip(_rbAusentes, TooltipAusentes)

        ResumeLayout(False)
        PerformLayout()
    End Sub

    Private Sub ConfigurarDataGridView(grid As DataGridView)
        With grid
            .Dock = DockStyle.Fill
            .ReadOnly = True
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .BackgroundColor = Color.White
            .BorderStyle = BorderStyle.None
        End With
    End Sub

    Private Async Sub frmReportePresenciasBase_Load(sender As Object, e As EventArgs)
        Try
            AppTheme.Aplicar(Me)
        Catch
        End Try

        If _dtpFecha IsNot Nothing Then
            _ignorarCambioFecha = True

            Dim fechaInicial = ObtenerFechaInicial()

            Try
                If fechaInicial < _dtpFecha.MinDate Then
                    _dtpFecha.MinDate = fechaInicial
                End If

                If fechaInicial > _dtpFecha.MaxDate Then
                    _dtpFecha.MaxDate = fechaInicial
                End If

                _dtpFecha.Value = fechaInicial
            Catch
                _dtpFecha.Value = Date.Today
            End Try
        End If

        Try
            Await CargarDatosAsync()
        Finally
            _ignorarCambioFecha = False
        End Try
    End Sub

    Private Async Sub Filtro_CheckedChanged(sender As Object, e As EventArgs)
        Dim rb = TryCast(sender, RadioButton)
        If rb Is Nothing OrElse Not rb.Checked Then Return
        Await CargarDatosAsync()
    End Sub

    Private Async Sub dtpFecha_ValueChanged(sender As Object, e As EventArgs)
        If _ignorarCambioFecha Then
            Return
        End If

        Await CargarDatosAsync()
    End Sub

    Private Async Function CargarDatosAsync() As Task
        If _cargando Then
            _refrescarPendiente = True
            Return
        End If

        _cargando = True
        Cursor = Cursors.WaitCursor
        _rbPresentes.Enabled = False
        _rbAusentes.Enabled = False
        _lblEstado.Text = "Cargando datos..."
        _lblEstado.ForeColor = SystemColors.GrayText

        Try
            Dim filtroSeleccionado = If(_rbPresentes.Checked, FiltroPresencia.Presentes, FiltroPresencia.Ausentes)
            Dim fecha = ObtenerFechaConsulta()
            Dim datos = Await _funcionarioService.ObtenerPresenciasAgrupadasAsync(fecha, filtroSeleccionado, Agrupacion)
            If datos Is Nothing Then
                datos = New List(Of PresenciaAgrupada)()
            End If

            _bsGrupos.DataSource = New BindingList(Of PresenciaAgrupada)(datos)

            If _dgvGrupos.Rows.Count > 0 Then
                _dgvGrupos.ClearSelection()
                _dgvGrupos.Rows(0).Selected = True
            End If

            ActualizarDetalle()

            Dim total = datos.Sum(Function(d) d.Cantidad)
            If total > 0 Then
                _lblEstado.Text = String.Format("{0} {1} el {2:d}", total, If(filtroSeleccionado = FiltroPresencia.Presentes, "presentes", "ausentes"), fecha)
                _lblEstado.ForeColor = Color.FromArgb(33, 115, 70)
            Else
                _lblEstado.Text = "Sin datos para la selección actual."
                _lblEstado.ForeColor = SystemColors.GrayText
            End If
        Catch ex As Exception
            _lblEstado.Text = "Error al cargar datos: " & ex.Message
            _lblEstado.ForeColor = Color.Firebrick
            _bsGrupos.DataSource = New BindingList(Of PresenciaAgrupada)()
            _bsFuncionarios.DataSource = New BindingList(Of PresenciaFuncionarioDetalle)()
        Finally
            _rbPresentes.Enabled = True
            _rbAusentes.Enabled = True
            Cursor = Cursors.Default
            _cargando = False
        End Try

        If _refrescarPendiente Then
            _refrescarPendiente = False
            Await CargarDatosAsync()
        End If
    End Function

    Private Sub bsGrupos_CurrentChanged(sender As Object, e As EventArgs)
        ActualizarDetalle()
    End Sub

    Private Sub dgvGrupos_SelectionChanged(sender As Object, e As EventArgs)
        If _dgvGrupos.Focused AndAlso _dgvGrupos.CurrentRow IsNot Nothing Then
            _bsGrupos.Position = _dgvGrupos.CurrentRow.Index
        End If
    End Sub

    Private Sub ActualizarDetalle()
        Dim seleccionado = TryCast(_bsGrupos.Current, PresenciaAgrupada)
        If seleccionado Is Nothing Then
            _bsFuncionarios.DataSource = New BindingList(Of PresenciaFuncionarioDetalle)()
            Return
        End If

        _bsFuncionarios.DataSource = New BindingList(Of PresenciaFuncionarioDetalle)(seleccionado.Funcionarios)
        If _dgvFuncionarios.Rows.Count > 0 Then
            _dgvFuncionarios.ClearSelection()
            _dgvFuncionarios.Rows(0).Selected = True
        End If
    End Sub
End Class
