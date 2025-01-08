using Microsoft.AspNetCore.Mvc;
using handheld_beta_api.Services;
using System.Text.Json;

namespace handheld_beta_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly Dictionary<int, string> _databases;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
            _databases = new Dictionary<int, string>
            {
                { 1, "JJVDMSCIERREAGOSTO" },
                { 2, "CORSAN" }
            };
        }
        [HttpPost]
        public async Task<ActionResult<string>> VerificarUsuario([FromBody] JsonElement body)
        {
            try
            {
                if (!body.TryGetProperty("referencia", out JsonElement referenciaElement) ||
                    !int.TryParse(referenciaElement.ToString(), out int referencia))
                {
                    return BadRequest("La referencia es inválida.");
                }

                decimal nit = 0;

                if (!body.TryGetProperty("nit", out JsonElement nitElement) || !decimal.TryParse(nitElement.ToString(), out nit) || nit <= 0)
                {
                    return BadRequest("El valor de NIT debe ser mayor a 0.");
                }

                string database = _databases[referencia];
                decimal devNit = Convert.ToDecimal(nit);

                var usuarios = await _usuarioService.GetUsuariosAsync(devNit, referencia);
                var usuario = usuarios.FirstOrDefault();

                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                return Ok(new { nombres = usuario.nombres });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        /*[HttpPost]
        public async Task<ActionResult<string>> VerificarUsuario([FromBody] JsonElement body)
        {
            try
            {
                if (!body.TryGetProperty("referencia", out JsonElement referenciaElement) ||
                !int.TryParse(referenciaElement.ToString(), out int referencia) ||
                !_databases.ContainsKey(referencia))
                {
                    return BadRequest("La referencia es inválida.");
                }

                decimal nit = 0;

                if (!body.TryGetProperty("nit", out JsonElement nitElement) || !decimal.TryParse(nitElement.ToString(), out nit) || nit <= 0)
                {
                    return BadRequest("El valor de NIT debe ser mayor a 0.");
                }

                string database = _databases[referencia];
                decimal devNit = Convert.ToDecimal(nit);

                var usuarios = await _usuarioService.GetUsuariosAsync(devNit);
                var usuario = usuarios.FirstOrDefault();

                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                return Ok(new { nombres = usuario.nombres });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }*/
    }
}