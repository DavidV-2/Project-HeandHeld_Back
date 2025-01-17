using Microsoft.AspNetCore.Mvc;
using handheld_beta_api.Services;
using System.Text.Json;

namespace handheld_beta_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PermisosTrasladoController : ControllerBase
    {
        private readonly PermisosTrasladoService _PTService;
        private readonly Dictionary<int, string> _databases;

        public PermisosTrasladoController(PermisosTrasladoService service)
        {
            _PTService = service;
            _databases = new Dictionary<int, string>
            {
                { 1, "JJVPRGPRODUCCION" },
                { 2, "PRGPRODUCCION" }
            };
        }

        [HttpPost]
        public async Task<ActionResult> VerificarPermisosTraslado([FromBody] JsonElement body)
        {
            try
            {
                if (!body.TryGetProperty("referencia", out JsonElement referenciaElement) ||
                    !int.TryParse(referenciaElement.ToString(), out int referencia))
                {
                    return BadRequest("La referencia es inválida.");
                }

                if (!body.TryGetProperty("nitEntrega", out JsonElement nitEntregaElement) || !body.TryGetProperty("nitRecibe", out JsonElement nitRecibeElement))
                {
                    return BadRequest("El cuerpo de la solicitud debe contener 'nitEntrega' y 'nitRecibe'.");
                }

                string database = _databases[referencia];
                int nitEntrega = nitEntregaElement.GetInt32();
                int nitRecibe = nitRecibeElement.GetInt32();

                // Verificar permisos para ambos NITs
                var permisoEntrega = await _PTService.GetPermisosTrasladosAsync(nitEntrega, referencia);
                var permisoRecibe = await _PTService.GetPermisosTrasladosAsync(nitRecibe, referencia);

                // Validar los resultados
                if (permisoEntrega == null || permisoRecibe == null)
                {
                    return NotFound("No se encontraron permisos para uno o ambos NITs.");
                }

                return Ok(new
                {
                    nitEntrega = new { permiso = permisoEntrega.permiso, cargo = permisoEntrega.modulo },
                    nitRecibe = new { permiso = permisoRecibe.permiso, cargo = permisoRecibe.modulo }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}

