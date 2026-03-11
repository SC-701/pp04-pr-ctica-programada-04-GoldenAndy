using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface IModeloCarroFlujo
    {
        Task<IEnumerable<ModeloCarroResponse>> ObtenerPorMarca(Guid IdMarca);
    }
}
