using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaInventarioModelos.Especificiaciones
{
    public class Parametros
    {
        public int PageNumber { get; set; } = 1;//NUMERO DE PAGINA QUE INICIE SIEMPRE POR UNO
        public int PageSize { get; set; } = 4; //TAMANO DE PAGINA , OSEA 4 REGISTROS POR PAGINA
    }
}
