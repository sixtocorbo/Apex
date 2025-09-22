Option Strict On
Option Explicit On
Option Infer On

Imports System.Data.Entity
Imports System.Data.Entity.Infrastructure

Public Class CategoriaAusenciaService
    Inherits GenericService(Of CategoriaAusencia)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' (Opcional) exponer UoW
    Public ReadOnly Property UnitOfWork As IUnitOfWork
        Get
            Return _unitOfWork
        End Get
    End Property

    ' ----------------- Lectura auxiliar (combos) -----------------
    Public Async Function ObtenerCategoriasParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        ' Con tu Repository: GetAll() -> IQueryable(Of T)
        Dim lista = Await _unitOfWork.Repository(Of CategoriaAusencia)().
            GetAll().AsNoTracking().
            OrderBy(Function(c) c.Nombre).
            Select(Function(c) New With {.Id = c.Id, .Nombre = c.Nombre}).
            ToListAsync()

        Return lista.Select(Function(c) New KeyValuePair(Of Integer, String)(c.Id, c.Nombre)).ToList()
    End Function

    ' ----------------- Chequeo de relaciones -----------------
    Public Async Function TieneUsosAsync(categoriaId As Integer) As Task(Of Boolean)
        ' Usa AnyAsync del Repository con predicado
        Dim hayTiposLicencia = Await _unitOfWork.Repository(Of TipoLicencia)().
            AnyAsync(Function(t) t.CategoriaAusenciaId = categoriaId)

        ' Si tuvieras otras tablas hijas, agregalas con OR (o en consultas separadas)
        Return hayTiposLicencia
    End Function

    ' ----------------- Eliminación segura -----------------
    Public Async Function EliminarAsync(id As Integer) As Task
        ' 1) Pre‐chequeo UX rápido
        If Await TieneUsosAsync(id) Then
            Throw New InvalidOperationException("No se puede eliminar: hay Tipos de Licencia que usan esta categoría.")
        End If

        ' 2) Cargar y marcar para borrar con tu Repository
        Dim repo = _unitOfWork.Repository(Of CategoriaAusencia)()
        Dim entity = repo.GetById(id)
        If entity Is Nothing Then Exit Function

        repo.Remove(entity) ' tu implementación marca Deleted vía Entry.State

        Try
            Await _unitOfWork.CommitAsync()
        Catch ex As DbUpdateException
            ' 3) Segundo cinturón por si otra FK se dispara (concurrencia, etc.)
            Throw New InvalidOperationException(TraducirDbUpdateParaEliminar(ex), ex)
        End Try
    End Function

    ' ----------------- Traducción de errores FK -----------------
    Private Function TraducirDbUpdateParaEliminar(ex As DbUpdateException) As String
        Dim inner As Exception = ex
        While inner IsNot Nothing
            ' Evitamos dependencia directa de System.Data.SqlClient
            If inner.GetType().Name = "SqlException" Then
                Dim numProp = inner.GetType().GetProperty("Number")
                Dim number As Integer =
                    If(numProp IsNot Nothing AndAlso numProp.GetValue(inner, Nothing) IsNot Nothing,
                       CInt(numProp.GetValue(inner, Nothing)), 0)
                If number = 547 Then
                    ' Intento de mensaje más específico si vemos la constraint
                    Dim m = inner.Message
                    If Not String.IsNullOrEmpty(m) AndAlso
                       m.IndexOf("FK_TipoLicencia_CategoriaAusencia", StringComparison.OrdinalIgnoreCase) >= 0 Then
                        Return "No se puede eliminar: hay Tipos de Licencia que usan esta categoría."
                    End If
                    Return "No se puede eliminar: existen registros relacionados que dependen de esta categoría."
                End If
            End If

            ' Heurística por texto (otros motores o providers)
            Dim msg = inner.Message
            If Not String.IsNullOrEmpty(msg) Then
                Dim u = msg.ToUpperInvariant()
                If u.Contains("FOREIGN KEY") OrElse u.Contains("REFERENCE") OrElse u.Contains("CLAVE FORÁNEA") Then
                    Return "No se puede eliminar: este registro está en uso por otros datos relacionados."
                End If
            End If

            inner = inner.InnerException
        End While

        Return "Ocurrió un error al eliminar: " & ex.Message
    End Function
End Class
