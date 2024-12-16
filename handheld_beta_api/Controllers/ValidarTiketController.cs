using Microsoft.AspNetCore.Mvc;
using handheld_beta_api.Services;
using handheld_beta_api.Model;

namespace handheld_beta_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValidarTiketController : ControllerBase
    {
        private readonly ValidarTiketService _service;

        public ValidarTiketController(ValidarTiketService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<List<ValidarTiket>>> PostValidarTiket([FromBody] TiketRequest request)
        {
            try
            {
                // Llamar al servicio con el parámetro recibido
                var tikets = await _service.GetValidarTiketsAsync(request.Tiket);

                if (tikets == null || tikets.Count == 0)
                {
                    return NotFound("No se encontraron tickets.");
                }

                return Ok(tikets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        public class TiketRequest
        {
            public string Tiket { get; set; }
            public string? CodigoVista { get; internal set; }
        }
    }
}
