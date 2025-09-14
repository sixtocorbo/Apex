' Services/TiposEstadoCatalog.vb
Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Data.Entity
Imports System.Globalization
Imports System.Linq
Imports System.Text

Public NotInheritable Class TiposEstadoCatalog
    Private Sub New()
    End Sub

    Private Shared ReadOnly _syncRoot As New Object()
    Private Shared _byName As Dictionary(Of String, Integer)
    Private Shared _allReadOnly As IReadOnlyDictionary(Of String, Integer)
    Private Shared _initialized As Boolean

    ' Init con UoW externo (se comparte contexto si ya estás en una operación)
    Public Shared Sub Init(uow As IUnitOfWork)
        If uow Is Nothing Then Throw New ArgumentNullException(NameOf(uow))
        SyncLock _syncRoot
            If _initialized Then Exit Sub
            LoadFromDb(uow)
            _initialized = True
        End SyncLock
    End Sub

    ' Init cómodo: crea y dispone su propio UnitOfWork
    Public Shared Sub Init()
        Using uow As New UnitOfWork()
            Init(uow)
        End Using
    End Sub

    ' Si agregás tipos nuevos en DB en tiempo de ejecución, podés refrescar.
    Public Shared Sub Refresh(uow As IUnitOfWork)
        If uow Is Nothing Then Throw New ArgumentNullException(NameOf(uow))
        SyncLock _syncRoot
            LoadFromDb(uow)
            _initialized = True
        End SyncLock
    End Sub

    Private Shared Sub LoadFromDb(uow As IUnitOfWork)
        ' GetAll() ya es AsNoTracking, pero explicitamos por claridad
        Dim all = uow.Repository(Of TipoEstadoTransitorio)().
                    GetAll().
                    AsNoTracking().
                    ToList()

        Dim dict As New Dictionary(Of String, Integer)(StringComparer.Ordinal)
        For Each t In all
            If t Is Nothing OrElse String.IsNullOrWhiteSpace(t.Nombre) Then Continue For
            Dim key = Normalize(t.Nombre)
            If Not dict.ContainsKey(key) Then
                dict.Add(key, t.Id)
            End If
        Next

        ' Aliases / sinónimos
        AddAlias(dict, "Orden 5", "Orden Cinco")
        AddAlias(dict, "Orden V", "Orden Cinco")
        AddAlias(dict, "Baja", "Baja de Funcionario")
        AddAlias(dict, "Reactivacion", "Reactivación de Funcionario")
        AddAlias(dict, "Inicio de Proceso", "Inicio de Procesamiento")
        AddAlias(dict, "Procesamiento", "Inicio de Procesamiento")
        AddAlias(dict, "Separacion Cargo", "Separación del Cargo")
        AddAlias(dict, "Reten", "Retén")
        AddAlias(dict, "Licencia Medica", "Enfermedad")
        AddAlias(dict, "Licencia por enfermedad", "Enfermedad")

        _byName = dict
        _allReadOnly = New ReadOnlyDictionary(Of String, Integer)(_byName)
    End Sub

    Private Shared Sub AddAlias(dict As Dictionary(Of String, Integer), aliasName As String, targetName As String)
        Dim aliasKey = Normalize(aliasName)
        Dim targetKey = Normalize(targetName)
        If dict.ContainsKey(targetKey) AndAlso Not dict.ContainsKey(aliasKey) Then
            dict(aliasKey) = dict(targetKey)
        End If
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

    Private Shared Function EnsureInit() As Boolean
        If Not _initialized OrElse _byName Is Nothing Then
            Throw New InvalidOperationException("TiposEstadoCatalog no inicializado. Llamá a Init(uow) o Init() al arrancar.")
        End If
        Return True
    End Function

    Private Shared Function IdDe(nombre As String) As Integer
        EnsureInit()
        Dim key = Normalize(nombre)
        Dim id As Integer
        If _byName.TryGetValue(key, id) Then Return id
        Throw New KeyNotFoundException($"No se encontró TipoEstadoTransitorio '{nombre}'.")
    End Function

    ' API pública extra
    Public Shared Function TryGetId(nombre As String, ByRef id As Integer) As Boolean
        EnsureInit()
        Return _byName.TryGetValue(Normalize(nombre), id)
    End Function

    Public Shared Function GetId(nombre As String) As Integer
        Return IdDe(nombre)
    End Function

    Public Shared ReadOnly Property All As IReadOnlyDictionary(Of String, Integer)
        Get
            EnsureInit()
            Return _allReadOnly
        End Get
    End Property

    ' Propiedades fuertes (sin números mágicos)
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

    Public Shared ReadOnly Property BajaDeFuncionario As Integer
        Get
            Return IdDe("Baja de Funcionario")
        End Get
    End Property

    Public Shared ReadOnly Property CambioDeCargo As Integer
        Get
            Return IdDe("Cambio de Cargo")
        End Get
    End Property

    Public Shared ReadOnly Property ReactivacionDeFuncionario As Integer
        Get
            Return IdDe("Reactivación de Funcionario")
        End Get
    End Property

    Public Shared ReadOnly Property SeparacionDelCargo As Integer
        Get
            Return IdDe("Separación del Cargo")
        End Get
    End Property

    Public Shared ReadOnly Property InicioDeProcesamiento As Integer
        Get
            Return IdDe("Inicio de Procesamiento")
        End Get
    End Property

    Public Shared ReadOnly Property Desarmado As Integer
        Get
            Return IdDe("Desarmado")
        End Get
    End Property
End Class
