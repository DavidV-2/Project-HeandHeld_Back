using Microsoft.AspNetCore.Mvc;
using handheld_beta_api.Services;
using handheld_beta_api.Model;

namespace handheld_beta_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ObtenerPedidoController : ControllerBase
    {
        private readonly ObtenerPedidoService _service;

        public ObtenerPedidoController(ObtenerPedidoService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<List<ObtenerPedido>>> GetObtenerPedidos([FromBody] DevolverRequest body)
        {
            try
            {
                // Obtener el parámetro 'devolver' del cuerpo
                string devolver = body.Devolver;

                var pedidos = await _service.GetObtenerPedidosAsync(devolver);

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
        }
    }
}
