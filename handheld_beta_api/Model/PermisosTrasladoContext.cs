using Microsoft.EntityFrameworkCore;

namespace handheld_beta_api.Model
{
    public class PermisosTrasladoContext : DbContext
    {   
        public PermisosTrasladoContext(DbContextOptions<PermisosTrasladoContext> options) : base(options) { }
        public DbSet<PermisosTraslado> PermisosTraslado { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar 'PermisosTraslado' como una entidad sin clave
            modelBuilder.Entity<PermisosTraslado>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
    
}
