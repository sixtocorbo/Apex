/*
===============================================================================
  SCRIPT DE MIGRACIÓN COMPLETO: Personal -> Apex (CORREGIDO v4)
  Autor:        Equipo de migración (revisado y corregido por Gemini)
  Fecha:        07/08/2025
  Descripción: Migra todos los datos desde la base 'Personal' hacia 'Apex'.
                Este script ha sido corregido para mapear completamente los
                campos de las tablas de detalle y resolver conflictos de
                intercalación (collation).

  Notas:
  - Se utiliza COLLATE DATABASE_DEFAULT para que las comparaciones de texto
    utilicen la configuración de la base de datos de destino (Apex).
  - Se utilizan sentencias MERGE para las tablas de detalle, asegurando
    que el script se pueda ejecutar múltiples veces sin duplicar datos.
===============================================================================
*/

USE [Apex];
GO

-- Se redefine y corrige el procedimiento de limpieza.
IF OBJECT_ID('usp_LimpiarDatosDeApex', 'P') IS NOT NULL
    DROP PROCEDURE usp_LimpiarDatosDeApex;
GO

CREATE PROCEDURE usp_LimpiarDatosDeApex
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        PRINT '--- INICIANDO LIMPIEZA DE DATOS EN APEX ---';
        
        -- Eliminación desde las tablas de detalle primero
        DELETE FROM [dbo].[DesignacionDetalle];
        DELETE FROM [dbo].[EnfermedadDetalle];
        DELETE FROM [dbo].[SancionDetalle];
        DELETE FROM [dbo].[OrdenCincoDetalle];
        DELETE FROM [dbo].[RetenDetalle];
        DELETE FROM [dbo].[SumarioDetalle];

        -- Se eliminan en el orden correcto para evitar conflictos de FK
        DELETE FROM [dbo].[NovedadFuncionario];
        DELETE FROM [dbo].[NovedadFoto];
        DELETE FROM [dbo].[Novedad];
        DELETE FROM [dbo].[NovedadGenerada];
        DELETE FROM [dbo].[Movimiento];
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
        DELETE FROM [dbo].[Funcionario];
        DELETE FROM [dbo].[Arma];
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
        DELETE FROM [dbo].[PuestoTrabajo];
        DELETE FROM [dbo].[Seccion];
        DELETE FROM [dbo].[Semana];
        DELETE FROM [dbo].[Turno];
        DELETE FROM [dbo].[NotificacionEstado];
        DELETE FROM [dbo].[TipoNotificacion];
        DELETE FROM [dbo].[RolUsuario];
        DELETE FROM [dbo].[TipoFuncionario];
        DELETE FROM [dbo].[TipoViatico];
        DELETE FROM [dbo].[AreaTrabajo];
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
exec usp_LimpiarDatosDeApex

BEGIN TRY
    -- Se asegura que IDENTITY_INSERT esté desactivado
    PRINT 'Asegurando que IDENTITY_INSERT esté desactivado para todas las tablas...';
    BEGIN TRY SET IDENTITY_INSERT dbo.Cargo OFF;                  END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.Estado OFF;                 END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.Seccion OFF;                END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.PuestoTrabajo OFF;          END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.Turno OFF;                  END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.Semana OFF;                 END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.Horario OFF;                END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.TipoLicencia OFF;           END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.Genero OFF;                 END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.EstadoCivil OFF;            END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.NivelEstudio OFF;           END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.TipoEstadoTransitorio OFF;  END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.EstadoTransitorio OFF;      END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.HistoricoLicencia OFF;      END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.CategoriaAusencia OFF;      END TRY BEGIN CATCH END CATCH;
    BEGIN TRY SET IDENTITY_INSERT dbo.FuncionarioFotoHistorico OFF; END TRY BEGIN CATCH END CATCH;

    BEGIN TRANSACTION;

    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    PRINT '--- INICIO DE LA MIGRACIÓN COMPLETA ---';

    /*================================================================
      PASO 1: Poblar Catálogos
    =================================================================*/
    PRINT 'Paso 1: Poblando catálogos...';

    -- TipoFuncionario
    SET IDENTITY_INSERT dbo.TipoFuncionario ON;
    MERGE dbo.TipoFuncionario AS tgt USING (VALUES (1,'Funcionario')) AS src(Id,Nombre) ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.TipoFuncionario OFF;

    -- RolUsuario
    MERGE dbo.RolUsuario AS tgt USING (VALUES (1,'Admin',1), (2,'Usuario',2)) AS src(Id,Nombre,Orden) ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre,Orden) VALUES (src.Id,src.Nombre,src.Orden);
    
    -- CategoriaAusencia
    SET IDENTITY_INSERT dbo.CategoriaAusencia ON;
    MERGE dbo.CategoriaAusencia AS tgt USING (VALUES (1,'General'),(2,'Médica'),(3,'Especial')) AS src(Id,Nombre) ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre);
    SET IDENTITY_INSERT dbo.CategoriaAusencia OFF;

    -- Cargo
    SET IDENTITY_INSERT dbo.Cargo ON;
    MERGE dbo.Cargo AS tgt
    USING (VALUES
        (1,'AGENTE',1), (2,'CABO',2), (3,'SARGENTO',3), (4,'SUB OFICIAL MAYOR',4), (5,'OFICIAL AYUDANTE',5), (6,'OFICIAL PRINCIPAL',6),
        (7,'SUB COMISARIO',7), (8,'COMISARIO',8), (9,'COMISARIO MAYOR',9), (10,'COMISARIO GENERAL',10), (14,'LICENCIADO EN PSICOLOGIA (9)',9),
        (15,'ASISTENTE SOCIAL',10), (16,'OP.PEN. G II',2), (17,'OP.PEN. G III',3), (18,'OP.PEN. G I',1), (19,'ALCAIDE',7),
        (20,'LIC. TRABAJO SOCIAL',8), (21,'EDUCADOR SOCIAL (10)',10), (22,'PROFESOR EDUCACION FISICA',9), (23,'GUARDIA',1),
        (24,'EDUCADOR SOCIAL (7)',7), (25,'OP. PEN. G IV',4), (26,'SUPERVISOR PENITENCIARIO',5), (27,'SUB ALCAIDE',6),
        (28,'ALCAIDE MAYOR',8), (29,'ESC B (4)',4), (30,'PA 2',2), (31,'PA 3',3), (32,'PA 6',6), (33,'PA 8',8), (34,'PE 3',3),
        (35,'PE 6',6), (36,'PE 8',8), (37,'PE 9',9), (38,'VERIFICAR',10), (39,'LICENCIADO EN PSICOLOGIA (11)',11),
        (1039,'ESC. A PROFESIONAL',8), (1040,'ESC B (9)',9), (1041,'ESC. B',8), (1042,'ESC. C',5), (1043,'SUB PREFECTO',9), (1044,'PREFECTO',10)
    ) AS src(Id,Nombre,Grado)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre,Grado,CreatedAt) VALUES(src.Id,src.Nombre,src.Grado,GETDATE());
    SET IDENTITY_INSERT dbo.Cargo OFF;

    -- Estado
    SET IDENTITY_INSERT dbo.Estado ON;
	MERGE dbo.Estado AS tgt
	USING (
		SELECT
			id_estado,
			nom_estado,
			CASE WHEN nom_estado IN ('AU', 'O5', 'CO') THEN 1 ELSE 0 END AS EsAusentismo
		FROM Personal.dbo.tblEstados
	) AS src(Id, Nombre, EsAusentismo)
	ON tgt.Id = src.Id
	WHEN NOT MATCHED THEN
		INSERT (Id, Nombre, EsAusentismo, CreatedAt)
		VALUES (src.Id, src.Nombre, src.EsAusentismo, GETDATE());
	SET IDENTITY_INSERT dbo.Estado OFF;

    -- Catálogos restantes (Sección, PuestoTrabajo, etc.)
    SET IDENTITY_INSERT dbo.Seccion ON;
    MERGE dbo.Seccion AS tgt USING (VALUES (1,'4'),(2,'4A'),(3,'4B'),(4,'4C'),(5,'4D'),(6,'4E'),(7,'VERIFICAR'),(8,'4JF'),(9,'4CA'),(10,'4CT')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Seccion OFF;
    SET IDENTITY_INSERT dbo.PuestoTrabajo ON;
    MERGE dbo.PuestoTrabajo AS tgt USING (VALUES (1,'MODULO 6'),(2,'MODULO 7'),(3,'MODULO 9'),(4,'OFICINA OPERATIVA'),(5,'OFICINA ADMINISTRATIVA'),(6,'OFICINA TECNICA'),(7,'JURIDICA'),(8,'GUARDIA'),(27,'OBRA'),(28,'CONTROL INGRESOS'),(29,'PERSONAL'),(30,'JEFE DE SERVICIO'),(31,'EDUCACION'),(32,'ECONOMATO'),(33,'HERRERIA'),(34,'SECRETARIA GENERAL'),(35,'LABORAL'),(36,'DIRECTOR'),(37,'ASSE'),(38,'MODULO 10'),(39,'MODULO 11'),(40,'MODULO 4'),(41,'MODULO 5'),(42,'MODULO 2'),(43,'MODULO 3'),(44,'MODULO 8'),(45,'MODULO 12'),(1038,'SUB DIRECTOR TECNICO'),(1039,'SUB DIRECTOR OPERATIVO'),(1040,'SUB DIRECTOR ADMINISTRATIVO'),(1041,'JUNTA MEDICA'),(1042,'LICENCIA MEDICA'),(1043,'EXTRAMUROS'),(1044,'VERIFICAR'),(1045,'CONYUGALES'),(1046,'PORTON PRINCIPAL'),(1047,'PORTON 22'),(1048,'REVISORIA'),(1049,'JEFATURA DE SERVICIO'),(1050,'LOCUTORIO DE ABOGADOS'),(1051,'ENCOMIENDAS'),(1052,'COCINA C.G.E.'),(1053,'SUB DIRECCION OPERATIVA'),(1054,'O.C.T'),(1055,'SUB DIRECCION TECNICA'),(1056,'COORDINACION TECNICA'),(2056,'SUB DIRECCION ADMINISTRATIVA (SECRETARIA)'),(2057,'TESORERIA Y VALORES'),(2058,'CENTRO MOTOR - SANITARIO-'),(2059,'CENTRO MOTOR - HERRERIA -'),(2060,'CENTRO MOTOR - ELECTRICISTA -'),(2061,'ARMERIA'),(3056,'OGLAST'),(3057,'REDENCION DE PENA'),(3058,'ARCHIVO GENERAL DEL COMPLEJO'),(3059,'DEPORTE Y RECREACIÓN'),(3060,'ÁREA DE TRATAMIENTO'),(3061,'COORDINACION OPERATIVA'),(3062,'COORDINACION GENERAL'),(3063,'COORDINACION ADMINISTRATIVA'),(3064,'IGLESIA'),(3065,'SUB DIRECCION ADMINISTRATIVA'),(4064,'ENCARGADO DE DESPACHO DE UNIDAD 4B'),(4065,'ENCARGADO DE DESPACHO SDO'),(4066,'CENTRO MOTOR - INFORMATICO'),(4067,'ENCARGADA CASINO SSOO'),(4068,'CHOFER'),(4069,'Simagin'),(4070,'AMBOS'),(4071,'CASINO SSOO'),(4072,'COORDINACION DE RECLUSION'),(4073,'MODULO 1'),(4074,'EVALUACION Y TRATAMIENTO'),(5072,'COORDINADOR GENERAL'),(5073,'COORDINADOR ADMINISTRATIVO'),(5074,'COORDINADOR OPERATIVO'),(5075,'COORDINADOR TÉCNICO'),(5076,'COORDINADOR DE RECLUSIÓN'),(5077,'GESTORIA DE AREAS VERDES'),(5078,'REFERENTE DE SALUD'),(6078,'4CT'),(6079,'ANEXO - UTU'),(7079,'SALA DE MONITOREO')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt,Activo) VALUES(src.Id,src.Nombre,GETDATE(),1);
    SET IDENTITY_INSERT dbo.PuestoTrabajo OFF;
    SET IDENTITY_INSERT dbo.Turno ON;
    MERGE dbo.Turno AS tgt USING (VALUES (1,'DIURNO'),(2,'NOCTURNO'),(3,'VERIFICAR')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Turno OFF;
    SET IDENTITY_INSERT dbo.Semana ON;
    MERGE dbo.Semana AS tgt USING (VALUES (1,'SEMANA 1'),(2,'SEMANA 2'),(3,'LMMJV'),(4,'12 X 36'),(5,'24 X 48'),(6,'LMMJVS'),(7,'MMJVS'),(8,'DLMMJV'),(10,'DLMMJ'),(11,'5 X 2'),(12,'AMBAS'),(13,'FULL'),(14,'LMV'),(15,'MJVSD'),(16,'VERIFICAR')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Semana OFF;
    SET IDENTITY_INSERT dbo.Horario ON;
    MERGE dbo.Horario AS tgt USING (VALUES (1,'07_19'),(2,'19_07'),(3,'09_15'),(4,'08_15'),(5,'07_15'),(6,'22_10'),(7,'09_16'),(8,'08_16'),(9,'ROTATIVO'),(10,'FULL'),(11,'MATERNAL'),(12,'00_06'),(13,'00_08'),(14,'23_06'),(15,'08_14'),(16,'06_12'),(17,'12_18'),(18,'12_20'),(19,'22_06'),(21,'1830_0630'),(22,'07_14'),(23,'07_13'),(24,'11_17'),(25,'23_07'),(26,'15_23'),(27,'06_14'),(28,'0930_1730'),(29,'13_21'),(30,'10_18'),(31,'06_18'),(32,'0630_1430'),(33,'0730_1530'),(34,'0750_1550'),(35,'07_1412'),(36,'07_17'),(37,'08_1515'),(38,'0815_1527'),(39,'0830_1630'),(40,'09_17'),(41,'10_16'),(42,'11_15'),(43,'11_19'),(44,'12_16'),(45,'14_22'),(46,'18_06'),(48,'FLEXIBLE DIARIO'),(49,'PRESENCIAL'),(50,'VERIFICAR'),(51,'L,MI,V 09. MA,J 10'),(52,'1030_1830'),(53,'08_20'),(54,'09_13'),(55,'07_1730'),(56,'08_1512'),(57,'08_12'),(58,'24 HS')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Horario OFF;
    SET IDENTITY_INSERT dbo.TipoLicencia ON;
    MERGE dbo.TipoLicencia AS tgt USING (
        SELECT id_ausencia,
               nom_ausencia,
               CASE WHEN ausente = 'SI' THEN 1 ELSE 0 END,
               CASE WHEN viatico = 'NO' THEN 1 ELSE 0 END,
               CASE WHEN presentismo = 'NO' THEN 1 ELSE 0 END,
               CASE WHEN habil = 'SI' THEN 1 ELSE 0 END
        FROM Personal.dbo.tblAusencias
    ) AS src(Id,Nombre,EsAusencia,SuspendeViatico,AfectaPresentismo,EsHabil)
    ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
        INSERT (Id,Nombre,EsAusencia,SuspendeViatico,AfectaPresentismo,EsHabil,CreatedAt,CategoriaAusenciaId)
        VALUES(src.Id,src.Nombre,src.EsAusencia,src.SuspendeViatico,src.AfectaPresentismo,src.EsHabil,GETDATE(),1);
    SET IDENTITY_INSERT dbo.TipoLicencia OFF;
    SET IDENTITY_INSERT dbo.Genero ON;
    MERGE dbo.Genero AS tgt USING (VALUES (1,'Masculino'),(2,'Femenino'),(3,'Otro'),(4,'No especifica')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre) WHEN MATCHED AND tgt.Nombre <> src.Nombre THEN UPDATE SET Nombre = src.Nombre;
    SET IDENTITY_INSERT dbo.Genero OFF;
    SET IDENTITY_INSERT dbo.EstadoCivil ON;
    MERGE dbo.EstadoCivil AS tgt USING (VALUES (1,'Soltero/a'),(2,'Casado/a'),(3,'Divorciado/a'),(4,'Viudo/a'),(5,'Unión libre'),(6,'Separado/a'),(7,'No especifica')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre) WHEN MATCHED AND tgt.Nombre <> src.Nombre THEN UPDATE SET Nombre = src.Nombre;
    SET IDENTITY_INSERT dbo.EstadoCivil OFF;
    SET IDENTITY_INSERT dbo.NivelEstudio ON;
    MERGE dbo.NivelEstudio AS tgt USING (VALUES (1,'Sin estudios'),(2,'Primaria incompleta'),(3,'Primaria completa'),(4,'Secundaria incompleta'),(5,'Secundaria completa'),(6,'Técnico terciario'),(7,'Universitario incompleto'),(8,'Universitario completo'),(9,'Postgrado'),(10,'Otro')) AS src(Id,Nombre) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre) WHEN MATCHED AND tgt.Nombre <> src.Nombre THEN UPDATE SET Nombre = src.Nombre;
    SET IDENTITY_INSERT dbo.NivelEstudio OFF;
    SET IDENTITY_INSERT dbo.TipoEstadoTransitorio ON;
    MERGE dbo.TipoEstadoTransitorio AS tgt USING (VALUES (1,'Designación',0),(2,'Enfermedad',0),(3,'Sanción',0),(4,'Orden Cinco',0),(5,'Retén',0),(6,'Sumario',0)) AS src(Id,Nombre,EsJerarquico) ON tgt.Id = src.Id WHEN NOT MATCHED THEN INSERT (Id,Nombre,EsJerarquico,CreatedAt) VALUES(src.Id,src.Nombre,src.EsJerarquico,SYSUTCDATETIME()) WHEN MATCHED AND tgt.Nombre <> src.Nombre THEN UPDATE SET Nombre = src.Nombre, UpdatedAt = SYSUTCDATETIME();
    SET IDENTITY_INSERT dbo.TipoEstadoTransitorio OFF;
    
    -- CORRECCIÓN: Se añade COLLATE DATABASE_DEFAULT para resolver conflictos
    MERGE dbo.Escalafon AS tgt USING (SELECT DISTINCT LTRIM(RTRIM(p.escalafon)) AS Nombre FROM Personal.dbo.tblPolicias p WHERE p.escalafon IS NOT NULL AND LTRIM(RTRIM(p.escalafon)) <> '') AS src ON tgt.Nombre = src.Nombre COLLATE DATABASE_DEFAULT WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (src.Nombre);
    MERGE dbo.Funcion AS tgt USING (SELECT DISTINCT LTRIM(RTRIM(p.funcion)) AS Nombre FROM Personal.dbo.tblPolicias p WHERE p.funcion IS NOT NULL AND LTRIM(RTRIM(p.funcion)) <> '') AS src ON tgt.Nombre = src.Nombre COLLATE DATABASE_DEFAULT WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (src.Nombre);

    PRINT 'Paso 1: Catálogos completados.';

    /*================================================================
      PASO 2: Migrar Funcionarios
    =================================================================*/
    PRINT 'Paso 2: Migrando funcionarios...';
    IF OBJECT_ID('tempdb..#MapeoGenero') IS NOT NULL DROP TABLE #MapeoGenero;
    IF OBJECT_ID('tempdb..#MapeoEstadoCivil') IS NOT NULL DROP TABLE #MapeoEstadoCivil;
    IF OBJECT_ID('tempdb..#MapeoNivelEstudio') IS NOT NULL DROP TABLE #MapeoNivelEstudio;
    IF OBJECT_ID('tempdb..#MapeoEstado') IS NOT NULL DROP TABLE #MapeoEstado;

    CREATE TABLE #MapeoGenero      (TxtOrigen varchar(100) PRIMARY KEY, IdDestino int);
    CREATE TABLE #MapeoEstadoCivil (TxtOrigen varchar(100) PRIMARY KEY, IdDestino int);
    CREATE TABLE #MapeoNivelEstudio(TxtOrigen varchar(100) PRIMARY KEY, IdDestino int);
    CREATE TABLE #MapeoEstado      (IdOrigen int PRIMARY KEY, IdDestino int);

    INSERT INTO #MapeoGenero VALUES ('M',1),('F',2),('',4);
    INSERT INTO #MapeoEstadoCivil VALUES ('CASADO',2),('CASADO/A',2),('COMPROMETIDO/A',5),('DIVORCIADO',3),('DIVORCIADO/A',3),('SOLTERO',1),('SOLTERO/A',1),('',7);
    INSERT INTO #MapeoNivelEstudio VALUES ('CICLO BÁSICO COMPLETO',5),('CICLO BÁSICO INCOMPLETO',4),('EDUCACIÓN PRIMARIA',3),('EDUCACION SECUNDARIA',5),('EDUCACIÓN TERCIARIA',6),('PROFESIONAL',8),('',10);
    -- Se especifica COLLATE DATABASE_DEFAULT en la comparación para evitar conflictos de intercalación (Modern_Spanish_CI_AS vs Latin1_General_CI_AI)
    INSERT INTO #MapeoEstado(IdOrigen, IdDestino)
        SELECT p.id_estado, a.Id
        FROM Personal.dbo.tblEstados p
        JOIN dbo.Estado a
            ON p.nom_estado COLLATE DATABASE_DEFAULT = a.Nombre COLLATE DATABASE_DEFAULT;
    
    ;WITH PoliciasStg AS (
        SELECT p.id_policia AS IdPolicia,
               RIGHT('00000000' + CAST(p.num_cedula AS VARCHAR(8)),8) AS CI,
               LEFT(LTRIM(RTRIM(p.nom_policia)), 60) AS Nombre,
               p.alta AS FechaIngreso,
               CASE WHEN p.id_estado = 1 THEN 1 ELSE 0 END AS Activo, p.id_estado AS EstadoId_Origen, p.id_seccion AS SeccionId, p.id_puesto AS PuestoTrabajoId,
               p.id_turno AS TurnoId, p.id_semana AS SemanaId, p.id_horario AS HorarioId, UPPER(LTRIM(RTRIM(p.genero))) AS GeneroTxt,
               UPPER(LTRIM(RTRIM(p.nivelestudio))) AS NivelEstudioTxt, UPPER(LTRIM(RTRIM(p.estadocivil))) AS EstadoCivilTxt, p.id_grado AS CargoId,
               p.fecha_registro AS CreatedAt,
               p.fecha_actualizado AS UpdatedAt,
               p.fecha_nacimiento AS FechaNacimiento,
               LEFT(LTRIM(RTRIM(p.domicilio)), 250) AS Domicilio,
               LEFT(LTRIM(RTRIM(p.correo)), 255) AS Email,
               LEFT(LTRIM(RTRIM(p.telefono)), 50)  AS Telefono,
               LTRIM(RTRIM(p.escalafon)) AS EscalafonTxt,
               LTRIM(RTRIM(p.funcion)) AS FuncionTxt
        FROM Personal.dbo.tblPolicias p
    ),
    PoliciasMapeadas AS (
        SELECT s.CI, s.Nombre, s.FechaIngreso, s.Activo, meE.IdDestino AS EstadoId, s.SeccionId, s.PuestoTrabajoId, s.TurnoId, s.SemanaId, s.HorarioId,
               COALESCE(meG.IdDestino,4) AS GeneroId, COALESCE(meN.IdDestino,10) AS NivelEstudioId, COALESCE(meC.IdDestino,7) AS EstadoCivilId,
               s.CargoId, s.CreatedAt, s.UpdatedAt, s.FechaNacimiento, s.Domicilio, s.Email, s.Telefono AS Telefono, e.Id AS EscalafonId, f.Id AS FuncionId
        FROM PoliciasStg s
        LEFT JOIN #MapeoEstado meE ON meE.IdOrigen = s.EstadoId_Origen
        LEFT JOIN #MapeoGenero meG ON meG.TxtOrigen = COALESCE(s.GeneroTxt,'') COLLATE DATABASE_DEFAULT
        LEFT JOIN #MapeoNivelEstudio meN ON meN.TxtOrigen = COALESCE(s.NivelEstudioTxt,'') COLLATE DATABASE_DEFAULT
        LEFT JOIN #MapeoEstadoCivil meC ON meC.TxtOrigen = COALESCE(s.EstadoCivilTxt,'') COLLATE DATABASE_DEFAULT
        LEFT JOIN dbo.Escalafon e ON e.Nombre = s.EscalafonTxt COLLATE DATABASE_DEFAULT
        LEFT JOIN dbo.Funcion f ON f.Nombre = s.FuncionTxt COLLATE DATABASE_DEFAULT
    )
    MERGE dbo.Funcionario AS tgt
    USING PoliciasMapeadas AS src ON tgt.CI = src.CI COLLATE DATABASE_DEFAULT
    WHEN MATCHED THEN
      UPDATE SET tgt.Nombre = src.Nombre,
                 tgt.FechaIngreso = src.FechaIngreso,
                 tgt.Activo = src.Activo,
                 tgt.EstadoId = src.EstadoId,
                 tgt.SeccionId = src.SeccionId,
                 tgt.PuestoTrabajoId = src.PuestoTrabajoId,
                 tgt.TurnoId = src.TurnoId,
                 tgt.SemanaId = src.SemanaId,
                 tgt.HorarioId = src.HorarioId,
                 tgt.GeneroId = src.GeneroId,
                 tgt.NivelEstudioId = src.NivelEstudioId,
                 tgt.EstadoCivilId = src.EstadoCivilId,
                 tgt.CargoId = src.CargoId,
                 tgt.FechaNacimiento = src.FechaNacimiento,
                 tgt.Domicilio = src.Domicilio,
                 tgt.Email = src.Email,
                 tgt.Telefono = src.Telefono,
                 tgt.EscalafonId = src.EscalafonId,
                 tgt.FuncionId = src.FuncionId,
                 tgt.UpdatedAt = SYSUTCDATETIME()
    WHEN NOT MATCHED BY TARGET THEN
      INSERT (CI,Nombre,FechaIngreso,Activo,CreatedAt,UpdatedAt,EstadoId,SeccionId,PuestoTrabajoId,TurnoId,SemanaId,HorarioId,GeneroId,NivelEstudioId,EstadoCivilId,CargoId,FechaNacimiento,Domicilio,Email,Telefono,EscalafonId,FuncionId,TipoFuncionarioId)
      VALUES (src.CI,src.Nombre,src.FechaIngreso,src.Activo,src.CreatedAt,src.UpdatedAt,src.EstadoId,src.SeccionId,src.PuestoTrabajoId,src.TurnoId,src.SemanaId,src.HorarioId,src.GeneroId,src.NivelEstudioId,src.EstadoCivilId,src.CargoId,src.FechaNacimiento,src.Domicilio,src.Email,src.Telefono,src.EscalafonId,src.FuncionId,1);

    TRUNCATE TABLE dbo.MapPoliciaFunc;
    INSERT INTO dbo.MapPoliciaFunc (id_policia,FuncionarioId) SELECT p.id_policia,f.Id FROM Personal.dbo.tblPolicias p JOIN dbo.Funcionario f ON f.CI = RIGHT('00000000' + CAST(p.num_cedula AS VARCHAR(8)),8);

    PRINT 'Paso 2: Funcionarios migrados.';

    /*================================================================
      PASO 3: Migrar tablas dependientes
    =================================================================*/
    PRINT 'Paso 3: Migrando tablas dependientes...';
    
    -- CORRECCIÓN: Este bloque ahora solo inserta el registro principal en EstadoTransitorio.
    -- La fecha de creación se establece a la fecha actual de la migración.
    SET IDENTITY_INSERT dbo.EstadoTransitorio ON;
    MERGE dbo.EstadoTransitorio as TGT
    USING (
        SELECT 
            eu.id_estado,
            mp.FuncionarioId,
            CASE eu.categoria 
                WHEN 'D' THEN 1 WHEN 'E' THEN 2 WHEN 'N' THEN 3
                WHEN 'O' THEN 4 WHEN 'R' THEN 5 WHEN 'S' THEN 6
            END as TipoEstadoTransitorioId
        FROM Personal.dbo.tblEstadosUnificados eu
        JOIN dbo.MapPoliciaFunc mp ON eu.id_policia = mp.id_policia
    ) AS SRC
    ON TGT.Id = SRC.id_estado
    WHEN NOT MATCHED THEN
        INSERT (Id, FuncionarioId, TipoEstadoTransitorioId, CreatedAt)
        VALUES (SRC.id_estado, SRC.FuncionarioId, SRC.TipoEstadoTransitorioId, GETDATE());
    SET IDENTITY_INSERT dbo.EstadoTransitorio OFF;

    -- Migración de detalles
    MERGE dbo.DesignacionDetalle AS TGT
    USING (
        SELECT d.id_estado, d.fecha_inicio, d.fecha_fin, d.descripcion, d.doc_resolucion
        FROM Personal.dbo.tblDesignacionesDetalle d
    ) AS SRC
    ON TGT.EstadoTransitorioId = SRC.id_estado
    WHEN NOT MATCHED THEN
        INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, DocResolucion)
        VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion, SRC.doc_resolucion);

    MERGE dbo.EnfermedadDetalle AS TGT
    USING (
        SELECT e.id_estado, e.fecha_inicio, e.fecha_fin, e.descripcion, e.descripcion as Diagnostico -- Usando descripción como diagnóstico temporalmente
        FROM Personal.dbo.tblEnfermedadDetalle e
    ) AS SRC
    ON TGT.EstadoTransitorioId = SRC.id_estado
    WHEN NOT MATCHED THEN
        INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Diagnostico)
        VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion, SRC.Diagnostico);

    MERGE dbo.SancionDetalle AS TGT
    USING (
        SELECT s.id_estado, s.fecha_inicio, s.fecha_fin, s.descripcion, s.doc_resolucion
        FROM Personal.dbo.tblSancionesDetalle s
    ) AS SRC
    ON TGT.EstadoTransitorioId = SRC.id_estado
    WHEN NOT MATCHED THEN
        INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Resolucion)
        VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion, SRC.doc_resolucion);

    MERGE dbo.OrdenCincoDetalle AS TGT
    USING (
        SELECT o.id_estado, o.fecha_inicio, o.fecha_fin, o.descripcion
        FROM Personal.dbo.tblOrdenCincoDetalle o
    ) AS SRC
    ON TGT.EstadoTransitorioId = SRC.id_estado
    WHEN NOT MATCHED THEN
        INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones)
        VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion);

    MERGE dbo.RetenDetalle AS TGT
    USING (
        SELECT r.id_estado, r.fecha_reten, r.descripcion, t.nom_turno
        FROM Personal.dbo.tblRetenesDetalle r
        JOIN Personal.dbo.tblTurnos t ON r.id_turno = t.id_turno
    ) AS SRC
    ON TGT.EstadoTransitorioId = SRC.id_estado
    WHEN NOT MATCHED THEN
        INSERT (EstadoTransitorioId, FechaReten, Observaciones, Turno)
        VALUES (SRC.id_estado, SRC.fecha_reten, SRC.descripcion, SRC.nom_turno);

    MERGE dbo.SumarioDetalle AS TGT
    USING (
        SELECT s.id_estado, s.fecha_inicio, s.fecha_fin, s.descripcion, s.doc_resolucion
        FROM Personal.dbo.tblSumariosDetalle s
    ) AS SRC
    ON TGT.EstadoTransitorioId = SRC.id_estado
    WHEN NOT MATCHED THEN
        INSERT (EstadoTransitorioId, FechaDesde, FechaHasta, Observaciones, Expediente)
        VALUES (SRC.id_estado, SRC.fecha_inicio, SRC.fecha_fin, SRC.descripcion, SRC.doc_resolucion);


    SET IDENTITY_INSERT dbo.HistoricoLicencia ON;
    MERGE dbo.HistoricoLicencia AS TGT USING ( SELECT l.id_licencia, mp.FuncionarioId, l.id_ausencia AS TipoLicenciaId, l.inicio, l.finaliza, l.fecha_registro, l.fecha_actualizado, l.usuario, l.datos, l.estado, l.Unidad_Ejecutora, l.Unidad_Organizativa, l.Cantidad, l.Cantidad_dentro_del_período, l.Unidad, l.Afecta_a_días, l.C_Presentó_certificado_, l.Usuario_aprobó_anuló_rechazó, l.Fecha_aprobación_anulación_rechazo, l.Comentario FROM Personal.dbo.tblLicencias AS l JOIN dbo.MapPoliciaFunc AS mp ON l.id_policia = mp.id_policia ) AS SRC ON TGT.Id = SRC.id_licencia
    WHEN NOT MATCHED BY TARGET THEN INSERT (Id,FuncionarioId,TipoLicenciaId,inicio,finaliza,fecha_registro,fecha_actualizado,usuario,datos,estado,Unidad_Ejecutora,Unidad_Organizativa,Cantidad,Cantidad_dentro_del_período,Unidad,Afecta_a_días,C_Presentó_certificado_,Usuario_aprobó_anuló_rechazó,Fecha_aprobación_anulación_rechazo,Comentario) VALUES(SRC.id_licencia,SRC.FuncionarioId,SRC.TipoLicenciaId,SRC.inicio,SRC.finaliza,SRC.fecha_registro,SRC.fecha_actualizado,SRC.usuario,SRC.datos,SRC.estado,SRC.Unidad_Ejecutora,SRC.Unidad_Organizativa,SRC.Cantidad,SRC.Cantidad_dentro_del_período,SRC.Unidad,SRC.Afecta_a_días,SRC.C_Presentó_certificado_,SRC.Usuario_aprobó_anuló_rechazó,SRC.Fecha_aprobación_anulación_rechazo,SRC.Comentario);
    SET IDENTITY_INSERT dbo.HistoricoLicencia OFF;

    ;WITH SrcArmas AS ( SELECT DISTINCT CAST(LTRIM(RTRIM(a.marca)) AS NVARCHAR(50)) COLLATE DATABASE_DEFAULT AS Marca, CAST(LTRIM(RTRIM(a.modelo)) AS NVARCHAR(100)) COLLATE DATABASE_DEFAULT AS Modelo, CAST(LTRIM(RTRIM(a.calibre))AS NVARCHAR(50)) COLLATE DATABASE_DEFAULT AS Calibre FROM Personal.dbo.tblArmas a WHERE a.marca IS NOT NULL AND a.modelo IS NOT NULL) INSERT INTO dbo.Arma (Marca,Modelo,Calibre) SELECT s.Marca,s.Modelo,s.Calibre FROM SrcArmas s WHERE NOT EXISTS (SELECT 1 FROM dbo.Arma d WHERE d.Marca = s.Marca AND d.Modelo = s.Modelo);
    ;WITH SrcPoli AS ( SELECT DISTINCT CAST(LTRIM(RTRIM(p.marcaArma)) AS NVARCHAR(50)) COLLATE DATABASE_DEFAULT AS Marca, CAST(LTRIM(RTRIM(p.modeloArma)) AS NVARCHAR(100)) COLLATE DATABASE_DEFAULT AS Modelo, '9 mm' AS Calibre FROM Personal.dbo.tblPolicias p WHERE p.marcaArma IS NOT NULL AND p.modeloArma IS NOT NULL ) INSERT INTO dbo.Arma (Marca,Modelo,Calibre) SELECT s.Marca,s.Modelo,s.Calibre FROM SrcPoli s WHERE NOT EXISTS (SELECT 1 FROM dbo.Arma d WHERE d.Marca = s.Marca AND d.Modelo = s.Modelo);
    ;WITH Datos AS ( SELECT RIGHT('00000000' + CAST(p.num_cedula AS varchar(8)),8) AS CI, TRY_CAST(p.cargadoresArma AS INT) AS Cargadores, TRY_CAST(p.municionesArma AS INT) AS Municiones, p.observacionesArma AS Observaciones, p.serieArma AS Serie, CAST(LTRIM(RTRIM(p.marcaArma)) AS NVARCHAR(50)) COLLATE DATABASE_DEFAULT AS Marca, CAST(LTRIM(RTRIM(p.modeloArma)) AS NVARCHAR(100)) COLLATE DATABASE_DEFAULT AS Modelo FROM Personal.dbo.tblPolicias p WHERE p.marcaArma IS NOT NULL AND p.modeloArma IS NOT NULL ) INSERT INTO dbo.FuncionarioArma (FuncionarioId,Cargadores,Municiones,Observaciones,FechaAsign,ArmaId,Serie) SELECT f.Id,d.Cargadores,d.Municiones,d.Observaciones,SYSUTCDATETIME(),a.Id,d.Serie FROM Datos d INNER JOIN dbo.Funcionario f ON f.CI = d.CI INNER JOIN dbo.Arma a ON a.Marca = d.Marca AND a.Modelo = d.Modelo LEFT JOIN dbo.FuncionarioArma fa ON fa.FuncionarioId = f.Id AND fa.ArmaId = a.Id WHERE fa.Id IS NULL;

    ;WITH CustodiasUnicas AS ( SELECT hc.id_policia,hc.fecha,MIN(hc.area) AS area FROM Personal.dbo.tblHistoricoCustodias hc GROUP BY hc.id_policia,hc.fecha ) INSERT INTO dbo.HistoricoCustodia (FuncionarioId,Fecha,Area) SELECT m.FuncionarioId,cu.fecha,cu.area FROM CustodiasUnicas cu JOIN dbo.MapPoliciaFunc m ON m.id_policia = cu.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoCustodia hc WHERE hc.FuncionarioId = m.FuncionarioId AND hc.Fecha = cu.fecha);
    ;WITH NoctAgreg AS ( SELECT hn.id_policia,hn.año,hn.mes,SUM(hn.minutos) AS minutos FROM Personal.dbo.tblHistoricoNocturnidad hn GROUP BY hn.id_policia,hn.año,hn.mes) INSERT INTO dbo.HistoricoNocturnidad (FuncionarioId,Anio,Mes,Minutos) SELECT m.FuncionarioId,na.año,na.mes,na.minutos FROM NoctAgreg na JOIN dbo.MapPoliciaFunc m ON m.id_policia = na.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoNocturnidad hn WHERE hn.FuncionarioId = m.FuncionarioId AND hn.Anio = na.año AND hn.Mes = na.mes);
    ;WITH PresAgreg AS ( SELECT hp.id_policia,hp.año,hp.mes,SUM(hp.minutos) AS minutos,SUM(hp.dias) AS dias, MIN(hp.incidencia) AS incidencia,MIN(hp.observaciones) AS observaciones FROM Personal.dbo.tblHistoricoPresentismo hp GROUP BY hp.id_policia,hp.año,hp.mes ) INSERT INTO dbo.HistoricoPresentismo (FuncionarioId,Anio,Mes,Minutos,Dias,Incidencia,Observaciones) SELECT m.FuncionarioId,pa.año,pa.mes,pa.minutos,pa.dias,pa.incidencia,pa.observaciones FROM PresAgreg pa JOIN dbo.MapPoliciaFunc m ON m.id_policia = pa.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoPresentismo hp WHERE hp.FuncionarioId = m.FuncionarioId AND hp.Anio = pa.año AND hp.Mes = pa.mes);
    ;WITH ViatAgreg AS ( SELECT hv.id_policia,hv.año,hv.mes,MIN(hv.incidencia) AS incidencia,MIN(hv.motivo) AS motivo FROM Personal.dbo.tblHistoricoViaticos hv GROUP BY hv.id_policia,hv.año,hv.mes) INSERT INTO dbo.HistoricoViatico (FuncionarioId,Anio,Mes,Incidencia,Motivo) SELECT m.FuncionarioId,va.año,va.mes,va.incidencia,va.motivo FROM ViatAgreg va JOIN dbo.MapPoliciaFunc m ON m.id_policia = va.id_policia WHERE NOT EXISTS (SELECT 1 FROM dbo.HistoricoViatico hv WHERE hv.FuncionarioId = m.FuncionarioId AND hv.Anio = va.año AND hv.Mes = va.mes);

    ;WITH Src AS ( SELECT u.id_usuario AS OrigId, LOWER(LTRIM(RTRIM(u.usuario))) COLLATE DATABASE_DEFAULT AS UserName, HASHBYTES('SHA2_256',CAST(u.clave AS NVARCHAR(4000))) AS PwdHash, mp.FuncionarioId FROM Personal.dbo.tblUsuarios u LEFT JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = u.id_policia) MERGE dbo.Usuario AS tgt USING Src AS src ON tgt.UserName = src.UserName WHEN MATCHED THEN UPDATE SET tgt.FuncionarioId = src.FuncionarioId, tgt.PasswordHash = src.PwdHash, tgt.UpdatedAt = SYSUTCDATETIME() WHEN NOT MATCHED BY TARGET THEN INSERT (UserName,PasswordHash,FuncionarioId,RolId,CreatedAt) VALUES(src.UserName,src.PwdHash,src.FuncionarioId,2,SYSUTCDATETIME());

    PRINT 'Paso 3: Tablas dependientes migradas.';

    /*================================================================
      PASO 4: Migrar Fotos de Funcionarios
    =================================================================*/
    PRINT 'Paso 4: Migrando fotos de funcionarios...';

    MERGE dbo.FuncionarioFotoHistorico AS tgt
    USING ( SELECT p.id_picture AS PictureId, mp.FuncionarioId, CAST(p.Data AS VARBINARY(MAX)) AS Foto, p.FileName FROM Personal.dbo.tblPictures p JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = p.id_policia ) AS src
      ON tgt.PictureId = src.PictureId
    WHEN NOT MATCHED BY TARGET THEN INSERT (FuncionarioId, PictureId, Foto, FileName) VALUES (src.FuncionarioId, src.PictureId, src.Foto, src.FileName);

    ;WITH UltimaFoto AS ( SELECT ffh.FuncionarioId, ffh.Foto, ROW_NUMBER() OVER(PARTITION BY ffh.FuncionarioId ORDER BY ffh.PictureId DESC) as rn FROM dbo.FuncionarioFotoHistorico ffh )
    UPDATE f SET f.Foto = uf.Foto
    FROM dbo.Funcionario f
    JOIN UltimaFoto uf ON f.Id = uf.FuncionarioId
    WHERE uf.rn = 1;

    PRINT 'Paso 4: Fotos de funcionarios migradas.';

    PRINT '--- MIGRACIÓN COMPLETA FINALIZADA CON ÉXITO ---';
    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    PRINT '!!! ERROR DURANTE LA MIGRACIÓN !!!';
    THROW;
END CATCH;
GO