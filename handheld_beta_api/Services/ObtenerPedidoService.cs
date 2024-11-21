using handheld_beta_api.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

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
            // Ejecutar el procedimiento almacenado
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
