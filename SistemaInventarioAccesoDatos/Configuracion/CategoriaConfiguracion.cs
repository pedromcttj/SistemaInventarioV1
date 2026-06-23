using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventarioModelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaInventarioAccesoDatos.Configuracion
{
    public class CategoriaConfiguracion : IEntityTypeConfiguration<Categoria> //Herada de IEntityTypeConfiguration<del modelo Bodega>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder) {
            //Se dan de alta las columnas de la BD, se dan de alta como propiedades. 
            builder.Property(x => x.Id).IsRequired(); 
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Estado).IsRequired();
        }
    }
}
