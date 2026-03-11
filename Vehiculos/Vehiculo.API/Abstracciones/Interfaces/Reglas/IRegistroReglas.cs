namespace Abstracciones.Interfaces.Reglas
{
    public interface IRegistroReglas
    {
        Task<bool> VehiculoEstaRegistrado(string placa, string email);
    }
}
