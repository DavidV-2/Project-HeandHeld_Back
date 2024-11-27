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

        public async Task<PermisosTraslado> GetPermisosTrasladosAsync(int nit)
        {
            try
            {
                var devnit = new SqlParameter("@nit", SqlDbType.VarChar) { Value = nit };

                var permiso = await _PTContext.PermisosTraslado
                    .FromSqlRaw("EXEC DVp_Sp_permisos_traslado @nit ", devnit)
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
