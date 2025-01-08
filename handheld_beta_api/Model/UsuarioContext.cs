using Microsoft.EntityFrameworkCore;
using handheld_beta_api.Model;
using System.Reflection.Emit;

namespace handheld_beta_api.Model
{
    public class UsuarioContext : DbContext
     {
        public UsuarioContext(DbContextOptions<UsuarioContext> options)
        : base(options) { }

        public UsuarioContext(string connectionString)
            : base(new DbContextOptionsBuilder<UsuarioContext>().UseSqlServer(connectionString).Options) { }

        public DbSet<Usuario> Usuario { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar 'Usuario' como una entidad sin clave
            modelBuilder.Entity<Usuario>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
        /* public UsuarioContext(DbContextOptions<UsuarioContext> options) : base(options) { }
         public DbSet<Usuario> Usuario { get; set; }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             // Configurar 'Usuario' como una entidad sin clave
             modelBuilder.Entity<Usuario>().HasNoKey();
             base.OnModelCreating(modelBuilder);
         }*/
    }
     
}
