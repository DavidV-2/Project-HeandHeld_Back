using Microsoft.AspNetCore.Mvc;
using handheld_beta_api.Services;
using handheld_beta_api.Model;
using System.Text.Json;

namespace handheld_beta_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValidarTiketController : ControllerBase
    {
        private readonly ValidarTiketService _service;
        private readonly Dictionary<int, string> _databases;

        public ValidarTiketController(ValidarTiketService service)
        {
            _service = service;
            _databases = new Dictionary<int, string>
            {
                { 1, "JJVPRGPRODUCCION" },
                { 2, "PRGPRODUCCION" }
            };
        }

        [HttpPost]
        public async Task<ActionResult<List<ValidarTiket>>> PostValidarTiket([FromBody] TiketRequest request)
        {
            try
            {
                if (!_databases.ContainsKey(request.referencia))
                {
                    return BadRequest("Referencia inválida.");
                }
                string database = _databases[request.referencia];

                // Llamar al servicio con el parámetro recibido
                var tikets = await _service.GetValidarTiketsAsync(request.Tiket, request.referencia);

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
            public int referencia { get; set; }
        }
    }
}
