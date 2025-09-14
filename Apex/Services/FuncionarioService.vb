' Apex/Services/FuncionarioService.vb
Option Strict On
Option Explicit On

Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Linq

Public Class EstadisticaItem
    Public Property Etiqueta As String
    Public Property Valor As Integer
End Class

Public Class FuncionarioService
    Inherits GenericService(Of Funcionario)

    Public Sub New()
        MyBase.New(New UnitOfWork())
    End Sub

    Public Sub New(unitOfWork As IUnitOfWork)
        MyBase.New(unitOfWork)
    End Sub

    ' -----------------------------------------
    '   CONSULTAS / LECTURAS (UoW por operación)
    ' -----------------------------------------
    Public Async Function GetByIdCompletoAsync(id As Integer) As Task(Of Funcionario)
        Using uow As New UnitOfWork()
            Return Await uow.Context.Set(Of Funcionario)().
                AsNoTracking().
                Include(Function(f) f.Cargo).
                Include(Function(f) f.Escalafon).
                Include(Function(f) f.Funcion).
                Include(Function(f) f.TipoFuncionario).
                Include(Function(f) f.EstadoCivil).
                Include(Function(f) f.Genero).
                Include(Function(f) f.NivelEstudio).
                Include(Function(f) f.FuncionarioDotacion.Select(Function(d) d.DotacionItem)).
                Include(Function(f) f.FuncionarioEstadoLegal).
                Include(Function(f) f.FuncionarioDispositivo).
                Include(Function(f) f.EstadoTransitorio.Select(Function(et) et.TipoEstadoTransitorio)).
                FirstOrDefaultAsync(Function(f) f.Id = id)
        End Using
    End Function

    Public Async Function GetFuncionariosParaVistaAsync(
        Optional fecha As Date? = Nothing,
        Optional incluirSoloConPresencia As Boolean = False,
        Optional ordenarComoPersonal As Boolean = False
    ) As Task(Of List(Of Object))
        Using uow As New UnitOfWork()

            ' 1) Fecha
            Dim ffecha = If(fecha.HasValue, fecha.Value.Date, Date.Today)
            Dim pFecha = New SqlParameter("@Fecha", ffecha)

            ' 2) SP de presencias
            Dim presenciasList = Await uow.Context.Database.
                SqlQuery(Of PresenciaDTO)("EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha).
                ToListAsync()

            Dim presenciasDict As Dictionary(Of Integer, String) =
                presenciasList.
                    GroupBy(Function(p) p.FuncionarioId).
                    ToDictionary(Function(g) g.Key, Function(g) g.First().Resultado)

            ' 3) Base de funcionarios
            Dim q = uow.Context.Set(Of Funcionario)().AsNoTracking().AsQueryable()

            If incluirSoloConPresencia AndAlso presenciasDict.Count > 0 Then
                Dim ids = presenciasDict.Keys.ToList()
                q = q.Where(Function(f) ids.Contains(f.Id))
            End If

            ' 4) Proyección
            Dim funcionarios = Await q.
                Select(Function(f) New With {
                    .Id = f.Id,
                    .NombreCompleto = f.Nombre,
                    .Cedula = f.CI,
                    .FechaIngreso = f.FechaIngreso,
                    .Activo = f.Activo,
                    .CorreoElectronico = f.Email,
                    .FechaNacimiento = f.FechaNacimiento,
                    .Domicilio = f.Domicilio,
                    .Cargo = If(f.Cargo IsNot Nothing, f.Cargo.Nombre, "N/A"),
                    .TipoDeFuncionario = If(f.TipoFuncionario IsNot Nothing, f.TipoFuncionario.Nombre, "N/A"),
                    .Escalafon = If(f.Escalafon IsNot Nothing, f.Escalafon.Nombre, "N/A"),
                    .Funcion = If(f.Funcion IsNot Nothing, f.Funcion.Nombre, "N/A"),
                    .EstadoActual = If(f.Activo, "Activo", "Inactivo"),
                    .Seccion = If(f.Seccion IsNot Nothing, f.Seccion.Nombre, "N/A"),
                    .PuestoDeTrabajo = If(f.PuestoTrabajo IsNot Nothing, f.PuestoTrabajo.Nombre, "N/A"),
                    .Turno = If(f.Turno IsNot Nothing, f.Turno.Nombre, "N/A"),
                    .Semana = If(f.Semana IsNot Nothing, f.Semana.Nombre, "N/A"),
                    .Horario = If(f.Horario IsNot Nothing, f.Horario.Nombre, "N/A"),
                    .Genero = If(f.Genero IsNot Nothing, f.Genero.Nombre, "N/A"),
                    .EstadoCivil = If(f.EstadoCivil IsNot Nothing, f.EstadoCivil.Nombre, "N/A"),
                    .NivelDeEstudio = If(f.NivelEstudio IsNot Nothing, f.NivelEstudio.Nombre, "N/A")
                }).
                ToListAsync()

            ' 5) Superponer presencia
            Dim mezclado = funcionarios.Select(Function(f) New With {
                f.Id,
                f.NombreCompleto,
                f.Cedula,
                f.FechaIngreso,
                f.Activo,
                f.CorreoElectronico,
                f.FechaNacimiento,
                f.Domicilio,
                f.Cargo,
                f.TipoDeFuncionario,
                f.Escalafon,
                f.Funcion,
                f.EstadoActual,
                f.Seccion,
                f.PuestoDeTrabajo,
                f.Turno,
                f.Semana,
                f.Horario,
                f.Genero,
                f.EstadoCivil,
                f.NivelDeEstudio,
                .Presencia = If(presenciasDict.ContainsKey(f.Id), presenciasDict(f.Id), "-")
            })

            ' 6) Orden
            Dim resultado As IEnumerable(Of Object)
            If ordenarComoPersonal Then
                resultado = mezclado.
                    OrderBy(Function(x) x.Seccion).
                    ThenByDescending(Function(x) x.Presencia).
                    Cast(Of Object)()
            Else
                resultado = mezclado.Cast(Of Object)()
            End If

            Return resultado.ToList()
        End Using
    End Function

    ' ------------------------------
    '   CATÁLOGOS / COMBOS (UoW op)
    ' ------------------------------
    Public Async Function ObtenerCargosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of Cargo)().
                AsNoTracking().
                OrderBy(Function(c) c.Nombre).
                ToListAsync()
            Return lista.Select(Function(c) New KeyValuePair(Of Integer, String)(c.Id, c.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerTiposFuncionarioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of TipoFuncionario)().
                AsNoTracking().
                OrderBy(Function(t) t.Nombre).
                ToListAsync()
            Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerEscalafonesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of Escalafon)().
                AsNoTracking().
                OrderBy(Function(e) e.Nombre).
                ToListAsync()
            Return lista.Select(Function(e) New KeyValuePair(Of Integer, String)(e.Id, e.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerFuncionesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of Funcion)().
                AsNoTracking().
                OrderBy(Function(f) f.Nombre).
                ToListAsync()
            Return lista.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerEstadosCivilesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of EstadoCivil)().
                AsNoTracking().
                OrderBy(Function(ec) ec.Nombre).
                ToListAsync()
            Return lista.Select(Function(ec) New KeyValuePair(Of Integer, String)(ec.Id, ec.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerGenerosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of Genero)().
                AsNoTracking().
                OrderBy(Function(g) g.Nombre).
                ToListAsync()
            Return lista.Select(Function(g) New KeyValuePair(Of Integer, String)(g.Id, g.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerNivelesEstudioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of NivelEstudio)().
                AsNoTracking().
                OrderBy(Function(ne) ne.Nombre).
                ToListAsync()
            Return lista.Select(Function(ne) New KeyValuePair(Of Integer, String)(ne.Id, ne.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerSeccionesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of Seccion)().
                AsNoTracking().
                OrderBy(Function(s) s.Nombre).
                ToListAsync()
            Return lista.Select(Function(s) New KeyValuePair(Of Integer, String)(s.Id, s.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerPuestosTrabajoAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of PuestoTrabajo)().
                AsNoTracking().
                OrderBy(Function(p) p.Nombre).
                ToListAsync()
            Return lista.Select(Function(p) New KeyValuePair(Of Integer, String)(p.Id, p.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerTurnosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of Turno)().
                AsNoTracking().
                OrderBy(Function(t) t.Nombre).
                ToListAsync()
            Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerSemanasAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of Semana)().
                AsNoTracking().
                OrderBy(Function(s) s.Nombre).
                ToListAsync()
            Return lista.Select(Function(s) New KeyValuePair(Of Integer, String)(s.Id, s.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerHorariosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of Horario)().
                AsNoTracking().
                OrderBy(Function(h) h.Nombre).
                ToListAsync()
            Return lista.Select(Function(h) New KeyValuePair(Of Integer, String)(h.Id, h.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerItemsDotacionCompletosAsync() As Task(Of List(Of DotacionItem))
        Using uow As New UnitOfWork()
            Return Await uow.Context.Set(Of DotacionItem)().
                AsNoTracking().
                OrderBy(Function(di) di.Nombre).
                ToListAsync()
        End Using
    End Function

    Public Async Function ObtenerTiposEstadoTransitorioCompletosAsync() As Task(Of List(Of TipoEstadoTransitorio))
        Using uow As New UnitOfWork()
            Return Await uow.Context.Set(Of TipoEstadoTransitorio)().
                AsNoTracking().
                OrderBy(Function(t) t.Nombre).
                ToListAsync()
        End Using
    End Function

    Public Async Function ObtenerTiposEstadoTransitorioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Dim lista = Await ObtenerTiposEstadoTransitorioCompletosAsync()
        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
    End Function

    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim funcionariosData = Await uow.Context.Set(Of Funcionario)().
                AsNoTracking().
                OrderBy(Function(f) f.Nombre).
                Select(Function(f) New With {Key .Id = f.Id, Key .Nombre = f.Nombre}).
                ToListAsync()
            Return funcionariosData.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
        End Using
    End Function

    ' -----------------------------
    '   ESTADÍSTICAS (solo lectura)
    ' -----------------------------
    Public Function GetDistribucionPorGenero() As List(Of EstadisticaItem)
        Using context As New ApexEntities()
            Return context.Funcionario.
                Where(Function(f) f.Activo).
                GroupBy(Function(f) f.Genero.Nombre).
                Select(Function(g) New EstadisticaItem With {
                    .Etiqueta = If(g.Key IsNot Nothing, g.Key, "Sin especificar"),
                    .Valor = g.Count()
                }).ToList()
        End Using
    End Function

    Public Function GetDistribucionPorRangoEdad() As List(Of EstadisticaItem)
        Using context As New ApexEntities()
            Dim funcionarios = context.Funcionario.
                Where(Function(f) f.Activo AndAlso f.FechaNacimiento.HasValue).
                Select(Function(f) New With {.AnioNacimiento = f.FechaNacimiento.Value.Year}).
                ToList()

            Dim anioActual = DateTime.Now.Year

            Return funcionarios.
                Select(Function(f) anioActual - f.AnioNacimiento).
                GroupBy(Function(edad)
                            Return If(edad <= 25, "18-25",
                                   If(edad <= 35, "26-35",
                                      If(edad <= 45, "36-45",
                                         If(edad <= 55, "46-55", "Más de 55"))))
                        End Function).
                Select(Function(g) New EstadisticaItem With {.Etiqueta = g.Key, .Valor = g.Count()}).
                OrderBy(Function(item) item.Etiqueta).
                ToList()
        End Using
    End Function

    Public Function GetDistribucionPorAreaTrabajo() As List(Of EstadisticaItem)
        Using context As New ApexEntities()
            Return context.Funcionario.
                Where(Function(f) f.Activo).
                GroupBy(Function(f) f.PuestoTrabajo.Nombre).
                Select(Function(g) New EstadisticaItem With {
                    .Etiqueta = If(g.Key IsNot Nothing, g.Key, "Sin especificar"),
                    .Valor = g.Count()
                }).
                OrderByDescending(Function(item) item.Valor).
                ToList()
        End Using
    End Function

    Public Function GetDistribucionPorCargo() As List(Of EstadisticaItem)
        Using context As New ApexEntities()
            Return context.Funcionario.
                Where(Function(f) f.Activo).
                GroupBy(Function(f) f.Cargo.Nombre).
                Select(Function(g) New EstadisticaItem With {
                    .Etiqueta = If(g.Key IsNot Nothing, g.Key, "Sin especificar"),
                    .Valor = g.Count()
                }).
                OrderByDescending(Function(item) item.Valor).
                Take(10).
                ToList()
        End Using
    End Function

    Public Async Function ObtenerPresenciasParaFechaAsync(fecha As Date) As Task(Of List(Of PresenciaDTO))
        Using uow As New UnitOfWork()
            Dim ctx = uow.Context
            Dim pFecha = New SqlParameter("@Fecha", fecha.Date)
            Dim lista = Await ctx.Database.SqlQuery(Of PresenciaDTO)(
                "EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha
            ).ToListAsync()
            Return lista
        End Using
    End Function

    ' DTOs
    Public Class PresenciaDTO
        Public Property FuncionarioId As Integer
        Public Property Resultado As String
    End Class

End Class

Public Class FuncionarioVistaDTO
    Public Property Id As Integer
    Public Property CI As String
    Public Property Nombre As String
    Public Property CargoNombre As String
    Public Property Activo As Boolean
End Class

'' Apex/Services/FuncionarioService.vb
'Imports System.Data.Entity
'Imports System.Data.SqlClient
'Public Class EstadisticaItem
'    Public Property Etiqueta As String
'    Public Property Valor As Integer
'End Class
'Public Class FuncionarioService
'    Inherits GenericService(Of Funcionario)

'    Public Sub New()
'        MyBase.New(New UnitOfWork())
'    End Sub

'    Public Sub New(unitOfWork As IUnitOfWork)
'        MyBase.New(unitOfWork)
'    End Sub

'    Public Async Function GetByIdCompletoAsync(id As Integer) As Task(Of Funcionario)
'        Return Await _unitOfWork.Repository(Of Funcionario)().
'        GetAll().
'        Include(Function(f) f.Cargo).
'        Include(Function(f) f.Escalafon).
'        Include(Function(f) f.Funcion).
'        Include(Function(f) f.TipoFuncionario).
'        Include(Function(f) f.EstadoCivil).
'        Include(Function(f) f.Genero).
'        Include(Function(f) f.NivelEstudio).
'        Include(Function(f) f.FuncionarioDotacion.Select(Function(d) d.DotacionItem)).
'        Include(Function(f) f.FuncionarioEstadoLegal).
'        Include(Function(f) f.FuncionarioDispositivo).
'        Include(Function(f) f.EstadoTransitorio.Select(Function(et) et.TipoEstadoTransitorio)).
'        FirstOrDefaultAsync(Function(f) f.Id = id)
'    End Function

'    Public Async Function GetFuncionariosParaVistaAsync(
'    Optional fecha As Date? = Nothing,
'    Optional incluirSoloConPresencia As Boolean = False,
'    Optional ordenarComoPersonal As Boolean = False
') As Task(Of List(Of Object))
'        Using uow As New UnitOfWork()

'            ' 1) Fecha para presencia (hoy si no viene)
'            Dim ffecha = If(fecha.HasValue, fecha.Value.Date, Date.Today)
'            Dim pFecha = New SqlParameter("@Fecha", ffecha)

'            ' 2) Ejecutar SP de presencias (Apex: FuncionarioId, Resultado) y tolerar duplicados
'            Dim presenciasList = Await uow.Context.Database _
'            .SqlQuery(Of PresenciaDTO)("EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha) _
'            .ToListAsync()

'            Dim presenciasDict As Dictionary(Of Integer, String) =
'            presenciasList _
'                .GroupBy(Function(p) p.FuncionarioId) _
'                .ToDictionary(Function(g) g.Key, Function(g) g.First().Resultado)

'            ' 3) Base de funcionarios SIN prefiltrado (activos/inactivos, todos)
'            '    Uso el DbSet directo para evitar cualquier filtro implícito del repositorio.
'            Dim q = uow.Context.Set(Of Funcionario)().AsNoTracking().AsQueryable()

'            ' (Opcional) si querés traer SOLO los que aparecen en el SP (comportamiento del sistema anterior):
'            If incluirSoloConPresencia AndAlso presenciasDict.Count > 0 Then
'                Dim ids = presenciasDict.Keys.ToList()
'                q = q.Where(Function(f) ids.Contains(f.Id))
'            End If

'            ' 4) Proyección: mismos campos que ya usás (nada extra)
'            Dim funcionarios = Await q _
'            .Select(Function(f) New With {
'                .Id = f.Id,
'                .NombreCompleto = f.Nombre,
'                .Cedula = f.CI,
'                .FechaIngreso = f.FechaIngreso,
'                .Activo = f.Activo,
'                .CorreoElectronico = f.Email,
'                .FechaNacimiento = f.FechaNacimiento,
'                .Domicilio = f.Domicilio,
'                .Cargo = If(f.Cargo IsNot Nothing, f.Cargo.Nombre, "N/A"),
'                .TipoDeFuncionario = If(f.TipoFuncionario IsNot Nothing, f.TipoFuncionario.Nombre, "N/A"),
'                .Escalafon = If(f.Escalafon IsNot Nothing, f.Escalafon.Nombre, "N/A"),
'                .Funcion = If(f.Funcion IsNot Nothing, f.Funcion.Nombre, "N/A"),
'                .EstadoActual = If(f.Activo, "Activo", "Inactivo"),
'                .Seccion = If(f.Seccion IsNot Nothing, f.Seccion.Nombre, "N/A"),
'                .PuestoDeTrabajo = If(f.PuestoTrabajo IsNot Nothing, f.PuestoTrabajo.Nombre, "N/A"),
'                .Turno = If(f.Turno IsNot Nothing, f.Turno.Nombre, "N/A"),
'                .Semana = If(f.Semana IsNot Nothing, f.Semana.Nombre, "N/A"),
'                .Horario = If(f.Horario IsNot Nothing, f.Horario.Nombre, "N/A"),
'                .Genero = If(f.Genero IsNot Nothing, f.Genero.Nombre, "N/A"),
'                .EstadoCivil = If(f.EstadoCivil IsNot Nothing, f.EstadoCivil.Nombre, "N/A"),
'                .NivelDeEstudio = If(f.NivelEstudio IsNot Nothing, f.NivelEstudio.Nombre, "N/A")
'            }) _
'            .ToListAsync()

'            ' 5) Superponer presencia SIN excluir a nadie
'            Dim mezclado = funcionarios.Select(Function(f) New With {
'            f.Id,
'            f.NombreCompleto,
'            f.Cedula,
'            f.FechaIngreso,
'            f.Activo,
'            f.CorreoElectronico,
'            f.FechaNacimiento,
'            f.Domicilio,
'            f.Cargo,
'            f.TipoDeFuncionario,
'            f.Escalafon,
'            f.Funcion,
'            f.EstadoActual,
'            f.Seccion,
'            f.PuestoDeTrabajo,
'            f.Turno,
'            f.Semana,
'            f.Horario,
'            f.Genero,
'            f.EstadoCivil,
'            f.NivelDeEstudio,
'            .Presencia = If(presenciasDict.ContainsKey(f.Id), presenciasDict(f.Id), "-")
'        })

'            ' 6) Orden: por defecto nada especial; si querés emular el viejo (Sección, Resultado desc):
'            Dim resultado As IEnumerable(Of Object)
'            If ordenarComoPersonal Then
'                resultado = mezclado _
'                .OrderBy(Function(x) x.Seccion) _
'                .ThenByDescending(Function(x) x.Presencia) _
'                .Cast(Of Object)()
'            Else
'                resultado = mezclado.Cast(Of Object)()
'            End If

'            Return resultado.ToList()
'        End Using
'    End Function

'    ' --- Métodos para poblar los ComboBox (Catálogos) ---
'    Public Async Function ObtenerCargosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of Cargo)().GetAll().AsNoTracking().OrderBy(Function(c) c.Nombre).ToListAsync()
'        Return lista.Select(Function(c) New KeyValuePair(Of Integer, String)(c.Id, c.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerTiposFuncionarioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of TipoFuncionario)().GetAll().AsNoTracking().OrderBy(Function(t) t.Nombre).ToListAsync()
'        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerEscalafonesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of Escalafon)().GetAll().AsNoTracking().OrderBy(Function(e) e.Nombre).ToListAsync()
'        Return lista.Select(Function(e) New KeyValuePair(Of Integer, String)(e.Id, e.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerFuncionesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of Funcion)().GetAll().AsNoTracking().OrderBy(Function(f) f.Nombre).ToListAsync()
'        Return lista.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerEstadosCivilesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of EstadoCivil)().GetAll().AsNoTracking().OrderBy(Function(ec) ec.Nombre).ToListAsync()
'        Return lista.Select(Function(ec) New KeyValuePair(Of Integer, String)(ec.Id, ec.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerGenerosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of Genero)().GetAll().AsNoTracking().OrderBy(Function(g) g.Nombre).ToListAsync()
'        Return lista.Select(Function(g) New KeyValuePair(Of Integer, String)(g.Id, g.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerNivelesEstudioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of NivelEstudio)().GetAll().AsNoTracking().OrderBy(Function(ne) ne.Nombre).ToListAsync()
'        Return lista.Select(Function(ne) New KeyValuePair(Of Integer, String)(ne.Id, ne.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerSeccionesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of Seccion)().GetAll().AsNoTracking().OrderBy(Function(s) s.Nombre).ToListAsync()
'        Return lista.Select(Function(s) New KeyValuePair(Of Integer, String)(s.Id, s.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerPuestosTrabajoAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of PuestoTrabajo)().GetAll().AsNoTracking().OrderBy(Function(p) p.Nombre).ToListAsync()
'        Return lista.Select(Function(p) New KeyValuePair(Of Integer, String)(p.Id, p.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerTurnosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of Turno)().GetAll().AsNoTracking().OrderBy(Function(t) t.Nombre).ToListAsync()
'        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerSemanasAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of Semana)().GetAll().AsNoTracking().OrderBy(Function(s) s.Nombre).ToListAsync()
'        Return lista.Select(Function(s) New KeyValuePair(Of Integer, String)(s.Id, s.Nombre)).ToList()
'    End Function

'    Public Async Function ObtenerHorariosAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await _unitOfWork.Repository(Of Horario)().GetAll().AsNoTracking().OrderBy(Function(h) h.Nombre).ToListAsync()
'        Return lista.Select(Function(h) New KeyValuePair(Of Integer, String)(h.Id, h.Nombre)).ToList()
'    End Function
'    ' --- FIN: Métodos añadidos ---

'    Public Async Function ObtenerItemsDotacionCompletosAsync() As Task(Of List(Of DotacionItem))
'        Return Await _unitOfWork.Repository(Of DotacionItem)().
'        GetAll().
'            AsNoTracking().
'            OrderBy(Function(di) di.Nombre).
'            ToListAsync()
'    End Function

'    Public Async Function ObtenerTiposEstadoTransitorioCompletosAsync() As Task(Of List(Of TipoEstadoTransitorio))
'        Return Await _unitOfWork.Repository(Of TipoEstadoTransitorio)().
'        GetAll().OrderBy(Function(t) t.Nombre).
'            ToListAsync()
'    End Function

'    Public Async Function ObtenerTiposEstadoTransitorioAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim lista = Await ObtenerTiposEstadoTransitorioCompletosAsync()
'        Return lista.Select(Function(t) New KeyValuePair(Of Integer, String)(t.Id, t.Nombre)).ToList()
'    End Function

'    ' --- MÉTODO AÑADIDO ---
'    Public Async Function ObtenerFuncionariosParaComboAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
'        Dim funcionariosData = Await _unitOfWork.Repository(Of Funcionario)().GetAll().AsNoTracking() _
'        .OrderBy(Function(f) f.Nombre) _
'        .Select(Function(f) New With {
'            Key .Id = f.Id,
'            Key .Nombre = f.Nombre
'        }) _
'        .ToListAsync()
'        Return funcionariosData.Select(Function(f) New KeyValuePair(Of Integer, String)(f.Id, f.Nombre)).ToList()
'    End Function
'    ' --- INICIO DE LAS FUNCIONES DE ANÁLISIS ESTADÍSTICO ---

'    ' 1. Obtiene la cantidad de funcionarios por género (SOLO ACTIVOS)
'    Public Function GetDistribucionPorGenero() As List(Of EstadisticaItem)
'        Using context As New ApexEntities()
'            Return context.Funcionario _
'            .Where(Function(f) f.Activo) _
'            .GroupBy(Function(f) f.Genero.Nombre) _
'            .Select(Function(g) New EstadisticaItem With {
'                .Etiqueta = If(g.Key IsNot Nothing, g.Key, "Sin especificar"),
'                .Valor = g.Count()
'            }).ToList()
'        End Using
'    End Function

'    ' 2. Obtiene la cantidad de funcionarios por rango de edad (SOLO ACTIVOS)
'    Public Function GetDistribucionPorRangoEdad() As List(Of EstadisticaItem)
'        Using context As New ApexEntities()
'            Dim funcionarios = context.Funcionario _
'            .Where(Function(f) f.Activo AndAlso f.FechaNacimiento.HasValue) _
'            .Select(Function(f) New With {
'                .AnioNacimiento = f.FechaNacimiento.Value.Year
'            }).ToList()

'            Dim anioActual = DateTime.Now.Year

'            Return funcionarios _
'            .Select(Function(f) anioActual - f.AnioNacimiento) _
'            .GroupBy(Function(edad)
'                         Return If(edad <= 25, "18-25",
'                                If(edad <= 35, "26-35",
'                                   If(edad <= 45, "36-45",
'                                      If(edad <= 55, "46-55", "Más de 55"))))
'                     End Function) _
'            .Select(Function(g) New EstadisticaItem With {
'                .Etiqueta = g.Key,
'                .Valor = g.Count()
'            }) _
'            .OrderBy(Function(item) item.Etiqueta) _
'            .ToList()
'        End Using
'    End Function

'    ' 3. Obtiene la cantidad de funcionarios por Área de Trabajo (SOLO ACTIVOS)
'    ' Nota: Basado en tu código, usamos 'PuestoTrabajo' como equivalente a 'Área de Trabajo'.
'    Public Function GetDistribucionPorAreaTrabajo() As List(Of EstadisticaItem)
'        Using context As New ApexEntities()
'            Return context.Funcionario _
'            .Where(Function(f) f.Activo) _
'            .GroupBy(Function(f) f.PuestoTrabajo.Nombre) _
'            .Select(Function(g) New EstadisticaItem With {
'                .Etiqueta = If(g.Key IsNot Nothing, g.Key, "Sin especificar"),
'                .Valor = g.Count()
'            }) _
'            .OrderByDescending(Function(item) item.Valor) _
'            .ToList()
'        End Using
'    End Function

'    ' 4. Obtiene la cantidad de funcionarios por Cargo (SOLO ACTIVOS)
'    Public Function GetDistribucionPorCargo() As List(Of EstadisticaItem)
'        Using context As New ApexEntities()
'            Return context.Funcionario _
'            .Where(Function(f) f.Activo) _
'            .GroupBy(Function(f) f.Cargo.Nombre) _
'            .Select(Function(g) New EstadisticaItem With {
'                .Etiqueta = If(g.Key IsNot Nothing, g.Key, "Sin especificar"),
'                .Valor = g.Count()
'            }) _
'            .OrderByDescending(Function(item) item.Valor) _
'            .Take(10) _
'            .ToList()
'        End Using
'    End Function

'    Public Async Function ObtenerPresenciasParaFechaAsync(fecha As Date) As Task(Of List(Of PresenciaDTO))
'        Using uow As New UnitOfWork()
'            Dim ctx = uow.Context
'            Dim pFecha = New SqlParameter("@Fecha", fecha.Date)
'            Dim lista = Await ctx.Database.SqlQuery(Of PresenciaDTO)(
'            "EXEC dbo.usp_PresenciaFecha_Apex @Fecha", pFecha
'        ).ToListAsync()
'            Return lista
'        End Using
'    End Function

'    Public Class PresenciaDTO
'        Public Property FuncionarioId As Integer
'        Public Property Resultado As String
'    End Class

'End Class
'Public Class FuncionarioVistaDTO
'    Public Property Id As Integer
'    Public Property CI As String
'    Public Property Nombre As String
'    Public Property CargoNombre As String
'    Public Property Activo As Boolean
'End Class
