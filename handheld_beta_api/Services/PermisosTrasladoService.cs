using Azure.Messaging;
using handheld_beta_api.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace handheld_beta_api.Services
{
    public class PermisosTrasladoService
    {
        //private readonly PermisosTrasladoContext _PTContext;
        private readonly IConfiguration _configuration;

        public PermisosTrasladoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<PermisosTraslado> GetPermisosTrasladosAsync(int nit, int referencia)
        {
            try
            {
                string connectionName = referencia switch
                {
                    1 => "DBConnectionJJVPRGPRODUCCION",
                    2 => "DBConnectionPRGPRODUCCION",
                    _ => throw new ArgumentException("Referencia no válida")
                };

                string connectionString = _configuration.GetConnectionString(connectionName);

                var optionsBuilder = new DbContextOptionsBuilder<PermisosTrasladoContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using var context = new PermisosTrasladoContext(optionsBuilder.Options);

                var devnit = new SqlParameter("@nit", SqlDbType.VarChar) { Value = nit };


                var permiso = await context.PermisosTraslado
                    .FromSqlRaw("EXEC And_Sp_PermisosTraslado @nit ", devnit)
                    .ToListAsync();

                // Verifica si la lista de permisos está vacía
                if (permiso == null || !permiso.Any())
                {
                    return null;
                }

                return permiso.First();
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
