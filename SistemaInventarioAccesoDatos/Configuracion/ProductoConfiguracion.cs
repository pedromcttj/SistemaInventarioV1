using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventarioModelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaInventarioAccesoDatos.Configuracion
{
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto> //Herada de IEntityTypeConfiguration<del modelo Bodega>
    {
        public void Configure(EntityTypeBuilder<Producto> builder) {
            //Se dan de alta las columnas de la BD, se dan de alta como propiedades. 
            builder.Property(x => x.Id).IsRequired(); 
            builder.Property(x => x.NumeroSerie).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.Precio).IsRequired();
            builder.Property(x => x.Costo).IsRequired();
            builder.Property(x => x.CategoriaId).IsRequired();
            builder.Property(x => x.MarcaId).IsRequired();
            builder.Property(x => x.ImageUrl).IsRequired(false);
            builder.Property(x => x.PadreId).IsRequired(false);

            /** Relaciones**/
            //De uno a muchos es HasOne().WithMany()
            //.WithMany
            builder.HasOne(x => x.Categoria).WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.NoAction); //PARA QUE NO AFECTE BORRADO EN CASCADA

            builder.HasOne(x => x.Marca).WithMany()
                .HasForeignKey(x => x.MarcaId)
                .OnDelete(DeleteBehavior.NoAction);//PARA QUE NO AFECTE BORRADO EN CASCADA


            builder.HasOne(x => x.Padre).WithMany()
            .HasForeignKey(x => x.PadreId)
            .OnDelete(DeleteBehavior.NoAction);//PARA QUE NO AFECTE BORRADO EN CASCADA
        }
    }
}
