
/* =============================================================================
   Script de creación de base de datos: Sistema de Funcionarios Penitenciarios
   Target: SQL Server 2019+
   Autor: ChatGPT (OpenAI o3) – Junio 2025
   NOTA: Ajuste nombres de base de datos y rutas de almacenamiento según entorno.
============================================================================= */

-- Cree la base si lo desea (opcional)
 --CREATE DATABASE CarcelPersonalDB;
 --GO
 --USE CarcelPersonalDB;
 --GO

/* ======================== TABLAS DE CATÁLOGO ============================== */

CREATE TABLE dbo.TipoFuncionario (
    Id                INT            IDENTITY(1,1) PRIMARY KEY,
    Nombre            NVARCHAR(50)   NOT NULL UNIQUE,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2      NULL
);
GO

CREATE TABLE dbo.Cargo (
    Id                INT            IDENTITY(1,1) PRIMARY KEY,
    Nombre            NVARCHAR(100)  NOT NULL UNIQUE,
    EsJerarquico      BIT            NOT NULL DEFAULT 0,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2      NULL
);
GO

CREATE TABLE dbo.Estado (
    Id                INT            IDENTITY(1,1) PRIMARY KEY,
    Nombre            NVARCHAR(60)   NOT NULL UNIQUE,
    EsAusentismo      BIT            NOT NULL DEFAULT 0,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2      NULL
);
GO

CREATE TABLE dbo.TipoLicencia (
    Id                INT            IDENTITY(1,1) PRIMARY KEY,
    Codigo            NVARCHAR(20)   NOT NULL UNIQUE,
    Nombre            NVARCHAR(100)  NOT NULL,
    RequiereAdjunto   BIT            NOT NULL DEFAULT 0,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2      NULL
);
GO

CREATE TABLE dbo.TipoEstadoTransitorio (
    Id                INT            IDENTITY(1,1) PRIMARY KEY,
    Nombre            NVARCHAR(60)   NOT NULL UNIQUE,
    EsJerarquico      BIT            NOT NULL DEFAULT 0,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2      NULL
);
GO

/* ========================= TABLAS PRINCIPALES ============================ */

CREATE TABLE dbo.Funcionario (
    Id                INT           IDENTITY(1,1) PRIMARY KEY,
    NroLegajo         INT           NOT NULL UNIQUE,
    CI                CHAR(8)       NOT NULL UNIQUE,
    Nombre            NVARCHAR(60)  NOT NULL,
    Apellido          NVARCHAR(60)  NOT NULL,
    Foto              VARBINARY(MAX) NULL,      -- o almacene ruta en NVARCHAR
    FechaIngreso      DATE          NOT NULL,
    TipoFuncionarioId INT           NOT NULL,
    CargoId           INT           NULL,
    Activo            BIT           NOT NULL DEFAULT 1,
    CreatedAt         DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2     NULL,

    CONSTRAINT FK_Funcionario_TipoFuncionario
        FOREIGN KEY (TipoFuncionarioId) REFERENCES dbo.TipoFuncionario(Id),

    CONSTRAINT FK_Funcionario_Cargo
        FOREIGN KEY (CargoId) REFERENCES dbo.Cargo(Id)
);
GO

CREATE TABLE dbo.RegimenTrabajo (
    Id                INT            IDENTITY(1,1) PRIMARY KEY,
    Descripcion       NVARCHAR(120)  NOT NULL,
    TipoPatron        TINYINT        NOT NULL,       -- 0:Semanal 1:Quincenal 2:Rango
    Observaciones     NVARCHAR(250)  NULL,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2      NULL
);
GO

CREATE TABLE dbo.RegimenDetalle (
    Id                INT            IDENTITY(1,1) PRIMARY KEY,
    RegimenTrabajoId  INT            NOT NULL,
    DiaSemana         TINYINT        NULL,          -- 0=Domingo ... 6=Sábado
    SemanaPar         BIT            NULL,          -- NULL si no aplica
    FechaInicio       DATE           NULL,
    FechaFin          DATE           NULL,
    HoraDesde         TIME           NULL,
    HoraHasta         TIME           NULL,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2      NULL,

    CONSTRAINT FK_RegimenDetalle_RegimenTrabajo
        FOREIGN KEY (RegimenTrabajoId) REFERENCES dbo.RegimenTrabajo(Id)
);
GO

CREATE TABLE dbo.Movimiento (
    Id                INT           IDENTITY(1,1) PRIMARY KEY,
    FuncionarioId     INT           NOT NULL,
    Fecha             DATE          NOT NULL,
    EstadoId          INT           NOT NULL,
    CreatedAt         DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2     NULL,

    CONSTRAINT FK_Movimiento_Funcionario
        FOREIGN KEY (FuncionarioId) REFERENCES dbo.Funcionario(Id),

    CONSTRAINT FK_Movimiento_Estado
        FOREIGN KEY (EstadoId) REFERENCES dbo.Estado(Id)
);
GO
-- No puede haber más de un registro por día y funcionario
CREATE UNIQUE INDEX UX_Movimiento_Funcionario_Fecha
    ON dbo.Movimiento(FuncionarioId, Fecha);
GO

CREATE TABLE dbo.LicenciaDetalle (
    Id                INT            IDENTITY(1,1) PRIMARY KEY,
    MovimientoId      INT            NOT NULL,
    TipoLicenciaId    INT            NOT NULL,
    PropiedadesJSON   NVARCHAR(MAX)  NULL,
    FechaDesde        DATE           NOT NULL,
    FechaHasta        DATE           NOT NULL,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2      NULL,

    CONSTRAINT FK_LicenciaDetalle_Movimiento
        FOREIGN KEY (MovimientoId) REFERENCES dbo.Movimiento(Id),

    CONSTRAINT FK_LicenciaDetalle_TipoLicencia
        FOREIGN KEY (TipoLicenciaId) REFERENCES dbo.TipoLicencia(Id)
);
GO

CREATE TABLE dbo.EstadoTransitorio (
    Id                       INT            IDENTITY(1,1) PRIMARY KEY,
    FuncionarioId            INT            NOT NULL,
    TipoEstadoTransitorioId  INT            NOT NULL,
    FechaDesde               DATE           NOT NULL,
    FechaHasta               DATE           NULL,
    Observaciones            NVARCHAR(250)  NULL,
    CreatedAt                DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt                DATETIME2      NULL,

    CONSTRAINT FK_EstadoTransitorio_Funcionario
        FOREIGN KEY (FuncionarioId) REFERENCES dbo.Funcionario(Id),

    CONSTRAINT FK_EstadoTransitorio_Tipo
        FOREIGN KEY (TipoEstadoTransitorioId) REFERENCES dbo.TipoEstadoTransitorio(Id)
);
GO

/* ========================= NOVEDADES & NOTIFICACIONES ==================== */

CREATE TABLE dbo.Novedad (
    Id                INT            IDENTITY(1,1) PRIMARY KEY,
    Fecha             DATETIME2      NOT NULL,
    Descripcion       NVARCHAR(250)  NOT NULL,
    TieneNotificacion BIT            NOT NULL DEFAULT 0,
    CreatedAt         DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt         DATETIME2      NULL
);
GO

CREATE TABLE dbo.NovedadFuncionario (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    NovedadId     INT NOT NULL,
    FuncionarioId INT NOT NULL,
    CreatedAt     DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt     DATETIME2 NULL,

    CONSTRAINT FK_NovedadFuncionario_Novedad
        FOREIGN KEY (NovedadId) REFERENCES dbo.Novedad(Id),

    CONSTRAINT FK_NovedadFuncionario_Funcionario
        FOREIGN KEY (FuncionarioId) REFERENCES dbo.Funcionario(Id)
);
GO
-- Evita duplicar la misma persona en la misma novedad
CREATE UNIQUE INDEX UX_NovedadFuncionario_Novedad_Funcionario
    ON dbo.NovedadFuncionario(NovedadId, FuncionarioId);
GO

CREATE TABLE dbo.Notificacion (
    Id               INT            IDENTITY(1,1) PRIMARY KEY,
    NovedadId        INT            NOT NULL,
    FechaProgramada  DATETIME2      NOT NULL,
    FechaEnviada     DATETIME2      NULL,
    Medio            NVARCHAR(30)   NULL,
    Resultado        NVARCHAR(100)  NULL,
    CreatedAt        DATETIME2      NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt        DATETIME2      NULL,

    CONSTRAINT FK_Notificacion_Novedad
        FOREIGN KEY (NovedadId) REFERENCES dbo.Novedad(Id)
);
GO

/* ========================= ÍNDICES ADICIONALES =========================== */

-- Agiliza reporte de ausentismo por tipo de licencia
CREATE INDEX IX_LicenciaDetalle_Fechas
    ON dbo.LicenciaDetalle(FechaDesde, FechaHasta);
GO

-- Historial de estados transitorios por persona
CREATE INDEX IX_EstadoTransitorio_Persona_Fechas
    ON dbo.EstadoTransitorio(FuncionarioId, FechaDesde, FechaHasta);
GO

/* ========================= FIN DEL SCRIPT =============================== */
