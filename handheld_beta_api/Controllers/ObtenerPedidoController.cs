using Microsoft.AspNetCore.Mvc;
using handheld_beta_api.Services;
using handheld_beta_api.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

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

        // GET /ObtenerPedido
        [HttpPost]
        public async Task<ActionResult<List<ObtenerPedido>>> GetObtenerPedidos([FromQuery] string devolver)
        {
            try
            {
                var pedidos = await _service.GetObtenerPedidosAsync(devolver);

                if (pedidos != null) { 

                    return Ok(pedidos.ToList());
                    
                }
                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");

            }
        }
    }
}
