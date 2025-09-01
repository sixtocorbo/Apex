' Services/TiposEstadoCatalog.vb
Option Strict On
Option Explicit On
Imports System.Text
Imports System.Globalization

Public NotInheritable Class TiposEstadoCatalog
    Private Sub New()
    End Sub

    Private Shared _byName As Dictionary(Of String, Integer)
    Private Shared _initialized As Boolean

    ' Llamalo una sola vez al arrancar (p. ej. en frmMain/frmDashboard).
    Public Shared Sub Init(uow As IUnitOfWork)
        If _initialized Then Exit Sub
        Dim all = uow.Repository(Of TipoEstadoTransitorio)().GetAll().ToList()
        _byName = all.ToDictionary(Function(t) Normalize(t.Nombre), Function(t) t.Id)
        _initialized = True
    End Sub

    Private Shared Function Normalize(s As String) As String
        If String.IsNullOrWhiteSpace(s) Then Return String.Empty
        Dim t = s.Trim().ToUpperInvariant().Normalize(NormalizationForm.FormD)
        Dim sb As New StringBuilder(t.Length)
        For Each ch In t
            If CharUnicodeInfo.GetUnicodeCategory(ch) <> UnicodeCategory.NonSpacingMark Then
                sb.Append(ch)
            End If
        Next
        Return sb.ToString().Normalize(NormalizationForm.FormC)
    End Function

    Private Shared Function IdDe(nombre As String) As Integer
        If Not _initialized Then
            Throw New InvalidOperationException("TiposEstadoCatalog no inicializado. Llamá a Init(uow) al arrancar.")
        End If
        Dim key = Normalize(nombre)
        Dim id As Integer
        If _byName.TryGetValue(key, id) Then Return id
        Throw New KeyNotFoundException($"No se encontró TipoEstadoTransitorio '{nombre}'.")
    End Function

    ' ---- Propiedades fuertes (sin magic numbers) ----
    Public Shared ReadOnly Property Designacion As Integer
        Get
            Return IdDe("Designación")
        End Get
    End Property

    Public Shared ReadOnly Property Enfermedad As Integer
        Get
            Return IdDe("Enfermedad")
        End Get
    End Property

    Public Shared ReadOnly Property Sancion As Integer
        Get
            Return IdDe("Sanción")
        End Get
    End Property

    Public Shared ReadOnly Property OrdenCinco As Integer
        Get
            Return IdDe("Orden Cinco")
        End Get
    End Property

    Public Shared ReadOnly Property Reten As Integer
        Get
            Return IdDe("Retén")
        End Get
    End Property

    Public Shared ReadOnly Property Sumario As Integer
        Get
            Return IdDe("Sumario")
        End Get
    End Property

    Public Shared ReadOnly Property Traslado As Integer
        Get
            Return IdDe("Traslado")
        End Get
    End Property

    ' Agregá aquí el resto (Baja, Cambio de Cargo, etc.) si corresponde.
End Class
