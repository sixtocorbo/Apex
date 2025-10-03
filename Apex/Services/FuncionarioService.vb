' Apex/Services/FuncionarioService.vb
Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Data.SqlClient
Imports System.Linq

Public Class EstadisticaItem
    Public Property Etiqueta As String
    Public Property Valor As Integer
End Class

Public Enum AgrupacionPresencia
    Seccion
    PuestoTrabajo
End Enum

Public Enum FiltroPresencia
    Presentes
    Ausentes
End Enum

Public Class PresenciaFuncionarioDetalle
    Public Property FuncionarioId As Integer
    Public Property Nombre As String
    Public Property Seccion As String
    Public Property PuestoTrabajo As String
    Public Property Estado As String
End Class

Public Class PresenciaAgrupada
    Public Property Grupo As String
    Public Property Cantidad As Integer
    Public Property Funcionarios As List(Of PresenciaFuncionarioDetalle)
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
                    .SubEscalafon = If(f.SubEscalafon IsNot Nothing, f.SubEscalafon.Nombre, "N/A"),
                    .SubDireccion = If(f.SubDireccion IsNot Nothing, f.SubDireccion.Nombre, "N/A"),
                    .PrestadorSalud = If(f.PrestadorSalud IsNot Nothing, f.PrestadorSalud.Nombre, "N/A"),
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
                f.SubEscalafon,
                f.SubDireccion,
                f.PrestadorSalud,
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

    Public Function GetDistribucionPorEstado() As List(Of EstadisticaItem)
        Using context As New ApexEntities()
            Return context.Funcionario.
                GroupBy(Function(f) f.Activo).
                Select(Function(g) New EstadisticaItem With {
                    .Etiqueta = If(g.Key, "Activos", "Inactivos"),
                    .Valor = g.Count()
                }).
                OrderByDescending(Function(item) item.Valor).
                ToList()
        End Using
    End Function

    Public Function GetDistribucionPorTurno() As List(Of EstadisticaItem)
        Using context As New ApexEntities()
            Return context.Funcionario.
                Where(Function(f) f.Activo).
                GroupBy(Function(f) f.Turno.Nombre).
                Select(Function(g) New EstadisticaItem With {
                    .Etiqueta = If(g.Key IsNot Nothing, g.Key, "Sin especificar"),
                    .Valor = g.Count()
                }).
                OrderByDescending(Function(item) item.Valor).
                ToList()
        End Using
    End Function

    Public Function GetDistribucionPorNivelEstudio() As List(Of EstadisticaItem)
        Using context As New ApexEntities()
            Return context.Funcionario.
                Where(Function(f) f.Activo).
                GroupBy(Function(f) f.NivelEstudio.Nombre).
                Select(Function(g) New EstadisticaItem With {
                    .Etiqueta = If(g.Key IsNot Nothing, g.Key, "Sin especificar"),
                    .Valor = g.Count()
                }).
                OrderByDescending(Function(item) item.Valor).
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

    Public Async Function ObtenerPresenciasAgrupadasAsync(
        fecha As Date,
        filtro As FiltroPresencia,
        agrupacion As AgrupacionPresencia
    ) As Task(Of List(Of PresenciaAgrupada))

        Dim presencias = Await ObtenerPresenciasParaFechaAsync(fecha)
        Dim presenciasDict = presencias.
            GroupBy(Function(p) p.FuncionarioId).
            ToDictionary(Function(g) g.Key, Function(g) g.Select(Function(p) p.Resultado).FirstOrDefault())

        Using uow As New UnitOfWork()
            Dim ctx = uow.Context

            Dim funcionarios = Await ctx.Set(Of Funcionario)().
                AsNoTracking().
                Where(Function(f) f.Activo).
                Select(Function(f) New With {
                    .Id = f.Id,
                    .Nombre = f.Nombre,
                    .Seccion = If(f.Seccion IsNot Nothing, f.Seccion.Nombre, Nothing),
                    .Puesto = If(f.PuestoTrabajo IsNot Nothing, f.PuestoTrabajo.Nombre, Nothing)
                }).
                ToListAsync()

            Dim grupos = New Dictionary(Of String, PresenciaAgrupada)(StringComparer.OrdinalIgnoreCase)

            For Each funcionario In funcionarios
                Dim estadoRaw As String = Nothing
                If presenciasDict.TryGetValue(funcionario.Id, estadoRaw) Then
                    estadoRaw = If(estadoRaw, String.Empty)
                Else
                    estadoRaw = String.Empty
                End If

                Dim estadoNormalizado = If(String.IsNullOrWhiteSpace(estadoRaw), "Sin información", estadoRaw.Trim())
                Dim categoria = ClasificarPresencia(estadoNormalizado)

                Dim incluir = False
                Select Case filtro
                    Case FiltroPresencia.Presentes
                        incluir = (categoria = CategoriaPresencia.Presente)
                    Case FiltroPresencia.Ausentes
                        incluir = (categoria <> CategoriaPresencia.Presente AndAlso categoria <> CategoriaPresencia.Inactivo)
                End Select

                If Not incluir Then
                    Continue For
                End If

                Dim seccionNombre = NormalizarGrupo(funcionario.Seccion, "Sin sección")
                Dim puestoNombre = NormalizarGrupo(funcionario.Puesto, "Sin puesto")
                Dim clave = If(agrupacion = AgrupacionPresencia.Seccion, seccionNombre, puestoNombre)

                Dim grupo As PresenciaAgrupada = Nothing
                If Not grupos.TryGetValue(clave, grupo) Then
                    grupo = New PresenciaAgrupada With {
                        .Grupo = clave,
                        .Funcionarios = New List(Of PresenciaFuncionarioDetalle)()
                    }
                    grupos.Add(clave, grupo)
                End If

                grupo.Funcionarios.Add(New PresenciaFuncionarioDetalle With {
                    .FuncionarioId = funcionario.Id,
                    .Nombre = funcionario.Nombre,
                    .Seccion = seccionNombre,
                    .PuestoTrabajo = puestoNombre,
                    .Estado = estadoNormalizado
                })
            Next

            For Each grupo In grupos.Values
                grupo.Funcionarios = grupo.Funcionarios.
                    OrderBy(Function(f) f.Nombre).
                    ToList()
                grupo.Cantidad = grupo.Funcionarios.Count
            Next

            Return grupos.Values.
                OrderByDescending(Function(g) g.Cantidad).
                ThenBy(Function(g) g.Grupo).
                ToList()
        End Using
    End Function

    Private Shared Function NormalizarGrupo(valor As String, reemplazo As String) As String
        If String.IsNullOrWhiteSpace(valor) Then
            Return reemplazo
        End If
        Return valor.Trim()
    End Function

    Private Shared Function ClasificarPresencia(resultado As String) As CategoriaPresencia
        If String.IsNullOrWhiteSpace(resultado) Then
            Return CategoriaPresencia.SinDatos
        End If

        Dim valor = resultado.Trim()

        If valor.StartsWith("Inactivo", StringComparison.OrdinalIgnoreCase) Then
            Return CategoriaPresencia.Inactivo
        End If

        If valor.Equals("Franco", StringComparison.OrdinalIgnoreCase) Then
            Return CategoriaPresencia.Franco
        End If

        If valor.StartsWith("Presente", StringComparison.OrdinalIgnoreCase) Then
            Return CategoriaPresencia.Presente
        End If

        Return CategoriaPresencia.Licencia
    End Function

    Private Enum CategoriaPresencia
        Presente
        Franco
        Licencia
        Inactivo
        SinDatos
    End Enum

    ' --- INICIO DEL NUEVO CÓDIGO ---
    Public Async Function GuardarFuncionarioAsync(funcionario As Funcionario, uow As IUnitOfWork) As Task
        ' Determina si es una entidad nueva o existente
        If funcionario.Id <= 0 Then
            ' Es nuevo, lo agregamos
            uow.Repository(Of Funcionario).Add(funcionario)
        Else
            ' Es existente, lo actualizamos
            uow.Repository(Of Funcionario).Update(funcionario)
        End If

        ' Guarda los cambios en la base de datos
        Await uow.CommitAsync()

        ' Notifica a toda la aplicación sobre el cambio,
        ' una vez que se ha confirmado en la base de datos.
        NotificadorEventos.NotificarCambiosEnFuncionario(funcionario.Id)
    End Function
    Public Async Function ObtenerSubDireccionesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of SubDireccion)().
                AsNoTracking().
                OrderBy(Function(sd) sd.Nombre).
                ToListAsync()
            Return lista.Select(Function(sd) New KeyValuePair(Of Integer, String)(sd.Id, sd.Nombre)).ToList()
        End Using
    End Function

    Public Async Function ObtenerPrestadoresSaludAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of PrestadorSalud)().
                AsNoTracking().
                OrderBy(Function(ps) ps.Nombre).
                ToListAsync()
            Return lista.Select(Function(ps) New KeyValuePair(Of Integer, String)(ps.Id, ps.Nombre)).ToList()
        End Using
    End Function
    Public Async Function ObtenerSubEscalafonesAsync() As Task(Of List(Of KeyValuePair(Of Integer, String)))
        Using uow As New UnitOfWork()
            Dim lista = Await uow.Context.Set(Of SubEscalafon)().
                AsNoTracking().
                OrderBy(Function(se) se.Nombre).
                ToListAsync()
            Return lista.Select(Function(se) New KeyValuePair(Of Integer, String)(se.Id, se.Nombre)).ToList()
        End Using
    End Function
End Class

Public Class FuncionarioVistaDTO
    Public Property Id As Integer
    Public Property CI As String
    Public Property Nombre As String
    Public Property CargoNombre As String
    Public Property Activo As Boolean
End Class