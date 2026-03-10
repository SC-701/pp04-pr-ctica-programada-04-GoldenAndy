CREATE   PROCEDURE [dbo].[EditarVehiculo]
    @Id UNIQUEIDENTIFIER,
    @IdModelo UNIQUEIDENTIFIER,
    @Placa VARCHAR(MAX),
    @Color VARCHAR(MAX),
    @Anio INT,
    @Precio DECIMAL(18,2),
    @CorreoPropietario VARCHAR(MAX),
    @TelefonoPropietario VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION
    UPDATE [dbo].[Vehiculo]
    SET
        [IdModelo] = @IdModelo,
        [Placa] = @Placa,
        [Color] = @Color,
        [Anio] = @Anio,
        [Precio] = @Precio,
        [CorreoPropietario] = @CorreoPropietario,
        [TelefonoPropietario] = @TelefonoPropietario
    WHERE [Id] = @Id;
    SELECT @Id
    COMMIT TRANSACTION
END