using handheld_beta_api.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace handheld_beta_api.Services
{
    public class ValidarTiketService
    {
        private readonly IConfiguration _configuration;

        public ValidarTiketService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ValidarTiket>> GetValidarTiketsAsync(string tiket, int referencia)
        {
            try
            {
                string connectionName = referencia switch
                {
                    1 => "DBConnectionJJVPRGPRODUCCION",
                    2 => "DBConnectionPRGPRODUCCION",
                    _ => throw new ArgumentException("Referencia no válida")
                };
                // Obtener la cadena de conexión
                string connectionString = _configuration.GetConnectionString(connectionName);

                
                var optionsBuilder = new DbContextOptionsBuilder<ValidarTiketContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using var context = new ValidarTiketContext(optionsBuilder.Options);

                var referenciaParam = new SqlParameter("@referencia", SqlDbType.Int) { Value = referencia };
                var tiketParam = new SqlParameter("@Tiket", SqlDbType.NVarChar) { Value = tiket };

               var resultados = await context.ValidarTiket
                    .FromSqlRaw("EXEC And_SP_ValidarTikets @referencia,@Tiket", referenciaParam, tiketParam)
                    .ToListAsync();

                return resultados;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pedidos", ex);
            }
        }
    }

}