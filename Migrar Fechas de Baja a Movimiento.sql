-- Script corregido para migrar únicamente las fechas de baja
SET NOCOUNT ON;

-- Migrar las fechas de baja a la tabla Movimiento
INSERT INTO dbo.Movimiento (FuncionarioId, Fecha, EstadoId, CreatedAt)
SELECT
    F.Id,
    P.baja,
    (SELECT Id FROM dbo.Estado WHERE Nombre = 'Baja'),
    GETDATE()
FROM
    Personal.dbo.tblPolicias AS P
JOIN
    Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
WHERE
    P.baja IS NOT NULL
    AND NOT EXISTS (
        -- Evita duplicados si el script se ejecuta más de una vez
        SELECT 1 FROM dbo.Movimiento M
        WHERE M.FuncionarioId = F.Id AND M.EstadoId = (SELECT Id FROM dbo.Estado WHERE Nombre = 'Baja')
    );
GO

PRINT '✔ Fechas de Baja migradas correctamente a la tabla Movimiento.';