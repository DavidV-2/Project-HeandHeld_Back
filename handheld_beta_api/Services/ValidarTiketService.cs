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
        private readonly ValidarTiketContext _context;

        public ValidarTiketService(ValidarTiketContext context)
        {
            _context = context;
        }

        public async Task<List<ValidarTiket>> GetValidarTiketsAsync(string tiket)
        {
            try
            {
                var tiketParam = new SqlParameter("@Tiket", SqlDbType.NVarChar) { Value = tiket };

                return await _context.ValidarTiket
                    .FromSqlRaw("EXEC DV_ValidarTikets @Tiket", tiketParam)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los pedidos", ex);
            }
        }
    }

}