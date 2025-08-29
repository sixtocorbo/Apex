' /Common/IFormularioActualizable.vb

Public Interface IFormularioActualizable
    ''' <summary>
    ''' Define un método estándar para que los formularios recarguen sus datos
    ''' desde la base de datos o cualquier otra fuente.
    ''' </summary>
    Sub ActualizarDatos()
End Interface