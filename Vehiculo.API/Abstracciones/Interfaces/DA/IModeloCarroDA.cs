using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.DA
{
    public interface IModeloCarroDA
    {
        Task<IEnumerable<ModeloCarroResponse>> ObtenerPorMarca(Guid IdMarca);
    }
}

