using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Web.Pages.Vehiculos
{
    [Authorize]
    public class DetalleModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public VehiculoResponse vehiculo { get; set; } = default!;

        public DetalleModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGet(Guid? id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerVehiculo");

            using var cliente = ObtenerClienteConToken();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            vehiculo = JsonSerializer.Deserialize<VehiculoResponse>(resultado, opciones)!;
        }

        private HttpClient ObtenerClienteConToken()
        {
            var tokenClaim = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "AccessToken");

            var cliente = new HttpClient();

            if (tokenClaim != null)
            {
                cliente.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", tokenClaim.Value);
            }

            return cliente;
        }
    }
}