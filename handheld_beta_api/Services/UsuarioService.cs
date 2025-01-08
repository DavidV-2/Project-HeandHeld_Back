using Azure.Messaging;
using handheld_beta_api.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace handheld_beta_api.Services
{
    public class UsuarioService
    {
        private readonly IConfiguration _configuration;

        public UsuarioService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Usuario>> GetUsuariosAsync(decimal nit, int referencia)
        {
            try
            {
                // Selección dinámica de cadena de conexión
                string connectionName = referencia switch
                {
                    1 => "DBConnectionJJVDMSCIERREAGOSTO",
                    2 => "DBConnectionCorsan",
                    _ => throw new ArgumentException("Referencia no válida")
                };

                string connectionString = _configuration.GetConnectionString(connectionName);

                // Crear dinámicamente el contexto con la cadena seleccionada
                var optionsBuilder = new DbContextOptionsBuilder<UsuarioContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using var context = new UsuarioContext(optionsBuilder.Options);

                var devnit = new SqlParameter("@nit", SqlDbType.Decimal) { Value = nit };

                var usuarios = await context.Usuario
                    .FromSqlRaw("EXEC DV_SPpersonal_Activo @nit", devnit)
                    .ToListAsync();

                if (usuarios == null || usuarios.Count == 0)
                {
                    throw new KeyNotFoundException("No se encontraron usuarios.");
                }

                return usuarios;
            }
            catch (Exception ex) when (ex is SqlException || ex is KeyNotFoundException)
            {
                throw new Exception($"Error en la consulta: {ex.Message}", ex);
            }
        }
    }
    /*public class UsuarioService
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
                    .FromSqlRaw("EXEC DV_SPpersonal_Activo @nit ", devnit)
                    .ToListAsync();

                if (usuarios == null || usuarios.Count == 0)
                {
                    throw new KeyNotFoundException("No se encontraron usuarios.");
                }

                return usuarios;
            }
            catch (Exception ex) when (ex is SqlException || ex is KeyNotFoundException)
            {
                throw new Exception($"Error en la consulta: {ex.Message}", ex);
            }
        }
    }*/
}