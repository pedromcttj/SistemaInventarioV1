using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace SistemaInventarioModelos.Especificiaciones
{
    public class MetaData
    {
        public int TotalPages { get; set; } //Total de paginas de toda la vista
        public int PageSize { get; set; } //Tamaño de pagina 
        public int TotalCount { get; set; }//Total de registros

    }
}
