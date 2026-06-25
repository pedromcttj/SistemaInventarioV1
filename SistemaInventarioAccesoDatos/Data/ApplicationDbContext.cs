using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioModelos;
using System.Reflection;

namespace SistemaInventarioV1.AccesoDatos.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        //Se crea el DbSet , con el nombre de nuestro modelo <> seguido con el Nombre que aparezca en la BD
        public DbSet<Bodega> Bodegas { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Marca> Marcas { get; set; }

        public DbSet<Producto> Productos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}
