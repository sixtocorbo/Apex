-- Script para agregar los nuevos tipos y actualizar todos los funcionarios

-- 1. Asegurar que TODOS los tipos de funcionario existan en el catálogo
SET IDENTITY_INSERT dbo.TipoFuncionario ON;

MERGE INTO dbo.TipoFuncionario AS Target
USING (VALUES
    (1, 'Policia'),
    (2, 'Operador Penitenciario'),
    (3, 'Profesional Universitario'), -- Nuevo
    (4, 'Técnico'),                 -- Nuevo
    (5, 'Administrativo')            -- Nuevo
) AS Source (Id, Nombre)
ON Target.Id = Source.Id
-- Si el Id existe pero el nombre es diferente, lo actualiza
WHEN MATCHED AND Target.Nombre <> Source.Nombre THEN
    UPDATE SET Nombre = Source.Nombre, UpdatedAt = GETDATE()
-- Si el Id no existe, lo inserta
WHEN NOT MATCHED BY TARGET THEN
    INSERT (Id, Nombre, CreatedAt)
    VALUES (Source.Id, Source.Nombre, GETDATE());

SET IDENTITY_INSERT dbo.TipoFuncionario OFF;
GO

PRINT '✔ Tabla [TipoFuncionario] actualizada con todos los escalafones.';

-- 2. Actualizar masivamente TODOS los funcionarios según su escalafón
UPDATE F
SET
    -- Usamos una sentencia CASE para aplicar la lógica completa
    F.TipoFuncionarioId = CASE E.Nombre
                            WHEN 'L' THEN 1 -- Policia
                            WHEN 'S' THEN 2 -- Operador Penitenciario
                            WHEN 'A' THEN 3 -- Profesional Universitario
                            WHEN 'B' THEN 4 -- Técnico
                            WHEN 'C' THEN 5 -- Administrativo
                            ELSE F.TipoFuncionarioId -- No cambiar si el escalafón es otro
                          END
FROM
    dbo.Funcionario AS F
JOIN
    dbo.Escalafon AS E ON F.EscalafonId = E.Id
WHERE
    E.Nombre IN ('L', 'S', 'A', 'B', 'C'); -- Aplicamos el cambio para todos los escalafones relevantes
GO

PRINT '✔ El campo TipoFuncionarioId ha sido actualizado para todos los funcionarios según su escalafón.';