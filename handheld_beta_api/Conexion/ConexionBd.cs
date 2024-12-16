using handheld_beta_api.Model;
using handheld_beta_api.Services;
using Microsoft.EntityFrameworkCore;

namespace handheld_beta_api.Conexion
{
    public class ConexionBD
    {
        private readonly IConfiguration _configuration;

        public ConexionBD(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigurarConexionBD(IServiceCollection services)
        {

            // Configuración de ValidarTiketContext
            services.AddDbContext<ValidarTiketContext>(opt =>
                opt.UseSqlServer(_configuration.GetConnectionString("DBConnectionJJVPRGPRODUCCION"))
            );

            // Configuración de PermisosTrasladoContext
            services.AddDbContext<PermisosTrasladoContext>(opt =>
                opt.UseSqlServer(_configuration.GetConnectionString("DBConnectionJJVPRGPRODUCCION"))
            );

            // Configuración de ObtenerPedidoContext
            services.AddDbContext<ObtenerPedidoContext>(opt =>
                opt.UseSqlServer(_configuration.GetConnectionString("DBConnectionJJVPRGPRODUCCION"))
            );

            // Configuración de UsuarioContext
            services.AddDbContext<UsuarioContext>(opt =>
                opt.UseSqlServer(_configuration.GetConnectionString("DBConnectionCorsan"))
            );
        }
    }

}
