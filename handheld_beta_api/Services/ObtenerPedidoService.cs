using handheld_beta_api.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace handheld_beta_api.Services
{
    public class ObtenerPedidoService
    {
        private readonly ObtenerPedidoContext _context;

        public ObtenerPedidoService(ObtenerPedidoContext context)
        {
            _context = context;
        }

        public async Task<List<ObtenerPedido>> GetObtenerPedidosAsync(string devolver)
        {
            try
            {
                var devolverParam = new SqlParameter("@devolver", SqlDbType.Char) { Value = devolver};

                return await _context.ObtenerPedido
                    .FromSqlRaw("EXEC DV_ObtenerPedidos @devolver", devolverParam)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pedidos", ex);
            }
        }
    }
}
