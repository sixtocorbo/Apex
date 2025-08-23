' Definimos la clase como PARCIAL. Esto la "une" a la clase que genera Entity Framework.
Partial Public Class EstadoTransitorio
    ' Esta propiedad personalizada NUNCA será borrada si regeneras el modelo,
    ' porque vive en su propio archivo.
    Public Property AdjuntosNuevos As New List(Of EstadoTransitorioAdjunto)
End Class
