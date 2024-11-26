using Azure.Messaging;
using handheld_beta_api.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace handheld_beta_api.Services
{
    public class PermisosTrasladoService
    {
        private readonly PermisosTrasladoContext _PTContext;

        public PermisosTrasladoService(PermisosTrasladoContext context)
        {
            _PTContext = context;
        }

        public async Task<PermisosTraslado> GetPermisosTrasladosAsync(decimal nit)
        {
            try
            {
                var devpermiso = new SqlParameter("@nit", SqlDbType.Decimal) { Value = nit };

                var permiso = await _PTContext.PermisosTraslado
                    .FromSqlRaw("EXEC DV_Sp_permisos_traslado @nit ", devpermiso)
                    .FirstOrDefaultAsync();

                if (permiso == null)
                {
                    throw new KeyNotFoundException("No se encontraron Permisos para traslado Traslados.");
                }

                return permiso;
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
