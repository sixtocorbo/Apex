Imports System.Reflection
Imports System.Runtime.CompilerServices ' Asegúrate de importar System.Data para usar DataTable

Public Module ModuloExtensions

    ' Caché para almacenar las propiedades de cada tipo y mejorar el rendimiento
    Private ReadOnly PropertyCache As New Dictionary(Of Type, PropertyInfo())()

    ''' <summary>
    ''' Convierte una colección genérica en un DataTable.
    ''' </summary>
    ''' <typeparam name="T">El tipo de los elementos en la colección.</typeparam>
    ''' <param name="source">La colección de elementos a convertir.</param>
    ''' <returns>Un DataTable que representa la colección.</returns>
    <Extension()>
    Public Function ToDataTable(Of T)(source As IEnumerable(Of T)) As DataTable
        Dim table As New DataTable()
        Dim typeOfT As Type = GetType(T)

        ' Inicializar 'properties' a Nothing para evitar advertencias del compilador
        Dim properties As PropertyInfo() = Nothing

        ' Obtener las propiedades desde la caché o mediante reflexión
        SyncLock PropertyCache
            If Not PropertyCache.TryGetValue(typeOfT, properties) Then
                properties = typeOfT.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                PropertyCache(typeOfT) = properties
            End If
        End SyncLock

        ' Lista para almacenar el nombre de la columna correspondiente a cada propiedad
        Dim columnNames As New List(Of String)()

        ' Definir las columnas del DataTable
        For Each prop In properties
            ' Evitar propiedades sin getters
            If Not prop.CanRead Then Continue For

            Dim underlyingType As Type = Nullable.GetUnderlyingType(prop.PropertyType)
            Dim propType As Type = If(underlyingType, prop.PropertyType)

            ' Agregar la columna con el tipo adecuado
            table.Columns.Add(prop.Name, propType)
            columnNames.Add(prop.Name) ' Guardar el nombre de la columna correspondiente
        Next

        ' Población de filas
        For Each item In source
            Dim row As DataRow = table.NewRow()

            For Each prop In properties
                ' Ignorar propiedades sin getters
                If Not prop.CanRead Then Continue For

                Dim propName As String = prop.Name
                Dim propValue As Object = prop.GetValue(item, Nothing)

                If propValue IsNot Nothing Then
                    row(propName) = propValue
                Else
                    row(propName) = DBNull.Value
                End If
            Next
            table.Rows.Add(row)
        Next

        Return table
    End Function

End Module
