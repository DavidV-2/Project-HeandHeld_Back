using handheld_beta_api.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace handheld_beta_api.Services
{
    public class ObtenerPedidoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ObtenerPedidoService> _logger;

        public ObtenerPedidoService(IConfiguration configuration, ILogger<ObtenerPedidoService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<ObtenerPedido>> GetObtenerPedidosAsync(string devolver, int referencia)
        {
            try
            {
                // Determinar el nombre de la conexión basado en la referencia
                string connectionName = referencia switch
                {
                    1 => "DBConnectionJJVPRGPRODUCCION",
                    2 => "DBConnectionPRGPRODUCCION",
                    _ => throw new ArgumentException("Referencia no válida")
                };

                // Obtener la cadena de conexión
                string connectionString = _configuration.GetConnectionString(connectionName);

                // Log para saber qué base de datos está en uso
                _logger.LogInformation("Conexión establecida con la base de datos: {ConnectionName}", connectionName);

                var optionsBuilder = new DbContextOptionsBuilder<ObtenerPedidoContext>();
                optionsBuilder.UseSqlServer(connectionString);

                using var context = new ObtenerPedidoContext(optionsBuilder.Options);

                // Crear parámetros para el procedimiento almacenado
                var referenciaParam = new SqlParameter("@referencia", SqlDbType.Int) { Value = referencia };
                var devolverParam = new SqlParameter("@devolver", SqlDbType.Char, 1) { Value = devolver };

                // Log para registrar los parámetros enviados
                _logger.LogDebug("Ejecutando procedimiento almacenado con parámetros: @referencia={Referencia}, @devolver={Devolver}", referencia, devolver);


                var resultados = await context.ObtenerPedido
                    .FromSqlRaw("EXEC And_Sp_ObtenerPedidos @referencia, @devolver", referenciaParam, devolverParam)
                    .ToListAsync();

                return resultados;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Error al ejecutar el procedimiento almacenado en la base de datos: {ConnectionName}");
                throw new Exception("Error al ejecutar el procedimiento almacenado.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general al obtener los pedidos.");
                throw new Exception("Error al obtener los pedidos.", ex);
            }
        }
    }
}