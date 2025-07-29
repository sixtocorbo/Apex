/*
===============================================================================
  SCRIPT DE MIGRACI�N COMPLETO: Personal -> Apex
  Autor:        Equipo de migraci�n
  Fecha:        26/07/2025
  Descripci�n: Migra todos los datos desde la base 'Personal' hacia 'Apex'.
                Incluye limpieza preventiva, poblaci�n de cat�logos, datos
                dependientes y migraci�n de fotos de funcionarios.

  Notas:
  - Este script est� dise�ado para ejecutarse en una nueva conexi�n hacia
    la instancia de SQL Server para evitar estados residuales de IDENTITY_INSERT.
  - Cada vez que se activa IDENTITY_INSERT para una tabla, se desactiva
    inmediatamente antes de activar la siguiente.
  - [CORRECCI�N]: Se ha a�adido el PASO 4 para migrar las fotos.
===============================================================================
*/

USE [Apex];
GO

-- Se asume que este procedimiento existe y limpia la BD de destino.
	exec usp_LimpiarDatosDeApex

BEGIN TRY
    /*
      Limpieza preventiva: nos aseguramos de que ninguna tabla tenga
      IDENTITY_INSERT activo al iniciar la migraci�n.
    */

    -- TipoFuncionario: insertar al menos un tipo para los funcionarios migrados
    SET IDENTITY_INSERT dbo.TipoFuncionario ON;
    MERGE dbo.TipoFuncionario AS tgt
    USING (VALUES (1,'Funcionario')) AS src(Id,Nombre)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
      INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.TipoFuncionario OFF;

    -- RolUsuario
    MERGE dbo.RolUsuario AS tgt
    USING (VALUES 
        (1,'Admin',1),
        (2,'Usuario',2)
    ) AS src(Id,Nombre,Orden)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
      INSERT (Id,Nombre,Orden) VALUES (src.Id,src.Nombre,src.Orden);

    PRINT 'Asegurando que IDENTITY_INSERT est� desactivado para todas las tablas...';
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

    /*
      Iniciamos una transacci�n para asegurar que todas las etapas de la
      migraci�n se ejecuten de forma at�mica.
    */
    BEGIN TRANSACTION;

    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    PRINT '--- INICIO DE LA MIGRACI�N COMPLETA ---';

    /*================================================================
      PASO 1: Poblar Cat�logos
      Se insertan registros de tablas maestras con sus IDs originales.
    =================================================================*/
    PRINT 'Paso 1: Poblando cat�logos...';

    -- CategoriaAusencia
    SET IDENTITY_INSERT dbo.CategoriaAusencia ON;
    MERGE dbo.CategoriaAusencia AS tgt
    USING (VALUES (1,'General'),(2,'M�dica'),(3,'Especial')) AS src(Id,Nombre)
        ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
        INSERT (Id,Nombre) VALUES(src.Id,src.Nombre);
    SET IDENTITY_INSERT dbo.CategoriaAusencia OFF;

    -- Cargo
    SET IDENTITY_INSERT dbo.Cargo ON;
    MERGE dbo.Cargo AS tgt
    USING (VALUES
        (1,'AGENTE',1), (2,'CABO',2), (3,'SARGENTO',3), (4,'SUB OFICIAL MAYOR',4),
        (5,'OFICIAL AYUDANTE',5), (6,'OFICIAL PRINCIPAL',6), (7,'SUB COMISARIO',7),
        (8,'COMISARIO',8), (9,'COMISARIO MAYOR',9), (10,'COMISARIO GENERAL',10),
        (14,'LICENCIADO EN PSICOLOGIA (9)',9), (15,'ASISTENTE SOCIAL',10),
        (16,'OP.PEN. G II',2), (17,'OP.PEN. G III',3), (18,'OP.PEN. G I',1),
        (19,'ALCAIDE',7), (20,'LIC. TRABAJO SOCIAL',8), (21,'EDUCADOR SOCIAL (10)',10),
        (22,'PROFESOR EDUCACION FISICA',9), (23,'GUARDIA',1), (24,'EDUCADOR SOCIAL (7)',7),
        (25,'OP. PEN. G IV',4), (26,'SUPERVISOR PENITENCIARIO',5), (27,'SUB ALCAIDE',6),
        (28,'ALCAIDE MAYOR',8), (29,'ESC B (4)',4), (30,'PA 2',2), (31,'PA 3',3),
        (32,'PA 6',6), (33,'PA 8',8), (34,'PE 3',3), (35,'PE 6',6), (36,'PE 8',8),
        (37,'PE 9',9), (38,'VERIFICAR',10), (39,'LICENCIADO EN PSICOLOGIA (11)',11),
        (1039,'ESC. A PROFESIONAL',8), (1040,'ESC B (9)',9), (1041,'ESC. B',8),
        (1042,'ESC. C',5), (1043,'SUB PREFECTO',9), (1044,'PREFECTO',10)
    ) AS src(Id,Nombre,Grado)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
      INSERT (Id,Nombre,Grado,CreatedAt) VALUES(src.Id,src.Nombre,src.Grado,GETDATE());
    SET IDENTITY_INSERT dbo.Cargo OFF;

    -- Estado
    SET IDENTITY_INSERT dbo.Estado ON;
    MERGE dbo.Estado AS tgt
    USING (VALUES (1,'SI',0),(2,'NO',0),(6,'AU',1),(1005,'O5',1)) AS src(Id,Nombre,EsAusentismo)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
      INSERT (Id,Nombre,EsAusentismo,CreatedAt)
      VALUES (src.Id,src.Nombre,src.EsAusentismo,GETDATE());
    SET IDENTITY_INSERT dbo.Estado OFF;

    -- Secci�n
    SET IDENTITY_INSERT dbo.Seccion ON;
    MERGE dbo.Seccion AS tgt
    USING (VALUES (1,'4'),(2,'4A'),(3,'4B'),(4,'4C'),(5,'4D'),(6,'4E'),(7,'VERIFICAR'),(8,'4JF'),(9,'4CA'),(10,'4CT')) AS src(Id,Nombre)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
      INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Seccion OFF;

    -- Puesto de trabajo
    SET IDENTITY_INSERT dbo.PuestoTrabajo ON;
    MERGE dbo.PuestoTrabajo AS tgt
    USING (VALUES
        (1,'MODULO 6'),(2,'MODULO 7'),(3,'MODULO 9'),(4,'OFICINA OPERATIVA'),(5,'OFICINA ADMINISTRATIVA'),
        (6,'OFICINA TECNICA'),(7,'JURIDICA'),(8,'GUARDIA'),(27,'OBRA'),(28,'CONTROL INGRESOS'),
        (29,'PERSONAL'),(30,'JEFE DE SERVICIO'),(31,'EDUCACION'),(32,'ECONOMATO'),(33,'HERRERIA'),
        (34,'SECRETARIA GENERAL'),(35,'LABORAL'),(36,'DIRECTOR'),(37,'ASSE'),(38,'MODULO 10'),
        (39,'MODULO 11'),(40,'MODULO 4'),(41,'MODULO 5'),(42,'MODULO 2'),(43,'MODULO 3'),
        (44,'MODULO 8'),(45,'MODULO 12'),(1038,'SUB DIRECTOR TECNICO'),(1039,'SUB DIRECTOR OPERATIVO'),
        (1040,'SUB DIRECTOR ADMINISTRATIVO'),(1041,'JUNTA MEDICA'),(1042,'LICENCIA MEDICA'),
        (1043,'EXTRAMUROS'),(1044,'VERIFICAR'),(1045,'CONYUGALES'),(1046,'PORTON PRINCIPAL'),
        (1047,'PORTON 22'),(1048,'REVISORIA'),(1049,'JEFATURA DE SERVICIO'),(1050,'LOCUTORIO DE ABOGADOS'),
        (1051,'ENCOMIENDAS'),(1052,'COCINA C.G.E.'),(1053,'SUB DIRECCION OPERATIVA'),(1054,'O.C.T'),
        (1055,'SUB DIRECCION TECNICA'),(1056,'COORDINACION TECNICA'),
        (2056,'SUB DIRECCION ADMINISTRATIVA (SECRETARIA)'),(2057,'TESORERIA Y VALORES'),
        (2058,'CENTRO MOTOR - SANITARIO-'),(2059,'CENTRO MOTOR - HERRERIA -'),
        (2060,'CENTRO MOTOR - ELECTRICISTA -'),(2061,'ARMERIA'),
        (3056,'OGLAST'),(3057,'REDENCION DE PENA'),(3058,'ARCHIVO GENERAL DEL COMPLEJO'),
        (3059,'DEPORTE Y RECREACI�N'),(3060,'�REA DE TRATAMIENTO'),(3061,'COORDINACION OPERATIVA'),
        (3062,'COORDINACION GENERAL'),(3063,'COORDINACION ADMINISTRATIVA'),
        (3064,'IGLESIA'),(3065,'SUB DIRECCION ADMINISTRATIVA'),
        (4064,'ENCARGADO DE DESPACHO DE UNIDAD 4B'),(4065,'ENCARGADO DE DESPACHO SDO'),
        (4066,'CENTRO MOTOR - INFORMATICO'),(4067,'ENCARGADA CASINO SSOO'),(4068,'CHOFER'),
        (4069,'Simagin'),(4070,'AMBOS'),(4071,'CASINO SSOO'),(4072,'COORDINACION DE RECLUSION'),
        (4073,'MODULO 1'),(4074,'EVALUACION Y TRATAMIENTO'),
        (5072,'COORDINADOR GENERAL'),(5073,'COORDINADOR ADMINISTRATIVO'),
        (5074,'COORDINADOR OPERATIVO'),(5075,'COORDINADOR T�CNICO'),(5076,'COORDINADOR DE RECLUSI�N'),
        (5077,'GESTORIA DE AREAS VERDES'),(5078,'REFERENTE DE SALUD'),
        (6078,'4CT'),(6079,'ANEXO - UTU'),(7079,'SALA DE MONITOREO')
    ) AS src(Id,Nombre)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
      INSERT (Id,Nombre,CreatedAt,Activo)
      VALUES(src.Id,src.Nombre,GETDATE(),1);
    SET IDENTITY_INSERT dbo.PuestoTrabajo OFF;

    -- Turno
    SET IDENTITY_INSERT dbo.Turno ON;
    MERGE dbo.Turno AS tgt
    USING (VALUES (1,'DIURNO'),(2,'NOCTURNO'),(3,'VERIFICAR')) AS src(Id,Nombre)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Turno OFF;

    -- Semana
    SET IDENTITY_INSERT dbo.Semana ON;
    MERGE dbo.Semana AS tgt
    USING (VALUES
        (1,'SEMANA 1'),(2,'SEMANA 2'),(3,'LMMJV'),(4,'12 X 36'),(5,'24 X 48'),(6,'LMMJVS'),
        (7,'MMJVS'),(8,'DLMMJV'),(10,'DLMMJ'),(11,'5 X 2'),(12,'AMBAS'),(13,'FULL'),
        (14,'LMV'),(15,'MJVSD'),(16,'VERIFICAR')
    ) AS src(Id,Nombre)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Semana OFF;

    -- Horario
    SET IDENTITY_INSERT dbo.Horario ON;
    MERGE dbo.Horario AS tgt
    USING (VALUES
        (1,'07_19'),(2,'19_07'),(3,'09_15'),(4,'08_15'),(5,'07_15'),(6,'22_10'),
        (7,'09_16'),(8,'08_16'),(9,'ROTATIVO'),(10,'FULL'),(11,'MATERNAL'),
        (12,'00_06'),(13,'00_08'),(14,'23_06'),(15,'08_14'),(16,'06_12'),(17,'12_18'),
        (18,'12_20'),(19,'22_06'),(21,'1830_0630'),(22,'07_14'),(23,'07_13'),
        (24,'11_17'),(25,'23_07'),(26,'15_23'),(27,'06_14'),(28,'0930_1730'),(29,'13_21'),
        (30,'10_18'),(31,'06_18'),(32,'0630_1430'),(33,'0730_1530'),(34,'0750_1550'),
        (35,'07_1412'),(36,'07_17'),(37,'08_1515'),(38,'0815_1527'),(39,'0830_1630'),
        (40,'09_17'),(41,'10_16'),(42,'11_15'),(43,'11_19'),(44,'12_16'),(45,'14_22'),
        (46,'18_06'),(48,'FLEXIBLE DIARIO'),(49,'PRESENCIAL'),(50,'VERIFICAR'),
        (51,'L,MI,V 09. MA,J 10'),(52,'1030_1830'),(53,'08_20'),(54,'09_13'),
        (55,'07_1730'),(56,'08_1512'),(57,'08_12'),(58,'24 HS')
    ) AS src(Id,Nombre)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre,CreatedAt) VALUES(src.Id,src.Nombre,GETDATE());
    SET IDENTITY_INSERT dbo.Horario OFF;

    -- TipoLicencia
    SET IDENTITY_INSERT dbo.TipoLicencia ON;
    MERGE dbo.TipoLicencia AS tgt
    USING (
        SELECT id_ausencia, nom_ausencia,
                CASE WHEN ausente     = 'SI' THEN 1 ELSE 0 END AS EsAusencia,
                CASE WHEN viatico     = 'NO' THEN 1 ELSE 0 END AS SuspendeViatico,
                CASE WHEN presentismo = 'NO' THEN 1 ELSE 0 END AS AfectaPresentismo,
                CASE WHEN habil       = 'SI' THEN 1 ELSE 0 END AS EsHabil
          FROM Personal.dbo.tblAusencias
    ) AS src(Id,Nombre,EsAusencia,SuspendeViatico,AfectaPresentismo,EsHabil)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
      INSERT (Id,Nombre,EsAusencia,SuspendeViatico,AfectaPresentismo,EsHabil,CreatedAt,CategoriaAusenciaId)
      VALUES(src.Id,src.Nombre,src.EsAusencia,src.SuspendeViatico,src.AfectaPresentismo,src.EsHabil,GETDATE(),1);
    SET IDENTITY_INSERT dbo.TipoLicencia OFF;

    -- G�nero
    SET IDENTITY_INSERT dbo.Genero ON;
    MERGE dbo.Genero AS tgt
    USING (VALUES (1,'Masculino'),(2,'Femenino'),(3,'Otro'),(4,'No especifica')) AS src(Id,Nombre)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre)
    WHEN MATCHED AND tgt.Nombre <> src.Nombre THEN UPDATE SET Nombre = src.Nombre;
    SET IDENTITY_INSERT dbo.Genero OFF;

    -- Estado Civil
    SET IDENTITY_INSERT dbo.EstadoCivil ON;
    MERGE dbo.EstadoCivil AS tgt
    USING (VALUES (1,'Soltero/a'),(2,'Casado/a'),(3,'Divorciado/a'),(4,'Viudo/a'),(5,'Uni�n libre'),(6,'Separado/a'),(7,'No especifica')) AS src(Id,Nombre)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre)
    WHEN MATCHED AND tgt.Nombre <> src.Nombre THEN UPDATE SET Nombre = src.Nombre;
    SET IDENTITY_INSERT dbo.EstadoCivil OFF;

    -- Nivel de estudio
    SET IDENTITY_INSERT dbo.NivelEstudio ON;
    MERGE dbo.NivelEstudio AS tgt
    USING (VALUES
        (1,'Sin estudios'),(2,'Primaria incompleta'),(3,'Primaria completa'),(4,'Secundaria incompleta'),
        (5,'Secundaria completa'),(6,'T�cnico terciario'),(7,'Universitario incompleto'),
        (8,'Universitario completo'),(9,'Postgrado'),(10,'Otro')
    ) AS src(Id,Nombre)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN INSERT (Id,Nombre) VALUES(src.Id,src.Nombre)
    WHEN MATCHED AND tgt.Nombre <> src.Nombre THEN UPDATE SET Nombre = src.Nombre;
    SET IDENTITY_INSERT dbo.NivelEstudio OFF;

    -- Tipo de Estado Transitorio
    SET IDENTITY_INSERT dbo.TipoEstadoTransitorio ON;
    MERGE dbo.TipoEstadoTransitorio AS tgt
    USING (VALUES (1,'Designaci�n',0),(2,'Enfermedad',0),(3,'Sanci�n',0),(4,'Orden Cinco',0),(5,'Ret�n',0),(6,'Sumario',0)) AS src(Id,Nombre,EsJerarquico)
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
      INSERT (Id,Nombre,EsJerarquico,CreatedAt) VALUES(src.Id,src.Nombre,src.EsJerarquico,SYSUTCDATETIME())
    WHEN MATCHED AND tgt.Nombre <> src.Nombre THEN
      UPDATE SET Nombre = src.Nombre, UpdatedAt = SYSUTCDATETIME();
    SET IDENTITY_INSERT dbo.TipoEstadoTransitorio OFF;

    -- Escalaf�n y Funci�n
    MERGE dbo.Escalafon AS tgt
    USING (
        SELECT DISTINCT LTRIM(RTRIM(p.escalafon)) COLLATE Modern_Spanish_CI_AS AS Nombre
          FROM Personal.dbo.tblPolicias p
         WHERE p.escalafon IS NOT NULL AND LTRIM(RTRIM(p.escalafon)) <> ''
    ) AS src
      ON tgt.Nombre = src.Nombre
    WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (src.Nombre);

    MERGE dbo.Funcion AS tgt
    USING (
        SELECT DISTINCT LTRIM(RTRIM(p.funcion)) COLLATE Modern_Spanish_CI_AS AS Nombre
          FROM Personal.dbo.tblPolicias p
         WHERE p.funcion IS NOT NULL AND LTRIM(RTRIM(p.funcion)) <> ''
    ) AS src
      ON tgt.Nombre = src.Nombre
    WHEN NOT MATCHED THEN INSERT (Nombre) VALUES (src.Nombre);

    PRINT 'Paso 1: Cat�logos completados.';

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
    INSERT INTO #MapeoEstadoCivil VALUES
        ('CASADO',2),('CASADO/A',2),('COMPROMETIDO/A',5),('DIVORCIADO',3),('DIVORCIADO/A',3),
        ('SOLTERO',1),('SOLTERO/A',1),('',7);
    INSERT INTO #MapeoNivelEstudio VALUES
        ('CICLO B�SICO COMPLETO',5),('CICLO B�SICO INCOMPLETO',4),('EDUCACI�N PRIMARIA',3),
        ('EDUCACION SECUNDARIA',5),('EDUCACI�N TERCIARIA',6),('PROFESIONAL',8),('',10);
    INSERT INTO #MapeoEstado VALUES (1,1),(2,2),(6,6),(1005,1005);

    ;WITH PoliciasStg AS (
        SELECT p.id_policia AS IdPolicia,
               RIGHT('00000000' + CAST(p.num_cedula AS VARCHAR(8)),8) COLLATE DATABASE_DEFAULT AS CI,
               LTRIM(RTRIM(p.nom_policia)) AS Nombre,
               p.alta AS FechaIngreso,
               CASE WHEN p.id_estado = 1 THEN 1 ELSE 0 END AS Activo,
               p.id_estado AS EstadoId_Origen,
               p.id_seccion AS SeccionId,
               p.id_puesto AS PuestoTrabajoId,
               p.id_turno AS TurnoId,
               p.id_semana AS SemanaId,
               p.id_horario AS HorarioId,
               UPPER(LTRIM(RTRIM(p.genero))) COLLATE DATABASE_DEFAULT       AS GeneroTxt,
               UPPER(LTRIM(RTRIM(p.nivelestudio))) COLLATE DATABASE_DEFAULT AS NivelEstudioTxt,
               UPPER(LTRIM(RTRIM(p.estadocivil))) COLLATE DATABASE_DEFAULT  AS EstadoCivilTxt,
               p.id_grado AS CargoId,
               p.fecha_registro AS CreatedAt,
               p.fecha_actualizado AS UpdatedAt,
               p.fecha_nacimiento AS FechaNacimiento,
               LTRIM(RTRIM(p.domicilio)) AS Domicilio,
               LTRIM(RTRIM(p.correo))    AS Email,
               LTRIM(RTRIM(p.escalafon)) AS EscalafonTxt,
               LTRIM(RTRIM(p.funcion))   AS FuncionTxt
          FROM Personal.dbo.tblPolicias p
    ),
    PoliciasMapeadas AS (
        SELECT
            s.CI, s.Nombre, s.FechaIngreso, s.Activo,
            meE.IdDestino                    AS EstadoId,
            s.SeccionId, s.PuestoTrabajoId, s.TurnoId, s.SemanaId, s.HorarioId,
            COALESCE(meG.IdDestino,4)        AS GeneroId,
            COALESCE(meN.IdDestino,10)       AS NivelEstudioId,
            COALESCE(meC.IdDestino,7)        AS EstadoCivilId,
            s.CargoId, s.CreatedAt, s.UpdatedAt, s.FechaNacimiento, s.Domicilio, s.Email,
            e.Id AS EscalafonId,
            f.Id AS FuncionId
        FROM PoliciasStg s
        JOIN #MapeoEstado         meE ON meE.IdOrigen = s.EstadoId_Origen
        LEFT JOIN #MapeoGenero       meG ON meG.TxtOrigen = COALESCE(s.GeneroTxt,'')
        LEFT JOIN #MapeoNivelEstudio meN ON meN.TxtOrigen = COALESCE(s.NivelEstudioTxt,'')
        LEFT JOIN #MapeoEstadoCivil  meC ON meC.TxtOrigen = COALESCE(s.EstadoCivilTxt,'')
        LEFT JOIN dbo.Escalafon      e   ON e.Nombre = s.EscalafonTxt COLLATE Modern_Spanish_CI_AS
        LEFT JOIN dbo.Funcion        f   ON f.Nombre  = s.FuncionTxt   COLLATE Modern_Spanish_CI_AS
    )
    MERGE dbo.Funcionario AS tgt
    USING PoliciasMapeadas AS src
      ON tgt.CI = src.CI COLLATE DATABASE_DEFAULT
    WHEN MATCHED THEN
      UPDATE SET
        tgt.Nombre          = src.Nombre,
        tgt.FechaIngreso    = src.FechaIngreso,
        tgt.Activo          = src.Activo,
        tgt.EstadoId        = src.EstadoId,
        tgt.SeccionId       = src.SeccionId,
        tgt.PuestoTrabajoId = src.PuestoTrabajoId,
        tgt.TurnoId         = src.TurnoId,
        tgt.SemanaId        = src.SemanaId,
        tgt.HorarioId       = src.HorarioId,
        tgt.GeneroId        = src.GeneroId,
        tgt.NivelEstudioId  = src.NivelEstudioId,
        tgt.EstadoCivilId   = src.EstadoCivilId,
        tgt.CargoId         = src.CargoId,
        tgt.FechaNacimiento = src.FechaNacimiento,
        tgt.Domicilio       = src.Domicilio,
        tgt.Email           = src.Email,
        tgt.EscalafonId     = src.EscalafonId,
        tgt.FuncionId       = src.FuncionId,
        tgt.UpdatedAt       = SYSUTCDATETIME()
    WHEN NOT MATCHED BY TARGET THEN
      INSERT (CI,Nombre,FechaIngreso,Activo,CreatedAt,UpdatedAt,EstadoId,SeccionId,
              PuestoTrabajoId,TurnoId,SemanaId,HorarioId,GeneroId,NivelEstudioId,
              EstadoCivilId,CargoId,FechaNacimiento,Domicilio,Email,EscalafonId,FuncionId,TipoFuncionarioId)
      VALUES (src.CI,src.Nombre,src.FechaIngreso,src.Activo,src.CreatedAt,src.UpdatedAt,
              src.EstadoId,src.SeccionId,src.PuestoTrabajoId,src.TurnoId,src.SemanaId,
              src.HorarioId,src.GeneroId,src.NivelEstudioId,src.EstadoCivilId,src.CargoId,
              src.FechaNacimiento,src.Domicilio,src.Email,src.EscalafonId,src.FuncionId,1);

    TRUNCATE TABLE dbo.MapPoliciaFunc;
    INSERT INTO dbo.MapPoliciaFunc (id_policia,FuncionarioId)
    SELECT p.id_policia,f.Id
      FROM Personal.dbo.tblPolicias p
      JOIN dbo.Funcionario f ON f.CI = RIGHT('00000000' + CAST(p.num_cedula AS VARCHAR(8)),8);

    PRINT 'Paso 2: Funcionarios migrados.';

    /*================================================================
      PASO 3: Migrar tablas dependientes
    =================================================================*/
    -- 3.1 Estados Transitorios
    PRINT 'Paso 3.1: Migrando estados transitorios...';
    SET IDENTITY_INSERT dbo.EstadoTransitorio ON;
    ;WITH Detalles AS (
        SELECT u.id_estado,u.id_policia,u.categoria COLLATE Modern_Spanish_CI_AS AS categoria,
               d.descripcion,d.fecha_inicio,d.fecha_fin
          FROM Personal.dbo.tblEstadosUnificados u
          LEFT JOIN Personal.dbo.tblDesignacionesDetalle d ON d.id_estado = u.id_estado AND u.categoria = 'D'
        UNION ALL
        SELECT u.id_estado,u.id_policia,u.categoria COLLATE Modern_Spanish_CI_AS AS categoria,
               e.descripcion,e.fecha_inicio,e.fecha_fin
          FROM Personal.dbo.tblEstadosUnificados u
          LEFT JOIN Personal.dbo.tblEnfermedadDetalle e ON e.id_estado = u.id_estado AND u.categoria = 'E'
        UNION ALL
        SELECT u.id_estado,u.id_policia,u.categoria COLLATE Modern_Spanish_CI_AS AS categoria,
               s.descripcion,s.fecha_inicio,s.fecha_fin
          FROM Personal.dbo.tblEstadosUnificados u
          LEFT JOIN Personal.dbo.tblSancionesDetalle s ON s.id_estado = u.id_estado AND u.categoria = 'N'
        UNION ALL
        SELECT u.id_estado,u.id_policia,u.categoria COLLATE Modern_Spanish_CI_AS AS categoria,
               o.descripcion,o.fecha_inicio,o.fecha_fin
          FROM Personal.dbo.tblEstadosUnificados u
          LEFT JOIN Personal.dbo.tblOrdenCincoDetalle o ON o.id_estado = u.id_estado AND u.categoria = 'O'
        UNION ALL
        SELECT u.id_estado,u.id_policia,u.categoria COLLATE Modern_Spanish_CI_AS AS categoria,
               r.descripcion,r.fecha_reten AS fecha_inicio,NULL AS fecha_fin
          FROM Personal.dbo.tblEstadosUnificados u
          LEFT JOIN Personal.dbo.tblRetenesDetalle r ON r.id_estado = u.id_estado AND u.categoria = 'R'
        UNION ALL
        SELECT u.id_estado,u.id_policia,u.categoria COLLATE Modern_Spanish_CI_AS AS categoria,
               su.descripcion,su.fecha_inicio,su.fecha_fin
          FROM Personal.dbo.tblEstadosUnificados u
          LEFT JOIN Personal.dbo.tblSumariosDetalle su ON su.id_estado = u.id_estado AND u.categoria = 'S'
    ), Prim AS (
        SELECT d.*, ROW_NUMBER() OVER (PARTITION BY d.id_estado ORDER BY d.fecha_inicio) AS rn
          FROM Detalles d
    )
    MERGE dbo.EstadoTransitorio AS tgt
    USING (
        SELECT p.id_estado AS Id,
               mp.FuncionarioId,
               CASE p.categoria WHEN 'D' THEN 1 WHEN 'E' THEN 2 WHEN 'N' THEN 3
                                 WHEN 'O' THEN 4 WHEN 'R' THEN 5 WHEN 'S' THEN 6 END AS TipoEstadoTransitorioId,
               CAST(COALESCE(p.fecha_inicio,SYSUTCDATETIME()) AS date) AS FechaDesde,
               p.fecha_fin AS FechaHasta,
               p.descripcion AS Observaciones
          FROM Prim p
          JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = p.id_policia
         WHERE p.rn = 1
    ) AS src
      ON tgt.Id = src.Id
    WHEN NOT MATCHED THEN
      INSERT (Id,FuncionarioId,TipoEstadoTransitorioId,FechaDesde,FechaHasta,Observaciones,CreatedAt)
      VALUES(src.Id,src.FuncionarioId,src.TipoEstadoTransitorioId,src.FechaDesde,src.FechaHasta,src.Observaciones,SYSUTCDATETIME());
    SET IDENTITY_INSERT dbo.EstadoTransitorio OFF;
    PRINT 'Paso 3.1: Estados transitorios migrados.';

    -- 3.2 Historial de licencias
    PRINT 'Paso 3.2: Migrando historial de licencias...';
    SET IDENTITY_INSERT dbo.HistoricoLicencia ON;
    MERGE dbo.HistoricoLicencia AS TGT
    USING (
        SELECT l.id_licencia, mp.FuncionarioId, l.id_ausencia AS TipoLicenciaId,
               l.inicio, l.finaliza, l.fecha_registro, l.fecha_actualizado, l.usuario,
               l.datos, l.estado, l.Unidad_Ejecutora, l.Unidad_Organizativa, l.Cantidad,
               l.Cantidad_dentro_del_per�odo, l.Unidad, l.Afecta_a_d�as, l.C_Present�_certificado_,
               l.Usuario_aprob�_anul�_rechaz�, l.Fecha_aprobaci�n_anulaci�n_rechazo, l.Comentario
          FROM Personal.dbo.tblLicencias AS l
          JOIN dbo.MapPoliciaFunc AS mp ON l.id_policia = mp.id_policia
    ) AS SRC
      ON TGT.Id = SRC.id_licencia
    WHEN NOT MATCHED BY TARGET THEN
      INSERT (Id,FuncionarioId,TipoLicenciaId,inicio,finaliza,fecha_registro,fecha_actualizado,usuario,
              datos,estado,Unidad_Ejecutora,Unidad_Organizativa,Cantidad,Cantidad_dentro_del_per�odo,
              Unidad,Afecta_a_d�as,C_Present�_certificado_,Usuario_aprob�_anul�_rechaz�,
              Fecha_aprobaci�n_anulaci�n_rechazo,Comentario)
      VALUES(SRC.id_licencia,SRC.FuncionarioId,SRC.TipoLicenciaId,SRC.inicio,SRC.finaliza,
             SRC.fecha_registro,SRC.fecha_actualizado,SRC.usuario,SRC.datos,SRC.estado,SRC.Unidad_Ejecutora,
             SRC.Unidad_Organizativa,SRC.Cantidad,SRC.Cantidad_dentro_del_per�odo,SRC.Unidad,
             SRC.Afecta_a_d�as,SRC.C_Present�_certificado_,SRC.Usuario_aprob�_anul�_rechaz�,
             SRC.Fecha_aprobaci�n_anulaci�n_rechazo,SRC.Comentario);
    SET IDENTITY_INSERT dbo.HistoricoLicencia OFF;
    PRINT 'Paso 3.2: Historial de licencias migrado.';

    -- 3.3 Armas y asignaciones
    PRINT 'Paso 3.3: Migrando armas y asignaciones...';
    ;WITH SrcArmas AS (
        SELECT DISTINCT
            CAST(LTRIM(RTRIM(a.marca))  AS NVARCHAR(50)) COLLATE Modern_Spanish_CI_AS AS Marca,
            CAST(LTRIM(RTRIM(a.modelo)) AS NVARCHAR(100)) COLLATE Modern_Spanish_CI_AS AS Modelo,
            CAST(LTRIM(RTRIM(a.calibre))AS NVARCHAR(50)) COLLATE Modern_Spanish_CI_AS AS Calibre
          FROM Personal.dbo.tblArmas a
         WHERE a.marca IS NOT NULL AND a.modelo IS NOT NULL
    )
    INSERT INTO dbo.Arma (Marca,Modelo,Calibre)
    SELECT s.Marca,s.Modelo,s.Calibre
      FROM SrcArmas s
     WHERE NOT EXISTS (SELECT 1 FROM dbo.Arma d WHERE d.Marca = s.Marca AND d.Modelo = s.Modelo);

    ;WITH SrcPoli AS (
        SELECT DISTINCT
            CAST(LTRIM(RTRIM(p.marcaArma))  AS NVARCHAR(50))  COLLATE Modern_Spanish_CI_AS AS Marca,
            CAST(LTRIM(RTRIM(p.modeloArma)) AS NVARCHAR(100)) COLLATE Modern_Spanish_CI_AS AS Modelo,
            '9 mm' AS Calibre
          FROM Personal.dbo.tblPolicias p
         WHERE p.marcaArma IS NOT NULL AND p.modeloArma IS NOT NULL
    )
    INSERT INTO dbo.Arma (Marca,Modelo,Calibre)
    SELECT s.Marca,s.Modelo,s.Calibre
      FROM SrcPoli s
     WHERE NOT EXISTS (SELECT 1 FROM dbo.Arma d WHERE d.Marca = s.Marca AND d.Modelo = s.Modelo);

    ;WITH Datos AS (
        SELECT
            RIGHT('00000000' + CAST(p.num_cedula AS varchar(8)),8) AS CI,
            TRY_CAST(p.cargadoresArma AS INT)    AS Cargadores,
            TRY_CAST(p.municionesArma AS INT)    AS Municiones,
            p.observacionesArma                  AS Observaciones,
            p.serieArma                          AS Serie,
            CAST(LTRIM(RTRIM(p.marcaArma))  AS NVARCHAR(50))  COLLATE Modern_Spanish_CI_AS AS Marca,
            CAST(LTRIM(RTRIM(p.modeloArma)) AS NVARCHAR(100)) COLLATE Modern_Spanish_CI_AS AS Modelo
          FROM Personal.dbo.tblPolicias p
         WHERE p.marcaArma IS NOT NULL AND p.modeloArma IS NOT NULL
    )
    INSERT INTO dbo.FuncionarioArma (FuncionarioId,Cargadores,Municiones,Observaciones,FechaAsign,ArmaId,Serie)
    SELECT f.Id,d.Cargadores,d.Municiones,d.Observaciones,SYSUTCDATETIME(),a.Id,d.Serie
      FROM Datos d
     INNER JOIN dbo.Funcionario f ON f.CI = d.CI
     INNER JOIN dbo.Arma a ON a.Marca = d.Marca AND a.Modelo = d.Modelo
      LEFT JOIN dbo.FuncionarioArma fa ON fa.FuncionarioId = f.Id AND fa.ArmaId = a.Id
     WHERE fa.Id IS NULL;
    PRINT 'Paso 3.3: Armas y asignaciones migradas.';

    -- 3.4 Tablas hist�ricas
    PRINT 'Paso 3.4: Migrando tablas hist�ricas...';
    ;WITH CustodiasUnicas AS (
        SELECT hc.id_policia,hc.fecha,MIN(hc.area) AS area
          FROM Personal.dbo.tblHistoricoCustodias hc
         GROUP BY hc.id_policia,hc.fecha
    )
    INSERT INTO dbo.HistoricoCustodia (FuncionarioId,Fecha,Area)
    SELECT m.FuncionarioId,cu.fecha,cu.area
      FROM CustodiasUnicas cu
      JOIN dbo.MapPoliciaFunc m ON m.id_policia = cu.id_policia
     WHERE NOT EXISTS (
         SELECT 1 FROM dbo.HistoricoCustodia hc
          WHERE hc.FuncionarioId = m.FuncionarioId AND hc.Fecha = cu.fecha
    );

    ;WITH NoctAgreg AS (
        SELECT hn.id_policia,hn.a�o,hn.mes,SUM(hn.minutos) AS minutos
          FROM Personal.dbo.tblHistoricoNocturnidad hn
         GROUP BY hn.id_policia,hn.a�o,hn.mes
    )
    INSERT INTO dbo.HistoricoNocturnidad (FuncionarioId,Anio,Mes,Minutos)
    SELECT m.FuncionarioId,na.a�o,na.mes,na.minutos
      FROM NoctAgreg na
      JOIN dbo.MapPoliciaFunc m ON m.id_policia = na.id_policia
     WHERE NOT EXISTS (
         SELECT 1 FROM dbo.HistoricoNocturnidad hn
          WHERE hn.FuncionarioId = m.FuncionarioId AND hn.Anio = na.a�o AND hn.Mes = na.mes
    );

    ;WITH PresAgreg AS (
        SELECT hp.id_policia,hp.a�o,hp.mes,SUM(hp.minutos) AS minutos,SUM(hp.dias) AS dias,
               MIN(hp.incidencia) AS incidencia,MIN(hp.observaciones) AS observaciones
          FROM Personal.dbo.tblHistoricoPresentismo hp
         GROUP BY hp.id_policia,hp.a�o,hp.mes
    )
    INSERT INTO dbo.HistoricoPresentismo (FuncionarioId,Anio,Mes,Minutos,Dias,Incidencia,Observaciones)
    SELECT m.FuncionarioId,pa.a�o,pa.mes,pa.minutos,pa.dias,pa.incidencia,pa.observaciones
      FROM PresAgreg pa
      JOIN dbo.MapPoliciaFunc m ON m.id_policia = pa.id_policia
     WHERE NOT EXISTS (
         SELECT 1 FROM dbo.HistoricoPresentismo hp
          WHERE hp.FuncionarioId = m.FuncionarioId AND hp.Anio = pa.a�o AND hp.Mes = pa.mes
    );

    ;WITH ViatAgreg AS (
        SELECT hv.id_policia,hv.a�o,hv.mes,MIN(hv.incidencia) AS incidencia,MIN(hv.motivo) AS motivo
          FROM Personal.dbo.tblHistoricoViaticos hv
         GROUP BY hv.id_policia,hv.a�o,hv.mes
    )
    INSERT INTO dbo.HistoricoViatico (FuncionarioId,Anio,Mes,Incidencia,Motivo)
    SELECT m.FuncionarioId,va.a�o,va.mes,va.incidencia,va.motivo
      FROM ViatAgreg va
      JOIN dbo.MapPoliciaFunc m ON m.id_policia = va.id_policia
     WHERE NOT EXISTS (
         SELECT 1 FROM dbo.HistoricoViatico hv
          WHERE hv.FuncionarioId = m.FuncionarioId AND hv.Anio = va.a�o AND hv.Mes = va.mes
    );
    PRINT 'Paso 3.4: Tablas hist�ricas migradas.';

    -- 3.5 Usuarios
    PRINT 'Paso 3.5: Migrando usuarios...';
    ;WITH Src AS (
        SELECT u.id_usuario AS OrigId,
               LOWER(LTRIM(RTRIM(u.usuario))) COLLATE Modern_Spanish_CI_AS AS UserName,
               HASHBYTES('SHA2_256',CAST(u.clave AS NVARCHAR(4000)))       AS PwdHash,
               mp.FuncionarioId
          FROM Personal.dbo.tblUsuarios u
          LEFT JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = u.id_policia
    )
    MERGE dbo.Usuario AS tgt
    USING Src AS src
      ON tgt.UserName = src.UserName
    WHEN MATCHED THEN
      UPDATE SET tgt.FuncionarioId = src.FuncionarioId,
                 tgt.PasswordHash = src.PwdHash,
                 tgt.UpdatedAt    = SYSUTCDATETIME()
    WHEN NOT MATCHED BY TARGET THEN
      INSERT (UserName,PasswordHash,FuncionarioId,RolId,CreatedAt)
      VALUES(src.UserName,src.PwdHash,src.FuncionarioId,2,SYSUTCDATETIME());
    PRINT 'Paso 3.5: Usuarios migrados.';

    /*================================================================
      PASO 4: Migrar Fotos de Funcionarios
      Se migran todas las fotos del historial y se asigna la m�s
      reciente como foto de perfil principal en dbo.Funcionario.
    =================================================================*/
    PRINT 'Paso 4: Migrando fotos de funcionarios...';

    -- 4.1 Migrar el historial de fotos
    MERGE dbo.FuncionarioFotoHistorico AS tgt
    USING (
        SELECT
            p.id_picture AS PictureId,
            mp.FuncionarioId,
            CAST(p.Data AS VARBINARY(MAX)) AS Foto,
            p.FileName
        FROM Personal.dbo.tblPictures p
        JOIN dbo.MapPoliciaFunc mp ON mp.id_policia = p.id_policia
    ) AS src
      ON tgt.PictureId = src.PictureId
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (FuncionarioId, PictureId, Foto, FileName)
        VALUES (src.FuncionarioId, src.PictureId, src.Foto, src.FileName);

    -- 4.2 Actualizar la foto principal en la tabla Funcionario
    ;WITH UltimaFoto AS (
        SELECT
            ffh.FuncionarioId,
            ffh.Foto,
            ROW_NUMBER() OVER(PARTITION BY ffh.FuncionarioId ORDER BY ffh.PictureId DESC) as rn
        FROM dbo.FuncionarioFotoHistorico ffh
    )
    UPDATE f
    SET f.Foto = uf.Foto
    FROM dbo.Funcionario f
    JOIN UltimaFoto uf ON f.Id = uf.FuncionarioId
    WHERE uf.rn = 1;

    PRINT 'Paso 4: Fotos de funcionarios migradas.';

    PRINT '--- MIGRACI�N COMPLETA FINALIZADA CON �XITO ---';
    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    PRINT '!!! ERROR DURANTE LA MIGRACI�N !!!';
    THROW;
END CATCH;
GO
