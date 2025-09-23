' En: /Services/UsuarioService.vb
Imports System.Data.Entity
Imports System.Security.Cryptography
Imports System.Text

Public Class UsuarioService
    Private ReadOnly _uow As IUnitOfWork

    Public Sub New()
        _uow = New UnitOfWork()
    End Sub

    Public Async Function GetAllAsync() As Task(Of List(Of Usuario))
        Return Await _uow.Repository(Of Usuario)().GetAll().Include(Function(u) u.Rols).ToListAsync()
    End Function

    Public Async Function GetByIdAsync(id As Integer) As Task(Of Usuario)
        Return Await _uow.Repository(Of Usuario)().GetAll().Include(Function(u) u.Rols).FirstOrDefaultAsync(Function(u) u.Id = id)
    End Function

    Public Async Function CreateAsync(usuario As Usuario, password As String) As Task
        ' Generar Salt y Hash para la nueva contraseña
        Dim salt = GenerateSalt()
        usuario.PasswordSalt = salt
        usuario.PasswordHash = HashPassword(password, salt)
        _uow.Repository(Of Usuario)().Insert(usuario)
        Await _uow.SaveChangesAsync()
    End Function

    Public Async Function UpdateAsync(usuario As Usuario, Optional newPassword As String = Nothing) As Task
        Dim repo = _uow.Repository(Of Usuario)()
        Dim dbUser = Await repo.GetAll().Include(Function(u) u.Rols).SingleOrDefaultAsync(Function(u) u.Id = usuario.Id)

        If dbUser Is Nothing Then Throw New Exception("Usuario no encontrado.")

        ' Actualizar propiedades
        _uow.Context.Entry(dbUser).CurrentValues.SetValues(usuario)

        ' Si se proporcionó una nueva contraseña, actualizarla
        If Not String.IsNullOrWhiteSpace(newPassword) Then
            Dim salt = GenerateSalt()
            dbUser.PasswordSalt = salt
            dbUser.PasswordHash = HashPassword(newPassword, salt)
        End If

        ' Actualizar roles
        dbUser.Rols.Clear()
        For Each rol In usuario.Rols
            dbUser.Rols.Add(_uow.Context.Set(Of Rol)().Find(rol.Id))
        Next

        Await _uow.SaveChangesAsync()
    End Function

    Public Async Function DeleteAsync(id As Integer) As Task
        _uow.Repository(Of Usuario)().Delete(id)
        Await _uow.SaveChangesAsync()
    End Function

    ' --- Lógica de Seguridad ---

    Public Async Function ValidateCredentialsAsync(username As String, password As String) As Task(Of Usuario)
        Dim user = Await _uow.Repository(Of Usuario)().FirstOrDefaultAsync(Function(u) u.NombreUsuario = username AndAlso u.Activo)
        If user Is Nothing Then Return Nothing

        Dim passwordHash = HashPassword(password, user.PasswordSalt)

        If AreHashesEqual(passwordHash, user.PasswordHash) Then
            Return user
        End If

        Return Nothing
    End Function

    Private Function GenerateSalt() As Byte()
        Using rng As New RNGCryptoServiceProvider()
            Dim salt(127) As Byte
            rng.GetBytes(salt)
            Return salt
        End Using
    End Function

    Private Function HashPassword(password As String, salt As Byte()) As Byte()
        Using sha512 As New SHA512Managed()
            Dim passwordBytes = Encoding.UTF8.GetBytes(password)
            Dim combinedBytes = New Byte(passwordBytes.Length + salt.Length - 1) {}
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length)
            Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length)
            Return sha512.ComputeHash(combinedBytes)
        End Using
    End Function

    Private Function AreHashesEqual(hash1 As Byte(), hash2 As Byte()) As Boolean
        If hash1.Length <> hash2.Length Then Return False
        For i = 0 To hash1.Length - 1
            If hash1(i) <> hash2(i) Then Return False
        Next
        Return True
    End Function
End Class