using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Web.Pages.Vehiculos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public IList<VehiculoResponse> vehiculos { get; set; } = default!;

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGet()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerVehiculos");

            using var cliente = ObtenerClienteConToken();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            vehiculos = JsonSerializer.Deserialize<List<VehiculoResponse>>(resultado, opciones)
                       ?? new List<VehiculoResponse>();
        }

        private HttpClient ObtenerClienteConToken()
        {
            var tokenClaim = HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "AccessToken");

            if (tokenClaim == null || string.IsNullOrWhiteSpace(tokenClaim.Value))
            {
                throw new Exception("No se encontró el claim AccessToken en el usuario autenticado.");
            }

            var cliente = new HttpClient();
            cliente.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenClaim.Value);

            return cliente;
        }
    }
}