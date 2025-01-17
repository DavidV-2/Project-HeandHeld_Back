using handheld_beta_api.Model;
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

        public void ConfigurarConexionBD(IServiceCollection services) // Configuración de BD para manejo dinámico
        {

            // Configuración de ValidarTiketContext
            services.AddScoped<ValidarTiketContext>(provider =>
            {

                var config = provider.GetRequiredService<IConfiguration>();


                var optionsBuilder = new DbContextOptionsBuilder<ValidarTiketContext>();

                string defaultConnection = config.GetConnectionString("DBConnectionJJVPRGPRODUCCION");
                optionsBuilder.UseSqlServer(defaultConnection);

                return new ValidarTiketContext(optionsBuilder.Options);
            });

            // Configuración de PermisosTrasladoContext
            services.AddScoped<PermisosTrasladoContext>(provider =>
            {

                var config = provider.GetRequiredService<IConfiguration>();


                var optionsBuilder = new DbContextOptionsBuilder<PermisosTrasladoContext>();

                string defaultConnection = config.GetConnectionString("DBConnectionJJVPRGPRODUCCION");
                optionsBuilder.UseSqlServer(defaultConnection);

                return new PermisosTrasladoContext(optionsBuilder.Options);
            });
            
            // Configuración de ObtenerPedidoContext para manejo dinámico
            services.AddScoped<ObtenerPedidoContext>(provider =>
            {
                var config = provider.GetRequiredService<IConfiguration>();

                
                var optionsBuilder = new DbContextOptionsBuilder<ObtenerPedidoContext>();

                
                string defaultConnection = config.GetConnectionString("DBConnectionJJVPRGPRODUCCION");
                optionsBuilder.UseSqlServer(defaultConnection);

                return new ObtenerPedidoContext(optionsBuilder.Options);
            });

            
            services.AddScoped<UsuarioContext>(provider =>
            {
                
                var config = provider.GetRequiredService<IConfiguration>();

                
                var optionsBuilder = new DbContextOptionsBuilder<UsuarioContext>();

                
                string defaultConnection = config.GetConnectionString("DBConnectionJJVPRGPRODUCCION");
                optionsBuilder.UseSqlServer(defaultConnection);

                return new UsuarioContext(optionsBuilder.Options);
            });

        }
    }

}