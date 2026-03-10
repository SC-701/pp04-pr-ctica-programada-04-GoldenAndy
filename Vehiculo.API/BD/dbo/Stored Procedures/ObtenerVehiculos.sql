CREATE   PROCEDURE dbo.ObtenerVehiculos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        v.Id,
        v.IdModelo,
        v.Placa,
        v.Color,
        v.Anio,
        v.Precio,
        v.CorreoPropietario,
        v.TelefonoPropietario,
        m.Nombre  AS Modelo,
        ma.Nombre AS Marca
    FROM dbo.Vehiculo AS v
    INNER JOIN dbo.Modelos AS m
        ON v.IdModelo = m.Id
    INNER JOIN dbo.Marca AS ma
        ON m.IdMarca = ma.Id;
END