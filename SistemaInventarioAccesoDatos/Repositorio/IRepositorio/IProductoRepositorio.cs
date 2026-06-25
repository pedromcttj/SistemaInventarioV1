using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaInventarioModelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaInventarioAccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<Producto>
    {
        void Actualizar(Producto producto); // Va recibir categoria y va actualizar al objeto categoria 

        IEnumerable<SelectListItem> ObtenerTodosDropDownLista(string obj);
    }
}
