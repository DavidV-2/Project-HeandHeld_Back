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

        /*public async Task<List<Usuario>> GetUsuariosAsync(int nit)
        {
            try
            {
                var devnit = new SqlParameter("@nit", SqlDbType.Int) { Value = nit };


                var usuarios = await _usuariocontext.Usuario
                    .FromSqlRaw("SELECT nit, nombres FROM V_nom_personal_Activo_con_maquila where nit = @nit", devnit)
                    .ToListAsync();

                // Si no se encontraron usuarios, lanzar una excepción
                if (usuarios == null || usuarios.Count == 0)
                {
                    throw new Exception("No se encontraron usuarios.");
                }

                return usuarios;
            }
            catch (SqlException sqlEx)
            {
                // Manejo específico de excepciones de SQL
                throw new Exception($"Error en la consulta SQL: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Manejo general de excepciones
                throw new Exception($"Error al ejecutar la consulta: {ex.Message}", ex);
            }
        }*/
    }
}
