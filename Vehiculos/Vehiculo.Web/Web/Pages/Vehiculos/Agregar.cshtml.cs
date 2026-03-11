using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Web.Pages.Vehiculos
{
    [Authorize]
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public VehiculoRequest vehiculo { get; set; } = default!;

        [BindProperty]
        public List<SelectListItem> marcas { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> modelos { get; set; } = new();

        [BindProperty]
        public Guid marcaSeleccionada { get; set; }

        public async Task<ActionResult> OnGet()
        {
            await ObtenerMarcas();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await ObtenerMarcas();
                return Page();
            }

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarVehiculo");

            using var cliente = ObtenerClienteConToken();
            var respuesta = await cliente.PostAsJsonAsync(endpoint, vehiculo);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }

        private async Task ObtenerMarcas()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerMarcas");

            using var cliente = ObtenerClienteConToken();
            var respuesta = await cliente.GetAsync(endpoint);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var resultadoDeserializado = JsonSerializer.Deserialize<List<MarcaResponse>>(resultado, opciones) ?? new();

            marcas = resultadoDeserializado
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Nombre
                })
                .ToList();
        }

        private async Task<List<ModeloCarroResponse>> ObtenerModelos(Guid marcaID)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerModelos");

            using var cliente = ObtenerClienteConToken();
            var respuesta = await cliente.GetAsync(string.Format(endpoint, marcaID));
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<ModeloCarroResponse>>(resultado, opciones) ?? new List<ModeloCarroResponse>();
        }

        public async Task<JsonResult> OnGetObtenerModelos(Guid marcaID)
        {
            var modelos = await ObtenerModelos(marcaID);
            return new JsonResult(modelos);
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