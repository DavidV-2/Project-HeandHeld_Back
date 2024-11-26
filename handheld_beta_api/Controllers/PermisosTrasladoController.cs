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

        public PermisosTrasladoController(PermisosTrasladoService service)
        {
            _PTService = service;
        }

        [HttpPost]
        public async Task<ActionResult> VerificarPermisosTraslado([FromBody] JsonElement body)
        {
            try
            {
                // Validar si el JSON contiene los campos necesarios
                if (!body.TryGetProperty("nitEntrega", out JsonElement nitEntregaElement) ||
                    !body.TryGetProperty("nitRecibe", out JsonElement nitRecibeElement))
                {
                    return BadRequest("El cuerpo de la solicitud debe contener 'nitEntrega' y 'nitRecibe'.");
                }

                // Convertir los valores de NIT
                if (!decimal.TryParse(nitEntregaElement.ToString(), out decimal nitEntrega) || nitEntrega <= 0 ||
                    !decimal.TryParse(nitRecibeElement.ToString(), out decimal nitRecibe) || nitRecibe <= 0)
                {
                    return BadRequest("Ambos NITs deben ser valores decimales mayores a 0.");
                }

                // Verificar permisos para ambos NITs
                var permisoEntrega = await _PTService.GetPermisosTrasladosAsync(nitEntrega);
                var permisoRecibe = await _PTService.GetPermisosTrasladosAsync(nitRecibe);

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
        /*
    [HttpGet("{nit}")]
    public async Task<IActionResult> VerificarPermiso(decimal nit)
    {
        try
        {
            var permiso = await _PTService.GetPermisosTrasladosAsync(nit);

            if (permiso == null)
                return NotFound(new { mensaje = "No se encontró un permiso para el NIT proporcionado." });

            return Ok(permiso);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno: {ex.Message}");
        }
    }*/
    }
}

