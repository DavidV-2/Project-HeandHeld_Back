using Azure.Messaging;
using handheld_beta_api.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace handheld_beta_api.Services
{
    public class UsuarioService
    {
        private readonly UsuarioContext _usuariocontext;

        public UsuarioService(UsuarioContext context)
        {
            _usuariocontext = context;
        }

        public async Task<List<Usuario>> GetUsuariosAsync(decimal nit)
        {
            try
            {
                var devnit = new SqlParameter("@nit", SqlDbType.Decimal) { Value = nit };

                var usuarios = await _usuariocontext.Usuario
                    .FromSqlRaw("EXEC DV_SPpersonal_Activo @nit ",devnit)
                    .ToListAsync();

                if (usuarios == null || usuarios.Count == 0)
                {
                    throw new KeyNotFoundException("No se encontraron usuarios.");
                }

                return usuarios;
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Error en la consulta SQL: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar la consulta: {ex.Message}", ex);
            }
        }
    }
}
