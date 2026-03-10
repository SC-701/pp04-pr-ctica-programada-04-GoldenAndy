CREATE   PROCEDURE [dbo].[AgregarVehiculo]
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
    INSERT INTO [dbo].[Vehiculo]
    (
        [Id],
        [IdModelo],
        [Placa],
        [Color],
        [Anio],
        [Precio],
        [CorreoPropietario],
        [TelefonoPropietario]
    )
    VALUES
    (
        @Id,
        @IdModelo,
        @Placa,
        @Color,
        @Anio,
        @Precio,
        @CorreoPropietario,
        @TelefonoPropietario)
        
        SELECT @Id
   COMMIT TRANSACTION

END