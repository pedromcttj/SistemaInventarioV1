using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SistemaInventarioModelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Numero de Serie es Requerido")]
        [MaxLength(60)]
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerido")]
        [MaxLength(60)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Precio es Requerido")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "Costo es Requerido")]
        public double Costo { get; set; }

        public string ImageUrl { get; set; }


        [Required(ErrorMessage = "Estado es Requerido")]
        public bool Estado { get; set; }

        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]

        public Categoria Categoria { get; set; }

        public int MarcaId { get; set; }

        [ForeignKey("MarcaId")]
        public Marca Marca { get; set; }

        public int? PadreId { get; set; }

        public virtual Producto Padre { get; set; }



    }
}
