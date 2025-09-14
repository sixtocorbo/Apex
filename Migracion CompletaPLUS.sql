/*
===============================================================================
  SCRIPT DE MIGRACIÓN COMPLETO: Personal -> Apex (CORREGIDO v15 - UNIFICADO)
  Autor:        Equipo de migración (revisado y unificado por Gemini)
  Fecha:        06/09/2025
  Descripción: Migra TODOS los datos desde 'Personal' a 'Apex' en un solo
                script idempotente. Esta versión integra todas las tareas
                post-migración y corrige los conflictos de intercalación (collation).
                1. Migración de niveles de estudio desde datos de origen.
                2. Creación de categorías de ausencia adicionales.
                3. Clasificación de todos los tipos de licencia.
                4. Migración completa de dotaciones (Armas, Chalecos, etc.).
                5. Migración de estados legales del funcionario.
                6. Clasificación completa de tipos de funcionario por escalafón.
===============================================================================
*/

USE [Apex];
GO

-- Se redefine y corrige el procedimiento de limpieza para evitar errores de FK.
IF OBJECT_ID('usp_LimpiarDatosDeApex', 'P') IS NOT NULL
    DROP PROCEDURE usp_LimpiarDatosDeApex;
GO

CREATE PROCEDURE usp_LimpiarDatosDeApex
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        PRINT '--- INICIANDO LIMPIEZA DE DATOS EN APEX (ORDEN CORREGIDO) ---';
        
        -- Se eliminan en el orden correcto para evitar conflictos de FK
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
        DELETE FROM [dbo].[Usuario];
        
        -- Se elimina Funcionario ANTES que las tablas a las que hace referencia
        DELETE FROM [dbo].[Funcionario];
        
        -- Ahora sí se pueden eliminar las tablas catálogo
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
        DELETE FROM [dbo].[MapPoliciaFunc];
        DELETE FROM [dbo].[RegimenDetalle];
        DELETE FROM [dbo].[RegimenTrabajo];
        DELETE FROM [dbo].[RegimenAlternancia];

        PRINT '--- LIMPIEZA COMPLETADA CON ÉXITO ---';
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        PRINT '!!! ERROR DURANTE LA LIMPIEZA DE DATOS !!!';
        THROW;
    END CATCH;
END
GO

-- Se ejecuta el procedimiento de limpieza
EXEC usp_LimpiarDatosDeApex;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    PRINT '--- INICIO DE LA MIGRACIÓN COMPLETA Y UNIFICADA ---';

    /*================================================================
      PASO 1: Poblar Catálogos
    =================================================================*/
    PRINT 'Paso 1: Poblando catálogos...';

	-- TipoFuncionario (con todos los escalafones)
	SET IDENTITY_INSERT dbo.TipoFuncionario ON;
	MERGE INTO dbo.TipoFuncionario AS Target
	USING (VALUES
	    (1, 'Policia'), (2, 'Operador Penitenciario'), (3, 'Profesional Universitario'),
	    (4, 'Técnico'), (5, 'Administrativo')
	) AS Source (Id, Nombre)
	ON Target.Id = Source.Id
	WHEN MATCHED AND Target.Nombre <> Source.Nombre THEN
	    UPDATE SET Nombre = Source.Nombre, UpdatedAt = GETDATE()
	WHEN NOT MATCHED BY TARGET THEN
	    INSERT (Id, Nombre, CreatedAt)
	    VALUES (Source.Id, Source.Nombre, GETDATE());
	SET IDENTITY_INSERT dbo.TipoFuncionario OFF;

    -- RolUsuario
    MERGE dbo.RolUsuario AS tgt USING (VALUES (1,'Admin',1), (2,'Usuario',2)) AS src(Id,Nombre,Orden) ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre,Orden) VALUES (src.Id,src.Nombre,src.Orden);
    
    -- CategoriaAusencia (con tipos de sanción)
    SET IDENTITY_INSERT dbo.CategoriaAusencia ON;
    MERGE dbo.CategoriaAusencia AS tgt
    USING (VALUES
      (1,'General'), (2,'Salud'), (3,'Especial'),
      (4, 'Sanción Leve'), (5, 'Sanción Grave')
    ) AS src(Id,Nombre)
    ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre);
    SET IDENTITY_INSERT dbo.CategoriaAusencia OFF;

    -- NotificacionEstado
    MERGE dbo.NotificacionEstado AS TGT USING (VALUES (1, 'Pendiente', 1), (2, 'Vencida', 2), (3, 'Firmada', 3)) AS SRC(Id, Nombre, Orden) ON TGT.Id = SRC.Id
    WHEN NOT MATCHED THEN INSERT (Id, Nombre, Orden) VALUES (SRC.Id, SRC.Nombre, SRC.Orden);

    -- TipoNotificacion
    SET IDENTITY_INSERT dbo.TipoNotificacion ON;
    MERGE dbo.TipoNotificacion AS TGT
    USING (VALUES (1, 'VISTA', 1), (2, 'INTIMACION', 2), (3, 'CITACION', 3), (4, 'NOTIFICACION', 4)) AS SRC(Id, Nombre, Orden)
    ON TGT.Id = SRC.Id
    WHEN NOT MATCHED THEN INSERT (Id, Nombre, Orden) VALUES (SRC.Id, SRC.Nombre, SRC.Orden);
    SET IDENTITY_INSERT dbo.TipoNotificacion OFF;

    -- TipoEstadoTransitorio
    SET IDENTITY_INSERT dbo.TipoEstadoTransitorio ON;
    MERGE dbo.TipoEstadoTransitorio AS tgt
    USING (VALUES
        (1, 'Designación', 0), (2, 'Enfermedad', 0), (3, 'Sanción', 0), (4, 'Orden Cinco', 0), (5, 'Retén', 0),
        (6, 'Sumario', 0), (7, 'Baja de Funcionario', 0), (8, 'Cambio de Cargo', 0), (9, 'Reactivación de Funcionario', 0),
        (10, 'Separación del Cargo', 0), (11, 'Inicio de Procesamiento', 0), (12, 'Desarmado', 0), (21, 'Traslado', 0)
    ) AS src(Id, Nombre, EsJerarquico)
    ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id, Nombre, EsJerarquico, CreatedAt) VALUES (src.Id, src.Nombre, src.EsJerarquico, GETDATE());
    SET IDENTITY_INSERT dbo.TipoEstadoTransitorio OFF;

    -- Cargo (poblado desde Personal.dbo.tblGrados)
    SET IDENTITY_INSERT dbo.Cargo ON;
    MERGE dbo.Cargo AS tgt
    USING (
        SELECT id_grado, nom_grado, grado FROM (
            SELECT id_grado, nom_grado, grado, ROW_NUMBER() OVER(PARTITION BY nom_grado ORDER BY id_grado) as rn
            FROM Personal.dbo.tblGrados
        ) as Sub WHERE rn = 1
    ) AS src(Id, Nombre, Grado)
    ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id, Nombre, Grado, CreatedAt) VALUES (src.Id, src.Nombre, src.Grado, GETDATE());
    SET IDENTITY_INSERT dbo.Cargo OFF;
	
	-- Crear mapeo para resolver duplicados de Grados que causan FK error
    IF OBJECT_ID('tempdb..#MapeoGrado') IS NOT NULL DROP TABLE #MapeoGrado;
    CREATE TABLE #MapeoGrado (IdGradoOrigen INT PRIMARY KEY, IdGradoDestino INT);
    ;WITH GradosCanonicos AS (
        SELECT id_grado, nom_grado FROM (
            SELECT id_grado, nom_grado, ROW_NUMBER() OVER(PARTITION BY nom_grado ORDER BY id_grado) as rn
            FROM Personal.dbo.tblGrados
        ) as Sub WHERE rn = 1
    )
    INSERT INTO #MapeoGrado (IdGradoOrigen, IdGradoDestino)
    SELECT g.id_grado, gc.id_grado
    FROM Personal.dbo.tblGrados g
    JOIN GradosCanonicos gc ON g.nom_grado COLLATE DATABASE_DEFAULT = gc.nom_grado COLLATE DATABASE_DEFAULT;

    -- Estado
    SET IDENTITY_INSERT dbo.Estado ON;
	MERGE dbo.Estado AS tgt
	USING (
		SELECT id_estado, nom_estado, CASE WHEN nom_estado IN ('AU', 'O5', 'CO') THEN 1 ELSE 0 END AS EsAusentismo
		FROM Personal.dbo.tblEstados
	) AS src(Id, Nombre, EsAusentismo)
	ON tgt.Id = src.Id
	WHEN NOT MATCHED THEN INSERT (Id, Nombre, EsAusentismo, CreatedAt) VALUES (src.Id, src.Nombre, src.EsAusentismo, GETDATE());
	SET IDENTITY_INSERT dbo.Estado OFF;

    -- Catálogos simples
    SET IDENTITY_INSERT dbo.Seccion ON;
    MERGE dbo.Seccion AS tgt USING (SELECT id_seccion, nom_seccion FROM Personal.dbo.tblSecciones) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES (src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Seccion OFF;
    
    SET IDENTITY_INSERT dbo.Turno ON;
    MERGE dbo.Turno AS tgt USING (SELECT id_turno, nom_turno FROM Personal.dbo.tblTurnos) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES (src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Turno OFF;
    
    SET IDENTITY_INSERT dbo.Semana ON;
    MERGE dbo.Semana AS tgt USING (SELECT id_semana, nom_semana FROM Personal.dbo.tblSemanas) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES (src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Semana OFF;
    
    SET IDENTITY_INSERT dbo.Horario ON;
    MERGE dbo.Horario AS tgt USING (SELECT id_horario, nom_horario FROM Personal.dbo.tblHorarios) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES (src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Horario OFF;
    
    SET IDENTITY_INSERT dbo.TipoLicencia ON;
    MERGE dbo.TipoLicencia AS tgt USING (
        SELECT id_ausencia, nom_ausencia, CASE WHEN ausente = 'SI' THEN 1 ELSE 0 END, CASE WHEN viatico = 'NO' THEN 1 ELSE 0 END,
               CASE WHEN presentismo = 'NO' THEN 1 ELSE 0 END, CASE WHEN habil = 'SI' THEN 1 ELSE 0 END
        FROM Personal.dbo.tblAusencias
    ) AS src(Id,Nombre,EsAusencia,SuspendeViatico,AfectaPresentismo,EsHabil)
    ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre,EsAusencia,SuspendeViatico,AfectaPresentismo,EsHabil,CreatedAt,CategoriaAusenciaId) VALUES(src.Id,src.Nombre,src.EsAusencia,src.SuspendeViatico,src.AfectaPresentismo,src.EsHabil,GETDATE(),1); -- Default a General
    SET IDENTITY_INSERT dbo.TipoLicencia OFF;
    
    SET IDENTITY_INSERT dbo.Genero ON;
    MERGE dbo.Genero AS tgt USING (VALUES (1,'Masculino'),(2,'Femenino'),(3,'Otro'),(4,'No especifica')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre);
    SET IDENTITY_INSERT dbo.Genero OFF;
    
    SET IDENTITY_INSERT dbo.EstadoCivil ON;
    MERGE dbo.EstadoCivil AS tgt USING (VALUES (1,'Soltero/a'),(2,'Casado/a'),(3,'Divorciado/a'),(4,'Viudo/a'),(5,'Unión libre'),(6,'Separado/a'),(7,'No especifica')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre);
    SET IDENTITY_INSERT dbo.EstadoCivil OFF;
    
	-- NivelEstudio (poblado desde los datos de origen)
	DELETE FROM Apex.dbo.NivelEstudio;
	INSERT INTO Apex.dbo.NivelEstudio (Nombre)
	SELECT DISTINCT LTRIM(RTRIM(p.nivelestudio))
	FROM Personal.dbo.tblPolicias AS p
	WHERE p.nivelestudio IS NOT NULL AND LTRIM(RTRIM(p.nivelestudio)) <> '';

    MERGE dbo.Escalafon AS tgt USING (SELECT DISTINCT LTRIM(RTRIM(p.escalafon)) AS Nombre FROM Personal.dbo.tblPolicias p WHERE p.escalafon IS NOT NULL AND LTRIM(RTRIM(p.escalafon)) <> '') AS src ON tgt.Nombre = src.Nombre COLLATE Modern_Spanish_CI_AS WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (src.Nombre);
    MERGE dbo.Funcion AS tgt USING (SELECT DISTINCT LTRIM(RTRIM(p.funcion)) AS Nombre FROM Personal.dbo.tblPolicias p WHERE p.funcion IS NOT NULL AND LTRIM(RTRIM(p.funcion)) <> '') AS src ON tgt.Nombre = src.Nombre COLLATE Modern_Spanish_CI_AS WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (src.Nombre);

    PRINT 'Paso 1.1: Migrando catálogos jerárquicos (Area, Puesto)...';
    SET IDENTITY_INSERT dbo.AreaTrabajo ON;
    MERGE dbo.AreaTrabajo AS TGT USING Personal.dbo.tblAreaPrincipal AS SRC ON (TGT.Id = SRC.id_area)
    WHEN NOT MATCHED BY TARGET THEN INSERT (Id, Nombre, CreatedAt) VALUES (SRC.id_area, SRC.area, GETDATE());
    SET IDENTITY_INSERT dbo.AreaTrabajo OFF;

    MERGE dbo.TipoViatico AS TGT USING (VALUES ('Viático General', 15),('Viático Especial', 20)) AS SRC(Descripcion, Dias) ON TGT.Dias = SRC.Dias
    WHEN NOT MATCHED BY TARGET THEN INSERT (Descripcion, Dias, CreatedAt) VALUES (SRC.Descripcion, SRC.Dias, GETDATE());
    
    SET IDENTITY_INSERT dbo.PuestoTrabajo ON;
    MERGE dbo.PuestoTrabajo AS TGT
    USING (
        SELECT p.id_puesto, p.nom_puesto,
               CASE p.id_viatico WHEN 1 THEN (SELECT TOP 1 Id FROM dbo.TipoViatico WHERE Dias = 15) WHEN 2 THEN (SELECT TOP 1 Id FROM dbo.TipoViatico WHERE Dias = 20) ELSE NULL END AS TipoViaticoId,
               p.id_area AS AreaTrabajoId
        FROM Personal.dbo.tblPuestos p
    ) AS SRC
    ON (TGT.Id = SRC.id_puesto)
    WHEN NOT MATCHED BY TARGET THEN INSERT (Id, Nombre, TipoViaticoId, AreaTrabajoId, Activo, CreatedAt) VALUES (SRC.id_puesto, SRC.nom_puesto, SRC.TipoViaticoId, SRC.AreaTrabajoId, 1, GETDATE());
    SET IDENTITY_INSERT dbo.PuestoTrabajo OFF;
    
    PRINT 'Paso 1: Catálogos completados.';

    /*================================================================
      PASO 2: Migrar Funcionarios y mapeos principales
    =================================================================*/
    PRINT 'Paso 2: Migrando funcionarios...';
    
    ;WITH PoliciasStg AS (
        SELECT p.id_policia, RIGHT('00000000' + CAST(p.num_cedula AS VARCHAR(8)),8) AS CI,
               LEFT(LTRIM(RTRIM(p.nom_policia)), 60) AS Nombre, p.alta AS FechaIngreso,
               CASE WHEN p.id_estado = 1 THEN 1 ELSE 0 END AS Activo, p.id_seccion, p.id_puesto, p.id_turno,
               p.id_semana, p.id_horario,
               CASE LTRIM(RTRIM(UPPER(p.genero))) WHEN 'M' THEN 1 WHEN 'F' THEN 2 ELSE 4 END AS GeneroId,
               p.nivelestudio, p.estadocivil, p.id_grado, p.fecha_registro, p.fecha_actualizado,
               p.fecha_nacimiento, LEFT(LTRIM(RTRIM(p.domicilio)), 250) AS Domicilio,
               LEFT(LTRIM(RTRIM(p.correo)), 255) AS Email, LEFT(LTRIM(RTRIM(p.telefono)), 50)  AS Telefono,
               LTRIM(RTRIM(p.escalafon)) AS EscalafonTxt, LTRIM(RTRIM(p.funcion)) AS FuncionTxt, p.departamento AS Ciudad,
               p.seccional, p.credencial, CASE WHEN UPPER(LTRIM(RTRIM(ISNULL(p.estudia, 'NO')))) = 'SI' THEN 1 ELSE 0 END AS Estudia
        FROM Personal.dbo.tblPolicias p
    ),
    PoliciasMapeadas AS (
        SELECT s.CI, s.Nombre, s.FechaIngreso, s.Activo, s.id_seccion AS SeccionId, s.id_puesto AS PuestoTrabajoId,
               s.id_turno AS TurnoId, s.id_semana AS SemanaId, s.id_horario AS HorarioId, s.GeneroId,
               ne.Id AS NivelEstudioId,
               ec.Id AS EstadoCivilId,
                mg.IdGradoDestino AS CargoId, s.fecha_registro AS CreatedAt, s.fecha_actualizado AS UpdatedAt,
                s.fecha_nacimiento AS FechaNacimiento, s.Domicilio, s.Email, s.Telefono, e.Id AS EscalafonId,
                f.Id AS FuncionId, s.Ciudad, s.seccional AS Seccional, s.credencial AS Credencial, s.Estudia
        FROM PoliciasStg s
            LEFT JOIN #MapeoGrado mg ON s.id_grado = mg.IdGradoOrigen
        LEFT JOIN dbo.NivelEstudio ne ON ne.Nombre = s.nivelestudio COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.EstadoCivil ec ON ec.Nombre = s.estadocivil COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.Escalafon e ON e.Nombre = s.EscalafonTxt COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.Funcion f ON f.Nombre = s.FuncionTxt COLLATE Modern_Spanish_CI_AS
    )
    MERGE dbo.Funcionario AS tgt
    USING PoliciasMapeadas AS src ON tgt.CI = src.CI COLLATE DATABASE_DEFAULT
    WHEN NOT MATCHED BY TARGET THEN
      INSERT (CI,Nombre,FechaIngreso,Activo,CreatedAt,UpdatedAt,SeccionId,PuestoTrabajoId,TurnoId,SemanaId,HorarioId,GeneroId,NivelEstudioId,EstadoCivilId,CargoId,FechaNacimiento,Domicilio,Email,Telefono,EscalafonId,FuncionId,TipoFuncionarioId,Ciudad,Seccional,Credencial,Estudia)
      VALUES (src.CI,src.Nombre,src.FechaIngreso,src.Activo,src.CreatedAt,src.UpdatedAt,src.SeccionId,src.PuestoTrabajoId,src.TurnoId,src.SemanaId,src.HorarioId,src.GeneroId,src.NivelEstudioId,src.EstadoCivilId,src.CargoId,src.FechaNacimiento,src.Domicilio,src.Email,src.Telefono,src.EscalafonId,src.FuncionId,1,src.Ciudad,src.Seccional,src.Credencial,src.Estudia);

    TRUNCATE TABLE dbo.MapPoliciaFunc;
    INSERT INTO dbo.MapPoliciaFunc (id_policia,FuncionarioId) SELECT p.id_policia,f.Id FROM Personal.dbo.tblPolicias p JOIN dbo.Funcionario f ON f.CI = RIGHT('00000000' + CAST(p.num_cedula AS VARCHAR(8)),8);

    PRINT 'Paso 2: Funcionarios migrados.';

    /*================================================================
      PASO 3: Migrar Datos Relacionados (Estados, Licencias, Históricos)
    =================================================================*/
    PRINT 'Paso 3: Migrando datos relacionados...';
    
    SET IDENTITY_INSERT dbo.EstadoTransitorio ON;
    MERGE dbo.EstadoTransitorio as TGT
    USING (
        SELECT eu.id_estado, mp.FuncionarioId,
               CASE eu.categoria WHEN 'D' THEN 1 WHEN 'E' THEN 2 WHEN 'S' THEN 3 WHEN 'O' THEN 4 WHEN 'R' THEN 5 WHEN 'U' THEN 6 END as TipoEstadoTransitorioId
        FROM Personal.dbo.tblEstadosUnificados eu
        JOIN dbo.MapPoliciaFunc mp ON eu.id_policia = mp.id_policia
		WHERE eu.categoria IN ('D', 'E', 'S', 'O', 'R', 'U')
    ) AS SRC
    ON TGT.Id = SRC.id_estado
    WHEN NOT MATCHED THEN INSERT (Id, FuncionarioId, TipoEstadoTransitorioId, CreatedAt) VALUES (SRC.id_estado, SRC.FuncionarioId, SRC.TipoEstadoTransitorioId, GETDATE());
    SET IDENTITY_INSERT dbo.EstadoTransitorio OFF;

	-- Migrar Detalles de Estados Transitorios, asegurando que el padre exista
    MERGE dbo.DesignacionDetalle AS TGT USING (SELECT d.id_estado, d.fecha_inicio, d.fecha_fin, d.descripcion, d.doc_resolucion FROM Personal.dbo.tblDesignacionesDetalle d JOIN dbo.EstadoTransitorio et ON d.id_estado = et.Id) AS SRC ON TGT.EstadoTransitorioId = SRC.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, DocResolucion) VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion, SRC.doc_resolucion);
    MERGE dbo.EnfermedadDetalle AS TGT USING (SELECT e.id_estado, e.fecha_inicio, e.fecha_fin, e.descripcion, e.descripcion as Diagnostico FROM Personal.dbo.tblEnfermedadDetalle e JOIN dbo.EstadoTransitorio et ON e.id_estado = et.Id) AS SRC ON TGT.EstadoTransitorioId = SRC.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Diagnostico) VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion, SRC.Diagnostico);
    MERGE dbo.SancionDetalle AS TGT USING (SELECT s.id_estado, s.fecha_inicio, s.fecha_fin, s.descripcion, s.doc_resolucion FROM Personal.dbo.tblSancionesDetalle s JOIN dbo.EstadoTransitorio et ON s.id_estado = et.Id) AS SRC ON TGT.EstadoTransitorioId = SRC.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Resolucion) VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion, SRC.doc_resolucion);
    MERGE dbo.OrdenCincoDetalle AS TGT USING (SELECT o.id_estado, o.fecha_inicio, o.fecha_fin, o.descripcion FROM Personal.dbo.tblOrdenCincoDetalle o JOIN dbo.EstadoTransitorio et ON o.id_estado = et.Id) AS SRC ON TGT.EstadoTransitorioId = SRC.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones) VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion);
    MERGE dbo.RetenDetalle AS TGT USING (SELECT r.id_estado, r.fecha_reten, r.descripcion, t.nom_turno FROM Personal.dbo.tblRetenesDetalle r JOIN Personal.dbo.tblTurnos t ON r.id_turno = t.id_turno JOIN dbo.EstadoTransitorio et ON r.id_estado = et.Id) AS SRC ON TGT.EstadoTransitorioId = SRC.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaReten, Observaciones, Turno) VALUES (SRC.id_estado, SRC.fecha_reten, SRC.descripcion, SRC.nom_turno);
    MERGE dbo.SumarioDetalle AS TGT USING (SELECT s.id_estado, s.fecha_inicio, s.fecha_fin, s.descripcion, s.doc_resolucion FROM Personal.dbo.tblSumariosDetalle s JOIN dbo.EstadoTransitorio et ON s.id_estado = et.Id) AS SRC ON TGT.EstadoTransitorioId = SRC.id_estado WHEN NOT MATCHED THEN INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Expediente) VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion, SRC.doc_resolucion);

    SET IDENTITY_INSERT dbo.HistoricoLicencia ON;
    MERGE dbo.HistoricoLicencia AS TGT USING ( SELECT l.id_licencia, mp.FuncionarioId, l.id_ausencia AS TipoLicenciaId, l.inicio, l.finaliza, l.fecha_registro, l.fecha_actualizado, l.usuario, l.datos, l.estado, l.Unidad_Ejecutora, l.Unidad_Organizativa, l.Cantidad, l.Cantidad_dentro_del_período, l.Unidad, l.Afecta_a_días, l.C_Presentó_certificado_, l.Usuario_aprobó_anuló_rechazó, l.Fecha_aprobación_anulación_rechazo, l.Comentario FROM Personal.dbo.tblLicencias AS l JOIN dbo.MapPoliciaFunc AS mp ON l.id_policia = mp.id_policia ) AS SRC ON TGT.Id = SRC.id_licencia
    WHEN NOT MATCHED BY TARGET THEN INSERT (Id,FuncionarioId,TipoLicenciaId,inicio,finaliza,fecha_registro,fecha_actualizado,usuario,datos,estado,Unidad_Ejecutora,Unidad_Organizativa,Cantidad,Cantidad_dentro_del_período,Unidad,Afecta_a_días,C_Presentó_certificado_,Usuario_aprobó_anuló_rechazó,Fecha_aprobación_anulación_rechazo,Comentario) VALUES(SRC.id_licencia,SRC.FuncionarioId,SRC.TipoLicenciaId,SRC.inicio,SRC.finaliza,SRC.fecha_registro,SRC.fecha_actualizado,SRC.usuario,SRC.datos,SRC.estado,SRC.Unidad_Ejecutora,SRC.Unidad_Organizativa,SRC.Cantidad,SRC.Cantidad_dentro_del_período,SRC.Unidad,SRC.Afecta_a_días,SRC.C_Presentó_certificado_,SRC.Usuario_aprobó_anuló_rechazó,SRC.Fecha_aprobación_anulación_rechazo,SRC.Comentario);
    SET IDENTITY_INSERT dbo.HistoricoLicencia OFF;

    ;WITH CustodiasUnicas AS ( SELECT hc.id_policia,hc.fecha,MIN(hc.area) AS area FROM Personal.dbo.tblHistoricoCustodias hc GROUP BY hc.id_policia,hc.fecha ) INSERT INTO dbo.HistoricoCustodia (FuncionarioId,Fecha,Area) SELECT m.FuncionarioId,cu.fecha,cu.area FROM CustodiasUnicas cu JOIN dbo.MapPoliciaFunc m ON m.id_policia = cu.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoCustodia hc WHERE hc.FuncionarioId = m.FuncionarioId AND hc.Fecha = cu.fecha);
    ;WITH NoctAgreg AS ( SELECT hn.id_policia,hn.año,hn.mes,SUM(hn.minutos) AS minutos FROM Personal.dbo.tblHistoricoNocturnidad hn GROUP BY hn.id_policia,hn.año,hn.mes) INSERT INTO dbo.HistoricoNocturnidad (FuncionarioId,Anio,Mes,Minutos) SELECT m.FuncionarioId,na.año,na.mes,na.minutos FROM NoctAgreg na JOIN dbo.MapPoliciaFunc m ON m.id_policia = na.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoNocturnidad hn WHERE hn.FuncionarioId = m.FuncionarioId AND hn.Anio = na.año AND hn.Mes = na.mes);
    ;WITH PresAgreg AS ( SELECT hp.id_policia,hp.año,hp.mes,SUM(hp.minutos) AS minutos,SUM(hp.dias) AS dias, MIN(hp.incidencia) AS incidencia,MIN(hp.observaciones) AS observaciones FROM Personal.dbo.tblHistoricoPresentismo hp GROUP BY hp.id_policia,hp.año,hp.mes ) INSERT INTO dbo.HistoricoPresentismo (FuncionarioId,Anio,Mes,Minutos,Dias,Incidencia,Observaciones) SELECT m.FuncionarioId,pa.año,pa.mes,pa.minutos,pa.dias,pa.incidencia,pa.observaciones FROM PresAgreg pa JOIN dbo.MapPoliciaFunc m ON m.id_policia = pa.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoPresentismo hp WHERE hp.FuncionarioId = m.FuncionarioId AND hp.Anio = pa.año AND hp.Mes = pa.mes);
    ;WITH ViatAgreg AS ( SELECT hv.id_policia,hv.año,hv.mes,MIN(hv.incidencia) AS incidencia,MIN(hv.motivo) AS motivo FROM Personal.dbo.tblHistoricoViaticos hv GROUP BY hv.id_policia,hv.año,hv.mes) INSERT INTO dbo.HistoricoViatico (FuncionarioId,Anio,Mes,Incidencia,Motivo) SELECT m.FuncionarioId,va.año,va.mes,va.incidencia,va.motivo FROM ViatAgreg va JOIN dbo.MapPoliciaFunc m ON m.id_policia = va.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoViatico hv WHERE hv.FuncionarioId = m.FuncionarioId AND hv.Anio = va.año AND hv.Mes = va.mes);

    ;WITH Src AS ( SELECT u.id_usuario AS OrigId, LOWER(LTRIM(RTRIM(u.usuario))) COLLATE DATABASE_DEFAULT AS UserName, HASHBYTES('SHA2_256',CAST(u.clave AS NVARCHAR(4000))) AS PwdHash, mp.FuncionarioId FROM Personal.dbo.tblUsuarios u LEFT JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = u.id_policia) MERGE dbo.Usuario AS tgt USING Src AS src ON tgt.UserName = src.UserName WHEN NOT MATCHED BY TARGET THEN INSERT (UserName,PasswordHash,FuncionarioId,RolId,CreatedAt) VALUES(src.UserName,src.PwdHash,src.FuncionarioId,2,GETDATE());

	PRINT 'Paso 3: Datos relacionados migrados.';

    /*================================================================
      PASO 4: Refinamiento y Categorización de Datos
    =================================================================*/
	PRINT 'Paso 4: Refinando y categorizando datos...';

	-- Clasificar Tipos de Licencia
	UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 2 WHERE [Id] IN (20, 21, 23, 59, 60, 68, 69, 71, 72, 73, 81, 82, 87, 88, 101, 104, 105, 3450, 3463, 3465, 3466, 3467, 4471); -- Categoría: Salud
	UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 3 WHERE [Id] IN (1, 5, 6, 7, 8, 12, 17, 18, 42, 43, 44, 45, 47, 53, 54, 78, 89, 91, 3446, 3456, 3457, 3458, 3459, 4467, 4470, 4472); -- Categoría: Especial
	UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 4 WHERE [Id] IN (38, 39, 40, 41, 2443, 3444); -- Categoría: Sanción Leve
	UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 5 WHERE [Id] IN (2, 90, 97, 98, 99, 102, 106, 107, 108, 109, 110, 3455, 3462, 4469); -- Categoría: Sanción Grave
	UPDATE [dbo].[TipoLicencia] SET [CategoriaAusenciaId] = 1 WHERE [CategoriaAusenciaId] IS NULL OR [Id] IN (3, 4, 9, 10, 11, 13, 14, 15, 16, 19, 22, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 46, 48, 49, 50, 51, 52, 55, 56, 57, 58, 61, 62, 63, 64, 65, 66, 67, 70, 74, 75, 76, 77, 79, 80, 83, 84, 85, 86, 92, 93, 94, 95, 96, 100, 103, 2444, 3445, 3447, 3448, 3449, 3460, 3468, 4468); -- Categoría: General
	
	-- Actualizar TipoFuncionarioId según escalafón
	UPDATE F SET F.TipoFuncionarioId = CASE E.Nombre
                            WHEN 'L' THEN 1 WHEN 'S' THEN 2 WHEN 'A' THEN 3
                            WHEN 'B' THEN 4 WHEN 'C' THEN 5 ELSE F.TipoFuncionarioId END
	FROM dbo.Funcionario AS F
	JOIN dbo.Escalafon AS E ON F.EscalafonId = E.Id
	WHERE E.Nombre IN ('L', 'S', 'A', 'B', 'C');

	PRINT 'Paso 4: Refinamiento completado.';
	
    /*================================================================
      PASO 5: Migración de Novedades, Notificaciones, Dotaciones y Estados Legales
    =================================================================*/
	PRINT 'Paso 5: Migrando Novedades, Notificaciones, Dotaciones y Estados Legales...';

    -- Notificaciones
    SET IDENTITY_INSERT dbo.NotificacionPersonal ON;
    MERGE dbo.NotificacionPersonal AS TGT
    USING (
        SELECT n.id_notificacion, mp.FuncionarioId, CAST(n.fecha AS DATETIME2(0)) AS FechaProgramada, n.nom_notificacion AS Medio,
               n.documento, LEFT(n.n_exp_ministerial, 50) AS ExpMinisterial, LEFT(n.n_exp_inr, 50) AS ExpINR,
               LEFT(n.procedencia, 200) AS Oficina, COALESCE(tn.Id, 4) AS TipoNotificacionId,
               CASE LTRIM(RTRIM(n.estado)) WHEN 'Pendiente' THEN 1 WHEN 'Vencida' THEN 2 WHEN 'Firmada' THEN 3 ELSE 1 END AS EstadoId
        FROM Personal.dbo.tblNotificaciones n
        JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = n.id_policia
        LEFT JOIN dbo.TipoNotificacion tn ON tn.Nombre = UPPER(LTRIM(RTRIM(n.tipo_notificacion))) COLLATE Modern_Spanish_CI_AS
    ) AS SRC ON TGT.Id = SRC.id_notificacion
    WHEN NOT MATCHED THEN
        INSERT (Id, FuncionarioId, FechaProgramada, Medio, Documento, ExpMinisterial, ExpINR, Oficina, TipoNotificacionId, EstadoId, CreatedAt)
        VALUES (SRC.id_notificacion, SRC.FuncionarioId, SRC.FechaProgramada, SRC.Medio, SRC.documento, SRC.ExpMinisterial, SRC.ExpINR, SRC.Oficina, SRC.TipoNotificacionId, SRC.EstadoId, GETDATE());
    SET IDENTITY_INSERT dbo.NotificacionPersonal OFF;

    -- Novedades
    SET IDENTITY_INSERT dbo.NovedadGenerada ON;
    MERGE dbo.NovedadGenerada AS tgt USING (SELECT id_novedad_generada, fecha FROM Personal.dbo.tblNovedadesGeneradas) AS src ON tgt.Id = src.id_novedad_generada
    WHEN NOT MATCHED THEN INSERT (Id, Fecha, CreatedAt) VALUES (src.id_novedad_generada, src.fecha, GETDATE());
    SET IDENTITY_INSERT dbo.NovedadGenerada OFF;

    SET IDENTITY_INSERT dbo.Novedad ON;
    MERGE dbo.Novedad AS tgt
    USING (
        SELECT n.id_novedad, n.id_novedad_generada, n.fecha, n.novedad AS Texto,
               CASE n.Estado WHEN 'Pendiente' THEN 1 WHEN 'Vencida' THEN 2 WHEN 'Firmada' THEN 3 ELSE 1 END AS EstadoId
        FROM Personal.dbo.tblNovedades n
    ) AS src ON tgt.Id = src.id_novedad
    WHEN NOT MATCHED THEN INSERT (Id, NovedadGeneradaId, Fecha, Texto, EstadoId, CreatedAt) VALUES (src.id_novedad, src.id_novedad_generada, src.fecha, src.Texto, src.EstadoId, GETDATE());
    SET IDENTITY_INSERT dbo.Novedad OFF;
    
    MERGE dbo.NovedadFuncionario AS tgt
    USING (SELECT n.id_novedad, mp.FuncionarioId FROM Personal.dbo.tblNovedades n JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = n.id_policia) AS src
    ON tgt.NovedadId = src.id_novedad AND tgt.FuncionarioId = src.FuncionarioId
    WHEN NOT MATCHED THEN INSERT (NovedadId, FuncionarioId) VALUES (src.id_novedad, src.FuncionarioId);

    SET IDENTITY_INSERT dbo.NovedadFoto ON;
    MERGE dbo.NovedadFoto AS TGT
    USING (
        SELECT f.id_foto_novedad, CAST(f.Data AS VARBINARY(MAX)) AS Foto,
              (SELECT TOP 1 n.Id FROM dbo.Novedad n WHERE n.NovedadGeneradaId = f.id_novedad_generada ORDER BY n.Id) as NovedadId
        FROM Personal.dbo.tblNovedadesFotos f
    ) AS SRC ON TGT.Id = SRC.id_foto_novedad
    WHEN NOT MATCHED AND SRC.NovedadId IS NOT NULL THEN INSERT (Id, Foto, NovedadId, CreatedAt) VALUES (SRC.id_foto_novedad, SRC.Foto, SRC.NovedadId, GETDATE());
    SET IDENTITY_INSERT dbo.NovedadFoto OFF;
	
	-- Dotaciones: Armas, Chalecos y Equipamiento
	PRINT 'Paso 5.1: Migrando Dotaciones...';
	INSERT INTO dbo.Arma (Marca, Modelo, Calibre)
SELECT DISTINCT
    LTRIM(RTRIM(p.marcaArma)),
    LTRIM(RTRIM(p.modeloArma)),
    '9mm'
FROM Personal.dbo.tblPolicias AS p
WHERE p.marcaArma IS NOT NULL
  AND p.modeloArma IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.Arma d
      WHERE d.Marca  = LTRIM(RTRIM(p.marcaArma))  COLLATE DATABASE_DEFAULT
        AND d.Modelo = LTRIM(RTRIM(p.modeloArma)) COLLATE DATABASE_DEFAULT
  );


	INSERT INTO dbo.FuncionarioArma (FuncionarioId, ArmaId, Serie, Cargadores, Municiones, Observaciones, FechaAsign)
SELECT
    m.FuncionarioId,
    a.Id,
    p.serieArma,
    TRY_CAST(p.cargadoresArma AS INT),
    TRY_CAST(p.municionesArma AS INT),
    p.observacionesArma,
    GETDATE()
FROM Personal.dbo.tblPolicias AS p
JOIN dbo.MapPoliciaFunc AS m
  ON p.id_policia = m.id_policia
JOIN dbo.Arma AS a
  ON a.Marca  = LTRIM(RTRIM(p.marcaArma))  COLLATE DATABASE_DEFAULT
 AND a.Modelo = LTRIM(RTRIM(p.modeloArma)) COLLATE DATABASE_DEFAULT
WHERE p.marcaArma IS NOT NULL
  AND p.modeloArma IS NOT NULL
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.FuncionarioArma fa
      WHERE fa.FuncionarioId = m.FuncionarioId
        AND fa.ArmaId = a.Id
  );


	INSERT INTO dbo.FuncionarioChaleco (FuncionarioId, Marca, Modelo, Serie, Tipo, Observaciones, FechaAsign)
SELECT
    m.FuncionarioId,
    p.marcaChaleco,
    p.modeloChaleco,
    p.serieChaleco,
    p.tipoChaleco,
    p.observacionesChaleco,
    GETDATE()
FROM Personal.dbo.tblPolicias AS p
JOIN dbo.MapPoliciaFunc AS m
  ON p.id_policia = m.id_policia
WHERE (p.marcaChaleco IS NOT NULL OR p.modeloChaleco IS NOT NULL OR p.serieChaleco IS NOT NULL)
  AND NOT EXISTS (
      SELECT 1
      FROM dbo.FuncionarioChaleco fc
      WHERE fc.FuncionarioId = m.FuncionarioId
        AND fc.Serie = p.serieChaleco COLLATE DATABASE_DEFAULT
  );


	MERGE INTO dbo.DotacionItem AS Target
	USING (VALUES ('Camisa'), ('Pantalón'), ('Botas'), ('Zapatos'), ('Gorro'), ('Buso'), ('Campera'), ('Esposas')) AS Source (Nombre)
	ON Target.Nombre = Source.Nombre
	WHEN NOT MATCHED BY TARGET THEN INSERT (Nombre, Activo, CreatedAt) VALUES (Source.Nombre, 1, GETDATE());

	WITH DotacionesUnpivot AS (
		SELECT id_policia, Item, Talla FROM Personal.dbo.tblPolicias
		UNPIVOT (Talla FOR Item IN (camisa, pantalon, botas, zapatos, gorro, buso, campera, esposas)) AS unpvt
	)
	INSERT INTO dbo.FuncionarioDotacion (FuncionarioId, DotacionItemId, Talla, FechaAsign)
	SELECT m.FuncionarioId, di.Id, du.Talla, GETDATE()
	FROM DotacionesUnpivot AS du
	JOIN dbo.MapPoliciaFunc AS m ON du.id_policia = m.id_policia
	JOIN dbo.DotacionItem AS di ON di.Nombre = du.Item
	WHERE LTRIM(RTRIM(du.Talla)) <> '' AND du.Talla IS NOT NULL
	  AND NOT EXISTS (SELECT 1 FROM dbo.FuncionarioDotacion fd WHERE fd.FuncionarioId = m.FuncionarioId AND fd.DotacionItemId = di.Id);

	-- Estados Legales
	PRINT 'Paso 5.2: Migrando Estados Legales...';
	INSERT INTO dbo.FuncionarioEstadoLegal (FuncionarioId, Tipo, Valor, FechaRegistro)
	SELECT F.Id, 'Sumariado', 'SI', GETDATE()
	FROM Personal.dbo.tblPolicias AS P JOIN Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
	WHERE P.sumariado IS NOT NULL AND LTRIM(RTRIM(P.sumariado)) <> '' AND NOT EXISTS (SELECT 1 FROM dbo.FuncionarioEstadoLegal WHERE FuncionarioId = F.Id AND Tipo = 'Sumariado');

	INSERT INTO dbo.FuncionarioEstadoLegal (FuncionarioId, Tipo, Valor, FechaRegistro)
	SELECT F.Id, 'Separado del Cargo', 'SI', GETDATE()
	FROM Personal.dbo.tblPolicias AS P JOIN Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
	WHERE P.separado_cargo IS NOT NULL AND LTRIM(RTRIM(P.separado_cargo)) <> '' AND NOT EXISTS (SELECT 1 FROM dbo.FuncionarioEstadoLegal WHERE FuncionarioId = F.Id AND Tipo = 'Separado del Cargo');

	INSERT INTO dbo.FuncionarioEstadoLegal (FuncionarioId, Tipo, Valor, FechaRegistro)
	SELECT F.Id, 'Procesado', 'SI', GETDATE()
	FROM Personal.dbo.tblPolicias AS P JOIN Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
	WHERE P.procesado IS NOT NULL AND LTRIM(RTRIM(P.procesado)) <> '' AND NOT EXISTS (SELECT 1 FROM dbo.FuncionarioEstadoLegal WHERE FuncionarioId = F.Id AND Tipo = 'Procesado');

	INSERT INTO dbo.FuncionarioEstadoLegal (FuncionarioId, Tipo, Valor, FechaRegistro)
	SELECT F.Id, 'Desarmado', 'SI', GETDATE()
	FROM Personal.dbo.tblPolicias AS P JOIN Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
	WHERE P.desarmado IS NOT NULL AND LTRIM(RTRIM(P.desarmado)) <> '' AND NOT EXISTS (SELECT 1 FROM dbo.FuncionarioEstadoLegal WHERE FuncionarioId = F.Id AND Tipo = 'Desarmado');

	PRINT 'Paso 5: Migraciones adicionales completadas.';

    /*================================================================
      PASO 6: Migrar Fotos de Funcionarios
    =================================================================*/
    PRINT 'Paso 6: Migrando fotos de funcionarios...';

    MERGE dbo.FuncionarioFotoHistorico AS tgt
    USING ( SELECT p.id_picture AS PictureId, mp.FuncionarioId, CAST(p.Data AS VARBINARY(MAX)) AS Foto, p.FileName FROM Personal.dbo.tblPictures p JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = p.id_policia ) AS src
      ON tgt.PictureId = src.PictureId
    WHEN NOT MATCHED BY TARGET THEN INSERT (FuncionarioId, PictureId, Foto, FileName) VALUES (src.FuncionarioId, src.PictureId, src.Foto, src.FileName);

    ;WITH UltimaFoto AS ( SELECT ffh.FuncionarioId, ffh.Foto, ROW_NUMBER() OVER(PARTITION BY ffh.FuncionarioId ORDER BY ffh.PictureId DESC) as rn FROM dbo.FuncionarioFotoHistorico ffh )
    UPDATE f SET f.Foto = uf.Foto
    FROM dbo.Funcionario f
    JOIN UltimaFoto uf ON f.Id = uf.FuncionarioId
    WHERE uf.rn = 1;

    PRINT 'Paso 6: Fotos de funcionarios migradas.';
  
    PRINT '--- MIGRACIÓN COMPLETA Y UNIFICADA FINALIZADA CON ÉXITO ---';
    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    PRINT '!!! ERROR DURANTE LA MIGRACIÓN !!!';
    THROW;
END CATCH;
GO

