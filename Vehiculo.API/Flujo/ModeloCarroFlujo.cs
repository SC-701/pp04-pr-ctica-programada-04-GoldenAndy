using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class ModeloCarroFlujo : IModeloCarroFlujo
    {
        private readonly IModeloCarroDA _modeloCarroDA;

        public ModeloCarroFlujo(IModeloCarroDA modeloCarroDA)
        {
            _modeloCarroDA = modeloCarroDA;
        }

        public async Task<IEnumerable<ModeloCarroResponse>> ObtenerPorMarca(Guid IdMarca)
        {
            return await _modeloCarroDA.ObtenerPorMarca(IdMarca);
        }
    }
}
