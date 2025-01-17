using Microsoft.AspNetCore.Mvc;
using handheld_beta_api.Services;
using handheld_beta_api.Model;
using System.Text.Json;

namespace handheld_beta_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ObtenerPedidoController : ControllerBase
    {
        private readonly ObtenerPedidoService _service;
        private readonly Dictionary<int, string> _databases;
        public ObtenerPedidoController(ObtenerPedidoService service)
        {
            _service = service;
            _databases = new Dictionary<int, string>
            {
                { 1, "JJVPRGPRODUCCION" },
                { 2, "PRGPRODUCCION" }
            };
        }

        [HttpPost]
        public async Task<ActionResult<string>> GetObtenerPedidos([FromBody] DevolverRequest body)
        {
            try
            {
                if (!_databases.ContainsKey(body.referencia))
                {
                    return BadRequest("Referencia inválida.");
                }

                string database = _databases[body.referencia];
                string devolver = body.Devolver;

                var pedidos = await _service.GetObtenerPedidosAsync(devolver,body.referencia);

                if (pedidos == null || pedidos.Count == 0)
                {
                    return NotFound("Pedidos no encontrados.");
                }

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        public class DevolverRequest
        {
            public string Devolver { get; set; }
            public int referencia { get; set; }
        }
    }
}