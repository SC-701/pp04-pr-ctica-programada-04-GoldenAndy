using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IModeloCarroController
    {
        Task<IActionResult> ObtenerPorMarca(Guid IdMarca);
    }
}