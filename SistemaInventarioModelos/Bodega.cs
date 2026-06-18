using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SistemaInventarioModelos
{
    public class Bodega
    {
        
        //Estas serian las columnas en nuestras tablas, para que se cree a nuestra base de datos,
        //Debemos agregarlo como un DBSet , en nuestro archivo de DbContext, de la clase AccesoDatos
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Nombre es Requerido")]
        [MaxLength(60, ErrorMessage ="Nombre debe ser Maximon 60 Caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Descripcion es Requerido")]
        [MaxLength(60, ErrorMessage = "Descripcion debe ser Maximon 100 Caracteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Estado es Requerido")]
        public bool Estado { get; set; }




    }
}
