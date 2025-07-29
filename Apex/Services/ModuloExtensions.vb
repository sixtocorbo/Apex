' Apex/Services/ModuloExtensions.vb
' VERSIÓN CORREGIDA Y ROBUSTA

Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module ModuloExtensions

    Private ReadOnly PropertyCache As New Dictionary(Of Type, PropertyInfo())()

    <Extension()>
    Public Function ToDataTable(Of T)(source As IEnumerable(Of T)) As DataTable
        Dim table As New DataTable()

        ' --- INICIO DE LA CORRECCIÓN ---
        ' Si la colección está vacía, devuelve una tabla vacía.
        If source Is Nothing OrElse Not source.Any() Then
            Return table
        End If

        ' Obtenemos el tipo del primer elemento para manejar correctamente
        ' listas de objetos y tipos anónimos.
        Dim firstItem = source.First()
        Dim itemType As Type = firstItem.GetType()
        ' --- FIN DE LA CORRECCIÓN ---

        Dim properties As PropertyInfo() = Nothing

        SyncLock PropertyCache
            If Not PropertyCache.TryGetValue(itemType, properties) Then
                properties = itemType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)
                PropertyCache(itemType) = properties
            End If
        End SyncLock

        ' Definir las columnas del DataTable
        For Each prop In properties
            If Not prop.CanRead Then Continue For

            Dim underlyingType As Type = Nullable.GetUnderlyingType(prop.PropertyType)
            Dim propType As Type = If(underlyingType, prop.PropertyType)

            table.Columns.Add(prop.Name, propType)
        Next

        ' Población de filas
        For Each item In source
            Dim row As DataRow = table.NewRow()
            For Each prop In properties
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

