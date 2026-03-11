namespace Abstracciones.Interfaces.Reglas
{
    public interface IRevisionReglas
    {
        Task<bool> RevisionEsValida(string placa);
    }
}
