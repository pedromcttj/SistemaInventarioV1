using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaInventarioModelos;
using System.Reflection;
using SistemaInventarioAccesoDatos.Data; //se agrego para el tema dek Applicationuser

namespace SistemaInventarioV1.AccesoDatos.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
   : IdentityDbContext<ApplicationUser>(options)

    // CAMBIO AQUÍ: Cambia ApplicationUser por UsuarioAplicacion
    //public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    //: IdentityDbContext<UsuarioAplicacion>(options)
    //ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        //Se crea el DbSet , con el nombre de nuestro modelo <> seguido con el Nombre que aparezca en la BD
        public DbSet<Bodega> Bodegas { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Marca> Marcas { get; set; }

        public DbSet<Producto> Productos { get; set; }

        // COMENTE ESTA LÍNEA (IdentityDbContext ya se encarga de los usuarios)
        public DbSet<UsuarioAplicacion> UsuarioAplicacion { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}
