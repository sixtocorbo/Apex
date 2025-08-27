USE Apex;
GO
CREATE OR ALTER PROCEDURE dbo.usp_ProcesarEstadoNotificaciones
AS
BEGIN
    SET NOCOUNT, XACT_ABORT ON;

    DECLARE @Hoy DATETIME2 = SYSUTCDATETIME();

    /*
      Firmada  → EstadoId = 3 (no se modifica aquí).
      Pendiente→ Hoy  < FechaProgramada
      Vencida  → Hoy >= FechaProgramada
    */

    /* Pendiente */
    UPDATE dbo.NotificacionPersonal
    SET EstadoId = 1,
        UpdatedAt = @Hoy
    WHERE EstadoId <> 3               -- no tocar firmadas
      AND @Hoy <  FechaProgramada
      AND EstadoId <> 1;

    /* Vencida */
    UPDATE dbo.NotificacionPersonal
    SET EstadoId = 2,
        UpdatedAt = @Hoy
    WHERE EstadoId <> 3
      AND @Hoy >= FechaProgramada
      AND EstadoId <> 2;
END;
GO
