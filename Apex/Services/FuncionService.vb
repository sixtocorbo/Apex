Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Linq

Public Class FuncionService
    Inherits GenericService(Of Funcion)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    Public Async Function ObtenerFuncionesParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await _unitOfWork.Repository(Of Funcion)().
            GetAll().
            AsNoTracking().
            OrderBy(Function(f) f.Nombre).
            Select(Function(f) New With {.Id = f.Id, .Nombre = f.Nombre}).
            ToListAsync()

        Return lista.Select(Function(x) New KeyValuePair(Of Integer, String)(x.Id, x.Nombre)).ToList()
    End Function

    Public Async Function FusionarFuncionesAsync(funcionDestinoId As Integer,
                                                 funcionesSeleccionadasIds As IEnumerable(Of Integer),
                                                 nombreDestino As String) As Task

        If funcionesSeleccionadasIds Is Nothing Then
            Throw New ArgumentNullException(NameOf(funcionesSeleccionadasIds))
        End If

        Dim idsSeleccionados = funcionesSeleccionadasIds.Where(Function(id) id > 0).
            Distinct().
            ToList()

        If idsSeleccionados.Count = 0 Then
            Throw New ArgumentException("Debe indicar las funciones a fusionar.", NameOf(funcionesSeleccionadasIds))
        End If

        If Not idsSeleccionados.Contains(funcionDestinoId) Then
            Throw New ArgumentException("La función destino debe formar parte de la selección.", NameOf(funcionDestinoId))
        End If

        Dim nombreNormalizado = If(nombreDestino, String.Empty).Trim()
        If String.IsNullOrWhiteSpace(nombreNormalizado) Then
            Throw New ArgumentException("El nombre resultante no puede estar vacío.", NameOf(nombreDestino))
        End If

        Dim idsParaEliminar = idsSeleccionados.Where(Function(id) id <> funcionDestinoId).ToList()

        Dim contexto = _unitOfWork.Context

        Using transaccion = contexto.Database.BeginTransaction()
            Try
                Dim repoFunciones = _unitOfWork.Repository(Of Funcion)()

                Dim funcionDestino = Await repoFunciones.GetByIdAsync(funcionDestinoId)
                If funcionDestino Is Nothing Then
                    Throw New InvalidOperationException("No se encontró la función de destino.")
                End If

                Dim existeOtraFuncionConNombre = Await repoFunciones.GetQueryable().
                    Where(Function(f) f.Id <> funcionDestinoId).
                    AnyAsync(Function(f) f.Nombre = nombreNormalizado)

                If existeOtraFuncionConNombre Then
                    Throw New InvalidOperationException("Ya existe otra función con el nombre indicado.")
                End If

                If Not String.Equals(funcionDestino.Nombre, nombreNormalizado, StringComparison.Ordinal) Then
                    funcionDestino.Nombre = nombreNormalizado
                    contexto.Entry(funcionDestino).State = EntityState.Modified
                End If

                If idsParaEliminar.Count > 0 Then
                    Dim repoFuncionarios = _unitOfWork.Repository(Of Funcionario)()
                    Dim funcionarios = Await repoFuncionarios.GetQueryable(tracking:=True).
                        Where(Function(f) f.FuncionId.HasValue AndAlso idsParaEliminar.Contains(f.FuncionId.Value)).
                        ToListAsync()

                    For Each funcionario In funcionarios
                        funcionario.FuncionId = funcionDestinoId
                        contexto.Entry(funcionario).State = EntityState.Modified
                    Next

                    For Each id In idsParaEliminar
                        Dim stub = New Funcion() With {.Id = id}
                        contexto.Entry(stub).State = EntityState.Deleted
                    Next
                End If

                Await _unitOfWork.CommitAsync()
                transaccion.Commit()
            Catch
                transaccion.Rollback()
                Throw
            End Try
        End Using
    End Function
End Class
