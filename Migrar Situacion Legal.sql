-- Script para migrar los diferentes estatus a la tabla FuncionarioEstadoLegal
SET NOCOUNT ON;

-- 1. Migrar estado 'Sumariado'
INSERT INTO dbo.FuncionarioEstadoLegal (FuncionarioId, Tipo, Valor, FechaRegistro)
SELECT
    F.Id,
    'Sumariado', -- El tipo de estado legal
    'SI',        -- Un valor estandarizado
    GETDATE()    -- La fecha de la migración
FROM
    Personal.dbo.tblPolicias AS P
JOIN
    Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
WHERE
    P.sumariado IS NOT NULL AND LTRIM(RTRIM(P.sumariado)) <> ''
    AND NOT EXISTS (
        SELECT 1 FROM dbo.FuncionarioEstadoLegal
        WHERE FuncionarioId = F.Id AND Tipo = 'Sumariado'
    );
GO

-- 2. Migrar estado 'Separado del Cargo'
INSERT INTO dbo.FuncionarioEstadoLegal (FuncionarioId, Tipo, Valor, FechaRegistro)
SELECT
    F.Id,
    'Separado del Cargo',
    'SI',
    GETDATE()
FROM
    Personal.dbo.tblPolicias AS P
JOIN
    Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
WHERE
    P.separado_cargo IS NOT NULL AND LTRIM(RTRIM(P.separado_cargo)) <> ''
    AND NOT EXISTS (
        SELECT 1 FROM dbo.FuncionarioEstadoLegal
        WHERE FuncionarioId = F.Id AND Tipo = 'Separado del Cargo'
    );
GO

-- 3. Migrar estado 'Procesado'
INSERT INTO dbo.FuncionarioEstadoLegal (FuncionarioId, Tipo, Valor, FechaRegistro)
SELECT
    F.Id,
    'Procesado',
    'SI',
    GETDATE()
FROM
    Personal.dbo.tblPolicias AS P
JOIN
    Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
WHERE
    P.procesado IS NOT NULL AND LTRIM(RTRIM(P.procesado)) <> ''
    AND NOT EXISTS (
        SELECT 1 FROM dbo.FuncionarioEstadoLegal
        WHERE FuncionarioId = F.Id AND Tipo = 'Procesado'
    );
GO

-- 4. Migrar estado 'Desarmado'
INSERT INTO dbo.FuncionarioEstadoLegal (FuncionarioId, Tipo, Valor, FechaRegistro)
SELECT
    F.Id,
    'Desarmado',
    'SI',
    GETDATE()
FROM
    Personal.dbo.tblPolicias AS P
JOIN
    Apex.dbo.Funcionario AS F ON F.CI = RIGHT('00000000' + CAST(P.num_cedula AS VARCHAR(8)), 8)
WHERE
    P.desarmado IS NOT NULL AND LTRIM(RTRIM(P.desarmado)) <> ''
    AND NOT EXISTS (
        SELECT 1 FROM dbo.FuncionarioEstadoLegal
        WHERE FuncionarioId = F.Id AND Tipo = 'Desarmado'
    );
GO

PRINT '✔ Datos de estatus (Sumariado, Separado del Cargo, Procesado, Desarmado) migrados correctamente.';