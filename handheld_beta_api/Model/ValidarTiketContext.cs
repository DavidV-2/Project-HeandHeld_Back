using Microsoft.EntityFrameworkCore;
using handheld_beta_api.Model;

namespace handheld_beta_api.Model
{
    public class ValidarTiketContext : DbContext
    {
        public ValidarTiketContext(DbContextOptions<ValidarTiketContext> options) : base(options) { }
        public DbSet<ValidarTiket> ValidarTiket { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar 'ValidarTiket' como una entidad sin clave
            modelBuilder.Entity<ValidarTiket>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }

}
