USE [Apex]
GO
/****** Object:  StoredProcedure [dbo].[usp_MigrarDotacionesCompletas]    Script Date: 27/08/2025 0:33:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_MigrarDotacionesCompletas]
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    -- ====================================================================
    -- 1. MIGRACIÓN DE ARMAS
    -- ====================================================================
    BEGIN TRY
        BEGIN TRAN;

        -- 1a. Poblar el catálogo dbo.Arma con datos únicos de tblPolicias
        -- Se insertan las combinaciones de marca/modelo que no existan.
        INSERT INTO dbo.Arma (Marca, Modelo, Calibre)
        SELECT DISTINCT
               LTRIM(RTRIM(p.marcaArma)) COLLATE Modern_Spanish_CI_AS,
               LTRIM(RTRIM(p.modeloArma)) COLLATE Modern_Spanish_CI_AS,
               '9mm' AS Calibre -- Asumimos calibre 9mm como estándar
        FROM Personal.dbo.tblPolicias AS p
        WHERE p.marcaArma IS NOT NULL
          AND p.modeloArma IS NOT NULL
          AND NOT EXISTS (
              SELECT 1 FROM dbo.Arma d
              WHERE d.Marca = LTRIM(RTRIM(p.marcaArma)) COLLATE Modern_Spanish_CI_AS
                AND d.Modelo = LTRIM(RTRIM(p.modeloArma)) COLLATE Modern_Spanish_CI_AS
          );

        -- 1b. Migrar las asignaciones de armas a dbo.FuncionarioArma
        -- Se limpian los datos existentes para evitar duplicados
        TRUNCATE TABLE dbo.FuncionarioArma;

        INSERT INTO dbo.FuncionarioArma (FuncionarioId, ArmaId, Serie, Cargadores, Municiones, Observaciones, FechaAsign)
        SELECT
            m.FuncionarioId,
            a.Id AS ArmaId,
            p.serieArma,
            TRY_CAST(p.cargadoresArma AS INT),
            TRY_CAST(p.municionesArma AS INT),
            p.observacionesArma,
            GETDATE() AS FechaAsign
        FROM Personal.dbo.tblPolicias AS p
        JOIN dbo.MapPoliciaFunc AS m ON p.id_policia = m.id_policia
        JOIN dbo.Arma AS a ON a.Marca = LTRIM(RTRIM(p.marcaArma)) COLLATE Modern_Spanish_CI_AS
                           AND a.Modelo = LTRIM(RTRIM(p.modeloArma)) COLLATE Modern_Spanish_CI_AS
        WHERE p.marcaArma IS NOT NULL AND p.modeloArma IS NOT NULL;

        COMMIT;
        PRINT 'Migración de Armas completada.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        PRINT 'Error en la migración de Armas: ' + ERROR_MESSAGE();
        THROW;
    END CATCH;

    -- ====================================================================
    -- 2. MIGRACIÓN DE CHALECOS
    -- ====================================================================
    BEGIN TRY
        BEGIN TRAN;

        -- Se limpian los datos existentes para evitar duplicados
        TRUNCATE TABLE dbo.FuncionarioChaleco;

        INSERT INTO dbo.FuncionarioChaleco (FuncionarioId, Marca, Modelo, Serie, Tipo, Observaciones, FechaAsign)
        SELECT
            m.FuncionarioId,
            p.marcaChaleco,
            p.modeloChaleco,
            p.serieChaleco,
            p.tipoChaleco,
            p.observacionesChaleco,
            GETDATE() AS FechaAsign
        FROM Personal.dbo.tblPolicias AS p
        JOIN dbo.MapPoliciaFunc AS m ON p.id_policia = m.id_policia
        WHERE p.marcaChaleco IS NOT NULL OR p.modeloChaleco IS NOT NULL OR p.serieChaleco IS NOT NULL;

        COMMIT;
        PRINT 'Migración de Chalecos completada.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        PRINT 'Error en la migración de Chalecos: ' + ERROR_MESSAGE();
        THROW;
    END CATCH;

    -- ====================================================================
    -- 3. MIGRACIÓN DE OTRAS DOTACIONES (Unpivot)
    -- ====================================================================
    BEGIN TRY
        BEGIN TRAN;

        -- 3a. Poblar el catálogo dbo.DotacionItem con los tipos de equipo
        MERGE INTO dbo.DotacionItem AS Target
        USING (VALUES
            ('Camisa'), ('Pantalón'), ('Botas'), ('Zapatos'),
            ('Gorro'), ('Buso'), ('Campera'), ('Esposas')
        ) AS Source (Nombre)
        ON Target.Nombre COLLATE Modern_Spanish_CI_AS = Source.Nombre
        WHEN NOT MATCHED BY TARGET THEN
            INSERT (Nombre, Activo, CreatedAt)
            VALUES (Source.Nombre, 1, GETDATE());

        -- 3b. Migrar las dotaciones "despivotando" las columnas de tblPolicias
        TRUNCATE TABLE dbo.FuncionarioDotacion;

        WITH DotacionesUnpivot AS (
            SELECT id_policia, Item, Talla
            FROM Personal.dbo.tblPolicias
            UNPIVOT (
                Talla FOR Item IN (camisa, pantalon, botas, zapatos, gorro, buso, campera, esposas)
            ) AS unpvt
        )
        INSERT INTO dbo.FuncionarioDotacion (FuncionarioId, DotacionItemId, Talla, FechaAsign)
        SELECT
            m.FuncionarioId,
            di.Id AS DotacionItemId,
            du.Talla,
            GETDATE() AS FechaAsign
        FROM DotacionesUnpivot AS du
        JOIN dbo.MapPoliciaFunc AS m ON du.id_policia = m.id_policia
        JOIN dbo.DotacionItem AS di ON di.Nombre COLLATE Modern_Spanish_CI_AS = du.Item
        WHERE LTRIM(RTRIM(du.Talla)) <> '' AND du.Talla IS NOT NULL;

        COMMIT;
        PRINT 'Migración de Otras Dotaciones completada.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        PRINT 'Error en la migración de Otras Dotaciones: ' + ERROR_MESSAGE();
        THROW;
    END CATCH;

    PRINT 'PROCESO DE MIGRACIÓN DE DOTACIONES FINALIZADO CON ÉXITO.';

END
