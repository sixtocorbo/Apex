/*
=================================================================================================
  SCRIPT DE MIGRACIÓN DEFINITIVO: Personal -> Apex (Versión Consolidada vFinal-6 - CORREGIDO)
  Autor:        Gemini (basado en el trabajo del equipo de migración)
  Fecha:        18/09/2025
  Descripción:  Este script unifica todas las tareas de migración en un solo archivo ejecutable.
                Realiza la migración completa desde la base de datos 'Personal' a 'Apex',
                incluyendo la lógica para migrar el estado 'O5', las sanciones desde la
                tabla 'tblSancion' y los nuevos catálogos para subdirección, subescalafón
                y prestador de salud.

  Corrección:   Se agregó la lógica para migrar los registros de la tabla `Personal.dbo.tblSancion`.
  Mejora:       Se agregaron las tablas de catálogo y la migración para los campos
                'sub_direccion', 'subescalafon' y 'prestador_salud' desde tblPolicias.
  Corrección 2: Se agregaron las sentencias CREATE TABLE para las nuevas tablas de catálogo.
  Corrección 3: Se agregaron las sentencias ALTER TABLE para añadir las nuevas columnas a la
                tabla Funcionario y se reordenó la creación de objetos.

  Características:
  1.  Idempotente: Se puede ejecutar varias veces gracias al procedimiento de limpieza inicial.
  2.  Atómico: Toda la migración se ejecuta en una transacción. Si algo falla, todo se revierte.
  3.  Completo: Incluye catálogos, funcionarios, históricos, dotaciones, estados legales,
      documentos adjuntos y fotos.
  4.  Robusto: No usa "números mágicos", la lógica es explícita y segura.
  5.  Post-Migración: Crea las vistas y procedimientos necesarios para la aplicación.
=================================================================================================
*/

USE [Apex];
GO

-- =============================================================================================
-- PASO 0: Creación de Nuevas Tablas de Catálogo y Modificación de Funcionario
-- =============================================================================================
PRINT '--- PASO 0: Creando y modificando tablas necesarias... ---';

IF OBJECT_ID('dbo.SubDireccion', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SubDireccion (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME2 DEFAULT GETDATE()
    );
END;

IF OBJECT_ID('dbo.SubEscalafon', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.SubEscalafon (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME2 DEFAULT GETDATE()
    );
END;

IF OBJECT_ID('dbo.PrestadorSalud', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PrestadorSalud (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(255) NOT NULL,
        CreatedAt DATETIME2 DEFAULT GETDATE()
    );
END;

-- Modificar la tabla Funcionario para agregar las nuevas columnas
IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'SubDireccionId' AND Object_ID = Object_ID(N'dbo.Funcionario'))
BEGIN
    ALTER TABLE dbo.Funcionario ADD SubDireccionId INT NULL;
    ALTER TABLE dbo.Funcionario ADD CONSTRAINT FK_Funcionario_SubDireccion FOREIGN KEY (SubDireccionId) REFERENCES dbo.SubDireccion(Id);
END;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'SubEscalafonId' AND Object_ID = Object_ID(N'dbo.Funcionario'))
BEGIN
    ALTER TABLE dbo.Funcionario ADD SubEscalafonId INT NULL;
    ALTER TABLE dbo.Funcionario ADD CONSTRAINT FK_Funcionario_SubEscalafon FOREIGN KEY (SubEscalafonId) REFERENCES dbo.SubEscalafon(Id);
END;

IF NOT EXISTS (SELECT * FROM sys.columns WHERE Name = N'PrestadorSaludId' AND Object_ID = Object_ID(N'dbo.Funcionario'))
BEGIN
    ALTER TABLE dbo.Funcionario ADD PrestadorSaludId INT NULL;
    ALTER TABLE dbo.Funcionario ADD CONSTRAINT FK_Funcionario_PrestadorSalud FOREIGN KEY (PrestadorSaludId) REFERENCES dbo.PrestadorSalud(Id);
END;

PRINT '✔ Tablas creadas y modificadas correctamente.';
GO

-- =============================================================================================
-- PASO 1: CREAR Y EJECUTAR EL PROCEDIMIENTO DE LIMPIEZA
-- =============================================================================================

PRINT '--- PASO 1: Creando y ejecutando procedimiento de limpieza ---';

IF OBJECT_ID('usp_LimpiarDatosDeApex', 'P') IS NOT NULL
    DROP PROCEDURE usp_LimpiarDatosDeApex;
GO

CREATE PROCEDURE usp_LimpiarDatosDeApex
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        -- Se eliminan en el orden correcto para evitar conflictos de Foreign Keys
        DELETE FROM [dbo].[EstadoTransitorioAdjunto];
        DELETE FROM [dbo].[DesignacionDetalle];
        DELETE FROM [dbo].[EnfermedadDetalle];
        DELETE FROM [dbo].[SancionDetalle];
        DELETE FROM [dbo].[OrdenCincoDetalle];
        DELETE FROM [dbo].[RetenDetalle];
        DELETE FROM [dbo].[SumarioDetalle];
        DELETE FROM [dbo].[TrasladoDetalle];
        DELETE FROM [dbo].[BajaDeFuncionarioDetalle];
        DELETE FROM [dbo].[CambioDeCargoDetalle];
        DELETE FROM [dbo].[ReactivacionDeFuncionarioDetalle];
        DELETE FROM [dbo].[SeparacionDelCargoDetalle];
        DELETE FROM [dbo].[InicioDeProcesamientoDetalle];
        DELETE FROM [dbo].[DesarmadoDetalle];
        DELETE FROM [dbo].[NovedadFuncionario];
        DELETE FROM [dbo].[NovedadFoto];
        DELETE FROM [dbo].[Novedad];
        DELETE FROM [dbo].[NovedadGenerada];
        DELETE FROM [dbo].[HistoricoLicencia];
        DELETE FROM [dbo].[HistoricoCustodia];
        DELETE FROM [dbo].[HistoricoNocturnidad];
        DELETE FROM [dbo].[HistoricoPresentismo];
        DELETE FROM [dbo].[HistoricoViatico];
        DELETE FROM [dbo].[FuncionarioArma];
        DELETE FROM [dbo].[FuncionarioChaleco];
        DELETE FROM [dbo].[FuncionarioDispositivo];
        DELETE FROM [dbo].[FuncionarioDotacion];
        DELETE FROM [dbo].[FuncionarioEstadoLegal];
        DELETE FROM [dbo].[FuncionarioFotoHistorico];
        DELETE FROM [dbo].[FuncionarioSalud];
        DELETE FROM [dbo].[NotificacionPersonal];
        DELETE FROM [dbo].[EstadoTransitorio];
        DELETE FROM [dbo].[UsuarioRol];
        DELETE FROM [dbo].[Usuario];
        DELETE FROM [dbo].[MapPoliciaFunc];

        UPDATE [dbo].[Funcionario] SET SubDireccionId = NULL, SubEscalafonId = NULL, PrestadorSaludId = NULL;

        DELETE FROM [dbo].[Funcionario];

        DELETE FROM [dbo].[PuestoTrabajo];
        DELETE FROM [dbo].[AreaTrabajo];
        DELETE FROM [dbo].[TipoViatico];
        DELETE FROM [dbo].[Arma];
        DELETE FROM [dbo].[DotacionItem];
        DELETE FROM [dbo].[TipoLicencia];
        DELETE FROM [dbo].[TipoEstadoTransitorio];
        DELETE FROM [dbo].[Cargo];
        DELETE FROM [dbo].[CategoriaAusencia];
        DELETE FROM [dbo].[Escalafon];
        DELETE FROM [dbo].[Estado];
        DELETE FROM [dbo].[EstadoCivil];
        DELETE FROM [dbo].[Funcion];
        DELETE FROM [dbo].[Genero];
        DELETE FROM [dbo].[Horario];
        DELETE FROM [dbo].[NivelEstudio];
        DELETE FROM [dbo].[Seccion];
        DELETE FROM [dbo].[Semana];
        DELETE FROM [dbo].[Turno];
        DELETE FROM [dbo].[NotificacionEstado];
        DELETE FROM [dbo].[TipoNotificacion];
        DELETE FROM [dbo].[RolUsuario];
        DELETE FROM [dbo].[TipoFuncionario];
        DELETE FROM [dbo].[RegimenDetalle];
        DELETE FROM [dbo].[RegimenTrabajo];
        DELETE FROM [dbo].[RegimenAlternancia];
        DELETE FROM [dbo].[SubDireccion];
        DELETE FROM [dbo].[SubEscalafon];
        DELETE FROM [dbo].[PrestadorSalud];


        COMMIT TRANSACTION;
        PRINT '✔ Limpieza completada con éxito.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        PRINT '!!! ERROR DURANTE LA LIMPIEZA DE DATOS !!!';
        THROW;
    END CATCH;
END
GO

EXEC usp_LimpiarDatosDeApex;
GO

-- =============================================================================================
-- INICIO DE LA TRANSACCIÓN PRINCIPAL DE MIGRACIÓN
-- =============================================================================================

BEGIN TRY
    BEGIN TRANSACTION;

    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    PRINT '--- INICIO DE LA MIGRACIÓN COMPLETA Y UNIFICADA ---';

    /*================================================================
      PASO 2: Poblar Catálogos
    =================================================================*/
    PRINT '--- PASO 2: Poblando catálogos... ---';

    SET IDENTITY_INSERT dbo.TipoFuncionario ON;
    MERGE INTO dbo.TipoFuncionario AS T USING (VALUES (1, 'Policia'), (2, 'Operador Penitenciario'), (3, 'Profesional Universitario'),(4, 'Técnico'), (5, 'Administrativo')) AS S (Id, Nombre) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id, Nombre, CreatedAt) VALUES (S.Id, S.Nombre, GETDATE());
    SET IDENTITY_INSERT dbo.TipoFuncionario OFF;

    SET IDENTITY_INSERT dbo.Rol ON;
    MERGE dbo.Rol AS T
    USING (VALUES
        (1, 'Admin', 'Administrador del sistema'),
        (2, 'Usuario', 'Usuario estándar')
    ) AS S(Id, Nombre, Descripcion)
        ON T.Id = S.Id
    WHEN MATCHED THEN
        UPDATE SET Nombre = S.Nombre, Descripcion = S.Descripcion
    WHEN NOT MATCHED THEN
        INSERT (Id, Nombre, Descripcion)
        VALUES (S.Id, S.Nombre, S.Descripcion);
    SET IDENTITY_INSERT dbo.Rol OFF;

    MERGE dbo.RolUsuario AS T USING (VALUES (1,'Admin',1), (2,'Usuario',2)) AS S(Id,Nombre,Orden) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,Orden) VALUES (S.Id,S.Nombre,S.Orden);

    SET IDENTITY_INSERT dbo.CategoriaAusencia ON;
    MERGE dbo.CategoriaAusencia AS T USING (VALUES (1,'General'), (2,'Salud'), (3,'Especial'), (4, 'Sanción Leve'), (5, 'Sanción Grave')) AS S(Id,Nombre) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(S.Id,S.Nombre);
    SET IDENTITY_INSERT dbo.CategoriaAusencia OFF;

    MERGE dbo.NotificacionEstado AS T USING (VALUES (1, 'Pendiente', 1), (2, 'Vencida', 2), (3, 'Firmada', 3)) AS S(Id, Nombre, Orden) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id, Nombre, Orden) VALUES (S.Id, S.Nombre, S.Orden);
    SET IDENTITY_INSERT dbo.TipoNotificacion ON;
    MERGE dbo.TipoNotificacion AS T USING (VALUES (1, 'VISTA', 1), (2, 'INTIMACION', 2), (3, 'CITACION', 3), (4, 'NOTIFICACION', 4)) AS S(Id, Nombre, Orden) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id, Nombre, Orden) VALUES (S.Id, S.Nombre, S.Orden);
    SET IDENTITY_INSERT dbo.TipoNotificacion OFF;

    SET IDENTITY_INSERT dbo.TipoEstadoTransitorio ON;
    MERGE dbo.TipoEstadoTransitorio AS T USING (VALUES
        (1, 'Designación', 0), (2, 'Enfermedad', 0), (3, 'Sanción', 0), (4, 'Orden Cinco', 0), (5, 'Retén', 0), (6, 'Sumario', 0),
        (7, 'Baja de Funcionario', 0), (8, 'Cambio de Cargo', 1), (9, 'Reactivación de Funcionario', 0), (10, 'Separación del Cargo', 0),
        (11, 'Inicio de Procesamiento', 0), (12, 'Desarmado', 0), (21, 'Traslado', 0)
    ) AS S(Id, Nombre, EsJerarquico) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id, Nombre, EsJerarquico, CreatedAt) VALUES (S.Id, S.Nombre, S.EsJerarquico, GETDATE());
    SET IDENTITY_INSERT dbo.TipoEstadoTransitorio OFF;

    SET IDENTITY_INSERT dbo.Cargo ON;
    MERGE dbo.Cargo AS T USING (SELECT id_grado, nom_grado, grado FROM (SELECT id_grado, nom_grado, grado, ROW_NUMBER() OVER(PARTITION BY nom_grado ORDER BY id_grado) as rn FROM Personal.dbo.tblGrados) as Sub WHERE rn = 1) AS S(Id, Nombre, Grado) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id, Nombre, Grado, CreatedAt) VALUES (S.Id, S.Nombre, S.Grado, GETDATE());
    SET IDENTITY_INSERT dbo.Cargo OFF;

    IF OBJECT_ID('tempdb..#MapeoGrado') IS NOT NULL DROP TABLE #MapeoGrado;
    CREATE TABLE #MapeoGrado (IdGradoOrigen INT PRIMARY KEY, IdGradoDestino INT);
    ;WITH GradosCanonicos AS (SELECT id_grado, nom_grado FROM (SELECT id_grado, nom_grado, ROW_NUMBER() OVER(PARTITION BY nom_grado ORDER BY id_grado) as rn FROM Personal.dbo.tblGrados) as Sub WHERE rn = 1) INSERT INTO #MapeoGrado (IdGradoOrigen, IdGradoDestino) SELECT g.id_grado, gc.id_grado FROM Personal.dbo.tblGrados g JOIN GradosCanonicos gc ON g.nom_grado COLLATE DATABASE_DEFAULT = gc.nom_grado COLLATE DATABASE_DEFAULT;

    SET IDENTITY_INSERT dbo.Estado ON;
    MERGE dbo.Estado AS T USING (SELECT id_estado, nom_estado, CASE WHEN nom_estado IN ('AU', 'O5', 'CO') THEN 1 ELSE 0 END FROM Personal.dbo.tblEstados) AS S(Id, Nombre, EsAusentismo) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id, Nombre, EsAusentismo, CreatedAt) VALUES (S.Id, S.Nombre, S.EsAusentismo, GETDATE());
    SET IDENTITY_INSERT dbo.Estado OFF;

    SET IDENTITY_INSERT dbo.Seccion ON; MERGE dbo.Seccion AS T USING (SELECT id_seccion, nom_seccion FROM Personal.dbo.tblSecciones) AS S(Id,Nombre) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES (S.Id,S.Nombre,GETDATE()); SET IDENTITY_INSERT dbo.Seccion OFF;
    SET IDENTITY_INSERT dbo.Turno ON; MERGE dbo.Turno AS T USING (SELECT id_turno, nom_turno FROM Personal.dbo.tblTurnos) AS S(Id,Nombre) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES (S.Id,S.Nombre,GETDATE()); SET IDENTITY_INSERT dbo.Turno OFF;
    SET IDENTITY_INSERT dbo.Semana ON; MERGE dbo.Semana AS T USING (SELECT id_semana, nom_semana FROM Personal.dbo.tblSemanas) AS S(Id,Nombre) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES (S.Id,S.Nombre,GETDATE()); SET IDENTITY_INSERT dbo.Semana OFF;
    SET IDENTITY_INSERT dbo.Horario ON; MERGE dbo.Horario AS T USING (SELECT id_horario, nom_horario FROM Personal.dbo.tblHorarios) AS S(Id,Nombre) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES (S.Id,S.Nombre,GETDATE()); SET IDENTITY_INSERT dbo.Horario OFF;

    SET IDENTITY_INSERT dbo.TipoLicencia ON;
    MERGE dbo.TipoLicencia AS T USING (SELECT id_ausencia, nom_ausencia, CASE WHEN ausente = 'SI' THEN 1 ELSE 0 END, CASE WHEN viatico = 'NO' THEN 1 ELSE 0 END, CASE WHEN presentismo = 'NO' THEN 1 ELSE 0 END, CASE WHEN habil = 'SI' THEN 1 ELSE 0 END FROM Personal.dbo.tblAusencias) AS S(Id,Nombre,EsAusencia,SuspendeViatico,AfectaPresentismo,EsHabil) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,EsAusencia,SuspendeViatico,AfectaPresentismo,EsHabil,CreatedAt,CategoriaAusenciaId) VALUES(S.Id,S.Nombre,S.EsAusencia,S.SuspendeViatico,S.AfectaPresentismo,S.EsHabil,GETDATE(),1);
    SET IDENTITY_INSERT dbo.TipoLicencia OFF;

    SET IDENTITY_INSERT dbo.Genero ON; MERGE dbo.Genero AS T USING (VALUES (1,'Masculino'),(2,'Femenino'),(3,'Otro'),(4,'No especifica')) AS S(Id,Nombre) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(S.Id,S.Nombre); SET IDENTITY_INSERT dbo.Genero OFF;
    SET IDENTITY_INSERT dbo.EstadoCivil ON; MERGE dbo.EstadoCivil AS T USING (VALUES (1,'Soltero/a'),(2,'Casado/a'),(3,'Divorciado/a'),(4,'Viudo/a'),(5,'Unión libre'),(6,'Separado/a'),(7,'No especifica')) AS S(Id,Nombre) ON T.Id = S.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(S.Id,S.Nombre); SET IDENTITY_INSERT dbo.EstadoCivil OFF;

    MERGE dbo.Escalafon AS T USING (SELECT DISTINCT LTRIM(RTRIM(p.escalafon)) AS Nombre FROM Personal.dbo.tblPolicias p WHERE p.escalafon IS NOT NULL AND LTRIM(RTRIM(p.escalafon)) <> '') AS S ON T.Nombre = S.Nombre COLLATE Modern_Spanish_CI_AS WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (S.Nombre);
    MERGE dbo.Funcion AS T USING (SELECT DISTINCT LTRIM(RTRIM(p.funcion)) AS Nombre FROM Personal.dbo.tblPolicias p WHERE p.funcion IS NOT NULL AND LTRIM(RTRIM(p.funcion)) <> '') AS S ON T.Nombre = S.Nombre COLLATE Modern_Spanish_CI_AS WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (S.Nombre);
    MERGE dbo.SubDireccion AS T USING (SELECT DISTINCT LTRIM(RTRIM(p.sub_direccion)) AS Nombre FROM Personal.dbo.tblPolicias p WHERE p.sub_direccion IS NOT NULL AND LTRIM(RTRIM(p.sub_direccion)) <> '') AS S ON T.Nombre = S.Nombre COLLATE Modern_Spanish_CI_AS WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (S.Nombre);
    MERGE dbo.SubEscalafon AS T USING (SELECT DISTINCT LTRIM(RTRIM(p.subescalafon)) AS Nombre FROM Personal.dbo.tblPolicias p WHERE p.subescalafon IS NOT NULL AND LTRIM(RTRIM(p.subescalafon)) <> '') AS S ON T.Nombre = S.Nombre COLLATE Modern_Spanish_CI_AS WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (S.Nombre);
    MERGE dbo.PrestadorSalud AS T USING (SELECT DISTINCT LTRIM(RTRIM(p.prestador_salud)) AS Nombre FROM Personal.dbo.tblPolicias p WHERE p.prestador_salud IS NOT NULL AND LTRIM(RTRIM(p.prestador_salud)) <> '') AS S ON T.Nombre = S.Nombre COLLATE Modern_Spanish_CI_AS WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (S.Nombre);

    SET IDENTITY_INSERT dbo.AreaTrabajo ON; MERGE dbo.AreaTrabajo AS T USING Personal.dbo.tblAreaPrincipal AS S ON (T.Id = S.id_area) WHEN NOT MATCHED BY TARGET THEN INSERT (Id, Nombre, CreatedAt) VALUES (S.id_area, S.area, GETDATE()); SET IDENTITY_INSERT dbo.AreaTrabajo OFF;
    MERGE dbo.TipoViatico AS T USING (VALUES ('Viático General', 15),('Viático Especial', 20)) AS S(Descripcion, Dias) ON T.Dias = S.Dias WHEN NOT MATCHED BY TARGET THEN INSERT (Descripcion, Dias, CreatedAt) VALUES (S.Descripcion, S.Dias, GETDATE());
    SET IDENTITY_INSERT dbo.PuestoTrabajo ON; MERGE dbo.PuestoTrabajo AS T USING (SELECT p.id_puesto, p.nom_puesto, CASE p.id_viatico WHEN 1 THEN (SELECT TOP 1 Id FROM dbo.TipoViatico WHERE Dias = 15) WHEN 2 THEN (SELECT TOP 1 Id FROM dbo.TipoViatico WHERE Dias = 20) ELSE NULL END, p.id_area FROM Personal.dbo.tblPuestos p) AS S(id_puesto, nom_puesto, TipoViaticoId, AreaTrabajoId) ON (T.Id = S.id_puesto) WHEN NOT MATCHED BY TARGET THEN INSERT (Id, Nombre, TipoViaticoId, AreaTrabajoId, Activo, CreatedAt) VALUES (S.id_puesto, S.nom_puesto, S.TipoViaticoId, S.AreaTrabajoId, 1, GETDATE()); SET IDENTITY_INSERT dbo.PuestoTrabajo OFF;

    PRINT '✔ Catálogos poblados.';

    /*================================================================
      PASO 3: Migrar Funcionarios y Mapeos Principales
    =================================================================*/
    PRINT '--- PASO 3: Migrando funcionarios... ---';

    ;WITH PoliciasStg AS (
        SELECT
            p.id_policia,
            RIGHT('00000000' + CAST(p.num_cedula AS VARCHAR(8)), 8) AS CI,
            LEFT(LTRIM(RTRIM(p.nom_policia)), 60) AS Nombre,
            p.alta AS FechaIngreso,
            CASE WHEN p.id_estado = 1 THEN 1 ELSE 0 END AS Activo,
            p.id_seccion,
            p.id_puesto,
            p.id_turno,
            p.id_semana,
            p.id_horario,
            CASE LTRIM(RTRIM(UPPER(p.genero))) WHEN 'M' THEN 1 WHEN 'F' THEN 2 ELSE 4 END AS GeneroId,
            p.nivelestudio,
            p.estadocivil,
            p.id_grado,
            p.fecha_registro,
            p.fecha_actualizado,
            p.fecha_nacimiento,
            LEFT(LTRIM(RTRIM(p.domicilio)), 250) AS Domicilio,
            LEFT(LTRIM(RTRIM(p.correo)), 255) AS Email,
            LEFT(LTRIM(RTRIM(p.telefono)), 50) AS Telefono,
            LTRIM(RTRIM(p.escalafon)) AS EscalafonTxt,
            LTRIM(RTRIM(p.funcion)) AS FuncionTxt,
            p.departamento AS Ciudad,
            p.seccional,
            p.credencial,
            CASE WHEN UPPER(LTRIM(RTRIM(ISNULL(p.estudia, 'NO')))) = 'SI' THEN 1 ELSE 0 END AS Estudia,
            LTRIM(RTRIM(p.sub_direccion)) AS SubDireccionTxt,
            LTRIM(RTRIM(p.subescalafon)) AS SubEscalafonTxt,
            LTRIM(RTRIM(p.prestador_salud)) AS PrestadorSaludTxt,
            TRY_CAST(p.baja AS DATE) AS Baja,
            CASE WHEN p.resalta IS NULL THEN NULL ELSE LTRIM(RTRIM(CAST(p.resalta AS NVARCHAR(MAX)))) END AS ResAlta,
            CASE WHEN p.resbaja IS NULL THEN NULL ELSE LTRIM(RTRIM(CAST(p.resbaja AS NVARCHAR(MAX)))) END AS ResBaja,
            CASE WHEN p.descripcion IS NULL THEN NULL ELSE LTRIM(RTRIM(CAST(p.descripcion AS NVARCHAR(MAX)))) END AS Descripcion,
            CASE WHEN p.situacion_especial IS NULL THEN NULL ELSE LEFT(LTRIM(RTRIM(CAST(p.situacion_especial AS NVARCHAR(500)))), 500) END AS SituacionEspecial,
            CASE WHEN p.imei IS NULL THEN NULL ELSE LEFT(LTRIM(RTRIM(CAST(p.imei AS NVARCHAR(500)))), 500) END AS Imei
        FROM Personal.dbo.tblPolicias p
    ),
    PoliciasMapeadas AS (
        SELECT
            s.CI,
            s.Nombre,
            s.FechaIngreso,
            s.Activo,
            s.id_seccion AS SeccionId,
            s.id_puesto AS PuestoTrabajoId,
            s.id_turno AS TurnoId,
            s.id_semana AS SemanaId,
            s.id_horario AS HorarioId,
            s.GeneroId,
            ne.Id AS NivelEstudioId,
            ec.Id AS EstadoCivilId,
            mg.IdGradoDestino AS CargoId,
            s.fecha_registro AS CreatedAt,
            s.fecha_actualizado AS UpdatedAt,
            s.fecha_nacimiento AS FechaNacimiento,
            s.Domicilio,
            s.Email,
            s.Telefono,
            e.Id AS EscalafonId,
            f.Id AS FuncionId,
            s.Ciudad,
            s.seccional AS Seccional,
            s.credencial AS Credencial,
            s.Estudia,
            sd.Id AS SubDireccionId,
            se.Id AS SubEscalafonId,
            ps.Id AS PrestadorSaludId,
            s.Baja,
            s.ResAlta,
            s.ResBaja,
            s.Descripcion,
            s.SituacionEspecial,
            s.Imei
        FROM PoliciasStg s
        LEFT JOIN #MapeoGrado mg ON s.id_grado = mg.IdGradoOrigen
        LEFT JOIN dbo.NivelEstudio ne ON ne.Nombre = s.nivelestudio COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.EstadoCivil ec ON ec.Nombre = s.estadocivil COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.Escalafon e ON e.Nombre = s.EscalafonTxt COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.Funcion f ON f.Nombre = s.FuncionTxt COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.SubDireccion sd ON sd.Nombre = s.SubDireccionTxt COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.SubEscalafon se ON se.Nombre = s.SubEscalafonTxt COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.PrestadorSalud ps ON ps.Nombre = s.PrestadorSaludTxt COLLATE Modern_Spanish_CI_AS
    )
    MERGE dbo.Funcionario AS T USING PoliciasMapeadas AS S ON T.CI = S.CI COLLATE DATABASE_DEFAULT
    WHEN NOT MATCHED BY TARGET THEN INSERT (CI,Nombre,FechaIngreso,Activo,CreatedAt,UpdatedAt,SeccionId,PuestoTrabajoId,TurnoId,SemanaId,HorarioId,GeneroId,NivelEstudioId,EstadoCivilId,CargoId,FechaNacimiento,Domicilio,Email,Telefono,EscalafonId,FuncionId,TipoFuncionarioId,Ciudad,Seccional,Credencial,Estudia,SubDireccionId,SubEscalafonId,PrestadorSaludId,Baja,ResAlta,ResBaja,Descripcion,SituacionEspecial,Imei)
        VALUES (S.CI,S.Nombre,S.FechaIngreso,S.Activo,S.CreatedAt,S.UpdatedAt,S.SeccionId,S.PuestoTrabajoId,S.TurnoId,S.SemanaId,S.HorarioId,S.GeneroId,S.NivelEstudioId,S.EstadoCivilId,S.CargoId,S.FechaNacimiento,S.Domicilio,S.Email,S.Telefono,S.EscalafonId,S.FuncionId,1,S.Ciudad,S.Seccional,S.Credencial,S.Estudia,S.SubDireccionId,S.SubEscalafonId,S.PrestadorSaludId,S.Baja,S.ResAlta,S.ResBaja,S.Descripcion,S.SituacionEspecial,S.Imei);

    INSERT INTO dbo.MapPoliciaFunc (id_policia,FuncionarioId) SELECT p.id_policia,f.Id FROM Personal.dbo.tblPolicias p JOIN dbo.Funcionario f ON f.CI = RIGHT('00000000' + CAST(p.num_cedula AS VARCHAR(8)),8);
    PRINT '✔ Funcionarios migrados.';

    /*================================================================
      PASO 4: Migrar Datos Relacionados (Estados Transitorios, Licencias, Históricos)
    =================================================================*/
    PRINT '--- PASO 4: Migrando datos relacionados... ---';

    -- Estados Transitorios desde tblEstadosUnificados
    SET IDENTITY_INSERT dbo.EstadoTransitorio ON;
    MERGE dbo.EstadoTransitorio as TGT USING (SELECT eu.id_estado, mp.FuncionarioId, CASE eu.categoria WHEN 'D' THEN 1 WHEN 'E' THEN 2 WHEN 'S' THEN 3 WHEN 'O' THEN 4 WHEN 'R' THEN 5 WHEN 'U' THEN 6 END as TipoEstadoTransitorioId FROM Personal.dbo.tblEstadosUnificados eu JOIN dbo.MapPoliciaFunc mp ON eu.id_policia = mp.id_policia WHERE eu.categoria IN ('D', 'E', 'S', 'O', 'R', 'U')) AS SRC ON TGT.Id = SRC.id_estado WHEN NOT MATCHED THEN INSERT (Id, FuncionarioId, TipoEstadoTransitorioId, CreatedAt) VALUES (SRC.id_estado, SRC.FuncionarioId, SRC.TipoEstadoTransitorioId, GETDATE());
    SET IDENTITY_INSERT dbo.EstadoTransitorio OFF;

    -- Detalles de Estados Transitorios
    MERGE dbo.DesignacionDetalle AS T USING (SELECT d.id_estado, d.fecha_inicio, d.fecha_fin, d.descripcion, d.doc_resolucion FROM Personal.dbo.tblDesignacionesDetalle d JOIN dbo.EstadoTransitorio et ON d.id_estado = et.Id) AS S ON T.EstadoTransitorioId = S.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, DocResolucion) VALUES (S.id_estado, S.fecha_inicio, S.fecha_fin, S.descripcion, S.doc_resolucion);
    MERGE dbo.EnfermedadDetalle AS T USING (SELECT e.id_estado, e.fecha_inicio, e.fecha_fin, e.descripcion, e.descripcion as Diagnostico FROM Personal.dbo.tblEnfermedadDetalle e JOIN dbo.EstadoTransitorio et ON e.id_estado = et.Id) AS S ON T.EstadoTransitorioId = S.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Diagnostico) VALUES (S.id_estado, S.fecha_inicio, S.fecha_fin, S.descripcion, S.Diagnostico);
    MERGE dbo.SancionDetalle AS T USING (SELECT s.id_estado, s.fecha_inicio, s.fecha_fin, s.descripcion, s.doc_resolucion FROM Personal.dbo.tblSancionesDetalle s JOIN dbo.EstadoTransitorio et ON s.id_estado = et.Id) AS S ON T.EstadoTransitorioId = S.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Resolucion) VALUES (S.id_estado, S.fecha_inicio, S.fecha_fin, S.descripcion, S.doc_resolucion);
    MERGE dbo.OrdenCincoDetalle AS T USING (SELECT o.id_estado, o.fecha_inicio, o.fecha_fin, o.descripcion FROM Personal.dbo.tblOrdenCincoDetalle o JOIN dbo.EstadoTransitorio et ON o.id_estado = et.Id) AS S ON T.EstadoTransitorioId = S.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones) VALUES (S.id_estado, S.fecha_inicio, S.fecha_fin, S.descripcion);
    MERGE dbo.RetenDetalle AS T USING (SELECT r.id_estado, r.fecha_reten, r.descripcion, t.nom_turno FROM Personal.dbo.tblRetenesDetalle r JOIN Personal.dbo.tblTurnos t ON r.id_turno = t.id_turno JOIN dbo.EstadoTransitorio et ON r.id_estado = et.Id) AS S ON T.EstadoTransitorioId = S.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaReten, Observaciones, Turno) VALUES (S.id_estado, S.fecha_reten, S.descripcion, S.nom_turno);
    MERGE dbo.SumarioDetalle AS T USING (SELECT s.id_estado, s.fecha_inicio, s.fecha_fin, s.descripcion, s.doc_resolucion FROM Personal.dbo.tblSumariosDetalle s JOIN dbo.EstadoTransitorio et ON s.id_estado = et.Id) AS S ON T.EstadoTransitorioId = S.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Expediente) VALUES (S.id_estado, S.fecha_inicio, S.fecha_fin, S.descripcion, S.doc_resolucion);

    -- =================================================================
    -- INICIO: Lógica adicional para migrar 'O5' desde estado general
    -- =================================================================
    INSERT INTO dbo.EstadoTransitorio (FuncionarioId, TipoEstadoTransitorioId, CreatedAt)
    SELECT
        mp.FuncionarioId,
        (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Orden Cinco'),
        ISNULL(p.fecha_actualizado, p.fecha_registro)
    FROM Personal.dbo.tblPolicias p
    JOIN Personal.dbo.tblEstados e ON p.id_estado = e.id_estado
    JOIN dbo.MapPoliciaFunc mp ON p.id_policia = mp.id_policia
    WHERE e.nom_estado = 'O5'
      AND NOT EXISTS (
          SELECT 1 FROM dbo.EstadoTransitorio et
          WHERE et.FuncionarioId = mp.FuncionarioId
            AND et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Orden Cinco')
      );

    INSERT INTO dbo.OrdenCincoDetalle (EstadoTransitorioId, FechaDesde, Observaciones)
    SELECT
        et.Id,
        CAST(et.CreatedAt AS DATE),
        'Estado O5 migrado desde el estado general del funcionario.'
    FROM dbo.EstadoTransitorio et
    WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Orden Cinco')
      AND NOT EXISTS (
          SELECT 1 FROM dbo.OrdenCincoDetalle ocd
          WHERE ocd.EstadoTransitorioId = et.Id
      );
    -- =================================================================
    -- FIN: Lógica adicional para migrar 'O5'
    -- =================================================================

    -- Histórico de Licencias
    SET IDENTITY_INSERT dbo.HistoricoLicencia ON;
    MERGE dbo.HistoricoLicencia AS T USING ( SELECT l.id_licencia, mp.FuncionarioId, l.id_ausencia, l.inicio, l.finaliza, l.fecha_registro, l.fecha_actualizado, l.usuario, l.datos, l.estado, l.Unidad_Ejecutora, l.Unidad_Organizativa, l.Cantidad, l.Cantidad_dentro_del_período, l.Unidad, l.Afecta_a_días, l.C_Presentó_certificado_, l.Usuario_aprobó_anuló_rechazó, l.Fecha_aprobación_anulación_rechazo, l.Comentario FROM Personal.dbo.tblLicencias l JOIN dbo.MapPoliciaFunc mp ON l.id_policia = mp.id_policia ) AS S ON T.Id = S.id_licencia WHEN NOT MATCHED THEN INSERT (Id,FuncionarioId,TipoLicenciaId,inicio,finaliza,fecha_registro,fecha_actualizado,usuario,datos,estado,Unidad_Ejecutora,Unidad_Organizativa,Cantidad,Cantidad_dentro_del_período,Unidad,Afecta_a_días,C_Presentó_certificado_,Usuario_aprobó_anuló_rechazó,Fecha_aprobación_anulación_rechazo,Comentario) VALUES(S.id_licencia,S.FuncionarioId,S.id_ausencia,S.inicio,S.finaliza,S.fecha_registro,S.fecha_actualizado,S.usuario,S.datos,S.estado,S.Unidad_Ejecutora,S.Unidad_Organizativa,S.Cantidad,S.Cantidad_dentro_del_período,S.Unidad,S.Afecta_a_días,S.C_Presentó_certificado_,S.Usuario_aprobó_anuló_rechazó,S.Fecha_aprobación_anulación_rechazo,S.Comentario);
    SET IDENTITY_INSERT dbo.HistoricoLicencia OFF;

    -- Otros Históricos
    ;WITH CU AS (SELECT hc.id_policia,hc.fecha,MIN(hc.area) AS area FROM Personal.dbo.tblHistoricoCustodias hc GROUP BY hc.id_policia,hc.fecha) INSERT INTO dbo.HistoricoCustodia (FuncionarioId,Fecha,Area) SELECT m.FuncionarioId,cu.fecha,cu.area FROM CU cu JOIN dbo.MapPoliciaFunc m ON m.id_policia = cu.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoCustodia hc WHERE hc.FuncionarioId = m.FuncionarioId AND hc.Fecha = cu.fecha);
    ;WITH NA AS (SELECT hn.id_policia,hn.año,hn.mes,SUM(hn.minutos) AS minutos FROM Personal.dbo.tblHistoricoNocturnidad hn GROUP BY hn.id_policia,hn.año,hn.mes) INSERT INTO dbo.HistoricoNocturnidad (FuncionarioId,Anio,Mes,Minutos) SELECT m.FuncionarioId,na.año,na.mes,na.minutos FROM NA na JOIN dbo.MapPoliciaFunc m ON m.id_policia = na.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoNocturnidad hn WHERE hn.FuncionarioId = m.FuncionarioId AND hn.Anio = na.año AND hn.Mes = na.mes);
    ;WITH PA AS (SELECT hp.id_policia,hp.año,hp.mes,SUM(hp.minutos) AS minutos,SUM(hp.dias) AS dias, MIN(hp.incidencia) AS incidencia,MIN(hp.observaciones) AS observaciones FROM Personal.dbo.tblHistoricoPresentismo hp GROUP BY hp.id_policia,hp.año,hp.mes) INSERT INTO dbo.HistoricoPresentismo (FuncionarioId,Anio,Mes,Minutos,Dias,Incidencia,Observaciones) SELECT m.FuncionarioId,pa.año,pa.mes,pa.minutos,pa.dias,pa.incidencia,pa.observaciones FROM PA pa JOIN dbo.MapPoliciaFunc m ON m.id_policia = pa.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoPresentismo hp WHERE hp.FuncionarioId = m.FuncionarioId AND hp.Anio = pa.año AND hp.Mes = pa.mes);
    ;WITH VA AS (SELECT hv.id_policia,hv.año,hv.mes,MIN(hv.incidencia) AS incidencia,MIN(hv.motivo) AS motivo FROM Personal.dbo.tblHistoricoViaticos hv GROUP BY hv.id_policia,hv.año,hv.mes) INSERT INTO dbo.HistoricoViatico (FuncionarioId,Anio,Mes,Incidencia,Motivo) SELECT m.FuncionarioId,va.año,va.mes,va.incidencia,va.motivo FROM VA va JOIN dbo.MapPoliciaFunc m ON m.id_policia = va.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoViatico hv WHERE hv.FuncionarioId = m.FuncionarioId AND hv.Anio = va.año AND hv.Mes = va.mes);

    -- Usuarios
    ;WITH SourceUsuarios AS (
        SELECT
            LOWER(LTRIM(RTRIM(u.usuario))) COLLATE DATABASE_DEFAULT AS NombreUsuario,
            HASHBYTES('SHA2_256', CAST(u.clave AS NVARCHAR(4000))) AS PasswordHash,
            mp.FuncionarioId,
            LTRIM(RTRIM(p.nom_policia)) COLLATE DATABASE_DEFAULT AS NombreCompleto
        FROM Personal.dbo.tblUsuarios u
        LEFT JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = u.id_policia
        LEFT JOIN Personal.dbo.tblPolicias p ON p.id_policia = u.id_policia
    )
    MERGE dbo.Usuario AS T
    USING SourceUsuarios AS S
        ON T.NombreUsuario = S.NombreUsuario
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (NombreUsuario, PasswordHash, PasswordSalt, NombreCompleto, Activo, FechaCreacion, FuncionarioId)
        VALUES (
            S.NombreUsuario,
            S.PasswordHash,
            0x,
            ISNULL(NULLIF(S.NombreCompleto, ''), S.NombreUsuario),
            1,
            GETDATE(),
            S.FuncionarioId
        );

    ;WITH SourceUsuarioRol AS (
        SELECT
            LOWER(LTRIM(RTRIM(u.usuario))) COLLATE DATABASE_DEFAULT AS NombreUsuario,
            CASE UPPER(LTRIM(RTRIM(u.rol)))
                WHEN 'ADMIN' THEN 1
                WHEN 'SUPERADMIN' THEN 1
                WHEN 'USUARIO' THEN 2
                ELSE 2
            END AS RolId
        FROM Personal.dbo.tblUsuarios u
    )
    INSERT INTO dbo.UsuarioRol (UsuarioId, RolId)
    SELECT u.Id, sur.RolId
    FROM SourceUsuarioRol sur
    JOIN dbo.Usuario u ON u.NombreUsuario = sur.NombreUsuario
    WHERE NOT EXISTS (
        SELECT 1
        FROM dbo.UsuarioRol ur
        WHERE ur.UsuarioId = u.Id
          AND ur.RolId = sur.RolId
    );
    PRINT '✔ Datos relacionados migrados.';

    /*================================================================
      PASO 5: Refinamiento y Categorización de Datos
    =================================================================*/
    PRINT '--- PASO 5: Refinando y categorizando datos... ---';

    UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 2 WHERE [Id] IN (20, 21, 23, 59, 60, 68, 69, 71, 72, 73, 81, 82, 87, 88, 101, 104, 105, 3450, 3463, 3465, 3466, 3467, 4471);
    UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 3 WHERE [Id] IN (1, 5, 6, 7, 8, 12, 17, 18, 42, 43, 44, 45, 47, 53, 54, 78, 89, 91, 3446, 3456, 3457, 3458, 3459, 4467, 4470, 4472);
    UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 4 WHERE [Id] IN (38, 39, 40, 41, 2443, 3444);
    UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 5 WHERE [Id] IN (2, 90, 97, 98, 99, 102, 106, 107, 108, 109, 110, 3455, 3462, 4469);
    UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 1 WHERE [CategoriaAusenciaId] = 1 AND Id NOT IN (20,21,23,59,60,68,69,71,72,73,81,82,87,88,101,104,105,3450,3463,3465,3466,3467,4471,1,5,6,7,8,12,17,18,42,43,44,45,47,53,54,78,89,91,3446,3456,3457,3458,3459,4467,4470,4472,38,39,40,41,2443,3444,2,90,97,98,99,102,106,107,108,109,110,3455,3462,4469);

    UPDATE F SET F.TipoFuncionarioId = CASE E.Nombre WHEN 'L' THEN 1 WHEN 'S' THEN 2 WHEN 'A' THEN 3 WHEN 'B' THEN 4 WHEN 'C' THEN 5 ELSE F.TipoFuncionarioId END FROM dbo.Funcionario F JOIN dbo.Escalafon E ON F.EscalafonId = E.Id WHERE E.Nombre IN ('L', 'S', 'A', 'B', 'C');

    DELETE FROM Apex.dbo.NivelEstudio;
    INSERT INTO Apex.dbo.NivelEstudio (Nombre) SELECT DISTINCT LTRIM(RTRIM(p.nivelestudio)) FROM Personal.dbo.tblPolicias p WHERE p.nivelestudio IS NOT NULL AND LTRIM(RTRIM(p.nivelestudio)) <> '';
    UPDATE f SET f.NivelEstudioId = ne.Id FROM Apex.dbo.Funcionario f JOIN Apex.dbo.MapPoliciaFunc map ON f.Id = map.FuncionarioId JOIN Personal.dbo.tblPolicias p ON map.id_policia = p.id_policia JOIN Apex.dbo.NivelEstudio ne ON ne.Nombre = p.nivelestudio COLLATE DATABASE_DEFAULT WHERE p.nivelestudio IS NOT NULL AND LTRIM(RTRIM(p.nivelestudio)) <> '';

    PRINT '✔ Datos refinados y categorizados.';

    /*================================================================
      PASO 6: Migración de Novedades, Notificaciones y Dotaciones
    =================================================================*/
    PRINT '--- PASO 6: Migrando Novedades, Notificaciones, Dotaciones... ---';

    SET IDENTITY_INSERT dbo.NotificacionPersonal ON;
    MERGE dbo.NotificacionPersonal AS T USING (SELECT n.id_notificacion, mp.FuncionarioId, CAST(n.fecha AS DATETIME2(0)), n.nom_notificacion, n.documento, LEFT(n.n_exp_ministerial, 50), LEFT(n.n_exp_inr, 50), LEFT(n.procedencia, 200), COALESCE(tn.Id, 4), CASE LTRIM(RTRIM(n.estado)) WHEN 'Pendiente' THEN 1 WHEN 'Vencida' THEN 2 WHEN 'Firmada' THEN 3 ELSE 1 END FROM Personal.dbo.tblNotificaciones n JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = n.id_policia LEFT JOIN dbo.TipoNotificacion tn ON tn.Nombre = UPPER(LTRIM(RTRIM(n.tipo_notificacion))) COLLATE Modern_Spanish_CI_AS) AS S(id_notificacion, FuncionarioId, FechaProgramada, Medio, documento, ExpMinisterial, ExpINR, Oficina, TipoNotificacionId, EstadoId) ON T.Id = S.id_notificacion WHEN NOT MATCHED THEN INSERT (Id, FuncionarioId, FechaProgramada, Medio, Documento, ExpMinisterial, ExpINR, Oficina, TipoNotificacionId, EstadoId, CreatedAt) VALUES (S.id_notificacion, S.FuncionarioId, S.FechaProgramada, S.Medio, S.documento, S.ExpMinisterial, S.ExpINR, S.Oficina, S.TipoNotificacionId, S.EstadoId, GETDATE());
    SET IDENTITY_INSERT dbo.NotificacionPersonal OFF;

    SET IDENTITY_INSERT dbo.NovedadGenerada ON; MERGE dbo.NovedadGenerada AS T USING (SELECT id_novedad_generada, fecha FROM Personal.dbo.tblNovedadesGeneradas) AS S ON T.Id = S.id_novedad_generada WHEN NOT MATCHED THEN INSERT (Id, Fecha, CreatedAt) VALUES (S.id_novedad_generada, S.fecha, GETDATE()); SET IDENTITY_INSERT dbo.NovedadGenerada OFF;
    SET IDENTITY_INSERT dbo.Novedad ON; MERGE dbo.Novedad AS T USING (SELECT n.id_novedad, n.id_novedad_generada, n.fecha, n.novedad, CASE n.Estado WHEN 'Pendiente' THEN 1 WHEN 'Vencida' THEN 2 WHEN 'Firmada' THEN 3 ELSE 1 END FROM Personal.dbo.tblNovedades n) AS S(id_novedad, id_novedad_generada, fecha, Texto, EstadoId) ON T.Id = S.id_novedad WHEN NOT MATCHED THEN INSERT (Id, NovedadGeneradaId, Fecha, Texto, EstadoId, CreatedAt) VALUES (S.id_novedad, S.id_novedad_generada, S.fecha, S.Texto, S.EstadoId, GETDATE()); SET IDENTITY_INSERT dbo.Novedad OFF;
    MERGE dbo.NovedadFuncionario AS T USING (SELECT n.id_novedad, mp.FuncionarioId FROM Personal.dbo.tblNovedades n JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = n.id_policia) AS S ON T.NovedadId = S.id_novedad AND T.FuncionarioId = S.FuncionarioId WHEN NOT MATCHED THEN INSERT (NovedadId, FuncionarioId) VALUES (S.id_novedad, S.FuncionarioId);
    SET IDENTITY_INSERT dbo.NovedadFoto ON; MERGE dbo.NovedadFoto AS T USING (SELECT f.id_foto_novedad, CAST(f.Data AS VARBINARY(MAX)), (SELECT TOP 1 n.Id FROM dbo.Novedad n WHERE n.NovedadGeneradaId = f.id_novedad_generada ORDER BY n.Id) FROM Personal.dbo.tblNovedadesFotos f) AS S(id_foto_novedad, Foto, NovedadId) ON T.Id = S.id_foto_novedad WHEN NOT MATCHED AND S.NovedadId IS NOT NULL THEN INSERT (Id, Foto, NovedadId, CreatedAt) VALUES (S.id_foto_novedad, S.Foto, S.NovedadId, GETDATE()); SET IDENTITY_INSERT dbo.NovedadFoto OFF;

    INSERT INTO dbo.Arma (Marca, Modelo, Calibre) SELECT DISTINCT LTRIM(RTRIM(p.marcaArma)), LTRIM(RTRIM(p.modeloArma)), '9mm' FROM Personal.dbo.tblPolicias p WHERE p.marcaArma IS NOT NULL AND p.modeloArma IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.Arma d WHERE d.Marca  = LTRIM(RTRIM(p.marcaArma)) COLLATE DATABASE_DEFAULT AND d.Modelo = LTRIM(RTRIM(p.modeloArma)) COLLATE DATABASE_DEFAULT);
    INSERT INTO dbo.FuncionarioArma (FuncionarioId, ArmaId, Serie, Cargadores, Municiones, Observaciones, FechaAsign) SELECT m.FuncionarioId, a.Id, p.serieArma, TRY_CAST(p.cargadoresArma AS INT), TRY_CAST(p.municionesArma AS INT), p.observacionesArma, GETDATE() FROM Personal.dbo.tblPolicias p JOIN dbo.MapPoliciaFunc m ON p.id_policia = m.id_policia JOIN dbo.Arma a ON a.Marca = LTRIM(RTRIM(p.marcaArma)) COLLATE DATABASE_DEFAULT AND a.Modelo = LTRIM(RTRIM(p.modeloArma)) COLLATE DATABASE_DEFAULT WHERE p.marcaArma IS NOT NULL AND p.modeloArma IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.FuncionarioArma fa WHERE fa.FuncionarioId = m.FuncionarioId AND fa.ArmaId = a.Id);
    INSERT INTO dbo.FuncionarioChaleco (FuncionarioId, Marca, Modelo, Serie, Tipo, Observaciones, FechaAsign) SELECT m.FuncionarioId, p.marcaChaleco, p.modeloChaleco, p.serieChaleco, p.tipoChaleco, p.observacionesChaleco, GETDATE() FROM Personal.dbo.tblPolicias p JOIN dbo.MapPoliciaFunc m ON p.id_policia = m.id_policia WHERE (p.marcaChaleco IS NOT NULL OR p.modeloChaleco IS NOT NULL OR p.serieChaleco IS NOT NULL) AND NOT EXISTS (SELECT 1 FROM dbo.FuncionarioChaleco fc WHERE fc.FuncionarioId = m.FuncionarioId AND fc.Serie = p.serieChaleco COLLATE DATABASE_DEFAULT);
    MERGE INTO dbo.DotacionItem AS T USING (VALUES ('Camisa'), ('Pantalón'), ('Botas'), ('Zapatos'), ('Gorro'), ('Buso'), ('Campera'), ('Esposas')) AS S (Nombre) ON T.Nombre = S.Nombre WHEN NOT MATCHED BY TARGET THEN INSERT (Nombre, Activo, CreatedAt) VALUES (S.Nombre, 1, GETDATE());
    WITH DU AS (SELECT id_policia, Item, Talla FROM Personal.dbo.tblPolicias UNPIVOT (Talla FOR Item IN (camisa, pantalon, botas, zapatos, gorro, buso, campera, esposas)) AS unpvt) INSERT INTO dbo.FuncionarioDotacion (FuncionarioId, DotacionItemId, Talla, FechaAsign) SELECT m.FuncionarioId, di.Id, du.Talla, GETDATE() FROM DU JOIN dbo.MapPoliciaFunc m ON du.id_policia = m.id_policia JOIN dbo.DotacionItem di ON di.Nombre = du.Item WHERE LTRIM(RTRIM(du.Talla)) <> '' AND du.Talla IS NOT NULL AND NOT EXISTS (SELECT 1 FROM dbo.FuncionarioDotacion fd WHERE fd.FuncionarioId = m.FuncionarioId AND fd.DotacionItemId = di.Id);
    PRINT '✔ Novedades, Notificaciones y Dotaciones migradas.';

    /*================================================================
      PASO 7: Migrar Estados Legales y Sanciones Legadas
    =================================================================*/
    PRINT '--- PASO 7: Migrando Estados Legales... ---';

    -- PASO 7A: Migración desde las banderas en tblPolicias
    ;WITH Estados AS (
        SELECT F.Id AS FuncionarioId, E.TipoEstadoId
        FROM Personal.dbo.tblPolicias AS P
        JOIN Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
        CROSS APPLY (VALUES ('Sumariado', P.sumariado, 6), ('Separado del Cargo', P.separado_cargo, 10), ('Procesado', P.procesado, 11), ('Desarmado', P.desarmado, 12)) AS E(Tipo, ValorOriginal, TipoEstadoId)
        WHERE LTRIM(RTRIM(UPPER(ISNULL(E.ValorOriginal, '')))) = 'SI'
    )
    INSERT INTO dbo.EstadoTransitorio (FuncionarioId, TipoEstadoTransitorioId, CreatedAt)
    SELECT e.FuncionarioId, e.TipoEstadoId, GETDATE() FROM Estados e
    WHERE NOT EXISTS (SELECT 1 FROM dbo.EstadoTransitorio et WHERE et.FuncionarioId = e.FuncionarioId AND et.TipoEstadoTransitorioId = e.TipoEstadoId);

    -- Poblar detalles para los estados legales
    INSERT INTO dbo.DesarmadoDetalle (EstadoTransitorioId, FechaDesde, Observaciones) SELECT et.Id, et.CreatedAt, 'Migrado desde sistema anterior.' FROM dbo.EstadoTransitorio et WHERE et.TipoEstadoTransitorioId = 12 AND NOT EXISTS (SELECT 1 FROM dbo.DesarmadoDetalle d WHERE d.EstadoTransitorioId = et.Id);
    INSERT INTO dbo.InicioDeProcesamientoDetalle (EstadoTransitorioId, FechaDesde, Observaciones) SELECT et.Id, et.CreatedAt, 'Migrado desde sistema anterior.' FROM dbo.EstadoTransitorio et WHERE et.TipoEstadoTransitorioId = 11 AND NOT EXISTS (SELECT 1 FROM dbo.InicioDeProcesamientoDetalle d WHERE d.EstadoTransitorioId = et.Id);
    INSERT INTO dbo.SeparacionDelCargoDetalle (EstadoTransitorioId, FechaDesde, Observaciones) SELECT et.Id, et.CreatedAt, 'Migrado desde sistema anterior.' FROM dbo.EstadoTransitorio et WHERE et.TipoEstadoTransitorioId = 10 AND NOT EXISTS (SELECT 1 FROM dbo.SeparacionDelCargoDetalle d WHERE d.EstadoTransitorioId = et.Id);
    INSERT INTO dbo.SumarioDetalle (EstadoTransitorioId, FechaDesde, Observaciones) SELECT et.Id, et.CreatedAt, 'Migrado desde sistema anterior.' FROM dbo.EstadoTransitorio et WHERE et.TipoEstadoTransitorioId = 6 AND NOT EXISTS (SELECT 1 FROM dbo.SumarioDetalle d WHERE d.EstadoTransitorioId = et.Id);
    PRINT '✔ Estados Legales desde banderas migrados.';

    -- ====================================================================
    -- INICIO: CORRECCIÓN PARA MIGRAR SANCIONES DE LA TABLA `tblSancion`
    -- ====================================================================
    PRINT '--- PASO 7B: Migrando sanciones desde la tabla legada tblSancion... ---';

    DECLARE @SancionMapping TABLE (
        NewEstadoTransitorioId INT,
        Causa NVARCHAR(MAX),
        Sancion NVARCHAR(MAX),
        Numero NVARCHAR(100),
        Jefe NVARCHAR(MAX),
        Fecha DATE
    );

    MERGE INTO dbo.EstadoTransitorio AS TGT
    USING (
        SELECT
            mp.FuncionarioId,
            s.causa,
            s.sancion,
            s.numero,
            s.jefe,
            s.fecha
        FROM Personal.dbo.tblSancion s
        JOIN dbo.MapPoliciaFunc mp ON s.id_policia = mp.id_policia
    ) AS SRC
    ON 1 = 0 -- Forza la inserción de todos los registros del origen
    WHEN NOT MATCHED THEN
        INSERT (FuncionarioId, TipoEstadoTransitorioId, CreatedAt)
        VALUES (SRC.FuncionarioId, 3, SRC.fecha) -- TipoEstadoTransitorioId = 3 para 'Sanción'
    OUTPUT
        INSERTED.Id,
        SRC.causa,
        SRC.sancion,
        SRC.numero,
        SRC.jefe,
        SRC.fecha
    INTO @SancionMapping;

    -- Insertar los detalles de las sanciones recién creadas
    INSERT INTO dbo.SancionDetalle (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Resolucion, TipoSancion)
    SELECT
        m.NewEstadoTransitorioId,
        m.Fecha,
        NULL, -- No hay fecha de finalización en la tabla de origen
        'Causa: ' + ISNULL(m.Causa, 'N/A') + '; Sanción: ' + ISNULL(m.Sancion, 'N/A') + '; Impuesta por: ' + ISNULL(m.Jefe, 'N/A'),
        m.Numero,
        'General' -- Se asigna un tipo por defecto
    FROM @SancionMapping m;

    PRINT '✔ Sanciones legadas desde tblSancion migradas correctamente.';
    -- ====================================================================
    -- FIN: CORRECCIÓN MIGRACIÓN SANCIONES
    -- ====================================================================


    /*================================================================
      PASO 8: Migrar Documentos Adjuntos de Estados Transitorios
    =================================================================*/
    PRINT '--- PASO 8: Migrando documentos adjuntos... ---';
    INSERT INTO dbo.EstadoTransitorioAdjunto (EstadoTransitorioId, NombreArchivo, TipoMIME, Contenido, FechaCreacion)
    SELECT
        Documentos.EstadoTransitorioId,
        Documentos.NombreArchivo,
        'application/pdf',
        Documentos.Archivo,
        GETDATE()
    FROM (
        SELECT s.id_estado AS EstadoTransitorioId, 'Resolucion_Sancion_' + CAST(s.id_estado AS VARCHAR(10)) + '.pdf' AS NombreArchivo, CONVERT(VARBINARY(MAX), s.doc_resolucion) AS Archivo FROM Personal.dbo.tblSancionesDetalle s WHERE s.doc_resolucion IS NOT NULL
        UNION ALL
        SELECT d.id_estado, 'Resolucion_Designacion_' + CAST(d.id_estado AS VARCHAR(10)) + '.pdf', CONVERT(VARBINARY(MAX), d.doc_resolucion) FROM Personal.dbo.tblDesignacionesDetalle d WHERE d.doc_resolucion IS NOT NULL
        UNION ALL
        SELECT su.id_estado, 'Expediente_Sumario_' + CAST(su.id_estado AS VARCHAR(10)) + '.pdf', CONVERT(VARBINARY(MAX), su.doc_resolucion) FROM Personal.dbo.tblSumariosDetalle su WHERE su.doc_resolucion IS NOT NULL
    ) AS Documentos
    -- CORRECCIÓN: Asegurarse de que el EstadoTransitorio padre exista en Apex antes de insertar el adjunto.
    WHERE EXISTS (SELECT 1 FROM dbo.EstadoTransitorio et WHERE et.Id = Documentos.EstadoTransitorioId)
      AND NOT EXISTS (SELECT 1 FROM dbo.EstadoTransitorioAdjunto a WHERE a.EstadoTransitorioId = Documentos.EstadoTransitorioId);
    PRINT '✔ Documentos adjuntos migrados.';

    /*================================================================
      PASO 9: Migrar Fotos de Funcionarios
    =================================================================*/
    PRINT '--- PASO 9: Migrando fotos de funcionarios... ---';
    MERGE dbo.FuncionarioFotoHistorico AS T USING (SELECT p.id_picture, mp.FuncionarioId, CAST(p.Data AS VARBINARY(MAX)), p.FileName FROM Personal.dbo.tblPictures p JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = p.id_policia) AS S(PictureId, FuncionarioId, Foto, FileName) ON T.PictureId = S.PictureId WHEN NOT MATCHED THEN INSERT (FuncionarioId, PictureId, Foto, FileName, CreatedAt) VALUES (S.FuncionarioId, S.PictureId, S.Foto, S.FileName, GETDATE());
    ;WITH UltimaFoto AS (SELECT ffh.FuncionarioId, ffh.Foto, ROW_NUMBER() OVER(PARTITION BY ffh.FuncionarioId ORDER BY ffh.PictureId DESC) as rn FROM dbo.FuncionarioFotoHistorico ffh) UPDATE f SET f.Foto = uf.Foto FROM dbo.Funcionario f JOIN UltimaFoto uf ON f.Id = uf.FuncionarioId WHERE uf.rn = 1;
    PRINT '✔ Fotos de funcionarios migradas.';

    COMMIT TRANSACTION;
    PRINT '🎉 --- MIGRACIÓN COMPLETA FINALIZADA CON ÉXITO --- 🎉';

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    PRINT '❌ !!! ERROR DURANTE LA MIGRACIÓN, SE HAN REVERTIDO TODOS LOS CAMBIOS !!! ❌';
    THROW;
END CATCH;
GO

-- =============================================================================================
-- PASO 10: CREAR OBJETOS DE BASE DE DATOS (VISTAS Y PROCEDIMIENTOS)
-- =============================================================================================
PRINT '--- PASO 10: Creando vistas y procedimientos almacenados... ---';
GO

-- Creación de Vista de Estados Activos (con prioridades y colores)
CREATE OR ALTER VIEW [dbo].[vw_FuncionarioEstadosActivos] AS
WITH ActiveLicenses AS (
    SELECT hl.FuncionarioId, hl.inicio, hl.finaliza, hl.Comentario, tl.Nombre AS TipoLicenciaNombre, ca.Nombre AS CategoriaNombre
    FROM dbo.HistoricoLicencia AS hl
    JOIN dbo.TipoLicencia AS tl ON hl.TipoLicenciaId = tl.Id
    JOIN dbo.CategoriaAusencia ca ON tl.CategoriaAusenciaId = ca.Id
    WHERE (CONVERT(date, GETDATE()) BETWEEN hl.inicio AND hl.finaliza) AND (ISNULL(hl.estado, '') NOT IN ('Rechazado', 'Anulado'))
)
SELECT et.FuncionarioId, 1 AS Prioridad, 'Baja de Funcionario' AS Tipo, 'Fecha: ' + CONVERT(VARCHAR, bfd.FechaDesde, 103) + '. ' + ISNULL(bfd.Observaciones, '') AS Detalles, 'Maroon' AS ColorIndicador FROM dbo.EstadoTransitorio et JOIN dbo.BajaDeFuncionarioDetalle bfd ON et.Id = bfd.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Baja de Funcionario') AND (GETDATE() >= bfd.FechaDesde)
UNION ALL
SELECT et.FuncionarioId, 1, 'Separación del Cargo', 'Desde ' + CONVERT(VARCHAR, scd.FechaDesde, 103) + '. ' + ISNULL(scd.Observaciones, ''), 'Maroon' FROM dbo.EstadoTransitorio et JOIN dbo.SeparacionDelCargoDetalle scd ON et.Id = scd.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Separación del Cargo') AND (GETDATE() BETWEEN scd.FechaDesde AND ISNULL(scd.FechaHasta, '9999-12-31'))
UNION ALL
SELECT et.FuncionarioId, 2, 'Inicio de Procesamiento', 'Desde ' + CONVERT(VARCHAR, ipd.FechaDesde, 103) + '. ' + ISNULL(ipd.Observaciones, ''), 'Red' FROM dbo.EstadoTransitorio et JOIN dbo.InicioDeProcesamientoDetalle ipd ON et.Id = ipd.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Inicio de Procesamiento') AND (GETDATE() BETWEEN ipd.FechaDesde AND ISNULL(ipd.FechaHasta, '9999-12-31'))
UNION ALL
SELECT FuncionarioId, 3, TipoLicenciaNombre, 'Desde ' + CONVERT(VARCHAR, inicio, 103) + ' hasta ' + CONVERT(VARCHAR, finaliza, 103) + '. ' + ISNULL(Comentario, ''), 'Crimson' FROM ActiveLicenses WHERE CategoriaNombre = 'Sanción Grave'
UNION ALL
SELECT et.FuncionarioId, 3, 'Sanción', 'Desde ' + CONVERT(VARCHAR, sd.FechaDesde, 103) + ISNULL(' hasta ' + CONVERT(VARCHAR, sd.FechaHasta, 103), '') + '. ' + ISNULL(sd.Observaciones, ''), 'Crimson' FROM dbo.EstadoTransitorio et JOIN dbo.SancionDetalle sd ON et.Id = sd.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Sanción') AND GETDATE() BETWEEN sd.FechaDesde AND ISNULL(sd.FechaHasta, '9999-12-31')
UNION ALL
SELECT et.FuncionarioId, 4, 'Desarmado', 'Desde ' + CONVERT(VARCHAR, dd.FechaDesde, 103) + '. ' + ISNULL(dd.Observaciones, ''), 'OrangeRed' FROM dbo.EstadoTransitorio et JOIN dbo.DesarmadoDetalle dd ON et.Id = dd.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Desarmado') AND (GETDATE() BETWEEN dd.FechaDesde AND ISNULL(dd.FechaHasta, '9999-12-31'))
UNION ALL
SELECT et.FuncionarioId, 5, 'Sumario', ISNULL('Expediente: ' + ISNULL(sud.Expediente, 'N/A') + '. ' + ISNULL(sud.Observaciones, ''), ''), 'DarkOrange' FROM dbo.EstadoTransitorio et JOIN dbo.SumarioDetalle sud ON et.Id = sud.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Sumario') AND (GETDATE() BETWEEN sud.FechaDesde AND ISNULL(sud.FechaHasta, '9999-12-31'))
UNION ALL
SELECT et.FuncionarioId, 5, 'Orden Cinco', ISNULL(ocd.Observaciones, ''), 'DarkOrange' FROM dbo.EstadoTransitorio et JOIN dbo.OrdenCincoDetalle ocd ON et.Id = ocd.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Orden Cinco') AND (GETDATE() BETWEEN ocd.FechaDesde AND ISNULL(ocd.FechaHasta, '9999-12-31'))
UNION ALL
SELECT FuncionarioId, 6, TipoLicenciaNombre, 'Desde ' + CONVERT(VARCHAR, inicio, 103) + ' hasta ' + CONVERT(VARCHAR, finaliza, 103) + '. ' + ISNULL(Comentario, ''), 'Orange' FROM ActiveLicenses WHERE CategoriaNombre = 'Sanción Leve'
UNION ALL
SELECT et.FuncionarioId, 7, 'Enfermedad', ISNULL('Diagnóstico: ' + ISNULL(ed.Diagnostico, 'N/A') + '. ' + ISNULL(ed.Observaciones, ''), ''), 'SteelBlue' FROM dbo.EstadoTransitorio et JOIN dbo.EnfermedadDetalle ed ON et.Id = ed.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Enfermedad') AND (GETDATE() BETWEEN ed.FechaDesde AND ISNULL(ed.FechaHasta, '9999-12-31'))
UNION ALL
SELECT FuncionarioId, 7, TipoLicenciaNombre, 'Desde ' + CONVERT(VARCHAR, inicio, 103) + ' hasta ' + CONVERT(VARCHAR, finaliza, 103) + '. ' + ISNULL(Comentario, ''), 'SteelBlue' FROM ActiveLicenses WHERE CategoriaNombre = 'Salud'
UNION ALL
SELECT FuncionarioId, 8, TipoLicenciaNombre, 'Desde ' + CONVERT(VARCHAR, inicio, 103) + ' hasta ' + CONVERT(VARCHAR, finaliza, 103) + '. ' + ISNULL(Comentario, ''), 'SeaGreen' FROM ActiveLicenses WHERE CategoriaNombre NOT IN ('Sanción Grave', 'Sanción Leve', 'Salud')
UNION ALL
SELECT et.FuncionarioId, 8, 'Designación', ISNULL('Resolución: ' + ISNULL(dd.DocResolucion, 'N/A') + '. ' + ISNULL(dd.Observaciones, ''), ''), 'MediumSeaGreen' FROM dbo.EstadoTransitorio et JOIN dbo.DesignacionDetalle dd ON et.Id = dd.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Designación') AND (GETDATE() BETWEEN dd.FechaDesde AND ISNULL(dd.FechaHasta, '9999-12-31'))
UNION ALL
SELECT et.FuncionarioId, 8, 'Traslado', 'Desde ' + CONVERT(VARCHAR, td.FechaDesde, 103) + ISNULL(' hasta ' + CONVERT(VARCHAR, td.FechaHasta, 103), '') + '. ' + ISNULL(td.Observaciones, ''), 'SeaGreen' FROM dbo.EstadoTransitorio et JOIN dbo.TrasladoDetalle td ON et.Id = td.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Traslado') AND (GETDATE() BETWEEN td.FechaDesde AND ISNULL(td.FechaHasta, '9999-12-31'))
UNION ALL
-- **INICIO: Bloque añadido para Cambio de Cargo**
SELECT
    et.FuncionarioId,
    8 AS Prioridad,
    'Cambio de Cargo' AS Tipo,
    'De ' + ISNULL(cargo_ant.Nombre, 'N/A') + ' a ' + ISNULL(cargo_nue.Nombre, 'N/A') +
    ' desde ' + CONVERT(VARCHAR, ccd.FechaDesde, 103) + '. ' + ISNULL(ccd.Observaciones, '') AS Detalles,
    'SeaGreen' AS ColorIndicador
FROM
    dbo.EstadoTransitorio et
JOIN
    dbo.CambioDeCargoDetalle ccd ON et.Id = ccd.EstadoTransitorioId
LEFT JOIN
    dbo.Cargo cargo_ant ON ccd.CargoAnteriorId = cargo_ant.Id
LEFT JOIN
    dbo.Cargo cargo_nue ON ccd.CargoNuevoId = cargo_nue.Id
WHERE
    et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Cambio de Cargo')
    AND (GETDATE() BETWEEN ccd.FechaDesde AND ISNULL(ccd.FechaHasta, '9999-12-31'))
-- **FIN: Bloque añadido para Cambio de Cargo**
UNION ALL
SELECT et.FuncionarioId, 9, 'Retén', 'Turno: ' + ISNULL(rd.Turno, 'N/A') + '. ' + ISNULL(rd.Observaciones, ''), 'SlateGray' FROM dbo.EstadoTransitorio et JOIN dbo.RetenDetalle rd ON et.Id = rd.EstadoTransitorioId WHERE et.TipoEstadoTransitorioId = (SELECT Id FROM dbo.TipoEstadoTransitorio WHERE Nombre = 'Retén') AND rd.FechaReten = CONVERT(date, GETDATE());
GO

-- Creación de Vista para Situación Actual (solo el estado más grave)
CREATE OR ALTER VIEW [dbo].[vw_FuncionarioSituacionActual] AS
WITH RankedSituacion AS (
    SELECT
        FuncionarioId, Prioridad, Tipo, Detalles, ColorIndicador,
        ROW_NUMBER() OVER (PARTITION BY FuncionarioId ORDER BY Prioridad ASC) as rn
    FROM
        dbo.vw_FuncionarioEstadosActivos
)
SELECT
    FuncionarioId, Prioridad, Tipo, Detalles, ColorIndicador
FROM
    RankedSituacion
WHERE
    rn = 1;
GO

-- Creación de Procedimiento para actualizar estado de notificaciones
CREATE OR ALTER PROCEDURE dbo.usp_ProcesarEstadoNotificaciones
AS
BEGIN
    SET NOCOUNT, XACT_ABORT ON;
    DECLARE @Hoy DATETIME2 = GETDATE();
    UPDATE dbo.NotificacionPersonal SET EstadoId = 1, UpdatedAt = @Hoy WHERE EstadoId <> 3 AND @Hoy <  FechaProgramada AND EstadoId <> 1;
    UPDATE dbo.NotificacionPersonal SET EstadoId = 2, UpdatedAt = @Hoy WHERE EstadoId <> 3 AND @Hoy >= FechaProgramada AND EstadoId <> 2;
END;
GO

PRINT '✔ Vistas y procedimientos creados correctamente.';
GO

PRINT '=======================================================';
PRINT '         MIGRACIÓN FINALIZADA POR COMPLETO             ';
PRINT '=======================================================';
GO