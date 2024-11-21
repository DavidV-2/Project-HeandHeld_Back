using Microsoft.AspNetCore.Mvc;
using handheld_beta_api.Services;
using DocumentFormat.OpenXml.Wordprocessing;
using Twilio.TwiML.Messaging;
using System.Text.Json;

namespace handheld_beta_api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        [HttpPost]
        public async Task<ActionResult<string>> VerificarUsuario([FromBody] JsonElement body)
        {
            try
            {
                decimal nit = 0;

                if (!body.TryGetProperty("nit", out JsonElement nitElement) || !decimal.TryParse(nitElement.ToString(), out nit) || nit <= 0)
                {
                    return BadRequest("El valor de NIT debe ser mayor a 0.");
                }

                // Convertir el NIT a decimal
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
        }
    }
}
