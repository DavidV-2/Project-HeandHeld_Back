using Microsoft.EntityFrameworkCore;

namespace handheld_beta_api.Model
{
    public class ObtenerPedidoContext : DbContext
    {   
        public ObtenerPedidoContext(DbContextOptions<ObtenerPedidoContext> options) : base(options) { }
        public DbSet<ObtenerPedido> ObtenerPedido { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar 'ObtenerPedido' como una entidad sin clave
            modelBuilder.Entity<ObtenerPedido>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
    
}
