using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;


namespace Web.Pages.Vehiculos
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public VehiculoRequest vehiculo { get; set; }

        [BindProperty]
        public List<SelectListItem> marcas { get; set; }

        [BindProperty]
        public List<SelectListItem> modelos { get; set; }

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
                return Page();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarVehiculo");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Post, endpoint);

            var respuesta = await cliente.PostAsJsonAsync(endpoint, vehiculo);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }

        private async Task ObtenerMarcas()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerMarcas");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

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
                var cliente = new HttpClient();

                var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, marcaID));

                var respuesta = await cliente.SendAsync(solicitud);
                respuesta.EnsureSuccessStatusCode();

                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                return JsonSerializer.Deserialize<List<ModeloCarroResponse>>(resultado, opciones) ?? new();
            }


        public async Task<JsonResult> OnGetObtenerModelos(Guid marcaID)
            {
                var modelos = await ObtenerModelos(marcaID);
                return new JsonResult(modelos);
            }

    }
}
